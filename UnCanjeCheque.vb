Imports System.Transactions
Public Class UnCanjeCheque
    Dim Dt As DataTable
    '
    Dim ConexionCheque As String
    Dim Abierto As Boolean
    Dim Clave As Integer
    Dim MedioPago As Integer
    Dim Emisor As Integer
    Dim EmisorDestino As Integer
    Dim OrigenDestino As Integer
    Dim NumeroFondoFijo As Integer
    Dim cb As ComboBox
    Private Sub UnCanjeCheque_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LlenaComboTablas(ComboBanco, 26)
        ComboBanco.SelectedValue = 0
        With ComboBanco
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        PictureCandado.Image = ImageList1.Images.Item("Abierto")
        Abierto = True
        If Not PermisoTotal Then PictureCandado.Visible = False : PanelAfectado.Visible = False

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then
            If TextNumeroCheque.Focused Then ButtonBuscar_Click(Nothing, Nothing)
        End If

    End Sub
    Private Sub TextNumeroCheque_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumeroCheque.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextClaveCheque_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextClaveCheque.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If Abierto Then
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            Abierto = False
        Else : PictureCandado.Image = ImageList1.Images.Item("Abierto")
            Abierto = True
        End If

    End Sub
    Private Sub ButtonBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBuscar.Click

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
            ButtonBuscaPorNumero()
        Else
            ButtonBuscaPorClave()
        End If

    End Sub
    Private Sub ButtonBuscaPorNumero()

        PanelCheque.Visible = False

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
    Private Sub ButtonBuscaPorClave()

        PanelCheque.Visible = False

        If Abierto Then
            ConexionCheque = Conexion
        Else : ConexionCheque = ConexionN
        End If

        Clave = CInt(TextClaveCheque.Text)

        MedioPago = 3

        MuestraCheque()

    End Sub
    Private Sub ButtonReemplazar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReemplazar.Click

        Select Case Dt.Rows(0).Item("TipoDestino")
            Case 600, 1010, 65, 4010, 5010, 6000, 7001, 64
            Case Else
                MsgBox("Reemplazo de Cheque Solo Valido para los utilizados en Ordenes de Pago, Cancelacion de Prestamos, Devolución Seña, Compra Divisa,Aumento Fondo Fijo. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
        End Select

        If (Dt.Rows(0).Item("TipoDestino") = 600 Or Dt.Rows(0).Item("TipoDestino") = 65) And EsExterior(Dt.Rows(0).Item("CompDestino")) Then
            MsgBox("Reemplazo NO Valido para Orden Pago Exterior. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If LabelEstado.Text <> "" Then
            MsgBox("Cheque No Se Puede Reemplazar. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If TextAfectado.Text <> "" Then
            MsgBox("Cheque Esta Afectado a Otra Orden. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Dt.Rows(0).Item("FechaDeposito") <> "1/1/1800" Then
            MsgBox("Cheque Fue Depositado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Dt.Rows(0).Item("TipoDestino") = 90 Then
            MsgBox("Cheque Propio Depositado en Banco No Se Puede Anular. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Select Case Dt.Rows(0).Item("TipoDestino")
            Case 600, 1010, 65, 4010, 5010, 6000, 7001, 64
                UnChequeReemplazo.PNota = 0
                UnChequeReemplazo.PAbierto = Abierto
                UnChequeReemplazo.PTipoNota = Dt.Rows(0).Item("TipoDestino")
                UnChequeReemplazo.PEmisor = EmisorDestino
                UnChequeReemplazo.PMedioPagoAReemplazar = MedioPago
                UnChequeReemplazo.PClaveChequeAReemplazar = Dt.Rows(0).Item("ClaveCheque")
                UnChequeReemplazo.POrigenDestino = OrigenDestino
                UnChequeReemplazo.PCompDestino = Dt.Rows(0).Item("CompDestino")
                UnChequeReemplazo.PNumeroFondoFijo = NumeroFondoFijo
                UnChequeReemplazo.ShowDialog()
                UnChequeReemplazo.Dispose()
        End Select
        If GModificacionOk Then ButtonBuscar_Click(Nothing, Nothing)

    End Sub
    Private Sub MuestraCheque()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

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
        TextCuenta.Text = FormatNumber(Row("Cuenta"), 0)
        DateTime3.Value = Row("Fecha")
        TextImporte.Text = FormatNumber(Row("Importe"), GDecimales)
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
        TextRecibidoDe.Text = ""
        TextEntregadoA.Text = ""

        If MedioPago = 3 Then
            TextRecibidoDe.Text = HallaNombreEmisor(DtAux.Rows(0).Item("Origen"), DtAux.Rows(0).Item("Emisor"))
            If TextRecibidoDe.Text = "" Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("ERROR Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            Emisor = DtAux.Rows(0).Item("Emisor")
        End If

        NumeroFondoFijo = 0
        If Dt.Rows(0).Item("TipoDestino") = 600 Then
            NumeroFondoFijo = HallaNumeroFondoFijoRendicion(Dt.Rows(0).Item("CompDestino"), ConexionCheque)
            If NumeroFondoFijo <> 0 Then DtAux.Rows(0).Item("Destino") = 6
        End If
        If Dt.Rows(0).Item("TipoDestino") = 7001 Then
            NumeroFondoFijo = HallaNumeroFondoFijoAjuste(Dt.Rows(0).Item("CompDestino"), ConexionCheque)
            If NumeroFondoFijo <> 0 Then DtAux.Rows(0).Item("Destino") = 6
        End If

        If DtAux.Rows(0).Item("EmisorDestino") <> 0 Then
            TextEntregadoA.Text = HallaNombreDestino(DtAux.Rows(0).Item("Destino"), DtAux.Rows(0).Item("EmisorDestino"))
            If TextEntregadoA.Text = "" Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("ERROR Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            OrigenDestino = DtAux.Rows(0).Item("Destino")
            EmisorDestino = DtAux.Rows(0).Item("EmisorDestino")
        End If

        DtAux.Dispose()

        LabelEstado.Text = ""

        If Row("Estado") = 3 Then LabelEstado.Text = "ANULADO"
        If Row("Estado") = 4 Then LabelEstado.Text = "RECHAZADO"

        PanelCheque.Visible = True

        ButtonReemplazar.Visible = False

        If Row("Estado") = 4 Or Row("Estado") = 3 Then
            ButtonReemplazar.Visible = False
        End If
        If Row("Estado") = 1 Then
            ButtonReemplazar.Visible = True
        End If
        If Row("TipoDestino") = 0 Then
            ButtonReemplazar.Visible = False
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function EsExterior(ByVal Nota As Double) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionCheque)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT EsExterior FROM RecibosCabeza WHERE TipoNota = 600 AND Nota = " & Nota & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer RecibosCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    

End Class