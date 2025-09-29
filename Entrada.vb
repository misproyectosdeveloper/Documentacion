Imports System.Xml
Public Class Entrada
    Dim Servidor As String
    Dim ClaveUsuario As Integer
    Dim ExisteDatosEmpresa As Boolean = True
    Private Sub Entrar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '-----------------Ejemplo: la suma de dos numeros con dos digitos da varios decimales.
        'Dim C As Double = 0
        'C = C + 683.02
        'C = C + 1388.55
        '-------------------------------------------------------------------------------------
        ' Dim a As Double = 683.02
        ' Dim b As Double = 1388.55
        ' Dim c As Decimal
        ' c = a + b
        CambiarPuntoDecimal(",")

        If Environment.GetCommandLineArgs.Length > 1 Then
            Servidor = Environment.GetCommandLineArgs(1)
            GClaveEmpresa = Environment.GetCommandLineArgs(2)
            ClaveUsuario = Environment.GetCommandLineArgs(3)
            PermisoTotal = Environment.GetCommandLineArgs(4)
            GTipoLicencia = Environment.GetCommandLineArgs(5)
        Else
            CargarPorParametros()
        End If

        GServidor = Servidor

        PreparaInicio(Servidor, GClaveEmpresa, ClaveUsuario)

        If GFaltaDatosEmpresa Then
            DesHabilitarMenu()
            MenuTablas.Enabled = True
            Exit Sub
        End If

        MenuStrip1.Enabled = False

        ButtonEntrar_Click(Nothing, Nothing)

        CerrarAutomaticamentePedidos()

        GExcepcion = HallaDatoGenerico("SELECT Clave FROM Proveedores WHERE EsEgresoCaja = 1;", Conexion, GProveedorEgresoCaja)
        If Not IsNothing(GExcepcion) Then
            MsgBox("Error al Leer Tabla: Proveedores." + vbCrLf + vbCrLf + GExcepcion.Message)
            Me.Close() : Exit Sub
        End If

        Me.Text = Me.Text & "                      Ultima Actualización: 15/02/2024    #150"

        '    MsgBox("Sacar")    para probar sin Base negra.
        '    PermisoTotal = False
        '    ConexionN = ""

    End Sub
    Private Sub Entrada_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        End

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonEntrar_Click(Nothing, Nothing)

        If e.KeyData = 112 Then
            If Button2.Visible = False Then
                Button2.Visible = True
                ' Button3.Visible = True
                ' Button6.Visible = True
                ' Button5.Visible = True
            Else
                Button2.Visible = False
                '  Button3.Visible = False
                ' Button6.Visible = False
                ' Button5.Visible = False
            End If
        End If

        Exit Sub

        If e.KeyData = 112 Then
            If PideCaja <> 1953 Then Exit Sub
            If Button6.Visible = False Then
                ' Button2.Visible = True
                ' Button3.Visible = True
                Button6.Visible = True
                ' Button5.Visible = True
            Else
                '  Button2.Visible = False
                '  Button3.Visible = False
                Button6.Visible = False
                ' Button5.Visible = False
            End If
        End If

    End Sub
    Private Sub ButtonEntrar_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        If Not PermisoLectura(1) Then MenuClientes.Enabled = False
        If Not PermisoEscritura(1) Then
            MenuNuevoCliente.Enabled = False
        End If
        If Not PermisoEscritura(101) Then
            MenuNuevoCliente.Enabled = False
        End If
        '----------------------------------------Proveedores ------------------------------------------
        If Not PermisoLectura(2) Then MenuProveedores.Enabled = False
        If Not PermisoEscritura(2) Then
            MenuNuevoProveedor.Enabled = False
            MenuIngresoFactura.Enabled = False
            MenuRegistraFacturaImportacion.Enabled = False
            MenuLiquidacionConsignatarios.Enabled = False
            MenuEmitirNotasDEBITOFINANCIERAAProveedores.Enabled = False
            MenuEmitirNotasDEBITOFINANCIERAPProveedoresPorDiferenciaDeCambio.Enabled = False
            MenuEmitirNotasCREDITOFINANCIERAAProveedores.Enabled = False
            MenuEmitirNotasCREDITOFINANCIERAProveedoresPorDiferenciaDeCambio.Enabled = False
            MenuRegistrarNotasDebitosDelProveedor.Enabled = False
            MenuRegistrarNotasCREDITODelProveedor.Enabled = False
            MenuLiquidacionContable.Enabled = False
            MenuNuevoFondoFijo.Enabled = False
            MenuRendición.Enabled = False
            menuNuevaListaProveedores.Enabled = False
        End If
        ' -------------------------------------------------------------------------------------------------
        If Not PermisoLectura(3) Then MenuArticulos.Enabled = False
        '
        If Not PermisoLectura(4) Then MenuOtrosProveedores.Enabled = False
        If Not PermisoEscritura(4) Then
            MenuOtroProveedor.Enabled = False
            MenuNuevaOtraFactura.Enabled = False
        End If
        '
        If Not PermisoLectura(5) Then MenuStock.Enabled = False
        If Not PermisoEscritura(5) Then
            MenuNuevoRemito.Enabled = False
            MenuIngresoMercaderias.Enabled = False
            MenuTransferenciasMercaderias.Enabled = False
            MenuReprocesosLotes.Enabled = False
            MenuBajaPorDescarte.Enabled = False
            MenuCambioProveedorAUnIngreso.Enabled = False
            MenuInformarDiferencia.Enabled = False
        End If
        '
        If Not PermisoLectura(6) Then FacturacionToolStripMenuItem.Enabled = False
        If Not PermisoEscritura(6) Then
            MenuFactura.Enabled = False
            MenuNVLP.Enabled = False
            MenuNotaDebitoFinanciera.Enabled = False
            MenuEmitirNotasDEBITOFINANCIERAPorDiferenciaDeCambio.Enabled = False
            MenuNotaCreditoFinanciera.Enabled = False
            EmitirNotasCREDITOFINANCIERAPorDiferenciaDeCambio.Enabled = False
            MenuRegistrarNotasDebitoDelCliente.Enabled = False
            MenuRegistrarNotasCreditoDelCliente.Enabled = False
        End If
        If TieneClientesFCE() Then MenuListaSaldosFacturasFCE.Visible = True

        If Not PermisoLectura(8) Then TesoreriaToolStripMenuItem.Enabled = False 'Tesoreria
        If Not PermisoEscritura(8) Then
            MenuReciboCobro.Enabled = False
            MenuRegistrarCobranzaManual.Enabled = False
            MenuOrdenDePago.Enabled = False
            ToolStripMenuItem3.Enabled = False
            MenuOrdenPagoEquilibradora.Enabled = False
            MenuDevolucionSeña.Enabled = False
            MenuOtroProveedores.Enabled = False
            MenuExtracciones.Enabled = False
            MenuDepositosBancarios.Enabled = False
            MenuTransferenciasCuentasPropias.Enabled = False
            MenuReemplazoChequesPropios.Enabled = False
            MenuRechazoDeCheques.Enabled = False
            IngresoDeValesRecupero.Enabled = False
            MenuAjusteFondoFijo.Enabled = False
            MenuReposición.Enabled = False
        End If
        '
        If Not PermisoLectura(9) Then SueldosToolStripMenuItem.Enabled = False
        If Not PermisoEscritura(9) Then
            MenuNuevoEmpleado.Enabled = False
            MenuRecibosDeSueldo.Enabled = False
            MenuOrdenDePagoSueldos.Enabled = False
        End If
        '
        If Not PermisoLectura(10) Then ComercialToolStripMenuItem.Enabled = False
        If Not PermisoEscritura(10) Then
            MenuNuevaLista.Enabled = False
        End If
        '
        If Not PermisoLectura(11) Then CuentasContableToolStripMenuItem.Enabled = False
        If Not PermisoEscritura(11) Then
            MenuAsientosManuales.Enabled = False
        End If
        '
        If Not PermisoLectura(12) Then MenuInsumosToolStripMenuItem.Enabled = False
        If Not PermisoEscritura(12) Then
            ''''''''''  MenuOrdenDeCompra.Enabled = False
            MenuRecepcion.Enabled = False
            MenuTransferenciaDeInsumo.Enabled = False
            MenuConsumoDeInsumos.Enabled = False
        End If
        '
        If Not PermisoLectura(13) Then ControlDeGestionToolStripMenuItem.Enabled = False
        If Not PermisoEscritura(13) Then
        End If
        '
        If Not PermisoLectura(14) Then InformesToolStripMenuItem.Enabled = False
        If Not PermisoEscritura(14) Then
        End If
        '
        If Not PermisoEscritura(15) Then
            UnaRetencion.PBloqueaFunciones = True
            ListaBancos.PBloqueaFunciones = True
            UnCosteo.PBloqueaFunciones = True
            UnaTablaCajas.PBloqueaFunciones = True
        End If
        '
        'Permisos sobre Tablas y sistema.
        If Not GAdministrador Then
            MenuSistema.Enabled = False
        End If
        If Not GAdministrador And Not PermisoLectura(15) And Not PermisoEscritura(15) Then
            MenuEspecies.Enabled = False
            MenuVariedades.Enabled = False
            MenuMarcas.Enabled = False
            MenuCategorias.Enabled = False
            MenuEnvases.Enabled = False
            MenuCalibres.Enabled = False
            MenuDatosDeLaEmpresa.Enabled = False
            MenuPuntosDeVenta.Enabled = False
            MenuDepositos.Enabled = False
            MenuDepositosInsumos.Enabled = False
            MenuCanalDeVenta.Enabled = False
            MenuPaises.Enabled = False
            MenuProvincias.Enabled = False
            MenuAlicuotasParaLiqProveedores.Enabled = False
            MenuRetenciónes2.Enabled = False
            MenuBancos11.Enabled = False
            MenuMonedas.Enabled = False
            MenuConceptosDeGastos.Enabled = False
            CosteosToolStripMenuItem.Enabled = False
            MenuConceptosPrestamos.Enabled = False
            MenuGastosBancarios.Enabled = False
            MenuConceptosNetosParaSueldos.Enabled = False
            MenuVendedoresYTransportes.Enabled = False
            MenuTiposDePagoOtrosProveedores.Enabled = False
            MenuListaInsumos.Enabled = True
            MenuINCOTERM.Enabled = False
            SaldosInicialesDeCajasToolStripMenuItem.Enabled = False
            menuMaestroFondosFijo.Enabled = False
            MenuArticulosLogisticosIFCO.Enabled = False
            MenuZonasParaListaDePrecios.Enabled = False
        End If

        If PermisoEscritura(2) Then             'Proveedores
            '  MenuCanalDeVenta.Enabled = True
            ' MenuConceptosDeGastos.Enabled = True
        End If
        If PermisoEscritura(3) Then             'Articulo
            '    MenuEspecies.Enabled = True
            '       MenuVariedades.Enabled = True
            '        MenuMarcas.Enabled = True
            '        MenuCategorias.Enabled = True
            '        MenuEnvases.Enabled = True
            '        MenuCalibres.Enabled = True
        End If
        If PermisoEscritura(4) Then             'Otros Proveedores
            '    MenuTiposDePagoOtrosProveedores.Enabled = True
        End If
        If PermisoEscritura(8) Then             'Tesoreria
            '   MenuConceptosPrestamos.Enabled = True
            '    MenuGastosBancarios.Enabled = True
            '    SaldosInicialesDeCajasToolStripMenuItem.Enabled = True
        End If
        If PermisoEscritura(9) Then             'Sueldos
            '    MenuConceptosNetosParaSueldos.Enabled = True
        End If
        If Not PermisoLectura(110) Then             'Cierre Periodo Contable
            MenuCierreContable.Enabled = False
        End If
        UnaTablaCierreContable.PBloqueaFunciones = Not PermisoEscritura(110)
        '
        If PermisoTotal Then
            MenuFacturaEquilibradora.Visible = True
            MenuOrdenPagoEquilibradora.Visible = True
            MenuOrdenPagoEspecial.Visible = True
            MenuLiquidacionContable.Visible = True
            MenuAsientoFacturaContable.Visible = True
            MenuAsientoLiquidacionContable.Visible = True
            MenuOrdenPagoContable.Visible = True
            MenuFacturaContable.Visible = True
            MenuFacturaVentaContable.Visible = True
            MenuUnaNVLPContable.Visible = True
            MenuAsientoNVLPContable.Visible = True
            MenuRegistrarNotasCREDITOContableDelProveedor.Visible = True
            MenuEmitirNotaCreditoContableDelProveedor.Visible = True
            MenuEmitirNotaCREDITOSContableACliente.Visible = True
            MenuEmitirNotasCREDITOFINANCIERACONTABLEACliente.Visible = True
            MenuOrdenPagoContable.Visible = True
            CobranzaContable.Visible = True
        Else : MenuFacturaEquilibradora.Visible = False
            MenuEmitirNotasCREDITOFINANCIERACONTABLEACliente.Visible = True
            MenuOrdenPagoEquilibradora.Visible = False
            MenuOrdenPagoEspecial.Visible = False
            MenuLiquidacionContable.Visible = False
            MenuAsientoFacturaContable.Visible = False
            MenuAsientoLiquidacionContable.Visible = False
            MenuOrdenPagoContable.Visible = False
            MenuFacturaContable.Visible = False
            MenuFacturaVentaContable.Visible = False
            MenuUnaNVLPContable.Visible = False
            MenuAsientoNVLPContable.Visible = False
            MenuRegistrarNotasCREDITOContableDelProveedor.Visible = False
            MenuEmitirNotaCreditoContableDelProveedor.Visible = False
            MenuEmitirNotasCREDITOFINANCIERACONTABLEACliente.Visible = False
            MenuEmitirNotaCREDITOSContableACliente.Visible = False
            MenuOrdenPagoContable.Visible = False
            CobranzaContable.Visible = False
        End If

        MenuStrip1.Enabled = True

    End Sub
    Private Sub CargarPorParametros()

        Dim Directorio As String = System.AppDomain.CurrentDomain.BaseDirectory()

        Dim reader As XmlTextReader = New XmlTextReader(Directorio & "\Parametros.xml")
        Do While (reader.Read())
            If reader.Name = "Servidor" Then
                Servidor = reader.ReadString
            End If
        Loop
        reader.Close()
        GTipoLicencia = "U"

        ClaveUsuario = 1 '1 '14 '6   '7              
        GClaveUsuario = ClaveUsuario
        GClaveEmpresa = 1 '59
        PermisoTotal = True

        '-----------------------------------------------------------------------------------------------------------
        ' Para Homologacion (prueba)  usar GClaveEmpresa = 9 usuario: hmologacion  Punto de venta: 12
        '-----------------------------------------------------------------------------------------------------------

        'Scomer: 9     Admin: 3   Hugo = 6 Prueba1=14  www:7  ddd:13   homologacion = 15 (usar con punto venta 12)       
        'Fruit Pack: 2 Admin: 3
        'Patagonia: 1 Admin: 1
        'Cadesi: 7 Admin: 1
        'MC: 8     Admin:1
        'Cuadro norte: 13  adminiustrador: 1 Mortera:6
        'Vegetal = 31

    End Sub
    Private Sub MenuDatosDeLaEmpresa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDatosDeLaEmpresa.Click

        If Not FormularioOK(DatosEmpresa) Then Exit Sub

        DatosEmpresa.PBloqueaFunciones = Not PermisoEscritura(15)
        DatosEmpresa.Show()

    End Sub
    Private Sub MenuEspecies_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuEspecies.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 1
        UnaTabla.Show()

    End Sub
    Private Sub MenuVariedades_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuVariedades.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 2
        UnaTabla.Show()

    End Sub
    Private Sub MenuMarcas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuMarcas.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 3
        UnaTabla.Show()

    End Sub
    Private Sub MenuCategorias_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuCategorias.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 4
        UnaTabla.Show()

    End Sub
    Private Sub MenuEnvases_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuEnvases.Click

        UnaTablaEnvases.Show()

    End Sub
    Private Sub MenuArticulos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuArticulos.Click

        If Not PermisoLectura(3) Then Exit Sub

        If Not FormularioOK(ListaArticulos) Then Exit Sub

        ListaArticulos.ShowDialog()
        ListaArticulos.Dispose()

    End Sub
    Private Sub MenuCalibres_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuCalibres.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 5
        UnaTabla.Show()

    End Sub
    Private Sub TextPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If e.KeyChar = " " Then e.KeyChar = ""

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Lotes.Show()
    End Sub
    Private Sub MenuIngresoMercaderias_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuIngresoMercaderias.Click

        If Not FormularioOK(UnIngresoMercaderia) Then Exit Sub

        UnIngresoMercaderia.Show()

    End Sub
    Private Sub MenuTransferenciasMercaderias_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuTransferenciasMercaderias.Click

        UnaTransferencia.ShowDialog()
        UnaTransferencia.Dispose()

    End Sub
    Private Sub MenuListaTransferencias_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaTransferencias.Click

        If Not FormularioOK(ListaTransferencias) Then Exit Sub

        ListaTransferencias.Show()

    End Sub
    Private Sub MenuListaIngresoMercaderias_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuListaIngresoMercaderias.Click

        If Not FormularioOK(ListaIngresoMercaderias) Then Exit Sub

        ListaIngresoMercaderias.PBloqueaFunciones = Not PermisoEscritura(5)
        ListaIngresoMercaderias.Show()

    End Sub
    Private Sub MenuTrasabilidadDeLotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuTrasabilidadDeLotes.Click

        If Not PermisoLectura(5) Then MsgBox("No Autorizado") : Exit Sub

        If TrasabilidadLotes.WindowState = FormWindowState.Minimized Then TrasabilidadLotes.WindowState = FormWindowState.Normal : Exit Sub
        If TrasabilidadLotes.Visible Then MsgBox("Formulario Ya esta Abierto") : Exit Sub

        TrasabilidadLotes.Show()

    End Sub
    Private Sub MenuIngresoCliente_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        UnCliente.Show()

    End Sub
    Private Sub MenuListaRemitos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuListaRemitos.Click

        If Not FormularioOK(ListaRemitos) Then Exit Sub

        ListaRemitos.PBloqueaFunciones = Not PermisoEscritura(5)

        ListaRemitos.Show()

    End Sub
    Private Sub MenuListaDevoluciones_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuListaDevoluciones.Click

        If Not FormularioOK(ListaDevoluciones) Then Exit Sub

        ListaDevoluciones.ShowDialog()
        ListaDevoluciones.Dispose()

    End Sub
    Private Sub MenuListaFacturasNormales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaFacturasNormales.Click

        If ListaFacturas.WindowState = FormWindowState.Minimized Then ListaFacturas.WindowState = FormWindowState.Normal : Exit Sub
        If ListaFacturas.Visible Then MsgBox("Formulario Ya esta Abierto") : Exit Sub

        ListaFacturas.PBloqueaFunciones = Not PermisoEscritura(6)
        ListaFacturas.Show()

    End Sub
    Private Sub MenuListaSaldosFacturasFCE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaSaldosFacturasFCE.Click

        If ListaFacturasFCE.WindowState = FormWindowState.Minimized Then ListaFacturasFCE.WindowState = FormWindowState.Normal : Exit Sub
        If ListaFacturasFCE.Visible Then MsgBox("Formulario Ya esta Abierto") : Exit Sub

        ListaFacturasFCE.Show()

    End Sub
    Private Sub MenuNuevoRemito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNuevoRemito.Click

        If Not FormularioOK(UnRemito) Then Exit Sub

        UnRemito.PRemito = 0
        UnRemito.Show()

    End Sub
    Private Sub MenuListaNumerosFaltantesDeRemitos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaNumerosFaltantesDeRemitos.Click

        If Not FormularioOK(ListaRecibosFaltantes) Then Exit Sub

        ListaRecibosFaltantes.PTipo = 1000
        ListaRecibosFaltantes.ShowDialog()

    End Sub
    Private Sub MenuFacturasPendietes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturasPendietes.Click

        If ListaFacturas.WindowState = FormWindowState.Minimized Then UnRemito.WindowState = FormWindowState.Normal : Exit Sub
        If ListaFacturas.Visible Then MsgBox("Formulario Ya esta Abierto") : Exit Sub

        ListaFacturas.PBloqueaFunciones = Not PermisoEscritura(6)
        ListaFacturas.PSoloPendientes = True
        ListaFacturas.Show()

    End Sub
    Private Sub MenuNVLP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNVLP.Click

        If Not PermisoEscritura(6) Then MsgBox("No Autorizado") : Exit Sub

        If Not FormularioOK(UnaNVLP) Then Exit Sub

        UnaNVLP.PLiquidacion = 0
        UnaNVLP.ShowDialog()
        UnaNVLP.Dispose()

    End Sub
    Private Sub MenuListaNVLP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaNVLP.Click

        If Not FormularioOK(ListaNVLP) Then Exit Sub

        ListaNVLP.ShowDialog()
        ListaNVLP.Dispose()

    End Sub
    Private Sub MenuFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFactura.Click

        If Not FormularioOK(UnaFactura) Then Exit Sub

        UnaFactura.PFactura = 0
        UnaFactura.Show()

    End Sub
    Private Sub MenuFacturaContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturaContable.Click

        If Not FormularioOK(UnaFactura) Then Exit Sub

        UnaFactura.PFactura = 0
        UnaFactura.PEsContable = True
        UnaFactura.Show()

    End Sub
    Private Sub MenuUnaNVLPContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuUnaNVLPContable.Click

        If Not FormularioOK(UnaNVLPContable) Then Exit Sub

        UnaNVLPContable.PLiquidacion = 0
        UnaNVLPContable.ShowDialog()
        UnaNVLPContable.Dispose()

    End Sub
    Private Sub MenuRegistrarFacturaExportacionFacturaElectronica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If UnaFactura.Visible Then MsgBox("Formulario Ya esta Abierto") : Exit Sub

        UnaFactura.PFactura = 0
        UnaFactura.ShowDialog()

    End Sub
    Private Sub MenuListaNotasCredito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaNotasCredito.Click

        If ListaNotasCredito.WindowState = FormWindowState.Minimized Then ListaNotasCredito.WindowState = FormWindowState.Normal : Exit Sub
        If ListaNotasCredito.Visible Then MsgBox("Formulario Ya esta Abierto") : Exit Sub

        ListaNotasCredito.Show()

    End Sub
    Private Sub MenuListaFinancieros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaNotasFinancieras.Click

        If Not FormularioOK(ListaNotasTerceros) Then Exit Sub

        ListaNotasTerceros.PTipoEmisor = 1
        ListaNotasTerceros.PEsFinanciera = True
        ListaNotasTerceros.Show()

    End Sub
    Private Sub RegistrarNotasDebitoDelCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRegistrarNotasDebitoDelCliente.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PTipoNota = 50
        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub MenuRegistrarNotasCreditoDelCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRegistrarNotasCreditoDelCliente.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PTipoNota = 70
        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub MenuListaCierreFacturaExportacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaCierreFacturaExportacion.Click

        If Not PermisoLectura(6) Then MsgBox("No Autorizado") : Exit Sub

        If Not FormularioOK(ListaCierreFactura) Then Exit Sub

        ListaCierreFactura.Show()

    End Sub
    Private Sub MenuFacturasFaltantes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturasFaltantes.Click

        If Not FormularioOK(ListaRecibosFaltantes) Then Exit Sub

        ListaRecibosFaltantes.PTipo = 7
        ListaRecibosFaltantes.ShowDialog()

    End Sub
    Private Sub MenuNotasDeCreditoFaltantes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotasDeCreditoFaltantes.Click

        If Not FormularioOK(ListaRecibosFaltantes) Then Exit Sub

        ListaRecibosFaltantes.PTipo = 2
        ListaRecibosFaltantes.ShowDialog()

    End Sub
    Private Sub MenuNotasDeDebitoFaltantes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotasDeDebitoFaltantes.Click

        If Not FormularioOK(ListaRecibosFaltantes) Then Exit Sub

        ListaRecibosFaltantes.PTipo = 5
        ListaRecibosFaltantes.ShowDialog()

    End Sub
    Private Sub MenuNuevaLista_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNuevaLista.Click

        If Not FormularioOK(UnaListaDePrecios) Then Exit Sub

        UnaListaDePrecios.ShowDialog()
        UnaListaDePrecios.Dispose()

    End Sub
    Private Sub MenuListadoDeListaDePrecios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListadoDeListaDePrecios.Click

        If Not FormularioOK(ListaListaDePrecios) Then Exit Sub

        ListaListaDePrecios.ShowDialog()
        ListaListaDePrecios.Dispose()

    End Sub
    Private Sub MenuListadoDeListaDePreciosPorVendedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListadoDeListaDePreciosPorVendedor.Click

        If Not FormularioOK(ListaListaDePrecios) Then Exit Sub

        ListaListaDePrecios.PEsVendedor = True
        ListaListaDePrecios.ShowDialog()
        ListaListaDePrecios.Dispose()

    End Sub
    Private Sub MenuNuevoPedido_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNuevoPedido.Click

        If Not FormularioOK(UnPedido) Then Exit Sub

        UnPedido.PPedido = 0
        UnPedido.Show()

    End Sub
    Private Sub MenuListadoDePedidos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListadoDePedidos.Click

        If Not FormularioOK(ListaPedidos) Then Exit Sub

        ListaPedidos.Show()

    End Sub
    Private Sub MenuExxelPedidosPorFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuExxelPedidosPorFecha.Click

        OpcionPedidos.PEsInforme = True
        OpcionPedidos.ShowDialog()
        If OpcionPedidos.PRegresar Then
            OpcionPedidos.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformePedidosEntreFechas(OpcionPedidos.PCliente, OpcionPedidos.PFechaEntregaDesde, OpcionPedidos.PFechaEntregaHasta, OpcionPedidos.PConSaldoPositivo, OpcionPedidos.PEsAbierto, OpcionPedidos.PEsCerrado, OpcionPedidos.PEsConRepeticion)

        OpcionPedidos.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuExxelResumenPedidoDeArticulosPorFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuExxelResumenPedidoDeArticulosPorFecha.Click

        OpcionPedidos.PEsResumenPorArticulo = True
        OpcionPedidos.ShowDialog()
        If OpcionPedidos.PRegresar Then
            OpcionPedidos.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeResumenArticulosPedidosPorFecha(OpcionPedidos.PCliente, OpcionPedidos.PFechaEntregaDesde, OpcionPedidos.PFechaEntregaHasta)

        OpcionPedidos.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuNuevoCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNuevoCliente.Click

        If Not FormularioOK(UnCliente) Then Exit Sub

        UnCliente.PCliente = 0
        UnCliente.ShowDialog()
        UnCliente.Dispose()

    End Sub
    Private Sub MenuNuevoClienteDeOperacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNuevoClienteDeOperacion.Click

        If Not FormularioOK(UnCliente) Then Exit Sub

        UnCliente.PDeOperacion = True
        UnCliente.PCliente = 0
        UnCliente.ShowDialog()
        UnCliente.Dispose()

    End Sub
    Private Sub MenuListadoClientes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListadoClientes.Click

        If Not FormularioOK(ListaClientes) Then Exit Sub

        ListaClientes.Show()

    End Sub
    Private Sub MenuCuentaCorriente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCuentaCorriente.Click

        If Not FormularioOK(UnaCtaCte) Then Exit Sub

        UnaCtaCte.PTipoEmisor = 1
        UnaCtaCte.Show()

    End Sub
    Private Sub MenuNuevoProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNuevoProveedor.Click

        If Not FormularioOK(UnProveedor) Then Exit Sub

        UnProveedor.PProveedor = 0
        UnProveedor.ShowDialog()
        UnProveedor.Dispose()

    End Sub
    Private Sub MenuListadoProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListadoProveedor.Click

        If Not FormularioOK(ListaProveedores) Then Exit Sub

        ListaProveedores.ShowDialog()
        ListaProveedores.Dispose()

    End Sub
    Private Sub MenuListadoFacturas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListadoFacturas.Click

        If Not FormularioOK(ListaFacturasProveedor) Then Exit Sub

        ListaFacturasProveedor.Show()

    End Sub
    Private Sub MenuListadoFacturasAfectaLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListadoFacturasAfectaLotes.Click

        If Not FormularioOK(ListaLotesFacturasProveedorAfectaLotes) Then Exit Sub

        ListaLotesFacturasProveedorAfectaLotes.Show()

    End Sub
    Private Sub MenuListaNumerosFaltantesDeLiquidaciones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaNumerosFaltantesDeLiquidaciones.Click

        If Not FormularioOK(ListaRecibosFaltantes) Then Exit Sub

        ListaRecibosFaltantes.PTipo = 2000
        ListaRecibosFaltantes.Show()

    End Sub
    Private Sub MenuLiquidacionContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuLiquidacionContable.Click

        If Not PermisoEscritura(2) Then MsgBox("No Autorizado") : Exit Sub

        If Not FormularioOK(UnaLiquidacionContable) Then Exit Sub

        UnaLiquidacionContable.PLiquidacion = 0
        UnaLiquidacionContable.PAbierto = True
        UnaLiquidacionContable.ShowDialog()
        UnaLiquidacionContable.Dispose()

    End Sub
    Private Sub MenuNuevoFondoFijo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNuevoFondoFijo.Click

        If Not FormularioOK(UnFondoFijo) Then Exit Sub

        UnFondoFijo.PNumero = 0
        UnFondoFijo.ShowDialog()
        UnFondoFijo.Dispose()

    End Sub
    Private Sub MenuListaFondosFijos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaFondosFijos.Click

        If Not FormularioOK(ListaFondosFijos) Then Exit Sub

        ListaFondosFijos.Show()

    End Sub
    Private Sub MenuRendición_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRendición.Click

        If Not FormularioOK(UnaRendicion) Then Exit Sub

        UnaRendicion.PRendicion = 0
        UnaRendicion.ShowDialog()
        UnaRendicion.Dispose()

    End Sub
    Private Sub ListaDeRendiciones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListaDeRendiciones.Click

        If Not FormularioOK(ListaRendiciones) Then Exit Sub

        ListaRendiciones.Show()

    End Sub
    Private Sub MenuCuentasCorrientesFondosFijo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCuentasCorrientesFondosFijo.Click

        If Not FormularioOK(ListaSaldosFondosFijoDetalle) Then Exit Sub

        ListaSaldosFondosFijoDetalle.ShowDialog()
        ListaSaldosFondosFijoDetalle.Dispose()

    End Sub
    Private Sub MenuTrazabilidadPorLotesProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTrazabilidadPorLotesProveedores.Click

        If TrasabilidadLotes.WindowState = FormWindowState.Minimized Then TrasabilidadLotes.WindowState = FormWindowState.Normal : Exit Sub
        If TrasabilidadLotes.Visible Then MsgBox("Formulario Ya esta Abierto") : Exit Sub

        TrasabilidadLotes.Show()

    End Sub
    Private Sub menuNuevaListaProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuNuevaListaProveedores.Click

        If Not FormularioOK(UnaListaDePreciosProveedores) Then Exit Sub

        UnaListaDePreciosProveedores.ShowDialog()
        UnaListaDePreciosProveedores.Dispose()

    End Sub
    Private Sub menuListaListaDePreciosProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuListaListaDePreciosProveedores.Click

        If Not FormularioOK(ListaListaDePreciosProveedores) Then Exit Sub

        ListaListaDePreciosProveedores.ShowDialog()
        ListaListaDePreciosProveedores.Dispose()

    End Sub
    Private Sub MenuInformeDetalleComprobanresQueAfectanLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuInformeDetalleComprobanresQueAfectanLotes.Click

        OpcionInformeComprobantesQueAfectaALotes.ShowDialog()
        If OpcionInformeComprobantesQueAfectaALotes.PRegresar Then
            OpcionInformeComprobantesQueAfectaALotes.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeDetalleComprobantesQueAfectanLotes(OpcionInformeComprobantesQueAfectaALotes.PProveedor, OpcionInformeComprobantesQueAfectaALotes.PLote, OpcionInformeComprobantesQueAfectaALotes.PSecuencia, OpcionInformeComprobantesQueAfectaALotes.PDesde, OpcionInformeComprobantesQueAfectaALotes.PHasta, OpcionInformeComprobantesQueAfectaALotes.PCosteo, OpcionInformeComprobantesQueAfectaALotes.PAbierto, OpcionInformeComprobantesQueAfectaALotes.PCerrado, OpcionInformeComprobantesQueAfectaALotes.PConIva, OpcionInformeComprobantesQueAfectaALotes.PSinIva)

        OpcionInformeComprobantesQueAfectaALotes.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuCuentaCorrienteProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCuentaCorrienteProveedor.Click

        If Not FormularioOK(UnaCtaCte) Then Exit Sub

        UnaCtaCte.PTipoEmisor = 2
        UnaCtaCte.Show()

    End Sub
    Private Sub MenuNotaDebitoFinanciera_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotaDebitoFinanciera.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.PTipoNota = 5
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub MenuEmitirNotasDEBITOFINANCIERAPorDiferenciaDeCambio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEmitirNotasDEBITOFINANCIERAPorDiferenciaDeCambio.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PDiferenciaDeCambio = True
        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.PTipoNota = 5
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub MenuNotaCreditoFinanciera_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotaCreditoFinanciera.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.PTipoNota = 7
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub MenuEmitirNotasCREDITOFINANCIERACONTABLEACliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEmitirNotasCREDITOFINANCIERACONTABLEACliente.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.PTipoNota = 7
        UnReciboDebitoCredito.PTr = True
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub ImputarSaldosIniciales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuImputarSaldosIniciales.Click

        If Not FormularioOK(UnaImputacionSaldosIniciales) Then Exit Sub

        UnaImputacionSaldosIniciales.PTipoNota = 7
        UnaImputacionSaldosIniciales.Show()

    End Sub
    Private Sub EmitirNotasCREDITOFINANCIERAPorDiferenciaDeCambio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EmitirNotasCREDITOFINANCIERAPorDiferenciaDeCambio.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PDiferenciaDeCambio = True
        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.PTipoNota = 7
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub MenuListaLiquidaciones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaLiquidaciones.Click

        If Not FormularioOK(ListaLiquidaciones) Then Exit Sub

        ListaLiquidaciones.Show()

    End Sub
    Private Sub MenuListaDevolucionDeLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaDevolucionDeLotes.Click

        If Not FormularioOK(ListaDevolucionLotes) Then Exit Sub

        ListaDevolucionLotes.Show()

    End Sub
    Private Sub MenuBajaPorDescarte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuBajaPorDescarte.Click

        If Not FormularioOK(UnDescarte) Then Exit Sub

        UnDescarte.Show()

    End Sub
    Private Sub MenuListaDescartes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaDescartes.Click

        If Not FormularioOK(ListaDescartes) Then Exit Sub

        ListaDescartes.Show()

    End Sub
    Private Sub MenuCambioProveedorAUnIngreso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCambioProveedorAUnIngreso.Click

        If Not FormularioOK(UnCambioProveedorIngresoMercaderia) Then Exit Sub

        UnCambioProveedorIngresoMercaderia.Show()

    End Sub
    Private Sub MenuConsumoLotesProdTerminadosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuConsumoLotesProdTerminadosToolStripMenuItem.Click

        If Not FormularioOK(UnConsumoPT) Then Exit Sub

        UnConsumoPT.Show()

    End Sub
    Private Sub MenuListaConsumoLotesProdTerminados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaConsumoLotesProdTerminados.Click

        If Not FormularioOK(ListaConsumosPT) Then Exit Sub

        ListaConsumosPT.Show()

    End Sub
    Private Sub MenuReprocesosLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuReprocesosLotes.Click

        If Not FormularioOK(UnReprocesoLotesNuevo) Then Exit Sub

        UnReprocesoLotesNuevo.PReproceso = 0
        UnReprocesoLotesNuevo.Show()

    End Sub
    Private Sub MenuListadoNotasCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListadoNotasCliente.Click

        If Not FormularioOK(ListaNotasTerceros) Then Exit Sub

        ListaNotasTerceros.PTipoEmisor = 1
        ListaNotasTerceros.Show()

    End Sub
    Private Sub MenuListadoNotasProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListadoNotasProveedor.Click

        If Not FormularioOK(ListaNotasTerceros) Then Exit Sub

        ListaNotasTerceros.PTipoEmisor = 2
        ListaNotasTerceros.Show()

    End Sub
    Private Sub MenuStockDeLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuStockDeLotes.Click

        If Not PermisoLectura(5) Then MsgBox("No Autorizado") : Exit Sub

        If Not FormularioOK(ListaStockLotes) Then Exit Sub

        ListaStockLotes.Show()

    End Sub
    Private Sub MenuInformarDiferencia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuInformarDiferencia.Click

        If Not FormularioOK(UnaDiferenciaInventario) Then Exit Sub

        UnaDiferenciaInventario.Show()

    End Sub
    Private Sub ListaComprobantesDiferencias_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListaComprobantesDiferencias.Click

        If Not PermisoLectura(5) Then MsgBox("No Autorizado") : Exit Sub

        If Not FormularioOK(ListaDiferenciaInventario) Then Exit Sub

        ListaDiferenciaInventario.Show()

    End Sub
    Private Sub MenuDepositosInsumos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDepositosInsumos.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 20
        UnaTabla.Show()

    End Sub
    Private Sub MenuDepositos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDepositos.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 19
        UnaTabla.Show()

    End Sub
    Private Sub MenuCentrosDeCosto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCentrosDeCosto.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 30
        UnaTabla.Show()

    End Sub
    Private Sub MenuListaDeNegocios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNuevoCosteo.Click

        If Not FormularioOK(UnCosteo) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnCosteo.ShowDialog()
        UnCosteo.Dispose()

    End Sub
    Private Sub MenuListaCosteos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaCosteos.Click

        If Not FormularioOK(ListaNegociosYCosteos) Then Exit Sub

        ListaNegociosYCosteos.Show()

    End Sub
    Private Sub MenuBancos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuBancos.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 26
        UnaTabla.Show()

    End Sub
    Private Sub Menurecepcion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRecepcion.Click

        If Not FormularioOK(UnaRecepcion) Then Exit Sub

        UnaRecepcion.ShowDialog()
        UnaRecepcion.Dispose()

    End Sub
    Private Sub MenuListaStock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaStock.Click

        If Not FormularioOK(ListaStockInsumos) Then Exit Sub

        ListaStockInsumos.Show()

    End Sub
    Private Sub MenuInformeSeguimientoOrdenCompra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuInformeSeguimientoOrdenCompra.Click

        OpcionInformeOrdenCompra.ShowDialog()
        If OpcionInformeOrdenCompra.PRegresar Then
            OpcionInformeOrdenCompra.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeSeguimientoOrdenCompra(OpcionInformeOrdenCompra.PProveedor, OpcionInformeOrdenCompra.POrdenCompra, OpcionInformeOrdenCompra.PFechaDesde, OpcionInformeOrdenCompra.PFechaHasta, OpcionInformeOrdenCompra.PPeriodoDesde, OpcionInformeOrdenCompra.PPeriodoHasta, OpcionInformeOrdenCompra.PEstado)

        OpcionInformeOrdenCompra.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuListaDeRecepciones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaDeRecepciones.Click

        If Not FormularioOK(ListaRecepciones) Then Exit Sub

        ListaRecepciones.PTipo = 2
        ListaRecepciones.ShowDialog()
        ListaRecepciones.Dispose()

    End Sub
    Private Sub MenuListaDevolucion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaDevolucion.Click

        If Not FormularioOK(ListaDevolucionInsumos) Then Exit Sub

        ListaDevolucionInsumos.Show()

    End Sub
    Private Sub MenuTransferenciaDeInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTransferenciaDeInsumo.Click

        If Not FormularioOK(UnaTransferenciaInsumo) Then Exit Sub

        UnaTransferenciaInsumo.Show()

    End Sub
    Private Sub MenuListaTransferenciasInsumos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaTransferenciasInsumos.Click

        If Not FormularioOK(ListaTransferenciasInsumo) Then Exit Sub

        ListaTransferenciasInsumo.Show()

    End Sub
    Private Sub MenuConsumoParaLotesDeReventa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuConsumoParaLotesDeReventa.Click

        If Not FormularioOK(UnConsumoDeInsumo) Then Exit Sub

        UnConsumoDeInsumo.PEsReventa = True
        UnConsumoDeInsumo.ShowDialog()
        UnConsumoDeInsumo.Dispose()

    End Sub
    Private Sub MenuParaLotesEnConsignacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuParaLotesEnConsignacion.Click

        If Not FormularioOK(UnConsumoDeInsumo) Then Exit Sub

        UnConsumoDeInsumo.PEsConsignacion = True
        UnConsumoDeInsumo.ShowDialog()
        UnConsumoDeInsumo.Dispose()

    End Sub
    Private Sub MenuParaCosteo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuParaCosteo.Click

        If Not FormularioOK(UnConsumoDeInsumo) Then Exit Sub

        UnConsumoDeInsumo.PEsNegocio = True
        UnConsumoDeInsumo.ShowDialog()
        UnConsumoDeInsumo.Dispose()

    End Sub
    Private Sub MenuSinDestino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSinDestino.Click

        If Not FormularioOK(UnConsumoDeInsumo) Then Exit Sub

        UnConsumoDeInsumo.ShowDialog()
        UnConsumoDeInsumo.Dispose()

    End Sub
    Private Sub MenuListaConsumos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaConsumos.Click

        If Not FormularioOK(ListaConsumos) Then Exit Sub

        ListaConsumos.Show()

    End Sub
    Private Sub MenuRemitoInsumosIFCO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRemitoInsumosIFCO.Click

        If Not FormularioOK(UnRemitoLogistico) Then Exit Sub

        UnRemitoLogistico.Show()

    End Sub
    Private Sub MenuListaRemitosInsumosIFCO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaRemitosInsumosIFCO.Click

        If Not FormularioOK(ListaRemitosLogisticos) Then Exit Sub

        ListaRemitosLogisticos.Show()

    End Sub
    Private Sub MenuListaStockArticulosLogisticos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaStockArticulosLogisticos.Click

        If Not FormularioOK(ListaStockArticulosLogisticos) Then Exit Sub

        ListaStockArticulosLogisticos.Show()

    End Sub
    Private Sub MenuListaIngresosArticulosLogisticos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaIngresosArticulosLogisticos.Click

        If Not FormularioOK(ListaIngresosArticulosLogisticos) Then Exit Sub

        ListaIngresosArticulosLogisticos.Show()

    End Sub
    Private Sub MenuListaDevoluciónIngresoLogísticos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaDevoluciónIngresoLogísticos.Click

        If Not FormularioOK(ListaDevolucionIngresoLogistico) Then Exit Sub

        ListaDevolucionIngresoLogistico.Show()

    End Sub
    Private Sub MenuTransferenciaArtículosLogísticos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTransferenciaArtículosLogísticos.Click

        If Not FormularioOK(UnaTransferenciaLogistica) Then Exit Sub

        UnaTransferenciaLogistica.Show()

    End Sub
    Private Sub MenuListaTransferenciasLogísticas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaTransferenciasLogísticas.Click

        If Not FormularioOK(ListaTransferenciasLogisticas) Then Exit Sub

        ListaTransferenciasLogisticas.Show()

    End Sub
    Private Sub MenuSaldoInicialArticulosLogisticos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSaldoInicialArticulosLogisticos.Click

        If Not FormularioOK(UnIngresoArticulosLogisticos) Then Exit Sub

        UnIngresoArticulosLogisticos.PEsSaldoInicial = True
        UnIngresoArticulosLogisticos.Show()

    End Sub
    Private Sub MenuListaSaldosInicialesArtículosLogísticos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaSaldosInicialesArtículosLogísticos.Click

        If Not FormularioOK(ListaIngresosArticulosLogisticos) Then Exit Sub

        ListaIngresosArticulosLogisticos.PEsSaldoInicial = True
        ListaIngresosArticulosLogisticos.Show()

    End Sub
    Private Sub MenuIngresoArticulosLogisticos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuIngresoArticulosLogisticos.Click

        If Not FormularioOK(UnIngresoArticulosLogisticos) Then Exit Sub

        UnIngresoArticulosLogisticos.Show()

    End Sub
    Private Sub MenuInformeIngresosArticulosLogisticos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuInformeIngresosArticulosLogisticos.Click

        OpcionInformeLogistico.ShowDialog()
        If OpcionInformeLogistico.PRegresar Then
            OpcionInformeLogistico.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeIngresosLogisticos(OpcionInformeLogistico.PEmisor, OpcionInformeLogistico.PArticulo, OpcionInformeLogistico.PEstado, OpcionInformeLogistico.PDeposito, OpcionInformeLogistico.PDesde, OpcionInformeLogistico.PHasta, OpcionInformeLogistico.PCandadoAbierto, OpcionInformeLogistico.PCandadoCerrado)

        OpcionInformeLogistico.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuRemitosArtículosLogísticos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRemitosArtículosLogísticos.Click

        OpcionInformeLogistico.PInformeRemitos = True
        OpcionInformeLogistico.ShowDialog()
        If OpcionInformeLogistico.PRegresar Then
            OpcionInformeLogistico.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeRemitosLogisticos(OpcionInformeLogistico.PEmisor, OpcionInformeLogistico.PArticulo, OpcionInformeLogistico.PEstado, OpcionInformeLogistico.PDeposito, OpcionInformeLogistico.PDesde, OpcionInformeLogistico.PHasta, OpcionInformeLogistico.PCandadoAbierto, OpcionInformeLogistico.PCandadoCerrado, OpcionInformeLogistico.PSucursal)

        OpcionInformeLogistico.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuAlicuotasParaLiqProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAlicuotasParaLiqProveedores.Click
        'anulado
        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 22
        UnaTabla.Show()

    End Sub
    Private Sub MenuCanalDeVenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCanalDeVenta.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 23
        UnaTabla.Show()

    End Sub
    Private Sub MenuCanalDeDistribucion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCanalDeDistribucion.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 45
        UnaTabla.Show()

    End Sub
    Private Sub MenuProvincias_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuProvincias.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 31
        UnaTabla.Show()

    End Sub
    Private Sub MenuPaises_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuPaises.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 28
        UnaTabla.Show()

    End Sub
    Private Sub MenuRetenciónes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRetenciónes.Click

        If Not FormularioOK(UnaTablaRetenciones) Then Exit Sub

        UnaTablaRetenciones.Show()

    End Sub
    Private Sub MenuCodigosDeLaAFIP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCodigosAFIPElectronicos.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 41
        UnaTabla.Show()

    End Sub
    Private Sub MenuMonedas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuMonedas.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 27
        UnaTabla.Show()

    End Sub
    Private Sub MenuConceptosDeGastos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuConceptosDeGastos.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 29
        UnaTabla.Show()

    End Sub
    Private Sub MenuConceptosPrestamos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuConceptosPrestamos.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 32
        UnaTabla.Show()

    End Sub
    Private Sub MenuGastosBancarios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuGastosBancarios.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 33
        UnaTabla.Show()

    End Sub
    Private Sub MenuOtroProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOtroProveedor.Click

        If Not FormularioOK(UnOtroProveedor) Then Exit Sub

        UnOtroProveedor.ShowDialog()
        UnOtroProveedor.Dispose()

    End Sub
    Private Sub MenuListaOtrosProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaOtrosProveedores.Click

        If Not FormularioOK(ListaOtrosProveedores) Then Exit Sub

        ListaOtrosProveedores.Show()

    End Sub
    Private Sub MenuConceptosParaFacturasOtrosProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuConceptosParaFacturasOtrosProveedores.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 36
        UnaTabla.Show()

    End Sub
    Private Sub MenuListaInsumos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaInsumos.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        If Not PermisoLectura(150) Then MsgBox("No tiene Permiso de Lectura para esta Opción.") : Exit Sub
        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(150)
        UnaTabla.Ptipo = 1000
        UnaTabla.Show()

    End Sub
    Private Sub MenuINCOTERM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuINCOTERM.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 38
        UnaTabla.Show()

    End Sub
    Private Sub SaldosInicialesDeCajasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaldosInicialesDeCajasToolStripMenuItem.Click

        If Not FormularioOK(UnaTablaCajas) Then Exit Sub

        UnaTablaCajas.Show()

    End Sub
    Private Sub menuMaestroFondosFijo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuMaestroFondosFijo.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 4000
        UnaTabla.Show()

    End Sub
    Private Sub MenuArticulosLogisticosIFCO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuArticulosLogisticosIFCO.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 6
        UnaTabla.Show()

    End Sub
    Private Sub MenuZonasParaListaDePrecios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuZonasParaListaDePrecios.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 40
        UnaTabla.Show()

    End Sub
    Private Sub MenuVendedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuVendedores.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 37
        UnaTabla.Show()

    End Sub
    Private Sub MenuTransportistas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTransportistas.Click

        If Not FormularioOK(UnaTablaTransporte) Then Exit Sub

        UnaTablaTransporte.Show()

    End Sub
    Private Sub MenuAsientoTiposDePagoOtrosProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsientoTiposDePagoOtrosProveedores.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 39
        UnaTabla.Show()

    End Sub
    Private Sub MenuPuntosDeVenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuPuntosDeVenta.Click

        If Not FormularioOK(UnPuntosDeVenta) Then Exit Sub

        UnPuntosDeVenta.PBloqueaFunciones = Not PermisoEscritura(15)
        UnPuntosDeVenta.ShowDialog()
        UnPuntosDeVenta.Dispose()

    End Sub
    Private Sub MenuFacturasDeInsumos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturasDeInsumos.Click

        If Not PermisoEscritura(2) Then MsgBox("No Autorizado") : Exit Sub

        If Not FormularioOK(UnaFacturaProveedor) Then Exit Sub

        UnaFacturaProveedor.PFactura = 0
        UnaFacturaProveedor.PCodigoFactura = 902
        UnaFacturaProveedor.ShowDialog()
        UnaFacturaProveedor.Dispose()

    End Sub
    Private Sub MenuFacturaDeReventa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturaDeReventa.Click

        If Not FormularioOK(UnaFacturaProveedor) Then Exit Sub

        UnaFacturaProveedor.PFactura = 0
        UnaFacturaProveedor.PCodigoFactura = 900
        UnaFacturaProveedor.ShowDialog()

    End Sub
    Private Sub MenuFacturaSinComprobante_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturaSinComprobante.Click

        If Not FormularioOK(UnaFacturaProveedor) Then Exit Sub

        UnaFacturaProveedor.PFactura = 0
        UnaFacturaProveedor.PCodigoFactura = 903
        UnaFacturaProveedor.ShowDialog()

    End Sub
    Private Sub MenuFacturaAfectadoaACostoDeLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturaAfectadoaACostoDeLotes.Click

        If Not FormularioOK(UnaFacturaProveedor) Then Exit Sub

        UnaFacturaProveedor.PFactura = 0
        UnaFacturaProveedor.PCodigoFactura = 901
        UnaFacturaProveedor.ShowDialog()

    End Sub
    Private Sub MenuFacturaEquilibradora_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturaEquilibradora.Click

        If Not FormularioOK(UnaFacturaProveedor) Then Exit Sub

        UnaFacturaProveedor.PEsTr = True
        UnaFacturaProveedor.PFactura = 0
        UnaFacturaProveedor.PCodigoFactura = 903
        UnaFacturaProveedor.ShowDialog()

    End Sub
    Private Sub MenuFacturaDeReventaImportacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturaDeReventaImportacion.Click

        If Not FormularioOK(UnaFacturaProveedor) Then Exit Sub

        UnaFacturaProveedor.PFactura = 0
        UnaFacturaProveedor.PEsImportacion = True
        UnaFacturaProveedor.PCodigoFactura = 900
        UnaFacturaProveedor.ShowDialog()

    End Sub
    Private Sub MenuOtrasFacturasImportacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOtrasFacturasImportacion.Click

        If Not FormularioOK(UnaFacturaProveedor) Then Exit Sub

        UnaFacturaProveedor.PFactura = 0
        UnaFacturaProveedor.PEsImportacion = True
        UnaFacturaProveedor.PCodigoFactura = 901
        UnaFacturaProveedor.ShowDialog()

    End Sub
    Private Sub MenuConsignatarios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuConsignatarios.Click

        If Not FormularioOK(AnalisisLotes) Then Exit Sub

        AnalisisLotes.EsConsignacion = True
        AnalisisLotes.Show()

    End Sub
    Private Sub MenuCosteo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCosteo.Click

        If Not FormularioOK(AnalisisLotes) Then Exit Sub

        AnalisisLotes.EsCosteo = True
        AnalisisLotes.Show()

    End Sub
    Private Sub MenuReventa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuReventa.Click

        If Not FormularioOK(AnalisisLotes) Then Exit Sub

        AnalisisLotes.EsReventa = True
        AnalisisLotes.Show()

    End Sub
    Private Sub MenuLiquidaConsignacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuLiquidaConsignacion.Click

        If Not FormularioOK(UnaPreLiquidacion) Then Exit Sub

        UnaPreLiquidacion.EsConsignacion = True
        UnaPreLiquidacion.Show()

    End Sub
    Private Sub menuLiquidacionReventa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuLiquidacionReventa.Click

        If Not FormularioOK(UnaPreLiquidacion) Then Exit Sub

        UnaPreLiquidacion.EsReventa = True
        UnaPreLiquidacion.Show()

    End Sub
    Private Sub MenuLiquidaciónAlProductorConOrdenDeCompra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuLiquidaciónInsumos.Click

        If Not FormularioOK(UnaLiquidacionInsumos) Then Exit Sub

        UnaLiquidacionInsumos.Show()

    End Sub
    Private Sub MenuListaReprocesos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaReprocesos.Click

        If Not FormularioOK(ListaReprocesos) Then Exit Sub

        ListaReprocesos.Show()

    End Sub
    Private Sub MenuRegistrarNotasDebitosDelProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRegistrarNotasDebitosDelProveedor.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PTipoNota = 500
        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub MenuRegistrarNotasCREDITODelProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRegistrarNotasCREDITODelProveedor.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PTipoNota = 700
        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub MenuRegistrarNotasCREDITOContableDelProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRegistrarNotasCREDITOContableDelProveedor.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PTipoNota = 700
        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.PTr = True
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub EmitirNotasDEBITOFINANCIERAAProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEmitirNotasDEBITOFINANCIERAAProveedores.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PTipoNota = 6
        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub MenuEmitirNotasDEBITOFINANCIERAPProveedoresPorDiferenciaDeCambio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEmitirNotasDEBITOFINANCIERAPProveedoresPorDiferenciaDeCambio.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PDiferenciaDeCambio = True
        UnReciboDebitoCredito.PTipoNota = 6
        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub MenuEmitirNotasCREDITOFINANCIERAAProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEmitirNotasCREDITOFINANCIERAAProveedores.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PTipoNota = 8
        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub MenuEmitirNotasCREDITOFINANCIERAProveedoresPorDiferenciaDeCambio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEmitirNotasCREDITOFINANCIERAProveedoresPorDiferenciaDeCambio.Click

        If Not FormularioOK(UnReciboDebitoCredito) Then Exit Sub

        UnReciboDebitoCredito.PDiferenciaDeCambio = True
        UnReciboDebitoCredito.PTipoNota = 8
        UnReciboDebitoCredito.PNota = 0
        UnReciboDebitoCredito.Show()

    End Sub
    Private Sub MenuSucursalesYCuentas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSucursalesYCuentas.Click

        If Not FormularioOK(ListaBancos) Then Exit Sub

        ListaBancos.ShowDialog()
        ListaBancos.Dispose()

    End Sub
    Private Sub MenuOrdenDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenDePago.Click

        UnRecibo.PTipoNota = 600
        UnRecibo.Show()

    End Sub
    Private Sub MenuOrdenPagoOtrosProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenPagoOtrosProveedores.Click

        If Not FormularioOK(UnReciboOtrosProveedores) Then Exit Sub

        UnReciboOtrosProveedores.PTipoNota = 5010
        UnReciboOtrosProveedores.PNota = 0
        UnReciboOtrosProveedores.Show()

    End Sub
    Private Sub MenuDevolucionDelProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDevolucionDelProveedor.Click

        If Not FormularioOK(UnReciboOtrosProveedores) Then Exit Sub

        UnReciboOtrosProveedores.PTipoNota = 5020
        UnReciboOtrosProveedores.PNota = 0
        UnReciboOtrosProveedores.Show()

    End Sub
    Private Sub MenuOrdenPagoEquilibradora_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenPagoEquilibradora.Click

        If Not FormularioOK(UnRecibo) Then Exit Sub

        UnRecibo.PEsTr = True
        UnRecibo.PTipoNota = 600
        UnRecibo.Show()

    End Sub
    Private Sub MenuTrOPagoEspecial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenPagoEspecial.Click

        Dim myValue As String = InputBox("INGRESE CLAVE", "", "")

        If String.IsNullOrEmpty(myValue) Then
            Return
        End If
        If myValue <> "mer2020" Then
            MessageBox.Show("Clave Erronea.")
            Return
        End If

        If Not FormularioOK(UnRecibo) Then Exit Sub

        UnRecibo.PEsTr = True
        UnRecibo.PEsTrOPagoEspecial = True
        UnRecibo.PTipoNota = 600
        UnRecibo.Show()

    End Sub
    Private Sub CobranzaContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CobranzaContable.Click

        If Not FormularioOK(UnRecibo) Then Exit Sub

        UnRecibo.PEsTr = True
        UnRecibo.PTipoNota = 60
        UnRecibo.Show()

    End Sub
    Private Sub MenuParaEgresoDeCajaACuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuParaEgresoDeCajaACuenta.Click

        UnRecibo.PTipoNota = 600
        UnRecibo.PEsEgresoCaja = True
        UnRecibo.Show()

    End Sub
    Private Sub MenuCobranzaAProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCobranzaAProveedores.Click

        If Not FormularioOK(UnRecibo) Then Exit Sub

        UnRecibo.PTipoNota = 604
        UnRecibo.Show()

    End Sub
    Private Sub MenuReciboCobro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuReciboCobro.Click

        If Not FormularioOK(UnRecibo) Then Exit Sub

        UnRecibo.PTipoNota = 60
        UnRecibo.Show()

    End Sub
    Private Sub IngresoDeValesRecupero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IngresoDeValesRecupero.Click

        If Not FormularioOK(UnRecuperoSenia) Then Exit Sub

        UnRecuperoSenia.ShowDialog()
        UnRecuperoSenia.Dispose()

    End Sub
    Private Sub MenuListadosDeValesRecupero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListadosDeValesRecupero.Click

        If Not FormularioOK(ListaRecuperoSenia) Then Exit Sub

        ListaRecuperoSenia.ShowDialog()

    End Sub
    Private Sub MenuCompraDivisas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCompraDivisas.Click

        If Not FormularioOK(UnaCompraDivisas) Then Exit Sub

        UnaCompraDivisas.PMovimiento = 0
        UnaCompraDivisas.PTipoMovimiento = 6000
        UnaCompraDivisas.Show()

    End Sub
    Private Sub MenuVentaDivisas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuVentaDivisas.Click

        If Not FormularioOK(UnaCompraDivisas) Then Exit Sub

        UnaCompraDivisas.PMovimiento = 0
        UnaCompraDivisas.PTipoMovimiento = 6001
        UnaCompraDivisas.Show()

    End Sub
    Private Sub MenuListaComprobantesDivisas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaComprobantesDivisas.Click

        If Not FormularioOK(ListaCompraDivisas) Then Exit Sub

        ListaCompraDivisas.Show()

    End Sub
    Private Sub MenuReposición_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuReposición.Click

        If Not FormularioOK(UnReciboReposicion) Then Exit Sub

        UnReciboReposicion.PNota = 0
        UnReciboReposicion.Show()

    End Sub
    Private Sub MenuAjusteFondoFijo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAjusteFondoFijo.Click

        If Not FormularioOK(UnMovimientoFondoFijo) Then Exit Sub

        UnMovimientoFondoFijo.PMovimiento = 0
        UnMovimientoFondoFijo.Show()

    End Sub
    Private Sub MenuRegistrarCobranzaManual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRegistrarCobranzaManual.Click

        If Not FormularioOK(UnRecibo) Then Exit Sub

        UnRecibo.PManual = True
        UnRecibo.PTipoNota = 60
        UnRecibo.Show()

    End Sub
    Private Sub MenuOrdenPagoAClientes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenPagoAClientes.Click

        If Not FormularioOK(UnRecibo) Then Exit Sub

        UnRecibo.PTipoNota = 64
        UnRecibo.Show()

    End Sub
    Private Sub MenuListadoOrdenesDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListadoRecibosEmitido.Click

        If Not FormularioOK(ListaRecibosEmitido) Then Exit Sub

        ListaRecibosEmitido.Show()

    End Sub
    Private Sub MenuCierreCaja_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCierreCaja.Click

        If Not FormularioOK(UnCierreCaja) Then Exit Sub

        UnCierreCaja.ShowDialog()
        UnCierreCaja.Dispose()

    End Sub
    Private Sub MenuCierreCajaDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCierreCajaDetalle.Click

        If Not FormularioOK(UnCierreCajaDetalle) Then Exit Sub

        UnCierreCajaDetalle.ShowDialog()

    End Sub
    Private Sub MenuListaPaseDeCaja_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaPasesDeCaja.Click

        If Not FormularioOK(ListaPasesDeCaja) Then Exit Sub

        ListaPasesDeCaja.ShowDialog()

    End Sub
    Private Sub MenuExtraccionEfectivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuExtraccionEfectivo.Click

        If Not FormularioOK(UnMovimientoBancario) Then Exit Sub

        UnMovimientoBancario.PTipoNota = 90
        UnMovimientoBancario.PEsExtraccionEfectivo = True
        UnMovimientoBancario.PMovimiento = 0
        UnMovimientoBancario.ShowDialog()
        UnMovimientoBancario.Dispose()

    End Sub
    Private Sub MenuExtraccionChequePropio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuExtraccionChequePropio.Click

        If Not FormularioOK(UnMovimientoBancario) Then Exit Sub

        UnMovimientoBancario.PTipoNota = 90
        UnMovimientoBancario.PEsExtraccionChequePropio = True
        UnMovimientoBancario.PMovimiento = 0
        UnMovimientoBancario.ShowDialog()
        UnMovimientoBancario.Dispose()

    End Sub
    Private Sub MenuDepositoEfectivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDepositoEfectivo.Click

        If Not FormularioOK(UnMovimientoBancario) Then Exit Sub

        UnMovimientoBancario.PTipoNota = 91
        UnMovimientoBancario.PEsDepositoEfectivo = True
        UnMovimientoBancario.PMovimiento = 0
        UnMovimientoBancario.ShowDialog()
        UnMovimientoBancario.Dispose()

    End Sub
    Private Sub MenuDepositoChequesTerceros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDepositoChequesTercerosAlDia.Click

        If Not FormularioOK(UnMovimientoBancario) Then Exit Sub

        UnMovimientoBancario.PTipoNota = 91
        UnMovimientoBancario.PEsDepositoChequesTerceros = True
        UnMovimientoBancario.PMovimiento = 0
        UnMovimientoBancario.ShowDialog()
        UnMovimientoBancario.Dispose()

    End Sub
    Private Sub MenuDepositoChequesTercerosDiferidos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDepositoChequesTercerosDiferidos.Click

        If Not FormularioOK(UnMovimientoBancario) Then Exit Sub

        UnMovimientoBancario.PTipoNota = 91
        UnMovimientoBancario.PEsDepositoChequesTerceros = True
        UnMovimientoBancario.PEsDiferidos = True
        UnMovimientoBancario.PMovimiento = 0
        UnMovimientoBancario.ShowDialog()
        UnMovimientoBancario.Dispose()

    End Sub
    Private Sub MenuTransferenciasCuentasPropias_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTransferenciasCuentasPropias.Click

        If Not FormularioOK(UnMovimientoBancario) Then Exit Sub

        UnMovimientoBancario.PTipoNota = 92
        UnMovimientoBancario.PEsTransferenciaPropia = True
        UnMovimientoBancario.PMovimiento = 0
        UnMovimientoBancario.ShowDialog()
        UnMovimientoBancario.Dispose()

    End Sub
    Private Sub MenuListaMovimientosBancarios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaMovimientosBancarios.Click

        If Not FormularioOK(ListaMovimientosBancario) Then Exit Sub

        ListaMovimientosBancario.ShowDialog()

    End Sub
    Private Sub MenuSaldoCuentasBancarias_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSaldoCuentasBancarias.Click

        If Not FormularioOK(UnSaldosCuentas) Then Exit Sub

        UnSaldosCuentas.ShowDialog()
        UnSaldosCuentas.Dispose()

    End Sub
    Private Sub MenuSaldosCuentasBancariasDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSaldosCuentasBancariasDetalle.Click

        If Not FormularioOK(ListaSaldosBancoDetalle) Then Exit Sub

        ListaSaldosBancoDetalle.ShowDialog()
        ListaSaldosBancoDetalle.Dispose()

    End Sub
    Private Sub MenuDevolucionSeña_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDevolucionSeña.Click

        If Not FormularioOK(UnRecibo) Then Exit Sub

        UnRecibo.PTipoNota = 65
        UnRecibo.Show()

    End Sub
    Private Sub MenuConciliacionChequesPropios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuConciliacionChequesPropios.Click

        If Not FormularioOK(UnChequesPropios) Then Exit Sub

        UnChequesPropios.ShowDialog()
        UnChequesPropios.Dispose()

    End Sub
    Private Sub MenuConciliacionChequesTerceros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuConciliacionChequesTerceros.Click

        If Not FormularioOK(UnChequesEnCartera) Then Exit Sub

        UnChequesEnCartera.ShowDialog()
        UnChequesEnCartera.Dispose()

    End Sub
    Private Sub MenuRechazoChequesTerceros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRechazoDeCheques.Click

        If Not FormularioOK(UnChequeRechazo) Then Exit Sub

        UnChequeRechazo.ShowDialog()
        UnChequeRechazo.Dispose()

    End Sub
    Private Sub MenuChequesSinNotasDeDebito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuChequesSinNotasDeDebito.Click

        If Not FormularioOK(ListaChequesSinNotaDebito) Then Exit Sub

        ListaChequesSinNotaDebito.Show()

    End Sub
    Private Sub MenuReemplazoChequesPropios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If Not FormularioOK(UnChequeReemplazo) Then Exit Sub

        UnChequeReemplazo.ShowDialog()
        UnChequeReemplazo.Dispose()

    End Sub
    Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.Click

        If Not FormularioOK(UnRecibo) Then Exit Sub

        UnRecibo.PTipoNota = 600
        UnRecibo.PEsPorCuentaYOrden = True
        UnRecibo.ShowDialog()
        UnRecibo.Dispose()

    End Sub
    Private Sub MenuNuevoPrestamo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNuevoPrestamo.Click

        If Not FormularioOK(UnPrestamo) Then Exit Sub

        UnPrestamo.ShowDialog()
        UnPrestamo.Dispose()

    End Sub
    Private Sub MenuIngresoPrestamoConSaldoInicia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuIngresoPrestamoConSaldoInicia.Click

        If Not FormularioOK(UnPrestamo) Then Exit Sub

        UnPrestamo.PSaldoInicial = True
        UnPrestamo.ShowDialog()
        UnPrestamo.Dispose()

    End Sub
    Private Sub MenuListaPrestamos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaPrestamos.Click

        If Not FormularioOK(ListaPrestamos) Then Exit Sub

        ListaPrestamos.ShowDialog()
        ListaPrestamos.Dispose()

    End Sub
    Private Sub MenuMovimientosPrestamos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSaldosPrestamosDetalle.Click

        If Not FormularioOK(ListaSaldosPrestamosDetalle) Then Exit Sub

        ListaSaldosPrestamosDetalle.ShowDialog()
        ListaSaldosPrestamosDetalle.Dispose()

    End Sub
    Private Sub MenuNuevoMovimientoGasto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNuevoMovimientoGasto.Click

        If Not FormularioOK(UnGastoBancario) Then Exit Sub

        UnGastoBancario.PMovimiento = 0
        UnGastoBancario.ShowDialog()
        UnGastoBancario.Dispose()

    End Sub
    Private Sub MenuListadoDeMovimientosGastos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListadoDeMovimientosGastos.Click

        If Not FormularioOK(ListaMovimientosGastosBancario) Then Exit Sub

        ListaMovimientosGastosBancario.ShowDialog()
        ListaMovimientosGastosBancario.Dispose()

    End Sub
    Private Sub MenuRecibosManualesFaltantes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRecibosManualesFaltantes.Click

        If Not FormularioOK(ListaRecibosFaltantes) Then Exit Sub

        ListaRecibosFaltantes.PTipo = 60
        ListaRecibosFaltantes.ShowDialog()

    End Sub
    Private Sub MenuNuevoEmpleado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNuevoEmpleado.Click

        If Not FormularioOK(UnLegajo) Then Exit Sub

        UnLegajo.PLegajo = 0
        UnLegajo.ShowDialog()
        UnLegajo.Dispose()

    End Sub
    Private Sub MenuListadoDeEmpleados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListadoDeEmpleados.Click

        If Not FormularioOK(ListaEmpleados) Then Exit Sub

        ListaEmpleados.ShowDialog()
        ListaEmpleados.Dispose()

    End Sub
    Private Sub MenuRecibosDeSueldo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRecibosDeSueldo.Click

        If Not FormularioOK(UnReciboSueldo) Then Exit Sub

        UnReciboSueldo.PRecibo = 0
        UnReciboSueldo.ShowDialog()
        UnReciboSueldo.Dispose()

    End Sub
    Private Sub MenuListaRecibosDeSueldos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaRecibosDeSueldos.Click

        If Not FormularioOK(ListaRecibosSueldo) Then Exit Sub

        ListaRecibosSueldo.ShowDialog()
        ListaRecibosSueldo.Dispose()

    End Sub
    Private Sub MenuUsuarios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuUsuarios.Click

        If Not FormularioOK(UnUsuario) Then Exit Sub

        UnUsuario.ShowDialog()
        UnUsuario.Dispose()

    End Sub
    Private Sub MenuOrdenDePagoSueldos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenDePagoSueldos.Click

        If GCaja = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Exit Sub
        End If

        UnaOrdenPagoSueldos.PTipoNota = 4010
        UnaOrdenPagoSueldos.PNota = 0
        UnaOrdenPagoSueldos.ShowDialog()

    End Sub
    Private Sub MenuCuentaCorrientes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCuentaCorrientes.Click

        UnaCtaCteSueldos.ShowDialog()
        UnaCtaCteSueldos.Dispose()

    End Sub
    Private Sub MenuListaOrdenesDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaOrdenesDePago.Click

        ListaSueldosOrdenPago.ShowDialog()
        ListaSueldosOrdenPago.Dispose()

    End Sub
    Private Sub MenuNuevaOtraFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNuevaOtraFactura.Click

        If Not FormularioOK(UnaFacturaOtrosProveedores) Then Exit Sub

        UnaFacturaOtrosProveedores.ShowDialog()
        UnaFacturaOtrosProveedores.Dispose()

    End Sub
    Private Sub MenuListaOtrasFacturas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaOtrasFacturas.Click

        If Not FormularioOK(ListaOtrasFacturas) Then Exit Sub

        ListaOtrasFacturas.ShowDialog()
        ListaOtrasFacturas.Dispose()

    End Sub
    Private Sub MenuCuentaCorrienteOtrosPagos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCuentaCorrienteOtrosPagos.Click

        If Not FormularioOK(ListaSaldosOtrosPagosDetalle) Then Exit Sub

        ListaSaldosOtrosPagosDetalle.ShowDialog()
        ListaSaldosOtrosPagosDetalle.Dispose()

    End Sub
    Private Sub NotaEmitirDebitoFinancieraOtrosProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotaEmitirDebitoFinancieraOtrosProveedores.Click

        If Not FormularioOK(UnReciboDebitoCreditoOtrosProveedores) Then Exit Sub

        UnReciboDebitoCreditoOtrosProveedores.PNota = 0
        UnReciboDebitoCreditoOtrosProveedores.PTipoNota = 5005
        UnReciboDebitoCreditoOtrosProveedores.ShowDialog()
        UnReciboDebitoCreditoOtrosProveedores.Dispose()

    End Sub
    Private Sub EmitirNotasCreditoFinancieraOtrosProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EmitirNotasCreditoFinancieraOtrosProveedores.Click

        If Not FormularioOK(UnReciboDebitoCreditoOtrosProveedores) Then Exit Sub

        UnReciboDebitoCreditoOtrosProveedores.PNota = 0
        UnReciboDebitoCreditoOtrosProveedores.PTipoNota = 5007
        UnReciboDebitoCreditoOtrosProveedores.ShowDialog()
        UnReciboDebitoCreditoOtrosProveedores.Dispose()

    End Sub
    Private Sub MenuListaNotasFinancierasOtrosProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaNotasFinancierasOtrosProveedores.Click

        If Not FormularioOK(ListaNotasFinancierasOtrosProveedores) Then Exit Sub

        ListaNotasFinancierasOtrosProveedores.ShowDialog()
        ListaNotasFinancierasOtrosProveedores.Dispose()

    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        PruebaPrintDocument.Show()

    End Sub
    Private Sub MenuAnalisisResultadoCosteo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAnalisisResultadoCosteo.Click

        If Not FormularioOK(AnalisisDeCosteos) Then Exit Sub

        AnalisisDeCosteos.Show()

    End Sub
    Private Sub MenuAnalisisCompraVentaPorCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAnalisisCompraVentaPorCliente.Click

        If Not FormularioOK(AnalisisCompraVentaPorCliente) Then Exit Sub

        AnalisisCompraVentaPorCliente.Show()

    End Sub
    Private Sub MenuAnalisisDeResultadoReventaConsig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAnalisisDeResultadoReventaConsig.Click

        If Not FormularioOK(AnalisisResulatdosReventaConsignacio) Then Exit Sub

        AnalisisResulatdosReventaConsignacio.Show()

    End Sub
    Private Sub MenuAnálisisdeResultadoLotesCosteos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAnálisisDeResultadoLotesCosteos.Click

        If Not FormularioOK(AnalisisResultadosLotesCosteo) Then Exit Sub

        AnalisisResultadosLotesCosteo.Show()

    End Sub
    Private Sub MenuListaCentrosDeCosto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaCentrosDeCosto.Click

        If Not FormularioOK(ListaCentrosDeCosto) Then Exit Sub

        ListaCentrosDeCosto.Show()

    End Sub
    Private Sub ListaCuentasYSubCuentasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListaCuentasYSubCuentasToolStripMenuItem.Click

        If Not FormularioOK(ListaSubCuentas) Then Exit Sub

        ListaSubCuentas.Show()

    End Sub
    Private Sub MenuFacturaVentaDomestica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturaVentaDomestica.Click
        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 2
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNotaCreditoDomestica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotaCreditoDomestica.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 4
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNotaCreditoExportacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotaCreditoExportacion.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 41
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNotaCreditoCostoDomestica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotaCreditoCostoDomestica.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6072
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNotaCreditoCostoExportación_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotaCreditoCostoExportación.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6073
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuOrdenPagoAsiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem19.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 600
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub EgresoDeCajaACuentaDeResultado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEgresoDeCajaACuentaDeResultado.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 607
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuOrdenDePagoComerExterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenDePagoComerExterior.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 601
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuReciboCobroAProveedoresContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuReciboCobroAProveedoresContable.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 604
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuOrdenPagoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenPagoContable.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 605
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuDevoluciónSeña_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem20.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 65
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuReciboDeCobro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem18.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 60
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuReciboCobranzaContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuReciboCobranzaContable.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 62
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuReciboDeCobroComerExterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuReciboDeCobroComerExterior.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 61
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuOrdenPagoAClienteContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenPagoAClienteContable.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 64
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuFacturaProveedorReventaDomestica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturaProveedorReventaDomestica.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 900
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuFacturaProveedoresQueAfectaLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturaProveedoresQueAfectaLotes.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 901
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuFacturaProveedoresConOrdenDeCompra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturaProveedoresConOrdenDeCompra.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 902
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuOtraFacturaProveedorDomestica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOtraFacturaProveedorDomestica.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 903
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuOtraFacturaProveedorImportación_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOtraFacturaProveedorImportación.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7008
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuAsientoFacturaContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsientoFacturaContable.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7030
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuFacturaProveedorReventaImportacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturaProveedorReventaImportacion.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7007
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuLiquidacionesReventa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuLiquidacionesReventa.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 910
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuLiquidacionesConsignación_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuLiquidacionesConsignación.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 912
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuLiquidaciónContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsientoLiquidacionContable.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 913
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuLiquidacionDeInsumosContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuLiquidacionDeInsumosContable.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 915
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuLiquidaciónPorReemplazoDeFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuLiquidaciónPorReemplazoDeFactura.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 911
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNVLPAsiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNVLPAsiento.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 800
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuAsientoNVLPContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsientoNVLPContable.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 801
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuEmitirNotaCREDITOSContableACliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEmitirNotaCREDITOSContableACliente.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 11010
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNotaDebitoClienteDomestica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotaDebitoClienteDomestica.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 5
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNotaDebitoClienteExportación_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotaDebitoClienteExportación.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 10005
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuEmitirNDebitoClientePorDiferenciaDeCambio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEmitirNDebitoClientePorDiferenciaDeCambio.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 11005
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MneuNCreditoClienteDomestica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MneuNCreditoClienteDomestica.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNCreditoClienteExportación_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNCreditoClienteExportación.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 10007
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuEmitirNCreditoClientePorDiferenciaDeCambio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEmitirNCreditoClientePorDiferenciaDeCambio.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 11007
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuCostoDomestica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCostoDomestica.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6070
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuCostoExportacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCostoExportacion.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6071
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuVentaExportación_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuVentaExportación.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7006
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuAsientoServicios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsientoServicios.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7009
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuSecos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSecos.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7010
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuFacturaVentaContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturaVentaContable.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 71001
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNotasDEBITODeCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotasDEBITODeCliente.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 50
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNotaCREDITODeCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotaCREDITODeCliente.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 70
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNotaDebitoProveedorDomestica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNDebitoProveedorDomestica.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNDebitoProveedorImportación_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNDebitoProveedorImportación.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 10006
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuEmitirNDebitoPorDiferenciadeCambio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEmitirNDebitoPorDiferenciadeCambio.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 11006
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNCreditoProveedorDomestica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNCreditoProveedorDomestica.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 8
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNCreditoProveedorImportación_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNCreditoProveedorImportación.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 10008
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuEmitirNCreditoPorDiferenciaDeCambio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEmitirNCreditoPorDiferenciaDeCambio.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 11008
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuOtrosProveedoresFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOtrosProveedoresFactura.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 5000
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuOrdenDePagoParaAsiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenDePagoParaAsiento.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 5010
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuAsientoDevoluciónDelProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsientoDevoluciónDelProveedor.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 5020
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNotaDeDebitoPorRechazoDeCheque_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotaDeDebitoPorRechazoDeCheque.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7011
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub AsientoNotaDeDebitoFinancieraOtrosProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AsientoNotaDeDebitoFinancieraOtrosProveedores.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 5005
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub AsientoNotaCreditoFinancieraOtrosProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AsientoNotaCreditoFinancieraOtrosProveedores.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 5007
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuRechazoChequePrestamoCapital_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRechazoChequePrestamoCapital.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 1005
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuRechazoChequePagoPrestamo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRechazoChequePagoPrestamo.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 1007
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuRegistroDePrestamo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRegistroDePrestamo.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 8000
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuMovimientoDePrestamos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuMovimientoDePrestamos.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 8001
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuMovimientoAjusteDeCapita_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuMovimientoAjusteDeCapita.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 8002
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuAsientoDeposito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsientoDepositoEfectivo.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 91
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuAsientoDepositoChquesAlDiaYDiferidos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsientoDepositoChquesAlDiaYDiferidos.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6091
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuAsientoExtracción_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsientoExtracción.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 90
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuRechazoChequeEnDeposito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRechazoChequeEnDeposito.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 93
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MneuAsientoGastosBancarios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MneuAsientoGastosBancarios.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7005
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuRechazoChequesNoDepositados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRechazoDebitoCliente.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7001
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuRechazoChequesCreditoProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRechazoChequesCreditoProveedor.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7002
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuReemplazoDeCheque_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuReemplazoDeCheque.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 1008
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuCompraDivisa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCompraDivisa.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 16000
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuVentaDivisa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuVentaDivisa.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 16001
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuAjusteAFondoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAjusteAFondoContable.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7201
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuRendicionContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRendicionContable.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7203
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuReposiciónContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuReposiciónContable.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7205
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuCierreFondoFijo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCierreFondoFijo.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7204
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuRechazoDisminuyeFondo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRechazoAumentaFondo.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7206
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuCobranzaEnElExterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCobranzaEnElExterior.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 12000
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub menuCobranzaEnArgentina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuCobranzaEnArgentina.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 12003
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuCuentaTransferenciaBancoExportacionParaLiquidar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCuentaTransferenciaBancoExportacionParaLiquidar.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 12010
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuContableLiquidacionDivisasTransferidas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuContableLiquidacionDivisasTransferidas.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 12004
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuAsientoReintegros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsientoReintegros.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 12006
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuContableCobranzasReintegrosAduaneros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuContableCobranzasReintegrosAduaneros.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 12007
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuRechazoChequeEnReposicion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRechazoChequeEnReposicion.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7207
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuTransferenciasCtasPropias_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTransferenciasCtasPropias.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6080
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNotaDEBITODelProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotaDEBITODelProveedor.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 500
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuNotaCREDITODelProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNotaCREDITODelProveedor.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 700
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuEmitirNotaCreditoContableDelProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEmitirNotaCreditoContableDelProveedor.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 11009
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuRecuperoSenia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRecuperoSenia.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7020
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuReciboDeSueldo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuReciboDeSueldo.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 4000
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuOrdenPagoSueldo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenPagoSueldo.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 4010
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuConsumoInsumos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuConsumoInsumos.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6000
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuRecepciónDeInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRecepciónDeInsumo.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7003
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuDevoluciondeRecepcion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDevoluciondeRecepcion.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7004
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuDepositoChequePropios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDepositoChequePropios.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6010
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuConfirmarDepositoDebitoAutomaticoDiferido_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuConfirmarDepositoDebitoAutomaticoDiferido.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6011
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuIngresoMercaderia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuIngresoMercaderia.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6050
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuDevolucionIngresoMercaderia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDevolucionIngresoMercaderia.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6052
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuReprocesoMercaderia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuReprocesoMercaderia.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7000
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuAsientoRemito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsientoRemito.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6060
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuDevoluciónRemito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDevoluciónRemito.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6062
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuDescarteMercaderia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDescarteMercaderia.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6055
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuConsumosLotesProdTerminados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuConsumosLotesProdTerminados.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 61000
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuCostoConsumosLotesProdTerminados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCostoConsumosLotesProdTerminados.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 61001
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuDiferenciaInventario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDiferenciaInventario.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 6056
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuAjusteFacturasExportacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAjusteFacturasExportacion.Click

        If Not FormularioOK(UnSeteoDocumento) Then Exit Sub

        UnSeteoDocumento.PTipoDocumento = 7100
        UnSeteoDocumento.ShowDialog()

    End Sub
    Private Sub MenuAsignarCentrosDeCostoATipoDeOperaciones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsignarCentrosDeCostoATipoDeOperaciones.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(11)
        UnaTabla.BackColor = Color.Thistle
        UnaTabla.Ptipo = 2000
        UnaTabla.Show()

    End Sub
    Private Sub MenuIVACompra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuIVACompra.Click

        If Not FormularioOK(ListaLibroIvaCompra) Then Exit Sub

        ListaLibroIvaCompra.Show()

    End Sub
    Private Sub MenuIVAVenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuIVAVenta.Click

        If Not FormularioOK(ListaLibroIvaVenta) Then Exit Sub

        ListaLibroIvaVenta.Show()

    End Sub
    Private Sub MenuRetencionesEfectuadas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRetencionesEfectuadas.Click

        If Not FormularioOK(ListaRetencionesEfectuadas) Then Exit Sub

        ListaRetencionesEfectuadas.Show()

    End Sub
    Private Sub MenuInformeRetencionesPercibidas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRetencionesPercibidas.Click

        If Not FormularioOK(ListaRetencionesPercibidas) Then Exit Sub

        ListaRetencionesPercibidas.Show()

    End Sub
    Private Sub MenuPercepcionesEfectuadas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuPercepcionesEfectuadas.Click

        If Not FormularioOK(ListaPercepcionesEfectuadas) Then Exit Sub

        ListaPercepcionesEfectuadas.Show()

    End Sub
    Private Sub MenuPercepcionesPrecibidas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuPercepcionesPrecibidas.Click

        If Not FormularioOK(ListaPercepcionesPercibidas) Then Exit Sub

        ListaPercepcionesPercibidas.Show()

    End Sub
    Private Sub ListadoPorProvinciasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListadoPorProvinciasToolStripMenuItem.Click

        If Not FormularioOK(ListaRetencionesPorProvincia) Then Exit Sub

        ListaRetencionesPorProvincia.Show()

    End Sub
    Private Sub MenuCierreContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCierreContable.Click

        If Not FormularioOK(UnaTablaCierreContable) Then Exit Sub

        UnaTablaCierreContable.Show()

    End Sub
    Private Sub SaldosInicialesPlanDeCuentasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaldosInicialesPlanDeCuentasToolStripMenuItem.Click

        If Not FormularioOK(UnSaldoInicialPlanDeCuentas) Then Exit Sub

        UnSaldoInicialPlanDeCuentas.Show()

    End Sub
    Private Sub MenuBalance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuBalance.Click

        If Not FormularioOK(UnaTablaDeBalances) Then Exit Sub

        UnaTablaDeBalances.Show()

    End Sub
    Private Sub MenuCicoreCompraVenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCicoreCompraVenta.Click

        If Not FormularioOK(ListaSICORE) Then Exit Sub

        ListaSICORE.Show()

    End Sub
    Private Sub ListaPlanDeCuentasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListaPlanDeCuentasToolStripMenuItem.Click

        ListaPlanDeCuentas.Show()

    End Sub
    Private Sub MenuAsientosManuales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsientosManuales.Click

        If Not FormularioOK(UnAsiento) Then Exit Sub

        UnAsiento.Show()

    End Sub
    Private Sub MenuSaldosContables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSaldosContables.Click

        If Not FormularioOK(ListaSaldosContables) Then Exit Sub

        ListaSaldosContables.Show()

    End Sub
    Private Sub MenuListaAsientos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaAsientos.Click

        If Not FormularioOK(ListaAsientos) Then Exit Sub

        ListaAsientos.Show()

    End Sub
    Private Sub MenuAsignarCuentasMediodePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAsignarCuentasMediodePago.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(11)
        UnaTabla.BackColor = Color.Thistle
        UnaTabla.Ptipo = 3000
        UnaTabla.Show()

    End Sub
    Private Sub MenuDescripcionLotesLiquidados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDescripcionLotesLiquidados.Click

        OpcionInformeLotesReventasFacturados.PEsLotesLiquidados = True
        OpcionInformeLotesReventasFacturados.ShowDialog()
        If OpcionInformeLotesReventasFacturados.PProveedor = 0 Then
            OpcionInformeLotesReventasFacturados.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If OpcionInformeLotesReventasFacturados.PFacturados Then
            LotesReventaFactLiquidados(OpcionInformeLotesReventasFacturados.PProveedor, OpcionInformeLotesReventasFacturados.PNombre, OpcionInformeLotesReventasFacturados.PDesde, OpcionInformeLotesReventasFacturados.PHasta, OpcionInformeLotesReventasFacturados.PTextRemitos)
        End If

        If OpcionInformeLotesReventasFacturados.PPendientes Then
            LotesReventaPendientes(OpcionInformeLotesReventasFacturados.PProveedor, OpcionInformeLotesReventasFacturados.PNombre, OpcionInformeLotesReventasFacturados.PDesde, OpcionInformeLotesReventasFacturados.PHasta, OpcionInformeLotesReventasFacturados.PTextRemitos)
        End If

        OpcionInformeLotesReventasFacturados.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuInformeParaInventario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuInformeParaInventario.Click

        OpcionInformeParaInventario.PEsInventario = True
        OpcionInformeParaInventario.ShowDialog()
        If OpcionInformeParaInventario.PRegresar Then
            OpcionInformeParaInventario.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeParaInventario(OpcionInformeParaInventario.PEspecie, OpcionInformeParaInventario.PVariedad, OpcionInformeParaInventario.PMarca, OpcionInformeParaInventario.PCategoria, OpcionInformeParaInventario.PEnvase, OpcionInformeParaInventario.PDeposito, _
                              OpcionInformeParaInventario.PListaComprobantes, OpcionInformeParaInventario.PListaComprobantesCerrado, OpcionInformeParaInventario.PPedidos)

        OpcionInformeParaInventario.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuArticulosPendientesDeAsignar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuInformeParaInventarioDetalle.Click

        OpcionInformeParaInventario.PEsInventario = True
        OpcionInformeParaInventario.ShowDialog()
        If OpcionInformeParaInventario.PRegresar Then
            OpcionInformeParaInventario.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeParaInventarioDetalle(OpcionInformeParaInventario.PEspecie, OpcionInformeParaInventario.PVariedad, OpcionInformeParaInventario.PMarca, OpcionInformeParaInventario.PCategoria, OpcionInformeParaInventario.PEnvase, OpcionInformeParaInventario.PDeposito, _
                                     OpcionInformeParaInventario.PListaComprobantes, OpcionInformeParaInventario.PListaComprobantesCerrado, OpcionInformeParaInventario.PPedidos)

        OpcionInformeParaInventario.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuLotesIngresadosPorProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuLotesIngresadosPorProveedor.Click

        OpcionInformeLotesReventasFacturados.PEsIngresos = True
        OpcionInformeLotesReventasFacturados.ShowDialog()
        If OpcionInformeLotesReventasFacturados.PRegresar Then
            OpcionInformeLotesReventasFacturados.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        LotesIngresadosPorProveedor(OpcionInformeLotesReventasFacturados.PProveedor, OpcionInformeLotesReventasFacturados.PDesde, OpcionInformeLotesReventasFacturados.PHasta, OpcionInformeLotesReventasFacturados.PReventa, OpcionInformeLotesReventasFacturados.PConsignacion, OpcionInformeLotesReventasFacturados.PTodos, OpcionInformeLotesReventasFacturados.PEspecie, OpcionInformeLotesReventasFacturados.PVariedad, OpcionInformeLotesReventasFacturados.PMarca, OpcionInformeLotesReventasFacturados.PCategoria, OpcionInformeLotesReventasFacturados.PEnvase, _
                              OpcionInformeLotesReventasFacturados.PDeposito, OpcionInformeLotesReventasFacturados.PCosteo, OpcionInformeLotesReventasFacturados.PDuenio, OpcionInformeLotesReventasFacturados.PSucursal, OpcionInformeLotesReventasFacturados.PTextRemitos)
        OpcionInformeLotesReventasFacturados.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuSeniasPagadasYRecuperadas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSeniasPagadas.Click

        OpcionInformeLotesReventasFacturados.PEsSenias = True
        OpcionInformeLotesReventasFacturados.ShowDialog()
        If OpcionInformeLotesReventasFacturados.PRegresar Then
            OpcionInformeLotesReventasFacturados.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        SeniasPagadas(OpcionInformeLotesReventasFacturados.PProveedor, OpcionInformeLotesReventasFacturados.PDesde, OpcionInformeLotesReventasFacturados.PHasta)
        OpcionInformeLotesReventasFacturados.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuSeñasRecuperadasPorProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSeñasRecuperadasPorProveedor.Click

        OpcionInformeLotesReventasFacturados.PEsSenias = True
        OpcionInformeLotesReventasFacturados.ShowDialog()
        If OpcionInformeLotesReventasFacturados.PRegresar Then
            OpcionInformeLotesReventasFacturados.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        SeniasRecuperadas(OpcionInformeLotesReventasFacturados.PProveedor, OpcionInformeLotesReventasFacturados.PDesde, OpcionInformeLotesReventasFacturados.PHasta)
        OpcionInformeLotesReventasFacturados.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuInformeDescartes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuInformeDescartes.Click

        OpcionInformeParaInventario.PDescartes = True
        OpcionInformeParaInventario.ShowDialog()
        If OpcionInformeParaInventario.PRegresar Then
            OpcionInformeParaInventario.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeDescartes(OpcionInformeParaInventario.PEspecie, OpcionInformeParaInventario.PVariedad, OpcionInformeParaInventario.PMarca, OpcionInformeParaInventario.PCategoria, OpcionInformeParaInventario.PEnvase, OpcionInformeParaInventario.PDesde, OpcionInformeParaInventario.PHasta)

        OpcionInformeParaInventario.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuInformeVentas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuInformeVentas.Click

        OpcionVentas.ShowDialog()
        If OpcionVentas.PRegresar Then
            OpcionVentas.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeVentas(OpcionVentas.PCliente, OpcionVentas.PEspecie, OpcionVentas.PVariedad, OpcionVentas.PDesde, _
                      OpcionVentas.PHasta, OpcionVentas.PEstado, OpcionVentas.PAbierto, OpcionVentas.PCerrado, _
                      OpcionVentas.PVendedor, OpcionVentas.PCanalVenta, OpcionVentas.PDeposito, OpcionVentas.PMuestraLotes, _
                      OpcionVentas.PRepetirDatos, OpcionVentas.PSucursal, OpcionVentas.PSinContables, OpcionVentas.PSoloContables, _
                      OpcionVentas.PTodas, OpcionVentas.PMarca, OpcionVentas.PCategoria, OpcionVentas.PEnvase)

        OpcionVentas.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuInformeRemitos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuInformeRemitos.Click

        OpcionVentas.PRemitos = True
        OpcionVentas.ShowDialog()
        If OpcionVentas.PRegresar Then
            OpcionVentas.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeRemitos(OpcionVentas.PCliente, OpcionVentas.PEspecie, OpcionVentas.PVariedad, OpcionVentas.PDesde, OpcionVentas.PHasta, OpcionVentas.PEstado, OpcionVentas.PAbierto, OpcionVentas.PCerrado, OpcionVentas.PCanalVenta, OpcionVentas.PFacturados, OpcionVentas.PPendientesFacturar, _
             OpcionVentas.PDeposito, OpcionVentas.PMuestraLotes, OpcionVentas.PRepetirDatos, OpcionVentas.PSucursal, OpcionVentas.PPorFechaEmision, OpcionVentas.PPorFechaEntrega, OpcionVentas.PTextRemitos, OpcionVentas.PMuestraPedido, OpcionVentas.PSoloConfirmados)

        OpcionVentas.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuEnvasesPerdidos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuEnvasesPerdidos.Click

        Opcionfechas.PTitulo = "Fecha de Facturas Venta"
        Opcionfechas.PEsConProveedorLote = True
        Opcionfechas.ShowDialog()

        If Not Opcionfechas.PRegresa Then
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            InformeEnvasesPerdidos(Opcionfechas.PDesde, Opcionfechas.PHasta, Opcionfechas.PProveedor)
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End If

        Opcionfechas.Dispose()

    End Sub
    Private Sub MenuReprocesos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuReprocesos.Click

        OpcionInformeReprocesos.ShowDialog()
        If OpcionInformeReprocesos.PRegresar Then
            OpcionInformeReprocesos.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeReprocesos(OpcionInformeReprocesos.PEspecie, OpcionInformeReprocesos.PVariedad, OpcionInformeReprocesos.PEnvase, OpcionInformeReprocesos.PEspecie2, OpcionInformeReprocesos.PVariedad2, OpcionInformeReprocesos.PEnvase2, OpcionInformeReprocesos.PDesde, OpcionInformeReprocesos.PHasta, OpcionVentas.PEstado, OpcionInformeReprocesos.PAbierto, OpcionInformeReprocesos.PCerrado, OpcionInformeReprocesos.PDeposito, OpcionInformeReprocesos.PProveedor)

        OpcionInformeReprocesos.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuDetalleFacturasAfectadasAUnCosteo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDetalleFacturasAfectadasAUnCosteo.Click

        OpcionParaNegocios.ShowDialog()
        If OpcionParaNegocios.PRegresar Then
            OpcionParaNegocios.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeComprobantesDeUnCosteo(OpcionParaNegocios.PInsumos, OpcionParaNegocios.PNegocio, OpcionParaNegocios.PCosteo, OpcionParaNegocios.PDesde, OpcionParaNegocios.PHasta, OpcionParaNegocios.PEstado, OpcionParaNegocios.PAbierto, OpcionParaNegocios.PCerrado, OpcionParaNegocios.PNombreCosteo)

        OpcionParaNegocios.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuGestionCompraVentaPorLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuGestionCompraVentaPorLotes.Click

        OpcionCompraVentaPorLotes.ShowDialog()
        If OpcionCompraVentaPorLotes.PRegresar Then
            OpcionCompraVentaPorLotes.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeComprasVentasPorLotes(OpcionCompraVentaPorLotes.PProveedor, OpcionCompraVentaPorLotes.PNombre, OpcionCompraVentaPorLotes.PDesde, OpcionCompraVentaPorLotes.PHasta, OpcionCompraVentaPorLotes.PEspecie, OpcionCompraVentaPorLotes.PConStock, OpcionCompraVentaPorLotes.PSinStock, OpcionCompraVentaPorLotes.PFacturados, OpcionCompraVentaPorLotes.PPendientes)

        OpcionCompraVentaPorLotes.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub TotalizaArticulosRemitidosMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TotalizaArticulosRemitidosMenu.Click

        OpcionClienteDesdeHasta.ShowDialog()
        If OpcionClienteDesdeHasta.PRegresar Then
            OpcionClienteDesdeHasta.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        TotalizaArticulosRemitidos(OpcionClienteDesdeHasta.PCliente, OpcionClienteDesdeHasta.PDesde, OpcionClienteDesdeHasta.PHasta, OpcionClienteDesdeHasta.PSucursal)

        OpcionClienteDesdeHasta.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuGestionComprasPorArticulos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuGestionComprasPorArticulos.Click

        OpcionInformeGestionCompra.ShowDialog()
        If OpcionInformeGestionCompra.PRegresar Then
            OpcionInformeGestionCompra.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeComprasVentasPorArticulo(0, OpcionInformeGestionCompra.PDesdeFactura, OpcionInformeGestionCompra.PHastaFactura, OpcionInformeGestionCompra.PDesdeIngreso, OpcionInformeGestionCompra.PHastaIngreso, OpcionInformeGestionCompra.PDomesticas, OpcionInformeGestionCompra.PImportacion, OpcionInformeGestionCompra.PAbierto, OpcionInformeGestionCompra.PCerrado)

        OpcionInformeGestionCompra.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuVentasYCostosDeLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuVentasYCostosDeLotes.Click

        OpcionVentasYCostosDeLotes.PEsCompraVentaFactura = True
        OpcionVentasYCostosDeLotes.ShowDialog()
        If OpcionVentasYCostosDeLotes.PRegresar Then
            OpcionVentasYCostosDeLotes.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

       InformeVentasYCostosDeFacturas(OpcionVentasYCostosDeLotes.PDesde, OpcionVentasYCostosDeLotes.PHasta, OpcionVentasYCostosDeLotes.PAbierto, OpcionVentasYCostosDeLotes.PCerrado, OpcionVentasYCostosDeLotes.PEsExportacion, OpcionVentasYCostosDeLotes.PEsDomestica, OpcionVentasYCostosDeLotes.PCliente)

        OpcionVentasYCostosDeLotes.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuCotosDeMermaPorLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCotosDeMermaPorLotes.Click

        Opcionfechas.PEsConProveedorLote = True
        Opcionfechas.ShowDialog()
        If Opcionfechas.PRegresa Then
            Opcionfechas.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeCostosMermaPorLote(Opcionfechas.PDesde, Opcionfechas.PHasta, Opcionfechas.PProveedor)

        Opcionfechas.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub GastosPorEgresoDeCajaACuentaDeResultadoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GastosPorEgresoDeCajaACuentaDeResultadoToolStripMenuItem.Click

        Opcionfechas.PEsEgresoCaja = True
        Opcionfechas.ShowDialog()
        If Opcionfechas.PRegresa Then
            Opcionfechas.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeEgresoCaja(Opcionfechas.PDesde, Opcionfechas.PHasta, Opcionfechas.PAbierto, Opcionfechas.PCerrado)

        Opcionfechas.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub menuCobranzasSeñasValesPropios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuCobranzasSeñasValesPropios.Click

        Opcionfechas.PEsSeniasValesPropios = True
        Opcionfechas.ShowDialog()
        If Opcionfechas.PRegresa Then
            Opcionfechas.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeSeniasValesPropios(Opcionfechas.PCliente, Opcionfechas.PDesde, Opcionfechas.PHasta, Opcionfechas.PAbierto, Opcionfechas.PCerrado)

        Opcionfechas.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default


    End Sub
    Private Sub MenuDebitosCreditosQueAfectanFacturas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDebitosCreditosQueAfectanFacturas.Click

        OpcionClienteDesdeHasta.ShowDialog()
        If OpcionClienteDesdeHasta.PRegresar Then
            OpcionClienteDesdeHasta.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeDebitosCreditosPorFacturas(OpcionClienteDesdeHasta.PCliente, OpcionClienteDesdeHasta.PDesde, OpcionClienteDesdeHasta.PHasta)

        OpcionClienteDesdeHasta.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuInformeVentaPorArticulos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuInformeVentaPorArticulos.Click

        OpcionVentasTotalesPorArticulos.PEsPorClienteSucursal = True
        OpcionVentasTotalesPorArticulos.ShowDialog()
        If OpcionVentasTotalesPorArticulos.PRegresar Then
            OpcionVentasTotalesPorArticulos.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeVentasPorArticulos(OpcionVentasTotalesPorArticulos.PCliente, OpcionVentasTotalesPorArticulos.PEspecie, OpcionVentasTotalesPorArticulos.PVariedad, OpcionVentasTotalesPorArticulos.PDesde, OpcionVentasTotalesPorArticulos.PHasta, OpcionVentasTotalesPorArticulos.PSucursal, OpcionVentasTotalesPorArticulos.PPorClienteSucursal)

        OpcionVentasTotalesPorArticulos.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuComparativoCantidadesFacturadas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuComparativoCantidadesFacturadas.Click

        OpcionVentasTotalesPorArticulos.PEsComparativo = True
        OpcionVentasTotalesPorArticulos.ShowDialog()
        If OpcionVentasTotalesPorArticulos.PRegresar Then
            OpcionVentasTotalesPorArticulos.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeComparativoPorArticulos(OpcionVentasTotalesPorArticulos.PCliente, OpcionVentasTotalesPorArticulos.PEspecie, OpcionVentasTotalesPorArticulos.PVariedad, OpcionVentasTotalesPorArticulos.PMes, OpcionVentasTotalesPorArticulos.PAnio, OpcionVentasTotalesPorArticulos.PSucursal)

        OpcionVentasTotalesPorArticulos.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuFacturasCedidas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturasCedidas.Click

        OpcionCesion.ShowDialog()
        If OpcionCesion.PRegresar Then
            OpcionCesion.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeFacturasCedidas(OpcionCesion.PTipoDestino, OpcionCesion.PDestino, OpcionCesion.PFechaDesde, OpcionCesion.PFechaHasta, OpcionCesion.PSoloPendientes)

        OpcionCesion.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuFacturasSaldos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFacturasSaldos.Click

        OpcionVentasYCostosDeLotes.PEsFacturasCobranzas = True
        OpcionVentasYCostosDeLotes.ShowDialog()
        If OpcionVentasYCostosDeLotes.PRegresar Then
            OpcionVentasYCostosDeLotes.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeFacturasSaldos(OpcionVentasYCostosDeLotes.PDesde, OpcionVentasYCostosDeLotes.PHasta, OpcionVentasYCostosDeLotes.PAbierto, OpcionVentasYCostosDeLotes.PCerrado, OpcionVentasYCostosDeLotes.PEsExportacion, OpcionVentasYCostosDeLotes.PEsDomestica, OpcionVentasYCostosDeLotes.PPuntoDeVenta, OpcionVentasYCostosDeLotes.PVendedor)

        OpcionVentasYCostosDeLotes.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuMermasDeLotesEntreFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuMermasDeLotesEntreFecha.Click

        OpcionCompraVentaPorLotes.PEsMerma = True
        OpcionCompraVentaPorLotes.ShowDialog()
        If OpcionCompraVentaPorLotes.PRegresar Then
            OpcionCompraVentaPorLotes.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeMermaPorArticulos(OpcionCompraVentaPorLotes.PProveedor, OpcionCompraVentaPorLotes.PNombre, OpcionCompraVentaPorLotes.PDesde, OpcionCompraVentaPorLotes.PHasta, OpcionCompraVentaPorLotes.PEspecie)

        OpcionCompraVentaPorLotes.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuOrdenesDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenesDePago.Click

        OpcionPagos.ShowDialog()
        If OpcionPagos.PRegresar Then
            OpcionPagos.Dispose()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        InformeOrdenesDePago(OpcionPagos.PTipo, OpcionPagos.PEmisor, OpcionPagos.PEmpleado, OpcionPagos.PDesde, OpcionPagos.PHasta, OpcionPagos.PEstado)
        OpcionPagos.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MenuStrip1_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

        If e.ClickedItem.Name <> "MenuTablas" And Not ExisteDatosEmpresa Then
            MsgBox("Debe Completar Datos del la Empresa antes de Ingresar al Sistema.", MsgBoxStyle.Critical)
        End If

    End Sub
    Private Sub CanjeChequeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuReemplazoChequesPropios.Click

        UnCanjeCheque.Show()

    End Sub
    Private Sub DesHabilitarMenu()

        For Each mnuItem As ToolStripItem In MenuStrip1.Items
            mnuItem.Enabled = False
        Next
    End Sub
    Private Sub MenuParametrosDeImpresion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If Not FormularioOK(UnSeteoImpresora) Then Exit Sub

        UnSeteoImpresora.ShowDialog()
        UnSeteoImpresora.Dispose()

    End Sub
    Private Sub MenuControlImpresoras_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuControlImpresoras.Click

        If Not FormularioOK(UnSeteoImpresora) Then Exit Sub

        UnSeteoImpresora.ShowDialog()
        UnSeteoImpresora.Dispose()

    End Sub
    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        CuilNoValidos.Show()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        ArreglaSaldosIniciales.Show()

    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Exit Sub
        Copiafechaafechaentregaenremitoscabeza.Show()

    End Sub
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        Cantidades.Show()

    End Sub
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        Copiafechaafechaentregaenremitoscabeza.Show()

    End Sub
    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click

        ArreglaStock.Show() 'Recorre los remitos y facturas desde una fecha selecciona lotes con errores que no descontaba stok de remitos y facturas  B con lotes N y biseversa,da el total de lotes asignados, por lote.
        ' no cuenta las facturas con parte b y n.
    End Sub
    Private Sub MenuOrdenesCompraAticulos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenesCompraAticulos.Click

        If Not PermisoLectura(3) Then Exit Sub

        If Not FormularioOK(UnaOrdenCompra) Then Exit Sub

        UnaOrdenCompra.POrden = 0
        UnaOrdenCompra.PTipo = 1
        UnaOrdenCompra.Show()

    End Sub
    Private Sub MenuOrdenesCompraInsumos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuOrdenesCompraInsumos.Click

        If Not FormularioOK(UnaOrdenCompra) Then Exit Sub

        UnaOrdenCompra.POrden = 0
        UnaOrdenCompra.PTipo = 2
        UnaOrdenCompra.Show()

    End Sub
    Private Sub MenuListaOrdenesCompraArticulos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuListaOrdenesCompraArticulos.Click

        If Not FormularioOK(ListaOrdenDeCompra) Then Exit Sub

        ListaOrdenDeCompra.Show()

    End Sub
    Private Sub AnalisisCompraImportacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AnalisisCompraImportacion.Click

        If Not FormularioOK(AnalisisComprasImportacion) Then Exit Sub

        AnalisisComprasImportacion.Show()

    End Sub
    Private Sub MenuObrasSociales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuObrasSociales.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 46
        UnaTabla.Show()

    End Sub

    Private Sub MenuConceptosSueldos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuConceptosSueldos.Click

        If Not FormularioOK(UnaTabla) Then Exit Sub

        UnaTabla.PBloqueaFunciones = Not PermisoEscritura(15)
        UnaTabla.Ptipo = 34
        UnaTabla.Show()

    End Sub

    
    
    
End Class
