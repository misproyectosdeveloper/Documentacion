Public Class OpcionInformeReprocesos
    Public PEspecie As Integer
    Public PVariedad As Integer
    Public PEnvase As Integer
    Public PEspecie2 As Integer
    Public PVariedad2 As Integer
    Public PEnvase2 As Integer
    Public PDesde As Date
    Public PHasta As Date
    Public PEstado As Integer
    Public PAbierto As Boolean
    Public PCerrado As Boolean
    Public PDeposito As Integer
    Public PProveedor As Integer
    Public PRegresar As Boolean
    Private Sub OpcionVentas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboProveedor.DataSource = ProveedoresDeFrutas()
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = ProveedoresDeFrutasAlias()
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboEspecie, 1)
        With ComboEspecie
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboEspecie.SelectedValue = 0

        LlenaComboTablas(ComboEspecie2, 1)
        With ComboEspecie2
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboEspecie2.SelectedValue = 0

        LLenaComboEnvases(ComboEnvase)
        With ComboEnvase
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboEnvase.SelectedValue = 0

        LLenaComboEnvases(ComboEnvase2)
        With ComboEnvase2
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboEnvase2.SelectedValue = 0

        LlenaComboTablas(ComboVariedad, 2)
        With ComboVariedad
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboVariedad.SelectedValue = 0

        LlenaComboTablas(ComboVariedad2, 2)
        With ComboVariedad2
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboVariedad2.SelectedValue = 0

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not PermisoTotal Then Panel1.Visible = False

        PRegresar = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            MsgBox("Debe Informar Candado Abierto o Cerrado.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboProveedor.SelectedValue <> 0 Then
            PProveedor = ComboProveedor.SelectedValue
        Else
            PProveedor = ComboAlias.SelectedValue
        End If
        PEspecie = ComboEspecie.SelectedValue
        PVariedad = ComboVariedad.SelectedValue
        PEnvase = ComboEnvase.SelectedValue
        PEspecie2 = ComboEspecie2.SelectedValue
        PVariedad2 = ComboVariedad2.SelectedValue
        PEnvase2 = ComboEnvase2.SelectedValue
        PEstado = ComboEstado.SelectedValue
        PDeposito = ComboDeposito.SelectedValue
        PDesde = DateTimeDesde.Value
        PHasta = DateTimeHasta.Value
        PAbierto = CheckAbierto.Checked
        PCerrado = CheckCerrado.Checked

        PRegresar = False
        Me.Close()

    End Sub
    Private Sub ComboEspecie_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEspecie.Validating

        If IsNothing(ComboEspecie.SelectedValue) Then ComboEspecie.SelectedValue = 0

    End Sub
    Private Sub ComboVariedad_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboVariedad.Validating

        If IsNothing(ComboVariedad.SelectedValue) Then ComboVariedad.SelectedValue = 0

    End Sub
    Private Sub ComboEnvase_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEnvase.Validating

        If IsNothing(ComboEnvase.SelectedValue) Then ComboEnvase.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub


End Class
