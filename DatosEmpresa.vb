Imports System.Drawing
Imports System.Drawing.Printing
Public Class DatosEmpresa
    Public PBloqueaFunciones As Boolean
    Private MiEnlazador As New BindingSource
    '
    Dim Dt As DataTable
    Dim TablaIva(0) As Double
    Private Sub DatosEmpresa_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ArmaTipoIva(ComboTipoIva)
        ComboTipoIva.SelectedValue = 0
        With ComboTipoIva
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboProvincia, 31)
        ComboProvincia.SelectedValue = 0
        With ComboProvincia
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ArmaTablaIva(TablaIva)

        ArmaArchivos()

    End Sub
    Private Sub DatosEmpresa_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        GModificacionOk = False

        If Not Valida() Then Exit Sub

        Dim FechaInicioW As String = ""
        If TextFechaInicio.Text <> "" Then
            FechaInicioW = Format(CDate(TextFechaInicio.Text).Month, "00") & "/" & Strings.Right(TextFechaInicio.Text, 4)
        End If

        If Dt.Rows(0).Item("FechaInicio") <> FechaInicioW Then
            Dt.Rows(0).Item("FechaInicio") = FechaInicioW
        End If

        If Dt.Rows(0).Item("Opcion1LibrosIva") <> RadioOpcion1.Checked Then
            Dt.Rows(0).Item("Opcion1LibrosIva") = RadioOpcion1.Checked
        End If

        If IsNothing(Dt.GetChanges) Then MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3) : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Resul As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM DatosEmpresa;", Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(Dt)
                    MsgBox("Cambios Realizados Exitosamente. Debe Reingresar al Sistema para que los Cambios Tengan EFECTO.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Resul = 1000
                    GModificacionOk = True
                End Using
            End Using
        Catch ex As OleDb.OleDbException
            MsgBox("Error Base de Datos. Operación se CANCELA.")
            Resul = -1
        Catch ex As DBConcurrencyException
            MsgBox("Error Otro Usuario Modifico Datos. Operación se CANCELA.")
            Resul = 0
        Finally
        End Try

        If Resul >= 0 Then
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonColorPantallaInicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonColorPantallaInicio.Click

        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            ButtonColorPantallaInicio.BackColor = ColorDialog1.Color
            Entrada.BackColor = ColorDialog1.Color
            Dim C As Color = ColorDialog1.Color
            Dim ColorB As String = Format(C.B, "000")
            Dim ColorG As String = Format(C.G, "000")
            Dim ColorR As String = Format(C.R, "000")
            Dt.Rows(0).Item("ColorPantallaInicial") = ColorR & ColorG & ColorB
        End If

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueContable.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaInicio.Text = ""
        Else : TextFechaInicio.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub ButtonVerClientes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerClientes.Click

        Dim Directorio As String = System.AppDomain.CurrentDomain.BaseDirectory()

        MuestraExcel(Directorio & "ExcelOpcionesLibrosIva.xlsx")

    End Sub
    Private Sub TextIvacomision_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextIvaComision.Validating

        If TextIvaComision.Text = "" Then Exit Sub

        If Not IsNumeric(TextIvaComision.Text) Then
            MsgBox("Iva debe ser Numérico.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            e.Cancel = True
            Exit Sub
        End If
        If CDbl(TextIvaComision.Text) > 100 Then
            MsgBox("Iva Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            e.Cancel = True
            Exit Sub
        End If

    End Sub
    Private Sub Textnumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumero.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, 0)

    End Sub
    Private Sub TextIvacomision_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextIvaComision.KeyPress

        EsNumerico(e.KeyChar, TextIvaComision.Text, GDecimales)

    End Sub
    Private Sub TextIvadescarga_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextIvaDescarga.Validating

        If TextIvaDescarga.Text = "" Then Exit Sub

        If Not IsNumeric(TextIvaDescarga.Text) Then
            MsgBox("Iva debe ser Numérico.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            e.Cancel = True
            Exit Sub
        End If
        If CDbl(TextIvaDescarga.Text) > 100 Then
            MsgBox("Iva Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            e.Cancel = True
            Exit Sub
        End If

    End Sub
    Private Sub TextIvaDescarga_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextIvaDescarga.KeyPress

        EsNumerico(e.KeyChar, TextIvaDescarga.Text, GDecimales)

    End Sub
    Private Sub TextSeniaMaxima_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSeniaMaxima.KeyPress

        EsNumerico(e.KeyChar, TextSeniaMaxima.Text, GDecimales)

    End Sub
    Private Sub TextTolerancia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextTolerancia.KeyPress

        EsNumerico(e.KeyChar, TextTolerancia.Text, GDecimales)

    End Sub
    Private Sub Textcbu_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCBU.KeyPress

        EsNumerico(e.KeyChar, TextCBU.Text, 0)

    End Sub
    Private Sub TextImporteMinimoFCE_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextImporteMinimoFCE.KeyPress

        EsNumerico(e.KeyChar, TextImporteMinimoFCE.Text, GDecimales)

    End Sub
    Private Sub TextLimiteConsumidorFinal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextLimiteParaConsumidorFinal.KeyPress

        EsNumerico(e.KeyChar, TextLimiteParaConsumidorFinal.Text, GDecimales)

    End Sub
    Private Sub ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM DatosEmpresa;", Conexion, Dt) Then Me.Close() : Exit Sub
        If Dt.Rows.Count = 0 Then
            Dim Row As DataRow = Dt.NewRow
            Row("Indice") = 1
            Row("Nombre") = ""
            Row("Calle") = ""
            Row("Numero") = 0
            Row("Localidad") = ""
            Row("Provincia") = 0
            Row("CodigoPostal") = ""
            Row("TipoIva") = 0
            Row("Cuit") = 0
            Row("Telefonos") = ""
            Row("Faxes") = ""
            Row("IvaComision") = 0
            Row("IvaDescarga") = 0
            Row("ColorPantallaInicial") = "225225225"
            Row("SeniaMaxima") = 0
            Row("Direccion1") = ""
            Row("Direccion2") = ""
            Row("Direccion3") = ""
            Row("IngBruto") = ""
            Row("FechaInicio") = ""
            Row("Tolerancia") = 0
            Row("Tolerancia") = 0
            Row("Opcion1LibrosIva") = True
            Row("CBU") = ""
            Row("ImporteMinimoFCE") = 0
            Row("LimiteParaConsumidorFinal") = 0
            Dt.Rows.Add(Row)
        End If

        MuestraCabeza()

        If Dt.Rows(0).Item("ColorPantallaInicial") <> "" Then
            Dim ColorR As Integer = Mid(Dt.Rows(0).Item("ColorPantallaInicial"), 1, 3)
            Dim ColorG As Integer = Mid(Dt.Rows(0).Item("ColorPantallaInicial"), 4, 3)
            Dim ColorB As Integer = Mid(Dt.Rows(0).Item("ColorPantallaInicial"), 7, 3)
            ButtonColorPantallaInicio.BackColor = Color.FromArgb(ColorR, ColorG, ColorB)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = Dt

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Calle")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextCalle.DataBindings.Clear()
        TextCalle.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Numero")
        AddHandler Enlace.Format, AddressOf FormatNumerico
        AddHandler Enlace.Parse, AddressOf ParseIva
        TextNumero.DataBindings.Clear()
        TextNumero.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Localidad")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextLocalidad.DataBindings.Clear()
        TextLocalidad.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CodigoPostal")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextCodigoPostal.DataBindings.Clear()
        TextCodigoPostal.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Provincia")
        ComboProvincia.DataBindings.Clear()
        ComboProvincia.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Telefonos")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextTelefonos.DataBindings.Clear()
        TextTelefonos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Faxes")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextFaxes.DataBindings.Clear()
        TextFaxes.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "TipoIva")
        ComboTipoIva.DataBindings.Clear()
        ComboTipoIva.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "IvaComision")
        AddHandler Enlace.Format, AddressOf FormatDecimales
        AddHandler Enlace.Parse, AddressOf ParseIva
        TextIvaComision.DataBindings.Clear()
        TextIvaComision.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "IvaDescarga")
        AddHandler Enlace.Format, AddressOf FormatDecimales
        AddHandler Enlace.Parse, AddressOf ParseIva
        TextIvaDescarga.DataBindings.Clear()
        TextIvaDescarga.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "SeniaMaxima")
        AddHandler Enlace.Format, AddressOf FormatDecimales
        AddHandler Enlace.Parse, AddressOf ParseIva
        TextSeniaMaxima.DataBindings.Clear()
        TextSeniaMaxima.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Tolerancia")
        AddHandler Enlace.Format, AddressOf FormatDecimales
        AddHandler Enlace.Parse, AddressOf ParseIva
        TextTolerancia.DataBindings.Clear()
        TextTolerancia.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CBU")
        TextCBU.DataBindings.Clear()
        TextCBU.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "ImporteMinimoFCE")
        AddHandler Enlace.Format, AddressOf FormatDecimales
        AddHandler Enlace.Parse, AddressOf ParseIva
        TextImporteMinimoFCE.DataBindings.Clear()
        TextImporteMinimoFCE.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Direccion1")
        TextDireccion1.DataBindings.Clear()
        TextDireccion1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Direccion2")
        TextDireccion2.DataBindings.Clear()
        TextDireccion2.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Direccion3")
        TextDireccion3.DataBindings.Clear()
        TextDireccion3.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "IngBruto")
        TextIngresoBruto.DataBindings.Clear()
        TextIngresoBruto.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "LimiteParaConsumidorFinal")
        AddHandler Enlace.Format, AddressOf FormatDecimales
        AddHandler Enlace.Parse, AddressOf ParseIva
        TextLimiteParaConsumidorFinal.DataBindings.Clear()
        TextLimiteParaConsumidorFinal.DataBindings.Add(Enlace)

        If Dt.Rows(0).Item("FechaInicio") <> "" Then
            TextFechaInicio.Text = "01/" & Strings.Left(Dt.Rows(0).Item("FechaInicio"), 2) & "/" & Strings.Right(Dt.Rows(0).Item("FechaInicio"), 4)
        Else
            TextFechaInicio.Text = ""
        End If

        If Dt.Rows(0).Item("Opcion1LibrosIva") Then
            RadioOpcion1.Checked = True
            RadioOpcion2.Checked = False
        Else
            RadioOpcion1.Checked = False
            RadioOpcion2.Checked = True
        End If

        TextNombre.Text = GNombreEmpresa
        TextCuit.Text = GCuitEmpresa

    End Sub
    Private Sub ParseTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Sub FormatCuit(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "00000000000")

    End Sub
    Private Sub FormatDecimales(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub FormatNumerico(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then Numero.Value = ""

    End Sub
    Private Sub ParseIva(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Function Valida() As Boolean

        If TextNombre.Text = "" Then
            MsgBox("Falta Razon Social.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNombre.Focus()
            Return False
        End If
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
        If ComboTipoIva.SelectedValue = 0 Then
            MsgBox("Falta Tipo IVA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboTipoIva.Focus()
            Return False
        End If

        Dim Esta As Boolean

        For Each Item As Double In TablaIva
            If Item = Dt.Rows(0).Item("IvaComision") Then Esta = True : Exit For
        Next
        If Esta = False Then
            MsgBox("Alicuota Comisión no Existe en el Sistema.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        For Each Item As Double In TablaIva
            If Item = Dt.Rows(0).Item("IvaDescarga") Then Esta = True : Exit For
        Next
        If Esta = False Then
            MsgBox("Alicuota Descarga no Existe en el Sistema.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TextFechaInicio.Text <> "" Then
            If Not ConsisteFecha(TextFechaInicio.Text) Then
                MsgBox("Fecha Inicio Actividad Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaInicio.Focus()
                Return False
            End If
        End If

        If TextCBU.Text <> "" Then
            If TextCBU.Text.Length <> 22 Then
                MsgBox("C.B.U. debe contener 22 digitos.", MsgBoxStyle.Critical) : Exit Function
            End If
        End If

        Return True

    End Function
    Private Sub ButtonMascara_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMascara.Click

        Dim print_document As New PrintDocument
        AddHandler print_document.PrintPage, AddressOf Print_PrintPage
        print_document.Print()

    End Sub
    Private Sub Print_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim NombreEmpres As String = GNombreEmpresa '"Patagonia Sunrise S.R.L."
        Dim CuitEmpresa As String = GCuitEmpresa    '"30-12345678-5"
        Dim TipoComprobante As Integer = 1              '1. factura 2. debito 3. credito
        Dim Direccion1 As String = TextDireccion1.Text     '"Nave S-2 Puesto 34 - Mercado Central de Buenos Aires"
        Dim Direccion2 As String = TextDireccion2.Text       '"Autopista Ricchieri y Boulongne Sur Mer"
        Dim Direccion3 As String = TextDireccion3.Text       '"C.P.(1771) Villa Celina Pcia. de Buenos Aires"
        Dim LetraComprobante As String = "A"
        Dim CondicionIva As String = ComboTipoIva.Text
        Dim IngBruto As String = TextIngresoBruto.Text     '"Conv.Mult. Nª 901-898989-6"
        Dim FechaInicio As String = TextFechaInicio.Text
        '
        MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, LetraComprobante, CondicionIva, IngBruto, FechaInicio, "C:\XML Afip\", e, False, 1)

        e.HasMorePages = False

    End Sub



End Class