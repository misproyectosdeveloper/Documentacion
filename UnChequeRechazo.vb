Imports System.Transactions
Public Class UnChequeRechazo
    Dim Dt As DataTable
    Dim DtRechazos As DataTable
    Dim TipoNotaRechazoOrigen As Integer = 0
    Dim NotaRechazoOrigen As Double = 0
    Dim TipoNotaRechazoDestino As Integer = 0
    Dim NotaRechazoDestino As Double = 0
    Dim FondoFijoDestino As Integer
    Dim NumeroFondoFijoDestino As Integer
    Dim FondoFijoOrigen As Integer
    Dim NumeroFondoFijoOrigen As Integer
    Dim PorCuenta As Integer = 0
    Dim PrestamoDestino As Decimal = 0
    Dim PrestamoOrigen As Decimal = 0
    Dim OrigenPrestamo As Integer = 0
    Dim CuentaBancaria As Decimal = 0
    '
    Dim ConexionCheque As String
    Dim Abierto As Boolean
    Dim Clave As Integer
    Dim MedioPago As Integer
    '
    Dim Origen As Integer
    Dim Emisor As Integer
    Dim EmisorDestino As Integer
    Private Sub UnRechazoDeCheque_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LlenaComboTablas(ComboBanco1, 26)

        LlenaComboTablas(ComboBanco, 26)
        ComboBanco.SelectedValue = 0
        With ComboBanco
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        PictureCandado1.Image = ImageList1.Images.Item("Abierto")
        Abierto = True
        If Not PermisoTotal Then PictureCandado1.Visible = False : PanelAfectado.Visible = False

        LabelCartel.Text = ""

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then
            If TextClaveCheque.Focused Then ButtonBuscaPorClave_Click(Nothing, Nothing)
        End If

    End Sub
    Private Sub TextNumeroCheque_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumeroCheque.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ButtonEmitirDebito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEmitirDebito.Click

        Dim Row As DataRow = Dt.Rows(0)

        If Row("CompDestino") <> 0 And NotaRechazoDestino = 0 Then
            MsgBox("Debe Emitir Previamente Nota de Credito. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Row("CompDestino") = 0 Then
            If Not ChequeSePuedeRechazar() Then Exit Sub
        End If

        If Dt.Rows(0).Item("TipoOrigen") = 6001 Then
            If TextEntregadoA.Text <> "" Then
                MsgBox("Cheque debe ser Previamente Reemplazado a " & TextEntregadoA.Text & ". Operación se CANCELA.", MsgBoxStyle.Critical)
                Exit Sub
            End If
        End If

        GModificacionOk = False

        Select Case Row("TipoOrigen")
            Case 60
                UnReciboDebitoCredito.PMedioPago = MedioPago
                UnReciboDebitoCredito.PTipoNota = 5
                UnReciboDebitoCredito.PNota = 0
                UnReciboDebitoCredito.PClaveCheque = Clave
                UnReciboDebitoCredito.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCredito.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCredito.PEsRechazoCheque = True
                UnReciboDebitoCredito.PEmisor = Emisor
                UnReciboDebitoCredito.PAbierto = Abierto
                UnReciboDebitoCredito.ShowDialog()
            Case 604
                UnReciboDebitoCredito.PMedioPago = MedioPago
                UnReciboDebitoCredito.PTipoNota = 6
                UnReciboDebitoCredito.PNota = 0
                UnReciboDebitoCredito.PClaveCheque = Clave
                UnReciboDebitoCredito.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCredito.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCredito.PEsRechazoCheque = True
                UnReciboDebitoCredito.PEmisor = Emisor
                UnReciboDebitoCredito.PAbierto = Abierto
                UnReciboDebitoCredito.ShowDialog()
            Case 7002
                UnReciboDebitoCreditoGenerica.PMedioPago = MedioPago
                UnReciboDebitoCreditoGenerica.PTipoNota = 7005
                UnReciboDebitoCreditoGenerica.PNota = 0
                UnReciboDebitoCreditoGenerica.PClaveCheque = Clave
                UnReciboDebitoCreditoGenerica.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCreditoGenerica.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCreditoGenerica.PEsRechazoCheque = True
                UnReciboDebitoCreditoGenerica.PProveedorFondoFijo = FondoFijoOrigen
                UnReciboDebitoCreditoGenerica.PNumero = NumeroFondoFijoOrigen
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.ShowDialog()
            Case 5020
                UnReciboDebitoCreditoOtrosProveedores.PMedioPago = MedioPago
                UnReciboDebitoCreditoOtrosProveedores.PTipoNota = 5005
                UnReciboDebitoCreditoOtrosProveedores.PNota = 0
                UnReciboDebitoCreditoOtrosProveedores.PClaveCheque = Clave
                UnReciboDebitoCreditoOtrosProveedores.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCreditoOtrosProveedores.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCreditoOtrosProveedores.PEsRechazoCheque = True
                UnReciboDebitoCreditoOtrosProveedores.PProveedor = Emisor
                UnReciboDebitoCreditoOtrosProveedores.PAbierto = Abierto
                UnReciboDebitoCreditoOtrosProveedores.ShowDialog()
            Case 1000, 1015   'nuevo prestamo y ajuste de capital.
                UnReciboDebitoCreditoGenerica.PMedioPago = MedioPago
                UnReciboDebitoCreditoGenerica.PTipoNota = 1005
                UnReciboDebitoCreditoGenerica.PNota = 0
                UnReciboDebitoCreditoGenerica.PClaveCheque = Clave
                UnReciboDebitoCreditoGenerica.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCreditoGenerica.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCreditoGenerica.PEsRechazoCheque = True
                UnReciboDebitoCreditoGenerica.PEmisorPrestamo = Emisor
                UnReciboDebitoCreditoGenerica.PPrestamo = PrestamoOrigen
                UnReciboDebitoCreditoGenerica.POrigenPrestamo = OrigenPrestamo
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.ShowDialog()
            Case 6001
                Select Case Origen
                    Case 3
                        UnReciboDebitoCredito.PTipoNota = 5
                    Case 2
                        UnReciboDebitoCredito.PTipoNota = 6
                End Select
                UnReciboDebitoCredito.PMedioPago = MedioPago
                UnReciboDebitoCredito.PNota = 0
                UnReciboDebitoCredito.PClaveCheque = Clave
                UnReciboDebitoCredito.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCredito.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCredito.PEsRechazoCheque = True
                UnReciboDebitoCredito.PEmisor = Emisor
                UnReciboDebitoCredito.PAbierto = Abierto
                UnReciboDebitoCredito.ShowDialog()
        End Select

        If GModificacionOk Then MuestraCheque()

    End Sub
    Private Sub ButtonEmitirDebitoInterno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEmitirDebitoInterno.Click

        Dim Row As DataRow = Dt.Rows(0)

        If Row("CompDestino") <> 0 And NotaRechazoDestino = 0 Then
            MsgBox("Debe Emitir Previamente Nota de Credito. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Row("CompDestino") = 0 Then
            If Not ChequeSePuedeRechazar() Then Exit Sub
        End If

        If Dt.Rows(0).Item("TipoOrigen") = 6001 Then
            If TextEntregadoA.Text <> "" Then
                MsgBox("Cheque debe ser Previamente Reemplazado a " & TextEntregadoA.Text & ". Operación se CANCELA.", MsgBoxStyle.Critical)
                Exit Sub
            End If
        End If

        GModificacionOk = False

        Select Case Row("TipoOrigen")
            Case 60
                UnReciboDebitoCredito.PMedioPago = MedioPago
                UnReciboDebitoCredito.PTipoNota = 13005
                UnReciboDebitoCredito.PNota = 0
                UnReciboDebitoCredito.PClaveCheque = Clave
                UnReciboDebitoCredito.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCredito.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCredito.PEsRechazoCheque = True
                UnReciboDebitoCredito.PEmisor = Emisor
                UnReciboDebitoCredito.PAbierto = Abierto
                UnReciboDebitoCredito.ShowDialog()
            Case 604
                UnReciboDebitoCredito.PMedioPago = MedioPago
                UnReciboDebitoCredito.PTipoNota = 13006
                UnReciboDebitoCredito.PNota = 0
                UnReciboDebitoCredito.PClaveCheque = Clave
                UnReciboDebitoCredito.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCredito.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCredito.PEsRechazoCheque = True
                UnReciboDebitoCredito.PEmisor = Emisor
                UnReciboDebitoCredito.PAbierto = Abierto
                UnReciboDebitoCredito.ShowDialog()
            Case 6001
                Select Case Origen
                    Case 3
                        UnReciboDebitoCredito.PTipoNota = 13005
                    Case 2
                        UnReciboDebitoCredito.PTipoNota = 13006
                End Select
                UnReciboDebitoCredito.PMedioPago = MedioPago
                UnReciboDebitoCredito.PNota = 0
                UnReciboDebitoCredito.PClaveCheque = Clave
                UnReciboDebitoCredito.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCredito.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCredito.PEsRechazoCheque = True
                UnReciboDebitoCredito.PEmisor = Emisor
                UnReciboDebitoCredito.PAbierto = Abierto
                UnReciboDebitoCredito.ShowDialog()
        End Select

        If GModificacionOk Then MuestraCheque()

    End Sub
    Private Sub ButtonEmitirCredito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEmitirCredito.Click

        If Not ChequeSePuedeRechazar() Then Exit Sub

        If Dt.Rows(0).Item("TipoDestino") = 6000 Then
            If TextEntregadoA.Text <> "" Then
                MsgBox("Cheque debe ser Previamente Reemplazado a " & TextEntregadoA.Text & ". Operación se CANCELA.", MsgBoxStyle.Critical)
                Exit Sub
            End If
        End If

        Dim Row As DataRow = Dt.Rows(0)

        GModificacionOk = False

        Select Case Row("TipoDestino")
            Case 600    'Orden pago a proveedores y fondo fijo.
                If NumeroFondoFijoDestino <> 0 Then
                    UnReciboDebitoCreditoGenerica.PMedioPago = MedioPago
                    UnReciboDebitoCreditoGenerica.PTipoNota = 7007
                    UnReciboDebitoCreditoGenerica.PNota = 0
                    UnReciboDebitoCreditoGenerica.PClaveCheque = Clave
                    UnReciboDebitoCreditoGenerica.PNumeroCheque = TextNumero.Text
                    UnReciboDebitoCreditoGenerica.PImporteCheque = CDec(TextImporte.Text)
                    UnReciboDebitoCreditoGenerica.PEsRechazoCheque = True
                    UnReciboDebitoCreditoGenerica.PProveedorFondoFijo = FondoFijoDestino
                    UnReciboDebitoCreditoGenerica.PNumero = NumeroFondoFijoDestino
                    UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                    UnReciboDebitoCreditoGenerica.ShowDialog()
                Else
                    UnReciboDebitoCredito.PMedioPago = MedioPago
                    UnReciboDebitoCredito.PTipoNota = 8
                    UnReciboDebitoCredito.PNota = 0
                    UnReciboDebitoCredito.PClaveCheque = Clave
                    UnReciboDebitoCredito.PNumeroCheque = TextNumero.Text
                    UnReciboDebitoCredito.PImporteCheque = CDec(TextImporte.Text)
                    UnReciboDebitoCredito.PEsRechazoCheque = True
                    If PorCuenta = 0 Then
                        UnReciboDebitoCredito.PEmisor = EmisorDestino
                    Else
                        UnReciboDebitoCredito.PEmisor = PorCuenta
                    End If
                    UnReciboDebitoCredito.PAbierto = Abierto
                    UnReciboDebitoCredito.ShowDialog()
                End If
            Case 7001    'fondo fijo. 
                UnReciboDebitoCreditoGenerica.PMedioPago = MedioPago
                UnReciboDebitoCreditoGenerica.PTipoNota = 7007
                UnReciboDebitoCreditoGenerica.PNota = 0
                UnReciboDebitoCreditoGenerica.PClaveCheque = Clave
                UnReciboDebitoCreditoGenerica.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCreditoGenerica.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCreditoGenerica.PEsRechazoCheque = True
                UnReciboDebitoCreditoGenerica.PProveedorFondoFijo = FondoFijoDestino
                UnReciboDebitoCreditoGenerica.PNumero = NumeroFondoFijoDestino
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.ShowDialog()
            Case 1010   'cancelacion prestamo
                UnReciboDebitoCreditoGenerica.PMedioPago = MedioPago
                UnReciboDebitoCreditoGenerica.PTipoNota = 1007
                UnReciboDebitoCreditoGenerica.PNota = 0
                UnReciboDebitoCreditoGenerica.PClaveCheque = Clave
                UnReciboDebitoCreditoGenerica.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCreditoGenerica.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCreditoGenerica.PEsRechazoCheque = True
                UnReciboDebitoCreditoGenerica.PEmisorPrestamo = EmisorDestino
                UnReciboDebitoCreditoGenerica.PPrestamo = PrestamoDestino
                UnReciboDebitoCreditoGenerica.POrigenPrestamo = OrigenPrestamo
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.ShowDialog()
            Case 91    'deposito cheques terceros en bancos.
                UnReciboDebitoCreditoGenerica.PMedioPago = MedioPago
                UnReciboDebitoCreditoGenerica.PTipoNota = 93
                UnReciboDebitoCreditoGenerica.PNota = 0
                UnReciboDebitoCreditoGenerica.PClaveCheque = Clave
                UnReciboDebitoCreditoGenerica.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCreditoGenerica.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCreditoGenerica.PEsRechazoCheque = True
                UnReciboDebitoCreditoGenerica.PBanco = EmisorDestino
                UnReciboDebitoCreditoGenerica.PCuentaBancaria = CuentaBancaria
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.ShowDialog()
            Case 5010      ' Pago Otros Proveedores
                UnReciboDebitoCreditoOtrosProveedores.PMedioPago = MedioPago
                UnReciboDebitoCreditoOtrosProveedores.PTipoNota = 5007
                UnReciboDebitoCreditoOtrosProveedores.PNota = 0
                UnReciboDebitoCreditoOtrosProveedores.PClaveCheque = Clave
                UnReciboDebitoCreditoOtrosProveedores.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCreditoOtrosProveedores.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCreditoOtrosProveedores.PEsRechazoCheque = True
                UnReciboDebitoCreditoOtrosProveedores.PProveedor = EmisorDestino
                UnReciboDebitoCreditoOtrosProveedores.PAbierto = Abierto
                UnReciboDebitoCreditoOtrosProveedores.ShowDialog()
            Case 65, 64    ' 65.Devolucion seña  64.Devolucion a Cliente. 
                UnReciboDebitoCredito.PMedioPago = MedioPago
                UnReciboDebitoCredito.PTipoNota = 7
                UnReciboDebitoCredito.PNota = 0
                UnReciboDebitoCredito.PClaveCheque = Clave
                UnReciboDebitoCredito.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCredito.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCredito.PEsRechazoCheque = True
                UnReciboDebitoCredito.PEmisor = EmisorDestino
                UnReciboDebitoCredito.PAbierto = Abierto
                UnReciboDebitoCredito.ShowDialog()
            Case 4010
                '                MsgBox("Rechazo de Cheque para Sueldos No Contemplado.")
                UnReciboDebitoCreditoGenerica.PMedioPago = MedioPago
                UnReciboDebitoCreditoGenerica.PTipoNota = 4007
                UnReciboDebitoCreditoGenerica.PNota = 0
                UnReciboDebitoCreditoGenerica.PClaveCheque = Clave
                UnReciboDebitoCreditoGenerica.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCreditoGenerica.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCreditoGenerica.PEsRechazoCheque = True
                UnReciboDebitoCreditoGenerica.PLegajo = EmisorDestino
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.ShowDialog()
            Case 90
                MsgBox("Rechazo de Cheque para Retiro Bancario No Contemplado.")
        End Select

        If GModificacionOk Then MuestraCheque()

    End Sub
    Private Sub ButtonEmitirCreditoInterno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEmitirCreditoInterno.Click

        If Not ChequeSePuedeRechazar() Then Exit Sub

        If Dt.Rows(0).Item("TipoDestino") = 6000 Then
            If TextEntregadoA.Text <> "" Then
                MsgBox("Cheque debe ser Previamente Reemplazado a " & TextEntregadoA.Text & ". Operación se CANCELA.", MsgBoxStyle.Critical)
                Exit Sub
            End If
        End If

        Dim Row As DataRow = Dt.Rows(0)

        GModificacionOk = False

        Select Case Row("TipoDestino")
            Case 600    'Orden pago a proveedores y fondo fijo.
                If NumeroFondoFijoDestino <> 0 Then
                    UnReciboDebitoCreditoGenerica.PMedioPago = MedioPago
                    UnReciboDebitoCreditoGenerica.PTipoNota = 7007
                    UnReciboDebitoCreditoGenerica.PNota = 0
                    UnReciboDebitoCreditoGenerica.PClaveCheque = Clave
                    UnReciboDebitoCreditoGenerica.PNumeroCheque = TextNumero.Text
                    UnReciboDebitoCreditoGenerica.PImporteCheque = CDec(TextImporte.Text)
                    UnReciboDebitoCreditoGenerica.PEsRechazoCheque = True
                    UnReciboDebitoCreditoGenerica.PProveedorFondoFijo = FondoFijoDestino
                    UnReciboDebitoCreditoGenerica.PNumero = NumeroFondoFijoDestino
                    UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                    UnReciboDebitoCreditoGenerica.ShowDialog()
                Else
                    UnReciboDebitoCredito.PMedioPago = MedioPago
                    UnReciboDebitoCredito.PTipoNota = 13008
                    UnReciboDebitoCredito.PNota = 0
                    UnReciboDebitoCredito.PClaveCheque = Clave
                    UnReciboDebitoCredito.PNumeroCheque = TextNumero.Text
                    UnReciboDebitoCredito.PImporteCheque = CDec(TextImporte.Text)
                    UnReciboDebitoCredito.PEsRechazoCheque = True
                    If PorCuenta = 0 Then
                        UnReciboDebitoCredito.PEmisor = EmisorDestino
                    Else
                        UnReciboDebitoCredito.PEmisor = PorCuenta
                    End If
                    UnReciboDebitoCredito.PAbierto = Abierto
                    UnReciboDebitoCredito.ShowDialog()
                End If
            Case 65, 64    ' 65.Devolucion seña  64.Devolucion a Cliente. 
                UnReciboDebitoCredito.PMedioPago = MedioPago
                UnReciboDebitoCredito.PTipoNota = 13007
                UnReciboDebitoCredito.PNota = 0
                UnReciboDebitoCredito.PClaveCheque = Clave
                UnReciboDebitoCredito.PNumeroCheque = TextNumero.Text
                UnReciboDebitoCredito.PImporteCheque = CDec(TextImporte.Text)
                UnReciboDebitoCredito.PEsRechazoCheque = True
                UnReciboDebitoCredito.PEmisor = EmisorDestino
                UnReciboDebitoCredito.PAbierto = Abierto
                UnReciboDebitoCredito.ShowDialog()
            Case 90
                MsgBox("Rechazo de Cheque para Retiro Bancario No Contemplado.")
        End Select

        If GModificacionOk Then MuestraCheque()

    End Sub
    Private Sub ButtonVerDebito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerDebito.Click

        Dim Row As DataRow = Dt.Rows(0)

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GModificacionOk = False

        Select Case TipoNotaRechazoOrigen
            Case 5, 6, 13005, 13006
                UnReciboDebitoCredito.PTipoNota = TipoNotaRechazoOrigen
                UnReciboDebitoCredito.PAbierto = Abierto
                UnReciboDebitoCredito.PNota = NotaRechazoOrigen
                UnReciboDebitoCredito.ShowDialog()
            Case 7005, 1005
                UnReciboDebitoCreditoGenerica.PTipoNota = TipoNotaRechazoOrigen
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.PNota = NotaRechazoOrigen
                UnReciboDebitoCreditoGenerica.ShowDialog()
            Case 5005
                UnReciboDebitoCreditoOtrosProveedores.PTipoNota = TipoNotaRechazoOrigen
                UnReciboDebitoCreditoOtrosProveedores.PAbierto = Abierto
                UnReciboDebitoCreditoOtrosProveedores.PNota = NotaRechazoOrigen
                UnReciboDebitoCreditoOtrosProveedores.ShowDialog()
        End Select

        If GModificacionOk Then MuestraCheque()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonVerCredito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerCredito.Click

        Dim Row As DataRow = Dt.Rows(0)
        Dim DtCabeza As New DataTable

        GModificacionOk = False

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Select Case TipoNotaRechazoDestino
            Case 8, 7, 13008, 13007
                UnReciboDebitoCredito.PTipoNota = TipoNotaRechazoDestino
                UnReciboDebitoCredito.PAbierto = Abierto
                UnReciboDebitoCredito.PNota = NotaRechazoDestino
                UnReciboDebitoCredito.ShowDialog()
            Case 7007
                If NumeroFondoFijoDestino <> 0 Then
                    UnReciboDebitoCreditoGenerica.PTipoNota = 7007
                    UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                    UnReciboDebitoCreditoGenerica.PNota = NotaRechazoDestino
                    UnReciboDebitoCreditoGenerica.ShowDialog()
                End If
            Case 1007, 93
                UnReciboDebitoCreditoGenerica.PTipoNota = TipoNotaRechazoDestino
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.PNota = NotaRechazoDestino
                UnReciboDebitoCreditoGenerica.ShowDialog()
            Case 5007
                UnReciboDebitoCreditoOtrosProveedores.PTipoNota = TipoNotaRechazoDestino
                UnReciboDebitoCreditoOtrosProveedores.PAbierto = Abierto
                UnReciboDebitoCreditoOtrosProveedores.PNota = NotaRechazoDestino
                UnReciboDebitoCreditoOtrosProveedores.ShowDialog()
            Case 4007
                UnReciboDebitoCreditoGenerica.PTipoNota = TipoNotaRechazoDestino
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.PNota = NotaRechazoDestino
                UnReciboDebitoCreditoGenerica.ShowDialog()
        End Select

        If GModificacionOk Then MuestraCheque()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub TextClaveCheque_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextClaveCheque.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub PictureCandado1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado1.Click

        If Abierto Then
            PictureCandado1.Image = ImageList1.Images.Item("Cerrado")
            Abierto = False
        Else : PictureCandado1.Image = ImageList1.Images.Item("Abierto")
            Abierto = True
        End If

    End Sub
    Private Sub ButtonBuscaPorClave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBuscaPorClave.Click

        If TextNumeroCheque.Text = "" And TextClaveCheque.Text = "" Then
            MsgBox("Debe Informar Cheque Propio o Cheque Terceros.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNumeroCheque.Focus()
            Exit Sub
        End If

        If TextNumeroCheque.Text <> "" And TextClaveCheque.Text <> "" Then
            MsgBox("Debe Informar Cheque Propio o Cheque Terceros.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNumeroCheque.Focus()
            Exit Sub
        End If

        If ComboBanco.SelectedValue <> 0 And TextNumeroCheque.Text = "" Then
            MsgBox("Debe Informar Numero de Cheque Propio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNumeroCheque.Focus()
            Exit Sub
        End If

        If ComboBanco.SelectedValue = 0 And TextNumeroCheque.Text <> "" Then
            MsgBox("Debe Informar Banco del Cheque Propio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNumeroCheque.Focus()
            Exit Sub
        End If

        If TextNumeroCheque.Text <> "" Then
            If CDbl(TextNumeroCheque.Text) = 0 Then
                MsgBox("Numero Cheque Propio Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextNumeroCheque.Focus()
                Exit Sub
            End If
        End If

        If TextClaveCheque.Text <> "" Then
            If CDbl(TextClaveCheque.Text) = 0 Then
                MsgBox("Clave Cheque Terceros Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextClaveCheque.Focus()
                Exit Sub
            End If
        End If

        If TextNumeroCheque.Text <> "" Then
            BuscaPorNumero()
        Else
            BuscaPorClave()
        End If

    End Sub
    Private Sub BuscaPorNumero()

        Clave = 0

        SeleccionarVarios.PAbierto = Abierto
        SeleccionarVarios.PBanco = ComboBanco.SelectedValue
        SeleccionarVarios.PNumero = CInt(TextNumeroCheque.Text)
        SeleccionarVarios.PCaja = GCaja
        SeleccionarVarios.PEsChequeParaReemplazo = True
        SeleccionarVarios.ShowDialog()
        If SeleccionarVarios.PClave <> 0 Then
            Clave = SeleccionarVarios.PClave
        End If
        SeleccionarVarios.Dispose()

        If Clave = 0 Then
            MsgBox("Cheque No Encontrado o Caja Actual No corresponde a la Caja que lo Genero.", MsgBoxStyle.Exclamation + MsgBoxStyle.Exclamation, MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MedioPago = 2

        If Abierto Then
            ConexionCheque = Conexion
        Else : ConexionCheque = ConexionN
        End If

        MuestraCheque()

    End Sub
    Private Sub BuscaPorClave()

        If Abierto Then
            ConexionCheque = Conexion
        Else : ConexionCheque = ConexionN
        End If

        Clave = CInt(TextClaveCheque.Text)

        MedioPago = 3

        MuestraCheque()

    End Sub
    Private Sub MuestraCheque()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        TextSerie.Text = ""
        TextNumero.Text = ""
        TextImporte.Text = ""
        ComboBanco1.SelectedValue = 0
        TextEmisorCheque.Text = ""
        TextAfectado.Text = ""
        TextRecibidoDe.Text = ""
        TextEntregadoA.Text = ""
        TextACuenta.Text = ""
        TextPrestamoDestino.Text = ""
        TextTipoComprobanteDestino.Text = ""
        TextComprobanteDestino.Text = ""
        TextTipoComprobanteOrigen.Text = ""
        TextComprobanteOrigen.Text = ""
        LabelEstado.Text = ""
        ButtonVerDebito.Visible = False
        ButtonVerCredito.Visible = False
        ButtonEmitirDebito.Visible = False
        ButtonEmitirDebitoInterno.Visible = False
        ButtonEmitirCredito.Visible = False
        ButtonEmitirCreditoInterno.Visible = False
        TipoNotaRechazoOrigen = 0
        NotaRechazoOrigen = 0
        TipoNotaRechazoDestino = 0
        NotaRechazoDestino = 0
        FondoFijoDestino = 0
        NumeroFondoFijoDestino = 0
        FondoFijoOrigen = 0
        NumeroFondoFijoOrigen = 0
        PorCuenta = 0
        PrestamoDestino = 0
        PrestamoOrigen = 0
        OrigenPrestamo = 0
        CuentaBancaria = 0

        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & MedioPago & " AND ClaveCheque = " & Clave & ";", ConexionCheque, Dt) Then Me.Close() : Exit Sub
        If Dt.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Cheque No Encontrado", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Not GCajaTotal Then
            If Dt.Rows(0).Item("Caja") <> GCaja Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Cheque No Pertenece a la Caja " & GCaja, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        Dim Row As DataRow = Dt.Rows(0)

        TextSerie.Text = Row("Serie")
        TextNumero.Text = FormatNumber(Row("Numero"), 0)
        DateTime3.Value = Row("Fecha")
        TextImporte.Text = FormatNumber(Row("Importe"), GDecimales)
        ComboBanco1.SelectedValue = Row("Banco")
        TextEmisorCheque.Text = Row("EmisorCheque")
        If Row("Afectado") <> 0 Then
            TextAfectado.Text = NumeroEditado(Row("Afectado"))
        Else : TextAfectado.Text = ""
        End If
        '
        Dim DtAux As New DataTable
        If MedioPago = 2 Then
            If Not ArmaDtChequesPropios(DtAux, 999, "1/1/1000", "1/1/1000", Abierto, Not Abierto, Clave, 0, 0, False) Then Me.Close() : Exit Sub
        Else
            If Not ArmaDtChequesTerceros(DtAux, 999, "1/1/1000", "1/1/1000", Abierto, Not Abierto, True, True, Clave) Then Me.Close() : Exit Sub
        End If
        '
        If MedioPago = 3 Then
            TextRecibidoDe.Text = HallaNombreEmisor(DtAux.Rows(0).Item("Origen"), DtAux.Rows(0).Item("Emisor"))
            If TextRecibidoDe.Text = "" Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("ERROR Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            Emisor = DtAux.Rows(0).Item("Emisor")
            Origen = DtAux.Rows(0).Item("Origen")
        End If

        'Modifica Destino para encontrar el nombre del fondo fijo del destino.
        If Dt.Rows(0).Item("TipoDestino") = 600 Then
            NumeroFondoFijoDestino = HallaNumeroFondoFijoRendicion(Dt.Rows(0).Item("CompDestino"), ConexionCheque)
            If NumeroFondoFijoDestino <> 0 Then DtAux.Rows(0).Item("Destino") = 6
        End If
        If Dt.Rows(0).Item("TipoDestino") = 7001 Then
            NumeroFondoFijoDestino = HallaNumeroFondoFijoAjuste(Dt.Rows(0).Item("CompDestino"), ConexionCheque)
            If NumeroFondoFijoDestino <> 0 Then DtAux.Rows(0).Item("Destino") = 6
        End If
        '
        If DtAux.Rows(0).Item("EmisorDestino") <> 0 Then
            TextEntregadoA.Text = HallaNombreDestino(DtAux.Rows(0).Item("Destino"), DtAux.Rows(0).Item("EmisorDestino"))
            If TextEntregadoA.Text = "" Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("ERROR Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            EmisorDestino = DtAux.Rows(0).Item("EmisorDestino")
        End If

        DtAux.Dispose()

        If Row("Estado") = 3 Then LabelEstado.Text = "ANULADO"
        If Row("Estado") = 4 Then LabelEstado.Text = "RECHAZADO"

        'Muestra cartel instructivo.
        If Row("Estado") = 1 Then
            LabelCartel.Text = "Para Rechazar: Generar Notas de Debitos/Créditos al Emisor y Destinatario del Cheque."
        End If
        If Row("Estado") = 4 Then
            LabelCartel.Text = "Para Anular Rechazo: Anumar Notas de Debitos/Créditos al Emisor y Destinatario del Cheque."
        End If

        Select Case Row("TipoDestino")
            Case 600
                PorCuenta = HallaAcuenta(Row("CompDestino"), ConexionCheque)
                If PorCuenta > 0 Then TextACuenta.Text = NombreProveedor(PorCuenta)
                If PorCuenta < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Error de Base de Datos al Leer Tabla Recibos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
        End Select

        TextTipoComprobanteDestino.Text = NombreComprobante(Row("TipoDestino"))
        If Row("CompDestino") <> 0 Then TextComprobanteDestino.Text = NumeroEditado(Row("CompDestino"))
        TextTipoComprobanteOrigen.Text = NombreComprobante(Row("TipoOrigen"))
        If Row("CompOrigen") <> 0 Then TextComprobanteOrigen.Text = NumeroEditado(Row("CompOrigen"))

        Select Case Row("TipoDestino")
            Case 600, 7001
                NumeroFondoFijoDestino = HallaNumeroFondoFijoReposicion(Row("TipoDestino"), Row("CompDestino"), ConexionCheque)  'Ve si es un fondo fijo.
                If NumeroFondoFijoDestino < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    If Row("TipoDestino") = 600 Then
                        MsgBox("Error de Base de Datos al Leer Tabla RecibosCabeza. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    End If
                    If Row("TipoDestino") = 7001 Then
                        MsgBox("Error de Base de Datos al Leer Tabla MovimientosFondoFijoCabeza. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    End If
                    Exit Sub
                End If
                If NumeroFondoFijoDestino <> 0 Then FondoFijoDestino = EmisorDestino : TextNumeroFondoFijoDestino.Text = NumeroFondoFijoDestino
            Case 1010
                PrestamoDestino = HallaPrestamo(Row("CompDestino"), ConexionCheque, EmisorDestino)
                If PrestamoDestino <= 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Error Base de Datos al leer Tabla Prestamos MovimientoCabeza. Operación se CANCELA.")
                    Exit Sub
                End If
                TextPrestamoDestino.Text = NumeroEditado(PrestamoDestino)
                OrigenPrestamo = Row("TipoDestino")
            Case 91
                CuentaBancaria = HallaCuentaBancaria(Row("CompDestino"), ConexionCheque)
                If CuentaBancaria <= 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Error Base de Datos al leer Tabla MovimientosBancarioCabeza. Operación se CANCELA.")
                    Exit Sub
                End If
                TextCuenta.Text = FormatNumber(CuentaBancaria, 0)
        End Select

        If MedioPago = 3 Then
            Select Case Row("TipoOrigen")
                Case 7002
                    NumeroFondoFijoOrigen = HallaNumeroFondoFijoReposicion(Row("TipoOrigen"), Row("CompOrigen"), ConexionCheque)
                    If NumeroFondoFijoOrigen < 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        If Row("TipoOrigen") = 7002 Then
                            MsgBox("Error de Base de Datos al Leer Tabla MovimientosFondoFijoCabeza. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        End If
                        Exit Sub
                    End If
                    If NumeroFondoFijoOrigen <> 0 Then FondoFijoOrigen = Emisor : TextNumeroFondoFijoOrigen.Text = NumeroFondoFijoOrigen
                Case 1000
                    PrestamoOrigen = Dt.Rows(0).Item("CompOrigen")
                    TextPrestamoOrigen.Text = NumeroEditado(PrestamoOrigen)
                    OrigenPrestamo = Row("TipoOrigen")
                Case 1015
                    PrestamoOrigen = HallaPrestamo(Row("CompOrigen"), ConexionCheque, Emisor)
                    If PrestamoOrigen <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Error Base de Datos al leer Tabla PrestamosMovimientoCabeza. Operación se CANCELA.")
                        Exit Sub
                    End If
                    TextPrestamoOrigen.Text = NumeroEditado(PrestamoOrigen)
                    OrigenPrestamo = 1000
            End Select
        End If

        If MedioPago = 3 Then
            If Not TieneRechazo(MedioPago, Row("TipoOrigen"), Row("CompOrigen"), Clave, Origen, TipoNotaRechazoOrigen, NotaRechazoOrigen, FondoFijoOrigen) Then Exit Sub
            If NotaRechazoOrigen = 0 Then
                ButtonEmitirDebito.Visible = True
                If Abierto Then
                    Select Case Row("TipoOrigen")
                        Case 60, 604, 6001
                            ButtonEmitirDebitoInterno.Visible = True
                    End Select
                End If
            Else
                ButtonVerDebito.Visible = True
            End If
        End If

        Dim EsChequeInicial As Boolean = False
        If Row("CompDestino") = 0 Then     'Ver si esta en cheques-iniciales.
            EsChequeInicial = EsChequesIniciales(MedioPago, Clave, ConexionCheque)
        End If

        '  If Row("CompDestino") <> 0 Or EsChequeInicial Then
        If Row("CompDestino") <> 0 Or (EsChequeInicial And MedioPago = 2) Then
            If Row("TipoDestino") <> 90 Then
                If Not TieneRechazo(MedioPago, Row("TipoDestino"), Row("CompDestino"), Clave, Origen, TipoNotaRechazoDestino, NotaRechazoDestino, FondoFijoDestino) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                If NotaRechazoDestino = 0 Then
                    ButtonEmitirCredito.Visible = True
                    If Abierto Then
                        Select Case Row("TipoDestino")
                            Case 600
                                If NumeroFondoFijoDestino = 0 Then
                                    ButtonEmitirCreditoInterno.Visible = True
                                End If
                            Case 64, 65
                                ButtonEmitirCreditoInterno.Visible = True
                        End Select
                    End If
                Else
                    ButtonVerCredito.Visible = True
                End If
            Else
                ButtonEmitirCredito.Visible = True
            End If
        End If

        PanelCheque.Visible = True

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function TieneRechazo(ByVal MedioaPago As Integer, ByVal TipoComp As Integer, ByVal Comp As Double, ByVal ClaveCheque As Integer, ByVal Origen As Integer, ByRef TipoNota As Integer, ByRef Nota As Double, ByVal FondoFijo As Integer) As Boolean

        Dim DtCabeza As New DataTable

        Nota = 0
        TipoNota = 0

        Select Case TipoComp
            '------------------------------
            ' Documento que reciben cheque.
            '------------------------------
            Case 600
                If FondoFijo = 0 Then
                    If Not Tablas.Read("SELECT TipoNota,Nota FROM RecibosCabeza WHERE Estado = 1 AND NotaAnulacion = 0 AND (TipoNota = 8 or tiponota = 13008) AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                    If DtCabeza.Rows.Count <> 0 Then
                        TipoNota = DtCabeza.Rows(0).Item("TipoNota")
                        Nota = DtCabeza.Rows(0).Item("Nota")
                    End If
                Else
                    If Not Tablas.Read("SELECT Movimiento FROM MovimientosFondoFijoCabeza WHERE Estado = 1 AND Tipo = 7007 AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                    If DtCabeza.Rows.Count <> 0 Then
                        TipoNota = 7007
                        Nota = DtCabeza.Rows(0).Item("Movimiento")
                    End If
                End If
            Case 65                                        ' devolucion senia.
                If Not Tablas.Read("SELECT TipoNota,Nota FROM RecibosCabeza WHERE Estado = 1 AND NotaAnulacion = 0 AND (TipoNota = 7 or TipoNota = 13007) AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                If DtCabeza.Rows.Count = 0 Then
                    Return True
                End If
                TipoNota = DtCabeza.Rows(0).Item("TipoNota")
                Nota = DtCabeza.Rows(0).Item("Nota")
            Case 64                                         ' devolucion a cliente.
                If Not Tablas.Read("SELECT TipoNota,Nota FROM RecibosCabeza WHERE Estado = 1 AND NotaAnulacion = 0 AND (TipoNota = 7 or TipoNota = 13007) AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                If DtCabeza.Rows.Count = 0 Then
                    Return True
                End If
                TipoNota = DtCabeza.Rows(0).Item("TipoNota")
                UnRecibo.PAbierto = Abierto
                Nota = DtCabeza.Rows(0).Item("Nota")
            Case 1010
                If Not Tablas.Read("SELECT Movimiento FROM PrestamosMovimientoCabeza WHERE Estado = 1 AND TipoNota = 1007 AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                If DtCabeza.Rows.Count <> 0 Then
                    TipoNota = 1007
                    Nota = DtCabeza.Rows(0).Item("Movimiento")
                End If
            Case 4010
                If Not Tablas.Read("SELECT Movimiento FROM SueldosMovimientoCabeza WHERE Estado = 1 AND TipoNota = 4007 AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                If DtCabeza.Rows.Count = 0 Then
                    Return True
                End If
                TipoNota = 4007
                Nota = DtCabeza.Rows(0).Item("Movimiento")
            Case 5010
                If Not Tablas.Read("SELECT Movimiento FROM OtrosPagosCabeza WHERE Estado = 1 AND TipoNota = 5007 AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                If DtCabeza.Rows.Count = 0 Then
                    Return True
                End If
                TipoNota = 5007
                Nota = DtCabeza.Rows(0).Item("Movimiento")
            Case 91
                If Not Tablas.Read("SELECT Movimiento FROM MovimientosBancarioCabeza WHERE Estado = 1 AND TipoNota = 93 AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                If DtCabeza.Rows.Count = 0 Then
                    Return True
                End If
                TipoNota = 93
                Nota = DtCabeza.Rows(0).Item("Movimiento")
            Case 7001
                If Not Tablas.Read("SELECT Movimiento FROM MovimientosFondoFijoCabeza WHERE Tipo = 7007 AND Estado = 1 AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                If DtCabeza.Rows.Count <> 0 Then
                    TipoNota = 7007
                    Nota = DtCabeza.Rows(0).Item("Movimiento")
                End If
                '-----------------------------
                ' Documentos que emiten cheque.
                '-----------------------------
            Case 60
                If Not Tablas.Read("SELECT TipoNota,Nota FROM RecibosCabeza WHERE Estado = 1 AND NotaAnulacion = 0 AND (TipoNota = 5 or TipoNota = 13005) AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                If DtCabeza.Rows.Count <> 0 Then
                    TipoNota = DtCabeza.Rows(0).Item("TipoNota")
                    Nota = DtCabeza.Rows(0).Item("Nota")
                End If
            Case 6001
                Dim Str As String
                Select Case Origen
                    Case 2
                        Str = "(TipoNota = 6 OR TipoNota = 13006)"
                    Case 3
                        Str = "(TipoNota = 5 OR TipoNota = 13005)"
                End Select
                If Not Tablas.Read("SELECT TipoNota,Nota FROM RecibosCabeza WHERE Estado = 1 AND NotaAnulacion = 0 AND " & Str & " AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                If DtCabeza.Rows.Count = 0 Then
                    Return True
                End If
                TipoNota = DtCabeza.Rows(0).Item("TipoNota")
                Nota = DtCabeza.Rows(0).Item("Nota")
            Case 1000, 1015
                If Not Tablas.Read("SELECT Movimiento FROM PrestamosMovimientoCabeza WHERE Estado = 1 AND TipoNota = 1005 AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                If DtCabeza.Rows.Count = 0 Then
                    Return True
                End If
                TipoNota = 1005
                Nota = DtCabeza.Rows(0).Item("Movimiento")
            Case 7002
                If Not Tablas.Read("SELECT Movimiento FROM MovimientosFondoFijoCabeza WHERE Tipo = 7005 AND Estado = 1 AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                If DtCabeza.Rows.Count <> 0 Then
                    TipoNota = 7005
                    Nota = DtCabeza.Rows(0).Item("Movimiento")
                End If
            Case 604
                If Not Tablas.Read("SELECT TipoNota,Nota FROM RecibosCabeza WHERE Estado = 1 AND NotaAnulacion = 0 AND (TipoNota = 6 OR TipoNota = 13006) AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                If DtCabeza.Rows.Count = 0 Then
                    Return True
                End If
                TipoNota = DtCabeza.Rows(0).Item("TipoNota")
                Nota = DtCabeza.Rows(0).Item("Nota")
            Case 5020
                If Not Tablas.Read("SELECT Movimiento FROM OtrosPagosCabeza WHERE Estado = 1 AND TipoNota = 5005 AND ChequeRechazado = " & ClaveCheque & " AND MedioPagoRechazado = " & MedioPago & ";", ConexionCheque, DtCabeza) Then Exit Function
                If DtCabeza.Rows.Count = 0 Then
                    Return True
                End If
                TipoNota = 5005
                Nota = DtCabeza.Rows(0).Item("Movimiento")
            Case Else
                MsgBox("Tipo Nota " & TipoComp & " No Considerada.")
                Exit Function
        End Select

        Return True

    End Function
    Private Function HallaPrestamo(ByVal Comprobante As Integer, ByVal ConexionStr As String, ByVal Emisor As Integer) As Double

        If Comprobante = 0 Then
            Return HallaPrestamoManual(ConexionStr, Emisor)
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Prestamo FROM PrestamosMovimientoCabeza WHERE Movimiento = " & Comprobante & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaNumeroFondoFijo(ByVal Comprobante As Integer, ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Numero FROM MovimientosFondoFijoCabeza WHERE Movimiento = " & Comprobante & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaNumeroFondoFijoReposicion(ByVal TipoComprobante As Integer, ByVal Comprobante As Decimal, ByVal ConexionStr As String) As Double

        Select Case TipoComprobante
            Case 600
                Try
                    Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                        Miconexion.Open()
                        Using Cmd As New OleDb.OleDbCommand("SELECT NumeroFondoFijo FROM Reciboscabeza WHERE TipoNota = 600 AND Nota = " & Comprobante & ";", Miconexion)
                            Return Cmd.ExecuteScalar()
                        End Using
                    End Using
                Catch ex As Exception
                    Return -1
                End Try
            Case 7001
                Try
                    Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                        Miconexion.Open()
                        Using Cmd As New OleDb.OleDbCommand("SELECT Numero FROM MovimientosFondoFijoCabeza WHERE Tipo = 7001 AND Movimiento = " & Comprobante & ";", Miconexion)
                            Return Cmd.ExecuteScalar()
                        End Using
                    End Using
                Catch ex As Exception
                    Return -1
                End Try
            Case 7002
                Try
                    Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                        Miconexion.Open()
                        Using Cmd As New OleDb.OleDbCommand("SELECT Numero FROM MovimientosFondoFijoCabeza WHERE Tipo = 7002 AND Movimiento = " & Comprobante & ";", Miconexion)
                            Return Cmd.ExecuteScalar()
                        End Using
                    End Using
                Catch ex As Exception
                    Return -1
                End Try
        End Select

    End Function
    Private Function HallaAcuenta(ByVal Comprobante As Integer, ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ACuenta FROM RecibosCabeza WHERE TipoNota = 600 AND Nota = " & Comprobante & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaCuentaBancaria(ByVal Comprobante As Decimal, ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cuenta FROM MovimientosBancarioCabeza WHERE Movimiento = " & Comprobante & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function ChequeSePuedeRechazar() As Boolean

        If LabelEstado.Text <> "" Then
            MsgBox("Cheque No Se Puede Rechazar. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If TextAfectado.Text <> "" Then
            MsgBox("Cheque Esta Afectado a Otra Orden. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If ExiteChequeEnPaseCaja(ConexionCheque, Dt.Rows(0).Item("ClaveCheque")) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Cheque " & Dt.Rows(0).Item("ClaveCheque") & " en Proceso de Pase de Caja. Operación se CANCELA.")
            Exit Function
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Function EsChequesIniciales(ByVal MedioPago As Integer, ByVal ClaveCheque As Integer, ByVal ConexionStr As String) As Boolean

        Dim Esta As Boolean
        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT * FROM ChequesIniciales WHERE MedioPago = " & MedioPago & " AND ClaveCheque = " & ClaveCheque & ";", ConexionStr, Dt) Then End
        If Dt.Rows.Count <> 0 Then Esta = True

        Dt.Dispose()
        Return Esta

    End Function
    Public Function NombreComprobante(ByVal Tipo As Integer) As String

        Select Case Tipo
            Case 60
                Return "Cobranza"
            Case 64
                Return "Devol. al Cliente"
            Case 65
                Return "Devol. Seña"
            Case 604
                Return "Devol. del Proveedor"
            Case 6001
                Return "Venta Divisas"
            Case 600
                Return "Orden Pago"
            Case 7001
                Return "Aumento F.Fijo"
            Case 1010
                Return "Cancelación Prestamo"
            Case 91
                Return "Depósito Bancario"
            Case 90
                Return "Extracción Bancaria"
            Case 7002
                Return "Disminuye F.Fijo"
            Case 1000
                Return "Prestamo"
            Case 1015
                Return "Ajuste Capital Prest."
            Case 5010
                Return "Otros Pagos"
            Case 5020
                Return "Devol. Otros Prov."
            Case Else
                Return ""
        End Select

    End Function
   
  
End Class