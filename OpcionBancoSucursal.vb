Public Class OpcionBancoSucursal
    Public PBanco As Integer
    Public PSucursal As Integer
    '
    Dim DtSucursales As DataTable
    Private Sub OpcionBancoSucursal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LlenaComboTablas(ComboBancos, 26)
        With ComboBancos
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboSucursal.DataSource = ArmaSucursalesDeBanco(0)
        ComboSucursal.DisplayMember = "Nombre"
        ComboSucursal.ValueMember = "Clave"
        ComboSucursal.SelectedValue = 0
        With ComboSucursal
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ArmaDtSucursales()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        PBanco = ComboBancos.SelectedValue
        PSucursal = ComboSucursal.SelectedValue

        Me.Close()

    End Sub
    Private Sub ComboBancos_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboBancos.Validating

        If IsNothing(ComboBancos.SelectedValue) Then ComboBancos.SelectedValue = 0
        If ComboBancos.SelectedValue = 0 Then
            ComboSucursal.SelectedValue = 0
            Exit Sub
        End If

        ComboSucursal.DataSource = ArmaSucursalesDeBanco(ComboBancos.SelectedValue)
        ComboSucursal.DisplayMember = "Nombre"
        ComboSucursal.ValueMember = "Clave"
        ComboSucursal.SelectedValue = 0

    End Sub
    Private Function ArmaSucursalesDeBanco(ByVal Banco) As DataTable

        Dim Dt As New DataTable
        Dim Row1 As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        Dt.Columns.Add(Nombre)

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtSucursales.Select("Banco = " & Banco)
        For Each Row As DataRow In RowsBusqueda
            Row1 = Dt.NewRow
            Row1("Clave") = Row("Sucursal")
            Row1("Nombre") = Row("Nombre")
            Dt.Rows.Add(Row1)
        Next
        Row1 = Dt.NewRow
        Row1("Clave") = 0
        Row1("Nombre") = ""
        Dt.Rows.Add(Row1)

        Return Dt

    End Function
    Private Sub ArmaDtSucursales()

        DtSucursales = New DataTable

        If Not Tablas.Read("SELECT Banco,Sucursal,NombreSucursal AS Nombre FROM CuentasBancarias;", Conexion, DtSucursales) Then End

    End Sub
    Private Function Valida() As Boolean

        If ComboBancos.SelectedValue = 0 Then
            MsgBox("Falta Banco.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboBancos.Focus()
            Return False
        End If

        If ComboSucursal.SelectedValue = 0 Then
            MsgBox("Falta Sucursal.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboSucursal.Focus()
            Return False
        End If

        Return True

    End Function
End Class