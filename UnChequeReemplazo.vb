Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnChequeReemplazo
    Public PNota As Double
    Public PAbierto As Boolean
    Public PTipoNota As Integer
    Public PEmisor As Integer
    Public PBloqueaFunciones As Boolean
    Public PMedioPagoAReemplazar As Integer
    Public PClaveChequeAReemplazar As Integer
    Public PCompDestino As Double
    Public POrigenDestino As Double
    Public PNumeroFondoFijo As Double
    '
    Dim DtNotaCabeza As DataTable
    Dim DtMedioPago As DataTable
    Dim DtGrid As DataTable
    Dim DtFormasPago As DataTable
    Dim DtChequeAReemplazar As DataTable
    Dim DtReemplazo As DataTable
    '
    Private MiEnlazador As New BindingSource
    '
    Dim PEsTr As Boolean = False
    Dim PManual As Boolean = False
    Dim cb As ComboBox
    Dim ConexionNota As String
    Dim TotalConceptos As Double
    Dim UltimoNumero As Double = 0
    Dim LetraIva As Integer
    Dim UltimaFechaW As DateTime
    Dim TipoAsiento As Integer
    Dim Prestamo As Double
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim LineasParaImpresion As Integer = 0
    Private Sub UnChequeReemplazo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If GCaja = 0 And PNota = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PNota = 0 Then
            Select Case PTipoNota
                Case 600, 65, 64
                    PideDatosEmisor()
            End Select
        End If

        If PAbierto Then
            ConexionNota = Conexion
        Else : ConexionNota = ConexionN
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

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

        Select Case PTipoNota
            Case 600
                If POrigenDestino = 6 Then
                    ArmaMedioPagoOrdenFondoFijo(DtFormasPago)
                Else
                    ArmaMedioPagoOrden(DtFormasPago, False, PNota)
                End If
                LabelTipoNota.Text = "Orden de Pago"
            Case 65
                ArmaMedioPagoDevolucionSenia(DtFormasPago)
                LabelTipoNota.Text = "Devolución Seña"
            Case 64
                ArmaMedioPagoOrdenPagoClientes(DtFormasPago)
                LabelTipoNota.Text = "Devolución a Cliente"
            Case 1010
                ArmaMedioPagoPrestamo(DtFormasPago)
                LabelTipoNota.Text = "Cancelación Prestamo"
            Case 4010
                ArmaMedioPagoSueldo(DtFormasPago)
                LabelTipoNota.Text = "Orden de Pago - Sueldos"
            Case 5010
                ArmaMedioPagoOrdenOtrosPagos(DtFormasPago, PAbierto)
                LabelTipoNota.Text = "Otros Pagos"
            Case 6000
                ArmaMedioPagoCobroDivisas(DtFormasPago)
                LabelTipoNota.Text = "Compra Divisas"
            Case 7001
                ArmaMedioPagoOrdenFondoFijo(DtFormasPago)
                LabelTipoNota.Text = "Ajuste Fondo Fijo"
        End Select

        ArmaArchivos()

        Select Case PTipoNota
            Case 600, 4010, 5010, 65, 64
                If PTipoNota = 600 And POrigenDestino = 6 Then
                    TipoAsiento = 7205
                Else
                    TipoAsiento = PTipoNota
                End If
            Case 1010
                TipoAsiento = 8001
            Case 6000
                TipoAsiento = 16000
            Case 7001
                TipoAsiento = 7201
            Case Else
                MsgBox(PTipoNota & " No Comtemplado")
                Me.Close() : Exit Sub
        End Select

        UltimaFechaW = UltimaFecha(Conexion)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PNota <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
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

        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtChequeAReemplazarAux As DataTable = DtChequeAReemplazar.Copy
        Dim DtReemplazoAux As DataTable = DtReemplazo.Copy

        'Actualiza Notas Cabeza.
        DtNotaCabezaAux.Rows(0).Item("Importe") = 0

        If IsNothing(DtNotaCabezaAux.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtAsientoCabezaAux, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        Select Case PTipoNota
            Case 600, 65, 1010, 4010, 5010, 6000, 7001, 64
                DtReemplazoAux.Rows(0).Item("TipoNotaReemplazada") = DtChequeAReemplazar.Rows(0).Item("TipoDestino")
                DtReemplazoAux.Rows(0).Item("NotaReemplazada") = DtChequeAReemplazar.Rows(0).Item("CompDestino")
                DtReemplazoAux.Rows(0).Item("TipoNotaReemplazante") = PTipoNota
            Case Else
                MsgBox(PTipoNota & " No Comtemplado")
                Exit Sub
        End Select

        Select Case PTipoNota
            Case 600, 65, 1010, 4010, 5010, 6000, 7001, 64
                If PMedioPagoAReemplazar = 3 Then
                    DtChequeAReemplazarAux.Rows(0).Item("TipoDestino") = 0
                    DtChequeAReemplazarAux.Rows(0).Item("CompDestino") = 0
                Else
                    DtChequeAReemplazarAux.Rows(0).Item("Estado") = 3
                End If
            Case Else
                MsgBox(PTipoNota & " No Comtemplado")
                Exit Sub
        End Select
        '
        Select Case PTipoNota
            Case 600, 65, 64
                If HacerAlta600(DtNotaCabezaAux, DtMedioPagoAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux) Then
                    ArmaArchivos()
                End If
            Case 1010
                If HacerAlta1010(DtNotaCabezaAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux) Then
                    ArmaArchivos()
                End If
            Case 4010
                If HacerAlta4010(DtNotaCabezaAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux) Then
                    ArmaArchivos()
                End If
            Case 5010
                If HacerAlta5010(DtNotaCabezaAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux) Then
                    ArmaArchivos()
                End If
            Case 6000
                If HacerAlta6000(DtNotaCabezaAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux) Then
                    ArmaArchivos()
                End If
            Case 7001
                If HacerAlta7001(DtNotaCabezaAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux) Then
                    ArmaArchivos()
                End If
        End Select

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

        If CInt(LabelCaja.Text) <> GCaja Then
            MsgBox("Modificación Solo Permitida para Caja " & LabelCaja.Text & ". Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtChequeAReemplazarAux As DataTable = DtChequeAReemplazar.Copy
        Dim DtReemplazoAux As DataTable = DtReemplazo.Copy

        Dim Resul As Double

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Select Case PTipoNota
            Case 600, 1010, 65, 4010, 5010, 6000, 7001, 64
                For Each Row As DataRow In DtGrid.Rows
                    If IsDBNull(Row("MedioPago")) Then Exit For
                    If Row("MedioPago") = 2 Or Row("MedioPago") = 3 Then
                        Dim Anulado As Boolean, Rechazado As Boolean, Depositado As Boolean, Entregado As Boolean, Afectado As Boolean
                        If Not EstadoCheque(Row("MedioPago"), Row("ClaveCheque"), ConexionNota, Anulado, Rechazado, Depositado, Entregado, Afectado) Then
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
                        If Not PEsTr Then
                            If Afectado Then
                                Me.Cursor = System.Windows.Forms.Cursors.Default
                                MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Esta Afectado a Otra Orden. Operación se CANCELA.")
                                Exit Sub
                            End If
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
        End Select

        For Each Row As DataRow In DtGrid.Rows
            If IsDBNull(Row("MedioPago")) Then Exit For
            If Row("MedioPago") = 2 Or Row("MedioPago") = 3 Then
                If ChequeReemplazado(Row("MedioPago"), Row("ClaveCheque"), PTipoNota, PNota, ConexionNota) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Esta Reemplazado. Operación se CANCELA.")
                    Exit Sub
                End If
            End If
        Next

        Select Case PTipoNota
            Case 600, 65, 1010, 4010, 5010, 6000, 7001, 64
                If PMedioPagoAReemplazar = 3 Then
                    DtChequeAReemplazarAux.Rows(0).Item("TipoDestino") = DtReemplazo.Rows(0).Item("TipoNotaReemplazada")
                    DtChequeAReemplazarAux.Rows(0).Item("CompDestino") = DtReemplazo.Rows(0).Item("NotaReemplazada")
                Else
                    DtChequeAReemplazarAux.Rows(0).Item("Estado") = 1
                End If
            Case Else
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox(PTipoNota & " No Comtemplado")
                Exit Sub
        End Select

        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        '
        Select Case PTipoNota
            Case 600, 65, 64
                If Not HallaAsientosCabeza(TipoAsiento, DtNotaCabeza.Rows(0).Item("Nota"), DtAsientoCabezaAux, ConexionNota) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            Case 1010, 4010, 5010, 6000, 7001
                If Not HallaAsientosCabeza(TipoAsiento, DtNotaCabeza.Rows(0).Item("Movimiento"), DtAsientoCabezaAux, ConexionNota) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            Case Else
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox(PTipoNota & " No Comtemplado")
                Exit Sub
        End Select

        For Each Row As DataRow In DtAsientoCabezaAux.Rows
            Row("Estado") = 3
        Next

        If MsgBox("Nota se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtReemplazoAux.Rows(0).Delete()
        DtNotaCabezaAux.Rows(0).Item("Estado") = 3

        Select Case PTipoNota
            Case 600, 65, 64
                Resul = ActualizaNota600("B", DtGrid, DtNotaCabezaAux, DtMedioPagoAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux)
            Case 1010
                Resul = ActualizaNota1010("B", DtNotaCabezaAux, DtMedioPagoAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux)
            Case 4010
                Resul = ActualizaNota4010("B", DtNotaCabezaAux, DtMedioPagoAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux)
            Case 5010
                Resul = ActualizaNota5010("B", DtNotaCabezaAux, DtMedioPagoAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux)
            Case 6000
                Resul = ActualizaNota6000("B", DtNotaCabezaAux, DtMedioPagoAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux)
            Case 7001
                Resul = ActualizaNota7001("B", DtNotaCabezaAux, DtMedioPagoAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux)
        End Select

        If Resul = -1 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -2 Then
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
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Movimiento debe ser Grabado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ErrorImpresion = False
        Paginas = 0
        Copias = 1

        Dim print_document As New PrintDocument
        AddHandler print_document.PrintPage, AddressOf Print_PrintOrdenPago

        print_document.Print()

    End Sub
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
    Private Sub ButtonMediosDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMediosDePago.Click

        Dim DtFormasPagoW As DataTable = DtFormasPago.Copy

        Dim TipoW As Integer = PTipoNota

        Select Case PTipoNota
            Case 5010
                TipoW = 600
            Case 7001
                TipoW = 600
        End Select

        UnMediosPago.PTipoNota = TipoW
        UnMediosPago.PEmisor = PEmisor
        UnMediosPago.PDtGrid = DtGrid
        UnMediosPago.PAbierto = PAbierto
        If PNota = 0 Then
            UnMediosPago.PBloqueaFunciones = False
        Else : UnMediosPago.PBloqueaFunciones = True
        End If
        UnMediosPago.PDtFormasPago = DtFormasPagoW
        UnMediosPago.PDtRetencionProvincia = New DataTable
        If DtNotaCabeza.Rows(0).Item("ChequeRechazado") <> 0 Then UnMediosPago.PEsChequeRechazado = True
        UnMediosPago.PMoneda = 1
        UnMediosPago.PCambio = 1
        UnMediosPago.PDtRetencionesAutomaticas = New DataTable
        UnMediosPago.PImporte = CDbl(TextImporteCheque.Text)
        UnMediosPago.PImporteAInformar = CDbl(TextImporteCheque.Text)
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
    Private Sub ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()

        Dim Sql As String
        Dim Row1 As DataRow
        Dim RowsBusqueda() As DataRow

        Select Case PTipoNota
            Case 600, 65, 64
                ArmaArchivos600()
            Case 1010
                ArmaArchivos1010()
            Case 4010
                ArmaArchivos4010()
            Case 5010
                ArmaArchivos5010()
            Case 6000
                ArmaArchivos6000()
            Case 7001
                ArmaArchivos7001()
        End Select

        DtReemplazo = New DataTable
        Sql = "SELECT * FROM Reemplazos WHERE TipoNotaReemplazante = " & PTipoNota & " AND NotaReemplazante = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtReemplazo) Then Me.Close() : Exit Sub
        If PNota = 0 Then
            Dim Row2 As DataRow = DtReemplazo.NewRow
            Row2("TipoNotaReemplazante") = 0
            Row2("NotaReemplazante") = 0
            Row2("TipoNotaReemplazada") = 0
            Row2("NotaReemplazada") = 0
            DtReemplazo.Rows.Add(Row2)
        End If

        Dim DtCheques As New DataTable
        For Each Row As DataRow In DtMedioPago.Rows
            If Row("ClaveCheque") <> 0 And (Row("MedioPago") = 2 Or Row("MedioPago") = 3) Then
                If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionNota, DtCheques) Then Me.Close() : Exit Sub
            End If
        Next

        For Each Row As DataRow In DtMedioPago.Rows
            Row1 = DtGrid.NewRow
            Row1("Item") = Row("Item")
            Row1("MedioPago") = Row("MedioPago")
            Row1("Importe") = Row("Importe")
            Row1("Cambio") = Row("Cambio")
            Select Case PTipoNota
                Case 6000, 7001
                    Row1("Detalle") = ""
                    Row1("Alicuota") = 0
                    Row1("Neto") = 0
                    Row1("ImporteIva") = 0
                Case Else
                    Row1("Detalle") = Row("Detalle")
                    Row1("Alicuota") = Row("Alicuota")
                    Row1("Neto") = Row("Neto")
                    Row1("ImporteIva") = Row("Neto") * Row("Alicuota") / 100
            End Select
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            Select Case PTipoNota
                Case 600, 65, 7001, 64
                    Row1("ClaveInterna") = Row("ClaveInterna")
                Case 1010, 4010, 5010, 6000
                    Row1("ClaveInterna") = 0
                Case Else
                    Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
                    MsgBox(PTipoNota & " No Comtemplado")
                    Me.Close()
                    Exit Sub
            End Select
            Row1("ClaveChequeVisual") = 0
            If Row("MedioPago") = 3 Then Row1("ClaveChequeVisual") = Row("ClaveCheque")
            If Row("ClaveCheque") <> 0 And (Row("MedioPago") = 2 Or Row("MedioPago") = 3) Then
                RowsBusqueda = DtCheques.Select("MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque"))
                If RowsBusqueda.Length <> 0 Then
                    Row1("Banco") = RowsBusqueda(0).Item("Banco")
                    Row1("Cuenta") = RowsBusqueda(0).Item("Cuenta")
                    Row1("Serie") = RowsBusqueda(0).Item("Serie")
                    Row1("Numero") = RowsBusqueda(0).Item("Numero")
                    Row1("EmisorCheque") = RowsBusqueda(0).Item("EmisorCheque")
                    Row1("Fecha") = RowsBusqueda(0).Item("Fecha")
                    Row1("eCheq") = RowsBusqueda(0).Item("eCheq")
                Else
                    Row1("Banco") = 0
                    Row1("Cuenta") = 0
                    Row1("Serie") = ""
                    Row1("Numero") = 0
                    Row1("EmisorCheque") = ""
                    Row1("Fecha") = "1/1/1800"
                    Row1("eCheq") = 0
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
            DtGrid.Rows.Add(Row1)
        Next

        DtCheques.Dispose()

        'Ver si es por reemplazo de cheque.
        PClaveChequeAReemplazar = DtNotaCabeza.Rows(0).Item("ClaveChequeReemplazado")
        PMedioPagoAReemplazar = DtNotaCabeza.Rows(0).Item("MedioPagoRechazado")
        If PMedioPagoAReemplazar = 0 Then PMedioPagoAReemplazar = 2 'Para compatibilizar con los reemplazos anteriores al arreglo.
        If PClaveChequeAReemplazar <> 0 Then
            DtChequeAReemplazar = New DataTable
            If Not Tablas.Read("Select * FROM Cheques WHERE MedioPago = " & PMedioPagoAReemplazar & " AND ClaveCheque = " & PClaveChequeAReemplazar & ";", ConexionNota, DtChequeAReemplazar) Then Me.Close() : Exit Sub
            If PNota = 0 Then
                If PMedioPagoAReemplazar = 2 Then
                    TextComentario.Text = "Reemp.Cheque " & DtChequeAReemplazar.Rows(0).Item("Numero") & " " & NombreBanco(DtChequeAReemplazar.Rows(0).Item("Banco"))
                Else
                    TextComentario.Text = "Reemp.Cheque " & PClaveChequeAReemplazar
                End If
                TextComentario.Text = Strings.Left(TextComentario.Text, 30)
            End If
            If PMedioPagoAReemplazar = 2 Then
                TextCheque.Text = DtChequeAReemplazar.Rows(0).Item("Numero")
            Else
                TextCheque.Text = DtChequeAReemplazar.Rows(0).Item("Numero") & " (Clave " & PClaveChequeAReemplazar & ")"
            End If
            TextBanco.Text = NombreBanco(DtChequeAReemplazar.Rows(0).Item("Banco"))
            TextImporteCheque.Text = FormatNumber(DtChequeAReemplazar.Rows(0).Item("Importe"), GDecimales)
        End If

        If PNota <> 0 Then
            LabelPuntoDeVenta.Visible = False
        Else
            LabelPuntoDeVenta.Visible = True
        End If

        CalculaTotales()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function ArmaArchivos600() As Boolean

        Dim Sql As String

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM RecibosCabeza WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaCabeza) Then Return False
        If PNota <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then AgregaCabeza600()

        PEmisor = DtNotaCabeza.Rows(0).Item("Emisor")

        MuestraCabeza()

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM RecibosDetallePago WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtMedioPago) Then Return False

        Return True

    End Function
    Private Function ArmaArchivos1010() As Boolean

        Dim Sql As String

        If PNota = 0 Then
            Prestamo = HallaPrestamo(PCompDestino, ConexionNota)
            If Prestamo <= 0 Then Return False
        End If

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM PrestamosMovimientoCabeza WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaCabeza) Then Return False
        If PNota <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then AgregaCabeza1010()

        MuestraCabeza()

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM PrestamosMovimientoPago WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtMedioPago) Then Return False

        If PNota <> 0 Then
            Dim Dt As New DataTable
            Sql = "SELECT * FROM PrestamosCabeza WHERE Prestamo = " & DtNotaCabeza.Rows(0).Item("Prestamo") & ";"
            If Not Tablas.Read(Sql, ConexionNota, Dt) Then Return False
            If Dt.Rows.Count = 0 Then
                MsgBox("Prestamo No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            POrigenDestino = Dt.Rows(0).Item("Origen")
            PEmisor = Dt.Rows(0).Item("Emisor")
            Dt.Dispose()
        End If

        If POrigenDestino = 1 Then TextEmisor.Text = "Bco. " & NombreBanco(PEmisor)
        If POrigenDestino = 2 Then TextEmisor.Text = NombreProveedor(PEmisor)
        If POrigenDestino = 3 Then TextEmisor.Text = NombreCliente(PEmisor)

        Return True

    End Function
    Private Function ArmaArchivos5010() As Boolean

        Dim Sql As String

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM OtrosPagosCabeza WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaCabeza) Then Return False
        If PNota <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then AgregaCabeza5010()

        PEmisor = DtNotaCabeza.Rows(0).Item("Proveedor")

        MuestraCabeza()

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM OtrosPagosPago WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtMedioPago) Then Return False

        Return True

    End Function
    Private Function ArmaArchivos4010() As Boolean

        Dim Sql As String

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM SueldosMovimientoCabeza WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaCabeza) Then Return False
        If PNota <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then AgregaCabeza4010()

        PEmisor = DtNotaCabeza.Rows(0).Item("Legajo")

        MuestraCabeza()

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM SueldosMovimientoPago WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtMedioPago) Then Return False

        Return True

    End Function
    Private Function ArmaArchivos6000() As Boolean

        Dim Sql As String

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM CompraDivisasCabeza WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaCabeza) Then Return False
        If PNota <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then AgregaCabeza6000()

        PEmisor = DtNotaCabeza.Rows(0).Item("Emisor")
        POrigenDestino = DtNotaCabeza.Rows(0).Item("Origen")

        MuestraCabeza()

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM CompraDivisasPago WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtMedioPago) Then Return False

        Return True

    End Function
    Private Function ArmaArchivos7001() As Boolean

        Dim Sql As String

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM MovimientosFondoFijoCabeza WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaCabeza) Then Return False
        If PNota <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then AgregaCabeza7001()

        PEmisor = DtNotaCabeza.Rows(0).Item("FondoFijo")

        MuestraCabeza()

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM MovimientosFondoFijoPago WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtMedioPago) Then Return False

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtNotaCabeza

        Dim Enlace As Binding
        Dim Row As DataRowView = MiEnlazador.Current

        Select Case PTipoNota
            Case 600, 65, 64
                Enlace = New Binding("Text", MiEnlazador, "Nota")
                AddHandler Enlace.Format, AddressOf FormatNotaOrden
                TextNota.DataBindings.Clear()
                TextNota.DataBindings.Add(Enlace)
                If PTipoNota = 600 And POrigenDestino = 6 Then
                    TextEmisor.Text = HallaNombreDestino(6, Row("Emisor"))
                Else
                    TextEmisor.Text = HallaNombreDestino(2, Row("Emisor"))
                End If
            Case 1010
                Enlace = New Binding("Text", MiEnlazador, "Movimiento")
                AddHandler Enlace.Format, AddressOf FormatEntero
                TextNota.DataBindings.Clear()
                TextNota.DataBindings.Add(Enlace)
            Case 4010
                Enlace = New Binding("Text", MiEnlazador, "Movimiento")
                AddHandler Enlace.Format, AddressOf FormatEntero
                TextNota.DataBindings.Clear()
                TextNota.DataBindings.Add(Enlace)
                TextEmisor.Text = HallaNombreDestino(4, Row("Legajo"))
            Case 5010
                Enlace = New Binding("Text", MiEnlazador, "Movimiento")
                AddHandler Enlace.Format, AddressOf FormatEntero
                TextNota.DataBindings.Clear()
                TextNota.DataBindings.Add(Enlace)
                TextEmisor.Text = HallaNombreDestino(5, Row("Proveedor"))
            Case 6000
                Enlace = New Binding("Text", MiEnlazador, "Movimiento")
                AddHandler Enlace.Format, AddressOf FormatEntero
                TextNota.DataBindings.Clear()
                TextNota.DataBindings.Add(Enlace)
                TextEmisor.Text = HallaNombreDestino(Row("Origen"), Row("Emisor"))
            Case 7001
                Enlace = New Binding("Text", MiEnlazador, "Movimiento")
                AddHandler Enlace.Format, AddressOf FormatEntero
                TextNota.DataBindings.Clear()
                TextNota.DataBindings.Add(Enlace)
                TextEmisor.Text = HallaNombreDestino(6, Row("FondoFijo"))
        End Select

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalRecibo.DataBindings.Clear()
        TextTotalRecibo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatNotaOrden(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(0, "#")
        Else
            Numero.Value = Format(Numero.Value, "0000-00000000")
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
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
            Item = New ItemListaConceptosAsientos
            Item.Clave = Row("MedioPago")
            If Row("Cambio") <> 0 Then
                Item.Importe = Trunca(Row("Cambio") * Row("Importe"))
            Else : Item.Importe = Row("Importe")
            End If
            ListaConceptos.Add(Item)
        Next

        If PMedioPagoAReemplazar = 2 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = -501
            Item.Importe = CDbl(TextImporteCheque.Text)
            ListaConceptos.Add(Item)
        End If

        If PMedioPagoAReemplazar = 3 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = -500
            Item.Importe = CDbl(TextImporteCheque.Text)
            ListaConceptos.Add(Item)
        End If

        If Not Asiento(1008, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

        If DtCabeza.Rows.Count <> 0 Then DtCabeza.Rows(0).Item("TipoDocumento") = TipoAsiento

        Return True

    End Function
    Private Sub AgregaCabeza600()

        Dim Row As DataRow

        Row = DtNotaCabeza.NewRow
        ArmaNuevoRecibo(Row)
        Row("TipoNota") = PTipoNota
        Row("Nota") = UltimoNumero
        Row("Emisor") = PEmisor
        Row("NumeroFondoFijo") = PNumeroFondoFijo
        Row("Saldo") = 0
        Row("Fecha") = DateTime1.Value
        Row("CodigoIva") = 1
        Row("Estado") = 1
        Row("Caja") = GCaja
        Row("Tr") = PEsTr
        Row("Moneda") = 1
        Row("Cambio") = 1
        Row("ClaveChequeReemplazado") = PClaveChequeAReemplazar
        Row("MedioPagoRechazado") = PMedioPagoAReemplazar
        DtNotaCabeza.Rows.Add(Row)

    End Sub
    Private Sub AgregaCabeza1010()

        Dim Row As DataRow

        Row = DtNotaCabeza.NewRow
        ArmaNuevoMovimientoPrestamo(Row)
        Row("Prestamo") = Prestamo
        Row("TipoNota") = 1010
        Row("Movimiento") = 0
        Row("Importe") = 0
        Row("Fecha") = DateTime1.Value
        Row("Caja") = GCaja
        Row("Estado") = 1
        Row("ClaveChequeReemplazado") = PClaveChequeAReemplazar
        Row("MedioPagoRechazado") = PMedioPagoAReemplazar
        DtNotaCabeza.Rows.Add(Row)

    End Sub
    Private Sub AgregaCabeza5010()

        Dim Row As DataRow

        Row = DtNotaCabeza.NewRow
        ArmaNuevoMovimientoOtroPago(Row)
        Row("Proveedor") = PEmisor
        Row("TipoNota") = 5010
        Row("Movimiento") = 0
        Row("Importe") = 0
        Row("Fecha") = DateTime1.Value
        Row("Caja") = GCaja
        Row("Estado") = 1
        Row("Saldo") = 0
        Row("ClaveChequeReemplazado") = PClaveChequeAReemplazar
        Row("MedioPagoRechazado") = PMedioPagoAReemplazar
        DtNotaCabeza.Rows.Add(Row)

    End Sub
    Private Sub AgregaCabeza4010()

        Dim Row As DataRow

        Row = DtNotaCabeza.NewRow
        ArmaNuevoPagoSueldo(Row)
        Row("Legajo") = PEmisor
        Row("TipoNota") = 4010
        Row("Importe") = 0
        Row("Saldo") = 0
        Row("Fecha") = DateTime1.Value
        Row("Caja") = GCaja
        Row("Estado") = 1
        Row("ClaveChequeReemplazado") = PClaveChequeAReemplazar
        Row("MedioPagoRechazado") = PMedioPagoAReemplazar
        DtNotaCabeza.Rows.Add(Row)

    End Sub
    Private Sub AgregaCabeza6000()

        Dim Row As DataRow

        Row = DtNotaCabeza.NewRow
        ArmaNuevoCompraDivisas(Row)
        Row("TipoMovimiento") = 6000
        Row("Origen") = POrigenDestino
        Row("Emisor") = PEmisor
        Row("Fecha") = DateTime1.Value
        Row("Caja") = GCaja
        Row("Estado") = 1
        Row("ClaveChequeReemplazado") = PClaveChequeAReemplazar
        Row("MedioPagoRechazado") = PMedioPagoAReemplazar
        DtNotaCabeza.Rows.Add(Row)

    End Sub
    Private Sub AgregaCabeza7001()

        Dim Row As DataRow

        Row = DtNotaCabeza.NewRow
        ArmaNuevoMovimientoFondoFijo(Row)
        Row("FondoFijo") = PEmisor
        Row("Numero") = PNumeroFondoFijo
        Row("Tipo") = PTipoNota
        Row("Fecha") = DateTime1.Value
        Row("Estado") = 1
        Row("Caja") = GCaja
        Row("ClaveChequeReemplazado") = PClaveChequeAReemplazar
        Row("MedioPagoRechazado") = PMedioPagoAReemplazar
        DtNotaCabeza.Rows.Add(Row)

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = CreaDtParaGrid()

    End Sub
    Private Sub CalculaTotales()

        TotalConceptos = 0
        For Each Row As DataRow In DtGrid.Rows
            TotalConceptos = TotalConceptos + Row("Importe")
        Next

        TextTotalRecibo.Text = FormatNumber(TotalConceptos, GDecimales)

    End Sub
    Private Function HacerAlta600(ByVal DtNotaCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtChequeAReemplazarAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtReemplazoAux As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroNota As Double
        Dim NumeroAsiento As Double
        Dim NumeroW As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion comprobante.
            NumeroNota = UltimaNumeracionPagoYOrden(PTipoNota, ConexionNota)
            If NumeroNota < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            '
            DtNotaCabezaAux.Rows(0).Item("Nota") = NumeroNota
            DtNotaCabezaAux.Rows(0).Item("Interno") = NumeroInternoRecibos(LetraIva, PTipoNota, ConexionNota)
            If DtNotaCabezaAux.Rows(0).Item("Interno") <= 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            '
            DtReemplazoAux.Rows(0).Item("NotaReemplazante") = NumeroNota
            '
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

            NumeroW = ActualizaNota600("A", DtGridAux, DtNotaCabezaAux, DtMedioPagoAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -2 Then Exit For
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
        If NumeroW = -3 Then
            Return False
        End If
        If NumeroW = -2 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
    Private Function HacerAlta1010(ByVal DtNotaCabezaAux As DataTable, ByVal DtChequeAReemplazarAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtReemplazoAux As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroNota As Double
        Dim NumeroAsiento As Double
        Dim NumeroW As Double

        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy

        'Actualizo MedioPago.
        For Each Row As DataRow In DtGrid.Rows
            Dim Row1 As DataRow = DtMedioPagoAux.NewRow
            Row1("Movimiento") = 0
            Row1("Item") = 0
            Row1("MedioPago") = Row("MedioPago")
            Row1("Detalle") = ""
            Row1("Neto") = 0
            Row1("Alicuota") = 0
            Row1("Cambio") = Row("Cambio")
            Row1("Importe") = Row("Importe")
            Row1("Banco") = Row("Banco")
            Row1("Cuenta") = Row("Cuenta")
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            DtMedioPagoAux.Rows.Add(Row1)
        Next

        For i As Integer = 1 To 50
            'Halla Ultima numeracion comprobante.
            NumeroNota = UltimaNumeracionPrestamo(ConexionNota)
            If NumeroNota < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            '
            DtNotaCabezaAux.Rows(0).Item("Movimiento") = NumeroNota
            '
            DtReemplazoAux.Rows(0).Item("NotaReemplazante") = NumeroNota
            '
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

            NumeroW = ActualizaNota1010("A", DtNotaCabezaAux, DtMedioPagoAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -2 Then Exit For
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
        If NumeroW = -3 Then
            Return False
        End If
        If NumeroW = -2 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
    Private Function HacerAlta5010(ByVal DtNotaCabezaAux As DataTable, ByVal DtChequeAReemplazarAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtReemplazoAux As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroNota As Double
        Dim NumeroAsiento As Double
        Dim NumeroW As Double

        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy

        'Actualizo MedioPago.
        For Each Row As DataRow In DtGrid.Rows
            Dim Row1 As DataRow = DtMedioPagoAux.NewRow
            Row1("Movimiento") = 0
            Row1("MedioPago") = Row("MedioPago")
            Row1("Detalle") = ""
            Row1("Neto") = 0
            Row1("Alicuota") = 0
            Row1("Cambio") = Row("Cambio")
            Row1("Importe") = Row("Importe")
            Row1("Banco") = Row("Banco")
            Row1("Cuenta") = Row("Cuenta")
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            DtMedioPagoAux.Rows.Add(Row1)
        Next

        For i As Integer = 1 To 50
            'Halla Ultima numeracion comprobante.
            NumeroNota = UltimaNumeracionOtrosPagos(ConexionNota)
            If NumeroNota < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            '
            DtNotaCabezaAux.Rows(0).Item("Movimiento") = NumeroNota
            For Each Row As DataRow In DtMedioPagoAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Movimiento") = NumeroNota
                End If
            Next
            '
            DtReemplazoAux.Rows(0).Item("NotaReemplazante") = NumeroNota
            '
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

            NumeroW = ActualizaNota5010("A", DtNotaCabezaAux, DtMedioPagoAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -2 Then Exit For
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
        If NumeroW = -3 Then
            Return False
        End If
        If NumeroW = -2 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
    Private Function HacerAlta4010(ByVal DtNotaCabezaAux As DataTable, ByVal DtChequeAReemplazarAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtReemplazoAux As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroNota As Double
        Dim NumeroAsiento As Double
        Dim NumeroW As Double

        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy

        'Actualizo MedioPago.
        For Each Row As DataRow In DtGrid.Rows
            Dim Row1 As DataRow = DtMedioPagoAux.NewRow
            Row1("Movimiento") = 0
            Row1("MedioPago") = Row("MedioPago")
            Row1("Detalle") = ""
            Row1("Neto") = 0
            Row1("Alicuota") = 0
            Row1("Cambio") = Row("Cambio")
            Row1("Importe") = Row("Importe")
            Row1("Banco") = Row("Banco")
            Row1("Cuenta") = Row("Cuenta")
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            DtMedioPagoAux.Rows.Add(Row1)
        Next

        For i As Integer = 1 To 50
            'Halla Ultima numeracion comprobante.
            NumeroNota = UltimaNumeracionPagoSueldo(ConexionNota)
            If NumeroNota < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            '
            DtNotaCabezaAux.Rows(0).Item("Movimiento") = NumeroNota
            For Each Row As DataRow In DtMedioPagoAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Movimiento") = NumeroNota
                End If
            Next
            '
            DtReemplazoAux.Rows(0).Item("NotaReemplazante") = NumeroNota
            '
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

            NumeroW = ActualizaNota4010("A", DtNotaCabezaAux, DtMedioPagoAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -2 Then Exit For
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
        If NumeroW = -3 Then
            Return False
        End If
        If NumeroW = -2 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
    Private Function HacerAlta6000(ByVal DtNotaCabezaAux As DataTable, ByVal DtChequeAReemplazarAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtReemplazoAux As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroNota As Double
        Dim NumeroAsiento As Double
        Dim NumeroW As Double
        Dim Item As Integer = 0

        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy

        'Actualizo MedioPago.
        For Each Row As DataRow In DtGrid.Rows
            Dim Row1 As DataRow = DtMedioPagoAux.NewRow
            Item = Item + 1
            Row1("Movimiento") = 0
            Row1("Item") = Item
            Row1("Tipo") = 1
            Row1("MedioPago") = Row("MedioPago")
            Row1("Cambio") = Row("Cambio")
            Row1("Importe") = Row("Importe")
            Row1("Banco") = Row("Banco")
            Row1("Cuenta") = Row("Cuenta")
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            DtMedioPagoAux.Rows.Add(Row1)
        Next

        For i As Integer = 1 To 50
            'Halla Ultima numeracion comprobante.
            NumeroNota = UltimaNumeracionCompraDivisas(ConexionNota)
            If NumeroNota < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            '
            DtNotaCabezaAux.Rows(0).Item("Movimiento") = NumeroNota
            For Each Row As DataRow In DtMedioPagoAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Movimiento") = NumeroNota
                End If
            Next
            '
            DtReemplazoAux.Rows(0).Item("NotaReemplazante") = NumeroNota
            '
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

            NumeroW = ActualizaNota6000("A", DtNotaCabezaAux, DtMedioPagoAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -1 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = -3 Then
            Return False
        End If
        If NumeroW = -2 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
    Private Function HacerAlta7001(ByVal DtNotaCabezaAux As DataTable, ByVal DtChequeAReemplazarAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtReemplazoAux As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroNota As Double
        Dim NumeroAsiento As Double
        Dim NumeroW As Double
        Dim Item As Integer = 0

        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy

        'Actualizo MedioPago.
        For Each Row As DataRow In DtGrid.Rows
            Dim Row1 As DataRow = DtMedioPagoAux.NewRow
            Item = Item + 1
            Row1("Movimiento") = 0
            Row1("Item") = Item
            Row1("MedioPago") = Row("MedioPago")
            Row1("Cambio") = Row("Cambio")
            Row1("Importe") = Row("Importe")
            Row1("Banco") = Row("Banco")
            Row1("Cuenta") = Row("Cuenta")
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            Row1("ClaveInterna") = Row("ClaveInterna")
            DtMedioPagoAux.Rows.Add(Row1)
        Next

        For i As Integer = 1 To 50
            'Halla Ultima numeracion comprobante.
            NumeroNota = UltimaNumeracionAjuste(ConexionNota)
            If NumeroNota < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            '
            DtNotaCabezaAux.Rows(0).Item("Movimiento") = NumeroNota
            For Each Row As DataRow In DtMedioPagoAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Movimiento") = NumeroNota
                End If
            Next
            '
            DtReemplazoAux.Rows(0).Item("NotaReemplazante") = NumeroNota
            '
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

            NumeroW = ActualizaNota7001("A", DtNotaCabezaAux, DtMedioPagoAux, DtChequeAReemplazarAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtReemplazoAux)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -2 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = -3 Then
            Return False
        End If
        If NumeroW = -2 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
    Private Function ActualizaNota600(ByVal Funcion As String, ByVal DtGridAux As DataTable, ByVal DtNotaCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtChequeAReemplazarAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtReemplazoAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                Dim DtNotaDetalleAux As New DataTable

                Resul = ActualizaRecibo(DtFormasPago, Funcion, DtGridAux, DtNotaCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, ConexionNota, False)
                If Resul <= 0 Then Return Resul
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
                If Not IsNothing(DtChequeAReemplazarAux.GetChanges) Then
                    Resul = GrabaTabla(DtChequeAReemplazarAux.GetChanges, "Cheques", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtReemplazoAux.GetChanges) Then
                    Resul = GrabaTabla(DtReemplazoAux.GetChanges, "Reemplazos", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return DtNotaCabezaAux.Rows(0).Item("Nota")
                '
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function ActualizaNota1010(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtChequeAReemplazarAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtReemplazoAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "PrestamosMovimientoCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Funcion = "A" Then
                    For I As Integer = 0 To DtMedioPagoAux.Rows.Count - 1
                        Dim Row As DataRow = DtMedioPagoAux.Rows(I)
                        If Row("MedioPago") = 2 Then
                            Row("ClaveCheque") = ActualizaClavesComprobantes("A", 0, DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq"))
                            If Row("ClaveCheque") <= 0 Then Return Row("ClaveCheque")
                        End If
                        If Row("MedioPago") = 3 Then
                            If ActualizaClavesComprobantes("A", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), DtGrid.Rows(I).Item("EmisorCheque"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq")) <= 0 Then
                                MsgBox("Banco " & Row("Banco") & " Cuenta " & Row("Cuenta") & " Error al Grabar Cheque.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            End If
                        End If
                    Next
                End If
                If Funcion = "B" Then
                    For Each Row As DataRow In DtMedioPagoAux.Rows
                        If Row("MedioPago") = 3 Then           'eCheq poner False a lo ultimo.
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("ClaveCheque") & " " & Row("Cuenta") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            Else
                            End If
                        End If
                        If Row("MedioPago") = 2 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("Banco") & " " & Row("Cuenta") & " Error de datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            Else
                            End If
                        End If
                    Next
                End If
                '
                If Not IsNothing(DtMedioPagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "PrestamosMovimientoPago", ConexionNota)
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
                If Not IsNothing(DtChequeAReemplazarAux.GetChanges) Then
                    Resul = GrabaTabla(DtChequeAReemplazarAux.GetChanges, "Cheques", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtReemplazoAux.GetChanges) Then
                    Resul = GrabaTabla(DtReemplazoAux.GetChanges, "Reemplazos", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return DtNotaCabezaAux.Rows(0).Item("Movimiento")
                '
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function ActualizaNota5010(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtChequeAReemplazarAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtReemplazoAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "OtrosPagosCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Funcion = "A" Then
                    For I As Integer = 0 To DtMedioPagoAux.Rows.Count - 1
                        Dim Row As DataRow = DtMedioPagoAux.Rows(I)
                        If Row("MedioPago") = 2 Then
                            Row("ClaveCheque") = ActualizaClavesComprobantes("A", 0, DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq"))
                            If Row("ClaveCheque") <= 0 Then Return Row("ClaveCheque")
                        End If
                        If Row("MedioPago") = 3 Then
                            If ActualizaClavesComprobantes("A", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), DtGrid.Rows(I).Item("EmisorCheque"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq")) <= 0 Then
                                MsgBox("Banco " & Row("Banco") & " Cuenta " & Row("Cuenta") & " Error al Grabar Cheque.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            End If
                        End If
                        If Row("MedioPago") = 14 Then
                            If ActualizaClavesDebito("A", Row("ClaveCheque"), PTipoNota, Row("MedioPago"), Row("Banco"), Row("Cuenta"), Row("Comprobante"), Row("FechaComprobante"), Row("Importe"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota) <= 0 Then
                                MsgBox("Debito Auto.Dife. " & Row("Comprobante") & " " & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            Else
                            End If
                        End If
                    Next
                End If
                If Funcion = "B" Then
                    For Each Row As DataRow In DtMedioPagoAux.Rows
                        If Row("MedioPago") = 3 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("ClaveCheque") & " " & Row("Cuenta") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            Else
                            End If
                        End If
                        If Row("MedioPago") = 2 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("Banco") & " " & Row("Cuenta") & " Error de datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            Else
                            End If
                        End If
                        If Row("MedioPago") = 14 Then
                            If ActualizaClavesDebito("B", Row("ClaveCheque"), PTipoNota, Row("MedioPago"), Row("Banco"), Row("Cuenta"), Row("Comprobante"), Row("FechaComprobante"), Row("Importe"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota) <= 0 Then
                                MsgBox("Debito Auto.Dife. " & Row("Comprobante") & " Error de datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            Else
                            End If
                        End If
                    Next
                End If
                '
                If Not IsNothing(DtMedioPagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "OtrosPagosPago", ConexionNota)
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
                If Not IsNothing(DtChequeAReemplazarAux.GetChanges) Then
                    Resul = GrabaTabla(DtChequeAReemplazarAux.GetChanges, "Cheques", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtReemplazoAux.GetChanges) Then
                    Resul = GrabaTabla(DtReemplazoAux.GetChanges, "Reemplazos", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return DtNotaCabezaAux.Rows(0).Item("Movimiento")
                '
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function ActualizaNota4010(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtChequeAReemplazarAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtReemplazoAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "SueldosMovimientoCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Funcion = "A" Then
                    For I As Integer = 0 To DtMedioPagoAux.Rows.Count - 1
                        Dim Row As DataRow = DtMedioPagoAux.Rows(I)
                        If Row("MedioPago") = 2 Then
                            Row("ClaveCheque") = ActualizaClavesComprobantes("A", 0, DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq"))
                            If Row("ClaveCheque") <= 0 Then Return Row("ClaveCheque")
                        End If
                        If Row("MedioPago") = 3 Then
                            If ActualizaClavesComprobantes("A", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), DtGrid.Rows(I).Item("EmisorCheque"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq")) <= 0 Then
                                MsgBox("Banco " & Row("Banco") & " Cuenta " & Row("Cuenta") & " Error al Grabar Cheque.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            End If
                        End If
                    Next
                End If
                If Funcion = "B" Then
                    For Each Row As DataRow In DtMedioPagoAux.Rows
                        If Row("MedioPago") = 3 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("ClaveCheque") & " " & Row("Cuenta") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            Else
                            End If
                        End If
                        If Row("MedioPago") = 2 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("Banco") & " " & Row("Cuenta") & " Error de datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            Else
                            End If
                        End If
                    Next
                End If
                '
                If Not IsNothing(DtMedioPagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "SueldosMovimientoPago", ConexionNota)
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
                If Not IsNothing(DtChequeAReemplazarAux.GetChanges) Then
                    Resul = GrabaTabla(DtChequeAReemplazarAux.GetChanges, "Cheques", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtReemplazoAux.GetChanges) Then
                    Resul = GrabaTabla(DtReemplazoAux.GetChanges, "Reemplazos", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return DtNotaCabezaAux.Rows(0).Item("Movimiento")
                '
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function ActualizaNota6000(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtChequeAReemplazarAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtReemplazoAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "CompraDivisasCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Funcion = "A" Then
                    For I As Integer = 0 To DtMedioPagoAux.Rows.Count - 1
                        Dim Row As DataRow = DtMedioPagoAux.Rows(I)
                        If Row("MedioPago") = 2 Then
                            Row("ClaveCheque") = ActualizaClavesComprobantes("A", 0, DtNotaCabezaAux.Rows(0).Item("TipoMovimiento"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq"))
                            If Row("ClaveCheque") <= 0 Then Return Row("ClaveCheque")
                        End If
                        If Row("MedioPago") = 3 Then
                            If ActualizaClavesComprobantes("A", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoMovimiento"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), DtGrid.Rows(I).Item("EmisorCheque"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq")) <= 0 Then
                                MsgBox("Banco " & Row("Banco") & " Cuenta " & Row("Cuenta") & " Error al Grabar Cheque.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            End If
                        End If
                    Next
                End If
                If Funcion = "B" Then
                    For Each Row As DataRow In DtMedioPagoAux.Rows
                        If Row("MedioPago") = 3 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoMovimiento"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("ClaveCheque") & " " & Row("Cuenta") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            Else
                            End If
                        End If
                        If Row("MedioPago") = 2 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoMovimiento"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("Banco") & " " & Row("Cuenta") & " Error de datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            Else
                            End If
                        End If
                    Next
                End If
                '
                If Not IsNothing(DtMedioPagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "CompraDivisasPago", ConexionNota)
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
                If Not IsNothing(DtChequeAReemplazarAux.GetChanges) Then
                    Resul = GrabaTabla(DtChequeAReemplazarAux.GetChanges, "Cheques", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtReemplazoAux.GetChanges) Then
                    Resul = GrabaTabla(DtReemplazoAux.GetChanges, "Reemplazos", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return DtNotaCabezaAux.Rows(0).Item("Movimiento")
                '
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function ActualizaNota7001(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtChequeAReemplazarAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtReemplazoAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "MovimientosFondoFijoCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If

                If Funcion = "A" Then
                    For I As Integer = 0 To DtMedioPagoAux.Rows.Count - 1
                        Dim Row As DataRow = DtMedioPagoAux.Rows(I)
                        If Row("MedioPago") = 2 Then
                            Row("ClaveCheque") = ActualizaClavesComprobantes("A", 0, 7001, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq"))
                            If Row("ClaveCheque") <= 0 Then Return Row("ClaveCheque")
                        End If
                        If Row("MedioPago") = 3 Then
                            If ActualizaClavesComprobantes("A", Row("ClaveCheque"), 7001, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), DtGrid.Rows(I).Item("EmisorCheque"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq")) <= 0 Then
                                MsgBox("Banco " & Row("Banco") & " Cuenta " & Row("Cuenta") & " Error al Grabar Cheque.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            End If
                        End If
                    Next
                End If
                If Funcion = "B" Then
                    For Each Row As DataRow In DtMedioPagoAux.Rows
                        If Row("MedioPago") = 3 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), 7001, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("ClaveCheque") & " " & Row("Cuenta") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            Else
                            End If
                        End If
                        If Row("MedioPago") = 2 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), 7001, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("Banco") & " " & Row("Cuenta") & " Error de datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            Else
                            End If
                        End If
                    Next
                End If

                If Not IsNothing(DtMedioPagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "MovimientosFondoFijoPago", ConexionNota)
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
                If Not IsNothing(DtChequeAReemplazarAux.GetChanges) Then
                    Resul = GrabaTabla(DtChequeAReemplazarAux.GetChanges, "Cheques", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtReemplazoAux.GetChanges) Then
                    Resul = GrabaTabla(DtReemplazoAux.GetChanges, "Reemplazos", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return DtNotaCabezaAux.Rows(0).Item("Movimiento")
                '
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Public Function UltimaNumeracionPrestamo(ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Movimiento) FROM PrestamosMovimientoCabeza;", Miconexion)
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
    Private Sub Print_PrintOrdenPago(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.


        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            'Encabezado.
            Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
            Texto = GNombreEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
            Texto = "Reemplazo Cheque"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 115, MTop)
            ' 
            PrintFont = New Font("Courier New", 12)
            Texto = "Nro. Comprobante:  " & TextNota.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
            Texto = "Fecha:  " & DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
            Texto = "Razon Social: " & TextEmisor.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 30)
            Texto = "Importe Cheque : " & TextImporteCheque.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 60, MTop + 40)

            Dim Longi As Integer
            Dim Xq As Integer
            Dim Yq As Integer
            Dim RowsBusqueda() As DataRow
            Dim Ancho As Integer
            Dim Alto As Integer

            'Grafica -Rectangulo medios de pagos. ----------------------------------------------------------------------
            x = MIzq
            y = MTop + 50
            Ancho = 180
            Alto = 95
            PrintFont = New Font("Courier New", 10)
            Dim LineaDescripcion As Integer = x + 40
            Dim LineaCambio As Integer = x + 55
            Dim LineaImporte As Integer = x + 85
            Dim LineaBanco As Integer = x + 125
            Dim LineaNumero As Integer = x + 154
            Dim LineaVencimiento As Integer = x + Ancho
            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaDescripcion, y, LineaDescripcion, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCambio, y, LineaCambio, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte, y, LineaImporte, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaBanco, y, LineaBanco, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaNumero, y, LineaNumero, y + Alto)
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
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)
            '------------------------------------------------------------------------------------------------------------
            'Descripcion del pago.
            Yq = y - SaltoLinea
            For Each Row As DataRow In DtGrid.Rows
                If IsNothing(Row("MedioPago")) Then Exit For
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                Yq = Yq + SaltoLinea
                'Imprime Detalle.
                Texto = RowsBusqueda(0).Item("Nombre")
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Importe.
                Texto = FormatNumber(Row("Importe"), 2)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Banco.
                Texto = NombreBanco(Row("Banco"))
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte + 1
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Numero.
                Texto = FormatNumber(Row("Numero"), 0)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaNumero - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Vencimeinto.
                Texto = Format(Row("Fecha"), "dd/MM/yyyy")
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaVencimiento - Longi - 2
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Comprobante.
                If Row("Comprobante") <> 0 Then
                    Texto = Row("Comprobante")
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaNumero - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
                'Imprime FechaComprobante.
                If Format(Row("FechaComprobante"), "dd/MM/yyyy") <> "01/01/1800" Then
                    Texto = Format(Row("FechaComprobante"), "dd/MM/yyyy")
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaVencimiento - Longi - 2
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
            Next
            'Final.
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
    Private Function UltimaNumeracionOtrosPagos(ByVal ConexionStr) As Double

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
    Private Function UltimaNumeracionPagoSueldo(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Movimiento) FROM SueldosMovimientoCabeza;", Miconexion)
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
    Public Function UltimaNumeracionCompraDivisas(ByVal ConexionStr As String) As Double

        Dim Patron As String = GCaja & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Movimiento) FROM CompraDivisasCabeza WHERE CAST(CAST(CompraDivisasCabeza.Movimiento AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(GCaja & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function UltimaNumeracionAjuste(ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Movimiento) FROM MovimientosFondoFijoCabeza;", Miconexion)
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
    Private Sub PideDatosEmisor()

        If (PTipoNota = 65 Or PTipoNota = 64) Then
            LetraIva = HallaTipoIvaCliente(PEmisor)
        End If
        If PTipoNota = 600 And POrigenDestino <> 6 Then
            LetraIva = HallaTipoIvaProveedor(PEmisor)
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
        End If

        LabelPuntoDeVenta.Text = "Punto de Venta " & Format(GPuntoDeVenta, "0000")

    End Sub
    Private Function HallaTipo(ByVal MedioPago As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtFormasPago.Select("Clave = " & MedioPago)
        Return RowsBusqueda(0).Item("Tipo")

    End Function
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM RecibosCabeza;", Miconexion)
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
    Private Function HallaPrestamo(ByVal Comprobante As Integer, ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Prestamo FROM PrestamosMovimientoCabeza WHERE Movimiento = " & Comprobante & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Movimientos de Prestamos.", MsgBoxStyle.Critical)
            Return -1
        End Try

    End Function
    Private Function Valida() As Boolean

        Dim PuntoVentaManual As Boolean

        If CDbl(TextTotalRecibo.Text) <> CDbl(TextImporteCheque.Text) Then
            MsgBox("Total Recibo no coincide con Importe del Cheque a Reemplazar.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PTipoNota = 600 Then
            If PManual Then
                PuntoVentaManual = EsReciboManual(Val(Strings.Left(TextNota.Text, 4)))
            Else : PuntoVentaManual = EsReciboManual(GPuntoDeVenta)
            End If
            If PNota = 0 Then
                If Not PManual And PuntoVentaManual Then
                    MsgBox("Punto de Venta del Comprobante SOLO Habilitado para Recibo Manual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    TextNota.Focus()
                    Return False
                End If
            End If
        End If

        If TotalConceptos = 0 Then
            MsgBox("Debe informar Importes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function

End Class