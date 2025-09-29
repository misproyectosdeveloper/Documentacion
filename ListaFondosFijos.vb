Public Class ListaFondosFijos
    Public PBloqueaFunciones As Boolean
    '
    Dim Dt As DataTable
    Private WithEvents bs As New BindingSource
    Private Sub UnaTablaFondosFijos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 40

        Grid.AutoGenerateColumns = False

        ComboFondoFijo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM MaestroFondoFijo;")
        Dim Row As DataRow = ComboFondoFijo.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboFondoFijo.DataSource.rows.add(Row)
        ComboFondoFijo.DisplayMember = "Nombre"
        ComboFondoFijo.ValueMember = "Clave"
        ComboFondoFijo.SelectedValue = 0
        With ComboFondoFijo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        LLenaGrid()

        If Not PermisoTotal Then
            Grid.Columns("Candado").Visible = False
        End If

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonSeleccionar_Click(Nothing, Nothing)

    End Sub
    Private Sub UnaTablaFondosFijos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSeleccionar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If TextNumero.Text <> "" Then
            If CInt(TextNumero.Text) = 0 Then
                MsgBox("Numero No debe ser igual a 0.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                TextNumero.Text = ""
                Exit Sub
            End If
        End If

        LLenaGrid()

    End Sub
    Private Sub ComboFondoFijo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboFondoFijo.Validating

        If IsNothing(ComboFondoFijo.SelectedValue) Then ComboFondoFijo.SelectedValue = 0

    End Sub
    Private Sub TextNumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumero.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, 0)

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

        Dim SqlFecha As String
        SqlFecha = "Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlFondoFijo As String = ""
        If ComboFondoFijo.SelectedValue <> 0 Then
            SqlFondoFijo = " AND FondoFijo = " & ComboFondoFijo.SelectedValue
        End If

        Dim SqlNumero As String = ""
        If TextNumero.Text <> "" Then
            SqlNumero = " AND Numero = " & CInt(TextNumero.Text)
        End If

        Dim SqlB As String = "SELECT 1 AS Operacion, * FROM FondosFijos WHERE " & SqlFecha & SqlFondoFijo & SqlNumero & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion, * FROM FondosFijos WHERE " & SqlFecha & SqlFondoFijo & SqlNumero & ";"

        Dt = New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Tablas.Read(SqlB, Conexion, Dt) Then End : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then End : Exit Sub
        End If

        Grid.DataSource = bs
        bs.DataSource = Dt

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LlenaCombosGrid()

        FondoFijo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM MaestroFondoFijo;")
        Dim Row As DataRow = FondoFijo.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        FondoFijo.DataSource.rows.add(Row)
        FondoFijo.DisplayMember = "Nombre"
        FondoFijo.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Then
            e.Value = FormatNumber(e.Value, 0)
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        UnFondoFijo.PNumero = Grid.CurrentRow.Cells("Numero").Value
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            UnFondoFijo.PAbierto = True
        Else : UnFondoFijo.PAbierto = False
        End If
        UnFondoFijo.ShowDialog()
        UnFondoFijo.Dispose()
        If GModificacionOk Then LLenaGrid()

    End Sub
End Class
