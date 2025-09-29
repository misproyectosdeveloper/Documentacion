Public Class ListaArticulosSecuencial
    Public PEsConStock As Boolean
    Public PEsServicios As Boolean
    Public PEsSecos As Boolean
    Private WithEvents bs As New BindingSource
    '
    Dim Dt As DataTable
    '
    Dim Sql As String
    Private Sub ListaArticulosAlfaveticamente_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        GeneraCombosGrid()

        ComboEspecie.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 1;")
        Dim Row As DataRow = ComboEspecie.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboEspecie.DataSource.Rows.Add(Row)
        ComboEspecie.DisplayMember = "Nombre"
        ComboEspecie.ValueMember = "Clave"
        ComboEspecie.SelectedValue = 0
        With ComboEspecie
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboVariedad.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 2;")
        Row = ComboVariedad.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboVariedad.DataSource.Rows.Add(Row)
        ComboVariedad.DisplayMember = "Nombre"
        ComboVariedad.ValueMember = "Clave"
        ComboVariedad.SelectedValue = 0
        With ComboVariedad
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboMarca.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 3;")
        Row = ComboMarca.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboMarca.DataSource.Rows.Add(Row)
        ComboMarca.DisplayMember = "Nombre"
        ComboMarca.ValueMember = "Clave"
        ComboMarca.SelectedValue = 0
        With ComboMarca
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboCategoria.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 4;")
        Row = ComboCategoria.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboCategoria.DataSource.Rows.Add(Row)
        ComboCategoria.DisplayMember = "Nombre"
        ComboCategoria.ValueMember = "Clave"
        ComboCategoria.SelectedValue = 0
        With ComboCategoria
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboEnvase.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Envases;")
        Row = ComboEnvase.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboEnvase.DataSource.Rows.Add(Row)
        ComboEnvase.DisplayMember = "Nombre"
        ComboEnvase.ValueMember = "Clave"
        ComboEnvase.SelectedValue = 0
        With ComboEnvase
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboSecundario.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Envases;")
        Row = ComboSecundario.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboSecundario.DataSource.Rows.Add(Row)
        ComboSecundario.DisplayMember = "Nombre"
        ComboSecundario.ValueMember = "Clave"
        ComboSecundario.SelectedValue = 0
        With ComboSecundario
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboEstado.DataSource = DtActivoDeshabilitado()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Codigo"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PEsServicios Or PEsSecos Then
            Grid.Columns("Especie").Visible = False
            Grid.Columns("Variedad").Visible = False
            Grid.Columns("Marca").Visible = False
            Grid.Columns("Categoria").Visible = False
            Grid.Columns("Envase").Visible = False
            Grid.Columns("Secundario").Visible = False
            Grid.Columns("CantidadPri").Visible = False
            Panel2.Visible = False
        Else
            Grid.Columns("Cuenta").Visible = False
        End If

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaArticulosAlfaveticamente_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PEsConStock Then
            Sql = "SELECT * FROM Articulos WHERE Clave LIKE '%' "
        End If
        If PEsServicios Then
            Sql = "SELECT * FROM ArticulosServicios WHERE Secos = 0 AND Clave LIKE '%' "
        End If
        If PEsSecos Then
            Sql = "SELECT * FROM ArticulosServicios WHERE Secos = 1 AND Clave LIKE '%' "
        End If

        Dim SqlEspecie As String
        If ComboEspecie.SelectedValue <> 0 Then
            SqlEspecie = "AND Especie = " & ComboEspecie.SelectedValue & " "
        End If

        Dim SqlVariedad As String
        If ComboVariedad.SelectedValue <> 0 Then
            SqlVariedad = "AND Variedad = " & ComboVariedad.SelectedValue & " "
        End If

        Dim SqlMarca As String
        If ComboMarca.SelectedValue <> 0 Then
            SqlMarca = "AND Marca = " & ComboMarca.SelectedValue & " "
        End If

        Dim SqlCategoria As String
        If ComboCategoria.SelectedValue <> 0 Then
            SqlCategoria = "AND Categoria = " & ComboCategoria.SelectedValue & " "
        End If

        Dim SqlEnvase As String
        If ComboEnvase.SelectedValue <> 0 Then
            SqlEnvase = "AND Envase = " & ComboEnvase.SelectedValue & " "
        End If

        Dim SqlSecundario As String
        If ComboSecundario.SelectedValue <> 0 Then
            SqlSecundario = "AND Secundario = " & ComboSecundario.SelectedValue & " "
        End If

        Dim SqlEstado As String
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND Estado = " & ComboEstado.SelectedValue & " "
        End If

        Sql = Sql & SqlEspecie & SqlVariedad & SqlMarca & SqlCategoria & SqlEnvase & SqlSecundario & SqlEstado & " ORDER BY Nombre;"


        LLenaGrid()

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
    Private Sub ButtonExportaExxel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportaExxel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, "Articulos", "", "")

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
    Private Sub GeneraCombosGrid()

        Dim Row As DataRow

        Especie.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 1;")
        Row = Especie.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Especie.DataSource.Rows.Add(Row)
        Especie.DisplayMember = "Nombre"
        Especie.ValueMember = "Clave"

        Variedad.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 2;")
        Row = Variedad.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Variedad.DataSource.Rows.Add(Row)
        Variedad.DisplayMember = "Nombre"
        Variedad.ValueMember = "Clave"

        Marca.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 3;")
        Row = Marca.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Marca.DataSource.Rows.Add(Row)
        Marca.DisplayMember = "Nombre"
        Marca.ValueMember = "Clave"

        Categoria.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 4;")
        Row = Categoria.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Categoria.DataSource.Rows.Add(Row)
        Categoria.DisplayMember = "Nombre"
        Categoria.ValueMember = "Clave"

        Envase.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Envases;")
        Row = Envase.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Envase.DataSource.Rows.Add(Row)
        Envase.DisplayMember = "Nombre"
        Envase.ValueMember = "Clave"

        Secundario.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Envases;")
        Row = Secundario.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Secundario.DataSource.Rows.Add(Row)
        Secundario.DisplayMember = "Nombre"
        Secundario.ValueMember = "Clave"

        Estado.DataSource = DtActivoDeshabilitado()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Codigo"

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Cuenta" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "000-000000-00")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Iva" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, 2)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "CantidadPri" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else : e.Value = FormatNumber(e.Value, 2)
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Estado" Then
            If Not IsDBNull(e.Value) Then
                If Grid.Rows(e.RowIndex).Cells("Estado").Value = 1 Then e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "EAN" Then
            If Not IsDBNull(e.Value) Then
                If Grid.Rows(e.RowIndex).Cells("EAN").Value = 0 Then e.Value = Nothing
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        Dim Row As DataRowView
        Row = bs.Current

        If bs.Count > 0 Then
            If IsDBNull(Row("Clave")) Then Exit Sub
        Else : Exit Sub
        End If

        If PEsConStock Then
            GEnvase = Row("Envase")
            GCategoria = Row("Categoria")
            GMarca = Row("Marca")
            GVariedad = Row("Variedad")
            GEspecie = Row("Especie")
            UnArticulo.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        Else
            UnArticulo.PEsServicios = PEsServicios
            UnArticulo.PEsSecos = PEsSecos
            UnArticulo.PClave = Row("Clave")
            UnArticulo.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If
    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError

    End Sub
End Class