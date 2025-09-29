Public Class UnaTablaArticulosCliente
    Public PCliente As Integer
    Public PTieneCodigo As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Dim Dt As DataTable
    Private WithEvents bs As New BindingSource
    Dim cb As ComboBox
    Private Sub UnaTablaArticulosCliente_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        LlenaCombosGrid()

        LLenaGrid()

        AddHandler Dt.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dt_ColumnChanging)
        AddHandler Dt.TableNewRow, New DataTableNewRowEventHandler(AddressOf Dt_NewRow)
        AddHandler Dt.RowDeleting, New DataRowChangeEventHandler(AddressOf Dt_Deleting)
        AddHandler Dt.RowChanged, New DataRowChangeEventHandler(AddressOf Dt_RowChanged)

    End Sub
    Private Sub UnaTablaArticulosCliente_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If Not IsNothing(Dt.GetChanges) And Not PBloqueaFunciones Then
            If MsgBox("Los cambios no fueron Actualizados. Quiere Actualizarlos?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                ButtonAceptar_Click(Nothing, Nothing)
                If Not GModificacionOk Then e.Cancel = True : Exit Sub
            End If
        End If

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        bs.EndEdit()

        If IsNothing(Dt.GetChanges) Then Exit Sub

        If Dt.HasErrors Then
            MsgBox("Debe Corregir Errores antes de Realizar los Cambios.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GModificacionOk = False

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM CodigosCliente;", Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(Dt.GetChanges)
                End Using
                MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                GModificacionOk = True
            End Using
        Catch ex As OleDb.OleDbException
            MsgBox("Error de Base de datos. Operación se CANCELA." & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        Catch ex As DBConcurrencyException
            MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        Finally
        End Try

        Me.Cursor = System.Windows.Forms.Cursors.Default

        UnaTablaArticulosCliente_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        If bs.Current Is Nothing Then
            MsgBox("No hay registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Dim Row As DataRowView = bs.Current

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        bs.RemoveCurrent()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportar.Click

        If Dt.Rows.Count <> 0 Then
            If MsgBox("Existe Articulos. Desa Continuar igualmente (Los Articulos se Mantendrán)?.", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Cliente As Integer = 0
        Dim Numero As Integer = 0
        Dim RowsBusqueda() As DataRow

        OpcionEmisor.PEsCliente = True
        OpcionEmisor.PEsSinCandado = True
        OpcionEmisor.ShowDialog()
        Cliente = OpcionEmisor.PEmisor
        OpcionEmisor.Dispose()
        If Cliente = 0 Then Exit Sub

        Dim DtAux As New DataTable
        If Not Tablas.Read("SELECT C.* FROM CodigosCliente AS C INNER JOIN Articulos As A ON C.Articulo = A.Clave WHERE A.Estado = 1 AND Cliente = " & Cliente & ";", Conexion, DtAux) Then Me.Close() : Exit Sub
        If DtAux.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No Existe Articulos para el Cliente.")
            DtAux.Dispose()
            Exit Sub
        End If

        For Each Row As DataRow In DtAux.Rows
            RowsBusqueda = Dt.Select("Articulo = " & Row("Articulo"))
            If RowsBusqueda.Length = 0 Then
                Dim Row1 As DataRow = Dt.NewRow
                For I As Integer = 0 To DtAux.Columns.Count - 1
                    Row1(I) = Row(I)
                Next
                Row1("Cliente") = PCliente
                Row1("CodigoDeCliente") = ""
                Row1("CodigoCliente") = 0
                Dt.Rows.Add(Row1)
            End If
        Next

        Grid.Refresh()
        DtAux.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUltimo.Click

        bs.MoveLast()

    End Sub
    Private Sub ButtonAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnterior.Click

        bs.MovePrevious()

    End Sub
    Private Sub ButtonPosterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPosterior.Click

        bs.MoveNext()

    End Sub
    Private Sub ButtonPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPrimero.Click

        bs.MoveFirst()

    End Sub
    Private Sub LLenaGrid()

        Dt = New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Tablas.Read("SELECT * FROM CodigosCliente AS C INNER JOIN Articulos AS A ON A.Clave = C.Articulo WHERE A.Estado = 1 AND Cliente = " & PCliente & " ORDER BY A.Nombre;", Conexion, Dt) Then End : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = Dt

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LlenaCombosGrid()

        Articulo.DataSource = ArticulosActivos()
        Dim Row As DataRow = Articulo.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "EAN" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = Format(e.Value, "0000000000000")
                End If
            End If
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
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
        End If

        If Grid.Columns(columna).Name = "EAN" Or Grid.Columns(columna).Name = "CodigoCliente" Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        End If

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "CodigoCliente" Then
            If e.KeyChar = " " Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "EAN" Then
            If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Dt_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Cliente") = PCliente
        e.Row("Articulo") = 0
        e.Row("CodigoDeCliente") = ""
        e.Row("CodigoCliente") = 0
        e.Row("EAN") = 0

    End Sub
    Private Sub Dt_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        Dim RowsBusqueda() As DataRow

        If e.Column.ColumnName.Equals("Articulo") Then
            If IsNothing(e.ProposedValue) Then e.ProposedValue = 0
            RowsBusqueda = Dt.Select("Articulo = " & e.ProposedValue)
            If RowsBusqueda.Length <> 0 Then
                MsgBox("Articulo Repetido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                e.ProposedValue = e.Row("Articulo")
            End If
        End If

        If e.Column.ColumnName.Equals("CodigoDeCliente") Then
            If IsDBNull(e.ProposedValue) Or IsNothing(e.ProposedValue) Then e.ProposedValue = "" : Exit Sub
            If StringSonCeros(e.ProposedValue) Then e.ProposedValue = ""
        End If

        If e.Column.ColumnName.Equals("EAN") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

    End Sub
    Private Sub Dt_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        Dim RowsBusqueda(0) As DataRow

        e.Row.RowError = ""
        If e.Row.RowState = DataRowState.Deleted Then Exit Sub
        If e.Row.GetColumnsInError.Length <> 0 Then e.Row.RowError = "Error." : Exit Sub

        If e.Row("Articulo") = 0 Then
            e.Row.RowError = "Error."
            MsgBox("Falta Articulo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If

        If e.Row("CodigoDeCliente") = "" And PTieneCodigo Then
            e.Row.RowError = "Error."
            MsgBox("Falta Codigo Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If
        RowsBusqueda = Dt.Select("CodigoDeCliente = '" & e.Row("CodigoDeCliente") & "'")
        If RowsBusqueda.Length > 1 Then
            If MsgBox("Codigo " & e.Row("CodigoDeCliente") & " Ya Existe. Quiere Continuar de todos Modos? (Si/No)  ", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                e.Row.RowError = "Error."
                Grid.Refresh()
                Exit Sub
            End If
        End If

        If e.Row("EAN") <> 0 Then
            If e.Row("EAN").ToString.Length <> 13 Then
                e.Row.RowError = "Error."
                MsgBox("EAN debe tenet 13 digitos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.Refresh()
                Exit Sub
            End If
            RowsBusqueda = Dt.Select("EAN = " & e.Row("EAN") & " AND EAN <> 0")
            If RowsBusqueda.Length > 1 Then
                If MsgBox("EAN " & e.Row("EAN") & " Ya Existe. Quiere Continuar de todos Modos? (Si/No)  ", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    e.Row.RowError = "Error."
                    Grid.Refresh()
                    Exit Sub
                End If
            End If
        End If

    End Sub
    Private Sub Dt_Deleting(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        e.Row.RowError = ""

    End Sub

End Class