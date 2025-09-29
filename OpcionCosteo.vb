Public Class OpcionCosteo
    Public PNegocio As Integer
    Public PCosteo As Integer
    Public PDeposito As Integer
    Public PAbierto As Boolean
    Public PFecha As Date
    Public PRegresar As Boolean = True
    Private Sub OpcionCosteo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboNegocio.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 AND Producto = " & Fruta & " ORDER BY Nombre;")
        Dim Row As DataRow = ComboNegocio.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboNegocio.DataSource.rows.add(Row)
        ComboNegocio.DisplayMember = "Nombre"
        ComboNegocio.ValueMember = "Clave"
        ComboNegocio.SelectedValue = 0
        With ComboNegocio
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
        PAbierto = True

        If Not PermisoTotal Then
            PictureCandado.Visible = False
        End If

    End Sub
    Private Sub ComboNegocio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboNegocio.Validating

        If IsNothing(ComboNegocio.SelectedValue) Then ComboNegocio.SelectedValue = 0
        BuscaCosteo(ComboNegocio.SelectedValue)

    End Sub
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = UnRemito.ImageList1.Images.Item("Cerrado")
            PAbierto = False
        Else : PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

    End Sub
    Private Sub BuscaCosteo(ByVal Negocio As Integer)

        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & Negocio & " AND Cerrado = 0;"
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
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If ComboNegocio.SelectedValue = 0 Then
            MsgBox("Falta Informar Negocio.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            ComboNegocio.Focus()
            Exit Sub
        End If

        If ComboCosteo.SelectedValue = 0 Then
            MsgBox("Falta Informar Costeo.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            ComboCosteo.Focus()
            Exit Sub
        End If

        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Informar Deposito.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Exit Sub
        End If

        PNegocio = ComboNegocio.SelectedValue
        PCosteo = ComboCosteo.SelectedValue
        PDeposito = ComboDeposito.SelectedValue
        PFecha = DateTime1.Value
        PRegresar = False

        Me.Close()

    End Sub
End Class