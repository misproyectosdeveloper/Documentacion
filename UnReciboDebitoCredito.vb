Option Explicit On
Imports System.Transactions
Imports System.Drawing.Printing
Imports ClassPassWord
Public Class UnReciboDebitoCredito
    Public PNota As Double
    Public PAbierto As Boolean
    Public PTipoNota As Integer
    Public PEmisor As Integer
    Public PBloqueaFunciones As Boolean
    Public PImporte As Double
    Public PDiferenciaDeCambio As Boolean
    Public PImputa As Boolean
    Public PTr As Boolean
    Public PEsFCE As Boolean
    'Datos para rechazos de cheques.
    Public PClaveCheque As Integer
    Public PNumeroCheque As Decimal
    Public PImporteCheque As Decimal
    Public PEsRechazoCheque As Boolean
    Public PMedioPago As Integer
    '
    Public PConexion As String
    Public PConexionN As String
    '
    Dim DtNotaDetalle As DataTable
    Dim DtNotaCabeza As DataTable
    Dim DtMedioPago As DataTable
    Dim DtNotaLotes As DataTable
    Dim DtRecibosPercepciones As DataTable
    Dim DtChequesRechazados As DataTable
    '
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
    Dim ListaDePercepciones As New List(Of ItemIvaReten)
    Dim EmisorOpr As Boolean
    Dim DocTipo As Integer
    Dim DocNro As Decimal
    Dim TextoFijoParaFacturas1 As String
    Dim TextoFijoParaFacturas2 As String
    '
    Private MiEnlazador As New BindingSource
    '
    Dim Calle As String
    Dim Numero As Integer
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
    Dim EsFacturaElectronica As Boolean
    Dim FacturaAnteriorOK As Boolean
    Dim EsNotaInterna As Boolean
    Dim ReciboOficialAnt As Decimal
    Dim TotalPercepciones As Decimal
    Dim TotalNetoPerc As Decimal
    'Para FCE.
    Dim EsFCE As Boolean
    Dim FacturaFCE As Decimal
    'Variables Impresion. 
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim LineasParaImpresion As Integer = 0
    Dim LineasParaImpresionImputacion As Integer = 0
    Dim IgualRazonSocial As Integer = 0
    Dim CopiasSegunPuntoVenta As Integer = 0
    Dim UltimoPuntoVentaParaCopiaSeleccionado As Integer = 0
    'variables para imprimir retenciones manuales.
    Dim NumeroRetencion As Integer
    Dim NombreRetencion As String
    Dim ImporteRetencion As Decimal
    Private Sub UnaNotaTerceros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Conexion = "" Then Conexion = PConexion : ConexionN = PConexionN  'Si es llamado de otro proyecto. 

        EsFCE = PEsFCE

        Select Case PTipoNota
            Case 6, 8, 500, 700, 13006, 13008
                If Not PermisoEscritura(2) Then PBloqueaFunciones = True
            Case 5, 7, 50, 70, 13005, 13007
                If Not PermisoEscritura(6) Then PBloqueaFunciones = True
            Case Else
                MsgBox("Tipo Nota " & PTipoNota & " No Prevista.")
                Me.Close() : Exit Sub
        End Select

        If GCaja = 0 And PNota = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PNota <> 0 Then
            Select Case PTipoNota
                Case 6, 700
                    If PAbierto Then
                        If HallaFacturaDiferencia(PTipoNota, PNota) <> 0 Then
                            UnReciboDiferencia.PTipoNota = PTipoNota
                            UnReciboDiferencia.PNota = PNota
                            UnReciboDiferencia.PAbierto = True
                            UnReciboDiferencia.PBloqueaFunciones = PBloqueaFunciones
                            UnReciboDiferencia.ShowDialog()
                            Me.Close()
                            Exit Sub
                        End If
                    End If
            End Select
        End If

        GridCompro.AutoGenerateColumns = False
        GridCompro.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        If PNota = 0 And IgualRazonSocial = 0 Then
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
        End If

        IgualRazonSocial = 0

        If PAbierto Then
            ConexionNota = Conexion
        Else : ConexionNota = ConexionN
        End If

        Select Case PTipoNota
            Case 5, 7, 50, 70, 13005, 13007
                LlenaCombo(ComboEmisor, "", "Clientes") : Label16.Text = "Cliente"
            Case 6, 8, 500, 700, 13006, 13008
                LlenaCombo(ComboEmisor, "", "Proveedores") : Label16.Text = "Proveedor"
        End Select

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

        ArmaMedioPagoOtras(PTipoNota, DtFormasPago, PAbierto, PNota)

        If PTipoNota = 5 Then LabelTipoNota.Text = "Nota Debito Financiera a Cliente"
        If PTipoNota = 13005 Then LabelTipoNota.Text = "Nota Debito Interna a Cliente"
        If PTipoNota = 6 Then LabelTipoNota.Text = "Nota Debito Financiera a Proveedor"
        If PTipoNota = 13006 Then LabelTipoNota.Text = "Nota Debito Interna a Proveedor"
        If PTipoNota = 7 Then LabelTipoNota.Text = "Nota Crédito Financiera a Cliente"
        If PTipoNota = 13007 Then LabelTipoNota.Text = "Nota Crédito Interna a Cliente"
        If PTipoNota = 8 Then LabelTipoNota.Text = "Nota Crédito Financiera a Proveedor"
        If PTipoNota = 13008 Then LabelTipoNota.Text = "Nota Crédito Interna a Proveedor"
        If PTipoNota = 50 Then LabelTipoNota.Text = "Nota Debito del Cliente" : LabelReciboOficial.Text = "Nota Debito"
        If PTipoNota = 70 Then LabelTipoNota.Text = "Nota Crédito del Cliente" : LabelReciboOficial.Text = "Nota Crédito"
        If PTipoNota = 500 Then LabelTipoNota.Text = "Nota Debito del Proveedor" : LabelReciboOficial.Text = "Nota Debito"
        If PTipoNota = 700 Then LabelTipoNota.Text = "Nota Crédito del Proveedor" : LabelReciboOficial.Text = "Nota Crédito"

        GModificacionOk = False

        Select Case PTipoNota
            Case 13005, 13006, 13007, 13008
                EsNotaInterna = True
            Case Else
                ArmaListaDeRetenciones()
                EsNotaInterna = False
        End Select

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

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        LlenaCombosGrid()

        If PTipoNota = 70 Or PTipoNota = 700 Or PTipoNota = 50 Or PTipoNota = 500 Then
            Panel5.Visible = True
        End If

        Select Case PTipoNota
            Case 70, 700, 50, 500
                PanelFechaContable.Visible = True
        End Select

        If EsFacturaElectronica Then
            PanelFechaContable.Visible = True
            If PNota <> 0 Then
                If DtNotaCabeza.Rows(0).Item("Cae") <> 0 Then
                    PanelFechaContable.Enabled = False
                    DateTime1.Enabled = False
                Else
                    PanelFechaContable.Enabled = True
                    DateTime1.Enabled = True
                End If
            End If
        End If

        If PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 50 Or PTipoNota = 70 Then
            Me.BackColor = Color.LightGreen
        End If

        If PTipoNota = 6 Or PTipoNota = 8 Or PTipoNota = 500 Or PTipoNota = 700 Then
            Me.BackColor = Color.LightBlue
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

        Select Case PTipoNota
            Case 5, 7, 6, 8
            Case Else
                ButtonTextoRecibo.Visible = False
        End Select

        FacturaAnteriorOK = True
        If EsFacturaElectronica And PNota = 0 Then
            If Not VerificaRecursosAFIP("C:\XML Afip\") Then Me.Close() : Exit Sub
            UltimafechaContableW = UltimaFechacontableDebitoCredito(Conexion, GPuntoDeVenta, LetraIva, PTipoNota)
            If UltimafechaContableW = "2/1/1000" Then
                MsgBox("Error Base de Datos al Leer Ultima Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
            End If
            If LetraIva = 4 Then
                MsgBox("Punto de Venta " & Format(GPuntoDeVenta, "0000") & " NO debe estar definido como Electrónica para Comercio Exterior..", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
            End If
            If Not EsFacturaAnteriorOk(PTipoNota, UltimoNumero) Then
                FacturaAnteriorOK = False
                MsgBox("La Nota " & NumeroEditado(UltimoNumero - 1) & " No Tiene Autorización AFIP. Si continua, Afip Rechazara este Comprobante.")
            End If
        End If

        Select Case PTipoNota
            Case 5, 13005, 6, 13006, 7, 13007, 8, 13008
                UltimaFechaW = UltimaFechaLetraPuntoVenta(Conexion)
            Case 50, 70, 500, 700
                UltimaFechaW = UltimaFechaPuntoVentaTipoNota(Conexion)
        End Select

        Select Case PTipoNota
            Case 5, 7
                If PNota = 0 Then DateTime1.Enabled = True
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

        Dim RowsBusqueda() As DataRow

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtNotaLotesAux As DataTable = DtNotaLotes.Copy
        Dim DtRecibosPercepcionesAux As DataTable = DtRecibosPercepciones.Copy
        Dim DtChequesRechazadosAux As DataTable = DtChequesRechazados.Copy

        Dim DtFacturasCabezaAux As DataTable = DtFacturasCabeza.Copy
        Dim DtNVLPCabezaAux As DataTable = DtNVLPCabeza.Copy
        Dim DtLiquidacionCabezaAux As DataTable = DtLiquidacionCabeza.Copy
        Dim DtSenaAux As DataTable = DtSena.Copy
        Dim DtComprobantesCabezaAux As DataTable = DtComprobantesCabeza.Copy
        Dim DtSaldosInicialesAux As DataTable = DtSaldosIniciales.Copy

        'Actualiza Saldo de Comprobantes Imputados.
        ActualizaComprobantes("M", DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux)

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

        'Actualiza Archivo de Percepciones realizadas.
        TotalPercepciones = 0
        If PAbierto And PNota = 0 Then
            TotalPercepciones = CalculaPercepciones(ListaDePercepciones, TotalNetoPerc)
            ArmaRecibosPercepciones(PTipoNota, 0, ListaDePercepciones, DtRecibosPercepcionesAux)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PEsRechazoCheque And PNota = 0 Then
            DtChequesRechazadosAux.Rows(0).Item("Estado") = 4
            If PTipoNota = 5 Or PTipoNota = 6 Or PTipoNota = 13005 Or PTipoNota = 13006 Then
                DtChequesRechazadosAux.Rows(0).Item("TieneDebito") = True
            End If
        End If

        If IsNothing(DtNotaDetalleAux.GetChanges) And IsNothing(DtNotaCabezaAux.GetChanges) And IsNothing(DtNotaLotesAux.GetChanges) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GridCompro.Focus()
            Exit Sub
        End If

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If PNota = 0 Then
                If Not ArmaArchivosAsiento("A", DtAsientoCabezaAux, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            Else
                If Not HallaAsientosCabeza(TipoAsiento, DtNotaCabeza.Rows(0).Item("Nota"), DtAsientoCabezaAux, ConexionNota) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                If TextFechaContable.Visible Then
                    If DtAsientoCabezaAux.Rows(0).Item("IntFecha") <> Format(CDate(TextFechaContable.Text), "yyyyMMdd") Then
                        DtAsientoCabezaAux.Rows(0).Item("IntFecha") = Format(CDate(TextFechaContable.Text), "yyyyMMdd")
                    End If
                End If
            End If
        End If

        If PNota = 0 Then
            If HacerAlta(DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtNotaLotesAux, DtChequesRechazadosAux, DtRecibosPercepcionesAux) Then
                ArmaArchivos()
            End If
        Else
            If HacerModificacion(DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtNotaLotesAux, DtChequesRechazadosAux, DtRecibosPercepcionesAux) Then
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

        If CInt(LabelCaja.Text) <> GCaja Then
            MsgBox("Modificación Solo Permitida para Caja " & LabelCaja.Text & ". Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsFacturaElectronica Then
            MsgBox("Nota Electrónica No se Puede ANULAR. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
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

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtRecibosPercepcionesAux As DataTable = DtRecibosPercepciones.Copy
        Dim DtNotaLotesAux As DataTable = DtNotaLotes.Copy
        Dim DtChequesRechazadosAux As DataTable = DtChequesRechazados.Copy

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

        'Actualiza Saldo de Comprobantes Imputados.
        DtFacturasCabezaAux = DtFacturasCabeza.Copy
        DtNVLPCabezaAux = DtNVLPCabeza.Copy
        DtLiquidacionCabezaAux = DtLiquidacionCabeza.Copy
        DtSenaAux = DtSena.Copy
        DtComprobantesCabezaAux = DtComprobantesCabeza.Copy
        DtSaldosInicialesAux = DtSaldosIniciales.Copy

        ' Fue anulado por que se obliga a anular las imputaciones manualmente.
        '     ActualizaComprobantes("B", DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux)

        If PEsRechazoCheque Then
            Select Case PTipoNota
                Case 8, 7, 13008, 13007
                    If DtChequesRechazadosAux.Rows(0).Item("TieneDebito") Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Se Genero un Debito para este Cheque, debe anularlo previamente para continuar con la anulación de esta Nota. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    Else
                        DtChequesRechazadosAux.Rows(0).Item("Estado") = 1
                    End If
                Case 5, 6, 13005, 13006
                    DtChequesRechazadosAux.Rows(0).Item("Estado") = 1
                    DtChequesRechazadosAux.Rows(0).Item("TieneDebito") = False
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

        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        '
        If Not HallaAsientosCabeza(TipoAsiento, DtNotaCabeza.Rows(0).Item("Nota"), DtAsientoCabezaAux, ConexionNota) Then Exit Sub
        For Each Row As DataRow In DtAsientoCabezaAux.Rows
            Row("Estado") = 3
        Next

        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone

        DtNotaCabezaAux.Rows(0).Item("Estado") = 3

        Resul = ActualizaNota("B", DtGrid, DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtNotaLotesAux, DtChequesRechazadosAux, DtRecibosPercepcionesAux)
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

        If GCaja = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
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

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota Ya Esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If CInt(LabelCaja.Text) <> GCaja Then
            MsgBox("Modificación Solo Permitida para Caja " & LabelCaja.Text & ". Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsFacturaElectronica And DtNotaCabeza.Rows(0).Item("Cae") <> 0 Then
            MsgBox("Nota Electrónica No se Puede BORRAR pues Tiene Autorizacon en la AFIP. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
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

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtRecibosPercepcionesAux As DataTable = DtRecibosPercepciones.Copy
        Dim DtNotaLotesAux As DataTable = DtNotaLotes.Copy
        Dim DtChequesRechazadosAux As DataTable = DtChequesRechazados.Copy

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

        If PEsRechazoCheque Then
            Select Case PTipoNota
                Case 8, 7
                    If DtChequesRechazadosAux.Rows(0).Item("TieneDebito") Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Se Genero un Debito para este Cheque, debe anularlo previamente para continuar con la anulación de esta Nota. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    Else
                        DtChequesRechazadosAux.Rows(0).Item("Estado") = 1
                    End If
                Case 5, 6
                    DtChequesRechazadosAux.Rows(0).Item("Estado") = 1
                    DtChequesRechazadosAux.Rows(0).Item("TieneDebito") = False
            End Select
        End If

        If DtNotaCabezaAux.Rows(0).Item("Importe") <> DtNotaCabezaAux.Rows(0).Item("Saldo") Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Nota Tiene Imputaciones, Debe anularlas para Continuar. Operación se CANCELA.")
            Exit Sub
        End If

        If MsgBox("Nota se Borrara Definitivamente del Sistema. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

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
        For Each Row As DataRow In DtRecibosPercepcionesAux.Rows
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

        Resul = BorraNota(DtNotaCabezaAux, DtFacturasCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtNotaLotesAux, DtChequesRechazadosAux, DtPuntosDeVenta, ReciboOficial, DtRecibosPercepcionesAux)
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
    Private Sub ButtonComprobantesAImputar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonComprobantesAImputar.Click

        UNComprobanteAImputar.PDtGridCompro = DtGridCompro
        UNComprobanteAImputar.PTipo = PTipoNota
        UNComprobanteAImputar.PAbierto = PAbierto
        UNComprobanteAImputar.PTotalConceptos = TextTotalRecibo.Text
        UNComprobanteAImputar.PTipoIva = ComboTipoIva.SelectedValue
        UNComprobanteAImputar.PMoneda = ComboMoneda.SelectedValue
        UNComprobanteAImputar.PCambio = TextCambio.Text
        UNComprobanteAImputar.ShowDialog()
        DtGridCompro = UNComprobanteAImputar.PDtGridCompro
        UNComprobanteAImputar.Dispose()

        ArmaGridCompro()
        CalculaTotales()

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
        UnMediosPago.PlistaDePercepciones = ListaDePercepciones
        If DtNotaCabeza.Rows(0).Item("ChequeRechazado") <> 0 Then UnMediosPago.PEsChequeRechazado = True
        If ComboTipoIva.SelectedValue = Exterior Then UnMediosPago.PEsExterior = True
        UnMediosPago.PMoneda = ComboMoneda.SelectedValue
        UnMediosPago.PCambio = CDbl(TextCambio.Text)
        UnMediosPago.PDtRetencionesAutomaticas = DtRetencionesAutomaticas
        UnMediosPago.PDiferenciaDeCambio = PDiferenciaDeCambio
        UnMediosPago.PImporte = CDbl(TextTotalRecibo.Text) - TotalPercepciones
        UnMediosPago.ShowDialog()
        DtGrid = UnMediosPago.PDtGrid

        UnMediosPago.Dispose()

        CalculaTotales()

    End Sub
    Private Sub ButtonDatosCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDatosCliente.Click

        If ComboEmisor.SelectedValue = 0 Then Exit Sub

        Select Case PTipoNota
            Case 5, 7, 50, 70, 60, 65, 64
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
        CalculaTotales()

    End Sub
    Private Sub ButtonCompAsociado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCompAsociado.Click

        MsgBox(HallaCompAsociado(DtNotaCabeza.Rows(0).Item("TipoCompAsociado"), DtNotaCabeza.Rows(0).Item("CompAsociado")))

    End Sub
    Private Sub ButtonLotesAImputar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonLotesAImputar.Click

        If ComboNegocio.SelectedValue <> 0 Then
            MsgBox("No se puede Imputar Lotes Si se Asigna a Un Costeo.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If PTr Then
            MsgBox("Función No Habilitada.", MsgBoxStyle.Information)
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
    Private Sub ButtonTextoRecibo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTextoRecibo.Click

        UnTextoParaRecibo.TextoFijoParaFacturas1 = TextoFijoParaFacturas1
        UnTextoParaRecibo.TextoFijoParaFacturas2 = TextoFijoParaFacturas2
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

        Dim PuntoVentaW As Integer
        Select Case PTipoNota
            Case 5, 6, 7, 8, 13005, 13006, 13007, 13008
                PuntoVentaW = Val(Strings.Mid(DtNotaCabeza.Rows(0).Item("Nota"), 2, 4))
            Case Else
                PuntoVentaW = Val(Strings.Left(Format(DtNotaCabeza.Rows(0).Item("Nota"), "0000-00000000"), 4))
        End Select
        If PAbierto And (CopiasSegunPuntoVenta = 0 Or PuntoVentaW <> UltimoPuntoVentaParaCopiaSeleccionado) Then
            UltimoPuntoVentaParaCopiaSeleccionado = PuntoVentaW
            CopiasSegunPuntoVenta = TraeCopiasComprobante(3, PuntoVentaW)
            If CopiasSegunPuntoVenta < 0 Then CopiasSegunPuntoVenta = 0 : MsgBox("Error al Leer Tabla: PuntosDeVenta. Operacion se CANCELA.", MsgBoxStyle.Critical) : Exit Sub
        End If

        If PAbierto Then
            Copias = CopiasSegunPuntoVenta
        Else
            Copias = 1
        End If

        Dim print_document As New PrintDocument
        UnSeteoImpresora.SeteaImpresion(print_document)


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
    Private Sub ButtonPedirAutorizacionAfip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPedirAutorizacionAfip.Click

        If EsNotaInterna Then
            MsgBox("Opcion Invalida para Notas Internas. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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

        If PideAutorizacionAfip(DtNotaCabeza, DtRecibosPercepciones) Then   'Pide y graba CAE a la AFIP.----
            If Not ArmaArchivos() Then Me.Close() : Exit Sub
        End If

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PNota = 0
        PEmisor = 0

        UnaNotaTerceros_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonNuvoIgualEmisor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuvoIgualEmisor.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PNota = 0
        IgualRazonSocial = PEmisor

        UnaNotaTerceros_Load(Nothing, Nothing)

    End Sub
    Private Sub PictureLupaCuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupaCuenta.Click

        If PNota <> 0 Then Exit Sub

        If CDbl(TextTotalRecibo.Text) = 0 Then
            MsgBox("Falta Informar Conceptos.")
            Exit Sub
        End If

        Dim Neto As Decimal

        Select Case PTipoNota
            Case 6, 8, 500, 700, 5, 7, 50, 70
                For I As Integer = 0 To DtGrid.Rows.Count - 1
                    Dim Row As DataRow = DtGrid.Rows(I)
                    If Row("MedioPago") = 100 Then Neto = Neto + Row("Neto")
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
    Private Sub MaskedReciboOficial_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MaskedReciboOficial.Validating

        If Not MaskedOK(MaskedReciboOficial) Then
            MaskedReciboOficial.Text = "000000000000"
        End If

    End Sub
    Private Function ArmaArchivos() As Boolean                'ArmaArchivos

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

        If ComboTipoIva.SelectedValue = Exterior Then ArmaMedioPagoOtrasExterior(DtFormasPago, True)

        If ComboTipoIva.SelectedValue = Exterior Then
            Select Case PTipoNota
                Case 6, 8, 500, 700, 5, 7, 13005, 13006, 13007, 13008
                Case Else
                    MsgBox("Comprobante No Permitido Para Tipo Iva Exportación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
            End Select
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

        ReciboOficialAnt = DtNotaCabeza.Rows(0).Item("ReciboOficial")
        EsFacturaElectronica = DtNotaCabeza.Rows(0).Item("EsElectronica")
        PMedioPago = DtNotaCabeza.Rows(0).Item("MedioPagoRechazado")
        PTr = DtNotaCabeza.Rows(0).Item("Tr")
        PDiferenciaDeCambio = DtNotaCabeza.Rows(0).Item("DiferenciaDeCambio")

        If Not EmisorOpr And PAbierto And PNota = 0 Then
            If EsFacturaElectronica Then
                MsgBox("Cliente/Proveedor Solo Opera a Candado Cerrado. Debe Cambiar Candado en Cliente/Proveedor. Candado Abierto No Permitido para Notas Electrónica.", MsgBoxStyle.Critical) : Return False
            End If
            If MsgBox("Cliente/Proveedor Solo Opera a Candado Cerrado. Debe Cambiar Candado en Cliente/Proveedor. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
        End If

        If PTr And PermisoTotal Then
            LabelTr.Visible = True
        End If

        If PNota <> 0 Then
            PEmisor = DtNotaCabeza.Rows(0).Item("Emisor")
            PClaveCheque = DtNotaCabeza.Rows(0).Item("ChequeRechazado")
            If PClaveCheque <> 0 Then
                PEsRechazoCheque = True
            Else : PEsRechazoCheque = False
            End If
        End If

        DtChequesRechazados = New DataTable
        If PEsRechazoCheque Then
            If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & PMedioPago & " AND ClaveCheque = " & PClaveCheque & ";", ConexionNota, DtChequesRechazados) Then Me.Close() : Exit Function
        End If

        'Arma Lista de Percpciones/Retenciones de terceros --------------------------------------- 
        DtRetencionProvincia = New DataTable
        If PAbierto Then
            If Not Tablas.Read("SELECT * FROM RecibosRetenciones WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";", Conexion, DtRetencionProvincia) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
            For Each Row As DataRow In DtRetencionProvincia.Rows
                Row1 = DtRetencionProvinciaAux.NewRow
                Row1("Retencion") = Row("Retencion")
                Row1("Provincia") = Row("Provincia")
                Row1("Comprobante") = Row("Comprobante")
                Row1("Importe") = Row("Importe")
                DtRetencionProvinciaAux.Rows.Add(Row1)
            Next
        End If

        'Arma Lista de Percpciones realizadas.--------------------------------------- 
        DtRecibosPercepciones = New DataTable
        TotalPercepciones = 0
        Select Case PTipoNota
            Case 5, 6, 7, 8
                If ComboTipoIva.SelectedValue <> Exterior And PAbierto And Not PEsRechazoCheque Then
                    If PNota = 0 Then
                        Sql = "SELECT * FROM RecibosPercepciones WHERE TipoComprobante = 0 AND Comprobante = " & 0 & ";"
                        If Not Tablas.Read(Sql, Conexion, DtRecibosPercepciones) Then Return False
                        AgregalistaPercepciones()
                    End If
                    If PNota <> 0 Then
                        Sql = "SELECT * FROM RecibosPercepciones WHERE TipoComprobante = " & PTipoNota & " AND Comprobante = " & PNota & ";"
                        If Not Tablas.Read(Sql, Conexion, DtRecibosPercepciones) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
                        For Each Row2 As DataRow In DtRecibosPercepciones.Rows
                            TotalPercepciones = TotalPercepciones + CDec(Row2("Importe"))
                        Next
                    End If
                End If
        End Select

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
            Row1("Banco") = Row("Banco")
            Row1("Cuenta") = Row("Cuenta")
            Row1("Serie") = ""
            Row1("Numero") = 0
            Row1("Fecha") = "1/1/1800"
            Row1("TieneLupa") = False
            Row1("eCheq") = False
            If PAbierto Then
                RowsBusqueda = DtRetencionProvincia.Select("Retencion = " & Row("MedioPago"))
                If RowsBusqueda.Length <> 0 Then Row1("TieneLupa") = True
            End If
            DtGrid.Rows.Add(Row1)
        Next

        If PNota = 0 And PEsRechazoCheque Then
            Dim Row As DataRow = DtGrid.NewRow
            Row("Item") = 0
            Row("MedioPago") = 100
            Row("Importe") = PImporteCheque
            Row("Cambio") = 0
            If PMedioPago = 2 Then
                Row("Detalle") = "Rechazo Cheque " & FormatNumber(PNumeroCheque, 0)
            End If
            If PMedioPago = 3 Then
                Row("Detalle") = "Rechazo Cheque " & FormatNumber(PClaveCheque, 0)
            End If
            Row("Alicuota") = 0
            Row("Neto") = PImporteCheque
            Row("ImporteIva") = 0
            Row("Comprobante") = 0
            Row("FechaComprobante") = "01/01/1800"
            Row("ClaveCheque") = 0
            Row("ClaveInterna") = 0
            Row("Banco") = 0
            Row("Cuenta") = 0
            Row("Bultos") = 0
            Row("Serie") = ""
            Row("Numero") = 0
            Row("Fecha") = "1/1/1800"
            Row("TieneLupa") = False
            Row("eCheq") = False
            DtGrid.Rows.Add(Row)
            ButtonNuevo.Enabled = False
        End If

        'Muestra Comprobantes a Imputar.
        DtFacturasCabeza = New DataTable
        DtNVLPCabeza = New DataTable
        DtComprobantesCabeza = New DataTable
        DtLiquidacionCabeza = New DataTable
        DtSena = New DataTable
        DtSaldosIniciales = New DataTable

        If (PTipoNota = 7 Or PTipoNota = 13007) And Not PDiferenciaDeCambio Then      'N.Credito Financiera a Cliente.
            If Not ArmaConFacturas(PTipoNota) Then Return False
            If Not ArmaConSaldosIniciales(PTipoNota) Then Return False
            If Not ArmaConNVLP() Then Return False
            If Not ArmaConNotas(64) Then Return False 'Devolucións a Clientes. 
            If Not ArmaConNotas(5) Then Return False 'N.Debito Financiera a Clientes.     
            If Not ArmaConNotas(13005) Then Return False 'N.Debito Financiera a Clientes interna.     
            If Not ArmaConNotas(70) Then Return False 'N.Credito Financiera del Clientes.     
        End If
        If PTipoNota = 50 Then                                                         'N.Debito Financiera del Cliente.
            If Not ArmaConFacturas(PTipoNota) Then Return False
            If Not ArmaConSaldosIniciales(PTipoNota) Then Return False
            If Not ArmaConNVLP() Then Return False
            If Not ArmaConNotas(5) Then Return False 'N.Debito Financiera a Cliente. 
            If Not ArmaConNotas(13005) Then Return False 'N.Debito Financiera a Cliente interna. 
            If Not ArmaConNotas(64) Then Return False 'Devolucións a Clientes.     
            If Not ArmaConNotas(70) Then Return False 'N.Credito del Clientes.     
        End If
        If (PTipoNota = 6 Or PTipoNota = 13006) And Not PDiferenciaDeCambio Then       'N.Debito Financiera a Proveedor.
            If Not ArmaConFacturas(PTipoNota) Then Return False
            If Not ArmaConSaldosIniciales(PTipoNota) Then Return False
            If Not ArmaConLiquidaciones() Then Return False
            If Not ArmaConNotas(604) Then Return False 'Devolucións del Proveedor,     
            If Not ArmaConNotas(500) Then Return False 'N.Debito del Proveedor.
            If Not ArmaConNotas(8) Then Return False 'N.Credito a Proveedor.     
            If Not ArmaConNotas(13008) Then Return False 'N.Credito a Proveedor interna.     
        End If
        If PTipoNota = 700 Then                                                        'N.Credito Financiera del Proveedor.
            If Not ArmaConFacturas(PTipoNota) Then Return False
            If Not ArmaConSaldosIniciales(PTipoNota) Then Return False
            If Not ArmaConLiquidaciones() Then Return False
            If Not ArmaConNotas(500) Then Return False 'N.Debito Financiera del Proveedor. 
            If Not ArmaConNotas(604) Then Return False 'Devolucións del Proveedor     
            If Not ArmaConNotas(8) Then Return False 'N.Credito a Proveedor.     
            If Not ArmaConNotas(13008) Then Return False 'N.Credito a Proveedor interna.     
        End If

        Select Case PTipoNota
            Case 5, 13005, 8, 13008, 70, 500
                ButtonComprobantesAImputar.Visible = False
                GridCompro.Visible = False
                Panel3.Visible = False
        End Select
        If PDiferenciaDeCambio Then
            ButtonComprobantesAImputar.Visible = False
            GridCompro.Visible = False
            Panel3.Visible = False
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
            Row1("TipoVisible") = 2
            If Row("Esz") Then Row1("TipoVisible") = 44
            Row1("Comprobante") = Row("Factura")
            If PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 13005 Or PTipoNota = 13007 Or PTipoNota = 50 Or PTipoNota = 70 Or PTipoNota = 60 Then
                Row1("Recibo") = Row("Factura")
                Row1("Fecha") = Row("Fecha")
            Else
                Row1("Recibo") = Row("ReciboOficial")
                Row1("Fecha") = Row("FechaFactura")
            End If
            Row1("Comentario") = Row("Comentario")
            Row1("Importe") = Row("Importe")
            Select Case PTipoNota
                Case 5, 7
                    Row1("Importe") = Row("Importe") + Row("Percepciones")
            End Select
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

        TipoAsiento = PTipoNota
        If (PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 13005 Or PTipoNota = 13007) And PEsRechazoCheque <> 0 Then TipoAsiento = 7001
        If (PTipoNota = 8 Or PTipoNota = 13008) And PEsRechazoCheque <> 0 Then TipoAsiento = 7002
        If PTipoNota = 5 And DtNotaCabeza.Rows(0).Item("EsExterior") Then TipoAsiento = 10005
        If PTipoNota = 5 And DtNotaCabeza.Rows(0).Item("DiferenciaDeCambio") Then TipoAsiento = 11005
        If PTipoNota = 7 And DtNotaCabeza.Rows(0).Item("EsExterior") Then TipoAsiento = 10007
        If PTipoNota = 7 And DtNotaCabeza.Rows(0).Item("DiferenciaDeCambio") Then TipoAsiento = 11007
        If PTipoNota = 6 And DtNotaCabeza.Rows(0).Item("EsExterior") Then TipoAsiento = 10006
        If PTipoNota = 6 And DtNotaCabeza.Rows(0).Item("DiferenciaDeCambio") Then TipoAsiento = 11006
        If (PTipoNota = 6 Or PTipoNota = 13006) And PEsRechazoCheque <> 0 Then TipoAsiento = 7002
        If PTipoNota = 8 And DtNotaCabeza.Rows(0).Item("EsExterior") Then TipoAsiento = 10008
        If PTipoNota = 8 And DtNotaCabeza.Rows(0).Item("DiferenciaDeCambio") Then TipoAsiento = 11008
        If PTipoNota = 700 And DtNotaCabeza.Rows(0).Item("Tr") Then TipoAsiento = 11009
        If PTipoNota = 7 And DtNotaCabeza.Rows(0).Item("Tr") Then TipoAsiento = 11010

        LabelInterno.Text = "Nro. Interno"

        If (PTipoNota = 5 Or PTipoNota = 7) And ComboTipoIva.SelectedValue = Exterior Then
            Label16.Text = "Cliente Facturación"
            ComboClienteOperacion.Enabled = True
        Else
            ComboClienteOperacion.Enabled = False
        End If

        Select Case PTipoNota
            Case 6, 8, 500, 700, 13006, 13008
            Case Else
                ComboNegocio.Enabled = False
                ComboCosteo.Enabled = False
        End Select

        If PNota <> 0 Then
            PanelMoneda.Enabled = False
            PictureLupaCuenta.Enabled = False
            ButtonAceptar.Text = "Modificar Recibo"
            LabelPuntoDeVenta.Visible = False
        Else
            PanelMoneda.Enabled = True
            PictureLupaCuenta.Enabled = True
            ButtonAceptar.Text = "Grabar Recibo"
            LabelPuntoDeVenta.Visible = True
        End If

        If DtNotaCabeza.Rows(0).Item("Estado") = 3 Then
            GridCompro.ReadOnly = True
        Else : GridCompro.ReadOnly = False
        End If

        CalculaTotales()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If DtNotaCabeza.Rows(0).Item("NotaAnulacion") <> 0 Then
            Label4.Text = "Nota Anulada Electrónicamente por la Nota " & NumeroEditado(DtNotaCabeza.Rows(0).Item("NotaAnulacion"))
            Label4.Visible = True
            ButtonLotesAImputar.Enabled = False
            GridCompro.ReadOnly = True
        Else
            Label4.Visible = False
            ButtonLotesAImputar.Enabled = True
            GridCompro.ReadOnly = False
        End If

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
        If PTr Then SqlTr = " AND Tr = 1"

        Dim SqlESFCE As String = ""
        If TipoNota = 50 Then
            '   SqlESFCE = " AND EsFCE = 0"    Es para que nomuestre facturas de credito. Anulada hasta nuevo aviso,
        End If

        '------------------------------------------------------------------------------------------------------------
        'ClienteOperacion = 0 para que no aparezcan las generadas en el modulo de exportacion.-----------------------
        '------------------------------------------------------------------------------------------------------------

        If TipoNota = 5 Or TipoNota = 7 Or TipoNota = 13005 Or TipoNota = 13007 Or TipoNota = 50 Or TipoNota = 70 Or TipoNota = 60 Then
            Sql = "SELECT * FROM FacturasCabeza WHERE ClienteOperacion = 0 AND EsZ = 0 AND Estado <> 3 AND FacturasCabeza.Cliente = " & Emisor & SqlTr & SqlESFCE & " ORDER BY Factura,Fecha;"
        Else
            Sql = "SELECT *,0 As EsZ FROM FacturasProveedorCabeza WHERE Rendicion = 0 AND Estado = 1 AND Liquidacion = 0 AND Proveedor = " & Emisor & SqlTr & " ORDER BY Factura,Fecha;"
        End If

        If Not Tablas.Read(Sql, ConexionNota, DtFacturasCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConNVLP() As Boolean

        Dim Sql As String = ""

        Dim SqlTr As String = " AND Tr = 0"
        If PTr Then SqlTr = " AND Tr = 1"

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
        If PTr Then SqlTr = " AND Tr = 1"

        Dim SqlESFCE As String = ""
        If PTipoNota = 50 And TipoNota = 5 Then
            '    SqlESFCE = " AND EsFCE = 0"    Es para que no muestre N.Debito de credito  a Cliente. Anulada hasta nuevo aviso. 
        End If

        Sql = "SELECT * FROM RecibosCabeza WHERE Estado = 1 AND TipoNota = " & TipoNota & " AND Emisor = " & Emisor & SqlTr & SqlESFCE & " ORDER BY Nota,Fecha;"
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
        If PTr Then SqlTr = " AND Tr = 1"

        Sql = "SELECT * FROM LiquidacionCabeza WHERE Estado = 1 AND LiquidacionCabeza.Proveedor = " & Emisor & SqlTr & " ORDER BY Liquidacion,Fecha;"

        If Not Tablas.Read(Sql, ConexionNota, DtLiquidacionCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConSaldosIniciales(ByVal TipoNota As Integer) As Boolean

        Dim Sql As String = ""

        If TipoNota = 5 Or TipoNota = 7 Or TipoNota = 13005 Or TipoNota = 13007 Or TipoNota = 50 Or TipoNota = 70 Or TipoNota = 60 Then
            Sql = "SELECT * FROM SaldosInicialesCabeza WHERE Tipo = 3 AND Importe > 0 AND Emisor = " & PEmisor & ";"
        Else
            Sql = "SELECT * FROM SaldosInicialesCabeza WHERE Tipo = 2 AND Importe < 0 AND Emisor = " & PEmisor & ";"
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

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalRecibo.DataBindings.Clear()
        TextTotalRecibo.DataBindings.Add(Enlace)

        ''''  '     Enlace = New Binding("SelectedValue", MiEnlazador, "CodigoIva")
        '''     ComboIva.DataBindings.Clear()
        ''''  ComboIva.DataBindings.Add(Enlace)

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

        If Row("FechaReciboOficial") = "01/01/1800" Then
            TextFechaReciboOficial.Text = ""
        Else
            TextFechaReciboOficial.Text = Format(DtNotaCabeza.Rows(0).Item("FechaReciboOficial"), "dd/MM/yyyy")
        End If

        If PNota = 0 Then
            If TextFechaContable.Visible Then
                TextFechaContable.Text = ""
            End If
        Else
            TextFechaContable.Text = Format(DtNotaCabeza.Rows(0).Item("FechaContable"), "dd/MM/yyyy")
        End If

        FechaAnt = Row("fecha")

        If Row("Cae") <> 0 Then
            LabelCAE.Text = "Autorización AFIP  CAE: " & Row("Cae") & "  Vto: " & Strings.Right(Row("FechaCae"), 2) & "/" & Strings.Mid(Row("FechaCae"), 5, 2) & "/" & Strings.Left(Row("FechaCae"), 4) : LabelCAE.Visible = True
        Else
            LabelCAE.Visible = False
        End If

        If Row("EsFCE") Then
            LabelFCE.Text = "Factura de Crédito MiPyMEs(FCE)"
            EsFCE = True
        Else
            LabelFCE.Text = ""
            EsFCE = False
        End If

    End Sub
    Private Sub FormatMaskedNota(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Select Case PTipoNota
            Case 5, 6, 7, 8, 13005, 13006, 13007, 13008
                If Numero.Value <> 0 Then
                    TextLetra.Text = LetraTipoIva(Val(Strings.Left(Numero.Value.ToString, 1)))
                    LetraIva = HallaNumeroLetra(TextLetra.Text)
                    Numero.Value = Format(Val(Strings.Right(Numero.Value.ToString, 12)), "000000000000")
                Else
                    Numero.Value = Format(Numero.Value, "000000000000")
                End If
            Case Else
                Numero.Value = Format(Numero.Value, "000000000000")
        End Select

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
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        If PEsRechazoCheque And Funcion = "A" Then
            Return ArmaArchivosAsientoPorRechazo(Funcion, DtCabeza, DtDetalle)
        End If
        If PEsRechazoCheque And Funcion = "M" Then
            Return True
        End If

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos
        Dim RowsBusqueda() As DataRow

        If Funcion = "A" Then
            Select Case PTipoNota
                Case 5, 13005, 7, 13007, 50, 70
                    Dim Neto As Decimal
                    Dim Total As Decimal
                    For Each Row As DataRow In DtGrid.Rows
                        If Row("MedioPago") = 100 Then Neto = Neto + Row("Neto")
                        Total = Total + Row("Importe")
                    Next
                    If Not PDiferenciaDeCambio Then
                        For Each Row As DataRow In DtGrid.Rows
                            If Row("ImporteIva") <> 0 Then
                                Item = New ItemListaConceptosAsientos
                                Item.Clave = HallaClaveIva(Row("Alicuota"))
                                Item.Importe = Row("ImporteIva")
                                Item.TipoIva = 6
                                ListaIVA.Add(Item)
                            End If
                        Next
                        For Each Row As DataRow In DtGrid.Rows
                            RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                            If RowsBusqueda(0).Item("Tipo") = 4 Then
                                Item = New ItemListaConceptosAsientos
                                Item.Clave = Row("MedioPago")
                                Item.Importe = Row("Importe")
                                If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
                                Item.TipoIva = 9        'Credito fiscal.
                                If PTipoNota = 5 Then
                                    Item.TipoIva = 9        'Credito fiscal.
                                End If
                                If PTipoNota = 7 Then
                                    Item.TipoIva = 11        'Debito fiscal.
                                End If
                                ListaRetenciones.Add(Item)
                            End If
                        Next
                        Item = New ItemListaConceptosAsientos
                        Item.Clave = 202
                        Item.Importe = Neto
                        If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
                        ListaConceptos.Add(Item)
                    End If
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = 213
                    Item.Importe = Total
                    If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
                    ListaConceptos.Add(Item)
                Case 6, 13006, 8, 13008, 500, 700
                    Dim Neto As Decimal
                    Dim Total As Decimal
                    For Each Row As DataRow In DtGrid.Rows
                        If Row("MedioPago") = 100 Then Neto = Neto + Row("Neto")
                        Total = Total + Row("Importe")
                    Next
                    If Not PDiferenciaDeCambio Then
                        For Each Row As DataRow In DtGrid.Rows
                            If Row("ImporteIva") <> 0 Then
                                Item = New ItemListaConceptosAsientos
                                Item.Clave = HallaClaveIva(Row("Alicuota"))
                                Item.Importe = Row("ImporteIva")
                                Item.TipoIva = 5
                                ListaIVA.Add(Item)
                            End If
                        Next
                        For Each Row As DataRow In DtGrid.Rows
                            RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                            If RowsBusqueda(0).Item("Tipo") = 4 Then
                                Item = New ItemListaConceptosAsientos
                                Item.Clave = Row("MedioPago")
                                Item.Importe = Row("Importe")
                                If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
                                Item.TipoIva = 11       'Debito fiscal.
                                If PTipoNota = 6 Then
                                    Item.TipoIva = 9        'Credito fiscal.
                                End If
                                If PTipoNota = 8 Then
                                    Item.TipoIva = 11        'Debito fiscal.
                                End If
                                ListaRetenciones.Add(Item)
                            End If
                        Next
                        Item = New ItemListaConceptosAsientos
                        Item.Clave = 202
                        Item.Importe = Neto
                        If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
                        ListaConceptos.Add(Item)
                    End If
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = 213
                    Item.Importe = Total
                    If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
                    ListaConceptos.Add(Item)
            End Select

            'Arma Lista con Cuentas definidas en documento.
            If ListCuentas.Visible Then
                Dim Neto As Decimal = 0
                For Each Row As DataRow In DtGrid.Rows
                    If Row("MedioPago") = 100 Then Neto = Trunca(Neto + Row("Neto"))
                Next
                If ComboMoneda.SelectedValue <> 1 Then Neto = Trunca(CDec(TextCambio.Text) * Neto)
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
                        Fila.Clave = 202
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

        If MontoAfectado = 0 And Funcion = "M" Then Return True

        Dim Fecha As Date
        If PanelFechaContable.Visible Then
            Fecha = CDate(TextFechaContable.Text)
        Else : Fecha = DateTime1.Value
        End If

        If Not Asiento(TipoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Fecha, PTipoNota) Then Return False

        Return True

    End Function
    Private Function ArmaArchivosAsientoPorRechazo(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos

        If Funcion = "M" Then Return True

        If Funcion = "A" Then
            Dim Total As Double = PImporteCheque
            If PTipoNota = 7 Or PTipoNota = 13007 Then
                Total = -Total
            End If
            If PTipoNota = 6 Or PTipoNota = 13006 Then
                Total = -Total
            End If
            If PTipoNota = 7005 Then
                Total = -Total
            End If
            Item = New ItemListaConceptosAsientos
            If PMedioPago = 3 Then
                Item.Clave = 213
            Else
                Item.Clave = -501
            End If
            Item.Importe = Total
            ListaConceptos.Add(Item)
        End If

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
        Row("CodigoIva") = 0
        Row("Estado") = 1
        Row("Caja") = GCaja
        Row("Tr") = PTr
        Row("EsFCE") = EsFCE
        'Si es FCE y Alta lleno compasociado con numero factura. 
        If EsFCE Then
            Row("TipoCompAsociado") = 1
            Row("CompAsociado") = FacturaFCE
        End If
        '---------------------------------
        Row("Moneda") = ComboMoneda.SelectedValue
        If ComboMoneda.SelectedValue = 1 Then
            Row("Cambio") = 1
        Else : Row("Cambio") = 0
        End If
        Row("Manual") = False
        Row("RetencionManual") = False
        Row("DiferenciaDeCambio") = PDiferenciaDeCambio
        If ComboTipoIva.SelectedValue = Exterior Then Row("EsExterior") = True
        If PEsRechazoCheque Then
            Row("Importe") = PImporteCheque
            Row("MedioPagoRechazado") = PMedioPago
            Row("ChequeRechazado") = PClaveCheque
            If PMedioPago = 2 Then
                Row("Comentario") = "Rechazo Cheque " & FormatNumber(PNumeroCheque, 0)
            End If
            If PMedioPago = 3 Then
                Row("Comentario") = "Rechazo Cheque " & FormatNumber(PClaveCheque, 0)
            End If
        End If
        If PAbierto Then
            Select Case PTipoNota
                Case 5, 7, 6, 8
                    Row("EsElectronica") = EsPuntoDeVentaFacturasElectronicas(GPuntoDeVenta)
            End Select
        End If
        DtNotaCabeza.Rows.Add(Row)

    End Sub
    Private Sub ProrroteaImportesLotes(ByVal DtNotaLotesAux As DataTable)

        If DtGridLotes.Rows.Count = 0 Then Exit Sub

        Dim Cantidad As Decimal = 0

        For Each Row As DataRow In DtGridLotes.Rows
            Cantidad = Cantidad + Row("Cantidad")
        Next

        Dim ImporteConIva As Decimal = 0
        Dim ImporteSinIva As Decimal = 0

        If DtNotaCabeza.Rows(0).Item("ACuenta") <> 0 Then
            ImporteConIva = CalculaNeto(CDec(TextTotalRecibo.Text) - TotalPercepciones, CDec(TextCambio.Text))
            ImporteSinIva = CalculaNeto(CDec(TextTotalRecibo.Text) - TotalPercepciones, CDec(TextCambio.Text))
        Else
            For Each Row As DataRow In DtGrid.Rows
                ImporteConIva = CalculaNeto(CDec(TextTotalRecibo.Text) - TotalPercepciones, CDec(TextCambio.Text))
                If Row("MedioPago") = 100 Then
                    ImporteSinIva = ImporteSinIva + CalculaNeto(Row("Neto"), CDec(TextCambio.Text))
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
        DocTipo = 0
        DocNro = 0

        If PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 13005 Or PTipoNota = 13007 Or PTipoNota = 50 Or PTipoNota = 70 Or PTipoNota = 60 Or PTipoNota = 65 Or PTipoNota = 64 Then
            Sql = "SELECT * FROM Clientes WHERE Clave = " & Cliente & ";"
            Dta = Tablas.Leer(Sql)
            If Dta.Rows.Count = 0 Then
                MsgBox("ERROR, Cliente No Existe o Error en la Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            ComboMoneda.SelectedValue = Dta.Rows(0).Item("Moneda")
            MonedaEmisor = Dta.Rows(0).Item("Moneda")
            DocTipo = Dta.Rows(0).Item("DocumentoTipo")
            DocNro = Dta.Rows(0).Item("DocumentoNumero")
            TextoFijoParaFacturas1 = Dta.Rows(0).Item("TextoFijoParaFacturas1")
            TextoFijoParaFacturas2 = Dta.Rows(0).Item("TextoFijoParaFacturas2")
        Else
            Sql = "SELECT * FROM Proveedores WHERE Clave = " & Cliente & ";"
            Dta = Tablas.Leer(Sql)
            If Dta.Rows.Count = 0 Then
                MsgBox("ERROR, Proveedor No Existe o Error en la Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            ComboMoneda.SelectedValue = Dta.Rows(0).Item("Moneda")
            MonedaEmisor = Dta.Rows(0).Item("Moneda")
        End If

        Calle = Dta.Rows(0).Item("Calle")
        Numero = Dta.Rows(0).Item("Numero")
        Localidad = Dta.Rows(0).Item("Localidad")
        EmisorOpr = Dta.Rows(0).Item("Opr")
        Provincia = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        Cuit = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")

        Dta.Dispose()

        Return True

    End Function
    Private Function HacerAlta(ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtSaldosInicialesAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable, ByVal DtNotaLotesAux As DataTable, ByVal DtChequesRechazadosAux As DataTable, ByVal DtRecibosPercepcionesAux As DataTable) As Boolean

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
            If (PTipoNota = 13005 Or PTipoNota = 13006 Or PTipoNota = 13007 Or PTipoNota = 13008) Then NumeroNota = UltimaNumeracionDebitoCredito(ConexionNota)
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
            For Each Row As DataRow In DtRecibosPercepcionesAux.Rows
                Row("Comprobante") = NumeroNota
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

            NumeroW = ActualizaNota("A", DtGridAux, DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtNotaLotesAux, DtChequesRechazadosAux, DtRecibosPercepcionesAux)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -10 Then
            MsgBox("Recibo Ya Fue Grabado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
            PideAutorizacionAfip(DtNotaCabezaAux, DtRecibosPercepcionesAux)
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PNota = NumeroNota
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function HacerModificacion(ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtSaldosInicialesAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable, ByVal DtNotaLotesAux As DataTable, ByVal DtChequesRechazadosAux As DataTable, ByVal DtRecibosPercepcionesAux As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroAsiento As Double
        Dim Resul As Double

        For i As Integer = 1 To 50

            Resul = ActualizaNota("M", DtGrid, DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtNotaLotesAux, DtChequesRechazadosAux, DtRecibosPercepcionesAux)

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
    Private Function ActualizaNota(ByVal Funcion As String, ByVal DtGridAux As DataTable, ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtSaldosInicialesAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable, ByVal DtNotaLotesAux As DataTable, ByVal DtChequesRechazadosAux As DataTable, ByVal DtRecibosPercepcionesAux As DataTable) As Double

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
                If Not IsNothing(DtRecibosPercepcionesAux.GetChanges) Then
                    Resul = GrabaTabla(DtRecibosPercepcionesAux.GetChanges, "RecibosPercepciones", ConexionNota)
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
                If Not IsNothing(DtChequesRechazadosAux.GetChanges) Then
                    Resul = GrabaTabla(DtChequesRechazadosAux.GetChanges, "Cheques", ConexionNota)
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
    Private Function BorraNota(ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtNotaLotesAux As DataTable, ByVal DtChequesRechazadosAux As DataTable, ByVal DtPuntosDeVenta As DataTable, ByVal ReciboOficial As Decimal, ByVal DtRecibosPercepcionesAux As DataTable) As Double

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

                If Not IsNothing(DtRecibosPercepcionesAux.GetChanges) Then
                    Resul = GrabaTabla(DtRecibosPercepcionesAux.GetChanges, "RecibosPercepciones", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtNotaLotesAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaLotesAux.GetChanges, "RecibosLotes", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtChequesRechazadosAux.GetChanges) Then
                    Resul = GrabaTabla(DtChequesRechazadosAux.GetChanges, "Cheques", ConexionNota)
                    If Resul <= 0 Then Return Resul
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
    Private Function UltimaNumeracionDebitoCredito(ByVal ConexionStr As String) As Double

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
    Private Function PideAutorizacionAfip(ByVal DtCabezaW As DataTable, ByVal DtRecibosPercepcionesW As DataTable) As Boolean

        If Not PAbierto Then Return True
        If Not DtCabezaW.Rows(0).Item("EsElectronica") Then Return True
        If EsNotaInterna Then Return True

        Dim CAE As String = ""
        Dim FechaCae As String = ""
        Dim Concepto As Integer = 0
        Dim FchServDesde As Date = DateAdd(DateInterval.Day, 30, CDate(TextFechaContable.Text))
        Dim FchServHasta As Date = DateAdd(DateInterval.Day, 30, CDate(TextFechaContable.Text))
        Dim FchVtoPago As Date = DateAdd(DateInterval.Day, 30, CDate(TextFechaContable.Text))
        Concepto = 1
        Dim CbteTipoAsociado As Integer = 0
        Dim CbteAsociado As Decimal = 0
        Dim CancelarGrabar As Boolean

        Dim DatosParaAfip As New ItemDatosParaAFIP   'Si es FCE pone datos de la Factura FCE.
        DatosParaAfip.InscripcionAfip = ComboTipoIva.SelectedValue     'Responsable insc, montributo, etc.
        DatosParaAfip.DocTipo = DocTipo    'Tipo Documento Consumidor Final.
        DatosParaAfip.DocNro = DocNro      'Numero Documento Consumidor Final.

        If EsFCE Then
            Dim NumeroLetra As String
            Dim Nro As String
            Dim PtoVta As String
            DescomponeNumeroComprobante(DtCabezaW.Rows(0).Item("CompAsociado"), NumeroLetra, PtoVta, Nro)
            DatosParaAfip.EsFCE = EsFCE
            DatosParaAfip.Tipo = 1
            DatosParaAfip.TipoIva = NumeroLetra
            DatosParaAfip.NroCbte = Nro
            DatosParaAfip.PtoVta = PtoVta
            DatosParaAfip.Cbte = DtCabezaW.Rows(0).Item("CompAsociado")
            DatosParaAfip.Cuit = CuitNumerico(GCuitEmpresa)
            HallaFechaContableFactura(DtCabezaW.Rows(0).Item("CompAsociado"), Concepto, DatosParaAfip.FechaCbte)
            DatosParaAfip.EsAnulacion = "N"   'para anular FCE rechazada por el cliente.
        End If

        Dim Mensaje As String = Autorizar("CD", DtCabezaW, DtGrid, DtRecibosPercepcionesW, FchServDesde, FchServHasta, FchVtoPago, LetraIva, DtCabezaW.Rows(0).Item("Nota"), Cuit, Concepto, 0, CAE, FechaCae, CbteTipoAsociado, CbteAsociado, CancelarGrabar, DatosParaAfip)
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
    Private Sub HallaFechaContableFactura(ByVal Factura As Decimal, ByRef Concepto As Integer, ByRef FechaCbte As Date)

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT FechaContable,EsServicios FROM FacturasCabeza WHERE Factura = " & Factura & ";", Conexion, Dt) Then End
        If Dt.Rows(0).Item("EsServicios") Then
            Concepto = 2
        Else
            Concepto = 1
        End If
        FechaCbte = Dt.Rows(0).Item("FechaContable")

        Dt.Dispose()

    End Sub
    Private Sub ArmaListaDeRetenciones()

        DtRetencionesAutomaticas = New DataTable

    End Sub
    Private Sub PideDatosEmisor()

        If PTipoNota = 50 Or PTipoNota = 70 Or PTipoNota = 500 Or PTipoNota = 700 Then
            If PTipoNota = 50 Or PTipoNota = 70 Then
                OpcionLetra.PEsProveedor = False
            Else : OpcionLetra.PEsProveedor = True : OpcionLetra.PEsLocalYImportacion = True
            End If
            OpcionLetra.PTipoNota = PTipoNota
            OpcionLetra.ShowDialog()
            PEmisor = OpcionLetra.PEmisor
            PAbierto = OpcionLetra.PAbierto
            LetraIva = OpcionLetra.PNumeroLetra
            TextLetra.Text = LetraTipoIva(LetraIva)
            OpcionLetra.Dispose()
            If PEmisor = 0 Then Exit Sub
            Select Case PTipoNota
                Case 500, 700
                    If HallaTipoIvaProveedor(PEmisor) = 4 Then   'caso importador.
                        If LetraIva <> 4 Then
                            MsgBox("Incorrecta Letra para Proveedor de Importación.")
                            PEmisor = 0 : Exit Sub
                        End If
                    Else
                        If LetraIva = 4 Then
                            MsgBox("Incorrecta Letra para para Proveedor Interno.")
                            PEmisor = 0 : Exit Sub
                        End If
                    End If
                Case 50, 70
                    If HallaTipoIvaCliente(PEmisor) = 4 Then   'caso Cliente del Exterior.
                        If LetraIva <> 4 Then
                            MsgBox("Incorrecta Letra para Cliente de Exportación.")
                            PEmisor = 0 : Exit Sub
                        End If
                    Else
                        If LetraIva = 4 Then
                            MsgBox("Incorrecta Letra para para Cliente Interno.")
                            PEmisor = 0 : Exit Sub
                        End If
                    End If
            End Select
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
            Exit Sub
        End If

        If PEmisor = 0 Then   'Dejar. Es para opcion nueva factura igual proveedor. 
            If PTipoNota = 6 Or PTipoNota = 8 Or PTipoNota = 13006 Or PTipoNota = 13008 Then OpcionEmisor.PEsProveedor = True
            If PTipoNota = 5 Or PTipoNota = 7 Or PTipoNota = 13005 Or PTipoNota = 13007 Then OpcionEmisor.PEsCliente = True
            OpcionEmisor.PTipoNota = PTipoNota
            OpcionEmisor.PEsPorDiferenciaCambio = PDiferenciaDeCambio
            If PTr Then
                OpcionEmisor.PPuedeTenerFCE = False
            Else
                If PTipoNota = 5 Or PTipoNota = 7 Then OpcionEmisor.PPuedeTenerFCE = True
            End If
            OpcionEmisor.ShowDialog()
            PEmisor = OpcionEmisor.PEmisor
            PACuenta = OpcionEmisor.PACuenta
            PAbierto = OpcionEmisor.PAbierto
            EsFCE = OpcionEmisor.PEsFCE
            FacturaFCE = OpcionEmisor.PFacturaFCE
            OpcionEmisor.Dispose()
            If PEmisor = 0 Then Exit Sub
        End If

        Dim TipoIvaCliente As Integer

        Select Case PTipoNota
            Case 5, 7
                TipoIvaCliente = HallaTipoIvaCliente(PEmisor)
                LetraIva = LetrasPermitidasCliente(TipoIvaCliente, PTipoNota)
            Case 6, 8
                LetraIva = LetrasPermitidasProveedor(HallaTipoIvaProveedor(PEmisor), PTipoNota)
            Case 13005
                TipoIvaCliente = HallaTipoIvaCliente(PEmisor)
                LetraIva = LetrasPermitidasCliente(TipoIvaCliente, 5)
            Case 13006
                LetraIva = LetrasPermitidasProveedor(HallaTipoIvaProveedor(PEmisor), 6)
            Case 13007
                TipoIvaCliente = HallaTipoIvaCliente(PEmisor)
                LetraIva = LetrasPermitidasCliente(TipoIvaCliente, 7)
            Case 13008
                LetraIva = LetrasPermitidasProveedor(HallaTipoIvaProveedor(PEmisor), 8)
        End Select

        TextLetra.Text = LetraTipoIva(LetraIva)

        Dim TipoNotaParaPuntoVenta As Integer
        Select Case PTipoNota
            Case 13005
                TipoNotaParaPuntoVenta = 5
            Case 13006
                TipoNotaParaPuntoVenta = 6
            Case 13007
                TipoNotaParaPuntoVenta = 7
            Case 13008
                TipoNotaParaPuntoVenta = 8
            Case Else
                TipoNotaParaPuntoVenta = PTipoNota
        End Select

        If EsFCE Then
            GPuntoDeVenta = HallaPuntoVentaFce()
        Else
            If TipoIvaCliente = 3 Then
                GPuntoDeVenta = HallaPuntoVentaSegunTipo(TipoNotaParaPuntoVenta, 3)
            Else
                GPuntoDeVenta = HallaPuntoVentaSegunTipo(TipoNotaParaPuntoVenta, LetraIva)
            End If
        End If
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
        Dim EsFCEW As Boolean = EsPuntoDeVentaCFE(GPuntoDeVenta)
        If EsFCE And Not EsFCEW Then
            MsgBox("Punto de Venta del Operador " & Format(GPuntoDeVenta, "0000") & " No es para FCE.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            PEmisor = 0
        End If
        If Not EsFCE And EsFCEW Then
            MsgBox("Punto de Venta del Operador " & Format(GPuntoDeVenta, "0000") & " Reservado para FCE.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
    Private Sub CalculaTotales()

        TotalNetoPerc = 0
        TotalConceptos = 0

        If ComboTipoIva.SelectedValue <> Exterior Then
            For Each Row As DataRow In DtGrid.Rows
                TotalNetoPerc = TotalNetoPerc + Row("Neto")
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

        Select Case PTipoNota
            Case 5, 6, 7, 8
                If PAbierto Then
                    If PNota = 0 Then
                        TotalPercepciones = 0
                        For Each Fila As ItemIvaReten In ListaDePercepciones
                            TotalPercepciones = TotalPercepciones + Fila.Importe
                        Next
                    End If
                End If
        End Select
        TextImporteRetenciones.Text = FormatNumber(TotalPercepciones, 2)

        TotalFacturas = 0
        For Each Row As DataRow In DtGridCompro.Rows
            If Row.RowState <> DataRowState.Deleted Then
                TotalFacturas = Trunca(TotalFacturas + Row("Asignado"))
            End If
        Next

        TextTotalFacturas.Text = FormatNumber(TotalFacturas, GDecimales)

        Select Case PTipoNota    'Esto es para que no modifique el saldo cuando se regraba la nota.(Saldo esta enlazado con Registro cabeza.
            Case 7, 13007, 50, 6, 13006, 700
                TextSaldo.Text = FormatNumber(CDec(TextTotalRecibo.Text) - ImputacionDeOtros - TotalFacturas, GDecimales)
            Case Else
                If PNota = 0 Then
                    TextSaldo.Text = FormatNumber(CDec(TextTotalRecibo.Text), GDecimales)
                End If
        End Select

    End Sub
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
            MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, LetraComprobante, GCondicionIvaEmpresa, IngBruto, FechaInicio, "C:\XML Afip\", e, EsFCE, PTipoNota)
            Texto = NumeroEditado(Strings.Right(DtNotaCabeza.Rows(0).Item("Nota").ToString, 12))
            PrintFont = New Font("Arial", 16)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, MTop + 6)
        End If

        If EsNotaInterna And PAbierto Then
            Dim NombreEmpres As String = GNombreEmpresa '"Patagonia Sunrise S.R.L."
            Dim CuitEmpresa As String = GCuitEmpresa    '"30-12345678-5"
            Dim TipoComprobante As Integer          '1. factura 2. debito 3. credito
            Select Case PTipoNota
                Case 13005, 13006
                    TipoComprobante = 12
                Case 13007, 13008
                    TipoComprobante = 13
            End Select
            Dim Direccion1 As String = GDireccion1      '"Nave S-2 Puesto 34 - Mercado Central de Buenos Aires"
            Dim Direccion2 As String = GDireccion2      '"Autopista Ricchieri y Boulongne Sur Mer"
            Dim Direccion3 As String = GDireccion3      '"C.P.(1771) Villa Celina Pcia. de Buenos Aires"
            Dim LetraComprobante As String = LetraTipoIva(LetraIva)
            Dim IngBruto As String = GIngBruto     '"Conv.Mult. Nª 901-898989-6"
            Dim FechaInicio As String = GFechaInicio
            '
            MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, "X", GCondicionIvaEmpresa, IngBruto, FechaInicio, "C:\XML Afip\", e, False, TipoComprobante)
            Texto = NumeroEditado(Strings.Right(DtNotaCabeza.Rows(0).Item("Nota").ToString, 12))
            Texto = LetraComprobante & Texto
            PrintFont = New Font("Arial", 16)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, MTop + 6)
        End If

        Dim Str1 As String = ""
        If EsFCE Then
            Str1 = "miPyMEs (FCE)"
            Texto = Str1
            PrintFont = New Font("Arial", 12)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 129, MTop - 1)
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
                If Numero <> 0 Then
                    Texto = "DOMICILIO   : " & Calle & " No: " & Numero
                Else
                    Texto = "DOMICILIO   : " & Calle
                End If
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "LOCALIDAD   : " & Localidad & " " & Provincia
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "DOM.ENTREGA : "
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "CUIT        : " & Cuit & " " & ComboTipoIva.Text
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
                Texto = Row("Detalle")
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Dim EsPercepcion As Boolean = False
                If DtRecibosPercepciones.Rows.Count = 0 Then
                    EsPercepcion = False
                Else
                    Dim RowsBusqueda() As DataRow
                    RowsBusqueda = DtRecibosPercepciones.Select("Percepcion = " & Row("MedioPago")) 've si es percepcion siempre lo imprime.
                    If RowsBusqueda.Length <> 0 Then EsPercepcion = True
                End If
                If EsPercepcion Then
                    Texto = FormatNumber(Row("Importe"), GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
                If Not EsPercepcion Then
                    'Imprime Neto.
                    Texto = FormatNumber(Row("Neto"), GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
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
            For Each Row As DataRow In DtGrid.Rows
                TotalNeto = TotalNeto + Row("Neto")
            Next
            Texto = "Neto"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaImporte - 60, Yq)
            Texto = FormatNumber(TotalNeto, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            '
            Dim ListaIva As New List(Of ItemIva)
            ArmaListaImportesIva(ListaIva)
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

            'Percepcion.
            Yq = Yq + SaltoLinea
            Texto = "Tributos"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaImporte - 60, Yq)
            Texto = TextImporteRetenciones.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)

            'Total
            Yq = Yq + SaltoLinea
            Texto = "Total"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaImporte - 60, Yq)
            Texto = TextTotalRecibo.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)

            'Texto para impresion.
            PrintFont = New Font("Courier New", 9)
            Yq = MTop + 72 + Alto + 10
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox1.Text, PrintFont, Brushes.Black, x, Yq)
            Yq = Yq + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox2.Text, PrintFont, Brushes.Black, x, Yq)
            Yq = Yq + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox3.Text, PrintFont, Brushes.Black, x, Yq)
            Yq = Yq + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox4.Text, PrintFont, Brushes.Black, x, Yq)
            PrintFont = New Font("Courier New", 11)

            'Imprime Cae
            If DtNotaCabeza.Rows(0).Item("Cae") <> 0 Then
                PrintFont = New Font("Courier New", 12)
                Yq = 265    '270
                e.Graphics.DrawString(LabelCAE.Text, PrintFont, Brushes.Black, x + 25, Yq)
                '----------------------------------------Codigo Barra anulado. Reemplazado Por QR.------------------------------------------------------------------------------
                '   Dim CodigoBarra As String = Format(CuitNumerico(GCuitEmpresa), "00000000000") & Format(HallaTipo("CD", LetraIva, DtNotaCabeza.Rows(0).Item("TipoNota")), "00") & Format(CInt(Strings.Mid(DtNotaCabeza.Rows(0).Item("Nota").ToString, 2, 4)), "0000") & Format(DtNotaCabeza.Rows(0).Item("Cae"), "00000000000000") & Format(DtNotaCabeza.Rows(0).Item("FechaCae"), "00000000")
                '   Dim aa As New DllVarias
                '   CodigoBarra = aa.CombierteTextoAInterleaved2Of5(CodigoBarra, True)
                '   Dim Tamanio As Integer = 18
                '  Dim fuente As Font
                '  fuente = CustomFont.GetInstance(Tamanio, FontStyle.Regular)
                '  e.Graphics.DrawString(CodigoBarra, fuente, Brushes.Black, x, Yq + 7)
                '------------------------------------------------------------------------------------------------------------------------------------
                '----------------------------------------- QR -------------------------------------
                ImprimeQR(x - 4, Yq - 2, 22, 22, e)
                '------------------------------------------------------------------------------------------------------------------------------------
            End If
            'Cartel para montributo ley 27.618
            If ComboTipoIva.SelectedValue = 6 And (LetraIva = 1 Or LetraIva = 5) And PAbierto Then
                PrintFont = New Font("Courier New", 8)
                Texto = "El crédito fiscal discriminado en el presente comprobante,sólo podra ser"
                Yq = Yq + 2 * SaltoLinea
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 25, Yq)
                Yq = Yq + SaltoLinea
                Texto = "computado a efectos del Régimen de Sostenimiento e Inclusión Fiscal para"
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 25, Yq)
                Yq = Yq + SaltoLinea
                Texto = "Pequeños Contribuyentes de la ley 27.618"
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 25, Yq)
            End If
            '-------------------------------------------------------------


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
    Private Sub ImprimeQR(ByVal X As Long, ByVal Y As Long, ByVal Ancho As Long, ByVal Alto As Long, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim ImageQR As Image = ArmaDatoParaQR(PTipoNota, DtNotaCabeza.Rows(0).Item("Fecha"), DtNotaCabeza.Rows(0).Item("Nota"), DtNotaCabeza.Rows(0).Item("EsFCE"), DtNotaCabeza.Rows(0).Item("Cae"), DtNotaCabeza.Rows(0).Item("Importe"), Cuit, DocTipo, DocNro, ComboTipoIva.SelectedValue)
        e.Graphics.DrawImage(ImageQR, X, Y, Ancho, Alto)

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
        Else
            Return True
        End If

    End Function
    Private Function EsReemplazoCheque(ByVal TipoNota As Integer, ByVal Nota As Double, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ClaveChequeReemplazado FROM RecibosCabeza WHERE TipoNota = " & TipoNota & " AND Nota = " & Nota & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Proveedores.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function AgregalistaPercepciones() As Boolean

        ListaDePercepciones = New List(Of ItemIvaReten)

        Dim TipoEmisor As Integer
        Select Case PTipoNota
            Case 5, 7
                TipoEmisor = 2  'cliente
            Case 6, 8
                TipoEmisor = 1  'proveedor
        End Select
        Dim TipoNotaW As Integer
        Select Case PTipoNota
            Case 5, 6
                TipoNotaW = 5
            Case 7, 8
                TipoNotaW = 7
        End Select

        If Not AgregaPercepciones(ListaDePercepciones, TipoNotaW, ComboEmisor.SelectedValue, Cuit, TipoEmisor, DateTime1.Value) Then Return False

        If ListaDePercepciones.Count <> 0 Then   'Agrega percepciones al grid.
            AgregaPercepcionesAlGrid()
        End If
        If ListaDePercepciones.Count = 0 Then    'Lo saca de medios de pago.
            Select Case PTipoNota
                Case 5, 6, 7, 8
                    For i As Integer = DtFormasPago.Rows.Count - 1 To 0 Step -1
                        If DtFormasPago.Rows(i).Item("Tipo") = 4 Then DtFormasPago.Rows(i).Delete()
                    Next
            End Select
            DtFormasPago.AcceptChanges()
        End If


        Return True

    End Function
    Private Function AgregaPercepcionesAlGrid() As Decimal

        Dim Item As Integer = 0

        For Each Fila As ItemIvaReten In ListaDePercepciones
            Dim Row1 As DataRow = DtGrid.NewRow
            Item = Item + 1
            Row1("Item") = Item
            Row1("MedioPago") = Fila.Clave
            Row1("Detalle") = Fila.Nombre
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
            Row1("Importe") = 0
            Row1("Comprobante") = 0
            Row1("FechaComprobante") = "1/1/1800"
            Row1("Bultos") = 0
            Row1("ClaveCheque") = 0
            Row1("ClaveInterna") = 0
            Row1("ClaveChequeVisual") = 0
            Row1("TieneLupa") = False
            Row1("eCheq") = False
            DtGrid.Rows.Add(Row1)
        Next

    End Function
    Private Function Valida() As Boolean

        Select Case PTipoNota
            Case 50, 70, 500, 700
            Case 5, 7
                If PNota = 0 And DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 Then
                    MsgBox("Fecha Menor a la Fecha del ultimo Recibo Grabado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    DateTime1.Focus()
                    Return False
                End If
            Case Else
                '     If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 Then
                '         MsgBox("Fecha Menor a la Fecha del ultimo Recibo Grabado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                '         DateTime1.Focus()
                '         Return False
                '     End If
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
            If ReciboOficialAnt <> DtNotaCabeza.Rows(0).Item("ReciboOficial") Then
                If ExisteReciboOficial(PTipoNota, PNota, PEmisor, DtNotaCabeza.Rows(0).Item("ReciboOficial"), ConexionNota) Then
                    MsgBox("Recibo Oficial Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    MaskedReciboOficial.Focus()
                    Return False
                End If
            End If
            If Not ConsisteFecha(TextFechaReciboOficial.Text) Then
                MsgBox("Fecha Recibo Oficial Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaReciboOficial.Focus()
                Return False
            End If
            If DiferenciaDias(DateTime1.Value, TextFechaReciboOficial.Text) < -365 Then
                If MsgBox("Fecha Recibo Oficial Supera a un año. Desea Continuar?: ", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                    TextFechaReciboOficial.Focus()
                    Return False
                End If
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
                If MsgBox("Fecha Contable Supera a un año. Desea Continuar?: ", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                    TextFechaContable.Focus()
                    Return False
                End If
            End If
            If DiferenciaDias(Date.Now, TextFechaContable.Text) > 0 Then
                MsgBox("Fecha Contable Mayor que la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
            If Panel5.Visible Then
                If DiferenciaDias(TextFechaReciboOficial.Text, TextFechaContable.Text) < 0 Then
                    MsgBox("Fecha Contable Debe ser Mayor o igual a la Fecha Recibo Oficial.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    TextFechaContable.Focus()
                    Return False
                End If
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

        Dim ImporteIva As Decimal = 0
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("ImporteIva") <> 0 Then
                ImporteIva = ImporteIva + Row1("ImporteIva")
            End If
        Next

        Dim ImporteRetencion As Decimal = 0
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("TieneLupa") Then
                ImporteRetencion = ImporteRetencion + Row1("Importe")
            End If
        Next

        If EsNotaInterna And (ImporteIva <> 0 Or ImporteRetencion <> 0) Then
            MsgBox("En Nota Interna no debe Informarse IVA, Percepción o Retención. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

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

        If EsFCE And PNota = 0 And PTipoNota = 7 Then  'Ve si facturaFCE tiene saldo disponible.
            UnSaldoFCE.PFactura = FacturaFCE
            UnSaldoFCE.PMuestraFormulario = False
            UnSaldoFCE.ShowDialog()
            Dim Saldo As Decimal = UnSaldoFCE.PSaldo
            UnSaldoFCE.Dispose()
            If (Saldo - CDec(TextTotalRecibo.Text)) < 0 Then
                MsgBox("Importe De la Nota mayor al Saldo de la Faltura FCE. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRIDCOMPRO DE COMPROBANTES.      ----------------------------------------------
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