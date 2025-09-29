Imports System.Data
Imports System.Data.OleDb
Imports System.Transactions
Imports System.Math
Public Class UnaDevolucion
    Public PRemito As Double
    Public PDevolucion As Integer
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Dim ListaDeLotes As List(Of FilaAsignacion)
    Private MiEnlazador As New BindingSource
    '
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtLotesAsignados As DataTable
    Dim DtLotesAsignadosDevolucion As DataTable
    Dim DtCabezaRemito As DataTable
    Dim DtDetalleRemito As DataTable
    Dim DtDetallePedido As DataTable
    Private DtGrid As DataTable
    '
    Dim Pedido As Integer
    Dim ConexionRemito As String
    Dim UltimaFechaW As DateTime
    Private Sub UnaDevolucion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If Not PermisoEscritura(5) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False

        LlenaCombo(ComboCliente, "", "Clientes")
        LlenaComboTablas(ComboDeposito, 19)
        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        LlenaCombosGrid()

        If PAbierto Then
            ConexionRemito = Conexion
        Else : ConexionRemito = ConexionN
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not PreparaArchivo() Then Me.Cursor = System.Windows.Forms.Cursors.Default : Me.Close() : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If PDevolucion <> 0 Then ButtonAjuste.Visible = False

        If PermisoTotal Then
            PictureCandado.Visible = True
            If PAbierto Then
                PictureCandado.Image = ImageList1.Images.Item("Abierto")
            Else : PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            End If
        Else
            PictureCandado.Visible = False
        End If

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)

        UltimaFechaW = UltimaFecha(Conexion)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PDevolucion <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid.EndEdit()

        If Not Valida() Then Exit Sub

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow

        If DtCabezaRemito.Rows(0).Item("Estado") = 3 Then
            MsgBox("Remito esta Anulado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If
        If DtCabezaRemito.Rows(0).Item("Factura") <> 0 Then
            MsgBox("Remito Ya esta Facturado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        'Ve si el lote a asignar esta liquidado.
        For Each Fila As FilaAsignacion In ListaDeLotes    'En alta contiene asignacion lotes del remito.
            RowsBusqueda = DtLotesAsignados.Select("Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & Fila.Deposito)
            If Fila.Devolucion <> 0 Then
                If RowsBusqueda(0).Item("Liquidado") Then
                    MsgBox("Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " Esta Liquidado en una N.V.L.P. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    Exit Sub
                End If
            End If
        Next

        Dim DtDetalleAux As DataTable = DtDetalle.Copy
        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        Dim DtLotesAsignadosDevolucionAux As DataTable = DtLotesAsignadosDevolucion.Copy
        Dim DtLotesAsignadosAux As DataTable = DtLotesAsignados.Copy
        Dim DtDetalleRemitoAux As DataTable = DtDetalleRemito.Copy
        Dim DtCabezaRemitoAux As DataTable = DtCabezaRemito.Copy

        'Arma detalle de devolucion.
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                If Row1("Cantidad") <> 0 Then
                    Row = DtDetalleAux.NewRow()
                    Row("Devolucion") = 0
                    Row("Indice") = Row1("Indice")
                    Row("Articulo") = Row1("Articulo")
                    Row("KilosXUnidad") = Row1("KilosXUnidad")
                    Row("Cantidad") = Row1("Cantidad")
                    DtDetalleAux.Rows.Add(Row)
                End If
            End If
        Next

        If HallaArticuloDeshabilitado(DtDetalleAux) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        'Arma AsignacionLotes remito.
        Dim EsDevolucion As Boolean
        For Each Fila As FilaAsignacion In ListaDeLotes
            RowsBusqueda = DtDetalleAux.Select("Indice = " & Fila.Indice)
            If RowsBusqueda.Length <> 0 Then
                If RowsBusqueda(0).Item("Cantidad") > 0 Then
                    EsDevolucion = True
                Else : EsDevolucion = False
                End If
                RowsBusqueda = DtLotesAsignadosAux.Select("Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & Fila.Deposito)
                If EsDevolucion Then
                    RowsBusqueda(0).Item("Cantidad") = CDec(RowsBusqueda(0).Item("Cantidad")) - Fila.Devolucion
                Else
                    RowsBusqueda(0).Item("Cantidad") = CDec(RowsBusqueda(0).Item("Cantidad")) + Fila.Devolucion  'es un aumento.
                End If
            End If
        Next

        'Actualiza devolucion en RemitoDetalle.
        For Each Row1 As DataRow In DtGrid.Rows
            RowsBusqueda = DtDetalleRemitoAux.Select("Indice = " & Row1("Indice"))
            RowsBusqueda(0).Item("Devueltas") = CDec(RowsBusqueda(0).Item("Devueltas")) + Row1("cantidad")
        Next

        'Actualiza Valor en RemitoCabeza.
        Dim Valor As Decimal = 0
        For Each Row1 As DataRow In DtDetalleRemitoAux.Rows
            Dim CantidadFinal As Decimal = Row1("Cantidad") - Row1("Devueltas")
            If Row1("TipoPrecio") = 2 Then CantidadFinal = CantidadFinal * Row1("KilosXUnidad")
            Valor = Valor + CalculaNeto(CantidadFinal, Row1("Precio"))
        Next
        DtCabezaRemitoAux.Rows(0).Item("Valor") = Valor

        'Arma AsignacionLotes de la devolucion.
        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Devolucion <> 0 Then
                Dim Row1 As DataRow = DtLotesAsignadosDevolucionAux.NewRow()
                Row1("Lote") = Fila.Lote
                Row1("Secuencia") = Fila.Secuencia
                Row1("Deposito") = Fila.Deposito
                Row1("Indice") = Fila.Indice
                Row1("TipoComprobante") = 3
                Row1("Comprobante") = 0
                Row1("Cantidad") = Fila.Devolucion
                Row1("Operacion") = Fila.Operacion
                Row1("ImporteSinIva") = 0
                Row1("Importe") = 0
                Row1("Facturado") = False
                Row1("Liquidado") = False
                Row1("Rel") = False
                DtLotesAsignadosDevolucionAux.Rows.Add(Row1)
            End If
        Next

        'Actualiza Pedido.
        Dim DtDetallePedidoAux As New DataTable
        If Pedido <> 0 Then
            If Not ActualizaDetallePedido("B", "D", Pedido, DtDetalleAux, DtDetallePedidoAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'ReHace Detalle del Aciento del Remito.
        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not ReHaceAsientoDetalleRemito(DtLotesAsignadosAux, DtDetalleRemitoAux, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        'separa listas con devoluciones y aumento.
        Dim ListaDeLotesDevolucion As New List(Of FilaAsignacion)
        Dim ListaDeLotesAumento As New List(Of FilaAsignacion)

        For Each Fila As FilaAsignacion In ListaDeLotes
            RowsBusqueda = DtDetalleAux.Select("Indice = " & Fila.Indice)
            If RowsBusqueda.Length <> 0 Then
                If RowsBusqueda(0).Item("Cantidad") > 0 Then
                    Dim Fila2 As New FilaAsignacion
                    Fila2.Operacion = Fila.Operacion
                    Fila2.Lote = Fila.Lote
                    Fila2.Secuencia = Fila.Secuencia
                    Fila2.Deposito = Fila.Deposito
                    Fila2.Devolucion = Fila.Devolucion
                    ListaDeLotesDevolucion.Add(Fila2)
                End If
                If RowsBusqueda(0).Item("Cantidad") < 0 Then
                    Dim Fila2 As New FilaAsignacion
                    Fila2.Operacion = Fila.Operacion
                    Fila2.Lote = Fila.Lote
                    Fila2.Secuencia = Fila.Secuencia
                    Fila2.Deposito = Fila.Deposito
                    Fila2.Devolucion = Fila.Devolucion
                    ListaDeLotesAumento.Add(Fila2)
                End If
            End If
        Next

        Dim NumeroDevolucion As Integer = 0
        Dim Resul As Double = 0
        Dim NumeroAsiento As Double = 0

        For i As Integer = 1 To 50
            NumeroDevolucion = UltimaNumeracion(ConexionRemito)
            If NumeroDevolucion < 0 Then
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                UnaDevolucion_Load(Nothing, Nothing)
                Exit Sub
            End If
            DtCabezaAux.Rows(0).Item("Devolucion") = NumeroDevolucion
            For Each Row In DtDetalleAux.Rows
                Row("Devolucion") = NumeroDevolucion
            Next
            If DtLotesAsignadosDevolucionAux.Rows.Count <> 0 Then
                For Each Row In DtLotesAsignadosDevolucionAux.Rows
                    Row("Comprobante") = NumeroDevolucion
                Next
            End If

            Resul = AltaDevolucion(DtCabezaAux, DtDetalleAux, DtLotesAsignadosDevolucionAux, DtAsientoDetalle, ListaDeLotesDevolucion, ListaDeLotesAumento, DtCabezaRemitoAux, DtDetalleRemitoAux, DtLotesAsignadosAux, DtDetallePedidoAux)

            If Resul >= 0 Then Exit For
            If Resul = -2 Or Resul = -3 Then Exit For
        Next

        Select Case Resul
            Case -1, -2
                MsgBox("ERROR: En Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Case -3
            Case 0
                MsgBox("ERROR: Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Case Else
                MsgBox("Devolución realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                GModificacionOk = True
                PDevolucion = NumeroDevolucion
        End Select

        Me.Cursor = System.Windows.Forms.Cursors.Default

        UnaDevolucion_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PDevolucion = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()
        Grid.EndEdit()

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Devolución Ya esta Anulada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If DtCabezaRemito.Rows(0).Item("Estado") = 3 Then
            MsgBox("Remito esta Anulado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If
        If DtCabezaRemito.Rows(0).Item("Factura") <> 0 Then
            MsgBox("Remito Ya esta Facturado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If
        For Each Row As DataRow In DtLotesAsignados.Rows
            If Row("Liquidado") = True Then
                MsgBox("Existe Lotes Ya Liquidado en una N.V.L.P Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
        Next

        If DtLotesAsignadosDevolucion.Rows.Count = 0 And DtCabezaRemito.Rows(0).Item("Estado") = 1 Then
            MsgBox("Remito Tiene Lotes Asignados. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Dim RowsBusqueda() As DataRow

        For Each Row As DataGridViewRow In Grid.Rows
            If IsNothing(Row.Cells("Articulo").Value) Then Exit For
            RowsBusqueda = DtDetalleRemito.Select("Indice = " & Row.Cells("Indice").Value)
            Dim Saldo As Decimal = CDec(RowsBusqueda(0).Item("Devueltas")) - CDec(Row.Cells("Cantidad").Value)
            If CDec(RowsBusqueda(0).Item("Cantidad")) - Saldo < 0 Then
                MsgBox("Baja Hace Negativa la Cantidad en el Remito del Articulo " & Row.Cells("Articulo").FormattedValue & ". Operación se CANCELA.", MsgBoxStyle.Information)
                Exit Sub
            End If
        Next

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If MsgBox("Devolución se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim DtLotesAsignadosDevolucionAux As DataTable = DtLotesAsignadosDevolucion.Copy
        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        Dim DtLotesAsignadosAux = DtLotesAsignados.Copy
        Dim DtDetalleRemitoAux = DtDetalleRemito.Copy
        Dim DtCabezaRemitoAux = DtCabezaRemito.Copy

        'Borra Asignacion de lotes para la devolucion.
        For Each Row As DataRow In DtLotesAsignadosDevolucionAux.Rows
            Row.Delete()
        Next

        'Actualiza Pedido.
        Dim DtDetallePedidoAux As New DataTable
        If Pedido <> 0 Then
            If Not ActualizaDetallePedido("A", "D", Pedido, DtDetalle, DtDetallePedidoAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        'Arma AsignacionLotes remito.
        ' Esta rutina restitute un lote de la devolucion que no esta en AsignacionLote del remito por que se dio el caso
        ' de tener una devolucion del lote, luego reasignar el lote a otro numero de lote en el remito y al querer anular la devolucion
        ' y no encontrar el lote anterior a la reasignacion da error.
        ' Ej.: en el remito asigne al lote 200/001 1000 Un. luego una devolucion de 500 un. Luego reasigne en el remito las 500 Un restante al lote
        ' 300/001, cuando quise anular la devolucion no encontro el lote 200/001 en el remito se produce un error en ejecucion. 
        ' Si es un aumento, se debe cumplir que la cantidad del lote a devolver, sea igual o mayor a la cantidad en la AsignacionLotes del remito. 
        Dim EsDevolucion As Boolean
        For Each Fila As FilaAsignacion In ListaDeLotes
            RowsBusqueda = DtDetalle.Select("Indice = " & Fila.Indice)
            If RowsBusqueda(0).Item("Cantidad") > 0 Then
                EsDevolucion = True
            Else : EsDevolucion = False
            End If
            RowsBusqueda = DtLotesAsignadosAux.Select("Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & Fila.Deposito)
            If Not EsDevolucion Then
                If RowsBusqueda.Length = 0 Then
                    MsgBox("Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " No se encuentra asignado al Remito. Operación se CANCELA.") : Exit Sub
                Else
                    If RowsBusqueda(0).Item("Cantidad") < Fila.Devolucion Then
                        MsgBox("Asignación Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " En Remito Debe ser igual o mayor a " & Fila.Devolucion & ". Operación se CANCELA.") : Exit Sub
                    End If
                End If
            End If
            If RowsBusqueda.Length = 0 And EsDevolucion Then
                Dim Row As DataRow = DtLotesAsignadosAux.NewRow()
                Row("Lote") = Fila.Lote
                Row("Secuencia") = Fila.Secuencia
                Row("Deposito") = Fila.Deposito
                Row("Indice") = Fila.Indice
                Row("TipoComprobante") = 1
                Row("Comprobante") = DtCabezaRemito.Rows(0).Item("Remito")
                Row("Cantidad") = Fila.Asignado
                Row("Operacion") = Fila.Operacion
                Row("ImporteSinIva") = 0
                Row("Importe") = 0
                Row("Facturado") = False
                Row("Liquidado") = False
                Row("Rel") = False
                DtLotesAsignadosAux.Rows.Add(Row)
            End If
        Next
        '
        For Each Fila As FilaAsignacion In ListaDeLotes
            RowsBusqueda = DtDetalle.Select("Indice = " & Fila.Indice)
            If RowsBusqueda(0).Item("Cantidad") > 0 Then
                EsDevolucion = True
            Else : EsDevolucion = False
            End If
            RowsBusqueda = DtLotesAsignadosAux.Select("Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & Fila.Deposito)
            If EsDevolucion Then
                RowsBusqueda(0).Item("Cantidad") = CDec(RowsBusqueda(0).Item("Cantidad")) + Fila.Devolucion
            Else
                RowsBusqueda(0).Item("Cantidad") = CDec(RowsBusqueda(0).Item("Cantidad")) - Fila.Devolucion
            End If
        Next

        'Actualiza devolucion en RemitoDetalle.
        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtDetalleRemitoAux.Select("Indice = " & Row.Item("Indice"))
            RowsBusqueda(0).Item("Devueltas") = CDec(RowsBusqueda(0).Item("Devueltas")) - Row.Item("Cantidad")
        Next

        'Actualiza Valor en RemitoCabeza.
        Dim Valor As Decimal = 0
        For Each Row1 As DataRow In DtDetalleRemitoAux.Rows
            Dim CantidadFinal As Decimal = Row1("Cantidad") - Row1("Devueltas")
            If Row1("TipoPrecio") = 2 Then CantidadFinal = CantidadFinal * Row1("KilosXUnidad")
            Valor = Valor + CalculaNeto(CantidadFinal, Row1("Precio"))
        Next
        DtCabezaRemitoAux.Rows(0).Item("Valor") = Valor

        'ReHace Detalle del Aciento del Remito.
        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not ReHaceAsientoDetalleRemito(DtLotesAsignadosAux, DtDetalleRemitoAux, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        'separa listas con devoluciones y aumento.
        Dim ListaDeLotesDevolucion As New List(Of FilaAsignacion)
        Dim ListaDeLotesAumento As New List(Of FilaAsignacion)

        For Each Fila As FilaAsignacion In ListaDeLotes
            RowsBusqueda = DtDetalle.Select("Indice = " & Fila.Indice)
            If RowsBusqueda(0).Item("Cantidad") > 0 Then
                Dim Fila2 As New FilaAsignacion
                Fila2.Operacion = Fila.Operacion
                Fila2.Lote = Fila.Lote
                Fila2.Secuencia = Fila.Secuencia
                Fila2.Deposito = Fila.Deposito
                Fila2.Devolucion = Fila.Devolucion
                ListaDeLotesDevolucion.Add(Fila2)
            End If
            If RowsBusqueda(0).Item("Cantidad") < 0 Then
                Dim Fila2 As New FilaAsignacion
                Fila2.Operacion = Fila.Operacion
                Fila2.Lote = Fila.Lote
                Fila2.Secuencia = Fila.Secuencia
                Fila2.Deposito = Fila.Deposito
                Fila2.Devolucion = Fila.Devolucion
                ListaDeLotesAumento.Add(Fila2)
            End If
        Next

        DtCabezaAux.Rows(0).Item("Estado") = 3

        If Not AnulaDevolucion(DtCabezaAux, DtLotesAsignadosDevolucionAux, DtAsientoDetalle, ListaDeLotesDevolucion, ListaDeLotesAumento, DtCabezaRemitoAux, DtDetalleRemitoAux, DtLotesAsignadosAux, DtDetallePedidoAux) Then
            MsgBox("Otro Usuario modifico datos o Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        Else
            MsgBox("Devolución fue Anulada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
        End If

        DtAsientoDetalle.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        UnaDevolucion_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonAjuste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAjuste.Click

        If ListaDeLotes.Count = 0 Then
            MsgBox("REMITO NO LOTEAD0! Operación Se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Exit Sub
        End If

        AsignacionDevoluciones(Grid, ListaDeLotes)

    End Sub
    Private Function PreparaArchivo() As Boolean

        DtCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM DevolucionCabeza WHERE Devolucion = " & PDevolucion & ";", ConexionRemito, DtCabeza) Then Return False
        If PDevolucion <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Devolución No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PDevolucion = 0 Then
            Dim Row1 As DataRow = DtCabeza.NewRow
            Row1("Devolucion") = 0
            Row1("Remito") = PRemito
            Row1("Fecha") = DateTime1.Value
            Row1("Estado") = 1
            Row1("Comentario") = ""
            DtCabeza.Rows.Add(Row1)
        End If

        MuestraCabeza()

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM DevolucionDetalle WHERE Devolucion = " & PDevolucion & ";", ConexionRemito, DtDetalle) Then Return False

        DtLotesAsignadosDevolucion = New DataTable
        Dim Sql As String = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 3 AND Comprobante = " & PDevolucion & ";"
        If Not Tablas.Read(Sql, ConexionRemito, DtLotesAsignadosDevolucion) Then Return False

        DtCabezaRemito = New DataTable
        If Not Tablas.Read("SELECT * FROM RemitosCabeza WHERE Remito = " & DtCabeza.Rows(0).Item("Remito") & ";", ConexionRemito, DtCabezaRemito) Then Return False
        If DtCabezaRemito.Rows.Count = 0 Then
            MsgBox("Remito YA no Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Pedido = DtCabezaRemito.Rows(0).Item("Pedido")
        ComboCliente.SelectedValue = DtCabezaRemito.Rows(0).Item("Cliente")
        ComboDeposito.SelectedValue = DtCabezaRemito.Rows(0).Item("Deposito")
        TextFechaRemito.Text = Format(DtCabezaRemito.Rows(0).Item("Fecha"), "dd/MM/yyyy")

        DtDetalleRemito = New DataTable
        If Not Tablas.Read("SELECT * FROM RemitosDetalle WHERE Remito = " & DtCabeza.Rows(0).Item("Remito") & ";", ConexionRemito, DtDetalleRemito) Then
            MsgBox("Error de Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        DtLotesAsignados = New DataTable
        If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante = 1 AND Comprobante = " & DtCabeza.Rows(0).Item("Remito") & ";", ConexionRemito, DtLotesAsignados) Then Return False

        'ListaDeLotes contiene Lotes asignados al Remito si es alta o Lotes asignados a la devoluciona si es modificacion.
        If PDevolucion = 0 Then
            ListaDeLotes = New List(Of FilaAsignacion)
            For Each Row As DataRow In DtLotesAsignados.Rows
                Dim Fila As New FilaAsignacion
                Fila.Indice = Row("Indice")
                Fila.Lote = Row("Lote")
                Fila.Secuencia = Row("Secuencia")
                Fila.Deposito = Row("Deposito")
                Fila.Operacion = Row("Operacion")
                Fila.Asignado = Row("Cantidad")
                ListaDeLotes.Add(Fila)
            Next
        Else
            ListaDeLotes = New List(Of FilaAsignacion)
            For Each Row As DataRow In DtLotesAsignadosDevolucion.Rows
                Dim Fila As New FilaAsignacion
                Fila.Indice = Row("Indice")
                Fila.Lote = Row("Lote")
                Fila.Secuencia = Row("Secuencia")
                Fila.Deposito = Row("Deposito")
                Fila.Operacion = Row("Operacion")
                Fila.Devolucion = Row("Cantidad")
                ListaDeLotes.Add(Fila)
            Next
        End If

        CreaDtGrid()

        If PDevolucion = 0 Then
            For Each Row As DataRow In DtDetalleRemito.Rows
                Dim Row1 As DataRow
                Row1 = DtGrid.NewRow()
                Row1("Indice") = Row("Indice")
                Row1("Articulo") = Row("Articulo")
                Row1("KilosXUnidad") = Row("KilosXUnidad")
                Row1("CantidadOriginal") = Row("Cantidad")
                If Row("Devueltas") > 0 Then
                    Row1("DevueltosPantalla") = Row("Devueltas")
                Else
                    Row1("AumentosPantalla") = Abs(Row("Devueltas"))  'Poruque en aumentos row("Devueltas") es negativo. 
                End If
                Row1("Diferencia") = Row("Cantidad") - Row("Devueltas")
                Row1("Cantidad") = 0
                Row1("ADevolver") = 0
                Row1("AAumentar") = 0
                Row1("AGranel") = False
                Row1("Medida") = ""
                HallaAGranelYMedida(Row("Articulo"), Row1("AGranel"), Row1("Medida"))
                DtGrid.Rows.Add(Row1)
            Next
        Else
            For Each Row As DataRow In DtDetalle.Rows
                Dim Row1 As DataRow
                Row1 = DtGrid.NewRow()
                Row1("Indice") = Row("Indice")
                Row1("Articulo") = Row("Articulo")
                Row1("KilosXUnidad") = Row("KilosXUnidad")
                Row1("CantidadOriginal") = 0
                Row1("Devueltos") = 0
                Row1("Diferencia") = 0
                Row1("DevueltosPantalla") = 0
                Row1("AumentosPantalla") = 0
                Row1("Cantidad") = Row("Cantidad")
                If Row("Cantidad") > 0 Then
                    Row1("ADevolver") = Row("Cantidad")
                Else
                    Row1("AAumentar") = Abs(Row("Cantidad"))
                End If
                Row1("AGranel") = False
                Row1("Medida") = ""
                HallaAGranelYMedida(Row("Articulo"), Row1("AGranel"), Row1("Medida"))
                DtGrid.Rows.Add(Row1)
            Next
        End If

        If PDevolucion = 0 Then
            Grid.Columns("CantidadOriginal").Visible = True
            Grid.Columns("DevueltosPantalla").Visible = True
            Grid.Columns("AumentosPantalla").Visible = True
            Grid.Columns("Diferencia").Visible = True
            Panel2.Enabled = True
            Grid.Columns("ADevolver").ReadOnly = False
            Grid.Columns("AAumentar").ReadOnly = False
        Else
            Grid.Columns("CantidadOriginal").Visible = False
            Grid.Columns("DevueltosPantalla").Visible = False
            Grid.Columns("AumentosPantalla").Visible = False
            Grid.Columns("Diferencia").Visible = False
            Panel2.Enabled = False
            Grid.Columns("ADevolver").ReadOnly = True
            Grid.Columns("AAumentar").ReadOnly = True
        End If

        Grid.DataSource = DtGrid

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Devolucion")
        TextDevolucion.DataBindings.Clear()
        TextDevolucion.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Remito")
        AddHandler Enlace.Format, AddressOf FormatRemito
        MaskedRemito.DataBindings.Clear()
        MaskedRemito.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Parse, AddressOf FormatTexto
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatRemito(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = "000000000000"
        Else : Numero.Value = Format(Numero.Value, "000000000000")
        End If

    End Sub
    Private Sub FormatTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Function AltaDevolucion(ByVal DtCabezaAux As DataTable, ByVal DtDetalleAux As DataTable, ByVal DtLotesAsignadosDevolucionAux As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal ListaDeLotesDevolucion As List(Of FilaAsignacion), ByVal ListaDeLotesAumento As List(Of FilaAsignacion), ByVal DtCabezaRemitoAux As DataTable, ByVal DtDetalleRemitoAux As DataTable, ByVal DtLotesAsignadosAux As DataTable, ByVal DtDetallePedidoAux As DataTable) As Double

        Dim Resul As Double = 0
        Dim Sql As String = ""

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                'Graba cabeza de Devolucion.
                Resul = GrabaTabla(DtCabezaAux.GetChanges, "DevolucionCabeza", ConexionRemito)
                If Resul <= 0 Then Return Resul

                'Graba detalle de Devolucion.
                Resul = GrabaTabla(DtDetalleAux.GetChanges, "DevolucionDetalle", ConexionRemito)
                If Resul <= 0 Then Return Resul

                'Modifica Detalle Remito.
                Resul = GrabaTabla(DtDetalleRemitoAux.GetChanges, "RemitosDetalle", ConexionRemito)
                If Resul <= 0 Then Return Resul

                'Modifica Cabeza Remito.
                Resul = GrabaTabla(DtCabezaRemitoAux.GetChanges, "RemitosCabeza", ConexionRemito)
                If Resul <= 0 Then Return Resul

                'Modifica AsignacionLotes.
                If Not IsNothing(DtLotesAsignadosAux.GetChanges) Then
                    Resul = GrabaTabla(DtLotesAsignadosAux.GetChanges, "AsignacionLotes", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                    'Modifica AsignacionLotes de la devolucion.
                End If
                If DtLotesAsignadosDevolucionAux.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtLotesAsignadosDevolucionAux.GetChanges, "AsignacionLotes", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza lotes.
                If ListaDeLotesDevolucion.Count <> 0 Then
                    If Not ActualizaStockDevolucion(ListaDeLotesDevolucion, "+") Then Return -3
                End If
                If ListaDeLotesAumento.Count <> 0 Then
                    If Not ActualizaStockDevolucion(ListaDeLotesAumento, "-") Then Return -3
                End If

                'Graba Asiento.
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                'Graba Pedido.
                If Not IsNothing(DtDetallePedidoAux.GetChanges) Then
                    Resul = GrabaTabla(DtDetallePedidoAux.GetChanges, "PedidosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return -2
        Catch ex As OleDb.OleDbException
            If ex.ErrorCode = GAltaExiste Then
                Return -1
            Else : Return -2
            End If
        Catch ex As DBConcurrencyException
            Return 0
        End Try

    End Function
    Private Function AnulaDevolucion(ByVal DtCabezaAux As DataTable, ByVal DtLotesAsignadosDevolucionAux As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal ListaDeLotesDevolucion As List(Of FilaAsignacion), ByVal ListaDeLotesAumento As List(Of FilaAsignacion), ByVal DtCabezaRemitoAux As DataTable, ByVal DtDetalleRemitoAux As DataTable, ByVal DtLotesAsignadosAux As DataTable, ByVal DtDetallePedidoAux As DataTable) As Boolean

        Dim Sql As String = ""
        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                Resul = GrabaTabla(DtCabezaAux.GetChanges, "DevolucionCabeza", ConexionRemito)
                If Resul <= 0 Then Return False

                'Borra asignacion lotes de la devolucion. 
                If Not IsNothing(DtLotesAsignadosDevolucionAux.GetChanges) Then
                    Resul = GrabaTabla(DtLotesAsignadosDevolucionAux.GetChanges, "AsignacionLotes", ConexionRemito)
                    If Resul <= 0 Then Return False
                End If

                'Actualiza Detalle Remito.
                Resul = GrabaTabla(DtDetalleRemitoAux.GetChanges, "RemitosDetalle", ConexionRemito)
                If Resul <= 0 Then Return False

                'Actualiza Cabeza Remito.
                Resul = GrabaTabla(DtCabezaRemitoAux.GetChanges, "RemitosCabeza", ConexionRemito)
                If Resul <= 0 Then Return False

                'Actualiza Asignacion Lotes del Remito.
                If Not IsNothing(DtLotesAsignadosAux.GetChanges) Then
                    Resul = GrabaTabla(DtLotesAsignadosAux.GetChanges, "AsignacionLotes", ConexionRemito)
                    If Resul <= 0 Then Return False
                End If

                'Actualiza Asientos.
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionRemito)
                    If Resul <= 0 Then Return False
                End If

                'Actualiza lotes.
                If ListaDeLotesDevolucion.Count <> 0 Then
                    If Not ActualizaStockDevolucion(ListaDeLotesDevolucion, "-") Then Return False
                End If
                If ListaDeLotesAumento.Count <> 0 Then
                    If Not ActualizaStockDevolucion(ListaDeLotesAumento, "+") Then Return False
                End If

                'Actualiza Pedido.
                If Not IsNothing(DtDetallePedidoAux.GetChanges) Then
                    Resul = GrabaTabla(DtDetallePedidoAux.GetChanges, "PedidosDetalle", Conexion)
                    If Resul <= 0 Then Return False
                End If

                Scope.Complete()
                Return True
            End Using
        Catch ex As TransactionException
            Return False
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        End Try

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Indice As New DataColumn("Indice")
        Indice.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Indice)

        Dim AGranel As New DataColumn("AGranel")
        AGranel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(AGranel)

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Medida)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim KilosXUnidad As New DataColumn("KilosXUnidad")
        KilosXUnidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(KilosXUnidad)

        Dim CantidadOriginal As New DataColumn("CantidadOriginal")
        CantidadOriginal.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CantidadOriginal)

        Dim Devueltos As New DataColumn("Devueltos")
        Devueltos.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Devueltos)

        Dim DevueltosPantalla As New DataColumn("DevueltosPantalla")
        DevueltosPantalla.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(DevueltosPantalla)

        Dim AumentosPantalla As New DataColumn("AumentosPantalla")
        AumentosPantalla.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(AumentosPantalla)

        Dim Diferencia As New DataColumn("Diferencia")
        Diferencia.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Diferencia)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim ADevolver As New DataColumn("ADevolver")
        ADevolver.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ADevolver)

        Dim AAumentar As New DataColumn("AAumentar")
        AAumentar.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(AAumentar)


    End Sub
    Private Function EstanDevueltos(ByVal Cantidad As Integer, ByVal Indice As Integer) As Boolean

        Dim cantidadEnLotes As Integer = 0
        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Indice = Indice Then cantidadEnLotes = cantidadEnLotes + Fila.Devolucion
        Next
        If cantidadEnLotes = Cantidad Then Return True

        Return False

    End Function
    Private Function BuscaIndiceEnListaDeLotes(ByVal Indice As Integer) As Boolean

        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Indice = Indice Then Return True
        Next

        Return False

    End Function
    Private Sub LlenaCombosGrid()

        Articulo.DataSource = TodosLosArticulos()
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Devolucion) FROM DevolucionCabeza;", Miconexion)
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
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM DevolucionCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return "1/1/1000"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "2/1/1000"
        End Try

    End Function
    Private Function ReHaceAsientoDetalleRemito(ByVal DtLotesAsignadosAux As DataTable, ByVal DtDetalleRemitoAux As DataTable, ByVal DtAsientoDetalle As DataTable) As Boolean

        'DtLotesAsignadosAux tiene asignacion lotes del remito i es alta.

        Dim NroAsiento As Integer = 0

        If Not Tablas.Read("SELECT D.* FROM AsientosCabeza As C INNER JOIN AsientosDetalle As D ON C.Asiento = D.Asiento WHERE C.TipoDocumento = 6060 AND C.Documento = " & DtCabezaRemito.Rows(0).Item("Remito") & ";", ConexionRemito, DtAsientoDetalle) Then Return False

        Dim DtAsientoCabeza As New DataTable
        If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = 6060 AND Documento = " & DtCabezaRemito.Rows(0).Item("Remito") & ";", ConexionRemito, DtAsientoCabeza) Then Return False
        If DtAsientoCabeza.Rows.Count = 0 Then
            Return True
        End If

        NroAsiento = DtAsientoCabeza.Rows(0).Item("Asiento")

        Dim DevolucionTotal As Boolean = True
        For Each Row As DataRow In DtDetalleRemitoAux.Rows
            If (Row("Cantidad") - Row("Devueltas")) <> 0 Then DevolucionTotal = False : Exit For
        Next
        If DevolucionTotal Then Return True

        'Re-hace asiento del remito.
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
    Private Function Valida() As Boolean

        If DateTime1.Value < UltimaFechaW And PDevolucion = 0 Then
            MsgBox("Fecha Incorrecta. Existe Otro Comprobante con Fecha " & UltimaFechaW & " Posterior a la Informada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If
        If DateTime1.Value < CDate(TextFechaRemito.Text) Then
            MsgBox("Fecha debe ser mayor o igual a fecha remito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If

        Dim RowsBusqueda() As DataRow
        Dim Cancelar As Boolean = True
        For Each Row As DataRow In DtDetalleRemito.Rows
            RowsBusqueda = DtGrid.Select("Indice = " & Row("Indice"))
            If RowsBusqueda.Length = 0 Then Cancelar = False : Exit For
            If Row("Cantidad") <> RowsBusqueda(0).Item("ADevolver") Then Cancelar = False : Exit For
        Next
        If Cancelar And PDevolucion = 0 Then
            MsgBox("Para Devolucion del Remito Completo Debe Anularlo.", MsgBoxStyle.Critical)
            Return False
        End If

        Dim Cantidad As Decimal = 0
        Dim Aumento As Decimal = 0
        For i As Integer = 0 To Grid.RowCount - 1
            Cantidad = Cantidad + Abs(Grid.Rows(i).Cells("Cantidad").Value)
            If Grid.Rows(i).Cells("Cantidad").Value <> 0 Then
                If TieneDecimales(Grid.Rows(i).Cells("Cantidad").Value) And Not Grid.Rows(i).Cells("AGranel").Value Then
                    MsgBox("Cantidad no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
        Next
        If Cantidad = 0 Then
            MsgBox("Debe Informar Cantidad a Devolver o Aumento. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If ListaDeLotes.Count <> 0 Then
            For i As Integer = 0 To Grid.RowCount - 1
                If Abs(Grid.Rows(i).Cells("Cantidad").Value) <> 0 Then
                    Dim CantidadEnLotes As Decimal = 0
                    For Each Fila As FilaAsignacion In ListaDeLotes
                        If Fila.Indice = Grid.Rows(i).Cells("Indice").Value Then
                            CantidadEnLotes = CantidadEnLotes + Fila.Devolucion
                        End If
                    Next
                    If Abs(Grid.Rows(i).Cells("Cantidad").Value) <> CantidadEnLotes Then
                        MsgBox("Debe Informar Lotes a Devolver o Aumentar en la linea " & i + 1 & ". Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Return False
                    End If
                End If
            Next
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        'para manejo del autocoplete de articulos.
        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.CellBeginEdit

        If Grid.Columns(e.ColumnIndex).Name = "ADevolver" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("AAumentar").Value) Then
                If Grid.CurrentRow.Cells("AAumentar").Value <> 0 Then e.Cancel = True
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "AAumentar" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("ADevolver").Value) Then
                If Grid.CurrentRow.Cells("ADevolver").Value <> 0 Then e.Cancel = True
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Grid.CurrentRow.Cells("LoteYSecuencia").Value = "" Then Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" And PDevolucion = 0 Then
            If IsDBNull(Grid.CurrentRow.Cells("Articulo").Value) Then
                MsgBox("Falta Seleccionar Articulo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If TieneDecimales(Grid.CurrentRow.Cells("Cantidad").Value) And Not Grid.CurrentRow.Cells("AGranel").Value Then
                MsgBox("Cantidad no debe tener Decimales.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If Grid.CurrentRow.Cells("Cantidad").Value = 0 Then Exit Sub
            SeleccionaLotesDevolucion.PArticulo = Grid.CurrentRow.Cells("Articulo").Value
            SeleccionaLotesDevolucion.PDeposito = ComboDeposito.SelectedValue
            SeleccionaLotesDevolucion.PCantidad = Abs(Grid.CurrentRow.Cells("Cantidad").Value)
            If Grid.CurrentRow.Cells("Cantidad").Value < 0 Then SeleccionaLotesDevolucion.PEsAumento = True
            SeleccionaLotesDevolucion.PIndice = Grid.CurrentRow.Cells("Indice").Value
            SeleccionaLotesDevolucion.PLista = ListaDeLotes
            SeleccionaLotesDevolucion.ShowDialog()
            ListaDeLotes = SeleccionaLotesDevolucion.PLista
            SeleccionaLotesDevolucion.Dispose()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" And PDevolucion <> 0 Then
            MuestraLotesAsignados.PEsDevolucion = True
            MuestraLotesAsignados.PArticulo = Grid.CurrentRow.Cells("Articulo").Value
            MuestraLotesAsignados.PIndice = Grid.CurrentRow.Cells("Indice").Value
            MuestraLotesAsignados.PLista = ListaDeLotes
            If Grid.CurrentRow.Cells("Cantidad").Value < 0 Then MuestraLotesAsignados.PEsAumento = True
            MuestraLotesAsignados.ShowDialog()
            MuestraLotesAsignados.Dispose()
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "ADevolver" Or Grid.Columns(e.ColumnIndex).Name = "AAumentar" Then
            If Not IsDBNull(e.Value) Then
                If BuscaIndiceEnListaDeLotes(Grid.Rows(e.RowIndex).Cells("Indice").Value) Then
                    Grid.Rows(e.RowIndex).Cells("LoteYSecuencia").Value = "Asignación" : Grid.Rows(e.RowIndex).Cells("LoteYSecuencia").Style.ForeColor = Color.Green
                Else
                    Grid.Rows(e.RowIndex).Cells("LoteYSecuencia").Value = "" : Grid.Rows(e.RowIndex).Cells("LoteYSecuencia").Style.ForeColor = Color.Green
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "ADevolver" Or Grid.Columns(e.ColumnIndex).Name = "AAumentar" Or Grid.Columns(e.ColumnIndex).Name = "Diferencia" Or Grid.Columns(e.ColumnIndex).Name = "DevueltosPantalla" Or Grid.Columns(e.ColumnIndex).Name = "AumentosPantalla" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                End If
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If (Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "ADevolver" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "AAumentar") And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloDecimales_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub SoloDecimales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "ADevolver" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "AAumentar" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "ADevolver" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "AAumentar" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("ADevolver") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If e.ProposedValue > e.Row("Diferencia") Then
                ' Cantidad Devolución Supera Saldo.
                MsgBox("Devolucion Supera Saldo")
                e.ProposedValue = e.Row("ADevolver")
                Exit Sub
            End If
            e.Row("Cantidad") = e.ProposedValue
            If e.ProposedValue <> e.Row("ADevolver") Then
                For Each Fila As FilaAsignacion In ListaDeLotes
                    If Fila.Indice = e.Row("Indice") Then Fila.Devolucion = 0
                Next
            End If
        End If

        If e.Column.ColumnName.Equals("AAumentar") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.Row("Cantidad") = -e.ProposedValue     'Si aumento el stock el signo es negativo(Inverso de la devolucion)
            If e.ProposedValue <> e.Row("AAumentar") Then
                For Each Fila As FilaAsignacion In ListaDeLotes
                    If Fila.Indice = e.Row("Indice") Then Fila.Devolucion = 0
                Next
            End If
        End If

    End Sub
   
End Class