Public Class ListaNegociosYCosteos
    Dim Sql As String
    Dim DtGrid As DataTable
    Private WithEvents bs As New BindingSource
    Private Sub ListaNegociosYCosteos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.ReadOnly = True

        LlenaCombosGrid()

        ComboNegocio.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;")
        Dim Row As DataRow = ComboNegocio.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboNegocio.DataSource.rows.add(Row)
        ComboNegocio.DisplayMember = "Nombre"
        ComboNegocio.ValueMember = "Clave"
        ComboNegocio.SelectedValue = 0
        With ComboNegocio
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        With ComboCosteo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        DateTimeDesde.Value = DateTimeHasta.Value.AddMonths(-6)

        Dim UltimaFech As Integer = UltimaFecha(Conexion)
        If UltimaFech > Format(DateTimeHasta.Value, "yyyyMMdd") Then
            DateTimeHasta.Value = UltimaFech.ToString.Substring(6, 2) & "/" & UltimaFech.ToString.Substring(4, 2) & "/" & UltimaFech.ToString.Substring(0, 4)
        End If

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaNegociosYCosteos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        Dim SqlFecha As String
        SqlFecha = "IntFechaDesde BETWEEN " & Format(DateTimeDesde.Value, "yyyyMMdd") & " AND " & Format(DateTimeHasta.Value, "yyyyMMdd") & " "

        Dim SqlNegocio As String = ""
        If ComboNegocio.SelectedValue <> 0 Then
            SqlNegocio = "AND Negocio = " & ComboNegocio.SelectedValue & " "
        End If

        Dim SqlCosteo As String = ""
        If ComboCosteo.SelectedValue <> 0 Then
            SqlCosteo = "AND Costeo = " & ComboCosteo.SelectedValue & " "
        End If

        Dim SqlCerrado As String = ""
        If CheckCerrado.Checked And Not CheckAbierto.Checked Then SqlCerrado = " AND Cerrado = 1"
        If CheckAbierto.Checked And Not CheckCerrado.Checked Then SqlCerrado = " AND Cerrado = 0"

        Sql = "SELECT * FROM Costeos WHERE "

        Sql = Sql & SqlFecha & SqlCerrado & SqlNegocio & SqlCosteo & " ORDER BY Negocio,IntFechaDesde;"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not PreparaArchivos() Then Me.Close()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboNegocio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboNegocio.Validating

        If IsNothing(ComboNegocio.SelectedValue) Then ComboNegocio.SelectedValue = 0

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & ComboNegocio.SelectedValue & ";"
        ComboCosteo.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboCosteo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        ComboCosteo.DataSource.rows.add(Row)
        ComboCosteo.DisplayMember = "Nombre"
        ComboCosteo.ValueMember = "Costeo"
        ComboCosteo.SelectedValue = 0

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

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
    Private Function PreparaArchivos() As Boolean

        DtGrid = New DataTable
        If Not Tablas.Read(Sql, Conexion, DtGrid) Then Return False

        Grid.DataSource = bs
        bs.DataSource = DtGrid
        Grid.EndEdit()

        Return True

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Negocio.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;")
        Row = Negocio.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Negocio.DataSource.Rows.Add(Row)
        Negocio.DisplayMember = "Nombre"
        Negocio.ValueMember = "Clave"

        Costeo.DataSource = Tablas.Leer("SELECT Costeo,Nombre FROM Costeos;")
        Row = Costeo.DataSource.NewRow()
        Row("Costeo") = 0
        Row("Nombre") = " "
        Costeo.DataSource.Rows.Add(Row)
        Costeo.DisplayMember = "Nombre"
        Costeo.ValueMember = "Costeo"

        Especie.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 1;")
        Row = Especie.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Especie.DataSource.Rows.Add(Row)
        Especie.DisplayMember = "Nombre"
        Especie.ValueMember = "Clave"

        Variedad.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 2;")
        Row = Variedad.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Variedad.DataSource.Rows.Add(Row)
        Variedad.DisplayMember = "Nombre"
        Variedad.ValueMember = "Clave"

    End Sub
    Private Function UltimaFecha(ByVal ConexionStr) As Integer
        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(IntFechaDesde) FROM Costeos;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return -1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "FechaDesde" Or Grid.Columns(e.ColumnIndex).Name = "FechaHasta" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else : e.Value = e.Value.ToString.Substring(6, 2) & "/" & e.Value.ToString.Substring(4, 2) & "/" & e.Value.ToString.Substring(0, 4)
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Costo" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else : e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If IsDBNull(Grid.CurrentRow.Cells("Costeo").Value) Then Exit Sub

        UnCosteo.PCosteo = Grid.CurrentRow.Cells("Costeo").Value
        UnCosteo.ShowDialog()
        UnCosteo.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub

End Class