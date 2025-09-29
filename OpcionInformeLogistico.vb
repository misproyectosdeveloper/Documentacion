Public Class OpcionInformeLogistico
    Public PEmisor As Integer
    Public PDeposito As Integer
    Public PEstado As Integer
    Public PArticulo As Integer
    Public PDesde As Date
    Public PHasta As Date
    Public PInformeRemitos As Boolean
    Public PCandadoAbierto As Boolean
    Public PCandadoCerrado As Boolean
    Public PSucursal As Integer
    Public PRegresar As Boolean
    Private Sub OpcionInformeLogistico_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Row As DataRow

        If PInformeRemitos Then
            LlenaCombo(ComboEmisor, "", "Clientes")
            ComboEmisor.SelectedValue = 0
            ' 
            ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Clientes WHERE Alias <> '';")
            Row = ComboAlias.DataSource.NewRow
            Row("Clave") = 0
            Row("Alias") = ""
            ComboAlias.DataSource.rows.add(Row)
            ComboAlias.DisplayMember = "Alias"
            ComboAlias.ValueMember = "Clave"
            ComboAlias.SelectedValue = 0
            LabelEmisor.Text = "Clientes"
        Else
            ComboEmisor.DataSource = ProveedoresDeInsumos()
            ComboEmisor.DisplayMember = "Nombre"
            ComboEmisor.ValueMember = "Clave"
            ComboEmisor.SelectedValue = 0
            ' 
            ComboAlias.DataSource = ProveedoresDeInsumosAlias()
            ComboAlias.DisplayMember = "Alias"
            ComboAlias.ValueMember = "Clave"
            ComboAlias.SelectedValue = 0
        End If

        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboDeposito, 20)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboArticulo.DataSource = Tablas.Leer("SELECT Nombre,Clave FROM Tablas WHERE Tipo = 6;")
        Row = ComboArticulo.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboArticulo.DataSource.rows.add(Row)
        ComboArticulo.DisplayMember = "Nombre"
        ComboArticulo.ValueMember = "Clave"
        ComboArticulo.SelectedValue = 0
        With ComboArticulo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        PRegresar = True

        If Not PermisoTotal Then Panel1.Visible = False : PCandadoAbierto = True

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        If PInformeRemitos Then VerSucursales(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        If PInformeRemitos Then VerSucursales(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub ComboArticulos_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboArticulo.Validating

        If IsNothing(ComboArticulo.SelectedValue) Then ComboArticulo.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        Dim Cliente As Integer

        If ComboEmisor.SelectedValue <> 0 Then Cliente = ComboEmisor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Cliente = ComboAlias.SelectedValue
        PEmisor = Cliente
        PDeposito = ComboDeposito.SelectedValue
        PDeposito = ComboDeposito.SelectedValue
        PArticulo = ComboArticulo.SelectedValue
        PEstado = ComboEstado.SelectedValue
        PDesde = DateTimeDesde.Value
        PHasta = DateTimeHasta.Value
        PCandadoAbierto = CheckAbierto.Checked
        PCandadoCerrado = CheckCerrado.Checked
        If PInformeRemitos And ComboEmisor.SelectedValue <> 0 Then
            PSucursal = ComboSucursales.SelectedValue
        End If

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

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If PInformeRemitos Then
            If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
                MsgBox("Falta Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Return False
            End If
        Else
            If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
                MsgBox("Falta Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Return False
            End If
        End If

        Return True

    End Function
End Class