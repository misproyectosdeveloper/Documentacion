Imports System.io
Imports ClassPassWord
Public Class UnProveedor
    Public PProveedor As Integer
    Public PModificacionOk As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Dim Dt As DataTable
    Dim DtExentas As DataTable
    '
    Dim AliasW As String
    Dim NombreW As String
    Dim CuitW As Double
    Dim UltimaClave As Integer
    Private Sub UnProveedor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(2) Then PBloqueaFunciones = True

        Me.Top = 50

        If PermisoTotal Then
            PanelCandado.Visible = True : PictureCandadoOPR.Visible = True
        Else : PanelCandado.Visible = False : PictureCandadoOPR.Visible = False
        End If

        ComboEstado.DataSource = DtEstadoLegajoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        ArmaTipoIva(ComboTipoIva)
        With ComboTipoIva
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboTipoIva.SelectedValue = 0

        LlenaComboTablas(ComboPais, 28)
        With ComboPais
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboProvincia, 31)
        ComboProvincia.SelectedValue = 0
        With ComboProvincia
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboProducto.DataSource = ArmaProductos()
        Dim Row As DataRow = ComboProducto.DataSource.NewRow()
        Row("Codigo") = 0
        Row("Nombre") = ""
        ComboProducto.DataSource.Rows.Add(Row)
        ComboProducto.DisplayMember = "Nombre"
        ComboProducto.ValueMember = "Codigo"
        ComboProducto.SelectedValue = 0
        With ComboProducto
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboTipoOperacion.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Miselaneas WHERE Clave <> 3 AND Codigo = 1;")
        Dim RowsBusqueda() As DataRow
        RowsBusqueda = ComboTipoOperacion.DataSource.select("Clave = 4")
        RowsBusqueda(0).Delete()
        Row = ComboTipoOperacion.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboTipoOperacion.DataSource.rows.add(Row)
        ComboTipoOperacion.DisplayMember = "Nombre"
        ComboTipoOperacion.ValueMember = "Clave"
        ComboTipoOperacion.SelectedValue = 0
        With ComboTipoOperacion
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboMoneda.DataSource = Nothing
        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Row = ComboMoneda.DataSource.newrow
        Row("Nombre") = "PESOS"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"
        With ComboMoneda
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboMonedaSaldoInicial.DataSource = Nothing
        ComboMonedaSaldoInicial.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Row = ComboMonedaSaldoInicial.DataSource.newrow
        Row("Nombre") = "PESOS"
        Row("Clave") = 1
        ComboMonedaSaldoInicial.DataSource.rows.add(Row)
        Row = ComboMonedaSaldoInicial.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboMonedaSaldoInicial.DataSource.rows.add(Row)
        ComboMonedaSaldoInicial.DisplayMember = "Nombre"
        ComboMonedaSaldoInicial.ValueMember = "Clave"
        ComboMonedaSaldoInicial.SelectedValue = 0

        PModificacionOk = False

        If Not ArmaArchivos() Then Me.Close()

        If Not PermisoTotal Then
            PictureCandadoB.Visible = False
            Panel7.Visible = False
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        If IsNothing(Dt.GetChanges) And IsNothing(DtExentas.GetChanges) Then MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3) : Exit Sub

        If Not Valida() Then Exit Sub

        Dim DtSucursales As New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not ActualizaProveedor(DtSucursales) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        
        GExcepcion = HallaDatoGenerico("SELECT Clave FROM Proveedores WHERE EsEgresoCaja = 1;", Conexion, GProveedorEgresoCaja)
        If Not IsNothing(GExcepcion) Then
            MsgBox("Error al Leer Tabla: Proveedores." + vbCrLf + vbCrLf + GExcepcion.Message)
            Me.Close() : Exit Sub
        End If

        If PProveedor = 0 Then PProveedor = UltimaClave

        If Not ArmaArchivos() Then Me.Close()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        MsgBox("Bloqueada Momentaneamente,Consultar Administrador.")
        Exit Sub

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PProveedor = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If GUsaNegra And Not PermisoTotal Then
            MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If TextCambio.Text <> "" Then
            MsgBox("Proveedor Tiene Saldo Inicial. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Usado(Dt.Rows(0).Item("Clave"), Conexion) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("El Cliente esta siendo usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Usado(Dt.Rows(0).Item("Clave"), ConexionN) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("El Cliente esta siendo usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtSucursales As New DataTable
        If Not Tablas.Read("SELECT * FROM SucursalesProveedores WHERE Proveedor = " & PProveedor & ";", Conexion, DtSucursales) Then End
        For Each Row As DataRow In DtSucursales.Rows
            Row.Delete()
        Next

        If MsgBox("El Proveedor se eliminara Definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        MiEnlazador.Remove(MiEnlazador.Current)

        For Each Row As DataRow In DtExentas.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Row.Delete()
            End If
        Next

        Dim ArchivoAgenda As String = "Proveedor" & PProveedor.ToString
        Dim RutaAgenda As String = System.AppDomain.CurrentDomain.BaseDirectory() & "\" & "Agenda" & GClaveEmpresa & "\" & ArchivoAgenda & ".TXT"

        If ActualizaProveedor(DtSucursales) Then
            If Directory.Exists(RutaAgenda) Then
                File.Delete(RutaAgenda)
            End If
        End If

        GExcepcion = HallaDatoGenerico("SELECT Clave FROM Proveedores WHERE EsEgresoCaja = 1;", Conexion, GProveedorEgresoCaja)
        If Not IsNothing(GExcepcion) Then
            MsgBox("Error al Leer Tabla: Proveedores." + vbCrLf + vbCrLf + GExcepcion.Message)
            Me.Close() : Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Me.Close()

    End Sub
    Private Sub PictureCandadoOpr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandadoOPR.Click

        If Dt.Rows(0).Item("Opr") Then
            PictureCandadoOPR.Image = ImageList1.Images.Item("Cerrado")
            Dt.Rows(0).Item("Opr") = False
        Else : PictureCandadoOPR.Image = ImageList1.Images.Item("Abierto")
            Dt.Rows(0).Item("Opr") = True
        End If

    End Sub
    Private Sub ButtonSaldoInicial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSaldoInicial.Click

        If PProveedor = 0 Then
            MsgBox("Debe Previamente Realizar el Alta.", MsgBoxStyle.Information)
            Exit Sub
        End If

        UnSaldoInicial.PEmisor = PProveedor
        UnSaldoInicial.PMoneda = ComboMoneda.SelectedValue
        UnSaldoInicial.PTipo = 2
        UnSaldoInicial.ShowDialog()
        UnSaldoInicial.Dispose()
        If GModificacionOk Then MuestraSaldos()

    End Sub
    Private Sub ButtonRetencionesExentas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRetencionesExentas.Click

        SeleccionarVarios.PListaDeRetenciones = New List(Of FilaItemsRetencion)

        For Each Row As DataRow In DtExentas.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Dim Fila As New FilaItemsRetencion
                Fila.Clave = Row("Clave")
                SeleccionarVarios.PListaDeRetenciones.Add(Fila)
            End If
        Next
        SeleccionarVarios.PEsRetencionesExentas = True
        SeleccionarVarios.PBloqueaFunciones = PBloqueaFunciones
        SeleccionarVarios.ShowDialog()

        Dim RowsBusqueda() As DataRow

        For Each Fila As FilaItemsRetencion In SeleccionarVarios.PListaDeRetenciones
            RowsBusqueda = DtExentas.Select("Clave = " & Fila.Clave)
            If RowsBusqueda.Length = 0 Then
                Dim Row As DataRow = DtExentas.NewRow
                Row("Clave") = Fila.Clave
                Row("TipoEmisor") = 1
                Row("Emisor") = PProveedor
                DtExentas.Rows.Add(Row)
            End If
        Next
        Dim I As Integer
        For Z As Integer = DtExentas.Rows.Count - 1 To 0 Step -1
            Dim Row As DataRow = DtExentas.Rows(Z)
            If Row.RowState <> DataRowState.Deleted Then
                For I = 0 To SeleccionarVarios.PListaDeRetenciones.Count - 1
                    If SeleccionarVarios.PListaDeRetenciones(I).Clave = Row("Clave") Then Exit For
                Next
                If I > SeleccionarVarios.PListaDeRetenciones.Count - 1 Then
                    Row.Delete()
                End If
            End If
        Next

        SeleccionarVarios.Dispose()

    End Sub
    Private Sub ComboProducto_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboProducto.SelectionChangeCommitted

        If ComboProducto.SelectedValue <> Fruta Then
            Panel4.Visible = False
        Else : Panel4.Visible = True
        End If

    End Sub
    Private Sub PictureSucursales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureSucursales.Click

        If PProveedor = 0 Then
            MsgBox("Debe dar de Alta al Proveedor.", MsgBoxStyle.Information)
            Exit Sub
        End If

        UnaTablaSucursalcliente.PBloqueaFunciones = PBloqueaFunciones
        UnaTablaSucursalcliente.PProveedor = PProveedor
        UnaTablaSucursalcliente.BackColor = Color.PowderBlue
        UnaTablaSucursalcliente.ShowDialog()

    End Sub
    Private Sub CheckEsEgresoCaja_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckEsEgresoCaja.Click

        Dim Sql As String
        Dim Dt As New DataTable

        If CheckEsEgresoCaja.Checked Then
            Sql = "SELECT COUNT(Clave) as Conta FROM Proveedores WHERE EsEgresoCaja = 1 AND Clave <> " & PProveedor & ";"
            If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
            If Dt.Rows(0).Item("Conta") <> 0 Then
                MsgBox("Ya Existe Otro Proveedor que 'Es Egreso Caja'. Operación se CANCELA.", MsgBoxStyle.Information)
                CheckEsEgresoCaja.Checked = False
            End If
        Else
            If Not PermisoTotal Then
                MsgBox("Error (1000). Operación se CANCELA.")
                CheckEsEgresoCaja.Checked = True
                Exit Sub
            End If
            Sql = "SELECT COUNT(Nota) as Conta FROM RecibosCabeza WHERE TipoNota = 600 AND Emisor = " & PProveedor & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
            If Dt.Rows(0).Item("Conta") <> 0 Then
                MsgBox("Ya Existe Movimientos para este proveedor. Operación se CANCELA.", MsgBoxStyle.Information)
                CheckEsEgresoCaja.Checked = True
            End If
        End If

        Dt.Dispose()

    End Sub
    Private Sub ButtonAgenda_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAgenda.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PProveedor = 0 Then
            MsgBox("Debe dar de Alta al Proveedor", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        UnaAgenda.PArchivo = "Proveedor" & PProveedor.ToString
        UnaAgenda.ShowDialog()

    End Sub
    Private Sub TextNombre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNombre.KeyPress

        If ValidaStringNombres(e.KeyChar) <> "" Then
            e.KeyChar = ""
        End If

    End Sub
    Private Sub TextAlias_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextAlias.KeyPress

        If ValidaStringNombres(e.KeyChar) <> "" Then
            e.KeyChar = ""
        End If

    End Sub
    Private Sub TextDirecto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextDirecto.KeyPress

        If Asc(e.KeyChar) = 13 Then TextDirecto_Validating(Nothing, Nothing) : Exit Sub

        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextDirecto_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextDirecto.Validating

        If Not IsNumeric(TextDirecto.Text) Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextDirecto.Focus()
            Exit Sub
        Else : TextDirecto.Text = FormatNumber(TextDirecto.Text, GDecimales, True, True, True)
            If CDbl(TextDirecto.Text) > 100 Then
                MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                TextDirecto.Focus()
                Exit Sub
            End If
            TextAutorizar.Text = FormatNumber(100 - CDbl(TextDirecto.Text), GDecimales, True, True, True)
        End If

    End Sub
    Private Sub TextComision_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComision.KeyPress

        If Asc(e.KeyChar) = 13 Then TextComision_Validating(Nothing, Nothing) : Exit Sub

        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextComision_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextComision.Validating

        If Not IsNumeric(TextComision.Text) Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextComision.Focus()
            Exit Sub
        Else : TextComision.Text = FormatNumber(TextComision.Text, GDecimales, True, True, True)
            If CDbl(TextComision.Text) >= 100 Then
                MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                TextComision.Focus()
                Exit Sub
            End If
        End If

    End Sub
    Private Sub TextComisionAdicional_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComisionAdicional.KeyPress

        If Asc(e.KeyChar) = 13 Then TextComisionAdicional_Validating(Nothing, Nothing) : Exit Sub

        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextComisionAdicional_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextComisionAdicional.Validating

        If Not IsNumeric(TextComisionAdicional.Text) Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextComisionAdicional.Focus()
            Exit Sub
        Else : TextComisionAdicional.Text = FormatNumber(TextComisionAdicional.Text, GDecimales, True, True, True)
            If CDbl(TextComisionAdicional.Text) >= 100 Then
                MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                TextComisionAdicional.Focus()
                Exit Sub
            End If
        End If

    End Sub
    Private Sub TextCondicionPago_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCondicionPago.KeyPress

        EsNumerico(e.KeyChar, TextCondicionPago.Text, 0)

    End Sub
    Private Sub Textnumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumero.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, 0)

    End Sub
    Private Sub ComboTipoIva_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipoIva.Validating

        If IsNothing(ComboTipoIva.SelectedValue) Then ComboTipoIva.SelectedValue = 0

        If ComboTipoIva.SelectedValue = Exterior Then
            MaskedCuit.Text = "00000000000"
            MaskedCuit.Enabled = False
        Else
            MaskedCuit.Enabled = True
        End If

    End Sub
    Private Sub ComboTipoOperacion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipoOperacion.Validating

        If IsNothing(ComboTipoOperacion.SelectedValue) Then ComboTipoOperacion.SelectedValue = 0

    End Sub
    Private Sub ComboProducto_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProducto.Validating

        If IsNothing(ComboProducto.SelectedValue) Then ComboProducto.SelectedValue = 0

    End Sub
    Private Sub ComboProvincia_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProvincia.Validating

        If IsNothing(ComboProvincia.SelectedValue) Then ComboProvincia.SelectedValue = 0

    End Sub
    Private Sub TextIngresoBruto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextIngresoBruto.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCodPostal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCodPostal.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM Proveedores Where Clave = " & PProveedor & ";", Conexion, Dt) Then Return False
        If Dt.Rows.Count = 0 And PProveedor <> 0 Then
            MsgBox("ERROR Proveedor no Existe. Opreacion se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Dt.Rows.Count = 0 Then AgregaRegistro()

        DtExentas = New DataTable
        If Not Tablas.Read("SELECT * FROM RetencionesExentas Where TipoEmisor = 1 AND Emisor = " & PProveedor & ";", Conexion, DtExentas) Then Return False
        If PProveedor = 0 Then
            If Not ArmaDtRetencionesExentas(1, DtExentas) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Error al Generar Retenciones/Percepciones Exentas. Salga e informe las no Exentas.", MsgBoxStyle.Information)
                Return False
            End If
        End If

        MuestraCabeza()

        '   If Dt.Rows(0).Item("Producto") <> Fruta Then
        '    ComboTipoOperacion.Enabled = False
        '     End If

        If PProveedor = 0 Then
            ComboEstado.Enabled = False
        Else
            ComboEstado.Enabled = True
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = Dt

        Dim Enlace As Binding

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Nombre")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextNombre.DataBindings.Clear()
        TextNombre.DataBindings.Add(Enlace)
        NombreW = TextNombre.Text

        Enlace = New Binding("Text", MiEnlazador, "Calle")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextCalle.DataBindings.Clear()
        TextCalle.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Numero")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextNumero.DataBindings.Clear()
        TextNumero.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Localidad")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextLocalidad.DataBindings.Clear()
        TextLocalidad.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Provincia")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        ComboProvincia.DataBindings.Clear()
        ComboProvincia.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Pais")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        ComboPais.DataBindings.Clear()
        ComboPais.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CodPostal")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextCodPostal.DataBindings.Clear()
        TextCodPostal.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Telefonos")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextTelefonos.DataBindings.Clear()
        TextTelefonos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Faxes")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextFaxes.DataBindings.Clear()
        TextFaxes.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cuit")
        AddHandler Enlace.Format, AddressOf FormatCuit
        MaskedCuit.DataBindings.Clear()
        MaskedCuit.DataBindings.Add(Enlace)
        CuitW = MaskedCuit.Text

        Enlace = New Binding("SelectedValue", MiEnlazador, "Producto")
        ComboProducto.DataBindings.Clear()
        ComboProducto.DataBindings.Add(Enlace)
        If ComboProducto.SelectedValue <> Fruta Then Panel4.Visible = False

        Enlace = New Binding("Checked", MiEnlazador, "EsCliente")
        CheckEsCliente.DataBindings.Clear()
        CheckEsCliente.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "EsDelGrupo")
        CheckBoxEsDelGrupo.DataBindings.Clear()
        CheckBoxEsDelGrupo.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "EsEgresoCaja")
        CheckEsEgresoCaja.DataBindings.Clear()
        CheckEsEgresoCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "TipoOperacion")
        ComboTipoOperacion.DataBindings.Clear()
        ComboTipoOperacion.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Directo")
        AddHandler Enlace.Format, AddressOf FormatNumerico
        TextDirecto.DataBindings.Clear()
        TextDirecto.DataBindings.Add(Enlace)
        TextAutorizar.Text = Format(100 - CDbl(TextDirecto.Text), "0.00")

        Enlace = New Binding("Text", MiEnlazador, "Comision")
        AddHandler Enlace.Format, AddressOf FormatNumerico
        TextComision.DataBindings.Clear()
        TextComision.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "ComisionAdicional")
        AddHandler Enlace.Format, AddressOf FormatNumerico
        TextComisionAdicional.DataBindings.Clear()
        TextComisionAdicional.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CondicionPago")
        TextCondicionPago.DataBindings.Clear()
        TextCondicionPago.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "IngresoBruto")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextIngresoBruto.DataBindings.Clear()
        TextIngresoBruto.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "TipoIva")
        ComboTipoIva.DataBindings.Clear()
        ComboTipoIva.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "ExentoRetencion")
        CheckExentoRetencion.DataBindings.Clear()
        CheckExentoRetencion.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "ListaDePrecios")
        CheckBoxListaDePrecios.DataBindings.Clear()
        CheckBoxListaDePrecios.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "ListaDePreciosPorZona")
        CheckBoxListaDePreciosPorZona.DataBindings.Clear()
        CheckBoxListaDePreciosPorZona.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Moneda")
        ComboMoneda.DataBindings.Clear()
        ComboMoneda.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Alias")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextAlias.DataBindings.Clear()
        TextAlias.DataBindings.Add(Enlace)
        AliasW = TextAlias.Text

        If PProveedor <> 0 Then
            MuestraSaldos()
        End If

        If Dt.Rows(0).Item("Opr") Then
            PictureCandadoOPR.Image = ImageList1.Images.Item("Abierto")
        Else : PictureCandadoOPR.Image = ImageList1.Images.Item("Cerrado")
        End If

    End Sub
    Private Sub ParseTexto(ByVal sender As Object, ByVal Nombre As ConvertEventArgs)

        Nombre.Value = Nombre.Value.ToString.Trim

    End Sub
    Private Sub FormatNumerico(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub FormatCambio(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, 3)

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "#")

    End Sub
    Private Sub FormatCuit(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "00000000000")

    End Sub
    Private Sub ParseCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub AgregaRegistro()

        Dim Row As DataRow = Dt.NewRow
        ArmaNuevoProveedor(Row)
        Row("Producto") = 0
        Row("Estado") = 1
        Dt.Rows.Add(Row)

    End Sub
    Private Sub MuestraSaldos()

        Dim Dt As New DataTable

        TextSaldoInicialN.Text = ""
        TextSaldoInicial.Text = ""
        TextCambio.Text = ""
        ComboMonedaSaldoInicial.SelectedValue = 0

        If Not Tablas.Read("SELECT * FROM SaldosInicialesCabeza WHERE Tipo = 2 AND Emisor = " & PProveedor & ";", Conexion, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            TextSaldoInicial.Text = FormatNumber(Dt.Rows(0).Item("Importe"), GDecimales)
            TextCambio.Text = FormatNumber(Dt.Rows(0).Item("Cambio"), 3)
            ComboMonedaSaldoInicial.SelectedValue = Dt.Rows(0).Item("Moneda")
        End If
        If PermisoTotal Then
            Dt = New DataTable
            If Not Tablas.Read("SELECT * FROM SaldosInicialesCabeza WHERE Tipo = 2 AND Emisor = " & PProveedor & ";", ConexionN, Dt) Then End
            If Dt.Rows.Count <> 0 Then
                TextSaldoInicialN.Text = FormatNumber(Dt.Rows(0).Item("Importe"), GDecimales)
            End If
        End If

        Dt.Dispose()

    End Sub
    Private Function ActualizaProveedor(ByVal DtSucursales As DataTable) As Boolean

        Dim Row As DataRowView = MiEnlazador.Current

        If ComboProducto.SelectedValue = ProveedorInsumos Then
            If Row("Comision") <> 0 Then Row("Comision") = 0
            If Row("ComisionAdicional") <> 0 Then Row("ComisionAdicional") = 0
            If Row("CondicionPago") <> 0 Then Row("CondicionPago") = 0
            If Row("TipoOperacion") <> 0 Then Row("TipoOperacion") = 0
        End If

        Dim Resul As Double = GrabaProveedor(Dt, DtExentas, DtSucursales)

        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PModificacionOk = True
            Return True
        End If
        If Resul = -1 Then
            MsgBox("Nombre Proveedor Ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = -2 Then
            MsgBox("ERROR, De Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = 0 Then
            MsgBox("ERROR, Otro usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

    End Function
    Private Function GrabaProveedor(ByVal DtProveedor As DataTable, ByVal DtExentas As DataTable, ByVal DtSucursales As DataTable) As Double

        Dim Dt As New DataTable
        UltimaClave = 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                If Not IsNothing(DtProveedor.GetChanges) Then
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Proveedores;", Miconexion)
                        Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtProveedor)
                    End Using
                    'Recupera valor de Clave(AutoIncremental) 
                    If PProveedor = 0 Then
                        If Not Tablas.Read("SELECT MAX(Clave) as Identidad FROM Proveedores;", Conexion, Dt) Then Return -2
                        UltimaClave = Dt.Rows(0).Item("Identidad")
                        For Each Row As DataRow In DtExentas.Rows
                            Row("Emisor") = UltimaClave
                        Next
                    End If
                End If
                If Not IsNothing(DtExentas.GetChanges) Then
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM RetencionesExentas;", Miconexion)
                        Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtExentas.GetChanges)
                    End Using
                End If
                If Not IsNothing(DtSucursales) Then
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM SucursalesProveedores;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtSucursales)
                    End Using
                End If
                Return 1000
            End Using
        Catch ex As OleDb.OleDbException
            If ex.ErrorCode = GAltaExiste Then
                Return -1
            Else
                Return -2
            End If
        Catch ex As DBConcurrencyException
            Return 0
        Finally
        End Try

    End Function
    Public Function Usado(ByVal Proveedor As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Proveedor) FROM FacturasProveedorCabeza WHERE Proveedor = " & Proveedor & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Proveedor) FROM IngresoMercaderiasCabeza WHERE Proveedor = " & Proveedor & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Emisor) FROM RecibosCabeza WHERE NOT (TipoNota = 5 OR TipoNota = 7 OR TipoNota = 50 OR TipoNota = 60 OR TipoNota = 65 OR TipoNota = 70) AND Emisor = " & Proveedor & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Proveedor) FROM LiquidacionCabeza WHERE Proveedor = " & Proveedor & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Proveedor) FROM RecuperoSenia WHERE Proveedor = " & Proveedor & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Proveedor) FROM OrdenCompraCabeza WHERE Proveedor = " & Proveedor & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function Valida() As Boolean

        Dim Dts As New DataTable
        If Not Tablas.Read("SELECT Clave FROM Proveedores WHERE Clave = 1;", Conexion, Dts) Then Me.Close()
        If ComboProducto.SelectedValue = 8 And Dts.Rows.Count <> 0 And Dt.Rows(0).Item("Clave") = 0 Then
            MsgBox("Proveedor de Transporte NO debe ser el primer Proveedor Informado.", MsgBoxStyle.Information)
            Return False
        End If

        If TextNombre.Text = "" Then
            MsgBox("Falta Nombre.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNombre.Focus()
            Return False
        End If

        If ComboTipoIva.SelectedValue = 0 Then
            MsgBox("Falta Tipo IVA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboTipoIva.Focus()
            Return False
        End If

        If ComboTipoIva.SelectedValue = 3 Then
            MsgBox("Consumidor Final Incorrecto para un Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboTipoIva.Focus()
            Return False
        End If

        If CheckBoxListaDePrecios.Checked And CheckBoxListaDePreciosPorZona.Checked Then
            MsgBox("Debe Seleccionar Un Solo Tipo de Lista de Precios.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            CheckBoxListaDePrecios.Focus()
            Return False
        End If

        If Dt.Rows(0).Item("Nombre") <> NombreW Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(Conexion)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Nombre) FROM Proveedores WHERE Nombre = '" & TextNombre.Text & "';", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then
                            MsgBox("Nombre Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            TextNombre.Focus()
                            Return False
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer Tabla de Proveedores.", MsgBoxStyle.Critical)
                End
            End Try
        End If

        If Dt.Rows(0).Item("Alias") <> AliasW And TextAlias.Text <> "" Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(Conexion)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Alias) FROM Proveedores WHERE Alias = '" & TextAlias.Text & "';", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then
                            MsgBox("Alias Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            TextAlias.Focus()
                            Return False
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer Tabla de Proveedor.", MsgBoxStyle.Critical)
                End
            End Try
        End If

        If ComboTipoIva.SelectedValue <> Exterior Then
            If TextCalle.Text = "" Then
                MsgBox("Falta Calle.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextCalle.Focus()
                Return False
            End If
            If TextLocalidad.Text = "" Then
                MsgBox("Falta Localidad.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextLocalidad.Focus()
                Return False
            End If
            If ComboProvincia.SelectedValue = 0 Then
                MsgBox("Falta Provincia.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboProvincia.Focus()
                Return False
            End If
        End If

        If ComboPais.SelectedValue = 0 Then
            MsgBox("Falta Pais.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboPais.Focus()
            Return False
        End If

        If ComboProducto.SelectedValue = 0 Then
            MsgBox("Falta Producto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboProducto.Focus()
            Return False
        End If
        '
        If ComboTipoIva.SelectedValue = Exterior And ComboPais.SelectedValue = Argentina Then
            MsgBox("Tipo Iva Incorrecto para Pais Argentina.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboTipoIva.Focus()
            Return False
        End If
        If ComboTipoIva.SelectedValue <> Exterior And ComboPais.SelectedValue <> Argentina Then
            MsgBox("Tipo Iva Incorrecto para Pais Informado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboTipoIva.Focus()
            Return False
        End If

        If ComboTipoIva.SelectedValue <> Exterior Then
            Dim aa As New DllVarias
            If Not aa.ValidaCuiT(MaskedCuit.Text) Then
                MsgBox("CUIT Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedCuit.Focus()
                Return False
            End If
        End If

        If Dt.Rows(0).Item("Cuit") = CuitNumerico(GCuitEmpresa) Then
            MsgBox("Cuit informado es igual al de " & GNombreEmpresa, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedCuit.Focus()
            Return False
        End If

        If Dt.Rows(0).Item("Cuit") <> CuitW And ComboTipoIva.SelectedValue <> Exterior Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(Conexion)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuit) FROM Proveedores WHERE Cuit = " & MaskedCuit.Text & ";", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then
                            If MsgBox("Cuit Ya Existe. Quiere Continuar Igualmente?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                                MaskedCuit.Focus()
                                Return False
                            End If
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer Tabla de Proveedores.", MsgBoxStyle.Critical)
                End
            End Try
        End If

        If ComboTipoIva.SelectedValue = Exterior Then
            TextCuitPais.Text = HallaCuitPais(ComboPais.SelectedValue)
            If TextCuitPais.Text = "" Then
                MsgBox("Falta Informar CUIT PAIS en Tabla de Paises.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboPais.Focus()
                Return False
            End If
            If CheckBoxListaDePrecios.Checked Or CheckBoxListaDePreciosPorZona.Checked Then
                MsgBox("Proveedores de Importación No Habilitados para Lista de Precios.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                CheckBoxListaDePrecios.Focus()
                Return False
            End If
        End If

        If ComboTipoIva.SelectedValue = Exterior And ComboMoneda.SelectedValue = 1 Then
            If MsgBox("Moneda Local No Valida para Importación.Desea Continuar?.", MsgBoxStyle.Question + MsgBoxStyle.DefaultButton3 + MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                ComboMoneda.Focus()
                Return False
            End If
        End If

        If ComboTipoIva.SelectedValue <> Exterior And ComboMoneda.SelectedValue <> 1 Then
            MsgBox("Moneda Incorrecta para Tipo Iva.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboMoneda.Focus()
            Return False
        End If

        If ComboMonedaSaldoInicial.SelectedValue <> 0 Then
            If ComboMoneda.SelectedValue <> ComboMonedaSaldoInicial.SelectedValue Then
                MsgBox("Moneda Saldo Inicial No Coincide con la Moneda en que Opera.(Debe poner a cero Saldo Inicial, Modificar Cliente y Definir Saldo Inicial Nuevamente).", MsgBoxStyle.Critical)
                ComboMoneda.Focus()
                Return False
            End If
        End If

        If ComboProducto.SelectedValue = Fruta And ComboTipoOperacion.SelectedValue = 0 Then
            MsgBox("Falta Tipo Operación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboTipoOperacion.Focus()
            Return False
        End If

        Return True

    End Function
    
End Class