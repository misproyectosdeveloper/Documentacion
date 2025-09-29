Imports System.Transactions
Public Class UnRemitoReemplazo
    Public PRemito As Decimal
    Public PAbierto As Boolean
    Public PEsReemplazo As Boolean
    Public PEsRecupera As Boolean
    '
    Dim DtRemitoCabeza As DataTable
    Dim DtRemitoDetalle As DataTable
    Dim DtAsientoCabeza As DataTable
    Dim DtAsientoDetalle As New DataTable
    Dim DtIngresoMercaderiasCabeza As DataTable
    Dim DtAsignacionLotes As New DataTable
    '
    Dim DtRemitoCabezaAux As DataTable
    Dim DtRemitoDetalleAux As DataTable
    Dim DtAsignacionLotesAux As DataTable
    Dim DtAsientoCabezaAux As DataTable
    Dim DtAsientoDetalleAux As DataTable
    '
    Dim ConexionRemito As String
    Dim ConexionRemitonew As String
    Private Sub UnCambioOperacionRemito_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GModificacionOk = False

        If PAbierto Then
            ConexionRemito = Conexion
            ConexionRemitonew = ConexionN
        Else
            ConexionRemito = ConexionN
            ConexionRemitonew = Conexion
        End If

        If PEsReemplazo Then
            Label1.Text = "El Remito " & NumeroEditado(PRemito) & " se ANULARA, y se Reemplazara"
            Label2.Text = "por otro con distinto Numero y a Candado Cerrado."
        End If

        If PEsRecupera Then
            Label1.Text = "El Remito " & NumeroEditado(PRemito) & " ANULADO pasara al Estado ACTIVO"
            Label2.Text = "(Se recupera sin sus Devoluciones. Debe informarlas nuevamente)"
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PEsReemplazo Then
            ProcesaReemplazo()
        End If

        If PEsRecupera Then
            ProcesoRecupera()
        End If

        ButtonAceptar.Visible = False

    End Sub
    Private Function ProcesaReemplazo() As Boolean

        If Not PAbierto Then
            MsgBox("Remito NO HABILITADO para esta Función. Operación se CANCELA.", MsgBoxStyle.Critical)
            Exit Function
        End If

        'Lee Remito.
        Dim DtRemitoCabeza As DataTable
        Dim DtRemitoDetalle As DataTable
        Dim DtAsignacionLotes As DataTable
        Dim DtAsientoCabeza As DataTable
        Dim DtAsientoDetalle As DataTable

        If Not HallaTablasRemito(PRemito, PAbierto, DtRemitoCabeza, DtRemitoDetalle, DtAsignacionLotes, DtAsientoCabeza, DtAsientoDetalle) Then
            MsgBox("Error al leer Tablas del Remito. Operación se CANCELA.", MsgBoxStyle.Critical)
            Exit Function
        End If
        If DtRemitoCabeza.Rows(0).Item("Factura") <> 0 Then
            MsgBox("Remito esta Facturado. Operación se CANCELA.")
            Exit Function
        End If
        If DtRemitoCabeza.Rows(0).Item("Estado") = 3 Then
            MsgBox("Remito esta dado de Baja. Operación se CANCELA.")
            Exit Function
        End If
        For Each Row As DataRow In DtAsignacionLotes.Rows
            If Row("Liquidado") Then
                MsgBox("Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000") & " del Remito Tiene NVLP. Operación se CANCELA.", MsgBoxStyle.Critical)
                Exit Function
            End If
        Next

        'Lee Devoluciones.
        Dim DtDevolucionCabeza As DataTable

        If Not HallaTablasDevoluciones(PRemito, PAbierto, DtDevolucionCabeza) Then
            MsgBox("Error al leer Tablas de Devolucion Remito. Operación se CANCELA.", MsgBoxStyle.Critical)
            Exit Function
        End If

        Dim DtDevolucionDetalle As New DataTable
        Dim DtDevolucionAsignacionLotes As New DataTable
        Dim Sql As String

        For Each Row As DataRow In DtDevolucionCabeza.Rows
            Sql = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 3 AND Comprobante = " & Row("Devolucion") & ";"
            If Not Tablas.Read(Sql, ConexionRemito, DtDevolucionAsignacionLotes) Then Exit Function
            Sql = "SELECT * FROM DevolucionDetalle WHERE Devolucion = " & Row("Devolucion") & ";"
            If Not Tablas.Read(Sql, ConexionRemito, DtDevolucionDetalle) Then Exit Function
        Next

        'Copia al remito nuevo.
        Dim DtRemitoCabezaNew As DataTable
        DtRemitoCabezaNew = DtRemitoCabeza.Clone
        CopiaTabla(DtRemitoCabeza, DtRemitoCabezaNew)
        '
        Dim DtRemitoDetalleNew As DataTable
        DtRemitoDetalleNew = DtRemitoDetalle.Clone
        CopiaTabla(DtRemitoDetalle, DtRemitoDetalleNew)
        '
        Dim DtAsignacionLotesNew As DataTable
        DtAsignacionLotesNew = DtAsignacionLotes.Clone
        CopiaTabla(DtAsignacionLotes, DtAsignacionLotesNew)
        '
        Dim DtAsientoCabezaNew As DataTable
        DtAsientoCabezaNew = DtAsientoCabeza.Clone
        CopiaTabla(DtAsientoCabeza, DtAsientoCabezaNew)
        '
        Dim DtAsientoDetalleNew As DataTable
        DtAsientoDetalleNew = DtAsientoDetalle.Clone
        CopiaTabla(DtAsientoDetalle, DtAsientoDetalleNew)

        'Actualizo detalle del remito nuevo, en cantidad queda el neto (Sin devoluciones).
        For Each Row As DataRow In DtRemitoDetalleNew.Rows
            Row("Cantidad") = Row("Cantidad") - Row("Devueltas")
            Row("Devueltas") = 0
        Next
        'Actualizo remito cabeza nuevo.
        DtRemitoCabezaNew.Rows(0).Item("RemitoReemplazado") = PRemito
        DtRemitoCabezaNew.Rows(0).Item("Ingreso") = 0

        'Actualiza Remitos Origen.
        'Borra las devoluciones en detalle del remito.   Pues despues del reemplazo el remito origen queda como el original y des-loteado.
        For Each Row As DataRow In DtRemitoDetalle.Rows
            Row("Devueltas") = 0
        Next

        'Re-calcular Valor del remito origen.
        Dim Total As Decimal = 0
        For Each Row As DataRow In DtRemitoDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Total = Total + CalculaNeto(Row("Cantidad") - Row("Devueltas"), Row("Precio"))
            End If
        Next
        'Actualiza Cabeza remito origen.
        DtRemitoCabeza.Rows(0).Item("Valor") = Total
        DtRemitoCabeza.Rows(0).Item("Estado") = 3

        'Borra asignacion de lotes origen.
        For Each Row As DataRow In DtAsignacionLotes.Rows
            Row.Delete()
        Next

        'Re-calcula Asiento detalle origen.
        DtAsientoDetalle = New DataTable
        If DtAsientoCabeza.Rows.Count <> 0 Then
            If Not CalculaYReemplazaDetalleRemito(PRemito, DtAsignacionLotes, DtRemitoDetalle, DtAsientoDetalle, ConexionRemito) Then Exit Function
            'Anula asiento Remito origen.
            DtAsientoCabeza.Rows(0).Item("Estado") = 3
        End If

        'Actualiza Devoluciiones origen.
        'Borra asignacion de lotes.
        For Each Row As DataRow In DtDevolucionAsignacionLotes.Rows
            Row.Delete()
        Next
        'Anula devoluciones.
        For Each Row As DataRow In DtDevolucionCabeza.Rows
            Row("Estado") = 3
        Next

        GPuntoDeVenta = HallaPuntoVentaSegunTipo(1, 0)
        If GPuntoDeVenta = 0 Then
            MsgBox("Usuario No tiene definido Punto de Venta para Remito.")
            Exit Function
        End If
        If Not ExistePuntoDeVenta(GPuntoDeVenta) Then
            MsgBox("Punto de Venta " & GPuntoDeVenta & " No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If
        If EsPuntoDeVentaZ(GPuntoDeVenta) Then
            MsgBox("Punto de Venta del Operador " & GPuntoDeVenta & " Reservado Para Ticket Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        'Graba Remitos.
        Dim NumeroRemito As Double = 0
        Dim NumeroW As Double
        Dim NumeroAsiento As Double

        For i As Integer = 1 To 50
            NumeroRemito = UltimaNumeracion(ConexionRemitonew)
            If NumeroRemito < 0 Then
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            DtRemitoCabezaNew.Rows(0).Item("Remito") = NumeroRemito
            DtRemitoCabezaNew.Rows(0).Item("Interno") = UltimoNumeroInternoRemito(ConexionRemitonew)
            If DtRemitoCabezaNew.Rows(0).Item("Interno") < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            For Each Row As DataRow In DtRemitoDetalleNew.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Remito") = NumeroRemito
                End If
            Next

            'Halla Ultima numeracion Asiento B
            If DtAsientoCabezaNew.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionRemitonew)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                DtAsientoCabezaNew.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabezaNew.Rows(0).Item("Documento") = NumeroRemito
                For Each Row As DataRow In DtAsientoDetalleNew.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            If Not IsNothing(DtAsignacionLotesNew) Then
                For Each Row As DataRow In DtAsignacionLotesNew.Rows
                    Row("Comprobante") = NumeroRemito
                Next
            End If

            NumeroW = ActualizaRemito(DtRemitoCabeza, DtRemitoDetalle, DtAsignacionLotes, DtAsientoCabeza, DtAsientoDetalle, DtDevolucionCabeza, DtDevolucionAsignacionLotes, DtRemitoCabezaNew, DtRemitoDetalleNew, DtAsignacionLotesNew, DtAsientoCabezaNew, DtAsientoDetalleNew)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -2 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW > 0 Then
            MsgBox("Reemplazo Remito Realizado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
        End If

        Label1.Text = "Remito " & NumeroEditado(PRemito) & " fue dado de BAJA y reemplazado"
        Label2.Text = "por el Remito " & NumeroEditado(NumeroRemito) & " (Candado Cerrado)."

        Return True

    End Function
    Private Function ProcesoRecupera() As Boolean

        Dim DtRemitoCabeza As DataTable
        Dim DtRemitoDetalle As DataTable
        Dim DtAsignacionLotes As DataTable
        Dim DtAsientoCabeza As DataTable
        Dim DtAsientoDetalle As DataTable

        If Not HallaTablasRemito(PRemito, PAbierto, DtRemitoCabeza, DtRemitoDetalle, DtAsignacionLotes, DtAsientoCabeza, DtAsientoDetalle) Then
            MsgBox("Error al leer Tablas del Remito. Operación se CANCELA.", MsgBoxStyle.Critical)
            Exit Function
        End If
        If DtRemitoCabeza.Rows(0).Item("Estado") <> 3 Then
            MsgBox("Remito no esta dado de Baja. Operación se CANCELA.")
            Exit Function
        End If

        Dim DtDetallePedido As New DataTable
        If DtRemitoCabeza.Rows(0).Item("Pedido") <> 0 Then
            If Not ActualizaDetallePedido("A", "R", DtRemitoCabeza.Rows(0).Item("Pedido"), DtRemitoDetalle, DtDetallePedido) Then Exit Function
        End If

        Dim ConexionStr As String
        If PAbierto Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        'Reemplaza RemitoCliente en Ingreso de Mercaderia.
        Dim DtIngresoMercaderiasCabeza As New DataTable
        If DtRemitoCabeza.Rows(0).Item("Ingreso") <> 0 Then
            If Not Tablas.Read("SELECT * FROM IngresoMercaderiasCabeza Where Lote = " & DtRemitoCabeza.Rows(0).Item("Ingreso") & ";", ConexionStr, DtIngresoMercaderiasCabeza) Then Exit Function
            If DtIngresoMercaderiasCabeza.Rows.Count <> 0 Then
                If DtIngresoMercaderiasCabeza.Rows(0).Item("RemitoCliente") = 0 Then
                    DtIngresoMercaderiasCabeza.Rows(0).Item("RemitoCliente") = PRemito
                End If
            End If
        End If

        DtRemitoCabeza.Rows(0).Item("Estado") = 2
        If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 1

        Dim NumeroW As Integer

        NumeroW = GrabaRecuperacion(DtRemitoCabeza, DtAsientoCabeza, DtIngresoMercaderiasCabeza, DtDetallePedido, ConexionStr)

        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW > 0 Then
            MsgBox("Restauración Remito Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
        End If

        Label1.Text = "Remito " & NumeroEditado(PRemito) & " fue Restaurado a ACTIVO"
        Label2.Text = "Deberá Asignar Lotes Nuevamente."

        Return True

    End Function
    Private Function HallaTablasRemito(ByVal Remito As Decimal, ByVal Abierto As Boolean, ByRef DtRemitoCabeza As DataTable, ByRef DtRemitoDetalle As DataTable, ByRef DtAsignacionLotes As DataTable, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable) As Boolean

        DtRemitoCabeza = New DataTable
        Dim Sql As String = "SELECT * FROM RemitosCabeza WHERE Remito = " & Remito & ";"
        If Not Tablas.Read(Sql, ConexionRemito, DtRemitoCabeza) Then Exit Function

        DtRemitoDetalle = New DataTable
        Sql = "SELECT * FROM RemitosDetalle WHERE Remito = " & Remito & ";"
        If Not Tablas.Read(Sql, ConexionRemito, DtRemitoDetalle) Then Exit Function

        DtAsignacionLotes = New DataTable
        Sql = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 1 AND Comprobante = " & Remito & ";"
        If Not Tablas.Read(Sql, ConexionRemito, DtAsignacionLotes) Then Exit Function

        DtAsientoCabeza = New DataTable
        DtAsientoDetalle = New DataTable

        If Not HallaAsientosCabeza(6060, Remito, DtAsientoCabeza, ConexionRemito) Then Exit Function
        If DtAsientoCabeza.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabeza.Rows(0).Item("Asiento") & ";", ConexionRemito, DtAsientoDetalle) Then Exit Function
        End If

        Return True

    End Function
    Private Function HallaTablasDevoluciones(ByVal Remito As Decimal, ByVal Abierto As Boolean, ByRef DtDevolucionesCabezaAux As DataTable) As Boolean

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        DtDevolucionesCabezaAux = New DataTable
        Dim Sql As String = "SELECT * FROM DevolucionCabeza WHERE Remito = " & Remito & " AND Estado <> 3;"
        If Not Tablas.Read(Sql, ConexionStr, DtDevolucionesCabezaAux) Then Exit Function

        Return True

    End Function
    Private Function CalculaYReemplazaDetalleRemito(ByVal Remito As Decimal, ByVal DtLotesAsignadosAux As DataTable, ByVal DtDetalleRemitoAux As DataTable, ByRef DtAsientoDetalle As DataTable, ByVal ConexionStr As String) As Boolean

        DtAsientoDetalle = New DataTable

        Dim NroAsiento As Integer = 0

        Dim DtAsientoCabeza As New DataTable
        If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = 6060 AND Documento = " & Remito & ";", ConexionStr, DtAsientoCabeza) Then Return False
        If DtAsientoCabeza.Rows.Count <> 0 Then
            NroAsiento = DtAsientoCabeza.Rows(0).Item("Asiento")
        Else
            Return True
        End If

        If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & NroAsiento & ";", ConexionStr, DtAsientoDetalle) Then Return False

        Dim DtAsientoDetalleAux As DataTable = DtAsientoDetalle.Clone
        DtAsientoCabeza.Clear()
        If Not ArmaAsientoRemito(6060, DtLotesAsignadosAux, DtDetalleRemitoAux, DtAsientoCabeza, DtAsientoDetalleAux, "01/01/1800") Then Return False

        For Each Row As DataRow In DtAsientoDetalle.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtAsientoDetalleAux.Rows
            Row("Asiento") = NroAsiento
            Dim Row1 As DataRow = DtAsientoDetalle.NewRow
            For I As Integer = 0 To DtAsientoDetalleAux.Columns.Count - 1
                Row1.Item(I) = Row.Item(I)
            Next
            DtAsientoDetalle.Rows.Add(Row1)
        Next

        Return True

    End Function
    Private Function ActualizaRemito(ByVal DtRemitoCabeza As DataTable, ByVal DtRemitoDetalle As DataTable, ByVal DtAsignacionLotes As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtDevolucionCabeza As DataTable, ByVal DtDevolucionAsignacionLotes As DataTable, ByVal DtRemitoCabezaNew As DataTable, ByVal DtRemitoDetalleNew As DataTable, ByVal DtAsignacionLotesNew As DataTable, ByVal DtAsientoCabezaNew As DataTable, ByVal DtAsientoDetalleNew As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                'Actualiza Remito Cabeza y detalle viejo.
                If Not IsNothing(DtRemitoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtRemitoCabeza.GetChanges, "RemitosCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtRemitoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtRemitoDetalle.GetChanges, "RemitosDetalle", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza Asignacion lotes viejo.
                If Not IsNothing(DtAsignacionLotes.GetChanges) Then
                    Resul = GrabaTabla(DtAsignacionLotes.GetChanges, "AsignacionLotes", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza Asiento Cabeza y detalle viejo.
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza Devolucion Cabeza y asignacion de lotes viejo.
                If Not IsNothing(DtDevolucionCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtDevolucionCabeza.GetChanges, "DevolucionCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtDevolucionAsignacionLotes.GetChanges) Then
                    Resul = GrabaTabla(DtDevolucionAsignacionLotes.GetChanges, "AsignacionLotes", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                'Graba Remito Cabeza y detalle nuevo.
                If Not IsNothing(DtRemitoCabezaNew.GetChanges) Then
                    Resul = GrabaTabla(DtRemitoCabezaNew.GetChanges, "RemitosCabeza", ConexionRemitonew)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtRemitoDetalleNew.GetChanges) Then
                    Resul = GrabaTabla(DtRemitoDetalleNew.GetChanges, "RemitosDetalle", ConexionRemitonew)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza Asignacion lotes nuevo.
                If Not IsNothing(DtAsignacionLotesNew.GetChanges) Then
                    Resul = GrabaTabla(DtAsignacionLotesNew.GetChanges, "AsignacionLotes", ConexionRemitonew)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza Asiento Cabeza y detalle nuevo.
                If Not IsNothing(DtAsientoCabezaNew.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaNew.GetChanges, "AsientosCabeza", ConexionRemitonew)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalleNew.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleNew.GetChanges, "AsientosDetalle", ConexionRemitonew)
                    If Resul <= 0 Then Return Resul
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function GrabaRecuperacion(ByVal DtRemitoCabeza As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtIngresoMercaderiasCabeza As DataTable, ByVal DtDetallePedido As DataTable, ByVal ConexionStr As String) As Integer

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                'Actualiza Remito Cabeza viejo.
                If Not IsNothing(DtRemitoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtRemitoCabeza.GetChanges, "RemitosCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza Asiento Cabeza viejo.
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza Pedido.
                If Not IsNothing(DtDetallePedido.GetChanges) Then
                    Resul = GrabaTabla(DtDetallePedido.GetChanges, "PedidosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza Ingreso Mercaderias nuevo.
                If Not IsNothing(DtIngresoMercaderiasCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtIngresoMercaderiasCabeza.GetChanges, "IngresoMercaderiasCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Dim Patron As String = GPuntoDeVenta & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Remito) FROM RemitosCabeza WHERE CAST(CAST(Remito AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(GPuntoDeVenta & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function UltimoNumeroInternoRemito(ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Interno) FROM RemitosCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function


End Class