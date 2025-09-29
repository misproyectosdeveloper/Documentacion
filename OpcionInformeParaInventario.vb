Public Class OpcionInformeParaInventario
    Public PEspecie As Integer
    Public PVariedad As Integer
    Public PMarca As Integer
    Public PCategoria As Integer
    Public PEnvase As Integer
    Public PDeposito As Integer
    Public PDesde As Date
    Public PHasta As Date
    Public PRegresar As Boolean
    Public PDescartes As Boolean
    Public PEsInventario As Boolean
    Public PPedidos As Boolean
    Public PListaComprobantes As List(Of Decimal)
    Public PListaComprobantesCerrado As List(Of Decimal)
    Private Sub OpcionInformeParaInventario_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LlenaComboTablas(ComboEspecie, 1)
        With ComboEspecie
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboEspecie.SelectedValue = 0

        LlenaComboTablas(ComboVariedad, 2)
        With ComboVariedad
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboVariedad.SelectedValue = 0

        LlenaComboTablas(ComboMarca, 3)
        With ComboMarca
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboMarca.SelectedValue = 0

        LlenaComboTablas(ComboCategoria, 4)
        With ComboCategoria
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboCategoria.SelectedValue = 0

        LLenaComboEnvases(ComboEnvase)
        With ComboEnvase
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboEnvase.SelectedValue = 0

        If Not PDescartes Then
            LlenaComboTablas(ComboDeposito, 19)
            ComboDeposito.SelectedValue = 0
            With ComboDeposito
                .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                .AutoCompleteSource = AutoCompleteSource.ListItems
            End With
            ComboDeposito.SelectedValue = 0
        End If

        If PDescartes Then
            ComboDeposito.Visible = False
            Label6.Visible = False
            Panel1.Visible = True
        Else
            Panel1.Visible = False
        End If

        If PEsInventario Then
            PanelComprobantes.Visible = True
            If PermisoTotal Then
                PanelComprobantesCerrado.Visible = True
            End If
            CheckPedidos.Visible = True
        Else
            PanelComprobantes.Visible = False
            PanelComprobantesCerrado.Visible = False
            CheckPedidos.Visible = False
        End If

        PRegresar = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PListaComprobantes = New List(Of Decimal)
        PListaComprobantesCerrado = New List(Of Decimal)

        If Not ValidaTextBoxComprobantes(TextComprobantes, PListaComprobantes) Then Exit Sub
        If Not ValidaTextBoxComprobantes(TextComprobantesCerrado, PListaComprobantesCerrado) Then Exit Sub
        If Not ValidaComprobantes() Then Exit Sub

        PEspecie = ComboEspecie.SelectedValue
        PVariedad = ComboVariedad.SelectedValue
        PMarca = ComboMarca.SelectedValue
        PCategoria = ComboCategoria.SelectedValue
        PEnvase = ComboEnvase.SelectedValue
        PDeposito = ComboDeposito.SelectedValue
        PDesde = DateTimeDesde.Value
        PHasta = DateTimeHasta.Value
        PPedidos = CheckPedidos.Checked
        PRegresar = False

        Me.Close()

    End Sub
    Private Sub ComboEspecie_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEspecie.Validating

        If IsNothing(ComboEspecie.SelectedValue) Then ComboEspecie.SelectedValue = 0

    End Sub
    Private Sub ComboVariedad_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboVariedad.Validating

        If IsNothing(ComboVariedad.SelectedValue) Then ComboVariedad.SelectedValue = 0

    End Sub
    Private Sub ComboMarca_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboMarca.Validating

        If IsNothing(ComboMarca.SelectedValue) Then ComboMarca.SelectedValue = 0

    End Sub
    Private Sub ComboCategoria_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCategoria.Validating

        If IsNothing(ComboCategoria.SelectedValue) Then ComboCategoria.SelectedValue = 0

    End Sub
    Private Sub ComboEnvase_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEnvase.Validating

        If IsNothing(ComboEnvase.SelectedValue) Then ComboEnvase.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub TextComprobantes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComprobantes.KeyPress

        If Asc(e.KeyChar) = 13 Then Exit Sub
        e.KeyChar = e.KeyChar.ToString.ToUpper
        If InStr("ABCET0123456789-" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextComprobantesCerrado_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComprobantesCerrado.KeyPress

        If Asc(e.KeyChar) = 13 Then Exit Sub
        e.KeyChar = e.KeyChar.ToString.ToUpper
        If InStr("ABCET0123456789-" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Function ValidaComprobantes() As Boolean

        For Each Fila As Decimal In PListaComprobantes
            If Fila.ToString.Length > 12 Then
                If Not ExisteFactura(Fila, Conexion) Then
                    MsgBox("Factura " & NumeroEditado(Fila) & " No Existe.")
                    Return False
                End If
            End If
            If Fila.ToString.Length <= 12 Then
                If Not ExisteRemito(Fila, Conexion) Then
                    MsgBox("Remito " & NumeroEditado(Fila) & " No Existe.")
                    Return False
                End If
            End If
        Next
        For Each Fila As Decimal In PListaComprobantesCerrado
            If Fila.ToString.Length > 12 Then
                If Not ExisteFactura(Fila, ConexionN) Then
                    MsgBox("Factura " & NumeroEditado(Fila) & " No Existe.")
                    Return False
                End If
            End If
            If Fila.ToString.Length <= 12 Then
                If Not ExisteRemito(Fila, ConexionN) Then
                    MsgBox("Remito " & NumeroEditado(Fila) & " No Existe.")
                    Return False
                End If
            End If
        Next

        For Each Fila As Decimal In PListaComprobantesCerrado
            If Fila.ToString.Length > 12 Then
                Dim Relacionada As Decimal = HallaFacturaRelacionada(Fila, ConexionN)
                For Each FilaW As Decimal In PListaComprobantes
                    If FilaW = Relacionada Then
                        MsgBox("Factura " & NumeroEditado(Fila) & " Y " & NumeroEditado(FilaW) & " Son Relacionadas. Debe borrar una de ellas.")
                        Return False
                    End If
                Next
            End If
        Next

        Return True

    End Function

End Class