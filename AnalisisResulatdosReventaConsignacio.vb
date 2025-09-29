Public Class AnalisisResulatdosReventaConsignacio
    Dim DtGrid As DataTable
    Private WithEvents bs As New BindingSource
    Dim DtLotes As DataTable
    '
    Dim SqlN As String
    Dim SqlB As String
    Private Sub UnAnalisisResultado_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ComboProveedor.DataSource = ProveedoresDeFrutas()
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

        LlenaCombosGrid()

        CreaDtGrid()

    End Sub
    Private Sub AnalisisResulatdosReventaConsignacio_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

        If e.KeyData = 112 Then
            Dim pa As New PrintGPantalla(Me)
            pa.Print()
        End If

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
            SqlB = "SELECT 1 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.TipoOperacion,A.Especie,A.Variedad FROM Lotes AS L INNER JOIN Articulos AS A ON L.Articulo = A.Clave WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 1 " & SqlLiquidados
            SqlN = "SELECT 2 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.TipoOperacion,0 AS Especie,0 AS Variedad FROM Lotes AS L WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 1 " & SqlLiquidados
        End If
        If CheckReventa.Checked Then
            SqlB = "SELECT 1 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.TipoOperacion,A.Especie,A.Variedad FROM Lotes AS L INNER JOIN Articulos AS A ON L.Articulo = A.Clave WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 2 " & SqlLiquidados
            SqlN = "SELECT 2 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.TipoOperacion,0 AS Especie,0 AS Variedad FROM Lotes AS L WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 2 " & SqlLiquidados
        End If
        If CheckConsignacion.Checked And CheckReventa.Checked Then
            SqlB = "SELECT 1 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.TipoOperacion,A.Especie,A.Variedad FROM Lotes AS L INNER JOIN Articulos AS A ON L.Articulo = A.Clave WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND (L.TipoOperacion = 1 OR L.TipoOperacion = 2) " & SqlLiquidados
            SqlN = "SELECT 2 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Fecha,L.TipoOperacion,0 AS Especie,0 AS Variedad FROM Lotes AS L WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND (L.TipoOperacion = 1 OR L.TipoOperacion = 2) " & SqlLiquidados
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
            SqlLote = "AND Lote = " & Lote & " AND Secuencia = " & Secuencia & " "
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "AND Fecha BETWEEN '" & FechaParaSql(DateTimeDesde.Value) & "' AND '" & FechaParaSql(DateTimeHasta.Value.AddDays(1)) & "' "

        Dim SqlProveedor As String = ""
        If ComboProveedor.SelectedValue <> 0 Then
            SqlProveedor = "AND Proveedor = " & ComboProveedor.SelectedValue & " "
        End If

        SqlB = SqlB & SqlArticulo & SqlLote & SqlFecha & SqlProveedor
        SqlN = SqlN & SqlArticulo & SqlLote & SqlFecha & SqlProveedor

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
    Private Sub RadioImportesiniva_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioImporteSinIva.CheckedChanged

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Grid.Columns("Importe").Visible = Not Grid.Columns("Importe").Visible
        Grid.Columns("ImporteSinIva").Visible = Not Grid.Columns("ImporteSinIva").Visible
        Grid.Columns("GastoComercial").Visible = Not Grid.Columns("GastoComercial").Visible
        Grid.Columns("GastoComercialSinIva").Visible = Not Grid.Columns("GastoComercialSinIva").Visible
        Grid.Columns("CostoAsignado").Visible = Not Grid.Columns("CostoAsignado").Visible
        Grid.Columns("CostoAsignadoSinIva").Visible = Not Grid.Columns("CostoAsignadoSinIva").Visible
        Grid.Columns("CostoFruta").Visible = Not Grid.Columns("CostoFruta").Visible
        Grid.Columns("CostoFrutaSinIva").Visible = Not Grid.Columns("CostoFrutaSinIva").Visible
        Grid.Columns("Resultado").Visible = Not Grid.Columns("Resultado").Visible
        Grid.Columns("ResultadoSinIva").Visible = Not Grid.Columns("ResultadoSinIva").Visible
        Grid.Columns("PrResultado").Visible = Not Grid.Columns("PrResultado").Visible
        Grid.Columns("PrResultadoSinIva").Visible = Not Grid.Columns("PrResultadoSinIva").Visible

        If RadioImporteTotal.Checked Then
            RadioImporteTotal.ForeColor = Drawing.Color.Red
            RadioImporteSinIva.ForeColor = Drawing.Color.Black
        Else
            RadioImporteTotal.ForeColor = Drawing.Color.Black
            RadioImporteSinIva.ForeColor = Drawing.Color.Red
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function LLenaGrid() As Boolean

        If Not CheckSinStock.Checked And Not CheckConStock.Checked Then
            CheckSinStock.Checked = True
            CheckConStock.Checked = True
        End If
        '
        Dim Remitido As Decimal
        Dim Facturado As Decimal
        Dim Importe As Decimal
        Dim ImporteSinIva As Decimal
        Dim Senia As Decimal
        Dim PrecioS As Decimal
        Dim PrecioSSinIva As Decimal
        Dim Baja As Decimal
        Dim Merma As Decimal
        Dim MermaTr As Decimal
        Dim Saldo As Decimal
        Dim Stock As Decimal
        Dim Liquidado As Decimal
        Dim PrecioF As Decimal
        Dim PrecioCompra As Decimal
        Dim CantidadInicial As Decimal
        Dim Descarga As Decimal
        Dim DescargaSinIva As Decimal
        Dim GastoComercial As Decimal
        Dim GastoComercialSinIva As Decimal
        Dim InsumosConIva As Decimal
        Dim InsumosSinIva As Decimal
        Dim Color As Integer
        '
        Dim RowGrid As DataRow
        Dim View As New DataView
        View = DtLotes.DefaultView
        View.Sort = "Lote,Secuencia"

        Dim DtFacturas As New DataTable

        CreaDtGrid()

        For Each Row As DataRowView In View
            If Not GestionVentaLoteOriginal(True, Row("Operacion"), Row("Lote"), Row("Secuencia"), Remitido, Stock, Facturado, _
                                CantidadInicial, Baja, Importe, ImporteSinIva, Senia, GastoComercial, GastoComercialSinIva) Then Return False
            If (CheckConStock.Checked And CheckSinStock.Checked) Or (CheckConStock.Checked = True And Stock <> 0) Or (CheckSinStock.Checked = True And Stock = 0) Then
                'Agrega registro en DtGrid.
                Color = 0
                If Row("TipoOperacion") = 2 Then
                    Color = HallaMoneda(Row("Lote"), Row("Operacion"))
                    If Color = 1 Then Color = 0
                End If
                RowGrid = DtGrid.NewRow()
                RowGrid("Color") = Color
                RowGrid("Operacion") = Row("Operacion")
                RowGrid("Lote") = Row("Lote")
                RowGrid("Secuencia") = Row("Secuencia")
                RowGrid("Articulo") = Row("Articulo")
                RowGrid("Especie") = Row("Especie")
                RowGrid("Variedad") = Row("Variedad")
                RowGrid("Proveedor") = Row("Proveedor")
                RowGrid("Fecha") = Format(Row("Fecha"), "dd/MM/yyyy 00:00:00")
                RowGrid("Importe") = Importe
                RowGrid("ImporteSinIva") = ImporteSinIva
                RowGrid("GastoComercial") = GastoComercial
                RowGrid("GastoComercialSinIva") = GastoComercialSinIva
                RowGrid("Stock") = Stock
                RowGrid("Cantidad") = CantidadInicial - Baja
                RowGrid("Facturado") = Facturado
                RowGrid("Remitido") = Remitido
                RowGrid("CostoAsignado") = 0
                RowGrid("CostoAsignadoSinIva") = 0
                RowGrid("CostoFruta") = 0
                RowGrid("CostoFrutaSinIva") = 0
                'Halla Gastos del Lote.
                Dim CantidadInicialW As Decimal
                Dim KilosXUnidadW As Decimal
                Dim BajaW As Decimal
                Dim DescargaW As Decimal
                Dim DescargaSinIvaW As Decimal
                If Not HallaCostosDelLoteOrigen(Row("Lote"), Row("Secuencia"), Row("Operacion"), Row("Fecha"), CantidadInicialW, KilosXUnidadW, BajaW, DescargaW, DescargaSinIvaW, RowGrid("CostoAsignado"), RowGrid("CostoAsignadoSinIva"), RowGrid("CostoFruta"), RowGrid("CostoFrutaSinIva")) Then Me.Close() : Exit Function
                RowGrid("Resultado") = RowGrid("Importe") - RowGrid("GastoComercial") - RowGrid("CostoAsignado") - RowGrid("CostoFruta")
                RowGrid("ResultadoSinIva") = RowGrid("ImporteSinIva") - RowGrid("GastoComercialSinIva") - RowGrid("CostoAsignadoSinIva") - RowGrid("CostoFrutaSinIva")
                If Facturado <> 0 Then
                    RowGrid("PrResultado") = RowGrid("Resultado") / (CantidadInicial - Baja)
                    RowGrid("PrResultadoSinIva") = RowGrid("ResultadoSinIva") / (CantidadInicial - Baja)
                End If
                DtGrid.Rows.Add(RowGrid)
            End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Color As New DataColumn("Color")
        Color.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Color)

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

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Facturado As New DataColumn("Facturado")
        Facturado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Facturado)

        Dim Remitido As New DataColumn("Remitido")
        Remitido.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Remitido)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Importe)

        Dim ImporteSinIva As New DataColumn("ImporteSinIva")
        ImporteSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ImporteSinIva)

        Dim GastoComercial As New DataColumn("GastoComercial")
        GastoComercial.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(GastoComercial)

        Dim GastoComercialSinIva As New DataColumn("GastoComercialSinIva")
        GastoComercialSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(GastoComercialSinIva)

        Dim CostoAsignado As New DataColumn("CostoAsignado")
        CostoAsignado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CostoAsignado)

        Dim CostoAsignadoSinIva As New DataColumn("CostoAsignadoSinIva")
        CostoAsignadoSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CostoAsignadoSinIva)

        Dim CostoFruta As New DataColumn("CostoFruta")
        CostoFruta.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CostoFruta)

        Dim CostoFrutaSinIva As New DataColumn("CostoFrutaSinIva")
        CostoFrutaSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CostoFrutaSinIva)

        Dim Resultado As New DataColumn("Resultado")
        Resultado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Resultado)

        Dim ResultadoSinIva As New DataColumn("ResultadoSinIva")
        ResultadoSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ResultadoSinIva)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Stock)

        Dim PrResultado As New DataColumn("PrResultado")
        PrResultado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrResultado)

        Dim PrResultadoSinIva As New DataColumn("PrResultadoSinIva")
        PrResultadoSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrResultadoSinIva)

    End Sub
    Private Function HallaCostosDelLoteOrigen(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As Integer, ByRef FechaIngreso As Date, ByRef CantidadInicial As Decimal, ByRef KilosXUnidad As Decimal, ByRef Baja As Decimal, ByRef Descarga As Decimal, ByRef DescargaSinIva As Decimal, ByRef OtrosCostos As Decimal, ByRef OtrosCostosSinIva As Decimal, ByRef CostoFruta As Decimal, ByRef CostoFrutaSinIva As Decimal) As Boolean

        Dim Senia As Decimal
        Dim Merma As Decimal
        Dim Stock As Decimal
        Dim Liquidado As Decimal
        Dim Total As Decimal
        Dim TotalSinIva As Decimal

        CantidadInicial = 0
        Baja = 0
        KilosXUnidad = 0
        Descarga = 0
        DescargaSinIva = 0
        OtrosCostos = 0
        OtrosCostosSinIva = 0
        CostoFruta = 0
        CostoFrutaSinIva = 0

        If Not CostoDeUnLote(Operacion, Lote, Secuencia, Baja, Merma, Stock, Liquidado, CantidadInicial, KilosXunidad, FechaIngreso, Senia, Descarga, DescargaSinIva, OtrosCostos, OtrosCostosSinIva, CostoFruta, CostoFrutaSinIva, Total, TotalSinIva, False) Then Return False

        Return True

    End Function
    Private Sub LlenaCombosGrid()

        Articulo.DataSource = TodosLosArticulos()
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

        Especie.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 1;")
        Especie.DisplayMember = "Nombre"
        Especie.ValueMember = "Clave"

        Variedad.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 2;")
        Variedad.DisplayMember = "Nombre"
        Variedad.ValueMember = "Clave"

        Proveedor.DataSource = ProveedoresDeFrutas()
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

    End Sub
    Private Function FacturaReventaLiquidacion(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As String, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Boolean

        Dim Sql As String
        Dim ConexionLote As String
        Dim DtFacturas As DataTable

        ImporteConIva = 0
        ImporteSinIva = 0

        If Operacion = 1 Then
            ConexionLote = Conexion
        Else : ConexionLote = ConexionN
        End If

        'analiza facturas.
        Sql = "SELECT L.ImporteConIva,L.ImporteSinIva FROM FacturasProveedorCabeza AS C INNER JOIN ComproFacturados AS L ON C.Factura = L.Factura " & _
                  "WHERE C.EsReventa = 1 AND C.Estado = 1 AND L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & ";"
        DtFacturas = New DataTable
        If Not Tablas.Read(Sql, Conexion, DtFacturas) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(Sql, ConexionN, DtFacturas) Then Return False
        End If
        For Each Row1 As DataRow In DtFacturas.Rows
            ImporteConIva = ImporteConIva + Row1("ImporteConIva")
            ImporteSinIva = ImporteSinIva + Row1("ImporteSinIva")
        Next

        'analiza liquidaciones. 
        Sql = "SELECT 1 AS Operacion,L.NetoConIva,L.NetoSinIva FROM LiquidacionCabeza AS C INNER JOIN LiquidacionDetalle AS L ON C.Liquidacion = L.Liquidacion " & _
              "WHERE C.Estado = 1 AND L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & ";"
        DtFacturas = New DataTable
        If Not Tablas.Read(Sql, Conexion, DtFacturas) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(Sql, ConexionN, DtFacturas) Then Return False
        End If
        For Each Row1 As DataRow In DtFacturas.Rows
            ImporteConIva = ImporteConIva + Row1("NetoConIva")
            ImporteSinIva = ImporteSinIva + Row1("NetoSinIva")
        Next

        DtFacturas.Dispose()

        Return True

    End Function
    Public Function HallaMoneda(ByVal Lote As Integer, ByVal Operacion As Integer) As Integer

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Moneda FROM IngresoMercaderiasCabeza WHERE Lote = " & Lote & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Ingreso de Mercaderias.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
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
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 1 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.LightBlue
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) And Not IsNothing(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "ImporteSinIva" Or Grid.Columns(e.ColumnIndex).Name = "GastoComercial" Or Grid.Columns(e.ColumnIndex).Name = "GastoComercialSinIva" Or _
            Grid.Columns(e.ColumnIndex).Name = "CostoAsignado" Or Grid.Columns(e.ColumnIndex).Name = "CostoAsignadoSinIva" Or Grid.Columns(e.ColumnIndex).Name = "CostoFruta" Or Grid.Columns(e.ColumnIndex).Name = "CostoFrutaSinIva" Or _
            Grid.Columns(e.ColumnIndex).Name = "Resultado" Or Grid.Columns(e.ColumnIndex).Name = "ResultadoSinIva" Or _
            Grid.Columns(e.ColumnIndex).Name = "PrResultado" Or Grid.Columns(e.ColumnIndex).Name = "PrResultadoSinIva" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Facturado" Or Grid.Columns(e.ColumnIndex).Name = "Remitido" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Stock" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

    End Sub
   
End Class