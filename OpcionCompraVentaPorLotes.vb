Public Class OpcionCompraVentaPorLotes
    Public PProveedor As Integer
    Public PNombre As String
    Public PDesde As Date
    Public PHasta As Date
    Public PFacturados As Boolean
    Public PPendientes As Boolean
    Public PConStock As Boolean
    Public PSinStock As Boolean
    Public PEspecie As Integer
    Public PRegresar As Boolean
    Public PEsMerma As Boolean
    Private Sub OpcionCompraVentaPorLotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        If PEsMerma Then
            Panel2.Visible = False
            Panel4.Visible = False
        End If

        PRegresar = True

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

        PFacturados = CheckFacturados.Checked
        PPendientes = CheckNoFacturados.Checked
        PEspecie = ComboEspecie.SelectedValue
        PConStock = CheckConStock.Checked
        PSinStock = CheckSinStock.Checked

        PRegresar = False

        Me.Close()
    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

    End Sub
    Private Sub ComboEspecie_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEspecie.Validating

        If IsNothing(ComboEspecie.SelectedValue) Then ComboEspecie.SelectedValue = 0

    End Sub
    Private Function Valida() As Boolean

        If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Seleccionar Proveedor o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Not PEsMerma Then
            If Not CheckFacturados.Checked And Not CheckNoFacturados.Checked Then
                MsgBox("Falta Informar Facturados o No Facturados.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Not CheckConStock.Checked And Not CheckSinStock.Checked Then
                MsgBox("Falta Informar Con Stock o Sin Stock.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        Return True

    End Function
End Class