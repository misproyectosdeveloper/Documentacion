Public Class OpcionPedidos
    Public PCliente As Integer
    Public PSucursal As Integer
    Public PPedido As Integer
    Public PPedidoCliente As String
    Public PFechaEntregaDesde As Date
    Public PFechaEntregaHasta As Date
    Public PConSaldoPositivo As Boolean
    Public PConCantidades As Boolean
    Public PEsImportacion As Boolean
    Public PEsImportacionKrikos As Boolean = False
    Public PEsAlta As Boolean
    Public PEsInforme As Boolean
    Public PEsResumenPorArticulo As Boolean
    Public PEsAbierto As Boolean
    Public PEsCerrado As Boolean
    Public PEsConRepeticion As Boolean
    Public PRegresar As Boolean
    Private Sub OpcionClienteDesdeHasta_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        TextPedido.Text = Format(PPedido, "#")

        If PEsAlta Then
            Panel1.Visible = False
            Panel5.Visible = False
        End If

        If PEsInforme Then
            Panel4.Visible = False
            Panel1.Visible = False
            Panel3.Visible = False
            Panel5.Visible = False
            Label5.Visible = True
            TextFechaHasta.Visible = True
            PictureAlmanaqueHasta.Visible = True
            Label7.Visible = True
            CheckConSaldoPositivo.Visible = True
            CheckAbiertos.Visible = True
            CheckCerrados.Visible = True
            CheckCerrados.Visible = True
            CheckConRepeticion.Visible = True
        End If

        If PEsResumenPorArticulo Then
            Panel1.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            TextFechaHasta.Visible = True
            PictureAlmanaqueHasta.Visible = True
        End If

        PPedido = 0
        PRegresar = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If PEsAlta Then
            If ComboEmisor.SelectedValue <> 0 Then PCliente = ComboEmisor.SelectedValue
            If ComboAlias.SelectedValue <> 0 Then PCliente = ComboAlias.SelectedValue
            PFechaEntregaDesde = CDate(TextFechaDesde.Text)
            If TextFechaHasta.Text = "" Then
                PFechaEntregaHasta = CDate(TextFechaDesde.Text)
            Else
                PFechaEntregaHasta = CDate(TextFechaHasta.Text)
            End If
            PSucursal = ComboSucursales.SelectedValue
            PPedidoCliente = TextPedidoCliente.Text
            If CheckImportacionKrikos.Checked Then
                BorraOrdenesVencidas()
                If Not LeeArchivoTXT(PCliente, PSucursal, PFechaEntregaDesde, PPedidoCliente) Then Exit Sub
                PEsImportacionKrikos = True
                If ComboSucursales.SelectedValue = 0 Then UnPedido.PSinSucursal = True
            Else
                PEsImportacionKrikos = False
            End If
        End If

        If PEsImportacion Then
            PPedido = TextPedido.Text
            PConCantidades = RadioConCantidad.Checked
        End If

        If PEsInforme Then
            If ComboEmisor.SelectedValue <> 0 Then PCliente = ComboEmisor.SelectedValue
            If ComboAlias.SelectedValue <> 0 Then PCliente = ComboAlias.SelectedValue
            PFechaEntregaDesde = CDate(TextFechaDesde.Text)
            If TextFechaHasta.Text <> "" Then
                PFechaEntregaHasta = CDate(TextFechaHasta.Text)
            Else
                PFechaEntregaHasta = CDate(TextFechaDesde.Text)
            End If
            PConSaldoPositivo = CheckConSaldoPositivo.Checked
            PEsAbierto = CheckAbiertos.Checked
            PEsCerrado = CheckCerrados.Checked
            PEsConRepeticion = CheckConRepeticion.Checked
        End If

        If PEsResumenPorArticulo Then
            If ComboEmisor.SelectedValue <> 0 Then PCliente = ComboEmisor.SelectedValue
            If ComboAlias.SelectedValue <> 0 Then PCliente = ComboAlias.SelectedValue
            PFechaEntregaDesde = CDate(TextFechaDesde.Text)
            If TextFechaHasta.Text <> "" Then
                PFechaEntregaHasta = CDate(TextFechaHasta.Text)
            Else
                PFechaEntregaHasta = CDate(TextFechaDesde.Text)
            End If
        End If

        PRegresar = False

        Me.Close()

    End Sub
    Private Sub PictureAlmanaqueDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueDesde.Click

        TextFechaDesde.Select()  'truco para que si digito manualmente el nombre del Emisor o alias y selecciono almanaque no me toma el value del emisor o alias.

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaDesde.Text = ""
        Else : TextFechaDesde.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If

        If PEsAlta Then
            If ComboEmisor.SelectedValue <> 0 Then VerSiEsKrikos(ComboEmisor.SelectedValue)
            If ComboAlias.SelectedValue <> 0 Then VerSiEsKrikos(ComboAlias.SelectedValue)
        End If

        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueHasta.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaHasta.Text = ""
        Else : TextFechaHasta.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If

        Calendario.Dispose()

    End Sub
    Private Sub ComboEmisor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboEmisor.Click

        ComboAlias.SelectedValue = 0

    End Sub
    Private Sub ComboEmisor_TextUpdate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboEmisor.TextUpdate

        ComboAlias.SelectedValue = 0

    End Sub
    Private Sub ComboEmisor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboEmisor.GotFocus

        TextFechaDesde.Text = ""
        TextFechaHasta.Text = ""

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        VerSucursales(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboAlias_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboAlias.Click

        ComboEmisor.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_TextUpdate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboAlias.TextUpdate

        ComboEmisor.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        VerSucursales(ComboAlias.SelectedValue)

    End Sub
    Private Sub ComboSucursales_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboSucursales.Validating

        If IsNothing(ComboSucursales.SelectedValue) Then ComboSucursales.SelectedValue = 0

    End Sub
    Private Sub TextPedido_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextPedido.KeyPress

        EsNumerico(e.KeyChar, TextPedido.Text, 0)

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
    Private Sub VerSiEsKrikos(ByVal Emisor As Integer)

        If Not ExisteUnidadRedKrikos() Then CheckImportacionKrikos.Visible = False : Exit Sub

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT GLN FROM ClientesCodigos WHERE Sucursal = 0 AND Cliente = " & Emisor & ";", Conexion, Dt) Then
            MsgBox("Error al leer Tabla: ClientesCodigos")
        End If
        If Dt.Rows.Count = 0 Then
            MsgBox("No se Encuentra GLN de CASA CENTRAL Del Cliente: " & NombreCliente(Emisor))
            CheckImportacionKrikos.Visible = False
        Else
            CheckImportacionKrikos.Visible = True
            CheckImportacionKrikos.Checked = False
        End If

        Dt = Nothing

    End Sub
    Public Function PedidoExiste(ByVal Cliente As Integer, ByVal Sucursal As Integer, ByVal Pedido As Integer) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Pedido) FROM PedidosCabeza WHERE Pedido = " & Pedido & " AND Sucursal = " & Sucursal & " AND Cliente = " & Cliente & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: PedidosCabeza. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Private Function Valida() As Boolean

        If PEsImportacion Or PEsAlta Or PEsInforme Then
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
        End If

        Dim Cliente As Integer = 0
        If ComboEmisor.SelectedValue <> 0 Then Cliente = ComboEmisor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Cliente = ComboAlias.SelectedValue

        If PEsImportacion Then
            If TextPedido.Text = "" Then
                MsgBox("Debe Informar Pedido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextPedido.Focus()
                Return False
            End If
            If CInt(TextPedido.Text) = 0 Then
                MsgBox("Debe Informar Pedido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextPedido.Focus()
                Return False
            End If
            If PedidoExiste(Cliente, ComboSucursales.SelectedValue, CInt(TextPedido.Text)) = 0 Then
                MsgBox("Pedido Existe para este Cliente y Sucursal.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextPedido.Focus()
                Return False
            End If
        End If

        If PEsAlta Then
            If TextFechaDesde.Text = "" Then
                MsgBox("Debe Informar Fecha Desde. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                TextFechaDesde.Focus()
                Return False
            End If
            If Not ConsisteFecha(TextFechaDesde.Text) Then
                MsgBox("Fecha Desde Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaDesde.Focus()
                Return False
            End If
            If TextFechaHasta.Text <> "" Then
                If Not ConsisteFecha(TextFechaHasta.Text) Then
                    MsgBox("Fecha Hasta Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    TextFechaHasta.Focus()
                    Return False
                End If
                If DiferenciaDias(CDate(TextFechaDesde.Text), CDate(TextFechaHasta.Text)) < 0 Then
                    MsgBox("Fecha Entrega Desde Mayor a Fecha Entrega Hasta. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                    TextFechaDesde.Focus()
                    Return False
                End If
            End If
            If TextPedidoCliente.Text = "" And Not CheckImportacionKrikos.Checked Then
                MsgBox("Debe Informar Pedido Cliente. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                TextPedidoCliente.Focus()
                Return False
            End If
        End If

        If (PEsImportacion Or PEsAlta) And Not CheckImportacionKrikos.Checked Then
            If ComboSucursales.SelectedValue = 0 And ComboSucursales.Items.Count > 1 Then
                If MsgBox("No Informo Sucursal. Se generara un Pedido Genérico. Desea Continuar? (Si/No) ", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                    ComboSucursales.Focus()
                    Return False
                End If
            End If
        End If

        If PEsInforme Then
            If TextFechaDesde.Text = "" Then
                MsgBox("Debe Informar Fecha Desde. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                TextFechaDesde.Focus()
                Return False
            End If
            If Not ConsisteFecha(TextFechaDesde.Text) Then
                MsgBox("Fecha Desde Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaDesde.Focus()
                Return False
            End If
            If TextFechaHasta.Text <> "" Then
                If Not ConsisteFecha(TextFechaHasta.Text) Then
                    MsgBox("Fecha Hasta Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    TextFechaHasta.Focus()
                    Return False
                End If
            End If
        End If

        If PEsResumenPorArticulo Then
            If TextFechaDesde.Text = "" Then
                MsgBox("Debe Informar Fecha Desde. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                TextFechaDesde.Focus()
                Return False
            End If
            If Not ConsisteFecha(TextFechaDesde.Text) Then
                MsgBox("Fecha Desde Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaDesde.Focus()
                Return False
            End If
            If TextFechaHasta.Text <> "" Then
                If Not ConsisteFecha(TextFechaHasta.Text) Then
                    MsgBox("Fecha Hasta Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    TextFechaHasta.Focus()
                    Return False
                End If
                If DiferenciaDias(CDate(TextFechaDesde.Text), CDate(TextFechaHasta.Text)) < 0 Then
                    MsgBox("Fecha Entrega Desde Mayor a Fecha Entrega Hasta. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                    TextFechaDesde.Focus()
                    Return False
                End If
            End If
        End If

        Return True

    End Function



End Class