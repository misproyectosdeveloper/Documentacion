Public Class OpcionVentasYCostosDeLotes
    Public PDesde As Date
    Public PHasta As Date
    Public PAbierto As Boolean
    Public PCerrado As Boolean
    Public PPuntoDeVenta As Integer
    Public PEsExportacion As Boolean
    Public PEsDomestica As Boolean
    Public PEsFacturasCobranzas As Boolean
    Public PVendedor As Integer
    Public PCliente As Integer
    Public PEsCompraVentaFactura As Boolean
    Public PRegresar As Boolean
    Private Sub OpcionVentasYCostosDeLotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboPuntoDeVenta.DataSource = Tablas.Leer("SELECT Clave,RIGHT('0000' + CAST(Clave AS varchar),4) as Nombre FROM PuntosDeVenta WHERE Clave > 0 ORDER BY Nombre;")
        Dim Row As DataRow = ComboPuntoDeVenta.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboPuntoDeVenta.DataSource.rows.add(Row)
        ComboPuntoDeVenta.DisplayMember = "Nombre"
        ComboPuntoDeVenta.ValueMember = "Clave"
        ComboPuntoDeVenta.SelectedValue = 0
        With ComboPuntoDeVenta
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboVendedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 37 ORDER BY Nombre;")
        Row = ComboVendedor.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboVendedor.DataSource.rows.add(Row)
        ComboVendedor.DisplayMember = "Nombre"
        ComboVendedor.ValueMember = "Clave"
        ComboVendedor.SelectedValue = 0
        With ComboVendedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboCliente, "", "Clientes")
        ComboCliente.SelectedValue = 0
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Clientes WHERE Alias <> '';")
        Dim Fila As DataRow = ComboAlias.DataSource.NewRow
        Fila("Clave") = 0
        Fila("Alias") = ""
        ComboAlias.DataSource.Rows.Add(Fila)
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not PermisoTotal Then Panel1.Visible = False

        PRegresar = True
        PEsExportacion = True
        PEsDomestica = True
        If PEsFacturasCobranzas Then Panel4.Visible = True : Panel3.Visible = True : CheckExportacion.Checked = False : CheckExportacion.Enabled = False : CheckDomestica.Enabled = False : PanelCliente.Visible = False

        If PEsCompraVentaFactura Then PanelCliente.Visible = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If ComboCliente.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Cliente o Alias.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            MsgBox("Debe Informar Candado Abierto o Cerrado.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not CheckExportacion.Checked And Not CheckDomestica.Checked Then
            MsgBox("Debe Informar Si Es Exportación o Domestica.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PDesde = DateTimeDesde.Value
        PHasta = DateTimeHasta.Value
        PAbierto = CheckAbierto.Checked
        PCerrado = CheckCerrado.Checked
        PEsExportacion = CheckExportacion.Checked
        PEsDomestica = CheckDomestica.Checked
        PVendedor = ComboVendedor.SelectedValue
        PPuntoDeVenta = ComboPuntoDeVenta.SelectedValue

        If ComboCliente.SelectedValue <> 0 Then PCliente = ComboCliente.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then PCliente = ComboAlias.SelectedValue

        PRegresar = False
        Me.Close()

    End Sub
    Private Sub ComboPuntoDeVenta_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVenta.Validating

        If IsNothing(ComboPuntoDeVenta.SelectedValue) Then ComboPuntoDeVenta.SelectedValue = 0

    End Sub
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

    End Sub
End Class