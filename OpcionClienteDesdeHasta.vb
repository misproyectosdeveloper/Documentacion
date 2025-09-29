Public Class OpcionClienteDesdeHasta
    Public PCliente As Integer
    Public PDesde As Date
    Public PHasta As Date
    Public PEsDebitoCreditoPorFactura As Boolean
    Public PSucursal As Integer
    Public PRegresar As Boolean
    Private Sub OpcionClienteDesdeHasta_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LlenaCombo(ComboEmisor, "", "Clientes")
        ComboEmisor.SelectedValue = 0

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

        If PEsDebitoCreditoPorFactura Then
            Label1.Visible = False
            ComboSucursales.Visible = False
        End If

        PRegresar = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ComboEmisor.SelectedValue <> 0 Then PCliente = ComboEmisor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then PCliente = ComboAlias.SelectedValue

        PSucursal = ComboSucursales.SelectedValue

        PDesde = Format(DateTimeDesde.Value, "dd/MM/yyyy")
        PHasta = Format(DateTimeHasta.Value, "dd/MM/yyyy")

        PRegresar = False

        Me.Close()

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        VerSucursales(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        VerSucursales(ComboEmisor.SelectedValue)

    End Sub
    Private Sub VerSucursales(ByVal Emisor As Integer)

        If Emisor = 0 Then
            ComboSucursales.DataSource = Nothing
            Exit Sub
        End If

        Dim Sql As String

        Sql = "SELECT Clave,Nombre FROM SucursalesClientes WHERE Cliente = " & Emisor & ";"
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
            MsgBox("Debe Elegir Cliente o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Elegir Cliente o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
End Class