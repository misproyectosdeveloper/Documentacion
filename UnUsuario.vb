Public Class UnUsuario
    Private MiEnlazador As New BindingSource
    '
    Dim DtUsuarios As DataTable
    Dim Dt As DataTable
    Dim DtPuntos As DataTable
    Dim DtN As DataTable
    Dim DtFunciones As DataTable
    Dim cb As ComboBox
    Private Sub Usuarios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        PictureCandadoAbierto.Image = ImageList1.Images.Item("Abierto")
        PictureCandadoCerrado.Image = ImageList1.Images.Item("Cerrado")

        Grid.AutoGenerateColumns = False

        CreaDtFunciones()
        ComboFuncion.DataSource = DtFunciones
        ComboFuncion.DisplayMember = "Nombre"
        ComboFuncion.ValueMember = "Funcion"
        With ComboFuncion
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboFuncion.SelectedValue = 0

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        Dim Row As DataRow = ComboEstado.DataSource.NewRow()
        Row("Clave") = 2
        Row("Nombre") = "Suspendido"
        ComboEstado.DataSource.Rows.Add(Row)
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0

        DtPuntos = New DataTable
        DtPuntos = Tablas.Leer("SELECT Clave,RIGHT('0000' + CAST(Clave AS varchar),4) as Nombre,EsFce FROM PuntosDeVenta ORDER BY Clave;")
        Row = DtPuntos.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        DtPuntos.Rows.Add(Row)

        ComboPuntoDeVentaResponsableInsc.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaResponsableInsc.DisplayMember = "Nombre"
        ComboPuntoDeVentaResponsableInsc.ValueMember = "Clave"
        ComboPuntoDeVentaResponsableInsc.SelectedValue = 0

        ComboPuntoDeVentaResponsableNoInsc.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaResponsableNoInsc.DisplayMember = "Nombre"
        ComboPuntoDeVentaResponsableNoInsc.ValueMember = "Clave"
        ComboPuntoDeVentaResponsableNoInsc.SelectedValue = 0

        ComboPuntoDeVentaConsumidorFinal.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaConsumidorFinal.DisplayMember = "Nombre"
        ComboPuntoDeVentaConsumidorFinal.ValueMember = "Clave"
        ComboPuntoDeVentaConsumidorFinal.SelectedValue = 0

        ComboPuntoDeVentaExportacion.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaExportacion.DisplayMember = "Nombre"
        ComboPuntoDeVentaExportacion.ValueMember = "Clave"
        ComboPuntoDeVentaExportacion.SelectedValue = 0

        ComboPuntoDeVentaRemitos.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaRemitos.DisplayMember = "Nombre"
        ComboPuntoDeVentaRemitos.ValueMember = "Clave"
        ComboPuntoDeVentaRemitos.SelectedValue = 0

        ComboPuntoDeVentaRecibos.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaRecibos.DisplayMember = "Nombre"
        ComboPuntoDeVentaRecibos.ValueMember = "Clave"
        ComboPuntoDeVentaRecibos.SelectedValue = 0

        ComboPuntoDeVentaDebResponsableInsc.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaDebResponsableInsc.DisplayMember = "Nombre"
        ComboPuntoDeVentaDebResponsableInsc.ValueMember = "Clave"
        ComboPuntoDeVentaDebResponsableInsc.SelectedValue = 0

        ComboPuntoDeVentaDebResponsableNoInsc.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaDebResponsableNoInsc.DisplayMember = "Nombre"
        ComboPuntoDeVentaDebResponsableNoInsc.ValueMember = "Clave"
        ComboPuntoDeVentaDebResponsableNoInsc.SelectedValue = 0

        ComboPuntoDeVentaDebConsumidorFinal.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaDebConsumidorFinal.DisplayMember = "Nombre"
        ComboPuntoDeVentaDebConsumidorFinal.ValueMember = "Clave"
        ComboPuntoDeVentaDebConsumidorFinal.SelectedValue = 0

        ComboPuntoDeVentaDebExportacion.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaDebExportacion.DisplayMember = "Nombre"
        ComboPuntoDeVentaDebExportacion.ValueMember = "Clave"
        ComboPuntoDeVentaDebExportacion.SelectedValue = 0

        ComboPuntoDeVentaCredResponsableInsc.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaCredResponsableInsc.DisplayMember = "Nombre"
        ComboPuntoDeVentaCredResponsableInsc.ValueMember = "Clave"
        ComboPuntoDeVentaCredResponsableInsc.SelectedValue = 0

        ComboPuntoDeVentaCredResponsableNoInsc.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaCredResponsableNoInsc.DisplayMember = "Nombre"
        ComboPuntoDeVentaCredResponsableNoInsc.ValueMember = "Clave"
        ComboPuntoDeVentaCredResponsableNoInsc.SelectedValue = 0

        ComboPuntoDeVentaCredConsumidorFinal.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaCredConsumidorFinal.DisplayMember = "Nombre"
        ComboPuntoDeVentaCredConsumidorFinal.ValueMember = "Clave"
        ComboPuntoDeVentaCredConsumidorFinal.SelectedValue = 0

        ComboPuntoDeVentaCredExportacion.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaCredExportacion.DisplayMember = "Nombre"
        ComboPuntoDeVentaCredExportacion.ValueMember = "Clave"
        ComboPuntoDeVentaCredExportacion.SelectedValue = 0

        ComboPuntoDeVentaLiqResponsableInsc.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaLiqResponsableInsc.DisplayMember = "Nombre"
        ComboPuntoDeVentaLiqResponsableInsc.ValueMember = "Clave"
        ComboPuntoDeVentaLiqResponsableInsc.SelectedValue = 0

        ComboPuntoDeVentaLiqResponsableNoInsc.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaLiqResponsableNoInsc.DisplayMember = "Nombre"
        ComboPuntoDeVentaLiqResponsableNoInsc.ValueMember = "Clave"
        ComboPuntoDeVentaLiqResponsableNoInsc.SelectedValue = 0

        ComboPuntoDeVentaLiqConsumidorFinal.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaLiqConsumidorFinal.DisplayMember = "Nombre"
        ComboPuntoDeVentaLiqConsumidorFinal.ValueMember = "Clave"
        ComboPuntoDeVentaLiqConsumidorFinal.SelectedValue = 0

        ComboPuntoDeVentaFCE.DataSource = DtPuntos.Copy
        ComboPuntoDeVentaFCE.DisplayMember = "Nombre"
        ComboPuntoDeVentaFCE.ValueMember = "Clave"
        ComboPuntoDeVentaFCE.SelectedValue = 0

        If Not PermisoTotal Then
            PanelAlternativo.Visible = False
            PictureCandadoAbierto.Visible = False
        End If

        ArmaArchivoUsuarios()

        If Grid.Rows.Count <> 0 Then ArmaArchivos(Grid.Rows(0).Cells("Usuario").Value)

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        ArmaArchivos("")

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Dim Usuario As String = Dt.Rows(0).Item("Usuario")

        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        If Dt.Rows(0).Item("Password") <> Encriptar(TextPassword.Text) Then
            Dt.Rows(0).Item("Password") = Encriptar(TextPassword.Text)
        End If

        If DtN.Rows.Count <> 0 Then
            If TextPasswordN.Text <> DtN.Rows(0).Item("Password") Then
                DtN.Rows(0).Item("Password") = Encriptar(TextPasswordN.Text)
            End If
        Else
            If TextPasswordN.Text <> "" Then
                Dim Row As DataRow = DtN.NewRow
                For I As Integer = 0 To Dt.Columns.Count - 1
                    Row.Item(I) = Dt.Rows(0).Item(I)
                Next
                DtN.Rows.Add(Row)
                DtN.Rows(0).Item("Password") = Encriptar(TextPasswordN.Text)
                DtN.Rows(0).Item("Rel") = Dt.Rows(0).Item("Clave")
            End If
        End If

        Dim Resul As Integer

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not IsNothing(Dt.GetChanges) Then
            Resul = GrabaArchivo(Dt.GetChanges, Conexion)
            If Resul <> 1000 Then Exit Sub
        End If

        If DtN.Rows.Count <> 0 Then
            Resul = GrabaArchivo(DtN.GetChanges, ConexionN)
            If Resul <> 1000 Then Exit Sub
        End If
        If Resul = 1000 Then
            MsgBox("Cambios Realizados Exitosamente. Debe Reingresar al Sistema para que los Cambios Tengan EFECTO.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            ArmaArchivoUsuarios()
            ArmaArchivos(Usuario)
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Usuario").Value = Usuario Then Grid.CurrentCell = Grid.Rows(Row.Index).Cells("Usuario") : Exit For
            Next
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBaja_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBaja.Click

        Dim Row As DataRow = Dt.Rows(0)

        If IsDBNull(Row("Clave")) Then Exit Sub

        If Row("Clave") = GClaveUsuario Then
            MsgBox("No se Puede dar de Baja Así mismo")
            Exit Sub
        End If

        If MsgBox("Usuario se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        Dt.Rows(0).Item("Estado") = 3

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Resul As Integer

        If Not IsNothing(Dt.GetChanges) Then
            Resul = GrabaArchivo(Dt.GetChanges, Conexion)
            If Resul <> 1000 Then Me.Cursor = System.Windows.Forms.Cursors.Default : Dt.Rows(0).Item("Estado") = 1 : Exit Sub
        End If

        Usuarios_Load(Nothing, Nothing)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonPerfiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPerfiles.Click

        UnPerfil.PUsuario = Dt.Rows(0).Item("Clave")
        UnPerfil.PNombre = Dt.Rows(0).Item("Nombre")
        UnPerfil.ShowDialog()

    End Sub
    Private Sub ComboFuncion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboFuncion.Validating

        If IsNothing(ComboFuncion.SelectedValue) Then ComboFuncion.SelectedValue = 0

    End Sub
    Private Sub ComboPuntoDeVentaResponsableinsc_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaResponsableInsc.Validating

        If IsNothing(ComboPuntoDeVentaResponsableInsc.SelectedValue) Then ComboPuntoDeVentaResponsableInsc.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaResponsableInsc, False)

    End Sub
    Private Sub ComboPuntoDeVentaResponsableNoinsc_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaResponsableNoInsc.Validating

        If IsNothing(ComboPuntoDeVentaResponsableNoInsc.SelectedValue) Then ComboPuntoDeVentaResponsableNoInsc.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaResponsableNoInsc, False)

    End Sub
    Private Sub ComboPuntoDeVentaConsumidorFinal_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaConsumidorFinal.Validating

        If IsNothing(ComboPuntoDeVentaConsumidorFinal.SelectedValue) Then ComboPuntoDeVentaConsumidorFinal.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaConsumidorFinal, False)

    End Sub
    Private Sub ComboPuntoDeVentaExportacion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaExportacion.Validating

        If IsNothing(ComboPuntoDeVentaExportacion.SelectedValue) Then ComboPuntoDeVentaExportacion.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaExportacion, False)

    End Sub
    Private Sub ComboPuntoDeVentaDebExportacion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaDebExportacion.Validating

        If IsNothing(ComboPuntoDeVentaDebExportacion.SelectedValue) Then ComboPuntoDeVentaDebExportacion.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaDebExportacion, False)

    End Sub
    Private Sub ComboPuntoDeVentaCredExportacion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaCredExportacion.Validating

        If IsNothing(ComboPuntoDeVentaCredExportacion.SelectedValue) Then ComboPuntoDeVentaCredExportacion.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaCredExportacion, False)

    End Sub
    Private Sub ComboPuntoDeVentaDebResponsableInsc_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaDebResponsableInsc.Validating

        If IsNothing(ComboPuntoDeVentaDebResponsableInsc.SelectedValue) Then ComboPuntoDeVentaDebResponsableInsc.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaDebResponsableInsc, False)

    End Sub
    Private Sub ComboPuntoDeVentaDebResponsableNoinsc_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaDebResponsableNoInsc.Validating

        If IsNothing(ComboPuntoDeVentaDebResponsableNoInsc.SelectedValue) Then ComboPuntoDeVentaDebResponsableNoInsc.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaDebResponsableNoInsc, False)

    End Sub
    Private Sub ComboPuntoDeVentaDebConsumidorFinal_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaDebConsumidorFinal.Validating

        If IsNothing(ComboPuntoDeVentaDebConsumidorFinal.SelectedValue) Then ComboPuntoDeVentaDebConsumidorFinal.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaDebConsumidorFinal, False)

    End Sub
    Private Sub ComboPuntoDeVentaCredResponsableInsc_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaCredResponsableInsc.Validating

        If IsNothing(ComboPuntoDeVentaCredResponsableInsc.SelectedValue) Then ComboPuntoDeVentaCredResponsableInsc.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaCredResponsableInsc, False)

    End Sub
    Private Sub ComboPuntoDeVentaCredResponsableNoinsc_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaCredResponsableNoInsc.Validating

        If IsNothing(ComboPuntoDeVentaCredResponsableNoInsc.SelectedValue) Then ComboPuntoDeVentaCredResponsableNoInsc.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaCredResponsableNoInsc, False)

    End Sub
    Private Sub ComboPuntoDeVentaCredConsumidorFinal_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaCredConsumidorFinal.Validating

        If IsNothing(ComboPuntoDeVentaCredConsumidorFinal.SelectedValue) Then ComboPuntoDeVentaCredConsumidorFinal.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaCredConsumidorFinal, False)

    End Sub
    Private Sub ComboPuntoDeVentaLiqResponsableInsc_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaLiqResponsableInsc.Validating

        If IsNothing(ComboPuntoDeVentaLiqResponsableInsc.SelectedValue) Then ComboPuntoDeVentaLiqResponsableInsc.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaLiqResponsableInsc, False)

    End Sub
    Private Sub ComboPuntoDeVentaLiqResponsableNoInsc_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaLiqResponsableNoInsc.Validating

        If IsNothing(ComboPuntoDeVentaLiqResponsableNoInsc.SelectedValue) Then ComboPuntoDeVentaLiqResponsableNoInsc.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaLiqResponsableNoInsc, False)

    End Sub
    Private Sub ComboPuntoDeVentaLiqConsumidorFinal_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaLiqConsumidorFinal.Validating

        If IsNothing(ComboPuntoDeVentaLiqConsumidorFinal.SelectedValue) Then ComboPuntoDeVentaLiqConsumidorFinal.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaLiqConsumidorFinal, False)

    End Sub
    Private Sub ComboPuntoDeVentaFCE_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaFCE.Validating

        If IsNothing(ComboPuntoDeVentaFCE.SelectedValue) Then ComboPuntoDeVentaFCE.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaFCE, True)

    End Sub
    Private Sub ComboPuntoDeVentaRemitos_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaRemitos.Validating

        If IsNothing(ComboPuntoDeVentaRemitos.SelectedValue) Then ComboPuntoDeVentaRemitos.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaRemitos, False)

    End Sub
    Private Sub ComboPuntoDeVentaRecibos_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVentaRecibos.Validating

        If IsNothing(ComboPuntoDeVentaRecibos.SelectedValue) Then ComboPuntoDeVentaRecibos.SelectedValue = 0

        ValidaPuntoVenta(ComboPuntoDeVentaRecibos, False)

    End Sub
    Private Sub TextCaja_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCaja.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCaja_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextCaja.Validating

        If Not IsNumeric(TextCaja.Text) Then TextCaja.Text = 0

    End Sub
    Private Sub TextPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextPassword.KeyPress

        If e.KeyChar = " " Then e.KeyChar = ""

    End Sub
    Private Sub TextPasswordN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextPasswordN.KeyPress

        If e.KeyChar = " " Then e.KeyChar = ""

    End Sub
    Private Sub TextConfirmacion_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextConfirmacion.KeyPress

        If e.KeyChar = " " Then e.KeyChar = ""

    End Sub
    Private Sub TextConfirmacionN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextConfirmacionN.KeyPress

        If e.KeyChar = " " Then e.KeyChar = ""

    End Sub
    Private Sub ArmaArchivoUsuarios()

        DtUsuarios = New DataTable
        If Not Tablas.Read("SELECT Clave,Usuario FROM Usuarios WHERE Estado <> 3;", Conexion, DtUsuarios) Then Me.Close() : Exit Sub

        Grid.DataSource = DtUsuarios

    End Sub
    Private Sub ArmaArchivos(ByVal Usuario As String)

        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM Usuarios WHERE Estado <> 3 AND Usuario = '" & Usuario & "';", Conexion, Dt) Then Me.Close() : Exit Sub

        If Dt.Rows.Count = 0 Then
            Dim Row As DataRow = Dt.NewRow
            Row("Clave") = 0
            Row("Rel") = 0
            Row("Usuario") = ""
            Row("Nombre") = ""
            Row("Caja") = 0
            Row("CajaTotal") = False
            Row("CajaTotalExportacion") = False
            Row("AccesoASueldos") = False
            Row("PonePrecios") = False
            Row("Articulos") = False
            Row("Stock") = False
            Row("Facturacion") = False
            Row("Clientes") = False
            Row("Proveedores") = False
            Row("OtrosProveedores") = False
            Row("Tesoreria") = False
            Row("Insumos") = False
            Row("Comercial") = False
            Row("ControlDeGestion") = False
            Row("Informes") = False
            Row("Contabilidad") = False
            Row("Funcion") = 0
            Row("PuntodeVentaResponsableInsc") = 0
            Row("PuntodeVentaResponsableNoInsc") = 0
            Row("PuntodeVentaConsumidorFinal") = 0
            Row("PuntodeVentaExportacion") = 0
            Row("PuntodeVentaRemitos") = 0
            Row("PuntodeVentaRecibos") = 0
            Row("PuntodeVentaDebResponsableInsc") = 0
            Row("PuntodeVentaDebResponsableNoInsc") = 0
            Row("PuntodeVentaDebConsumidorFinal") = 0
            Row("PuntodeVentaDebExportacion") = 0
            Row("PuntodeVentaCredResponsableInsc") = 0
            Row("PuntodeVentaCredResponsableNoInsc") = 0
            Row("PuntodeVentaCredConsumidorFinal") = 0
            Row("PuntodeVentaCredExportacion") = 0
            Row("PuntodeVentaLiqResponsableInsc") = 0
            Row("PuntodeVentaLiqResponsableNoInsc") = 0
            Row("PuntodeVentaLiqConsumidorFinal") = 0
            Row("PuntodeVentaFCE") = 0
            Row("Password") = ""
            Row("Estado") = 1
            Row("SubAdministradorExportacion") = False
            Row("SubAdministradorProduccion") = False
            Dt.Rows.Add(Row)
            TextUsuario.ReadOnly = False
        Else : TextUsuario.ReadOnly = True
        End If

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = Dt

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Usuario")
        TextUsuario.DataBindings.Clear()
        TextUsuario.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Nombre")
        TextNombre.DataBindings.Clear()
        TextNombre.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "CajaTotal")
        CheckCajaTotal.DataBindings.Clear()
        CheckCajaTotal.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "CajaTotalExportacion")
        CheckCajaTotalExportacion.DataBindings.Clear()
        CheckCajaTotalExportacion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Funcion")
        ComboFuncion.DataBindings.Clear()
        ComboFuncion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaResponsableInsc")
        ComboPuntoDeVentaResponsableInsc.DataBindings.Clear()
        ComboPuntoDeVentaResponsableInsc.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaResponsableNoInsc")
        ComboPuntoDeVentaResponsableNoInsc.DataBindings.Clear()
        ComboPuntoDeVentaResponsableNoInsc.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaConsumidorFinal")
        ComboPuntoDeVentaConsumidorFinal.DataBindings.Clear()
        ComboPuntoDeVentaConsumidorFinal.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaExportacion")
        ComboPuntoDeVentaExportacion.DataBindings.Clear()
        ComboPuntoDeVentaExportacion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaRemitos")
        ComboPuntoDeVentaRemitos.DataBindings.Clear()
        ComboPuntoDeVentaRemitos.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaRecibos")
        ComboPuntoDeVentaRecibos.DataBindings.Clear()
        ComboPuntoDeVentaRecibos.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaDebResponsableInsc")
        ComboPuntoDeVentaDebResponsableInsc.DataBindings.Clear()
        ComboPuntoDeVentaDebResponsableInsc.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaDebResponsableNoInsc")
        ComboPuntoDeVentaDebResponsableNoInsc.DataBindings.Clear()
        ComboPuntoDeVentaDebResponsableNoInsc.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaDebConsumidorFinal")
        ComboPuntoDeVentaDebConsumidorFinal.DataBindings.Clear()
        ComboPuntoDeVentaDebConsumidorFinal.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaDebExportacion")
        ComboPuntoDeVentaDebExportacion.DataBindings.Clear()
        ComboPuntoDeVentaDebExportacion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaCredResponsableInsc")
        ComboPuntoDeVentaCredResponsableInsc.DataBindings.Clear()
        ComboPuntoDeVentaCredResponsableInsc.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaCredResponsableNoInsc")
        ComboPuntoDeVentaCredResponsableNoInsc.DataBindings.Clear()
        ComboPuntoDeVentaCredResponsableNoInsc.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaCredConsumidorFinal")
        ComboPuntoDeVentaCredConsumidorFinal.DataBindings.Clear()
        ComboPuntoDeVentaCredConsumidorFinal.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaCredExportacion")
        ComboPuntoDeVentaCredExportacion.DataBindings.Clear()
        ComboPuntoDeVentaCredExportacion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaLiqResponsableInsc")
        ComboPuntoDeVentaLiqResponsableInsc.DataBindings.Clear()
        ComboPuntoDeVentaLiqResponsableInsc.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaLiqResponsableNoInsc")
        ComboPuntoDeVentaLiqResponsableNoInsc.DataBindings.Clear()
        ComboPuntoDeVentaLiqResponsableNoInsc.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaLiqConsumidorFinal")
        ComboPuntoDeVentaLiqConsumidorFinal.DataBindings.Clear()
        ComboPuntoDeVentaLiqConsumidorFinal.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "PuntoDeVentaFCE")
        ComboPuntoDeVentaFCE.DataBindings.Clear()
        ComboPuntoDeVentaFCE.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        TextCaja.DataBindings.Clear()
        TextCaja.DataBindings.Add(Enlace)

        TextPassword.Text = Desencriptar(Dt.Rows(0).Item("Password"))
        TextConfirmacion.Text = TextPassword.Text

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "SubAdministradorExportacion")  'No se usa.
        CheckSubAdministradorExportacion.DataBindings.Clear()
        CheckSubAdministradorExportacion.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "SubAdministradorProduccion")   'No se usa.
        CheckSubAdministradorProduccion.DataBindings.Clear()
        CheckSubAdministradorProduccion.DataBindings.Add(Enlace)

        DtN = New DataTable
        If PermisoTotal Then
            If Not Tablas.Read("SELECT * FROM Usuarios WHERE Rel = " & Dt.Rows(0).Item("Clave") & ";", ConexionN, DtN) Then Me.Close() : Exit Sub
        Else
            DtN = Dt.Clone
        End If

        If DtN.Rows.Count <> 0 Then
            TextPasswordN.Text = Desencriptar(DtN.Rows(0).Item("Password"))
            TextConfirmacionN.Text = TextPasswordN.Text
        Else : TextPasswordN.Text = ""
            TextConfirmacionN.Text = ""
        End If

        If Usuario = "" Then                        'Si es un alta no muestra passward alternativa.
            PanelAlternativo.Visible = False
        Else
            If PermisoTotal Then
                PanelAlternativo.Visible = True
            End If
        End If

        If TextCaja.Text = "0" Then
            TextCaja.ReadOnly = False
        Else
            TextCaja.ReadOnly = True
        End If

    End Sub
    Private Sub ParseCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatNumerico(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Function GrabaArchivo(ByVal DtAux As DataTable, ByVal ConexionStr As String) As Integer

        Dim Mensaje As String = Tablas.GrabarOleDb(DtAux, "SELECT * FROM Usuarios;", ConexionStr)
        If Not GModificacionOk Then
            MsgBox(Mensaje, MsgBoxStyle.Critical)
        Else
            Return 1000
        End If

    End Function
    Private Sub CreaDtFunciones()

        DtFunciones = New DataTable

        Dim Funcion As New DataColumn("Funcion")
        Funcion.DataType = System.Type.GetType("System.Int32")
        DtFunciones.Columns.Add(Funcion)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtFunciones.Columns.Add(Nombre)

        Dim Row As DataRow = DtFunciones.NewRow
        Row("Funcion") = 1
        Row("Nombre") = "Administrador"
        DtFunciones.Rows.Add(Row)
        Row = DtFunciones.NewRow
        Row("Funcion") = 2
        Row("Nombre") = "Operador"
        DtFunciones.Rows.Add(Row)
        Row = DtFunciones.NewRow
        Row("Funcion") = 0
        Row("Nombre") = ""
        DtFunciones.Rows.Add(Row)

    End Sub
    Public Function ExisteUsuario(ByVal Usuario As String, ByVal ConexionStr As String) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Usuario FROM Usuarios WHERE Clave <> " & Dt.Rows(0).Item("Clave") & " AND Estado <> 3 AND Usuario = '" & Usuario & "';", Miconexion)
                    If Cmd.ExecuteScalar() <> "" Then
                        Return True
                    Else : Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos.")
            End
        End Try

    End Function
    Public Function ExisteCaja(ByVal Caja As Integer, ByVal ConexionStr As String) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Caja FROM Usuarios WHERE Clave <> " & Dt.Rows(0).Item("Clave") & " AND Estado <> 3 AND Caja = " & Caja & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then
                        Return True
                    Else : Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos.")
            End
        End Try

    End Function
    Public Function EsPuntoVentaFCE(ByVal PuntoVenta As Integer, ByVal ConexionStr As String) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT EsFCE FROM PuntosDeVenta WHERE Clave = " & PuntoVenta & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then
                        Return True
                    Else : Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Archivo Puntos De Venta.")
            End
        End Try

    End Function
    Private Sub ValidaPuntoVenta(ByVal PuntoVenta As ComboBox, ByVal ESFce As Boolean)

        If PuntoVenta.SelectedValue = 0 Then Exit Sub

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtPuntos.Select("Clave = " & PuntoVenta.SelectedValue)
        If RowsBusqueda(0).Item("EsFce") And Not ESFce Then
            MsgBox("Punto de Venta solo para FCE.", MsgBoxStyle.Information)
            PuntoVenta.SelectedValue = 0 : PuntoVenta.Focus()
        End If
        If Not RowsBusqueda(0).Item("EsFce") And ESFce Then
            MsgBox("Punto de Venta No corresponde a FCE.", MsgBoxStyle.Information)
            PuntoVenta.SelectedValue = 0 : PuntoVenta.Focus()
        End If

    End Sub
    Private Function Valida() As Boolean

        TextUsuario.Text = TextUsuario.Text.Trim
        TextNombre.Text = TextNombre.Text.Trim
        TextPassword.Text = TextPassword.Text.Trim
        TextPasswordN.Text = TextPasswordN.Text.Trim
        TextConfirmacion.Text = TextConfirmacion.Text.Trim
        TextConfirmacionN.Text = TextConfirmacionN.Text.Trim

        If TextUsuario.Text = "" Then
            MsgBox("Falta Informar Usuario")
            TextUsuario.Focus()
            Return False
        End If

        If ExisteUsuario(TextUsuario.Text, Conexion) Then
            MsgBox("Usuario Ya Existe.")
            TextUsuario.Focus()
            Return False
        End If

        If TextCaja.Text <> "" Then
            If ExisteCaja(CInt(TextCaja.Text), Conexion) Then
                MsgBox("Caja Ya Existe.")
                TextUsuario.Focus()
                Return False
            End If
        End If

        If TextNombre.Text = "" Then
            MsgBox("Falta Informar Nombre")
            TextNombre.Focus()
            Return False
        End If

        If ComboFuncion.SelectedValue = 0 Then
            MsgBox("Falta Informar Funcion")
            ComboFuncion.Focus()
            Return False
        End If

        If TextPassword.Text = "" Then
            MsgBox("Falta Informar Password")
            TextPassword.Focus()
            Return False
        End If
        If TextPassword.Text.Length < 8 Then
            MsgBox("Password debe tener mas de 8 Digitos.")
            TextPassword.Focus()
            Return False
        End If
        If TextPassword.Text <> TextConfirmacion.Text Then
            MsgBox("Confirmación Password No Coincidente.")
            TextPassword.Focus()
            Return False
        End If

        If TextPassword.Text = TextPasswordN.Text Then
            MsgBox("Passwords No Pueden ser Iguales.")
            TextPassword.Focus()
            Return False
        End If
        If TextPasswordN.Text <> "" And TextPasswordN.Text.Length < 8 Then
            MsgBox("Password Alternativa debe tener mas de 8 Digitos.")
            TextPasswordN.Focus()
            Return False
        End If
        If TextPasswordN.Text <> "" And TextPasswordN.Text <> TextConfirmacionN.Text Then
            MsgBox("Confirmación Password Alternativa No Coincidente.")
            TextPasswordN.Focus()
            Return False
        End If

        If ComboPuntoDeVentaFCE.SelectedValue <> 0 Then
            If Not EsPuntoVentaFCE(ComboPuntoDeVentaFCE.SelectedValue, Conexion) Then
                MsgBox("Punto de Venta " & ComboPuntoDeVentaFCE.Text & " No esta adherido a MiPyMEs (FCE).")
                ComboPuntoDeVentaFCE.Focus()
                Return False
            End If
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        ArmaArchivos(Grid.CurrentRow.Cells("Usuario").Value)

    End Sub
   
    
End Class