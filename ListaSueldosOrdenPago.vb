Public Class ListaSueldosOrdenPago
    Private WithEvents bs As New BindingSource
    '
    Dim Dt As DataTable
    Dim DtFormasPago As DataTable
    Dim DtGrid As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaSueldosOrdenPago_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        Dim DtLegajos As New DataTable
        If Not Tablas.Read("SELECT Legajo,RIGHT('0000' + CAST(Legajo AS varchar),4) + ' - ' + Apellidos + ' ' + Nombres As Con FROM Empleados WHERE Estado = 1;", Conexion, DtLegajos) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read("SELECT Legajo,RIGHT('0000' + CAST(Legajo AS varchar),4) + ' - ' + Apellidos + ' ' + Nombres As Con FROM Empleados WHERE Estado = 1;", ConexionN, DtLegajos) Then Exit Sub
        End If
        Dim Row As DataRow = DtLegajos.NewRow
        Row("Legajo") = 0
        Row("Con") = ""
        DtLegajos.Rows.Add(Row)
        ComboLegajo.DataSource = DtLegajos
        ComboLegajo.DisplayMember = "Con"
        ComboLegajo.ValueMember = "Legajo"
        ComboLegajo.SelectedValue = 0
        With ComboLegajo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
        End If

        GeneraCombosGrid()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 AS Operacion,C.* FROM SueldosMovimientoCabeza AS C WHERE TipoNota = 4010 AND "
        SqlN = "SELECT 2 AS Operacion,C.* FROM SueldosMovimientoCabeza AS C WHERE TipoNota = 4010 AND "

        Dim SqlFecha As String
        SqlFecha = "C.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlLegajo As String = ""
        If ComboLegajo.SelectedValue <> 0 Then
            SqlLegajo = "AND C.Legajo = " & ComboLegajo.SelectedValue
        End If

        SqlB = SqlB & SqlFecha & SqlLegajo & ";"
        SqlN = SqlN & SqlFecha & SqlLegajo & ";"

        LLenaGrid()

        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        Grid.Focus()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Sueldos", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub Textcomprobante_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

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
    Private Sub ComboLegajo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboLegajo.Validating

        If IsNothing(ComboLegajo.SelectedValue) Then ComboLegajo.SelectedValue = 0

    End Sub
    Private Sub LLenaGrid()

        CreaDtGrid()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        Dt = New DataTable
        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked And PermisoTotal Then
            Tablas.Read(SqlN, ConexionN, Dt)
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Movimiento,Fecha"

        Dim Row1 As DataRow

        For Each Row As DataRowView In View
            Row1 = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Movimiento") = Row("Movimiento")
            Row1("Legajo") = Row("Legajo")
            If Row("Legajo") < 5000 Then
                Row1("Nombre") = NombreLegajo(Row("Legajo"), Conexion)
            Else : Row1("Nombre") = NombreLegajo(Row("Legajo"), ConexionN)
            End If
            Row1("Fecha") = Row("Fecha")
            Row1("Caja") = Row("Caja")
            Row1("Importe") = Row("Importe")
            Row1("Estado") = Row("Estado")
            If Row("Estado") <> 3 Then Row1("Estado") = 0
            If CheckComentarios.Checked Then Row1("Comentario") = Row("Comentario") Else Row1("Comentario") = ""
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Movimiento As New DataColumn("Movimiento")
        Movimiento.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Movimiento)

        Dim Legajo As New DataColumn("Legajo")
        Legajo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Legajo)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Nombre)

        Dim Caja As New DataColumn("Caja")
        Caja.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Caja)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Comentario As New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comentario)

    End Sub
    Private Sub GeneraCombosGrid()

        Estado.DataSource = DtEstadoActivoYBaja()
        Dim Row As DataRow = Estado.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Estado.DataSource.Rows.Add(Row)
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Legajo" Then
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        Dim Abierto As Boolean
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        If EsReemplazoChequeSueldos(4010, Grid.CurrentRow.Cells("Comprobante").Value, Abierto) Then
            UnChequeReemplazo.PTipoNota = 4010
            UnChequeReemplazo.PNota = Grid.CurrentRow.Cells("Comprobante").Value
            UnChequeReemplazo.PAbierto = Abierto
            UnChequeReemplazo.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            Exit Sub
        End If

        UnaOrdenPagoSueldos.PNota = Grid.CurrentRow.Cells("Comprobante").Value
        UnaOrdenPagoSueldos.PTipoNota = 4010
        UnaOrdenPagoSueldos.PAbierto = Abierto
        UnaOrdenPagoSueldos.ShowDialog()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub


End Class