Public Class OpcionInformeComprobantesQueAfectaALotes
    Public PProveedor As Integer
    Public PCosteo As Integer
    Public PDesde As Date
    Public PHasta As Date
    Public PAbierto As Boolean
    Public PCerrado As Boolean
    Public PConIva As Boolean
    Public PSinIva As Boolean
    Public PLote As Integer
    Public PSecuencia As Integer
    Public PRegresar As Boolean
    Private Sub OpcionInformeComprobantesQueAfectaALotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Dim Row As DataRow = ComboEmisor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboEmisor.DataSource.rows.add(Row)
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = 0
        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '';")
        Row = ComboAlias.DataSource.NewRow
        Row("Clave") = 0
        Row("Alias") = ""
        ComboAlias.DataSource.rows.add(Row)
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        PRegresar = True

        If Not PermisoTotal Then Panel1.Visible = False : PAbierto = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ComboEmisor.SelectedValue <> 0 Then
            PProveedor = ComboEmisor.SelectedValue
        Else
            PProveedor = ComboAlias.SelectedValue
        End If

        PDesde = DateTimeDesde.Value
        PHasta = DateTimeHasta.Value

        PCosteo = ComboCosteo.SelectedValue
        PAbierto = CheckAbierto.Checked
        PCerrado = CheckCerrado.Checked
        PConIva = RadioConIva.Checked
        PSinIva = RadioSinIva.Checked

        PLote = 0
        PSecuencia = 0
        If Val(MaskedLote.Text) <> 0 Then
            ConsisteMaskedLote(MaskedLote.Text, PLote, PSecuencia)
        End If

        PRegresar = False

        Me.Close()

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        VerSiTieneCosteo(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        VerSiTieneCosteo(ComboEmisor.SelectedValue)

    End Sub
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub VerSiTieneCosteo(ByVal Emisor As Integer)

        If Emisor = 0 Then
            ComboCosteo.DataSource = Nothing
            Exit Sub
        End If

        If Not EsUnNegocio(Emisor) Then
            ComboCosteo.DataSource = Nothing
            Exit Sub
        End If

        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & Emisor & ";"
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
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Falta Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Seleccionar Proveedor o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            MsgBox("Debe Seleccionar Candado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            CheckAbierto.Focus()
            Return False
        End If

        Dim Lote As Integer = 0
        Dim Secuencia As Integer = 0
        If Val(MaskedLote.Text) <> 0 Then
            If Not ConsisteMaskedLote(MaskedLote.Text, Lote, Secuencia) Then Return False
        End If

        Return True

    End Function

   
End Class