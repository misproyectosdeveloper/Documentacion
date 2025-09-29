Public Class OpcionLotesAImputar
    Public PEsPorCuentaYOrden As Boolean
    Public PDtGrid As DataTable
    Public PEmisor As Integer
    '
    Dim DtGrid As DataTable
    Private Sub OpcionLotesAImputar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        Grid.Columns("CandadoLote").DefaultCellStyle.NullValue = Nothing

        DtGrid = PDtGrid.Copy

        Grid.DataSource = DtGrid

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        PDtGrid.Clear()
        CopiaTabla(DtGrid, PDtGrid)

        Me.Close()

    End Sub
    Private Sub PictureLupa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupa.Click

        Dim ListaDeLotes As New List(Of FilaComprobanteFactura)

        For Each Row As DataRow In DtGrid.Rows
            Dim Item As New FilaComprobanteFactura
            Item.Lote = Row("Lote")
            Item.Secuencia = Row("Secuencia")
            ListaDeLotes.Add(Item)
        Next

        If PEsPorCuentaYOrden Then
            SeleccionaLotesAfectados.PEsPorCuentaYOrden = True
            SeleccionaLotesAfectados.PEmisor = PEmisor
            SeleccionaLotesAfectados.ComboProveedor.SelectedValue = PEmisor
            SeleccionaLotesAfectados.ComboProveedor.Enabled = False
        End If
        SeleccionaLotesAfectados.PEsRecibo = True
        SeleccionaLotesAfectados.CheckAbierto.Checked = True
        SeleccionaLotesAfectados.CheckCerrado.Checked = True
        SeleccionaLotesAfectados.PListaDeLotes = ListaDeLotes
        SeleccionaLotesAfectados.ShowDialog()
        If SeleccionaLotesAfectados.PAceptado Then
            ListaDeLotes = SeleccionaLotesAfectados.PListaDeLotes
            AgregaListaADtGrid(ListaDeLotes)
        End If
        SeleccionaLotesAfectados.Dispose()

    End Sub
    Private Sub ButtonBorraLineaLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorraLineaLotes.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        Dim Row As DataRow
        Row = DtGrid.Rows.Item(Grid.CurrentRow.Index)
        Row.Delete()

    End Sub
    Private Sub AgregaListaADtGrid(ByVal Lista As List(Of FilaComprobanteFactura))

        For Each Fila As FilaComprobanteFactura In Lista
            Dim Row As DataRow = DtGrid.NewRow
            Row("Operacion") = Fila.Operacion
            Row("Lote") = Fila.Lote
            Row("Secuencia") = Fila.Secuencia
            Row("Cantidad") = Fila.Cantidad
            DtGrid.Rows.Add(Row)
        Next

    End Sub

    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID de Lotes.  ---------------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridLotes_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("OperacionLote").Value = 1 Then Grid.Rows(e.RowIndex).Cells("CandadoLote").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("OperacionLote").Value = 2 Then Grid.Rows(e.RowIndex).Cells("CandadoLote").Value = ImageList1.Images.Item("Cerrado")
            End If
            If Grid.Rows(e.RowIndex).Cells("Lote").Value <> 0 Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
            End If
        End If

    End Sub



End Class