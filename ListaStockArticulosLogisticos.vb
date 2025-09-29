Public Class ListaStockArticulosLogisticos
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Private Sub ListaStockArticulosLogisticos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ComboInsumos.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 6;")
        Dim Row As DataRow = ComboInsumos.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboInsumos.DataSource.Rows.Add(Row)
        ComboInsumos.DisplayMember = "Nombre"
        ComboInsumos.ValueMember = "Clave"
        ComboInsumos.SelectedValue = 0
        With ComboInsumos
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
    Private Sub ListaStockArticulosLogisticos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        PreparaArchivos()

    End Sub
    Private Sub ComboArticulos_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboInsumos.Validating

        If IsNothing(ComboInsumos.SelectedValue) Then ComboInsumos.SelectedValue = 0

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
        Dim SqlN As String = ""

        SqlB = "SELECT 1 AS Operacion,* FROM StockArticulosLogisticos WHERE "
        SqlN = "SELECT 2 AS Operacion,* FROM StockArticulosLogisticos WHERE "

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "Deposito = " & ComboDeposito.SelectedValue & " "
        Else : SqlDeposito = "Deposito LIKE '%' "
        End If

        Dim SqlInsumos As String = ""
        If ComboInsumos.SelectedValue <> 0 Then
            SqlInsumos = "AND Articulo = " & ComboInsumos.SelectedValue & " "
        End If

        SqlB = SqlB & SqlDeposito & SqlInsumos
        SqlN = SqlN & SqlDeposito & SqlInsumos

        DtGrid.Clear()

        Dim Dt As New DataTable
        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Articulo,Deposito,Operacion"

        Dim TotalStock As Double = 0
        Dim TotalStockDepositoB As Double = 0
        Dim TotalStockDepositoN As Double = 0
        Dim InsumoAnt As Integer = 0
        Dim DepositoAnt As Integer = 0

        For Each Row As DataRowView In View
            InsumoAnt = Row("Articulo")
            DepositoAnt = Row("Deposito") & Row("Operacion")
            Exit For
        Next

        For Each Row As DataRowView In View
            If InsumoAnt <> Row("Articulo") Then
                CorteXDeposito(InsumoAnt, DepositoAnt, TotalStockDepositoB, TotalStockDepositoN, TotalStock)
                CorteXInsumo(TotalStock)
                InsumoAnt = Row("Articulo")
                DepositoAnt = Row("Deposito") & Row("Operacion")
            End If
            If DepositoAnt <> Row("Deposito") & Row("Operacion") Then
                CorteXDeposito(InsumoAnt, DepositoAnt, TotalStockDepositoB, TotalStockDepositoN, TotalStock)
                DepositoAnt = Row("Deposito") & Row("Operacion")
            End If
            If Row("Operacion") = 1 Then
                TotalStockDepositoB = TotalStockDepositoB + Row("Stock")
            Else : TotalStockDepositoN = TotalStockDepositoN + Row("Stock")
            End If
        Next
        If View.Count <> 0 Then CorteXDeposito(InsumoAnt, DepositoAnt, TotalStockDepositoB, TotalStockDepositoN, TotalStock)
        CorteXInsumo(TotalStock)

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

    End Sub
    Private Sub CorteXDeposito(ByVal Insumo As Integer, ByVal Deposito As Integer, ByRef TotalDepositoB As Double, ByRef TotalDepositoN As Double, ByRef Total As Double)

        Dim Row1 As DataRow = DtGrid.NewRow
        Row1("Insumo") = Insumo
        Row1("Deposito") = Strings.Left(Deposito.ToString, Deposito.ToString.Length - 1)
        Row1("Stock") = 0
        If TotalDepositoB <> 0 Then
            Row1("Operacion") = 1
            Row1("Stock") = TotalDepositoB
        End If
        If TotalDepositoN <> 0 Then
            Row1("Operacion") = 2
            Row1("Stock") = TotalDepositoN
        End If
        If TotalDepositoB <> 0 Or TotalDepositoN <> 0 Then DtGrid.Rows.Add(Row1)

        Total = Total + TotalDepositoB + TotalDepositoN
        TotalDepositoB = 0
        TotalDepositoN = 0

    End Sub
    Private Sub CorteXInsumo(ByRef Stock As Double)

        If Stock <> 0 Then DtGrid.Rows(DtGrid.Rows.Count - 1).Item("Total") = Stock
        Stock = 0

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

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

        Insumo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 6;")
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

        If Grid.Columns(e.ColumnIndex).Name = "Insumo" Then
            If PermisoTotal Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

    End Sub
 

End Class