Module AsignacionLotesYAsientos
    Public Function ArmaAsignacionFactura(ByVal TipoComprobante As Integer, ByVal DtCabezaB As DataTable, ByVal DtDetalleB As DataTable, ByVal DtCabezaN As DataTable, ByVal DtDetalleN As DataTable, _
                                     ByRef DtAsignacionLotesB As DataTable, ByRef DtAsignacionLotesN As DataTable, ByVal ListaDeLotes As List(Of FilaAsignacion), ByVal PPaseDeProyectos As ItemPaseDeProyectos) As Boolean

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            Conexion = PPaseDeProyectos.Conexion
            ConexionN = PPaseDeProyectos.ConexionN
            PermisoTotal = PPaseDeProyectos.PermisoTotal
        End If
        '----------------------------------------------------------------------------------

        'Actualiza Asignacion lotes.
        Dim DtAsignacionLotesBAux As DataTable = DtAsignacionLotesB.Copy
        Dim DtAsignacionLotesNAux As DataTable = DtAsignacionLotesN.Copy
        Dim Rel As Boolean

        If DtCabezaB.Rows.Count <> 0 And DtCabezaN.Rows.Count <> 0 Then Rel = True
        'Genera DtAsignacionDeLote con cantides de ListaDeLotes e Importes de la factura.  
        If DtCabezaB.Rows.Count <> 0 Then
            AsignaLotesFacturas(TipoComprobante, DtCabezaB, ListaDeLotes, DtAsignacionLotesBAux, DtDetalleB, Rel)
        End If
        If DtCabezaN.Rows.Count <> 0 Then
            AsignaLotesFacturas(TipoComprobante, DtCabezaN, ListaDeLotes, DtAsignacionLotesNAux, DtDetalleN, Rel)
        End If

        DtAsignacionLotesB = DtAsignacionLotesBAux
        DtAsignacionLotesN = DtAsignacionLotesNAux

        DtAsignacionLotesBAux.Dispose()
        DtAsignacionLotesNAux.Dispose()

        Return True

    End Function
    Public Function ArmaStockFactura(ByVal DtAsignacionLotes As DataTable, ByRef DtStockB As DataTable, ByRef DtStockN As DataTable, _
                                 ByVal ListaDeLotes As List(Of FilaAsignacion), ByVal PPaseDeProyectos As ItemPaseDeProyectos) As Boolean

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            Conexion = PPaseDeProyectos.Conexion
            ConexionN = PPaseDeProyectos.ConexionN
            PermisoTotal = PPaseDeProyectos.PermisoTotal
        End If
        '----------------------------------------------------------------------------------

        'Actualiza Stock.  Arma las tablas para actualizar el Stock de Lotes.
        DtStockB = New DataTable
        DtStockN = New DataTable

        If Not ActualizaStockAsignados(DtAsignacionLotes, DtStockB, ListaDeLotes, 1) Then Return False
        If Not ActualizaStockAsignados(DtAsignacionLotes, DtStockN, ListaDeLotes, 2) Then Return False

        Return True

    End Function
    Public Function ArmaAsientosFacturas(ByVal Funcion As String, ByVal TipoAsiento As Integer, ByVal DtCabezaB As DataTable, ByVal DtCabezaN As DataTable, ByVal DtDetalleB As DataTable, ByVal DtDetalleN As DataTable, ByVal ListaDeLotes As List(Of FilaAsignacion), _
                    ByVal DtAsignacionLotesOriginal As DataTable, ByRef DtAsientoCabezaB As DataTable, ByRef DtAsientoDetalleB As DataTable, ByRef DtAsientoCabezaN As DataTable, ByRef DtAsientoDetalleN As DataTable, ByVal DiferenciaDescuentoB As Decimal, ByVal DiferenciaDescuentoN As Decimal, ByVal PPaseDeProyectos As ItemPaseDeProyectos, ByVal Fecha As Date) As Boolean

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            Conexion = PPaseDeProyectos.Conexion
            ConexionN = PPaseDeProyectos.ConexionN
            PermisoTotal = PPaseDeProyectos.PermisoTotal
        End If
        '----------------------------------------------------------------------------------

        'Arma nueva lista con las diferencias de cantidades entre asignacion original y nueva asignacion. (Asignacion-Nueva - Asignacion-Vieja) 
        Dim ListaDeLotesParaModificar As New List(Of FilaAsignacion)
        ArmaLotesParaModificar(ListaDeLotesParaModificar, DtAsignacionLotesOriginal, ListaDeLotes)

        If DtCabezaB.Rows.Count <> 0 Then
            If Not ArmaArchivosAsiento(Funcion, TipoAsiento, DtCabezaB.Rows(0).Item("Factura"), DtCabezaB, DtDetalleB, DtAsientoCabezaB, DtAsientoDetalleB, Conexion, ListaDeLotesParaModificar, DiferenciaDescuentoB, Fecha) Then Return False
        End If
        If DtCabezaN.Rows.Count <> 0 Then
            If Not ArmaArchivosAsiento(Funcion, TipoAsiento, DtCabezaN.Rows(0).Item("Factura"), DtCabezaN, DtDetalleN, DtAsientoCabezaN, DtAsientoDetalleN, ConexionN, ListaDeLotesParaModificar, DiferenciaDescuentoN, Fecha) Then Return False
        End If

        Return True

    End Function
    Public Function ArmaAsientosCostosFacturas(ByVal Funcion As String, ByVal TipoAsientoCosto As Integer, ByVal DtCabezaB As DataTable, ByVal DtCabezaN As DataTable, ByVal DtDetalleB As DataTable, ByVal DtDetalleN As DataTable, ByVal ListaDeLotes As List(Of FilaAsignacion), _
                      ByVal DtAsignacionLotesOriginal As DataTable, ByRef DtAsientoCabezaB As DataTable, ByRef DtAsientoDetalleB As DataTable, ByRef DtAsientoCabezaN As DataTable, ByRef DtAsientoDetalleN As DataTable, ByVal PPaseDeProyectos As ItemPaseDeProyectos, ByVal fecha As Date) As Boolean

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            Conexion = PPaseDeProyectos.Conexion
            ConexionN = PPaseDeProyectos.ConexionN
            PermisoTotal = PPaseDeProyectos.PermisoTotal
        End If
        '----------------------------------------------------------------------------------

        'Arma nueva lista con las diferencias de cantidades entre asignacion original y nueva asignacion. (Asignacion-Nueva - Asignacion-Vieja) 
        Dim ListaDeLotesParaModificar As New List(Of FilaAsignacion)
        ArmaLotesParaModificar(ListaDeLotesParaModificar, DtAsignacionLotesOriginal, ListaDeLotes)

        Dim PorcentajeB As Decimal = 0
        Dim PorcentajeN As Decimal = 0
        Dim ImporteBW As Decimal = 0
        Dim ImporteNW As Decimal = 0

        ' Calcula Porcentajes entre parte blanca y negra para calcular asiernos.             REVISAR!!!!!!
        If DtCabezaB.Rows.Count <> 0 And DtCabezaN.Rows.Count = 0 Then
            If DtCabezaB.Rows(0).Item("Final") Then
                ImporteBW = DtDetalleB.Rows(0).Item("TotalArticulo")
            Else
                ImporteBW = DtDetalleB.Rows(0).Item("TotalArticulo") - CalculaIva(DtDetalleB.Rows(0).Item("Cantidad"), DtDetalleB.Rows(0).Item("Precio"), DtDetalleB.Rows(0).Item("Iva"))
            End If
            PorcentajeB = 100
            PorcentajeN = 0
        End If
        If DtCabezaN.Rows.Count <> 0 And DtCabezaB.Rows.Count = 0 Then
            If DtCabezaN.Rows(0).Item("Final") Then
                ImporteNW = DtDetalleN.Rows(0).Item("TotalArticulo")
            Else
                ImporteNW = DtDetalleN.Rows(0).Item("TotalArticulo") - CalculaIva(DtDetalleN.Rows(0).Item("Cantidad"), DtDetalleN.Rows(0).Item("Precio"), DtDetalleN.Rows(0).Item("Iva"))
            End If
            PorcentajeB = 0
            PorcentajeN = 100
        End If
        If DtCabezaB.Rows.Count <> 0 And DtCabezaN.Rows.Count <> 0 Then
            If DtCabezaB.Rows(0).Item("Final") Then
                ImporteBW = DtDetalleB.Rows(0).Item("TotalArticulo")
                ImporteNW = DtDetalleN.Rows(0).Item("TotalArticulo")
                PorcentajeB = Trunca(ImporteBW * 100 / (ImporteBW + ImporteNW))
                PorcentajeN = Trunca(100 - PorcentajeB)
            Else
                ImporteBW = DtDetalleB.Rows(0).Item("TotalArticulo") - CalculaIva(DtDetalleB.Rows(0).Item("Cantidad"), DtDetalleB.Rows(0).Item("Precio"), DtDetalleB.Rows(0).Item("Iva"))
                ImporteNW = DtDetalleN.Rows(0).Item("TotalArticulo") - CalculaIva(DtDetalleN.Rows(0).Item("Cantidad"), DtDetalleN.Rows(0).Item("Precio"), DtDetalleN.Rows(0).Item("Iva"))
                PorcentajeB = Trunca(ImporteBW * 100 / (ImporteBW + ImporteNW))
                PorcentajeN = 100 - PorcentajeB
            End If
        End If

        If DtCabezaB.Rows.Count <> 0 Then
            If Not ArmaArchivosAsientoCosto(Funcion, TipoAsientoCosto, DtCabezaB.Rows(0).Item("Factura"), DtCabezaB, DtDetalleB, DtAsientoCabezaB, DtAsientoDetalleB, PorcentajeB, Conexion, ListaDeLotesParaModificar, fecha) Then Return False
        End If
        If DtCabezaN.Rows.Count <> 0 Then
            If Not ArmaArchivosAsientoCosto(Funcion, TipoAsientoCosto, DtCabezaN.Rows(0).Item("Factura"), DtCabezaN, DtDetalleN, DtAsientoCabezaN, DtAsientoDetalleN, PorcentajeN, ConexionN, ListaDeLotesParaModificar, fecha) Then Return False
        End If

        Return True

    End Function
    Private Sub AsignaLotesFacturas(ByVal TipoComprobante As Integer, ByVal DtCabeza As DataTable, ByVal ListaDeLotes As List(Of FilaAsignacion), ByRef DtAsignacionLotesAux As DataTable, ByVal DtDetalle As DataTable, ByVal Rel As Boolean)

        'que pasa cuando el lote esta facturado o liquidado.

        Dim RowsBusqueda() As DataRow
        Dim RowsBusqueda1() As DataRow

        For Each Fila As FilaAsignacion In ListaDeLotes
            RowsBusqueda = DtAsignacionLotesAux.Select("Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Cantidad") = Fila.Asignado
                RowsBusqueda1 = DtDetalle.Select("Indice = " & Fila.Indice)
                If RowsBusqueda1.Length <> 0 Then  'Si fue copiado de un remito puede ser que una devolucion en remito lo halla dejado en 0 y al copia la asignacion de la factura no lo encuentre en detalle.
                    RowsBusqueda(0).Item("ImporteSinIva") = CalculaNeto(Fila.Asignado, RowsBusqueda1(0).Item("Precio"))
                    RowsBusqueda(0).Item("Importe") = Trunca(RowsBusqueda(0).Item("ImporteSinIva") + CalculaIva(Fila.Asignado, RowsBusqueda1(0).Item("Precio"), RowsBusqueda1(0).Item("Iva")))
                    If DtCabeza.Rows(0).Item("Moneda") <> 1 Then
                        RowsBusqueda(0).Item("ImporteSinIva") = Trunca(RowsBusqueda(0).Item("ImporteSinIva") * DtCabeza.Rows(0).Item("Cambio"))
                        RowsBusqueda(0).Item("Importe") = Trunca(RowsBusqueda(0).Item("Importe") * DtCabeza.Rows(0).Item("Cambio"))
                    End If
                End If
            Else
                Dim Row As DataRow = DtAsignacionLotesAux.NewRow()
                Row("TipoComprobante") = TipoComprobante
                Row("Comprobante") = DtCabeza.Rows(0).Item("Factura")
                Row("Indice") = Fila.Indice
                Row("Lote") = Fila.Lote
                Row("Secuencia") = Fila.Secuencia
                Row("Deposito") = Fila.Deposito
                Row("Operacion") = Fila.Operacion
                Row("Cantidad") = Fila.Asignado
                Row("Rel") = Rel
                RowsBusqueda = DtDetalle.Select("Indice = " & Fila.Indice)
                If RowsBusqueda.Length <> 0 Then ' igual = 0 es el caso en que hubo una devolucion por todo lo asignado.
                    Row("ImporteSinIva") = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                    Row("Importe") = Trunca(Row("ImporteSinIva") + CalculaIva(Fila.Asignado, RowsBusqueda(0).Item("Precio"), RowsBusqueda(0).Item("Iva")))
                    If DtCabeza.Rows(0).Item("Moneda") <> 1 Then
                        Row("ImporteSinIva") = Trunca(Row("ImporteSinIva") * DtCabeza.Rows(0).Item("Cambio"))
                        Row("Importe") = Trunca(Row("Importe") * DtCabeza.Rows(0).Item("Cambio"))
                    End If
                Else
                    Row("ImporteSinIva") = 0
                    Row("Importe") = 0
                End If
                Row("Facturado") = False
                Row("Liquidado") = False
                DtAsignacionLotesAux.Rows.Add(Row)
            End If
        Next
        For Each Row As DataRow In DtAsignacionLotesAux.Rows
            If Not ExisteEnListaAsignacionIndiceLotes(ListaDeLotes, Row("Indice"), Row("Lote"), Row("Secuencia")) Then
                Row.Delete()
            End If
        Next

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal TipoAsiento As Integer, ByVal Comprobante As Double, ByVal DtFacturaCabeza As DataTable, ByVal DtFacturaDetalle As DataTable, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable, ByVal ConexionStr As String, ByVal ListaLotes As List(Of FilaAsignacion), ByVal DiferenciaDescuento As Decimal, ByVal Fecha As Date) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow
        Dim Importe As Decimal = 0

        For Each Fila As FilaAsignacion In ListaLotes
            If Fila.Asignado <> 0 Then
                Dim Tipo As Integer
                Dim Centro As Integer
                Dim Fila2 As New ItemLotesParaAsientos
                If Fila.Operacion = 1 Then
                    HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, Conexion, Tipo, Centro)
                Else : HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, ConexionN, Tipo, Centro)
                End If
                If Centro <= 0 Then
                    MsgBox("Error en Tipo Operacion en Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                    Return False
                End If
                RowsBusqueda = DtFacturaDetalle.Select("Indice = " & Fila.Indice)
                Fila2.Centro = Centro
                Fila2.MontoNeto = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                If DtFacturaCabeza.Rows(0).Item("Moneda") <> 1 Then Fila2.MontoNeto = Trunca(Fila2.MontoNeto * DtFacturaCabeza.Rows(0).Item("Cambio"))
                Importe = Importe + Fila2.MontoNeto
                If Funcion = "B" Then Fila2.MontoNeto = -Fila2.MontoNeto
                If Tipo = 1 Then Fila2.Clave = 301 'consignacion
                If Tipo = 2 Then Fila2.Clave = 300 'reventa
                If Tipo = 3 Then Fila2.Clave = 303 'reventa MG
                If Tipo = 4 Then Fila2.Clave = 302 'costeo
                ListaLotesParaAsiento.Add(Fila2)
            End If
        Next

        Dim Item As New ItemListaConceptosAsientos

        Item = New ItemListaConceptosAsientos
        Item.Clave = 202
        If Funcion = "A" Then Item.Importe = -Importe
        If Funcion = "B" Then Item.Importe = Importe
        If Funcion = "M" Then
            If Importe <= 0 Then
                Item.Importe = -Importe
            Else
                Item.Importe = Importe
            End If
        End If
        ListaConceptos.Add(Item)

        If Not AgregaAlAsiento(TipoAsiento, Comprobante, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Fecha) Then Return False

        If DtAsientoDetalle.Rows.Count = 0 Then
            '    MsgBox("No se pudo Generar Asiento. Operación se CANCELA.")
            '    Return False
        End If

        Return True
    End Function
    Private Function ArmaArchivosAsientoCosto(ByVal Funcion As String, ByVal TipoAsientoCosto As Integer, ByVal Comprobante As Decimal, ByVal DtFacturaCabeza As DataTable, ByVal DtFacturaDetalle As DataTable, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable, ByVal Porcentaje As Double, ByVal ConexionStr As String, ByVal ListaLotes As List(Of FilaAsignacion), ByVal Fecha As Date) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow

        Dim Precio As Decimal = 0
        Dim Tipo As Integer
        Dim Centro As Integer
        Dim ImporteTotal As Decimal
        Dim KilosNoAsignado As Decimal = 0
        Dim Item As New ItemListaConceptosAsientos

        For Each Fila As FilaAsignacion In ListaLotes
            If Fila.Asignado <> 0 Then
                Tipo = 0
                Centro = 0
                Dim Fila2 As New ItemLotesParaAsientos
                '
                RowsBusqueda = DtFacturaDetalle.Select("Indice = " & Fila.Indice)
                '
                If Fila.Operacion = 1 Then
                    HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, Conexion, Tipo, Centro)
                Else : HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, ConexionN, Tipo, Centro)
                End If
                If Centro <= 0 Then
                    MsgBox("Error en Tipo Operacion en Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                    Return False
                End If
                If Tipo = 4 Then
                    Dim Negocio As Integer = HallaProveedorLote(Fila.Operacion, Fila.Lote, Fila.Secuencia)
                    If Negocio <= 0 Then
                        MsgBox("Error al Leer Lotes " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                        Return False
                    End If
                    Dim Costeo = HallaCosteoLote(Fila.Operacion, Fila.Lote)
                    If Costeo = -1 Then Return False
                    If Not HallaPrecioYCentroCosteo(Negocio, Costeo, Centro, Precio) Then Return False
                Else
                    Precio = Refe
                End If
                Dim Kilos As Double
                Dim Iva As Double
                HallaKilosIva(RowsBusqueda(0).Item("Articulo"), Kilos, Iva)
                '
                Fila2.Centro = Centro
                Fila2.MontoNeto = Trunca(Precio * Fila.Asignado * Kilos * Porcentaje / 100)
                If Funcion = "B" Then Fila2.MontoNeto = -Fila2.MontoNeto
                ImporteTotal = ImporteTotal + Fila2.MontoNeto
                KilosNoAsignado = Trunca(KilosNoAsignado + (Fila.Asignado * Kilos))
                If Fila.Asignado < 0 Then KilosNoAsignado = -KilosNoAsignado
                If Tipo = 1 Then Fila2.Clave = 301 'consignacion
                If Tipo = 2 Then Fila2.Clave = 300 'reventa
                If Tipo = 3 Then Fila2.Clave = 303 'reventa MG
                If Tipo = 4 Then Fila2.Clave = 302 'costeo
                ListaLotesParaAsiento.Add(Fila2)
            End If
        Next
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 202
        If Funcion = "A" Then Item.Importe = -Trunca(Refe * KilosNoAsignado * Porcentaje / 100)
        If Funcion = "B" Then Item.Importe = Trunca(Refe * KilosNoAsignado * Porcentaje / 100)
        ListaConceptos.Add(Item)

        If Not AgregaAlAsiento(TipoAsientoCosto, Comprobante, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Fecha) Then Return False

        Return True

    End Function
    Private Function AgregaAlAsiento(ByVal TipoDocumento As Integer, ByVal Documento As Double, ByRef ListaConceptos As List(Of ItemListaConceptosAsientos), ByRef DtAsientoCabeza As DataTable, _
                          ByRef DtAsientoDetalle As DataTable, ByRef ListaCuentas As List(Of ItemCuentasAsientos), ByRef ListaLotes As List(Of ItemLotesParaAsientos), ByRef ListaIVA As List(Of ItemListaConceptosAsientos), ByRef ListaRetenciones As List(Of ItemListaConceptosAsientos), ByVal FechaContable As DateTime) As Boolean

        Dim NroAsiento As Integer = 0

        If DtAsientoCabeza.Rows.Count <> 0 Then
            NroAsiento = DtAsientoCabeza.Rows(0).Item("Asiento")
        End If

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
    '--------------------------------------------------------------------------------------------------------------
    '----------------------------------------------Para Remito ----------------------------------------------------
    '--------------------------------------------------------------------------------------------------------------
    Public Function ArmaAsignacionRemitos(ByVal TipoComprobante As Integer, ByVal DtCabeza As DataTable, ByRef DtAsignacionLotes As DataTable, ByVal ListaDeLotes As List(Of FilaAsignacion)) As Boolean

        'Actualiza Asignacion lotes.
        Dim DtAsignacionLotesAux As DataTable = DtAsignacionLotes.Copy

        'Genera DtAsignacionDeLote con cantides de ListaDeLotes e Importes de la factura.  
        AsignaLotesRemitos(TipoComprobante, DtCabeza, ListaDeLotes, DtAsignacionLotesAux)

        DtAsignacionLotes = DtAsignacionLotesAux

        DtAsignacionLotesAux.Dispose()

        Return True

    End Function
    Private Sub AsignaLotesRemitos(ByVal TipoComprobante As Integer, ByVal DtCabezaAux As DataTable, ByVal ListaDeLotes As List(Of FilaAsignacion), ByRef DtAsignacionLotesAux As DataTable)

        Dim RowsBusqueda() As DataRow

        'Modifica AsignacionLotes del Remito.
        For Each Fila As FilaAsignacion In ListaDeLotes
            RowsBusqueda = DtAsignacionLotesAux.Select("Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Cantidad") = Fila.Asignado
            Else
                Dim Row As DataRow = DtAsignacionLotesAux.NewRow()
                Row("Lote") = Fila.Lote
                Row("Secuencia") = Fila.Secuencia
                Row("Deposito") = Fila.Deposito
                Row("Indice") = Fila.Indice
                Row("TipoComprobante") = TipoComprobante
                Row("Comprobante") = DtCabezaAux.Rows(0).Item("Remito")
                Row("Cantidad") = Fila.Asignado
                Row("Operacion") = Fila.Operacion
                Row("ImporteSinIva") = 0
                Row("Importe") = 0
                Row("Facturado") = False
                Row("Liquidado") = False
                Row("Rel") = False
                DtAsignacionLotesAux.Rows.Add(Row)
            End If
        Next
        For Each Row As DataRow In DtAsignacionLotesAux.Rows
            If Not ExisteEnListaAsignacionIndiceLotes(ListaDeLotes, Row("Indice"), Row("Lote"), Row("Secuencia")) Then
                Row.Delete()
                ''''''''Row.Delete()   esta instruccion esta anulado, debo poner cantidad = 0 para que no desaparezca de AsignacionLotes por que se dio el caso
                '                      de tener una devolucion del lote, luego reasignar el lote a otro numero de lote en el remito y al querer anular la devolucion
                '                      y no encontrar el lote anterior a la reasignacion da error.
                '                      Ej.: en el remito asigne al lote 200/001 1000 Un. luego una devolucion de 500 un. Luego reasigne en el remito las 500 Un restante al lote
                '                      300/001, cuando quise anular la devolucion no encontro el lote 200/001 en el remito se produce un error en ejecucion. 
            End If
        Next

    End Sub
    Public Sub AgregarADtAFacturar(ByRef Dt As DataTable, ByVal Operacion As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Cantidad As Decimal, ByVal Senia As Decimal, ByVal Articulo As Integer, ByVal Descuento As Decimal, ByVal KilosXUnidad As Decimal, ByVal TipoPrecio As Integer, ByVal Precio As Decimal)

        Dim Row As DataRow = Dt.NewRow
        Row("Operacion") = Operacion
        Row("Indice") = 0
        Row("Lote") = Lote
        Row("Secuencia") = Secuencia
        Row("LoteYSecuencia") = Lote & "/" & Format(Secuencia, "000")
        Row("Articulo") = Articulo
        Row("Descuento") = Descuento
        Row("KilosXUnidad") = KilosXUnidad
        Row("Cantidad") = Cantidad
        Row("TipoPrecio") = TipoPrecio
        Row("Precio") = Precio
        Row("Senia") = Senia
        Dt.Rows.Add(Row)

    End Sub


End Module
