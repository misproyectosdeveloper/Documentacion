Public Class ListaBancos
    Public PEsSeleccionaCuenta As Boolean
    Public PConChequera As Boolean
    Public PBanco As Integer
    Public PCuenta As Double
    Public PSerie As String
    Public PEsBancoNegro As Boolean
    Public PEsBancoBlanco As Boolean
    Public PUltimoNumero As Integer
    Public PNumeracionInicial As Integer
    Public PNumeracionFinal As Integer
    Public PEsSoloPesos As Boolean
    Public PMoneda As Integer
    Public PEsBancoLiquidaDivisa As Boolean
    Public PSaldoInicial As Double
    Public PMonedaCta As Integer
    Public PPaseDeProyectos As ItemPaseDeProyectos
    Public PBloqueaFunciones As Boolean
    '
    Dim Dt As DataTable
    Dim DtBancosNegro As DataTable
    Private Sub ListaBancos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            Conexion = PPaseDeProyectos.Conexion
            ConexionN = PPaseDeProyectos.ConexionN
            PermisoTotal = PPaseDeProyectos.PermisoTotal
        End If
        '----------------------------------------------------------------------------------

        LlenaComboTablas(ComboBancos, 26)
        ComboBancos.SelectedValue = 0

        LlenaComboTablas(ComboMoneda, 27)
        Dim Row As DataRow = ComboMoneda.DataSource.newrow
        Row("Nombre") = "Pesos"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)

        ComboTipo.Items.Clear()
        ComboTipo.Items.Add("")
        ComboTipo.Items.Add("CA")
        ComboTipo.Items.Add("CC")

        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM CuentasBancarias;", Conexion, Dt) Then Me.Close() : Exit Sub

        DtBancosNegro = New DataTable
        If Not Tablas.Read("SELECT Clave FROM Tablas WHERE Tipo = 26 AND Activo2 = 1;", Conexion, DtBancosNegro) Then
            Me.Close()
            Exit Sub
        End If

        If Not PermisoTotal Then
            Dim RowsBusqueda() As DataRow
            For Each Row1 As DataRow In Dt.Rows
                RowsBusqueda = DtBancosNegro.Select("Clave = " & Row1("Banco"))
                If RowsBusqueda.Length <> 0 Then Row1.Delete()
            Next
        End If

        TreeView1.Nodes.Clear()
        ArmaArbol()

        MuestraNodo(TreeView1.Nodes(0))

        If PEsSeleccionaCuenta Then
            Panel1.Enabled = False
            ButtonElegir.Visible = True
            ButtonAgenda.Visible = False
        End If

    End Sub
    Private Sub ButtonModificarSucursal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonModificarSucursal.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboBancos.SelectedValue = 0 Then
            MsgBox("Debe seleccionar Banco.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If TextSucursal.Text = "" Then
            MsgBox("Debe seleccionar Sucursal.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim Banco As Integer = ComboBancos.SelectedValue
        Dim Sucursal As Integer = TextSucursal.Text
        Dim Cuenta As Decimal
        If TextNumero.Text = "" Then
            Cuenta = 0
        Else
            Cuenta = CDec(TextNumero.Text)
        End If

        GModificacionOk = False

        UnaCuentaBancaria.PBanco = ComboBancos.SelectedValue
        UnaCuentaBancaria.PSucursal = CInt(TextSucursal.Text)
        UnaCuentaBancaria.PEsModificarSucursal = True
        UnaCuentaBancaria.ShowDialog()
        If GModificacionOk = True Then
            ListaBancos_Load(Nothing, Nothing)
            BuscaNodo(Banco, Sucursal, Cuenta)
        End If
        UnaCuentaBancaria.Dispose()

    End Sub
    Private Sub ButtonNuevaSucursal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevaSucursal.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboBancos.SelectedValue = 0 Then
            MsgBox("Debe seleccionar Banco.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim Banco As Integer = ComboBancos.SelectedValue

        GModificacionOk = False

        UnaCuentaBancaria.PBanco = ComboBancos.SelectedValue
        UnaCuentaBancaria.PEsAltaSucursal = True
        UnaCuentaBancaria.ShowDialog()
        If GModificacionOk = True Then
            ListaBancos_Load(Nothing, Nothing)
            BuscaNodo(Banco, UnaCuentaBancaria.PSucursal, UnaCuentaBancaria.PCuenta)
        End If
        UnaCuentaBancaria.Dispose()

    End Sub
    Private Sub ButtonModificaCuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonModificaCuenta.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboBancos.SelectedValue = 0 Then
            MsgBox("Debe seleccionar Banco.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If TextSucursal.Text = "" Then
            MsgBox("Debe seleccionar Sucursal.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If TextNumero.Text = "" Then
            MsgBox("Debe seleccionar Cuenta.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim Banco As Integer = ComboBancos.SelectedValue
        Dim Sucursal As Integer = TextSucursal.Text
        Dim Cuenta As Decimal = TextNumero.Text

        GModificacionOk = False

        UnaCuentaBancaria.PBanco = ComboBancos.SelectedValue
        UnaCuentaBancaria.PSucursal = CInt(TextSucursal.Text)
        UnaCuentaBancaria.PCuenta = CDec(TextNumero.Text)
        UnaCuentaBancaria.PEsModificarCuenta = True
        UnaCuentaBancaria.ShowDialog()
        If GModificacionOk = True Then
            ListaBancos_Load(Nothing, Nothing)
            BuscaNodo(Banco, Sucursal, Cuenta)
        End If
        UnaCuentaBancaria.Dispose()

    End Sub
    Private Sub ButtonNuevaCuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevaCuenta.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If


        If ComboBancos.SelectedValue = 0 Then
            MsgBox("Debe seleccionar Banco.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If TextSucursal.Text = "" Then
            MsgBox("Debe seleccionar Sucursal.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim Banco As Integer = ComboBancos.SelectedValue
        Dim Sucursal As Integer = TextSucursal.Text

        GModificacionOk = False

        UnaCuentaBancaria.PBanco = ComboBancos.SelectedValue
        UnaCuentaBancaria.PSucursal = CInt(TextSucursal.Text)
        UnaCuentaBancaria.PCuenta = 0
        UnaCuentaBancaria.PEsAltaCuenta = True
        UnaCuentaBancaria.ShowDialog()
        If GModificacionOk = True Then
            ListaBancos_Load(Nothing, Nothing)
            BuscaNodo(Banco, Sucursal, UnaCuentaBancaria.PCuenta)
        End If
        UnaCuentaBancaria.Dispose()

    End Sub
    Private Sub ButtonElegir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonElegir.Click

        If TreeView1.SelectedNode.Level <> 3 Then
            MsgBox("Selección Incorrecta.")
            Exit Sub
        End If

        If PEsSoloPesos And ComboMoneda.SelectedValue <> 1 Then
            MsgBox("Moneda Incorrecta.")
            Exit Sub
        End If

        If Not PEsSoloPesos And PMoneda <> 0 And ComboMoneda.SelectedValue <> PMoneda Then
            MsgBox("Moneda Incorrecta.")
            Exit Sub
        End If

        If PConChequera And Not CheckTieneChequera.Checked Then
            MsgBox("Cuenta No Tiene Chequera.")
            Exit Sub
        End If

        If PEsBancoLiquidaDivisa And Not CheckLiquidaDivisa.Checked Then
            MsgBox("Cuenta No Liquida Divisa.")
            Exit Sub
        End If

        If PEsBancoNegro Then
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = DtBancosNegro.Select("Clave = " & ComboBancos.SelectedValue)
            If RowsBusqueda.Length = 0 Then
                MsgBox("Banco Incorrecto.")
                Exit Sub
            End If
        End If

        If PEsBancoBlanco Then
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = DtBancosNegro.Select("Clave = " & ComboBancos.SelectedValue)
            If RowsBusqueda.Length <> 0 Then
                MsgBox("Banco Incorrecto.")
                Exit Sub
            End If
        End If

        PCuenta = CDbl(TextNumero.Text)
        PBanco = ComboBancos.SelectedValue
        PSerie = TextUltimaSerie.Text
        PUltimoNumero = CInt(TextUltimoNumero.Text)
        PNumeracionInicial = CInt(TextNumeracionInicial.Text)
        PNumeracionFinal = CInt(TextNumeracionFinal.Text)
        PSaldoInicial = CDbl(TextSaldoInicial.Text)
        PMonedaCta = ComboMoneda.SelectedValue

        Me.Close()

    End Sub
    Private Sub TreeView1_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick

        MuestraNodo(e.Node)

    End Sub
    Private Sub ButtonAgenda_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAgenda.Click

        If ComboBancos.SelectedValue = 0 Then
            MsgBox("Falta Banco", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        UnaAgenda.PArchivo = "Banco" & ComboBancos.SelectedValue.ToString
        UnaAgenda.ShowDialog()

    End Sub
    Private Sub ArmaArbol()

        Dim NodoRaiz As New TreeNode

        TreeView1.Nodes.Clear()

        NodoRaiz.Name = "Bancos"
        NodoRaiz.Text = "Bancos"
        TreeView1.Nodes.Add(NodoRaiz)
        Nivel_1(NodoRaiz)
        TreeView1.Nodes("Bancos").Expand()

    End Sub
    Private Function Nivel_1(ByVal Padre As TreeNode) As Boolean

        Dim BancoAnt As Integer = 0
        Dim SucursalAnt As Integer = 0
        Dim CuentaAnt As Double = 0
        Dim NodoBanco As TreeNode
        Dim NodoSucursal As TreeNode
        Dim NodoCuenta As TreeNode

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Banco,Sucursal,TipoCuenta,Numero"

        For Each Row As DataRowView In View
            If BancoAnt <> Row("Banco") Then
                NodoBanco = New TreeNode
                NodoBanco.Text = HallaNombreBanco(Row("Banco"))
                NodoBanco.Name = Row("Banco")
                Padre.Nodes.Add(NodoBanco)
                BancoAnt = Row("Banco")
                SucursalAnt = 0
                CuentaAnt = 0
            End If
            If SucursalAnt <> Row("Sucursal") Then
                NodoSucursal = New TreeNode
                NodoSucursal.Text = Format(Row("Sucursal"), "000") & "-" & Row("NombreSucursal")
                NodoSucursal.Name = Row("Sucursal")
                NodoBanco.Nodes.Add(NodoSucursal)
                SucursalAnt = Row("Sucursal")
                CuentaAnt = 0
            End If
            If CuentaAnt <> Row("Numero") Then
                NodoCuenta = New TreeNode
                Dim Tipo As String = ""
                Select Case Row("TipoCuenta")
                    Case 1
                        Tipo = "CA"
                    Case 2
                        Tipo = "CC"
                End Select
                NodoCuenta.Text = Tipo & ": " & FormatNumber(Row("Numero"), 0)
                NodoCuenta.Name = Row("Numero")
                NodoSucursal.Nodes.Add(NodoCuenta)
                CuentaAnt = Row("Numero")
            Else
                NodoCuenta = New TreeNode
                NodoCuenta.Text = Row("TipoCuenta") & "-" & FormatNumber(Row("Numero"), 0)
                NodoCuenta.Name = Row("Numero")
                NodoSucursal.Nodes.Add(NodoCuenta)
            End If
        Next

        'Muestra bancos sin Cuentas.
        Dim DtBancos As New DataTable
        Dim SqlNegro As String = ""
        If Not PermisoTotal Then
            SqlNegro = " AND Activo2 = 0"
        End If
        If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 26 " & SqlNegro & ";", Conexion, DtBancos) Then
            Me.Close()
            Exit Function
        End If

        Dim RowsBusqueda() As DataRow
        For Each Row As DataRow In DtBancos.Rows
            RowsBusqueda = Dt.Select("Banco = " & Row("Clave"))
            If RowsBusqueda.Length = 0 Then
                NodoBanco = New TreeNode
                NodoBanco.Text = Row("Nombre")
                NodoBanco.Name = Row("Clave")
                Padre.Nodes.Add(NodoBanco)
            End If
        Next

        DtBancos.Dispose()

    End Function
    Private Sub TreeView1_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseDoubleClick

        e.Node.ExpandAll()

        If e.Node.Level = 3 And PEsSeleccionaCuenta Then
            ButtonElegir_Click(Nothing, Nothing)
        End If

    End Sub
    Private Sub MuestraNodo(ByVal Nodo As TreeNode)

        Dim RowsBusqueda() As DataRow

        ComboBancos.SelectedValue = 0
        TextSucursal.Text = ""
        TextNombre.Text = ""
        ComboTipo.SelectedIndex = 0
        TextNumero.Text = ""
        TextUltimaSerie.Text = ""
        TextUltimoNumero.Text = ""
        Panel3.Visible = False
        TextCbu.Text = ""
        ComboMoneda.SelectedValue = 0
        TextSaldoInicial.Text = ""
        TextNumeracionInicial.Text = ""
        TextNumeracionFinal.Text = ""

        If Nodo.Level = 1 Then
            ComboBancos.SelectedValue = Nodo.Name
        End If
        If Nodo.Level = 2 Then
            ComboBancos.SelectedValue = Nodo.Parent.Name
            TextSucursal.Text = Nodo.Name
            TextNombre.Text = Mid$(Nodo.Text, 5, 30)
        End If
        If Nodo.Level = 3 Then
            RowsBusqueda = Dt.Select("Banco = " & Nodo.Parent.Parent.Name & " AND Sucursal = " & Nodo.Parent.Name & " AND Numero = " & Nodo.Name)
            ComboBancos.SelectedValue = RowsBusqueda(0).Item("Banco")
            TextSucursal.Text = RowsBusqueda(0).Item("Sucursal")
            TextNombre.Text = RowsBusqueda(0).Item("NombreSucursal")
            ComboTipo.SelectedIndex = RowsBusqueda(0).Item("TipoCuenta")
            TextNumero.Text = FormatNumber(RowsBusqueda(0).Item("Numero"), 0)
            TextUltimaSerie.Text = RowsBusqueda(0).Item("UltimaSerie")
            TextUltimoNumero.Text = FormatNumber(RowsBusqueda(0).Item("UltimoNumero"), 0)
            CheckTieneChequera.Checked = RowsBusqueda(0).Item("TieneChequera")
            If CheckTieneChequera.Checked Then
                Panel3.Visible = True
            Else : Panel3.Visible = False
            End If
            CheckLiquidaDivisa.Checked = RowsBusqueda(0).Item("LiquidaDivisa")
            TextCbu.Text = RowsBusqueda(0).Item("Cbu")
            ComboMoneda.SelectedValue = RowsBusqueda(0).Item("Moneda")
            TextSaldoInicial.Text = FormatNumber(RowsBusqueda(0).Item("SaldoInicial"), 2)
            TextNumeracionInicial.Text = FormatNumber(RowsBusqueda(0).Item("NumeracionInicial"), 0)
            TextNumeracionFinal.Text = FormatNumber(RowsBusqueda(0).Item("NumeracionFinal"), 0)
        End If

    End Sub
    Private Function HallaNombreBanco(ByVal Banco As Integer) As String

        Dim Dt As New DataTable
        If Not Tablas.Read("Select Nombre From Tablas WHERE Tipo = 26 AND Clave = " & Banco & ";", Conexion, Dt) Then End
        If Dt.Rows.Count <> 0 Then Dt.Dispose() : Return Dt.Rows(0).Item("Nombre")

        Dt.Dispose()

    End Function
    Private Sub BuscaNodo(ByVal Banco As Integer, ByVal Sucursal As Integer, ByVal Cuenta As Decimal)

        ' se puede hacer recursivo.  Ver: https://msdn.microsoft.com/es-es/library/wwc698z7%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396

        Dim n As TreeNode = TreeView1.Nodes("BANCOS")

        Dim aNode As TreeNode
        For Each aNode In n.Nodes
            If aNode.Name = Banco Then  'Nivel Bancos.
                aNode.Expand()
                Dim aNode2 As TreeNode
                For Each aNode2 In aNode.Nodes
                    If aNode2.Name = Sucursal Then   'Nivel Sucursales.
                        aNode2.Expand()
                        MuestraNodo(aNode2)
                        Dim aNode3 As TreeNode
                        For Each aNode3 In aNode2.Nodes
                            If aNode3.Name = Cuenta Then MuestraNodo(aNode3) : Exit Sub 'Nivel de cuentas.
                        Next
                    End If
                Next
            End If
        Next

    End Sub
    
    
   
    
    
   
   
End Class