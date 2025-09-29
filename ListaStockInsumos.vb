Public Class ListaStockInsumos
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Private Sub ListaStockInsumos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50
        Grid.AutoGenerateColumns = False

        ComboEmisor.DataSource = ProveedoresDeInsumos()
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = 0
        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboInsumo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Insumos;")
        Dim Row As DataRow = ComboInsumo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboInsumo.DataSource.Rows.Add(Row)
        ComboInsumo.DisplayMember = "Nombre"
        ComboInsumo.ValueMember = "Clave"
        ComboInsumo.SelectedValue = 0
        With ComboInsumo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboDeposito, 20)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        CreaDtGrid()

        LlenaCombosGrid()

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaNotasTerceros_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        ComboInsumo.Focus()

    End Sub
    Private Sub ListaNotasTerceros_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        PreparaArchivos()

    End Sub
    Private Sub ComboInsumo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboInsumo.Validating

        If IsNothing(ComboInsumo.SelectedValue) Then ComboInsumo.SelectedValue = 0

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

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
    Private Sub PreparaArchivos()

        Dim SqlB As String = ""

        If ComboEmisor.SelectedValue <> 0 Then
            SqlB = "SELECT S.* FROM StockInsumos AS S INNER JOIN OrdenCompraCabeza AS O ON S.OrdenCompra = O.Orden AND O.Proveedor = " & ComboEmisor.SelectedValue & " WHERE "
        Else
            SqlB = "SELECT S.* FROM StockInsumos AS S WHERE "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "S.Deposito = " & ComboDeposito.SelectedValue & " "
        Else : SqlDeposito = "S.Deposito LIKE '%' "
        End If

        Dim SqlInsumo As String = ""
        If ComboInsumo.SelectedValue <> 0 Then
            SqlInsumo = "AND S.Articulo = " & ComboInsumo.SelectedValue & " "
        End If

        SqlB = SqlB & SqlDeposito & SqlInsumo

        DtGrid.Clear()

        Dim Dt As New DataTable
        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Articulo,Deposito"

        Dim TotalStock As Double = 0
        Dim TotalStockDepositoB As Double = 0
        Dim InsumoAnt As Integer = 0
        Dim DepositoAnt As Integer = 0

        For Each Row As DataRowView In View
            InsumoAnt = Row("Articulo")
            DepositoAnt = Row("Deposito")
            Exit For
        Next

        For Each Row As DataRowView In View
            If InsumoAnt <> Row("Articulo") Then
                CorteXDeposito(InsumoAnt, DepositoAnt, TotalStockDepositoB, TotalStock)
                CorteXInsumo(TotalStock)
                InsumoAnt = Row("Articulo")
                DepositoAnt = Row("Deposito")
            End If
            If DepositoAnt <> Row("Deposito") Then
                CorteXDeposito(InsumoAnt, DepositoAnt, TotalStockDepositoB, TotalStock)
                DepositoAnt = Row("Deposito")
            End If
            TotalStockDepositoB = TotalStockDepositoB + Row("Stock")
        Next
        If View.Count <> 0 Then CorteXDeposito(InsumoAnt, DepositoAnt, TotalStockDepositoB, TotalStock)
        CorteXInsumo(TotalStock)

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

    End Sub
    Private Sub CorteXDeposito(ByVal Insumo As Integer, ByVal Deposito As Integer, ByRef TotalDepositoB As Double, ByRef Total As Double)

        Dim Row1 As DataRow = DtGrid.NewRow
        Row1("Insumo") = Insumo
        Row1("Deposito") = Deposito
        Row1("Stock") = 0
        If TotalDepositoB <> 0 Then
            Row1("Stock") = TotalDepositoB
        End If
        If TotalDepositoB <> 0 Then DtGrid.Rows.Add(Row1)

        Total = Total + TotalDepositoB
        TotalDepositoB = 0

    End Sub
    Private Sub CorteXInsumo(ByRef Stock As Double)

        If Stock <> 0 Then DtGrid.Rows(DtGrid.Rows.Count - 1).Item("Total") = Stock
        Stock = 0

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Insumo As New DataColumn("Insumo")
        Insumo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Insumo)

        Dim Deposito As New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Stock)

        Dim Total As New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Total)

    End Sub
    Private Sub LlenaCombosGrid()

        Insumo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Insumos;")
        Insumo.DisplayMember = "Nombre"
        Insumo.ValueMember = "Clave"

        Deposito.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = " & 20 & " ORDER BY Nombre;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            UnaOrdenCompra.POrden = Grid.CurrentCell.Value
            UnaOrdenCompra.ShowDialog()
            If UnaOrdenCompra.PActualizacionOk Then PreparaArchivos()
            UnaOrdenCompra.Dispose()
            Exit Sub
        End If

    End Sub

End Class