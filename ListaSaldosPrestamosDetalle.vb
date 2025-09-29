Public Class ListaSaldosPrestamosDetalle
    Public PBloqueaFunciones As Boolean
    '
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim DtTrabajo As DataTable
    Dim DtSucursales As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub UnPrestamoMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaCombosGrid()

        CreaDtGrid()
        DtTrabajo = DtGrid.Clone

        ArmaDtSucursales()

        LlenaCombo(ComboCliente, "", "Clientes")
        ComboCliente.SelectedValue = 0
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboProveedor, "", "Proveedores")
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboBancos, 26)
        ComboBancos.SelectedValue = 0
        With ComboBancos
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

        DateTimeDesde.Value = DateTimeHasta.Value.AddMonths(-3)

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar.Focus() : ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        Dim SqlFecha As String
        SqlFecha = "FechaOtorgado >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND FechaOtorgado < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlProveedor As String = ""
        If ComboProveedor.SelectedValue <> 0 Then
            SqlProveedor = " AND Origen = 2 AND Emisor = " & ComboProveedor.SelectedValue
        End If

        Dim SqlCliente As String = ""
        If ComboCliente.SelectedValue <> 0 Then
            SqlCliente = " AND Origen = 3 AND Emisor = " & ComboCliente.SelectedValue
        End If

        Dim SqlBanco As String = ""
        If ComboBancos.SelectedValue <> 0 Then
            SqlBanco = " AND Origen = 1 AND Emisor = " & ComboBancos.SelectedValue
        End If

        Dim SqlPrestamo As String = ""
        If Val(MaskedPrestamo.Text) <> 0 Then
            SqlPrestamo = " AND Prestamo = " & Val(MaskedPrestamo.Text)
        End If

        Dim SqlSucursal As String = ""
        If ComboSucursal.SelectedValue <> 0 Then
            SqlSucursal = "AND Origen = 1 AND Banco = " & ComboBancos.SelectedValue & " AND Sucursal = " & ComboSucursal.SelectedValue
        End If

        Dim SqlEstado As String = ""
        If Not CheckConAnuladas.Checked Then
            SqlEstado = " AND Estado <> 3"
        End If

        Dim StrCaja As String
        If Not GCajaTotal Then
            StrCaja = " AND Caja = " & GCaja
        End If

        SqlB = "SELECT 1 as Operacion,Capital,Origen,Emisor,Sucursal,Prestamo,FechaOtorgado AS Fecha,Estado FROM PrestamosCabeza WHERE " & SqlFecha & SqlProveedor & SqlCliente & SqlBanco & SqlPrestamo & SqlSucursal & SqlEstado & StrCaja & ";"
        SqlN = "SELECT 2 as Operacion,Capital,Origen,Emisor,Sucursal,Prestamo,FechaOtorgado AS Fecha,Estado FROM PrestamosCabeza WHERE " & SqlFecha & SqlProveedor & SqlCliente & SqlBanco & SqlPrestamo & SqlSucursal & SqlEstado & StrCaja & ";"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

    End Sub
    Private Sub ComboBancos_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboBancos.Validating

        If IsNothing(ComboBancos.SelectedValue) Then ComboBancos.SelectedValue = 0
        If ComboBancos.SelectedValue = 0 Then
            ComboSucursal.DataSource = ArmaSucursalesDeBanco(0)
            ComboSucursal.SelectedValue = 0
            Exit Sub
        End If

        ComboSucursal.DataSource = ArmaSucursalesDeBanco(ComboBancos.SelectedValue)
        ComboSucursal.DisplayMember = "Nombre"
        ComboSucursal.ValueMember = "Clave"
        ComboSucursal.SelectedValue = 0

    End Sub
    Private Sub ButtonUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUltimo.Click

        bs.MoveLast()

    End Sub
    Private Sub ButtonAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnterior.Click

        bs.MovePrevious()

    End Sub
    Private Sub ButtonPosterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPosterior.Click

        bs.MoveNext()

    End Sub
    Private Sub ButtonPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPrimero.Click

        bs.MoveFirst()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Saldos Prestamos", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LLenaGrid()

        DtGrid.Clear()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If
        If Not CheckCancelados.Checked And Not CheckPendientes.Checked Then
            CheckCancelados.Checked = True
            CheckPendientes.Checked = True
        End If

        Dim Dt As New DataTable

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked = True Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim CuentaAnt As Double = 0
        Dim OperacionAnt As Integer = 0
        Dim TotSaldoCuenta As Double = 0
        Dim Saldo As Double = 0
        Dim TotDebitoCuenta As Double = 0
        Dim TotCreditoCuenta As Double = 0
        Dim ConexionStr As String = ""
        Dim Row1 As DataRow
        Dim ImporteCancelado As Double
        Dim InteresCancelado As Double
        Dim CapitalAjustado As Double
        Dim Gastos As Double
        Dim TotalSaldo As Double

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Prestamo,Sucursal"

        For I As Integer = 0 To View.Count - 1
            Dim Row As DataRowView = View(I)
            If Row("Operacion") = 1 Then
                ConexionStr = Conexion
            Else : ConexionStr = ConexionN
            End If
            '
            If Not ProcesaPrestamo(True, CapitalAjustado, ImporteCancelado, InteresCancelado, Gastos, DtTrabajo, Row("Prestamo"), Row("Capital"), Row("Fecha"), Row("Estado"), ConexionStr) Then Me.Close() : Exit Sub
            '
            If (CheckPendientes.Checked And Not CheckCancelados.Checked And (CapitalAjustado - ImporteCancelado) > 0 And Row("Estado") <> 3) Or _
               (Not CheckPendientes.Checked And CheckCancelados.Checked And (CapitalAjustado - ImporteCancelado) <= 0 And Row("Estado") <> 3) Or _
               (CheckPendientes.Checked And CheckCancelados.Checked) Then
                Saldo = 0
                If Row("Sucursal") <> CuentaAnt Then
                    If CuentaAnt <> 0 Then
                        CortePorCuenta(TotDebitoCuenta, TotCreditoCuenta, TotSaldoCuenta)
                    End If
                    CuentaAnt = Row("Sucursal")
                    TotDebitoCuenta = 0 : TotCreditoCuenta = 0 : TotSaldoCuenta = 0
                End If
                Row1 = DtGrid.NewRow
                Row1("Operacion") = Row("Operacion")
                Row1("Prestamo") = Row("Prestamo")
                Row1("Sucursal") = ""
                If Row("Origen") = 1 Then
                    Row1("Emisor") = "Bco. " & NombreBanco(Row("Emisor"))
                    Row1("Sucursal") = NombreSucursal(Row("Emisor"), Row("Sucursal"))
                End If
                If Row("Origen") = 2 Then Row1("Emisor") = NombreProveedor(Row("Emisor"))
                If Row("Origen") = 3 Then Row1("Emisor") = NombreCliente(Row("Emisor"))
                Row1("Color") = 0
                Row1("TipoNota") = 1000
                Row1("Movimiento") = 0
                Row1("Concepto") = 1
                Row1("Debito") = 0
                Row1("Credito") = Row("Capital")
                Row1("Fecha") = Format(Row("Fecha"), "dd/MM/yyyy 00:00:00")
                Row1("Estado") = 0
                If Row("Estado") <> 1 Then Row1("Estado") = Row("Estado")
                If Row("Estado") <> 3 Then
                    Saldo = Saldo + Row1("Debito") - Row1("Credito")
                    TotDebitoCuenta = TotDebitoCuenta + Row1("Debito")
                    TotCreditoCuenta = TotCreditoCuenta + Row1("Credito")
                    TotSaldoCuenta = TotSaldoCuenta + Saldo
                    Row1("Saldo") = Saldo
                Else
                    Row1("Saldo") = Saldo
                End If
                Row1("MedioPagoRechazado") = 0
                Row1("ChequeRechazado") = 0
                DtGrid.Rows.Add(Row1)
                '
                If CheckRepetirLineas.Checked Then
                    AddGrid(DtTrabajo, Row("Operacion"), Saldo, TotDebitoCuenta, TotCreditoCuenta, TotSaldoCuenta, Row1("Sucursal"), Row1("Prestamo"), Row1("Emisor"))
                Else
                    AddGrid(DtTrabajo, Row("Operacion"), Saldo, TotDebitoCuenta, TotCreditoCuenta, TotSaldoCuenta, "", 0, "")
                End If
                '
                TotalSaldo = TotalSaldo + Saldo
            End If
        Next
        If CuentaAnt <> 0 Then
            CortePorCuenta(TotDebitoCuenta, TotCreditoCuenta, TotSaldoCuenta)
        End If
        Row1 = DtGrid.NewRow
        Row1("Color") = 1
        Row1("Operacion") = 0
        Row1("Sucursal") = ""
        Row1("Prestamo") = 0
        Row1("Emisor") = ""
        Row1("Movimiento") = 0
        Row1("Concepto") = 0
        Row1("Debito") = 0
        Row1("Credito") = 0
        Row1("Fecha") = "01/01/1800"
        Row1("Estado") = 0
        Row1("Saldo") = TotalSaldo
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

    End Sub
    Private Sub AddGrid(ByVal DtTrabajo As DataTable, ByVal Operacion As Integer, ByRef Saldo As Double, ByRef TotDebitoCuenta As Double, ByRef TotCreditoCuenta As Double, ByRef TotSaldoCuenta As Double, ByVal Sucursal As String, ByVal Prestamo As Double, ByVal Emisor As String)

        Dim Row1 As DataRow

        For Y As Integer = 0 To DtTrabajo.Rows.Count - 1
            If (Not CheckConAnuladas.Checked And DtTrabajo.Rows(Y).Item("Estado") <> 3) Or CheckConAnuladas.Checked Then
                Row1 = DtGrid.NewRow
                Row1("Operacion") = Operacion
                Row1("Sucursal") = Sucursal
                Row1("Prestamo") = Prestamo
                Row1("Emisor") = Emisor
                Row1("Color") = 0
                Row1("TipoNota") = DtTrabajo.Rows(Y).Item("TipoNota")
                Row1("Movimiento") = DtTrabajo.Rows(Y).Item("Movimiento")
                Row1("Concepto") = DtTrabajo.Rows(Y).Item("Concepto")
                Row1("Debito") = DtTrabajo.Rows(Y).Item("Debito")
                Row1("Credito") = DtTrabajo.Rows(Y).Item("Credito")
                Row1("Fecha") = Format(DtTrabajo.Rows(Y).Item("Fecha"), "dd/MM/yyyy 00:00:00")
                Row1("Estado") = 0
                If DtTrabajo.Rows(Y).Item("Estado") <> 1 Then Row1("Estado") = DtTrabajo.Rows(Y).Item("Estado")
                If Row1("Estado") = 0 And (Row1("Concepto") = 2 Or Row1("Concepto") = 6 Or Row1("Concepto") = 93) Then
                    Saldo = Saldo + Row1("Debito") - Row1("Credito")
                    TotDebitoCuenta = TotDebitoCuenta + Row1("Debito")
                    TotCreditoCuenta = TotCreditoCuenta + Row1("Credito")
                    TotSaldoCuenta = TotSaldoCuenta + Saldo
                    Row1("Saldo") = Saldo
                Else
                    Row1("Saldo") = Saldo
                End If
                Row1("MedioPagoRechazado") = DtTrabajo.Rows(Y).Item("MedioPagoRechazado")
                Row1("ChequeRechazado") = DtTrabajo.Rows(Y).Item("ChequeRechazado")
                DtGrid.Rows.Add(Row1)
            End If
        Next

    End Sub
    Private Sub CortePorCuenta(ByVal Debito As Double, ByVal Credito As Double, ByVal Saldo As Double)

        If Val(MaskedPrestamo.Text) <> 0 Then Exit Sub

        Dim Row1 As DataRow = DtGrid.NewRow
        Row1("Color") = 1
        Row1("Operacion") = 0
        Row1("Sucursal") = ""
        Row1("Prestamo") = 0
        Row1("Emisor") = "TOTAL SUCURSAL"
        Row1("Movimiento") = 0
        Row1("Concepto") = 0
        Row1("Debito") = Debito
        Row1("Credito") = Credito
        Row1("Fecha") = "01/01/1800"
        Row1("Estado") = 0
        Row1("Saldo") = Debito - Credito
        DtGrid.Rows.Add(Row1)

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Color As New DataColumn("Color")
        Color.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Color)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Sucursal As New DataColumn("Sucursal")
        Sucursal.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Sucursal)

        Dim Prestamo As New DataColumn("Prestamo")
        Prestamo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Prestamo)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Concepto)

        Dim TipoNota As New DataColumn("TipoNota")
        TipoNota.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoNota)

        Dim Movimiento As New DataColumn("Movimiento")
        Movimiento.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Movimiento)

        Dim Emisor As New DataColumn("Emisor")
        Emisor.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Emisor)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Debito As New DataColumn("Debito")
        Debito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Debito)

        Dim Credito As New DataColumn("Credito")
        Credito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Credito)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Saldo)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim MedioPagoRechazado As New DataColumn("MedioPagoRechazado")
        MedioPagoRechazado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(MedioPagoRechazado)

        Dim ChequeRechazado As New DataColumn("ChequeRechazado")
        ChequeRechazado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ChequeRechazado)

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
    Private Function NombreSucursal(ByVal Banco As Integer, ByVal Sucursal As Integer) As String

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtSucursales.Select("Banco = " & Banco & " AND Sucursal = " & Sucursal)
        Return RowsBusqueda(0).Item("Nombre")

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Concepto.DataSource = ArmaTodosConceptosPrestamo(True)
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

        TipoNota.DataSource = ArmaTodosDocumentosPrestamo(True)
        TipoNota.DisplayMember = "Nombre"
        TipoNota.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Row = Estado.DataSource.NewRow()
        Row("Clave") = 2
        Row("Nombre") = "Suspendido"
        Estado.DataSource.Rows.Add(Row)
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    Private Sub ArmaDtSucursales()

        DtSucursales = New DataTable

        If Not Tablas.Read("SELECT Banco,Sucursal,NombreSucursal AS Nombre FROM CuentasBancarias;", Conexion, DtSucursales) Then End

    End Sub
    Private Function Valida() As Boolean
        Return True
    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Prestamo" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else : e.Value = NumeroEditado(e.Value)
            End If
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cuenta" Or Grid.Columns(e.ColumnIndex).Name = "Movimiento" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, 0)
            Else : e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If e.Value = "01/01/1800" Then
                e.Value = ""
            Else
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Debito" Or Grid.Columns(e.ColumnIndex).Name = "Credito" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If IsDBNull(Grid.CurrentRow.Cells("Prestamo").Value) Then Exit Sub

        Dim Abierto As Boolean
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Prestamo" Then
            If Grid.CurrentRow.Cells("Prestamo").Value = 0 Then Exit Sub
            UnPrestamo.PPrestamo = Grid.CurrentRow.Cells("Prestamo").Value
            UnPrestamo.PAbierto = Abierto
            UnPrestamo.ShowDialog()
            If UnPrestamo.PActualizacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            UnPrestamo.Dispose()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Movimiento" Then
            If Grid.CurrentRow.Cells("Movimiento").Value = 0 Then Exit Sub
            If Grid.CurrentRow.Cells("TipoNota").Value = 1010 Or Grid.CurrentRow.Cells("TipoNota").Value = 1015 Then
                If EsReemplazoChequePrestamo(Grid.CurrentRow.Cells("Movimiento").Value, Abierto) Then
                    UnChequeReemplazo.PTipoNota = Grid.CurrentRow.Cells("TipoNota").Value
                    UnChequeReemplazo.PNota = Grid.CurrentRow.Cells("Movimiento").Value
                    UnChequeReemplazo.PAbierto = Abierto
                    UnChequeReemplazo.PBloqueaFunciones = PBloqueaFunciones
                    UnChequeReemplazo.ShowDialog()
                    UnChequeReemplazo.Dispose()
                    If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
                    Exit Sub
                End If
                UnMovimientoPrestamo.PMovimiento = Grid.CurrentRow.Cells("Movimiento").Value
                UnMovimientoPrestamo.PAbierto = Abierto
                UnMovimientoPrestamo.PTipoNota = Grid.CurrentRow.Cells("TipoNota").Value
                UnMovimientoPrestamo.ShowDialog()
                UnMovimientoPrestamo.Dispose()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            Else
                UnReciboDebitoCreditoGenerica.PNota = Grid.CurrentRow.Cells("Movimiento").Value
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.PTipoNota = Grid.CurrentRow.Cells("TipoNota").Value
                UnReciboDebitoCreditoGenerica.ShowDialog()
                UnReciboDebitoCreditoGenerica.Dispose()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            End If
        End If

    End Sub



End Class