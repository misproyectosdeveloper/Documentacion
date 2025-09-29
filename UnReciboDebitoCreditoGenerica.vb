Option Explicit On
Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnReciboDebitoCreditoGenerica
    Public PNota As Double
    Public PAbierto As Boolean
    Public PTipoNota As Integer
    Public PBloqueaFunciones As Boolean
    Public PImporte As Double
    Public PImputa As Boolean
    'Datos Credito Bancario.
    Public PCuentaBancaria As Double
    Public PBanco As Integer
    'Datos Fondo Fijo.
    Public PProveedorFondoFijo As Integer
    Public PNumero As Integer
    'Datos para Prestamos.
    Public PEmisorPrestamo As Integer
    Public PPrestamo As Decimal
    Public POrigenPrestamo As Integer
    'Datos para rechazos de cheques.
    Public PClaveCheque As Integer
    Public PNumeroCheque As Decimal
    Public PImporteCheque As Decimal
    Public PEsRechazoCheque As Boolean
    Public PMedioPago As Integer
    'Datos para Sueldos.
    Public PLegajo As Integer
    '
    Public PConexion As String
    Public PConexionN As String
    '
    Dim DtNotaCabeza As DataTable
    Dim DtMedioPago As DataTable
    Dim DtNotaDetalle As DataTable
    Dim DtChequesRechazados As DataTable
    '
    Dim DtFormasPago As DataTable
    Dim DtGrid As DataTable
    Dim DtGridCompro As DataTable
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
    Dim UltimaFechaW As DateTime
    Dim FechaAnt As DateTime
    Dim TablaIva(0) As Double
    Dim TipoAsiento As Integer
    'Variables Impresion. 
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim LineasParaImpresion As Integer = 0
    Dim LineasParaImpresionImputacion As Integer = 0
    Private Sub UnRecibo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Select Case PTipoNota
            Case 7005, 7007
                If Not PermisoEscritura(8) Then PBloqueaFunciones = True
            Case 1005, 1007
                If Not PermisoEscritura(801) Then PBloqueaFunciones = True
            Case 93
                If Not PermisoEscritura(8) Then PBloqueaFunciones = True
            Case 4007
                If Not PermisoEscritura(9) Then PBloqueaFunciones = True
        End Select

        If Conexion = "" Then Conexion = PConexion : ConexionN = PConexionN 'Si es llamado de otro proyecto. 

        Me.Top = 50

        If GCaja = 0 And PNota = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        GridCompro.AutoGenerateColumns = False
        GridCompro.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        Select Case PTipoNota
            Case 7005, 7007
                If PNota = 0 And PProveedorFondoFijo = 0 Then
                    PideDatosProveedorFondoFijo()
                    If PProveedorFondoFijo = 0 Then Me.Close() : Exit Sub
                    If FondoFijoCerrado(PNumero, PAbierto) Then
                        MsgBox("Fondo Fijo cerrado.", MsgBoxStyle.Critical)
                        Me.Close()
                        Exit Sub
                    End If
                End If
        End Select

        If PAbierto Then
            ConexionNota = Conexion
        Else : ConexionNota = ConexionN
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        ArmaTablaIva(TablaIva)

        DtFormasPago = ArmaConceptosDebitoCredito100()

        If PTipoNota = 7005 Then LabelTipoNota.Text = "Nota Debito Financiera a Fondo Fijo" : Me.BackColor = Color.Wheat
        If PTipoNota = 7007 Then LabelTipoNota.Text = "Nota Crédito Financiera a Fondo Fijo" : Me.BackColor = Color.Wheat
        If PTipoNota = 1005 Then LabelTipoNota.Text = "Nota Debito Prestamo" : Me.BackColor = Color.Wheat
        If PTipoNota = 1007 Then LabelTipoNota.Text = "Nota Crédito Prestamo" : Me.BackColor = Color.Wheat
        If PTipoNota = 93 Then LabelTipoNota.Text = "Nota Crédito Bancario" : Me.BackColor = Color.Wheat
        If PTipoNota = 4007 Then LabelTipoNota.Text = "Nota Crédito Pago Sueldo" : Me.BackColor = Color.Wheat

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

        GridCompro.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtChequesRechazadosAux As DataTable = DtChequesRechazados.Copy

        If PNota = 0 Then
            Select Case PTipoNota
                Case 7005, 7007
                    GeneraMediosPagoFondoFijo(DtMedioPagoAux)
                Case 1005, 1007
                    GeneraMediosPagoPrestamo(DtMedioPagoAux)
                Case 4007
                    GeneraMediosPagoSueldos(DtMedioPagoAux)
            End Select
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PEsRechazoCheque And PNota = 0 Then
            DtChequesRechazadosAux.Rows(0).Item("Estado") = 4
            Select Case PTipoNota
                Case 7005, 1005
                    DtChequesRechazadosAux.Rows(0).Item("TieneDebito") = True
            End Select
        End If

        If IsNothing(DtNotaCabezaAux.GetChanges) And IsNothing(DtNotaDetalleAux.GetChanges) Then
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
            If HacerAlta(DtNotaCabezaAux, DtNotaDetalleAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux) Then
                ArmaArchivos()
            End If
        Else
            If HacerModificacion(DtNotaCabezaAux, DtNotaDetalleAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux) Then
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

        Select Case PTipoNota
            Case 4007
                If DtNotaCabezaAux.Rows(0).Item("Importe") <> DtNotaCabezaAux.Rows(0).Item("Saldo") Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Nota Tiene Imputaciones, Debe anularlas para Continuar. Operación se CANCELA.")
                    Exit Sub
                End If
        End Select

        Dim Resul As Double

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PEsRechazoCheque Then
            Select Case PTipoNota
                Case 7007, 1007, 93, 4007
                    If DtChequesRechazadosAux.Rows(0).Item("TieneDebito") Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Se Genero un Debito para este Cheque, debe anularlo previamente para continuar con la anulación de esta Nota. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    Else
                        DtChequesRechazadosAux.Rows(0).Item("Estado") = 1
                    End If
                Case 7005, 1005
                    DtChequesRechazadosAux.Rows(0).Item("Estado") = 1
                    DtChequesRechazadosAux.Rows(0).Item("TieneDebito") = False
            End Select
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

        DtNotaCabezaAux.Rows(0).Item("Estado") = 3

        Select Case PTipoNota
            Case 7005, 7007
                Resul = ActualizaNotaFondoFijo("B", DtNotaCabezaAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux)
            Case 1005, 1007
                Resul = ActualizaNotaPrestamo("B", DtNotaCabezaAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux)
            Case 93
                Resul = ActualizaNotaBancos("B", DtNotaCabezaAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux)
            Case 4007
                Resul = ActualizaNotaPagoSueldos("B", DtNotaCabezaAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux)
        End Select

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
    Private Sub ButtonMediosDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMediosDePago.Click

        Dim DtFormasPagoW As DataTable = DtFormasPago.Copy

        'No permite informar en grid si total orden de compra no esta informado.  

        UnMediosPago.PTipoNota = PTipoNota
        UnMediosPago.PEmisor = PProveedorFondoFijo
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
        UnMediosPago.PCambio = 0
        UnMediosPago.PDtRetencionesAutomaticas = New DataTable
        UnMediosPago.PDiferenciaDeCambio = False
        UnMediosPago.PImporte = CDbl(TextTotalRecibo.Text)
        UnMediosPago.ShowDialog()
        DtGrid = UnMediosPago.PDtGrid

        UnMediosPago.Dispose()

        CalculaTotales()

    End Sub
    Private Sub ButtonComprobantesAImputar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonComprobantesAImputar.Click


        UNComprobanteAImputar.PDtGridCompro = DtGridCompro
        UNComprobanteAImputar.PTipo = PTipoNota
        UNComprobanteAImputar.PAbierto = PAbierto
        UNComprobanteAImputar.PTotalConceptos = TextTotalRecibo.Text
        UNComprobanteAImputar.PTipoIva = 0
        UNComprobanteAImputar.PMoneda = 1
        UNComprobanteAImputar.PCambio = 1
        UNComprobanteAImputar.ShowDialog()
        DtGridCompro = UNComprobanteAImputar.PDtGridCompro
        UNComprobanteAImputar.Dispose()

        ArmaGridCompro()

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
    Private Sub ButtonTextoRecibo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTextoRecibo.Click

        UnTextoParaRecibo.ShowDialog()

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Nota debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ErrorImpresion = False
        Paginas = 0
        If Not PAbierto Then Copias = 1

        Dim print_document As New PrintDocument

        If PAbierto Then Copias = 2
        AddHandler print_document.PrintPage, AddressOf Print_PrintPageNota    'Notas credito y debitos financieras.
        print_document.Print()
        If ErrorImpresion Then Exit Sub

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
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PNota = 0
        PProveedorFondoFijo = 0
        UnRecibo_Load(Nothing, Nothing)

    End Sub
    Private Function ArmaArchivos() As Boolean                           'ArmaArchivos

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()
        CreaDtGridCompro()

        Select Case PTipoNota
            Case 7005, 7007
                If Not ArmaReciboFondoFijo(DtNotaCabeza, DtNotaDetalle, DtMedioPago) Then Me.Close() : Exit Function
            Case 1005, 1007
                If Not ArmaReciboPrestamo(DtNotaCabeza, DtNotaDetalle, DtMedioPago) Then Me.Close() : Exit Function
            Case 93
                If Not ArmaReciboBancario(DtNotaCabeza, DtNotaDetalle, DtMedioPago) Then Me.Close() : Exit Function
            Case 4007
                If Not ArmaReciboPagoSueldos(DtNotaCabeza, DtNotaDetalle, DtMedioPago) Then Me.Close() : Exit Function
        End Select

        If PNota <> 0 Then
            PClaveCheque = DtNotaCabeza.Rows(0).Item("ChequeRechazado")
            If PClaveCheque <> 0 Then
                PEsRechazoCheque = True : ButtonNuevo.Enabled = False
            Else : PEsRechazoCheque = False : ButtonNuevo.Enabled = True
            End If
        End If

        DtChequesRechazados = New DataTable
        If PEsRechazoCheque Then
            If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & PMedioPago & " AND ClaveCheque = " & PClaveCheque & ";", ConexionNota, DtChequesRechazados) Then Me.Close() : Exit Function
        End If

        If PTipoNota = 7005 And PEsRechazoCheque <> 0 Then TipoAsiento = 7206
        If PTipoNota = 7007 And PEsRechazoCheque <> 0 Then TipoAsiento = 7207
        If PTipoNota = 1005 And PEsRechazoCheque <> 0 Then TipoAsiento = 1005
        If PTipoNota = 1007 And PEsRechazoCheque <> 0 Then TipoAsiento = 1007
        If PTipoNota = 93 And PEsRechazoCheque <> 0 Then TipoAsiento = 93
        If PTipoNota = 4007 And PEsRechazoCheque <> 0 Then TipoAsiento = 7002

        'Procesa Facturas.
        Dim Row1 As DataRow
        DtGridCompro.Clear()

        ButtonComprobantesAImputar.Visible = False
        GridCompro.Visible = False
        Panel9.Visible = False

        Dim RowsBusqueda() As DataRow

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
        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Saldo") = 0 And Row("Asignado") = 0 Then Row.Delete()
        Next
        DtGridCompro.AcceptChanges()
        ArmaGridCompro()

        If DtNotaCabeza.Rows(0).Item("Estado") = 3 Then
            GridCompro.ReadOnly = True
        Else : GridCompro.ReadOnly = False
        End If

        If PNota <> 0 Then
            ButtonAceptar.Text = "Modificar Recibo"
        Else
            ButtonAceptar.Text = "Grabar Recibo"
        End If

        If DtNotaCabeza.Rows(0).Item("Estado") = 3 Then
            GridCompro.ReadOnly = True
        Else : GridCompro.ReadOnly = False
        End If

        CalculaTotales()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Function ArmaReciboFondoFijo(ByRef DtCabeza As DataTable, ByRef DtNotaDetalle As DataTable, ByRef DtMedioPago As DataTable) As Boolean

        Dim Sql As String
        Dim Row1 As DataRow

        DtCabeza = New DataTable
        Sql = "SELECT * FROM MovimientosFondoFijoCabeza WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtCabeza) Then Return False
        If PNota <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then
            Dim Row As DataRow = DtCabeza.NewRow
            ArmaNuevoMovimientoFondoFijo(Row)
            Row("Numero") = PNumero
            Row("Tipo") = PTipoNota
            Row("FondoFijo") = PProveedorFondoFijo
            Row("Fecha") = Now
            Row("Caja") = GCaja
            Row("Estado") = 1
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
            DtCabeza.Rows.Add(Row)
        End If

        PTipoNota = DtCabeza.Rows(0).Item("Tipo")
        PProveedorFondoFijo = DtCabeza.Rows(0).Item("FondoFijo")
        PNumero = DtCabeza.Rows(0).Item("Numero")
        PMedioPago = DtCabeza.Rows(0).Item("MedioPagoRechazado")
        TextNumeroFondoFijo.Text = PNumero
        TextEmisorFondoFijo.Text = HallaNombreDestino(6, PProveedorFondoFijo)
        Panel3.Visible = True

        MuestraCabezaFondoFijo(DtCabeza)

        DtNotaDetalle = New DataTable

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM MovimientosFondoFijoPago WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtMedioPago) Then Return False

        For Each Row As DataRow In DtMedioPago.Rows
            Row1 = DtGrid.NewRow
            Row1("Item") = Row("Item")
            Row1("MedioPago") = Row("MedioPago")
            Row1("Importe") = Row("Importe")
            Row1("Bultos") = 0
            Row1("Cambio") = Row("Cambio")
            Row1("Alicuota") = 0
            Row1("Neto") = Row("Importe")
            Row1("ImporteIva") = 0
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

        Return True

    End Function
    Private Function ArmaReciboPrestamo(ByRef DtCabeza As DataTable, ByRef DtNotaDetalle As DataTable, ByRef DtMedioPago As DataTable) As Boolean

        Dim Sql As String
        Dim Row1 As DataRow

        DtCabeza = New DataTable
        Sql = "SELECT * FROM PrestamosMovimientoCabeza WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtCabeza) Then Return False
        If PNota <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then
            Dim Row As DataRow
            Row = DtCabeza.NewRow
            ArmaNuevoMovimientoPrestamo(Row)
            Row("Prestamo") = PPrestamo
            Row("TipoNota") = PTipoNota
            Row("Movimiento") = 0
            Row("Fecha") = Now
            Row("Estado") = 1
            Row("Caja") = GCaja
            If PEsRechazoCheque Then
                Row("Importe") = PImporteCheque
                Row("MedioPagoRechazado") = PMedioPago
                Row("ChequeRechazado") = PClaveCheque
                Row("OrigenRechazo") = POrigenPrestamo
                If PMedioPago = 2 Then
                    Row("Comentario") = "Rechazo Cheque " & FormatNumber(PNumeroCheque, 0)
                End If
                If PMedioPago = 3 Then
                    Row("Comentario") = "Rechazo Cheque " & FormatNumber(PClaveCheque, 0)
                End If
            End If
            DtCabeza.Rows.Add(Row)
        End If

        PPrestamo = DtCabeza.Rows(0).Item("Prestamo")
        TextPrestamo.Text = DtCabeza.Rows(0).Item("Prestamo")
        PTipoNota = DtCabeza.Rows(0).Item("TipoNota")
        PMedioPago = DtCabeza.Rows(0).Item("MedioPagoRechazado")
        Panel5.Visible = True

        Dim Dt As New DataTable
        Sql = "SELECT Origen,Emisor FROM PrestamosCabeza WHERE Prestamo = " & PPrestamo & ";"
        If Not Tablas.Read(Sql, ConexionNota, Dt) Then Return False
        If Dt.Rows.Count = 0 Then
            MsgBox("Prestamo No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        TextEmisorPrestamo.Text = HallaNombreDestino(Dt.Rows(0).Item("Origen"), Dt.Rows(0).Item("Emisor"))
        Dt.Dispose()

        MuestraCabezaPrestamo(DtCabeza)

        DtNotaDetalle = New DataTable

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM PrestamosMovimientoPago WHERE Movimiento = " & PNota & ";"
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

        Return True

    End Function
    Private Function ArmaReciboPagoSueldos(ByRef DtCabeza As DataTable, ByRef DtNotaDetalle As DataTable, ByRef DtMedioPago As DataTable) As Boolean

        Dim Sql As String
        Dim Row1 As DataRow

        DtCabeza = New DataTable
        Sql = "SELECT * FROM SueldosMovimientoCabeza WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtCabeza) Then Return False
        If PNota <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then
            Dim Row As DataRow
            Row = DtCabeza.NewRow
            ArmaNuevoPagoSueldo(Row)
            Row("Legajo") = PLegajo
            Row("TipoNota") = PTipoNota
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
            DtCabeza.Rows.Add(Row)
        End If

        PLegajo = DtCabeza.Rows(0).Item("Legajo")
        TextLegajo.Text = HallaNombreDestino(4, PLegajo)
        PTipoNota = DtCabeza.Rows(0).Item("TipoNota")
        PMedioPago = DtCabeza.Rows(0).Item("MedioPagoRechazado")
        Panel8.Visible = True

        MuestraCabezaPagoSueldos(DtCabeza)

        DtNotaDetalle = New DataTable
        Sql = "SELECT * FROM SueldosMovimientoDetalle WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaDetalle) Then Return False

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM SueldosMovimientoPago WHERE Movimiento = " & PNota & ";"
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

        Return True

    End Function
    Private Function ArmaReciboBancario(ByRef DtCabeza As DataTable, ByRef DtNotaDetalle As DataTable, ByRef DtMedioPago As DataTable) As Boolean

        Dim Sql As String

        DtCabeza = New DataTable
        Sql = "SELECT * FROM MovimientosBancarioCabeza WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtCabeza) Then Return False
        If PNota <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then
            Dim Row As DataRow
            Row = DtCabeza.NewRow
            ArmaNuevoMovimientoBancario(Row)
            Row("TipoNota") = PTipoNota
            Row("MedioPago") = 0
            Row("Fecha") = Now
            Row("FechaComprobante") = DateTime1.Value
            Row("Banco") = PBanco
            Row("Cuenta") = PCuentaBancaria
            Row("Importe") = 0
            Row("Caja") = GCaja
            Row("Estado") = 1
            If PEsRechazoCheque Then
                Row("MedioPago") = 3
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
            DtCabeza.Rows.Add(Row)
        End If

        PTipoNota = DtCabeza.Rows(0).Item("TipoNota")
        PBanco = DtCabeza.Rows(0).Item("Banco")
        TextEmisorBanco.Text = NombreBanco(PBanco)
        PCuentaBancaria = DtCabeza.Rows(0).Item("Cuenta")
        PMedioPago = DtCabeza.Rows(0).Item("MedioPagoRechazado")
        TextCuentaBancaria.Text = FormatNumber(PCuentaBancaria, 0)
        Panel6.Visible = True

        MuestraCabezaBancario(DtCabeza)

        If DtCabeza.Rows(0).Item("ChequeRechazado") <> 0 Then
            Dim Row As DataRow = DtGrid.NewRow
            Row("Item") = 0
            Row("MedioPago") = 100
            Row("Importe") = DtCabeza.Rows(0).Item("Importe")
            Row("Cambio") = 0
            Row("Bultos") = 0
            Row("Detalle") = "Rechazo Cheque " & FormatNumber(DtCabeza.Rows(0).Item("ChequeRechazado"), 0)
            Row("Alicuota") = 0
            Row("Neto") = DtCabeza.Rows(0).Item("Importe")
            Row("ImporteIva") = 0
            Row("Comprobante") = DtCabeza.Rows(0).Item("Comprobante")
            Row("FechaComprobante") = DtCabeza.Rows(0).Item("FechaComprobante")
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

        DtNotaDetalle = New DataTable
        DtMedioPago = New DataTable

        Return True

    End Function
    Private Sub MuestraCabezaFondoFijo(ByVal DtCabeza As DataTable)

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Movimiento")
        AddHandler Enlace.Format, AddressOf Formatmovimiento
        TextNota.DataBindings.Clear()
        TextNota.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalRecibo.DataBindings.Clear()
        TextTotalRecibo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Saldo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextSaldo.DataBindings.Clear()
        TextSaldo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub MuestraCabezaPrestamo(ByVal DtCabeza As DataTable)

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Prestamo")
        AddHandler Enlace.Format, AddressOf FormatPrestamo
        TextPrestamo.DataBindings.Clear()
        TextPrestamo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Movimiento")
        AddHandler Enlace.Format, AddressOf Formatmovimiento
        TextNota.DataBindings.Clear()
        TextNota.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalRecibo.DataBindings.Clear()
        TextTotalRecibo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub MuestraCabezaBancario(ByVal DtCabeza As DataTable)

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Movimiento")
        AddHandler Enlace.Format, AddressOf FormatMovimientoBancario
        TextNota.DataBindings.Clear()
        TextNota.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalRecibo.DataBindings.Clear()
        TextTotalRecibo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub MuestraCabezaOtrosProveedores(ByVal DtCabeza As DataTable)

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Movimiento")
        AddHandler Enlace.Format, AddressOf Formatmovimiento
        TextNota.DataBindings.Clear()
        TextNota.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalRecibo.DataBindings.Clear()
        TextTotalRecibo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Saldo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextSaldo.DataBindings.Clear()
        TextSaldo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub MuestraCabezaPagoSueldos(ByVal DtCabeza As DataTable)

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Movimiento")
        AddHandler Enlace.Format, AddressOf Formatmovimiento
        TextNota.DataBindings.Clear()
        TextNota.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalRecibo.DataBindings.Clear()
        TextTotalRecibo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Saldo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextSaldo.DataBindings.Clear()
        TextSaldo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatMovimientoBancario(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = Format(Numero.Value, "0000-00000000")
        End If

    End Sub
    Private Sub FormatPrestamo(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = Format(Numero.Value, "0000-00000000")
        End If

    End Sub
    Private Sub Formatmovimiento(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub ArmaGridCompro()

        GridCompro.Rows.Clear()

        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Asignado") <> 0 Then
                GridCompro.Rows.Add(Row("Operacion"), "", Row("Tipo"), Row("TipoVisible"), Row("Comprobante"), Row("Recibo"), Row("Comentario"), Row("Fecha"), Row("Importe"), Row("Moneda"), Row("Saldo"), Row("Asignado"))
            End If
        Next

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        If PEsRechazoCheque And Funcion = "A" Then
            Return ArmaArchivosAsientoPorRechazo(Funcion, DtCabeza, DtDetalle)
        End If
        If PEsRechazoCheque And Funcion = "M" Then
            Return True
        End If

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

        If Not Asiento(TipoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Fecha, 0) Then Return False

        Return True

    End Function
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
    Public Function DtTiposComprobantes(ByVal Abierto As Boolean) As DataTable

        Dim Dt As New DataTable

        Dt.Columns.Add("Codigo", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Try
            Dim Row As DataRow = Dt.NewRow
            Row("Codigo") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 5000
            Row("Nombre") = "Factura"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 5020
            Row("Nombre") = "Devolución"
            Dt.Rows.Add(Row)
            '
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
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
    Private Function HacerAlta(ByVal DtNotaCabezaAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtChequesRechazadosAux As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroNota As Double
        Dim NumeroAsiento As Double
        Dim NumeroW As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion comprobante.
            If PTipoNota = 7005 Or PTipoNota = 7007 Then NumeroNota = UltimaNumeracionDebitoCreditoFondoFijo(ConexionNota)
            If PTipoNota = 1005 Or PTipoNota = 1007 Then NumeroNota = UltimaNumeracionPrestamo(ConexionNota)
            If PTipoNota = 93 Then NumeroNota = UltimaNumeracionBancaria(ConexionNota)
            If PTipoNota = 4007 Then NumeroNota = UltimaNumeracionPagoSueldos(ConexionNota)
            If NumeroNota < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            '
            Select Case PTipoNota
                Case 7005, 7007, 1005, 1007, 93, 4007
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
            End Select

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

            Select Case PTipoNota
                Case 7005, 7007
                    NumeroW = ActualizaNotaFondoFijo("A", DtNotaCabezaAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux)
                Case 1005, 1007
                    NumeroW = ActualizaNotaPrestamo("A", DtNotaCabezaAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux)
                Case 93
                    NumeroW = ActualizaNotaBancos("A", DtNotaCabezaAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux)
                Case 4007
                    NumeroW = ActualizaNotaPagoSueldos("A", DtNotaCabezaAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux)
            End Select

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
    Private Function HacerModificacion(ByVal DtNotaCabezaAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtChequesRechazadosAux As DataTable) As Boolean

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

            Select Case PTipoNota
                Case 7005, 7007
                    Resul = ActualizaNotaFondoFijo("M", DtNotaCabezaAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux)
                Case 1005, 1007
                    Resul = ActualizaNotaPrestamo("M", DtNotaCabezaAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux)
                Case 93
                    Resul = ActualizaNotaBancos("M", DtNotaCabezaAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux)
                Case 4007
                    Resul = ActualizaNotaPagoSueldos("M", DtNotaCabezaAux, DtMedioPagoAux, DtAsientoCabezaAux, DtAsientoDetalleAux, DtChequesRechazadosAux)
            End Select

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
    Private Function ActualizaNotaFondoFijo(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtChequesRechazadosAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "MovimientosFondoFijoCabeza", ConexionNota)
                    If Resul <= 0 Then Return 0
                End If
                '
                If Not IsNothing(DtMedioPagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "MovimientosFondoFijoPago", ConexionNota)
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
    Private Function ActualizaNotaPrestamo(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtChequesRechazadosAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "PrestamosMovimientoCabeza", ConexionNota)
                    If Resul <= 0 Then Return 0
                End If
                '
                If Not IsNothing(DtMedioPagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "PrestamosMovimientoPago", ConexionNota)
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
    Private Function ActualizaNotaBancos(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtChequesRechazadosAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "MovimientosBancarioCabeza", ConexionNota)
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
    Private Function ActualizaNotaPagoSueldos(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtChequesRechazadosAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "SueldosMovimientoCabeza", ConexionNota)
                    If Resul <= 0 Then Return 0
                End If
                '
                If Not IsNothing(DtMedioPagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "SueldosMovimientoPago", ConexionNota)
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
                Return DtNotaCabezaAux.Rows(0).Item("Movimiento")
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Public Function UltimaNumeracionDebitoCreditoFondoFijo(ByVal ConexionStr As String) As Integer

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
    Private Function UltimaNumeracionBancaria(ByVal ConexionStr) As Double

        Dim Patron As String = GCaja & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Movimiento) FROM MovimientosBancarioCabeza WHERE CAST(CAST(MovimientosBancarioCabeza.Movimiento AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
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
    Public Function UltimaNumeracionPagoSueldos(ByVal ConexionStr As String) As Integer

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
    Private Sub PideDatosProveedorFondoFijo()

        OpcionFondoFijo.ShowDialog()
        If OpcionFondoFijo.PRegresar Then Me.Close() : Exit Sub
        PProveedorFondoFijo = OpcionFondoFijo.PFondoFijo
        PNumero = OpcionFondoFijo.PNumeroFondoFijo
        PAbierto = OpcionFondoFijo.PAbierto
        OpcionFondoFijo.Dispose()

    End Sub
    Private Sub GeneraMediosPagoFondoFijo(ByVal DtMedioPagoAux As DataTable)

        Dim Row1 As DataRow
        Dim I As Integer = 0

        For Each Row As DataRow In DtGrid.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Row1 = DtMedioPagoAux.NewRow
                Row1("Movimiento") = 0
                I = I + 1
                Row1("Item") = I
                Row1("MedioPago") = Row("MedioPago")
                Row1("Cambio") = Row("Cambio")
                Row1("Importe") = Row("Importe")
                Row1("Banco") = Row("Banco")
                Row1("Cuenta") = Row("Cuenta")
                Row1("Comprobante") = Row("Comprobante")
                Row1("FechaComprobante") = Row("FechaComprobante")
                Row1("ClaveInterna") = Row("ClaveInterna")
                Row1("ClaveCheque") = Row("ClaveCheque")
                DtMedioPagoAux.Rows.Add(Row1)
            End If
        Next

    End Sub
    Private Sub GeneraMediosPagoPrestamo(ByVal DtMedioPagoAux As DataTable)

        Dim Row1 As DataRow

        'Actualizo MedioPago.
        For Each Row As DataRow In DtGrid.Rows
            Row1 = DtMedioPagoAux.NewRow
            Row1("Movimiento") = 0
            Row1("Item") = 0
            Row1("MedioPago") = Row("MedioPago")
            Row1("Detalle") = Row("Detalle")
            Row1("Neto") = Row("Neto")
            Row1("Alicuota") = Row("Alicuota")
            Row1("Cambio") = Row("Cambio")
            Row1("Importe") = Row("Importe")
            Row1("Banco") = Row("Banco")
            Row1("Cuenta") = Row("Cuenta")
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            DtMedioPagoAux.Rows.Add(Row1)
        Next

    End Sub
    Private Sub GeneraMediosPagoSueldos(ByVal DtMedioPagoAux As DataTable)

        Dim Row1 As DataRow

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

    End Sub
    Private Sub CalculaTotales()

        TotalConceptos = 0

        For Each Row As DataRow In DtGrid.Rows
            If Row("Cambio") = 0 Then
                TotalConceptos = TotalConceptos + Row("Importe")
            Else : TotalConceptos = TotalConceptos + Trunca(Row("Cambio") * Row("Importe"))
            End If
        Next

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
            Case 7005, 1005
                TextSaldo.Text = FormatNumber(CDec(TextTotalRecibo.Text) - ImputacionDeOtros - TotalFacturas, GDecimales)
            Case Else
                If PNota = 0 Then
                    TextSaldo.Text = FormatNumber(CDec(TextTotalRecibo.Text), GDecimales)
                End If
        End Select

    End Sub
    Private Sub ArmaChequeRechazadoFondoFijo()

        Dim Row As DataRow = DtGrid.NewRow
        Row("Item") = 0
        Row("MedioPago") = 100
        Row("Importe") = PImporteCheque
        Row("Cambio") = 0
        Row("Detalle") = "Rechazo Cheque " & FormatNumber(PClaveCheque, 0)
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

        CalculaTotales()

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

        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 11)

        x = MIzq + 135 : y = MTop

        If TextFechaContable.Visible Then
            Texto = TextFechaContable.Text
        Else
            Texto = Format(DateTime1.Value, "dd/MM/yyyy")
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y + 15)

        x = MIzq : y = MTop + 42

        Dim RazonSocial As String = ""
        Select Case PTipoNota
            Case 7005, 7007
                RazonSocial = TextEmisorFondoFijo.Text
            Case 1005, 1007
                RazonSocial = TextEmisorPrestamo.Text
            Case 93
                RazonSocial = TextEmisorBanco.Text & "  Cuenta: " & TextCuentaBancaria.Text
            Case 4007
                RazonSocial = TextLegajo.Text
        End Select

        Try
            'Titulos.
            If PAbierto Then
                Texto = "Razón Social: " & RazonSocial & "    Comprobante: " & TextNota.Text
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
                y = y + SaltoLinea
            Else
                Texto = "R. Social    : " & RazonSocial & "                Comprobante: " & TextNota.Text
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
                'Imprime Neto.
                Texto = FormatNumber(Row("Neto"), GDecimales)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Next
            'Resuardo
            Yq = MTop + 72 + Alto + 2
            If PAbierto Then
                Texto = GNombreEmpresa & " " & TextNota.Text
            Else
                Texto = TextNota.Text
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
    Private Function ArmaConceptosDebitoCredito100() As DataTable

        Dim dt As New DataTable

        Try
            dt = New DataTable
            dt.Columns.Add("Clave", Type.GetType("System.Int32"))
            dt.Columns.Add("Nombre", Type.GetType("System.String"))
            dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = " "
            Row("Tipo") = 0
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 100
            Row("Nombre") = "Importe"
            Row("Tipo") = 8
            dt.Rows.Add(Row)
            '
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
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

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
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
    



   
End Class