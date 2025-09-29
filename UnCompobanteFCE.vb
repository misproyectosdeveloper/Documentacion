Imports System.Math
Public Class UnCompobanteFCE
    Public PEsFactura As Boolean
    Public PEsDebitoCredito As Boolean
    Public PTipoNota As Integer
    Public PCliente As Integer
    Public PCBU As String
    Public PMinimo As Decimal
    Public PAlias As String
    Public PEsFCE As Boolean
    Public PFacturaFCE As Decimal
    Public POpcionAgente As String
    Public PRegresar As Boolean = True
    '
    Dim TipoFactura As Integer
    Private Sub UnCompobanteFCE_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        PFacturaFCE = 0
        PMinimo = 0
        PEsFCE = False

        MaskedFactura.Text = "000000000000"
        Panel2.Visible = False
        POpcionAgente = ""

        TipoFactura = LetrasPermitidasCliente(HallaTipoIvaCliente(PCliente), 1)
        TextLetra.Text = LetraTipoIva(TipoFactura)

        If PEsFactura Then
            Dim AliasW As String
            HallaDatosFCE(PCBU, AliasW, PMinimo)
            Label3.Text = Label3.Text & " " & FormatNumber(PMinimo) & "$"
        Else
            Panel5.Visible = True
            Label3.Visible = False
        End If

    End Sub
    Private Sub CheckSI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckSI.Click

        If PEsFactura Then Panel2.Visible = True
        CheckNO.Checked = False

    End Sub
    Private Sub CheckNO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckNO.Click

        If PEsFactura Then Panel2.Visible = False
        CheckSI.Checked = False

    End Sub
    Private Sub TextLetra_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextLetra.KeyPress

        If InStr("abcABC" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If CheckSI.Checked = False And CheckNO.Checked = False Then
            MsgBox("Debe Indicar si es FCE.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If CheckSI.Checked Then
            If PEsFactura Then If Not ValidaSiEsFactura() Then Exit Sub
            If PEsDebitoCredito Then
                If Not ValidaSiEsDebitoCredito() Then Exit Sub
                PFacturaFCE = HallaNumeroFactura()
            End If
            If RadioADC.Checked Then POpcionAgente = "ADC"
            If RadioSCA.Checked Then POpcionAgente = "SCA"
            PEsFCE = True : PRegresar = False : Me.Close()
        End If

        If CheckNO.Checked Then
            POpcionAgente = ""
            PEsFCE = False : PRegresar = False : Me.Close()
        End If

    End Sub
    Private Function ValidaSiEsFactura() As Boolean

        If PCBU = "" Then
            MsgBox("Falta Informar CBU." + vbCrLf + "Informarlo en Menu-->Datos de la Empresa-->CBU.", MsgBoxStyle.Critical)
            Exit Function
        End If

        Return True

    End Function
    Private Function ValidaSiEsDebitoCredito() As Boolean

        Dim Respuesta As Decimal = ValidaFacturaFCE(TextLetra.Text, MaskedFactura)
        If Respuesta = -1000 Then Exit Function
        UnSaldoFCE.PFactura = Respuesta
        UnSaldoFCE.PEsAnulacion = False
        UnSaldoFCE.PMuestraFormulario = False
        UnSaldoFCE.ShowDialog()
        If UnSaldoFCE.PSaldo = 0 And ptiponota = 7 Then
            MsgBox("Factura FCE Esta ANULADA.") : UnSaldoFCE.Dispose() : Exit Function
        Else
            UnSaldoFCE.Dispose()
        End If

        Return True

    End Function
    Private Function ValidaFacturaFCE(ByVal Letra As String, ByVal Mask As MaskedTextBox) As Decimal

        If Letra = "" Then
            MsgBox("Debe Informar Letra.", MsgBoxStyle.Critical) : Return -1000
        End If

        If Not MaskedOK2(Mask) Then
            MsgBox("Numero Factura Erroneo.", MsgBoxStyle.Critical) : Return -1000
        End If

        Dim Factura As Decimal = HallaNumeroFactura()

        Dim dt As New DataTable
        If Not Tablas.Read("SELECT EsFCE FROM FacturasCabeza WHERE Factura = " & Factura & " AND Cliente = " & PCliente & ";", Conexion, dt) Then Return -1000
        If dt.Rows.Count = 0 Then
            MsgBox("Factura FCE No Existe.", MsgBoxStyle.Information)
            Return -1000
        Else
            If Not dt.Rows(0).Item("EsFCE") Then
                MsgBox("Factura no es de Crédito.", MsgBoxStyle.Information)
                Return -1000
            End If
        End If

        Return Factura

        dt.Dispose()

    End Function
    Private Function HallaNumeroFactura() As Decimal

        Return TipoFactura & MaskedFactura.Text

    End Function
    
End Class