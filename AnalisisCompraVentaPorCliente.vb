Public Class AnalisisCompraVentaPorCliente
    Dim DtGrid As DataTable
    Private WithEvents bs As New BindingSource
    Dim DtLotes As DataTable
    '
    Dim SqlN As String
    Dim SqlB As String
    Private Sub AnalisisCompraVentaPorCliente_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ComboCliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes WHERE DeOperacion = 0;")
        Dim Row As DataRow = ComboCliente.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboCliente.DataSource.rows.add(Row)
        ComboCliente.DisplayMember = "Nombre"
        ComboCliente.ValueMember = "Clave"
        ComboCliente.SelectedValue = 0
        With ComboCliente
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

        CreaDtGrid()

        Dim SqlFecha As String
        SqlFecha = "Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlCliente As String = ""
        If ComboCliente.SelectedValue <> 0 Then
            SqlCliente = "AND Cliente = " & ComboCliente.SelectedValue & " "
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

        SqlB = "SELECT 1 AS Operacion,A.Lote,A.Secuencia,A.Operacion As OperacionLote,C.Factura,C.Rel,C.Relacionada,C.Fecha,0 AS Articulo,0 AS Especie,0 AS Variedad,0.00 AS Liquidado,0 AS SecuenciaOrigen FROM FacturasCabeza AS C INNER JOIN AsignacionLotes AS A ON C.Factura = A.Comprobante AND A.TipoComprobante = 2 WHERE C.Tr = 0 AND " & SqlFecha & SqlCliente & SqlLote & ";"
        SqlN = "SELECT 2 AS Operacion,A.Lote,A.Secuencia,A.Operacion As OperacionLote,C.Factura,C.Rel,C.Relacionada,C.Fecha,0 AS Articulo,0 AS Especie,0 AS Variedad,0.00 AS Liquidado,0 AS SecuenciaOrigen FROM FacturasCabeza AS C INNER JOIN AsignacionLotes AS A ON C.Factura = A.Comprobante AND A.TipoComprobante = 2 WHERE C.Tr = 0 AND C.Rel = 0 AND " & SqlFecha & SqlCliente & SqlLote & ";"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtLotesAux As New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtLotesaux) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtLotesaux) Then Me.Close() : Exit Sub
        End If

        DtLotes = DtLotesAux.Clone
        Dim RowsBusqueda() As DataRow

        'Saco Lotes Repetidos Y Pone la secuencia = secuenciaOrigen.
        For Each Row As DataRow In DtLotesAux.Rows
            Dim EsRepetido As Boolean = False
            If DtLotes.Rows.Count <> 0 Then
                RowsBusqueda = DtLotes.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
                If RowsBusqueda.Length <> 0 Then EsRepetido = True
            End If
            If Not EsRepetido Then
                Dim Row1 As DataRow = DtLotes.NewRow
                For I As Integer = 0 To DtLotesAux.Columns.Count - 1
                    Row1.Item(I) = Row.Item(I)
                Next
                Dim ConexionStr
                If Row("OperacionLote") = 1 Then
                    ConexionStr = Conexion
                Else : ConexionStr = ConexionN
                End If
                If HallaDatosLotes(Row("Lote"), Row("Secuencia"), ConexionStr, Row1("SecuenciaOrigen"), Row1("Articulo"), Row1("Especie"), Row1("Variedad"), Row1("Liquidado")) Then
                    DtLotes.Rows.Add(Row1)
                End If
            End If
        Next

        If DtLotes.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
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

        Dim Facturado As Decimal
        Dim Importe As Decimal
        Dim ImporteSinIva As Decimal
        Dim KilosFacturado As Decimal
        Dim KilosXUnidad As Decimal
        Dim Liquidado As Decimal
        Dim CantidadInicial As Decimal
        Dim Baja As Decimal
        Dim Descarga As Decimal
        Dim DescargaSinIva As Decimal
        Dim GastoComercial As Decimal
        Dim GastoComercialSinIva As Decimal
        Dim OtrosCostos As Decimal
        Dim OtrosCostosSinIva As Decimal
        Dim CostoFruta As Decimal
        Dim CostoFrutaSinIva As Decimal
        Dim FechaIngreso As Date
        Dim Color As Integer
        '
        Dim RowGrid As DataRow
        Dim View As New DataView
        View = DtLotes.DefaultView
        View.Sort = "Lote,SecuenciaOrigen"
        Dim ClaveAnt As String = ""

        For Each Row As DataRowView In View
            Dim ConnexionStr
            If Row("OperacionLote") = 1 Then
                ConnexionStr = Conexion
            Else : ConnexionStr = ConexionN
            End If
            If Row("Lote") & Row("SecuenciaOrigen") <> ClaveAnt Then
                ClaveAnt = Row("Lote") & Row("SecuenciaOrigen")
                If Not HallaCostosDelLoteOrigen(Row("Lote"), Row("SecuenciaOrigen"), Row("OperacionLote"), FechaIngreso, CantidadInicial, KilosXUnidad, Baja, Descarga, DescargaSinIva, OtrosCostos, OtrosCostosSinIva, CostoFruta, CostoFrutaSinIva, Liquidado) Then Return False
            End If
            If Not GestionLotesVendidos(ComboCliente.SelectedValue, Row("OperacionLote"), Row("Lote"), Row("Secuencia"), 0, FechaIngreso, Facturado, KilosFacturado, _
                   Importe, ImporteSinIva, GastoComercial, GastoComercialSinIva) Then Return False
            'Agrega registro en DtGrid.
            Color = 0
            Color = HallaMoneda(Row("Lote"), Row("Operacion"))
            If Color = 1 Then Color = 0
            RowGrid = DtGrid.NewRow()
            RowGrid("Color") = Color
            RowGrid("Operacion") = Row("OperacionLote")
            RowGrid("Lote") = Row("Lote")
            RowGrid("Secuencia") = Row("Secuencia")
            RowGrid("Articulo") = Row("Articulo")
            RowGrid("Especie") = Row("Especie")
            RowGrid("Variedad") = Row("Variedad")
            RowGrid("Cliente") = ComboCliente.SelectedValue
            RowGrid("Fecha") = Format(FechaIngreso, "dd/MM/yyyy 00:00:00")
            RowGrid("Importe") = Importe
            RowGrid("ImporteSinIva") = ImporteSinIva
            RowGrid("GastoComercial") = GastoComercial
            RowGrid("GastoComercialSinIva") = GastoComercialSinIva
            RowGrid("Liquidado") = Liquidado
            RowGrid("Facturado") = Facturado
            'Halla proporcional a lo facturado.
            Dim R As Decimal = KilosFacturado / KilosXUnidad
            If (CantidadInicial - Baja) <> 0 Then R = R / (CantidadInicial - Baja)
            '
            RowGrid("CostoAsignado") = OtrosCostos * R
            RowGrid("CostoAsignadoSinIva") = OtrosCostosSinIva * R
            RowGrid("CostoFruta") = CostoFruta * R
            RowGrid("CostoFrutaSinIva") = CostoFrutaSinIva * R
            RowGrid("Resultado") = RowGrid("Importe") - RowGrid("GastoComercial") - RowGrid("CostoAsignado") - RowGrid("CostoFruta")
            RowGrid("ResultadoSinIva") = RowGrid("ImporteSinIva") - RowGrid("GastoComercialSinIva") - RowGrid("CostoAsignadoSinIva") - RowGrid("CostoFrutaSinIva")
            If Facturado <> 0 Then
                RowGrid("PrResultado") = RowGrid("Resultado") / Facturado
                RowGrid("PrResultadoSinIva") = RowGrid("ResultadoSinIva") / Facturado
            End If
            DtGrid.Rows.Add(RowGrid)
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

        Dim Cliente As New DataColumn("Cliente")
        Cliente.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cliente)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Facturado As New DataColumn("Facturado")
        Facturado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Facturado)

        Dim Liquidado As New DataColumn("Liquidado")
        Liquidado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Liquidado)

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

    End Sub
    Private Function HallaDatosLotes(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal ConexionStr As String, ByRef SecuenciaOrigen As Integer, ByRef Articulo As Integer, ByRef Especie As Integer, ByRef Variedad As Integer, ByRef Liquidado As Decimal) As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Articulo,SecuenciaOrigen FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", ConexionStr, Dt) Then End
        Articulo = Dt.Rows(0).Item("Articulo")
        SecuenciaOrigen = Dt.Rows(0).Item("SecuenciaOrigen")

        HallaEspecieYVariedad(Articulo, Especie, Variedad)

        Dt = New DataTable
        If Not Tablas.Read("SELECT Liquidado FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & SecuenciaOrigen & " AND Deposito = DepositoOrigen" & ";", ConexionStr, Dt) Then End
        Liquidado = Dt.Rows(0).Item("Liquidado")
        Dt.Dispose()

        If Not (CheckLiquidados.Checked = True And CheckNoLiquidados.Checked = True) Then
            If CheckLiquidados.Checked And Liquidado = 0 Then Return False
            If CheckNoLiquidados.Checked And Liquidado <> 0 Then Return False
        End If

        If ComboEspecie.SelectedValue <> 0 Then
            If ComboEspecie.SelectedValue <> Especie Then Return False
        End If
        If ComboVariedad.SelectedValue <> 0 Then
            If ComboVariedad.SelectedValue <> Variedad Then Return False
        End If

        Return True

    End Function
    Private Sub HallaDatosLoteOrigen(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal ConexionStr As String, ByRef FechaIngreso As Date)

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Fecha FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", ConexionStr, Dt) Then End
        FechaIngreso = Dt.Rows(0).Item("Fecha")
        Dt.Dispose()

    End Sub
    Private Sub HallaSecuenciaLoteOrigen(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal OperacionAux As Integer, ByRef SecuenciaOrigen As Integer)

        Dim ConexionStr As String
        Dim Dt As New DataTable

        SecuenciaOrigen = 0

        If OperacionAux = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT SecuenciaOrigen FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", ConexionStr, Dt) Then End
        SecuenciaOrigen = Dt.Rows(0).Item("SecuenciaOrigen")

        Dt.Dispose()

    End Sub
    Private Function HallaCostosDelLoteOrigen(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As Integer, ByRef FechaIngreso As Date, ByRef CantidadInicial As Decimal, ByRef KilosXUnidad As Decimal, ByRef Baja As Decimal, ByRef Descarga As Decimal, ByRef DescargaSinIva As Decimal, ByRef OtrosCostos As Decimal, ByRef OtrosCostosSinIva As Decimal, ByRef CostoFruta As Decimal, ByRef CostoFrutaSinIva As Decimal, ByRef Liquidado As Decimal) As Boolean

        Dim Senia As Decimal
        Dim Merma As Decimal
        Dim Stock As Decimal
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
        Liquidado = 0

        If Not CostoDeUnLote(Operacion, Lote, Secuencia, Baja, Merma, Stock, Liquidado, CantidadInicial, KilosXUnidad, FechaIngreso, Senia, Descarga, DescargaSinIva, OtrosCostos, OtrosCostosSinIva, CostoFruta, CostoFrutaSinIva, Total, TotalSinIva, False) Then Return False

        Return True

    End Function
    Private Function HallaLiquidado(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal OperacionAux As Integer) As Decimal

        Dim ConexionStr As String

        If OperacionAux = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Liquidado FROM Lotes WHERE Lote =  " & Lote & " AND Secuencia = " & Secuencia & ";", Miconexion)
                    Return Cmd.ExecuteScalar
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: Lotes.", MsgBoxStyle.Critical)
            End
        End Try

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