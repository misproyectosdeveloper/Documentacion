Public Class ListaOrdenDeCompra
    Public PTipo As Integer
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim Sql As String
    Private Sub ListaOrdenDeCompra_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        ComboEmisor.DataSource = ProveedoresTodos()
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = 0
        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        CreaDtGrid()

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaNotasTerceros_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        ComboEmisor.Focus()

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

        If Not CheckFinalizados.Checked And Not CheckEnProceso.Checked Then
            CheckFinalizados.Checked = True
            CheckEnProceso.Checked = True
        End If

        Sql = "SELECT * FROM OrdenCompraCabeza WHERE "

        Dim SqlFecha As String
        SqlFecha = "Fecha BETWEEN '" & Format(DateTimeDesde.Value, "yyyyMMdd 00:00:00") & "' AND '" & Format(DateTimeHasta.Value, "yyyyMMdd 23:59:59") & "' "

        Dim SqlProveedor As String = ""
        If ComboEmisor.SelectedValue <> 0 Then
            SqlProveedor = "AND Proveedor = " & ComboEmisor.SelectedValue & " "
        End If

        Dim SqlOrdenCompra As String = ""
        If TextOrdenCompra.Text <> "" Then
            SqlOrdenCompra = "AND Orden = " & Val(TextOrdenCompra.Text) & " "
        End If

        Dim SqlEstado As String = ""
        If CheckFinalizados.Checked And Not CheckEnProceso.Checked Or Not CheckFinalizados.Checked And CheckEnProceso.Checked Then
            SqlEstado = "AND Estado = 1 "
        End If

        Sql = Sql & SqlFecha & SqlProveedor & SqlOrdenCompra & SqlEstado & "ORDER BY Orden;"

        LLenaGrid()

    End Sub
    Private Sub TextOrdenCompra_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextOrdenCompra.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

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

        DtGrid.Clear()

        Dim Dt As New DataTable

        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Orden"

        Dim Completa As Boolean

        For Each Row As DataRowView In View
            Completa = OrdenCompraCompleta(Row("Orden"))
            If (CheckFinalizados.Checked And Completa) Or (CheckEnProceso.Checked And Not Completa) Or (CheckFinalizados.Checked And CheckEnProceso.Checked) Then
                AgregaADtGrid(Row("Tipo"), Row("Orden"), Row("Fecha"), Row("FechaEntrega"), Row("FechaEntregaHasta"), Row("Proveedor"), Row("Importe"), Row("Estado"), _
                                                Completa)
            End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        Dt.Dispose()

    End Sub
    Private Sub AgregaADtGrid(ByVal Tipo As Integer, ByVal Orden As Double, ByVal Fecha As DateTime, ByVal FechaEntrega As DateTime, ByVal FechaEntregaHasta As DateTime, ByVal Proveedor As Integer, ByVal Importe As Double, _
                          ByVal Estado As Integer, ByVal Completa As Boolean)

        Dim Row As DataRow

        Row = DtGrid.NewRow()
        Row("Tipo") = Tipo
        Row("Comprobante") = Orden
        Row("Fecha") = Format(Fecha, "dd/MM/yyyy")
        Row("FechaEntrega") = Format(FechaEntrega, "dd/MM/yyyy")
        Row("FechaEntregaHasta") = Format(FechaEntregaHasta, "dd/MM/yyyy")
        Row("Importe") = Importe
        Row("Proveedor") = Proveedor
        If Completa Then
            Row("Entregado") = "Finalizado"
        Else
            Row("Entregado") = "En proceso"
        End If
        Row("Estado") = 0
        If Estado <> 1 Then Row("Estado") = Estado
        DtGrid.Rows.Add(Row)

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Tipo)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim FechaEntrega As New DataColumn("FechaEntrega")
        FechaEntrega.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaEntrega)

        Dim FechaEntregaHasta As New DataColumn("FechaEntregaHasta")
        FechaEntregaHasta.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaEntregaHasta)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim Entregado As New DataColumn("Entregado")
        Entregado.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Entregado)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

    End Sub
    Private Sub LlenaCombosGrid()

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        Proveedor.DataSource = ProveedoresTodos()
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        Dim Dt As New DataTable

        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Dim Row As DataRow = Dt.NewRow
        Row("Tipo") = 1
        Row("Nombre") = "Articulo"
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Tipo") = 2
        Row("Nombre") = "Insumo"
        Dt.Rows.Add(Row)
        '
        Tipo.DataSource = Dt
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Tipo"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            e.Value = Format(e.Value, "00000000")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaEntrega" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            UnaOrdenCompra.PTipo = Grid.CurrentRow.Cells("Tipo").Value
            UnaOrdenCompra.POrden = Grid.CurrentCell.Value
            UnaOrdenCompra.ShowDialog()
            If UnaOrdenCompra.PActualizacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            UnaOrdenCompra.Dispose()
        End If

    End Sub


End Class