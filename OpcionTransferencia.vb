Public Class OpcionTransferencia
    Public PEsInsumo As Boolean
    Public POrigen As Integer
    Public PDestino As Integer
    Public PAbierto As Boolean
    Public PEsDescarte As Boolean
    Private Sub OpcionTransferencia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If PEsInsumo Then
            LlenaComboTablas(ComboDepositoOrigen, 20)
            ComboDepositoOrigen.SelectedValue = 0
            LlenaComboTablas(ComboDepositoDestino, 20)
            ComboDepositoDestino.SelectedValue = 0
            Me.BackColor = Color.Thistle
        Else
            LlenaComboTablas(ComboDepositoOrigen, 19)
            ComboDepositoOrigen.SelectedValue = 0
            LlenaComboTablas(ComboDepositoDestino, 19)
            ComboDepositoDestino.SelectedValue = 0
        End If

        With ComboDepositoOrigen
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        With ComboDepositoDestino
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        POrigen = 0
        PDestino = 0

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If
        PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
        PAbierto = True

        If PEsInsumo Then
            PictureCandado.Visible = False
        End If

        If PEsDescarte Then
            LabelOrigen.Text = "Deposito"
            LabelDestino.Visible = False
            ComboDepositoDestino.Visible = False
        End If

    End Sub
    Private Sub ComboDepositoOrigen_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDepositoOrigen.Validating

        If IsNothing(ComboDepositoOrigen.SelectedValue) Then ComboDepositoOrigen.SelectedValue = 0

    End Sub
    Private Sub ComboDepositoDestino_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDepositoDestino.Validating

        If IsNothing(ComboDepositoDestino.SelectedValue) Then ComboDepositoDestino.SelectedValue = 0

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

        If Not Valida() Then Exit Sub

        POrigen = ComboDepositoOrigen.SelectedValue
        PDestino = ComboDepositoDestino.SelectedValue

        Me.Close()

    End Sub
    Private Function Valida() As Boolean

        If ComboDepositoOrigen.SelectedValue = 0 Then
            MsgBox("Falta Deposito Origen.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDepositoOrigen.Focus()
            Return False
        End If

        If Not PEsDescarte Then
            If ComboDepositoDestino.SelectedValue = 0 Then
                MsgBox("Falta Deposito Destino.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboDepositoDestino.Focus()
                Return False
            End If

            If ComboDepositoOrigen.SelectedValue = ComboDepositoDestino.SelectedValue Then
                MsgBox("Deposito Origen y Destino no deben ser los mismos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboDepositoDestino.Focus()
                Return False
            End If
        End If

        Return True

    End Function
End Class