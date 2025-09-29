Public Class OpcionListaDePrecios
    Public PEmisor As Integer
    Public PEsCliente As Boolean
    Public PEsVendedor As Boolean
    Public PEsProveedor As Boolean
    Public PNombre As String
    Public PZona As Integer
    Public PListaDiaria As Boolean
    Public PRegresar As Boolean
    '
    Dim Tipo As Integer
    Private Sub OpcionClienteDesdeHasta_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Sql As String

        If PEsCliente Then
            Sql = "SELECT Clave,Nombre FROM Clientes WHERE TipoIva <> 4;"
            LabelEmisor.Text = "Cliente"
        End If
        If PEsProveedor Then
            Sql = "SELECT Clave,Nombre FROM Proveedores WHERE TipoIva <> 4;"
            LabelEmisor.Text = "Proveedor"
        End If

        ComboEmisor.DataSource = Tablas.Leer(Sql)
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

        LlenaComboTablas(ComboVendedor, 37)
        ComboVendedor.SelectedValue = 0
        With ComboVendedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PEsCliente Then
            Sql = "SELECT Clave,Alias FROM Clientes WHERE TipoIva <> 4 AND Alias <> '';"
        End If
        If PEsProveedor Then
            Sql = "SELECT Clave,Alias FROM Proveedores WHERE TipoIva <> 4 AND Alias <> '';"
        End If
        ComboAlias.DataSource = Tablas.Leer(Sql)
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

        If PEsProveedor Then Panel3.Visible = False : RadioListaDiaria.Checked = True
        If PEsVendedor Then Panel4.Visible = False : LabelEmisor.Text = "Vendedor" : Label2.Visible = False : ComboAlias.Visible = False

        If PEsProveedor Then
            Panel1.Visible = False
        End If

        PZona = 0
        PEmisor = 0
        PRegresar = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ComboEmisor.SelectedValue <> 0 Then PEmisor = ComboEmisor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then PEmisor = ComboAlias.SelectedValue
        If ComboVendedor.SelectedValue <> 0 Then PEmisor = ComboVendedor.SelectedValue
        If PEsProveedor Then PNombre = NombreProveedor(PEmisor)
        If PEsCliente Then
            If ComboEmisor.SelectedValue <> 0 Then
                PNombre = NombreCliente(PEmisor)
            End If
            If ComboVendedor.SelectedValue <> 0 Then
                PNombre = NombreVendedor(ComboVendedor.SelectedValue)
                PEsVendedor = True
            End If
        End If
        PZona = ComboZona.SelectedValue
        PListaDiaria = RadioListaDiaria.Checked

        PRegresar = False

        Me.Close()

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        VerZonas(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        VerZonas(ComboAlias.SelectedValue)

    End Sub
    Private Sub ComboZona_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboZona.Validating

        If IsNothing(ComboZona.SelectedValue) Then ComboZona.SelectedValue = 0

    End Sub
    Private Sub ComboVendedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboVendedor.Validating

        If IsNothing(ComboVendedor.SelectedValue) Then ComboZona.SelectedValue = 0

    End Sub
    Private Sub VerZonas(ByVal Emisor As Integer)

        If Emisor = 0 Then
            ComboZona.DataSource = Nothing
            Exit Sub
        End If

        If PEsCliente Then Tipo = TieneListaDePreciosW(Emisor)
        If PEsProveedor Then Tipo = TieneListaDePreciosProveedorW(Emisor)

        If Tipo = 2 Then
            LLenaComboZonas(Emisor)
            ComboZona.SelectedValue = 0
            With ComboZona
                .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                .AutoCompleteSource = AutoCompleteSource.ListItems
            End With
        End If

        If Tipo = 1 Or Tipo = 0 Or Tipo = 3 Then
            ComboZona.DataSource = Nothing
            Exit Sub
        End If

        If Tipo = -1 Then
            If PEsCliente Then MsgBox("Cliente No Encontrado en Tabla:Clientes.", MsgBoxStyle.Critical)
            If PEsProveedor Then MsgBox("Proveedor No Encontrado en Tabla:Proveedores.", MsgBoxStyle.Critical)
            End
        End If

    End Sub
    Private Sub LLenaComboZonas(ByVal Emisor As Integer)

        Dim Sql As String

        If PEsCliente Then Sql = "SELECT DISTINCT C.Zona AS Clave,T.Nombre FROM SucursalesClientes AS C INNER JOIN Tablas AS T ON C.Zona = T.Clave WHERE C.Estado = 1 AND C.Cliente = " & Emisor & ";"
        If PEsProveedor Then Sql = "SELECT DISTINCT C.Zona AS Clave,T.Nombre FROM SucursalesProveedores AS C INNER JOIN Tablas AS T ON C.Zona = T.Clave WHERE C.Estado = 1 AND C.Proveedor = " & Emisor & ";"

        Dim Dt As New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then End

        ComboZona.DataSource = Dt
        Dim Row As DataRow = ComboZona.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        ComboZona.DataSource.Rows.Add(Row)
        ComboZona.DisplayMember = "Nombre"
        ComboZona.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        If Tipo = 0 And ComboVendedor.SelectedValue = 0 Then
            If PEsCliente Then MsgBox("Cliente No Autorizado para Definir Lista de Precios.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            If PEsProveedor Then MsgBox("Proveedor No Autorizado para Definir Lista de Precios.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue = 0 And ComboVendedor.SelectedValue = 0 Then
            If PEsCliente Then MsgBox("Debe Elegir Cliente o Alias del Cliente o Vendedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            If PEsProveedor Then MsgBox("Debe Elegir Proveedor o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If (ComboEmisor.SelectedValue <> 0 Or ComboAlias.SelectedValue <> 0) And ComboVendedor.SelectedValue <> 0 Then
            If PEsCliente Then MsgBox("Debe Elegir Cliente, Alias del Cliente o Vendedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            If PEsProveedor Then MsgBox("Debe Elegir Proveedor o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If PEsCliente Or ComboVendedor.SelectedValue <> 0 Then
            If Not RadioListaDiaria.Checked And Not RadioSemanaCompleta.Checked Then
                MsgBox("Falta Informar Lista Diaria o a Semana Completa.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If Tipo = 2 And ComboZona.SelectedValue = 0 Then
            MsgBox("Debe Informar Zona.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboZona.Focus()
            Return False
        End If

        Return True

    End Function
End Class