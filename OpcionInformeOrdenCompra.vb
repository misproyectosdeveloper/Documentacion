Public Class OpcionInformeOrdenCompra
    Public PProveedor As Integer
    Public POrdenCompra As Integer
    Public PFechaDesde As Date
    Public PFechaHasta As Date
    Public PPeriodoDesde As Date
    Public PPeriodoHasta As Date
    Public PEstado As Integer
    Public PRegresar As Boolean
    Private Sub OpcionSeguimientoOrdenesCompras_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboProveedor.DataSource = ProveedoresDeInsumos()
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0

        ComboAlias.DataSource = ProveedoresDeInsumosAlias()
        ComboAlias.DisplayMember = "Nombre"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0

        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        With ComboAlias
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

        PRegresar = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ComboProveedor.SelectedValue <> 0 Then
            PProveedor = ComboProveedor.SelectedValue
        Else
            PProveedor = ComboAlias.SelectedValue
        End If

        If TextOrdenCompra.Text <> "" Then
            POrdenCompra = TextOrdenCompra.Text
        Else
            POrdenCompra = 0
        End If
        PFechaDesde = FechaDesde.Value
        PFechaHasta = FechaHasta.Value
        If TextPeriodoDesde.Text <> "" Then
            PPeriodoDesde = CDate(TextPeriodoDesde.Text)
            PPeriodoHasta = CDate(TextPeriodoHasta.Text)
        Else
            PPeriodoDesde = "01/01/1800"
            PPeriodoHasta = "01/01/1800"
        End If
        PEstado = ComboEstado.SelectedValue

        PRegresar = False
        Me.Close()

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub TextNumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextOrdenCompra.KeyPress

        EsNumerico(e.KeyChar, TextOrdenCompra.Text, 0)

    End Sub
    Private Sub PictureAlmanaquePeriodoDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaquePeriodoDesde.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextPeriodoDesde.Text = ""
        Else : TextPeriodoDesde.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaquePeriodoHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaquePeriodoHasta.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextPeriodoHasta.Text = ""
        Else : TextPeriodoHasta.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Function Valida() As Boolean

        If ComboProveedor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Proveedor o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboProveedor.Focus()
            Return False
        End If

        If DiferenciaDias(FechaDesde.Value, FechaHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TextPeriodoDesde.Text <> "" Or TextPeriodoHasta.Text <> "" Then
            If TextPeriodoDesde.Text = "" Then
                MsgBox("Falta Informar Periodo Desde.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextPeriodoDesde.Focus()
                Return False
            End If
            If TextPeriodoHasta.Text = "" Then
                MsgBox("Falta Informar Periodo Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextPeriodoHasta.Focus()
                Return False
            End If
            If Not ConsisteFecha(TextPeriodoDesde.Text) Then
                MsgBox("Fecha Periodo Desde Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextPeriodoDesde.Focus()
                Return False
            End If
            If Not ConsisteFecha(TextPeriodoHasta.Text) Then
                MsgBox("Fecha Periodo Hasta Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextPeriodoHasta.Focus()
                Return False
            End If
            If DiferenciaDias(CDate(TextPeriodoDesde.Text), CDate(TextPeriodoHasta.Text)) < 0 Then
                MsgBox("Periodo Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextPeriodoDesde.Focus()
                Return False
            End If
        End If

        Return True

    End Function


End Class