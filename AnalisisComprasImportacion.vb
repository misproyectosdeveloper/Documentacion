Public Class AnalisisComprasImportacion
    Dim DtGrid As DataTable
    Private WithEvents bs As New BindingSource
    Dim DtLotes As DataTable
    Dim DTProveedores As DataTable
    '
    Dim SqlN As String
    Dim SqlB As String
    Private Sub UnAnalisisResultado_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 10

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        DTProveedores = New DataTable
        DTProveedores = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion <> 4 AND Producto = " & Fruta & " AND TipoIVA = " & Exterior & " ORDER BY Nombre;")

        ComboProveedor.DataSource = DTProveedores
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboArticulo, "", "Articulos")
        ComboArticulo.SelectedValue = 0
        With ComboArticulo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboEspecie, 1)
        With ComboEspecie
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboEspecie.SelectedValue = 0

        LlenaComboTablas(ComboVariedad, 2)
        With ComboVariedad
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboVariedad.SelectedValue = 0

        LlenaComboTablas(ComboCalibre, 5)
        With ComboCalibre
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboCalibre.SelectedValue = 0

        LlenaCombosGrid()

        CreaDtGrid()

    End Sub
    Private Sub AnalisisResulatdosReventaConsignacio_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If Not CheckConsignacion.Checked And Not CheckReventa.Checked Then
            CheckConsignacion.Checked = True
            CheckReventa.Checked = True
        End If

        Dim SqlLiquidados As String = ""
        If CheckLiquidados.Checked <> 0 Then
            SqlLiquidados = " AND L.Liquidado <> 0 "
        End If
        If CheckNoLiquidados.Checked <> 0 Then
            SqlLiquidados = " AND L.Liquidado = 0 "
        End If
        If CheckLiquidados.Checked <> 0 And CheckNoLiquidados.Checked <> 0 Then
            SqlLiquidados = ""
        End If

        If CheckConsignacion.Checked Then
            SqlB = "SELECT 1 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.TipoOperacion,L.Calibre,L.PrecioCompra,L.Cantidad,L.Stock,A.Especie,A.Variedad,F.ImporteSinIVA,FPC.Cambio,FPC.Moneda FROM Lotes AS L,Articulos AS A,ComproFacturados AS F,FacturasProveedorCabeza AS FPC WHERE FPC.EsExterior = 1 AND F.Factura = FPC.Factura AND L.Lote = F.Lote AND L.Secuencia = F.Secuencia AND L.Articulo = A.Clave AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 1 " & SqlLiquidados
            SqlN = "SELECT 2 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.TipoOperacion,L.Calibre,L.PrecioCompra,L.Cantidad,L.Stock,0 AS Especie,0 AS Variedad,F.ImporteSinIVA,FPC.Cambio,FPC.Moneda FROM Lotes AS L,ComproFacturados AS F,FacturasProveedorCabeza AS FPC WHERE FPC.EsExterior = 1 AND F.Factura = FPC.Factura AND L.Lote = F.Lote AND L.Secuencia = F.Secuencia AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 1 " & SqlLiquidados
        End If
        If CheckReventa.Checked Then
            SqlB = "SELECT 1 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.TipoOperacion,L.Calibre,L.PrecioCompra,L.Cantidad,L.Stock,A.Especie,A.Variedad,F.ImporteSinIVA,FPC.Cambio,FPC.Moneda FROM Lotes AS L,Articulos AS A,ComproFacturados AS F,FacturasProveedorCabeza AS FPC WHERE FPC.EsExterior = 1 AND F.Factura = FPC.Factura AND L.Lote = F.Lote AND L.Secuencia = F.Secuencia AND L.Articulo = A.Clave AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 2 " & SqlLiquidados
            SqlN = "SELECT 2 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.TipoOperacion,L.Calibre,L.PrecioCompra,L.Cantidad,L.Stock,0 AS Especie,0 AS Variedad,F.ImporteSinIVA,FPC.Cambio,FPC.Moneda FROM Lotes AS L,ComproFacturados AS F,FacturasProveedorCabeza AS FPC WHERE FPC.EsExterior = 1 AND F.Factura = FPC.Factura AND L.Lote = F.Lote AND L.Secuencia = F.Secuencia AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 2 " & SqlLiquidados
        End If
        If CheckConsignacion.Checked And CheckReventa.Checked Then
            SqlB = "SELECT 1 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.TipoOperacion,L.Calibre,L.PrecioCompra,L.Cantidad,L.Stock,A.Especie,A.Variedad,F.ImporteSinIVA,FPC.Cambio,FPC.Moneda FROM Lotes AS L,Articulos AS A,ComproFacturados AS F,FacturasProveedorCabeza AS FPC WHERE FPC.EsExterior = 1 AND F.Factura = FPC.Factura AND L.Lote = F.Lote AND L.Secuencia = F.Secuencia AND L.Articulo = A.Clave AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND (L.TipoOperacion = 1 OR L.TipoOperacion = 2) " & SqlLiquidados
            SqlN = "SELECT 2 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.TipoOperacion,L.Calibre,L.PrecioCompra,L.Cantidad,L.Stock,0 AS Especie,0 AS Variedad,F.ImporteSinIVA,FPC.Cambio,FPC.Moneda FROM Lotes AS L,ComproFacturados AS F,FacturasProveedorCabeza AS FPC WHERE FPC.EsExterior = 1 AND F.Factura = FPC.Factura AND L.Lote = F.Lote AND L.Secuencia = F.Secuencia AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND (L.TipoOperacion = 1 OR L.TipoOperacion = 2) " & SqlLiquidados
        End If

        Dim SqlArticulo As String = ""
        If ComboArticulo.SelectedValue <> 0 Then
            SqlArticulo = "AND Articulo = " & ComboArticulo.SelectedValue & " "
        Else : SqlArticulo = "AND Articulo LIKE '%' "
        End If

        Dim Lote As Integer = 0
        Dim Secuencia As Integer = 0
        If Val(MaskedLote.Text) <> 0 Then
            ConsisteMaskedLote(MaskedLote.Text, Lote, Secuencia)
        End If

        Dim SqlLote As String = ""
        If Lote <> 0 Then
            SqlLote = "AND L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & " "
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "AND L.Fecha BETWEEN '" & FechaParaSql(DateTimeDesde.Value) & "' AND '" & FechaParaSql(DateTimeHasta.Value.AddDays(1)) & "' "

        Dim SqlProveedor As String = ""
        If ComboProveedor.SelectedValue <> 0 Then
            SqlProveedor = "AND L.Proveedor = " & ComboProveedor.SelectedValue & " "
        End If

        Dim SqlCalibre As String = ""
        If ComboCalibre.SelectedValue <> 0 Then
            SqlCalibre = "AND L.Calibre = " & ComboCalibre.SelectedValue & " "
        End If

        SqlB = SqlB & SqlArticulo & SqlLote & SqlFecha & SqlProveedor & SqlCalibre
        SqlN = SqlN & SqlArticulo & SqlLote & SqlFecha & SqlProveedor & SqlCalibre

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtLotes = New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtLotes) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtLotes) Then Me.Close() : Exit Sub
        End If

        Dim DtArticulos As New DataTable
        For Each Row As DataRow In DtLotes.Rows
            If Row("Especie") = 0 Then
                DtArticulos = Tablas.Leer("SELECT Especie,Variedad FROM Articulos WHERE Clave = " & Row("Articulo") & ";")
                Row("Especie") = DtArticulos.Rows(0).Item("Especie")
                Row("Variedad") = DtArticulos.Rows(0).Item("Variedad")
            End If
            If ComboEspecie.SelectedValue <> 0 Then
                If ComboEspecie.SelectedValue <> Row("Especie") Then Row.Delete()
            End If
            If ComboVariedad.SelectedValue <> 0 Then
                If ComboVariedad.SelectedValue <> Row("Variedad") Then
                    If Row.RowState <> DataRowState.Deleted Then Row.Delete()
                End If
            End If
        Next
        DtArticulos.Dispose()

        If DtLotes.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If Not LLenaGrid() Then Me.Close() : Exit Sub

        Grid.Focus()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, "", "", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboArticulo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboArticulo.Validating

        If IsNothing(ComboArticulo.SelectedValue) Then ComboArticulo.SelectedValue = 0

    End Sub
    Private Sub ComboEspecie_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEspecie.Validating

        If IsNothing(ComboEspecie.SelectedValue) Then ComboEspecie.SelectedValue = 0

    End Sub
    Private Sub ComboVariedad_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboVariedad.Validating

        If IsNothing(ComboVariedad.SelectedValue) Then ComboVariedad.SelectedValue = 0

    End Sub
    Private Sub ComboCalibre_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCalibre.Validating

        If IsNothing(ComboCalibre.SelectedValue) Then ComboCalibre.SelectedValue = 0

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
    Private Function LLenaGrid() As Boolean

        If Not CheckSinStock.Checked And Not CheckConStock.Checked Then
            CheckSinStock.Checked = True
            CheckConStock.Checked = True
        End If
        '
        Dim RowGrid As DataRow
        Dim View As New DataView
        View = DtLotes.DefaultView
        View.Sort = "Lote,Secuencia"

        Dim DtFacturas As New DataTable

        CreaDtGrid()

        For Each Row As DataRowView In View
            If (CheckConStock.Checked And CheckSinStock.Checked) Or (CheckConStock.Checked = True And Row("Stock") <> 0) Or (CheckSinStock.Checked = True And Row("Stock") = 0) Then
                'Agrega registro en DtGrid.
                RowGrid = DtGrid.NewRow()
                RowGrid("Operacion") = Row("Operacion")
                RowGrid("Lote") = Row("Lote")
                RowGrid("Secuencia") = Row("Secuencia")
                RowGrid("Articulo") = Row("Articulo")
                RowGrid("Especie") = Row("Especie")
                RowGrid("Variedad") = Row("Variedad")
                RowGrid("Calibre") = Row("Calibre")
                RowGrid("Proveedor") = Row("Proveedor")
                RowGrid("Fecha") = Format(Row("Fecha"), "dd/MM/yyyy 00:00:00")
                RowGrid("Stock") = Row("Stock")
                RowGrid("Cantidad") = Row("Cantidad")
                RowGrid("CostoCompra") = Row("Cantidad") * Row("PrecioCompra")
                RowGrid("CostoFinal") = Row("ImporteSinIVA") / Row("Cambio")
                RowGrid("Moneda") = Row("Moneda")
                DtGrid.Rows.Add(RowGrid)
            End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Especie As New DataColumn("Especie")
        Especie.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Especie)

        Dim Variedad As New DataColumn("Variedad")
        Variedad.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Variedad)

        Dim Calibre As New DataColumn("Calibre")
        Calibre.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Calibre)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Importe)

        Dim ImporteSinIva As New DataColumn("ImporteSinIva")
        ImporteSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ImporteSinIva)

        Dim CostoFinal As New DataColumn("CostoFinal")
        CostoFinal.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CostoFinal)

        Dim CostoCompra As New DataColumn("CostoCompra")
        CostoCompra.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CostoCompra)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Stock)

        Dim Moneda As New DataColumn("Moneda")
        Moneda.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Moneda)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Articulo.DataSource = TodosLosArticulos()
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

        Especie.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 1;")
        Especie.DisplayMember = "Nombre"
        Especie.ValueMember = "Clave"

        Variedad.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 2;")
        Variedad.DisplayMember = "Nombre"
        Variedad.ValueMember = "Clave"

        Calibre.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 5;")
        Calibre.DisplayMember = "Nombre"
        Calibre.ValueMember = "Clave"
        Row = Calibre.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Calibre.DataSource.Rows.Add(Row)
        Calibre.DisplayMember = "Nombre"
        Calibre.ValueMember = "Clave"

        Moneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27 UNION SELECT 1,'PESOS';")
        Moneda.DisplayMember = "Nombre"
        Moneda.ValueMember = "Clave"
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Moneda.DataSource.Rows.Add(Row)

        Proveedor.DataSource = DTProveedores
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Hasta debe ser mayor o igual a fecha Desde.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Return False
        End If

        Dim Lote As Integer = 0
        Dim Secuencia As Integer = 0
        If Val(MaskedLote.Text) <> 0 Then
            If Not ConsisteMaskedLote(MaskedLote.Text, Lote, Secuencia) Then
                MaskedLote.Focus()
                Return False
            End If
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) And _
              Not IsNothing(Grid.Rows(e.RowIndex).Cells("Lote").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
            End If
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) And PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) And Not IsNothing(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "CostoCompra" Or Grid.Columns(e.ColumnIndex).Name = "CostoFinal" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Stock" Or Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

    End Sub
End Class