Public Class ListaOtrosProveedores

    Dim Sql As String
    Private WithEvents bs As New BindingSource
    Private Sub ListaOtrosProveedores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        LlenaCombo(ComboProveedor, "", "Proveedores")
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        GeneraCombosGrid()

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaOtrosProveedores_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Sql = "SELECT Clave,Nombre,Cuit,Localidad,Provincia,Pais FROM OtrosProveedores "

        Dim SqlProveedor As String
        If ComboProveedor.SelectedValue = 0 Then
            SqlProveedor = "WHERE Clave LIKE '%' "
        Else : SqlProveedor = "WHERE Clave = " & ComboProveedor.SelectedValue
        End If

        Sql = Sql & SqlProveedor & ";"

        LLenaGrid()

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0 : Exit Sub

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

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Dt As New DataTable

        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = Dt

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub GeneraCombosGrid()

        Provincia.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 31;")
        Provincia.DisplayMember = "Nombre"
        Provincia.ValueMember = "Clave"

        Pais.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 28;")
        Dim Row As DataRow = Pais.DataSource.NewRow()
        Row("Clave") = 1
        Row("Nombre") = "ARGENTINA"
        Pais.DataSource.Rows.Add(Row)
        Pais.DisplayMember = "Nombre"
        Pais.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Cuit" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "00-00000000-0")
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        UnOtroProveedor.PProveedor = Grid.CurrentRow.Cells("Clave").Value
        UnOtroProveedor.ShowDialog()
        UnOtroProveedor.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub

End Class