Public Class OpcionEmisor
    Public PEsProveedor As Boolean
    Public PEsCliente As Boolean
    Public PEsVendedor As Boolean
    Public PEsSinCandado As Boolean
    Public PEsPorCuentayOrden As Boolean
    Public PEsTr As Boolean
    Public PPuedeTenerFCE As Boolean
    Public PEsListaDePrecios As Boolean
    Public PEsNuevaListaDePreciosVenta As Boolean
    Public PEsPorDiferenciaCambio As Boolean
    Public PEsEgresoCaja As Boolean
    Public PEsSoloAltas As Boolean
    Public PTipoNota As Integer
    Public PListaDiaria As Boolean
    Public PEmisor As Integer
    Public PNombre As String
    Public PACuenta As Integer
    Public PNumero As Integer
    Public PAbierto As Boolean
    'Para FCE.
    Public PEsFCE As Boolean
    Public PFacturaFCE As Decimal
    Private Sub OpcionEmisor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim TipoIvaStr As String

        If PEsCliente Then
            If PEsPorDiferenciaCambio Then
                TipoIvaStr = "TipoIva = 4 AND"
            Else : TipoIvaStr = ""
            End If
        End If

        If PEsProveedor Then
            If PEsPorDiferenciaCambio Then
                TipoIvaStr = "TipoIva = 4 AND"
            Else : TipoIvaStr = ""
            End If
        End If

        If PEsCliente Then
            LabelEmisor.Text = "Cliente" : Me.Text = "Opcion Cliente"
            ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Estado,EsFCE FROM Clientes WHERE " & TipoIvaStr & " DeOperacion = 0 ORDER BY Nombre;")
            Dim Row As DataRow = ComboEmisor.DataSource.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            ComboEmisor.DataSource.rows.add(Row)
            ComboEmisor.DisplayMember = "Nombre"
            ComboEmisor.ValueMember = "Clave"
            ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias,EsFCE FROM Clientes WHERE " & TipoIvaStr & " DeOperacion = 0 AND Alias <> '' ORDER BY Alias;")
            Row = ComboAlias.DataSource.NewRow
            Row("Clave") = 0
            Row("Alias") = ""
            ComboAlias.DataSource.rows.add(Row)
            ComboAlias.DisplayMember = "Alias"
            ComboAlias.ValueMember = "Clave"
        End If
        If PEsProveedor And Not PEsEgresoCaja Then
            LabelEmisor.Text = "Proveedor" : Me.Text = "Opcion Proveedor"
            ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Estado FROM Proveedores WHERE " & TipoIvaStr & " TipoOperacion <> 4 ORDER BY Nombre;")
            ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE " & TipoIvaStr & " Alias <> '' AND TipoOperacion <> 4 ORDER BY Alias;")
            Dim Row As DataRow = ComboEmisor.DataSource.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            ComboEmisor.DataSource.rows.add(Row)
            ComboEmisor.DisplayMember = "Nombre"
            ComboEmisor.ValueMember = "Clave"
            Row = ComboAlias.DataSource.NewRow
            Row("Clave") = 0
            Row("Alias") = ""
            ComboAlias.DataSource.rows.add(Row)
            ComboAlias.DisplayMember = "Alias"
            ComboAlias.ValueMember = "Clave"
            ' 
            ComboACuenta.DataSource = Tablas.Leer("SELECT Clave,Nombre,Estado FROM Proveedores WHERE TipoOperacion <> 4 ORDER BY Nombre;")
            ComboAliasACuenta.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '' AND TipoOperacion <> 4 ORDER BY Alias;")
            Row = ComboACuenta.DataSource.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            ComboACuenta.DataSource.rows.add(Row)
            ComboACuenta.DisplayMember = "Nombre"
            ComboACuenta.ValueMember = "Clave"
            Row = ComboAliasACuenta.DataSource.NewRow
            Row("Clave") = 0
            Row("Alias") = ""
            ComboAliasACuenta.DataSource.rows.add(Row)
            ComboAliasACuenta.DisplayMember = "Alias"
            ComboAliasACuenta.ValueMember = "Clave"
        End If
        If PEsProveedor And PEsEgresoCaja Then
            LabelEmisor.Text = "Proveedor" : Me.Text = "Opcion Proveedor"
            ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Estado FROM Proveedores WHERE EsEgresoCaja = 1 AND " & TipoIvaStr & " TipoOperacion <> 4;")
            ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE EsEgresoCaja = 1 AND " & TipoIvaStr & " Alias <> '' AND TipoOperacion <> 4;")
            Dim Row As DataRow = ComboEmisor.DataSource.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            ComboEmisor.DataSource.rows.add(Row)
            ComboEmisor.DisplayMember = "Nombre"
            ComboEmisor.ValueMember = "Clave"
            Row = ComboAlias.DataSource.NewRow
            Row("Clave") = 0
            Row("Alias") = ""
            ComboAlias.DataSource.rows.add(Row)
            ComboAlias.DisplayMember = "Alias"
            ComboAlias.ValueMember = "Clave"
        End If
        If PEsVendedor Then
            LabelEmisor.Text = "Vendedor" : Me.Text = "Opcion Vendedor"
            ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 37  ORDER BY Nombre;")
            Dim Row As DataRow = ComboEmisor.DataSource.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            ComboEmisor.DataSource.rows.add(Row)
            ComboEmisor.DisplayMember = "Nombre"
            ComboEmisor.ValueMember = "Clave"
        End If

        ComboEmisor.SelectedValue = 0
        ComboAlias.SelectedValue = 0
        ComboACuenta.SelectedValue = 0
        ComboAliasACuenta.SelectedValue = 0

        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        With ComboACuenta
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        With ComboAliasACuenta
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PEsTr Then PictureCandado.Visible = False

        PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
        PAbierto = True

        If Not PermisoTotal Then
            PictureCandado.Visible = False
        End If

        PEmisor = 0
        PACuenta = 0

        If PEsSinCandado Then
            PictureCandado.Visible = False
        End If

        If PEsPorCuentayOrden Then
            PanelBeneficiario.Visible = True
            LabelEmisor.Text = "Pago A"
        End If

        If PEsListaDePrecios Then
            Panel2.Visible = True
        Else : Panel2.Visible = False
            TextNumero.Text = "0"
        End If

        If PEsNuevaListaDePreciosVenta Then
            Panel3.Visible = True
        Else : Panel3.Visible = False
        End If

        If PEsProveedor And PEsEgresoCaja Then
            If ComboEmisor.DataSource.rows.count > 1 Then
                ComboEmisor.SelectedIndex = 0
            Else
                MsgBox("Debe Definir un Proveedor con la Propiedad 'Es Egreso de Caja a Cuenta de Resultado'.", MsgBoxStyle.Information)
                Me.Close() : Exit Sub
            End If
        End If

        If PEsVendedor Then
            Label2.Visible = False : ComboAlias.Visible = False
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ComboEmisor.SelectedValue <> 0 Then
            PEmisor = ComboEmisor.SelectedValue
            PNombre = ComboEmisor.Text
        Else
            PEmisor = ComboAlias.SelectedValue
            If PEsProveedor Then PNombre = NombreProveedor(ComboAlias.SelectedValue)
            If PEsCliente Then PNombre = NombreCliente(ComboAlias.SelectedValue)
        End If

        If PEsPorCuentayOrden Then
            If ComboACuenta.SelectedValue <> 0 Then
                PACuenta = ComboACuenta.SelectedValue
            Else : PACuenta = ComboAliasACuenta.SelectedValue
            End If
        End If

        If PPuedeTenerFCE Then
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = ComboEmisor.DataSource.Select("Clave = " & PEmisor)
            If RowsBusqueda(0).Item("EsFCE") And (PTipoNota = 5 Or PTipoNota = 7) And PAbierto Then
                UnCompobanteFCE.PEsDebitoCredito = True
                UnCompobanteFCE.PTipoNota = PTipoNota
                UnCompobanteFCE.PCliente = PEmisor
                UnCompobanteFCE.ShowDialog()
                If UnCompobanteFCE.PRegresar Then
                    PEmisor = 0
                    UnCompobanteFCE.Dispose()
                Else
                    PFacturaFCE = UnCompobanteFCE.PFacturaFCE
                    PEsFCE = UnCompobanteFCE.PEsFCE
                    UnCompobanteFCE.Dispose()
                End If
            End If
        End If

        PNumero = CInt(TextNumero.Text)
        PListaDiaria = RadioListaDiaria.Checked

        Me.Close()

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        If PEsProveedor And ComboEmisor.SelectedValue <> 0 And ComboEmisor.SelectedValue = GProveedorEgresoCaja Then
            MsgBox("Proveedor Egreso de Caja Inválida pera este Comprobante.", MsgBoxStyle.Information)
            ComboEmisor.SelectedValue = 0
            Exit Sub
        End If

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        If PEsProveedor And ComboAlias.SelectedValue <> 0 And ComboAlias.SelectedValue = GProveedorEgresoCaja Then
            MsgBox("Proveedor Egreso de Caja Inválida pera este Comprobante.", MsgBoxStyle.Information)
            ComboAlias.SelectedValue = 0
            Exit Sub
        End If

    End Sub
    Private Sub ComboACuenta_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboACuenta.Validating

        If IsNothing(ComboACuenta.SelectedValue) Then ComboACuenta.SelectedValue = 0

        If PEsProveedor And ComboACuenta.SelectedValue <> 0 And ComboACuenta.SelectedValue = GProveedorEgresoCaja Then
            MsgBox("Proveedor Egreso de Caja Inválida pera este Comprobante.", MsgBoxStyle.Information)
            ComboACuenta.SelectedValue = 0
            Exit Sub
        End If

    End Sub
    Private Sub ComboAliasACuenta_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAliasACuenta.Validating

        If IsNothing(ComboAliasACuenta.SelectedValue) Then ComboAliasACuenta.SelectedValue = 0

        If PEsProveedor And ComboAliasACuenta.SelectedValue <> 0 And ComboAliasACuenta.SelectedValue = GProveedorEgresoCaja Then
            MsgBox("Proveedor Egreso de Caja Inválida pera este Comprobante.", MsgBoxStyle.Information)
            ComboAliasACuenta.SelectedValue = 0
            Exit Sub
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
    Private Sub TextNumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumero.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, 0)

    End Sub
    Private Function Valida() As Boolean

        Dim Emisor As Integer = 0
        Dim ACuenta As Integer = 0

        If Not PEsPorCuentayOrden Then
            If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue = 0 Then
                If PEsCliente Then MsgBox("Falta Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                If PEsProveedor Then MsgBox("Falta Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Return False
            End If
            If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
                If PEsCliente Then MsgBox("Debe Seleccionar Cliente o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                If PEsProveedor Then MsgBox("Debe Seleccionar Proveedor o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Return False
            End If
        End If

        If PEsPorCuentayOrden Then
            If ComboACuenta.SelectedValue = 0 And ComboAliasACuenta.SelectedValue = 0 Then
                MsgBox("Falta Por Cuenta De.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboACuenta.Focus()
                Return False
            End If
            If ComboACuenta.SelectedValue <> 0 And ComboAliasACuenta.SelectedValue <> 0 Then
                MsgBox("Debe Seleccionar Por Cuenta De. o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboACuenta.Focus()
                Return False
            End If
            If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue = 0 Then
                MsgBox("Falta Pagar A.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Return False
            End If
            If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
                MsgBox("Debe Seleccionar Pagar A. o Alias", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Return False
            End If
            If ComboEmisor.SelectedValue = ComboACuenta.SelectedValue And ComboACuenta.SelectedValue <> 0 Then
                MsgBox("Por Cuenta De no debe ser Pagar A.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboACuenta.Focus()
                Return False
            End If
            If ComboAlias.SelectedValue = ComboAliasACuenta.SelectedValue And ComboAliasACuenta.SelectedValue <> 0 Then
                MsgBox("Por Cuenta De no debe ser Pagar A.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboACuenta.Focus()
                Return False
            End If
            If ComboEmisor.SelectedValue <> 0 Then Emisor = ComboEmisor.SelectedValue
            If ComboAlias.SelectedValue <> 0 Then Emisor = ComboAlias.SelectedValue
            If ComboACuenta.SelectedValue <> 0 Then ACuenta = ComboACuenta.SelectedValue
            If ComboAliasACuenta.SelectedValue <> 0 Then ACuenta = ComboAliasACuenta.SelectedValue
            If HallaMonedaProveedor(Emisor) <> HallaMonedaProveedor(ACuenta) Then
                MsgBox("Monedas en que operan los proveedores son Distintas.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If PEsListaDePrecios Then
            If TextNumero.Text = "" Then
                MsgBox("Falta Informar Lista De Precio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextNumero.Focus()
                Return False
            End If
            If TextNumero.Text = "0" Then
                MsgBox("Falta Informar Lista De Precio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextNumero.Focus()
                Return False
            End If
        End If

        If PEsNuevaListaDePreciosVenta Then
            If Not RadioListaDiaria.Checked And Not RadioSemanaCompleta.Checked Then
                MsgBox("Falta Informar Lista Diaria o a Semana Completa.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If PEsSoloAltas Then
            Dim EmisorW As Integer
            If ComboEmisor.SelectedValue <> 0 Then EmisorW = ComboEmisor.SelectedValue
            If ComboAlias.SelectedValue <> 0 Then EmisorW = ComboAlias.SelectedValue
            '
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = ComboEmisor.DataSource.Select("Clave = " & EmisorW)
            If RowsBusqueda(0).Item("Estado") = 3 Then
                If PEsProveedor Then
                    MsgBox("Proveedor Esta dado de Baja. Debe cambiar estado en : " + vbCrLf + "Menu --> Lista Proveedores--> Proveedor --> Estado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End If
                If PEsCliente Then
                    MsgBox("Cliente Esta dado de Baja. Debe cambiar estado en : " + vbCrLf + "Menu --> Lista Clientes--> Cliente --> Estado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End If
                Return False
            End If
            '
            If PEsPorCuentayOrden Then
                If ComboACuenta.SelectedValue <> 0 Then EmisorW = ComboACuenta.SelectedValue
                If ComboAliasACuenta.SelectedValue <> 0 Then EmisorW = ComboAliasACuenta.SelectedValue
                RowsBusqueda = ComboACuenta.DataSource.Select("Clave = " & EmisorW)
                If RowsBusqueda(0).Item("Estado") = 3 Then
                    MsgBox("Proveedor A Cuenta Esta dado de Baja. Debe cambiar estado en : " + vbCrLf + "Menu --> Lista Proveedores--> Proveedor --> Estado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
        End If


        Return True

    End Function

    
End Class