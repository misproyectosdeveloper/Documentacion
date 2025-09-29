Imports System.Transactions
Imports System.Drawing.Printing
Imports ClassPassWord
Public Class UnRemito
    'http://www.xmlfox.com/datagridview.htm datatime en datagrid.
    Public PRemito As Double
    Public PAbierto As Boolean
    Public PCliente As Integer
    Public PDeposito As Integer
    ' Variables que bienen de UnIngresoMercaderias--
    Public PIngreso As Integer
    Public PDepositoIngreso As Integer
    Public PAbiertoIngreso As Boolean
    Public PDtIngreso As DataTable
    '-----------------------------------------------
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Private DtDetalle As DataTable
    Private DtCabeza As DataTable
    Private DtAsignacionLotes As DataTable
    Private DtDetallePedido As DataTable
    Private DtIngresoMercaderiasCabeza As DataTable
    Private DtPedido As DataTable
    Private DtDetalleAnt As DataTable
    '
    Dim ListaDeLotes As List(Of FilaAsignacion)
    ' Datios por Cuenta Y Orden.
    Dim PorCuentaYOrden As Integer
    Dim SucursalPorCuentaYOrden As Integer
    '-------------------------------------------------------------
    Dim PermiteMuchosArticulos As Boolean
    Dim EsAlta As Boolean
    Dim ConexionRemito As String
    Dim ConexionFactura As String
    Dim ConexionFacturaRelacionada As String
    Dim IndiceW As Integer
    Dim cb As ComboBox
    Dim UltimoNumero As Double
    Dim UltimaFechaW As DateTime
    Dim Directo As Double
    Dim TieneCodigoCliente As Boolean
    Dim Lista As Integer = 0
    Dim FinalEnLista As Boolean
    Dim PorUnidadEnLista As Boolean
    Dim Pedido As Integer
    Dim Sucursal As Integer
    Dim EsManual As Boolean
    Dim EsArticulosLoteados As Boolean
    Dim EsAutoImpreso As Boolean
    Dim Cai As Decimal
    Dim IntFechaCai As Integer
    Dim IgualClienteW As Integer
    Dim IgualDepositoW As Integer
    Dim FechaEntregaAnt As Date
    Dim CantidadPorBulto As Integer
    'Para impresion.
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim CopiasSegunPuntoVenta As Integer = 0
    Dim UltimoPuntoVentaParaCopiaSeleccionado As Integer = 0
    Dim ContadorCopias As Integer = 0
    Dim PuedeModificarPrecios As Boolean
    'Para impresora
    Dim Alto As Integer
    Dim mRow As Integer = 0
    Dim newpage As Boolean = True
    Dim ContadorPaginas As Integer = 0
    Dim TotalPaginas As Integer = 0

    'Datos.
    Dim Calle As String
    Dim Numero As String
    Dim Localidad As String
    Dim Cuit As String
    Private Sub UnRemito_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Para copiar datagridview al clipboard (portapapeles) poner en true MultiSelect del grid.  

        GModificacionOk = False

        If Not PermisoEscritura(5) Then
            PBloqueaFunciones = True
        Else
            If PermisoEscritura(500) Then PuedeModificarPrecios = True
        End If

        GPuntoDeVenta = HallaPuntoVentaSegunTipo(1, 0)
        If GPuntoDeVenta = 0 And PRemito = 0 Then
            MsgBox("Usuario No tiene definido Punto de Venta para Remito.")
            Me.Close()
            Exit Sub
        End If
        If Not ExistePuntoDeVenta(GPuntoDeVenta) Then
            MsgBox("Punto de Venta " & GPuntoDeVenta & " No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
            Exit Sub
        End If

        Grid.AutoGenerateColumns = False

        Pedido = 0
        Sucursal = 0
        PermiteMuchosArticulos = False
        EsArticulosLoteados = False

        If PRemito = 0 Then
            OpcionRemito.PIngreso = PIngreso
            OpcionRemito.PAbiertoIngreso = PAbiertoIngreso
            OpcionRemito.PDepositoIngreso = PDepositoIngreso
            If PIngreso = 0 Then
                OpcionRemito.PIgualCliente = IgualClienteW
                OpcionRemito.PIgualDeposito = IgualDepositoW
            End If
            If PRemito = 0 Then OpcionRemito.PEsSoloAltas = True
            OpcionRemito.ShowDialog()
            If OpcionRemito.PRegresar Then OpcionRemito.Dispose() : PCliente = 0 : Me.Close() : Exit Sub
            PAbierto = OpcionRemito.PAbierto
            PCliente = OpcionRemito.PCliente
            PDeposito = OpcionRemito.PDeposito
            Pedido = OpcionRemito.PPedido
            Sucursal = OpcionRemito.PSucursal
            DtPedido = OpcionRemito.PDtPedido
            Lista = OpcionRemito.PLista
            FinalEnLista = OpcionRemito.PFinalEnLista
            PorUnidadEnLista = OpcionRemito.PPorUnidadEnLista
            FechaEntrega.Value = OpcionRemito.PFechaEntrega
            PorCuentaYOrden = OpcionRemito.PPorCuentaYOrden
            EsArticulosLoteados = OpcionRemito.PEsArticulosLoteados
            SucursalPorCuentaYOrden = OpcionRemito.PSucursalPorCuentaYOrden
            EsManual = False
            If OpcionRemito.PPuntoDeVentaManual <> 0 Then GPuntoDeVenta = OpcionRemito.PPuntoDeVentaManual : EsManual = True
            OpcionRemito.Dispose()
            If PCliente = 0 Then Me.Close() : Exit Sub
        End If

        Dim EsZ As Boolean
        If PRemito = 0 Then
            DatosPuntoDeVentaParaRemito(GPuntoDeVenta, EsAutoImpreso, EsZ, Cai, IntFechaCai)
            If EsZ Then
                MsgBox("Punto de Venta del Operador " & GPuntoDeVenta & " Reservado Para Ticket Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
                Exit Sub
            End If
        Else
            Dim PV As Integer = Val(Strings.Mid(PRemito.ToString, 1, PRemito.ToString.Length - 8))
            DatosPuntoDeVentaParaRemito(PV, EsAutoImpreso, EsZ, Cai, IntFechaCai)
        End If
        If EsAutoImpreso And PAbierto And PRemito = 0 Then
            FechaCaiVencida()
        End If

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = PDeposito

        ArmaTipoIva(ComboTipoIva)

        LlenaCombo(ComboCliente, "", "Clientes")
        ComboCliente.SelectedValue = PCliente

        ComboEstado.DataSource = DtAfectaPendienteAnulada()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Codigo"

        ComboTransporte.DataSource = Nothing
        ComboTransporte.DataSource = Tablas.Leer("SELECT Nombre,Clave,Comentario FROM Tablas WHERE Tipo = 43;")
        Dim Row As DataRow = ComboTransporte.DataSource.NewRow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboTransporte.DataSource.Rows.Add(Row)
        ComboTransporte.DisplayMember = "Nombre"
        ComboTransporte.ValueMember = "Clave"
        ComboTransporte.SelectedValue = 0
        With ComboTransporte
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LabelPuntodeVenta.Text = "Punto de Venta    " & Format(GPuntoDeVenta, "0000")

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionRemito = Conexion
            ConexionFactura = Conexion
            ConexionFacturaRelacionada = ConexionN
            RadioPrecioConIva.Enabled = True
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionRemito = ConexionN
            ConexionFactura = ConexionN
            ConexionFacturaRelacionada = Conexion
            RadioPrecioConIva.Enabled = False
        End If

        MuestraDatos()

        ComboSucursal.DataSource = Nothing
        ComboSucursal.DataSource = Tablas.Leer("SELECT Nombre,Clave FROM SucursalesClientes WHERE  Cliente = " & DtCabeza.Rows(0).Item("Cliente") & ";")
        Row = ComboSucursal.DataSource.NewRow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboSucursal.DataSource.Rows.Add(Row)
        ComboSucursal.DisplayMember = "Nombre"
        ComboSucursal.ValueMember = "Clave"
        ComboSucursal.SelectedValue = DtCabeza.Rows(0).Item("Sucursal")
        With ComboSucursal
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        UnTextoParaRecibo.Dispose()
        FechaImpresion.Value = DateTime1.Value
        FechaEntregaImpresion.Value = FechaEntrega.Value

        UltimaFechaW = UltimaFecha(Conexion)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

        If EsArticulosLoteados Then ButtonArticulosEnStock_Click(Nothing, Nothing)

    End Sub
    Private Sub UnRemito_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()
        bs.EndEdit()

        If Not Valida() Then Exit Sub

        If PorCuentaYOrden <> DtCabeza.Rows(0).Item("PorCuentaYOrden") Then
            DtCabeza.Rows(0).Item("PorCuentaYOrden") = PorCuentaYOrden
        End If
        If SucursalPorCuentaYOrden <> DtCabeza.Rows(0).Item("SucursalPorCuentaYOrden") Then
            DtCabeza.Rows(0).Item("SucursalPorCuentaYOrden") = SucursalPorCuentaYOrden
        End If

        If IsNothing(DtCabeza.GetChanges) And IsNothing(DtDetalle.GetChanges) Then
            MsgBox("No hay Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Exit Sub
        End If

        If CheckConfirmado.Checked And Not IsNothing(DtDetalle.GetChanges) Then
            MsgBox("Remito fue Confirmado por el Cliente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtDetallePedido = New DataTable
        If EsAlta And DtCabeza.Rows(0).Item("Pedido") <> 0 Then
            If Not ActualizaDetallePedido("A", "R", DtCabeza.Rows(0).Item("Pedido"), DtDetalle, DtDetallePedido) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If EsAlta Then
            If HacerAlta() Then
                MuestraDatos()
            End If
        Else
            If HacerModificacion() Then MuestraDatos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PRemito = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Remito ya esta ANULADO. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Factura") <> 0 Then
            MsgBox("Remito Esta Facturado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not IsNothing(DtDetalle.GetChanges) And Not GModificacionOk Then
            MsgBox("Debe Grabar Modificaciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If HallaArticuloDeshabilitado(DtDetalle) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        ' 
        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(6060, DtCabeza.Rows(0).Item("Remito"), DtAsientoCabeza, ConexionRemito) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3
        End If

        If ComboEstado.SelectedValue = 1 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Debe Previamente Reingresar Lotes al Stock. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ExisteDevoluciones() <> "" Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Debe Previamente Anular Devoluciones: " & ExisteDevoluciones() & " .Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        DtDetallePedido = New DataTable
        If DtCabeza.Rows(0).Item("Pedido") <> 0 Then
            If Not ActualizaDetallePedido("B", "R", DtCabeza.Rows(0).Item("Pedido"), DtDetalle, DtDetallePedido) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If MsgBox("Remito se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtCabeza.Rows(0).Item("Estado") = 3

        If PIngreso <> 0 Then
            DtIngresoMercaderiasCabeza.Rows(0).Item("RemitoCliente") = 0
        End If

        Dim DtLotes As New DataTable
        Dim DtStockB As New DataTable
        Dim DtStockN As New DataTable

        Dim NumeroW As Double = ActualizaRemito("B", DtAsientoCabeza, DtAsientoDetalle, DtLotes, DtStockB, DtStockN)

        If NumeroW < 0 Then
            MsgBox("ERROR: En base de datos. Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("ERROR: Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            MsgBox("Remito Anulado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
        End If

        MuestraDatos()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PRemito = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue <> 3 Then
            MsgBox("Remito Debe estar ANULADO. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not EsRemitoManual(Val(Strings.Left(DtCabeza.Rows(0).Item("Remito").ToString, DtCabeza.Rows(0).Item("Remito").ToString.Length - 8))) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Función Valida Solo para Remitos Manuales. Operación se CANCELA.", MsgBoxStyle.Critical)
            Exit Sub
        End If

        If HallaArticuloDeshabilitado(DtDetalle) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If MsgBox("Remito se Borrara del Sistema. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        ' 
        If Not HallaAsientosCabeza(6060, DtCabeza.Rows(0).Item("Remito"), DtAsientoCabeza, ConexionRemito) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabeza.Rows(0).Item("Asiento") & ";", ConexionRemito, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        For Each Row As DataRow In DtAsientoCabeza.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtAsientoDetalle.Rows
            Row.Delete()
        Next
        DtCabeza.Rows(0).Delete()
        For Each Row As DataRow In DtDetalle.Rows
            Row.Delete()
        Next

        Dim NumeroW As Double = BorraRemito(DtCabeza, DtDetalle, DtAsientoCabeza, DtAsientoDetalle)

        If NumeroW < 0 Then
            MsgBox("ERROR: En base de datos. Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("ERROR: Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            MsgBox("Remito Borrado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close() : Exit Sub
        End If

        MuestraDatos()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonLiberarFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonLiberarFactura.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PRemito = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Remito ya esta ANULADO. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Factura") = 0 Then
            MsgBox("Remito No Esta Facturado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If (Not IsNothing(DtDetalle.GetChanges) Or Not IsNothing(DtCabeza.GetChanges)) And Not GModificacionOk Then
            MsgBox("Debe Grabar Modificaciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ConexionFactura = ConexionN And Not PermisoTotal Then
            MsgBox("ERROR Base de Datos (1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Factura As Decimal = DtCabeza.Rows(0).Item("Factura")

        Dim DtCabezaFactura As New DataTable
        Dim DtDetalleFactura As New DataTable
        Dim DtCabezaFacturaRel As New DataTable
        Dim DtDetalleFacturaRel As New DataTable
        If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Factura = " & Factura & ";", ConexionFactura, DtCabezaFactura) Then Exit Sub
        If Not Tablas.Read("SELECT * FROM FacturasDetalle WHERE Factura = " & Factura & ";", ConexionFactura, DtDetalleFactura) Then Exit Sub
        If DtCabezaFactura.Rows(0).Item("Rel") Then
            If ConexionFactura = Conexion And Not PermisoTotal Then
                MsgBox("ERROR Base de Datos (1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Relacionada = " & Factura & ";", ConexionFacturaRelacionada, DtCabezaFacturaRel) Then Exit Sub
            Dim Fac As Decimal = DtCabezaFacturaRel.Rows(0).Item("Factura")
            If Not Tablas.Read("SELECT * FROM FacturasDetalle WHERE Factura = " & Fac & ";", ConexionFacturaRelacionada, DtDetalleFacturaRel) Then Exit Sub
        End If

        For Each Row As DataRow In DtDetalleFactura.Rows
            If Row("Remito") = PRemito Then
                Row("Remito") = 0
                If Row("Cantidad") <> Row("Devueltas") Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Factura No Tiene Nota de Credito por el 100% de los Articulos que corresponden al Remito. Operación se CANCELA", MsgBoxStyle.Critical)
                    Exit Sub
                End If
            End If
        Next
        If DtCabezaFactura.Rows(0).Item("Rel") Then
            For Each Row As DataRow In DtDetalleFacturaRel.Rows
                If Row("Remito") = PRemito Then
                    Row("Remito") = 0
                End If
            Next
        End If

        DtCabezaFactura.Rows(0).Item("Remito") = 0
        For Each Row As DataRow In DtDetalleFactura.Rows
            If Row("Remito") <> 0 Then DtCabezaFactura.Rows(0).Item("Remito") = 1
        Next
        If DtCabezaFacturaRel.Rows.Count <> 0 Then DtCabezaFacturaRel.Rows(0).Item("Remito") = DtCabezaFactura.Rows(0).Item("Remito")

        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        DtCabezaAux.Rows(0).Item("Factura") = 0

        Dim DtAsientoCabezaRemito As New DataTable

        If Not HallaAsientosCabezaUltimo(6060, PRemito, DtAsientoCabezaRemito, ConexionRemito) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        If DtAsientoCabezaRemito.Rows.Count <> 0 Then DtAsientoCabezaRemito.Rows(0).Item("Estado") = 1

        If ActualizaRemitoYFactura(DtCabezaAux, DtAsientoCabezaRemito, DtCabezaFactura, DtCabezaFacturaRel, DtDetalleFactura, DtDetalleFacturaRel) Then
            MuestraDatos()
        End If

        DtAsientoCabezaRemito.Dispose()
        DtCabezaAux.Dispose()
        DtCabezaFactura.Dispose()
        DtDetalleFactura.Dispose()
        DtCabezaFacturaRel.Dispose()
        DtDetalleFacturaRel.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonActualizarPrecios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonActualizarPrecios.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PRemito = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Remito ya esta ANULADO. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not ModificaDetalleConLista() Then Exit Sub

    End Sub
    Private Sub ButtonReAsignaLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReAsignaLotes.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PRemito = 0 Then
            MsgBox("Remito debe ser Grabado Previamente. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Estado") = 3 Then
            MsgBox("Remito Esta Anulado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Factura") <> 0 Then
            MsgBox("Remito Ya esta Facturado. Asignación debe realizarce en la Factura. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If Not IsNothing(DtDetalle.GetChanges) And Not GModificacionOk Then
            MsgBox("Debe Grabar Modificaciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        GModificacionOk = False

        UnaReAsignacionRemito.PIngreso = DtCabeza.Rows(0).Item("Ingreso")
        UnaReAsignacionRemito.PRemito = PRemito
        UnaReAsignacionRemito.PAbierto = PAbierto
        UnaReAsignacionRemito.ShowDialog()
        If GModificacionOk Then MuestraDatos()
        UnaReAsignacionRemito.Dispose()

    End Sub
    Private Sub ButtonModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonModificar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PRemito = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Remito ya esta ANULADO. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Lista = -1 Then
            MsgBox("No se puede modificar articulos del remito porque falta lista de precio para este Cliente y Fecha Entrega.")
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Factura") <> 0 Then
            MsgBox("Remito Esta Facturado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ListaDeLotes.Count <> 0 Then
            MsgBox("Remito Loteado No Habilitado para esta Función. Operación se CANCELA.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Ingreso") <> 0 Then
            MsgBox("Remito Hecho sobre un Ingreso de Mercaderias No Habilitado para esta Función. Operación se CANCELA.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Impreso") Then
            If MsgBox("Remito Esta Impreso. Si Continua Este Comprobante No sera igual al entregado al Cliente. " + vbCrLf + "Desea Continuar(S/N)?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        Pedido = DtCabeza.Rows(0).Item("Pedido")
        If Pedido <> 0 Then
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT Sucursal FROM PedidosCabeza WHERE Pedido = " & Pedido & ";", Conexion, Dt) Then Exit Sub
            If Dt.Rows.Count = 0 Then
                If MsgBox("Pedido del Remito Ya No Existe. Desea Continuar(S/N)?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Dt.Dispose() : Exit Sub
                End If
            Else
                Sucursal = Dt.Rows(0).Item("Sucursal")
            End If
            Dt.Dispose()
        End If

        Grid.Columns("Articulo").ReadOnly = False
        Grid.Columns("Cantidad").ReadOnly = False
        ButtonNuevo.Enabled = False
        ButtonEliminarLinea.Visible = True

    End Sub
    Private Sub ButtonArticulosEnStock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonArticulosEnStock.Click

        Dim RowsBusqueda() As DataRow
        Dim DtAFacturar As DataTable
        DtAFacturar = CreaDtAFacturar()
        For Each Fila As FilaAsignacion In ListaDeLotes
            RowsBusqueda = DtDetalle.Select("Indice = " & Fila.Indice)    'Fila.Importe2 tiene la senia.
            AgregarADtAFacturar(DtAFacturar, Fila.Operacion, Fila.Lote, Fila.Secuencia, Fila.Asignado, 0, RowsBusqueda(0).Item("Articulo"), 0, RowsBusqueda(0).Item("KilosXUnidad"), RowsBusqueda(0).Item("TipoPrecio"), RowsBusqueda(0).Item("Precio"))
        Next

        UnaPreFactura.PEsRemito = True
        UnaPreFactura.PCliente = PCliente
        UnaPreFactura.PFechaEntrega = FechaEntrega.Value
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
            RowA("Devueltas") = 0
            RowA("Precio") = Row("Precio")
            RowA("TipoPrecio") = Row("TipoPrecio")
            DtDetalle.Rows.Add(RowA)
            Cantidad = Cantidad + 1
            If Cantidad = GLineasRemitos Then
                If MsgBox("Supera Cantidad Articulos Permitidos(" & GLineasRemitos & "). Si Continua No Podra Imprimirse. Desea Continuar?", MsgBoxStyle.YesNo, MsgBoxStyle.Question) = MsgBoxResult.No Then
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
            End If
        Next

        UnaPreFactura.Dispose()
        DtAFacturar.Dispose()

    End Sub
    Private Sub ButtonVerClientes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerClientes.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Factura") <> 0 Then
            MsgBox("Remito Ya Esta Facturado. Operación se CANCELA.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If Pedido <> 0 Then
            MsgBox("Remito fue descontado del Pedido = " & Pedido & " .Operación se CANCELA.", MsgBoxStyle.Information)
            Exit Sub
        End If

        OpcionClientes.PFechaEntrega = FechaEntrega.Value
        OpcionClientes.ShowDialog()
        If OpcionClientes.PRegresar Then OpcionClientes.Dispose() : Exit Sub

        If OpcionClientes.PEmisor <> PorCuentaYOrden And OpcionClientes.PEmisor = 0 Then
            If MsgBox("Cliente Por Cuenta Y Orden se Borrara. Desea Continuar? ", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                OpcionClientes.Dispose()
                Exit Sub
            End If
        End If

        If OpcionClientes.PEmisor = PorCuentaYOrden And OpcionClientes.PSucursal = SucursalPorCuentaYOrden Then
            OpcionClientes.Dispose()
            Exit Sub
        End If

        Dim SucursalWW As Integer
        Dim ClienteWW As Integer

        If OpcionClientes.PEmisor = 0 Then
            ClienteWW = PCliente
            SucursalWW = ComboSucursal.SelectedValue
        Else
            ClienteWW = OpcionClientes.PEmisor
            SucursalWW = OpcionClientes.PSucursal
        End If

        Dim FinalEnListaBak As Boolean
        FinalEnListaBak = FinalEnLista

        Dim ListaWW As Integer = HallaListaPrecios(ClienteWW, SucursalWW, FechaEntrega.Value, PorUnidadEnLista, FinalEnLista)
        If ListaWW = -1 Then
            FinalEnLista = FinalEnListaBak
            OpcionClientes.Dispose()
            Exit Sub
        End If

        If Not RecalculaPrecio(ListaWW, ClienteWW, SucursalWW, FechaEntrega.Value) Then
            FinalEnLista = FinalEnListaBak
            OpcionClientes.Dispose()
            Exit Sub
        End If

        TextPorCuentaYOrden.Text = OpcionClientes.PNombre
        TextSucursalPorCuentaYOrden.Text = OpcionClientes.PNombreSucursal
        PorCuentaYOrden = OpcionClientes.PEmisor
        SucursalPorCuentaYOrden = OpcionClientes.PSucursal

        Lista = ListaWW

        OpcionClientes.Dispose()

        LlenaCombosGrid()

    End Sub
    Private Sub ButtonImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PRemito <> 0 Then
            MsgBox("Remito Ya Esta Grabado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsArticulosLoteados Then
            MsgBox("Función No Permitida Para Articulos Loteados. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Pedido <> 0 Then
            MsgBox("Función No Permitida Bajo un Pedido. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        OpcionNumero.PEsImportacionRemito = True
        If Lista <> 0 Then OpcionNumero.PTieneListaDePrecios = True
        OpcionNumero.ShowDialog()
        If OpcionNumero.PRegresar Then OpcionNumero.Dispose() : Exit Sub
        Dim ConexionStr As String
        Dim Remito As Decimal = OpcionNumero.PRemito
        Dim ConCantidad As Boolean = OpcionNumero.PConCantidad
        Dim ConPrecios As Boolean = OpcionNumero.PConPrecios
        If OpcionNumero.PAbierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If
        OpcionNumero.Dispose()

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Articulo, (Cantidad - Devueltas) AS Cantidad,TipoPrecio,Precio,KilosXUnidad FROM RemitosDetalle WHERE Remito = " & Remito & ";", ConexionStr, Dt) Then Exit Sub
        If Dt.Rows.Count = 0 Then
            Dt.Dispose()
            MsgBox("Remito NO Existe. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Dt.Rows.Count > GLineasRemitos And Not EsAutoImpreso Then
            If MsgBox("Supera Cantidad Articulos Permitidos (" & GLineasRemitos & "). Si Continua No Podra Imprimirse. Desea Continuar?", MsgBoxStyle.YesNo, MsgBoxStyle.Question) = MsgBoxResult.No Then
                Dt.Dispose()
                Exit Sub
            Else
                PermiteMuchosArticulos = True
            End If
        End If

        'Consiste errores.
        For Each Row As DataRow In Dt.Rows
            Dim Cantidad As Decimal = 0
            Dim Precio As Decimal = 0
            Dim Codigo As String = ""
            Dim TipoPrecio As Integer = 0
            Dim ArticuloExisteEnPedido As Boolean = False
            If Not AnalizaPropiedadesArticulo(PCliente, Row("Articulo"), Row("KilosXUnidad"), FechaEntrega.Value, TieneCodigoCliente, Lista, Pedido, Cantidad, TipoPrecio, Precio, Codigo, ArticuloExisteEnPedido) Then Exit Sub
            If IsNothing(Codigo) Then Codigo = "-1"
            If TieneCodigoCliente And Codigo = "-1" Then
                MsgBox("Articulo " & NombreArticulo(Row("Articulo")) & " No Definido Código de Cliente. Operación se CANCELA.")
                Exit Sub
            End If
            If Lista <> 0 And Precio = 0 Then
                MsgBox("Articulo " & NombreArticulo(Row("Articulo")) & " No Definido En Lista de Precios. Operación se CANCELA.")
                Exit Sub
            End If
            If Pedido <> 0 And Not ArticuloExisteEnPedido Then
                MsgBox("Articulo " & NombreArticulo(Row("Articulo")) & " No Definido En Pedido. Operación se CANCELA.")
                Exit Sub
            End If
        Next

        For Each Row As DataRow In Dt.Rows
            Dim RowA As DataRow = DtDetalle.NewRow
            RowA("Articulo") = Row("Articulo")
            If ConCantidad Then RowA("Cantidad") = Row("Cantidad")
            If ConPrecios Then RowA("Precio") = Row("Precio") : RowA("TipoPrecio") = Row("TipoPrecio")
            DtDetalle.Rows.Add(RowA)
        Next

        CompletaGrid()

        Dt.Dispose()

    End Sub
    Private Sub FechaEntrega_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles FechaEntrega.Validating

        Dim PorUnidadEnListaW As Boolean
        Dim FinalEnListaW As Boolean

        If DiferenciaDias(FechaEntregaAnt, FechaEntrega.Value) <> 0 And PIngreso = 0 Then
            If DtCabeza.Rows(0).Item("Pedido") Then
                If Not ValidaFechaEntregaPedido() Then FechaEntrega.Value = FechaEntregaAnt : Exit Sub
            Else
                Dim ListaOriginal As Integer = HallaListaPrecios(PCliente, DtCabeza.Rows(0).Item("Sucursal"), FechaEntregaAnt, PorUnidadEnListaW, FinalEnListaW)
                If ListaOriginal = -1 Then Exit Sub
                Dim ListaActual = HallaListaPrecios(PCliente, DtCabeza.Rows(0).Item("Sucursal"), FechaEntrega.Value, PorUnidadEnListaW, FinalEnListaW)
                If ListaActual = -1 Then Exit Sub
                If ListaActual <> ListaOriginal Then
                    MsgBox("Los Precios se modificaran con la nueva lista de precios:  " & ListaActual)
                    If Not ModificaDetalleConLista() Then Exit Sub
                End If
            End If
        End If

    End Sub
    Private Function RecalculaPrecio(ByVal ListaWW As Integer, ByVal Cliente As Integer, ByVal Sucursal As Integer, ByVal FechaEntrega As Date) As Boolean

        Dim KilosXUNidad As Decimal
        Dim Cantidad As Decimal
        Dim Precio As Decimal

        If ListaWW = 0 Then
            For Each Row As DataRow In DtDetalle.Rows
                Row("Precio") = 0
            Next
            Return True
        End If

        Dim TipoPrecio As Integer

        For Each Row As DataRow In DtDetalle.Rows
            Dim Final As Boolean
            HallaPrecioDeListaSegunArticuloConTipoPrecio(ListaWW, FechaEntrega, Row("Articulo"), Precio, TipoPrecio, Final)
            If Precio = 0 Then
                MsgBox("Articulo " & NombreArticulo(Row("Articulo")) & " No Esta definido en Lista de Precios.", MsgBoxStyle.Information)
            Else
                If Not Final Then
                    Dim Iva As Double = HallaIva(Row("Articulo"))
                    Precio = Precio + CalculaIva(1, Precio, Iva)
                End If
                If Precio <> Row("Precio") Then Row("Precio") = Precio
                If TipoPrecio <> Row("TipoPrecio") Then Row("TipoPrecio") = TipoPrecio
            End If
        Next

        Return True

    End Function
    Private Sub ButtonDatosCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDatosCliente.Click

        If ComboCliente.SelectedValue = 0 Then Exit Sub

        UnDatosEmisor.PEsCliente = True
        UnDatosEmisor.PEmisor = ComboCliente.SelectedValue
        UnDatosEmisor.ShowDialog()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If (Not IsNothing(DtDetalle.GetChanges) Or Not IsNothing(DtCabeza.GetChanges)) And Not GModificacionOk Then
            MsgBox("Debe Grabar Modificaciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PRemito = 0
        PIngreso = 0
        PDtIngreso = Nothing
        IgualClienteW = 0
        IgualDepositoW = 0

        UnRemito_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonNuevoIgualCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevoIgualCliente.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If (Not IsNothing(DtDetalle.GetChanges) Or Not IsNothing(DtCabeza.GetChanges)) And Not GModificacionOk Then
            MsgBox("Debe Grabar Modificaciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PRemito = 0
        PIngreso = 0
        PDtIngreso = Nothing
        IgualClienteW = PCliente
        IgualDepositoW = PDeposito

        UnRemito_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PRemito = 0 Then Exit Sub

        If (Not IsNothing(DtDetalle.GetChanges) Or Not IsNothing(DtCabeza.GetChanges)) And Not GModificacionOk Then
            MsgBox("Debe Grabar Modificaciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ListaAsientos.PTipoDocumento = 6060
        If PAbierto Then
            ListaAsientos.PDocumentoB = PRemito
        Else
            ListaAsientos.PDocumentoN = PRemito
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonTextoRecibo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTextoRecibo.Click

        UnTextoParaRecibo.ShowDialog()

    End Sub
    Private Sub ButtonRefrescar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRefrescar.Click

        If Lista = 0 Then
            MsgBox("No tiene Lista de Precios.") : Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        LlenaCombosGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboTransporte_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTransporte.Validating

        If IsNothing(ComboTransporte.SelectedValue) Then ComboTransporte.SelectedValue = 0
        If ComboTransporte.SelectedValue = 0 Then Exit Sub

        Dim RowsBusqueda As DataRow()
        RowsBusqueda = ComboTransporte.DataSource.select("Clave = " & ComboTransporte.SelectedValue)
        If RowsBusqueda.Length <> 0 Then
            TextPatentes.Text = Strings.Left(RowsBusqueda(0).Item("Comentario"), 13)
        End If

    End Sub
    Private Sub ComboSucursal_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboSucursal.Validating

        If IsNothing(ComboSucursal.SelectedValue) Then ComboSucursal.SelectedValue = 0

        'Si tiene Pedido Cancela.
        If DtCabeza.Rows(0).Item("Pedido") <> 0 Then
            MsgBox("Existe un Pedido Asociado al Remito. Operación se CANCELA.", MsgBoxStyle.Critical)
            ComboSucursal.SelectedValue = DtCabeza.Rows(0).Item("Sucursal")
            Exit Sub
        End If

        ' Ve si tiene lista de precios con sucursal nueva.
        Dim PorUnidadEnLista As Boolean, FinalEnLista As Boolean
        Dim ListaSucursalNueva As Integer = HallaListaPrecios(PCliente, ComboSucursal.SelectedValue, DtCabeza.Rows(0).Item("FechaEntrega"), PorUnidadEnLista, FinalEnLista)
        Select Case ListaSucursalNueva
            Case -1
                ComboSucursal.SelectedValue = DtCabeza.Rows(0).Item("Sucursal")
            Case 0
            Case Else
                If MsgBox("Nueva Sucursal Tiene Lista de Precio " & ListaSucursalNueva & ". Si Continua debe 'Actualiza Precios con Lista' para Tomar los nuevos Precios. Desea Continua? (Si/No): ") = MsgBoxResult.No Then
                    ComboSucursal.SelectedValue = DtCabeza.Rows(0).Item("Sucursal")
                    Exit Sub
                End If
        End Select

        'Actualiza direccion.
        If ComboSucursal.SelectedValue <> 0 Then
            TextDestino.Text = HallaDireccionSucursalCliente(PCliente, ComboSucursal.SelectedValue)
            If TextDestino.Text = "-1" Then TextDestino.Text = ""
        Else
            TextDestino.Text = ""
        End If

    End Sub
    Private Sub MuestraDatos()              'MuestraDatos

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtCabeza = New DataTable
        Dim Sql As String = "SELECT * FROM RemitosCabeza WHERE Remito = " & PRemito & ";"
        If Not Tablas.Read(Sql, ConexionRemito, DtCabeza) Then Me.Close() : Exit Sub
        If PRemito <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Remito No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If DtCabeza.Rows.Count <> 0 Then
            If DtCabeza.Rows(0).Item("Factura") <> 0 Then
                Grid.Columns("KilosXUnidad").ReadOnly = True
            End If
        End If

        If Not MuestraCabeza() Then Me.Close() : Exit Sub

        'Poner despues de MuestraCabeza. 
        If PRemito <> 0 Then  'Halla Lista de precios.
            Lista = HallaListaPrecioCliente()
        End If

        PCliente = DtCabeza.Rows(0).Item("Cliente")
        PIngreso = DtCabeza.Rows(0).Item("Ingreso")
        FechaEntregaAnt = DtCabeza.Rows(0).Item("FechaEntrega")
        PorCuentaYOrden = DtCabeza.Rows(0).Item("PorCuentaYOrden")
        TextPorCuentaYOrden.Text = NombreCliente(PorCuentaYOrden)
        SucursalPorCuentaYOrden = DtCabeza.Rows(0).Item("SucursalPorCuentaYOrden")
        Pedido = DtCabeza.Rows(0).Item("Pedido")
        TextSucursalPorCuentaYOrden.Text = NombreSucursalCliente(PorCuentaYOrden, SucursalPorCuentaYOrden)
        If PRemito <> 0 Then
            If EsRemitoManual(Val(Strings.Left(DtCabeza.Rows(0).Item("Remito").ToString, DtCabeza.Rows(0).Item("Remito").ToString.Length - 8))) Then
                EsManual = True
            End If
        End If

        LlenaDatosCliente(DtCabeza.Rows(0).Item("Cliente"))

        If Lista > 0 Then
            If PuedeModificarPrecios Then
                Grid.Columns("Precio").ReadOnly = False
                Grid.Columns("TipoPrecio").ReadOnly = False
            Else
                Grid.Columns("Precio").ReadOnly = True
                Grid.Columns("TipoPrecio").ReadOnly = True
            End If
        End If

        DtDetalle = New DataTable
        Sql = "SELECT * FROM RemitosDetalle WHERE Remito = " & PRemito & ";"
        If Not Tablas.Read(Sql, ConexionRemito, DtDetalle) Then Me.Close() : Exit Sub

        'Arma tabla con AsignacionLotes. 
        DtAsignacionLotes = New DataTable
        ListaDeLotes = New List(Of FilaAsignacion)
        Sql = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 1 AND Comprobante = " & PRemito & ";"
        If Not Tablas.Read(Sql, ConexionRemito, DtAsignacionLotes) Then Me.Close() : Exit Sub
        For Each Row As DataRow In DtAsignacionLotes.Rows
            Dim Fila As New FilaAsignacion
            Fila.Indice = Row("Indice")
            Fila.Lote = Row("Lote")
            Fila.Secuencia = Row("Secuencia")
            Fila.Deposito = Row("Deposito")
            Fila.Operacion = Row("Operacion")
            Fila.Asignado = Row("Cantidad")
            'Muestra Permiso de Importacion.
            Fila.PermisoImp = HallaPermisoImp(Fila.Operacion, Fila.Lote, Fila.Secuencia, Fila.Deposito)
            If Fila.PermisoImp = "-1" Then
                Fila.PermisoImp = ""
            End If
            ListaDeLotes.Add(Fila)
        Next

        IndiceW = 0

        'arreglo para no permitir modificar row antiguas cunado agregar aericulos.
        DtDetalleAnt = DtDetalle.Copy
        For Each Row As DataRow In DtDetalleAnt.Rows   'Recupera ultimo Indice utilizado.
            IndiceW = Row("Indice")
        Next
        '--------------------------------------------------------------------------

        Grid.DataSource = Nothing
        Grid.DataSource = bs
        bs.DataSource = DtDetalle

        AddHandler DtDetalle.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtdetalle_ColumnChanging)
        AddHandler DtDetalle.ColumnChanged, New DataColumnChangeEventHandler(AddressOf Dtdetalle_ColumnChanged)
        AddHandler DtDetalle.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtDetalle_NewRow)
        AddHandler DtDetalle.RowChanged, New DataRowChangeEventHandler(AddressOf DtDetalle_RowChanged)

        If Pedido <> 0 And PRemito = 0 Then  'LLena detalle con pedidos.  
            For Each Row1 As DataRow In DtPedido.Rows
                Dim Row2 As DataRow = DtDetalle.NewRow
                Row2("Articulo") = Row1("Articulo")
                DtDetalle.Rows.Add(Row2)
            Next
        End If

        If PIngreso <> 0 And PRemito = 0 Then  'LLena detalle con ingreso. 
            Dim Row1 As DataRow
            Dim Contador As Integer = 0
            For Each Row1 In PDtIngreso.Rows
                Contador = Contador + 1
            Next
            If Contador > GLineasRemitos And Not PermiteMuchosArticulos Then
                If MsgBox("Articulos De Los Ingresos Supera numero de Articulos Permitidos para Remitos.(" & GLineasRemitos & "). Si Continua NO prodra Imprimirse. Desea Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    PermiteMuchosArticulos = True
                Else
                    Me.Close() : Exit Sub
                End If
            End If
            For Each Row1 In PDtIngreso.Rows
                Dim PrecioW As Decimal
                Dim TipoPrecioW As Decimal
                Dim CantidadW As Decimal
                If AnalizaArticuloConTipoPrecio(Lista, Row1("Articulo"), 1, CantidadW, PrecioW, TipoPrecioW) Then
                    Dim Row2 As DataRow = DtDetalle.NewRow
                    Row2("Articulo") = Row1("Articulo")
                    Row2("Cantidad") = Row1("Cantidad")
                    DtDetalle.Rows.Add(Row2)
                End If
            Next
        End If

        'Completa el grid con datos que no estan en DtDetalle.
        CompletaGrid()

        If PRemito <> 0 Then
            Grid.Columns("Articulo").ReadOnly = True
            Grid.Columns("Cantidad").ReadOnly = True
            ButtonEliminarLinea.Visible = False
            ButtonAceptar.Text = "Graba Cambios"
            LabelPuntodeVenta.Visible = False
            TextPorCuentaYOrden.ReadOnly = False
            TextSucursalPorCuentaYOrden.ReadOnly = False
            ButtonVerClientes.Enabled = True
            FechaEntrega.Enabled = True
            '  ComboSucursal.Enabled = True
        Else
            Grid.Columns("Articulo").ReadOnly = False
            Grid.Columns("Cantidad").ReadOnly = False
            ButtonEliminarLinea.Visible = True
            ButtonAceptar.Text = "Alta Remito"
            LabelPuntodeVenta.Visible = True
            TextPorCuentaYOrden.ReadOnly = True
            TextSucursalPorCuentaYOrden.ReadOnly = True
            ButtonVerClientes.Enabled = False
            FechaEntrega.Enabled = False
        End If

        If PIngreso <> 0 Then
            Grid.Columns("Articulo").ReadOnly = True
            Grid.Columns("Cantidad").ReadOnly = True
            ButtonEliminarLinea.Visible = False
            ButtonNuevo.Enabled = False
        End If

        If EsArticulosLoteados And PRemito = 0 Then
            ButtonArticulosEnStock.Visible = True
            Grid.Columns("Articulo").ReadOnly = True
            Grid.Columns("Cantidad").ReadOnly = True
            ButtonEliminarLinea.Enabled = False
        Else
            ButtonArticulosEnStock.Visible = False
        End If

        If EsManual And PRemito = 0 Then
            MaskedRemito.ReadOnly = False
        Else
            MaskedRemito.ReadOnly = True
        End If

        DtIngresoMercaderiasCabeza = New DataTable
        If PIngreso <> 0 Then
            If Not Tablas.Read("SELECT * FROM IngresoMercaderiasCabeza Where Lote = " & PIngreso & ";", ConexionRemito, DtIngresoMercaderiasCabeza) Then Me.Close() : Exit Sub
            If DtIngresoMercaderiasCabeza.Rows.Count = 0 Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Ingreso " & PIngreso & " Mercaderías No Encontrado.", MsgBoxStyle.Critical)
                Exit Sub
            End If
        End If

        If ComboEstado.SelectedValue = 3 Then Grid.Columns("KilosXUnidad").ReadOnly = True

        If ListaDeLotes.Count <> 0 Or EsArticulosLoteados Then
            ComboDeposito.Enabled = False
        Else
            ComboDeposito.Enabled = True
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function HacerAlta() As Boolean

        If LicenciaVencida(DateTime1.Value) Then End

        'Calcula Valor del remito.
        Dim Total As Decimal = 0
        Total = CalculaValor()
        If DtCabeza.Rows(0).Item("Valor") <> Total Then DtCabeza.Rows(0).Item("Valor") = Total

        For Each Row As DataRow In DtDetalle.Rows
            Row("Fecha") = DateTime1.Value
            Row("Devueltas") = 0
        Next

        DtCabeza.Rows(0).Item("Pedido") = Pedido
        DtCabeza.Rows(0).Item("Estado") = 2
        'viejo''''''''''''''''''''''''''''''''

        'Arma Asignacion Lotes.
        '*******ARREGLO FRUTA MAX****************************************************************************************************************
        Dim DtLotes As DataTable = DtAsignacionLotes.Clone
        Dim DtStockB As New DataTable
        Dim DtStockN As New DataTable
        If EsArticulosLoteados Then
            'Arma Asignacion Lotes.
            ArmaAsignacionRemitos(1, DtCabeza, DtLotes, ListaDeLotes)
            'Arma Stock Lotes para articulos Loteados.
            If Not ArmaStockFactura(DtAsignacionLotes, DtStockB, DtStockN, ListaDeLotes, Nothing) Then Exit Function
            DtCabeza.Rows(0).Item("Estado") = 1
        End If

        'Arma Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = -10;", Conexion, DtAsientoCabeza) Then Exit Function
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = -10;", Conexion, DtAsientoDetalle) Then Exit Function
            If Not ArmaAsientoRemito(6060, DtLotes, DtDetalle, DtAsientoCabeza, DtAsientoDetalle, DateTime1.Value) Then Return False
        End If

        'Graba Remito.
        Dim NumeroRemito As Double = 0
        Dim NumeroW As Double
        Dim NumeroAsiento As Double

        For i As Integer = 1 To 50
            If PAbierto Then
                If EsManual Then
                    NumeroRemito = MaskedRemito.Text
                Else
                    NumeroRemito = UltimoNumero
                End If
            Else
                NumeroRemito = UltimaNumeracion(ConexionN)
                If NumeroRemito < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If

            DtCabeza.Rows(0).Item("Remito") = NumeroRemito
            DtCabeza.Rows(0).Item("Interno") = UltimoNumeroInternoRemito(ConexionRemito)
            If DtCabeza.Rows(0).Item("Interno") < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            For Each Row As DataRow In DtDetalle.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Remito") = NumeroRemito
                End If
            Next

            'Halla Ultima numeracion Asiento B
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionRemito)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = NumeroRemito
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            'Actualiza Cabeza de Ingreso de Mercaderias.
            If PIngreso <> 0 Then
                DtIngresoMercaderiasCabeza.Rows(0).Item("RemitoCliente") = NumeroRemito
            End If

            'Asignacion lotes.
            If DtLotes.Rows.Count <> 0 Then
                For Each Row As DataRow In DtLotes.Rows
                    Row("Comprobante") = NumeroRemito
                Next
            End If

            NumeroW = ActualizaRemito("A", DtAsientoCabeza, DtAsientoDetalle, DtLotes, DtStockB, DtStockN)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -2 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
            '----------------------------------------------------------
            'Caso en que la numeracion esta ya grabada busca la proxima.
            If NumeroW = -10 Then
                Dim Numeracion As Decimal = 0
                NumeracionAlternativa(1, GPuntoDeVenta, 0, UltimoNumero, False, Numeracion)
                If Numeracion <> 0 Then
                    UltimoNumero = Numeracion
                Else
                    Exit For
                End If
            End If
            '-----------------------------------------------------------
        Next

        If NumeroW = -10 Then
            MsgBox("Factura Ya Fue Grabada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            PRemito = NumeroRemito
            CompruebaAlta(NumeroRemito, NumeroAsiento, ConexionRemito)
            Return True
        End If

    End Function
    Private Function HacerModificacion() As Boolean

        Dim Total As Decimal = 0
        Total = CalculaValor()
        If DtCabeza.Rows(0).Item("Valor") <> Total Then DtCabeza.Rows(0).Item("Valor") = Total

        For Each Row As DataRow In DtDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If RowEsNueva(Row("Indice")) Then Row("Fecha") = DateTime1.Value
            End If
        Next

        If EsRemitoModificado() And DtCabeza.Rows(0).Item("Pedido") <> 0 Then
            DtDetallePedido = New DataTable
            Dim DtDetalleAux As DataTable = DtDetalle.Clone
            PreparaActualizacionPedidos(DtDetalleAnt, DtDetalle.Copy, DtDetalleAux)
            If Not ActualizaDetallePedido("A", "R", DtCabeza.Rows(0).Item("Pedido"), DtDetalleAux, DtDetallePedido) Then Exit Function
        End If

        Dim NumeroW As Double = 0
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable

        If EsRemitoModificado() Then
            If Not LeerCabezaYDetalleAsiento(6060, PRemito, DtAsientoCabeza, DtAsientoDetalle, ConexionRemito) Then Return False
            If DtAsientoCabeza.Rows.Count <> 0 Then
                Dim DtAsientoCabezaAux As DataTable = DtAsientoCabeza.Clone
                Dim DtAsientoDetalleAux As DataTable = DtAsientoDetalle.Clone
                If Not ArmaAsientoRemito(6060, New DataTable, DtDetalle, DtAsientoCabezaAux, DtAsientoDetalleAux, DateTime1.Value) Then Return False
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row.Delete()
                Next
                For Each Row As DataRow In DtAsientoDetalleAux.Rows
                    Row("Asiento") = DtAsientoCabeza.Rows(0).Item("Asiento")
                    Dim Row1 As DataRow = DtAsientoDetalle.NewRow
                    For I As Integer = 0 To DtAsientoDetalleAux.Columns.Count - 1
                        Row1.Item(I) = Row.Item(I)
                    Next
                    DtAsientoDetalle.Rows.Add(Row1)
                Next
            End If
        End If

        Dim DtLotes As New DataTable
        Dim DtStockB As New DataTable
        Dim DtStockN As New DataTable

        NumeroW = ActualizaRemito("M", DtAsientoCabeza, DtAsientoDetalle, DtLotes, DtStockB, DtStockN)

        If NumeroW < 0 Then
            MsgBox("ERROR: En base de datos. Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = 0 Then
            MsgBox("ERROR: Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW > 0 Then
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function LeerAsientosParaModificacion(ByRef DtAsientoDetalle As DataTable, ByRef Asiento As Integer) As Boolean

        If Not Tablas.Read("SELECT D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.Estado = 1 AND C.TipoDocumento = 6060 AND C.Documento = " & DtCabeza.Rows(0).Item("Remito") & ";", ConexionRemito, DtAsientoDetalle) Then Return False
        If DtAsientoDetalle.Rows.Count <> 0 Then Asiento = DtAsientoDetalle.Rows(0).Item("Asiento")

        Return True

    End Function
    Private Function MuestraCabeza() As Boolean

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        If MiEnlazador.Count = 0 Then
            EsAlta = True
            If Not EsManual Then
                If PAbierto And Not EsManual Then
                    UltimoNumero = UltimaNumeracionPV(1, GPuntoDeVenta)
                    If UltimoNumero = 0 Then Return False
                    AgregaCabeza(UltimoNumero)
                Else
                    AgregaCabeza(0)
                End If
            Else
                AgregaCabeza(GPuntoDeVenta & "00000000")
            End If
        Else
            EsAlta = False
        End If

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Remito")
        AddHandler Enlace.Format, AddressOf FormatMaskedRemito
        MaskedRemito.DataBindings.Clear()
        MaskedRemito.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Valor")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextValor.DataBindings.Clear()
        TextValor.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Pedido")
        AddHandler Enlace.Format, AddressOf FormatEntero
        TextPedidoRemito.DataBindings.Clear()
        TextPedidoRemito.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Ingreso")
        AddHandler Enlace.Format, AddressOf FormatEntero
        TextIngreso.DataBindings.Clear()
        TextIngreso.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Cliente")
        ComboCliente.DataBindings.Clear()
        ComboCliente.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Sucursal")
        ComboSucursal.DataBindings.Clear()
        ComboSucursal.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Deposito")
        ComboDeposito.DataBindings.Clear()
        ComboDeposito.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Transporte")
        ComboTransporte.DataBindings.Clear()
        ComboTransporte.DataBindings.Add(Enlace)
        ComboTransporte_Validating(Nothing, Nothing)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaEntrega")
        FechaEntrega.DataBindings.Clear()
        FechaEntrega.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Destino")
        TextDestino.DataBindings.Clear()
        TextDestino.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "PedidoCliente")
        TextPedidoCliente.DataBindings.Clear()
        TextPedidoCliente.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Confirmado")
        CheckConfirmado.DataBindings.Clear()
        CheckConfirmado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "ComprobanteElectronico")
        AddHandler Enlace.Format, AddressOf FormatEntero
        Label26.DataBindings.Clear()
        Label26.DataBindings.Add(Enlace)

        Return True

    End Function
    Private Sub FormatMaskedRemito(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "000000000000")

    End Sub
    Private Sub AgregaCabeza(ByVal Numero As Double)

        Dim Row As DataRow = DtCabeza.NewRow
        Row("Remito") = Numero
        Row("Interno") = 0
        Row("Cliente") = PCliente
        Row("Sucursal") = Sucursal
        Row("Deposito") = PDeposito
        Row("Fecha") = Now
        Row("Estado") = 2
        Row("Factura") = 0
        Row("Comentario") = ""
        Row("Transporte") = 0
        Row("Destino") = ""
        Row("Impreso") = False
        Row("Valor") = 0
        Row("FechaEntrega") = FechaEntrega.Value
        Row("PedidoCliente") = ""
        If Pedido <> 0 Then Row("PedidoCliente") = HallaPedidoCliente(Pedido)
        Row("Pedido") = Pedido
        Row("Ingreso") = PIngreso
        Row("ComprobanteElectronico") = 0
        Row("Confirmado") = False
        Row("PorCuentaYOrden") = PorCuentaYOrden
        Row("SucursalPorCuentaYOrden") = SucursalPorCuentaYOrden
        Row("RemitoReemplazado") = 0
        If Sucursal <> 0 Then
            Row("Destino") = HallaDireccionSucursalCliente(PCliente, Sucursal)
        End If
        DtCabeza.Rows.Add(Row)

    End Sub
    Private Sub FormatRemito(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "0000-00000000")

    End Sub
    Private Sub FormatFactura(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = ""
        Else : Numero.Value = NumeroEditado(Numero.Value)
        End If

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(0, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If IsDBNull(Numero.Value) Then Numero.Value = 0
        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub LlenaDatosCliente(ByVal Cliente As Integer)

        Dim Dta As New DataTable

        Dim Sql As String = "SELECT * FROM Clientes WHERE Clave = " & Cliente & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, el Cliente ya no existe o error en la Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
            Exit Sub
        End If

        Calle = Dta.Rows(0).Item("Calle")
        Numero = Dta.Rows(0).Item("Numero")
        Localidad = Dta.Rows(0).Item("Localidad")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")
        Cuit = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        TieneCodigoCliente = Dta.Rows(0).Item("TieneCodigoCliente")
        Directo = Dta.Rows(0).Item("Directo")

        Dta.Dispose()

    End Sub
    Private Function ModificaDetalleConLista() As Boolean

        Dim ClienteAux As Integer
        Dim SucursalAux As Integer

        If PorCuentaYOrden = 0 Then
            ClienteAux = PCliente
            SucursalAux = ComboSucursal.SelectedValue
        Else
            ClienteAux = PorCuentaYOrden
            SucursalAux = SucursalPorCuentaYOrden
        End If

        Lista = HallaListaPrecios(ClienteAux, SucursalAux, FechaEntrega.Value, PorUnidadEnLista, FinalEnLista)
        If Lista = -1 Then Exit Function
        If Lista = 0 Then
            MsgBox("Remito No Tiene Lista de Precios. Operación se CANCELA.")
            Exit Function
        End If

        Dim Precio As Decimal
        Dim TipoPrecio As Integer

        For Each Row As DataRow In DtDetalle.Rows
            Dim Final As Boolean
            HallaPrecioDeListaSegunArticuloConTipoPrecio(Lista, FechaEntrega.Value, Row("Articulo"), Precio, TipoPrecio, Final)
            If Precio = 0 Then
                MsgBox("Articulo " & NombreArticulo(Row("Articulo")) & " No Esta definido en Lista de Precios.", MsgBoxStyle.Information)
            Else
                If Not Final Then
                    Dim Iva As Double = HallaIva(Row("Articulo"))
                    Precio = Precio + CalculaIva(1, Precio, Iva)
                End If
                If Precio <> Row("Precio") Then Row("Precio") = Precio
                If TipoPrecio <> Row("TipoPrecio") Then Row("TipoPrecio") = TipoPrecio
            End If
        Next

        Return True

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow
        Dim DtArticulo As DataTable

        If PRemito = 0 Then
            DtArticulo = ArticulosActivos()
        Else
            DtArticulo = TodosLosArticulos()
        End If

        If PRemito = 0 And Pedido <> 0 Then
            ActualizaConPedido(DtArticulo)
        End If

        If PRemito = 0 And Pedido = 0 And Lista > 0 Then
            ActualizaConListaPreciosParaAlta(DtArticulo)
        End If

        If PRemito <> 0 And Pedido = 0 And Lista > 0 Then
            ActualizaConListaPreciosParaMofificacion(DtArticulo)
        End If

        If PRemito = 0 And Pedido = 0 And TieneCodigoCliente And Lista = 0 Then
            ActualizoConCodigoCliente(DtArticulo)
        End If

        If PRemito <> 0 Then   'Descarta los articulos desactivados que no esten en el remito.
            For Each Row In DtArticulo.Rows
                Dim RowsBusqueda() As DataRow
                If Row.RowState <> DataRowState.Deleted Then
                    If Row("Estado") = 3 Then
                        RowsBusqueda = DtDetalle.Select("Articulo = " & Row("Clave"))
                        If RowsBusqueda.Length = 0 Then
                            Row.Delete()
                        End If
                    End If
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
    Private Sub ActualizaConPedido(ByRef DtArticulo As DataTable)

        If PRemito = 0 And Pedido <> 0 Then
            Dim RowsBusqueda() As DataRow
            For Each Row As DataRow In DtArticulo.Rows
                RowsBusqueda = DtPedido.Select("Articulo = " & Row("Clave"))
                If RowsBusqueda.Length = 0 Then
                    Row.Delete()
                End If
            Next
        End If

    End Sub
    Private Sub ActualizaConListaPreciosParaAlta(ByRef DtArticulo As DataTable)

        Dim Dt As DataTable
        Dim RowsBusqueda() As DataRow

        Dt = HallaArticulosListaDePrecio(Lista)
        For Each Row As DataRow In DtArticulo.Rows
            RowsBusqueda = Dt.Select("Articulo = " & Row("Clave"))
            If RowsBusqueda.Length = 0 Then
                Row.Delete()
            End If
        Next
        Dt.Dispose()

    End Sub
    Private Sub ActualizaConListaPreciosParaMofificacion(ByVal DtArticulo As DataTable)

        Dim Dt As DataTable
        Dim RowsBusqueda() As DataRow

        Dt = HallaArticulosListaDePrecio(Lista)
        For Each Row As DataRow In DtArticulo.Rows
            RowsBusqueda = Dt.Select("Articulo = " & Row("Clave"))
            If RowsBusqueda.Length = 0 Then
                RowsBusqueda = DtDetalle.Select("Articulo = " & Row("Clave"))
                If RowsBusqueda.Length = 0 Then Row.Delete()
            End If
        Next
        Dt.Dispose()

    End Sub
    Private Sub ActualizoConCodigoCliente(ByVal DtArticulo As DataTable)

        Dim dt As DataTable
        Dim RowsBusqueda() As DataRow

        dt = HallaArticulosConCodigo(PCliente)
        For Each Row As DataRow In DtArticulo.Rows
            RowsBusqueda = dt.Select("Articulo = " & Row("Clave"))
            If RowsBusqueda.Length = 0 Then
                Row.Delete()
            End If
        Next
        dt.Dispose()

    End Sub
    Private Function BuscaIndice(ByVal Indice As Integer) As Boolean

        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Indice = Indice Then Return True
        Next

        Return False

    End Function
    Public Function GrabaImpreso() As Boolean

        Dim Sql As String = "UPDATE RemitosCabeza Set Impreso = 1 WHERE Remito = " & DtCabeza.Rows(0).Item("Remito") & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionRemito)
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
    Private Function ActualizaRemito(ByVal Funcion As String, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtLotes As DataTable, ByVal DtStockB As DataTable, ByVal DtStockN As DataTable) As Double

        If Funcion = "A" Then
            If DtDetalle.Rows.Count = 0 Then   ' lo puse por que graba la cabeza sin el detalle ???????.
                MsgBox("Falta Informar Articulos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return -2
            End If
        End If

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                'Regraba numeracion.
                If PAbierto And Funcion = "A" And Not EsManual Then
                    If Not ReGrabaUltimaNumeracion(DtCabeza.Rows(0).Item("Remito"), 1) Then Return -10
                End If

                'Actualiza Remito.
                If Not IsNothing(DtCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtCabeza.GetChanges, "RemitosCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtDetalle.GetChanges, "RemitosDetalle", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                'Graba Asiento.
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza Pedidos.
                If Not IsNothing(DtDetallePedido.GetChanges) Then
                    Resul = GrabaTabla(DtDetallePedido.GetChanges, "PedidosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza Ingreso.
                If Not IsNothing(DtIngresoMercaderiasCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtIngresoMercaderiasCabeza.GetChanges, "IngresoMercaderiasCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza AsignacionLotes y stock Para Articulos Loteados.
                If Not IsNothing(DtLotes.GetChanges) Then
                    Resul = GrabaTabla(DtLotes.GetChanges, "AsignacionLotes", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtStockB.GetChanges) Then
                    Resul = GrabaTabla(DtStockB.GetChanges, "Lotes", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtStockN.GetChanges) Then
                    Resul = GrabaTabla(DtStockN.GetChanges, "Lotes", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function BorraRemito(ByVal DtCabezaAux As DataTable, ByVal DtDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                'Actualiza Remito.
                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaAux.GetChanges, "RemitosCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleAux.GetChanges, "RemitosDetalle", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                'Graba Asiento.
                If Not IsNothing(DtAsientoCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaAux.GetChanges, "AsientosCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleAux.GetChanges, "AsientosDetalle", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function ActualizaRemitoYFactura(ByVal DtCabezaAux As DataTable, ByVal DtAsientoCabezaW As DataTable, ByVal DtCabezaFactura As DataTable, ByVal DtCabezaFacturaRel As DataTable, ByVal DtDetalleFactura As DataTable, ByVal DtDetalleFacturaRel As DataTable) As Boolean

        Dim NumeroW As Integer

        NumeroW = GrabaModificacion(DtCabezaAux, DtAsientoCabezaW, DtCabezaFactura, DtCabezaFacturaRel, DtDetalleFactura, DtDetalleFacturaRel)

        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW > 0 Then
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function GrabaModificacion(ByVal DtCabezaAux As DataTable, ByVal DtAsientoCabezaW As DataTable, ByVal DtCabezaFactura As DataTable, ByVal DtCabezaFacturaRel As DataTable, ByVal DtDetalleFactura As DataTable, ByVal DtDetalleFacturaRel As DataTable) As Double

        Dim Resul As Double
        Dim NumeroAsiento As Integer

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                'Actualiza Remito.
                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaAux.GetChanges, "RemitosCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                'Graba Asiento.
                If Not IsNothing(DtAsientoCabezaW.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaW.GetChanges, "AsientosCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                'Graba factura.
                If Not IsNothing(DtCabezaFactura.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaFactura.GetChanges, "FacturasCabeza", ConexionFactura)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtDetalleFactura.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleFactura.GetChanges, "FacturasDetalle", ConexionFactura)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtCabezaFacturaRel.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaFacturaRel.GetChanges, "FacturasCabeza", ConexionFacturaRelacionada)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtDetalleFacturaRel.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleFacturaRel.GetChanges, "FacturasDetalle", ConexionFacturaRelacionada)
                    If Resul <= 0 Then Return Resul
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Dim Patron As String = GPuntoDeVenta & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Remito) FROM RemitosCabeza WHERE CAST(CAST(Remito AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(GPuntoDeVenta & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Dim Patron As String = GPuntoDeVenta & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM RemitosCabeza WHERE CAST(CAST(RemitosCabeza.Remito AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
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
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        'http://vb-helper.com/howto_net_print_and_preview.html

        If CheckSinPesos.Checked Then
            If MsgBox("No se Imprimirán las columnas Kilos por Unidad(Kg/Un) ni Total Kilos(Total Kg). Quiere continuar de todas maneras(Y/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        If PRemito = 0 Then
            MsgBox("Opcion Invalida. Remito debe ser Grabado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If (Not IsNothing(DtDetalle.GetChanges) Or Not IsNothing(DtCabeza.GetChanges)) And Not GModificacionOk Then
            MsgBox("Debe Grabar Modificaciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtDetalle.Rows.Count > GLineasRemitos And Not EsAutoImpreso Then
            MsgBox("Remito tiene más Articulos de lo Permitido para Impresión.", MsgBoxStyle.Information) : Exit Sub
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Impreso") And PAbierto Then
            If MsgBox("Remito Ya fue Impreso. Quiere Re-Imprimir?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        If PAbierto And EsAutoImpreso Then
            If FechaCaiVencida() Then Exit Sub
        End If

        If TieneCodigoCliente Then
            For Each Row As DataGridViewRow In Grid.Rows
                If IsNothing(Row.Cells("Articulo").Value) Then Exit For
                Dim Codigo As String = HallaCodigoCliente(PCliente, Row.Cells("Articulo").Value)
                If IsNothing(Codigo) Then Codigo = "-1"
                If Codigo = "-1" Then
                    MsgBox("Articulo: " & Row.Cells("Articulo").FormattedValue & " No Tiene Codigo Cliente.", MsgBoxStyle.Information)
                    Exit Sub
                End If
                Row.Cells("CodigoCliente").Value = Codigo
            Next
        End If

        ErrorImpresion = False
        Paginas = 0

        Dim PuntoVentaW As Integer = Val(Strings.Left(Format(DtCabeza.Rows(0).Item("Remito"), "0000-00000000"), 4))
        If PAbierto And (CopiasSegunPuntoVenta = 0 Or PuntoVentaW <> UltimoPuntoVentaParaCopiaSeleccionado) Then
            UltimoPuntoVentaParaCopiaSeleccionado = PuntoVentaW
            CopiasSegunPuntoVenta = TraeCopiasComprobante(1, PuntoVentaW)
            If CopiasSegunPuntoVenta < 0 Then CopiasSegunPuntoVenta = 0 : MsgBox("Error al Leer Tabla: PuntosDeVenta. Operacion se CANCELA.", MsgBoxStyle.Critical) : Exit Sub
        End If

        If PAbierto Then
            Copias = CopiasSegunPuntoVenta
        Else : Copias = 1
        End If
        'Acepción para Patagonia y Pradan.
        If (GCuitEmpresa = GPatagonia Or GCuitEmpresa = GPradan) And PAbierto = False Then
            Copias = 3
        End If
        If GCuitEmpresa = GPedidosYaTrucho And PAbierto = False Then
            Copias = 2
        End If

        ContadorCopias = 1

        TotalPaginas = Grid.Rows.Count / GLineasRemitos
        If Grid.Rows.Count / GLineasRemitos - TotalPaginas > 0 Then
            TotalPaginas = TotalPaginas + 1
        End If
        ContadorPaginas = 1

        Dim print_document As New PrintDocument
        UnSeteoImpresora.SeteaImpresion(print_document)
        AddHandler print_document.PrintPage, AddressOf Print_PrintPage
        print_document.Print()

        'newpage = True
        print_document.Dispose()

        If ErrorImpresion Then Exit Sub

        If Not GrabaImpreso() Then Exit Sub
        DtCabeza.Rows(0).Item("Impreso") = True
        DtCabeza.AcceptChanges()

        RadioPrecioSinIva.Checked = False
        RadioPrecioConIva.Checked = False

    End Sub
    Private Sub Print_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '        Dim xPos As Single = e.MarginBounds.Left

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim PrintFont As System.Drawing.Font
        Dim SaltoLinea As Integer = 4
        Dim MIzq = 9
        Dim MTop = 20

        If (PAbierto And EsAutoImpreso) Then
            Dim NombreEmpres As String = GNombreEmpresa '"Patagonia Sunrise S.R.L."
            Dim CuitEmpresa As String = GCuitEmpresa    '"30-12345678-5"
            Dim TipoComprobante As Integer              '1. factura 2. debito 3. credito 4.Guia 5. Remito
            If Not PAbierto Then
                TipoComprobante = 4
            End If
            If PAbierto And EsAutoImpreso Then
                TipoComprobante = 5
            End If
            Dim Direccion1 As String = GDireccion1      '"Nave S-2 Puesto 34 - Mercado Central de Buenos Aires"
            Dim Direccion2 As String = GDireccion2      '"Autopista Ricchieri y Boulongne Sur Mer"
            Dim Direccion3 As String = GDireccion3      '"C.P.(1771) Villa Celina Pcia. de Buenos Aires"
            Dim LetraComprobante As String = "X"
            If PAbierto And EsAutoImpreso Then
                LetraComprobante = "R"
            End If
            Dim IngBruto As String = GIngBruto     '"Conv.Mult. Nª 901-898989-6"
            Dim FechaInicio As String = GFechaInicio
            '
            MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, LetraComprobante, GCondicionIvaEmpresa, IngBruto, FechaInicio, "C:\XML Afip\", e, False, 0)
            Texto = NumeroEditado(MaskedRemito.Text)
            PrintFont = New Font("Arial", 16)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, MTop + 6)           '19
        End If
        If Not PAbierto Then
            MascaraImpresionRemitosN.ImprimeMascara(GNombreEmpresa, MaskedRemito.Text, e)
        End If

        Dim Str1 As String = ""
        Select Case ContadorCopias
            Case 1
                Str1 = "ORIGINAL"
            Case 2
                Str1 = "DUPLICADO"
            Case 3
                Str1 = "TRIPLICADO"
            Case 4
                Str1 = "CUATRIPLICADO"
        End Select

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            'Titulos.
            x = MIzq
            y = MTop + 44
            If EsAutoImpreso Then
                Texto = Str1
                PrintFont = New Font("Arial", 10)
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 160, MTop)
            End If
            '
            PrintFont = New Font("Courier New", 10)
            If DateTime1.Value <> FechaImpresion.Value Then
                Texto = FechaImpresion.Text
            Else
                Texto = DateTime1.Text
            End If
            Dim Posicion As Integer
            If GCuitEmpresa = MecaFront Then
                Posicion = MTop
            Else
                Posicion = MTop + 14
            End If
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 120, Posicion)
            'Titulos.
            If PAbierto Then
                Texto = "CLIENTE        : " & ComboCliente.Text
            Else
                If RadioPrecioSinIva.Checked Or RadioPrecioConIva.Checked Then
                    Texto = "CLIENTE        : " & ComboCliente.Text & "        VALOR : " & TextValor.Text
                Else
                    Texto = "CLIENTE        : " & ComboCliente.Text
                End If
            End If
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            If Numero <> 0 Then
                Texto = "DOMICILIO      : " & Calle & " No: " & Numero
            Else
                Texto = "DOMICILIO      : " & Calle
            End If
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "LOCALIDAD      : " & Localidad
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "DOM.ENTREGA    : " & ComboSucursal.Text & "-" & TextDestino.Text
            PrintFont = New Font("Courier New", 10, FontStyle.Bold)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "CUIT           : " & Cuit & "  -  " & ComboTipoIva.Text
            PrintFont = New Font("Courier New", 10)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Dim Fecha As Date
            If FechaEntrega.Text <> FechaEntregaImpresion.Value Then
                Fecha = FechaEntregaImpresion.Value
            Else
                Fecha = FechaEntrega.Value
            End If
            Texto = "CONDICION VENTA: " & TextCondicionVenta.Text & " Pedido: " & TextPedidoCliente.Text & "  F. Entrega " & Format(Fecha, "dd/MM/yyyy")
            PrintFont = New Font("Courier New", 10, FontStyle.Bold)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            PrintFont = New Font("Courier New", 10)
            'Grafica.

            Dim YInicioRectangulo As Integer = 70

            x = 9
            y = MTop + YInicioRectangulo

            Dim x1 As Integer = x - 5   'margen izquierdo para cuadro de articulos.
            Dim Ancho As Integer = 198
            Dim Alto As Integer = 125
            Dim LineaArticulo As Integer = x1 + 87
            Dim LineaCantidad As Integer = x1 + 112
            Dim LineaBultos As Integer = x1 + 129    '132
            Dim LineaPrecio As Integer = x1 + 150
            Dim LineaKilos As Integer = x1 + 172   '174
            Dim LineaTotalKilos As Integer = x1 + Ancho - 1
            Dim Longi As Integer
            Dim Xq As Integer
            Dim Bultos As Decimal
            Dim Kilos As Decimal
            Dim Articulo As String
            Dim Partes() As String
            Dim Excedente As Integer = Grid.Rows.Count - 29
            '
            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.2), x1, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.2), LineaArticulo, y, LineaArticulo, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.2), LineaCantidad, y, LineaCantidad, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.2), LineaBultos, y, LineaBultos, y + Alto)
            If RadioPrecioConIva.Checked Or RadioPrecioSinIva.Checked Then
                e.Graphics.DrawLine(New Pen(Color.Black, 0.2), LineaPrecio, y, LineaPrecio, y + Alto)
            Else
                LineaTotalKilos = LineaKilos + 24
                LineaKilos = LineaPrecio + 10
            End If
            '
            PrintFont = New Font("Courier New", 12)
            Texto = "Pagina: " & ContadorPaginas & "/" & TotalPaginas
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + Ancho - 40, MTop - 17)
            '
            e.Graphics.DrawLine(New Pen(Color.Black, 0.2), LineaKilos, y, LineaKilos, y + Alto)
            'Titulos de descripcion.
            PrintFont = New Font("Courier New", 11)
            Texto = "ARTICULO"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x1 + 2, y + 2)
            Texto = "CANTIDAD"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi - 3
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "BULTOS"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaBultos - Longi
            e.Graphics.DrawString(Texto, New Font("Courier New", 9), Brushes.Black, Xq, y + 2)
            If RadioPrecioSinIva.Checked Or RadioPrecioConIva.Checked Then
                Texto = "PRECIO"
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaPrecio - Longi - 1
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            End If
            Texto = "U.Med/Un"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaKilos - Longi + 1
            e.Graphics.DrawString(Texto, New Font("Courier New", 9), Brushes.Black, Xq, y + 2)
            Texto = "Tot.U.Med"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaTotalKilos - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.3), x1, y, x1 + Ancho, y)
            'Descripcion de Articulos.
            PrintFont = New Font("Courier New", 9)
            y = y - SaltoLinea
            'For Each Row As DataGridViewRow In Grid.Rows
            Do While mRow < Grid.Rows.Count
                Dim Row As DataGridViewRow = Grid.Rows(mRow)
                If IsNothing(Row.Cells("Articulo").Value) Then Exit Do
                y = y + SaltoLinea
                Partes = Split(Row.Cells("Articulo").FormattedValue, "(")
                Articulo = Partes(0)
                If TieneCodigoCliente Then
                    Articulo = RellenarBlancos(Row.Cells("CodigoCliente").Value, 8) & " " & Articulo
                End If
                Texto = Articulo
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x1, y)
                '
                '  Cantidad ------------------------------------------------
                Texto = Row.Cells("Cantidad").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaCantidad - Longi - 6
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                '  Medida---------------------------------------------------
                Texto = Row.Cells("Medida").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaCantidad - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                '  Bultos --------------------------------------------------
                CantidadPorBulto = HallaCantidadPrimaria(Row.Cells("Articulo").Value)
                If CantidadPorBulto = 0 Then CantidadPorBulto = 1
                Texto = Math.Floor(Row.Cells("Cantidad").FormattedValue / CantidadPorBulto)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaBultos - Longi
                e.Graphics.DrawString(Texto, New Font("Courier New", 7), Brushes.Black, Xq, y)
                '  ----------------------------------------------------------
                Dim PrecioSinIVA As Decimal = 0, Precio As Decimal = 0
                If Row.Cells("TipoPrecio").Value = 1 Then
                    PrecioSinIVA = Row.Cells("PrecioSinIva").Value
                    Precio = Row.Cells("Precio").Value
                End If
                If Row.Cells("TipoPrecio").Value = 2 Then
                    PrecioSinIVA = CalculaNeto(Row.Cells("PrecioSinIva").Value, Row.Cells("KilosXUnidad").Value)
                    Precio = CalculaNeto(Row.Cells("Precio").Value, Row.Cells("KilosXUnidad").Value)
                End If
                If RadioPrecioSinIva.Checked Then
                    Texto = Format(PrecioSinIVA, "#0.00")
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaPrecio - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                End If
                If RadioPrecioConIva.Checked Then
                    Texto = Format(Precio, "#0.00")
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaPrecio - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                End If
                '  U.Medida por unidad y Total U.Medida--------------------
                If Row.Cells("AGranel").Value Then
                    Kilos = Kilos + Row.Cells("Cantidad").Value
                Else
                    Bultos = Bultos + Math.Floor(Row.Cells("Cantidad").FormattedValue / CantidadPorBulto)
                End If
                If Not CheckSinPesos.Checked Then
                    PrintFont = New Font("Courier New", 7)
                    Texto = Row.Cells("KilosXUnidad").FormattedValue & " " & Row.Cells("UMedida").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaKilos - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                    ' Total de U.Medida-------------------------------------
                    PrintFont = New Font("Courier New", 9)
                    Texto = Row.Cells("TotalKilos").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaTotalKilos - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                End If
                mRow += 1
                newpage = False
                If Excedente <= 0 Then Continue Do
                If mRow + 1 = Grid.Rows.Count - Excedente Then
                    e.HasMorePages = True
                    newpage = True
                    ContadorPaginas = ContadorPaginas + 1
                    Exit Do
                End If
            Loop
            '
            y = MTop + YInicioRectangulo + Alto + 1
            PrintFont = New Font("Courier New", 9)
            Texto = "Bultos: " & Bultos & " A-Granel: " & Kilos & " U.Medida  " & GNombreEmpresa & " RMT: " & MaskedRemito.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "Conductores: " & TextConductor.Text & "  Patentes:   " & TextPatentes.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            'Imprime Leyenda.
            y = y + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox1.Text, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox2.Text, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox3.Text, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox4.Text, PrintFont, Brushes.Black, x, y)

            y = y + 1 * SaltoLinea
            PrintFont = New Font("Courier New", 10)
            If DtCabeza.Rows(0).Item("ComprobanteElectronico") <> 0 Then
                Texto = "Comprobante Electrónico " & DtCabeza.Rows(0).Item("ComprobanteElectronico")
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            End If
            Texto = "Recibí Conforme:........................"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 90, y)

            'Imprime Permisos de Importacion.
            Dim PermisoImportacion(0) As String
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.PermisoImp <> "" Then
                    ReDim Preserve PermisoImportacion(UBound(PermisoImportacion) + 1)
                    PermisoImportacion(UBound(PermisoImportacion)) = Fila.PermisoImp
                End If
            Next

            y = MTop + 227
            e.Graphics.DrawString("Permiso Imp.:  ", PrintFont, Brushes.Black, x, y)
            Dim TextoPermiso As String = ""
            PrintFont = New Font("Courier New", 7)
            For K As Integer = 1 To UBound(PermisoImportacion)
                TextoPermiso = TextoPermiso & " " & PermisoImportacion(K)
                If K = 2 Then
                    e.Graphics.DrawString(TextoPermiso, PrintFont, Brushes.Black, x + 29, y)
                    TextoPermiso = ""
                End If
                If K = 4 Or K = 6 Or K = 9 Then
                    y = y + SaltoLinea
                    e.Graphics.DrawString(TextoPermiso, PrintFont, Brushes.Black, x + 29, y)
                    TextoPermiso = ""
                End If
                If K = 9 Then Exit For
            Next
            If TextoPermiso <> "" Then
                y = y + SaltoLinea
                e.Graphics.DrawString(TextoPermiso, PrintFont, Brushes.Black, x + 29, y)
            End If
            PrintFont = New Font("Courier New", 10)
            '-----------------------------------------------------------
            'Imprime Cai si es autoimpresion.
            If PAbierto And EsAutoImpreso Then
                PrintFont = New Font("Courier New", 10)
                y = 263
                Dim CaiText As String = "C.A.I. Nro.: " & Format(Cai, "00000000000000") & "           Fecha de Vto.: " & Strings.Right(IntFechaCai, 2) & "/" & Strings.Mid(IntFechaCai, 5, 2) & "/" & Strings.Left(IntFechaCai, 4)
                e.Graphics.DrawString(CaiText, PrintFont, Brushes.Black, x, y)
                '----------------------------------------Codigo Barra ------------------------------------------------------------------------------
                Dim CodigoBarra As String = Format(CuitNumerico(GCuitEmpresa), "00000000000") & "91" & Format(CInt(Strings.Left(MaskedRemito.Text, 4)), "0000") & Format(Cai, "00000000000000") & Format(IntFechaCai, "00000000")
                Dim aa As New DllVarias
                CodigoBarra = aa.CombierteTextoAInterleaved2Of5(CodigoBarra, True)
                Dim Tamanio As Integer = 18
                Dim fuente As Font
                fuente = CustomFont.GetInstance(Tamanio, FontStyle.Regular)
                e.Graphics.DrawString(CodigoBarra, fuente, Brushes.Black, x, y + 5)
                '------------------------------------------------------------------------------------------------------------------------------------
                PrintFont = New Font("Courier New", 10)
                CaiText = "Auto Impreso por " & GNombreEmpresa
                e.Graphics.DrawString(CaiText, PrintFont, Brushes.Black, x + 10, y + 23)
            End If
            '-----------------------------------------------------------
            If GCuitEmpresa = GPatagonia Or GCuitEmpresa = GCuadroNorte Or GCuitEmpresa = GPremiumFruit Then
                ArmaRectangulos(x, y, e)
            End If
            '-----------------------------------------------------------
            If newpage Then Exit Sub
            mRow = 0

            Paginas = Paginas + 1
            ContadorCopias = ContadorCopias + 1
            ContadorPaginas = 1

            If Paginas < Copias Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
            e.HasMorePages = False
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
    Private Function AnalizaArticuloConTipoPrecio(ByVal ListaW As Integer, ByVal Articulo As Integer, ByVal KilosXUnidad As Decimal, ByRef Cantidad As Decimal, ByRef Precio As Decimal, ByRef TipoPrecio As Integer) As Boolean

        Cantidad = 0
        Precio = 0
        TipoPrecio = 0

        If TieneCodigoCliente Then
            Dim Codigo As String = HallaCodigoCliente(PCliente, Articulo)
            If IsNothing(Codigo) Then Codigo = "-1"
            If Codigo = "-1" Then
                MsgBox("Articulo " & NombreArticulo(Articulo) & " No Tiene Código Cliente, No sera incluido en el Remito.", MsgBoxStyle.Information)
                Return False
            End If
        End If

        If Pedido <> 0 Then
            Dim Entregada As Decimal
            Dim ArticuloExisteEnPedido As Boolean
            HallaCantidadYPrecioPedido(Pedido, Articulo, Cantidad, Entregada, Precio, TipoPrecio, ArticuloExisteEnPedido)
            Cantidad = Cantidad - Entregada
            If Cantidad < 0 Then Cantidad = 0
        End If

        If ListaW <> 0 Then
            Dim Final As Boolean
            HallaPrecioDeListaSegunArticuloConTipoPrecio(ListaW, FechaEntrega.Value, Articulo, Precio, TipoPrecio, Final)
            If Precio = 0 Then
                MsgBox("Articulo " & NombreArticulo(Articulo) & " No Esta definido en Lista de Precios.", MsgBoxStyle.Information)
                Return False
            End If
            '
            '
            If Not Final Then
                Dim Iva As Double = HallaIva(Articulo)
                Precio = Precio + CalculaIva(1, Precio, Iva)
                Precio = Trunca3(Precio)
            End If
        End If

        Return True

    End Function
    Private Sub CompletaGrid()

        'Completa el grid con datos que no estan en DtDetalle.
        For Each Row As DataGridViewRow In Grid.Rows
            If IsNothing(Row.Cells("Articulo").Value) Then Exit For
            Row.Cells("TotalKilos").Value = Trunca3((Row.Cells("Cantidad").Value - Row.Cells("Devueltas").Value) * Row.Cells("KilosXUnidad").Value)
            HallaAGranelYMedida(Row.Cells("Articulo").Value, Row.Cells("AGranel").Value, Row.Cells("Medida").Value)
            If Row.Cells("AGranel").Value Then
                Row.Cells("KilosXUnidad").ReadOnly = True
                Row.Cells("TotalKilos").ReadOnly = True
            End If
            If Row.Cells("Devueltas").Value > 0 Then
                Row.Cells("Devueltos").Value = Row.Cells("Devueltas").Value
            Else : Row.Cells("Aumentos").Value = -Row.Cells("Devueltas").Value
            End If
            Row.Cells("PrecioSinIva").Value = FormatNumber(HallaPrecioSinIva(Row.Cells("Articulo").Value, Row.Cells("Precio").Value), 2)
        Next

    End Sub
    Private Function EsRemitoModificado() As Boolean

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtDetalleAnt.Select("Indice = " & Row("Indice"))
                If RowsBusqueda.Length <> 0 Then
                    If RowsBusqueda(0).Item("Cantidad") <> Row("Cantidad") Or RowsBusqueda(0).Item("KilosXUnidad") <> Row("KilosXUnidad") Then
                        Return True
                    End If
                Else
                    Return True
                End If
            End If
        Next

        For Each Row As DataRow In DtDetalleAnt.Rows
            RowsBusqueda = DtDetalle.Select("Indice = " & Row("Indice"))
            If RowsBusqueda.Length = 0 Then
                Return True
            End If
        Next

        Return False

    End Function
    Private Sub PreparaActualizacionPedidos(ByVal DtDetalleAntW As DataTable, ByVal DtDetalleW As DataTable, ByRef DtDetalleAux As DataTable)

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtDetalleW.Rows
            If Row.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtDetalleAntW.Select("Indice = " & Row("Indice"))
                If RowsBusqueda.Length <> 0 Then
                    If RowsBusqueda(0).Item("Cantidad") <> Row("Cantidad") Then
                        Dim Diferencia As Decimal = Row("Cantidad") - RowsBusqueda(0).Item("Cantidad")
                        If Diferencia <> 0 Then
                            Row("Cantidad") = Diferencia
                            DtDetalleAux.ImportRow(Row)
                        End If
                    End If
                Else
                    DtDetalleAux.ImportRow(Row)
                End If
            End If
        Next

        For Each Row As DataRow In DtDetalleAntW.Rows
            RowsBusqueda = DtDetalleW.Select("Indice = " & Row("Indice"))
            If RowsBusqueda.Length = 0 Then
                Row("Cantidad") = -Row("Cantidad")
                DtDetalleAux.ImportRow(Row)
            End If
        Next

    End Sub
    Private Function RowEsNueva(ByVal Indice As Integer) As Boolean

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtDetalleAnt.Select("Indice = " & Indice)
        If RowsBusqueda.Length = 0 Then Return True

    End Function
    Private Sub CompruebaAlta(ByVal Remito As Decimal, ByVal Asiento As Decimal, ByVal ConexionStr As String)

        Dim Str As String = ""
        Dim NoExisteCabeza As Boolean
        Dim NoExisteCabezaAsiento As Boolean

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Remito FROM RemitosCabeza WHERE Remito = " & Remito & ";", ConexionStr, Dt) Then
        End If
        If Dt.Rows.Count = 0 Then Str = Str & vbCrLf & " Remito Cabeza No Grabado." : NoExisteCabeza = True

        Dt = New DataTable
        If Not Tablas.Read("SELECT Remito FROM RemitosDetalle WHERE Remito = " & Remito & ";", ConexionStr, Dt) Then
        End If
        If Dt.Rows.Count = 0 Then Str = Str & vbCrLf & " Remito Detalle No Grabado."

        If Asiento <> 0 Then
            Dt = New DataTable
            If Not Tablas.Read("SELECT Asiento FROM AsientosCabeza WHERE Asiento = " & Asiento & ";", ConexionStr, Dt) Then
            End If
            If Dt.Rows.Count = 0 Then Str = Str & vbCrLf & " Asiento Cabeza No Grabado." : NoExisteCabezaAsiento = True
            '
            Dt = New DataTable
            If Not Tablas.Read("SELECT Asiento FROM AsientosDetalle WHERE Asiento = " & Asiento & ";", ConexionStr, Dt) Then
            End If
            If Dt.Rows.Count = 0 Then Str = Str & vbCrLf & " Asiento Detalle No Grabado."
        End If

        If Not NoExisteCabeza And Str <> "" Then
            Str = "Error 5000 " & Str
            Str = Str & vbCrLf & vbCrLf & " SE RECOMIENDA dar de Baja al Remito y Generarlo Nuevamente."
            Str = Str & vbCrLf & " Si el Error se repite Avisar al Administrador del Sistema."
        End If

        If Asiento <> 0 And NoExisteCabezaAsiento Then  'Aveces por errores desconocido no graba todos los archivos. Esto borra Asiento detalle en caso de existir sin Asiento Cabeza.
            BorraAsientosDetalleSinCabeza(Asiento, ConexionStr)
        End If

        Dt.Dispose()

        If Str <> "" Then
            MsgBox(Str)
        End If

    End Sub
    Public Function RemitoExiste(ByVal Remito As Decimal) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionRemito)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Remito FROM RemitosCabeza WHERE Remito = " & Remito & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function UltimoNumeroInternoRemito(ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Interno) FROM RemitosCabeza;", Miconexion)
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
    Private Function FechaCaiVencida() As Boolean

        Dim Fe As Integer = Format(FechaImpresion.Value.Year, "0000") & Format(FechaImpresion.Value.Month, "00") & Format(FechaImpresion.Value.Day, "00")
        If IntFechaCai < Fe Then
            MsgBox("Fecha CAI Ya Vencida para Punto De Venta: " & GPuntoDeVenta, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return True
        End If

    End Function
    Private Function HallaListaPrecioCliente() As Integer

        Lista = 0

        'Halla lista de precios (Si es del cliente o por cliente por cuenta y orden).
        Dim ClienteWW As Integer
        Dim SucursalWW As Integer
        If DtCabeza.Rows(0).Item("PorCuentaYOrden") <> 0 Then
            ClienteWW = DtCabeza.Rows(0).Item("PorCuentaYOrden")
            SucursalWW = DtCabeza.Rows(0).Item("SucursalPorCuentaYOrden")
        Else
            ClienteWW = PCliente
            SucursalWW = DtCabeza.Rows(0).Item("Sucursal")
        End If

        Return HallaListaPrecios(ClienteWW, SucursalWW, DtCabeza.Rows(0).Item("FechaEntrega"), PorUnidadEnLista, FinalEnLista)

    End Function
    Private Function CalculaValor() As Decimal

        Dim Total As Decimal = 0
        Dim Precio As Decimal

        For Each Row As DataRow In DtDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If PAbierto Then
                    Precio = Row("Precio")
                Else
                    Dim Iva As Decimal = HallaIva(Row("Articulo"))
                    Precio = Trunca(Row("Precio") / (1 + Iva / 100))
                End If
                If Row("TipoPrecio") = 1 Then
                    Total = Total + CalculaNeto(Row("Cantidad") - Row("Devueltas"), Precio)
                End If
                If Row("TipoPrecio") = 2 Then
                    Total = Total + CalculaNeto(Row("Cantidad") - Row("Devueltas"), (Precio * Row("KilosXUnidad")))
                End If
            End If
        Next

        Return Total

    End Function
    Private Function ExisteDevoluciones() As String

        Dim Dt As New DataTable
        ExisteDevoluciones = ""

        Dim Sql As String = "SELECT Devolucion FROM DevolucionCabeza WHERE Estado <> 3 AND Remito = " & PRemito & ";"

        If Not Tablas.Read(Sql, ConexionRemito, Dt) Then End
        If Dt.Rows.Count = 0 Then
            Dt.Dispose()
            Exit Function
        End If

        For Each Row As DataRow In Dt.Rows
            ExisteDevoluciones = ExisteDevoluciones & NumeroEditado(Row("Devolucion")) & " "
        Next

        Dt.Dispose()

    End Function
    Private Function Valida() As Boolean

        If EsManual Then
            If Val(Strings.Left(MaskedRemito.Text, 4)) = "0000" Then
                MsgBox("Falta Informar Punto de Venta en Numero de Remito.", MsgBoxStyle.Critical)
                Return False
            End If
            If PRemito = 0 Then
                If Val(Strings.Left(MaskedRemito.Text, 4)) <> GPuntoDeVenta Then
                    MsgBox("Punto de Venta Debe ser " & Format(GPuntoDeVenta, "0000"), MsgBoxStyle.Critical)
                    Return False
                End If
            End If
            If PAbierto Then
                If Val(Strings.Mid(MaskedRemito.Text, 5, 8)) = "00000000" Then
                    MsgBox("Falta Informar Numero de Remito.", MsgBoxStyle.Critical)
                    Return False
                End If
                If PRemito = 0 Then
                    If RemitoExiste(Val(MaskedRemito.Text)) Then
                        MsgBox("Numero Remito Ya Existe.", MsgBoxStyle.Critical)
                        Return False
                    End If
                End If
            End If
        Else
            If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 And PRemito = 0 Then
                MsgBox("Fecha Incorrecta. Existe Otro Comprobante con Fecha " & UltimaFechaW & " Posterior a la Informada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                DateTime1.Focus()
                Return False
            End If
            If GCuitEmpresa <> GARD And GCuitEmpresa <> GQUORE Then
                If PRemito = 0 And DiferenciaDias(Date.Now, FechaEntrega.Value) < 0 And PIngreso = 0 Then
                    MsgBox("Fecha Entrega Debe ser Mayor o Igual a Fecha Actual.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
        End If

        If DtDetalle.Rows.Count = 0 Then
            MsgBox("Falta Informar Articulos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        For i As Integer = 0 To Grid.Rows.Count - 2
            If Not IsNumeric(Grid.Rows(i).Cells("Cantidad").Value) Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Cantidad")
                Grid.BeginEdit(True)
                Return False
            End If
            If Grid.Rows(i).Cells("Cantidad").Value = 0 Or Grid.Rows(i).Cells("Cantidad").Value = 0 Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Cantidad")
                Grid.BeginEdit(True)
                Return False
            End If
            If TieneDecimales(Grid.Rows(i).Cells("Cantidad").Value) And Not Grid.Rows(i).Cells("AGranel").Value Then
                MsgBox("Cantidad no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Cantidad")
                Grid.BeginEdit(True)
                Return False
            End If
            If Not IsNumeric(Grid.Rows(i).Cells("KilosXUnidad").Value) Then
                MsgBox("Debe Informar Kilos por Unidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("KilosXUnidad")
                Grid.BeginEdit(True)
                Return False
            End If
            If Grid.Rows(i).Cells("KilosXUnidad").Value = 0 Then
                MsgBox("Debe Informar Kilos por Unidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("KilosXUnidad")
                Grid.BeginEdit(True)
                Return False
            End If
            If Grid.Rows(i).Cells("TipoPrecio").Value = 0 Then
                If Grid.Rows(i).Cells("Precio").Value <> 0 Then
                    MsgBox("Debe Informar Si Precio es por Unidad o Por Kilo en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
            If Grid.Rows(i).Cells("TipoPrecio").Value <> 0 Then
                If Grid.Rows(i).Cells("Precio").Value = 0 Then
                    MsgBox("Debe Informar Precio en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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

        Return True

    End Function
    Private Function ValidaFechaEntregaPedido() As Boolean

        Dim Dt As New DataTable

        If Not PideDatosPedido(FechaEntrega.Value, PCliente, DtCabeza.Rows(0).Item("Sucursal"), True, True, Dt) Then Return False
        For Each Row As DataRow In Dt.Rows
            If Row("Pedido") = DtCabeza.Rows(0).Item("Pedido") Then
                If Row("Cerrado") Then
                    MsgBox("El Pedido " & DtCabeza.Rows(0).Item("Pedido") & " asociado al Remito esta Cerrado. Operación se CANCELA.") : Dt.Dispose() : Return False
                Else
                    Dt.Dispose() : Return True
                End If
            End If
        Next
        MsgBox("Nueva Fecha Entrega no se corresponde al Pedido " & DtCabeza.Rows(0).Item("Pedido") & " asociado al Remito. Operación se CANCELA.")

        Dt.Dispose()
        Return False

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then
            MsgBox("No hay un registro actual para Eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Devueltas").Value <> 0 Then
            MsgBox("Articulos con Devoluciones no se pueden Borrar.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Grid.Rows.Remove(Grid.CurrentRow)

    End Sub
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

        If Grid.Columns(e.ColumnIndex).Name = "KilosXUnidad" Or Grid.Columns(e.ColumnIndex).Name = "TotalKilos" Or Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Precio" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Articulo").Value) Then
                If Grid.CurrentRow.Cells("Articulo").Value = 0 Then e.Cancel = True
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "KilosXUnidad" Or Grid.Columns(e.ColumnIndex).Name = "TotalKilos" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Cantidad").Value) Then
                If Grid.CurrentRow.Cells("Cantidad").Value = 0 Then e.Cancel = True
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Cantidad").Value) Then
                If Grid.CurrentRow.Cells("Devueltas").Value <> 0 Then
                    MsgBox("Cantidad con Devoluciones no se pueden Modificar.", MsgBoxStyle.Information)
                    e.Cancel = True
                End If
            End If
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then            'Completa el grid.
            HallaAGranelYMedida(Grid.Rows(e.RowIndex).Cells("Articulo").Value, Grid.Rows(e.RowIndex).Cells("AGranel").Value, Grid.Rows(e.RowIndex).Cells("Medida").Value)
            If Grid.Rows(e.RowIndex).Cells("AGranel").Value Then
                Grid.Rows(e.RowIndex).Cells("KilosXUnidad").ReadOnly = True
                Grid.Rows(e.RowIndex).Cells("TotalKilos").ReadOnly = True
            Else
                Grid.Rows(e.RowIndex).Cells("KilosXUnidad").ReadOnly = False
                Grid.Rows(e.RowIndex).Cells("TotalKilos").ReadOnly = False
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "TotalKilos" Then          'Recalcula KilosXUnidad segun TotalKilos. 
            If IsNothing(Grid.Rows(e.RowIndex).Cells("Totalkilos").Value) Then Grid.Rows(e.RowIndex).Cells("Totalkilos").Value = 0
            Grid.Rows(e.RowIndex).Cells("KilosXUnidad").Value = Trunca3(Grid.Rows(e.RowIndex).Cells("TotalKilos").Value / (Grid.Rows(e.RowIndex).Cells("Cantidad").Value - Grid.Rows(e.RowIndex).Cells("Devueltas").Value))
        End If

        Grid.Rows(e.RowIndex).Cells("TotalKilos").Value = Trunca3(Grid.Rows(e.RowIndex).Cells("KilosXUnidad").Value * (Grid.Rows(e.RowIndex).Cells("Cantidad").Value - Grid.Rows(e.RowIndex).Cells("Devueltas").Value))

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "KilosXUnidad" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloDecimales_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "TotalKilos" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloDecimales_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Precio" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloDecimales_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloDecimales_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub SoloDecimales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "KilosXUnidad" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Precio" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "TotalKilos" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If CType(sender, TextBox).Text = "" Then Exit Sub

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "KilosXUnidad" Then
            EsNumericoGridBox.Valida(CType(sender, TextBox), 4)
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "TotalKilos" Then
            EsNumericoGridBox.Valida(CType(sender, TextBox), 3)
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Precio" Then
            EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Grid.Rows(e.RowIndex).Cells("Articulo").Value <> 0 Then
                'aca Dim AGranelW As Boolean    'lo repito en EndEdit y Formatting para que prosece articulos loteados que vienen de PreFactura. 
                HallaAGranelYMedida(Grid.Rows(e.RowIndex).Cells("Articulo").Value, Grid.Rows(e.RowIndex).Cells("AGranel").Value, Grid.Rows(e.RowIndex).Cells("Medida").Value)
                Grid.Rows(e.RowIndex).Cells("Umedida").Value = HallaUMedidaArticulo(Grid.Rows(e.RowIndex).Cells("Articulo").Value)
            End If
        End If

        If IsNumeric(e.Value) Then
            If e.Value = 0 Then e.Value = Format(0, "#") : Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Grid.Rows(e.RowIndex).Cells("Articulo").Value = 0 Then Exit Sub
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

        If Grid.Columns(e.ColumnIndex).Name = "Precio" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Precio").Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else : e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "KilosXUnidad" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("KilosXUnidad").Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else : e.Value = Format(e.Value, "0.000")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "TotalKilos" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else : e.Value = Format(e.Value, "0.000")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Devueltos" Or Grid.Columns(e.ColumnIndex).Name = "Aumentos" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
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

        IndiceW = IndiceW + 1
        e.Row("Remito") = PRemito
        e.Row("Indice") = IndiceW
        e.Row("Articulo") = 0
        e.Row("Cantidad") = 0
        e.Row("KilosXUnidad") = 0
        e.Row("Devueltas") = 0
        e.Row("Precio") = 0
        e.Row("TipoPrecio") = 0

    End Sub
    Private Sub Dtdetalle_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Articulo") Then
            If Not IsDBNull(e.Row("Articulo")) Then
                If e.Row("Articulo") <> e.ProposedValue Then
                    e.Row("KilosXUnidad") = HallaKilosXUnidad(e.ProposedValue)
                    AnalizaArticuloConTipoPrecio(Lista, e.ProposedValue, e.Row("KilosXUnidad"), e.Row("Cantidad"), e.Row("Precio"), e.Row("TipoPrecio"))
                    Grid.Refresh()
                End If
            End If
        End If

        If e.Column.ColumnName.Equals("KilosXUnidad") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Precio") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Cantidad") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("TipoPrecio") Then
            If IsNothing(e.ProposedValue) Then e.ProposedValue = 0
        End If

    End Sub
    Private Sub Dtdetalle_ColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Articulo") Then
            If PRemito <> 0 And e.ProposedValue <> 0 And DtCabeza.Rows(0).Item("Pedido") <> 0 Then
                Dim Cantidad As Decimal
                Dim Entregada As Decimal
                Dim Precio As Decimal
                Dim TipoPrecio As Decimal
                Dim ArticuloExisteEnPedido As Boolean
                HallaCantidadYPrecioPedido(DtCabeza.Rows(0).Item("Pedido"), e.ProposedValue, Cantidad, Entregada, Precio, TipoPrecio, ArticuloExisteEnPedido)
                If Cantidad = 0 Then
                    MsgBox("Articulo no Existe en el Pedido.", MsgBoxStyle.Information)
                    Dim Row As DataRowView = bs.Current
                    Row.Delete()
                    Exit Sub
                End If
            End If
        End If

        If e.Column.ColumnName.Equals("Articulo") Then
            If e.ProposedValue <> 0 Then
                If EsArticulosLoteados Then Exit Sub
                If EsAutoImpreso Then
                    If DtDetalle.Rows.Count + 1 > 2 * GLineasRemitos Then
                        MsgBox("Supera Cantidad Articulos Permitidos (" & 2 * GLineasRemitos & ").", , MsgBoxStyle.Critical)
                        Dim Row As DataRowView = bs.Current
                        Row.Delete()
                        Exit Sub
                    Else
                        Exit Sub
                    End If
                End If
                If DtDetalle.Rows.Count + 1 > GLineasRemitos And Not PermiteMuchosArticulos Then
                    If MsgBox("Supera Cantidad Articulos Permitidos (" & GLineasRemitos & "). Si Continua No Podra Imprimirse. Desea Continuar?", MsgBoxStyle.YesNo, MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                        PermiteMuchosArticulos = True
                    Else
                        Dim Row As DataRowView = bs.Current
                        Row.Delete()
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub DtDetalle_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)
        '  Dim Indice As Integer = DtDetalle.Rows.IndexOf(e.Row)

        'Borra las lineas en blancos que aparecen cuando hago click en el ultimo renglon y sin informar nada regreso a algun renglon anterior. 

        If e.Row("Articulo") = 0 And e.Row("Cantidad") = 0 Then e.Row.Delete()

    End Sub





End Class