Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnaCompraDivisas
    '6000 Compra Divisa. 
    '6001 Venta Divisa. 
    Public PMovimiento As Decimal
    Public PAbierto As Boolean
    Public PTipoMovimiento As Integer
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim DtCompraCabeza As DataTable
    Dim DtCompraPago As DataTable
    Dim DtGrid As DataTable
    Dim DtGrid1 As DataTable
    Dim DtFormasCompraVentaDivisas As DataTable
    Dim DtFormasPagoCobranza As DataTable
    '
    Dim ConexionCompra As String
    Dim TipoAsiento As Integer
    '     Para Impresion.  
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim LineasParaImpresionCompraVentaDivisas As Integer = 0
    Dim LineasParaImpresionPagoCobranza As Integer = 0
    Private Sub UnaCompraDivisas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(8) Then PBloqueaFunciones = True

        If GCaja = 0 And PMovimiento = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PMovimiento <> 0 Then
            PTipoMovimiento = HallaTipoMovimiento(PMovimiento, PAbierto)
            If EsReemplazoChequeCompraDivisas(PMovimiento, PAbierto) Then
                UnChequeReemplazo.PTipoNota = PTipoMovimiento
                UnChequeReemplazo.PNota = PMovimiento
                UnChequeReemplazo.PAbierto = PAbierto
                UnChequeReemplazo.PBloqueaFunciones = PBloqueaFunciones
                UnChequeReemplazo.ShowDialog()
                Me.Close()
                Exit Sub
            End If
        End If

        If PMovimiento = 0 Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

        If PMovimiento <> 0 Then
            If PAbierto Then PictureCandado.Image = ImageList1.Images.Item("Abierto")
            If Not PAbierto Then PictureCandado.Image = ImageList1.Images.Item("Cerrado")
        End If

        If Not PermisoTotal Then
            PictureCandado.Visible = False
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboMoneda.DataSource = Nothing
        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"
        With ComboMoneda
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PAbierto Then
            ConexionCompra = Conexion
        Else : ConexionCompra = ConexionN
        End If

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        If PMovimiento = 0 Then
            DateTime1.Enabled = True
        Else
            DateTime1.Enabled = False
        End If

        GModificacionOk = False

    End Sub
    Private Sub UnaCompraDivisas_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PMovimiento <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtCompraCabezaAux As DataTable = DtCompraCabeza.Copy
        Dim DtCompraPagoAux As DataTable = DtCompraPago.Copy

        'Actualiza Cabeza.
        If RadioBancario.Checked Then DtCompraCabezaAux.Rows(0).Item("Origen") = 1
        If RadioProveedor.Checked Then DtCompraCabezaAux.Rows(0).Item("Origen") = 2
        If RadioCliente.Checked Then DtCompraCabezaAux.Rows(0).Item("Origen") = 3
        If Format(DtCompraCabezaAux.Rows(0).Item("FechaContable"), "dd/MM/yyyy") <> CDate(TextFechaContable.Text) Then DtCompraCabezaAux.Rows(0).Item("FechaContable") = CDate(TextFechaContable.Text)

        'Actualizo MedioPago 1.
        'Enumero los registros.
        Dim Item As Integer = 0
        For Each Row As DataRow In DtGrid.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Item = Item + 1
                Row("Item") = Item
            End If
        Next
        For Each Row As DataRow In DtGrid1.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Item = Item + 1
                Row("Item") = Item
            End If
        Next
        'Genero archivos de pagos.
        For Each Row As DataRow In DtGrid.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Dim Row1 As DataRow = DtCompraPagoAux.NewRow
                Row1("Item") = Row("Item")
                Row1("Tipo") = 0
                Row1("MedioPago") = Row("MedioPago")
                Row1("Cambio") = Row("Cambio")
                Row1("Importe") = Row("Importe")
                Row1("Banco") = Row("Banco")
                Row1("Cuenta") = Row("Cuenta")
                Row1("Comprobante") = Row("Comprobante")
                Row1("FechaComprobante") = Row("FechaComprobante")
                Row1("ClaveCheque") = Row("ClaveCheque")
                DtCompraPagoAux.Rows.Add(Row1)
            End If
        Next
        For Each Row As DataRow In DtGrid1.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Dim Row1 As DataRow = DtCompraPagoAux.NewRow
                Row1("Item") = Row("Item")
                Row1("Tipo") = 1
                Row1("MedioPago") = Row("MedioPago")
                Row1("Cambio") = Row("Cambio")
                Row1("Importe") = Row("Importe")
                Row1("Banco") = Row("Banco")
                Row1("Cuenta") = Row("Cuenta")
                Row1("Comprobante") = Row("Comprobante")
                Row1("FechaComprobante") = Row("FechaComprobante")
                Row1("ClaveCheque") = Row("ClaveCheque")
                DtCompraPagoAux.Rows.Add(Row1)
            End If
        Next

        'Arma Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        '
        If GGeneraAsiento And Not IsNothing(DtCompraPagoAux.GetChanges) Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtCompraPagoAux, DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If HacerAlta(DtCompraCabezaAux, DtCompraPagoAux, DtAsientoCabeza, DtAsientoDetalle) Then
            If Not ArmaArchivos() Then Me.Close() : Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PMovimiento = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3, "ERROR!") : Exit Sub
        End If

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If GCaja <> DtCompraCabeza.Rows(0).Item("Caja") Then
            MsgBox("Caja del Comprobante no corresponde a la del Usuario Actual. Operación se CANCELA.")
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Comprobante esta ANULADO.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        MiEnlazador.EndEdit()

        Dim DtCompraCabezaAux As DataTable = DtCompraCabeza.Copy
        Dim DtCompraPagoAux As DataTable = DtCompraPago.Copy

        For Each Row As DataRow In DtGrid1.Rows
            If Row("MedioPago") = 2 Or Row("MedioPago") = 3 Then
                Dim Anulado As Boolean, Rechazado As Boolean, Depositado As Boolean, Entregado As Boolean, Afectado As Boolean
                If Not EstadoCheque(Row("MedioPago"), Row("ClaveCheque"), ConexionCompra, Anulado, Rechazado, Depositado, Entregado, Afectado) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Error Base de Datos al Consistir Cheques.")
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
                If Row("MedioPago") = 3 Then
                    If ExiteChequeEnPaseCaja(ConexionCompra, Row("ClaveCheque")) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " en Proceso de Pase de Caja. Operación se CANCELA.")
                        Exit Sub
                    End If
                End If
                If PTipoMovimiento = 6000 Then
                    If Depositado Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Cheque Depositado. Operación se CANCELA.")
                        Exit Sub
                    End If
                    If Afectado Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Esta Afectado a una Orden de Pago. Operación se CANCELA.")
                        Exit Sub
                    End If
                    If ChequeReemplazado(Row("MedioPago"), Row("ClaveCheque"), PTipoMovimiento, PMovimiento, ConexionCompra) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar, fue Reemplazado. Operación se CANCELA.")
                        Exit Sub
                    End If
                End If
                If PTipoMovimiento = 6001 Then
                    If Entregado Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar, fue usado para Pago o Depositado. Operación se CANCELA.")
                        Exit Sub
                    End If
                    If Anulado Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Cheque Anulado. Operación se CANCELA.")
                        Exit Sub
                    End If
                End If
            End If
        Next

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        '
        If Not HallaAsientosCabeza(TipoAsiento, PMovimiento, DtAsientoCabeza, ConexionCompra) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If
        For Each Row As DataRow In DtAsientoCabeza.Rows
            Row("Estado") = 3
        Next

        If MsgBox("El Comprobante se eliminara definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtCompraCabezaAux.Rows(0).Item("Estado") = 3

        Dim Resul As Integer = ActualizaMovi("B", DtCompraCabezaAux, DtCompraPagoAux, DtAsientoCabeza, DtAsientoDetalle)

        If Resul < 0 And Resul <> -3 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Comprobante Anulado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            If Not ArmaArchivos() Then Me.Close() : Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueContable.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PMovimiento = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = TipoAsiento
        If PAbierto Then
            ListaAsientos.PDocumentoB = PMovimiento
        Else
            ListaAsientos.PDocumentoN = PMovimiento
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PMovimiento = 0 Then
            MsgBox("Opcion Invalida. Nota debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ErrorImpresion = False
        Paginas = 0
        Copias = 1

        Dim print_document As New PrintDocument

        LineasParaImpresionCompraVentaDivisas = 0
        LineasParaImpresionPagoCobranza = 0

        AddHandler print_document.PrintPage, AddressOf Print_PrintComprobante
        print_document.Print()

    End Sub
    Private Sub ButtonMediosDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMediosDePago.Click

        If ComboMoneda.SelectedValue = 0 Then
            MsgBox("Debe Informar Moneda.", MsgBoxStyle.Information)
            ComboMoneda.Focus()
            Exit Sub
        End If
        If CDbl(TextCambio.Text) = 0 Then
            MsgBox("Debe Informar Cambio.", MsgBoxStyle.Information)
            TextCambio.Focus()
            Exit Sub
        End If

        Dim DtMediosPagos As DataTable = DtFormasCompraVentaDivisas.Copy
        For I As Integer = DtMediosPagos.Rows.Count - 1 To 0 Step -1
            Dim Row As DataRow = DtMediosPagos.Rows(I)
            If (Row("Tipo") = 1 Or Row("Tipo") = 3) And Row("Clave") <> ComboMoneda.SelectedValue Then Row.Delete()
        Next

        UnMediosPago.PTipoNota = PTipoMovimiento
        UnMediosPago.PDtGrid = DtGrid
        UnMediosPago.PAbierto = PAbierto
        If PMovimiento = 0 Then
            UnMediosPago.PBloqueaFunciones = False
        Else : UnMediosPago.PBloqueaFunciones = True
        End If
        UnMediosPago.PDtFormasPago = DtMediosPagos
        UnMediosPago.PDtRetencionProvincia = New DataTable
        UnMediosPago.PEsExterior = False
        UnMediosPago.PDtRetencionesAutomaticas = New DataTable
        UnMediosPago.PDiferenciaDeCambio = False
        UnMediosPago.PMoneda = ComboMoneda.SelectedValue
        UnMediosPago.PCambio = CDbl(TextCambio.Text)
        UnMediosPago.PImporte = CDbl(TextTotalMediosPago.Text)
        UnMediosPago.PImporteAInformar = 0
        UnMediosPago.PEsTr = False
        UnMediosPago.PEsEnMonedaExtranjera = True
        UnMediosPago.ShowDialog()
        DtGrid = UnMediosPago.PDtGrid
        TextTotalMediosPago.Text = FormatNumber(UnMediosPago.PImporte, GDecimales)
        TextTotalPesos.Text = FormatNumber(Trunca(UnMediosPago.PImporte * CDbl(TextCambio.Text)), GDecimales)

        UnMediosPago.Dispose()
        DtMediosPagos.Dispose()

        If CDbl(TextTotalMediosPago.Text) <> 0 Then
            ComboMoneda.Enabled = False
            TextCambio.ReadOnly = True
        Else
            ComboMoneda.Enabled = True
            TextCambio.ReadOnly = False
        End If

    End Sub
    Private Sub ButtonMediosDePago1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMediosDePago1.Click

        If Not RadioBancario.Checked And Not RadioCliente.Checked And Not RadioProveedor.Checked Then
            MsgBox("Debe Informar Comprador/Vendedor")
            Exit Sub
        End If

        Dim DtMediosPagos As DataTable = DtFormasPagoCobranza.Copy
        For I As Integer = DtMediosPagos.Rows.Count - 1 To 0 Step -1
            Dim Row As DataRow = DtMediosPagos.Rows(I)
            If PTipoMovimiento = 6001 Then
                If Row("Clave") = 2 Then
                    Row.Delete()
                Else
                    If Row("Clave") = 3 And RadioBancario.Checked Then Row.Delete()
                End If
            End If
        Next

        UnMediosPago.PTipoNota = PTipoMovimiento
        UnMediosPago.PAbierto = PAbierto
        UnMediosPago.PDtGrid = DtGrid1
        If PMovimiento = 0 Then
            UnMediosPago.PBloqueaFunciones = False
        Else : UnMediosPago.PBloqueaFunciones = True
        End If
        UnMediosPago.PDtFormasPago = DtMediosPagos
        UnMediosPago.PDtRetencionProvincia = New DataTable
        UnMediosPago.PEsExterior = False
        UnMediosPago.PDtRetencionesAutomaticas = New DataTable
        UnMediosPago.PDiferenciaDeCambio = False
        UnMediosPago.PImporte = CDbl(TextTotalMediosPago1.Text)
        UnMediosPago.PImporteAInformar = 0
        UnMediosPago.PEsTr = False
        UnMediosPago.ShowDialog()
        DtGrid1 = UnMediosPago.PDtGrid
        TextTotalMediosPago1.Text = FormatNumber(UnMediosPago.PImporte, GDecimales)

        UnMediosPago.Dispose()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        PMovimiento = 0
        ArmaArchivos()
        RadioBancario.Checked = False
        RadioProveedor.Checked = False
        RadioCliente.Checked = False

    End Sub
    Private Sub RadioBancario_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioBancario.CheckedChanged

        If RadioBancario.Checked Then
            If PTipoMovimiento = 6001 Then
                For Each Row As DataRow In DtGrid1.Rows
                    If Row("MedioPago") = 3 Then
                        MsgBox("Existe Cheque de Terceros en Pago por la venta, No Valido para Bancos.")
                        RadioBancario.Checked = False
                        ComboEmisor.DataSource = Nothing
                        Exit Sub
                    End If
                Next
            End If
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            ComboEmisor.DataSource = Nothing
            LlenaComboTablas(ComboEmisor, 26)
            ComboEmisor.SelectedValue = 0
            With ComboEmisor
                .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                .AutoCompleteSource = AutoCompleteSource.ListItems
            End With
            LabelEmisor.Text = "Banco"
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End If

    End Sub
    Private Sub RadioProveedor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioProveedor.CheckedChanged

        If RadioProveedor.Checked Then
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            ComboEmisor.DataSource = Nothing
            ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Opr FROM Proveedores WHERE TipoOperacion <> 4")
            Dim Row As DataRow = ComboEmisor.DataSource.NewRow()
            Row("Clave") = 0
            Row("Nombre") = ""
            ComboEmisor.DataSource.Rows.Add(Row)
            ComboEmisor.DisplayMember = "Nombre"
            ComboEmisor.ValueMember = "Clave"
            LabelEmisor.Text = "Proveeedor"
            ComboEmisor.SelectedValue = 0
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End If

    End Sub
    Private Sub RadioCliente_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioCliente.CheckedChanged

        If RadioCliente.Checked Then
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            ComboEmisor.DataSource = Nothing
            ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Opr FROM Clientes ORDER BY Nombre;")
            Dim Row As DataRow = ComboEmisor.DataSource.NewRow()
            Row("Clave") = 0
            Row("Nombre") = ""
            ComboEmisor.DataSource.Rows.Add(Row)
            ComboEmisor.DisplayMember = "Nombre"
            ComboEmisor.ValueMember = "Clave"
            LabelEmisor.Text = "Cliente"
            ComboEmisor.SelectedValue = 0
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End If

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

    End Sub
    Private Sub ComboMoneda_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboMoneda.Validating

        If IsNothing(ComboMoneda.SelectedValue) Then ComboMoneda.SelectedValue = 0

    End Sub
    Private Sub TextCambio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCambio.KeyPress

        EsNumerico(e.KeyChar, TextCambio.Text, 3)

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            PAbierto = False
        Else
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarTodo.Click

        If MsgBox("Medios Pagos se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtGrid.Clear()
        TextTotalMediosPago.Text = "0,00"
        ComboMoneda.Enabled = True
        TextCambio.ReadOnly = False

    End Sub
    Private Sub ButtonEliminarTodo1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarTodo1.Click

        If MsgBox("Medios Pagos se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtGrid1.Clear()
        TextTotalMediosPago1.Text = "0,00"

    End Sub
    Private Function ArmaArchivos() As Boolean                'ArmaArchivos

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtGrid = CreaDtParaGrid()
        DtGrid1 = DtGrid.Clone

        DtCompraCabeza = New DataTable
        Dim Sql As String = "SELECT * FROM CompraDivisasCabeza WHERE Movimiento = " & PMovimiento & ";"
        If Not Tablas.Read(Sql, ConexionCompra, DtCompraCabeza) Then Return False
        If PMovimiento <> 0 And DtCompraCabeza.Rows.Count = 0 Then
            MsgBox("Compra/Venta Divisa No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PMovimiento = 0 Then
            Dim Row As DataRow = DtCompraCabeza.NewRow
            ArmaNuevoCompraDivisas(Row)
            Row("TipoMovimiento") = PTipoMovimiento
            Row("Fecha") = DateTime1.Value
            Row("Caja") = GCaja
            Row("Estado") = 1
            DtCompraCabeza.Rows.Add(Row)
        End If

        PTipoMovimiento = DtCompraCabeza.Rows(0).Item("TipoMovimiento")

        If PTipoMovimiento = 6000 Then
            ArmaMedioPagoCompraDivisas(DtFormasCompraVentaDivisas)
            TipoAsiento = 16000
        Else
            ArmaMedioPagoVentaDivisas(DtFormasCompraVentaDivisas)
            TipoAsiento = 16001
        End If
        ArmaMedioPagoCobroDivisas(DtFormasPagoCobranza)

        MuestraCabeza()

        DtCompraPago = New DataTable
        Sql = "SELECT * FROM CompraDivisasPago WHERE Movimiento = " & PMovimiento & ";"
        If Not Tablas.Read(Sql, ConexionCompra, DtCompraPago) Then Return False

        Dim DtCheques As New DataTable
        For Each Row As DataRow In DtCompraPago.Rows
            If Row("ClaveCheque") <> 0 Then
                If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionCompra, DtCheques) Then Return False
            End If
        Next

        Dim Row1 As DataRow
        Dim RowsBusqueda() As DataRow
        Dim Importe As Double = 0

        For Each Row As DataRow In DtCompraPago.Rows
            If Row("Tipo") = 0 Then
                Row1 = DtGrid.NewRow
                Row1("Item") = Row("Item")
                Row1("MedioPago") = Row("MedioPago")
                Row1("Importe") = Row("Importe")
                Importe = Trunca(Importe + Row("Importe"))
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
                Row1("ClaveInterna") = 0
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
                    Row1("eCheq") = 0
                End If
                Row1("TieneLupa") = False
                DtGrid.Rows.Add(Row1)
            End If
        Next
        TextTotalMediosPago.Text = FormatNumber(Importe, GDecimales)
        TextTotalPesos.Text = FormatNumber(Trunca(Importe * CDbl(TextCambio.Text)), GDecimales)

        Importe = 0
        For Each Row As DataRow In DtCompraPago.Rows
            If Row("Tipo") = 1 Then
                Row1 = DtGrid1.NewRow
                Row1("Item") = Row("Item")
                Row1("MedioPago") = Row("MedioPago")
                Row1("Importe") = Row("Importe")
                Row1("Cambio") = Row("Cambio")
                If Row("Cambio") <> 0 Then
                    Importe = Trunca(Importe + Trunca(Row("Cambio") * Row("Importe")))
                Else : Importe = Trunca(Importe + Row("Importe"))
                End If
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
                Row1("ClaveInterna") = 0
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
                Row1("TieneLupa") = False
                DtGrid1.Rows.Add(Row1)
            End If
        Next
        TextTotalMediosPago1.Text = FormatNumber(Importe, GDecimales)

        If DtCompraCabeza.Rows(0).Item("TipoMovimiento") = 6000 Then
            Label7.Text = "C O M P R A  de  D I V I S A "
            ButtonMediosDePago.Text = "Medios De Pago Recibidos por la Compra"
            ButtonMediosDePago1.Text = "Medios De Pago Entregados"
        Else : Label7.Text = "V E N T A  de  D I V I S A "
            ButtonMediosDePago.Text = "Medios De Pago Entregados por la Venta"
            ButtonMediosDePago1.Text = "Medios De Pago Recibidos"
        End If

        If PMovimiento <> 0 Then
            PanelEmisor.Enabled = False
            Panel3.Enabled = False
            PictureCandado.Enabled = False
            ButtonEliminarTodo.Enabled = False
            ButtonEliminarTodo1.Enabled = False
            ComboMoneda.Enabled = False
            TextCambio.ReadOnly = True
            TextFechaContable.ReadOnly = True
            PictureAlmanaqueContable.Enabled = False
        Else
            PanelEmisor.Enabled = True
            Panel3.Enabled = True
            PictureCandado.Enabled = True
            ButtonEliminarTodo.Enabled = True
            ButtonEliminarTodo1.Enabled = True
            ComboMoneda.Enabled = True
            TextCambio.ReadOnly = False
            TextFechaContable.ReadOnly = False
            PictureAlmanaqueContable.Enabled = True
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCompraCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Movimiento")
        AddHandler Enlace.Format, AddressOf FormatMovimiento
        TextMovimiento.DataBindings.Clear()
        TextMovimiento.DataBindings.Add(Enlace)

        Select Case DtCompraCabeza.Rows(0).Item("Origen") 'Poner antes de mostrar emisor.
            Case 1
                RadioBancario.Checked = True
            Case 2
                RadioProveedor.Checked = True
            Case 3
                RadioCliente.Checked = True
        End Select

        Enlace = New Binding("SelectedValue", MiEnlazador, "Emisor")
        ComboEmisor.DataBindings.Clear()
        ComboEmisor.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Moneda")
        ComboMoneda.DataBindings.Clear()
        ComboMoneda.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cambio")
        AddHandler Enlace.Format, AddressOf FormatCambio
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextCambio.DataBindings.Clear()
        TextCambio.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalMediosPago.DataBindings.Clear()
        TextTotalMediosPago.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        If DtCompraCabeza.Rows(0).Item("FechaContable") = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(DtCompraCabeza.Rows(0).Item("FechaContable"), "dd/MM/yyyy")
        End If

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatMovimiento(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = NumeroEditado(Numero.Value)

    End Sub
    Private Sub FormatCambio(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, 3)

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Function HacerAlta(ByVal DtCompraCabezaAux As DataTable, ByVal DtCompraPagoAux As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable) As Boolean

        Dim NumeroMovi As Double
        Dim Resul As Double
        Dim NumeroAsiento As Double

        If PAbierto Then
            ConexionCompra = Conexion
        Else : ConexionCompra = ConexionN
        End If

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            NumeroMovi = UltimaNumeracion(ConexionCompra)
            If NumeroMovi < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            DtCompraCabezaAux.Rows(0).Item("Movimiento") = NumeroMovi
            For Each Row As DataRow In DtCompraPagoAux.Rows
                Row("Movimiento") = NumeroMovi
            Next

            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionCompra)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = NumeroMovi
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            Resul = ActualizaMovi("A", DtCompraCabezaAux, DtCompraPagoAux, DtAsientoCabeza, DtAsientoDetalle)

            If Resul >= 0 Then Exit For
            If Resul = -3 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = -3 Then
            Return False
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PMovimiento = NumeroMovi
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function ActualizaMovi(ByVal Funcion As String, ByVal DtCabezaAux As DataTable, ByVal DtDetalleAux As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable) As Double

        Dim Resul As Double
        Dim RowsBusqueda() As DataRow

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaAux.GetChanges, "CompraDivisasCabeza", ConexionCompra)
                    If Resul <= 0 Then Return Resul
                End If
                '
                For Each Row As DataRow In DtDetalleAux.Rows
                    If Row("Tipo") = 1 Then
                        If (Row("MedioPago") = 2 Or Row("MedioPago") = 3) Then
                            RowsBusqueda = DtGrid1.Select("Item = " & Row("Item"))
                        End If
                        If PTipoMovimiento = 6000 Then
                            If Row.RowState = DataRowState.Added Then
                                If Row("MedioPago") = 2 Then
                                    Row("ClaveCheque") = ActualizaClavesComprobantes("A", 0, 6000, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), RowsBusqueda(0).Item("Serie"), RowsBusqueda(0).Item("Numero"), RowsBusqueda(0).Item("Fecha"), Row("Importe"), "", DtCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionCompra, False, False, RowsBusqueda(0).Item("eCheq"))
                                    If Row("ClaveCheque") <= 0 Then
                                        MsgBox("Cheque " & RowsBusqueda(0).Item("Serie") & " " & RowsBusqueda(0).Item("Numero") & " ya fue Emitido.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    End If
                                End If
                                If Row("MedioPago") = 3 Then
                                    If ActualizaClavesComprobantes("A", Row("ClaveCheque"), 6000, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), RowsBusqueda(0).Item("Serie"), RowsBusqueda(0).Item("Numero"), RowsBusqueda(0).Item("Fecha"), Row("Importe"), RowsBusqueda(0).Item("EmisorCheque"), DtCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionCompra, False, False, RowsBusqueda(0).Item("eCheq")) <= 0 Then
                                        MsgBox("Cheque " & RowsBusqueda(0).Item("Serie") & " " & RowsBusqueda(0).Item("Numero") & " Error al Grabar.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    End If
                                End If
                            End If
                        End If
                        If PTipoMovimiento = 6001 Then
                            If Row.RowState = DataRowState.Added Then
                                If Row("MedioPago") = 3 Then
                                    Row("ClaveCheque") = ActualizaClavesComprobantes("A", Row("ClaveCheque"), 6001, Row("MedioPago"), 0, Row("Banco"), Row("Cuenta"), RowsBusqueda(0).Item("Serie"), RowsBusqueda(0).Item("Numero"), RowsBusqueda(0).Item("Fecha"), Row("Importe"), RowsBusqueda(0).Item("EmisorCheque"), DtCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionCompra, False, False, RowsBusqueda(0).Item("eCheq"))
                                    If Row("ClaveCheque") <= 0 Then
                                        MsgBox("Cheque " & RowsBusqueda(0).Item("Serie") & " " & RowsBusqueda(0).Item("Numero") & " ya fue Emitido.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
                '
                If Funcion = "B" Then
                    For Each Row As DataRow In DtDetalleAux.Rows
                        If Row("Tipo") = 1 Then
                            If (Row("MedioPago") = 2 Or Row("MedioPago") = 3) Then
                                RowsBusqueda = DtGrid1.Select("Item = " & Row("Item"))
                            End If
                            If PTipoMovimiento = 6000 Then
                                If Row("MedioPago") = 3 Then
                                    If ActualizaClavesComprobantes("B", Row("ClaveCheque"), 6000, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionCompra, False, False, False) <= 0 Then
                                        MsgBox("Cheque " & Row("ClaveCheque") & " " & Row("Cuenta") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    End If
                                End If
                                If Row("MedioPago") = 2 Then
                                    If ActualizaClavesComprobantes("B", Row("ClaveCheque"), 6000, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionCompra, False, False, False) <= 0 Then
                                        MsgBox("Cheque " & Row("Banco") & " " & Row("Cuenta") & " Error de datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    End If
                                End If
                            End If
                            If PTipoMovimiento = 6001 Then
                                If Row("MedioPago") = 3 Then
                                    If ActualizaClavesComprobantes("B", Row("ClaveCheque"), 6001, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionCompra, False, False, False) <= 0 Then
                                        MsgBox("Cheque " & Row("ClaveCheque") & " " & Row("Cuenta") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
                '
                If Not IsNothing(DtDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleAux.GetChanges, "CompraDivisasPago", ConexionCompra)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionCompra)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionCompra)
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
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtDetallePago As DataTable, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos

        If PTipoMovimiento = 6000 Then
            For Each Row As DataRow In DtDetallePago.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    If Row("Tipo") = 0 Then
                        Item = New ItemListaConceptosAsientos
                        Item.Clave = Row("MedioPago") + 2000000
                        Item.Importe = Row("Importe")
                        Item.Importe = Trunca(Item.Importe * CDbl(TextCambio.Text))
                        ListaConceptos.Add(Item)
                    End If
                End If
            Next
            For Each Row As DataRow In DtDetallePago.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    If Row("Tipo") = 1 Then
                        Item = New ItemListaConceptosAsientos
                        Item.Clave = Row("MedioPago") + 4000000
                        Item.Importe = Row("Importe")
                        If Row("Cambio") <> 0 Then Item.Importe = Trunca(Item.Importe * Row("Cambio"))
                        ListaConceptos.Add(Item)
                    End If
                End If
            Next
        End If
        '
        If PTipoMovimiento = 6001 Then
            For Each Row As DataRow In DtDetallePago.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    If Row("Tipo") = 0 Then
                        Item = New ItemListaConceptosAsientos
                        Item.Clave = Row("MedioPago") + 3000000
                        Item.Importe = Row("Importe")
                        Item.Importe = Trunca(Item.Importe * CDbl(TextCambio.Text))
                        ListaConceptos.Add(Item)
                    End If
                End If
            Next
            For Each Row As DataRow In DtDetallePago.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    If Row("Tipo") = 1 Then
                        Item = New ItemListaConceptosAsientos
                        Item.Clave = Row("MedioPago") + 5000000
                        Item.Importe = Row("Importe")
                        If Row("Cambio") <> 0 Then Item.Importe = Trunca(Item.Importe * Row("Cambio"))
                        ListaConceptos.Add(Item)
                    End If
                End If
            Next
        End If
        '
        If Not Asiento(TipoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, CDate(TextFechaContable.Text), 0) Then Return False

        Return True

    End Function
    Private Sub Print_PrintComprobante(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 37
        Dim UltimaLinea As Integer = 0
        Dim LineasImpresas As Integer = 0
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        Try
            Print_Titulo(e, MIzq, MTop)

            If LineasParaImpresionCompraVentaDivisas < DtGrid.Rows.Count Then
                Print_MediosPago(e, MIzq, MTop, UltimaLinea, LineasImpresas)
                'Imprime imputaciones.
                If LineasParaImpresionCompraVentaDivisas >= DtGrid.Rows.Count And LineasImpresas < 27 Then
                    Print_MediosPago1(e, MIzq, UltimaLinea + 10, 37 - LineasImpresas - 4)
                End If
            Else
                Print_MediosPago1(e, MIzq, UltimaLinea + 10, 37 - LineasImpresas - 4)
            End If

            If PTipoMovimiento = 6000 Then Print_Final(e, MIzq, MTop)

            If (LineasParaImpresionCompraVentaDivisas < DtGrid.Rows.Count) Or LineasParaImpresionPagoCobranza < DtGrid1.Rows.Count Then
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
    Private Sub Print_Titulo(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

        Dim Texto As String = ""

        'Encabezado.
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
        Texto = GNombreEmpresa
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
        If PTipoMovimiento = 6000 Then
            Texto = "ORDEN DE PAGO"
        Else : Texto = "COBRANZA"
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 140, MTop)
        ' 
        PrintFont = New Font("Courier New", 12)
        Texto = "Nro. Comprobante: " & TextMovimiento.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
        Texto = "Fecha:  " & DateTime1.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
        Texto = ComboEmisor.Text
        If RadioBancario.Checked Then Texto = "Banco " & Texto
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 27)
        e.Graphics.DrawString("", PrintFont, Brushes.Black, MIzq, MTop + 33)
        Texto = "Importe Venta : " & TextTotalMediosPago.Text & " " & ComboMoneda.Text & "  Importe Pesos " & TextTotalPesos.Text
        PrintFont = New Font("Courier New", 12, FontStyle.Bold)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 15, MTop + 40)

    End Sub
    Private Sub Print_Final(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

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
        If PTipoMovimiento = 6000 Then
            Texto = "COMPRA DIVISAS"
        Else : Texto = "VENTA DIVISAS"
        End If

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
        While Contador < LineasPorPagina And LineasParaImpresionCompraVentaDivisas < DtGrid.Rows.Count
            Dim Row As DataRow = DtGrid.Rows(LineasParaImpresionCompraVentaDivisas)
            RowsBusqueda = DtFormasCompraVentaDivisas.Select("Clave = " & Row("MedioPago"))
            Yq = Yq + SaltoLinea
            'Imprime Detalle.
            Texto = RowsBusqueda(0).Item("Nombre")
            Xq = x
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            'Imprime Importe.
            Texto = FormatNumber(Row("Importe"), GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
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
            End If
            'Imprime Vencimeinto.
            If Format(Row("Fecha"), "dd/MM/yyyy") <> "01/01/1800" Then
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
            End If
            'Imprime FechaComprobante.
            If Format(Row("FechaComprobante"), "dd/MM/yyyy") <> "01/01/1800" Then
                Texto = Format(Row("FechaComprobante"), "dd/MM/yyyy")
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaVencimiento - Longi - 2
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            End If
            Contador = Contador + 1
            LineasParaImpresionCompraVentaDivisas = LineasParaImpresionCompraVentaDivisas + 1
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
    Private Sub Print_MediosPago1(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer, ByRef LineasImpresas As Integer)

        Dim Texto As String = ""
        Dim SaltoLinea As Integer = 4
        Dim Longi As Integer
        Dim Xq As Integer
        Dim Yq As Integer
        Dim x As Integer = MIzq
        Dim y As Integer = MTop
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
        If PTipoMovimiento = 6000 Then
            Texto = "PAGO POR COMPRA DE DIVISAS"
        Else : Texto = "COBRANZA POR VENTA DE DIVISAS"
        End If

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
        While Contador < LineasPorPagina And LineasParaImpresionPagoCobranza < DtGrid1.Rows.Count
            Dim Row As DataRow = DtGrid1.Rows(LineasParaImpresionPagoCobranza)
            RowsBusqueda = DtFormasPagoCobranza.Select("Clave = " & Row("MedioPago"))
            Yq = Yq + SaltoLinea
            'Imprime Detalle.
            Texto = RowsBusqueda(0).Item("Nombre")
            Xq = x
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            If Row("Cambio") <> 0 Then
                'Imprime Cambio.
                Texto = FormatNumber(Row("Cambio"), 3)
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
            End If
            'Imprime Vencimeinto.
            If Format(Row("Fecha"), "dd/MM/yyyy") <> "01/01/1800" Then
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
            End If
            'Imprime FechaComprobante.
            If Format(Row("FechaComprobante"), "dd/MM/yyyy") <> "01/01/1800" Then
                Texto = Format(Row("FechaComprobante"), "dd/MM/yyyy")
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaVencimiento - Longi - 2
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            End If
            Contador = Contador + 1
            LineasParaImpresionPagoCobranza = LineasParaImpresionPagoCobranza + 1
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

    End Sub
    Public Function HallaAliasProveedor() As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Alias FROM Proveedores WHERE Clave = " & ComboEmisor.SelectedValue & ";", Miconexion)
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
                Using Cmd As New OleDb.OleDbCommand("SELECT Alias FROM Clientes WHERE Clave = " & ComboEmisor.SelectedValue & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Proveedores.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function HallaTipoMovimiento(ByVal Movimiento As Double, ByVal Abierto As Boolean) As Integer

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT TipoMovimiento FROM CompraDivisasCabeza WHERE Movimiento = " & Movimiento & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer CompraDivisasCabeza. " & ex.Message, MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function UltimaNumeracion(ByVal ConexionStr As String) As Double

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
    Private Function Valida() As Boolean

        If ComboEmisor.SelectedValue = 0 Then
            MsgBox("Falta Informar Vendedor o Comprador. Operación se CANCELA.", MsgBoxStyle.Critical)
            Return False
        End If

        If Not RadioBancario.Checked Then
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = ComboEmisor.DataSource.select("Clave = " & ComboEmisor.SelectedValue)
            If Not RowsBusqueda(0).Item("Opr") And PAbierto Then
                If MsgBox("Cliente/Proveedor Solo Opera a Candado Cerrado. Debe Cambiar Candado en Cliente/Proveedor. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
            End If
        End If

        If CDbl(TextTotalMediosPago.Text) = 0 Then
            MsgBox("Falta Informar " & ButtonMediosDePago.Text & " .Operación se CANCELA.", MsgBoxStyle.Critical)
            Return False
        End If

        If CDbl(TextTotalMediosPago1.Text) = 0 Then
            MsgBox("Falta Informar " & ButtonMediosDePago1.Text & " .Operación se CANCELA.", MsgBoxStyle.Critical)
            Return False
        End If

        If CDbl(TextTotalPesos.Text) <> CDbl(TextTotalMediosPago1.Text) Then
            MsgBox("Importes en pesos no son Iguales. Operación se CANCELA.", MsgBoxStyle.Critical)
            Return False
        End If

        If TextFechaContable.Text = "" Then
            MsgBox("Falta Informar Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If Not ConsisteFecha(TextFechaContable.Text) Then
            MsgBox("Fecha Contable Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If DiferenciaDias(TextFechaContable.Text, Date.Now) < 0 Then
            MsgBox("Fecha Contable Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If

        Return True

    End Function





   
End Class