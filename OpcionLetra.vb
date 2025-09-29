Public Class OpcionLetra
    Public PEmisor As Integer
    Public PEmisorBloqueado As Integer
    Public PEsProveedor As Boolean
    Public PNumeroLetra As Integer
    Public PSoloInsumos As Boolean
    Public PSoloArticulos As Boolean
    Public PEsImportacion As Boolean
    Public PEsLocalYImportacion As Boolean
    Public PEsSinLetra As Boolean
    Public PEsNVLP As Boolean
    Public PTipoNota As Integer
    Public PAbierto As Boolean
    Private Sub OpcionFacturaProveedores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If PEsProveedor And PSoloInsumos Then
            If PEsLocalYImportacion Then
                ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Opr FROM Proveedores WHERE Producto = " & Insumo & " ORDER BY Nombre;")
                ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias,Opr   FROM Proveedores WHERE Alias <> '' AND Producto = " & Insumo & " ORDER BY Alias;")
            Else
                If PEsImportacion Then
                    ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Opr FROM Proveedores WHERE TipoIva = " & Exterior & " AND Producto = " & Insumo & " ORDER BY Nombre;")
                    ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias,Opr   FROM Proveedores WHERE Alias <> '' AND TipoIva = " & Exterior & " AND Producto = " & Insumo & " ORDER BY Alias;")
                Else
                    ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Opr FROM Proveedores WHERE TipoIva <> " & Exterior & " AND Producto = " & Insumo & " ORDER BY Nombre;")
                    ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias,Opr   FROM Proveedores WHERE Alias <> '' AND TipoIva <> " & Exterior & " AND Producto = " & Insumo & " ORDER BY Alias;")
                End If
                Dim Row As DataRow = ComboEmisor.DataSource.NewRow
                Row("Clave") = 0
                Row("Nombre") = ""
                ComboEmisor.DataSource.Rows.Add(Row)
                Row = ComboAlias.DataSource.NewRow
                Row("Clave") = 0
                Row("Alias") = ""
                ComboAlias.DataSource.Rows.Add(Row)
            End If
        End If

        If PEsProveedor And PSoloArticulos Then
            If PEsLocalYImportacion Then
                ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Opr FROM Proveedores WHERE TipoOperacion <> 4 AND Producto = " & Fruta & " ORDER BY Nombre;")
                ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias,Opr   FROM Proveedores WHERE Alias <> '' AND TipoOperacion <> 4 AND Producto = " & Fruta & " ORDER BY Alias;")
            Else
                If PEsImportacion Then
                    ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Opr FROM Proveedores WHERE TipoIva = " & Exterior & " AND TipoOperacion <> 4 AND Producto = " & Fruta & " ORDER BY Nombre;")
                    ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias,Opr   FROM Proveedores WHERE Alias <> '' AND TipoIva = " & Exterior & " AND TipoOperacion <> 4 AND Producto = " & Fruta & " ORDER BY Alias;")
                Else
                    ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Opr FROM Proveedores WHERE TipoIva <> " & Exterior & " AND TipoOperacion <> 4 AND Producto = " & Fruta & " ORDER BY Nombre;")
                    ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias,Opr   FROM Proveedores WHERE Alias <> '' AND TipoIva <> " & Exterior & " AND TipoOperacion <> 4 AND Producto = " & Fruta & " ORDER BY Alias;")
                End If
                Dim Row As DataRow = ComboEmisor.DataSource.NewRow
                Row("Clave") = 0
                Row("Nombre") = ""
                ComboEmisor.DataSource.Rows.Add(Row)
                Row = ComboAlias.DataSource.NewRow
                Row("Clave") = 0
                Row("Alias") = ""
                ComboAlias.DataSource.Rows.Add(Row)
            End If
        End If

        If PEsProveedor And Not PSoloArticulos And Not PSoloInsumos Then
            If PEsLocalYImportacion Then
                ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Opr FROM Proveedores WHERE TipoOperacion <> 4;")
                ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias,Opr   FROM Proveedores WHERE Alias <> '' AND TipoOperacion <> 4;")
            Else
                If PEsImportacion Then
                    ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Opr FROM Proveedores WHERE TipoIva = " & Exterior & " AND TipoOperacion <> 4;")
                    ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias,Opr   FROM Proveedores WHERE Alias <> '' AND TipoIva = " & Exterior & " AND TipoOperacion <> 4;")
                Else
                    ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Opr FROM Proveedores WHERE TipoIva <> " & Exterior & " AND TipoOperacion <> 4;")
                    ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias,Opr   FROM Proveedores WHERE Alias <> '' AND TipoIva <> " & Exterior & " AND TipoOperacion <> 4;")
                End If
            End If
        End If

        If Not PEsProveedor Then
            ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Opr FROM Clientes WHERE TipoIva <> 4;")
            ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias,Opr   FROM Clientes WHERE Alias <> '' AND TipoIva <> 4;")
        End If

        If Not PermisoTotal Then    'Saca Candado cerrado.
            For Each Row As DataRow In ComboEmisor.DataSource.rows
                If Not Row("Opr") Then Row.Delete()
            Next
            For Each Row As DataRow In ComboAlias.DataSource.rows
                If Not Row("Opr") Then Row.Delete()
            Next
        End If

        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = 0
        With ComboEmisor
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

        PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
        PAbierto = True
        If Not PermisoTotal Then
            PictureCandado.Visible = False
        End If

        PEmisor = 0

        If PEsSinLetra Then
            Label1.Visible = False
            TextTipoFactura.Visible = False
        End If

        If PEsNVLP Then
            ' TextTipoFactura.ReadOnly = True   Activar cuando este probado.
        End If

        Select Case PTipoNota
            Case 50, 70
                '    TextTipoFactura.ReadOnly = True    Activar cuando este probado.
            Case 500, 700
                '   TextTipoFactura.ReadOnly = True     Activar cuando este probado.
        End Select

        If PEmisorBloqueado <> 0 Then
            ComboEmisor.Enabled = False
            ComboAlias.Enabled = False
            ComboEmisor.SelectedValue = PEmisorBloqueado
            TextTipoFactura.Text = LetraTipoIva(LetrasPermitidasProveedor(HallaTipoIvaProveedor(ComboEmisor.SelectedValue), PTipoNota))
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ComboEmisor.SelectedValue <> 0 Then PEmisor = ComboEmisor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then PEmisor = ComboAlias.SelectedValue

        If Not PEsSinLetra Then
            PNumeroLetra = HallaNumeroLetra(TextTipoFactura.Text)
        End If

        Me.Close()

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        If PEsProveedor And ComboEmisor.SelectedValue <> 0 And ComboEmisor.SelectedValue = GProveedorEgresoCaja Then
            MsgBox("Proveedor Egreso de Caja Inválida pera este Comprobante." + vbCrLf + "Orden Pago por Egreso de Caja se debe Borrar y Generar Nuevamente.", MsgBoxStyle.Information)
            ComboEmisor.SelectedValue = 0
            Exit Sub
        End If

        If PEsNVLP Then
            TextTipoFactura.Text = LetraTipoIva(LetrasPermitidasCliente(HallaTipoIvaCliente(ComboEmisor.SelectedValue), 800))
        End If
        Select Case PTipoNota
            Case 50, 70
                TextTipoFactura.Text = LetraTipoIva(LetrasPermitidasCliente(HallaTipoIvaCliente(ComboEmisor.SelectedValue), PTipoNota))
            Case 500, 700
                TextTipoFactura.Text = LetraTipoIva(LetrasPermitidasProveedor(HallaTipoIvaProveedor(ComboEmisor.SelectedValue), PTipoNota))
        End Select

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        If PEsProveedor And ComboAlias.SelectedValue <> 0 And ComboAlias.SelectedValue = GProveedorEgresoCaja Then
            MsgBox("Proveedor Egreso de Caja Inválida pera este Comprobante." + vbCrLf + "Orden Pago por Egreso de Caja se debe Borrar y Generar Nuevamente.", MsgBoxStyle.Information)
            ComboAlias.SelectedValue = 0
            Exit Sub
        End If

        If PEsNVLP Then
            TextTipoFactura.Text = LetraTipoIva(LetrasPermitidasCliente(HallaTipoIvaCliente(ComboAlias.SelectedValue), 800))
        End If
        Select Case PTipoNota
            Case 50, 70
                TextTipoFactura.Text = LetraTipoIva(LetrasPermitidasCliente(HallaTipoIvaCliente(ComboAlias.SelectedValue), PTipoNota))
            Case 500, 700
                TextTipoFactura.Text = LetraTipoIva(LetrasPermitidasProveedor(HallaTipoIvaProveedor(ComboAlias.SelectedValue), PTipoNota))
        End Select

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
            If PEsNVLP Then
                If TextTipoFactura.Text = "E" Then
                    MsgBox("Letra No permitida para N.V.L.P.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
        End If

        Dim RowsBusqueda() As DataRow
        Dim EmisorW As Integer
        If ComboEmisor.SelectedValue <> 0 Then EmisorW = ComboEmisor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then EmisorW = ComboAlias.SelectedValue

        Return True

    End Function

End Class