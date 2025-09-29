Public Class UnAsiento
    Public PAsiento As Double
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim DtDetalle As DataTable
    Dim DtCabeza As DataTable
    Dim DtLotes As DataTable
    Dim DtGrid As DataTable
    Dim DtGridLotes As New DataTable
    '
    Dim ConexionAsiento As String
    Private Sub UnAsiento_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(11) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False
        Grid.Columns("Lupa").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")

        ComboNegocio.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;")
        Dim Row As DataRow = ComboNegocio.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboNegocio.DataSource.rows.add(Row)
        ComboNegocio.DisplayMember = "Nombre"
        ComboNegocio.ValueMember = "Clave"
        ComboNegocio.SelectedValue = 0
        With ComboNegocio
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboNegocio_SelectionChangeCommitted(Nothing, Nothing)
        With ComboCosteo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        If PAsiento = 0 Then PAbierto = True

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionAsiento = Conexion
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionAsiento = ConexionN
        End If

        LlenaCombosGrid()

        CreaDtGrid()
        CreaDtGridLotes()

        MuestraDatos()

    End Sub
    Private Sub UnAsiento_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()
        bs.EndEdit()

        If DtCabeza.Rows(0).Item("Estado") = 3 Then
            MsgBox("Asiento esta anulado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        If DtGrid.HasErrors Then
            MsgBox("Debe Corregir errores antes de Realizar los Cambios. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        Dim DtDetalleAux As DataTable = DtDetalle.Copy
        Dim DtlotesAux As DataTable = DtLotes.Copy
        Dim RowsBusqueda() As DataRow

        If DtCabezaAux.Rows(0).Item("IntFecha") <> DateTime1.Value.Year & Format(DateTime1.Value.Month, "00") & Format(DateTime1.Value.Day, "00") Then
            DtCabezaAux.Rows(0).Item("IntFecha") = DateTime1.Value.Year & Format(DateTime1.Value.Month, "00") & Format(DateTime1.Value.Day, "00")
        End If

        For Each Row As DataRow In DtGrid.Rows
            If Row("Item") <> 0 Then
                RowsBusqueda = DtDetalleAux.Select("Item = " & Row("Item"))
                If RowsBusqueda.Length <> 0 Then
                    If Row("CuentaP") <> RowsBusqueda(0).Item("Cuenta") Then RowsBusqueda(0).Item("Cuenta") = Row("CuentaP")
                    If Row("Debe") <> RowsBusqueda(0).Item("Debe") Then RowsBusqueda(0).Item("Debe") = Row("Debe")
                    If Row("Haber") <> RowsBusqueda(0).Item("Haber") Then RowsBusqueda(0).Item("Haber") = Row("Haber")
                End If
            Else
                Dim Row1 As DataRow = DtDetalleAux.NewRow
                Row1("Asiento") = DtCabeza.Rows(0).Item("Asiento")
                Row1("Item") = 0
                Row1("Cuenta") = Row("CuentaP")
                Row1("Concepto") = Row("Concepto")
                Row1("Debe") = Row("Debe")
                Row1("Haber") = Row("Haber")
                DtDetalleAux.Rows.Add(Row1)
            End If
        Next
        For Each Row As DataRow In DtDetalleAux.Rows
            RowsBusqueda = DtGrid.Select("Item = " & Row("Item"))
            If RowsBusqueda.Length = 0 Then
                Row.Delete()
            End If
        Next

        'Actualiza Lotes Imputados.
        For Each Row1 As DataRow In DtGridLotes.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtlotesAux.Select("Lote = " & Row1("Lote") & " AND Secuencia = " & Row1("Secuencia"))
                If RowsBusqueda.Length = 0 Then
                    Dim Row As DataRow = DtlotesAux.NewRow()
                    Row("Asiento") = PAsiento
                    Row("Operacion") = Row1("Operacion")
                    Row("Lote") = Row1("Lote")
                    Row("Secuencia") = Row1("Secuencia")
                    Row("ImporteConIva") = 0
                    Row("ImporteSinIva") = 0
                    DtlotesAux.Rows.Add(Row)
                End If
            End If
        Next
        For Each Row1 As DataRow In DtlotesAux.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtGridLotes.Select("Lote = " & Row1("Lote") & " AND Secuencia = " & Row1("Secuencia"))
                If RowsBusqueda.Length = 0 Then Row1.Delete()
            End If
        Next
        If Not IsNothing(DtlotesAux.GetChanges) Then
            ProrroteaImportesLotes(DtlotesAux)
        End If

        If IsNothing(DtCabezaAux.GetChanges) And IsNothing(DtDetalleAux.GetChanges) And IsNothing(DtlotesAux.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PAsiento = 0 Then
            If HacerAlta(DtCabezaAux, DtDetalleAux, DtlotesAux) Then MuestraDatos()
        Else
            If HacerModificacion(DtCabezaAux, DtDetalleAux, DtlotesAux) Then MuestraDatos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PAsiento = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Asiento ya esta ANULADO. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If MsgBox("Asiento se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        Dim DtDetalleAux As DataTable = DtDetalle.Copy
        Dim DtLotesAux As DataTable = DtLotes.Copy

        For Each Row As DataRow In DtLotesAux.Rows
            Row.Delete()
        Next

        DtCabezaAux.Rows(0).Item("Estado") = 3

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Resul As Double = ActualizaAsiento(DtCabezaAux, DtDetalleAux, DtLotesAux)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If Resul < 0 Then
            MsgBox("ERROR: En base de datos. Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("ERROR: Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Asiento Anulado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            MuestraDatos()
        End If

    End Sub
    Private Sub ButtonLotesAImputar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonLotesAImputar.Click

        If ComboNegocio.SelectedValue <> 0 Then
            MsgBox("No se puede Imputar Lotes Si se Asigna a Un Costeo.")
            Exit Sub
        End If

        OpcionLotesAImputar.PDtGrid = DtGridLotes
        OpcionLotesAImputar.ShowDialog()
        OpcionLotesAImputar.Dispose()

    End Sub
    Private Sub ComboNegocio_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboNegocio.SelectionChangeCommitted

        If ComboNegocio.SelectedValue <> 0 Then
            If DtGridLotes.Rows.Count <> 0 Then
                MsgBox("No se puede Asignar un Negocio si se Imputo Lotes.")
                ComboNegocio.SelectedValue = 0
                Exit Sub
            End If
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "IntFechaDesde <= " & Format(DateTime1.Value, "yyyyMMdd") & " AND IntFechaHasta >= " & Format(DateTime1.Value, "yyyyMMdd") & ";"
        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & ComboNegocio.SelectedValue & " AND Cerrado = 0 AND " & SqlFecha
        ComboCosteo.DataSource = New DataTable
        ComboCosteo.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboCosteo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        ComboCosteo.DataSource.rows.add(Row)
        ComboCosteo.DisplayMember = "Nombre"
        ComboCosteo.ValueMember = "Costeo"
        ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub ButtonImportePorLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportePorLotes.Click

        SeleccionarVarios.PAbierto = PAbierto
        SeleccionarVarios.PNota = PAsiento
        SeleccionarVarios.PEsImportePorLotesAsientos = True
        SeleccionarVarios.Show()

    End Sub
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub CheckBoxDebito_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxDebito.Click

        If CheckBoxDebito.Checked Then
            CheckBoxCredito.Checked = False
        End If

    End Sub
    Private Sub CheckBoxCredito_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxCredito.Click

        If CheckBoxCredito.Checked Then
            CheckBoxDebito.Checked = False
        End If

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PAsiento = 0
        PAbierto = True

        UnAsiento_Load(Nothing, Nothing)

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            PAbierto = False
            ConexionAsiento = ConexionN
        Else : PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
            ConexionAsiento = Conexion
        End If

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then
            MsgBox("No hay un registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Grid.Rows.Remove(Grid.CurrentRow)

        CalculaTotales()

    End Sub
    Private Sub MuestraDatos()

        DtGrid.Clear()
        DtGridlotes.Clear()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtCabeza = New DataTable
        Dim Sql As String = "SELECT * FROM AsientosCabeza WHERE Asiento = " & PAsiento & ";"
        If Not Tablas.Read(Sql, ConexionAsiento, DtCabeza) Then Me.Close() : Exit Sub
        If PAsiento <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Asiento No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PAsiento = 0 Then
            Dim Row As DataRow = DtCabeza.NewRow
            Row("Asiento") = 0
            Row("IntFecha") = 0
            Row("Costeo") = 0
            Row("Estado") = 1
            Row("TipoDocumento") = 0
            Row("Documento") = 0
            Row("Comentario") = ""
            Row("TipoComprobante") = 0
            Row("Debito") = False
            Row("Credito") = False
            DtCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtDetalle = New DataTable
        Sql = "SELECT * FROM AsientosDetalle WHERE Asiento = " & PAsiento & ";"
        If Not Tablas.Read(Sql, ConexionAsiento, DtDetalle) Then Me.Close() : Exit Sub
        For Each Row As DataRow In DtDetalle.Rows
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Item") = Row("Item")
            Row1("CuentaP") = Row("Cuenta")
            Row1("CuentaStr") = Format(Row("Cuenta"), "000-000000-00")
            Row1("Centro") = 0
            Row1("Cuenta") = 0
            Row1("SubCuenta") = 0
            HallaPartesCuenta(Row("Cuenta"), Row1("Centro"), Row1("Cuenta"), Row1("SubCuenta"))
            Row1("SubCuenta") = Row1("Cuenta") & Format(Row1("SubCuenta"), "00")
            Row1("Debe") = Row("Debe")
            Row1("Haber") = Row("Haber")
            DtGrid.Rows.Add(Row1)
        Next
        DtLotes = New DataTable
        Sql = "SELECT * FROM AsientosLotes WHERE Asiento = " & PAsiento & ";"
        If Not Tablas.Read(Sql, ConexionAsiento, DtLotes) Then Me.Close() : Exit Sub
        For Each Row2 As DataRow In DtLotes.Rows
            Dim Row As DataRow = DtGridLotes.NewRow
            Row("Operacion") = Row2("Operacion")
            Row("Lote") = Row2("Lote")
            Row("Secuencia") = Row2("Secuencia")
            Row("Cantidad") = HallaCantidadLote(Row2("Lote"), Row2("Secuencia"), Row2("Operacion"))
            DtGridLotes.Rows.Add(Row)
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        CalculaTotales()

        If PAsiento = 0 Then
            PictureCandado.Enabled = True
        Else : PictureCandado.Enabled = False
        End If

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)
        AddHandler DtGrid.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid_RowChanged)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function HacerAlta(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtLotesW As DataTable) As Boolean

        'Graba Asiento.
        Dim NumeroAsiento As Double = 0
        Dim Resul As Double

        For i As Integer = 1 To 50
            NumeroAsiento = UltimaNumeracion(ConexionAsiento)
            If NumeroAsiento < 0 Then
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return -2
            End If

            DtCabezaW.Rows(0).Item("Asiento") = NumeroAsiento
            For Each Row As DataRow In DtDetalleW.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Asiento") = NumeroAsiento
                End If
            Next

            For Each Row As DataRow In DtLotesW.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Asiento") = NumeroAsiento
                End If
            Next

            Resul = ActualizaAsiento(DtCabezaW, DtDetalleW, DtLotesW)

            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PAsiento = NumeroAsiento
            Return True
        End If

    End Function
    Private Function HacerModificacion(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtLotesW As DataTable) As Boolean

        Dim Resul As Double

        GModificacionOk = False

        Resul = ActualizaAsiento(DtCabezaW, DtDetalleW, DtLotesW)

        If Resul < 0 Then
            MsgBox("ERROR: En base de datos. Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = 0 Then
            MsgBox("ERROR: Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul > 0 Then
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Return True
        End If

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Enlace As Binding
        Dim Row As DataRowView = MiEnlazador.Current

        Enlace = New Binding("Text", MiEnlazador, "Asiento")
        AddHandler Enlace.Format, AddressOf FormatAsiento
        TextAsiento.DataBindings.Clear()
        TextAsiento.DataBindings.Add(Enlace)

        If Row("IntFecha") <> 0 Then
            datetime1.Value = Strings.Left(Row("IntFecha"), 4) & "/" & Strings.Mid(Row("IntFecha"), 5, 2) & "/" & Strings.Right(Row("IntFecha"), 2)
        End If

        If Row("Costeo") <> 0 Then
            ComboNegocio.SelectedValue = HallaNegocio(Row("Costeo"))
            ComboNegocio_SelectionChangeCommitted(Nothing, Nothing)
        End If

        Enlace = New Binding("SelectedValue", MiEnlazador, "Costeo")
        ComboCosteo.DataBindings.Clear()
        ComboCosteo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Debito")
        CheckBoxDebito.DataBindings.Clear()
        CheckBoxDebito.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Credito")
        CheckBoxCredito.DataBindings.Clear()
        CheckBoxCredito.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatAsiento(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then Numero.Value = Format(0, "#")

    End Sub
    Private Sub FormatFecha(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value <> 0 Then
            Numero.Value = Numero.Value.ToString.Substring(6, 2) & "/" & Numero.Value.ToString.Substring(4, 2) & "/" & Numero.Value.ToString.Substring(0, 4)
        Else
            Numero.Value = Format(Date.Now, "dd/MM/yyyy")
        End If

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Item As New DataColumn("Item")
        Item.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Item)

        Dim CuentaP As New DataColumn("CuentaP")
        CuentaP.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(CuentaP)

        Dim CuentaStr As New DataColumn("CuentaStr")
        CuentaStr.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(CuentaStr)
        Dim Centro As New DataColumn("Centro")
        Centro.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Centro)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cuenta)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Concepto)

        Dim SubCuenta As New DataColumn("SubCuenta")
        SubCuenta.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(SubCuenta)

        Dim Debe As New DataColumn("Debe")
        Debe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Debe)

        Dim Haber As New DataColumn("Haber")
        Haber.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Haber)

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
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGridLotes.Columns.Add(Cantidad)

    End Sub
    Private Sub ProrroteaImportesLotes(ByVal DtLotesAux As DataTable)

        If DtGridLotes.Rows.Count = 0 Then Exit Sub

        Dim Total As Decimal
        For Each Row As DataRow In DtGrid.Rows
            Total = Total + Row("Debe")
        Next

        Dim Cantidad As Decimal = 0

        For Each Row As DataRow In DtGridLotes.Rows
            Cantidad = Cantidad + Row("Cantidad")
        Next

        Dim ImporteConIva As Decimal = Total
        Dim ImporteSinIva As Decimal = Total

        Dim IndiceCorreccionConIva As Decimal = ImporteConIva / Cantidad
        Dim IndiceCorreccionSinIva As Decimal = ImporteSinIva / Cantidad

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtLotesAux.Rows
            If Row.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtGridLotes.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
                Row("ImporteConIva") = Trunca(IndiceCorreccionConIva * RowsBusqueda(0).Item("Cantidad"))
                Row("ImporteSinIva") = Trunca(IndiceCorreccionSinIva * RowsBusqueda(0).Item("Cantidad"))
            End If
        Next

    End Sub
    Private Sub BuscaCosteo(ByVal Negocio As Integer)

        Dim SqlFecha As String = ""
        SqlFecha = "IntFechaDesde <= " & Format(DateTime1.Value, "yyyyMMdd") & " AND IntFechaHasta >= " & Format(DateTime1.Value, "yyyyMMdd") & ";"
        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & Negocio & " AND Cerrado = 0 AND " & SqlFecha
        ComboCosteo.DataSource = New DataTable
        ComboCosteo.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboCosteo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        ComboCosteo.DataSource.rows.add(Row)
        ComboCosteo.DisplayMember = "Nombre"
        ComboCosteo.ValueMember = "Costeo"
        ComboCosteo.SelectedValue = 0
        With ComboCosteo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Private Sub CalculaTotales()

        Dim TotalDebe As Decimal = 0
        Dim TotalHaber As Decimal = 0

        For I As Integer = 0 To Grid.Rows.Count - 2
            TotalDebe = TotalDebe + Grid.Rows(I).Cells("Debe").Value
            TotalHaber = TotalHaber + Grid.Rows(I).Cells("Haber").Value
        Next

        TextTotalDebe.Text = FormatNumber(TotalDebe, 2)
        TextTotalHaber.Text = FormatNumber(TotalHaber, 2)
        TextSaldo.Text = FormatNumber(TotalDebe - TotalHaber, 2)

    End Sub
    Private Sub LlenaCombosGrid()

        Centro.DataSource = Tablas.Leer("SELECT Centro,Nombre FROM CentrosDeCosto;")
        Dim Row As DataRow = Centro.DataSource.newrow
        Row("Centro") = 0
        Row("Nombre") = " "
        Centro.DataSource.rows.add(Row)
        Centro.DisplayMember = "Nombre"
        Centro.ValueMember = "Centro"

        Cuenta.DataSource = Tablas.Leer("SELECT Cuenta,Nombre FROM Cuentas;")
        Row = Cuenta.DataSource.newrow
        Row("Cuenta") = 0
        Row("Nombre") = " "
        Cuenta.DataSource.rows.add(Row)
        Cuenta.DisplayMember = "Nombre"
        Cuenta.ValueMember = "Cuenta"

        SubCuenta.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM SubCuentas;")
        Row = SubCuenta.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = " "
        SubCuenta.DataSource.rows.add(Row)
        SubCuenta.DisplayMember = "Nombre"
        SubCuenta.ValueMember = "Clave"

    End Sub
    Private Function ActualizaAsiento(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtLotesW As DataTable) As Double

        Dim Trans As OleDb.OleDbTransaction

        GModificacionOk = False

        Using MiConexion As New OleDb.OleDbConnection(ConexionAsiento)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Try
                    If Not IsNothing(DtCabezaW.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosCabeza;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtCabezaW.GetChanges)
                        End Using
                    End If
                    If Not IsNothing(DtDetalleW.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosDetalle;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtDetalleW.GetChanges)
                        End Using
                    End If
                    If Not IsNothing(DtLotesW.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosLotes;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtLotesW.GetChanges)
                        End Using
                    End If
                    Trans.Commit()
                    GModificacionOk = True
                    Return 1000
                Catch ex As OleDb.OleDbException
                    Trans.Rollback()
                    Return -2
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
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Asiento) FROM AsientosCabeza;", Miconexion)
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
    Private Function Valida() As Boolean

        If DtGrid.Rows.Count = 0 Then
            MsgBox("Debe Informar Cuenta", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        For Each Row As DataRow In DtGrid.Rows
            Row.RowError = ""
            If Row("CuentaP") = 0 Then
                Row.RowError = "Error"
                MsgBox("Debe Informar Cuenta", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.Refresh()
                Return False
            End If
        Next

        For Each Row As DataRow In DtGrid.Rows
            If Row("Debe") = 0 And Row("Haber") = 0 Then
                MsgBox("Debe Informar Debe o Haber.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        Next

        If ComboNegocio.SelectedValue <> 0 And ComboCosteo.SelectedValue = 0 Then
            MsgBox("Falta Informar Costeo del Negocio. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboCosteo.Focus()
            Return False
        End If

        Dim TotalDebe As Decimal
        Dim TotalHaber As Decimal
        For Each Row As DataRow In DtGrid.Rows
            TotalDebe = TotalDebe + Row("Debe")
            TotalHaber = TotalHaber + Row("Haber")
        Next
        If TotalDebe <> TotalHaber Then
            MsgBox("Asiento NO Balancea.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If ComboNegocio.SelectedValue <> 0 Or DtGridLotes.Rows.Count <> 0 Then
            If Not CheckBoxDebito.Checked And Not CheckBoxCredito.Checked Then
                MsgBox("Debe Informar si el Asiento es Debito O Crédito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        Else
            If CheckBoxDebito.Checked Or CheckBoxCredito.Checked Then
                MsgBox("Solo se Informa Asiento Debito O Crédito si se imputa a Lotes o Costeo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        'para manejo del autocoplete de articulos.
        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        CalculaTotales()

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Debe" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Haber" Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If e.KeyChar = "." Then e.KeyChar = ","
        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If CType(sender, TextBox).Text <> "" Then
            If Not IsNumeric(CType(sender, TextBox).Text) Then
                CType(sender, TextBox).Text = ""
                CType(sender, TextBox).Focus()
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If IsNumeric(e.Value) Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
            Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            e.Value = Nothing
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            SeleccionarCuenta.PCentro = 0
            SeleccionarCuenta.ShowDialog()
            If SeleccionarCuenta.PCuenta <> 0 Then
                Dim Centro As Integer
                Dim Cuenta As Integer
                Dim SubCuenta As Integer
                HallaPartesCuenta(SeleccionarCuenta.PCuenta, Centro, Cuenta, SubCuenta)
                Grid.Rows(e.RowIndex).Cells("CuentaP").Value = SeleccionarCuenta.PCuenta
                Grid.Rows(e.RowIndex).Cells("Centro").Value = Centro
                Grid.Rows(e.RowIndex).Cells("Cuenta").Value = Cuenta
                Grid.Rows(e.RowIndex).Cells("SubCuenta").Value = Cuenta & Format(SubCuenta, "00")
                Grid.Rows(e.RowIndex).Cells("CuentaStr").Value = Format(SeleccionarCuenta.PCuenta, "000-000000-00")
                Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Debe")
            End If
            SeleccionarCuenta.Dispose()
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Item") = 0
        e.Row("CuentaStr") = ""
        e.Row("CuentaP") = 0
        e.Row("Centro") = 0
        e.Row("Concepto") = 0
        e.Row("Cuenta") = 0
        e.Row("SubCuenta") = 0
        e.Row("Debe") = 0
        e.Row("Haber") = 0

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Debe") Or e.Column.ColumnName.Equals("Haber") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

    End Sub
    Private Sub DtGrid_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        'Borra las lineas en blancos que aparecen cuando hago click en el ultimo renglon y sin informar nada regreso a algun renglon anterior. 
        If e.Row("Cuenta") = 0 And e.Row("Debe") = 0 And e.Row("Haber") = 0 Then e.Row.Delete()

    End Sub

End Class