Public Class ListaRetencionesPorProvincia
    Public PEsEfectuada As Boolean
    ' 
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim Dt As DataTable
    Dim DtTipo As DataTable
    Dim DtProvincia As DataTable
    Dim View As DataView
    Private Sub ListaRetencionesPorProvincia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        DtTipo = New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 25;", Conexion, DtTipo) Then Me.Close() : Exit Sub
        Dim Row As DataRow = DtTipo.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        DtTipo.Rows.Add(Row)
        ComboTipo.DataSource = DtTipo
        ComboTipo.DisplayMember = "Nombre"
        ComboTipo.ValueMember = "Clave"
        ComboTipo.SelectedValue = 0
        With ComboTipo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        DtProvincia = New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 31;", Conexion, DtProvincia) Then Me.Close() : Exit Sub
        Row = DtProvincia.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        DtProvincia.Rows.Add(Row)
        Row = DtProvincia.NewRow
        Row("Clave") = -1
        Row("Nombre") = "TOTAL"
        DtProvincia.Rows.Add(Row)
        ComboProvincia.DataSource = DtProvincia
        ComboProvincia.DisplayMember = "Nombre"
        ComboProvincia.ValueMember = "Clave"
        ComboProvincia.SelectedValue = 0
        With ComboProvincia
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DateTimeDesde.Value = DateTimeHasta.Value.AddMonths(-1)

        ButtonAceptar_Click(Nothing, Nothing)

        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ListaRetenciones_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If IsNothing(ComboTipo.SelectedValue) Then ComboTipo.SelectedValue = 0
        If IsNothing(ComboProvincia.SelectedValue) Then ComboProvincia.SelectedValue = 0

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Hasta debe ser mayor o igual a Fecha Desde.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Exit Sub
        End If

        Dim SqlFecha As String
        SqlFecha = "R.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND R.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaContable As String
        SqlFechaContable = "R.FechaContable >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND R.FechaContable < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaReciboOficial As String
        SqlFechaReciboOficial = "R.FechaReciboOficial >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND R.FechaReciboOficial < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaComprobante As String
        SqlFechaComprobante = "D.FechaComprobante >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND D.FechaComprobante < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim Sql As String

        Sql = "SELECT P.Retencion,P.Provincia,P.Importe FROM RecibosRetenciones AS P INNER JOIN (RecibosCabeza AS R INNER JOIN RecibosDetallePago AS D ON R.TipoNota = D.TipoNota AND R.Nota = D.Nota) ON P.TipoNota = R.TipoNota AND P.Nota = R.Nota WHERE R.Estado = 1 AND P.TipoNota = 60 AND D.MedioPago = P.Retencion AND " & SqlFechaComprobante & _
                " UNION ALL " & _
              "SELECT P.Retencion,P.Provincia,P.Importe FROM RecibosRetenciones AS P INNER JOIN PrestamosMovimientoCabeza AS R ON P.Nota = R.Movimiento WHERE R.Estado = 1 AND P.TipoNota = 1010 AND " & SqlFecha & _
                " UNION ALL " & _
              "SELECT P.Retencion,P.Provincia,P.Importe FROM RecibosRetenciones AS P INNER JOIN GastosBancarioCabeza AS R ON P.Nota = R.Movimiento WHERE R.Estado = 1 AND P.TipoNota = 3000 AND " & SqlFechaContable & _
                " UNION ALL " & _
              "SELECT P.Retencion,P.Provincia,P.Importe FROM RecibosRetenciones AS P INNER JOIN RecibosCabeza AS R ON P.TipoNota = R.TipoNota AND P.Nota = R.Nota WHERE R.Estado = 1 AND (P.TipoNota = 50 OR P.TipoNota = 70 OR P.TipoNota = 500 OR P.TipoNota = 700) AND " & SqlFechaContable & _
                " UNION ALL " & _
              "SELECT P.Retencion,P.Provincia,P.Importe FROM RecibosRetenciones AS P INNER JOIN NVLPCabeza AS R ON P.Nota = R.Liquidacion WHERE R.Estado = 1 AND P.TipoNota = 800 AND " & SqlFechaContable & _
                " UNION ALL " & _
              "SELECT P.Retencion,P.Provincia,P.Importe FROM RecibosRetenciones AS P INNER JOIN PrestamosMovimientoCabeza AS R ON P.Nota = R.Movimiento WHERE R.Estado = 1 AND P.TipoNota = 1010 AND " & SqlFecha & _
                " UNION ALL " & _
              "SELECT P.Retencion,P.Provincia,P.Importe FROM RecibosRetenciones AS P INNER JOIN GastosBancarioCabeza AS R ON P.Nota = R.Movimiento WHERE R.Estado = 1 AND P.TipoNota = 3000 AND " & SqlFechaContable & ";"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()

        Dt = New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        View = New DataView
        View = Dt.DefaultView
        View.Sort = "Provincia,Retencion"

        Dim Row As DataRowView
        Dim Total As Double = 0
        Dim TotalProvincia As Double = 0
        Dim Importe As Double = 0
        Dim ProvinciaAnt As Integer = 0
        Dim RetencionAnt As Integer = 0

        For I As Integer = 0 To View.Count - 1
            Row = View(I)
            If MovimientoOk(Row) Then
                If ProvinciaAnt <> Row("Provincia") Then
                    If ProvinciaAnt <> 0 Then
                        CortePorRetencion(RetencionAnt, ProvinciaAnt, Importe, "")
                        Importe = 0
                        CortePorProvincia(ProvinciaAnt, TotalProvincia, "Total Provincia")
                        TotalProvincia = 0
                    End If
                    RetencionAnt = Row("Retencion")
                    ProvinciaAnt = Row("Provincia")
                End If
                If RetencionAnt <> Row("Retencion") Then
                    CortePorRetencion(RetencionAnt, ProvinciaAnt, Importe, "")
                    Importe = 0
                    RetencionAnt = Row("Retencion")
                End If
                Importe = Importe + Row("Importe")
                TotalProvincia = TotalProvincia + Row("Importe")
                Total = Total + Row("Importe")
            End If
        Next
        If Total <> 0 Then
            CortePorRetencion(RetencionAnt, ProvinciaAnt, Importe, "")
            CortePorProvincia(ProvinciaAnt, TotalProvincia, "Total Provincia")
            CortePorTotal(Total, "Total")
        End If

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PEsEfectuada Then
            GridAExcel(Grid, "PATAGONIA SUNRICE S.R.L.                     " & Date.Now, "", "Retenciones/Percepciones Por Provincia - PERIODO :           " & Format(DateTimeDesde.Value, "dd/MM/yyyy") & "      -      " & Format(DateTimeHasta.Value, "dd/MM/yyyy"))
        Else
            GridAExcel(Grid, "PATAGONIA SUNRICE S.R.L.                     " & Date.Now, "", "Retenciones/Percepciones Por Provincia - PERIODO :           " & Format(DateTimeDesde.Value, "dd/MM/yyyy") & "      -      " & Format(DateTimeHasta.Value, "dd/MM/yyyy"))
        End If

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
    Private Sub CortePorRetencion(ByVal Retencion As Integer, ByVal Provincia As Integer, ByVal Importe As Double, ByVal Cartel As String)

        Dim Row As DataRow = DtGrid.NewRow
        Row("Retencion") = Retencion
        Row("Provincia") = Provincia
        Row("Importe") = Importe
        DtGrid.Rows.Add(Row)

    End Sub
    Private Sub CortePorProvincia(ByVal Provincia As Integer, ByVal Importe As Double, ByVal Cartel As String)

        Dim Row As DataRow = DtGrid.NewRow
        Row("Estilo") = 1
        Row("Retencion") = 0
        Row("Provincia") = Provincia
        Row("Importe") = Importe
        DtGrid.Rows.Add(Row)

    End Sub
    Private Sub CortePorTotal(ByVal Importe As Double, ByVal Cartel As String)

        Dim Row As DataRow = DtGrid.NewRow
        DtGrid.Rows.Add(Row)

        Row = DtGrid.NewRow
        Row("Estilo") = 1
        Row("Retencion") = 0
        Row("Provincia") = -1
        Row("Importe") = Importe
        DtGrid.Rows.Add(Row)

    End Sub
    Private Function MovimientoOk(ByVal Row As DataRowView) As Boolean

        If ComboTipo.SelectedValue <> 0 And ComboTipo.SelectedValue <> Row("Retencion") Then Return False
        If ComboProvincia.SelectedValue <> 0 And ComboProvincia.SelectedValue <> Row("Provincia") Then Return False

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Estilo As DataColumn = New DataColumn("Estilo")
        Estilo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estilo)

        Dim Provincia As DataColumn = New DataColumn("Provincia")
        Provincia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Provincia)

        Dim Retencion As DataColumn = New DataColumn("Retencion")
        Retencion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Retencion)

        Dim Importe As DataColumn = New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

    End Sub
    Private Sub LlenaCombosGrid()

        Provincia.DataSource = DtProvincia
        Provincia.DisplayMember = "Nombre"
        Provincia.ValueMember = "Clave"

        Retencion.DataSource = DtTipo
        Retencion.DisplayMember = "Nombre"
        Retencion.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            If Not IsDBNull(e.Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Estilo").Value) Then
                    If Grid.Rows(e.RowIndex).Cells("Estilo").Value = 1 Then
                        Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Yellow
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
End Class