Imports System.Transactions
Imports System.Drawing.Printing
Imports System.Math
Public Class UnReciboDiferencia
    Public PNota As Double
    Public PAbierto As Boolean
    Public PTipoNota As Integer
    Public PEmisor As Integer
    Public PBloqueaFunciones As Boolean
    Public PImporte As Double
    Public PPorCuentaYOrden As Boolean
    Public PEsTr As Boolean
    Public PManual As Boolean
    Public PDiferenciaDeCambio As Boolean
    Public PFacturaDiferencia As Double
    Public PImporteFactura As Decimal
    Public PImporteDiferencia As Decimal
    Public PIvaDiferencia As Decimal
    Public PDtComproFacturados As DataTable
    Public PCodigoFactura As Integer
    '
    Dim DtNotaDetalle As DataTable
    Dim DtNotaCabeza As DataTable
    Dim DtMedioPago As DataTable
    Dim DtNotaLotes As DataTable
    Dim DtComproFacturados As DataTable
    Dim DtComprobantesCabeza As DataTable
    Dim DtFacturasCabeza As DataTable
    Dim DtLiquidacionCabeza As DataTable
    Dim DtNVLPCabeza As DataTable
    Dim DtGridCompro As DataTable
    Dim DtGrid As DataTable
    Dim DtGridLotes As New DataTable
    Dim DtSena As DataTable
    Dim DtFormasPago As DataTable
    Dim DtRetencionProvincia As DataTable
    Dim DtRetencionProvinciaAux As DataTable
    '
    Private MiEnlazador As New BindingSource
    '
    Dim cb As ComboBox
    Dim ConexionNota As String
    Dim UltimoNumero As Double = 0
    Dim UltimafechaContableW As DateTime
    Dim UltimoNumeroRetencion As Integer = 0
    Dim ImputacionDeOtros As Double = 0
    Dim LetraIva As Integer
    Dim UltimaFechaW As DateTime
    Dim TablaIva(0) As Double
    Dim TipoAsiento As Integer
    Dim Calle As String
    Dim Localidad As String
    Dim Provincia As String
    Dim Cuit As String
    Dim EsFacturaElectronica As Boolean
    Dim FacturaAnteriorOK As Boolean
    Dim DocTipo As Integer
    Dim DocNro As Decimal
    'para impresion.
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim LineasParaImpresion As Integer = 0
    Dim LineasParaImpresionImputacion As Integer = 0
    Private Sub UnaNotaTerceros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 30

        GModificacionOk = False

        If GCaja = 0 And PNota = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        GridCompro.AutoGenerateColumns = False
        GridCompro.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        If PNota = 0 Then
            PideDatosEmisor()
            If PEmisor = 0 Then Me.Close() : Exit Sub
        End If

        If PAbierto Then
            ConexionNota = Conexion
        Else : ConexionNota = ConexionN
        End If

        If PTipoNota = 50 Or PTipoNota = 70 Then LlenaCombo(ComboEmisor, "", "Clientes") : Label16.Text = "Cliente"
        If PTipoNota = 5 Or PTipoNota = 7 Then LlenaCombo(ComboEmisor, "", "Clientes") : Label16.Text = "Cliente"
        If PTipoNota = 500 Or PTipoNota = 700 Or PTipoNota = 6 Or PTipoNota = 8 Then LlenaCombo(ComboEmisor, "", "Proveedores") : Label16.Text = "Proveedor"

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        ArmaTipoIva(ComboTipoIva)
        LlenaComboTablas(ComboPais, 28)

        ArmaTablaIva(TablaIva)

        ArmaMedioPagoOtras(PTipoNota, DtFormasPago, PAbierto, PNota)

        If PTipoNota = 5 Then LabelTipoNota.Text = "Nota Debito Financiera a Cliente"
        If PTipoNota = 6 Then LabelTipoNota.Text = "Nota Debito Financiera a Proveedor"
        If PTipoNota = 7 Then LabelTipoNota.Text = "Nota Credito Financiera a Cliente"
        If PTipoNota = 8 Then LabelTipoNota.Text = "Nota Credito Financiera a Proveedor"
        If PTipoNota = 50 Then LabelTipoNota.Text = "Nota Debito del Cliente" : LabelReciboOficial.Text = "Nota Debito"
        If PTipoNota = 70 Then LabelTipoNota.Text = "Nota Credito del Cliente" : LabelReciboOficial.Text = "Nota Credito"
        If PTipoNota = 500 Then LabelTipoNota.Text = "Nota Debito del Proveedor" : LabelReciboOficial.Text = "Nota Debito"
        If PTipoNota = 700 Then LabelTipoNota.Text = "Nota Credito del Proveedor" : LabelReciboOficial.Text = "Nota Credito"

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionNota = Conexion
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionNota = ConexionN
        End If

        LlenaCombosGrid()

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        If PTipoNota = 70 Or PTipoNota = 700 Or PTipoNota = 50 Or PTipoNota = 500 Then Panel5.Visible = True
        If PTipoNota = 70 Or PTipoNota = 700 Or PTipoNota = 50 Or PTipoNota = 500 Then PanelFechaContable.Visible = True

        If EsFacturaElectronica Then
            PanelFechaContable.Visible = True
            If PNota <> 0 Then PanelFechaContable.Enabled = False
        End If

        If Not (PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 6 Or PTipoNota = 8 Or PTipoNota = 50 Or PTipoNota = 500 Or PTipoNota = 70 Or PTipoNota = 700) Then
            TextLetra.Visible = False
        End If

        If PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 50 Or PTipoNota = 70 Then
            Me.BackColor = Color.LightGreen
        End If
        If PTipoNota = 6 Or PTipoNota = 8 Or PTipoNota = 500 Or PTipoNota = 700 Then
            Me.BackColor = Color.LightBlue
        End If

        If GGeneraAsiento Then
            Dim Conta = TieneTabla1(PCodigoFactura)
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

        Select Case PTipoNota
            Case 5, 7, 6, 8
            Case Else
                ButtonTextoRecibo.Visible = False
        End Select

        If EsFacturaElectronica Then
            UltimafechaContableW = UltimaFechacontableDebitoCredito(Conexion, GPuntoDeVenta, LetraIva, PTipoNota)
            If UltimafechaContableW = "2/1/1000" Then
                MsgBox("Error Base de Datos al Leer Ultima Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
            End If
        End If

        FacturaAnteriorOK = True
        If EsFacturaElectronica And PNota = 0 Then
            If Not EsFacturaAnteriorOk(PTipoNota, UltimoNumero) Then
                FacturaAnteriorOK = False
                MsgBox("La Nota " & NumeroEditado(UltimoNumero - 1) & " No Tiene Autorización AFIP. Si continua, Afip Rechazara este Comprobante.")
            End If
        End If

        Select Case PTipoNota
            Case 5, 6, 7, 8
                UltimaFechaW = UltimaFechaLetraPuntoVenta(Conexion)
            Case 60, 600, 65, 64, 604, 50, 70, 500, 700
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

        If PBloqueaFunciones Then
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

        Dim Fecha As Date
        If PanelFechaContable.Visible Then
            If TextFechaContable.Text = "" Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Debe Informar Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If Not ConsisteFecha(TextFechaContable.Text) Then
                MsgBox("Fecha Contable Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
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
            If EsFacturaElectronica Then
                If DiferenciaDias(TextFechaContable.Text, UltimafechaContableW) > 0 Then
                    MsgBox("Fecha Contable Menor a la Ultimo Recibo Informado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If DiferenciaDias(TextFechaContable.Text, DateTime1.Value) <> 0 Then
                    If DateTime1.Value.Day > 8 Then
                        MsgBox("Fecha Contable menor a fecha actual solo puede ser informada del 1 al 8 del mes actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
            End If
        End If

        GridCompro.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim RowsBusqueda() As DataRow

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtNotaDetalleAux = DtNotaDetalle.Copy
        Dim DtNotaLotesAux As DataTable = DtNotaLotes.Copy

        Dim DtFacturasCabezaAux As DataTable = DtFacturasCabeza.Copy
        Dim DtNVLPCabezaAux As DataTable = DtNVLPCabeza.Copy
        Dim DtLiquidacionCabezaAux As DataTable = DtLiquidacionCabeza.Copy
        Dim DtSenaAux As DataTable = DtSena.Copy
        Dim DtComprobantesCabezaAux As DataTable = DtComprobantesCabeza.Copy

        'Actualiza Saldo de Comprobantes Imputados.
        ActualizaComprobantes("M", DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux)

        'Actualiza Notas Cabeza.
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

        'Actualiza Notas Detalle.
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

        If IsNothing(DtNotaDetalleAux.GetChanges) And IsNothing(DtNotaCabezaAux.GetChanges) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GridCompro.Focus()
            Exit Sub
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

        'modifico los importeconiva y importesiniva de los lotes de la factura de proveedor.
        Dim DtComproFacturadosAux As DataTable = DtComproFacturados.Copy
        If PNota = 0 Then
            If Not ActualizaComproFacturados("A", DtComproFacturadosAux) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
        End If

        If PNota = 0 Then
            If HacerAlta(DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtComproFacturadosAux, DtNotaLotesAux) Then
                ArmaArchivos()
            End If
        Else
            If HacerModificacion(DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtComproFacturadosAux, DtNotaLotesAux) Then
                ArmaArchivos()
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If GCaja = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsFacturaElectronica Then
            MsgBox("Nota Electrónica No se Puede ANULAR. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If CInt(LabelCaja.Text) <> GCaja Then
            MsgBox("Modificación Solo Permitida para Caja " & LabelCaja.Text & ". Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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

        ActualizaComprobantes("M", DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux)

        If Not (IsNothing(DtFacturasCabezaAux.GetChanges) And IsNothing(DtNVLPCabezaAux.GetChanges) And IsNothing(DtLiquidacionCabezaAux.GetChanges) And IsNothing(DtSenaAux.GetChanges) And IsNothing(DtComprobantesCabezaAux.GetChanges)) Then
            MsgBox("Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Resul As Double

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtComproFacturadosAux As DataTable = DtComproFacturados.Copy
        If Not ActualizaComproFacturados("B", DtComproFacturadosAux) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
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

        ActualizaComprobantes("B", DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux)

        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        '
        If Not HallaAsientosCabeza(TipoAsiento, DtNotaCabeza.Rows(0).Item("Nota"), DtAsientoCabezaAux, ConexionNota) Then Exit Sub
        For Each Row As DataRow In DtAsientoCabezaAux.Rows
            Row("Estado") = 3
        Next

        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone

        DtNotaCabezaAux.Rows(0).Item("Estado") = 3

        Resul = ActualizaNota("B", DtGrid, DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtComproFacturadosAux, DtNotaLotesAux)
        If Resul = -1 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Baja Fue Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If GCaja = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue <> 3 And Not EsFacturaElectronica Then
            MsgBox("Nota Puede ser ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        'Ve si es la ultim Nota.
        Dim PuntoVentaW As Integer = Strings.Mid(DtNotaCabeza.Rows(0).Item("Nota"), 2, 4)
        Dim TipoIvaW As Integer = Strings.Mid(DtNotaCabeza.Rows(0).Item("Nota"), 1, 1)
        Dim UltimoNumeroW As Decimal = Strings.Right(DtNotaCabeza.Rows(0).Item("Nota"), 8)
        If HallaUltimaNumeracionDebitoCreditoW(PTipoNota, TipoIvaW, PuntoVentaW, ConexionNota) <> PNota Then
            MsgBox("Existe Notas Posteriores a Esta. Nota a Borrar Debe ser La Ultima Realizada. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsFacturaElectronica And DtNotaCabeza.Rows(0).Item("Cae") <> 0 Then
            MsgBox("Nota Electrónica No se Puede BORRAR pues Tiene Autorizacon en la AFIP. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If CInt(LabelCaja.Text) <> GCaja Then
            MsgBox("Modificación Solo Permitida para Caja " & LabelCaja.Text & ". Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtNotaCabeza.Rows(0).Item("Importe") <> DtNotaCabeza.Rows(0).Item("Saldo") Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Nota Tiene Imputaciones, Debe anularlas para Continuar. Operación se CANCELA.")
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

        ActualizaComprobantes("M", DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux)

        If Not (IsNothing(DtFacturasCabezaAux.GetChanges) And IsNothing(DtNVLPCabezaAux.GetChanges) And IsNothing(DtLiquidacionCabezaAux.GetChanges) And IsNothing(DtSenaAux.GetChanges) And IsNothing(DtComprobantesCabezaAux.GetChanges)) Then
            MsgBox("Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Resul As Double

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtComproFacturadosAux As DataTable = DtComproFacturados.Copy
        If Not ActualizaComproFacturados("B", DtComproFacturadosAux) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
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

        ActualizaComprobantes("B", DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux)

        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        '
        If Not HallaAsientosCabeza(TipoAsiento, DtNotaCabeza.Rows(0).Item("Nota"), DtAsientoCabezaAux, ConexionNota) Then Exit Sub
        If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaAux.Rows(0).Item("Asiento") & ";", ConexionNota, DtAsientoDetalleAux) Then Exit Sub
        For Each Row As DataRow In DtAsientoCabezaAux.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtAsientoDetalleAux.Rows
            Row.Delete()
        Next

        Dim ReciboOficial As Decimal = DtNotaCabezaAux.Rows(0).Item("ReciboOficial")

        DtNotaCabezaAux.Rows(0).Delete()
        For Each Row As DataRow In DtMedioPagoAux.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtNotaLotesAux.Rows
            Row.Delete()
        Next

        Dim DtPuntosDeVenta As New DataTable
        If (PTipoNota = 5 Or PTipoNota = 6 Or PTipoNota = 7 Or PTipoNota = 8) And PAbierto Then
            UltimoNumeroW = UltimoNumeroW - 1
            If Not Tablas.Read("SELECT * FROM PuntosDeVenta WHERE Clave = " & PuntoVentaW & ";", Conexion, DtPuntosDeVenta) Then Exit Sub
            Select Case TipoIvaW
                Case 1
                    Select Case PTipoNota
                        Case 5, 6
                            DtPuntosDeVenta.Rows(0).Item("NotasDebitoA") = UltimoNumeroW
                        Case 7, 8
                            DtPuntosDeVenta.Rows(0).Item("NotasCreditoA") = UltimoNumeroW
                    End Select
                Case 2
                    Select Case PTipoNota
                        Case 5, 6
                            DtPuntosDeVenta.Rows(0).Item("NotasDebitoB") = UltimoNumeroW
                        Case 7, 8
                            DtPuntosDeVenta.Rows(0).Item("NotasCreditoB") = UltimoNumeroW
                    End Select
                Case 3
                    Select Case PTipoNota
                        Case 5, 6
                            DtPuntosDeVenta.Rows(0).Item("NotasDebitoC") = UltimoNumeroW
                        Case 7, 8
                            DtPuntosDeVenta.Rows(0).Item("NotasCreditoC") = UltimoNumeroW
                    End Select
                Case 4
                    Select Case PTipoNota
                        Case 5, 6
                            DtPuntosDeVenta.Rows(0).Item("NotasDebitoE") = UltimoNumeroW
                        Case 7, 8
                            DtPuntosDeVenta.Rows(0).Item("NotasCreditoE") = UltimoNumeroW
                    End Select
                Case 5
                    Select Case PTipoNota
                        Case 5, 6
                            DtPuntosDeVenta.Rows(0).Item("NotasDebitoM") = UltimoNumeroW
                        Case 7, 8
                            DtPuntosDeVenta.Rows(0).Item("NotasCreditoM") = UltimoNumeroW
                    End Select
            End Select
        End If

        Resul = BorraNota(DtNotaCabezaAux, DtFacturasCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtNotaLotesAux, DtComproFacturadosAux, DtPuntosDeVenta, ReciboOficial)
        If Resul = -1 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Borrado Fue Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonPedirAutorizacionAfip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPedirAutorizacionAfip.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not PAbierto Then
            MsgBox("No Permitido en esta Nota", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If Not DtNotaCabeza.Rows(0).Item("EsElectronica") Then
            MsgBox("Comprobante No Es Electrónico.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If DtNotaCabeza.Rows(0).Item("CAE") <> 0 Then
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

        Dim mensaje As String = HallaCaeComprobante("CD", DtNotaCabeza.Rows(0).Item("TipoNota"), LetraIva, DtNotaCabeza.Rows(0).Item("Nota"), Cae, FechaCae, Resultado, ImpTotal, CbteTipoAsociado, CbteAsociado, ConceptoW, DtNotaCabeza.Rows(0).Item("EsFCE"))
        If mensaje = "" And Cae <> "" Then
            If Not GrabaCAE(DtNotaCabeza.Rows(0).Item("TipoNota"), DtNotaCabeza.Rows(0).Item("Nota"), CDec(Cae), CInt(FechaCae), CbteTipoAsociado, CbteAsociado) Then
                MsgBox("CAE de la AFIP No se pudo Grabar." + vbCrLf + "Intentelo Nuevamente.")
            Else
                If Not ArmaArchivos() Then Me.Close() : Exit Sub
            End If
            Exit Sub
        End If

        If PideAutorizacionAfip(DtNotaCabeza) Then   'Pide y graba CAE a la AFIP.----
            If Not ArmaArchivos() Then Me.Close() : Exit Sub
        End If

    End Sub
    Private Sub ButtonImportePorLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportePorLotes.Click

        SeleccionarVarios.PAbierto = PAbierto
        SeleccionarVarios.PTipoNota = PTipoNota
        SeleccionarVarios.PNota = PNota
        SeleccionarVarios.PEsImportePorLotesRecibos = True
        SeleccionarVarios.Show()

    End Sub
    Private Sub ButtonTextoRecibo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTextoRecibo.Click

        UnTextoParaRecibo.ShowDialog()

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Nota debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsFacturaElectronica And DtNotaCabeza.Rows(0).Item("Cae") = 0 Then
            MsgBox("Falta CAE.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PAbierto And Not (PTipoNota = 50 Or PTipoNota = 70 Or PTipoNota = 500 Or PTipoNota = 700) Then
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

        If PAbierto Then Copias = 2
        AddHandler print_document.PrintPage, AddressOf Print_PrintPageNota    'Notas credito y debitos financieras.
        print_document.Print()
        If ErrorImpresion Then Exit Sub

        If PAbierto Then
            If Not GrabaImpreso(DtNotaCabeza, Conexion) Then Exit Sub
            DtNotaCabeza.Rows(0).Item("Impreso") = True
        Else
            If Not GrabaImpreso(DtNotaCabeza, ConexionN) Then Exit Sub
            DtNotaCabeza.Rows(0).Item("Impreso") = True
        End If

        ArmaArchivos()

    End Sub
    Private Sub ButtonCompAsociado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCompAsociado.Click

        MsgBox(HallaCompAsociado(DtNotaCabeza.Rows(0).Item("TipoCompAsociado"), DtNotaCabeza.Rows(0).Item("CompAsociado")))

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PNota = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = TipoAsiento
        ListaAsientos.PTipoComprobante = PTipoNota
        If PAbierto Then
            ListaAsientos.PDocumentoB = PNota
        Else
            ListaAsientos.PDocumentoN = PNota
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub PictureLupaCuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupaCuenta.Click

        If PNota <> 0 Then Exit Sub

        If CDbl(TextTotalRecibo.Text) = 0 Then
            MsgBox("Falta Informar Conceptos.")
            Exit Sub
        End If

        Dim Neto As Double

        Select Case PTipoNota
            Case 6, 8, 500, 700, 5, 7, 50, 70
                For I As Integer = 0 To DtGrid.Rows.Count - 1
                    If DtGrid.Rows(I).Item("Mediopago") = 100 Then Neto = Trunca(Neto + DtGrid.Rows(I).Item("Neto"))
                Next
                If Neto = 0 Then Exit Sub
            Case Else
                MsgBox("Tipo Nota " & PTipoNota & " No Contemplada.")
                Exit Sub
        End Select

        SeleccionarCuentaDocumento.PListaDeCuentas = New List(Of ItemCuentasAsientos)

        Dim Item As New ListViewItem

        For I As Integer = 0 To ListCuentas.Items.Count - 1
            Dim Fila As New ItemCuentasAsientos
            Dim CuentaStr As String = ListCuentas.Items.Item(I).SubItems(0).Text
            Fila.Cuenta = Mid(CuentaStr, 1, 3) & Mid(CuentaStr, 5, 6) & Mid(CuentaStr, 12, 2)
            Fila.ImporteB = CDbl(ListCuentas.Items.Item(I).SubItems(1).Text)
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
    Private Sub TextBultos_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Function ArmaArchivos() As Boolean                        'ArmaArchivos

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

        If PNota = 0 Then
            If PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 6 Or PTipoNota = 8 Then
                If PAbierto Then
                    If PTipoNota = 5 Or PTipoNota = 6 Then UltimoNumero = UltimaNumeracionNDebito(LetraIva, GPuntoDeVenta)
                    If PTipoNota = 7 Or PTipoNota = 8 Then UltimoNumero = UltimaNumeracionNCredito(LetraIva, GPuntoDeVenta)
                    If UltimoNumero <= 0 Then
                        MsgBox("ERROR Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Return False
                    End If
                Else
                    UltimoNumero = CDbl(LetraIva & Format(GPuntoDeVenta, "0000") & "00000000")
                End If
            End If
        End If

        If PNota = 0 Then AgregaCabeza()

        MuestraCabeza()

        ComboEmisor.SelectedValue = DtNotaCabeza.Rows(0).Item("Emisor")
        EsFacturaElectronica = DtNotaCabeza.Rows(0).Item("EsElectronica")

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

        If PNota = 0 Then
            For Each Row1 In PDtComproFacturados.Rows
                Dim Row As DataRow = DtGridLotes.NewRow
                Row("Operacion") = Row1("Operacion")
                Row("Lote") = Row1("Lote")
                Row("Secuencia") = Row1("Secuencia")
                Row("Cantidad") = HallaCantidadLote(Row1("Lote"), Row1("Secuencia"), Row1("Operacion"))
                DtGridLotes.Rows.Add(Row)
            Next
        End If

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM RecibosDetallePago WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtMedioPago) Then Return False

        For Each Row As DataRow In DtMedioPago.Rows
            Row1 = DtGrid.NewRow
            Row1("Item") = Row("Item")
            Row1("MedioPago") = Row("MedioPago")
            Row1("Importe") = Row("Importe")
            Row1("Cambio") = Row("Cambio")
            Row1("Bultos") = 0
            Row1("Detalle") = Row("Detalle")
            Row1("Alicuota") = Row("Alicuota")
            Row1("Neto") = Row("Neto")
            Row1("ImporteIva") = Row("Neto") * Row("Alicuota") / 100
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            Row1("ClaveInterna") = Row("ClaveInterna")
            Row1("ClaveChequeVisual") = 0
            If Row("MedioPago") = 3 Then Row1("ClaveChequeVisual") = Row("ClaveCheque")
            Row1("Banco") = Row("Banco")
            Row1("Cuenta") = Row("Cuenta")
            Row1("Serie") = ""
            Row1("Numero") = 0
            Row1("EmisorCheque") = ""
            Row1("Fecha") = "1/1/1800"
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

        If PTipoNota = 7 And Not PDiferenciaDeCambio Then
            If Not ArmaConFacturas(PTipoNota) Then Return False
            If Not ArmaConNVLP() Then Return False
        End If
        If PTipoNota = 50 Then
            If Not ArmaConFacturas(PTipoNota) Then Return False
            If Not ArmaConNVLP() Then Return False
            If Not ArmaConNotas(5) Then Return False
        End If
        If PTipoNota = 6 And Not PDiferenciaDeCambio Then
            If Not ArmaConFacturas(PTipoNota) Then Return False
            If Not ArmaConLiquidaciones() Then Return False
        End If
        If PTipoNota = 700 Then
            If Not ArmaConFacturas(PTipoNota) Then Return False
            If Not ArmaConLiquidaciones() Then Return False
            If Not ArmaConNotas(500) Then Return False
        End If

        'Procesa Facturas.
        DtGridCompro.Clear()
        For Each Row As DataRow In DtFacturasCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 2
            Row1("Comprobante") = Row("Factura")
            If PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 50 Or PTipoNota = 70 Or PTipoNota = 60 Then
                Row1("Recibo") = Row("Factura")
                Row1("Fecha") = Row("Fecha")
            Else
                Row1("Recibo") = Row("ReciboOficial")
                Row1("Fecha") = Row("FechaFactura")
            End If
            Row1("Importe") = Row("Importe")
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
            Row1("Comprobante") = Row("Liquidacion")
            Row1("Recibo") = Row("ReciboOficial")
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
            Row1("Comprobante") = Row("Liquidacion")
            Row1("Recibo") = Row("Liquidacion")
            Row1("Fecha") = Row("Fecha")
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
            Row1("Comprobante") = Row("Nota")
            If Row("TipoNota") = 50 Or Row("TipoNota") = 70 Or Row("TipoNota") = 500 Or Row("TipoNota") = 700 Then
                Row1("Recibo") = Row("ReciboOficial")
                Row1("Fecha") = Row("FechaReciboOficial")
            Else
                Row1("Recibo") = Row("Nota")
                Row1("Fecha") = Row("Fecha")
            End If
            Row1("Importe") = Row("Importe")
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

        GridCompro.DataSource = DtGridCompro

        TipoAsiento = PTipoNota
        If (PTipoNota = 5 Or PTipoNota = 7) And DtNotaCabeza.Rows(0).Item("ChequeRechazado") <> 0 Then TipoAsiento = 7001
        If PTipoNota = 8 And DtNotaCabeza.Rows(0).Item("ChequeRechazado") <> 0 Then TipoAsiento = 7002
        If PTipoNota = 5 And DtNotaCabeza.Rows(0).Item("EsExterior") Then TipoAsiento = 10005
        If PTipoNota = 5 And DtNotaCabeza.Rows(0).Item("DiferenciaDeCambio") Then TipoAsiento = 11005
        If PTipoNota = 7 And DtNotaCabeza.Rows(0).Item("EsExterior") Then TipoAsiento = 10007
        If PTipoNota = 7 And DtNotaCabeza.Rows(0).Item("DiferenciaDeCambio") Then TipoAsiento = 11007
        If PTipoNota = 6 And DtNotaCabeza.Rows(0).Item("EsExterior") Then TipoAsiento = 10006
        If PTipoNota = 6 And DtNotaCabeza.Rows(0).Item("DiferenciaDeCambio") Then TipoAsiento = 11006
        If PTipoNota = 8 And DtNotaCabeza.Rows(0).Item("EsExterior") Then TipoAsiento = 10008
        If PTipoNota = 8 And DtNotaCabeza.Rows(0).Item("DiferenciaDeCambio") Then TipoAsiento = 11008

        LabelInterno.Text = "Nro. Interno"

        'halla si es un debito por diferencia de factura.
        If PNota <> 0 Then
            PFacturaDiferencia = HallaFacturaDiferencia(PTipoNota, PNota)
            If PFacturaDiferencia = 0 Then
                MsgBox("Factura " & NumeroEditado(PFacturaDiferencia) & " No Encontrada. Operación se CANCELA.")
                Me.Close() : Exit Function
            End If
        End If

        If Not LeerComproFacturados(DtComproFacturados, PFacturaDiferencia) Then Me.Close() : Exit Function
        PCodigoFactura = HallaCodigoFactura(PFacturaDiferencia, ConexionNota)
        If PCodigoFactura < 0 Then Me.Close() : Exit Function
        If PNota = 0 And PTipoNota = 6 Then ArmaConDiferenciaFactura()
        Dim Operacion As Integer
        If PAbierto Then
            Operacion = 1
        Else
            Operacion = 2
        End If
        For I As Integer = DtGridCompro.Rows.Count - 1 To 0 Step -1
            Row1 = DtGridCompro.Rows(I)
            If Row1.RowState <> DataRowState.Deleted Then
                If Row1("Tipo") = 2 And Row1("Comprobante") = PFacturaDiferencia And Row1("Operacion") = Operacion Then
                    If PNota = 0 Then
                        If Trunca(Row1("Saldo")) < PImporteDiferencia Then
                            MsgBox("Diferencia Supera Saldo de la Factura. Operación se CANCELA.")
                            Me.Close() : Exit Function
                        End If
                        Row1("Asignado") = PImporteDiferencia : Row1("Saldo") = Trunca(Row1("Saldo") - PImporteDiferencia)
                    End If
                Else
                    ''''             Row1.Delete()
                End If
            End If
        Next
        ''''     GridCompro.ReadOnly = True
        If PNota = 0 Then
            TextTotalDiferencia.Text = FormatNumber(PImporteDiferencia, GDecimales)
        Else
            TextTotalDiferencia.Text = FormatNumber(DtNotaCabeza.Rows(0).Item("Importe"), GDecimales)
        End If

        Panel3.Visible = True

        If PNota <> 0 Then
            Panel3.Enabled = False
            PictureLupaCuenta.Enabled = False
            ButtonAceptar.Text = "Modificar Recibo"
            LabelPuntoDeVenta.Visible = False
        Else
            Panel3.Enabled = True
            PictureLupaCuenta.Enabled = True
            ButtonAceptar.Text = "Grabar Recibo"
            LabelPuntoDeVenta.Visible = True
        End If

        If DtNotaCabeza.Rows(0).Item("Estado") = 3 Then
            GridCompro.ReadOnly = True
        Else : GridCompro.ReadOnly = False
        End If

        If PTipoNota = 6 Then LabelImporteNota.Text = "Importe Debito"
        If PTipoNota = 700 Then LabelImporteNota.Text = "Importe Credito"

        CalculaTotales()

        AddHandler DtGridCompro.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGridCompro_ColumnChanging)
        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If DtNotaCabeza.Rows(0).Item("NotaAnulacion") <> 0 Then
            Label4.Text = "Nota Anulada Electrónicamente por la Nota " & NumeroEditado(DtNotaCabeza.Rows(0).Item("NotaAnulacion"))
            Label4.Visible = True
            GridCompro.ReadOnly = True
        Else
            Label4.Visible = False
            GridCompro.ReadOnly = False
        End If

        Return True

    End Function
    Private Function ArmaConFacturas(ByVal TipoNota As Integer) As Boolean

        Dim Sql As String = ""

        If TipoNota = 5 Or TipoNota = 7 Or TipoNota = 50 Or TipoNota = 70 Then
            Sql = "SELECT * FROM FacturasCabeza WHERE Tr = 0 AND EsZ = 0 AND Estado <> 3 AND FacturasCabeza.Cliente = " & ComboEmisor.SelectedValue & " ORDER BY Factura,Fecha;"
        Else
            If PEsTr Then
                Sql = "SELECT * FROM FacturasProveedorCabeza WHERE Tr = 1 AND Estado = 1 AND Liquidacion = 0 AND Proveedor = " & ComboEmisor.SelectedValue & " ORDER BY Factura,Fecha;"
            Else
                Sql = "SELECT * FROM FacturasProveedorCabeza WHERE Tr = 0 AND Estado = 1 AND Liquidacion = 0 AND Proveedor = " & ComboEmisor.SelectedValue & " ORDER BY Factura,Fecha;"
            End If
        End If
        If Not Tablas.Read(Sql, ConexionNota, DtFacturasCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConNVLP() As Boolean

        Dim Sql As String = ""

        Sql = "SELECT * FROM NVLPCabeza WHERE Estado = 1 AND Cliente = " & ComboEmisor.SelectedValue & " ORDER BY Liquidacion,Fecha;"
        If Not Tablas.Read(Sql, ConexionNota, DtNVLPCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConNotas(ByVal TipoNota As Integer) As Boolean

        Dim Sql As String = ""

        Sql = "SELECT * FROM RecibosCabeza WHERE Estado = 1 AND TipoNota = " & TipoNota & " AND Emisor = " & ComboEmisor.SelectedValue & " ORDER BY Nota,Fecha;"
        If Not Tablas.Read(Sql, ConexionNota, DtComprobantesCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConLiquidaciones() As Boolean

        Dim Sql As String = ""

        Sql = "SELECT * FROM LiquidacionCabeza WHERE Estado = 1 AND LiquidacionCabeza.Proveedor = " & ComboEmisor.SelectedValue & " ORDER BY Liquidacion,Fecha;"

        If Not Tablas.Read(Sql, ConexionNota, DtLiquidacionCabeza) Then Return False

        Return True

    End Function
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

        Enlace = New Binding("Checked", MiEnlazador, "Secos")
        CheckSecos.DataBindings.Clear()
        CheckSecos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalRecibo.DataBindings.Clear()
        TextTotalRecibo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Saldo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextSaldo.DataBindings.Clear()
        TextSaldo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Dim Row As DataRowView = MiEnlazador.Current

        If Row("Importe") <> 0 Then
            TextTotalDiferencia.Text = FormatNumber(Row("Importe"))
        End If

        If Row("FechaReciboOficial") = "01/01/1800" Then
            TextFechaReciboOficial.Text = ""
        Else
            TextFechaReciboOficial.Text = Format(DtNotaCabeza.Rows(0).Item("FechaReciboOficial"), "dd/MM/yyyy")
        End If

        If Row("FechaContable") = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else
            TextFechaContable.Text = Format(DtNotaCabeza.Rows(0).Item("FechaContable"), "dd/MM/yyyy")
        End If

        If Row("Cae") <> 0 Then
            LabelCAE.Text = "Autorización AFIP  CAE: " & Row("Cae") & "  Vto: " & Strings.Right(Row("FechaCae"), 2) & "/" & Strings.Mid(Row("FechaCae"), 5, 2) & "/" & Strings.Left(Row("FechaCae"), 4) : LabelCAE.Visible = True
        Else
            LabelCAE.Visible = False
        End If

    End Sub
    Private Sub FormatMaskedNota(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If (PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 6 Or PTipoNota = 8) And Numero.Value <> 0 Then
            TextLetra.Text = LetraTipoIva(Val(Strings.Left(Numero.Value.ToString, 1)))
            LetraIva = HallaNumeroLetra(TextLetra.Text)
            Numero.Value = Format(Val(Strings.Right(Numero.Value.ToString, 12)), "000000000000")
        Else
            Numero.Value = Format(Numero.Value, "000000000000")
        End If

    End Sub
    Private Sub ParseMaskedNota(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 6 Or PTipoNota = 8 Then
            Numero.Value = CDbl(LetraIva & Format(Numero.Value, "000000000000"))
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

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
    Private Sub FormatReciboOficial(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If (PTipoNota = 50 Or PTipoNota = 500 Or PTipoNota = 70 Or PTipoNota = 700) And Numero.Value <> 0 Then
            TextLetra.Text = LetraTipoIva(Val(Strings.Left(Numero.Value.ToString, 1)))
            LetraIva = Val(Strings.Left(Numero.Value.ToString, 1))
            Numero.Value = Format(Val(Strings.Right(Numero.Value.ToString, 12)), "000000000000")
        Else
            Numero.Value = Format(Numero.Value, "000000000000")
        End If

    End Sub
    Private Sub ParseReciboOficial(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If PTipoNota = 50 Or PTipoNota = 500 Or PTipoNota = 70 Or PTipoNota = 700 Then
            Numero.Value = CDbl(LetraIva & Numero.Value)
        End If

    End Sub
    Private Sub ArmaConDiferenciaFactura()

        Dim Row As DataRow = DtGrid.NewRow

        ArmaDetalle(Row)

        Row("MedioPago") = 100
        Row("Detalle") = "Diferencia Factura " & PFacturaDiferencia
        Row("Neto") = PImporteDiferencia
        Row("Importe") = PImporteDiferencia

        DtGrid.Rows.Add(Row)
        TextTotalRecibo.Text = FormatNumber(PImporteDiferencia, GDecimales)

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos
        Dim RowsBusqueda() As DataRow

        If PCodigoFactura = 900 Then
            If Not ArmaListaParaTipoOperacionesReventa(ListaLotesParaAsiento) Then Return False
        End If

        'Arma lista de iva.
        For Each Row As DataRow In DtGrid.Rows
            If Row("MedioPago") = 100 And Row("Alicuota") <> 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = HallaClaveIva(Row("Alicuota"))
                Item.Importe = -Row("ImporteIva")
                Item.TipoIva = 5
                If Item.Importe <> 0 Then ListaIVA.Add(Item)
            End If
        Next

        'Arma lista de Retencion.
        Item = New ItemListaConceptosAsientos
        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
            If RowsBusqueda(0).Item("Tipo") = 4 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = Row("MedioPago")
                Item.Importe = -Row("Importe")
                Item.TipoIva = 9
                If Item.Importe <> 0 Then ListaRetenciones.Add(Item)
            End If
        Next

        Dim Neto As Decimal = 0
        For Each Row As DataRow In DtGrid.Rows
            If Row("MedioPago") = 100 Then Neto = Trunca(Neto + Row("Neto"))
        Next

        Item = New ItemListaConceptosAsientos
        Item.Clave = 202
        Item.Importe = -Neto
        ' If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
        ListaConceptos.Add(Item)

        'Arma Lista con Cuentas definidas en documento.
        If ListCuentas.Visible Then
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
                    Fila.Importe = -CDec(ListCuentas.Items.Item(I).SubItems(1).Text)
                    '  If ComboMoneda.SelectedValue <> 1 Then Fila.Importe = Trunca(CDec(TextCambio.Text) * Fila.Importe)
                    Fila.Clave = 202
                    ListaCuentas.Add(Fila)
                    ImporteLista = Trunca(ImporteLista + (-Fila.Importe))
                Next
                If ImporteLista <> Neto Then
                    MsgBox("Importe de Cuentas Informada Difiere del Neto de la Factura. " & ImporteLista & " " & Neto)
                    Return False
                End If
            End If
        End If

        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = -CDbl(TextTotalRecibo.Text)
        ListaConceptos.Add(Item)

        Dim Fecha As Date
        If PanelFechaContable.Visible Then
            Fecha = CDate(TextFechaContable.Text)
        Else : Fecha = DateTime1.Value
        End If

        If Not Asiento(PCodigoFactura, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Fecha, PTipoNota) Then Return False

        If DtCabeza.Rows.Count <> 0 Then DtCabeza.Rows(0).Item("TipoDocumento") = TipoAsiento

        Return True

    End Function
    Private Function ArmaListaParaTipoOperacionesReventa(ByRef Lista As List(Of ItemLotesParaAsientos)) As Boolean

        Dim ImporteNetoGrabadoInformado As Double

        For Each Row As DataRow In DtGrid.Rows
            If Row("MedioPago") = 100 Then
                ImporteNetoGrabadoInformado = ImporteNetoGrabadoInformado + Row("Neto")
            End If
        Next

        Dim Fila2 As New ItemLotesParaAsientos
        Dim Tipo As Integer
        Dim Centro As Integer

        If DtComproFacturados.Rows(0).Item("Operacion") = 1 Then
            HallaCentroTipoOperacion(DtComproFacturados.Rows(0).Item("Lote"), DtComproFacturados.Rows(0).Item("Secuencia"), Conexion, Tipo, Centro)
        Else
            HallaCentroTipoOperacion(DtComproFacturados.Rows(0).Item("Lote"), DtComproFacturados.Rows(0).Item("Secuencia"), ConexionN, Tipo, Centro)
        End If
        If Centro <= 0 Then
            MsgBox("Error en Tipo Operacion en Lote ")
            Return False
        End If
        '
        Fila2 = New ItemLotesParaAsientos
        Fila2.TipoOperacion = Tipo
        Fila2.Centro = Centro
        Fila2.Clave = 300 'reventa
        Fila2.MontoNeto = -ImporteNetoGrabadoInformado
        If Fila2.MontoNeto <> 0 Then Lista.Add(Fila2)

        Return True

    End Function
    Private Sub AgregaCabeza()

        Dim Row As DataRow

        Row = DtNotaCabeza.NewRow
        ArmaNuevoRecibo(Row)
        Row("TipoNota") = PTipoNota
        Row("Nota") = UltimoNumero
        Row("Emisor") = PEmisor
        Row("Fecha") = DateTime1.Value
        Row("CodigoIva") = 1
        Row("Estado") = 1
        Row("Caja") = GCaja
        Row("Tr") = PEsTr
        Row("Moneda") = 1
        Row("Cambio") = 1
        Row("Manual") = PManual
        Row("DiferenciaDeCambio") = PDiferenciaDeCambio
        If ComboTipoIva.SelectedValue = Exterior Then Row("EsExterior") = True
        If PAbierto Then
            Select Case PTipoNota
                Case 6
                    Row("EsElectronica") = EsPuntoDeVentaFacturasElectronicas(GPuntoDeVenta)
            End Select
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

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Comprobante)

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
        DocTipo = 0
        DocNro = 0

        If PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 50 Or PTipoNota = 70 Or PTipoNota = 60 Or PTipoNota = 65 Then
            Sql = "SELECT * FROM Clientes WHERE Clave = " & Cliente & ";"
            Dta = Tablas.Leer(Sql)
            If Dta.Rows.Count = 0 Then
                MsgBox("ERROR, Cliente No Existe o Error en la Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            DocTipo = Dta.Rows(0).Item("DocumentoTipo")
            DocNro = Dta.Rows(0).Item("DocumentoNumero")
        Else
            Sql = "SELECT * FROM Proveedores WHERE Clave = " & Cliente & ";"
            Dta = Tablas.Leer(Sql)
            If Dta.Rows.Count = 0 Then
                MsgBox("ERROR, Proveedor No Existe o Error en la Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        Calle = Dta.Rows(0).Item("Calle")
        Localidad = Dta.Rows(0).Item("Localidad")
        Provincia = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
        TextCuit.Text = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")
        Cuit = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")

        Dta.Dispose()

        Return True

    End Function
    Private Sub CalculaTotales()

        Dim TotalFacturas As Decimal = 0

        For Each Row As DataRow In DtGridCompro.Rows
            If Row.RowState <> DataRowState.Deleted Then
                TotalFacturas = TotalFacturas + Row("Asignado")
            End If
        Next

        TextTotalFacturas.Text = FormatNumber(TotalFacturas, GDecimales)

        Select Case PTipoNota    'Esto es para que no modifique el saldo cuando se regraba la nota.(Saldo esta enlazado con Registro cabeza.
            Case 7, 13007, 50, 600, 6, 13006, 700
                TextSaldo.Text = FormatNumber(CDec(TextTotalRecibo.Text) - ImputacionDeOtros - TotalFacturas, GDecimales)
            Case Else
                If PNota = 0 Then
                    TextSaldo.Text = FormatNumber(CDec(TextTotalRecibo.Text), GDecimales)
                End If
        End Select

    End Sub
    Private Function HacerAlta(ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable, ByVal DtComproFacturadosAux As DataTable, ByVal DtNotaLotesAux As DataTable) As Boolean
        'Graba Facturas.

        If Not FacturaAnteriorOK Then
            MsgBox("La Nota no se puede Grabar hasta que no autorice la Nota anterior por la AFIP. Operacion se CANCELA.", MsgBoxStyle.Information)
            Exit Function
        End If

        Dim NumeroNota As Double
        Dim NumeroAsiento As Double
        Dim NumeroW As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion comprobante.
            If (PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 6 Or PTipoNota = 8) And PAbierto Then NumeroNota = UltimoNumero
            If (PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 6 Or PTipoNota = 8) And Not PAbierto Then NumeroNota = UltimaNumeracionDebitoCredito(ConexionNota)
            If (PTipoNota = 50 Or PTipoNota = 70 Or PTipoNota = 500 Or PTipoNota = 700) Then NumeroNota = UltimaNumeracionPagoYOrden(PTipoNota, ConexionNota)
            If PManual Then NumeroNota = Val(MaskedNota.Text)
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

            NumeroW = ActualizaNota("A", DtGridAux, DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtComproFacturadosAux, DtNotaLotesAux)

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
            PideAutorizacionAfip(DtNotaCabezaAux)
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PNota = NumeroNota
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function HacerModificacion(ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable, ByVal DtComproFacturadosAux As DataTable, ByVal DtNotaLotesAux As DataTable) As Boolean

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

            Resul = ActualizaNota("M", DtGrid, DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtComproFacturadosAux, DtNotaLotesAux)

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
    Private Function ActualizaNota(ByVal Funcion As String, ByVal DtGridAux As DataTable, ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable, ByVal DtComproFacturadosAux As DataTable, ByVal DtNotaLotesAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                Resul = ActualizaRecibo(DtFormasPago, Funcion, DtGridAux, DtNotaCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, ConexionNota, False)
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
                If Not IsNothing(DtAsientoCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaAux.GetChanges, "AsientosCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleAux.GetChanges, "AsientosDetalle", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Funcion = "A" Then
                    If Not ReGrabaFacturaDiferencia(PFacturaDiferencia, DtNotaCabezaAux.Rows(0).Item("TipoNota"), DtNotaCabezaAux.Rows(0).Item("Nota")) Then
                        MsgBox("Error Base de Datos al Grabar Factura Diferencia. Operación se CANCELA.", MsgBoxStyle.Critical)
                        Return -3
                    End If
                End If
                If Funcion = "B" Then
                    If Not ReGrabaFacturaDiferencia(PFacturaDiferencia, 0, -1) Then
                        MsgBox("Error Base de Datos al Grabar Factura Diferencia. Operación se CANCELA.", MsgBoxStyle.Critical)
                        Return -3
                    End If
                End If

                If Not IsNothing(DtComproFacturadosAux.GetChanges) Then
                    Resul = GrabaTabla(DtComproFacturadosAux.GetChanges, "ComproFacturados", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtNotaLotesAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaLotesAux.GetChanges, "RecibosLotes", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If

                Scope.Complete()
                Return DtNotaCabezaAux.Rows(0).Item("Nota")
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function BorraNota(ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtNotaLotesAux As DataTable, ByVal DtComproFacturadosAux As DataTable, ByVal DtPuntosDeVenta As DataTable, ByVal ReciboOficial As Decimal) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "RecibosCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtMedioPagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "RecibosDetallePago", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtNotaLotesAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaLotesAux.GetChanges, "RecibosLotes", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtComproFacturadosAux.GetChanges) Then
                    Resul = GrabaTabla(DtComproFacturadosAux.GetChanges, "ComproFacturados", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If

                If Not ReGrabaFacturaDiferencia(PFacturaDiferencia, 0, -1) Then
                    MsgBox("Error Base de Datos al Grabar Factura Diferencia. Operación se CANCELA.", MsgBoxStyle.Critical)
                    Return -3
                End If

                If Not IsNothing(DtPuntosDeVenta.GetChanges) Then
                    Resul = GrabaTabla(DtPuntosDeVenta.GetChanges, "PuntosDeVenta", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If

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
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function PideAutorizacionAfip(ByVal DtCabezaW As DataTable) As Boolean

        If Not PAbierto Then Return True
        If Not DtCabezaW.Rows(0).Item("EsElectronica") Then Return True

        Dim CAE As String = ""
        Dim FechaCae As String = ""
        Dim Concepto As Integer = 0
        Dim FchServDesde As Date = DateAdd(DateInterval.Day, 30, CDate(TextFechaContable.Text))
        Dim FchServHasta As Date = DateAdd(DateInterval.Day, 30, CDate(TextFechaContable.Text))
        Dim FchVtoPago As Date = DateAdd(DateInterval.Day, 30, CDate(TextFechaContable.Text))
        Concepto = 1
        Dim CbteTipoAsociado As Integer = DtCabezaW.Rows(0).Item("TipoCompAsociado")
        Dim CbteAsociado As Decimal = DtCabezaW.Rows(0).Item("CompAsociado")
        Dim CancelarGrabar As Boolean

        Dim DatosParaAfip As New ItemDatosParaAFIP
        DatosParaAfip.InscripcionAfip = ComboTipoIva.SelectedValue    'Responsable insc, montributo, etc.
        DatosParaAfip.DocTipo = DocTipo    'Tipo Documento Consumidor Final.
        DatosParaAfip.DocNro = DocNro      'Numero Documento Consumidor Final.

        Dim Mensaje As String = Autorizar("CD", DtCabezaW, DtGrid, New DataTable, FchServDesde, FchServHasta, FchVtoPago, LetraIva, DtCabezaW.Rows(0).Item("Nota"), Cuit, Concepto, 0, CAE, FechaCae, CbteTipoAsociado, CbteAsociado, CancelarGrabar, DatosParaAfip)
        If CAE = "" Then
            MsgBox(Mensaje + vbCrLf + "Deberá Pedir Autorización AFIP.")
            Return False
        End If

        If CAE <> "" Then
            If Not GrabaCAE(DtCabezaW.Rows(0).Item("TipoNota"), DtCabezaW.Rows(0).Item("Nota"), CDec(CAE), CInt(FechaCae), CbteTipoAsociado, CbteAsociado) Then
                MsgBox("CAE de la AFIP No se pudo Grabar." + vbCrLf + "Deberá Pedir Autorización AFIP.")
                Return False
            End If
        End If

        Return True

    End Function
    Public Function GrabaCAE(ByVal TipoNota As Integer, ByVal Nota As Decimal, ByVal Cae As Decimal, ByVal FechaCae As Integer, ByVal CbteTipoAsociado As Integer, ByVal CbteAsociado As Decimal) As Boolean

        Dim Sql As String = "UPDATE RecibosCabeza Set CAE = " & Cae & ",FechaCae = " & FechaCae & ",TipoCompAsociado = " & CbteTipoAsociado & ",CompAsociado = " & CbteAsociado & _
            " WHERE TipoNota = " & TipoNota & " AND Nota = " & Nota & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                If CInt(Cmd.ExecuteNonQuery()) = 0 Then Return False
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos al Grabar CAE en RecibosCabeza." & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End Try

        Return True

    End Function
    Private Function UltimaNumeracionDebitoCredito(ByVal ConexionStr) As Double

        Dim Patron As String = HallaNumeroLetra(TextLetra.Text) & Format(GPuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Nota) FROM RecibosCabeza WHERE TipoNota = " & PTipoNota & " AND CAST(CAST(RecibosCabeza.Nota AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(LetraIva & Format(GPuntoDeVenta, "0000") & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function ReGrabaFacturaDiferencia(ByVal Factura As Double, ByVal TipoNota As Integer, ByVal Nota As Double) As Boolean

        Dim Sql As String = "UPDATE " & "FacturasProveedorCabeza" & _
                 " Set NotaDebito = " & Nota & ",TipoNota = " & TipoNota & _
                 " WHERE Factura = " & Factura & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionNota)
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
    Private Sub PideDatosEmisor()

        If PTipoNota = 50 Or PTipoNota = 70 Or PTipoNota = 500 Or PTipoNota = 700 Then
            If PTipoNota = 50 Or PTipoNota = 70 Then
                OpcionLetra.PEsProveedor = False
            Else : OpcionLetra.PEsProveedor = True
            End If
            OpcionLetra.PEmisorBloqueado = PEmisor
            OpcionLetra.PTipoNota = PTipoNota
            OpcionLetra.ShowDialog()
            PEmisor = OpcionLetra.PEmisor
            PAbierto = OpcionLetra.PAbierto
            LetraIva = OpcionLetra.PNumeroLetra
            TextLetra.Text = LetraTipoIva(LetraIva)
            OpcionLetra.Dispose()
            If PEmisor = 0 Then Exit Sub
            If LetraIva = 4 Then
                MsgBox("Letra No valida para el Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PEmisor = 0
            End If
            GPuntoDeVenta = HallaPuntoVentaSegunTipo(PTipoNota, LetraIva)
            If GPuntoDeVenta = 0 Then
                MsgBox("No tiene Definido Punto de Venta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PEmisor = 0
            End If
            If Not ExistePuntoDeVenta(GPuntoDeVenta) Then
                MsgBox("Punto de Venta " & GPuntoDeVenta & " No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PEmisor = 0
            End If
            If EsPuntoDeVentaZ(GPuntoDeVenta) Then
                MsgBox("Punto de Venta del Operador " & GPuntoDeVenta & " Reservado Para Ticket Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PEmisor = 0
                Exit Sub
            End If

            LabelPuntoDeVenta.Text = "Punto de Venta " & Format(GPuntoDeVenta, "0000")
            Exit Sub
        End If

        If PTipoNota = 5 Or PTipoNota = 7 Then
            LetraIva = LetrasPermitidasCliente(HallaTipoIvaCliente(PEmisor), PTipoNota)
        End If

        If PTipoNota = 6 Or PTipoNota = 8 Then
            LetraIva = LetrasPermitidasProveedor(HallaTipoIvaProveedor(PEmisor), PTipoNota)
        End If

        If PEsTr Then
            If LetraIva = Exterior Then
                MsgBox("Cliente o Proveedor No Habilitado para Este Recibo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PEmisor = 0
            End If
        End If

        ''''''''''       If PTipoNota = 6 Or PTipoNota = 8 Or PTipoNota = 5 Or PTipoNota = 7 Then
        ''''''''''''If LetraIva = 3 Then LetraIva = 2
        '''''''''''''' End If

        ''''     If GTipoIva = 2 Then
        ''''Select Case PTipoNota
        ''''    Case 5, 7, 6, 8
        ''''LetraIva = 3
        '''' End Select
        '''' End If

        TextLetra.Text = LetraTipoIva(LetraIva)

        GPuntoDeVenta = HallaPuntoVentaSegunTipo(PTipoNota, LetraIva)
        If GPuntoDeVenta = 0 Then
            MsgBox("No tiene Definido Punto de Venta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
    Private Sub ActualizaComprobantes(ByVal Funcion As String, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable)

        Dim RowsBusqueda() As DataRow

        'Actualiza Saldo de Comprobantes Imputados.

        If Funcion = "M" Then
            For Each Row1 As DataRow In DtGridCompro.Rows
                If Row1.RowState <> DataRowState.Deleted Then
                    If Row1("Saldo") <> Row1("SaldoAnt") Then
                        If Row1("Tipo") = 2 Then
                            RowsBusqueda = DtFacturasCabezaAux.Select("Factura = " & Row1("Comprobante"))
                            RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                        Else
                            If Row1("Tipo") = 10 Then
                                RowsBusqueda = DtLiquidacionCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                            Else
                                If Row1("Tipo") = 30 Then
                                    RowsBusqueda = DtSenaAux.Select("Comprobante = " & Row1("Comprobante"))
                                    RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                                Else
                                    If Row1("Tipo") = 800 Then
                                        RowsBusqueda = DtNVLPCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                        RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                                    Else
                                        RowsBusqueda = DtComprobantesCabezaAux.Select("TipoNota = " & Row1("Tipo") & " AND Nota = " & Row1("Comprobante"))
                                        RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If
        '
        If Funcion = "B" Then
            Dim Importe As Double = 0
            For Each Row1 As DataRow In DtGridCompro.Rows
                If Row1.RowState <> DataRowState.Deleted Then
                    If Row1.Item("Asignado") <> 0 Then
                        If Row1("Tipo") = 2 Then
                            RowsBusqueda = DtFacturasCabezaAux.Select("Factura = " & Row1("Comprobante"))
                            RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                        Else
                            If Row1("Tipo") = 800 Then
                                RowsBusqueda = DtNVLPCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                            Else
                                If Row1("Tipo") = 10 Then
                                    RowsBusqueda = DtLiquidacionCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                    RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                                Else
                                    If Row1("Tipo") = 30 Then
                                        RowsBusqueda = DtSenaAux.Select("Comprobante = " & Row1("Comprobante"))
                                        RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                                    Else
                                        RowsBusqueda = DtComprobantesCabezaAux.Select("TipoNota = " & Row1("Tipo") & " AND Nota = " & Row1("Comprobante"))
                                        RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If

    End Sub
    Private Function LeerComproFacturados(ByRef DtComproFacturadosAux As DataTable, ByVal Factura As Double) As Boolean

        DtComproFacturadosAux = New DataTable
        If Not Tablas.Read("SELECT * FROM ComproFacturados WHERE Factura = " & Factura & ";", ConexionNota, DtComproFacturadosAux) Then
            Return False
        End If
        If DtComproFacturadosAux.Rows.Count = 0 Then
            MsgBox("Factura Proveedor " & NumeroEditado(Factura) & " No Encontrada. Operación se CANCELA.")
            Return False
        End If

        Return True

    End Function
    Private Function ActualizaComproFacturados(ByVal Funcion As String, ByRef DtComproFacturadosAux As DataTable) As Boolean

        If PCodigoFactura = 902 Then Return True

        'prorrotea importe comprobantes.  
        Dim ImporteConIva As Decimal
        Dim ImporteSinIva As Decimal
        For Each Row As DataRow In DtGrid.Rows
            If Row("MedioPago") = 100 Then
                ImporteSinIva = ImporteSinIva + Row("Neto")
            End If
            ImporteConIva = ImporteConIva + Row("Importe")
        Next

        If Funcion = "A" Then
            ImporteSinIva = -ImporteSinIva
            ImporteConIva = -ImporteConIva
        End If

        Dim ImporteTotalLotes As Decimal
        For Each Row As DataRow In DtComproFacturadosAux.Rows
            ImporteTotalLotes = ImporteTotalLotes + Row("Importe")
        Next

        Dim CoeficienteConIva As Decimal = ImporteConIva / ImporteTotalLotes
        Dim CoeficienteSinIva As Decimal = ImporteSinIva / ImporteTotalLotes

        For Each Row As DataRow In DtComproFacturadosAux.Rows
            Row("ImporteConIva") = CDec(Row("ImporteConIva")) + CDec(Row("Importe")) * CoeficienteConIva
            Row("ImporteSinIva") = CDec(Row("ImporteSinIva")) + CDec(Row("Importe")) * CoeficienteSinIva
            If Row("ImporteConIva") < 0 Or Row("ImportesinIva") < 0 Then
                MsgBox("Importe Recibo Hace Negativo el Neto por Lotes.")
                Return False
            End If
        Next

        Return True

    End Function
    Private Function EsFacturaAnteriorOk(ByVal TipoNotaW As Integer, ByVal NotaW As Decimal) As Boolean

        Dim Numero As Decimal = Strings.Right(NotaW, 8)
        If Numero - 1 = 0 Then Return True

        Select Case TipoNotaW
            Case 7, 8
                Return EsNotaCreditoAnteriorOk(NotaW)
                Exit Function
        End Select

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cae FROM RecibosCabeza WHERE (TipoNota = 5 OR TipoNota = 6) AND Nota = " & NotaW - 1 & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then Return False
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: RecibosCabeza (A). Operación se CANCELA.", MsgBoxStyle.Critical)
            End
        End Try

        Return True

    End Function
    Private Function EsNotaCreditoAnteriorOk(ByVal Nota As Decimal) As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Cae FROM RecibosCabeza WHERE (TipoNota = 7 or TipoNota = 8) AND Nota = " & Nota - 1 & ";", Conexion, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            If Dt.Rows(0).Item("Cae") = 0 Then
                Dt.Dispose() : Return False
            Else
                Dt.Dispose() : Return True
            End If
        End If

        Dt = New DataTable

        If Not Tablas.Read("SELECT Cae FROM NotasCreditoCabeza WHERE NotaCredito = " & Nota - 1 & ";", Conexion, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            If Dt.Rows(0).Item("Cae") = 0 Then
                Dt.Dispose() : Return False
            Else
                Dt.Dispose() : Return True
            End If
        End If

    End Function
    Private Function HallaTipo(ByVal MedioPago As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtFormasPago.Select("Clave = " & MedioPago)
        Return RowsBusqueda(0).Item("Tipo")

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
    Private Function UltimaFechaLetraPuntoVenta(ByVal ConexionStr) As Date

        Dim Patron As String = LetraIva & Format(GPuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM RecibosCabeza WHERE TipoNota = " & PTipoNota & " AND CAST(CAST(RecibosCabeza.Nota AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
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
    Private Sub Print_PrintPageNota(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim PrintFont As System.Drawing.Font

        If EsFacturaElectronica Then
            Dim NombreEmpres As String = GNombreEmpresa '"Patagonia Sunrise S.R.L."
            Dim CuitEmpresa As String = GCuitEmpresa    '"30-12345678-5"
            Dim TipoComprobante As Integer          '1. factura 2. debito 3. credito
            Select Case PTipoNota
                Case 5, 6
                    TipoComprobante = 2
                Case 7, 8
                    TipoComprobante = 3
            End Select
            Dim Direccion1 As String = GDireccion1      '"Nave S-2 Puesto 34 - Mercado Central de Buenos Aires"
            Dim Direccion2 As String = GDireccion2      '"Autopista Ricchieri y Boulongne Sur Mer"
            Dim Direccion3 As String = GDireccion3      '"C.P.(1771) Villa Celina Pcia. de Buenos Aires"
            Dim LetraComprobante As String = LetraTipoIva(LetraIva)
            Dim IngBruto As String = GIngBruto     '"Conv.Mult. Nª 901-898989-6"
            Dim FechaInicio As String = GFechaInicio
            '
            MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, LetraComprobante, GCondicionIvaEmpresa, IngBruto, FechaInicio, "C:\XML Afip\", e, False, PTipoNota)
            Texto = NumeroEditado(Strings.Right(DtNotaCabeza.Rows(0).Item("Nota").ToString, 12))
            PrintFont = New Font("Arial", 16)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, MTop + 6)
        End If

        x = MIzq + 135 : y = MTop

        PrintFont = New Font("Courier New", 12)
        If TextFechaContable.Visible Then
            Texto = TextFechaContable.Text
        Else
            Texto = Format(DateTime1.Value, "dd/MM/yyyy")
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y + 15)

        PrintFont = New Font("Courier New", 11)
        x = MIzq : y = MTop + 42

        Try
            'Titulos.
            If PAbierto Then
                Texto = "Razón Social: " & ComboEmisor.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "DOMICILIO   : " & Calle
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "LOCALIDAD   : " & Localidad & " " & Provincia
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "DOM.ENTREGA : "
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "CUIT        : " & TextCuit.Text & " " & ComboTipoIva.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
            Else
                Texto = "R. Social    : " & ComboEmisor.Text & "                Nro.: " & MaskedNota.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            End If

            PrintFont = New Font("Courier New", 10)

            'Grafica -Rectangulo----------------------------------------------------------------------
            x = MIzq
            y = MTop + 72

            Dim Ancho As Integer = 185
            Dim Alto As Integer = 95
            Dim LineaDescripcion As Integer = x + 145
            Dim LineaImporte As Integer = x + Ancho
            Dim Longi As Integer
            Dim Xq As Integer
            Dim Yq As Integer
            Dim RowsBusqueda() As DataRow

            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaDescripcion, y, LineaDescripcion, y + Alto)
            'Titulos de descripcion.
            Texto = "DESCRIPCION"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 2, y + 2)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)
            '------------------------------------------------------------------------------------------------------------
            'Descripcion de Articulos.
            Yq = y - SaltoLinea
            For Each Row As DataRow In DtGrid.Rows
                Yq = Yq + SaltoLinea
                'Imprime Detalle.
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                Texto = RowsBusqueda(0).Item("Nombre")
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Neto.
                Select Case LetraIva
                    Case 2, 3
                        Texto = FormatNumber(Row("Importe"), GDecimales)
                    Case Else
                        Texto = FormatNumber(Row("Neto"), GDecimales)
                End Select
                Texto = FormatNumber(Row.Item("Neto"), GDecimales)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Next
            'Resuardo
            Yq = MTop + 72 + Alto + 2
            If PAbierto Then
                Texto = GNombreEmpresa & " " & NumeroEditado(MaskedNota.Text)
            Else
                Texto = NumeroEditado(MaskedNota.Text)
            End If
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, Yq)

            'Totales
            PrintFont = New Font("Courier New", 11)

            'Neto
            Dim TotalNeto As Double = 0
            Select Case LetraIva
                Case 2, 3
                    For Each Row As DataRow In DtGrid.Rows
                        TotalNeto = TotalNeto + Row("Importe")
                    Next
                Case Else
                    For Each Row As DataRow In DtGrid.Rows
                        TotalNeto = TotalNeto + Row("Neto")
                    Next
            End Select
            Texto = "Neto"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaImporte - 60, Yq)
            Texto = FormatNumber(TotalNeto, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            '
            Dim ListaIva As New List(Of ItemIva)
            Select Case LetraIva
                Case 2, 3
                Case Else
                    ArmaListaImportesIva(ListaIva)
            End Select
            'Iva.
            For Each Fila As ItemIva In ListaIva
                Yq = Yq + SaltoLinea
                Texto = "IVA. " & FormatNumber(Fila.Iva, GDecimales)
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaImporte - 60, Yq)
                Texto = FormatNumber(Fila.Importe, GDecimales)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Next
            ListaIva = Nothing

            'Total
            Yq = Yq + SaltoLinea
            Texto = "Total"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaImporte - 60, Yq)
            Texto = TextTotalRecibo.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)

            'Texto
            Yq = MTop + 72 + Alto + 10
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox1.Text, PrintFont, Brushes.Black, x, Yq)
            Yq = Yq + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox2.Text, PrintFont, Brushes.Black, x, Yq)
            Yq = Yq + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox3.Text, PrintFont, Brushes.Black, x, Yq)
            Yq = Yq + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox4.Text, PrintFont, Brushes.Black, x, Yq)

            'Imprime Cae ----------------------------------------------
            If DtNotaCabeza.Rows(0).Item("Cae") <> 0 Then
                PrintFont = New Font("Courier New", 14)
                Yq = 270
                e.Graphics.DrawString(LabelCAE.Text, PrintFont, Brushes.Black, x, Yq)
            End If
            '-----------------------------------------------------------

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
        Dim Iva As Double = 0

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
    Private Function Valida() As Boolean

        Dim PuntoVentaManual As Boolean

        If PManual Then
            PuntoVentaManual = EsReciboManual(Val(Strings.Left(MaskedNota.Text, 4)))
        Else : PuntoVentaManual = EsReciboManual(GPuntoDeVenta)
        End If

        If PNota = 0 Then
            If Not PManual And PuntoVentaManual Then
                MsgBox("Punto de Venta del Comprobante SOLO Habilitado para Recibo Manual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedNota.Focus()
                Return False
            End If
        End If

        Select Case PTipoNota
            Case 50, 70, 500, 700
            Case Else
                '        If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 Then
                '       MsgBox("Fecha Menor a la Fecha del ultimo Recibo Grabado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                '          DateTime1.Focus()
                '          Return False
                '         End If
        End Select

        If Panel5.Visible = True Then
            If Val(MaskedReciboOficial.Text) = 0 Then
                MsgBox("Falta informar Recibo Oficial.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedReciboOficial.Focus()
                Return False
            End If
            If Not MaskedOK(MaskedReciboOficial) Then
                MsgBox("Numero Recibo Oficial Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedReciboOficial.Focus()
                Return False
            End If
            If ExisteReciboOficial(PTipoNota, PNota, ComboEmisor.SelectedValue, DtNotaCabeza.Rows(0).Item("ReciboOficial"), ConexionNota) Then
                MsgBox("Recibo Oficial Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedReciboOficial.Focus()
                Return False
            End If
            If Not ConsisteFecha(TextFechaReciboOficial.Text) Then
                MsgBox("Fecha Recibo Oficial Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaReciboOficial.Focus()
                Return False
            End If
            If DiferenciaDias(DateTime1.Value, TextFechaReciboOficial.Text) < -365 Then
                MsgBox("Fecha Recibo Oficial Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaReciboOficial.Focus()
                Return False
            End If
            If DiferenciaDias(DateTime1.Value, TextFechaReciboOficial.Text) > 0 Then
                MsgBox("Fecha Recibo Oficial Mayor que la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
                TextFechaReciboOficial.Focus()
                Return False
            End If
        End If

        If CDbl(TextTotalRecibo.Text) = 0 Then
            MsgBox("Debe informar Importes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then
            If PTipoNota = 6 And CDbl(TextTotalRecibo.Text) <> PImporteDiferencia Then
                MsgBox("Total Recibo Debe ser Igual al Importe de la Diferencia.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If PTipoNota = 700 And CDec(TextTotalRecibo.Text) < PImporteDiferencia Then
                If Abs(CDec(TextTotalRecibo.Text) - PImporteDiferencia) > GTolerancia Then
                    MsgBox("Total Recibo Debe ser Igual o Mayor al Importe de la Diferencia.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
            If PTipoNota = 700 And PImporteFactura - CDec(TextTotalRecibo.Text) < 0 Then
                MsgBox("Total Recibo Mayor al Importe de la Factura.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If CDec(TextTotalRecibo.Text) - CDec(TextTotalFacturas.Text) < 0 Then
            MsgBox("Importes Imputados supera importe de Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        For Each Row1 As DataRow In DtGridCompro.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                If Row1("Saldo") <> Row1("SaldoAnt") Then
                    If Row1("Moneda") <> 1 Then
                        MsgBox("Se Imputo a Documento en Distinta Moneda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        GridCompro.Focus()
                        Return False
                    End If
                End If
            End If
        Next

        Return True

    End Function
    Private Sub ArmaDetalle(ByVal Row As DataRow)

        Row("Item") = 0
        Row("MedioPago") = 0
        Row("Detalle") = ""
        Row("Neto") = 0
        Row("Alicuota") = 0
        Row("ImporteIva") = 0
        Row("Banco") = 0
        Row("Fecha") = "1/1/1800"
        Row("Cuenta") = 0
        Row("Serie") = ""
        Row("Numero") = 0
        Row("EmisorCheque") = ""
        Row("Cambio") = 0
        Row("Importe") = 0
        Row("Comprobante") = 0
        Row("FechaComprobante") = "1/1/1800"
        Row("ClaveCheque") = 0
        Row("ClaveChequeVisual") = 0
        Row("ClaveInterna") = 0
        Row("TieneLupa") = False
        Row("Bultos") = 0

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL DtGRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row

        ArmaDetalle(Row)

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Detalle") Then
            If IsDBNull(e.Row("Detalle")) Then Exit Sub
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
        End If

        If e.Column.ColumnName.Equals("Neto") Then
            If IsDBNull(e.Row("Neto")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
            If Not IsDBNull(e.Row("Alicuota")) Then
                e.Row("ImporteIva") = Trunca(e.ProposedValue * e.Row("Alicuota") / 100)
                e.Row("Importe") = CDec(e.ProposedValue) + e.Row("ImporteIva")
            End If
        End If

        If e.Column.ColumnName.Equals("Alicuota") Then
            If IsDBNull(e.Row("Alicuota")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
            If e.ProposedValue >= 100 Then
                MsgBox("Alicuota Invalida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Alicuota")
                Exit Sub
            End If
            Dim Esta As Boolean
            For Each Item As Double In TablaIva
                If Item = e.ProposedValue Then Esta = True : Exit For
            Next
            If Esta = False Then
                MsgBox("Alicuota no Existe en el Sistema.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Alicuota")
            End If
            If Not IsDBNull(e.Row("Neto")) Then
                e.Row("Neto") = e.Row("Neto") / (1 + CDec(e.ProposedValue) / 100)
                e.Row("ImporteIva") = Trunca(e.ProposedValue * e.Row("Neto") / 100)
                e.Row("Importe") = e.Row("Neto") + e.Row("ImporteIva")
            End If
        End If

        If e.Column.ColumnName.Equals("Cuenta") Then
            If IsDBNull(e.Row("Cuenta")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Numero") Then
            If IsDBNull(e.Row("Numero")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Cambio") Then
            If IsDBNull(e.Row("Cambio")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

        If e.Column.ColumnName.Equals("Importe") Then
            If IsDBNull(e.Row("Importe")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

        If (e.Column.ColumnName.Equals("Comprobante")) Then
            If IsDBNull(e.Row("Comprobante")) Then Exit Sub
            If IsNothing(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If (e.Column.ColumnName.Equals("EmisorCheque")) Then
            If IsDBNull(e.Row("EmisorCheque")) Then Exit Sub
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
        End If

        CalculaTotales()

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub ButtonMediosDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMediosDePago.Click

        UnMediosPago.PTipoNota = PTipoNota
        UnMediosPago.PDtGrid = DtGrid
        UnMediosPago.PAbierto = PAbierto
        If PNota = 0 Then
            UnMediosPago.PBloqueaFunciones = False
        Else : UnMediosPago.PBloqueaFunciones = True
        End If
        UnMediosPago.PDtFormasPago = DtFormasPago
        UnMediosPago.PDtRetencionProvincia = DtRetencionProvinciaAux
        If ComboTipoIva.SelectedValue = Exterior Then
            UnMediosPago.PEsExterior = True
        Else : UnMediosPago.PEsExterior = False
        End If
        UnMediosPago.PDtRetencionesAutomaticas = New DataTable
        UnMediosPago.PDiferenciaDeCambio = PDiferenciaDeCambio
        UnMediosPago.PMoneda = 1
        UnMediosPago.PCambio = 1
        UnMediosPago.PImporte = CDbl(TextTotalRecibo.Text)
        UnMediosPago.PImporteAInformar = PImporteDiferencia
        UnMediosPago.PEsTr = PEsTr
        UnMediosPago.ShowDialog()
        DtGrid = UnMediosPago.PDtGrid
        TextTotalRecibo.Text = FormatNumber(UnMediosPago.PImporte, GDecimales)

        UnMediosPago.Dispose()

        '   For Each Row As DataRow In DtGridCompro.Rows
        '     If Row.RowState <> DataRowState.Deleted Then
        '   If Row("Tipo") = 2 And Row("Comprobante") = PFacturaDiferencia Then
        '    Row("Asignado") = CDbl(TextTotalRecibo.Text)
        '     End If
        '      End If
        '       Next

        CalculaTotales()

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridCompro_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellContentClick

        If GridCompro.Columns(e.ColumnIndex).Name = "Seleccion" Then
            Dim chkCell As DataGridViewCheckBoxCell = Me.GridCompro.Rows(e.RowIndex).Cells("Seleccion")
            chkCell.Value = Not chkCell.Value
            If chkCell.Value Then
                If GridCompro.Rows(e.RowIndex).Cells("Saldo").Value < 0 Then Exit Sub
                Dim Diferencia As Double = 0
                Diferencia = CDbl(TextTotalRecibo.Text) - CDbl(TextTotalFacturas.Text) - GridCompro.Rows(e.RowIndex).Cells("Asignado").Value
                If Diferencia <= 0 Then Exit Sub
                If (GridCompro.Rows(e.RowIndex).Cells("Saldo").Value - Diferencia) <= 0 Then
                    GridCompro.Rows(e.RowIndex).Cells("Asignado").Value = GridCompro.Rows(e.RowIndex).Cells("Saldo").Value
                Else
                    GridCompro.Rows(e.RowIndex).Cells("Asignado").Value = Diferencia
                End If
                CalculaTotales()
            Else
                GridCompro.Rows(e.RowIndex).Cells("Asignado").Value = 0
                CalculaTotales()
            End If
        End If

    End Sub
    Private Sub GridCompro_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEnter

        If Not GridCompro.Columns(e.ColumnIndex).ReadOnly Then
            GridCompro.BeginEdit(True)
        End If

    End Sub
    Private Sub GridCompro_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles GridCompro.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If GridCompro.Columns(e.ColumnIndex).Name = "Comprobante1" Then
            e.Value = NumeroEditado(e.Value)
            If PermisoTotal Then
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Recibo" Then
            e.Value = NumeroEditado(e.Value)
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
    Private Sub GridCompro_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles GridCompro.EditingControlShowing

        Dim columna As Integer = GridCompro.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPressCompro
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChangedCompro

    End Sub
    Private Sub ValidaKey_KeyPressCompro(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Asignado" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChangedCompro(ByVal sender As Object, ByVal e As System.EventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Asignado" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

    End Sub
    Private Sub GridCompro_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEndEdit

        If GridCompro.Columns(e.ColumnIndex).Name = "Asignado" Then
            If IsDBNull(GridCompro.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then GridCompro.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0
            CalculaTotales()
        End If

    End Sub
    Private Sub DtGridCompro_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        Dim SaldoAnt As Decimal = 0

        If (e.Column.ColumnName.Equals("Asignado")) Then
            If IsDBNull(e.Row("Asignado")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
            If CDec(TextTotalFacturas.Text) - e.Row("Asignado") + CDec(e.ProposedValue) > CDec(TextTotalRecibo.Text) Then
                MsgBox("Imputación Supera Total del Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Asignado")
                Exit Sub
            End If
            '
            SaldoAnt = e.Row("Saldo")
            '
            e.Row("Saldo") = e.Row("Saldo") + e.Row("Asignado") - CDec(e.ProposedValue)
            '
            If e.Row("Saldo") < 0 Then
                MsgBox("Imputación Supera Saldo de la Factura.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Asignado")
                e.Row("Saldo") = SaldoAnt
                Exit Sub
            End If
        End If

        CalculaTotales()

    End Sub

   
    
 
   
End Class