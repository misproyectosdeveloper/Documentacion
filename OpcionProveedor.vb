Public Class OpcionProveedor
    Public PProveedor As Integer
    Public PNombreProveedor As String
    Public PAbierto As Boolean
    Public PSoloInsumos As Boolean
    Public PSoloArticulos As Boolean
    Public PSoloNegocios As Boolean
    Public PEsOrdenCompra As Boolean
    Public PEsFacturaProveedor As Boolean
    Public PEsPreLiquidacion As Boolean
    Public PSoloNacional As Boolean
    Private Sub OpcionProveedor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If PSoloInsumos Then
            ComboProveedor.DataSource = ProveedoresDeInsumos()
            ComboAlias.DataSource = ProveedoresDeInsumosAlias()
            Me.BackColor = Color.Thistle
        Else
            If PSoloArticulos Then
                If PSoloNacional Then
                    ComboProveedor.DataSource = ProveedoresDeFrutasNacional()
                    ComboAlias.DataSource = ProveedoresDeFrutasAliasNacional()
                Else
                    ComboProveedor.DataSource = ProveedoresDeFrutas()
                    ComboAlias.DataSource = ProveedoresDeFrutasAlias()
                End If
            Else
                If PSoloNegocios Then
                    ComboProveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;")
                    ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '' AND TipoOperacion = 4 ORDER BY Alias;")
                    Dim Row As DataRow = ComboProveedor.DataSource.newrow
                    Row("Clave") = 0
                    Row("Nombre") = ""
                    ComboProveedor.DataSource.rows.add(Row)
                    Row = ComboAlias.DataSource.newrow
                    Row("Clave") = 0
                    Row("Alias") = ""
                    ComboAlias.DataSource.rows.add(Row)
                Else
                    LlenaCombo(ComboProveedor, "", "Proveedores")
                    ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '';")
                    Dim Row As DataRow = ComboAlias.DataSource.newrow
                    Row("Clave") = 0
                    Row("Alias") = ""
                    ComboAlias.DataSource.rows.add(Row)
                End If
            End If
        End If
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PermisoTotal And Not PEsFacturaProveedor And Not PEsPreLiquidacion Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If
        If PSoloNegocios Then
            PictureCandado.Visible = False
            LabelEmisor.Text = "Negocio"
        End If

        If PEsOrdenCompra Then PictureCandado.Visible = False

        PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
        PAbierto = True

        PProveedor = 0

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ComboProveedor.SelectedValue <> 0 Then
            PProveedor = ComboProveedor.SelectedValue
            PNombreProveedor = ComboProveedor.Text
        Else
            PProveedor = ComboAlias.SelectedValue
            PNombreProveedor = NombreProveedor(ComboAlias.SelectedValue)
        End If

        Me.Close()

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

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

        If ComboProveedor.SelectedValue = 0 And ComboAlias.SelectedValue = 0 Then
            MsgBox("Falta Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboProveedor.Focus()
            Return False
        End If
        If ComboProveedor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Proveedor o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboProveedor.Focus()
            Return False
        End If

        Return True

    End Function

   
End Class