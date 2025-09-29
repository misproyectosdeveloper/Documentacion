Public Class UnaTransferenciaInsumo
    Public PTrans As Double
    Public PActualizacionOk As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim DtTransCabeza As DataTable
    Dim DtTransDetalle As DataTable
    Dim DtGrid As DataTable
    '
    Dim ErrorFatal As Boolean
    Dim cb As ComboBox
    Dim ConexionTrans As String
    Private Sub UnaTransferenciaInsumo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Lupa").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")

        LlenaComboTablas(ComboDepositoOrigen, 20)
        LlenaComboTablas(ComboDepositoDestino, 20)

        If PTrans = 0 Then
            OpcionTransferencia.PEsInsumo = True
            OpcionTransferencia.ShowDialog()
            If OpcionTransferencia.POrigen = 0 Then OpcionTransferencia.Dispose() : Me.Close() : Exit Sub
            ComboDepositoOrigen.SelectedValue = OpcionTransferencia.POrigen
            ComboDepositoDestino.SelectedValue = OpcionTransferencia.PDestino
            OpcionTransferencia.Dispose()
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        CreaDtGrid()

        PActualizacionOk = False

        ArmaArchivos()

        LlenaCombosGrid()

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)

    End Sub
    Private Sub UnaTransferenciaInsumo_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PTrans <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()
        Grid.EndEdit()

        If Not Valida() Then Exit Sub

        Dim Row As DataRow
        Dim DtCabezaW As DataTable
        Dim DtDetalleW As DataTable

        DtCabezaW = DtTransCabeza.Copy
        DtDetalleW = DtTransDetalle.Clone

        DtCabezaW.Rows(0).Item("Transferencia") = Val(TextComprobante.Text)

        For Each row1 As DataRow In DtGrid.Rows
            Row = DtDetalleW.NewRow()
            Row("Transferencia") = Val(TextComprobante.Text)
            Row("Articulo") = row1("Articulo")
            Row("OrdenCompra") = row1("OrdenCompra")
            Row("Cantidad") = row1("Cantidad")
            DtDetalleW.Rows.Add(Row)
        Next

        Dim DtStock As New DataTable

        If Not ActualizaArchivoAlta(DtStock) Then Exit Sub

        Dim Trans As OleDb.OleDbTransaction

        Using MiConexion As New OleDb.OleDbConnection(ConexionTrans)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                Try
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM TransInsumoCabeza;", MiConexion)
                        DaP.SelectCommand.Transaction = Trans
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtCabezaW)
                    End Using
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM TransInsumoDetalle;", MiConexion)
                        DaP.SelectCommand.Transaction = Trans
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtDetalleW)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM StockInsumos;", MiConexion)
                        DaP.SelectCommand.Transaction = Trans
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtStock)
                    End Using
                    Trans.Commit()
                    MsgBox("Transferencia Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    PTrans = Val(TextComprobante.Text)
                    ArmaArchivos()
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                    MsgBox("Error Otro Usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Catch err As OleDb.OleDbException
                    Trans.Rollback()
                    If err.ErrorCode = GAltaExiste Then
                        MsgBox("Error Numero Transferencia Ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Else
                        MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    End If
                Finally
                    Trans = Nothing
                End Try
            Catch err As OleDb.OleDbException
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End Try
        End Using

        DtStock.Dispose()
        DtCabezaW.Dispose()
        DtDetalleW.Dispose()

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PTrans = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Transferencia Ya esta Anulada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtCabezaW As DataTable
        DtCabezaW = DtTransCabeza.Copy

        DtCabezaW.Rows(0).Item("estado") = 3

        Dim DtStock As New DataTable

        If Not ActualizaArchivoBaja(DtStock) Then Exit Sub

        Dim Trans As OleDb.OleDbTransaction

        Using MiConexion As New OleDb.OleDbConnection(ConexionTrans)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Try
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM TransInsumoCabeza;", MiConexion)
                        DaP.SelectCommand.Transaction = Trans
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtCabezaW)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM StockInsumos;", MiConexion)
                        DaP.SelectCommand.Transaction = Trans
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtStock)
                    End Using
                    Trans.Commit()
                    MsgBox("Transferencia Anulada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    PActualizacionOk = True
                    ArmaArchivos()
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                    MsgBox("Error Otro Usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Catch err As OleDb.OleDbException
                    Trans.Rollback()
                    MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Finally
                    Trans = Nothing
                End Try
            Catch err As OleDb.OleDbException
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End Try
        End Using

        DtCabezaW.Dispose()
        DtStock.Dispose()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        PTrans = 0
        UnaTransferenciaInsumo_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then
            MsgBox("No hay un registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If
        Grid.Rows.Remove(Grid.CurrentRow)

    End Sub
    Private Sub TextComprobante_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComprobante.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ArmaArchivos()

        Dim Row As DataRow

        ConexionTrans = Conexion

        DtTransCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM TransInsumoCabeza WHERE Transferencia = " & PTrans & ";", ConexionTrans, DtTransCabeza) Then Me.Close() : Exit Sub
        If PTrans <> 0 And DtTransCabeza.Rows.Count = 0 Then
            MsgBox("Transferencia No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PTrans = 0 Then
            Row = DtTransCabeza.NewRow()
            Row("Transferencia") = 0
            Row("Origen") = ComboDepositoOrigen.SelectedValue
            Row("Destino") = ComboDepositoDestino.SelectedValue
            Row("Fecha") = Now
            Row("Estado") = 1
            Row("Comentario") = ""
            DtTransCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtTransDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM TransInsumoDetalle WHERE Transferencia = " & PTrans & ";", ConexionTrans, DtTransDetalle) Then Me.Close() : Exit Sub

        DtGrid.Clear()

        For Each Row1 As DataRow In DtTransDetalle.Rows
            Row = DtGrid.NewRow()
            Row("OrdenCompra") = Row1("OrdenCompra")
            Row("Articulo") = Row1("Articulo")
            Row("Cantidad") = Row1("Cantidad")
            DtGrid.Rows.Add(Row)
        Next

        Grid.DataSource = DtGrid

        If PTrans = 0 Then
            TextComprobante.ReadOnly = False
            Grid.ReadOnly = False
            ButtonEliminarLinea.Enabled = True
            Grid.Columns("Stock").Visible = True
            Grid.Columns("Lupa").Visible = True
        Else
            TextComprobante.ReadOnly = True
            Grid.ReadOnly = True
            ButtonEliminarLinea.Enabled = False
            Grid.Columns("Stock").Visible = False
            Grid.Columns("Lupa").Visible = False
        End If

    End Sub
    Private Function MuestraCabeza() As Boolean

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtTransCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Transferencia")
        AddHandler Enlace.Format, AddressOf FormatComprobante
        TextComprobante.DataBindings.Clear()
        TextComprobante.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Origen")
        ComboDepositoOrigen.DataBindings.Clear()
        ComboDepositoOrigen.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Destino")
        ComboDepositoDestino.DataBindings.Clear()
        ComboDepositoDestino.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Return True

    End Function
    Private Sub FormatComprobante(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then Numero.Value = ""

    End Sub
    Private Function ActualizaArchivoAlta(ByVal Dt As DataTable) As Boolean

        Dim Sql As String
        Dim RowsBusqueda() As DataRow

        'deposito origen.
        For Each Row As DataRow In DtGrid.Rows
            If Not Row.RowState = DataRowState.Deleted Then
                Sql = "SELECT * FROM StockInsumos WHERE Articulo = " & Row("Articulo") & " AND OrdenCompra = " & Row("OrdenCompra") & _
                               " AND Deposito = " & ComboDepositoOrigen.SelectedValue & ";"
                If Not Tablas.Read(Sql, ConexionTrans, Dt) Then Return False
            End If
        Next
        For Each Row As DataRow In DtGrid.Rows
            If Not Row.RowState = DataRowState.Deleted Then
                RowsBusqueda = Dt.Select("Articulo = " & Row("Articulo") & " AND OrdenCompra = " & Row("OrdenCompra"))
                RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") - Row("Cantidad")
                If RowsBusqueda(0).Item("Stock") < 0 Then
                    MsgBox("Stock negativo en Stock Orden Compra " & Format(Row("OrdenCompra"), "0000-00000000") & " Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    Return False
                End If
            End If
        Next
        'deposito destino.
        For Each Row As DataRow In DtGrid.Rows
            If Not Row.RowState = DataRowState.Deleted Then
                Sql = "SELECT * FROM StockInsumos WHERE Articulo = " & Row("Articulo") & " AND OrdenCompra = " & Row("OrdenCompra") & _
                               " AND Deposito = " & ComboDepositoDestino.SelectedValue & ";"
                If Not Tablas.Read(Sql, ConexionTrans, Dt) Then Return False
            End If
        Next
        For Each Row As DataRow In DtGrid.Rows
            If Not Row.RowState = DataRowState.Deleted Then
                RowsBusqueda = Dt.Select("Articulo = " & Row("Articulo") & " AND OrdenCompra = " & Row("OrdenCompra") & " AND Deposito = " & ComboDepositoDestino.SelectedValue)
                If RowsBusqueda.Length <> 0 Then
                    RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") + Row("Cantidad")
                Else
                    Dim row2 As DataRow
                    row2 = Dt.NewRow()
                    row2("Articulo") = Row("Articulo")
                    row2("OrdenCompra") = Row("OrdenCompra")
                    row2("Deposito") = ComboDepositoDestino.SelectedValue
                    row2("Stock") = Row("Cantidad")
                    Dt.Rows.Add(row2)
                End If
            End If
        Next

        Return True

    End Function
    Private Function ActualizaArchivoBaja(ByVal dt As DataTable) As Boolean

        Dim Sql As String
        Dim RowsBusqueda() As DataRow

        'deposito origen.
        For Each Row As DataRow In DtGrid.Rows
            Sql = "SELECT * FROM StockInsumos WHERE Articulo = " & Row("Articulo") & " AND OrdenCompra = " & Row("OrdenCompra") & _
                               " AND Deposito = " & ComboDepositoOrigen.SelectedValue & ";"
            If Not Tablas.Read(Sql, ConexionTrans, dt) Then Return False
        Next
        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = dt.Select("Articulo = " & Row("Articulo") & " AND OrdenCompra = " & Row("OrdenCompra"))
            RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") + Row("Cantidad")
        Next
        'deposito destino.
        For Each Row As DataRow In DtGrid.Rows
            Sql = "SELECT * FROM StockInsumos WHERE Articulo = " & Row("Articulo") & " AND OrdenCompra = " & Row("OrdenCompra") & _
                               " AND Deposito = " & ComboDepositoDestino.SelectedValue & ";"
            If Not Tablas.Read(Sql, ConexionTrans, dt) Then Return False
        Next
        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = dt.Select("Articulo = " & Row("Articulo") & " AND OrdenCompra = " & Row("OrdenCompra") & " AND Deposito = " & ComboDepositoDestino.SelectedValue)
            RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") - Row("Cantidad")
            If RowsBusqueda(0).Item("Stock") < 0 Then
                MsgBox("Stock negativo en Stock Orden Compra " & Format(Row("OrdenCompra"), "0000-00000000") & " Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Return False
            End If
        Next

        Return True

    End Function
    Private Sub LlenaCombosGrid()

        Articulo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Insumos ORDER BY Nombre;")
        Dim Row As DataRow = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim OrdenCompra As DataColumn = New DataColumn("OrdenCompra")
        OrdenCompra.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(OrdenCompra)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Stock)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cantidad)

    End Sub
    Private Function Valida() As Boolean

        If TextComprobante.Text = "" Then
            MsgBox("Numero Comprobante Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Val(TextComprobante.Text) = 0 Then
            MsgBox("Numero Comprobante Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If ComboDepositoOrigen.Text.ToString.Trim.Length = 0 Then
            MsgBox("Falta Deposito Origen.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDepositoOrigen.Focus()
            Return False
        End If
        If ComboDepositoDestino.Text.ToString.Trim.Length = 0 Then
            MsgBox("Falta Deposito Destino.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDepositoDestino.Focus()
            Return False
        End If
        If ComboDepositoOrigen.SelectedValue = ComboDepositoDestino.SelectedValue Then
            MsgBox("Deposito Origen no debe ser igual as Deposito Destino.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDepositoOrigen.Focus()
            Return False
        End If

        If Grid.Rows.Count = 1 Then
            MsgBox("Falta Informar Articulos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If
        For i As Integer = 0 To Grid.RowCount - 2
            If Grid.Rows(i).Cells("Articulo").Value = 0 Then
                MsgBox("Debe Informar Articulo en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Not IsNumeric(Grid.Rows(i).Cells("Cantidad").Value) Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Grid.Rows(i).Cells("Cantidad").Value = 0 Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        Next

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
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "OrdenCompra" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = Format(e.Value, "00000000")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Stock" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 0)
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            e.Value = Nothing
        End If

    End Sub
    Private Sub Grid_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellValueChanged

        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            Grid.CurrentRow.Cells("Operacion").Value = 0
            Grid.CurrentRow.Cells("OrdenCompra").Value = 0
            Grid.CurrentRow.Cells("Cantidad").Value = 0
            Grid.CurrentRow.Cells("Stock").Value = 0
            Grid.Refresh()
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloEntero_KeyPress
        End If

    End Sub
    Private Sub SoloEntero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name <> "Lupa" Then Exit Sub

        If ComboDepositoOrigen.SelectedValue = 0 Then
            MsgBox("Falta Seleccionar depositos Origen.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Grid.Rows(e.RowIndex).Cells("Articulo").Value = 0 Then
            MsgBox("Falta Seleccionar Articulo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        SeleccionarVarios.PEsSeleccionaOrden = True
        SeleccionarVarios.PArticulo = Grid.Rows(e.RowIndex).Cells("Articulo").Value
        SeleccionarVarios.PDeposito = ComboDepositoOrigen.SelectedValue
        SeleccionarVarios.ShowDialog()
        If SeleccionarVarios.POrden <> 0 Then
            Grid.CurrentRow.Cells("OrdenCompra").Value = SeleccionarVarios.POrden
            Grid.CurrentRow.Cells("Stock").Value = SeleccionarVarios.PStock
        End If
        SeleccionarVarios.Dispose()

        Grid.CurrentCell = Grid.CurrentRow.Cells("Cantidad")
        Grid.Refresh()

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        If PTrans <> 0 Then e.Row.Delete()

        e.Row("OrdenCompra") = 0
        e.Row("Articulo") = 0
        e.Row("Stock") = 0
        e.Row("Cantidad") = 0

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("OrdenCompra")) Then
            If e.ProposedValue <> 0 Then
                For Each Row As DataRow In DtGrid.Rows
                    If Row("Articulo") = e.Row("Articulo") And Row("OrdenCompra") = e.ProposedValue Then
                        MsgBox("Insumo y Orden Compra Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Grid.Rows.Remove(Grid.CurrentRow)
                        Exit Sub
                    End If
                Next
            End If
        End If

        If (e.Column.ColumnName.Equals("Cantidad")) Then
            If PTrans = 0 Then
                If Not IsDBNull(e.Row("Cantidad")) Then
                    If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
                    If e.ProposedValue > e.Row("Stock") Then
                        MsgBox("Cantidad Supera Stock.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        e.ProposedValue = e.Row("Cantidad")
                        Exit Sub
                    End If
                End If
            End If
        End If

    End Sub

End Class