Imports System.io
Imports ClassPassWord
Public Class UnCliente
    Public PCliente As Integer
    Public PActualizacionOk As Boolean
    Public PBloqueaFunciones As Boolean
    Public PDeOperacion As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim Dt As DataTable
    Dim DtSaldoInicial As DataTable
    Dim DtExentas As DataTable
    '
    Dim AliasW As String
    Dim NombreW As String
    Dim CuitW As String
    Dim UltimaClave As Integer
    Private Sub UnCliente_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(1) Then PBloqueaFunciones = True

        Dim Row As DataRow

        If PermisoTotal Then
            PanelCandado.Visible = True
            PictureCandadoOPR.Visible = True
        Else : PanelCandado.Visible = False : PictureCandadoOPR.Visible = False
        End If

        If PCliente = 0 Then
            ButtonAceptar.Text = "Graba Nuevo Cliente"
        Else : ButtonAceptar.Text = "Graba Cambios"
        End If

        PActualizacionOk = False

        ComboEstado.DataSource = DtEstadoLegajoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        LlenaComboTablas(ComboVendedor, 37)
        ComboVendedor.SelectedValue = 0

        ArmaTipoIva(ComboTipoIva)
        With ComboTipoIva
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboPais, 28)
        ComboPais.SelectedValue = Argentina
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

        LlenaComboTablas(ComboCanalDistribucion, 45)
        ComboCanalDistribucion.SelectedValue = 0
        With ComboCanalDistribucion
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboDocumentoTipo.DataSource = DtDocumentoTipo()
        ComboDocumentoTipo.DisplayMember = "Nombre"
        ComboDocumentoTipo.ValueMember = "Clave"

        ComboCanalVenta.DataSource = New DataTable
        If Not Tablas.Read("Select Clave,Nombre From Tablas WHERE Tipo = 23;", Conexion, ComboCanalVenta.DataSource) Then Me.Close() : Exit Sub
        Row = ComboCanalVenta.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboCanalVenta.DataSource.Rows.Add(Row)
        ComboCanalVenta.DisplayMember = "Nombre"
        ComboCanalVenta.ValueMember = "Clave"
        ComboCanalVenta.SelectedValue = 0
        With ComboCanalVenta
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboTipoOperacion.DataSource = Tablas.Leer("Select Clave,Nombre From Miselaneas WHERE Codigo = 4;")
        Row = ComboTipoOperacion.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboTipoOperacion.DataSource.Rows.Add(Row)
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

        If Not MuestraCliente() Then Me.Close()

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

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtSucursales As New DataTable
        Dim DtCodigos As New DataTable

        If Not ActualizaCliente(Dt, DtSucursales, DtCodigos) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        If PCliente = 0 Then PCliente = UltimaClave

        If Not MuestraCliente() Then Me.Close()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        'MsgBox("Bloqueada Momentaneamente,Consultar Administrador.")
        ' Exit Sub

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PCliente = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If GUsaNegra And Not PermisoTotal Then
            MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If TextCambio.Text <> "" Then
            MsgBox("Cliente Tiene Saldo Inicial. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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

        Dim Mensaje As String = ""
        If CDbl(TextBoniComercial.Text) <> 0 Then Mensaje = Mensaje & "Bonif.Comercial-"
        If CDbl(TextBoniLogistica.Text) <> 0 Then Mensaje = Mensaje & "Bonif.Logistica-"
        If CDbl(TextIngresoBruto.Text) <> 0 Then Mensaje = Mensaje & "Ing.Bruto-"
        If CDbl(TextImpDebCred.Text) <> 0 Then Mensaje = Mensaje & "Imp.Deb/Cred-"
        If CDbl(TextFletePorBulto.Text) <> 0 Then Mensaje = Mensaje & "Flete Por Bulto-"
        If CDbl(TextFletePorMedioBulto.Text) <> 0 Then Mensaje = Mensaje & "Flete Por Medio Bulto-"
        If CDbl(TextFletePorUnidad.Text) <> 0 Then Mensaje = Mensaje & "Flete Por Unidad-"
        If CDbl(TextFletePorKilo.Text) <> 0 Then Mensaje = Mensaje & "Flete Por Kilo-"
        If Mensaje <> "" Then
            Mensaje = "Debe Borrar las Vigencias de : " & Mensaje
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox(Mensaje, MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ListaDePrecios(PCliente) > 0 Then
            MsgBox("Debe Borrar Lista de Precios del Cliente.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If MsgBox("Cliente se eliminara Definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        MiEnlazador.Remove(MiEnlazador.Current)

        For Each Row As DataRow In DtExentas.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Row.Delete()
            End If
        Next

        Dim DtSucursales As New DataTable
        If Not Tablas.Read("SELECT * FROM SucursalesClientes WHERE Cliente = " & PCliente & ";", Conexion, DtSucursales) Then End
        For Each Row As DataRow In DtSucursales.Rows
            Row.Delete()
        Next

        Dim DtCodigosArticulos As New DataTable
        If Not Tablas.Read("SELECT * FROM CodigosCliente WHERE Cliente = " & PCliente & ";", Conexion, DtCodigosArticulos) Then End
        For Each Row As DataRow In DtCodigosArticulos.Rows
            Row.Delete()
        Next

        Dim ArchivoAgenda As String = "Cliente" & PCliente.ToString
        Dim RutaAgenda As String = System.AppDomain.CurrentDomain.BaseDirectory() & "\" & "Agenda" & GClaveEmpresa & "\" & ArchivoAgenda & ".TXT"

        If ActualizaCliente(Dt, DtSucursales, DtCodigosArticulos) Then
            If Directory.Exists(RutaAgenda) Then
                File.Delete(RutaAgenda)
            End If
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

        If PCliente = 0 Then
            MsgBox("Debe Previamente Realizar el Alta.", MsgBoxStyle.Information)
            Exit Sub
        End If

        UnSaldoInicial.PEmisor = PCliente
        UnSaldoInicial.PMoneda = ComboMoneda.SelectedValue
        UnSaldoInicial.PTipo = 3
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
                Row("TipoEmisor") = 2
                Row("Emisor") = PCliente
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
    Private Sub PictureBonificacionComercial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBonificacionComercial.Click

        If PCliente = 0 Then
            MsgBox("Debe Dar de Alta al Cliente antes de Informar este items.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Vigencias.PCodigo = 1
        Vigencias.POrigen = PCliente
        Vigencias.PBloqueaFunciones = PBloqueaFunciones
        Vigencias.ShowDialog()
        If Vigencias.PActualizacionOk Then
            Dim Valor As Double
            If Not BuscaVigencia(1, Now, Valor, PCliente) Then Me.Close()
            TextBoniComercial.Text = FormatNumber(Valor, GDecimales)
        End If
        Vigencias.Dispose()

    End Sub
    Private Sub PictureBonificacionLogistica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBonificacionLogistica.Click

        If PCliente = 0 Then
            MsgBox("Debe Dar de Alta al Cliente antes de Informar este items.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Vigencias.PCodigo = 2
        Vigencias.POrigen = PCliente
        Vigencias.PBloqueaFunciones = PBloqueaFunciones
        Vigencias.ShowDialog()
        If Vigencias.PActualizacionOk Then
            Dim Valor As Double
            If Not BuscaVigencia(2, Now, Valor, PCliente) Then Me.Close()
            TextBoniLogistica.Text = FormatNumber(Valor, GDecimales)
        End If
        Vigencias.Dispose()

    End Sub
    Private Sub PictureIngresoBruto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureIngresoBruto.Click

        If PCliente = 0 Then
            MsgBox("Debe Dar de Alta al Cliente antes de Informar este items.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Vigencias.PCodigo = 3
        Vigencias.POrigen = PCliente
        Vigencias.PBloqueaFunciones = PBloqueaFunciones
        Vigencias.ShowDialog()
        If Vigencias.PActualizacionOk Then
            Dim Valor As Double
            If Not BuscaVigencia(3, Now, Valor, PCliente) Then Me.Close()
            TextIngresoBruto.Text = FormatNumber(Valor, GDecimales)
        End If
        Vigencias.Dispose()

    End Sub
    Private Sub PictureFletePorBulto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureFletePorBulto.Click

        If PCliente = 0 Then
            MsgBox("Debe Dar de Alta al Cliente antes de Informar este items.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Vigencias.PCodigo = Bulto
        Vigencias.POrigen = PCliente
        Vigencias.PBloqueaFunciones = PBloqueaFunciones
        Vigencias.ShowDialog()
        If Vigencias.PActualizacionOk Then
            Dim Valor As Double
            If Not BuscaVigencia(Bulto, Now, Valor, PCliente) Then Me.Close()
            TextFletePorBulto.Text = FormatNumber(Valor, GDecimales)
        End If
        Vigencias.Dispose()

    End Sub
    Private Sub PicturePorMedioBulto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PicturePorMedioBulto.Click

        If PCliente = 0 Then
            MsgBox("Debe Dar de Alta al Cliente antes de Informar este items.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Vigencias.PCodigo = MedioBulto
        Vigencias.POrigen = PCliente
        Vigencias.PBloqueaFunciones = PBloqueaFunciones
        Vigencias.ShowDialog()
        If Vigencias.PActualizacionOk Then
            Dim Valor As Double
            If Not BuscaVigencia(MedioBulto, Now, Valor, PCliente) Then Me.Close()
            TextFletePorMedioBulto.Text = FormatNumber(Valor, GDecimales)
        End If
        Vigencias.Dispose()

    End Sub
    Private Sub PictureFletePorUnidad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureFletePorUnidad.Click

        If PCliente = 0 Then
            MsgBox("Debe Dar de Alta al Cliente antes de Informar este items.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Vigencias.PCodigo = Unidad
        Vigencias.POrigen = PCliente
        Vigencias.PBloqueaFunciones = PBloqueaFunciones
        Vigencias.ShowDialog()
        If Vigencias.PActualizacionOk Then
            Dim Valor As Double
            If Not BuscaVigencia(Unidad, Now, Valor, PCliente) Then Me.Close()
            TextFletePorUnidad.Text = FormatNumber(Valor, GDecimales)
        End If
        Vigencias.Dispose()

    End Sub
    Private Sub PictureFletePorKilo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureFletePorKilo.Click

        If PCliente = 0 Then
            MsgBox("Debe Dar de Alta al Cliente antes de Informar este items.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Vigencias.PCodigo = Kilo
        Vigencias.POrigen = PCliente
        Vigencias.PBloqueaFunciones = PBloqueaFunciones
        Vigencias.ShowDialog()
        If Vigencias.PActualizacionOk Then
            Dim Valor As Double
            If Not BuscaVigencia(Kilo, Now, Valor, PCliente) Then Me.Close()
            TextFletePorKilo.Text = FormatNumber(Valor, GDecimales)
        End If
        Vigencias.Dispose()

    End Sub
    Private Sub PictureDebitoCredito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureDebitoCredito.Click

        If PCliente = 0 Then
            MsgBox("Debe Dar de Alta al Cliente antes de Informar este items.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Vigencias.PCodigo = 5
        Vigencias.POrigen = PCliente
        Vigencias.PBloqueaFunciones = PBloqueaFunciones
        Vigencias.ShowDialog()
        If Vigencias.PActualizacionOk Then
            Dim Valor As Double
            If Not BuscaVigencia(5, Now, Valor, PCliente) Then Me.Close()
            TextImpDebCred.Text = FormatNumber(Valor, GDecimales)
        End If
        Vigencias.Dispose()

    End Sub
    Private Sub PictureSucursales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureSucursales.Click

        If PCliente = 0 Then
            MsgBox("Debe dar de Alta al Cliente.", MsgBoxStyle.Information)
            Exit Sub
        End If

        UnaTablaSucursalcliente.PBloqueaFunciones = PBloqueaFunciones
        UnaTablaSucursalcliente.PCliente = PCliente
        UnaTablaSucursalcliente.ShowDialog()

    End Sub
    Private Sub PictureArticulos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureArticulos.Click

        If PCliente = 0 Then
            MsgBox("Debe dar de Alta al Cliente.", MsgBoxStyle.Information)
            Exit Sub
        End If

        UnaTablaArticulosCliente.PCliente = PCliente
        UnaTablaArticulosCliente.PBloqueaFunciones = PBloqueaFunciones
        UnaTablaArticulosCliente.PTieneCodigo = CheckBoxTieneCodigoCliente.Checked
        UnaTablaArticulosCliente.ShowDialog()
        UnaTablaArticulosCliente.Dispose()

    End Sub
    Private Sub ButtonAgenda_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAgenda.Click

        If Not PermisoEscritura(1) Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PCliente = 0 Then
            MsgBox("Debe dar de Alta al Cliente", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        UnaAgenda.PArchivo = "Cliente" & PCliente.ToString
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
    Private Sub Textnumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumero.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, 0)

    End Sub
    Private Sub ComboTipoOperacion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipoOperacion.Validating

        If IsNothing(ComboTipoOperacion.SelectedValue) Then ComboTipoOperacion.SelectedValue = 0

    End Sub
    Private Sub ComboProvincia_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProvincia.Validating

        If IsNothing(ComboProvincia.SelectedValue) Then ComboProvincia.SelectedValue = 0

    End Sub
    Private Sub ComboCanalVenta_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCanalVenta.Validating

        If IsNothing(ComboCanalVenta.SelectedValue) Then ComboCanalVenta.SelectedValue = 0

    End Sub
    Private Sub ComboCanalDistribucion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCanalDistribucion.Validating

        If IsNothing(ComboCanalDistribucion.SelectedValue) Then ComboCanalDistribucion.SelectedValue = 0

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
    Private Sub ComboPais_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPais.Validating

        If IsNothing(ComboPais.SelectedValue) Then ComboPais.SelectedValue = 0

    End Sub
    Private Sub ComboMoneda_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboMoneda.Validating

        If IsNothing(ComboMoneda.SelectedValue) Then ComboMoneda.SelectedValue = 1

    End Sub
    Private Sub TextCondicionPago_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCondicionPago.KeyPress

        EsNumerico(e.KeyChar, TextCondicionPago.Text, 0)

    End Sub
    Private Sub TextBoniComercial_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBoniComercial.KeyPress

        EsNumerico(e.KeyChar, TextBoniComercial.Text, GDecimales)

    End Sub
    Private Sub TextBoniLogistica_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBoniLogistica.KeyPress

        EsNumerico(e.KeyChar, TextBoniLogistica.Text, GDecimales)

    End Sub
    Private Sub TextDirecto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextDirecto.KeyPress

        If Asc(e.KeyChar) = 13 Then TextDirecto_Validating(Nothing, Nothing) : Exit Sub

        EsNumerico(e.KeyChar, TextDirecto.Text, GDecimales)

    End Sub
    Private Sub TextCodPostal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCodPostal.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

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
    Private Sub TextIngresoBruto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextIngresoBruto.KeyPress

        EsNumerico(e.KeyChar, TextIngresoBruto.Text, GDecimales)

    End Sub
    Private Sub TextFlete_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextFletePorBulto.KeyPress

        EsNumerico(e.KeyChar, TextFletePorBulto.Text, GDecimales)

    End Sub
    Private Sub TextImpDebCred_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextImpDebCred.KeyPress

        EsNumerico(e.KeyChar, TextImpDebCred.Text, GDecimales)

    End Sub
    Private Sub TextSaldoInicial_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSaldoInicial.KeyPress

        EsNumericoConSigno(e.KeyChar, TextSaldoInicial.Text, GDecimales)

    End Sub
    Private Sub TextSaldoInicialN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSaldoInicialN.KeyPress

        EsNumericoConSigno(e.KeyChar, TextSaldoInicialN.Text, GDecimales)

    End Sub
    Private Sub TextCambio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCambio.KeyPress

        EsNumericoConSigno(e.KeyChar, TextCambio.Text, 3)

    End Sub
    Private Sub TextDescuento_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextDescuento.KeyPress

        EsPorcentaje(e.KeyChar, TextDescuento.Text)

    End Sub
    Private Sub TextDescuento_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextDescuento.Validating

        If TextDescuento.Text = "" Then Exit Sub

        If Not CDbl(TextDescuento.Text) < 100 Then
            MsgBox("Dato Erroneo. Descuento debe ser menor a 100.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextDescuento.Focus()
            Exit Sub
        End If

    End Sub
    Private Sub ComboDocumentoTipo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDocumentoTipo.Validating

        If IsNothing(ComboDocumentoTipo.SelectedValue) Then ComboDocumentoTipo.SelectedValue = 0

    End Sub
    Private Sub TextDocumneto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextDocumento.KeyPress

        EsNumerico(e.KeyChar, TextDocumento.Text, 0)

    End Sub
    Private Function MuestraCabeza() As Boolean

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = Dt

        Dim Enlace As Binding

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Nombre")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextNombre.DataBindings.Clear()
        TextNombre.DataBindings.Add(Enlace)
        NombreW = TextNombre.Text

        Enlace = New Binding("SelectedValue", MiEnlazador, "Vendedor")
        ComboVendedor.DataBindings.Clear()
        ComboVendedor.DataBindings.Add(Enlace)

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

        Enlace = New Binding("Text", MiEnlazador, "CodPostal")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextCodPostal.DataBindings.Clear()
        TextCodPostal.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Provincia")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        ComboProvincia.DataBindings.Clear()
        ComboProvincia.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Pais")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        ComboPais.DataBindings.Clear()
        ComboPais.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Telefonos")
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

        Enlace = New Binding("SelectedValue", MiEnlazador, "TipoOperacion")
        ComboTipoOperacion.DataBindings.Clear()
        ComboTipoOperacion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "CanalVenta")
        ComboCanalVenta.DataBindings.Clear()
        ComboCanalVenta.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "CanalDistribucion")
        ComboCanalDistribucion.DataBindings.Clear()
        ComboCanalDistribucion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "TipoIva")
        ComboTipoIva.DataBindings.Clear()
        ComboTipoIva.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Directo")
        AddHandler Enlace.Format, AddressOf FormatNumerico
        TextDirecto.DataBindings.Clear()
        TextDirecto.DataBindings.Add(Enlace)
        TextAutorizar.Text = Format(100 - CDbl(TextDirecto.Text), "0.00")

        Enlace = New Binding("Text", MiEnlazador, "Descuento")
        AddHandler Enlace.Format, AddressOf FormatNumerico
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextDescuento.DataBindings.Clear()
        TextDescuento.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "TipoOperacion")
        ComboTipoOperacion.DataBindings.Clear()
        ComboTipoOperacion.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CondicionPago")
        TextCondicionPago.DataBindings.Clear()
        TextCondicionPago.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "CtaCteCerrada")
        CheckBoxCtaCteCerrada.DataBindings.Clear()
        CheckBoxCtaCteCerrada.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "TieneCodigoCliente")
        CheckBoxTieneCodigoCliente.DataBindings.Clear()
        CheckBoxTieneCodigoCliente.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "EsDelGrupo")
        CheckBoxEsDelGrupo.DataBindings.Clear()
        CheckBoxEsDelGrupo.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "ListaDePrecios")
        CheckBoxListaDePrecios.DataBindings.Clear()
        CheckBoxListaDePrecios.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "ListaDePreciosPorZona")
        CheckBoxListaDePreciosPorZona.DataBindings.Clear()
        CheckBoxListaDePreciosPorZona.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "ListaDePreciosPorVendedor")
        CheckBoxListaDePreciosPorVendedor.DataBindings.Clear()
        CheckBoxListaDePreciosPorVendedor.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Alias")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextAlias.DataBindings.Clear()
        TextAlias.DataBindings.Add(Enlace)
        AliasW = TextAlias.Text

        Enlace = New Binding("Text", MiEnlazador, "CodigoExterno")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextCodigoExterno.DataBindings.Clear()
        TextCodigoExterno.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Moneda")
        ComboMoneda.DataBindings.Clear()
        ComboMoneda.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "EsFCE")
        CheckBoxEsFCE.DataBindings.Clear()
        CheckBoxEsFCE.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "DocumentoTipo")
        ComboDocumentoTipo.DataBindings.Clear()
        ComboDocumentoTipo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "DocumentoNumero")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextDocumento.DataBindings.Clear()
        TextDocumento.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "TextoFijoParaFacturas1")
        TextTextoFijoParaFacturas1.DataBindings.Clear()
        TextTextoFijoParaFacturas1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "TextoFijoParaFacturas2")
        TextTextoFijoParaFacturas2.DataBindings.Clear()
        TextTextoFijoParaFacturas2.DataBindings.Add(Enlace)

        Dim Valor As Double

        If Not BuscaVigencia(1, Now, Valor, PCliente) Then Return False
        TextBoniComercial.Text = FormatNumber(Valor, GDecimales)
        If Not BuscaVigencia(2, Now, Valor, PCliente) Then Return False
        TextBoniLogistica.Text = FormatNumber(Valor, GDecimales)
        If Not BuscaVigencia(3, Now, Valor, PCliente) Then Return False
        TextIngresoBruto.Text = FormatNumber(Valor, GDecimales)
        If Not BuscaVigencia(5, Now, Valor, PCliente) Then Return False
        TextImpDebCred.Text = FormatNumber(Valor, GDecimales)
        '        If Not BuscaVigencia(4, Now, Valor, PCliente) Then Return False
        '        TextFletePorBulto.Text = FormatNumber(Valor, GDecimales)
        If Not BuscaVigencia(Bulto, Now, Valor, PCliente) Then Return False
        TextFletePorBulto.Text = FormatNumber(Valor, GDecimales)
        If Not BuscaVigencia(MedioBulto, Now, Valor, PCliente) Then Return False
        TextFletePorMedioBulto.Text = FormatNumber(Valor, GDecimales)
        If Not BuscaVigencia(Unidad, Now, Valor, PCliente) Then Return False
        TextFletePorUnidad.Text = FormatNumber(Valor, GDecimales)
        If Not BuscaVigencia(Kilo, Now, Valor, PCliente) Then Return False
        TextFletePorKilo.Text = FormatNumber(Valor, GDecimales)

        If ComboPais.SelectedValue <> Argentina Then
            TextCuitPais.Text = HallaCuitPais(ComboPais.SelectedValue)
        End If

        If PCliente <> 0 Then
            MuestraSaldos()
        End If

        If Dt.Rows(0).Item("Opr") Then
            PictureCandadoOPR.Image = ImageList1.Images.Item("Abierto")
        Else : PictureCandadoOPR.Image = ImageList1.Images.Item("Cerrado")
        End If

        Return True

    End Function
    Private Sub ParseTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Sub FormatCambio(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, 3)

    End Sub
    Private Sub FormatCuit(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "00000000000")

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then Numero.Value = ""

    End Sub
    Private Sub FormatNumerico(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub ParseCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub MuestraSaldos()

        Dim Dt As New DataTable

        TextSaldoInicialN.Text = ""
        TextSaldoInicial.Text = ""
        TextCambio.Text = ""
        ComboMonedaSaldoInicial.SelectedValue = 0

        If Not Tablas.Read("SELECT * FROM SaldosInicialesCabeza WHERE Tipo = 3 AND Emisor = " & PCliente & ";", Conexion, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            TextSaldoInicial.Text = FormatNumber(Dt.Rows(0).Item("Importe"), GDecimales)
            TextCambio.Text = FormatNumber(Dt.Rows(0).Item("Cambio"), 3)
            ComboMonedaSaldoInicial.SelectedValue = Dt.Rows(0).Item("Moneda")
        End If
        If PermisoTotal Then
            Dt = New DataTable
            If Not Tablas.Read("SELECT * FROM SaldosInicialesCabeza WHERE Tipo = 3 AND Emisor = " & PCliente & ";", ConexionN, Dt) Then End
            If Dt.Rows.Count <> 0 Then
                TextSaldoInicialN.Text = FormatNumber(Dt.Rows(0).Item("Importe"), GDecimales)
            End If
        End If

        Dt.Dispose()

    End Sub
    Private Sub AgregaRegistro()

        Dim Row As DataRow = Dt.NewRow
        Row("Nombre") = ""
        Row("Calle") = ""
        Row("Numero") = 0
        Row("Localidad") = ""
        Row("Provincia") = 0
        Row("CodPostal") = 0
        Row("Pais") = Argentina
        Row("Telefonos") = ""
        Row("Faxes") = ""
        Row("Cuit") = 0
        Row("TipoIva") = 0
        Row("CondicionPago") = 0
        Row("CanalVenta") = 0
        Row("CanalDistribucion") = 0
        Row("TipoOperacion") = 0
        Row("Directo") = 100
        Row("Estado") = 1
        Row("CtaCteCerrada") = False
        Row("TieneCodigoCliente") = False
        Row("ListaDePrecios") = 0
        Row("ListaDePreciosPorZona") = 0
        Row("ListaDePreciosPorVendedor") = False
        Row("Vendedor") = 0
        Row("Alias") = ""
        Row("SaldoInicial") = 0
        Row("Cambio") = 1
        Row("Moneda") = 1
        Row("EsDelGrupo") = False
        Row("DeOperacion") = PDeOperacion
        Row("CodigoExterno") = ""
        Row("Descuento") = 0
        Row("Opr") = True
        Row("EsFCE") = False
        Row("DocumentoTipo") = 0
        Row("DocumentoNumero") = 0
        Row("TextoFijoParaFacturas1") = ""
        Row("TextoFijoParaFacturas2") = ""
        Dt.Rows.Add(Row)

    End Sub
    Private Function MuestraCliente() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dt = New DataTable

        If Not Tablas.Read("SELECT * FROM Clientes Where Clave = " & PCliente & ";", Conexion, Dt) Then Return False
        If Dt.Rows.Count = 0 And PCliente <> 0 Then
            MsgBox("ERROR Cliente no Existe. Opreacion se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Dt.Rows.Count = 0 Then AgregaRegistro()

        If Dt.Rows(0).Item("DeOperacion") Then
            LabelDeOperacion.Visible = True
        Else
            LabelDeOperacion.Visible = False
        End If

        DtExentas = New DataTable
        If Not Tablas.Read("SELECT * FROM RetencionesExentas Where TipoEmisor = 2 AND Emisor = " & PCliente & ";", Conexion, DtExentas) Then Return False
        If PCliente = 0 Then
            If Not ArmaDtRetencionesExentas(2, DtExentas) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Error al Generar Retenciones/Percepciones Exentas. Salga e informe las no Exentas.", MsgBoxStyle.Information)
                Return False
            End If
        End If

        If Not MuestraCabeza() Then Return False

        If ComboTipoIva.SelectedValue = Exterior Then
            MaskedCuit.Enabled = False
        End If

        If PCliente = 0 Then
            ComboEstado.Enabled = False
        Else
            ComboEstado.Enabled = True
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Function ActualizaCliente(ByVal dt As DataTable, ByVal DtSucursales As DataTable, ByVal DtCodigos As DataTable) As Boolean

        Dim Resul As Double = GrabaCliente(dt, DtSucursales, DtCodigos)

        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PActualizacionOk = True
            Return True
        End If
        If Resul = -1 Then
            MsgBox("Cliente o Cuit ya existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
    Private Function GrabaCliente(ByVal DtCliente As DataTable, ByVal DtSucursales As DataTable, ByVal DtCodigos As DataTable) As Integer

        Dim Dt As New DataTable
        UltimaClave = 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                If Not IsNothing(DtCliente) Then
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Clientes;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtCliente)
                    End Using
                    'Recupera valor de Clave(AutoIncremental) 
                    If PCliente = 0 Then
                        If Not Tablas.Read("SELECT MAX(Clave) as Identidad FROM Clientes;", Conexion, Dt) Then Return -2
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
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM SucursalesClientes;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtSucursales)
                    End Using
                End If
                If Not IsNothing(DtCodigos) Then
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM CodigosCliente;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtCodigos)
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
    Public Function Usado(ByVal Cliente As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cliente) FROM FacturasCabeza WHERE Cliente = " & Cliente & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cliente) FROM RemitosCabeza WHERE Cliente = " & Cliente & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Emisor) FROM RecibosCabeza WHERE (TipoNota = 5 OR TipoNota = 7 OR TipoNota = 50 OR TipoNota = 60 OR TipoNota = 65 OR TipoNota = 70 OR TipoNota = 64) AND Emisor = " & Cliente & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Emisor) FROM PrestamosCabeza WHERE Origen = 3 AND Emisor = " & Cliente & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Emisor) FROM CompraDivisasCabeza WHERE Origen = 3 AND Emisor = " & Cliente & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Emisor) FROM SaldosInicialesCabeza WHERE Tipo = 3 AND Emisor = " & Cliente & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Liquidacion) FROM NVLPCabeza WHERE Cliente = " & Cliente & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Nota) FROM RecibosExteriorCabeza WHERE (TipoNota = 11 or TipoNOta = 12) AND Emisor = " & Cliente & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                If ConexionStr = Conexion Then
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Lista) FROM ListaDePreciosCabeza WHERE Cliente = " & Cliente & ";", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then Return True
                    End Using
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Pedido) FROM PedidosCabeza WHERE Cliente = " & Cliente & ";", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then Return True
                    End Using
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Nota) FROM LiquidacionDivisasCabeza WHERE Emisor = " & Cliente & ";", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then Return True
                    End Using
                End If
                If ConexionStr = ConexionN Then
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cliente) FROM CierreFacturasCabeza WHERE Cliente = " & Cliente & ";", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then Return True
                    End Using
                End If
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
                Return False
            End Try
        End Using

    End Function
    Public Function ListaDePrecios(ByVal Cliente As Integer) As Integer


        Dim Miconexion As New OleDb.OleDbConnection(Conexion)

        Try
            Miconexion.Open()
            Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cliente) FROM ListaDePreciosCabeza WHERE Cliente = " & Cliente & ";", Miconexion)
                Return Cmd.ExecuteScalar()
            End Using
        Catch ex As Exception
            Return False
        Finally
            If Miconexion.State = ConnectionState.Open Then Miconexion.Close()
        End Try

    End Function
    Public Function DtDocumentoTipo() As DataTable

        Dim Dt As New DataTable

        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Dim Row As DataRow
        Try
            Row = Dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 86
            Row("Nombre") = "CUIL"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 87
            Row("Nombre") = "CDI"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 89
            Row("Nombre") = "LE"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 90
            Row("Nombre") = "LC"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 91
            Row("Nombre") = "CI-Extranjera"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 94
            Row("Nombre") = "PASAPORTE"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 96
            Row("Nombre") = "DNI"
            Dt.Rows.Add(Row)
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Private Function Valida() As Boolean

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

        Dim Conta As Integer = 0
        If CheckBoxListaDePrecios.Checked Then Conta = Conta + 1
        If CheckBoxListaDePreciosPorZona.Checked Then Conta = Conta + 1
        If CheckBoxListaDePreciosPorVendedor.Checked Then Conta = Conta + 1

        If Conta > 1 Then
            MsgBox("Debe Seleccionar Un Solo Tipo de Lista de Precios.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            CheckBoxListaDePrecios.Focus()
            Return False
        End If

        If Dt.Rows(0).Item("Nombre") <> NombreW Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(Conexion)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Nombre) FROM Clientes WHERE Nombre = '" & TextNombre.Text & "';", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then
                            MsgBox("Nombre Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            TextNombre.Focus()
                            Return False
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer Tabla de Clientes.", MsgBoxStyle.Critical)
                End
            End Try
        End If

        If Dt.Rows(0).Item("Alias") <> AliasW And TextAlias.Text <> "" Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(Conexion)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Alias) FROM Clientes WHERE Alias = '" & TextAlias.Text & "';", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then
                            MsgBox("Alias Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            TextAlias.Focus()
                            Return False
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer Tabla de Clientes.", MsgBoxStyle.Critical)
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
        '
        If ComboPais.SelectedValue = 0 Then
            MsgBox("Falta Pais.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboPais.Focus()
            Return False
        End If
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

        If ComboTipoIva.SelectedValue <> Exterior And ComboTipoIva.SelectedValue <> 3 Then
            Dim aa As New DllVarias
            If Not aa.ValidaCuiT(MaskedCuit.Text) Then
                MsgBox("CUIT Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedCuit.Focus()
                Return False
            End If
        End If

        If ComboTipoIva.SelectedValue = 3 And MaskedCuit.Text <> "00000000000" Then
            MsgBox("Cuit debe ser 00-00000000-0 para Consumidor Final.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboMoneda.Focus()
            Return False
        End If

        If ComboTipoIva.SelectedValue = Exterior Then
            TextCuitPais.Text = HallaCuitPais(ComboPais.SelectedValue)
            If TextCuitPais.Text = "" Then
                MsgBox("Falta Informar CUIT PAIS en Tabla de Paises.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboPais.Focus()
                Return False
            End If
            If CheckBoxListaDePrecios.Checked Or CheckBoxListaDePreciosPorZona.Checked Or CheckBoxListaDePreciosPorVendedor.Checked Then
                MsgBox("Clientes de Exportación No Habilitados para Lista de Precios.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                CheckBoxListaDePrecios.Focus()
                Return False
            End If
        End If

        If ComboTipoIva.SelectedValue = Exterior And ComboMoneda.SelectedValue = 1 Then
            If MsgBox("Moneda Local No Valida Para Exportación. Desea Continuar?.", MsgBoxStyle.Question + MsgBoxStyle.DefaultButton3 + MsgBoxStyle.YesNo) = MsgBoxResult.No Then
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
        '
        If ComboCanalVenta.SelectedValue = 0 Then
            MsgBox("Falta Canal de Venta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboCanalVenta.Focus()
            Return False
        End If

        If ComboTipoOperacion.SelectedValue = 0 Then
            MsgBox("Falta Tipo Operación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboTipoOperacion.Focus()
            Return False
        End If

        If Dt.Rows(0).Item("Cuit") = CuitNumerico(GCuitEmpresa) Then   'anulada por ahora.
            If MsgBox("Cuit Informado es igual al de " & GNombreEmpresa & " Quiere Continuar Igualmente?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                MaskedCuit.Focus()
                Return False
            End If
        End If

        If Dt.Rows(0).Item("Cuit") <> CuitW And ComboTipoIva.SelectedValue <> Exterior Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(Conexion)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuit) FROM Clientes WHERE Cuit = " & MaskedCuit.Text & ";", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then
                            If MsgBox("Cuit Ya Existe. Quiere Continuar Igualmente?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                                MaskedCuit.Focus()
                                Return False
                            End If
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer Tabla de Clientes.", MsgBoxStyle.Critical)
                End
            End Try
        End If

        If Dt.Rows(0).Item("DeOperacion") And ComboTipoIva.SelectedValue <> Exterior Then
            MsgBox("Cliente de Operación debe ser del Exterior.", MsgBoxStyle.Critical)
            Return False
        End If

        If Dt.Rows(0).Item("DocumentoTipo") <> 0 And Dt.Rows(0).Item("DocumentoNumero") = 0 Then
            MsgBox("Falta Numero Documento.", MsgBoxStyle.Critical)
            Return False
        End If
        If Dt.Rows(0).Item("DocumentoTipo") = 0 And Dt.Rows(0).Item("DocumentoNumero") <> 0 Then
            MsgBox("Falta Tipo Documento.", MsgBoxStyle.Critical)
            Return False
        End If

        Return True

    End Function

   
    
   
End Class