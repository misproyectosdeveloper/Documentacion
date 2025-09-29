Public Class ListaClientes
    Dim Sql As String
    Private WithEvents bs As New BindingSource
    Private Sub ListaClientes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        ComboCliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes;")
        Dim Row As DataRow = ComboCliente.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboCliente.DataSource.Rows.Add(Row)
        ComboCliente.DisplayMember = "Nombre"
        ComboCliente.ValueMember = "Clave"
        ComboCliente.SelectedValue = 0
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Clientes WHERE Alias <> '';")
        Row = ComboAlias.DataSource.NewRow
        Row("Clave") = 0
        Row("Alias") = ""
        ComboAlias.DataSource.Rows.Add(Row)
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        GeneraCombosGrid()

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaClientes_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position
        Grid.Focus()

    End Sub
    Private Sub ListaClientes_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()
        Entrada.Activate()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If ComboCliente.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Cliente o Alias.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Sql = "SELECT Clave,Nombre,Alias,Cuit,Calle,Numero,Localidad,Provincia,Pais,DeOperacion,CanalVenta,CanalDistribucion FROM Clientes "

        Dim Cliente As Integer = 0
        If ComboCliente.SelectedValue <> 0 Then Cliente = ComboCliente.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Cliente = ComboAlias.SelectedValue

        Dim SqlCliente As String
        If Cliente = 0 Then
            Sqlcliente = "WHERE Clave LIKE '%' "
        Else
            Sqlcliente = "WHERE Clave = " & Cliente & " "
        End If

        Dim SqlDeOperacion As String
        If CheckClientesDeFacturacion.Checked Then
            SqlDeOperacion = " AND DeOperacion = 0 "
        End If
        If CheckClientesDeOperacion.Checked Then
            SqlDeOperacion = " AND DeOperacion = 1 "
        End If
        If CheckClientesDeFacturacion.Checked And CheckClientesDeOperacion.Checked Then
            SqlDeOperacion = ""
        End If
        Dim SqlOpr As String = ""
        If Not PermisoTotal Then
            SqlOpr = " AND Opr = 1 "
        End If

        Dim SqlOrder As String = "ORDER BY Nombre;"

        Sql = Sql & SqlCliente & SqlDeOperacion & SqlOpr & SqlOrder

        LLenaGrid()

    End Sub
    Private Sub LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Dt As New DataTable

        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Me.Close() : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = Dt

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        Dim Row As DataRowView
        Row = bs.Current

        If bs.Count > 0 Then
            If IsDBNull(Row("Clave")) Then Exit Sub
        Else : Exit Sub
        End If
        UnCliente.PCliente = Row("Clave")
        UnCliente.ShowDialog()
        If UnCliente.PActualizacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        UnCliente.Dispose()

    End Sub
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

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
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Clientes", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub GeneraCombosGrid()

        Provincia.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 31;")
        Dim Row As DataRow = Provincia.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Provincia.DataSource.Rows.Add(Row)
        Provincia.DisplayMember = "Nombre"
        Provincia.ValueMember = "Clave"

        Pais.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 28;")
        Row = Pais.DataSource.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Argentina"
        Pais.DataSource.Rows.Add(Row)
        Pais.DisplayMember = "Nombre"
        Pais.ValueMember = "Clave"

        CanalVenta.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = 23;")
        Row = CanalVenta.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        CanalVenta.DataSource.rows.add(Row)
        CanalVenta.DisplayMember = "Nombre"
        CanalVenta.ValueMember = "Clave"

        CanalDistribucion.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 45;")
        Row = CanalDistribucion.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        CanalDistribucion.DataSource.Rows.Add(Row)
        CanalDistribucion.DisplayMember = "Nombre"
        CanalDistribucion.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Or Grid.Columns(e.ColumnIndex).Name = "Cuit" Then
            If e.Value = 0 Then e.Value = ""
        End If

    End Sub

End Class