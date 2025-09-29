Imports ClassPassWord
Public Class UnLegajo
    Public PLegajo As Integer
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Dim Dt As DataTable
    '
    Dim ConexionLegajo As String
    Private Sub UnEmpleado_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(9) Then PBloqueaFunciones = True

        Me.Top = 50

        If Not PermisoTotal Then PictureCandado.Visible = False : PanelBrutoN.Visible = False

        If PLegajo <> 0 Then
            If PAbierto Then PictureCandado.Image = ImageList1.Images.Item("Abierto")
            If Not PAbierto Then PictureCandado.Image = ImageList1.Images.Item("Cerrado")
        Else
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

        If PAbierto Then
            ConexionLegajo = Conexion
        Else : ConexionLegajo = ConexionN
        End If

        LlenaComboTablas(ComboPais, 28)
        ComboPais.SelectedValue = 0
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

        ComboEstado.DataSource = DtEstadoLegajoActivoYBaja()
        Dim Row As DataRow = ComboEstado.DataSource.NewRow()
        Row("Clave") = 2
        Row("Nombre") = "Suspendido"
        ComboEstado.DataSource.Rows.Add(Row)
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        LlenaComboTablas(ComboBancos, 26)
        With ComboBancos
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboBancosPago, 26)
        With ComboBancosPago
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboObraSocial, 46)
        With ComboObraSocial
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        GModificacionOk = False

        If Not ArmaArchivos() Then Me.Close()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        If IsNothing(Dt.GetChanges) Then MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3) : Exit Sub

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PLegajo = 0 Then
            HacerAlta()
        Else
            Dim Resul As Integer = ActualizaEmpleado(Dt, ConexionLegajo)
            If Resul = -1 Then
                MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If Resul = 0 Then
                MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If Resul > 0 Then
                MsgBox("Cambios Realizado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                GModificacionOk = True
                If Not ArmaArchivos() Then Me.Close() : Exit Sub
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBaja_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBaja.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PLegajo = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Legajo Esta Dado de BAJA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not IsNothing(Dt.GetChanges) Then MsgBox("Hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3) : Exit Sub

        If MsgBox("Legajo se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim DtAux As DataTable = Dt.Copy
        DtAux.Rows(0).Item("Estado") = 3
        DtAux.Rows(0).Item("FechaBaja") = Date.Now

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim resul As Integer = ActualizaEmpleado(DtAux, ConexionLegajo)
        If resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If resul > 0 Then
            MsgBox("Cambios Realizado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            If Not ArmaArchivos() Then Me.Close()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnulaBaja_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnulaBaja.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PLegajo = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 1 Then
            MsgBox("Legajo Esta ACTIVO. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not IsNothing(Dt.GetChanges) Then MsgBox("Hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3) : Exit Sub

        Dim DtAux As DataTable = Dt.Copy
        DtAux.Rows(0).Item("Estado") = 1
        DtAux.Rows(0).Item("FechaBaja") = "01/01/1800"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim resul As Integer = ActualizaEmpleado(DtAux, ConexionLegajo)
        If resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If resul > 0 Then
            MsgBox("Cambios Realizado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            If Not ArmaArchivos() Then Me.Close()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonListaFamiliares_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonListaFamiliares.Click

        If PLegajo = 0 Then
            MsgBox("Debe dar de Alta al Legajo.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ListaFamiliares.PLegajo = PLegajo
        ListaFamiliares.PNombre = TextNombres.Text
        ListaFamiliares.PApellido = TextApellidos.Text

        ListaFamiliares.ShowDialog()
        ListaFamiliares.Dispose()

    End Sub
    Private Sub PictureCuentaContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureCuentaContable.Click

        SeleccionarCuenta.PCentro = 0
        SeleccionarCuenta.ShowDialog()
        If SeleccionarCuenta.PCuenta <> 0 Then
            TextCuentaContable.Text = Format(SeleccionarCuenta.PCuenta, "000-000000-00")
        End If
        SeleccionarCuenta.Dispose()

    End Sub
    Private Sub PictureCuentaPago_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCuentaPago.Click

        ListaBancos.PEsSeleccionaCuenta = True
        ListaBancos.PEsSoloPesos = True
        ListaBancos.ShowDialog()
        If ListaBancos.PCuenta <> 0 Then
            ComboBancosPago.SelectedValue = ListaBancos.PBanco
            TextCuentaPago.Text = ListaBancos.PCuenta
        End If
        ListaBancos.Dispose()

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PLegajo <> 0 Then Exit Sub

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            PAbierto = False
            ConexionLegajo = ConexionN
        Else
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
            ConexionLegajo = Conexion
        End If

    End Sub
    Private Sub PictureAlmanaque_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaque.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaNacimiento.Text = ""
        Else : TextFechaNacimiento.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If

        Calendario.Dispose()

    End Sub
    Private Sub ButtonBorraCtaCta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorraCtaCta.Click

        TextCuentaContable.Text = ""

    End Sub
    Private Sub TextNombres_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNombres.KeyPress

        If ValidaStringNombres(e.KeyChar) <> "" Then
            e.KeyChar = ""
        End If

    End Sub
    Private Sub TextApellidos_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextApellidos.KeyPress

        If ValidaStringNombres(e.KeyChar) <> "" Then
            e.KeyChar = ""
        End If

    End Sub
    Private Sub TextSaldoInicial_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSaldoInicial.KeyPress

        If InStr("-0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextBruto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBruto.KeyPress

        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextBrutoN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBrutoN.KeyPress

        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextDni_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextDni.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCuenta_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCuenta.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCbu_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCbu.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM Empleados Where Legajo = " & PLegajo & ";", ConexionLegajo, Dt) Then Return False
        If Dt.Rows.Count = 0 And PLegajo <> 0 Then
            MsgBox("ERROR Legajo no Existe. Opreacion se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Dt.Rows.Count = 0 Then AgregaRegistro()

        MuestraCabeza()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = Dt

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Legajo")
        AddHandler Enlace.Format, AddressOf FormatEntero
        TextLegajo.DataBindings.Clear()
        TextLegajo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Nombres")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextNombres.DataBindings.Clear()
        TextNombres.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Apellidos")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextApellidos.DataBindings.Clear()
        TextApellidos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Dni")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseNumerico
        TextDni.DataBindings.Clear()
        TextDni.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cuil")
        AddHandler Enlace.Format, AddressOf FormatCuil
        MaskedCuil.DataBindings.Clear()
        MaskedCuil.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaAlta")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaBaja")
        DateTime2.DataBindings.Clear()
        DateTime2.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Calle")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextCalle.DataBindings.Clear()
        TextCalle.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CalleEntre")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextCalleEntre.DataBindings.Clear()
        TextCalleEntre.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CalleY")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextCalleY.DataBindings.Clear()
        TextCalleY.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Localidad")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextLocalidad.DataBindings.Clear()
        TextLocalidad.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Provincia")
        ComboProvincia.DataBindings.Clear()
        ComboProvincia.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Pais")
        ComboPais.DataBindings.Clear()
        ComboPais.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Telefonos")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextTelefonos.DataBindings.Clear()
        TextTelefonos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Faxes")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextFaxes.DataBindings.Clear()
        TextFaxes.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Bruto")
        AddHandler Enlace.Format, AddressOf FormatNumerico
        AddHandler Enlace.Parse, AddressOf ParseNumerico
        TextBruto.DataBindings.Clear()
        TextBruto.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Bruto2")
        AddHandler Enlace.Format, AddressOf FormatNumerico
        AddHandler Enlace.Parse, AddressOf ParseNumerico
        TextBrutoN.DataBindings.Clear()
        TextBrutoN.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "SaldoInicial")
        AddHandler Enlace.Format, AddressOf FormatNumerico
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextSaldoInicial.DataBindings.Clear()
        TextSaldoInicial.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Banco")
        ComboBancos.DataBindings.Clear()
        ComboBancos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cuenta")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseNumerico
        TextCuenta.DataBindings.Clear()
        TextCuenta.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cbu")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextCbu.DataBindings.Clear()
        TextCbu.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "ObraSocial")
        ComboObraSocial.DataBindings.Clear()
        ComboObraSocial.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CuentaContable")
        AddHandler Enlace.Format, AddressOf FormatCuenta
        AddHandler Enlace.Parse, AddressOf ParseCuenta
        TextCuentaContable.DataBindings.Clear()
        TextCuentaContable.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        If Dt.Rows(0).Item("Estado") = 3 Then
            PanelFechaBaja.Visible = True
        Else : PanelFechaBaja.Visible = False
        End If

        Enlace = New Binding("Checked", MiEnlazador, "PagoEnPesos")
        RadioPagoEnPesos.DataBindings.Clear()
        RadioPagoEnPesos.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "PagoEnTransferencia")
        RadioPagoEnTransferencia.DataBindings.Clear()
        RadioPagoEnTransferencia.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "BancoPago")
        ComboBancosPago.DataBindings.Clear()
        ComboBancosPago.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CuentaPago")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseNumerico
        TextCuentaPago.DataBindings.Clear()
        TextCuentaPago.DataBindings.Add(Enlace)

        If Not Dt.Rows(0).Item("PagoEnPesos") And Not Dt.Rows(0).Item("PagoEnTransferencia") Then
            RadioNinguno.Checked = True
        End If

        '--------------------------
        Enlace = New Binding("Text", MiEnlazador, "FechaNacimiento")
           AddHandler Enlace.Format, AddressOf FormatFecha
        AddHandler Enlace.Parse, AddressOf ParseFecha
        TextFechaNacimiento.DataBindings.Clear()
        TextFechaNacimiento.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Funcion")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextFuncion.DataBindings.Clear()
        TextFuncion.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Categoria")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextCategoria.DataBindings.Clear()
        TextCategoria.DataBindings.Add(Enlace)

    End Sub
    Private Sub ParseTexto(ByVal sender As Object, ByVal Nombre As ConvertEventArgs)

        Nombre.Value = Nombre.Value.ToString.Trim

    End Sub
    Private Sub FormatNumerico(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(0, "#")
        Else
            Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(0, "#")
        Else
            Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub FormatCuenta(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(0, "#")
        Else
            Numero.Value = Format(Numero.Value, "000-000000-00")
        End If

    End Sub
    Private Sub ParseNumerico(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub ParseCuenta(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then
            Numero.Value = 0
        Else
            Numero.Value = Mid$(TextCuentaContable.Text, 1, 3) & Mid$(TextCuentaContable.Text, 5, 6) & Mid$(TextCuentaContable.Text, 12, 2)
        End If

    End Sub
    Private Sub FormatCuil(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "00000000000")

    End Sub
    Private Sub ParseCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatFecha(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "1800/01/01" Then
            Numero.Value = ""
        Else
            Numero.Value = Format(Numero.Value, "dd/MM/yyyy")
        End If

    End Sub
    Private Sub ParseFecha(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = "1800/01/01" : Exit Sub

        If Not ConsisteFecha(Numero.Value) Then
            MsgBox("Fecha Incorrecta.") : Numero.Value = "1800/01/01"
        End If

    End Sub
    Private Sub AgregaRegistro()

        Dim Row As DataRow = Dt.NewRow
        Row("Legajo") = 0
        Row("Nombres") = ""
        Row("Apellidos") = ""
        Row("Calle") = ""
        Row("CalleEntre") = ""
        Row("CalleY") = ""
        Row("Localidad") = ""
        Row("Provincia") = 0
        Row("Pais") = 0
        Row("Telefonos") = ""
        Row("Faxes") = ""
        Row("Dni") = 0
        Row("Cuil") = 0
        Row("Cbu") = ""
        Row("FechaAlta") = DateTime1.Value
        Row("FechaBaja") = "01/01/1800"
        Row("Bruto") = 0
        Row("Bruto2") = 0
        Row("SaldoInicial") = 0
        Row("Banco") = 0
        Row("Cuenta") = 0
        Row("ObraSocial") = 0
        Row("CuentaContable") = 0
        Row("Estado") = 1
        Row("PagoEnPesos") = False
        Row("PagoEnTransferencia") = False
        Row("BancoPago") = 0
        Row("CuentaPago") = 0
        Row("FechaNacimiento") = "01/01/1800"
        Row("Funcion") = ""
        Row("Categoria") = ""

        Dt.Rows.Add(Row)

    End Sub
    Private Function HacerAlta() As Boolean

        Dim NumeroMovi As Double
        Dim Resul As Double

        Dim DtAux As DataTable = Dt.Copy

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            NumeroMovi = UltimaNumeracion(ConexionLegajo)
            If NumeroMovi < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End If
            DtAux.Rows(0).Item("Legajo") = NumeroMovi

            Resul = ActualizaEmpleado(DtAux, ConexionLegajo)

            If Resul >= 0 Then Exit For
            If Resul = -3 Then Exit For
            If Resul = -1 Then Exit For
        Next

        If Resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = -3 Then
            Return False
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PLegajo = NumeroMovi
            GModificacionOk = True
            If Not ArmaArchivos() Then Me.Close()
        End If

    End Function
    Public Function ActualizaEmpleado(ByVal DtAux As DataTable, ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Empleados;", Miconexion)
                    Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(DtAux)
                End Using
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
    Public Function UltimaNumeracion(ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Legajo) FROM Empleados;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CInt(Ultimo) + 1
                    Else
                        If ConexionStr = Conexion Then
                            Return 1
                        Else
                            Return 5000
                        End If
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function Valida() As Boolean

        If TextNombres.Text = "" Then
            MsgBox("Falta Nombres.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNombres.Focus()
            Return False
        End If
        If TextApellidos.Text = "" Then
            MsgBox("Falta Apellidos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextApellidos.Focus()
            Return False
        End If
        If Dt.Rows(0).Item("Dni") = 0 Then
            MsgBox("Falta DNI.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextDni.Focus()
            Return False
        End If
        Dim aa As New DllVarias
        If Not aa.ValidaCuiT(Dt.Rows(0).Item("Cuil")) Then
            MsgBox("Falta CUIL o CUIL Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedCuil.Focus()
            Return False
        End If
        If Dt.Rows(0).Item("Bruto") = 0 Then
            MsgBox("Falta Sueldo Bruto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextBruto.Focus()
            Return False
        End If
        If DiferenciaDias(DateTime1.Value, Date.Now) < 0 And PLegajo = 0 Then
            MsgBox("Fecha Alta No debe ser mayor a Fecha Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If
        If DateTime2.Value <> "01/01/1800" Then
            If DiferenciaDias(DateTime1.Value, DateTime2.Value) < 0 Then
                MsgBox("Fecha Baja No debe ser Menor a Fecha Alta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                DateTime2.Focus()
                Return False
            End If
        End If
        If Dt.Rows(0).Item("Banco") <> 0 And Dt.Rows(0).Item("Cuenta") = 0 Then
            MsgBox("Falta Cuenta Bancaria.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCuenta.Focus()
            Return False
        End If
        If Dt.Rows(0).Item("Banco") = 0 And Dt.Rows(0).Item("Cuenta") <> 0 Then
            MsgBox("Falta Banco.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboBancos.Focus()
            Return False
        End If
        If RadioPagoEnTransferencia.Checked Then
            If ComboBancosPago.SelectedValue = 0 Or TextCuentaPago.Text = "" Then
                MsgBox("Para Medio Pago Transferencia debe completar Banco Y Cuenta.", MsgBoxStyle.Critical)
                Return False
            End If
        End If
        If TextFechaNacimiento.Text <> "" Then
            If DiferenciaDias(CDate(TextFechaNacimiento.Text), Date.Now) < 0 Then
                MsgBox("Fecha Nacimiento Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaNacimiento.Focus()
                Return False
            End If
        End If

        '     If GGeneraAsiento Then
        ' If TextCuentaContable.Text = "" Then
        ' MsgBox("Falta Informar Cuenta Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        ' TextCuentaContable.Focus()
        '  Return False
        '  End If
        ' End If

        Return True

    End Function




   
   
   
End Class