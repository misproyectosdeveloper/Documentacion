Public Class OpcionVentasTotalesPorArticulos
    Public PEsComparativo As Boolean
    Public PEspecie As Integer
    Public PVariedad As Integer
    Public PDesde As Date
    Public PHasta As Date
    Public PAnio As Integer
    Public PMes As Integer
    Public PCliente As Integer
    Public PNombre As String
    Public PSucursal As Integer
    Public PPorClienteSucursal As Boolean
    Public PEsPorClienteSucursal As Boolean
    Public PRegresar As Boolean
    Private Sub OpcionVentasTotalesPorArticulos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        ComboMeses.DataSource = HallaMesesDelAñio()
        ComboMeses.DisplayMember = "Nombre"
        ComboMeses.ValueMember = "Mes"
        ComboMeses.SelectedValue = 0

        If PEsComparativo Then
            Panel6.Visible = False
            Panel1.Visible = True
        Else
            Panel6.Visible = True
            Panel1.Visible = False
        End If

        If Not PEsPorClienteSucursal Then RadioPorSucursal.Visible = False

        PRegresar = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If ComboEmisor.SelectedValue <> 0 Then
            PCliente = ComboEmisor.SelectedValue
            PNombre = ComboEmisor.Text
        Else
            PCliente = ComboAlias.SelectedValue
            PNombre = ComboAlias.Text
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Exit Sub
        End If

        If PEsComparativo Then
            If ComboMeses.SelectedValue = 0 Then
                MsgBox("Falta Informar Mes.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                ComboMeses.Focus()
                Exit Sub
            End If
            If TextAnio.Text = "" Then
                MsgBox("Falta Informar Año.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                TextAnio.Focus()
                Exit Sub
            End If
            If Val(TextAnio.Text) = 0 Then
                MsgBox("Falta Informar Año.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                TextAnio.Focus()
                Exit Sub
            End If
        End If

        PEspecie = ComboEspecie.SelectedValue
        PVariedad = ComboVariedad.SelectedValue
        PDesde = DateTimeDesde.Value
        PHasta = DateTimeHasta.Value
        PSucursal = ComboSucursales.SelectedValue
        PMes = ComboMeses.SelectedValue
        If TextAnio.Text <> "" Then PAnio = TextAnio.Text
        PPorClienteSucursal = RadioPorClienteSucursal.Checked
        If PPorClienteSucursal Then PSucursal = 0

        PRegresar = False

        Me.Close()

    End Sub
    Private Sub ComboEspecie_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEspecie.Validating

        If IsNothing(ComboEspecie.SelectedValue) Then ComboEspecie.SelectedValue = 0

    End Sub
    Private Sub ComboVariedad_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboVariedad.Validating

        If IsNothing(ComboVariedad.SelectedValue) Then ComboVariedad.SelectedValue = 0

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        VerSucursales(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        VerSucursales(ComboEmisor.SelectedValue)

    End Sub
    Private Sub TextAnio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextAnio.KeyPress

        EsNumerico(e.KeyChar, TextAnio.Text, 0)

    End Sub
    Private Sub RadioPorClienteSucursal_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioPorClienteSucursal.CheckedChanged

        If RadioPorClienteSucursal.Checked Then
            Panel5.Visible = True
        Else
            Panel5.Visible = False
        End If

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
   
End Class