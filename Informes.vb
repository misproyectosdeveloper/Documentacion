Imports Microsoft.Office.Interop
Imports System.IO
Module Informes
    Dim ClienteW As Integer
    Dim NombreClienteW As String
    Dim CanalVentaW As String
    Public Sub DetalleLoteLiquidadosEnUnaOrdenPago(ByVal Orden As Double, ByVal ConexionStr As String)

        Dim DtRecibos As New DataTable
        If Not Tablas.Read("SELECT TipoComprobante,Comprobante,Importe As Imputado FROM RecibosDetalle WHERE TipoNota = 600 AND Nota = " & Orden & ";", ConexionStr, DtRecibos) Then
            MsgBox("Error Al Leer Orden Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If DtRecibos.Rows.Count = 0 Then
            MsgBox("Recibo No Tiene Documentos Imputados.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        'Muestra titulos.
        exHoja.Cells.Item(1, 9) = Date.Now
        exHoja.Cells.Item(2, 1) = "Orden pago : " & NumeroEditado(Orden)

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = "Factura/Liquidación"
        exHoja.Cells.Item(RowExxel, 2) = "Importe Fac./Liqui."
        exHoja.Cells.Item(RowExxel, 3) = "Lote          "
        exHoja.Cells.Item(RowExxel, 4) = "Articulo  "
        exHoja.Cells.Item(RowExxel, 5) = "Ingreso"
        exHoja.Cells.Item(RowExxel, 6) = "Cantidad"
        exHoja.Cells.Item(RowExxel, 7) = "Importe Lote"
        exHoja.Cells.Item(RowExxel, 8) = "Imputado"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        Dim DtComprobantes As New DataTable
        Dim DtLotes As New DataTable
        Dim Sql As String
        Dim SeniaPagada As Double = 0
        Dim ImporteFactura As Double = 0
        Dim ImporteLiquidacion As Double = 0
        Dim CoeficienteSeniaPagada As Double = 0
        Dim TotalImputado As Double = 0
        Dim ColExxel As Integer = 7

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        For Each Row As DataRow In DtRecibos.Rows
            If Row("TipoComprobante") = 2 Then
                DtComprobantes.Clear()
                Sql = "SELECT Rel,Nrel,Importe,ReciboOficial FROM FacturasProveedorCabeza WHERE EsReventa = 1 AND Factura = " & Row("Comprobante") & ";"
                If Not Tablas.Read(Sql, ConexionStr, DtComprobantes) Then
                    MsgBox("Error Al Leer Facturas Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If DtComprobantes.Rows.Count <> 0 Then
                    Dim FacturaRel As Double = HallaFacturaProveedorRelacionada(Row("Comprobante"), ConexionStr, DtComprobantes.Rows(0).Item("Rel"), DtComprobantes.Rows(0).Item("Nrel"))
                    DtLotes.Clear()
                    ImporteFactura = DtComprobantes.Rows(0).Item("Importe")
                    Dim ImporteW As Double = 0
                    If Not HallaLotesFacturados(Row("Comprobante"), FacturaRel, ConexionStr, DtLotes, ImporteW) Then
                        MsgBox("Error Al Leer Lotes Facturados.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    ImporteFactura = ImporteFactura + ImporteW
                    SeniaPagada = HallaSeniaEnFacturaReventa(Row("Comprobante"), FacturaRel, ConexionStr)
                    Dim TotalSeniaLote As Double = 0
                    For Each Row1 As DataRow In DtLotes.Rows
                        TotalSeniaLote = TotalSeniaLote + Row1("Senia")
                    Next
                    If TotalSeniaLote <> 0 Then
                        CoeficienteSeniaPagada = SeniaPagada / TotalSeniaLote
                    End If
                End If
            End If
            If Row("TipoComprobante") = 10 Then
                DtComprobantes.Clear()
                Sql = "SELECT Importe,Rel,Nrel FROM LiquidacionCabeza WHERE Liquidacion = " & Row("Comprobante") & ";"
                If Not Tablas.Read(Sql, ConexionStr, DtComprobantes) Then
                    MsgBox("Error Al Leer Liquidacion a Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If DtComprobantes.Rows.Count <> 0 Then
                    ImporteLiquidacion = DtComprobantes.Rows(0).Item("Importe")
                    Dim LiquidacionRel As Double = HallaLiquidacionRelacionada(Row("Comprobante"), ConexionStr, DtComprobantes.Rows(0).Item("Rel"), DtComprobantes.Rows(0).Item("Nrel"))
                    If LiquidacionRel <> 0 Then
                        Dim ImporteW As Double = 0
                        If ConexionStr = Conexion Then
                            ImporteW = HallaImporteLiquidacionRel(LiquidacionRel, ConexionN)
                        Else : ImporteW = HallaImporteLiquidacionRel(LiquidacionRel, Conexion)
                        End If
                        ImporteLiquidacion = ImporteLiquidacion + ImporteW
                    End If
                    DtLotes.Clear()
                    If Not HallaLotesLiquidados(Row("Comprobante"), LiquidacionRel, ConexionStr, DtLotes) Then
                        MsgBox("Error Al Leer Lotes Liquidados.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If DtLotes.Rows.Count <> 0 Then
                        Dim Contador As Integer = 0
                        For Each Row2 As DataRow In DtLotes.Rows
                            RowExxel = RowExxel + 1
                            If Contador = 0 Then
                                If Row("TipoComprobante") = 2 Then
                                    exHoja.Cells.Item(RowExxel, 1) = "Fac.    " & NumeroEditado(DtComprobantes.Rows(0).Item("ReciboOficial"))
                                    exHoja.Cells.Item(RowExxel, 2) = ImporteFactura
                                End If
                                If Row("TipoComprobante") = 10 Then
                                    exHoja.Cells.Item(RowExxel, 1) = "Liq.    " & NumeroEditado(Row("Comprobante"))
                                    exHoja.Cells.Item(RowExxel, 2) = ImporteLiquidacion
                                End If
                                exHoja.Cells.Item(RowExxel, 8) = Row("Imputado")
                                TotalImputado = TotalImputado + Row("Imputado")
                                Contador = 1
                            End If
                            '
                            Dim Articulo As Integer = 0
                            If Row2("Operacion") = 1 Then
                                Articulo = HallaArticulo(Row2("Lote"), Row2("Secuencia"), Conexion)
                            Else
                                Articulo = HallaArticulo(Row2("Lote"), Row2("Secuencia"), ConexionN)
                            End If
                            If Articulo <= 0 Then
                                MsgBox("Error al Leer Lotes " & Row2("Lote") & "/" & Format(Row2("Secuencia"), "000"), MsgBoxStyle.Critical)
                                Exit Sub
                            End If
                            Dim NombreArticuloW As String = ""
                            Dim ImporteBlanco As Double = 0
                            Dim NetoNegro As Double = 0
                            Dim FechaIngreso As New Date
                            Dim Cantidad As Integer = 0
                            If Not HallaDatosLote(Row2("Lote"), Row2("Secuencia"), Row2("Operacion"), FechaIngreso, Cantidad) Then Exit Sub
                            exHoja.Cells.Item(RowExxel, 5) = Format(FechaIngreso, "MM/dd/yyyy")
                            exHoja.Cells.Item(RowExxel, 6) = Cantidad
                            If Row("TipoComprobante") = 2 Then
                                If Not HallaNombreArticuloYNetos(Articulo, Row2("ImporteConIva"), Row2("ImporteSinIva"), NombreArticuloW, ImporteBlanco, NetoNegro) Then Exit Sub
                                exHoja.Cells.Item(RowExxel, 3) = Row2("Lote") & "/" & Format(Row2("Secuencia"), "000")
                                exHoja.Cells.Item(RowExxel, 4) = NombreArticuloW
                                exHoja.Cells.Item(RowExxel, 7) = Trunca(Row2("ImporteConIva") + Row2("Senia") * CoeficienteSeniaPagada)
                            End If
                            If Row("TipoComprobante") = 10 Then
                                NombreArticuloW = NombreArticulo(Articulo)
                                exHoja.Cells.Item(RowExxel, 3) = Row2("Lote") & "/" & Format(Row2("Secuencia"), "000")
                                exHoja.Cells.Item(RowExxel, 4) = NombreArticuloW
                                exHoja.Cells.Item(RowExxel, 7) = Row2("ImporteConIva")
                            End If
                        Next
                        RowExxel = RowExxel + 1
                    End If
                End If
            End If
        Next
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 1) = "Imputado en Orden de Pago"
        exHoja.Cells.Item(RowExxel, 8) = TotalImputado

        DtRecibos.Dispose()
        DtComprobantes.Dispose()
        DtLotes.Dispose()

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String = Chr(65 + 2 - 1) & InicioDatos
        Dim BB As String = Chr(65 + 2 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        exApp.Application.Visible = True
        '
        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub RemitosPorLote(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Deposito As Integer, ByVal Operacion As Integer)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------
        Dim Sql As String = "L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & " AND L.Deposito = " & Deposito
        Sql = "SELECT L.Comprobante,L.Cantidad,C.Fecha,C.Cliente FROM AsignacionLotes AS L INNER JOIN RemitosCabeza AS C ON L.TipoComprobante = 1 AND L.Comprobante = C.Remito WHERE L.Liquidado = 0 AND L.Facturado = 0 AND L.TipoComprobante = 1 AND " & Sql & ";"

        Dim ConexionStr As String = ""
        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim Dt As New DataTable
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then Exit Sub

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Comprobante"

        exApp.StandardFont = "Courier New"

        exHoja.Cells.Item(1, 4) = Format(Date.Now, "MM/dd/yyyy")

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = "Remito"
        exHoja.Cells.Item(RowExxel, 2) = "Fecha"
        exHoja.Cells.Item(RowExxel, 3) = "Cliente"
        exHoja.Cells.Item(RowExxel, 4) = "Cantidad"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Total As Decimal = 0

        For Each Row As DataRowView In View
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 1) = NumeroEditado(Row("Comprobante"))
            exHoja.Cells.Item(RowExxel, 2) = Format(Row("Fecha"), "MM/dd/yyyy")
            exHoja.Cells.Item(RowExxel, 3) = NombreCliente(Row("Cliente"))
            exHoja.Cells.Item(RowExxel, 4) = Row("Cantidad")
            Total = Total + Row("Cantidad")
        Next
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 1) = "Total"
        exHoja.Cells.Item(RowExxel, 4) = Total

        Dt.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 1 - 1) & InicioDatos
        BB = Chr(65 + 1 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(2, 1) = "Remitos del Lote : " & Lote & "/" & Format(Secuencia, "000")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub RemitosPorLoteIngresado(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As Integer)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim ConexionStr As String = ""
        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim DtLotes As New DataTable
        Dim Sql1 As String = "SELECT Lote,Secuencia,Deposito,Articulo FROM Lotes WHERE LoteOrigen = " & Lote & " AND SecuenciaOrigen = " & Secuencia & ";"
        If Not Tablas.Read(Sql1, ConexionStr, DtLotes) Then Exit Sub

        Dim Sql As String
        Dim Dt As New DataTable
        Dim SqlB As String
        Dim SqlN As String

        For Each Row As DataRow In DtLotes.Rows
            Sql = "L.Lote = " & Row("Lote") & " AND L.Secuencia = " & Row("Secuencia") & " AND L.Deposito = " & Row("Deposito")
            SqlB = "SELECT 1 AS Operacion,L.Lote,L.Secuencia,L.Comprobante,L.Cantidad,L.Facturado,L.Liquidado,C.Fecha,C.Cliente FROM AsignacionLotes AS L INNER JOIN RemitosCabeza AS C ON L.TipoComprobante = 1 AND L.Comprobante = C.Remito WHERE C.Estado <> 3 AND L.TipoComprobante = 1 AND " & Sql & ";"
            SqlN = "SELECT 2 as Operacion,L.Lote,L.Secuencia,L.Comprobante,L.Cantidad,L.Facturado,L.Liquidado,C.Fecha,C.Cliente FROM AsignacionLotes AS L INNER JOIN RemitosCabeza AS C ON L.TipoComprobante = 1 AND L.Comprobante = C.Remito WHERE C.Estado <> 3 AND L.TipoComprobante = 1 AND " & Sql & ";"
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Exit Sub
            If PermisoTotal Then
                If Not Tablas.Read(SqlN, ConexionN, Dt) Then Exit Sub
            End If
        Next

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Comprobante"

        exApp.StandardFont = "Courier New"

        exHoja.Cells.Item(1, 4) = Format(Date.Now, "MM/dd/yyyy")

        Dim RowExxel As Integer
        Dim RowsBusqueda() As DataRow

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Remito"
        exHoja.Cells.Item(RowExxel, 3) = "Fecha"
        exHoja.Cells.Item(RowExxel, 4) = "Cliente"
        exHoja.Cells.Item(RowExxel, 5) = "Cantidad"
        exHoja.Cells.Item(RowExxel, 6) = "        "
        exHoja.Cells.Item(RowExxel, 7) = "Lote"
        exHoja.Cells.Item(RowExxel, 8) = "Articulo"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Total As Decimal = 0

        For Each Row As DataRowView In View
            RowExxel = RowExxel + 1
            If Row("Operacion") = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
            exHoja.Cells.Item(RowExxel, 2) = NumeroEditado(Row("Comprobante"))
            exHoja.Cells.Item(RowExxel, 3) = Format(Row("Fecha"), "MM/dd/yyyy")
            exHoja.Cells.Item(RowExxel, 4) = NombreCliente(Row("Cliente"))
            exHoja.Cells.Item(RowExxel, 5) = Row("Cantidad")
            If Row("Facturado") Then exHoja.Cells.Item(RowExxel, 6) = "Facturado"
            If Row("Liquidado") Then exHoja.Cells.Item(RowExxel, 6) = "Liquidado"
            RowsBusqueda = DtLotes.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
            exHoja.Cells.Item(RowExxel, 7) = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
            exHoja.Cells.Item(RowExxel, 8) = HallaArticuloYKXEnvase(RowsBusqueda(0).Item("Articulo"))
            Total = Total + Row("Cantidad")
        Next
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 2) = "Total"
        exHoja.Cells.Item(RowExxel, 5) = Total

        DtLotes.Dispose()
        Dt.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(2, 1) = "Remitos del Lote : " & Lote & "/" & Format(Secuencia, "000")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub LotesReventaFactLiquidados(ByVal Proveedor As Integer, ByVal Nombre As String, ByVal Desde As Date, ByVal Hasta As Date, ByVal TextRemitos As TextBox)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim SqlFecha As String = ""
        SqlFecha = " AND L.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND L.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "

        Dim DtLotes As New DataTable
        If Not Tablas.Read("SELECT 1 As Operacion,L.*,C.Remito,C.Guia FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE L.Liquidado <> 0 AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 2 AND L.Proveedor = " & Proveedor & SqlFecha & ";", Conexion, DtLotes) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read("SELECT 2 As Operacion,L.*,C.Remito,C.Guia FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE L.Liquidado <> 0 AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 2 AND L.Proveedor = " & Proveedor & SqlFecha & ";", ConexionN, DtLotes) Then DtLotes.Dispose() : Exit Sub
        End If

        If TextRemitos.Lines.Length <> 0 Then
            For Each Row2 As DataRow In DtLotes.Rows
                Dim Comprobante As Decimal
                If Row2("Remito") <> 0 Then Comprobante = Row2("Remito")
                If Row2("Guia") <> 0 Then Comprobante = Row2("Guia")
                If Not EstaRemitoEnTextBox(Comprobante, TextRemitos) Then
                    Row2.Delete()
                End If
            Next
        End If

        Dim View As New DataView
        View = DtLotes.DefaultView
        View.Sort = "Lote,Secuencia"

        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = "Lote          "
        exHoja.Cells.Item(RowExxel, 2) = "Fecha  "
        exHoja.Cells.Item(RowExxel, 3) = "Articulo       "
        exHoja.Cells.Item(RowExxel, 4) = " "
        exHoja.Cells.Item(RowExxel, 5) = "Factura/Liquidación"
        exHoja.Cells.Item(RowExxel, 6) = "Cant. Inicial"
        exHoja.Cells.Item(RowExxel, 7) = "Devolución"
        exHoja.Cells.Item(RowExxel, 8) = "Cant. Neta"
        exHoja.Cells.Item(RowExxel, 9) = "Monto   "
        exHoja.Cells.Item(RowExxel, 10) = "Monto (2) "
        exHoja.Cells.Item(RowExxel, 11) = "Seña Articulo"
        exHoja.Cells.Item(RowExxel, 12) = "Remito/Guia"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim NetoConIva As Double = 0
        Dim NetoSinIva As Double = 0
        Dim Senia As Double = 0
        Dim Factura As Double = 0
        Dim Liquidacion As Double = 0
        Dim ReciboOficial As Double = 0
        Dim ConexionFactura As String = ""
        Dim Marca As String
        Dim Tipo As String

        For Each Row As DataRowView In View
            Liquidacion = 0 : Factura = 0
            If Not HallaNetosYFacturas(Row("Lote"), Row("Secuencia"), NetoConIva, NetoSinIva, Factura, ReciboOficial, ConexionFactura) Then DtLotes.Dispose() : Exit Sub
            If Factura = 0 Then
                If Not HallaNetosYLiquidacion(Row("Lote"), Row("Secuencia"), NetoConIva, NetoSinIva, Liquidacion, ReciboOficial, ConexionFactura) Then DtLotes.Dispose() : Exit Sub
            End If
            If ConexionFactura = Conexion Then
                Marca = ""
            Else
                Marca = "X"
            End If
            Dim NombreArticuloW As String = ""
            Dim ImporteBlanco As Double = 0
            Dim ImporteNegro As Double = 0
            If Factura <> 0 Then
                If Not HallaNombreArticuloYNetos(Row("Articulo"), NetoConIva, NetoSinIva, NombreArticuloW, ImporteBlanco, ImporteNegro) Then Exit Sub
            End If
            If Liquidacion <> 0 Then
                NombreArticuloW = NombreArticulo(Row("Articulo"))
                ImporteBlanco = NetoConIva
                ImporteNegro = NetoSinIva
            End If
            '
            If Factura <> 0 Or Liquidacion <> 0 Then
                If Row("Senia") = -1 Then
                    Dim Envase As Integer = HallaEnvase(Row("Articulo"))
                    If Envase < 0 Then Exit Sub
                    Senia = CalculaSenia(Envase, Row("Fecha"))
                    If Senia < 0 Then Exit Sub
                Else
                    Senia = Row("Senia")
                End If
                Senia = CalculaNeto((Row("Cantidad") - Row("Baja")), Senia)
                '
                If Factura <> 0 Then Tipo = "Fac."
                If Liquidacion <> 0 Then Tipo = "Liq."
                RowExxel = RowExxel + 1
                exHoja.Cells.Item(RowExxel, 1) = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
                exHoja.Cells.Item(RowExxel, 2) = Format(Row("Fecha"), "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel, 3) = NombreArticuloW
                exHoja.Cells.Item(RowExxel, 4) = Marca
                exHoja.Cells.Item(RowExxel, 5) = Tipo & " " & NumeroEditado(ReciboOficial)
                exHoja.Cells.Item(RowExxel, 6) = Row("Cantidad")
                exHoja.Cells.Item(RowExxel, 7) = Row("Baja")
                exHoja.Cells.Item(RowExxel, 8) = Row("Cantidad") - Row("Baja")
                exHoja.Cells.Item(RowExxel, 9) = ImporteBlanco
                exHoja.Cells.Item(RowExxel, 10) = ImporteNegro
                exHoja.Cells.Item(RowExxel, 11) = Senia
                Dim Remito As Decimal
                If HallaRemitoGuia(Row("Lote"), Row("Operacion"), Remito) Then
                    exHoja.Cells.Item(RowExxel, 12) = NumeroEditado(Remito)
                End If
            End If
        Next

        DtLotes.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 1 - 1) & InicioDatos
        BB = Chr(65 + 1 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 12) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Lotes Facturados/Liquidados del Proveedor : " & Nombre

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub LotesReventaPendientes(ByVal Proveedor As Integer, ByVal Nombre As String, ByVal Desde As Date, ByVal Hasta As Date, ByVal TextRemitos As TextBox)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim SqlFecha As String = ""
        SqlFecha = " AND L.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND L.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "

        Dim DtLotes As New DataTable
        If Not Tablas.Read("SELECT 1 As Operacion,L.*,C.Remito,C.Guia FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE L.Liquidado = 0 AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 2 AND L.Proveedor = " & Proveedor & SqlFecha & ";", Conexion, DtLotes) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read("SELECT 2 As Operacion,L.*,C.Remito,C.Guia FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE L.Liquidado = 0 AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.TipoOperacion = 2 AND L.Proveedor = " & Proveedor & SqlFecha & ";", ConexionN, DtLotes) Then DtLotes.Dispose() : Exit Sub
        End If

        If TextRemitos.Lines.Length <> 0 Then
            For Each Row2 As DataRow In DtLotes.Rows
                Dim Comprobante As Decimal
                If Row2("Remito") <> 0 Then Comprobante = Row2("Remito")
                If Row2("Guia") <> 0 Then Comprobante = Row2("Guia")
                If Not EstaRemitoEnTextBox(Comprobante, TextRemitos) Then
                    Row2.Delete()
                End If
            Next
        End If

        Dim View As New DataView
        View = DtLotes.DefaultView
        View.Sort = "Lote,Secuencia"

        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Lote          "
        exHoja.Cells.Item(RowExxel, 3) = "Fecha  "
        exHoja.Cells.Item(RowExxel, 4) = "Articulo       "
        exHoja.Cells.Item(RowExxel, 5) = "Cant. Inicial"
        exHoja.Cells.Item(RowExxel, 6) = "Devolución"
        exHoja.Cells.Item(RowExxel, 7) = "Cant. Neta"
        exHoja.Cells.Item(RowExxel, 8) = "Monto   "
        exHoja.Cells.Item(RowExxel, 9) = "Seña Articulo"
        exHoja.Cells.Item(RowExxel, 10) = "Base Calculo"
        exHoja.Cells.Item(RowExxel, 11) = "Remito/Guia"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim NetoConIva As Decimal = 0
        Dim NetoSinIva As Decimal = 0
        Dim ConexionFactura As String = ""
        Dim Marca As String

        Dim Saldo As Decimal
        Dim Remitido As Decimal
        Dim Facturado As Decimal
        Dim Baja As Decimal
        Dim Merma As Decimal
        Dim MermaTr As Decimal
        Dim Importe As Decimal
        Dim ImporteSinIva As Decimal
        Dim Senia As Decimal
        Dim PrecioS As Decimal
        Dim PrecioSSinIva As Decimal
        Dim Stock As Decimal
        Dim PrecioF As Decimal
        Dim Liquidado As Decimal
        Dim PrecioCompra As Decimal
        Dim CantidadInicial As Decimal
        Dim Descarga As Decimal
        Dim DescargaSinIva As Decimal
        Dim GastoComercial As Decimal
        Dim GastoComercialSinIva As Decimal
        Dim Cartel As String

        For Each Row As DataRowView In View
            Dim ListaOrden As String
            If Not AnalisisCostoLote(True, Row("Operacion"), Row("Proveedor"), Row("Lote"), Row("Secuencia"), Saldo, PrecioS, PrecioSSinIva, Remitido, Facturado, Importe, ImporteSinIva, Baja, Merma, MermaTr, Stock, Liquidado, PrecioF, PrecioCompra, CantidadInicial, Senia, Descarga, DescargaSinIva, GastoComercial, GastoComercialSinIva, False, False, ListaOrden) Then Exit Sub
            RowExxel = RowExxel + 1
            If Row("Operacion") = 1 Then
                Marca = ""
            Else
                Marca = "X"
            End If
            exHoja.Cells.Item(RowExxel, 1) = Marca
            exHoja.Cells.Item(RowExxel, 2) = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
            exHoja.Cells.Item(RowExxel, 3) = Format(Row("Fecha"), "MM/dd/yyyy")
            exHoja.Cells.Item(RowExxel, 4) = NombreArticulo(Row("Articulo"))
            exHoja.Cells.Item(RowExxel, 5) = Row("Cantidad")
            exHoja.Cells.Item(RowExxel, 6) = Row("Baja")
            exHoja.Cells.Item(RowExxel, 7) = CDec(Row("Cantidad")) - CDec(Row("Baja"))
            Cartel = ""
            Dim Precio As Double = 0
            If PrecioF <> 0 Then
                Precio = PrecioF
                Cartel = "Precio Final"
            Else
                Precio = PrecioCompra
                Cartel = "Precio Compra"
            End If
            exHoja.Cells.Item(RowExxel, 8) = CalculaNeto(Row("Cantidad") - Row("Baja"), Precio)
            exHoja.Cells.Item(RowExxel, 9) = Senia
            exHoja.Cells.Item(RowExxel, 10) = Cartel
            Dim Remito As Decimal
            If HallaRemitoGuia(Row("Lote"), Row("Operacion"), Remito) Then
                exHoja.Cells.Item(RowExxel, 11) = NumeroEditado(Remito)
            End If
        Next

        DtLotes.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 1 - 1) & InicioDatos
        BB = Chr(65 + 1 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 10) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Lotes Pendientes del Proveedor : " & Nombre

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeParaInventario(ByVal Especie As Integer, ByVal Variedad As Integer, ByVal Marca As Integer, ByVal Categoria As Integer, ByVal Envase As Integer, ByVal Deposito As Integer, _
                                     ByVal ListaComprobantes As List(Of Decimal), ByVal ListaComprobantesCerrado As List(Of Decimal), ByVal ConPedidos As Boolean)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Articulo       "
        exHoja.Cells.Item(RowExxel, 3) = "Especie"
        exHoja.Cells.Item(RowExxel, 4) = "Variedad"
        exHoja.Cells.Item(RowExxel, 5) = "Kgs.X Uni."
        exHoja.Cells.Item(RowExxel, 6) = "En Stock"
        exHoja.Cells.Item(RowExxel, 7) = "Pendiente"
        exHoja.Cells.Item(RowExxel, 8) = "Stock Real"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Sql As String = ""

        If Especie <> 0 Then
            If Sql = "" Then
                Sql = "Especie = " & Especie
            Else : Sql = "AND Especie = " & Especie
            End If
        End If
        If Variedad <> 0 Then
            If Sql = "" Then
                Sql = "Variedad = " & Variedad
            Else : Sql = Sql & " AND Variedad = " & Variedad
            End If
        End If
        If Marca <> 0 Then
            If Sql = "" Then
                Sql = "Marca = " & Marca
            Else : Sql = Sql & " AND Marca = " & Marca
            End If
        End If
        If Categoria <> 0 Then
            If Sql = "" Then
                Sql = "Categoria = " & Categoria
            Else : Sql = Sql & " AND Categoria = " & Categoria
            End If
        End If
        If Envase <> 0 Then
            If Sql = "" Then
                Sql = "Envase = " & Envase
            Else : Sql = Sql & " AND Envase = " & Envase
            End If
        End If

        If Sql <> "" Then Sql = "WHERE " & Sql

        Dim SqlDepo As String = ""
        If Deposito <> 0 Then SqlDepo = " AND Deposito = " & Deposito

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT A.Clave,A.Especie,A.Variedad,A.Nombre,E.Kilos FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave " & Sql & ";", Conexion, Dt) Then Exit Sub

        Dim SqlB As String = "SELECT 1 AS Operacion,1 AS Tipo,Stock As Cantidad,Articulo,0 As Comprobante FROM Lotes WHERE Stock <> 0 " & SqlDepo & _
                             " UNION ALL " & _
                             "SELECT 1 AS Operacion,2 AS Tipo,(D.Cantidad - D.Devueltas) As Cantidad,D.Articulo,C.Remito AS Comprobante FROM RemitosCabeza As C INNER JOIN RemitosDetalle AS D ON C.Remito = D.Remito WHERE C.Estado = 2 AND C.Factura = 0 " & SqlDepo & _
                             " UNION ALL " & _
                             "SELECT 1 AS Operacion,2 AS Tipo,(D.Cantidad - D.Devueltas) As Cantidad,D.Articulo,C.Factura AS Compronamte FROM FacturasCabeza As C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE C.Tr = 0 AND C.EsServicios = 0 AND C.EsSecos = 0 AND Articulo <> 0 AND C.Estado = 2 " & SqlDepo & ";"

        Dim SqlN As String = "SELECT 2 AS Operacion,1 AS Tipo,Stock As Cantidad,Articulo,0 As Comprobante FROM Lotes WHERE Stock <> 0 " & SqlDepo & _
                             " UNION ALL " & _
                             "SELECT 2 AS Operacion,2 AS Tipo,(D.Cantidad - D.Devueltas) As Cantidad,D.Articulo,C.Remito AS Comprobante FROM RemitosCabeza As C INNER JOIN RemitosDetalle AS D ON C.Remito = D.Remito WHERE C.Estado = 2 AND C.Factura = 0 " & SqlDepo & _
                             " UNION ALL " & _
                             "SELECT 2 AS Operacion,2 AS Tipo,(D.Cantidad - D.Devueltas) As Cantidad,D.Articulo,C.Factura AS Compronamte FROM FacturasCabeza As C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE C.Tr = 0 AND C.EsServicios = 0 AND C.EsSecos = 0 AND Articulo <> 0 AND C.Rel = 0 AND C.Estado = 2 " & SqlDepo & ";"

        Dim DtStock As New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtStock) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtStock) Then Exit Sub
        End If

        If ConPedidos Then
            SqlB = "SELECT 1 AS Operacion, 2 AS Tipo,(D.Cantidad - D.Entregada) AS Cantidad,Articulo,C.Pedido As Comprobante FROM PedidosCabeza AS C INNER JOIN PedidosDetalle AS D ON C.Pedido = D.Pedido WHERE Cerrado = 0 AND D.Cantidad > D.Entregada;"
            If Not Tablas.Read(SqlB, Conexion, DtStock) Then Exit Sub
        End If

        Dim ListaArticulos As New List(Of ItemPedido)
        If ListaComprobantes.Count <> 0 Or ListaComprobantesCerrado.Count <> 0 Then
            ListaArticulos = HallaArticulosDeComprobantes(Deposito, ListaComprobantes, ListaComprobantesCerrado)
        End If

        For Each Row1 As DataRow In DtStock.Rows
            If Row1("Tipo") = 2 Then
                If Row1("Operacion") = 1 Then
                    For Each Fila As Decimal In ListaComprobantes
                        If Fila = Row1("Comprobante") Then
                            Row1.Delete()
                        End If
                    Next
                Else
                    For Each Fila As Decimal In ListaComprobantesCerrado
                        If Fila = Row1("Comprobante") Then Row1.Delete()
                    Next
                End If
            End If
        Next

        Dim View As New DataView
        View = DtStock.DefaultView
        View.Sort = "Articulo"

        Dim Stock As Decimal = 0
        Dim Pendiente As Decimal = 0
        Dim StockReal As Decimal = 0
        Dim ArticuloAnt As Integer = 0
        Dim RowsBusqueda() As DataRow

        If View.Count = 0 Then
            MsgBox("No Hay Stock.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim Row As DataRowView
        Row = View(0)
        ArticuloAnt = Row("Articulo")

        For Each Row In View
            If ArticuloAnt <> Row("Articulo") Then
                RowsBusqueda = Dt.Select("Clave = " & ArticuloAnt)
                If RowsBusqueda.Length <> 0 Then
                    RowExxel = RowExxel + 1
                    exHoja.Cells.Item(RowExxel, 2) = RowsBusqueda(0).Item("Nombre")
                    exHoja.Cells.Item(RowExxel, 3) = NombreEspecie(RowsBusqueda(0).Item("Especie"))
                    exHoja.Cells.Item(RowExxel, 4) = NombreVariedad(RowsBusqueda(0).Item("Variedad"))
                    exHoja.Cells.Item(RowExxel, 5) = RowsBusqueda(0).Item("Kilos")
                    If ListaArticulos.Count <> 0 Then
                        For Each Fila As ItemPedido In ListaArticulos
                            If Fila.Articulo = ArticuloAnt Then Stock = Stock - Fila.Cantidad
                        Next
                    End If
                    exHoja.Cells.Item(RowExxel, 6) = Stock
                    exHoja.Cells.Item(RowExxel, 7) = Pendiente
                    exHoja.Cells.Item(RowExxel, 8) = Stock - Pendiente
                End If
                Stock = 0
                Pendiente = 0
                ArticuloAnt = Row("Articulo")
            End If
            If Row("Tipo") = 1 Then Stock = Stock + Row("Cantidad")
            If Row("Tipo") = 2 Then Pendiente = Pendiente + Row("Cantidad")
        Next
        RowsBusqueda = Dt.Select("Clave = " & ArticuloAnt)
        If RowsBusqueda.Length <> 0 Then
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 2) = RowsBusqueda(0).Item("Nombre")
            exHoja.Cells.Item(RowExxel, 3) = NombreEspecie(RowsBusqueda(0).Item("Especie"))
            exHoja.Cells.Item(RowExxel, 4) = NombreVariedad(RowsBusqueda(0).Item("Variedad"))
            exHoja.Cells.Item(RowExxel, 5) = RowsBusqueda(0).Item("Kilos")
            exHoja.Cells.Item(RowExxel, 6) = Stock
            exHoja.Cells.Item(RowExxel, 7) = Pendiente
            exHoja.Cells.Item(RowExxel, 8) = Trunca(Stock - Pendiente)
        End If

        Dt.Dispose()
        DtStock.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 10) = Format(Date.Now, "MM/dd/yyyy")
        Dim Str2 As String = ""
        If ConPedidos Then Str2 = ".Incluye Artículos de Pedidos aun Abiertos."
        If Deposito = 0 Then
            exHoja.Cells.Item(2, 1) = "Artículos En Stock Real: Todos Los depositos." & NombreDeposito(Deposito) & Str2
        Else
            exHoja.Cells.Item(2, 1) = "Artículos En Stock Real: " & NombreDeposito(Deposito) & Str2
        End If

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeParaInventarioDetalle(ByVal Especie As Integer, ByVal Variedad As Integer, ByVal Marca As Integer, ByVal Categoria As Integer, ByVal Envase As Integer, ByVal Deposito As Integer, _
                                            ByVal ListaComprobantes As List(Of Decimal), ByVal ListaComprobantesCerrado As List(Of Decimal), ByVal ConPedidos As Boolean)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = "Articulo       "
        exHoja.Cells.Item(RowExxel, 2) = "Especie"
        exHoja.Cells.Item(RowExxel, 3) = "Variedad"
        exHoja.Cells.Item(RowExxel, 4) = "Kg. X Uni."
        exHoja.Cells.Item(RowExxel, 5) = " "
        exHoja.Cells.Item(RowExxel, 6) = " "
        exHoja.Cells.Item(RowExxel, 7) = "Comprobante"
        exHoja.Cells.Item(RowExxel, 8) = "Pendiente"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Sql As String = ""

        If Especie <> 0 Then
            If Sql = "" Then
                Sql = "Especie = " & Especie
            Else : Sql = "AND Especie = " & Especie
            End If
        End If
        If Variedad <> 0 Then
            If Sql = "" Then
                Sql = "Variedad = " & Variedad
            Else : Sql = Sql & " AND Variedad = " & Variedad
            End If
        End If
        If Marca <> 0 Then
            If Sql = "" Then
                Sql = "Marca = " & Marca
            Else : Sql = Sql & " AND Marca = " & Marca
            End If
        End If
        If Categoria <> 0 Then
            If Sql = "" Then
                Sql = "Categoria = " & Categoria
            Else : Sql = Sql & " AND Categoria = " & Categoria
            End If
        End If
        If Envase <> 0 Then
            If Sql = "" Then
                Sql = "Envase = " & Envase
            Else : Sql = Sql & " AND Envase = " & Envase
            End If
        End If

        If Sql <> "" Then Sql = "WHERE " & Sql

        Dim SqlDepo As String = ""
        If Deposito <> 0 Then SqlDepo = " AND Deposito = " & Deposito

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT A.Clave,A.Especie,A.Variedad,A.Nombre,E.Kilos FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave " & Sql & ";", Conexion, Dt) Then Exit Sub

        Dim SqlB As String = "SELECT 1 AS Operacion,1 AS Tipo,Stock As Cantidad,Articulo,0 AS Comprobante FROM Lotes WHERE Stock <> 0 " & SqlDepo & _
                             " UNION ALL " & _
                             "SELECT 1 AS Operacion,2 AS Tipo,(D.Cantidad - D.Devueltas) As Cantidad,D.Articulo,C.Remito AS Comprobante FROM RemitosCabeza As C INNER JOIN RemitosDetalle AS D ON C.Remito = D.Remito WHERE C.Estado = 2 AND C.Factura = 0 " & SqlDepo & _
                             " UNION ALL " & _
                             "SELECT 1 AS Operacion,3 AS Tipo,(D.Cantidad - D.Devueltas) As Cantidad,D.Articulo,C.Factura AS Comprobante FROM FacturasCabeza As C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE C.Tr = 0 AND C.EsServicios = 0 AND C.EsSecos = 0 AND Articulo <> 0 AND C.Estado = 2 " & SqlDepo & ";"

        Dim SqlN As String = "SELECT 2 AS Operacion,1 AS Tipo,Stock As Cantidad,Articulo,0 AS Comprobante  FROM Lotes WHERE Stock <> 0 " & SqlDepo & _
                             " UNION ALL " & _
                             "SELECT 2 AS Operacion,2 AS Tipo,(D.Cantidad - D.Devueltas) As Cantidad,D.Articulo,C.Remito AS Comprobante FROM RemitosCabeza As C INNER JOIN RemitosDetalle AS D ON C.Remito = D.Remito WHERE C.Estado = 2 AND C.Factura = 0 " & SqlDepo & _
                             " UNION ALL " & _
                             "SELECT 2 AS Operacion,3 AS Tipo,(D.Cantidad - D.Devueltas) As Cantidad,D.Articulo,C.Factura AS Comprobante FROM FacturasCabeza As C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE C.Tr = 0 AND C.EsServicios = 0 AND C.EsSecos = 0 AND Articulo <> 0 AND C.Rel = 0 AND C.Estado = 2 " & SqlDepo & ";"

        Dim DtStock As New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtStock) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtStock) Then Exit Sub
        End If

        If ConPedidos Then
            SqlB = "SELECT 1 AS Operacion, 4 AS Tipo,(D.Cantidad - D.Entregada) AS Cantidad,Articulo,C.Pedido As Comprobante FROM PedidosCabeza AS C INNER JOIN PedidosDetalle AS D ON C.Pedido = D.Pedido WHERE Cerrado = 0 AND D.Cantidad > D.Entregada;"
            If Not Tablas.Read(SqlB, Conexion, DtStock) Then Exit Sub
        End If

        Dim ListaArticulos As New List(Of ItemPedido)
        If ListaComprobantes.Count <> 0 Or ListaComprobantesCerrado.Count <> 0 Then
            ListaArticulos = HallaArticulosDeComprobantes(Deposito, ListaComprobantes, ListaComprobantesCerrado)
        End If

        For Each Row1 As DataRow In DtStock.Rows
            If Row1("Tipo") = 2 Or Row1("Tipo") = 3 Then
                If Row1("Operacion") = 1 Then
                    For Each Fila As Decimal In ListaComprobantes
                        If Fila = Row1("Comprobante") Then
                            Row1.Delete()
                        End If
                    Next
                Else
                    For Each Fila As Decimal In ListaComprobantesCerrado
                        If Fila = Row1("Comprobante") Then Row1.Delete()
                    Next
                End If
            End If
        Next

        Dim View As New DataView
        View = DtStock.DefaultView
        View.Sort = "Articulo"

        Dim Stock As Decimal = 0
        Dim Pendiente As Decimal = 0
        Dim StockReal As Decimal = 0
        Dim ArticuloAnt As Integer = 0
        Dim RowsBusqueda() As DataRow

        If View.Count = 0 Then
            MsgBox("No Hay Stock.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim Row As DataRowView
        Row = View(0)
        ArticuloAnt = Row("Articulo")
        ArticuloAnt = 0

        For Each Row In View
            If ArticuloAnt <> Row("Articulo") Then
                If Stock <> 0 Or Pendiente <> 0 Then
                    RowExxel = RowExxel + 1
                    exHoja.Cells.Item(RowExxel, 1) = "Total Pendientes"
                    exHoja.Cells.Item(RowExxel, 8) = Pendiente
                    RowExxel = RowExxel + 1
                    exHoja.Cells.Item(RowExxel, 1) = "Total Stock"
                    If ListaArticulos.Count <> 0 Then
                        For Each Fila As ItemPedido In ListaArticulos
                            If Fila.Articulo = ArticuloAnt Then Stock = Stock - Fila.Cantidad
                        Next
                    End If
                    exHoja.Cells.Item(RowExxel, 8) = Stock
                    RowExxel = RowExxel + 1
                    exHoja.Cells.Item(RowExxel, 1) = "Total Stock Real"
                    exHoja.Cells.Item(RowExxel, 8) = Stock - Pendiente
                    Stock = 0 : Pendiente = 0
                End If
                RowsBusqueda = Dt.Select("Clave = " & Row("Articulo"))
                If RowsBusqueda.Length <> 0 Then
                    RowExxel = RowExxel + 2
                    exHoja.Cells.Item(RowExxel, 1) = RowsBusqueda(0).Item("Nombre")
                    exHoja.Cells.Item(RowExxel, 2) = NombreEspecie(RowsBusqueda(0).Item("Especie"))
                    exHoja.Cells.Item(RowExxel, 3) = NombreVariedad(RowsBusqueda(0).Item("Variedad"))
                    exHoja.Cells.Item(RowExxel, 4) = RowsBusqueda(0).Item("Kilos")
                    RowExxel = RowExxel - 1
                End If
                ArticuloAnt = Row("Articulo")
            End If
            If RowsBusqueda.Length <> 0 Then
                If Row("Tipo") = 1 Then
                    Stock = Stock + Row("Cantidad")
                Else
                    RowExxel = RowExxel + 1
                    If Row("Operacion") = 2 Then exHoja.Cells.Item(RowExxel, 5) = "X"
                    If Row("Tipo") = 2 Then exHoja.Cells.Item(RowExxel, 6) = "Remito"
                    If Row("Tipo") = 3 Then exHoja.Cells.Item(RowExxel, 6) = "Factura"
                    If Row("Tipo") = 4 Then exHoja.Cells.Item(RowExxel, 6) = "Pedido"
                    exHoja.Cells.Item(RowExxel, 7) = NumeroEditado(Row("Comprobante"))
                    exHoja.Cells.Item(RowExxel, 8) = Row("Cantidad")
                    Pendiente = Pendiente + Row("cantidad")
                End If
            End If
        Next
        If Stock <> 0 Or Pendiente <> 0 Then
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 1) = "Total Pendientes"
            exHoja.Cells.Item(RowExxel, 8) = Pendiente
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 1) = "Total Stock"
            exHoja.Cells.Item(RowExxel, 8) = Stock
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 1) = "Total Stock Real"
            exHoja.Cells.Item(RowExxel, 8) = Trunca(Stock - Pendiente)
            Stock = 0 : Pendiente = 0
        End If

        Dt.Dispose()
        DtStock.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 10) = Format(Date.Now, "MM/dd/yyyy")
        Dim Str2 As String = ""
        If ConPedidos Then Str2 = ".Incluye Artículos de Pedidos aun Abiertos."
        If Deposito = 0 Then
            exHoja.Cells.Item(2, 1) = "Articulos En Stock Real : Todos Los depositos." & NombreDeposito(Deposito) & Str2
        Else
            exHoja.Cells.Item(2, 1) = "Articulos En Stock Real: " & NombreDeposito(Deposito) & Str2
        End If

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub LotesIngresadosPorProveedor(ByVal Proveedor As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Reventa As Boolean, ByVal Consignacion As Boolean, ByVal Todos As Boolean, ByVal Especie As Integer, ByVal Variedad As Integer, ByVal Marca As Integer, ByVal Categoria As Integer, ByVal Envase As Integer, ByVal Deposito As Integer, ByVal Costeo As Integer, ByVal Duenio As Integer, ByVal Sucursal As Integer, ByVal TextRemitos As TextBox)

        Dim Sql As String = ""

        If Especie <> 0 Then
            If Sql = "" Then
                Sql = "Especie = " & Especie
            Else : Sql = "AND Especie = " & Especie
            End If
        End If
        If Variedad <> 0 Then
            If Sql = "" Then
                Sql = "Variedad = " & Variedad
            Else : Sql = Sql & " AND Variedad = " & Variedad
            End If
        End If
        If Marca <> 0 Then
            If Sql = "" Then
                Sql = "Marca = " & Marca
            Else : Sql = Sql & " AND Marca = " & Marca
            End If
        End If
        If Categoria <> 0 Then
            If Sql = "" Then
                Sql = "Categoria = " & Categoria
            Else : Sql = Sql & " AND Categoria = " & Categoria
            End If
        End If
        If Envase <> 0 Then
            If Sql = "" Then
                Sql = "Envase = " & Envase
            Else : Sql = Sql & " AND Envase = " & Envase
            End If
        End If

        If Sql <> "" Then Sql = "WHERE " & Sql

        Dim Dt As New DataTable
        If Sql <> "" Then
            If Not Tablas.Read("SELECT Clave,Especie,Nombre FROM Articulos " & Sql & ";", Conexion, Dt) Then Exit Sub
            If Dt.Rows.Count = 0 Then
                MsgBox("No Existe Articulos Seleccionados.", MsgBoxStyle.Information)
                Dt.Dispose()
                Exit Sub
            End If
        End If

        Dim DtDuenio As New DataTable
        ArmaDueño(DtDuenio, True)

        Dim SqlFecha As String = ""
        SqlFecha = " AND L.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND L.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "

        Dim SqlProveedor As String = ""
        If Proveedor <> 0 Then SqlProveedor = " AND L.Proveedor = " & Proveedor

        Dim SqlTipoOperacion As String = ""
        If Reventa Then SqlTipoOperacion = " AND C.TipoOperacion = 2 "
        If Consignacion Then SqlTipoOperacion = " AND C.TipoOperacion = 1 "

        Dim SqlDepo As String = ""
        If Deposito <> 0 Then SqlDepo = " AND L.Deposito = " & Deposito

        Dim SqlCosteo As String = ""
        If Costeo <> 0 Then SqlCosteo = " AND C.Costeo = " & Costeo

        Dim SqlSucursal As String = ""
        If Sucursal <> 0 Then SqlSucursal = " AND C.Sucursal = " & Sucursal

        Dim DtIngresos As New DataTable

        Dim SqlB As String = "SELECT 1 AS Operacion,L.Lote,L.Secuencia,L.Cantidad,L.Baja,L.Fecha,L.Articulo,L.TipoOperacion,L.Proveedor,L.Deposito,L.Senia,C.Remito,C.Guia,C.Sucursal FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen " & SqlProveedor & SqlFecha & SqlTipoOperacion & SqlDepo & SqlCosteo & SqlSucursal & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion,L.Lote,L.Secuencia,L.Cantidad,L.Baja,L.Fecha,L.Articulo,L.TipoOperacion,L.Proveedor,L.Deposito,L.Senia,C.Remito,C.Guia,C.Sucursal FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen " & SqlProveedor & SqlFecha & SqlTipoOperacion & SqlDepo & SqlCosteo & SqlSucursal & ";"

        If Not Tablas.Read(SqlB, Conexion, DtIngresos) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtIngresos) Then Exit Sub
        End If

        If DtIngresos.Rows.Count = 0 Then
            MsgBox("No Tiene Lotes Ingresados.", MsgBoxStyle.Information)
            Dt.Dispose()
            Exit Sub
        End If

        If TextRemitos.Lines.Length <> 0 Then
            For Each Row2 As DataRow In DtIngresos.Rows
                Dim Comprobante As Decimal
                If Row2("Remito") <> 0 Then Comprobante = Row2("Remito")
                If Row2("Guia") <> 0 Then Comprobante = Row2("Guia")
                If Not EstaRemitoEnTextBox(Comprobante, TextRemitos) Then
                    Row2.Delete()
                End If
            Next
        End If

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = "Proveedor"
        exHoja.Cells.Item(RowExxel, 2) = "Alias"
        exHoja.Cells.Item(RowExxel, 3) = " "
        exHoja.Cells.Item(RowExxel, 4) = "Lote"
        exHoja.Cells.Item(RowExxel, 5) = "Articulo       "
        exHoja.Cells.Item(RowExxel, 6) = "Fecha Ing."
        exHoja.Cells.Item(RowExxel, 7) = "Remito"
        exHoja.Cells.Item(RowExxel, 8) = "Guia"
        exHoja.Cells.Item(RowExxel, 9) = "Ingresado"
        exHoja.Cells.Item(RowExxel, 10) = "Devuelto"
        exHoja.Cells.Item(RowExxel, 11) = "Cantidad"
        exHoja.Cells.Item(RowExxel, 12) = "Senia/Uni."
        exHoja.Cells.Item(RowExxel, 13) = "T.Operación"
        exHoja.Cells.Item(RowExxel, 14) = "Deposito"
        exHoja.Cells.Item(RowExxel, 15) = "Dueño Envase"
        exHoja.Cells.Item(RowExxel, 16) = "Sucursal"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim View As New DataView
        View = DtIngresos.DefaultView
        View.Sort = "Proveedor,Lote,Secuencia"

        Dim ProveedorAnt As Integer = 0
        Dim Nombre As String = ""
        Dim AliasStr As String = ""
        Dim DuenioW As Integer = 0
        Dim NombreDuenio As String = ""

        Dim RowsBusqueda() As DataRow
        Dim Ok As Boolean

        For Each Row As DataRowView In View
            If Dt.Rows.Count <> 0 Then
                RowsBusqueda = Dt.Select("Clave = " & Row("Articulo"))
                If RowsBusqueda.Length <> 0 Then
                    Ok = True
                Else
                    Ok = False
                End If
            Else
                Ok = True
            End If
            HallaEnvaseYDuenio(Row("Articulo"), DtDuenio, DuenioW, NombreDuenio)
            If Duenio <> 0 And DuenioW <> Duenio Then Ok = False
            If Ok Then
                If ProveedorAnt <> Row("Proveedor") Then
                    NombreYAliasProveedor(Row("Proveedor"), Nombre, AliasStr)
                    exHoja.Cells.Item(RowExxel + 2, 1) = Nombre
                    exHoja.Cells.Item(RowExxel + 2, 2) = AliasStr
                    ProveedorAnt = Row("Proveedor")
                    RowExxel = RowExxel + 1
                End If
                RowExxel = RowExxel + 1
                If Row("Operacion") = 2 Then
                    exHoja.Cells.Item(RowExxel, 3) = "X"
                End If
                exHoja.Cells.Item(RowExxel, 4) = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
                exHoja.Cells.Item(RowExxel, 5) = NombreArticulo(Row("Articulo"))
                exHoja.Cells.Item(RowExxel, 6) = Format(Row("Fecha"), "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel, 7) = NumeroEditado(Row("Remito"))
                exHoja.Cells.Item(RowExxel, 8) = Row("Guia")
                exHoja.Cells.Item(RowExxel, 9) = Row("Cantidad")
                exHoja.Cells.Item(RowExxel, 10) = Row("Baja")
                exHoja.Cells.Item(RowExxel, 11) = CDec(Row("Cantidad")) - CDec(Row("Baja"))
                Dim Senia As Double = 0
                If Row("Senia") = -1 Then
                    Senia = HallaSeniaArticulo(Row("Articulo"), Row("Fecha"))
                Else
                    Senia = Row("Senia")
                End If
                exHoja.Cells.Item(RowExxel, 12) = Senia
                If Row("TipoOperacion") = 1 Then
                    exHoja.Cells.Item(RowExxel, 13) = "Consignación"
                End If
                If Row("TipoOperacion") = 2 Then
                    exHoja.Cells.Item(RowExxel, 13) = "Reventa"
                End If
                exHoja.Cells.Item(RowExxel, 14) = NombreDeposito(Row("Deposito"))
                exHoja.Cells.Item(RowExxel, 15) = NombreDuenio
                exHoja.Cells.Item(RowExxel, 16) = NombreSucursalProveedor(Row("Proveedor"), Row("Sucursal"))
            End If
        Next

        Dt.Dispose()
        DtIngresos.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        ' 
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        ' 
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 12 - 1) & InicioDatos
        BB = Chr(65 + 12 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 15) = Format(Date.Now, "MM/dd/yyyy")
        Dim Titulo As String = "Ingresos Del proveedor : " & NombreProveedor(Proveedor) & "  Dese el " & Format(Desde, "dd/MM/yyyy") & " Hasta el " & Format(Hasta, "dd/MM/yyyy")
        If Costeo <> 0 Then
            Titulo = Titulo & "     Costeo " & NombreCosteo(Costeo)
        End If
        exHoja.Cells.Item(2, 1) = Titulo

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeDescartes(ByVal Especie As Integer, ByVal Variedad As Integer, ByVal Marca As Integer, ByVal Categoria As Integer, ByVal Envase As Integer, ByVal Desde As Date, ByVal Hasta As Date)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Fecha"
        exHoja.Cells.Item(RowExxel, 3) = "Lote"
        exHoja.Cells.Item(RowExxel, 4) = "Articulo       "
        exHoja.Cells.Item(RowExxel, 5) = "Comprobante"
        exHoja.Cells.Item(RowExxel, 6) = "Cantidad"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Sql As String = ""

        If Especie <> 0 Then
            If Sql = "" Then
                Sql = "Especie = " & Especie
            Else : Sql = "AND Especie = " & Especie
            End If
        End If
        If Variedad <> 0 Then
            If Sql = "" Then
                Sql = "Variedad = " & Variedad
            Else : Sql = Sql & " AND Variedad = " & Variedad
            End If
        End If
        If Marca <> 0 Then
            If Sql = "" Then
                Sql = "Marca = " & Marca
            Else : Sql = Sql & " AND Marca = " & Marca
            End If
        End If
        If Categoria <> 0 Then
            If Sql = "" Then
                Sql = "Categoria = " & Categoria
            Else : Sql = Sql & " AND Categoria = " & Categoria
            End If
        End If
        If Envase <> 0 Then
            If Sql = "" Then
                Sql = "Envase = " & Envase
            Else : Sql = Sql & " AND Envase = " & Envase
            End If
        End If

        If Sql <> "" Then Sql = "WHERE " & Sql

        Dim Dt As New DataTable
        If Sql <> "" Then
            If Not Tablas.Read("SELECT Clave,Nombre FROM Articulos " & Sql & ";", Conexion, Dt) Then Exit Sub
        End If

        Dim SqlFecha As String = ""
        SqlFecha = " C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "

        Dim SqlB As String = "SELECT 1 AS Operacion,D.Lote,D.Secuencia,D.Cantidad,C.Clave,C.Fecha FROM DescarteCabeza AS C INNER JOIN DescarteDetalle AS D ON C.Clave = D.Clave WHERE C.Estado <> 3 AND " & SqlFecha & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion,D.Lote,D.Secuencia,D.Cantidad,C.Clave,C.Fecha FROM DescarteCabeza AS C INNER JOIN DescarteDetalle AS D ON C.Clave = D.Clave WHERE C.Estado <> 3 AND " & SqlFecha & ";"

        Dim DtDescarte As New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtDescarte) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtDescarte) Then Exit Sub
        End If

        Dim View As New DataView
        View = DtDescarte.DefaultView
        View.Sort = "Fecha"

        If View.Count = 0 Then
            MsgBox("No Hay Descartes.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim Row As DataRowView
        Dim ConexionLote As String
        Dim RowsBusqueda() As DataRow
        Dim Ok As Boolean
        Dim Articulo As Integer = 0
        Dim NombreArticuloW As String = ""

        For Each Row In View
            If Row("Operacion") = 1 Then
                ConexionLote = Conexion
            Else : ConexionLote = ConexionN
            End If
            Articulo = HallaArticulo(Row("Lote"), Row("secuencia"), ConexionLote)
            Ok = False
            If Dt.Rows.Count = 0 Then
                Ok = True
                NombreArticuloW = NombreArticulo(Articulo)
            Else
                RowsBusqueda = Dt.Select("Clave = " & Articulo)
                If RowsBusqueda.Length <> 0 Then
                    Ok = True
                    NombreArticuloW = RowsBusqueda(0).Item("Nombre")
                End If
            End If
            If Ok Then
                RowExxel = RowExxel + 1
                If Row("Operacion") = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
                exHoja.Cells.Item(RowExxel, 2) = Format(Row("Fecha"), "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel, 3) = Row("Lote") & "/" & Format(Row("secuencia"), "000")
                exHoja.Cells.Item(RowExxel, 4) = NombreArticuloW
                exHoja.Cells.Item(RowExxel, 5) = Row("Clave")
                exHoja.Cells.Item(RowExxel, 6) = Row("Cantidad")
            End If
        Next

        Dt.Dispose()
        DtDescarte.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 10) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Descarte Desde el : " & Format(Desde, "dd/MM/yyyy") & "  Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeVentas(ByVal Cliente As Integer, ByVal Especie As Integer, ByVal Variedad As Integer, ByVal Desde As Date, ByVal Hasta As Date, _
                             ByVal Estado As Integer, ByVal Blanco As Boolean, ByVal Negro As Boolean, ByVal Vendedor As Integer, ByVal CanalVenta As Integer, _
                             ByVal Deposito As Integer, ByVal MuestraLotes As Boolean, ByVal RepetirDatos As Boolean, ByVal Sucursal As Integer, ByVal SinContables As Boolean, _
                             ByVal SoloContables As Boolean, ByVal Todas As Boolean, ByVal Marca As Integer, ByVal Categoria As Integer, ByVal Envase As Integer)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = "Cliente"
        exHoja.Cells.Item(RowExxel, 2) = " "
        exHoja.Cells.Item(RowExxel, 3) = "Factura"
        exHoja.Cells.Item(RowExxel, 4) = "Fecha Fact."
        exHoja.Cells.Item(RowExxel, 5) = "Remito"
        exHoja.Cells.Item(RowExxel, 6) = "Canal Venta"
        exHoja.Cells.Item(RowExxel, 7) = "Vendedor"
        exHoja.Cells.Item(RowExxel, 8) = "Deposito"
        exHoja.Cells.Item(RowExxel, 9) = "Estado"
        exHoja.Cells.Item(RowExxel, 10) = "Articulo"
        exHoja.Cells.Item(RowExxel, 11) = "Cantidad"
        exHoja.Cells.Item(RowExxel, 12) = "Devolucion"
        exHoja.Cells.Item(RowExxel, 13) = "Precio"
        exHoja.Cells.Item(RowExxel, 14) = "Importe"
        exHoja.Cells.Item(RowExxel, 15) = "Total U.Medida"
        exHoja.Cells.Item(RowExxel, 16) = "   "
        exHoja.Cells.Item(RowExxel, 17) = "Lotes"
        exHoja.Cells.Item(RowExxel, 18) = "Moneda"
        exHoja.Cells.Item(RowExxel, 19) = "Sucursal"
        exHoja.Cells.Item(RowExxel, 20) = "Importe"
        exHoja.Cells.Item(RowExxel, 21) = "Importe C/Dev."
        exHoja.Cells.Item(RowExxel, 22) = "Saldo"
        exHoja.Cells.Item(RowExxel, 23) = ""
        exHoja.Cells.Item(RowExxel, 24) = "Pedido Cliente"
        exHoja.Cells.Item(RowExxel, 24) = "Pedido Cliente"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Sql As String = ""

        If Especie <> 0 Then
            If Sql = "" Then
                Sql = "Especie = " & Especie
            Else : Sql = "AND Especie = " & Especie
            End If
        End If
        If Variedad <> 0 Then
            If Sql = "" Then
                Sql = "Variedad = " & Variedad
            Else : Sql = Sql & " AND Variedad = " & Variedad
            End If
        End If
        If Marca <> 0 Then
            If Sql = "" Then
                Sql = "Marca = " & Marca
            Else : Sql = Sql & " AND Marca = " & Marca
            End If
        End If
        If Categoria <> 0 Then
            If Sql = "" Then
                Sql = "Categoria = " & Categoria
            Else : Sql = Sql & " AND Categoria = " & Categoria
            End If
        End If
        If Envase <> 0 Then
            If Sql = "" Then
                Sql = "Envase = " & Envase
            Else : Sql = Sql & " AND Envase = " & Envase
            End If
        End If

        If Sql <> "" Then Sql = "WHERE " & Sql

        Dim Dt As New DataTable
        If Sql <> "" Then
            If Not Tablas.Read("SELECT Clave,Nombre FROM Articulos " & Sql & ";", Conexion, Dt) Then Exit Sub
            If Dt.Rows.Count = 0 Then
                MsgBox("No Existe Articulos.", MsgBoxStyle.Information)
                Dt.Dispose()
                Exit Sub
            End If
        End If

        Dim DtMonedas As New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27", Conexion, DtMonedas) Then Exit Sub
        Dim RowM As DataRow = DtMonedas.NewRow
        RowM("Clave") = 1
        RowM("Nombre") = "Pesos"
        DtMonedas.Rows.Add(RowM)

        Dim SqlFecha As String = ""
        SqlFecha = "C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlFechaContable As String = ""
        SqlFechaContable = "C.FechaContable >='" & Format(Desde, "yyyyMMdd") & "' AND C.FechaContable < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlEstado As String = ""
        If Estado <> 0 Then SqlEstado = " AND C.Estado = " & Estado
        If Estado = 4 Then SqlEstado = " AND C.Estado <> 3"
        Dim SqlCliente As String = ""
        If Cliente <> 0 Then SqlCliente = " AND C.Cliente = " & Cliente
        Dim SqlVendedor As String = ""
        If Vendedor <> 0 Then SqlVendedor = " AND C.Vendedor = " & Vendedor
        Dim SqlDeposito As String = ""
        If Deposito <> 0 Then SqlDeposito = " AND C.Deposito = " & Deposito
        Dim SqlDepositoNVLP As String = ""
        If Deposito <> 0 Then SqlDepositoNVLP = " AND D.Deposito = " & Deposito

        Dim SqlB As String = "SELECT 1 AS Operacion,1 As Tipo,C.Importe + C.Percepciones AS Importe,C.Moneda,C.Factura,C.Fecha,C.Remito,C.Cliente,C.Estado,C.Vendedor,C.Rel,C.Relacionada,C.Deposito,D.Articulo,D.Cantidad,D.Devueltas,D.Precio,D.TotalArticulo,D.Indice,C.Sucursal,C.Saldo,C.Tr,C.ImporteDev,C.PedidoCliente FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE Tr = 0 AND D.Articulo <> 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & SqlEstado & SqlCliente & SqlVendedor & SqlDeposito & _
                             " UNION ALL " & _
                             "SELECT 1 AS Operacion,1 As Tipo,C.Importe + C.Percepciones AS Importe,C.Moneda,C.Factura,C.Fecha,C.Remito,C.Cliente,C.Estado,C.Vendedor,C.Rel,C.Relacionada,C.Deposito,D.Articulo,D.Cantidad,D.Devueltas,D.Precio,D.TotalArticulo,D.Indice,C.Sucursal,C.Saldo,C.Tr,C.ImporteDev,C.PedidoCliente FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE Tr = 1 AND D.Articulo <> 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & SqlEstado & SqlCliente & SqlVendedor & SqlDeposito & _
                             " UNION ALL " & _
                             "SELECT 1 AS Operacion,2 AS Tipo,C.Importe,1 AS Moneda,C.Liquidacion AS Factura,C.FechaContable AS Fecha,D.Remito,C.Cliente,C.Estado,0 AS Vendedor,C.Rel,C.Nrel AS Relacionada,D.Deposito,R.Articulo,D.Cantidad,D.Merma AS Devueltas,D.Precio,D.NetoConIva AS TotalArticulo,D.Indice,0 AS Sucursal,C.Saldo,C.Tr,0 AS ImporteDev,'' AS PedidoCliente FROM NVLPCabeza AS C INNER JOIN (NVLPLotes AS D INNER JOIN RemitosDetalle AS R ON D.Indice = R.Indice AND D.Remito = R.Remito) ON C.Liquidacion = D.Liquidacion WHERE C.Tr = 0 AND " & SqlFechaContable & SqlEstado & SqlCliente & SqlDepositoNVLP & _
                             " UNION ALL " & _
                             "SELECT 1 AS Operacion,2 AS Tipo,C.Importe,1 AS Moneda,C.Liquidacion AS Factura,C.FechaContable As Fecha,0 AS Remito,C.Cliente,C.Estado,0 AS Vendedor,C.Rel,C.Nrel AS Relacionada,0 AS Deposito,D.Articulo,D.Cantidad,0 AS Devueltas,D.Precio,D.Importe AS TotalArticulo,D.Indice,0 AS Sucursal,C.Saldo,C.Tr,0 AS ImporteDev,'' AS PedidoCliente FROM NVLPCabeza AS C INNER JOIN NVLPArticulos AS D ON C.Liquidacion = D.Liquidacion WHERE C.Tr = 1 AND " & SqlFechaContable & SqlEstado & SqlCliente & ";"


        Dim SqlN As String = "SELECT 2 AS Operacion,1 AS Tipo,C.Importe,C.Moneda,C.Factura,C.Fecha,C.Remito,C.Cliente,C.Estado,C.Vendedor,C.Rel,C.Relacionada,C.Deposito,D.Articulo,D.Cantidad,D.Devueltas,D.Precio,D.TotalArticulo,D.Indice,C.Sucursal,C.Saldo,C.Tr,C.ImporteDev,C.PedidoCliente FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE Tr = 0 AND D.Articulo <> 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & SqlEstado & SqlCliente & SqlVendedor & SqlDeposito & _
                             " UNION ALL " & _
                             "SELECT 2 AS Operacion,1 AS Tipo,C.Importe,C.Moneda,C.Factura,C.Fecha,C.Remito,C.Cliente,C.Estado,C.Vendedor,C.Rel,C.Relacionada,C.Deposito,D.Articulo,D.Cantidad,D.Devueltas,D.Precio,D.TotalArticulo,D.Indice,C.Sucursal,C.Saldo,C.Tr,C.ImporteDev,C.PedidoCliente FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE Tr = 1 AND D.Articulo <> 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & SqlEstado & SqlCliente & SqlVendedor & SqlDeposito & _
                             " UNION ALL " & _
                             "SELECT 2 AS Operacion,2 AS Tipo,C.Importe,1 AS Moneda,C.Liquidacion AS Factura,C.FechaContable As Fecha,D.Remito,C.Cliente,C.Estado,0 AS Vendedor,C.Rel,C.Nrel AS Relacionada,D.Deposito,R.Articulo,D.Cantidad,D.Merma AS Devueltas,D.Precio,D.NetoConIva AS TotalArticulo,D.Indice,0 AS Sucursal,C.Saldo,C.Tr,0 AS ImporteDev,'' AS PedidoCliente FROM NVLPCabeza AS C INNER JOIN (NVLPLotes AS D INNER JOIN RemitosDetalle AS R ON D.Indice = R.Indice AND D.Remito = R.Remito) ON C.Liquidacion = D.Liquidacion WHERE C.Tr = 0 AND C.Rel = 0 AND " & SqlFechaContable & SqlEstado & SqlCliente & SqlDepositoNVLP & _
                             " UNION ALL " & _
                             "SELECT 2 AS Operacion,2 AS Tipo,C.Importe,1 AS Moneda,C.Liquidacion AS Factura,C.FechaContable As Fecha,D.Remito,C.Cliente,C.Estado,0 AS Vendedor,C.Rel,C.Nrel AS Relacionada,D.Deposito,0 AS Articulo,D.Cantidad,D.Merma AS Devueltas,D.Precio,D.NetoConIva AS TotalArticulo,D.Indice,0 AS Sucursal,C.Saldo,C.Tr,0 AS ImporteDev,'' AS PedidoCliente FROM NVLPCabeza AS C INNER JOIN NVLPLotes AS D ON C.Liquidacion = D.Liquidacion WHERE C.Tr = 0 AND C.Rel = 1 AND " & SqlFechaContable & SqlEstado & SqlCliente & SqlDepositoNVLP & ";"    'caso en que tenga NVLP mixta y no se puede aparear NVLPLotes con RemitosDetalle.


        Dim DtFacturas As New DataTable
        If Blanco Then
            If Not Tablas.Read(SqlB, Conexion, DtFacturas) Then Exit Sub
        End If
        If Negro Then
            If Not Tablas.Read(SqlN, ConexionN, DtFacturas) Then Exit Sub
        End If

        If DtFacturas.Rows.Count = 0 Then
            MsgBox("No Hay Datos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim View As New DataView
        View = DtFacturas.DefaultView
        View.Sort = "Fecha,Factura,Tipo"

        Dim Row As DataRowView
        Dim FacturaAnt As Double = 0
        Dim TipoAnt As Integer = 0
        Dim NombreArticuloW As String = ""
        Dim RowsBusqueda() As DataRow
        Dim Ok As Boolean
        Dim ConexionStr As String
        Dim DtLotes As New DataTable
        Dim Tipo As String
        Dim TotalArticuloAux As Decimal
        Dim KilosXUnidad As Decimal
        Dim UMedida As String
        Dim TotalImporte As Decimal
        Dim TotalImporteConDev As Decimal

        For Each Row In View
            Ok = False
            If Dt.Rows.Count = 0 Then
                NombreArticuloW = NombreArticulo(Row("Articulo"))
                If NombreArticuloW <> "" Then Ok = True
            Else
                RowsBusqueda = Dt.Select("Clave = " & Row("Articulo"))
                If RowsBusqueda.Length <> 0 Then
                    Ok = True
                    NombreArticuloW = RowsBusqueda(0).Item("Nombre")
                End If
            End If
            HallaKilosXUnidadYUMedida(Row("Articulo"), KilosXUnidad, UMedida)
            If Row("Operacion") = 2 And Row("Rel") And Blanco And Negro Then Ok = False
            If Sucursal <> 0 And Row("Sucursal") <> Sucursal Then Ok = False
            If SinContables And Row("Tr") Then Ok = False
            If SoloContables And Not Row("Tr") Then Ok = False
            If Ok Then
                If TipoAnt & FacturaAnt <> Row("Tipo") & Row("Factura") Then
                    HallaDatosCliente(Row("Cliente"))
                    exHoja.Cells.Item(RowExxel + 1, 1) = NombreClienteW
                    If Row("operacion") = 2 Then exHoja.Cells.Item(RowExxel + 1, 2) = "X"
                    If Row("Tipo") = 1 Then
                        Tipo = "Fact. "
                    Else : Tipo = "NVLP. "
                    End If
                    exHoja.Cells.Item(RowExxel + 1, 3) = Tipo & NumeroEditado(Row("Factura"))
                    exHoja.Cells.Item(RowExxel + 1, 4) = Format(Row("Fecha"), "MM/dd/yyyy")
                    If Row("Remito") > 1 Then exHoja.Cells.Item(RowExxel + 1, 5) = NumeroEditado(Row("Remito")) 'Sistema anterior a facturar varios remitos.
                    If Row("Remito") = 1 Then  'Facturas con varios remitos tiene un 1 en remito.
                        Dim Str As String = HallaRemitosFactura(Row("Factura"), Row("Operacion"), Row("Relacionada"))
                        exHoja.Cells.Item(RowExxel + 1, 5) = Str
                    End If
                    If CanalVentaW <> 0 Then exHoja.Cells.Item(RowExxel + 1, 6) = HallaCanalDeVenta(CanalVentaW)
                    If Row("Vendedor") <> 0 Then exHoja.Cells.Item(RowExxel + 1, 7) = HallaVendedor(Row("Vendedor"))
                    exHoja.Cells.Item(RowExxel + 1, 8) = NombreDeposito(Row("Deposito"))
                    If Row("Tipo") = 1 Then
                        If Row("Estado") = 1 Then exHoja.Cells.Item(RowExxel + 1, 9) = "Afecta Stock"
                        If Row("Estado") = 2 Then exHoja.Cells.Item(RowExxel + 1, 9) = "Pendiente"
                        If Row("Estado") = 3 Then exHoja.Cells.Item(RowExxel + 1, 9) = "Baja"
                    End If
                    If Row("Tipo") = 2 Then
                        If Row("Estado") = 1 Then exHoja.Cells.Item(RowExxel + 1, 9) = "Activo"
                        If Row("Estado") = 3 Then exHoja.Cells.Item(RowExxel + 1, 9) = "Baja"
                    End If
                    RowsBusqueda = DtMonedas.Select("Clave = " & Row("Moneda"))
                    If RowsBusqueda.Length = 0 Then MsgBox(Row("Moneda") & " " & Row("Factura"))
                    exHoja.Cells.Item(RowExxel + 1, 18) = RowsBusqueda(0).Item("Nombre")
                    If Row("Sucursal") <> 0 Then exHoja.Cells.Item(RowExxel + 1, 19) = NombreSucursalCliente(Row("Cliente"), Row("Sucursal"))
                    exHoja.Cells.Item(RowExxel + 1, 20) = Row("Importe")
                    If Row("Estado") <> 3 Then TotalImporte = TotalImporte + Row("Importe")
                    exHoja.Cells.Item(RowExxel + 1, 21) = Row("Importe") - Row("ImporteDev")
                    If Row("Estado") <> 3 Then TotalImporteConDev = TotalImporteConDev + Row("Importe") - Row("ImporteDev")
                    exHoja.Cells.Item(RowExxel + 1, 22) = Row("Saldo")
                    If Row("Tr") Then exHoja.Cells.Item(RowExxel + 1, 23) = "Contable"
                    exHoja.Cells.Item(RowExxel + 1, 24) = Row("PedidoCliente")
                    If Row("Operacion") = 1 And Row("Rel") And Blanco And Negro Then
                        RowsBusqueda = DtFacturas.Select("Operacion = 2 AND Relacionada = " & Row("Factura") & " AND Tipo = " & Row("Tipo"))
                        If RowsBusqueda.Length <> 0 Then
                            Dim Importe As Decimal = Row("Importe") + RowsBusqueda(0).Item("Importe")
                            exHoja.Cells.Item(RowExxel + 1, 20) = Importe
                            If Row("Estado") <> 3 Then TotalImporte = TotalImporte + RowsBusqueda(0).Item("Importe")
                            Dim ImporteConDev As Decimal = Row("Importe") - Row("ImporteDev") + RowsBusqueda(0).Item("Importe") - RowsBusqueda(0).Item("ImporteDev")  'importe con devolucion. 
                            exHoja.Cells.Item(RowExxel + 1, 21) = ImporteConDev
                            If Row("Estado") <> 3 Then TotalImporteConDev = TotalImporteConDev + +RowsBusqueda(0).Item("Importe") - RowsBusqueda(0).Item("ImporteDev")
                            Dim Saldo As Decimal = Row("Saldo") + RowsBusqueda(0).Item("Saldo")
                            exHoja.Cells.Item(RowExxel + 1, 22) = Saldo
                        End If
                    End If
                    FacturaAnt = Row("Factura")
                    TipoAnt = Row("Tipo")
                End If
                RowExxel = RowExxel + 1
                If RepetirDatos And exHoja.Cells.Item(RowExxel, 1).value = "" Then
                    exHoja.Cells.Item(RowExxel, 1) = exHoja.Cells.Item(RowExxel - 1, 1)
                    exHoja.Cells.Item(RowExxel, 2) = exHoja.Cells.Item(RowExxel - 1, 2)
                    exHoja.Cells.Item(RowExxel, 3) = exHoja.Cells.Item(RowExxel - 1, 3)
                    exHoja.Cells.Item(RowExxel, 4) = Format(Row("Fecha"), "MM/dd/yyyy")
                    exHoja.Cells.Item(RowExxel, 5) = exHoja.Cells.Item(RowExxel - 1, 5)
                    exHoja.Cells.Item(RowExxel, 6) = exHoja.Cells.Item(RowExxel - 1, 6)
                    exHoja.Cells.Item(RowExxel, 7) = exHoja.Cells.Item(RowExxel - 1, 7)
                    exHoja.Cells.Item(RowExxel, 8) = exHoja.Cells.Item(RowExxel - 1, 8)
                    exHoja.Cells.Item(RowExxel, 9) = exHoja.Cells.Item(RowExxel - 1, 9)
                    exHoja.Cells.Item(RowExxel, 19) = exHoja.Cells.Item(RowExxel - 1, 19)
                End If
                exHoja.Cells.Item(RowExxel, 10) = NombreArticuloW
                If Row("Tipo") = 2 And Row("Remito") <> 0 Then exHoja.Cells.Item(RowExxel, 5) = NumeroEditado(Row("Remito"))
                exHoja.Cells.Item(RowExxel, 11) = Row("Cantidad")
                exHoja.Cells.Item(RowExxel, 12) = Row("Devueltas")
                exHoja.Cells.Item(RowExxel, 13) = Row("Precio")
                exHoja.Cells.Item(RowExxel, 14) = Row("TotalArticulo")
                exHoja.Cells.Item(RowExxel, 15) = Trunca(KilosXUnidad * (Row("Cantidad") - Row("Devueltas")))
                exHoja.Cells.Item(RowExxel, 16) = UMedida
                If Row("Operacion") = 1 And Row("Rel") And Blanco And Negro Then
                    TotalArticuloAux = 0
                    Select Case Row("Tipo")
                        Case 1
                            RowsBusqueda = DtFacturas.Select("Operacion = 2 AND Relacionada = " & Row("Factura") & " AND Tipo = " & Row("Tipo") & " AND Indice = " & Row("Indice"))
                        Case 2
                            RowsBusqueda = DtFacturas.Select("Operacion = 2 AND Relacionada = " & Row("Factura") & " AND Tipo = " & Row("Tipo") & " AND Indice = " & Row("Indice") & " AND Remito = " & Row("Remito"))
                    End Select
                    If RowsBusqueda.Length <> 0 Then
                        TotalArticuloAux = RowsBusqueda(0).Item("TotalArticulo")
                    End If
                    exHoja.Cells.Item(RowExxel, 13) = (Row("TotalArticulo") + TotalArticuloAux) / Row("Cantidad")
                    exHoja.Cells.Item(RowExxel, 14) = Row("TotalArticulo") + TotalArticuloAux
                End If
                If MuestraLotes Then
                    If Row("operacion") = 1 Then
                        ConexionStr = Conexion
                    Else : ConexionStr = ConexionN
                    End If
                    DtLotes.Clear()
                    If Not Tablas.Read("SELECT Lote,Secuencia FROM AsignacionLotes WHERE Cantidad <> 0 AND TipoComprobante = 2 AND Comprobante = " & Row("Factura") & " AND Indice = " & Row("Indice") & ";", ConexionStr, DtLotes) Then Exit Sub
                    If DtLotes.Rows.Count <> 0 Then RowExxel = RowExxel - 1
                    For Each Row1 As DataRow In DtLotes.Rows
                        RowExxel = RowExxel + 1
                        exHoja.Cells.Item(RowExxel, 17) = Row1("lote") & "/" & Format(Row1("Secuencia"), "000")
                    Next
                End If
            End If
        Next
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 20) = TotalImporte
        exHoja.Cells.Item(RowExxel, 21) = TotalImporteConDev

        Dt.Dispose()
        DtFacturas.Dispose()
        View = Nothing
        DtLotes.Dispose()

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 12 - 1) & InicioDatos
        BB = Chr(65 + 12 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 13 - 1) & InicioDatos
        BB = Chr(65 + 13 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 14 - 1) & InicioDatos
        BB = Chr(65 + 14 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 15 - 1) & InicioDatos
        BB = Chr(65 + 15 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 20 - 1) & InicioDatos
        BB = Chr(65 + 20 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 21 - 1) & InicioDatos
        BB = Chr(65 + 21 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 22 - 1) & InicioDatos
        BB = Chr(65 + 22 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.ColumnWidth = 18     'limita el ancho de la columna si son varios remito.

        'Muestra titulos.
        exHoja.Cells.Item(1, 18) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Informe Ventas Desde el : " & Format(Desde, "dd/MM/yyyy") & "  Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeVentasPorArticulos(ByVal Cliente As Integer, ByVal Especie As Integer, ByVal Variedad As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Sucursal As Integer, ByVal PorClienteSucursal As Boolean)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = "Cliente"
        exHoja.Cells.Item(RowExxel, 2) = "Sucursal"
        exHoja.Cells.Item(RowExxel, 3) = "Articulo"
        exHoja.Cells.Item(RowExxel, 4) = "Facturado"
        exHoja.Cells.Item(RowExxel, 5) = "Devueltas"
        exHoja.Cells.Item(RowExxel, 6) = "Total"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Sql As String = ""

        If Especie <> 0 Then
            If Sql = "" Then
                Sql = "Especie = " & Especie
            Else : Sql = "AND Especie = " & Especie
            End If
        End If
        If Variedad <> 0 Then
            If Sql = "" Then
                Sql = "Variedad = " & Variedad
            Else : Sql = Sql & " AND Variedad = " & Variedad
            End If
        End If

        If Sql <> "" Then Sql = "WHERE " & Sql

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Articulos " & Sql & ";", Conexion, Dt) Then Exit Sub
        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Articulos.", MsgBoxStyle.Information)
            Dt.Dispose()
            Exit Sub
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlFechaContable As String = ""
        SqlFechaContable = "C.FechaContable >='" & Format(Desde, "yyyyMMdd") & "' AND C.FechaContable < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlCliente As String = ""
        If Cliente <> 0 Then SqlCliente = " AND C.Cliente = " & Cliente
        Dim SqlSucursal As String = ""
        If Sucursal <> 0 Then SqlSucursal = " AND C.Sucursal = " & Sucursal

        Dim SqlB As String
        Dim SqlN As String

        If Sucursal = 0 Then
            SqlB = "SELECT 1 AS Operacion,1 As Tipo,C.Importe,C.Moneda,C.Factura,C.Fecha,C.Remito,C.Cliente,C.Estado,C.Vendedor,C.Rel,C.Relacionada,C.Deposito,D.Articulo,D.Cantidad,D.Devueltas,D.Precio,D.TotalArticulo,D.Indice,C.Sucursal,C.Saldo,' ' AS NombreArticulo FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE Tr = 0 AND C.Estado <> 3 AND D.Articulo <> 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & SqlCliente & SqlSucursal & _
                                 " UNION ALL " & _
                                 "SELECT 1 AS Operacion,2 AS Tipo,C.Importe,1 AS Moneda,C.Liquidacion AS Factura,C.FechaContable AS Fecha,D.Remito,C.Cliente,C.Estado,0 AS Vendedor,C.Rel,C.Nrel AS Relacionada,D.Deposito,R.Articulo,D.Cantidad,D.Merma AS Devueltas,D.Precio,D.NetoConIva AS TotalArticulo,D.Indice,0 AS Sucursal,C.Saldo,' ' AS NombreArticulo FROM NVLPCabeza AS C INNER JOIN (NVLPLotes AS D INNER JOIN RemitosDetalle AS R ON D.Indice = R.Indice AND D.Remito = R.Remito) ON C.Liquidacion = D.Liquidacion WHERE C.Estado <> 3 AND " & SqlFechaContable & SqlCliente & ";"
            SqlN = "SELECT 2 AS Operacion,1 AS Tipo,C.Importe,C.Moneda,C.Factura,C.Fecha,C.Remito,C.Cliente,C.Estado,C.Vendedor,C.Rel,C.Relacionada,C.Deposito,D.Articulo,D.Cantidad,D.Devueltas,D.Precio,D.TotalArticulo,D.Indice,C.Sucursal,C.Saldo,' ' AS NombreArticulo FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE Tr = 0 AND C.Rel = 0 AND C.Estado <> 3 AND D.Articulo <> 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & SqlCliente & SqlSucursal & _
                                 " UNION ALL " & _
                                 "SELECT 2 AS Operacion,2 AS Tipo,C.Importe,1 AS Moneda,C.Liquidacion AS Factura,C.FechaContable As Fecha,D.Remito,C.Cliente,C.Estado,0 AS Vendedor,C.Rel,C.Nrel AS Relacionada,D.Deposito,R.Articulo,D.Cantidad,D.Merma AS Devueltas,D.Precio,D.NetoConIva AS TotalArticulo,D.Indice,0 AS Sucursal,C.Saldo,' ' AS NombreArticulo FROM NVLPCabeza AS C INNER JOIN (NVLPLotes AS D INNER JOIN RemitosDetalle AS R ON D.Indice = R.Indice AND D.Remito = R.Remito) ON C.Liquidacion = D.Liquidacion WHERE C.Rel = 0 AND C.Estado <> 3 AND " & SqlFechaContable & SqlCliente & ";"
        Else
            SqlB = "SELECT 1 AS Operacion,1 As Tipo,C.Importe,C.Moneda,C.Factura,C.Fecha,C.Remito,C.Cliente,C.Estado,C.Vendedor,C.Rel,C.Relacionada,C.Deposito,D.Articulo,D.Cantidad,D.Devueltas,D.Precio,D.TotalArticulo,D.Indice,C.Sucursal,C.Saldo,' ' AS NombreArticulo FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE Tr = 0 AND C.Estado <> 3 AND D.Articulo <> 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & SqlCliente & SqlSucursal & ";"
            SqlN = "SELECT 2 AS Operacion,1 AS Tipo,C.Importe,C.Moneda,C.Factura,C.Fecha,C.Remito,C.Cliente,C.Estado,C.Vendedor,C.Rel,C.Relacionada,C.Deposito,D.Articulo,D.Cantidad,D.Devueltas,D.Precio,D.TotalArticulo,D.Indice,C.Sucursal,C.Saldo,' ' AS NombreArticulo FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE Tr = 0 AND C.Rel = 0 AND C.Estado <> 3 AND D.Articulo <> 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & SqlCliente & SqlSucursal & ";"
        End If

        Dim DtFacturas As New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtFacturas) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtFacturas) Then Exit Sub
        End If

        If DtFacturas.Rows.Count = 0 Then
            MsgBox("No Hay Datos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim RowsBusqueda() As DataRow

        For Each Row1 As DataRow In DtFacturas.Rows
            RowsBusqueda = Dt.Select("Clave = " & Row1("Articulo"))
            If RowsBusqueda.Length <> 0 Then
                Row1("NombreArticulo") = RowsBusqueda(0).Item("Nombre")
            Else
                Row1.Delete()
            End If
            If Row1.RowState <> DataRowState.Deleted Then
                If Sucursal <> 0 And Row1("Sucursal") <> Sucursal Then Row1.Delete()
            End If
        Next

        If DtFacturas.Rows.Count = 0 Then
            MsgBox("No Hay Datos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim View As New DataView
        View = DtFacturas.DefaultView
        If PorClienteSucursal Then
            View.Sort = "Cliente,Sucursal,NombreArticulo"
        Else
            View.Sort = "Cliente,NombreArticulo"
        End If

        Dim Row As DataRowView
        Dim TotalCantidad As Decimal
        Dim TotalDevueltas As Decimal
        Dim ClienteAnt As Integer = 0
        Dim SucursalAnt As Integer = 0
        Dim NombreArticuloAnt As String = 0
        Dim ArticuloAnt As Integer = 0

        For Each Row In View
            If ClienteAnt <> Row("Cliente") Then
                RowExxel = RowExxel + 1
                exHoja.Cells.Item(RowExxel, 1) = NombreCliente(Row("Cliente"))
                If Row("Sucursal") <> 0 And PorClienteSucursal Then exHoja.Cells.Item(RowExxel, 2) = NombreSucursalCliente(Row("Cliente"), Row("Sucursal"))
                exHoja.Cells.Item(RowExxel, 3) = Row("NombreArticulo")
                ClienteAnt = Row("Cliente") : SucursalAnt = Row("Sucursal") : ArticuloAnt = Row("Articulo") : NombreArticuloAnt = Row("NombreArticulo") : TotalCantidad = 0 : TotalDevueltas = 0
            End If
            If SucursalAnt <> Row("Sucursal") And PorClienteSucursal Then
                RowExxel = RowExxel + 1
                If Row("Sucursal") <> 0 Then exHoja.Cells.Item(RowExxel, 2) = NombreSucursalCliente(Row("Cliente"), Row("Sucursal"))
                exHoja.Cells.Item(RowExxel, 3) = Row("NombreArticulo")
                SucursalAnt = Row("Sucursal") : ArticuloAnt = Row("Articulo") : NombreArticuloAnt = Row("NombreArticulo") : TotalCantidad = 0 : TotalDevueltas = 0
            End If
            If ArticuloAnt <> Row("Articulo") Then
                RowExxel = RowExxel + 1
                exHoja.Cells.Item(RowExxel, 3) = Row("NombreArticulo")
                ArticuloAnt = Row("Articulo") : NombreArticuloAnt = Row("NombreArticulo") : TotalCantidad = 0 : TotalDevueltas = 0
            End If
            TotalCantidad = TotalCantidad + Row("Cantidad")
            TotalDevueltas = TotalDevueltas + Row("Devueltas")
            exHoja.Cells.Item(RowExxel, 4) = TotalCantidad
            exHoja.Cells.Item(RowExxel, 5) = TotalDevueltas
            exHoja.Cells.Item(RowExxel, 6) = TotalCantidad - TotalDevueltas
        Next

        Dt.Dispose()
        DtFacturas.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        '   AA = Chr(65 + 5 - 1) & InicioDatos
        '   BB = Chr(65 + 5 - 1) & UltimaLinea
        '   Rango = Nothing
        '   Rango = exHoja.Range(AA, BB)
        '   Rango.ColumnWidth = 18     'limita el ancho de la columna si son varios remito.

        'Muestra titulos.
        exHoja.Cells.Item(1, 7) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Informe Ventas Acumulado por Articulos Desde el : " & Format(Desde, "dd/MM/yyyy") & "  Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeComparativoPorArticulos(ByVal Cliente As Integer, ByVal Especie As Integer, ByVal Variedad As Integer, ByVal Mes As Integer, ByVal Anio As Integer, ByVal Sucursal As Integer)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = "Cliente"
        exHoja.Cells.Item(RowExxel, 2) = "Sucursal"
        exHoja.Cells.Item(RowExxel, 3) = "Articulo"
        exHoja.Cells.Item(RowExxel, 4) = "Semana 1"
        exHoja.Cells.Item(RowExxel, 5) = "Semana 2"
        exHoja.Cells.Item(RowExxel, 6) = "Semana 3"
        exHoja.Cells.Item(RowExxel, 7) = "Semana 4"
        exHoja.Cells.Item(RowExxel, 8) = "Semana 5"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Sql As String = ""

        If Especie <> 0 Then
            If Sql = "" Then
                Sql = "Especie = " & Especie
            Else : Sql = "AND Especie = " & Especie
            End If
        End If
        If Variedad <> 0 Then
            If Sql = "" Then
                Sql = "Variedad = " & Variedad
            Else : Sql = Sql & " AND Variedad = " & Variedad
            End If
        End If

        If Sql <> "" Then Sql = "WHERE " & Sql

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Articulos " & Sql & ";", Conexion, Dt) Then Exit Sub
        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Articulos.", MsgBoxStyle.Information)
            Dt.Dispose()
            Exit Sub
        End If

        Dim ViewA As New DataView      'arma lista acumulados.
        ViewA = Dt.DefaultView
        ViewA.Sort = "Nombre"
        Dim Lista As New List(Of FilaGenerica)
        For Each Row1 As DataRowView In ViewA
            Dim Item As New FilaGenerica
            Item.Secuencia = Row1("Clave")
            Item.String1 = Row1("Nombre")
            Lista.Add(Item)
        Next
        ViewA = Nothing

        Dim Desde As New DateTime(Anio, Mes, 1)
        Dim Hasta As DateTime = Desde.AddMonths(1).AddDays(-1)
        Dim Diferencia As Integer = DatePart("ww", Desde)

        Dim SqlFecha As String = ""
        SqlFecha = "C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlFechaContable As String = ""
        SqlFechaContable = "C.FechaContable >='" & Format(Desde, "yyyyMMdd") & "' AND C.FechaContable < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlCliente As String = ""
        If Cliente <> 0 Then SqlCliente = " AND C.Cliente = " & Cliente
        Dim SqlSucursal As String = ""
        If Sucursal <> 0 Then SqlSucursal = " AND C.Sucursal = " & Sucursal

        Dim SqlB As String
        Dim SqlN As String

        If Sucursal = 0 Then
            SqlB = "SELECT 1 AS Operacion,1 As Tipo,C.Importe,C.Moneda,C.Factura,C.Fecha,C.Remito,C.Cliente,C.Estado,C.Vendedor,C.Rel,C.Relacionada,C.Deposito,D.Articulo,D.Cantidad,D.Devueltas,D.Precio,D.TotalArticulo,D.Indice,C.Sucursal,C.Saldo FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE Tr = 0 AND C.Estado <> 3 AND D.Articulo <> 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & SqlCliente & SqlSucursal & _
                                 " UNION ALL " & _
                                 "SELECT 1 AS Operacion,2 AS Tipo,C.Importe,1 AS Moneda,C.Liquidacion AS Factura,C.FechaContable AS Fecha,D.Remito,C.Cliente,C.Estado,0 AS Vendedor,C.Rel,C.Nrel AS Relacionada,D.Deposito,R.Articulo,D.Cantidad,D.Merma AS Devueltas,D.Precio,D.NetoConIva AS TotalArticulo,D.Indice,0 AS Sucursal,C.Saldo FROM NVLPCabeza AS C INNER JOIN (NVLPLotes AS D INNER JOIN RemitosDetalle AS R ON D.Indice = R.Indice AND D.Remito = R.Remito) ON C.Liquidacion = D.Liquidacion WHERE C.Estado <> 3 AND " & SqlFechaContable & SqlCliente & ";"
            SqlN = "SELECT 2 AS Operacion,1 AS Tipo,C.Importe,C.Moneda,C.Factura,C.Fecha,C.Remito,C.Cliente,C.Estado,C.Vendedor,C.Rel,C.Relacionada,C.Deposito,D.Articulo,D.Cantidad,D.Devueltas,D.Precio,D.TotalArticulo,D.Indice,C.Sucursal,C.Saldo FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE Tr = 0 AND C.Rel = 0 AND C.Estado <> 3 AND D.Articulo <> 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & SqlCliente & SqlSucursal & _
                                 " UNION ALL " & _
                                 "SELECT 2 AS Operacion,2 AS Tipo,C.Importe,1 AS Moneda,C.Liquidacion AS Factura,C.FechaContable As Fecha,D.Remito,C.Cliente,C.Estado,0 AS Vendedor,C.Rel,C.Nrel AS Relacionada,D.Deposito,R.Articulo,D.Cantidad,D.Merma AS Devueltas,D.Precio,D.NetoConIva AS TotalArticulo,D.Indice,0 AS Sucursal,C.Saldo FROM NVLPCabeza AS C INNER JOIN (NVLPLotes AS D INNER JOIN RemitosDetalle AS R ON D.Indice = R.Indice AND D.Remito = R.Remito) ON C.Liquidacion = D.Liquidacion WHERE C.Rel = 0 AND C.Estado <> 3 AND " & SqlFechaContable & SqlCliente & ";"
        Else
            SqlB = "SELECT 1 AS Operacion,1 As Tipo,C.Importe,C.Moneda,C.Factura,C.Fecha,C.Remito,C.Cliente,C.Estado,C.Vendedor,C.Rel,C.Relacionada,C.Deposito,D.Articulo,D.Cantidad,D.Devueltas,D.Precio,D.TotalArticulo,D.Indice,C.Sucursal,C.Saldo FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE Tr = 0 AND C.Estado <> 3 AND D.Articulo <> 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & SqlCliente & SqlSucursal & ";"
            SqlN = "SELECT 2 AS Operacion,1 AS Tipo,C.Importe,C.Moneda,C.Factura,C.Fecha,C.Remito,C.Cliente,C.Estado,C.Vendedor,C.Rel,C.Relacionada,C.Deposito,D.Articulo,D.Cantidad,D.Devueltas,D.Precio,D.TotalArticulo,D.Indice,C.Sucursal,C.Saldo FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE Tr = 0 AND C.Rel = 0 AND C.Estado <> 3 AND D.Articulo <> 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & SqlCliente & SqlSucursal & ";"
        End If

        Dim DtFacturas As New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtFacturas) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtFacturas) Then Exit Sub
        End If

        If DtFacturas.Rows.Count = 0 Then
            MsgBox("No Hay Datos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim RowsBusqueda() As DataRow

        For Each Row1 As DataRow In DtFacturas.Rows
            RowsBusqueda = Dt.Select("Clave = " & Row1("Articulo"))
            If RowsBusqueda.Length = 0 Then
                Row1.Delete()
            End If
            If Row1.RowState <> DataRowState.Deleted Then
                If Sucursal <> 0 And Row1("Sucursal") <> Sucursal Then Row1.Delete()
            End If
        Next

        If DtFacturas.Rows.Count = 0 Then
            MsgBox("No Hay Datos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim View As New DataView
        View = DtFacturas.DefaultView
        View.Sort = "Cliente,Sucursal"

        Dim Row As DataRowView
        Dim ClienteAnt As Integer = 0
        Dim SucursalAnt As Integer = 0
        Dim ClienteAct As Integer = 0
        Dim SucursalAct As Integer = 0
        Dim Columna As Integer

        RowExxel = RowExxel + 1

        For Each Row In View
            If ClienteAnt <> Row("Cliente") Then
                ClienteAct = Row("Cliente")
                If ClienteAnt <> 0 Then
                    CorteXCliente_InformeComparativoPorArticulos(exHoja, RowExxel, Lista, ClienteAnt, SucursalAnt)
                End If
                ClienteAnt = Row("Cliente") : SucursalAnt = Row("Sucursal")
            End If
            If SucursalAnt <> Row("Sucursal") Then
                SucursalAct = Row("Sucursal")
                CorteXCliente_InformeComparativoPorArticulos(exHoja, RowExxel, Lista, ClienteAnt, SucursalAnt)
                SucursalAnt = Row("Sucursal")
            End If
            Columna = DatePart("ww", Row("Fecha")) - Diferencia + 1
            Dim Fila As Integer = 0
            For Each FilaL As FilaGenerica In Lista
                If FilaL.Secuencia = Row("Articulo") Then
                    Select Case Columna
                        Case 1
                            FilaL.Importe1 = FilaL.Importe1 + Row("Cantidad") - Row("Devueltas")
                        Case 2
                            FilaL.Importe2 = FilaL.Importe2 + Row("Cantidad") - Row("Devueltas")
                        Case 3
                            FilaL.Importe3 = FilaL.Importe3 + Row("Cantidad") - Row("Devueltas")
                        Case 4
                            FilaL.Importe4 = FilaL.Importe4 + Row("Cantidad") - Row("Devueltas")
                        Case 5
                            FilaL.Importe5 = FilaL.Importe5 + Row("Cantidad") - Row("Devueltas")
                    End Select
                    Exit For
                End If
            Next
        Next
        CorteXCliente_InformeComparativoPorArticulos(exHoja, RowExxel, Lista, ClienteAnt, SucursalAnt)

        Dt.Dispose()
        DtFacturas.Dispose()
        View = Nothing
        Lista = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        '   AA = Chr(65 + 5 - 1) & InicioDatos
        '   BB = Chr(65 + 5 - 1) & UltimaLinea
        '   Rango = Nothing
        '   Rango = exHoja.Range(AA, BB)
        '   Rango.ColumnWidth = 18     'limita el ancho de la columna si son varios remito.

        'Muestra titulos.
        exHoja.Cells.Item(1, 9) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Informe Comparativo Cantidades Facturadas por Articulos del Mes : " & Mes & "  Anio " & Anio

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub CorteXCliente_InformeComparativoPorArticulos(ByVal exHoja As Microsoft.Office.Interop.Excel.Worksheet, ByRef RowExxel As Integer, ByRef Lista As List(Of FilaGenerica), ByVal ClienteAnt As Integer, ByVal SucursalAnt As Integer)

        exHoja.Cells.Item(RowExxel, 1) = NombreCliente(ClienteAnt)
        If SucursalAnt <> 0 Then exHoja.Cells.Item(RowExxel, 2) = NombreSucursalCliente(ClienteAnt, SucursalAnt)

        For Each FilaA As FilaGenerica In Lista
            If FilaA.Importe1 <> 0 Or FilaA.Importe2 <> 0 Or FilaA.Importe3 <> 0 Or FilaA.Importe4 <> 0 Or FilaA.Importe5 <> 0 Then
                exHoja.Cells.Item(RowExxel, 3) = FilaA.String1
                If FilaA.Importe1 <> 0 Then exHoja.Cells.Item(RowExxel, 4) = FilaA.Importe1
                If FilaA.Importe2 <> 0 Then exHoja.Cells.Item(RowExxel, 5) = FilaA.Importe2
                If FilaA.Importe3 <> 0 Then exHoja.Cells.Item(RowExxel, 6) = FilaA.Importe3
                If FilaA.Importe4 <> 0 Then exHoja.Cells.Item(RowExxel, 7) = FilaA.Importe4
                If FilaA.Importe5 <> 0 Then exHoja.Cells.Item(RowExxel, 8) = FilaA.Importe5
                RowExxel = RowExxel + 1
            End If
        Next
        For Each FilaA As FilaGenerica In Lista
            FilaA.Importe1 = 0
            FilaA.Importe2 = 0
            FilaA.Importe3 = 0
            FilaA.Importe4 = 0
            FilaA.Importe5 = 0
        Next

    End Sub
    Public Sub InformeRemitos(ByVal Cliente As Integer, ByVal Especie As Integer, ByVal Variedad As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Estado As Integer, _
                              ByVal Blanco As Boolean, ByVal Negro As Boolean, ByVal CanalVenta As Integer, ByVal Facturadas As Boolean, ByVal PendienteFacturar As Boolean, ByVal Deposito As Integer, _
                              ByVal MuestraLotes As Boolean, ByVal RepetirDatos As Boolean, ByVal Sucursal As Integer, ByVal PorFechaEmision As Boolean, ByVal PorFechaEntrega As Boolean, _
                              ByVal TextRemitos As TextBox, ByVal MuestraPedido As Boolean, ByVal SoloConfirmados As Boolean)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = "Cliente"
        exHoja.Cells.Item(RowExxel, 2) = " "
        exHoja.Cells.Item(RowExxel, 3) = "Remito"
        exHoja.Cells.Item(RowExxel, 4) = "Fecha Rem."
        exHoja.Cells.Item(RowExxel, 5) = "Factura"
        exHoja.Cells.Item(RowExxel, 6) = "Canal Venta"
        exHoja.Cells.Item(RowExxel, 7) = "Deposito"
        exHoja.Cells.Item(RowExxel, 8) = "Estado"
        exHoja.Cells.Item(RowExxel, 9) = "Articulo"
        exHoja.Cells.Item(RowExxel, 10) = "Cantidad"
        exHoja.Cells.Item(RowExxel, 11) = "Devolución"
        exHoja.Cells.Item(RowExxel, 12) = "Tiene NVLP"
        exHoja.Cells.Item(RowExxel, 13) = "Lotes"
        exHoja.Cells.Item(RowExxel, 14) = "Valor"
        exHoja.Cells.Item(RowExxel, 15) = "Sucursal"
        exHoja.Cells.Item(RowExxel, 16) = "Fecha Entrega"
        exHoja.Cells.Item(RowExxel, 17) = "Por Cuenta Y Orden"
        exHoja.Cells.Item(RowExxel, 18) = "Pedido Cliente"
        exHoja.Cells.Item(RowExxel, 19) = "Confirmado"
        exHoja.Cells.Item(RowExxel, 20) = "Sin NVLP"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Sql As String = ""

        If Especie <> 0 Then
            If Sql = "" Then
                Sql = "Especie = " & Especie
            Else : Sql = "AND Especie = " & Especie
            End If
        End If
        If Variedad <> 0 Then
            If Sql = "" Then
                Sql = "Variedad = " & Variedad
            Else : Sql = Sql & " AND Variedad = " & Variedad
            End If
        End If

        If Sql <> "" Then Sql = "WHERE " & Sql

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Articulos " & Sql & ";", Conexion, Dt) Then Exit Sub
        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Articulos.", MsgBoxStyle.Information)
            Dt.Dispose()
            Exit Sub
        End If

        Dim SqlFecha As String = ""
        If PorFechaEmision Then
            SqlFecha = "C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Else
            SqlFecha = "C.FechaEntrega >='" & Format(Desde, "yyyyMMdd") & "' AND C.FechaEntrega < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        End If
        Dim SqlEstado As String = ""
        If Estado <> 0 Then SqlEstado = " AND C.Estado = " & Estado
        If Estado = 4 Then SqlEstado = " AND C.Estado <> 3"
        Dim SqlCliente As String = ""
        If Cliente <> 0 Then SqlCliente = " AND C.Cliente = " & Cliente
        Dim SqlFactura As String = ""
        If Estado = 2 Then SqlFactura = " AND C.Factura = 0 "
        Dim SqlDeposito As String = ""
        If Deposito <> 0 Then SqlDeposito = " AND Deposito = " & Deposito
        Dim SqlSucursal As String = ""
        If Sucursal <> 0 Then SqlSucursal = " AND Sucursal = " & Sucursal
        Dim SqlSoloConfirmados As String = ""
        If SoloConfirmados <> 0 Then SqlSoloConfirmados = " AND Confirmado = 1"

        Dim SqlB As String = "SELECT 1 AS Operacion,0 AS Liquidado,'' as NombreArticulo,C.Remito,C.Fecha,C.FechaEntrega,C.Factura,C.Cliente,C.Estado,C.Deposito,D.Precio,D.Articulo,D.Cantidad,D.Devueltas,D.Indice,C.Sucursal,C.PorCuentaYOrden,C.pedido,C.Confirmado FROM RemitosCabeza AS C INNER JOIN RemitosDetalle As D ON C.Remito = D.Remito WHERE D.Articulo <> 0 AND " & SqlFecha & SqlEstado & SqlCliente & SqlFactura & SqlDeposito & SqlSucursal & SqlSoloConfirmados & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion,0 AS Liquidado,'' as NombreArticulo,C.Remito,C.Fecha,C.FechaEntrega,C.Factura,C.Cliente,C.Estado,C.Deposito,D.Precio,D.Articulo,D.Cantidad,D.Devueltas,D.Indice,C.Sucursal,C.PorCuentaYOrden,C.Pedido,C.Confirmado FROM RemitosCabeza AS C INNER JOIN RemitosDetalle As D ON C.Remito = D.Remito WHERE D.Articulo <> 0 AND " & SqlFecha & SqlEstado & SqlCliente & SqlFactura & SqlDeposito & SqlSucursal & SqlSoloConfirmados & ";"

        Dim DtRemitos As New DataTable
        If Blanco Then
            If Not Tablas.Read(SqlB, Conexion, DtRemitos) Then Exit Sub
        End If
        If Negro Then
            If Not Tablas.Read(SqlN, ConexionN, DtRemitos) Then Exit Sub
        End If

        If DtRemitos.Rows.Count = 0 Then
            MsgBox("No Hay Datos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If TextRemitos.Lines.Length <> 0 Then
            For Each Row2 As DataRow In DtRemitos.Rows
                If Not EstaRemitoEnTextBox(Row2("Remito"), TextRemitos) Then
                    Row2.Delete()
                End If
            Next
        End If

        Dim Borrar As Boolean
        Dim RowsBusqueda() As DataRow
        Dim Liquidado As Boolean
        Dim RemitoAnt As Double = 0

        Dim View As New DataView
        View = DtRemitos.DefaultView
        View.Sort = "Fecha,Remito"

        Dim Row As DataRowView

        For Each Row In View
            Borrar = False
            If Row("Factura") = 0 Then
                If RemitoAnt <> Row("Remito") Then
                    Liquidado = EstaRemitoLiquidado(Row("Operacion"), Row("Remito"))
                    RemitoAnt = Row("Remito")
                End If
                If Facturadas And Not PendienteFacturar And Not Liquidado Then Borrar = True
                If PendienteFacturar And Not Facturadas And Liquidado Then Borrar = True
                If Liquidado Then Row("Liquidado") = 1
            End If
            If Row("Factura") <> 0 Then
                If PendienteFacturar And Not Facturadas Then Borrar = True
            End If
            If Borrar Then
                Row.Delete()
            Else
                RowsBusqueda = Dt.Select("Clave = " & Row("Articulo"))
                If RowsBusqueda.Length <> 0 Then
                    Row("NombreArticulo") = RowsBusqueda(0).Item("Nombre")
                Else
                    Row.Delete()
                End If
            End If
        Next

        Dim DtLotes As New DataTable
        RemitoAnt = 0
        Dim ConexionStr As String

        For Each Row In View
            If RemitoAnt <> Row("Remito") Then
                HallaDatosCliente(Row("Cliente"))
                exHoja.Cells.Item(RowExxel + 1, 1) = NombreClienteW
                If Row("operacion") = 2 Then exHoja.Cells.Item(RowExxel + 1, 2) = "X"
                exHoja.Cells.Item(RowExxel + 1, 3) = NumeroEditado(Row("Remito"))
                exHoja.Cells.Item(RowExxel + 1, 4) = Format(Row("Fecha"), "MM/dd/yyyy")
                If Row("Factura") <> 0 Then exHoja.Cells.Item(RowExxel + 1, 5) = NumeroEditado(Row("Factura"))
                If CanalVentaW <> 0 Then exHoja.Cells.Item(RowExxel + 1, 6) = HallaCanalDeVenta(CanalVentaW)
                exHoja.Cells.Item(RowExxel + 1, 7) = NombreDeposito(Row("Deposito"))
                If Row("Estado") = 1 Then exHoja.Cells.Item(RowExxel + 1, 8) = "Afecta Stock"
                If Row("Estado") = 2 Then exHoja.Cells.Item(RowExxel + 1, 8) = "Pendiente de Afectar"
                If Row("Estado") = 3 Then exHoja.Cells.Item(RowExxel + 1, 8) = "Baja"
                If Row("Sucursal") <> 0 Then exHoja.Cells.Item(RowExxel + 1, 15) = NombreSucursalCliente(Row("Cliente"), Row("Sucursal"))
                exHoja.Cells.Item(RowExxel + 1, 16) = Format(Row("FechaEntrega"), "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel + 1, 17) = NombreCliente(Row("PorCuentaYOrden"))
                If MuestraPedido Then
                    exHoja.Cells.Item(RowExxel + 1, 18) = HallaPedidoCliente(Row("Pedido"))
                End If
                If Row("Confirmado") Then exHoja.Cells.Item(RowExxel + 1, 19) = "SI"
                RemitoAnt = Row("Remito")
            End If
            RowExxel = RowExxel + 1
            If RepetirDatos And exHoja.Cells.Item(RowExxel, 1).value = "" Then
                exHoja.Cells.Item(RowExxel, 1) = exHoja.Cells.Item(RowExxel - 1, 1)
                exHoja.Cells.Item(RowExxel, 2) = exHoja.Cells.Item(RowExxel - 1, 2)
                exHoja.Cells.Item(RowExxel, 3) = exHoja.Cells.Item(RowExxel - 1, 3)
                exHoja.Cells.Item(RowExxel, 4) = Format(Row("Fecha"), "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel, 5) = exHoja.Cells.Item(RowExxel - 1, 5)
                exHoja.Cells.Item(RowExxel, 6) = exHoja.Cells.Item(RowExxel - 1, 6)
                exHoja.Cells.Item(RowExxel, 7) = exHoja.Cells.Item(RowExxel - 1, 7)
                exHoja.Cells.Item(RowExxel, 8) = exHoja.Cells.Item(RowExxel - 1, 8)
                exHoja.Cells.Item(RowExxel, 15) = exHoja.Cells.Item(RowExxel - 1, 15)
                exHoja.Cells.Item(RowExxel, 16) = exHoja.Cells(RowExxel - 1, 16).text
                exHoja.Cells.Item(RowExxel, 17) = exHoja.Cells(RowExxel - 1, 17).text
                exHoja.Cells.Item(RowExxel, 18) = exHoja.Cells(RowExxel - 1, 18).text
                exHoja.Cells.Item(RowExxel, 19) = exHoja.Cells(RowExxel - 1, 19).text
            End If
            exHoja.Cells.Item(RowExxel, 9) = Row("NombreArticulo")
            exHoja.Cells.Item(RowExxel, 10) = Row("Cantidad")
            exHoja.Cells.Item(RowExxel, 11) = Row("Devueltas")
            If Row("Liquidado") Then exHoja.Cells.Item(RowExxel, 12) = "SI"

            Dim Iva As Decimal = HallaIva(Row("Articulo"))
            Dim Valor As Decimal
            If Row("Operacion") = 2 Then
                Valor = Row("Precio") / (1 + Iva / 100)
            Else
                Valor = Row("Precio")
            End If
            Valor = Trunca(Valor)
            exHoja.Cells.Item(RowExxel, 14) = CalculaNeto(Row("Cantidad") - Row("Devueltas"), Valor)
            If MuestraLotes Then
                If Row("operacion") = 1 Then
                    ConexionStr = Conexion
                Else : ConexionStr = ConexionN
                End If
                DtLotes.Clear()
                If Not Tablas.Read("SELECT Lote,Secuencia,Facturado,Liquidado FROM AsignacionLotes WHERE Cantidad <> 0 AND TipoComprobante = 1 AND Comprobante = " & Row("Remito") & " AND Indice = " & Row("Indice") & ";", ConexionStr, DtLotes) Then Exit Sub
                If DtLotes.Rows.Count <> 0 Then RowExxel = RowExxel - 1
                For Each Row1 As DataRow In DtLotes.Rows
                    RowExxel = RowExxel + 1
                    exHoja.Cells.Item(RowExxel, 13) = Row1("lote") & "/" & Format(Row1("Secuencia"), "000")
                    If Row1("Liquidado") = 0 And Row1("Facturado") = 0 Then exHoja.Cells.Item(RowExxel, 20) = "SI"
                Next
            End If
        Next

        Dt.Dispose()
        DtRemitos.Dispose()
        View = Nothing
        DtLotes.Dispose()

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 12 - 1) & InicioDatos
        BB = Chr(65 + 12 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 13 - 1) & InicioDatos
        BB = Chr(65 + 13 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 14 - 1) & InicioDatos
        BB = Chr(65 + 14 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 16 - 1) & InicioDatos
        BB = Chr(65 + 16 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 15) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Informe Remitos Desde el : " & Format(Desde, "dd/MM/yyyy") & "  Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub TotalizaArticulosRemitidos(ByVal Cliente As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Sucursal As Integer)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = "Articulo"
        exHoja.Cells.Item(RowExxel, 2) = "U.Medida/Por Envase"
        exHoja.Cells.Item(RowExxel, 3) = "Remitidas"
        exHoja.Cells.Item(RowExxel, 4) = "Devolución"
        exHoja.Cells.Item(RowExxel, 5) = "Cant.Neto"
        exHoja.Cells.Item(RowExxel, 6) = "U.Medida Netos"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim SqlFecha As String = ""
        SqlFecha = "C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlCliente As String = ""
        If Cliente <> 0 Then SqlCliente = " AND C.Cliente = " & Cliente
        Dim SqlSucursal As String = ""
        If Sucursal <> 0 Then SqlSucursal = " AND C.Sucursal = " & Sucursal

        Dim Sql As String = "SELECT D.Articulo,D.Cantidad,D.Devueltas,D.KilosXUnidad FROM RemitosCabeza AS C INNER JOIN RemitosDetalle As D ON C.Remito = D.Remito WHERE C.Estado <> 3 AND D.Articulo <> 0 AND " & SqlFecha & SqlCliente & SqlSucursal & ";"

        Dim Dt As New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Exit Sub
        End If

        If Dt.Rows.Count = 0 Then
            MsgBox("No Hay Datos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim Remitida As Decimal
        Dim Devuelta As Decimal
        Dim KilosXUnidad As Decimal
        Dim UMedida As String

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Articulo"

        Dim Row As DataRowView
        Dim ArticuloAnt As Integer

        For Each Row In View
            If Row("Articulo") <> ArticuloAnt Then
                If ArticuloAnt <> 0 Then
                    exHoja.Cells.Item(RowExxel, 1) = NombreArticulo(ArticuloAnt)
                    exHoja.Cells.Item(RowExxel, 2) = KilosXUnidad
                    exHoja.Cells.Item(RowExxel, 3) = Remitida
                    exHoja.Cells.Item(RowExxel, 4) = Devuelta
                    exHoja.Cells.Item(RowExxel, 5) = Remitida - Devuelta
                    exHoja.Cells.Item(RowExxel, 6) = KilosXUnidad * (Remitida - Devuelta) & " " & UMedida
                    RowExxel = RowExxel + 1
                End If
                Dim KilosXUnidadWW As Decimal
                HallaKilosXUnidadYUMedida(Row("Articulo"), KilosXUnidadWW, UMedida)
                ArticuloAnt = Row("Articulo")
                KilosXUnidad = Row("KilosXUnidad")
                Remitida = 0
                Devuelta = 0
            End If
            Remitida = Remitida + Row("Cantidad")
            Devuelta = Devuelta + Row("Devueltas")
        Next
        '
        exHoja.Cells.Item(RowExxel, 1) = NombreArticulo(ArticuloAnt)
        exHoja.Cells.Item(RowExxel, 2) = KilosXUnidad
        exHoja.Cells.Item(RowExxel, 3) = Remitida
        exHoja.Cells.Item(RowExxel, 4) = Devuelta
        exHoja.Cells.Item(RowExxel, 5) = Remitida - Devuelta
        exHoja.Cells.Item(RowExxel, 6) = KilosXUnidad * (Remitida - Devuelta) & " " & UMedida

        Dt.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 8) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(1, 1) = "Totalizador Articulos Remitidos Desde el : " & Format(Desde, "dd/MM/yyyy") & "  Hasta el " & Format(Hasta, "dd/MM/yyyy")
        Dim Titulo As String = "Para el Cliente : " & NombreCliente(Cliente)
        If Sucursal <> 0 Then Titulo = Titulo & "     Sucursal " & NombreSucursalCliente(Cliente, Sucursal)
        exHoja.Cells.Item(2, 1) = Titulo

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub SeniasPagadas(ByVal Proveedor As Integer, ByVal Desde As Date, ByVal Hasta As Date)

        Dim Dt As New DataTable

        Dim SqlFecha As String = ""
        SqlFecha = " AND C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlProveedor As String
        If Proveedor <> 0 Then
            SqlProveedor = " AND C.Proveedor = " & Proveedor
        End If

        Dim SqlB As String = "SELECT 1 As Operacion,C.Factura,C.Rel,C.Nrel,C.ReciboOficial,C.FechaFactura AS Fecha,C.Proveedor FROM FacturasProveedorCabeza AS C WHERE C.Estado <> 3" & SqlProveedor & SqlFecha & ";"
        Dim SqlN As String = "SELECT 2 As Operacion,C.Factura,C.Rel,C.Nrel,C.ReciboOficial,C.FechaFactura AS Fecha,C.Proveedor FROM FacturasProveedorCabeza AS C WHERE C.Estado <> 3 AND C.Rel = 0" & SqlProveedor & SqlFecha & ";"

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Exit Sub
        End If

        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Factura"
        exHoja.Cells.Item(RowExxel, 3) = "Fecha"
        exHoja.Cells.Item(RowExxel, 4) = "Cantidad"
        exHoja.Cells.Item(RowExxel, 5) = "Pagado"
        exHoja.Cells.Item(RowExxel, 6) = "Proveedor"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Senia As Decimal
        Dim Total As Decimal = 0
        Dim TotalCantidad As Decimal = 0

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Fecha,Factura"

        For Each Row As DataRowView In View
            Senia = 0
            Dim FacturaRel As Double = 0
            If Row("Rel") Then
                FacturaRel = HallaFacturaProveedorRelacionada(Row("Factura"), Conexion, Row("Rel"), Row("Nrel"))
            End If
            Dim ConexionStr As String = ""
            If Row("Operacion") = 1 Then
                ConexionStr = Conexion
            Else : ConexionStr = ConexionN
            End If
            Senia = HallaSeniaEnFacturaReventa(Row("Factura"), FacturaRel, ConexionStr)
            If Senia <> 0 Then
                RowExxel = RowExxel + 1
                If Row("Operacion") = 2 Then
                    exHoja.Cells.Item(RowExxel, 1) = "X"
                End If
                exHoja.Cells.Item(RowExxel, 2) = NumeroEditado(Row("ReciboOficial"))
                exHoja.Cells.Item(RowExxel, 3) = Format(Row("Fecha"), "MM/dd/yyyy")
                Dim Unidades As Decimal = HallaUnidadesSeniaPagada(Row("Factura"), Row("Rel"), Row("Nrel"), ConexionStr)
                exHoja.Cells.Item(RowExxel, 4) = Unidades
                exHoja.Cells.Item(RowExxel, 5) = Senia
                exHoja.Cells.Item(RowExxel, 6) = NombreProveedor(Row("Proveedor"))
                Total = Total + Senia
                TotalCantidad = TotalCantidad + Unidades
            End If
        Next
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 2) = "Totales"
        exHoja.Cells.Item(RowExxel, 4) = TotalCantidad
        exHoja.Cells.Item(RowExxel, 5) = Total

        Dt.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 10) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Señas Pagadas Del proveedor Dese el " & Format(Desde, "dd/MM/yyyy") & " Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub SeniasRecuperadas(ByVal Proveedor As Integer, ByVal Desde As Date, ByVal Hasta As Date)

        Dim Dt As New DataTable

        Dim SqlFecha As String = ""
        SqlFecha = " AND FechaVale >='" & Format(Desde, "yyyyMMdd") & "' AND FechaVale < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlProveedor As String
        If Proveedor <> 0 Then
            SqlProveedor = " AND Proveedor = " & Proveedor
        End If

        Dim SqlB As String = "SELECT 1 As Operacion,Vale,FechaVale AS Fecha,Cantidad,Importe,Proveedor FROM RecuperoSenia WHERE Estado <> 3" & SqlProveedor & SqlFecha & ";"
        Dim SqlN As String = "SELECT 2 As Operacion,Vale,FechaVale AS Fecha,Cantidad,Importe,Proveedor FROM RecuperoSenia WHERE Estado <> 3" & SqlProveedor & SqlFecha & ";"

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Exit Sub
        End If

        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Information)
            Dt.Dispose()
            Exit Sub
        End If

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Vale"
        exHoja.Cells.Item(RowExxel, 3) = "Fecha"
        exHoja.Cells.Item(RowExxel, 4) = "Unidades"
        exHoja.Cells.Item(RowExxel, 5) = "Importe"
        exHoja.Cells.Item(RowExxel, 6) = "^Proveedor"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Total As Decimal = 0
        Dim TotalCantidad As Decimal = 0

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Fecha,Vale"

        For Each Row As DataRowView In View
            RowExxel = RowExxel + 1
            If Row("Operacion") = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
            exHoja.Cells.Item(RowExxel, 2) = NumeroEditado(Row("Vale"))
            exHoja.Cells.Item(RowExxel, 3) = Format(Row("Fecha"), "MM/dd/yyyy")
            exHoja.Cells.Item(RowExxel, 4) = Row("Cantidad")
            exHoja.Cells.Item(RowExxel, 5) = Row("Importe")
            exHoja.Cells.Item(RowExxel, 6) = NombreProveedor(Row("Proveedor"))
            Total = Total + Row("Importe")
            TotalCantidad = TotalCantidad + Row("Cantidad")
        Next
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 2) = "Totales"
        exHoja.Cells.Item(RowExxel, 4) = TotalCantidad
        exHoja.Cells.Item(RowExxel, 5) = Total

        Dt.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 10) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Señas Recuperadas Dese el " & Format(Desde, "dd/MM/yyyy") & " Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeEnvasesPerdidos(ByVal Desde As Date, ByVal Hasta As Date, ByVal Proveedor As Integer)

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Articulos;", Conexion, Dt) Then Exit Sub
        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Articulos.", MsgBoxStyle.Information)
            Dt.Dispose()
            Exit Sub
        End If

        Dim DtLotes As New DataTable
        Dim SqlFecha As String = ""

        SqlFecha = "C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "

        Dim SqlB As String = "SELECT 1 AS OPR,C.Factura,C.Fecha,C.Senia,A.Lote,A.Secuencia,A.Operacion,A.Cantidad FROM FacturasCabeza AS C INNER JOIN AsignacionLotes As A ON C.Factura = A.Comprobante AND A.TipoComprobante = 2 WHERE C.Tr = 0 AND C.Senia = 0 AND " & SqlFecha & ";"
        Dim SqlN As String = "SELECT 2 AS OPR,C.Factura,C.Fecha,C.Senia,A.Lote,A.Secuencia,A.Operacion,A.Cantidad FROM FacturasCabeza AS C INNER JOIN AsignacionLotes As A ON C.Factura = A.Comprobante AND A.TipoComprobante = 2 WHERE C.Tr = 0 AND C.Rel = 0 AND C.Senia = 0 AND " & SqlFecha & ";"

        If Not Tablas.Read(SqlB, Conexion, DtLotes) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtLotes) Then Exit Sub
        End If

        If DtLotes.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Information)
            Dt.Dispose()
            Exit Sub
        End If
        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Fecha"
        exHoja.Cells.Item(RowExxel, 3) = "Factura"
        exHoja.Cells.Item(RowExxel, 4) = "Lote"
        exHoja.Cells.Item(RowExxel, 5) = "Articulo"
        exHoja.Cells.Item(RowExxel, 6) = "Proveedor"
        exHoja.Cells.Item(RowExxel, 7) = "Cantidad Env."
        exHoja.Cells.Item(RowExxel, 8) = "Seña Compra"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim View As New DataView
        View = DtLotes.DefaultView
        View.Sort = "Fecha,Factura"

        Dim ConexionStr As String = ""
        Dim Senia As Decimal = 0
        Dim TotalCompra As Decimal = 0
        Dim TotalVenta As Decimal = 0
        Dim Cantidad As Decimal = 0
        Dim TipoOperacion As Integer
        Dim Articulo As Integer
        Dim ProveedorW As Integer
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRowView In View
            If Row("OPR") = 1 Then
                ConexionStr = Conexion
            Else : ConexionStr = ConexionN
            End If
            HallaTipoOperacionSeniaArticulo(Row("Lote"), Row("Secuencia"), ConexionStr, TipoOperacion, Senia, Articulo, ProveedorW)
            If Senia > 0 And TipoOperacion = 2 Then
                If Proveedor = 0 Or (Proveedor <> 0 And Proveedor = ProveedorW) Then
                    RowsBusqueda = Dt.Select("Clave = " & Articulo)
                    Cantidad = Trunca(Senia * Row("Cantidad"))
                    TotalCompra = TotalCompra + Cantidad
                    RowExxel = RowExxel + 1
                    If Row("OPR") = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
                    exHoja.Cells.Item(RowExxel, 2) = Format(Row("Fecha"), "MM/dd/yyyy")
                    exHoja.Cells.Item(RowExxel, 3) = NumeroEditado(Row("Factura"))
                    exHoja.Cells.Item(RowExxel, 4) = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
                    exHoja.Cells.Item(RowExxel, 5) = ""
                    If RowsBusqueda.Length <> 0 Then exHoja.Cells.Item(RowExxel, 5) = RowsBusqueda(0).Item("Nombre")
                    exHoja.Cells.Item(RowExxel, 6) = NombreProveedor(ProveedorW)
                    exHoja.Cells.Item(RowExxel, 7) = Row("Cantidad")
                    exHoja.Cells.Item(RowExxel, 8) = Row("Cantidad") * Senia
                End If
            End If
        Next
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 5) = "Totales"
        exHoja.Cells.Item(RowExxel, 8) = TotalCompra

        Dt.Dispose()
        DtLotes.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 9) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Envases perdidos Dese el " & Format(Desde, "dd/MM/yyyy") & " Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeReprocesos(ByVal EspecieB As Integer, ByVal VariedadB As Integer, ByVal EnvaseB As Integer, ByVal EspecieA As Integer, ByVal VariedadA As Integer, ByVal EnvaseA As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Estado As Integer, ByVal Blanco As Boolean, ByVal Negro As Boolean, ByVal Deposito As Integer, ByVal Proveedor As Integer)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Fecha"
        exHoja.Cells.Item(RowExxel, 3) = "Reproceso "
        exHoja.Cells.Item(RowExxel, 4) = "Deposito"
        exHoja.Cells.Item(RowExxel, 5) = "Estado"
        exHoja.Cells.Item(RowExxel, 6) = "Proveedor"
        exHoja.Cells.Item(RowExxel, 7) = "Art. Baja"
        exHoja.Cells.Item(RowExxel, 8) = "U.Medida/Uni."
        exHoja.Cells.Item(RowExxel, 9) = "Baja"
        exHoja.Cells.Item(RowExxel, 10) = "Merma"
        exHoja.Cells.Item(RowExxel, 11) = "Art. Alta"
        exHoja.Cells.Item(RowExxel, 12) = "U.Medida/Uni."
        exHoja.Cells.Item(RowExxel, 13) = "Alta"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT A.Clave,A.Nombre,A.Especie,A.Variedad,A.Envase,E.Kilos FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave;", Conexion, Dt) Then Exit Sub
        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Articulos.", MsgBoxStyle.Information)
            Dt.Dispose()
            Exit Sub
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlEstado As String = ""
        If Estado <> 0 Then SqlEstado = " AND C.Estado = " & Estado
        Dim SqlDeposito As String = ""
        If Deposito <> 0 Then SqlDeposito = " AND C.Deposito = " & Deposito
        Dim SqlProveedor As String = ""
        If Proveedor <> 0 Then SqlProveedor = " AND L.Proveedor = " & Proveedor

        Dim SqlB As String
        Dim SqlN As String

        'Se procesa C.lote <> 0 para conpatbilizar con ultimo arreglo con C.Lote = 0.

        SqlB = "SELECT 1 AS Operacion,'B' AS Tipo,L.KilosXunidad,'' AS NombreArticulo,D.Lote AS Lote,D.Secuencia AS Secuencia,D.Baja AS Cantidad,D.Merma AS Merma,C.Clave,C.Fecha,C.Deposito,C.Estado,L.Proveedor FROM ReprocesoCabeza AS C INNER JOIN (ReprocesoDetalleBaja As D INNER JOIN Lotes As L ON D.Lote = L.Lote AND D.Secuencia = L.Secuencia) ON C.Clave = D.Clave WHERE C.Lote = 0 AND " & SqlFecha & SqlEstado & SqlDeposito & SqlProveedor & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,'B' AS Tipo,L.KilosXunidad,'' AS NombreArticulo,C.Lote AS Lote,C.Secuencia AS Secuencia,C.Baja AS Cantidad,C.Merma AS Merma,C.Clave,C.Fecha,C.Deposito,C.Estado,L.Proveedor FROM ReprocesoCabeza AS C INNER JOIN Lotes As L ON C.Lote = L.Lote AND C.Secuencia = L.Secuencia WHERE C.Lote <> 0 AND " & SqlFecha & SqlEstado & SqlDeposito & SqlProveedor & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,'A' AS Tipo,L.KilosXunidad,'' AS NombreArticulo,D.Lote AS Lote,D.Secuencia AS Secuencia,D.Alta AS Cantidad,0 AS Merma,C.Clave,C.Fecha,C.Deposito,C.Estado,L.Proveedor FROM ReprocesoCabeza AS C INNER JOIN (ReprocesoDetalle As D INNER JOIN Lotes As L ON D.Lote = L.Lote AND D.Secuencia = L.Secuencia) ON C.Clave = D.Clave WHERE " & SqlFecha & SqlEstado & SqlDeposito & SqlProveedor & ";"

        SqlN = "SELECT 2 AS Operacion,'B' AS Tipo,L.KilosXunidad,'' AS NombreArticulo,D.Lote AS Lote,D.Secuencia AS Secuencia,D.Baja AS Cantidad,D.Merma AS Merma,C.Clave,C.Fecha,C.Deposito,C.Estado,L.Proveedor FROM ReprocesoCabeza AS C INNER JOIN (ReprocesoDetalleBaja As D INNER JOIN Lotes As L ON D.Lote = L.Lote AND D.Secuencia = L.Secuencia) ON C.Clave = D.Clave WHERE C.Lote = 0 AND " & SqlFecha & SqlEstado & SqlDeposito & SqlProveedor & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,'B' AS Tipo,L.KilosXunidad,'' AS NombreArticulo,C.Lote AS Lote,C.Secuencia AS Secuencia,C.Baja AS Cantidad,C.Merma AS Merma,C.Clave,C.Fecha,C.Deposito,C.Estado,L.Proveedor FROM ReprocesoCabeza AS C INNER JOIN Lotes As L ON C.Lote = L.Lote AND C.Secuencia = L.Secuencia WHERE C.Lote <> 0 AND " & SqlFecha & SqlEstado & SqlDeposito & SqlProveedor & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,'A' AS Tipo,L.KilosXunidad,'' AS NombreArticulo,D.Lote AS Lote,D.Secuencia AS Secuencia,D.Alta AS Cantidad,0 AS Merma,C.Clave,C.Fecha,C.Deposito,C.Estado,L.Proveedor FROM ReprocesoCabeza AS C INNER JOIN (ReprocesoDetalle As D INNER JOIN Lotes As L ON D.Lote = L.Lote AND D.Secuencia = L.Secuencia) ON C.Clave = D.Clave WHERE " & SqlFecha & SqlEstado & SqlDeposito & SqlProveedor & ";"

        Dim DtRepro As New DataTable
        If Blanco Then
            If Not Tablas.Read(SqlB, Conexion, DtRepro) Then Exit Sub
        End If
        If Negro Then
            If Not Tablas.Read(SqlN, ConexionN, DtRepro) Then Exit Sub
        End If

        If DtRepro.Rows.Count = 0 Then
            MsgBox("No Hay Datos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim Borrar As Boolean
        Dim RowsBusqueda() As DataRow
        Dim Articulo As Integer
        Dim ConexionStr As String

        For Each Row1 As DataRow In DtRepro.Rows
            Borrar = False
            If Row1("Operacion") = 1 Then
                ConexionStr = Conexion
            Else : ConexionStr = ConexionN
            End If
            Articulo = HallaArticulo(Row1("Lote"), Row1("Secuencia"), ConexionStr)
            If Articulo < 0 Then Exit Sub
            RowsBusqueda = Dt.Select("Clave = " & Articulo)
            If Row1("Tipo") = "B" Then
                If EspecieB <> 0 Then
                    If EspecieB <> RowsBusqueda(0).Item("Especie") Then Borrar = True
                End If
                If VariedadB <> 0 Then
                    If VariedadB <> RowsBusqueda(0).Item("Variedad") Then Borrar = True
                End If
                If EnvaseB <> 0 Then
                    If EnvaseB <> RowsBusqueda(0).Item("Envase") Then Borrar = True
                End If
            End If
            Row1("NombreArticulo") = RowsBusqueda(0).Item("Nombre")
            '
            If Borrar Then
                Row1.Delete()
            End If
        Next

        Dim View As New DataView
        View = DtRepro.DefaultView
        View.Sort = "Fecha,Clave"

        Dim Row As DataRowView
        Dim ReprocesoAnt As Decimal = 0
        Dim FilaInicial As Integer = 0
        Dim FilaFinalBaja As Integer = 0

        For Each Row In View
            If Row("Tipo") = "B" Then
                If ReprocesoAnt <> Row("Clave") Then
                    If ReprocesoAnt <> 0 Then             'Lista las Altas.
                        RowsBusqueda = DtRepro.Select("Tipo = 'A' AND Clave = " & Row("Clave"))
                        RowExxel = FilaInicial - 1
                        For I As Integer = 0 To RowsBusqueda.Length - 1
                            RowExxel = RowExxel + 1
                            exHoja.Cells.Item(RowExxel, 11) = RowsBusqueda(I).Item("NombreArticulo")
                            exHoja.Cells.Item(RowExxel, 12) = RowsBusqueda(I).Item("KilosXunidad")
                            exHoja.Cells.Item(RowExxel, 13) = RowsBusqueda(I).Item("Cantidad")
                        Next
                        If RowExxel < FilaFinalBaja Then RowExxel = FilaFinalBaja
                    End If
                    'Lista Cabeza del Reproceso.
                    If Row("operacion") = 2 Then exHoja.Cells.Item(RowExxel + 1, 1) = "X"
                    exHoja.Cells.Item(RowExxel + 1, 2) = Format(Row("Fecha"), "MM/dd/yyyy")
                    exHoja.Cells.Item(RowExxel + 1, 3) = Row("Clave")
                    exHoja.Cells.Item(RowExxel + 1, 4) = NombreDeposito(Row("Deposito"))
                    If Row("Estado") = 1 Then exHoja.Cells.Item(RowExxel + 1, 5) = "Activo"
                    If Row("Estado") = 3 Then exHoja.Cells.Item(RowExxel + 1, 5) = "Baja"
                    ReprocesoAnt = Row("Clave")
                    FilaInicial = RowExxel + 1
                End If
                RowExxel = RowExxel + 1
                exHoja.Cells.Item(RowExxel, 6) = NombreProveedor(Row("Proveedor"))
                exHoja.Cells.Item(RowExxel, 7) = Row("NombreArticulo")
                exHoja.Cells.Item(RowExxel, 8) = Row("KilosXunidad")
                exHoja.Cells.Item(RowExxel, 9) = Row("Cantidad")
                exHoja.Cells.Item(RowExxel, 10) = Row("Merma")
                FilaFinalBaja = RowExxel
            End If
        Next
        RowsBusqueda = DtRepro.Select("Tipo = 'A' AND Clave = " & Row("Clave"))
        RowExxel = FilaInicial - 1
        For I As Integer = 0 To RowsBusqueda.Length - 1
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 11) = RowsBusqueda(I).Item("NombreArticulo")
            exHoja.Cells.Item(RowExxel, 12) = RowsBusqueda(I).Item("KilosXunidad")
            exHoja.Cells.Item(RowExxel, 13) = RowsBusqueda(I).Item("Cantidad")
        Next

        Dt.Dispose()
        DtRepro.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 1 - 1) & InicioDatos
        BB = Chr(65 + 1 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 12 - 1) & InicioDatos
        BB = Chr(65 + 12 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 13 - 1) & InicioDatos
        BB = Chr(65 + 13 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 14) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Informe Reprocesos Desde el : " & Format(Desde, "dd/MM/yyyy") & "  Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeComprobantesDeUnCosteo(ByVal EsInsumos As Boolean, ByVal Negocio As Integer, ByVal Costeo As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Estado As Integer, ByVal Abierto As Boolean, ByVal Cerrado As Boolean, ByVal NombreCosteo As String)

        Dim DtConceptos As New DataTable
        If Not Tablas.Read("SELECT Nombre,Clave FROM Tablas WHERE Tipo = 29;", Conexion, DtConceptos) Then Exit Sub

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Proveedor"
        exHoja.Cells.Item(RowExxel, 3) = ""
        exHoja.Cells.Item(RowExxel, 4) = "Comprobante"
        exHoja.Cells.Item(RowExxel, 5) = "Fecha Cont."
        exHoja.Cells.Item(RowExxel, 6) = "Estado"
        exHoja.Cells.Item(RowExxel, 7) = "Monto S/IVA."
        exHoja.Cells.Item(RowExxel, 8) = "IVA"
        exHoja.Cells.Item(RowExxel, 9) = "Perc./Ret."
        exHoja.Cells.Item(RowExxel, 10) = "Seña"
        exHoja.Cells.Item(RowExxel, 11) = "Total"
        exHoja.Cells.Item(RowExxel, 12) = "Concepto Gasto"
        exHoja.Cells.Item(RowExxel, 13) = "Cuenta"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim DtFormaPago As DataTable
        DtFormaPago = ArmaConceptosPagoProveedores(False, 10)

        Dim SqlFechaContable As String = ""
        SqlFechaContable = " AND FechaContable >='" & Format(Desde, "yyyyMMdd") & "' AND FechaContable < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlFecha As String = ""
        SqlFecha = " AND Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlFechaContableInt As String
        SqlFechaContableInt = " AND IntFecha >=" & Format(Desde, "yyyyMMdd") & " AND IntFecha <=" & Format(Hasta, "yyyyMMdd") & ""

        Dim SqlEstado As String = ""
        If Estado <> 0 Then SqlEstado = " AND Estado = " & Estado
        Dim SqlEsInsumos As String = ""
        If EsInsumos Then
            SqlEsInsumos = " AND EsInsumos = 1"
        Else : SqlEsInsumos = " AND EsInsumos = 0"
        End If

        Dim SqlB As String = "SELECT 1 AS Operacion,2 AS Tipo,Factura As Comprobante,ReciboOficial,Estado,FechaContable,Rel,NRel,Cambio,Proveedor,0 As Importe,ConceptoGasto,0 Debito FROM FacturasProveedorCabeza WHERE Costeo = " & Costeo & SqlEsInsumos & SqlFechaContable & SqlEstado & _
                             " UNION ALL " & _
                             "SELECT 1 AS Operacion,TipoNota AS Tipo,Nota As Comprobante,Nota As ReciboOficial,Estado,Fecha As FechaContable,0 AS Rel,0 AS NRel,Cambio,Emisor AS Proveedor,Importe,0 AS ConceptoGasto,0 Debito FROM RecibosCabeza WHERE (TipoNota = 6 OR TipoNota = 8 or TipoNota = 13006 OR TipoNota = 13008) AND Costeo = " & Costeo & SqlFecha & SqlEstado & _
                             " UNION ALL " & _
                             "SELECT 1 AS Operacion,TipoNota AS Tipo,Nota As Comprobante,Nota AS ReciboOficial,Estado,FechaContable,0 AS Rel,0 AS NRel,Cambio,Emisor AS Proveedor,Importe,0 AS ConceptoGasto,0 Debito FROM RecibosCabeza WHERE (TipoNota = 500 OR TipoNota = 700) AND Costeo = " & Costeo & SqlFechaContable & SqlEstado & _
                             " UNION ALL " & _
                             "SELECT 1 AS Operacion,60000 AS Tipo,Consumo As Comprobante,Consumo AS ReciboOficial,Estado,Fecha AS FechaContable,0 AS Rel,0 AS NRel,0 AS Cambio,0 AS Proveedor,0 AS Importe,0 AS ConceptoGasto,0 Debito FROM ConsumosCabeza WHERE Costeo = " & Costeo & SqlFecha & SqlEstado & _
                             " UNION ALL " & _
                             "SELECT 1 AS Operacion,61000 AS Tipo,Consumo As Comprobante,Consumo AS ReciboOficial,Estado,Fecha AS FechaContable,0 AS Rel,0 AS NRel,0 AS Cambio,0 AS Proveedor, Importe,0 AS ConceptoGasto,0 Debito FROM ConsumosPTCabeza WHERE Costeo = " & Costeo & SqlFecha & SqlEstado & _
                             " UNION ALL " & _
                             "SELECT 1 AS Operacion,99000 AS Tipo,Asiento As Comprobante,Asiento AS ReciboOficial,Estado,CONVERT (DateTime,Convert(Char(8),intFecha)) as FechaContable,0 AS Rel,0 AS NRel,0 AS Cambio,0 AS Proveedor, 0 AS Importe,0 AS ConceptoGasto,Debito FROM AsientosCabeza WHERE Costeo = " & Costeo & SqlFechaContableInt & SqlEstado & ";"



        Dim SqlN As String = "SELECT 2 AS Operacion,2 AS Tipo,Factura As Comprobante,ReciboOficial,Estado,FechaContable,Rel,NRel,Cambio,Proveedor,0 As Importe,ConceptoGasto,0 Debito FROM FacturasProveedorCabeza WHERE Costeo = " & Costeo & SqlEsInsumos & SqlFechaContable & SqlEstado & _
                             " UNION ALL " & _
                             "SELECT 2 AS Operacion,TipoNota AS Tipo,Nota As Comprobante,Nota As ReciboOficial,Estado,Fecha As FechaContable,0 AS Rel,0 AS NRel,Cambio,Emisor AS Proveedor,Importe,0 AS ConceptoGasto,0 Debito FROM RecibosCabeza WHERE (TipoNota = 6 OR TipoNota = 8 or TipoNota = 13006 OR TipoNota = 13008) AND Costeo = " & Costeo & SqlFecha & SqlEstado & _
                             " UNION ALL " & _
                             "SELECT 2 AS Operacion,TipoNota AS Tipo,Nota As Comprobante,ReciboOficial,Estado,FechaContable,0 AS Rel,0 AS NRel,Cambio,Emisor AS Proveedor,Importe,0 AS ConceptoGasto,0 Debito FROM RecibosCabeza WHERE (TipoNota = 500 OR TipoNota = 700) AND Costeo = " & Costeo & SqlFechaContable & SqlEstado & _
                             " UNION ALL " & _
                             "SELECT 2 AS Operacion,60000 AS Tipo,Consumo As Comprobante,Consumo AS ReciboOficial,Estado,Fecha AS FechaContable,0 AS Rel,0 AS NRel,0 AS Cambio,0 AS Proveedor,0 AS Importe,0 AS ConceptoGasto,0 Debito FROM ConsumosCabeza WHERE Costeo = " & Costeo & SqlFecha & SqlEstado & _
                             " UNION ALL " & _
                             "SELECT 2 AS Operacion,61000 AS Tipo,Consumo As Comprobante,Consumo AS ReciboOficial,Estado,Fecha AS FechaContable,0 AS Rel,0 AS NRel,0 AS Cambio,0 AS Proveedor, Importe,0 AS ConceptoGasto,0 AS Debito FROM ConsumosPTCabeza WHERE Costeo = " & Costeo & SqlFecha & SqlEstado & _
                             " UNION ALL " & _
                             "SELECT 2 AS Operacion,99000 AS Tipo,Asiento As Comprobante,Asiento AS ReciboOficial,Estado,CONVERT (DateTime,Convert(Char(8),intFecha)) as FechaContable,0 AS Rel,0 AS NRel,0 AS Cambio,0 AS Proveedor, 0 AS Importe,0 AS ConceptoGasto,Debito FROM AsientosCabeza WHERE Costeo = " & Costeo & SqlFechaContableInt & SqlEstado & ";"


        Dim Dt As New DataTable
        If Abierto Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Exit Sub
        End If
        If Cerrado Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Exit Sub
        End If

        Dim Cartel As String = ""

        Dim Importe As Decimal
        Dim ImporteIva As Decimal
        Dim ImporteRetPer As Decimal
        Dim ImporteSenia As Decimal
        '
        Dim TImporte As Decimal = 0
        Dim TImporteIva As Decimal = 0
        Dim TImporteRetPer As Decimal = 0
        Dim TImporteSenia As Decimal = 0
        '
        Dim NetoGrabado As Decimal
        Dim NetoNoGrabado As Decimal

        Dim RowsBusqueda() As DataRow

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "FechaContable,Comprobante"

        For Each Row As DataRowView In View
            Importe = 0
            ImporteIva = 0
            ImporteRetPer = 0
            ImporteSenia = 0
            NetoGrabado = 0
            NetoNoGrabado = 0
            Select Case Row("Tipo")
                Case 2
                    HallaImportesFacturaProveedor(Row("Operacion"), Row("Comprobante"), Row("Cambio"), DtFormaPago, NetoGrabado, NetoNoGrabado, ImporteIva, ImporteRetPer, ImporteSenia)
                    Cartel = "Factura Proveedor"
                Case 8, 13008, 500, 6, 13006, 700
                    HallaTotalesImportesNotasDebitoCredito(Row("Operacion"), Row("Tipo"), Row("Comprobante"), NetoGrabado, NetoNoGrabado, ImporteRetPer, ImporteIva, Row("Cambio"))
                    If Row("Tipo") = 6 Or Row("Tipo") = 13006 Or Row("Tipo") = 700 Then
                        NetoGrabado = -NetoGrabado
                        NetoNoGrabado = -NetoNoGrabado
                        ImporteRetPer = -ImporteRetPer
                        ImporteIva = -ImporteIva
                    End If
                    Select Case Row("Tipo")
                        Case 6, 13006
                            Cartel = "Nota Debito"
                        Case 700
                            Cartel = "Nota Crédito del Proveedor"
                        Case 8, 13008
                            Cartel = "Nota Crédito"
                        Case 500
                            Cartel = "Nota Debito del Proveedor"
                    End Select
                Case 60000
                    Cartel = "Consumos Insumos"
                    HallaImportesConsumo(Row("Operacion"), Row("Comprobante"), NetoGrabado, ImporteIva, Row("Cambio"))
                Case 61000
                    Cartel = "Consumos Prod.Terminados"
                    HallaImportesConsumoTerminados(Row("Operacion"), Row("Comprobante"), NetoGrabado, ImporteIva, Row("Cambio"))
                Case 99000
                    Cartel = "Asientos Manuales"
                    HallaImportesAsientosManuales(Row("Operacion"), Row("Comprobante"), NetoGrabado, ImporteIva, Row("Cambio"), Row("Debito"))
            End Select
            '
            Importe = NetoGrabado + NetoNoGrabado
            TImporte = TImporte + Importe
            TImporteIva = TImporteIva + ImporteIva
            TImporteSenia = TImporteSenia + ImporteSenia
            TImporteRetPer = TImporteRetPer + ImporteRetPer
            '
            RowExxel = RowExxel + 1
            If Row("operacion") = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
            exHoja.Cells.Item(RowExxel, 2) = NombreProveedor(Row("Proveedor"))
            exHoja.Cells.Item(RowExxel, 3) = Cartel
            exHoja.Cells.Item(RowExxel, 4) = NumeroEditado(Row("ReciboOficial"))
            If IsDate(Row("FechaContable")) Then
                exHoja.Cells.Item(RowExxel, 5) = Format(Row("FechaContable"), "MM/dd/yyyy")
            Else
                exHoja.Cells.Item(RowExxel, 5) = Row("FechaContable").ToString.Substring(6, 2) & "/" & Row("FechaContable").ToString.Substring(4, 2) & "/" & Row("FechaContable").ToString.Substring(0, 4)
            End If
            If Row("Estado") = 1 Then exHoja.Cells.Item(RowExxel, 6) = "Activo"
            If Row("Estado") = 3 Then exHoja.Cells.Item(RowExxel, 6) = "Anulado"
            exHoja.Cells.Item(RowExxel, 7) = Importe
            exHoja.Cells.Item(RowExxel, 8) = ImporteIva
            exHoja.Cells.Item(RowExxel, 9) = ImporteRetPer
            exHoja.Cells.Item(RowExxel, 10) = ImporteSenia
            exHoja.Cells.Item(RowExxel, 11) = Importe + ImporteIva + ImporteRetPer + ImporteSenia
            RowsBusqueda = DtConceptos.Select("Clave = " & Row("ConceptoGasto"))
            If Row("ConceptoGasto") <> 0 Then exHoja.Cells.Item(RowExxel, 12) = RowsBusqueda(0).Item("Nombre")
            exHoja.Cells.Item(RowExxel, 13) = HallaCuentas(Row("Tipo"), Row("Comprobante"), Row("operacion"))
        Next
        '
        RowExxel = RowExxel + 2
        exHoja.Cells.Item(RowExxel, 7) = TImporte
        exHoja.Cells.Item(RowExxel, 8) = TImporteIva
        exHoja.Cells.Item(RowExxel, 9) = TImporteRetPer
        exHoja.Cells.Item(RowExxel, 10) = TImporteSenia
        exHoja.Cells.Item(RowExxel, 11) = TImporte + TImporteIva + TImporteRetPer + TImporteSenia

        Dt.Dispose()
        DtFormaPago.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        If Not PermisoTotal Then exHoja.Columns(9).delete()
        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 14) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Informe Facturas del costeo " & NombreCosteo & " Desde el : " & Format(Desde, "dd/MM/yyyy") & "  Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeDocumentosNoImputados(ByVal TipoCtaCte As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Abierto As Boolean, ByVal Cerrado As Boolean, ByVal SqlB As String, ByVal SqlN As String, ByVal DtEmisores As DataTable, ByVal Moneda As Integer, ByVal DtTipos As DataTable)

        Dim Dt As New DataTable
        If Abierto Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Exit Sub
        End If
        If Cerrado Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Exit Sub
        End If

        Dim PView As New DataView
        PView = Dt.DefaultView
        If TipoCtaCte = 1 Then
            PView.Sort = "Emisor,Fecha,Comprobante"
        End If
        If TipoCtaCte = 2 Then
            PView.Sort = "Legajo,Fecha,Comprobante"
        End If
        If TipoCtaCte = 3 Then
            PView.Sort = "Proveedor,Fecha,Comprobante"
        End If

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Cuenta"
        exHoja.Cells.Item(RowExxel, 3) = "Fecha"
        exHoja.Cells.Item(RowExxel, 4) = "    "
        exHoja.Cells.Item(RowExxel, 5) = "Comprobante"
        exHoja.Cells.Item(RowExxel, 6) = "Recibo Oficial"
        exHoja.Cells.Item(RowExxel, 7) = "Importe"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Total As Double = 0
        Dim RowsBusqueda() As DataRow

        If TipoCtaCte = 1 Then
            For Each Row As DataRowView In PView
                If DiferenciaDias(Row("Fecha"), Hasta) >= 0 And DiferenciaDias(Row("Fecha"), Desde) <= 0 And Row("Saldo") <> 0 And Row("Estado") = 1 And Moneda = Row("Moneda") Then
                    Select Case Row("Tipo")
                        Case 7, 50, 60, 6, 600, 700
                            RowExxel = RowExxel + 1
                            If Row("Operacion") = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
                            RowsBusqueda = DtEmisores.Select("Clave = " & Row("Emisor"))
                            exHoja.Cells.Item(RowExxel, 2) = RowsBusqueda(0).Item("Nombre")
                            If Row("ReciboOficial") <> 0 Then exHoja.Cells.Item(RowExxel, 6) = NumeroEditado(Row("ReciboOficial"))
                            exHoja.Cells.Item(RowExxel, 3) = Format(Row("Fecha"), "MM/dd/yyyy")
                            RowsBusqueda = DtTipos.Select("Codigo = " & Row("Tipo"))
                            exHoja.Cells.Item(RowExxel, 4) = RowsBusqueda(0).Item("Nombre")
                            exHoja.Cells.Item(RowExxel, 5) = NumeroEditado(Row("Comprobante"))
                            exHoja.Cells.Item(RowExxel, 7) = Row("Saldo")
                            Total = Total + Row("Saldo")
                    End Select
                End If
            Next
        End If
        If TipoCtaCte = 2 Then
            For Each Row As DataRowView In PView
                If DiferenciaDias(Row("Fecha"), Hasta) >= 0 And DiferenciaDias(Row("Fecha"), Desde) <= 0 And Row("Saldo") <> 0 And Row("Estado") = 1 Then
                    Select Case Row("Tipo")
                        Case 4010
                            RowExxel = RowExxel + 1
                            If Row("Operacion") = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
                            RowsBusqueda = DtEmisores.Select("Legajo = " & Row("Legajo"))
                            exHoja.Cells.Item(RowExxel, 2) = RowsBusqueda(0).Item("Con")
                            exHoja.Cells.Item(RowExxel, 3) = Format(Row("Fecha"), "MM/dd/yyyy")
                            RowsBusqueda = DtTipos.Select("Clave = " & Row("Tipo"))
                            exHoja.Cells.Item(RowExxel, 4) = RowsBusqueda(0).Item("Nombre")
                            exHoja.Cells.Item(RowExxel, 5) = NumeroEditado(Row("Comprobante"))
                            exHoja.Cells.Item(RowExxel, 7) = Row("Saldo")
                            Total = Total + Row("Saldo")
                    End Select
                End If
            Next
        End If
        If TipoCtaCte = 3 Then
            For Each Row As DataRowView In PView
                If DiferenciaDias(Row("Fecha"), Hasta) >= 0 And DiferenciaDias(Row("Fecha"), Desde) <= 0 And Row("Saldo") <> 0 And Row("Estado") = 1 Then
                    Select Case Row("Tipo")
                        Case 5010
                            RowExxel = RowExxel + 1
                            If Row("Operacion") = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
                            RowsBusqueda = DtEmisores.Select("Clave = " & Row("Proveedor"))
                            exHoja.Cells.Item(RowExxel, 2) = RowsBusqueda(0).Item("Nombre")
                            exHoja.Cells.Item(RowExxel, 3) = Format(Row("Fecha"), "MM/dd/yyyy")
                            RowsBusqueda = DtTipos.Select("Clave = " & Row("Tipo"))
                            exHoja.Cells.Item(RowExxel, 4) = RowsBusqueda(0).Item("Nombre")
                            exHoja.Cells.Item(RowExxel, 5) = NumeroEditado(Row("Comprobante"))
                            exHoja.Cells.Item(RowExxel, 7) = Row("Saldo")
                            Total = Total + Row("Saldo")
                    End Select
                End If
            Next
        End If

        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 7) = FormatNumber(Total, GDecimales)

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String

        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"


        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 9) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Desde el " & Format(Desde, "dd/MM/yyyy") & " Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeDocumentosFondoFijoNoImputados(ByVal FondoFijo As Integer, ByVal Numero As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Abierto As Boolean, ByVal Cerrado As Boolean, ByVal DtEmisores As DataTable, ByVal Moneda As Integer, ByVal DtTipos As DataTable)

        Dim SqlB As String
        Dim SqlN As String

        Dim StrFondoFijo As String
        If FondoFijo <> 0 Then
            StrFondoFijo = "FondoFijo = " & FondoFijo
        Else
            StrFondoFijo = "FondoFijo LIKE '%'"
        End If

        Dim StrFondoFijoR As String
        If FondoFijo <> 0 Then
            StrFondoFijoR = "Emisor = " & FondoFijo
        Else
            StrFondoFijoR = "Emisor LIKE '%'"
        End If

        Dim StrNumeroFondoFijo As String = ""
        If Numero <> 0 Then
            StrNumeroFondoFijo = " AND Numero = " & Numero
        End If

        Dim StrNumeroFondoFijoR As String = ""
        If Numero <> 0 Then
            If Numero <> 0 Then
                StrNumeroFondoFijoR = " AND NumeroFondoFijo = " & Numero
            End If
        End If

        SqlB = "SELECT 1 AS Operacion,FondoFijo,Numero,Fecha,0 As Tipo,0 AS Comprobante,Saldo,1 AS Estado,Cerrado FROM FondosFijos WHERE " & StrFondoFijo & StrNumeroFondoFijo & _
               " UNION ALL " & _
              "SELECT 1 As Operacion,Emisor AS FondoFijo,NumeroFondoFijo as Numero,Fecha,7004 AS Tipo,Nota As Comprobante,Saldo,Estado,0 As Cerrado FROM RecibosCabeza WHERE NumeroFondoFijo <> 0 AND " & StrFondoFijoR & StrNumeroFondoFijoR & ";"

        SqlN = "SELECT 2 AS Operacion,FondoFijo,Numero,Fecha,0 As Tipo,0 AS Comprobante,Saldo,1 AS Estado,Cerrado FROM FondosFijos WHERE " & StrFondoFijo & StrNumeroFondoFijo & _
               " UNION ALL " & _
               "SELECT 2 As Operacion,Emisor AS FondoFijo,NumeroFondoFijo as Numero,Fecha,7004 AS Tipo,Nota As Comprobante,Saldo,Estado,0 As Cerrado FROM RecibosCabeza WHERE NumeroFondoFijo <> 0 AND " & StrFondoFijoR & StrNumeroFondoFijoR & ";"


        Dim Dt As New DataTable
        If Abierto Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Exit Sub
        End If
        If Cerrado Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Exit Sub
        End If

        Dim PView As New DataView
        PView = Dt.DefaultView
        PView.Sort = "FondoFijo,Fecha,Comprobante"

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Cuenta"
        exHoja.Cells.Item(RowExxel, 3) = "Numero"
        exHoja.Cells.Item(RowExxel, 4) = "Fecha"
        exHoja.Cells.Item(RowExxel, 5) = "    "
        exHoja.Cells.Item(RowExxel, 6) = "Comprobante"
        exHoja.Cells.Item(RowExxel, 7) = "Importe"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Total As Double = 0
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRowView In PView
            If DiferenciaDias(Row("Fecha"), Hasta) >= 0 And DiferenciaDias(Row("Fecha"), Desde) <= 0 And Row("Saldo") <> 0 And Row("Estado") = 1 Then
                Select Case Row("Tipo")
                    Case 7004
                        RowExxel = RowExxel + 1
                        If Row("Operacion") = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
                        RowsBusqueda = DtEmisores.Select("Clave = " & Row("FondoFijo"))
                        exHoja.Cells.Item(RowExxel, 2) = RowsBusqueda(0).Item("Nombre")
                        exHoja.Cells.Item(RowExxel, 3) = Row("Numero")
                        exHoja.Cells.Item(RowExxel, 4) = Format(Row("Fecha"), "MM/dd/yyyy")
                        RowsBusqueda = DtTipos.Select("Clave = " & Row("Tipo"))
                        exHoja.Cells.Item(RowExxel, 5) = RowsBusqueda(0).Item("Nombre")
                        exHoja.Cells.Item(RowExxel, 6) = NumeroEditado(Row("Comprobante"))
                        exHoja.Cells.Item(RowExxel, 7) = Row("Saldo")
                        Total = Total + Row("Saldo")
                    Case 0
                        RowExxel = RowExxel + 1
                        If Row("Operacion") = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
                        RowsBusqueda = DtEmisores.Select("Clave = " & Row("FondoFijo"))
                        exHoja.Cells.Item(RowExxel, 2) = RowsBusqueda(0).Item("Nombre")
                        exHoja.Cells.Item(RowExxel, 3) = Row("Numero")
                        exHoja.Cells.Item(RowExxel, 4) = Format(Row("Fecha"), "MM/dd/yyyy")
                        RowsBusqueda = DtTipos.Select("Clave = " & Row("Tipo"))
                        exHoja.Cells.Item(RowExxel, 5) = "Apertura Fondo Fijo"
                        exHoja.Cells.Item(RowExxel, 6) = ""
                        If Not Row("Cerrado") Then
                            exHoja.Cells.Item(RowExxel, 7) = Row("Saldo")
                            Total = Total + Row("Saldo")
                        End If
                End Select
            End If
        Next

        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 7) = FormatNumber(Total, GDecimales)

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String

        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0"

        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 9) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Desde el " & Format(Desde, "dd/MM/yyyy") & " Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeComprasVentasPorLotes(ByVal Proveedor As Integer, ByVal Nombre As String, ByVal Desde As Date, ByVal Hasta As Date, ByVal Especie As Integer, ByVal ConStock As Boolean, ByVal SinStock As Boolean, ByVal Facturados As Boolean, ByVal Pendientes As Boolean)

        Dim Sql As String = ""

        If Especie <> 0 Then
            If Sql = "" Then
                Sql = "Especie = " & Especie
            Else : Sql = "AND Especie = " & Especie
            End If
        End If

        If Sql <> "" Then Sql = "WHERE " & Sql

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Articulos " & Sql & ";", Conexion, Dt) Then Exit Sub

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim SqlFecha As String = ""
        SqlFecha = "AND L.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND L.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "

        Dim SqlFacturados As String = ""
        If Facturados Then SqlFacturados = "AND L.Liquidado <> 0 "
        Dim SqlPendientes As String = ""
        If Pendientes Then SqlFacturados = "AND L.Liquidado = 0 "
        If Facturados And Pendientes Then SqlFacturados = ""
        Dim SqlProveedor As String = ""
        If Proveedor <> 0 Then SqlProveedor = "AND L.Proveedor = " & Proveedor
        Dim SqlStock As String = ""
        If ConStock Then SqlStock = "AND L.Stock <> 0 "
        If SinStock Then SqlStock = "AND L.Stock = 0 "
        If ConStock And SinStock Then SqlStock = ""

        Dim DtLotes As New DataTable
        If Not Tablas.Read("SELECT 1 As Operacion,L.Lote,L.Secuencia,L.Cantidad,L.Baja,L.Articulo,L.Proveedor,L.Liquidado,L.Fecha,L.Stock,C.Remito,C.Guia FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen " & SqlFecha & SqlFacturados & SqlProveedor & SqlStock & ";", Conexion, DtLotes) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read("SELECT 2 As Operacion,L.Lote,L.Secuencia,L.Cantidad,L.Baja,L.Articulo,L.Proveedor,L.Liquidado,L.Fecha,L.Stock,C.Remito,C.Guia FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen " & SqlFecha & SqlFacturados & SqlProveedor & SqlStock & ";", ConexionN, DtLotes) Then Exit Sub
        End If

        Dim View As New DataView
        View = DtLotes.DefaultView
        View.Sort = "Lote,Secuencia"

        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Lote          "
        exHoja.Cells.Item(RowExxel, 3) = "Fecha  "
        exHoja.Cells.Item(RowExxel, 4) = "Articulo       "
        exHoja.Cells.Item(RowExxel, 5) = "Proveedor       "
        exHoja.Cells.Item(RowExxel, 6) = "Cant. Inicial"
        exHoja.Cells.Item(RowExxel, 7) = "Devolución"
        exHoja.Cells.Item(RowExxel, 8) = "Cant. Neta"
        exHoja.Cells.Item(RowExxel, 9) = "Stock  "
        exHoja.Cells.Item(RowExxel, 10) = "Remito/Guia"
        exHoja.Cells.Item(RowExxel, 11) = " "
        exHoja.Cells.Item(RowExxel, 12) = "Fact./Liquidación"
        exHoja.Cells.Item(RowExxel, 13) = "Monto S/IVA."
        exHoja.Cells.Item(RowExxel, 14) = "Comision/Descarga"
        exHoja.Cells.Item(RowExxel, 15) = "Total (2)"
        exHoja.Cells.Item(RowExxel, 16) = "Total"
        exHoja.Cells.Item(RowExxel, 17) = " "
        exHoja.Cells.Item(RowExxel, 18) = "Monto S/IVA."
        exHoja.Cells.Item(RowExxel, 19) = "Comision/Descarga"
        exHoja.Cells.Item(RowExxel, 20) = "Total (2)"
        exHoja.Cells.Item(RowExxel, 21) = "Total"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim NetoConIva As Decimal = 0
        Dim NetoSinIva As Decimal = 0
        Dim ConexionFactura As String = ""
        Dim Marca As String
        Dim RowsBusqueda() As DataRow
        Dim LiquidacionB As Double
        Dim LiquidacionN As Double
        Dim FacturaB As Double
        Dim FacturaN As Double
        Dim ReciboOficial As Double
        Dim Tipo As String = ""

        Dim MontoBSinIva As Decimal, NetoN As Decimal, ComisionDescargaB As Decimal
        Dim MontoBSinIvaT As Decimal = 0, NetoNT As Decimal = 0, ComisionDescargaBT As Decimal = 0
        Dim MontoFacturaBSinIva As Decimal, NetoFacturaN As Decimal, ComisionDescargaFacturaB As Decimal
        Dim MontoFacturaBSinIvaW As Decimal, NetoFacturaNW As Decimal, ComisionDescargaFacturaBW As Decimal
        Dim MontoFacturaBSinIvaT As Decimal = 0, NetoFacturaNT As Decimal = 0, ComisionDescargaFacturaBT As Decimal = 0

        For Each Row As DataRowView In View
            RowsBusqueda = Dt.Select("Clave = " & Row("Articulo"))
            If RowsBusqueda.Length <> 0 Then
                RowExxel = RowExxel + 1
                If Row("Operacion") = 1 Then
                    Marca = ""
                Else
                    Marca = "X"
                End If
                exHoja.Cells.Item(RowExxel, 1) = Marca
                exHoja.Cells.Item(RowExxel, 2) = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
                exHoja.Cells.Item(RowExxel, 3) = Format(Row("Fecha"), "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel, 4) = RowsBusqueda(0).Item("Nombre")
                exHoja.Cells.Item(RowExxel, 5) = NombreProveedor(Row("Proveedor"))
                exHoja.Cells.Item(RowExxel, 6) = Row("Cantidad")
                exHoja.Cells.Item(RowExxel, 7) = Row("Baja")
                exHoja.Cells.Item(RowExxel, 8) = Row("Cantidad") - Row("Baja")
                exHoja.Cells.Item(RowExxel, 9) = Row("Stock")
                exHoja.Cells.Item(RowExxel, 10) = ""
                If Row("Remito") <> 0 Then
                    exHoja.Cells.Item(RowExxel, 10) = NumeroEditado(Row("Remito"))
                Else
                    exHoja.Cells.Item(RowExxel, 10) = Row("Guia")
                End If
                '
                ComisionDescargaB = 0 : MontoBSinIva = 0 : NetoN = 0
                LiquidacionB = 0 : LiquidacionN = 0 : FacturaB = 0 : FacturaN = 0 : ReciboOficial = 0
                Dim TipoNota As Integer, NotaDebito As Double
                '
                If Row("Liquidado") <> 0 Then
                    If Not HallaFacturasLote(Row("Lote"), Row("Secuencia"), FacturaB, FacturaN, ReciboOficial, TipoNota, NotaDebito) Then Dt.Dispose() : DtLotes.Dispose() : Exit Sub
                    If FacturaB = 0 And FacturaN = 0 Then
                        If Not HallaLiquidacionLote(Row("Lote"), Row("Secuencia"), LiquidacionB, LiquidacionN) Then Dt.Dispose() : DtLotes.Dispose() : Exit Sub
                    End If
                    If FacturaB <> 0 Or FacturaN <> 0 Then
                        If Not HallaImporteFacturaBlancoNegro(Row("Lote"), Row("Secuencia"), FacturaB, FacturaN, MontoBSinIva, NetoN, TipoNota, NotaDebito) Then Dt.Dispose() : DtLotes.Dispose() : Exit Sub
                    End If
                    If LiquidacionB <> 0 Or LiquidacionN <> 0 Then
                        If Not HallaImporteLiquidacionBlancoNegro(Row("Lote"), Row("Secuencia"), LiquidacionB, LiquidacionN, MontoBSinIva, NetoN, ComisionDescargaB) Then Dt.Dispose() : DtLotes.Dispose() : Exit Sub
                    End If
                    If FacturaB = 0 And FacturaN <> 0 Then Marca = "X"
                    If LiquidacionB = 0 And LiquidacionN <> 0 Then Marca = "X"
                    exHoja.Cells.Item(RowExxel, 11) = Marca
                    If FacturaB <> 0 Or FacturaN <> 0 Then
                        Tipo = "Fac."
                        exHoja.Cells.Item(RowExxel, 12) = Tipo & " " & NumeroEditado(ReciboOficial)
                    End If
                    If LiquidacionB <> 0 Then
                        Tipo = "Liq."
                        exHoja.Cells.Item(RowExxel, 12) = Tipo & " " & NumeroEditado(LiquidacionB)
                    End If
                    If LiquidacionB = 0 And LiquidacionN <> 0 Then
                        Tipo = "Liq."
                        exHoja.Cells.Item(RowExxel, 12) = Tipo & " " & NumeroEditado(LiquidacionN)
                    End If
                    Dim ImporteConIvaRecibos As Decimal = 0
                    Dim ImporteSinIvaRecibos As Decimal = 0
                    GConListaDeCostos = True
                    If Not HallaImportesLotesRecibos(Row("Lote"), Row("Secuencia"), ImporteConIvaRecibos, ImporteSinIvaRecibos) Then Dt.Dispose() : DtLotes.Dispose() : Exit Sub
                    For Each Item As ItemCostosAsignados In GListaLotesDeRecibos
                        Select Case Item.Operacion
                            Case 1
                                MontoBSinIva = MontoBSinIva + Item.ImporteSinIva
                            Case 2
                                NetoN = NetoN + Item.ImporteSinIva
                        End Select
                    Next
                    GListaLotesDeRecibos = Nothing
                    '
                    exHoja.Cells.Item(RowExxel, 13) = MontoBSinIva
                    exHoja.Cells.Item(RowExxel, 14) = -ComisionDescargaB
                    exHoja.Cells.Item(RowExxel, 15) = NetoN
                    exHoja.Cells.Item(RowExxel, 16) = MontoBSinIva - ComisionDescargaB + NetoN
                    '
                    MontoBSinIvaT = MontoBSinIvaT + MontoBSinIva
                    ComisionDescargaBT = ComisionDescargaBT + ComisionDescargaB
                    NetoNT = NetoNT + NetoN
                End If
                '
                MontoFacturaBSinIva = 0 : NetoFacturaN = 0 : ComisionDescargaFacturaB = 0
                MontoFacturaBSinIvaW = 0 : NetoFacturaNW = 0 : ComisionDescargaFacturaBW = 0
                '
                If Not HallaTotalesPorFacturas(Row("Lote"), Row("Secuencia"), MontoFacturaBSinIva, NetoFacturaN) Then Dt.Dispose() : DtLotes.Dispose() : Exit Sub
                If Not HallaTotalesPorNVLP(Row("Lote"), Row("Secuencia"), MontoFacturaBSinIvaW, NetoFacturaNW, ComisionDescargaFacturaBW) Then Dt.Dispose() : DtLotes.Dispose() : Exit Sub
                MontoFacturaBSinIva = MontoFacturaBSinIva + MontoFacturaBSinIvaW
                ComisionDescargaFacturaB = ComisionDescargaFacturaB + ComisionDescargaFacturaBW
                NetoFacturaN = NetoFacturaN + NetoFacturaNW
                '
                exHoja.Cells.Item(RowExxel, 18) = MontoFacturaBSinIva
                exHoja.Cells.Item(RowExxel, 19) = -ComisionDescargaFacturaB
                exHoja.Cells.Item(RowExxel, 20) = NetoFacturaN
                exHoja.Cells.Item(RowExxel, 21) = MontoFacturaBSinIva + NetoFacturaN - ComisionDescargaFacturaB
                '
                MontoFacturaBSinIvaT = MontoFacturaBSinIvaT + MontoFacturaBSinIva
                ComisionDescargaFacturaBT = ComisionDescargaFacturaBT + ComisionDescargaFacturaB
                NetoFacturaNT = NetoFacturaNT + NetoFacturaN
            End If
        Next
        'Totales.
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 13) = MontoBSinIvaT
        exHoja.Cells.Item(RowExxel, 14) = -ComisionDescargaBT
        exHoja.Cells.Item(RowExxel, 15) = NetoNT
        exHoja.Cells.Item(RowExxel, 16) = MontoBSinIvaT - ComisionDescargaBT + NetoNT
        exHoja.Cells.Item(RowExxel, 18) = MontoFacturaBSinIvaT
        exHoja.Cells.Item(RowExxel, 19) = -ComisionDescargaFacturaBT
        exHoja.Cells.Item(RowExxel, 20) = NetoFacturaNT
        exHoja.Cells.Item(RowExxel, 21) = MontoFacturaBSinIvaT + NetoFacturaNT - ComisionDescargaFacturaBT

        Dt.Dispose()
        DtLotes.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 1 - 1) & InicioDatos
        BB = Chr(65 + 1 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        ' 
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 13 - 1) & InicioDatos
        BB = Chr(65 + 13 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 14 - 1) & InicioDatos
        BB = Chr(65 + 14 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 15 - 1) & InicioDatos
        BB = Chr(65 + 15 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 16 - 1) & InicioDatos
        BB = Chr(65 + 16 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 18 - 1) & InicioDatos
        BB = Chr(65 + 18 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 19 - 1) & InicioDatos
        BB = Chr(65 + 19 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 20 - 1) & InicioDatos
        BB = Chr(65 + 20 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 21 - 1) & InicioDatos
        BB = Chr(65 + 21 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Dibuja rectangulos.=------------------------------
        If InicioDatos < UltimaLinea - 1 Then
            AA = Chr(65 + 1 - 1) & InicioDatos
            BB = Chr(65 + 10 - 1) & UltimaLinea - 1
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
            AA = Chr(65 + 11 - 1) & InicioDatos
            BB = Chr(65 + 16 - 1) & UltimaLinea - 1
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
            AA = Chr(65 + 18 - 1) & InicioDatos
            BB = Chr(65 + 21 - 1) & UltimaLinea - 1
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
        End If
        '--------------------------------------------------

        If Not PermisoTotal Then exHoja.Columns(15).delete() : exHoja.Columns(19).delete()

        exHoja.Rows.Item(5).Font.Bold = True
        exHoja.Cells.Item(5, 12) = "        DETALLE DE COMPRA (Facturas Reventa, Liquidaciones)"
        exHoja.Cells.Item(5, 18) = "        DETALLE DE VENTA  (Facturas, NVLP)"

        'Muestra titulos.
        exHoja.Cells.Item(1, 23) = Format(Date.Now, "MM/dd/yyyy")
        If Nombre = "" Then
            exHoja.Cells.Item(2, 1) = "Gestión Compra/Venta de Lotes"
        Else
            exHoja.Cells.Item(2, 1) = "Gestión Compra/Venta de Lotes del Proveedor : " & Nombre
        End If

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeMermaPorArticulos(ByVal Proveedor As Integer, ByVal Nombre As String, ByVal Desde As Date, ByVal Hasta As Date, ByVal Especie As Integer)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim SqlFecha As String = ""
        SqlFecha = "AND C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "

        Dim SqlB1 As String = "SELECT 1 AS Operacion,B.Lote,B.Secuencia,C.Fecha,B.Merma FROM ReprocesoCabeza AS C INNER JOIN ReprocesoDetalleBaja AS B ON C.Clave = B.Clave WHERE C.Estado <> 3 AND B.Merma <> 0 " & SqlFecha & ";"
        Dim SqlN1 As String = "SELECT 2 AS Operacion,B.Lote,B.Secuencia,C.Fecha,B.Merma FROM ReprocesoCabeza AS C INNER JOIN ReprocesoDetalleBaja AS B ON C.Clave = B.Clave WHERE C.Estado <> 3 AND B.Merma <> 0 " & SqlFecha & ";"
        Dim DtReproceso As New DataTable
        If Not Tablas.Read(SqlB1, Conexion, DtReproceso) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN1, ConexionN, DtReproceso) Then Exit Sub
        End If
        '
        SqlB1 = "SELECT 1 AS Operacion,B.Lote,B.Secuencia,C.Fecha,B.Cantidad AS Merma FROM DescarteCabeza AS C INNER JOIN DescarteDetalle AS B ON C.Clave = B.Clave WHERE C.Estado <> 3 AND B.Cantidad <> 0 " & SqlFecha & ";"
        SqlN1 = "SELECT 2 AS Operacion,B.Lote,B.Secuencia,C.Fecha,B.Cantidad AS Merma FROM DescarteCabeza AS C INNER JOIN DescarteDetalle AS B ON C.Clave = B.Clave WHERE C.Estado <> 3 AND B.Cantidad <> 0 " & SqlFecha & ";"
        If Not Tablas.Read(SqlB1, Conexion, DtReproceso) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN1, ConexionN, DtReproceso) Then Exit Sub
        End If
        '
        Dim SqlB2 As String = "SELECT A.Operacion,A.Lote,A.Secuencia,C.Fecha,A.Cantidad FROM AsignacionLotes AS A INNER JOIN FacturasCabeza AS C ON A.Comprobante = C.Factura WHERE A.TipoComprobante = 2 " & SqlFecha & ";"
        Dim SqlN2 As String = "SELECT A.Operacion,A.Lote,A.Secuencia,C.Fecha,A.Cantidad FROM AsignacionLotes AS A INNER JOIN FacturasCabeza AS C ON A.Comprobante = C.Factura WHERE A.Rel = 0 AND A.TipoComprobante = 2 " & SqlFecha & ";"
        Dim DtFacturado As New DataTable
        If Not Tablas.Read(SqlB2, Conexion, DtFacturado) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN2, ConexionN, DtFacturado) Then Exit Sub
        End If
        '
        SqlB2 = "SELECT A.Operacion,A.Lote,A.Secuencia,C.Fecha,A.Cantidad FROM AsignacionLotes AS A INNER JOIN RemitosCabeza AS C ON A.Comprobante = C.Remito WHERE A.TipoComprobante = 1 " & SqlFecha & ";"
        SqlN2 = "SELECT A.Operacion,A.Lote,A.Secuencia,C.Fecha,A.Cantidad FROM AsignacionLotes AS A INNER JOIN RemitosCabeza AS C ON A.Comprobante = C.Remito WHERE A.TipoComprobante = 1 " & SqlFecha & ";"
        If Not Tablas.Read(SqlB2, Conexion, DtFacturado) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN2, ConexionN, DtFacturado) Then Exit Sub
        End If

        Dim DtLotes As New DataTable
        ArmaArchivoEstadisticasMermarPorLotes(DtLotes)
        For Each Row As DataRow In DtReproceso.Rows
            AcumulaEnMerma(DtLotes, Row("Lote"), Row("Secuencia"), Row("Merma"), 0, Proveedor, Especie, Row("Operacion"))
        Next
        For Each Row As DataRow In DtFacturado.Rows
            AcumulaEnMerma(DtLotes, Row("Lote"), Row("Secuencia"), 0, Row("Cantidad"), Proveedor, Especie, Row("Operacion"))
        Next

        If DtLotes.Rows.Count = 0 Then
            MsgBox("No Hay Datos.") : Exit Sub
        End If

        Dim View As New DataView
        View = DtLotes.DefaultView
        View.Sort = "Articulo"

        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Articulo          "
        exHoja.Cells.Item(RowExxel, 3) = "Proveedor       "
        exHoja.Cells.Item(RowExxel, 4) = "Ingresado(Kg)"
        exHoja.Cells.Item(RowExxel, 5) = "Merma y Descarte(Kg)"
        exHoja.Cells.Item(RowExxel, 6) = "Fact./Rem.(Kg)"
        exHoja.Cells.Item(RowExxel, 7) = "Merma/Ingreso(%)"
        exHoja.Cells.Item(RowExxel, 8) = "Fact./Ingreso(%)"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel
        Dim ArticuloAnt As Integer = View(0).Item("Articulo")
        Dim ProveedorAnt As Integer = View(0).Item("Proveedor")
        Dim IngresadoAnt As Integer
        Dim MermaAnt As Decimal
        Dim FacturadoAnt As Decimal

        For Each Row As DataRowView In View
            If Row("Articulo") <> ArticuloAnt Or Row("Proveedor") <> ProveedorAnt Then
                CorteInormeMerma(exHoja, RowExxel, ArticuloAnt, ProveedorAnt, IngresadoAnt, MermaAnt, FacturadoAnt)
                ArticuloAnt = Row("Articulo")
                ProveedorAnt = Row("Proveedor")
                MermaAnt = 0
                FacturadoAnt = 0
                IngresadoAnt = 0
            End If
            MermaAnt = MermaAnt + Row("Merma")
            FacturadoAnt = FacturadoAnt + Row("Facturado")
            IngresadoAnt = IngresadoAnt + Row("Ingresado")
        Next
        CorteInormeMerma(exHoja, RowExxel, ArticuloAnt, ProveedorAnt, IngresadoAnt, MermaAnt, FacturadoAnt)

        DtLotes.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        ' 
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Rows.Item(5).Font.Bold = True
        exHoja.Cells.Item(1, 7) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Merma, Descarte y Facturación Generada Desde el " & Format(Desde, "dd/MM/yyyy") & " Hasta el " & Format(Hasta, "dd/MM/yyyy")
        exHoja.Cells.Item(3, 1) = "Del Proveedore " & Nombre

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Private Sub CorteInormeMerma(ByVal exHoja As Microsoft.Office.Interop.Excel.Worksheet, ByRef RowExxel As Integer, ByVal Articulo As Integer, ByVal Proveedor As Integer, ByVal Ingresado As Decimal, ByVal Merma As Decimal, ByVal Facturado As Decimal)

        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 1) = ""
        exHoja.Cells.Item(RowExxel, 2) = NombreArticulo(Articulo)
        exHoja.Cells.Item(RowExxel, 3) = NombreProveedor(Proveedor)
        exHoja.Cells.Item(RowExxel, 4) = Ingresado
        exHoja.Cells.Item(RowExxel, 5) = Merma
        exHoja.Cells.Item(RowExxel, 6) = Facturado
        If Ingresado <> 0 Then
            exHoja.Cells.Item(RowExxel, 7) = (Merma * 100) / Ingresado
            exHoja.Cells.Item(RowExxel, 8) = (Facturado * 100) / Ingresado
        End If

    End Sub
    Public Sub InformePedidosEntreFechas(ByVal Cliente As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal ConSaldoPositivo As Boolean, ByVal Abierto As Boolean, ByVal Cerrado As Boolean, ByVal ConRepeticion As Boolean)

        Dim SqlFecha As String
        SqlFecha = "FechaEntregaDesde <='" & Format(Hasta, "yyyyMMdd") & "' AND FechaEntregaHasta >='" & Format(Desde, "yyyyMMdd") & "'"
        Dim SqlCerrado As String
        If Abierto Then SqlCerrado = "Cerrado = 0 "
        If Cerrado Then SqlCerrado = "Cerrado = 1 "
        If Abierto And Cerrado Then SqlCerrado = "(Cerrado = 0 OR Cerrado = 1) "

        Dim Sql As String = "SELECT Pedido,Sucursal,Cerrado FROM PedidosCabeza WHERE " & SqlCerrado & " AND Cliente = " & Cliente & " AND " & SqlFecha & ";"
        Dim Dt As New DataTable

        If Not Tablas.Read(Sql, Conexion, Dt) Then Exit Sub
        If Dt.Rows.Count = 0 Then
            MsgBox("No Existen Pedidos para esta Fecha y Cliente.")
            Dt.Dispose()
            Exit Sub
        End If

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer
        RowExxel = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Pedido,Sucursal"

        Dim UltimaColumnaUtilizada As Integer = 5
        Dim NombreSucursal As String = ""
        Dim NombreArticuloW As String = ""
        Dim DtDetallePedido As DataTable

        Dim SqlSaldo As String = ""
        If ConSaldoPositivo Then
            SqlSaldo = " AND Cantidad - Entregada > 0"
        End If

        For Each Row As DataRowView In View
            DtDetallePedido = New DataTable
            If Not Tablas.Read("SELECT Articulo,Cantidad,Entregada,(Cantidad - Entregada) AS Saldo,Precio FROM PedidosDetalle WHERE Pedido = " & Row("Pedido") & SqlSaldo & ";", Conexion, DtDetallePedido) Then Exit Sub
            If DtDetallePedido.Rows.Count <> 0 Then
                NombreSucursal = NombreSucursalCliente(Cliente, Row("Sucursal"))
                If NombreSucursal = "" Then NombreSucursal = "Central"
                RowExxel = RowExxel + 2
                ' exHoja.Cells.Item(RowExxel, 1) = "Pedido " & Row("Pedido") & "  " & NombreSucursal
                '  exHoja.Cells.Item(RowExxel, 3) = NombreSucursal
                exHoja.Rows.Item(RowExxel).Font.Bold = True
                exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3
                'RowExxel = RowExxel + 1
                exHoja.Cells.Item(RowExxel, 1) = " "
                exHoja.Cells.Item(RowExxel, 1) = "Pedido " & Row("Pedido") & "  " & NombreSucursal
                exHoja.Cells.Item(RowExxel, 2) = "Articulo         "
                exHoja.Cells.Item(RowExxel, 3) = "Precio    "
                exHoja.Cells.Item(RowExxel, 4) = "Pedido    "
                exHoja.Cells.Item(RowExxel, 5) = "Entregado "
                exHoja.Cells.Item(RowExxel, 6) = "Saldo     "
                Dim Cartel As String
                If Row("Cerrado") = 0 Then
                    Cartel = "Abierto"
                Else
                    Cartel = "Cerrado"
                End If
                exHoja.Cells.Item(RowExxel, 7) = Cartel
                exHoja.Rows.Item(RowExxel).Font.Bold = True
                exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3
                RowExxel = RowExxel + 1
                For Each Row1 As DataRow In DtDetallePedido.Rows
                    RowExxel = RowExxel + 1
                    If ConRepeticion Then
                        exHoja.Cells.Item(RowExxel, 1) = "Pedido " & Row("Pedido") & "  " & NombreSucursal
                    End If
                    exHoja.Cells.Item(RowExxel, 2) = NombreArticulo(Row1("Articulo"))
                    exHoja.Cells.Item(RowExxel, 3) = Row1("Precio")
                    exHoja.Cells.Item(RowExxel, 4) = Row1("Cantidad")
                    exHoja.Cells.Item(RowExxel, 5) = Row1("Entregada")
                    exHoja.Cells.Item(RowExxel, 6) = Row1("Saldo")
                Next
            End If
        Next

        DtDetallePedido.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        Dim AA As String
        Dim BB As String

        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        exHoja.Rows.Item(5).Font.Bold = True

        'Muestra titulos.
        exHoja.Rows.Item(1).Font.Bold = True
        exHoja.Rows.Item(2).Font.Bold = True
        exHoja.Cells.Item(1, 23) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Pedidos Para el Cliente " & NombreCliente(Cliente) & "  Desde " & Format(Desde, "dd/MM/yyyy") & " Hasta " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeResumenArticulosPedidosPorFecha(ByVal Cliente As Integer, ByVal Desde As Date, ByVal Hasta As Date)

        Dim SqlFecha As String
        SqlFecha = "FechaEntregaDesde >='" & Format(Desde, "yyyyMMdd") & "' AND FechaEntregaDesde <='" & Format(Hasta, "yyyyMMdd") & "'"

        Dim SqlCliente As String = ""
        If Cliente <> 0 Then
            SqlCliente = " AND Cliente = " & Cliente
        End If

        'Dim Sql As String = "SELECT C.Pedido,Articulo,Cantidad FROM PedidosCabeza AS C INNER JOIN PedidosDetalle AS D ON C.Pedido = D.Pedido WHERE Cerrado = 0 " & SqlCliente & " AND " & SqlFecha & ";"
        Dim Sql As String = "SELECT C.Pedido,Articulo,Cantidad FROM PedidosCabeza AS C INNER JOIN PedidosDetalle AS D ON C.Pedido = D.Pedido WHERE " & SqlFecha & SqlCliente & ";"
        Dim Dt As New DataTable

        If Not Tablas.Read(Sql, Conexion, Dt) Then Exit Sub
        If Dt.Rows.Count = 0 Then
            MsgBox("No Existen Pedidos para esta Fecha o Cliente.")
            Dt.Dispose()
            Exit Sub
        End If

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Articulo         "
        exHoja.Cells.Item(RowExxel, 3) = "Cantidad"
        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        Dim AA As String
        Dim BB As String

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Articulo"

        Dim UltimaColumnaUtilizada As Integer = 5
        Dim NombreArticuloW As String = ""
        Dim AGranel As Boolean
        Dim Medida As String = ""
        Dim Col As Integer = 3

        For Each Row As DataRowView In View
            NombreArticuloW = NombreArticulo(Row("Articulo"))
            Dim I2 As Integer = InicioDatos
            Do
                AA = Chr(65 + 2 - 1) & I2
                Rango = Nothing
                Rango = exHoja.Range(AA, AA)
                If Rango.Text = NombreArticuloW Or Rango.Text = "" Then Exit Do
                I2 = I2 + 1
            Loop
            exHoja.Cells.Item(I2, 2) = NombreArticuloW
            HallaAGranelYMedida(Row("Articulo"), AGranel, Medida)
            AA = Chr(65 + Col - 1) & I2
            Rango = exHoja.Range(AA, AA)
            If Rango.Text = "" Then
                exHoja.Cells.Item(I2, Col) = Row("Cantidad")
            Else
                exHoja.Cells.Item(I2, Col) = CDec(Rango.Text) + Row("Cantidad")
            End If
            exHoja.Cells.Item(I2, Col + 1) = Medida
        Next

        Dt.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#"
        '
        exHoja.Columns.AutoFit()

        exHoja.Rows.Item(5).Font.Bold = True

        'Muestra titulos.
        exHoja.Rows.Item(1).Font.Bold = True
        exHoja.Rows.Item(2).Font.Bold = True
        exHoja.Cells.Item(1, 23) = Format(Date.Now, "MM/dd/yyyy")
        If Cliente = 0 Then
            exHoja.Cells.Item(2, 1) = "Resumen de Articulo pedidos " & "  Desde el " & Format(Desde, "dd/MM/yyyy") & "  Hasta el " & Format(Hasta, "dd/MM/yyyy")
        Else
            exHoja.Cells.Item(2, 1) = "Resumen de Articulos pedidos Por el Cliente " & NombreCliente(Cliente) & "  Desde el " & Format(Desde, "dd/MM/yyyy") & "  Hasta el " & Format(Hasta, "dd/MM/yyyy")
        End If

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeIngresosLogisticos(ByVal Proveedor As Integer, ByVal Articulo As Integer, ByVal Estado As Integer, ByVal Deposito As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Blanco As Boolean, ByVal Negro As Boolean)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Ingreso"
        exHoja.Cells.Item(RowExxel, 3) = "Fecha"
        exHoja.Cells.Item(RowExxel, 4) = "Proveedor"
        exHoja.Cells.Item(RowExxel, 5) = "Remito/Guia"
        exHoja.Cells.Item(RowExxel, 6) = "Deposito"
        exHoja.Cells.Item(RowExxel, 7) = "Estado"
        exHoja.Cells.Item(RowExxel, 8) = "Articulo"
        exHoja.Cells.Item(RowExxel, 9) = "Cantidad"
        exHoja.Cells.Item(RowExxel, 10) = "Devolucion"
        exHoja.Cells.Item(RowExxel, 11) = "Neto"
        exHoja.Cells.Item(RowExxel, 12) = ""

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim SqlFecha As String = ""
        SqlFecha = "C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlEstado As String = ""
        If Estado <> 0 Then SqlEstado = " AND C.Estado = " & Estado
        Dim SqlProveedor As String = ""
        If Proveedor <> 0 Then SqlProveedor = " AND C.Proveedor = " & Proveedor
        Dim SqlDeposito As String = ""
        If Deposito <> 0 Then SqlDeposito = " AND C.Deposito = " & Deposito
        Dim SqlArticulo As String = ""
        If Articulo <> 0 Then SqlArticulo = " AND D.Articulo = " & Articulo

        Dim SqlB As String = "SELECT 1 AS Operacion,C.Ingreso,C.Fecha,C.Proveedor,C.Estado,C.Deposito,C.Remito,C.Guia,D.Articulo,D.Cantidad,D.Devueltas FROM IngresoArticulosLogisticosCabeza AS C INNER JOIN IngresoArticulosLogisticosDetalle As D ON C.Ingreso = D.Ingreso WHERE " & SqlFecha & SqlEstado & SqlProveedor & SqlDeposito & SqlArticulo & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion,C.Ingreso,C.Fecha,C.Proveedor,C.Estado,C.Deposito,C.Remito,C.Guia,D.Articulo,D.Cantidad,D.Devueltas FROM IngresoArticulosLogisticosCabeza AS C INNER JOIN IngresoArticulosLogisticosDetalle As D ON C.Ingreso = D.Ingreso WHERE " & SqlFecha & SqlEstado & SqlProveedor & SqlDeposito & SqlArticulo & ";"

        Dim DtIngresos As New DataTable
        If Blanco Then
            If Not Tablas.Read(SqlB, Conexion, DtIngresos) Then Exit Sub
        End If
        If Negro Then
            If Not Tablas.Read(SqlN, ConexionN, DtIngresos) Then Exit Sub
        End If

        If DtIngresos.Rows.Count = 0 Then
            MsgBox("No Hay Datos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim View As New DataView
        View = DtIngresos.DefaultView
        View.Sort = "Ingreso"

        Dim IngresoAnt As Integer = 0

        For Each Row As DataRowView In View
            If IngresoAnt <> Row("Ingreso") Then
                If Row("operacion") = 2 Then exHoja.Cells.Item(RowExxel + 1, 1) = "X"
                exHoja.Cells.Item(RowExxel + 1, 2) = Row("Ingreso")
                exHoja.Cells.Item(RowExxel + 1, 3) = Format(Row("Fecha"), "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel + 1, 4) = NombreProveedor(Row("Proveedor"))
                If Row("Remito") <> 0 Then exHoja.Cells.Item(RowExxel + 1, 5) = NumeroEditado(Row("Remito"))
                If Row("Guia") <> 0 Then exHoja.Cells.Item(RowExxel + 1, 5) = Row("Guia")
                If Row("Remito") = 0 And Row("Guia") = 0 Then
                    exHoja.Cells.Item(RowExxel, 12) = "Saldo Inicial"
                End If
                exHoja.Cells.Item(RowExxel + 1, 6) = NombreDepositoInsumos(Row("Deposito"))
                If Row("Estado") = 3 Then exHoja.Cells.Item(RowExxel + 1, 7) = "Anulado"
                IngresoAnt = Row("Ingreso")
            End If
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 8) = NombreArticuloLogistico(Row("Articulo"))
            exHoja.Cells.Item(RowExxel, 9) = Row("Cantidad")
            exHoja.Cells.Item(RowExxel, 10) = Row("Devueltas")
            exHoja.Cells.Item(RowExxel, 11) = CDec(Row("Cantidad")) - CDec(Row("Devueltas"))
        Next

        DtIngresos.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 14) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Informe Ingresos Logísticos Desde el : " & Format(Desde, "dd/MM/yyyy") & "  Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeRemitosLogisticos(ByVal Cliente As Integer, ByVal Articulo As Integer, ByVal Estado As Integer, ByVal Deposito As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Blanco As Boolean, ByVal Negro As Boolean, ByVal Sucursal As Integer)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Interno"
        exHoja.Cells.Item(RowExxel, 3) = "Fecha"
        exHoja.Cells.Item(RowExxel, 4) = "Cliente"
        exHoja.Cells.Item(RowExxel, 5) = "Sucursal"
        exHoja.Cells.Item(RowExxel, 6) = "Remito"
        exHoja.Cells.Item(RowExxel, 7) = "Deposito"
        exHoja.Cells.Item(RowExxel, 8) = "Estado"
        exHoja.Cells.Item(RowExxel, 9) = "Confirmado"
        exHoja.Cells.Item(RowExxel, 10) = "Articulo"
        exHoja.Cells.Item(RowExxel, 11) = "Cantidad"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim SqlFecha As String = ""
        SqlFecha = "C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlEstado As String = ""
        If Estado <> 0 Then SqlEstado = " AND C.Estado = " & Estado
        Dim SqlCliente As String = ""
        If Cliente <> 0 Then SqlCliente = " AND C.Cliente = " & Cliente
        Dim SqlDeposito As String = ""
        If Deposito <> 0 Then SqlDeposito = " AND C.Deposito = " & Deposito
        Dim SqlArticulo As String = ""
        If Articulo <> 0 Then SqlArticulo = " AND D.Articulo = " & Articulo
        Dim SqlSucursal As String = ""
        If Sucursal <> 0 Then SqlSucursal = " AND C.Sucursal = " & Sucursal

        Dim SqlB As String = "SELECT 1 AS Operacion,C.Consumo,C.Fecha,C.Cliente,C.Sucursal,C.Estado,C.Deposito,C.Remito,C.Confirmado,D.Articulo,D.Cantidad FROM RemitosLogisticosCabeza AS C INNER JOIN RemitosLogisticosDetalle As D ON C.Consumo = D.Consumo WHERE " & SqlFecha & SqlEstado & SqlCliente & SqlDeposito & SqlSucursal & SqlArticulo & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion,C.Consumo,C.Fecha,C.Cliente,C.Sucursal,C.Estado,C.Deposito,C.Remito,C.Confirmado,D.Articulo,D.Cantidad FROM RemitosLogisticosCabeza AS C INNER JOIN RemitosLogisticosDetalle As D ON C.Consumo = D.Consumo WHERE " & SqlFecha & SqlEstado & SqlCliente & SqlDeposito & SqlSucursal & SqlArticulo & ";"

        Dim DtIngresos As New DataTable
        If Blanco Then
            If Not Tablas.Read(SqlB, Conexion, DtIngresos) Then Exit Sub
        End If
        If Negro Then
            If Not Tablas.Read(SqlN, ConexionN, DtIngresos) Then Exit Sub
        End If

        If DtIngresos.Rows.Count = 0 Then
            MsgBox("No Hay Datos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim View As New DataView
        View = DtIngresos.DefaultView
        View.Sort = "Consumo"

        Dim ConsumoAnt As Integer = 0

        For Each Row As DataRowView In View
            If ConsumoAnt <> Row("Consumo") Then
                If Row("operacion") = 2 Then exHoja.Cells.Item(RowExxel + 1, 1) = "X"
                exHoja.Cells.Item(RowExxel + 1, 2) = Row("Consumo")
                exHoja.Cells.Item(RowExxel + 1, 3) = Format(Row("Fecha"), "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel + 1, 4) = NombreCliente(Row("Cliente"))
                exHoja.Cells.Item(RowExxel + 1, 5) = NombreSucursalCliente(Row("Cliente"), Row("Sucursal"))
                exHoja.Cells.Item(RowExxel + 1, 6) = NumeroEditado(Row("Remito"))
                exHoja.Cells.Item(RowExxel + 1, 7) = NombreDepositoInsumos(Row("Deposito"))
                If Row("Estado") = 3 Then exHoja.Cells.Item(RowExxel + 1, 8) = "Anulado"
                If Row("Confirmado") Then
                    exHoja.Cells.Item(RowExxel + 1, 9) = "SI"
                End If
                ConsumoAnt = Row("Consumo")
            End If
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 10) = NombreArticuloLogistico(Row("Articulo"))
            exHoja.Cells.Item(RowExxel, 11) = Row("Cantidad")
        Next

        DtIngresos.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 14) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Informe Remitos Logísticos Desde el : " & Format(Desde, "dd/MM/yyyy") & "  Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeSeguimientoOrdenCompra(ByVal Proveedor As Integer, ByVal OrdenCompra As Integer, ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal PeriodoDesde As Date, ByVal PeriodoHasta As Date, ByVal Estado As Integer)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "O.Compra"
        exHoja.Cells.Item(RowExxel, 3) = "Proveedor"
        exHoja.Cells.Item(RowExxel, 4) = "Fecha"
        exHoja.Cells.Item(RowExxel, 5) = "Entrega Desde"
        exHoja.Cells.Item(RowExxel, 6) = "Entrega Hasta"
        exHoja.Cells.Item(RowExxel, 7) = "Insumo"
        exHoja.Cells.Item(RowExxel, 8) = "Cantidad"
        exHoja.Cells.Item(RowExxel, 9) = "C. Entregada"
        exHoja.Cells.Item(RowExxel, 10) = "Saldo"
        exHoja.Cells.Item(RowExxel, 11) = "Estado"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim SqlFecha As String = ""
        SqlFecha = "C.Fecha >='" & Format(FechaDesde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(FechaHasta, "yyyyMMdd") & "') "
        Dim SqlPeriodoFecha As String = ""
        If Format(PeriodoDesde, "dd/MM/yyyy") <> "01/01/1800" Then
            SqlPeriodoFecha = " AND C.FechaEntrega >='" & Format(PeriodoDesde, "yyyyMMdd") & "' AND C.FechaEntrega < DATEADD(dd,1,'" & Format(PeriodoHasta, "yyyyMMdd") & "') "
        End If
        Dim SqlEstado As String = ""
        If Estado <> 0 Then SqlEstado = " AND C.Estado = " & Estado
        Dim Sqlproveedor As String = ""
        If Proveedor <> 0 Then Sqlproveedor = " AND C.Proveedor = " & Proveedor
        Dim SqlOrdenCompra As String = ""
        If OrdenCompra <> 0 Then SqlOrdenCompra = " AND C.Orden = " & OrdenCompra

        Dim SqlB As String = "SELECT C.Orden,C.Fecha,C.FechaEntrega,C.FechaEntregaHasta,C.Proveedor,C.Estado,D.Articulo,D.Cantidad,D.Recibido FROM OrdenCompraCabeza AS C INNER JOIN OrdenCompraDetalle As D ON C.Orden = D.Orden WHERE C.Tipo = 2 AND " & SqlFecha & SqlPeriodoFecha & SqlEstado & Sqlproveedor & SqlOrdenCompra & ";"

        Dim DtOrdenesCompras As New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtOrdenesCompras) Then Exit Sub

        If DtOrdenesCompras.Rows.Count = 0 Then
            MsgBox("No Hay Datos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim View As New DataView
        View = DtOrdenesCompras.DefaultView
        View.Sort = "Orden"

        Dim OrdenAnt As Integer = 0

        For Each Row As DataRowView In View
            If OrdenAnt <> Row("Orden") Then
                exHoja.Cells.Item(RowExxel + 1, 2) = Row("Orden")
                exHoja.Cells.Item(RowExxel + 1, 3) = NombreProveedor(Row("Proveedor"))
                exHoja.Cells.Item(RowExxel + 1, 4) = Format(Row("Fecha"), "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel + 1, 5) = Format(Row("FechaEntrega"), "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel + 1, 6) = Format(Row("FechaEntregaHasta"), "MM/dd/yyyy")
                If Row("Estado") = 3 Then exHoja.Cells.Item(RowExxel + 1, 11) = "Anulado"
                OrdenAnt = Row("Orden")
            End If
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 7) = NombreInsumo(Row("Articulo"))
            exHoja.Cells.Item(RowExxel, 8) = Row("Cantidad")
            exHoja.Cells.Item(RowExxel, 9) = Row("Recibido")
            exHoja.Cells.Item(RowExxel, 10) = CDec(Row("Cantidad")) - CDec(Row("Recibido"))
        Next

        DtOrdenesCompras.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 14) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Informe Seguimiento Ordenes de Compras Desde el : " & Format(FechaDesde, "dd/MM/yyyy") & "  Hasta el " & Format(FechaHasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeDetalleComprobantesQueAfectanLotes(ByVal Proveedor As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Costeo As Integer, ByVal Abierto As Boolean, ByVal Cerrado As Boolean, ByVal ConIva As Boolean, ByVal SinIva As Boolean)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Lote"
        exHoja.Cells.Item(RowExxel, 3) = "Proveedor"
        exHoja.Cells.Item(RowExxel, 4) = "Ingreso"
        exHoja.Cells.Item(RowExxel, 5) = " "
        exHoja.Cells.Item(RowExxel, 6) = "Tipo"
        exHoja.Cells.Item(RowExxel, 7) = "Comprobante"
        exHoja.Cells.Item(RowExxel, 8) = "Proveedor"
        exHoja.Cells.Item(RowExxel, 9) = "Recibo-Oficial"
        exHoja.Cells.Item(RowExxel, 10) = "Fecha"
        exHoja.Cells.Item(RowExxel, 11) = "Importe"
        exHoja.Cells.Item(RowExxel, 12) = " "
        exHoja.Cells.Item(RowExxel, 13) = "Comprobante"
        exHoja.Cells.Item(RowExxel, 14) = "Importe"
        exHoja.Cells.Item(RowExxel, 15) = "Total"
        exHoja.Cells.Item(RowExxel, 16) = "Costo Lote"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim SqlB As String
        Dim SqlN As String

        Dim SqlFecha As String = ""
        SqlFecha = " I.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND I.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlProveedor As String = ""
        If Proveedor <> 0 Then SqlProveedor = " AND I.Proveedor = " & Proveedor
        Dim SqlCosteo As String = ""
        If Costeo <> 0 Then SqlCosteo = " AND I.Costeo = " & Costeo
        Dim SqlLote As String = ""
        If Lote <> 0 Then
            SqlLote = " AND L.Lote = " & Lote & " AND L.Secuencia = " & Format(Secuencia, "000")
        End If

        SqlB = "SELECT 1 As Operacion,L.Lote,L.Secuencia,L.Proveedor,I.Costeo,I.Fecha FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS I ON L.Lote = I.Lote WHERE " & SqlFecha & SqlProveedor & SqlCosteo & SqlLote & ";"
        SqlN = "SELECT 2 As Operacion,L.Lote,L.Secuencia,L.Proveedor,I.Costeo,I.Fecha FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS I ON L.Lote = I.Lote WHERE " & SqlFecha & SqlProveedor & SqlCosteo & SqlLote & ";"

        Dim DtLotes As New DataTable

        If Abierto Then
            If Not Tablas.Read(SqlB, Conexion, DtLotes) Then Exit Sub
        End If
        If Cerrado Then
            If Not Tablas.Read(SqlN, ConexionN, DtLotes) Then Exit Sub
        End If

        Dim View As New DataView
        View = DtLotes.DefaultView
        View.Sort = "Lote,Secuencia"

        Dim LoteAnt As String = ""

        For Each Row As DataRowView In View
            If LoteAnt <> Row("Lote") & "/" & Row("Secuencia") Then
                If Row("operacion") = 2 Then exHoja.Cells.Item(RowExxel + 1, 1) = "X"
                exHoja.Cells.Item(RowExxel + 1, 2) = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
                exHoja.Cells.Item(RowExxel + 1, 3) = NombreProveedor(Row("Proveedor"))
                exHoja.Cells.Item(RowExxel + 1, 4) = Format(Row("Fecha"), "MM/dd/yyyy")
                LoteAnt = Row("Lote") & "/" & Row("Secuencia")
            End If
            AgregaComprobantes(exHoja, Row("Lote"), Row("Secuencia"), RowExxel, Abierto, Cerrado, ConIva)
        Next

        DtLotes.Dispose()
        View = Nothing
        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 13 - 1) & InicioDatos
        BB = Chr(65 + 13 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 14 - 1) & InicioDatos
        BB = Chr(65 + 14 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 15 - 1) & InicioDatos
        BB = Chr(65 + 15 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 16 - 1) & InicioDatos
        BB = Chr(65 + 16 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 17) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Detalle Comprobantes que afectan costos de Lotes Desde el : " & Format(Desde, "dd/MM/yyyy") & "  Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub HallaDocumentosQueImputan(ByVal TipoNota As Integer, ByVal Comprobante As Decimal, ByVal Operacion As Integer, ByVal EsOtrosProveedores As Boolean)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Tipo"
        exHoja.Cells.Item(RowExxel, 3) = "Comprobante"
        exHoja.Cells.Item(RowExxel, 4) = "Fecha"
        exHoja.Cells.Item(RowExxel, 5) = "Importe"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim Dt As New DataTable
        Dim Total As Decimal = 0

        If EsOtrosProveedores Then
            If Not Tablas.Read("SELECT C.TipoNota,C.Movimiento AS Nota,C.Fecha,D.Importe FROM OtrosPagosCabeza AS C INNER JOIN OtrosPagosDetalle AS D ON C.Movimiento = D.Movimiento WHERE C.Estado = 1 AND D.TipoComprobante = " & TipoNota & " AND Comprobante = " & Comprobante & ";", ConexionStr, Dt) Then Exit Sub
        Else
            If Not Tablas.Read("SELECT C.TipoNota,C.Nota,C.Fecha,D.Importe FROM RecibosCabeza AS C INNER JOIN RecibosDetalle AS D ON C.TipoNota = D.TipoNota AND C.Nota = D.Nota WHERE C.Estado = 1 AND D.TipoComprobante = " & TipoNota & " AND Comprobante = " & Comprobante & ";", ConexionStr, Dt) Then Exit Sub
        End If

        For Each Row As DataRow In Dt.Rows
            RowExxel = RowExxel + 1
            If Operacion = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
            exHoja.Cells.Item(RowExxel, 2) = NombreComprobante(Row("TipoNota"))
            exHoja.Cells.Item(RowExxel, 3) = NumeroEditado(Row("Nota"))
            exHoja.Cells.Item(RowExxel, 4) = Format(Row("Fecha"), "MM/dd/yyyy")
            exHoja.Cells.Item(RowExxel, 5) = Row("Importe")
            Total = Total + Row("Importe")
        Next
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 5) = Total

        Dt.Dispose()
        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String

        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 6) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Comprobantes que Imputan"

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeCostosMermaPorLote(ByVal Desde As Date, ByVal Hasta As Date, ByVal Proveedor As Integer)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim SqlFecha As String = ""
        SqlFecha = " F.FechaContable >='" & Format(Desde, "yyyyMMdd") & "' AND F.FechaContable < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlFechaIngreso As String = ""
        SqlFechaIngreso = " L.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND L.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "

        Dim SqlProveedor As String = ""
        If Proveedor <> 0 Then SqlProveedor = "AND F.Proveedor = " & Proveedor
        Dim SqlProveedorIngreso As String = ""
        If Proveedor <> 0 Then SqlProveedorIngreso = "AND L.Proveedor = " & Proveedor

        Dim Dt As New DataTable

        'Facturas.
        If Not Tablas.Read("SELECT C.Lote,C.Secuencia,C.Operacion FROM FacturasProveedorCabeza AS F INNER JOIN ComproFacturados AS C ON F.Factura = C.Factura WHERE F.Estado = 1 AND " & SqlFecha & SqlProveedor & ";", Conexion, Dt) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read("SELECT C.Lote,C.Secuencia,C.Operacion FROM FacturasProveedorCabeza AS F INNER JOIN ComproFacturados AS C ON F.Factura = C.Factura WHERE F.Estado = 1 AND " & SqlFecha & SqlProveedor & " AND F.Rel = 0;", ConexionN, Dt) Then Exit Sub
        End If
        ' Liquidaciones.
        If Not Tablas.Read("SELECT C.Lote,C.Secuencia,C.Operacion FROM LiquidacionCabeza AS F INNER JOIN LiquidacionDetalle AS C ON F.Liquidacion = C.Liquidacion WHERE F.Estado = 1 AND  " & SqlFecha & SqlProveedor & ";", Conexion, Dt) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read("SELECT C.Lote,C.Secuencia,C.Operacion FROM LiquidacionCabeza AS F INNER JOIN LiquidacionDetalle AS C ON F.Liquidacion = C.Liquidacion WHERE F.Estado = 1 AND " & SqlFecha & SqlProveedor & " AND F.Rel = 0;", ConexionN, Dt) Then Exit Sub
        End If

        Dim DtLotes As New DataTable
        Dim Sql As String
        Dim ConexionStr As String

        For Each Row As DataRow In Dt.Rows
            If Row("Operacion") = 1 Then
                ConexionStr = Conexion
            Else
                ConexionStr = ConexionN
            End If
            If Row("Operacion") = 1 Then
                Sql = "SELECT 1 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Liquidado,L.Fecha,L.TipoOperacion,L.Deposito FROM Lotes AS L WHERE L.TipoOperacion = 2 AND L.Cantidad <> L.Baja AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & ";"
            Else
                Sql = "SELECT 2 AS Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Liquidado,L.Fecha,L.TipoOperacion,L.Deposito FROM Lotes AS L WHERE L.TipoOperacion = 2 AND L.Cantidad <> L.Baja AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & ";"
            End If
            If ConexionStr = Conexion Or (ConexionStr = ConexionN And PermisoTotal) Then
                If Not Tablas.Read(Sql, ConexionStr, DtLotes) Then Exit Sub
            End If
        Next

        'Lotes de costeo.
        If Not Tablas.Read("SELECT 1 As Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Liquidado,L.Fecha,L.TipoOperacion,L.Deposito FROM Lotes AS L WHERE L.Cantidad <> L.Baja AND L.TipoOperacion = 4 AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND " & SqlFechaIngreso & SqlProveedorIngreso & ";", Conexion, DtLotes) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read("SELECT 2 As Operacion,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.Liquidado,L.Fecha,L.TipoOperacion,L.Deposito FROM Lotes AS L WHERE L.Cantidad <> L.Baja AND L.TipoOperacion = 4 AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND " & SqlFechaIngreso & SqlProveedorIngreso & ";", ConexionN, DtLotes) Then Exit Sub
        End If

        Dim View As New DataView
        View = DtLotes.DefaultView
        View.Sort = "Lote,Secuencia"

        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer

        Dim TotalImporteSinIva As Decimal
        Dim TotalImporteIva As Decimal
        Dim TotalImporteSinIvaAsignados As Decimal
        Dim TotalImporteIvaAsignados As Decimal

        Dim NombreProveedorW As String = ""

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Lote          "
        exHoja.Cells.Item(RowExxel, 3) = "Fecha  "
        exHoja.Cells.Item(RowExxel, 4) = "Articulo       "
        exHoja.Cells.Item(RowExxel, 5) = "Cantidad Liquidada"
        exHoja.Cells.Item(RowExxel, 6) = "Merma"
        exHoja.Cells.Item(RowExxel, 7) = "Costo Compra S/IVA."
        exHoja.Cells.Item(RowExxel, 8) = "Costo IVA Compra."
        exHoja.Cells.Item(RowExxel, 9) = "Total Costos Compra"
        exHoja.Cells.Item(RowExxel, 10) = ""
        exHoja.Cells.Item(RowExxel, 11) = "Costo Asignado S/IVA"
        exHoja.Cells.Item(RowExxel, 12) = "Costo IVA Asignado"
        exHoja.Cells.Item(RowExxel, 13) = "Total Costos Asignados"
        exHoja.Cells.Item(RowExxel, 14) = "Proveedor   "

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel
        Dim LoteAnt As Integer, SecuenciaAnt As Integer

        For Each Row As DataRowView In View
            If Row("Lote") = LoteAnt And Row("Secuencia") = SecuenciaAnt Then
                Continue For
            Else
                LoteAnt = Row("Lote") : SecuenciaAnt = Row("Secuencia")
            End If
            If ((Row("TipoOperacion") = 1 Or Row("TipoOperacion") = 2) And Row("Liquidado") <> 0) Or Row("TipoOperacion") = 4 Then
                Dim Operacion As String
                If Row("Operacion") = 2 Then Operacion = "X"
                'Halla Datos del Lote Origen.
                Dim CantidadLiquidada As Decimal = 0
                Dim KilosXUnidad As Decimal = 0
                Dim Cantidad As Decimal = 0
                Dim Baja As Decimal = 0
                Dim Merma As Decimal = 0
                Dim Descarte As Decimal = 0
                If Not HallaCanidadLiquidadaParaCostoRealDelLote(Row("Lote"), Row("Secuencia"), Row("Deposito"), Row("Operacion"), CantidadLiquidada, KilosXUnidad, Cantidad, Baja, Merma, Descarte) Then Exit Sub
                Merma = Merma + Descarte
                'Halla Importe sin iva e Iva del lote origen Liquidado. 
                Dim ImporteSinIvaLiquidado As Decimal = 0
                Dim ImporteIvaLiquidado As Decimal = 0
                Dim ImporteSinIvaLiquidadoWW As Decimal = 0
                Dim ImporteIvaLiquidadoWW As Decimal = 0
                Dim Facturado As Boolean = True
                If Not HallaLiquidadoAProveedoresPorLote(Row("Lote"), Row("Secuencia"), Row("Deposito"), Row("Operacion"), Row("TipoOperacion"), ImporteSinIvaLiquidado, ImporteIvaLiquidado, Facturado) Then Exit Sub
                If Row("TipoOperacion") = 1 Or Row("TipoOperacion") = 2 Then
                    ImporteSinIvaLiquidadoWW = Trunca(Merma * (ImporteSinIvaLiquidado / CantidadLiquidada))
                    ImporteIvaLiquidadoWW = Trunca(Merma * (ImporteIvaLiquidado / CantidadLiquidada))
                End If
                If Row("TipoOperacion") = 4 Then
                    ImporteSinIvaLiquidadoWW = Trunca(Merma * KilosXUnidad * ImporteSinIvaLiquidado)
                    ImporteIvaLiquidadoWW = Trunca(Merma * KilosXUnidad * ImporteIvaLiquidado)
                End If
                Dim Cartel As String
                If Row("TipoOperacion") = 4 Then
                    Dim Costeo As Integer = HallaCosteoLote(Row("Operacion"), Row("Lote"))
                    If Costeo < 0 Then Exit Sub
                    If Not HallaCosteoCerrado(Costeo) Then
                        Cartel = "Costo Estimado"
                    End If
                End If
                TotalImporteSinIva = TotalImporteSinIva + ImporteSinIvaLiquidadoWW
                TotalImporteIva = TotalImporteIva + ImporteIvaLiquidadoWW
                'Halla Importe sin iva e Iva del costo asignados a lotes de lote origen Liquidado. 
                Dim ImporteSinIvaRecibos As Decimal = 0
                Dim ImporteIvaRecibos As Decimal = 0
                Dim ImporteSinIvaRecibosWW As Decimal = 0
                Dim ImporteIvaRecibosWW As Decimal = 0
                GConListaDeCostos = False
                If Not CostosAsinadosPorLote(Row("Lote"), Row("Secuencia"), ImporteSinIvaRecibos, ImporteIvaRecibos) Then Exit Sub
                Dim CantidadLoteLiquidadoWW As Decimal = 0
                CantidadLoteLiquidadoWW = Cantidad - Baja
                ImporteSinIvaRecibosWW = Trunca(Merma * (ImporteSinIvaRecibos / CantidadLoteLiquidadoWW))  'Importe sin iva del lote origen liquidado por Unidad del LoteOrigen.
                ImporteIvaRecibosWW = Trunca(Merma * (ImporteIvaRecibos / CantidadLoteLiquidadoWW))        'Importe iva del lote origen liquidado por Unidad del LoteOrigen.
                TotalImporteSinIvaAsignados = TotalImporteSinIvaAsignados + ImporteSinIvaRecibosWW
                TotalImporteIvaAsignados = TotalImporteIvaAsignados + ImporteIvaRecibosWW
                NombreProveedorW = NombreProveedor(Row("Proveedor"))
                If Merma <> 0 Then
                    RowExxel = RowExxel + 1
                    AgregaAExcelParaCostoMerma(exHoja, RowExxel, Operacion, Format(Row("Fecha"), "MM/dd/yyyy"), NombreArticulo(Row("Articulo")), Row("Lote") & "/" & Format(Row("Secuencia"), "000"), CantidadLiquidada, Merma, ImporteSinIvaLiquidadoWW, ImporteIvaLiquidadoWW, ImporteSinIvaLiquidadoWW + ImporteIvaLiquidadoWW, Cartel, ImporteSinIvaRecibosWW, ImporteIvaRecibosWW, ImporteSinIvaRecibosWW + ImporteIvaRecibosWW, NombreProveedorW)
                End If
            End If
        Next
        'Totales.
        RowExxel = RowExxel + 1
        AgregaAExcelParaCostoMerma(exHoja, RowExxel, "", "", "", "", 0, 0, TotalImporteSinIva, TotalImporteIva, TotalImporteSinIva + TotalImporteIva, "", TotalImporteSinIvaAsignados, TotalImporteIvaAsignados, TotalImporteSinIvaAsignados + TotalImporteIvaAsignados, "")

        DtLotes.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 12 - 1) & InicioDatos
        BB = Chr(65 + 12 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 13 - 1) & InicioDatos
        BB = Chr(65 + 13 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Dibuja rectangulos.=------------------------------
        If InicioDatos < UltimaLinea - 1 Then
            AA = Chr(65 + 1 - 1) & InicioDatos
            BB = Chr(65 + 14 - 1) & UltimaLinea - 1
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
        End If
        '--------------------------------------------------

        exHoja.Rows.Item(5).Font.Bold = True

        'Muestra titulos.
        exHoja.Cells.Item(1, 14) = Format(Date.Now, "MM/dd/yyyy")
        If Proveedor = 0 Then
            exHoja.Cells.Item(2, 1) = "Detalle Costos Mermas de Lotes"
        Else
            exHoja.Cells.Item(2, 1) = "Detalle Costos Mermas de Lotes del Proveedor : " & NombreProveedor(Proveedor)
        End If

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Private Function HallaImportesFacturaProveedor(ByVal Operacion As Integer, ByVal Factura As Double, ByVal Cambio As Double, ByVal DtMedioPago As DataTable, ByRef NetoGrabado As Decimal, ByRef NetoNoGrabado As Decimal, ByRef ImporteIva As Decimal, ByRef ImporteRetPer As Decimal, ByRef ImporteSenia As Decimal) As Boolean

        Dim ConexionStr As String
        Dim RowsBusqueda() As DataRow

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Impuesto,Importe FROM FacturasProveedorDetalle WHERE Factura = " & Factura & ";", ConexionStr, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            RowsBusqueda = DtMedioPago.Select("Clave = " & Row("Impuesto"))
            If RowsBusqueda.Length = 0 Then
                MsgBox(RowsBusqueda(0).Item("Nombre") & " No Encontrado como Medio de Pago. Proceso se CANCELA.", MsgBoxStyle.Critical)
                Dt.Dispose()
                Return False
            End If
            If Cambio <> 1 Then Row("Importe") = Trunca(Row("Importe") * Cambio)
            If Not ProcesaTotalesFacturaProveedor(RowsBusqueda(0).Item("Clave"), RowsBusqueda(0).Item("Tipo"), Row("Importe"), NetoGrabado, NetoNoGrabado, ImporteIva, ImporteRetPer, ImporteSenia) Then Return False
        Next

        Dt.Dispose()
        Return False

    End Function
    Private Function HallaImportesConsumo(ByVal Operacion As Integer, ByVal Consumo As Double, ByRef NetoGrabado As Decimal, ByRef ImporteIva As Decimal, ByRef Cambio As Double) As Boolean

        Dim ConexionStr As String

        NetoGrabado = 0
        ImporteIva = 0
        Cambio = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT ImporteSinIva, ImporteConIva FROM ConsumosCabeza WHERE Consumo = " & Consumo & ";", ConexionStr, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            NetoGrabado = Dt.Rows(0).Item("ImporteSinIva")
            ImporteIva = Dt.Rows(0).Item("ImporteConIva") - Dt.Rows(0).Item("ImporteSinIva")
            Cambio = 1
        End If

        Dt.Dispose()

        Return True

    End Function
    Private Function HallaImportesConsumoTerminados(ByVal Operacion As Integer, ByVal Consumo As Double, ByRef NetoGrabado As Decimal, ByRef ImporteIva As Decimal, ByRef Cambio As Double) As Boolean

        Dim ConexionStr As String

        NetoGrabado = 0
        ImporteIva = 0
        Cambio = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim Sql As String = "SELECT Lote,Secuencia,Importe FROM ConsumosPTLotes WHERE Consumo = " & Consumo & ";"
        Dim Dt As New DataTable
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then End
        If Dt.Rows.Count = 0 Then dt.dispose() : Return True

        For Each Row As DataRow In dt.rows
            Dim Articulo As Integer = HallaArticulo(Row("Lote"), Row("Secuencia"), ConexionStr)
            Dim Iva As Decimal = HallaIva(Articulo)
            Dim NetoGrabadoW = Trunca(Row("Importe") * 100 / (100 + Iva))
            Dim ImporteIvaW = Row("Importe") - NetoGrabadoW
            NetoGrabado = NetoGrabado + NetoGrabadoW
            ImporteIva = ImporteIva + ImporteIvaW
        Next
        Cambio = 1

        Dt.Dispose()

        Return True

    End Function
    Private Function HallaImportesAsientosManuales(ByVal Operacion As Integer, ByVal Asiento As Double, ByRef NetoGrabado As Decimal, ByRef ImporteIva As Decimal, ByRef Cambio As Double, ByVal EsDebito As Boolean) As Boolean

        Dim ConexionStr As String

        NetoGrabado = 0
        ImporteIva = 0
        Cambio = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim Sql As String = "SELECT * FROM AsientosDetalle WHERE Asiento  = " & Asiento & ";"
        Dim Dt As New DataTable
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then End
        If Dt.Rows.Count = 0 Then Dt.Dispose() : Return True

        For Each Row As DataRow In Dt.Rows
            NetoGrabado = NetoGrabado + Row("Debe")
        Next
        Cambio = 1

        If Not EsDebito Then
            NetoGrabado = -NetoGrabado
        End If

        Dt.Dispose()

        Return True

    End Function
    Private Function ProcesaTotalesFacturaProveedor(ByVal Clave As Integer, ByVal Tipo As Integer, ByVal Importe As Decimal, ByRef NetoGrabado As Decimal, ByRef NetoNoGrabado As Decimal, ByRef ImporteIva As Decimal, ByRef ImporteRetPer As Decimal, ByRef ImporteSenia As Decimal) As Boolean

        Select Case Clave
            Case 1
                NetoGrabado = NetoGrabado + Importe
            Case 2
                NetoNoGrabado = NetoNoGrabado + Importe
            Case 10
                ImporteSenia = ImporteSenia + Importe
            Case Else
                Select Case Tipo
                    Case 22
                        ImporteIva = ImporteIva + Importe
                    Case 25
                        ImporteRetPer = ImporteRetPer + Importe
                    Case Else
                        Return False
                End Select
        End Select

        Return True

    End Function
    Private Sub HallaDatosCliente(ByVal Cliente As Integer)

        If Cliente = ClienteW Then Exit Sub

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Nombre,Canalventa FROM Clientes WHERE Clave = " & Cliente & ";", Conexion, Dt) Then End
        NombreClienteW = Dt.Rows(0).Item("Nombre")
        CanalVentaW = Dt.Rows(0).Item("CanalVenta")
        ClienteW = Cliente

        Dt.Dispose()

    End Sub
    Private Function HallaCanalDeVenta(ByVal Canal As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Tipo = 23 AND Clave = " & Canal & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tablas.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function HallaVendedor(ByVal Vendedor As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Tipo = 37 AND Clave = " & Vendedor & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tablas.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function HallaLotesFacturados(ByVal Factura As Double, ByVal FacturaRel As Double, ByVal ConexionStr As String, ByRef Dt As DataTable, ByRef ImporteFactura As Decimal) As Boolean

        Dim DtFactura As New DataTable
        Dim FacturaCompro As Double = Factura
        Dim ConexionCompro As String = ConexionStr

        ImporteFactura = 0

        If FacturaRel <> 0 And ConexionStr = Conexion Then
            If Not Tablas.Read("SELECT Factura,Importe FROM FacturasProveedorCabeza WHERE Factura = " & FacturaRel & ";", ConexionN, DtFactura) Then Return False
            FacturaCompro = DtFactura.Rows(0).Item("Factura")
            ConexionCompro = ConexionN
            ImporteFactura = DtFactura.Rows(0).Item("Importe")
        End If

        If FacturaRel <> 0 And ConexionStr = ConexionN Then
            DtFactura = New DataTable
            If Not Tablas.Read("SELECT Importe FROM FacturasProveedorCabeza WHERE Factura = " & FacturaRel & ";", Conexion, DtFactura) Then Return False
            ImporteFactura = DtFactura.Rows(0).Item("Importe")
        End If
        DtFactura.Dispose()

        Dim Sql As String = "SELECT Lote,Secuencia,Operacion,ImporteConIva,ImporteSinIva,Senia FROM ComproFacturados WHERE Factura = " & FacturaCompro & ";"
        If Not Tablas.Read(Sql, ConexionCompro, Dt) Then
            MsgBox("Error Al Leer Facturas Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    Private Function HallaLotesLiquidados(ByVal Liquidacion As Double, ByVal LiquidacionRel As Double, ByVal ConexionStr As String, ByVal Dt As DataTable) As Boolean

        Dim Sql As String = "SELECT Lote,Secuencia,Operacion,NetoConIva as ImporteConIva FROM LiquidacionDetalle WHERE Liquidacion = " & Liquidacion & ";"
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then
            MsgBox("Error Al Leer Facturas Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Dim ConexionRel As String = ""
        If LiquidacionRel <> 0 And ConexionStr = Conexion Then ConexionRel = ConexionN
        If LiquidacionRel <> 0 And ConexionStr = ConexionN Then ConexionRel = Conexion

        If LiquidacionRel <> 0 Then
            Dim DtRel As New DataTable
            Sql = "SELECT Lote,Secuencia,Operacion,NetoConIva as ImporteConIva FROM LiquidacionDetalle WHERE Liquidacion = " & LiquidacionRel & ";"
            If Not Tablas.Read(Sql, ConexionRel, DtRel) Then
                MsgBox("Error Al Leer Facturas Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            Dim RowsBusqueda() As DataRow
            For Each Row As DataRow In DtRel.Rows
                RowsBusqueda = Dt.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
                RowsBusqueda(0).Item("ImporteConIva") = RowsBusqueda(0).Item("ImporteConIva") + Row("ImporteConIva")
            Next
            DtRel.Dispose()
        End If

        Return True

    End Function
    Private Function HallaNombreArticuloYNetos(ByVal Articulo As Integer, ByVal ImporteConIva As Double, ByVal ImporteSinIva As Double, ByRef NombreArticulo As String, ByRef ImporteBlanco As Double, ByRef NetoNegro As Double) As Boolean

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Nombre,Iva FROM Articulos WHERE Clave = " & Articulo & ";", Conexion, Dt) Then Return False

        NombreArticulo = Dt.Rows(0).Item("Nombre")

        ImporteBlanco = 0
        NetoNegro = 0

        If Dt.Rows(0).Item("Iva") = 0 Then
            ImporteBlanco = ImporteConIva
            NetoNegro = ImporteSinIva
            Dt.Dispose()
            Return True
        End If

        Dim MontoIva As Double = ImporteConIva - ImporteSinIva
        Dim NetoBlanco = Trunca(MontoIva * 100 / Dt.Rows(0).Item("Iva"))
        ImporteBlanco = Trunca(NetoBlanco + MontoIva)
        NetoNegro = Trunca(ImporteSinIva - NetoBlanco)

        Dt.Dispose()

        Return True

    End Function
    Private Function HallaDatosLote(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As Integer, ByRef FechaIngreso As Date, ByRef Cantidad As Integer) As Boolean

        Dim ConexionStr As String
        Dim Dt As New DataTable

        Cantidad = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Dim Sql As String = "SELECT Fecha,Cantidad FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"

        If Not Tablas.Read(Sql, ConexionStr, Dt) Then Return False
        Cantidad = Dt.Rows(0).Item("Cantidad")
        FechaIngreso = Dt.Rows(0).Item("Fecha")
        Dt.Dispose()

        Return True

    End Function
    Private Sub HallaTipoOperacionSeniaArticulo(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal ConexionStr As String, ByRef TipoOperacion As Integer, ByRef Senia As Double, ByRef Articulo As Integer, ByRef Proveedor As Integer)

        Dim Dt As New DataTable
        Dim ArticuloW As Integer = 0

        If Secuencia >= 100 Then
            If Not Tablas.Read("SELECT Articulo,LoteOrigen,SecuenciaOrigen FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", ConexionStr, Dt) Then
                MsgBox("Error Base de Datos al Leer Tabla Lotes.", MsgBoxStyle.Critical)
                End
            End If
            ArticuloW = Dt.Rows(0).Item("Articulo")
            Lote = Dt.Rows(0).Item("LoteOrigen")
            Secuencia = Dt.Rows(0).Item("SecuenciaOrigen")
        End If

        Dt = New DataTable

        Dim Sql As String = "SELECT TipoOperacion,Senia,Articulo,Proveedor FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"

        If Not Tablas.Read(Sql, ConexionStr, Dt) Then
            MsgBox("Error Base de Datos al Leer Tabla Lotes.", MsgBoxStyle.Critical)
            End
        End If

        TipoOperacion = Dt.Rows(0).Item("TipoOperacion")
        Senia = Dt.Rows(0).Item("Senia")
        Articulo = Dt.Rows(0).Item("Articulo")
        Proveedor = Dt.Rows(0).Item("Proveedor")

        If ArticuloW <> 0 Then
            If ArticuloW <> Articulo Then Senia = 0
        End If

        Dt.Dispose()

    End Sub
    Private Function HallaNetosYFacturas(ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef NetoConIva As Double, ByRef NetoSinIva As Double, ByRef Factura As Double, ByRef ReciboOficial As Double, ByRef ConexionW As String) As Boolean

        Dim Dt As New DataTable
        Dim ConexionFactura As String = Conexion

        NetoConIva = 0
        NetoSinIva = 0
        ConexionW = ""

        Dim Sql As String = "SELECT ImporteConIva,ImporteSinIva,C.Factura,C.Rel,C.Nrel,C.ReciboOficial FROM FacturasProveedorCabeza AS C INNER JOIN ComproFacturados AS F ON C.Factura = F.Factura WHERE C.EsReventa = 1 AND C.Estado <> 3 AND Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"
        If Not Tablas.Read(Sql, ConexionFactura, Dt) Then
            MsgBox("Error Al Leer Facturas Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Dt.Rows.Count <> 0 Then
            NetoConIva = Dt.Rows(0).Item("ImporteConIva")
            NetoSinIva = Dt.Rows(0).Item("ImporteSinIva")
            Factura = Dt.Rows(0).Item("Factura")
            ReciboOficial = Dt.Rows(0).Item("ReciboOficial")
            ConexionW = Conexion
        Else
            'si no esta en la blanca entonces esta en la negra.
            If Not PermisoTotal Then Dt.Dispose() : Factura = 0 : Return True
            ConexionFactura = ConexionN
            If Not Tablas.Read(Sql, ConexionFactura, Dt) Then
                MsgBox("Error Al Leer Facturas Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Dt.Dispose()
                Return False
            End If
            If Dt.Rows.Count <> 0 Then
                NetoConIva = Dt.Rows(0).Item("ImporteConIva")
                NetoSinIva = Dt.Rows(0).Item("ImporteSinIva")
                If Dt.Rows(0).Item("Nrel") = 0 Then
                    Factura = Dt.Rows(0).Item("Factura")
                    ConexionW = ConexionN
                Else : Factura = Dt.Rows(0).Item("Nrel")
                    ConexionW = Conexion
                End If
                ReciboOficial = Dt.Rows(0).Item("ReciboOficial")
            End If
        End If

        Dt.Dispose()
        Return True

    End Function
    Private Function HallaNetosYLiquidacion(ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef NetoConIva As Double, ByRef NetoSinIva As Double, ByRef Liquidacion As Double, ByRef ReciboOficial As Double, ByRef ConexionW As String) As Boolean

        Dim Dt As New DataTable
        Dim ConexionLiquidacion As String = Conexion

        NetoConIva = 0
        NetoSinIva = 0
        Liquidacion = 0
        ConexionW = ""

        Dim Sql As String = "SELECT Lote,Secuencia,Operacion,NetoConIva,NetoSinIva,C.Liquidacion,C.Rel,C.Nrel FROM LiquidacionCabeza AS C INNER JOIN LiquidacionDetalle AS D ON C.Liquidacion = D.Liquidacion WHERE C.Estado <> 3 AND Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"
        If Not Tablas.Read(Sql, ConexionLiquidacion, Dt) Then
            MsgBox("Error Al Leer Tabla Liquidaciones Detalle.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Dt.Rows.Count <> 0 Then
            Liquidacion = Dt.Rows(0).Item("Liquidacion")
            ReciboOficial = Liquidacion
            NetoConIva = Dt.Rows(0).Item("NetoConIva")
            ConexionW = Conexion
            If Dt.Rows(0).Item("Rel") And PermisoTotal Then
                Dim Dt2 As New DataTable
                If Not Tablas.Read("SELECT NetoConIva, netoSinIva FROM LiquidacionDetalle AS D INNER JOIN LiquidacionCabeza AS C ON D.Liquidacion = C.Liquidacion WHERE C.Nrel = " & Liquidacion & ";", ConexionN, Dt2) Then
                    MsgBox("Error Al Leer Tabla Liquidaciones Detalle.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Dt.Dispose()
                    Return False
                End If
                NetoSinIva = Dt2.Rows(0).Item("NetoSinIva")
                Dt2.Dispose()
            End If
            Dt.Dispose()
            Return True
        End If

        If Not PermisoTotal Then Dt.Dispose() : Return True

        'procesa caso que es negro solamente.
        Dt.Clear()
        ConexionLiquidacion = ConexionN
        If Not Tablas.Read(Sql, ConexionLiquidacion, Dt) Then
            MsgBox("Error Al Leer Tabla Liquidaciones Detalle.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Dt.Rows.Count <> 0 Then
            Liquidacion = Dt.Rows(0).Item("Liquidacion")
            ReciboOficial = Liquidacion
            NetoSinIva = Dt.Rows(0).Item("NetoSinIva")
            ConexionW = ConexionN
            Dt.Dispose()
            Return True
        End If

        Dt.Dispose()
        Return True

    End Function
    Private Function HallaSeniaEnFacturaReventa(ByVal Factura As Double, ByVal FacturaRel As Double, ByVal ConexionStr As String) As Double

        Dim Senia As Decimal = 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Importe FROM FacturasProveedorDetalle WHERE Impuesto = 10 AND Factura = " & Factura & ";", Miconexion)
                    Senia = Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer FacturasProveedorDetalle.", MsgBoxStyle.Critical)
            End
        End Try

        If FacturaRel <> 0 And ConexionStr = Conexion And Not PermisoTotal Then Return Senia

        Dim ConexionRel As String = ""
        If FacturaRel <> 0 And ConexionStr = Conexion Then ConexionRel = ConexionN
        If FacturaRel <> 0 And ConexionStr = ConexionN Then ConexionRel = Conexion

        If FacturaRel <> 0 Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(ConexionRel)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT Importe FROM FacturasProveedorDetalle WHERE Impuesto = 10 AND Factura = " & FacturaRel & ";", Miconexion)
                        Senia = Senia + Cmd.ExecuteScalar()
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer FacturasProveedorDetalle.", MsgBoxStyle.Critical)
                End
            End Try
        End If

        Return Senia

    End Function
    Private Function HallaFacturaProveedorRelacionada(ByVal Factura As Double, ByVal ConexionStr As String, ByVal Rel As Boolean, ByVal Nrel As Double) As Double

        If Rel And ConexionStr = Conexion And Not PermisoTotal Then Return 0

        If Rel And ConexionStr = Conexion Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT Factura FROM FacturasProveedorCabeza WHERE Nrel = " & Factura & ";", Miconexion)
                        Return Cmd.ExecuteScalar()
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer FacturasProveedorCabeza.", MsgBoxStyle.Critical)
                End
            End Try
        End If

        If Rel And ConexionStr = ConexionN Then Return Nrel

    End Function
    Private Function HallaLiquidacionRelacionada(ByVal Liquidacion As Double, ByVal ConexionStr As String, ByVal Rel As Boolean, ByVal Nrel As Double) As Double

        If Rel And ConexionStr = Conexion And Not PermisoTotal Then Return 0

        If Rel And ConexionStr = Conexion Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT Liquidacion FROM LiquidacionCabeza WHERE Nrel = " & Liquidacion & ";", Miconexion)
                        Return Cmd.ExecuteScalar()
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer Tabla LiquidacionCabeza.", MsgBoxStyle.Critical)
                End
            End Try
        End If

        If Rel And ConexionStr = ConexionN Then Return Nrel

    End Function
    Private Function HallaImporteLiquidacionRel(ByVal LiquidacionRel As Double, ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Importe FROM LiquidacionCabeza WHERE Liquidacion = " & LiquidacionRel & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla LiquidacionCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function HallaUnidadesSeniaPagada(ByVal Factura As Double, ByVal Rel As Boolean, ByVal Nrel As Double, ByVal ConexionStr As String) As Double

        Dim ConexionComp As String = ConexionStr
        Dim FacturaComp As Double = Factura
        Dim Cantidad As Decimal = 0

        If Rel And ConexionStr = Conexion And Not PermisoTotal Then Return 0

        If Rel And ConexionStr = Conexion Then
            FacturaComp = HallaFacturaProveedorRelacionada(Factura, ConexionStr, Rel, Nrel)
            ConexionComp = ConexionN
        End If

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Operacion,Lote,Secuencia FROM ComproFacturados WHERE Senia <> 0 AND Factura = " & FacturaComp & ";", ConexionComp, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Dim ConexionLote As String
            If Row("Operacion") = 1 Then
                ConexionLote = Conexion
            Else : ConexionLote = ConexionN
            End If
            Try
                Using Miconexion As New OleDb.OleDbConnection(ConexionLote)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT Cantidad - Baja FROM Lotes WHERE Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & ";", Miconexion)
                        Cantidad = Cantidad + Cmd.ExecuteScalar()
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer Tabla Lotes.", MsgBoxStyle.Critical)
                End
            End Try

        Next

        Dt.Dispose()
        Return Cantidad

    End Function
    Private Sub NombreYAliasProveedor(ByVal Proveedor As Integer, ByRef Nombre As String, ByRef AliasStr As String)

        Nombre = ""
        AliasStr = ""

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Nombre,Alias FROM Proveedores WHERE Clave = " & Proveedor & ";", Conexion, Dt) Then End
        If Dt.Rows.Count Then
            Nombre = Dt.Rows(0).Item("Nombre")
            AliasStr = Dt.Rows(0).Item("Alias")
        End If

    End Sub
    Private Sub LoteArticuloRemitoConIndice(ByVal Remito As Double, ByVal Operacion As Integer, ByVal Indice As Integer, ByRef Lote As Integer, ByRef Secuencia As Integer)

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Lote = 0
        Secuencia = 0

        Dim Sql As String = "SELECT Lote,Secuencia FROM AsignacionLotes WHERE TipoComprobante = 1 AND Comprobante = " & Remito & " AND Indice = " & Indice & ";"

        Dim Dt As New DataTable
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then
            MsgBox("Error Base de Datos al leer Tabla de AsignacionLotes.", MsgBoxStyle.Critical)
            End
        End If
        If Dt.Rows.Count = 0 Then Dt.Dispose() : Exit Sub
        Lote = Dt.Rows(0).Item("Lote")
        Secuencia = Dt.Rows(0).Item("Secuencia")

        Dt.Dispose()

    End Sub
    Private Sub DibujaRango(ByVal Rango As Microsoft.Office.Interop.Excel.Range)

        Rango.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.XlLineStyle.xlLineStyleNone
        Rango.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.XlLineStyle.xlLineStyleNone

        With Rango.Borders(Excel.XlBordersIndex.xlEdgeLeft)
            .LineStyle = Excel.XlLineStyle.xlContinuous
            .ColorIndex = 1 'black
            .Weight = Excel.XlBorderWeight.xlMedium
        End With
        With Rango.Borders(Excel.XlBordersIndex.xlEdgeTop)
            .LineStyle = Excel.XlLineStyle.xlContinuous
            .ColorIndex = 1 'black
            .Weight = Excel.XlBorderWeight.xlMedium
        End With
        With Rango.Borders(Excel.XlBordersIndex.xlEdgeBottom)
            .LineStyle = Excel.XlLineStyle.xlContinuous
            .ColorIndex = 1 'black
            .Weight = Excel.XlBorderWeight.xlMedium
        End With
        With Rango.Borders(Excel.XlBordersIndex.xlEdgeRight)
            .LineStyle = Excel.XlLineStyle.xlContinuous
            .ColorIndex = 1 'black
            .Weight = Excel.XlBorderWeight.xlMedium
        End With
        With Rango.Borders(Excel.XlBordersIndex.xlInsideVertical)
            .LineStyle = Excel.XlLineStyle.xlContinuous
            .ColorIndex = 1 'black
            .Weight = Excel.XlBorderWeight.xlThin
        End With
        With Rango.Borders(Excel.XlBordersIndex.xlInsideHorizontal)
            .LineStyle = Excel.XlLineStyle.xlContinuous
            .ColorIndex = 1 'black
            .Weight = Excel.XlBorderWeight.xlThin
        End With

    End Sub
    Private Function HallaTotalesPorFacturas(ByVal LoteOrigen As Integer, ByVal SecuenciaOrigen As Integer, ByRef MontoFacturaBSinIva As Decimal, ByRef NetoFacturaN As Decimal) As Boolean

        MontoFacturaBSinIva = 0
        NetoFacturaN = 0

        Dim Dt As New DataTable
        Dim Sql As String

        If Not Tablas.Read("SELECT 1 As Operacion,Lote,Secuencia FROM Lotes WHERE LoteOrigen = " & LoteOrigen & " AND SecuenciaOrigen = " & SecuenciaOrigen & ";", Conexion, Dt) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read("SELECT 2 As Operacion,Lote,Secuencia FROM Lotes WHERE LoteOrigen = " & LoteOrigen & " AND SecuenciaOrigen = " & SecuenciaOrigen & ";", ConexionN, Dt) Then Return False
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Lote,Secuencia"

        For Each Row As DataRowView In View
            Try
                Sql = "SELECT SUM(ImporteSinIva) FROM AsignacionLotes WHERE TipoComprobante = 2 AND Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & ";"
                Using Miconexion As New OleDb.OleDbConnection(Conexion)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                        Dim Resul = Cmd.ExecuteScalar()
                        If Not IsDBNull(Resul) Then MontoFacturaBSinIva = MontoFacturaBSinIva + CDbl(Resul)
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer Tabla AsignacionLotes.", MsgBoxStyle.Critical)
                Dt.Dispose()
                Return False
            End Try
        Next

        If PermisoTotal Then
            For Each Row As DataRowView In View
                Try
                    Sql = "SELECT SUM(ImporteSinIva) FROM AsignacionLotes WHERE TipoComprobante = 2 AND Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & ";"
                    Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                        Miconexion.Open()
                        Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                            Dim Resul = Cmd.ExecuteScalar()
                            If Not IsDBNull(Resul) Then NetoFacturaN = NetoFacturaN + CDbl(Resul)
                        End Using
                    End Using
                Catch ex As Exception
                    MsgBox("Error Base de Datos al leer Tabla AsignacionLotes.", MsgBoxStyle.Critical)
                    Dt.Dispose()
                    Return False
                End Try
            Next
        End If

        Dt.Dispose()
        View.Dispose()
        Return True

    End Function
    Private Function HallaTotalesPorNVLP(ByVal LoteOrigen As Integer, ByVal SecuenciaOrigen As Integer, ByRef MontoFacturaBSinIva As Decimal, ByRef NetoFacturaN As Decimal, ByRef ComisionDescargaFacturaB As Decimal) As Boolean

        Dim Dt As New DataTable
        Dim MontoFacturaBSinIvaW As Decimal, NetoFacturaNW As Decimal, ComisionDescargaFacturaBW As Decimal

        MontoFacturaBSinIva = 0 : NetoFacturaN = 0 : ComisionDescargaFacturaB = 0

        If Not Tablas.Read("SELECT 1 As Operacion,Lote,Secuencia FROM Lotes WHERE LoteOrigen = " & LoteOrigen & " AND SecuenciaOrigen = " & SecuenciaOrigen & ";", Conexion, Dt) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read("SELECT 2 As Operacion,Lote,Secuencia FROM Lotes WHERE LoteOrigen = " & LoteOrigen & " AND SecuenciaOrigen = " & SecuenciaOrigen & ";", ConexionN, Dt) Then Return False
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Lote,Secuencia"

        Dim DtNVLP As DataTable
        Dim ConexionStr As String

        For Each Row As DataRowView In View
            DtNVLP = HallaNVLPLote(Row("Operacion"), Row("Lote"), Row("Secuencia"))
            For Each Row1 As DataRow In DtNVLP.Rows
                If Row1("Operacion") = 1 Then
                    ConexionStr = Conexion
                Else : ConexionStr = ConexionN
                End If
                If Not HallaImporteNVLPBlancoNegro(Row("Lote"), Row("Secuencia"), Row1("Liquidacion"), ConexionStr, MontoFacturaBSinIvaW, NetoFacturaNW, ComisionDescargaFacturaBW) Then Return False
                MontoFacturaBSinIva = MontoFacturaBSinIva + MontoFacturaBSinIvaW
                ComisionDescargaFacturaB = ComisionDescargaFacturaB + ComisionDescargaFacturaBW
                NetoFacturaN = NetoFacturaN + NetoFacturaNW
            Next
        Next

        Dt.Dispose()
        Return True

    End Function
    Private Function HallaLiquidacionLote(ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef LiquidacionB As Double, ByRef LiquidacionN As Double) As Boolean

        Dim Dt As New DataTable

        LiquidacionB = 0
        LiquidacionN = 0

        Dim Sql As String = "SELECT C.Liquidacion,C.Rel FROM LiquidacionCabeza AS C INNER JOIN LiquidacionDetalle AS D ON C.Liquidacion = D.Liquidacion WHERE C.Estado <> 3 AND D.Lote = " & Lote & " AND D.Secuencia = " & Secuencia & ";"

        If Not Tablas.Read(Sql, Conexion, Dt) Then
            MsgBox("Error Al Leer Tabla Liquidaciones.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Dt.Rows.Count <> 0 Then
            LiquidacionB = Dt.Rows(0).Item("Liquidacion")
        End If

        If Dt.Rows.Count <> 0 Then
            If Not Dt.Rows(0).Item("Rel") Then Dt.Dispose() : Return True
        End If
        If Not PermisoTotal Then Dt.Dispose() : Return True

        If Dt.Rows.Count = 0 Then
            Dt.Clear()
            If Not Tablas.Read(Sql, ConexionN, Dt) Then
                MsgBox("Error Al Leer Tabla Liquidaciones.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Dt.Rows.Count <> 0 Then
                LiquidacionN = Dt.Rows(0).Item("Liquidacion")
            End If
        End If
        If Dt.Rows.Count <> 0 Then
            Dt.Clear()
            Sql = "SELECT Liquidacion FROM LiquidacionCabeza WHERE Nrel = " & LiquidacionB & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then
                MsgBox("Error Al Leer Tabla Liquidaciones.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Dt.Rows.Count <> 0 Then
                LiquidacionN = Dt.Rows(0).Item("Liquidacion")
            End If
        End If

        Dt.Dispose()
        Return True

    End Function
    Private Function HallaNVLPLote(ByVal Operacion As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer) As DataTable

        Dim Dt As New DataTable

        Dim SqlB As String = "SELECT 1 AS Operacion, C.Liquidacion FROM NVLPCabeza AS C INNER JOIN NVLPLotes AS D ON C.Liquidacion = D.Liquidacion WHERE C.Estado <> 3 AND D.Lote = " & Lote & " AND D.Secuencia = " & Secuencia & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion, C.Liquidacion FROM NVLPCabeza AS C INNER JOIN NVLPLotes AS D ON C.Liquidacion = D.Liquidacion WHERE C.Estado <> 3 AND D.Lote = " & Lote & " AND D.Secuencia = " & Secuencia & ";"

        If Operacion = 1 Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then
                MsgBox("Error Al Leer Tabla NVLP.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End If
        End If
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then
                MsgBox("Error Al Leer Tabla NVLP.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End If
        End If

        HallaNVLPLote = Dt

        Dt.Dispose()

    End Function
    Private Function HallaFacturasLote(ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef FacturaB As Double, ByRef FacturaN As Double, ByRef ReciboOficial As Double, ByRef TipoNota As Integer, ByRef NotaDebito As Double) As Boolean

        Dim Dt As New DataTable
        Dim DtFactura As New DataTable
        Dim Sql As String
        Dim SqlFac As String

        FacturaB = 0
        FacturaN = 0
        TipoNota = 0
        NotaDebito = 0

        Sql = "SELECT Factura FROM ComproFacturados WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then
            MsgBox("Error Al Leer ComproFacturados.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        For Each Row As DataRow In Dt.Rows
            DtFactura = New DataTable
            SqlFac = "SELECT ReciboOficial,TipoNota,NotaDebito FROM FacturasProveedorCabeza WHERE EsReventa = 1 AND Liquidacion = 0 AND Estado <> 3 AND Factura = " & Row("Factura") & ";"
            If Not Tablas.Read(SqlFac, Conexion, DtFactura) Then
                MsgBox("Error Al Leer Facturas Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If DtFactura.Rows.Count <> 0 Then
                FacturaB = Row("Factura")
                ReciboOficial = DtFactura.Rows(0).Item("ReciboOficial")
                TipoNota = DtFactura.Rows(0).Item("TipoNota")
                NotaDebito = DtFactura.Rows(0).Item("NotaDebito")
                Dt.Dispose() : DtFactura.Dispose()
                Return True
            End If
        Next

        If Not PermisoTotal Then Dt.Dispose() : Return True

        Dt.Clear()
        If Not Tablas.Read(Sql, ConexionN, Dt) Then
            MsgBox("Error Al Leer ComproFacturados.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        For Each Row As DataRow In Dt.Rows
            DtFactura = New DataTable
            SqlFac = "SELECT ReciboOficial,NRel FROM FacturasProveedorCabeza WHERE EsReventa = 1 AND Estado <> 3 AND Factura = " & Row("Factura") & ";"
            If Not Tablas.Read(SqlFac, ConexionN, DtFactura) Then
                MsgBox("Error Al Leer Facturas Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If DtFactura.Rows.Count <> 0 Then
                FacturaN = Row("Factura")
                FacturaB = DtFactura.Rows(0).Item("Nrel")
                ReciboOficial = DtFactura.Rows(0).Item("ReciboOficial")
                Dt.Dispose() : DtFactura.Dispose()
                Return True
            End If
        Next

        Dt.Dispose() : DtFactura.Dispose()
        Return True

    End Function
    Private Function HallaImporteFacturaBlancoNegro(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal FacturaB As Double, ByVal FacturaN As Double, ByRef MontoBSinIva As Decimal, ByRef NetoN As Decimal, ByVal TipoNota As Integer, ByVal NotaDebito As Double) As Boolean

        Dim Dt As New DataTable
        Dim DtComproFacturados As New DataTable
        Dim ConexionCompro As String
        Dim FacturaCompro As Double
        Dim Sql As String

        MontoBSinIva = 0
        NetoN = 0

        If FacturaN <> 0 Then
            FacturaCompro = FacturaN
            ConexionCompro = ConexionN
        Else
            FacturaCompro = FacturaB
            ConexionCompro = Conexion
        End If

        Dim ImporteTotalLotes As Decimal
        Dim ImporteLote As Decimal
        Dim CoeficienteSinIva As Decimal

        If Not Tablas.Read("SELECT Lote,Secuencia,Importe FROM ComproFacturados WHERE Factura = " & FacturaCompro & ";", ConexionCompro, DtComproFacturados) Then Return False

        For Each Row As DataRow In DtComproFacturados.Rows
            If Row("Lote") = Lote And Row("Secuencia") = Secuencia Then
                ImporteLote = Row("Importe")
            End If
            ImporteTotalLotes = ImporteTotalLotes + Row("Importe")
        Next

        CoeficienteSinIva = Trunca(ImporteLote * 100 / ImporteTotalLotes)

        If FacturaB <> 0 Then
            Sql = "SELECT Impuesto,Importe FROM FacturasProveedorDetalle WHERE Factura = " & FacturaB & ";"
            If Not Tablas.Read(Sql, Conexion, Dt) Then
                MsgBox("Error Al Leer Facturas Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            'Calcula Totales de la FACTURA.
            For Each Row As DataRow In Dt.Rows
                Select Case Row("Impuesto")
                    Case 1, 2
                        MontoBSinIva = MontoBSinIva + Row("Importe")
                End Select
            Next
            If TipoNota <> 0 Then
                Dim Importe As Double = 0
                If Not HallaImporteSinIvaNotaDebitoCredito(TipoNota, NotaDebito, Importe) Then Return False
                MontoBSinIva = MontoBSinIva - Importe
            End If
        End If

        MontoBSinIva = MontoBSinIva * CoeficienteSinIva / 100

        If FacturaN <> 0 Then
            Sql = "SELECT Impuesto,Importe FROM FacturasProveedorDetalle WHERE Factura = " & FacturaN & ";"
            Dt.Clear()
            If Not Tablas.Read(Sql, ConexionN, Dt) Then
                MsgBox("Error Al Leer Facturas Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            'Calcula Totales de la FACTURA.
            For Each Row As DataRow In Dt.Rows
                Select Case Row("Impuesto")
                    Case 2
                        NetoN = NetoN + Row("Importe")
                End Select
            Next
        End If

        NetoN = NetoN * CoeficienteSinIva / 100

        Dt.Dispose()

        Return True

    End Function
    Private Function HallaImporteSinIvaNotaDebitoCredito(ByVal TipoNota As Integer, ByVal NotaDebito As Double, ByRef Importe As Decimal) As Boolean

        Dim Sql As String

        Importe = 0

        Try
            Sql = "SELECT SUM(Neto) FROM RecibosDetallePago WHERE TipoNota = " & TipoNota & " AND Nota = " & NotaDebito & " AND MedioPago = 100;"
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Resul = Cmd.ExecuteScalar()
                    If Not IsDBNull(Resul) Then
                        Importe = CDec(Resul)
                    Else
                        Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer RecibosDetallePago.", MsgBoxStyle.Critical)
            Return False
        End Try

        Return True

    End Function
    Private Function HallaImporteLiquidacionBlancoNegro(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal LiquidacionB As Double, ByVal LiquidacionN As Double, ByRef MontoBSinIva As Double, ByRef NetoN As Double, ByRef ComisionDescargaB As Double) As Boolean

        Dim Dt As New DataTable
        Dim DtLiquidacionDetalle As New DataTable
        Dim Sql As String

        MontoBSinIva = 0
        NetoN = 0
        ComisionDescargaB = 0

        Dim LiquidacionCompro As Double
        Dim ConexionCompro As String

        If LiquidacionB <> 0 Then
            LiquidacionCompro = LiquidacionB
            ConexionCompro = Conexion
        Else
            LiquidacionCompro = LiquidacionN
            ConexionCompro = ConexionN
        End If

        Dim ImporteTotalLotes As Double
        Dim ImporteLote As Double
        Dim CoeficienteSinIva As Double
        If Not Tablas.Read("SELECT Lote,Secuencia,PrecioS,Cantidad FROM LiquidacionDetalle WHERE Liquidacion = " & LiquidacionCompro & ";", ConexionCompro, DtLiquidacionDetalle) Then Return False
        For Each Row As DataRow In DtLiquidacionDetalle.Rows
            If Row("Lote") = Lote And Row("Secuencia") = Secuencia Then
                ImporteLote = Row("Cantidad") * Row("PrecioS")
            End If
            ImporteTotalLotes = ImporteTotalLotes + Row("Cantidad") * Row("PrecioS")
        Next
        CoeficienteSinIva = Trunca(ImporteLote * 100 / ImporteTotalLotes)

        If LiquidacionB <> 0 Then
            Sql = "SELECT Concepto,Importe FROM LiquidacionDetalleConceptos WHERE Liquidacion = " & LiquidacionB & ";"
            If Not Tablas.Read(Sql, Conexion, Dt) Then
                MsgBox("Error Al Leer Facturas Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            'Calcula Totales de la liquidacion.
            For Each Row As DataRow In Dt.Rows
                Select Case Row("Concepto")
                    Case 1
                        MontoBSinIva = MontoBSinIva + Row("Importe")
                    Case 3, 4
                        ComisionDescargaB = ComisionDescargaB + Row("Importe")
                End Select
            Next
        End If

        If LiquidacionN <> 0 Then
            Dt.Clear()
            Sql = "SELECT Concepto,Importe FROM LiquidacionDetalleConceptos WHERE Liquidacion = " & LiquidacionN & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then
                MsgBox("Error Al Leer Facturas Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            'Calcula Totales de la liquidacion.
            For Each Row As DataRow In Dt.Rows
                Select Case Row("Concepto")
                    Case 1
                        NetoN = NetoN + Row("Importe")
                    Case 3, 4
                        NetoN = NetoN - Row("Importe")
                End Select
            Next
        End If

        MontoBSinIva = MontoBSinIva * CoeficienteSinIva / 100
        ComisionDescargaB = ComisionDescargaB * CoeficienteSinIva / 100
        NetoN = NetoN * CoeficienteSinIva / 100

        Dt.Dispose()

        Return True

    End Function
    Private Function HallaImporteNVLPBlancoNegro(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Liquidacion As Double, ByVal ConexionStr As String, ByRef MontoBSinIva As Decimal, ByRef NetoN As Decimal, ByRef ComisionDescargaB As Decimal) As Boolean

        Dim Dt As New DataTable
        Dim Sql As String

        MontoBSinIva = 0
        NetoN = 0
        ComisionDescargaB = 0

        Sql = "SELECT Impuesto,Importe FROM NVLPDetalle WHERE Liquidacion = " & Liquidacion & ";"
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then Return False
        'Calcula Totales de la liquidacion.
        For Each Row As DataRow In Dt.Rows
            If ConexionStr = Conexion Then
                Select Case Row("Impuesto")
                    Case 1, 2
                        MontoBSinIva = MontoBSinIva + Row("Importe")
                    Case 5, 7, 9, 10
                        ComisionDescargaB = ComisionDescargaB + Row("Importe")
                End Select
            Else
                Select Case Row("Impuesto")
                    Case 2
                        NetoN = NetoN + Row("Importe")
                    Case 5, 7, 9, 10
                        NetoN = NetoN - Row("Importe")
                End Select
            End If
        Next

        Dim ImporteTotalLotes As Decimal = 0
        Dim ImporteLote As Decimal = 0
        Dim CoeficienteSinIva As Decimal = 0

        Dt.Clear()
        If Not Tablas.Read("SELECT Lote,Secuencia,Importe FROM NVLPLotes WHERE Liquidacion = " & Liquidacion & ";", ConexionStr, Dt) Then Return False

        For Each Row As DataRow In Dt.Rows
            If Row("Lote") = Lote And Row("Secuencia") = Secuencia Then
                ImporteLote = Row("Importe")
            End If
            ImporteTotalLotes = ImporteTotalLotes + Row("Importe")
        Next
        CoeficienteSinIva = Trunca(ImporteLote * 100 / ImporteTotalLotes)
        MontoBSinIva = MontoBSinIva * CoeficienteSinIva / 100
        ComisionDescargaB = ComisionDescargaB * CoeficienteSinIva / 100
        NetoN = NetoN * CoeficienteSinIva / 100

        Dt.Dispose()
        Return True

    End Function
    Public Function HallaArticuloYKXEnvase(ByVal Clave As Integer) As String

        Dim Dt As New DataTable
        Dim Senia As Double = 0
        Dim Nombre As String = ""

        Dt = Tablas.Leer("SELECT A.Nombre,E.Kilos FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave WHERE A.Clave = " & Clave & ";")
        If Dt.Rows.Count = 0 Then Dt.Dispose() : Return ""

        Nombre = Dt.Rows(0).Item("Nombre") & " (Kg:" & Dt.Rows(0).Item("Kilos") & ")"

        Dt.Dispose()

        Return Nombre

    End Function
    Private Sub HallaImportesDetalleNotaDebitoCredito(ByVal Operacion As Integer, ByVal TipoNota As Integer, ByVal Nota As Double, ByVal DtFormaPago As DataTable, ByRef NetoGrabado As Decimal, ByRef NetoNoGrabado As Decimal, ByRef ImporteRetencion As Decimal, ByRef ImporteIva As Decimal, ByVal Cambio As Decimal)

        Dim Sql As String
        Dim Dt As New DataTable
        Dim RowsBusqueda() As DataRow
        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        NetoGrabado = 0
        NetoNoGrabado = 0
        ImporteRetencion = 0
        ImporteIva = 0

        Sql = "SELECT MedioPago,Neto,Alicuota,Importe FROM RecibosDetallePago WHERE TipoNota = " & TipoNota & " AND Nota = " & Nota & ";"
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Select Case Row("MedioPago")
                Case 100
                    If Row("Alicuota") <> 0 Then
                        NetoGrabado = NetoGrabado + CalculaNeto(Row("Neto"), Cambio)
                        ImporteIva = ImporteIva + CalculaNeto(CalculaIva(1, Row("Neto"), Row("Alicuota")), Cambio)
                    Else
                        NetoNoGrabado = NetoNoGrabado + CalculaNeto(Row("Neto"), Cambio)
                    End If
                Case Else
                    RowsBusqueda = DtFormaPago.Select("Clave = " & Row("MedioPago"))
                    If RowsBusqueda(0).Item("Tipo") = 25 Then
                        ImporteRetencion = ImporteRetencion + CalculaNeto(Row("Importe"), Cambio)
                    End If
            End Select
        Next

    End Sub
    Private Function HallaRemitosFactura(ByVal Factura As Double, ByVal Operacion As Integer, ByVal Relacionada As Double) As String

        Dim Dt As New DataTable
        Dim Str As String = ""

        Dim Sql As String = "SELECT Remito FROM RemitosCabeza WHERE Estado <> 3 AND Factura = " & Factura & ";"

        If Operacion = 1 Then
            If Not Tablas.Read(Sql, Conexion, Dt) Then End
        Else
            If Relacionada <> 0 Then
                Sql = "SELECT Remito FROM RemitosCabeza WHERE Estado <> 3 AND Factura = " & Relacionada & ";"
                If Not Tablas.Read(Sql, Conexion, Dt) Then End
            Else
                If Not Tablas.Read(Sql, ConexionN, Dt) Then End 'caso en que es 100% en negro.
            End If
        End If

        For Each Row As DataRow In Dt.Rows
            Str = Str & NumeroEditado(Row("Remito")) & " "
        Next

        Return Str

    End Function
    Private Function HallaArticuloRemitoNVLP(ByVal Remito As Double, ByVal Indice As Integer) As Integer

        Dim Sql As String = "SELECT Articulo FROM RemitosDetalle WHERE Remito = " & Remito & " AND Indice = " & Indice & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: RemitosDetalle.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Private Sub HallaEnvaseYDuenio(ByVal Articulo As Integer, ByVal DtDuenios As DataTable, ByRef Duenio As Integer, ByRef NombreDuenio As String)

        Dim Dt As New DataTable
        Dim RowsBusqueda() As DataRow

        If Not Tablas.Read("SELECT E.Dueño FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave WHERE A.Clave = " & Articulo & ";", Conexion, Dt) Then End
        If Dt.Rows.Count = 0 Then
            Duenio = 0
        Else
            Duenio = Dt.Rows(0).Item("Dueño")
        End If
        RowsBusqueda = DtDuenios.Select("Tipo = " & Duenio)
        NombreDuenio = RowsBusqueda(0).Item("Nombre")

        Dt.Dispose()

    End Sub
    Private Sub AgregaComprobantes(ByVal ExHoja As Microsoft.Office.Interop.Excel.Worksheet, ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef RowExxel As Integer, ByVal Abierto As Boolean, ByVal Cerrado As Boolean, ByVal ConIva As Boolean)

        Dim Dt As New DataTable
        Dim SqlB As String, SqlN As String
        Dim SqlLote = " AND C.Lote = " & Lote & " AND C.Secuencia = " & Secuencia

        Dim SqlBenN As String
        SqlBenN = "SELECT 1 AS Operacion,1 As Tipo,F.Factura AS Comprobante,F.Proveedor,F.Fecha,F.ReciboOficial,F.NRel,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva FROM ComproFacturados As C INNER JOIN FacturasProveedorCabeza AS F ON C.Factura = F.Factura AND F.EsAfectaCostoLotes = 1 WHERE F.Estado = 1 AND NRel <> 0" & SqlLote & ";"


        SqlB = "SELECT 1 AS Operacion,1 As Tipo,F.Factura AS Comprobante,F.Proveedor,F.Fecha,F.ReciboOficial,F.NRel,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva FROM ComproFacturados As C INNER JOIN FacturasProveedorCabeza AS F ON C.Factura = F.Factura AND F.EsAfectaCostoLotes = 1 WHERE F.Estado = 1 AND F.Rel = 0" & SqlLote & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,2 As Tipo,F.Factura AS Comprobante,F.Proveedor,F.Fecha,F.Comprobante AS ReciboOficial,0 AS NRel,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva FROM OtrasFacturasLotes As C INNER JOIN OtrasFacturasCabeza AS F ON C.Factura = F.Factura WHERE F.Estado = 1" & SqlLote & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,3 As Tipo,F.Nota AS Comprobante,F.Cliente AS Proveedor,F.Fecha,F.Factura AS ReciboOficial,0 AS NRel,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva FROM ReintegrosLotes As C INNER JOIN ReintegrosCabeza AS F ON C.Nota = F.Nota WHERE F.Estado = 1" & SqlLote & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,F.TipoNota As Tipo,F.Nota AS Comprobante,F.Emisor AS Proveedor,F.Fecha,F.ReciboOficial,0 AS NRel,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva FROM RecibosLotes As C INNER JOIN RecibosCabeza AS F ON C.Nota = F.Nota AND C.TipoNota = F.TipoNota WHERE F.Estado = 1" & SqlLote & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,4 As Tipo,F.Consumo AS Comprobante,0 AS Proveedor,F.Fecha,0 AS ReciboOficial,0 AS NRel,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva FROM ConsumosLotes As C INNER JOIN ConsumosCabeza AS F ON C.Consumo = F.Consumo WHERE F.Estado = 1" & SqlLote & ";"


        SqlN = "SELECT 2 AS Operacion,1 As Tipo,F.Factura AS Comprobante,F.Proveedor,F.Fecha,F.ReciboOficial,F.NRel,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva FROM ComproFacturados As C INNER JOIN FacturasProveedorCabeza AS F ON C.Factura = F.Factura AND F.EsAfectaCostoLotes = 1 WHERE F.NRel = 0 AND F.Estado = 1" & SqlLote & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,2 As Tipo,F.Factura AS Comprobante,F.Proveedor,F.Fecha,F.Comprobante AS ReciboOficial,0 AS NRel,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva FROM OtrasFacturasLotes As C INNER JOIN OtrasFacturasCabeza AS F ON C.Factura = F.Factura WHERE F.Estado = 1" & SqlLote & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,F.TipoNota As Tipo,F.Nota AS Comprobante,F.Emisor AS Proveedor,F.Fecha,F.ReciboOficial,0 AS NRel,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva FROM RecibosLotes As C INNER JOIN RecibosCabeza AS F ON C.Nota = F.Nota AND C.TipoNota = F.TipoNota WHERE F.Estado = 1" & SqlLote & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,4 As Tipo,F.Consumo AS Comprobante,0 AS Proveedor,F.Fecha,0 AS ReciboOficial,0 AS NRel,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva FROM ConsumosLotes As C INNER JOIN ConsumosCabeza AS F ON C.Consumo = F.Consumo WHERE F.Estado = 1" & SqlLote & ";"

        If Abierto Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then End
            If PermisoTotal Then
                If Not Tablas.Read(SqlBenN, ConexionN, Dt) Then End
            End If
        End If
        If Cerrado Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then End
        End If

        If Dt.Rows.Count = 0 Then
            Dt.Dispose()
            Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Lote,Secuencia,Fecha"

        Dim CostoLote As Decimal = 0

        Dim ImporteFacturaB As Decimal = 0
        Dim ImporteFacturaN As Decimal = 0
        Dim Importe As Decimal = 0

        For Each Row As DataRow In Dt.Rows
            RowExxel = RowExxel + 1
            If ConIva Then
                ImporteFacturaB = Row("ImporteConIva")
                ImporteFacturaN = Row("ImporteSinIva")
                Importe = Row("ImporteConIva")
            Else
                If Row("Tipo") = 1 Then
                    If Row("NRel") <> 0 Then
                        ImporteFacturaB = HallaImporteSinIvaFacturaProveedorB(Row("NRel"))
                        ImporteFacturaN = Row("ImporteSinIva")
                    Else
                        ImporteFacturaB = HallaImporteSinIvaFacturaProveedorB(Row("Comprobante"))
                        ImporteFacturaN = Row("ImporteSinIva")
                    End If
                Else
                    Importe = Row("ImporteSinIva")
                End If
            End If
            ' 
            If Row("NRel") <> 0 Then   'Esto solo sucede si es una factura proveedor con parte en negro.
                If Abierto Then   'Esto solo sucede si es una factura proveedor con parte en negro.
                    ExHoja.Cells.Item(RowExxel, 5) = " "
                    ExHoja.Cells.Item(RowExxel, 6) = "Factura"
                    ExHoja.Cells.Item(RowExxel, 7) = NumeroEditado(Row("NRel"))
                    ExHoja.Cells.Item(RowExxel, 8) = NombreProveedor(Row("Proveedor"))
                    ExHoja.Cells.Item(RowExxel, 9) = NumeroEditado(Row("ReciboOficial"))
                    ExHoja.Cells.Item(RowExxel, 10) = Format(Row("Fecha"), "MM/dd/yyyy")
                    ExHoja.Cells.Item(RowExxel, 11) = ImporteFacturaB
                    ExHoja.Cells.Item(RowExxel, 15) = ImporteFacturaB
                    CostoLote = CostoLote + ImporteFacturaB
                    If Cerrado Then
                        ExHoja.Cells.Item(RowExxel, 12) = "X"
                        ExHoja.Cells.Item(RowExxel, 13) = NumeroEditado(Row("Comprobante"))
                        ExHoja.Cells.Item(RowExxel, 14) = ImporteFacturaN
                        ExHoja.Cells.Item(RowExxel, 15) = ImporteFacturaB + ImporteFacturaN
                        CostoLote = CostoLote + ImporteFacturaN
                    End If
                End If
            Else
                If Row("Operacion") = 2 Then ExHoja.Cells.Item(RowExxel, 5) = "X"
                If Row("Tipo") = 1 Then
                    ExHoja.Cells.Item(RowExxel, 6) = "Factura"
                    ExHoja.Cells.Item(RowExxel, 8) = NombreProveedor(Row("Proveedor"))
                End If
                If Row("Tipo") = 2 Then
                    ExHoja.Cells.Item(RowExxel, 6) = "Factura Otros Proveedores"
                    ExHoja.Cells.Item(RowExxel, 8) = NombreDestino(Row("Proveedor"))
                End If
                If Row("Tipo") = 3 Then
                    ExHoja.Cells.Item(RowExxel, 6) = "Reintegro"
                    ExHoja.Cells.Item(RowExxel, 8) = NombreAduana(Row("Proveedor"))
                    Importe = -Importe
                End If
                If Row("Tipo") = 4 Then
                    ExHoja.Cells.Item(RowExxel, 6) = "Consumo"
                End If
                If Row("Tipo") = 6 Then
                    ExHoja.Cells.Item(RowExxel, 6) = "Debito Financiero"
                    ExHoja.Cells.Item(RowExxel, 8) = NombreProveedor(Row("Proveedor"))
                    Importe = -Importe
                End If
                If Row("Tipo") = 8 Then
                    ExHoja.Cells.Item(RowExxel, 6) = "Crédito Financiero"
                    ExHoja.Cells.Item(RowExxel, 8) = NombreProveedor(Row("Proveedor"))
                End If
                If Row("Tipo") = 500 Then
                    ExHoja.Cells.Item(RowExxel, 6) = "Debito Del Proveedor"
                    ExHoja.Cells.Item(RowExxel, 8) = NombreProveedor(Row("Proveedor"))
                End If
                If Row("Tipo") = 700 Then
                    ExHoja.Cells.Item(RowExxel, 6) = "Crédito Del Proveedor"
                    ExHoja.Cells.Item(RowExxel, 8) = NombreProveedor(Row("Proveedor"))
                    Importe = -Importe
                End If
                ExHoja.Cells.Item(RowExxel, 7) = NumeroEditado(Row("Comprobante"))
                If Row("ReciboOficial") <> 0 Then ExHoja.Cells.Item(RowExxel, 9) = NumeroEditado(Row("ReciboOficial"))
                ExHoja.Cells.Item(RowExxel, 10) = Format(Row("Fecha"), "MM/dd/yyyy")
                If Row("Tipo") = 1 And Row("Operacion") = 1 Then ExHoja.Cells.Item(RowExxel, 11) = ImporteFacturaB : ExHoja.Cells.Item(RowExxel, 15) = ImporteFacturaB : CostoLote = CostoLote + ImporteFacturaB
                If Row("Tipo") = 1 And Row("Operacion") = 2 Then ExHoja.Cells.Item(RowExxel, 11) = ImporteFacturaN : ExHoja.Cells.Item(RowExxel, 15) = ImporteFacturaN : CostoLote = CostoLote + ImporteFacturaN
                If Row("Tipo") <> 1 Then ExHoja.Cells.Item(RowExxel, 11) = Importe : ExHoja.Cells.Item(RowExxel, 15) = Importe : CostoLote = CostoLote + Importe
            End If
        Next

        Dt.Dispose()
        View.Dispose()

        ExHoja.Cells.Item(RowExxel, 16) = CostoLote

    End Sub
    Public Sub InformeOrdenesDePago(ByVal Tipo As Integer, ByVal Proveedor As Integer, ByVal Empleado As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Estado As Integer)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------
        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = ""
        exHoja.Cells.Item(RowExxel, 2) = "Tipo"
        exHoja.Cells.Item(RowExxel, 3) = "Comprobante"
        exHoja.Cells.Item(RowExxel, 4) = ""
        exHoja.Cells.Item(RowExxel, 5) = "Fecha"
        exHoja.Cells.Item(RowExxel, 6) = "Imp.Total"
        exHoja.Cells.Item(RowExxel, 7) = "Estado"
        exHoja.Cells.Item(RowExxel, 8) = "Medios Pago"
        exHoja.Cells.Item(RowExxel, 9) = "Banco"
        exHoja.Cells.Item(RowExxel, 10) = "Cuenta"
        exHoja.Cells.Item(RowExxel, 11) = "Comprobante"
        exHoja.Cells.Item(RowExxel, 12) = "Fecha M.Pago"
        exHoja.Cells.Item(RowExxel, 13) = "Nro.Cheque"
        exHoja.Cells.Item(RowExxel, 14) = "Venc.Cheque"
        exHoja.Cells.Item(RowExxel, 15) = "Importe"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim SqlFecha As String = ""
        SqlFecha = " AND C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "

        Dim SqlProveedor As String = ""
        If Proveedor <> 0 Then SqlProveedor = " AND Emisor = " & Proveedor
        Dim SqlProveedorOtros As String = ""
        If Proveedor <> 0 Then SqlProveedorOtros = " AND Proveedor = " & Proveedor
        Dim SqlEmpleado As String = ""
        If Empleado <> 0 Then SqlEmpleado = " AND Legajo = " & Empleado
        Dim SqlEstado As String = ""
        If Estado <> 0 Then SqlEstado = " AND Estado = " & Estado
        Dim TipoStr As String
        Dim EstadoStr As String
        Dim OperacionAnt As Integer
        Dim TipoAnt As Integer
        Dim ComprobanteAnt As Decimal

        Dim Dt As New DataTable
        If Tipo = 600 Or Tipo = 0 Then
            If Not Tablas.Read("SELECT 1 As Operacion,C.TipoNota AS Tipo,C.Nota AS Comprobante,C.Emisor,C.Fecha,C.Importe,C.Estado,C.Tr,D.MedioPago,D.Banco,D.Cuenta,D.FechaComprobante,D.Comprobante AS ComprobanteMP,D.Importe AS ImporteMP,D.Cambio,D.ClaveCheque,CH.Fecha AS FechaCH,CH.Numero AS Numero FROM RecibosCabeza AS C INNER JOIN (RecibosDetallePago AS D LEFT JOIN Cheques AS CH ON D.ClaveCheque = CH.ClaveCheque AND D.MedioPago = CH.MedioPago) ON C.Nota = D.Nota AND C.TipoNota = D.TipoNota WHERE C.Tr = 0 AND C.TipoNota = 600" & SqlFecha & SqlProveedor & SqlEstado, Conexion, Dt) Then Exit Sub
            If PermisoTotal Then
                If Not Tablas.Read("SELECT 2 As Operacion,C.TipoNota AS Tipo,C.Nota AS Comprobante,C.Emisor,C.Fecha,C.Importe,C.Estado,C.Tr,D.MedioPago,D.Banco,D.Cuenta,D.FechaComprobante,D.Comprobante AS ComprobanteMP,D.Importe AS ImporteMP,D.Cambio,D.ClaveCheque,CH.Fecha AS FechaCH,CH.Numero AS Numero FROM RecibosCabeza AS C INNER JOIN (RecibosDetallePago AS D LEFT JOIN Cheques AS CH ON D.ClaveCheque = CH.ClaveCheque AND D.MedioPago = CH.MedioPago) ON C.Nota = D.Nota AND C.TipoNota = D.TipoNota WHERE C.TipoNota = 600" & SqlFecha & SqlProveedor & SqlEstado, ConexionN, Dt) Then Exit Sub
            End If
        End If
        If (Tipo = 601 Or Tipo = 0) And PermisoTotal Then
            If Not Tablas.Read("SELECT 1 As Operacion,C.TipoNota AS Tipo,C.Nota AS Comprobante,C.Emisor,C.Fecha,C.Importe,C.Estado,C.Tr,D.MedioPago,D.Banco,D.Cuenta,D.FechaComprobante,D.Comprobante AS ComprobanteMP,D.Importe AS ImporteMP,D.Cambio,D.ClaveCheque,CH.Fecha AS FechaCH,CH.Numero AS Numero FROM RecibosCabeza AS C INNER JOIN (RecibosDetallePago AS D LEFT JOIN Cheques AS CH ON D.ClaveCheque = CH.ClaveCheque AND D.MedioPago = CH.MedioPago) ON C.Nota = D.Nota AND C.TipoNota = D.TipoNota WHERE C.Tr = 1 AND C.TipoNota = 600" & SqlFecha & SqlProveedor & SqlEstado, Conexion, Dt) Then Exit Sub
        End If
        If Tipo = 5010 Or Tipo = 0 Then
            If Not Tablas.Read("SELECT 1 As Operacion,C.TipoNota AS Tipo,C.Movimiento AS Comprobante,C.Proveedor AS Emisor,C.Fecha,C.Importe,C.Estado,0 AS Tr,D.MedioPago,D.Banco,D.Cuenta,D.FechaComprobante,D.Comprobante AS ComprobanteMP,D.Importe AS ImporteMP,D.Cambio,D.ClaveCheque,CH.Fecha AS FechaCH,CH.Numero AS Numero FROM OtrosPagosCabeza AS C INNER JOIN (OtrosPagosPago AS D LEFT JOIN Cheques AS CH ON D.ClaveCheque = CH.ClaveCheque AND D.MedioPago = CH.MedioPago) ON C.Movimiento = D.Movimiento AND C.TipoNota = 5010 WHERE C.TipoNota = 5010" & SqlFecha & SqlProveedorOtros & SqlEstado, Conexion, Dt) Then Exit Sub
            If PermisoTotal Then
                If Not Tablas.Read("SELECT 2 As Operacion,C.TipoNota AS Tipo,C.Movimiento AS Comprobante,C.Proveedor AS Emisor,C.Fecha,C.Importe,C.Estado,0 AS Tr,D.MedioPago,D.Banco,D.Cuenta,D.FechaComprobante,D.Comprobante AS ComprobanteMP,D.Importe AS ImporteMP,D.Cambio,D.ClaveCheque,CH.Fecha AS FechaCH,CH.Numero AS Numero FROM OtrosPagosCabeza AS C INNER JOIN (OtrosPagosPago AS D LEFT JOIN Cheques AS CH ON D.ClaveCheque = CH.ClaveCheque AND D.MedioPago = CH.MedioPago) ON C.Movimiento = D.Movimiento AND C.TipoNota = 5010 WHERE C.TipoNota = 5010" & SqlFecha & SqlProveedorOtros & SqlEstado, ConexionN, Dt) Then Exit Sub
            End If
        End If
        If Tipo = 4010 Or Tipo = 0 Then
            If Not Tablas.Read("SELECT 1 As Operacion,C.TipoNota AS Tipo,C.Movimiento AS Comprobante,C.Legajo AS Emisor,C.Fecha,C.Importe,C.Estado,0 AS Tr,D.MedioPago,D.Banco,D.Cuenta,D.FechaComprobante,D.Comprobante AS ComprobanteMP,D.Importe AS ImporteMP,D.Cambio,D.ClaveCheque,CH.Fecha AS FechaCH,CH.Numero AS Numero FROM SueldosMovimientoCabeza AS C INNER JOIN (SueldosMovimientoPago AS D LEFT JOIN Cheques AS CH ON D.ClaveCheque = CH.ClaveCheque AND D.MedioPago = CH.MedioPago) ON C.Movimiento = D.Movimiento AND C.TipoNota = 4010 WHERE C.TipoNota = 4010" & SqlFecha & SqlEmpleado & SqlEstado, Conexion, Dt) Then Exit Sub
            If PermisoTotal Then
                If Not Tablas.Read("SELECT 2 As Operacion,C.TipoNota AS Tipo,C.Movimiento AS Comprobante,C.Legajo AS Emisor,C.Fecha,C.Importe,C.Estado,0 AS Tr,D.MedioPago,D.Banco,D.Cuenta,D.FechaComprobante,D.Comprobante AS ComprobanteMP,D.Importe AS ImporteMP,D.Cambio,D.ClaveCheque,CH.Fecha AS FechaCH,CH.Numero AS Numero FROM SueldosMovimientoCabeza AS C INNER JOIN (SueldosMovimientoPago AS D LEFT JOIN Cheques AS CH ON D.ClaveCheque = CH.ClaveCheque AND D.MedioPago = CH.MedioPago) ON C.Movimiento = D.Movimiento AND C.TipoNota = 4010 WHERE C.TipoNota = 4010" & SqlFecha & SqlEmpleado & SqlEstado, ConexionN, Dt) Then Exit Sub
            End If
        End If

        Dim Row As DataRow
        Dim Total As Decimal

        Dim DtC As New DataTable
        For Each Row In Dt.Rows
            If Row("Tr") And Row("ClaveCheque") <> 0 Then
                If Not Tablas.Read("SELECT Fecha,Numero FROM Cheques WHERE ClaveCheque = " & Row("ClaveCheque") & " AND MedioPago = " & Row("MedioPago") & ";", ConexionN, DtC) Then Exit Sub
                If DtC.Rows.Count <> 0 Then
                    Row("FechaCH") = DtC.Rows(0).Item("Fecha")
                    Row("Numero") = DtC.Rows(0).Item("Numero")
                End If
            End If
        Next
        DtC.Dispose()

        For Each Row In Dt.Rows
            If Row("Operacion") & Format(Row("Tipo"), "0000") & Format(Row("Comprobante"), "00000000") <> OperacionAnt & Format(TipoAnt, "0000") & Format(ComprobanteAnt, "00000000") Then
                TipoStr = "" : EstadoStr = ""
                If Row("Tipo") = 600 And Not Row("Tr") Then TipoStr = "O.Pago Proveedor"
                If Row("Tipo") = 600 And Row("Tr") Then TipoStr = "O.Pago Contable"
                If Row("Tipo") = 5010 Then TipoStr = "O.Pago Otros Proveedor"
                If Row("Tipo") = 4010 Then TipoStr = "O.Pago Sueldos"
                Select Case Row("Estado")
                    Case 1
                        EstadoStr = "Alta"
                    Case 3
                        EstadoStr = "Baja"
                End Select
                RowExxel = RowExxel + 1
                If Row("Operacion") = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
                exHoja.Cells.Item(RowExxel, 2) = TipoStr
                exHoja.Cells.Item(RowExxel, 3) = NumeroEditado(Row("Comprobante"))
                Select Case Row("Tipo")
                    Case 600
                        exHoja.Cells.Item(RowExxel, 4) = NombreProveedor(Row("Emisor"))
                    Case 4010
                        If Row("Operacion") = 1 Then
                            exHoja.Cells.Item(RowExxel, 4) = NombreLegajo(Row("Emisor"), Conexion)
                        Else
                            exHoja.Cells.Item(RowExxel, 4) = NombreLegajo(Row("Emisor"), ConexionN)
                        End If
                    Case 5010
                        exHoja.Cells.Item(RowExxel, 4) = NombreDestino(Row("Emisor"))
                End Select
                exHoja.Cells.Item(RowExxel, 5) = Format(Row("Fecha"), "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel, 6) = Row("Importe")
                Total = Total + Row("Importe")
                exHoja.Cells.Item(RowExxel, 7) = EstadoStr
                OperacionAnt = Row("Operacion") : TipoAnt = Row("Tipo") : ComprobanteAnt = Row("Comprobante")
                RowExxel = RowExxel - 1
            End If
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 8) = HallaNombreMedioPago(Row("MedioPago"))
            If Row("Banco") <> 0 Then exHoja.Cells.Item(RowExxel, 9) = NombreBanco(Row("Banco"))
            If Row("Cuenta") <> 0 Then exHoja.Cells.Item(RowExxel, 10) = Row("Cuenta")
            If Row("ComprobanteMP") <> 0 Then exHoja.Cells.Item(RowExxel, 11) = Row("ComprobanteMP")
            If Format(Row("FechaComprobante"), "dd/MM/yyy") <> "01/01/1800" Then exHoja.Cells.Item(RowExxel, 12) = Format(Row("FechaComprobante"), "MM/dd/yyyy")
            If Not IsDBNull(Row("Numero")) Then
                If Row("Numero") <> 0 Then exHoja.Cells.Item(RowExxel, 13) = Row("Numero")
                If Format(Row("FechaCH"), "dd/MM/yyyy") <> "01/01/1800" Then exHoja.Cells.Item(RowExxel, 14) = Format(Row("FechaCH"), "MM/dd/yyyy")
            End If
            If Row("Cambio") > 0 Then
                exHoja.Cells.Item(RowExxel, 15) = Row("ImporteMp") * Row("Cambio")
            Else
                exHoja.Cells.Item(RowExxel, 15) = Row("ImporteMp")
            End If
        Next
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 2) = "T O T A L"
        exHoja.Cells.Item(RowExxel, 15) = Total
        exHoja.Rows.Item(RowExxel).Font.Bold = True

        Dt.Dispose()

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        Rango.ColumnWidth = "20"
        '
        AA = Chr(65 + 1 - 1) & InicioDatos
        BB = Chr(65 + 1 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        Rango.ColumnWidth = "3"
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.ColumnWidth = "14"
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        Rango.ColumnWidth = "20"
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        Rango.ColumnWidth = "20"
        '
        AA = Chr(65 + 15 - 1) & InicioDatos
        BB = Chr(65 + 15 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        'Muestra titulos.
        exHoja.Cells.Item(1, 11) = "Fecha Proceso " & Date.Now
        exHoja.Rows.Item(2).Font.Bold = True
        exHoja.Cells.Item(2, 1) = "Comprobantes de Pagos desde el " & Format(Desde, "dd/MM/yyyy") & " Al " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

    End Sub
    Private Function HallaImporteSinIvaFacturaProveedorB(ByVal Factura As Decimal) As Decimal

        Dim Dt As New DataTable
        Dim ImporteSinIva As Decimal = 0

        Dim Sql As String = "SELECT Impuesto,Importe FROM FacturasProveedorDetalle WHERE Factura = " & Factura & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then
            MsgBox("Error Al Leer Facturas Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        'Calcula Totales de la FACTURA.
        For Each Row As DataRow In Dt.Rows
            Select Case Row("Impuesto")
                Case 1, 2
                    ImporteSinIva = ImporteSinIva + Row("Importe")
            End Select
        Next

        Dt.Dispose()

        Return ImporteSinIva

    End Function
    Private Function HallaCuentas(ByVal Tipo As Integer, ByVal Documento As Double, ByVal Operacion As Integer) As String

        Dim Sql As String = ""
        Dim ConexionStr As String
        Dim Dt As New DataTable
        Dim Str As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Select Case Tipo
            Case 6
                Sql = "SELECT C.Debito,D.Cuenta,D.Debe,D.Haber FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE (C.TipoDocumento = 6 OR C.TipoDocumento = 10006) AND Documento = " & Documento & ";"
            Case 8
                Sql = "SELECT C.Debito,D.Cuenta,D.Debe,D.Haber FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE (C.TipoDocumento = 8 OR C.TipoDocumento = 10008) AND Documento = " & Documento & ";"
            Case 500
                Sql = "SELECT C.Debito,D.Cuenta,D.Debe,D.Haber FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.TipoDocumento = 500 AND Documento = " & Documento & ";"
            Case 700
                Sql = "SELECT C.Debito,D.Cuenta,D.Debe,D.Haber FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.TipoDocumento = 700 AND Documento = " & Documento & ";"
            Case 2
                Sql = "SELECT C.Debito,D.Cuenta,D.Debe,D.Haber FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE (C.TipoDocumento = 900 OR C.TipoDocumento = 901 OR C.TipoDocumento = 902 OR C.TipoDocumento = 903 OR C.TipoDocumento = 7008) AND Documento = " & Documento & ";"
            Case 60000
                Sql = "SELECT C.Debito,D.Cuenta,D.Debe,D.Haber FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.TipoDocumento = 6000 AND Documento = " & Documento & ";"
            Case 61000
                Sql = "SELECT C.Debito,D.Cuenta,D.Debe,D.Haber FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.TipoDocumento = 61000 AND Documento = " & Documento & ";"
            Case 99000
                Sql = "SELECT C.Debito,D.Cuenta,D.Debe,D.Haber FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.Asiento = " & Documento & " AND C.TipoDocumento = 0 AND Documento = 0;"
        End Select

        If Not Tablas.Read(Sql, ConexionStr, Dt) Then End

        For Each Row As DataRow In Dt.Rows
            Dim Cuenta As String = Format(Row("Cuenta"), "000-000000-00")
            Select Case Tipo
                Case 8, 500, 2, 60000, 61000
                    If Row("Debe") <> 0 Then Str = Str & Cuenta & "/"
                Case 99000
                    If Row("Debito") And Row("Debe") <> 0 Then Str = Str & Cuenta & "/"
                    If Not Row("Debito") And Row("Haber") <> 0 Then Str = Str & Cuenta & "/"
                Case 6, 700
                    If Row("Haber") <> 0 Then Str = Str & Cuenta & "/"
                Case Else
                    Str = "No Habilitado para tipo comprobante " & Tipo
            End Select
        Next

        Dt.Dispose()

        Return Str

    End Function
    Private Function NombreComprobante(ByVal Tipo As Integer) As String

        Select Case Tipo
            Case 60
                Return "Cobranza"
            Case 50
                Return "Debito del Cliente"
            Case 7
                Return "Crédito a Cliente"
            Case 6
                Return "Debito a Proveedor"
            Case 600
                Return "Orden de Pago"
            Case 700
                Return "Crédito de Proveedor"
            Case 5010
                Return "Orden Pago Otros Proveedores"
            Case 5005
                Return "Debito Otros Proveedores"
        End Select

    End Function
    Public Sub GestionCompra(ByVal DesdeIngreso As Date, ByVal HastaIngreso As Date, ByVal DesdeFactura As Date, ByVal HastaFactura As Date, ByVal Domesticas As Boolean, ByVal Importacion As Boolean, ByVal Abierto As Boolean, ByVal Cerrado As Boolean)

        '            InformeComprasVentasPorLotes
        'Por ahora anulado.
        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim SqlFecha As String = ""
        SqlFecha = "AND F.Fecha >='" & Format(DesdeFactura, "yyyyMMdd") & "' AND F.Fecha < DATEADD(dd,1,'" & Format(HastaFactura, "yyyyMMdd") & "') "

        Dim SqlB As String
        SqlB = "SELECT 1 AS Operacion,1 AS Tipo,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva,C.Operacion AS OperacionLote,F.Factura,F.Rel,F.NRel,F.TipoNota,F.NotaDebito,0 AS Cantidad,0 AS Articulo FROM FacturasProveedorCabeza AS F INNER JOIN ComproFacturados AS C ON F.Factura = C.Factura WHERE F.Estado = 1 AND EsReventa = 1 " & SqlFecha & _
               " UNUION ALL " & _
               "SELECT 1 AS Operacion,2 AS Tipo,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva,C.Operacion AS OperacionLote,F.Liquidacion,0 AS Rel,0 AS NRel,0 AS TipoNota,0 AS NotaDebito,Cantidad,0 AS Articulo FROM LIquidacionCabeza AS F INNER JOIN LiquidacionDetalle AS C ON F.Liquidacion = C.Liquidacion WHERE Estado = 1 AND (EsReventa = 1 OR EsConsignacion = 1) " & SqlFecha & ";"
        Dim SqlBenN As String
        SqlBenN = "SELECT 12 AS Operacion,1 AS Tipo,C.Lote,C.Secuencia,0 AS ImporteConIva,0 AS ImporteSinIva,C.Operacion AS OperacionLote,F.Factura,F.Rel,F.NRel,F.TipoNota,F.NotaDebito,0 AS Cantidad,0 AS Articulo FROM FacturasProveedorCabeza AS F INNER JOIN ComproFacturados AS C ON F.Factura = C.Factura WHERE F.Estado = 1  AND EsReventa = 1 AND C.ImporteConIva <> C.ImporteSinIva " & SqlFecha & ";"
        Dim SqlN As String
        SqlN = "SELECT 2 AS Operacion,1 AS Tipo,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva,C.Operacion AS OperacionLote,F.Factura,F.Rel,F.NRel,F.TipoNota,F.NotaDebito,0 AS Cantidad,0 AS Articulo FROM FacturasProveedorCabeza AS F INNER JOIN ComproFacturados AS C ON F.Factura = C.Factura WHERE F.Estado = 1  AND EsReventa = 1 AND C.ImporteConIva = C.ImporteSinIva " & SqlFecha & _
               " UNUION ALL " & _
               "SELECT 2 AS Operacion,2 AS Tipo,C.Lote,C.Secuencia,C.ImporteConIva,C.ImporteSinIva,C.Operacion AS OperacionLote,F.Liquidacion,0 AS Rel,0 AS NRel,0 AS TipoNota,0 AS NotaDebito,Cantidad,0 AS Articulo FROM LIquidacionCabeza AS F INNER JOIN LiquidacionDetalle AS C ON F.Liquidacion = C.Liquidacion WHERE Estado = 1 AND (EsReventa = 1 OR EsConsignacion = 1) " & SqlFecha & ";"

        Dim DtFacturas As New DataTable

        If Abierto Then
            If Not Tablas.Read(SqlB, Conexion, DtFacturas) Then Exit Sub
            If PermisoTotal Then
                If Not Tablas.Read(SqlBenN, ConexionN, DtFacturas) Then Exit Sub
            End If
        End If
        If Cerrado Then
            If Not Tablas.Read(SqlN, ConexionN, DtFacturas) Then Exit Sub
        End If

        Dim DtLotes As New DataTable
        Dim StrLotes As String
        Dim StrConexion As String

        Dim SqlIngreso As String = ""
        SqlIngreso = " AND C.Fecha >='" & Format(DesdeIngreso, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(HastaIngreso, "yyyyMMdd") & "') "

        Dim StrTipo As String = ""
        If Domesticas Then
            StrTipo = " AND C.Moneda = 1"
        End If
        If Importacion Then
            StrTipo = " AND C.Moneda > 1"
        End If
        If Domesticas And Importacion Then
            StrTipo = ""
        End If

        Dim MontoBSinIva As Decimal = 0
        Dim NetoN As Decimal = 0

        For Each Row As DataRow In DtFacturas.Rows
            StrLotes = "SELECT L.Articulo,L.Cantidad,L.Baja FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote  WHERE L.Lote = " & Row("Lote") & " AND L.Secuencia = " & Row("Secuencia") & SqlIngreso & StrTipo & ";"
            If Row("OperacionLote") = 1 Then
                StrConexion = Conexion
            Else : StrConexion = ConexionN
            End If
            DtLotes.Clear()
            If Not Tablas.Read(StrLotes, StrConexion, DtLotes) Then Exit Sub
            If DtLotes.Rows.Count = 0 Then
                Row.Delete()
            Else
                Row("Articulo") = DtLotes.Rows(0).Item("Articulo")
                Select Case Row("Tipo")
                    Case 1
                        Row("Cantidad") = DtLotes.Rows(0).Item("Cantidad") - DtLotes.Rows(0).Item("Baja")
                        If Row("Operacion") = 12 Then
                            If Not HallaImporteFacturaBlancoNegro(Row("Lote"), Row("Secuencia"), Row("NRel"), Row("Factura"), MontoBSinIva, NetoN, Row("TipoNota"), Row("NotaDebito")) Then DtFacturas.Dispose() : Exit Sub

                        End If
                        If Row("Operacion") = 1 Then
                            If Not HallaImporteFacturaBlancoNegro(Row("Lote"), Row("Secuencia"), Row("Factura"), Row("Nrel"), MontoBSinIva, NetoN, Row("TipoNota"), Row("NotaDebito")) Then DtFacturas.Dispose() : Exit Sub
                        End If
                End Select

            End If
        Next

        DtLotes.Dispose()
        DtFacturas.AcceptChanges()

        If DtFacturas.Rows.Count = 0 Then
            MsgBox("No Hay Datos.")
            DtFacturas.Dispose()
            Exit Sub
        End If

        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Articulo          "
        exHoja.Cells.Item(RowExxel, 3) = "Cantidad"
        exHoja.Cells.Item(RowExxel, 4) = "Monto S/IVA "
        exHoja.Cells.Item(RowExxel, 5) = "Monto Final "
        exHoja.Cells.Item(RowExxel, 6) = "Precio Promedio S/IVA"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim View As New DataView
        View = DtFacturas.DefaultView
        View.Sort = "Articulo"

        Dim ArticuloAnt As Integer = View(0).Item("Articulo")
        Dim ImporteSinIva As Decimal = 0
        Dim ImporteConIva As Decimal = 0
        Dim Cantidad As Decimal = 0
        Dim Row1 As DataRowView

        For Each Row1 In View
            If Row1("Articulo") <> ArticuloAnt Then
                RowExxel = RowExxel + 1
                exHoja.Cells.Item(RowExxel, 2) = NombreArticulo(ArticuloAnt)
                exHoja.Cells.Item(RowExxel, 3) = Cantidad
                exHoja.Cells.Item(RowExxel, 4) = ImporteSinIva
                exHoja.Cells.Item(RowExxel, 5) = ImporteConIva
                exHoja.Cells.Item(RowExxel, 6) = Trunca(ImporteSinIva / Cantidad)
                ArticuloAnt = Row1("Articulo")
                Cantidad = 0 : ImporteSinIva = 0 : ImporteConIva = 0
            End If
            Cantidad = Cantidad + Row1("Cantidad")
            ImporteSinIva = ImporteSinIva + Row1("ImporteSinIva")
            ImporteConIva = ImporteConIva + Row1("ImporteConIva")
        Next
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 2) = NombreArticulo(ArticuloAnt)
        exHoja.Cells.Item(RowExxel, 3) = Cantidad
        exHoja.Cells.Item(RowExxel, 4) = ImporteSinIva
        exHoja.Cells.Item(RowExxel, 5) = ImporteConIva
        exHoja.Cells.Item(RowExxel, 6) = Trunca(ImporteSinIva / Cantidad)

        DtFacturas.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 1 - 1) & InicioDatos
        BB = Chr(65 + 1 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 23) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Gestión Compras"

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeComprasVentasPorArticulo(ByVal Proveedor As Integer, ByVal DesdeFactura As Date, ByVal HastaFactura As Date, ByVal DesdeIngreso As Date, ByVal HastaIngreso As Date, ByVal Domesticas As Boolean, ByVal Importacion As Boolean, ByVal Abierto As Boolean, ByVal Cerrado As Boolean)

        MsgBox("Preguntar a Gustavo por las Cantidades.")

        Dim DtAux As New DataTable
        ArmaArchivoAuxiliar(DtAux)

        If Not ArmaConFacturasProveedor(DtAux, DesdeFactura, HastaFactura, DesdeIngreso, HastaIngreso, Domesticas, Importacion) Then Exit Sub
        If Not ArmaConLIquidacionesProveedor(DtAux, DesdeFactura, HastaFactura, DesdeIngreso, HastaIngreso, Domesticas, Importacion) Then Exit Sub

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim View As New DataView
        View = DtAux.DefaultView
        View.Sort = "Articulo"

        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = "Articulo "
        exHoja.Cells.Item(RowExxel, 2) = "Monto S/IVA."
        exHoja.Cells.Item(RowExxel, 3) = "Comision/Descarga"
        exHoja.Cells.Item(RowExxel, 4) = "Total (2)"
        exHoja.Cells.Item(RowExxel, 5) = "Total"
        exHoja.Cells.Item(RowExxel, 6) = "Cantidad"
        exHoja.Cells.Item(RowExxel, 7) = "Pr.Promedio"
        exHoja.Cells.Item(RowExxel, 8) = " "
        exHoja.Cells.Item(RowExxel, 9) = "Monto S/IVA."
        exHoja.Cells.Item(RowExxel, 10) = "Comision/Descarga"
        exHoja.Cells.Item(RowExxel, 11) = "Total (2)"
        exHoja.Cells.Item(RowExxel, 12) = "Total"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim MontoBSinIvaT As Decimal = 0
        Dim NetoNT As Decimal = 0
        Dim ComisionDescargaBT As Decimal = 0

        For Each Row As DataRowView In View
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 1) = NombreArticulo(Row("Articulo"))
            exHoja.Cells.Item(RowExxel, 2) = Row("MontoBSinIva")
            exHoja.Cells.Item(RowExxel, 3) = -Row("ComisionDescargaB")
            exHoja.Cells.Item(RowExxel, 4) = Row("NetoN")
            exHoja.Cells.Item(RowExxel, 5) = Row("MontoBSinIva") + Row("NetoN") - Row("ComisionDescargaB")
            exHoja.Cells.Item(RowExxel, 6) = Row("Cantidad")
            If Row("Cantidad") <> 0 Then exHoja.Cells.Item(RowExxel, 7) = Trunca((Row("MontoBSinIva") + Row("NetoN") - Row("ComisionDescargaB")) / Row("Cantidad"))
            exHoja.Cells.Item(RowExxel, 8) = 0
            exHoja.Cells.Item(RowExxel, 9) = 0
            exHoja.Cells.Item(RowExxel, 10) = Row("MontoBSinIva") + Row("NetoN") - Row("ComisionDescargaB")
            MontoBSinIvaT = MontoBSinIvaT + Row("MontoBSinIva")
            NetoNT = NetoNT + Row("NetoN")
            ComisionDescargaBT = ComisionDescargaBT + Row("ComisionDescargaB")
        Next
        'Totales.
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 2) = MontoBSinIvaT
        exHoja.Cells.Item(RowExxel, 3) = -ComisionDescargaBT
        exHoja.Cells.Item(RowExxel, 4) = NetoNT
        exHoja.Cells.Item(RowExxel, 5) = MontoBSinIvaT - ComisionDescargaBT + NetoNT
        '     exHoja.Cells.Item(RowExxel, 15) = NetoNT
        '    exHoja.Cells.Item(RowExxel, 16) = MontoBSinIvaT - ComisionDescargaBT + NetoNT
        '   exHoja.Cells.Item(RowExxel, 18) = MontoFacturaBSinIvaT
        '  exHoja.Cells.Item(RowExxel, 19) = -ComisionDescargaFacturaBT
        ' exHoja.Cells.Item(RowExxel, 20) = NetoFacturaNT
        ' exHoja.Cells.Item(RowExxel, 21) = MontoFacturaBSinIvaT + NetoFacturaNT - ComisionDescargaFacturaBT

        DtAux.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 1 - 1) & InicioDatos
        BB = Chr(65 + 1 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 13 - 1) & InicioDatos
        BB = Chr(65 + 13 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 14 - 1) & InicioDatos
        BB = Chr(65 + 14 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 15 - 1) & InicioDatos
        BB = Chr(65 + 15 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 16 - 1) & InicioDatos
        BB = Chr(65 + 16 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 18 - 1) & InicioDatos
        BB = Chr(65 + 18 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 19 - 1) & InicioDatos
        BB = Chr(65 + 19 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 20 - 1) & InicioDatos
        BB = Chr(65 + 20 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 21 - 1) & InicioDatos
        BB = Chr(65 + 21 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Dibuja rectangulos.=------------------------------
        If InicioDatos < UltimaLinea - 1 Then
            AA = Chr(65 + 1 - 1) & InicioDatos
            '            BB = Chr(65 + 9 - 1) & UltimaLinea - 1
            BB = Chr(65 + 11 - 1) & UltimaLinea - 1
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
            '       AA = Chr(65 + 11 - 1) & InicioDatos
            AA = Chr(65 + 14 - 1) & InicioDatos
            BB = Chr(65 + 16 - 1) & UltimaLinea - 1
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
            AA = Chr(65 + 18 - 1) & InicioDatos
            BB = Chr(65 + 21 - 1) & UltimaLinea - 1
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
        End If
        '--------------------------------------------------

        If Not PermisoTotal Then exHoja.Columns(15).delete() : exHoja.Columns(19).delete()

        exHoja.Rows.Item(5).Font.Bold = True
        exHoja.Cells.Item(5, 12) = "        DETALLE DE COMPRA (Facturas Reventa, Liquidaciones)"
        exHoja.Cells.Item(5, 18) = "        DETALLE DE VENTA  (Facturas, NVLP)"

        'Muestra titulos.
        exHoja.Cells.Item(1, 23) = Format(Date.Now, "MM/dd/yyyy")
        '      If Nombre = "" Then
        '     exHoja.Cells.Item(2, 1) = "Gestión Compra/Venta de Lotes"
        '    Else
        '   exHoja.Cells.Item(2, 1) = "Gestión Compra/Venta de Lotes del Proveedor : " & Nombre
        '   End If

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub GeneraExcelCITICompras()

        Dim Directorio1 As String = ""
        Dim Directorio2 As String = ""

        Directorio1 = PideCarpeta()
        If Directorio1 = "" Then Exit Sub
        Directorio2 = Directorio1
        Directorio1 = Directorio1 & "\" & "REGINFO_CV_COMPRAS_CBTE.TXT"
        Directorio2 = Directorio2 & "\" & "REGINFO_CV_COMPRAS_ALICUOTAS.TXT"

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        exApp.DisplayAlerts = False  'para arreglar un error por valores multiples en celda. No supe por que. 

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        'Muestra titulos.
        exHoja.Cells.Item(1, 6) = Format(Date.Now, "MM/dd/yyyy")

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = "Fecha"
        exHoja.Cells.Item(RowExxel, 2) = "Tipo Compro."
        exHoja.Cells.Item(RowExxel, 3) = "Punto "
        exHoja.Cells.Item(RowExxel, 4) = "Nro.Compro."
        exHoja.Cells.Item(RowExxel, 5) = "Despacho "
        exHoja.Cells.Item(RowExxel, 6) = "Cód.Docu."
        exHoja.Cells.Item(RowExxel, 7) = "Numero Identif."
        exHoja.Cells.Item(RowExxel, 8) = "Nombre Vendedor"
        exHoja.Cells.Item(RowExxel, 9) = "Importe Total"
        exHoja.Cells.Item(RowExxel, 10) = "Neto No Grabado"
        exHoja.Cells.Item(RowExxel, 11) = "Neto Excento"
        exHoja.Cells.Item(RowExxel, 12) = "Percepciones "
        exHoja.Cells.Item(RowExxel, 13) = "Percepciones  "
        exHoja.Cells.Item(RowExxel, 14) = "Percepciones"
        exHoja.Cells.Item(RowExxel, 15) = "Percepciones "
        exHoja.Cells.Item(RowExxel, 16) = "Importe Impuestos "
        exHoja.Cells.Item(RowExxel, 17) = "Código Moneda"
        exHoja.Cells.Item(RowExxel, 18) = "Tipo "
        exHoja.Cells.Item(RowExxel, 19) = "Cantidad "
        exHoja.Cells.Item(RowExxel, 20) = "Código "
        exHoja.Cells.Item(RowExxel, 21) = "Crédito Fiscal"
        exHoja.Cells.Item(RowExxel, 22) = "Otros Tributos"
        exHoja.Cells.Item(RowExxel, 23) = "Cuit Emisor"
        exHoja.Cells.Item(RowExxel, 24) = "Denominación Emisor"
        exHoja.Cells.Item(RowExxel, 25) = "Iva Comisión"
        exHoja.Cells.Item(RowExxel, 26) = "Neto Grabado"
        exHoja.Cells.Item(RowExxel, 27) = "Alicuota "
        exHoja.Cells.Item(RowExxel, 28) = "Impuesto "
        exHoja.Cells.Item(RowExxel, 29) = "ORDEN"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 2

        RowExxel = 7
        exHoja.Cells.Item(RowExxel, 1) = ""
        exHoja.Cells.Item(RowExxel, 2) = ""
        exHoja.Cells.Item(RowExxel, 3) = "de Venta"
        exHoja.Cells.Item(RowExxel, 4) = ""
        exHoja.Cells.Item(RowExxel, 5) = "Impo."
        exHoja.Cells.Item(RowExxel, 6) = "Vendedor"
        exHoja.Cells.Item(RowExxel, 7) = "Vendedor"
        exHoja.Cells.Item(RowExxel, 8) = ""
        exHoja.Cells.Item(RowExxel, 9) = ""
        exHoja.Cells.Item(RowExxel, 10) = ""
        exHoja.Cells.Item(RowExxel, 11) = ""
        exHoja.Cells.Item(RowExxel, 12) = "Imp.Valor Agregado"
        exHoja.Cells.Item(RowExxel, 13) = "Imp.Nacionales"
        exHoja.Cells.Item(RowExxel, 14) = "Ingresos Brutos"
        exHoja.Cells.Item(RowExxel, 15) = "Imp.Municipales"
        exHoja.Cells.Item(RowExxel, 16) = "Internos"
        exHoja.Cells.Item(RowExxel, 17) = ""
        exHoja.Cells.Item(RowExxel, 18) = "Cambio"
        exHoja.Cells.Item(RowExxel, 19) = "Ivas"
        exHoja.Cells.Item(RowExxel, 20) = "Operación"
        exHoja.Cells.Item(RowExxel, 21) = "Computable"
        exHoja.Cells.Item(RowExxel, 22) = ""
        exHoja.Cells.Item(RowExxel, 23) = ""
        exHoja.Cells.Item(RowExxel, 24) = ""
        exHoja.Cells.Item(RowExxel, 25) = ""
        exHoja.Cells.Item(RowExxel, 26) = ""
        exHoja.Cells.Item(RowExxel, 27) = "IVA"
        exHoja.Cells.Item(RowExxel, 28) = "Liquidado"
        exHoja.Cells.Item(RowExxel, 29) = "En Afip"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 2

        Dim ColExxel As Integer = 8

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Linea As String
        Dim Total10 As Decimal = 0
        Dim Total16 As Decimal = 0
        Dim Total21 As Decimal = 0
        Dim Total26 As Decimal = 0
        Dim Total28 As Decimal = 0
        Dim Orden As Integer = 0

        Using reader As StreamReader = New StreamReader(Directorio1)
            Linea = reader.ReadLine
            Do While (Not Linea Is Nothing)
                RowExxel = RowExxel + 1
                exHoja.Cells.Item(RowExxel, 1) = Mid$(Linea, 7, 2) & "/" & Mid$(Linea, 5, 2) & "/" & Mid$(Linea, 1, 4)
                exHoja.Cells.Item(RowExxel, 2) = CodigoDocumentoCITI(Mid$(Linea, 9, 3))
                exHoja.Cells.Item(RowExxel, 3) = Mid$(Linea, 12, 5)
                exHoja.Cells.Item(RowExxel, 4) = Mid$(Linea, 17, 20)
                exHoja.Cells.Item(RowExxel, 5) = Mid$(Linea, 37, 16)
                exHoja.Cells.Item(RowExxel, 6) = Mid$(Linea, 53, 2)
                exHoja.Cells.Item(RowExxel, 7) = Mid$(Linea, 55, 20)
                exHoja.Cells.Item(RowExxel, 8) = Mid$(Linea, 75, 30)
                exHoja.Cells.Item(RowExxel, 9) = CDec(Mid$(Linea, 105, 15)) / 100
                exHoja.Cells.Item(RowExxel, 10) = CDec(Mid$(Linea, 120, 15)) / 100
                Total10 = Total10 + CDec(Mid$(Linea, 120, 15)) / 100
                exHoja.Cells.Item(RowExxel, 11) = CDec(Mid$(Linea, 135, 15)) / 100
                exHoja.Cells.Item(RowExxel, 12) = CDec(Mid$(Linea, 150, 15)) / 100
                exHoja.Cells.Item(RowExxel, 13) = CDec(Mid$(Linea, 165, 15)) / 100
                exHoja.Cells.Item(RowExxel, 14) = CDec(Mid$(Linea, 180, 15)) / 100
                exHoja.Cells.Item(RowExxel, 15) = CDec(Mid$(Linea, 195, 15)) / 100
                exHoja.Cells.Item(RowExxel, 16) = CDec(Mid$(Linea, 210, 15)) / 100
                Total16 = Total16 + CDec(Mid$(Linea, 210, 15)) / 100
                exHoja.Cells.Item(RowExxel, 17) = Mid$(Linea, 225, 3)
                exHoja.Cells.Item(RowExxel, 18) = CDec(Mid$(Linea, 228, 10)) / 1000000
                exHoja.Cells.Item(RowExxel, 19) = Mid$(Linea, 238, 1)
                exHoja.Cells.Item(RowExxel, 20) = Mid$(Linea, 239, 1)
                exHoja.Cells.Item(RowExxel, 21) = CDec(Mid$(Linea, 240, 15)) / 100
                Total21 = Total21 + CDec(Mid$(Linea, 240, 15)) / 100
                exHoja.Cells.Item(RowExxel, 22) = CDec(Mid$(Linea, 255, 15)) / 100
                exHoja.Cells.Item(RowExxel, 23) = Mid$(Linea, 270, 11)
                exHoja.Cells.Item(RowExxel, 24) = Mid$(Linea, 281, 30)
                exHoja.Cells.Item(RowExxel, 25) = CDec(Mid$(Linea, 311, 15)) / 100
                Orden = Orden + 1
                exHoja.Cells.Item(RowExxel, 29) = Orden
                Using reader2 As StreamReader = New StreamReader(Directorio2)
                    Dim Linea2 = reader2.ReadLine
                    Do While (Not Linea2 Is Nothing)
                        If Mid$(Linea, 9, 28) & Mid$(Linea, 53, 22) = Mid$(Linea2, 1, 50) Then
                            RowExxel = RowExxel + 1
                            exHoja.Cells.Item(RowExxel, 26) = CDec(Mid$(Linea2, 51, 15)) / 100
                            Total26 = Total26 + CDec(Mid$(Linea2, 51, 15)) / 100
                            exHoja.Cells.Item(RowExxel, 27) = Mid$(Linea2, 66, 4)
                            exHoja.Cells.Item(RowExxel, 28) = CDec(Mid$(Linea2, 70, 15)) / 100
                            Total28 = Total28 + CDec(Mid$(Linea2, 70, 15)) / 100
                        End If
                        Linea2 = reader2.ReadLine
                    Loop
                End Using
                Linea = reader.ReadLine
            Loop
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 10) = Total10
            exHoja.Cells.Item(RowExxel, 16) = Total16
            exHoja.Cells.Item(RowExxel, 21) = Total21
            exHoja.Cells.Item(RowExxel, 26) = Total26
            exHoja.Cells.Item(RowExxel, 28) = Total28
        End Using

        '
        Dim Rango As Microsoft.Office.Interop.Excel.Range
        Dim AA As String
        Dim BB As String

        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        Rango.NumberFormat = "00-00000000-0"
        '
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 12 - 1) & InicioDatos
        BB = Chr(65 + 12 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 13 - 1) & InicioDatos
        BB = Chr(65 + 13 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 14 - 1) & InicioDatos
        BB = Chr(65 + 14 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 15 - 1) & InicioDatos
        BB = Chr(65 + 15 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 16 - 1) & InicioDatos
        BB = Chr(65 + 16 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 18 - 1) & InicioDatos
        BB = Chr(65 + 18 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,000000"
        '
        AA = Chr(65 + 21 - 1) & InicioDatos
        BB = Chr(65 + 21 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 22 - 1) & InicioDatos
        BB = Chr(65 + 22 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        exHoja.Cells.Item(3, 1) = "Orden en Afip: Numero del Registro en el archivo, usado para hallar Errores."

        exApp.Application.Visible = True
        '
        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub GeneraExcelCITIVentas()

        Dim Directorio1 As String = ""
        Dim Directorio2 As String = ""

        Directorio1 = PideCarpeta()
        If Directorio1 = "" Then Exit Sub
        Directorio2 = Directorio1
        Directorio1 = Directorio1 & "\" & "REGINFO_CV_VENTAS_CBTE.TXT"
        Directorio2 = Directorio2 & "\" & "AuxParaExcel.TXT"

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        exApp.DisplayAlerts = False  'para arreglar un error por valores multiples en celda. No supe por que. 

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        'Muestra titulos.
        exHoja.Cells.Item(1, 6) = Format(Date.Now, "MM/dd/yyyy")

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = "Fecha"
        exHoja.Cells.Item(RowExxel, 2) = "Tipo Compro."
        exHoja.Cells.Item(RowExxel, 3) = "Punto "
        exHoja.Cells.Item(RowExxel, 4) = "Nro.Compr."
        exHoja.Cells.Item(RowExxel, 5) = "Nro.Hasta"
        exHoja.Cells.Item(RowExxel, 6) = "Cód.Docu."
        exHoja.Cells.Item(RowExxel, 7) = "Numero Identif."
        exHoja.Cells.Item(RowExxel, 8) = "Nombre Comprador"
        exHoja.Cells.Item(RowExxel, 9) = "Importe Total"
        exHoja.Cells.Item(RowExxel, 10) = "Neto No Grabado"
        exHoja.Cells.Item(RowExxel, 11) = "Percepciones No "
        exHoja.Cells.Item(RowExxel, 12) = "Neto Excento"
        exHoja.Cells.Item(RowExxel, 13) = "Percepciones "
        exHoja.Cells.Item(RowExxel, 14) = "Percepciones "
        exHoja.Cells.Item(RowExxel, 15) = "Percepciones "
        exHoja.Cells.Item(RowExxel, 16) = "Importe Impuestos"
        exHoja.Cells.Item(RowExxel, 17) = "Código Moneda"
        exHoja.Cells.Item(RowExxel, 18) = "Tipo "
        exHoja.Cells.Item(RowExxel, 19) = "Cantidad "
        exHoja.Cells.Item(RowExxel, 20) = "Código "
        exHoja.Cells.Item(RowExxel, 21) = "Otros Tributos"
        exHoja.Cells.Item(RowExxel, 22) = "Fecha Pago"
        exHoja.Cells.Item(RowExxel, 23) = "Neto Grabado"
        exHoja.Cells.Item(RowExxel, 24) = "Alicuota "
        exHoja.Cells.Item(RowExxel, 25) = "Impuesto "
        exHoja.Cells.Item(RowExxel, 26) = "ORDEN"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 2

        RowExxel = 7
        exHoja.Cells.Item(RowExxel, 1) = ""
        exHoja.Cells.Item(RowExxel, 2) = ""
        exHoja.Cells.Item(RowExxel, 3) = "de Venta"
        exHoja.Cells.Item(RowExxel, 4) = ""
        exHoja.Cells.Item(RowExxel, 5) = ""
        exHoja.Cells.Item(RowExxel, 6) = "Comprador"
        exHoja.Cells.Item(RowExxel, 7) = "Comprador"
        exHoja.Cells.Item(RowExxel, 8) = ""
        exHoja.Cells.Item(RowExxel, 9) = ""
        exHoja.Cells.Item(RowExxel, 10) = ""
        exHoja.Cells.Item(RowExxel, 11) = "Categorizado"
        exHoja.Cells.Item(RowExxel, 12) = ""
        exHoja.Cells.Item(RowExxel, 13) = "Imp.Nacionales"
        exHoja.Cells.Item(RowExxel, 14) = "Ingresos Brutos"
        exHoja.Cells.Item(RowExxel, 15) = "Imp.Municipales"
        exHoja.Cells.Item(RowExxel, 16) = "Internos"
        exHoja.Cells.Item(RowExxel, 17) = ""
        exHoja.Cells.Item(RowExxel, 18) = "Cambio"
        exHoja.Cells.Item(RowExxel, 19) = "Ivas"
        exHoja.Cells.Item(RowExxel, 20) = "Operación"
        exHoja.Cells.Item(RowExxel, 21) = "Computable"
        exHoja.Cells.Item(RowExxel, 22) = ""
        exHoja.Cells.Item(RowExxel, 26) = "En Afip"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 2

        Dim ColExxel As Integer = 8

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim Linea As String
        Dim Total9 As Decimal = 0
        Dim Total10 As Decimal = 0
        Dim Total16 As Decimal = 0
        Dim Total21 As Decimal = 0
        Dim Total23 As Decimal = 0
        Dim Total25 As Decimal = 0
        Dim Orden As Integer = 0

        Using reader As StreamReader = New StreamReader(Directorio1)
            Linea = reader.ReadLine
            Do While (Not Linea Is Nothing)
                RowExxel = RowExxel + 1
                exHoja.Cells.Item(RowExxel, 1) = Mid$(Linea, 7, 2) & "/" & Mid$(Linea, 5, 2) & "/" & Mid$(Linea, 1, 4)
                exHoja.Cells.Item(RowExxel, 2) = CodigoDocumentoCITI(Mid$(Linea, 9, 3))
                exHoja.Cells.Item(RowExxel, 3) = Mid$(Linea, 12, 5)
                exHoja.Cells.Item(RowExxel, 4) = Mid$(Linea, 17, 20)
                exHoja.Cells.Item(RowExxel, 5) = Mid$(Linea, 37, 20)
                exHoja.Cells.Item(RowExxel, 6) = Mid$(Linea, 57, 2)
                exHoja.Cells.Item(RowExxel, 7) = Mid$(Linea, 59, 20)
                exHoja.Cells.Item(RowExxel, 8) = Mid$(Linea, 79, 30)
                exHoja.Cells.Item(RowExxel, 9) = CDec(Mid$(Linea, 109, 15)) / 100
                Total9 = Total9 + CDec(Mid$(Linea, 109, 15)) / 100
                exHoja.Cells.Item(RowExxel, 10) = CDec(Mid$(Linea, 124, 15)) / 100
                Total10 = Total10 + CDec(Mid$(Linea, 124, 15)) / 100
                exHoja.Cells.Item(RowExxel, 11) = CDec(Mid$(Linea, 139, 15)) / 100
                exHoja.Cells.Item(RowExxel, 12) = CDec(Mid$(Linea, 154, 15)) / 100
                exHoja.Cells.Item(RowExxel, 13) = CDec(Mid$(Linea, 169, 15)) / 100
                exHoja.Cells.Item(RowExxel, 14) = CDec(Mid$(Linea, 184, 15)) / 100
                exHoja.Cells.Item(RowExxel, 15) = CDec(Mid$(Linea, 199, 15)) / 100
                exHoja.Cells.Item(RowExxel, 16) = CDec(Mid$(Linea, 214, 15)) / 100
                Total16 = Total16 + CDec(Mid$(Linea, 214, 15)) / 100
                exHoja.Cells.Item(RowExxel, 17) = Mid$(Linea, 229, 3)
                exHoja.Cells.Item(RowExxel, 18) = CDec(Mid$(Linea, 232, 10)) / 1000000
                exHoja.Cells.Item(RowExxel, 19) = Mid$(Linea, 242, 1)
                exHoja.Cells.Item(RowExxel, 20) = Mid$(Linea, 243, 1)
                exHoja.Cells.Item(RowExxel, 21) = CDec(Mid$(Linea, 244, 15)) / 100
                Total21 = Total21 + CDec(Mid$(Linea, 244, 15)) / 100
                If Mid$(Linea, 259, 1) <> " " Then
                    exHoja.Cells.Item(RowExxel, 22) = Mid$(Linea, 263, 2) & "/" & Mid$(Linea, 265, 2) & "/" & Mid$(Linea, 259, 4)
                End If
                Orden = Orden + 1
                exHoja.Cells.Item(RowExxel, 26) = Orden
                Using reader2 As StreamReader = New StreamReader(Directorio2)
                    Dim Linea2 = reader2.ReadLine
                    Do While (Not Linea2 Is Nothing)
                        If Mid$(Linea, 9, 28) & Mid$(Linea, 57, 22) = Mid$(Linea2, 1, 28) & Mid$(Linea2, 63, 22) Then
                            RowExxel = RowExxel + 1
                            exHoja.Cells.Item(RowExxel, 23) = CDec(Mid$(Linea2, 29, 15)) / 100
                            Total23 = Total23 + CDec(Mid$(Linea2, 29, 15)) / 100
                            exHoja.Cells.Item(RowExxel, 24) = Mid$(Linea2, 44, 4)
                            exHoja.Cells.Item(RowExxel, 25) = CDec(Mid$(Linea2, 48, 15)) / 100
                            Total25 = Total25 + CDec(Mid$(Linea2, 48, 15)) / 100
                        End If
                        Linea2 = reader2.ReadLine
                    Loop
                End Using
                Linea = reader.ReadLine
            Loop
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 9) = Total9
            exHoja.Cells.Item(RowExxel, 10) = Total10
            exHoja.Cells.Item(RowExxel, 16) = Total16
            exHoja.Cells.Item(RowExxel, 21) = Total21
            exHoja.Cells.Item(RowExxel, 23) = Total23
            exHoja.Cells.Item(RowExxel, 25) = Total25
        End Using

        '
        Dim Rango As Microsoft.Office.Interop.Excel.Range
        Dim AA As String
        Dim BB As String

        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        Rango.NumberFormat = "00-00000000-0"
        '
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 12 - 1) & InicioDatos
        BB = Chr(65 + 12 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 13 - 1) & InicioDatos
        BB = Chr(65 + 13 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 14 - 1) & InicioDatos
        BB = Chr(65 + 14 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 15 - 1) & InicioDatos
        BB = Chr(65 + 15 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 16 - 1) & InicioDatos
        BB = Chr(65 + 16 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 18 - 1) & InicioDatos
        BB = Chr(65 + 18 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,000000"
        '
        AA = Chr(65 + 21 - 1) & InicioDatos
        BB = Chr(65 + 21 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 22 - 1) & InicioDatos
        BB = Chr(65 + 22 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 23 - 1) & InicioDatos
        BB = Chr(65 + 23 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 25 - 1) & InicioDatos
        BB = Chr(65 + 25 - 1) & exLibro.ActiveSheet.UsedRange.Rows.Count
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        exHoja.Cells.Item(3, 1) = "Orden en Afip: Numero del Registro en el archivo, usado para hallar Errores."

        exApp.Application.Visible = True
        '
        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Private Function CodigoDocumentoCITI(ByVal Codigo As String) As String

        Select Case Codigo
            Case "001"
                Return "Factura A"
            Case "201"
                Return "Factura A FCE"
            Case "006"
                Return "Factura B"
            Case "206"
                Return "Factura B FCE"
            Case "011"
                Return "Factura C"
            Case "211"
                Return "Factura C FCE"
            Case "019"
                Return "Factura E"
            Case "051"
                Return "Factura M"
            Case "002"
                Return "N.Debito A"
            Case "202"
                Return "N.Debito A FCE"
            Case "003"
                Return "N.Crédito A"
            Case "203"
                Return "N.Crédito A FCE"
            Case "007"
                Return "N.Debito B"
            Case "207"
                Return "N.Debito B FCE"
            Case "008"
                Return "N.Crédito B"
            Case "208"
                Return "N.Crédito B FCE"
            Case "012"
                Return "N.Debito C"
            Case "212"
                Return "N.Debito C FCE"
            Case "013"
                Return "N.Crédito C"
            Case "213"
                Return "N.Crédito C FCE"
            Case "021"
                Return "N.Crédito E"
            Case "052"
                Return "N.Debito M"
            Case "053"
                Return "N.Crédito M"
            Case "060"
                Return "N.V.L.P. A"
            Case "061"
                Return "N.V.L.P. B"
            Case "058"
                Return "N.V.L.P. M"
            Case "063"
                Return "Liquidación A"
            Case "064"
                Return "Liquidación B"
            Case "068"
                Return "Liquidación C"
            Case "059"
                Return "Liquidación M"
            Case "099"
                Return "Otros Conceptos"
            Case "081"
                Return "Ticket A"
            Case "082"
                Return "Ticket B"
            Case "111"
                Return "Ticket C"
            Case "118"
                Return "Ticket M"
            Case "083"
                Return "Ticket"
            Case "112"
                Return "N.Credito Ticket A"
            Case "113"
                Return "N.Credito Ticket B"
            Case "114"
                Return "N.Credito Ticket C"
            Case "119"
                Return "N.Credito Ticket M"
            Case "110"
                Return "N.Credito Ticket"
        End Select

    End Function
    Public Sub InformeRemitosFactura(ByVal DtDetalle As DataTable, ByVal Cliente As Integer, ByVal Factura As String, ByVal Abierto As Boolean)

        Dim Cantidad As Decimal = 0
        For Each Row As DataRow In DtDetalle.Rows
            If Row("Remito") <> 0 Then Cantidad = Cantidad + 1
        Next
        If Cantidad = 0 Then
            MsgBox("No existen Remitos para Esta Factura.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = "Op."
        exHoja.Cells.Item(RowExxel, 2) = "Remito"
        exHoja.Cells.Item(RowExxel, 3) = "CodigoCliente"
        exHoja.Cells.Item(RowExxel, 4) = "Articulo"
        exHoja.Cells.Item(RowExxel, 5) = "Cantidad"
        exHoja.Cells.Item(RowExxel, 6) = "Nombre Sucursal"
        exHoja.Cells.Item(RowExxel, 7) = "Codigo Externo"
        exHoja.Cells.Item(RowExxel, 8) = "Errores"
        exHoja.Cells.Item(RowExxel, 10) = "Fecha Remito"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim RemitoAnt As Decimal = 0
        Dim FechaRemito As Date
        Dim Sucursal As Integer = 0
        Dim CodigoExterno As String = ""
        Dim NombreSucursal As String = ""
        Dim SucursalPorCuenta As Integer = 0
        Dim ClientePorCuenta As Integer = 0
        Dim ClienteW As Integer = 0
        Dim Cartel As String = ""

        For Each Row As DataRow In DtDetalle.Rows
            If Row("Remito") <> 0 Then
                Cartel = ""
                If RemitoAnt <> Row("Remito") Then
                    Sucursal = 0
                    HallaSucursalRemito(Row("Remito"), Abierto, Sucursal, FechaRemito, ClientePorCuenta, SucursalPorCuenta)
                    If SucursalPorCuenta <> 0 Then
                        Sucursal = SucursalPorCuenta
                        ClienteW = ClientePorCuenta
                    Else
                        ClienteW = Cliente
                    End If
                    CodigoExterno = ""
                    NombreSucursal = ""
                    If Sucursal <> 0 Then
                        If Not HallaNombreYCodigoExternoRemito(ClienteW, Sucursal, CodigoExterno, NombreSucursal) Then
                            Cartel = Cartel & " Sucursal no existe"
                        End If
                        If CodigoExterno = "" Then
                            Cartel = Cartel & " Falta Codigo Externo Sucursal"
                        End If
                    End If
                    RemitoAnt = Row("Remito")
                End If
                RowExxel = RowExxel + 1
                If Not Abierto Then exHoja.Cells.Item(RowExxel, 1) = "X"
                exHoja.Cells.Item(RowExxel, 2) = Row("Remito")
                Dim CodigoArticuloCliente As String = HallaCodigoCliente(Cliente, Row("Articulo"))
                If IsNothing(CodigoArticuloCliente) Then CodigoArticuloCliente = "-1"
                If CodigoArticuloCliente = "-1" Then Cartel = Cartel & " Falta Codigo Cliente Articulo"
                exHoja.Cells.Item(RowExxel, 3) = CodigoArticuloCliente
                exHoja.Cells.Item(RowExxel, 4) = NombreArticulo(Row("Articulo"))
                exHoja.Cells.Item(RowExxel, 5) = Row("Cantidad") - Row("Devueltas")
                exHoja.Cells.Item(RowExxel, 6) = NombreSucursal
                exHoja.Cells.Item(RowExxel, 7) = CodigoExterno
                exHoja.Cells.Item(RowExxel, 8) = Cartel
                exHoja.Cells.Item(RowExxel, 9) = "D"
                exHoja.Cells.Item(RowExxel, 10) = Format(FechaRemito, "MM/dd/yyyy")
            End If
        Next

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 1 - 1) & InicioDatos
        BB = Chr(65 + 1 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft

        exHoja.Columns.AutoFit()

        exHoja.Rows.Item(5).Font.Bold = True
        exHoja.Cells.Item(1, 2) = GNombreEmpresa
        exHoja.Cells.Item(1, 20) = "IMPEXC"
        exHoja.Cells.Item(3, 2) = "DETALLE DE Remitos de la factura: " & NumeroEditado(Factura) & "  Cliente: " & NombreCliente(Cliente)

        'Muestra titulos.
        exHoja.Cells.Item(1, 9) = "Fecha Proceso " & Format(Date.Now, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeVentasYCostosDeFacturas(ByVal Desde As Date, ByVal Hasta As Date, ByVal Abierto As Boolean, ByVal Cerrado As Boolean, ByVal EsExportacion As Boolean, ByVal EsDomestica As Boolean, ByVal Cliente As Integer)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim SqlB As String = ""
        Dim SqlN As String = ""
        Dim SqlFecha As String = ""
        SqlFecha = "AND Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlFechaContable As String = ""
        SqlFechaContable = "AND FechaContable >='" & Format(Desde, "yyyyMMdd") & "' AND FechaContable < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlFechaElectronica As String = ""
        SqlFechaElectronica = "AND FechaElectronica >='" & Format(Desde, "yyyyMMdd") & "' AND FechaElectronica < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlEsExportacion As String = ""
        If EsExportacion Then
            SqlEsExportacion = " AND EsExterior = 1"
        End If
        If EsDomestica Then
            SqlEsExportacion = " AND EsExterior = 0"
        End If
        If EsExportacion And EsDomestica Then
            SqlEsExportacion = ""
        End If

        Dim SqlCliente As String = " AND Cliente = " & Cliente & ""
        If Cliente = 0 Then SqlCliente = ""

        If EsExportacion And Not EsDomestica Then
            SqlB = "SELECT 1 As Operacion,1 AS Tipo,F.Factura as ReciboOficial,F.Cliente,F.Rel,F.Relacionada AS NRel,F.FechaElectronica AS Fecha,A.Comprobante,A.Lote,A.Secuencia,A.Operacion AS OperacionLote,A.Cantidad,A.Importe AS ImporteConIva,A.ImporteSinIva,0 AS Merma,F.EsExterior FROM FacturasCabeza AS F INNER JOIN AsignacionLotes AS A ON F.Factura = A.Comprobante WHERE F.Tr = 0 AND F.Estado = 1 AND A.Cantidad <> 0 AND A.TipoComprobante = 2 " & SqlFechaElectronica & SqlEsExportacion & SqlCliente & ";"
            SqlN = "SELECT 2 As Operacion,1 AS Tipo,F.Factura as ReciboOficial,F.Cliente,F.Rel,F.Relacionada AS NRel,F.FechaElectronica AS Fecha,A.Comprobante,A.Lote,A.Secuencia,A.Operacion AS OperacionLote,A.Cantidad,A.Importe AS ImporteConIva,A.ImporteSinIva,0 AS Merma,F.EsExterior FROM FacturasCabeza AS F INNER JOIN AsignacionLotes AS A ON F.Factura = A.Comprobante WHERE F.Tr = 0 AND F.Estado = 1 AND A.Cantidad <> 0 AND A.TipoComprobante = 2 " & SqlFechaElectronica & SqlEsExportacion & SqlCliente & ";"
        Else
            SqlB = "SELECT 1 As Operacion,1 AS Tipo,F.Factura as ReciboOficial,F.Cliente,F.Rel,F.Relacionada AS NRel,F.FechaContable AS Fecha,A.Comprobante,A.Lote,A.Secuencia,A.Operacion AS OperacionLote,A.Cantidad,A.Importe AS ImporteConIva,A.ImporteSinIva,0 AS Merma,F.EsExterior FROM FacturasCabeza AS F INNER JOIN AsignacionLotes AS A ON F.Factura = A.Comprobante WHERE F.Tr = 0 AND EsExterior = 0 AND F.Estado = 1 AND A.Cantidad <> 0 AND A.TipoComprobante = 2 " & SqlFechaContable & SqlEsExportacion & SqlCliente & _
                   " UNION ALL " & _
                   "SELECT 1 As Operacion,1 AS Tipo,F.Factura as ReciboOficial,F.Cliente,F.Rel,F.Relacionada AS NRel,F.FechaElectronica AS Fecha,A.Comprobante,A.Lote,A.Secuencia,A.Operacion AS OperacionLote,A.Cantidad,A.Importe AS ImporteConIva,A.ImporteSinIva,0 AS Merma,F.EsExterior FROM FacturasCabeza AS F INNER JOIN AsignacionLotes AS A ON F.Factura = A.Comprobante WHERE F.Tr = 0 AND ESExterior = 1 AND F.Estado = 1 AND A.Cantidad <> 0 AND A.TipoComprobante = 2 " & SqlFechaElectronica & SqlEsExportacion & SqlCliente & _
                   " UNION ALL " & _
                   "SELECT 1 As Operacion,2 AS Tipo,F.ReciboOficial,F.Cliente,F.Rel,F.NRel,F.FechaContable AS Fecha,A.Liquidacion AS Comprobante,A.Lote,A.Secuencia,A.Operacion AS OperacionLote,A.Cantidad,A.NetoConIva AS ImporteConIva,A.NetoSinIva AS ImporteSinIva,A.Merma,1 AS EsExterior FROM NVLPCabeza AS F INNER JOIN NVLPLotes AS A ON F.Liquidacion = A.Liquidacion WHERE F.Estado = 1 " & SqlFechaContable & SqlCliente & ";"
            '
            SqlN = "SELECT 2 As Operacion,1 AS Tipo,F.Factura as ReciboOficial,F.Cliente,F.Rel,F.Relacionada AS NRel,F.FechaContable AS Fecha,A.Comprobante,A.Lote,A.Secuencia,A.Operacion AS OperacionLote,A.Cantidad,A.Importe AS ImporteConIva,A.ImporteSinIva,0 AS Merma,F.EsExterior FROM FacturasCabeza AS F INNER JOIN AsignacionLotes AS A ON F.Factura = A.Comprobante WHERE F.Tr = 0 AND F.EsExterior = 0 AND F.Estado = 1 AND A.Cantidad <> 0 AND A.TipoComprobante = 2 " & SqlFechaContable & SqlEsExportacion & SqlCliente & _
                   " UNION ALL " & _
                   "SELECT 2 As Operacion,1 AS Tipo,F.Factura as ReciboOficial,F.Cliente,F.Rel,F.Relacionada AS NRel,F.FechaElectronica AS Fecha,A.Comprobante,A.Lote,A.Secuencia,A.Operacion AS OperacionLote,A.Cantidad,A.Importe AS ImporteConIva,A.ImporteSinIva,0 AS Merma,F.EsExterior FROM FacturasCabeza AS F INNER JOIN AsignacionLotes AS A ON F.Factura = A.Comprobante WHERE F.Tr = 0 AND F.EsExterior = 1 AND F.Estado = 1 AND A.Cantidad <> 0 AND A.TipoComprobante = 2 " & SqlFechaElectronica & SqlEsExportacion & SqlCliente & _
                   " UNION ALL " & _
                   "SELECT 2 As Operacion,2 AS Tipo,F.ReciboOficial,F.Cliente,F.Rel,F.NRel,F.FechaContable AS Fecha,A.Liquidacion AS Comprobante,A.Lote,A.Secuencia,A.Operacion AS OperacionLote,A.Cantidad,A.NetoConIva AS ImporteConIva,A.NetoSinIva AS ImporteSinIva,A.Merma,1 AS EsExterior FROM NVLPCabeza AS F INNER JOIN NVLPLotes AS A ON F.Liquidacion = A.Liquidacion WHERE F.Estado = 1 " & SqlFechaContable & SqlCliente & ";"
        End If

        Dim DtVentas As New DataTable
        If Abierto Then
            If Not Tablas.Read(SqlB, Conexion, DtVentas) Then Exit Sub
        End If
        If Cerrado Then
            If Not Tablas.Read(SqlN, ConexionN, DtVentas) Then Exit Sub
        End If

        If DtVentas.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim View As New DataView
        View = DtVentas.DefaultView
        View.Sort = "Fecha,Comprobante,Tipo,Lote,Secuencia"

        Dim ListaLotes As New List(Of FilaGenerica)

        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = " "
        exHoja.Cells.Item(RowExxel, 3) = "Comprobante"
        exHoja.Cells.Item(RowExxel, 4) = "Fecha"
        exHoja.Cells.Item(RowExxel, 5) = "Lote"
        exHoja.Cells.Item(RowExxel, 6) = "Articulo       "
        exHoja.Cells.Item(RowExxel, 7) = "Cantidad"
        exHoja.Cells.Item(RowExxel, 8) = "Importe Sin Iva"
        exHoja.Cells.Item(RowExxel, 9) = "Importe Iva"
        exHoja.Cells.Item(RowExxel, 10) = "Total"
        exHoja.Cells.Item(RowExxel, 11) = "Cliente"
        exHoja.Cells.Item(RowExxel, 12) = " "
        exHoja.Cells.Item(RowExxel, 13) = "Monto Sin Iva"
        exHoja.Cells.Item(RowExxel, 14) = "Monto IVA."
        exHoja.Cells.Item(RowExxel, 15) = "Total"
        exHoja.Cells.Item(RowExxel, 16) = "Merma Imp.Sin Iva"
        exHoja.Cells.Item(RowExxel, 17) = "Merma Imp.Iva"
        exHoja.Cells.Item(RowExxel, 18) = "Total"
        exHoja.Cells.Item(RowExxel, 19) = "Proveedor"
        exHoja.Cells.Item(RowExxel, 20) = " "
        exHoja.Cells.Item(RowExxel, 21) = "Importe Sin Iva"
        exHoja.Cells.Item(RowExxel, 22) = "Importe Iva"
        exHoja.Cells.Item(RowExxel, 23) = "Total"
        exHoja.Cells.Item(RowExxel, 25) = "Fecha Ingreso"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim ImporteConIva As Decimal
        Dim ImporteSinIva As Decimal
        Dim ImporteIva As Decimal
        Dim ImporteSinIvaT As Decimal
        Dim ImporteIvaT As Decimal
        Dim CantidadT As Decimal
        Dim RowsBusqueda() As DataRow
        '
        Dim OperacionW As String
        Dim TipoW As String
        Dim TipoDocuAnt As Integer
        Dim FechaW As String
        Dim ComprobanteW As String
        Dim ArticuloW As String
        Dim LoteW As String
        Dim ClienteW As String
        Dim ReciboOficialW As String
        '
        Dim ComprobanteAnt As String = ""
        Dim LoteAnt As Integer = 0
        Dim SecuenciaAnt As Integer = 0
        Dim CantidadAnt As Decimal
        Dim OperacionLoteAnt As Integer = 0
        Dim MermaAnt As Decimal = 0

        For Each Row As DataRowView In View
            If Abierto And Cerrado And Row("operacion") = 2 And Row("Rel") Then GoTo continuar
            If ComprobanteAnt <> Row("Comprobante") & "/" & Row("Tipo") Then
                If ComprobanteAnt <> "" Then
                    RowExxel = RowExxel + 1
                    CortePorLote_InformeVentasYCostosDeLotes(exHoja, RowExxel, OperacionW, TipoDocuAnt, TipoW, ReciboOficialW, FechaW, ArticuloW, LoteW, CantidadAnt, ImporteSinIva, ImporteIva, ClienteW, LoteAnt, SecuenciaAnt, OperacionLoteAnt, ListaLotes, MermaAnt)
                    ImporteSinIvaT = ImporteSinIvaT + ImporteSinIva
                    ImporteIvaT = ImporteIvaT + ImporteIva
                    CantidadT = CantidadT + CantidadAnt
                End If

                If Row("Operacion") = 2 Then
                    OperacionW = "X"
                End If
                Select Case Row("Tipo")
                    Case 1
                        TipoW = "Factura"
                    Case 2
                        TipoW = "NVLP"
                End Select
                If Row("Tipo") = 1 And Row("EsExterior") = 1 And PermisoTotal Then
                    If TieneCierreFactura(Row("Comprobante")) Then TipoW = "Fact.Cerrada"
                End If

                TipoDocuAnt = Row("Tipo")
                ComprobanteW = NumeroEditado(Row("Comprobante"))
                ReciboOficialW = NumeroEditado(Row("ReciboOficial"))
                If Not Abierto And Cerrado And Row("Rel") Then
                    ComprobanteW = NumeroEditado(Row("NRel"))
                End If
                FechaW = Format(Row("Fecha"), "MM/dd/yyyy")
                ClienteW = NombreCliente(Row("Cliente"))
                ComprobanteAnt = Row("Comprobante") & "/" & Row("Tipo")
                LoteAnt = 0
                SecuenciaAnt = 0
            End If
            If LoteAnt & "/" & SecuenciaAnt <> Row("Lote") & "/" & Row("Secuencia") Then
                If LoteAnt <> 0 Then
                    RowExxel = RowExxel + 1
                    CortePorLote_InformeVentasYCostosDeLotes(exHoja, RowExxel, OperacionW, TipoDocuAnt, TipoW, ReciboOficialW, FechaW, ArticuloW, LoteW, CantidadAnt, ImporteSinIva, ImporteIva, ClienteW, LoteAnt, SecuenciaAnt, OperacionLoteAnt, ListaLotes, MermaAnt)
                    ImporteSinIvaT = ImporteSinIvaT + ImporteSinIva
                    ImporteIvaT = ImporteIvaT + ImporteIva
                    CantidadT = CantidadT + CantidadAnt
                End If
                Dim ConexionLote As String = ""
                Select Case Row("OperacionLote")
                    Case 1
                        ConexionLote = Conexion
                    Case 2
                        ConexionLote = ConexionN
                End Select
                ArticuloW = NombreArticulo(HallaArticulo(Row("Lote"), Row("Secuencia"), ConexionLote))
                LoteW = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
                MermaAnt = Row("Merma")
                ImporteConIva = 0
                ImporteSinIva = 0
                ImporteIva = 0
                CantidadAnt = 0
                LoteAnt = Row("Lote")
                SecuenciaAnt = Row("Secuencia")
                OperacionLoteAnt = Row("OperacionLote")
            End If
            CantidadAnt = CantidadAnt + CDec(Row("Cantidad"))
            If Row("Operacion") = 1 Then ImporteIva = ImporteIva + CDec(Row("ImporteConIva")) - CDec(Row("ImporteSinIva"))
            ImporteSinIva = ImporteSinIva + CDec(Row("ImporteSinIva"))
            If Row("Rel") And Row("Operacion") = 1 And Cerrado Then
                RowsBusqueda = DtVentas.Select("NRel = " & Row("Comprobante") & " AND Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
                If RowsBusqueda.Length <> 0 Then ImporteSinIva = ImporteSinIva + CDec(RowsBusqueda(0).Item("ImporteSinIva"))
            End If
continuar:
        Next
        '
        RowExxel = RowExxel + 1
        CortePorLote_InformeVentasYCostosDeLotes(exHoja, RowExxel, OperacionW, TipoDocuAnt, TipoW, ReciboOficialW, FechaW, ArticuloW, LoteW, CantidadAnt, ImporteSinIva, ImporteIva, ClienteW, LoteAnt, SecuenciaAnt, OperacionLoteAnt, ListaLotes, MermaAnt)
        ImporteSinIvaT = ImporteSinIvaT + ImporteSinIva
        ImporteIvaT = ImporteIvaT + ImporteIva
        CantidadT = CantidadT + CantidadAnt

        'Totales.
        RowExxel = RowExxel + 1
        AgregaAExcel(exHoja, RowExxel, "", "Totales", "", "", "", "", CantidadT, ImporteSinIvaT, ImporteIvaT, 0, "", 0, 0, 0, "", 0, 0, 0, 0, 0, 0, "", "1800/01/01")

        DtVentas.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 1 - 1) & InicioDatos
        BB = Chr(65 + 1 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        ' 
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        ' 
        AA = Chr(65 + 13 - 1) & InicioDatos
        BB = Chr(65 + 13 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 14 - 1) & InicioDatos
        BB = Chr(65 + 14 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 15 - 1) & InicioDatos
        BB = Chr(65 + 15 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 16 - 1) & InicioDatos
        BB = Chr(65 + 16 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 17 - 1) & InicioDatos
        BB = Chr(65 + 17 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 18 - 1) & InicioDatos
        BB = Chr(65 + 18 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 19 - 1) & InicioDatos
        BB = Chr(65 + 19 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        ' 
        AA = Chr(65 + 21 - 1) & InicioDatos
        BB = Chr(65 + 21 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 22 - 1) & InicioDatos
        BB = Chr(65 + 22 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 23 - 1) & InicioDatos
        BB = Chr(65 + 23 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 25 - 1) & InicioDatos
        BB = Chr(65 + 25 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter

        exHoja.Columns.AutoFit()

        'Dibuja rectangulos.=------------------------------
        If InicioDatos < UltimaLinea - 1 Then
            AA = Chr(65 + 1 - 1) & InicioDatos              ' 1,11,13,19,21,23,25 son las columnas.
            BB = Chr(65 + 11 - 1) & UltimaLinea - 1
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
            AA = Chr(65 + 13 - 1) & InicioDatos
            BB = Chr(65 + 19 - 1) & UltimaLinea - 1
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
            AA = Chr(65 + 21 - 1) & InicioDatos
            BB = Chr(65 + 23 - 1) & UltimaLinea - 1
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
            DibujaRango(Rango)
            'Fecha Ingreso.
            AA = Chr(65 + 25 - 1) & InicioDatos
            BB = Chr(65 + 25 - 1) & UltimaLinea - 1
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
        End If
        '--------------------------------------------------

        exHoja.Rows.Item(5).Font.Bold = True
        'Muestra titulos.
        exHoja.Cells.Item(2, 1) = "Gestión Compra-Venta Por Factura       " & Format(Date.Now, "dd/MM/yyyy")

        exHoja.Cells.Item(5, 6) = "        V E N T A S      (incluye devoluciones)"
        exHoja.Cells.Item(5, 16) = "C O M P R A S"
        exHoja.Cells.Item(5, 21) = "    COSTOS AFECTADOS AL LOTE"

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeFacturasSaldos(ByVal Desde As Date, ByVal Hasta As Date, ByVal Abierto As Boolean, ByVal Cerrado As Boolean, ByVal EsExportacion As Boolean, ByVal EsDomestica As Boolean, ByVal PuntoDeVenta As Integer, ByVal Vendedor As Integer)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim SqlB As String = ""
        Dim SqlN As String = ""
        Dim SqlFecha As String = ""
        SqlFecha = "AND Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlFechaContable As String = ""
        SqlFechaContable = "AND FechaContable >='" & Format(Desde, "yyyyMMdd") & "' AND FechaContable < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlFechaElectronica As String = ""
        SqlFechaElectronica = "AND FechaElectronica >='" & Format(Desde, "yyyyMMdd") & "' AND FechaElectronica < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlEsExportacion As String = ""
        If EsExportacion Then
            SqlEsExportacion = " AND EsExterior = 1"
        End If
        If EsDomestica Then
            SqlEsExportacion = " AND EsExterior = 0"
        End If
        If EsExportacion And EsDomestica Then
            SqlEsExportacion = ""
        End If
        Dim SqlVendedor As String = ""
        If Vendedor <> 0 Then
            SqlVendedor = " AND Vendedor = " & Vendedor
        End If

        Dim Patron As String = "[0-9]" & Format(PuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Dim SqlPuntoDeVenta As String = ""
        If PuntoDeVenta <> 0 Then SqlPuntoDeVenta = " AND CAST(CAST(F.Factura AS numeric) as char)LIKE '" & Patron & "'"

        If EsExportacion And Not EsDomestica Then
            SqlB = "SELECT 1 As Operacion,F.* FROM FacturasCabeza AS F WHERE F.Estado <> 3 " & SqlFecha & SqlEsExportacion & SqlPuntoDeVenta & SqlVendedor & ";"
            SqlN = "SELECT 2 As Operacion,F.* FROM FacturasCabeza AS F WHERE F.Estado <> 3 " & SqlFecha & SqlEsExportacion & SqlPuntoDeVenta & SqlVendedor & ";"
        Else
            SqlB = "SELECT 1 As Operacion,F.* FROM FacturasCabeza AS F WHERE F.EsExterior = 0 AND F.Estado <> 3 " & SqlFecha & SqlEsExportacion & SqlPuntoDeVenta & SqlVendedor & _
                   " UNION ALL " & _
                   "SELECT 1 As Operacion,F.* FROM FacturasCabeza AS F WHERE F.ESExterior = 1 AND F.Estado <> 3 " & SqlFecha & SqlEsExportacion & SqlPuntoDeVenta & SqlVendedor & ";"
            SqlN = "SELECT 2 As Operacion,F.* FROM FacturasCabeza AS F WHERE F.EsExterior = 0 AND F.Estado <> 3 " & SqlFecha & SqlEsExportacion & SqlPuntoDeVenta & SqlVendedor & _
                   " UNION ALL " & _
                   "SELECT 2 As Operacion,F.* FROM FacturasCabeza AS F WHERE F.EsExterior = 1 AND F.Estado <> 3 " & SqlFecha & SqlEsExportacion & SqlPuntoDeVenta & SqlVendedor & ";"
        End If

        Dim DtVentas As New DataTable
        If Abierto Then
            If Not Tablas.Read(SqlB, Conexion, DtVentas) Then Exit Sub
        End If
        If Cerrado Then
            If Not Tablas.Read(SqlN, ConexionN, DtVentas) Then Exit Sub
        End If

        If DtVentas.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim View As New DataView
        View = DtVentas.DefaultView
        View.Sort = "Fecha,Factura"

        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer
        Dim BrutoW As Decimal
        Dim SeniaW As Decimal
        Dim PercepcionesW As Decimal
        Dim TotalW As Decimal
        Dim CanceladoW As Decimal
        Dim CtaCteW As Decimal
        Dim FactDevW As Decimal
        Dim Dt As DataTable
        Dim TotalDevolucion As Decimal
        Dim TotalCredito As Decimal
        Dim TotalDebito As Decimal

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Factura"
        exHoja.Cells.Item(RowExxel, 3) = "Fecha"
        exHoja.Cells.Item(RowExxel, 4) = "Bruto+Iva"
        exHoja.Cells.Item(RowExxel, 5) = "Seña"
        exHoja.Cells.Item(RowExxel, 6) = "Ret/Perc."
        exHoja.Cells.Item(RowExxel, 7) = "Total"
        exHoja.Cells.Item(RowExxel, 8) = "Cancelado"
        exHoja.Cells.Item(RowExxel, 9) = "Deuda"
        exHoja.Cells.Item(RowExxel, 10) = "Facturado - N.Créditos"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        For Each Row As DataRowView In View
            RowExxel = RowExxel + 1
            If Row("Operacion") = 2 Then
                exHoja.Cells.Item(RowExxel, 1) = "X"
            End If
            Dim Bruto As Decimal = CDec(Row("Importe")) - CDec(Row("Senia"))
            Dim Total As Decimal = CDec(Row("Importe") + CDec(Row("Percepciones")))
            exHoja.Cells.Item(RowExxel, 2) = NumeroEditado(Row("Factura"))
            exHoja.Cells.Item(RowExxel, 3) = Format(Row("Fecha"), "MM/dd/yyyy")
            exHoja.Cells.Item(RowExxel, 4) = FormatNumber(Bruto, 2)
            If Row("Senia") <> 0 Then exHoja.Cells.Item(RowExxel, 5) = FormatNumber(Row("Senia"), 2)
            If Row("Percepciones") <> 0 Then exHoja.Cells.Item(RowExxel, 6) = FormatNumber(Row("Percepciones"), 2)
            exHoja.Cells.Item(RowExxel, 7) = FormatNumber(Total, 2)
            exHoja.Cells.Item(RowExxel, 8) = FormatNumber(Total - CDec(Row("Saldo")), 2)              'cancelado
            exHoja.Cells.Item(RowExxel, 9) = FormatNumber(Row("Saldo"), 2)                            'deuda
            '
            Dim ConexionStr As String
            If Row("Operacion") = 1 Then
                ConexionStr = Conexion
            Else
                ConexionStr = ConexionN
            End If
            If Vendedor <> 0 Then
                If Not HallaNotasDeFacturas(Row("Factura"), ConexionStr, TotalDevolucion, TotalDebito, TotalCredito, Dt) Then Exit Sub
                exHoja.Cells.Item(RowExxel, 10) = FormatNumber(Row("Importe") + Row("Percepciones") - TotalDevolucion - TotalCredito, 2)   'Facturado - Notas de credito. 
            End If
            '
            BrutoW = BrutoW + Bruto
            SeniaW = SeniaW + CDec(Row("Senia"))
            PercepcionesW = PercepcionesW + CDec(Row("Percepciones"))
            TotalW = TotalW + Total
            CanceladoW = CanceladoW + Total - CDec(Row("Saldo"))
            CtaCteW = CtaCteW + CDec(Row("Saldo"))
            If Vendedor <> 0 Then FactDevW = FactDevW + Row("Importe") + Row("Percepciones") - TotalDevolucion - TotalCredito
        Next
        '
        'Totales.
        RowExxel = RowExxel + 2
        exHoja.Cells.Item(RowExxel, 2) = "Totales"
        exHoja.Cells.Item(RowExxel, 4) = FormatNumber(BrutoW, 2)
        exHoja.Cells.Item(RowExxel, 5) = FormatNumber(SeniaW, 2)
        exHoja.Cells.Item(RowExxel, 6) = FormatNumber(PercepcionesW, 2)
        exHoja.Cells.Item(RowExxel, 7) = FormatNumber(TotalW, 2)
        exHoja.Cells.Item(RowExxel, 8) = FormatNumber(CanceladoW, 2)
        exHoja.Cells.Item(RowExxel, 9) = FormatNumber(CtaCteW, 2)
        exHoja.Cells.Item(RowExxel, 10) = FormatNumber(FactDevW, 2)

        DtVentas.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 1 - 1) & InicioDatos
        BB = Chr(65 + 1 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        ' 
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        exHoja.Rows.Item(5).Font.Bold = True
        'Muestra titulos.
        Dim VendedorW As String
        If Vendedor <> 0 Then
            VendedorW = "    Para el Vendedor: " & HallaVendedor(Vendedor)
        End If
        exHoja.Cells.Item(2, 1) = "Cobranza Facturas  Desde: " & Format(Desde, "dd/MM/yyyy") & "  Hasta: " & Format(Hasta, "dd/MM/yyyy") & VendedorW

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeEgresoCaja(ByVal Desde As Date, ByVal Hasta As Date, ByVal Abierto As Boolean, ByVal Cerrado As Boolean)

        If GProveedorEgresoCaja = 0 Then
            MsgBox("No Existe Proveedor Para Egreso Caja. Operación se CANCELA.")
            Exit Sub
        End If

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim SqlB As String = ""
        Dim SqlN As String = ""
        Dim SqlFecha As String = ""

        SqlFecha = " AND Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "

        SqlB = "SELECT 1 As Operacion,D.*,R.Nota,R.Fecha,R.ConceptoGasto FROM RecibosCabeza AS R INNER JOIN (AsientosCabeza As A INNER JOIN AsientosDetalle AS D ON A.Asiento = D.Asiento) " & _
               " ON R.Nota = A.Documento AND TipoDocumento = 607 WHERE R.Estado = 1 AND R.TipoNota = 600 AND R.Emisor = " & GProveedorEgresoCaja & SqlFecha & ";"
        SqlN = "SELECT 2 As Operacion,D.*,R.Nota,R.Fecha,R.ConceptoGasto FROM RecibosCabeza AS R INNER JOIN (AsientosCabeza As A INNER JOIN AsientosDetalle AS D ON A.Asiento = D.Asiento) " & _
               " ON R.Nota = A.Documento AND TipoDocumento = 607 WHERE R.Estado = 1 AND R.TipoNota = 600 AND R.Emisor = " & GProveedorEgresoCaja & SqlFecha & ";"

        Dim Dt As New DataTable

        If Abierto Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Exit Sub
        End If
        If Cerrado Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Exit Sub
        End If

        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Fecha,Nota,Asiento"

        Dim RowExxel As Integer

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Fecha"
        exHoja.Cells.Item(RowExxel, 3) = "Comprobante"
        exHoja.Cells.Item(RowExxel, 4) = "Asiento"
        exHoja.Cells.Item(RowExxel, 5) = "Cuenta"
        exHoja.Cells.Item(RowExxel, 6) = ""
        exHoja.Cells.Item(RowExxel, 7) = "Concepto Gasto"
        exHoja.Cells.Item(RowExxel, 8) = "Importe"
        exHoja.Cells.Item(RowExxel, 9) = "Total"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim AsientoAnt As Integer = 0
        Dim Total As Decimal = 0
        Dim TotalGral As Decimal = 0
        Dim Centro As Integer
        Dim NombreCuenta As String
        Dim Cuenta As Integer
        Dim SubCuenta As Integer
        Dim Nombre As String

        For Each Row As DataRowView In View
            If Row("Asiento") <> AsientoAnt Then
                If AsientoAnt <> 0 Then
                    exHoja.Cells.Item(RowExxel, 9) = Total
                End If
                Total = 0
                AsientoAnt = Row("Asiento")
                RowExxel = RowExxel + 1
                If Row("Operacion") = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
                exHoja.Cells.Item(RowExxel, 2) = Format(Row("Fecha"), "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel, 3) = NumeroEditado(Row("Nota"))
                exHoja.Cells.Item(RowExxel, 7) = NombreTabla(Row("ConceptoGasto"))
                exHoja.Cells.Item(RowExxel, 4) = Row("Asiento")
                RowExxel = RowExxel - 1
            End If
            If Row("Debe") <> 0 Then
                RowExxel = RowExxel + 1
                exHoja.Cells.Item(RowExxel, 5) = Format(Row("Cuenta"), "000-000000-00")
                HallaPartesCuenta(Row("Cuenta"), Centro, Cuenta, SubCuenta)
                GExcepcion = HallaDatoGenerico("SELECT Nombre FROM Cuentas WHERE Cuenta = " & Cuenta & ";", Conexion, Nombre)
                If Not IsNothing(GExcepcion) Then
                    MsgBox("Error al Leer Tabla: Cuentas." + vbCrLf + vbCrLf + GExcepcion.Message)
                    Exit Sub
                End If
                NombreCuenta = Nombre
                Dim SubCuentaW = Cuenta & Format(SubCuenta, "00")
                GExcepcion = HallaDatoGenerico("SELECT Nombre FROM SubCuentas WHERE Clave = " & SubCuentaW & ";", Conexion, Nombre)
                If Not IsNothing(GExcepcion) Then
                    MsgBox("Error al Leer Tabla: Cuentas." + vbCrLf + vbCrLf + GExcepcion.Message)
                    Exit Sub
                End If
                NombreCuenta = NombreCuenta & " - " & Nombre
                exHoja.Cells.Item(RowExxel, 6) = NombreCuenta
                exHoja.Cells.Item(RowExxel, 8) = Row("Debe")
                Total = Total + Row("Debe")
                TotalGral = TotalGral + Row("Debe")
            End If
        Next
        'Totales.
        exHoja.Cells.Item(RowExxel, 9) = Total
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 9) = TotalGral

        Dt.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        exHoja.Rows.Item(5).Font.Bold = True
        'Muestra titulos.
        exHoja.Cells.Item(2, 1) = "Gastos Egreso de Caja a Cuenta de Resultado      " & Format(Date.Now, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeSeniasValesPropios(ByVal Cliente As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Abierto As Boolean, ByVal Cerrado As Boolean)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim SqlB As String = ""
        Dim SqlN As String = ""
        Dim SqlFecha As String = ""
        Dim SqlCliente As String = ""

        If Cliente <> 0 Then
            SqlCliente = " AND C.Emisor = " & Cliente
        End If

        SqlFecha = " AND Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "

        SqlB = "SELECT 1 As Operacion,D.*,C.Nota,C.Fecha,C.Emisor,C.Caja FROM RecibosCabeza AS C INNER JOIN RecibosDetallePago AS D ON C.Nota = D.Nota " & _
               "WHERE C.Estado = 1 AND C.TipoNota = 60 AND D.MedioPago = 5" & SqlCliente & SqlFecha & ";"
        SqlN = "SELECT 2 As Operacion,D.*,C.Nota,C.Fecha,C.Emisor,C.Caja FROM RecibosCabeza AS C INNER JOIN RecibosDetallePago AS D ON C.Nota = D.Nota " & _
               "WHERE C.Estado = 1 AND C.TipoNota = 60 AND D.MedioPago = 5" & SqlCliente & SqlFecha & ";"

        Dim Dt As New DataTable

        If Abierto Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Exit Sub
        End If
        If Cerrado Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Exit Sub
        End If

        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Emisor,Fecha,Nota"

        Dim RowExxel As Integer

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Cliente"
        exHoja.Cells.Item(RowExxel, 3) = "Fecha"
        exHoja.Cells.Item(RowExxel, 4) = "Caja"
        exHoja.Cells.Item(RowExxel, 5) = "Recibo"
        exHoja.Cells.Item(RowExxel, 6) = "Nro. Vale"
        exHoja.Cells.Item(RowExxel, 7) = "Bultos"
        exHoja.Cells.Item(RowExxel, 8) = "Importe Vale"
        exHoja.Cells.Item(RowExxel, 9) = "Total"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim ClienteAnt As Integer = 0
        Dim Total As Decimal = 0
        Dim TotalGral As Decimal = 0

        For Each Row As DataRowView In View
            If Row("Emisor") <> ClienteAnt Then
                If ClienteAnt <> 0 Then
                    exHoja.Cells.Item(RowExxel, 9) = Total
                End If
                Total = 0
                ClienteAnt = Row("Emisor")
                RowExxel = RowExxel + 1
                If Row("Operacion") = 2 Then exHoja.Cells.Item(RowExxel, 1) = "X"
                exHoja.Cells.Item(RowExxel, 2) = NombreCliente(Row("Emisor"))
                exHoja.Cells.Item(RowExxel, 3) = Format(Row("Fecha"), "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel, 4) = Row("Caja")
                RowExxel = RowExxel - 1
            End If
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 5) = NumeroEditado(Row("Nota"))
            exHoja.Cells.Item(RowExxel, 6) = Row("Comprobante")
            exHoja.Cells.Item(RowExxel, 7) = Row("Bultos")
            exHoja.Cells.Item(RowExxel, 8) = Row("Importe")
            Total = Total + Row("Importe")
            TotalGral = TotalGral + Row("Importe")
        Next
        'Totales.
        exHoja.Cells.Item(RowExxel, 9) = Total
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 9) = TotalGral

        Dt.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        exHoja.Rows.Item(5).Font.Bold = True
        'Muestra titulos.
        exHoja.Cells.Item(2, 1) = "Cobranzas Señas Vales Propios                                          " & Format(Date.Now, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeFacturasCedidas(ByVal TipoDestino As Integer, ByVal Destino As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Pendientes As Boolean)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim RowExxel As Integer

        RowExxel = 4
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Factura"
        exHoja.Cells.Item(RowExxel, 3) = "Fecha Fac."
        exHoja.Cells.Item(RowExxel, 4) = "Importe"
        exHoja.Cells.Item(RowExxel, 5) = "Saldo "
        exHoja.Cells.Item(RowExxel, 6) = " "
        exHoja.Cells.Item(RowExxel, 7) = "Tipo"
        exHoja.Cells.Item(RowExxel, 8) = "Cedido A"
        exHoja.Cells.Item(RowExxel, 9) = "Fecha Cesión"
        exHoja.Cells.Item(RowExxel, 10) = "Aforo"
        exHoja.Cells.Item(RowExxel, 11) = "Interés"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim SqlB As String

        Dim SqlFecha As String = ""
        SqlFecha = " C.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlTipoDestino As String = ""
        If TipoDestino <> 0 Then
            SqlTipoDestino = " AND C.TipoDestino = " & TipoDestino
        End If
        Dim SqlDestino As String = ""
        If Destino <> 0 Then SqlDestino = " AND C.Destino = " & Destino
        Dim SqlPedientes As String = ""
        If Pendientes Then SqlPedientes = " AND C.Saldo <> 0"

        SqlB = "SELECT C.*,F.Importe,F.Saldo,F.Fecha AS FechaFactura FROM CesionFacturas AS C INNER JOIN FacturasCabeza AS F ON C.Factura = F.Factura WHERE " & SqlFecha & SqlTipoDestino & SqlDestino & SqlPedientes & ";"

        Dim Dt As New DataTable

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Exit Sub

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Factura"

        For Each Row As DataRowView In View
            RowExxel = RowExxel + 1
            exHoja.Cells.Item(RowExxel, 2) = NumeroEditado(Row("Factura"))
            exHoja.Cells.Item(RowExxel, 3) = Format(Row("FechaFactura"), "MM/dd/yyyy")
            exHoja.Cells.Item(RowExxel, 4) = Row("Importe")
            exHoja.Cells.Item(RowExxel, 5) = Row("Saldo")
            If Row("TipoDestino") = 1 Then exHoja.Cells.Item(RowExxel, 7) = "Proveedor" : exHoja.Cells.Item(RowExxel, 8) = NombreProveedor(Row("Destino"))
            If Row("TipoDestino") = 2 Then exHoja.Cells.Item(RowExxel, 7) = "Cliente" : exHoja.Cells.Item(RowExxel, 8) = NombreCliente(Row("Destino"))
            If Row("TipoDestino") = 3 Then exHoja.Cells.Item(RowExxel, 7) = "Banco" : exHoja.Cells.Item(RowExxel, 8) = NombreBanco(Row("Destino"))
            exHoja.Cells.Item(RowExxel, 9) = Format(Row("Fecha"), "MM/dd/yyyy")
            exHoja.Cells.Item(RowExxel, 10) = Row("Aforo")
            exHoja.Cells.Item(RowExxel, 11) = Row("Interes")
        Next

        Dt.Dispose()
        View = Nothing
        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        exHoja.Columns.AutoFit()

        'Muestra titulos.
        exHoja.Cells.Item(1, 17) = Format(Date.Now, "MM/dd/yyyy")
        exHoja.Cells.Item(2, 1) = "Facturas Cedidas Desde el : " & Format(Desde, "dd/MM/yyyy") & "  Hasta el " & Format(Hasta, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Private Sub ArmaArchivoAuxiliar(ByRef Dt As DataTable)

        Dt = New DataTable

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Articulo)

        Dim MontoBSinIva As New DataColumn("MontoBSinIva")
        MontoBSinIva.DataType = System.Type.GetType("System.Decimal")
        Dt.Columns.Add(MontoBSinIva)

        Dim NetoN As New DataColumn("NetoN")
        NetoN.DataType = System.Type.GetType("System.Decimal")
        Dt.Columns.Add(NetoN)

        Dim ComisionDescargaB As New DataColumn("ComisionDescargaB")
        ComisionDescargaB.DataType = System.Type.GetType("System.Decimal")
        Dt.Columns.Add(ComisionDescargaB)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        Dt.Columns.Add(Cantidad)

    End Sub
    Private Function ArmaConFacturasProveedor(ByVal Dt As DataTable, ByVal DesdeFactura As Date, ByVal HastaFactura As Date, ByVal DesdeIngreso As Date, ByVal HastaIngreso As Date, ByVal Domesticas As Boolean, ByVal Importacion As Boolean) As Boolean

        Dim SqlFechaFactura As String = ""
        SqlFechaFactura = "AND F.Fecha >='" & Format(DesdeFactura, "yyyyMMdd") & "' AND F.Fecha < DATEADD(dd,1,'" & Format(HastaFactura, "yyyyMMdd") & "') "

        Dim SqlIngreso As String = ""
        SqlIngreso = " AND C.Fecha >='" & Format(DesdeIngreso, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(HastaIngreso, "yyyyMMdd") & "') "

        Dim StrTipo As String = ""
        If Domesticas Then
            StrTipo = " AND C.Moneda = 1"
        End If
        If Importacion Then
            StrTipo = " AND C.Moneda > 1"
        End If
        If Domesticas And Importacion Then
            StrTipo = ""
        End If

        Dim DtFacturas As New DataTable
        Dim DtLotesW As New DataTable

        Dim SqlB As String = "SELECT 1 As Operacion,F.Factura,F.TipoNota,F.NotaDebito,F.Rel,F.NRel,C.Lote,C.Secuencia,C.Operacion AS OperacionLote FROM FacturasProveedorCabeza AS F INNER JOIN ComproFacturados AS C ON F.Factura = C.Factura WHERE F.Estado = 1 AND EsReventa = 1 " & SqlFechaFactura & ";"
        Dim SqlN As String = "SELECT 2 As Operacion,F.Factura,F.TipoNota,F.NotaDebito,F.Rel,F.NRel,C.Lote,C.Secuencia,C.Operacion AS OperacionLote FROM FacturasProveedorCabeza AS F INNER JOIN ComproFacturados AS C ON F.Factura = C.Factura WHERE F.Estado = 1 AND EsReventa = 1 " & SqlFechaFactura & ";"

        If Not Tablas.Read(SqlB, Conexion, DtFacturas) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtFacturas) Then Return False
        End If

        Dim FacturaB As Decimal
        Dim FacturaN As Decimal
        Dim NetoN As Decimal = 0
        Dim MontoBSinIva As Decimal = 0
        Dim ComisionDescargaB As Decimal = 0
        Dim StrLotes As String
        Dim StrConexion As String
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtFacturas.Rows
            If Row("Operacion") = 1 Then FacturaB = Row("Factura") : FacturaN = 0
            If Row("Operacion") = 2 Then FacturaB = Row("Nrel") : FacturaN = Row("Factura")
            If Not HallaImporteFacturaBlancoNegro(Row("Lote"), Row("Secuencia"), FacturaB, FacturaN, MontoBSinIva, NetoN, Row("TipoNota"), Row("NotaDebito")) Then DtFacturas.Dispose() : Return False
            StrLotes = "SELECT L.Articulo,L.Cantidad,L.Baja FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote  WHERE L.Lote = " & Row("Lote") & " AND L.Secuencia = " & Row("Secuencia") & SqlIngreso & StrTipo & ";"
            If Row("OperacionLote") = 1 Then
                StrConexion = Conexion
            Else : StrConexion = ConexionN
            End If
            DtLotesW.Clear()
            If Not Tablas.Read(StrLotes, StrConexion, DtLotesW) Then DtFacturas.Dispose() : DtLotesW.Dispose() : Return False
            If DtLotesW.Rows.Count <> 0 Then
                If Dt.Rows.Count <> 0 Then
                    RowsBusqueda = Dt.Select("Articulo = " & DtLotesW.Rows(0).Item("Articulo"))
                    If RowsBusqueda.Length <> 0 Then
                        RowsBusqueda(0).Item("MontoBSinIva") = RowsBusqueda(0).Item("MontoBSinIva") + MontoBSinIva
                        RowsBusqueda(0).Item("NetoN") = RowsBusqueda(0).Item("NetoN") + NetoN
                        RowsBusqueda(0).Item("ComisionDescargaB") = RowsBusqueda(0).Item("ComisionDescargaB") + ComisionDescargaB
                        RowsBusqueda(0).Item("Cantidad") = RowsBusqueda(0).Item("Cantidad") + CDec(DtLotesW.Rows(0).Item("Cantidad")) - CDec(DtLotesW.Rows(0).Item("Baja"))
                    Else
                        Dim Row1 As DataRow = Dt.NewRow
                        Row1("Articulo") = DtLotesW.Rows(0).Item("Articulo")
                        Row1("MontoBSinIva") = MontoBSinIva
                        Row1("NetoN") = NetoN
                        Row1("ComisionDescargaB") = ComisionDescargaB
                        Row1("Cantidad") = CDec(DtLotesW.Rows(0).Item("Cantidad")) - CDec(DtLotesW.Rows(0).Item("Baja"))
                        Dt.Rows.Add(Row1)
                    End If
                Else
                    Dim Row1 As DataRow = Dt.NewRow
                    Row1("Articulo") = DtLotesW.Rows(0).Item("Articulo")
                    Row1("MontoBSinIva") = MontoBSinIva
                    Row1("NetoN") = NetoN
                    Row1("ComisionDescargaB") = ComisionDescargaB
                    Row1("Cantidad") = CDec(DtLotesW.Rows(0).Item("Cantidad")) - CDec(DtLotesW.Rows(0).Item("Baja"))
                    Dt.Rows.Add(Row1)
                End If
            End If
        Next

        Return True

    End Function
    Private Function ArmaConLIquidacionesProveedor(ByVal Dt As DataTable, ByVal DesdeLiquidacion As Date, ByVal HastaLiquidacion As Date, ByVal DesdeIngreso As Date, ByVal HastaIngreso As Date, ByVal Domesticas As Boolean, ByVal Importacion As Boolean) As Boolean

        Dim SqlFechaFactura As String = ""
        SqlFechaFactura = "AND F.Fecha >='" & Format(DesdeLiquidacion, "yyyyMMdd") & "' AND F.Fecha < DATEADD(dd,1,'" & Format(HastaLiquidacion, "yyyyMMdd") & "') "

        Dim SqlIngreso As String = ""
        SqlIngreso = " AND C.Fecha >='" & Format(DesdeIngreso, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(HastaIngreso, "yyyyMMdd") & "') "

        Dim StrTipo As String = ""
        If Domesticas Then
            StrTipo = " AND C.Moneda = 1"
        End If
        If Importacion Then
            StrTipo = " AND C.Moneda > 1"
        End If
        If Domesticas And Importacion Then
            StrTipo = ""
        End If

        Dim DtLiquidaciones As New DataTable
        Dim DtLotesW As New DataTable

        Dim SqlB As String = "SELECT 1 As Operacion,F.Liquidacion,C.Lote,C.Secuencia,C.Operacion AS OperacionLote,C.Cantidad FROM LiquidacionCabeza AS F INNER JOIN LiquidacionDetalle AS C ON F.Liquidacion = C.Liquidacion WHERE F.Estado = 1 AND (F.EsReventa = 1 OR F.EsConsignacion = 1) " & SqlFechaFactura & ";"
        Dim SqlN As String = "SELECT 2 As Operacion,F.Liquidacion,C.Lote,C.Secuencia,C.Operacion AS OperacionLote,C.Cantidad FROM LiquidacionCabeza AS F INNER JOIN LiquidacionDetalle AS C ON F.Liquidacion = C.Liquidacion WHERE F.Estado = 1 AND (F.EsReventa = 1 OR F.EsConsignacion = 1) " & SqlFechaFactura & ";"

        If Not Tablas.Read(SqlB, Conexion, DtLiquidaciones) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtLiquidaciones) Then Return False
        End If

        Dim LiquidacionB As Decimal
        Dim LiquidacionN As Decimal

        Dim NetoN As Decimal = 0
        Dim MontoBSinIva As Decimal = 0
        Dim ComisionDescargaB As Decimal = 0
        Dim StrLotes As String
        Dim StrConexion As String
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtLiquidaciones.Rows
            If Row("Operacion") = 1 Then LiquidacionB = Row("Liquidacion") : LiquidacionN = 0
            If Row("Operacion") = 2 Then LiquidacionB = 0 : LiquidacionN = Row("Liquidacion")
            If Not HallaImporteLiquidacionBlancoNegro(Row("Lote"), Row("Secuencia"), LiquidacionB, LiquidacionN, MontoBSinIva, NetoN, ComisionDescargaB) Then DtLiquidaciones.Dispose() : DtLotesW.Dispose() : Return False
            StrLotes = "SELECT L.Articulo,L.Cantidad,L.Baja FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote  WHERE L.Lote = " & Row("Lote") & " AND L.Secuencia = " & Row("Secuencia") & SqlIngreso & StrTipo & ";"
            If Row("OperacionLote") = 1 Then
                StrConexion = Conexion
            Else : StrConexion = ConexionN
            End If
            DtLotesW.Clear()
            If Not Tablas.Read(StrLotes, StrConexion, DtLotesW) Then DtLiquidaciones.Dispose() : DtLotesW.Dispose() : Return False
            If DtLotesW.Rows.Count <> 0 Then
                If Dt.Rows.Count <> 0 Then
                    RowsBusqueda = Dt.Select("Articulo = " & DtLotesW.Rows(0).Item("Articulo"))
                    If RowsBusqueda.Length <> 0 Then
                        RowsBusqueda(0).Item("MontoBSinIva") = RowsBusqueda(0).Item("MontoBSinIva") + MontoBSinIva
                        RowsBusqueda(0).Item("NetoN") = RowsBusqueda(0).Item("NetoN") + NetoN
                        RowsBusqueda(0).Item("ComisionDescargaB") = RowsBusqueda(0).Item("ComisionDescargaB") + ComisionDescargaB
                        RowsBusqueda(0).Item("Cantidad") = RowsBusqueda(0).Item("Cantidad") + Row("Cantidad")
                    Else
                        Dim Row1 As DataRow = Dt.NewRow
                        Row1("Articulo") = DtLotesW.Rows(0).Item("Articulo")
                        Row1("MontoBSinIva") = MontoBSinIva
                        Row1("NetoN") = NetoN
                        Row1("ComisionDescargaB") = ComisionDescargaB
                        Row1("Cantidad") = Row("Cantidad")
                        Dt.Rows.Add(Row1)
                    End If
                Else
                    Dim Row1 As DataRow = Dt.NewRow
                    Row1("Articulo") = DtLotesW.Rows(0).Item("Articulo")
                    Row1("MontoBSinIva") = MontoBSinIva
                    Row1("NetoN") = NetoN
                    Row1("ComisionDescargaB") = ComisionDescargaB
                    Row1("Cantidad") = Row("Cantidad")
                    Dt.Rows.Add(Row1)
                End If
            End If
        Next

        Return True

    End Function
    Public Function HallaSucursalRemito(ByVal Remito As Decimal, ByVal Abierto As Boolean, ByRef Sucursal As String, ByRef FechaRemito As Date, ByRef ClientePorCuenta As Integer, ByRef SucursalPorCuenta As Integer) As Integer

        Dim Dt As New DataTable
        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT Sucursal,Fecha,PorCuentaYOrden,SucursalPorCuentaYOrden FROM RemitosCabeza WHERE Remito = " & Remito & ";", ConexionStr, Dt) Then End
        If Dt.Rows.Count = 0 Then
            MsgBox("Remito No Encontrado.", MsgBoxStyle.Critical)
            Dt.Dispose() : Return False
        End If
        Sucursal = Dt.Rows(0).Item("Sucursal")
        FechaRemito = Dt.Rows(0).Item("Fecha")
        SucursalPorCuenta = Dt.Rows(0).Item("SucursalPorCuentaYOrden")
        ClientePorCuenta = Dt.Rows(0).Item("PorCuentaYOrden")

        Dt.Dispose() : Return True

    End Function
    Public Function HallaNombreYCodigoExternoRemito(ByVal Cliente As Integer, ByVal Sucursal As Integer, ByRef CodigoExterno As String, ByRef NombreSucursal As String) As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Nombre,CodigoExterno FROM SucursalesClientes WHERE Cliente = " & Cliente & " AND Clave = " & Sucursal & ";", Conexion, Dt) Then End
        If Dt.Rows.Count = 0 Then
            MsgBox("Sucursal No Encontrada.", MsgBoxStyle.Critical)
            Dt.Dispose() : Return False
        End If
        CodigoExterno = Dt.Rows(0).Item("CodigoExterno")
        NombreSucursal = Dt.Rows(0).Item("Nombre")

        Dt.Dispose() : Return True

    End Function
    Private Function CortePorLote_InformeVentasYCostosDeLotes(ByVal exHoja As Microsoft.Office.Interop.Excel.Worksheet, ByRef RowExxel As Integer, ByVal OperacionW As String, ByVal TipoDocuW As Integer, ByVal TipoW As String, ByVal ComprobanteW As String, ByVal FechaW As String, ByVal ArticuloW As String, ByVal LoteW As String, ByVal CantidadAnt As Decimal, ByVal ImporteSinIva As Decimal, ByVal ImporteIva As Decimal, ByVal ClienteW As String, ByVal LoteAnt As Integer, ByVal SecuenciaAnt As Integer, ByVal OperacionLoteAnt As Integer, ByRef ListaLotes As List(Of FilaGenerica), ByVal Merma As Decimal) As Boolean

        Dim ImporteSinIvaRecibos As Decimal = 0
        Dim ImporteIvaRecibos As Decimal = 0
        Dim ImporteSinIvaLiquidado As Decimal = 0
        Dim ImporteIvaLiquidado As Decimal = 0
        Dim ImporteSinIvaMerma As Decimal = 0
        Dim ImporteIvaMerma As Decimal = 0
        Dim Cartel As String
        Dim Facturado As Boolean = False

        Dim LoteOrigen As Integer
        Dim SecuenciaOrigen As Integer
        Dim DepositoOrigen As Integer
        Dim TipoOperacion As Integer
        Dim KilosXUnidad As Decimal = 0
        Dim NombreProveedorW As String = ""
        Dim FechaIngreso As Date
        'Halla lote origen.
        If Not HallaLoteOrigen(LoteAnt, SecuenciaAnt, OperacionLoteAnt, LoteOrigen, SecuenciaOrigen, DepositoOrigen, NombreProveedorW, TipoOperacion, KilosXUnidad, FechaIngreso) Then Return False

        Dim CantidadParaCostosAsignados As Decimal = CantidadAnt
        If TipoDocuW = 2 And Merma > 0 Then CantidadParaCostosAsignados = CantidadParaCostosAsignados + Merma

        For Each Fila1 As FilaGenerica In ListaLotes
            If Fila1.Lote = LoteAnt And Fila1.Secuencia = SecuenciaAnt Then
                ImporteSinIvaLiquidado = Trunca(CantidadAnt * Fila1.Importe1)
                ImporteIvaLiquidado = Trunca(CantidadAnt * Fila1.Importe2)
                ImporteSinIvaRecibos = Trunca(CantidadParaCostosAsignados * Fila1.Importe3)
                ImporteIvaRecibos = Trunca(CantidadParaCostosAsignados * Fila1.Importe4)
                ImporteSinIvaMerma = Trunca(Merma * Fila1.Importe5)
                ImporteIvaMerma = Trunca(Merma * Fila1.Importe6)
                If TipoOperacion = 4 Then
                    Dim Costeo As Integer = HallaCosteoLote(OperacionLoteAnt, LoteOrigen)
                    If Costeo < 0 Then Return False
                    If Not HallaCosteoCerrado(Costeo) Then
                        Cartel = "Costo Estimado"
                    End If
                Else
                    If Fila1.Importe1 = 0 Then
                        Cartel = "Pendiente de Liquidar"
                    Else
                        Cartel = ""
                    End If
                End If
                AgregaAExcel(exHoja, RowExxel, OperacionW, TipoW, ComprobanteW, FechaW, ArticuloW, LoteW, CantidadAnt, ImporteSinIva, ImporteIva, ImporteSinIva + ImporteIva, ClienteW, ImporteSinIvaLiquidado, ImporteIvaLiquidado, ImporteSinIvaLiquidado + ImporteIvaLiquidado, Cartel, ImporteSinIvaRecibos, ImporteIvaRecibos, ImporteSinIvaRecibos + ImporteIvaRecibos, ImporteSinIvaMerma, ImporteIvaMerma, ImporteSinIvaMerma + ImporteIvaMerma, Fila1.String1, "1800/01/01")
                Return True
            End If
        Next

        Dim CantidadLoteLiquidado As Decimal
        Dim KilosXUnidadLiquidado As Decimal
        Dim MermaLiquidada As Decimal
        Dim DescarteLiquidada As Decimal
        Dim CantidadLote As Decimal
        Dim BajaLote As Decimal
        'Halla Datos del Lote Origen.
        If Not HallaCanidadLiquidadaParaCostoRealDelLote(LoteOrigen, SecuenciaOrigen, DepositoOrigen, OperacionLoteAnt, CantidadLoteLiquidado, KilosXUnidadLiquidado, CantidadLote, BajaLote, MermaLiquidada, DescarteLiquidada) Then Return False

        'Halla Importe sin iva e Iva del lote origen Liquidado. 
        If Not HallaLiquidadoAProveedoresPorLote(LoteOrigen, SecuenciaOrigen, DepositoOrigen, OperacionLoteAnt, TipoOperacion, ImporteSinIvaLiquidado, ImporteIvaLiquidado, Facturado) Then Return False

        Dim Fila As New FilaGenerica
        Fila.Lote = LoteAnt
        Fila.Secuencia = SecuenciaAnt
        If TipoOperacion = 4 Then
            Fila.Importe1 = KilosXUnidad * ImporteSinIvaLiquidado  'Importe sin iva del lote origen liquidado por unidad del LoteAnt.
            Fila.Importe2 = KilosXUnidad * ImporteIvaLiquidado     'Importe iva del lote origen liquidado por unidad del LoteAnt.
        Else
            If CantidadLoteLiquidado <> 0 Then
                Fila.Importe1 = KilosXUnidad * (ImporteSinIvaLiquidado / (CantidadLoteLiquidado * KilosXUnidadLiquidado)) 'Importe sin iva del lote origen liquidado por unidad del LoteAnt.
                Fila.Importe2 = KilosXUnidad * (ImporteIvaLiquidado / (CantidadLoteLiquidado * KilosXUnidadLiquidado))    'Importe iva del lote origen liquidado por unidad del LoteAnt.
            End If
        End If

        ImporteSinIvaLiquidado = Trunca(CantidadAnt * Fila.Importe1)
        ImporteIvaLiquidado = Trunca(CantidadAnt * Fila.Importe2)

        If TipoOperacion = 4 Then
            Dim Costeo As Integer = HallaCosteoLote(OperacionLoteAnt, LoteOrigen)
            If Costeo < 0 Then Return False
            If Not HallaCosteoCerrado(Costeo) Then
                Cartel = "Costo Estimado"
            End If
        Else
            If ImporteSinIvaLiquidado = 0 Then
                Cartel = "Pendiente de Facturación/Liquidación"
            Else
                Cartel = ""
            End If
        End If

        'Halla Importe sin iva e Iva del costo asignados a lotes de lote origen Liquidado. 
        GConListaDeCostos = False
        If Not CostosAsinadosPorLote(LoteOrigen, SecuenciaOrigen, ImporteSinIvaRecibos, ImporteIvaRecibos) Then Return False
        Dim CantidadLoteLiquidadoWW As Decimal = 0
        CantidadLoteLiquidadoWW = CantidadLote - BajaLote
        If CantidadLoteLiquidadoWW <> 0 Then
            Fila.Importe3 = KilosXUnidad * (ImporteSinIvaRecibos / (CantidadLoteLiquidadoWW * KilosXUnidadLiquidado)) 'Importe sin iva del lote origen liquidado por Unidad del LoteAnt.
            Fila.Importe4 = KilosXUnidad * (ImporteIvaRecibos / (CantidadLoteLiquidadoWW * KilosXUnidadLiquidado))    'Importe iva del lote origen liquidado por Unidad del LoteAnt.
        End If
        '
        ImporteSinIvaRecibos = Trunca(CantidadParaCostosAsignados * Fila.Importe3)
        ImporteIvaRecibos = Trunca(CantidadParaCostosAsignados * Fila.Importe4)

        'Halla costo de merma para las NVLP.
        If (TipoOperacion = 2 Or TipoOperacion = 4) And Merma > 0 Then  'solo si es NVLP y si es reventa o cosreo.
            Fila.Importe5 = Fila.Importe1
            Fila.Importe6 = Fila.Importe2
            ImporteSinIvaMerma = Trunca(Merma * Fila.Importe5)
            ImporteIvaMerma = Trunca(Merma * Fila.Importe6)
        End If

        Fila.String1 = NombreProveedorW
        ListaLotes.Add(Fila)

        If Not Facturado Then Cartel = "Pendiente de Facturación/Liquidación"
        AgregaAExcel(exHoja, RowExxel, OperacionW, TipoW, ComprobanteW, FechaW, ArticuloW, LoteW, CantidadAnt, ImporteSinIva, ImporteIva, ImporteSinIva + ImporteIva, ClienteW, ImporteSinIvaLiquidado, ImporteIvaLiquidado, ImporteSinIvaLiquidado + ImporteIvaLiquidado, Cartel, ImporteSinIvaRecibos, ImporteIvaRecibos, ImporteSinIvaRecibos + ImporteIvaRecibos, ImporteSinIvaMerma, ImporteIvaMerma, ImporteSinIvaMerma + ImporteIvaMerma, NombreProveedorW, FechaIngreso)

        Return True

    End Function
    Private Function HallaLiquidadoAProveedoresPorLote(ByVal LoteOrigen As Integer, ByVal SecuenciaOrigen As Integer, ByVal DepositoOrigen As Integer, ByVal Operacion As Integer, ByVal TipoOperacion As Integer, ByRef ImporteSinIva As Decimal, ByRef ImporteIva As Decimal, ByRef Facturado As Boolean) As Boolean

        Dim ConexionStr As String
        Dim Dt As DataTable

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        ImporteSinIva = 0
        ImporteIva = 0

        Dim ImporteConIvaW As Decimal = 0
        Dim ImporteSinIvaW As Decimal = 0
        Dim ImporteIvaW As Decimal = 0

        If TipoOperacion = 4 Then
            If Not HallaCostoCosteoXKilo(Operacion, LoteOrigen, ImporteConIvaW, ImporteSinIvaW) Then Return False
            ImporteSinIva = ImporteSinIvaW
            ImporteIva = ImporteConIvaW - ImporteSinIvaW
            Return True
        End If

        Dt = New DataTable
        If Not Tablas.Read("SELECT NetoSinIva,NetoConIva,Cantidad FROM LiquidacionCabeza As C INNER JOIN LiquidacionDetalle AS D ON C.liquidacion = D.Liquidacion WHERE C.Estado = 1  AND D.Lote = " & LoteOrigen & " AND D.Secuencia = " & SecuenciaOrigen & ";", Conexion, Dt) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read("SELECT NetoSinIva,NetoConIva,Cantidad FROM LiquidacionCabeza As C INNER JOIN LiquidacionDetalle AS D ON C.liquidacion = D.Liquidacion WHERE C.Estado = 1 AND D.Lote = " & LoteOrigen & " AND D.Secuencia = " & SecuenciaOrigen & ";", ConexionN, Dt) Then Return False
        End If
        If Dt.Rows.Count <> 0 Then Facturado = True

        For Each Row As DataRow In Dt.Rows
            ImporteConIvaW = ImporteConIvaW + Row("NetoConIva")
            ImporteSinIvaW = ImporteSinIvaW + Row("NetoSinIva")
        Next
        ImporteIvaW = ImporteIvaW + ImporteConIvaW - ImporteSinIvaW

        If ImporteSinIvaW <> 0 Then
            ImporteSinIva = ImporteSinIvaW
            ImporteIva = ImporteIvaW
            Dt.Dispose()
            Return True
        End If

        Dt = New DataTable
        If Not Tablas.Read("SELECT ImporteSinIva,ImporteConIva FROM FacturasProveedorCabeza AS C INNER JOIN ComproFacturados AS D ON C.Factura = D.Factura WHERE C.Estado = 1 AND EsReventa = 1 AND D.Lote = " & LoteOrigen & " AND D.Secuencia = " & SecuenciaOrigen & ";", Conexion, Dt) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read("SELECT ImporteSinIva,ImporteConIva FROM FacturasProveedorCabeza AS C INNER JOIN ComproFacturados AS D ON C.Factura = D.Factura WHERE C.Estado = 1 AND EsReventa = 1 AND D.Lote = " & LoteOrigen & " AND D.Secuencia = " & SecuenciaOrigen & ";", ConexionN, Dt) Then Return False
        End If
        If Dt.Rows.Count <> 0 Then Facturado = True

        For Each Row As DataRow In Dt.Rows
            ImporteConIvaW = ImporteConIvaW + Row("ImporteConIva")
            ImporteSinIvaW = ImporteSinIvaW + Row("ImporteSinIva")
        Next
        ImporteIvaW = ImporteIvaW + (ImporteConIvaW - ImporteSinIvaW)

        ImporteSinIva = ImporteSinIvaW
        ImporteIva = ImporteIvaW

        Dt.Dispose()

        Return True

    End Function
    Private Function CostosAsinadosPorLote(ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef ImporteSinIva As Decimal, ByRef ImporteIva As Decimal) As Boolean

        Dim ImporteConIva As Decimal = 0
        ImporteSinIva = 0
        ImporteIva = 0

        'Halla costo de reintegros para lotes primarios y reprocesos.
        Dim ImporteLotesReintegrosConIva As Decimal = 0
        Dim ImporteLotesReintegrosSinIva As Decimal = 0
        If Not HallaImportesLotesReintegros(Lote, Secuencia, ImporteLotesReintegrosConIva, ImporteLotesReintegrosSinIva) Then Return False
        '
        'Halla costo de lotes en Notas credito/debito.
        Dim ImporteLotesRecibosConIva As Decimal = 0
        Dim ImporteLotesRecibosSinIva As Decimal = 0
        If Not HallaImportesLotesRecibos(Lote, Secuencia, ImporteLotesRecibosConIva, ImporteLotesRecibosSinIva) Then Return False
        '
        ' Halla costos de lotes en Otras facturas.
        Dim ImporteLotesOtrasFacturasConIva As Decimal = 0
        Dim ImporteLotesOtrasfacturasSinIva As Decimal = 0
        If Not HallaImportesLotesOtrasFacturas(Lote, Secuencia, ImporteLotesOtrasFacturasConIva, ImporteLotesOtrasfacturasSinIva) Then Return False
        '
        ' Halla costos de lotes en Asientos Manuales.
        Dim ImporteLotesAsientosManualesConIva As Decimal = 0
        Dim ImporteLotesAsientosManualesSinIva As Decimal = 0
        If Not HallaImportesLotesAsientosManuales(Lote, Secuencia, ImporteLotesAsientosManualesConIva, ImporteLotesAsientosManualesSinIva) Then Return False
        '
        ' Halla costos de lotes en facturas que afectasn lotes.
        Dim ImporteFacturasAfectanLotesConIva As Decimal = 0
        Dim ImporteFacturasAfectanLotesSinIva As Decimal = 0
        If Not FacturaAfectaLotes(Lote, Secuencia, ImporteFacturasAfectanLotesConIva, ImporteFacturasAfectanLotesSinIva) Then Return False
        '
        ' Halla costos de lotes por insumos.
        Dim ImporteInsumosConIva As Decimal = 0
        Dim ImporteInsumosSinIva As Decimal = 0
        If HallaCostoInsumosPorLote(Lote, Secuencia, ImporteInsumosConIva, ImporteInsumosSinIva) < 0 Then Return False
        '

        ImporteConIva = ImporteFacturasAfectanLotesConIva + ImporteInsumosConIva + ImporteLotesRecibosConIva - ImporteLotesReintegrosConIva + ImporteLotesOtrasFacturasConIva _
                      + ImporteLotesAsientosManualesConIva

        ImporteSinIva = ImporteFacturasAfectanLotesSinIva + ImporteInsumosSinIva + ImporteLotesRecibosSinIva - ImporteLotesReintegrosSinIva + ImporteLotesOtrasfacturasSinIva _
                      + ImporteLotesAsientosManualesSinIva

        ImporteIva = ImporteConIva - ImporteSinIva

        Return True

    End Function
    Private Function HallaCanidadLiquidadaParaCostoRealDelLote(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Deposito As Integer, ByVal Operacion As Integer, ByRef CantidadLiquidada As Decimal, ByRef KilosXUnidad As Decimal, ByRef Cantidad As Decimal, ByRef Baja As Decimal, ByRef Merma As Decimal, ByRef Descarte As Decimal) As Boolean

        Dim ConexionStr As String
        Dim Dt As New DataTable
        Dim CantidadIngresada As Decimal

        Cantidad = 0
        Baja = 0
        Descarte = 0
        Merma = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT Lote,Secuencia,Cantidad,Baja,TipoOperacion,Merma,Descarte,KilosXUnidad,Deposito,LoteOrigen,SecuenciaOrigen,DepositoOrigen FROM Lotes WHERE LoteOrigen = " & Lote & " AND SecuenciaOrigen = " & Secuencia & " AND DepositoOrigen = " & Deposito & ";", ConexionStr, Dt) Then Return False
        If Dt.Rows.Count = 0 Then Return False
        For Each Row As DataRow In Dt.Rows
            If Row("lote") = Row("loteOrigen") And Row("Secuencia") = Row("SecuenciaOrigen") And Row("Deposito") = Row("DepositoOrigen") Then
                CantidadIngresada = Row("Cantidad")
                Cantidad = Row("cantidad")
                Baja = Row("Baja")
                Descarte = Row("Descarte")
                KilosXUnidad = Row("KilosXUnidad")
            End If
            Merma = Merma + Row("Merma") * Row("KilosXUnidad")
        Next

        Merma = Trunca(Merma / KilosXUnidad)

        If Dt.Rows(0).Item("TipoOperacion") = 2 Or Dt.Rows(0).Item("TipoOperacion") = 4 Then
            CantidadLiquidada = CantidadIngresada - Baja
        Else
            CantidadLiquidada = CantidadIngresada - Baja - Merma - Descarte   'Si es condignacion.
        End If

        Dt.Dispose()
        Return True

    End Function
    Private Sub AgregaAExcel(ByVal ExHoja As Microsoft.Office.Interop.Excel.Worksheet, ByVal RowExxel As Integer, ByVal Operacion As String, ByVal Tipo As String, ByVal Comprobante As String, ByVal Fecha As String, ByVal Articulo As String, ByVal Lote As String, ByVal Cantidad As Decimal, ByVal ImporteSinIva As Decimal, ByVal ImporteIva As Decimal, ByVal TotalVenta As Decimal, ByVal Cliente As String, ByVal ImporteSinIvaLiquidado As Decimal, ByVal ImporteIvaLiquidado As Decimal, ByVal TotalCompra As Decimal, ByVal Cartel As String, ByVal ImporteSinIvaRecibos As Decimal, ByVal ImporteIvaRecibos As Decimal, ByVal TotalRecibos As Decimal, ByVal ImporteSinIvaMerma As Decimal, ByVal ImporteIvaMerma As Decimal, ByVal TotalCostosMerma As Decimal, ByVal NombreProveedor As String, ByVal FechaIngreso As Date)

        ExHoja.Cells.Item(RowExxel, 1) = Operacion
        ExHoja.Cells.Item(RowExxel, 2) = Tipo
        ExHoja.Cells.Item(RowExxel, 3) = Comprobante
        ExHoja.Cells.Item(RowExxel, 4) = Fecha
        ExHoja.Cells.Item(RowExxel, 5) = Lote
        ExHoja.Cells.Item(RowExxel, 6) = Articulo
        ExHoja.Cells.Item(RowExxel, 7) = Cantidad
        ExHoja.Cells.Item(RowExxel, 8) = ImporteSinIva
        ExHoja.Cells.Item(RowExxel, 9) = ImporteIva
        ExHoja.Cells.Item(RowExxel, 10) = TotalVenta
        ExHoja.Cells.Item(RowExxel, 11) = Cliente
        '
        ExHoja.Cells.Item(RowExxel, 13) = ImporteSinIvaLiquidado
        ExHoja.Cells.Item(RowExxel, 14) = ImporteIvaLiquidado
        ExHoja.Cells.Item(RowExxel, 15) = TotalCompra
        ExHoja.Cells.Item(RowExxel, 16) = ImporteSinIvaMerma
        ExHoja.Cells.Item(RowExxel, 17) = ImporteIvaMerma
        ExHoja.Cells.Item(RowExxel, 18) = TotalCostosMerma
        ExHoja.Cells.Item(RowExxel, 19) = NombreProveedor
        '
        ExHoja.Cells.Item(RowExxel, 21) = ImporteSinIvaRecibos
        ExHoja.Cells.Item(RowExxel, 22) = ImporteIvaRecibos
        ExHoja.Cells.Item(RowExxel, 23) = TotalRecibos
        If FechaIngreso <> "1800/01/01" Then ExHoja.Cells.Item(RowExxel, 25) = Format(FechaIngreso, "MM/dd/yyyy")
        ExHoja.Cells.Item(RowExxel, 26) = Cartel

    End Sub
    Private Sub AgregaAExcelParaCostoMerma(ByVal ExHoja As Microsoft.Office.Interop.Excel.Worksheet, ByVal RowExxel As Integer, ByVal Operacion As String, ByVal Fecha As String, ByVal Articulo As String, ByVal Lote As String, ByVal Cantidad As Decimal, ByVal Merma As Decimal, ByVal ImporteSinIvaLiquidado As Decimal, ByVal ImporteIvaLiquidado As Decimal, ByVal TotalCompra As Decimal, ByVal Cartel As String, ByVal ImporteSinIvaRecibos As Decimal, ByVal ImporteIvaRecibos As Decimal, ByVal TotalRecibos As Decimal, ByVal NombreProveedor As String)

        ExHoja.Cells.Item(RowExxel, 1) = Operacion
        ExHoja.Cells.Item(RowExxel, 2) = Lote
        ExHoja.Cells.Item(RowExxel, 3) = Fecha
        ExHoja.Cells.Item(RowExxel, 4) = Articulo
        ExHoja.Cells.Item(RowExxel, 5) = Cantidad
        ExHoja.Cells.Item(RowExxel, 6) = Merma
        ExHoja.Cells.Item(RowExxel, 7) = ImporteSinIvaLiquidado
        ExHoja.Cells.Item(RowExxel, 8) = ImporteIvaLiquidado
        ExHoja.Cells.Item(RowExxel, 9) = TotalCompra
        ExHoja.Cells.Item(RowExxel, 10) = Cartel
        ExHoja.Cells.Item(RowExxel, 11) = ImporteSinIvaRecibos
        ExHoja.Cells.Item(RowExxel, 12) = ImporteIvaRecibos
        ExHoja.Cells.Item(RowExxel, 13) = TotalRecibos
        ExHoja.Cells.Item(RowExxel, 14) = NombreProveedor

    End Sub
    Private Function EstaRemitoEnTextBox(ByVal Remito As Decimal, ByVal TextRemitos As TextBox) As Boolean

        Dim MiArray() As String

        For I As Integer = 0 To TextRemitos.Lines.Length - 1
            MiArray = Split(TextRemitos.Lines(I), "-")
            Dim Numero As Decimal
            If MiArray.Length = 2 Then
                Numero = Val(MiArray(0) & MiArray(1))
            Else
                Numero = Val(TextRemitos.Lines(I))
            End If
            If Numero = Remito Then Return True
        Next

        Return False

    End Function
    Private Function HallaArticulosDeComprobantes(ByVal Deposito As Integer, ByVal ListaComprobantes As List(Of Decimal), ByVal ListaComprobantesCerrado As List(Of Decimal)) As List(Of ItemPedido)

        Dim ListaArticulos As New List(Of ItemPedido)

        Dim SqlDeposito As String = ""
        If Deposito <> 0 Then SqlDeposito = " AND C.Deposito = " & Deposito

        Dim Dt As New DataTable

        For Each Fila As Decimal In ListaComprobantes
            If Fila.ToString.Length < 13 Then
                If Not Tablas.Read("SELECT D.Articulo,D.Cantidad - D.Devueltas AS Cantidad FROM RemitosCabeza AS C INNER JOIN RemitosDetalle AS D ON C.Remito = D.Remito WHERE C.Estado = 1 AND C.Remito = " & Fila & SqlDeposito & ";", Conexion, Dt) Then End
            Else
                If Not Tablas.Read("SELECT D.Articulo,D.Cantidad - D.Devueltas AS Cantidad FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE C.Estado = 1 AND C.Factura = " & Fila & SqlDeposito & ";", Conexion, Dt) Then End
            End If
        Next
        For Each Fila As Decimal In ListaComprobantesCerrado
            If Fila.ToString.Length < 13 Then
                If Not Tablas.Read("SELECT D.Articulo,D.Cantidad - D.Devueltas AS Cantidad FROM RemitosCabeza AS C INNER JOIN RemitosDetalle AS D ON C.Remito = D.Remito WHERE C.Estado = 1 AND C.Remito = " & Fila & SqlDeposito & ";", ConexionN, Dt) Then End
            Else
                If Not Tablas.Read("SELECT D.Articulo,D.Cantidad - D.Devueltas AS Cantidad FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE C.Estado = 1 AND C.Factura = " & Fila & SqlDeposito & ";", ConexionN, Dt) Then End
            End If
        Next

        Dim Esta As Boolean

        For Each Row As DataRow In Dt.Rows
            Esta = False
            For Each Fila As ItemPedido In ListaArticulos
                If Fila.Articulo = Row("Articulo") Then
                    Fila.Cantidad = Fila.Cantidad + Row("Cantidad") : Esta = True
                End If
            Next
            If Not Esta Then
                Dim FilaW As New ItemPedido
                FilaW.Articulo = Row("Articulo")
                FilaW.Cantidad = Row("Cantidad")
                ListaArticulos.Add(FilaW)
            End If
        Next

        Dt.Dispose()
        Return ListaArticulos

    End Function
    Public Sub InformeDebitosCreditosPorFacturas(ByVal Cliente As Integer, ByVal Desde As Date, ByVal Hasta As Date)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Dim SqlB As String = ""
        Dim SqlN As String = ""
        Dim SqlFecha As String = ""
        SqlFecha = "AND Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlFechaContable As String = ""
        SqlFechaContable = "AND F.FechaContable >='" & Format(Desde, "yyyyMMdd") & "' AND F.FechaContable < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "') "
        Dim SqlCliente As String = ""
        If Cliente <> 0 Then
            SqlCliente = "AND Cliente = " & Cliente
        End If

        '------------------------------------------------------------------
        Dim DtFacturas As New DataTable
        SqlB = "SELECT 1 As Operacion,F.Factura,F.Cliente,F.Rel,F.Relacionada AS NRel,F.FechaContable AS Fecha,F.Importe + F.Percepciones AS ImporteT FROM FacturasCabeza AS F WHERE F.Tr = 0 AND EsExterior = 0 AND F.Estado <> 3 " & SqlFechaContable & SqlCliente & ";"
        SqlN = "SELECT 2 As Operacion,F.Factura,F.Cliente,F.Rel,F.Relacionada AS NRel,F.FechaContable AS Fecha,F.Importe AS ImporteT FROM FacturasCabeza AS F WHERE F.Tr = 0 AND EsExterior = 0 AND F.Estado <> 3 " & SqlFechaContable & SqlCliente & ";"

        '       Dim SqlNotas As String = "SELECT 1 AS TipoNota,NotaCredito AS Nota,Importe + Percepciones AS ImporteT,Fecha FROM NotasCreditoCabeza WHERE Estado <> 3 AND Factura = XXX" & _
        '                 " UNION ALL " & _
        '                "SELECT C.TipoNota,C.Nota,D.Importe AS ImporteT,C.FechaContable as Fecha FROM RecibosCabeza as C INNER JOIN RecibosDetalle AS D ON C.TipoNota = D.TipoNota AND C.Nota = D.Nota Where C.Estado = 1 AND (C.TipoNota = 50 or C.TipoNota = 70) AND D.TipoComprobante = 2 AND D.Comprobante = XXX " & _
        '               " UNION ALL " & _
        '              "SELECT C.TipoNota,C.Nota,D.Importe AS ImporteT,C.Fecha FROM RecibosCabeza as C INNER JOIN RecibosDetalle AS D ON C.TipoNota = D.TipoNota AND C.Nota = D.Nota Where C.Estado = 1 AND (C.TipoNota = 60 or C.TipoNota = 5 or C.TipoNota = 13005 or C.TipoNota = 7 or C.TipoNota = 13007) AND D.TipoComprobante = 2 AND D.Comprobante = XXX" & ";"

        If Not Tablas.Read(SqlB, Conexion, DtFacturas) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtFacturas) Then Exit Sub
        End If

        If DtFacturas.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Information)
            Exit Sub
        End If

        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Cliente"
        exHoja.Cells.Item(RowExxel, 3) = "Factura"
        exHoja.Cells.Item(RowExxel, 4) = "Fecha"
        exHoja.Cells.Item(RowExxel, 5) = "Importe C/Iva"
        exHoja.Cells.Item(RowExxel, 6) = " "
        exHoja.Cells.Item(RowExxel, 7) = "TipoNota"
        exHoja.Cells.Item(RowExxel, 8) = "Nota"
        exHoja.Cells.Item(RowExxel, 9) = "Fecha"
        exHoja.Cells.Item(RowExxel, 10) = "Importe C/Iva"
        exHoja.Cells.Item(RowExxel, 11) = "Saldo"
        'Parte N
        exHoja.Cells.Item(RowExxel, 12) = " "
        exHoja.Cells.Item(RowExxel, 13) = "Factura"
        exHoja.Cells.Item(RowExxel, 14) = "Importe Iva"
        exHoja.Cells.Item(RowExxel, 15) = " "
        exHoja.Cells.Item(RowExxel, 16) = "TipoNota"
        exHoja.Cells.Item(RowExxel, 17) = "Nota"
        exHoja.Cells.Item(RowExxel, 18) = "Fecha"
        exHoja.Cells.Item(RowExxel, 19) = "Importe"
        exHoja.Cells.Item(RowExxel, 20) = "Saldo"
        exHoja.Cells.Item(RowExxel, 21) = " "
        exHoja.Cells.Item(RowExxel, 22) = "Saldo Total"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        Dim View As New DataView
        View = DtFacturas.DefaultView
        View.Sort = "Fecha,Factura"
        Dim DtNotas As New DataTable

        Dim RowsBusqueda() As DataRow
        Dim FacturaB As Decimal
        Dim FacturaN As Decimal
        Dim SaldoB As Decimal
        Dim SaldoN As Decimal
        Dim Inicio As Integer
        Dim Final As Integer = RowExxel
        Dim Cartel As String = ""
        Dim TotalCreditoDevoluion As Decimal
        Dim TotalDebito As Decimal
        Dim TotalCredito As Decimal

        For Each Row As DataRowView In View
            FacturaB = 0
            FacturaN = 0
            SaldoB = 0
            SaldoN = 0
            RowExxel = Final + 1
            Inicio = RowExxel
            exHoja.Cells.Item(RowExxel, 2) = NombreCliente(Row("Cliente"))
            exHoja.Cells.Item(RowExxel, 4) = Format(Row("Fecha"), "MM/dd/yyy")
            If Row("Operacion") = 1 Then
                exHoja.Cells.Item(RowExxel, 3) = NumeroEditado(Row("Factura"))
                FacturaB = Row("Factura")
                exHoja.Cells.Item(RowExxel, 5) = FormatNumber(Row("ImporteT"), 2)
                exHoja.Cells.Item(RowExxel, 11) = FormatNumber(Row("ImporteT"), 2)
                SaldoB = Row("ImporteT")
                If Row("Rel") Then
                    RowsBusqueda = DtFacturas.Select("NRel = " & Row("Factura"))
                    exHoja.Cells.Item(RowExxel, 13) = NumeroEditado(RowsBusqueda(0).Item("Factura"))
                    FacturaN = RowsBusqueda(0).Item("Factura")
                    exHoja.Cells.Item(RowExxel, 14) = FormatNumber(RowsBusqueda(0).Item("ImporteT"), 2)
                    exHoja.Cells.Item(RowExxel, 20) = FormatNumber(RowsBusqueda(0).Item("ImporteT"), 2)
                    SaldoN = RowsBusqueda(0).Item("ImporteT")
                End If
            Else
                If Not Row("Rel") Then
                    exHoja.Cells.Item(RowExxel, 13) = NumeroEditado(Row("Factura"))
                    FacturaN = Row("Factura")
                    exHoja.Cells.Item(RowExxel, 14) = FormatNumber(Row("ImporteT"), 2)
                    exHoja.Cells.Item(RowExxel, 20) = FormatNumber(Row("ImporteT"), 2)
                    SaldoN = Row("ImporteT")
                End If
            End If
            If FacturaB <> 0 Then
                DtNotas = New DataTable
                If Not HallaNotasDeFacturas(FacturaB, Conexion, TotalCreditoDevoluion, TotalDebito, TotalCredito, DtNotas) Then Exit Sub
                For Each Row1 As DataRow In DtNotas.Rows
                    Select Case Row1.Item("TipoNota")
                        Case 1, 7, 13007, 70, 60
                            Row1("ImporteT") = -Row1("ImporteT")
                    End Select
                    exHoja.Cells.Item(RowExxel, 7) = HallaNombreTipoNota(Row1("TipoNota"))
                    exHoja.Cells.Item(RowExxel, 8) = NumeroEditado(Row1("Nota"))
                    exHoja.Cells.Item(RowExxel, 9) = Format(Row1("Fecha"), "MM/dd/yyy")
                    exHoja.Cells.Item(RowExxel, 10) = FormatNumber(Row1("ImporteT"), 2)
                    SaldoB = SaldoB + Row1("ImporteT")
                    exHoja.Cells.Item(RowExxel, 11) = FormatNumber(SaldoB, 2)
                    RowExxel = RowExxel + 1
                Next
                Final = RowExxel
            End If
            If FacturaN <> 0 Then
                RowExxel = Inicio
                DtNotas = New DataTable
                If Not HallaNotasDeFacturas(FacturaN, ConexionN, TotalCreditoDevoluion, TotalDebito, TotalCredito, DtNotas) Then Exit Sub
                For Each Row1 As DataRow In DtNotas.Rows
                    Select Case Row1.Item("TipoNota")
                        Case 1, 7, 13007, 5, 60
                            Row1("ImporteT") = -Row1("ImporteT")
                    End Select
                    exHoja.Cells.Item(RowExxel, 16) = HallaNombreTipoNota(Row1("TipoNota"))
                    exHoja.Cells.Item(RowExxel, 17) = NumeroEditado(Row1("Nota"))
                    exHoja.Cells.Item(RowExxel, 18) = Format(Row1("Fecha"), "MM/dd/yyy")
                    exHoja.Cells.Item(RowExxel, 19) = FormatNumber(Row1("ImporteT"), 2)
                    SaldoN = SaldoN + Row1("ImporteT")
                    exHoja.Cells.Item(RowExxel, 20) = FormatNumber(SaldoN, 2)
                    RowExxel = RowExxel + 1
                Next
                If RowExxel > Final Then Final = RowExxel
            End If
            exHoja.Cells.Item(Inicio, 22) = FormatNumber(SaldoB + SaldoN, 2)
        Next
        '---------------------------------------------------------------------------
        DtFacturas.Dispose()
        DtNotas.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 1 - 1) & InicioDatos
        BB = Chr(65 + 1 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        '
        AA = Chr(65 + 2 - 1) & InicioDatos
        BB = Chr(65 + 2 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        ' 
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        ' 
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        ' 
        AA = Chr(65 + 8 - 1) & InicioDatos
        BB = Chr(65 + 8 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 9 - 1) & InicioDatos
        BB = Chr(65 + 9 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        ' 
        AA = Chr(65 + 10 - 1) & InicioDatos
        BB = Chr(65 + 10 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 11 - 1) & InicioDatos
        BB = Chr(65 + 11 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 13 - 1) & InicioDatos
        BB = Chr(65 + 13 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 14 - 1) & InicioDatos
        BB = Chr(65 + 14 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 15 - 1) & InicioDatos
        BB = Chr(65 + 15 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 16 - 1) & InicioDatos
        BB = Chr(65 + 16 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
        ' 
        AA = Chr(65 + 17 - 1) & InicioDatos
        BB = Chr(65 + 17 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 18 - 1) & InicioDatos
        BB = Chr(65 + 18 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
        ' 
        AA = Chr(65 + 19 - 1) & InicioDatos
        BB = Chr(65 + 19 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        ' 
        AA = Chr(65 + 20 - 1) & InicioDatos
        BB = Chr(65 + 20 - 1) & UltimaLinea
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"

        If Not PermisoTotal Then
            exHoja.Columns(22).delete()
            exHoja.Columns(21).delete()
            exHoja.Columns(20).delete()
            exHoja.Columns(19).delete()
            exHoja.Columns(18).delete()
            exHoja.Columns(17).delete()
            exHoja.Columns(16).delete()
            exHoja.Columns(15).delete()
            exHoja.Columns(14).delete()
            exHoja.Columns(13).delete()
            exHoja.Columns(12).delete()
        End If

        exHoja.Columns.AutoFit()

        'Dibuja rectangulos.=------------------------------
        If InicioDatos < UltimaLinea - 1 Then
            AA = Chr(65 + 2 - 1) & InicioDatos - 1
            BB = Chr(65 + 11 - 1) & UltimaLinea
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
            AA = Chr(65 + 13 - 1) & InicioDatos - 1
            BB = Chr(65 + 20 - 1) & UltimaLinea
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
            AA = Chr(65 + 22 - 1) & InicioDatos - 1
            BB = Chr(65 + 22 - 1) & UltimaLinea
            Rango = exHoja.Range(AA, BB)
            DibujaRango(Rango)
        End If
        '--------------------------------------------------
        exHoja.Rows.Item(5).Font.Bold = True
        'Muestra titulos.
        exHoja.Cells.Item(2, 1) = "Debitos/Créditos/Cobranzas Por Factura       " & Format(Date.Now, "dd/MM/yyyy")

        exHoja.Cells.Item(5, 2) = "DATOS FACTURAS"
        exHoja.Cells.Item(5, 8) = "CREDITOS/DEBITOS/COBRANZAS"
        exHoja.Cells.Item(5, 13) = "FACTURAS"
        exHoja.Cells.Item(5, 16) = "CREDITOS/DEBITOS/COBRANZAS"

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Public Sub InformeFacturasDeOrdenCompra(ByVal Orden As Decimal)

        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Add
        exHoja = exLibro.Worksheets(1)

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------
        exApp.StandardFont = "Courier New"

        Dim RowExxel As Integer

        RowExxel = 6
        exHoja.Cells.Item(RowExxel, 1) = " "
        exHoja.Cells.Item(RowExxel, 2) = "Nro.Factura"
        exHoja.Cells.Item(RowExxel, 3) = " Recibo Oficial "
        exHoja.Cells.Item(RowExxel, 4) = " Fecha "
        exHoja.Cells.Item(RowExxel, 5) = "Importe Orden Compra"
        exHoja.Cells.Item(RowExxel, 6) = "Importe Facturado"
        exHoja.Cells.Item(RowExxel, 7) = "Estado"

        exHoja.Rows.Item(RowExxel).Font.Bold = True
        exHoja.Rows.Item(RowExxel).HorizontalAlignment = 3

        RowExxel = RowExxel + 1
        Dim InicioDatos As Integer = RowExxel

        'Procesa facturas.
        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT 1 AS Operacion, C.* FROM ComproFacturados AS C WHERE C.OrdenCompra = " & Orden & ";", Conexion, Dt) Then End
        If PermisoTotal Then
            If Not Tablas.Read("SELECT 2 AS Operacion, C.* FROM ComproFacturados AS C WHERE C.OrdenCompra = " & Orden & ";", ConexionN, Dt) Then End
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Factura"
        Dim ImporteTotalOrden As Decimal
        Dim ImporteTotalConIva As Decimal

        For Each Row As DataRowView In View
            RowExxel = RowExxel + 1
            Dim NroInterno As Decimal
            Dim ReciboOficial As String
            Dim Fecha As Date
            Dim Estado As Integer
            Dim OperacionFactura As Integer

            If PermisoTotal Or (Not PermisoTotal And Row("Operacion")) = 1 Then
                HallaFacturaProveedor(Row("Operacion"), Row("Factura"), NroInterno, ReciboOficial, Fecha, Estado, OperacionFactura)
                If OperacionFactura = 1 Then
                    exHoja.Cells.Item(RowExxel, 1) = ""
                Else
                    exHoja.Cells.Item(RowExxel, 1) = "X"
                End If
                exHoja.Cells.Item(RowExxel, 2) = NroInterno
                exHoja.Cells.Item(RowExxel, 3) = ReciboOficial
                exHoja.Cells.Item(RowExxel, 4) = Format(Fecha, "MM/dd/yyyy")
                exHoja.Cells.Item(RowExxel, 5) = Row("Importe")
                exHoja.Cells.Item(RowExxel, 6) = Row("ImporteConIva")
                If Estado = 3 Then
                    exHoja.Cells.Item(RowExxel, 7) = "Baja"
                Else
                    ImporteTotalConIva = ImporteTotalConIva + Row("ImporteConIva")
                    ImporteTotalOrden = ImporteTotalOrden + Row("Importe")
                End If
            Else
                exHoja.Cells.Item(RowExxel, 3) = "Error 1000"
            End If
        Next
        RowExxel = RowExxel + 1
        exHoja.Cells.Item(RowExxel, 3) = "Total Gral.:"
        exHoja.Cells.Item(RowExxel, 5) = ImporteTotalOrden
        exHoja.Cells.Item(RowExxel, 6) = ImporteTotalConIva

        Dt.Dispose()
        View = Nothing

        Dim UltimaLinea As Integer = exLibro.ActiveSheet.UsedRange.Row - 1 + exLibro.ActiveSheet.UsedRange.Rows.Count

        Dim Rango As Microsoft.Office.Interop.Excel.Range
        'se resta 1 a la columna para que sumado a 65 de la letra de la columna.
        Dim AA As String
        Dim BB As String
        '
        AA = Chr(65 + 3 - 1) & InicioDatos
        BB = Chr(65 + 3 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 4 - 1) & InicioDatos
        BB = Chr(65 + 4 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        '
        AA = Chr(65 + 5 - 1) & InicioDatos
        BB = Chr(65 + 5 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 6 - 1) & InicioDatos
        BB = Chr(65 + 6 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
        Rango.NumberFormat = "#.##0,00"
        '
        AA = Chr(65 + 7 - 1) & InicioDatos
        BB = Chr(65 + 7 - 1) & UltimaLinea
        Rango = Nothing
        Rango = exHoja.Range(AA, BB)
        Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter

        exHoja.Columns.AutoFit()

        exHoja.Rows.Item(5).Font.Bold = True
        'Muestra titulos.
        exHoja.Cells.Item(2, 1) = "Facturas que afectan a la Orden de Compra: " & Orden & "     " & Format(Date.Now, "dd/MM/yyyy")

        exApp.Application.Visible = True

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

    End Sub
    Private Sub HallaFacturaProveedor(ByVal Operacion As Integer, ByVal Factura As Decimal, ByRef NroInterno As Decimal, ByRef ReciboOficial As String, ByRef Fecha As Date, ByRef Estado As Integer, ByRef OperacionFactura As Integer)

        Dim Dt As New DataTable

        'Si esta en B entonces la factura esta en B.
        Dim Sql As String = "SELECT ReciboOficial,Fecha,Estado FROM FacturasProveedorCabeza WHERE Factura = " & Factura & ";"
        If Operacion = 1 Then
            Tablas.Read(Sql, Conexion, Dt)
            ReciboOficial = NumeroEditado(Dt.Rows(0).Item("ReciboOficial"))
            Fecha = Dt.Rows(0).Item("Fecha")
            NroInterno = Factura
            Estado = Dt.Rows(0).Item("Estado")
            OperacionFactura = 1
            Dt.Dispose() : Exit Sub
        End If
        'Si Operacion = 2 (es N) entonces la factura puedeser 100% N o Mixta.
        Dt = New DataTable
        Sql = "SELECT Nrel,ReciboOficial,Fecha,Estado FROM FacturasProveedorCabeza WHERE Factura = " & Factura & ";"
        If Operacion = 2 Then
            Tablas.Read(Sql, ConexionN, Dt)
            ReciboOficial = NumeroEditado(Dt.Rows(0).Item("ReciboOficial"))
            Fecha = Dt.Rows(0).Item("Fecha")
            Estado = Dt.Rows(0).Item("Estado")
            If Dt.Rows(0).Item("Nrel") = 0 Then    'es 100% N.
                NroInterno = Factura
                OperacionFactura = 2
            Else
                NroInterno = Dt.Rows(0).Item("Nrel")
                OperacionFactura = 1
            End If
        End If
        Dt.Dispose()

    End Sub
    Private Sub HallaLiquidacionProveedor(ByVal Lote As Integer)

        Dim Importe As Decimal

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT 1 AS Operacion,C.NRef, D.* FROM LiquidacionCabeza AS C INNER JOIN LiquidacionDetalle AS D ON D.Liquidacion = C.Liquidacion WHERE C.Estado = 1 AND D.Lote = " & Lote & ";", Conexion, Dt) Then End
        If PermisoTotal Then
            If Not Tablas.Read("SELECT 2 AS Operacion,C.NRef, D.* FROM LiquidacionCabeza AS C INNER JOIN LiquidacionDetalle AS D ON D.Liquidacion = C.Liquidacion WHERE C.Estado = 1 AND D.Lote = " & Lote & ";", ConexionN, Dt) Then End
        End If
        For Each RowW As DataRow In Dt.Rows
            If RowW("Operacion") = 1 Then
                Importe = Importe
            End If
        Next

    End Sub

    Private Function HallaNombreTipoNota(ByVal Tipo As Integer) As String

        Select Case Tipo
            Case 1
                Return "Credito con Devolución"
            Case 5
                Return "Debito a Cliente"
            Case 13005
                Return "Debito Interno a Cliente"
            Case 7
                Return "Crédito a Cliente"
            Case 13007
                Return "Crédito Interno a Cliente"
            Case 50
                Return "Debito del Cliente"
            Case 70
                Return "Crédito del Cliente"
            Case 60
                Return "Cobranza"
        End Select

    End Function
    Private Sub ArmaArchivoEstadisticasMermarPorLotes(ByRef Dt As DataTable)

        Dt = New DataTable

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Secuencia)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Proveedor)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Articulo)

        Dim Ingresado As New DataColumn("Ingresado")
        Ingresado.DataType = System.Type.GetType("System.Decimal")
        Dt.Columns.Add(Ingresado)

        Dim Merma As New DataColumn("Merma")
        Merma.DataType = System.Type.GetType("System.Decimal")
        Dt.Columns.Add(Merma)

        Dim Facturado As New DataColumn("Facturado")
        Facturado.DataType = System.Type.GetType("System.Decimal")
        Dt.Columns.Add(Facturado)

    End Sub
    Private Sub AcumulaEnMerma(ByVal Dt As DataTable, ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Merma As Decimal, ByVal Facturado As Decimal, ByVal Proveedor As Integer, ByVal Especie As Integer, ByVal Operacion As Integer)

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim ProveedorW As Integer
        Dim KilosXUnidad As Decimal
        Dim Ingresado As Decimal
        Dim Articulo As Integer

        If Not HallaProveedorYKilosLote(Lote, Secuencia, Operacion, ProveedorW, KilosXUnidad, Ingresado, Articulo) Then
            MsgBox("Error al leer Archivo Lotes.") : End
        End If

        If Proveedor <> 0 And ProveedorW <> Proveedor Then Exit Sub

        Dim EspecieW As Integer
        Dim Variedad As Integer

        HallaEspecieYVariedad(Articulo, EspecieW, Variedad)

        If Especie <> 0 And EspecieW <> Especie Then Exit Sub

        Dim RowsBusqueda() As DataRow
        Dim Alta As Boolean

        If Dt.Rows.Count <> 0 Then
            RowsBusqueda = Dt.Select("Lote = " & Lote & " AND Secuencia = " & Secuencia & " AND Proveedor = " & ProveedorW)
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Merma") = RowsBusqueda(0).Item("Merma") + Merma * KilosXUnidad
                RowsBusqueda(0).Item("Facturado") = RowsBusqueda(0).Item("Facturado") + Facturado * KilosXUnidad
                Alta = False
            Else
                Alta = True
            End If
        Else
            Alta = True
        End If
        If Alta Then
            Dim Row As DataRow = Dt.NewRow
            Row("Lote") = Lote
            Row("Secuencia") = Secuencia
            Row("Proveedor") = ProveedorW
            Row("Articulo") = Articulo
            Row("Merma") = Merma * KilosXUnidad
            Row("Facturado") = Facturado * KilosXUnidad
            Row("Ingresado") = Ingresado * KilosXUnidad
            Dt.Rows.Add(Row)
        End If

    End Sub
    Private Function HallaProveedorYKilosLote(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As Integer, ByRef Proveedor As Integer, ByRef KilosXUnidad As Decimal, ByRef Ingresado As Decimal, ByRef Articulo As Integer) As Boolean

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim Dt As New DataTable
        '''        If Not Tablas.Read("SELECT Articulo,Proveedor,KilosXUnidad, Cantidad - Baja - BajaReproceso AS Ingresado FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", ConexionStr, Dt) Then Dt.Dispose() : Return False
        If Not Tablas.Read("SELECT Articulo,Proveedor,KilosXUnidad, Cantidad - Baja AS Ingresado FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", ConexionStr, Dt) Then Dt.Dispose() : Return False
        If Dt.Rows.Count = 0 Then Dt.Dispose() : Return False
        Proveedor = Dt.Rows(0).Item("Proveedor")
        Articulo = Dt.Rows(0).Item("Articulo")
        KilosXUnidad = Dt.Rows(0).Item("KilosXUnidad")
        Ingresado = Dt.Rows(0).Item("Ingresado")
        Dt.Dispose()

        Return True

    End Function
    Private Function HallaPedidoCliente(ByVal Pedido As Integer) As String

        Dim Dt As New DataTable
        Dim PedidoCliente As String
        If Not Tablas.Read("SELECT PedidoCliente FROM PedidosCabeza WHERE Pedido = " & Pedido & ";", Conexion, Dt) Then Return ""
        If Dt.Rows.Count <> 0 Then
            PedidoCliente = Dt.Rows(0).Item("PedidoCliente")
        End If

        Dt.Dispose()
        Return PedidoCliente

    End Function
    Public Function HallaNombreMedioPago(ByVal Clave As Integer) As String

        If Clave = 1 Then Return "Pesos"
        If Clave = 2 Then Return "ChequePropio"
        If Clave = 3 Then Return "ChequeTercero"
        If Clave = 4 Then Return "InterBanking"
        If Clave = 9 Then Return "Debito Auto."
        If Clave = 14 Then Return "Debito Auto. Dife."
        If Clave = 11 Then Return "Transferencia"
        If Clave = 6 Then Return "Vale Terceros"

        'Moneda extranjera o Retenciones.
        Dim Dt As New DataTable
        Dim Nombre As String
        If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas Where Clave = " & Clave & ";", Conexion, Dt) Then Return ""
        If Dt.Rows.Count <> 0 Then
            Nombre = Dt.Rows(0).Item("Nombre") : Dt.Dispose() : Return Nombre
        End If

        Return ""

    End Function
    Private Function HallaNotasDeFacturas(ByVal Factura As Decimal, ByVal ConexionStr As String, ByRef TotalCreditoDevoluion As Decimal, ByRef TotalDebito As Decimal, ByRef TotalCredito As Decimal, ByRef Dt As DataTable) As Boolean
        'Halla las imputaciones en notas de credito por devolucion(4), notas credito financieras(7,13007), notas dedebitos financieras(5,13005), cobranzas(60),
        ' Notas de debito del cliente(50), Notas de creditos del cliente(70)
        Dt = New DataTable
        TotalCreditoDevoluion = 0
        TotalDebito = 0
        TotalCredito = 0

        Dim SqlNotas As String = "SELECT 1 AS TipoNota,C.NotaCredito AS Nota,D.Importe AS ImporteT,C.Fecha FROM NotasCreditoCabeza AS C INNER JOIN RecibosDetalle AS D ON C.NotaCredito = D.Nota AND D.TipoNota = 4 WHERE TipoComprobante = 2 AND Comprobante = XXX" & _
           " UNION ALL " & _
           "SELECT C.TipoNota,C.Nota,D.Importe AS ImporteT,C.FechaContable as Fecha FROM RecibosCabeza as C INNER JOIN RecibosDetalle AS D ON C.TipoNota = D.TipoNota AND C.Nota = D.Nota Where C.Estado = 1 AND (C.TipoNota = 50 or C.TipoNota = 70) AND D.TipoComprobante = 2 AND D.Comprobante = XXX " & _
           " UNION ALL " & _
           "SELECT C.TipoNota,C.Nota,D.Importe AS ImporteT,C.Fecha FROM RecibosCabeza as C INNER JOIN RecibosDetalle AS D ON C.TipoNota = D.TipoNota AND C.Nota = D.Nota Where C.Estado = 1 AND (C.TipoNota = 60 or C.TipoNota = 5 or C.TipoNota = 13005 or C.TipoNota = 7 or C.TipoNota = 13007) AND D.TipoComprobante = 2 AND D.Comprobante = XXX" & ";"

        Dim SqlNotasW = Strings.Replace(SqlNotas, "XXX", Factura)

        If Not Tablas.Read(SqlNotasW, ConexionStr, Dt) Then Exit Function
        For Each Row As DataRow In Dt.Rows
            Select Case Row.Item("TipoNota")
                Case 1
                    TotalCreditoDevoluion = TotalCreditoDevoluion + CDec(Row("ImporteT"))
                Case 7, 13007
                    TotalCredito = TotalCredito + CDec(Row("ImporteT"))
                Case 5, 13005
                    TotalDebito = TotalDebito + CDec(Row("ImporteT"))
            End Select
        Next

        Return True

    End Function
    Private Function TieneCierreFactura(ByVal Comprobante As Decimal) As Boolean

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Nota FROM CierreFacturasCabeza WHERE Factura = " & Comprobante & " AND Estado = 1;", ConexionN, Dt) Then
            MsgBox("Error al leer Tabla: CierreFacturasCabeza.") : Return False
        End If
        If Dt.Rows.Count <> 0 Then
            TieneCierreFactura = True
        End If

        Dt.Dispose()

    End Function
End Module
