Public Class OpcionReciboFactura
    Public PContado As Boolean
    Public PMixto As Boolean
    Public PRegresar As Boolean
    Private Sub OpcionReciboFactura_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        PRegresar = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not RadioContadoEfectivo.Checked And Not RadioMixto.Checked Then
            MsgBox("Debe Informar Forma de Pago.", MsgBoxStyle.Information)
            Exit Sub
        End If
        If RadioContadoEfectivo.Checked And RadioMixto.Checked Then
            MsgBox("Debe Informar una sola Forma de Pago.", MsgBoxStyle.Information)
            Exit Sub
        End If

        PContado = RadioContadoEfectivo.Checked
        PMixto = RadioMixto.Checked
        PRegresar = False

        Me.Close()

    End Sub

    
End Class