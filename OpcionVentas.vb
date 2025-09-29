Public Class OpcionVentas
    Public PEspecie As Integer
    Public PVariedad As Integer
    Public PMarca As Integer
    Public PCategoria As Integer
    Public PEnvase As Integer
    Public PDesde As Date
    Public PHasta As Date
    Public PCliente As Integer
    Public PNombre As String
    Public PVendedor As Integer
    Public PCanalVenta As Integer
    Public PEstado As Integer
    Public PAbierto As Boolean
    Public PCerrado As Boolean
    Public PDeposito As Integer
    Public PFacturados As Boolean
    Public PPendientesFacturar As Boolean
    Public PRemitos As Boolean
    Public PSucursal As Integer
    Public PMuestraLotes As Boolean
    Public PMuestraPedido As Boolean
    Public PRepetirDatos As Boolean
    Public PPorFechaEmision As Boolean
    Public PPorFechaEntrega As Boolean
    Public PTextRemitos As TextBox
    Public PSinContables As Boolean
    Public PSoloContables As Boolean
    Public PTodas As Boolean
    Public PSoloConfirmados As Boolean
    Public PRegresar As Boolean
    Private Sub OpcionVentas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LlenaCombo(ComboEmisor, "", "Clientes")
        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Clientes WHERE Alias <> '';")
        Dim Row As DataRow = ComboAlias.DataSource.NewRow
        Row("Clave") = 0
        Row("Alias") = ""
        ComboAlias.DataSource.rows.add(Row)
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboEmisor.SelectedValue = 0
        ComboAlias.SelectedValue = 0
        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboVendedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 37;")
        Row = ComboVendedor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboVendedor.DataSource.rows.add(Row)
        ComboVendedor.DisplayMember = "Nombre"
        ComboVendedor.ValueMember = "Clave"
        ComboVendedor.SelectedValue = 0
        With ComboVendedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboEspecie, 1)
        With ComboEspecie
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboEspecie.SelectedValue = 0

        LlenaComboTablas(ComboVariedad, 2)
        With ComboVariedad
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboVariedad.SelectedValue = 0

        LlenaComboTablas(ComboMarca, 3)
        With ComboMarca
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboMarca.SelectedValue = 0

        LlenaComboTablas(ComboCategoria, 4)
        With ComboCategoria
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboCategoria.SelectedValue = 0

        LLenaComboEnvases(ComboEnvase)
        With ComboEnvase
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboEnvase.SelectedValue = 0

        ComboCanalVenta.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = 23;")
        Row = ComboCanalVenta.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboCanalVenta.DataSource.Rows.Add(Row)
        ComboCanalVenta.DisplayMember = "Nombre"
        ComboCanalVenta.ValueMember = "Clave"
        ComboCanalVenta.SelectedValue = 0
        With ComboCanalVenta
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboEstado.DataSource = DtAfectaPendienteAnulada()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Codigo"
        Row = ComboEstado.DataSource.NewRow
        Row("Codigo") = 4
        Row("Nombre") = "Afecta Stock y Pendiente"
        ComboEstado.DataSource.rows.add(Row)
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

        If Not PermisoTotal Then Panel1.Visible = False : Panel9.Visible = False

        Panel7.Visible = False

        If PRemitos Then
            Panel2.Visible = True
            ComboVendedor.Visible = False
            Label4.Visible = False
            Panel7.Visible = True
            PanelRemitos.Visible = True
            Panel9.Visible = False
            CheckMuestraPedido.Visible = True
            ComboMarca.Visible = False
            Label14.Visible = False
            ComboCategoria.Visible = False
            Label15.Visible = False
            ComboEnvase.Visible = False
            Label16.Visible = False
            CheckSoloConfirmados.Visible = True
        Else
            CheckSoloConfirmados.Visible = False
        End If

        PRegresar = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            MsgBox("Debe Informar Candado Abierto o Cerrado.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not CheckFacturados.Checked And Not CheckPendientesFacturar.Checked Then
            MsgBox("Debe Informar Facturados o Pendientes de Facturar.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If CheckMuestraLotes.Checked And ComboEstado.SelectedValue <> 1 Then
            MsgBox("Muestra Lotes Solo valido para Estado 'Afecta Stock'.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEmisor.SelectedValue <> 0 Then
            PCliente = ComboEmisor.SelectedValue
            PNombre = ComboEmisor.Text
        Else
            PCliente = ComboAlias.SelectedValue
            PNombre = ComboAlias.Text
        End If

        PEspecie = ComboEspecie.SelectedValue
        PVariedad = ComboVariedad.SelectedValue
        PMarca = ComboMarca.SelectedValue
        PCategoria = ComboCategoria.SelectedValue
        PEnvase = ComboEnvase.SelectedValue
        PEstado = ComboEstado.SelectedValue
        PVendedor = ComboVendedor.SelectedValue
        PCanalVenta = ComboCanalVenta.SelectedValue
        PDeposito = ComboDeposito.SelectedValue
        PDesde = DateTimeDesde.Value
        PHasta = DateTimeHasta.Value
        PAbierto = CheckAbierto.Checked
        PCerrado = CheckCerrado.Checked
        PFacturados = CheckFacturados.Checked
        PPendientesFacturar = CheckPendientesFacturar.Checked
        PMuestraLotes = CheckMuestraLotes.Checked
        PMuestraPedido = CheckMuestraPedido.Checked
        PRepetirDatos = CheckRepetirDatos.Checked
        PSucursal = ComboSucursales.SelectedValue
        PPorFechaEmision = RadioFechaEmision.Checked
        PPorFechaEntrega = RadioFechaEntrega.Checked
        PTextRemitos = TextRemitos
        PSinContables = RadioSinContables.Checked
        PSoloContables = RadioSoloContables.Checked
        PTodas = RadioTodas.Checked
        PSoloConfirmados = CheckSoloConfirmados.Checked
        PRegresar = False

        Me.Close()

    End Sub
    Private Sub ComboEspecie_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEspecie.Validating

        If IsNothing(ComboEspecie.SelectedValue) Then ComboEspecie.SelectedValue = 0

    End Sub
    Private Sub ComboVariedad_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboVariedad.Validating

        If IsNothing(ComboVariedad.SelectedValue) Then ComboVariedad.SelectedValue = 0

    End Sub
    Private Sub ComboCategoria_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCategoria.Validating

        If IsNothing(ComboCategoria.SelectedValue) Then ComboCategoria.SelectedValue = 0

    End Sub
    Private Sub ComboEnvase_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEnvase.Validating

        If IsNothing(ComboEnvase.SelectedValue) Then ComboEnvase.SelectedValue = 0

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        VerSucursales(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        VerSucursales(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboVendedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboVendedor.Validating

        If IsNothing(ComboVendedor.SelectedValue) Then ComboVendedor.SelectedValue = 0

    End Sub
    Private Sub ComboCanalVentar_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCanalVenta.Validating

        If IsNothing(ComboCanalVenta.SelectedValue) Then ComboCanalVenta.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

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
    Private Sub TextRemitos_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextRemitos.KeyPress

        If Asc(e.KeyChar) = 13 Then Exit Sub
        If InStr("0123456789-" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Function Valida() As Boolean

        Return ValidaTextBoxDeRemitos(TextRemitos)

    End Function

    
    
End Class