Imports System.Transactions
Imports System.Math
Public Class AnalisisLotes
    '  valor false equivale 0.
    '  valor true  equivale 1.
    Public EsConsignacion As Boolean
    Public EsReventa As Boolean
    Public EsCosteo As Boolean
    Public PBloqueaFunciones As Boolean
    ' 
    Dim DtGrid As DataTable
    Dim DtLotes As DataTable
    Dim View As DataView
    '
    Private WithEvents bs As New BindingSource
    '
    Dim SqlN As String
    Dim SqlB As String
    Dim PermisoAdministrador As Boolean
    Private Sub AnalisisLotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoLectura(210) Then Grid.Columns("PrecioF").Visible = False
        If PermisoEscritura(210) Then
            Grid.Columns("PrecioF").ReadOnly = False
            Grid.Columns("PrecioFSinIva").ReadOnly = False
        Else : Grid.Columns("PrecioF").ReadOnly = True : Grid.Columns("PrecioFSinIva").ReadOnly = True
        End If

        If Not PermisoLectura(211) Then Grid.Columns("PrecioCompra").Visible = False
        If PermisoEscritura(211) Then
            Grid.Columns("PrecioCompra").ReadOnly = False
        Else : Grid.Columns("PrecioCompra").ReadOnly = True
        End If

        If Not PermisoLectura(212) Then Grid.Columns("MermaTr").Visible = False
        If PermisoEscritura(212) Then
            Grid.Columns("MermaTr").ReadOnly = False
        Else : Grid.Columns("MermaTr").ReadOnly = True
        End If

        If Not PermisoLectura(200) Then
            Grid.Columns("Importe").Visible = False
            Grid.Columns("ImporteSinIva").Visible = False
            Grid.Columns("PromedioRealTotal").Visible = False
            Grid.Columns("PromedioRealSIva").Visible = False
            Grid.Columns("PrecioS").Visible = False
            Grid.Columns("PrecioSSinIva").Visible = False
            Grid.Columns("Lupa").Visible = False
        End If

        Grid.AutoGenerateColumns = False

        GeneraCombosGrid()

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaCombo(ComboArticulo, "", "Articulos")
        ComboArticulo.SelectedValue = 0

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        Dim SqlTipoOperacion As String = ""
        If EsCosteo Then
            SqlTipoOperacion = " AND TipoOperacion = 4"
            ComboCosteo.Visible = True
            ComboAlias.Visible = False
            Label10.Text = "Costeo"
        Else
            SqlTipoOperacion = " AND TipoOperacion <> 4"
            ComboCosteo.Visible = False
            ComboAlias.Visible = True
            Label10.Text = "Alias"
        End If

        Dim Row As DataRow

        ComboProveedor.DataSource = Tablas.Leer("SELECT Clave,nombre FROM Proveedores WHERE Producto = " & Fruta & SqlTipoOperacion & ";")
        Row = ComboProveedor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboProveedor.DataSource.Rows.Add(Row)
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE TipoOperacion <> 4 AND Alias <> '' AND Producto = " & Fruta & ";")
        Row = ComboAlias.DataSource.NewRow
        Row("Clave") = 0
        Row("Alias") = ""
        ComboAlias.DataSource.Rows.Add(Row)
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        With ComboArticulo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        With ComboCosteo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PermisoTotal Then
            Grid.Columns("Candado").Visible = True
        Else : Grid.Columns("Candado").Visible = False
        End If

        If EsConsignacion Then
            Me.Text = "Lotes en Consignación."
            Grid.Columns("PrecioCompra").Visible = False
        End If
        If EsReventa Then
            Me.Text = "Lotes en Reventa."
            If PermisoLectura(211) Then Grid.Columns("PrecioCompra").Visible = True
            Grid.Columns("Proveedor").Width = Grid.Columns("Proveedor").Width - Grid.Columns("PrecioCompra").Width / 2
            Grid.Columns("Articulo").Width = Grid.Columns("Articulo").Width - Grid.Columns("PrecioCompra").Width / 2
        End If
        If EsCosteo Then
            Me.Text = "Lotes Costeo."
            If PermisoLectura(211) Then Grid.Columns("PrecioCompra").Visible = True
            Grid.Columns("Proveedor").Width = Grid.Columns("Proveedor").Width - Grid.Columns("PrecioCompra").Width / 2
            Grid.Columns("Articulo").Width = Grid.Columns("Articulo").Width - Grid.Columns("PrecioCompra").Width / 2
            Grid.Columns("PrecioF").Visible = False
            Grid.Columns("PrecioFSinIva").Visible = False
            Grid.Columns("PrecioCompra").Visible = False
            Grid.Columns("MermaTr").Visible = False
        End If

        CreaDtGrid()

    End Sub
    Private Sub AnalisisLotes_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not IsNothing(DtGrid.GetChanges) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            If MsgBox("Los Cambios no fueron Actualizados. Quiere Actualizarlos?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                e.Cancel = True
                Exit Sub
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 112 Then
            If Grid.Columns(Grid.CurrentCell.ColumnIndex).Name = "PrecioS" Then
                If Grid.CurrentRow.Cells("PrecioS").Value < 0 Then
                    MsgBox("No se puede copiar precio negatico.") : Exit Sub
                End If
                If Grid.CurrentRow.Cells("PrecioF").Value <> 0 Then
                    MsgBox("Ya existe un precio Final.")
                    Exit Sub
                Else
                    Grid.CurrentRow.Cells("PrecioF").Value = Grid.CurrentRow.Cells("PrecioS").Value
                End If
            End If
        End If

    End Sub
    Private Sub ButtonSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSeleccionar.Click

        If Not Valida() Then Exit Sub

        If EsConsignacion Then
            SqlB = "SELECT 1 AS Operacion,L.Lote,L.Secuencia,L.Deposito,L.Articulo,L.Proveedor,L.Fecha,L.PrecioCompra,L.MermaTr,0 AS Moneda,KilosXUnidad,Senia,0 AS Costeo,Sucursal,C.OrdenCompra FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE CAST(L.Cantidad AS Decimal(18,4)) <> CAST(L.Baja AS Decimal(18,4)) AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 1 "
            SqlN = "SELECT 2 AS Operacion,L.Lote,L.Secuencia,L.Deposito,L.Articulo,L.Proveedor,L.Fecha,L.PrecioCompra,L.MermaTr,0 AS Moneda,KilosXUnidad,Senia,0 AS Costeo,Sucursal,C.OrdenCompra FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE CAST(L.Cantidad AS Decimal(18,4)) <> CAST(L.Baja AS Decimal(18,4)) AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 1 "
        End If
        If EsReventa Then
            SqlB = "SELECT 1 AS Operacion,L.Lote,L.Secuencia,L.Deposito,L.Articulo,L.Proveedor,L.Fecha,L.PrecioCompra,L.MermaTr,L.Liquidado,C.Moneda,KilosXUnidad,Senia,0 AS Costeo,Sucursal,C.OrdenCompra FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE CAST(L.Cantidad AS Decimal(18,4)) <> CAST(L.Baja AS Decimal(18,4)) AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 2 "
            SqlN = "SELECT 2 AS Operacion,L.Lote,L.Secuencia,L.Deposito,L.Articulo,L.Proveedor,L.Fecha,L.PrecioCompra,L.MermaTr,L.Liquidado,C.Moneda,KilosXUnidad,Senia,0 AS Costeo,Sucursal,C.OrdenCompra FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE CAST(L.Cantidad AS Decimal(18,4)) <> CAST(L.Baja AS Decimal(18,4)) AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 2 "
        End If
        If EsCosteo Then
            SqlB = "SELECT 1 AS Operacion,L.Lote,L.Secuencia,L.Deposito,L.Articulo,L.Proveedor,L.Fecha,L.PrecioCompra,L.MermaTr,L.Liquidado,C.Moneda,KilosXUnidad,Senia,C.Costeo,Sucursal,C.OrdenCompra FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE CAST(L.Cantidad AS Decimal(18,4)) <> CAST(L.Baja AS Decimal(18,4)) AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 4 "
            SqlN = "SELECT 2 AS Operacion,L.Lote,L.Secuencia,L.Deposito,L.Articulo,L.Proveedor,L.Fecha,L.PrecioCompra,L.MermaTr,L.Liquidado,C.Moneda,KilosXUnidad,Senia,C.Costeo,Sucursal,C.OrdenCompra FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE CAST(L.Cantidad AS Decimal(18,4)) <> CAST(L.Baja AS Decimal(18,4)) AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 4 "
        End If

        Dim SqlArticulo As String = ""
        If ComboArticulo.SelectedValue <> 0 Then
            SqlArticulo = "AND L.Articulo = " & ComboArticulo.SelectedValue & " "
        Else : SqlArticulo = "AND L.Articulo LIKE '%' "
        End If

        Dim Lote As Integer = 0
        Dim Secuencia As Integer = 0
        If Val(MaskedLote.Text) <> 0 Then
            ConsisteMaskedLote(MaskedLote.Text, Lote, Secuencia)
        End If

        Dim SqlLote As String = ""
        If Lote <> 0 Then
            SqlLote = "AND L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & " "
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "AND L.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND L.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlProveedor As String = ""
        If ComboProveedor.SelectedValue <> 0 Then
            SqlProveedor = "AND L.Proveedor = " & ComboProveedor.SelectedValue & " "
        End If
        If ComboAlias.SelectedValue <> 0 Then
            SqlProveedor = "AND L.Proveedor = " & ComboAlias.SelectedValue & " "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND L.Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        Dim SqlLiquidado As String = ""
        If Not CheckLiquidados.Checked And Not CheckNoLiquidados.Checked Then
            CheckLiquidados.Checked = True
            CheckNoLiquidados.Checked = True
        End If
        If CheckLiquidados.Checked And Not CheckNoLiquidados.Checked Then
            SqlLiquidado = " AND L.Liquidado <> 0 "
        End If
        If Not CheckLiquidados.Checked And CheckNoLiquidados.Checked Then
            SqlLiquidado = " AND L.Liquidado = 0 "
        End If

        Dim SqlCosteo As String = ""
        If ComboCosteo.SelectedValue <> 0 And EsCosteo Then
            SqlCosteo = " AND Costeo = " & ComboCosteo.SelectedValue
        End If

        SqlB = SqlB & SqlArticulo & SqlLote & SqlFecha & SqlProveedor & SqlLiquidado & SqlDeposito & SqlCosteo
        SqlN = SqlN & SqlArticulo & SqlLote & SqlFecha & SqlProveedor & SqlLiquidado & SqlDeposito & SqlCosteo

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtLotes = New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtLotes) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtLotes) Then Me.Close() : Exit Sub
        End If

        If DtLotes.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If Not LLenaGrid() Then Me.Close() : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.Default

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)

        Grid.Focus()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtLotesB As New DataTable
        Dim DtLotesN As New DataTable
        Dim Sql As String

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not IsNothing(DtGrid.GetChanges) Then
            For Each Row As DataRow In DtGrid.GetChanges.Rows
                Sql = "SELECT * FROM Lotes WHERE Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Lotes.Lote = Lotes.LoteOrigen AND Lotes.Secuencia = Lotes.SecuenciaOrigen AND Lotes.Deposito = Lotes.DepositoOrigen;"
                If Row("Operacion") = 1 Then
                    If Not Tablas.Read(Sql, Conexion, DtLotesB) Then Me.Close() : Exit Sub
                    DtLotesB.Rows(DtLotesB.Rows.Count - 1).Item("PrecioCompra") = Row("PrecioCompra")
                    If Row("PrecioFAnt") <> Row("PrecioF") Then
                        DtLotesB.Rows(DtLotesB.Rows.Count - 1).Item("PrecioF") = Row("PrecioF")
                        ActualizoItemGrid(Row("Lote"), Row("Secuencia"), Row("PrecioF"))
                    End If
                    DtLotesB.Rows(DtLotesB.Rows.Count - 1).Item("MermaTr") = Row("MermaTr")
                Else
                    If Not Tablas.Read(Sql, ConexionN, DtLotesN) Then Me.Close() : Exit Sub
                    DtLotesN.Rows(DtLotesN.Rows.Count - 1).Item("PrecioCompra") = Row("PrecioCompra")
                    If Row("PrecioFAnt") <> Row("PrecioF") Then
                        DtLotesN.Rows(DtLotesN.Rows.Count - 1).Item("PrecioF") = Row("PrecioF")
                        ActualizoItemGrid(Row("Lote"), Row("Secuencia"), Row("PrecioF"))
                    End If
                    DtLotesN.Rows(DtLotesN.Rows.Count - 1).Item("MermaTr") = Row("MermaTr")
                End If
            Next
        End If

        If DtLotesB.Rows.Count = 0 And DtLotesN.Rows.Count = 0 Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim Resul As Boolean = ActualizaLotes(DtLotesB, DtLotesN)

        DtLotesB.Dispose()
        DtLotesN.Dispose()

        If Resul Then
            DtGrid.AcceptChanges()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonIngresos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRemito.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        Dim Abierto As Boolean
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If
        UnIngresoMercaderia.PLote = Grid.CurrentRow.Cells("Lote").Value
        UnIngresoMercaderia.PBloqueaFunciones = True
        UnIngresoMercaderia.PAbierto = Abierto
        UnIngresoMercaderia.ShowDialog()
        UnIngresoMercaderia.Dispose()

    End Sub
    Private Sub ButtonRemitoVentas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRemitoVentas.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        RemitosPorLoteIngresado(Grid.CurrentRow.Cells("Lote").Value, Grid.CurrentRow.Cells("Secuencia").Value, Grid.CurrentRow.Cells("Operacion").Value)

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Lotes Para Liquidación", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub RadioImportesiniva_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioImporteSinIva.CheckedChanged

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Grid.Columns("Importe").Visible = Not Grid.Columns("Importe").Visible
        Grid.Columns("ImporteSinIva").Visible = Not Grid.Columns("ImporteSinIva").Visible
        Grid.Columns("PrecioS").Visible = Not Grid.Columns("PrecioS").Visible
        Grid.Columns("PrecioSSinIva").Visible = Not Grid.Columns("PrecioSSinIva").Visible
        Grid.Columns("PromedioRealTotal").Visible = Not Grid.Columns("PromedioRealTotal").Visible
        Grid.Columns("PromedioRealSIva").Visible = Not Grid.Columns("PromedioRealSIva").Visible

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

        If EsCosteo Then
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & ComboProveedor.SelectedValue & ";"
            ComboCosteo.DataSource = Tablas.Leer(Sql)
            Dim Row As DataRow = ComboCosteo.DataSource.newrow
            Row("Costeo") = 0
            Row("Nombre") = ""
            ComboCosteo.DataSource.rows.add(Row)
            ComboCosteo.DisplayMember = "Nombre"
            ComboCosteo.ValueMember = "Costeo"
            ComboCosteo.SelectedValue = 0
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End If

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

    End Sub
    Private Sub ComboArticulo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboArticulo.Validating

        If IsNothing(ComboArticulo.SelectedValue) Then ComboArticulo.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

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
    Private Function LLenaGrid() As Boolean

        If Not CheckConStock.Checked And Not CheckSinStock.Checked Then
            CheckConStock.Checked = True
            CheckSinStock.Checked = True
        End If
        If Not CheckConPrecioFinal.Checked And Not CheckSinPrecioFinal.Checked Then
            CheckConPrecioFinal.Checked = True
            CheckSinPrecioFinal.Checked = True
        End If

        Dim RowGrid As DataRow
        Dim Remitido As Double
        Dim Facturado As Double
        Dim Importe As Double
        Dim ImporteSinIva As Double
        Dim Senia As Double
        Dim PrecioS As Double
        Dim PrecioSSinIva As Double
        Dim Baja As Double
        Dim Merma As Double
        Dim MermaTr As Double
        Dim Saldo As Double
        Dim Stock As Double
        Dim Liquidado As Double
        Dim PrecioF As Double
        Dim PrecioCompra As Double
        Dim CantidadInicial As Double
        Dim Descarga As Double
        Dim DescargaSinIva As Double
        Dim GastoComercial As Double
        Dim GastoComercialSinIva As Double
        Dim CostoDelCosteo As Double = 0
        Dim CostoDelCosteoSinIva As Double = 0

        Dim Color As Integer

        View = New DataView
        View = DtLotes.DefaultView
        View.Sort = "Lote,Secuencia"

        CreaDtGrid()

        For Each Row As DataRowView In View
            'costos de ventas ............................................
            Dim ListaOrden As String
            If Not AnalisisCostoLote(False, Row("Operacion"), Row("Proveedor"), Row("Lote"), Row("Secuencia"), Saldo, PrecioS, PrecioSSinIva, Remitido, Facturado, Importe, ImporteSinIva, Baja, Merma, MermaTr, Stock, Liquidado, PrecioF, PrecioCompra, CantidadInicial, Senia, Descarga, DescargaSinIva, GastoComercial, GastoComercialSinIva, True, CheckSinReintegro.Checked, ListaOrden) Then Return False
            If EsSelecionOk(Stock, PrecioF) Then
                'Agrega registro en DtGrid.
                Color = 0
                If Row("Moneda") > 1 Then Color = 1
                '
                RowGrid = DtGrid.NewRow()
                RowGrid("Color") = Color
                RowGrid("Operacion") = Row("Operacion")
                RowGrid("OrdenCompra") = Row("OrdenCompra")
                RowGrid("Lote") = Row("Lote")
                RowGrid("Secuencia") = Row("Secuencia")
                RowGrid("Deposito") = Row("Deposito")
                RowGrid("Articulo") = Row("Articulo")
                RowGrid("Proveedor") = Row("Proveedor")
                RowGrid("Fecha") = Row("Fecha")
                RowGrid("Cantidad") = CantidadInicial - Baja
                RowGrid("Merma") = Merma      'merma=merma + descarte
                RowGrid("MermaTr") = MermaTr
                RowGrid("Remitido") = Remitido
                RowGrid("Facturado") = Facturado
                RowGrid("Importe") = Importe
                RowGrid("ImporteSinIva") = ImporteSinIva
                RowGrid("Moneda") = Row("Moneda")
                RowGrid("PrecioS") = PrecioS
                If Facturado <> 0 Then
                    RowGrid("PromedioRealTotal") = Importe / Facturado
                    RowGrid("PromedioRealSIva") = ImporteSinIva / Facturado
                Else
                    RowGrid("PromedioRealTotal") = 0
                    RowGrid("PromedioRealSIva") = 0
                End If
                RowGrid("PrecioSSinIva") = PrecioSSinIva
                RowGrid("PrecioF") = PrecioF
                RowGrid("PrecioFAnt") = PrecioF
                RowGrid("PrecioCompra") = PrecioCompra
                RowGrid("Liquidado") = Liquidado
                RowGrid("Stock") = Stock
                RowGrid("Senia") = Row("Senia")
                RowGrid("PrecioFSinIva") = 0
                RowGrid("ListaOrden") = ListaOrden
                DtGrid.Rows.Add(RowGrid)
            End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        DtGrid.AcceptChanges()

        DtLotes.Dispose()

        Return True

    End Function
    Private Function EsSelecionOk(ByVal Stock As Decimal, ByVal PrecioF As Decimal) As Boolean

        If CheckConStock.Checked And Not CheckSinStock.Checked And Stock = 0 Then Return False
        If Not CheckConStock.Checked And CheckSinStock.Checked And Stock <> 0 Then Return False
        If CheckConPrecioFinal.Checked And Not CheckSinPrecioFinal.Checked And PrecioF = 0 Then Return False
        If Not CheckConPrecioFinal.Checked And CheckSinPrecioFinal.Checked And PrecioF <> 0 Then Return False

        Return True

    End Function
    Private Sub GeneraCombosGrid()

        Articulo.DataSource = TodosLosArticulos()
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

        Articulo.DataSource = TodosLosArticulos()
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        Moneda.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 27 order By Nombre;")
        Dim Row As DataRow = Moneda.DataSource.NewRow()
        Row("Clave") = 1
        Row("Nombre") = "PESOS"
        Moneda.DataSource.Rows.Add(Row)
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Moneda.DataSource.Rows.Add(Row)
        Moneda.DisplayMember = "Nombre"
        Moneda.ValueMember = "Clave"

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Color As New DataColumn("Color")
        Color.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Color)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Deposito As New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Merma As New DataColumn("Merma")
        Merma.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Merma)

        Dim MermaTr As New DataColumn("MermaTr")
        MermaTr.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(MermaTr)

        Dim Remitido As New DataColumn("Remitido")
        Remitido.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Remitido)

        Dim Facturado As New DataColumn("Facturado")
        Facturado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Facturado)

        Dim Liquidado As New DataColumn("Liquidado")
        Liquidado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Liquidado)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Importe)

        Dim ImporteSinIva As New DataColumn("ImporteSinIva")
        ImporteSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ImporteSinIva)

        Dim Senia As New DataColumn("Senia")
        Senia.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Senia)

        Dim PrecioSSinIva As New DataColumn("PrecioSSinIva")
        PrecioSSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioSSinIva)

        Dim PromedioRealTotal As New DataColumn("PromedioRealTotal")
        PromedioRealTotal.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PromedioRealTotal)

        Dim PromedioRealSIva As New DataColumn("PromedioRealSIva")
        PromedioRealSIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PromedioRealSIva)

        Dim PrecioS As New DataColumn("PrecioS")
        PrecioS.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioS)

        Dim PrecioCompra As New DataColumn("PrecioCompra")
        PrecioCompra.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioCompra)

        Dim Moneda As New DataColumn("Moneda")
        Moneda.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Moneda)

        Dim PrecioF As New DataColumn("PrecioF")
        PrecioF.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioF)

        Dim PrecioFAnt As New DataColumn("PrecioFAnt")
        PrecioFAnt.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioFAnt)

        Dim PrecioFSinIva As New DataColumn("PrecioFSinIva")
        PrecioFSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioFSinIva)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Stock)

        Dim OrdenCompra As New DataColumn("OrdenCompra")
        OrdenCompra.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(OrdenCompra)

        Dim ListaOrden As New DataColumn("ListaOrden")
        OrdenCompra.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(ListaOrden)

    End Sub
    Private Function ActualizaLotes(ByVal DtLotesB As DataTable, ByVal DtLotesN As DataTable) As Boolean

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If DtLotesB.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtLotesB.GetChanges, "Lotes", Conexion)
                    If Resul < 0 Then
                        MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        Return False
                    End If
                    If Resul = 0 Then
                        MsgBox("Error Otro Usuario Modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        Return False
                    End If
                End If

                If DtLotesN.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtLotesN.GetChanges, "Lotes", ConexionN)
                    If Resul < 0 Then
                        MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        Return False
                    End If
                    If Resul = 0 Then
                        MsgBox("Error Otro Usuario Modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        Return False
                    End If
                End If

                Scope.Complete()
                MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Return True
            End Using
        Catch ex As TransactionException
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        Finally
        End Try

    End Function
    Private Function HallaPrecioSinIva(ByVal Precio As Decimal, ByVal Articulo As Integer) As Decimal

        Return Trunca(Precio * 100 / (100 + HallaIva(Articulo)))

    End Function
    Private Function HallaPrecioConIva(ByVal Precio As Decimal, ByVal Articulo As Integer) As Decimal

        Return Trunca(Precio + CalculaIva(1, Precio, HallaIva(Articulo)))

    End Function
    Private Sub ActualizoItemGrid(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Valor As Decimal)

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Lote").Value = Lote And Row.Cells("Secuencia").Value = Secuencia Then
                Row.Cells("PrecioFAnt").Value = Valor
                Row.Cells("PrecioF").Value = Valor
                Exit For
            End If
        Next

    End Sub
    Private Function PidePermiso(ByVal Stock As Decimal, ByVal Remitido As Decimal, ByVal Facturado As Decimal, ByVal Valor As Decimal) As Boolean

        If Not EsConsignacion Or PermisoAdministrador Or Valor = 0 Then Return True

        If Stock <> 0 Or Remitido <> 0 Or Facturado = 0 Then
            If MsgBox("Esta Informando Precio Final a Lote de Consignación Con Stock o Remitos pendientes o sin facturación y Podra ser Liquidado." + vbCrLf + "Solo un Administrador puede Hacero. Quiere Informar Precio(SI/NO)?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
            If Not GAdministrador Then
                MsgBox("No es Administrador.") : Return False
            End If
        End If

        PermisoAdministrador = True

        Return True

    End Function
    Private Function Valida() As Boolean

        If ComboProveedor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Proveedor o Alias.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            ComboProveedor.Focus()
            Return False
        End If

        Dim Lote As Integer = 0
        Dim Secuencia As Integer = 0
        If Val(MaskedLote.Text) <> 0 Then
            If Not ConsisteMaskedLote(MaskedLote.Text, Lote, Secuencia) Then
                MaskedLote.Focus()
                Return False
            End If
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Hasta debe ser mayor o igual a fecha Desde.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) And _
              Not IsNothing(Grid.Rows(e.RowIndex).Cells("Lote").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
            End If
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 1 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.LightBlue
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) And Not IsNothing(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Liquidado" Then
            If e.Value <> 0 Then
                e.Value = NumeroEditado(e.Value)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Remitido" Or Grid.Columns(e.ColumnIndex).Name = "Facturado" Or _
             Grid.Columns(e.ColumnIndex).Name = "PromedioRealTotal" Or Grid.Columns(e.ColumnIndex).Name = "PromedioRealSIva" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Merma" Or Grid.Columns(e.ColumnIndex).Name = "MermaTr" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = -100 Then e.Value = ""
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Stock" Then
            If IsDBNull(e.Value) Then e.Value = 0
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "ImporteSinIva" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Senia" Or Grid.Columns(e.ColumnIndex).Name = "PrecioS" _
            Or Grid.Columns(e.ColumnIndex).Name = "PrecioSSinIva" Or Grid.Columns(e.ColumnIndex).Name = "PrecioCompra" Or Grid.Columns(e.ColumnIndex).Name = "PrecioF" Or Grid.Columns(e.ColumnIndex).Name = "PrecioFSinIva" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value <> 0 Then
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

    End Sub
    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            ValorizaLote.PImporteTotal = RadioImporteTotal.Checked
            ValorizaLote.PLote = Grid.CurrentRow.Cells("Lote").Value
            ValorizaLote.PSecuencia = Grid.CurrentRow.Cells("Secuencia").Value
            ValorizaLote.POperacion = Grid.CurrentRow.Cells("Operacion").Value
            ValorizaLote.PSinReintegro = CheckSinReintegro.Checked
            ValorizaLote.ShowDialog()
            ValorizaLote.Dispose()
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Not Grid.Columns.Item(columna).Name = "PrecioF" And Not Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioFSinIva" And Not Grid.Columns.Item(columna).Name = "PrecioCompra" And Not Grid.Columns.Item(columna).Name = "MermaTr" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioF" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioFSinIva" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioCompra" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "MermaTr" Then
            If EsArticuloAGranel(Grid.CurrentRow.Cells("Articulo").Value) Then
                If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
            Else
                If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
            End If
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioF" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioFSinIva" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioCompra" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 5)
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "MermaTr" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("PrecioF") Then
            If IsDBNull(e.Row("PrecioF")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca5(e.ProposedValue)
            If e.Row("Liquidado") <> 0 Then
                MsgBox("Lote ya fue Liquidado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("PrecioF")
                Exit Sub
            End If
            '
            If Not PidePermiso(Abs(e.Row("Stock")), e.Row("Remitido"), e.Row("Facturado"), e.ProposedValue) Then
                e.ProposedValue = e.Row("PrecioF")
                Exit Sub
            End If
        End If

        If e.Column.ColumnName.Equals("PrecioFSinIva") Then
            If IsDBNull(e.Row("PrecioFSinIva")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca5(e.ProposedValue)
            If e.Row("Liquidado") <> 0 Then
                MsgBox("Lote ya fue Liquidado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("PrecioFSinIva")
                Exit Sub
            End If
            '
            If Not PidePermiso(Abs(e.Row("Stock")), e.Row("Remitido"), e.Row("Facturado"), e.ProposedValue) Then
                e.ProposedValue = e.Row("PrecioFSinIva")
                Exit Sub
            End If
            e.Row("PrecioF") = HallaPrecioConIva(e.ProposedValue, e.Row("Articulo"))
            e.ProposedValue = 0
            e.Row.AcceptChanges()
        End If

        If e.Column.ColumnName.Equals("PrecioCompra") Then
            If IsDBNull(e.Row("PrecioCompra")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca5(e.ProposedValue)
            If e.Row("Liquidado") <> 0 Then
                MsgBox("Lote ya fue Liquidado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("PrecioCompra")
                Exit Sub
            End If
        End If

        If e.Column.ColumnName.Equals("MermaTr") Then
            If IsDBNull(e.Row("MermaTr")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = -100
            e.ProposedValue = Trunca(e.ProposedValue)
            If e.Row("Liquidado") <> 0 Then
                MsgBox("Lote ya fue Liquidado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("MermaTr")
                Exit Sub
            End If
            If e.ProposedValue <> -100 Then
                If (e.Row("Cantidad") - CDec(e.ProposedValue)) < 0 Then
                    MsgBox("MermaTr Hace Negativa cantidad a Liquidar.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    e.ProposedValue = e.Row("MermaTr")
                    Exit Sub
                End If
            End If
        End If

    End Sub
    Private Sub Grid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.CellBeginEdit

        If Grid.Columns(e.ColumnIndex).Name = "PrecioF" Or Grid.Columns(e.ColumnIndex).Name = "PrecioFSinIva" Then
            If Grid.Rows(e.RowIndex).Cells("ListaOrden").Value = "1" Then
                If MsgBox("Precio informado en Lista de Precio. Quiere Modificar a nuevo Precio?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                    e.Cancel = True
                End If
            End If
            If Grid.Rows(e.RowIndex).Cells("ListaOrden").Value = "2" Then
                If MsgBox("Precio informado en la Orden de Compra " & Grid.Rows(e.RowIndex).Cells("OrdenCompra").Value & ". Quiere Modificar a nuevo Precio?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                    e.Cancel = True
                End If
            End If
        End If

    End Sub

End Class