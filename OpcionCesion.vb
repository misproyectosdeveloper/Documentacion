Public Class OpcionCesion
    Public PTipoDestino As Integer
    Public PDestino As Integer
    Public PFechaDesde As Date
    Public PFechaHasta As Date
    Public PSoloPendientes As Boolean
    Public PRegresar As Boolean = True
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateFechaDesde.Value, DateFechaHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If RadioProveedor.Checked Then
            ptipodestino = 1
        Else
            If RadioCliente.Checked Then
                ptipodestino = 2
            Else
                If RadioBanco.Checked Then
                    ptipodestino = 3
                End If
            End If
        End If

        PFechaDesde = DateFechaDesde.Value
        PFechaHasta = DateFechaHasta.Value

        PRegresar = False
        Me.Close()

    End Sub
    Private Sub RadioBanco_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles RadioBanco.Validating

        ComboDestino.DataSource = Nothing
        ComboDestino.Text = ""
        LlenaComboTablas(ComboDestino, 26)
        ComboDestino.SelectedValue = 0

    End Sub
    Private Sub RadioCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles RadioCliente.Validating

        ComboDestino.DataSource = Nothing
        ComboDestino.Text = ""
        LlenaCombo(ComboDestino, "", "Clientes")
        ComboDestino.SelectedValue = 0

    End Sub
    Private Sub RadioProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles RadioProveedor.Validating

        ComboDestino.DataSource = Nothing
        ComboDestino.Text = ""
        LlenaCombo(ComboDestino, "", "Proveedores")
        ComboDestino.SelectedValue = 0

    End Sub

    
End Class