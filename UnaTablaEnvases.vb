'ejemplo AddHandler en datagridview  http://www.daniweb.com/forums/thread103297.html#
Public Class UnaTablaEnvases
    Dim DtEnvases As DataTable
    Private WithEvents bs As New BindingSource
    '
    Dim cb As ComboBox
    Private Sub Envases_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        Grid.Columns("TablaSeña").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")
        Grid.Columns("TablaDescarga").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")
        Grid.Columns("TablaCosto").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")

        LlenaCombosGrid()

        LLenaGrid()

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

        AddHandler DtEnvases.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtEnvases_ColumnChanging)
        AddHandler DtEnvases.ColumnChanged, New DataColumnChangeEventHandler(AddressOf DtEnvases_ColumnChanged)
        AddHandler DtEnvases.RowChanging, New DataRowChangeEventHandler(AddressOf DtEnvases_RowChanging)
        AddHandler DtEnvases.RowChanged, New DataRowChangeEventHandler(AddressOf DtEnvases_RowChanged)
        AddHandler DtEnvases.RowDeleting, New DataRowChangeEventHandler(AddressOf DtEnvases_Deleting)
        AddHandler DtEnvases.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtEnvases_NewRow)

    End Sub
    Private Sub Accesos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        If Not IsNothing(DtEnvases.GetChanges) And Not DtEnvases.HasErrors Then
            If MsgBox("Los cambios no fueron Actualizados. Quiere Actualizarlos?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                ButtonAceptar_Click(Nothing, Nothing)
            End If
        End If

        DtEnvases.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If IsNothing(DtEnvases.GetChanges) Then Exit Sub

        If DtEnvases.HasErrors Then
            MsgBox("Debe Corregir errores antes de Realizar los Cambios.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Dim Resul As Integer

        Resul = GrabaEnvases(DtEnvases)

        If Resul = -1 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -2 Then
            MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        Envases_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        If bs.Current Is Nothing Then
            MsgBox("No hay un registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not IsDBNull(Grid.CurrentRow.Cells("Clave").Value) Then
            If EnvaseUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                MsgBox("Item esta siendo usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
        End If

        If MsgBox("El Item se eliminara definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        bs.RemoveCurrent()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Grid.Focus()

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

        DtEnvases = New DataTable
        If Not Tablas.Read("SELECT * FROM Envases;", Conexion, DtEnvases) Then End : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = DtEnvases

        For Each Row As DataGridViewRow In Grid.Rows
            If IsDBNull(Row) Then Exit For
            Dim Valor As Double
            If Not BuscaVigencia(10, Now, Valor, Row.Cells("Clave").Value) Then End : Exit Sub
            Row.Cells("Seña").Value = FormatNumber(Valor, 4)
            If Not BuscaVigencia(11, Now, Valor, Row.Cells("Clave").Value) Then End : Exit Sub
            Row.Cells("Descarga").Value = FormatNumber(Valor, GDecimales)
            If Not BuscaVigencia(12, Now, Valor, Row.Cells("Clave").Value) Then End : Exit Sub
            Row.Cells("Costo").Value = FormatNumber(Valor, GDecimales)
        Next

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Dt As New DataTable

        ArmaDueño(Dt, True)
        Dueño.DataSource = Dt.Copy
        Dueño.DisplayMember = "Nombre"
        Dueño.ValueMember = "Tipo"

        ArmaCalculoFlete(Dt)
        CalculoFlete.DataSource = Dt.Copy
        CalculoFlete.DisplayMember = "Nombre"
        CalculoFlete.ValueMember = "Tipo"

        Unidad.Items.Clear()
        Unidad.Items.Add("")
        Unidad.Items.Add("Kgs")
        Unidad.Items.Add("Lts")
        Unidad.Items.Add("Mts")

        Dt.Dispose()

    End Sub
    Private Function EnvaseUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Clave) FROM Articulos WHERE Envase = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Dueño" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
                If cb.SelectedIndex < 0 Then cb.SelectedValue = 0
            Else
                Exit Sub
            End If
        End If

    End Sub
    Private Sub Grid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.CellBeginEdit

        If Grid.Columns(e.ColumnIndex).Name = "Nombre" Then
            '       If Not Grid.Rows(e.RowIndex).IsNewRow Then
            Dim row As DataRowView = bs.Current
            If row.Row.RowState <> DataRowState.Detached Then e.Cancel = True
            '   End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
            Exit Sub
        End If

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Nombre" Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloMayusculas_KeyPress
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Kilos" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloConDecimal_KeyPress
            AddHandler Texto.TextChanged, AddressOf TextoConDecimal_TextChanged
        End If

    End Sub
    Private Sub SoloConDecimal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(columna).Name = "Kilos" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub SoloMayusculas_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(columna).Name = "Nombre" Then
            e.KeyChar = e.KeyChar.ToString.ToUpper()
        End If

    End Sub
    Private Sub TextoConDecimal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(columna).Name = "Kilos" Then
            If Not IsNumeric(CType(sender, TextBox).Text) Then
                CType(sender, TextBox).Text = ""
                CType(sender, TextBox).Focus()
            End If
        End If

    End Sub
    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "TablaSeña" Then
            If IsDBNull(Grid.CurrentRow.Cells("Clave").Value) Then
                MsgBox("Debe  Aceptar Cambios Antes de Informar este Valor.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
            Vigencias.PCodigo = 10
            Vigencias.POrigen = Grid.CurrentRow.Cells("Clave").Value
            Vigencias.ShowDialog()
            If Vigencias.PActualizacionOk Then
                Dim Valor As Double
                If Not BuscaVigencia(10, Now, Valor, Grid.CurrentRow.Cells("Clave").Value) Then End
                Grid.CurrentRow.Cells("Seña").Value = FormatNumber(Valor, 4)
            End If
            Vigencias.Dispose()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "TablaDescarga" Then
            If IsDBNull(Grid.CurrentRow.Cells("Clave").Value) Then
                MsgBox("Debe Aceptar Cambios Antes de Informar este Valor.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
            Vigencias.PCodigo = 11
            Vigencias.POrigen = Grid.CurrentRow.Cells("Clave").Value
            Vigencias.ShowDialog()
            If Vigencias.PActualizacionOk Then
                Dim Valor As Double
                If Not BuscaVigencia(11, Now, Valor, Grid.CurrentRow.Cells("Clave").Value) Then End
                Grid.CurrentRow.Cells("Descarga").Value = FormatNumber(Valor, GDecimales)
            End If
            Vigencias.Dispose()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "TablaCosto" Then
            If IsDBNull(Grid.CurrentRow.Cells("Clave").Value) Then
                MsgBox("Debe  Aceptar Cambios  Antes de Informar este Valor.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
            Vigencias.PCodigo = 12
            Vigencias.POrigen = Grid.CurrentRow.Cells("Clave").Value
            Vigencias.ShowDialog()
            If Vigencias.PActualizacionOk Then
                Dim Valor As Double
                If Not BuscaVigencia(12, Now, Valor, Grid.CurrentRow.Cells("Clave").Value) Then End
                Grid.CurrentRow.Cells("Costo").Value = FormatNumber(Valor, GDecimales)
            End If
            Vigencias.Dispose()
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "TablaSeña" Or Grid.Columns(e.ColumnIndex).Name = "TablaDescarga" Or Grid.Columns(e.ColumnIndex).Name = "TablaCosto" Then
            e.Value = Nothing
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Kilos" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else : e.Value = Format(e.Value, "0.000")
                End If
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtEnvases_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("AGranel") = False

    End Sub
    Private Sub DtEnvases_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        e.Row.ClearErrors()

        If e.Column.ColumnName.Equals("Nombre") Then
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
            If e.ProposedValue.ToString.Trim = "" Then
                MsgBox("Nombre Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
        End If

        If e.Column.ColumnName.Equals("Kilos") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If e.ProposedValue = 0 Then
                MsgBox("Kilos Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Else
                e.ProposedValue = Trunca3(e.ProposedValue)
                If e.ProposedValue < 0.01 Then
                    MsgBox("Kilos debe ser mayor o igual a 0,01.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    e.ProposedValue = 0
                End If
                '    Dim Decimales() As String
                '    Decimales = Split(e.ProposedValue.ToString, ",")
                '    If Decimales.Length = 2 Then
                '        If Val(Decimales(1)) <> 50 And Val(Decimales(1)) <> 5 Then
                '            MsgBox("Decimal en Kilos debe ser 50.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                '            e.ProposedValue = 0
                '        End If
                '    End If
            End If
        End If

        If e.Column.ColumnName.Equals("Dueño") Then
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
            If e.ProposedValue.ToString.Trim = "" Then
                MsgBox("Dueño Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            End If
        End If

        If e.Column.ColumnName.Equals("Material") Then
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
            If e.ProposedValue.ToString.Trim = "" Then
                MsgBox("Material Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            End If
        End If

        Grid.Refresh()

    End Sub
    Private Sub DtEnvases_ColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)


    End Sub
    Private Sub DtEnvases_RowChanging(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        e.Row.RowError = ""
        If e.Row.RowState = DataRowState.Deleted Then Exit Sub
        '  If e.Row.GetColumnsInError.Length <> 0 Then e.Row.RowError = "Error." : Exit Sub

        If e.Row("Nombre").ToString.Trim = "" Then
            e.Row.RowError = "Error."
            MsgBox("Falta Nombre.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If
        If IsDBNull(e.Row("Kilos")) Then
            e.Row.RowError = "Error."
            MsgBox("Falta Kilos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If
        If e.Row("Kilos") = 0 Then
            e.Row.RowError = "Error."
            MsgBox("Falta Kilos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If
        If IsDBNull(e.Row("Unidad")) Then
            e.Row.RowError = "Error."
            MsgBox("Falta Unidad.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If
        If e.Row("AGranel") And e.Row("Kilos") <> 1 Then
            e.Row.RowError = "Error."
            MsgBox("Envase a Granel debe Contener 1 Unidad.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If
        If IsDBNull(e.Row("Dueño")) Then
            e.Row.RowError = "Error."
            MsgBox("Falta Dueño.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If
        If e.Row("Dueño") = 0 Then
            e.Row.RowError = "Error."
            MsgBox("Falta Dueño.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If
        If e.Row("Material").ToString.Trim = "" Then
            e.Row.RowError = "Error."
            MsgBox("Falta Material.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If
        If e.Row("CalculoFlete").ToString.Trim = "" Then
            e.Row.RowError = "Error."
            MsgBox("Falta Calculo Flete.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Dim RowsBusqueda() As DataRow
        Dim Kilos As Double = CDbl(e.Row("Kilos").ToString.Replace(",", "."))
        RowsBusqueda = DtEnvases.Select("Nombre = '" & e.Row("Nombre") & "' AND Kilos = " & Kilos & " AND Material = '" & e.Row("Material") & "' AND Dueño = " & e.Row("Dueño") & " AND CalculoFlete = " & e.Row("CalculoFlete"))
        If RowsBusqueda.Length > 0 Then
            If IsDBNull(e.Row("Clave")) Then
                e.Row.RowError = "Error"
                MsgBox("Envase Repetido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Else
                If e.Row("Clave") <> RowsBusqueda(0).Item("Clave") Then
                    e.Row.RowError = "Error"
                    MsgBox("Envase Repetido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                End If
            End If
        End If

    End Sub
    Private Sub DtEnvases_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        Grid.Refresh()

    End Sub
    Private Sub DtEnvases_Deleting(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        e.Row.RowError = ""

    End Sub

End Class