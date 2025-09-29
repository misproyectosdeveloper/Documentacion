Public Class UnResumenChequesDiario
    Private Sub UnResumenChequesDiario_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FechaAnt As DateTime = "01/01/1800"
        Dim Acumulado As Double = 0
        Dim Color As Integer

        For Each Row As DataRow In UnChequesPropios.DtGrid.Rows
            If Not IsDBNull(Row("Vencimiento")) Then
                If Row("FechaDeposito") = "01/01/1800" And Row("Cartel") = "" And Row("MedioPago") = 2 Then
                    If FechaAnt <> Row("Vencimiento") Then
                        If FechaAnt <> "01/01/1800" Then
                            Grid.Rows.Add(HallaColor(FechaAnt), FechaAnt, Acumulado)
                        End If
                        FechaAnt = Row("Vencimiento")
                        Acumulado = 0
                    End If
                    Acumulado = Acumulado + Row("Importe")
                End If
            End If
        Next
        If FechaAnt <> "01/01/1800" Then Grid.Rows.Add(HallaColor(FechaAnt), FechaAnt, Acumulado)

    End Sub
    Private Sub UnResumenChequesDiario_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Resumen Cheques Diarios", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function HallaColor(ByVal Fecha As DateTime) As Integer

        Select Case DatePart(DateInterval.Weekday, Fecha)
            Case 7, 1
                Return 1
            Case Else
                Return 0
        End Select

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            If Not IsDBNull(e.Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then
                If Grid.Rows(e.RowIndex).Cells("Color").Value = 1 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.Red
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If

    End Sub

End Class