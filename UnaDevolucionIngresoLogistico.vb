Imports System.Transactions
Public Class UnaDevolucionIngresoLogistico
    Public PDevolucion As Integer
    Public PAbierto As Boolean
    Public PIngreso As Double
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    '
    Dim DtGrid As DataTable
    Dim DtStockInsumos As DataTable
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtIngresoDetalle As DataTable
    '
    Dim ConexionIngreso As String
    Dim ConexionRelacionada As String
    Dim FechaIngreso As Date
    Private Sub UnaDevolucionInsumo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(12) Then PBloqueaFunciones = True

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        LlenaComboTablas(ComboDeposito, 20)
        ComboDeposito.SelectedValue = 0

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        LlenaCombosGrid()

        CreaDtGrid()

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionIngreso = Conexion
            ConexionRelacionada = ConexionN
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionIngreso = ConexionN
            ConexionRelacionada = Conexion
        End If

        ArmaArchivos()

        GModificacionOk = False

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid_ColumnChanging)

    End Sub
    Private Sub UnaDevolucionInsumo_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        If PDevolucion = 0 Then
            If Grid.RowCount <> 0 Then
                Grid.CurrentCell = Grid.Rows(0).Cells("Devolucion")
            End If
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
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtCabezaW As New DataTable
        Dim DtDetalleW As New DataTable
        Dim DtIngresoDetalleW As New DataTable
        Dim DtStockInsumosW As New DataTable

        DtCabezaW = DtCabeza.Copy
        DtDetalleW = DtDetalle.Clone
        DtIngresoDetalleW = DtIngresoDetalle.Copy
        DtStockInsumosW = DtStockInsumos.Copy

        'Arma detalle de devolucion.
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("Devolucion") <> 0 Then
                Dim Row As DataRow = DtDetalleW.NewRow()
                Row("Devolucion") = 0
                Row("Articulo") = Row1("Articulo")
                Row("Cantidad") = Row1("Devolucion")
                DtDetalleW.Rows.Add(Row)
            End If
        Next

        'Actualiza detalle Recepcion.
        Dim RowsBusqueda() As DataRow
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("Devolucion") <> 0 Then
                RowsBusqueda = DtIngresoDetalleW.Select("Articulo = " & Row1("Articulo"))
                RowsBusqueda(0).Item("Devueltas") = RowsBusqueda(0).Item("Devueltas") + Row1("Devolucion")
            End If
        Next

        'Actualiza Stock de Insumos.
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("Devolucion") <> 0 Then
                RowsBusqueda = DtStockInsumosW.Select("Articulo = " & Row1("Articulo"))
                RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") - Row1("Devolucion")
            End If
        Next

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim NumeroDevolucion As Integer = 0
        Dim i As Integer = 0
        Dim NumeroW As Double = 0

        For i = 1 To 50
            NumeroDevolucion = UltimaNumeracion(ConexionIngreso)
            If NumeroDevolucion < 0 Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If

            DtCabezaW.Rows(0).Item("Devolucion") = NumeroDevolucion
            For Each Row1 As DataRow In DtDetalleW.Rows
                Row1("Devolucion") = NumeroDevolucion
            Next

            NumeroW = ActualizaDevolucion(DtCabezaW, DtDetalleW, DtIngresoDetalleW, DtStockInsumosW)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -2 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -1 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = -2 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            MsgBox("Devolución Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PDevolucion = NumeroDevolucion
            GModificacionOk = True
            ArmaArchivos()
        End If

        DtCabezaW.Dispose()
        DtDetalleW.Dispose()
        DtIngresoDetalleW.Dispose()
        DtStockInsumosW.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

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

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Devolución Ya esta Anulada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Dim DtCabezaW As New DataTable
        Dim DtDetalleW As New DataTable
        Dim DtIngresoDetalleW As New DataTable
        Dim DtStockInsumosW As New DataTable

        DtCabezaW = DtCabeza.Copy
        DtDetalleW = DtDetalle.Clone
        DtIngresoDetalleW = DtIngresoDetalle.Copy
        DtStockInsumosW = DtStockInsumos.Copy

        Dim RowsBusqueda() As DataRow
        'Actualiza detalle Recepcion.
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("Devolucion") <> 0 Then
                RowsBusqueda = DtIngresoDetalleW.Select("Articulo = " & Row1("Articulo"))
                RowsBusqueda(0).Item("Devueltas") = RowsBusqueda(0).Item("Devueltas") - Row1("Devolucion")
            End If
        Next
        'Actualiza Stock de Insumos.
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("Devolucion") <> 0 Then
                RowsBusqueda = DtStockInsumosW.Select("Articulo = " & Row1("Articulo"))
                RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") + Row1("Devolucion")
            End If
        Next

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtCabezaW.Rows(0).Item("Estado") = 3

        Dim Resul As Integer = ActualizaDevolucion(DtCabezaW, DtDetalleW, DtIngresoDetalleW, DtStockInsumosW)

        If Resul < 0 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Devolución Anulada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

        DtCabezaW.Dispose()
        DtDetalleW.Dispose()
        DtIngresoDetalleW.Dispose()
        DtStockInsumosW.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonDevolverTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDevolverTodos.Click

        For i As Integer = 0 To Grid.Rows.Count - 1
            Grid.Rows(i).Cells("Devolucion").Value = Grid.Rows(i).Cells("Cantidad").Value - Grid.Rows(i).Cells("Devueltas").Value
        Next

    End Sub
    Private Sub ArmaArchivos()

        DtGrid.Clear()

        DtCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM DevolucionIngresoLogisticoCabeza WHERE Devolucion = " & PDevolucion & ";", ConexionIngreso, DtCabeza) Then Me.Close() : Exit Sub
        If PDevolucion <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Devolución No Existen En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PDevolucion = 0 Then
            Dim Row As DataRow = DtCabeza.NewRow
            Row("Devolucion") = 0
            Row("Ingreso") = PIngreso
            Row("Fecha") = DateTime1.Value
            Row("Estado") = 1
            Row("Comentario") = ""
            DtCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM DevolucionIngresoLogisticoDetalle WHERE Devolucion = " & PDevolucion & ";", ConexionIngreso, DtDetalle) Then Me.Close() : Exit Sub

        If PDevolucion <> 0 Then
            PIngreso = DtCabeza.Rows(0).Item("Ingreso")
        End If

        If Not LeerIngreso() Then Me.Close() : Exit Sub

        Dim RowsBusqueda() As DataRow

        If PDevolucion = 0 Then
            For Each Row As DataRow In DtIngresoDetalle.Rows
                Dim Row1 As DataRow = DtGrid.NewRow()
                Row1("Articulo") = Row("Articulo")
                Row1("Cantidad") = Row("Cantidad")
                Row1("Devueltas") = Row("Devueltas")
                Row1("Devolucion") = 0
                Row1("Stock") = 0
                Row1("AGranel") = False
                Row1("Medida") = ""
                HallaAGranelYMedidaLogistico(Row1("Articulo"), Row1("AGranel"), Row1("Medida"))
                DtGrid.Rows.Add(Row1)
            Next
        Else
            For Each Row As DataRow In DtDetalle.Rows
                Dim Row1 As DataRow = DtGrid.NewRow()
                Row1("Articulo") = Row("Articulo")
                RowsBusqueda = DtIngresoDetalle.Select("Articulo = " & Row("Articulo"))
                Row1("Cantidad") = RowsBusqueda(0).Item("Cantidad")
                Row1("Devueltas") = 0
                Row1("Devolucion") = Row("Cantidad")
                Row1("Stock") = 0
                Row1("AGranel") = False
                Row1("Medida") = ""
                HallaAGranelYMedidaLogistico(Row1("Articulo"), Row1("AGranel"), Row1("Medida"))
                DtGrid.Rows.Add(Row1)
            Next
        End If

        DtStockInsumos = New DataTable
        For Each Row As DataRow In DtGrid.Rows
            If Not Tablas.Read("SELECT * FROM StockArticulosLogisticos WHERE Articulo = " & Row("Articulo") & " AND Deposito = " & ComboDeposito.SelectedValue & ";", ConexionIngreso, DtStockInsumos) Then Me.Close() : Exit Sub
        Next
        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtStockInsumos.Select("Articulo = " & Row("Articulo"))
            If RowsBusqueda.Length <> 0 Then Row("Stock") = RowsBusqueda(0).Item("Stock")
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If PDevolucion = 0 Then
            Panel1.Enabled = True
            Grid.ReadOnly = False
            Grid.Columns("Devueltas").Visible = True
            ButtonDevolverTodos.Enabled = True
        Else
            Panel1.Enabled = False
            Grid.ReadOnly = True
            Grid.Columns("Devueltas").Visible = False
            ButtonDevolverTodos.Enabled = False
        End If

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Devolucion")
        AddHandler Enlace.Format, AddressOf FormatDevolucion
        TextDevolucion.DataBindings.Clear()
        TextDevolucion.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Parse, AddressOf FormatTexto
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatDevolucion(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "#")

    End Sub
    Private Sub FormatTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Function LeerIngreso() As Boolean

        Dim DtIngresoCabeza As New DataTable
        If Not Tablas.Read("SELECT * FROM IngresoArticulosLogisticosCabeza WHERE Ingreso = " & PIngreso & ";", ConexionIngreso, DtIngresoCabeza) Then Return False
        If DtIngresoCabeza.Rows.Count = 0 Then
            MsgBox("Ingreso NO EXISTE.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If

        DtIngresoDetalle = New DataTable
        Dim Sql As String = "SELECT * FROM IngresoArticulosLogisticosDetalle WHERE Ingreso = " & PIngreso & ";"
        If Not Tablas.Read(Sql, ConexionIngreso, DtIngresoDetalle) Then Return False
        If DtIngresoDetalle.Rows.Count = 0 Then
            MsgBox("Ingreso NO EXISTE.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If

        TextProveedor.Text = NombreProveedor(DtIngresoCabeza.Rows(0).Item("Proveedor"))
        ComboDeposito.SelectedValue = DtIngresoCabeza.Rows(0).Item("Deposito")
        TextRemito.Text = Format(DtIngresoCabeza.Rows(0).Item("Remito"), "0000-00000000")
        TextGuia.Text = Format(DtIngresoCabeza.Rows(0).Item("Guia"), "00000000")
        TextFechaIngreso.Text = Format(DtIngresoCabeza.Rows(0).Item("Fecha"), "dd/MM/yyyy")
        FechaIngreso = DtIngresoCabeza.Rows(0).Item("Fecha")

        DtIngresoCabeza.Dispose()

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim AGranel As DataColumn = New DataColumn("AGranel")
        AGranel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(AGranel)

        Dim Articulo As DataColumn = New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Devueltas As New DataColumn("Devueltas")
        Devueltas.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Devueltas)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Devolucion As New DataColumn("Devolucion")
        Devolucion.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Devolucion)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Stock)

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Medida)

    End Sub
    Private Function ActualizaDevolucion(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtIngresoDetalleW As DataTable, ByVal DtStockInsumosW As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaW.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaW.GetChanges, "DevolucionIngresoLogisticoCabeza", ConexionIngreso)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtDetalleW.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleW.GetChanges, "DevolucionIngresoLogisticoDetalle", ConexionIngreso)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtIngresoDetalleW.GetChanges) Then
                    Resul = GrabaTabla(DtIngresoDetalleW.GetChanges, "IngresoArticulosLogisticosDetalle", ConexionIngreso)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtStockInsumosW.GetChanges) Then
                    Resul = GrabaTabla(DtStockInsumosW.GetChanges, "StockArticulosLogisticos", ConexionIngreso)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Articulo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 6;")
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Devolucion) FROM DevolucionIngresoLogisticoCabeza;", Miconexion)
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
    Private Function Valida() As Boolean

        If DiferenciaDias(FechaIngreso, DateTime1.Value) < 0 Then
            MsgBox("Fecha Devolución no debe ser menor a Fecha de Ingreso.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Dim Cantidad As Double = 0
        For i As Integer = 0 To Grid.RowCount - 1
            Cantidad = Cantidad + Grid.Rows(i).Cells("Devolucion").Value
            If Grid.Rows(i).Cells("Devolucion").Value <> 0 Then
                If TieneDecimales(Grid.Rows(i).Cells("Devolucion").Value) And Not Grid.Rows(i).Cells("AGranel").Value Then
                    MsgBox("Cantidad no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Devolucion")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
        Next

        If Cantidad = 0 Then
            MsgBox("Falta Informar Devolución. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Grid.Columns(e.ColumnIndex).ReadOnly = False Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Devolucion" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloEntero_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub SoloEntero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Devolucion" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Devolucion" Or Grid.Columns(e.ColumnIndex).Name = "Devueltas" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then Format(0, "#")
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Dtgrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Devolucion")) Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If (e.Row("Cantidad") - e.Row("Devueltas")) < e.ProposedValue Then
                MsgBox("Devolución Supera Cantidad Remito.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3, "Error")
                e.ProposedValue = e.Row("Devolucion")
                Exit Sub
            End If
            If Not IsDBNull(e.Row("Stock")) Then
                If e.Row("Stock") < e.ProposedValue Then
                    MsgBox("Supera Stock en Deposito " & ComboDeposito.Text, MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3, "Error")
                    e.ProposedValue = e.Row("Devolucion")
                End If
            End If
        End If

    End Sub

End Class