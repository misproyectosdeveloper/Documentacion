Public Class OpcionIFCO
    Public PCliente As Integer
    Public PDeposito As Integer
    Public PSucursal As Integer
    Public PAbierto As Boolean
    Public PRegresar As Boolean
    Private Sub OpcionIFCO_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LlenaCombo(ComboEmisor, "", "Clientes")
        ComboEmisor.SelectedValue = PCliente

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Clientes WHERE Alias <> '';")
        Dim Row As DataRow = ComboAlias.DataSource.NewRow
        Row("Clave") = 0
        Row("Alias") = ""
        ComboAlias.DataSource.rows.add(Row)
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0

        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboDeposito, 20)
        ComboDeposito.SelectedValue = PDeposito
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PCliente = 0 Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
        Else
            PictureCandado_Click(Nothing, Nothing)
        End If
        PRegresar = True

        If Not PermisoTotal Then PictureCandado.Visible = False

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        VerSucursales(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        VerSucursales(ComboAlias.SelectedValue)

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub ComboSucursales_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboSucursales.Validating

        If IsNothing(ComboSucursales.SelectedValue) Then ComboSucursales.SelectedValue = 0

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

        If Not Valida() Then Exit Sub

        If ComboEmisor.SelectedValue <> 0 Then PCliente = ComboEmisor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then PCliente = ComboAlias.SelectedValue
        PDeposito = ComboDeposito.SelectedValue
        PSucursal = ComboSucursales.SelectedValue

        PRegresar = False

        Me.Close()

    End Sub
    Private Sub VerSucursales(ByVal Emisor As Integer)

        If Emisor = 0 Then
            ComboSucursales.DataSource = Nothing
            Exit Sub
        End If

        Dim Sql As String

        Sql = "SELECT Clave,Nombre FROM SucursalesClientes WHERE Estado = 1 AND Cliente = " & Emisor & ";"
        ComboSucursales.DataSource = New DataTable
        ComboSucursales.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboSucursales.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboSucursales.DataSource.rows.add(Row)
        ComboSucursales.DisplayMember = "Nombre"
        ComboSucursales.ValueMember = "Clave"
        ComboSucursales.SelectedValue = 0
        With ComboSucursales
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Private Function Valida() As Boolean

        If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue = 0 Then
            MsgBox("Falta Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Falta Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Return False
        End If

        If ComboSucursales.SelectedValue = 0 And ComboSucursales.Items.Count > 1 Then
            MsgBox("Falta Sucursal.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboSucursales.Focus()
            Return False
        End If

        Return True

    End Function
End Class