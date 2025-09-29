Public Class ListaCompraDivisas
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim DtClientes As DataTable
    Dim DtProveedores As DataTable
    Dim DtBancos As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaCompraDivisas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        DtClientes = New DataTable
        DtProveedores = New DataTable
        DtBancos = New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Clientes;", Conexion, DtClientes) Then Me.Close() : Exit Sub
        If Not Tablas.Read("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion <> 4;", Conexion, DtProveedores) Then Me.Close() : Exit Sub
        If Not Tablas.Read("Select Clave,Nombre From Tablas WHERE Tipo = 26;", Conexion, DtBancos) Then Me.Close() : Exit Sub

        LlenaCombosGrid()

        ComboCliente.DataSource = DtClientes
        Dim Row As DataRow = ComboCliente.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboCliente.DataSource.Rows.Add(Row)
        ComboCliente.DisplayMember = "Nombre"
        ComboCliente.ValueMember = "Clave"
        ComboCliente.SelectedValue = 0
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboProveedor.DataSource = DtProveedores
        Row = ComboProveedor.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboProveedor.DataSource.Rows.Add(Row)
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboBanco.DataSource = DtBancos
        Row = ComboBanco.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboBanco.DataSource.Rows.Add(Row)
        ComboBanco.DisplayMember = "Nombre"
        ComboBanco.ValueMember = "Clave"
        ComboBanco.SelectedValue = 0
        With ComboBanco
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        MaskedMovimiento.Text = "000000000000"

        CreaDtGrid()

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Checked = False
        Else
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar.Focus() : ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaCompraDivisas_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        Dim SqlFecha As String
        SqlFecha = "C.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlProveedor As String = ""
        If ComboProveedor.SelectedValue <> 0 Then
            SqlProveedor = " AND C.Origen = 2 AND C.Emisor = " & ComboProveedor.SelectedValue
        End If

        Dim SqlCliente As String = ""
        If ComboCliente.SelectedValue <> 0 Then
            SqlCliente = " AND C.Origen = 3 AND C.Emisor = " & ComboCliente.SelectedValue
        End If

        Dim SqlBanco As String = ""
        If ComboBanco.SelectedValue <> 0 Then
            SqlBanco = " AND C.Origen = 1 AND C.Emisor = " & ComboBanco.SelectedValue
        End If

        Dim SqlMovimiento As String = ""
        If Val(MaskedMovimiento.Text) <> 0 Then
            SqlMovimiento = " AND C.Movimiento = " & Val(MaskedMovimiento.Text)
        End If

        Dim StrCaja As String
        If Not GCajaTotal Then
            StrCaja = " AND Caja = " & GCaja
        End If

        SqlB = "SELECT 1 as Operacion,* FROM CompraDivisasCabeza AS C WHERE " & SqlFecha & SqlProveedor & SqlCliente & SqlBanco & SqlMovimiento & StrCaja & ";"
        SqlN = "SELECT 2 as Operacion,* FROM CompraDivisasCabeza AS C WHERE " & SqlFecha & SqlProveedor & SqlCliente & SqlBanco & SqlMovimiento & StrCaja & ";"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()

        LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0 : Exit Sub

        ComboCliente.SelectedValue = 0
        ComboBanco.SelectedValue = 0

    End Sub
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0 : Exit Sub

        ComboProveedor.SelectedValue = 0
        ComboBanco.SelectedValue = 0

    End Sub
    Private Sub ComboBancos_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboBanco.Validating

        If IsNothing(ComboBanco.SelectedValue) Then ComboBanco.SelectedValue = 0 : Exit Sub

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

        Dim Dt As New DataTable

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked = True Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim TotalCompra As Double = 0
        Dim TotalVenta As Double = 0
        Dim TotalPesos As Double = 0

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Movimiento"

        Dim Row1 As DataRow

        For Each Row As DataRowView In View
            Row1 = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Movimiento") = Row("Movimiento")
            If Row("Origen") = 1 Then Row1("Emisor") = HallaNombreBanco(Row("Emisor"))
            If Row("Origen") = 2 Then Row1("Emisor") = HallaNombreProveedor(Row("Emisor"))
            If Row("Origen") = 3 Then Row1("Emisor") = HallaNombreCliente(Row("Emisor"))
            Row1("Fecha") = Row("Fecha")
            Row1("Moneda") = Row("Moneda")
            If Row("TipoMovimiento") = 6000 Then
                Row1("Compra") = Row("Importe")
                TotalCompra = TotalCompra + Row("Importe")
                Row1("Venta") = 0
            Else : Row1("Venta") = Row("Importe")
                TotalVenta = TotalVenta + Row("Importe")
                Row1("Compra") = 0
            End If
            Row1("Cambio") = Row("Cambio")
            Row1("ImportePesos") = Trunca(Row("Cambio") * Row("Importe"))
            TotalPesos = TotalPesos + Row1("ImportePesos")
            Row1("Estado") = Row("Estado")
            If Row("Estado") = 1 Then Row1("Estado") = 0
            DtGrid.Rows.Add(Row1)
        Next
        Row1 = DtGrid.NewRow
        Row1("Compra") = TotalCompra
        Row1("Venta") = TotalVenta
        Row1("ImportePesos") = TotalPesos
        DtGrid.Rows.Add(Row1)

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

    End Sub
    Private Function HallaNombreBanco(ByVal Banco As Integer) As String

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtBancos.Select("Clave = " & Banco)
        If RowsBusqueda.Length <> 0 Then Return RowsBusqueda(0).Item("Nombre")

    End Function
    Private Function HallaNombreCliente(ByVal Cliente As Integer) As String

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtClientes.Select("Clave = " & Cliente)
        If RowsBusqueda.Length <> 0 Then Return RowsBusqueda(0).Item("Nombre")

    End Function
    Private Function HallaNombreProveedor(ByVal Proveedor As Integer) As String

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtProveedores.Select("Clave = " & Proveedor)
        If RowsBusqueda.Length <> 0 Then Return RowsBusqueda(0).Item("Nombre")

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Movimiento As New DataColumn("Movimiento")
        Movimiento.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Movimiento)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Compra As New DataColumn("Compra")
        Compra.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Compra)

        Dim Venta As New DataColumn("Venta")
        Venta.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Venta)

        Dim Moneda As New DataColumn("Moneda")
        Moneda.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Moneda)

        Dim Cambio As New DataColumn("Cambio")
        Cambio.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cambio)

        Dim ImportePesos As New DataColumn("ImportePesos")
        ImportePesos.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ImportePesos)

        Dim Emisor As New DataColumn("Emisor")
        Emisor.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Emisor)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

    End Sub
    Private Sub LlenaCombosGrid()

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        Moneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Dim Row As DataRow = Moneda.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Moneda.DataSource.Rows.Add(Row)
        Moneda.DisplayMember = "Nombre"
        Moneda.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If Val(MaskedMovimiento.Text) <> 0 And Not MaskedOK(MaskedMovimiento) Then
            MsgBox("Comprobante Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedMovimiento.Focus()
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If IsDBNull(e.Value) Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Movimiento" Then
            e.Value = NumeroEditado(e.Value)
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Compra" Or Grid.Columns(e.ColumnIndex).Name = "Venta" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(0, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "ImportePesos" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
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

        If EsReemplazoChequeCompraDivisas(Grid.CurrentRow.Cells("Movimiento").Value, Abierto) Then
            UnChequeReemplazo.PTipoNota = 6000
            UnChequeReemplazo.PNota = Grid.CurrentRow.Cells("Movimiento").Value
            UnChequeReemplazo.PAbierto = Abierto
            UnChequeReemplazo.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            Exit Sub
        End If

        UnaCompraDivisas.PMovimiento = Grid.CurrentRow.Cells("Movimiento").Value
        UnaCompraDivisas.PAbierto = Abierto
        UnaCompraDivisas.ShowDialog()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
End Class