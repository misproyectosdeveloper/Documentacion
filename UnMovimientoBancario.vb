Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnMovimientoBancario
    Public PTipoNota As Integer
    Public PMovimiento As Double
    Public PAbierto As Boolean
    Public PEsDepositoEfectivo As Boolean
    Public PEsExtraccionEfectivo As Boolean
    Public PEsExtraccionChequePropio As Boolean
    Public PEsTransferenciaPropia As Boolean
    Public PEsDepositoChequesTerceros As Boolean
    Public PEsDiferidos As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim DtDetalle As DataTable
    Dim DtCabeza As DataTable
    Dim DtCuentas As DataTable
    Dim DtFormasPago As DataTable
    Dim DtBancosNegro As DataTable
    '
    Dim cb As ComboBox
    Dim ConexionMovimiento As String
    Dim TipoAsiento As Integer
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Private Sub MovimientosBancario_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If GCaja = 0 And PMovimiento = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If Not PermisoEscritura(8) Then PBloqueaFunciones = True

        GModificacionOk = False

        Grid.Columns("LupaCuenta").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")
        Grid.AutoGenerateColumns = False

        ArmaDtCuentas()

        ArmaMedioPagoTodos(DtFormasPago, True)

        DtBancosNegro = New DataTable
        If Not Tablas.Read("SELECT Clave FROM Tablas WHERE Tipo = 26 AND Activo2 = 1;", Conexion, DtBancosNegro) Then
            Me.Close()
            Exit Sub
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0

        LlenaComboTablas(ComboBancoRetiro, 26)
        ComboBancoRetiro.SelectedValue = 0
        With ComboBancoRetiro
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboBancoDeposito, 26)
        ComboBancoDeposito.SelectedValue = 0
        With ComboBancoDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionMovimiento = Conexion
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionMovimiento = ConexionN
        End If

        If PMovimiento = 0 Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionMovimiento = Conexion
            PAbierto = True
            If Not PermisoTotal Then PanelOperacionCheque.Visible = False
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        If (PTipoNota = 90 And PEsExtraccionChequePropio) Or (PTipoNota = 91 And PEsDepositoChequesTerceros) Then
            TextTotalComprobante.ReadOnly = True
        Else
            TextTotalComprobante.ReadOnly = False
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)
        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid_RowChanged)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PMovimiento <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PMovimiento = 0 Then
            If EsBancoNegro(ComboBancoRetiro.SelectedValue) Then
                ConexionMovimiento = ConexionN
            End If
            If HacerAlta() Then If Not ArmaArchivos() Then Me.Close() : Exit Sub
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

        If PMovimiento = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Movimiento ya esta ANULADO. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If CInt(LabelCaja.Text) <> GCaja And Not GAdministrador Then
            MsgBox("Modificación Solo Permitida para Caja " & LabelCaja.Text & " o un Administrador. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        Dim Resul As Double

        If PEsDepositoChequesTerceros Then
            For Each Row As DataGridViewRow In Grid.Rows
                If IsDBNull(Row.Cells("Importe").Value) Then Exit For
                Resul = ChequeRechazado(3, ConexionMovimiento, Row.Cells("ClaveCheque").Value)
                If Resul < 0 Then
                    MsgBox("Error Base de Datos")
                    Exit Sub
                End If
                If Resul > 0 Then
                    MsgBox("Cheque " & Row.Cells("Numero").Value & " no se puede Borrar. Existe Nota de Rechazo. Operación se CANCELA.")
                    Exit Sub
                End If
            Next
        End If

        Dim DtCabezaAux As New DataTable
        DtCabezaAux = DtCabeza.Copy
        Dim DtDetalleAux As New DataTable
        DtDetalleAux = DtDetalle.Copy

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        '
        If Not HallaAsientosCabeza(TipoAsiento, DtCabeza.Rows(0).Item("Movimiento"), DtAsientoCabeza, ConexionMovimiento) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        For Each Row As DataRow In DtAsientoCabeza.Rows
            Row("Estado") = 3
        Next

        If MsgBox("Movimiento se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtCabezaAux.Rows(0).Item("Estado") = 3

        Resul = ActualizaMovi("B", DtCabezaAux, DtDetalleAux, ConexionMovimiento, 0, DtAsientoCabeza, DtAsientoDetalle)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If Resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = -2 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul > 0 Then
            MsgBox("Anulación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            If Not ArmaArchivos() Then Me.Close() : Exit Sub
        End If

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PMovimiento = 0 Then
            MsgBox("Opcion Invalida. Movimiento debe ser Grabado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ErrorImpresion = False
        Paginas = 0
        If Not PAbierto Then Copias = 1
        If PAbierto Then Copias = 1

        Dim print_document As New PrintDocument
        If PTipoNota <> 92 Then
            AddHandler print_document.PrintPage, AddressOf Print_Comprobante
        Else
            AddHandler print_document.PrintPage, AddressOf Print_ComprobanteTransferencia
        End If

        print_document.Print()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        PMovimiento = 0
        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.Default

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
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PMovimiento <> 0 Then Exit Sub

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionMovimiento = ConexionN
            PAbierto = False
        Else
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionMovimiento = Conexion
            PAbierto = True
        End If

    End Sub
    Private Sub TextComprobante_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComprobante.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextTotalComprobante_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextTotalComprobante.KeyPress

        If e.KeyChar = "." Then e.KeyChar = ","
        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCuentaRetiro_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCuentaRetiro.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCuentaDeposito_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCuentaDeposito.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNumeroCheque_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextTotalComprobante_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextTotalComprobante.Validating

        If Not IsNumeric(TextTotalComprobante.Text) Then
            MsgBox("Importe No Numérico.", MsgBoxStyle.Exclamation)
            TextTotalComprobante.Focus()
        Else
            TextTotalComprobante.Text = FormatNumber(CDbl(TextTotalComprobante.Text), GDecimales)
        End If

    End Sub
    Private Sub ComboBancos_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboBancoDeposito.Validating

        If IsNothing(ComboBancoDeposito.SelectedValue) Then ComboBancoDeposito.SelectedValue = 0

    End Sub
    Private Sub TextCambio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCambio.KeyPress

        EsNumerico(e.KeyChar, TextCambio.Text, 3)

    End Sub
    Private Sub ButtonLupaCuentaDeposito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonLupaCuentaDeposito.Click

        ListaBancos.PEsSeleccionaCuenta = True
        If PEsDepositoChequesTerceros Then ListaBancos.PEsSoloPesos = True
        ListaBancos.ShowDialog()
        If ListaBancos.PCuenta <> 0 Then
            If EsBancoNegro(ListaBancos.PBanco) Then
                MsgBox("Banco No Habilitado para esta Operación.", MsgBoxStyle.Information)
                ListaBancos.Dispose()
                Exit Sub
            End If
            ComboBancoDeposito.SelectedValue = ListaBancos.PBanco
            TextCuentaDeposito.Text = FormatNumber(ListaBancos.PCuenta, 0)
            LabelMonedaDeposito.Text = HallaMoneda(ListaBancos.PBanco, ListaBancos.PCuenta)
        End If
        ListaBancos.Dispose()

    End Sub
    Private Sub ButtonLupaCuentaRetiro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonLupaCuentaRetiro.Click

        ListaBancos.PEsSeleccionaCuenta = True
        ListaBancos.ShowDialog()
        If ListaBancos.PCuenta <> 0 Then
            If EsBancoNegro(ListaBancos.PBanco) And Not PEsExtraccionEfectivo Then
                MsgBox("Banco No Habilitado para esta Operación.", MsgBoxStyle.Information)
                ListaBancos.Dispose()
                Exit Sub
            End If
            If PEsExtraccionChequePropio Then
                If Not TieneChequeraCuenta(ListaBancos.PBanco, ListaBancos.PCuenta) Then
                    MsgBox("Banco No Tiene Chequera.", MsgBoxStyle.Information)
                    ListaBancos.Dispose()
                    Exit Sub
                End If
            End If
            ComboBancoRetiro.SelectedValue = ListaBancos.PBanco
            TextCuentaRetiro.Text = FormatNumber(ListaBancos.PCuenta, 0)
            LabelMonedaRetiro.Text = HallaMoneda(ListaBancos.PBanco, ListaBancos.PCuenta)
            If PEsExtraccionChequePropio Then
                DtGrid.Rows(0).Item("Banco") = ListaBancos.PBanco
                DtGrid.Rows(0).Item("Cuenta") = ListaBancos.PCuenta
                DtGrid.Rows(0).Item("Serie") = ListaBancos.PSerie
                If Not DtGrid.Rows(0).Item("eCheq") Then                                'eCheq.
                    DtGrid.Rows(0).Item("Numero") = ListaBancos.PUltimoNumero + 1
                End If
                Grid.Refresh()
            End If
        End If
        ListaBancos.Dispose()

    End Sub
    Private Function ArmaArchivos() As Boolean

        CreaDtGrid()

        Dim RowsBusqueda() As DataRow

        DtCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM MovimientosBancarioCabeza WHERE Movimiento = " & PMovimiento & ";", ConexionMovimiento, DtCabeza) Then Return False
        If PMovimiento <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Movimiento No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PMovimiento = 0 Then
            Dim Row As DataRow
            Row = DtCabeza.NewRow
            ArmaNuevoMovimientoBancario(Row)
            Row("TipoNota") = PTipoNota
            Row("Fecha") = Now
            Row("Diferido") = PEsDiferidos
            Row("FechaComprobante") = DateTime2.Value
            Row("Caja") = GCaja
            Row("Estado") = 1
            DtCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        If PMovimiento <> 0 Then
            If DtCabeza.Rows(0).Item("TipoNota") = 91 Then
                If DtCabeza.Rows(0).Item("MedioPago") = 3 Then
                    PEsDepositoChequesTerceros = True
                Else : PEsDepositoEfectivo = True = True
                End If
            End If
            If DtCabeza.Rows(0).Item("TipoNota") = 90 Then
                If DtCabeza.Rows(0).Item("MedioPago") = 2 Then
                    PEsExtraccionChequePropio = True
                Else : PEsExtraccionEfectivo = True
                End If
            End If
            If DtCabeza.Rows(0).Item("MedioPago") = 92 Then PEsTransferenciaPropia = True
        End If

        If PEsDepositoEfectivo Then PanelDeposito.Visible = True
        If PEsExtraccionEfectivo Then PanelRetiro.Visible = True
        If PEsTransferenciaPropia Then PanelRetiro.Visible = True : PanelDeposito.Visible = True
        If PEsExtraccionChequePropio Then PanelGrid.Visible = True : PanelRetiro.Visible = True : LabelImporte.Text = "Importe Cheque"
        If PEsDepositoChequesTerceros Then
            PanelGrid.Visible = True : PanelDeposito.Visible = True
            TextTotalComprobante.ReadOnly = True
        End If

        If PEsDepositoChequesTerceros Or PEsDiferidos Then TipoAsiento = 6091
        If PEsDepositoEfectivo Then TipoAsiento = 91
        If PTipoNota = 90 Then TipoAsiento = 90
        If PTipoNota = 93 Then TipoAsiento = 93
        If PEsTransferenciaPropia Then TipoAsiento = 6080

        If DtCabeza.Rows(0).Item("Banco") <> 0 Then
            LabelMonedaRetiro.Text = HallaMoneda(DtCabeza.Rows(0).Item("Banco"), DtCabeza.Rows(0).Item("Cuenta"))
            LabelMonedaDeposito.Text = HallaMoneda(DtCabeza.Rows(0).Item("Banco"), DtCabeza.Rows(0).Item("Cuenta"))
        End If

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM MovimientosBancarioDetalle WHERE Movimiento = " & PMovimiento & ";", ConexionMovimiento, DtDetalle) Then Return False

        Dim DtCheques As New DataTable
        For Each Row As DataRow In DtDetalle.Rows
            If Row("ClaveCheque") <> 0 Then
                If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & DtCabeza.Rows(0).Item("Mediopago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionMovimiento, DtCheques) Then Me.Close() : Exit Function
            End If
        Next

        If PEsDepositoChequesTerceros Or PEsExtraccionChequePropio Then
            For Each Row As DataRow In DtDetalle.Rows
                Dim Row1 As DataRow = DtGrid.NewRow
                Row1("Item") = Row("Item")
                Row1("Importe") = Row("Importe")
                Row1("ClaveCheque") = Row("ClaveCheque")
                If Row("ClaveCheque") <> 0 Then
                    RowsBusqueda = DtCheques.Select("ClaveCheque = " & Row("ClaveCheque"))
                    If RowsBusqueda.Length <> 0 Then
                        Row1("Serie") = RowsBusqueda(0).Item("Serie")
                        Row1("Banco") = RowsBusqueda(0).Item("Banco")
                        Row1("Numero") = RowsBusqueda(0).Item("Numero")
                        Row1("Cuenta") = RowsBusqueda(0).Item("Cuenta")
                        Row1("EmisorCheque") = RowsBusqueda(0).Item("EmisorCheque")
                        Row1("Fecha") = RowsBusqueda(0).Item("Fecha")
                        Row1("Operacion") = 0
                        Row1("eCheq") = RowsBusqueda(0).Item("eCheq")
                    Else
                        Row1("Banco") = 0
                        Row1("Serie") = ""
                        Row1("Numero") = 0
                        Row1("EmisorCheque") = "Cheque No Existe."
                        Row1("Fecha") = "1/1/1800"
                        Row1("Cuenta") = 0
                        Row1("Operacion") = 0
                        Row1("eCheq") = False
                    End If
                End If
                DtGrid.Rows.Add(Row1)
            Next
        End If

        DtCheques.Dispose()

        If PTipoNota = 90 Then Label1.Text = "EXTRACCION"
        If PTipoNota = 91 Then Label1.Text = "DEPOSITO"
        If PTipoNota = 92 Then Label1.Text = "TRASFERENCIA BANCARIA  (Cuentas Propias)"

        If PEsDepositoChequesTerceros Then
            Grid.Columns("Importe").ReadOnly = True
            Grid.Columns("Banco").ReadOnly = True
            Grid.Columns("Cuenta").Visible = False
            Grid.Columns("Serie").ReadOnly = True
            Grid.Columns("Numero").ReadOnly = True
            Grid.Columns("Fecha").ReadOnly = True
            Grid.Columns("EmisorCheque").ReadOnly = True
        End If
        If PEsExtraccionChequePropio Then
            Grid.Columns("Banco").ReadOnly = True
            Grid.Columns("Cuenta").ReadOnly = True
            Grid.Columns("Serie").ReadOnly = True
            Grid.Columns("LupaCuenta").Visible = False
            Grid.Columns("ClaveCheque").Visible = False
            Grid.Columns("Fecha").ReadOnly = True
            Grid.Columns("EmisorCheque").ReadOnly = True
            PanelOperacionCheque.Visible = False
            ButtonEliminarLinea.Visible = False
            Grid.AllowUserToAddRows = False
            If PMovimiento = 0 Then
                Dim Row As DataRow = DtGrid.NewRow
                Row("Operacion") = 1
                Row("Importe") = 0
                Row("Banco") = 0
                Row("Cuenta") = 0
                Row("Fecha") = "1/1/1800"
                Row("Serie") = ""
                Row("Numero") = 0
                Row("EmisorCheque") = ""
                Row("ClaveCheque") = 0
                Row("eCheq") = False
                DtGrid.Rows.Add(Row)
            End If
        End If

        bs.DataSource = DtGrid
        Grid.DataSource = bs

        If PMovimiento = 0 Then
            Panel1.Enabled = True
            PanelDeposito.Enabled = True
            Panel4.Enabled = True
            PanelRetiro.Enabled = True
            PanelDeposito.Enabled = True
            ButtonEliminarLinea.Enabled = True
            TextComentario.ReadOnly = False
            Grid.ReadOnly = False
            PanelMoneda.Enabled = True
        Else
            Panel1.Enabled = False
            PanelDeposito.Enabled = False
            Panel4.Enabled = False
            PanelRetiro.Enabled = False
            PanelDeposito.Enabled = False
            ButtonEliminarLinea.Enabled = False
            TextComentario.ReadOnly = True
            Grid.ReadOnly = True
            PanelMoneda.Enabled = False
        End If

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Movimiento")
        AddHandler Enlace.Format, AddressOf FormatMaskedMovimiento
        MaskedMovimiento.DataBindings.Clear()
        MaskedMovimiento.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comprobante")
        AddHandler Enlace.Format, AddressOf FormatComprobante
        AddHandler Enlace.Parse, AddressOf ParseComprobante
        TextComprobante.DataBindings.Clear()
        TextComprobante.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaComprobante")
        DateTime2.DataBindings.Clear()
        DateTime2.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        If DtCabeza.Rows(0).Item("TipoNota") = 91 Or DtCabeza.Rows(0).Item("TipoNota") = 93 Then
            Enlace = New Binding("SelectedValue", MiEnlazador, "Banco")
            ComboBancoDeposito.DataBindings.Clear()
            ComboBancoDeposito.DataBindings.Add(Enlace)
            '
            Enlace = New Binding("Text", MiEnlazador, "Cuenta")
            AddHandler Enlace.Format, AddressOf FormatComprobante
            AddHandler Enlace.Parse, AddressOf ParseComprobante
            TextCuentaDeposito.DataBindings.Clear()
            TextCuentaDeposito.DataBindings.Add(Enlace)
        End If

        If DtCabeza.Rows(0).Item("TipoNota") = 90 Then
            Enlace = New Binding("SelectedValue", MiEnlazador, "Banco")
            ComboBancoRetiro.DataBindings.Clear()
            ComboBancoRetiro.DataBindings.Add(Enlace)
            '
            Enlace = New Binding("Text", MiEnlazador, "Cuenta")
            AddHandler Enlace.Format, AddressOf FormatComprobante
            AddHandler Enlace.Parse, AddressOf ParseComprobante
            TextCuentaRetiro.DataBindings.Clear()
            TextCuentaRetiro.DataBindings.Add(Enlace)
        End If

        If DtCabeza.Rows(0).Item("TipoNota") = 92 Then
            Enlace = New Binding("SelectedValue", MiEnlazador, "Banco")
            ComboBancoRetiro.DataBindings.Clear()
            ComboBancoRetiro.DataBindings.Add(Enlace)
            '
            Enlace = New Binding("Text", MiEnlazador, "Cuenta")
            AddHandler Enlace.Format, AddressOf FormatComprobante
            AddHandler Enlace.Parse, AddressOf ParseComprobante
            TextCuentaRetiro.DataBindings.Clear()
            TextCuentaRetiro.DataBindings.Add(Enlace)
            '
            Enlace = New Binding("SelectedValue", MiEnlazador, "BancoDestino")
            ComboBancoDeposito.DataBindings.Clear()
            ComboBancoDeposito.DataBindings.Add(Enlace)
            '
            Enlace = New Binding("Text", MiEnlazador, "CuentaDestino")
            AddHandler Enlace.Format, AddressOf FormatComprobante
            AddHandler Enlace.Parse, AddressOf ParseComprobante
            TextCuentaDeposito.DataBindings.Clear()
            TextCuentaDeposito.DataBindings.Add(Enlace)
        End If

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalComprobante.DataBindings.Clear()
        TextTotalComprobante.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cambio")
        AddHandler Enlace.Format, AddressOf FormatCambio
        AddHandler Enlace.Parse, AddressOf ParseComprobante
        TextCambio.DataBindings.Clear()
        TextCambio.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatMaskedMovimiento(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "000000000000")

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub FormatCambio(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, 3)

    End Sub
    Private Sub FormatComprobante(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(0, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub ParseComprobante(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub ArmaDtCuentas()

        DtCuentas = New DataTable

        If Not Tablas.Read("SELECT * FROM CuentasBancarias;", Conexion, DtCuentas) Then End

    End Sub
    Private Function HacerAlta() As Boolean

        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        Dim DtDetalleAux As DataTable = DtDetalle.Copy

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PEsExtraccionEfectivo Or PEsDepositoEfectivo Then
            DtCabezaAux.Rows(0).Item("MedioPago") = HallaMonedaBanco(DtCabezaAux.Rows(0).Item("Banco"), DtCabezaAux.Rows(0).Item("Cuenta"))
        End If
        If PEsExtraccionChequePropio Then DtCabezaAux.Rows(0).Item("MedioPago") = 2
        If PEsDepositoChequesTerceros Then DtCabezaAux.Rows(0).Item("MedioPago") = 3
        If PEsTransferenciaPropia Then DtCabezaAux.Rows(0).Item("MedioPago") = 92

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Function
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Function
            If Not ArmaArchivosAsiento("A", DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Function
        End If

        Dim NumeroMovi As Double
        Dim Resul As Double
        Dim NumeroAsiento As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            NumeroMovi = UltimaNumeracion(ConexionMovimiento)
            If NumeroMovi < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            DtCabezaAux.Rows(0).Item("Movimiento") = NumeroMovi

            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionMovimiento)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Function
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = NumeroMovi
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            Resul = ActualizaMovi("A", DtCabezaAux, DtDetalleAux, ConexionMovimiento, NumeroMovi, DtAsientoCabeza, DtAsientoDetalle)

            If Resul >= 0 Then Exit For
            If Resul = -3 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        Me.Cursor = System.Windows.Forms.Cursors.Default

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
            PMovimiento = Resul
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function ActualizaMovi(ByVal Funcion As String, ByVal DtCabezaAux As DataTable, ByVal DtDetalleAux As DataTable, ByVal ConexionStr As String, ByVal Movimiento As Double, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Funcion = "A" Then
                    If PEsExtraccionChequePropio Then
                        DtDetalleAux.Clear()
                        Dim Row1 As DataRow = DtDetalleAux.NewRow
                        Row1("Movimiento") = Movimiento
                        Row1("Importe") = CDbl(TextTotalComprobante.Text)
                        Row1("ClaveCheque") = 0
                        Row1("ClaveCheque") = ActualizaClavesComprobantes("A", 0, PTipoNota, 2, 1, ComboBancoRetiro.SelectedValue, CDbl(TextCuentaRetiro.Text), DtGrid.Rows(0).Item("Serie"), DtGrid.Rows(0).Item("Numero"), DtGrid.Rows(0).Item("Fecha"), DtGrid.Rows(0).Item("Importe"), "", Movimiento, DateTime2.Value, ConexionMovimiento, False, False, DtGrid.Rows(0).Item("eCheq"))
                        If Row1("ClaveCheque") <= 0 Then Return Row1("ClaveCheque")
                        DtDetalleAux.Rows.Add(Row1)
                    End If
                    If PEsDepositoChequesTerceros Then
                        DtDetalleAux.Clear()
                        For Each Row As DataRow In DtGrid.Rows
                            If Row.RowState <> DataRowState.Deleted Then
                                Dim Row1 As DataRow = DtDetalleAux.NewRow
                                Row1("Movimiento") = Movimiento
                                Row1("Importe") = Row("Importe")
                                Row1("ClaveCheque") = Row("ClaveCheque")
                                If ActualizaClavesComprobantes("A", Row("ClaveCheque"), PTipoNota, DtCabezaAux.Rows(0).Item("MedioPago"), Row("Operacion"), Row("Banco"), Row("Cuenta"), Row("Serie"), Row("Numero"), Row("Fecha"), Row("Importe"), "", Movimiento, "01/01/1800", ConexionMovimiento, False, False, DtGrid.Rows(0).Item("eCheq")) <= 0 Then
                                    Return 0
                                Else
                                End If
                                DtDetalleAux.Rows.Add(Row1)
                            End If
                        Next
                    End If
                End If
                '
                If Funcion = "B" Then
                    If PEsExtraccionChequePropio Then
                        Dim Resul2 As Integer = ActualizaClavesComprobantes("B", DtGrid.Rows(0).Item("ClaveCheque"), PTipoNota, DtCabezaAux.Rows(0).Item("MedioPago"), 1, ComboBancoRetiro.SelectedValue, CDbl(TextCuentaRetiro.Text), DtGrid.Rows(0).Item("Serie"), DtGrid.Rows(0).Item("Numero"), DtGrid.Rows(0).Item("Fecha"), DtGrid.Rows(0).Item("Importe"), "", Movimiento, "01/01/1800", ConexionMovimiento, False, False, False)
                        If Resul2 <= 0 Then Return Resul2
                    End If
                    If PEsDepositoChequesTerceros Then
                        For Each Row As DataRow In DtGrid.Rows
                            If Row.RowState <> DataRowState.Deleted Then
                                If ActualizaClavesComprobantes("B", Row("ClaveCheque"), PTipoNota, DtCabezaAux.Rows(0).Item("MedioPago"), Row("Operacion"), ComboBancoDeposito.SelectedValue, CDbl(TextCuentaDeposito.Text), Row("Serie"), Row("Numero"), Row("Fecha"), Row("Importe"), "", Movimiento, "01/01/1800", ConexionMovimiento, False, False, False) <= 0 Then
                                    Return 0
                                Else
                                End If
                            End If
                        Next
                    End If
                End If
                '
                If Not IsNothing(DtDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleAux.GetChanges, "MovimientosBancarioDetalle", ConexionMovimiento)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaAux.GetChanges, "MovimientosBancarioCabeza", ConexionMovimiento)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionMovimiento)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionMovimiento)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                If Funcion = "A" Then
                    Return Movimiento
                Else
                    Return 1000
                End If
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function HallaMoneda(ByVal Banco As Integer, ByVal Cuenta As Double) As String

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtCuentas.Select("Banco = " & Banco & " AND Numero = " & Cuenta)
        If RowsBusqueda(0).Item("Moneda") = 1 Then
            Return "( Pesos )"
        Else
            Return "( " & Moneda(RowsBusqueda(0).Item("Moneda")) & " )"
        End If

    End Function
    Private Function HallaMonedaBanco(ByVal Banco As Integer, ByVal Cuenta As Double) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtCuentas.Select("Banco = " & Banco & " AND Numero = " & Cuenta)
        Return RowsBusqueda(0).Item("Moneda")

    End Function
    Private Function TieneChequeraCuenta(ByVal Banco As Integer, ByVal Cuenta As Double) As Boolean

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtCuentas.Select("Banco = " & Banco & " AND Numero = " & Cuenta)
        Return RowsBusqueda(0).Item("TieneChequera")

    End Function
    Private Function EsBancoNegro(ByVal Banco As Integer) As Boolean

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtBancosNegro.Select("Clave = " & Banco)
        If RowsBusqueda.Length <> 0 Then Return True

        Return False

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

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
    Private Function Moneda(ByVal Clave As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Item As New DataColumn("Item")
        Item.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Item)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim Banco As New DataColumn("Banco")
        Banco.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Banco)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cuenta)

        Dim Serie As New DataColumn("Serie")
        Serie.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Serie)

        Dim Numero As New DataColumn("Numero")
        Numero.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Numero)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim EmisorCheque As New DataColumn("EmisorCheque")
        EmisorCheque.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(EmisorCheque)

        Dim ClaveCheque As New DataColumn("ClaveCheque")
        ClaveCheque.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ClaveCheque)

        Dim NumeracionInicial As New DataColumn("NumeracionInicial")
        NumeracionInicial.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(NumeracionInicial)

        Dim NumeracionFinal As New DataColumn("NumeracionFinal")
        NumeracionFinal.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(NumeracionFinal)

        Dim eCheq As New DataColumn("eCheq")
        eCheq.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(eCheq)

    End Sub
    Private Sub CalculaTotales()

        Dim TotalConceptos As Double = 0

        For Each Row As DataGridViewRow In Grid.Rows
            If Not IsDBNull(Row.Cells("Importe").Value) Then
                TotalConceptos = TotalConceptos + Row.Cells("Importe").Value
            End If
        Next

        TextTotalComprobante.Text = FormatNumber(TotalConceptos, GDecimales)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow
        Dim RowsBusqueda(0) As DataRow

        Banco.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 order By Nombre;")
        Row = Banco.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Banco.DataSource.Rows.Add(Row)
        Banco.DisplayMember = "Nombre"
        Banco.ValueMember = "Clave"

    End Sub
    Private Sub IngresaCheques(ByVal ListaDeChequesAux As List(Of ItemCheque))

        Grid.DataSource = Nothing
        Dim Dt As DataTable = DtGrid.Copy
        DtGrid.Clear()

        For Each Row As DataRow In Dt.Rows
            If Row("Item") <> 0 Then
                DtGrid.ImportRow(Row)
            End If
        Next
        For Each Item As ItemCheque In ListaDeChequesAux
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Item") = 0
            Row1("Operacion") = Item.Operacion
            Row1("Banco") = Item.Banco
            Row1("Fecha") = Item.Fecha
            Row1("Cuenta") = Item.Cuenta
            Row1("Serie") = Item.Serie
            Row1("Numero") = Item.Numero
            Row1("EmisorCheque") = Item.EmisorCheque
            Row1("Importe") = Item.Importe
            Row1("ClaveCheque") = Item.ClaveCheque
            Row1("eCheq") = Item.echeq                      'eCheq.
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = DtGrid
        Grid.EndEdit()

        Dt.Dispose()

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable) As Boolean

        If TipoAsiento = 0 Then Return True

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos

        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = CDbl(TextTotalComprobante.Text)
        If DtCabeza.Rows(0).Item("Cambio") <> 0 Then Item.Importe = Trunca(Item.Importe * DtCabeza.Rows(0).Item("Cambio"))
        ListaConceptos.Add(Item)
        '
        If Not Asiento(TipoAsiento, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime2.Value, 0) Then Return False

        Return True

    End Function
    Private Sub Print_Comprobante(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.


        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim Cartel As String

        If PTipoNota = 91 Then
            Cartel = "DEPOSITO"
        End If
        If PTipoNota = 90 Then
            Cartel = "EXTRACION"
        End If

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            'Encabezado.
            Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
            Texto = GNombreEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
            Texto = "MOVIMIENTO BANCARIO"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 115, MTop)
            Texto = Cartel
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 75, MTop + 10)
            ' 
            PrintFont = New Font("Courier New", 12)
            Texto = "MOVIMIENTO     :  " & NumeroEditado(MaskedMovimiento.Text)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 25)
            Texto = "FECHA :  " & DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 25)
            Texto = "COMPROBANTE    :  " & TextComprobante.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 40)
            Texto = "FECHA COMPROBANTE : " & DateTime2.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 90, MTop + 40)
            If PTipoNota = 91 Then
                Texto = "BANCO          :  " & ComboBancoDeposito.Text
            Else
                Texto = "BANCO          :  " & ComboBancoRetiro.Text
            End If
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 45)
            If PTipoNota = 91 Then
                Texto = "CUENTA         :  " & TextCuentaDeposito.Text
            Else
                Texto = "CUENTA         :  " & TextCuentaRetiro.Text
            End If
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 50)

            Texto = "IMPORTE   : " & TextTotalComprobante.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 60, MTop + 60)

            Dim Longi As Integer
            Dim Xq As Integer
            Dim Yq As Integer
            Dim Ancho As Integer
            Dim Alto As Integer

            'Grafica -Rectangulo medios de pagos. ----------------------------------------------------------------------
            x = MIzq
            y = MTop + 75
            Ancho = 180
            Alto = 95
            PrintFont = New Font("Courier New", 10)
            Dim LineaImporte As Integer = x + 30
            Dim LineaBanco As Integer = x + 80
            Dim LineaCuenta As Integer = x + 115
            Dim LineaNumero As Integer = x + 150
            Dim LineaVencimiento As Integer = x + Ancho
            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte, y, LineaImporte, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaBanco, y, LineaBanco, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCuenta, y, LineaCuenta, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaNumero, y, LineaNumero, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaVencimiento, y, LineaVencimiento, y + Alto)
            'Titulos de descripcion.
            Texto = "DESCRIPCION"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, y - 4)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "BANCO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaBanco - Longi - 20
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "CUENTA"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCuenta - Longi - 10
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
                If IsNothing(Row.Cells("Importe").Value) Then Exit For
                Yq = Yq + SaltoLinea
                'Imprime Importe.
                Texto = Row.Cells("Importe").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Banco.
                Texto = Row.Cells("Banco").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte + 1
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Cuenta.
                Texto = Row.Cells("Cuenta").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaCuenta - Longi
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
            Next

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
    Private Sub Print_ComprobanteTransferencia(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.


        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim Cartel As String

        Cartel = "TRANSFERENCIA"

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            'Encabezado.
            Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
            Texto = GNombreEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
            Texto = "MOVIMIENTO BANCARIO"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 115, MTop)
            Texto = Cartel
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 75, MTop + 10)
            ' 
            PrintFont = New Font("Courier New", 12)
            Texto = "MOVIMIENTO     :  " & NumeroEditado(MaskedMovimiento.Text)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 25)
            Texto = "FECHA :  " & DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 25)
            Texto = "COMPROBANTE    :  " & TextComprobante.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 40)
            Texto = "FECHA COMPROBANTE : " & DateTime2.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 90, MTop + 40)

            Texto = "CUENTA DEPOSITO "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 60)
            Texto = "BANCO          :  " & ComboBancoDeposito.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 65)
            Texto = "CUENTA         :  " & TextCuentaDeposito.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 70)

            Texto = "CUENTA EXTRACION "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 80)
            Texto = "BANCO          :  " & ComboBancoRetiro.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 85)
            Texto = "CUENTA         :  " & TextCuentaRetiro.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 90)

            Texto = "IMPORTE   : " & TextTotalComprobante.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 60, MTop + 120)

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
    Private Function Valida() As Boolean

        Dim RowsBusqueda() As DataRow

        If DiferenciaDias(DateTime1.Value, DateTime2.Value) > 0 Then
            MsgBox("Fecha del Comprobante mayor a Fecha del Movimiento Bancario.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime2.Focus()
            Return False
        End If

        If Not PEsExtraccionChequePropio Then
            If TextComprobante.Text = "" Then
                MsgBox("Debe Informar Numero de Comprobante", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextComprobante.Focus()
                Return False
            End If
            If CDbl(TextComprobante.Text) = 0 Then
                MsgBox("Debe Informar Numero de Comprobante", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextComprobante.Focus()
                Return False
            End If
            If DiferenciaDias(DateTime2.Value, Date.Now) < 0 Then
                MsgBox("Fecha de la Comprobante es mayor a la actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                DateTime2.Focus()
                Return False
            End If
        End If

        If PEsExtraccionEfectivo Or PEsExtraccionChequePropio Or PEsTransferenciaPropia Then
            If ComboBancoRetiro.SelectedValue = 0 Then
                MsgBox("Debe Informar Banco Retiro.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboBancoRetiro.Focus()
                Return False
            End If
            If TextCuentaRetiro.Text = "" Then
                MsgBox("Debe Informar Cuenta Retiro.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextCuentaRetiro.Focus()
                Return False
            End If
            If CDbl(TextCuentaRetiro.Text) = 0 Then
                MsgBox("Debe Informar Cuenta Retiro.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextCuentaRetiro.Focus()
                Return False
            End If
        End If

        If PEsDepositoEfectivo Or PEsTransferenciaPropia Or PEsDepositoChequesTerceros Then
            If ComboBancoDeposito.SelectedValue = 0 Then
                MsgBox("Debe Informar Banco Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboBancoDeposito.Focus()
                Return False
            End If
            If TextCuentaDeposito.Text = "" Then
                MsgBox("Debe Informar Cuenta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextCuentaDeposito.Focus()
                Return False
            End If
            If CDbl(TextCuentaDeposito.Text) = 0 Then
                MsgBox("Debe Informar Cuenta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextCuentaDeposito.Focus()
                Return False
            End If
        End If

        If PEsExtraccionChequePropio Then
            If DtGrid.Rows(0).Item("Importe") = 0 Then
                MsgBox("Debe Informar Importe del Cheque.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(0).Cells("Importe")
                Return False
            End If
            If DtGrid.Rows(0).Item("Fecha") = "1/1/1800" Then
                MsgBox("Debe Informar Fecha Cheque.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(0).Cells("Fecha")
                Return False
            End If
            If ChequeVencido(DtGrid.Rows(0).Item("Fecha"), DateTime2.Value) Then
                MsgBox("Cheque Vencido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(0).Cells("Fecha")
                Return False
            End If
            RowsBusqueda = DtCuentas.Select("Banco = " & ComboBancoRetiro.SelectedValue & " AND Numero = " & CDbl(TextCuentaRetiro.Text))
            If RowsBusqueda.Length <> 0 Then
                If DtGrid.Rows(0).Item("Numero") < RowsBusqueda(0).Item("NumeracionInicial") Or DtGrid.Rows(0).Item("Numero") > RowsBusqueda(0).Item("NumeracionFinal") Then
                    MsgBox("Numero Cheque no corresponde a la Numeración de la Chequera.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(0).Cells("Numero")
                    Return False
                End If
            End If
        End If

        If ComboBancoRetiro.SelectedValue = ComboBancoDeposito.SelectedValue And TextCuentaRetiro.Text = TextCuentaDeposito.Text Then
            MsgBox("Cuenta Retiro no debe ser igual a la Cuenta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboBancoRetiro.Focus()
            Return False
        End If

        Dim Moneda As Integer

        If PEsTransferenciaPropia Then
            RowsBusqueda = DtCuentas.Select("Banco = " & ComboBancoRetiro.SelectedValue & " AND Numero = " & CDbl(TextCuentaRetiro.Text))
            Moneda = RowsBusqueda(0).Item("Moneda")
            RowsBusqueda = DtCuentas.Select("Banco = " & ComboBancoDeposito.SelectedValue & " AND Numero = " & CDbl(TextCuentaDeposito.Text))
            If Moneda <> RowsBusqueda(0).Item("Moneda") Then
                MsgBox("Difiere Monedas en Cuentas.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If
        If PanelDeposito.Visible = True Then
            RowsBusqueda = DtCuentas.Select("Banco = " & ComboBancoDeposito.SelectedValue & " AND Numero = " & CDbl(TextCuentaDeposito.Text))
            Moneda = RowsBusqueda(0).Item("Moneda")
        End If
        If PanelRetiro.Visible = True Then
            RowsBusqueda = DtCuentas.Select("Banco = " & ComboBancoRetiro.SelectedValue & " AND Numero = " & CDbl(TextCuentaRetiro.Text))
            Moneda = RowsBusqueda(0).Item("Moneda")
        End If

        If Moneda <> 1 Then
            If TextCambio.Text = "" Then
                MsgBox("Falta Informar Cambio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If CDbl(TextCambio.Text) = 0 Then
                MsgBox("Falta Informar Cambio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        Else
            If TextCambio.Text <> "" Then
                If Not (CDbl(TextCambio.Text) = 0 Or CDbl(TextCambio.Text) = 1) Then
                    MsgBox("Cambio Incorrecto para Moneda Local.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
        End If

        If PEsDepositoChequesTerceros Then
            Dim Row As DataGridViewRow
            For i As Integer = 1 To Grid.Rows.Count - 2
                Row = Grid.Rows(i)
                For Y As Integer = 0 To i - 1
                    If Row.Cells("ClaveCheque").Value = Grid.Rows(Y).Cells("ClaveCheque").Value And Row.Cells("Operacion").Value = Grid.Rows(Y).Cells("Operacion").Value Then
                        MsgBox("Cheque Repetido en la Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Grid.CurrentCell = Grid.Rows(i).Cells("Numero")
                        Grid.BeginEdit(True)
                        Return False
                    End If
                Next
            Next
            For i As Integer = 0 To Grid.Rows.Count - 2
                Row = Grid.Rows(i)
                If ChequeVencido(Row.Cells("Fecha").Value, DateTime2.Value) Then
                    MsgBox("Cheque Vencido en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Fecha")
                    Grid.BeginEdit(True)
                    Return False
                End If
            Next
            If PEsDiferidos Then
                For i As Integer = 0 To Grid.Rows.Count - 2
                    Row = Grid.Rows(i)
                    If DiferenciaDias(DateTime2.Value, Row.Cells("Fecha").Value) <= 0 Then
                        MsgBox("Fecha Cheque No Valida Para Cheque Diferido en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Grid.CurrentCell = Grid.Rows(i).Cells("Fecha")
                        Grid.BeginEdit(True)
                        Return False
                    End If
                Next
            Else
                For i As Integer = 0 To Grid.Rows.Count - 2
                    Row = Grid.Rows(i)
                    If DiferenciaDias(DateTime2.Value, Row.Cells("Fecha").Value) > 0 Then
                        MsgBox("Fecha Cheque No Valida Para Cheque Al Dia en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Grid.CurrentCell = Grid.Rows(i).Cells("Fecha")
                        Grid.BeginEdit(True)
                        Return False
                    End If
                Next
            End If
        End If

        If ComboBancoDeposito.SelectedValue <> 0 Then
            RowsBusqueda = DtCuentas.Select("Banco = " & ComboBancoDeposito.SelectedValue & " AND Numero = " & CDbl(TextCuentaDeposito.Text))
            If RowsBusqueda.Length = 0 Then
                MsgBox("Cuenta no pertenece al Banco de Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextCuentaDeposito.Focus()
                Return False
            End If
        End If
        If ComboBancoRetiro.SelectedValue <> 0 Then
            RowsBusqueda = DtCuentas.Select("Banco = " & ComboBancoRetiro.SelectedValue & " AND Numero = " & CDbl(TextCuentaRetiro.Text))
            If RowsBusqueda.Length = 0 Then
                MsgBox("Cuenta no pertenece al Banco de Retiro.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextCuentaDeposito.Focus()
                Return False
            End If
        End If

        If TextTotalComprobante.Text = "" Then
            MsgBox("Debe Informar Importe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextTotalComprobante.Focus()
            Return False
        End If
        If CDbl(TextTotalComprobante.Text) = 0 Then
            MsgBox("Debe Informar Importe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextTotalComprobante.Focus()
            Return False
        End If

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
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Banco" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
                If cb.SelectedIndex < 0 Then cb.SelectedValue = 0
            End If
        End If

    End Sub
    Private Sub Grid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.CellBeginEdit

        If e.RowIndex > 0 And Not PEsDepositoChequesTerceros Then e.Cancel = True

        'eCheq.
        If Grid.Columns(e.ColumnIndex).Name = "eCheq" Then
            If PTipoNota = 91 Then e.Cancel = True
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        CalculaTotales()

    End Sub
    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick

        If PMovimiento <> 0 Then Exit Sub

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta1" Then
            SeleccionarCheques.PAbierto = PAbierto
            SeleccionarCheques.PEsSoloUno = True
            SeleccionarCheques.PEsChequeEnCartera = True
            SeleccionarCheques.PCaja = GCaja
            SeleccionarCheques.ShowDialog()
            If SeleccionarCheques.PClave <> 0 Then
                For I As Integer = 0 To Grid.Rows.Count - 2
                    If Grid.Rows(I).Cells("Operacion").Value <> SeleccionarCheques.POperacion Then
                        MsgBox("Operación del Cheque Difiere de los YA Informados.")
                        Exit Sub
                    End If
                Next
                Grid.CurrentRow.Cells("Banco").Value = SeleccionarCheques.PBanco
                Grid.CurrentRow.Cells("Serie").Value = SeleccionarCheques.PUltimaSerie
                Grid.CurrentRow.Cells("Numero").Value = SeleccionarCheques.PUltimoNumero
                Grid.CurrentRow.Cells("Cuenta").Value = SeleccionarCheques.PCuenta
                Grid.CurrentRow.Cells("Importe").Value = SeleccionarCheques.PImporte
                Grid.CurrentRow.Cells("ClaveCheque").Value = SeleccionarCheques.PClave
                Grid.CurrentRow.Cells("Fecha").Value = SeleccionarCheques.PFecha
                Grid.CurrentRow.Cells("EmisorCheque").Value = SeleccionarCheques.PEmisorCheque
                Grid.CurrentRow.Cells("Operacion").Value = SeleccionarCheques.POperacion
                Grid.DataSource = Nothing
                Grid.DataSource = bs
                CalculaTotales()
            End If
            SeleccionarCheques.Dispose()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta2" Then
            SeleccionarCheques.PAbierto = PAbierto
            SeleccionarCheques.PEsSoloUno = True
            SeleccionarCheques.PEsChequeEnCartera = True
            SeleccionarCheques.PCaja = GCaja
            SeleccionarCheques.ShowDialog()
            If SeleccionarVarios.PClave <> 0 Then
                For I As Integer = 0 To Grid.Rows.Count - 2
                    If Grid.Rows(I).Cells("Operacion").Value <> SeleccionarCheques.POperacion Then
                        MsgBox("Operación del Cheque Difiere de los YA Informados.")
                        Exit Sub
                    End If
                Next
                Grid.CurrentRow.Cells("Banco").Value = SeleccionarCheques.PBanco
                Grid.CurrentRow.Cells("Serie").Value = SeleccionarCheques.PUltimaSerie
                Grid.CurrentRow.Cells("Numero").Value = SeleccionarVarios.PUltimoNumero
                Grid.CurrentRow.Cells("Cuenta").Value = SeleccionarCheques.PCuenta
                Grid.CurrentRow.Cells("Importe").Value = SeleccionarCheques.PImporte
                Grid.CurrentRow.Cells("ClaveCheque").Value = SeleccionarCheques.PClave
                Grid.CurrentRow.Cells("Fecha").Value = SeleccionarCheques.PFecha
                Grid.CurrentRow.Cells("EmisorCheque").Value = SeleccionarCheques.PEmisorCheque
                Grid.CurrentRow.Cells("Operacion").Value = SeleccionarCheques.POperacion
                Grid.DataSource = Nothing
                Grid.DataSource = bs
                CalculaTotales()
            End If
            SeleccionarCheques.Dispose()
        End If
        '
        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta" Then
            SeleccionarCheques.PCaja = GCaja
            SeleccionarCheques.PEsChequeEnCartera = True
            SeleccionarCheques.PListaDeCheques = New List(Of ItemCheque)
            For Each Row As DataRow In DtGrid.Rows
                If (Row("Operacion") = 1 And PAbierto) Or (Row("Operacion") = 2 And Not PAbierto) Then
                    Dim Item As New ItemCheque
                    Item.ClaveCheque = Row("ClaveCheque")
                    SeleccionarCheques.PListaDeCheques.Add(Item)
                End If
            Next
            SeleccionarCheques.PAbierto = PAbierto
            SeleccionarCheques.ShowDialog()
            If SeleccionarCheques.PListaDeCheques.Count <> 0 Then
                If DtGrid.Rows.Count <> 0 Then
                    If DtGrid.Rows(0).Item("Operacion") <> SeleccionarCheques.PListaDeCheques(0).Operacion Then
                        MsgBox("Operación de Cheques Seleccionados Difiere de los YA Informados.")
                        Exit Sub
                    End If
                End If
                IngresaCheques(SeleccionarCheques.PListaDeCheques)
                CalculaTotales()
            End If
            SeleccionarCheques.Dispose()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If PEsExtraccionChequePropio Then
                Calendario.ShowDialog()
                Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                Calendario.Dispose()
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = "1/1/1800" Then e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Or Grid.Columns(e.ColumnIndex).Name = "Cuenta" Or Grid.Columns(e.ColumnIndex).Name = "ClaveCheque" Then
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
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then Exit Sub

        Dim Row As DataRow
        Row = DtGrid.Rows.Item(Grid.CurrentRow.Index)
        Row.Delete()
        CalculaTotales()

    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        If PEsExtraccionChequePropio Then e.Row.Delete()

        Dim Row As DataRow = e.Row

        Row("Item") = 0
        Row("Operacion") = 1
        Row("Importe") = 0
        Row("Banco") = 0
        Row("Cuenta") = 0
        Row("Fecha") = "1/1/1800"
        Row("Serie") = ""
        Row("Numero") = 0
        Row("EmisorCheque") = ""
        Row("ClaveCheque") = 0
        Row("eCheq") = False

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

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
    Private Sub DtGrid_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        If DtGrid.Rows.Count > GLineasPagoRecibos Then
            MsgBox("Supera Cantidad Items Permitidos.", MsgBoxStyle.Information)
            e.Row.Delete()
        End If

    End Sub


End Class