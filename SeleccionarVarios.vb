Public Class SeleccionarVarios
    Public PEsReventa As Boolean
    Public PEsInsumos As Boolean
    Public PEsLiquidacionInsumos As Boolean
    Public PEsCostoInsumo As Boolean
    Public PEsSeleccionaLote As Boolean
    Public PEsSeleccionaOrden As Boolean
    Public PEsRetenciones As Boolean
    Public PEsCuentas As Boolean
    Public PEsTodos As Boolean
    Public PEsCheque As Boolean
    Public PEsChequeParaReemplazo As Boolean
    Public PEsNVLP As Boolean
    Public PEsElijeComprobante As Boolean
    Public PEsDetalleRemito As Boolean
    Public PEsDetalleIngreso As Boolean
    Public PEsCentroCosto As Boolean
    Public PEsNetoPorLotes As Boolean
    Public PEsNetoPorLotesAfectados As Boolean
    Public PEsNetoPorLotesFacturaVenta As Boolean
    Public PEsNetoPorLotesTicket As Boolean
    Public PEsImportePorLotesRecibos As Boolean
    Public PEsImportePorLotesAsientos As Boolean
    Public PEsImportePorLotesOtrasFacturas As Boolean
    Public PEsNetoPorLotesNVLP As Boolean
    Public PEsNetoPorLiquidacionInsumos As Boolean
    Public PEsDocuRetenciones As Boolean
    Public PEsRetencionesExentas As Boolean
    Public PEsValeTercerosOrdenPago As Boolean
    Public PEsConsumos As Boolean
    Public PEsConsumosLotesPT As Boolean
    Public PEsImporteLotesReintegros As Boolean
    Public PEsPercepciones As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Public PCaja As Integer
    Public PBloqueado As Boolean
    Public PEmisor As Integer
    Public PAbierto As Boolean
    Public PImporte As Double
    Public PLote As Integer
    Public PSecuencia As Integer
    Public PStock As Double
    Public POrden As Double
    Public PDeposito As Integer
    Public PArticulo As Integer
    Public PCuenta As Double
    Public PUltimaSerie As String
    Public PUltimoNumero As Integer
    Public PNumeracionInicial As Integer
    Public PNumeracionFinal As Integer
    Public POperacion As Integer
    Public PClave As Integer
    Public PBanco As Integer
    Public PNumero As Integer
    Public PSerie As String
    Public PEmisorCheque As String
    Public PMedioPago As String
    Public PFecha As Date
    Public PSoloPesos As Boolean
    Public PConceptoGasto As Integer
    Public PRemito As Double
    Public PCentro As Integer
    Public PNombre As String
    Public PLiquidacion As Double
    Public PLiquidacion2 As Double
    Public PFactura As Double
    Public PTicket As Double
    Public Pinsumo As Integer
    Public PCodigoRetencion As Integer
    Public PConsumo As Integer
    Public PTipoNota As Integer
    Public PNota As Double
    Public PSucursal As Integer
    Public PConexion As String
    Public PConexionN As String
    '  
    Public PListaDeLotes As List(Of FilaComprobanteFactura)
    Public PListaDeLotesOrigen As List(Of FilaComprobanteFactura)
    Public PListaDeRetenciones As List(Of FilaItemsRetencion)
    Public PListaDePercepciones As List(Of ItemIvaReten)
    Public PListaDeRecuperoSenia As List(Of ItemRecuperoSenia)
    Public PDtDetalleInsumo As DataTable
    '
    Dim DtGrid As DataTable
    Private Sub SeleccionarVarios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If PConexion <> "" Then
            Conexion = PConexion : ConexionN = PConexionN
        End If

        Grid.AutoGenerateColumns = False

        Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Grid.AllowUserToAddRows = False
        Grid.AllowUserToDeleteRows = False
        '      Grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White
        '       Grid.DefaultCellStyle.BackColor = Color.White
        '       Grid.DefaultCellStyle.SelectionBackColor = Color.Blue
        '       Grid.DefaultCellStyle.SelectionForeColor = Color.Black
        Grid.BackgroundColor = Color.White
        Grid.Columns.Clear()

        PImporte = 0

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PEsInsumos Then GenerarInsumos()
        If PEsLiquidacionInsumos Then GenerarLiquidacionInsumos()
        If PEsCostoInsumo Then GenerarCostoInsumo()
        If PEsReventa Then GenerarReventa()
        If PEsSeleccionaLote Then GenerarSeleccionaLotes()
        If PEsSeleccionaOrden Then GenerarSeleccionaOrden()
        If PEsRetenciones Then GenerarRetenciones()
        If PEsCuentas Then GenerarCuentas()
        If PEsChequeParaReemplazo Then GenerarChequesParaReemplazo()
        If PEsNVLP Then GenerarNVLP()
        If PEsDetalleRemito Then GenerarDetalleRemito()
        If PEsDetalleIngreso Then GenerarDetalleIngreso()
        If PEsCentroCosto Then GenerarCentrosCosto()
        If PEsNetoPorLotes Then GenerarNetoPorLotes()
        If PEsNetoPorLotesNVLP Then GenerarNetoPorLotesNVLP()
        If PEsNetoPorLiquidacionInsumos Then GenerarNetoPorLiquidacionInsumos()
        If PEsNetoPorLotesAfectados Then GenerarNetoPorLotesAfectados()
        If PEsNetoPorLotesFacturaVenta Then GenerarNetoPorLotesFacturaVenta()
        If PEsNetoPorLotesTicket Then GenerarNetoPorLotesTicket()
        If PEsImportePorLotesRecibos Then GenerarImportePorLotesRecibos()
        If PEsImportePorLotesAsientos Then GenerarImportePorLotesAsientos()
        If PEsImportePorLotesOtrasFacturas Then GenerarImportePorLotesOtrasFacturas()
        If PEsDocuRetenciones Then GenerarDocuRetenciones()
        If PEsRetencionesExentas Then GenerarRetencionesExentas()
        If PEsPercepciones Then MuestraPercepciones()
        If PEsValeTercerosOrdenPago Then GenerarValesTercerosOrdenPago()
        If PEsConsumos Then GenerarConsumosDeLotes()
        If PEsConsumosLotesPT Then GenerarTotalPorLotesConsumosPT()
        If PEsElijeComprobante Then GenerarElijeComprobante()
        If PEsImporteLotesReintegros Then GenerarImporteLotesReintegros()

        Grid.Width = 0
        For i As Integer = 0 To Grid.Columns.Count - 1
            If Grid.Columns(i).Visible = True Then
                Grid.Width = Grid.Width + Grid.Columns(i).Width + 5
            End If
        Next
        Grid.Width = Grid.Width + 60

        Me.Width = Grid.Width + 80

        Grid.Top = 50
        Grid.Left = Me.Width / 2 - Grid.Width / 2
        ButtonAceptar.Left = Me.Width / 2 - ButtonAceptar.Width / 2

        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2

        If PEsReventa Or PEsInsumos Or PEsLiquidacionInsumos Then
            PanelMarcarTodos.Visible = True
            PanelMarcarTodos.Left = Grid.Left
            PanelMarcarTodos.Top = Grid.Top - PanelMarcarTodos.Height
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub SeleccionarVarios_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If PEsNetoPorLotesAfectados Or PEsNetoPorLotesFacturaVenta Then Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Con As Integer = 0

        If PEsDocuRetenciones Or PEsRetencionesExentas Then
            PListaDeRetenciones.Clear()
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    Dim Item As New FilaItemsRetencion
                    Item.Clave = Row.Cells("Clave").Value
                    PListaDeRetenciones.Add(Item)
                End If
            Next
            Me.Close() : Exit Sub
        End If

        If PEsValeTercerosOrdenPago Then
            PListaDeRecuperoSenia.Clear()
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    Dim Item As New ItemRecuperoSenia
                    Item.Nota = Row.Cells("Recibo").Value
                    Item.Vale = Row.Cells("Vale").Value
                    Item.Fecha = Row.Cells("Fecha").Value
                    Item.Importe = Row.Cells("Importe").Value
                    PListaDeRecuperoSenia.Add(Item)
                End If
            Next
            Me.Close() : Exit Sub
        End If

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value = True Then Con = Con + 1
        Next
        If Con = 0 Then
            MsgBox("Debe elegir un Item.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Exit Sub
        End If

        If PEsInsumos Then
            PListaDeLotes.Clear()
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    Dim Item As New FilaComprobanteFactura
                    Item.OrdenCompra = Row.Cells("OrdenCompra").Value
                    Item.Remito = Row.Cells("Remito").Value
                    Item.Ingreso = Row.Cells("Ingreso").Value
                    Item.Operacion = Row.Cells("Operacion").Value
                    Item.Importe = Row.Cells("Importe").Value
                    Item.ImporteB = Row.Cells("ImporteB").Value
                    Item.ImporteN = Row.Cells("ImporteN").Value
                    PListaDeLotes.Add(Item)
                End If
            Next
            Me.Close() : Exit Sub
        End If

        If PEsLiquidacionInsumos Then
            PListaDeLotes.Clear()
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    Dim Item As New FilaComprobanteFactura
                    Item.OrdenCompra = Row.Cells("OrdenCompra").Value
                    Item.Remito = Row.Cells("Remito").Value
                    Item.Ingreso = Row.Cells("Ingreso").Value
                    Item.Operacion = Row.Cells("Operacion").Value
                    Item.ImporteB = Row.Cells("ImporteB").Value
                    Item.ImporteN = Row.Cells("ImporteN").Value
                    PListaDeLotes.Add(Item)
                End If
            Next
            Me.Close() : Exit Sub
        End If

        If PEsReventa Then
            PListaDeLotes.Clear()
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    If Row.Cells("Importe").Value = 0 Then
                        MsgBox("Falta Precio en Lote " & Row.Cells("Lote").Value & "/" & Format(Row.Cells("Secuencia").Value, "000"), MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                        Grid.Focus()
                        Exit Sub
                    End If
                End If
            Next
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    Dim Item As New FilaComprobanteFactura
                    Item.OrdenCompra = Row.Cells("OrdenCompraFacturaReventa").Value
                    Item.Lote = Row.Cells("Lote").Value
                    Item.Secuencia = Row.Cells("Secuencia").Value
                    Item.Articulo = Row.Cells("Articulo").Value
                    Item.Operacion = Row.Cells("Operacion").Value
                    Item.Remito = Row.Cells("Remito").Value
                    Item.Ingreso = Row.Cells("Ingresado").Value
                    Item.Fecha = Row.Cells("Fecha").Value
                    Item.Importe = Row.Cells("Importe").Value
                    Item.Senia = Row.Cells("Senia").Value
                    PListaDeLotes.Add(Item)
                End If
            Next
            Me.Close() : Exit Sub
        End If

        If PEsSeleccionaLote Then
            If Con > 1 Then
                MsgBox("Debe elegir un Solo Item.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Exit Sub
            End If
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    PLote = Row.Cells("Lote").Value
                    PSecuencia = Row.Cells("Secuencia").Value
                    PStock = Row.Cells("Stock").Value
                    Me.Close() : Exit Sub
                End If
            Next
        End If

        If PEsSeleccionaOrden Then
            If Con > 1 Then
                MsgBox("Debe elegir un Solo Item.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Exit Sub
            End If
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    POrden = Row.Cells("OrdenCompra").Value
                    PStock = Row.Cells("Stock").Value
                    Me.Close() : Exit Sub
                End If
            Next
        End If

        If PEsCuentas Then
            If Con > 1 Then
                MsgBox("Debe elegir un Solo Item.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Exit Sub
            End If
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    PCuenta = Row.Cells("Cuenta").Value
                    PUltimaSerie = Row.Cells("UltimaSerie").Value
                    PUltimoNumero = Row.Cells("UltimoNumero").Value
                    PNumeracionInicial = Row.Cells("NumeracionInicial").Value
                    PNumeracionFinal = Row.Cells("NumeracionFinal").Value
                    Me.Close() : Exit Sub
                End If
            Next
        End If

        If PEsChequeParaReemplazo Then
            If Con > 1 Then
                MsgBox("Debe elegir un Solo Item.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Exit Sub
            End If
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    If Row.Cells("Operacion").Value = 1 Then
                        PAbierto = True
                    Else : PAbierto = False
                    End If
                    PClave = Row.Cells("ClaveCheque").Value
                    PMedioPago = Row.Cells("MedioPago").Value
                    Me.Close() : Exit Sub
                End If
            Next
        End If

        If PEsNVLP Then
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    Dim Item As New FilaComprobanteFactura
                    Item.Operacion = Row.Cells("Operacion").Value
                    Item.Indice = Row.Cells("Indice").Value
                    Item.Lote = Row.Cells("Lote").Value
                    Item.Deposito = Row.Cells("Deposito").Value
                    Item.Articulo = Row.Cells("Articulo").Value
                    Item.Secuencia = Row.Cells("Secuencia").Value
                    Item.OperacionRemito = Row.Cells("OperacionRemito").Value
                    Item.Remito = Row.Cells("Remito").Value
                    Item.Cantidad = Row.Cells("Remitido").Value
                    PListaDeLotes.Add(Item)
                End If
            Next
            Me.Close() : Exit Sub
        End If

        If PEsCentroCosto Then
            If Con > 1 Then
                MsgBox("Debe elegir un Solo Item.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Exit Sub
            End If
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    If Row.Cells("Centro").Value = PCentro Then
                        PCentro = 0
                        PNombre = ""
                    Else : PCentro = Row.Cells("Centro").Value
                        PNombre = Row.Cells("Nombre").Value
                    End If
                    Me.Close() : Exit Sub
                End If
            Next
        End If

        If PEsElijeComprobante Then
            If Con > 1 Then
                MsgBox("Debe elegir un Solo Item.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Exit Sub
            End If
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    If Row.Cells("Operacion").Value = 1 Then
                        PLiquidacion = Row.Cells("Compro").Value
                    Else
                        PLiquidacion2 = Row.Cells("Compro").Value
                    End If
                    Me.Close() : Exit Sub
                End If
            Next
        End If

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonMarcarTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMarcarTodos.Click

        For Each Row As DataRow In DtGrid.Rows
            Row("Seleccion") = True
        Next

    End Sub
    Private Sub ButtonDesmarcarTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDesmarcarTodos.Click

        For Each Row As DataRow In DtGrid.Rows
            Row("Seleccion") = False
        Next

    End Sub
    Private Sub GenerarReventa()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn
        Dim GridComboBox As DataGridViewComboBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)
        Grid.Columns.Item("Sel").DataPropertyName = "Seleccion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Fecha"
        GridTextBox.ReadOnly = True
        GridTextBox.Name = "Fecha"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Fecha").DataPropertyName = "Fecha"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 150
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Articulo"
        GridComboBox.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Articulos;")
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.ReadOnly = True
        GridComboBox.Name = "Articulo"
        GridComboBox.SortMode = DataGridViewColumnSortMode.Automatic
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Articulo").DataPropertyName = "Articulo"
        Grid.Columns.Item("Articulo").SortMode = DataGridViewColumnSortMode.Automatic

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.HeaderText = "Orden Compra"
        GridTextBox.Name = "OrdenCompraFacturaReventa"
        GridTextBox.Visible = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("OrdenCompraFacturaReventa").DataPropertyName = "OrdenCompra"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridTextBox.SortMode = DataGridViewColumnSortMode.Automatic

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Ingresado"
        GridTextBox.Name = "Ingresado"
        GridTextBox.ReadOnly = True
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Ingresado").DataPropertyName = "Ingresado"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Remito/Guia"
        GridTextBox.Name = "Remito"
        GridTextBox.ReadOnly = True
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Remito").DataPropertyName = "Remito"
        GridTextBox.SortMode = DataGridViewColumnSortMode.Automatic

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        If GTipoIva = 2 Then
            GridTextBox.HeaderText = "Importe"
        Else
            GridTextBox.HeaderText = "Importe Con Iva"
        End If
        GridTextBox.Name = "Importe"
        GridTextBox.ReadOnly = True
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Importe").DataPropertyName = "Importe"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Seña"
        GridTextBox.Name = "Senia"
        GridTextBox.ReadOnly = True
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Senia").DataPropertyName = "Senia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.HeaderText = "PrecioCompra"
        GridTextBox.Name = "PrecioCompra"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("PrecioCompra").DataPropertyName = "PrecioCompra"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridReventa()

        Dim Saldo As Decimal
        Dim Remitido As Decimal
        Dim Facturado As Decimal
        Dim Baja As Decimal
        Dim Merma As Decimal
        Dim MermaTr As Decimal
        Dim Importe As Decimal
        Dim ImporteSinIva As Decimal
        Dim Senia As Decimal
        Dim PrecioSSinIva As Decimal
        Dim Stock As Decimal
        Dim Liquidado As Decimal
        Dim PrecioCompra As Decimal
        Dim CantidadInicial As Decimal
        Dim Descarga As Decimal
        Dim DescargaSinIva As Decimal
        Dim GastoComercial As Decimal
        Dim GastoComercialSinIva As Decimal

        Dim Dt As New DataTable
        Dim Sql As String

        Sql = "SELECT 1 as Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.Cantidad,L.Baja,C.Guia,C.Remito,C.OrdenCompra,L.KilosXUnidad,L.PrecioCompra,L.Senia,L.MermaTr,L.PrecioF FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza As C ON C.Lote = L.Lote WHERE L.Liquidado = 0 AND L.Cantidad <> L.Baja AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 2 AND L.Proveedor = " & PEmisor & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            Sql = "SELECT 2 as Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.Cantidad,L.Baja,C.Guia,C.Remito,C.OrdenCompra,L.KilosXUnidad,L.PrecioCompra,L.Senia,L.MermaTr,L.PrecioF FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza As C ON C.Lote = L.Lote WHERE L.Liquidado = 0 AND L.Cantidad <> L.Baja AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 2 AND L.Proveedor = " & PEmisor & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        For Each Row As DataRow In Dt.Rows
            If Row("PrecioF") = 0 And Row("OrdenCompra") <> 0 Then
                Row("PrecioF") = HallaOrdenCompraYPrecio(Row("Proveedor"), Row("Articulo"), Row("Operacion"), Row("Lote"))
            End If
            If Row("PrecioF") = 0 Then
                Row("PrecioF") = HallaPrecioDeListaDePrecios(Row("Proveedor"), Row("Lote"), Row("Fecha"), Row("Articulo"), Row("KilosXUnidad"), Row("Operacion"))
            End If
            If Row("PrecioF") = 0 Then Continue For
            '
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("Articulo") = Row("Articulo")
            Row1("Ingresado") = Row("Cantidad") - Row("Baja")
            Row1("Fecha") = Row("Fecha")
            Row1("PrecioCompra") = Row("PrecioCompra")
            Dim MermaAux As Double = 0
            If Row("MermaTr") > 0 Then
                MermaAux = Row("MermaTr")
            End If
            Row1("Importe") = CalculaNeto(Row("Cantidad") - Row("Baja") - MermaAux, Row("PrecioF"))
            Row1("Remito") = Row("Remito")
            If Row1("Remito") = 0 Then Row1("Remito") = Row("Guia")
            If Row1("Importe") <> 0 Then
                If Row("Senia") = -1 Then
                    Senia = HallaSeniaArticulo(Row("Articulo"), Row("Fecha"))
                Else
                    Senia = Row("Senia")
                End If
                Row1("Senia") = CalculaNeto(Row("Cantidad") - Row("Baja"), Senia)
            Else
                Row1("Senia") = 0
            End If
            Row1("OrdenCompra") = Row("OrdenCompra")
            Row1("Seleccion") = False
            DtGrid.Rows.Add(Row1)
        Next

        'Agrega los lotes seleccionados que fueron liquidados si es  una fatura ya hecha.
        Dt.Clear()
        For Each Fila As FilaComprobanteFactura In PListaDeLotesOrigen
            If Fila.Operacion = 1 Then
                Sql = "SELECT 1 as Operacion,Lote,Secuencia,Articulo,Proveedor,Fecha,Cantidad FROM Lotes WHERE Lotes.Lote = Lotes.LoteOrigen AND Lotes.Secuencia = Lotes.SecuenciaOrigen AND Lotes.Deposito = Lotes.DepositoOrigen AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & ";"
                If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
            Else
                Sql = "SELECT 2 as Operacion,Lote,Secuencia,Articulo,Proveedor,Fecha,Cantidad FROM Lotes WHERE Lotes.Lote = Lotes.LoteOrigen AND Lotes.Secuencia = Lotes.SecuenciaOrigen AND Lotes.Deposito = Lotes.DepositoOrigen AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & ";"
                If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
            End If
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Operacion") = Fila.Operacion
            Row1("Lote") = Fila.Lote
            Row1("Secuencia") = Fila.Secuencia
            Row1("Articulo") = Dt.Rows(0).Item("Articulo")
            Row1("Fecha") = Dt.Rows(0).Item("Fecha")
            Row1("Importe") = Fila.Importe
            Row1("Senia") = Fila.Senia
            DtGrid.Rows.Add(Row1)
        Next

        Dim View As New DataView
        View = DtGrid.DefaultView
        View.Sort = "Lote,Secuencia"

        'Marca los seleccionados.
        For Each Item As FilaComprobanteFactura In PListaDeLotes
            For Each Row As DataRow In DtGrid.Rows
                If Row("Lote") = Item.Lote And Row("Secuencia") = Item.Secuencia Then
                    Row("Seleccion") = True : Exit For
                End If
            Next
        Next

        Grid.DataSource = View

        Dt.Dispose()

        If DtGrid.Rows.Count = 0 Then
            MsgBox("NO Existe Comprobantes.") : Me.Close()
        End If

    End Sub
    Private Sub GenerarInsumos()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)
        Grid.Columns.Item("Sel").DataPropertyName = "Seleccion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Orden Compra"
        GridTextBox.Name = "OrdenCompra"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("OrdenCompra").DataPropertyName = "Orden"

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        GridImageBox.Visible = False
        Grid.Columns.Add(GridImageBox)
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 120
        GridTextBox.MaxInputLength = 120
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Remito"
        GridTextBox.Name = "Remito"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Remito").DataPropertyName = "Remito"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Fecha"
        GridTextBox.Name = "Fecha"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Fecha").DataPropertyName = "Fecha"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Ingreso"
        GridTextBox.Visible = False
        GridTextBox.Name = "Ingreso"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Ingreso").DataPropertyName = "Ingreso"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Con Iva"
        GridTextBox.Name = "Importe"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Importe").DataPropertyName = "Importe"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "ImporteB"
        GridTextBox.Name = "ImporteB"
        GridTextBox.Visible = False
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteB").DataPropertyName = "ImporteB"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "ImporteN"
        GridTextBox.Name = "ImporteN"
        GridTextBox.Visible = False
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteN").DataPropertyName = "ImporteN"

        CreaDtGridInsumos()

        Dim SqlB As String = "SELECT 1 AS Operacion,Ingreso,Remito,OrdenCompra,Fecha FROM IngresoInsumoCabeza WHERE Estado = 1 AND Facturado = 0 AND Proveedor = " & PEmisor & ";"

        Dim Dt As New DataTable
        Dim RowGrid As DataRow

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "OrdenCompra,Remito"

        Dim ImporteAux As Decimal = 0

        For Each Row As DataRowView In View
            RowGrid = DtGrid.NewRow
            RowGrid("Operacion") = Row("Operacion")
            RowGrid("Orden") = Row("OrdenCompra")
            RowGrid("Remito") = Row("Remito")
            RowGrid("Fecha") = Row("Fecha")
            RowGrid("Ingreso") = Row("Ingreso")
            RowGrid("ImporteB") = 0
            RowGrid("ImporteN") = 0
            RowGrid("Seleccion") = False
            If HallaImportesRemito(Row("OrdenCompra"), Row("Ingreso"), RowGrid("ImporteB"), RowGrid("ImporteN"), ImporteAux, Row("Operacion")) Then
                RowGrid("Importe") = RowGrid("ImporteB") + RowGrid("ImporteN")
                DtGrid.Rows.Add(RowGrid)
            End If
        Next

        'Marca los seleccionados.
        For Each Item As FilaComprobanteFactura In PListaDeLotes
            For Each Row As DataRow In DtGrid.Rows
                If Row("Orden") = Item.OrdenCompra And Row("Remito") = Item.Remito Then
                    Row("Seleccion") = True : Exit For
                End If
            Next
        Next
        Grid.DataSource = DtGrid

        Dt.Dispose()

    End Sub
    Private Sub GenerarLiquidacionInsumos()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)
        Grid.Columns.Item("Sel").DataPropertyName = "Seleccion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Orden Compra"
        GridTextBox.Name = "OrdenCompra"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("OrdenCompra").DataPropertyName = "Orden"

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 120
        GridTextBox.MaxInputLength = 120
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Remito"
        GridTextBox.Name = "Remito"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Remito").DataPropertyName = "Remito"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Fecha Remito"
        GridTextBox.Name = "Fecha"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Fecha").DataPropertyName = "Fecha"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Ingreso"
        GridTextBox.Visible = False
        GridTextBox.Name = "Ingreso"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Ingreso").DataPropertyName = "Ingreso"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "ImporteB"
        GridTextBox.Name = "ImporteB"
        GridTextBox.Visible = False
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteB").DataPropertyName = "ImporteB"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "ImporteN"
        GridTextBox.Name = "ImporteN"
        GridTextBox.Visible = False
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteN").DataPropertyName = "ImporteN"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridInsumos()

        Dim SqlB As String = "SELECT 1 AS Operacion,FechaRemito,Remito,OrdenCompra,Ingreso FROM IngresoInsumoCabeza WHERE Estado = 1 AND Facturado = 0 AND Proveedor = " & PEmisor & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion,FechaRemito,Remito,OrdenCompra,Ingreso FROM IngresoInsumoCabeza WHERE Estado = 1 AND Facturado = 0 AND Proveedor = " & PEmisor & ";"

        Dim Dt As New DataTable
        Dim RowGrid As DataRow

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "OrdenCompra,Remito"

        Dim ImporteAux As Double = 0

        For Each Row As DataRowView In View
            RowGrid = DtGrid.NewRow
            RowGrid("Operacion") = Row("Operacion")
            RowGrid("Orden") = Row("OrdenCompra")
            RowGrid("Remito") = Row("Remito")
            RowGrid("Fecha") = Row("FechaRemito")
            RowGrid("Ingreso") = Row("Ingreso")
            RowGrid("ImporteB") = 0
            RowGrid("ImporteN") = 0
            RowGrid("Seleccion") = False
            If HallaImportesRemito(Row("OrdenCompra"), Row("Ingreso"), RowGrid("ImporteB"), RowGrid("ImporteN"), ImporteAux, Row("Operacion")) Then
                DtGrid.Rows.Add(RowGrid)
            End If
        Next

        Grid.DataSource = DtGrid

        'Marca los seleccionados.
        For Each Item As FilaComprobanteFactura In PListaDeLotes
            For Each Row As DataRow In DtGrid.Rows
                If Row("Operacion") = Item.Operacion And Row("Ingreso") = Item.Ingreso Then
                    Row("Seleccion") = True : Exit For
                End If
            Next
        Next

        Dt.Dispose()

    End Sub
    Private Sub GenerarCostoInsumo()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Orden Compra"
        GridTextBox.Name = "OrdenCompra"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("OrdenCompra").DataPropertyName = "OrdenCompra"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Cantidad"
        GridTextBox.Name = "Cantidad"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Cantidad").DataPropertyName = "Cantidad"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Sin Iva"
        GridTextBox.Name = "ImporteSinIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Con Iva"
        GridTextBox.Name = "ImporteConIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        Grid.DataSource = PDtDetalleInsumo

        Dim Resul As Integer
        Dim ConexionStr As String
        Dim ImporteB As Double
        Dim PrecioB As Double
        Dim PrecioN As Double
        Dim Iva As Double

        For Each Row As DataGridViewRow In Grid.Rows
            Resul = CostoInsumoOCompra(Row.Cells("OrdenCompra").Value, Pinsumo, Row.Cells("Cantidad").Value, Row.Cells("ImporteConIva").Value, Row.Cells("ImporteSinIva").Value, ImporteB, Conexion, PrecioB, PrecioN, Iva)
            If Resul = -1 Then
                MsgBox("Error Base de Datos.")
                Exit Sub
            End If
        Next

        ButtonAceptar.Visible = False

    End Sub
    Private Sub GenerarSeleccionaLotes()

        PLote = 0
        PSecuencia = 0

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn
        Dim GridComboBox As DataGridViewComboBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 120
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Proveedor"
        GridComboBox.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Proveedor"
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Proveedor").DataPropertyName = "Proveedor"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 100
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Calibre"
        GridComboBox.Name = "Calibre"
        GridComboBox.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 5;")
        Dim RowC As DataRow = GridComboBox.DataSource.NewRow()
        RowC("Clave") = 0
        RowC("Nombre") = ""
        GridComboBox.DataSource.Rows.Add(RowC)
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Calibre").DataPropertyName = "Calibre"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Stock"
        GridTextBox.Name = "Stock"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Stock").DataPropertyName = "Stock"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridSeleccionaLotes()

        Dim SqlB As String = "SELECT 1 as Operacion, Lote,Secuencia,Proveedor,Calibre,Stock FROM Lotes WHERE Deposito = " & PDeposito & " AND Articulo = " & PArticulo & _
                                " AND Stock <> 0 ORDER BY lote,secuencia;"

        Dim SqlN As String = "SELECT 2 as Operacion, Lote,Secuencia, Proveedor,Calibre,Stock FROM Lotes WHERE Deposito = " & PDeposito & " AND Articulo = " & PArticulo & _
                                           " AND Stock <> 0 ORDER BY lote,secuencia;"

        Dim Dt As New DataTable
        If PAbierto Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If PermisoTotal And PAbierto = False Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        For Each Row As DataRow In Dt.Rows
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("Proveedor") = Row("Proveedor")
            Row1("Calibre") = Row("Calibre")
            Row1("Stock") = Row("Stock")
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = DtGrid

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dt.Dispose()

    End Sub
    Private Sub GenerarSeleccionaOrden()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Orden Compra"
        GridTextBox.Name = "OrdenCompra"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("OrdenCompra").DataPropertyName = "Orden"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Stock"
        GridTextBox.Name = "Stock"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Stock").DataPropertyName = "Stock"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridOrden()

        Dim SqlB As String = "SELECT 1 as Operacion,OrdenCompra,Stock FROM StockInsumos WHERE Stock <> 0 AND Articulo = " & PArticulo & " AND Deposito = " & PDeposito & ";"

        Dim Dt As New DataTable
        Dim RowGrid As DataRow

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "OrdenCompra"

        For Each Row As DataRowView In View
            RowGrid = DtGrid.NewRow
            RowGrid("Operacion") = Row("Operacion")
            RowGrid("Orden") = Row("OrdenCompra")
            RowGrid("Stock") = Row("Stock")
            DtGrid.Rows.Add(RowGrid)
        Next

        POrden = 0

        Grid.DataSource = DtGrid

        Dt.Dispose()

    End Sub
    Private Sub GenerarRetenciones()

        Dim GridTextBox As DataGridViewTextBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 70
        GridTextBox.MaxInputLength = 70
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        '    GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Comprobante"
        GridTextBox.Name = "Numero"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Numero").DataPropertyName = "Numero"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 140
        GridTextBox.MaxInputLength = 140
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        '    GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Retención"
        GridTextBox.Name = "Retencion"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Retencion").DataPropertyName = "Retencion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Importe"
        GridTextBox.Name = "Importe"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Importe").DataPropertyName = "Importe"

        CreaDtGridRetenciones()

        For Each Fila As FilaItemsRetencion In PListaDeRetenciones
            Dim Row As DataRow = DtGrid.NewRow
            If Fila.Importe <> 0 Then
                Row("Numero") = Fila.Numero
                Row("Retencion") = Fila.Nombre
                Row("Importe") = Fila.Importe
                DtGrid.Rows.Add(Row)
            End If
        Next

        Grid.DataSource = DtGrid
        ButtonAceptar.Visible = False
        Grid.ReadOnly = True

    End Sub
    Private Sub GenerarCuentas()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn
        Dim GridComboBox As DataGridViewComboBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Codigo Sucursal"
        GridTextBox.Name = "CodigoSucursal"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("CodigoSucursal").DataPropertyName = "Sucursal"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 140
        GridTextBox.MaxInputLength = 140
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Sucursal"
        GridTextBox.Name = "Sucursal"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Sucursal").DataPropertyName = "NombreSucursal"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DefaultCellStyle.Font = style.Font
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 40
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Tipo"
        GridComboBox.DataSource = TiposCuentas
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Tipo"
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Tipo").DataPropertyName = "TipoCuenta"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Cuenta"
        GridTextBox.Name = "Cuenta"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Cuenta").DataPropertyName = "Numero"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DefaultCellStyle.Font = style.Font
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 40
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Moneda"
        GridComboBox.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas Where Tipo = 27;")
        Dim Row1 As DataRow = GridComboBox.DataSource.newrow
        Row1("Nombre") = "Pesos"
        Row1("Clave") = 1
        GridComboBox.DataSource.rows.add(Row1)
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Moneda"
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Moneda").DataPropertyName = "Moneda"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 30
        GridTextBox.MaxInputLength = 30
        GridTextBox.HeaderText = "Serie"
        GridTextBox.Name = "UltimaSerie"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("UltimaSerie").DataPropertyName = "UltimaSerie"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 90
        GridTextBox.MaxInputLength = 90
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Ultimo Numero"
        GridTextBox.Name = "UltimoNumero"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("UltimoNumero").DataPropertyName = "UltimoNumero"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 2
        GridTextBox.MaxInputLength = 2
        GridTextBox.Visible = False
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "NumeracionInicial"
        GridTextBox.Name = "NumeracionInicial"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("NumeracionInicial").DataPropertyName = "NumeracionInicial"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 2
        GridTextBox.MaxInputLength = 2
        GridTextBox.Visible = False
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "NumeracionFinal"
        GridTextBox.Name = "NumeracionFinal"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("NumeracionFinal").DataPropertyName = "NumeracionFinal"

        Dim DtGrid As New DataTable
        Dim Sql As String

        If PEsCheque Then
            Sql = "SELECT * FROM CuentasBancarias WHERE TieneChequera = 1 AND Banco = " & PBanco & " ORDER BY Sucursal;"
        Else : Sql = "SELECT * FROM CuentasBancarias WHERE Banco = " & PBanco & " ORDER BY Sucursal;"
        End If
        If Not Tablas.Read(Sql, Conexion, DtGrid) Then Me.Close() : Exit Sub

        If PSoloPesos Then
            For Each Row As DataRow In DtGrid.Rows
                If Row("Moneda") <> 1 Then Row.Delete()
            Next
        End If

        PCuenta = 0

        Grid.DataSource = DtGrid

    End Sub
    Private Sub GenerarChequesParaReemplazo()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)
        Dim style1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        style1.Font = New Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.ReadOnly = True
        GridTextBox.Visible = False
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "MedioPago"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("MedioPago").DataPropertyName = "MedioPago"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "MedioPagoStr"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("MedioPagoStr").DataPropertyName = "MedioPagoStr"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Clave Cheque"
        GridTextBox.Name = "ClaveCheque"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ClaveCheque").DataPropertyName = "ClaveCheque"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 70
        GridTextBox.MaxInputLength = 70
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe"
        GridTextBox.Name = "Importe"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Importe").DataPropertyName = "Importe"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 40
        GridTextBox.MaxInputLength = 40
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Caja"
        GridTextBox.Name = "Caja"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Caja").DataPropertyName = "Caja"

        CreaDtGridChequesTercero()

        Dim SqlB As String = ""
        Dim SqlN As String = ""

        Dim SqlCaja As String = ""
        If PCaja <> 0 Then SqlCaja = " AND Caja = " & PCaja

        SqlB = "SELECT 1 AS Operacion,ClaveCheque,Caja,MedioPago,Importe FROM Cheques WHERE MedioPago = 2 AND Numero = " & PNumero & " AND Banco = " & PBanco & SqlCaja & ";"
        SqlN = "SELECT 2 AS Operacion,ClaveCheque,Caja,MedioPago,Importe FROM Cheques WHERE MedioPago = 2 AND Numero = " & PNumero & " AND Banco = " & PBanco & SqlCaja & ";"

        Dim Dt As New DataTable
        Dim RowGrid As DataRow

        If PAbierto Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If Not PAbierto Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        If Dt.Rows.Count = 1 Then
            PClave = Dt.Rows(0).Item("ClaveCheque")
            PMedioPago = Dt.Rows(0).Item("MedioPago")
            Dt.Dispose()
            Me.Close() : Exit Sub
        End If
        If Dt.Rows.Count = 0 Then
            PClave = 0
            PMedioPago = 2
            Dt.Dispose()
            Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "ClaveCheque"

        For Each Row As DataRowView In View
            RowGrid = DtGrid.NewRow
            RowGrid("Operacion") = Row("Operacion")
            RowGrid("MedioPago") = Row("MedioPago")
            If Row("MedioPago") = 2 Then
                RowGrid("MedioPagoStr") = "Cheque Propio"
            Else : RowGrid("MedioPagoStr") = "Cheque Terceros"
            End If
            RowGrid("ClaveCheque") = Row("ClaveCheque")
            RowGrid("Importe") = Row("Importe")
            RowGrid("Caja") = Row("Caja")
            DtGrid.Rows.Add(RowGrid)
        Next

        Grid.Sort(Grid.Columns("ClaveCheque"), System.ComponentModel.ListSortDirection.Ascending)

        PClave = 0

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Grid.DataSource = DtGrid

        Dt.Dispose()

    End Sub
    Private Sub GenerarNVLP()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn
        Dim GridComboBox As DataGridViewComboBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)
        Grid.Columns.Item("Sel").DataPropertyName = "Seleccion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "OperacionRemito"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("OperacionRemito").DataPropertyName = "OperacionRemito"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Indice"
        GridTextBox.Name = "Indice"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Indice").DataPropertyName = "Indice"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.ReadOnly = True
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 60
        GridTextBox.MaxInputLength = 60
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "F.Remito"
        GridTextBox.Name = "Fecha"
        GridTextBox.ReadOnly = True
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Fecha").DataPropertyName = "Fecha"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 80
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Deposito"
        GridComboBox.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Deposito"
        GridComboBox.ReadOnly = True
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Deposito").DataPropertyName = "Deposito"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 150
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Articulo"
        GridComboBox.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Articulos;")
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Articulo"
        GridComboBox.ReadOnly = True
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Articulo").DataPropertyName = "Articulo"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Remito"
        GridTextBox.Name = "Remito"
        GridTextBox.ReadOnly = True
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Remito").DataPropertyName = "Remito"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Remitido"
        GridTextBox.Name = "Remitido"
        GridTextBox.ReadOnly = True
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridTextBox.Visible = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Remitido").DataPropertyName = "Remitido"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridNVLP()

        Dim Dt As New DataTable
        Dim SqlB As String
        Dim SqlN As String

        SqlB = "SELECT 1 AS OperacionRemito,C.Remito,C.Fecha,L.Operacion,L.Lote,L.Secuencia,L.Deposito,L.Cantidad,L.Liquidado,L.Indice FROM RemitosCabeza AS C INNER JOIN AsignacionLotes AS L ON C.Remito = L.Comprobante AND L.TipoComprobante = 1 " & _
              "WHERE L.Cantidad <> 0 AND C.Estado = 1 AND C.Factura = 0 AND C.Cliente = " & PEmisor & ";"
        SqlN = "SELECT 2 AS OperacionRemito,C.Remito,C.Fecha,L.Operacion,L.Lote,L.Secuencia,L.Deposito,L.Cantidad,L.Liquidado,L.Indice FROM RemitosCabeza AS C INNER JOIN AsignacionLotes AS L ON C.Remito = L.Comprobante AND L.TipoComprobante = 1 " & _
              "WHERE L.Cantidad <> 0 AND C.Estado = 1 AND C.Factura = 0 AND C.Cliente = " & PEmisor & ";"
        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If
        For Each Row As DataRow In Dt.Rows
            If Row("Liquidado") = False Then
                Dim Row1 As DataRow = DtGrid.NewRow
                Row1("OperacionRemito") = Row("OperacionRemito")
                Row1("Operacion") = Row("Operacion")
                Row1("Remito") = Row("Remito")
                Row1("Indice") = Row("Indice")
                Row1("Lote") = Row("Lote")
                Row1("Secuencia") = Row("Secuencia")
                Row1("Fecha") = Format(Row("Fecha"), "dd/MM/yyyy")
                Row1("Deposito") = Row("Deposito")
                Row1("Remitido") = Row("Cantidad")
                If Row("Operacion") = 1 Then
                    Row1("Articulo") = HallaArticulo(Row("Lote"), Row("Secuencia"), Conexion)
                Else
                    Row1("Articulo") = HallaArticulo(Row("Lote"), Row("Secuencia"), ConexionN)
                End If
                Row1("Seleccion") = False
                DtGrid.Rows.Add(Row1)
            End If
        Next

        Grid.DataSource = DtGrid

        'Marca los seleccionados.
        For Each Item As FilaComprobanteFactura In PListaDeLotes
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Remito").Value = Item.Remito And Row.Cells("Indice").Value = Item.Indice And Row.Cells("Lote").Value = Item.Lote And Row.Cells("Secuencia").Value = Item.Secuencia And Row.Cells("Deposito").Value = Item.Deposito Then
                    Row.Cells("Sel").Value = True : Exit For
                End If
            Next
        Next

        PListaDeLotes.Clear()
        Dt.Dispose()

    End Sub
    Private Sub GenerarDetalleRemito()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn
        Dim GridComboBox As DataGridViewComboBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 80
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Deposito"
        GridComboBox.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Deposito"
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Deposito").DataPropertyName = "Deposito"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 150
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Articulo"
        GridComboBox.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Articulos;")
        Dim Row3 As DataRow = GridComboBox.DataSource.newrow
        Row3("Nombre") = "Lote No Existe"
        Row3("Clave") = 0
        GridComboBox.DataSource.rows.add(Row3)
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Articulo"
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Articulo").DataPropertyName = "Articulo"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 70
        GridTextBox.MaxInputLength = 70
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Remito"
        GridTextBox.Name = "Remito"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Remito").DataPropertyName = "Remito"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 50
        GridTextBox.MaxInputLength = 50
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet
        GridTextBox.HeaderText = "Remitido"
        GridTextBox.Name = "Remitido"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridTextBox.Visible = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Remitido").DataPropertyName = "Remitido"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 50
        GridTextBox.MaxInputLength = 50
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet
        GridTextBox.HeaderText = "Vendido"
        GridTextBox.Name = "Vendido"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridTextBox.Visible = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Vendido").DataPropertyName = "Vendido"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 70
        GridTextBox.MaxInputLength = 70
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Cartel"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Cartel").DataPropertyName = "Cartel"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Comprobante"
        GridTextBox.Name = "Comprobante"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Comprobante").DataPropertyName = "Comprobante"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Recibo oficial"
        GridTextBox.Name = "ReciboOficial"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ReciboOficial").DataPropertyName = "ReciboOficial"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridDetalleRemito()

        Dim Dt As New DataTable
        Dim Sql As String
        Dim Liquidacion As Double
        Dim ReciboOficial As Double
        Dim Vendido As Double

        Sql = "SELECT Operacion,Lote,Secuencia,Deposito,Cantidad,Liquidado FROM AsignacionLotes WHERE TipoComprobante = 1 AND Comprobante = " & PRemito & ";"
        If PAbierto Then
            If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
        Else
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If
        For Each Row As DataRow In Dt.Rows
            Liquidacion = 0
            ReciboOficial = 0
            Vendido = 0
            If Row("Liquidado") = True Then
                Dim OperacionRemito As Integer
                If PAbierto Then
                    OperacionRemito = 1
                Else : OperacionRemito = 2
                End If
                If Not HallaNVLP(Row("Operacion"), Row("Lote"), Row("Secuencia"), Row("Deposito"), OperacionRemito, PRemito, Liquidacion, ReciboOficial, Vendido) Then Me.Close() : Exit Sub
            End If
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("Deposito") = Row("Deposito")
            Row1("Remito") = PRemito
            Row1("Remitido") = Row("Cantidad")
            If Row("Operacion") = 1 Then
                Row1("Articulo") = HallaArticulo(Row("Lote"), Row("Secuencia"), Conexion)
            Else
                Row1("Articulo") = HallaArticulo(Row("Lote"), Row("Secuencia"), ConexionN)
            End If
            If Vendido <> 0 Then
                Row1("Cartel") = "N.V.L.P."
                Row1("Comprobante") = Liquidacion
                Row1("ReciboOficial") = ReciboOficial
                Row1("Vendido") = Vendido
            Else
                Row1("Cartel") = ""
                Row1("Comprobante") = 0
                Row1("ReciboOficial") = 0
                Row1("Vendido") = 0
            End If
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = DtGrid
        ButtonAceptar.Visible = False
        Dt.Dispose()

    End Sub
    Private Sub GenerarDetalleIngreso()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn
        Dim GridComboBox As DataGridViewComboBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 80
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Deposito"
        GridComboBox.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Deposito"
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Deposito").DataPropertyName = "Deposito"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 150
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Articulo"
        GridComboBox.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Articulos;")
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Articulo"
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Articulo").DataPropertyName = "Articulo"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 70
        GridTextBox.MaxInputLength = 70
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Ingreso"
        GridTextBox.Name = "Ingreso"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridTextBox.Visible = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Ingreso").DataPropertyName = "Ingreso"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 70
        GridTextBox.MaxInputLength = 70
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Cartel"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Cartel").DataPropertyName = "Cartel"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Comprobante"
        GridTextBox.Name = "Comprobante"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Comprobante").DataPropertyName = "Comprobante"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridDetalleIngreso()

        Dim Dt As New DataTable
        Dim Sql As String
        Dim ConexionStr As String

        If PAbierto Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Sql = "SELECT Lote,Secuencia,Deposito,Liquidado,Cantidad,Baja FROM Lotes WHERE Lote = LoteOrigen AND Secuencia = SecuenciaOrigen AND Deposito = DepositoOrigen AND Lote = " & PLote & ";"

        If Not Tablas.Read(Sql, ConexionStr, Dt) Then Me.Close() : Exit Sub
        For Each Row As DataRow In Dt.Rows
            Dim Row1 As DataRow = DtGrid.NewRow
            If PAbierto Then
                Row1("Operacion") = 1
            Else
                Row1("Operacion") = 2
            End If
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("Deposito") = Row("Deposito")
            Row1("Ingreso") = Row("Cantidad") - Row("Baja")
            If PAbierto Then
                Row1("Articulo") = HallaArticulo(Row("Lote"), Row("Secuencia"), Conexion)
            Else
                Row1("Articulo") = HallaArticulo(Row("Lote"), Row("Secuencia"), ConexionN)
            End If
            If Row("Liquidado") <> 0 Then
                If Row("Liquidado").ToString.Length < 13 Then
                    Row1("cartel") = "Factura" : Row1("Comprobante") = Row("Liquidado")
                Else
                    Row1("cartel") = "Liquidación" : Row1("Comprobante") = Row("Liquidado")
                End If
            Else
                Row1("cartel") = ""
                Row1("Comprobante") = 0
            End If
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = DtGrid
        ButtonAceptar.Visible = False
        Dt.Dispose()

    End Sub
    Private Sub GenerarCentrosCosto()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Centro"
        GridTextBox.Name = "Centro"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Centro").DataPropertyName = "Centro"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 120
        GridTextBox.MaxInputLength = 120
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Nombre"
        GridTextBox.Name = "Nombre"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Nombre").DataPropertyName = "Nombre"

        Dim Sql As String = "SELECT Centro,Nombre FROM CentrosDeCosto ORDER BY Centro;"

        Dim Dt As New DataTable

        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
        Dim Row As DataRow = Dt.NewRow
        Row("Centro") = 0
        Row("Nombre") = ""
        Dt.Rows.Add(Row)

        Grid.DataSource = Dt

        'Marca los seleccionados.
        For Each Row1 As DataGridViewRow In Grid.Rows
            If Row1.Cells("Centro").Value = PCentro Then
                If PCentro = 0 Then Exit For
                Row1.Cells("Sel").Value = True : Exit For
            End If
        Next

    End Sub
    Private Sub GenerarNetoPorLotes()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Con Iva"
        GridTextBox.Name = "ImporteConIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteConIva").DataPropertyName = "ImporteConIva"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Sin Iva"
        GridTextBox.Name = "ImporteSinIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteSinIva").DataPropertyName = "ImporteSinIva"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridNetoPorLotes()

        Dim Dt As New DataTable
        Dim Sql As String

        If Not PAbierto Then
            Sql = "SELECT 2 as Operacion,D.Lote,D.Secuencia,D.NetoConIva,D.NetoSinIva,C.Nrel FROM LiquidacionCabeza As C INNER JOIN LiquidacionDetalle AS D ON C.Liquidacion = D.Liquidacion WHERE C.Liquidacion = " & PLiquidacion & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
            If Dt.Rows.Count = 0 Then
                MsgBox("Error, Comprobante No Encontrado")
                Dt.Dispose()
                Me.Close() : Exit Sub
            End If
            If Dt.Rows(0).Item("Nrel") <> 0 Then
                Sql = "SELECT 1 as Operacion,Lote,Secuencia,NetoConIva,NetoSinIva,0 AS Nrel FROM LiquidacionDetalle WHERE Liquidacion = " & Dt.Rows(0).Item("Nrel") & ";"
                If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
            End If
        Else
            Sql = "SELECT 1 as Operacion,D.Lote,D.Secuencia,D.NetoConIva,D.NetoSinIva,C.Rel FROM LiquidacionCabeza As C INNER JOIN LiquidacionDetalle AS D ON C.Liquidacion = D.Liquidacion WHERE C.Liquidacion = " & PLiquidacion & ";"
            If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
            If Dt.Rows.Count = 0 Then
                MsgBox("Error, Comprobante No Encontrado")
                Dt.Dispose()
                Me.Close() : Exit Sub
            End If
            If Dt.Rows(0).Item("Rel") And PermisoTotal Then
                Sql = "SELECT 2 as Operacion,Lote,Secuencia,NetoConIva,NetoSinIva,0 AS Nrel FROM LiquidacionCabeza AS C INNER JOIN LiquidacionDetalle AS D ON C.Liquidacion = D.Liquidacion WHERE NRel = " & PLiquidacion & ";"
                If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
            End If
        End If

        Dim TotalConIva As Decimal
        Dim TotalSinIva As Decimal
        Dim Row1 As DataRow

        For Each Row As DataRow In Dt.Rows
            Row1 = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("ImporteConIva") = Row("NetoConIva")
            Row1("ImporteSinIva") = Row("NetoSinIva")
            TotalConIva = TotalConIva + Row("NetoConIva")
            TotalSinIva = TotalSinIva + Row("NetoSinIva")
            DtGrid.Rows.Add(Row1)
        Next

        Row1 = DtGrid.NewRow
        Row1("ImporteConIva") = TotalConIva
        Row1("ImporteSinIva") = TotalSinIva
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = DtGrid

        ButtonAceptar.Visible = False

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dt.Dispose()

    End Sub
    Private Sub GenerarNetoPorLotesNVLP()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Neto Con Iva"
        GridTextBox.Name = "ImporteConIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteConIva").DataPropertyName = "ImporteConIva"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Neto Sin Iva"
        GridTextBox.Name = "ImporteSinIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteSinIva").DataPropertyName = "ImporteSinIva"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridNetoPorLotes()

        Dim Dt As New DataTable
        Dim Sql As String

        If Not PAbierto Then
            Sql = "SELECT D.Operacion,D.Lote,D.Secuencia,D.NetoConIva,D.NetoSinIva,C.NRel FROM NVLPCabeza As C INNER JOIN NVLPLotes AS D ON C.Liquidacion = D.Liquidacion WHERE C.Liquidacion = " & PLiquidacion & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
            If Dt.Rows.Count = 0 Then
                MsgBox("Error, Comprobante No Encontrado")
                Dt.Dispose()
                Me.Close() : Exit Sub
            End If
            If Dt.Rows(0).Item("Nrel") <> 0 Then
                Sql = "SELECT D.Operacion,Lote,Secuencia,NetoConIva,NetoSinIva FROM NVLPLotes WHERE Liquidacion = " & Dt.Rows(0).Item("Nrel") & ";"
                If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
            End If
        Else
            Sql = "SELECT D.Operacion,D.Lote,D.Secuencia,D.NetoConIva,D.NetoSinIva,C.Rel FROM NVLPCabeza As C INNER JOIN NVLPLotes AS D ON C.Liquidacion = D.Liquidacion WHERE C.Liquidacion = " & PLiquidacion & ";"
            If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
            If Dt.Rows.Count = 0 Then
                MsgBox("Error, Comprobante No Encontrado")
                Dt.Dispose()
                Me.Close() : Exit Sub
            End If
            If Dt.Rows(0).Item("Rel") And PermisoTotal Then
                Sql = "SELECT D.Operacion,D.Lote,D.Secuencia,D.NetoConIva,D.NetoSinIva FROM NVLPCabeza AS C INNER JOIN NVLPLotes AS D ON C.Liquidacion = D.Liquidacion WHERE NRel = " & PLiquidacion & ";"
                If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
            End If
        End If

        Dim TotalConIva As Double
        Dim TotalSinIva As Double
        Dim Row1 As DataRow

        For Each Row As DataRow In Dt.Rows
            Row1 = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("ImporteConIva") = Row("NetoConIva")
            Row1("ImporteSinIva") = Row("NetoSinIva")
            TotalConIva = TotalConIva + Row("NetoConIva")
            TotalSinIva = TotalSinIva + Row("NetoSinIva")
            DtGrid.Rows.Add(Row1)
        Next

        Row1 = DtGrid.NewRow
        Row1("ImporteConIva") = TotalConIva
        Row1("ImporteSinIva") = TotalSinIva
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = DtGrid

        ButtonAceptar.Visible = False

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dt.Dispose()

    End Sub
    Private Sub GenerarNetoPorLiquidacionInsumos()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Articulo"
        GridTextBox.Name = "NombreArticulo"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("NombreArticulo").DataPropertyName = "NombreArticulo"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Neto Con Iva"
        GridTextBox.Name = "ImporteConIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteConIva").DataPropertyName = "NetoConIva"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Neto Sin Iva"
        GridTextBox.Name = "ImporteSinIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteSinIva").DataPropertyName = "NetoSinIva"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridNetoPorLiquidacionInsumos()

        Dim Dt As New DataTable
        Dim Sql As String

        If PLiquidacion <> 0 Then
            Sql = "SELECT 1 As Operacion,Articulo,NetoConIva,NetoSinIva FROM LiquidacionInsumosDetalle WHERE Liquidacion = " & PLiquidacion & ";"
            If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
            If Dt.Rows.Count = 0 Then
                MsgBox("Error, Comprobante No Encontrado")
                Dt.Dispose()
                Me.Close() : Exit Sub
            End If
        End If
        If PLiquidacion2 <> 0 Then
            Sql = "SELECT 2 As Operacion,Articulo,NetoConIva,NetoSinIva FROM LiquidacionInsumosDetalle WHERE Liquidacion = " & PLiquidacion2 & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
            If Dt.Rows.Count = 0 Then
                MsgBox("Error, Comprobante No Encontrado")
                Dt.Dispose()
                Me.Close() : Exit Sub
            End If
        End If

        Dim TotalConIva As Double
        Dim TotalSinIva As Double
        Dim Row1 As DataRow

        For Each Row As DataRow In Dt.Rows
            Row1 = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("NombreArticulo") = NombreInsumo(Row("Articulo"))
            Row1("NetoConIva") = Row("NetoConIva")
            Row1("NetoSinIva") = Row("NetoSinIva")
            TotalConIva = TotalConIva + Row("NetoConIva")
            TotalSinIva = TotalSinIva + Row("NetoSinIva")
            DtGrid.Rows.Add(Row1)
        Next

        Row1 = DtGrid.NewRow
        Row1("NetoConIva") = TotalConIva
        Row1("NetoSinIva") = TotalSinIva
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = DtGrid

        ButtonAceptar.Visible = False

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dt.Dispose()

    End Sub
    Private Sub GenerarNetoPorLotesAfectados()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Con Iva"
        GridTextBox.Name = "ImporteConIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteConIva").DataPropertyName = "ImporteConIva"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Sin Iva"
        GridTextBox.Name = "ImporteSinIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteSinIva").DataPropertyName = "ImporteSinIva"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridNetoPorLotes()

        Dim Dt As New DataTable
        Dim Sql As String

        If Not PAbierto Then
            Sql = "SELECT 2 as Operacion,D.Lote,D.Secuencia,D.ImporteConIva,D.ImporteSinIva,C.Nrel FROM FacturasProveedorCabeza As C INNER JOIN ComproFacturados AS D ON C.Factura = D.Factura WHERE C.Factura = " & PLiquidacion & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
            If Dt.Rows.Count = 0 Then
                MsgBox("Error, Comprobante No Encontrado")
                Dt.Dispose()
                Me.Close() : Exit Sub
            End If
            If Dt.Rows(0).Item("Nrel") <> 0 Then
                Sql = "SELECT 1 as Operacion,Lote,Secuencia,ImporteConIva,ImporteSinIva,0 AS Nrel FROM ComproFacturados WHERE Factura = " & Dt.Rows(0).Item("Nrel") & ";"
                If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
            End If
        Else
            Sql = "SELECT 1 as Operacion,D.Lote,D.Secuencia,D.ImporteConIva,D.ImporteSinIva,C.Rel FROM FacturasProveedorCabeza As C Left JOIN ComproFacturados AS D ON C.Factura = D.Factura WHERE C.Factura = " & PLiquidacion & ";"
            If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
            If Dt.Rows(0).Item("Rel") And PermisoTotal Then
                Sql = "SELECT 2 as Operacion,Lote,Secuencia,ImporteConIva,ImporteSinIva,0 AS Nrel FROM FacturasProveedorCabeza AS C INNER JOIN ComproFacturados AS D ON C.Factura = D.Factura WHERE NRel = " & PLiquidacion & ";"
                If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
            End If
        End If

        Dim TotalConIva As Double
        Dim TotalSinIva As Double
        Dim Row1 As DataRow

        For Each Row As DataRow In Dt.Rows
            If Not IsDBNull(Row("Lote")) Then
                Row1 = DtGrid.NewRow
                Row1("Operacion") = Row("Operacion")
                Row1("Lote") = Row("Lote")
                Row1("Secuencia") = Row("Secuencia")
                Row1("ImporteConIva") = Row("ImporteConIva")
                Row1("ImporteSinIva") = Row("ImporteSinIva")
                TotalConIva = TotalConIva + Row("ImporteConIva")
                TotalSinIva = TotalSinIva + Row("ImporteSinIva")
                DtGrid.Rows.Add(Row1)
            End If
        Next

        Row1 = DtGrid.NewRow
        Row1("ImporteConIva") = TotalConIva
        Row1("ImporteSinIva") = TotalSinIva
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = DtGrid

        ButtonAceptar.Visible = False

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dt.Dispose()

    End Sub
    Private Sub GenerarNetoPorLotesFacturaVenta()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Con Iva"
        GridTextBox.Name = "ImporteConIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteConIva").DataPropertyName = "ImporteConIva"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Sin Iva"
        GridTextBox.Name = "ImporteSinIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteSinIva").DataPropertyName = "ImporteSinIva"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridNetoPorLotes()

        Dim Dt As New DataTable
        Dim Sql As String

        If PAbierto Then
            Sql = "SELECT 1 as Operacion,Lote,Secuencia,Importe,ImporteSinIva FROM AsignacionLotes WHERE TipoComprobante = 2 AND Comprobante = " & PFactura & ";"
            If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
        Else
            Sql = "SELECT 2 as Operacion,Lote,Secuencia,Importe,ImporteSinIva FROM AsignacionLotes WHERE TipoComprobante = 2 AND Comprobante = " & PFactura & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim TotalConIva As Decimal
        Dim TotalSinIva As Decimal
        Dim Row1 As DataRow

        For Each Row As DataRow In Dt.Rows
            Row1 = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("ImporteConIva") = Row("Importe")
            Row1("ImporteSinIva") = Row("ImporteSinIva")
            TotalConIva = TotalConIva + Row("Importe")
            TotalSinIva = TotalSinIva + Row("ImporteSinIva")
            DtGrid.Rows.Add(Row1)
        Next

        Row1 = DtGrid.NewRow
        Row1("ImporteConIva") = TotalConIva
        Row1("ImporteSinIva") = TotalSinIva
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = DtGrid

        ButtonAceptar.Visible = False

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dt.Dispose()

    End Sub
    Private Sub GenerarNetoPorLotesTicket()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Con Iva"
        GridTextBox.Name = "ImporteConIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteConIva").DataPropertyName = "ImporteConIva"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Sin Iva"
        GridTextBox.Name = "ImporteSinIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteSinIva").DataPropertyName = "ImporteSinIva"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridNetoPorLotes()

        Dim Dt As New DataTable
        Dim Sql As String

        Sql = "SELECT 1 as Operacion,Lote,Secuencia,Importe,ImporteSinIva FROM AsignacionLotes WHERE TipoComprobante = 5 AND Comprobante = " & PTicket & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        Dim TotalConIva As Decimal
        Dim TotalSinIva As Decimal
        Dim Row1 As DataRow

        For Each Row As DataRow In Dt.Rows
            Row1 = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("ImporteConIva") = Row("Importe")
            Row1("ImporteSinIva") = Row("ImporteSinIva")
            TotalConIva = TotalConIva + Row("Importe")
            TotalSinIva = TotalSinIva + Row("ImporteSinIva")
            DtGrid.Rows.Add(Row1)
        Next

        Row1 = DtGrid.NewRow
        Row1("ImporteConIva") = TotalConIva
        Row1("ImporteSinIva") = TotalSinIva
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = DtGrid

        ButtonAceptar.Visible = False

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dt.Dispose()

    End Sub
    Private Sub GenerarTotalPorLotesConsumosPT()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe"
        GridTextBox.Name = "ImporteConIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteConIva").DataPropertyName = "ImporteConIva"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridNetoPorLotes()

        Dim Dt As New DataTable
        Dim Sql As String

        If PAbierto Then
            Sql = "SELECT 1 as Operacion,Lote,Secuencia,Importe FROM ConsumosPTLotes WHERE Consumo = " & PConsumo & ";"
            If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
        Else
            Sql = "SELECT 2 as Operacion,Lote,Secuencia,Importe FROM ConsumosPTLotes WHERE Consumo = " & PConsumo & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim TotalConIva As Decimal
        Dim Row1 As DataRow

        For Each Row As DataRow In Dt.Rows
            Row1 = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("ImporteConIva") = Row("Importe")
            Row1("ImporteSinIva") = 0
            TotalConIva = TotalConIva + Row("Importe")
            DtGrid.Rows.Add(Row1)
        Next

        Row1 = DtGrid.NewRow
        Row1("ImporteConIva") = TotalConIva
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = DtGrid

        ButtonAceptar.Visible = False

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dt.Dispose()

    End Sub
    Private Sub GenerarImportePorLotesRecibos()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Con Iva"
        GridTextBox.Name = "ImporteConIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteConIva").DataPropertyName = "ImporteConIva"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Sin Iva"
        GridTextBox.Name = "ImporteSinIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteSinIva").DataPropertyName = "ImporteSinIva"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridNetoPorLotes()

        Dim Dt As New DataTable
        Dim Sql As String

        If PAbierto Then
            Sql = "SELECT 1 as Operacion,Lote,Secuencia,ImporteConIva,ImporteSinIva FROM RecibosLotes WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
            If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
        Else
            Sql = "SELECT 2 as Operacion,Lote,Secuencia,ImporteConIva,ImporteSinIva FROM RecibosLotes WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim TotalConIva As Double
        Dim TotalSinIva As Double
        Dim Row1 As DataRow

        For Each Row As DataRow In Dt.Rows
            Row1 = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("ImporteConIva") = Row("ImporteConIva")
            Row1("ImporteSinIva") = Row("ImporteSinIva")
            TotalConIva = TotalConIva + Row("ImporteConIva")
            TotalSinIva = TotalSinIva + Row("ImporteSinIva")
            DtGrid.Rows.Add(Row1)
        Next

        Row1 = DtGrid.NewRow
        Row1("ImporteConIva") = TotalConIva
        Row1("ImporteSinIva") = TotalSinIva
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = DtGrid

        ButtonAceptar.Visible = False

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dt.Dispose()

    End Sub
    Private Sub GenerarImportePorLotesAsientos()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Con Iva"
        GridTextBox.Name = "ImporteConIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteConIva").DataPropertyName = "ImporteConIva"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Sin Iva"
        GridTextBox.Name = "ImporteSinIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteSinIva").DataPropertyName = "ImporteSinIva"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridNetoPorLotes()

        Dim Dt As New DataTable
        Dim Sql As String

        If PAbierto Then
            Sql = "SELECT 1 as Operacion,Lote,Secuencia,ImporteConIva,ImporteSinIva FROM AsientosLotes WHERE Asiento = " & PNota & ";"
            If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
        Else
            Sql = "SELECT 2 as Operacion,Lote,Secuencia,ImporteConIva,ImporteSinIva FROM AsientosLotes WHERE Asiento = " & PNota & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim TotalConIva As Double
        Dim TotalSinIva As Double
        Dim Row1 As DataRow

        For Each Row As DataRow In Dt.Rows
            Row1 = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("ImporteConIva") = Row("ImporteConIva")
            Row1("ImporteSinIva") = Row("ImporteSinIva")
            TotalConIva = TotalConIva + Row("ImporteConIva")
            TotalSinIva = TotalSinIva + Row("ImporteSinIva")
            DtGrid.Rows.Add(Row1)
        Next

        Row1 = DtGrid.NewRow
        Row1("ImporteConIva") = TotalConIva
        Row1("ImporteSinIva") = TotalSinIva
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = DtGrid

        ButtonAceptar.Visible = False

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dt.Dispose()

    End Sub
    Private Sub GenerarImportePorLotesOtrasFacturas()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Visible = False
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Con Iva"
        GridTextBox.Name = "ImporteConIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteConIva").DataPropertyName = "ImporteConIva"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Sin Iva"
        GridTextBox.Name = "ImporteSinIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteSinIva").DataPropertyName = "ImporteSinIva"


        CreaDtGridNetoPorLotes()

        Dim Dt As New DataTable
        Dim Sql As String

        Sql = "SELECT Lote,Secuencia,ImporteConIva,ImporteSinIva FROM OtrasFacturasLotes WHERE Factura = " & PFactura & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        Dim TotalConIva As Double
        Dim TotalSinIva As Double
        Dim Row1 As DataRow

        For Each Row As DataRow In Dt.Rows
            Row1 = DtGrid.NewRow
            Row1("Operacion") = 1
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("ImporteConIva") = Row("ImporteConIva")
            Row1("ImporteSinIva") = Row("ImporteSinIva")
            TotalConIva = TotalConIva + Row("ImporteConIva")
            TotalSinIva = TotalSinIva + Row("ImporteSinIva")
            DtGrid.Rows.Add(Row1)
        Next

        Row1 = DtGrid.NewRow
        Row1("ImporteConIva") = TotalConIva
        Row1("ImporteSinIva") = TotalSinIva
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = DtGrid

        ButtonAceptar.Visible = False

        Dt.Dispose()

    End Sub
    Private Sub GenerarImporteLotesReintegros()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Con Iva"
        GridTextBox.Name = "ImporteConIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteConIva").DataPropertyName = "ImporteConIva"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Sin Iva"
        GridTextBox.Name = "ImporteSinIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteSinIva").DataPropertyName = "ImporteSinIva"

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGridNetoPorLotes()

        Dim Dt As New DataTable
        Dim Sql As String

        Sql = "SELECT 1 as Operacion,D.Lote,D.Secuencia,D.ImporteConIva,D.ImporteSinIva FROM ReintegrosCabeza As C INNER JOIN ReintegrosLotes AS D ON C.Nota = D.Nota WHERE C.Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        Dim TotalConIva As Double
        Dim TotalSinIva As Double
        Dim Row1 As DataRow

        For Each Row As DataRow In Dt.Rows
            Row1 = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("ImporteConIva") = Row("ImporteConIva")
            Row1("ImporteSinIva") = Row("ImporteSinIva")
            TotalConIva = TotalConIva + Row("ImporteConIva")
            TotalSinIva = TotalSinIva + Row("ImporteSinIva")
            DtGrid.Rows.Add(Row1)
        Next

        Row1 = DtGrid.NewRow
        Row1("ImporteConIva") = TotalConIva
        Row1("ImporteSinIva") = TotalSinIva
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = DtGrid

        ButtonAceptar.Visible = False

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dt.Dispose()

    End Sub
    Private Sub GenerarDocuRetenciones()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 140
        GridTextBox.MaxInputLength = 140
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Clave"
        GridTextBox.Name = "Clave"
        GridTextBox.Visible = False
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Clave").DataPropertyName = "Clave"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 300
        GridTextBox.MaxInputLength = 300
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Documento"
        GridTextBox.Name = "Nombre"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Nombre").DataPropertyName = "Nombre"

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)

        CreaDtGridDocuRetenciones()

        If PCodigoRetencion = 1 Then      '1-Retencion.
            Dim Row As DataRow = DtGrid.NewRow
            Row("Clave") = 60
            Row("Nombre") = "Cobranza"
            DtGrid.Rows.Add(Row)
            '
            Row = DtGrid.NewRow
            Row("Clave") = 3000
            Row("Nombre") = "Gasto Bancario"
            DtGrid.Rows.Add(Row)
            '
            Row = DtGrid.NewRow
            Row("Clave") = 1010
            Row("Nombre") = "Cancelación Prestamos"
            DtGrid.Rows.Add(Row)
        Else
            Dim Row As DataRow = DtGrid.NewRow
            '          Row("Clave") = 600
            '         Row("Nombre") = "Orden de Pago"
            '           DtGrid.Rows.Add(Row)
            '
            Row = DtGrid.NewRow
            Row("Clave") = 3
            Row("Nombre") = "Factura Proveedor"
            DtGrid.Rows.Add(Row)
            '
            '        Row = DtGrid.NewRow
            '        Row("Clave") = 100
            '        Row("Nombre") = "Liquidacion a Proveedor"
            '        DtGrid.Rows.Add(Row)
            '
            Row = DtGrid.NewRow
            Row("Clave") = 800
            Row("Nombre") = "N.V.L.P."
            DtGrid.Rows.Add(Row)
            '
            Row = DtGrid.NewRow
            Row("Clave") = 3000
            Row("Nombre") = "Gasto Bancario"
            DtGrid.Rows.Add(Row)
            '
            '     Row = DtGrid.NewRow
            '     Row("Clave") = 5
            '     Row("Nombre") = "N.D. a Cliente"
            '     DtGrid.Rows.Add(Row)
            '
            '   Row = DtGrid.NewRow
            '   Row("Clave") = 7
            '   Row("Nombre") = "N.C. a Cliente"
            '   DtGrid.Rows.Add(Row)
            '
            '  Row = DtGrid.NewRow
            '  Row("Clave") = 6
            '  Row("Nombre") = "N.D. a Proveedor"
            '  DtGrid.Rows.Add(Row)
            '
            ' Row = DtGrid.NewRow
            ' Row("Clave") = 8
            ' Row("Nombre") = "N.C. a Proveedor"
            ' DtGrid.Rows.Add(Row)
            '
            Row = DtGrid.NewRow
            Row("Clave") = 50
            Row("Nombre") = "N.D. del Cliente"
            DtGrid.Rows.Add(Row)
            '
            Row = DtGrid.NewRow
            Row("Clave") = 70
            Row("Nombre") = "N.C. del Cliente"
            DtGrid.Rows.Add(Row)
            '
            Row = DtGrid.NewRow
            Row("Clave") = 500
            Row("Nombre") = "N.D. del Proveedor"
            DtGrid.Rows.Add(Row)
            '
            Row = DtGrid.NewRow
            Row("Clave") = 700
            Row("Nombre") = "N.C. del proveedor"
            DtGrid.Rows.Add(Row)
            '
            Row = DtGrid.NewRow
            Row("Clave") = 1010
            Row("Nombre") = "Cancelación Prestamo"
            DtGrid.Rows.Add(Row)
        End If

        Grid.DataSource = DtGrid

        For Each Row1 As DataGridViewRow In Grid.Rows
            For Each Fila As FilaItemsRetencion In PListaDeRetenciones
                If Fila.Clave = Row1.Cells("Clave").Value Then Row1.Cells("Sel").Value = True : Exit For
            Next
        Next

    End Sub
    Private Sub GenerarRetencionesExentas()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 140
        GridTextBox.MaxInputLength = 140
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Clave"
        GridTextBox.Name = "Clave"
        GridTextBox.Visible = False
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Clave").DataPropertyName = "Clave"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 300
        GridTextBox.MaxInputLength = 300
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Nombre"
        GridTextBox.Name = "Nombre"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Nombre").DataPropertyName = "Nombre"

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 25;", Conexion, Dt) Then Me.Close() : Exit Sub

        Grid.DataSource = Dt

        For Each Row1 As DataGridViewRow In Grid.Rows
            For Each Fila As FilaItemsRetencion In PListaDeRetenciones
                If Fila.Clave = Row1.Cells("Clave").Value Then Row1.Cells("Sel").Value = True : Exit For
            Next
        Next

    End Sub
    Private Sub MuestraPercepciones()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 140
        GridTextBox.MaxInputLength = 140
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Clave"
        GridTextBox.Name = "Clave"
        GridTextBox.Visible = False
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Clave").DataPropertyName = "Clave"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 300
        GridTextBox.MaxInputLength = 300
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Nombre"
        GridTextBox.Name = "Nombre"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Nombre").DataPropertyName = "Nombre"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 120
        GridTextBox.MaxInputLength = 120
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Importe"
        GridTextBox.Name = "Importe"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Importe").DataPropertyName = "Importe"

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT T.Clave,T.Nombre,0.0 AS Importe FROM Tablas AS T INNER JOIN DocuRetenciones AS D ON T.Clave = D.Clave WHERE T.Tipo = 25 AND D.TipoDocumento = " & PTipoNota & ";", Conexion, Dt) Then Me.Close() : Exit Sub
        For Each Row1 As DataRow In Dt.Rows
            For Each Fila As ItemIvaReten In PListaDePercepciones
                If Fila.Clave = Row1("Clave") Then Row1("Importe") = Fila.Importe
            Next
        Next

        Grid.DataSource = Dt

        ButtonAceptar.Visible = False

    End Sub
    Private Sub GenerarValesTercerosOrdenPago()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Recibo"
        GridTextBox.Name = "Recibo"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Recibo").DataPropertyName = "Nota"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Vale"
        GridTextBox.Name = "Vale"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Vale").DataPropertyName = "Vale"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Fecha Vale"
        GridTextBox.Name = "Fecha"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Fecha").DataPropertyName = "FechaVale"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 120
        GridTextBox.MaxInputLength = 120
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Importe"
        GridTextBox.Name = "Importe"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Importe").DataPropertyName = "Importe"

        Dim SqlB As String = ""
        Dim SqlN As String = ""

        If PEmisor = 0 Then
            SqlB = "SELECT 1 AS Operacion,Nota,Vale,Importe,FechaVale FROM RecuperoSenia WHERE Usado = 0 AND Estado = 1 AND PaseCaja = " & PCaja & ";"
            SqlN = "SELECT 2 AS Operacion,Nota,Vale,Importe,FechaVale FROM RecuperoSenia WHERE Usado = 0 AND Estado = 1 AND PaseCaja = " & PCaja & ";"
        Else
            SqlB = "SELECT 1 AS Operacion,Nota,Vale,Importe,FechaVale FROM RecuperoSenia WHERE Usado = 0 AND Estado = 1 AND PaseCaja = " & PCaja & " AND Proveedor = " & PEmisor & ";"
            SqlN = "SELECT 2 AS Operacion,Nota,Vale,Importe,FechaVale FROM RecuperoSenia WHERE Usado = 0 AND Estado = 1 AND PaseCaja = " & PCaja & " AND Proveedor = " & PEmisor & ";"
        End If

        Dim Dt As New DataTable

        If PAbierto Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If Not PAbierto Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        SqlB = "SELECT 1 AS Operacion,P.Recibo FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS P ON C.Pase = P.Pase WHERE P.MedioPago = 6 AND C.Aceptado = 0 AND C.CajaOrigen = " & PCaja & ";"
        SqlN = "SELECT 2 AS Operacion,P.Recibo FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS P ON C.Pase = P.Pase WHERE P.MedioPago = 6 AND C.Aceptado = 0 AND C.CajaOrigen = " & PCaja & ";"

        Dim Dtaux As New DataTable
        If PAbierto Then
            If Not Tablas.Read(SqlB, Conexion, Dtaux) Then Me.Close() : Exit Sub
        Else
            If Not Tablas.Read(SqlN, ConexionN, Dtaux) Then Me.Close() : Exit Sub
        End If

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In Dtaux.Rows
            RowsBusqueda = Dt.Select("Nota = " & Row("Recibo") & " AND Operacion = " & Row("Operacion"))
            If RowsBusqueda.Length <> 0 Then RowsBusqueda(0).Delete()
        Next

        Grid.DataSource = Dt

        'Marca los seleccionados.
        For Each Item As ItemRecuperoSenia In PListaDeRecuperoSenia
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Recibo").Value = Item.Nota Then
                    Row.Cells("Sel").Value = True : Exit For
                End If
            Next
        Next

        PListaDeRecuperoSenia.Clear()

        Dtaux.Dispose()

    End Sub
    Private Sub GenerarConsumosDeLotes()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Visible = False
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Con Iva"
        GridTextBox.Name = "ImporteConIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteConIva").DataPropertyName = "ImporteConIva"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 110
        GridTextBox.MaxInputLength = 110
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe Sin Iva"
        GridTextBox.Name = "ImporteSinIva"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ImporteSinIva").DataPropertyName = "ImporteSinIva"

        CreaDtGridNetoPorLotes()

        Dim Dt As New DataTable
        Dim Sql As String

        Sql = "SELECT * FROM ConsumosLotes WHERE Consumo = " & PConsumo & ";"

        If PAbierto Then
            If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
        Else
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim TotalConIva As Double
        Dim TotalSinIva As Double
        Dim Row1 As DataRow

        For Each Row As DataRow In Dt.Rows
            Row1 = DtGrid.NewRow
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("ImporteConIva") = Row("ImporteConIva")
            Row1("ImporteSinIva") = Row("ImporteSinIva")
            TotalConIva = TotalConIva + Row("ImporteConIva")
            TotalSinIva = TotalSinIva + Row("ImporteSinIva")
            DtGrid.Rows.Add(Row1)
        Next

        Row1 = DtGrid.NewRow
        Row1("ImporteConIva") = TotalConIva
        Row1("ImporteSinIva") = TotalSinIva
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = DtGrid

        ButtonAceptar.Visible = False

        Dt.Dispose()

    End Sub
    Private Sub GenerarElijeComprobante()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Visible = True
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 150
        GridTextBox.MaxInputLength = 130
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.HeaderText = "Comprbante"
        GridTextBox.Name = "Compro"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)


        Grid.Rows.Add(Nothing, 1, False, PLiquidacion)
        Grid.Rows.Add(Nothing, 2, False, PLiquidacion2)

        PLiquidacion = 0 : PLiquidacion2 = 0

    End Sub
    Private Sub CreaDtGridReventa()

        DtGrid = New DataTable

        Dim Sel As New DataColumn("Seleccion")
        Sel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Sel)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Ingresado As New DataColumn("Ingresado")
        Ingresado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Ingresado)

        Dim Remito As New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Remito)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim Senia As New DataColumn("Senia")
        Senia.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Senia)

        Dim PrecioCompra As New DataColumn("PrecioCompra")
        PrecioCompra.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(PrecioCompra)

        Dim PrecioS As New DataColumn("PrecioS")
        PrecioS.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(PrecioS)

        Dim PrecioF As New DataColumn("PrecioF")
        PrecioF.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(PrecioF)

        Dim OrdenCompra As New DataColumn("OrdenCompra")
        OrdenCompra.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(OrdenCompra)

    End Sub
    Private Sub CreaDtGridNetoPorLotes()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim ImporteConIva As New DataColumn("ImporteConIva")
        ImporteConIva.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ImporteConIva)

        Dim ImporteSinIva As New DataColumn("ImporteSinIva")
        ImporteSinIva.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ImporteSinIva)

    End Sub
    Private Sub CreaDtGridNetoPorLiquidacionInsumos()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Ingreso As New DataColumn("Ingreso")
        Ingreso.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Ingreso)

        Dim NombreArticulo As New DataColumn("NombreArticulo")
        NombreArticulo.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(NombreArticulo)

        Dim NetoConIva As New DataColumn("NetoConIva")
        NetoConIva.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(NetoConIva)

        Dim NetoSinIva As New DataColumn("NetoSinIva")
        NetoSinIva.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(NetoSinIva)

    End Sub
    Private Sub CreaDtGridCostoLotes()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Remito As New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Remito)

        Dim Guia As New DataColumn("Guia")
        Guia.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Guia)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cantidad)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

    End Sub
    Private Sub CreaDtGridInsumos()

        DtGrid = New DataTable

        Dim Seleccion As New DataColumn("Seleccion")
        Seleccion.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Seleccion)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Orden As New DataColumn("Orden")
        Orden.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Orden)

        Dim Remito As New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Remito)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Ingreso As New DataColumn("Ingreso")
        Ingreso.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Ingreso)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Importe)

        Dim ImporteB As New DataColumn("ImporteB")
        ImporteB.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ImporteB)

        Dim ImporteN As New DataColumn("ImporteN")
        ImporteN.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ImporteN)

        Dim Entrega As New DataColumn("Entrega")
        Entrega.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Entrega)

    End Sub
    Private Sub CreaDtGridOrden()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Orden As New DataColumn("Orden")
        Orden.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Orden)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Stock)

    End Sub
    Private Sub CreaDtGridSeleccionaLotes()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Calibre As New DataColumn("Calibre")
        Calibre.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Calibre)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Stock)

    End Sub
    Private Sub CreaDtGridRetenciones()

        DtGrid = New DataTable
        Dim Numero As New DataColumn("Numero")
        Numero.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Numero)

        Dim Retencion As New DataColumn("Retencion")
        Retencion.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Retencion)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

    End Sub
    Private Sub CreaDtGridChequesTercero()

        DtGrid = New DataTable

        Dim MedioPago As New DataColumn("MedioPago")
        MedioPago.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(MedioPago)

        Dim MedioPagoStr As New DataColumn("MedioPagoStr")
        MedioPagoStr.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(MedioPagoStr)

        Dim ClaveCheque As New DataColumn("ClaveCheque")
        ClaveCheque.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ClaveCheque)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Emisor As New DataColumn("Emisor")
        Emisor.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Emisor)

        Dim Banco As New DataColumn("Banco")
        Banco.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Banco)

        Dim Serie As New DataColumn("Serie")
        Serie.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Serie)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Numero As New DataColumn("Numero")
        Numero.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Numero)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim Caja As New DataColumn("Caja")
        Caja.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Caja)

        Dim EmisorCheque As New DataColumn("EmisorCheque")
        EmisorCheque.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(EmisorCheque)

    End Sub
    Private Sub CreaDtGridNVLP()

        DtGrid = New DataTable

        Dim Seleccion As New DataColumn("Seleccion")
        Seleccion.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Seleccion)

        Dim OperacionRemito As New DataColumn("OperacionRemito")
        OperacionRemito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(OperacionRemito)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Indice As New DataColumn("Indice")
        Indice.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Indice)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Deposito As New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Remitido As New DataColumn("Remitido")
        Remitido.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Remitido)

        Dim Remito As New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Remito)

    End Sub
    Private Sub CreaDtGridDetalleRemito()

        DtGrid = New DataTable

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

        Dim Remitido As New DataColumn("Remitido")
        Remitido.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Remitido)

        Dim Vendido As New DataColumn("Vendido")
        Vendido.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Vendido)

        Dim Remito As New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Remito)

        Dim Cartel As New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante)

        Dim ReciboOficial As New DataColumn("ReciboOficial")
        ReciboOficial.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ReciboOficial)

    End Sub
    Private Sub CreaDtGridDetalleIngreso()

        DtGrid = New DataTable

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

        Dim Ingreso As New DataColumn("Ingreso")
        Ingreso.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Ingreso)

        Dim Cartel As New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante)

    End Sub
    Private Sub CreaDtGridDocuRetenciones()

        DtGrid = New DataTable

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Nombre)

    End Sub
    Private Sub CreaDtGridPedido()

        DtGrid = New DataTable

        Dim Sel As New DataColumn("Sel")
        Sel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Sel)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim ArticuloNombre As New DataColumn("ArticuloNombre")
        ArticuloNombre.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(ArticuloNombre)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

    End Sub
    Private Function TiposCuentas() As DataTable

        DtGrid = New DataTable

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Nombre)

        Try
            Dim Row As DataRow = DtGrid.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            DtGrid.Rows.Add(Row)
            Row = DtGrid.NewRow
            Row("Clave") = 1
            Row("Nombre") = "CA"
            DtGrid.Rows.Add(Row)
            Row = DtGrid.NewRow
            Row("Clave") = 2
            Row("Nombre") = "CC"
            DtGrid.Rows.Add(Row)
            Return DtGrid
        Finally
            DtGrid.Dispose()
        End Try

    End Function
    Private Function Completa(ByVal DtW As DataTable) As Boolean

        Dim CantidadOrdenada As Double = 0
        Dim CantidadRecibida As Double = 0

        For Each Row As DataRow In DtW.Rows
            CantidadOrdenada = CantidadOrdenada + Row("Cantidad")
            CantidadRecibida = CantidadRecibida + Row("Recibido")
        Next

        If CantidadOrdenada <> CantidadRecibida Then
            Return False
        Else : Return True
        End If

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "OrdenCompra" Then
            e.Value = Format(e.Value, "00000000")
            If PermisoTotal Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "OrdenCompraFacturaReventa" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, 0)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
                If PermisoTotal Then
                    If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                        If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                        If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                    End If
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "ClaveCheque" And PEsChequeParaReemplazo Then
            If PermisoTotal Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "NombreArticulo" Then
            If PermisoTotal Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "Senia" Or Grid.Columns(e.ColumnIndex).Name = "ImporteConIva" Or Grid.Columns(e.ColumnIndex).Name = "ImporteSinIva" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Or Grid.Columns(e.ColumnIndex).Name = "ReciboOficial" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = NumeroEditado(e.Value)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Compro" Then
            If PermisoTotal Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                End If
            End If
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = NumeroEditado(e.Value)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Remitido" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Vendido" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Banco" Then
            If PermisoTotal Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Then
            e.Value = FormatNumber(e.Value, 0)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "UltimoNumero" Or Grid.Columns(e.ColumnIndex).Name = "Cuenta" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, 0)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Stock" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = Format(e.Value, "0000-00000000")
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Caja" Then
            e.Value = Format(e.Value, "0000")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Centro" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "000")
            End If
        End If

    End Sub

   
  
End Class