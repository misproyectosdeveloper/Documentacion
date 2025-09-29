Public Class OpcionPaseCaja
    Public PAbierto As Boolean
    Private Sub OpcionPaseCaja_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboCaja.DataSource = ArmaDtCajas()
        ComboCaja.DisplayMember = "Nombre"
        ComboCaja.ValueMember = "Clave"
        ComboCaja.SelectedValue = 0
        With ComboCaja
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        PictureCandado.Image = ImageList1.Images.Item("Abierto")
        PAbierto = True
        If Not PermisoTotal Then PictureCandado.Visible = False

    End Sub
    Private Sub ComboCaja_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCaja.Validating

        If IsNothing(ComboCaja.SelectedValue) Then ComboCaja.SelectedValue = 0

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = UnRemito.ImageList1.Images.Item("Cerrado")
            PAbierto = False
        Else : PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If ComboCaja.SelectedValue = 0 Then
            MsgBox("Debe Informat una Caja Destino.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            ComboCaja.SelectedValue = 0
            ComboCaja.Focus()
            Exit Sub
        End If
        If ComboCaja.SelectedValue = GCaja Then
            MsgBox("No Puede hacer un Pase sobre la misma Caja.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            ComboCaja.SelectedValue = 0
            ComboCaja.Focus()
            Exit Sub
        End If

        Me.Close()

    End Sub
End Class