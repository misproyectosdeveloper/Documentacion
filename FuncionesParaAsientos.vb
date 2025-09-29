Module FuncionesParaAsientos
    Private Const FacturasOtrosProveedores As Integer = 5000
    Private Const OrdenPagoOtrosProveedores As Integer = 5010
    Private Const DevolucionOtrosProveedores As Integer = 5020
    Dim PTipoDocumento As Integer
    Public Function Asiento(ByVal TipoDocumento As Integer, ByVal Documento As Double, ByRef ListaConceptos As List(Of ItemListaConceptosAsientos), ByRef DtAsientoCabeza As DataTable, _
                            ByRef DtAsientoDetalle As DataTable, ByRef ListaCuentas As List(Of ItemCuentasAsientos), ByRef ListaLotes As List(Of ItemLotesParaAsientos), ByRef ListaIVA As List(Of ItemListaConceptosAsientos), ByRef ListaRetenciones As List(Of ItemListaConceptosAsientos), ByVal FechaContable As DateTime, ByVal TipoComprobante As Integer) As Boolean

        Dim Dt As New DataTable
        Dim Row1 As DataRow
        Dim Importe As Decimal = 0
        Dim ImporteDebe As Decimal = 0
        Dim ImporteHaber As Decimal = 0
        Dim DtDetalleAux As DataTable = DtAsientoDetalle.Copy

        PTipoDocumento = TipoDocumento

        If Not Tablas.Read("SELECT Concepto,Debe,Haber,Tabla,ClaveCuenta FROM SeteoDocumentos WHERE TipoDocumento = " & TipoDocumento & ";", Conexion, Dt) Then Return False
        For Each Row As DataRow In Dt.Rows
            If Row("Concepto") = 205 Then
                GeneraCuentasIVACompra(DtDetalleAux, ListaIVA, Row("Debe"), Row("Haber"), Row("ClaveCuenta"), Row("Concepto"))
            End If
            If Row("Concepto") = 206 Then
                GeneraCuentasIVAVenta(DtDetalleAux, ListaIVA, Row("Debe"), Row("Haber"), Row("ClaveCuenta"), Row("Concepto"))
            End If
            If Row("Concepto") = 209 Then
                If TipoDocumento = FacturasOtrosProveedores Then
                    GeneraCuentasRetencionesParaFacturasOtrosProveedores(DtDetalleAux, ListaRetenciones, Row("Debe"), Row("Haber"), Row("ClaveCuenta"), Row("Concepto"))
                Else
                    GeneraCuentasRetenciones(DtDetalleAux, ListaRetenciones, Row("Debe"), Row("Haber"), Row("ClaveCuenta"), Row("Concepto"))
                End If
            End If
            If Row("Tabla") = 10 Then
                GeneraCuentasInsumos(DtDetalleAux, ListaRetenciones, Row("Debe"), Row("Haber"), Row("ClaveCuenta"), Row("Concepto"))
            End If
            If Row("Tabla") = 11 Then
                GeneraCuentasFacturaServicios(DtDetalleAux, ListaCuentas, Row("Debe"), Row("Haber"), Row("ClaveCuenta"), Row("Concepto"))
            End If
            If Row("Tabla") = 13 Then
                GeneraCuentasFacturaSecos(DtDetalleAux, ListaRetenciones, Row("Debe"), Row("Haber"), Row("ClaveCuenta"), Row("Concepto"))
            End If
            If Row("Tabla") = 14 Then
                If TipoDocumento = FacturasOtrosProveedores Then
                    GeneraCuentasOtrasFacturas(DtDetalleAux, ListaRetenciones, Row("Debe"), Row("Haber"), Row("ClaveCuenta"), Row("Concepto"))
                End If
                If TipoDocumento = OrdenPagoOtrosProveedores Or TipoDocumento = DevolucionOtrosProveedores Then
                    GeneraCuentasOrdenPagoOtrasFacturas(DtDetalleAux, ListaRetenciones, Row("Debe"), Row("Haber"), Row("ClaveCuenta"), Row("Concepto"))
                End If
            End If
            If Row("Tabla") = 1 Then
                GeneraCuentasTabla1(DtDetalleAux, ListaConceptos, ListaCuentas, Row("Concepto"), Row("Debe"), Row("Haber"))
            End If
            If EsPorTipoOperacion(TipoDocumento, Row("Concepto")) Then
                If Not ArmaAsientoTabla2(Row("Concepto"), Row("ClaveCuenta"), Row("Debe"), Row("Haber"), DtDetalleAux, ListaLotes, Row("Concepto")) Then Return False
            End If
            If Not EsPorTipoOperacion(TipoDocumento, Row("Concepto")) And Row("Concepto") <> 205 And Row("Concepto") <> 206 And Row("Concepto") <> 209 And Row("Tabla") <> 1 And Row("Tabla") <> 2 And Row("Tabla") <> 7 And Row("Tabla") <> 10 And Row("Tabla") <> 11 And Row("Tabla") <> 13 And Row("Tabla") <> 14 Then
                HallaImporteConcepto(ListaConceptos, Row("Concepto"), Importe)
                ImporteDebe = 0
                ImporteHaber = 0
                If Row("Debe") Then
                    If Importe < 0 Then
                        ImporteHaber = Importe * -1
                    Else : ImporteDebe = Importe
                    End If
                End If
                If Row("Haber") Then
                    If Importe < 0 Then
                        ImporteDebe = Importe * -1
                    Else : ImporteHaber = Importe
                    End If
                End If
                '       If Importe <> 0 Or Importe = 0 Then
                If Importe <> 0 Then
                    Row1 = DtDetalleAux.NewRow
                    Row1("Asiento") = 0
                    Row1("Concepto") = Row("Concepto")
                    Select Case Row("Tabla")
                        Case 0
                            Row1("Cuenta") = Row("ClaveCuenta")
                        Case 3
                            Row1("Cuenta") = HallaCuentaTabla3(Row("Concepto"))
                            If Row1("Cuenta") <= 0 Then
                                MsgBox("Error, NO Existe Cuenta del Medio de Pago. Operación se CANCELA.")
                                Return False
                            End If
                        Case 6
                            Row1("Cuenta") = HallaCuentaTabla6(Row("Concepto"))
                            If Row1("Cuenta") <= 0 Then
                                MsgBox("Error, NO Existe Cuenta Concepto Otro pago. Operación se CANCELA.")
                                Return False
                            End If
                        Case 8
                            Row1("Cuenta") = HallaCuentaTabla8(Row("Concepto"))
                            If Row1("Cuenta") <= 0 Then
                                MsgBox("Error, NO Existe Cuenta Concepto Otro pago. Operación se CANCELA.")
                                Return False
                            End If
                        Case 9
                            Row1("Cuenta") = HallaCuentaTabla9(Row("Concepto"))
                            If Row1("Cuenta") <= 0 Then
                                MsgBox("Error, NO Existe Cuenta Concepto Otro pago. Operación se CANCELA.")
                                Return False
                            End If
                        Case 12
                            Row1("Cuenta") = HallaCuentaTabla12(ListaConceptos(0).Legajo)
                            If Row1("Cuenta") <= 0 Then
                                MsgBox("Error, NO Existe Cuenta Concepto Recibo Sueldo. Operación se CANCELA.")
                                Return False
                            End If
                        Case Else
                            MsgBox("Falta tabla")
                            Return False
                    End Select
                    Row1("Debe") = ImporteDebe
                    Row1("Haber") = ImporteHaber
                    DtDetalleAux.Rows.Add(Row1)
                End If
            End If
        Next

        DtAsientoDetalle.Clear()
        Dim RowsBusqueda() As DataRow
        Dim Row2 As DataRow
        For Each Row As DataRow In DtDetalleAux.Rows
            RowsBusqueda = DtAsientoDetalle.Select("Cuenta = " & Row("Cuenta"))
            If RowsBusqueda.Length = 0 Then
                Row2 = DtAsientoDetalle.NewRow
                Row2("Asiento") = Row("Asiento")
                Row2("Concepto") = Row("Concepto")
                Row2("Cuenta") = Row("Cuenta")
                Row2("Debe") = Row("Debe")
                Row2("Haber") = Row("Haber")
                DtAsientoDetalle.Rows.Add(Row2)
            Else
                RowsBusqueda(0).Item("Debe") = CDec(RowsBusqueda(0).Item("Debe")) + CDec(Row("Debe"))
                RowsBusqueda(0).Item("Haber") = CDec(RowsBusqueda(0).Item("Haber")) + CDec(Row("Haber"))
            End If
        Next

        Dim TotalDebe As Decimal = 0
        Dim TotalHaber As Decimal = 0
        For Each Row As DataRow In DtAsientoDetalle.Rows
            TotalDebe = Trunca(TotalDebe + Row("Debe"))
            TotalHaber = Trunca(TotalHaber + Row("Haber"))
        Next
        '
        Dim Diferencia As Decimal = TotalDebe - TotalHaber
        If Diferencia > -1 And Diferencia < 1 Then
            If Diferencia < 0 Then
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    If Row("Debe") <> 0 Then Row("Debe") = CDec(Row("Debe")) - Diferencia : Exit For
                Next
            Else
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    If Row("Haber") <> 0 Then Row("Haber") = CDec(Row("Haber")) + Diferencia : Exit For
                Next
            End If
        End If
        '
        TotalDebe = 0
        TotalHaber = 0
        For Each Row As DataRow In DtAsientoDetalle.Rows
            TotalDebe = Trunca(TotalDebe + Row("Debe"))
            TotalHaber = Trunca(TotalHaber + Row("Haber"))
        Next
        If TotalDebe <> TotalHaber Then
            MsgBox("Asiento Contable no Balancea. Operación se CANCELA.")
            Return False
        End If

        If DtAsientoDetalle.Rows.Count <> 0 Then
            Row1 = DtAsientoCabeza.NewRow
            Row1("Asiento") = 0
            Row1("TipoDocumento") = TipoDocumento
            Row1("Documento") = Documento
            Row1("Intfecha") = FechaContable.Year & Format(FechaContable.Month, "00") & Format(FechaContable.Day, "00")
            Row1("Costeo") = 0
            Row1("Comentario") = ""
            Row1("Estado") = 1
            Row1("TipoComprobante") = TipoComprobante
            Row1("Debito") = False
            Row1("Credito") = False
            DtAsientoCabeza.Rows.Add(Row1)
        Else
            If GCancelaSinAsiento Then
                MsgBox("No se pudo Generar Asiento. Operación se CANCELA.")
                Return False
            End If
        End If

        For Each Row As DataRow In DtAsientoDetalle.Rows
            If Not CuentaOk(Row("Cuenta")) Then
                MsgBox("Cuenta-Subcuenta " & Format(Row("Cuenta"), "000-000000-00") & " No Pertenece al Centro de Costo.")
                If GCancelaSinAsiento Then
                    MsgBox("No se pudo Generar Asiento. Operación se CANCELA.")
                    Return False
                End If
            End If
        Next

        Return True

    End Function
    Public Function AgregaAlAsiento(ByVal TipoDocumento As Integer, ByVal Documento As Double, ByRef ListaConceptos As List(Of ItemListaConceptosAsientos), ByRef DtAsientoCabeza As DataTable, _
                           ByRef DtAsientoDetalle As DataTable, ByRef ListaCuentas As List(Of ItemCuentasAsientos), ByRef ListaLotes As List(Of ItemLotesParaAsientos), ByRef ListaIVA As List(Of ItemListaConceptosAsientos), ByRef ListaRetenciones As List(Of ItemListaConceptosAsientos), ByVal FechaContable As DateTime, ByVal ConexionStr As String) As Boolean

        If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & TipoDocumento & " AND Documento = " & Documento & ";", ConexionStr, DtAsientoCabeza) Then Return False
        Dim NroAsiento As Integer = DtAsientoCabeza.Rows(0).Item("Asiento")
        If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & NroAsiento & ";", ConexionStr, DtAsientoDetalle) Then Return False

        Dim DtAsientoCabezaAux As DataTable = DtAsientoCabeza.Clone
        Dim DtAsientoDetalleAux As DataTable = DtAsientoDetalle.Clone

        If Not Asiento(TipoDocumento, 0, ListaConceptos, DtAsientoCabezaAux, DtAsientoDetalleAux, ListaCuentas, ListaLotes, ListaIVA, ListaRetenciones, FechaContable, 0) Then Return False

        For Each Row As DataRow In DtAsientoDetalleAux.Rows
            Row("Asiento") = NroAsiento
            Dim Row1 As DataRow = DtAsientoDetalle.NewRow
            For I As Integer = 0 To DtAsientoDetalleAux.Columns.Count - 1
                Row1.Item(I) = Row.Item(I)
            Next
            DtAsientoDetalle.Rows.Add(Row1)
        Next

        DtAsientoCabezaAux.Dispose()
        DtAsientoDetalleAux.Dispose()

        Return True

    End Function
    Public Function EsPorTipoOperacion(ByVal TipoDocumento As Integer, ByVal Concepto As Integer) As Boolean

        'Correccion por que existe el concepto 600,601,602 y 603 generado por el sistema que puede ser un concepto generado en tabla por el usuario(Error de programacion).

        Select Case TipoDocumento
            Case 910, 911, 912, 800
                Select Case Concepto
                    Case 600, 601, 602, 603
                        Return True
                End Select
            Case 61000
                Select Case Concepto
                    Case -100, -101, -102, -103
                        Return True
                End Select
            Case 61001
                Select Case Concepto
                    Case -200, -201, -202, -203
                        Return True
                End Select
        End Select

        Select Case Concepto
            '            Case 300, 301, 302, 303, 400, 401, 402, 403, 600, 601, 602, 603, -100, -101, -102, -103, -105, -106, -107, -108, -110, -111, -112, -113
            Case 300, 301, 302, 303, 400, 401, 402, 403, 410, 411, 412, 413, 420, 421, 422, 423, -100, -101, -102, -103, -105, -106, -107, -108, -110, -111, -112, -113
                Return True
            Case Else
                Return False
        End Select

    End Function
    Private Function CuentaOk(ByVal ClaveCuenta As Double) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cuenta FROM PlanDeCuentas WHERE ClaveCuenta = " & ClaveCuenta & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Plan de Cuentas.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Sub HallaImporteConcepto(ByVal ListaConceptos As List(Of ItemListaConceptosAsientos), ByVal Concepto As Integer, ByRef Importe As Decimal)

        Importe = 0

        For Each Fila As ItemListaConceptosAsientos In ListaConceptos
            If Fila.Clave = Concepto Then
                Importe = Importe + Fila.Importe
            End If
        Next

    End Sub
    Private Function GeneraCuentasIVACompra(ByRef DtAsiento As DataTable, ByVal ListaIVA As List(Of ItemListaConceptosAsientos), ByVal Debe As Boolean, ByVal Haber As Boolean, ByVal Cuenta As Double, ByVal Concepto As Integer) As Boolean

        For Each Fila As ItemListaConceptosAsientos In ListaIVA
            If Fila.TipoIva = 5 Then
                Dim Row As DataRow = DtAsiento.NewRow
                Row("Asiento") = 0
                Row("Concepto") = Concepto
                If Cuenta <> 0 Then
                    Row("Cuenta") = Cuenta
                Else
                    Row("Cuenta") = HallaCuentaTabla5(Fila.Clave)
                End If
                If Row("Cuenta") <= 0 Then
                    MsgBox("Error, NO Existe Cuenta IVA Compra. Operación se CANCELA.")
                    Return False
                End If
                Dim ImporteDebe As Decimal = 0
                Dim ImporteHaber As Decimal = 0
                If Debe Then
                    If Fila.Importe < 0 Then
                        ImporteHaber = Fila.Importe * -1
                    Else : ImporteDebe = Fila.Importe
                    End If
                End If
                If Haber Then
                    If Fila.Importe < 0 Then
                        ImporteDebe = Fila.Importe * -1
                    Else : ImporteHaber = Fila.Importe
                    End If
                End If
                Row("Debe") = ImporteDebe
                Row("Haber") = ImporteHaber
                DtAsiento.Rows.Add(Row)
            End If
        Next

        Return True

    End Function
    Private Function GeneraCuentasIVAVenta(ByRef DtAsiento As DataTable, ByVal ListaIVA As List(Of ItemListaConceptosAsientos), ByVal Debe As Boolean, ByVal Haber As Boolean, ByVal Cuenta As Double, ByVal Concepto As Integer) As Boolean

        For Each Fila As ItemListaConceptosAsientos In ListaIVA
            If Fila.TipoIva = 6 Then
                Dim Row As DataRow = DtAsiento.NewRow
                Row("Asiento") = 0
                Row("Concepto") = Concepto
                If Cuenta <> 0 Then
                    Row("Cuenta") = Cuenta
                Else
                    Row("Cuenta") = HallaCuentaTabla5_6(Fila.Clave)
                End If
                If Row("Cuenta") <= 0 Then
                    MsgBox("Error, NO Existe Cuenta IVA Venta. Operación se CANCELA.")
                    Return False
                End If
                Dim ImporteDebe As Decimal = 0
                Dim ImporteHaber As Decimal = 0
                If Debe Then
                    If Fila.Importe < 0 Then
                        ImporteHaber = Fila.Importe * -1
                    Else : ImporteDebe = Fila.Importe
                    End If
                End If
                If Haber Then
                    If Fila.Importe < 0 Then
                        ImporteDebe = Fila.Importe * -1
                    Else : ImporteHaber = Fila.Importe
                    End If
                End If
                Row("Debe") = ImporteDebe
                Row("Haber") = ImporteHaber
                DtAsiento.Rows.Add(Row)
            End If
        Next

        Return True

    End Function
    Private Function GeneraCuentasRetenciones(ByRef DtAsiento As DataTable, ByVal ListaRetenciones As List(Of ItemListaConceptosAsientos), ByVal Debe As Boolean, ByVal Haber As Boolean, ByVal Cuenta As Double, ByVal Concepto As Integer) As Boolean

        For Each Fila As ItemListaConceptosAsientos In ListaRetenciones
            Dim Row As DataRow = DtAsiento.NewRow
            Row("Asiento") = 0
            Row("Concepto") = Concepto
            If Cuenta <> 0 Then
                Row("Cuenta") = Cuenta
            Else
                Row("Cuenta") = HallaCuentaTablaRetencion(Fila.Clave, Fila.TipoIva)
            End If
            If Row("Cuenta") <= 0 Then
                MsgBox("Error, NO Existe Cuenta Retención. Operación se CANCELA.")
                Return False
            End If
            Dim ImporteDebe As Double = 0
            Dim ImporteHaber As Double = 0
            If Debe Then
                If Fila.Importe < 0 Then
                    ImporteHaber = Fila.Importe * -1
                Else : ImporteDebe = Fila.Importe
                End If
            End If
            If Haber Then
                If Fila.Importe < 0 Then
                    ImporteDebe = Fila.Importe * -1
                Else : ImporteHaber = Fila.Importe
                End If
            End If
            Row("Debe") = ImporteDebe
            Row("Haber") = ImporteHaber
            DtAsiento.Rows.Add(Row)
        Next

        Return True

    End Function
    Private Function GeneraCuentasRetencionesParaFacturasOtrosProveedores(ByRef DtAsiento As DataTable, ByVal ListaRetenciones As List(Of ItemListaConceptosAsientos), ByVal Debe As Boolean, ByVal Haber As Boolean, ByVal Cuenta As Double, ByVal Concepto As Integer) As Boolean

        For Each Fila As ItemListaConceptosAsientos In ListaRetenciones
            If Fila.TipoIva <> -1 Then
                Dim Row As DataRow = DtAsiento.NewRow
                Row("Asiento") = 0
                Row("Concepto") = Concepto
                If Cuenta <> 0 Then
                    Row("Cuenta") = Cuenta
                Else
                    Row("Cuenta") = HallaCuentaTablaRetencion(Fila.Clave, Fila.TipoIva)
                End If
                If Row("Cuenta") <= 0 Then
                    MsgBox("Error, NO Existe Cuenta Retención. Operación se CANCELA.")
                    Return False
                End If
                Dim ImporteDebe As Double = 0
                Dim ImporteHaber As Double = 0
                If Debe Then
                    If Fila.Importe < 0 Then
                        ImporteHaber = Fila.Importe * -1
                    Else : ImporteDebe = Fila.Importe
                    End If
                End If
                If Haber Then
                    If Fila.Importe < 0 Then
                        ImporteDebe = Fila.Importe * -1
                    Else : ImporteHaber = Fila.Importe
                    End If
                End If
                Row("Debe") = ImporteDebe
                Row("Haber") = ImporteHaber
                DtAsiento.Rows.Add(Row)
            End If
        Next

        Return True

    End Function
    Private Function GeneraCuentasInsumos(ByRef DtAsiento As DataTable, ByVal ListaRetenciones As List(Of ItemListaConceptosAsientos), ByVal Debe As Boolean, ByVal Haber As Boolean, ByVal Cuenta As Double, ByVal Concepto As Integer) As Boolean

        For Each Fila As ItemListaConceptosAsientos In ListaRetenciones
            Dim Row As DataRow = DtAsiento.NewRow
            Row("Asiento") = 0
            Row("Concepto") = Concepto
            If Cuenta <> 0 Then
                Row("Cuenta") = Cuenta
            Else
                Row("Cuenta") = HallaCuentaTablaInsumos(Fila.Clave)
            End If
            If Row("Cuenta") <= 0 Then
                MsgBox("Error, NO Existe Cuenta Insumo. Operación se CANCELA.")
                Return False
            End If
            Dim ImporteDebe As Double = 0
            Dim ImporteHaber As Double = 0
            If Debe Then
                If Fila.Importe < 0 Then
                    ImporteHaber = Fila.Importe * -1
                Else : ImporteDebe = Fila.Importe
                End If
            End If
            If Haber Then
                If Fila.Importe < 0 Then
                    ImporteDebe = Fila.Importe * -1
                Else : ImporteHaber = Fila.Importe
                End If
            End If
            Row("Debe") = ImporteDebe
            Row("Haber") = ImporteHaber
            DtAsiento.Rows.Add(Row)
        Next

        Return True

    End Function
    Private Function GeneraCuentasFacturaServicios(ByRef DtAsiento As DataTable, ByVal ListaCuentas As List(Of ItemCuentasAsientos), ByVal Debe As Boolean, ByVal Haber As Boolean, ByVal Cuenta As Double, ByVal Concepto As Integer) As Boolean

        For Each Fila As ItemCuentasAsientos In ListaCuentas
            Dim Row As DataRow = DtAsiento.NewRow
            Row("Asiento") = 0
            Row("Concepto") = Concepto
            If Cuenta <> 0 Then
                Row("Cuenta") = Cuenta
            Else
                Row("Cuenta") = HallaCuentaTablaServicios(Fila.Clave)
            End If
            If Row("Cuenta") <= 0 Then
                MsgBox("Error, NO Existe Cuenta Insumo. Operación se CANCELA.")
                Return False
            End If
            Dim ImporteDebe As Double = 0
            Dim ImporteHaber As Double = 0
            If Debe Then
                If Fila.Importe < 0 Then
                    ImporteHaber = Fila.Importe * -1
                Else : ImporteDebe = Fila.Importe
                End If
            End If
            If Haber Then
                If Fila.Importe < 0 Then
                    ImporteDebe = Fila.Importe * -1
                Else : ImporteHaber = Fila.Importe
                End If
            End If
            Row("Debe") = ImporteDebe
            Row("Haber") = ImporteHaber
            DtAsiento.Rows.Add(Row)
        Next

        Return True

    End Function
    Private Function GeneraCuentasFacturaSecos(ByRef DtAsiento As DataTable, ByVal ListaRetenciones As List(Of ItemListaConceptosAsientos), ByVal Debe As Boolean, ByVal Haber As Boolean, ByVal Cuenta As Double, ByVal Concepto As Integer) As Boolean

        For Each Fila As ItemListaConceptosAsientos In ListaRetenciones
            Dim Row As DataRow = DtAsiento.NewRow
            Row("Asiento") = 0
            Row("Concepto") = Concepto
            If Cuenta <> 0 Then
                Row("Cuenta") = Cuenta
            Else
                Row("Cuenta") = HallaCuentaTablaSecos(Fila.Clave)
            End If
            If Row("Cuenta") <= 0 Then
                MsgBox("Error, NO Existe Cuenta Insumo. Operación se CANCELA.")
                Return False
            End If
            Dim ImporteDebe As Double = 0
            Dim ImporteHaber As Double = 0
            If Debe Then
                If Fila.Importe < 0 Then
                    ImporteHaber = Fila.Importe * -1
                Else : ImporteDebe = Fila.Importe
                End If
            End If
            If Haber Then
                If Fila.Importe < 0 Then
                    ImporteDebe = Fila.Importe * -1
                Else : ImporteHaber = Fila.Importe
                End If
            End If
            Row("Debe") = ImporteDebe
            Row("Haber") = ImporteHaber
            DtAsiento.Rows.Add(Row)
        Next

        Return True

    End Function
    Private Function GeneraCuentasOtrasFacturas(ByRef DtAsiento As DataTable, ByVal ListaRetenciones As List(Of ItemListaConceptosAsientos), ByVal Debe As Boolean, ByVal Haber As Boolean, ByVal Cuenta As Double, ByVal Concepto As Integer) As Boolean

        For Each Fila As ItemListaConceptosAsientos In ListaRetenciones
            If Fila.TipoIva = -1 Then
                Dim Row As DataRow = DtAsiento.NewRow
                Row("Asiento") = 0
                Row("Concepto") = Concepto
                If Cuenta <> 0 Then
                    Row("Cuenta") = Cuenta
                Else
                    Row("Cuenta") = HallaCuentaTabla14(Fila.Clave)
                End If
                If Row("Cuenta") <= 0 Then
                    MsgBox("Error, NO Existe Cuenta Otras Facturas. Operación se CANCELA.")
                    Return False
                End If
                Dim ImporteDebe As Double = 0
                Dim ImporteHaber As Double = 0
                If Debe Then
                    If Fila.Importe < 0 Then
                        ImporteHaber = Fila.Importe * -1
                    Else : ImporteDebe = Fila.Importe
                    End If
                End If
                If Haber Then
                    If Fila.Importe < 0 Then
                        ImporteDebe = Fila.Importe * -1
                    Else : ImporteHaber = Fila.Importe
                    End If
                End If
                Row("Debe") = ImporteDebe
                Row("Haber") = ImporteHaber
                DtAsiento.Rows.Add(Row)
            End If
        Next

        Return True

    End Function
    Private Function GeneraCuentasOrdenPagoOtrasFacturas(ByRef DtAsiento As DataTable, ByVal ListaRetenciones As List(Of ItemListaConceptosAsientos), ByVal Debe As Boolean, ByVal Haber As Boolean, ByVal Cuenta As Double, ByVal Concepto As Integer) As Boolean

        For Each Fila As ItemListaConceptosAsientos In ListaRetenciones
            Dim Row As DataRow = DtAsiento.NewRow
            Row("Asiento") = 0
            Row("Concepto") = Concepto
            If Cuenta <> 0 Then
                Row("Cuenta") = Cuenta
            Else
                Row("Cuenta") = HallaCuentaTabla14(Fila.Clave)
            End If
            If Row("Cuenta") <= 0 Then
                MsgBox("Error, NO Existe Cuenta Otras Facturas. Operación se CANCELA.")
                Return False
            End If
            Dim ImporteDebe As Double = 0
            Dim ImporteHaber As Double = 0
            If Debe Then
                If Fila.Importe < 0 Then
                    ImporteHaber = Fila.Importe * -1
                Else : ImporteDebe = Fila.Importe
                End If
            End If
            If Haber Then
                If Fila.Importe < 0 Then
                    ImporteDebe = Fila.Importe * -1
                Else : ImporteHaber = Fila.Importe
                End If
            End If
            Row("Debe") = ImporteDebe
            Row("Haber") = ImporteHaber
            DtAsiento.Rows.Add(Row)
        Next

        Return True

    End Function
    Private Function GeneraCuentasTabla1(ByRef DtAsiento As DataTable, ByVal ListaConceptos As List(Of ItemListaConceptosAsientos), ByRef ListaCuentas As List(Of ItemCuentasAsientos), ByVal Concepto As Integer, ByVal Debe As Boolean, ByVal Haber As Boolean) As Boolean

        Dim Row1 As DataRow
        Dim RowsBusqueda() As DataRow

        For Each Fila As ItemCuentasAsientos In ListaCuentas
            If Fila.Clave = Concepto Then
                Row1 = DtAsiento.NewRow
                Row1("Asiento") = 0
                Row1("Concepto") = Concepto
                Row1("Cuenta") = Fila.Cuenta
                Dim ImporteDebe As Decimal = 0
                Dim ImporteHaber As Decimal = 0
                If Debe Then
                    If Fila.Importe < 0 Then
                        ImporteHaber = Fila.Importe * -1
                    Else : ImporteDebe = Fila.Importe
                    End If
                End If
                If Haber Then
                    If Fila.Importe < 0 Then
                        ImporteDebe = Fila.Importe * -1
                    Else : ImporteHaber = Fila.Importe
                    End If
                End If
                Row1("Debe") = ImporteDebe
                Row1("Haber") = ImporteHaber
                If Row1("Debe") <> 0 Or Row1("Haber") <> 0 Then
                    RowsBusqueda = DtAsiento.Select("Cuenta = " & Row1("Cuenta"))
                    If RowsBusqueda.Length <> 0 Then
                        RowsBusqueda(0).Item("Debe") = CDec(RowsBusqueda(0).Item("Debe")) + Row1("Debe")
                        RowsBusqueda(0).Item("Haber") = CDec(RowsBusqueda(0).Item("Haber")) + Row1("Haber")
                    Else
                        DtAsiento.Rows.Add(Row1)
                    End If
                End If
            End If
        Next

        Return True

    End Function
    Private Function ArmaAsientoTabla2(ByVal Clave As Integer, ByVal Cuenta As Double, ByVal Debe As Boolean, ByVal Haber As Boolean, ByRef DtAsientoDetalle As DataTable, ByRef Lista As List(Of ItemLotesParaAsientos), ByVal Concepto As Integer) As Boolean

        Dim Row1 As DataRow
        Dim RowsBusqueda() As DataRow

        For Each Item As ItemLotesParaAsientos In Lista
            If Item.Clave = Clave Then
                Row1 = DtAsientoDetalle.NewRow
                Row1("Asiento") = 0
                Row1("Concepto") = Concepto
                If Cuenta.ToString.Length > 8 Then
                    Row1("Cuenta") = Cuenta
                Else
                    Row1("Cuenta") = Item.Centro & Format(Cuenta, "00000000")
                End If
                Dim ImporteDebe As Decimal = 0
                Dim ImporteHaber As Decimal = 0
                If Debe Then
                    If Item.MontoNeto < 0 Then
                        ImporteHaber = Item.MontoNeto * -1
                    Else : ImporteDebe = Item.MontoNeto
                    End If
                End If
                If Haber Then
                    If Item.MontoNeto < 0 Then
                        ImporteDebe = Item.MontoNeto * -1
                    Else : ImporteHaber = Item.MontoNeto
                    End If
                End If
                Row1("Debe") = ImporteDebe
                Row1("Haber") = ImporteHaber
                If Row1("Debe") <> 0 Or Row1("Haber") <> 0 Then
                    RowsBusqueda = DtAsientoDetalle.Select("Cuenta = " & Row1("Cuenta"))
                    If RowsBusqueda.Length <> 0 Then
                        RowsBusqueda(0).Item("Debe") = CDec(RowsBusqueda(0).Item("Debe")) + Row1("Debe")
                        RowsBusqueda(0).Item("Haber") = CDec(RowsBusqueda(0).Item("Haber")) + Row1("Haber")
                    Else
                        DtAsientoDetalle.Rows.Add(Row1)
                    End If
                End If
            End If
        Next

        Return True

    End Function
    Private Function HallaCuentaTabla3(ByVal MedioPago As Integer) As Double

        Dim cuenta As Double

        If MedioPago > 2000000 And (PTipoDocumento = 16000 Or PTipoDocumento = 16001) Then  'Caso Compra/Venta Divisas.
            Dim Str As String = MedioPago.ToString
            MedioPago = CInt(Strings.Right(Str, 6))
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cuenta FROM Miselaneas WHERE Codigo = 2 AND Clave = " & MedioPago & ";", Miconexion)
                    cuenta = Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

        If cuenta <> 0 Then Return cuenta

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cuenta FROM Tablas WHERE Tipo = 27 AND Clave = " & MedioPago & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaCuentaTabla6(ByVal MedioPago As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cuenta FROM Tablas WHERE Tipo = 36 AND Clave = " & MedioPago & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaCuentaTabla8(ByVal Clave As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cuenta FROM Tablas WHERE Tipo = 33 AND Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaCuentaTabla9(ByVal Clave As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cuenta FROM Tablas WHERE Tipo = 32 AND Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaCuentaTabla12(ByVal Legajo As Integer) As Double

        Dim Cuenta As Decimal

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT CuentaContable FROM Empleados WHERE Legajo = " & Legajo & ";", Miconexion)
                    Cuenta = Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

        If Cuenta <> 0 Then Return Cuenta

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT CuentaContable FROM Empleados WHERE Legajo = " & Legajo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaCuentaTablaRetencion(ByVal MedioPago As Integer, ByVal Tipo As Integer) As Double

        Dim Sql As String

        If Tipo = 9 Then Sql = "SELECT Cuenta2 FROM Tablas WHERE Clave = " & MedioPago & ";"
        If Tipo = 11 Then Sql = "SELECT Cuenta FROM Tablas WHERE Clave = " & MedioPago & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaCuentaTablaInsumos(ByVal Insumo As Integer) As Double

        Dim Sql As String = "SELECT Cuenta FROM Insumos WHERE Clave = " & Insumo & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaCuentaTablaServicios(ByVal Servicio As Integer) As Double

        Dim Sql As String = "SELECT Cuenta FROM ArticulosServicios WHERE Secos = 0 AND Clave = " & Servicio & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaCuentaTablaSecos(ByVal Secos As Integer) As Double

        Dim Sql As String = "SELECT Cuenta FROM ArticulosServicios WHERE Secos = 1 AND Clave = " & Secos & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaCuentaTabla14(ByVal Tipo As Integer) As Double

        Dim Sql As String = "SELECT Cuenta FROM Tablas WHERE Tipo = 39 AND Clave = " & Tipo & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaCuentaTabla5(ByVal Clave As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cuenta2 FROM Tablas WHERE Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaCuentaTabla5_6(ByVal Clave As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cuenta FROM Tablas WHERE Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaConceptoEnTabla(ByVal Concepto As Integer) As Boolean

        Dim Clave As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Clave FROM Tablas WHERE Clave = " & Concepto & ";", Miconexion)
                    Clave = Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de datos al leer Tablas.", MsgBoxStyle.Critical)
            End
        End Try

        If Clave <> 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function HallaAsientosCabeza(ByVal TipoDocumento As Integer, ByVal Documento As Double, ByVal DtAsientoCabeza As DataTable, ByVal ConexionStr As String) As Boolean

        If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & TipoDocumento & " AND Documento = " & Documento & ";", ConexionStr, DtAsientoCabeza) Then Return False

        Return True

    End Function
    Public Function HallaAsientosCabezaUltimo(ByVal TipoDocumento As Integer, ByVal Documento As Double, ByVal DtAsientoCabeza As DataTable, ByVal ConexionStr As String) As Boolean

        Dim Ultimo As Integer

        Dim Sql As String = "SELECT MAX(Asiento) FROM AsientosCabeza WHERE TipoDocumento = " & TipoDocumento & " AND Documento = " & Documento & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim UltimoW = Cmd.ExecuteScalar()
                    If Not IsDBNull(UltimoW) Then
                        Ultimo = UltimoW
                    Else : Ultimo = -1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Ultimo = -1
        End Try

        If Ultimo = -1 Then
            MsgBox("Error al Leer Tabla: AsientosCabeza.", MsgBoxStyle.ApplicationModal)
            Return False
        End If

        If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = " & Ultimo & ";", ConexionStr, DtAsientoCabeza) Then Return False

        Return True

    End Function
    Public Function ArmaDocumentosAsientos() As DataTable


        Dim Dt As New DataTable

        Dt.Columns.Add("Codigo", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Try
            Dim Row As DataRow = Dt.NewRow
            Row("Nombre") = ""
            Row("Codigo") = 0
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Factura Venta"
            Row("Codigo") = 2
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Factura Servicios"
            Row("Codigo") = 7009
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Factura Secos"
            Row("Codigo") = 7010
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Factura Contables"
            Row("Codigo") = 71001
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota de Credito Domestica"
            Row("Codigo") = 4
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota de Credito Exportación"
            Row("Codigo") = 41
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota DEBITO Financiera"
            Row("Codigo") = 5
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota DEBITO Financiera Exportación"
            Row("Codigo") = 10005
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota DEBITO Por Diferencia de Cambio"
            Row("Codigo") = 11005
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota CREDITO Financiera"
            Row("Codigo") = 7
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota CREDITO Financiera Exportación"
            Row("Codigo") = 10007
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota CREDITO Por Diferencia de Cambio"
            Row("Codigo") = 11007
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota DEBITO del Cliente"
            Row("Codigo") = 50
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota CREDITO del Cliente"
            Row("Codigo") = 70
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Devolución Seña"
            Row("Codigo") = 65
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Recibo de Cobro"
            Row("Codigo") = 60
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Recibo de Cobro Exterior"
            Row("Codigo") = 61
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Recibo de Cobro Contable"
            Row("Codigo") = 62
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Devolución a Cliente"
            Row("Codigo") = 64
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Orden de Pago"
            Row("Codigo") = 600
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Orden de Pago"
            Row("Codigo") = 605
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Orden de Pago Exterior"
            Row("Codigo") = 601
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Egreso de Caja a Cta.Resul."
            Row("Codigo") = 607
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Devolución del Proveedor"
            Row("Codigo") = 604
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "N.V.L.P."
            Row("Codigo") = 800
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "N.V.L.P. Contable"
            Row("Codigo") = 801
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Factura Proveedor Reventa"
            Row("Codigo") = 900
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Factura Proveedor Afecta Lotes"
            Row("Codigo") = 901
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Factura Proveedor Orden de Compra"
            Row("Codigo") = 902
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Factura Proveedor Sin Comprobante"
            Row("Codigo") = 903
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Factura Contable"
            Row("Codigo") = 7030
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Liquidación Reventa"
            Row("Codigo") = 910
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Liquidación Consignación"
            Row("Codigo") = 912
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Liq. que Rempaza Facturas"
            Row("Codigo") = 911
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Liquidación Contable"
            Row("Codigo") = 913
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Liquidación Insumos"
            Row("Codigo") = 915
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota DEBITO del Cliente"
            Row("Codigo") = 50
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota CREDITO del Cliente"
            Row("Codigo") = 70
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota DEBITO al proveedor"
            Row("Codigo") = 6
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota DEBITO al proveedor Importación"
            Row("Codigo") = 10006
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota DEBITO Por Diferencia de Cambio"
            Row("Codigo") = 11006
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota CREDITO al Proveedor"
            Row("Codigo") = 8
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota CREDITO al Proveedor Importación"
            Row("Codigo") = 10008
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota CREDITO Por Diferencia de Cambio"
            Row("Codigo") = 11008
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota CREDITO Contable Del Proveedor"
            Row("Codigo") = 11009
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota CREDITO Contable a Cliente"
            Row("Codigo") = 11010
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota DEBITO del proveedor"
            Row("Codigo") = 500
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota CREDITO del Proveedor"
            Row("Codigo") = 700
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Recibo de Sueldo"
            Row("Codigo") = 4000
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Orden Pago de Sueldo"
            Row("Codigo") = 4010
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Factura Otros Pagos"
            Row("Codigo") = 5000
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota Debito Otros Proveedores"
            Row("Codigo") = 5005
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Nota Crédito Otros Proveedores"
            Row("Codigo") = 5007
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Orden Otro Pago"
            Row("Codigo") = 5010
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Devolución Otro proveedor"
            Row("Codigo") = 5020
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Rech.Cheque Prestamo Capital"
            Row("Codigo") = 1005
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Rech.Cheque Pago Prestamo"
            Row("Codigo") = 1007
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Reemplazo de Cheque"
            Row("Codigo") = 1008
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Credito Pago Sueldo"
            Row("Codigo") = 4007
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Credito Otro Pago"
            Row("Codigo") = 5007
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Rech.Cheque Deposito Bancario"
            Row("Codigo") = 93
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Extracción Bancaria"
            Row("Codigo") = 90
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Deposito Cheques al Dia Y Dif."
            Row("Codigo") = 6091
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Deposito Efectivo"
            Row("Codigo") = 91
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Transferencia Cta. Propias"
            Row("Codigo") = 6080
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Consumo de Insumos"
            Row("Codigo") = 6000
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Deposito Cheque propio"
            Row("Codigo") = 6010
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Deposito Debito Autom.Dife."
            Row("Codigo") = 6011
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Compra Divisas"
            Row("Codigo") = 16000
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Venta Divisas"
            Row("Codigo") = 16001
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Imgreso Mercaderia"
            Row("Codigo") = 6050
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Devolución Imgreso Mercaderia"
            Row("Codigo") = 6052
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Remito"
            Row("Codigo") = 6060
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Devolución Remito"
            Row("Codigo") = 6062
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Descarte Mercaderia"
            Row("Codigo") = 6055
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Diferencia Inventario"
            Row("Codigo") = 6056
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Costo Factura/Remito"
            Row("Codigo") = 6070
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Costo Factura/Remito Exportación"
            Row("Codigo") = 6071
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Costo Nota Credito Domestica"
            Row("Codigo") = 6072
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Costo Nota Credito Exportación"
            Row("Codigo") = 6073
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Reproceso Lote"
            Row("Codigo") = 7000
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Rechazo Cheque"
            Row("Codigo") = 7001
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Rechazo Cheque"
            Row("Codigo") = 7002
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Recepción Insumos"
            Row("Codigo") = 7003
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Devolución de Recepción de Insumos"
            Row("Codigo") = 7004
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Gasto Bancario"
            Row("Codigo") = 7005
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Factura Venta Exportación"
            Row("Codigo") = 7006
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Factura Prov. Reventa Importación"
            Row("Codigo") = 7007
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Otra Factura Prov. Importación"
            Row("Codigo") = 7008
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Rechazo Cheque(Otro Proveedor)"
            Row("Codigo") = 7011
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Ajuste Factura Exportación"
            Row("Codigo") = 7100
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Registro Prestamo"
            Row("Codigo") = 8000
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Prestamo Cancelacion Capital,Int,Gastos"
            Row("Codigo") = 8001
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Prestamo Ajuste Capital"
            Row("Codigo") = 8002
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Recupero Seña"
            Row("Codigo") = 7020
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Ajuste Fondo Fijo"
            Row("Codigo") = 7201
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Rendición Fondo Fijo"
            Row("Codigo") = 7203
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Cierre Fondo Fijo"
            Row("Codigo") = 7204
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Reposición Fondo Fijo"
            Row("Codigo") = 7205
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Rechazo Cheque(Aumenta Fondo)"
            Row("Codigo") = 7206
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Rechazo Cheque Reposición"
            Row("Codigo") = 7207
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Cobranza en el Exterior"
            Row("Codigo") = 12000
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Cobranza en Argentina"
            Row("Codigo") = 12003
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Transferencia Banco Exportación"
            Row("Codigo") = 12010
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Liquidacion Divisas Transferidas"
            Row("Codigo") = 12004
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Reintegros Aduaneros"
            Row("Codigo") = 12006
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Cobranza Reintegros Aduaneros"
            Row("Codigo") = 12007
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Consumos Lotes Prod. Trminados"
            Row("Codigo") = 61000
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Nombre") = "Costo Consumos Lotes Prod. Trminados"
            Row("Codigo") = 61001
            Dt.Rows.Add(Row)
            Return Dt
        Catch ex As Exception
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function UltimaNumeracionAsiento(ByVal ConexionStr) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Asiento) FROM AsientosCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Sub HallaCentroTipoOperacion(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal ConexionStr As String, ByRef Tipo As Integer, ByRef Centro As Integer)

        Dim Proveedor As Integer = 0

        Tipo = 0
        Centro = 0

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT TipoOperacion,Proveedor FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", ConexionStr, Dt) Then Exit Sub
        If Dt.Rows.Count = 0 Then Dt.Dispose() : Exit Sub

        Tipo = Dt.Rows(0).Item("TipoOperacion")
        Proveedor = Dt.Rows(0).Item("Proveedor")

        Dt.Dispose()

        Select Case Tipo
            Case 1, 2, 3
                Try
                    Using Miconexion As New OleDb.OleDbConnection(Conexion)
                        Miconexion.Open()
                        Using Cmd As New OleDb.OleDbCommand("SELECT Centro FROM Miselaneas WHERE Codigo = 1 AND Clave = " & Tipo & ";", Miconexion)
                            Centro = Cmd.ExecuteScalar()
                        End Using
                    End Using
                Catch ex As Exception
                    Exit Sub
                End Try
            Case 4
                Try
                    Using Miconexion As New OleDb.OleDbConnection(Conexion)
                        Miconexion.Open()
                        Using Cmd As New OleDb.OleDbCommand("SELECT Centro FROM Proveedores WHERE Clave = " & Proveedor & ";", Miconexion)
                            Centro = Cmd.ExecuteScalar()
                        End Using
                    End Using
                Catch ex As Exception
                    Exit Sub
                End Try
            Case Else
                Exit Sub
        End Select

    End Sub
    Public Function TieneTabla1(ByVal CodigoDocumento As Integer) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Tabla) FROM SeteoDocumentos WHERE Tabla = 1 AND TipoDocumento = " & CodigoDocumento & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function ArmaAsientoRemito(ByVal TipoDocumento As Integer, ByVal DtAsignacionLotes As DataTable, ByVal DtDetalleRemito As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal Fecha As Date) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow

        Dim Precio As Double = 0
        Dim Tipo As Integer
        Dim Centro As Integer
        Dim KilosNoAsignado As Double = 0
        Dim ConexionLote As String
        Dim CantidadLotes As Decimal = 0

        Dim Item As New ItemListaConceptosAsientos

        For Each Row As DataRow In DtAsignacionLotes.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row("Cantidad") <> 0 Then
                    CantidadLotes = CantidadLotes + Row("Cantidad")
                    Dim Fila2 As New ItemLotesParaAsientos
                    '
                    If Row("Operacion") = 1 Then
                        ConexionLote = Conexion
                    Else : ConexionLote = ConexionN
                    End If
                    '
                    Precio = 0
                    Tipo = 0
                    Centro = 0
                    HallaCentroTipoOperacion(Row("Lote"), Row("Secuencia"), ConexionLote, Tipo, Centro)
                    If Centro <= 0 Then
                        MsgBox("Error en Tipo Operacion en Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000"))
                        Return False
                    End If
                    If Tipo = 4 Then
                        Dim Negocio As Integer = HallaProveedorLote(Row("Operacion"), Row("Lote"), Row("Secuencia"))
                        If Negocio <= 0 Then
                            MsgBox("Error al Leer Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000"))
                            Return False
                        End If
                        Dim Costeo = HallaCosteoLote(Row("Operacion"), Row("Lote"))
                        If Costeo = -1 Then Return False
                        If Not HallaPrecioYCentroCosteo(Negocio, Costeo, Centro, Precio) Then Return False
                    Else
                        Precio = Refe
                    End If
                    '
                    Dim Kilos As Double = 0
                    Dim Iva As Double = 0
                    RowsBusqueda = DtDetalleRemito.Select("Indice = " & Row("Indice"))
                    HallaKilosIva(RowsBusqueda(0).Item("Articulo"), Kilos, Iva)
                    '
                    Fila2.Centro = Centro
                    Fila2.MontoNeto = Trunca(Precio * Row("Cantidad") * Kilos)
                    If Tipo = 1 Then Fila2.Clave = 301 'consignacion
                    If Tipo = 2 Then Fila2.Clave = 300 'reventa
                    If Tipo = 3 Then Fila2.Clave = 303 'reventa MG
                    If Tipo = 4 Then Fila2.Clave = 302 'costeo
                    ListaLotesParaAsiento.Add(Fila2)
                End If
            End If
        Next

        If CantidadLotes = 0 Then
            Dim ImporteTotal As Decimal = 0
            For Each Row As DataRow In DtDetalleRemito.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Dim Kilos As Double
                    Dim Iva As Double
                    HallaKilosIva(Row("Articulo"), Kilos, Iva)
                    ImporteTotal = ImporteTotal + Refe * (Row("Cantidad") - Row("Devueltas")) * Kilos
                End If
            Next
            '
            Item = New ItemListaConceptosAsientos
            Item.Clave = 202
            Item.Importe = Trunca(ImporteTotal)
            ListaConceptos.Add(Item)
        End If

        If Not Asiento(TipoDocumento, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Fecha, 0) Then Return False

        Return True

    End Function
    Public Function LeerCabezaYDetalleAsiento(ByVal TipoComprobante As Integer, ByVal Comprobante As Decimal, ByRef DtAsientoCabezaAux As DataTable, ByRef DtAsientoDetalleAux As DataTable, ByVal ConexionStr As String) As Boolean

        DtAsientoCabezaAux = New DataTable
        DtAsientoDetalleAux = New DataTable

        If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & TipoComprobante & " AND Documento = " & Comprobante & ";", ConexionStr, DtAsientoCabezaAux) Then Return False
        If DtAsientoCabezaAux.Rows.Count = 0 Then Return True

        If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaAux.Rows(0).Item("Asiento") & ";", ConexionStr, DtAsientoDetalleAux) Then Return False

        Return True

    End Function
    Public Sub BorraAsientosDetalleSinCabeza(ByVal Asiento As Decimal, ByVal ConexionStr As String)

        Dim Dt As New DataTable

        Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & Asiento & ";", ConexionStr, Dt)
        For Each Row As DataRow In Dt.Rows
            Row.Delete()
        Next

        Dim Trans As OleDb.OleDbTransaction

        Using MiConexion As New OleDb.OleDbConnection(Conexion)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Try
                    Dim Sql1 As String = "SELECT * FROM AsientosDetalle;"
                    Using DaP As New OleDb.OleDbDataAdapter(Sql1, MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(Dt.GetChanges)
                    End Using
                    Trans.Commit()
                Catch ex As OleDb.OleDbException
                    Trans.Rollback()
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                Finally
                    Trans = Nothing
                End Try
            Catch ex As Exception
            End Try
        End Using

        Dt.Dispose()

    End Sub
    Public Function ReHaceAsientoDetalleRemito(ByVal Remito As Decimal, ByVal DtLotesAsignadosAux As DataTable, ByVal DtDetalleRemitoAux As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal ConexionRemito As String) As Boolean

        Dim NroAsiento As Integer = 0

        Dim DtAsientoCabeza As New DataTable

        If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Estado <> 3 AND TipoDocumento = 6060 AND Documento = " & Remito & ";", ConexionRemito, DtAsientoCabeza) Then Return False
        If DtAsientoCabeza.Rows.Count = 0 Then
            Return True
        End If

        NroAsiento = DtAsientoCabeza.Rows(0).Item("Asiento")

        If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & NroAsiento & ";", ConexionRemito, DtAsientoDetalle) Then Return False

        Dim DtAsientoDetalleAux As DataTable = DtAsientoDetalle.Clone
        DtAsientoCabeza.Rows.Clear()
        If Not ArmaAsientoRemito(6060, DtLotesAsignadosAux, DtDetalleRemitoAux, DtAsientoCabeza, DtAsientoDetalleAux, "01/01/1800") Then Return False

        For Each Row As DataRow In DtAsientoDetalle.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtAsientoDetalleAux.Rows
            Row("Asiento") = NroAsiento
            Dim Row1 As DataRow = DtAsientoDetalle.NewRow
            For I As Integer = 0 To DtAsientoDetalleAux.Columns.Count - 1
                Row1.Item(I) = Row.Item(I)
            Next
            DtAsientoDetalle.Rows.Add(Row1)
        Next

        Return True

    End Function
    Public Sub MuestraComprobanteDelTipoAsiento(ByVal TipoAsiento As Integer, ByVal TipoComprobante As Integer, ByVal Comprobante As Decimal, ByVal Abierto As Boolean)

        If TipoAsiento = 0 Then
            MsgBox("Asiento Es Manual. Ver en Listado de Asientos.")
            Exit Sub
        End If
        If Comprobante = 0 Then
            MsgBox("Error: Comprobante = 0.")
            Exit Sub
        End If

        Select Case TipoComprobante
            Case 5, 13005, 6, 13006, 7, 13007, 8, 13008, 50, 70, 500, 700 'UnReciboDebitoCredito
                UnReciboDebitoCredito.PTipoNota = TipoComprobante
                UnReciboDebitoCredito.PNota = Comprobante
                UnReciboDebitoCredito.PAbierto = Abierto
                UnReciboDebitoCredito.PBloqueaFunciones = True
                UnReciboDebitoCredito.ShowDialog()
                Exit Sub
            Case 60, 62, 65, 600, 605, 601, 607, 61, 64, 604        'UnRecibo
                UnRecibo.PTipoNota = TipoComprobante
                UnRecibo.PNota = Comprobante
                UnRecibo.PAbierto = Abierto
                UnRecibo.PBloqueaFunciones = True
                UnRecibo.ShowDialog()
                Exit Sub
            Case 900, 901, 902, 903          'UnaFacturaProveedor
                UnaFacturaProveedor.PTipoFactura = TipoComprobante
                UnaFacturaProveedor.PFactura = Comprobante
                UnaFacturaProveedor.PAbierto = Abierto
                UnaFacturaProveedor.PBloqueaFunciones = True
                UnaFacturaProveedor.ShowDialog()
                Exit Sub
            Case 5005, 5007             'UnReciboDebitoCreditoOtrosProveedores
                UnReciboDebitoCreditoOtrosProveedores.PTipoNota = TipoComprobante
                UnReciboDebitoCreditoOtrosProveedores.PNota = Comprobante
                UnReciboDebitoCreditoOtrosProveedores.PAbierto = Abierto
                UnReciboDebitoCreditoOtrosProveedores.PBloqueaFunciones = True
                UnReciboDebitoCreditoOtrosProveedores.ShowDialog()
                Exit Sub
            Case 61000                       'UnConsumoPT
                UnConsumoPT.PConsumo = Comprobante
                UnConsumoPT.PAbierto = Abierto
                UnConsumoPT.PBloqueaFunciones = True
                UnConsumoPT.ShowDialog()
                Exit Sub
        End Select

        Select Case TipoAsiento
            Case 2, 6071, 6070, 7009, 7010, 71001
                UnaFactura.PFactura = Comprobante
                UnaFactura.PAbierto = Abierto
                UnaFactura.PBloqueaFunciones = True
                UnaFactura.ShowDialog()
                Exit Sub
            Case 4, 6072
                UnaNotaCredito.PNota = Comprobante
                UnaNotaCredito.PAbierto = Abierto
                UnaNotaCredito.PBloqueaFunciones = True
                UnaNotaCredito.ShowDialog()
                Exit Sub
            Case 41, 6073
                MsgBox("Debe entrar en el sistema de Exportación.", MsgBoxStyle.Information)
                Exit Sub
            Case 7006
                MsgBox("Debe entrar en el sistema de Exportación.", MsgBoxStyle.Information)
                Exit Sub
            Case 7203
                UnaFacturaProveedorFondoFijo.PFactura = Comprobante
                UnaFacturaProveedorFondoFijo.PAbierto = Abierto
                UnaFacturaProveedorFondoFijo.PBloqueaFunciones = True
                UnaFacturaProveedorFondoFijo.ShowDialog()
                Exit Sub
            Case 910, 911, 912
                UnaLiquidacion.PLiquidacion = Comprobante
                If TipoAsiento = 910 Or TipoAsiento = 911 Then UnaLiquidacion.PEsReventa = True
                If TipoAsiento = 912 Then UnaLiquidacion.PEsConsignacion = True
                UnaLiquidacion.PAbierto = Abierto
                UnaLiquidacion.PBloqueaFunciones = True
                UnaLiquidacion.ShowDialog()
                Exit Sub
            Case 915
                UnaLiquidacionInsumos.PLiquidacion = Comprobante
                UnaLiquidacionInsumos.PAbierto = Abierto
                UnaLiquidacionInsumos.PBloqueaFunciones = True
                UnaLiquidacionInsumos.ShowDialog()
                Exit Sub
            Case 913
                UnaLiquidacionContable.PLiquidacion = Comprobante
                UnaLiquidacionContable.PAbierto = Abierto
                UnaLiquidacionContable.PBloqueaFunciones = True
                UnaLiquidacionContable.ShowDialog()
                Exit Sub
            Case 800
                UnaNVLP.PLiquidacion = Comprobante
                UnaNVLP.PAbierto = Abierto
                UnaNVLP.PBloqueaFunciones = True
                UnaNVLP.ShowDialog()
                Exit Sub
            Case 801
                UnaNVLPContable.PLiquidacion = Comprobante
                UnaNVLPContable.PAbierto = Abierto
                UnaNVLPContable.PBloqueaFunciones = True
                UnaNVLPContable.ShowDialog()
                Exit Sub
            Case 16000, 16001
                UnaCompraDivisas.PMovimiento = Comprobante
                UnaCompraDivisas.PAbierto = Abierto
                UnaCompraDivisas.PBloqueaFunciones = True
                UnaCompraDivisas.ShowDialog()
                Exit Sub
            Case 6000
                If Comprobante >= 0 Then
                    UnConsumoDeInsumo.PConsumo = Comprobante
                    UnConsumoDeInsumo.PAbierto = Abierto
                Else
                    UnConsumoDeInsumo.PConsumo = -Comprobante
                    UnConsumoDeInsumo.PAbierto = True
                End If
                UnConsumoDeInsumo.PBloqueaFunciones = True
                UnConsumoDeInsumo.ShowDialog()
                Exit Sub
            Case 41, 6073
                MsgBox("Debe entrar en el sistema de Exportación.", MsgBoxStyle.Information)
                Exit Sub
            Case 4010
                UnaOrdenPagoSueldos.PNota = Comprobante
                UnaOrdenPagoSueldos.PAbierto = Abierto
                UnaOrdenPagoSueldos.PBloqueaFunciones = True
                UnaOrdenPagoSueldos.ShowDialog()
                Exit Sub
            Case 6091, 91, 90, 93, 6080
                UnMovimientoBancario.PMovimiento = Comprobante
                UnMovimientoBancario.PAbierto = Abierto
                UnMovimientoBancario.PBloqueaFunciones = True
                UnMovimientoBancario.ShowDialog()
                Exit Sub
            Case 5010, 5020
                UnReciboOtrosProveedores.PNota = Comprobante
                UnReciboOtrosProveedores.PAbierto = Abierto
                UnReciboOtrosProveedores.PBloqueaFunciones = True
                UnReciboOtrosProveedores.ShowDialog()
                Exit Sub
            Case 6060
                UnRemito.PRemito = Comprobante
                UnRemito.PAbierto = Abierto
                UnRemito.PBloqueaFunciones = True
                UnRemito.ShowDialog()
                Exit Sub
            Case 7206, 7207, 1005, 1007, 7002
                If TipoAsiento = 7206 Then UnReciboDebitoCreditoGenerica.PTipoNota = 7005
                If TipoAsiento = 7207 Then UnReciboDebitoCreditoGenerica.PTipoNota = 7007
                If TipoAsiento = 1005 Then UnReciboDebitoCreditoGenerica.PTipoNota = 1005
                If TipoAsiento = 1007 Then UnReciboDebitoCreditoGenerica.PTipoNota = 1007
                If TipoAsiento = 7002 Then UnReciboDebitoCreditoGenerica.PTipoNota = 4007
                UnReciboDebitoCreditoGenerica.PNota = Comprobante
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.PBloqueaFunciones = True
                UnReciboDebitoCreditoGenerica.ShowDialog()
                Exit Sub
            Case 7011             'UnReciboDebitoCreditoOtrosProveedores
                UnReciboDebitoCreditoOtrosProveedores.PNota = Comprobante
                UnReciboDebitoCreditoOtrosProveedores.PAbierto = Abierto
                UnReciboDebitoCreditoOtrosProveedores.PBloqueaFunciones = True
                UnReciboDebitoCreditoOtrosProveedores.ShowDialog()
                Exit Sub

            Case Else
                MsgBox(TipoAsiento & " No Previsto. Avisar a Sistema.", MsgBoxStyle.Information)
                Exit Sub
        End Select

    End Sub
End Module
