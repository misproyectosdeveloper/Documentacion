Public Class UnComprobanteAsociado
    Public PPtoVtaAsociado As Integer
    Public PCbteAsociado As Decimal
    Public PNroAsociado As Decimal
    Public PCbteTipoAsociado As Integer
    Public PTipoIvaAsociado As Integer
    Public PFechaPeriodoDesde As String
    Public PFechaPeriodoHasta As String
    Public PCbteTipo As Integer
    Public PTipoIva As Integer
    Public PConcepto As Integer
    Public PCancelar As Boolean
    Public PEsConsumidorFinal As Boolean
    Private Sub UnComprobanteAsociado_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        PPtoVtaAsociado = 0
        PCbteAsociado = 0
        PCbteTipoAsociado = 0
        PConcepto = 0
        PCancelar = False
        RadioProducto.Checked = False
        RadioServicio.Checked = False
        RadioProductoServicio.Checked = False
        TextFechaDesde.Text = ""
        TextFechaHasta.Text = ""

        TextLetra.Text = LetraTipoIva(PTipoIva)
        MaskedComprobante.Text = "000000000000"

        ArmaComboTipoFinancieros()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Dim Contador As Integer = 0

        PPtoVtaAsociado = 0
        PCbteAsociado = 0
        PNroAsociado = 0
        PCbteTipoAsociado = 0
        PTipoIvaAsociado = 0
        PConcepto = 0
        PFechaPeriodoDesde = ""
        PFechaPeriodoHasta = ""

        If (ComboTipo.SelectedValue <> 0 Or MaskedComprobante.Text <> "000000000000") And (TextFechaDesde.Text <> "" Or TextFechaHasta.Text <> "") Then
            MsgBox("Debe informar Comprobante Asociado o Periodo Afectado.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If ComboTipo.SelectedValue <> 0 Or MaskedComprobante.Text <> "000000000000" Then    'informo comprobante asociado.
            If ComboTipo.SelectedValue <> 0 Then Contador = Contador + 1
            If MaskedComprobante.Text <> "000000000000" Then Contador = Contador + 1
            If Contador <> 0 And Contador <> 2 Then
                MsgBox("Falta Completar Comprobante Asociado.", MsgBoxStyle.Information)
                Exit Sub
            End If
            '
            If Contador = 2 Then
                If Not ValidaAsociado() Then Exit Sub
                PPtoVtaAsociado = Strings.Left(MaskedComprobante.Text, 4)
                PCbteAsociado = PTipoIva & MaskedComprobante.Text
                PNroAsociado = Strings.Right(MaskedComprobante.Text, 8)
                If ComboTipo.SelectedValue = 2 Then
                    PCbteTipoAsociado = 7
                Else
                    PCbteTipoAsociado = ComboTipo.SelectedValue
                End If
                PTipoIvaAsociado = HallaNumeroLetra(TextLetra.Text)
            End If
        End If

        If TextFechaDesde.Text <> "" Or TextFechaHasta.Text <> "" Then                     'informo periodo.
            If Not ValidaFecha() Then Exit Sub
            PFechaPeriodoDesde = Format(CDate(TextFechaDesde.Text), "yyyyMMdd")
            PFechaPeriodoHasta = Format(CDate(TextFechaHasta.Text), "yyyyMMdd")
        End If

        If Not RadioProducto.Checked And Not RadioServicio.Checked And Not RadioProductoServicio.Checked Then
            MsgBox("Falta Informar Tipo de Conceptos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If MaskedComprobante.Text = "000000000000" And PEsConsumidorFinal Then
            If MsgBox("Para Consumidor Final Debe Informarse Comprobante Asociado. AFIP Rechazará el Envio. Quiere Continuar de Todos Modos? (S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
        End If

        If RadioProducto.Checked Then PConcepto = 1
        If RadioServicio.Checked Then PConcepto = 2
        If RadioProductoServicio.Checked Then PConcepto = 3

        Me.Close()

    End Sub
    Private Sub ButtonCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancelar.Click

        PCancelar = True
        Me.Close()

    End Sub
    Private Sub ComboTipo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipo.Validating

        If IsNothing(ComboTipo.SelectedValue) Then ComboTipo.SelectedValue = 0

    End Sub
    Private Sub PictureAlmanaqueDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueDesde.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaDesde.Text = ""
        Else : TextFechaDesde.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueHasta.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaHasta.Text = ""
        Else : TextFechaHasta.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub ArmaComboTipoFinancieros()

        Dim Row As DataRow

        ComboTipo.DataSource = New DataTable

        Dim Clave As DataColumn = New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        ComboTipo.DataSource.Columns.Add(Clave)

        Dim Nombre As DataColumn = New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        ComboTipo.DataSource.Columns.Add(Nombre)

        Select Case PCbteTipo
            Case 5, 7
                Row = ComboTipo.DataSource.NewRow
                Row("Clave") = 7
                Row("Nombre") = "Notas Crédito a Cliente"
                ComboTipo.DataSource.Rows.Add(Row)
                Row = ComboTipo.DataSource.NewRow
                Row("Clave") = 5
                Row("Nombre") = "Notas Debito a Cliente"
                ComboTipo.DataSource.Rows.Add(Row)
                Row = ComboTipo.DataSource.NewRow
                Row("Clave") = 1
                Row("Nombre") = "Factura"
                ComboTipo.DataSource.Rows.Add(Row)
                Row = ComboTipo.DataSource.NewRow
                Row("Clave") = 2
                Row("Nombre") = "Noda Crédito con Devolución"
                ComboTipo.DataSource.Rows.Add(Row)
            Case 6, 8
                Row = ComboTipo.DataSource.NewRow
                Row("Clave") = 8
                Row("Nombre") = "Notas Crédito a Proveedor"
                ComboTipo.DataSource.Rows.Add(Row)
                Row = ComboTipo.DataSource.NewRow
                Row("Clave") = 6
                Row("Nombre") = "Notas Debito a Proveedor"
                ComboTipo.DataSource.Rows.Add(Row)
        End Select
        Row = ComboTipo.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        ComboTipo.DataSource.Rows.Add(Row)
        Row = ComboTipo.DataSource.NewRow
        ComboTipo.DisplayMember = "Nombre"
        ComboTipo.ValueMember = "Clave"
        ComboTipo.SelectedValue = 0

        With ComboTipo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Private Function HallaNumeroComprobante() As Decimal

        Dim TipoIvaW As Integer = HallaNumeroLetra(TextLetra.Text)
        Return TipoIvaW & MaskedComprobante.Text

    End Function
    Private Function EsFCE() As Boolean

        Dim Dt As New DataTable
        Dim Sql As String

        Select Case ComboTipo.SelectedValue
            Case 1
                Sql = "SELECT EsFCE FROM FacturasCabeza WHERE Factura = " & HallaNumeroComprobante() & ";"
                If Not Tablas.Read(Sql, Conexion, Dt) Then End
            Case 2
                Sql = "SELECT EsFCE FROM NotasCreditoCabeza WHERE NotaCredito = " & HallaNumeroComprobante() & ";"
                If Not Tablas.Read(Sql, Conexion, Dt) Then End
            Case 5, 7
                Sql = "SELECT EsFCE FROM RecibosCabeza WHERE Nota = " & HallaNumeroComprobante() & ";"
                If Not Tablas.Read(Sql, Conexion, Dt) Then End
        End Select

        Dim EsFCEW As Boolean = False

        If Dt.Rows.Count <> 0 Then
            EsFCEW = Dt.Rows(0).Item("EsFCE")
        End If

        Dt.Dispose()
        Return EsFCEW

    End Function
    Private Function ValidaAsociado() As Boolean

        If ComboTipo.SelectedValue = 0 Then
            MsgBox("Falta Tipo Comprobante.", MsgBoxStyle.Information)
            Exit Function
        End If
        If TextLetra.Text = "" Then
            MsgBox("Falta Letra Comprobante.", MsgBoxStyle.Information)
            Exit Function
        End If
        If HallaNumeroLetra(TextLetra.Text) = 0 Then
            MsgBox("Incorrecta Letra Comprobante.", MsgBoxStyle.Information)
            Exit Function
        End If
        If MaskedComprobante.Text = "" Then
            MsgBox("Falta Numero Comprobante.", MsgBoxStyle.Information)
            Exit Function
        End If

        Dim Numero As Decimal = HallaNumeroComprobante()
        Dim Dt As New DataTable

        Select Case ComboTipo.SelectedValue
            Case 5, 7, 6, 8
                If Not Tablas.Read("SELECT Estado FROM RecibosCabeza WHERE TipoNota = " & ComboTipo.SelectedValue & " AND  Nota = " & Numero & ";", Conexion, Dt) Then End
            Case 1
                If Not Tablas.Read("SELECT Estado FROM FacturasCabeza WHERE Factura = " & Numero & ";", Conexion, Dt) Then End
            Case 2
                If Not Tablas.Read("SELECT Estado FROM NotasCreditoCabeza WHERE NotaCredito = " & Numero & ";", Conexion, Dt) Then End
        End Select
        If Dt.Rows.Count = 0 Then
            MsgBox("Comprobante No Encontrado.", MsgBoxStyle.Information)
            Exit Function
        End If
        If Dt.Rows(0).Item("Estado") = 3 Then
            MsgBox("Comprobante Esta Anulado.", MsgBoxStyle.Information)
            Exit Function
        End If

        Dt.Dispose()

        Dim PtoVtaAsociadoW = Strings.Left(MaskedComprobante.Text, 4)

        If Not EsPuntoDeVentaFacturasElectronicas(PtoVtaAsociadoW) Then
            MsgBox("Punto de Venta NO Habilitado para Factura Electrónica.", MsgBoxStyle.Information)
            Exit Function
        End If

        If EsFCE() Then
            MsgBox("Comprobante no debe estar Adherido a Factura de Crédito MiPyMEs(FCE).", MsgBoxStyle.Information)
            Exit Function
        End If

        Return True

    End Function
    Private Function ValidaFecha() As Boolean

        If TextFechaDesde.Text = "" Then
            MsgBox("Fecha Desde Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            TextFechaDesde.Focus()
            Return False
        End If
        If Not ConsisteFecha(TextFechaDesde.Text) Then
            MsgBox("Fecha Desde Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaDesde.Focus()
            Return False
        End If

        If TextFechaHasta.Text = "" Then
            MsgBox("Fecha Hasta Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            TextFechaHasta.Focus()
            Return False
        End If
        If Not ConsisteFecha(TextFechaHasta.Text) Then
            MsgBox("Fecha Hasta de Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaHasta.Focus()
            Return False
        End If

        If DiferenciaDias(CDate(TextFechaDesde.Text), CDate(TextFechaHasta.Text)) < 0 Then
            MsgBox("Fecha Periodo Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    
    
End Class