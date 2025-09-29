Imports System.Drawing.Printing
Public Class Opcionfechas
    Public PDesde As DateTime
    Public PHasta As DateTime
    Public PFecha As DateTime
    Public PAbierto As Boolean
    Public PCerrado As Boolean
    Public PProveedor As Integer
    Public PCliente As Integer
    Public PTitulo As String
    Public PRegresa As Boolean
    Public PEsConProveedorLote As Boolean
    Public PEsEgresoCaja As Boolean
    Public PEsSeniasValesPropios As Boolean
    Public PEsSoloFecha As Boolean
    Private Sub Opcionfechas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Label1.Text = PTitulo

        If PEsConProveedorLote Then
            Panel1.Visible = True
            LabelEmisor.Text = "Proveedor Lote"
            LlenaCombo(ComboEmisor, "", "Proveedores")
            ComboEmisor.SelectedValue = 0
            '
            ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '';")
            Dim Row As DataRow = ComboAlias.DataSource.newrow
            Row("Clave") = 0
            Row("Alias") = ""
            ComboAlias.DataSource.rows.add(Row)
            ComboAlias.DisplayMember = "Alias"
            ComboAlias.ValueMember = "Clave"
            ComboAlias.SelectedValue = 0
        End If
        If PEsSeniasValesPropios Then
            Panel1.Visible = True
            LabelEmisor.Text = "Cliente"
            LlenaCombo(ComboEmisor, "", "Clientes")
            ComboEmisor.SelectedValue = 0
            '
            ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Clientes WHERE Alias <> '';")
            Dim Row As DataRow = ComboAlias.DataSource.newrow
            Row("Clave") = 0
            Row("Alias") = ""
            ComboAlias.DataSource.rows.add(Row)
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

        If PEsEgresoCaja Or PEsSeniasValesPropios Then PanelCandado.Visible = True

        If Not PermisoTotal Then PanelCandado.Visible = False : PAbierto = True

        TextFecha.Text = ""
        If PEsSoloFecha Then
            Panel3.Visible = True
            Panel2.Visible = False
        Else
            Panel3.Visible = False
        End If

        PRegresa = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If PEsConProveedorLote Or PEsSeniasValesPropios Then
            If ComboEmisor.SelectedValue <> 0 Then PProveedor = ComboEmisor.SelectedValue : PCliente = ComboEmisor.SelectedValue
            If ComboAlias.SelectedValue <> 0 Then PProveedor = ComboAlias.SelectedValue : PCliente = ComboAlias.SelectedValue
        End If

        PDesde = Format(DateTimeDesde.Value, "dd/MM/yyyy")
        PHasta = Format(DateTimeHasta.Value, "dd/MM/yyyy")
        PAbierto = CheckAbierto.Checked
        PCerrado = CheckCerrado.Checked
        If PEsSoloFecha Then PFecha = CDate(TextFecha.Text)

        PRegresa = False

        Me.Close()

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaque.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFecha.Text = ""
        Else : TextFecha.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Function Valida() As Boolean

        If PEsSoloFecha Then
            If TextFecha.Text = "" Then
                MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                TextFecha.Focus()
                Return False
            End If
            If Not ConsisteFecha(TextFecha.Text) Then
                MsgBox("Fecha Contable Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFecha.Focus()
                Return False
            End If
        End If

        If PEsConProveedorLote Then
            If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
                MsgBox("Debe Informar Proveedor o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Return False
            End If
        End If

        If PEsSeniasValesPropios Then
            If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
                MsgBox("Debe Informar Cliente o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Return False
            End If
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            MsgBox("Debe Seleccionar Candado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            CheckAbierto.Focus()
            Return False
        End If

        Return True

    End Function
End Class