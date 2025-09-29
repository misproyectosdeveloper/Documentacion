Public Class UnaCesionFactura
    Public PFactura As Decimal
    '
    Private MiEnlazadorFactura As New BindingSource
    Private MiEnlazador As New BindingSource
    '
    Dim DtFactura As DataTable
    Dim DtCesion As DataTable
    '
    Dim TipoDestinoBak As Integer
    Dim EsAlta As Boolean
    Private Sub UnaCesionFactura_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Label2.Text = "Ceder Factura " & NumeroEditado(PFactura)

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtFactura = New DataTable
        If Not Tablas.Read("SELECT Fecha, Importe, Saldo FROM FacturasCabeza WHERE Factura = " & PFactura & ";", Conexion, DtFactura) Then Me.Close() : Exit Sub

        DtCesion = New DataTable
        If Not Tablas.Read("SELECT * FROM CesionFacturas WHERE Factura = " & PFactura & ";", Conexion, DtCesion) Then Me.Close() : Exit Sub
        If DtCesion.Rows.Count = 0 Then
            Dim Row As DataRow = DtCesion.NewRow
            Row("Factura") = PFactura
            Row("Fecha") = Date.Now
            Row("Aforo") = 0
            Row("Interes") = 0
            Row("TipoDestino") = 0
            Row("Destino") = 0
            DtCesion.Rows.Add(Row)
            EsAlta = True
        End If

        EnlazaFactura()
        EnlazaCesion()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub RadioBanco_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles RadioBanco.Validating

        ComboDestino.DataSource = Nothing
        ComboDestino.Text = ""
        LlenaComboTablas(ComboDestino, 26)
        ComboDestino.SelectedValue = 0
        With ComboDestino
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Private Sub RadioCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles RadioCliente.Validating

        ComboDestino.DataSource = Nothing
        ComboDestino.Text = ""
        LlenaCombo(ComboDestino, "", "Clientes")
        ComboDestino.SelectedValue = 0
        With ComboDestino
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Private Sub RadioProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles RadioProveedor.Validating

        ComboDestino.DataSource = Nothing
        ComboDestino.Text = ""
        LlenaCombo(ComboDestino, "", "Proveedores")
        ComboDestino.SelectedValue = 0
        With ComboDestino
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Private Sub ComboDestino_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDestino.Validating

        If IsNothing(ComboDestino.SelectedValue) Then ComboDestino.SelectedValue = 0

    End Sub
    Private Sub TextAforo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextAforo.KeyPress

        EsPorcentaje(e.KeyChar, TextAforo.Text)

    End Sub
    Private Sub TextInteres_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextInteres.KeyPress

        EsPorcentaje(e.KeyChar, TextInteres.Text)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim NUevoTipoDestino As Integer = 0
        If RadioProveedor.Checked Then
            If TipoDestinoBak <> 1 Then NUevoTipoDestino = 1
        Else
            If RadioCliente.Checked Then
                If TipoDestinoBak <> 2 Then NUevoTipoDestino = 2
            Else
                If RadioBanco.Checked Then
                    If TipoDestinoBak <> 3 Then NUevoTipoDestino = 3
                End If
            End If
        End If

        If NUevoTipoDestino <> 0 Then
            If TipoDestinoBak <> NUevoTipoDestino Then
                DtCesion.Rows(0).Item("TipoDestino") = NUevoTipoDestino
            End If
        End If

        If IsNothing(DtCesion.GetChanges) Then MsgBox("No hay Cambios.", MsgBoxStyle.Information) : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Dim Mensaje As String = Tablas.Grabar(DtCesion, "CesionFacturas", Conexion)
        If Mensaje = "" Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Cambios Realizado Exitosamente.", MsgBoxStyle.Information)
            EsAlta = False
            UnaCesionFactura_Load(Nothing, Nothing)
        Else
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox(Mensaje)
        End If

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If EsAlta Then
            MsgBox("La Cesión No fue dada de Alta.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If Not IsNothing(DtCesion.GetChanges) Then
            If MsgBox("Hubo cambio. Desea Borrar de todas maneras?(Y/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
        End If

        If MsgBox("Cesión se Borrara Definitivamente del Sistema. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtCesionAux As DataTable = DtCesion.Copy
        DtCesionAux.Rows(0).Delete()

        Dim Mensaje As String = Tablas.Grabar(DtCesionAux, "CesionFacturas", Conexion)
        If Mensaje = "" Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Cesión Borrada Exitosamente.", MsgBoxStyle.Information)
            Me.Close() : Exit Sub
        Else
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox(Mensaje)
        End If


    End Sub
    Private Sub EnlazaFactura()

        MiEnlazadorFactura = New BindingSource
        MiEnlazadorFactura.DataSource = Dtfactura
        Dim Row As DataRowView = MiEnlazadorFactura.Current

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazadorFactura, "Fecha")
        DateFechaFactura.DataBindings.Clear()
        DateFechaFactura.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazadorFactura, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextImporteFactura.DataBindings.Clear()
        TextImporteFactura.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazadorFactura, "Saldo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextSaldoFactura.DataBindings.Clear()
        TextSaldoFactura.DataBindings.Add(Enlace)

    End Sub
    Private Sub EnlazaCesion()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCesion
        Dim Row As DataRowView = MiEnlazador.Current

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateFechaCesion.DataBindings.Clear()
        DateFechaCesion.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Aforo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextAforo.DataBindings.Clear()
        TextAforo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Interes")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextInteres.DataBindings.Clear()
        TextInteres.DataBindings.Add(Enlace)

        If Row("TipoDestino") = 1 Then RadioProveedor.Checked = True : RadioProveedor_Validating(Nothing, Nothing)
        If Row("TipoDestino") = 2 Then RadioCliente.Checked = True : RadioCliente_Validating(Nothing, Nothing)
        If Row("TipoDestino") = 3 Then RadioBanco.Checked = True : RadioBanco_Validating(Nothing, Nothing)
        TipoDestinoBak = Row("TipoDestino")

        Enlace = New Binding("SelectedValue", MiEnlazador, "Destino")
        ComboDestino.DataBindings.Clear()
        ComboDestino.DataBindings.Add(Enlace)


    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If IsDBNull(Numero.Value) Then Numero.Value = 0
        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Function Valida() As Boolean

        If ComboDestino.SelectedValue = 0 Then
            MsgBox("Fecha Informar Destino de la Cesión.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If DiferenciaDias(DateFechaFactura.Value, DateFechaCesion.Value) < 0 Then
            MsgBox("Fecha Cesión no debe ser menor a Fecha Factura.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TextAforo.Text = "" Then
            MsgBox("Falta Informar Aforo.", MsgBoxStyle.Information)
            Return False
        End If

        If CDec(TextAforo.Text) > 99 Then
            MsgBox("Incorrecto Aforo.", MsgBoxStyle.Information)
            Return False
        End If

        If TextInteres.Text = "" Then
            MsgBox("Falta Informar Interés.", MsgBoxStyle.Information)
            Return False
        End If

        If CDec(TextInteres.Text) > 99 Then
            MsgBox("Incorrecto Interés.", MsgBoxStyle.Information)
            Return False
        End If

        Return True

    End Function

   
End Class