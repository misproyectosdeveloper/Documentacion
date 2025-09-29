Imports System.Transactions
Imports System.Math
Public Class UnaReAsignacionRemito
    Public PRemito As Double
    Public PAbierto As Boolean
    Public PIngreso As Integer
    '
    Dim DtAsignacionLotes As DataTable
    Dim DtDetalle As DataTable
    Dim DtCabeza As DataTable
    '
    Dim ListaDeLotes As New List(Of FilaAsignacion)
    Dim ListaDeLotesOriginal As New List(Of FilaAsignacion)
    Private WithEvents bs As New BindingSource
    '
    Dim ConexionRemito As String
    'PMERCADO 10-06-2025
    Dim Carga_Grilla As Boolean

    Private Sub UnReAsignarLotesRemito_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If PIngreso <> 0 Then TextIngreso.Text = PIngreso

    End Sub
    Private Sub UnaReAsignacionRemito_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If Not ListasIguales() And Not GModificacionOk Then
            If MsgBox("Cambios no han sido grabados. Desesa Cerrar el programa de todos modos (S/N)", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.No Then
                e.Cancel = True
            End If
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ListasIguales() Then
            MsgBox("No hay Cambios. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        'Verifica que los lotes modificados no tengan N.V.L.P.
        If TieneNVLP() Then Exit Sub

        'Actualiza AsignacionLotes.
        Dim DtAsignacionLotesAux As DataTable = DtAsignacionLotes.Copy
        If Not ArmaAsignacionRemitos(1, DtCabeza, DtAsignacionLotesAux, ListaDeLotes) Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma Dt Devoluciones.
        Dim DtDevoluciones As New DataTable

        'Arma Stock Lotes para articulos Loteados.
        Dim DtStockB As New DataTable
        Dim DtStockN As New DataTable
        If Not ArmaStockFactura(DtAsignacionLotes, DtStockB, DtStockN, ListaDeLotes, Nothing) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        If Not PermisoTotal And DtStockN.Rows.Count <> 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Usuario No Autorizado(1000).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        'Arma Asientos.
        Dim ListaDeLotesParaModificar As New List(Of FilaAsignacion)
        ArmaLotesParaModificar(ListaDeLotesParaModificar, DtAsignacionLotes, ListaDeLotes)

        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not ReHaceAsientoDetalleRemito(DtAsignacionLotesAux, DtDetalle, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        If DtCabezaAux.Rows(0).Item("Estado") = 2 Then DtCabezaAux.Rows(0).Item("Estado") = 1

        Dim NumeroW As Double = ActualizaAsignacion("M", DtCabezaAux, DtAsientoDetalle, DtAsignacionLotesAux, DtStockB, DtStockN, DtDevoluciones)

        If NumeroW < 0 Then
            MsgBox("ERROR: En Base de Datos. Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("ERROR: Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonDevolverTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDevolverTodos.Click

        ButtonAsignacionManual.PerformClick()

        If DtCabeza.Rows(0).Item("Estado") <> 1 Then
            MsgBox("No Existe Asignaciones.")
            Exit Sub
        End If

        'Verifica que los lotes modificados no tengan N.V.L.P.
        For Each Row As DataRow In DtAsignacionLotes.Rows
            If Row("Liquidado") Then
                MsgBox("Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000") & " No se Puede cambiar Pues Tiene N.V.L.P. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        Next

        If Not ListasIguales() Then
            MsgBox("Exite Cambios en Asignaciones. Operación se CANCELA.")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If HallaArticuloDeshabilitado(DtDetalle) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        'Borra  AsignacionLotes de las devoluciones loteadas.
        Dim DtDevoluciones As New DataTable
        HallaDevolucionesLoteadas(DtDevoluciones)  'halla devoluciones loteadas para Borrar sus AsignacionesLotes.
        '----------------------------------------------------

        'Borra AsignacionLotes del remito.
        Dim ListaDeLotesAux As New List(Of FilaAsignacion)
        Dim DtAsignacionLotesAux As DataTable = DtAsignacionLotes.Copy
        For Each Row As DataRow In DtAsignacionLotesAux.Rows
            Row.Delete()
        Next
        '----------------------------------------------------

        'Actualiza Stock de Lotes para articulos Loteados.
        Dim DtStockB As New DataTable
        Dim DtStockN As New DataTable
        If Not ArmaStockFactura(DtAsignacionLotes, DtStockB, DtStockN, ListaDeLotesAux, Nothing) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        If Not PermisoTotal And DtStockN.Rows.Count <> 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Usuario No Autorizado(1000).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        '----------------------------------------------------
        'Arma Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not ReHaceAsientoDetalleRemito(DtAsignacionLotesAux, DtDetalle, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        DtCabezaAux.Rows(0).Item("Estado") = 2

        Dim NumeroW As Double = ActualizaAsignacion("M", DtCabezaAux, DtAsientoDetalle, DtAsignacionLotesAux, DtStockB, DtStockN, DtDevoluciones)

        If NumeroW < 0 Then
            MsgBox("ERROR: En Base de Datos. Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("ERROR: Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close() : Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAsignacionAutomatica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsignacionAutomatica.Click

        'PMERCADO 10-06-2025
        ButtonAsignacionManual.PerformClick()

        If DtAsignacionLotes.Rows.Count <> 0 Then
            MsgBox("Comprobante ya Asignado. Debe Devolver todos los Lotes.  Operación se CANCELA.")
            Exit Sub
        End If

        'ELIMINAR
        Dim Fecha1 As DateTime
        Dim Fecha2 As DateTime
        Dim diferencia As TimeSpan = Fecha2 - Fecha1

        Fecha1 = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")

        If Not AsignacionLotesAutomatico(DtCabeza, DtDetalle, 0, ListaDeLotes) Then Exit Sub

        Fecha2 = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")

        'Console.WriteLine("Días: " & diferencia.Days)
        'Console.WriteLine("Horas: " & diferencia.Hours)
        'Console.WriteLine("Minutos: " & diferencia.Minutes)
        'Console.WriteLine("Segundos: " & diferencia.Seconds)
        MsgBox("Segundos: " & diferencia.Seconds, MsgBoxStyle.Information)



        MsgBox("Asignacion Correcta.", MsgBoxStyle.Information)

        Grid.Refresh()

    End Sub
    Private Sub ButtonAsignarIngreso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsignarIngreso.Click

        'PMERCADO 10-06-2025
        ButtonAsignacionManual.PerformClick()

        If DtAsignacionLotes.Rows.Count <> 0 Then
            MsgBox("Comprobante ya Asignado. Debe Devolver todos los Lotes.  Operación se CANCELA.")
            Exit Sub
        End If

        If Not AsignacionLotesAutomatico(DtCabeza, DtDetalle, PIngreso, ListaDeLotes) Then Exit Sub
        MsgBox("Asignacion Correcta.", MsgBoxStyle.Information)
        Grid.Refresh()

    End Sub
    Private Sub MuestraDatos()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtCabeza = New DataTable
        Dim Sql As String = "SELECT * FROM RemitosCabeza WHERE Remito = " & PRemito & ";"
        If Not Tablas.Read(Sql, ConexionRemito, DtCabeza) Then Me.Close() : Exit Sub
        If PRemito <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Remito No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        TextRemito.Text = Format(PRemito, "0000-00000000")
        LabelCliente.Text = NombreCliente(DtCabeza.Rows(0).Item("Cliente"))
        LabelDeposito.Text = NombreDeposito(DtCabeza.Rows(0).Item("Deposito"))

        DtDetalle = New DataTable
        Sql = "SELECT * FROM RemitosDetalle WHERE Remito = " & PRemito & ";"
        If Not Tablas.Read(Sql, ConexionRemito, DtDetalle) Then Me.Close() : Exit Sub

        'Arma tabla con AsignacionLotes. 
        DtAsignacionLotes = New DataTable
        Sql = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 1 AND Comprobante = " & PRemito & ";"
        If Not Tablas.Read(Sql, ConexionRemito, DtAsignacionLotes) Then Me.Close() : Exit Sub

        ListaDeLotes = New List(Of FilaAsignacion)
        ListaDeLotesOriginal = New List(Of FilaAsignacion)

        For Each Row As DataRow In DtAsignacionLotes.Rows
            Dim Fila As New FilaAsignacion
            Fila.Indice = Row("Indice")
            Fila.Lote = Row("Lote")
            Fila.Secuencia = Row("Secuencia")
            Fila.Deposito = Row("Deposito")
            Fila.Operacion = Row("Operacion")
            Fila.Asignado = Row("Cantidad")
            ListaDeLotes.Add(Fila)
            ListaDeLotesOriginal.Add(Fila)
        Next

        Grid.DataSource = bs
        bs.DataSource = DtDetalle

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Articulo.DataSource = TodosLosArticulos()
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Function TieneNVLP() As Boolean

        For Each Row As DataRow In DtAsignacionLotes.Rows
            Dim Esta As Boolean = False
            If Row("Liquidado") Then
                For Each Fila As FilaAsignacion In ListaDeLotes
                    If Fila.Lote = Row("Lote") And Fila.Secuencia = Row("Secuencia") Then
                        If Row("cantidad") <> Fila.Asignado Then
                            Exit For
                        Else
                            Esta = True
                        End If
                    End If
                Next
                If Not Esta Then
                    MsgBox("Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000") & " No se Puede cambiar Pues Tiene N.V.L.P. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return True
                End If
            End If
        Next

    End Function
    Private Function BuscaIndice(ByVal Indice As Integer) As Boolean

        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Indice = Indice Then Return True
        Next

        Return False

    End Function
    Private Function ListasIguales() As Boolean

        Dim Esta As Boolean

        For Each Fila As FilaAsignacion In ListaDeLotes
            Esta = False
            For Each Fila1 As FilaAsignacion In ListaDeLotesOriginal
                If Fila.Indice = Fila1.Indice And Fila.Lote = Fila1.Lote And Fila.Secuencia = Fila1.Secuencia And Fila.Asignado = Fila1.Asignado Then
                    Esta = True
                    Exit For
                End If
            Next
            If Not Esta Then Return False
        Next

        For Each Fila As FilaAsignacion In ListaDeLotesOriginal
            Esta = False
            For Each Fila1 As FilaAsignacion In ListaDeLotes
                If Fila.Indice = Fila1.Indice And Fila.Lote = Fila1.Lote And Fila.Secuencia = Fila1.Secuencia And Fila.Asignado = Fila1.Asignado Then
                    Esta = True
                    Exit For
                End If
            Next
            If Not Esta Then Return False
        Next

        Return True

    End Function
    Private Function ActualizaAsignacion(ByVal Funcion As String, ByVal DtCabezaAux As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtAsignacionLotesAux As DataTable, ByVal DtStockB As DataTable, ByVal DtStockN As DataTable, ByVal DtDevoluciones As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                'Actualiza estado = 1 en CabezaRemito si es una asignacion. 
                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaAux.GetChanges, "RemitosCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                ' Graba Asignacion de lotes
                Resul = GrabaTabla(DtAsignacionLotesAux.GetChanges, "AsignacionLotes", ConexionRemito)
                If Resul <= 0 Then Return Resul

                ' Actualiza Stock.
                If Not IsNothing(DtStockB.GetChanges) Then
                    Resul = GrabaTabla(DtStockB.GetChanges, "Lotes", Conexion)
                End If
                If Not IsNothing(DtStockN.GetChanges) Then
                    Resul = GrabaTabla(DtStockN.GetChanges, "Lotes", ConexionN)
                End If
                'Graba Asiento.
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                'Borra AsignacionLotes de las devoluciones.
                For Each Row As DataRow In DtDevoluciones.Rows   'halla AsignacionLotes de las devoluciones para borrarlas.
                    Dim DtDevo As New DataTable
                    Dim Sql As String = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 3 AND Comprobante = " & Row("Devolucion") & ";"
                    If Not Tablas.Read(Sql, ConexionRemito, DtDevo) Then End
                    If DtDevo.Rows.Count <> 0 Then DtDevo.Rows(0).Delete()
                    Resul = GrabaTabla(DtDevo.GetChanges, "AsignacionLotes", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                Next
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function ReHaceAsientoDetalleRemito(ByVal DtLotesAsignadosAux As DataTable, ByVal DtDetalleRemitoAux As DataTable, ByVal DtAsientoDetalle As DataTable) As Boolean

        Dim NroAsiento As Integer = 0

        Dim DtAsientoCabeza As New DataTable

        If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Estado <> 3 AND TipoDocumento = 6060 AND Documento = " & PRemito & ";", ConexionRemito, DtAsientoCabeza) Then Return False
        If DtAsientoCabeza.Rows.Count = 0 Then
            Return True
        End If

        NroAsiento = DtAsientoCabeza.Rows(0).Item("Asiento")

        If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & NroAsiento & ";", ConexionRemito, DtAsientoDetalle) Then Return False

        Dim DtAsientoDetalleAux As DataTable = DtAsientoDetalle.Clone
        DtAsientoCabeza.Rows.Clear()
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
    Private Sub HallaDevolucionesLoteadas(ByRef DtDevoluciones As DataTable)

        DtDevoluciones = New DataTable

        Dim Sql As String = "SELECT C.* FROM DevolucionCabeza As C INNER JOIN AsignacionLotes AS A ON A.TipoComprobante = 3 AND A.Comprobante = C.Devolucion WHERE C.Estado <> 3 AND C.Remito = " & PRemito & ";"

        If Not Tablas.Read(Sql, ConexionRemito, DtDevoluciones) Then End

    End Sub
    Private Function Valida() As Boolean

        Dim Cantidad As Decimal, CantidadAsignada As Decimal

        For i As Integer = 0 To Grid.Rows.Count - 1
            Cantidad = Cantidad + (Grid.Rows(i).Cells("Cantidad").Value - Grid.Rows(i).Cells("Devueltas").Value)
        Next

        For Each Fila As FilaAsignacion In ListaDeLotes
            CantidadAsignada = CantidadAsignada + Fila.Asignado
        Next

        If CantidadAsignada = 0 Then
            MsgBox("Debe Asingnar Lotes. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If CantidadAsignada <> Cantidad Then
            MsgBox("Falta Asingnar Lotes. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If HallaEstadoArticulo(Grid.CurrentRow.Cells("Articulo").Value) = 3 Then
                MsgBox("Articulo " & Grid.CurrentRow.Cells("Articulo").FormattedValue & " Esta Deshabilitado; debe Activarlo previamente. Operación se CANCELA.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            SeleccionaLotesAAsignar.PArticulo = Grid.CurrentRow.Cells("Articulo").Value
            SeleccionaLotesAAsignar.PDeposito = DtCabeza.Rows(0).Item("Deposito")
            SeleccionaLotesAAsignar.PCantidad = Grid.CurrentRow.Cells("Cantidad").Value - Grid.CurrentRow.Cells("Devueltas").Value
            SeleccionaLotesAAsignar.PBloquea = False
            SeleccionaLotesAAsignar.PComprobante = PRemito
            SeleccionaLotesAAsignar.PConexion = ConexionRemito
            SeleccionaLotesAAsignar.PIndice = CInt(Grid.CurrentRow.Cells("Indice").Value)
            SeleccionaLotesAAsignar.PLista = ListaDeLotes
            SeleccionaLotesAAsignar.PListaOriginal = ListaDeLotesOriginal
            SeleccionaLotesAAsignar.ShowDialog()
            ListaDeLotes = SeleccionaLotesAAsignar.PLista
            SeleccionaLotesAAsignar.Dispose()
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        'PMERCADO 10-06-2025
        If Carga_Grilla = True Then

            If e.ColumnIndex < 0 Then Exit Sub

            If IsNumeric(e.Value) Then
                If e.Value = 0 Then e.Value = Format(0, "#") : Exit Sub
            End If

            If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
                If BuscaIndice(Grid.Rows(e.RowIndex).Cells("Indice").Value) Then
                    e.Value = "  Asignados  " : e.CellStyle.ForeColor = Color.Green
                Else
                    e.Value = "No Asignado" : e.CellStyle.ForeColor = Color.Red
                End If
            End If

        End If
    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub

    Private Sub ButtonAsignacionManual_Click(sender As Object, e As EventArgs) Handles ButtonAsignacionManual.Click
        'PMERCADO 10-06-2025
        Carga_Grilla = True

        Grid.AutoGenerateColumns = False

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionRemito = Conexion
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionRemito = ConexionN
        End If

        LlenaCombosGrid()

        GModificacionOk = False

        MuestraDatos()



    End Sub
End Class