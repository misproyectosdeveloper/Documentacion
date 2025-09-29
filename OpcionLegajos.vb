Public Class OpcionLegajos
    Public PLegajo As Integer
    Public PNombre As String
    Public PAbierto As Boolean
    Public PRegresar As Boolean
    '
    Dim Dt As DataTable
    Dim DtN As DataTable
    Private Sub OpcionLegajos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        PRegresar = True

        If PLegajo = 0 Then PAbierto = False
        PictureCandado_Click(Nothing, Nothing)

        Dt = New DataTable
        If Not Tablas.Read("SELECT 1 AS Operacion,Legajo,RIGHT('0000' + CAST(Legajo AS varchar),4) + ' - ' + Apellidos + ' ' + Nombres As Con FROM Empleados WHERE Estado = 1;", Conexion, Dt) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read("SELECT 2 as Operacion,Legajo,RIGHT('0000' + CAST(Legajo AS varchar),4) + ' - ' + Apellidos + ' ' + Nombres As Con FROM Empleados WHERE Estado = 1;", ConexionN, Dt) Then Exit Sub
        Else
            PictureCandado.Visible = False
        End If
        Dim Row As DataRow = Dt.NewRow
        Row("Legajo") = 0
        Row("Con") = ""
        Dt.Rows.Add(Row)
        ComboLegajos.DataSource = Dt
        ComboLegajos.DisplayMember = "Con"
        ComboLegajos.ValueMember = "Legajo"
        With ComboLegajos
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboLegajos.SelectedValue = PLegajo

        DtN = New DataTable
        If Not Tablas.Read("SELECT 1 AS Operacion,Legajo,Apellidos + ' ' + Nombres As Con FROM Empleados WHERE Estado = 1;", Conexion, DtN) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read("SELECT 2 as Operacion,Legajo,Apellidos + ' ' + Nombres As Con FROM Empleados WHERE Estado = 1;", ConexionN, DtN) Then Exit Sub
        Else
            PictureCandado.Visible = False
        End If
        Row = DtN.NewRow
        Row("Legajo") = 0
        Row("Con") = ""
        DtN.Rows.Add(Row)
        ComboNombre.DataSource = DtN
        ComboNombre.DisplayMember = "Con"
        ComboNombre.ValueMember = "Legajo"
        With ComboNombre
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboNombre.SelectedValue = 0

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ComboLegajos.SelectedValue <> 0 Then
            PLegajo = ComboLegajos.SelectedValue
            PNombre = ComboLegajos.Text
        Else
            PLegajo = ComboNombre.SelectedValue
            PNombre = ComboNombre.Text
        End If

        PRegresar = False

        Dt.Dispose()

        Me.Close()

    End Sub
    Private Sub ComboLegajos_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboLegajos.Validating

        If IsNothing(ComboLegajos.SelectedValue) Then ComboLegajos.SelectedValue = 0

    End Sub
    Private Sub ComboNombre_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboNombre.Validating

        If IsNothing(ComboNombre.SelectedValue) Then ComboNombre.SelectedValue = 0

    End Sub
    Private Sub TextMes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextAnio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            PAbierto = False
        Else
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

    End Sub
    Private Function Valida() As Boolean

        If ComboLegajos.SelectedValue = 0 And ComboNombre.SelectedValue = 0 Then
            MsgBox("Falta Informar Legajo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboLegajos.Focus()
            Return False
        End If

        If ComboLegajos.SelectedValue <> 0 And ComboNombre.SelectedValue <> 0 Then
            MsgBox("Debe Informar Legajo o Nombre.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboLegajos.Focus()
            Return False
        End If

        If (ComboLegajos.SelectedValue Or ComboNombre.SelectedValue) >= 5000 And PAbierto Then
            MsgBox("Proceso Invalido para este Legajo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboLegajos.Focus()
            Return False
        End If

        Return True

    End Function

End Class