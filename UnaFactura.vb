Imports System.Transactions
Imports System.Drawing.Printing
Imports ClassPassWord
Public Class UnaFactura
    Public PFactura As Double
    Public PCliente As Double
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    Public PEsElectronica As Boolean
    Public PEsContable As Boolean
    '   
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtFacturaPercepciones As DataTable
    Dim DtAsignacionLotes As DataTable
    Dim DtCabezaRemito As DataTable
    Dim DtAsignacionLotesRemito As New DataTable
    Dim DtPedido As DataTable
    Dim DtDetallePedido As DataTable
    '
    Dim ListaDeLotes As New List(Of FilaAsignacion)
    Dim ListaDePrecios As New List(Of ItemListaDePrecios)
    Dim ListaRemitos As New List(Of ItemRemito)
    Dim ListaRemitosParaImpresion As List(Of String)
    Dim ListaDePercepciones As New List(Of ItemIvaReten)
    ' 
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim TipoFactura As Integer
    Dim Deposito As Integer
    Dim Remito As Double
    Dim AbiertoRemito As Boolean
    Dim FechaEntrega As Date
    Dim Pedido As Integer
    Dim Sucursal As Integer
    Dim EsServicios As Boolean
    Dim EsSecos As Boolean
    Dim EsContable As Boolean
    Dim EsArticulosLoteados As Boolean
    Dim ClienteOpr As Boolean
    Dim DocTipo As Integer
    Dim DocNro As Decimal
    Dim TextoFijoParaFacturas1 As String
    Dim TextoFijoParaFacturas2 As String
    '
    'Para Facturas Z.
    Dim EsZ As Boolean
    Dim EsTicket As Boolean
    Dim EsTicketBCAM As Boolean
    Dim ComprobanteDesde As Integer
    Dim ComprobanteHasta As Integer

    Dim PermiteMuchosArticulos As Boolean
    Dim IndiceW As Integer
    Dim Total As Decimal
    Dim TotalB As Decimal
    Dim TotalN As Decimal
    Dim TotalGralB As Decimal
    Dim PagoB As Decimal
    Dim PagoN As Decimal
    Dim TotalNetoPerc As Decimal
    Dim TotalPercepciones As Decimal
    Dim ConexionFactura As String
    Dim ConexionRelacionada As String
    Dim ConexionRemito As String
    Dim CtaCteAbierta As Boolean
    Dim CtaCteCerrada As Boolean
    Dim IndiceCoincidencia As Integer
    Dim cb As ComboBox
    Dim UltimoNumero As Double
    Dim ConexionLotes As String = ""
    Dim UltimoImporteRecibo As Double
    Dim Relacionada As Double
    Dim AsignadoEnRemito As Boolean
    Dim SenaTotal As Decimal
    Dim Bultos As Decimal
    Dim UltimaFechaW As DateTime
    Dim UltimafechaContableW As DateTime
    Dim Lista As Integer
    Dim PorUnidadEnLista As Boolean
    Dim FinalEnLista As Boolean
    Dim TieneCodigoCliente As Boolean
    Dim TipoAsiento As Integer
    Dim TipoAsientoCosto As Integer
    Dim TipoListaDePreciosDelCliente As Integer
    Dim EsExterior As Boolean
    Dim SucursalRemito As Integer
    Dim FechaContableW As Date
    Dim EsfacturaElectronica As Boolean
    Dim FacturaAnteriorOK As Boolean
    Dim IgualClienteW As Integer
    Dim IgualDepositoW As Integer
    Dim PuedeModificarPrecios As Boolean
    Dim FacturaBorrada As Boolean
    '
    Dim Calle As String
    Dim Numero As String
    Dim Localidad As String
    Dim Provincia As String
    Dim Cuit As String
    'para FCE.
    Dim EsFCE As Boolean
    Dim Minimo As Decimal
    Dim CBU As String
    Dim AgenteDeposito As String
    ' Para impresion.
    Dim SinRemitos As Boolean
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim ContadorCopias As Integer = 0
    Dim ContadorPaginas As Integer = 0
    Dim TotalPaginas As Integer = 0
    Dim ContadorLineasListado As Integer = 0
    Dim Listado As List(Of ItemListado)
    Dim CopiasSegunPuntoVenta As Integer = 0
    Dim UltimoPuntoVentaParaCopiaSeleccionado As Integer = 0
    Private Sub UnaFactura_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(6) Then
            PBloqueaFunciones = True
        Else
            If PermisoEscritura(600) Then PuedeModificarPrecios = True
        End If

        Grid.AutoGenerateColumns = False

        TextDescuentoSobreFactura.Text = "0,00"
        TextDirecto.Text = "0"
        Remito = 0
        AsignadoEnRemito = False
        Pedido = 0
        Sucursal = 0
        Lista = 0
        PermiteMuchosArticulos = False
        EsArticulosLoteados = False
        'Para Facturas Z.
        EsZ = False
        ComprobanteDesde = 0
        ComprobanteHasta = 0
        EsTicket = False
        EsTicketBCAM = False

        If PFactura = 0 Then
            PideDatosEmisor()
            If PCliente = 0 Then Me.Close() : Exit Sub
        End If

        Panel4.Visible = True

        LlenaComboTablas(ComboDeposito, 19)

        LlenaComboTablas(ComboVendedor, 37)
        ComboVendedor.SelectedValue = 0
        With ComboVendedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboIncoTerm, 38)
        ComboIncoTerm.SelectedValue = 0
        With ComboIncoTerm
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboClienteOperacion.DataSource = Nothing
        ComboClienteOperacion.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes WHERE DeOperacion = 1;")
        Dim Row As DataRow = ComboClienteOperacion.DataSource.newrow
        Row("Nombre") = " "
        Row("Clave") = 0
        ComboClienteOperacion.DataSource.rows.add(Row)
        ComboClienteOperacion.DisplayMember = "Nombre"
        ComboClienteOperacion.ValueMember = "Clave"

        ComboMoneda.DataSource = Nothing
        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Row = ComboMoneda.DataSource.newrow
        Row("Nombre") = "PESOS"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"
        With ComboMoneda
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboEstado.DataSource = DtAfectaPendienteAnulada()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Codigo"
        ComboEstado.SelectedValue = 0

        ArmaTipoIva(ComboTipoIva)

        LlenaComboTablas(ComboPais, 28)

        If AbiertoRemito Then
            ConexionRemito = Conexion
        Else : ConexionRemito = ConexionN
        End If

        GModificacionOk = False

        If Not MuestraDatos() Then Me.Close() : Exit Sub

        If ListaRemitos.Count <> 0 Then   'Si tiene remitos define las listas de precios de acurdo a la fecha de los remitos.
            TipoListaDePreciosDelCliente = TieneListaDePreciosW(PCliente)
            If Not ArmaConDetalleDeRemitos() Then Me.Close() : Exit Sub
            If ListaDeLotes.Count <> 0 And PFactura = 0 Then ComboEstado.SelectedValue = 1 : Grid.Refresh()
            Grid.Columns("Cantidad").ReadOnly = True
            Grid.Columns("Articulo").ReadOnly = True
            Grid.Columns("KilosXUnidad").ReadOnly = True
            ButtonEliminarLinea.Visible = False
            If PFactura = 0 And Not AbiertoRemito Then
                TextDirecto.Text = Format(0, "0.00")
                TextDirecto.ReadOnly = True
                TextAutorizar.Text = Format(100, "0.00")
            End If
            CalculaSubTotal()
        End If

        If Pedido <> 0 Then
            If Lista = 0 Then   'Muestra parametros Por Kg/Ini Por Final/Sin Iva Si no tiene lista de precios.
                If Not ControlaParametrosPedido(Pedido) Then Me.Close() : Exit Sub
            End If
            If Not ArmaConDetalleDePedido() Then Me.Close() : Exit Sub
            CalculaSubTotal()
        End If

        ''''''arreglo     If PFactura = 0 And Lista <> 0 And EsServicios = False And ListaRemitos.Count = 0 And Pedido = 0 Then
        'Muestra parametros Por Kg/Ini Por Final/Sin Iva Si tiene lista de precios.
        If Lista <> 0 Then Panel4.Visible = False
        If PFactura = 0 And Lista <> 0 And EsServicios = False And ListaRemitos.Count = 0 Then
            Panel4.Visible = False
            If FinalEnLista Then
                RadioFinal.Checked = True
                RadioSinIva.Enabled = False
            Else
                RadioSinIva.Checked = True
                RadioFinal.Enabled = False
            End If
            If GCuitEmpresa = "30-70908844-7" And PCliente = 2 Then 'arreglo para patagonia y cliente DIA ARGENTINA.
                RadioSinIva.Enabled = True
                RadioFinal.Enabled = True
                'RadioPorKilo.Enabled = True
                'RadioPorUnidad.Enabled = True
            End If
        End If

        LlenaCombosGrid()

        ComboTipoPrecio.DisplayMember = "Text"
        ComboTipoPrecio.ValueMember = "Value"
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(Integer))
        tb.Rows.Add("", 0)
        tb.Rows.Add("Uni.", 1)
        tb.Rows.Add("Kgs.", 2)
        ComboTipoPrecio.DataSource = tb

        If EsServicios Or EsSecos Then
            Grid.Columns("KilosXUnidad").ReadOnly = True
            ButtonReAsignaLotes.Enabled = False
            Panel4.Enabled = False
            PanelSenia.Visible = False
            CheckSeniaEnvase.Checked = False
            CheckSeniaManual.Enabled = False
            ButtonNetoPorLotes.Visible = False
        End If

        If EsSecos Then
            UltimafechaContableW = UltimaFechacontable(Conexion, 4)
            If UltimafechaContableW = "2/1/1000" Then
                MsgBox("Error Base de Datos al Leer Ultima Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
            End If
        End If

        If EsfacturaElectronica And PFactura = 0 Then
            If Not VerificaRecursosAFIP("C:\XML Afip\") Then Me.Close() : Exit Sub
            UltimafechaContableW = UltimaFechacontableFactura(Conexion, GPuntoDeVenta, TipoFactura)
            If UltimafechaContableW = "2/1/1000" Then
                MsgBox("Error Base de Datos al Leer Ultima Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
            End If
        End If

        If Lista <> 0 Then
            If PuedeModificarPrecios Then
                Grid.Columns("PrecioLista").ReadOnly = False
                Grid.Columns("TipoPrecio").ReadOnly = False
            Else
                Grid.Columns("PrecioLista").ReadOnly = True
                Grid.Columns("TipoPrecio").ReadOnly = True
            End If
        End If

        UnTextoParaRecibo.Dispose()

        If Not EsZ Then
            Label9.Text = "FACTURA"
            Panel10.Visible = False
        Else
            Select Case TipoFactura
                Case 1
                    Label36.Text = "TICKET A"
                Case 2
                    Label36.Text = "TICKET B"
                Case 3
                    Label36.Text = "TICKET C"
                Case 5
                    Label36.Text = "TICKET M"
                Case 9
                    Label36.Text = "TICKET"
            End Select
            Panel10.Visible = True
        End If

        If PFactura = 0 Then
            If Pedido = 0 And Remito = 0 And EsArticulosLoteados = False And EsServicios = False And EsZ = False Then ButtonImportacionExcel.Visible = True
            If Not AgregaListaPercepciones() Then Me.Close() : Exit Sub
        End If

        UltimaFechaW = UltimaFecha(Conexion)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

        ''''''''''''   If GCuitEmpresa <> GEdeal Then ButtonImportacionExcel.Visible = False

        If GCuitEmpresa <> GEdeal Then ButtonImportacionExcel.Visible = False

        If EsArticulosLoteados Then ButtonArticulosEnStock_Click(Nothing, Nothing)

    End Sub
    Private Sub UnaFactura_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If PFactura <> 0 Then
            If ComboTipoIva.SelectedValue = 3 And Not EsZ And ComboEstado.SelectedValue <> 3 Then
                If Not TieneRecibos() And Not FacturaBorrada Then
                    If MsgBox("Debe Generar Recibo de Pago. Quiere SALIR de la Factura Igualmente? (S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                        e.Cancel = True
                        Exit Sub
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub UnaFactura_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()
        Grid.EndEdit()

        If Not Valida() Then Exit Sub

        If CheckConfirmado.Checked And Not IsNothing(DtDetalle.GetChanges) Then
            MsgBox("Factura fue Confirmada por el Cliente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        DtDetallePedido = New DataTable
        If PFactura = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            If Not ActualizaPedido("A", DtDetallePedido) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End If

        If PFactura = 0 Then
            HacerAlta()
        Else : HacerModificacion()
        End If

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PFactura = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Factura ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("EsExterior") Then
            MsgBox("Factura Exportación No Puede Anularse. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsfacturaElectronica Then
            MsgBox("Factura Electrónica Solo se Anula con Nota de Crédito. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not PermisoTotal And DtCabeza.Rows(0).Item("Rel") Then
            MsgBox("Usuario No Autorizado(1000).")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Valida() Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("EsServicios") Or DtCabeza.Rows(0).Item("EsSecos") Then
            If HallaArticuloServiciosDeshabilitado(DtDetalle) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
        Else
            If HallaArticuloDeshabilitado(DtDetalle) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
        End If

        If DtCabeza.Rows(0).Item("ImporteDev") <> 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Factura Tiene Notas de Credito. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtCabezaRel As New DataTable

        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Factura = " & Relacionada & ";", ConexionRelacionada, DtCabezaRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Recibo") <> 0 Then
            If Not EstaReciboAnulado(DtCabeza.Rows(0).Item("Recibo"), ConexionFactura) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Debe Anular Recibo antes de Anular Factura. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If
        If DtCabezaRel.Rows.Count <> 0 Then
            If DtCabezaRel.Rows(0).Item("Recibo") <> 0 Then
                If Not EstaReciboAnulado(DtCabezaRel.Rows(0).Item("Recibo"), ConexionRelacionada) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Debe Anular Recibo antes de Anular Factura Relacionada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            If Not PAbierto Then
                If DtCabezaRel.Rows(0).Item("EsElectronica") Then
                    MsgBox("Factura Electrónica Solo se Anula con Nota de Crédito. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
        End If

        If DtCabeza.Rows(0).Item("Importe") + DtCabeza.Rows(0).Item("Percepciones") <> DtCabeza.Rows(0).Item("Saldo") Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Factura Tiene Imputaciones. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If DtCabezaRel.Rows.Count <> 0 Then
            If DtCabezaRel.Rows(0).Item("Importe") + DtCabezaRel.Rows(0).Item("Percepciones") <> DtCabezaRel.Rows(0).Item("Saldo") Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Factura Tiene Imputaciones. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        '''''      Dim DtRemitoLotes As New DataTable

        '''''  If AsignadoEnRemito Then
        '''''     If Not HallaAsignacionLotesRemito(DtRemitoLotes, ConexionRemito) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        ''''   End If

        ''''    If ComboEstado.SelectedValue = 1 And DtRemitoLotes.Rows.Count = 0 Then
        If ComboEstado.SelectedValue = 1 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Debe Previamente Reingresar Lotes al Stock. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtCabezaAux As DataTable = DtCabeza.Copy

        DtCabezaAux.Rows(0).Item("Estado") = 3
        DtCabezaAux.Rows(0).Item("Saldo") = DtCabezaAux.Rows(0).Item("Importe") + DtCabezaAux.Rows(0).Item("Percepciones")
        DtCabezaAux.Rows(0).Item("Remito") = 0
        DtCabezaAux.Rows(0).Item("Recibo") = 0

        If Relacionada <> 0 Then
            DtCabezaRel.Rows(0).Item("Estado") = 3
            DtCabezaRel.Rows(0).Item("Saldo") = DtCabezaRel.Rows(0).Item("Importe") + DtCabezaRel.Rows(0).Item("Percepciones")
            DtCabezaRel.Rows(0).Item("Remito") = 0
            DtCabezaRel.Rows(0).Item("Recibo") = 0
        End If

        Dim DtCabezaRemitoAux As DataTable = DtCabezaRemito.Copy

        For Each Row As DataRow In DtCabezaRemitoAux.Rows
            Row("Factura") = 0
        Next
        '''  For Each Row As DataRow In DtRemitoLotes.Rows
        ''''    Row("Facturado") = False
        '''    Next

        Dim DtAsignacionLotesRel As New DataTable
        Dim DtAsignacionLotesAux As DataTable = DtAsignacionLotes.Copy

        If DtCabezaRel.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante = 2 and Comprobante = " & Relacionada & ";", ConexionRelacionada, DtAsignacionLotesRel) Then Me.Close() : Exit Sub
        End If
        For Each Row As DataRow In DtAsignacionLotesAux.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtAsignacionLotesRel.Rows
            Row.Delete()
        Next

        DtDetallePedido = New DataTable
        If Not ActualizaPedido("B", DtDetallePedido) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoCabezaRel As New DataTable
        Dim DtAsientoCabezaCosto As New DataTable
        Dim DtAsientoCabezaCostoRel As New DataTable
        Dim DtAsientoCabezaRemito As New DataTable
        ' 
        If Not HallaAsientosCabeza(TipoAsiento, DtCabeza.Rows(0).Item("Factura"), DtAsientoCabeza, ConexionFactura) Then Me.Close() : Exit Sub
        If DtCabezaRel.Rows.Count <> 0 Then
            If Not HallaAsientosCabeza(TipoAsiento, DtCabezaRel.Rows(0).Item("Factura"), DtAsientoCabezaRel, ConexionRelacionada) Then Me.Close() : Exit Sub
        End If
        If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3
        If DtAsientoCabezaRel.Rows.Count <> 0 Then DtAsientoCabezaRel.Rows(0).Item("Estado") = 3
        '
        If Not HallaAsientosCabeza(TipoAsientoCosto, DtCabeza.Rows(0).Item("Factura"), DtAsientoCabezaCosto, ConexionFactura) Then Me.Close() : Exit Sub
        If DtCabezaRel.Rows.Count <> 0 Then
            If Not HallaAsientosCabeza(TipoAsientoCosto, DtCabezaRel.Rows(0).Item("Factura"), DtAsientoCabezaCostoRel, ConexionRelacionada) Then Me.Close() : Exit Sub
        End If
        If DtAsientoCabezaCosto.Rows.Count <> 0 Then DtAsientoCabezaCosto.Rows(0).Item("Estado") = 3
        If DtAsientoCabezaCostoRel.Rows.Count <> 0 Then DtAsientoCabezaCostoRel.Rows(0).Item("Estado") = 3
        '
        If Remito <> 0 Then
            For Each Row As DataRow In DtCabezaRemito.Rows
                If Not HallaAsientosCabezaUltimo(6060, Row("Remito"), DtAsientoCabezaRemito, ConexionRemito) Then Me.Close() : Exit Sub
            Next
            For Each Row As DataRow In DtAsientoCabezaRemito.Rows
                Row.Item("Estado") = 1
            Next

        End If

        ''   If GGeneraAsiento Then
        '''''  If Not HallaAsientosCabeza(TipoAsiento, DtCabeza.Rows(0).Item("Factura"), DtAsientoCabeza, ConexionFactura) Then Me.Close() : Exit Sub
        '''''       If DtCabezaRel.Rows.Count <> 0 Then
        '''''If Not HallaAsientosCabeza(TipoAsiento, DtCabezaRel.Rows(0).Item("Factura"), DtAsientoCabezaRel, ConexionRelacionada) Then Me.Close() : Exit Sub
        '''''    End If
        '''''If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3
        '''''      If DtAsientoCabezaRel.Rows.Count <> 0 Then DtAsientoCabezaRel.Rows(0).Item("Estado") = 3
        '
        ''''  If DtCabeza.Rows(0).Item("Remito") = 0 Or (DtCabeza.Rows(0).Item("Remito") <> 0 And DtRemitoLotes.Rows.Count = 0) Then
        ''''   If Not HallaAsientosCabeza(TipoAsientoCosto, DtCabeza.Rows(0).Item("Factura"), DtAsientoCabezaCosto, ConexionFactura) Then Me.Close() : Exit Sub
        ''''    If DtCabezaRel.Rows.Count <> 0 Then
        ''''    If Not HallaAsientosCabeza(TipoAsientoCosto, DtCabezaRel.Rows(0).Item("Factura"), DtAsientoCabezaCostoRel, ConexionRelacionada) Then Me.Close() : Exit Sub
        '''' End If
        '''  If DtAsientoCabezaCosto.Rows.Count <> 0 Then DtAsientoCabezaCosto.Rows(0).Item("Estado") = 3
        '''  If DtAsientoCabezaCostoRel.Rows.Count <> 0 Then DtAsientoCabezaCostoRel.Rows(0).Item("Estado") = 3
        ''  End If
        ''      If Remito <> 0 And DtRemitoLotes.Rows.Count = 0 Then
        '''For Each Row As DataRow In DtCabezaRemito.Rows
        '''If Not HallaAsientosCabezaUltimo(6060, Row("Remito"), DtAsientoCabezaRemito, ConexionRemito) Then Me.Close() : Exit Sub
        '''     Next
        ''     For Each Row As DataRow In DtAsientoCabezaRemito.Rows
        ''Row.Item("Estado") = 1
        '''       Next
        ''      End If
        ''      End If

        If MsgBox("Factura se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    If GrabaTabla(DtCabezaAux.GetChanges, "FacturasCabeza", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsignacionLotesAux.GetChanges) Then
                    If GrabaTabla(DtAsignacionLotesAux.GetChanges, "AsignacionLotes", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtCabezaRel.GetChanges) Then
                    If GrabaTabla(DtCabezaRel.GetChanges, "FacturasCabeza", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsignacionLotesRel.GetChanges) Then
                    If GrabaTabla(DtAsignacionLotesRel.GetChanges, "AsignacionLotes", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtCabezaRemitoAux.GetChanges) Then
                    If GrabaTabla(DtCabezaRemitoAux.GetChanges, "RemitosCabeza", ConexionRemito) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    If GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaRel.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaRel.GetChanges, "AsientosCabeza", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaCosto.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaCosto.GetChanges, "AsientosCabeza", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaCostoRel.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaCostoRel.GetChanges, "AsientosCabeza", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaRemito.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaRemito.GetChanges, "AsientosCabeza", ConexionRemito) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                'Actualiza Pedidos.
                If Not IsNothing(DtDetallePedido.GetChanges) Then
                    If GrabaTabla(DtDetallePedido.GetChanges, "PedidosDetalle", Conexion) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If

                Scope.Complete()
                GModificacionOk = True
                MsgBox("Baja Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            End Using
        Catch ex As TransactionException
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        Finally
        End Try

        If Not MuestraDatos() Then Me.Close()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PFactura = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not PermisoTotal And DtCabeza.Rows(0).Item("Rel") Then
            MsgBox("Usuario No Autorizado(1000).")
            Exit Sub
        End If

        If (PAbierto And Not EsfacturaElectronica) Or Not PAbierto Then
            MsgBox("Nota Puede ser ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("EsExterior") Then
            MsgBox("Factura Exportación No Puede Borrarce. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsfacturaElectronica And DtCabeza.Rows(0).Item("Cae") <> 0 Then
            MsgBox("Nota Electrónica No se Puede BORRAR pues Tiene Autorización en la AFIP. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        'ver si hay posteriores.
        Dim FacturaW As Decimal = 0
        Dim ConexionW As String
        If PAbierto Then
            FacturaW = PFactura
            ConexionW = ConexionFactura
        Else
            If Relacionada <> 0 Then
                FacturaW = Relacionada
                ConexionW = ConexionRelacionada
            End If
        End If
        Dim PuntoVentaW As Integer = Strings.Mid(FacturaW, 2, 4)
        Dim TipoIvaW As Integer = Strings.Mid(FacturaW, 1, 1)
        Dim UltimoNumeroW As Decimal = Strings.Right(FacturaW, 8)
        If HallaUltimaNumeracionW(TipoIvaW, PuntoVentaW, ConexionW) <> FacturaW Then
            MsgBox("Existe Facturas Posteriores a Esta. Factura a Borrar Debe ser La Ultima Realizada. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Valida() Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("EsServicios") Or DtCabeza.Rows(0).Item("EsSecos") Then
            If HallaArticuloServiciosDeshabilitado(DtDetalle) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
        Else
            If HallaArticuloDeshabilitado(DtDetalle) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
        End If

        If DtCabeza.Rows(0).Item("ImporteDev") <> 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Factura Tiene Notas de Credito. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtCabezaRel As New DataTable

        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Factura = " & Relacionada & ";", ConexionRelacionada, DtCabezaRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Recibo") <> 0 Then
            If Not EstaReciboAnulado(DtCabeza.Rows(0).Item("Recibo"), ConexionFactura) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Debe Anular Recibo antes de Anular Factura. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If
        If DtCabezaRel.Rows.Count <> 0 Then
            If DtCabezaRel.Rows(0).Item("Recibo") <> 0 Then
                If Not EstaReciboAnulado(DtCabezaRel.Rows(0).Item("Recibo"), ConexionRelacionada) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Debe Anular Recibo antes de Anular Factura Relacionada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            If Not PAbierto Then
                If DtCabezaRel.Rows(0).Item("EsElectronica") And DtCabezaRel.Rows(0).Item("Cae") <> 0 Then
                    MsgBox("Factura Electrónica No se Puede BORRAR pues Tiene Autorizacon en la AFIP. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
        End If

        Dim Suma As Decimal = DtCabeza.Rows(0).Item("Importe") + DtCabeza.Rows(0).Item("Percepciones")
        If Suma <> DtCabeza.Rows(0).Item("Saldo") Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Factura Tiene Imputaciones. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If DtCabezaRel.Rows.Count <> 0 Then
            Suma = DtCabezaRel.Rows(0).Item("Importe") + DtCabezaRel.Rows(0).Item("Percepciones")
            If Suma <> DtCabezaRel.Rows(0).Item("Saldo") Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Factura Tiene Imputaciones. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        Dim DtRemitoLotes As New DataTable

        If AsignadoEnRemito Then
            If Not HallaAsignacionLotesRemito(DtRemitoLotes, ConexionRemito) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If ComboEstado.SelectedValue = 1 And DtRemitoLotes.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Debe Previamente Reingresar Lotes al Stock. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtCabezaAux As DataTable = DtCabeza.Copy

        Dim DtDetalleAux As DataTable = DtDetalle.Copy
        Dim DtDetalleRel As New DataTable
        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT * FROM FacturasDetalle WHERE Factura = " & Relacionada & ";", ConexionRelacionada, DtDetalleRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        Dim DtCabezaRemitoAux As DataTable = DtCabezaRemito.Copy

        For Each Row As DataRow In DtCabezaRemitoAux.Rows
            Row("Factura") = 0
        Next
        For Each Row As DataRow In DtRemitoLotes.Rows
            Row("Facturado") = False
        Next

        Dim DtAsignacionLotesRel As New DataTable
        Dim DtAsignacionLotesAux As DataTable = DtAsignacionLotes.Copy

        If DtCabezaRel.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante = 2 and Comprobante = " & Relacionada & ";", ConexionRelacionada, DtAsignacionLotesRel) Then Me.Close() : Exit Sub
        End If
        For Each Row As DataRow In DtAsignacionLotesAux.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtAsignacionLotesRel.Rows
            Row.Delete()
        Next

        DtDetallePedido = New DataTable
        If Not ActualizaPedido("B", DtDetallePedido) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoCabezaRel As New DataTable
        Dim DtAsientoCabezaCosto As New DataTable
        Dim DtAsientoCabezaCostoRel As New DataTable
        Dim DtAsientoCabezaRemito As New DataTable
        ' 
        If Remito <> 0 And DtRemitoLotes.Rows.Count = 0 Then
            For Each Row As DataRow In DtCabezaRemito.Rows
                If Not HallaAsientosCabezaUltimo(6060, Row("Remito"), DtAsientoCabezaRemito, ConexionRemito) Then Me.Close() : Exit Sub
            Next
            For Each Row As DataRow In DtAsientoCabezaRemito.Rows
                Row.Item("Estado") = 1
            Next
        End If

        Dim DtAsientoDetalle As New DataTable
        Dim DtAsientoDetalleRel As New DataTable
        Dim DtAsientoDetalleCosto As New DataTable
        Dim DtAsientoDetalleCostoRel As New DataTable

        If Not HallaAsientosCabeza(TipoAsiento, DtCabeza.Rows(0).Item("Factura"), DtAsientoCabeza, ConexionFactura) Then Me.Close() : Exit Sub
        If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabeza.Rows(0).Item("Asiento") & ";", ConexionFactura, DtAsientoDetalle) Then Me.Close() : Exit Sub
        If DtCabezaRel.Rows.Count <> 0 Then
            If Not HallaAsientosCabeza(TipoAsiento, DtCabezaRel.Rows(0).Item("Factura"), DtAsientoCabezaRel, ConexionRelacionada) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaRel.Rows(0).Item("Asiento") & ";", ConexionRelacionada, DtAsientoDetalleRel) Then Me.Close() : Exit Sub
        End If
        If Not HallaAsientosCabeza(TipoAsientoCosto, DtCabeza.Rows(0).Item("Factura"), DtAsientoCabezaCosto, ConexionFactura) Then Me.Close() : Exit Sub
        If DtAsientoCabezaCosto.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaCosto.Rows(0).Item("Asiento") & ";", ConexionFactura, DtAsientoDetalleCosto) Then Me.Close() : Exit Sub
            If DtCabezaRel.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(TipoAsientoCosto, DtCabezaRel.Rows(0).Item("Factura"), DtAsientoCabezaCostoRel, ConexionRelacionada) Then Me.Close() : Exit Sub
                If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaCostoRel.Rows(0).Item("Asiento") & ";", ConexionRelacionada, DtAsientoDetalleCostoRel) Then Me.Close() : Exit Sub
            End If
        End If

        If DtAsientoCabeza.Rows.Count <> 0 Then
            DtAsientoCabeza.Rows(0).Delete()
            For Each Row As DataRow In DtAsientoDetalle.Rows
                Row.Delete()
            Next
        End If
        If DtAsientoCabezaRel.Rows.Count <> 0 Then
            DtAsientoCabezaRel.Rows(0).Delete()
            For Each Row As DataRow In DtAsientoDetalleRel.Rows
                Row.Delete()
            Next
        End If

        If DtAsientoCabezaCosto.Rows.Count <> 0 Then
            DtAsientoCabezaCosto.Rows(0).Delete()
            For Each Row As DataRow In DtAsientoDetalleCosto.Rows
                Row.Delete()
            Next
        End If
        If DtAsientoCabezaCostoRel.Rows.Count <> 0 Then
            DtAsientoCabezaCostoRel.Rows(0).Delete()
            For Each Row As DataRow In DtAsientoDetalleCostoRel.Rows
                Row.Delete()
            Next
        End If

        'Corre numeracion de las facturas del punto de venta.
        Dim DtPuntosDeVenta As New DataTable
        If FacturaW <> 0 Then
            UltimoNumeroW = UltimoNumeroW - 1
            If Not Tablas.Read("SELECT * FROM PuntosDeVenta WHERE Clave = " & PuntoVentaW & ";", Conexion, DtPuntosDeVenta) Then Exit Sub
            Select Case TipoIvaW
                Case 1
                    DtPuntosDeVenta.Rows(0).Item("FacturasA") = UltimoNumeroW
                Case 2
                    DtPuntosDeVenta.Rows(0).Item("FacturasB") = UltimoNumeroW
                Case 3
                    DtPuntosDeVenta.Rows(0).Item("FacturasC") = UltimoNumeroW
                Case 4
                    DtPuntosDeVenta.Rows(0).Item("FacturasE") = UltimoNumeroW
                Case 5
                    DtPuntosDeVenta.Rows(0).Item("FacturasM") = UltimoNumeroW
            End Select
        End If

        DtCabezaAux.Rows(0).Delete()
        If Relacionada <> 0 Then
            DtCabezaRel.Rows(0).Delete()
        End If
        For Each Row As DataRow In DtDetalleAux.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtDetalleRel.Rows
            Row.Delete()
        Next

        'Borra percepciones realizadas.
        Dim DtFacturaPercepcionesAux As DataTable = DtFacturaPercepciones.Copy
        For Each Row As DataRow In DtFacturaPercepcionesAux.Rows
            Row.Delete()
        Next
        '-------------------------

        If MsgBox("Factura se Borrara Definitivamente del Sistema. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    If GrabaTabla(DtCabezaAux.GetChanges, "FacturasCabeza", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtDetalleAux.GetChanges) Then
                    If GrabaTabla(DtDetalleAux.GetChanges, "FacturasDetalle", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsignacionLotesAux.GetChanges) Then
                    If GrabaTabla(DtAsignacionLotesAux.GetChanges, "AsignacionLotes", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtCabezaRel.GetChanges) Then
                    If GrabaTabla(DtCabezaRel.GetChanges, "FacturasCabeza", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtDetalleRel.GetChanges) Then
                    If GrabaTabla(DtDetalleRel.GetChanges, "FacturasDetalle", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                'Actualiza Percepciones.
                If Not IsNothing(DtFacturaPercepcionesAux.GetChanges) Then
                    If GrabaTabla(DtFacturaPercepcionesAux.GetChanges, "RecibosPercepciones", Conexion) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsignacionLotesRel.GetChanges) Then
                    If GrabaTabla(DtAsignacionLotesRel.GetChanges, "AsignacionLotes", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtCabezaRemitoAux.GetChanges) Then
                    If GrabaTabla(DtCabezaRemitoAux.GetChanges, "RemitosCabeza", ConexionRemito) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtRemitoLotes.GetChanges) Then
                    If GrabaTabla(DtRemitoLotes.GetChanges, "AsignacionLotes", ConexionRemito) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    If GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaRel.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaRel.GetChanges, "AsientosCabeza", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaCosto.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaCosto.GetChanges, "AsientosCabeza", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaCostoRel.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaCostoRel.GetChanges, "AsientosCabeza", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaRemito.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaRemito.GetChanges, "AsientosCabeza", ConexionRemito) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                'Actualiza Pedidos.
                If Not IsNothing(DtDetallePedido.GetChanges) Then
                    If GrabaTabla(DtDetallePedido.GetChanges, "PedidosDetalle", Conexion) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                'Corre numeracion en Punto de venta.
                If Not IsNothing(DtPuntosDeVenta.GetChanges) Then
                    If GrabaTabla(DtPuntosDeVenta.GetChanges, "PuntosDeVenta", Conexion) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If

                Scope.Complete()
                GModificacionOk = True
                MsgBox("Borrado Realizado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                FacturaBorrada = True
                Me.Close() : Exit Sub
            End Using
        Catch ex As TransactionException
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        Finally
        End Try

        If Not MuestraDatos() Then Me.Close()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonRecibo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRecibo.Click

        If GCaja = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PFactura <> 0 And DtCabeza.Rows(0).Item("Recibo") <> 0 Then
            ''''            UnRecibo.PAbierto = PAbierto
            ''''        UnRecibo.PNota = DtCabeza.Rows(0).Item("Recibo")
            '''''    UnRecibo.PTipoNota = 60
            ''''    UnRecibo.ShowDialog()
            ''''    Exit Sub
            UnReciboPagoFactura.PEmisor = PCliente
            UnReciboPagoFactura.PNota = DtCabeza.Rows(0).Item("Recibo")
            UnReciboPagoFactura.PFactura = PFactura
            UnReciboPagoFactura.PAbierto = PAbierto
            UnReciboPagoFactura.PEsSecos = EsSecos
            UnReciboPagoFactura.ShowDialog()
            If UnReciboPagoFactura.PActualizacionOk Then
                If Not MuestraDatos() Then Me.Close() : Exit Sub
            End If
            UnReciboPagoFactura.Dispose()
            Exit Sub
        End If

        If PBloqueaFunciones Or EsContable Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboTipoIva.SelectedValue = Exterior Then
            MsgBox("Recibo para Exportación debe Realizarce Por Tesoreria. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PFactura = 0 Then
            MsgBox("Debe Grabar Factura. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Factura Esta Anulada. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Recibo") = 0 Then
            If DtCabeza.Rows(0).Item("Importe") <> DtCabeza.Rows(0).Item("Saldo") Then
                MsgBox("Factura Ya Tiene Pago por Tesoreria. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        OpcionReciboFactura.ShowDialog()
        If OpcionReciboFactura.PRegresar Then OpcionReciboFactura.Dispose() : Exit Sub

        If DtCabeza.Rows(0).Item("Recibo") = 0 Then
            UnReciboPagoFactura.PEsPagoEfectivo = OpcionReciboFactura.PContado
        End If
        OpcionReciboFactura.Dispose()

        UnReciboPagoFactura.PEmisor = PCliente
        UnReciboPagoFactura.PNota = DtCabeza.Rows(0).Item("Recibo")
        UnReciboPagoFactura.PFactura = PFactura
        UnReciboPagoFactura.PAbierto = PAbierto
        UnReciboPagoFactura.PEsSecos = EsSecos
        UnReciboPagoFactura.ShowDialog()
        If UnReciboPagoFactura.PActualizacionOk Then
            If Not MuestraDatos() Then Me.Close() : Exit Sub
        End If
        UnReciboPagoFactura.Dispose()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboTipoIva.SelectedValue = Exterior Then
            MsgBox("Función NO Valida para Facturas Exportación", MsgBoxStyle.Information)
            Exit Sub
        End If

        If PFactura <> 0 And ComboTipoIva.SelectedValue = 3 And Not EsZ And ComboEstado.SelectedValue <> 3 Then
            If Not TieneRecibos() Then
                If MsgBox("Debe Generar Recibo de Pago. Quiere SALIR de la Factura Igualmente? (S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
        End If

        PFactura = 0
        IgualClienteW = 0
        IgualDepositoW = 0
        ButtonImportacionExcel.Enabled = True
        UnaFactura_Load(Nothing, Nothing)
        TextDirecto.Focus()

    End Sub
    Private Sub ButtonNuevoIgualCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevoIgualCliente.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboTipoIva.SelectedValue = Exterior Then
            MsgBox("Función NO Valida para Facturas Exportación", MsgBoxStyle.Information)
            Exit Sub
        End If

        If PFactura <> 0 And ComboTipoIva.SelectedValue = 3 And Not EsZ And ComboEstado.SelectedValue <> 3 Then
            If Not TieneRecibos() Then
                If MsgBox("Debe Generar Recibo de Pago. Quiere SALIR de la Factura Igualmente? (S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
        End If

        PFactura = 0
        IgualClienteW = PCliente
        IgualDepositoW = Deposito
        ButtonImportacionExcel.Enabled = True
        UnaFactura_Load(Nothing, Nothing)
        TextDirecto.Focus()

    End Sub
    Private Sub ButtonExportaRemitos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportaRemitos.Click

        If PFactura = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Factura ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeRemitosFactura(DtDetalle, DtCabeza.Rows(0).Item("Cliente"), MaskedFactura.Text, PAbierto)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonReAsignaLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReAsignaLotes.Click

        If PBloqueaFunciones Or EsContable Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not PermisoTotal And DtCabeza.Rows(0).Item("Rel") Then
            MsgBox("Usuario No Autorizado(1000).")
            Exit Sub
        End If

        If PFactura = 0 Then
            MsgBox("Factura debe ser Grabada Previamente. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Estado") = 3 Then
            MsgBox("Factura Esta Anulada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If HallaCierreFactura(PFactura) <> 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Factura Tiene Cierre de Factura. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        GModificacionOk = False

        UnaReAsignacionFactura.PFactura = PFactura
        UnaReAsignacionFactura.PAbierto = PAbierto
        UnaReAsignacionFactura.PtipoAsiento = TipoAsiento
        UnaReAsignacionFactura.PtipoAsientoCosto = TipoAsientoCosto
        UnaReAsignacionFactura.ShowDialog()
        If GModificacionOk Then MuestraDatos()
        UnaReAsignacionFactura.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PFactura = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = TipoAsiento
        If PAbierto Then
            ListaAsientos.PDocumentoB = PFactura
        Else
            ListaAsientos.PDocumentoN = PFactura
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonNetoPorLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNetoPorLotes.Click

        SeleccionarVarios.PAbierto = PAbierto
        SeleccionarVarios.PFactura = PFactura
        SeleccionarVarios.PEsNetoPorLotesFacturaVenta = True
        SeleccionarVarios.Show()

    End Sub
    Private Sub ButtonDatosCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDatosCliente.Click

        UnDatosEmisor.PEsCliente = True
        UnDatosEmisor.PEmisor = PCliente
        UnDatosEmisor.ShowDialog()

    End Sub
    Private Sub ButtonTextoRecibo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTextoRecibo.Click

        UnTextoParaRecibo.TextoFijoParaFacturas1 = TextoFijoParaFacturas1
        UnTextoParaRecibo.TextoFijoParaFacturas2 = TextoFijoParaFacturas2
        UnTextoParaRecibo.ShowDialog()

    End Sub
    Private Sub ButtonPedirAutorizacionAfip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPedirAutorizacionAfip.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PFactura = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not PAbierto Then
            MsgBox("No Permitido en esta Factura", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If Not DtCabeza.Rows(0).Item("EsElectronica") Then
            MsgBox("Comprobante No Es Electrónico.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("CAE") <> 0 Then
            MsgBox("Autorización Ya Existe.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Dim Cae As String
        Dim FechaCae As String
        Dim Resultado As String
        Dim ImpTotal As String
        Dim CbteTipoAsociado As Integer
        Dim CbteAsociado As Decimal
        Dim ConceptoW As String

        Dim mensaje As String = HallaCaeComprobante("F", 0, TipoFactura, DtCabeza.Rows(0).Item("Factura"), Cae, FechaCae, Resultado, ImpTotal, CbteTipoAsociado, CbteAsociado, ConceptoW, DtCabeza.Rows(0).Item("EsFCE"))
        If mensaje = "" And Cae <> "" Then
            If Not GrabaCAE(DtCabeza.Rows(0).Item("Factura"), CDec(Cae), CInt(FechaCae)) Then
                MsgBox("CAE de la AFIP No se pudo Grabar." + vbCrLf + "Intentelo Nuevamente.")
            Else
                If Not MuestraDatos() Then Me.Close() : Exit Sub
            End If
            Exit Sub
        End If

        If PideAutorizacionAfip(DtCabeza, DtDetalle, DtFacturaPercepciones) Then   'Pide y graba CAE a la AFIP.----
            If Not MuestraDatos() Then Me.Close() : Exit Sub
        End If

    End Sub
    Private Sub ButtonArticulosEnStock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonArticulosEnStock.Click

        If PEsContable Then
            MsgBox("No Debe ser Contable.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim RowsBusqueda() As DataRow
        Dim DtAFacturar As DataTable
        DtAFacturar = CreaDtAFacturar()
        For Each Fila As FilaAsignacion In ListaDeLotes
            RowsBusqueda = DtDetalle.Select("Indice = " & Fila.Indice)         'Fila.Importe2 tiene la senia.
            AgregarADtAFacturar(DtAFacturar, Fila.Operacion, Fila.Lote, Fila.Secuencia, Fila.Asignado, Fila.Importe2, RowsBusqueda(0).Item("Articulo"), RowsBusqueda(0).Item("Descuento"), RowsBusqueda(0).Item("KilosXUnidad"), RowsBusqueda(0).Item("TipoPrecio"), RowsBusqueda(0).Item("PrecioLista"))
        Next

        UnaPreFactura.PEsFactura = True
        UnaPreFactura.PCliente = PCliente
        UnaPreFactura.PFechaEntrega = FechaEntrega
        UnaPreFactura.PLista = Lista
        UnaPreFactura.PTieneCodigoCliente = TieneCodigoCliente
        UnaPreFactura.PListaDeLotes = ListaDeLotes
        UnaPreFactura.PDtAFacturar = DtAFacturar
        UnaPreFactura.PDeposito = ComboDeposito.SelectedValue
        UnaPreFactura.ShowDialog()
        If UnaPreFactura.PRegresar Then UnaPreFactura.Dispose() : DtAFacturar.Dispose() : Exit Sub

        DtDetalle.Clear()

        Dim Cantidad As Integer = 0

        For Each Row As DataRow In DtAFacturar.Rows
            Dim RowA As DataRow = DtDetalle.NewRow
            RowA("Indice") = Row("Indice")
            RowA("Articulo") = Row("Articulo")
            RowA("Cantidad") = Row("Cantidad")
            RowA("KilosXUnidad") = Row("KilosXUnidad")
            RowA("Descuento") = Row("Descuento")
            RowA("TipoPrecio") = Row("TipoPrecio")
            RowA("PrecioLista") = Row("Precio")
            DtDetalle.Rows.Add(RowA)
            Cantidad = Cantidad + 1
            If Cantidad = GLineasFacturas And Not EsfacturaElectronica Then
                If MsgBox("Supera Cantidad Articulos Permitidos(" & GLineasFacturas & "). Si Continua No Podra Imprimirse. Desea Continuar?", MsgBoxStyle.YesNo, MsgBoxStyle.Question) = MsgBoxResult.No Then
                    Exit For
                End If
            End If
        Next

        For I As Integer = ListaDeLotes.Count - 1 To 0 Step -1
            Dim Fila As New FilaAsignacion
            Fila = ListaDeLotes.Item(I)
            RowsBusqueda = DtDetalle.Select("Indice = " & Fila.Indice)
            If RowsBusqueda.Length = 0 Then
                ListaDeLotes.Remove(Fila)
            Else
                RowsBusqueda(0).Item("Senia") = Fila.Importe2
            End If
        Next

        UnaPreFactura.Dispose()
        DtAFacturar.Dispose()

        CalculaSubTotal()

    End Sub
    Private Sub LabelCAE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelCAE.Click

        If Not PAbierto Then Exit Sub

        Dim Cae As String
        Dim ImpTotal As String

        ControlaComprobante("F", 0, TipoFactura, DtCabeza.Rows(0).Item("Factura"), Cae, ImpTotal)

        MsgBox("Cae:  " & Cae & "   Importe Total:  " & ImpTotal)

    End Sub
    Private Sub PictureLupa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupa.Click

        Dim Lista As New List(Of ItemIvaReten)
        If PFactura <> 0 Then
            If DtFacturaPercepciones.Rows.Count = 0 Then Exit Sub
            For Each Row As DataRow In DtFacturaPercepciones.Rows
                Dim Fila As New ItemIvaReten
                Fila.Clave = Row("Percepcion")
                Fila.Importe = Row("Importe")
                Lista.Add(Fila)
            Next
        End If

        If PFactura = 0 Then
            SeleccionarVarios.PListaDePercepciones = ListaDePercepciones
        Else
            SeleccionarVarios.PListaDePercepciones = Lista
        End If
        SeleccionarVarios.PTipoNota = 2
        SeleccionarVarios.PEsPercepciones = True
        SeleccionarVarios.ShowDialog()
        SeleccionarVarios.Dispose()

    End Sub
    Private Sub ComboSucursal_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboSucursal.Validating

        If IsNothing(ComboSucursal.SelectedValue) Then ComboSucursal.SelectedValue = 0

        If ComboSucursal.SelectedValue <> 0 Then
            TextDestino.Text = HallaDireccionSucursalCliente(PCliente, ComboSucursal.SelectedValue)
            If TextDestino.Text = "-1" Then
                Me.Close() : Exit Sub
            End If
        Else
            TextDestino.Text = ""
        End If

    End Sub
    Private Sub ButtonImprimirSinRecibos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimirSinRecibos.Click

        SinRemitos = True
        ButtonImprimir_Click(Nothing, Nothing)
        SinRemitos = False

    End Sub
    Private Sub ButtonRefrescar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRefrescar.Click

        If Lista = 0 Then
            MsgBox("No tiene Lista de Precios.") : Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        LlenaCombosGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        'http://vb-helper.com/howto_net_print_and_preview.html

        If PFactura = 0 Then
            MsgBox("Opcion Invalida. Factura debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsfacturaElectronica And DtCabeza.Rows(0).Item("Cae") = 0 Then
            MsgBox("Falta CAE.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtDetalle.Rows.Count > GLineasFacturas And Not EsfacturaElectronica Then
            MsgBox("Factura tiene mas Articulos de lo Permitido para Impresión.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Impreso") And PAbierto Then
            If MsgBox("Factura Ya fue Impresa. Quiere Re-Imprimir?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        If TieneCodigoCliente Then
            For Each Row As DataGridViewRow In Grid.Rows
                If IsNothing(Row.Cells("Articulo").Value) Then Exit For
                Dim Codigo As String = HallaCodigoCliente(PCliente, Row.Cells("Articulo").Value)
                If IsNothing(Codigo) Then Codigo = "-1"
                If Codigo = "-1" Then
                    MsgBox("Articulo " & Row.Cells("Articulo").FormattedValue & " No Tiene Codigo Cliente.", MsgBoxStyle.Information)
                    Exit Sub
                End If
                Row.Cells("CodigoCliente").Value = Codigo
            Next
        End If

        ErrorImpresion = False
        Paginas = 0
        Copias = 1
        ContadorCopias = 0

        Dim PuntoVentaW As Integer = Val(Strings.Mid(DtCabeza.Rows(0).Item("Factura"), 2, 4))
        If PAbierto And (CopiasSegunPuntoVenta = 0 Or PuntoVentaW <> UltimoPuntoVentaParaCopiaSeleccionado) Then
            UltimoPuntoVentaParaCopiaSeleccionado = PuntoVentaW
            CopiasSegunPuntoVenta = TraeCopiasComprobante(2, PuntoVentaW)
            If CopiasSegunPuntoVenta < 0 Then CopiasSegunPuntoVenta = 0 : MsgBox("Error al Leer Tabla: PuntosDeVenta. Operacion se CANCELA.", MsgBoxStyle.Critical) : Exit Sub
        End If

        If PAbierto Then
            Copias = CopiasSegunPuntoVenta
        Else
            If GCuitEmpresa = GPradan Then
                Copias = 3
            Else
                Copias = 1
            End If
        End If
        'Acepción para Patagonia.
        If GCuitEmpresa = GPatagonia And PAbierto = False Then
            Copias = 3
        End If

        Dim print_document As New PrintDocument

        ImprimeFactura()

        If ErrorImpresion Then Exit Sub

        If Not GrabaImpreso() Then Exit Sub

        DtCabeza.Rows(0).Item("Impreso") = True
        If Not MuestraDatos() Then Me.Close() : Exit Sub

    End Sub
    Private Function ImprimeFactura() As Boolean

        Listado = ArmaListado(Grid, GLineasFacturas)
        'Carga UMedida.
        For Each item As ItemListado In Listado
            item.UMedida = BuscaEnGrid(item.Articulo)
        Next

        ContadorLineasListado = 0
        ContadorCopias = 1
        ContadorPaginas = 1

        TotalPaginas = Listado.Count / GLineasFacturas
        If Listado.Count / GLineasFacturas - TotalPaginas > 0 Then
            TotalPaginas = TotalPaginas + 1
        End If

        Dim print_document As New PrintDocument
        UnSeteoImpresora.SeteaImpresion(print_document)
        AddHandler print_document.PrintPage, AddressOf Print_Factura
        print_document.Print()

    End Function
    Private Sub Print_Factura(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        PrintUnaPagina(e, Listado, ContadorLineasListado)
        If ErrorImpresion Then e.HasMorePages = False : Exit Sub
        '
        If ContadorLineasListado < Listado.Count Then
            ContadorPaginas = ContadorPaginas + 1
            e.HasMorePages = True
        Else
            If ContadorCopias < Copias Then
                ContadorLineasListado = 0
                ContadorCopias = ContadorCopias + 1
                ContadorPaginas = 1
                e.HasMorePages = True
            Else : e.HasMorePages = False
            End If
        End If

    End Sub
    Private Sub PrintUnaPagina(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal lista As List(Of ItemListado), ByRef ContadorLineasListado As Integer)

        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim PrintFont As System.Drawing.Font
        Dim SaltoLinea As Integer = 4
        Dim FechaEntregaImp As String = ""
        Dim LineasPorPagina As Integer = GLineasFacturas
        Dim Contador As Integer = 0
        Dim Ancho As Integer = 178
        Dim Alto As Integer = 125

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        If EsfacturaElectronica Then
            Dim NombreEmpres As String = GNombreEmpresa '"Patagonia Sunrise S.R.L."
            Dim CuitEmpresa As String = GCuitEmpresa    '"30-12345678-5"
            Dim TipoComprobante As Integer = 1          '1. factura 2. debito 3. credito
            Dim Direccion1 As String = GDireccion1      '"Nave S-2 Puesto 34 - Mercado Central de Buenos Aires"
            Dim Direccion2 As String = GDireccion2      '"Autopista Ricchieri y Boulongne Sur Mer"
            Dim Direccion3 As String = GDireccion3      '"C.P.(1771) Villa Celina Pcia. de Buenos Aires"
            Dim LetraComprobante As String = LetraTipoIva(TipoFactura)
            Dim IngBruto As String = GIngBruto     '"Conv.Mult. Nª 901-898989-6"
            Dim FechaInicio As String = GFechaInicio
            '
            MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, LetraComprobante, GCondicionIvaEmpresa, IngBruto, FechaInicio, "C:\XML Afip\", e, EsFCE, 1)
            Texto = NumeroEditado(MaskedFactura.Text)
            PrintFont = New Font("Arial", 16)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, MTop + 6)
        End If

        Dim Str1 As String = ""
        If EsFCE Then
            Str1 = "miPyMEs (FCE)"
            Texto = Str1
            PrintFont = New Font("Arial", 12)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 119, MTop - 1)
        End If

        Str1 = ""
        Select Case ContadorCopias
            Case 1
                Str1 = "ORIGINAL"
            Case 2
                Str1 = "DUPLICADO"
            Case 3
                Str1 = "TRIPLICADO"
        End Select
        Texto = Str1
        PrintFont = New Font("Arial", 10)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 165, MTop)
        '
        PrintFont = New Font("Courier New", 12)
        Texto = "Pagina: " & ContadorPaginas & "/" & TotalPaginas
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + Ancho - 40, MTop - 17)
        '

        If ListRemitos.Items.Count = 0 Then
            FechaEntregaImp = " Fecha Entrega " & DateEntrega.Text
        End If

        GoTo hh   'Anulada por pedido de factracion. La guardamos para concervar la extructura.
        Dim EjeY As Integer = MTop
        Dim OrientacionString As New StringFormat
        OrientacionString.FormatFlags = StringFormatFlags.DirectionVertical

        PrintFont = New Font("Courier New", 7)
        For Iterador As Integer = 0 To 3
            Texto = "___________________"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq - 1, EjeY + 75, OrientacionString)
            Select Case Iterador
                Case 0
                    Texto = "Firma"
                Case 1
                    Texto = "Aclaración"
                Case 2
                    Texto = "DNI"
                Case 3
                    Texto = "Hora"
            End Select
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq - 4, EjeY + 75, OrientacionString)
            EjeY = EjeY + 30
        Next
hh:     '-------------------------------------------------------------------

        x = MIzq + 10 : y = MTop + 44
        PrintFont = New Font("Courier New", 12)

        Try
            If EsfacturaElectronica Or EsContable Then
                Texto = TextFechaContable.Text
            Else
                Texto = DateTime1.Text
            End If
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 128, y - 27)
            PrintFont = New Font("Courier New", 10)
            'Titulos.
            If PAbierto Then
                Texto = "CLIENTE    : " & TextCliente.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                If Numero <> 0 Then
                    Texto = "DOMICILIO  : " & Calle & " No: " & Numero
                Else
                    Texto = "DOMICILIO  : " & Calle
                End If
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "LOCALIDAD  : " & Localidad & " " & Provincia
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Dim ComentarioSucursal As String = HallaComentarioSucursalCliente(PCliente, Sucursal)
                Texto = "DOM.ENTREGA: " & ComboSucursal.Text & "-" & TextDestino.Text & "-" & ComentarioSucursal
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "CUIT       : " & Cuit & " " & ComboTipoIva.Text & "   " & FechaEntregaImp
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                'Condicion de venta.
                Texto = ""
                If DtCabeza.Rows(0).Item("FormaPago") = 1 Then
                    Texto = "CONDICION DE VENTA: Contado"
                End If
                If DtCabeza.Rows(0).Item("FormaPago") = 2 Then
                    Texto = "CONDICION DE VENTA: Cuenta Corriente"
                End If
                If DtCabeza.Rows(0).Item("FormaPago") = 3 Then
                    Texto = "CONDICION DE VENTA: Mixta"
                End If
                If DtCabeza.Rows(0).Item("FormaPago") = 4 Then
                    Texto = "CONDICION DE VENTA: Contado Efectivo"
                End If
                If DtCabeza.Rows(0).Item("FormaPago") = 5 Then
                    Texto = "CONDICION DE VENTA: Cuenta de Gestión."
                End If
                If Trim(TextFormaPago.Text) <> "" Then
                    Texto = "CONDICION DE VENTA: " & TextFormaPago.Text
                End If
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                'Imprime Remitos Facturados.
                Dim Detalle1 As String = ""
                Dim Detalle2 As String = ""
                For I As Integer = 0 To DtCabezaRemito.Rows.Count - 1
                    Select Case I
                        Case 0
                            Detalle1 = Detalle1 & Format(DtCabezaRemito.Rows(I).Item("Remito"), "0000-00000000") & " "
                        Case 1, 2, 3, 4, 5, 6
                            Detalle2 = Detalle2 & Format(DtCabezaRemito.Rows(I).Item("Remito"), "0000-00000000") & " "
                        Case 7
                            Detalle2 = Detalle2 & "..."
                        Case Else
                    End Select
                Next
                If SinRemitos Then
                    Detalle1 = "" : Detalle2 = ""
                End If
                Texto = "REMITOS: " & Detalle1
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 115, y)
                y = y + SaltoLinea
                Texto = Detalle2
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Else
                Texto = "CLIENTE    : " & TextCliente.Text & "  Nro.: " & NumeroEditado(DtCabeza.Rows(0).Item("Factura"))
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                If GCuitEmpresa = GPatagonia Then
                    y = y + SaltoLinea
                    Texto = "Sucursal: " & ComboSucursal.Text
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                End If
                If CheckImprimeDomicilio.Checked Then
                    y = y + SaltoLinea
                    If Numero <> 0 Then
                        Texto = "DOMICILIO  : " & Calle & " No: " & Numero
                    Else
                        Texto = "DOMICILIO  : " & Calle
                    End If
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                    y = y + SaltoLinea
                    Texto = "LOCALIDAD  : " & Localidad & " " & Provincia
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                End If
            End If

            'Grafica -Rectangulos-.
            x = MIzq + 10
            y = MTop + 74
            Dim LineaArticulo As Integer = x + 90
            Dim LineaCantidad As Integer = x + 115 'x + 110
            Dim LineaUnitario As Integer = x + 143
            Dim LineaImporte As Integer = x + Ancho
            Dim Longi As Integer
            Dim Xq As Integer
            Dim Yq As Integer = y - SaltoLinea

            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.3), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.3), LineaArticulo, y, LineaArticulo, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.3), LineaCantidad, y, LineaCantidad, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.3), LineaUnitario, y, LineaUnitario, y + Alto)
            'Titulos de articulo.
            Texto = "ARTICULO"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 1, y + 2)
            Texto = "CANTIDAD"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi - 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "PRECIO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaUnitario - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.3), x, y, x + Ancho, y)
            Dim Articulo As String
            Yq = y - SaltoLinea
            '
            PrintFont = New Font("Courier New", 9)
            While Contador < LineasPorPagina And ContadorLineasListado < lista.Count
                Yq = Yq + SaltoLinea
                'Descripcion de Articulos.
                Articulo = HallaNombreArticulo(lista.Item(ContadorLineasListado).Articulo)
                If TieneCodigoCliente Then
                    Articulo = RellenarBlancos(lista.Item(ContadorLineasListado).CodigoCliente, 8) & " " & Articulo
                End If
                Texto = Articulo
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                If EsSecos Or EsServicios Then
                    lista.Item(ContadorLineasListado).Medida = ""
                End If
                'Imprime cantidad.
                Dim Cantidad As Decimal = 0
                If lista.Item(ContadorLineasListado).TipoPrecio = 2 Then
                    Cantidad = lista.Item(ContadorLineasListado).Cantidad * lista.Item(ContadorLineasListado).KilosXUnidad
                    Texto = Cantidad.ToString & " " & lista.Item(ContadorLineasListado).UMedida
                Else
                    Cantidad = lista.Item(ContadorLineasListado).Cantidad
                    Texto = Cantidad.ToString & " " & lista.Item(ContadorLineasListado).Medida
                End If
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaCantidad - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Unitario.
                Dim Precio As Decimal = 0
                If TipoFactura = 3 Or TipoFactura = 2 Then
                    Precio = lista.Item(ContadorLineasListado).TotalItem / Cantidad
                    If lista.Item(ContadorLineasListado).TipoPrecio = 2 Then
                        Texto = FormatNumber(FormatoSinRedondeo3Decimales(Precio), 3) & "x" & lista.Item(ContadorLineasListado).UMedida
                    Else
                        Texto = FormatNumber(FormatoSinRedondeo3Decimales(Precio), 3) & "x" & lista.Item(ContadorLineasListado).Medida
                    End If
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaUnitario - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Importe.
                    Texto = FormatNumber(lista.Item(ContadorLineasListado).TotalItem, GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Else
                    Precio = lista.Item(ContadorLineasListado).PrecioLista
                    If lista.Item(ContadorLineasListado).TipoPrecio = 2 Then
                        Precio = Precio / lista.Item(ContadorLineasListado).KilosXUnidad
                        Texto = FormatNumber(FormatoSinRedondeo3Decimales(Precio), 3) & "x" & lista.Item(ContadorLineasListado).UMedida
                    Else
                        Texto = FormatNumber(Precio, 3) & "x" & lista.Item(ContadorLineasListado).Medida
                    End If
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaUnitario - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Importe.
                    Dim Neto As Decimal = CalculaNeto(Cantidad, Precio)
                    Texto = FormatNumber(Neto, GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
                Contador = Contador + 1
                ContadorLineasListado = ContadorLineasListado + 1
            End While
            PrintFont = New Font("Courier New", 12)
            'Imprime Cae
            If DtCabeza.Rows(0).Item("Cae") <> 0 Then
                PrintFont = New Font("Courier New", 12)
                Yq = 265
                e.Graphics.DrawString(LabelCAE.Text, PrintFont, Brushes.Black, x + 15, Yq)
                '----------------------------------------Codigo Barra anulado. Reemplazado por QR.------------------------------------------------------------------------------
                'Dim CodigoBarra As String = Format(CuitNumerico(GCuitEmpresa), "00000000000") & Format(HallaTipo("F", TipoFactura, 0), "00") & Format(CInt(Strings.Left(MaskedFactura.Text, 4)), "0000") & Format(DtCabeza.Rows(0).Item("Cae"), "00000000000000") & Format(DtCabeza.Rows(0).Item("FechaCae"), "00000000")
                'Dim aa As New DllVarias
                ' CodigoBarra = aa.CombierteTextoAInterleaved2Of5(CodigoBarra, True)
                ' Dim Tamanio As Integer = 18
                ' Dim fuente As Font
                ' fuente = CustomFont.GetInstance(Tamanio, FontStyle.Regular)
                ' e.Graphics.DrawString(CodigoBarra, fuente, Brushes.Black, x, Yq + 7)
                '--------------------QR------------------------------------------------------
                ImprimeQR(x - 14, Yq - 2, 22, 22, e)
                '------------------------------------------------------------------------------------------------------------------------------------
            End If
            '------------------------------------------------------------------------------------------------------------------------------------
            If DtCabeza.Rows(0).Item("Cae") <> 0 Or Not PAbierto Then    'pregunta por cae <> 0 pues en facturas pre-impresa no entra la cuadricula. 
                If GCuitEmpresa = GPatagonia Or GCuitEmpresa = GCuadroNorte Or GCuitEmpresa = GPremiumFruit Then
                    Yq = 264
                    ArmaRectangulos(x - 10, Yq, e)
                End If
            End If
            '
            If ContadorLineasListado = lista.Count Then
                FinalDePagina(e, MTop, SaltoLinea, LineaCantidad, LineaImporte, Xq, Yq, x, Ancho)
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
        End Try

    End Sub
    Private Sub FinalDePagina(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal MTop As Integer, ByVal SaltoLinea As Integer, ByVal LineaCantidad As Integer, ByVal LineaImporte As Integer, ByVal Xq As Integer, ByVal Yq As Integer, ByVal x As Integer, ByVal Ancho As Integer)

        Dim CBU As String
        Dim AliasW As String
        Dim Minimo As Decimal
        Dim StrFCE As String = ""
        If EsFCE Then
            HallaDatosFCE(CBU, AliasW, Minimo)
            StrFCE = "C.B.U. " & CBU & " Fecha Pago " & Format(DateFechaPago.Value.Day, "00") & "/" & Format(DateFechaPago.Value.Month, "00") & "/" & Strings.Right(DateFechaPago.Value.Year, 2)
        End If

        Dim Texto As String
        Dim PrintFont As System.Drawing.Font
        Dim Longi As Integer
        Dim Cantidad As Decimal = 0

        For Each Row As DataRow In DtDetalle.Rows
            Cantidad = Cantidad + Row("Cantidad")
        Next

        e.Graphics.PageUnit = GraphicsUnit.Millimeter
        PrintFont = New Font("Courier New", 10)

        Try
            'Resguardo
            Yq = MTop + 199
            Dim Pedido As String = ""
            If TextPedidoCliente.Text <> "" Then
                Pedido = "Pedido Cliente: " & TextPedidoCliente.Text
            End If
            If PAbierto Then
                Texto = GNombreEmpresa & " " & "FC " & NumeroEditado(MaskedFactura.Text) & " Bultos/U.Medida: " & Cantidad & " " & Pedido
            Else
                Texto = "FC " & NumeroEditado(MaskedFactura.Text) & " Bultos/U.Medida: " & Cantidad & " " & Pedido
            End If
            e.Graphics.DrawString(Texto, New Font("Courier New", 8), Brushes.Black, x, Yq)
            'Totales
            PrintFont = New Font("Courier New", 10)
            Yq = MTop + 204
            'Neto
            Dim NetoPrecioLista As Decimal
            Dim Descuento As Decimal
            Dim StrNetoPrecioLista As String
            Dim StrDescuento As String
            Select Case TipoFactura
                Case 2, 3
                    CalculaNetoSinDescuentoMasIva(NetoPrecioLista, Descuento)
                    StrNetoPrecioLista = FormatNumber(NetoPrecioLista, GDecimales)
                    StrDescuento = FormatNumber(Descuento, GDecimales)
                Case Else
                    StrNetoPrecioLista = TextNetolPrecioLista.Text
                    StrDescuento = TextDescuento.Text
            End Select
            Texto = "Neto"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
            Texto = StrNetoPrecioLista
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Yq = Yq + SaltoLinea - 1
            'Descuento
            Texto = "Descuento"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
            Texto = StrDescuento
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Yq = Yq + SaltoLinea - 1
            If TipoFactura = 3 Or TipoFactura = 2 Then
                'Neto
                Texto = "Sub-Total"
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
                Texto = TextSubTotal.Text
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Else
                'Neto
                Texto = "Neto"
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
                Texto = TextTotalNeto.Text
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                '
                Dim ListaIva As New List(Of ItemIva)
                ArmaListaImportesIva(ListaIva)
                'Iva.
                For Each Fila As ItemIva In ListaIva
                    Yq = Yq + SaltoLinea - 1
                    Texto = "IVA. " & FormatNumber(Fila.Iva, GDecimales)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
                    Texto = FormatNumber(Fila.Importe, GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Next
                ListaIva = Nothing
            End If
            'Seña
            Yq = Yq + SaltoLinea - 1
            Texto = "Seña"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
            Texto = TextSenia.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)

            'Percepcion.
            If DtFacturaPercepciones.Rows.Count <> 0 Then
                Yq = Yq + SaltoLinea - 1
                Texto = HallaNombreRetencion(DtFacturaPercepciones.Rows(0).Item("Percepcion"))  'por que hay una sola percepcion.
                e.Graphics.DrawString(Texto, New Font("Courier New", 8), Brushes.Black, LineaCantidad + 5, Yq + 0.5)
                Texto = TextPercepcion.Text
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            End If

            'Total
            Yq = Yq + SaltoLinea
            Texto = "Total"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
            Texto = TextTotalGeneral.Text 'TextTotalFactura.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)

            'Imprime Leyenda.
            PrintFont = New Font("Courier New", 8)
            Yq = MTop + 198 '200  '204
            Dim SaltoLineaW As Integer = SaltoLinea - 1
            Yq = Yq + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox1.Text, PrintFont, Brushes.Black, x, Yq)
            Yq = Yq + SaltoLineaW
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox2.Text, PrintFont, Brushes.Black, x, Yq)
            Yq = Yq + SaltoLineaW
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox3.Text, PrintFont, Brushes.Black, x, Yq)
            Yq = Yq + SaltoLineaW
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox4.Text, PrintFont, Brushes.Black, x, Yq)

            'Leyenda AFIP para Consumidor final y exentos (letra Factura B)
            If TipoFactura = 3 Or TipoFactura = 2 Then
                Yq = Yq + SaltoLinea
                e.Graphics.DrawString("Régimen de Transferencia Fiscal al Consumidor(ley 27.743)", PrintFont, Brushes.Black, x, Yq)
                Texto = "__________________________________________________________"
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, Yq)
                Yq = Yq + SaltoLinea
                Dim TotalIva As Decimal = CalculaTotalIva(Grid)
                e.Graphics.DrawString("Iva Contenido $ " & FormatNumber(TotalIva, GDecimales), PrintFont, Brushes.Black, x, Yq)
                Yq = Yq + SaltoLineaW
                e.Graphics.DrawString("Otros Impuestos Nacionales Indirectos: $", PrintFont, Brushes.Black, x, Yq)
            End If

            Yq = Yq + SaltoLinea

            'Imprime Permisos de Importacion.
            Dim PermisoImportacion(0) As String
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.PermisoImp <> "" Then
                    ReDim Preserve PermisoImportacion(UBound(PermisoImportacion) + 1)
                    PermisoImportacion(UBound(PermisoImportacion)) = Fila.PermisoImp
                End If
            Next
            Yq = MTop + 227
            PrintFont = New Font("Courier New", 7)
            e.Graphics.DrawString("Permiso Imp.  ", PrintFont, Brushes.Black, x, Yq)
            Dim TextoPermiso As String = ""
            For K As Integer = 1 To UBound(PermisoImportacion)
                TextoPermiso = TextoPermiso & " " & PermisoImportacion(K)
                If K = 3 Then
                    e.Graphics.DrawString(TextoPermiso, PrintFont, Brushes.Black, x + 19, Yq)
                    TextoPermiso = ""
                End If
                If K = 6 Or K = 9 Then
                    Yq = Yq + SaltoLinea
                    e.Graphics.DrawString(TextoPermiso, PrintFont, Brushes.Black, x + 19, Yq)
                    TextoPermiso = ""
                End If
                If K = 9 Then Exit For
            Next
            If TextoPermiso <> "" Then
                Yq = Yq + SaltoLinea
                e.Graphics.DrawString(TextoPermiso, PrintFont, Brushes.Black, x + 19, Yq)
            End If
            'Para FCE.
            Yq = Yq + 3 * SaltoLinea
            PrintFont = New Font("Courier New", 9, FontStyle.Bold)
            e.Graphics.DrawString(StrFCE, PrintFont, Brushes.Black, x + 80, Yq - 1)
            'Cartel para montributo ley 27.618
            If ComboTipoIva.SelectedValue = 6 And (TipoFactura = 1 Or TipoFactura = 5) And PAbierto Then
                PrintFont = New Font("Courier New", 6)
                Texto = "El crédito fiscal discriminado en el presente comprobante,sólo podra ser"
                Yq = Yq + 4 * SaltoLinea
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 12, Yq)
                Yq = Yq + SaltoLinea
                Texto = "computado a efectos del Régimen de Sostenimiento e Inclusión Fiscal para"
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 12, Yq)
                Yq = Yq + SaltoLinea
                Texto = "Pequeños Contribuyentes de la ley 27.618"
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 12, Yq)
            End If
            '-------------------------------------------------------------

        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
        End Try

    End Sub
    Private Sub ArmaRectangulos(ByVal x As Decimal, ByVal Y As Decimal, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim PrintFont As System.Drawing.Font
        Dim AnchoWW As Integer = 34
        Dim AltoWW As Integer = 10
        Dim xVacios As Integer = 0
        Dim Texto As String

        x = x + 10

        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.2), x + 113, Y + 6, AnchoWW * 2, AltoWW + 5)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.2), x + 113 + AnchoWW, Y + 6, x + 113 + AnchoWW, Y + 6 + AltoWW + 5)   'vertical
        e.Graphics.DrawLine(New Pen(Color.Black, 0.2), x + 113, Y + 10, x + 113 + AnchoWW * 2, Y + 10)  'horizontal
        e.Graphics.DrawLine(New Pen(Color.Black, 0.2), x + 113, Y + 15, x + 113 + AnchoWW * 2, Y + 15)    'horizontal

        PrintFont = New Font("Courier New", 8)
        Texto = "     Pallets            Cajones"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 113, Y + 5 + 1)
        Y += 5

        For I As Integer = 1 To 2
            e.Graphics.DrawLine(New Pen(Color.Black, 0.2), x + 130 + xVacios, Y + 5, x + 130 + xVacios, Y + 5 + AltoWW)   'vertical
            PrintFont = New Font("Courier New", 8)
            Texto = " Armado   Devueltos"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + xVacios + 113, Y + 5 + 1)
            xVacios = AnchoWW
            AnchoWW *= 2
        Next

    End Sub
    Private Sub ImprimeQR(ByVal X As Long, ByVal Y As Long, ByVal Ancho As Long, ByVal Alto As Long, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim ImageQR As Image = ArmaDatoParaQR(1, DtCabeza.Rows(0).Item("FechaContable"), DtCabeza.Rows(0).Item("Factura"), DtCabeza.Rows(0).Item("EsFCE"), DtCabeza.Rows(0).Item("Cae"), DtCabeza.Rows(0).Item("Importe") + DtCabeza.Rows(0).Item("Percepciones"), Cuit, DocTipo, DocNro, DtCabeza.Rows(0).Item("TipoIva"))
        e.Graphics.DrawImage(ImageQR, X, Y, Ancho, Alto)

    End Sub
    Private Sub ButtonImprimeRemitos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimeRemitos.Click

        If PFactura = 0 Then
            MsgBox("Opcion Invalida. Factura debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ErrorImpresion = False
        Paginas = 0
        Copias = 1
        ContadorCopias = 0

        Dim print_document As New PrintDocument
        UnSeteoImpresora.SeteaImpresion(print_document)
        ImprimeRemito()

    End Sub
    Private Function ImprimeRemito() As Boolean

        Dim Str As String
        Dim I As Integer = 0

        ListaRemitosParaImpresion = New List(Of String)

        For Each row As DataRow In DtCabezaRemito.Rows
            Select Case I
                Case 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100
                    If Str <> "" Then ListaRemitosParaImpresion.Add(Str)
                    Str = ""
            End Select
            Str = Str & Format(row("Remito"), "0000-00000000") & " "
            I = I + 1
        Next
        If Str <> "" Then ListaRemitosParaImpresion.Add(Str)

        ContadorLineasListado = 0
        ContadorCopias = 1
        ContadorPaginas = 1

        TotalPaginas = ListaRemitosParaImpresion.Count / GLineasFacturas
        If ListaRemitosParaImpresion.Count / GLineasFacturas - TotalPaginas > 0 Then
            TotalPaginas = TotalPaginas + 1
        End If

        Dim print_document As New PrintDocument
        UnSeteoImpresora.SeteaImpresion(print_document)
        AddHandler print_document.PrintPage, AddressOf Print_Remito
        print_document.Print()

        ListaRemitosParaImpresion.Clear()

    End Function
    Private Sub Print_Remito(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)


        PrintUnaPaginaRemito(e, ListaRemitosParaImpresion, ContadorLineasListado)
        If ErrorImpresion Then e.HasMorePages = False : Exit Sub
        '
        If ContadorLineasListado < ListaRemitosParaImpresion.Count Then
            ContadorPaginas = ContadorPaginas + 1
            e.HasMorePages = True
        Else
            If ContadorCopias < Copias Then
                ContadorLineasListado = 0
                ContadorCopias = ContadorCopias + 1
                ContadorPaginas = 1
                e.HasMorePages = True
            Else : e.HasMorePages = False
            End If
        End If

    End Sub
    Private Sub PrintUnaPaginaRemito(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal lista As List(Of String), ByRef ContadorLineasListado As Integer)

        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim PrintFont As System.Drawing.Font
        Dim SaltoLinea As Integer = 4
        Dim FechaEntregaImp As String = ""
        Dim LineasPorPagina As Integer = GLineasFacturas
        Dim Contador As Integer = 0
        Dim Ancho As Integer = 178
        Dim Alto As Integer = 125

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        Dim NombreEmpres As String = GNombreEmpresa '"Patagonia Sunrise S.R.L."
        Dim CuitEmpresa As String = GCuitEmpresa    '"30-12345678-5"
        Dim TipoComprobante As Integer = 1              '1. factura 2. debito 3. credito
        Dim Direccion1 As String = GDireccion1      '"Nave S-2 Puesto 34 - Mercado Central de Buenos Aires"
        Dim Direccion2 As String = GDireccion2      '"Autopista Ricchieri y Boulongne Sur Mer"
        Dim Direccion3 As String = GDireccion3      '"C.P.(1771) Villa Celina Pcia. de Buenos Aires"
        Dim LetraComprobante As String = LetraTipoIva(TipoFactura)
        Dim IngBruto As String = GIngBruto     '"Conv.Mult. Nª 901-898989-6"
        Dim FechaInicio As String = GFechaInicio
        '
        MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, LetraComprobante, GCondicionIvaEmpresa, IngBruto, FechaInicio, "C:\XML Afip\", e, EsFCE, 1)
        Texto = NumeroEditado(MaskedFactura.Text)
        PrintFont = New Font("Arial", 16)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, MTop + 6)           '19

        Dim Str1 As String = ""
        Select Case ContadorCopias
            Case 1
                Str1 = "ORIGINAL"
            Case 2
                Str1 = "DUPLICADO"
            Case 3
                Str1 = "TRIPLICADO"
        End Select
        Texto = Str1
        PrintFont = New Font("Arial", 10)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 165, MTop)
        '
        PrintFont = New Font("Courier New", 12)
        Texto = "Pagina: " & ContadorPaginas & "/" & TotalPaginas
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + Ancho - 40, MTop - 17)
        '

        If ListRemitos.Items.Count = 0 Then
            FechaEntregaImp = " Fecha Entrega " & DateEntrega.Text
        End If

        x = MIzq + 10 : y = MTop + 44
        PrintFont = New Font("Courier New", 12)

        Try
            If EsfacturaElectronica Then
                Texto = TextFechaContable.Text
            Else
                Texto = DateTime1.Text
            End If
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 128, y - 27)
            PrintFont = New Font("Courier New", 10)
            'Titulos.
            If PAbierto Then
                Texto = "CLIENTE    : " & TextCliente.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "DOMICILIO  : " & Calle
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "LOCALIDAD  : " & Localidad & " " & Provincia
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "DOM.ENTREGA: " & ComboSucursal.Text & "-" & TextDestino.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "CUIT       : " & Cuit & " " & ComboTipoIva.Text & "   " & FechaEntregaImp
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                'Condicion de venta.
                Texto = ""
                If DtCabeza.Rows(0).Item("FormaPago") = 1 Then
                    Texto = "CONDICION DE VENTA: Contado"
                End If
                If DtCabeza.Rows(0).Item("FormaPago") = 2 Then
                    Texto = "CONDICION DE VENTA: Cuenta Corriente"
                End If
                If DtCabeza.Rows(0).Item("FormaPago") = 3 Then
                    Texto = "CONDICION DE VENTA: Mixta"
                End If
                If DtCabeza.Rows(0).Item("FormaPago") = 4 Then
                    Texto = "CONDICION DE VENTA: Contado Efectivo"
                End If
                If DtCabeza.Rows(0).Item("FormaPago") = 5 Then
                    Texto = "CONDICION DE VENTA: Cuenta de Gestión."
                End If
                If Trim(TextFormaPago.Text) <> "" Then
                    Texto = "CONDICION DE VENTA: " & TextFormaPago.Text
                End If
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Else
                Texto = "CLIENTE    : " & TextCliente.Text & "  Nro.: " & NumeroEditado(DtCabeza.Rows(0).Item("Factura"))
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            End If

            'Grafica -Rectangulos-.
            x = MIzq + 10
            y = MTop + 74
            Dim LineaArticulo As Integer = x + 90
            Dim LineaCantidad As Integer = x + 110
            Dim LineaUnitario As Integer = x + 143
            Dim LineaImporte As Integer = x + Ancho
            Dim Longi As Integer
            Dim Xq As Integer
            Dim Yq As Integer = y - SaltoLinea

            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.3), x, y, Ancho, Alto)
            'Lineas vertical.
            'Titulos de articulo.
            Texto = "DETALLE REMITOS FACTURADOS"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 1, y + 2)
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.3), x, y, x + Ancho, y)
            Dim Articulo As String
            Yq = y - SaltoLinea
            '
            While Contador < LineasPorPagina And ContadorLineasListado < lista.Count
                Yq = Yq + SaltoLinea
                'Linea de Remitos.
                Texto = lista.Item(ContadorLineasListado)
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Contador = Contador + 1
                ContadorLineasListado = ContadorLineasListado + 1
            End While
            FinalDePagina(e, 20, 4, 115, MIzq + 10 + 178, Xq, Yq, x, Ancho)
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
        End Try

    End Sub
    Private Function HallaNombreArticulo(ByVal Articulo As Integer) As String

        If DtCabeza.Rows(0).Item("EsServicios") Or DtCabeza.Rows(0).Item("EsSecos") Then
            Return NombreArticuloServicios(Articulo)
        Else
            Return NombreArticulo(Articulo)
        End If

    End Function
    Private Function ActualizaPedido(ByVal Funcion As String, ByVal DtPedido As DataTable) As Boolean

        Dim PedidoW As Integer = 0
        Dim RowsBusqueda() As DataRow

        If Funcion = "A" Then
            PedidoW = Pedido
        End If
        If Funcion = "B" Then
            PedidoW = DtCabeza.Rows(0).Item("Pedido")
        End If

        If PedidoW = 0 Then Return True

        If Not Tablas.Read("SELECT * FROM PedidosDetalle WHERE Pedido = " & PedidoW & ";", Conexion, DtPedido) Then Return False

        For Each Row As DataRow In DtDetalle.Rows
            RowsBusqueda = DtPedido.Select("Articulo = " & Row("Articulo"))
            If RowsBusqueda.Length <> 0 Then
                If Funcion = "A" Then
                    RowsBusqueda(0).Item("Entregada") = CDec(RowsBusqueda(0).Item("Entregada")) + CDec(Row("Cantidad"))
                End If
                If Funcion = "B" Then
                    RowsBusqueda(0).Item("Entregada") = CDec(RowsBusqueda(0).Item("Entregada")) - CDec(Row("Cantidad"))
                End If
            End If
        Next

        Return True

    End Function
    Public Function GrabaImpreso() As Boolean

        Dim Sql As String = "UPDATE FacturasCabeza Set Impreso = 1 WHERE Factura = " & DtCabeza.Rows(0).Item("Factura") & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionFactura)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                If CInt(Cmd.ExecuteNonQuery()) = 0 Then Return False
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos al Grabar Factura. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End Try

        Return True

    End Function
    Private Sub PictureAlmanaque_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaque.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaAfip.Text = ""
        Else : TextFechaAfip.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueContable.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then
            MsgBox("No hay un registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        IndiceCoincidencia = Grid.CurrentRow.Cells("Indice").Value
        ListaDeLotes.RemoveAll(AddressOf Coincidencia)
        Grid.Rows.Remove(Grid.CurrentRow)
        CalculaSubTotal()

    End Sub
    Private Sub TextDirecto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextDirecto.KeyPress

        If Asc(e.KeyChar) = 13 Then TextDirecto_Validating(Nothing, Nothing) : Exit Sub

        EsPorcentaje(e.KeyChar, TextDirecto.Text)

    End Sub
    Private Sub TextDirecto_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextDirecto.Validating

        If PEsContable Then
            Exit Sub
        End If

        If Not IsNumeric(TextDirecto.Text) Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextDirecto.Focus()
            Exit Sub
        Else : TextDirecto.Text = FormatNumber(TextDirecto.Text, GDecimales, True, True, True)
            If CDbl(TextDirecto.Text) > 100 Then
                MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                TextDirecto.Focus()
                Exit Sub
            End If
            If Not ClienteOpr Then
                If CDbl(TextDirecto.Text) <> 0 Then
                    If EsfacturaElectronica Then
                        MsgBox("Cliente Solo Opera a Candado Cerrado. Debe Cambiar Candado en Cliente. Candado Abierto No Permitido para Factura Electrónica", MsgBoxStyle.Information) : TextDirecto.Text = "0.00"
                    Else
                        If MsgBox("Cliente Solo Opera a Candado Cerrado. Debe Cambiar Candado en Cliente. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then TextDirecto.Text = "0.00"
                    End If
                End If
            End If
            TextAutorizar.Text = FormatNumber(100 - CDbl(TextDirecto.Text), GDecimales, True, True, True)
        End If

        CalculaSubTotal()

    End Sub
    Private Sub TextDescuentoSobreFactura_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextDescuentoSobreFactura.KeyPress

        If Asc(e.KeyChar) = 13 Then TextDescuentoSobreFactura_Validating(Nothing, Nothing) : Exit Sub

        EsPorcentaje(e.KeyChar, TextDescuentoSobreFactura.Text)

    End Sub
    Private Sub TextDescuentoSobreFactura_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextDescuentoSobreFactura.Validating

        If Not IsNumeric(TextDescuentoSobreFactura.Text) Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextDescuentoSobreFactura.Focus()
            Exit Sub
        Else : TextDescuentoSobreFactura.Text = FormatNumber(TextDescuentoSobreFactura.Text, GDecimales, True, True, True)
            If Not CDbl(TextDescuentoSobreFactura.Text) < 100 Then
                MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                TextDescuentoSobreFactura.Focus()
                Exit Sub
            End If
        End If

        If CDbl(TextDescuentoSobreFactura.Text) <> 0 Then
            'hacer invisible columna descuento
        End If

        CalculaSubTotal()

    End Sub
    Private Sub ComboMoneda_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboMoneda.Validating

        If IsNothing(ComboMoneda.SelectedValue) Then ComboMoneda.SelectedValue = 0

    End Sub
    Private Sub TextCambio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCambio.KeyPress

        EsNumerico(e.KeyChar, TextCambio.Text, 3)

    End Sub
    Private Sub TextCambio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextCambio.Validating

        If TextCambio.Text = "" Then Exit Sub

        If CDbl(TextCambio.Text) = 0 Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextCambio.Text = ""
            TextCambio.Focus()
        End If

    End Sub
    Private Sub TextSenia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSenia.KeyPress

        EsNumerico(e.KeyChar, TextSenia.Text, 2)

    End Sub
    Private Sub TextSenia_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextSenia.Validating

        CalculaSubTotal()

    End Sub
    Private Sub TextBultos_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBultos.KeyPress

        EsNumerico(e.KeyChar, TextBultos.Text, 2)

    End Sub
    Private Sub TextBultos_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBultos.Validating

        CalculaSubTotal()

    End Sub
    Private Sub TextComprobanteDesde_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComprobanteDesde.KeyPress

        EsNumerico(e.KeyChar, TextComprobanteDesde.Text, 0)

    End Sub
    Private Sub TextComprobanteHasta_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComprobanteHasta.KeyPress

        EsNumerico(e.KeyChar, TextComprobanteHasta.Text, 0)

    End Sub
    Private Sub ComboVendedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboVendedor.Validating

        If IsNothing(ComboVendedor.SelectedValue) Then ComboVendedor.SelectedValue = 0

    End Sub
    Private Sub ComboIncoTerm_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboIncoTerm.Validating

        If IsNothing(ComboIncoTerm.SelectedValue) Then ComboIncoTerm.SelectedValue = 0

    End Sub
    Private Sub RadioFinal_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioFinal.CheckedChanged

        CalculaSubTotal()

    End Sub
    'Private Sub RadioPorUnidad_Checked(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioPorUnidad.CheckedChanged

    '    CalculaSubTotal()

    'End Sub
    Private Sub CheckSeniaEnvase_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckSeniaEnvase.CheckedChanged

        If CheckSeniaEnvase.Checked Then
            For Each Row As DataRow In DtDetalle.Rows
                If Row("Senia") <> 0 Then
                    MsgBox("Existe Señas Informadas Manualmente. Debe Borrarlas para Calcular Seña por Envase.", MsgBoxStyle.Information)
                    CheckSeniaEnvase.Checked = False
                    CheckSeniaManual.Checked = True
                    Exit Sub
                End If
            Next
            CheckSeniaManual.Checked = False
        End If

        CalculaSubTotal()

    End Sub
    Private Sub CheckSeniaManual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckSeniaManual.Click

        If CheckSeniaManual.Checked Then
            CheckSeniaEnvase.Checked = False
        End If

        CalculaSubTotal()

    End Sub
    Private Sub ButtonRelacionada_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRelacionada.Click

        PFactura = Relacionada
        PAbierto = Not PAbierto
        If Not MuestraDatos() Then Me.Close() : Exit Sub

    End Sub
    Private Function MuestraDatos() As Boolean                  'MuestraDatos

        Dim Sql As String

        If PFactura = 0 Then PAbierto = True

        If PAbierto Then
            ConexionFactura = Conexion
            ConexionRelacionada = ConexionN
        Else
            ConexionFactura = ConexionN
            ConexionRelacionada = Conexion
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtCabeza = New DataTable
        Sql = "SELECT * FROM FacturasCabeza WHERE Factura = " & PFactura & ";"
        If Not Tablas.Read(Sql, ConexionFactura, DtCabeza) Then Return False
        If PFactura <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Factura No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return False
        End If

        If DtCabeza.Rows.Count <> 0 Then
            PCliente = DtCabeza.Rows(0).Item("Cliente")
            FechaContableW = DtCabeza.Rows(0).Item("FechaContable")
        End If
        TextCliente.Text = NombreCliente(PCliente)

        ComboSucursal.DataSource = Nothing
        ComboSucursal.DataSource = Tablas.Leer("SELECT Nombre,Clave FROM SucursalesClientes WHERE Cliente = " & PCliente & ";")
        Dim Row As DataRow = ComboSucursal.DataSource.NewRow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboSucursal.DataSource.Rows.Add(Row)
        ComboSucursal.DisplayMember = "Nombre"
        ComboSucursal.ValueMember = "Clave"
        ComboSucursal.SelectedValue = 0
        With ComboSucursal
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not LlenaDatosCliente(PCliente) Then Return False

        If ComboTipoIva.SelectedValue = Exterior Then
            EsExterior = True
            PanelMoneda.Visible = True
            ButtonAnula.Visible = False
        Else : PanelMoneda.Visible = False
            EsExterior = False
            ButtonAnula.Visible = True
        End If

        If PFactura = 0 Then
            If Not AgregaCabeza() Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        End If

        Relacionada = DtCabeza.Rows(0).Item("Relacionada")
        EsServicios = DtCabeza.Rows(0).Item("EsServicios")
        EsSecos = DtCabeza.Rows(0).Item("EsSecos")
        Remito = DtCabeza.Rows(0).Item("Remito")
        EsfacturaElectronica = DtCabeza.Rows(0).Item("EsElectronica")
        EsContable = DtCabeza.Rows(0).Item("Tr")
        EsZ = DtCabeza.Rows(0).Item("EsZ")
        EsFCE = DtCabeza.Rows(0).Item("EsFCE")

        If EsContable Then
            LabelTr.Visible = True
            TextDirecto.Text = Format(100, "0.00")
            TextDirecto.ReadOnly = True
            TextAutorizar.Text = Format(0, "0.00")
            If Not PermisoTotal Then LabelTr.Visible = False
        Else : LabelTr.Visible = False
        End If

        FacturaAnteriorOK = True
        If EsfacturaElectronica And PFactura = 0 Then
            If Not EsFacturaAnteriorOk(UltimoNumero) Then
                FacturaAnteriorOK = False
                MsgBox("La Factura " & NumeroEditado(UltimoNumero - 1) & " No Tiene Autorización AFIP. Si continua, Afip Rechazara este Comprobante.")
            End If
        End If

        'Halla Relacionada.
        If PAbierto And DtCabeza.Rows(0).Item("Rel") And PermisoTotal And PFactura <> 0 Then
            Relacionada = HallaRelacionada(PFactura)
            If Relacionada < 0 Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        End If

        MuestraCabeza()

        DtDetalle = New DataTable
        Sql = "SELECT * FROM FacturasDetalle WHERE Factura = " & PFactura & ";"
        If Not Tablas.Read(Sql, ConexionFactura, DtDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False

        'Arma tabla con AsignacionLotes de facturas. 
        DtAsignacionLotes = New DataTable
        ListaDeLotes = New List(Of FilaAsignacion)
        Sql = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 2 AND Comprobante = " & PFactura & ";"
        If Not Tablas.Read(Sql, ConexionFactura, DtAsignacionLotes) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        If DtAsignacionLotes.Rows.Count <> 0 Then
            For Each Row In DtAsignacionLotes.Rows
                Dim Fila As New FilaAsignacion
                Fila.Indice = Row("Indice")
                Fila.Lote = Row("Lote")
                Fila.Secuencia = Row("Secuencia")
                Fila.Deposito = Row("Deposito")
                Fila.Operacion = Row("Operacion")
                Fila.Asignado = Row("Cantidad")
                Fila.Importe = Row("Importe")
                Fila.ImporteSinIva = Row("ImporteSinIva")
                'Muestra Permiso de Importacion.
                Fila.PermisoImp = HallaPermisoImp(Fila.Operacion, Fila.Lote, Fila.Secuencia, Fila.Deposito)
                If Fila.PermisoImp = "-1" Then
                    MsgBox("Error, Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " No Encontrado.")
                    Me.Close() : Exit Function
                End If
                ListaDeLotes.Add(Fila)
            Next
        End If

        'Arma Lista de Percpciones realizadas.--------------------------------------- 
        DtFacturaPercepciones = New DataTable
        TotalPercepciones = 0
        If PFactura = 0 Then
            Sql = "SELECT * FROM RecibosPercepciones WHERE TipoComprobante = 0 AND Comprobante = " & 0 & ";"
            If Not Tablas.Read(Sql, Conexion, DtFacturaPercepciones) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        End If
        If PFactura <> 0 And PAbierto Then
            Sql = "SELECT * FROM RecibosPercepciones WHERE TipoComprobante = 2 AND Comprobante = " & PFactura & ";"
            If Not Tablas.Read(Sql, ConexionFactura, DtFacturaPercepciones) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
            TotalPercepciones = 0
            For Each Row2 As DataRow In DtFacturaPercepciones.Rows
                TotalPercepciones = TotalPercepciones + CDec(Row2("Importe"))
            Next
        End If

        'Trata remitos de la factura -------------------------------------------------
        DtCabezaRemito = New DataTable
        If PFactura <> 0 And DtCabeza.Rows(0).Item("Remito") = 1 Then ArmaDtCabezaRemitosNuevos(DtCabeza.Rows(0), DtCabezaRemito)
        If PFactura <> 0 And DtCabeza.Rows(0).Item("Remito") > 1 Then ArmaDtCabezaRemitosViejos(DtCabeza.Rows(0), DtCabezaRemito) 'Para Compatibilizar con sistema anterior cuando el remito se informaba en la cabeza de la factura.
        If DtCabezaRemito.Rows.Count <> 0 Then
            PanelRemito.Visible = True
            If DtCabezaRemito.Rows(0).Item("Estado") = 1 Then AsignadoEnRemito = True
        End If
        ListRemitos.Clear()
        For Each Row In DtCabezaRemito.Rows
            Dim Item As New ListViewItem(NumeroEditado(Row("Remito")))
            ListRemitos.Items.Add(Item)
        Next
        '------------------------------------------------------------------------------

        If PFactura = 0 Then
            LabelPuntodeVenta.Text = "Punto de Venta " & Format(GPuntoDeVenta, "0000")
        Else : LabelPuntodeVenta.Text = ""
        End If

        If Relacionada <> 0 Then
            If PermisoTotal Then
                ButtonRelacionada.Visible = True
            Else : ButtonRelacionada.Visible = False
            End If
        Else : ButtonRelacionada.Visible = False
        End If

        IndiceW = 0
        Grid.DataSource = bs
        bs.DataSource = DtDetalle

        If PermisoTotal Then
            If PAbierto Then
                PictureCandado.Image = ImageList1.Images.Item("Abierto")
            Else : PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            End If
        End If

        Grid.Columns("Indice").Visible = False

        Dim TotalNeto As Decimal = 0
        Dim TotalIva As Decimal = 0
        Dim TotalNetoPrecioLista As Decimal = 0

        If EmpresaOk() And RadioFinal.Checked Then
            For I As Integer = 0 To Grid.Rows.Count - 2
                Dim CoeficienteW As Decimal = 1 + CDec(Grid.Rows(I).Cells("Iva").Value) / 100
                Grid.Rows(I).Cells("Neto").Value = Grid.Rows(I).Cells("TotalArticulo").Value / CoeficienteW
                TotalNeto = TotalNeto + Grid.Rows(I).Cells("Neto").Value
                Grid.Rows(I).Cells("MontoIva").Value = Grid.Rows(I).Cells("TotalArticulo").Value - Grid.Rows(I).Cells("neto").Value
                TotalIva = TotalIva + Grid.Rows(I).Cells("MontoIva").Value
                TotalNetoPrecioLista = TotalNetoPrecioLista + Grid.Rows(I).Cells("Cantidad").Value * Grid.Rows(I).Cells("PrecioLista").Value
                HallaAGranelYMedidaFactura(EsSecos, EsServicios, Grid.Rows(I).Cells("Articulo").Value, Grid.Rows(I).Cells("Agranel").Value, Grid.Rows(I).Cells("Medida").Value)
                Grid.Rows(I).Cells("UMedida").Value = HallaUMedidaArticulo(Grid.Rows(I).Cells("Articulo").Value)
            Next
        Else
            For I As Integer = 0 To Grid.Rows.Count - 2
                If Grid.Rows(I).Cells("Descuento").Value = 0 Then
                    Grid.Rows(I).Cells("PrecioLista").Value = Grid.Rows(I).Cells("Precio").Value
                Else
                    If Grid.Rows(I).Cells("PrecioLista").Value = 0 Then  'para compatibilizar anterior donde no guardaba PrecioLista.  
                        Grid.Rows(I).Cells("PrecioLista").Value = Trunca3(Grid.Rows(I).Cells("Precio").Value / (1 - Grid.Rows(I).Cells("Descuento").Value / 100))
                    End If
                End If
                Grid.Rows(I).Cells("Neto").Value = CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("Precio").Value)
                TotalNeto = TotalNeto + Grid.Rows(I).Cells("Neto").Value
                Grid.Rows(I).Cells("MontoIva").Value = CalculaIva(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("Precio").Value, Grid.Rows(I).Cells("Iva").Value)
                TotalNetoPrecioLista = TotalNetoPrecioLista + CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioLista").Value)
                '  TotalIva = TotalIva + Grid.Rows(I).Cells("MontoIva").Value
                HallaAGranelYMedidaFactura(EsSecos, EsServicios, Grid.Rows(I).Cells("Articulo").Value, Grid.Rows(I).Cells("Agranel").Value, Grid.Rows(I).Cells("Medida").Value)
                Grid.Rows(I).Cells("UMedida").Value = HallaUMedidaArticulo(Grid.Rows(I).Cells("Articulo").Value)
            Next
            TotalIva = CalculaTotalIva(Grid)
        End If

        Dim DescuentoTotal As Decimal = 0
        Dim DescuentoW As Decimal = 0
        Dim TotalNetoW As Decimal = 0

        If DtCabeza.Rows(0).Item("Descuento") <> 0 Then
            DescuentoW = DtCabeza.Rows(0).Item("Descuento")
        Else
            DescuentoW = TotalNetoPrecioLista - TotalNeto
        End If
        TotalNetoW = TotalNetoPrecioLista - DescuentoW

        TextNetolPrecioLista.Text = FormatNumber(TotalNetoPrecioLista, GDecimales)
        TextDescuento.Text = FormatNumber(DescuentoW, GDecimales)
        TextTotalNeto.Text = FormatNumber(TotalNetoW, GDecimales)
        TextTotalIva.Text = FormatNumber(TotalIva, GDecimales)
        TextSubTotal.Text = FormatNumber(TotalNetoW + TotalIva, GDecimales)
        TextPercepcion.Text = FormatNumber(TotalPercepciones, GDecimales)

        If PFactura <> 0 Then
            ButtonAceptar.Text = "Graba Cambios"
            TextCambio.Enabled = False
            Panel4.Enabled = False
            Panel5.Enabled = False
            CheckSeniaEnvase.Enabled = False
            CheckSeniaEnvase.Checked = False
            CheckSeniaManual.Enabled = False
            TextBultos.ReadOnly = True
            TextSenia.ReadOnly = True
            ButtonEliminarLinea.Enabled = False
            PanelCandado.Visible = False
            Grid.ReadOnly = True
            TextDestino.ReadOnly = True
            If DtCabeza.Rows(0).Item("Remito") <> 0 Then
                CheckConfirmado.Enabled = False
            Else
                CheckConfirmado.Enabled = True
            End If
            ComboSucursal.Enabled = True
            ButtonArticulosEnStock.Visible = False
            If Not PAbierto Then
                CheckImprimeDomicilio.Visible = True
            Else
                CheckImprimeDomicilio.Visible = False
            End If
        Else : ButtonAceptar.Text = "Alta Factura"
            TextCambio.Enabled = True
            Panel4.Enabled = True
            Panel5.Enabled = True
            PanelSenia.Enabled = True
            CheckSeniaEnvase.Enabled = True
            CheckSeniaEnvase.Checked = False
            CheckSeniaManual.Enabled = True
            CheckSeniaManual.Checked = True
            CheckImprimeDomicilio.Visible = False
            ButtonEliminarLinea.Enabled = True
            PictureCandado.Visible = False
            If PermisoTotal Then
                PanelCandado.Visible = True
            Else : PanelCandado.Visible = False
            End If
            Grid.ReadOnly = False
            TextDestino.ReadOnly = False
            CheckConfirmado.Enabled = False
            ComboSucursal.Enabled = False
            ButtonArticulosEnStock.Visible = False
            If EsArticulosLoteados Then
                ButtonArticulosEnStock.Visible = True
                Grid.ReadOnly = True
                ButtonEliminarLinea.Enabled = False
                PanelSenia.Enabled = False
                CheckSeniaManual.Checked = True
                CheckSeniaManual.Checked = True
                CheckSeniaEnvase.Checked = False
            End If
        End If

        If PFactura = 0 And Lista <> 0 Then
            If FinalEnLista Then
                RadioFinal.Checked = True
                RadioSinIva.Enabled = False
            Else
                RadioSinIva.Checked = True
                RadioFinal.Enabled = False
            End If
        End If

        HallaTipoAsientoFactura(DtCabeza.Rows(0).Item("EsExterior"), EsServicios, EsSecos, EsContable, TipoAsiento, TipoAsientoCosto)

        Panel7.Enabled = False

        If PFactura = 0 Then
            TextFechaContable.Text = Format(DateTime1.Value, "dd/MM/yyyy")
            If EsfacturaElectronica And DateTime1.Value.Day < 11 Then TextFechaContable.Text = "" : Panel7.Enabled = True
            If EsSecos Or EsContable Then TextFechaContable.Text = "" : Panel7.Enabled = True
        End If
        If PFactura <> 0 And (EsContable Or EsSecos) Then
            Panel7.Enabled = True
        End If

        If EsZ Then
            Panel7.Enabled = True
            DateTime1.Enabled = True
        Else
            DateTime1.Enabled = False
        End If

        If ListaDeLotes.Count <> 0 Or EsArticulosLoteados Then
            ComboDeposito.Enabled = False
        Else
            ComboDeposito.Enabled = True
        End If

        AddHandler DtDetalle.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtdetalle_ColumnChanging)
        AddHandler DtDetalle.ColumnChanged, New DataColumnChangeEventHandler(AddressOf Dtdetalle_ColumnChanged)
        AddHandler DtDetalle.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtDetalle_NewRow)
        AddHandler DtDetalle.RowChanged, New DataRowChangeEventHandler(AddressOf DtDetalle_RowChanged)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Row As DataRowView = MiEnlazador.Current

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Factura")
        AddHandler Enlace.Format, AddressOf FormatFactura
        AddHandler Enlace.Parse, AddressOf FormatParseFactura
        MaskedFactura.DataBindings.Clear()
        MaskedFactura.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "ClienteOperacion")
        ComboClienteOperacion.DataBindings.Clear()
        ComboClienteOperacion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Sucursal")
        ComboSucursal.DataBindings.Clear()
        ComboSucursal.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Deposito")
        ComboDeposito.DataBindings.Clear()
        ComboDeposito.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Vendedor")
        ComboVendedor.DataBindings.Clear()
        ComboVendedor.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "IncoTerm")
        ComboIncoTerm.DataBindings.Clear()
        ComboIncoTerm.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaEntrega")
        DateEntrega.DataBindings.Clear()
        DateEntrega.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaPago")
        ''      AddHandler Enlace.Parse, AddressOf ParseFecha
        DateFechaPago.DataBindings.Clear()
        DateFechaPago.DataBindings.Add(Enlace)

        If Row("Final") Then
            RadioFinal.Checked = True
        Else : RadioSinIva.Checked = True
        End If

        If Row("FormaPago") = 3 Then
            '    Mixto
        End If
        If Row("FormaPago") = 4 Then
            '    ContadoEfectivo
        End If

        Enlace = New Binding("Text", MiEnlazador, "Senia")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextSenia.DataBindings.Clear()
        TextSenia.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Bultos")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextBultos.DataBindings.Clear()
        TextBultos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalFactura.DataBindings.Clear()
        TextTotalFactura.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Percepciones")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextPercepcion.DataBindings.Clear()
        TextPercepcion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Moneda")
        ComboMoneda.DataBindings.Clear()
        ComboMoneda.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cambio")
        AddHandler Enlace.Format, AddressOf FormatCambio
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextCambio.DataBindings.Clear()
        TextCambio.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Destino")
        TextDestino.DataBindings.Clear()
        TextDestino.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Pedido")
        TextPedido.DataBindings.Clear()
        TextPedido.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "PedidoCliente")
        TextPedidoCliente.DataBindings.Clear()
        TextPedidoCliente.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "ComprobanteDesde")
        AddHandler Enlace.Parse, AddressOf ParseEntero
        TextComprobanteDesde.DataBindings.Clear()
        TextComprobanteDesde.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "ComprobanteHasta")
        AddHandler Enlace.Parse, AddressOf ParseEntero
        TextComprobanteHasta.DataBindings.Clear()
        TextComprobanteHasta.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Confirmado")
        CheckConfirmado.DataBindings.Clear()
        CheckConfirmado.DataBindings.Add(Enlace)

        TextTotalGeneral.Text = FormatNumber(Row("Importe") + Row("Percepciones"), 2)

        If Row("FechaElectronica") = "01/01/1800" Then
            TextFechaAfip.Text = ""
        Else : TextFechaAfip.Text = Format(Row("FechaElectronica"), "dd/MM/yyyy")
        End If

        If Row("FechaContable") = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Row("FechaContable"), "dd/MM/yyyy")
        End If

        If Row("Cae") <> 0 Then
            LabelCAE.Text = "Autorización AFIP  CAE: " & Row("Cae") & "  Vto: " & Strings.Right(Row("FechaCae"), 2) & "/" & Strings.Mid(Row("FechaCae"), 5, 2) & "/" & Strings.Left(Row("FechaCae"), 4) : LabelCAE.Visible = True
        Else
            LabelCAE.Visible = False
        End If

        If Row("EsFCE") Then
            LabelFCE.Text = "Factura de Crédito MiPyMEs(FCE)"
            EsFCE = True
            Panel3.Visible = True
        Else
            LabelFCE.Text = ""
            EsFCE = False
            Panel3.Visible = False
        End If

    End Sub
    Private Sub FormatFactura(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        TipoFactura = Strings.Left(Numero.Value, 1)
        TextTipoFactura.Text = LetraTipoIva(TipoFactura)
        Numero.Value = Strings.Right(Numero.Value, 12)

    End Sub
    Private Sub FormatParseFactura(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = CDbl(TipoFactura & Format(Val(MaskedFactura.Text), "000000000000"))

    End Sub
    Private Sub FormatRemito(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = Format(Numero.Value, "0000-00000000")
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If IsDBNull(Numero.Value) Then Numero.Value = 0
        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub FormatCambio(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If IsDBNull(Numero.Value) Then Numero.Value = 0
        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 3)
        End If

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub ParseEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub ParseFecha(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "yyyy/MM/dd")

    End Sub
    Private Sub ArmaDtCabezaRemitosNuevos(ByVal Row As DataRow, ByVal DtRemitos As DataTable)

        Dim NumeroFacturaDelRemito As Double = 0

        If ConexionFactura = Conexion Then
            ConexionRemito = Conexion
            NumeroFacturaDelRemito = DtCabeza.Rows(0).Item("Factura")
        Else
            If DtCabeza.Rows(0).Item("Relacionada") <> 0 Then
                ConexionRemito = Conexion
                NumeroFacturaDelRemito = DtCabeza.Rows(0).Item("Relacionada")
            Else
                ConexionRemito = ConexionN
                NumeroFacturaDelRemito = DtCabeza.Rows(0).Item("Factura")
            End If
        End If

        If Not Tablas.Read("SELECT * FROM RemitosCabeza WHERE Factura = " & NumeroFacturaDelRemito & ";", ConexionRemito, DtCabezaRemito) Then End

    End Sub
    Private Sub ArmaDtCabezaRemitosViejos(ByVal Row As DataRow, ByVal DtRemitos As DataTable)

        ConexionRemito = ConexionN

        If Not Tablas.Read("SELECT * FROM RemitosCabeza WHERE Remito = " & Row("Remito") & ";", ConexionRemito, DtRemitos) Then End
        If DtCabezaRemito.Rows.Count = 0 Then
            ConexionRemito = Conexion
            If Not Tablas.Read("SELECT * FROM RemitosCabeza WHERE Remito = " & Row("Remito") & ";", ConexionRemito, DtRemitos) Then End
        End If

    End Sub
    Private Function PideAutorizacionAfip(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtFacturaPercepcionesW As DataTable) As Boolean

        If DtCabezaW.Rows.Count = 0 Then Return True
        If Not DtCabezaW.Rows(0).Item("EsElectronica") Then Return True

        Dim CAE As String = ""
        Dim FechaCae As String = ""
        Dim Concepto As Integer = 0
        Dim FchServDesde As Date = DateAdd(DateInterval.Day, 30, CDate(TextFechaContable.Text))
        Dim FchServHasta As Date = DateAdd(DateInterval.Day, 30, CDate(TextFechaContable.Text))
        Dim FchVtoPago As Date = DateAdd(DateInterval.Day, 30, CDate(TextFechaContable.Text))
        Dim CbteTipoAsociado As Integer = 0
        Dim CbteAsociado As Decimal = 0
        Dim CancelarGrabar As Boolean
        Dim TipoArticulo As Integer
        If DtCabezaW.Rows(0).Item("EsServicios") Then
            Concepto = 2
        Else
            Concepto = 1
        End If
        If EsSecos Or EsServicios Then
            TipoArticulo = 2
        Else
            TipoArticulo = 1
        End If

        If EsFCE Then
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT CBU FROM DatosEmpresa WHERE Indice = 1;", Conexion, Dt) Then Return False
            If Dt.Rows(0).Item("CBU") = 0 Then
                MsgBox("CBU de la Empresa No Informado." + vbCrLf + "Deberá Pedir Autorización AFIP.")
                Dt.Dispose()
                Return False
            End If
            CBU = Dt.Rows(0).Item("CBU")
            Dt.Dispose()
        End If

        Dim DatosParaAfip As New ItemDatosParaAFIP
        DatosParaAfip.CBU = CBU
        DatosParaAfip.AgenteDeposito = DtCabezaW.Rows(0).Item("AgenteDeposito")
        DatosParaAfip.EsFCE = EsFCE
        DatosParaAfip.FechaPago = DtCabezaW.Rows(0).Item("FechaPago")
        DatosParaAfip.InscripcionAfip = DtCabezaW.Rows(0).Item("TipoIva")      'Responsable insc, montributo, etc.
        DatosParaAfip.DocTipo = DocTipo    'Tipo Documento Consumidor Final.
        DatosParaAfip.DocNro = DocNro      'Numero Documento Consumidor Final.

        Dim Mensaje = Autorizar("F", DtCabezaW, DtDetalleW, DtFacturaPercepcionesW, FchServDesde, FchServHasta, FchVtoPago, TipoFactura, MaskedFactura.Text, Cuit, Concepto, TipoArticulo, CAE, FechaCae, CbteTipoAsociado, CbteAsociado, CancelarGrabar, DatosParaAfip)
        If CAE = "" Then
            MsgBox(Mensaje + vbCrLf + "Deberá Pedir Autorización AFIP.")
            Return False
        End If

        If CAE <> "" Then
            If Not GrabaCAE(DtCabezaW.Rows(0).Item("Factura"), CDec(CAE), CInt(FechaCae)) Then
                MsgBox("CAE de la AFIP No se pudo Grabar." + vbCrLf + "Deberá Pedir Autorización AFIP.")
                Return False
            End If
            '    VerificaCAE("F", 0, TipoFactura, DtCabeza.Rows(0).Item("Factura"), CDec(CAE), DtCabeza.Rows(0).Item("Importe"))
        End If

        Return True

    End Function
    Public Function GrabaCAE(ByVal Factura As Decimal, ByVal Cae As Decimal, ByVal FechaCae As Integer) As Boolean

        Dim Sql As String = "UPDATE FacturasCabeza Set CAE = " & Cae & ",FechaCae = " & FechaCae &
            " WHERE Factura = " & Factura & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                If CInt(Cmd.ExecuteNonQuery()) = 0 Then Return False
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos al Grabar CAE en FacturaCabeza." & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End Try

        Return True

    End Function
    Private Function LlenaDatosCliente(ByVal Cliente As Integer) As Boolean

        Dim Dta As New DataTable

        Dim Sql As String = "SELECT * FROM Clientes WHERE Clave = " & PCliente & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, el Cliente ya no existe o error en la Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Dta.Dispose()
            Return False
        End If

        Calle = Dta.Rows(0).Item("Calle")
        Numero = Dta.Rows(0).Item("Numero")
        Localidad = Dta.Rows(0).Item("Localidad")
        DocTipo = Dta.Rows(0).Item("DocumentoTipo")
        DocNro = Dta.Rows(0).Item("DocumentoNumero")
        ClienteOpr = Dta.Rows(0).Item("Opr")
        Provincia = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
        Cuit = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")
        DateFechaPago.Value = DateAdd(DateInterval.Day, Dta.Rows(0).Item("CondicionPago"), Date.Now)
        TextoFijoParaFacturas1 = Dta.Rows(0).Item("TextoFijoParaFacturas1")
        TextoFijoParaFacturas2 = Dta.Rows(0).Item("TextoFijoParaFacturas2")
        '
        CtaCteCerrada = Dta.Rows(0).Item("CtaCteCerrada")
        TieneCodigoCliente = Dta.Rows(0).Item("TieneCodigoCliente")
        ComboMoneda.SelectedValue = Dta.Rows(0).Item("Moneda")
        If PFactura = 0 Then
            ComboVendedor.SelectedValue = Dta.Rows(0).Item("Vendedor")
        End If

        If PFactura = 0 Then
            TextDirecto.Text = Format(Dta.Rows(0).Item("Directo"), "0.00")
            TextAutorizar.Text = Format(100 - Dta.Rows(0).Item("Directo"), "0.00")
            If Not PermisoTotal Then
                TextDirecto.Text = Format(100, "0.00")
                TextAutorizar.Text = Format(0, "0.00")
            End If
            If Not ClienteOpr Then
                TextDirecto.Text = Format(0, "0.00")
                TextAutorizar.Text = Format(100, "0.00")
            End If
        End If

        If PFactura = 0 Then
            If Dta.Rows(0).Item("Descuento") <> 0 Then
                TextDescuentoSobreFactura.Text = Format(Dta.Rows(0).Item("Descuento"), "0.00")
                TextDescuentoSobreFactura.ReadOnly = True
            Else
                TextDescuentoSobreFactura.Text = Format(0, "0.00")
                TextDescuentoSobreFactura.ReadOnly = False
            End If
        Else
            TextDescuentoSobreFactura.ReadOnly = True
        End If

        Dta.Dispose()

        Return True

    End Function
    Private Function HallaAsignacionLotesRemito(ByRef DtAsignacion As DataTable, ByVal ConexionRemito As String) As Boolean

        For Each Row As DataRow In DtCabezaRemito.Rows
            If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante = 1 AND Comprobante = " & Row("Remito") & ";", ConexionRemito, DtAsignacion) Then Return False
        Next

        Return True

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow
        Dim DtArticulo As DataTable

        If PFactura <> 0 Then
            If EsServicios Then
                DtArticulo = Tablas.Leer("SELECT * FROM ArticulosServicios WHERE Secos = 0;")
            Else
                If EsSecos Then
                    DtArticulo = Tablas.Leer("SELECT * FROM ArticulosServicios WHERE Secos = 1;")
                Else
                    DtArticulo = TodosLosArticulos()
                End If
            End If
        End If

        If PFactura = 0 Then
            If EsServicios Then
                DtArticulo = Tablas.Leer("SELECT * FROM ArticulosServicios WHERE Estado = 1 AND Secos = 0;")
            Else
                If EsSecos Then
                    DtArticulo = Tablas.Leer("SELECT * FROM ArticulosServicios WHERE Estado = 1 AND Secos = 1;")
                Else
                    DtArticulo = ArticulosActivos()
                End If
            End If
        End If

        'Saco articulos que no estan en lista de precios.
        If PFactura = 0 And Lista <> 0 And Remito = 0 And Pedido = 0 Then
            Dim Dt As DataTable
            Dt = HallaArticulosListaDePrecio(Lista)
            Dim RowsBusqueda() As DataRow
            For Each Row In DtArticulo.Rows
                RowsBusqueda = Dt.Select("Articulo = " & Row("Clave"))
                If RowsBusqueda.Length = 0 Then
                    Row.Delete()
                End If
            Next
            Dt.Dispose()
        End If

        If PFactura = 0 And TieneCodigoCliente And Lista = 0 And Remito = 0 And Pedido = 0 Then
            Dim Dt As DataTable
            Dt = HallaArticulosConCodigo(PCliente)
            Dim RowsBusqueda() As DataRow
            For Each Row In DtArticulo.Rows
                RowsBusqueda = Dt.Select("Articulo = " & Row("Clave"))
                If RowsBusqueda.Length = 0 Then
                    Row.Delete()
                End If
            Next
            Dt.Dispose()
        End If

        If PFactura = 0 And Pedido <> 0 Then
            Dim RowsBusqueda() As DataRow
            For Each Row In DtArticulo.Rows
                RowsBusqueda = DtPedido.Select("Articulo = " & Row("Clave"))
                If RowsBusqueda.Length = 0 Then
                    Row.Delete()
                End If
            Next
        End If

        Articulo.DataSource = DtArticulo
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

        Dim Dt2 As New DataTable
        ArmaTipoPrecio(Dt2)
        TipoPrecio.DataSource = Dt2.Copy
        TipoPrecio.DisplayMember = "Nombre"
        TipoPrecio.ValueMember = "Clave"
        Dt2.Dispose()

    End Sub
    Private Function FormatoSinRedondeo3Decimales(ByVal Numero As Double) As Double

        Dim PosicionDecimal As Integer = InStr(1, Numero.ToString, ",")
        If PosicionDecimal = 0 Then
            Return Numero
        Else
            Return CDbl(Mid(Numero.ToString, 1, PosicionDecimal + 3))
        End If

    End Function
    Private Sub ArmaListaImportesIva(ByRef Lista As List(Of ItemIva))

        Dim Esta As Boolean

        Lista = New List(Of ItemIva)

        For Each Row As DataGridViewRow In Grid.Rows
            If IsNothing(Row.Cells("Articulo").Value) Then Exit For
            Esta = False
            For Each Fila As ItemIva In Lista
                If Fila.Iva = Row.Cells("Iva").Value Then
                    Fila.Importe = Fila.Importe + Row.Cells("MontoIva").Value
                    Esta = True
                End If
            Next
            If Not Esta Then
                Dim Fila As New ItemIva
                Fila.Iva = Row.Cells("Iva").Value
                Fila.Importe = Row.Cells("MontoIva").Value
                Lista.Add(Fila)
            End If
        Next

    End Sub
    Private Sub CalculaNetoSinDescuentoMasIva(ByRef Neto As Decimal, ByRef Descuento As Decimal)

        Neto = 0
        Descuento = 0

        Dim PrecioLista As Decimal = 0
        Dim NetoTotal As Decimal

        For I As Integer = 0 To Grid.Rows.Count - 2
            If Grid.Rows(I).Cells("Descuento").Value = 0 Then
                PrecioLista = Grid.Rows(I).Cells("Precio").Value
            Else
                PrecioLista = Trunca3(Grid.Rows(I).Cells("Precio").Value / (1 - Grid.Rows(I).Cells("Descuento").Value / 100))
            End If
            Neto = Neto + CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, PrecioLista) + CalculaIva(Grid.Rows(I).Cells("Cantidad").Value, PrecioLista, Grid.Rows(I).Cells("Iva").Value)
            NetoTotal = NetoTotal + Grid.Rows(I).Cells("TotalArticulo").Value
        Next

        Descuento = Neto - NetoTotal

    End Sub
    Private Function Coincidencia(ByVal Fila As FilaAsignacion) As Boolean

        If Fila.Indice = IndiceCoincidencia Then Return True

    End Function
    Private Function HallaPrecioDeListaRemito(ByVal Cliente As Integer, ByVal Fecha As Date, ByVal Articulo As Integer, ByVal Sucursal As Integer, ByVal TipoListaDePreciosDelCliente As Integer) As Double

        Dim FinalEnLista As Boolean
        Dim PorUnidadEnLista As Boolean

        Dim Lista As Integer = HallaListaPrecios(Cliente, Sucursal, Fecha, PorUnidadEnLista, FinalEnLista)

        If Lista = -1 Then Lista = 0

        If Lista <> 0 Then
            If FinalEnLista Then
                RadioFinal.Checked = True
                RadioSinIva.Enabled = False
            Else
                RadioSinIva.Checked = True
                RadioFinal.Enabled = False
            End If
        End If

        Return Lista

    End Function
    Private Function AgregaCabeza() As Boolean

        Dim Row As DataRow

        If Not EsZ Then
            UltimoNumero = UltimaNumeracionFactura(TipoFactura, GPuntoDeVenta)
        Else
            UltimoNumero = TipoFactura & Format(GPuntoDeVenta, "0000") & "00000000"
        End If
        If UltimoNumero < 0 Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        EsfacturaElectronica = EsPuntoDeVentaFacturasElectronicas(GPuntoDeVenta)

        Row = DtCabeza.NewRow()
        ArmaNuevaFactura(Row)
        Row("Factura") = UltimoNumero
        Row("Cliente") = PCliente
        Row("Sucursal") = Sucursal
        Row("Remito") = Remito
        Row("Deposito") = Deposito
        Row("TipoIva") = ComboTipoIva.SelectedValue
        Row("Fecha") = Now
        Row("EsServicios") = EsServicios
        Row("EsSecos") = EsSecos
        Row("Estado") = 2
        If EsServicios Or EsSecos Then Row("Estado") = 2
        Row("Final") = 1
        '--------arreglo especial para merton--------------se confunde-$55ÇÇç
        If GCuitEmpresa = "30-71547547-9" Then Row("Final") = 0
        '--------------------------------------------------
        Row("Moneda") = ComboMoneda.SelectedValue
        If ComboMoneda.SelectedValue = 1 Then
            Row("Cambio") = 1
        Else : Row("Cambio") = 0
        End If
        Row("Impreso") = False
        Row("Destino") = ""
        If Sucursal <> 0 And Remito = 0 Then
            Row("Destino") = HallaDireccionSucursalCliente(PCliente, Sucursal)
        End If
        Row("Pedido") = Pedido
        Row("PedidoCliente") = ""
        If Pedido <> 0 Then Row("PedidoCliente") = HallaPedidoCliente(Pedido)
        Row("FechaEntrega") = FechaEntrega
        Row("EsElectronica") = EsfacturaElectronica
        Row("Tr") = PEsContable
        Row("EsZ") = EsZ
        Row("EsFCE") = EsFCE
        Row("FechaPago") = DateFechaPago.Value
        Row("ComprobanteDesde") = ComprobanteDesde
        Row("ComprobanteHasta") = ComprobanteHasta
        Row("Vendedor") = ComboVendedor.SelectedValue
        DtCabeza.Rows.Add(Row)

        Return True

    End Function
    Private Sub AgregaADtDetalle(ByVal Articulo As Integer, ByVal KilosXUnidad As Double, ByVal Iva As Double)

        Dim Row As DataRow

        Row = DtDetalle.NewRow()
        IndiceW = IndiceW + 1
        Row("Indice") = IndiceW
        Row("Factura") = UltimoNumero
        Row("Articulo") = 0
        Row("KilosXUnidad") = 0
        Row("Iva") = Iva
        Row("Precio") = 0
        Row("Cantidad") = 0
        Row("TotalArticulo") = 0
        Row("Devueltas") = 0
        DtDetalle.Rows.Add(Row)

    End Sub
    Private Sub AgregaRemitoADtDetalle(ByVal Remito As Decimal, ByVal Articulo As Integer, ByVal KilosXUnidad As Double, ByVal Iva As Double, ByVal Cantidad As Decimal, ByVal Fecha As Date, ByVal Precio As Double, ByVal TipoPrecio As Integer, ByRef Indice As Integer)

        Dim Row As DataRow

        Row = DtDetalle.NewRow()
        Row("Articulo") = Articulo
        Row("Cantidad") = Cantidad
        Row("Precio") = Precio
        Row("TipoPrecio") = TipoPrecio
        Row("KilosXUnidad") = KilosXUnidad
        Row("TotalArticulo") = 0
        Row("Devueltas") = 0
        Row("Descuento") = 0
        Row("Remito") = Remito
        Row("Senia") = 0
        DtDetalle.Rows.Add(Row)

        Dim Index As Integer = DtDetalle.Rows.Count - 1
        Grid.Rows(Index).Cells("FechaRemito").Value = Fecha   'es por si tengo que recalcular precio en lista de precio. 
        Grid.Rows(Index).Cells("PrecioLista").Value = Precio

        Indice = Index + 1

    End Sub
    Private Function ArmaConDetalleDeRemitos() As Boolean

        Dim Dt As New DataTable
        Dim Sql As String
        Dim Indice As Integer = 0
        Dim Kilos As Double
        Dim Iva As Double
        Dim RowsBusqueda() As DataRow

        If ListaRemitos(0).Abierto Then
            ConexionRemito = Conexion
        Else
            ConexionRemito = ConexionN
        End If

        ListaDeLotes = New List(Of FilaAsignacion)
        DtAsignacionLotesRemito = New DataTable
        DtCabezaRemito = New DataTable

        SucursalRemito = 0

        For Each Fila As ItemRemito In ListaRemitos
            Sql = "SELECT * FROM RemitosCabeza WHERE Remito = " & Fila.Remito & ";"
            If Not Tablas.Read(Sql, ConexionRemito, DtCabezaRemito) Then Return False
        Next

        For Each Row As DataRow In DtCabezaRemito.Rows
            If Row("Factura") <> 0 Then
                MsgBox("Remito " & NumeroEditado(Row("Remito")) & " Ya fue Facturado.Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Return False
            End If
            For I As Integer = 1 To Row("PedidoCliente").ToString.Trim.Length
                If TextPedidoCliente.Text.Length < 30 Then
                    TextPedidoCliente.Text = TextPedidoCliente.Text & Mid$(Row("PedidoCliente"), I, 1)
                End If
            Next
            If TextPedidoCliente.Text.Length < 30 Then TextPedidoCliente.Text = TextPedidoCliente.Text & " "
        Next

        Dim PedidoRemito As Integer = DtCabezaRemito.Rows(0).Item("Pedido")
        If PedidoRemito <> 0 Then
            ControlaParametrosPedido(PedidoRemito)
        End If

        If AsignadoEnRemito Then
            For Each Fila As ItemRemito In ListaRemitos
                Sql = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 1 AND Comprobante = " & Fila.Remito & ";"
                If Not Tablas.Read(Sql, ConexionRemito, DtAsignacionLotesRemito) Then Return False
            Next
        End If

        For Each Fila As ItemRemito In ListaRemitos
            Dt.Clear()
            Sql = "SELECT D.*,C.Sucursal,C.PorCuentaYOrden,C.SucursalPorCuentaYOrden FROM RemitosCabeza AS C INNER JOIN RemitosDetalle AS D ON C.Remito = D.Remito WHERE C.Remito = " & Fila.Remito & ";"
            If Not Tablas.Read(Sql, ConexionRemito, Dt) Then Return False
            Dim Contador As Integer
            For Each Row1 As DataRow In Dt.Rows
                Contador = Contador + 1
            Next
            If Contador > GLineasFacturas And Not PermiteMuchosArticulos And Not EsfacturaElectronica Then
                If MsgBox("Articulos De Los Remitos Supera numero de Articulos Permitidos para Facturas.(" & GLineasFacturas & "). Si Continua NO prodra Imprimirse. Desea Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    PermiteMuchosArticulos = True
                Else
                    Return False
                End If
            End If
            For Each Row As DataRow In Dt.Rows
                HallaKilosIva(Row("Articulo"), Kilos, Iva)
                If (Row("Cantidad") - Row("Devueltas")) > 0 Then
                    RowsBusqueda = DtCabezaRemito.Select("Remito = " & Fila.Remito)
                    Dim Precio As Double = 0
                    Dim TipoPrecio As Integer = 0
                    Dim Sucursal As Integer = 0
                    If Row("PorCuentaYOrden") <> 0 Then
                        Sucursal = Row("SucursalPorCuentaYOrden")
                    Else
                        Sucursal = Row("Sucursal")
                    End If
                    If TipoListaDePreciosDelCliente <> 0 Then  ' con TipoListaDePreciosDelCliente <> 0 Tiene lista de precios.
                        'Halla precio = HallaPrecioDeListaRemito hay que hcerlo para cada remito.
                        Dim ListaDelRemito As Integer = HallaPrecioDeListaRemito(RowsBusqueda(0).Item("Cliente"), RowsBusqueda(0).Item("FechaEntrega"), Row("Articulo"), Sucursal, TipoListaDePreciosDelCliente)
                        If ListaDelRemito <> 0 Then
                            Panel4.Visible = False
                            Dim Final As Boolean
                            HallaPrecioDeListaSegunArticuloConTipoPrecio(ListaDelRemito, RowsBusqueda(0).Item("FechaEntrega"), Row("Articulo"), Precio, TipoPrecio, Final)
                        End If
                    End If
                    If PedidoRemito <> 0 And TipoListaDePreciosDelCliente = 0 Then       ' Si no tiene lista de precios pero es sobre un remito con pedido toma precios del pedido.
                        Dim Cantidad As Decimal : Dim Entregada As Decimal : Dim ArticuloExisteEnPedido As Boolean
                        HallaCantidadYPrecioPedido(PedidoRemito, Row("Articulo"), Cantidad, Entregada, Precio, TipoPrecio, ArticuloExisteEnPedido)
                    End If
                    AgregaRemitoADtDetalle(Fila.Remito, Row("Articulo"), Row("KilosXUnidad"), Iva, Row("Cantidad") - Row("Devueltas"), RowsBusqueda(0).Item("FechaEntrega"), Precio, TipoPrecio, Indice)
                    If AsignadoEnRemito Then
                        RowsBusqueda = DtAsignacionLotesRemito.Select("Comprobante = " & Fila.Remito & " AND Indice = " & Row("Indice"))
                        For Each Row1 As DataRow In RowsBusqueda
                            Dim Fila1 As New FilaAsignacion
                            Fila1.Indice = Indice
                            Fila1.Lote = Row1("Lote")
                            Fila1.Secuencia = Row1("Secuencia")
                            Fila1.Deposito = Row1("Deposito")
                            Fila1.Operacion = Row1("Operacion")
                            Fila1.Asignado = Row1("Cantidad")
                            Fila1.Importe2 = 0
                            'Muestra Permiso de Importacion.
                            Fila1.PermisoImp = HallaPermisoImp(Fila1.Operacion, Fila1.Lote, Fila1.Secuencia, Fila1.Deposito)
                            If Fila1.PermisoImp = "-1" Then
                                MsgBox("Error, Lote " & Fila1.Lote & "/" & Format(Fila1.Secuencia, "000") & " No Encontrado.")
                                Return False
                            End If
                            ListaDeLotes.Add(Fila1)
                        Next
                    End If
                End If
            Next
        Next

        Dim SucursalAnt As Integer = -1
        Dim SucursalW As Integer
        For Each Row As DataRow In DtCabezaRemito.Rows
            If Row("SucursalPorCuentaYOrden") <> 0 Then
                SucursalW = Row("SucursalPorCuentaYOrden")
            Else
                SucursalW = Row("Sucursal")
            End If
            If SucursalAnt = -1 Then
                SucursalAnt = SucursalW
            Else
                If SucursalAnt <> SucursalW Then
                    SucursalAnt = -2
                End If
            End If
        Next
        If SucursalAnt <> -2 Then
            ComboSucursal.SelectedValue = SucursalAnt
        End If

        Dt.Dispose()

        Return True

    End Function
    Private Function ArmaConDetalleDePedido() As Boolean

        Dim Precio As Decimal
        Dim TipoPrecio As Decimal
        Dim Cantidad As Decimal
        Dim Entregada As Decimal
        Dim ArticuloExisteEnPedido As Boolean

        For Each Row As DataRow In DtPedido.Rows
            HallaCantidadYPrecioPedido(Pedido, Row("Articulo"), Cantidad, Entregada, Precio, TipoPrecio, ArticuloExisteEnPedido)
            Cantidad = Cantidad - Entregada
            If Cantidad < 0 Then Cantidad = 0
            If Lista <> 0 Then
                Dim Final As Boolean
                HallaPrecioDeListaSegunArticuloConTipoPrecio(Lista, DateEntrega.Value, Row("Articulo"), Precio, TipoPrecio, Final)
            End If
            'Agrega a detalle de la factura.
            Dim Row1 As DataRow = DtDetalle.NewRow
            Row1("Articulo") = Row("Articulo")
            Row1("Cantidad") = Cantidad
            Row1("Precio") = Precio
            Row1("TipoPrecio") = TipoPrecio
            Row1("Devueltas") = 0
            Row1("Descuento") = 0
            DtDetalle.Rows.Add(Row1)
            Dim Index As Integer = DtDetalle.Rows.Count - 1
            Grid.Rows(Index).Cells("PrecioLista").Value = Precio
        Next

        Return True

    End Function
    Private Function ControlaParametrosPedido(ByVal Pedido As Integer) As Boolean

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT PorUnidad,Final FROM PedidosCabeza WHERE Pedido = " & Pedido & ";", Conexion, Dt) Then Return False
        If Dt.Rows.Count = 0 Then
            MsgBox("Falta Pedido " & Pedido & " ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        PorUnidadEnLista = Dt.Rows(0).Item("PorUnidad")
        FinalEnLista = Dt.Rows(0).Item("Final")
        Dt.Dispose()

        If FinalEnLista Then
            RadioFinal.Checked = True
            RadioSinIva.Enabled = False
        Else
            RadioSinIva.Checked = True
            RadioFinal.Enabled = False
        End If

        Return True

    End Function
    Private Sub CalculaSubTotal()

        If PFactura <> 0 Then Exit Sub

        If RadioFinal.Checked And EmpresaOk() Then
            CalculaSubTotalPrecioFinal()
            Exit Sub
        End If

        Dim Precio As Decimal = 0
        Dim TotalNeto As Decimal = 0
        Dim TotalIva As Decimal = 0
        Dim PrecioLista As Decimal = 0
        Dim TotalNetoPrecioLista As Decimal = 0
        Dim TotalNetoPrecioListaB As Decimal = 0
        Dim TotalNetoPrecioListaN As Decimal = 0

        TotalB = 0
        TotalN = 0
        Total = 0
        TotalNetoPerc = 0

        For I As Integer = 0 To Grid.Rows.Count - 2
            If CDbl(TextDescuentoSobreFactura.Text) <> 0 Then Grid.Rows(I).Cells("Descuento").Value = CDec(TextDescuentoSobreFactura.Text)
            Precio = Grid.Rows(I).Cells("PrecioLista").Value - (Grid.Rows(I).Cells("PrecioLista").Value * Grid.Rows(I).Cells("Descuento").Value / 100)
            Precio = Trunca3(Precio)
            Grid.Rows(I).Cells("Precio").Value = Precio
            If Grid.Rows(I).Cells("TipoPrecio").Value = 0 Then Exit Sub
            If Grid.Rows(I).Cells("TipoPrecio").Value = 1 Then
                PrecioLista = Grid.Rows(I).Cells("PrecioLista").Value
            Else
                Precio = Trunca3(Grid.Rows(I).Cells("Precio").Value * Grid.Rows(I).Cells("KilosXUnidad").Value)
                PrecioLista = Trunca3(Grid.Rows(I).Cells("PrecioLista").Value * Grid.Rows(I).Cells("KilosXUnidad").Value)
            End If
            '
            If RadioFinal.Checked Then
                Grid.Rows(I).Cells("PrecioBlanco").Value = Precio * (CDbl(TextDirecto.Text) / 100) / (1 + Grid.Rows(I).Cells("Iva").Value / 100)
                Grid.Rows(I).Cells("PrecioNegro").Value = Precio * (1 - CDbl(TextDirecto.Text) / 100)
                Grid.Rows(I).Cells("PrecioListaB").Value = PrecioLista * (CDbl(TextDirecto.Text) / 100) / (1 + Grid.Rows(I).Cells("Iva").Value / 100)
                Grid.Rows(I).Cells("PrecioListaN").Value = PrecioLista * (1 - CDbl(TextDirecto.Text) / 100)
            Else
                Grid.Rows(I).Cells("PrecioBlanco").Value = Precio * (CDbl(TextDirecto.Text) / 100)
                Grid.Rows(I).Cells("PrecioNegro").Value = Precio * (1 - CDbl(TextDirecto.Text) / 100)
                Grid.Rows(I).Cells("PrecioListaB").Value = PrecioLista * (CDbl(TextDirecto.Text) / 100)
                Grid.Rows(I).Cells("PrecioListaN").Value = PrecioLista * (1 - CDbl(TextDirecto.Text) / 100)
            End If
            Grid.Rows(I).Cells("PrecioBlanco").Value = Trunca3(Grid.Rows(I).Cells("PrecioBlanco").Value)
            Grid.Rows(I).Cells("PrecioNegro").Value = Trunca3(Grid.Rows(I).Cells("PrecioNegro").Value)
            Grid.Rows(I).Cells("PrecioListaB").Value = Trunca3(Grid.Rows(I).Cells("PrecioListaB").Value)
            Grid.Rows(I).Cells("PrecioListaN").Value = Trunca3(Grid.Rows(I).Cells("PrecioListaN").Value)
            '
            Grid.Rows(I).Cells("NetoBlanco").Value = CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioBlanco").Value)
            Grid.Rows(I).Cells("NetoNegro").Value = CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioNegro").Value)
            Grid.Rows(I).Cells("Neto").Value = CDec(Grid.Rows(I).Cells("NetoBlanco").Value) + CDec(Grid.Rows(I).Cells("NetoNegro").Value)
            TotalNeto = TotalNeto + Grid.Rows(I).Cells("Neto").Value
            '
            Grid.Rows(I).Cells("NetoPrecioListaB").Value = CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioListaB").Value)
            Grid.Rows(I).Cells("NetoPrecioListaN").Value = CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioListaN").Value)
            TotalNetoPrecioLista = TotalNetoPrecioLista + CDec(Grid.Rows(I).Cells("NetoPrecioListaB").Value) + CDec(Grid.Rows(I).Cells("NetoPrecioListaN").Value)
            '
            Grid.Rows(I).Cells("MontoIva").Value = CalculaIva(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioBlanco").Value, Grid.Rows(I).Cells("Iva").Value)
            '   TotalIva = TotalIva + CDec(Grid.Rows(I).Cells("MontoIva").Value)
            Grid.Rows(I).Cells("TotalArticulo").Value = Trunca(Grid.Rows(I).Cells("NetoBlanco").Value + Grid.Rows(I).Cells("NetoNegro").Value + Grid.Rows(I).Cells("MontoIva").Value)
            '            TotalB = TotalB + CDec(Grid.Rows(I).Cells("NetoBlanco").Value) + CDec(Grid.Rows(I).Cells("MontoIva").Value)
            TotalB = TotalB + CDec(Grid.Rows(I).Cells("NetoBlanco").Value)
            TotalN = TotalN + CDec(Grid.Rows(I).Cells("NetoNegro").Value)
            Total = TotalB + TotalN
        Next

        'Calcula Total Iva.     'Cambio en calculo de Iva. No se hace por linea delarticulo sino se grupa neto para cada iva y se calcula importe iva. Para compatibilizar con calculo de AFIP. 
        TotalIva = CalculaTotalIva(Grid)
        TotalNetoPerc = TotalB
        TotalB = TotalB + TotalIva
        Total = Total + TotalIva

        TextNetolPrecioLista.Text = FormatNumber(TotalNetoPrecioLista, GDecimales)
        TextDescuento.Text = FormatNumber(TotalNetoPrecioLista - TotalNeto, GDecimales)
        TextTotalNeto.Text = FormatNumber(TotalNeto, GDecimales)
        TextTotalIva.Text = FormatNumber(TotalIva, GDecimales)
        TextSubTotal.Text = FormatNumber(Total, GDecimales)

        SenaTotal = 0
        Bultos = 0
        TotalPercepciones = 0

        CalculaSenia(SenaTotal, Bultos)
        TotalPercepciones = CalculaPercepciones(TotalNetoPerc)

        TextSenia.Text = FormatNumber(SenaTotal, GDecimales)
        TextBultos.Text = FormatNumber(Bultos, GDecimales)
        TextPercepcion.Text = FormatNumber(TotalPercepciones, GDecimales)
        TextTotalFactura.Text = FormatNumber(Total + SenaTotal, GDecimales)
        TextTotalGeneral.Text = FormatNumber(Total + SenaTotal + TotalPercepciones, GDecimales)

        TotalGralB = TotalB + TotalPercepciones
        If CDec(TextAutorizar.Text) <> 0 Then
            TotalGralB = TotalGralB + SenaTotal
        End If

    End Sub
    Private Sub CalculaSubTotalPrecioFinal()

        If PFactura <> 0 Then Exit Sub

        Dim Precio As Decimal = 0
        Dim TotalNeto As Decimal = 0
        Dim TotalIva As Decimal = 0
        Dim PrecioLista As Decimal = 0
        Dim TotalNetoPrecioLista As Decimal = 0
        Dim TotalNetoPrecioListaB As Decimal = 0
        Dim TotalNetoPrecioListaN As Decimal = 0
        Dim TotalTotalArticuloB As Decimal = 0
        Dim TotalTotalArticuloN As Decimal = 0

        TotalB = 0
        TotalN = 0
        Total = 0
        TotalNetoPerc = 0

        For I As Integer = 0 To Grid.Rows.Count - 2
            If CDbl(TextDescuentoSobreFactura.Text) <> 0 Then Grid.Rows(I).Cells("Descuento").Value = CDec(TextDescuentoSobreFactura.Text)
            Precio = Grid.Rows(I).Cells("PrecioLista").Value - (Grid.Rows(I).Cells("PrecioLista").Value * Grid.Rows(I).Cells("Descuento").Value / 100)
            Precio = Trunca3(Precio)
            Grid.Rows(I).Cells("Precio").Value = Precio
            If Grid.Rows(I).Cells("TipoPrecio").Value = 0 Then Exit Sub
            If Grid.Rows(I).Cells("TipoPrecio").Value = 1 Then
                PrecioLista = Grid.Rows(I).Cells("PrecioLista").Value
            Else
                Precio = Trunca3(Grid.Rows(I).Cells("Precio").Value * Grid.Rows(I).Cells("KilosXUnidad").Value)
                PrecioLista = Trunca3(Grid.Rows(I).Cells("PrecioLista").Value * Grid.Rows(I).Cells("KilosXUnidad").Value)
            End If
            '
            Dim CoeficienteW As Decimal = 0
            Dim TotalArticulo As Decimal = 0
            Dim TotalArticuloB As Decimal = 0
            Dim TotalArticuloN As Decimal = 0
            Dim TotalArticuloLista As Decimal = 0
            Dim TotalArticuloListaB As Decimal = 0
            Dim TotalArticuloListaN As Decimal = 0
            Dim NetoB As Decimal = 0
            Dim NetoN As Decimal = 0
            Dim NetoListaB As Decimal = 0
            Dim NetoListaN As Decimal = 0
            Dim ImporteIva As Decimal = 0
            Dim PrecioUnitarioB As Decimal = 0
            Dim PrecioUnitarioN As Decimal = 0
            Dim PrecioUnitarioListaB As Decimal = 0
            Dim PrecioUnitarioListaN As Decimal = 0
            '
            If Grid.Rows(I).Cells("Cantidad").Value <> 0 Then
                CoeficienteW = 1 + CDec(Grid.Rows(I).Cells("Iva").Value) / 100
                ' Por precio.
                TotalArticulo = Trunca(Grid.Rows(I).Cells("Cantidad").Value * Precio)
                TotalArticuloB = Trunca(TotalArticulo * (CDbl(TextDirecto.Text) / 100))
                TotalArticuloN = TotalArticulo - TotalArticuloB
                NetoB = TotalArticuloB / CoeficienteW
                NetoN = TotalArticuloN
                ImporteIva = TotalArticuloB - NetoB
                PrecioUnitarioB = NetoB / Grid.Rows(I).Cells("Cantidad").Value
                PrecioUnitarioN = NetoN / Grid.Rows(I).Cells("Cantidad").Value
                'Por precio de lista.
                TotalArticuloLista = Trunca(Grid.Rows(I).Cells("Cantidad").Value * PrecioLista)
                TotalArticuloListaB = Trunca(TotalArticuloLista * (CDbl(TextDirecto.Text) / 100))
                TotalArticuloListaN = TotalArticuloLista - TotalArticuloListaB
                NetoListaB = TotalArticuloListaB / CoeficienteW
                NetoListaN = TotalArticuloListaN
                PrecioUnitarioListaB = NetoListaB / Grid.Rows(I).Cells("Cantidad").Value
                PrecioUnitarioListaN = NetoListaN / Grid.Rows(I).Cells("Cantidad").Value
            End If
            'Llena Grid.
            Grid.Rows(I).Cells("PrecioBlanco").Value = PrecioUnitarioB
            Grid.Rows(I).Cells("PrecioNegro").Value = PrecioUnitarioN
            Grid.Rows(I).Cells("PrecioListaB").Value = PrecioUnitarioListaB
            Grid.Rows(I).Cells("PrecioListaN").Value = PrecioUnitarioListaN
            '
            Grid.Rows(I).Cells("NetoBlanco").Value = NetoB
            Grid.Rows(I).Cells("NetoNegro").Value = NetoN
            Grid.Rows(I).Cells("Neto").Value = NetoB + NetoN
            TotalNeto = TotalNeto + NetoB + NetoN
            '
            Grid.Rows(I).Cells("NetoPrecioListaB").Value = NetoListaB
            Grid.Rows(I).Cells("NetoPrecioListaN").Value = NetoListaN
            TotalNetoPrecioLista = TotalNetoPrecioLista + NetoListaB + NetoListaN
            '
            Grid.Rows(I).Cells("MontoIva").Value = ImporteIva
            Grid.Rows(I).Cells("TotalArticulo").Value = NetoB + NetoN + ImporteIva
            TotalIva = TotalIva + ImporteIva
            TotalTotalArticuloB = TotalTotalArticuloB + TotalArticuloB
            TotalTotalArticuloN = TotalTotalArticuloN + TotalArticuloN
            TotalB = TotalB + NetoB
            TotalN = TotalN + NetoN
            TotalNetoPerc = TotalNetoPerc + NetoB
        Next

        '''      TotalB = Trunca(TotalB)
        ''''      TotalN = Trunca(TotalN)
        ''''      TotalIva = TotalIva

        ''''       TotalB = TotalB + TotalIva
        '''      Total = TotalB + TotalN

        'AREGLO ---------------------------------------------------------------

        ''''''  TotalIva = CalculaTotalIva(Grid)
        TotalIva = Trunca(TotalIva)

        '''''TotalB = Trunca(TotalB)
        '''''TotalN = Trunca(TotalN)

        '''      TotalB = TotalB + TotalIva
        '''      Total = TotalB + TotalN
        TotalB = TotalTotalArticuloB
        TotalN = TotalTotalArticuloN
        Total = TotalB + TotalN
        '''      Total = TotalB + TotalN
        '-----------------------------------------------------------------------

        TextNetolPrecioLista.Text = FormatNumber(TotalNetoPrecioLista, GDecimales)
        TextDescuento.Text = FormatNumber(TotalNetoPrecioLista - TotalNeto, GDecimales)
        TextTotalNeto.Text = FormatNumber(TotalNeto, GDecimales)
        TextTotalIva.Text = FormatNumber(TotalIva, GDecimales)
        TextSubTotal.Text = FormatNumber(Total, GDecimales)

        SenaTotal = 0
        Bultos = 0
        TotalPercepciones = 0

        CalculaSenia(SenaTotal, Bultos)
        TotalPercepciones = CalculaPercepciones(TotalNetoPerc)

        TextSenia.Text = FormatNumber(SenaTotal, GDecimales)
        TextBultos.Text = FormatNumber(Bultos, GDecimales)
        TextPercepcion.Text = FormatNumber(TotalPercepciones, GDecimales)
        TextTotalFactura.Text = FormatNumber(Total + SenaTotal, GDecimales)
        TextTotalGeneral.Text = FormatNumber(Total + SenaTotal + TotalPercepciones, GDecimales)

        TotalGralB = TotalB + TotalPercepciones
        If CDec(TextAutorizar.Text) <> 0 Then
            TotalGralB = TotalGralB + SenaTotal
        End If

    End Sub
    Private Sub CalculaSenia(ByRef TotalTotal As Decimal, ByRef Bultos As Decimal)

        If ComboTipoIva.SelectedValue = Exterior Or EsContable Then Exit Sub

        If CheckSeniaEnvase.Checked Then
            For i As Integer = 0 To Grid.Rows.Count - 2
                Dim Sena As Decimal = CalculaSena(Grid.Rows(i).Cells("Articulo").Value, DateTime1.Value)
                If Sena <> 0 Then
                    SenaTotal = SenaTotal + CalculaNeto(Grid.Rows(i).Cells("Cantidad").Value, Sena)
                    Bultos = Bultos + Grid.Rows(i).Cells("Cantidad").Value
                End If
            Next
        End If

        If CheckSeniaManual.Checked Then
            For i As Integer = 0 To Grid.Rows.Count - 2
                If Grid.Rows(i).Cells("Senia").Value <> 0 Then
                    SenaTotal = SenaTotal + CalculaNeto(Grid.Rows(i).Cells("Cantidad").Value, Grid.Rows(i).Cells("Senia").Value)
                    Bultos = Bultos + Grid.Rows(i).Cells("Cantidad").Value
                End If
            Next
        End If

    End Sub
    Private Sub HacerAlta()

        If LicenciaVencida(DateTime1.Value) Then End

        Dim DtCabezaB As New DataTable
        Dim DtDetalleB As New DataTable
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaCostoB As New DataTable
        Dim DtAsientoDetalleCostoB As New DataTable
        Dim DtFacturaPercepcionesB As New DataTable
        '
        Dim DtCabezaN As New DataTable
        Dim DtDetalleN As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable
        Dim DtAsientoCabezaCostoN As New DataTable
        Dim DtAsientoDetalleCostoN As New DataTable
        '
        Dim DiferenciaDescuentoB As Decimal = 0
        Dim DiferenciaDescuentoN As Decimal = 0
        '
        If CDbl(TextDirecto.Text) <> 0 Then
            DtCabezaB = DtCabeza.Clone
            DtDetalleB = DtDetalle.Clone
            DtDetalleB = DtDetalle.Copy
            DtDetalleB.Rows(0).Item("Descuento") = DtDetalle.Rows(0).Item("Descuento")  ' esta porque despues del dtdetalle.copy 'descuento' del primer registro lo pone en = 0 ????????
            DtFacturaPercepcionesB = DtFacturaPercepciones.Copy
            ArmaArchivoParaAlta(DtCabezaB, DtDetalleB, DtFacturaPercepcionesB, DiferenciaDescuentoB, "B")
            If Not FacturaAnteriorOK Then
                MsgBox("La Factura no se puede Grabar hasta que no autorice la Factura anterior por la AFIP. Operacion se CANCELA.", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If

        If CDbl(TextAutorizar.Text) <> 0 Then
            DtCabezaN = DtCabeza.Clone
            DtDetalleN = DtDetalle.Copy
            DtDetalleN.Rows(0).Item("Descuento") = DtDetalle.Rows(0).Item("Descuento")
            ArmaArchivoParaAlta(DtCabezaN, DtDetalleN, Nothing, DiferenciaDescuentoN, "N")
        End If

        If DtCabezaB.Rows.Count <> 0 And DtCabezaN.Rows.Count <> 0 Then
            DtCabezaB.Rows(0).Item("Rel") = True
            DtCabezaN.Rows(0).Item("Rel") = True
        End If
        If DtCabezaB.Rows.Count <> 0 And DtCabezaN.Rows.Count = 0 Then
            DtCabezaB.Rows(0).Item("Rel") = False
        End If
        If DtCabezaB.Rows.Count = 0 And DtCabezaN.Rows.Count <> 0 Then
            DtCabezaN.Rows(0).Item("Rel") = False
        End If

        'Arma Asignacion Lotes.
        '*******ARREGLO FRUTA MAX****************************************************************************************************************
        Dim DtLotesB As DataTable = DtAsignacionLotes.Clone
        Dim DtLotesN As DataTable = DtAsignacionLotes.Clone
        Dim DtStockB As New DataTable
        Dim DtStockN As New DataTable
        'Arma Asignacion Lotes.
        If ListaDeLotes.Count <> 0 Then   'Esto es para generar AsignacionLotes si es sobre remitos loteados o Articulos Loteados.
            If Not ArmaAsignacionFactura(2, DtCabezaB, DtDetalleB, DtCabezaN, DtDetalleN, DtLotesB, DtLotesN, ListaDeLotes, Nothing) Then Exit Sub
        End If
        'Arma Stock Lotes para articulos Loteados.
        If EsArticulosLoteados Then       'Para factura sobre remitos loteados no hace falta porque ya estan actualizado al asignar lotes en los remitos. 
            If Not ArmaStockFactura(DtAsignacionLotes, DtStockB, DtStockN, ListaDeLotes, Nothing) Then Exit Sub
        End If

        'Arma Asientos.
        If GGeneraAsiento Then
            Dim FechaW As Date
            If EsSecos Or EsfacturaElectronica Or EsContable Then
                FechaW = CDate(TextFechaContable.Text)
            Else : FechaW = CDate(DateTime1.Text)
            End If
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = -10;", Conexion, DtAsientoCabezaB) Then Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = -10;", Conexion, DtAsientoDetalleB) Then Exit Sub
            DtAsientoCabezaN = DtAsientoCabezaB.Clone
            DtAsientoDetalleN = DtAsientoDetalleB.Clone
            If Not ClaseLotesYAsientos.ArmaAsientosFacturasParaAlta(TipoAsiento, DtCabezaB, DtCabezaN, DtDetalleB, DtDetalleN, DtFacturaPercepcionesB,
                                 DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, DiferenciaDescuentoB, DiferenciaDescuentoN, Nothing, FechaW) Then Exit Sub
            If ListaDeLotes.Count <> 0 Then
                If Not ArmaAsientosFacturas("A", TipoAsiento, DtCabezaB, DtCabezaN, DtDetalleB, DtDetalleN, ListaDeLotes,
                                     DtAsignacionLotes, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, DiferenciaDescuentoB, DiferenciaDescuentoN, Nothing, FechaW) Then Exit Sub
            End If
            If Not (EsServicios Or EsSecos Or EsContable) Then
                DtAsientoCabezaCostoB = DtAsientoCabezaB.Clone
                DtAsientoDetalleCostoB = DtAsientoDetalleB.Clone
                DtAsientoCabezaCostoN = DtAsientoCabezaB.Clone
                DtAsientoDetalleCostoN = DtAsientoDetalleB.Clone
                If Not ClaseLotesYAsientos.ArmaAsientosCostosFacturasParaAlta(TipoAsientoCosto, DtCabezaB, DtCabezaN, DtDetalleB, DtDetalleN,
                                     DtAsientoCabezaCostoB, DtAsientoDetalleCostoB, DtAsientoCabezaCostoN, DtAsientoDetalleCostoN, Nothing, FechaW) Then Exit Sub
                If ListaDeLotes.Count <> 0 Then
                    If Not ArmaAsientosCostosFacturas("A", TipoAsientoCosto, DtCabezaB, DtCabezaN, DtDetalleB, DtDetalleN, ListaDeLotes,
                                  DtAsignacionLotes, DtAsientoCabezaCostoB, DtAsientoDetalleCostoB, DtAsientoCabezaCostoN, DtAsientoDetalleCostoN, Nothing, FechaW) Then Exit Sub
                End If
            End If
        End If

        Dim DtCabezaRemitoAux As DataTable
        DtCabezaRemitoAux = DtCabezaRemito.Copy
        Dim DtLotesRemito As DataTable
        DtLotesRemito = DtAsignacionLotesRemito.Copy
        Dim DtAsientoCabezaRemito As New DataTable
        Dim DtAsientosDetalleRemitos As New DataTable
        Dim DtDevolucionCabeza As New DataTable
        'modificacion para dejar sin asignacion remitos con lotes*************************************************
        If ListaRemitos.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = -10;", Conexion, DtAsientosDetalleRemitos) Then Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = -10;", Conexion, DtAsientoCabezaRemito) Then Exit Sub
            If Not ArmaArchivosParaRemitos(DtCabezaRemitoAux, DtDevolucionCabeza, DtAsientoCabezaRemito, DtAsientosDetalleRemitos, DtLotesRemito, ConexionRemito) Then Exit Sub
        End If

        'Graba Facturas.
        Dim NumeroFacturaN As Double = 0
        Dim NumeroAsientoB As Double
        Dim NumeroAsientoN As Double
        Dim NumeroAsientoCostoB As Double
        Dim NumeroAsientoCostoN As Double
        Dim Patron As String = GPuntoDeVenta & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Dim NumeroW As Double
        Dim InternoB As Double
        Dim InternoN As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                NumeroAsientoB = UltimaNumeracionAsiento(Conexion)
                If NumeroAsientoB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                NumeroAsientoCostoB = NumeroAsientoB + 1
            End If
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                NumeroAsientoN = UltimaNumeracionAsiento(ConexionN)
                If NumeroAsientoN < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                NumeroAsientoCostoN = NumeroAsientoN + 1
            End If
            'Halla ultima numeracion interna.
            If DtCabezaB.Rows.Count <> 0 Then
                InternoB = UltimoNumeroInternoFactura(TipoFactura, Conexion)
                If InternoB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                InternoN = UltimoNumeroInternoFactura(TipoFactura, ConexionN)
                If InternoN < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            'Halla Ultima numeracion Factura N.
            If DtCabezaN.Rows.Count <> 0 Then
                NumeroFacturaN = UltimaNumeracion(ConexionN)
                If NumeroFacturaN < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            'Halla Ultima numeracion Factura B Si es Ticket.
            If DtCabezaB.Rows.Count <> 0 And EsZ Then
                UltimoNumero = UltimaNumeracion(Conexion)
                If UltimoNumero < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If

            'Actualiza numeracion factura.
            If DtCabezaB.Rows.Count <> 0 Then
                'Cabeza.
                DtCabezaB.Rows(0).Item("Factura") = UltimoNumero
                DtCabezaB.Rows(0).Item("Interno") = InternoB
                DtCabezaB.Rows(0).Item("Relacionada") = 0
                'Detalle.
                For Each Row As DataRow In DtDetalleB.Rows
                    Row("Factura") = UltimoNumero
                Next
                For Each Row As DataRow In DtFacturaPercepcionesB.Rows
                    Row("Comprobante") = UltimoNumero
                Next
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                'Cabeza.
                DtCabezaN.Rows(0).Item("Factura") = NumeroFacturaN
                DtCabezaN.Rows(0).Item("Interno") = InternoN
                If DtCabezaB.Rows.Count <> 0 Then
                    DtCabezaN.Rows(0).Item("Relacionada") = UltimoNumero
                Else : DtCabezaN.Rows(0).Item("Relacionada") = 0
                End If
                DtCabezaN.Rows(0).Item("Recibo") = 0
                'Detalle.
                For Each Row As DataRow In DtDetalleN.Rows
                    Row("Factura") = NumeroFacturaN
                Next
            End If

            'Asignacion lotes.
            If DtLotesB.Rows.Count <> 0 Then
                For Each Row As DataRow In DtLotesB.Rows
                    Row("Comprobante") = UltimoNumero
                Next
            End If
            If DtLotesN.Rows.Count <> 0 Then
                For Each Row As DataRow In DtLotesN.Rows
                    Row("Comprobante") = NumeroFacturaN
                Next
            End If

            'Actualiza Asientos.
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                DtAsientoCabezaB.Rows(0).Item("Asiento") = NumeroAsientoB
                DtAsientoCabezaB.Rows(0).Item("Documento") = UltimoNumero
                For Each Row As DataRow In DtAsientoDetalleB.Rows
                    Row("Asiento") = NumeroAsientoB
                Next
            End If
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                DtAsientoCabezaN.Rows(0).Item("Asiento") = NumeroAsientoN
                DtAsientoCabezaN.Rows(0).Item("Documento") = NumeroFacturaN
                For Each Row As DataRow In DtAsientoDetalleN.Rows
                    Row("Asiento") = NumeroAsientoN
                Next
            End If
            If DtAsientoCabezaCostoB.Rows.Count <> 0 Then
                DtAsientoCabezaCostoB.Rows(0).Item("Asiento") = NumeroAsientoCostoB
                DtAsientoCabezaCostoB.Rows(0).Item("Documento") = UltimoNumero
                For Each Row As DataRow In DtAsientoDetalleCostoB.Rows
                    Row("Asiento") = NumeroAsientoCostoB
                Next
            End If
            If DtAsientoCabezaCostoN.Rows.Count <> 0 Then
                DtAsientoCabezaCostoN.Rows(0).Item("Asiento") = NumeroAsientoCostoN
                DtAsientoCabezaCostoN.Rows(0).Item("Documento") = NumeroFacturaN
                For Each Row As DataRow In DtAsientoDetalleCostoN.Rows
                    Row("Asiento") = NumeroAsientoCostoN
                Next
            End If

            'Actualiza Cabeza de Remitos con numero de factura. 
            If ListaRemitos.Count <> 0 Then
                Dim FacturaParaRemito As Double = 0
                If DtCabezaB.Rows.Count <> 0 Then
                    FacturaParaRemito = DtCabezaB.Rows(0).Item("Factura")
                Else
                    FacturaParaRemito = DtCabezaN.Rows(0).Item("Factura")
                End If
                For Each Row As DataRow In DtCabezaRemitoAux.Rows
                    Row("Factura") = FacturaParaRemito
                Next
            End If

            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            NumeroW = AltaFactura(DtCabezaB, DtDetalleB, DtCabezaN, DtDetalleN, DtLotesRemito, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN,
                           DtAsientoCabezaCostoB, DtAsientoDetalleCostoB, DtAsientoCabezaCostoN, DtAsientoDetalleCostoN, DtAsientoCabezaRemito, DtLotesB, DtLotesN, DtCabezaRemitoAux,
                           DtStockB, DtStockN, DtDevolucionCabeza, DtAsientosDetalleRemitos, DtFacturaPercepcionesB)

            Me.Cursor = System.Windows.Forms.Cursors.Default

            If NumeroW >= 0 Then Exit For
            If NumeroW = -2 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
            '----------------------------------------------------------
            'Caso en que la numeracion esta ya grabada busca la proxima.
            If NumeroW = -10 Then
                Dim Numeracion As Decimal = 0
                NumeracionAlternativa(2, GPuntoDeVenta, TipoFactura, UltimoNumero, EsfacturaElectronica, Numeracion)
                If Numeracion <> 0 Then
                    UltimoNumero = Numeracion
                Else
                    Exit For
                End If
            End If
            '-----------------------------------------------------------
        Next

        If NumeroW = -10 Then
            MsgBox("Factura Ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If NumeroW = -2 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If NumeroW > 0 Then
            PideAutorizacionAfip(DtCabezaB, DtDetalleB, DtFacturaPercepcionesB)          'Pide y graba CAE a la AFIP.----
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Remito = 0
            If DtCabezaB.Rows.Count <> 0 Then
                PFactura = DtCabezaB.Rows(0).Item("Factura")
                PAbierto = True
            Else : PFactura = DtCabezaN.Rows(0).Item("Factura")
                PAbierto = False
            End If
            GModificacionOk = True
            If Not MuestraDatos() Then Me.Close() : Exit Sub
        End If

    End Sub
    Private Sub ArmaArchivoParaAlta(ByVal DtCabeza As DataTable, ByVal DtDetalle As DataTable, ByVal DtFacturaPercepciones As DataTable, ByRef DiferenciaDescuento As Decimal, ByVal Tipo As String)

        DiferenciaDescuento = 0

        Dim Row As DataRow
        Dim Total As Decimal
        Dim SeniaW As Decimal
        Dim BultosW As Decimal

        If Tipo = "B" Then
            Total = TotalB
        Else : Total = TotalN
        End If

        'Arma Senas.
        If CDbl(TextAutorizar.Text) <> 0 And Tipo = "N" Then
            SeniaW = SenaTotal
            BultosW = Bultos
        End If
        If CDbl(TextAutorizar.Text) = 0 And Tipo = "B" Then
            SeniaW = SenaTotal
            BultosW = Bultos
        End If

        Dim RowsBusqueda() As DataRow
        Dim Descuento As Decimal = 0
        Dim TotalListaDePrecios As Decimal = 0

        'Prepara Detalle.
        For I As Integer = 0 To Grid.Rows.Count - 2
            RowsBusqueda = DtDetalle.Select("Indice = " & Grid.Rows(I).Cells("Indice").Value)
            If Tipo = "B" Then
                RowsBusqueda(0).Item("Iva") = Grid.Rows(I).Cells("Iva").Value
                RowsBusqueda(0).Item("Precio") = Grid.Rows(I).Cells("PrecioBlanco").Value
                RowsBusqueda(0).Item("PrecioLista") = Grid.Rows(I).Cells("PrecioListaB").Value
                RowsBusqueda(0).Item("TotalArticulo") = Trunca(Grid.Rows(I).Cells("NetoBlanco").Value + Grid.Rows(I).Cells("MontoIva").Value)
                If CDec(TextDirecto.Text) <> 100 Then RowsBusqueda(0).Item("Senia") = 0
                Descuento = Descuento + CDec(Grid.Rows(I).Cells("NetoPrecioListaB").Value) - CDec(Grid.Rows(I).Cells("NetoBlanco").Value)
                TotalListaDePrecios = TotalListaDePrecios + CDec(Grid.Rows(I).Cells("NetoPrecioListaB").Value)
            Else : RowsBusqueda(0).Item("Iva") = 0
                RowsBusqueda(0).Item("Precio") = Grid.Rows(I).Cells("PrecioNegro").Value
                RowsBusqueda(0).Item("PrecioLista") = Grid.Rows(I).Cells("PrecioListaN").Value
                RowsBusqueda(0).Item("TotalArticulo") = Grid.Rows(I).Cells("NetoNegro").Value
                Descuento = Descuento + CDec(Grid.Rows(I).Cells("NetoPrecioListaN").Value) - CDec(Grid.Rows(I).Cells("NetoNegro").Value)
                TotalListaDePrecios = TotalListaDePrecios + CDec(Grid.Rows(I).Cells("NetoPrecioListaN").Value)
            End If
            RowsBusqueda(0).Item("Devueltas") = 0
        Next

        'Corrige cuando el descuento no coincide (por los redondeos) con el descuento general de la factura. Solo para el cliente: Mercado Central.
        If GCuitEmpresa = MercadoCentral Then
            Dim DescuentoTotal As Decimal = 0
            If TieneDescuentoTotal(DescuentoTotal) Then
                DiferenciaDescuento = CalculaIva(1, TotalListaDePrecios, DescuentoTotal)
                DiferenciaDescuento = DiferenciaDescuento - Descuento
                Descuento = Descuento + DiferenciaDescuento
                Total = Total - DiferenciaDescuento  ' si la diferencia es negativa resta descuento pero aumenta total.
            End If
        End If

        'Prepara RecibosPercepciones
        TotalPercepciones = 0
        If Tipo = "B" Then
            TotalPercepciones = CalculaPercepciones(TotalNetoPerc)
            ArmaRecibosPercepciones(2, 0, ListaDePercepciones, DtFacturaPercepciones)
        End If

        Total = Total + SeniaW

        'Prepara Cabeza.
        Row = DtCabeza.NewRow
        InicializaRegistros.ArmaNuevaFactura(Row)
        Row("Factura") = 0
        Row("Fecha") = DateTime1.Value
        Row("Cliente") = PCliente
        Row("ClienteOperacion") = 0
        Row("Sucursal") = ComboSucursal.SelectedValue
        Row("Remito") = Remito
        Row("Deposito") = ComboDeposito.SelectedValue
        If GCuitEmpresa = GEdeal Then
            Row("FormaPago") = 5
        Else
            Row("FormaPago") = 2
        End If
        Row("FechaPago") = DateFechaPago.Value
        Row("TipoIva") = ComboTipoIva.SelectedValue
        If ListaDeLotes.Count = 0 Then
            Row("Estado") = 2
        Else : Row("Estado") = 1
        End If
        If EsServicios Or EsSecos Then Row("Estado") = 2
        Row("Recibo") = 0
        Row("Senia") = SeniaW
        Row("Bultos") = BultosW
        Row("Rel") = True
        Row("EsServicios") = EsServicios
        Row("EsSecos") = EsSecos
        Row("Relacionada") = 0
        Row("Final") = RadioFinal.Checked
        Row("PorUnidad") = False
        Row("Comentario") = TextComentario.Text.Trim
        Row("Importe") = Total
        Row("Percepciones") = TotalPercepciones
        Row("Saldo") = Total + TotalPercepciones
        Row("ImporteDev") = 0
        Row("Vendedor") = ComboVendedor.SelectedValue
        Row("Moneda") = ComboMoneda.SelectedValue
        Row("Cambio") = CDbl(TextCambio.Text)
        If TextFechaAfip.Text = "" Then
            Row("FechaElectronica") = "01/01/1800"
        Else : Row("FechaElectronica") = TextFechaAfip.Text
        End If
        If TextFechaContable.Text = "" Then
            Row("FechaContable") = "01/01/1800"
        Else : Row("FechaContable") = TextFechaContable.Text
        End If
        Row("Impreso") = False
        If PEsElectronica Then
            Row("EsExterior") = True
        Else : Row("EsExterior") = False
        End If
        Row("IncoTerm") = ComboIncoTerm.SelectedValue
        Row("Destino") = TextDestino.Text.Trim
        Row("Pedido") = Pedido
        Row("PedidoCliente") = TextPedidoCliente.Text
        Row("FechaEntrega") = DateEntrega.Value
        Row("Descuento") = Descuento
        If Tipo = "B" Then
            Row("EsElectronica") = EsfacturaElectronica
        Else
            Row("EsElectronica") = False
        End If
        Row("Cae") = 0
        Row("FechaCae") = 0
        Row("Confirmado") = False
        Row("Tr") = PEsContable
        'Para Factura Z.
        Row("EsZ") = EsZ
        Row("ComprobanteDesde") = ComprobanteDesde
        Row("ComprobanteHasta") = ComprobanteHasta
        If EsFCE And Tipo = "B" Then
            Row("EsFCE") = EsFCE
            Row("AgenteDeposito") = AgenteDeposito
        Else
            Row("EsFCE") = False
            Row("AgenteDeposito") = ""
        End If
        DtCabeza.Rows.Add(Row)

    End Sub
    Private Function AltaFactura(ByVal DtCabezaB As DataTable, ByVal DtDetalleB As DataTable, ByVal DtCabezaN As DataTable, ByVal DtDetalleN As DataTable, ByVal DtLotesRemito As DataTable, ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtAsientoCabezaN As DataTable, ByVal DtAsientoDetalleN As DataTable,
                       ByVal DtAsientoCabezaCostoB As DataTable, ByVal DtAsientoDetalleCostoB As DataTable, ByVal DtAsientoCabezaCostoN As DataTable, ByVal DtAsientoDetalleCostoN As DataTable, ByVal DtAsientoCabezaRemito As DataTable, ByVal DtLotesB As DataTable, ByVal DtLotesN As DataTable, ByVal DtCabezaRemitoAux As DataTable,
                       ByVal DtStockB As DataTable, ByVal DtStockN As DataTable, ByVal DtDevolucionCabeza As DataTable, ByVal DtAsientosDetalleRemitos As DataTable, ByVal DtFacturaPercepcionesB As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaB.GetChanges) And Not EsZ Then
                    If Not ReGrabaUltimaNumeracionFactura(DtCabezaB.Rows(0).Item("Factura"), TipoFactura) Then
                        If ComboTipoIva.SelectedValue <> Exterior Then Return -10
                    End If
                End If

                ' graba factura B.
                Resul = Grabafactura(DtCabezaB, DtDetalleB, DtFacturaPercepcionesB, Conexion)
                If Resul = -1 Then Return -10
                If Resul <= 0 Then Return Resul
                ' graba factura N.
                Resul = Grabafactura(DtCabezaN, DtDetalleN, New DataTable, ConexionN)
                If Resul <= 0 Then Return Resul

                ' graba Asiento B.
                If DtAsientoCabezaB.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaB.GetChanges, "AsientosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalleB.GetChanges, "AsientosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba Asiento N.
                If DtAsientoCabezaN.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalleN.GetChanges, "AsientosDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba Asiento costo B.
                If DtAsientoCabezaCostoB.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaCostoB.GetChanges, "AsientosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalleCostoB.GetChanges, "AsientosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba Asiento costo N.
                If DtAsientoCabezaCostoN.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaCostoN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalleCostoN.GetChanges, "AsientosDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba Asiento costo Remito.
                If DtAsientoCabezaRemito.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaRemito.GetChanges, "AsientosCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba asignacion B.
                If Not IsNothing(DtLotesB.GetChanges) Then
                    Resul = GrabaTabla(DtLotesB.GetChanges, "AsignacionLotes", Conexion)
                    If Resul <= 0 Then Return 0
                End If
                ' graba asignacion N.
                If Not IsNothing(DtLotesN.GetChanges) Then
                    Resul = GrabaTabla(DtLotesN.GetChanges, "AsignacionLotes", ConexionN)
                    If Resul <= 0 Then Return 0
                End If

                ' graba Stock B.
                If Not IsNothing(DtStockB.GetChanges) Then
                    Resul = GrabaTabla(DtStockB.GetChanges, "Lotes", Conexion)
                    If Resul <= 0 Then Return 0
                End If
                ' graba Stock N.
                If Not IsNothing(DtStockN.GetChanges) Then
                    Resul = GrabaTabla(DtStockN.GetChanges, "Lotes", ConexionN)
                    If Resul <= 0 Then Return 0
                End If

                ' Actualiza Remito.
                If ListaRemitos.Count <> 0 Then
                    Resul = ActualizaRemito(DtCabezaRemitoAux, DtLotesRemito, DtDevolucionCabeza, DtAsientosDetalleRemitos)
                    If Resul <= 0 Then Return 0
                End If

                'Actualiza Pedidos.
                If Not IsNothing(DtDetallePedido.GetChanges) Then
                    Resul = GrabaTabla(DtDetallePedido.GetChanges, "PedidosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        Finally
        End Try

    End Function
    Private Sub HacerModificacion()

        If Not PermisoTotal And DtCabeza.Rows(0).Item("Rel") Then
            MsgBox("Usuario No Autorizado(1000).")
            Exit Sub
        End If

        Dim DtCabezaW As DataTable = DtCabeza.Copy
        Dim DtCabezaRel As New DataTable

        If TextFechaAfip.Text <> "" Then
            If DtCabezaW.Rows(0).Item("FechaElectronica") <> CDate(TextFechaAfip.Text) Then DtCabezaW.Rows(0).Item("FechaElectronica") = CDate(TextFechaAfip.Text)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Factura = " & Relacionada & ";", ConexionRelacionada, DtCabezaRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Me.Close() : Exit Sub
        End If

        If FechaContableW <> CDate(TextFechaContable.Text) Then DtCabezaW.Rows(0).Item("FechaContable") = CDate(TextFechaContable.Text)

        If Relacionada <> 0 Then
            If DtCabezaRel.Rows(0).Item("FechaElectronica") <> DtCabezaW.Rows(0).Item("FechaElectronica") Then DtCabezaRel.Rows(0).Item("FechaElectronica") = DtCabezaW.Rows(0).Item("FechaElectronica")
            If DtCabezaRel.Rows(0).Item("FechaContable") <> DtCabezaW.Rows(0).Item("FechaContable") Then DtCabezaRel.Rows(0).Item("FechaContable") = DtCabezaW.Rows(0).Item("FechaContable")
            If DtCabezaRel.Rows(0).Item("Vendedor") <> DtCabezaW.Rows(0).Item("Vendedor") Then DtCabezaRel.Rows(0).Item("Vendedor") = DtCabezaW.Rows(0).Item("Vendedor")
            If DtCabezaRel.Rows(0).Item("IncoTerm") <> DtCabezaW.Rows(0).Item("IncoTerm") Then DtCabezaRel.Rows(0).Item("IncoTerm") = DtCabezaW.Rows(0).Item("IncoTerm")
            If DtCabezaRel.Rows(0).Item("PedidoCliente") <> DtCabezaW.Rows(0).Item("PedidoCliente") Then DtCabezaRel.Rows(0).Item("PedidoCliente") = DtCabezaW.Rows(0).Item("PedidoCliente")
            If DtCabezaRel.Rows(0).Item("Comentario") <> DtCabezaW.Rows(0).Item("Comentario") Then DtCabezaRel.Rows(0).Item("Comentario") = DtCabezaW.Rows(0).Item("Comentario")
            If DtCabezaRel.Rows(0).Item("Confirmado") <> DtCabezaW.Rows(0).Item("Confirmado") Then DtCabezaRel.Rows(0).Item("Confirmado") = DtCabezaW.Rows(0).Item("Confirmado")
            If DtCabezaRel.Rows(0).Item("Sucursal") <> DtCabezaW.Rows(0).Item("Sucursal") Then DtCabezaRel.Rows(0).Item("Sucursal") = DtCabezaW.Rows(0).Item("Sucursal")
            If DtCabezaRel.Rows(0).Item("FechaEntrega") <> DtCabezaW.Rows(0).Item("FechaEntrega") Then DtCabezaRel.Rows(0).Item("FechaEntrega") = DtCabezaW.Rows(0).Item("FechaEntrega")
        End If

        If IsNothing(DtCabezaW.GetChanges) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If


        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoCabezaRel As New DataTable
        Dim DtAsientoCabezaCosto As New DataTable
        Dim DtAsientoCabezaCostoRel As New DataTable
        'Modifica fecha contable en asientos si cambio.
        '
        If FechaContableW <> CDate(TextFechaContable.Text) Then
            If Not HallaAsientosCabeza(TipoAsiento, DtCabeza.Rows(0).Item("Factura"), DtAsientoCabeza, ConexionFactura) Then Me.Close() : Exit Sub
            If DtCabezaRel.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(TipoAsiento, DtCabezaRel.Rows(0).Item("Factura"), DtAsientoCabezaRel, ConexionRelacionada) Then Me.Close() : Exit Sub
            End If
            If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("IntFecha") = Format(CDate(TextFechaContable.Text), "yyyyMMdd")
            If DtAsientoCabezaRel.Rows.Count <> 0 Then DtAsientoCabezaRel.Rows(0).Item("IntFecha") = Format(CDate(TextFechaContable.Text), "yyyyMMdd")
            If Not HallaAsientosCabeza(TipoAsientoCosto, DtCabeza.Rows(0).Item("Factura"), DtAsientoCabezaCosto, ConexionFactura) Then Me.Close() : Exit Sub
            If DtCabezaRel.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(TipoAsientoCosto, DtCabezaRel.Rows(0).Item("Factura"), DtAsientoCabezaCostoRel, ConexionRelacionada) Then Me.Close() : Exit Sub
            End If
            If DtAsientoCabezaCosto.Rows.Count <> 0 Then DtAsientoCabezaCosto.Rows(0).Item("IntFecha") = Format(CDate(TextFechaContable.Text), "yyyyMMdd")
            If DtAsientoCabezaCostoRel.Rows.Count <> 0 Then DtAsientoCabezaCostoRel.Rows(0).Item("IntFecha") = Format(CDate(TextFechaContable.Text), "yyyyMMdd")
        End If
        '
        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaW.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaW.GetChanges, "FacturasCabeza", ConexionFactura)
                    If Resul < 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If Resul = 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If

                If Not IsNothing(DtCabezaRel.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaRel.GetChanges, "FacturasCabeza", ConexionRelacionada)
                    If Resul < 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If Resul = 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If

                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionFactura)
                    If Resul < 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If Resul = 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If

                If Not IsNothing(DtAsientoCabezaRel.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaRel.GetChanges, "AsientosCabeza", ConexionRelacionada)
                    If Resul < 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If Resul = 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If

                If Not IsNothing(DtAsientoCabezaCosto.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaCosto.GetChanges, "AsientosCabeza", ConexionFactura)
                    If Resul < 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If Resul = 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If

                If Not IsNothing(DtAsientoCabezaCostoRel.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaCostoRel.GetChanges, "AsientosCabeza", ConexionRelacionada)
                    If Resul < 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If Resul = 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If

                Scope.Complete()
                GModificacionOk = True
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            End Using
        Catch ex As TransactionException
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        Finally
        End Try

        If Not MuestraDatos() Then Me.Close()

    End Sub
    Private Function LeerAsientosParaModificacion(ByRef DtAsientoDetalle As DataTable, ByRef DtAsientoDetalleRel As DataTable, ByRef Asiento As Integer, ByRef AsientoRel As Integer) As Boolean

        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.Estado = 1 AND C.TipoDocumento = 2 AND C.Documento = " & Relacionada & ";", ConexionRelacionada, DtAsientoDetalleRel) Then Return False
            If DtAsientoDetalleRel.Rows.Count <> 0 Then AsientoRel = DtAsientoDetalleRel.Rows(0).Item("Asiento")
        End If

        If Not Tablas.Read("SELECT D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.Estado = 1 AND C.TipoDocumento = 2 AND C.Documento = " & DtCabeza.Rows(0).Item("Factura") & ";", ConexionFactura, DtAsientoDetalle) Then Return False
        If DtAsientoDetalle.Rows.Count <> 0 Then Asiento = DtAsientoDetalle.Rows(0).Item("Asiento")

        Return True

    End Function
    Private Function LeerAsientosParaModificacionCosto(ByRef DtAsientoDetalle As DataTable, ByRef DtAsientoDetalleRel As DataTable, ByRef Asiento As Integer, ByRef AsientoRel As Integer) As Boolean

        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.Estado = 1 AND C.TipoDocumento = 6070 AND C.Documento = " & Relacionada & ";", ConexionRelacionada, DtAsientoDetalleRel) Then Return False
            If DtAsientoDetalleRel.Rows.Count <> 0 Then AsientoRel = DtAsientoDetalleRel.Rows(0).Item("Asiento")
        End If

        If Not Tablas.Read("SELECT D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.Estado = 1 AND C.TipoDocumento = 6070 AND C.Documento = " & DtCabeza.Rows(0).Item("Factura") & ";", ConexionFactura, DtAsientoDetalle) Then Return False
        If DtAsientoDetalle.Rows.Count <> 0 Then Asiento = DtAsientoDetalle.Rows(0).Item("Asiento")

        Return True

    End Function
    Private Sub ArmaAsignacionLotes(ByVal DtDetalleWB As DataTable, ByVal DtLotesWB As DataTable, ByVal ComprobanteWB As Double, ByVal DtDetalleWN As DataTable, ByVal DtLotesWN As DataTable, ByVal ComprobanteWN As Double)

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow

        If DtDetalleWB.Rows.Count <> 0 Then
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Asignado <> 0 Then
                    Row = DtLotesWB.NewRow()
                    Row("TipoComprobante") = 2
                    Row("Comprobante") = ComprobanteWB
                    Row("Indice") = Fila.Indice
                    Row("Lote") = Fila.Lote
                    Row("Secuencia") = Fila.Secuencia
                    Row("Deposito") = Fila.Deposito
                    Row("Operacion") = Fila.Operacion
                    Row("Cantidad") = Fila.Asignado
                    Row("Rel") = False
                    RowsBusqueda = DtDetalleWB.Select("Indice = " & Fila.Indice)
                    Row("ImporteSinIva") = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                    Row("Importe") = Trunca(Row("ImporteSinIva") + CalculaIva(Fila.Asignado, RowsBusqueda(0).Item("Precio"), RowsBusqueda(0).Item("Iva")))
                    If ComboMoneda.SelectedValue <> 1 Then
                        Row("ImporteSinIva") = Trunca(Row("ImporteSinIva") * CDbl(TextCambio.Text))
                        Row("Importe") = Trunca(Row("Importe") * CDbl(TextCambio.Text))
                    End If
                    Row("Facturado") = False
                    Row("Liquidado") = False
                    DtLotesWB.Rows.Add(Row)
                End If
            Next
        End If

        If DtDetalleWN.Rows.Count <> 0 Then
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Asignado <> 0 Then
                    Row = DtLotesWN.NewRow()
                    Row("TipoComprobante") = 2
                    Row("Comprobante") = ComprobanteWN
                    Row("Indice") = Fila.Indice
                    Row("Lote") = Fila.Lote
                    Row("Secuencia") = Fila.Secuencia
                    Row("Deposito") = Fila.Deposito
                    Row("Operacion") = Fila.Operacion
                    Row("Cantidad") = Fila.Asignado
                    Row("Rel") = False
                    RowsBusqueda = DtDetalleWN.Select("Indice = " & Fila.Indice)
                    Row("ImporteSinIva") = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                    Row("Importe") = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                    If ComboMoneda.SelectedValue <> 1 Then
                        Row("ImporteSinIva") = Trunca(Row("ImporteSinIva") * CDbl(TextCambio.Text))
                        Row("Importe") = Trunca(Row("Importe") * CDbl(TextCambio.Text))
                    End If
                    Row("Facturado") = False
                    Row("Liquidado") = False
                    DtLotesWN.Rows.Add(Row)
                End If
            Next
        End If

        If DtLotesWB.Rows.Count <> 0 And DtLotesWN.Rows.Count <> 0 Then
            For Each Row In DtLotesWB.Rows
                Row("Rel") = True
            Next
            For Each Row In DtLotesWN.Rows
                Row("Rel") = True
            Next
        End If

    End Sub
    Private Function BuscaIndice(ByVal Indice As Integer) As Boolean

        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Indice = Indice Then Return True
        Next

        Return False

    End Function
    Private Function ActualizaRemito(ByVal DtRemitoW As DataTable, ByVal DtLotesW As DataTable, ByVal DtDevolucionCabezaRemitos As DataTable, ByVal DtAsientosDetalleRemitos As DataTable) As Double

        Dim Resul As Double = 0

        If Not IsNothing(DtRemitoW.GetChanges) Then
            Resul = GrabaTabla(DtRemitoW.GetChanges, "RemitosCabeza", ConexionRemito)
            If Resul <= 0 Then Return Resul
        End If
        If Not IsNothing(DtLotesW.GetChanges) Then
            Resul = GrabaTabla(DtLotesW.GetChanges, "AsignacionLotes", ConexionRemito)
            If Resul <= 0 Then Return Resul
        End If
        If Not IsNothing(DtDevolucionCabezaRemitos.GetChanges) Then
            Resul = GrabaTabla(DtDevolucionCabezaRemitos.GetChanges, "DevolucionCabeza", ConexionRemito)
            If Resul <= 0 Then Return Resul
        End If
        If Not IsNothing(DtAsientosDetalleRemitos.GetChanges) Then
            Resul = GrabaTabla(DtAsientosDetalleRemitos.GetChanges, "AsientosDetalle", ConexionRemito)
            If Resul <= 0 Then Return Resul
        End If

        Return 1000

    End Function
    Private Function Grabafactura(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtFacturaPercepcionesB As DataTable, ByVal ConexionStr As String) As Double

        Dim resul As Double = 0

        If Not IsNothing(DtCabezaW.GetChanges) Then
            resul = GrabaTabla(DtCabezaW.GetChanges, "FacturasCabeza", ConexionStr)
            If resul <= 0 Then Return resul
        End If

        If Not IsNothing(DtDetalleW.GetChanges) Then
            resul = GrabaTabla(DtDetalleW.GetChanges, "FacturasDetalle", ConexionStr)
            If resul <= 0 Then Return resul
        End If

        If Not IsNothing(DtFacturaPercepcionesB.GetChanges) Then
            resul = GrabaTabla(DtFacturaPercepcionesB.GetChanges, "Recibospercepciones", ConexionStr)
            If resul <= 0 Then Return resul
        End If

        Return 1000

    End Function
    Private Function CalculaSena(ByVal Articulo As Integer, ByVal Fecha As Date) As Double

        Dim Envase As Integer = HallaEnvase(Articulo)
        If Envase <= 0 Then
            MsgBox("ERROR al Leer Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return -1
        End If
        Dim Sena As Double
        If Not BuscaVigencia(10, Fecha, Sena, Envase) Then Return -1
        Return Sena

    End Function
    Private Function EsFacturaAnteriorOk(ByVal Factura As Decimal) As Boolean

        Dim Numero As Decimal = Strings.Right(Factura, 8)
        If Numero - 1 = 0 Then Return True

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT * From FacturasCabeza Where Factura = " & Factura - 1 & ";", Conexion, Dt) Then
            MsgBox("Error al leer Tabla: FacturasCabeza", MsgBoxStyle.Critical) : Me.Close() : Exit Function
        End If
        If Dt.Rows.Count = 0 Then  'Es una factura > 0 y es la pimera y esta informada en AFIP y no  grabada en el sistema".
            Dt.Dispose()
            Return True
        End If
        Dim Cae As Decimal = Dt.Rows(0).Item("Cae")
        Dt.Dispose()
        If Cae <> 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Private Function TieneRecibos() As Boolean

        If DtCabeza.Rows(0).Item("Recibo") = 0 Then Return False
        If Relacionada <> 0 Then
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT Recibo FROM FacturasCabeza WHERE Factura = " & Relacionada & ";", ConexionRelacionada, Dt) Then End
            If Dt.Rows(0).Item("Recibo") = 0 Then
                Dt.Dispose() : Return False
            Else
                Dt.Dispose() : Return True
            End If
        Else
            Return True
        End If

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Dim Patron As String = TipoFactura & Format(GPuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Factura) FROM FacturasCabeza WHERE CAST(CAST(FacturasCabeza.Factura AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(TipoFactura & Format(GPuntoDeVenta, "0000") & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaUltimaNumeracionW(ByVal TipoIva As Integer, ByVal PuntoVenta As Integer, ByVal ConexionStr As String) As Double

        Dim Patron As String = TipoIva & Format(PuntoVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Factura) FROM FacturasCabeza WHERE CAST(CAST(FacturasCabeza.Factura AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo)
                    Else : Return CDbl(TipoIva & Format(PuntoVenta, "0000") & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaUltimaNumeracion(ByVal TipoIva As Integer, ByVal PuntoVenta As Integer, ByVal ConexionStr As String) As Double

        Dim Patron As String = TipoIva & Format(PuntoVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Factura) FROM FacturasCabeza WHERE CAST(CAST(FacturasCabeza.Factura AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(TipoIva & Format(PuntoVenta, "0000") & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function UltimaNumeracionAsiento(ByVal ConexionStr) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Asiento) FROM AsientosCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function UltimoNumeroInternoFactura(ByVal TipoFacturaW As Integer, ByVal ConexionStr As String) As Double

        Dim Patron As String = TipoFacturaW & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Interno) FROM FacturasCabeza WHERE CAST(CAST(FacturasCabeza.Interno AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(TipoFacturaW & Format(1, "000000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaRelacionada(ByVal Factura As Double) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Factura FROM FacturasCabeza WHERE Relacionada = " & Factura & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaEstadoRecibo(ByVal Recibo As Double, ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Estado FROM RecibosCabeza WHERE TipoNota = 60 AND Nota = " & Recibo & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then Return CInt(Ultimo)
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error de Base de Datos.")
            End
        End Try

    End Function
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Dim Patron As String = TipoFactura & Format(GPuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM FacturasCabeza WHERE CAST(CAST(FacturasCabeza.Factura AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return "1/1/1000"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "2/1/1000"
        End Try

    End Function
    Private Function HallaIvaServicio(ByVal Servicio As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Iva FROM ArticulosServicios WHERE Clave = " & Servicio & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Private Function ReciboOk(ByRef DtRecibosCabeza As DataTable, ByVal Factura As Double, ByVal ConexionStr As String) As Integer

        Dim DtDetalle As New DataTable

        If Not Tablas.Read("SELECT Nota,Importe FROM RecibosDetalle WHERE TipoComprobante = 2 AND Comprobante = " & Factura & ";", ConexionStr, DtDetalle) Then
            DtDetalle.Dispose()
            MsgBox("Error Base de Datos. Operación se CANCELA.")
            Return -1
        End If
        If DtDetalle.Rows.Count <> 1 Then
            DtDetalle.Dispose()
            MsgBox("Factura Tiene Imputaciones en Cta.Cte. Operación se CANCELA.")
            Return -1
        End If
        If DtRecibosCabeza.Rows(0).Item("Importe") <> DtDetalle.Rows(0).Item("Importe") Then
            MsgBox("Recibo de Cobro Fue Modificado en Cta.Cte. Operación se CANCELA.")
            Return -1
        End If

        DtDetalle.Dispose()

        Return 0

    End Function
    Private Sub PideDatosEmisor()

        Dim PuntoDeVentaZ As Integer = 0

        If PEsElectronica Then OpcionFacturas.PEsSoloExportacion = True
        OpcionFacturas.PListaRemitos = ListaRemitos
        OpcionFacturas.PEsContable = PEsContable
        OpcionFacturas.PIgualCliente = IgualClienteW
        OpcionFacturas.PIgualDeposito = IgualDepositoW
        If PEsContable Then
            OpcionFacturas.PPuedeTenerFCE = False
        Else
            OpcionFacturas.PPuedeTenerFCE = True
        End If
        If PFactura = 0 Then OpcionFacturas.PEsSoloAltas = True
        OpcionFacturas.ShowDialog()
        If OpcionFacturas.PRegresar Then OpcionFacturas.Dispose() : PCliente = 0 : Exit Sub
        PCliente = OpcionFacturas.PCliente
        Deposito = OpcionFacturas.PDeposito
        If OpcionFacturas.PListaRemitos.Count <> 0 Then
            Remito = 1
        Else
            Remito = 0
        End If
        ListaRemitos = OpcionFacturas.PListaRemitos
        AbiertoRemito = OpcionFacturas.PAbiertoRemito
        Select Case OpcionFacturas.PEstadoRemito
            Case 1
                AsignadoEnRemito = True
            Case 2
                AsignadoEnRemito = False
        End Select
        EsServicios = OpcionFacturas.PEsServicios
        EsSecos = OpcionFacturas.PEsSecos
        EsArticulosLoteados = OpcionFacturas.PEsArticulosLoteados
        Pedido = OpcionFacturas.PPedido
        Sucursal = OpcionFacturas.PSucursal
        DtPedido = OpcionFacturas.PDtPedido
        Lista = OpcionFacturas.PLista
        FinalEnLista = OpcionFacturas.PFinalEnLista
        PorUnidadEnLista = OpcionFacturas.PPorUnidadEnLista
        FechaEntrega = OpcionFacturas.PFechaEntrega
        'Para FCE.
        EsFCE = OpcionFacturas.PEsFCE
        Minimo = OpcionFacturas.PMinimo
        CBU = OpcionFacturas.PCBU
        AgenteDeposito = OpcionFacturas.POPcionAgente
        'Para Facturas Z.
        If OpcionFacturas.PEsZ Then
            EsZ = True
            ComprobanteDesde = OpcionFacturas.PDesde
            ComprobanteHasta = OpcionFacturas.PHasta
            PuntoDeVentaZ = OpcionFacturas.PPuntoDeVenta
            EsTicket = OpcionFacturas.PEsTicket
            EsTicketBCAM = OpcionFacturas.PEsTicketBCAM
        End If

        OpcionFacturas.Dispose()

        If PCliente = 0 Then Exit Sub

        Dim TipoIvaCliente = HallaTipoIvaCliente(PCliente)
        TipoFactura = LetrasPermitidasCliente(TipoIvaCliente, 1)
        TextTipoFactura.Text = LetraTipoIva(TipoFactura)

        If EsZ Then
            If EsTicket Then TipoFactura = 9
            GPuntoDeVenta = PuntoDeVentaZ
        End If

        If Not EsZ Then
            If EsFCE Then
                GPuntoDeVenta = HallaPuntoVentaFce()
                If TextTipoFactura.Text = "M" Then
                    MsgBox("Letra Factura 'M' NO PERMITIDA para FCE.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3) : PCliente = 0
                End If
            Else
                If TipoFactura = 2 And TipoIvaCliente = 3 Then 'para separar puntode venta para excentos con consumdor final.
                    GPuntoDeVenta = HallaPuntoVentaSegunTipo(2, 3)
                Else
                    GPuntoDeVenta = HallaPuntoVentaSegunTipo(2, TipoFactura)
                End If
            End If
            If GPuntoDeVenta = 0 Then
                MsgBox("No esta definido Punto de Venta para  " & HallaCondicionIva(TipoFactura) & "  o para FCE.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PCliente = 0
            End If
            If Not ExistePuntoDeVenta(GPuntoDeVenta) Then
                MsgBox("Punto de Venta " & GPuntoDeVenta & " No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PCliente = 0
            End If
            If EsPuntoDeVentaZ(GPuntoDeVenta) Then
                MsgBox("Punto de Venta del Operador " & GPuntoDeVenta & " Reservado Para Ticket Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PCliente = 0
            End If
            Dim EsFCEW As Boolean = EsPuntoDeVentaCFE(GPuntoDeVenta)
            If EsFCE And Not EsFCEW Then
                MsgBox("Punto de Venta del Operador " & Format(GPuntoDeVenta, "0000") & " No es para FCE.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PCliente = 0
            End If
            If Not EsFCE And EsFCEW Then
                MsgBox("Punto de Venta del Operador " & Format(GPuntoDeVenta, "0000") & " Reservado para FCE.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PCliente = 0
            End If
        Else
            If Not ExistePuntoDeVenta(GPuntoDeVenta) Then
                MsgBox("Punto de Venta del Operador " & GPuntoDeVenta & " No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PCliente = 0
            End If
        End If

        LabelPuntodeVenta.Text = "Punto de Venta " & Format(GPuntoDeVenta, "0000")

    End Sub
    Private Function TieneDescuentoTotal(ByRef DescuentoTotal As Decimal) As Boolean

        DescuentoTotal = 0

        For I As Integer = 0 To DtDetalle.Rows.Count - 1
            If I = 0 Then
                DescuentoTotal = DtDetalle.Rows(I).Item("Descuento")
            Else
                If DescuentoTotal <> DtDetalle.Rows(I).Item("Descuento") Then Return False
            End If
        Next

        Return True

    End Function
    Private Function CalculaTotalIva(ByVal Grid As DataGridView) As Decimal

        Dim Total As Decimal = 0
        Dim ListaDeIva As New List(Of ItemIva)

        For I As Integer = 0 To Grid.Rows.Count - 2
            If Grid.Rows(I).Cells("Iva").Value <> 0 Then
                If PFactura = 0 Then
                    AgregaAListaDeIva(Grid.Rows(I).Cells("Iva").Value, ListaDeIva, Grid.Rows(I).Cells("NetoBlanco").Value)
                Else
                    AgregaAListaDeIva(Grid.Rows(I).Cells("Iva").Value, ListaDeIva, Grid.Rows(I).Cells("Neto").Value)
                End If
            End If
        Next

        For Each Fila As ItemIva In ListaDeIva
            Total = Total + CalculaIva(1, Fila.Importe, Fila.Iva)
        Next

        Return Total

    End Function
    Private Sub AgregaAListaDeIva(ByVal Iva As Decimal, ByRef ListaIva As List(Of ItemIva), ByVal Importe As Decimal)

        For Each Item As ItemIva In ListaIva
            If Item.Iva = Iva Then Item.Importe = Item.Importe + Importe : Exit Sub
        Next
        Dim ItemAdd As New ItemIva
        ItemAdd.Iva = Iva
        ItemAdd.Importe = Importe
        ListaIva.Add(ItemAdd)

    End Sub
    Private Function ArmaArchivosParaRemitos(ByRef DtRemitosCabeza As DataTable, ByRef DtDevolucionCabeza As DataTable, ByRef DtAsientosCabezaRemitos As DataTable, ByRef DtAsientosDetalleRemitos As DataTable, ByRef DtAsignacion As DataTable, ByVal ConexionStr As String) As Boolean

        For Each Row As DataRow In DtRemitosCabeza.Rows
            If Not Tablas.Read("SELECT * FROM DevolucionCabeza WHERE Estado = 1 AND Remito = " & Row("Remito") & ";", ConexionStr, DtDevolucionCabeza) Then Return False
            If Not Tablas.Read("SELECT A.* FROM DevolucionCabeza as C INNER JOIN AsignacionLotes AS A ON A.TipoComprobante = 3 AND A.Comprobante = C.Devolucion " &
                   " WHERE C.Estado = 1 AND C.Remito = " & Row("Remito") & ";", ConexionStr, DtAsignacion) Then Return False
        Next

        For Each Row As DataRow In DtRemitosCabeza.Rows
            Dim DtLotesAsignadosAux As New DataTable
            Dim DtRemitoDetalleW As New DataTable
            Dim DtAsientoCabezaW As New DataTable
            Dim DtAsientoDetalleW As New DataTable
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Estado <> 3 AND TipoDocumento = 6060 AND Documento = " & Row("Remito") & ";", ConexionStr, DtAsientoCabezaW) Then Return False
            If DtAsientoCabezaW.Rows.Count <> 0 Then
                If Not Tablas.Read("SELECT * FROM RemitosDetalle WHERE Remito = " & Row("Remito") & ";", ConexionStr, DtRemitoDetalleW) Then Return False
                If Not ReHaceAsientoDetalleRemito(Row("Remito"), DtLotesAsignadosAux, DtRemitoDetalleW, DtAsientoDetalleW, ConexionStr) Then Return False
                For Each Row1 As DataRow In DtAsientoDetalleW.Rows
                    DtAsientosDetalleRemitos.ImportRow(Row1)
                Next
                DtAsientosCabezaRemitos.ImportRow(DtAsientoCabezaW.Rows(0))
            End If
        Next

        For Each Row As DataRow In DtRemitosCabeza.Rows
            Row("Estado") = 2
        Next
        For Each Row As DataRow In DtDevolucionCabeza.Rows
            Row("Estado") = 2
        Next
        For Each Row As DataRow In DtAsignacion.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtAsientosCabezaRemitos.Rows
            Row("Estado") = 3
        Next

        Return True

    End Function
    Private Function AgregaListaPercepciones() As Boolean

        ListaDePercepciones = New List(Of ItemIvaReten)

        If Not AgregaPercepciones(ListaDePercepciones, 2, PCliente, Cuit, 2, DateTime1.Value) Then Return False

        Return True

    End Function
    Private Function CalculaPercepciones(ByVal Neto As Decimal) As Decimal

        Dim Total As Decimal = 0

        For Each Fila As ItemIvaReten In ListaDePercepciones
            Fila.Importe = 0
            If Fila.Formula = 4 Then
                Fila.Importe = CalculaIva(1, Neto, Fila.Alicuota)
                Total = Total + Fila.Importe
            End If
        Next

        Return Total

    End Function
    Private Function BuscaEnGrid(ByVal Articulo As Integer) As String

        For Each Row As DataGridViewRow In Grid.Rows
            If Not IsNothing(Row.Cells("Articulo").Value) Then
                If Row.Cells("Articulo").Value = Articulo Then Return Row.Cells("UMedida").Value : Exit Function
            End If
        Next

    End Function
    Public Function EstaReciboAnulado(ByVal Recibo As Decimal, ByVal ConexionW As String) As Boolean

        Dim Estado As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionW)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Estado FROM RecibosCabeza WHERE TipoNota = 60 AND Nota = " & Recibo & ";", Miconexion)
                    Estado = Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
        Finally
        End Try

        If Estado = 3 Then
            Return True
        Else
            Return False
        End If

    End Function
    Private Sub ButtonImportacionExcel_Click(sender As Object, e As EventArgs) Handles ButtonImportacionExcel.Click

        If PFactura <> 0 Then
            MsgBox("Opcion Invalida. Factura esta Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3, "ERROR!") : Exit Sub
        End If

        If ComboTipoIva.SelectedValue <> 3 Then
            MsgBox("EL Cliente no es Consumidor Final.!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR!") : Exit Sub
        End If

        RadioFinal.Checked = True

        OpcionDirectorio.ShowDialog()
        If OpcionDirectorio.PRegresar Then OpcionDirectorio.Dispose() : Exit Sub
        Dim Path As String = OpcionDirectorio.PPath
        Dim Archivo As String = OpcionDirectorio.PFile
        Dim Extencion As String = OpcionDirectorio.PExtencion
        OpcionDirectorio.Dispose()

        If Extencion <> ".xlsx" Then MsgBox("SELECCIONO UN ARCHIVO QUE NO ES EXCEL!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR!") : Exit Sub

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        Try
            exLibro = exApp.Workbooks.Open(Path)

            Dim I As Integer = 0
            Dim CantidadArticulosImportados As Integer = 0

            Dim CodigoArticulo As Integer
            Dim Cantidad As Integer
            Dim PrecioUnitario As Double
            Dim Descuento As Double
            Dim PrecioTotal As Double
            Dim NumeroFacturaExcel As String
            Dim Encontrado As Boolean

            Dim KilosXUnidad As Double
            Dim Iva As Double

            Dim RowDetalle As DataRow

            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            For Iterator As Integer = 1 To exLibro.Worksheets.Count
                exHoja = exLibro.Worksheets(Iterator)

                If Iterator = 1 Then
                    NumeroFacturaExcel = exHoja.Cells(1, 4).Value.ToString.Trim
                    If NumeroFacturaExcel.Substring(0, 7) <> "FACTURA" Then MsgBox("ARCHIVO DE IMPORTACION NO ES FACTURA!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR!") : Exit Sub

                    NumeroFacturaExcel = NumeroFacturaExcel.Substring(8, 1) & NumeroFacturaExcel.Substring(13, 13).Replace("-", "")
                    If NumeroFacturaExcel <> TextTipoFactura.Text & MaskedFactura.Text.Replace("-", "") Then MsgBox("ARCHIVO NO COINCIDE CON EL NUMERO DE FACTURA", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR") : Exit Sub
                End If

                Encontrado = False

                For I = 5 To 80
                    If Not Encontrado Then
                        If IsNothing(exHoja.Cells(I, 1).Value) Then Continue For
                        If exHoja.Cells(I, 1).Value.ToString.ToUpper = "CANTIDAD" Then Encontrado = True : Continue For
                        If IsNumeric(exHoja.Cells(I, 1).Value.ToString) Then Encontrado = True Else Continue For
                    End If
                    If exHoja.Cells(I, 1).Value.ToString = "" Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit For

                    Cantidad = exHoja.Cells(I, 1).Value
                    CodigoArticulo = exHoja.Cells(I, 2).Value
                    PrecioUnitario = exHoja.Cells(I, 5).Value
                    PrecioTotal = exHoja.Cells(I, 6).Value
                    Descuento = exHoja.Cells(I, 4).Value

                    If HallaNombreArticulo(CodigoArticulo) = "" Then MsgBox("ARTICULO CON CODIGO INTERNO " & CodigoArticulo & " NO EXISTE!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR!") : Continue For

                    HallaKilosIva(CodigoArticulo, KilosXUnidad, Iva)

                    RowDetalle = DtDetalle.NewRow
                    RowDetalle("Indice") = IndiceW
                    RowDetalle("Factura") = UltimoNumero
                    RowDetalle("Articulo") = CodigoArticulo
                    RowDetalle("KilosXUnidad") = KilosXUnidad
                    RowDetalle("Descuento") = Descuento
                    RowDetalle("Iva") = Iva
                    RowDetalle("TipoPrecio") = 1
                    RowDetalle("PrecioLista") = PrecioUnitario
                    RowDetalle("Cantidad") = Cantidad
                    RowDetalle("Devueltas") = 0
                    DtDetalle.Rows.Add(RowDetalle)

                    CantidadArticulosImportados += 1
                    If CantidadArticulosImportados = GLineasFacturas And Not PermiteMuchosArticulos Then
                        If MsgBox("Supera Cantidad Articulos Permitidos(" & GLineasFacturas & "). Si Continua No Podra Imprimirse. Desea Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                            PermiteMuchosArticulos = True
                        Else
                            Exit Try
                        End If
                    End If
                Next
            Next

        Catch ex As Exception
            Me.Cursor = System.Windows.Forms.Cursors.Default : MsgBox(ex.Message) : Exit Sub
        End Try

        ButtonImportacionExcel.Enabled = False

        CalculaSubTotal()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function Valida() As Boolean

        If TextSubTotal.Text = "" Then
            MsgBox("Debe Informar Articulos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If

        If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 And PFactura = 0 Then
            MsgBox("Fecha Incorrecta. Existe Otro Comprobante con Fecha " & UltimaFechaW & " Posterior a la Informada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If
        If PCliente = 0 Then
            MsgBox("Falta Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCliente.Focus()
            Return False
        End If
        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Return False
        End If
        If Grid.Rows.Count = 1 Then
            MsgBox("Falta Informar Articulos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        If PFactura = 0 And ListaRemitos.Count <> 0 And CDbl(TextDirecto.Text) = 0 And AbiertoRemito Then
            MsgBox("Factura no puede ser 100 % N.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If CtaCteCerrada And PFactura = 0 Then
            If MsgBox("Cuenta Cte. esta Cerrada. Desea Continuar Con la Factura?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Return False
            End If
        End If

        If TextCambio.Text = "" Then
            MsgBox("Falta Informar Cambio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If
        If ComboMoneda.SelectedValue <> 1 And CDbl(TextCambio.Text) = 0 Then
            MsgBox("Falta Informar Cambio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If
        If ComboMoneda.SelectedValue = 1 And CDbl(TextCambio.Text) <> 1 Then
            MsgBox("Error Cambio de Moneda Local debe ser 1.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If

        If ComboTipoIva.SelectedValue = Exterior Then
            If TextFechaAfip.Text = "" Then
                MsgBox("Falta Informar Fecha AFIP.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaAfip.Focus()
                Return False
            End If
            If Not ConsisteFecha(TextFechaAfip.Text) Then
                MsgBox("Fecha AFIP Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaAfip.Focus()
                Return False
            End If
            If DiferenciaDias(TextFechaAfip.Text, Date.Now) < 0 Then
                MsgBox("Fecha AFIP Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaAfip.Focus()
                Return False
            End If
        End If

        Dim Fecha As Date
        If EsSecos Then
            Fecha = DtCabeza.Rows(0).Item("FechaContable")
        Else
            If EsExterior Then
                Fecha = DtCabeza.Rows(0).Item("FechaElectronica")
            Else
                If EsfacturaElectronica Then
                    Fecha = DtCabeza.Rows(0).Item("FechaContable")
                Else
                    Fecha = DtCabeza.Rows(0).Item("Fecha")
                End If
            End If
        End If

        If CierreContableCerrado(Fecha.Month, Fecha.Year) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If EsSecos Or EsfacturaElectronica Or EsContable Or EsZ Then
            If TextFechaContable.Text = "" Then
                MsgBox("Falta Informar Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
            If Not ConsisteFecha(TextFechaContable.Text) Then
                MsgBox("Fecha Contable Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
            If DiferenciaDias(TextFechaContable.Text, Date.Now) < 0 Then
                MsgBox("Fecha Contable Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
            If DiferenciaDias(TextFechaContable.Text, UltimafechaContableW) > 0 Then
                MsgBox("Fecha Contable Menor a la Ultima Liquidación Informada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
            If CierreContableCerrado(CDate(TextFechaContable.Text).Month, CDate(TextFechaContable.Text).Year) Then
                MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
            If EsfacturaElectronica And DiferenciaDias(TextFechaContable.Text, DateTime1.Value) <> 0 Then
                If DateTime1.Value.Day > 10 Then
                    MsgBox("Fecha Contable menor a fecha actual solo puede ser informada del 1 al 10 del mes actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    TextFechaContable.Focus()
                    Return False
                End If
            End If
        End If

        For i As Integer = 0 To Grid.Rows.Count - 2
            If Not IsNumeric(Grid.Rows(i).Cells("Cantidad").Value) Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Grid.Rows(i).Cells("Cantidad").Value = 0 Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            '     If TieneDecimales(Grid.Rows(i).Cells("Cantidad").Value) And Not Grid.Rows(i).Cells("AGranel").Value Then
            If TieneDecimales(Grid.Rows(i).Cells("Cantidad").Value) And Not EsArticuloAGranel(Grid.Rows(i).Cells("Articulo").Value) Then
                MsgBox("Cantidad no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            If Not (EsServicios Or EsSecos) Then
                If Not IsNumeric(Grid.Rows(i).Cells("KilosXUnidad").Value) Then
                    MsgBox("Debe Informar Kilos por Unidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                If Grid.Rows(i).Cells("KilosXUnidad").Value = 0 Then
                    MsgBox("Debe Informar Kilos por Unidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If

            If Not IsNumeric(Grid.Rows(i).Cells("Precio").Value) Then
                MsgBox("Debe Informar Precio en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            If Grid.Rows(i).Cells("TipoPrecio").Value = 0 Then
                MsgBox("Debe Informar Si Precio es por Unidad o Por Kilo en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            If GCuitEmpresa <> GTux And GCuitEmpresa <> GPruebaEnTux Then
                If Grid.Rows(i).Cells("Precio").Value = 0 Then
                    MsgBox("Debe Informar Precio en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                If Grid.Rows(i).Cells("TotalArticulo").Value = 0 Then
                    MsgBox("Precio Debe Tener por lo menos 2 Digitos Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
        Next

        If ComboSucursal.SelectedValue <> 0 And Pedido <> 0 Then
            If ComboSucursal.SelectedValue <> Sucursal Then
                If MsgBox("Sucursal No Coincide con la del Pedido. Desea Continuar?.", MsgBoxStyle.YesNo, MsgBoxStyle.Question) = MsgBoxResult.No Then
                    Return False
                End If
            End If
        End If

        If EsZ Then
            If DiferenciaDias(DateTime1.Value, Date.Now) < 0 Then
                MsgBox("Fecha debe ser  Menor a la Fecha Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                DateTime1.Focus()
                Return False
            End If
            If TextComprobanteDesde.Text = "" Then
                MsgBox("Incorrecto Comprobante Desde.", MsgBoxStyle.Critical)
                Return False
            End If
            If CInt(TextComprobanteDesde.Text) = 0 Then
                MsgBox("Incorrecto Comprobante Desde.", MsgBoxStyle.Critical)
                Return False
            End If
            If TextComprobanteHasta.Text = "" Then
                MsgBox("Incorrecto Comprobante Hasta.", MsgBoxStyle.Critical)
                Return False
            End If
            If CInt(TextComprobanteHasta.Text) = 0 Then
                MsgBox("Incorrecto Comprobante Hasta.", MsgBoxStyle.Critical)
                Return False
            End If
            If CInt(TextComprobanteDesde.Text) > CInt(TextComprobanteHasta.Text) Then
                MsgBox("Incorrecto Comprobante Desde/Hasta.", MsgBoxStyle.Critical)
                Return False
            End If
            Dim Tipo As Integer
            Tipo = TipoFactura
            Dim PuntoDeVentaW As Integer = Strings.Mid(MaskedFactura.Text, 1, 4)
            Dim FacturasW As Integer = FacturasConComprobantesZ(PFactura, CInt(TextComprobanteDesde.Text), CInt(TextComprobanteHasta.Text), PuntoDeVentaW, Tipo)
            If FacturasW > 0 Then
                MsgBox("Existen Comprobante Informados dentro de Comprobante-Desde, Comprobante-Hasta.", MsgBoxStyle.Critical)
                Return False
            End If
        End If

        If EsFCE Then
            If Not ValidaFCE() Then Return False
        End If

        Return True

    End Function
    Private Function ValidaFCE() As Boolean

        If CBU = "" And PFactura = 0 Then
            MsgBox("Falta Informar CBU." + vbCrLf + "Informarlo en Menu-->Datos de la Empresa-->CBU.", MsgBoxStyle.Critical)
            Exit Function
        End If
        If DiferenciaDias(CDate(TextFechaContable.Text), DateFechaPago.Value) < 0 Then
            MsgBox("Fecha Pago Debe ser mayor o igual Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateFechaPago.Focus()
            Return False
        End If
        If DiferenciaDias(CDate(Date.Now), DateFechaPago.Value) < 0 Then
            MsgBox("Fecha Pago Debe ser mayor o igual a Fecha de Corriente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateFechaPago.Focus()
            Return False
        End If
        If TotalGralB = 0 And PFactura = 0 Then
            MsgBox("Factura FCE Tiene Importe = 0.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If TotalGralB < Minimo And PFactura = 0 Then
            MsgBox("Total Factura debe ser mayor o igual al MINIMO para FCE.(" & FormatNumber(Minimo, 2) & "$)." + vbCrLf +
            "Puede modificarlo en Menu-->Datos de la Empresa-->Importe Minimo Factura.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        'para manejo del autocoplete de articulos.
        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
            End If
        End If

    End Sub
    Private Sub Grid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.CellBeginEdit

        If Grid.Columns(e.ColumnIndex).Name = "Precio" Or Grid.Columns(e.ColumnIndex).Name = "KilosXUnidad" Or Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Articulo").Value) Then
                If Grid.CurrentRow.Cells("Articulo").Value = 0 Then e.Cancel = True
            End If
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        Dim Row As DataGridViewRow = Grid.CurrentRow

        'Tiene que ir delante que cualquier codigo que utilize la columna "Cantidad". De lo contrario no se actulizara correctamente.
        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then    'Recalcula Cantidad segun sea a granel.
            If IsDBNull(Row.Cells("Cantidad").Value) Then Row.Cells("Cantidad").Value = 0
            CalculaSubTotal()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "TipoPrecio" Then
            If PFactura = 0 Then CalculaSubTotal()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "PrecioLista" Then
            If IsDBNull(Row.Cells("PrecioLista").Value) Then Row.Cells("PrecioLista").Value = 0
            If ComboTipoPrecio.SelectedValue <> 0 And Row.Cells("TipoPrecio").Value = 0 And Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value <> 0 Then
                Grid.Rows(e.RowIndex).Cells("TipoPrecio").Value = ComboTipoPrecio.SelectedValue
            End If
            CalculaSubTotal()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Descuento" Then
            If IsDBNull(Row.Cells("Descuento").Value) Then Row.Cells("Descuento").Value = 0
            CalculaSubTotal()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "KilosXUnidad" Then
            If IsDBNull(Row.Cells("KilosXUnidad").Value) Then Row.Cells("KilosXUnidad").Value = 0
            If PFactura = 0 Then CalculaSubTotal()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Senia" Then
            If IsDBNull(Row.Cells("Senia").Value) Then Row.Cells("Senia").Value = 0
            If PFactura = 0 Then CalculaSubTotal()
        End If

        '     If Grid.Columns(e.ColumnIndex).Name = "TotalArticulo" Then
        '        If IsDBNull(Row.Cells("TotalArticulo").Value) Then Row.Cells("TotalArticulo").Value = 0
        '         Dim Unitario As Double = (Row.Cells("TotalArticulo").Value * (1 - Row.Cells("Iva").Value / 100)) / Row.Cells("Cantidad").Value
        '          Row.Cells("Precio").Value = Unitario
        '          Row.Cells("TotalArticulo").Value = Unitario * Row.Cells("Cantidad").Value * (1 + Row.Cells("Iva").Value / 100)
        '          CalculaSubTotal()
        '     End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
            Exit Sub
        End If

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "KilosXUnidad" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioLista" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Descuento" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Senia" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "TotalArticulo" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Senia" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioLista" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 3)
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Descuento" Then
            If CType(sender, TextBox).Text <> "" Then
                EsPorcentajeGridBox.Valida(CType(sender, TextBox))
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "KilosXUnidad" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 4)
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Grid.Rows(e.RowIndex).Cells("Articulo").Value <> 0 Then
                HallaAGranelYMedidaFactura(EsSecos, EsServicios, Grid.Rows(e.RowIndex).Cells("Articulo").Value, Grid.Rows(e.RowIndex).Cells("AGranel").Value, Grid.Rows(e.RowIndex).Cells("Medida").Value)
                Grid.Rows(e.RowIndex).Cells("UMedida").Value = HallaUMedidaArticulo(Grid.Rows(e.RowIndex).Cells("Articulo").Value)
            End If
        End If

        If IsNumeric(e.Value) Then
            If e.Value = 0 Then e.Value = Format(0, "#") : Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Grid.Rows(e.RowIndex).Cells("Articulo").Value = 0 Then Exit Sub
            If Not (EsServicios Or EsSecos) Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Indice").Value) And Not IsNothing(Grid.Rows(e.RowIndex).Cells("Indice").Value) Then
                    If BuscaIndice(Grid.Rows(e.RowIndex).Cells("Indice").Value) Then
                        e.Value = "  Asignados  " : e.CellStyle.ForeColor = Color.Green
                    Else
                        If HallaStock(Grid.Rows(e.RowIndex).Cells("Articulo").Value, ComboDeposito.SelectedValue) - Grid.Rows(e.RowIndex).Cells("Cantidad").Value < 0 Then
                            e.Value = "Insuf."
                        Else : e.Value = "No Asignado"
                        End If
                        e.CellStyle.ForeColor = Color.Red
                    End If
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Iva" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Iva").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Descuento" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Descuento").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Senia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Senia").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "PrecioLista" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("PrecioLista").Value) Then
                e.Value = FormatNumber(e.Value, 3)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Precio" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Precio").Value) Then
                e.Value = FormatNumber(e.Value, 3)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "MontoIva" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("MontoIva").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Neto" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Neto").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "TotalArticulo" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("TotalArticulo").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" And ComboEstado.SelectedValue <> 2 Then
            MuestraLotesAsignados.PArticulo = Grid.CurrentRow.Cells("Articulo").Value
            MuestraLotesAsignados.PIndice = CInt(Grid.CurrentRow.Cells("Indice").Value)
            MuestraLotesAsignados.PLista = ListaDeLotes
            MuestraLotesAsignados.ShowDialog()
            MuestraLotesAsignados.Dispose()
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtDetalle_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        InicializaRegistros.ArmaNuevaFacturaDetalle(e.Row)

        IndiceW = IndiceW + 1
        e.Row("Indice") = IndiceW
        e.Row("Factura") = UltimoNumero
        e.Row("TipoPrecio") = ComboTipoPrecio.SelectedValue

    End Sub
    Private Sub Dtdetalle_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Articulo")) Then
            If Not IsDBNull(e.Row("Articulo")) Then
                Dim Kilos As Double
                Dim Iva As Double
                Dim Precio As Double
                Dim TipoPrecio As Integer = 0
                If EsServicios Or EsSecos Then
                    Iva = HallaIvaServicio(e.ProposedValue)
                    If Iva < 0 Then
                        MsgBox("Error Base de Datos al leer Articulos Servicios.", MsgBoxStyle.Critical)
                        e.ProposedValue = e.Row("Articulo")
                        Grid.Refresh()
                        Exit Sub
                    End If
                Else
                    HallaKilosIva(e.ProposedValue, Kilos, Iva)
                End If
                If ComboTipoIva.SelectedValue = Exterior Then Iva = 0
                If PFactura = 0 And Lista <> 0 And Not EsServicios And Remito = 0 And Pedido = 0 And Not EsArticulosLoteados Then
                    Dim Final As Boolean
                    HallaPrecioDeListaSegunArticuloConTipoPrecio(Lista, DateEntrega.Value, e.ProposedValue, Precio, TipoPrecio, Final)
                    e.Row("PrecioLista") = Precio
                    e.Row("Precio") = Precio
                    e.Row("TipoPrecio") = TipoPrecio
                End If
                e.Row("KilosXUnidad") = Kilos
                e.Row("Iva") = Iva
                Grid.Refresh()
            End If
        End If

        If e.Column.ColumnName.Equals("TipoPrecio") Then
            If IsNothing(e.ProposedValue) Then e.ProposedValue = 0
        End If

    End Sub
    Private Sub Dtdetalle_ColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Articulo")) Then
            If EsArticulosLoteados Then Exit Sub
            If e.ProposedValue <> 0 Then
                If TieneCodigoCliente Then
                    Dim Codigo As String = HallaCodigoCliente(PCliente, e.ProposedValue)
                    If IsNothing(Codigo) Then Codigo = "-1"
                    If Codigo = "-1" Then
                        MsgBox("Articulo No Tiene Codigo Cliente.", MsgBoxStyle.Information)
                        Dim Row As DataRowView = bs.Current
                        Row.Delete()
                    End If
                End If
                If DtDetalle.Rows.Count + 1 > GLineasFacturas And Not PermiteMuchosArticulos And Not EsfacturaElectronica And Not EsZ Then
                    MsgBox("Supera Cantidad Articulos Permitidos.(" & GLineasFacturas & ")", MsgBoxStyle.Information)
                    Dim Row As DataRowView = bs.Current
                    Row.Delete()
                End If
            End If
        End If

    End Sub
    Private Sub DtDetalle_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        'Borra las lineas en blancos que aparecen cuando hago click en el ultimo renglon y sin informar nada regreso a algun renglon anterior. 
        If e.Row("Articulo") = 0 And e.Row("Cantidad") = 0 And e.Row("Precio") = 0 Then e.Row.Delete() : Exit Sub

    End Sub


End Class