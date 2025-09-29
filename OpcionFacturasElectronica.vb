Public Class OpcionFacturasElectronica
    Public PClienteFacturacion As Integer
    Public PClienteOperacion As Integer
    Public PDeposito As Integer
    Private Sub OpcionFacturasElectronica_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboClienteFacturacion.DataSource = New DataTable
        ComboAlias.DataSource = New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Clientes WHERE DeOperacion = 0 AND TipoIva = 4;", Conexion, ComboClienteFacturacion.DataSource) Then Me.Close() : Exit Sub
        If Not Tablas.Read("SELECT Clave,Alias  FROM Clientes WHERE DeOperacion = 0 AND Alias <> '' AND TipoIva = 4;", Conexion, ComboAlias.DataSource) Then Me.Close() : Exit Sub
        Dim Row As DataRow = ComboClienteFacturacion.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboClienteFacturacion.DataSource.Rows.Add(Row)
        ComboClienteFacturacion.DisplayMember = "Nombre"
        ComboClienteFacturacion.ValueMember = "Clave"
        ComboClienteFacturacion.SelectedValue = 0
        With ComboClienteFacturacion
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        Row = ComboAlias.DataSource.NewRow
        Row("Clave") = 0
        Row("Alias") = ""
        ComboAlias.DataSource.Rows.Add(Row)
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboClienteOperacion.DataSource = New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Clientes WHERE DeOperacion = 1 AND TipoIva = 4;", Conexion, ComboClienteOperacion.DataSource) Then Me.Close() : Exit Sub
        Row = ComboClienteOperacion.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboClienteOperacion.DataSource.Rows.Add(Row)
        ComboClienteOperacion.DisplayMember = "Nombre"
        ComboClienteOperacion.ValueMember = "Clave"
        ComboClienteOperacion.SelectedValue = 0
        With ComboClienteOperacion
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        PClienteFacturacion = 0
        PClienteOperacion = 0

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        PClienteFacturacion = ComboClienteFacturacion.SelectedValue
        PClienteOperacion = ComboClienteOperacion.SelectedValue
        PDeposito = ComboDeposito.SelectedValue

        Me.Close()

    End Sub
    Private Sub ComboClienteFacturacion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboClienteFacturacion.Validating

        If IsNothing(ComboClienteFacturacion.SelectedValue) Then ComboClienteFacturacion.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

    End Sub
    Private Sub ComboClienteOperacion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboClienteOperacion.Validating

        If IsNothing(ComboClienteOperacion.SelectedValue) Then ComboClienteOperacion.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Function Valida() As Boolean

        If ComboClienteFacturacion.SelectedValue = 0 And ComboAlias.SelectedValue = 0 Then
            MsgBox("Falta Cliente Facturación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboClienteFacturacion.Focus()
            Return False
        End If

        If ComboClienteFacturacion.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Seleccionar Cliente Facturación o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboClienteFacturacion.Focus()
            Return False
        End If

        If ComboClienteOperacion.SelectedValue = 0 Then
            MsgBox("Debe Seleccionar Cliente Operación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboClienteOperacion.Focus()
            Return False
        End If

        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Return False
        End If

        If HallaMonedaCliente(ComboClienteOperacion.SelectedValue) <> HallaMonedaCliente(ComboClienteFacturacion.SelectedValue) Then
            MsgBox("Moneda Cliente Operación Difiere de la del Cliente Facturación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
End Class