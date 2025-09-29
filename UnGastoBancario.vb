Imports System.Transactions
Imports System.Math
Public Class UnGastoBancario
    ' Public PBanco As Integer
    Public PMovimiento As Integer
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim DtGastosCabeza As DataTable
    Dim DtGastosDetalle As DataTable
    Dim DtGrid1 As DataTable
    Dim DtConceptos As DataTable
    Dim DtRetencionProvincia As DataTable
    Dim DtRetencionProvinciaAux As DataTable
    '
    Dim ReciboOficialAnt As Decimal
    Dim ConexionMovimiento As String
    Dim Moneda As Integer
    Dim UltimaFechaContableW As DateTime
    Dim cc As ComboBox
    Private Sub UnGastoBancario_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(810) Then PBloqueaFunciones = True

        Me.Top = 50

        Grid1.AutoGenerateColumns = False
        Grid1.Columns("Lupa").DefaultCellStyle.NullValue = Nothing

        LlenaComboTablas(ComboBancos, 26)

        If PMovimiento = 0 Then
            ListaBancos.PEsSeleccionaCuenta = True
            ListaBancos.ShowDialog()
            ComboBancos.SelectedValue = ListaBancos.PBanco
            If EsBancoNegro(ListaBancos.PBanco) Then
                PAbierto = False
            Else : PAbierto = True
            End If
            TextCuenta.Text = FormatNumber(ListaBancos.PCuenta, 0)
            Moneda = ListaBancos.PMonedaCta
            ListaBancos.Dispose()
        End If
        If ComboBancos.SelectedValue = 0 Then Me.Close() : Exit Sub

        DtConceptos = ArmaConceptosParaGastosBancarios(PMovimiento, PAbierto)

        LlenaCombosGrid()

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        ComboMoneda.DataSource = Nothing
        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Dim Row As DataRow = ComboMoneda.DataSource.newrow
        Row("Nombre") = "PESOS"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionMovimiento = Conexion
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionMovimiento = ConexionN
        End If

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        GModificacionOk = False

        UltimaFechaContableW = UltimaFechacontableGasto(ConexionMovimiento)
        If UltimaFechaContableW = "2/1/1000" Then
            MsgBox("Error Base de Datos al Leer Ultima Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid1.EndEdit()
        MiEnlazador.EndEdit()
        bs.EndEdit()

        'Poner antes de Valida.
        If ReciboOficialAnt <> DtGastosCabeza.Rows(0).Item("ReciboOficial") Then
            DtGastosCabeza.Rows(0).Item("ReciboOficial") = CDbl(HallaNumeroLetra(TextLetra.Text) & MaskedReciboOficial.Text)
        End If

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PMovimiento = 0 Then
            If HacerAlta() Then ArmaArchivos()
        Else
            If HacerModificacion() Then ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBaja_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBaja.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PMovimiento = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Gasto esta ANULADO. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If CierreContableCerrado(CDate(TextFechaContable.Text).Month, CDate(TextFechaContable.Text).Year) Then
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtGastosCabezaAux As DataTable = DtGastosCabeza.Copy
        Dim DtGastosDetalleAux As DataTable = DtGastosDetalle.Copy

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If Not HallaAsientosCabeza(7005, PMovimiento, DtAsientoCabeza, ConexionMovimiento) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3

        If MsgBox("Movimiento se Dara de Baja. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtGastosCabezaAux.Rows(0).Item("Estado") = 3

        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone

        Dim resul As Integer = ActualizaMovi("B", DtGastosCabezaAux, DtGastosDetalleAux, DtAsientoCabeza, DtAsientoDetalle, DtRetencionProvinciaWW, ConexionMovimiento)
        If resul < 0 And resul <> -3 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If resul = 0 Then
            MsgBox("Otro usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If resul > 0 Then
            MsgBox("Movimiento Fue Dado de Baja Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PMovimiento = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 7005
        If PAbierto Then
            ListaAsientos.PDocumentoB = PMovimiento
        Else
            ListaAsientos.PDocumentoN = PMovimiento
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PMovimiento = 0
        UnGastoBancario_Load(Nothing, Nothing)

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueContable.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

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
    Private Sub TextCambio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCambio.KeyPress

        EsNumerico(e.KeyChar, TextCambio.Text, 3)

    End Sub
    Private Sub ButtonEliminarLineaConcepto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLineaConcepto.Click

        If Grid1.Rows.Count = 1 Then Exit Sub

        Dim Row As DataRow
        Row = DtGrid1.Rows.Item(Grid1.CurrentRow.Index)
        Dim Concepto As Integer = Row("Concepto")
        Row.Delete()

        For I As Integer = DtRetencionProvinciaAux.Rows.Count - 1 To 0 Step -1
            Row = DtRetencionProvinciaAux.Rows(I)
            If Row("Retencion") = Concepto Then Row.Delete()
        Next

        CalculaTotales()

    End Sub
    Private Function ArmaArchivos() As Boolean       'ArmaArchivos

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid1()
        CreaDtRetencionProvinciaAux()

        Dim Sql As String

        DtGastosCabeza = New DataTable
        Sql = "SELECT * FROM GastosBancarioCabeza WHERE Movimiento = " & PMovimiento & ";"
        If Not Tablas.Read(Sql, ConexionMovimiento, DtGastosCabeza) Then Return False
        If PMovimiento <> 0 And DtGastosCabeza.Rows.Count = 0 Then
            MsgBox("Informe Gastos No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PMovimiento = 0 Then
            Dim Row As DataRow = DtGastosCabeza.NewRow
            Row("Banco") = ComboBancos.SelectedValue
            Row("Movimiento") = 0
            Row("Cuenta") = CDbl(TextCuenta.Text)
            Row("Importe") = 0
            Row("Fecha") = Now
            Row("FechaContable") = "01/01/1800"
            Row("FechaDesde") = "01/01/1800"
            Row("FechaHasta") = "01/01/1800"
            Row("Caja") = GCaja
            Row("Comentario") = ""
            Row("Estado") = 1
            Row("Moneda") = Moneda
            If Moneda = 1 Then
                Row("Cambio") = 1
            Else
                Row("Cambio") = 0
            End If
            Row("ReciboOficial") = 0
            DtGastosCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtRetencionProvincia = New DataTable
        If Not Tablas.Read("SELECT * FROM RecibosRetenciones WHERE TipoNota = " & 3000 & " AND Nota = " & PMovimiento & ";", ConexionMovimiento, DtRetencionProvincia) Then Return False
        For Each Row As DataRow In DtRetencionProvincia.Rows
            Dim Row1 As DataRow = DtRetencionProvinciaAux.NewRow
            Row1("Retencion") = Row("Retencion")
            Row1("Provincia") = Row("Provincia")
            Row1("Comprobante") = Row("Comprobante")
            Row1("Importe") = Row("Importe")
            DtRetencionProvinciaAux.Rows.Add(Row1)
        Next

        DtGastosDetalle = New DataTable
        Sql = "SELECT * FROM GastosBancarioDetalle WHERE Movimiento = " & DtGastosCabeza.Rows(0).Item("Movimiento") & ";"
        If Not Tablas.Read(Sql, ConexionMovimiento, DtGastosDetalle) Then Return False

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGastosDetalle.Rows
            Dim Row1 As DataRow = DtGrid1.NewRow
            Row1("Item") = Row("Item")
            Row1("Movimiento") = Row("Movimiento")
            Row1("Concepto") = Row("Concepto")
            Row1("Importe") = Row("Importe")
            Row1("ImportePantalla") = Row("Importe")
            Row1("TieneLupa") = False
            RowsBusqueda = DtRetencionProvincia.Select("Retencion = " & Row("Concepto"))
            If RowsBusqueda.Length <> 0 Then Row1("TieneLupa") = True
            DtGrid1.Rows.Add(Row1)
        Next

        Grid1.DataSource = bs
        bs.DataSource = DtGrid1
        Grid1.EndEdit()

        If ComboMoneda.SelectedValue = 1 Then TextCambio.Enabled = False

        If ComboEstado.SelectedValue = 3 Then
            Grid1.ReadOnly = True
            ButtonEliminarLineaConcepto.Enabled = False
            TextCambio.Enabled = False
        End If

        CalculaTotales()

        AddHandler DtGrid1.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid1_ColumnChanging)
        AddHandler DtGrid1.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid1_NewRow)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtGastosCabeza

        Dim Enlace As Binding

        Enlace = New Binding("SelectedValue", MiEnlazador, "Banco")
        ComboBancos.DataBindings.Clear()
        ComboBancos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Movimiento")
        AddHandler Enlace.Format, AddressOf FormatEntero
        TextMovimiento.DataBindings.Clear()
        TextMovimiento.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cuenta")
        AddHandler Enlace.Format, AddressOf FormatEntero
        TextCuenta.DataBindings.Clear()
        TextCuenta.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextImporte.DataBindings.Clear()
        TextImporte.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Moneda")
        ComboMoneda.DataBindings.Clear()
        ComboMoneda.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cambio")
        AddHandler Enlace.Format, AddressOf FormatCambio
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextCambio.DataBindings.Clear()
        TextCambio.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "ReciboOficial")
        AddHandler Enlace.Format, AddressOf FormatReciboOficial
        MaskedReciboOficial.DataBindings.Clear()
        MaskedReciboOficial.DataBindings.Add(Enlace)
        ReciboOficialAnt = Val(MaskedReciboOficial.Text)

        Dim Row As DataRowView = MiEnlazador.Current

        If Row("FechaContable") = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Row("FechaContable"), "dd/MM/yyyy")
        End If

        If Row("FechaDesde") = "01/01/1800" Then
            TextFechaDesde.Text = ""
        Else : TextFechaDesde.Text = Format(Row("FechaDesde"), "dd/MM/yyyy")
        End If

        If Row("FechaHasta") = "01/01/1800" Then
            TextFechaHasta.Text = ""
        Else : TextFechaHasta.Text = Format(Row("FechaHasta"), "dd/MM/yyyy")
        End If

    End Sub
    Private Sub FormatReciboOficial(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value <> 0 Then
            TextLetra.Text = LetraTipoIva(Val(Strings.Left(Numero.Value.ToString, 1)))
            Numero.Value = Format(Val(Strings.Right(Numero.Value.ToString, 12)), "000000000000")
        Else
            Numero.Value = Format(Numero.Value, "000000000000")
        End If

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
    Private Sub FormatCuenta(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub ParseCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatCambio(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, 3)

    End Sub
    Private Function HacerAlta() As Boolean

        Dim DtGastosCabezaAux As DataTable = DtGastosCabeza.Copy
        Dim DtGastosDetalleAux As DataTable = DtGastosDetalle.Copy

        ActualizaArchivos("A", DtGastosCabezaAux, DtGastosDetalleAux)

        If IsNothing(DtGastosDetalleAux.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", ConexionMovimiento, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Function
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", ConexionMovimiento, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Function
            If Not ArmaArchivosAsiento("A", DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Function
        End If

        'Actualiza Archivo de distribucion retenciones por provincia.
        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone
        ArmaRetenciones(DtRetencionProvinciaWW, PMovimiento)

        If GrabarAlta(DtGastosCabezaAux, DtGastosDetalleAux, DtAsientoCabeza, DtAsientoDetalle, DtRetencionProvinciaWW) Then
            Return True
        Else
            Exit Function
        End If

    End Function
    Private Function HacerModificacion() As Boolean

        Dim DtGastosCabezaAux As DataTable = DtGastosCabeza.Copy
        Dim DtGastosDetalleAux As DataTable = DtGastosDetalle.Copy

        ActualizaArchivos("M", DtGastosCabezaAux, DtGastosDetalleAux)

        If IsNothing(DtGastosDetalleAux.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        'Re-Graba Asientos modificado.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If Not LeerCabezaYDetalleAsiento(7005, PMovimiento, DtAsientoCabeza, DtAsientoDetalle, ConexionMovimiento) Then Return False
        If DtAsientoCabeza.Rows.Count <> 0 Then
            Dim DtAsientoCabezaAux As DataTable = DtAsientoCabeza.Clone
            Dim DtAsientoDetalleAux As DataTable = DtAsientoDetalle.Clone
            If Not ArmaArchivosAsiento("A", DtAsientoCabezaAux, DtAsientoDetalleAux) Then Exit Function
            For Each Row As DataRow In DtAsientoDetalle.Rows
                Row.Delete()
            Next
            For Each Row As DataRow In DtAsientoDetalleAux.Rows
                Row("Asiento") = DtAsientoCabeza.Rows(0).Item("Asiento")
                Dim Row1 As DataRow = DtAsientoDetalle.NewRow
                For I As Integer = 0 To DtAsientoDetalleAux.Columns.Count - 1
                    Row1.Item(I) = Row.Item(I)
                Next
                DtAsientoDetalle.Rows.Add(Row1)
            Next
        End If

        'Actualiza Archivo de distribucion retenciones por provincia.
        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone
        DtRetencionProvinciaWW = DtRetencionProvincia.Copy
        For Each Row1 As DataRow In DtRetencionProvinciaWW.Rows
            Row1.Delete()
        Next
        ArmaRetenciones(DtRetencionProvinciaWW, PMovimiento)

        Dim Resul As Integer

        For i As Integer = 1 To 50
            Resul = ActualizaMovi("M", DtGastosCabezaAux, DtGastosDetalleAux, DtAsientoCabeza, DtAsientoDetalle, DtRetencionProvinciaWW, ConexionMovimiento)
            If Resul >= 0 Then Exit For
            If Resul = -3 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
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
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Sub ActualizaArchivos(ByVal Funcion As String, ByRef DtGastosCabezaAux As DataTable, ByRef DtGastosDetalleAux As DataTable)

        If Format(DtGastosCabezaAux.Rows(0).Item("FechaContable"), "dd/MM/yyyy") <> CDate(TextFechaContable.Text) Then DtGastosCabezaAux.Rows(0).Item("FechaContable") = CDate(TextFechaContable.Text)
        If Format(DtGastosCabezaAux.Rows(0).Item("FechaDesde"), "dd/MM/yyyy") <> CDate(TextFechaDesde.Text) Then DtGastosCabezaAux.Rows(0).Item("FechaDesde") = CDate(TextFechaDesde.Text)
        If Format(DtGastosCabezaAux.Rows(0).Item("FechaHasta"), "dd/MM/yyyy") <> CDate(TextFechaHasta.Text) Then DtGastosCabezaAux.Rows(0).Item("FechaHasta") = CDate(TextFechaHasta.Text)

        If Funcion = "M" Then
            DtGastosDetalleAux = DtGastosDetalle.Copy
            For Each Row As DataRow In DtGastosDetalleAux.Rows
                Row.Delete()
            Next
        End If

        'Actualizo Conceptos.
        For Each Row As DataRow In DtGrid1.Rows
            Dim Row1 As DataRow = DtGastosDetalleAux.NewRow
            Row1("Movimiento") = DtGastosCabeza.Rows(0).Item("Movimiento")
            Row1("Concepto") = Row("Concepto")
            Row1("Importe") = Row("Importe")
            DtGastosDetalleAux.Rows.Add(Row1)
        Next

    End Sub
    Private Sub ArmaRetenciones(ByVal DtReten As DataTable, ByVal Nota As Decimal)

        For Each Row As DataRow In DtRetencionProvinciaAux.Rows
            Dim Row1 As DataRow = DtReten.NewRow
            Row1("TipoNota") = 3000
            Row1("Nota") = Nota
            Row1("Provincia") = Row("Provincia")
            Row1("Retencion") = Row("Retencion")
            Row1("Comprobante") = Row("Comprobante")
            Row1("Importe") = Row("Importe")
            If CDec(TextCambio.Text) <> 1 Then
                Row1("Importe") = CalculaNeto(Row1("Importe"), CDec(TextCambio.Text))
            End If
            DtReten.Rows.Add(Row1)
        Next

    End Sub
    Private Sub CreaDtGrid1()

        DtGrid1 = New DataTable

        Dim Movimiento As New DataColumn("Movimiento")
        Movimiento.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Movimiento)

        Dim Item As New DataColumn("Item")
        Item.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Item)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Concepto)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGrid1.Columns.Add(Importe)

        Dim ImportePantalla As New DataColumn("ImportePantalla")
        ImportePantalla.DataType = System.Type.GetType("System.Decimal")
        DtGrid1.Columns.Add(ImportePantalla)

        Dim TieneLupa As New DataColumn("TieneLupa")
        TieneLupa.DataType = System.Type.GetType("System.Boolean")
        DtGrid1.Columns.Add(TieneLupa)

    End Sub
    Private Function GrabarAlta(ByVal DtGastosCabezaAux As DataTable, ByVal DtGastosDetalleAux As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtRetencionProvinciaWW As DataTable) As Boolean

        Dim NumeroMovi As Double
        Dim Resul As Double
        Dim NumeroAsiento As Double = 0

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            NumeroMovi = UltimaNumeracion(ConexionMovimiento)
            If NumeroMovi < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            DtGastosCabezaAux.Rows(0).Item("Movimiento") = NumeroMovi
            For Each Row As DataRow In DtGastosDetalleAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Movimiento") = NumeroMovi
                End If
            Next

            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionMovimiento)
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

            For Each Row As DataRow In DtRetencionProvinciaWW.Rows
                Row("Nota") = NumeroMovi
            Next

            Resul = ActualizaMovi("A", DtGastosCabezaAux, DtGastosDetalleAux, DtAsientoCabeza, DtAsientoDetalle, DtRetencionProvinciaWW, ConexionMovimiento)

            If Resul >= 0 Then Exit For
            If Resul = -3 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
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
            PMovimiento = NumeroMovi
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function ActualizaMovi(ByVal Funcion As String, ByVal DtGastosCabezaAux As DataTable, ByVal DtGastosDetalleAux As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtRetencionProvinciaWW As DataTable, ByVal ConexionStr As String) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtGastosCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtGastosCabezaAux.GetChanges, "GastosBancarioCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtGastosDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtGastosDetalleAux.GetChanges, "GastosBancarioDetalle", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If
                ' 
                ' graba Asiento.
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtRetencionProvinciaWW.GetChanges) Then
                    Resul = GrabaTabla(DtRetencionProvinciaWW.GetChanges, "RecibosRetenciones", ConexionStr)
                    If Resul <= 0 Then Return 0
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Public Function UltimaNumeracion(ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Movimiento) FROM GastosBancarioCabeza;", Miconexion)
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
    Private Sub LlenaCombosGrid()

        Concepto1.DataSource = DtConceptos
        Concepto1.DisplayMember = "Nombre"
        Concepto1.ValueMember = "Clave"

    End Sub
    Private Sub CalculaTotales()

        Dim Total As Double = 0
        Dim Grabado As Decimal = 0
        Dim BaseImponible As Decimal = 0
        Dim TotalIva As Decimal = 0
        Dim RowsBusqueda() As DataRow

        bs.EndEdit()

        For Each Row As DataRow In DtGrid1.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row("Concepto") <> 0 Then
                    If Operador(Row("Concepto")) = 2 Then
                        If Row("ImportePantalla") > 0 Then  'Si esta ya en negativo no lo haga positivo por multiplicar por -1.
                            Row("ImportePantalla") = -1 * Row("ImportePantalla")
                        End If
                    End If
                    Total = Total + Row("ImportePantalla")
                End If
            End If
        Next

        'poner despues de lo anterior.
        For Each Row As DataRow In DtGrid1.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row("Concepto") <> 0 Then
                    RowsBusqueda = DtConceptos.Select("Clave = " & Row("Concepto"))
                    If RowsBusqueda(0).Item("Activo2") And Row("ImportePantalla") < 0 Then
                        Grabado = Grabado + Row("Importe")
                    Else
                        If RowsBusqueda(0).Item("Tipo") = 22 Then
                            BaseImponible = BaseImponible + Trunca(Row("Importe") * 100 / RowsBusqueda(0).Item("Alicuota"))
                            TotalIva = TotalIva + Row("Importe")
                        End If
                    End If
                End If
            End If
        Next

        TextImporte.Text = FormatNumber(Total, GDecimales)
        TextGrabado.Text = FormatNumber(Grabado, GDecimales)
        TextBaseImponible.Text = FormatNumber(BaseImponible, GDecimales)
        TextTotalIva.Text = FormatNumber(TotalIva, GDecimales)

    End Sub
    Private Function Operador(ByVal Concepto As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtConceptos.Select("Clave = " & Concepto)
        Return RowsBusqueda(0).Item("Operador")

    End Function
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos
        Dim RowsBusqueda() As DataRow

        If Funcion = "A" Then
            Dim Total As Decimal = 0
            For Each Row As DataRow In DtGrid1.Rows
                Total = Total + CalculaNeto(Row("ImportePantalla"), CDbl(TextCambio.Text))
            Next
            'Arma lista de Conceptos.
            For Each Row As DataRow In DtGrid1.Rows
                RowsBusqueda = DtConceptos.Select("Clave = " & Row("Concepto"))
                If RowsBusqueda(0).Item("Tipo") = 33 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("Concepto")
                    Item.Importe = CalculaNeto(Row("Importe"), CDbl(TextCambio.Text))
                    ListaConceptos.Add(Item)
                End If
            Next
            'Arma lista de Iva.
            For Each Row As DataRow In DtGrid1.Rows
                RowsBusqueda = DtConceptos.Select("Clave = " & Row("Concepto"))
                If RowsBusqueda(0).Item("Tipo") = 22 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("Concepto")
                    Item.Importe = CalculaNeto(Row("Importe"), CDbl(TextCambio.Text))
                    Item.TipoIva = 5
                    ListaIVA.Add(Item)
                End If
            Next
            'Arma lista de Retencion.
            For Each Row As DataRow In DtGrid1.Rows
                RowsBusqueda = DtConceptos.Select("Clave = " & Row("Concepto"))
                If RowsBusqueda(0).Item("Tipo") = 25 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("Concepto")
                    Item.Importe = CalculaNeto(Row("Importe"), CDbl(TextCambio.Text))
                    Item.TipoIva = 9
                    ListaRetenciones.Add(Item)
                End If
            Next
            '
            Item = New ItemListaConceptosAsientos
            Item.Clave = 213
            Item.Importe = -Total
            ListaConceptos.Add(Item)
        End If

        If Not Asiento(7005, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, CDate(TextFechaContable.Text), 0) Then Return False

        Return True

    End Function
    Public Function UltimaFechacontableGasto(ByVal ConexionStr As String) As Date

        Dim Sql As String = ""

        Sql = "SELECT MAX(FechaContable) FROM GastosBancarioCabeza WHERE Estado <> 3;"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return "1/1/1000"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "2/1/1000"
        End Try

    End Function
    Private Function EstaEnGrid(ByVal Concepto As Integer, ByVal RegIndice As Integer) As Boolean

        Dim Conta As Integer = 0

        For Indice As Integer = 0 To Grid1.Rows.Count - 1
            If Indice <> RegIndice And Grid1.Rows(Indice).Cells("Concepto1").Value = Concepto And Grid1.Rows(Indice).ErrorText = "" Then
                Return True
            End If
        Next

        Return False

    End Function
    Private Function Valida() As Boolean

        If Grid1.Rows.Count = 1 Then
            MsgBox("Falta Informar Conceptos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid1.Focus()
            Return False
        End If

        If TextFechaContable.Text = "" Then
            MsgBox("Falta Informar Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If Not ConsisteFecha(TextFechaContable.Text) Then
            MsgBox("Fecha Contable Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If CierreContableCerrado(CDate(TextFechaContable.Text).Month, CDate(TextFechaContable.Text).Year) Then
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If DiferenciaDias(TextFechaContable.Text, Date.Now) < 0 Then
            MsgBox("Fecha Contable Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        '
        If TextFechaDesde.Text = "" Then
            MsgBox("Falta Informar Fecha Desde.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaDesde.Focus()
            Return False
        End If
        If Not ConsisteFecha(TextFechaDesde.Text) Then
            MsgBox("Fecha Desde Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaDesde.Focus()
            Return False
        End If
        If DiferenciaDias(TextFechaDesde.Text, Date.Now) < 0 Then
            MsgBox("Fecha Desde Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaDesde.Focus()
            Return False
        End If
        '
        If TextFechaHasta.Text = "" Then
            MsgBox("Falta Informar Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaHasta.Focus()
            Return False
        End If
        If Not ConsisteFecha(TextFechaHasta.Text) Then
            MsgBox("Fecha Hasta Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaHasta.Focus()
            Return False
        End If
        If DiferenciaDias(TextFechaHasta.Text, Date.Now) < 0 Then
            MsgBox("Fecha Hasta Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaHasta.Focus()
            Return False
        End If
        '
        If DiferenciaDias(CDate(TextFechaHasta.Text), CDate(TextFechaDesde.Text)) > 0 Then
            MsgBox("Fecha Hasta Menor a Fecha Desde.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaDesde.Focus()
            Return False
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
            If Row.Cells("Importe").Value = 0 Then
                MsgBox("Debe Informar Importe en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.CurrentCell = Grid1.Rows(i).Cells("ImportePantalla")
                Grid1.BeginEdit(True)
                Return False
            End If
        Next

        Dim ImporteRetencion As Double = 0
        For Each Row1 As DataGridViewRow In Grid1.Rows
            If Not IsNothing(Row1.Cells("Concepto1").Value) Then
                If Row1.Cells("TieneLupa").Value Then
                    ImporteRetencion = ImporteRetencion + Row1.Cells("Importe").Value
                End If
            End If
        Next
        If ImporteRetencion <> 0 Then
            If DtRetencionProvinciaAux.Rows.Count = 0 Then
                MsgBox("Falta Distribuir Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.Focus()
                Return False
            End If
            Dim ImporteDistribuido As Double
            For Each Row1 As DataRow In DtRetencionProvinciaAux.Rows
                ImporteDistribuido = ImporteDistribuido + Row1("Importe")
            Next
            If ImporteDistribuido <> ImporteRetencion Then
                MsgBox("Incorrecta Distribución Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.Focus()
                Return False
            End If
        End If

        If CDbl(TextCambio.Text) = 0 Then
            MsgBox("Falta Informar Cambio. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If

        If ComboMoneda.SelectedValue = 1 And CDbl(TextCambio.Text) <> 1 Then
            MsgBox("Cambio Incorrecto para Moneda Nacional. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If

        If CDec(TextGrabado.Text) <> 0 And CDec(TextTotalIva.Text) = 0 Then
            If MsgBox("No se Informó Iva para Conceptos-Grabados Informado. " + vbCrLf + "Puede producir inconsistencias en Informes Impositivos. Desea Continuar?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then Return False
        End If

        If CDec(TextGrabado.Text) = 0 And CDec(TextTotalIva.Text) <> 0 Then
            If MsgBox("No se Informó Conceptos-Grabados para Iva Informado. " + vbCrLf + "Puede producir inconsistencias en Informes Impositivos. Desea Continuar?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then Return False
        End If

        If CDec(TextTotalIva.Text) <> 0 Then
            If Abs(CDec(TextGrabado.Text) - CDec(TextBaseImponible.Text)) > 2 Then
                If MsgBox("Base-Imponible de los Ivas Informados difiere del Neto-Grabado Informado. " + vbCrLf + "Puede producir inconsistencias en Informes Impositivos. Desea Continuar?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then Return False
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
            If ReciboOficialAnt <> DtGastosCabeza.Rows(0).Item("ReciboOficial") Then
                If ExisteReciboOficial(3000, DtGastosCabeza.Rows(0).Item("Movimiento"), DtGastosCabeza.Rows(0).Item("Banco"), DtGastosCabeza.Rows(0).Item("ReciboOficial"), Conexion) Then
                    MsgBox("Recibo Oficial Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    MaskedReciboOficial.Focus()
                    Return False
                End If
            End If
        End If
        '--------------------------------------------------------------------------------------
        Return True

    End Function
    Private Function EsLetraOk(ByVal Letra As String) As Boolean

        Dim LetraW As String

        LetraW = LetraTipoIva(LetrasPermitidasProveedor(1, 500))

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
    Private Sub Grid1_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid1.EditingControlShowing

        'para manejo del autocoplete de conceptos.
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

        If Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "ImportePantalla" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "ImportePantalla" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid1_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellEndEdit

        If Grid1.Columns(e.ColumnIndex).Name = "Concepto1" Then
            Dim RowsBusqueda() As DataRow = DtConceptos.Select("Clave = " & Grid1.Rows(e.RowIndex).Cells("Concepto1").Value)
            If RowsBusqueda(0).Item("Tipo") = 25 Then
                If EstaEnGrid(Grid1.Rows(e.RowIndex).Cells("Concepto1").Value, Grid1.CurrentRow.Index) Then
                    MsgBox("Retención o Percepción ya Existe.")
                    Grid1.Rows(e.RowIndex).Cells("Concepto1").Value = 0
                End If
                If EsRetencionPorProvincia(Grid1.Rows(e.RowIndex).Cells("Concepto1").Value) Then Grid1.Rows(e.RowIndex).Cells("TieneLupa").Value = True
            Else
                Grid1.Rows(e.RowIndex).Cells("TieneLupa").Value = False
            End If
            Grid1.Refresh()
        End If

        If Grid1.Columns(e.ColumnIndex).Name = "ImportePantalla" Then
            If IsNumeric(Grid1.Rows(e.RowIndex).Cells("ImportePantalla").Value) Then
                Grid1.Rows(e.RowIndex).Cells("Importe").Value = Grid1.Rows(e.RowIndex).Cells("ImportePantalla").Value
                If Grid1.Rows(e.RowIndex).Cells("Importe").Value < 0 Then
                    Grid1.Rows(e.RowIndex).Cells("Importe").Value = -1 * Grid1.Rows(e.RowIndex).Cells("Importe").Value
                End If
                CalculaTotales()
            End If
        End If

    End Sub
    Private Sub Grid1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid1.Columns(e.ColumnIndex).Name = "Lupa" Then
            If Not Grid1.CurrentRow.Cells("TieneLupa").Value Then Exit Sub
            If Grid1.CurrentRow.Cells("Importe").Value = 0 Then
                MsgBox("Debe Informar Importe")
                Exit Sub
            End If
            SeleccionarRetencionesProvincias.PDtGrid = DtRetencionProvinciaAux
            SeleccionarRetencionesProvincias.PImporte = Grid1.CurrentRow.Cells("Importe").Value
            SeleccionarRetencionesProvincias.PRetencion = Grid1.CurrentRow.Cells("Concepto1").Value
            SeleccionarRetencionesProvincias.ShowDialog()
            SeleccionarRetencionesProvincias.Dispose()
        End If

    End Sub
    Private Sub Grid1_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid1.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid1.Columns(e.ColumnIndex).Name = "ImportePantalla" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#.##")
                Else : e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

        If Grid1.Columns(e.ColumnIndex).Name = "Lupa" Then
            If Not IsDBNull(Grid1.Rows(e.RowIndex).Cells("TieneLupa").Value) Then
                If Grid1.Rows(e.RowIndex).Cells("TieneLupa").Value Then
                    e.Value = ImageList1.Images.Item("Lupa")
                Else : e.Value = Nothing
                End If
            End If
        End If

    End Sub
    Private Sub Grid1_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid1.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid1_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row

        Row("Movimiento") = PMovimiento
        Row("Item") = 0
        Row("Concepto") = 0
        Row("Importe") = 0
        Row("ImportePantalla") = 0
        Row("TieneLupa") = False

    End Sub
    Private Sub DtGrid1_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("ImportePantalla") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

    End Sub


End Class