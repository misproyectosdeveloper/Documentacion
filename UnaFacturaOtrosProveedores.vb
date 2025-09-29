Public Class UnaFacturaOtrosProveedores
    Public PRecibo As Integer
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim DtRecibosCabeza As DataTable
    Dim DtRecibosDetalle As DataTable
    Dim DtRecibosLotes As DataTable

    Dim DtTablaConceptos As DataTable
    Dim DtGrid1 As DataTable
    Dim DtGridLotes As DataTable
    Dim DtRetencionProvinciaB As DataTable
    Dim DtRetencionProvinciaAux As DataTable
    Dim ReciboOficialAnt As Decimal
    ' 
    Dim cc As ComboBox
    Dim ConexionRecibo As String
    Dim Mes As Integer
    Dim Anio As Integer
    Dim Proveedor As Integer
    Dim TipoPago As Integer
    Private Sub UnaOtraFactura_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If Not PermisoEscritura(4) Then PBloqueaFunciones = True

        Grid1.AutoGenerateColumns = False
        Grid1.Columns("Lupa").DefaultCellStyle.NullValue = Nothing

        If PRecibo = 0 Then
            Opciones()
            If Proveedor = 0 Then Me.Close() : Exit Sub
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        ComboProveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM OtrosProveedores;")
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"

        ComboTipoPago.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 39;")
        ComboTipoPago.DisplayMember = "Nombre"
        ComboTipoPago.ValueMember = "Clave"

        If PAbierto Then
            ConexionRecibo = Conexion
        Else : ConexionRecibo = ConexionN
        End If

        If PAbierto Then PictureCandado.Image = ImageList1.Images.Item("Abierto")
        If Not PAbierto Then PictureCandado.Image = ImageList1.Images.Item("Cerrado")
        If Not PermisoTotal Then PictureCandado.Visible = False

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid1.EndEdit()
        MiEnlazador.EndEdit()

        'Poner antes de Valida.
        If ReciboOficialAnt <> DtRecibosCabeza.Rows(0).Item("ReciboOficial") Then
            DtRecibosCabeza.Rows(0).Item("ReciboOficial") = CDbl(HallaNumeroLetra(TextLetra.Text) & MaskedReciboOficial.Text)
        End If

        If Not Valida() Then Exit Sub

        Dim DtRecibosCabezaAux As DataTable = DtRecibosCabeza.Copy
        Dim DtRecibosDetalleAux As DataTable = DtRecibosDetalle.Copy
        Dim DtRecibosLotesAux As DataTable = DtRecibosLotes.Copy
        Dim DtRetencionProvinciaW As DataTable = DtRetencionProvinciaB.Copy

        If PRecibo = 0 Then
            If Not ActualizaArchivos(DtRecibosCabezaAux, DtRecibosDetalleAux, DtRetencionProvinciaW) Then Exit Sub
        End If

        'Actualiza Lotes Imputados.
        Dim RowsBusqueda() As DataRow
        For Each Row1 As DataRow In DtGridLotes.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtRecibosLotesAux.Select("Lote = " & Row1("Lote") & " AND Secuencia = " & Row1("Secuencia"))
                If RowsBusqueda.Length = 0 Then
                    Dim Row As DataRow = DtRecibosLotesAux.NewRow()
                    Row("Factura") = PRecibo
                    Row("Operacion") = Row1("Operacion")
                    Row("Lote") = Row1("Lote")
                    Row("Secuencia") = Row1("Secuencia")
                    Row("ImporteConIva") = 0
                    Row("ImporteSinIva") = 0
                    DtRecibosLotesAux.Rows.Add(Row)
                End If
            End If
        Next
        For Each Row1 As DataRow In DtRecibosLotesAux.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtGridLotes.Select("Lote = " & Row1("Lote") & " AND Secuencia = " & Row1("Secuencia"))
                If RowsBusqueda.Length = 0 Then Row1.Delete()
            End If
        Next
        If Not IsNothing(DtRecibosLotesAux.GetChanges) Then
            ProrroteaImportesLotes(DtRecibosLotesAux)
        End If

        If IsNothing(DtRecibosDetalleAux.GetChanges) And IsNothing(DtRecibosCabezaAux.GetChanges) And IsNothing(DtRecibosLotesAux.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        'Arma Tipo Operaciones de Lotes para Asientos.
        If GGeneraAsiento And PRecibo = 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If PRecibo = 0 Then
            If HacerAlta(DtRecibosCabezaAux, DtRecibosDetalleAux, DtAsientoCabeza, DtAsientoDetalle, DtRetencionProvinciaW, DtRecibosLotesAux) Then
                ArmaArchivos()
            End If
        Else
            Dim resul As Integer = ActualizaMovi("M", DtRecibosCabezaAux, DtRecibosDetalleAux, DtAsientoCabeza, DtAsientoDetalle, DtRetencionProvinciaW, DtRecibosLotesAux, ConexionRecibo)
            If resul < 0 And resul <> -3 Then
                MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            End If
            If resul = 0 Then
                MsgBox("Otro usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            End If
            If resul > 0 Then
                MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                ArmaArchivos()
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBaja_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBaja.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PRecibo = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Recibo esta ANULADO. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then
            Exit Sub
        End If

        Dim DtRecibosCabezaAux As DataTable = DtRecibosCabeza.Copy
        Dim DtRecibosDetalleAux As DataTable = DtRecibosDetalle.Copy
        Dim DtRecibosLotesAux As DataTable = DtRecibosLotes.Copy
        Dim DtRetencionProvinciaW As DataTable = DtRetencionProvinciaB.Copy

        If DtRecibosCabeza.Rows(0).Item("Saldo") <> DtRecibosCabeza.Rows(0).Item("Importe") Then
            MsgBox("Recibo tiene Imputaciones, debe anularlas para continuar. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(5000, DtRecibosCabezaAux.Rows(0).Item("Factura"), DtAsientoCabeza, ConexionRecibo) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3
        End If

        If MsgBox("Desea CONTINUAR la Operación de BAJA?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtRecibosCabezaAux.Rows(0).Item("Estado") = 3

        Dim resul As Integer = ActualizaMovi("B", DtRecibosCabezaAux, DtRecibosDetalleAux, DtAsientoCabeza, DtAsientoDetalle, DtRetencionProvinciaW, DtRecibosLotesAux, ConexionRecibo)
        If resul < 0 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If resul = 0 Then
            MsgBox("Otro usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If resul > 0 Then
            MsgBox("Factura Fue Dada de Baja Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonLotesAImputar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonLotesAImputar.Click

        OpcionLotesAImputar.PDtGrid = DtGridLotes
        OpcionLotesAImputar.ShowDialog()
        OpcionLotesAImputar.Dispose()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PRecibo = 0
        UnaOtraFactura_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PRecibo = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 5000
        If PAbierto Then ListaAsientos.PDocumentoB = PRecibo
        If PAbierto Then ListaAsientos.PDocumentoN = PRecibo
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonImportePorLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportePorLotes.Click

        SeleccionarVarios.PFactura = PRecibo
        SeleccionarVarios.PEsImportePorLotesOtrasFacturas = True
        SeleccionarVarios.Show()

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub TextMes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextMes.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextAnio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextAnio.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextComprobante_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComprobante.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid1()
        CreaDtRetencionProvinciaAux()
        CreaDtGridLotes()

        Dim Sql As String

        DtRecibosCabeza = New DataTable
        Sql = "SELECT * FROM OtrasFacturasCabeza WHERE Factura = " & PRecibo & ";"
        If Not Tablas.Read(Sql, ConexionRecibo, DtRecibosCabeza) Then Return False
        If PRecibo <> 0 And DtRecibosCabeza.Rows.Count = 0 Then
            MsgBox("Factura No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PRecibo = 0 Then
            Dim Row As DataRow = DtRecibosCabeza.NewRow
            Row("Proveedor") = Proveedor
            Row("Factura") = 0
            Row("Mes") = 0
            Row("Anio") = 0
            Row("Importe") = 0
            Row("Fecha") = Now
            Row("Caja") = GCaja
            Row("Comentario") = ""
            Row("Estado") = 1
            Row("Saldo") = 0
            Row("Cuenta") = 0
            Row("Comprobante") = ""
            Row("TipoPago") = TipoPago
            Row("ReciboOficial") = 0
            DtRecibosCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtRecibosDetalle = New DataTable
        Sql = "SELECT * FROM OtrasFacturasDetalle WHERE Factura = " & DtRecibosCabeza.Rows(0).Item("Factura") & ";"
        If Not Tablas.Read(Sql, ConexionRecibo, DtRecibosDetalle) Then Return False
        For Each Row As DataRow In DtRecibosDetalle.Rows
            Dim Row1 As DataRow = DtGrid1.NewRow
            Row1("Item") = Row("Item")
            Row1("Factura") = Row("Factura")
            Row1("Concepto") = Row("Concepto")
            Row1("Importe") = Row("Importe")
            DtGrid1.Rows.Add(Row1)
        Next

        Grid1.DataSource = bs
        bs.DataSource = DtGrid1

        DtRetencionProvinciaB = New DataTable
        If Not Tablas.Read("SELECT * FROM RecibosRetenciones WHERE TipoNota = 10000 AND Nota = " & PRecibo & ";", Conexion, DtRetencionProvinciaB) Then Return False
        DtRetencionProvinciaAux.Clear()
        For Each Row As DataRow In DtRetencionProvinciaB.Rows
            Dim Row1 As DataRow = DtRetencionProvinciaAux.NewRow
            Row1("Retencion") = Row("Retencion")
            Row1("Provincia") = Row("Provincia")
            Row1("Comprobante") = Row("Comprobante")
            Row1("Importe") = Row("Importe")
            DtRetencionProvinciaAux.Rows.Add(Row1)
        Next

        DtRecibosLotes = New DataTable
        Sql = "SELECT * FROM OtrasFacturasLotes WHERE Factura = " & PRecibo & ";"
        If Not Tablas.Read(Sql, ConexionRecibo, DtRecibosLotes) Then Return False
        For Each Row1 As DataRow In DtRecibosLotes.Rows
            Dim Row As DataRow = DtGridLotes.NewRow
            Row("Operacion") = Row1("Operacion")
            Row("Lote") = Row1("Lote")
            Row("Secuencia") = Row1("Secuencia")
            Row("Cantidad") = HallaCantidadLote(Row1("Lote"), Row1("Secuencia"), Row1("Operacion"))
            DtGridLotes.Rows.Add(Row)
        Next

        If ComboEstado.SelectedValue = 3 Then
            Grid1.ReadOnly = True
            ButtonEliminarLineaConcepto.Enabled = False
        End If

        If PRecibo = 0 Then
            DateTime1.Enabled = True
            Grid1.ReadOnly = False
            ButtonEliminarLineaConcepto.Enabled = True
        Else : DateTime1.Enabled = False
            Grid1.ReadOnly = True
            ButtonEliminarLineaConcepto.Enabled = False
        End If

        ArmaTablaConceptos()
        LlenaCombosGrid()

        CalculaTotales()

        Grid1.EndEdit()

        AddHandler DtGrid1.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid1_ColumnChanging)
        AddHandler DtGrid1.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid1_NewRow)
        AddHandler DtGrid1.ColumnChanged, New DataColumnChangeEventHandler(AddressOf DtGrid1_ColumnChanged)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtRecibosCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Factura")
        AddHandler Enlace.Format, AddressOf FormatRecibo
        TextRecibo.DataBindings.Clear()
        TextRecibo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comprobante")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextComprobante.DataBindings.Clear()
        TextComprobante.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Mes")
        AddHandler Enlace.Format, AddressOf FormatEntero
        TextMes.DataBindings.Clear()
        TextMes.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Anio")
        AddHandler Enlace.Format, AddressOf FormatAnio
        TextAnio.DataBindings.Clear()
        TextAnio.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Proveedor")
        ComboProveedor.DataBindings.Clear()
        ComboProveedor.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "TipoPago")
        ComboTipoPago.DataBindings.Clear()
        ComboTipoPago.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextImporte.DataBindings.Clear()
        TextImporte.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Saldo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextSaldo.DataBindings.Clear()
        TextSaldo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "ReciboOficial")
        AddHandler Enlace.Format, AddressOf FormatReciboOficial
        MaskedReciboOficial.DataBindings.Clear()
        MaskedReciboOficial.DataBindings.Add(Enlace)
        ReciboOficialAnt = Val(MaskedReciboOficial.Text)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub FormatAnio(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then Numero.Value = Format(Numero.Value, "#")

    End Sub
    Private Sub FormatRecibo(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub ParseCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub ParseTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Strings.Trim(Numero.Value)

    End Sub
    Private Sub FormatReciboOficial(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value <> 0 Then
            TextLetra.Text = LetraTipoIva(Val(Strings.Left(Numero.Value.ToString, 1)))
            Numero.Value = Format(Val(Strings.Right(Numero.Value.ToString, 12)), "000000000000")
        Else
            Numero.Value = Format(Numero.Value, "000000000000")
        End If

    End Sub
    Private Function ActualizaArchivos(ByVal DtRecibosCabezaAux As DataTable, ByRef DtRecibosDetalleAux As DataTable, ByVal DtRetencionProvinciaW As DataTable) As Boolean

        Dim RowsBusqueda() As DataRow

        If PRecibo = 0 Then
            DtRecibosCabezaAux.Rows(0).Item("Proveedor") = ComboProveedor.SelectedValue
            DtRecibosCabezaAux.Rows(0).Item("Saldo") = DtRecibosCabezaAux.Rows(0).Item("Importe")
        End If

        'Actualizo Conceptos.
        For Each Row As DataRow In DtGrid1.Rows
            RowsBusqueda = DtRecibosDetalleAux.Select("Item = " & Row("Item"))
            If RowsBusqueda.Length = 0 Then
                Dim Row1 As DataRow = DtRecibosDetalleAux.NewRow
                Row1("Factura") = DtRecibosCabeza.Rows(0).Item("Factura")
                Row1("Concepto") = Row("Concepto")
                Row1("Importe") = Row("Importe")
                DtRecibosDetalleAux.Rows.Add(Row1)
            End If
        Next

        'Actualiza Archivo de distribucion retenciones por provincia.
        For Each Row As DataRow In DtRetencionProvinciaAux.Rows
            RowsBusqueda = DtRetencionProvinciaW.Select("Retencion = " & Row("Retencion") & " AND Provincia = " & Row("Provincia"))
            If RowsBusqueda.Length <> 0 Then
                If Row("Importe") <> RowsBusqueda(0).Item("Importe") Then RowsBusqueda(0).Item("Importe") = Row("Importe")
            Else
                Dim Row1 As DataRow = DtRetencionProvinciaW.NewRow
                Row1("TipoNota") = 10000
                Row1("Nota") = DtRecibosCabeza.Rows(0).Item("Factura")
                Row1("Provincia") = Row("Provincia")
                Row1("Retencion") = Row("Retencion")
                Row1("Comprobante") = Row("Comprobante")
                Row1("Importe") = Row("Importe")
                DtRetencionProvinciaW.Rows.Add(Row1)
            End If
        Next
        For Each Row As DataRow In DtRetencionProvinciaW.Rows
            RowsBusqueda = DtRetencionProvinciaAux.Select("Retencion = " & Row("Retencion") & " AND Provincia = " & Row("Provincia"))
            If RowsBusqueda.Length = 0 Then Row.Delete()
        Next

        Return True

    End Function
    Private Sub CreaDtGrid1()

        DtGrid1 = New DataTable


        Dim Factura As New DataColumn("Factura")
        Factura.DataType = System.Type.GetType("System.Double")
        DtGrid1.Columns.Add(Factura)

        Dim Item As New DataColumn("Item")
        Item.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Item)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Concepto)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid1.Columns.Add(Importe)

    End Sub
    Private Sub CreaDtRetencionProvinciaAux()

        DtRetencionProvinciaAux = New DataTable

        Dim Retencion As New DataColumn("Retencion")
        Retencion.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Retencion)

        Dim Provincia As New DataColumn("Provincia")
        Provincia.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Provincia)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Comprobante)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtRetencionProvinciaAux.Columns.Add(Importe)

    End Sub
    Private Sub CreaDtGridLotes()

        DtGridLotes = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGridLotes.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGridLotes.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGridLotes.Columns.Add(Secuencia)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Int32")
        DtGridLotes.Columns.Add(Cantidad)

    End Sub
    Private Function HacerAlta(ByVal DtReciboCabezaAux As DataTable, ByVal DtReciboDetalleAux As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtRetencionProvinciaW As DataTable, ByVal DtRecibosLotesAux As DataTable) As Boolean

        Dim NumeroMovi As Double
        Dim Resul As Double
        Dim NumeroAsiento As Double = 0

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            NumeroMovi = UltimaNumeracion(ConexionRecibo)
            If NumeroMovi < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            DtReciboCabezaAux.Rows(0).Item("Factura") = NumeroMovi
            For Each Row As DataRow In DtReciboDetalleAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Factura") = NumeroMovi
                End If
            Next
            For Each Row As DataRow In DtRetencionProvinciaW.Rows
                Row("Nota") = NumeroMovi
            Next
            For Each Row As DataRow In DtRecibosLotesAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Factura") = NumeroMovi
                End If
            Next

            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionRecibo)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = NumeroMovi
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If
            '
            Resul = ActualizaMovi("A", DtReciboCabezaAux, DtReciboDetalleAux, DtAsientoCabeza, DtAsientoDetalle, DtRetencionProvinciaW, DtRecibosLotesAux, ConexionRecibo)

            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = -2 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PRecibo = NumeroMovi
            Return True
        End If

    End Function
    Private Function ActualizaMovi(ByVal Funcion As String, ByVal DtReciboCabezaAux As DataTable, ByVal DtReciboDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal DtRetencionProvinciaBAux As DataTable, ByVal DtRecibosLotesAux As DataTable, ByVal ConexionStr As String) As Double

        Dim Trans As OleDb.OleDbTransaction

        GModificacionOk = False

        Using MiConexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Try
                    If Not IsNothing(DtReciboCabezaAux.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM OtrasFacturasCabeza;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtReciboCabezaAux.GetChanges)
                        End Using
                    End If
                    '
                    If Not IsNothing(DtReciboDetalleAux.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM OtrasFacturasDetalle;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtReciboDetalleAux.GetChanges)
                        End Using
                    End If
                    '
                    If Not IsNothing(DtRetencionProvinciaBAux.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM RecibosRetenciones;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtRetencionProvinciaBAux.GetChanges)
                        End Using
                    End If
                    '
                    If Not IsNothing(DtRecibosLotesAux.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM OtrasFacturasLotes;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtRecibosLotesAux.GetChanges)
                        End Using
                    End If
                    '
                    If Not IsNothing(DtAsientoCabezaAux.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosCabeza;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtAsientoCabezaAux.GetChanges)
                        End Using
                    End If
                    '
                    If Not IsNothing(DtAsientoDetalleAux.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosDetalle;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtAsientoDetalleAux.GetChanges)
                        End Using
                    End If
                    '
                    Trans.Commit()
                    GModificacionOk = True
                    Return 1000
                Catch ex As OleDb.OleDbException
                    Trans.Rollback()
                    If ex.ErrorCode = GAltaExiste Then
                        Return -1
                    Else
                        Return -2
                    End If
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                    Return 0
                Finally
                    Trans = Nothing
                End Try
            Catch ex As Exception
                Return -2
            End Try
        End Using

    End Function
    Public Function UltimaNumeracion(ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Factura) FROM OtrasFacturasCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Sub ArmaTablaConceptos()

        DtTablaConceptos = New DataTable
        DtTablaConceptos = Tablas.Leer("SELECT Clave,Nombre,Operador,0 AS TieneLupa,Tipo FROM Tablas WHERE Tipo = 36 " & " AND TipoPago = " & ComboTipoPago.SelectedValue & " ORDER BY Nombre;")

        If Not Tablas.Read("SELECT Clave,Nombre,1 As Operador,EsPorProvincia AS TieneLupa,Tipo FROM Tablas Where Tipo = 22;", Conexion, DtTablaConceptos) Then End
        If Not Tablas.Read("SELECT Clave,Nombre,1 As Operador,EsPorProvincia AS TieneLupa,Tipo FROM Tablas Where Tipo = 25;", Conexion, DtTablaConceptos) Then End

    End Sub
    Private Sub LlenaCombosGrid()

        Concepto1.DataSource = DtTablaConceptos
        Dim Row As DataRow = Concepto1.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Operador") = 0
        Row("TieneLupa") = 0
        Concepto1.DataSource.Rows.Add(Row)
        Concepto1.DisplayMember = "Nombre"
        Concepto1.ValueMember = "Clave"

    End Sub
    Private Sub ProrroteaImportesLotes(ByVal DtNotaLotesAux As DataTable)

        Dim Cantidad As Decimal = 0

        For Each Row As DataRow In DtGridLotes.Rows
            Cantidad = Cantidad + Row("Cantidad")
        Next

        Dim ImporteConIva As Decimal = CDec(TextImporte.Text)
        Dim ImporteSinIva As Decimal = 0
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid1.Rows
            RowsBusqueda = DtTablaConceptos.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda(0).Item("Tipo") = 36 Then
                ImporteSinIva = ImporteSinIva + Row("Importe")
            End If
        Next

        Dim IndiceCorreccionConIva As Double = ImporteConIva / Cantidad
        Dim IndiceCorreccionSinIva As Double = ImporteSinIva / Cantidad

        For Each Row As DataRow In DtNotaLotesAux.Rows
            If Row.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtGridLotes.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
                Row("ImporteConIva") = Trunca(IndiceCorreccionConIva * RowsBusqueda(0).Item("Cantidad"))
                Row("ImporteSinIva") = Trunca(IndiceCorreccionSinIva * RowsBusqueda(0).Item("Cantidad"))
            End If
        Next

    End Sub
    Private Sub CalculaTotales()

        bs.EndEdit()

        Dim Total As Double = 0
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid1.Rows
            RowsBusqueda = DtTablaConceptos.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda(0).Item("Operador") = 2 Then
                If Row("Importe") > 0 Then
                    Row("Importe") = -1 * Row("Importe")
                End If
            End If
            Total = Total + Row("Importe")
        Next

        TextImporte.Text = FormatNumber(Total, GDecimales)

    End Sub
    Private Sub Opciones()

        OpcionOtrosProveedores.ShowDialog()
        Proveedor = OpcionOtrosProveedores.PProveedor
        TipoPago = OpcionOtrosProveedores.PTipoPago
        PAbierto = OpcionOtrosProveedores.PAbierto
        OpcionOtrosProveedores.Dispose()

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        '
        Dim Item As New ItemListaConceptosAsientos
        Dim RowsBusqueda() As DataRow

        'Arma lista de iva.
        For Each Row As DataRow In DtGrid1.Rows
            RowsBusqueda = DtTablaConceptos.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda(0).Item("Tipo") = 22 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = Row("Concepto")
                Item.Importe = Row("Importe")
                Item.TipoIva = 5
                If Item.Importe <> 0 Then ListaIVA.Add(Item)
            End If
        Next

        'Arma lista de Retencion.
        Item = New ItemListaConceptosAsientos
        For Each Row As DataRow In DtGrid1.Rows
            RowsBusqueda = DtTablaConceptos.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda(0).Item("Tipo") = 25 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = Row("Concepto")
                Item.Importe = Row("Importe")
                Item.TipoIva = 9
                If Item.Importe <> 0 Then ListaRetenciones.Add(Item)
            End If
        Next

        'Arma lista con los conceptos.
        For Each Row As DataRow In DtGrid1.Rows
            RowsBusqueda = DtTablaConceptos.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda(0).Item("Tipo") = 36 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = Row("Concepto")
                Item.Importe = Row("Importe")
                If Item.Importe < 0 Then Item.Importe = -Item.Importe
                If Item.Importe <> 0 Then ListaConceptos.Add(Item)
            End If
        Next

        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = CDbl(TextImporte.Text)
        ListaConceptos.Add(Item)
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = TipoPago
        Item.Importe = CDbl(TextImporte.Text)
        Item.TipoIva = -1      'Para poder usar la lista retenciones y no se mezclen con las retenciones.
        ListaRetenciones.Add(Item)

        If Not Asiento(5000, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

        If DtCabeza.Rows.Count = 0 Then
            '   MsgBox("No se pudo Generar Asiento. Operación se CANCELA.")
            '   Return False
        End If

        Return True

    End Function
    Private Function ConceptoExiste(ByVal Concepto As Integer) As Boolean

        For Each Row As DataRow In DtGrid1.Rows
            If Row("Concepto") = Concepto Then Return True
        Next

        Return False

    End Function
    Private Function Valida() As Boolean

        If ComboProveedor.SelectedValue = 0 Then
            MsgBox("Falta Informar Destinatario.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboProveedor.Focus()
            Return False
        End If

        If TextMes.Text = "" Then
            MsgBox("Falta Informar Mes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextMes.Focus()
            Return False
        End If

        If Not (CInt(TextMes.Text) > 0 And CInt(TextMes.Text) < 13) Then
            MsgBox("Mes Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextMes.Focus()
            Return False
        End If

        If TextAnio.Text = "" Then
            MsgBox("Falta Informar Año.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextAnio.Focus()
            Return False
        End If

        If (Val(TextAnio.Text) > DateTime1.Value.Year) Or ((Val(TextAnio.Text) = DateTime1.Value.Year) And Val(TextMes.Text) > DateTime1.Value.Month) Then
            MsgBox("Fecha Mayor a la actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextMes.Focus()
            Return False
        End If

        If Grid1.Rows.Count = 1 Then
            MsgBox("Falta Informar Conceptos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid1.Focus()
            Return False
        End If

        If CierreContableCerrado(DateTime1.Value.Month, DateTime1.Value.Year) Then
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Function
        End If

        Dim Row As DataGridViewRow
        For i As Integer = 0 To Grid1.Rows.Count - 2
            Row = Grid1.Rows(i)
            If Row.Cells("Concepto1").Value = 0 Then
                MsgBox("Debe Informar Concepto en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.CurrentCell = Grid1.Rows(i).Cells("Concepto1")
                Grid1.BeginEdit(True)
                Return False
            End If
            If Row.Cells("Importe1").Value = 0 Then
                MsgBox("Debe Informar Importe en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.CurrentCell = Grid1.Rows(i).Cells("Importe1")
                Grid1.BeginEdit(True)
                Return False
            End If
        Next

        'Consiste Retencion por Provincia.
        Dim ImporteRetencion As Decimal = 0
        Dim RowsBusqueda() As DataRow
        For Each Row1 As DataRow In DtGrid1.Rows
            RowsBusqueda = DtTablaConceptos.Select("Clave = " & Row1("Concepto"))
            If RowsBusqueda(0).Item("TieneLupa") = 1 Then
                ImporteRetencion = ImporteRetencion + Row1("Importe")
            End If
        Next
        If ImporteRetencion <> 0 Then
            If DtRetencionProvinciaAux.Rows.Count = 0 Then
                MsgBox("Falta Distribuir Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.Focus()
                Return False
            End If
            Dim ImporteDistribuido As Decimal = 0
            For Each Row1 As DataRow In DtRetencionProvinciaAux.Rows
                ImporteDistribuido = ImporteDistribuido + Row1("Importe")
            Next
            If ImporteDistribuido <> ImporteRetencion Then
                MsgBox("Incorrecta Distribución Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.Focus()
                Return False
            End If
        End If
        '--------------Consiste recibo 0ficial------------------------
        If PAbierto Then
            If TextLetra.Text = "" Then
                MsgBox("Falta Informar Letra.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextLetra.Focus()
                Return False
            Else
                If TextLetra.Text <> "A" And TextLetra.Text <> "B" And TextLetra.Text <> "C" And TextLetra.Text <> "M" And TextLetra.Text <> "E" Then
                    MsgBox("Letra Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    TextLetra.Focus()
                    Return False
                Else
                    If Not EsLetraOk(TextLetra.Text) Then
                        MsgBox("Letra no se Corresponde con Categoría IVA.")
                        TextLetra.Focus()
                        Return False
                    End If
                End If
            End If
            If Val(MaskedReciboOficial.Text) = 0 Then
                MsgBox("Falta informar Recibo Oficial.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedReciboOficial.Focus()
                Return False
            End If
            If Not MaskedOK(MaskedReciboOficial) Then
                MsgBox("Numero Recibo Oficial Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedReciboOficial.Focus()
                Return False
            End If
            If ReciboOficialAnt <> DtRecibosCabeza.Rows(0).Item("ReciboOficial") Then
                If ExisteReciboOficial(5000, DtRecibosCabeza.Rows(0).Item("Factura"), DtRecibosCabeza.Rows(0).Item("Proveedor"), DtRecibosCabeza.Rows(0).Item("ReciboOficial"), Conexion) Then
                    MsgBox("Recibo Oficial Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    MaskedReciboOficial.Focus()
                    Return False
                End If
            End If
        End If

        Return True

    End Function
    Private Function EsLetraOk(ByVal Letra As String) As Boolean

        Dim LetraW As String

        LetraW = LetraTipoIva(LetrasPermitidasProveedor(HallaTipoIvaOtroProveedor(DtRecibosCabeza.Rows(0).Item("Proveedor")), 500))

        If Letra = LetraW Then Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID1.                 --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid1_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellEnter

        If Not Grid1.Columns(e.ColumnIndex).ReadOnly Then
            Grid1.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid1_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellLeave

        'para manejo del autocoplete de conceptos.
        If Grid1.Columns(e.ColumnIndex).Name = "Concepto1" Then
            If Not cc Is Nothing Then
                cc.SelectedIndex = cc.FindStringExact(cc.Text)
                If cc.SelectedIndex < 0 Then cc.SelectedValue = 0
            End If
        End If

    End Sub
    Private Sub Grid1_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid1.EditingControlShowing

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cc = e.Control
            cc.DropDownStyle = ComboBoxStyle.DropDown
            cc.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cc.AutoCompleteSource = AutoCompleteSource.ListItems
            Exit Sub
        End If

        Dim columna As Integer = Grid1.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey1_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged1_TextChanged

    End Sub
    Private Sub ValidaKey1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        
        If Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Importe1" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Importe1" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid1_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellEndEdit

        If Grid1.Columns(e.ColumnIndex).Name = "Importe1" Then
            CalculaTotales()
        End If

    End Sub
    Private Sub Grid1_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid1.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        Dim RowsBusqueda() As DataRow

        If Grid1.Columns(e.ColumnIndex).Name = "Importe1" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid1.Columns(e.ColumnIndex).Name = "Lupa" Then
            e.Value = Nothing
            If IsNothing(Grid1.Rows(e.RowIndex).Cells("Concepto1").Value) Then Exit Sub
            RowsBusqueda = DtTablaConceptos.Select("Clave = " & Grid1.Rows(e.RowIndex).Cells("Concepto1").Value)
            If RowsBusqueda(0).Item("TieneLupa") = 1 Then e.Value = ImageList1.Images.Item("Lupa")
        End If

    End Sub
    Private Sub Grid1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        Dim RowsBusqueda() As DataRow

        If Grid1.Columns(e.ColumnIndex).Name = "Lupa" Then
            RowsBusqueda = DtTablaConceptos.Select("Clave = " & Grid1.Rows(e.RowIndex).Cells("Concepto1").Value)
            If RowsBusqueda(0).Item("TieneLupa") = 0 Then Exit Sub
            If Grid1.CurrentRow.Cells("Importe1").Value = 0 Then
                MsgBox("Debe Informar Importe")
                Exit Sub
            End If
            If PRecibo <> 0 Then SeleccionarRetencionesProvincias.PFuncionBloqueada = True
            SeleccionarRetencionesProvincias.PDtGrid = DtRetencionProvinciaAux
            SeleccionarRetencionesProvincias.PImporte = Grid1.CurrentRow.Cells("Importe1").Value
            SeleccionarRetencionesProvincias.PRetencion = Grid1.CurrentRow.Cells("Concepto1").Value
            SeleccionarRetencionesProvincias.ShowDialog()
            SeleccionarRetencionesProvincias.Dispose()
        End If

    End Sub
    Private Sub Grid1_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid1.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid1_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row
        Row("Factura") = 0
        Row("Item") = 0
        Row("Concepto") = 0
        Row("Importe") = 0

    End Sub
    Private Sub DtGrid1_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Importe") Then
            If IsDBNull(e.Row("Importe")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

    End Sub
    Private Sub DtGrid1_ColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If Not e.Column.ColumnName.Equals("Concepto") Then Exit Sub

        Dim RowsBusqueda() As DataRow

        If e.ProposedValue = 0 Then Exit Sub

        RowsBusqueda = DtTablaConceptos.Select("Clave = " & e.ProposedValue)
        If RowsBusqueda(0).Item("Tipo") <> 25 Then Exit Sub

        RowsBusqueda = DtGrid1.Select("Concepto = " & e.ProposedValue)
        If RowsBusqueda.Length > 0 Then
            Dim Row As DataRowView = bs.Current
            MsgBox("Concepto ya Existe", MsgBoxStyle.Information)
            Row.Delete()
        End If

    End Sub
    Private Sub ButtonEliminarLineaConcepto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLineaConcepto.Click

        If Grid1.Rows.Count = 1 Then Exit Sub

        Dim Concepto As Integer

        Dim Row As DataRow
        Row = DtGrid1.Rows.Item(Grid1.CurrentRow.Index)
        Concepto = Row("Concepto")
        Row.Delete()

        For I As Integer = DtRetencionProvinciaAux.Rows.Count - 1 To 0 Step -1
            Row = DtRetencionProvinciaAux.Rows(I)
            If Row("Retencion") = Concepto Then Row.Delete()
        Next

        CalculaTotales()

    End Sub

   
End Class