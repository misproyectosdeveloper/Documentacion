Public Class OpcionInsumo
    Public PInsumo As Integer
    Public PDeposito As Integer
    Public PNombre As String
    Private Sub OpcionCliente_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboInsumo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Insumos;")
        Dim Row As DataRow = ComboInsumo.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboInsumo.DataSource.rows.add(Row)
        ComboInsumo.DisplayMember = "Nombre"
        ComboInsumo.ValueMember = "Clave"
        ComboInsumo.SelectedValue = 0
        With ComboInsumo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboDeposito, 20)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        PInsumo = 0
        PDeposito = 0

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        PInsumo = ComboInsumo.SelectedValue
        PDeposito = ComboDeposito.SelectedValue
        PNombre = ComboInsumo.Text

        Me.Close()

    End Sub
    Private Sub ComboInsumo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboInsumo.Validating

        If IsNothing(ComboInsumo.SelectedValue) Then ComboInsumo.SelectedValue = 0

    End Sub
    Private Function Valida() As Boolean

        If ComboInsumo.SelectedValue = 0 Then
            MsgBox("Falta Insumo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboInsumo.Focus()
            Return False
        End If

        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Return False
        End If

        Dim Stock As Double
        Stock = HallaStockInsumo(ComboInsumo.SelectedValue, ComboDeposito.SelectedValue, Conexion)

        If Stock < 0 Then
            MsgBox("Error de Base de Datos al leer Stock de Insumos.")
            Return False
        End If
        If Stock = 0 Then
            MsgBox("Insumo Sin Stock.")
            Return False
        End If

        Return True

    End Function


End Class