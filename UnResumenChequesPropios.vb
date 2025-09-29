Public Class UnResumenChequesPropios
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    Private Sub UnResumenChequesPropios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        Dim Acumulado As Decimal = 0
        Dim Emitido As Decimal = 0
        Dim SinDepositar As Decimal = 0
        Dim VencidosSinDepositar As Decimal = 0
        Dim Row1 As DataRow
        Dim x

        CreaDtGrid()

        For Each Row As DataRow In UnChequesPropios.DtTotales.Rows
            Emitido = Emitido + Row("Emitido")
            SinDepositar = SinDepositar + Row("Emitido") - Row("Depositado") - Row("Vencido")
            VencidosSinDepositar = VencidosSinDepositar + Row("Vencido")
            Row1 = DtGrid.NewRow
            Row1("Semana") = Row("Semana")
            Row1("Emitido") = Row("Emitido")
            Row1("SinDepositar") = Row("Emitido") - Row("Depositado") - Row("Vencido")
            Row1("Vencidos") = Row("Vencido")
            Row1("Desde") = Format(Row("Desde"), "dd/MM/yyyy")
            Row1("Hasta") = Format(Row("Hasta"), "dd/MM/yyyy")
            If Row1("Emitido") = 0 And Row1("SinDepositar") = 0 Then
                Row1("Acumulado") = 0
            Else : Row1("Acumulado") = SinDepositar
            End If
            DtGrid.Rows.Add(Row1)
        Next
        Row1 = DtGrid.NewRow
        Row1("Semana") = 9999
        Row1("Emitido") = Emitido
        Row1("SinDepositar") = SinDepositar
        Row1("Vencidos") = VencidosSinDepositar
        Row1("Acumulado") = 0
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = bs
        bs.DataSource = DtGrid

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

        GridAExcel(Grid, Date.Now, "Resumen Cheques Semanales", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Semana As New DataColumn("Semana")
        Semana.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Semana)

        Dim Emitido As New DataColumn("Emitido")
        Emitido.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Emitido)

        Dim SinDepositar As New DataColumn("SinDepositar")
        SinDepositar.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(SinDepositar)

        Dim Vencidos As New DataColumn("Vencidos")
        Vencidos.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Vencidos)

        Dim Acumulado As New DataColumn("Acumulado")
        Acumulado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Acumulado)

        Dim Desde As New DataColumn("Desde")
        Desde.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Desde)

        Dim Hasta As New DataColumn("Hasta")
        Hasta.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Hasta)

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Semana" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = "Saldo Ant."
                    Grid.Rows(e.RowIndex).Cells("Desde").Value = ""
                    Grid.Rows(e.RowIndex).Cells("Hasta").Value = ""
                    Exit Sub
                End If
                If e.Value = 9999 Then
                    e.Value = "Totales"
                    Grid.Rows(e.RowIndex).Cells("Desde").Value = ""
                    Grid.Rows(e.RowIndex).Cells("Hasta").Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "SinDepositar" Or Grid.Columns(e.ColumnIndex).Name = "Acumulado" Or Grid.Columns(e.ColumnIndex).Name = "Emitido" Or Grid.Columns(e.ColumnIndex).Name = "Vencidos" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

    End Sub
End Class