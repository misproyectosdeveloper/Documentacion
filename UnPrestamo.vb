Imports System.Transactions
Public Class UnPrestamo
    Public PPrestamo As Double
    Public PAbierto As Boolean
    Public PSaldoInicial As Boolean
    Public PBloqueaFunciones As Boolean
    Public PActualizacionOk As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim DtPrestamosCabeza As DataTable
    Dim DtPrestamosDetallePago As DataTable
    Dim DtFormasPago As DataTable
    Dim DtGrid As DataTable
    Dim DtSucursales As DataTable
    ' 
    Dim cb As ComboBox
    Dim ConexionPrestamo As String
    Dim TotalConceptos As Double
    Dim CapitalW As Double
    Private Sub UnPrestamo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(800) Then PBloqueaFunciones = True

        If GCaja = 0 And PPrestamo = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        Grid.AutoGenerateColumns = False
        Grid.Columns("LupaCuenta").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")

        If PPrestamo = 0 Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

        If PPrestamo <> 0 Then
            If PAbierto Then PictureCandado.Image = ImageList1.Images.Item("Abierto")
            If Not PAbierto Then PictureCandado.Image = ImageList1.Images.Item("Cerrado")
        End If

        If Not PermisoTotal Then
            PictureCandado.Visible = False
        End If

        ArmaMedioPagoParaPrestamo(DtFormasPago)

        ArmaDtSucursales()

        Grid.Columns("Detalle").Visible = False
        Grid.Columns("Neto").Visible = False
        Grid.Columns("Alicuota").Visible = False
        Grid.Columns("Iva").Visible = False

        LlenaCombosGrid()

        ComboEstado.DataSource = DtEstadoActivoYBaja()
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

        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboSucursal.DataSource = ArmaSucursalesDeBanco(0)
        ComboSucursal.DisplayMember = "Nombre"
        ComboSucursal.ValueMember = "Clave"
        ComboSucursal.SelectedValue = 0
        With ComboSucursal
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PAbierto Then
            ConexionPrestamo = Conexion
        Else : ConexionPrestamo = ConexionN
        End If

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        PActualizacionOk = False

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)

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

        Grid.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim Fecha As Date = DateTime1.Value
        If CierreContableCerrado(Fecha.Month, Fecha.Year) Then
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtPrestamosCabezaAux As DataTable = DtPrestamosCabeza.Copy
        Dim DtPrestamosDetallePagoAux As DataTable = DtPrestamosDetallePago.Copy

        ActualizaArchivos(DtPrestamosCabezaAux, DtPrestamosDetallePagoAux)

        If IsNothing(DtPrestamosCabezaAux.GetChanges) And IsNothing(DtPrestamosDetallePagoAux.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        '
        If GGeneraAsiento And Not IsNothing(DtPrestamosDetallePagoAux.GetChanges) Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtPrestamosDetallePago, DtPrestamosDetallePagoAux, DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If PPrestamo = 0 Then
            If HacerAlta(DtPrestamosCabezaAux, DtPrestamosDetallePagoAux, DtAsientoCabeza, DtAsientoDetalle) Then
                UnPrestamo_Load(Nothing, Nothing)
            End If
        Else
            If HacerModificacion(DtPrestamosCabezaAux, DtPrestamosDetallePagoAux, DtAsientoCabeza, DtAsientoDetalle) Then
                UnPrestamo_Load(Nothing, Nothing)
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If GCaja = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Cancelación esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionPrestamo)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Prestamo) FROM PrestamosMovimientoCabeza WHERE Estado = 1 AND Prestamo = " & DtPrestamosCabeza.Rows(0).Item("Prestamo") & ";", Miconexion)
                    If CInt(Cmd.ExecuteScalar()) <> 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Existe Movimientos No Anulados. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End Using
            End Using
        Catch ex As Exception
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End Try

        Dim DtPrestamosCabezaAux As DataTable = DtPrestamosCabeza.Copy
        Dim DtPrestamosDetallepagoAux As DataTable = DtPrestamosDetallePago.Copy

        ActualizaArchivos(DtPrestamosCabezaAux, DtPrestamosDetallepagoAux)

        If Not IsNothing(DtPrestamosCabezaAux.GetChanges) Or Not IsNothing(DtPrestamosDetallepagoAux.GetChanges) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid.EndEdit()
        MiEnlazador.EndEdit()

        Dim Resul As Double

        For Each Row As DataGridViewRow In Grid.Rows
            If IsDBNull(Row.Cells("Concepto").Value) Then Exit For
            If Row.Cells("Concepto").Value = 3 Then
                Resul = ChequeEntregado(Row.Cells("Concepto").Value, ConexionPrestamo, Row.Cells("ClaveCheque").Value)
                If Resul < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Error Base de Datos")
                    Exit Sub
                End If
                If Resul > 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Cheque " & Row.Cells("Numero").Value & " no se puede Borrar, fue usado para Pago o Depositado. Operación se CANCELA.")
                    Exit Sub
                End If
            End If
        Next

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        '
        If Not HallaAsientosCabeza(8000, PPrestamo, DtAsientoCabeza, ConexionPrestamo) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If
        For Each Row As DataRow In DtAsientoCabeza.Rows
            Row("Estado") = 3
        Next

        If MsgBox("El Prestamo se eliminara definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtPrestamosCabezaAux.Rows(0).Item("Estado") = 3

        Resul = ActualizaMovi("B", DtPrestamosCabezaAux, DtPrestamosDetallepagoAux, ConexionPrestamo, DtAsientoCabeza, DtAsientoDetalle)
        If Resul < 0 And Resul <> -3 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Prestamo Anulado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PActualizacionOk = True
            UnPrestamo_Load(Nothing, Nothing)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PPrestamo = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 8000
        If PAbierto Then
            ListaAsientos.PDocumentoB = PPrestamo
        Else
            ListaAsientos.PDocumentoN = PPrestamo
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub RadioBancario_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioBancario.CheckedChanged

        If RadioBancario.Checked Then
            PanelBancario.Visible = True
            PanelEmisor.Visible = False
            ComboEmisor.SelectedValue = 0
            ComboBancos.SelectedValue = 0 : ComboSucursal.SelectedValue = 0
        End If

    End Sub
    Private Sub RadioProveedor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioProveedor.CheckedChanged

        If RadioProveedor.Checked Then
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            PanelBancario.Visible = False
            PanelEmisor.Visible = True
            ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion <> 4")
            Dim Row As DataRow = ComboEmisor.DataSource.NewRow()
            Row("Clave") = 0
            Row("Nombre") = ""
            ComboEmisor.DataSource.Rows.Add(Row)
            ComboEmisor.DisplayMember = "Nombre"
            ComboEmisor.ValueMember = "Clave"
            LabelEmisor.Text = "Proveeedor"
            ComboEmisor.SelectedValue = 0
            ComboBancos.SelectedValue = 0 : ComboSucursal.SelectedValue = 0
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End If

    End Sub
    Private Sub RadioCliente_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioCliente.CheckedChanged

        If RadioCliente.Checked Then
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            PanelBancario.Visible = False
            PanelEmisor.Visible = True
            LlenaCombo(ComboEmisor, "", "Clientes") : LabelEmisor.Text = "Cliente"
            ComboEmisor.SelectedValue = 0
            ComboBancos.SelectedValue = 0 : ComboSucursal.SelectedValue = 0
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End If

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        If RadioProveedor.Checked And ComboEmisor.SelectedValue <> 0 And ComboEmisor.SelectedValue = GProveedorEgresoCaja Then
            MsgBox("Proveedor Egreso de Caja Inválida pera este Comprobante.", MsgBoxStyle.Information)
            ComboEmisor.SelectedValue = 0
            Exit Sub
        End If

    End Sub
    Private Sub ComboBancos_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboBancos.Validating

        If IsNothing(ComboBancos.SelectedValue) Then ComboBancos.SelectedValue = 0
        If ComboBancos.SelectedValue = 0 Then
            ComboSucursal.SelectedValue = 0
            Exit Sub
        End If

        ComboSucursal.DataSource = ArmaSucursalesDeBanco(ComboBancos.SelectedValue)
        ComboSucursal.DisplayMember = "Nombre"
        ComboSucursal.ValueMember = "Clave"
        ComboSucursal.SelectedValue = 0

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then Exit Sub

        Dim Row As DataRow = DtGrid.Rows.Item(Grid.CurrentRow.Index)

        If Row("MedioPago") = 3 And PPrestamo <> 0 Then
            Dim Resul As Double = ChequeEntregado(Row("MedioPago"), ConexionPrestamo, Row("ClaveCheque"))
            If Resul < 0 Then
                MsgBox("Error Base de Datos")
                Exit Sub
            End If
            If Resul > 0 Then
                MsgBox("Cheque no se puede Borrar, fue usado para Pago o Depositado.")
                Exit Sub
            End If
        End If

        Row.Delete()

        CalculaTotales()

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PPrestamo <> 0 Then Exit Sub

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            PAbierto = False
        Else
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

    End Sub
    Private Sub TextCapital_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCapital.KeyPress

        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCapital_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextCapital.Validating

        If IsNumeric(TextCapital.Text) Then TextCapital.Text = Trunca(CDbl(TextCapital.Text))

    End Sub
    Private Sub TextInteres_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextInteres.KeyPress

        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCuotas_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCuotas.KeyPress

        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNumeracionTercero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumeracionTercero.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCuenta_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()

        Dim Sql As String
        Dim Row1 As DataRow
        Dim RowsBusqueda() As DataRow

        DtPrestamosCabeza = New DataTable
        Sql = "SELECT * FROM PrestamosCabeza WHERE Prestamo = " & PPrestamo & ";"
        If Not Tablas.Read(Sql, ConexionPrestamo, DtPrestamosCabeza) Then Return False
        If PPrestamo <> 0 And DtPrestamosCabeza.Rows.Count = 0 Then
            MsgBox("Prestamo No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PPrestamo = 0 Then
            Dim Row As DataRow = DtPrestamosCabeza.NewRow
            Row("Prestamo") = 0
            Row("Origen") = 0
            Row("Emisor") = 0
            Row("Sucursal") = 0
            Row("Fecha") = DateTime1.Value
            Row("FechaOtorgado") = DateTime2.Value
            Row("Capital") = 0
            Row("Interes") = 0
            Row("Cuotas") = 0
            Row("NumeracionTercero") = 0
            Row("Caja") = GCaja
            Row("Comentario") = ""
            Row("Estado") = 1
            DtPrestamosCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtPrestamosDetallePago = New DataTable
        Sql = "SELECT * FROM PrestamosDetalle WHERE Prestamo = " & PPrestamo & ";"
        If Not Tablas.Read(Sql, ConexionPrestamo, DtPrestamosDetallePago) Then Return False

        Dim DtCheques As New DataTable
        For Each Row As DataRow In DtPrestamosDetallePago.Rows
            If Row("ClaveCheque") <> 0 Then
                If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionPrestamo, DtCheques) Then Return False
            End If
        Next

        For Each Row As DataRow In DtPrestamosDetallePago.Rows
            Row1 = DtGrid.NewRow
            Row1("Item") = Row("Item")
            Row1("MedioPago") = Row("MedioPago")
            Row1("Importe") = Row("Importe")
            Row1("Cambio") = Row("Cambio")
            Row1("Bultos") = 0
            Row1("Detalle") = 0
            Row1("Alicuota") = 0
            Row1("Neto") = 0
            Row1("ImporteIva") = 0
            Row1("NumeracionInicial") = 1
            Row1("NumeracionFinal") = 999999999
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            If Row("ClaveCheque") <> 0 Then
                RowsBusqueda = DtCheques.Select("MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque"))
                If RowsBusqueda.Length <> 0 Then
                    Row1("Banco") = RowsBusqueda(0).Item("Banco")
                    Row1("Cuenta") = RowsBusqueda(0).Item("Cuenta")
                    Row1("Serie") = RowsBusqueda(0).Item("Serie")
                    Row1("Numero") = RowsBusqueda(0).Item("Numero")
                    Row1("EmisorCheque") = RowsBusqueda(0).Item("EmisorCheque")
                    Row1("Fecha") = RowsBusqueda(0).Item("Fecha")
                    Row1("eCheq") = RowsBusqueda(0).Item("eCheq")     'eCheq.
                Else
                    Row1("Banco") = 0
                    Row1("Cuenta") = 0
                    Row1("Serie") = ""
                    Row1("Numero") = 0
                    Row1("EmisorCheque") = "Comp.No Existe."
                    Row1("Fecha") = "1/1/1800"
                    Row1("eCheq") = 0
                End If
            Else
                Row1("Banco") = Row("Banco")
                Row1("Cuenta") = Row("Cuenta")
                Row1("Serie") = ""
                Row1("Numero") = 0
                Row1("EmisorCheque") = ""
                Row1("Fecha") = "1/1/1800"
            End If
            If Row("MedioPago") = 2 Then Row1("ClaveCheque") = 0
            DtGrid.Rows.Add(Row1)
        Next

        DtCheques.Dispose()

        Grid.DataSource = DtGrid
        Grid.EndEdit()

        CalculaTotales()

        If PPrestamo <> 0 And DtPrestamosDetallePago.Rows.Count = 0 Then
            PSaldoInicial = True
        End If

        If PSaldoInicial Then
            Panel2.Visible = False
            Label2.Text = "Capital Inicial"
        Else : Panel2.Visible = True
            Label2.Text = "Capital Otorgado"
        End If

        'Precenta las lineas del grid.
        For Each Row As DataGridViewRow In Grid.Rows
            If Not IsNothing(Row.Cells("Concepto").Value) Then
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row.Cells("Concepto").Value)
                ArmaGridSegunConcepto(Row, RowsBusqueda(0).Item("Tipo"), 60, False, False, PAbierto)
            End If
        Next

        ProtejeGrid(Grid)

        If PPrestamo = 0 Then
            Panel3.Enabled = True
            PanelBancario.Enabled = True
        Else : Panel3.Enabled = False
            PanelBancario.Enabled = False
        End If

        If ComboEstado.SelectedValue = 3 Then
            Panel1.Enabled = False
            Grid.ReadOnly = True
            ButtonEliminarLinea.Enabled = False
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtPrestamosCabeza

        Dim Enlace As Binding

        If DtPrestamosCabeza.Rows(0).Item("Origen") = 1 Then
            RadioBancario.Checked = True
            Enlace = New Binding("SelectedValue", MiEnlazador, "Emisor")
            ComboBancos.DataBindings.Clear()
            ComboBancos.DataBindings.Add(Enlace)
            ComboSucursal.DataSource = ArmaSucursalesDeBanco(DtPrestamosCabeza.Rows(0).Item("Emisor"))
        End If
        If DtPrestamosCabeza.Rows(0).Item("Origen") = 2 Then
            RadioProveedor.Checked = True
            Enlace = New Binding("SelectedValue", MiEnlazador, "Emisor")
            ComboEmisor.DataBindings.Clear()
            ComboEmisor.DataBindings.Add(Enlace)
        End If
        If DtPrestamosCabeza.Rows(0).Item("Origen") = 3 Then
            RadioCliente.Checked = True
            Enlace = New Binding("SelectedValue", MiEnlazador, "Emisor")
            ComboEmisor.DataBindings.Clear()
            ComboEmisor.DataBindings.Add(Enlace)
        End If

        Enlace = New Binding("Text", MiEnlazador, "Prestamo")
        AddHandler Enlace.Format, AddressOf FormatPrestamo
        TextPrestamo.DataBindings.Clear()
        TextPrestamo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        CapitalW = DtPrestamosCabeza.Rows(0).Item("Capital")

        Enlace = New Binding("SelectedValue", MiEnlazador, "Sucursal")
        ComboSucursal.DataBindings.Clear()
        ComboSucursal.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaOtorgado")
        DateTime2.DataBindings.Clear()
        DateTime2.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Capital")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextCapital.DataBindings.Clear()
        TextCapital.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Interes")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextInteres.DataBindings.Clear()
        TextInteres.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cuotas")
        AddHandler Enlace.Format, AddressOf FormatEntero
        TextCuotas.DataBindings.Clear()
        TextCuotas.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NumeracionTercero")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextNumeracionTercero.DataBindings.Clear()
        TextNumeracionTercero.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatPrestamo(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = Format(Numero.Value, "0000-00000000")
        End If

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
    Private Sub ActualizaArchivos(ByRef DtPrestamoCabezaAux As DataTable, ByRef DtDetallePagoAux As DataTable)

        Dim RowsBusqueda() As DataRow

        'Actualizo Cabeza.
        Dim Origen As Integer
        If RadioBancario.Checked Then Origen = 1
        If RadioProveedor.Checked Then Origen = 2
        If RadioCliente.Checked Then Origen = 3

        If DtPrestamoCabezaAux.Rows(0).Item("Origen") <> Origen Then
            DtPrestamoCabezaAux.Rows(0).Item("Origen") = Origen
            If Origen = 2 Or Origen = 3 Then
                DtPrestamoCabezaAux.Rows(0).Item("Emisor") = ComboEmisor.SelectedValue
            Else : DtPrestamoCabezaAux.Rows(0).Item("Emisor") = ComboBancos.SelectedValue
            End If
        End If

        'Actualizo MedioPago.
        For Each Row As DataRow In DtGrid.Rows
            If Row.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtPrestamosDetallePago.Select("Item = " & Row("Item"))
                If RowsBusqueda.Length = 0 Then
                    Dim Row1 As DataRow = DtDetallePagoAux.NewRow
                    Row1("Prestamo") = PPrestamo
                    Row1("Item") = 0
                    Row1("MedioPago") = Row("MedioPago")
                    Row1("Cambio") = Row("Cambio")
                    Row1("Importe") = Row("Importe")
                    Row1("Banco") = Row("Banco")
                    Row1("Cuenta") = Row("Cuenta")
                    Row1("Comprobante") = Row("Comprobante")
                    Row1("FechaComprobante") = Row("FechaComprobante")
                    Row1("ClaveCheque") = 0
                    DtDetallePagoAux.Rows.Add(Row1)
                End If
            End If
        Next
        For Each Row As DataRow In DtDetallePagoAux.Rows
            If Row.RowState <> DataRowState.Added Then
                RowsBusqueda = DtGrid.Select("Item = " & Row("Item"))
                If RowsBusqueda.Length = 0 Then Row.Delete()
            End If
        Next

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = CreaDtParaGrid()

    End Sub
    Private Function ArmaSucursalesDeBanco(ByVal Banco) As DataTable

        Dim Dt As New DataTable
        Dim Row1 As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        Dt.Columns.Add(Nombre)

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtSucursales.Select("Banco = " & Banco)
        For Each Row As DataRow In RowsBusqueda
            Row1 = Dt.NewRow
            Row1("Clave") = Row("Sucursal")
            Row1("Nombre") = Row("Nombre")
            Dt.Rows.Add(Row1)
        Next
        Row1 = Dt.NewRow
        Row1("Clave") = 0
        Row1("Nombre") = ""
        Dt.Rows.Add(Row1)

        Return Dt

    End Function
    Private Function HacerAlta(ByVal DtPrestamosCabezaAux As DataTable, ByVal DtPrestamosDetallePagoAux As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable) As Boolean

        Dim NumeroMovi As Double
        Dim Resul As Double
        Dim NumeroAsiento As Double

        If PAbierto Then
            ConexionPrestamo = Conexion
        Else : ConexionPrestamo = ConexionN
        End If

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            NumeroMovi = UltimaNumeracion(ConexionPrestamo)
            If NumeroMovi < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            DtPrestamosCabezaAux.Rows(0).Item("Prestamo") = NumeroMovi
            For Each Row As DataRow In DtPrestamosDetallePagoAux.Rows
                Row("Prestamo") = NumeroMovi
            Next

            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionPrestamo)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = NumeroMovi
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            Resul = ActualizaMovi("A", DtPrestamosCabezaAux, DtPrestamosDetallePagoAux, ConexionPrestamo, DtAsientoCabeza, DtAsientoDetalle)

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
            PPrestamo = NumeroMovi
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function HacerModificacion(ByVal DtPrestamosCabezaAux, ByVal DtPrestamosDetallePagoAux, ByVal DtAsientoCabeza, ByVal DtAsientoDetalle) As Boolean

        Dim NumeroAsiento As Double
        Dim Resul As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionPrestamo)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = PPrestamo
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If
            '
            Resul = ActualizaMovi("M", DtPrestamosCabezaAux, DtPrestamosDetallePagoAux, ConexionPrestamo, DtAsientoCabeza, DtAsientoDetalle)
            '
            If Resul >= 0 Then Exit For
            If Resul = -3 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -1 And resul <> -3 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PActualizacionOk = True
            Return True
        End If

    End Function
    Private Function ActualizaMovi(ByVal Funcion As String, ByVal DtCabezaAux As DataTable, ByVal DtDetalleAux As DataTable, ByVal ConexionStr As String, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable) As Double

        Dim Resul As Double
        Dim RowsBusqueda() As DataRow

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaAux.GetChanges, "PrestamosCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If
                '
                For I As Integer = 0 To DtDetalleAux.Rows.Count - 1
                    Dim Row As DataRow = DtDetalleAux.Rows(I)
                    If Row.RowState = DataRowState.Added Then
                        If Row("MedioPago") = 3 Then
                            Row("ClaveCheque") = ActualizaClavesComprobantes("A", Row("ClaveCheque"), 1000, Row("MedioPago"), 0, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), DtGrid.Rows(I).Item("EmisorCheque"), DtCabezaAux.Rows(0).Item("Prestamo"), "01/01/1800", ConexionStr, False, False, DtGrid.Rows(I).Item("eCheq"))
                            If Row("ClaveCheque") <= 0 Then
                                MsgBox("Cheque " & DtGrid.Rows(0).Item("Serie") & " " & DtGrid.Rows(0).Item("Numero") & " ya fue Emitido.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            End If
                        End If
                    End If
                Next
                '                For Each Row As DataRow In DtDetalleAux.Rows
                '    If Row.RowState = DataRowState.Added Then
                '  If Row("MedioPago") = 3 Then
                ' CambiarPuntoDecimal(".")
                '  RowsBusqueda = DtGrid.Select("MedioPago = " & Row("MedioPago") & " AND Banco = " & Row("Banco") & " AND Cuenta = " & Row("Cuenta") & " AND Importe = " & Row("Importe") & " AND ClaveCheque = " & Row("ClaveCheque"))
                '   CambiarPuntoDecimal(",")
                '    Row("ClaveCheque") = ActualizaClavesComprobantes("A", Row("ClaveCheque"), 1000, Row("MedioPago"), 0, Row("Banco"), Row("Cuenta"), RowsBusqueda(0).Item("Serie"), RowsBusqueda(0).Item("Numero"), RowsBusqueda(0).Item("Fecha"), Row("Importe"), RowsBusqueda(0).Item("EmisorCheque"), DtCabezaAux.Rows(0).Item("Prestamo"), "01/01/1800", ConexionStr, False)
                '    If Row("ClaveCheque") <= 0 Then
                '  MsgBox("Cheque " & RowsBusqueda(0).Item("Serie") & " " & RowsBusqueda(0).Item("Numero") & " ya fue Emitido.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                '  Return -3
                '  End If
                '  End If
                '  End If
                ' Next
                '
                If Funcion <> "B" Then
                    For Each Row As DataRow In DtPrestamosDetallePago.Rows
                        If Row("MedioPago") = 3 Then
                            RowsBusqueda = DtDetalleAux.Select("ClaveCheque = " & Row("ClaveCheque"))
                            If RowsBusqueda.Length = 0 Then
                                If ActualizaClavesComprobantes("B", Row("ClaveCheque"), 1000, Row("MedioPago"), 0, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtCabezaAux.Rows(0).Item("Prestamo"), "01/01/1800", ConexionStr, False, False, False) <= 0 Then
                                    MsgBox("Cheque " & Row("ClaveCheque") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                    Return -3
                                End If
                            End If
                        End If
                    Next
                End If
                '
                If Funcion = "B" Then
                    For Each Row As DataRow In DtDetalleAux.Rows
                        If Row("MedioPago") = 3 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), 1000, Row("MedioPago"), 0, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtCabezaAux.Rows(0).Item("Prestamo"), "01/01/1800", ConexionStr, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("ClaveCheque") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            End If
                        End If
                    Next
                End If
                '
                If Not IsNothing(DtDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleAux.GetChanges, "PrestamosDetalle", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Concepto.DataSource = DtFormasPago
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

        Banco.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 order By Nombre;")
        Row = Banco.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Banco.DataSource.Rows.Add(Row)
        Banco.DisplayMember = "Nombre"
        Banco.ValueMember = "Clave"

    End Sub
    Private Function HallaTipo(ByVal MedioPago As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtFormasPago.Select("Clave = " & MedioPago)
        Return RowsBusqueda(0).Item("Tipo")

    End Function
    Private Sub ArmaDtSucursales()

        DtSucursales = New DataTable

        If Not Tablas.Read("SELECT Banco,Sucursal,NombreSucursal AS Nombre FROM CuentasBancarias;", Conexion, DtSucursales) Then End

    End Sub
    Private Sub CalculaTotales()

        TotalConceptos = 0
        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Cambio").Value = 0 Then
                TotalConceptos = TotalConceptos + Row.Cells("Importe").Value
            Else : TotalConceptos = TotalConceptos + Row.Cells("Cambio").Value * Row.Cells("Importe").Value
            End If
        Next
        TextTotalPrestamo.Text = FormatNumber(TotalConceptos, GDecimales)

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtDetallePagoAnt As DataTable, ByVal DtDetallePagoAct As DataTable, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos
        Dim RowsBusqueda() As DataRow

        Dim ImporteTotal As Double

        For Each Row As DataRow In DtDetallePagoAct.Rows
            If Row.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtDetallePagoAnt.Select("Item = " & Row("Item"))
                If RowsBusqueda.Length = 0 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("MedioPago")
                    Item.Importe = Row("Importe")
                    If Row("Cambio") <> 0 Then Item.Importe = Trunca(Item.Importe * Row("Cambio"))
                    ImporteTotal = ImporteTotal + Item.Importe
                    ListaConceptos.Add(Item)
                End If
            End If
        Next
        '
        For Each Row As DataRow In DtDetallePagoAnt.Rows
            RowsBusqueda = DtDetallePagoAct.Select("Item = " & Row("Item"))
            If RowsBusqueda.Length = 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = Row("MedioPago")
                Item.Importe = -Row("Importe")
                If Row("Cambio") <> 0 Then Item.Importe = Trunca(Item.Importe * Row("Cambio"))
                ImporteTotal = ImporteTotal + Item.Importe
                ListaConceptos.Add(Item)
            End If
        Next

        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = ImporteTotal
        ListaConceptos.Add(Item)
        '
        If Not Asiento(8000, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

        Return True

    End Function
    Public Function UltimaNumeracion(ByVal ConexionStr As String) As Double

        Dim Patron As String = GCaja & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Prestamo) FROM PrestamosCabeza WHERE CAST(CAST(PrestamosCabeza.Prestamo AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(GCaja & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return -1
        End Try

    End Function
    Private Function Valida() As Boolean

        If Not RadioBancario.Checked And Not RadioProveedor.Checked And Not RadioCliente.Checked Then
            MsgBox("Falta Informar Origen del Prestamo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            RadioBancario.Focus()
            Return False
        End If

        If TextCapital.Text = "" Then
            MsgBox("Falta Informar Capital.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCapital.Focus()
            Return False
        End If
        If Not IsNumeric(TextCapital.Text) Then
            MsgBox("Importe no Numérico.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCapital.Focus()
            Return False
        End If
        If CDbl(TextCapital.Text) = 0 Then
            MsgBox("Falta Informar Capital.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCapital.Focus()
            Return False
        End If

        If CapitalW <> CDbl(TextCapital.Text) And PPrestamo <> 0 Then
            Dim Cancelado As Double
            Dim InteresCancelado As Double
            Dim CapitalAjustado As Double
            Dim Gastos As Double
            Dim Dtmovimientos As DataTable
            If Not ProcesaPrestamo(False, CapitalAjustado, Cancelado, InteresCancelado, Gastos, Dtmovimientos, PPrestamo, CDbl(TextCapital.Text), DateTime1.Value, ComboEstado.SelectedValue, ConexionPrestamo) Then Return False
            If CapitalAjustado < Cancelado Then
                MsgBox("Capital es inferior al Importe Cancelado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextCapital.Focus()
                Return False
            End If
        End If

        If TextInteres.Text = "" Then
            MsgBox("Falta Informar Interés.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextInteres.Focus()
            Return False
        End If
        If CDbl(TextInteres.Text) = 0 Then
            MsgBox("Falta Informar Interés.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextInteres.Focus()
            Return False
        End If
        If Not IsNumeric(TextInteres.Text) Then
            MsgBox("Importe no Numérico.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextInteres.Focus()
            Return False
        End If

        If TextCuotas.Text = "" Then
            MsgBox("Falta Informar Cuotas.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCuotas.Focus()
            Return False
        End If
        If CInt(TextCuotas.Text) = 0 Then
            MsgBox("Falta Informar Cuotas.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCuotas.Focus()
            Return False
        End If

        If DiferenciaDias(DateTime1.Value, DateTime2.Value) > 0 Then
            MsgBox("Fecha Otrogado No debe ser mayor a Fecha Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime2.Focus()
            Return False
        End If

        If RadioBancario.Checked Then
            If ComboBancos.SelectedValue = 0 Then
                MsgBox("Falta Informar Banco.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboBancos.Focus()
                Return False
            End If
            If ComboSucursal.SelectedValue = 0 Then
                MsgBox("Falta Informar Sucursal.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboSucursal.Focus()
                Return False
            End If
        End If

        If RadioProveedor.Checked Then
            If ComboEmisor.SelectedValue = 0 Then
                MsgBox("Falta Informar Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Return False
            End If
        End If

        If RadioCliente.Checked Then
            If ComboEmisor.SelectedValue = 0 Then
                MsgBox("Falta Informar Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Return False
            End If
        End If

        If Not PSaldoInicial Then
            If Grid.Rows.Count = 1 Then
                MsgBox("Falta Informar Medio de Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Return False
            End If
            If CDbl(TextCapital.Text) <> CDbl(TextTotalPrestamo.Text) Then
                MsgBox("Capital Prestado no Coincide con Importe Informado en Medio de Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Return False
            End If
            Dim Row As DataGridViewRow
            For i As Integer = 0 To Grid.Rows.Count - 2
                Row = Grid.Rows(i)
                If RadioBancario.Checked Then
                    If Row.Cells("Concepto").Value = 3 Or Row.Cells("Concepto").Value = 2 Then
                        MsgBox("Medio de Pago Invalido para Prestamo Bancario en linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Grid.Focus()
                        Return False
                    End If
                End If
            Next
            If Not ConsistePagos(Grid, DtFormasPago, 60, False) Then Return False
        End If

        Dim Opr As Boolean
        If RadioCliente.Checked Then
            Opr = HallaOPR(ComboEmisor.SelectedValue, "C")
        End If
        If RadioProveedor.Checked Then
            Opr = HallaOPR(ComboEmisor.SelectedValue, "P")
        End If
        If Not Opr And PAbierto And (RadioCliente.Checked Or RadioProveedor.Checked) Then
            If MsgBox("Cliente/Proveedor Solo Opera a Candado Cerrado. Debe Cambiar Candado en Cliente/Proveedor. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        Dim RowsBusqueda() As DataRow
        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Or Grid.Columns(e.ColumnIndex).Name = "Banco" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
                If cb.SelectedIndex < 0 Then cb.SelectedValue = 0
            Else
                Exit Sub
            End If

            If IsNothing(cb.SelectedValue) Then Exit Sub

            If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
                If cb.SelectedIndex <= 0 Then Exit Sub
                RowsBusqueda = DtFormasPago.Select("Clave = " & cb.SelectedValue)
                ArmaGridSegunConcepto(Grid.Rows(e.RowIndex), RowsBusqueda(0).Item("Tipo"), 60, False, False, PAbierto)
            End If
        End If

    End Sub
    Private Sub Grid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.CellBeginEdit
        'eCheq.
        If Grid.Columns(e.ColumnIndex).Name = "eCheq" Then
            If Grid.Rows(e.RowIndex).Cells("Concepto").Value <> 3 Then e.Cancel = True
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        If Grid.Columns(e.ColumnIndex).Name = "Neto" Or Grid.Columns(e.ColumnIndex).Name = "Alicuota" Or _
           Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "Cambio" Then
            If IsDBNull(Grid.CurrentCell.ToString) Then Grid.CurrentCell.Value = 0
            CalculaTotales()
        End If

    End Sub
    Private Sub Grid_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellValueChanged

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Concepto")) Then
                Grid.CurrentRow.Cells("Neto").Value = 0
                Grid.CurrentRow.Cells("Alicuota").Value = 0
                Grid.CurrentRow.Cells("Iva").Value = 0
                Grid.CurrentRow.Cells("Importe").Value = 0
                Grid.CurrentRow.Cells("Banco").Value = 0
                Grid.CurrentRow.Cells("Cuenta").Value = 0
                Grid.CurrentRow.Cells("Serie").Value = ""
                Grid.CurrentRow.Cells("Numero").Value = 0
                Grid.CurrentRow.Cells("Fecha").Value = "1/1/1800"
                Grid.CurrentRow.Cells("Cambio").Value = 0
                Grid.CurrentRow.Cells("EmisorCheque").Value = ""
                Grid.CurrentRow.Cells("ClaveCheque").Value = 0
                Grid.CurrentRow.Cells("Comprobante").Value = 0
                Grid.CurrentRow.Cells("FechaComprobante").Value = "1/1/1800"
                Grid.CurrentRow.Cells("eCheq").Value = False
                Grid.Refresh()
                CalculaTotales()
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Banco" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Banco")) Then
                Grid.CurrentRow.Cells("Cuenta").Value = 0
                Grid.Refresh()
            End If
        End If

    End Sub
    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If ComboEstado.SelectedValue = 3 Or DtPrestamosCabeza.Rows(0).Item("Estado") = 2 Then Exit Sub

        If Grid.Rows(e.RowIndex).Cells("Concepto").ReadOnly Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta" Then
            If IsDBNull(Grid.CurrentRow.Cells("Concepto").Value) Then Exit Sub
            If HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 7 Then
                ListaBancos.PEsSeleccionaCuenta = True
                ListaBancos.PEsSoloPesos = True
                ListaBancos.ShowDialog()
                If ListaBancos.PCuenta <> 0 Then
                    Grid.CurrentRow.Cells("Banco").Value = ListaBancos.PBanco
                    Grid.CurrentRow.Cells("Cuenta").Value = ListaBancos.PCuenta
                    Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Comprobante")
                End If
                ListaBancos.Dispose()
            End If
            '
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Grid.Rows(e.RowIndex).Cells("Concepto").Value = 3 Then
                Calendario.ShowDialog()
                Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                Grid.EndEdit()
                Calendario.Dispose()
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FechaComprobante" Then
            If HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 7 Or HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 4 Then
                Calendario.ShowDialog()
                Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                Grid.EndEdit()
                Calendario.Dispose()
            End If
            If HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 10 Then
                Calendario.ShowDialog()
                Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                Grid.EndEdit()
                Calendario.Dispose()
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "Neto" Or _
           Grid.Columns(e.ColumnIndex).Name = "Alicuota" Or Grid.Columns(e.ColumnIndex).Name = "Iva" Or Grid.Columns(e.ColumnIndex).Name = "Cambio" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaComprobante" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = "1/1/1800" Then e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Or Grid.Columns(e.ColumnIndex).Name = "Comprobante" Or Grid.Columns(e.ColumnIndex).Name = "Cuenta" Or Grid.Columns(e.ColumnIndex).Name = "ClaveCheque" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, 0)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta" Then
            e.Value = Nothing
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
            Exit Sub
        End If

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Detalle" Then Exit Sub
        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Banco" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Not Grid.Columns(Grid.CurrentCell.ColumnIndex).Name = "Concepto" Then
            If IsDBNull(Grid.CurrentRow.Cells("Concepto").Value) Then
                e.KeyChar = ""
                Exit Sub
            End If
            If Grid.CurrentRow.Cells("Concepto").Value = 0 Then
                e.KeyChar = ""
                Exit Sub
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Neto" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Alicuota" Then
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cambio" Then
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Numero" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Comprobante" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cuenta" Then
            If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Serie" Then
            If e.KeyChar = "" Then Exit Sub
            e.KeyChar = e.KeyChar.ToString.ToUpper
            If Asc(e.KeyChar) < 65 Or Asc(e.KeyChar) > 90 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Neto" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Alicuota" Or _
           Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cambio" Then
            If CType(sender, TextBox).Text <> "" Then
                If Not IsNumeric(CType(sender, TextBox).Text) Then
                    CType(sender, TextBox).Text = ""
                    CType(sender, TextBox).Focus()
                End If
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row

        Row("Item") = 0
        Row("MedioPago") = 0
        Row("Detalle") = ""
        Row("Neto") = 0
        Row("Alicuota") = 0
        Row("ImporteIva") = 0
        Row("Banco") = 0
        Row("Fecha") = "1/1/1800"
        Row("Cuenta") = 0
        Row("Serie") = ""
        Row("Numero") = 0
        Row("EmisorCheque") = ""
        Row("Cambio") = 0
        Row("Importe") = 0
        Row("Comprobante") = 0
        Row("FechaComprobante") = "1/1/1800"
        Row("ClaveCheque") = 0
        Row("eCheq") = False

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Cuenta") Then
            If IsDBNull(e.Row("Cuenta")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Numero") Then
            If IsDBNull(e.Row("Numero")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Cambio") Then
            If IsDBNull(e.Row("Cambio")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

        If e.Column.ColumnName.Equals("Importe") Then
            If IsDBNull(e.Row("Importe")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

        If (e.Column.ColumnName.Equals("Comprobante")) Then
            If IsDBNull(e.Row("Comprobante")) Then Exit Sub
            If IsNothing(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If (e.Column.ColumnName.Equals("EmisorCheque")) Then
            If IsDBNull(e.Row("EmisorCheque")) Then Exit Sub
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
        End If

        CalculaTotales()

    End Sub

  
End Class