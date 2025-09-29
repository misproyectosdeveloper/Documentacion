Public Class OpcionTipoNota
    Public PTipoEmisor As Integer
    Public PEsPrestamo As Boolean
    Public PAbierto As Boolean
    Public PIndice As Integer
    Public Ptipo As Integer
    Private Sub OpcionTipoRecibo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Ptipo = 0

        If PEsPrestamo Then
            ArmaTiposPrestamo(ComboTipo)
            ComboTipo.SelectedValue = 0
            ComboTipo.DroppedDown = True
            PictureCandado.Visible = False
            Exit Sub
        End If

        If PTipoEmisor = 1 Then
            ArmaTiposCliente(ComboTipo)
        End If
        If PTipoEmisor = 2 Then
            ArmaTiposProveedor(ComboTipo)
        End If
        ComboTipo.SelectedValue = 0
        ComboTipo.DroppedDown = True

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        PictureCandado.Image = ImageList1.Images.Item("Abierto")
        PAbierto = True

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            PAbierto = False
        Else : PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Ptipo = ComboTipo.SelectedValue

        Me.Close()

    End Sub

End Class