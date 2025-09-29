Imports System.Transactions
Public Class UnReciboPagoFactura
    Public PNota As Double = 0
    Public PActualizacionOk As Integer
    Public PAbierto As Boolean
    Public PEsPagoEfectivo As Boolean
    Public PTipoNota As Integer = 60
    Public PEmisor As Integer
    Public PFactura As Double
    Public PEsTr As Boolean = False
    Public PEsSecos As Boolean = False
    '
    Dim DtNotaDetalle As DataTable
    Dim DtNotaCabeza As DataTable
    Dim DtMedioPago As DataTable
    Dim DtFacturasCabeza As DataTable
    Dim DtGridCompro As DataTable
    Dim DtGrid As DataTable
    Dim DtFormasPago As DataTable
    Dim DtRetencionProvincia As DataTable
    Dim DtRetencionProvinciaAux As DataTable
    '
    Private MiEnlazador As New BindingSource
    '
    Dim cb As ComboBox
    Dim ConexionNota As String
    Dim TotalFacturas As Decimal
    Dim TotalConceptos As Decimal
    Dim UltimoNumero As Double = 0
    Dim UltimoNumeroRetencion As Integer = 0
    Dim LetraIva As Integer
    Dim ExentoRetencion As Boolean
    Dim UltimaFechaW As DateTime
    Private Sub UnReciboPagoFactura_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If GCaja = 0 And PNota = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        GridCompro.AutoGenerateColumns = False
        GridCompro.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        If PAbierto Then
            ConexionNota = Conexion
        Else : ConexionNota = ConexionN
        End If

        LlenaCombosGrid()

        ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes WHERE Clave = " & PEmisor & ";")
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        Label16.Text = "Cliente"

        ArmaTipoIva(ComboTipoIva)
        LlenaComboTablas(ComboPais, 28)

        ArmaIvaProveedores(ComboIva)
        ComboIva.SelectedValue = 1

        ArmaMedioPagoCobranza(DtFormasPago, PAbierto, PNota)
        If PEsPagoEfectivo Then
            For I As Integer = DtFormasPago.Rows.Count - 1 To 0 Step -1
                If DtFormasPago.Rows(I).Item("Tipo") <> 1 And DtFormasPago.Rows(I).Item("Tipo") <> 10 And DtFormasPago.Rows(I).Item("Tipo") <> 0 Then DtFormasPago.Rows(I).Delete()
            Next
        End If

        LabelTipoNota.Text = "Recibo de Cobro"

        PActualizacionOk = False

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

        TextLetra.Visible = False

        If GGeneraAsiento Then
            Dim Conta = TieneTabla1(PTipoNota)
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

        Select Case PTipoNota
            Case 60
                UltimaFechaW = UltimaFechaPuntoVentaTipoNota(Conexion)
        End Select

        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        GridCompro.EndEdit()
        MiEnlazador.EndEdit()

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        Dim RowsBusqueda() As DataRow

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtNotaDetalleAux = DtNotaDetalle.Copy

        If PEsPagoEfectivo And PNota = 0 Then
            DtNotaCabezaAux.Rows(0).Item("ContadoEfectivo") = True
        End If

        Dim DtFacturasCabezaAux As DataTable = DtFacturasCabeza.Copy

        'Actualiza Saldo de Comprobantes Imputados.
        ActualizaComprobantes("M", DtFacturasCabezaAux)

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

        If IsNothing(DtNotaDetalleAux.GetChanges) And IsNothing(DtNotaCabezaAux.GetChanges) Then
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

        'Actualiza Archivo de distribucion retenciones por provincia.
        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone
        If PNota = 0 Then
            For Each Row As DataRow In DtRetencionProvinciaAux.Rows
                Dim Row1 As DataRow = DtRetencionProvinciaWW.NewRow
                Row1("TipoNota") = PTipoNota
                Row1("Nota") = 0
                Row1("Provincia") = Row("Provincia")
                Row1("Retencion") = Row("Retencion")
                Row1("Importe") = Row("Importe")
                DtRetencionProvinciaWW.Rows.Add(Row1)
            Next
        End If

        If HacerAlta(DtNotaCabezaAux, DtFacturasCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW) Then
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If GCaja = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtNotaDetalle.Rows.Count = 0 Or DtNotaDetalle.Rows.Count > 1 Then
            MsgBox("Nota Tiene Otras Imputaciones. Solo se Anula Por Tesoreria. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If DtNotaDetalle.Rows(0).Item("TipoComprobante") <> 2 Then
            MsgBox("Nota Tiene Otras Imputaciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If DtNotaDetalle.Rows(0).Item("TipoComprobante") = 2 And DtNotaDetalle.Rows(0).Item("Comprobante") <> PFactura Then
            MsgBox("Nota Tiene Otras Imputaciones. Solo se Anula Por Tesoreria. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy

        'Actualiza Saldo de Comprobantes Imputados.
        Dim DtFacturasCabezaAux As DataTable = DtFacturasCabeza.Copy

        ActualizaComprobantes("M", DtFacturasCabezaAux)

        If Not IsNothing(DtFacturasCabezaAux.GetChanges) Then
            MsgBox("Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Resul As Double

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        For Each Row As DataRow In DtGrid.Rows
            If IsDBNull(Row("MedioPago")) Then Exit For
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

        If MsgBox("Nota se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        'Actualiza Saldo de Comprobantes Imputados.
        DtFacturasCabezaAux = DtFacturasCabeza.Copy

        ActualizaComprobantes("B", DtFacturasCabezaAux)

        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        '
        If Not HallaAsientosCabeza(PTipoNota, DtNotaCabeza.Rows(0).Item("Nota"), DtAsientoCabezaAux, ConexionNota) Then Exit Sub
        For Each Row As DataRow In DtAsientoCabezaAux.Rows
            Row("Estado") = 3
        Next

        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone

        DtNotaCabezaAux.Rows(0).Item("Estado") = 3
        DtNotaDetalleAux.Rows(0).Delete()
        DtFacturasCabezaAux.Rows(0).Item("Recibo") = 0

        Resul = ActualizaNota("B", DtGrid, DtNotaCabezaAux, DtFacturasCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW)
        If Resul = -1 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Baja Fue Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PActualizacionOk = True
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

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

        UnaImpresionReciboW(PTipoNota, PAbierto, DtNotaCabeza, DtGrid, GridCompro, DtFormasPago, False, TextCuit.Text, ComboEmisor.Text, "")

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PNota = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = PTipoNota
        If PAbierto Then
            ListaAsientos.PDocumentoB = PNota
        Else
            ListaAsientos.PDocumentoN = PNota
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub PictureLupaCuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupaCuenta.Click

        If PNota <> 0 Then Exit Sub

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
    Private Sub ButtonMediosDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMediosDePago.Click

        Dim DtFormasPagoW As DataTable = DtFormasPago.Copy

        'No permite informar en grid si total cobranza no esta informado.  
        If PTipoNota = 60 Then
            If TextImporteCobro.Text = "" Then
                MsgBox("Falta Informar Importe de la Cobranza.")
                TextImporteCobro.Focus()
                Exit Sub
            End If
            If CDec(TextImporteCobro.Text) = 0 Then
                MsgBox("Falta Informar Importe de la Cobranza.")
                TextImporteCobro.Focus()
                Exit Sub
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
        UnMediosPago.PMoneda = 1
        UnMediosPago.PCambio = 1
        UnMediosPago.PDtRetencionesAutomaticas = New DataTable
        UnMediosPago.PImporte = CDbl(TextImporteCobro.Text)
        If TextImporteCobro.Text <> "" Then
            UnMediosPago.PImporteAInformar = CDbl(TextImporteCobro.Text)
        End If
        UnMediosPago.ShowDialog()
        DtGrid = UnMediosPago.PDtGrid

        UnMediosPago.Dispose()

        CalculaTotales()

    End Sub
    Private Sub ButtonEliminarTodo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarTodo.Click

        If MsgBox("Medios Pagos se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtGrid.Clear()
        CalculaTotales()

    End Sub
    Private Function ArmaArchivos() As Boolean                            'ArmaArchivos

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()
        CreaDtGridCompro()
        CreaDtRetencionProvinciaAux()

        Dim Sql As String
        Dim Row1 As DataRow
        Dim RowsBusqueda() As DataRow

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM RecibosCabeza WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        If PNota <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then AgregaCabeza()

        MuestraCabeza()

        ComboEmisor.SelectedValue = DtNotaCabeza.Rows(0).Item("Emisor")
        If Not LlenaDatosEmisor(ComboEmisor.SelectedValue) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False

        DtRetencionProvincia = New DataTable
        If PAbierto Then
            If Not Tablas.Read("SELECT * FROM RecibosRetenciones WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";", Conexion, DtRetencionProvincia) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
            For Each Row As DataRow In DtRetencionProvincia.Rows
                Row1 = DtRetencionProvinciaAux.NewRow
                Row1("Retencion") = Row("Retencion")
                Row1("Provincia") = Row("Provincia")
                Row1("Importe") = Row("Importe")
                DtRetencionProvinciaAux.Rows.Add(Row1)
            Next
        End If

        DtNotaDetalle = New DataTable
        Sql = "SELECT * FROM RecibosDetalle WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM RecibosDetallePago WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtMedioPago) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False

        Dim DtCheques As New DataTable
        For Each Row As DataRow In DtMedioPago.Rows
            If Row("ClaveCheque") <> 0 Then
                If PEsTr Then
                    If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionN, DtCheques) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
                Else
                    If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionNota, DtCheques) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
                End If
            End If
        Next

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
            Row1("ClaveInterna") = Row("ClaveInterna")
            Row1("ClaveCheque") = Row("ClaveCheque")
            Row1("ClaveChequeVisual") = Row("ClaveCheque")
            If Row("MedioPago") = 2 Then Row1("ClaveChequeVisual") = 0
            If Row("ClaveCheque") <> 0 Then
                RowsBusqueda = DtCheques.Select("MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque"))
                If RowsBusqueda.Length <> 0 Then
                    Row1("Banco") = RowsBusqueda(0).Item("Banco")
                    Row1("Cuenta") = RowsBusqueda(0).Item("Cuenta")
                    Row1("Serie") = RowsBusqueda(0).Item("Serie")
                    Row1("Numero") = RowsBusqueda(0).Item("Numero")
                    Row1("EmisorCheque") = RowsBusqueda(0).Item("EmisorCheque")
                    Row1("Fecha") = RowsBusqueda(0).Item("Fecha")
                Else
                    Row1("Banco") = 0
                    Row1("Cuenta") = 0
                    Row1("Serie") = ""
                    Row1("Numero") = 0
                    Row1("EmisorCheque") = "Comp.No Existe."
                    Row1("Fecha") = "1/1/1800"
                End If
            Else
                Row1("Banco") = Row("Banco")
                Row1("Cuenta") = Row("Cuenta")
                Row1("Serie") = ""
                Row1("Numero") = 0
                Row1("EmisorCheque") = ""
                Row1("Fecha") = "1/1/1800"
            End If
            Row1("TieneLupa") = False
            If PAbierto Then
                RowsBusqueda = DtRetencionProvincia.Select("Retencion = " & Row("MedioPago"))
                If RowsBusqueda.Length <> 0 Then Row1("TieneLupa") = True
            End If
            DtGrid.Rows.Add(Row1)
        Next

        DtCheques.Dispose()

        'Muestra Factura a Imputar.
        DtFacturasCabeza = New DataTable

        If Not ArmaConFacturas() Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False

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
            Row1("Recibo") = Row("Factura")
            Row1("Fecha") = Row("Fecha")
            Row1("Importe") = Row("Importe")
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

        'TextImporteCobro.Text = DtFacturasCabeza.Rows(0).Item("Importe")
        If PEsPagoEfectivo Then
            TextImporteCobro.Text = DtFacturasCabeza.Rows(0).Item("Importe")
            TextImporteCobro.ReadOnly = True
        Else
            TextImporteCobro.ReadOnly = False
        End If

        GridCompro.DataSource = DtGridCompro
        If PEsPagoEfectivo Then
            Dim chkCell As DataGridViewCheckBoxCell = GridCompro.Rows(0).Cells("Seleccion")
            chkCell.Value = True
        End If

        If PNota <> 0 Then
            GridCompro.Enabled = False
            Panel3.Enabled = False
            Panel4.Enabled = False
            ButtonAceptar.Text = "Modificar Recibo"
            LabelPuntoDeVenta.Visible = False
        Else
            GridCompro.Enabled = True
            Panel3.Enabled = True
            Panel4.Enabled = True
            ButtonAceptar.Text = "Grabar Recibo"
            LabelPuntoDeVenta.Visible = True
        End If

        If PEsPagoEfectivo And PNota = 0 Then PagoEfectivo()

        CalculaTotales()

        AddHandler DtGridCompro.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGridCompro_ColumnChanging)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub PagoEfectivo()

        Dim Row As DataRow = DtGrid.NewRow
        Row("Item") = 0
        Row("MedioPago") = 1
        Row("Detalle") = ""
        Row("Neto") = 0
        Row("Alicuota") = 0
        Row("Bultos") = 0
        Row("ImporteIva") = 0
        Row("Banco") = 0
        Row("Fecha") = "1/1/1800"
        Row("Cuenta") = 0
        Row("Serie") = ""
        Row("Numero") = 0
        Row("EmisorCheque") = ""
        Row("Cambio") = 0
        Row("Importe") = DtGridCompro.Rows(0).Item("Importe")
        Row("Comprobante") = 0
        Row("FechaComprobante") = "1/1/1800"
        Row("ClaveCheque") = 0
        Row("ClaveInterna") = 0
        Row("ClaveChequeVisual") = 0
        Row("TieneLupa") = False
        DtGrid.Rows.Add(Row)

        DtGridCompro.Rows(0).Item("Asignado") = DtGridCompro.Rows(0).Item("Importe")
        DtGridCompro.Rows(0).Item("Saldo") = 0

        GridCompro.Enabled = False
        ButtonEliminarTodo.Enabled = False

    End Sub
    Private Function ArmaConFacturas() As Boolean

        Dim Sql As String = ""

        Sql = "SELECT * FROM FacturasCabeza WHERE Estado <> 0 AND FacturasCabeza.Factura = " & PFactura & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtFacturasCabeza) Then Return False

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

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextImporteCobro.DataBindings.Clear()
        TextImporteCobro.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "CodigoIva")
        ComboIva.DataBindings.Clear()
        ComboIva.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Saldo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextSaldo.DataBindings.Clear()
        TextSaldo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatMaskedNota(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "000000000000")

    End Sub
    Private Sub ParseMaskedNota(ByVal sender As Object, ByVal Numero As ConvertEventArgs)


    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

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
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos
        Dim RowsBusqueda() As DataRow

        If Funcion = "A" Then
            For Each Row As DataRow In DtGrid.Rows
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                If RowsBusqueda(0).Item("Tipo") <> 4 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("MedioPago")
                    If Row("Cambio") <> 0 Then
                        Item.Importe = Trunca(Row("Cambio") * Row("Importe"))
                    Else : Item.Importe = Row("Importe")
                    End If
                    ListaConceptos.Add(Item)
                End If
            Next
            For Each Row As DataRow In DtGrid.Rows
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                If RowsBusqueda(0).Item("Tipo") = 4 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("MedioPago")
                    Item.Importe = Row("Importe")
                    If PTipoNota = 60 Then
                        Item.TipoIva = 9        'Credito fiscal.
                    Else : Item.TipoIva = 11    'Debito fiscal. 
                    End If
                    ListaRetenciones.Add(Item)
                End If
            Next
            Item = New ItemListaConceptosAsientos
            Item.Clave = 213
            Item.Importe = CDbl(TextImporteCobro.Text)
            ListaConceptos.Add(Item)
        End If
        '
        'Arma Lista con Cuentas definidas en documento.
        If ListCuentas.Visible Then
            If ListCuentas.Items.Count = 0 Then
                MsgBox("Falta Informar Cuenta. Operación se CANCELA.")
                Return False
            End If
            Dim ImporteB As Double
            For I As Integer = 0 To ListCuentas.Items.Count - 1
                Dim Fila As New ItemCuentasAsientos
                Dim Cuenta As String = ListCuentas.Items.Item(I).SubItems(0).Text
                Fila.Cuenta = Mid$(Cuenta, 1, 3) & Mid$(Cuenta, 5, 6) & Mid$(Cuenta, 12, 2)
                Fila.Importe = CDbl(ListCuentas.Items.Item(I).SubItems(1).Text)
                Fila.Clave = 202
                ListaCuentas.Add(Fila)
                ImporteB = Trunca(ImporteB + CDbl(ListCuentas.Items.Item(I).SubItems(1).Text))
            Next
            Dim Neto As Double
            For Each Row As DataRow In DtGrid.Rows
                Neto = Trunca(Neto + Row("Neto"))
            Next
            If ImporteB <> Neto Then
                MsgBox("Importe de Cuentas Informada Difiere del Neto de la Factura")
                Return False
            End If
        End If
        '
        Dim MontoAfectado As Double
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
        '
        If MontoAfectado <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = 214
            Item.Importe = MontoAfectado
            ListaConceptos.Add(Item)
        End If

        If MontoAfectado = 0 And Funcion = "M" Then Return True

        If Not Asiento(PTipoNota, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

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
        Row("CodigoIva") = ComboIva.SelectedValue
        Row("Estado") = 1
        Row("Caja") = GCaja
        Row("Moneda") = 1
        Row("Cambio") = 1
        Row("Secos") = PEsSecos
        Row("Tr") = PEsTr
        DtNotaCabeza.Rows.Add(Row)

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
        Importe.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Importe)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Saldo)

        Dim SaldoAnt As New DataColumn("SaldoAnt")
        SaldoAnt.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(SaldoAnt)

        Dim Asignado As New DataColumn("Asignado")
        Asignado.DataType = System.Type.GetType("System.Double")
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
        Importe.DataType = System.Type.GetType("System.Double")
        DtRetencionProvinciaAux.Columns.Add(Importe)

    End Sub
    Private Function LlenaDatosEmisor(ByVal Cliente As Integer) As Boolean

        Dim Dta As New DataTable
        Dim Sql As String = ""

        Sql = "SELECT * FROM Clientes WHERE Clave = " & ComboEmisor.SelectedValue & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, Cliente No Existe o Error en la Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        TextCalle.Text = Dta.Rows(0).Item("Calle")
        TextLocalidad.Text = Dta.Rows(0).Item("Localidad")
        TextProvincia.Text = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
        TextTelefonos.Text = Dta.Rows(0).Item("Telefonos")
        TextFaxes.Text = Dta.Rows(0).Item("Faxes")
        TextCuit.Text = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")
        LetraIva = Dta.Rows(0).Item("TipoIva")
        TextLetra.Text = LetraTipoIva(LetraIva)

        Dta.Dispose()

        GPuntoDeVenta = HallaPuntoVentaSegunTipo(PTipoNota, LetraIva)
        If GPuntoDeVenta = 0 Then
            MsgBox("No tiene Definido Punto de Venta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Not ExistePuntoDeVenta(GPuntoDeVenta) Then
            MsgBox("Punto de Venta " & GPuntoDeVenta & " No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If EsPuntoDeVentaZ(GPuntoDeVenta) Then
            MsgBox("Punto de Venta del Operador " & GPuntoDeVenta & " Reservado Para Ticket Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        LabelPuntoDeVenta.Text = "Punto de Venta " & Format(GPuntoDeVenta, "0000")

        Return True

    End Function
    Private Sub CalculaTotales()

        TotalConceptos = 0
        For Each Row As DataRow In DtGrid.Rows
            TotalConceptos = TotalConceptos + Row("Importe")
        Next

        TextTotalRecibo.Text = FormatNumber(TotalConceptos, GDecimales)

        If PNota = 0 Then TextLibreParaImputar.Text = FormatNumber(TotalConceptos, 2)

        TotalFacturas = 0
        For Each Row As DataRow In DtGridCompro.Rows
            If Row.RowState <> DataRowState.Deleted Then
                TotalFacturas = TotalFacturas + Row("Asignado")
            End If
        Next

        TotalFacturas = Trunca(TotalFacturas)

        TextTotalFacturas.Text = FormatNumber(TotalFacturas, GDecimales)

        TextSaldo.Text = FormatNumber(CDec(TextTotalRecibo.Text) - TotalFacturas, GDecimales)

    End Sub
    Private Function HacerAlta(ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroNota As Double
        Dim NumeroAsiento As Double
        Dim NumeroW As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            NumeroNota = UltimaNumeracionPagoYOrden(PTipoNota, ConexionNota)
            If NumeroNota < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
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

            DtFacturasCabezaAux.Rows(0).Item("Recibo") = NumeroNota

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

            NumeroW = ActualizaNota("A", DtGridAux, DtNotaCabezaAux, DtFacturasCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -10 Then
            MsgBox("Factura Ya Fue Impresa. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
            PActualizacionOk = True
            Return True
        End If

    End Function
    Private Function HacerModificacion(ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable) As Boolean

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

            Resul = ActualizaNota("M", DtGrid, DtNotaCabezaAux, DtFacturasCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtRetencionProvinciaWW)

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
            PActualizacionOk = True
            Return True
        End If

    End Function
    Private Function ActualizaNota(ByVal Funcion As String, ByVal DtGridAux As DataTable, ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaWW As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
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
    Private Sub ActualizaComprobantes(ByVal Funcion As String, ByVal DtFacturasCabezaAux As DataTable)

        Dim RowsBusqueda() As DataRow

        'Actualiza Saldo de Comprobantes Imputados.

        If Funcion = "M" Then
            For Each Row1 As DataRow In DtGridCompro.Rows
                If Row1.RowState <> DataRowState.Deleted Then
                    If Row1("Saldo") <> Row1("SaldoAnt") Then
                        RowsBusqueda = DtFacturasCabezaAux.Select("Factura = " & Row1("Comprobante"))
                        RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                        If PEsPagoEfectivo Then
                            RowsBusqueda(0).Item("FormaPago") = 4
                        Else
                            RowsBusqueda(0).Item("FormaPago") = 3
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
                        RowsBusqueda = DtFacturasCabezaAux.Select("Factura = " & Row1("Comprobante"))
                        RowsBusqueda(0).Item("Saldo") = RowsBusqueda(0).Item("Saldo") + Row1.Item("Asignado")
                        RowsBusqueda(0).Item("FormaPago") = 2
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
    Private Function EsContadoEfectivo() As Boolean

        For Each Row As DataRow In DtGrid.Rows
            If Row("Mediopago") <> 1 And Row("MedioPago") <> 5 Then Return False
        Next

        If DtFacturasCabeza.Rows(0).Item("Importe") = CDbl(TextImporteCobro.Text) Then Return True

        Return False

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Tipo.DataSource = DtTiposComprobantes(PAbierto)
        Row = Tipo.DataSource.NewRow()
        Row("Codigo") = 44
        Row("Nombre") = "Ticket Fiscal"
        Tipo.DataSource.Rows.Add(Row)
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Codigo"

    End Sub
    Private Function Valida() As Boolean

        Select Case PTipoNota
            Case 50, 70, 500, 700
            Case Else
                If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 Then
                    MsgBox("Fecha Menor a la Fecha del ultimo Recibo Grabado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    DateTime1.Focus()
                    Return False
                End If
        End Select

        If TotalConceptos = 0 Then
            MsgBox("Debe informar Importes. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TotalFacturas = 0 Then
            MsgBox("Debe Informar Importes a Imputar. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TotalConceptos - TotalFacturas < 0 Then
            MsgBox("Importes Imputados supera importe de Pago. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PEsPagoEfectivo And Not EsContadoEfectivo() Then
            MsgBox("Pago No Corresponde a un Pago Contado Efectivo. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        Dim ImporteRetencion As Double = 0
        For Each Row1 As DataRow In DtGrid.Rows
            If Not IsNothing(Row1("MedioPago")) Then
                If Row1("TieneLupa") Then
                    ImporteRetencion = ImporteRetencion + Row1("Importe")
                End If
            End If
        Next
        If ImporteRetencion <> 0 Then
            If DtRetencionProvinciaAux.Rows.Count = 0 Then
                MsgBox("Falta Distribuir Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            Dim ImporteDistribuido As Double
            For Each Row As DataRow In DtRetencionProvinciaAux.Rows
                ImporteDistribuido = ImporteDistribuido + Row("Importe")
            Next
            If ImporteDistribuido <> ImporteRetencion Then
                MsgBox("Incorrecta Distribución Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If Not PEsPagoEfectivo Then
            If CDec(TextTotalRecibo.Text) > DtFacturasCabeza.Rows(0).Item("Importe") Then
                If MsgBox("Total Recibo mayor que el importe de la Factura. Quiere Continuar Igualmente? (Si/No) ", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
            End If
        End If

        Return True

    End Function
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
                Diferencia = TotalConceptos - TotalFacturas - GridCompro.Rows(e.RowIndex).Cells("Asignado").Value
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
                If Not IsNumeric(CType(sender, TextBox).Text) Then
                    CType(sender, TextBox).Text = ""
                    CType(sender, TextBox).Focus()
                End If
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
            If TotalFacturas - e.Row("Asignado") + CDec(e.ProposedValue) > TotalConceptos Then
                MsgBox("Importe Documentos Supera Total del Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Asignado")
                Exit Sub
            End If
            '
            SaldoAnt = e.Row("Saldo")
            '
            e.Row("Saldo") = e.Row("Saldo") + e.Row("Asignado") - CDec(e.ProposedValue)
            '
            If e.Row("Saldo") < 0 Then
                MsgBox("Imputación Supera el Saldo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Asignado")
                e.Row("Saldo") = SaldoAnt
                Exit Sub
            End If
        End If

        CalculaTotales()

    End Sub


    
   
End Class