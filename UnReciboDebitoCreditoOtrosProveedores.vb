Option Explicit On
Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnReciboDebitoCreditoOtrosProveedores
    Public PNota As Integer
    Public PAbierto As Boolean
    Public PTipoNota As Integer
    Public PProveedor As Integer
    Public PBloqueaFunciones As Boolean
    Public PImporte As Double
    Public PImputa As Boolean
    'Datos para rechazos de cheques.
    Public PClaveCheque As Integer
    Public PNumeroCheque As Decimal
    Public PImporteCheque As Decimal
    Public PEsRechazoCheque As Boolean
    Public PMedioPago As Integer
    '
    Dim DtNotaDetalle As DataTable
    Dim DtNotaCabeza As DataTable
    Dim DtMedioPago As DataTable
    Dim DtChequesRechazados As DataTable
    '
    Dim DtFacturasCabeza As DataTable
    Dim DtComprobantesCabeza As DataTable
    '
    Dim DtGridCompro As DataTable
    Dim DtGrid As DataTable
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
    Dim UltimaFechaW As DateTime
    Dim UltimafechaContableW As DateTime
    Dim FechaAnt As DateTime
    Dim TablaIva(0) As Double
    Dim TipoAsiento As Integer
    Dim EsNotaInterna As Boolean
    'Variables Impresion. 
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim LineasParaImpresion As Integer = 0
    Dim LineasParaImpresionImputacion As Integer = 0
    'variables para imprimir retenciones manuales.
    Dim NumeroRetencion As Integer
    Dim NombreRetencion As String
    Dim ImporteRetencion As Decimal
    Private Sub UnReciboDebitoCreditoOtrosProveedores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(8) Then PBloqueaFunciones = True

        Me.Top = 50

        If GCaja = 0 And Pnota = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        GridCompro.AutoGenerateColumns = False
        GridCompro.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        If PNota = 0 And PProveedor = 0 Then
            OpcionOtrosProveedores.PEsDebitoCredito = True
            OpcionOtrosProveedores.ShowDialog()
            PProveedor = OpcionOtrosProveedores.PProveedor
            PAbierto = OpcionOtrosProveedores.PAbierto
            OpcionOtrosProveedores.Dispose()
            If PProveedor = 0 Then Me.Close() : Exit Sub
        End If

        If PAbierto Then
            ConexionNota = Conexion
        Else : ConexionNota = ConexionN
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        ArmaTipoIva(ComboTipoIva)

        ComboProveedor.DataSource = Nothing
        ComboProveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM OtrosProveedores WHERE Estado = 1;")
        Dim Row As DataRow = ComboProveedor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboProveedor.DataSource.rows.add(Row)
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0

        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ArmaTablaIva(TablaIva)

        Dim TipoNotaW As Integer
        If PTipoNota = 5005 Then TipoNotaW = 6
        If PTipoNota = 5007 Then TipoNotaW = 8
        ArmaMedioPagoOtras(TipoNotaW, DtFormasPago, PAbierto, PNota)

        If TipoNotaW = 6 Then
            For I As Integer = DtFormasPago.Rows.Count - 1 To 0 Step -1
                Dim RowW As DataRow = DtFormasPago.Rows(I)
                If RowW("Tipo") = 4 Then
                    RowW.Delete()
                End If
            Next
        End If

        If PTipoNota = 5005 Then LabelTipoNota.Text = "Nota Debito Financiera a Proveedor"
        If PTipoNota = 5007 Then LabelTipoNota.Text = "Nota Crédito Financiera a Proveedor"

        GModificacionOk = False

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

        UnTextoParaRecibo.Dispose()

        UltimaFechaW = UltimaFecha(Conexion)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub UnReciboDebitoCreditoOtrosProveedores_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

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

        Dim RowsBusqueda() As DataRow

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtChequesRechazadosAux As DataTable = DtChequesRechazados.Copy

        Dim DtFacturasCabezaAux As DataTable = DtFacturasCabeza.Copy
        Dim DtComprobantesCabezaAux As DataTable = DtComprobantesCabeza.Copy

        'Actualiza Saldo de Comprobantes Imputados.
        ActualizaComprobantes("M", DtFacturasCabezaAux, DtComprobantesCabezaAux)

        'Actualiza Notas Cabeza.
        If PanelFechaContable.Visible = True Then
            If DtNotaCabezaAux.Rows(0).Item("FechaContable") <> CDate(TextFechaContable.Text) Then
                DtNotaCabezaAux.Rows(0).Item("FechaContable") = TextFechaContable.Text
            End If
        End If

        Dim Row1 As DataRow

        'Actualiza Notas Detalle.
        For Each Row1 In DtGridCompro.Rows
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
                        Row("Movimiento") = DtNotaCabeza.Rows(0).Item("Movimiento")
                        Row("TipoComprobante") = Row1("Tipo")
                        Row("Comprobante") = Row1("Comprobante")
                        Row("Importe") = Row1.Item("Asignado")
                        DtNotaDetalleAux.Rows.Add(Row)
                    End If
                End If
            End If
        Next

        'Genera Medios Pago si es alta.
        If PNota = 0 Then
            For Each Row As DataRow In DtGrid.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row1 = DtMedioPagoAux.NewRow
                    Row1("Movimiento") = 0
                    Row1("MedioPago") = Row("MedioPago")
                    Row1("Detalle") = Row("Detalle")
                    Row1("Neto") = Row("Neto")
                    Row1("Alicuota") = Row("Alicuota")
                    Row1("Cambio") = Row("Cambio")
                    Row1("Banco") = Row("Banco")
                    Row1("Cuenta") = Row("Cuenta")
                    Row1("Importe") = Row("Importe")
                    Row1("Comprobante") = Row("Comprobante")
                    Row1("FechaComprobante") = Row("FechaComprobante")
                    Row1("ClaveCheque") = Row("ClaveCheque")
                    DtMedioPagoAux.Rows.Add(Row1)
                End If
            Next
        End If

        'Actualiza Archivo de distribucion retenciones por provincia.
        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone
        If PNota = 0 Then
            For Each Row As DataRow In DtRetencionProvinciaAux.Rows
                Row1 = DtRetencionProvinciaWW.NewRow
                Row1("TipoNota") = PTipoNota
                Row1("Nota") = 0
                Row1("Provincia") = Row("Provincia")
                Row1("Retencion") = Row("Retencion")
                Row1("Importe") = Row("Importe")
                DtRetencionProvinciaWW.Rows.Add(Row1)
            Next
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PEsRechazoCheque And PNota = 0 Then
            DtChequesRechazadosAux.Rows(0).Item("Estado") = 4
            Select Case PTipoNota
                Case 5005
                    DtChequesRechazadosAux.Rows(0).Item("TieneDebito") = True
            End Select
        End If

        If IsNothing(DtNotaDetalleAux.GetChanges) And IsNothing(DtNotaCabezaAux.GetChanges) Then
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
                If Not ArmaArchivosAsiento("M", DtAsientoCabezaAux, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            End If
        End If

        If PNota = 0 Then
            If HacerAlta(DtNotaCabezaAux, DtFacturasCabezaAux, DtComprobantesCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtChequesRechazadosAux) Then
                ArmaArchivos()
            End If
        Else
            If HacerModificacion(DtNotaCabezaAux, DtFacturasCabezaAux, DtComprobantesCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW, DtChequesRechazadosAux) Then
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

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtChequesRechazadosAux As DataTable = DtChequesRechazados.Copy

        'Actualiza Saldo de Comprobantes Imputados.
        Dim DtFacturasCabezaAux As DataTable = DtFacturasCabeza.Copy
        Dim DtComprobantesCabezaAux As DataTable = DtComprobantesCabeza.Copy

        ActualizaComprobantes("M", DtFacturasCabezaAux, DtComprobantesCabezaAux)

        If Not (IsNothing(DtFacturasCabezaAux.GetChanges) And IsNothing(DtComprobantesCabezaAux.GetChanges)) Then
            MsgBox("Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Resul As Double

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Actualiza Saldo de Comprobantes Imputados.
        DtFacturasCabezaAux = DtFacturasCabeza.Copy
        DtComprobantesCabezaAux = DtComprobantesCabeza.Copy

        ' Fue anulado por que se obliga a anular las imputaciones manualmente.
        '     ActualizaComprobantes("B", DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux)

        If PEsRechazoCheque Then
            Select Case PTipoNota
                Case 5007
                    If DtChequesRechazadosAux.Rows(0).Item("TieneDebito") Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Se Genero un Debito para este Cheque, debe anularlo previamente para continuar con la anulación de esta Nota. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    Else
                        DtChequesRechazadosAux.Rows(0).Item("Estado") = 1
                    End If
                Case 5005
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
        If Not HallaAsientosCabeza(TipoAsiento, DtNotaCabeza.Rows(0).Item("Movimiento"), DtAsientoCabezaAux, ConexionNota) Then Exit Sub
        For Each Row As DataRow In DtAsientoCabezaAux.Rows
            Row("Estado") = 3
        Next

        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone

        DtNotaCabezaAux.Rows(0).Item("Estado") = 3

        Resul = ActualizaNota("B", DtNotaCabezaAux, DtNotaDetalleAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux, DtFacturasCabezaAux, DtComprobantesCabezaAux)

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
    Private Sub ButtonComprobantesAImputar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonComprobantesAImputar.Click

        UNComprobanteAImputar.PDtGridCompro = DtGridCompro
        UNComprobanteAImputar.PTipo = PTipoNota
        UNComprobanteAImputar.PAbierto = PAbierto
        UNComprobanteAImputar.PTotalConceptos = TextTotalRecibo.Text
        UNComprobanteAImputar.PTipoIva = ComboTipoIva.SelectedValue
        UNComprobanteAImputar.PMoneda = 1
        UNComprobanteAImputar.PCambio = 1
        UNComprobanteAImputar.ShowDialog()
        DtGridCompro = UNComprobanteAImputar.PDtGridCompro
        UNComprobanteAImputar.Dispose()

        ArmaGridCompro()
        CalculaTotales()

    End Sub
    Private Sub ButtonMediosDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMediosDePago.Click

        Dim DtFormasPagoW As DataTable = DtFormasPago.Copy

        'No permite informar en grid si total orden de compra no esta informado.  

        Dim TipoNotaW As Integer
        If PTipoNota = 5005 Then TipoNotaW = 6
        If PTipoNota = 5007 Then TipoNotaW = 8

        UnMediosPago.PTipoNota = TipoNotaW
        UnMediosPago.PEmisor = PProveedor
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
        UnMediosPago.PMoneda = 1
        UnMediosPago.PCambio = 1
        UnMediosPago.PDtRetencionesAutomaticas = New DataTable
        UnMediosPago.PDiferenciaDeCambio = False
        UnMediosPago.PImporte = CDbl(TextTotalRecibo.Text)
        UnMediosPago.PEsRetencionManual = False
        UnMediosPago.ShowDialog()
        DtGrid = UnMediosPago.PDtGrid

        UnMediosPago.Dispose()

        CalculaTotales()

    End Sub
    Private Sub ButtonDatosCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDatosCliente.Click

        If ComboProveedor.SelectedValue = 0 Then Exit Sub

        UnDatosEmisor.PEsOtroProveedor = True
        UnDatosEmisor.PEmisor = ComboProveedor.SelectedValue
        UnDatosEmisor.ShowDialog()

    End Sub
    Private Sub ButtonEliminarTodo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarTodo.Click

        If MsgBox("Medios Pagos se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtGrid.Clear()
        CalculaTotales()

    End Sub
    Private Sub ButtonTextoRecibo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTextoRecibo.Click

        UnTextoParaRecibo.ShowDialog()

    End Sub
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM OtrosPagosCabeza;", Miconexion)
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

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Nota debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ErrorImpresion = False
        Paginas = 0
        Copias = 1

        Dim print_document As New PrintDocument

        AddHandler print_document.PrintPage, AddressOf Print_PrintPageNota    'Notas credito y debitos financieras.
        print_document.Print()
        If ErrorImpresion Then Exit Sub

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
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PNota = 0
        PProveedor = 0

        UnReciboDebitoCreditoOtrosProveedores_Load(Nothing, Nothing)

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueContable.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Function ArmaArchivos() As Boolean                'ArmaArchivos

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()
        CreaDtGridCompro()
        CreaDtRetencionProvinciaAux()

        Dim Sql As String
        Dim Row1 As DataRow
        Dim RowsBusqueda() As DataRow

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM OtrosPagosCabeza WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaCabeza) Then Return False
        If PNota <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then
            If Not LlenaDatosEmisor(PProveedor) Then Return False
        Else
            If Not LlenaDatosEmisor(DtNotaCabeza.Rows(0).Item("Proveedor")) Then Return False
        End If

        If PNota = 0 Then AgregaCabeza()

        MuestraCabeza()

        PMedioPago = DtNotaCabeza.Rows(0).Item("MedioPagoRechazado")

        PProveedor = DtNotaCabeza.Rows(0).Item("Proveedor")
        PClaveCheque = DtNotaCabeza.Rows(0).Item("ChequeRechazado")
        If PClaveCheque <> 0 Then
            PEsRechazoCheque = True
        Else : PEsRechazoCheque = False
        End If

        DtChequesRechazados = New DataTable
        If PEsRechazoCheque Then
            If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & PMedioPago & " AND ClaveCheque = " & PClaveCheque & ";", ConexionNota, DtChequesRechazados) Then Me.Close() : Exit Function
        End If

        DtRetencionProvincia = New DataTable
        If PAbierto Then
            If Not Tablas.Read("SELECT * FROM RecibosRetenciones WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";", Conexion, DtRetencionProvincia) Then Return False
            For Each Row As DataRow In DtRetencionProvincia.Rows
                Row1 = DtRetencionProvinciaAux.NewRow
                Row1("Retencion") = Row("Retencion")
                Row1("Provincia") = Row("Provincia")
                Row1("Importe") = Row("Importe")
                DtRetencionProvinciaAux.Rows.Add(Row1)
            Next
        End If

        DtNotaDetalle = New DataTable
        Sql = "SELECT * FROM OtrosPagosDetalle WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaDetalle) Then Return False

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM OtrosPagosPago WHERE Movimiento = " & PNota & ";"
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
            Row1("ImporteIva") = 0
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            Row1("ClaveInterna") = 0
            Row1("Banco") = Row("Banco")
            Row1("Cuenta") = Row("Cuenta")
            Row1("Serie") = ""
            Row1("Numero") = 0
            Row1("Fecha") = "1/1/1800"
            Row1("TieneLupa") = False
            DtGrid.Rows.Add(Row1)
        Next

        If PNota = 0 And PEsRechazoCheque Then
            Dim Row As DataRow = DtGrid.NewRow
            Row("Item") = 0
            Row("MedioPago") = 100
            Row("Importe") = PImporteCheque
            Row("Cambio") = 0
            Row("Bultos") = 0
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
            Row("Serie") = ""
            Row("Numero") = 0
            Row("Fecha") = "1/1/1800"
            Row("TieneLupa") = False
            DtGrid.Rows.Add(Row)
        End If

        'Muestra Comprobantes a Imputar.
        DtFacturasCabeza = New DataTable
        DtComprobantesCabeza = New DataTable

        If PTipoNota = 5005 Then
            If Not ArmaConFacturas() Then Return False
            If Not ArmaConNotas(5020) Then Return False 'Devolucións a Clientes     
        End If

        Select Case PTipoNota
            Case 5005
            Case Else
                ButtonComprobantesAImputar.Visible = False
                GridCompro.Visible = False
                Panel3.Visible = False
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
            Row1("Comprobante") = Row("Factura")
            Row1("Recibo") = Row("Factura")
            Row1("Fecha") = Row("Fecha")
            Row1("Comentario") = Row("Comentario")
            Row1("Importe") = Row("Importe")
            Row1("Moneda") = 1
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next

        'Procesa Devoluciones.
        For Each Row As DataRow In DtComprobantesCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = Row("TipoNota")
            Row1("TipoVisible") = Row("TipoNota")
            Row1("Comprobante") = Row("Movimiento")
            Row1("Recibo") = Row("Movimiento")
            Row1("Fecha") = Row("Fecha")
            Row1("Comentario") = Row("Comentario")
            Row1("Importe") = Row("Importe")
            Row1("Moneda") = 1
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
        If PTipoNota = 5007 And PEsRechazoCheque <> 0 Then TipoAsiento = 7011
        If PTipoNota = 5005 And PEsRechazoCheque <> 0 Then TipoAsiento = 7011

        If PNota <> 0 Then
            ButtonAceptar.Text = "Modificar Recibo"
            DateTime1.Enabled = False
        Else
            ButtonAceptar.Text = "Grabar Recibo"
            DateTime1.Enabled = True
        End If

        If DtNotaCabeza.Rows(0).Item("Estado") = 3 Then
            GridCompro.ReadOnly = True
        Else : GridCompro.ReadOnly = False
        End If

        CalculaTotales()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If DtNotaCabeza.Rows(0).Item("Estado") = 3 Then
            GridCompro.ReadOnly = True
        Else
            GridCompro.ReadOnly = False
        End If

        Return True

    End Function
    Private Function ArmaConFacturas() As Boolean

        Dim Sql As String = ""

        If PNota = 0 Then
            Sql = "SELECT * FROM OtrasFacturasCabeza WHERE Estado = 1 AND Proveedor = " & PProveedor & " AND Saldo <> 0 ORDER BY Factura,Fecha;"
        Else
            Sql = "SELECT * FROM OtrasFacturasCabeza WHERE Estado = 1 AND Proveedor = " & PProveedor & " ORDER BY Factura,Fecha;"
        End If

        If Not Tablas.Read(Sql, ConexionNota, DtFacturasCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConNotas(ByVal TipoNota As Integer) As Boolean

        Dim Sql As String = ""

        Sql = "SELECT * FROM OtrosPagosCabeza WHERE Estado = 1 AND TipoNota = " & TipoNota & " AND Proveedor = " & PProveedor & " ORDER BY Movimiento,Fecha;"
        If Not Tablas.Read(Sql, ConexionNota, DtComprobantesCabeza) Then Return False

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

        Enlace = New Binding("SelectedValue", MiEnlazador, "Proveedor")
        ComboProveedor.DataBindings.Clear()
        ComboProveedor.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Movimiento")
        AddHandler Enlace.Format, AddressOf FormatMovimiento
        TextComprobante.DataBindings.Clear()
        TextComprobante.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalRecibo.DataBindings.Clear()
        TextTotalRecibo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Saldo")
        TextSaldo.DataBindings.Clear()
        TextSaldo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatMovimiento(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

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

        Dim Total As Decimal
        For Each Row As DataRow In DtGrid.Rows
            Total = Total + Row("Importe")
        Next

        If Funcion = "A" Then
            Select Case PTipoNota
                Case 5005, 5007
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = 213
                    Item.Importe = Total
                    '   If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
                    ListaConceptos.Add(Item)
            End Select
        End If

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

        If Funcion = "A" Then
            Dim Total As Double = PImporteCheque
            If PTipoNota = 5007 Then
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

        If Not Asiento(TipoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Fecha, 0) Then Return False

        Return True

    End Function
    Private Sub AgregaCabeza()

        Dim Row As DataRow = DtNotaCabeza.NewRow
        ArmaNuevoMovimientoOtroPago(Row)
        Row("Proveedor") = PProveedor
        Row("TipoNota") = PTipoNota
        Row("TipoPago") = 0
        Row("Fecha") = Now
        Row("Caja") = GCaja
        Row("Estado") = 1
        If PEsRechazoCheque Then
            Row("MedioPagoRechazado") = PMedioPago
            Row("ChequeRechazado") = PClaveCheque
            If PMedioPago = 2 Then
                Row("Comentario") = "Rechazo Cheque " & FormatNumber(PNumeroCheque, 0)
            End If
            If PMedioPago = 3 Then
                Row("Comentario") = "Rechazo Cheque " & FormatNumber(PClaveCheque, 0)
            End If
            Row("Importe") = PImporteCheque
            Row("Saldo") = PImporteCheque
        End If
        DtNotaCabeza.Rows.Add(Row)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Tipo.DataSource = DtTiposComprobantes(PAbierto)
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Codigo"

        TipoVisible.DataSource = DtTiposComprobantes(PAbierto)
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

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtRetencionProvinciaAux.Columns.Add(Importe)

    End Sub
    Private Function LlenaDatosEmisor(ByVal Proveedor As Integer) As Boolean

        Dim Dta As New DataTable
        Dim Sql As String = ""

        Sql = "SELECT * FROM OtrosProveedores WHERE Clave = " & Proveedor & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, Proveedor No Existe o Error en la Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Calle = Dta.Rows(0).Item("Calle")
        Localidad = Dta.Rows(0).Item("Localidad")
        Provincia = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        Cuit = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")

        Dta.Dispose()

        Return True

    End Function
    Private Function HacerAlta(ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable, ByVal DtChequesRechazadosAux As DataTable) As Boolean

        'Graba Facturas.
        'Graba Facturas.
        Dim NumeroNota As Double
        Dim NumeroAsiento As Double
        Dim NumeroW As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion comprobante.
            NumeroNota = UltimaNumeracion(ConexionNota)
            If NumeroNota < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            DtNotaCabezaAux.Rows(0).Item("Movimiento") = NumeroNota
            For Each Row As DataRow In DtNotaDetalleAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Movimiento") = NumeroNota
                End If
            Next
            For Each Row As DataRow In DtMedioPagoAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Movimiento") = NumeroNota
                End If
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

            NumeroW = ActualizaNota("A", DtNotaCabezaAux, DtNotaDetalleAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux, DtFacturasCabezaAux, DtComprobantesCabezaAux)

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
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PNota = NumeroNota
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function HacerModificacion(ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable, ByVal DtChequesRechazadosAux As DataTable) As Boolean

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
                DtAsientoCabezaAux.Rows(0).Item("Documento") = DtNotaCabezaAux.Rows(0).Item("Movimiento")
                For Each Row As DataRow In DtAsientoDetalleAux.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            Resul = ActualizaNota("M", DtNotaCabezaAux, DtNotaDetalleAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux, DtFacturasCabezaAux, DtComprobantesCabezaAux)

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
    Private Function ActualizaNota(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtChequesRechazadosAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "OtrosPagosCabeza", ConexionNota)
                    If Resul <= 0 Then Return 0
                End If
                '
                If Not IsNothing(DtNotaDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaDetalleAux.GetChanges, "OtrosPagosDetalle", ConexionNota)
                    If Resul <= 0 Then Return 0
                End If
                '
                If Not IsNothing(DtMedioPagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "OtrosPagosPago", ConexionNota)
                    If Resul <= 0 Then Return 0
                End If
                '
                If Not IsNothing(DtFacturasCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtFacturasCabezaAux.GetChanges, "OtrasFacturasCabeza", ConexionNota)
                    If Resul <= 0 Then Return 0
                End If
                '
                If Not IsNothing(DtComprobantesCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtComprobantesCabezaAux.GetChanges, "OtrosPagosCabeza", ConexionNota)
                    If Resul <= 0 Then Return 0
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
                Return DtNotaCabezaAux.Rows(0).Item("Movimiento")
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Public Function UltimaNumeracion(ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Movimiento) FROM OtrosPagosCabeza;", Miconexion)
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
    Private Sub ActualizaComprobantes(ByVal Funcion As String, ByVal DtFacturasCabezaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable)

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
                            Case 5020
                                RowsBusqueda = DtComprobantesCabezaAux.Select("TipoNota = " & Row1("Tipo") & " AND Movimiento = " & Row1("Comprobante"))
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
                            Case 5020
                                RowsBusqueda = DtComprobantesCabezaAux.Select("TipoNota = " & Row1("Tipo") & " AND Movimiento = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                        End Select
                    End If
                End If
            Next
        End If

    End Sub
    Private Sub CalculaTotales()

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

        Select Case PTipoNota    'Esto es para que no modifique el saldo cuando se regraba la nota.(Saldo esta enlazado con Registro cabeza.
            Case 5005
                TextSaldo.Text = FormatNumber(CDbl(TextTotalRecibo.Text) - ImputacionDeOtros - TotalFacturas, GDecimales)
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
                Texto = "Razón Social: " & ComboProveedor.Text
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
                Texto = "CUIT        : " & Cuit & " " & ComboTipoIva.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
            Else
                Texto = "R. Social    : " & ComboProveedor.Text & "                Nro.: " & TextComprobante.Text
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
                'Imprime Importe.
                Texto = FormatNumber(Row("Importe"), GDecimales)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Next
            'Resuardo
            Yq = MTop + 72 + Alto + 2
            If PAbierto Then
                Texto = GNombreEmpresa & " " & TextComprobante.Text
            Else
                Texto = TextComprobante.Text
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
            'Iva.
            Dim ListaIva As New List(Of ItemIva)
            ArmaListaImportesIva(ListaIva)
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
    Private Function Valida() As Boolean

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

        If ImporteIva <> 0 Or ImporteRetencion <> 0 Then
            MsgBox("En Nota Otros Proveedores no debe Informarse IVA, Percepción o Retención. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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