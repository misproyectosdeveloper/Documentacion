Public Class OpcionOtrosProveedores
    Public PEsDebitoCredito As Boolean
    Public PProveedor As Integer
    Public PTipoPago As Integer
    Public PAbierto As Boolean
    Private Sub OpcionOtrosProveedores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM OtrosProveedores WHERE Estado = 1;")
        Dim Row As DataRow = ComboEmisor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboEmisor.DataSource.rows.add(Row)
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = 0

        ComboTipoPago.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 39;")
        Row = ComboTipoPago.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboTipoPago.DataSource.rows.add(Row)
        ComboTipoPago.DisplayMember = "Nombre"
        ComboTipoPago.ValueMember = "Clave"
        ComboTipoPago.SelectedValue = 0

        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        With ComboTipoPago
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
        PAbierto = True

        If Not PermisoTotal Then
            PictureCandado.Visible = False
        End If

        If PEsDebitoCredito Then ComboTipoPago.Visible = False : Label2.Visible = False

        PProveedor = 0
        PTipoPago = 0

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        PProveedor = ComboEmisor.SelectedValue
        PTipoPago = ComboTipoPago.SelectedValue

        Me.Close()

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

    End Sub
    Private Sub ComboTipoPago_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipoPago.Validating

        If IsNothing(ComboTipoPago.SelectedValue) Then ComboTipoPago.SelectedValue = 0

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = UnRemito.ImageList1.Images.Item("Cerrado")
            PAbierto = False
        Else : PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

    End Sub
    Private Function Valida() As Boolean

        If ComboEmisor.SelectedValue = 0 Then
            MsgBox("Debe Seleccionar Otro Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If Not PEsDebitoCredito Then
            If ComboTipoPago.SelectedValue = 0 Then
                MsgBox("Debe Seleccionar Un Tipo pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboTipoPago.Focus()
                Return False
            End If
        End If

        Return True

    End Function
End Class