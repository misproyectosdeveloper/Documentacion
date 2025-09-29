Public Class ListaRecuperoSenia
    Private WithEvents bs As New BindingSource
    '
    Dim Dt As DataTable
    Dim SqlB As String
    Dim SqlN As String
    Dim NotaMaxima As Decimal
    Private Sub ListaRecuperoSenia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ComboProveedor.DataSource = Tablas.Leer("SELECT Nombre,Clave FROM Proveedores;")
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        Dim Row As DataRow = ComboProveedor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboProveedor.DataSource.rows.add(Row)
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = Tablas.Leer("SELECT Alias,Clave FROM Proveedores WHERE Alias <> '';")
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        Row = ComboAlias.DataSource.NewRow
        Row("Clave") = 0
        Row("Alias") = ""
        ComboAlias.DataSource.rows.add(Row)
        ComboAlias.SelectedValue = 0
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        TextCaja.Text = Format(GCaja, "000")
        If Not GCajaTotal Then TextCaja.Enabled = False

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Checked = False
        End If

        LlenaCombosGrid()

    End Sub
    Private Sub ListaNotasTerceros_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

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

        If ComboProveedor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Cliente o Alias.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not CheckUsados.Checked And Not CheckNoUsados.Checked Then
            CheckUsados.Checked = True
            CheckNoUsados.Checked = True
        End If

        SqlB = "SELECT 1 As Operacion,* FROM RecuperoSenia WHERE "
        SqlN = "SELECT 2 As Operacion,* FROM RecuperoSenia WHERE "

        Dim SqlFecha As String
        SqlFecha = "Fecha BETWEEN '" & Format(DateTimeDesde.Value, "yyyyMMdd 00:00:00") & "' AND '" & Format(DateTimeHasta.Value, "yyyyMMdd 23:59:59") & "' "

        Dim Proveedor As Integer = 0
        If ComboProveedor.SelectedValue <> 0 Then Proveedor = ComboProveedor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Proveedor = ComboAlias.SelectedValue

        Dim SqlProveedor As String = ""
        If Proveedor <> 0 Then
            SqlProveedor = "AND Proveedor = " & Proveedor & " "
        End If

        Dim SqlRecibo As String = ""
        If TextRecibo.Text <> "" Then
            SqlRecibo = "AND Nota = " & Val(TextRecibo.Text) & " "
        End If

        Dim SqlEstado As String = ""
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND Estado = " & ComboEstado.SelectedValue & " "
        End If

        If TextCaja.Text = "" Then TextCaja.Text = Format(GCaja, "000")

        Dim SqlCaja As String = ""
        If CInt(TextCaja.Text) <> 999 Then
            SqlCaja = " AND PaseCaja = " & CInt(TextCaja.Text)
        End If

        Dim SqlUsados As String = ""
        If Not (CheckUsados.Checked And CheckNoUsados.Checked) Then
            If CheckUsados.Checked Then SqlUsados = "AND Usado = 1 "
            If CheckNoUsados.Checked Then SqlUsados = "AND Usado = 0 "
        End If

        SqlB = SqlB & SqlFecha & SqlProveedor & SqlRecibo & SqlUsados & SqlCaja & SqlEstado & ";"
        SqlN = SqlN & SqlFecha & SqlProveedor & SqlRecibo & SqlUsados & SqlCaja & SqlEstado & ";"

        LLenaGrid()

    End Sub
    Private Sub TextRecibo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextRecibo.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

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

        GridAExcel(Grid, Date.Now, "Recupero Señas", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LLenaGrid()

        Dt = New DataTable

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim TotalGral As Decimal = 0
        NotaMaxima = 0

        For Each Row As DataRow In Dt.Rows
            If Row("Estado") = 1 Then TotalGral = TotalGral + Row("Importe")
            If Row("Nota") > NotaMaxima Then NotaMaxima = Row("Nota")
        Next

        NotaMaxima = NotaMaxima + 1
        Dim RowTotal As DataRow = Dt.NewRow
        RowTotal("Operacion") = 3
        RowTotal("Nota") = NotaMaxima
        RowTotal("Importe") = TotalGral
        RowTotal("Estado") = 0
        Dt.Rows.Add(RowTotal)

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Nota"

        Grid.DataSource = bs
        bs.DataSource = View

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

    End Sub
    Private Sub LlenaCombosGrid()

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        Proveedor.DataSource = Tablas.Leer("SELECT Nombre,Clave FROM Proveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Nota" Then
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
            If e.Value = NotaMaxima Then e.Value = "T O T A L"
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaVale" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Estado" Then
            If e.Value <> "Anulado" Then e.Value = ""
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        Dim Abierto As Boolean

        If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
            Abierto = True
        Else : Abierto = False
        End If

        UnRecuperoSenia.PAbierto = Abierto
        UnRecuperoSenia.PNota = Grid.Rows(e.RowIndex).Cells("Nota").Value
        UnRecuperoSenia.ShowDialog()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        UnRecuperoSenia.Dispose()

    End Sub
   
End Class