Public Class ValorizaLote
    Public POperacion As Integer
    Public PLote As Integer
    Public PSecuencia As Integer
    Public PImporteTotal As Boolean
    Public PSinReintegro As Boolean
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    Private Sub VarolizaLotes2_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGrid()

        ArmaArchivo()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Valorización del Lote " & PLote & "/" & Format(PSecuencia, "000"), "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

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
    Private Sub ArmaArchivo()

        If Not ProcesaLote() Then Me.Close() : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = DtGrid

    End Sub
    Private Function ProcesaLote() As Boolean

        Dim Dt As New DataTable
        Dim DtAux As New DataTable
        Dim RowGrid As DataRow
        Dim ListaBoniComercial As New List(Of Vigencia)
        Dim ListaBoniLogistica As New List(Of Vigencia)
        Dim ListaIngresoBruto As New List(Of Vigencia)
        Dim ListaFletePorBulto As New List(Of Vigencia)
        Dim ListaFletePorMedioBulto As New List(Of Vigencia)
        Dim ListaFletePorUnidad As New List(Of Vigencia)
        Dim ListaFletePorKilo As New List(Of Vigencia)
        Dim ListaImpDebCred As New List(Of Vigencia)
        Dim Importe As Decimal = 0
        Dim Costo As Decimal = 0
        Dim BoniComercial As Decimal = 0
        Dim BoniLogistica As Decimal = 0
        Dim IngresoBruto As Decimal = 0
        Dim Flete As Decimal = 0
        Dim ImpDebCred As Decimal = 0
        Dim PrecioPromedio As Decimal = 0
        Dim GastoComercial As Decimal = 0
        Dim Envase As Integer
        Dim KilosXUnidad As Decimal
        Dim CantidadInicial As Decimal
        Dim EsReventa As Boolean
        Dim EsCosteo As Boolean
        Dim Articulo As Integer
        Dim FechaIngreso As Date
        Dim Proveedor As Integer
        Dim Merma As Decimal
        Dim Baja As Decimal = 0
        Dim Descarte As Decimal = 0
        Dim ImporteT As Decimal
        Dim BoniComercialT As Decimal
        Dim BoniLogisticaT As Decimal
        Dim IngresoBrutoT As Decimal
        Dim ImpDebCredT As Decimal
        Dim FleteT As Decimal
        Dim CostoT As Decimal
        Dim TotalBonificacion As Decimal = 0
        Dim Facturado As Decimal
        Dim NetoParcial As Decimal
        Dim CantidadLote As Decimal
        Dim ImporteLotesReintegrosConIva As Decimal = 0
        Dim ImporteLotesReintegrosSinIva As Decimal = 0

        GListaLotesDeReintegros = New List(Of ItemCostosAsignados)
        GListaLotesDeConsumosTerminados = New List(Of ItemCostosAsignados)
        GConListaDeCostos = True
        GConListaDeVentas = True

        Dim Sql As String = "SELECT * FROM Lotes WHERE Lotes.LoteOrigen = " & PLote & " AND Lotes.SecuenciaOrigen = " & PSecuencia & " ORDER BY Lote,Secuencia;"
        If POperacion = 1 Then
            If Not Tablas.Read(Sql, Conexion, Dt) Then Return False
        Else
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Return False
        End If

        Label1.Text = NombreArticulo(Dt.Rows(0).Item("Articulo")) & "        " & NombreCalibre(Dt.Rows(0).Item("Calibre"))

        For Each Row As DataRow In Dt.Rows
            If Row("Lote") = Row("LoteOrigen") And Row("Secuencia") = Row("SecuenciaOrigen") And Row("Deposito") And Row("DepositoOrigen") Then
                KilosXUnidad = Row("KilosXUnidad")
                Proveedor = Row("Proveedor")
                Articulo = Row("Articulo")
                CantidadInicial = Row("Cantidad")
                FechaIngreso = Row("Fecha")
                Baja = Row("Baja")
                If Row("TipoOperacion") = 2 Then EsReventa = True
                If Row("TipoOperacion") = 4 Then EsCosteo = True
                Envase = HallaEnvase(Row("Articulo"))
                If Envase < 0 Then Return False
            End If
            RowGrid = DtGrid.NewRow
            RowGrid("Concepto") = "Lote "
            RowGrid("Comprobante") = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
            DtGrid.Rows.Add(RowGrid)
            '
            Merma = Merma + Row("Merma") * Row("KilosXUnidad")
            Descarte = Descarte + Row("Descarte") * Row("KilosXUnidad")
            Dim StrLote As String = "C.Lote = " & Row("Lote") & " AND C.Secuencia = " & Row("Secuencia") & " AND C.Deposito = " & Row("Deposito")
            Dim CostoConIvaW As Decimal
            Dim CostoSinIvaW As Decimal
            If Not CalculaCostoEnvase(Row("Articulo"), Row("Fecha"), CostoConIvaW, CostoSinIvaW) Then Return False
            'procesa facturas.
            Dim StrFacturas As String = "SELECT 1 AS Operacion,1 AS Tipo,C.Cantidad AS Cantidad,C.Importe AS Importe,C.ImporteSinIva AS ImporteSinIva, " & _
                                        "F.Factura as Comprobante,F.Cliente AS Cliente,F.Fecha As Fecha,C.Rel FROM AsignacionLotes AS C INNER JOIN FacturasCabeza AS F ON C.Comprobante = F.Factura AND C.TipoComprobante = 2 " & _
                                        "WHERE F.Tr = 0 AND " & StrLote & ";"
            DtAux = New DataTable
            If Not Tablas.Read(StrFacturas, Conexion, DtAux) Then Return False
            If PermisoTotal Then
                StrFacturas = "SELECT 2 AS Operacion,1 AS Tipo,C.Cantidad AS Cantidad,C.Importe AS Importe,C.ImporteSinIva AS ImporteSinIva, " & _
                              "F.Factura as Comprobante,F.Cliente AS Cliente,F.Fecha As Fecha,F.Rel FROM AsignacionLotes AS C INNER JOIN FacturasCabeza AS F ON C.Comprobante = F.Factura AND C.TipoComprobante = 2 " & _
                              "WHERE F.Tr = 0 AND F.Rel = 0 AND " & StrLote & ";"   'Con Rel = 0(false) para sumar cantidad de los que tienen negro solo. 
                If Not Tablas.Read(StrFacturas, ConexionN, DtAux) Then Return False
                StrFacturas = "SELECT 2 AS Operacion,1 AS Tipo,C.Cantidad,C.Importe AS Importe,C.ImporteSinIva AS ImporteSinIva, " & _
                              "F.Factura as Comprobante,F.Cliente AS Cliente,F.Fecha As Fecha,F.Rel FROM AsignacionLotes AS C INNER JOIN FacturasCabeza AS F ON C.Comprobante = F.Factura AND C.TipoComprobante = 2 " & _
                              "WHERE F.Tr = 0 AND F.Rel = 1 AND " & StrLote & ";"   'Con Rel = 1(true) y cantidad = 0 para no sumar dos veces.
                If Not Tablas.Read(StrFacturas, ConexionN, DtAux) Then Return False
            End If
            'Procesa NVLP.
            StrFacturas = "SELECT 1 AS Operacion,2 AS Tipo,C.Cantidad,C.NetoConIva AS Importe,C.NetoSinIva AS ImporteSinIva, " & _
                                        "F.Liquidacion AS Comprobante,F.Cliente AS Cliente,F.Fecha As Fecha,F.Rel FROM NVLPLotes AS C INNER JOIN NVLPCabeza AS F ON C.Liquidacion = F.Liquidacion " & _
                                        "WHERE F.Estado = 1 AND " & StrLote & ";"
            If Not Tablas.Read(StrFacturas, Conexion, DtAux) Then Return False
            If PermisoTotal Then
                StrFacturas = "SELECT 2 AS Operacion,2 AS Tipo,C.Cantidad,C.NetoConIva AS Importe,C.NetoSinIva AS ImporteSinIva, " & _
                              "F.Liquidacion AS Comprobante,F.Cliente AS Cliente,F.Fecha As Fecha,F.Rel FROM NVLPLotes AS C INNER JOIN NVLPCabeza AS F ON C.Liquidacion = F.Liquidacion " & _
                              "WHERE F.Estado = 1 AND F.Rel = 0 AND " & StrLote & ";"   'Con Rel = 0(false) para sumar cantidad de los que tienen negro solo. 
                If Not Tablas.Read(StrFacturas, ConexionN, DtAux) Then Return False
                StrFacturas = "SELECT 2 AS Operacion,2 AS Tipo,Cantidad,C.NetoConIva AS Importe,C.NetoSinIva AS ImporteSinIva, " & _
                              "F.Liquidacion AS Comprobante,F.Cliente AS Cliente,F.Fecha As Fecha,F.Rel FROM NVLPLotes AS C INNER JOIN NVLPCabeza AS F ON C.Liquidacion = F.Liquidacion " & _
                              "WHERE F.Estado = 1 AND F.Rel = 1 AND " & StrLote & ";"   'Con Rel = 1(true) y cantidad = 0 para no sumar dos veces.
                If Not Tablas.Read(StrFacturas, ConexionN, DtAux) Then Return False
            End If
            '
            Dim View As New DataView
            View = DtAux.DefaultView
            View.Sort = "Cliente"
            Dim ClienteAnt As Integer = 0
            '
            For Each Row1 As DataRowView In View
                If Row1("Cliente") <> ClienteAnt Then
                    If Not ArmaLista(ListaBoniComercial, 1, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaBoniLogistica, 2, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaIngresoBruto, 3, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaFletePorBulto, Bulto, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaFletePorMedioBulto, MedioBulto, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaFletePorUnidad, Unidad, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaFletePorKilo, Kilo, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaImpDebCred, 5, Row1("Cliente")) Then Return False
                    ClienteAnt = Row1("Cliente")
                    ' 
                    RowGrid = DtGrid.NewRow
                    RowGrid("Concepto") = "Cliente "
                    RowGrid("Comprobante") = NombreCliente(Row1("Cliente"))
                    DtGrid.Rows.Add(RowGrid)
                    ' 
                End If
                Dim CantidadVendida As Decimal = 0
                If Row1("Operacion") = 1 Then CantidadVendida = Row1("Cantidad") 'Para tomar cantidad una sola vez.
                If Row1("Operacion") = 2 And Not Row1("Rel") Then CantidadVendida = Row1("Cantidad") 'Para tomar cantidad una sola vez.

                Facturado = Facturado + CantidadVendida * Row("KilosXUnidad")
                Dim ImporteW As Decimal = 0
                If PImporteTotal Then
                    ImporteW = Row1("Importe")
                Else : ImporteW = Row1("ImporteSinIva")
                End If
                Importe = Importe + ImporteW
                ImporteT = ImporteT + ImporteW
                '
                Dim BoniComercialAux As Decimal = CalculaIva(1, Row1("ImporteSinIva"), EncuentraVigencia(ListaBoniComercial, Row1("Fecha")))
                Dim BoniComercialW As Decimal = 0
                If PImporteTotal Then
                    BoniComercialW = BoniComercialAux + CalculaIva(1, BoniComercialAux, EncuentraVigenciaAlicuota(ListaBoniComercial, Row1("Fecha")))
                Else : BoniComercialW = BoniComercialAux
                End If
                BoniComercial = BoniComercial + BoniComercialW
                BoniComercialT = BoniComercialT + BoniComercialW
                '
                Dim BoniLogisticaAux As Decimal = CalculaIva(1, Row1("ImporteSinIva"), EncuentraVigencia(ListaBoniLogistica, Row1("Fecha")))
                Dim BoniLogisticaW As Decimal = 0
                If PImporteTotal Then
                    BoniLogisticaW = BoniLogisticaAux + CalculaIva(1, BoniLogisticaAux, EncuentraVigenciaAlicuota(ListaBoniLogistica, Row1("Fecha")))
                Else : BoniLogisticaW = BoniLogisticaAux
                End If
                BoniLogistica = BoniLogistica + BoniLogisticaW
                BoniLogisticaT = BoniLogisticaT + BoniLogisticaW

                Dim IngresoBrutoW As Decimal = CalculaIva(1, ImporteW, EncuentraVigencia(ListaIngresoBruto, Row1("Fecha")))
                IngresoBruto = IngresoBruto + IngresoBrutoW
                IngresoBrutoT = IngresoBrutoT + IngresoBrutoW
                Dim ImpDebCredW As Decimal = CalculaIva(1, ImporteW, EncuentraVigencia(ListaImpDebCred, Row1("Fecha")))
                ImpDebCred = ImpDebCred + ImpDebCredW
                ImpDebCredT = ImpDebCredT + ImpDebCredW
                Dim FleteW As Decimal = 0
                Dim CostoW As Decimal = 0
                Dim FleteConIvaWWW As Decimal = 0
                Dim FleteSinIvaWWW As Decimal = 0
                If Not CalculaFlete(ListaFletePorBulto, ListaFletePorMedioBulto, ListaFletePorUnidad, ListaFletePorKilo, Row("Articulo"), Row1("Fecha"), FleteConIvaWWW, FleteSinIvaWWW) Then
                    Return False
                End If
                If PImporteTotal Then
                    FleteW = CalculaNeto(CantidadVendida, FleteConIvaWWW)
                    Flete = Flete + FleteW
                    FleteT = FleteT + FleteW
                    CostoW = CalculaNeto(CantidadVendida, CostoConIvaW)
                    Costo = Costo + CostoW
                    CostoT = CostoT + CostoW
                Else
                    FleteW = CalculaNeto(CantidadVendida, FleteSinIvaWWW)
                    FleteT = FleteT + FleteW
                    Flete = Flete + FleteW
                    CostoW = CalculaNeto(CantidadVendida, CostoSinIvaW)
                    Costo = Costo + CostoW
                    CostoT = CostoT + CostoW
                End If
                ' 
                RowGrid = DtGrid.NewRow
                If Row1("Tipo") = 1 Then
                    RowGrid("Concepto") = "Factura"
                    RowGrid("TipoComprobante") = 2
                Else : RowGrid("Concepto") = "NVLP"
                    RowGrid("TipoComprobante") = 800
                End If
                RowGrid("Operacion") = Row1("Operacion")
                RowGrid("Comprobante") = NumeroEditado(Row1("Comprobante"))
                RowGrid("NroComprobante") = Row1("Comprobante")
                RowGrid("Cantidad") = FormatNumber(Row1("Cantidad"), GDecimales)
                If Row1("Operacion") = 2 And Row1("Rel") Then RowGrid("Cantidad") = 0
                CantidadLote = CantidadLote + CantidadVendida
                RowGrid("Importe") = FormatNumber(ImporteW, GDecimales)
                RowGrid("BoniComercial") = FormatNumber(BoniComercialW, GDecimales)
                RowGrid("BoniLogistica") = FormatNumber(BoniLogisticaW, GDecimales)
                RowGrid("IngresoBruto") = FormatNumber(IngresoBrutoW, GDecimales)
                RowGrid("ImpDebCred") = FormatNumber(ImpDebCredW, GDecimales)
                RowGrid("Flete") = FormatNumber(FleteW, GDecimales)
                RowGrid("Costo") = FormatNumber(CostoW, GDecimales)
                If Row1("Cantidad") <> 0 Then PrecioPromedio = ImporteW / Row1("Cantidad")
                RowGrid("PrecioPromedio") = FormatNumber(PrecioPromedio, GDecimales)
                NetoParcial = RowGrid("Importe") - RowGrid("BoniComercial") - RowGrid("BoniLogistica") - RowGrid("IngresoBruto") - RowGrid("ImpDebCred") - RowGrid("Flete") - RowGrid("Costo")
                RowGrid("Neto") = FormatNumber(NetoParcial, GDecimales)
                DtGrid.Rows.Add(RowGrid)
            Next
            'Halla costo de consumos de lotes productos Terminados.
            Dim LotesConsumosTerminados As Decimal = 0
            Dim CantidadLotesConsumosTerminados As Decimal = 0
            GListaLotesDeConsumosTerminados = New List(Of ItemCostosAsignados)
            HallaImportesLotesConsumosTermindos(Row("Lote"), Row("Secuencia"), LotesConsumosTerminados, CantidadLotesConsumosTerminados)
            Importe = Importe + LotesConsumosTerminados
            ImporteT = ImporteT + LotesConsumosTerminados
            CantidadLote = CantidadLote + CantidadLotesConsumosTerminados
            Facturado = Facturado + CantidadLotesConsumosTerminados * KilosXUnidad
            If GListaLotesDeConsumosTerminados.Count <> 0 Then
                For Each Item As ItemCostosAsignados In GListaLotesDeConsumosTerminados
                    RowGrid = DtGrid.NewRow
                    RowGrid("Operacion") = Item.Operacion
                    RowGrid("Cantidad") = FormatNumber(Item.Cantidad, GDecimales)
                    RowGrid("Concepto") = Item.Nombre
                    RowGrid("Comprobante") = Item.Comprobante
                    RowGrid("NroComprobante") = Item.NroComprobante
                    RowGrid("TipoComprobante") = 10
                    RowGrid("Importe") = FormatNumber(Item.ImporteConIva, GDecimales)
                    If Item.Cantidad <> 0 Then PrecioPromedio = Item.ImporteConIva / Item.Cantidad
                    RowGrid("PrecioPromedio") = FormatNumber(PrecioPromedio, GDecimales)
                    NetoParcial = Item.ImporteConIva
                    RowGrid("Neto") = FormatNumber(NetoParcial, GDecimales)
                    DtGrid.Rows.Add(RowGrid)
                Next
            End If
            '
            RowGrid = DtGrid.NewRow
            RowGrid("Concepto") = "Sub-Total Lote"
            RowGrid("Cantidad") = FormatNumber(CantidadLote, GDecimales)
            RowGrid("Importe") = FormatNumber(Importe, GDecimales)
            RowGrid("BoniComercial") = FormatNumber(BoniComercial, GDecimales)
            RowGrid("BoniLogistica") = FormatNumber(BoniLogistica, GDecimales)
            RowGrid("IngresoBruto") = FormatNumber(IngresoBruto, GDecimales)
            RowGrid("ImpDebCred") = FormatNumber(ImpDebCred, GDecimales)
            RowGrid("Flete") = FormatNumber(Flete, GDecimales)
            RowGrid("Costo") = FormatNumber(Costo, GDecimales)
            If CantidadLote <> 0 Then
                PrecioPromedio = Importe / CantidadLote
            Else : PrecioPromedio = 0
            End If
            RowGrid("PrecioPromedio") = FormatNumber(PrecioPromedio, GDecimales)
            NetoParcial = RowGrid("Importe") - RowGrid("BoniComercial") - RowGrid("BoniLogistica") - RowGrid("IngresoBruto") - RowGrid("ImpDebCred") - RowGrid("Flete") - RowGrid("Costo")
            RowGrid("Neto") = FormatNumber(NetoParcial, GDecimales)
            DtGrid.Rows.Add(RowGrid)
            '
            RowGrid = DtGrid.NewRow
            DtGrid.Rows.Add(RowGrid)
            '
            Importe = 0
            BoniComercial = 0
            BoniLogistica = 0
            IngresoBruto = 0
            ImpDebCred = 0
            Flete = 0
            Costo = 0
            CantidadLote = 0
            Dim LotesReintegrosConIva As Decimal = 0
            Dim LotesReintegrosSinIva As Decimal = 0
            If Not PSinReintegro Then
                HallaImportesLotesReintegros(Row("Lote"), Row("Secuencia"), LotesReintegrosConIva, LotesReintegrosSinIva)
                ImporteLotesReintegrosConIva = ImporteLotesReintegrosConIva + LotesReintegrosConIva
                ImporteLotesReintegrosSinIva = ImporteLotesReintegrosSinIva + LotesReintegrosSinIva
            End If
        Next

        Facturado = Trunca(Facturado / KilosXUnidad)
        '
        RowGrid = DtGrid.NewRow
        RowGrid("Concepto") = "Total Lote"
        RowGrid("Cantidad") = FormatNumber(Facturado, GDecimales)
        RowGrid("Importe") = FormatNumber(ImporteT, GDecimales)
        RowGrid("BoniComercial") = FormatNumber(BoniComercialT, GDecimales)
        RowGrid("BoniLogistica") = FormatNumber(BoniLogisticaT, GDecimales)
        RowGrid("IngresoBruto") = FormatNumber(IngresoBrutoT, GDecimales)
        RowGrid("ImpDebCred") = FormatNumber(ImpDebCredT, GDecimales)
        RowGrid("Flete") = FormatNumber(FleteT, GDecimales)
        RowGrid("Costo") = FormatNumber(CostoT, GDecimales)
        If Facturado <> 0 Then
            PrecioPromedio = ImporteT / Facturado
        Else : PrecioPromedio = 0
        End If
        RowGrid("PrecioPromedio") = FormatNumber(PrecioPromedio, GDecimales)
        TotalBonificacion = BoniComercialT + BoniLogisticaT + IngresoBrutoT + ImpDebCredT + FleteT + CostoT
        RowGrid("Neto") = FormatNumber(ImporteT - TotalBonificacion)

        DtGrid.Rows.Add(RowGrid)

        'costos asignados..........................................
        Dim CostoAsignado As Decimal = 0
        Dim CostoAsignadoConIva As Decimal = 0
        Dim CostoAsignadoSinIva As Decimal = 0
        If Not FacturaAfectaLotes(PLote, PSecuencia, CostoAsignadoConIva, CostoAsignadoSinIva) Then Return False
        If PImporteTotal Then
            CostoAsignado = CostoAsignadoConIva
        Else : CostoAsignado = CostoAsignadoSinIva
        End If
        '
        If GListaCostosAsignados.Count <> 0 Then
            RowGrid = DtGrid.NewRow
            DtGrid.Rows.Add(RowGrid)
            RowGrid = DtGrid.NewRow
            RowGrid("Concepto") = "Costos Asignados"
            DtGrid.Rows.Add(RowGrid)
            For Each Item As ItemCostosAsignados In GListaCostosAsignados
                RowGrid = DtGrid.NewRow
                RowGrid("Operacion") = Item.Operacion
                RowGrid("Concepto") = Item.Nombre
                RowGrid("Comprobante") = Item.Comprobante
                RowGrid("NroComprobante") = Item.NroComprobante
                RowGrid("TipoComprobante") = 1
                If PImporteTotal Then
                    RowGrid("Importe") = FormatNumber(Item.ImporteConIva, GDecimales)
                Else : RowGrid("Importe") = FormatNumber(Item.ImporteSinIva, GDecimales)
                End If
                DtGrid.Rows.Add(RowGrid)
            Next
        End If
        '
        'Importes Lotes en Recibos..........................................
        Dim ImporteLotesRecibosConIva As Decimal = 0
        Dim ImporteLotesRecibosSinIva As Decimal = 0
        HallaImportesLotesRecibos(PLote, PSecuencia, ImporteLotesRecibosConIva, ImporteLotesRecibosSinIva)
        If PImporteTotal Then
            CostoAsignado = CostoAsignado + ImporteLotesRecibosConIva
        Else : CostoAsignado = CostoAsignado + ImporteLotesRecibosSinIva
        End If
        '
        If GListaLotesDeRecibos.Count <> 0 Then
            RowGrid = DtGrid.NewRow
            DtGrid.Rows.Add(RowGrid)
            RowGrid = DtGrid.NewRow
            RowGrid("Concepto") = "Debitos/Creditos"
            DtGrid.Rows.Add(RowGrid)
            For Each Item As ItemCostosAsignados In GListaLotesDeRecibos
                RowGrid = DtGrid.NewRow
                RowGrid("Operacion") = Item.Operacion
                RowGrid("Concepto") = Item.Nombre
                RowGrid("Comprobante") = Item.Comprobante
                RowGrid("NroComprobante") = Item.NroComprobante
                RowGrid("TipoComprobante") = Item.TipoComprobante
                If PImporteTotal Then
                    RowGrid("Importe") = FormatNumber(Item.ImporteConIva, GDecimales)
                Else : RowGrid("Importe") = FormatNumber(Item.ImporteSinIva, GDecimales)
                End If
                DtGrid.Rows.Add(RowGrid)
            Next
        End If
        '
        'Importes Lotes en Reinteros Aduana..........................................
        If Not PSinReintegro Then
            If PImporteTotal Then
                CostoAsignado = CostoAsignado - ImporteLotesReintegrosConIva
            Else : CostoAsignado = CostoAsignado - ImporteLotesReintegrosSinIva
            End If
            '
            If GListaLotesDeReintegros.Count <> 0 Then
                RowGrid = DtGrid.NewRow
                DtGrid.Rows.Add(RowGrid)
                RowGrid = DtGrid.NewRow
                RowGrid("Concepto") = "Reintegros Aduana"
                DtGrid.Rows.Add(RowGrid)
                For Each Item As ItemCostosAsignados In GListaLotesDeReintegros
                    RowGrid = DtGrid.NewRow
                    RowGrid("Operacion") = Item.Operacion
                    RowGrid("Concepto") = Item.Nombre
                    RowGrid("Comprobante") = Item.Comprobante
                    RowGrid("TipoComprobante") = 12006
                    If PImporteTotal Then
                        RowGrid("Importe") = -FormatNumber(Item.ImporteConIva, GDecimales)
                    Else : RowGrid("Importe") = -FormatNumber(Item.ImporteSinIva, GDecimales)
                    End If
                    DtGrid.Rows.Add(RowGrid)
                Next
            End If
        End If
        '
        'Importes Lotes en Otras Facturas..........................................
        Dim ImporteLotesOtrasFacturasConIva As Decimal = 0
        Dim ImporteLotesOtrasfacturasSinIva As Decimal = 0
        HallaImportesLotesOtrasFacturas(PLote, PSecuencia, ImporteLotesOtrasFacturasConIva, ImporteLotesOtrasfacturasSinIva)
        If PImporteTotal Then
            CostoAsignado = CostoAsignado + ImporteLotesOtrasFacturasConIva
        Else : CostoAsignado = CostoAsignado + ImporteLotesOtrasfacturasSinIva
        End If
        '
        If GListaLotesDeOtrasFacturas.Count <> 0 Then
            RowGrid = DtGrid.NewRow
            DtGrid.Rows.Add(RowGrid)
            RowGrid = DtGrid.NewRow
            RowGrid("Concepto") = "Fact.Otros Proveedores"
            DtGrid.Rows.Add(RowGrid)
            For Each Item As ItemCostosAsignados In GListaLotesDeOtrasFacturas
                RowGrid = DtGrid.NewRow
                RowGrid("Operacion") = Item.Operacion
                RowGrid("Concepto") = Item.Nombre
                RowGrid("Comprobante") = Item.Comprobante
                RowGrid("TipoComprobante") = 5000
                If PImporteTotal Then
                    RowGrid("Importe") = FormatNumber(Item.ImporteConIva, GDecimales)
                Else : RowGrid("Importe") = FormatNumber(Item.ImporteSinIva, GDecimales)
                End If
                DtGrid.Rows.Add(RowGrid)
            Next
        End If
        '
        'Importes Lotes en Asientos Manuales..........................................
        Dim ImporteLotesasientosmanualesConIva As Decimal = 0
        Dim ImporteLotesAsientosManualesSinIva As Decimal = 0
        HallaImportesLotesAsientosManuales(PLote, PSecuencia, ImporteLotesasientosmanualesConIva, ImporteLotesAsientosManualesSinIva)
        If PImporteTotal Then
            CostoAsignado = CostoAsignado + ImporteLotesasientosmanualesConIva
        Else : CostoAsignado = CostoAsignado + ImporteLotesAsientosManualesSinIva
        End If
        '
        If GListaLotesDeAsientosManuales.Count <> 0 Then
            RowGrid = DtGrid.NewRow
            DtGrid.Rows.Add(RowGrid)
            RowGrid = DtGrid.NewRow
            RowGrid("Concepto") = "Asientos Manuales"
            DtGrid.Rows.Add(RowGrid)
            For Each Item As ItemCostosAsignados In GListaLotesDeAsientosManuales
                RowGrid = DtGrid.NewRow
                RowGrid("Operacion") = Item.Operacion
                RowGrid("Concepto") = Item.Nombre
                RowGrid("Comprobante") = Item.Comprobante
                RowGrid("TipoComprobante") = 70000
                If PImporteTotal Then
                    RowGrid("Importe") = FormatNumber(Item.ImporteConIva, GDecimales)
                Else : RowGrid("Importe") = FormatNumber(Item.ImporteSinIva, GDecimales)
                End If
                DtGrid.Rows.Add(RowGrid)
            Next
        End If
        '
        'Importes Mermas Positivas de NVLP.................................................
        Dim ImporteMermasNVLPConIva As Decimal = 0
        Dim ImporteMermasNVLPSinIva As Decimal = 0
        HallaImporteMermasNVLP(PLote, PSecuencia, ImporteMermasNVLPConIva, ImporteMermasNVLPSinIva)
        If PImporteTotal Then
            CostoAsignado = CostoAsignado + ImporteMermasNVLPConIva
        Else : CostoAsignado = CostoAsignado + ImporteMermasNVLPSinIva
        End If
        '
        If GListaMermaPositivasNVLP.Count <> 0 Then
            RowGrid = DtGrid.NewRow
            DtGrid.Rows.Add(RowGrid)
            RowGrid = DtGrid.NewRow
            RowGrid("Concepto") = "Merma Positiva N.V.L.P."
            For Each Item As ItemCostosAsignados In GListaMermaPositivasNVLP
                RowGrid = DtGrid.NewRow
                RowGrid("Operacion") = Item.Operacion
                RowGrid("Concepto") = Item.Nombre
                RowGrid("Comprobante") = Item.Comprobante
                RowGrid("TipoComprobante") = 800
                If PImporteTotal Then
                    RowGrid("Importe") = FormatNumber(Item.ImporteConIva, GDecimales)
                Else : RowGrid("Importe") = FormatNumber(Item.ImporteSinIva, GDecimales)
                End If
                DtGrid.Rows.Add(RowGrid)
            Next
        End If
        '
        'costos por insumos........................................
        Dim CostoInsumos As Decimal = 0
        Dim InsumosConIva As Decimal = 0
        Dim InsumosSinIva As Decimal = 0
        If HallaCostoInsumosPorLote(PLote, PSecuencia, InsumosConIva, InsumosSinIva) < 0 Then Return False
        If PImporteTotal Then
            CostoInsumos = InsumosConIva
        Else : CostoInsumos = InsumosSinIva
        End If
        '
        If GListaCostosInsumos.Count <> 0 Then
            RowGrid = DtGrid.NewRow
            DtGrid.Rows.Add(RowGrid)
            RowGrid = DtGrid.NewRow
            RowGrid("Concepto") = "Costos Insumos"
            DtGrid.Rows.Add(RowGrid)
            For Each Item As ItemCostosAsignados In GListaCostosInsumos
                RowGrid = DtGrid.NewRow
                RowGrid("Operacion") = Item.Operacion
                RowGrid("Concepto") = Item.Nombre
                RowGrid("Comprobante") = Item.Comprobante
                RowGrid("TipoComprobante") = 60000
                If PImporteTotal Then
                    RowGrid("Importe") = FormatNumber(Item.ImporteConIva, GDecimales)
                Else : RowGrid("Importe") = FormatNumber(Item.ImporteSinIva, GDecimales)
                End If
                DtGrid.Rows.Add(RowGrid)
            Next
        End If

        '
        'costos del Costeo.........................................ANULADO
        Dim CostoDelCosteo As Decimal = 0
        Dim CostoDelCosteoConIva As Decimal = 0
        Dim CostoDelCosteoSinIva As Decimal = 0
        '       If EsCosteo Then
        '     If Not HallaCostoCosteoXKilo(POperacion, PLote, CostoDelCosteoConIva, CostoDelCosteoSinIva) Then Return False
        '     CostoDelCosteoConIva = CostoDelCosteoConIva * (CantidadInicial - Baja) * KilosXUnidad
        '     CostoDelCosteoSinIva = CostoDelCosteoSinIva * (CantidadInicial - Baja) * KilosXUnidad
        '     End If
        '    If PImporteTotal Then
        ' CostoDelCosteo = CostoDelCosteoConIva
        ' Else : CostoDelCosteo = CostoDelCosteoSinIva
        ' End If

        GastoComercial = BoniComercialT + BoniLogisticaT + IngresoBrutoT + ImpDebCredT + FleteT + CostoT
        Dim Costos As Decimal = CostoAsignado + CostoInsumos + CostoDelCosteo

        'Halla costo de descarga.
        Dim Descarga As Decimal
        Dim DescargaW As Decimal
        Dim DescargaSinIvaW As Decimal
        If Not BuscaVigenciaValorAlicuota(11, FechaIngreso, DescargaW, DescargaSinIvaW, Envase) Then Return False
        If PImporteTotal Then
            Descarga = DescargaW
        Else : Descarga = DescargaSinIvaW
        End If

        Descarga = CalculaNeto(CantidadInicial, Descarga)
        Merma = Trunca(Merma / KilosXUnidad)
        Descarte = Trunca(Descarte / KilosXUnidad)

        Dim ComisionAdicional As Decimal = HallaComisionAdicional(Proveedor)
        If ComisionAdicional < 0 Then Return False
        Dim ImporteComisionAdicional As Decimal = Trunca(ImporteT * ComisionAdicional / 100)

        Dim CantidadW As Decimal = 0
        If EsReventa Or EsCosteo Then
            CantidadW = CantidadInicial - Baja
        Else
            CantidadW = CantidadInicial - Merma - Descarte - Baja
        End If

        Dim PrecioS As Decimal
        Dim Neto As Decimal

        If EsReventa Or EsCosteo Then
            Neto = (ImporteT - ImporteComisionAdicional - GastoComercial - Descarga - Costos)
        Else
            Neto = (ImporteT - ImporteComisionAdicional - GastoComercial - Costos)
        End If

        If CantidadW > 0 Then
            PrecioS = Trunca(Neto / CantidadW)
        End If
        '
        RowGrid = DtGrid.NewRow
        DtGrid.Rows.Add(RowGrid)
        '
        RowGrid = DtGrid.NewRow
        RowGrid("Color") = 1
        RowGrid("Concepto") = "Total Facturado"
        RowGrid("Importe") = FormatNumber(ImporteT, GDecimales)
        DtGrid.Rows.Add(RowGrid)
        '
        If TotalBonificacion <> 0 Then
            RowGrid = DtGrid.NewRow
            RowGrid("Color") = 1
            RowGrid("Concepto") = "Total Bonificación"
            RowGrid("Importe") = FormatNumber(TotalBonificacion, GDecimales)
            DtGrid.Rows.Add(RowGrid)
        End If
        '
        If ImporteComisionAdicional <> 0 Then
            RowGrid = DtGrid.NewRow
            RowGrid("Color") = 1
            RowGrid("Concepto") = "Comisión Adicional"
            RowGrid("Importe") = FormatNumber(ImporteComisionAdicional, GDecimales)
            DtGrid.Rows.Add(RowGrid)
        End If
        '
        If EsReventa Then
            If Descarga <> 0 Then
                RowGrid = DtGrid.NewRow
                RowGrid("Color") = 1
                RowGrid("Concepto") = "Descarga"
                RowGrid("Importe") = FormatNumber(Descarga, GDecimales)
                DtGrid.Rows.Add(RowGrid)
            End If
        End If
        '
        If CostoAsignado <> 0 Then
            RowGrid = DtGrid.NewRow
            RowGrid("Color") = 1
            RowGrid("Concepto") = "Costo Asignado"
            RowGrid("Importe") = FormatNumber(CostoAsignado, GDecimales)
            DtGrid.Rows.Add(RowGrid)
        End If
        '
        If CostoInsumos <> 0 Then
            RowGrid = DtGrid.NewRow
            RowGrid("Color") = 1
            RowGrid("Concepto") = "Costo Insumos"
            RowGrid("Importe") = FormatNumber(CostoInsumos, GDecimales)
            DtGrid.Rows.Add(RowGrid)
        End If
        '
        If CostoDelCosteo <> 0 Then
            RowGrid = DtGrid.NewRow
            RowGrid("Color") = 1
            RowGrid("Concepto") = "Costo del Costeo"
            RowGrid("Importe") = FormatNumber(CostoDelCosteo, GDecimales)
            '     DtGrid.Rows.Add(RowGrid)
        End If
        '
        RowGrid = DtGrid.NewRow
        RowGrid("Color") = 1
        RowGrid("Concepto") = "Neto"
        RowGrid("Importe") = FormatNumber(Neto, GDecimales)
        DtGrid.Rows.Add(RowGrid)
        '
        RowGrid = DtGrid.NewRow
        RowGrid("Color") = 1
        RowGrid("Concepto") = "Cantidad Neta Lote Origen"
        RowGrid("Importe") = FormatNumber(CantidadInicial, GDecimales)
        DtGrid.Rows.Add(RowGrid)
        '
        If Baja <> 0 Then
            RowGrid = DtGrid.NewRow
            RowGrid("Color") = 1
            RowGrid("Concepto") = "Devueltas"
            RowGrid("Importe") = FormatNumber(Baja, GDecimales)
            DtGrid.Rows.Add(RowGrid)
        End If
        '
        If Merma <> 0 Then
            RowGrid = DtGrid.NewRow
            RowGrid("Color") = 1
            RowGrid("Concepto") = "Cantidad Merma"
            RowGrid("Importe") = FormatNumber(Merma, GDecimales)
            DtGrid.Rows.Add(RowGrid)
        End If
        '
        If Descarte <> 0 Then
            RowGrid = DtGrid.NewRow
            RowGrid("Color") = 1
            RowGrid("Concepto") = "Cantidad Descarte"
            RowGrid("Importe") = FormatNumber(Descarte, GDecimales)
            DtGrid.Rows.Add(RowGrid)
        End If
        '
        RowGrid = DtGrid.NewRow
        RowGrid("Color") = 1
        RowGrid("Concepto") = "Cantidad Neta"
        RowGrid("Importe") = FormatNumber(CantidadW, GDecimales)
        DtGrid.Rows.Add(RowGrid)
        '
        RowGrid = DtGrid.NewRow
        RowGrid("Color") = 1
        RowGrid("Concepto") = "Precio Sugerido"
        RowGrid("Importe") = FormatNumber(PrecioS, GDecimales)
        DtGrid.Rows.Add(RowGrid)
        '
        Dim PrecioL As Decimal = HallaPrecioDeListaDePrecios(Proveedor, PLote, FechaIngreso, Articulo, KilosXUnidad, POperacion)
        If PrecioL <> 0 Then
            RowGrid = DtGrid.NewRow
            DtGrid.Rows.Add(RowGrid)
            RowGrid = DtGrid.NewRow
            RowGrid("Color") = 1
            RowGrid("Concepto") = "Precio de Lista"
            RowGrid("Importe") = FormatNumber(PrecioL, GDecimales)
            DtGrid.Rows.Add(RowGrid)
        End If
        '
        DtAux.Dispose()
        Dt.Dispose()

        GConListaDeCostos = False
        GConListaDeVentas = False

        GListaLotesDeRecibos = Nothing
        GListaLotesDeReintegros = Nothing
        GListaLotesDeOtrasFacturas = Nothing
        GListaLotesDeAsientosManuales = Nothing
        GListaCostosInsumos = Nothing
        GListaCostosAsignados = Nothing
        GListaMermaPositivasNVLP = Nothing
        GListaLotesDeConsumosTerminados = Nothing

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

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Concepto)

        Dim TipoComprobante As New DataColumn("TipoComprobante")
        TipoComprobante.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoComprobante)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comprobante)

        Dim NroComprobante As New DataColumn("NroComprobante")
        NroComprobante.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(NroComprobante)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Importe)

        Dim BoniComercial As New DataColumn("BoniComercial")
        BoniComercial.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(BoniComercial)

        Dim BoniLogistica As New DataColumn("BoniLogistica")
        BoniLogistica.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(BoniLogistica)

        Dim ImporteIva As New DataColumn("ImporteIva")
        ImporteIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ImporteIva)

        Dim IngresoBruto As New DataColumn("IngresoBruto")
        IngresoBruto.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(IngresoBruto)

        Dim ImpDebCred As New DataColumn("ImpDebCred")
        ImpDebCred.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ImpDebCred)

        Dim Flete As New DataColumn("Flete")
        Flete.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Flete)

        Dim Costo As New DataColumn("Costo")
        Costo.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Costo)

        Dim Neto As New DataColumn("Neto")
        Neto.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Neto)

        Dim PrecioPromedio As New DataColumn("PrecioPromedio")
        PrecioPromedio.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioPromedio)

    End Sub
    Public Function NombreCalibre(ByVal Calibre As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Tipo = 5 AND Clave = " & Calibre & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: Tablas. " & ex.Message, MsgBoxStyle.Critical)
            End
        End Try


    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
            If PermisoTotal And Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Color").Value) Then
                Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.Yellow
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name <> "Comprobante" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
                If IsNumeric(e.Value) Then
                    If e.Value = 0 Then
                        e.Value = ""
                    Else
                        e.Value = FormatNumber(e.Value, GDecimales)
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Not Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then Exit Sub
        If IsDBNull(Grid.CurrentRow.Cells("TipoComprobante").Value) Then Exit Sub
        If IsDBNull(Grid.CurrentRow.Cells("Comprobante").Value) Then Exit Sub

        Dim Abierto As Boolean
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        Select Case Grid.CurrentRow.Cells("TipoComprobante").Value
            Case 1
                UnaFacturaProveedor.PFactura = Grid.CurrentRow.Cells("NroComprobante").Value
                UnaFacturaProveedor.PAbierto = Abierto
                UnaFacturaProveedor.PBloqueaFunciones = True
                UnaFacturaProveedor.ShowDialog()
            Case 2
                If EsExterior(Grid.CurrentRow.Cells("NroComprobante").Value) Then
                    MsgBox("Factura Disponible en Exportación.")
                    Exit Sub
                End If
                UnaFactura.PFactura = Grid.CurrentRow.Cells("NroComprobante").Value
                UnaFactura.PAbierto = Abierto
                UnaFactura.PBloqueaFunciones = True
                UnaFactura.ShowDialog()
            Case 5, 6, 7, 8, 50, 70, 500, 700
                UnRecibo.PTipoNota = Grid.CurrentRow.Cells("TipoComprobante").Value
                UnRecibo.PNota = Grid.CurrentRow.Cells("NroComprobante").Value
                UnRecibo.PAbierto = Abierto
                UnRecibo.PBloqueaFunciones = True
                UnRecibo.ShowDialog()
            Case 800
                UnaNVLP.PLiquidacion = Grid.CurrentRow.Cells("NroComprobante").Value
                UnaNVLP.PAbierto = Abierto
                UnaNVLP.PBloqueaFunciones = True
                UnaNVLP.ShowDialog()
                UnaNVLP.Dispose()
            Case 5000
                UnaFacturaOtrosProveedores.PRecibo = Grid.CurrentRow.Cells("Comprobante").Value
                UnaFacturaOtrosProveedores.PAbierto = Abierto
                UnaFacturaOtrosProveedores.PBloqueaFunciones = True
                UnaFacturaOtrosProveedores.ShowDialog()
                UnaFacturaOtrosProveedores.Dispose()
            Case 60000
                UnConsumoDeInsumo.PConsumo = Grid.CurrentRow.Cells("Comprobante").Value
                UnConsumoDeInsumo.PAbierto = Abierto
                UnConsumoDeInsumo.PBloqueaFunciones = True
                UnConsumoDeInsumo.ShowDialog()
                UnConsumoDeInsumo.Dispose()
            Case 70000
                UnAsiento.PAsiento = Grid.CurrentRow.Cells("Comprobante").Value
                UnAsiento.PAbierto = Abierto
                UnAsiento.PBloqueaFunciones = True
                UnAsiento.ShowDialog()
                UnAsiento.Dispose()
            Case 10
                UnConsumoPT.PConsumo = Grid.CurrentRow.Cells("Comprobante").Value
                UnConsumoPT.PAbierto = Abierto
                UnConsumoPT.PBloqueaFunciones = True
                UnConsumoPT.ShowDialog()
                UnConsumoPT.Dispose()
            Case 12006
                MsgBox("Reintegro Disponible en Exportación.")
                Exit Sub
        End Select

    End Sub


End Class