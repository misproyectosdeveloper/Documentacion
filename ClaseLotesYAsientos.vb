Public Class ClaseLotesYAsientos

    Public Shared Function ArmaAsientosFacturasParaAlta(ByVal TipoAsiento As Integer, ByVal DtCabezaB As DataTable, ByVal DtCabezaN As DataTable, ByVal DtDetalleB As DataTable, ByVal DtDetalleN As DataTable, ByVal DtFacturasPercepcionesB As DataTable, _
                       ByRef DtAsientoCabezaB As DataTable, ByRef DtAsientoDetalleB As DataTable, ByRef DtAsientoCabezaN As DataTable, ByRef DtAsientoDetalleN As DataTable, ByVal DiferenciaDescuentoB As Decimal, ByVal DiferenciaDescuentoN As Decimal, ByVal PPaseDeProyectos As ItemPaseDeProyectos, ByVal Fecha As Date) As Boolean

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            Conexion = PPaseDeProyectos.Conexion
            ConexionN = PPaseDeProyectos.ConexionN
            PermisoTotal = PPaseDeProyectos.PermisoTotal
        End If
        '----------------------------------------------------------------------------------

        If DtCabezaB.Rows.Count <> 0 Then
            If Not ArmaArchivosAsientoFacturaNueva("A", TipoAsiento, DtCabezaB, DtDetalleB, DtFacturasPercepcionesB, DtAsientoCabezaB, DtAsientoDetalleB, DiferenciaDescuentoB, Fecha) Then Return False
        End If
        If DtCabezaN.Rows.Count <> 0 Then
            If Not ArmaArchivosAsientoFacturaNueva("A", TipoAsiento, DtCabezaN, DtDetalleN, New DataTable, DtAsientoCabezaN, DtAsientoDetalleN, DiferenciaDescuentoN, Fecha) Then Return False
        End If

        Return True

    End Function
    Public Shared Function ArmaAsientosCostosFacturasParaAlta(ByVal TipoAsientoCosto As Integer, ByVal DtCabezaB As DataTable, ByVal DtCabezaN As DataTable, ByVal DtDetalleB As DataTable, ByVal DtDetalleN As DataTable, _
                    ByRef DtAsientoCabezaB As DataTable, ByRef DtAsientoDetalleB As DataTable, ByRef DtAsientoCabezaN As DataTable, ByRef DtAsientoDetalleN As DataTable, ByVal PPaseDeProyectos As ItemPaseDeProyectos, ByVal fecha As Date) As Boolean

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            Conexion = PPaseDeProyectos.Conexion
            ConexionN = PPaseDeProyectos.ConexionN
            PermisoTotal = PPaseDeProyectos.PermisoTotal
        End If
        '----------------------------------------------------------------------------------

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
            If DtCabezaB.Rows(0).Item("EsExterior") Then
                If Not ArmaArchivosAsientoCostoExteriorNueva("A", TipoAsientoCosto, DtDetalleB, DtDetalleN, DtAsientoCabezaB, DtAsientoDetalleB, fecha) Then Return False
            Else
                If Not ArmaArchivosAsientoCostoFacturaNueva("A", TipoAsientoCosto, DtCabezaB, DtDetalleB, DtAsientoCabezaB, DtAsientoDetalleB, PorcentajeB, fecha) Then Return False
            End If
        End If

        If DtCabezaN.Rows.Count <> 0 Then
            If DtCabezaN.Rows(0).Item("EsExterior") Then
                If Not ArmaArchivosAsientoCostoExteriorNueva("A", TipoAsientoCosto, DtDetalleN, DtDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, fecha) Then Return False
            Else
                If Not ArmaArchivosAsientoCostoFacturaNueva("A", TipoAsientoCosto, DtCabezaN, DtDetalleN, DtAsientoCabezaN, DtAsientoDetalleN, PorcentajeN, fecha) Then Return False
            End If
        End If

        Return True

    End Function
    Public Shared Function ArmaArchivosAsientoFacturaNueva(ByVal Funcion As String, ByVal TipoAsiento As Integer, ByVal DtFacturaCabeza As DataTable, ByVal DtFacturaDetalle As DataTable, ByVal DtFacturasPercepcionesB As DataTable, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable, ByVal DiferenciaDescuento As Decimal, ByVal Fecha As Date) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Importe As Decimal = 0

        Dim Item As New ItemListaConceptosAsientos

        Item = New ItemListaConceptosAsientos
        For Each Row As DataRow In DtFacturaDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row("Iva") <> 0 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = HallaClaveIva(Row("Iva"))
                    If Item.Clave <= 0 Then
                        MsgBox("Error al leer Tabla de IVA. Operación se CANCELA.")
                        Return False
                    End If
                    Item.Importe = CalculaIva(Row("Cantidad"), Row("Precio"), Row("Iva"))
                    Item.TipoIva = 6
                    ListaIVA.Add(Item)
                End If
            End If
        Next
        '
        Dim MontoNeto As Decimal
        For Each Row As DataRow In DtFacturaDetalle.Rows
            MontoNeto = MontoNeto + CalculaNeto(Row("Cantidad"), Row("Precio"))
        Next
        '
        If (DtFacturaCabeza.Rows(0).Item("EsServicios") Or DtFacturaCabeza.Rows(0).Item("EsSecos")) And Not DtFacturaCabeza.Rows(0).Item("Tr") Then
            'Arma lista de Insumos, Utiliso listaRetenciones.
            For Each Row As DataRow In DtFacturaDetalle.Rows
                Dim Item2 As New ItemCuentasAsientos
                Item2.Clave = Row("Articulo")
                Item2.Importe = CalculaNeto(Row("Cantidad"), Row("Precio"))
                ListaCuentas.Add(Item2)
            Next
        Else
            '
            If ListaLotesParaAsiento.Count = 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = 202
                Item.Importe = MontoNeto
                ListaConceptos.Add(Item)
            End If
        End If
        '
        'Arma lista de Retenciones.
        Dim TotalPercepciones As Decimal = 0
        For Each Row As DataRow In DtFacturasPercepcionesB.Rows
            Item = New ItemListaConceptosAsientos
            Item.Clave = Row("Percepcion")
            Item.Importe = Row("Importe")
            TotalPercepciones = TotalPercepciones + Row("Importe")
            Item.TipoIva = 9              'Credito fiscal.
            ListaRetenciones.Add(Item)
        Next
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 204
        Item.Importe = DtFacturaCabeza.Rows(0).Item("Senia")
        If Item.Importe <> 0 Then ListaConceptos.Add(Item)
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = DtFacturaCabeza.Rows(0).Item("Importe") + DiferenciaDescuento + TotalPercepciones
        ListaConceptos.Add(Item)

        If Not Asiento(TipoAsiento, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Fecha, 0) Then Return False

        If DtAsientoDetalle.Rows.Count = 0 Then
            '    MsgBox("No se pudo Generar Asiento. Operación se CANCELA.")
            '    Return False
        End If

        Return True

    End Function
    Private Shared Function ArmaArchivosAsientoCostoFacturaNueva(ByVal Funcion As String, ByVal TipoAsientoCosto As Integer, ByVal DtFacturaCabeza As DataTable, ByVal DtFacturaDetalle As DataTable, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable, ByVal Porcentaje As Double, ByVal Fecha As Date) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)

        Dim ImporteTotal As Decimal
        Dim Item As New ItemListaConceptosAsientos

        For Each Row As DataRow In DtFacturaDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Dim Kilos As Double
                Dim Iva As Double
                HallaKilosIva(Row("Articulo"), Kilos, Iva)
                ImporteTotal = ImporteTotal + Trunca(Refe * Row("Cantidad") * Kilos * Porcentaje / 100)
            End If
        Next
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 202
        Item.Importe = ImporteTotal
        ListaConceptos.Add(Item)

        If Not Asiento(TipoAsientoCosto, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Fecha, 0) Then Return False

        Return True

    End Function
    Private Shared Function ArmaArchivosAsientoCostoExteriorNueva(ByVal Funcion As String, ByVal TipoAsientoCosto As Integer, ByVal DtFacturaDetalle As DataTable, ByVal DtFacturaDetalleR As DataTable, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable, ByVal Fecha As Date) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ScomerV01.ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ScomerV01.ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ScomerV01.ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ScomerV01.ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ScomerV01.ItemListaConceptosAsientos)

        Dim ImporteTotal As Decimal = 0
        Dim RowsBusqueda() As DataRow
        Dim Item As New ScomerV01.ItemListaConceptosAsientos

        For Each Row As DataRow In DtFacturaDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Dim Kilos As Decimal = 0
                Dim Iva As Decimal = 0
                HallaKilosIva(Row("Articulo"), Kilos, Iva)
                ' 
                Dim Porcentaje As Decimal = 0
                Dim Total As Decimal = 0
                If DtFacturaDetalleR.Rows.Count <> 0 Then
                    RowsBusqueda = DtFacturaDetalleR.Select("Indice = " & Row("Indice"))
                    Total = Row("TotalArticulo") + RowsBusqueda(0).Item("TotalArticulo")
                Else
                    Total = Row("TotalArticulo")
                End If
                Porcentaje = Row("TotalArticulo") * 100 / Total
                ImporteTotal = ImporteTotal + Trunca(Refe * Row("Cantidad") * Kilos * Porcentaje / 100)
                ImporteTotal = Trunca(ImporteTotal)
            End If
        Next

        Item = New ScomerV01.ItemListaConceptosAsientos
        Item.Clave = 202
        Item.Importe = ImporteTotal
        ListaConceptos.Add(Item)

        If Not Asiento(TipoAsientoCosto, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Fecha, 0) Then Return False

        Return True

    End Function
End Class
