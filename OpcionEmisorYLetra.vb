Public Class OpcionEmisorYLetra
    Public PEsProveedor As Boolean
    Public PEsCliente As Boolean
    Public PSoloInsumos As Boolean
    Public PSoloArticulos As Boolean
    Public PEsExterior As Boolean
    Public PEsLocal As Boolean
    Public PEsNegocio As Boolean
    Public PEsNoNegocio As Boolean
    Public PEsSinLetra As Boolean
    Public PEsFacturaProveedor As Boolean
    Public PEsConDeposito As Boolean
    Public PEsSoloAltas As Boolean
    Public PNumeroLetra As Integer
    Public PEmisor As Integer
    Public PDeposito As Integer
    Public PAbierto As Boolean
    '
    Dim Dt As New DataTable
    Dim DtAlias As New DataTable
    Private Sub OpcionEmisorYLetra_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Borrar As Boolean

        If PEsProveedor Then
            Dt = Tablas.Leer("SELECT Clave,Nombre,Producto,TipoIva,TipoOperacion,Estado FROM Proveedores ORDER BY Nombre;")
            DtAlias = Tablas.Leer("SELECT Clave,Alias,Producto,TipoIva,TipoOperacion FROM Proveedores WHERE Alias <> '' ORDER BY Alias;")
        Else
            Dt = Tablas.Leer("SELECT Clave,Nombre,0 AS Producto,TipoIva,0 AS TipoOperacion,Estado FROM Clientes ORDER BY Nombre;")
            DtAlias = Tablas.Leer("SELECT Clave,Alias,0 AS Producto,TipoIva,0 AS TipoOperacion FROM Clientes WHERE Alias <> '' ORDER BY Alias;")
        End If

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Row("Producto") = 0
        Row("TipoIva") = 0
        Row("TipoOperacion") = 0
        Dt.Rows.Add(Row)

        Row = DtAlias.NewRow
        Row("Clave") = 0
        Row("Alias") = ""
        Row("Producto") = 0
        Row("TipoIva") = 0
        Row("TipoOperacion") = 0
        DtAlias.Rows.Add(Row)

        For i As Integer = Dt.Rows.Count - 1 To 0 Step -1
            Borrar = False
            Row = Dt.Rows(i)
            If PSoloInsumos And Row("Producto") <> Insumo Then Borrar = True
            If PSoloArticulos And Row("Producto") <> Fruta Then Borrar = True
            If PEsExterior And Row("TipoIva") <> Exterior Then Borrar = True
            If PEsLocal And Row("TipoIva") = Exterior Then Borrar = True
            If PEsNoNegocio And Row("TipoOperacion") = 4 Then Borrar = True
            If PEsNegocio And Row("TipoOperacion") <> 4 Then Borrar = True
            If Borrar Then Row.Delete()
        Next

        For i As Integer = DtAlias.Rows.Count - 1 To 0 Step -1
            Borrar = False
            Row = DtAlias.Rows(i)
            If PSoloInsumos And Row("Producto") <> Insumo Then Borrar = True
            If PSoloArticulos And Row("Producto") <> Fruta Then Borrar = True
            If PEsExterior And Row("TipoIva") <> Exterior Then Borrar = True
            If PEsLocal And Row("TipoIva") = Exterior Then Borrar = True
            If PEsNoNegocio And Row("TipoOperacion") = 4 Then Borrar = True
            If PEsNegocio And Row("TipoOperacion") <> 4 Then Borrar = True
            If Borrar Then Row.Delete()
        Next

        ComboEmisor.DataSource = Dt
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = 0
        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = DtAlias
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0
        With ComboAlias
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

        PEmisor = 0
        PDeposito = 0

        If Not PEsSinLetra Then
            TextTipoFactura.ReadOnly = True
        End If

        If PEsSinLetra Then
            Label1.Visible = False
            TextTipoFactura.Visible = False
        End If
        If PEsConDeposito Then
            Panel1.Visible = True
        End If
        If Not PermisoTotal Then
            PictureCandado.Visible = False
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ComboEmisor.SelectedValue <> 0 Then PEmisor = ComboEmisor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then PEmisor = ComboAlias.SelectedValue

        If Not PEsSinLetra Then
            PNumeroLetra = HallaNumeroLetra(TextTipoFactura.Text)
        End If

        PDeposito = ComboDeposito.SelectedValue

        Me.Close()

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        If PEsProveedor And ComboEmisor.SelectedValue <> 0 And ComboEmisor.SelectedValue = GProveedorEgresoCaja Then
            MsgBox("Proveedor Egreso de Caja Inválida pera este Comprobante.", MsgBoxStyle.Information)
            ComboEmisor.SelectedValue = 0
            Exit Sub
        End If

        If Not PEsSinLetra And PEsFacturaProveedor Then
            TextTipoFactura.Text = LetraTipoIva(LetrasPermitidasProveedor(HallaTipoIvaProveedor(ComboEmisor.SelectedValue), 5000))
        End If

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0
        If ComboAlias.SelectedValue = 0 Then Exit Sub

        If PEsProveedor And ComboAlias.SelectedValue <> 0 And ComboAlias.SelectedValue = GProveedorEgresoCaja Then
            MsgBox("Proveedor Egreso de Caja Inválida pera este Comprobante.", MsgBoxStyle.Information)
            ComboAlias.SelectedValue = 0
            Exit Sub
        End If

        If Not PEsSinLetra And PEsFacturaProveedor Then
            TextTipoFactura.Text = LetraTipoIva(LetrasPermitidasProveedor(HallaTipoIvaProveedor(ComboAlias.SelectedValue), 5000))
        End If

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = UnRemito.ImageList1.Images.Item("Cerrado")
            PAbierto = False
        Else : PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

    End Sub
    Private Function Valida() As Boolean

        If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue = 0 Then
            MsgBox("Falta Razon Social.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If
        If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Razon Social o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If Not PEsSinLetra Then
            If TextTipoFactura.Text = "" Then
                MsgBox("Falta Informar Letra.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextTipoFactura.Focus()
                Return False
            End If
            If TextTipoFactura.Text <> "A" And TextTipoFactura.Text <> "B" And TextTipoFactura.Text <> "C" And TextTipoFactura.Text <> "E" And TextTipoFactura.Text <> "M" Then
                MsgBox("Letra Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextTipoFactura.Focus()
                Return False
            End If
        End If

        If PEsSoloAltas Then
            Dim Emisor As Integer
            If ComboEmisor.SelectedValue <> 0 Then Emisor = ComboEmisor.SelectedValue
            If ComboAlias.SelectedValue <> 0 Then Emisor = ComboAlias.SelectedValue
            '
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = Dt.Select("Clave = " & Emisor)
            If RowsBusqueda(0).Item("Estado") = 3 Then
                If PEsProveedor Then
                    MsgBox("Proveedor Esta dado de Baja. Debe cambiar estado en : " + vbCrLf + "Menu --> Lista Proveedores--> Proveedor --> Estado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End If
                If PEsCliente Then
                    MsgBox("Cliente Esta dado de Baja. Debe cambiar estado en : " + vbCrLf + "Menu --> Lista Clientes--> Cliente --> Estado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End If
                Return False
            End If
        End If


        Return True

    End Function
End Class