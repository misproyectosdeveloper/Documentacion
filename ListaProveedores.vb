Public Class ListaProveedores
    Dim Sql As String
    Private WithEvents bs As New BindingSource
    Private Sub ListaClientes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        ComboProveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion <> 4;")
        Dim Row As DataRow = ComboProveedor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboProveedor.DataSource.Rows.Add(Row)
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE TipoOperacion <> 4 AND Alias <> '';")
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

        ComboProducto.DataSource = ArmaProductos()
        Row = ComboProducto.DataSource.NewRow()
        Row("Codigo") = 0
        Row("Nombre") = ""
        ComboProducto.DataSource.Rows.Add(Row)
        ComboProducto.DisplayMember = "Nombre"
        ComboProducto.ValueMember = "Codigo"
        ComboProducto.SelectedValue = 0
        With ComboProducto
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        GeneraCombosGrid()

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If ComboProveedor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Proveedor o Alias.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Sql = "SELECT Clave,Nombre,Alias,Cuit,Localidad,Provincia,Pais,Producto FROM Proveedores WHERE TipoOperacion <> 4 "

        Dim Proveedor As Integer = 0
        If ComboProveedor.SelectedValue <> 0 Then Proveedor = ComboProveedor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Proveedor = ComboAlias.SelectedValue

        Dim SqlProveedor As String
        If Proveedor = 0 Then
            SqlProveedor = "AND Clave LIKE '%' "
        Else
            SqlProveedor = "AND Clave = " & Proveedor & " "
        End If

        Dim SqlProducto As String = ""
        If ComboProducto.SelectedValue <> 0 Then
            SqlProducto = "AND Producto = " & ComboProducto.SelectedValue & " "
        End If

        Dim SqlOpr As String = ""
        If Not PermisoTotal Then
            SqlOpr = " AND Opr = 1 "
        End If

        Dim SqlOrder As String = "ORDER BY Nombre;"

        Sql = Sql & SqlProveedor & SqlProducto & SqlOpr & SqlOrder

        LLenaGrid()

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

    End Sub
    Private Sub ComboProducto_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProducto.Validating

        If IsNothing(ComboProducto.SelectedValue) Then ComboProducto.SelectedValue = 0

    End Sub
    Private Sub ButtonExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "", "Listado de Proveedores")

        Me.Cursor = System.Windows.Forms.Cursors.Default

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
        UnProveedor.PProveedor = Row("Clave")
        UnProveedor.ShowDialog()
        If UnProveedor.PModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        UnProveedor.Dispose()

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

        Producto.DataSource = ArmaProductos()
        Producto.DisplayMember = "Nombre"
        Producto.ValueMember = "Codigo"

    End Sub

    
End Class