Public Class OpcionClientes
    Public PEmisor As Integer
    Public PNombre As String
    Public PSucursal As Integer
    Public PNombreSucursal As String
    Public PFechaEntrega As Date
    Public PEsClienteObligatorio As Boolean
    Public PRegresar As Boolean
    Private Sub OpcionClientes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes WHERE TipoIva <> 4 AND DeOperacion = 0;")
        Dim Row As DataRow = ComboEmisor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboEmisor.DataSource.rows.add(Row)
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = 0
        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Clientes WHERE TipoIva <> 4 AND DeOperacion = 0 AND Alias <> '';")
        Row = ComboAlias.DataSource.NewRow
        Row("Clave") = 0
        Row("Alias") = ""
        ComboAlias.DataSource.rows.add(Row)
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        PEmisor = 0
        PSucursal = 0
        PRegresar = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ComboEmisor.SelectedValue <> 0 Then
            PEmisor = ComboEmisor.SelectedValue
            PNombre = ComboEmisor.Text
        Else
            PEmisor = ComboAlias.SelectedValue
            NombreCliente(ComboAlias.SelectedValue)
        End If

        PSucursal = ComboSucursales.SelectedValue
        PNombreSucursal = ComboSucursales.Text

        PRegresar = False

        Me.Close()

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        VerSucursales(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        VerSucursales(ComboAlias.SelectedValue)

    End Sub
    Private Sub ComboSucursales_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboSucursales.Validating

        If IsNothing(ComboSucursales.SelectedValue) Then ComboSucursales.SelectedValue = 0

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
    Private Function ObtieneCliente() As Integer

        If ComboEmisor.SelectedValue <> 0 Then Return ComboEmisor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Return ComboAlias.SelectedValue

    End Function
    Private Function Valida() As Boolean

        If PEsClienteObligatorio Then
            If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue = 0 Then
                MsgBox("Debe Seleccionar Cliente o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Return False
            End If
        End If

        If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Seleccionar Cliente o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        Dim Cliente As Integer = ObtieneCliente()

        If Cliente <> 0 Then
            Dim PorUnidadEnLista As Boolean
            Dim FinalEnLista As Boolean
            Dim Lista As Integer = HallaListaPrecios(Cliente, ComboSucursales.SelectedValue, PFechaEntrega, PorUnidadEnLista, FinalEnLista)
            If Lista = -1 Then Return False
        End If

        Return True

    End Function


End Class