Public Class ListaPrestamos
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim DtSucursales As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaPrestamos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaCombosGrid()

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

        MaskedPrestamo.Text = "000000000000"

        CreaDtGrid()
        ArmaDtSucursales()

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Checked = False
        Else
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar.Focus() : ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        Dim SqlFecha As String
        SqlFecha = "C.FechaOtorgado >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND C.FechaOtorgado < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlProveedor As String = ""
        If ComboProveedor.SelectedValue <> 0 Then
            SqlProveedor = " AND C.Origen = 2 AND C.Emisor = " & ComboProveedor.SelectedValue
        End If

        Dim SqlCliente As String = ""
        If ComboCliente.SelectedValue <> 0 Then
            SqlCliente = " AND C.Origen = 3 AND C.Emisor = " & ComboCliente.SelectedValue
        End If

        Dim SqlBanco As String = ""
        If ComboBancos.SelectedValue <> 0 Then
            SqlBanco = " AND C.Origen = 1 AND C.Emisor = " & ComboBancos.SelectedValue
        End If

        Dim SqlPrestamo As String = ""
        If Val(MaskedPrestamo.Text) <> 0 Then
            SqlPrestamo = " AND C.Prestamo = " & Val(MaskedPrestamo.Text)
        End If

        Dim SqlEstado As String = ""
        If Not CheckConAnuladas.Checked Then
            SqlEstado = " AND C.Estado <> 3"
        End If

        Dim StrCaja As String
        If Not GCajaTotal Then
            StrCaja = " AND Caja = " & GCaja
        End If

        SqlB = "SELECT 1 as Operacion,* FROM PrestamosCabeza AS C WHERE " & SqlFecha & SqlProveedor & SqlCliente & SqlBanco & SqlPrestamo & SqlEstado & StrCaja & ";"
        SqlN = "SELECT 2 as Operacion,* FROM PrestamosCabeza AS C WHERE " & SqlFecha & SqlProveedor & SqlCliente & SqlBanco & SqlPrestamo & SqlEstado & StrCaja & ";"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()

        LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

    End Sub
    Private Sub ButtonCancelacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMovimientos.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        If Grid.CurrentRow.Cells("Estado").Value <> 0 Then
            MsgBox("Prestamo no se puede Cancelar.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Opcion As Integer

        OpcionTipoNota.PEsPrestamo = True
        OpcionTipoNota.ShowDialog()
        Opcion = OpcionTipoNota.Ptipo
        OpcionTipoNota.Dispose()

        If Opcion = 0 Then Exit Sub

        If Opcion = 1010 Or Opcion = 1015 Then
            UnMovimientoPrestamo.PPrestamo = Grid.CurrentRow.Cells("Prestamo").Value
            UnMovimientoPrestamo.PTipoNota = Opcion
            UnMovimientoPrestamo.PMovimiento = 0
            If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
                UnMovimientoPrestamo.PAbierto = True
            Else : UnMovimientoPrestamo.PAbierto = False
            End If
            UnMovimientoPrestamo.ShowDialog()
            UnMovimientoPrestamo.Dispose()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

    End Sub
    Private Sub ButtonListaSaldo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonListaSaldo.Click

        If Grid.Rows.Count = 0 Then Exit Sub
        ListaSaldosPrestamosDetalle.MaskedPrestamo.Text = Format(Grid.CurrentRow.Cells("Prestamo").Value, "0000-00000000")
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            ListaSaldosPrestamosDetalle.CheckAbierto.Checked = True
            ListaSaldosPrestamosDetalle.CheckCerrado.Checked = False
        Else : ListaSaldosPrestamosDetalle.CheckAbierto.Checked = False
            ListaSaldosPrestamosDetalle.CheckCerrado.Checked = True
        End If
        ListaSaldosPrestamosDetalle.CheckAbierto.Enabled = False
        ListaSaldosPrestamosDetalle.CheckCerrado.Enabled = False
        ListaSaldosPrestamosDetalle.CheckCancelados.Checked = True
        ListaSaldosPrestamosDetalle.CheckPendientes.Checked = True
        ListaSaldosPrestamosDetalle.ComboBancos.Enabled = False
        ListaSaldosPrestamosDetalle.ComboCliente.Enabled = False
        ListaSaldosPrestamosDetalle.ComboProveedor.Enabled = False
        ListaSaldosPrestamosDetalle.ComboSucursal.Enabled = False
        ListaSaldosPrestamosDetalle.MaskedPrestamo.Enabled = False
        ListaSaldosPrestamosDetalle.PBloqueaFunciones = True
        ListaSaldosPrestamosDetalle.ShowDialog()
        ListaSaldosPrestamosDetalle.Dispose()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Prestamos y Movimientos", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub TextPrestamo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0 : Exit Sub

        ComboCliente.SelectedValue = 0
        ComboBancos.SelectedValue = 0

    End Sub
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0 : Exit Sub

        ComboProveedor.SelectedValue = 0
        ComboBancos.SelectedValue = 0

    End Sub
    Private Sub ComboBancos_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboBancos.Validating

        If IsNothing(ComboBancos.SelectedValue) Then ComboBancos.SelectedValue = 0 : Exit Sub

        ComboProveedor.SelectedValue = 0
        ComboCliente.SelectedValue = 0

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

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Prestamo"

        Dim ConexionStr As String
        Dim CapitalAjustado As Double
        Dim DtMovimientos As DataTable

        For Each Row As DataRowView In View
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Prestamo") = Row("Prestamo")
            Row1("NumeracionTercero") = Row("NumeracionTercero")
            Row1("Sucursal") = ""
            If Row("Origen") = 1 Then
                Row1("Emisor") = "Bco. " & NombreBanco(Row("Emisor"))
                Row1("Sucursal") = NombreSucursal(Row("Emisor"), Row("Sucursal"))
            End If
            If Row("Origen") = 2 Then Row1("Emisor") = NombreProveedor(Row("Emisor"))
            If Row("Origen") = 3 Then Row1("Emisor") = NombreCliente(Row("Emisor"))

            Row1("Fecha") = Format(Row("FechaOtorgado"), "dd/MM/yyyy 00:00:00")
            If Row("Operacion") = 1 Then
                ConexionStr = Conexion
            Else : ConexionStr = ConexionN
            End If
            Row1("Cancelado") = 0
            Row1("InteresCancelado") = 0
            Row1("Gastos") = 0
            '
            If Not ProcesaPrestamo(False, CapitalAjustado, Row1("Cancelado"), Row1("InteresCancelado"), Row1("Gastos"), DtMovimientos, Row("Prestamo"), Row("Capital"), Row("FechaOtorgado"), Row("Estado"), ConexionStr) Then Me.Close() : Exit Sub
            '
            Row1("Capital") = CapitalAjustado
            Row1("Saldo") = CapitalAjustado - Row1("Cancelado")
            Row1("Estado") = 0
            If Row("Estado") <> 1 Then Row1("Estado") = Row("Estado")
            If (CheckPendientes.Checked And Row1("Saldo") <> 0) Or (CheckCancelados.Checked And Row1("Saldo") = 0) Or (CheckPendientes.Checked And CheckCancelados.Checked) Then
                DtGrid.Rows.Add(Row1)
            End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Prestamo As New DataColumn("Prestamo")
        Prestamo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Prestamo)

        Dim Emisor As New DataColumn("Emisor")
        Emisor.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Emisor)

        Dim NumeracionTercero As New DataColumn("NumeracionTercero")
        NumeracionTercero.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(NumeracionTercero)

        Dim Sucursal As New DataColumn("Sucursal")
        Sucursal.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Sucursal)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Capital As New DataColumn("Capital")
        Capital.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Capital)

        Dim Interes As New DataColumn("Interes")
        Interes.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Interes)

        Dim Cuotas As New DataColumn("Cuotas")
        Cuotas.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cuotas)

        Dim Cancelado As New DataColumn("Cancelado")
        Cancelado.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cancelado)

        Dim InteresCancelado As New DataColumn("InteresCancelado")
        InteresCancelado.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(InteresCancelado)

        Dim Gastos As New DataColumn("Gastos")
        Gastos.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Gastos)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Saldo)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Estado.DataSource = DtEstadoActivoYBaja()
        Row = Estado.DataSource.NewRow()
        Row("Clave") = 2
        Row("Nombre") = "Suspendido"
        Estado.DataSource.Rows.Add(Row)
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    Private Function NombreSucursal(ByVal Banco As Integer, ByVal Sucursal As Integer) As String

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtSucursales.Select("Banco = " & Banco & " AND Sucursal = " & Sucursal)
        Return RowsBusqueda(0).Item("Nombre")

    End Function
    Private Sub ArmaDtSucursales()

        DtSucursales = New DataTable

        If Not Tablas.Read("SELECT Banco,Sucursal,NombreSucursal AS Nombre FROM CuentasBancarias;", Conexion, DtSucursales) Then End

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If Val(MaskedPrestamo.Text) <> 0 And Not MaskedOK(MaskedPrestamo) Then
            MsgBox("Numero Prestamo Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedPrestamo.Focus()
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Prestamo" Then
            e.Value = NumeroEditado(e.Value)
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cuenta" Or Grid.Columns(e.ColumnIndex).Name = "NumeracionTercero" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, 0)
            Else : e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Capital" Or Grid.Columns(e.ColumnIndex).Name = "Cancelado" Or Grid.Columns(e.ColumnIndex).Name = "InteresCancelado" Or Grid.Columns(e.ColumnIndex).Name = "Gastos" Or Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        Dim Abierto As Boolean
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        UnPrestamo.PPrestamo = Grid.CurrentRow.Cells("Prestamo").Value
        UnPrestamo.PAbierto = Abierto
        UnPrestamo.ShowDialog()
        If UnPrestamo.PActualizacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        UnPrestamo.Dispose()

    End Sub

End Class