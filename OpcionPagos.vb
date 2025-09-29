Public Class OpcionPagos
    Public PEmisor As Integer
    Public PEmpleado As Integer
    Public PTipo As Integer
    Public PEstado As Integer
    Public PDesde As Date
    Public PHasta As Date
    Public PRegresar As Boolean = True
    Private Sub OpcionPagos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LlenaCombos()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        Dim Emisor As Integer
        If ComboEmisor.SelectedValue <> 0 Then Emisor = ComboEmisor.SelectedValue
        If ComboAliasEmisor.SelectedValue <> 0 Then Emisor = ComboAliasEmisor.SelectedValue

        PTipo = ComboTipo.SelectedValue
        PDesde = DateTimeDesde.Value
        PHasta = DateTimeHasta.Value
        PEmisor = Emisor
        PEmpleado = ComboEmpleados.SelectedValue
        PRegresar = False
        Me.Close()

    End Sub
    Private Sub ComboTipo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipo.Validating

        If IsNothing(ComboTipo.SelectedValue) Then ComboTipo.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

    End Sub
    Private Sub ComboAliasEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAliasEmisor.Validating

        If IsNothing(ComboAliasEmisor.SelectedValue) Then ComboAliasEmisor.SelectedValue = 0

    End Sub
    Private Sub ComboEmpleados_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmpleados.Validating

        If IsNothing(ComboEmpleados.SelectedValue) Then ComboEmpleados.SelectedValue = 0

    End Sub
    Private Sub LlenaCombos()

        ComboTipo.DataSource = ArmaDtComprobantes()
        ComboTipo.DisplayMember = "Nombre"
        ComboTipo.ValueMember = "Clave"
        ComboTipo.SelectedValue = 0
        With ComboTipo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        '------proveedores----------------------------------------------------------------
        ComboEmisor.DataSource = Tablas.Leer("SELECT Nombre,Alias,Clave FROM Proveedores;")
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = 0
        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        '
        ComboAliasEmisor.DataSource = Tablas.Leer("SELECT Alias,Clave FROM Proveedores WHERE Alias <> '';")
        ComboAliasEmisor.DisplayMember = "Alias"
        ComboAliasEmisor.ValueMember = "Clave"
        ComboAliasEmisor.SelectedValue = 0
        With ComboAliasEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        '------Empleados ---------------------------------------------------------------------
        ComboEmpleados.DataSource = ArmaDtEmpleados()
        ComboEmpleados.DisplayMember = "Nombre"
        ComboEmpleados.ValueMember = "Clave"
        ComboEmpleados.SelectedValue = 0
        With ComboEmpleados
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        '--------Estado----------------------------------------------------------------------
        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Public Function ArmaDtComprobantes() As DataTable

        Dim Dt As New DataTable
        '
        Dim Clave As DataColumn = New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Clave)
        '
        Dim Nombre As DataColumn = New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        Dt.Columns.Add(Nombre)

        Dim Row As DataRow

        Row = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Dt.Rows.Add(Row)
        Row = Dt.NewRow
        Row("Clave") = 600
        Row("Nombre") = "Orden de Pago Proveedores"
        Dt.Rows.Add(Row)
        Row = Dt.NewRow
        Row("Clave") = 601
        Row("Nombre") = "Orden Pago Contables"
        Dt.Rows.Add(Row)
        Row = Dt.NewRow
        Row("Clave") = 5010
        Row("Nombre") = "Pagos Otros Proveedores"
        Dt.Rows.Add(Row)
        If PermisoLectura(9) Then
            Row = Dt.NewRow
            Row("Clave") = 4010
            Row("Nombre") = "Pagos Sueldos"
            Dt.Rows.Add(Row)
        End If

        Return Dt

    End Function
    Private Function ArmaDtEmpleados() As DataTable

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Apellidos AS Nombre,Legajo AS Clave FROM Empleados;", Conexion, Dt) Then Return Nothing
        If PermisoTotal Then
            If Not Tablas.Read("SELECT Apellidos AS Nombre,Legajo AS Clave FROM Empleados;", ConexionN, Dt) Then Return Nothing
        End If

        Return Dt

    End Function
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If ComboEmisor.SelectedValue <> 0 And ComboAliasEmisor.SelectedValue <> 0 Then
            MsgBox("Debe Seleccionar Proveedor o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        Return True

    End Function

    
    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class