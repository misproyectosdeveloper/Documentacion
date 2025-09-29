Public Class OpcionParaNegocios
    Public PNegocio As Integer
    Public PCosteo As Integer
    Public PNombreCosteo As String
    Public PDesde As Date
    Public PHasta As Date
    Public PEstado As Integer
    Public PAbierto As Boolean
    Public PCerrado As Boolean
    Public PInsumos As Boolean
    Public PRegresar As Boolean
    Private Sub OpcionParaNegocios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not PermisoTotal Then Panel1.Visible = False

        PRegresar = True

    End Sub
    Private Sub ComboNegocio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboNegocio.Validating

        If IsNothing(ComboNegocio.SelectedValue) Then ComboNegocio.SelectedValue = 0

        If ComboNegocio.SelectedValue = 0 Then
            ComboCosteo.DataSource = Nothing
            Exit Sub
        End If

        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & ComboNegocio.SelectedValue & ";"
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
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboNegocio.SelectedValue = 0 Then
            MsgBox("Fecha Informar Negocio.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If ComboCosteo.SelectedValue = 0 Then
            MsgBox("Fecha Informar Costeo.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PEstado = ComboEstado.SelectedValue
        PNegocio = ComboNegocio.SelectedValue
        PCosteo = ComboCosteo.SelectedValue
        PNombreCosteo = ComboCosteo.Text
        PDesde = DateTimeDesde.Value
        PHasta = DateTimeHasta.Value
        PAbierto = CheckAbierto.Checked
        PCerrado = CheckCerrado.Checked

        PRegresar = False

        Me.Close()

    End Sub
End Class