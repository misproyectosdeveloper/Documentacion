Public Class OpcionInformeLotesReventasFacturados
    Public PProveedor As Integer
    Public PNombre As String
    Public PDesde As Date
    Public PHasta As Date
    Public PFacturados As Boolean
    Public PPendientes As Boolean
    Public PEsIngresos As Boolean
    Public PEsSenias As Boolean
    Public PEsLotesLiquidados As Boolean
    Public PReventa As Boolean
    Public PConsignacion As Boolean
    Public PTodos As Boolean
    Public PEspecie As Integer
    Public PVariedad As Integer
    Public PMarca As Integer
    Public PCategoria As Integer
    Public PEnvase As Integer
    Public PDuenio As Integer
    Public PDeposito As Integer
    Public PCosteo As Integer
    Public PSucursal As Integer
    Public PTextRemitos As TextBox
    Public PRegresar As Boolean
    Private Sub OpcionInformeLotesReventasFacturados_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LabelEmisor.Text = "Proveedor"
        ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE Producto = " & Fruta & ";")
        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '' AND Producto = " & Fruta & ";")
        Dim Row As DataRow = ComboEmisor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboEmisor.DataSource.rows.add(Row)
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = 0

        Row = ComboAlias.DataSource.NewRow
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

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboDeposito.SelectedValue = 0

        Dim Dt As New DataTable

        ArmaDueño(Dt, True)
        comboDuenio.DataSource = Dt.Copy
        comboDuenio.DisplayMember = "Nombre"
        comboDuenio.ValueMember = "Tipo"
        comboDuenio.SelectedValue = 0
        With ComboDuenio
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PEsIngresos Then
            Panel2.Visible = False
            Panel3.Visible = True
            Panel4.Visible = True
            Panel5.Visible = True
            PanelRemitos.Visible = True
        End If

        If PEsSenias Then
            Panel2.Visible = False
            Panel3.Visible = True
            Panel4.Visible = False
            Label1.Visible = False
        End If

        If PEsLotesLiquidados Then
            Panel2.Visible = True
            Panel3.Visible = False
            Panel4.Visible = False
            PanelRemitos.Visible = True
        End If

        PProveedor = 0
        PNombre = ""

        PRegresar = True

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        VerSiTieneCosteo(ComboEmisor.SelectedValue)

        VerSucursales(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        VerSiTieneCosteo(ComboEmisor.SelectedValue)

        VerSucursales(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboSucursales_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboSucursales.Validating

        If IsNothing(ComboSucursales.SelectedValue) Then ComboSucursales.SelectedValue = 0

    End Sub
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub ComboEspecie_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEspecie.Validating

        If IsNothing(ComboEspecie.SelectedValue) Then ComboEspecie.SelectedValue = 0

    End Sub
    Private Sub ComboVariedad_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboVariedad.Validating

        If IsNothing(ComboVariedad.SelectedValue) Then ComboVariedad.SelectedValue = 0

    End Sub
    Private Sub ComboMarca_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboMarca.Validating

        If IsNothing(ComboMarca.SelectedValue) Then ComboMarca.SelectedValue = 0

    End Sub
    Private Sub ComboCategoria_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCategoria.Validating

        If IsNothing(ComboCategoria.SelectedValue) Then ComboCategoria.SelectedValue = 0

    End Sub
    Private Sub ComboEnvase_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEnvase.Validating

        If IsNothing(ComboEnvase.SelectedValue) Then ComboEnvase.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub ComboDuenio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDuenio.Validating

        If IsNothing(ComboDuenio.SelectedValue) Then ComboDuenio.SelectedValue = 0

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ComboEmisor.SelectedValue <> 0 Then
            PProveedor = ComboEmisor.SelectedValue
            PNombre = ComboEmisor.Text
        Else
            PProveedor = ComboAlias.SelectedValue
            PNombre = ComboAlias.Text
        End If

        PDesde = DateTimeDesde.Value
        PHasta = DateTimeHasta.Value

        PFacturados = RadioFacturados.Checked
        PPendientes = RadioPendientes.Checked
        PReventa = CheckBoxReventa.Checked
        PConsignacion = CheckBoxConsignacion.Checked
        PTodos = CheckBoxTodos.Checked
        PEspecie = ComboEspecie.SelectedValue
        PVariedad = ComboVariedad.SelectedValue
        PMarca = ComboMarca.SelectedValue
        PCategoria = ComboCategoria.SelectedValue
        PEnvase = ComboEnvase.SelectedValue
        PDuenio = ComboDuenio.SelectedValue
        PDeposito = ComboDeposito.SelectedValue
        If PEsIngresos Then
            PCosteo = ComboCosteo.SelectedValue
        End If
        PSucursal = ComboSucursales.SelectedValue
        PTextRemitos = TextRemitos

        PRegresar = False

        Me.Close()

    End Sub
    Private Sub VerSiTieneCosteo(ByVal Emisor As Integer)

        If Not PEsIngresos Then
            Exit Sub
        End If

        If Emisor = 0 Then
            ComboCosteo.DataSource = Nothing
            Exit Sub
        End If

        If Not EsUnNegocio(Emisor) Then
            ComboCosteo.DataSource = Nothing
            Exit Sub
        End If

        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & Emisor & ";"
        ComboCosteo.DataSource = New DataTable
        ComboCosteo.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboCosteo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        ComboCosteo.DataSource.rows.add(Row)
        ComboCosteo.DisplayMember = "Nombre"
        ComboCosteo.ValueMember = "Costeo"
        ComboCosteo.SelectedValue = 0
        With ComboCosteo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Private Sub VerSucursales(ByVal Emisor As Integer)

        If Emisor = 0 Then
            ComboSucursales.DataSource = Nothing
            Exit Sub
        End If

        Dim Sql As String

        Sql = "SELECT Clave,Nombre FROM SucursalesProveedores WHERE Estado = 1 AND Proveedor = " & Emisor & ";"
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

        If PEsLotesLiquidados Then
            If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue = 0 Then
                MsgBox("Falta Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Return False
            End If
            If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
                MsgBox("Debe Seleccionar Proveedor o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Return False
            End If
        End If

        If Not PEsIngresos And Not PEsLotesLiquidados Then
            If Not CheckBoxReventa.Checked And Not CheckBoxConsignacion.Checked And Not CheckBoxTodos.Checked Then
                MsgBox("Debe Seleccionar Tipo de Operacion.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                CheckBoxReventa.Focus()
                Return False
            End If
            Dim i As Integer = 0
            If CheckBoxReventa.Checked Then i = i + 1
            If CheckBoxConsignacion.Checked Then i = i + 1
            If CheckBoxTodos.Checked Then i = i + 1
            If i > 1 Then
                MsgBox("Debe Seleccionar Un Solo Tipo Operacion.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                CheckBoxReventa.Focus()
                Return False
            End If
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PEsLotesLiquidados And Not RadioFacturados.Checked And Not RadioPendientes.Checked Then
            MsgBox("Falta Fact/Liqui. o Pendiente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If PEsLotesLiquidados And RadioFacturados.Checked And RadioPendientes.Checked Then
            MsgBox("Debe Informar Fact/Liqui. o Pendiente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return ValidaTextBoxDeRemitos(TextRemitos)

    End Function

   
End Class