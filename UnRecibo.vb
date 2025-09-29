Option Explicit On
Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnRecibo
    Public PNota As Double
    Public PAbierto As Boolean
    Public PTipoNota As Integer
    Public PEmisor As Integer
    Public PBloqueaFunciones As Boolean
    Public PImporte As Double
    Public PEsTr As Boolean
    Public PEsTrOPagoEspecial As Boolean  'Solo para eleccion de medios pagos (Cheque de 3ros. y B. 
    Public PManual As Boolean
    Public PDiferenciaDeCambio As Boolean
    Public PImputa As Boolean
    Public PEsPorCuentaYOrden As Boolean
    Public PEsEgresoCaja As Boolean
    Public PConexion As String
    Public PConexionN As String
    '
    Dim DtNotaDetalle As DataTable
    Dim DtNotaCabeza As DataTable
    Dim DtMedioPago As DataTable
    Dim DtNotaLotes As DataTable
    Dim DtComprobantesCabeza As DataTable
    Dim DtFacturasCabeza As DataTable
    Dim DtLiquidacionCabeza As DataTable
    Dim DtNVLPCabeza As DataTable
    Dim DtSaldosIniciales As DataTable
    Dim DtGridCompro As DataTable
    Dim DtGrid As DataTable
    Dim DtGridLotes As New DataTable
    Dim DtSena As DataTable
    Dim DtFormasPago As DataTable
    Dim DtRetencionesAutomaticas As DataTable
    Dim DtRetencionProvincia As DataTable
    Dim DtRetencionProvinciaAux As DataTable
    '
    Private MiEnlazador As New BindingSource
    '
    Dim Calle As String
    Dim Localidad As String
    Dim Provincia As String
    Dim Cuit As String
    ' 
    Dim cb As ComboBox
    Dim ConexionNota As String
    Dim TotalFacturas As Decimal
    Dim TotalConceptos As Decimal
    Dim UltimoNumero As Double = 0
    Dim UltimoNumeroRetencion As Integer = 0
    Dim ImputacionDeOtros As Decimal = 0
    Dim LetraIva As Integer
    Dim PACuenta As Integer = 0
    Dim UltimaFechaW As DateTime
    Dim UltimafechaContableW As DateTime
    Dim FechaAnt As DateTime
    Dim TablaIva(0) As Double
    Dim MonedaEmisor As Integer
    Dim TipoAsiento As Integer
    Dim ReciboOficialAnt As Decimal
    Dim MesRetencion4 As Integer
    Dim AnioRetencion4 As Integer
    Dim ComprobantesMarcados As Boolean = False
    'Variables Impresion. 
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim LineasParaImpresion As Integer = 0
    Dim LineasParaImpresionImputacion As Integer = 0
    'variables para imprimir retenciones manuales.
    Dim ClaveRetencion As Integer
    Dim NumeroRetencion As Integer
    Dim NombreRetencion As String
    Dim ImporteRetencion As Decimal
    Dim PagoNetoMesRetencion As Decimal
    Dim RetencionPagadaMes As Decimal
    Dim TopeMesRetencion As Decimal
    Dim AlicuotaRetencion As Decimal
    Private Sub UnaNotaTerceros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Conexion = "" Then Conexion = PConexion : ConexionN = PConexionN 'Si es llamado de otro proyecto. 

        Select Case PTipoNota
            Case 5, 6, 7, 8, 50, 70, 500, 700, 13005, 13006, 13007, 13008
                UnReciboDebitoCredito.PTipoNota = PTipoNota
                UnReciboDebitoCredito.PNota = PNota
                UnReciboDebitoCredito.PAbierto = PAbierto
                UnReciboDebitoCredito.PBloqueaFunciones = PBloqueaFunciones
                UnReciboDebitoCredito.ShowDialog()
                Me.Close()
                Exit Sub
        End Select

        Select Case PTipoNota
            Case 60, 65, 600, 64, 604
                If Not PermisoEscritura(8) Then PBloqueaFunciones = True
            Case Else
                MsgBox("Tipo Nota " & PTipoNota & " No Prevista.")
                Me.Close() : Exit Sub
        End Select

        Me.Top = 50

        If GCaja = 0 And PNota = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PNota <> 0 Then
            Select Case PTipoNota
                Case 600, 65, 64
                    If EsReemplazoCheque(PTipoNota, PNota, PAbierto) Then
                        UnChequeReemplazo.PTipoNota = PTipoNota
                        UnChequeReemplazo.PNota = PNota
                        UnChequeReemplazo.PAbierto = PAbierto
                        UnChequeReemplazo.PBloqueaFunciones = PBloqueaFunciones
                        UnChequeReemplazo.ShowDialog()
                        Me.Close()
                        Exit Sub
                    End If
            End Select
        End If

        GridCompro.AutoGenerateColumns = False
        GridCompro.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        If PNota = 0 Then
            PideDatosEmisor()
            If PEmisor = 0 Then Me.Close() : Exit Sub
            If GPuntoDeVenta = 0 Then
                MsgBox("No Tiene Definido Punto de Venta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close() : Exit Sub
            End If
            If Not ExistePuntoDeVenta(GPuntoDeVenta) Then
                MsgBox("Punto de Venta " & GPuntoDeVenta & " No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
                Exit Sub
            End If
            If EsPuntoDeVentaZ(GPuntoDeVenta) Then
                MsgBox("Punto de Venta del Operador " & GPuntoDeVenta & " Reservado Para Ticket Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
                Exit Sub
            End If
            If PTipoNota = 65 Or PTipoNota = 64 Then
                If HallaSaldoCtaCteCliente(PEmisor, PAbierto) > 0 Then
                    If MsgBox("Tipo Comprobante no Permitido con Saldo Cta. Cte. Deudor. Desea Continuar?.", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.No Then
                        Me.Close() : Exit Sub
                    End If
                End If
            End If
        End If

        If (PTipoNota = 600 Or PTipoNota = 65 Or PTipoNota = 64) And PNota = 0 Then
            If TieneNotasPendientes(PEmisor) Then
                If MsgBox("Tiene Nota de Debito o Crédito Pendientes Por Diferencia de Factura. Desea Continuar?.", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Me.Close() : Exit Sub
                End If
            End If
        End If

        If PAbierto Then
            ConexionNota = Conexion
        Else : ConexionNota = ConexionN
        End If

        If PTipoNota = 60 Or PTipoNota = 65 Or PTipoNota = 64 Then LlenaCombo(ComboEmisor, "", "Clientes") : Label16.Text = "Cliente"
        If PTipoNota = 600 Or PTipoNota = 604 Then LlenaCombo(ComboEmisor, "", "Proveedores") : Label16.Text = "Proveedor"
        If PTipoNota = 600 Then LlenaCombo(ComboACuenta, "", "Proveedores")

        ComboClienteOperacion.DataSource = Nothing
        ComboClienteOperacion.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes WHERE DeOperacion = 1;")
        Dim Row As DataRow = ComboClienteOperacion.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboClienteOperacion.DataSource.rows.add(Row)
        ComboClienteOperacion.DisplayMember = "Nombre"
        ComboClienteOperacion.ValueMember = "Clave"
        ComboClienteOperacion.SelectedValue = 0
        With ComboClienteOperacion
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        ArmaTipoIva(ComboTipoIva)

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

        ArmaIvaProveedores(ComboIva)
        ComboIva.SelectedValue = 1
        With ComboIva
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ArmaTablaIva(TablaIva)

        ComboNegocio.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;")
        Row = ComboNegocio.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboNegocio.DataSource.rows.add(Row)
        ComboNegocio.DisplayMember = "Nombre"
        ComboNegocio.ValueMember = "Clave"
        ComboNegocio.SelectedValue = 0
        With ComboNegocio
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboNegocio_SelectionChangeCommitted(Nothing, Nothing)
        With ComboCosteo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PTipoNota = 600 And Not PEsTr Then ArmaMedioPagoOrden(DtFormasPago, PAbierto, PNota)
        If PEsTr And PTipoNota = 600 Then
            If Not PEsTrOPagoEspecial Then
                ArmaTrucha(DtFormasPago, PAbierto, PNota)
            Else
                ArmaTruchaEspecial(DtFormasPago, PAbierto, PNota)
            End If
        End If
        If PTipoNota = 604 Then ArmaMedioPagoCobranzaProveedores(DtFormasPago, PAbierto, PNota)
        If PTipoNota = 60 And Not PEsTr Then ArmaMedioPagoCobranza(DtFormasPago, PAbierto, PNota)
        If PTipoNota = 60 And PEsTr Then ArmaCobranzaTrucha(DtFormasPago, PAbierto, PNota)
        If PTipoNota = 64 Then ArmaMedioPagoOrdenPagoClientes(DtFormasPago)
        If PTipoNota = 65 Then ArmaMedioPagoDevolucionSenia(DtFormasPago)

        If PTipoNota = 60 Then LabelTipoNota.Text = "Recibo de Cobro" : Me.BackColor = Color.Pink
        If PTipoNota = 64 Then LabelTipoNota.Text = "Devolución a Cliente"
        If PTipoNota = 65 Then LabelTipoNota.Text = "Devolución Seña a Cliente" : LabelReciboOficial.Text = "Numero Vale" : LabelFechaNota.Text = "Fecha Vale"
        If PTipoNota = 600 Then LabelTipoNota.Text = "Orden de Pago" : Me.BackColor = Color.LightCoral
        If PTipoNota = 604 Then LabelTipoNota.Text = "Devolución del Proveedor" : Me.BackColor = Color.LightSteelBlue

        GModificacionOk = False

        If PNota = 0 Then ArmaListaDeRetencionesATerceros()

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
            GridCompro.Columns("Candado").Visible = False
        End If

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionNota = Conexion
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionNota = ConexionN
        End If

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        LlenaCombosGrid()

        If PTipoNota = 65 Then Panel5.Visible = True

        TextLetra.Visible = False

        If PTipoNota = 600 Then
            Panel3.Visible = True
            If Not PAbierto Then ComboIva.Enabled = False
            If ComboTipoIva.SelectedValue = Exterior Then
                ComboIva.Enabled = False
            Else
                ComboIva.Enabled = True
            End If
        End If
        If PTipoNota = 64 Then
            Panel3.Visible = True
            ComboIva.Enabled = False
        End If
        If PTipoNota = 65 Then
            Panel7.Visible = True
            TextLetra.Visible = False
        End If
        If PTipoNota = 60 Or PTipoNota = 604 Then
            Panel3.Visible = True
            Label9.Visible = False
            LabelImporteOrden.Text = "Importe Cobranza"
            Label19.Visible = False
            ComboIva.Visible = False
        End If

        If PEsEgresoCaja Then
            Label7.Visible = True : ComboConceptoGasto.Visible = True
        End If

        If GGeneraAsiento Then
            Dim Conta = TieneTabla1(TipoAsiento)
            If Conta < 0 Then
                MsgBox("Error Base de Datos al leer Seteo de Documento.")
                Me.Close()
                Exit Sub
            End If
            If Conta = 0 Then
                LabelCuentas.Visible = False
                ListCuentas.Visible = False
                PictureLupaCuenta.Visible = False
            End If
        End If

        ListCuentas.Clear()
        UnTextoParaRecibo.Dispose()

        If GTipoIva = 2 Then
            ComboIva.SelectedValue = 0
            ComboIva.Enabled = False
        End If

        Select Case PTipoNota
            Case 60, 600, 65, 64, 604
                UltimaFechaW = UltimaFechaPuntoVentaTipoNota(Conexion)
        End Select

        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub UnRecibo_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones And Not PImputa Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If CInt(LabelCaja.Text) <> GCaja And PNota = 0 Then
            MsgBox("Modificación Solo Permitida para Caja " & LabelCaja.Text & ". Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        GridCompro.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim Fecha As Date
        If PanelFechaContable.Visible Then
            If TextFechaContable.Text = "" Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Debe Informar Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            Fecha = CDate(TextFechaContable.Text)
        Else : Fecha = DateTime1.Value
        End If
        If PNota = 0 Then
            If CierreContableCerrado(Fecha.Month, Fecha.Year) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If
        If PTipoNota = 600 And PEsTr Then
            If CierreContableCerrado(Fecha.Month, Fecha.Year) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        Dim RowsBusqueda() As DataRow

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtNotaLotesAux As DataTable = DtNotaLotes.Copy

        Dim DtFacturasCabezaAux As DataTable = DtFacturasCabeza.Copy
        Dim DtNVLPCabezaAux As DataTable = DtNVLPCabeza.Copy
        Dim DtLiquidacionCabezaAux As DataTable = DtLiquidacionCabeza.Copy
        Dim DtSenaAux As DataTable = DtSena.Copy
        Dim DtComprobantesCabezaAux As DataTable = DtComprobantesCabeza.Copy
        Dim DtSaldosInicialesAux As DataTable = DtSaldosIniciales.Copy

        'Actualiza Saldo de Comprobantes Imputados.
        ActualizaComprobantes("M", DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux)

        'Actualiza Notas Cabeza.
        If DtNotaCabezaAux.Rows(0).Item("Manual") Then
            If Format(DtNotaCabezaAux.Rows(0).Item("FechaReciboOficial"), "dd/MM/yyyy") <> CDate(TextFechaManual.Text) Then
                DtNotaCabezaAux.Rows(0).Item("FechaReciboOficial") = TextFechaManual.Text
            End If
        End If
        If Panel5.Visible = True Then
            If DtNotaCabezaAux.Rows(0).Item("FechaReciboOficial") <> CDate(TextFechaReciboOficial.Text) Then
                DtNotaCabezaAux.Rows(0).Item("FechaReciboOficial") = TextFechaReciboOficial.Text
            End If
        End If
        If PanelFechaContable.Visible = True Then
            If DtNotaCabezaAux.Rows(0).Item("FechaContable") <> CDate(TextFechaContable.Text) Then
                DtNotaCabezaAux.Rows(0).Item("FechaContable") = TextFechaContable.Text
            End If
        End If

        'Actualiza detalle Comprobantes Imputados.
        For Each Row1 As DataRow In DtGridCompro.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtNotaDetalleAux.Select("TipoComprobante = " & Row1("Tipo") & " AND Comprobante = " & Row1("Comprobante"))
                If RowsBusqueda.Length <> 0 Then
                    If RowsBusqueda(0).Item("Importe") <> Row1.Item("Asignado") Then
                        If Row1.Item("Asignado") = 0 Then
                            RowsBusqueda(0).Delete()
                        Else
                            RowsBusqueda(0).Item("Importe") = Row1.Item("Asignado")
                        End If
                    End If
                Else
                    If Row1.Item("Asignado") <> 0 Then
                        Dim Row As DataRow = DtNotaDetalleAux.NewRow()
                        Row("TipoNota") = PTipoNota
                        Row("Nota") = DtNotaCabeza.Rows(0).Item("Nota")
                        Row("TipoComprobante") = Row1("Tipo")
                        Row("Comprobante") = Row1("Comprobante")
                        Row("Importe") = Row1.Item("Asignado")
                        DtNotaDetalleAux.Rows.Add(Row)
                    End If
                End If
            End If
        Next

        'Actualiza Lotes Imputados.
        For Each Row1 As DataRow In DtGridLotes.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtNotaLotesAux.Select("Lote = " & Row1("Lote") & " AND Secuencia = " & Row1("Secuencia"))
                If RowsBusqueda.Length = 0 Then
                    Dim Row As DataRow = DtNotaLotesAux.NewRow()
                    Row("TipoNota") = PTipoNota
                    Row("Nota") = PNota
                    Row("Operacion") = Row1("Operacion")
                    Row("Lote") = Row1("Lote")
                    Row("Secuencia") = Row1("Secuencia")
                    Row("ImporteConIva") = 0
                    Row("ImporteSinIva") = 0
                    DtNotaLotesAux.Rows.Add(Row)
                End If
            End If
        Next
        For Each Row1 As DataRow In DtNotaLotesAux.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtGridLotes.Select("Lote = " & Row1("Lote") & " AND Secuencia = " & Row1("Secuencia"))
                If RowsBusqueda.Length = 0 Then Row1.Delete()
            End If
        Next
        If Not IsNothing(DtNotaLotesAux.GetChanges) Then
            ProrroteaImportesLotes(DtNotaLotesAux)
        End If

        'Actualiza Archivo de distribucion retenciones por provincia.
        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone
        If PNota = 0 Then
            For Each Row As DataRow In DtRetencionProvinciaAux.Rows
                Dim Row1 As DataRow = DtRetencionProvinciaWW.NewRow
                Row1("TipoNota") = PTipoNota
                Row1("Nota") = 0
                Row1("Provincia") = Row("Provincia")
                Row1("Retencion") = Row("Retencion")
                Row1("Comprobante") = Row("Comprobante")
                Row1("Importe") = Row("Importe")
                DtRetencionProvinciaWW.Rows.Add(Row1)
            Next
        End If

        If IsNothing(DtNotaDetalleAux.GetChanges) And IsNothing(DtNotaCabezaAux.GetChanges) And IsNothing(DtNotaLotesAux.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GridCompro.Focus()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If PNota = 0 Then
                If Not ArmaArchivosAsiento("A", DtAsientoCabezaAux, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            Else
                If Not ArmaArchivosAsiento("M", DtAsientoCabezaAux, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            End If
        End If

        If PNota = 0 Then
            If HacerAlta(DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtNotaLotesAux) Then
                ArmaArchivos()
            End If
        Else
            If HacerModificacion(DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtNotaLotesAux) Then
                ArmaArchivos()
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If GCaja = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If CInt(LabelCaja.Text) <> GCaja And Not GAdministrador Then
            MsgBox("Modificación Solo Permitida para Caja " & LabelCaja.Text & " o un Administrador. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Fecha As Date
        If PanelFechaContable.Visible Then
            Fecha = CDate(TextFechaContable.Text)
        Else : Fecha = DateTime1.Value
        End If
        If CierreContableCerrado(Fecha.Month, Fecha.Year) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        If DtNotaCabeza.Rows(0).Item("ChequeRechazado") <> 0 Or DtNotaCabeza.Rows(0).Item("ClaveChequeReemplazado") <> 0 Then
            MsgBox("Anulación Nota Por Cheque Rechazado o Reemplazado, Debe Realizarce Por Menu Rechazo de Cheques. Operación se CANCELA.")
            Exit Sub
        End If

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtNotaLotesAux As DataTable = DtNotaLotes.Copy

        'Actualiza Saldo de Comprobantes Imputados.
        Dim DtFacturasCabezaAux As DataTable = DtFacturasCabeza.Copy
        Dim DtNVLPCabezaAux As DataTable = DtNVLPCabeza.Copy
        Dim DtLiquidacionCabezaAux As DataTable = DtLiquidacionCabeza.Copy
        Dim DtSenaAux As DataTable = DtSena.Copy
        Dim DtComprobantesCabezaAux As DataTable = DtComprobantesCabeza.Copy
        Dim DtSaldosInicialesAux As DataTable = DtSaldosIniciales.Copy

        ActualizaComprobantes("M", DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux)

        If Not (IsNothing(DtFacturasCabezaAux.GetChanges) And IsNothing(DtNVLPCabezaAux.GetChanges) And IsNothing(DtLiquidacionCabezaAux.GetChanges) And IsNothing(DtSenaAux.GetChanges) And IsNothing(DtComprobantesCabezaAux.GetChanges)) Then
            MsgBox("Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Resul As Double

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If (PTipoNota = 600 And PEsTr = False) Or PTipoNota = 65 Or PTipoNota = 64 Then
            For Each Row As DataRow In DtGrid.Rows
                If Row("MedioPago") = 2 Or Row("Mediopago") = 3 Then
                    Dim Anulado As Boolean, Rechazado As Boolean, Depositado As Boolean, Entregado As Boolean, Afectado As Boolean
                    If Not EstadoCheque(Row("Mediopago"), Row("ClaveCheque"), ConexionNota, Anulado, Rechazado, Depositado, Entregado, Afectado) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Error Base de Datos.")
                        Exit Sub
                    End If
                    If Rechazado Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Existe Nota de Rechazo. Operación se CANCELA.")
                        Exit Sub
                    End If
                    If Anulado Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Cheque Anulado. Operación se CANCELA.")
                        Exit Sub
                    End If
                    If Depositado Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Cheque Depositado. Operación se CANCELA.")
                        Exit Sub
                    End If
                    If Afectado Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Esta Afectado a Otra Orden. Operación se CANCELA.")
                        Exit Sub
                    End If
                End If
                If Row("MedioPago") = 14 Then
                    Dim Anulado As Boolean, Rechazado As Boolean, Depositado As Boolean, Entregado As Boolean, Afectado As Boolean
                    If Not EstadoCheque(Row("MedioPago"), Row("ClaveCheque"), ConexionNota, Anulado, Rechazado, Depositado, Entregado, Afectado) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Error Base de Datos.")
                        Exit Sub
                    End If
                    If Depositado Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Debito Auto.Dife. " & Row("Comprobante") & " no se puede Borrar, Fue Depositado. Operación se CANCELA.")
                        Exit Sub
                    End If
                End If
                If Row("MedioPago") = 3 Then
                    If ExiteChequeEnPaseCaja(ConexionNota, Row("ClaveCheque")) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " en Proceso de Pase de Caja. Operación se CANCELA.")
                        Exit Sub
                    End If
                End If
            Next
        End If

        If (PTipoNota = 60 And Not PEsTr) Or PTipoNota = 604 Then
            For Each Row As DataRow In DtGrid.Rows
                If Row("MedioPago") = 3 Then
                    Dim Anulado As Boolean, Rechazado As Boolean, Depositado As Boolean, Entregado As Boolean, Afectado As Boolean
                    If Not EstadoCheque(Row("MedioPago"), Row("ClaveCheque"), ConexionNota, Anulado, Rechazado, Depositado, Entregado, Afectado) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Error Base de Datos.")
                        Exit Sub
                    End If
                    If Entregado Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar, fue usado para Pago o Depositado. Operación se CANCELA.")
                        Exit Sub
                    End If
                    If Rechazado Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Existe Nota de Rechazo. Operación se CANCELA.")
                        Exit Sub
                    End If
                    If Anulado Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Cheque Anulado. Operación se CANCELA.")
                        Exit Sub
                    End If
                    If ExiteChequeEnPaseCaja(ConexionNota, Row("ClaveCheque")) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " en Proceso de Pase de Caja. Operación se CANCELA.")
                        Exit Sub
                    End If
                End If
            Next
        End If

        If PEsTr = False Then
            Select Case PTipoNota
                Case 600, 65, 64
                    For Each Row As DataRow In DtGrid.Rows
                        If Row("MedioPago") = 2 Or Row("MedioPago") = 3 Then
                            If ChequeReemplazado(Row("MedioPago"), Row("ClaveCheque"), PTipoNota, PNota, ConexionNota) Then
                                Me.Cursor = System.Windows.Forms.Cursors.Default
                                MsgBox("Cheque " & Row("Numero") & " no se puede Borrar, fue Reemplazado. Operación se CANCELA.")
                                Exit Sub
                            End If
                        End If
                    Next
            End Select
        End If

        If DtNotaCabezaAux.Rows(0).Item("Importe") <> DtNotaCabezaAux.Rows(0).Item("Saldo") Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Nota Tiene Imputaciones, Debe anularlas para Continuar. Operación se CANCELA.")
            Exit Sub
        End If

        If MsgBox("Nota se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        'Actualiza Saldo de Comprobantes Imputados.
        DtFacturasCabezaAux = DtFacturasCabeza.Copy
        DtNVLPCabezaAux = DtNVLPCabeza.Copy
        DtLiquidacionCabezaAux = DtLiquidacionCabeza.Copy
        DtSenaAux = DtSena.Copy
        DtComprobantesCabezaAux = DtComprobantesCabeza.Copy
        DtSaldosInicialesAux = DtSaldosIniciales.Copy

        ' Fue anulado por que se obliga a anular las imputaciones manualmente.
        '    ActualizaComprobantes("B", DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux)

        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        '
        If Not HallaAsientosCabeza(TipoAsiento, DtNotaCabeza.Rows(0).Item("Nota"), DtAsientoCabezaAux, ConexionNota) Then Exit Sub
        For Each Row As DataRow In DtAsientoCabezaAux.Rows
            Row("Estado") = 3
        Next

        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone

        DtNotaCabezaAux.Rows(0).Item("Estado") = 3

        Resul = ActualizaNota("B", DtGrid, DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtNotaLotesAux)
        If Resul = -1 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Baja Fue Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If CInt(LabelCaja.Text) <> GCaja And Not GAdministrador Then
            MsgBox("Modificación Solo Permitida para Caja " & LabelCaja.Text & " o un Administrador. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Fecha As Date
        If PanelFechaContable.Visible Then
            Fecha = CDate(TextFechaContable.Text)
        Else : Fecha = DateTime1.Value
        End If
        If CierreContableCerrado(Fecha.Month, Fecha.Year) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        If Not EsReciboManual(Val(Strings.Left(MaskedNota.Text, 4))) And Not PEsEgresoCaja Then
            MsgBox("Opcion Valida solo para Cobranza Manual o Egreso de Caja. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue <> 3 Then
            MsgBox("Nota Debe ser ANULADA previamente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtRetencionProvinciaAux As DataTable = DtRetencionProvincia.Copy
        Dim DtNotaLotesAux As DataTable = DtNotaLotes.Copy

        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        '
        If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & TipoAsiento & " AND Documento = " & DtNotaCabeza.Rows(0).Item("Nota") & ";", ConexionNota, DtAsientoCabezaAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        If DtAsientoCabezaAux.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaAux.Rows(0).Item("Asiento") & ";", ConexionNota, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If
        DtAsientoCabezaAux.Rows(0).Delete()
        For Each Row As DataRow In DtAsientoDetalleAux.Rows
            Row.Delete()
        Next

        Dim DtCheques As New DataTable
        If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = 3 AND TipoOrigen = 60 AND CompOrigen = " & DtNotaCabeza.Rows(0).Item("Nota") & ";", ConexionNota, DtCheques) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If
        For Each Row As DataRow In DtCheques.Rows
            Row.Delete()
        Next

        DtNotaCabezaAux.Rows(0).Delete()
        For Each Row As DataRow In DtNotaDetalleAux.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtMedioPagoAux.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtNotaLotesAux.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtRetencionProvinciaAux.Rows
            Row.Delete()
        Next

        If MsgBox("Nota se Borrara Definitivamente del Sistema. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                Resul = GrabaTabla(DtNotaCabezaAux, "RecibosCabeza", ConexionNota)
                If Resul <= 0 Then Exit Try

                Resul = GrabaTabla(DtNotaDetalleAux, "RecibosDetalle", ConexionNota)
                If Resul <= 0 Then Exit Try

                Resul = GrabaTabla(DtMedioPagoAux, "RecibosDetallePago", ConexionNota)
                If Resul <= 0 Then Exit Try

                If Not IsNothing(DtAsientoCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaAux, "AsientosCabeza", ConexionNota)
                    If Resul <= 0 Then Exit Try
                End If
                If Not IsNothing(DtAsientoDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleAux, "AsientosDetalle", ConexionNota)
                    If Resul <= 0 Then Exit Try
                End If

                If Not IsNothing(DtRetencionProvinciaAux.GetChanges) Then
                    Resul = GrabaTabla(DtRetencionProvinciaAux, "RecibosRetenciones", ConexionNota)
                    If Resul <= 0 Then Exit Try
                End If

                If Not IsNothing(DtNotaLotesAux.GetChanges) Then
                    Resul = GrabaTabla(DtRetencionProvinciaAux, "RecibosLotes", ConexionNota)
                    If Resul <= 0 Then Exit Try
                End If

                If Not IsNothing(DtCheques.GetChanges) Then
                    Resul = GrabaTabla(DtCheques, "Cheques", ConexionNota)
                    If Resul <= 0 Then Exit Try
                End If

                Scope.Complete()
                Resul = 1000
            End Using
        Catch ex As TransactionException
            Resul = -2
        End Try

        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul < 0 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Recibo Borrado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonComprobantesAImputar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonComprobantesAImputar.Click

        Dim SinImporte As Boolean

        If CDec(TextTotalRecibo.Text) = 0 And TextTotalOrden.Text = "" And PTipoNota = 600 Then
            SinImporte = True
        End If

        UNComprobanteAImputar.PSinImporte = SinImporte
        UNComprobanteAImputar.PDtGridCompro = DtGridCompro
        UNComprobanteAImputar.PTipo = PTipoNota
        UNComprobanteAImputar.PAbierto = PAbierto
        UNComprobanteAImputar.PTotalConceptos = TextTotalRecibo.Text
        UNComprobanteAImputar.PTipoIva = ComboTipoIva.SelectedValue
        UNComprobanteAImputar.PMoneda = ComboMoneda.SelectedValue
        UNComprobanteAImputar.PCambio = TextCambio.Text
        UNComprobanteAImputar.ShowDialog()
        DtGridCompro = UNComprobanteAImputar.PDtGridCompro
        If SinImporte Then TextTotalOrden.Text = UNComprobanteAImputar.PImputado
        UNComprobanteAImputar.Dispose()

        ArmaGridCompro()

        CalculaTotales()

    End Sub
    Private Sub CheckRetencionManual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckRetencionManual.Click

        If CheckRetencionManual.Checked Then Exit Sub

        Dim RowsBusqueda() As DataRow
        Dim HayRetenciones As Boolean

        For I As Integer = DtGrid.Rows.Count - 1 To 0 Step -1
            Dim Row As DataRow = DtGrid.Rows(I)
            RowsBusqueda = DtRetencionesAutomaticas.Select("Clave = " & Row("MedioPago"))
            If RowsBusqueda.Length <> 0 Then
                Row.Delete()
                HayRetenciones = True
            End If
        Next
        If HayRetenciones Then CalculaRetencion()

    End Sub
    Private Sub ComboNegocio_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboNegocio.SelectionChangeCommitted

        If ComboNegocio.SelectedValue <> 0 Then
            If DtGridLotes.Rows.Count <> 0 Then
                MsgBox("No se puede Asignar un Negocio si se Imputo Lotes.")
                ComboNegocio.SelectedValue = 0
                Exit Sub
            End If
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "IntFechaDesde <= " & Format(DateTime1.Value, "yyyyMMdd") & " AND IntFechaHasta >= " & Format(DateTime1.Value, "yyyyMMdd") & ";"
        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & ComboNegocio.SelectedValue & " AND Cerrado = 0 AND " & SqlFecha
        ComboCosteo.DataSource = New DataTable
        ComboCosteo.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboCosteo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        ComboCosteo.DataSource.rows.add(Row)
        ComboCosteo.DisplayMember = "Nombre"
        ComboCosteo.ValueMember = "Costeo"
        ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub ButtonMediosDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMediosDePago.Click

        Dim DtFormasPagoW As DataTable = DtFormasPago.Copy

        'No permite informar en grid si total orden de compra no esta informado.  
        If PTipoNota = 600 Then
            If TextTotalOrden.Text = "" Then
                MsgBox("Falta Informar Importe de la Orden de Pago.")
                TextTotalOrden.Focus()
                Exit Sub
            End If
            If ComboIva.SelectedValue = 1 And PAbierto And ComboTipoIva.SelectedValue <> Exterior Then
                MsgBox("Falta Informar IVA de la Orden de Pago.")
                ComboIva.Focus() : Exit Sub
            End If
            If PEsEgresoCaja Then
                For I As Integer = DtFormasPagoW.Rows.Count - 1 To 0 Step -1
                    Dim Row As DataRow = DtFormasPagoW.Rows(I)
                    If Row("Tipo") = 4 Then
                        If Row.RowState <> DataRowState.Deleted Then Row.Delete()
                    End If
                Next
            End If
        End If

        'No permite informar en grid si total cobranza no esta informado.  
        If PTipoNota = 60 Or PTipoNota = 604 Or PTipoNota = 64 Then
            If TextTotalOrden.Text = "" Then
                MsgBox("Falta Informar Importe del Comprobante.")
                TextTotalOrden.Focus()
                Exit Sub
            End If
        End If

        'Filtra Moneda y Cambio.  
        If PTipoNota = 600 Or PTipoNota = 604 Then
            If ComboTipoIva.SelectedValue = Exterior Then
                For I As Integer = DtFormasPagoW.Rows.Count - 1 To 0 Step -1
                    Dim Row As DataRow = DtFormasPagoW.Rows(I)
                    If (Row("Tipo") = 1 Or Row("Tipo") = 3) And Row("Clave") <> ComboMoneda.SelectedValue Then
                        Row.Delete()
                    End If
                Next
            End If
            If ComboTipoIva.SelectedValue = Exterior And CDec(TextCambio.Text) = 0 Then
                MsgBox("Falta Informar Cambio.")
                TextCambio.Focus() : Exit Sub
            End If
        End If

        UnMediosPago.PTipoNota = PTipoNota
        UnMediosPago.PEmisor = PEmisor
        UnMediosPago.PDtGrid = DtGrid
        UnMediosPago.PAbierto = PAbierto
        If PNota = 0 Then
            UnMediosPago.PBloqueaFunciones = False
        Else : UnMediosPago.PBloqueaFunciones = True
        End If
        UnMediosPago.PDtFormasPago = DtFormasPagoW
        UnMediosPago.PDtRetencionProvincia = DtRetencionProvinciaAux
        If DtNotaCabeza.Rows(0).Item("ChequeRechazado") <> 0 Then UnMediosPago.PEsChequeRechazado = True
        If ComboTipoIva.SelectedValue = Exterior Then UnMediosPago.PEsExterior = True
        UnMediosPago.PMoneda = ComboMoneda.SelectedValue
        If ComboTipoIva.SelectedValue = Exterior And PTipoNota = 600 Then
            UnMediosPago.PEsOrdenPagoImportador = True
        End If
        UnMediosPago.PCambio = CDbl(TextCambio.Text)
        UnMediosPago.PDtRetencionesAutomaticas = DtRetencionesAutomaticas
        UnMediosPago.PDiferenciaDeCambio = PDiferenciaDeCambio
        UnMediosPago.PImporte = CDbl(TextTotalRecibo.Text)
        If TextTotalOrden.Text <> "" Then
            UnMediosPago.PImporteAInformar = CDbl(TextTotalOrden.Text)
        End If
        UnMediosPago.PEsTr = PEsTr
        If PEsTrOPagoEspecial Then UnMediosPago.PEsTrOPagoEspecial = True ' Solo para que mande cheques terceros B. Solo para PEsTrOPagoEspecial.
        UnMediosPago.PEsRetencionManual = CheckRetencionManual.Checked
        If PNota = 0 Then UnMediosPago.PEsAlta = True
        UnMediosPago.ShowDialog()
        DtGrid = UnMediosPago.PDtGrid

        UnMediosPago.Dispose()

        CalculaTotales()

        'Imputa si el importe orden fue la suma pre-marcada en documentos.
        If ComprobantesMarcados Then
            For Each Row As DataGridViewRow In GridCompro.Rows
                If Row.Cells("Seleccion").Value Then
                    Row.Cells("Asignado").Value = Row.Cells("Saldo").Value
                End If
            Next
            ComprobantesMarcados = False
            CalculaTotales()
        End If

    End Sub
    Private Sub ButtonDatosCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDatosCliente.Click

        If ComboEmisor.SelectedValue = 0 Then Exit Sub

        Select Case PTipoNota
            Case 60, 65, 64
                UnDatosEmisor.PEsCliente = True
            Case Else
                UnDatosEmisor.PEsProveedor = True
        End Select

        UnDatosEmisor.PEmisor = ComboEmisor.SelectedValue
        UnDatosEmisor.ShowDialog()

    End Sub
    Private Sub ButtonImportePorLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportePorLotes.Click

        SeleccionarVarios.PAbierto = PAbierto
        SeleccionarVarios.PTipoNota = PTipoNota
        SeleccionarVarios.PNota = PNota
        SeleccionarVarios.PEsImportePorLotesRecibos = True
        SeleccionarVarios.Show()

    End Sub
    Private Sub ButtonEliminarTodo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarTodo.Click

        If MsgBox("Medios Pagos se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtGrid.Clear()
        CalculaRetencion()
        CalculaTotales()

    End Sub
    Private Sub ButtonLotesAImputar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonLotesAImputar.Click

        If ComboNegocio.SelectedValue <> 0 Then
            MsgBox("No se puede Imputar Lotes Si se Asigna a Un Costeo.")
            Exit Sub
        End If

        If DtNotaCabeza.Rows(0).Item("ACuenta") <> 0 Then
            OpcionLotesAImputar.PEsPorCuentaYOrden = True
        End If

        OpcionLotesAImputar.PEmisor = PACuenta
        OpcionLotesAImputar.PDtGrid = DtGridLotes
        OpcionLotesAImputar.ShowDialog()
        OpcionLotesAImputar.Dispose()

    End Sub
    Private Sub ButtonExportarExcelLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcelLotes.Click

        DetalleLoteLiquidadosEnUnaOrdenPago(PNota, ConexionNota)

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Nota debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PAbierto And Not (PTipoNota = 600 Or PTipoNota = 64) Then
            If DtNotaCabeza.Rows(0).Item("Impreso") Then
                If MsgBox("Recibo fue Impreso. Quiere Re-Imprimir?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
        End If

        ErrorImpresion = False
        Paginas = 0
        If Not PAbierto Then Copias = 1

        Dim print_document As New PrintDocument

        If PTipoNota = 60 Or PTipoNota = 604 Then
            LineasParaImpresion = 0
            LineasParaImpresionImputacion = 0
            Copias = 1
            AddHandler print_document.PrintPage, AddressOf Print_PrintCobranza
            print_document.Print()
        End If

        If PTipoNota = 600 Or PTipoNota = 64 Then
            LineasParaImpresion = 0
            LineasParaImpresionImputacion = 0
            AddHandler print_document.PrintPage, AddressOf Print_PrintOrdenPago
            print_document.Print()
            'Imprime retenciones.
            If Not ImprimirRetenciones() Then Exit Sub
        End If

        If PAbierto Then
            If Not GrabaImpreso(DtNotaCabeza, Conexion) Then Exit Sub
            DtNotaCabeza.Rows(0).Item("Impreso") = True
        Else
            If Not GrabaImpreso(DtNotaCabeza, ConexionN) Then Exit Sub
            DtNotaCabeza.Rows(0).Item("Impreso") = True
        End If

        ArmaArchivos()

    End Sub
    Private Function ImprimirRetenciones() As Boolean

        Dim DtRetencionesAux As New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre,TopeMes,CodigoRetencion,AlicuotaRetencion,TipoIva,Formula,0.0 AS Importe FROM Tablas WHERE Tipo= 25;", Conexion, DtRetencionesAux) Then End

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtRetencionesAux.Select("Clave = " & Row("MedioPago"))
            If RowsBusqueda.Length <> 0 Then
                ClaveRetencion = Row("MedioPago")
                NumeroRetencion = Row("Comprobante")
                ImporteRetencion = Row("Importe")
                NombreRetencion = RowsBusqueda(0).Item("Nombre")
                TopeMesRetencion = RowsBusqueda(0).Item("TopeMes")
                AlicuotaRetencion = RowsBusqueda(0).Item("AlicuotaRetencion")
                ErrorImpresion = False
                Paginas = 0
                Copias = 1
                Dim print_document As New PrintDocument
                If CheckRetencionManual.Checked Then
                    AddHandler print_document.PrintPage, AddressOf Print_RetencionManual
                Else
                    Select Case RowsBusqueda(0).Item("Formula")
                        Case 0
                            AddHandler print_document.PrintPage, AddressOf Print_RetencionManual
                        Case 1
                            TotalesImprisionRetencionGanancia(RowsBusqueda(0))
                            AddHandler print_document.PrintPage, AddressOf Print_RetencionGanancia
                        Case 4
                            AlicuotaRetencion = LeerPadron(RowsBusqueda(0).Item("Clave"), DateTime1.Value, CuitNumerico(Cuit))
                            If AlicuotaRetencion = -100 Then AlicuotaRetencion = 0
                            AddHandler print_document.PrintPage, AddressOf Print_RetencionFormula4
                    End Select
                End If
                print_document.Print()
                If ErrorImpresion Then Return False
            End If
        Next

        Return True

    End Function
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PNota = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = TipoAsiento
        If PAbierto Then
            ListaAsientos.PDocumentoB = PNota
        Else
            ListaAsientos.PDocumentoN = PNota
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonCompAsociado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCompAsociado.Click

        MsgBox(HallaCompAsociado(DtNotaCabeza.Rows(0).Item("TipoCompAsociado"), DtNotaCabeza.Rows(0).Item("CompAsociado")))

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PNota = 0
        PEmisor = 0
        ComboIva.SelectedValue = 0
        TextTotalOrden.Text = ""
        TextTotalOrden.Focus()
        UnaNotaTerceros_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonIgualRazon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonIgualRazon.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If (Not IsNothing(DtNotaDetalle.GetChanges) Or Not IsNothing(DtNotaCabeza.GetChanges)) And Not GModificacionOk Then
            MsgBox("Debe Grabar Modificaciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PNota = 0
        TextTotalOrden.Text = ""
        ComboIva.SelectedValue = 0
        TextTotalOrden.Focus()
        If Not ArmaArchivos() Then Me.Close() : Exit Sub

    End Sub
    Private Sub PictureLupaCuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupaCuenta.Click

        If PNota <> 0 Then Exit Sub

        If CDbl(TextTotalRecibo.Text) = 0 Then
            MsgBox("Falta Informar Conceptos.")
            Exit Sub
        End If

        Dim Neto As Decimal = CDec(TextTotalRecibo.Text)

        SeleccionarCuentaDocumento.PListaDeCuentas = New List(Of ItemCuentasAsientos)

        Dim Item As New ListViewItem

        For I As Integer = 0 To ListCuentas.Items.Count - 1
            Dim Fila As New ItemCuentasAsientos
            Dim CuentaStr As String = ListCuentas.Items.Item(I).SubItems(0).Text
            Fila.Cuenta = Mid(CuentaStr, 1, 3) & Mid(CuentaStr, 5, 6) & Mid(CuentaStr, 12, 2)
            Fila.ImporteB = CDec(ListCuentas.Items.Item(I).SubItems(1).Text)
            Fila.ImporteN = 0
            SeleccionarCuentaDocumento.PListaDeCuentas.Add(Fila)
        Next

        SeleccionarCuentaDocumento.PSoloUnImporte = True
        SeleccionarCuentaDocumento.PImporteB = Neto
        SeleccionarCuentaDocumento.ShowDialog()
        If SeleccionarCuentaDocumento.PAcepto Then
            ListCuentas.Clear()
            For Each Fila As ItemCuentasAsientos In SeleccionarCuentaDocumento.PListaDeCuentas
                Item = New ListViewItem(Format(Fila.Cuenta, "000-000000-00"))
                Item.SubItems.Add(Fila.ImporteB.ToString)
                Item.SubItems.Add("0")
                ListCuentas.Items.Add(Item)
            Next
        End If

        SeleccionarCuentaDocumento.Dispose()

    End Sub
    Private Sub PictureAlmanaque_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueManual.Click

        If TextFechaManual.Text = "" Then
            Calendario.PFecha = "01/01/1800"
        Else : Calendario.PFecha = TextFechaManual.Text
        End If

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaManual.Text = ""
        Else : TextFechaManual.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueReciboOficial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueReciboOficial.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaReciboOficial.Text = ""
        Else : TextFechaReciboOficial.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
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
    Private Sub TextBultos_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBultos.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextTotalOrden_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextTotalOrden.KeyPress

        EsNumerico(e.KeyChar, TextTotalOrden.Text, GDecimales)

    End Sub
    Private Sub TextTotalOrden_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextTotalOrden.Validating

        If PTipoNota = 60 Or PTipoNota = 604 Then Exit Sub

        If TextTotalOrden.Text = "" Then Exit Sub

        If Not IsNumeric(TextTotalOrden.Text) Then
            MsgBox("Importe de la Orden Incorrecto.")
            TextTotalOrden.Text = ""
            TextTotalOrden.Focus()
            Exit Sub
        End If
        If CDbl(TextTotalOrden.Text) = 0 Then
            MsgBox("Falta Importe de la Orden.")
            TextTotalOrden.Text = ""
            TextTotalOrden.Focus()
            Exit Sub
        End If

        If Not PAbierto Then Exit Sub

        If ComboIva.SelectedValue = 1 Then ComboIva.Focus() : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CalculaRetencion()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboIva_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboIva.Validating

        If IsNothing(ComboIva.SelectedValue) Then
            MsgBox("Iva Incorrecto.")
            ComboIva.SelectedValue = 1
            ComboIva.Focus()
            Exit Sub
        End If
        If ComboIva.SelectedValue = 1 Then
            MsgBox("Iva Incorrecto.")
            ComboIva.Focus()
            Exit Sub
        End If

        If TextTotalOrden.Text = "" Then TextTotalOrden.Focus() : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CalculaRetencion()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub DateTime1_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles DateTime1.Validating

        If PTipoNota <> 600 Then Exit Sub

        Dim Formula As Integer
        For Each row As DataRow In DtGrid.Rows
            Formula = HallaFormulaRetencion(row("MedioPago"))
            If Formula = 4 Then Exit For
        Next

        If Formula <> 4 Then Exit Sub

        If PNota <> 0 Then
            Dim FechaAux As DateTime = DtNotaCabeza.Rows(0).Item("Fecha")
            If DateTime1.Value.Month <> FechaAux.Month Or DateTime1.Value.Year <> FechaAux.Year Then
                If MsgBox("El Cambio de Fecha no corresponde con las Retenciones/Percepciones efectudadas. Quiere Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    DateTime1.Value = FechaAux
                End If
            End If
        End If

        If PNota = 0 Then
            If (MesRetencion4 <> DateTime1.Value.Month And MesRetencion4 <> 0) Or (AnioRetencion4 <> DateTime1.Value.Year And AnioRetencion4 <> 0) Then
                DtGrid.Clear()
                CalculaRetencion()
            End If
        End If

    End Sub
    Private Sub ComboMoneda_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboMoneda.Validating

        If IsNothing(ComboMoneda.SelectedValue) Then ComboMoneda.SelectedValue = 0

        If ComboMoneda.SelectedValue = 1 Then
            TextCambio.Text = 1
        Else : TextCambio.Text = ""
        End If

    End Sub
    Private Sub TextCambio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCambio.KeyPress

        EsNumerico(e.KeyChar, TextCambio.Text, 3)

    End Sub
    Private Function ArmaArchivos() As Boolean                     'ArmaArchivos

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()
        CreaDtGridCompro()
        CreaDtRetencionProvinciaAux()
        CreaDtGridLotes()

        Dim Sql As String
        Dim Row1 As DataRow
        Dim RowsBusqueda() As DataRow

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM RecibosCabeza WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaCabeza) Then Return False
        If PNota <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then
            If Not LlenaDatosEmisor(PEmisor) Then Return False
        Else
            If Not LlenaDatosEmisor(DtNotaCabeza.Rows(0).Item("Emisor")) Then Return False
        End If

        If (PTipoNota = 60 Or PTipoNota = 600 Or PTipoNota = 604) And ComboTipoIva.SelectedValue = Exterior Then ArmaMedioPagoCobranzaExterior(DtFormasPago)
        If Not (PTipoNota = 60 Or PTipoNota = 600 Or PTipoNota = 604 Or PTipoNota = 65) And ComboTipoIva.SelectedValue = Exterior Then ArmaMedioPagoOtrasExterior(DtFormasPago, True)

        If ComboTipoIva.SelectedValue = Exterior Then
            If Not (PTipoNota = 600 Or PTipoNota = 60 Or PTipoNota = 604) Then
                MsgBox("Comprobante No Permitido Para Tipo Iva Exportación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If PDiferenciaDeCambio Then
                MonedaEmisor = 1
                ComboMoneda.SelectedValue = 1
                PanelMoneda.Visible = False
            Else
                PanelMoneda.Visible = True
                ComboMoneda.SelectedValue = MonedaEmisor
            End If
            LlenaCombosGrid()
        Else
            PanelMoneda.Visible = False
            If PDiferenciaDeCambio Then
                MsgBox("Cliente o Proveedor No Permitido para Este Tipo de Nota.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If PNota = 0 Then AgregaCabeza()

        MuestraCabeza()

        ReciboOficialAnt = DtNotaCabeza.Rows(0).Item("ReciboOficial")
        If DtNotaCabeza.Rows(0).Item("Tr") = True Then PEsTr = True
        If PEsTr = True And PermisoTotal Then LabelTr.Visible = True
        PACuenta = DtNotaCabeza.Rows(0).Item("ACuenta")
        PEmisor = DtNotaCabeza.Rows(0).Item("Emisor")
        If PNota <> 0 Then
            If DtNotaCabeza.Rows(0).Item("Comentario") = "O.P.Esp." Then
                PEsTrOPagoEspecial = True
            Else
                PEsTrOPagoEspecial = False
            End If
        End If

        If DtNotaCabeza.Rows(0).Item("ACuenta") <> 0 Then
            PanelAcuenta.Visible = True
        End If

        Select Case PTipoNota
            Case 600
                If DtNotaCabeza.Rows(0).Item("ACuenta") <> 0 Then PaneLotes.Visible = True
        End Select

        DtRetencionProvincia = New DataTable
        If PAbierto Then
            If Not Tablas.Read("SELECT * FROM RecibosRetenciones WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";", Conexion, DtRetencionProvincia) Then Return False
            For Each Row As DataRow In DtRetencionProvincia.Rows
                Row1 = DtRetencionProvinciaAux.NewRow
                Row1("Retencion") = Row("Retencion")
                Row1("Provincia") = Row("Provincia")
                Row1("Comprobante") = Row("Comprobante")
                Row1("Importe") = Row("Importe")
                DtRetencionProvinciaAux.Rows.Add(Row1)
            Next
        End If

        DtNotaDetalle = New DataTable
        Sql = "SELECT * FROM RecibosDetalle WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaDetalle) Then Return False

        DtNotaLotes = New DataTable
        Sql = "SELECT * FROM RecibosLotes WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaLotes) Then Return False
        For Each Row1 In DtNotaLotes.Rows
            Dim Row As DataRow = DtGridLotes.NewRow
            Row("Operacion") = Row1("Operacion")
            Row("Lote") = Row1("Lote")
            Row("Secuencia") = Row1("Secuencia")
            Row("Cantidad") = HallaCantidadLote(Row1("Lote"), Row1("Secuencia"), Row1("Operacion"))
            DtGridLotes.Rows.Add(Row)
        Next

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM RecibosDetallePago WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtMedioPago) Then Return False

        For Each Row As DataRow In DtMedioPago.Rows
            Row1 = DtGrid.NewRow
            Row1("Item") = Row("Item")
            Row1("MedioPago") = Row("MedioPago")
            Row1("Importe") = Row("Importe")
            Row1("Cambio") = Row("Cambio")
            Row1("Bultos") = Row("Bultos")
            Row1("Detalle") = Row("Detalle")
            Row1("Alicuota") = Row("Alicuota")
            Row1("Neto") = Row("Neto")
            Row1("ImporteIva") = Row("Neto") * Row("Alicuota") / 100
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            Row1("ClaveInterna") = Row("ClaveInterna")
            Row1("ClaveChequeVisual") = 0
            Row1("Banco") = Row("Banco")
            Row1("Cuenta") = Row("Cuenta")
            Row1("Serie") = ""
            Row1("Numero") = 0
            Row1("EmisorCheque") = ""
            Row1("Fecha") = "1/1/1800"
            Row1("echeq") = False
            If Row("MedioPago") = 3 Then Row1("ClaveChequeVisual") = Row("ClaveCheque")
            If Row("ClaveCheque") <> 0 And (Row("MedioPago") = 2 Or Row("MedioPago") = 3) Then
                If Not HallaDatosCheque(Row("MedioPago"), Row("ClaveCheque"), Row1("Serie"), Row1("Numero"), Row1("EmisorCheque"), Row1("Fecha"), Row1("eCheq")) Then Return False
            End If
            Row1("TieneLupa") = False
            If PAbierto Then
                RowsBusqueda = DtRetencionProvincia.Select("Retencion = " & Row("MedioPago"))
                If RowsBusqueda.Length <> 0 Then Row1("TieneLupa") = True
            End If
            DtGrid.Rows.Add(Row1)
        Next

        'Muestra Comprobantes a Imputar.
        DtFacturasCabeza = New DataTable
        DtNVLPCabeza = New DataTable
        DtComprobantesCabeza = New DataTable
        DtLiquidacionCabeza = New DataTable
        DtSena = New DataTable
        DtSaldosIniciales = New DataTable

        If PTipoNota = 60 And Not PEsTr Then
            If Not ArmaConFacturas(PTipoNota) Then Return False
            If Not ArmaConSaldosIniciales(PTipoNota) Then Return False
            If Not ArmaConNVLP() Then Return False
            If Not ArmaConNotas(70) Then Return False 'N.Credito del Cliente. 
            If Not ArmaConNotas(5) Then Return False 'N.Debito financiera a Cliente. 
            If Not ArmaConNotas(13005) Then Return False 'N.Debito financiera a Cliente Interna.     
            If Not ArmaConNotas(64) Then Return False 'Devolucións a Clientes.     
        End If
        If PTipoNota = 60 And PEsTr Then
            If Not ArmaConFacturas(PTipoNota) Then Return False
            If Not ArmaConSaldosIniciales(PTipoNota) Then Return False
            If Not ArmaConNVLP() Then Return False
        End If
        If PTipoNota = 600 And Not PEsTr Then
            If Not ArmaConFacturas(PTipoNota) Then Return False
            If Not ArmaConSaldosIniciales(PTipoNota) Then Return False
            If Not ArmaConLiquidaciones() Then Return False
            If Not ArmaConNotas(500) Then Return False 'N.Debito Financiera del Proveedor. 
            If Not ArmaConNotas(8) Then Return False 'N.Credito Financiera a Proveedor. 
            If Not ArmaConNotas(13008) Then Return False 'N.Credito Financiera a Proveedor Interna.     
            If Not ArmaConNotas(604) Then Return False 'Devolucións del Proveedor.     
        End If
        If PTipoNota = 600 And PEsTr Then
            If Not ArmaConFacturas(PTipoNota) Then Return False
            If Not ArmaConSaldosIniciales(PTipoNota) Then Return False
            If Not ArmaConLiquidaciones() Then Return False
        End If

        Select Case PTipoNota
            Case 60, 600
            Case Else
                ButtonComprobantesAImputar.Visible = False
                GridCompro.Visible = False
                Panel9.Visible = False
        End Select

        DtGridCompro.Clear()

        'Procesa Facturas.
        For Each Row As DataRow In DtFacturasCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 2
            Row1("TipoVisible") = 2
            If Row("EsZ") Then Row1("TipoVisible") = 44
            Row1("Comprobante") = Row("Factura")
            If PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 50 Or PTipoNota = 70 Or PTipoNota = 60 Then
                Row1("Recibo") = Row("Factura")
                Row1("Fecha") = Row("Fecha")
            Else
                Row1("Recibo") = Row("ReciboOficial")
                Row1("Fecha") = Row("FechaFactura")
            End If
            Row1("Comentario") = Row("Comentario")
            Row1("Importe") = Row("Importe")
            If PTipoNota = 60 Then
                Row1("Importe") = Row("Importe") + Row("Percepciones")
            End If
            Row1("Moneda") = Row("Moneda")
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next
        'Procesa NVLP.
        For Each Row As DataRow In DtNVLPCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 800
            Row1("TipoVisible") = 800
            Row1("Comprobante") = Row("Liquidacion")
            Row1("Recibo") = Row("ReciboOficial")
            Row1("Comentario") = Row("Comentario")
            Row1("Fecha") = Row("FechaLiquidacion")
            Row1("Importe") = Row("Importe")
            Row1("Moneda") = 1
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next
        'Procesa Liquidacion.
        For Each Row As DataRow In DtLiquidacionCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 10
            Row1("TipoVisible") = 10
            Row1("Comprobante") = Row("Liquidacion")
            Row1("Recibo") = Row("Liquidacion")
            Row1("Comentario") = Row("Comentario")
            Row1("Fecha") = Row("FechaContable")
            Row1("Importe") = Row("Importe")
            Row1("Moneda") = 1
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next
        'Procesa Notas.
        For Each Row As DataRow In DtComprobantesCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = Row("TipoNota")
            Row1("TipoVisible") = Row("TipoNota")
            Row1("Comprobante") = Row("Nota")
            If Row("TipoNota") = 50 Or Row("TipoNota") = 70 Or Row("TipoNota") = 500 Or Row("TipoNota") = 700 Then
                Row1("Recibo") = Row("ReciboOficial")
                Row1("Fecha") = Row("FechaReciboOficial")
            Else
                Row1("Recibo") = Row("Nota")
                Row1("Fecha") = Row("Fecha")
            End If
            Row1("Comentario") = Row("Comentario")
            Row1("Importe") = Row("Importe")
            Row1("Moneda") = Row("Moneda")
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next
        'Procesa SaldosIniciales.
        For Each Row As DataRow In DtSaldosIniciales.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 900
            Row1("TipoVisible") = 900
            Row1("Comprobante") = Row("Clave")
            Row1("Recibo") = Row("Clave")
            Row1("Fecha") = Row("Fecha")
            Row1("Comentario") = ""
            Row1("Importe") = Row("Importe")
            If Row1("Importe") < 0 Then Row1("Importe") = -Row1("Importe")
            Row1("Moneda") = Row("Moneda")
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next

        For Each Row As DataRow In DtNotaDetalle.Rows
            RowsBusqueda = DtGridCompro.Select("Tipo = " & Row("TipoComprobante") & " AND Comprobante = " & Row("Comprobante"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Asignado") = Row("Importe")
            End If
        Next

        'Borra los documentos con saldo 0 y no tienen asignacion. 
        DtGridCompro.AcceptChanges()
        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Saldo") = 0 And Row("Asignado") = 0 Then Row.Delete()
        Next
        DtGridCompro.AcceptChanges()
        ArmaGridCompro()

        If PEsEgresoCaja Then
            DtGridCompro.Clear()
        End If

        ''''''''    GridCompro.DataSource = DtGridCompro

        TipoAsiento = PTipoNota
        If PTipoNota = 60 And DtNotaCabeza.Rows(0).Item("EsExterior") Then TipoAsiento = 61
        If PTipoNota = 60 And PEsTr Then TipoAsiento = 62
        If PTipoNota = 600 And DtNotaCabeza.Rows(0).Item("EsExterior") Then TipoAsiento = 601
        If PTipoNota = 600 And DtNotaCabeza.Rows(0).Item("Tr") Then TipoAsiento = 605
        If PTipoNota = 600 And PEsEgresoCaja Then TipoAsiento = 607

        If DtNotaCabeza.Rows(0).Item("Manual") Then
            PanelReciboManual.Visible = True
            LabelInterno.Text = "Recibo Manual"
            If PNota = 0 Then
                MaskedNota.ReadOnly = False
            Else : MaskedNota.ReadOnly = True
            End If
        Else
            LabelInterno.Text = "Nro. Interno"
        End If

        ComboClienteOperacion.Enabled = False

        Select Case PTipoNota               'Se puede borrar?.
            Case 6, 8, 500, 700
            Case Else
                ComboNegocio.Enabled = False
                ComboCosteo.Enabled = False
        End Select                          '-----------------

        If PNota <> 0 Then
            PanelMoneda.Enabled = False
            Panel3.Enabled = False
            PictureLupaCuenta.Enabled = False
            ButtonAceptar.Text = "Modificar Recibo"
            LabelPuntoDeVenta.Visible = False
        Else
            PanelMoneda.Enabled = True
            Panel3.Enabled = True
            PictureLupaCuenta.Enabled = True
            ButtonAceptar.Text = "Grabar Recibo"
            LabelPuntoDeVenta.Visible = True
        End If

        TextImpresionOrdenPago.Text = ""
        If PTipoNota = 600 Then
            Panel6.Visible = True
        Else
            Panel6.Visible = False
        End If

        If PAbierto And PTipoNota = 600 Then
            Panel8.Visible = True
        Else
            Panel8.Visible = False
        End If
        If PNota <> 0 Then
            Panel8.Enabled = False
        Else
            Panel8.Enabled = True
        End If

        If PEsTrOPagoEspecial Then
            TextComentario.ReadOnly = True
        End If

        If DtNotaCabeza.Rows(0).Item("Estado") = 3 Then
            GridCompro.ReadOnly = True
        Else : GridCompro.ReadOnly = False
        End If

        CalculaTotales()

        ''''''       AddHandler DtGridCompro.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGridCompro_ColumnChanging)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Function ArmaConFacturas(ByVal TipoNota As Integer) As Boolean

        Dim Sql As String = ""
        Dim Emisor As Integer

        If PACuenta <> 0 Then
            Emisor = PACuenta
        Else
            Emisor = PEmisor
        End If

        Dim SqlTr As String = " AND Tr = 0"
        If PEsTr Then SqlTr = " AND Tr = 1"

        '------------------------------------------------------------------------------------------------------------
        'ClienteOperacion = 0 para que no aparezcan las generadas en el modulo de exportacion.-----------------------
        '------------------------------------------------------------------------------------------------------------

        If TipoNota = 5 Or TipoNota = 7 Or TipoNota = 50 Or TipoNota = 70 Or TipoNota = 60 Then
            Sql = "SELECT * FROM FacturasCabeza WHERE ClienteOperacion = 0 AND Estado <> 3 AND FacturasCabeza.Cliente = " & Emisor & SqlTr & " ORDER BY Factura,Fecha;"
        Else
            If PEsTr Then
                Sql = "SELECT *,0 AS EsZ FROM FacturasProveedorCabeza WHERE Rendicion = 0 AND Tr = 1 AND Estado = 1 AND Liquidacion = 0 AND Proveedor = " & Emisor & " ORDER BY Factura,Fecha;"
            Else
                Sql = "SELECT *,0 AS EsZ  FROM FacturasProveedorCabeza WHERE Rendicion = 0 AND Tr = 0 AND Estado = 1 AND Liquidacion = 0 AND Proveedor = " & Emisor & " ORDER BY Factura,Fecha;"
            End If
        End If
        If Not Tablas.Read(Sql, ConexionNota, DtFacturasCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConNVLP() As Boolean

        Dim Sql As String = ""

        Dim SqlTr As String = " AND Tr = 0"
        If PEsTr Then SqlTr = " AND Tr = 1"

        Sql = "SELECT * FROM NVLPCabeza WHERE Estado = 1 AND Cliente = " & PEmisor & SqlTr & " ORDER BY Liquidacion,Fecha;"
        If Not Tablas.Read(Sql, ConexionNota, DtNVLPCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConNotas(ByVal TipoNota As Integer) As Boolean

        Dim Sql As String = ""
        Dim Emisor As Integer

        If PACuenta <> 0 Then
            Emisor = PACuenta
        Else
            Emisor = PEmisor
        End If

        Dim SqlTr As String = " AND Tr = 0"
        If PEsTr Then SqlTr = " AND Tr = 1"

        Sql = "SELECT * FROM RecibosCabeza WHERE Estado = 1 AND TipoNota = " & TipoNota & " AND Emisor = " & Emisor & SqlTr & " ORDER BY Nota,Fecha;"
        If Not Tablas.Read(Sql, ConexionNota, DtComprobantesCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConLiquidaciones() As Boolean

        Dim Sql As String = ""
        Dim Emisor As Integer

        If PACuenta <> 0 Then
            Emisor = PACuenta
        Else
            Emisor = PEmisor
        End If

        Dim SqlTr As String = " AND Tr = 0"
        If PEsTr Then SqlTr = " AND Tr = 1"

        Sql = "SELECT * FROM LiquidacionCabeza WHERE Estado = 1 AND LiquidacionCabeza.Proveedor = " & Emisor & SqlTr & " ORDER BY Liquidacion,Fecha;"

        If PTipoNota = 600 Then
            If PEsTr Then
                Sql = "SELECT * FROM LiquidacionCabeza WHERE Tr = 1 AND Estado = 1 AND LiquidacionCabeza.Proveedor = " & Emisor & " ORDER BY Liquidacion,Fecha;"
            Else
                Sql = "SELECT * FROM LiquidacionCabeza WHERE Tr = 0 AND Estado = 1 AND LiquidacionCabeza.Proveedor = " & Emisor & " ORDER BY Liquidacion,Fecha;"
            End If
        End If

        If Not Tablas.Read(Sql, ConexionNota, DtLiquidacionCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConSaldosIniciales(ByVal TipoNota As Integer) As Boolean

        Dim Sql As String = ""
        Dim Emisor As Integer

        If PACuenta <> 0 Then
            Emisor = PACuenta
        Else
            Emisor = PEmisor
        End If

        If TipoNota = 5 Or TipoNota = 7 Or TipoNota = 50 Or TipoNota = 70 Or TipoNota = 60 Then
            Sql = "SELECT * FROM SaldosInicialesCabeza WHERE Tipo = 3 AND Importe > 0 AND Emisor = " & Emisor & ";"
        Else
            Sql = "SELECT * FROM SaldosInicialesCabeza WHERE Tipo = 2 AND Importe < 0 AND Emisor = " & Emisor & ";"
        End If
        If Not Tablas.Read(Sql, ConexionNota, DtSaldosIniciales) Then Return False

        Return True

    End Function
    Private Sub ArmaGridCompro()

        GridCompro.Rows.Clear()

        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Asignado") <> 0 Then
                GridCompro.Rows.Add(Row("Operacion"), "", Row("Tipo"), Row("TipoVisible"), Row("Comprobante"), Row("Recibo"), Row("Comentario"), Row("Fecha"), Row("Importe"), Row("Moneda"), Row("Saldo"), Row("Asignado"))
            End If
        Next

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtNotaCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Nota")
        AddHandler Enlace.Format, AddressOf FormatMaskedNota
        AddHandler Enlace.Parse, AddressOf ParseMaskedNota
        MaskedNota.DataBindings.Clear()
        MaskedNota.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Emisor")
        ComboEmisor.DataBindings.Clear()
        ComboEmisor.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "ClienteOperacion")
        ComboClienteOperacion.DataBindings.Clear()
        ComboClienteOperacion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "ACuenta")
        ComboACuenta.DataBindings.Clear()
        ComboACuenta.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "ReciboOficial")
        AddHandler Enlace.Format, AddressOf FormatReciboOficial
        AddHandler Enlace.Parse, AddressOf ParseReciboOficial
        MaskedReciboOficial.DataBindings.Clear()
        MaskedReciboOficial.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "RetencionManual")
        CheckRetencionManual.DataBindings.Clear()
        CheckRetencionManual.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Bultos")
        AddHandler Enlace.Format, AddressOf FormatEntero
        TextBultos.DataBindings.Clear()
        TextBultos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalRecibo.DataBindings.Clear()
        TextTotalRecibo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "CodigoIva")
        ComboIva.DataBindings.Clear()
        ComboIva.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Saldo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextSaldo.DataBindings.Clear()
        TextSaldo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Moneda")
        ComboMoneda.DataBindings.Clear()
        ComboMoneda.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cambio")
        AddHandler Enlace.Format, AddressOf FormatCambio
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextCambio.DataBindings.Clear()
        TextCambio.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "ConceptoGasto")
        ComboConceptoGasto.DataBindings.Clear()
        ComboConceptoGasto.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Dim Row As DataRowView = MiEnlazador.Current

        If Row("Costeo") <> 0 Then
            ComboNegocio.SelectedValue = HallaNegocio(Row("Costeo"))
            ComboNegocio_SelectionChangeCommitted(Nothing, Nothing)
        End If

        Enlace = New Binding("SelectedValue", MiEnlazador, "Costeo")
        ComboCosteo.DataBindings.Clear()
        ComboCosteo.DataBindings.Add(Enlace)

        If Row("Importe") <> 0 Then
            TextTotalOrden.Text = FormatNumber(Row("Importe"))
        End If

        If Row("FechaReciboOficial") = "01/01/1800" Then
            TextFechaManual.Text = ""
            TextFechaReciboOficial.Text = ""
        Else
            TextFechaManual.Text = Format(DtNotaCabeza.Rows(0).Item("FechaReciboOficial"), "dd/MM/yyyy")
            TextFechaReciboOficial.Text = Format(DtNotaCabeza.Rows(0).Item("FechaReciboOficial"), "dd/MM/yyyy")
        End If

        If Row("FechaContable") = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else
            TextFechaContable.Text = Format(DtNotaCabeza.Rows(0).Item("FechaContable"), "dd/MM/yyyy")
        End If

        FechaAnt = Row("fecha")

    End Sub
    Private Sub FormatMaskedNota(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "000000000000")

    End Sub
    Private Sub ParseMaskedNota(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatCambio(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, 3)

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub FormatReciboOficial(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "000000000000")

    End Sub
    Private Sub ParseReciboOficial(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos
        Dim RowsBusqueda() As DataRow

        If Funcion = "A" Then
            Select Case PTipoNota
                Case 60, 600, 64, 604
                    For Each Row As DataRow In DtGrid.Rows
                        RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                        If RowsBusqueda(0).Item("Tipo") <> 4 Then
                            Item = New ItemListaConceptosAsientos
                            Item.Clave = Row("MedioPago")
                            If Row("Cambio") <> 0 Then
                                Item.Importe = Trunca(Row("Cambio") * Row("Importe"))
                            Else : Item.Importe = Row("Importe")
                            End If
                            If ComboMoneda.SelectedValue <> 1 And Row("Cambio") = 0 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
                            ListaConceptos.Add(Item)
                        End If
                    Next
                    For Each Row As DataRow In DtGrid.Rows
                        RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                        If RowsBusqueda(0).Item("Tipo") = 4 Then
                            Item = New ItemListaConceptosAsientos
                            Item.Clave = Row("MedioPago")
                            Item.Importe = Row("Importe")
                            If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
                            If PTipoNota = 60 Then
                                Item.TipoIva = 9        'Credito fiscal.
                            Else : Item.TipoIva = 11    'Debito fiscal. 
                            End If
                            ListaRetenciones.Add(Item)
                        End If
                    Next
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = 213
                    Item.Importe = CDbl(TextTotalRecibo.Text)
                    If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
                    ListaConceptos.Add(Item)
                Case 65
                    For Each Row As DataRow In DtGrid.Rows
                        RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                        Item = New ItemListaConceptosAsientos
                        Item.Clave = Row("MedioPago")
                        Item.Importe = Row("Importe")
                        If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
                        ListaConceptos.Add(Item)
                    Next
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = 213
                    Item.Importe = CDbl(TextTotalRecibo.Text)
                    If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
                    ListaConceptos.Add(Item)
            End Select

            'Arma Lista con Cuentas definidas en documento.
            If ListCuentas.Visible Then
                Dim Neto As Decimal = CDec(TextTotalRecibo.Text)
                If ComboMoneda.SelectedValue <> 1 Then Neto = Trunca(CDbl(TextCambio.Text) * Neto)
                '
                If Neto <> 0 Then
                    If ListCuentas.Items.Count = 0 Then
                        MsgBox("Falta Informar Cuenta. Operación se CANCELA.")
                        Return False
                    End If
                    Dim ImporteLista As Decimal = 0
                    For I As Integer = 0 To ListCuentas.Items.Count - 1
                        Dim Fila As New ItemCuentasAsientos
                        Dim Cuenta As String = ListCuentas.Items.Item(I).SubItems(0).Text
                        Fila.Cuenta = Mid$(Cuenta, 1, 3) & Mid$(Cuenta, 5, 6) & Mid$(Cuenta, 12, 2)
                        Fila.Importe = CDec(ListCuentas.Items.Item(I).SubItems(1).Text)
                        If ComboMoneda.SelectedValue <> 1 Then Fila.Importe = Trunca(CDec(TextCambio.Text) * Fila.Importe)
                        Fila.Clave = 213
                        ListaCuentas.Add(Fila)
                        ImporteLista = Trunca(ImporteLista + Fila.Importe)
                    Next
                    If ImporteLista <> Neto Then
                        MsgBox("Importe de Cuentas Informada Difiere del Neto de la Factura. " & ImporteLista & " " & Neto)
                        Return False
                    End If
                End If
            End If
        End If
        '
        Dim MontoAfectado As Decimal = 0
        Select Case PTipoNota
            Case 60, 600, 64, 604
                For Each Row As DataRow In DtGridCompro.Rows
                    If Row.RowState <> DataRowState.Deleted Then
                        RowsBusqueda = DtNotaDetalle.Select("TipoComprobante = " & Row("Tipo") & " AND Comprobante = " & Row("Comprobante"))
                        If RowsBusqueda.Length = 0 Then
                            If Row("Asignado") <> 0 Then
                                MontoAfectado = MontoAfectado + Row("Asignado")
                            End If
                        Else
                            If Row("Asignado") <> RowsBusqueda(0).Item("Importe") Then
                                MontoAfectado = MontoAfectado + Row("Asignado") - RowsBusqueda(0).Item("Importe")
                            End If
                        End If
                    End If
                Next
        End Select
        '
        If MontoAfectado <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = 214
            Item.Importe = MontoAfectado
            If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
            ListaConceptos.Add(Item)
        End If

        If MontoAfectado = 0 And Funcion = "M" Then Return True
        '
        Dim Fecha As Date
        If PanelFechaContable.Visible Then
            Fecha = CDate(TextFechaContable.Text)
        Else : Fecha = DateTime1.Value
        End If

        If Not Asiento(TipoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Fecha, PTipoNota) Then Return False

        Return True

    End Function
    Private Sub AgregaCabeza()

        Dim Row As DataRow

        Row = DtNotaCabeza.NewRow
        ArmaNuevoRecibo(Row)
        Row("TipoNota") = PTipoNota
        Row("Nota") = UltimoNumero
        Row("Emisor") = PEmisor
        Row("Fecha") = Now
        Row("CodigoIva") = ComboIva.SelectedValue
        Row("Estado") = 1
        Row("Caja") = GCaja
        Row("Tr") = PEsTr
        Row("Moneda") = ComboMoneda.SelectedValue
        If ComboMoneda.SelectedValue = 1 Then
            Row("Cambio") = 1
        Else : Row("Cambio") = 0
        End If
        Row("Manual") = PManual
        Row("RetencionManual") = False
        Row("DiferenciaDeCambio") = PDiferenciaDeCambio
        If ComboTipoIva.SelectedValue = Exterior Then Row("EsExterior") = True
        If PEsPorCuentaYOrden Then
            Row("ACuenta") = PACuenta
            Row("Comentario") = "Pago Flete. "
        End If
        If PEsTrOPagoEspecial Then
            Row("Comentario") = "O.P.Esp."
        End If
        DtNotaCabeza.Rows.Add(Row)

    End Sub
    Private Sub ProrroteaImportesLotes(ByVal DtNotaLotesAux As DataTable)

        Dim Cantidad As Decimal = 0

        For Each Row As DataRow In DtGridLotes.Rows
            Cantidad = Cantidad + Row("Cantidad")
        Next

        Dim ImporteConIva As Decimal = 0
        Dim ImporteSinIva As Decimal = 0

        If DtNotaCabeza.Rows(0).Item("ACuenta") <> 0 Then
            ImporteConIva = CDec(TextTotalRecibo.Text)
            ImporteSinIva = CDec(TextTotalRecibo.Text)
        Else
            For Each Row As DataRow In DtGrid.Rows
                ImporteConIva = CDec(TextTotalRecibo.Text)
                If Row("MedioPago") = 100 Then
                    ImporteSinIva = ImporteSinIva + Row("Neto")
                End If
            Next
        End If

        Dim IndiceCorreccionConIva As Decimal = ImporteConIva / Cantidad
        Dim IndiceCorreccionSinIva As Decimal = ImporteSinIva / Cantidad

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtNotaLotesAux.Rows
            If Row.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtGridLotes.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
                Row("ImporteConIva") = Trunca(IndiceCorreccionConIva * RowsBusqueda(0).Item("Cantidad"))
                Row("ImporteSinIva") = Trunca(IndiceCorreccionSinIva * RowsBusqueda(0).Item("Cantidad"))
            End If
        Next

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Tipo.DataSource = DtTiposComprobantes(PAbierto)
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Codigo"

        TipoVisible.DataSource = DtTiposComprobantes(PAbierto)
        Row = TipoVisible.DataSource.NewRow()
        Row("Codigo") = 44
        Row("Nombre") = "Ticket Fiscal"
        TipoVisible.DataSource.Rows.Add(Row)
        TipoVisible.DisplayMember = "Nombre"
        TipoVisible.ValueMember = "Codigo"

        Moneda.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 27 order By Nombre;")
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 1
        Row("Nombre") = "PESOS"
        Moneda.DataSource.Rows.Add(Row)
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Moneda.DataSource.Rows.Add(Row)
        Moneda.DisplayMember = "Nombre"
        Moneda.ValueMember = "Clave"

        If PEsEgresoCaja Then
            LlenaComboTablas(ComboConceptoGasto, 29)
            ComboConceptoGasto.SelectedValue = 0
            With ComboConceptoGasto
                .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                .AutoCompleteSource = AutoCompleteSource.ListItems
            End With
        End If

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = CreaDtParaGrid()

    End Sub
    Private Sub CreaDtGridCompro()

        DtGridCompro = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Operacion)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Tipo)

        Dim TipoVisible As New DataColumn("TipoVisible")
        TipoVisible.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(TipoVisible)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Comprobante)

        Dim Comentario As New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGridCompro.Columns.Add(Comentario)

        Dim Recibo As New DataColumn("Recibo")
        Recibo.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Recibo)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGridCompro.Columns.Add(Fecha)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Importe)

        Dim Moneda As New DataColumn("Moneda")
        Moneda.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Moneda)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Saldo)

        Dim SaldoAnt As New DataColumn("SaldoAnt")
        SaldoAnt.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(SaldoAnt)

        Dim Asignado As New DataColumn("Asignado")
        Asignado.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Asignado)

    End Sub
    Private Sub CreaDtRetencionProvinciaAux()

        DtRetencionProvinciaAux = New DataTable

        Dim Retencion As New DataColumn("Retencion")
        Retencion.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Retencion)

        Dim Provincia As New DataColumn("Provincia")
        Provincia.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Provincia)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Comprobante)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtRetencionProvinciaAux.Columns.Add(Importe)

    End Sub
    Private Sub CreaDtGridLotes()

        DtGridLotes = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGridLotes.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGridLotes.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGridLotes.Columns.Add(Secuencia)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGridLotes.Columns.Add(Cantidad)

    End Sub
    Private Function LlenaDatosEmisor(ByVal Cliente As Integer) As Boolean

        Dim Dta As New DataTable
        Dim Sql As String = ""

        If PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 50 Or PTipoNota = 70 Or PTipoNota = 60 Or PTipoNota = 65 Or PTipoNota = 64 Then
            Sql = "SELECT * FROM Clientes WHERE Clave = " & Cliente & ";"
            Dta = Tablas.Leer(Sql)
            If Dta.Rows.Count = 0 Then
                MsgBox("ERROR, Cliente No Existe o Error en la Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            ComboMoneda.SelectedValue = Dta.Rows(0).Item("Moneda")
            MonedaEmisor = Dta.Rows(0).Item("Moneda")
        Else
            Sql = "SELECT * FROM Proveedores WHERE Clave = " & Cliente & ";"
            Dta = Tablas.Leer(Sql)
            If Dta.Rows.Count = 0 Then
                MsgBox("ERROR, Proveedor No Existe o Error en la Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            ComboMoneda.SelectedValue = Dta.Rows(0).Item("Moneda")
            MonedaEmisor = Dta.Rows(0).Item("Moneda")
            PEsEgresoCaja = Dta.Rows(0).Item("EsEgresoCaja")
        End If

        Calle = Dta.Rows(0).Item("Calle")
        Localidad = Dta.Rows(0).Item("Localidad")
        Provincia = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        Cuit = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")

        If Not Dta.Rows(0).Item("Opr") And PAbierto And PNota = 0 Then
            If MsgBox("Cliente/Proveedor Solo Opera a Candado Cerrado. Debe Cambiar Candado en Cliente/Proveedor. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
        End If

        Dta.Dispose()

        Return True

    End Function
    Private Function HacerAlta(ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtSaldosInicialesAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable, ByVal DtNotaLotesAux As DataTable) As Boolean

        If LicenciaVencida(DateTime1.Value) Then End

        'Graba Facturas.
        Dim NumeroNota As Double
        Dim NumeroAsiento As Double
        Dim NumeroW As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion comprobante.
            If ((PTipoNota = 60 And Not PManual) Or PTipoNota = 600 Or PTipoNota = 65 Or PTipoNota = 64 Or PTipoNota = 604) Then NumeroNota = UltimaNumeracionPagoYOrden(PTipoNota, ConexionNota)
            If (PManual And PTipoNota = 60) Then NumeroNota = Val(MaskedNota.Text)
            If NumeroNota < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            '
            DtNotaCabezaAux.Rows(0).Item("TipoNota") = PTipoNota
            DtNotaCabezaAux.Rows(0).Item("Nota") = NumeroNota
            DtNotaCabezaAux.Rows(0).Item("Interno") = NumeroInternoRecibos(LetraIva, PTipoNota, ConexionNota)
            If DtNotaCabezaAux.Rows(0).Item("Interno") <= 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            For Each Row As DataRow In DtNotaDetalleAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("TipoNota") = PTipoNota
                    Row("Nota") = NumeroNota
                End If
            Next

            For Each Row As DataRow In DtNotaLotesAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("TipoNota") = PTipoNota
                    Row("Nota") = NumeroNota
                End If
            Next

            For Each Row As DataRow In DtRetencionProvinciaWW.Rows
                Row("Nota") = NumeroNota
            Next

            'Halla Ultima numeracion Asiento.
            If DtAsientoCabezaAux.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionNota)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Function
                End If
                DtAsientoCabezaAux.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabezaAux.Rows(0).Item("Documento") = NumeroNota
                For Each Row As DataRow In DtAsientoDetalleAux.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            Dim DtGridAux As New DataTable
            DtGridAux = DtGrid.Copy
            DtMedioPagoAux.Rows.Clear()

            For Each Row As DataRow In DtGridAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    If PTipoNota = 600 And HallaTipo(Row("MedioPago")) = 4 Then
                        Row("FechaComprobante") = DateTime1.Value
                        Row("Comprobante") = UltimaNumeracionRetenciones(Row("MedioPago"))
                        If Row("Comprobante") <= 0 Then
                            MsgBox("Error Base de Datos al leer Tabla de Retenciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            Return False
                        End If
                    End If
                End If
            Next

            NumeroW = ActualizaNota("A", DtGridAux, DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtNotaLotesAux)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -10 Then
            MsgBox("Recibo Ya Fue Impreso. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PNota = NumeroNota
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function HacerModificacion(ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtSaldosInicialesAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable, ByVal DtNotaLotesAux As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroAsiento As Double
        Dim Resul As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Asiento.
            If DtAsientoCabezaAux.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionNota)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Function
                End If
                DtAsientoCabezaAux.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabezaAux.Rows(0).Item("Documento") = DtNotaCabezaAux.Rows(0).Item("Nota")
                For Each Row As DataRow In DtAsientoDetalleAux.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            Resul = ActualizaNota("M", DtGrid, DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtNotaLotesAux)

            If Resul >= 0 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function ActualizaNota(ByVal Funcion As String, ByVal DtGridAux As DataTable, ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtSaldosInicialesAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable, ByVal DtNotaLotesAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Funcion = "A" And PTipoNota = 600 Then
                    For Each Row As DataRow In DtGridAux.Rows
                        If Row.RowState <> DataRowState.Deleted Then
                            If HallaTipo(Row("MedioPago")) = 4 Then
                                If Not ReGrabaUltimaNumeracionRetenciones(Row("MedioPago"), Row("Comprobante")) Then
                                    MsgBox("Error, Base de datos al Regrabar ultimo numero en Tablas de Retenciones. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                                    Return -3
                                End If
                            End If
                        End If
                    Next
                End If
                '
                Resul = ActualizaRecibo(DtFormasPago, Funcion, DtGridAux, DtNotaCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, ConexionNota, PEsTrOPagoEspecial)
                If Resul <= 0 Then Return Resul
                '
                If Not IsNothing(DtFacturasCabezaAux.GetChanges) Then
                    If Not ActualizaSaldoFacturas(PTipoNota, DtFacturasCabezaAux.GetChanges, ConexionNota) Then
                        Return 0
                    End If
                End If
                '
                If Not IsNothing(DtRetencionProvinciaWW.GetChanges) Then
                    Resul = GrabaTabla(DtRetencionProvinciaWW.GetChanges, "RecibosRetenciones", ConexionNota)
                    If Resul <= 0 Then Return 0
                End If
                '
                If Not IsNothing(DtNVLPCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNVLPCabezaAux.GetChanges, "NVLPCabeza", ConexionNota)
                    If Resul <= 0 Then Return 0
                End If
                '
                If Not IsNothing(DtComprobantesCabezaAux.GetChanges) Then
                    If Not ActualizaSaldoNotas(DtComprobantesCabezaAux.GetChanges, ConexionNota) Then
                        Return 0
                    End If
                End If
                '
                If Not IsNothing(DtLiquidacionCabezaAux.GetChanges) Then
                    If Not ActualizaSaldoLiquidacion(DtLiquidacionCabezaAux.GetChanges, ConexionNota) Then
                        Return 0
                    End If
                End If
                '
                If Not IsNothing(DtSaldosInicialesAux.GetChanges) Then
                    If Not ActualizaSaldosIniciales(DtSaldosInicialesAux.GetChanges, ConexionNota) Then
                        Return 0
                    End If
                End If
                '
                If Not IsNothing(DtNotaLotesAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaLotesAux.GetChanges, "RecibosLotes", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaAux.GetChanges, "AsientosCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleAux.GetChanges, "AsientosDetalle", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return DtNotaCabezaAux.Rows(0).Item("Nota")
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function UltimaNumeracionRetenciones(ByVal Retencion As Integer) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT UltimoNumero FROM Tablas WHERE Tipo = 25 AND Clave = " & Retencion & ";", Miconexion)
                    Return Cmd.ExecuteScalar() + 1
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function ReGrabaUltimaNumeracionRetenciones(ByVal Retencion As Integer, ByVal Numero As Integer) As Boolean

        Dim Sql As String = "UPDATE " & "Tablas" & _
                 " Set UltimoNumero = " & Numero & _
                 " WHERE Tipo = 25 AND Clave = " & Retencion & " AND UltimoNumero = " & Numero - 1 & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                If Resul > 0 Then Return True
            End Using
        Catch ex As Exception
            Return False
        End Try

    End Function
    Private Sub CalculaRetencion()

        If Not PAbierto Or PTipoNota <> 600 Or PNota <> 0 Or DtRetencionesAutomaticas.Rows.Count = 0 Or CheckRetencionManual.Checked Then Exit Sub
        If PEsEgresoCaja Then Exit Sub

        Dim Retencion As Decimal = AgregaRetencionesAlGrid()
        If Retencion = -100 Then Me.Close() : Exit Sub

        LabelSaldoSinRetencion.Text = "Efectivo sin Retención :" & FormatCurrency(CDbl(TextTotalOrden.Text) - Retencion, GDecimales)
        TextImporteRetenciones.Text = FormatNumber(Retencion, GDecimales)

        CalculaTotales()

    End Sub
    Private Function AgregaRetencionesAlGrid() As Decimal

        Dim Retencion As Decimal = 0
        Dim RetencionT As Decimal = 0
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtRetencionesAutomaticas.Rows
            Select Case Row("Formula")
                Case 1
                    Retencion = CalculaRetencionFormula1(Row("Nombre"), PEmisor, Month(DateTime1.Value), Year(DateTime1.Value), CDbl(TextTotalOrden.Text), ComboIva.SelectedValue, Row("Clave"), Row("TopeMes"), Row("AlicuotaRetencion"))
                Case 4
                    Retencion = CalculaRetencionFormula4(Row("Clave"), PEmisor, Cuit, DateTime1.Value, CDbl(TextTotalOrden.Text), ComboIva.SelectedValue)
                    MesRetencion4 = DateTime1.Value.Month : AnioRetencion4 = DateTime1.Value.Year
                    If Retencion = -100 Then
                        If MsgBox("No de puede realizar Calculo Automático de " & Row("Nombre") & ". Para la fecha " & DateTime1.Text & " No se puede encontrar Padrón en el Servidor." + vbCrLf + "Debe cargar al sistema el Padrón por la opcion: 'Incorpora Padrones Retenciones/Percepciones' en el Menu de Empresas." + vbCrLf + "Desea continuar de todas manera? (Y/N).", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                            Return Retencion
                        Else
                            Return 0
                        End If
                    End If
                Case Else
                    Retencion = 0
            End Select
            If Retencion > 0 Or Row("Formula") = 0 Then
                RowsBusqueda = DtGrid.Select("MedioPago = " & Row("Clave"))
                If RowsBusqueda.Length <> 0 Then
                    RowsBusqueda(0).Item("Importe") = Retencion
                Else
                    Dim Row1 As DataRow = DtGrid.NewRow
                    Row1("Item") = 0
                    Row1("MedioPago") = Row("Clave")
                    Row1("Detalle") = ""
                    Row1("Neto") = 0
                    Row1("Alicuota") = 0
                    Row1("ImporteIva") = 0
                    Row1("Banco") = 0
                    Row1("Fecha") = "1/1/1800"
                    Row1("Cuenta") = 0
                    Row1("Serie") = ""
                    Row1("Numero") = 0
                    Row1("EmisorCheque") = ""
                    Row1("Cambio") = 0
                    Row1("Importe") = Retencion
                    Row1("Comprobante") = 0
                    Row1("FechaComprobante") = "1/1/1800"
                    Row1("Bultos") = 0
                    Row1("ClaveCheque") = 0
                    Row1("ClaveInterna") = 0
                    Row1("ClaveChequeVisual") = 0
                    Row1("TieneLupa") = False
                    DtGrid.Rows.Add(Row1)
                End If
                RetencionT = RetencionT + Retencion
            Else
                'Borra retenciones = 0 que estan en el grid.
                For I As Integer = DtGrid.Rows.Count - 1 To 0 Step -1
                    RowsBusqueda = DtGrid.Select("MedioPago = " & Row("Clave"))
                    If RowsBusqueda.Length <> 0 Then RowsBusqueda(0).Delete()
                Next
            End If
        Next

        Return RetencionT

    End Function
    Private Function HallaIva(ByVal Codigo As Integer) As Double

        If GTipoIva = 2 Then
            Return 0
        End If

        For Each Row As DataRow In ComboIva.DataSource.rows
            If Row("Clave") = Codigo Then Return Row("Iva")
        Next

    End Function
    Private Sub ArmaListaDeRetencionesATerceros()

        DtRetencionesAutomaticas = New DataTable

        If PTipoNota <> 600 Or Not PAbierto Then Exit Sub

        ArmaRetencionesATercerosAplicables(PTipoNota, 1, PEmisor, DtRetencionesAutomaticas, DateTime1.Value, PNota)

        For I As Integer = DtFormasPago.Rows.Count - 1 To 0 Step -1
            Dim Row As DataRow = DtFormasPago.Rows(I)
            If Row("Tipo") = 4 Then           'Borrar percepciones/retenciones de medios de pago que no esten en DtRetencionesAutomaticas.
                Dim RowsBusqueda() As DataRow
                RowsBusqueda = DtRetencionesAutomaticas.Select("Clave = " & Row("Clave"))
                If RowsBusqueda.Length = 0 Then Row.Delete()
            End If
        Next

    End Sub
    Private Sub PideDatosEmisor()

        If PEmisor = 0 Then   'Dejar. Es para opcion nueva factura igual proveedor. 
            If PTipoNota = 600 Or PTipoNota = 604 Then OpcionEmisor.PEsProveedor = True
            If PTipoNota = 60 Or PTipoNota = 64 Or PTipoNota = 65 Then OpcionEmisor.PEsCliente = True
            OpcionEmisor.PTipoNota = PTipoNota
            OpcionEmisor.PEsTr = PEsTr
            OpcionEmisor.PEsEgresoCaja = PEsEgresoCaja
            OpcionEmisor.PEsPorCuentayOrden = PEsPorCuentaYOrden
            OpcionEmisor.PEsPorDiferenciaCambio = PDiferenciaDeCambio
            Select Case PTipoNota
                Case 600, 64
                    OpcionEmisor.PEsSoloAltas = True
            End Select
            OpcionEmisor.ShowDialog()
            PEmisor = OpcionEmisor.PEmisor
            PACuenta = OpcionEmisor.PACuenta
            PAbierto = OpcionEmisor.PAbierto
            OpcionEmisor.Dispose()
            If PEmisor = 0 Then Exit Sub
        End If

        If PTipoNota = 60 Or PTipoNota = 65 Or PTipoNota = 64 Then
            LetraIva = HallaTipoIvaCliente(PEmisor)
        End If

        If PTipoNota = 600 Or PTipoNota = 604 Then
            LetraIva = HallaTipoIvaProveedor(PEmisor)
        End If

        If PEsPorCuentaYOrden Then
            If LetraIva = Exterior Then
                MsgBox("Proveedor Por Cuenta y Orden No habilitado para este Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PEmisor = 0
            End If
            Dim TipoIva As Integer = HallaTipoIvaProveedor(PACuenta)
            If TipoIva = Exterior Then
                MsgBox("Proveedor Por Cuenta y Orden No habilitado para este Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PEmisor = 0
            End If
        End If

        If PTipoNota = 65 Then
            If LetraIva = Exterior Then
                MsgBox("Cliente o Proveedor No habilitado para Este Recibo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PEmisor = 0
            End If
        End If

        If PEsTr Then
            If LetraIva = Exterior Then
                MsgBox("Cliente o Proveedor No Habilitado para Este Recibo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PEmisor = 0
            End If
        End If

        TextLetra.Text = LetraTipoIva(LetraIva)

        GPuntoDeVenta = HallaPuntoVentaSegunTipo(PTipoNota, LetraIva)
        If GPuntoDeVenta = 0 Then
            MsgBox("No Tiene Definido Punto de Venta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            PEmisor = 0
        End If
        If Not ExistePuntoDeVenta(GPuntoDeVenta) Then
            MsgBox("Punto de Venta " & GPuntoDeVenta & " No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            PEmisor = 0
        End If
        If EsPuntoDeVentaZ(GPuntoDeVenta) Then
            MsgBox("Punto de Venta del Operador " & GPuntoDeVenta & " Reservado Para Ticket Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            PEmisor = 0
        End If

        LabelPuntoDeVenta.Text = "Punto de Venta " & Format(GPuntoDeVenta, "0000")


    End Sub
    Private Sub ActualizaComprobantes(ByVal Funcion As String, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtSaldosInicialesAux As DataTable)

        Dim RowsBusqueda() As DataRow

        'Actualiza Saldo de Comprobantes Imputados.

        If Funcion = "M" Then
            For Each Row1 As DataRow In DtGridCompro.Rows
                If Row1.RowState <> DataRowState.Deleted Then
                    If Row1("Saldo") <> Row1("SaldoAnt") Then
                        Select Case Row1("Tipo")
                            Case 2
                                RowsBusqueda = DtFacturasCabezaAux.Select("Factura = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                            Case 10
                                RowsBusqueda = DtLiquidacionCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                            Case 30
                                RowsBusqueda = DtSenaAux.Select("Comprobante = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                            Case 800
                                RowsBusqueda = DtNVLPCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                            Case 900
                                RowsBusqueda = DtSaldosInicialesAux.Select("Clave = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                            Case Else
                                RowsBusqueda = DtComprobantesCabezaAux.Select("TipoNota = " & Row1("Tipo") & " AND Nota = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                        End Select
                    End If
                End If
            Next
        End If

        If Funcion = "B" Then
            Dim Importe As Double = 0
            For Each Row1 As DataRow In DtGridCompro.Rows
                If Row1.RowState <> DataRowState.Deleted Then
                    If Row1.Item("Asignado") <> 0 Then
                        Select Case Row1("Tipo")
                            Case 2
                                RowsBusqueda = DtFacturasCabezaAux.Select("Factura = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                            Case 10
                                RowsBusqueda = DtLiquidacionCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                            Case 30
                                RowsBusqueda = DtSenaAux.Select("Comprobante = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                            Case 800
                                RowsBusqueda = DtNVLPCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                            Case 900
                                RowsBusqueda = DtSaldosInicialesAux.Select("Clave = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo") + Row1.Item("Asignado"))
                            Case Else
                                RowsBusqueda = DtComprobantesCabezaAux.Select("TipoNota = " & Row1("Tipo") & " AND Nota = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                        End Select
                    End If
                End If
            Next
        End If

    End Sub
    Private Function HallaTipo(ByVal MedioPago As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtFormasPago.Select("Clave = " & MedioPago)
        Return RowsBusqueda(0).Item("Tipo")

    End Function
    Private Function HallaDatosCheque(ByVal MedioPago As Integer, ByVal ClaveCheque As Integer, ByRef Serie As String, ByRef Numero As Integer, ByRef EmisorCheque As String, ByRef Fecha As Date, ByRef eCheq As Boolean) As Boolean

        Serie = ""
        Numero = 0
        EmisorCheque = ""
        Fecha = "1/1/1800"

        Dim ConexionStr As String

        If PEsTr And Not PermisoTotal Then Return True

        Dim DtCheques As New DataTable

        If PEsTr Then
            If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & MedioPago & " AND ClaveCheque = " & ClaveCheque & ";", ConexionN, DtCheques) Then Return False
        Else
            If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & MedioPago & " AND ClaveCheque = " & ClaveCheque & ";", ConexionNota, DtCheques) Then Return False
        End If

        If DtCheques.Rows.Count <> 0 Then
            Serie = DtCheques.Rows(0).Item("Serie")
            Numero = DtCheques.Rows(0).Item("Numero")
            EmisorCheque = DtCheques.Rows(0).Item("EmisorCheque")
            Fecha = DtCheques.Rows(0).Item("Fecha")
            eCheq = DtCheques.Rows(0).Item("eCheq")
        End If

        DtCheques.Dispose()
        Return True

    End Function
    Private Function UltimaFechaPuntoVentaTipoNota(ByVal ConexionStr) As Date

        Dim Patron As String = GPuntoDeVenta & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM RecibosCabeza WHERE CAST(CAST(RecibosCabeza.Nota AS numeric) as char)LIKE '" & Patron & "' AND TipoNota = " & PTipoNota & ";", Miconexion)
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
    Private Function UltimaFechaDeUnaContable(ByVal ConexionStr) As Date

        Dim Sql As String

        If PNota = 0 Then
            Sql = "SELECT MAX(Fecha) FROM Reciboscabeza WHERE Tr = 1;"
        Else
            Sql = "SELECT MAX(Fecha) FROM Reciboscabeza WHERE Tr = 1 AND Nota <> " & PNota & ";"
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
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
    Private Function EsReciboManual(ByVal PuntoVenta As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT EsReciboManual FROM PuntosDeVenta WHERE Clave = " & PuntoVenta & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Punto de Venta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Private Function EsNumeroReciboOficialOK() As Boolean

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT ReciboOficial FROM RecibosCabeza WHERE Estado = 1 AND TipoNota = 60 AND ReciboOficial = " & Val(MaskedReciboOficial.Text) & ";", Conexion, Dt) Then
            MsgBox("Error Base de Datos al Leer Tabla de Recibos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End If
        If Dt.Rows.Count <> 0 Then
            MsgBox("Recibo Oficial Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Dt.Dispose()
            Return False
        End If

        Dt.Dispose()
        Return True

    End Function
    Private Function ReciboExiste(ByVal Recibo As Double) As Boolean

        Dim Miconexion As New OleDb.OleDbConnection(ConexionNota)

        Try
            Miconexion.Open()
            Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Nota) FROM Reciboscabeza WHERE Nota = " & Recibo & ";", Miconexion)
                If Cmd.ExecuteScalar() <> 0 Then Return True
            End Using
        Catch ex As Exception
            Return False
        Finally
            If Miconexion.State = ConnectionState.Open Then Miconexion.Close()
        End Try

    End Function
    Private Sub CalculaTotales()                                  'CalculaTotales

        TotalConceptos = 0
        If ComboTipoIva.SelectedValue <> Exterior Then
            For Each Row As DataRow In DtGrid.Rows
                If Row("Cambio") = 0 Then
                    TotalConceptos = TotalConceptos + Row("Importe")
                Else : TotalConceptos = Trunca(TotalConceptos + Trunca(Row("Cambio") * Row("Importe")))
                End If
            Next
        Else
            For Each Row As DataRow In DtGrid.Rows
                TotalConceptos = Trunca(TotalConceptos + Row("Importe"))
            Next
        End If

        If DtNotaCabeza.Rows(0).Item("Importe") = 0 And PNota <> 0 Then
            TotalConceptos = 0
        End If

        TextTotalRecibo.Text = FormatNumber(TotalConceptos, GDecimales)

        TotalFacturas = 0
        For Each Row As DataRow In DtGridCompro.Rows
            If Row.RowState <> DataRowState.Deleted Then
                TotalFacturas = Trunca(TotalFacturas + Row("Asignado"))
            End If
        Next

        TextTotalFacturas.Text = FormatNumber(TotalFacturas, GDecimales)

        Dim RowsBusqueda() As DataRow

        Dim Retenciones As Decimal
        For Each RowQ As DataRow In DtGrid.Rows   ' Suma retenciones.
            RowsBusqueda = DtFormasPago.Select("Clave = " & RowQ("MedioPago"))
            If RowsBusqueda(0).Item("Tipo") = 4 Then Retenciones = Retenciones + RowQ("Importe")
        Next
        TextImporteRetenciones.Text = FormatNumber(Retenciones, GDecimales)

        Select Case PTipoNota    'Esto es para que no modifique el saldo cuando se regraba la nota.(Saldo esta enlazado con Registro cabeza.
            Case 60, 600
                TextSaldo.Text = FormatNumber(CDec(TextTotalRecibo.Text) - ImputacionDeOtros - TotalFacturas, GDecimales)
            Case Else
                If PNota = 0 Then
                    TextSaldo.Text = FormatNumber(CDec(TextTotalRecibo.Text), GDecimales)
                End If
        End Select

    End Sub
    Private Sub Print_PrintOrdenPago(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 37
        Dim UltimaLinea As Integer
        Dim LineasImpresas As Integer
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        Dim Imputaciones As Integer = CantidadImputaciones()

        Try
            Print_TituloOrdenPago(e, MIzq, MTop)

            If LineasParaImpresion < DtGrid.Rows.Count Then
                Print_MediosPago(e, MIzq, MTop, UltimaLinea, LineasImpresas)
                'Imprime imputaciones.
                If Imputaciones > 0 And LineasParaImpresion >= DtGrid.Rows.Count And LineasImpresas < 27 Then
                    Print_Imputaciones(e, MIzq, UltimaLinea + 10, 37 - LineasImpresas - 4)
                End If
            Else
                If Imputaciones > 0 Then
                    Print_Imputaciones(e, MIzq, MTop + 50, 37)
                End If
            End If

            Print_FinalOrdenPago(e, MIzq, MTop)

            If (LineasParaImpresion < DtGrid.Rows.Count) Or (Imputaciones > 0 And LineasParaImpresionImputacion < GridCompro.Rows.Count) Then
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
    Private Sub Print_PrintCobranza(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 37
        Dim UltimaLinea As Integer
        Dim LineasImpresas As Integer
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        Dim Imputaciones As Integer = CantidadImputaciones()

        Try
            Print_TituloCobranza(e, MIzq, MTop)

            If LineasParaImpresion < DtGrid.Rows.Count Then
                Print_MediosPago(e, MIzq, MTop, UltimaLinea, LineasImpresas)
                'Imprime imputaciones.
                If Imputaciones > 0 And LineasParaImpresion >= DtGrid.Rows.Count And LineasImpresas < 27 Then
                    Print_Imputaciones(e, MIzq, UltimaLinea + 10, 37 - LineasImpresas - 4)
                End If
            Else
                If Imputaciones > 0 Then
                    Print_Imputaciones(e, MIzq, MTop + 50, 37)
                End If
            End If

            ''''''''''''       Print_FinalOrdenPago(e, MIzq, MTop)

            If (LineasParaImpresion < DtGrid.Rows.Count) Or (Imputaciones > 0 And LineasParaImpresionImputacion < GridCompro.Rows.Count) Then
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
    Private Function CantidadImputaciones() As Integer

        Dim Contador As Integer = 0

        For Each Row As DataGridViewRow In GridCompro.Rows
            If Row.Cells("Asignado").Value <> 0 Then Contador = Contador + 1
        Next

        Return Contador

    End Function
    Private Sub Print_TituloOrdenPago(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

        Dim Texto As String = ""

        'Encabezado.
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
        Texto = GNombreEmpresa
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
        Texto = "ORDEN DE PAGO"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 140, MTop)
        If PTipoNota = 64 Then
            Texto = "(Devolución) "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 140, MTop + 4)
        End If
        ' 
        PrintFont = New Font("Courier New", 12)
        Texto = "Nro. Orden Pago:  " & NumeroEditado(MaskedNota.Text)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
        Texto = "Fecha:  " & DateTime1.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
        Texto = "Proveedor: " & ComboEmisor.Text & "  Cuit.: " & Cuit
        If PTipoNota = 64 Then
            Texto = "Cliente  : " & ComboEmisor.Text & "  Cuit.: " & Cuit
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 27)
        If PTipoNota = 64 Then
            Texto = "Alias    : " & HallaAliasCliente()
        Else
            Texto = "Alias    : " & HallaAliasProveedor() & "     " & TextImpresionOrdenPago.Text
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 33)
        Texto = "Importe Orden de Pago : " & TextTotalRecibo.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 60, MTop + 40)

    End Sub
    Private Sub Print_TituloCobranza(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

        Dim Texto As String = ""

        'Encabezado.
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
        Texto = GNombreEmpresa
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
        Texto = "RECIBO DE COBRANZA"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 125, MTop)
        If PTipoNota = 604 Then
            Texto = "(Devolución) "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 125, MTop + 4)
        End If
        ' 
        PrintFont = New Font("Courier New", 12)
        Texto = "Nro. Cobranza:  " & NumeroEditado(MaskedNota.Text)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
        Texto = "Fecha:  " & DateTime1.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
        Texto = "Cliente: " & ComboEmisor.Text & "  Cuit.: " & Cuit
        If PTipoNota = 604 Then
            Texto = "Proveedor  : " & ComboEmisor.Text & "  Cuit.: " & Cuit
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 27)
        If PTipoNota = 604 Then
            Texto = "Alias    : " & HallaAliasProveedor()
        Else
            Texto = "Alias    : " & HallaAliasCliente()
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 33)
        Texto = "Importe Cobranza: " & TextTotalRecibo.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 60, MTop + 40)

    End Sub
    Private Sub Print_FinalOrdenPago(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

        Dim Texto As String = ""
        Dim Yq As Integer
        Dim SaltoLinea As Integer = 4
        Dim PrintFont As System.Drawing.Font

        Yq = MTop + 215
        PrintFont = New Font("Courier New", 10)
        Texto = "RECIBI LOS VALORES DESCRIPTOS PRECEDENTEMENTE  ........................... "
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
        Yq = Yq + SaltoLinea
        Texto = "                                                       F I R M A           "
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
        Yq = Yq + 2 * SaltoLinea
        Texto = "ACLARACION APELLIDO Y NOMBRE :............................................."
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
        Yq = Yq + 2 * SaltoLinea
        Texto = "DOCUMENTO  TIPO :...........  Nro.:........................................"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
        Yq = Yq + 3 * SaltoLinea
        Texto = "A U T O R I Z O : ............................."
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)

    End Sub
    Private Sub Print_MediosPago(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer, ByRef UltimaLinea As Integer, ByRef LineasImpresas As Integer)

        Dim Texto As String = ""
        Dim SaltoLinea As Integer = 4
        Dim Longi As Integer
        Dim Xq As Integer
        Dim Yq As Integer
        Dim x As Integer = MIzq
        Dim y As Integer = MTop + 50
        Dim Ancho As Integer = 180
        Dim RowsBusqueda() As DataRow
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 37

        'Grafica -Rectangulo medios de pagos. ----------------------------------------------------------------------

        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 10)
        Dim LineaDescripcion As Integer = x + 40
        Dim LineaCambio As Integer = x + 55
        Dim LineaImporte As Integer = x + 85
        Dim LineaBanco As Integer = x + 125
        Dim LineaNumero As Integer = x + 154
        Dim LineaVencimiento As Integer = x + Ancho
        'Titulos de descripcion.
        Texto = "DESCRIPCION DEL PAGO"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, y - 4)
        Texto = "DESCRIPCION"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 2, y + 2)
        Texto = "CAMBIO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaCambio - Longi
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "IMPORTE"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaImporte - Longi - 10
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "BANCO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaBanco - Longi - 20
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "NUMERO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaNumero - Longi - 10
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "VTO."
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaVencimiento - Longi - 10
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        'Descripcion del pago.
        Yq = y + SaltoLinea
        While Contador < LineasPorPagina And LineasParaImpresion < DtGrid.Rows.Count
            Dim Row As DataRow = DtGrid.Rows(LineasParaImpresion)
            RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
            Yq = Yq + SaltoLinea
            'Imprime Detalle.
            Texto = RowsBusqueda(0).Item("Nombre")
            Xq = x
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            If Row("Cambio") <> 0 Then
                'Imprime Cambio.
                Texto = FormatNumber(Row("Cambio"), GDecimales)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaCambio - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Importe.
                Texto = FormatNumber(Trunca(Row("Cambio") * Row("Importe")), GDecimales)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Else
                'Imprime Importe.
                Texto = FormatNumber(Row("Importe"), GDecimales)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            End If
            'Imprime Banco.
            Texto = NombreBanco(Row("Banco"))
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte + 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            'Imprime Numero.
            If Row("Numero") <> 0 Then
                Texto = FormatNumber(Row("Numero"), 0)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaNumero - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Vencimeinto.
                Texto = Format(Row("Fecha"), "dd/MM/yyyy")
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaVencimiento - Longi - 2
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            End If
            'Imprime Comprobante.
            If Row("Comprobante") <> 0 Then
                Texto = FormatNumber(Row("Comprobante"), 0)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaNumero - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime FechaComprobante.
                If Format(Row("FechaComprobante"), "dd/MM/yyyy") <> "01/01/1800" Then
                    Texto = Format(Row("FechaComprobante"), "dd/MM/yyyy")
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaVencimiento - Longi - 2
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
            End If
            Contador = Contador + 1
            LineasParaImpresion = LineasParaImpresion + 1
        End While

        Yq = Yq + SaltoLinea

        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Yq - y)
        'Lineas vertical.
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaDescripcion, y, LineaDescripcion, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCambio, y, LineaCambio, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte, y, LineaImporte, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaBanco, y, LineaBanco, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaNumero, y, LineaNumero, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y + 2 * SaltoLinea, x + Ancho, y + 2 * SaltoLinea)

        UltimaLinea = Yq
        LineasImpresas = Contador

    End Sub
    Private Sub Print_Imputaciones(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer, ByVal LineasPorPagina As Integer)

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim Contador As Integer = 0
        Dim PrintFont As System.Drawing.Font

        Dim Longi As Integer
        Dim Xq As Integer
        Dim Yq As Integer
        Dim Ancho As Integer

        'Grafica -Rectangulo Imputacion. ----------------------------------------------------------------------
        y = MTop
        x = MIzq
        Ancho = 183
        PrintFont = New Font("Courier New", 10)

        Dim LineaTipo As Integer = x + 35
        Dim LineaComprobante As Integer = x + 69
        Dim LineaFecha As Integer = x + 94
        Dim LineaImporte1 As Integer = x + 125
        Dim LineaSaldo As Integer = x + 155
        Dim LineaImporte2 As Integer = x + Ancho
        'Titulos de descripcion.
        Texto = "ASIGNACIONES"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, y - 4)
        Texto = "TIPO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 2, y + 2)
        Texto = "COMPROBANTE"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaComprobante - Longi - 6
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "FECHA"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaFecha - Longi - 5
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "IMPORTE"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaImporte1 - Longi - 10
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "SALDO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaSaldo - Longi - 5
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "IMPUTADO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaImporte2 - Longi - 5
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)

        'Descripcion Imputacion.
        Yq = y + SaltoLinea
        While Contador < LineasPorPagina And LineasParaImpresionImputacion < GridCompro.Rows.Count
            Dim Row As DataGridViewRow = GridCompro.Rows(LineasParaImpresionImputacion)
            If Row.Cells("Asignado").Value <> 0 Then
                Yq = Yq + SaltoLinea
                'Imprime Tipo.
                Texto = Row.Cells("Tipo").FormattedValue
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Comprobante.
                Texto = Row.Cells("Recibo").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaComprobante - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Fecha.
                Texto = Row.Cells("FechaCompro").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaFecha - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Importe.
                Texto = Row.Cells("ImporteCompro").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte1 - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime saldo.
                Texto = Row.Cells("Saldo").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaSaldo - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Asignado.
                Texto = Row.Cells("Asignado").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte2 - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Contador = Contador + 1
            End If
            LineasParaImpresionImputacion = LineasParaImpresionImputacion + 1
        End While

        Yq = Yq + SaltoLinea

        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Yq - y)
        'Lineas vertical.
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaTipo, y, LineaTipo, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaComprobante, y, LineaComprobante, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaFecha, y, LineaFecha, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte1, y, LineaImporte1, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaSaldo, y, LineaSaldo, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte2, y, LineaImporte2, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y + 2 * SaltoLinea, x + Ancho, y + 2 * SaltoLinea)

    End Sub
    Private Sub TotalesImprisionRetencionGanancia(ByVal row1 As DataRow)

        Dim Sql As String = "SELECT Importe,CodigoIva FROM RecibosCabeza WHERE TipoNota = 600 AND Estado <> 3 AND Emisor = " & PEmisor & " AND MONTH(Fecha) = " & Month(DateTime1.Value) & " AND YEAR(Fecha) = " & Year(DateTime1.Value) & ";"

        Dim Dt As New DataTable
        Dim Row2 As DataRow

        PagoNetoMesRetencion = 0

        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        For Each Row2 In Dt.Rows
            Dim Coeficiente As Decimal = 1 + HallaIva(Row2("CodigoIva")) / 100
            PagoNetoMesRetencion = PagoNetoMesRetencion + Row2("Importe") / Coeficiente
        Next

        Sql = "SELECT P.Importe AS Importe FROM RecibosCabeza AS C INNER JOIN RecibosDetallePago AS P ON C.TipoNota = P.TipoNota AND C.Nota = P.Nota " & _
              "WHERE C.TipoNota = 600 AND C.Estado <> 3 AND C.Emisor = " & PEmisor & " AND MONTH(C.Fecha) = " & Month(DateTime1.Value) & " AND Year(C.Fecha) = " & Year(DateTime1.Value) & " AND P.MedioPago = " & row1("Clave") & ";"
        Dt = New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        RetencionPagadaMes = 0
        For Each Row2 In Dt.Rows
            RetencionPagadaMes = RetencionPagadaMes + Row2("Importe")
        Next
        Dt.Dispose()

    End Sub
    Private Sub Print_RetencionGanancia(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 60
        Dim RowsBusqueda() As DataRow
        Dim Longi As Integer
        Dim LineaImporte As Integer = MIzq + 150
        Dim Xq As Integer

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            '--------------------------------------------------- Logo -----------------------------------------------
            Dim NombreEmpres As String = GNombreEmpresa '"Patagonia Sunrise S.R.L."
            Dim CuitEmpresa As String = GCuitEmpresa    '"30-12345678-5"
            Dim TipoComprobante As Integer = 14          '14. CERTIFICADO
            Dim Direccion1 As String = GDireccion1      '"Nave S-2 Puesto 34 - Mercado Central de Buenos Aires"
            Dim Direccion2 As String = GDireccion2      '"Autopista Ricchieri y Boulongne Sur Mer"
            Dim Direccion3 As String = GDireccion3      '"C.P.(1771) Villa Celina Pcia. de Buenos Aires"
            Dim LetraComprobante As String = ""
            Dim IngBruto As String = GIngBruto     '"Conv.Mult. Nª 901-898989-6"
            Dim FechaInicio As String = GFechaInicio
            '
            MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, LetraComprobante, GCondicionIvaEmpresa, IngBruto, FechaInicio, "C:\XML Afip\", e, False, 14)
            Texto = FormatNumber(NumeroRetencion, 0)
            Dim PrintFont As System.Drawing.Font = New Font("Arial", 16)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, 25)
            Texto = DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, 35)
            ''''''''''''''''''''''''''''''''''''''''''''''''Fin Logo --------------------------------------------------
            'Encabezado.
            y = MTop
            x = MIzq
            y = y + SaltoLinea
            PrintFont = New Font("Courier New", 14, FontStyle.Bold)
            Texto = "CERTIFICADO DE RETENCION"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 50, y)
            '
            PrintFont = New Font("Courier New", 13, FontStyle.Bold)
            y = y + 5 * SaltoLinea
            Texto = "RETENCION : " & NombreRetencion
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = "FECHA : " & DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 120, y)
            y = y + SaltoLinea
            Texto = "NRO. CERTIFICADO : " & FormatNumber(NumeroRetencion, 0)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            y = y + 2 * SaltoLinea
            Texto = "AGENTE DE RETENCION : " & GNombreEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "CUIT.: " & GCuitEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + 2 * SaltoLinea
            Texto = "SUJETO DE RETENCION : " & ComboEmisor.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "CUIT.: " & Cuit
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            PrintFont = New Font("Courier New", 13)
            y = y + 4 * SaltoLinea
            Texto = "EN EL DIA DE LA FECHA HEMOS RETENIDO EL IMPORTE DE $ " & FormatNumber(ImporteRetencion, GDecimales)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "Son pesos " & Letras(ImporteRetencion.ToString)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            Dim Coeficiente As Decimal = 1 + HallaIva(ComboIva.SelectedValue) / 100
            Dim PagadoActual As Decimal = CDec(TextTotalRecibo.Text) / Coeficiente
            '
            y = y + 2 * SaltoLinea
            Texto = "PAGOS EFECTUADOS EN EL PRESENTE MES      "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(PagoNetoMesRetencion - PagadoActual, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "PAGOS ACTUAL                            "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(PagadoActual, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "---------------"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            '
            y = y + SaltoLinea
            Texto = "TOTAL PAGADO EN EL MES                  "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(PagoNetoMesRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "TOPE DE PAGOS SEGUN RESOLUCION          "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(TopeMesRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "---------------"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "DIFERENCIA A APLICAR PARA RETENCION     "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(PagoNetoMesRetencion - TopeMesRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + 2 * SaltoLinea
            Texto = "TOTAL A RETENER CON ALICUOTA DEL " & FormatNumber(AlicuotaRetencion, 2) & "%"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Dim Ret As Double = Trunca((PagoNetoMesRetencion - TopeMesRetencion) * AlicuotaRetencion / 100)
            Texto = FormatNumber(Ret, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "RETENCIONES EFECTUADAS EN ESTE MES        "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(RetencionPagadaMes - ImporteRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "RETENCIONES ACTUAL                     "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(ImporteRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + 8 * SaltoLinea
            Texto = "FIRMA ........................"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 80, y)

            Paginas = Paginas + 1

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
    Private Sub Print_RetencionFormula4(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 60   '20
        Dim RowsBusqueda() As DataRow
        Dim Longi As Integer
        Dim LineaImporte As Integer = MIzq + 150
        Dim Xq As Integer

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            '--------------------------------------------------- Logo -----------------------------------------------
            Dim NombreEmpres As String = GNombreEmpresa '"Patagonia Sunrise S.R.L."
            Dim CuitEmpresa As String = GCuitEmpresa    '"30-12345678-5"
            Dim TipoComprobante As Integer = 14          '14. CERTIFICADO
            Dim Direccion1 As String = GDireccion1      '"Nave S-2 Puesto 34 - Mercado Central de Buenos Aires"
            Dim Direccion2 As String = GDireccion2      '"Autopista Ricchieri y Boulongne Sur Mer"
            Dim Direccion3 As String = GDireccion3      '"C.P.(1771) Villa Celina Pcia. de Buenos Aires"
            Dim LetraComprobante As String = ""
            Dim IngBruto As String = GIngBruto     '"Conv.Mult. Nª 901-898989-6"
            Dim FechaInicio As String = GFechaInicio
            '
            MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, LetraComprobante, GCondicionIvaEmpresa, IngBruto, FechaInicio, "C:\XML Afip\", e, False, 14)
            Texto = FormatNumber(NumeroRetencion, 0)
            Dim PrintFont As System.Drawing.Font = New Font("Arial", 16)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, 25)
            Texto = DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, 35)
            ''''''''''''''''''''''''''''''''''''''''''''''''Fin Logo --------------------------------------------------
            'Encabezado.
            y = MTop
            x = MIzq
            y = y + SaltoLinea
            PrintFont = New Font("Courier New", 14, FontStyle.Bold)
            Texto = "CERTIFICADO DE RETENCION (ARBA)"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 50, y)
            '
            PrintFont = New Font("Courier New", 13, FontStyle.Bold)
            y = y + 5 * SaltoLinea
            Texto = "RETENCION : " & NombreRetencion
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = "FECHA : " & DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 120, y)
            y = y + SaltoLinea
            Texto = "NRO. CERTIFICADO : " & FormatNumber(NumeroRetencion, 0)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            y = y + 2 * SaltoLinea
            Texto = "AGENTE DE RETENCION : " & GNombreEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "CUIT.: " & GCuitEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + 2 * SaltoLinea
            Texto = "SUJETO DE RETENCION : " & ComboEmisor.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "CUIT.: " & Cuit
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            PrintFont = New Font("Courier New", 13)
            y = y + 4 * SaltoLinea
            Texto = "EN EL DIA DE LA FECHA HEMOS RETENIDO EL IMPORTE DE $ " & FormatNumber(ImporteRetencion, GDecimales)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "Son pesos " & Letras(ImporteRetencion.ToString)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            Dim Coeficiente As Decimal = 1 + HallaIva(ComboIva.SelectedValue) / 100
            Dim PagadoActual As Decimal = CDec(TextTotalRecibo.Text) / Coeficiente
            '
            y = y + 2 * SaltoLinea
            Texto = "PAGO ACTUAL                             "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(PagadoActual, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = ""
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            '
            y = y + SaltoLinea
            Texto = "TOTAL A RETENER CON ALICUOTA DEL " & FormatNumber(AlicuotaRetencion, 2) & "%"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(ImporteRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + 8 * SaltoLinea
            Texto = "FIRMA ........................"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 80, y)

            Paginas = Paginas + 1

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
    Private Sub Print_RetencionManual(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 60
        Dim Longi As Integer
        Dim LineaImporte As Integer = MIzq + 150
        Dim Xq As Integer

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            '--------------------------------------------------- Logo -----------------------------------------------
            Dim NombreEmpres As String = GNombreEmpresa '"Patagonia Sunrise S.R.L."
            Dim CuitEmpresa As String = GCuitEmpresa    '"30-12345678-5"
            Dim TipoComprobante As Integer = 14          '14. CERTIFICADO
            Dim Direccion1 As String = GDireccion1      '"Nave S-2 Puesto 34 - Mercado Central de Buenos Aires"
            Dim Direccion2 As String = GDireccion2      '"Autopista Ricchieri y Boulongne Sur Mer"
            Dim Direccion3 As String = GDireccion3      '"C.P.(1771) Villa Celina Pcia. de Buenos Aires"
            Dim LetraComprobante As String = ""
            Dim IngBruto As String = GIngBruto     '"Conv.Mult. Nª 901-898989-6"
            Dim FechaInicio As String = GFechaInicio
            '
            MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, LetraComprobante, GCondicionIvaEmpresa, IngBruto, FechaInicio, "C:\XML Afip\", e, False, 14)
            Texto = FormatNumber(NumeroRetencion, 0)
            Dim PrintFont As System.Drawing.Font = New Font("Arial", 16)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, 25)
            Texto = DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, 35)
            ''''''''''''''''''''''''''''''''''''''''''''''''Fin Logo --------------------------------------------------
            'Encabezado.
            y = MTop
            x = MIzq
            y = y + SaltoLinea
            PrintFont = New Font("Courier New", 14, FontStyle.Bold)
            Texto = "CERTIFICADO DE RETENCION"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 50, y)
            '
            PrintFont = New Font("Courier New", 13, FontStyle.Bold)
            y = y + 5 * SaltoLinea
            Texto = "RETENCION : " & NombreRetencion
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = "FECHA : " & DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 120, y)
            y = y + SaltoLinea
            Texto = "NRO. CERTIFICADO : " & FormatNumber(NumeroRetencion, 0)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            y = y + 2 * SaltoLinea
            Texto = "AGENTE DE RETENCION : " & GNombreEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "CUIT.: " & GCuitEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + 2 * SaltoLinea
            Texto = "SUJETO DE RETENCION : " & ComboEmisor.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "CUIT.: " & Cuit
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            PrintFont = New Font("Courier New", 13)
            y = y + 4 * SaltoLinea
            Texto = "EN EL DIA DE LA FECHA HEMOS RETENIDO EL IMPORTE DE $ " & FormatNumber(ImporteRetencion, GDecimales)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "Son pesos " & Letras(ImporteRetencion.ToString)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            Dim PagadoActual As Double = CDbl(TextTotalRecibo.Text) - CDbl(TextTotalRecibo.Text) * HallaIva(ComboIva.SelectedValue) / 100
            '
            y = y + 2 * SaltoLinea
            y = y + SaltoLinea
            Texto = "MONTO RETENIDO                "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(ImporteRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + 8 * SaltoLinea
            Texto = "FIRMA ........................"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 80, y)

            Paginas = Paginas + 1

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
    Private Sub ArmaListaImportesIva(ByRef Lista As List(Of ItemIva))

        Dim Esta As Boolean
        Dim Iva As Decimal = 0

        For Each Row As DataRow In DtGrid.Rows
            Esta = False
            Iva = Row("Alicuota")
            If Iva <> 0 Then
                For Each Fila As ItemIva In Lista
                    If Fila.Iva = Iva Then
                        Fila.Importe = Fila.Importe + Row("ImporteIva")
                        Esta = True
                    End If
                Next
                If Not Esta Then
                    Dim Fila As New ItemIva
                    Fila.Iva = Iva
                    Fila.Importe = Row("ImporteIva")
                    Lista.Add(Fila)
                End If
            End If
        Next

    End Sub
    Public Function HallaAliasProveedor() As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Alias FROM Proveedores WHERE Clave = " & PEmisor & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Proveedores.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaAliasCliente() As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Alias FROM Clientes WHERE Clave = " & PEmisor & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Clientes.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function GrabaImpreso(ByVal DtCabezaW As DataTable, ByVal ConexionAux As String) As Boolean

        Dim Sql As String = "UPDATE RecibosCabeza Set Impreso = 1 WHERE TipoNota = " & PTipoNota & " AND Nota = " & DtCabezaW.Rows(0).Item("Nota") & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionAux)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                If CInt(Cmd.ExecuteNonQuery()) = 0 Then
                    MsgBox("Error, Base de Datos al Grabar Nota de Credito. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos al Grabar Nota de Credito. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End Try

        Return True

    End Function
    Private Function EsRechazoCheque(ByVal TipoNota As Integer, ByVal Nota As Double, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ChequeRechazado FROM RecibosCabeza WHERE TipoNota = " & TipoNota & " AND Nota = " & Nota & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer RecibosCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function Valida() As Boolean

        If PTipoNota <> 65 Then
            If TextTotalOrden.Text = "" Then
                MsgBox("Falta Informar Importe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextTotalOrden.Focus()
                Return False
            End If
            If CDbl(TextTotalOrden.Text) = 0 Then
                MsgBox("Falta Informar Importe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextTotalOrden.Focus()
                Return False
            End If
        End If

        Dim PuntoVentaManual As Boolean
        Dim RowsBusqueda() As DataRow

        If PManual Then
            PuntoVentaManual = EsReciboManual(Val(Strings.Left(MaskedNota.Text, 4)))
        Else : PuntoVentaManual = EsReciboManual(GPuntoDeVenta)
        End If

        If PNota = 0 And PTipoNota = 60 Then
            If Not PManual And PuntoVentaManual Then
                MsgBox("Punto de Venta del Comprobante Solo Habilitado para Recibo Manual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedNota.Focus()
                Return False
            End If
        End If

        Select Case PTipoNota
            Case 50, 70, 500, 700
            Case Else
                '     If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 Then
                '         MsgBox("Fecha Menor a la Fecha del ultimo Recibo Grabado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                '         DateTime1.Focus()
                '         Return False
                '     End If
        End Select

        If DiferenciaDias(DateTime1.Value, Date.Now) < 0 Then
            MsgBox("Fecha Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If

        If PManual Then
            If Val(MaskedNota.Text) = 0 Then
                MsgBox("Falta Comprobante.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedNota.Focus()
                Return False
            End If
            If Not MaskedOK(MaskedNota) Then
                MsgBox("Numero Comprobante Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedNota.Focus()
                Return False
            End If
            If Not PuntoVentaManual Then
                MsgBox("Punto de Venta del Comprobante No Habilitado para Recibo Manual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedNota.Focus()
                Return False
            End If
            If PNota = 0 And ReciboExiste(Val(MaskedNota.Text)) Then
                MsgBox("Recibo Manual Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedNota.Focus()
                Return False
            End If
            If Not ConsisteFecha(TextFechaManual.Text) Then
                MsgBox("Fecha Manual Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaManual.Focus()
                Return False
            End If
            If DiferenciaDias(DateTime1.Value, TextFechaManual.Text) < -365 Then
                MsgBox("Fecha Manual Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                DateTime1.Focus()
                Return False
            End If
            If DiferenciaDias(DateTime1.Value, TextFechaManual.Text) > 0 Then
                MsgBox("Fecha Manual Mayor que la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                DateTime1.Focus()
                Return False
            End If
        End If

        If Panel5.Visible = True Then
            If Val(MaskedReciboOficial.Text) = 0 Then
                MsgBox("Falta informar Vale.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedReciboOficial.Focus()
                Return False
            End If
            If Not MaskedOK(MaskedReciboOficial) Then
                MsgBox("Numero Vale Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedReciboOficial.Focus()
                Return False
            End If
            If ReciboOficialAnt <> DtNotaCabeza.Rows(0).Item("ReciboOficial") Then
                If ExisteReciboOficial(PTipoNota, PNota, PEmisor, DtNotaCabeza.Rows(0).Item("ReciboOficial"), ConexionNota) Then
                    MsgBox("Vale Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    MaskedReciboOficial.Focus()
                    Return False
                End If
            End If
            If Not ConsisteFecha(TextFechaReciboOficial.Text) Then
                MsgBox("Fecha Vale Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaReciboOficial.Focus()
                Return False
            End If
            If DiferenciaDias(DateTime1.Value, TextFechaReciboOficial.Text) < -365 Then
                MsgBox("Fecha Vale Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaReciboOficial.Focus()
                Return False
            End If
            If DiferenciaDias(DateTime1.Value, TextFechaReciboOficial.Text) > 0 Then
                MsgBox("Fecha Vale Mayor que la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaReciboOficial.Focus()
                Return False
            End If
        End If

        If PanelFechaContable.Visible = True Then
            If Not ConsisteFecha(TextFechaContable.Text) Then
                MsgBox("Fecha Contable Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
            If DiferenciaDias(DateTime1.Value, TextFechaContable.Text) < -365 Then
                MsgBox("Fecha Contable Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
            If DiferenciaDias(DateTime1.Value, TextFechaContable.Text) > 0 Then
                MsgBox("Fecha Contable Mayor que la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
        End If

        If PTipoNota = 600 And PEsTr Then
            Dim FechaW As Date
            FechaW = UltimaFechaDeUnaContable(Conexion)
            If FechaW = "2/1/1000" Then
                MsgBox("Error Base de Datos al leer Tabla: RecibosCabeza.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If DiferenciaDias(DateTime1.Value, FechaAnt) <> 0 Then
                If DiferenciaDias(DateTime1.Value, FechaW) > 0 Then
                    '    MsgBox("Fecha Menor a la Ultima Orden de Pago Grabada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    '    DateTime1.Focus()
                    '   Return False
                End If
            End If
        End If

        If Panel7.Visible Then
            If TextBultos.Text = "" Then
                TextBultos.Focus()
                MsgBox("Falta informar Bultos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If (PTipoNota = 600 Or PTipoNota = 64) And ComboTipoIva.SelectedValue <> Exterior Then
            If CDbl(TextTotalRecibo.Text) <> CDbl(TextTotalOrden.Text) Then
                MsgBox("Total Recibo no coincide con Importe de la Orden de Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If (PTipoNota = 60 Or PTipoNota = 604) And ComboTipoIva.SelectedValue <> Exterior Then
            If CDbl(TextTotalRecibo.Text) <> CDbl(TextTotalOrden.Text) Then
                MsgBox("Total Recibo no coincide con Importe de la Cobranza.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        For Each Row As DataRow In DtGrid.Rows
            If Row("Alicuota") <> 0 And (PDiferenciaDeCambio Or ComboTipoIva.SelectedValue = Exterior) Then
                MsgBox("IVA No corresponde para Exportación o Importación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        Next

        If TotalConceptos = 0 Then
            MsgBox("Debe informar Importes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TotalConceptos - TotalFacturas < 0 Then
            MsgBox("Importes Imputados supera importe de Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Dim ImporteRetencion As Decimal = 0
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("TieneLupa") Then
                ImporteRetencion = ImporteRetencion + Row1("Importe")
            End If
        Next
        If ImporteRetencion <> 0 Then
            If DtRetencionProvinciaAux.Rows.Count = 0 Then
                MsgBox("Falta Distribuir Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            Dim ImporteDistribuido As Decimal
            For Each Row As DataRow In DtRetencionProvinciaAux.Rows
                ImporteDistribuido = ImporteDistribuido + Row("Importe")
            Next
            If ImporteDistribuido <> ImporteRetencion Then
                MsgBox("Incorrecta Distribución Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        'CONSISTE Exterior.
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
            MsgBox("Error, Cambio de Moneda Local debe ser 1.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If
        If ComboMoneda.SelectedValue <> MonedaEmisor Then
            MsgBox("Moneda Incorrecta. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboMoneda.Focus()
            Return False
        End If
        If ComboTipoIva.SelectedValue = Exterior Then
            For Each Row1 As DataRow In DtGrid.Rows
                If Row1("MedioPago") <> 0 Then
                    RowsBusqueda = DtFormasPago.Select("Clave = " & Row1("MedioPago"))
                    'Actualizo cambio de moneda con el cambio declarado en recibo.
                    If (RowsBusqueda(0).Item("Tipo") = 1 Or RowsBusqueda(0).Item("Tipo") = 3) And RowsBusqueda(0).Item("Clave") <> ComboMoneda.SelectedValue Then
                        MsgBox("Moneda no se Corresponde con la que Opera el Cliente o Proveedor. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Return False
                    End If
                    If RowsBusqueda(0).Item("Tipo") = 3 Then Row1("Cambio") = CDbl(TextCambio.Text)
                End If
            Next
        End If

        For Each Row1 As DataRow In DtGridCompro.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                If Row1("Saldo") <> Row1("SaldoAnt") Then
                    If Row1("Moneda") <> ComboMoneda.SelectedValue Then
                        MsgBox("Se Imputo a Documento en Distinta Moneda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        GridCompro.Focus()
                        Return False
                    End If
                End If
            End If
        Next

        If ComboClienteOperacion.Enabled = True Then
            If ComboClienteOperacion.SelectedValue = 0 Then
                MsgBox("Falta Informar Cliente Operación.", MsgBoxStyle.Critical)
                ComboClienteOperacion.Focus()
                Return False
            End If
            If HallaMonedaCliente(ComboClienteOperacion.SelectedValue) <> ComboMoneda.SelectedValue Then
                MsgBox("Moneda Cliente Operación Difiere del Cliente de Facturación.", MsgBoxStyle.Critical)
                ComboClienteOperacion.Focus()
                Return False
            End If
        End If

        If ComboNegocio.SelectedValue <> 0 And ComboCosteo.SelectedValue = 0 Then
            MsgBox("Falta Informar Costeo del Negocio. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboCosteo.Focus()
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridCompro_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles GridCompro.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If GridCompro.Columns(e.ColumnIndex).Name = "Recibo" Then
            e.Value = NumeroEditado(e.Value)
            If PermisoTotal Then
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "FechaCompro" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Asignado" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "ImporteCompro" Or GridCompro.Columns(e.ColumnIndex).Name = "Saldo" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

    End Sub
    Private Sub GridCompro_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles GridCompro.DataError
        Exit Sub
    End Sub
    
End Class