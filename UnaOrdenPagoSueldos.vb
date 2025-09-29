Imports System.Transactions
Imports System.Drawing.Printing
Imports System.IO
Public Class UnaOrdenPagoSueldos
    Public PNota As Double
    Public PAbierto As Boolean
    Public PTipoNota As Integer
    Public PLegajo As Integer
    Public PBloqueaFunciones As Boolean
    Public PImputa As Boolean
    '  Public PImporte As Double
    '
    Dim DtNotaDetalle As DataTable
    Dim DtNotaCabeza As DataTable
    Dim DtMedioPago As DataTable
    Dim DtRecibosCabeza As DataTable
    Dim DtGridCompro As DataTable
    Dim DtGrid As DataTable
    Dim DtFormasPago As DataTable
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim PagoEnPesos As Boolean
    Dim PagoEnTransferencia As Boolean
    Dim BancoPago As Integer
    Dim CuentaPago As Decimal
    '
    Dim cb As ComboBox
    Dim ConexionNota As String
    Dim TotalRecibos As Double
    Dim TotalConceptos As Double
    Dim UltimoNumero As Double = 0
    Dim UltimaFechaW As DateTime
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    '
    ''' Dim DirectorioDatos As String = "C:\ArchivosImportados\" + Format(CInt(GClaveEmpresa), "000") + "DatosOP.TXT"
    '' Dim DirectorioParametros As String = "C:\ArchivosImportados\" + Format(CInt(GClaveEmpresa), "000") + "ParametrosOP.TXT"
    Dim DirectorioDatos As String = "\\SERVER_RDAPP\" & "ArchivosImportados\" + Format(CInt(GClaveEmpresa), "000") + "DatosOP.TXT"
    Dim DirectorioParametros As String = "\\SERVER_RDAPP\" & "ArchivosImportados\" + Format(CInt(GClaveEmpresa), "000") + "ParametrosOP.TXT"

    Private Sub UnaOrdenPagoSueldos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(9) Then PBloqueaFunciones = True

        If GCaja = 0 And PNota = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PNota <> 0 Then
            Select Case PTipoNota
                Case 4010
                    If EsReemplazoChequeSueldos(PTipoNota, PNota, PAbierto) Then
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

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        GridCompro.AutoGenerateColumns = False
        GridCompro.Columns("Candado").DefaultCellStyle.NullValue = Nothing
        Grid.Columns("LupaCuenta").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")

        If PNota = 0 Then
            OpcionLegajos.PLegajo = PLegajo
            OpcionLegajos.ShowDialog()
            PLegajo = OpcionLegajos.PLegajo
            PAbierto = OpcionLegajos.PAbierto
            TextNombre.Text = OpcionLegajos.PNombre
            OpcionLegajos.Dispose()
            If PLegajo = 0 Then Me.Close() : Exit Sub
        End If

        If PAbierto Then
            ConexionNota = Conexion
        Else : ConexionNota = ConexionN
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        ArmaMedioPagoSueldo(DtFormasPago)

        LabelTipoNota.Text = "Orden de Pago - Sueldos"

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

        LlenaCombosGrid()

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        Grid.Columns("Detalle").Visible = False
        Grid.Columns("Neto").Visible = False
        Grid.Columns("Alicuota").Visible = False
        Grid.Columns("Iva").Visible = False

        Grid.Width = 0
        For i As Integer = 0 To Grid.Columns.Count - 1
            If Grid.Columns(i).Visible = True Then
                Grid.Width = Grid.Width + Grid.Columns(i).Width + 0
            End If
        Next
        Grid.Width = Grid.Width + 50
        Grid.Left = Panel2.Width / 2 - Grid.Width / 2
        ButtonEliminarLinea.Left = Grid.Left

        UltimaFechaW = UltimaFecha(Conexion)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

        PresentaLineasGrid(Grid, DtFormasPago, 600, False, False, PAbierto)

        TextImpresionOrdenPago.Text = ""

        'halla datos de parametros.
        If GCuitEmpresa = "30-70908844-7" Then
            Dim FechaAcreditacion As String, Comentario As String
            HallaDatosImportacion(FechaAcreditacion, Comentario)
            ButtonImportar.Text = ButtonImportar.Text + " Para Acreditación: " + FechaAcreditacion
        Else
            ButtonImportar.Visible = False
        End If

    End Sub
    Private Sub UnaOrdenPagoSueldos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        GridCompro.EndEdit()
        bs.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim RowsBusqueda() As DataRow

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtNotaDetalleAux = DtNotaDetalle.Copy
        Dim DtRecibosCabezaAux As DataTable = DtRecibosCabeza.Copy

        'Actualiza Archivos de la nota.
        If PNota = 0 Then ActualizaArchivos(DtNotaCabezaAux, DtMedioPagoAux)
        'Actualiza Saldo de Comprobantes Imputados.
        ActualizaComprobantes("M", DtRecibosCabezaAux)

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
                        Row("Movimiento") = DtNotaCabeza.Rows(0).Item("Movimiento")
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

        'Arma Asientos.
        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        '
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
            If HacerAlta(DtNotaCabezaAux, DtRecibosCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux) Then
                ArmaArchivos()
            End If
        Else
            If HacerModificacion(DtNotaCabezaAux, DtRecibosCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux) Then
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
            MsgBox("Usuario No Tiene Caja.", MsgBoxStyle.Information)
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

        If DtNotaCabeza.Rows(0).Item("ChequeRechazado") <> 0 Or DtNotaCabeza.Rows(0).Item("ClaveChequeReemplazado") <> 0 Then
            MsgBox("Anulación Nota Por Cheque Rechazado o Reemplazado Debe Realizarce Por Menu Rechazo de Cheques. Operación se CANCELA.")
            Exit Sub
        End If

        If CInt(LabelCaja.Text) <> GCaja And Not GAdministrador Then
            MsgBox("Modificación Solo Permitida para Caja " & LabelCaja.Text & " o un Administrador. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy

        'Actualiza Saldo de Comprobantes Imputados.
        Dim DtRecibosCabezaAux As DataTable = DtRecibosCabeza.Copy

        ActualizaComprobantes("M", DtRecibosCabezaAux)

        If Not (IsNothing(DtRecibosCabezaAux.GetChanges)) Then
            MsgBox("Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        '
        If Not HallaAsientosCabeza(4010, DtNotaCabezaAux.Rows(0).Item("Movimiento"), DtAsientoCabezaAux, ConexionNota) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If
        For Each Row As DataRow In DtAsientoCabezaAux.Rows
            Row("Estado") = 3
        Next

        Dim Resul As Double

        For Each Row As DataGridViewRow In Grid.Rows
            If IsDBNull(Row.Cells("Concepto").Value) Then Exit For
            If Row.Cells("Concepto").Value = 2 Or Row.Cells("Concepto").Value = 3 Then
                Dim Anulado As Boolean, Rechazado As Boolean, Depositado As Boolean, Entregado As Boolean, Afectado As Boolean
                If Not EstadoCheque(Row.Cells("Concepto").Value, Row.Cells("ClaveCheque").Value, ConexionNota, Anulado, Rechazado, Depositado, Entregado, Afectado) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Error Base de Datos")
                    Exit Sub
                End If
                If Anulado Then
                    MsgBox("Cheque " & Row.Cells("Numero").Value & " no se puede Borrar. Cheque Anulado. Operación se CANCELA.")
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If Rechazado Then
                    MsgBox("Cheque " & Row.Cells("Numero").Value & " no se puede Borrar. Existe Nota de Rechazo. Operación se CANCELA.")
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If Row.Cells("Concepto").Value = 2 Then
                    If Depositado Then
                        MsgBox("Cheque " & Row.Cells("Numero").Value & " no se puede Borrar. Fue Depositado. Operación se CANCELA.")
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        Exit Sub
                    End If
                End If
            End If
        Next

        If PTipoNota = 4010 Then
            For Each Row As DataGridViewRow In Grid.Rows
                If IsDBNull(Row.Cells("Concepto").Value) Then Exit For
                If Row.Cells("Concepto").Value = 2 Or Row.Cells("Concepto").Value = 3 Then
                    If ChequeReemplazado(Row.Cells("Concepto").Value, Row.Cells("ClaveCheque").Value, PTipoNota, PNota, ConexionNota) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row.Cells("Numero").Value & " no se puede Borrar, fue Reemplazado. Operación se CANCELA.")
                        Exit Sub
                    End If
                End If
            Next
        End If

        If MsgBox("Nota se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        'Actualiza Saldo de Comprobantes Imputados.
        DtRecibosCabezaAux = DtRecibosCabeza.Copy

        ActualizaComprobantes("B", DtRecibosCabezaAux)

        DtNotaCabezaAux.Rows(0).Item("Estado") = 3

        Resul = ActualizaNota("B", DtNotaCabezaAux, DtRecibosCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux)
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
            MsgBox("Opcion Invalida. Nota debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Paginas = 0
        If Not PAbierto Then Copias = 1

        Dim print_document As New PrintDocument
        AddHandler print_document.PrintPage, AddressOf Print_Print

        print_document.Print()


    End Sub
    Private Sub ButtonImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportar.Click

        If Strings.Left(ButtonImportar.Text, 2) = "No" Then Exit Sub

        Dim CodigoBanco As Integer
        Dim Cuenta As String
        Dim Importe As Decimal

        Dim Cuil As Long = HallaCuilEmpleado(PLegajo, Conexion)

        If Not BuscaEmpleado(DirectorioDatos, Cuil, CodigoBanco, Cuenta, Importe) Then Exit Sub

        If Importe = 0 Then MsgBox("Empleado no tiene transferencia.") : Exit Sub

        'halla datos de parametros.
        Dim FechaAcreditacion As String, Comentario As String
        HallaDatosImportacion(FechaAcreditacion, Comentario)
        ButtonImportar.Text = ButtonImportar.Text + " Para Acreditación: " + FechaAcreditacion

        TextComentario.Text = Comentario
        DateTime1.Text = FechaAcreditacion

        DtGrid.Clear()

        Dim Row As DataRow = DtGrid.NewRow
        Row("Item") = 0
        Row("MedioPago") = 11
        Row("Detalle") = ""
        Row("Neto") = 0
        Row("Alicuota") = 0
        Row("ImporteIva") = 0
        Row("Banco") = CodigoBanco
        Row("Fecha") = "1/1/1800"
        Row("Cuenta") = Cuenta
        Row("Serie") = ""
        Row("Numero") = 0
        Row("EmisorCheque") = ""
        Row("Cambio") = 0
        Row("Importe") = Importe
        Row("Comprobante") = 0
        Row("FechaComprobante") = DateTime1.Value
        Row("ClaveCheque") = 0
        Row("ClaveChequeVisual") = 0
        Row("eCheq") = False
        DtGrid.Rows.Add(Row)


        PresentaLineasGrid(Grid, DtFormasPago, 600, False, False, PAbierto)

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PNota = 0
        UnaOrdenPagoSueldos_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PNota = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 4010
        If PAbierto Then
            ListaAsientos.PDocumentoB = PNota
        Else
            ListaAsientos.PDocumentoN = PNota
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then Exit Sub

        Dim Row As DataRow
        Row = DtGrid.Rows.Item(Grid.CurrentRow.Index)
        Row.Delete()

        CalculaTotales()

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()
        CreaDtGridCompro()

        Dim Sql As String
        Dim Row1 As DataRow
        Dim RowsBusqueda() As DataRow

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM SueldosMovimientoCabeza WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaCabeza) Then Return False
        If PNota <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then
            Dim Row As DataRow = DtNotaCabeza.NewRow
            ArmaNuevoPagoSueldo(Row)
            Row("Legajo") = PLegajo
            Row("TipoNota") = PTipoNota
            Row("Fecha") = Now
            Row("Caja") = GCaja
            Row("Estado") = 1
            DtNotaCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        PLegajo = DtNotaCabeza.Rows(0).Item("Legajo")
        LeerLegajo()

        DtNotaDetalle = New DataTable
        Sql = "SELECT * FROM SueldosMovimientoDetalle WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaDetalle) Then Return False

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM SueldosMovimientoPago WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtMedioPago) Then Return False

        Dim DtCheques As New DataTable
        For Each Row As DataRow In DtMedioPago.Rows
            If Row("ClaveCheque") <> 0 Then
                If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionNota, DtCheques) Then Return False
            End If
        Next

        For Each Row As DataRow In DtMedioPago.Rows
            Row1 = DtGrid.NewRow
            Row1("Item") = Row("Item")
            Row1("MedioPago") = Row("MedioPago")
            Row1("Importe") = Row("Importe")
            Row1("Cambio") = Row("Cambio")
            Row1("Bultos") = 0
            Row1("Detalle") = 0
            Row1("Alicuota") = 0
            Row1("Neto") = 0
            Row1("ImporteIva") = 0
            Row1("NumeracionInicial") = 1
            Row1("NumeracionFinal") = 999999999
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
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
                    Row1("eCheq") = RowsBusqueda(0).Item("eCheq")
                Else
                    Row1("Banco") = 0
                    Row1("Cuenta") = 0
                    Row1("Serie") = ""
                    Row1("Numero") = 0
                    Row1("EmisorCheque") = "Comp.No Existe."
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
            DtGrid.Rows.Add(Row1)
        Next

        If DtGrid.Rows.Count = 0 And (PagoEnPesos Or PagoEnTransferencia) Then 'caso legajo tiene medios-pago informados.
            Row1 = DtGrid.NewRow
            Row1("Item") = 0
            If PagoEnPesos Then
                Row1("MedioPago") = 1
            End If
            If PagoEnTransferencia Then
                Row1("MedioPago") = 11
            End If
            Row1("Detalle") = ""
            Row1("Neto") = 0
            Row1("Alicuota") = 0
            Row1("ImporteIva") = 0
            Row1("Banco") = 0
            Row1("Cuenta") = 0
            If PagoEnTransferencia Then Row1("Banco") = BancoPago
            If PagoEnTransferencia Then Row1("Cuenta") = CuentaPago
            Row1("Fecha") = "1/1/1800"
            Row1("Serie") = ""
            Row1("Numero") = 0
            Row1("EmisorCheque") = ""
            Row1("Cambio") = 0
            Row1("Importe") = 0
            Row1("Comprobante") = 0
            Row1("FechaComprobante") = "1/1/1800"
            Row1("ClaveCheque") = 0
            Row1("ClaveChequeVisual") = 0
            Row1("eCheq") = False
            DtGrid.Rows.Add(Row1)
        End If

        DtCheques.Dispose()

        Grid.DataSource = DtGrid
        Grid.EndEdit()

        'Precenta las lineas del grid.
        For Each Row As DataGridViewRow In Grid.Rows
            If Not IsNothing(Row.Cells("Concepto").Value) Then
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row.Cells("Concepto").Value)
                ArmaGridSegunConcepto(Row, RowsBusqueda(0).Item("Tipo"), PTipoNota, False, False, PAbierto)
            End If
        Next

        'Muestra Comprobantes a Imputar.
        DtRecibosCabeza = New DataTable
        If Not ArmaConRecibos() Then Return False

        'Procesa Recibos.
        DtGridCompro.Clear()
        For Each Row As DataRow In DtRecibosCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 4000
            Row1("Comprobante") = Row("Recibo")
            Row1("Fecha") = Row("Fecha")
            Row1("Importe") = Row("Importe")
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next

        For Each Row As DataRow In DtNotaDetalle.Rows
            RowsBusqueda = DtGridCompro.Select("Tipo = " & Row("TipoComprobante") & " AND Comprobante = " & Row("Comprobante"))
            If RowsBusqueda.Length <> 0 Then RowsBusqueda(0).Item("Asignado") = Row("Importe")
        Next

        'Borra los documentos con saldo 0 y no tienen asignacion. 
        DtGridCompro.AcceptChanges()
        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Saldo") = 0 And Row("Asignado") = 0 Then Row.Delete()
        Next

        GridCompro.DataSource = DtGridCompro

        If PNota <> 0 Then
            ButtonEliminarLinea.Enabled = False
            Grid.ReadOnly = True
        Else
            ButtonEliminarLinea.Enabled = True
            Grid.ReadOnly = False
        End If

        CalculaTotales()

        AddHandler DtGridCompro.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGridCompro_ColumnChanging)
        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Function ArmaConRecibos() As Boolean

        Dim Sql As String = ""

        If PNota = 0 Then
            Sql = "SELECT * FROM RecibosSueldosCabeza WHERE Estado = 1 AND Legajo = " & PLegajo & " AND Saldo <> 0 ORDER BY Recibo,Fecha;"
        Else
            Sql = "SELECT * FROM RecibosSueldosCabeza WHERE Estado = 1 AND Legajo = " & PLegajo & " ORDER BY Recibo,Fecha;"
        End If
        If Not Tablas.Read(Sql, ConexionNota, DtRecibosCabeza) Then Return False

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtNotaCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Movimiento")
        AddHandler Enlace.Format, AddressOf FormatNota
        TextComprobante.DataBindings.Clear()
        TextComprobante.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalPago.DataBindings.Clear()
        TextTotalPago.DataBindings.Add(Enlace)

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
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub FormatNota(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Concepto.DataSource = DtFormasPago
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

        Tipo.DataSource = ArmaMovimientosSueldo()
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Clave"

        Banco.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 order By Nombre;")
        Row = Banco.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Banco.DataSource.Rows.Add(Row)
        Banco.DisplayMember = "Nombre"
        Banco.ValueMember = "Clave"

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
    Private Sub CalculaTotales()

        TotalConceptos = 0
        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Cambio").Value = 0 Then
                TotalConceptos = TotalConceptos + Row.Cells("Importe").Value
            Else : TotalConceptos = TotalConceptos + Row.Cells("Cambio").Value * Row.Cells("Importe").Value
            End If
        Next

        If DtNotaCabeza.Rows(0).Item("Importe") = 0 And PNota <> 0 Then
            TotalConceptos = 0
        End If

        TextTotalPago.Text = FormatNumber(TotalConceptos, GDecimales)

        TotalRecibos = 0
        For Each Row As DataRow In DtGridCompro.Rows
            If Row.RowState <> DataRowState.Deleted Then
                TotalRecibos = TotalRecibos + Row("Asignado")
            End If
        Next
        TextTotalRecibos.Text = FormatNumber(TotalRecibos, GDecimales)

        TextSaldo.Text = FormatNumber(TotalConceptos - TotalRecibos, GDecimales)

    End Sub
    Private Function HacerAlta(ByVal DtNotaCabezaAux As DataTable, ByVal DtRecibosCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroNota As Double
        Dim NumeroAsiento As Double
        Dim NumeroW As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
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

            NumeroW = ActualizaNota("A", DtNotaCabezaAux, DtRecibosCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -2 Then Exit For
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
    Private Function HacerModificacion(ByVal DtNotaCabezaAux As DataTable, ByVal DtRecibosCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable) As Boolean

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
            '
            Resul = ActualizaNota("M", DtNotaCabezaAux, DtRecibosCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux)
            '
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
    Private Function ActualizaNota(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtRecibosCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable) As Double

        Dim Resul As Double
        Dim RowsBusqueda() As DataRow

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "SueldosMovimientoCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtNotaDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaDetalleAux.GetChanges, "SueldosMovimientoDetalle", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                For I As Integer = 0 To DtMedioPagoAux.Rows.Count - 1
                    Dim Row As DataRow = DtMedioPagoAux.Rows(I)
                    If Row.RowState = DataRowState.Added Then
                        If Row("MedioPago") = 2 Then
                            Row("ClaveCheque") = ActualizaClavesComprobantes("A", 0, 4010, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq"))
                            If Row("ClaveCheque") <= 0 Then Return Row("ClaveCheque")
                        End If
                        If Row("MedioPago") = 3 Then
                            If ActualizaClavesComprobantes("A", Row("ClaveCheque"), 4010, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), DtGrid.Rows(I).Item("EmisorCheque"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq")) <= 0 Then
                                MsgBox("Banco " & Row("Banco") & " Cuenta " & Row("Cuenta") & " Error al Grabar Cheque.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            End If
                        End If
                    End If
                Next
                '
                If Funcion <> "B" Then
                    For Each Row As DataRow In DtMedioPago.Rows
                        If Row("MedioPago") = 3 Or Row("MedioPago") = 2 Then
                            RowsBusqueda = DtMedioPagoAux.Select("ClaveCheque = " & Row("ClaveCheque"))
                            If RowsBusqueda.Length = 0 Then
                                If Row("MedioPago") = 3 Then
                                    If ActualizaClavesComprobantes("B", Row("ClaveCheque"), 4010, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", 0, "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                        MsgBox("Cheque " & Row("ClaveCheque") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    Else
                                    End If
                                End If
                                If Row("MedioPago") = 2 Then
                                    If ActualizaClavesComprobantes("B", Row("ClaveCheque"), 4010, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                        MsgBox("Cheque " & Row("Banco") & " " & Row("Cuenta") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    Else
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
                '
                If Funcion = "B" Then
                    For Each Row As DataRow In DtMedioPagoAux.Rows
                        If Row("MedioPago") = 3 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), 4010, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("ClaveCheque") & " " & Row("Cuenta") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            Else
                            End If
                        End If
                        If Row("MedioPago") = 2 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), 4010, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
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
                If Not IsNothing(DtRecibosCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtRecibosCabezaAux.GetChanges, "RecibosSueldosCabeza", ConexionNota)
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
                '
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Sub ActualizaArchivos(ByRef DtNotaCabezaAux As DataTable, ByRef DtMediopagoAux As DataTable)

        'Actualizo MedioPago.
        For Each Row As DataRow In DtGrid.Rows
            Dim Row1 As DataRow = DtMediopagoAux.NewRow
            Row1("Movimiento") = DtNotaCabezaAux.Rows(0).Item("Movimiento")
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
            DtMediopagoAux.Rows.Add(Row1)
        Next

    End Sub
    Private Sub ActualizaComprobantes(ByVal Funcion As String, ByVal DtRecibosCabezaAux As DataTable)

        Dim RowsBusqueda() As DataRow

        'Actualiza Saldo de Comprobantes Imputados.

        If Funcion = "M" Then
            For Each Row1 As DataRow In DtGridCompro.Rows
                If Row1.RowState <> DataRowState.Deleted Then
                    If Row1("Saldo") <> Row1("SaldoAnt") Then
                        If Row1("Tipo") = 4000 Then
                            RowsBusqueda = DtRecibosCabezaAux.Select("Recibo = " & Row1("Comprobante"))
                            RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
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
                        If Row1("Tipo") = 4000 Then
                            RowsBusqueda = DtRecibosCabezaAux.Select("Recibo = " & Row1("Comprobante"))
                            RowsBusqueda(0).Item("Saldo") = Trunca(RowsBusqueda(0).Item("Saldo") + Row1.Item("Asignado"))
                        End If
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
    Public Sub LeerLegajo()

        Dim ConexionLegajo As String

        If DtNotaCabeza.Rows(0).Item("Legajo") < 5000 Then
            ConexionLegajo = Conexion
        Else : ConexionLegajo = ConexionN
        End If

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Nombres,Apellidos,Legajo,Banco,Cuenta,PagoEnPesos,PagoEnTransferencia,BancoPago,CuentaPago FROM Empleados WHERE Legajo = " & DtNotaCabeza.Rows(0).Item("Legajo") & ";", ConexionLegajo, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            TextNombre.Text = Format(Dt.Rows(0).Item("Legajo"), "0000") & " - " & Dt.Rows(0).Item("Nombres") & " " & Dt.Rows(0).Item("Apellidos")
            PagoEnPesos = Dt.Rows(0).Item("PagoEnPesos") : PagoEnTransferencia = Dt.Rows(0).Item("PagoEnTransferencia") : BancoPago = Dt.Rows(0).Item("BancoPago") : CuentaPago = Dt.Rows(0).Item("CuentaPago")
        End If

        Dt.Dispose()

    End Sub
    Private Function HallaSecuenciaCheque(ByVal Banco As Integer, ByVal Cuenta As Double, ByVal UltimoNumero As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGrid.Select("eCheq = 0 AND Banco = " & Banco & " AND Cuenta = " & Cuenta)
        Return UltimoNumero + 1 + RowsBusqueda.Length

    End Function
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos
        Dim RowsBusqueda() As DataRow

        If Funcion = "A" Then
            Dim ImporteTotal As Double
            For Each Row As DataRow In DtGrid.Rows
                Item = New ItemListaConceptosAsientos
                Item.Clave = Row("MedioPago")
                Item.Importe = Row("Importe")
                ImporteTotal = ImporteTotal + Row("Importe")
                ListaConceptos.Add(Item)
            Next
            Item = New ItemListaConceptosAsientos
            Item.Clave = 213
            Item.Importe = ImporteTotal
            ListaConceptos.Add(Item)
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

        If Not Asiento(4010, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

        Return True

    End Function
    Private Sub Print_Print(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
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
            Texto = "ORDEN DE PAGO"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 140, MTop)
            ' 
            PrintFont = New Font("Courier New", 12)
            Texto = "Nro. Orden Pago:  " & NumeroEditado(TextComprobante.Text)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
            Texto = "Fecha:  " & DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
            Texto = "Nombre: " & TextNombre.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 26)
            Texto = "        " & TextImpresionOrdenPago.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 33)
            Texto = "Importe Orden : " & TextTotalPago.Text
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
            For Each Row As DataGridViewRow In Grid.Rows
                If IsNothing(Row.Cells("Concepto").Value) Then Exit For
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row.Cells("Concepto").Value)
                Yq = Yq + SaltoLinea
                'Imprime Detalle.
                Texto = Row.Cells("Concepto").FormattedValue
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                If Row.Cells("Cambio").Value <> 0 Then
                    'Imprime Cambio.
                    Texto = Row.Cells("Cambio").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaCambio - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Importe.
                    Texto = FormatNumber(Trunca(Row.Cells("Cambio").Value * Row.Cells("Importe").Value), GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Else
                    'Imprime Importe.
                    Texto = Row.Cells("Importe").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
                'Imprime Banco.
                Texto = Row.Cells("Banco").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte + 1
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Numero.
                Texto = Row.Cells("Numero").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaNumero - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Vencimeinto.
                Texto = Row.Cells("Fecha").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaVencimiento - Longi - 2
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Comprobante.
                If Row.Cells("Comprobante").FormattedValue <> "" Then
                    Texto = Row.Cells("Comprobante").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaNumero - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
                'Imprime FechaComprobante.
                If Row.Cells("FechaComprobante").FormattedValue <> "" Then
                    Texto = Row.Cells("FechaComprobante").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaVencimiento - Longi - 2
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
            Next
            'Grafica -Rectangulo Imputacion. ----------------------------------------------------------------------
            y = MTop + 150
            Ancho = 183
            Alto = 50
            Dim LineaTipo As Integer = x + 35
            Dim LineaComprobante As Integer = x + 69
            Dim LineaFecha As Integer = x + 94
            Dim LineaImporte1 As Integer = x + 125
            Dim LineaSaldo As Integer = x + 155
            Dim LineaImporte2 As Integer = x + Ancho
            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaTipo, y, LineaTipo, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaComprobante, y, LineaComprobante, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaFecha, y, LineaFecha, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte1, y, LineaImporte1, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaSaldo, y, LineaSaldo, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte2, y, LineaImporte2, y + Alto)
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
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)
            'Descripcion Imputacion.
            Yq = y - SaltoLinea
            For Each Row As DataGridViewRow In GridCompro.Rows
                If Row.Cells("Asignado").Value <> 0 Then
                    Yq = Yq + SaltoLinea
                    'Imprime Tipo.
                    Texto = Row.Cells("Tipo").FormattedValue
                    Xq = x
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Comprobante.
                    Texto = Row.Cells("Comprobante1").FormattedValue
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
            e.HasMorePages = False
        End Try

    End Sub
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

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
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM SueldosMovimientoCabeza;", Miconexion)
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
    Private Sub IngresaCheques(ByVal ListaDeChequesAux As List(Of ItemCheque))

        Grid.DataSource = Nothing
        Dim Dt As DataTable = DtGrid.Copy
        DtGrid.Clear()

        For Each Row As DataRow In Dt.Rows
            If Row("Item") <> 0 Then
                DtGrid.ImportRow(Row)
            End If
            If Row("Item") = 0 And Row("MedioPago") <> ListaDeChequesAux(0).MedioPago Then
                DtGrid.ImportRow(Row)
            End If
        Next
        For Each Item As ItemCheque In ListaDeChequesAux
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Item") = 0
            Row1("MedioPago") = Item.MedioPago
            Row1("Detalle") = ""
            Row1("Neto") = 0
            Row1("Alicuota") = 0
            Row1("ImporteIva") = 0
            Row1("Banco") = Item.Banco
            Row1("Fecha") = Item.Fecha
            Row1("Cuenta") = Item.Cuenta
            Row1("Serie") = Item.Serie
            Row1("Numero") = Item.Numero
            Row1("EmisorCheque") = Item.EmisorCheque
            Row1("Cambio") = 0
            Row1("Importe") = Item.Importe
            Row1("Comprobante") = 0
            Row1("FechaComprobante") = "1/1/1800"
            Row1("ClaveCheque") = Item.ClaveCheque
            Row1("ClaveChequeVisual") = Item.ClaveCheque
            Row1("eCheq") = Item.echeq
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = DtGrid
        PresentaLineasGrid(Grid, DtFormasPago, 600, False, False, PAbierto)
        Grid.EndEdit()

        Dt.Dispose()

    End Sub
    Private Function BuscaEmpleado(ByVal DirectorioDatos As String, ByVal Cuil As String, ByRef CodigoBanco As Integer, ByRef Cuenta As String, ByRef Importe As Decimal) As Boolean

        Dim Linea As String
        Dim Datos() As String

        If Not File.Exists(DirectorioDatos) Then
            MsgBox("No se encontro Datos para esta Fecha de Acreditación") : Return False
        End If

        'Lee Datos.
        Try
            Using reader As StreamReader = New StreamReader(DirectorioDatos)
                Linea = reader.ReadLine

                While Linea <> ""
                    Datos = Split(Linea, ";")
                    If Datos(0) = Cuil Then
                        CodigoBanco = Datos(1)
                        Cuenta = Datos(2)
                        Importe = Datos(3)
                        '''''   Comentario = Datos(5)
                        Return True
                    End If
                    Linea = reader.ReadLine
                End While
                MsgBox("NO SE ENCONTRO EMPLEADO EN EL ARCHIVO DE IMPORTACION!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "ATENCION!") : Return False
            End Using
        Catch exIO As IOException
            MsgBox("ERROR EN LA IMPORTACION DEL ARCHIVO: ---> " & exIO.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Return False
        Catch ex As Exception
            MsgBox("ERROR EN LA IMPORTACION DEL ARCHIVO: ---> " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Return False
        End Try

    End Function
    Private Sub HallaDatosImportacion(ByRef Fecha As String, ByRef Comentario As String)

        If Not File.Exists(DirectorioParametros) Then ButtonImportar.Text = "No hay Datos de Importacion." : Exit Sub

        Dim Linea As String

        Try
            Using reader As StreamReader = New StreamReader(DirectorioParametros)
                Linea = reader.ReadLine
                Fecha = Strings.Left(Linea, 10)
                Comentario = Strings.Right(Linea, Linea.Length - 10)
            End Using
        Catch ex As Exception
            MsgBox("No se pudo leer Archivo Importado.  " + ex.Message)
        End Try

    End Sub
    Public Function HallaCuilEmpleado(ByVal Legajo As Integer, ByVal ConexionStr As String) As Long

        Dim Cuil As Long
        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Dim Cmd As New OleDb.OleDbCommand("SELECT Cuil FROM Empleados WHERE Legajo = " & Legajo & ";", Miconexion)
                Miconexion.Open()
                Cuil = Cmd.ExecuteScalar()
                Miconexion.Close()
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

        Return Cuil

    End Function
    Private Function Valida() As Boolean

        If PNota = 0 Then
            If Not ConsistePagos(Grid, DtFormasPago, PTipoNota, False) Then Return False
        End If

        If TotalConceptos = 0 Then
            MsgBox("Debe informar Importes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TotalConceptos - TotalRecibos < 0 Then
            MsgBox("Importes Imputados supera importe del Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If GCuitEmpresa <> GPatagonia And GCuitEmpresa <> GPatagoniaFresh Then   'Arreglo excepcional pedido por Federico.
            If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 And PNota = 0 Then
                MsgBox("Fecha Incorrecta. Existe Otro Comprobante con Fecha " & UltimaFechaW & " Posterior a la Informada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                DateTime1.Focus()
                Return False
            End If
        End If

        For Each Row As DataRow In DtGrid.Rows
            If Row("MedioPago") = 11 Then
                If ExisteCuentaBancaria(Row("Banco"), Row("Cuenta")) = 0 Then
                    MsgBox("Cuenta de Pago " & Row("Cuenta") & " Informada para el Legajo No Existe.")
                    Return False
                End If
            End If
        Next

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.CellBeginEdit

        If Grid.Columns(e.ColumnIndex).Name = "eCheq" Then
            If Grid.Rows(e.RowIndex).Cells("Concepto").Value <> 2 Then e.Cancel = True
        End If

    End Sub
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        Dim RowsBusqueda() As DataRow
        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Or Grid.Columns(e.ColumnIndex).Name = "Banco" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
                If cb.SelectedIndex < 0 Then cb.SelectedValue = 0
            Else
                Exit Sub
            End If

            If IsNothing(cb.SelectedValue) Then Exit Sub

            If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
                If cb.SelectedIndex <= 0 Then Exit Sub
                RowsBusqueda = DtFormasPago.Select("Clave = " & cb.SelectedValue)
                ArmaGridSegunConcepto(Grid.Rows(e.RowIndex), RowsBusqueda(0).Item("Tipo"), 600, False, False, PAbierto)
            End If
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        If Grid.Columns(e.ColumnIndex).Name = "Neto" Or Grid.Columns(e.ColumnIndex).Name = "Alicuota" Or _
           Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "Cambio" Then
            If IsDBNull(Grid.CurrentCell.ToString) Then Grid.CurrentCell.Value = 0
            CalculaTotales()
        End If

        If GCuitEmpresa <> GPatagonia Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
            If IsDBNull(Grid.Rows(e.RowIndex)) Then Exit Sub
            If IsNothing(Grid.Rows(e.RowIndex)) Then Exit Sub

            Select Case Grid.Rows(e.RowIndex).Cells("Concepto").Value
                Case 4, 11, 12
                    Grid.Rows(e.RowIndex).Cells("Comprobante").Value = Grid.CurrentRow.Index + 1
                    Grid.Rows(e.RowIndex).Cells("FechaComprobante").Value = Today.Date
            End Select
        End If

    End Sub
    Private Sub Grid_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellValueChanged

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Concepto")) Then
                Grid.CurrentRow.Cells("Neto").Value = 0
                Grid.CurrentRow.Cells("Alicuota").Value = 0
                Grid.CurrentRow.Cells("Iva").Value = 0
                Grid.CurrentRow.Cells("Importe").Value = 0
                Grid.CurrentRow.Cells("Banco").Value = 0
                Grid.CurrentRow.Cells("Cuenta").Value = 0
                Grid.CurrentRow.Cells("Serie").Value = ""
                Grid.CurrentRow.Cells("Numero").Value = 0
                Grid.CurrentRow.Cells("Fecha").Value = "1/1/1800"
                Grid.CurrentRow.Cells("Cambio").Value = 0
                Grid.CurrentRow.Cells("EmisorCheque").Value = ""
                Grid.CurrentRow.Cells("ClaveCheque").Value = 0
                Grid.CurrentRow.Cells("ClaveChequeVisual").Value = 0
                Grid.CurrentRow.Cells("Comprobante").Value = 0
                Grid.CurrentRow.Cells("FechaComprobante").Value = "1/1/1800"
                Grid.Refresh()
                CalculaTotales()
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Banco" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Banco")) Then
                Grid.CurrentRow.Cells("Cuenta").Value = 0
                Grid.Refresh()
            End If
        End If

    End Sub
    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If PNota <> 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta" Then
            If IsDBNull(Grid.CurrentRow.Cells("Concepto").Value) Then Exit Sub
            If Grid.CurrentRow.Cells("Concepto").Value = 2 Then
                ListaBancos.PEsSeleccionaCuenta = True
                ListaBancos.PEsSoloPesos = True
                ListaBancos.PConChequera = True
                ListaBancos.ShowDialog()
                If ListaBancos.PCuenta <> 0 Then
                    Grid.CurrentRow.Cells("Banco").Value = ListaBancos.PBanco
                    Grid.CurrentRow.Cells("Cuenta").Value = ListaBancos.PCuenta
                    Grid.CurrentRow.Cells("Serie").Value = ListaBancos.PSerie
                    Grid.CurrentRow.Cells("NumeracionInicial").Value = ListaBancos.PNumeracionInicial
                    Grid.CurrentRow.Cells("NumeracionFinal").Value = ListaBancos.PNumeracionFinal
                    If Not Grid.CurrentRow.Cells("eCheq").Value Then
                        Grid.CurrentRow.Cells("Numero").Value = HallaSecuenciaCheque(ListaBancos.PBanco, ListaBancos.PCuenta, ListaBancos.PUltimoNumero)
                        Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Numero")
                        'EnumeraCheques()
                    End If
                End If
                ListaBancos.Dispose()
            End If
            '
            If HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 7 Then
                ListaBancos.PEsSeleccionaCuenta = True
                ListaBancos.PEsSoloPesos = True
                ListaBancos.ShowDialog()
                If ListaBancos.PCuenta <> 0 Then
                    Grid.CurrentRow.Cells("Banco").Value = ListaBancos.PBanco
                    Grid.CurrentRow.Cells("Cuenta").Value = ListaBancos.PCuenta
                    Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Comprobante")
                End If
                ListaBancos.Dispose()
            End If
            '
            If Grid.CurrentRow.Cells("Concepto").Value = 3 Then
                If PTipoNota = 60 Then Exit Sub
                SeleccionarCheques.PEsChequeEnCartera = True
                SeleccionarCheques.PCaja = GCaja
                SeleccionarCheques.PAbierto = PAbierto
                SeleccionarCheques.PListaDeCheques = New List(Of ItemCheque)
                For Each Row As DataRow In DtGrid.Rows
                    If Row("MedioPago") = 3 And Row("ClaveCheque") <> 0 Then
                        Dim Item As New ItemCheque
                        Item.ClaveCheque = Row("ClaveCheque")
                        SeleccionarCheques.PListaDeCheques.Add(Item)
                    End If
                Next
                SeleccionarCheques.ShowDialog()
                If SeleccionarCheques.PListaDeCheques.Count <> 0 Then
                    IngresaCheques(SeleccionarCheques.PListaDeCheques)
                    CalculaTotales()
                End If
                SeleccionarCheques.Dispose()
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Grid.Rows(e.RowIndex).Cells("Concepto").Value = 2 Or (PTipoNota = 60 And Grid.Rows(e.RowIndex).Cells("Concepto").Value = 3) Then
                Calendario.ShowDialog()
                Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                Grid.EndEdit()
                Calendario.Dispose()
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FechaComprobante" Then
            If HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 7 Or (HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 4 And PTipoNota = 60) Then
                Calendario.ShowDialog()
                Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                Grid.EndEdit()
                Calendario.Dispose()
            End If
            If HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 10 Then
                Calendario.ShowDialog()
                Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                Grid.EndEdit()
                Calendario.Dispose()
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "Neto" Or _
           Grid.Columns(e.ColumnIndex).Name = "Alicuota" Or Grid.Columns(e.ColumnIndex).Name = "Iva" Or Grid.Columns(e.ColumnIndex).Name = "Cambio" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaComprobante" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = "1/1/1800" Then e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Or Grid.Columns(e.ColumnIndex).Name = "Comprobante" Or Grid.Columns(e.ColumnIndex).Name = "Cuenta" Or Grid.Columns(e.ColumnIndex).Name = "ClaveChequeVisual" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, 0)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta" Then
            e.Value = Nothing
        End If

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

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Detalle" Then Exit Sub
        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Banco" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Not Grid.Columns(Grid.CurrentCell.ColumnIndex).Name = "Concepto" Then
            If IsDBNull(Grid.CurrentRow.Cells("Concepto").Value) Then
                e.KeyChar = ""
                Exit Sub
            End If
            If Grid.CurrentRow.Cells("Concepto").Value = 0 Then
                e.KeyChar = ""
                Exit Sub
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Neto" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Alicuota" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cambio" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Numero" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Comprobante" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cuenta" Then
            If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Serie" Then
            If e.KeyChar = "" Then Exit Sub
            e.KeyChar = e.KeyChar.ToString.ToUpper
            If Asc(e.KeyChar) < 65 Or Asc(e.KeyChar) > 90 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Neto" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Alicuota" Or _
           Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cambio" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row

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
        Row("eCheq") = False

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
                Grid.Refresh()
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
            If Not IsDBNull(e.Row("Neto")) Then
                e.Row("ImporteIva") = Trunca(e.ProposedValue * e.Row("Neto") / 100)
                e.Row("Importe") = e.Row("Neto") + e.Row("ImporteIva")
                Grid.Refresh()
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

        If (e.Column.ColumnName.Equals("Fecha") Or e.Column.ColumnName.Equals("FechaComprobante")) Then
            If IsDBNull(e.Row("Fecha")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = "1/1/1800"
        End If

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
                Diferencia = TotalConceptos - TotalRecibos - GridCompro.Rows(e.RowIndex).Cells("Asignado").Value
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
            Exit Sub
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
            '
            SaldoAnt = e.Row("Saldo")
            e.Row("Saldo") = Trunca(e.Row("Saldo") + e.Row("Asignado") - CDec(e.ProposedValue))
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