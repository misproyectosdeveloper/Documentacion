Imports System.IO
Public Class ListaSICORE
    Dim Dt As DataTable
    Dim DtTipo As DataTable
    Dim DtTipoComprobante As DataTable
    Dim TablaIvas(0) As ItemIvaReten
    Dim DtPercepciones As DataTable
    Dim DtGastosGrabados As DataTable
    Dim DtGastosBancarios As DataTable
    Dim View As DataView
    Dim Directorio1 As String
    Dim Directorio2 As String
    Dim Directorio3 As String
    '
    Dim ImporteGrabado As Decimal = 0
    Dim ImporteNoGrabado As Decimal = 0
    Dim ImporteExento As Decimal = 0
    Dim ImportePercepcionIva As Decimal = 0
    Dim ImportePercepcionesOtrosImpuestosNacionales As Decimal = 0
    Dim ImportePercepcionIngresoBruto As Decimal = 0
    Dim ImportePercepcionesImpuestosMunicipales As Decimal = 0
    Dim ImportePercepcionesImpuestosInternos As Decimal = 0
    Dim ImportePercepcionesNoCategorizada As Decimal = 0
    Dim ImporteIva As Decimal = 0
    Dim IvaComision As Decimal = 0
    Dim ImporteOtrosTributos As Decimal = 0
    Dim ImportePercepciones As Decimal = 0
    Dim CantidadIvas As Integer = 0
    Dim Opcion1LibrosIva As Boolean
    Private Sub ListaSICORE_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ArmaTablaIvaConImporte(TablaIvas)
        DtGastosGrabados = New DataTable
        If Not Tablas.Read("SELECT Clave,Activo2 AS Grabado,Operador FROM Tablas WHERE Tipo = 33 or Tipo = 32;", Conexion, DtGastosGrabados) Then
            Me.Close()
            Exit Sub
        End If

        DtGastosBancarios = ArmaConceptosParaGastosBancarios(1, True)

        DtPercepciones = New DataTable
        DtPercepciones = Tablas.Leer("SELECT Clave,OrigenPercepcion FROM Tablas Where Tipo = 25 AND CodigoRetencion = 2;")

        GExcepcion = HallaDatoGenerico("SELECT Opcion1LibrosIva FROM DatosEmpresa WHERE Indice = 1;", Conexion, Opcion1LibrosIva)
        If Not IsNothing(GExcepcion) Then
            MsgBox("Error al Leer Tabla: DatosEmpresa." + vbCrLf + vbCrLf + GExcepcion.Message)
            Me.Close() : Exit Sub
        End If

        Select Case Opcion1LibrosIva
            Case True
                Label10.Text = "Método de Calculo Según Opción 1."
            Case False
                Label10.Text = "Método de Calculo Según Opción 2."
        End Select

    End Sub
    Private Sub ListaSICORE_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptarCompras_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptarCompras.Click

        Directorio1 = ""
        Directorio2 = ""

        Directorio1 = PideCarpeta()
        If Directorio1 = "" Then Exit Sub
        Directorio2 = Directorio1
        Directorio1 = Directorio1 & "\" & "REGINFO_CV_COMPRAS_CBTE.TXT"
        Directorio2 = Directorio2 & "\" & "REGINFO_CV_COMPRAS_ALICUOTAS.TXT"

        Using writer As StreamWriter = New StreamWriter(Directorio1)                        'Borra Archivo.
        End Using
        Using writer As StreamWriter = New StreamWriter(Directorio2)                        'Borra Archivo.
        End Using

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Hasta debe ser mayor o igual a Fecha Desde.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim SqlFecha As String = ""
        SqlFecha = "F.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaContable As String = ""
        SqlFechaContable = "F.FechaContable >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.FechaContable < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaOficial As String = ""
        SqlFechaOficial = "F.FechaReciboOficial >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.FechaReciboOficial < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim Sql As String
        Sql = "SELECT 1 AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial AS Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Impuesto AS Concepto,0 AS Neto,0 AS Iva,D.Importe,F.Cambio,F.Factura AS Rec,C.Pais,F.ConceptoGasto AS Gasto,F.Rendicion,'' AS CompStr,F.Importe AS ImporteTotal FROM Proveedores AS C INNER JOIN (FacturasProveedorCabeza AS F INNER JOIN FacturasProveedorDetalle AS D " & _
                      "ON F.Factura = D.Factura) ON F.Proveedor = C.Clave WHERE F.Estado <> 3 AND " & SqlFechaContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,F.Nota As Comprobante,CAST(FLOOR(CAST(F.Fecha AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.MedioPago As Concepto,D.Neto,Alicuota AS Iva,D.Importe,F.Cambio,F.Nota AS Rec,C.Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Importe AS ImporteTotal FROM Proveedores AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.Estado <> 3 AND F.ChequeRechazado = 0 AND (F.TipoNota = 6 or F.TipoNota = 8) AND F.EsElectronica = 0 AND EsExterior = 0 AND " & SqlFecha & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,F.Nota As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.MedioPago As Concepto,D.Neto,Alicuota AS Iva,D.Importe,F.Cambio,F.Nota AS Rec,C.Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Importe AS ImporteTotal FROM Proveedores AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.Estado <> 3 AND F.ChequeRechazado = 0 AND (F.TipoNota = 6 or F.TipoNota = 8) AND F.EsElectronica = 1 AND EsExterior = 0 AND " & SqlFechaContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.MedioPago As Concepto,D.Neto,D.Alicuota AS Iva,D.Importe,F.Cambio,F.Nota AS Rec,C.Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Importe AS ImporteTotal FROM Proveedores AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.Estado <> 3 AND F.ChequeRechazado = 0 AND (F.TipoNota = 500 or F.TipoNota = 700) AND F.EsElectronica = 0 AND EsExterior = 0 AND " & SqlFechaContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.MedioPago As Concepto,D.Neto,D.Alicuota AS Iva,D.Importe,F.Cambio,F.Nota AS Rec,C.Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Importe AS ImporteTotal FROM Clientes AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.Estado <> 3 AND F.ChequeRechazado = 0 AND (F.TipoNota = 50 or F.TipoNota = 70) AND " & SqlFechaContable & _
                   " UNION ALL " & _
              "SELECT 200 AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Impuesto As Concepto,0 AS Neto,Alicuota AS Iva,D.Importe,1 AS Cambio,F.Liquidacion AS Rec,C.Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Importe AS ImporteTotal FROM Clientes AS C INNER JOIN (NVLPCabeza AS F INNER JOIN NVLPDetalle D ON F.Liquidacion = D.Liquidacion) ON C.Clave = F.Cliente WHERE F.Estado <> 3 AND " & SqlFechaContable & _
                   " UNION ALL " & _
              "SELECT 100 AS TipoComprobante,C.Nombre,C.Cuit,F.Liquidacion As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Concepto,F.Bruto AS Neto,F.Alicuota AS Iva,D.Importe,1 AS Cambio,F.Liquidacion AS Rec,C.Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Bruto AS ImporteTotal FROM Proveedores AS C INNER JOIN (LiquidacionCabeza AS F INNER JOIN LiquidacionDetalleConceptos AS D ON F.Liquidacion = D.Liquidacion) ON C.Clave = F.Proveedor WHERE F.Estado <> 3 AND " & SqlFechaContable & _
                   " UNION ALL " & _
              "SELECT 300 AS TipoComprobante,C.Nombre,C.Cuit,F.Movimiento As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Concepto,0 AS Neto,0 AS Iva,D.Importe AS Importe,Cambio,F.Movimiento AS Rec,1 AS Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Importe AS ImporteTotal FROM Tablas AS C INNER JOIN (GastosBancarioCabeza AS F INNER JOIN GastosBancarioDetalle D ON F.Movimiento = D.Movimiento) ON C.Clave = F.Banco WHERE F.Estado <> 3 AND " & SqlFechaContable & _
                   " UNION ALL " & _
              "SELECT 5000 AS TipoComprobante,'' AS Nombre,0 AS Cuit,F.Movimiento As Comprobante,CAST(FLOOR(CAST(F.Fecha AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Concepto,0 AS Neto,0 AS Iva,D.Importe AS Importe,1 AS Cambio,F.Movimiento AS Rec,1 AS Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Importe AS ImporteTotal FROM PrestamosMovimientoCabeza AS F INNER JOIN PrestamosMovimientoDetalle D ON F.Movimiento = D.Movimiento WHERE F.Estado <> 3 AND D.Concepto <> 6 AND " & SqlFecha & ";"

        Dt = New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        If Dt.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        View = New DataView
        View = Dt.DefaultView
        View.Sort = "TipoComprobante,Rec"

        Dim ClaveAnt As String = View(0).Item("TipoComprobante") & "/" & View(0).Item("Rec")

        Dim I_Inicial As Integer = 0
        Dim I_Final As Integer = 0

        Dim Row_Final As Integer = View.Count - 1
        Dim Row As DataRowView

        For I As Integer = 0 To Row_Final
            Row = View(I)
            If ClaveAnt <> Row("TipoComprobante") & "/" & Row("Rec") Then
                If Not CorteXComprobanteCompra(I_Inicial, I_Final) Then Me.Close() : Exit Sub
                I_Inicial = I
                I_Final = I
                ClaveAnt = Row("TipoComprobante") & "/" & Row("Rec")
            Else
                I_Final = I
            End If
        Next
        If Not CorteXComprobanteCompra(I_Inicial, Row_Final) Then Me.Close() : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.Default

        MsgBox("FIN PROCESO")

    End Sub
    Private Function CorteXComprobanteCompra(ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim TipoComprobante As Integer = View(Inicial).Item("TipoComprobante")

        Select Case TipoComprobante
            Case 1
                If Not ProcesaFacturaProveedorParaCompra(Inicial, Final) Then Return False 'Facturas proveedores.
            Case 6, 8, 500, 700, 50, 70
                If Not ProcesaNotaParaCompra(Inicial, Final) Then Return False 'Notas debito/credito a Proveedores, del Proveedor y Notas Debito/credito del Cliente. 
            Case 200
                If Not ProcesaNVLPParaCompra(Inicial, Final) Then Return False 'N.V.L.P.
            Case 100
                If Not ProcesaLiquidacionParaCompra(Inicial, Final) Then Return False 'Liquidacion. 
            Case 300
                If Not ProcesaGastoBancarioParaCompra(Inicial, Final) Then Return False 'Gasto Bancario. 
            Case 5000
                If Not ProcesaPrestamoParaCompra(Inicial, Final) Then Return True 'Prestamo. 
        End Select

        Return True

    End Function
    Private Sub ButtonEXCELComprasComprobantes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEXCELComprasComprobantes.Click

        GeneraExcelCITICompras()

    End Sub
    Private Sub ButtonEXCELComprasComprobantesVentas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEXCELComprasComprobantesVentas.Click

        GeneraExcelCITIVentas()

    End Sub
    Private Function ProcesaFacturaProveedorParaCompra(ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim RowsBusqueda() As DataRow
        Dim Row As DataRowView

        Dim NumeroLetra As Integer = Strings.Left(View(Inicial).Item("Comprobante"), 1)
        If NumeroLetra = 4 Then Return True 'No procesa si es importacion.

        Dim Fecha As Date = View(Inicial).Item("Fecha")
        Dim PuntoVenta As Integer = Strings.Mid(View(Inicial).Item("Comprobante").ToString, 2, 4)
        Dim Comprobante As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim CuitVendedor As Decimal = View(Inicial).Item("Cuit")
        Dim DenominacionVendedor As String = View(Inicial).Item("Nombre")
        Dim TipoComprobante As Integer = 0
        Dim DespachoImportacion As String = ""
        Dim CodigoDocuVendedor As Integer = 80
        Dim ImporteTotal As Decimal = 0       'View(Inicial).Item("ImporteTotal")
        Dim CodigoMoneda As String = "PES"
        Dim Cambio As Decimal = 1
        Dim CuitEmisor As Decimal = 0   'View(Inicial).Item("Cuit")
        Dim DenominacionEmisor As String = ""   'View(Inicial).Item("Nombre")
        Dim CodigoOperacion As String = " "

        Select Case NumeroLetra
            Case 1
                TipoComprobante = 1
            Case 2
                TipoComprobante = 6
            Case 3
                TipoComprobante = 11
            Case 4
                TipoComprobante = 19
            Case 5
                TipoComprobante = 51
            Case Else
                If MsgBox("Letra " & LetraTipoIva(NumeroLetra) & " en la Factura de proveedor " & NumeroEditado(View(Inicial).Item("Comprobante")) & " No Prevista. " + vbCrLf + "Si Continua este Comprobante NO se Incluirá en Archivo para la AFIP." + vbCrLf + "Desae Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Return False
                Else
                    Return True
                End If
        End Select

        ImporteIva = 0
        IvaComision = 0
        CantidadIvas = 0
        ImporteGrabado = 0
        ImporteNoGrabado = 0
        ImporteExento = 0
        ImportePercepcionIva = 0
        ImportePercepcionesOtrosImpuestosNacionales = 0
        ImportePercepcionIngresoBruto = 0
        ImportePercepcionesImpuestosMunicipales = 0
        ImportePercepcionesImpuestosInternos = 0
        ImporteOtrosTributos = 0
        ImportePercepciones = 0
        Dim ImporteConcepto1 As Decimal = 0

        AceraTabla(TablaIvas)

        For I As Integer = Inicial To Final
            Row = View(I)
            For J As Integer = 1 To UBound(TablaIvas)
                If Row("Concepto") = TablaIvas(J).Clave Then
                    TablaIvas(J).Importe = TablaIvas(J).Importe + Row("Importe")
                    ImporteIva = ImporteIva + Row("Importe")
                    ImporteGrabado = ImporteGrabado + Trunca(Row("Importe") * 100 / TablaIvas(J).Alicuota)
                End If
            Next
            RowsBusqueda = DtPercepciones.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda.Length <> 0 Then
                If RowsBusqueda(0).Item("OrigenPercepcion") = 0 Then
                    MsgBox("Falta Código Afip de la Percepción " & NombreTabla(Row("Concepto")))
                    Return False
                End If
                AcumulaPercepciones(RowsBusqueda(0).Item("OrigenPercepcion"), Row("Importe"), ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos)
                ImportePercepciones = ImportePercepciones + Row("Importe")
            End If
            '
            If Row("Concepto") = 1 Then
                ImporteConcepto1 = Row("Importe")
            End If
            If Row("Concepto") = 2 Then
                ImporteNoGrabado = ImporteNoGrabado + Row("Importe")
            End If
            If Row("Concepto") = 10 Then
                ImporteNoGrabado = ImporteNoGrabado + Row("Importe")
            End If
        Next

        For J As Integer = 1 To UBound(TablaIvas)
            If TablaIvas(J).Importe <> 0 Then
                CantidadIvas = CantidadIvas + 1
            End If
        Next

        ImporteTotal = ImporteGrabado + ImporteNoGrabado + ImporteIva + ImportePercepciones

        Select Case NumeroLetra
            Case 2, 3
                CantidadIvas = 0
                CodigoOperacion = "N"
                ImporteNoGrabado = 0
            Case Else
                If CantidadIvas = 0 Then
                    CantidadIvas = 1
                    CodigoOperacion = "N"
                End If
        End Select

        CorteCompra(TablaIvas, Fecha, PuntoVenta, Comprobante, TipoComprobante, DespachoImportacion, CodigoDocuVendedor, CuitVendedor, DenominacionVendedor, ImporteTotal, ImporteGrabado, ImporteNoGrabado, ImporteExento, ImporteIva, CantidadIvas, ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos, ImportePercepciones, CodigoMoneda, Cambio, CuitEmisor, DenominacionEmisor, IvaComision, CodigoOperacion)

        Return True

    End Function
    Private Function ProcesaNotaParaCompra(ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim RowsBusqueda() As DataRow
        Dim Row As DataRowView

        Dim TipoNota As Integer = View(Inicial).Item("TipoComprobante")

        Select Case TipoNota
            Case 6, 8
                If Not Opcion1LibrosIva Then Return True
            Case 50, 70
                If Opcion1LibrosIva Then Return True
        End Select

        Dim NumeroLetra As Integer = Strings.Left(View(Inicial).Item("Comprobante"), 1)
        Select Case TipoNota
            Case 50, 70
            Case Else
                If NumeroLetra = 4 Then Return True 'No procesa si es importacion.
        End Select

        Dim Fecha As Date = View(Inicial).Item("Fecha")
        Dim PuntoVenta As Integer = Strings.Mid(View(Inicial).Item("Comprobante").ToString, 2, 4)
        Dim Comprobante As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim CuitVendedor As Decimal = View(Inicial).Item("Cuit")
        Dim DenominacionVendedor As String = View(Inicial).Item("Nombre")
        Dim TipoComprobante As Integer = 0
        Dim DespachoImportacion As String = ""
        Dim CodigoDocuVendedor As Integer = 80
        Dim ImporteTotal As Decimal = View(Inicial).Item("ImporteTotal")
        Dim CodigoMoneda As String = "PES"
        Dim Cambio As Decimal = 1
        Dim CuitEmisor As Decimal = 0          'View(Inicial).Item("Cuit")
        Dim DenominacionEmisor As String = ""  'View(Inicial).Item("Nombre")
        Dim CodigoOperacion As String = " "

        'Opcion 1:
        'Modificamos Las NCF a Proveedor(8) del TipoComprobante 003 al 002 se comporta como NDF del Proveedor(500). 
        'Modificamos Las NDF a Proveedor(6) del TipoComprobante 002 al 003 se comporta como NCF del Proveedor(700). 
        'Opcion 2:
        'Modificamos Las NCF del Cliente(70) del TipoComprobante 002 al 003 se comporta como NDF a Proveedor(6). 
        'Modificamos Las NDF del Cliente(50) del TipoComprobante 003 al 002 se comporta como NCF a Proveedor(8). 
        Select Case NumeroLetra
            Case 1
                Select Case TipoNota
                    Case 8, 500, 50
                        TipoComprobante = 2
                    Case 6, 700, 70
                        TipoComprobante = 3
                End Select
            Case 2
                Select Case TipoNota
                    Case 8, 500, 50
                        TipoComprobante = 7
                    Case 6, 700, 70
                        TipoComprobante = 8
                End Select
            Case 3
                Select Case TipoNota
                    Case 8, 500, 50
                        TipoComprobante = 12
                    Case 6, 700, 70
                        TipoComprobante = 13
                End Select
            Case 5
                Select Case TipoNota
                    Case 8, 500, 50
                        TipoComprobante = 52
                    Case 6, 700, 70
                        TipoComprobante = 53
                End Select
            Case Else
                If MsgBox("Letra " & LetraTipoIva(NumeroLetra) & " en la Nota Crédito/Debito Financiera " & NumeroEditado(View(Inicial).Item("Comprobante")) & " No Prevista. " + vbCrLf + "Si Continua este Comprobante NO se Incluirá en Archivo para la AFIP." + vbCrLf + "Desae Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Return False
                Else
                    Return True
                End If
        End Select

        ImporteIva = 0
        IvaComision = 0
        CantidadIvas = 0
        ImporteGrabado = 0
        ImporteNoGrabado = 0
        ImporteExento = 0
        ImportePercepcionIva = 0
        ImportePercepcionesOtrosImpuestosNacionales = 0
        ImportePercepcionIngresoBruto = 0
        ImportePercepcionesImpuestosMunicipales = 0
        ImportePercepcionesImpuestosInternos = 0
        ImporteOtrosTributos = 0
        ImportePercepciones = 0

        AceraTabla(TablaIvas)

        For I As Integer = Inicial To Final
            Row = View(I)
            If Row("Concepto") = 100 Then
                If Row("Iva") = 0 Then
                    ImporteNoGrabado = ImporteNoGrabado + Row("Importe")
                Else
                    ImporteGrabado = ImporteGrabado + Row("Neto")
                End If
            End If
            For J As Integer = 1 To UBound(TablaIvas)
                If Row("Iva") = TablaIvas(J).Alicuota Then
                    TablaIvas(J).Importe = TablaIvas(J).Importe + (Row("Importe") - Row("Neto"))
                    ImporteIva = ImporteIva + (Row("Importe") - Row("Neto"))
                End If
            Next
            RowsBusqueda = DtPercepciones.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda.Length <> 0 Then
                If RowsBusqueda(0).Item("OrigenPercepcion") = 0 Then
                    MsgBox("Falta Código Afip de la Percepción " & NombreTabla(Row("Concepto")))
                    Return False
                End If
                AcumulaPercepciones(RowsBusqueda(0).Item("OrigenPercepcion"), Row("Importe"), ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos)
                ImportePercepciones = ImportePercepciones + Row("Importe")
            End If
        Next

        For J As Integer = 1 To UBound(TablaIvas)
            If TablaIvas(J).Importe <> 0 Then
                CantidadIvas = CantidadIvas + 1
            End If
        Next

        Select Case NumeroLetra
            Case 2, 3
                CantidadIvas = 0
                CodigoOperacion = "N"
                ImporteNoGrabado = 0
            Case Else
                If CantidadIvas = 0 Then
                    CantidadIvas = 1
                    CodigoOperacion = "N"
                End If
        End Select

        CorteCompra(TablaIvas, Fecha, PuntoVenta, Comprobante, TipoComprobante, DespachoImportacion, CodigoDocuVendedor, CuitVendedor, DenominacionVendedor, ImporteTotal, ImporteGrabado, ImporteNoGrabado, ImporteExento, ImporteIva, CantidadIvas, ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos, ImportePercepciones, CodigoMoneda, Cambio, CuitEmisor, DenominacionEmisor, IvaComision, CodigoOperacion)

        Return True

    End Function
    Private Function ProcesaNVLPParaCompra(ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim RowsBusqueda() As DataRow
        Dim Row As DataRowView

        Dim NumeroLetra As Integer = Strings.Left(View(Inicial).Item("Comprobante"), 1)

        Dim Fecha As Date = View(Inicial).Item("Fecha")
        Dim PuntoVenta As Integer = Strings.Mid(View(Inicial).Item("Comprobante").ToString, 2, 4)
        Dim Comprobante As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim CuitVendedor As Decimal = View(Inicial).Item("Cuit")
        Dim DenominacionVendedor As String = View(Inicial).Item("Nombre")
        Dim TipoComprobante As Integer = 0
        Dim DespachoImportacion As String = ""
        Dim CodigoDocuVendedor As Integer = 80
        Dim ImporteTotal As Decimal = 0
        Dim CodigoMoneda As String = "PES"
        Dim Cambio As Decimal = 1
        Dim CuitEmisor As Decimal = View(Inicial).Item("Cuit")
        Dim DenominacionEmisor As String = View(Inicial).Item("Nombre")
        Dim CodigoOperacion As String = " "

        Select Case NumeroLetra
            Case 1
                TipoComprobante = 60
            Case 2
                TipoComprobante = 61
            Case 5
                TipoComprobante = 58
            Case Else
                If MsgBox("Letra " & LetraTipoIva(NumeroLetra) & " en la N.V.L.P. " & NumeroEditado(View(Inicial).Item("Comprobante")) & " No Prevista. " + vbCrLf + "Si Continua este Comprobante NO se Incluirá en Archivo para la AFIP." + vbCrLf + "Desae Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Return False
                Else
                    Return True
                End If
        End Select

        ImporteIva = 0
        IvaComision = 0
        CantidadIvas = 0
        ImporteGrabado = 0
        ImporteNoGrabado = 0
        ImporteExento = 0
        ImportePercepcionIva = 0
        ImportePercepcionesOtrosImpuestosNacionales = 0
        ImportePercepcionIngresoBruto = 0
        ImportePercepcionesImpuestosMunicipales = 0
        ImportePercepcionesImpuestosInternos = 0
        ImporteOtrosTributos = 0
        ImportePercepciones = 0

        AceraTabla(TablaIvas)

        Dim AlicuotaComision As Decimal = 0
        Dim AlicuotaDescarga As Decimal = 0
        Dim AlicuotaFlete As Decimal = 0
        Dim AlicuotaOtrosConceptos As Decimal = 0
        Dim Comision As Decimal = 0
        Dim Descarga As Decimal = 0
        Dim Flete As Decimal = 0
        Dim OtrosConceptos As Decimal = 0

        Dim I As Integer
        Dim J As Integer

        For I = Inicial To Final
            Row = View(I)
            Select Case Row("Concepto")
                Case 6
                    AlicuotaComision = Row("Iva")
                Case 8
                    AlicuotaDescarga = Row("Iva")
                Case 11
                    AlicuotaFlete = Row("Iva")
                Case 12
                    AlicuotaOtrosConceptos = Row("Iva")
            End Select
        Next

        For I = Inicial To Final
            Row = View(I)
            Select Case Row("Concepto")
                Case 5
                    Comision = Row("Importe")
                    If AlicuotaComision = 0 Then
                        ImporteNoGrabado = ImporteNoGrabado + Row("Importe")
                    End If
                Case 7
                    Descarga = Row("Importe")
                    If AlicuotaDescarga = 0 Then
                        ImporteNoGrabado = ImporteNoGrabado + Row("Importe")
                    End If
                Case 9
                    Flete = Row("Importe")
                    If AlicuotaFlete = 0 Then
                        ImporteNoGrabado = ImporteNoGrabado + Row("Importe")
                    End If
                Case 10
                    OtrosConceptos = Row("Importe")
                    If AlicuotaOtrosConceptos = 0 Then
                        ImporteNoGrabado = ImporteNoGrabado + Row("Importe")
                    End If
                Case 6, 8, 11, 12
                    For J = 1 To UBound(TablaIvas)
                        If Row("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Importe = TablaIvas(J).Importe + Row("Importe")
                            ImporteIva = ImporteIva + Row("Importe")
                            Exit For
                        End If
                    Next
                    If Row("Concepto") = 6 Then IvaComision = IvaComision + Row("Importe")
                Case Else
                    RowsBusqueda = DtPercepciones.Select("Clave = " & Row("Concepto"))
                    If RowsBusqueda.Length <> 0 Then
                        AcumulaPercepciones(RowsBusqueda(0).Item("OrigenPercepcion"), Row("Importe"), ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos)
                        ImportePercepciones = ImportePercepciones + Row("Importe")
                    End If
            End Select
        Next

        ImporteTotal = Comision + Descarga + Flete + OtrosConceptos + ImporteIva + ImportePercepciones

        If ImporteTotal = 0 Then Return True

        For J = 1 To UBound(TablaIvas)
            If TablaIvas(J).Importe <> 0 Then
                CantidadIvas = CantidadIvas + 1
            End If
        Next

        Select Case NumeroLetra
            Case 2, 3
                CantidadIvas = 0
                CodigoOperacion = "N"
                ImporteNoGrabado = 0
            Case Else
                If CantidadIvas = 0 Then
                    CantidadIvas = 1
                    CodigoOperacion = "N"
                End If
        End Select

        CorteCompra(TablaIvas, Fecha, PuntoVenta, Comprobante, TipoComprobante, DespachoImportacion, CodigoDocuVendedor, CuitVendedor, DenominacionVendedor, ImporteTotal, ImporteGrabado, ImporteNoGrabado, ImporteExento, ImporteIva, CantidadIvas, ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos, ImportePercepciones, CodigoMoneda, Cambio, CuitEmisor, DenominacionEmisor, IvaComision, CodigoOperacion)

        Return True

    End Function
    Private Function ProcesaLiquidacionParaCompra(ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim Row As DataRowView

        Dim NumeroLetra As Integer = Strings.Left(View(Inicial).Item("Comprobante"), 1)

        Dim Fecha As Date = View(Inicial).Item("Fecha")
        Dim PuntoVenta As Integer = Strings.Mid(View(Inicial).Item("Comprobante").ToString, 2, 4)
        Dim Comprobante As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim CuitVendedor As Decimal = View(Inicial).Item("Cuit")
        Dim DenominacionVendedor As String = View(Inicial).Item("Nombre")
        Dim TipoComprobante As Integer = 0
        Dim DespachoImportacion As String = ""
        Dim CodigoDocuVendedor As Integer = 80
        Dim ImporteTotal As Decimal = View(Inicial).Item("ImporteTotal")
        Dim CodigoMoneda As String = "PES"
        Dim Cambio As Decimal = 1
        Dim CuitEmisor As Decimal = CuitNumerico(GCuitEmpresa)
        Dim DenominacionEmisor As String = GNombreEmpresa
        Dim CodigoOperacion As String = " "

        Select Case NumeroLetra
            Case 1
                TipoComprobante = 63
            Case 2
                TipoComprobante = 64
            Case 3
                TipoComprobante = 68
            Case 5
                TipoComprobante = 59
            Case Else
                If MsgBox("Letra " & LetraTipoIva(NumeroLetra) & " en Liquidación " & NumeroEditado(View(Inicial).Item("Comprobante")) & " No Prevista. " + vbCrLf + "Si Continua este Comprobante NO se Incluirá en Archivo para la AFIP." + vbCrLf + "Desae Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Return False
                Else
                    Return True
                End If
        End Select

        ImporteIva = 0
        IvaComision = 0
        CantidadIvas = 0
        ImporteGrabado = 0
        ImporteNoGrabado = 0
        ImporteExento = 0
        ImportePercepcionIva = 0
        ImportePercepcionesOtrosImpuestosNacionales = 0
        ImportePercepcionIngresoBruto = 0
        ImportePercepcionesImpuestosMunicipales = 0
        ImportePercepcionesImpuestosInternos = 0
        ImporteOtrosTributos = 0
        ImportePercepciones = 0

        AceraTabla(TablaIvas)

        Dim J As Integer
        Dim I As Integer

        For I = Inicial To Final
            Row = View(I)
            Select Case Row("Concepto")
                Case 2
                    If Row("Importe") <> 0 Then
                        For J = 1 To UBound(TablaIvas)
                            If Row("Iva") = TablaIvas(J).Alicuota Then
                                TablaIvas(J).Importe = TablaIvas(J).Importe + Row("Importe")
                                ImporteIva = ImporteIva + Row("Importe")
                                Exit For
                            End If
                        Next
                    End If
            End Select
        Next

        ImporteTotal = ImporteTotal + ImporteIva
        If ImporteIva = 0 Then ImporteNoGrabado = ImporteTotal

        For J = 1 To UBound(TablaIvas)
            If TablaIvas(J).Importe <> 0 Then
                CantidadIvas = CantidadIvas + 1
            End If
        Next

        Select Case NumeroLetra
            Case 2, 3
                CantidadIvas = 0
                CodigoOperacion = "N"
                ImporteNoGrabado = 0
            Case Else
                If CantidadIvas = 0 Then
                    CantidadIvas = 1
                    CodigoOperacion = "N"
                End If
        End Select

        CorteCompra(TablaIvas, Fecha, PuntoVenta, Comprobante, TipoComprobante, DespachoImportacion, CodigoDocuVendedor, CuitVendedor, DenominacionVendedor, ImporteTotal, ImporteGrabado, ImporteNoGrabado, ImporteExento, ImporteIva, CantidadIvas, ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos, ImportePercepciones, CodigoMoneda, Cambio, CuitEmisor, DenominacionEmisor, IvaComision, CodigoOperacion)

        Return True

    End Function
    Private Function ProcesaGastoBancarioParaCompra(ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim RowsBusqueda() As DataRow
        Dim Row As DataRowView

        Dim NumeroLetra As Integer = Strings.Left(View(Inicial).Item("Comprobante"), 1)

        Dim Fecha As Date = View(Inicial).Item("Fecha")
        Dim PuntoVenta As Integer = 0
        Dim Comprobante As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim Cuit As Decimal = View(Inicial).Item("Cuit")
        Dim RazonSocial As String = View(Inicial).Item("Nombre")
        Dim TipoComprobante As Integer = 0
        Dim DespachoImportacion As String = ""
        Dim CodigoDocuVendedor As Integer = 80
        Dim ImporteTotal As Decimal = 0
        Dim CodigoMoneda As String = "PES"
        Dim Cambio As Decimal = 1
        Dim CuitEmisor As Decimal = 0          'View(Inicial).Item("Cuit")
        Dim DenominacionEmisor As String = ""  'View(Inicial).Item("Nombre")
        Dim CodigoOperacion As String = " "
        Dim BaseImponible As Decimal = 0

        TipoComprobante = 99

        ImporteIva = 0
        IvaComision = 0
        CantidadIvas = 0
        ImporteGrabado = 0
        ImporteNoGrabado = 0
        ImporteExento = 0
        ImportePercepcionIva = 0
        ImportePercepcionesOtrosImpuestosNacionales = 0
        ImportePercepcionIngresoBruto = 0
        ImportePercepcionesImpuestosMunicipales = 0
        ImportePercepcionesImpuestosInternos = 0
        ImporteOtrosTributos = 0
        ImportePercepciones = 0

        AceraTabla(TablaIvas)

        Dim J As Integer
        Dim I As Integer

        For I = Inicial To Final
            Row = View(I)
            'Procesa Concepto Gastos.
            RowsBusqueda = DtGastosGrabados.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda.Length <> 0 Then
                If Operador(RowsBusqueda(0).Item("Clave")) = 2 Then
                    If RowsBusqueda(0).Item("Grabado") Then
                        ImporteGrabado = ImporteGrabado + Row("Importe")
                    Else
                        '                        ImporteExento = ImporteExento + Row("Importe")
                        ImporteNoGrabado = ImporteNoGrabado + Row("Importe")
                    End If
                End If
                If Operador(RowsBusqueda(0).Item("Clave")) = 1 Then
                    If RowsBusqueda(0).Item("Grabado") Then
                        ImporteGrabado = ImporteGrabado - Row("Importe")
                    Else
                        '                        ImporteExento = ImporteExento - Row("Importe")
                        ImporteNoGrabado = ImporteNoGrabado - Row("Importe")
                    End If
                End If
            End If
            'Procesa Iva.
            For J = 1 To UBound(TablaIvas)
                If Row("Concepto") = TablaIvas(J).Clave Then
                    TablaIvas(J).Importe = TablaIvas(J).Importe + Row("Importe")
                    ImporteIva = ImporteIva + Row("Importe")
                    If TablaIvas(J).Alicuota <> 0 Then BaseImponible = BaseImponible + Trunca(Row("Importe") * 100 / TablaIvas(J).Alicuota)
                End If
            Next
            'Procesa Percepciones.
            RowsBusqueda = DtPercepciones.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda.Length <> 0 Then
                If RowsBusqueda(0).Item("OrigenPercepcion") = 0 Then
                    MsgBox("Falta Código Afip de la Percepción " & NombreTabla(Row("Concepto")))
                    Return False
                End If
                AcumulaPercepciones(RowsBusqueda(0).Item("OrigenPercepcion"), Row("Importe"), ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos)
                ImportePercepciones = ImportePercepciones + Row("Importe")
            End If
        Next

        For J = 1 To UBound(TablaIvas)
            If TablaIvas(J).Importe <> 0 Then
                CantidadIvas = CantidadIvas + 1
            End If
        Next

        If BaseImponible <> 0 Then ImporteGrabado = BaseImponible

        ImporteTotal = ImporteGrabado + ImporteIva + ImportePercepciones + ImporteNoGrabado + ImporteExento

        If ImporteIva = 0 Then
            CantidadIvas = 1
            CodigoOperacion = "N"
        End If

        If ImporteIva <> 0 Or ImportePercepcionIva <> 0 Then
            CorteCompra(TablaIvas, Fecha, PuntoVenta, Comprobante, TipoComprobante, DespachoImportacion, CodigoDocuVendedor, Cuit, RazonSocial, ImporteTotal, ImporteGrabado, ImporteNoGrabado, ImporteExento, ImporteIva, CantidadIvas, ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos, ImportePercepciones, CodigoMoneda, Cambio, CuitEmisor, DenominacionEmisor, IvaComision, CodigoOperacion)
        End If

        Return True

    End Function
    Private Function ProcesaPrestamoParaCompra(ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim RowsBusqueda() As DataRow
        Dim Row As DataRowView

        Dim CuitEmisorW As Decimal = 0
        Dim DenominacionEmisorW As String = ""

        HallaDatosPrestamo(View(Inicial).Item("Comprobante"), CuitEmisorW, DenominacionEmisorW)

        Dim NumeroLetra As Integer = Strings.Left(View(Inicial).Item("Comprobante"), 1)

        Dim Fecha As Date = View(Inicial).Item("Fecha")
        Dim PuntoVenta As Integer = 0
        Dim Comprobante As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        'ultima correccion
        Dim Cuit As Decimal = CuitEmisorW
        Dim RazonSocial As String = DenominacionEmisorW
        '-------------------------------------------------
        Dim TipoComprobante As Integer = 0
        Dim DespachoImportacion As String = ""
        Dim CodigoDocuVendedor As Integer = 80
        Dim ImporteTotal As Decimal = 0
        Dim CodigoMoneda As String = "PES"
        Dim Cambio As Decimal = 1
        'ultima correccion
        Dim CuitEmisor As Decimal = 0
        Dim DenominacionEmisor As String = ""
        '-----------------------------------------------
        Dim CodigoOperacion As String = " "
        Dim BaseImponible As Decimal = 0

        'ultima correccion 


        TipoComprobante = 99

        ImporteIva = 0
        IvaComision = 0
        CantidadIvas = 0
        ImporteGrabado = 0
        ImporteNoGrabado = 0
        ImporteExento = 0
        ImportePercepcionIva = 0
        ImportePercepcionesOtrosImpuestosNacionales = 0
        ImportePercepcionIngresoBruto = 0
        ImportePercepcionesImpuestosMunicipales = 0
        ImportePercepcionesImpuestosInternos = 0
        ImporteOtrosTributos = 0
        ImportePercepciones = 0

        AceraTabla(TablaIvas)

        Dim J As Integer
        Dim I As Integer

        For I = Inicial To Final
            Row = View(I)
            'Procesa Conceptos.
            RowsBusqueda = DtGastosGrabados.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda.Length <> 0 Then
                If RowsBusqueda(0).Item("Grabado") Then
                    ImporteGrabado = ImporteGrabado + Row("Importe")
                Else
                    '                    ImporteExento = ImporteExento + Row("Importe")
                    ImporteNoGrabado = ImporteNoGrabado + Row("Importe")
                End If
            End If
            If Row("Concepto") = 7 Then ImporteGrabado = ImporteGrabado + Row("Importe") 'Intereses
            '            If Row("Concepto") = 6 Then ImporteExento = ImporteExento + Row("Importe") 'Capital a Cancelar. 
            If Row("Concepto") = 6 Then ImporteNoGrabado = ImporteNoGrabado + Row("Importe") 'Capital a Cancelar. 
            'Procesa Iva.
            For J = 1 To UBound(TablaIvas)
                If Row("Concepto") = TablaIvas(J).Clave Then
                    TablaIvas(J).Importe = TablaIvas(J).Importe + Row("Importe")
                    ImporteIva = ImporteIva + Row("Importe")
                    If TablaIvas(J).Alicuota <> 0 Then BaseImponible = BaseImponible + Trunca(Row("Importe") * 100 / TablaIvas(J).Alicuota)
                End If
            Next
            'Procesa Percepciones.
            RowsBusqueda = DtPercepciones.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda.Length <> 0 Then
                If RowsBusqueda(0).Item("OrigenPercepcion") = 0 Then
                    MsgBox("Falta Código Afip de la Percepción " & NombreTabla(Row("Concepto")))
                    Return False
                End If
                AcumulaPercepciones(RowsBusqueda(0).Item("OrigenPercepcion"), Row("Importe"), ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos)
                ImportePercepciones = ImportePercepciones + Row("Importe")
            End If
        Next

        For J = 1 To UBound(TablaIvas)
            If TablaIvas(J).Importe <> 0 Then
                CantidadIvas = CantidadIvas + 1
            End If
        Next

        If BaseImponible <> 0 Then ImporteGrabado = BaseImponible

        ImporteTotal = ImporteGrabado + ImporteIva + ImportePercepciones + ImporteNoGrabado + ImporteExento

        If ImporteIva = 0 Then
            CantidadIvas = 1
            CodigoOperacion = "N"
        End If

        If ImporteIva <> 0 Or ImportePercepcionIva <> 0 Then
            CorteCompra(TablaIvas, Fecha, PuntoVenta, Comprobante, TipoComprobante, DespachoImportacion, CodigoDocuVendedor, Cuit, RazonSocial, ImporteTotal, ImporteGrabado, ImporteNoGrabado, ImporteExento, ImporteIva, CantidadIvas, ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos, ImportePercepciones, CodigoMoneda, Cambio, CuitEmisor, DenominacionEmisor, IvaComision, CodigoOperacion)
        End If

        Return True

    End Function
    Private Sub CorteVenta(ByVal TablaIvas() As ItemIvaReten, ByVal Fecha As Date, ByVal TipoComprobante As Integer, ByVal PuntoVenta As Integer, ByVal Comprobante As Decimal, ByVal ComprobanteHasta As Decimal, ByVal CodigoDocuComprador As Integer, ByVal NumeroIdentificacionComprador As Decimal, ByVal NombreComprador As String, ByVal ImporteTotalW As Decimal, ByVal ImporteNoGrabadoW As Decimal, ByVal ImportePercepcionesNoCategorizadaW As Decimal, ByVal ImporteExentoW As Decimal, ByVal ImportePercepcionImpuestosNacionalesW As Decimal, ByVal ImportePercepcionIngresoBrutoW As Decimal, ByVal ImportePercepcionImpuestosMunicipalesW As Decimal, ByVal ImportePercepcionImpuestosInternosW As Decimal, ByVal CodigoMoneda As String, ByVal Cambio As Decimal, ByVal CantidadIvasW As Integer, ByVal CodigoOperacion As String, ByVal ImporteOtrosTributosW As Decimal, ByVal FechaPago As String)

        Dim FechaW As String = FormateaFecha(Fecha)
        Dim TipoComprobanteW As String = RellenarCeros(TipoComprobante, 3)
        Dim PuntoVentaW As String = RellenarCeros(PuntoVenta, 5)
        Dim ComprobanteW As String = RellenarCeros(Comprobante, 20)
        Dim ComprobanteHastaW As String = RellenarCeros(ComprobanteHasta, 20)
        Dim CodigoDocuCompradorW As String = RellenarCeros(CodigoDocuComprador, 2)
        Dim NumeroIdentificacionCompradorW As String = RellenarCeros(NumeroIdentificacionComprador, 20)
        Dim CodigoMonedaW = RellenarBlancos(CodigoMoneda, 3)
        Dim NombreCompradorW As String = RellenarBlancos(Strings.Left(NombreComprador, 30), 30)
        Dim ImporteTotalWW As String = FormateaImporteCITI(ImporteTotalW)
        Dim ImporteNoGrabadoWW As String = FormateaImporteCITI(ImporteNoGrabadoW)
        Dim ImportePercepcionNoCategorizadaWW As String = FormateaImporteCITI(ImportePercepcionesNoCategorizadaW)
        Dim ImporteExentoWW As String = FormateaImporteCITI(ImporteExentoW)
        Dim ImportePercepcionImpuestosNacionalesWW As String = FormateaImporteCITI(ImportePercepcionImpuestosNacionalesW)
        Dim ImportePercepcionIngresoBrutoWW As String = FormateaImporteCITI(ImportePercepcionIngresoBrutoW)
        Dim ImportePercepcionImpuestosMunicipalesWW As String = FormateaImporteCITI(ImportePercepcionImpuestosMunicipalesW)
        Dim ImportePercepcionImpuestosInternosWW As String = FormateaImporteCITI(ImportePercepcionImpuestosInternosW)
        Dim TipoCambioWW As String = FormateaTipoCambio(Cambio)
        Dim ImporteOtrosTributosWW As String = FormateaImporteCITI(ImporteOtrosTributosW)
        Dim FechaPagoW As String = FechaPago
        If FechaPagoW <> "00000000" Then
            FormateaFecha(CDate(FechaPago))
        Else
            FechaPagoW = "00000000"
        End If

        Dim Str As String = FechaW & TipoComprobanteW & PuntoVentaW & ComprobanteW & ComprobanteHastaW & CodigoDocuCompradorW & NumeroIdentificacionCompradorW & NombreCompradorW & ImporteTotalWW & ImporteNoGrabadoWW & ImportePercepcionNoCategorizadaWW & ImporteExentoWW & ImportePercepcionImpuestosNacionalesWW & ImportePercepcionIngresoBrutoWW & ImportePercepcionImpuestosMunicipalesWW & ImportePercepcionImpuestosInternosWW & CodigoMonedaW & TipoCambioWW & CantidadIvasW & CodigoOperacion & ImporteOtrosTributosWW & FechaPagoW

        Using writer As StreamWriter = New StreamWriter(Directorio1, True)
            writer.WriteLine(Str)
            writer.Close()
        End Using

        Dim AlicuotaIvaWW As Integer = 0
        Dim J As Integer

        Dim Importe As Decimal = 0
        For J = 1 To UBound(TablaIvas)
            Importe = Importe + TablaIvas(J).Importe
        Next
        If Importe = 0 Then
            Dim AlicuotaIvaWWW As String = RellenarCeros(3, 4)
            Dim ImporteLiquidadoWWW As String = FormateaImporteCITI(0)
            Dim ImporteGrabadoWWW As String = FormateaImporteCITI(0)
            Str = TipoComprobanteW & PuntoVentaW & ComprobanteW & ImporteGrabadoWWW & AlicuotaIvaWWW & ImporteLiquidadoWWW
            Using writer As StreamWriter = New StreamWriter(Directorio2, True)
                writer.WriteLine(Str)
                writer.Close()
            End Using
            'Para generar Excel.
            Dim Str3 As String = Str & CodigoDocuCompradorW & NumeroIdentificacionCompradorW
            Using writer As StreamWriter = New StreamWriter(Directorio3, True)
                writer.WriteLine(Str3)
                writer.Close()
            End Using
            Exit Sub
        End If

        For J = 1 To UBound(TablaIvas)
            If TablaIvas(J).Importe <> 0 Then
                Select Case TablaIvas(J).Alicuota
                    Case 0
                        AlicuotaIvaWW = 3
                    Case 10.5
                        AlicuotaIvaWW = 4
                    Case 21
                        AlicuotaIvaWW = 5
                    Case 27
                        AlicuotaIvaWW = 6
                    Case 5
                        AlicuotaIvaWW = 8
                    Case 2.5
                        AlicuotaIvaWW = 9
                End Select
                Dim AlicuotaIvaWWW As String = RellenarCeros(AlicuotaIvaWW, 4)
                Dim ImporteLiquidadoWWW As String = FormateaImporteCITI(TablaIvas(J).Importe)
                Dim ImporteGrabadoWWW As String
                ImporteGrabadoWWW = FormateaImporteCITI(Trunca(TablaIvas(J).Base))
                Str = TipoComprobanteW & PuntoVentaW & ComprobanteW & ImporteGrabadoWWW & AlicuotaIvaWWW & ImporteLiquidadoWWW
                Using writer As StreamWriter = New StreamWriter(Directorio2, True)
                    writer.WriteLine(Str)
                    writer.Close()
                End Using
                'Para generar Excel.
                Dim Str3 As String = Str & CodigoDocuCompradorW & NumeroIdentificacionCompradorW
                Using writer As StreamWriter = New StreamWriter(Directorio3, True)
                    writer.WriteLine(Str3)
                    writer.Close()
                End Using
            End If
        Next

    End Sub
    Private Sub CorteCompra(ByVal TablaIvas() As ItemIvaReten, ByVal Fecha As Date, ByVal PuntoVenta As Integer, ByVal Comprobante As Decimal, ByVal TipoComprobante As Integer, ByVal DespachoImportacion As String, ByVal CodigoDocuVendedor As Integer, ByVal NumeroIdentificacionProveedor As Decimal, ByVal NombreVendedor As String, ByVal ImporteTotalW As Decimal, ByVal ImporteGrabadoW As Decimal, ByVal ImporteNoGrabadoW As Decimal, ByVal ImporteExentoW As Decimal, ByVal ImporteIvaW As Decimal, ByVal CantidadIvasW As Integer, ByVal ImportePercepcionIvaW As Decimal, ByVal ImportePercepcionOtrosImpuestosNacionalesW As Decimal, ByVal ImportePercepcionIngresoBrutoW As Decimal, ByVal ImportePercepcionImpuestosMunicipalesW As Decimal, ByVal ImportePercepcionImpuestosInternosW As Decimal, ByVal ImportePercepcionesW As Decimal, ByVal CodigoMoneda As String, ByVal Cambio As Decimal, ByVal CuitEmisor As Decimal, ByVal DenominacionEmisor As String, ByVal IvaComisionW As Decimal, ByVal CodigoOperacion As String)

        Dim FechaW As String = FormateaFecha(Fecha)
        Dim TipoComprobanteW As String = RellenarCeros(TipoComprobante, 3)
        Dim PuntoVentaW As String = RellenarCeros(PuntoVenta, 5)
        Dim ComprobanteW As String = RellenarCeros(Comprobante, 20)
        Dim DespachoImportacionW As String = RellenarBlancos(DespachoImportacion, 16)
        Dim CodigoDocuVendedorW As String = RellenarCeros(CodigoDocuVendedor, 2)
        Dim NumeroIdentificacionProveedorW As String = RellenarCeros(NumeroIdentificacionProveedor, 20)
        Dim NombreVendedorW As String = RellenarBlancos(NombreVendedor, 30)
        Dim ImporteTotalWW As String = FormateaImporteCITI(ImporteTotalW)
        Dim ImporteNoGrabadoWW As String = FormateaImporteCITI(ImporteNoGrabadoW)
        Dim ImporteExentoWW As String = FormateaImporteCITI(ImporteExentoW)
        Dim ImportePercepcionIvaWW As String = FormateaImporteCITI(ImportePercepcionIvaW)
        Dim ImportePercepcionOtrosImpuestosNacionalesWW As String = FormateaImporteCITI(ImportePercepcionOtrosImpuestosNacionalesW)
        Dim ImportePercepcionIngresoBrutoWW As String = FormateaImporteCITI(ImportePercepcionIngresoBrutoW)
        Dim ImportePercepcionImpuestosMunicipalesWW As String = FormateaImporteCITI(ImportePercepcionImpuestosMunicipalesW)
        Dim ImportePercepcionImpuestosInternosWW As String = FormateaImporteCITI(ImportePercepcionImpuestosInternosW)
        Dim TipoCambioWW As String = FormateaTipoCambio(Cambio)
        Dim ImporteCreditoFiscalComputableWW As String = FormateaImporteCITI(ImporteIvaW)
        Dim ImporteOtrosTributosWW As String = FormateaImporteCITI(0)
        Dim CuitEmisorWW As String = RellenarCeros(CuitEmisor, 11)
        Dim DenominacionEmisorWW As String = RellenarBlancos(DenominacionEmisor, 30)
        Dim IvaComisionWW As String = FormateaImporteCITI(IvaComisionW)

        Dim Str As String = FechaW & TipoComprobanteW & PuntoVentaW & ComprobanteW & DespachoImportacionW & CodigoDocuVendedorW & NumeroIdentificacionProveedorW & NombreVendedorW & ImporteTotalWW & ImporteNoGrabadoWW & ImporteExentoWW & ImportePercepcionIvaWW & ImportePercepcionOtrosImpuestosNacionalesWW & ImportePercepcionIngresoBrutoWW & ImportePercepcionImpuestosMunicipalesWW & ImportePercepcionImpuestosInternosWW & CodigoMoneda & TipoCambioWW & CantidadIvasW & CodigoOperacion & ImporteCreditoFiscalComputableWW & ImporteOtrosTributosWW & CuitEmisorWW & DenominacionEmisorWW & IvaComisionWW

        Using writer As StreamWriter = New StreamWriter(Directorio1, True)
            writer.WriteLine(Str)
            writer.Close()
        End Using

        Dim AlicuotaIvaWW As Integer = 0
        Dim J As Integer

        If ImporteGrabadoW = 0 And ImporteNoGrabadoW <> 0 Then   'Caso no tiene informado ImporteGrabadoW y tiene ImporteNoGrabado.
            Dim AlicuotaIvaWWW As String = RellenarCeros(3, 4)
            Dim ImporteLiquidadoWWW As String = FormateaImporteCITI(0)
            Dim ImporteGrabadoWWW As String = FormateaImporteCITI(0)
            Str = TipoComprobanteW & PuntoVentaW & ComprobanteW & CodigoDocuVendedorW & NumeroIdentificacionProveedorW & ImporteGrabadoWWW & AlicuotaIvaWWW & ImporteLiquidadoWWW
            Using writer As StreamWriter = New StreamWriter(Directorio2, True)
                writer.WriteLine(Str)
                writer.Close()
            End Using
            Exit Sub
        End If

        'Caso tiene Informado Ivas.
        For J = 1 To UBound(TablaIvas)
            If TablaIvas(J).Importe <> 0 Then
                Select Case TablaIvas(J).Alicuota
                    Case 0
                        AlicuotaIvaWW = 3
                    Case 10.5
                        AlicuotaIvaWW = 4
                    Case 21
                        AlicuotaIvaWW = 5
                    Case 27
                        AlicuotaIvaWW = 6
                    Case 5
                        AlicuotaIvaWW = 8
                    Case 2.5
                        AlicuotaIvaWW = 9
                End Select
                Dim AlicuotaIvaWWW As String = RellenarCeros(AlicuotaIvaWW, 4)
                Dim ImporteLiquidadoWWW As String = FormateaImporteCITI(TablaIvas(J).Importe)
                Dim ImporteGrabadoWWW As String = FormateaImporteCITI(Trunca(TablaIvas(J).Importe * 100 / TablaIvas(J).Alicuota))
                Str = TipoComprobanteW & PuntoVentaW & ComprobanteW & CodigoDocuVendedorW & NumeroIdentificacionProveedorW & ImporteGrabadoWWW & AlicuotaIvaWWW & ImporteLiquidadoWWW
                Using writer As StreamWriter = New StreamWriter(Directorio2, True)
                    writer.WriteLine(Str)
                    writer.Close()
                End Using
            End If
        Next

    End Sub
    Private Function Operador(ByVal Concepto As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGastosBancarios.Select("Clave = " & Concepto)
        Return RowsBusqueda(0).Item("Operador")

    End Function
    Private Function RellenarBlancos(ByVal Dato As String, ByVal Logitud As Integer) As String

        Dim Str As String = ""

        Str = Dato

        For i As Integer = 1 To Logitud - Str.Length
            Str = Str & " "
        Next

        Return Str

    End Function
    Private Function FormateaFecha(ByVal Fecha As Date) As String

        Return Format(Fecha.Year, "0000") & Format(Fecha.Month, "00") & Format(Fecha.Day, "00")

    End Function
    Private Function PideDirectorio() As String

        'https://msdn.microsoft.com/es-es/library/sfezx97z(v=vs.110).aspx

        Dim saveFileDialog1 As New SaveFileDialog()
        'saveFileDialog1.FileName = "wwwww"
        '   saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif"
        saveFileDialog1.Title = "Guardar Archivo para la Importación AFIP."
        saveFileDialog1.ShowDialog()

        Return saveFileDialog1.FileName

    End Function
    Private Function PideArchivoParaExcel() As String

        Dim Archivo As String
        Dim openFileDialog1 As New OpenFileDialog()

        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            Archivo = openFileDialog1.FileName
        End If

        openFileDialog1.Dispose()

        Return Archivo

    End Function
    Private Sub ButtonCITIVentas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCITIVentas.Click

        Directorio1 = ""
        Directorio2 = ""
        Directorio3 = ""

        Directorio1 = PideCarpeta()
        If Directorio1 = "" Then Exit Sub
        Directorio2 = Directorio1
        Directorio3 = Directorio1
        Directorio1 = Directorio1 & "\" & "REGINFO_CV_VENTAS_CBTE.TXT"
        Directorio2 = Directorio2 & "\" & "REGINFO_CV_VENTAS_ALICUOTAS.TXT"
        Directorio3 = Directorio3 & "\" & "AuxParaExcel.TXT"

        Using writer As StreamWriter = New StreamWriter(Directorio1)                        'Borra Archivo.
        End Using
        Using writer As StreamWriter = New StreamWriter(Directorio2)                        'Borra Archivo.
        End Using
        Using writer As StreamWriter = New StreamWriter(Directorio3)                        'Borra Archivo.
        End Using

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Hasta debe ser mayor o igual a Fecha Desde.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim SqlFecha As String = ""
        SqlFecha = "F.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaContable As String = ""
        SqlFechaContable = "F.FechaContable >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.FechaContable < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaOficial As String = ""
        SqlFechaOficial = "F.FechaReciboOficial >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.FechaReciboOficial < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaElectronica As String
        SqlFechaElectronica = "F.FechaElectronica >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.FechaElectronica < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim Sql As String
        Sql = "SELECT 1 AS TipoComprobante,C.Nombre,C.Cuit,C.Provincia,F.Factura AS Comprobante,D.Articulo,D.Cantidad,D.Precio,D.Iva,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,F.Factura,0 As Impuesto,F.Importe,F.Moneda,F.Cambio,F.Factura AS Rec,F.Senia AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,D.TotalArticulo,0 AS ImporteTotal,EsExterior,F.EsZ,F.ComprobanteDesde,F.ComprobanteHasta,F.Percepciones,F.EsFCE FROM Clientes AS C INNER JOIN (FacturasCabeza AS F INNER JOIN FacturasDetalle AS D " & _
                      "ON F.Factura = D.Factura) ON F.Cliente = C.Clave WHERE F.Estado <> 3 AND EsExterior = 0 AND " & SqlFechaContable & _
                      " UNION ALL " & _
              "SELECT 1 AS TipoComprobante,C.Nombre,C.Cuit,C.Provincia,F.Factura AS Comprobante,D.Articulo,D.Cantidad,D.Precio,D.Iva,CAST(FLOOR(CAST(F.FechaElectronica AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,F.Factura,0 As Impuesto,F.Importe,F.Moneda,F.Cambio,F.Factura AS Rec,F.Senia AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,D.TotalArticulo,0 AS ImporteTotal,EsExterior,F.EsZ,F.ComprobanteDesde,F.ComprobanteHasta,F.Percepciones,F.EsFCE FROM Clientes AS C INNER JOIN (FacturasCabeza AS F INNER JOIN FacturasDetalle AS D " & _
                      "ON F.Factura = D.Factura) ON F.Cliente = C.Clave WHERE F.Estado <> 3 AND EsExterior = 1 AND " & SqlFechaElectronica & _
                      " UNION ALL " & _
              "SELECT 4 AS TipoComprobante,'' AS Nombre,0.0 AS Cuit,0 As Provincia,F.NotaCredito As Comprobante,D.Articulo,D.Cantidad,D.Precio,D.Iva,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado AS Estado,F.Factura as Factura,0 As Impuesto,F.Importe,F.Moneda,F.Cambio,F.NotaCredito AS Rec,0 AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,1 As Pais,D.Importe AS TotalArticulo,0 AS ImporteTotal,Fac.EsExterior AS EsExterior,FAC.EsZ,FAC.ComprobanteDesde,FAC.ComprobanteHasta,F.Percepciones,F.EsFCE FROM FacturasCabeza AS FAC INNER JOIN(NotasCreditoCabeza AS F INNER JOIN NotasCreditoDetalle AS D ON F.NotaCredito = D.NotaCredito) ON Fac.Factura = F.Factura WHERE F.Estado <> 3 AND " & SqlFechaContable & _
                      " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,C.Provincia,F.Nota As Comprobante,0 AS Remito,0 AS Cantidad,0 AS Precio,D.Alicuota As IVA,CAST(FLOOR(CAST(F.Fecha AS FLOAT)) AS DATETIME) AS Fecha,F.Estado AS Estado,0 AS Factura,D.Mediopago As Impuesto,D.Neto AS Importe,F.Moneda,F.Cambio,F.Nota AS Rec,0 AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,D.Importe AS TotalArticulo,F.Importe AS ImporteTotal,EsExterior,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,0 AS Percepciones,F.EsFCE FROM Clientes AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.Estado = 1  AND F.ChequeRechazado = 0 AND (F.TipoNota = 5 or F.TipoNota = 7 or F.TipoNota = 6 or F.TipoNota = 8) AND F.EsElectronica = 0 AND F.EsExterior = 0 AND " & SqlFecha & _
                      " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,C.Provincia,F.Nota As Comprobante,0 AS Remito,0 AS Cantidad,0 AS Precio,D.Alicuota As IVA,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado AS Estado,0 AS Factura,D.Mediopago As Impuesto,D.Neto AS Importe,F.Moneda,F.Cambio,F.Nota AS Rec,0 AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,D.Importe AS TotalArticulo,F.Importe AS ImporteTotal,EsExterior,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,0 AS Percepciones,F.EsFCE FROM Clientes AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.Estado = 1  AND F.ChequeRechazado = 0 AND (F.TipoNota = 5 or F.TipoNota = 7 or F.TipoNota = 6 or F.TipoNota = 8) AND (F.EsElectronica = 1 OR F.EsExterior = 1) AND " & SqlFechaContable & _
                      " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,C.Provincia,F.ReciboOficial As Comprobante,0 AS Remito,0 AS Cantidad,0 AS Precio,D.Alicuota As IVA,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado AS Estado,0 AS Factura,D.Mediopago As Impuesto,D.Neto AS Importe,F.Moneda,F.Cambio,F.Nota AS Rec,0 AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,D.Importe AS TotalArticulo,F.Importe AS ImporteTotal,EsExterior,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,0 AS Percepciones,F.EsFCE FROM Clientes AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.Estado = 1 AND F.ChequeRechazado = 0 AND (F.TipoNota = 50 or F.TipoNota = 70) AND F.EsExterior = 0 AND " & SqlFechaContable & _
                     " UNION ALL " & _
             "SELECT 100 AS TipoComprobante,C.Nombre,C.Cuit,C.Provincia,F.ReciboOficial As Comprobante,0 AS Remito,0 AS Cantidad,0 AS Precio,D.Alicuota As IVA,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,0 AS Factura,D.Impuesto,D.Importe,1 AS Moneda,1 AS Cambio,F.Liquidacion AS Rec,0 AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,D.Importe AS TotalArticulo,F.ImporteBruto AS ImporteTotal,0 AS EsExterior,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,0 AS Percepciones,0 AS EsFCE FROM Clientes AS C INNER JOIN (NVLPCabeza AS F INNER JOIN NVLPDetalle AS D ON F.Liquidacion = D.Liquidacion) ON C.Clave = F.Cliente WHERE F.Estado = 1 AND " & SqlFechaContable & _
                     " UNION ALL " & _
             "SELECT 200 AS TipoComprobante,C.Nombre,C.Cuit,C.Provincia,F.Liquidacion As Comprobante,0 AS Remito,1 AS Cantidad,Bruto AS Precio,D.Valor AS IVA,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,0 AS Factura,D.Concepto AS Impuesto,D.Importe AS Importe,1 AS Moneda,1 AS Cambio,F.Liquidacion AS Rec,F.Comision,F.AlicuotaComision,F.Descarga,F.AlicuotaDescarga,C.Pais,D.Importe AS TotalArticulo,F.Importe AS ImporteTotal,0 AS EsExterior,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,0 AS Percepciones,0 AS EsFCE  FROM Proveedores AS C INNER JOIN (LiquidacionCabeza AS F INNER JOIN LiquidacionDetalleConceptos AS D ON F.Liquidacion = D.Liquidacion) ON C.Clave = F.Proveedor WHERE F.Estado = 1 AND F.EsInsumos = 0 AND " & SqlFechaContable & _
                     " UNION ALL " & _
             "SELECT 210 AS TipoComprobante,C.Nombre,C.Cuit,C.Provincia,F.Liquidacion As Comprobante,0 AS Remito,1 AS Cantidad,Bruto AS Precio,D.Valor AS IVA,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,0 AS Factura,D.Concepto AS Impuesto,D.Importe AS Importe,1 AS Moneda,1 AS Cambio,F.Liquidacion AS Rec,F.Comision,F.AlicuotaComision,F.Descarga,F.AlicuotaDescarga,C.Pais,D.Importe AS TotalArticulo,F.Importe AS ImporteTotal,0 AS EsExterior,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,0 AS Percepciones,0 AS EsFCE  FROM Proveedores AS C INNER JOIN (LiquidacionCabeza AS F INNER JOIN LiquidacionDetalleConceptos AS D ON F.Liquidacion = D.Liquidacion) ON C.Clave = F.Proveedor WHERE F.Estado = 1 AND F.EsInsumos = 1 AND " & SqlFechaContable & ";"

        Dt = New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        If Dt.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim ComprobanteDesde As Decimal
        Dim ComprobanteHasta As Decimal

        For Each Row1 As DataRow In Dt.Rows
            If Row1("TipoComprobante") = 4 Then
                HallaDatosCliente(Row1("Factura"), Row1("Nombre"), Row1("Cuit"), Row1("Pais"), Row1("Provincia"), ComprobanteDesde, ComprobanteHasta)
                If Row1("Nombre") = "" Then
                    MsgBox("Error Base de Datos al leer Tabla de Facturas Cabeza", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close() : Exit Sub
                End If
            End If
            If Row1("TipoComprobante") = 1 And Row1("EsExterior") Then
                Row1("Cuit") = HallaCuitPais(Row1("Pais"))
                If Row1("Cuit") <= 0 Then
                    MsgBox("Error Al Leer Cuit del Pais: " & NombrePais(Row1("Pais")), MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close() : Exit Sub
                End If
            End If
        Next

        View = New DataView
        View = Dt.DefaultView
        View.Sort = "TipoComprobante,Rec"

        Dim ClaveAnt As String = View(0).Item("TipoComprobante") & "/" & View(0).Item("Rec")

        Dim I_Inicial As Integer = 0
        Dim I_Final As Integer = 0

        Dim Row_Final As Integer = View.Count - 1
        Dim Row As DataRowView

        For I As Integer = 0 To Row_Final
            Row = View(I)
            If ClaveAnt <> Row("TipoComprobante") & "/" & Row("Rec") Then
                If Not CorteXComprobanteVenta(I_Inicial, I_Final) Then Me.Close() : Exit Sub
                I_Inicial = I
                I_Final = I
                ClaveAnt = Row("TipoComprobante") & "/" & Row("Rec")
            Else
                I_Final = I
            End If
        Next
        If Not CorteXComprobanteVenta(I_Inicial, Row_Final) Then Me.Close() : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.Default

        MsgBox("FIN PROCESO")

    End Sub
    Private Function CorteXComprobanteVenta(ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim TipoComprobante As Integer = View(Inicial).Item("TipoComprobante")

        Select Case TipoComprobante
            Case 1
                If Not ProcesaFacturaParaVenta(Inicial, Final) Then Return False 'Facturas.
            Case 4
                If Not ProcesaNotaDeCreditoParaVenta(Inicial, Final) Then Return False 'Nota de Creditos con devolucion.
            Case 5, 7, 50, 70
                If Not ProcesaNotaParaVenta(View(Inicial).Item("TipoComprobante"), Inicial, Final) Then Return False 'Notas debito/credito financieras a clientes. 
            Case 100
                If Not ProcesaNVLPParaVenta(Inicial, Final) Then Return False 'N.V.L.P.
            Case 200
                If Not ProcesaLiquidacionParaVenta(Inicial, Final) Then Return False 'Liquidacion Productos. 
            Case 210
                If Not ProcesaLiquidacionInsumosParaVenta(Inicial, Final) Then Return False 'Liquidacion Insumos. 
        End Select

        Return True

    End Function
    Private Function ProcesaFacturaParaVenta(ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim ExentoW As Decimal = 0
        Dim NoGrabadoW As Decimal = 0
        Dim Row As DataRowView

        Dim NumeroLetra As Integer = Strings.Left(View(Inicial).Item("Comprobante"), 1)

        Dim Fecha As Date = View(Inicial).Item("Fecha")
        Dim TipoComprobante As Integer = 0
        Dim PuntoVenta As Integer = Strings.Mid(View(Inicial).Item("Comprobante").ToString, 2, 4)
        Dim Comprobante As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim ComprobanteHasta As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim CodigoDocuComprador As Integer = 80
        If View(Inicial).Item("Cuit") = 0 Then
            CodigoDocuComprador = 99
        End If
        Dim CuitComprador As Decimal = View(Inicial).Item("Cuit")
        Dim DenominacionComprador As String = View(Inicial).Item("Nombre")
        Dim ImporteTotal As Decimal = Trunca(CDec(View(Inicial).Item("Importe")) + CDec(View(Inicial).Item("Percepciones")))
        Dim TotalPercepciones As Decimal = View(Inicial).Item("Percepciones")
        Dim Senia As Decimal = CDec(View(Inicial).Item("Comision"))        ' Comision en facturas es la seña.
        Dim CodigoMoneda As String = "PES"
        Dim Cambio As Decimal = 1
        Dim CodigoOperacion As String = " "
        Dim FechaPago As String = "00000000"
        Dim EsZ As Boolean = View(Inicial).Item("EsZ")
        Dim EsFCE As Boolean = View(Inicial).Item("EsFCE")
        Dim ComprobanteDesdeZ As Integer = View(Inicial).Item("ComprobanteDesde")
        Dim ComprobanteHastaZ As Integer = View(Inicial).Item("ComprobanteHasta")

        If View(Inicial).Item("EsExterior") Then
            CodigoMoneda = HallaCodigoMonedaAfip(View(Inicial).Item("Moneda"))
            If CodigoMoneda = "" Then
                MsgBox("No Existe Codigo Afip de la moneda : " & NombreTabla(View(Inicial).Item("Moneda")), MsgBoxStyle.Critical)
                Return False
            End If
            Cambio = View(Inicial).Item("Cambio")
        End If

        TipoComprobante = HallaTipoAfip(NumeroLetra, 1, EsZ, EsFCE)
        If TipoComprobante = -1000 Then
            If MsgBox("Letra " & LetraTipoIva(NumeroLetra) & " en la Factura " & NumeroEditado(View(Inicial).Item("Comprobante")) & " No Prevista. " + vbCrLf + "Si Continua este Comprobante NO se Incluirá en Archivo para la AFIP." + vbCrLf + "Desae Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Return False
            Else
                Return True
            End If
        End If

        Dim TipoArticulo As Integer

        Dim Dt1 As New DataTable
        If Not Tablas.Read("SELECT EsServicios,ESSecos FROM FacturasCabeza WHERE Factura = " & View(Inicial).Item("Factura") & ";", Conexion, Dt1) Then Exit Function
        If Dt1.Rows(0).Item("EsServicios") Or Dt1.Rows(0).Item("EsSecos") Then
            TipoArticulo = 2
        Else
            TipoArticulo = 1
        End If
        Dt1.Dispose()

        ImporteIva = 0
        IvaComision = 0
        CantidadIvas = 0
        ImporteGrabado = 0
        ImporteNoGrabado = 0
        ImporteExento = 0
        ImportePercepcionIva = 0
        ImportePercepcionesOtrosImpuestosNacionales = 0
        ImportePercepcionIngresoBruto = 0
        ImportePercepcionesImpuestosMunicipales = 0
        ImportePercepcionesImpuestosInternos = 0
        ImporteOtrosTributos = 0
        ImportePercepciones = 0
        ImportePercepcionesNoCategorizada = 0

        AceraTabla(TablaIvas)

        For I As Integer = Inicial To Final
            ExentoW = 0 : NoGrabadoW = 0
            Row = View(I)
            Row("TotalArticulo") = Trunca(Row("TotalArticulo"))
            For J As Integer = 1 To UBound(TablaIvas)
                If Row("Iva") = TablaIvas(J).Alicuota Then
                    TablaIvas(J).Base = TablaIvas(J).Base + CalculaNeto(Row("Cantidad"), Row("Precio"))
                End If
            Next
            Dim Res As Integer = HallaCondicionIvaArticulo(TipoArticulo, Row("Articulo"))
            Select Case Res
                Case -1
                    Select Case TipoArticulo
                        Case 2
                            MsgBox("Error Base de Datos al Leer Tabla: Articulos para articulo & " & NombreArticuloServicios(Row("Articulo")) & " Operación se CANCELA") : Return False
                        Case 1
                            MsgBox("Error Base de Datos al Leer Tabla: Articulos para articulo & " & NombreArticulo(Row("Articulo")) & " Operación se CANCELA") : Return False
                    End Select
                Case 1
                    NoGrabadoW = Row("TotalArticulo")
                Case 2
                    NoGrabadoW = Row("TotalArticulo")
                Case 0
                    If Row("Iva") = 0 Then
                        NoGrabadoW = Row("TotalArticulo")
                    End If
            End Select
            ImporteNoGrabado = ImporteNoGrabado + NoGrabadoW
            ImporteExento = ImporteExento + ExentoW
        Next

        ImporteNoGrabado = ImporteNoGrabado + Senia + ImporteExento

        If TotalPercepciones <> 0 Then     'procesa percepciones realizadas.
            Dim DtRetenciones As New DataTable
            HallaPercepciones(2, View(Inicial).Item("Comprobante"), DtRetenciones)
            For Each RowP As DataRow In DtRetenciones.Rows
                Dim RowsBusqueda() As DataRow
                RowsBusqueda = DtPercepciones.Select("Clave = " & RowP("Percepcion"))
                If RowsBusqueda.Length <> 0 Then
                    If RowsBusqueda(0).Item("OrigenPercepcion") = 0 Then
                        MsgBox("Falta Código Afip de la Percepción " & NombreTabla(RowP("Percepcion")))
                        Return False
                    End If
                    AcumulaPercepciones(RowsBusqueda(0).Item("OrigenPercepcion"), RowP("Importe"), ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos)
                End If
            Next
        End If

        For J As Integer = 1 To UBound(TablaIvas)
            TablaIvas(J).Importe = CalculaIva(1, TablaIvas(J).Base, TablaIvas(J).Alicuota)
            ImporteIva = ImporteIva + TablaIvas(J).Importe
            If TablaIvas(J).Importe <> 0 Then
                CantidadIvas = CantidadIvas + 1
            End If
        Next

        If CantidadIvas = 0 Then
            CantidadIvas = 1
            CodigoOperacion = "N"
        End If

        If EsZ Then
            Comprobante = ComprobanteDesdeZ
            ComprobanteHasta = ComprobanteHastaZ
        End If

        CorteVenta(TablaIvas, Fecha, TipoComprobante, PuntoVenta, Comprobante, ComprobanteHasta, CodigoDocuComprador, CuitComprador, DenominacionComprador, ImporteTotal, ImporteNoGrabado, ImportePercepcionesNoCategorizada, ImporteExento, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos, CodigoMoneda, Cambio, CantidadIvas, CodigoOperacion, ImporteOtrosTributos, FechaPago)

        Return True

    End Function
    Private Function ProcesaNotaDeCreditoParaVenta(ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim ExentoW As Decimal = 0
        Dim NoGrabadoW As Decimal = 0
        Dim Row As DataRowView

        Dim NumeroLetra As Integer = Strings.Left(View(Inicial).Item("Comprobante"), 1)

        Dim Fecha As Date = View(Inicial).Item("Fecha")
        Dim TipoComprobante As Integer = 0
        Dim PuntoVenta As Integer = Strings.Mid(View(Inicial).Item("Comprobante").ToString, 2, 4)
        Dim Comprobante As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim ComprobanteHasta As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim CodigoDocuComprador As Integer = 80
        If View(Inicial).Item("Cuit") = 0 Then
            CodigoDocuComprador = 99
        End If
        Dim CuitComprador As Decimal = View(Inicial).Item("Cuit")
        Dim DenominacionComprador As String = View(Inicial).Item("Nombre")
        Dim ImporteTotal As Decimal = Trunca(View(Inicial).Item("Importe") + View(Inicial).Item("Percepciones"))
        Dim TotalPercepciones As Decimal = View(Inicial).Item("Percepciones")
        Dim CodigoMoneda As String = "PES"
        Dim Cambio As Decimal = 1
        Dim CodigoOperacion As String = " "
        Dim FechaPago As String = "00000000"
        Dim EsZ As Boolean = View(Inicial).Item("EsZ")
        Dim EsFCE As Boolean = View(Inicial).Item("EsFCE")
        Dim ComprobanteDesdeZ As Integer = View(Inicial).Item("ComprobanteDesde")
        Dim ComprobanteHastaZ As Integer = View(Inicial).Item("ComprobanteHasta")

        If View(Inicial).Item("EsExterior") Then
            CodigoMoneda = HallaCodigoMonedaAfip(View(Inicial).Item("Moneda"))
            If CodigoMoneda = "" Then
                MsgBox("No Existe Codigo Afip de la moneda : " & NombreTabla(View(Inicial).Item("Moneda")), MsgBoxStyle.Critical)
                Return False
            End If
            Cambio = View(Inicial).Item("Cambio")
        End If

        TipoComprobante = HallaTipoAfip(NumeroLetra, 2, EsZ, EsFCE)
        If TipoComprobante = -1000 Then
            If MsgBox("Letra " & LetraTipoIva(NumeroLetra) & " en la N.Credito Devolucion. " & NumeroEditado(View(Inicial).Item("Comprobante")) & " No Prevista. " + vbCrLf + "Si Continua este Comprobante NO se Incluirá en Archivo para la AFIP." + vbCrLf + "Desae Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Return False
            Else
                Return True
            End If
        End If

        Dim TipoArticulo As Integer

        Dim Dt1 As New DataTable
        If Not Tablas.Read("SELECT EsServicios,ESSecos FROM FacturasCabeza WHERE Factura = " & View(Inicial).Item("Factura") & ";", Conexion, Dt1) Then Exit Function
        If Dt1.Rows(0).Item("EsServicios") Or Dt1.Rows(0).Item("EsSecos") Then
            TipoArticulo = 2
        Else
            TipoArticulo = 1
        End If
        Dt1.Dispose()

        ImporteIva = 0
        IvaComision = 0
        CantidadIvas = 0
        ImporteGrabado = 0
        ImporteNoGrabado = 0
        ImporteExento = 0
        ImportePercepcionIva = 0
        ImportePercepcionesOtrosImpuestosNacionales = 0
        ImportePercepcionIngresoBruto = 0
        ImportePercepcionesImpuestosMunicipales = 0
        ImportePercepcionesImpuestosInternos = 0
        ImporteOtrosTributos = 0
        ImportePercepciones = 0
        ImportePercepcionesNoCategorizada = 0

        AceraTabla(TablaIvas)

        For I As Integer = Inicial To Final
            ExentoW = 0 : NoGrabadoW = 0
            Row = View(I)
            For J As Integer = 1 To UBound(TablaIvas)
                If Row("Iva") = TablaIvas(J).Alicuota Then
                    TablaIvas(J).Base = TablaIvas(J).Base + CalculaNeto(Row("Cantidad"), Row("Precio"))
                End If
            Next
            Dim Res As Integer = HallaCondicionIvaArticulo(TipoArticulo, Row("Articulo"))
            Select Case Res
                Case -1
                    Select Case TipoArticulo
                        Case 2
                            MsgBox("Error Base de Datos al Leer Tabla: Articulos para articulo & " & NombreArticuloServicios(Row("Articulo")) & " Operación se CANCELA") : Return False
                        Case 1
                            MsgBox("Error Base de Datos al Leer Tabla: Articulos para articulo & " & NombreArticulo(Row("Articulo")) & " Operación se CANCELA") : Return False
                    End Select
                Case 1
                    '    ExentoW = Row("TotalArticulo")
                    NoGrabadoW = Row("TotalArticulo")
                Case 2
                    NoGrabadoW = Row("TotalArticulo")
                Case 0
                    If Row("Iva") = 0 Then
                        NoGrabadoW = Row("TotalArticulo")
                    End If
            End Select
            ImporteNoGrabado = ImporteNoGrabado + NoGrabadoW
            ImporteExento = ImporteExento + ExentoW
        Next

        If TotalPercepciones <> 0 Then     'procesa percepciones realizadas.
            Dim DtRetenciones As New DataTable
            HallaPercepciones(4, View(Inicial).Item("Comprobante"), DtRetenciones)
            For Each RowP As DataRow In DtRetenciones.Rows
                Dim RowsBusqueda() As DataRow
                RowsBusqueda = DtPercepciones.Select("Clave = " & RowP("Percepcion"))
                If RowsBusqueda.Length <> 0 Then
                    If RowsBusqueda(0).Item("OrigenPercepcion") = 0 Then
                        MsgBox("Falta Código Afip de la Percepción " & NombreTabla(RowP("Percepcion")))
                        Return False
                    End If
                    AcumulaPercepciones(RowsBusqueda(0).Item("OrigenPercepcion"), RowP("Importe"), ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos)
                End If
            Next
        End If

        For J As Integer = 1 To UBound(TablaIvas)
            TablaIvas(J).Importe = CalculaIva(1, TablaIvas(J).Base, TablaIvas(J).Alicuota)
            ImporteIva = ImporteIva + TablaIvas(J).Importe
            If TablaIvas(J).Importe <> 0 Then
                CantidadIvas = CantidadIvas + 1
            End If
        Next

        If CantidadIvas = 0 Then
            CantidadIvas = 1
            CodigoOperacion = "N"
        End If

        If EsZ Then
            Comprobante = ComprobanteDesdeZ
            ComprobanteHasta = ComprobanteHastaZ
        End If

        CorteVenta(TablaIvas, Fecha, TipoComprobante, PuntoVenta, Comprobante, ComprobanteHasta, CodigoDocuComprador, CuitComprador, DenominacionComprador, ImporteTotal, ImporteNoGrabado, ImportePercepcionesNoCategorizada, ImporteExento, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos, CodigoMoneda, Cambio, CantidadIvas, CodigoOperacion, ImporteOtrosTributos, FechaPago)

        Return True

    End Function
    Private Function ProcesaNotaParaVenta(ByVal TipoNota As Integer, ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim ExentoW As Decimal = 0
        Dim NoGrabadoW As Decimal = 0
        Dim Row As DataRowView

        Select Case TipoNota
            Case 50, 70
                If Not Opcion1LibrosIva Then Return True
            Case 6, 8
                If Opcion1LibrosIva Then Return True
        End Select

        Dim NumeroLetra As Integer = Strings.Left(View(Inicial).Item("Comprobante"), 1)
        Select Case TipoNota
            Case 6, 8
                If NumeroLetra = 4 Then Return True 'No procesa si es importacion.
        End Select

        Dim Fecha As Date = View(Inicial).Item("Fecha")
        Dim TipoComprobante As Integer = 0
        Dim PuntoVenta As Integer = Strings.Mid(View(Inicial).Item("Comprobante").ToString, 2, 4)
        Dim Comprobante As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim ComprobanteHasta As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim CodigoDocuComprador As Integer = 80
        If View(Inicial).Item("Cuit") = 0 Then
            CodigoDocuComprador = 99
        End If
        Dim CuitComprador As Decimal = View(Inicial).Item("Cuit")
        Dim DenominacionComprador As String = View(Inicial).Item("Nombre")
        Dim ImporteTotal As Decimal = View(Inicial).Item("ImporteTotal")
        Dim CodigoMoneda As String = "PES"
        Dim EsFCE As Boolean = View(Inicial).Item("EsFCE")
        Dim Cambio As Decimal = 1
        Dim CodigoOperacion As String = " "
        Dim FechaPago As String = "00000000"
        Dim RowsBusqueda() As DataRow

        If View(Inicial).Item("EsExterior") Then
            CodigoMoneda = HallaCodigoMonedaAfip(View(Inicial).Item("Moneda"))
            If CodigoMoneda = "" Then
                MsgBox("No Existe Codigo Afip de la moneda : " & NombreTabla(View(Inicial).Item("Moneda")), MsgBoxStyle.Critical)
                Return False
            End If
            Cambio = View(Inicial).Item("Cambio")
        End If

        'Opcion 1.
        'Modificamos Las NC del Cliente(70) del TipoComprobante 003 al 002 se comporta como NDF al Cliente(5). 
        'Modificamos Las ND del Cliente(50) del TipoComprobante 002 al 003 se comporta como NCF al Cliente(7). 
        'Opcion 2.
        'Modificamos Las NC al Proveedor(8) del TipoComprobante 003 al 003 se comporta como NCF al Cliente(7).  
        'Modificamos Las ND al Proveedor(6) del TipoComprobante 002 al 002 se comporta como NDF al Cliente(5).  

        TipoComprobante = HallaTipoAfip(NumeroLetra, TipoNota, False, EsFCE)
        If TipoComprobante = -1000 Then
            If MsgBox("Letra " & LetraTipoIva(NumeroLetra) & " en la N.Credito/Debito. " & NumeroEditado(View(Inicial).Item("Comprobante")) & " No Prevista. " + vbCrLf + "Si Continua este Comprobante NO se Incluirá en Archivo para la AFIP." + vbCrLf + "Desae Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Return False
            Else
                Return True
            End If
        End If

        ImporteIva = 0
        IvaComision = 0
        CantidadIvas = 0
        ImporteGrabado = 0
        ImporteNoGrabado = 0
        ImporteExento = 0
        ImportePercepcionIva = 0
        ImportePercepcionesOtrosImpuestosNacionales = 0
        ImportePercepcionIngresoBruto = 0
        ImportePercepcionesImpuestosMunicipales = 0
        ImportePercepcionesImpuestosInternos = 0
        ImporteOtrosTributos = 0
        ImportePercepciones = 0
        ImportePercepcionesNoCategorizada = 0

        AceraTabla(TablaIvas)

        For I As Integer = Inicial To Final
            ExentoW = 0 : NoGrabadoW = 0
            Row = View(I)
            If Row("Impuesto") = 100 Then
                If Row("Iva") <> 0 Then
                    For J As Integer = 1 To UBound(TablaIvas)
                        If Row("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Base = TablaIvas(J).Base + CDec(Row("Importe"))
                            TablaIvas(J).Importe = TablaIvas(J).Importe + CalculaIva(1, Row("Importe"), Row("Iva"))
                            ImporteIva = ImporteIva + CalculaIva(1, Row("Importe"), Row("Iva"))
                        End If
                    Next
                Else
                    ImporteNoGrabado = ImporteNoGrabado + Row("Importe")
                End If
            End If
            RowsBusqueda = DtPercepciones.Select("Clave = " & Row("Impuesto"))
            If RowsBusqueda.Length <> 0 Then
                If RowsBusqueda(0).Item("OrigenPercepcion") = 0 Then
                    MsgBox("Falta Código Afip de la Percepción " & NombreTabla(Row("Impuesto")))
                    Return False
                End If
                AcumulaPercepciones(RowsBusqueda(0).Item("OrigenPercepcion"), Row("TotalArticulo"), ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos)
                ImportePercepciones = ImportePercepciones + Row("Importe")
            End If
        Next

        ImportePercepcionesOtrosImpuestosNacionales = ImportePercepcionesOtrosImpuestosNacionales + ImportePercepcionIva

        For J As Integer = 1 To UBound(TablaIvas)
            If TablaIvas(J).Importe <> 0 Then
                CantidadIvas = CantidadIvas + 1
            End If
        Next

        If CantidadIvas = 0 Then
            CantidadIvas = 1
            CodigoOperacion = "N"
        End If

        CorteVenta(TablaIvas, Fecha, TipoComprobante, PuntoVenta, Comprobante, ComprobanteHasta, CodigoDocuComprador, CuitComprador, DenominacionComprador, ImporteTotal, ImporteNoGrabado, ImportePercepcionesNoCategorizada, ImporteExento, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos, CodigoMoneda, Cambio, CantidadIvas, CodigoOperacion, ImporteOtrosTributos, FechaPago)

        Return True

    End Function
    Private Function ProcesaNVLPParaVenta(ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim ExentoW As Decimal = 0
        Dim NoGrabadoW As Decimal = 0
        Dim Row As DataRowView

        Dim NumeroLetra As Integer = Strings.Left(View(Inicial).Item("Comprobante"), 1)
        If NumeroLetra = 4 Then Return True 'No procesa si es exportacion.

        Dim Fecha As Date = View(Inicial).Item("Fecha")
        Dim TipoComprobante As Integer = 0
        Dim PuntoVenta As Integer = Strings.Mid(View(Inicial).Item("Comprobante").ToString, 2, 4)
        Dim Comprobante As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim ComprobanteHasta As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim CodigoDocuComprador As Integer = 80
        Dim CuitComprador As Decimal = View(Inicial).Item("Cuit")
        Dim DenominacionComprador As String = View(Inicial).Item("Nombre")
        Dim ImporteTotal As Decimal = 0
        Dim CodigoMoneda As String = "PES"
        Dim Cambio As Decimal = 1
        Dim CodigoOperacion As String = " "
        Dim FechaPago As String = "00000000"

        Select Case NumeroLetra
            Case 1
                TipoComprobante = 60
            Case 2
                TipoComprobante = 61
            Case 5
                TipoComprobante = 58
            Case Else
                If MsgBox("Letra " & LetraTipoIva(NumeroLetra) & " en la N.V.L.P. " & NumeroEditado(View(Inicial).Item("Comprobante")) & " No Prevista. " + vbCrLf + "Si Continua este Comprobante NO se Incluirá en Archivo para la AFIP." + vbCrLf + "Desae Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Return False
                Else
                    Return True
                End If
        End Select

        ImporteIva = 0
        IvaComision = 0
        CantidadIvas = 0
        ImporteGrabado = 0
        ImporteNoGrabado = 0
        ImporteExento = 0
        ImportePercepcionIva = 0
        ImportePercepcionesOtrosImpuestosNacionales = 0
        ImportePercepcionIngresoBruto = 0
        ImportePercepcionesImpuestosMunicipales = 0
        ImportePercepcionesImpuestosInternos = 0
        ImporteOtrosTributos = 0
        ImportePercepciones = 0
        ImportePercepcionesNoCategorizada = 0

        AceraTabla(TablaIvas)

        Dim Bruto As Decimal = 0

        Dim I As Integer
        Dim J As Integer

        For I = Inicial To Final
            Row = View(I)
            Select Case Row("Impuesto")
                Case 1
                    Bruto = Row("Importe")
                Case 3
                    For J = 1 To UBound(TablaIvas)
                        If Row("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Base = Bruto
                            TablaIvas(J).Importe = Row("Importe")
                            ImporteIva = Row("Importe")
                            Exit For
                        End If
                    Next
            End Select
        Next

        If ImporteIva = 0 Then ImporteNoGrabado = Bruto

        ImporteTotal = Bruto + ImporteNoGrabado + ImporteIva

        For J = 1 To UBound(TablaIvas)
            If TablaIvas(J).Importe <> 0 Then
                CantidadIvas = CantidadIvas + 1
            End If
        Next

        If CantidadIvas = 0 Then
            CantidadIvas = 1
            CodigoOperacion = "N"
        End If

        CorteVenta(TablaIvas, Fecha, TipoComprobante, PuntoVenta, Comprobante, ComprobanteHasta, CodigoDocuComprador, CuitComprador, DenominacionComprador, ImporteTotal, ImporteNoGrabado, ImportePercepcionesNoCategorizada, ImporteExento, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos, CodigoMoneda, Cambio, CantidadIvas, CodigoOperacion, ImporteOtrosTributos, FechaPago)

        Return True

    End Function
    Private Function ProcesaLiquidacionParaVenta(ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim ExentoW As Decimal = 0
        Dim NoGrabadoW As Decimal = 0
        Dim RowsBusqueda() As DataRow
        Dim Row As DataRowView

        Dim NumeroLetra As Integer = Strings.Left(View(Inicial).Item("Comprobante"), 1)

        Dim Fecha As Date = View(Inicial).Item("Fecha")
        Dim TipoComprobante As Integer = 0
        Dim PuntoVenta As Integer = Strings.Mid(View(Inicial).Item("Comprobante").ToString, 2, 4)
        Dim Comprobante As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim ComprobanteHasta As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim CodigoDocuComprador As Integer = 80
        Dim CuitComprador As Decimal = View(Inicial).Item("Cuit")
        Dim DenominacionComprador As String = View(Inicial).Item("Nombre")
        Dim ImporteTotal As Decimal = 0
        Dim CodigoMoneda As String = "PES"
        Dim Cambio As Decimal = 1
        Dim CodigoOperacion As String = " "
        Dim FechaPago As String = "00000000"

        Select Case NumeroLetra
            Case 1
                TipoComprobante = 63
            Case 2
                TipoComprobante = 64
            Case 3
                TipoComprobante = 68
            Case 5
                TipoComprobante = 59
            Case Else
                If MsgBox("Letra " & LetraTipoIva(NumeroLetra) & " en la Liquidación " & NumeroEditado(View(Inicial).Item("Comprobante")) & " No Prevista. " + vbCrLf + "Si Continua este Comprobante NO se Incluirá en Archivo para la AFIP." + vbCrLf + "Desae Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Return False
                Else
                    Return True
                End If
        End Select

        ImporteIva = 0
        IvaComision = 0
        CantidadIvas = 0
        ImporteGrabado = 0
        ImporteNoGrabado = 0
        ImporteExento = 0
        ImportePercepcionIva = 0
        ImportePercepcionesOtrosImpuestosNacionales = 0
        ImportePercepcionIngresoBruto = 0
        ImportePercepcionesImpuestosMunicipales = 0
        ImportePercepcionesImpuestosInternos = 0
        ImporteOtrosTributos = 0
        ImportePercepciones = 0
        ImportePercepcionesNoCategorizada = 0

        AceraTabla(TablaIvas)

        Dim Comision As Decimal = 0
        Dim Descarga As Decimal = 0
        Dim AlicuotaComision As Decimal = View(Inicial).Item("AlicuotaComision")
        Dim AlicuotaDescarga As Decimal = View(Inicial).Item("AlicuotaDescarga")

        Dim I As Integer
        Dim J As Integer

        For I = Inicial To Final
            Row = View(I)
            Select Case Row("Impuesto")
                Case 3
                    If View(I).Item("AlicuotaComision") = 0 Then
                        ImporteNoGrabado = ImporteNoGrabado + View(I).Item("Importe")
                    Else
                        Comision = View(I).Item("Importe")
                    End If
                Case 4
                    If View(I).Item("AlicuotaDescarga") = 0 Then
                        ImporteNoGrabado = ImporteNoGrabado + View(I).Item("Importe")
                    Else
                        Descarga = View(I).Item("Importe")
                    End If
                Case 5
                    For J = 1 To UBound(TablaIvas)
                        If View(I).Item("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Base = TablaIvas(J).Base + Comision
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(I).Item("Importe")
                            ImporteIva = ImporteIva + View(I).Item("Importe")
                            Exit For
                        End If
                    Next
                Case 6
                    For J = 1 To UBound(TablaIvas)
                        If View(I).Item("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Base = TablaIvas(J).Base + Descarga
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(I).Item("Importe")
                            ImporteIva = ImporteIva + View(I).Item("Importe")
                            Exit For
                        End If
                    Next
                Case Else
                    RowsBusqueda = DtPercepciones.Select("Clave = " & Row("Impuesto"))
                    If RowsBusqueda.Length <> 0 Then
                        If RowsBusqueda(0).Item("OrigenPercepcion") = 0 Then
                            MsgBox("Falta Código Afip de la Percepción " & NombreTabla(Row("Impuesto")))
                            Return False
                        End If
                        AcumulaPercepciones(RowsBusqueda(0).Item("OrigenPercepcion"), Row("TotalArticulo"), ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos)
                        ImportePercepciones = ImportePercepciones + Row("Importe")
                    End If
            End Select
        Next

        ImportePercepcionesOtrosImpuestosNacionales = ImportePercepcionesOtrosImpuestosNacionales + ImportePercepcionIva

        ImporteTotal = Comision + Descarga + ImporteNoGrabado + ImportePercepciones + ImporteIva

        For J = 1 To UBound(TablaIvas)
            If TablaIvas(J).Importe <> 0 Then
                CantidadIvas = CantidadIvas + 1
            End If
        Next

        If CantidadIvas = 0 Then
            CantidadIvas = 1
            CodigoOperacion = "N"
        End If

        CorteVenta(TablaIvas, Fecha, TipoComprobante, PuntoVenta, Comprobante, ComprobanteHasta, CodigoDocuComprador, CuitComprador, DenominacionComprador, ImporteTotal, ImporteNoGrabado, ImportePercepcionesNoCategorizada, ImporteExento, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos, CodigoMoneda, Cambio, CantidadIvas, CodigoOperacion, ImporteOtrosTributos, FechaPago)

        Return True

    End Function
    Private Function ProcesaLiquidacionInsumosParaVenta(ByVal Inicial As Integer, ByVal Final As Integer) As Boolean

        Dim ExentoW As Decimal = 0
        Dim NoGrabadoW As Decimal = 0
        Dim RowsBusqueda() As DataRow
        Dim Row As DataRowView

        Dim NumeroLetra As Integer = Strings.Left(View(Inicial).Item("Comprobante"), 1)

        Dim Fecha As Date = View(Inicial).Item("Fecha")
        Dim TipoComprobante As Integer = 0
        Dim PuntoVenta As Integer = Strings.Mid(View(Inicial).Item("Comprobante").ToString, 2, 4)
        Dim Comprobante As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim ComprobanteHasta As Integer = Strings.Right(View(Inicial).Item("Comprobante").ToString, 8)
        Dim CodigoDocuComprador As Integer = 80
        Dim CuitComprador As Decimal = View(Inicial).Item("Cuit")
        Dim DenominacionComprador As String = View(Inicial).Item("Nombre")
        Dim ImporteTotal As Decimal = 0
        Dim CodigoMoneda As String = "PES"
        Dim Cambio As Decimal = 1
        Dim CodigoOperacion As String = " "
        Dim FechaPago As String = "00000000"

        Select Case NumeroLetra
            Case 1
                TipoComprobante = 63
            Case 2
                TipoComprobante = 64
            Case 3
                TipoComprobante = 68
            Case 5
                TipoComprobante = 59
            Case Else
                If MsgBox("Letra " & LetraTipoIva(NumeroLetra) & " en Liquidación Insumos " & NumeroEditado(View(Inicial).Item("Comprobante")) & " No Prevista. " + vbCrLf + "Si Continua este Comprobante NO se Incluirá en Archivo para la AFIP." + vbCrLf + "Desae Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Return False
                Else
                    Return True
                End If
        End Select

        ImporteIva = 0
        IvaComision = 0
        CantidadIvas = 0
        ImporteGrabado = 0
        ImporteNoGrabado = 0
        ImporteExento = 0
        ImportePercepcionIva = 0
        ImportePercepcionesOtrosImpuestosNacionales = 0
        ImportePercepcionIngresoBruto = 0
        ImportePercepcionesImpuestosMunicipales = 0
        ImportePercepcionesImpuestosInternos = 0
        ImporteOtrosTributos = 0
        ImportePercepciones = 0
        ImportePercepcionesNoCategorizada = 0

        AceraTabla(TablaIvas)

        Dim Comision As Decimal = 0
        Dim InsumosDeProduccion As Decimal = 0
        Dim ServiciosDeProduccion As Decimal = 0
        Dim OtrosConceptos As Decimal = 0

        Dim I As Integer
        Dim J As Integer

        For I = Inicial To Final
            Row = View(I)
            Select Case Row("Impuesto")
                Case 5
                    Comision = View(I).Item("Importe")
                Case 7
                    InsumosDeProduccion = View(I).Item("Importe")
                Case 9
                    ServiciosDeProduccion = View(I).Item("Importe")
                Case 10
                    OtrosConceptos = View(I).Item("Importe")
                Case 6
                    For J = 1 To UBound(TablaIvas)
                        If View(I).Item("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Base = TablaIvas(J).Base + Comision
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(I).Item("Importe")
                            ImporteIva = ImporteIva + View(I).Item("Importe")
                        End If
                    Next
                    If View(I).Item("Importe") = 0 Then ImporteNoGrabado = ImporteNoGrabado + Comision : Comision = 0
                Case 8
                    For J = 1 To UBound(TablaIvas)
                        If View(I).Item("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Base = TablaIvas(J).Base + InsumosDeProduccion
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(I).Item("Importe")
                            ImporteIva = ImporteIva + View(I).Item("Importe")
                        End If
                    Next
                    If View(I).Item("Importe") = 0 Then ImporteNoGrabado = ImporteNoGrabado + InsumosDeProduccion : InsumosDeProduccion = 0
                Case 11
                    For J = 1 To UBound(TablaIvas)
                        If View(I).Item("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Base = TablaIvas(J).Base + ServiciosDeProduccion
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(I).Item("Importe")
                            ImporteIva = ImporteIva + View(I).Item("Importe")
                        End If
                    Next
                    If View(I).Item("Importe") = 0 Then ImporteNoGrabado = ImporteNoGrabado + ServiciosDeProduccion : ServiciosDeProduccion = 0
                Case 12
                    For J = 1 To UBound(TablaIvas)
                        If View(I).Item("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Base = TablaIvas(J).Base + OtrosConceptos
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(I).Item("Importe")
                            ImporteIva = ImporteIva + View(I).Item("Importe")
                        End If
                    Next
                    If View(I).Item("Importe") = 0 Then ImporteNoGrabado = ImporteNoGrabado + OtrosConceptos : OtrosConceptos = 0
                Case Else
                    RowsBusqueda = DtPercepciones.Select("Clave = " & Row("Impuesto"))
                    If RowsBusqueda.Length <> 0 Then
                        If RowsBusqueda(0).Item("OrigenPercepcion") = 0 Then
                            MsgBox("Falta Código Afip de la Percepción " & NombreTabla(Row("Impuesto")))
                            Return False
                        End If
                        AcumulaPercepciones(RowsBusqueda(0).Item("OrigenPercepcion"), Row("TotalArticulo"), ImportePercepcionIva, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos)
                        ImportePercepciones = ImportePercepciones + Row("Importe")
                    End If
            End Select
        Next

        ImportePercepcionesOtrosImpuestosNacionales = ImportePercepcionesOtrosImpuestosNacionales + ImportePercepcionIva

        ImporteTotal = Comision + InsumosDeProduccion + ServiciosDeProduccion + OtrosConceptos + ImporteNoGrabado + ImportePercepciones + ImporteIva

        For J = 1 To UBound(TablaIvas)
            If TablaIvas(J).Importe <> 0 Then
                CantidadIvas = CantidadIvas + 1
            End If
        Next

        If CantidadIvas = 0 Then
            CantidadIvas = 1
            CodigoOperacion = "N"
        End If

        CorteVenta(TablaIvas, Fecha, TipoComprobante, PuntoVenta, Comprobante, ComprobanteHasta, CodigoDocuComprador, CuitComprador, DenominacionComprador, ImporteTotal, ImporteNoGrabado, ImportePercepcionesNoCategorizada, ImporteExento, ImportePercepcionesOtrosImpuestosNacionales, ImportePercepcionIngresoBruto, ImportePercepcionesImpuestosMunicipales, ImportePercepcionesImpuestosInternos, CodigoMoneda, Cambio, CantidadIvas, CodigoOperacion, ImporteOtrosTributos, FechaPago)

        Return True

    End Function
    Private Function HallaTipoArticulos(ByVal Factura As Decimal, ByVal EsServicios As Boolean, ByVal EsSecos As Boolean) As Boolean

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT EsServicios,EsSecos FROM FacturasCabeza WHERE Factura = " & Factura & ";", Conexion, Dt) Then
            MsgBox("Error Base de Datos al leer Tabla: FacturasCabeza en factura " & NumeroEditado(Factura) & " Operación se CANCELA.", MsgBoxStyle.Critical)
            End
        End If
        If Dt.Rows.Count = 0 Then
            If Not MsgBox("Factura " & NumeroEditado(Factura) & " No Encontrada en Base de Datos.", MsgBoxStyle.Question + MsgBoxStyle.YesNo) Then
                Dt.Dispose()
                Return False
            End If
        End If

        EsServicios = Dt.Rows(0).Item("EsServicios")
        EsSecos = Dt.Rows(0).Item("EsSecos")

        Dt.Dispose()
        Return True

    End Function
    Private Sub AcumulaPercepciones(ByVal OrigenPrecepcion As Integer, ByVal Importe As Decimal, ByRef ImportePercepcionIva As Decimal, ByRef ImportePercepcionesOtrosImpuestosNacionales As Decimal, ByRef ImportePercepcionIngresoBruto As Decimal, ByRef ImportePercepcionesImpuestosMunicipales As Decimal, ByRef ImportePercepcionesImpuestosInternos As Decimal)

        Select Case OrigenPrecepcion
            Case 1
                ImportePercepcionIva = ImportePercepcionIva + Importe                                                        'Importe Iva.
            Case 2
                ImportePercepcionIngresoBruto = ImportePercepcionIngresoBruto + Importe                                      'Impuestos Ing.Bruto.
            Case 3
                ImportePercepcionesOtrosImpuestosNacionales = ImportePercepcionesOtrosImpuestosNacionales + Importe          'Impuestos Ing.Bruto.
            Case 4
                ImportePercepcionesImpuestosMunicipales = ImportePercepcionesImpuestosMunicipales + Importe                  'Impuestos Ing.Bruto.
            Case 5
                ImportePercepcionesImpuestosInternos = ImportePercepcionesImpuestosInternos + Importe                        'Impuestos Ing.Bruto.
        End Select

    End Sub
    Private Function FormateaImporteCITI(ByVal Importe As Decimal) As String

        Importe = Trunca(Importe)

        ' decimales del importe debe tener 1 o 2 digitos.

        Dim ImporteAux As Decimal = Importe
        If Importe < 0 Then Importe = -1 * Importe

        Dim Arr(2) As String
        Arr = Importe.ToString.Split(",")

        Dim EnteroStr As String = Arr(0)
        Do
            EnteroStr = "0" & EnteroStr
        Loop Until EnteroStr.Length = 13
        If ImporteAux < 0 Then EnteroStr = "-" & Strings.Right(EnteroStr, 12)

        Dim DecimalStr As String = ""
        If Arr.Length = 2 Then
            DecimalStr = Arr(1)
        End If
        If DecimalStr.Length = 1 Then DecimalStr = DecimalStr & "0"
        If DecimalStr.Length = 0 Then DecimalStr = DecimalStr & "00"

        Return EnteroStr & DecimalStr

    End Function
    Private Function FormateaImporte(ByVal Importe As Decimal) As String

        ' decimales del importe debe tener 1 o 2 digitos.

        Dim Arr(2) As String
        Arr = Importe.ToString.Split(",")

        Dim EnteroStr As String = Arr(0)
        Dim DecimalStr As String = ""
        If Arr.Length = 2 Then
            DecimalStr = Arr(1)
        End If
        If DecimalStr.Length = 1 Then DecimalStr = DecimalStr & "0"
        If DecimalStr.Length = 0 Then DecimalStr = DecimalStr & "00"

        Return EnteroStr & DecimalStr

    End Function
    Public Function FormateaTipoCambio(ByVal Importe As Decimal) As String

        ' decimales del importe debe tener 1 o 2 digitos.

        Dim Arr(2) As String
        Arr = Importe.ToString.Split(",")

        Dim EnteroStr As String = RellenarCeros(Arr(0), 4)
        Dim DecimalStr As String = ""
        If Arr.Length = 2 Then
            DecimalStr = Arr(1)
        End If
        DecimalStr = RellenarBlancos(DecimalStr, 6)
        DecimalStr = DecimalStr.Replace(" ", "0")

        Return EnteroStr & DecimalStr

    End Function
    Public Function HallaCodigoMonedaAfip(ByVal Moneda As Integer) As String

        Dim CodigoMonedaAfip As String = ""
        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT CodigoMonedaAfip FROM Tablas WHERE Clave = " & Moneda & ";", Conexion, Dt) Then
            Return ""
        End If
        If Dt.Rows.Count = 0 Then Return ""

        CodigoMonedaAfip = Dt.Rows(0).Item("CodigoMonedaAfip")

        Dt.Dispose()

        Return CodigoMonedaAfip

    End Function
    Private Function HallaPrimerosDigitos(ByVal Cuit As Decimal) As Integer

        Return Strings.Left(Cuit.ToString, 2)

    End Function
    Private Sub HallaPercepciones(ByVal TipoComprobante As Integer, ByVal Comprobante As Decimal, ByRef Dt As DataTable)

        If Not Tablas.Read("SELECT Percepcion,Importe FROM RecibosPercepciones WHERE TipoComprobante = " & TipoComprobante & " AND Comprobante = " & Comprobante & ";", Conexion, Dt) Then End

    End Sub
    Private Function HallaTipoAfip(ByVal TipoIva As Integer, ByVal TipoNota As Integer, ByVal EsZ As Boolean, ByVal EsFCE As Boolean) As Integer

        Select Case TipoNota
            Case 1
                If EsZ Then
                    Select Case TipoIva
                        Case 1
                            Return 81
                        Case 2
                            Return 82
                        Case 3
                            Return 111
                        Case 5
                            Return 118
                        Case 9
                            Return 83
                        Case Else
                            Return -1000
                    End Select
                End If
                If TipoIva = 4 Then Return 19
                If Not EsFCE Then
                    Return HallaTipoSegunAfip(TipoIva, TipoNota)
                Else
                    Return HallaTipoSegunAfipFCE(TipoIva, TipoNota)
                End If
            Case 2
                If EsZ Then
                    Select Case TipoIva
                        Case 1
                            Return 112
                        Case 2
                            Return 113
                        Case 3
                            Return 114
                        Case 5
                            Return 119
                        Case 9
                            Return 110
                        Case Else
                            Return -1000
                    End Select
                End If
                If TipoIva = 4 Then Return 21
                If Not EsFCE Then    'solo tipo iva 1,2,3,5.
                    Return HallaTipoSegunAfip(TipoIva, TipoNota)
                Else
                    Return HallaTipoSegunAfipFCE(TipoIva, TipoNota)
                End If
            Case 5, 6
                If TipoIva = 4 Then Return 20
                If Not EsFCE Then
                    Return HallaTipoSegunAfip(TipoIva, TipoNota)
                Else
                    Return HallaTipoSegunAfipFCE(TipoIva, TipoNota)
                End If
            Case 7, 8
                If TipoIva = 4 Then Return 21
                If Not EsFCE Then
                    Return HallaTipoSegunAfip(TipoIva, TipoNota)
                Else
                    Return HallaTipoSegunAfipFCE(TipoIva, TipoNota)
                End If
            Case 50
                Select Case TipoIva
                    Case 1
                        Return 3
                    Case 2
                        Return 8
                    Case 3
                        Return 13
                    Case 4
                        Return 21
                    Case 5
                        Return 53
                    Case Else
                        Return -1000
                End Select
            Case 70
                Select Case TipoIva
                    Case 1
                        Return 2
                    Case 2
                        Return 7
                    Case 3
                        Return 12
                    Case 4
                        Return 20
                    Case 5
                        Return 52
                    Case Else
                        Return -1000
                End Select
        End Select

    End Function
   
End Class