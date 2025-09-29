Public Class AnalisisResultadosLotesCosteo
    Dim DtGrid As DataTable
    Dim DtCosteos As DataTable
    Private WithEvents bs As New BindingSource
    Dim DtLotes As DataTable
    '
    Dim SqlN As String
    Dim SqlB As String
    Private Sub AnalisisDeResultadoLotesCosteo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaCombo(ComboProveedor, "", "Negocios")
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
        CreaDtCosteos()

    End Sub
    Private Sub AnalisisDeResultadoLotesCosteo_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

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

        SqlB = "SELECT 1 AS Operacion,L.Lote,L.Secuencia,L.Cantidad,L.KilosXUnidad,L.Baja,L.Articulo,L.Proveedor,L.Fecha,A.Especie,A.Variedad,A.Iva FROM Lotes AS L INNER JOIN Articulos AS A ON L.Articulo = A.Clave WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 4 "
        SqlN = "SELECT 2 AS Operacion,L.Lote,L.Secuencia,L.Cantidad,L.KilosXUnidad,L.Baja,L.Articulo,L.Proveedor,L.Fecha,0 AS Especie,0 AS Variedad,0 AS Iva FROM Lotes AS L WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 4 "

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

        SqlB = SqlB & SqlLote & SqlFecha & SqlProveedor & SqlArticulo
        SqlN = SqlN & SqlLote & SqlFecha & SqlProveedor & SqlArticulo

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtLotes = New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtLotes) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtLotes) Then Me.Close() : Exit Sub
        End If

        If ComboCosteo.SelectedValue <> 0 Then
            Dim DtCosteo As New DataTable
            If Not Tablas.Read("SELECT Lote FROM IngresoMercaderiasCabeza WHERE Costeo = " & ComboCosteo.SelectedValue & ";", Conexion, DtCosteo) Then Me.Close() : Exit Sub
            If PermisoTotal Then
                If Not Tablas.Read("SELECT Lote FROM IngresoMercaderiasCabeza WHERE Costeo = " & ComboCosteo.SelectedValue & ";", ConexionN, DtCosteo) Then Me.Close() : Exit Sub
            End If
            Dim RowsBusqueda() As DataRow
            For Each Row As DataRow In DtLotes.Rows
                RowsBusqueda = DtCosteo.Select("Lote = " & Row("Lote"))
                If RowsBusqueda.Length = 0 Then
                    Row.Delete()
                End If
            Next
            DtCosteo.Dispose()
            DtLotes.AcceptChanges()
        End If

        Dim DtArticulos As New DataTable
        For Each Row As DataRow In DtLotes.Rows
            If Row("Especie") = 0 Then
                DtArticulos = Tablas.Leer("SELECT Especie,Variedad,Iva FROM Articulos WHERE Clave = " & Row("Articulo") & ";")
                Row("Especie") = DtArticulos.Rows(0).Item("Especie")
                Row("Variedad") = DtArticulos.Rows(0).Item("Variedad")
                Row("Iva") = DtArticulos.Rows(0).Item("Iva")
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
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & ComboProveedor.SelectedValue & ";"
        ComboCosteo.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboCosteo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        ComboCosteo.DataSource.rows.add(Row)
        ComboCosteo.DisplayMember = "Nombre"
        ComboCosteo.ValueMember = "Costeo"
        ComboCosteo.SelectedValue = 0

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

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
        Grid.Columns("CostoProduccion").Visible = Not Grid.Columns("CostoProduccion").Visible
        Grid.Columns("CostoProduccionSinIva").Visible = Not Grid.Columns("CostoProduccionSinIva").Visible
        Grid.Columns("Resultado").Visible = Not Grid.Columns("Resultado").Visible
        Grid.Columns("ResultadoSinIva").Visible = Not Grid.Columns("ResultadoSinIva").Visible
        Grid.Columns("PrResultado").Visible = Not Grid.Columns("PrResultado").Visible
        Grid.Columns("PrResultadoSinIva").Visible = Not Grid.Columns("PrResultadoSinIva").Visible

        If RadioImporteTotal.Checked Then
            RadioImporteTotal.ForeColor = Color.Red
            RadioImporteSinIva.ForeColor = Color.Black
        Else
            RadioImporteTotal.ForeColor = Color.Black
            RadioImporteSinIva.ForeColor = Color.Red
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, "", "", "")

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
        '
        Dim RowGrid As DataRow
        Dim View As New DataView
        View = DtLotes.DefaultView
        View.Sort = "Lote,Secuencia"

        DtGrid.Clear()
        DtCosteos.Clear()

        For Each Row As DataRowView In View
            Dim ListaOrden As String
            If Not AnalisisCostoLote(False, Row("Operacion"), Row("Proveedor"), Row("Lote"), Row("Secuencia"), Saldo, PrecioS, PrecioSSinIva, Remitido, Facturado, Importe, ImporteSinIva, Baja, Merma, MermaTr, Stock, Liquidado, PrecioF, PrecioCompra, CantidadInicial, Senia, Descarga, DescargaSinIva, GastoComercial, GastoComercialSinIva, False, False, ListaOrden) Then Return False
            If (CheckConStock.Checked And CheckSinStock.Checked) Or (CheckConStock.Checked = True And Stock <> 0) Or (CheckSinStock.Checked = True And Stock = 0) Then
                'Agrega registro en DtGrid.
                RowGrid = DtGrid.NewRow()
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
                '
                'costos asignados.
                If Not FacturaAfectaLotes(Row("Lote"), Row("Secuencia"), RowGrid("CostoAsignado"), RowGrid("CostoAsignadoSinIva")) Then Me.Close() : Exit Function
                '
                'Importes Lotes en Recibos..........................................
                Dim ImporteLotesRecibosConIva As Decimal = 0
                Dim ImporteLotesRecibosSinIva As Decimal = 0
                HallaImportesLotesRecibos(Row("Lote"), Row("Secuencia"), ImporteLotesRecibosConIva, ImporteLotesRecibosSinIva)
                RowGrid("CostoAsignado") = RowGrid("CostoAsignado") + ImporteLotesRecibosConIva
                RowGrid("CostoAsignadoSinIva") = RowGrid("CostoAsignadoSinIva") + ImporteLotesRecibosSinIva
                '
                'Importes Lotes en Reintegros..........................................
                Dim ImporteLotesReintegrosConIva As Decimal = 0
                Dim ImporteLotesReintegrosSinIva As Decimal = 0
                HallaImportesLotesReintegros(Row("Lote"), Row("Secuencia"), ImporteLotesReintegrosConIva, ImporteLotesReintegrosSinIva)
                RowGrid("CostoAsignado") = RowGrid("CostoAsignado") - ImporteLotesReintegrosConIva
                RowGrid("CostoAsignadoSinIva") = RowGrid("CostoAsignadoSinIva") - ImporteLotesReintegrosSinIva
                '
                'Importes Lotes en Otras Facturas..........................................
                Dim ImporteLotesOtrasFacturasConIva As Decimal = 0
                Dim ImporteLotesOtrasfacturasSinIva As Decimal = 0
                HallaImportesLotesOtrasFacturas(Row("Lote"), Row("Secuencia"), ImporteLotesOtrasFacturasConIva, ImporteLotesOtrasfacturasSinIva)
                RowGrid("CostoAsignado") = RowGrid("CostoAsignado") + ImporteLotesOtrasFacturasConIva
                RowGrid("CostoAsignadoSinIva") = RowGrid("CostoAsignadoSinIva") + ImporteLotesOtrasfacturasSinIva
                '
                'Importes Lotes en asientos manuales..........................................
                Dim ImporteLotesAsientosManualesConIva As Decimal = 0
                Dim ImporteLotesAsientosManualesSinIva As Decimal = 0
                HallaImportesLotesAsientosManuales(Row("Lote"), Row("Secuencia"), ImporteLotesAsientosManualesConIva, ImporteLotesAsientosManualesSinIva)
                RowGrid("CostoAsignado") = RowGrid("CostoAsignado") + ImporteLotesAsientosManualesConIva
                RowGrid("CostoAsignadoSinIva") = RowGrid("CostoAsignadoSinIva") + ImporteLotesAsientosManualesSinIva
                '
                'Suma costo de la descarga del lote.
                RowGrid("CostoAsignado") = RowGrid("CostoAsignado") + Descarga
                RowGrid("CostoAsignadoSinIva") = RowGrid("CostoAsignadoSinIva") + DescargaSinIva
                '
                'Costos de insumos.
                Dim Costeo As Integer = HallaCosteoLote(Row("Operacion"), Row("Lote"))
                If Costeo < 0 Then Me.Close() : Exit Function
                Dim Cerrado As Boolean = HallaCosteoCerrado(Costeo)
                RowGrid("CostoProduccion") = 0
                RowGrid("CostoProduccionSinIva") = 0
                If Costeo < 0 Then Me.Close() : Exit Function
                Dim RowsBusqueda() As DataRow
                RowsBusqueda = DtCosteos.Select("Costeo = " & Costeo)
                If RowsBusqueda.Length <> 0 Then
                    RowGrid("CostoProduccion") = RowsBusqueda(0).Item("CostoConIva") * (Row("Cantidad") - Row("Baja")) * Row("KilosXUnidad")
                    RowGrid("CostoProduccionSinIva") = RowsBusqueda(0).Item("CostoSinIva") * (Row("Cantidad") - Row("Baja")) * Row("KilosXUnidad")
                Else
                    Dim CostoConIva As Decimal
                    Dim CostoSinIva As Decimal
                    Dim CostoEst As Decimal
                    Dim Iva As Decimal
                    Dim Kilos As Decimal
                    If HallaCostosPrecioKilos(Costeo, CostoConIva, CostoSinIva, CostoEst, Kilos, Cerrado, Iva) < 0 Then Me.Close() : Exit Function
                    If Cerrado Then
                        If Kilos <> 0 Then
                            RowGrid("CostoProduccion") = Trunca3(CostoConIva / Kilos) * (Row("Cantidad") - Row("Baja")) * Row("KilosXUnidad")
                            RowGrid("CostoProduccionSinIva") = Trunca3(CostoSinIva / Kilos) * (Row("Cantidad") - Row("Baja")) * Row("KilosXUnidad")
                        End If
                    Else
                        RowGrid("CostoProduccion") = Trunca3(CostoEst * (1 + Iva / 100)) * (Row("Cantidad") - Row("Baja")) * Row("KilosXUnidad")
                        RowGrid("CostoProduccionSinIva") = CostoEst * (Row("Cantidad") - Row("Baja")) * Row("KilosXUnidad")
                    End If
                    Dim Row1 As DataRow = DtCosteos.NewRow
                    Row1("Costeo") = Costeo
                    If Cerrado And Kilos <> 0 Then
                        Row1("CostoConIva") = Trunca3(CostoConIva / Kilos)
                        Row1("CostoSinIva") = Trunca3(CostoSinIva / Kilos)
                    Else
                        Row1("CostoConIva") = Trunca3(CostoEst * (1 + Iva / 100))
                        Row1("CostoSinIva") = CostoEst
                    End If
                    DtCosteos.Rows.Add(Row1)
                End If
                RowGrid("Cerrado") = Cerrado
                RowGrid("Resultado") = RowGrid("Importe") - RowGrid("GastoComercial") - RowGrid("CostoAsignado") - RowGrid("CostoProduccion")
                RowGrid("ResultadoSinIva") = RowGrid("ImporteSinIva") - RowGrid("GastoComercialSinIva") - RowGrid("CostoAsignadoSinIva") - RowGrid("CostoProduccionSinIva")
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

        Dim CostoProduccion As New DataColumn("CostoProduccion")
        CostoProduccion.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CostoProduccion)

        Dim CostoProduccionSinIva As New DataColumn("CostoProduccionSinIva")
        CostoProduccionSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CostoProduccionSinIva)

        Dim Resultado As New DataColumn("Resultado")
        Resultado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Resultado)

        Dim ResultadoSinIva As New DataColumn("ResultadoSinIva")
        ResultadoSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ResultadoSinIva)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Stock)

        Dim Facturado As New DataColumn("Facturado")
        Facturado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Facturado)

        Dim Remitido As New DataColumn("Remitido")
        Remitido.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Remitido)

        Dim Cerrado As New DataColumn("Cerrado")
        Cerrado.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Cerrado)

        Dim PrResultado As New DataColumn("PrResultado")
        PrResultado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrResultado)

        Dim PrResultadoSinIva As New DataColumn("PrResultadoSinIva")
        PrResultadoSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrResultadoSinIva)

    End Sub
    Private Sub CreaDtCosteos()

        DtCosteos = New DataTable

        Dim Costeo As New DataColumn("Costeo")
        Costeo.DataType = System.Type.GetType("System.Int32")
        DtCosteos.Columns.Add(Costeo)

        Dim CostoConIva As New DataColumn("CostoConIva")
        CostoConIva.DataType = System.Type.GetType("System.Decimal")
        DtCosteos.Columns.Add(CostoConIva)

        Dim CostoSinIva As New DataColumn("CostoSinIva")
        CostoSinIva.DataType = System.Type.GetType("System.Decimal")
        DtCosteos.Columns.Add(CostoSinIva)

    End Sub
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

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;")
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

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "ImporteSinIva" Or Grid.Columns(e.ColumnIndex).Name = "GastoComercial" Or Grid.Columns(e.ColumnIndex).Name = "GastoComercialSinIva" Or _
            Grid.Columns(e.ColumnIndex).Name = "CostoAsignado" Or Grid.Columns(e.ColumnIndex).Name = "CostoAsignadoSinIva" Or Grid.Columns(e.ColumnIndex).Name = "CostoProduccion" Or Grid.Columns(e.ColumnIndex).Name = "CostoProduccionSinIva" Or _
            Grid.Columns(e.ColumnIndex).Name = "Resultado" Or Grid.Columns(e.ColumnIndex).Name = "ResultadoSinIva" Or _
            Grid.Columns(e.ColumnIndex).Name = "PrResultado" Or Grid.Columns(e.ColumnIndex).Name = "PrResultadoSinIva" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Facturado" Or Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Stock" Or Grid.Columns(e.ColumnIndex).Name = "Remitido" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

    End Sub

   
End Class