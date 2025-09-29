Imports System.Transactions
Imports System.Drawing.Printing
Imports System.Math
Public Class UnaLiquidacion
    Public PLiquidacion As Double
    Public PAbierto As Boolean
    Public PListaDeLotes As List(Of FilaLiquidacion)
    Public PProveedor As Integer
    Public PDirecto As Decimal
    Public PComision As Decimal
    Public PIvaComision As Decimal
    Public PMontoComision As Decimal
    Public PDescarga As Decimal
    Public PNeto As Decimal
    Public PBruto As Decimal
    Public PSenia As Decimal
    Public PFactura As Double
    Public PActualizacionOk As Boolean
    Public PBloqueaFunciones As Boolean
    Public PEsReventa As Boolean
    Public PEsConsignacion As Boolean
    Public PEsTr As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim DtGrid1 As DataTable
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtDetalleConceptos As DataTable
    '
    Dim ListaDeRetenciones As New List(Of ItemIvaReten)
    Dim TablaIva(0) As Double
    Dim PromedioIva As Decimal = 0
    Dim UltimoNumero As Double
    Dim ConexionLiquidacion As String
    Dim ConexionRelacionada As String
    Dim Relacionada As Double
    Dim UltimaFechaW As DateTime
    Dim UltimaFechaContableW As DateTime
    Dim TipoFactura As Integer
    Dim DocumentoAsiento As Integer
    Dim EsPreliquidacion As Boolean
    Dim RetencionManual As Boolean
    Dim Provincia As Integer
    Dim Calle As String
    Dim Numero As Integer
    Dim Localidad As String
    'para impresion.
    Dim Paginas As Integer
    Dim ErrorImpresion As Boolean
    Dim Copias As Integer
    Dim CopiasSegunPuntoVenta As Integer = 0
    Dim UltimoPuntoVentaParaCopiaSeleccionado As Integer = 0
    Private Sub UnaLiquidacion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(2) Then PBloqueaFunciones = True

        If PLiquidacion <> 0 Then
            If EsInsumos(PLiquidacion, PAbierto) Then
                UnaLiquidacionInsumos.PLiquidacion = PLiquidacion
                UnaLiquidacionInsumos.PAbierto = PAbierto
                UnaLiquidacionInsumos.PBloqueaFunciones = PBloqueaFunciones
                UnaLiquidacionInsumos.ShowDialog()
                PActualizacionOk = GModificacionOk
                Me.Close()
                Exit Sub
            End If
        End If

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        ComboProveedor.DataSource = ProveedoresDeFrutas()
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = PProveedor

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        LlenaComboTablas(ComboPais, 28)

        LlenaCombosGrid()
        LlenaCombosGrid1()

        ArmaTablaIva(TablaIva)
        ArmaTipoIva(ComboTipoIva)

        PActualizacionOk = False

        CreaDtGrid()
        CreaDtGrid1()

        If Not MuestraDatos() Then Me.Close() : Exit Sub

        UltimaFechaW = UltimaFecha(Conexion)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

        UltimaFechaContableW = UltimaFechacontableLiquidacion(Conexion, GPuntoDeVenta, TipoFactura)
        If UltimaFechaContableW = "2/1/1000" Then
            MsgBox("Error Base de Datos al Leer Ultima Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PLiquidacion <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Row As DataRow
        Dim DtCabezaN As New DataTable
        Dim DtdetalleN As New DataTable
        Dim DtdetalleConceptosN As New DataTable
        Dim DtCabezaB As New DataTable
        Dim DtdetalleB As New DataTable
        Dim DtdetalleConceptosB As New DataTable
        Dim DtFactura As New DataTable

        DtCabezaB = DtCabeza.Clone
        DtdetalleB = DtDetalle.Clone
        DtdetalleConceptosB = DtDetalleConceptos.Clone
        DtCabezaN = DtCabeza.Clone
        DtdetalleN = DtDetalle.Clone
        DtdetalleConceptosN = DtDetalleConceptos.Clone

        If PFactura <> 0 Then
            If Not Tablas.Read("SELECT * FROM FacturasProveedorCabeza WHERE Factura = " & PFactura & ";", Conexion, DtFactura) Then Me.Close() : Exit Sub
            If DtFactura.Rows.Count = 0 Then
                MsgBox("Factura A Reemplazar No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close() : Exit Sub
            End If
        End If

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtGrid1.Select("Concepto = 1")
        Dim BrutoB As Decimal = RowsBusqueda(0).Item("Importe")
        RowsBusqueda = DtGrid1.Select("Concepto = 3")
        Dim ComisionB As Decimal = -RowsBusqueda(0).Item("Importe")
        Dim PorComisionB As Decimal = RowsBusqueda(0).Item("Valor")
        RowsBusqueda = DtGrid1.Select("Concepto = 4")
        Dim DescargaB As Decimal = -RowsBusqueda(0).Item("Importe")
        RowsBusqueda = DtGrid1.Select("Concepto = 11")
        Dim NetoB As Decimal = RowsBusqueda(0).Item("Importe")
        RowsBusqueda = DtGrid1.Select("Concepto = 5")
        Dim AlicuotaComisionB As Decimal = RowsBusqueda(0).Item("Valor")
        RowsBusqueda = DtGrid1.Select("Concepto = 6")
        Dim AlicuotaDescargaB As Decimal = RowsBusqueda(0).Item("Valor")
        RowsBusqueda = DtGrid1.Select("Concepto = 7")
        Dim SeniaB As Decimal = RowsBusqueda(0).Item("Importe")

        ' para completar el Neto sin iva.
        Dim IndiceSinIva As Decimal = BrutoB - ComisionB - DescargaB
        If BrutoB <> 0 Then IndiceSinIva = IndiceSinIva / BrutoB
        ' para completar el Neto con iva.
        Dim IndiceConIva As Decimal = NetoB
        If BrutoB <> 0 Then IndiceConIva = IndiceConIva / BrutoB

        If PDirecto > 0 Then
            Row = DtCabezaB.NewRow
            Row("Proveedor") = PProveedor
            If PDirecto = 100 Then
                Row("Rel") = False
            Else
                Row("Rel") = True
            End If
            Row("Tr") = PEsTr
            Row("Nrel") = 0
            Row("EsReventa") = DtCabeza.Rows(0).Item("EsReventa")
            Row("EsConsignacion") = DtCabeza.Rows(0).Item("EsConsignacion")
            Row("EsInsumos") = False
            Row("Fecha") = DateTime1.Value
            Row("FechaContable") = CDate(TextFechaContable.Text)
            Row("Proveedor") = PProveedor
            Row("Bruto") = BrutoB
            Row("Alicuota") = PromedioIva
            Row("Comision") = PorComisionB
            Row("AlicuotaComision") = AlicuotaComisionB
            Row("Descarga") = DescargaB
            Row("AlicuotaDescarga") = AlicuotaDescargaB
            Row("Directo") = PDirecto
            Row("Factura") = PFactura
            Row("Neto") = NetoB
            Row("Saldo") = NetoB + SeniaB
            Row("Estado") = 1
            Row("Impreso") = False
            Row("Comentario") = TextComentario.Text
            Row("Senia") = SeniaB
            Row("Importe") = NetoB + SeniaB
            DtCabezaB.Rows.Add(Row)
            For Each Row1 As DataRow In DtGrid.Rows
                Row = DtdetalleB.NewRow
                Row("Lote") = Row1("Lote")
                Row("Secuencia") = Row1("Secuencia")
                Row("Operacion") = Row1("Operacion")
                Row("Alicuota") = Row1("Alicuota")
                Row("Merma") = Row1("Merma")
                Row("Cantidad") = Row1("Cantidad")
                Row("PrecioS") = Row1("PrecioS")
                Row("Precio") = Row1("PrecioF")
                Row("Importe") = Row1("Total")
                Row("NetoConIva") = Row("Importe") * IndiceConIva
                Row("NetoSinIva") = Row("Importe") * IndiceSinIva
                Row("RemitoGuia") = 0
                DtdetalleB.Rows.Add(Row)
            Next
            Dim Item As Integer = 0
            For Each Row1 As DataRow In DtGrid1.Rows
                Row = DtdetalleConceptosB.NewRow
                Item = Item + 1
                Row("Item") = Item
                Row("Concepto") = Row1("Concepto")
                Row("Valor") = Row1("Valor")
                Row("Importe") = Row1("Importe")
                If Row("Importe") < 0 And Row("Concepto") <> 7 Then Row("Importe") = -Row("Importe")
                If Row("Importe") <> 0 Then DtdetalleConceptosB.Rows.Add(Row)
            Next
        End If

        Dim Neto As Decimal = Trunca(PNeto - PNeto * PDirecto / 100)
        Dim Descarga As Decimal = PDescarga
        If DtCabezaB.Rows.Count <> 0 Then Descarga = Trunca(PDescarga - DtCabezaB.Rows(0).Item("Descarga"))
        Dim ComisionWW As Decimal = PMontoComision
        If DtCabezaB.Rows.Count <> 0 Then ComisionWW = Trunca(ComisionWW - DtCabezaB.Rows(0).Item("Comision") * DtCabezaB.Rows(0).Item("Bruto") / 100)
        Dim Bruto As Decimal = Neto + Descarga + ComisionWW
        Dim Comision As Decimal = PComision
        If DtCabezaB.Rows.Count <> 0 And Bruto <> 0 Then Comision = ComisionWW * 100 / Bruto
        Dim Senia As Decimal = PSenia
        If DtCabezaB.Rows.Count <> 0 Then Senia = PSenia - DtCabezaB.Rows(0).Item("Senia")

        Dim IndiceDeCorreccionN As Decimal = 1

        If DtCabezaB.Rows.Count <> 0 Then
            IndiceDeCorreccionN = (Neto + ComisionWW + Descarga) / PBruto
        End If

        ' para completar el Neto sin iva.
        IndiceSinIva = Neto
        If Bruto <> 0 Then IndiceSinIva = IndiceSinIva / Bruto

        If PDirecto <> 100 Then
            Row = DtCabezaN.NewRow
            Row("Proveedor") = PProveedor
            If PDirecto <> 0 Then
                Row("Rel") = True
            Else
                Row("Rel") = False
            End If
            Row("Tr") = PEsTr
            Row("Nrel") = 0
            Row("EsReventa") = DtCabeza.Rows(0).Item("EsReventa")
            Row("EsConsignacion") = DtCabeza.Rows(0).Item("EsConsignacion")
            Row("EsInsumos") = False
            Row("Fecha") = DateTime1.Value
            Row("FechaContable") = CDate(TextFechaContable.Text)
            Row("Proveedor") = PProveedor
            Row("Bruto") = Bruto
            Row("Alicuota") = 0
            Row("Comision") = Comision
            Row("AlicuotaComision") = 0
            Row("Descarga") = Descarga
            Row("AlicuotaDescarga") = 0
            Row("Directo") = PDirecto
            Row("Factura") = 0
            Row("Neto") = Neto
            Row("Saldo") = Neto + Senia
            Row("Estado") = 1
            Row("Impreso") = False
            Row("Comentario") = TextComentario.Text
            Row("Senia") = Senia
            Row("Importe") = Neto + Senia
            DtCabezaN.Rows.Add(Row)
            For Each Fila As FilaLiquidacion In PListaDeLotes
                Row = DtdetalleN.NewRow
                Row("Lote") = Fila.Lote
                Row("Secuencia") = Fila.Secuencia
                Row("Operacion") = Fila.Operacion
                Row("Alicuota") = 0
                Row("Cantidad") = Fila.Aliquidar
                Row("Merma") = Fila.Merma
                Row("PrecioS") = Fila.PrecioF
                Row("Precio") = Fila.PrecioF * IndiceDeCorreccionN
                Row("Importe") = CalculaNeto(Row("Cantidad"), Row("Precio"))
                Row("NetoConIva") = Row("Importe") * IndiceSinIva
                Row("NetoSinIva") = Row("Importe") * IndiceSinIva
                Row("RemitoGuia") = 0
                DtdetalleN.Rows.Add(Row)
            Next
            Dim Item As Integer = 0
            Row = DtdetalleConceptosN.NewRow
            Item = Item + 1
            Row("Item") = Item
            Row("Concepto") = 1
            Row("Valor") = 0
            Row("Importe") = Bruto
            DtdetalleConceptosN.Rows.Add(Row)
            If Comision <> 0 Then
                Row = DtdetalleConceptosN.NewRow
                Item = Item + 1
                Row("Item") = Item
                Row("Concepto") = 3
                Row("Valor") = Comision
                Row("Importe") = Trunca(Comision / 100 * Bruto)
                DtdetalleConceptosN.Rows.Add(Row)
            End If
            If Descarga <> 0 Then
                Row = DtdetalleConceptosN.NewRow
                Item = Item + 1
                Row("Item") = Item
                Row("Concepto") = 4
                Row("Valor") = 0
                Row("Importe") = Descarga
                DtdetalleConceptosN.Rows.Add(Row)
            End If
            Row = DtdetalleConceptosN.NewRow
            Item = Item + 1
            Row("Item") = Item
            Row("Concepto") = 11
            Row("Valor") = 0
            Row("Importe") = Neto
            DtdetalleConceptosN.Rows.Add(Row)
            '
            If Senia <> 0 Then
                Row = DtdetalleConceptosN.NewRow
                Item = Item + 1
                Row("Item") = Item
                Row("Concepto") = 7
                Row("Valor") = 0
                Row("Importe") = Senia
                DtdetalleConceptosN.Rows.Add(Row)
            End If
            Row = DtdetalleConceptosN.NewRow
            Item = Item + 1
            Row("Item") = Item
            Row("Concepto") = 100
            Row("Valor") = 0
            Row("Importe") = Neto + Senia
            DtdetalleConceptosN.Rows.Add(Row)
        End If

        'Arma Tipo Operaciones de Lotes para Asientos.
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable
        '
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaB) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleB) Then Me.Close() : Exit Sub
            DtAsientoCabezaN = DtAsientoCabezaB.Clone
            DtAsientoDetalleN = DtAsientoDetalleB.Clone
            If DtCabezaB.Rows.Count <> 0 Then
                If Not ArmaArchivosAsiento("A", DtCabezaB, DtdetalleConceptosB, DtAsientoCabezaB, DtAsientoDetalleB) Then Me.Close() : Exit Sub
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                If Not ArmaArchivosAsiento("A", DtCabezaN, DtdetalleConceptosN, DtAsientoCabezaN, DtAsientoDetalleN) Then Me.Close() : Exit Sub
            End If
        End If

        'Graba Liquidacion.
        Dim NumeroLiquidacionN As Double = 0
        Dim NumeroAsientoB As Double = 0
        Dim NumeroAsientoN As Double = 0
        Dim NumeroW As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion para N.
            If DtCabezaN.Rows.Count <> 0 Then
                NumeroLiquidacionN = UltimaNumeracion(ConexionN)
                If NumeroLiquidacionN < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close()
                    Exit Sub
                End If
            End If
            'Actualiza numeracion Liquidacion B.
            If DtCabezaB.Rows.Count <> 0 Then
                'Cabeza.
                DtCabezaB.Rows(0).Item("Liquidacion") = UltimoNumero
                DtCabezaB.Rows(0).Item("Interno") = UltimoNumeroInternoLiquidacion(TipoFactura, Conexion)
                If DtCabezaB.Rows(0).Item("Interno") < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close()
                    Exit Sub
                End If
                DtCabezaB.Rows(0).Item("Nrel") = 0
                'Detalle.
                For Each Row1 As DataRow In DtdetalleB.Rows
                    Row1("Liquidacion") = UltimoNumero
                Next
                For Each Row1 As DataRow In DtdetalleConceptosB.Rows
                    Row1("Liquidacion") = UltimoNumero
                Next
            End If
            'Actualiza numeracion Liquidacion N.
            If DtCabezaN.Rows.Count <> 0 Then
                'Cabeza.
                DtCabezaN.Rows(0).Item("Liquidacion") = NumeroLiquidacionN
                DtCabezaN.Rows(0).Item("Interno") = UltimoNumeroInternoLiquidacion(TipoFactura, ConexionN)
                If DtCabezaN.Rows(0).Item("Interno") < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close()
                    Exit Sub
                End If
                If DtCabezaB.Rows.Count <> 0 Then
                    DtCabezaN.Rows(0).Item("Nrel") = UltimoNumero
                Else
                    DtCabezaN.Rows(0).Item("Nrel") = 0
                End If
                'Detalle.
                For Each Row1 As DataRow In DtdetalleN.Rows
                    Row1("Liquidacion") = NumeroLiquidacionN
                Next
                For Each Row1 As DataRow In DtdetalleConceptosN.Rows
                    Row1("Liquidacion") = NumeroLiquidacionN
                Next
            End If
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                NumeroAsientoB = UltimaNumeracionAsiento(Conexion)
                If NumeroAsientoB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close()
                    Exit Sub
                End If
            End If
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                NumeroAsientoN = UltimaNumeracionAsiento(ConexionN)
                If NumeroAsientoN < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close()
                    Exit Sub
                End If
            End If
            'Actualiza Asientos.
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                DtAsientoCabezaB.Rows(0).Item("Asiento") = NumeroAsientoB
                DtAsientoCabezaB.Rows(0).Item("Documento") = UltimoNumero
                For Each Row In DtAsientoDetalleB.Rows
                    Row("Asiento") = NumeroAsientoB
                Next
            End If
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                DtAsientoCabezaN.Rows(0).Item("Asiento") = NumeroAsientoN
                DtAsientoCabezaN.Rows(0).Item("Documento") = NumeroLiquidacionN
                For Each Row In DtAsientoDetalleN.Rows
                    Row("Asiento") = NumeroAsientoN
                Next
            End If
            'Actualiza Factura.
            If PFactura <> 0 Then
                DtFactura.Rows(0).Item("Liquidacion") = UltimoNumero
            End If
            '
            NumeroW = AltaLiquidacion(DtCabezaB, DtdetalleB, DtdetalleConceptosB, DtCabezaN, DtdetalleN, DtdetalleConceptosN, DtFactura, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN)
            '
            If NumeroW >= 0 Then Exit For
            If NumeroW = -2 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -10 Then
            MsgBox("Numero de Liquidación Ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = -2 Then
            MsgBox("ERROR en Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PActualizacionOk = True
            If DtCabezaB.Rows.Count <> 0 Then
                PLiquidacion = DtCabezaB.Rows(0).Item("Liquidacion")
                PAbierto = True
            Else : PLiquidacion = DtCabezaN.Rows(0).Item("Liquidacion")
                PAbierto = False
            End If
            If Not MuestraDatos() Then Me.Close() : Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PLiquidacion = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not PermisoTotal Then
            If Relacionada <> 0 Then
                MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Liquidacion ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If CierreContableCerrado(CDate(TextFechaContable.Text).Month, CDate(TextFechaContable.Text).Year) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If HallaAsignacion(10, DtCabeza.Rows(0).Item("Liquidacion"), DtCabeza.Rows(0).Item("Proveedor"), ConexionLiquidacion) > 0 Then
            MsgBox("Liquidacion imputada en cuenta corriente.  Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim DtCabezaR As New DataTable
        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT * FROM LiquidacionCabeza WHERE Liquidacion = " & Relacionada & ";", ConexionRelacionada, DtCabezaR) Then Me.Close() : Exit Sub
            If HallaAsignacion(10, Relacionada, DtCabezaR.Rows(0).Item("Proveedor"), ConexionRelacionada) > 0 Then
                MsgBox("Liquidacion imputada en cuenta corriente.  Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
            DtCabezaR.Rows(0).Item("Estado") = 3
        End If

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoCabezaRel As New DataTable
        '
        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(DocumentoAsiento, DtCabeza.Rows(0).Item("Liquidacion"), DtAsientoCabeza, ConexionLiquidacion) Then Me.Close() : Exit Sub
            If Relacionada <> 0 Then
                If Not HallaAsientosCabeza(DocumentoAsiento, Relacionada, DtAsientoCabezaRel, ConexionRelacionada) Then Me.Close() : Exit Sub
            End If
            If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3
            If DtAsientoCabezaRel.Rows.Count <> 0 Then DtAsientoCabezaRel.Rows(0).Item("Estado") = 3
        End If

        If MsgBox("Liquidacion se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim Factura As Double
        If PAbierto Then Factura = DtCabeza.Rows(0).Item("Factura")
        If Not PAbierto And Relacionada <> 0 Then
            Factura = DtCabezaR.Rows(0).Item("Factura")
        End If

        Dim DtFactura As New DataTable
        If Factura <> 0 Then
            If Not Tablas.Read("SELECT * FROM FacturasProveedorCabeza WHERE Factura = " & Factura & ";", Conexion, DtFactura) Then Me.Close() : Exit Sub
            If DtFactura.Rows.Count = 0 Then
                MsgBox("Factura Reemplazada No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
            DtFactura.Rows(0).Item("Liquidacion") = 0
        End If

        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        DtCabezaAux.Rows(0).Item("Estado") = 3

        If Not AnulaLiquidacion(DtCabezaAux, DtCabezaR, DtFactura, DtAsientoCabeza, DtAsientoCabezaRel) Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        Else
            MsgBox("Liquidación Anulada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PActualizacionOk = True
        End If

        If Not MuestraDatos() Then Me.Close() : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub CheckRetencionManual_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckRetencionManual.CheckedChanged

        If CheckRetencionManual.Checked Then
            RetencionManual = True
        Else
            RetencionManual = False
            CalculaTotales()
        End If

        PresentaGrid1Retenciones()
        Grid1.Refresh()

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        'http://vb-helper.com/howto_net_print_and_preview.html

        If PLiquidacion = 0 Then
            MsgBox("Opcion Invalida. Liquidación debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Impreso") And PAbierto And Not EsPreliquidacion Then
            If MsgBox("Liquidación Ya fue Impresa. Quiere Re-Imprimir?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        If Not PAbierto And Not EsPreliquidacion And Relacionada <> 0 Then
            MsgBox("Versión no Imprimible. Debe Ingresar a 'Ver Pre-Liquidación'. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ErrorImpresion = False
        Paginas = 0

        Dim PuntoVentaW As Integer = Val(Strings.Mid(DtCabeza.Rows(0).Item("Liquidacion"), 2, 4))
        If PAbierto And (CopiasSegunPuntoVenta = 0 Or PuntoVentaW <> UltimoPuntoVentaParaCopiaSeleccionado) Then
            UltimoPuntoVentaParaCopiaSeleccionado = PuntoVentaW
            CopiasSegunPuntoVenta = TraeCopiasComprobante(4, PuntoVentaW)
            If CopiasSegunPuntoVenta < 0 Then CopiasSegunPuntoVenta = 0 : MsgBox("Error al Leer Tabla: PuntosDeVenta. Operacion se CANCELA.", MsgBoxStyle.Critical) : Exit Sub
        End If

        If PAbierto Then
            Copias = CopiasSegunPuntoVenta
        Else
            Copias = 1
        End If

        If EsPreliquidacion Then Copias = 1

        Dim print_document As New PrintDocument
        AddHandler print_document.PrintPage, AddressOf Print_PrintPage

        print_document.Print()

        If ErrorImpresion Then Exit Sub

        If Not EsPreliquidacion Then
            If Not GrabaImpreso() Then Exit Sub
            DtCabeza.Rows(0).Item("Impreso") = True
        End If

    End Sub
    Private Sub ButtonDatosCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDatosEmisor.Click

        If ComboProveedor.SelectedValue = 0 Then Exit Sub

        UnDatosEmisor.PEsProveedor = True

        UnDatosEmisor.PEmisor = ComboProveedor.SelectedValue
        UnDatosEmisor.ShowDialog()

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim pa As New PrintGPantalla(Me)
        pa.Print()

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PLiquidacion = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = DocumentoAsiento
        If PAbierto Then
            ListaAsientos.PDocumentoB = PLiquidacion
            ListaAsientos.PDocumentoN = 0
        Else
            ListaAsientos.PDocumentoB = 0
            ListaAsientos.PDocumentoN = PLiquidacion
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonRelacionada_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRelacionada.Click

        PLiquidacion = Relacionada
        PAbierto = Not PAbierto
        Label23.Text = "Liquidación"

        If Not MuestraDatos() Then Me.Close() : Exit Sub

        EsPreliquidacion = False
        MaskedLiquidacion.Visible = True

    End Sub
    Private Sub ButtonPreLiquidacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPreLiquidacion.Click

        MuestraPreLiquidacion(False)

    End Sub
    Private Sub ButtonPreLiquidacionSinDescarga_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPreLiquidacionSinDescarga.Click

        MuestraPreLiquidacion(True)

    End Sub
    Private Sub MuestraPreLiquidacion(ByVal EsSinSenia As Boolean)

        Label23.Text = "Pre-Liquidación"

        Dim Cantidad As Decimal
        Dim Bruto As Decimal
        Dim Senia As Decimal
        Dim RowGrid As DataRow
        '
        Dim BrutoR As Decimal
        Dim ComisionR As Decimal
        Dim DescargaR As Decimal
        Dim NetoR As Decimal
        Dim SeniaR As Decimal

        If Relacionada <> 0 Then
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT * FROM LiquidacionCabeza WHERE Liquidacion = " & Relacionada & ";", ConexionRelacionada, Dt) Then Exit Sub
            BrutoR = Dt.Rows(0).Item("Bruto")
            ComisionR = Dt.Rows(0).Item("Comision")
            DescargaR = Dt.Rows(0).Item("Descarga")
            NetoR = Dt.Rows(0).Item("Neto")
            SeniaR = Dt.Rows(0).Item("Senia")
            Dt.Dispose()
        End If

        Dim ImporteComisionW As Decimal = Trunca(DtCabeza.Rows(0).Item("Bruto") * DtCabeza.Rows(0).Item("Comision") / 100 + BrutoR * ComisionR / 100)
        Dim ImporteDescargaW As Decimal = DtCabeza.Rows(0).Item("Descarga") + DescargaR
        Dim NetoAnterior As Decimal = 0
        Dim SeniaAnterior As Decimal = 0
        Dim Neto As Decimal = 0
        If ConexionLiquidacion = Conexion Then
            NetoAnterior = DtCabeza.Rows(0).Item("Neto")
            SeniaAnterior = DtCabeza.Rows(0).Item("Senia")
            Neto = NetoR
        Else : NetoAnterior = NetoR
            Neto = DtCabeza.Rows(0).Item("Neto")
            SeniaAnterior = SeniaR
        End If

        '----------Caso que no se quiera mostrar Descarga--------------------
        Dim CantidadWW As Decimal
        Dim DescuentoDescarga As Decimal = 0
        If EsSinSenia Then
            For Each Fila As FilaLiquidacion In PListaDeLotes
                CantidadWW = CantidadWW + Fila.Aliquidar
            Next
            DescuentoDescarga = ImporteDescargaW / CantidadWW
            ImporteDescargaW = 0
        End If
        '--------------------------------------------------------------------

        Bruto = DtCabeza.Rows(0).Item("Neto") + NetoR + ImporteComisionW + ImporteDescargaW
        Senia = DtCabeza.Rows(0).Item("Senia") + SeniaR

        Dim ComisionWW As Decimal = ImporteComisionW * 100 / Bruto

        DtGrid.Clear()

        For Each Fila As FilaLiquidacion In PListaDeLotes
            RowGrid = DtGrid.NewRow()
            RowGrid("Operacion") = Fila.Operacion
            RowGrid("Lote") = Fila.Lote
            RowGrid("Secuencia") = Fila.Secuencia
            RowGrid("RemitoGuia") = Fila.RemitoGuia
            RowGrid("Articulo") = Fila.Articulo
            RowGrid("Ingresados") = Fila.Iniciales
            RowGrid("FechaIngreso") = Fila.Fecha
            RowGrid("Merma") = Fila.Merma
            RowGrid("Bruto") = Fila.Importe
            RowGrid("Cantidad") = Fila.Aliquidar
            RowGrid("PrecioF") = Fila.PrecioS - DescuentoDescarga
            RowGrid("Total") = CalculaNeto(RowGrid("PrecioF"), RowGrid("Cantidad"))
            Cantidad = Cantidad + RowGrid("Cantidad")
            DtGrid.Rows.Add(RowGrid)
        Next

        DtGrid1.Clear()

        Dim Row As DataRow = DtGrid1.NewRow
        Row("Tipo") = 0
        Row("Concepto") = 1
        Row("Valor") = 0
        Row("Importe") = Bruto
        DtGrid1.Rows.Add(Row)
        '
        Row = DtGrid1.NewRow
        Row("Tipo") = 0
        Row("Concepto") = 3
        Row("Valor") = ComisionWW
        Row("Importe") = -ImporteComisionW
        If ImporteComisionW <> 0 Then DtGrid1.Rows.Add(Row)
        '
        Row = DtGrid1.NewRow
        Row("Tipo") = 0
        Row("Concepto") = 4
        Row("Valor") = 0
        Row("Importe") = -ImporteDescargaW
        If ImporteDescargaW <> 0 Then DtGrid1.Rows.Add(Row)
        '
        Row = DtGrid1.NewRow
        Row("Tipo") = 0
        Row("Concepto") = 12
        Row("Valor") = 0
        Row("Importe") = Bruto - ImporteComisionW - ImporteDescargaW
        If NetoAnterior <> 0 Then DtGrid1.Rows.Add(Row)
        '
        Row = DtGrid1.NewRow
        Row("Tipo") = 0
        Row("Concepto") = 10
        Row("Valor") = 0
        Row("Importe") = -NetoAnterior
        If NetoAnterior <> 0 Then DtGrid1.Rows.Add(Row)
        '
        Row = DtGrid1.NewRow
        Row("Tipo") = 0
        Row("Concepto") = 11
        Row("Valor") = 0
        Row("Importe") = Neto
        DtGrid1.Rows.Add(Row)
        '
        Row = DtGrid1.NewRow
        Row("Tipo") = 0
        Row("Concepto") = 7
        Row("Valor") = 0
        Row("Importe") = Senia
        If Senia <> 0 Then DtGrid1.Rows.Add(Row)
        '
        Row = DtGrid1.NewRow
        Row("Tipo") = 0
        Row("Concepto") = 8
        Row("Valor") = 0
        Row("Importe") = -SeniaAnterior
        If SeniaAnterior <> 0 Then DtGrid1.Rows.Add(Row)
        '
        Row = DtGrid1.NewRow
        Row("Tipo") = 0
        Row("Concepto") = 100
        Row("Valor") = 0
        Row("Importe") = Neto + Senia - SeniaAnterior
        DtGrid1.Rows.Add(Row)

        PresentaGrid1(True)

        EsPreliquidacion = True
        MaskedLiquidacion.Visible = False

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueContable.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub ButtonNetoPorLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNetoPorLotes.Click

        If PLiquidacion = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        SeleccionarVarios.PEsNetoPorLotes = True
        SeleccionarVarios.PLiquidacion = PLiquidacion
        SeleccionarVarios.PAbierto = PAbierto
        SeleccionarVarios.ShowDialog()
        SeleccionarVarios.Dispose()

    End Sub
    Private Function MuestraDatos() As Boolean           'MuestraDatos

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PAbierto Then
            ConexionLiquidacion = Conexion
            ConexionRelacionada = ConexionN
        Else : ConexionLiquidacion = ConexionN
            ConexionRelacionada = Conexion
        End If

        If PermisoTotal Then
            If PAbierto Then
                PictureCandado.Image = ImageList1.Images.Item("Abierto")
            Else : PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            End If
        End If

        DtCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM LiquidacionCabeza WHERE Liquidacion = " & PLiquidacion & ";", ConexionLiquidacion, DtCabeza) Then Return False
        If PLiquidacion <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Liquidacion No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PLiquidacion <> 0 Then
            PProveedor = DtCabeza.Rows(0).Item("Proveedor")
        End If

        If Not LlenaDatosEmisor(PProveedor) Then Return False

        If PLiquidacion = 0 Then  'debe ir antes de AgregarCabeza.
            TipoFactura = LetrasPermitidasProveedor(HallaTipoIvaProveedor(PProveedor), 100)
            TextTipoFactura.Text = LetraTipoIva(TipoFactura)
            GPuntoDeVenta = HallaPuntoVentaSegunTipo(910, TipoFactura)
            If GPuntoDeVenta = 0 Then
                MsgBox("Usuario No tiene definido Punto de Venta para el Tipo de Iva de este Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Not ExistePuntoDeVenta(GPuntoDeVenta) Then
                MsgBox("Punto de Venta " & GPuntoDeVenta & " No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If EsPuntoDeVentaZ(GPuntoDeVenta) Then
                MsgBox("Punto de Venta del Operador " & GPuntoDeVenta & " Reservado Para Ticket Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            LabelPuntodeVenta.Text = "Punto de Venta " & Format(GPuntoDeVenta, "0000")
        End If

        If PLiquidacion = 0 Then
            If Not AgregaCabeza() Then Return False
        End If

        If DtCabeza.Rows(0).Item("Factura") <> 0 Then
            DocumentoAsiento = 911
        Else
            If DtCabeza.Rows(0).Item("EsReventa") Then
                DocumentoAsiento = 910
            Else
                DocumentoAsiento = 912
            End If
        End If

        EnlazaCabeza()

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM LiquidacionDetalle WHERE Liquidacion = " & PLiquidacion & ";", ConexionLiquidacion, DtDetalle) Then Return False

        DtDetalleConceptos = New DataTable
        If Not Tablas.Read("SELECT * FROM LiquidacionDetalleConceptos WHERE Liquidacion = " & PLiquidacion & ";", ConexionLiquidacion, DtDetalleConceptos) Then Return False

        Relacionada = 0
        If Not PAbierto Then Relacionada = DtCabeza.Rows(0).Item("Nrel")
        If PAbierto And DtCabeza.Rows(0).Item("Rel") Then
            Relacionada = HallaRelacionada(DtCabeza.Rows(0).Item("Liquidacion"))
            If Relacionada < 0 Then Return False
        End If

        If Relacionada = 0 Then
            ButtonPreLiquidacion.Visible = False
            ButtonPreLiquidacionSinDescarga.Visible = False
        Else
            If Not PermisoTotal Then
                ButtonPreLiquidacion.Visible = False
                ButtonPreLiquidacionSinDescarga.Visible = False
            Else
                ButtonPreLiquidacion.Visible = True
                ButtonPreLiquidacionSinDescarga.Visible = True
            End If
        End If

        Dim Row As DataRow

        DtGrid1.Clear()
        DtGrid.Clear()

        If PLiquidacion = 0 Then
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 1
            Row("Valor") = 0
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 2
            Row("Valor") = 0
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 3
            Row("Valor") = PComision
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 4
            Row("Valor") = 0
            Row("Importe") = PDescarga
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 5
            Row("Valor") = PIvaComision      'AgregaIvaComision(PDirecto)
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 6
            Row("Valor") = AgregaIvaDescarga(PDirecto)
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            If PDirecto <> 0 Then AgregaRetenciones()
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 10
            Row("Valor") = 0
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 11
            Row("Valor") = 0
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 7
            Row("Valor") = 0
            Row("Importe") = PSenia
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 100
            Row("Valor") = 0
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            If PDirecto = 0 Then
                If PermisoTotal Then PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            Else
                If PermisoTotal Then PictureCandado.Image = ImageList1.Images.Item("Abierto")
            End If
            CompletaConFechaDeIngresoLote(PListaDeLotes)
            ButtonRelacionada.Visible = False
            CalculaTotales()
        Else
            PListaDeLotes = New List(Of FilaLiquidacion)
            Dim Fila As FilaLiquidacion
            For Each Row In DtDetalle.Rows
                Fila = New FilaLiquidacion
                Fila.Lote = Row("Lote")
                Fila.Secuencia = Row("Secuencia")
                Fila.Operacion = Row("Operacion")
                Fila.Alicuota = Row("Alicuota")
                Dim Deposito As Integer = HallaDepositoLote(Fila.Lote, Fila.Secuencia, Fila.Operacion)
                If Deposito < 0 Then
                    MsgBox("Error en Tabla IngresoMercaderiasCabeza para el Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                    Me.Close() : Exit Function
                End If
                If Not HallaDatosLote(Row("Lote"), Row("Secuencia"), Row("Operacion"), Deposito, Fila.Iniciales, Fila.Articulo, Fila.PrecioS, Fila.RemitoGuia) Then Return False
                Fila.Merma = Row("Merma")
                Fila.PrecioF = Row("Precio")
                Fila.Aliquidar = Row("Cantidad")
                Fila.PrecioS = Row("PrecioS")
                'Muestra Permiso de Importacion.
                Fila.PermisoImp = HallaPermisoImp(Fila.Operacion, Fila.Lote, Fila.Secuencia, Deposito)
                If Fila.PermisoImp = "-1" Then
                    MsgBox("Error, Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " No Encontrado.")
                    Me.Close() : Exit Function
                End If
                Dim AGranel As Boolean
                HallaAGranelYMedida(Fila.Articulo, AGranel, Fila.Medida)
                PListaDeLotes.Add(Fila)
            Next
            CompletaConFechaDeIngresoLote(PListaDeLotes)
            For Each Row1 As DataRow In DtDetalleConceptos.Rows
                Dim Row2 As DataRow = DtGrid1.NewRow
                Row2("Tipo") = HallaCodigoRetencion(Row1("Concepto"))
                Row2("Concepto") = Row1("Concepto")
                Row2("Valor") = Row1("Valor")
                Row2("Importe") = Row1("Importe")
                Select Case Row2("Concepto")
                    Case 1, 2, 11, 7, 100
                    Case Else
                        Row2("Importe") = -Row2("Importe")
                End Select
                DtGrid1.Rows.Add(Row2)
            Next
            MuestraTotales()
            TextFechaContable.Enabled = False
            PictureAlmanaqueContable.Enabled = False
            TextComentario.Enabled = False
            ButtonRelacionada.Visible = True
        End If

        If Relacionada = 0 Or Not PermisoTotal Then
            ButtonRelacionada.Visible = False
        Else : ButtonRelacionada.Visible = True
        End If

        Grid.DataSource = DtGrid
        Grid1.DataSource = DtGrid1

        If PLiquidacion = 0 Then
            If PDirecto = 0 Or PDirecto = 100 Then
                PresentaGrid1(True)
            Else
                PresentaGrid1(False)
            End If
        Else
            PresentaGrid1(True)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub EnlazaCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Row As DataRowView = MiEnlazador.Current

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Liquidacion")
        AddHandler Enlace.Format, AddressOf Formatliquidacion
        AddHandler Enlace.Parse, AddressOf FormatParseLiquidacion
        MaskedLiquidacion.DataBindings.Clear()
        MaskedLiquidacion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Proveedor")
        ComboProveedor.DataBindings.Clear()
        ComboProveedor.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Factura")
        AddHandler Enlace.Format, AddressOf FormatFactura
        TextFactura.DataBindings.Clear()
        TextFactura.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Parse, AddressOf FormatParseString
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        If Row("FechaContable") = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Row("FechaContable"), "dd/MM/yyyy")
        End If

    End Sub
    Private Sub Formatliquidacion(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        TipoFactura = Strings.Left(Numero.Value, 1)
        Dim PuntoDeVenta = Strings.Mid(Numero.Value, 2, 4)
        TextTipoFactura.Text = LetraTipoIva(Strings.Left(Numero.Value, 1))
        Numero.Value = Strings.Right(Numero.Value, 12)
        LabelPuntodeVenta.Text = "Punto de Venta " & PuntoDeVenta

    End Sub
    Private Sub FormatParseLiquidacion(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = CDbl(TipoFactura & Format(Val(MaskedLiquidacion.Text), "000000000000"))

    End Sub
    Private Sub FormatParseString(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Sub FormatFactura(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = NumeroEditado(Numero.Value)
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If IsDBNull(Numero.Value) Then Numero.Value = 0
        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Function AgregaCabeza() As Boolean

        Dim Row As DataRow

        UltimoNumero = UltimaNumeracionLiquidacion(TipoFactura, GPuntoDeVenta)
        If UltimoNumero < 0 Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Row = DtCabeza.NewRow()
        ArmaNuevaLiquidacion(Row)
        Row("Liquidacion") = UltimoNumero
        Row("EsReventa") = PEsReventa
        If PFactura <> 0 Then Row("EsReventa") = True
        Row("EsConsignacion") = PEsConsignacion
        Row("Tr") = PEsTr
        Row("Proveedor") = PProveedor
        Row("Fecha") = DateTime1.Value
        Row("Factura") = PFactura

        DtCabeza.Rows.Add(Row)

        Return True

    End Function
    Private Sub CompletaConFechaDeIngresoLote(ByVal ListaLotes As List(Of FilaLiquidacion))

        Dim ConexionStr As String

        For Each Fila As FilaLiquidacion In ListaLotes
            If Fila.Operacion = 1 Then
                ConexionStr = Conexion
            Else : ConexionStr = ConexionN
            End If
            Fila.Fecha = HallaFechaIngreso(Fila.Lote, Fila.Secuencia, ConexionStr)
        Next

    End Sub
    Public Function HallaFechaIngreso(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal ConexionStr As String) As Date

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Fecha FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return "01/01/1800"
        End Try

    End Function
    Private Function AltaLiquidacion(ByVal DtCabezaB As DataTable, ByVal DtDetalleB As DataTable, ByVal DtDetalleConceptosB As DataTable, ByVal DtCabezaN As DataTable, ByVal DtDetalleN As DataTable, ByVal DtDetalleConceptosN As DataTable, ByVal DtFacturaW As DataTable, ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtAsientoCabezaN As DataTable, ByVal DtAsientoDetalleN As DataTable) As Double

        'CUIDADO: en GrabaTabla siempre poner getChange de la tabla para que tome los cambio cuando pase por segunda ves.

        Dim NumeroW As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaB.GetChanges) Then
                    If Not ReGrabaUltimaNumeracionLiquidacion(DtCabezaB.Rows(0).Item("Liquidacion"), TipoFactura) Then Return -10
                    NumeroW = GrabaTabla(DtCabezaB.GetChanges, "LiquidacionCabeza", Conexion)
                    If NumeroW <= 0 Then Return NumeroW
                    NumeroW = GrabaTabla(DtDetalleB.GetChanges, "LiquidacionDetalle", Conexion)
                    If NumeroW <= 0 Then Return NumeroW
                    NumeroW = GrabaTabla(DtDetalleConceptosB.GetChanges, "LiquidacionDetalleConceptos", Conexion)
                    If NumeroW <= 0 Then Return NumeroW
                    For Each Row As DataRow In DtDetalleB.GetChanges.Rows
                        If Not GrabaLiquidadoYPrecioEnLote("A", Row("Lote"), Row("Secuencia"), Row("Operacion"), DtCabezaB.Rows(0).Item("Liquidacion")) Then Return -2
                    Next
                    If DtFacturaW.Rows.Count <> 0 Then
                        NumeroW = GrabaTabla(DtFacturaW.GetChanges, "FacturasProveedorCabeza", Conexion)
                        If NumeroW <= 0 Then Return 0
                    End If
                End If
                '
                If Not IsNothing(DtCabezaN.GetChanges) Then
                    NumeroW = GrabaTabla(DtCabezaN.GetChanges, "LiquidacionCabeza", ConexionN)
                    If NumeroW <= 0 Then Return NumeroW
                    NumeroW = GrabaTabla(DtDetalleN.GetChanges, "LiquidacionDetalle", ConexionN)
                    If NumeroW <= 0 Then Return NumeroW
                    NumeroW = GrabaTabla(DtDetalleConceptosN.GetChanges, "LiquidacionDetalleConceptos", ConexionN)
                    If NumeroW <= 0 Then Return NumeroW
                    If DtCabezaB.Rows.Count = 0 Then
                        For Each Row As DataRow In DtDetalleN.Rows
                            If Not GrabaLiquidadoYPrecioEnLote("A", Row("Lote"), Row("Secuencia"), Row("Operacion"), DtCabezaN.Rows(0).Item("Liquidacion")) Then Return -2
                        Next
                    End If
                End If
                '
                ' graba Asiento B.
                If DtAsientoCabezaB.Rows.Count <> 0 Then
                    NumeroW = GrabaTabla(DtAsientoCabezaB.GetChanges, "AsientosCabeza", Conexion)
                    If NumeroW <= 0 Then Return NumeroW
                    NumeroW = GrabaTabla(DtAsientoDetalleB.GetChanges, "AsientosDetalle", Conexion)
                    If NumeroW <= 0 Then Return NumeroW
                End If
                ' graba Asiento N.
                If DtAsientoCabezaN.Rows.Count <> 0 Then
                    NumeroW = GrabaTabla(DtAsientoCabezaN.GetChanges, "AsientosCabeza", ConexionN)
                    If NumeroW <= 0 Then Return NumeroW
                    NumeroW = GrabaTabla(DtAsientoDetalleN.GetChanges, "AsientosDetalle", ConexionN)
                    If NumeroW <= 0 Then Return NumeroW
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        Finally
        End Try

    End Function
    Private Function AnulaLiquidacion(ByVal DtCabezaAux As DataTable, ByVal DtCabezaR As DataTable, ByVal DtFactura As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoCabezaRel As DataTable) As Boolean

        Dim MitransactionOptions As New TransactionOptions
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                'Graba Cabeza de la Liquidacion.
                If GrabaTabla(DtCabezaAux, "LiquidacionCabeza", ConexionLiquidacion) <= 0 Then Return False
                For Each Row As DataRow In DtDetalle.Rows
                    If Not GrabaLiquidadoYPrecioEnLote("B", Row("Lote"), Row("Secuencia"), Row("Operacion"), DtCabezaAux.Rows(0).Item("Factura")) Then Return False
                Next
                '
                If Not IsNothing(DtCabezaR.GetChanges) Then
                    If GrabaTabla(DtCabezaR, "LiquidacionCabeza", ConexionRelacionada) <= 0 Then Return False
                End If
                '
                If DtFactura.Rows.Count <> 0 Then
                    If GrabaTabla(DtFactura, "FacturasProveedorCabeza", Conexion) <= 0 Then Return False
                End If
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    If GrabaTabla(DtAsientoCabeza, "AsientosCabeza", ConexionLiquidacion) <= 0 Then Return False
                End If
                '
                If Not IsNothing(DtAsientoCabezaRel.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaRel, "AsientosCabeza", ConexionRelacionada) <= 0 Then Return False
                End If

                Scope.Complete()
                Return True
            End Using
        Catch ex As TransactionException
            Return False
        Finally
        End Try

    End Function
    Private Function GrabaLiquidadoYPrecioEnLote(ByVal Funcion As String, ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As Integer, ByVal Liquidacion As Double) As Boolean

        Dim Sql As String
        Dim Dt As New DataTable
        Dim ConexionLote As String

        If Operacion = 1 Then
            ConexionLote = Conexion
        Else : ConexionLote = ConexionN
        End If

        Sql = "SELECT * FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & " AND Lote = LoteOrigen AND Secuencia = SecuenciaOrigen AND Deposito = DepositoOrigen;"
        If Not Tablas.Read(Sql, ConexionLote, Dt) Then Return False
        If Dt.Rows.Count = 0 Then
            MsgBox("Lote " & Lote & "/" & Format(Secuencia, "000") & " No se encuentra en Lotes. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If

        If Funcion = "A" Then
            Dim PrecioLista As Decimal = HallaPrecioDeListaDePrecios(Dt.Rows(0).Item("Proveedor"), Lote, Dt.Rows(0).Item("Fecha"), Dt.Rows(0).Item("Articulo"), HallaKilosXUnidad(Dt.Rows(0).Item("Articulo")), Operacion)
            If PrecioLista > 0 Then
                If Dt.Rows(0).Item("PrecioF") = 0 Then Dt.Rows(0).Item("PrecioF") = PrecioLista : Dt.Rows(0).Item("PrecioPorLista") = True
            End If
        End If
        If Funcion = "B" Then
            If Dt.Rows(0).Item("PrecioPorLista") Then
                Dt.Rows(0).Item("PrecioF") = 0 : Dt.Rows(0).Item("PrecioPorLista") = False
            End If
        End If

        Dt.Rows(0).Item("Liquidado") = Liquidacion

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionLote)
                Miconexion.Open()
                Sql = "SELECT * FROM Lotes"
                Using DaP As New OleDb.OleDbDataAdapter(Sql, Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(Dt.GetChanges)
                End Using
            End Using
            Return True
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
            Dt.Dispose()
        End Try

    End Function
    Private Sub LLenaGrid()

        Dim RowGrid As DataRow
        DtGrid.Clear()

        For Each Fila As FilaLiquidacion In PListaDeLotes
            RowGrid = DtGrid.NewRow()
            RowGrid("Operacion") = Fila.Operacion
            RowGrid("Lote") = Fila.Lote
            RowGrid("Secuencia") = Fila.Secuencia
            RowGrid("Articulo") = Fila.Articulo
            If PLiquidacion = 0 Then
                If PDirecto <> 0 Then
                    RowGrid("Alicuota") = HallaIva(Fila.Articulo)
                Else : RowGrid("Alicuota") = 0
                End If
            Else
                RowGrid("Alicuota") = Fila.Alicuota
            End If
            RowGrid("Ingresados") = Fila.Iniciales
            RowGrid("Merma") = Fila.Merma
            RowGrid("Bruto") = Fila.Importe
            RowGrid("Cantidad") = Fila.Aliquidar
            RowGrid("PrecioS") = Fila.PrecioF
            RowGrid("PrecioF") = Fila.PrecioF
            RowGrid("Total") = CalculaNeto(Fila.PrecioF, Fila.Aliquidar)
            RowGrid("RemitoGuia") = Fila.RemitoGuia
            RowGrid("FechaIngreso") = Fila.Fecha
            RowGrid("Medida") = Fila.Medida
            DtGrid.Rows.Add(RowGrid)
        Next

    End Sub
    Private Sub CalculaTotales()                    'CalculaTotales    CalculaRetenciones.

        If PLiquidacion <> 0 Then Exit Sub

        LLenaGrid()

        Dim Unidades As Decimal = 0
        Dim BrutoB As Decimal = 0
        Dim IvaB As Decimal = 0
        Dim NetoB As Decimal = Trunca(PNeto * PDirecto / 100)

        PromedioIva = DtGrid.Rows(0).Item("Alicuota")

        Dim IndiceCorreccion As Decimal = 1

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGrid1.Select("Concepto = 3")
        Dim ComisionT As Decimal = RowsBusqueda(0).Item("Valor")
        RowsBusqueda = DtGrid1.Select("Concepto = 4")
        Dim DescargaT As Decimal = RowsBusqueda(0).Item("Importe")
        If DescargaT < 0 Then DescargaT = -DescargaT
        RowsBusqueda = DtGrid1.Select("Concepto = 5")
        Dim AlicuotaComisionT As Decimal = RowsBusqueda(0).Item("Valor")
        RowsBusqueda = DtGrid1.Select("Concepto = 6")
        Dim AlicuotaDescargaT As Decimal = RowsBusqueda(0).Item("Valor")
        RowsBusqueda = DtGrid1.Select("Concepto = 7")
        Dim SeniaT As Decimal = RowsBusqueda(0).Item("Importe")

        'Reten. Ingreso Bruto.
        Dim AlicuotaProvincia As Decimal = 0
        Dim ImpRetencionesManual As Decimal = 0

        RowsBusqueda = DtGrid1.Select("Tipo = 1")   'Tipo = 1 es Reten/Perc.  Prepara variables para calculo retenciones.
        For Each Row As DataRow In RowsBusqueda
            For Each Fila As ItemIvaReten In ListaDeRetenciones
                If Fila.Clave = Row("Concepto") Then
                    If Fila.Formula = 0 Or RetencionManual Then
                        Fila.Importe = Abs(Row("Importe"))
                        ImpRetencionesManual = ImpRetencionesManual + Fila.Importe
                    Else
                        Fila.Importe = 0
                        If Fila.Formula = 2 Then Fila.Alicuota = Row("Valor") : AlicuotaProvincia = AlicuotaProvincia + Row("Valor")
                    End If
                End If
            Next
        Next

        Dim IvaDescargaW As Decimal = Trunca(DescargaT * AlicuotaDescargaT / 100)

        Dim Fecha As Date
        Fecha = DateTime1.Value
        Dim RetencionesGanancia As Decimal = 0

        If PDirecto <> 0 Then     'Calcula retenciones.
            'se incluye AlicuotaProvincia para que calcule el Bruto con IB. Pero solo deja una sola retencion. 
            Dim BrutoW As Decimal = CalculaBruto(NetoB, PromedioIva, ComisionT, AlicuotaComisionT, AlicuotaProvincia, DescargaT, IvaDescargaW, ImpRetencionesManual)
            IndiceCorreccion = (BrutoW * 100 / PBruto) / 100
            've si existe ret/perc. a las ganancias recalcula a partir del BrutoW calculado arriva y recalcula el BrutoW.
            RetencionesGanancia = CalculaRetenFormulaLiquidacionProv1(PProveedor, BrutoW, PromedioIva, ListaDeRetenciones, RetencionManual, Fecha)
            If RetencionesGanancia > 0 Then
                ImpRetencionesManual = ImpRetencionesManual + RetencionesGanancia
                'se incluye AlicuotaProvincia para que calcule el Bruto con IB. Pero solo deja una sola retencion. 
                BrutoW = CalculaBruto(NetoB, PromedioIva, ComisionT, AlicuotaComisionT, AlicuotaProvincia, DescargaT, IvaDescargaW, ImpRetencionesManual)
                IndiceCorreccion = (BrutoW * 100 / PBruto) / 100
            End If
            CalculaRetenFormula2(BrutoW, AlicuotaProvincia, ListaDeRetenciones)
        End If

        For Each Row As DataRow In DtGrid.Rows
            Row("PrecioF") = Trunca5(Row("PrecioF") * IndiceCorreccion)
            Row("Total") = CalculaNeto(Row("Cantidad"), Row("PrecioF"))
            Unidades = Unidades + Row("Cantidad")
            BrutoB = BrutoB + Row("Total")
        Next

        TextUnidades.Text = FormatNumber(Unidades, GDecimales)
        IvaB = Trunca(BrutoB * PromedioIva / 100)

        Dim ImporteComisionW As Decimal = Trunca(BrutoB * ComisionT / 100)
        Dim IvaImporteComisionW As Decimal = Trunca(ImporteComisionW * AlicuotaComisionT / 100)

        Dim RetencionesTotal As Decimal = 0
        For Each Fila As ItemIvaReten In ListaDeRetenciones
            RetencionesTotal = RetencionesTotal + Fila.Importe
        Next

        NetoB = BrutoB + IvaB - ImporteComisionW - IvaImporteComisionW - DescargaT - IvaDescargaW - RetencionesTotal
        NetoB = Trunca(NetoB)

        For Each Row As DataRow In DtGrid1.Rows
            Select Case Row("Concepto")
                Case 1
                    Row("Importe") = BrutoB
                Case 2
                    Row("Importe") = IvaB
                    Row("Valor") = PromedioIva
                Case 3
                    Row("Importe") = -ImporteComisionW
                Case 4
                    If Row("Importe") > 0 Then Row("Importe") = -Row("Importe")
                Case 5
                    Row("Importe") = -IvaImporteComisionW
                Case 6
                    Row("Importe") = -IvaDescargaW
                Case 10
                Case 11
                    Row("Importe") = NetoB
                Case 100
                    Row("Importe") = NetoB + SeniaT
                Case Else
                    If Row("Tipo") = 1 Then
                        For Each Fila As ItemIvaReten In ListaDeRetenciones
                            If Fila.Clave = Row("Concepto") Then Row("Importe") = -Fila.Importe
                        Next
                    End If
            End Select
        Next

    End Sub
    Private Function CalculaBruto(ByVal NetoB As Decimal, ByVal PromedioIva As Decimal, ByVal ComisionT As Decimal, ByVal AlicuotaComisionT As Decimal, ByVal AlicuotaProvincia As Decimal, ByVal DescargaT As Decimal, ByVal IvaDescargaW As Decimal, ByVal RetencionGanancia As Decimal) As Decimal

        Dim IndiceCorreccion As Decimal = 1

        'IndiceCorreccion = bruto * (1 + IvaVenta - Comision - IvaComision) / bruto   
        IndiceCorreccion = 1 + PromedioIva / 100 - ComisionT / 100 - ComisionT / 100 * AlicuotaComisionT / 100 - AlicuotaProvincia / 100
        'neto = bruto * IndiceCorreccion - Descarga - IvaDescarga
        Dim BrutoW As Decimal = (NetoB + DescargaT + IvaDescargaW + RetencionGanancia) / IndiceCorreccion

        Return BrutoW

    End Function
    Private Function CalculaRetenFormula2(ByVal Bruto As Decimal, ByVal AlicuotaProvincia As Decimal, ByRef ListaReten As List(Of ItemIvaReten)) As Decimal

        Dim TotRetencion As Decimal = 0

        For Each Fila As ItemIvaReten In ListaReten
            If Fila.Formula = 2 And Not RetencionManual Then
                Fila.Importe = Trunca(Fila.Alicuota / 100 * Bruto)
                TotRetencion = TotRetencion + Fila.Importe
            End If
        Next

        Return TotRetencion

    End Function
    Private Sub AgregaRetenciones()

        ListaDeRetenciones = HallaRetencionesAplicables(10, 1, PProveedor, DateTime1.Value)
        For Each Fila As ItemIvaReten In ListaDeRetenciones
            If Fila.Formula = 2 Then
                Dim AlicuotaProvinciaW As Decimal = HallaAlicuotaRetIngBruto(Provincia)
                If AlicuotaProvinciaW < 0 Then
                    MsgBox("Error Base de Datos al Leer tabla de Provincias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close() : Exit Sub
                End If
                Dim Row As DataRow = DtGrid1.NewRow
                Row("Tipo") = 1
                Row("Concepto") = Fila.Clave
                Row("Valor") = AlicuotaProvinciaW
                Row("Importe") = 0
                DtGrid1.Rows.Add(Row)
            End If
            If Fila.Formula = 1 Then
                Dim Row As DataRow = DtGrid1.NewRow
                Row("Tipo") = 1
                Row("Concepto") = Fila.Clave
                Row("Valor") = Fila.Alicuota
                Row("Importe") = 0
                DtGrid1.Rows.Add(Row)
            End If
            If Fila.Formula = 0 Then
                Dim Row As DataRow = DtGrid1.NewRow
                Row("Tipo") = 1
                Row("Concepto") = Fila.Clave
                Row("Valor") = 0
                Row("Importe") = 0
                DtGrid1.Rows.Add(Row)
            End If
        Next

    End Sub
    Private Sub MuestraTotales()

        LLenaGrid()

        Dim Unidades As Decimal = 0

        For Each Row As DataRow In DtGrid.Rows
            Unidades = Unidades + Row("Cantidad")
        Next

        TextUnidades.Text = FormatNumber(Unidades, GDecimales)

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Ingresados As New DataColumn("Ingresados")
        Ingresados.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Ingresados)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Merma As New DataColumn("Merma")
        Merma.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Merma)

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Medida)

        Dim Iva As New DataColumn("Alicuota")
        Iva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Iva)

        Dim PrecioS As New DataColumn("PrecioS")
        PrecioS.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioS)

        Dim PrecioF As New DataColumn("PrecioF")
        PrecioF.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioF)

        Dim Bruto As New DataColumn("Bruto")
        Bruto.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Bruto)

        Dim Total As New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Total)

        Dim RemitoGuia As New DataColumn("RemitoGuia")
        RemitoGuia.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(RemitoGuia)

        Dim FechaIngreso As New DataColumn("FechaIngreso")
        FechaIngreso.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaIngreso)

    End Sub
    Private Sub CreaDtGrid1()

        DtGrid1 = New DataTable

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Tipo)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Concepto)

        Dim Valor As New DataColumn("Valor")
        Valor.DataType = System.Type.GetType("System.Decimal")
        DtGrid1.Columns.Add(Valor)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGrid1.Columns.Add(Importe)

    End Sub
    Private Function TodosLosConceptos() As DataTable

        Dim Dt As New DataTable

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        Dt.Columns.Add(Nombre)

        Try
            Dim Row As DataRow = Dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 1
            Row("Nombre") = "Venta Bruta"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 2
            Row("Nombre") = "IVA Sobre Venta"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 3
            Row("Nombre") = "Comisión"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 4
            Row("Nombre") = "Descarga"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 5
            Row("Nombre") = "IVA Comisión"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 6
            Row("Nombre") = "IVA Descarga"
            Dt.Rows.Add(Row)
            '
            If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 25;", Conexion, Dt) Then End
            '
            Row = Dt.NewRow
            Row("Clave") = 12
            Row("Nombre") = "Sub-Total"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 10
            Row("Nombre") = "Neto Anterior"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 11
            Row("Nombre") = "Total Neto"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 7
            Row("Nombre") = "Seña Por Envases"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 8
            Row("Nombre") = "Seña Anterior"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 100
            Row("Nombre") = "Total"
            Dt.Rows.Add(Row)
            '
            Return Dt
        Catch ex As Exception
            Dt.Dispose()
        End Try

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Articulo.DataSource = TodosLosArticulos()
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Sub LlenaCombosGrid1()

        Concepto.DataSource = TodosLosConceptos()
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

    End Sub
    Private Function LlenaDatosEmisor(ByVal Proveedor As Integer) As Boolean

        Dim Dta As New DataTable
        Dim Sql As String = ""

        Sql = "SELECT * FROM Proveedores WHERE Clave = " & Proveedor & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, Proveedor ya no existe o error en la Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Calle = Dta.Rows(0).Item("Calle")
        Numero = Dta.Rows(0).Item("Numero")
        Localidad = Dta.Rows(0).Item("Localidad")
        TextProvincia.Text = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        Provincia = Dta.Rows(0).Item("Provincia")
        ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
        TextCuit.Text = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")
        If Dta.Rows(0).Item("TipoIva") = 6 And PLiquidacion = 0 Then
            MsgBox("Tipo Iva MONOTRIBUTO no puede hacer liquidaciones.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Dta.Dispose()

        Return True

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Dim Patron As String = TipoFactura & Format(GPuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Liquidacion) FROM LiquidacionCabeza WHERE CAST(CAST(LiquidacionCabeza.Liquidacion AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(TipoFactura & Format(GPuntoDeVenta, "0000") & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function UltimoNumeroInternoLiquidacion(ByVal TipoFacturaW As Integer, ByVal ConexionStr As String) As Double

        Dim Patron As String = TipoFacturaW & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Interno) FROM LiquidacionCabeza WHERE CAST(CAST(LiquidacionCabeza.Interno AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(TipoFacturaW & Format(1, "000000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaRelacionada(ByVal Liquidacion As Double) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Liquidacion FROM LiquidacionCabeza WHERE Nrel = " & Liquidacion & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos.  " & ex.Message)
            Return -1
        End Try

    End Function
    Private Function UltimaNumeracionAsiento(ByVal ConexionStr) As Integer

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
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Dim Patron As String = TipoFactura & Format(GPuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM LiquidacionCabeza WHERE CAST(CAST(LiquidacionCabeza.Liquidacion AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return "1/1/1000"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "2/1/1000"
        End Try

    End Function
    Private Function HallaDatosLote(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As Integer, ByVal Deposito As Integer, ByRef Inicio As Decimal, ByRef Articulo As Integer, ByRef PrecioF As Double, ByRef RemitoGuia As Double) As Boolean

        Dim Dt As New DataTable
        Dim ConexionStr As String

        Inicio = 0 : Articulo = 0 : PrecioF = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT Cantidad,Baja,Articulo,PrecioF FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & " AND Deposito = " & Deposito & ";", ConexionStr, Dt) Then Return False
        If Dt.Rows.Count <> 0 Then
            Inicio = Dt.Rows(0).Item("Cantidad") - Dt.Rows(0).Item("Baja")
            Articulo = Dt.Rows(0).Item("Articulo")
            PrecioF = Dt.Rows(0).Item("PrecioF")
        End If

        Dt.Dispose()

        If Not HallaRemitoGuia(Lote, Operacion, RemitoGuia) Then Return False

        Return True

    End Function
    Private Function IvaExiste(ByVal Tablaiva() As Double, ByVal Alicuota As Double) As Boolean

        For Each Item As Double In Tablaiva
            If Item = Alicuota Then Return True
        Next

    End Function
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtFacturaCabeza As DataTable, ByVal DtDetalleConceptos As DataTable, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow

        Dim Comision As Decimal = Trunca(DtFacturaCabeza.Rows(0).Item("Comision") * DtFacturaCabeza.Rows(0).Item("Bruto") / 100)

        Dim Tipo As Integer
        Dim Centro As Integer
        If Grid.Rows(0).Cells("Operacion").Value = 1 Then
            HallaCentroTipoOperacion(Grid.Rows(0).Cells("Lote").Value, Grid.Rows(0).Cells("Secuencia").Value, Conexion, Tipo, Centro)
        Else
            HallaCentroTipoOperacion(Grid.Rows(0).Cells("Lote").Value, Grid.Rows(0).Cells("Secuencia").Value, ConexionN, Tipo, Centro)
        End If
        If Centro <= 0 Then
            MsgBox("Error en Tipo Operacion en Lote ")
            Return False
        End If
        '
        Dim Fila2 As New ItemLotesParaAsientos
        Fila2.TipoOperacion = Tipo
        Fila2.Centro = Centro
        Fila2.MontoNeto = DtFacturaCabeza.Rows(0).Item("Bruto")
        If Tipo = 1 Then Fila2.Clave = 301 'consignacion
        If Tipo = 2 Then Fila2.Clave = 300 'reventa
        If Fila2.MontoNeto <> 0 Then ListaLotesParaAsiento.Add(Fila2)
        '
        Fila2 = New ItemLotesParaAsientos
        Fila2.TipoOperacion = Tipo
        Fila2.Centro = Centro
        Fila2.MontoNeto = Comision
        If Tipo = 1 Then Fila2.Clave = 401
        If Tipo = 2 Then Fila2.Clave = 400
        If Fila2.MontoNeto <> 0 Then ListaLotesParaAsiento.Add(Fila2)
        '
        Fila2 = New ItemLotesParaAsientos
        Fila2.TipoOperacion = Tipo
        Fila2.Centro = Centro
        Fila2.MontoNeto = DtFacturaCabeza.Rows(0).Item("Descarga")
        If Tipo = 1 Then Fila2.Clave = 601
        If Tipo = 2 Then Fila2.Clave = 600
        If Fila2.MontoNeto <> 0 Then ListaLotesParaAsiento.Add(Fila2)

        Dim Item As New ItemListaConceptosAsientos

        If DtFacturaCabeza.Rows(0).Item("Senia") <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = 204
            Item.Importe = DtFacturaCabeza.Rows(0).Item("Senia")
            If Item.Importe <> 0 Then ListaConceptos.Add(Item)
        End If

        If DtFacturaCabeza.Rows(0).Item("Alicuota") <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = HallaClaveIva(DtFacturaCabeza.Rows(0).Item("Alicuota"))
            Item.Importe = Trunca(DtFacturaCabeza.Rows(0).Item("Bruto") * DtFacturaCabeza.Rows(0).Item("Alicuota") / 100)
            Item.TipoIva = 5
            If Item.Importe <> 0 Then ListaIVA.Add(Item)
        End If
        '
        If DtFacturaCabeza.Rows(0).Item("AlicuotaComision") <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = HallaClaveIva(DtFacturaCabeza.Rows(0).Item("AlicuotaComision"))
            Item.Importe = Trunca(Comision * DtFacturaCabeza.Rows(0).Item("AlicuotaComision") / 100)
            Item.TipoIva = 6
            If Item.Importe <> 0 Then ListaIVA.Add(Item)
        End If
        '
        If DtFacturaCabeza.Rows(0).Item("AlicuotaDescarga") <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = HallaClaveIva(DtFacturaCabeza.Rows(0).Item("AlicuotaDescarga"))
            Item.Importe = Trunca(DtFacturaCabeza.Rows(0).Item("Descarga") * DtFacturaCabeza.Rows(0).Item("AlicuotaDescarga") / 100)
            Item.TipoIva = 6
            If Item.Importe <> 0 Then ListaIVA.Add(Item)
        End If
        '
        For Each Row As DataRow In DtDetalleConceptos.Rows
            RowsBusqueda = DtGrid1.Select("Concepto = " & Row("Concepto"))
            If RowsBusqueda.Length <> 0 Then
                If RowsBusqueda(0).Item("Tipo") = 1 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("Concepto")
                    Item.Importe = Row("Importe")
                    Item.TipoIva = 11    'Debito fiscal. 
                    If Item.Importe <> 0 Then ListaRetenciones.Add(Item)
                End If
            End If
        Next
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = DtFacturaCabeza.Rows(0).Item("Neto") + DtFacturaCabeza.Rows(0).Item("Senia")
        If Item.Importe <> 0 Then ListaConceptos.Add(Item)
        '

        If Funcion = "A" Then
            If Not Asiento(DocumentoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, CDate(TextFechaContable.Text), 0) Then Return False
        End If

        Return True

    End Function
    Public Function GrabaImpreso() As Boolean

        Dim Sql As String = "UPDATE LiquidacionCabeza Set Impreso = 1 WHERE Liquidacion = " & DtCabeza.Rows(0).Item("Liquidacion") & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionLiquidacion)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                If CInt(Cmd.ExecuteNonQuery()) = 0 Then Return False
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos al Grabar Liquidación. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End Try

        Return True

    End Function
    Private Sub Print_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        '        Dim xPos As Single = e.MarginBounds.Left

        Dim MIzq = 15
        Dim MTop = 20

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim PrintFont As System.Drawing.Font
        Dim SaltoLinea As Integer = 4

        x = MIzq : y = MTop + 46 ''''''MTop + 40

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            PrintFont = New Font("Courier New", 11)
            'Titulos.
            Texto = TextFechaContable.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 130, y - 25)
            If PAbierto Or EsPreliquidacion Then
                If Not PAbierto Then
                    Texto = Format(CDec(MaskedLiquidacion.Text), "0000-00000000")
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y - 25)
                End If
                Texto = "REMITENTE  : " & ComboProveedor.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                If Numero <> 0 Then
                    Texto = "DOMICILIO  : " & Calle & " No: " & Numero
                Else
                    Texto = "DOMICILIO  : " & Calle
                End If
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "LOCALIDAD  : " & Localidad
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                Texto = "PROVINCIA : " & TextProvincia.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 60, y)
                y = y + SaltoLinea
                Texto = "CUIT       : " & TextCuit.Text & "    " & ComboTipoIva.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Else
                Texto = Format(CDec(MaskedLiquidacion.Text), "0000-00000000")
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y - 25)
                Texto = "REMITENTE  : " & ComboProveedor.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + 3 * SaltoLinea
            End If

            'Grafica 
            Dim Ancho As Integer = 185
            Dim Alto As Integer = 125
            Dim LineaLote As Integer = x + 16
            Dim LineaArticulo As Integer = x + 69
            Dim LineaRemito As Integer = x + 93
            Dim LineaIngresado As Integer = x + 108
            Dim LineaMerma As Integer = x + 121
            Dim LineaCantidad As Integer = x + 137
            Dim LineaPrecio As Integer = x + 161
            Dim LineaTotal As Integer = x + Ancho
            Dim LineaResumen As Integer = x + 140
            Dim Longi As Double
            Dim Articulo As String
            Dim Partes() As String
            Dim Xq As Integer

            y = MTop + 64 ''''''''''''MTop + 58

            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaLote, y, LineaLote, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaArticulo, y, LineaArticulo, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaRemito, y, LineaRemito, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaIngresado, y, LineaIngresado, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaMerma, y, LineaMerma, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCantidad, y, LineaCantidad, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaPrecio, y, LineaPrecio, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaTotal, y, LineaTotal, y + Alto)
            'Titulos de descripcion.
            PrintFont = New Font("Courier New", 9)
            '    Texto = "LOTE"
            Texto = "FECHA"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 2, y + 2)
            Texto = "ARTICULO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaArticulo - Longi - 35
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "REM/GUIA"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaRemito - Longi - 5
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "INGRESO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaIngresado - Longi - 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "MERMA"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaMerma - Longi - 2
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "A"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi - 6
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 1)
            Texto = "LIQUI."
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi - 2
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 4)
            Texto = "PRECIO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaPrecio - Longi - 5
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaTotal - Longi - 3
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'Linea Horizontal 
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)

            'Descripcion de Articulos.
            PrintFont = New Font("Courier New", 8)
            y = y - SaltoLinea
            For Each Row As DataGridViewRow In Grid.Rows
                y = y + SaltoLinea
                'Imprime fecha ingreso del lote. (antes se imprimia lote).
                '  Texto = Row.Cells("LoteYSecuencia").FormattedValue
                Texto = Strings.Left(Row.Cells("Fecha").FormattedValue, 6) & Strings.Right(Row.Cells("Fecha").FormattedValue, 2)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaLote - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime Articulo.
                Partes = Split(Row.Cells("Articulo").FormattedValue, "(")
                Articulo = Partes(0)
                Texto = Articulo
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaLote
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime remito.
                Texto = Row.Cells("RemitoGuia").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaRemito - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime lotes Ingresado.
                Texto = Row.Cells("Ingresados").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaIngresado - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime Merma.
                Texto = Row.Cells("Merma").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaMerma - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime Cantidad.
                Texto = Row.Cells("Cantidad").FormattedValue & Row.Cells("Medida").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaCantidad - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime Precio.
                Texto = Format(Row.Cells("PrecioF").Value, "0.#####")
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaPrecio - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime Importe.
                Texto = Format(Row.Cells("Total").Value, "0.00")
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaTotal - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime Permisos de Importacion.
                Longi = e.Graphics.MeasureString(Articulo, PrintFont).Width
                Dim Permisos As String = ""
                For Each Fila As FilaLiquidacion In PListaDeLotes
                    If Fila.Lote = Row.Cells("Lote").Value And Fila.Secuencia = Row.Cells("Secuencia").Value And Fila.PermisoImp <> "" Then
                        Permisos = Fila.PermisoImp
                        Exit For
                    End If
                Next
                If Permisos <> "" Then
                    PrintFont = New Font("Courier New", 7)
                    Permisos = "PI " & Permisos
                    y = y + SaltoLinea
                    Texto = Permisos
                    Xq = LineaLote + 1
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                    PrintFont = New Font("Courier New", 8)
                End If
            Next

            y = MTop + 191 '''''''''MTop + 185
            '
            'Imprime Totales.
            PrintFont = New Font("Courier New", 11)
            Texto = "TOTAL UNIDADES :   " & TextUnidades.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 50, y)
            y = MTop + 196  ''''''''''MTop + 190
            For Each Row As DataGridViewRow In Grid1.Rows
                y = y + SaltoLinea
                Dim Valor As String = ""
                If Row.Cells("Valor").FormattedValue <> "" Then Valor = Row.Cells("Valor").FormattedValue & " %"
                Texto = Row.Cells("Concepto").FormattedValue & "  " & Valor
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 40, y)
                Texto = Row.Cells("Importe").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaResumen - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            Next

            Paginas = Paginas + 1

            If Paginas < Copias Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
            e.HasMorePages = False
        End Try

    End Sub
    Private Function EsInsumos(ByVal Liquidacion As Double, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String
        If Abierto Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT EsInsumos FROM LiquidacionCabeza WHERE Liquidacion = " & Liquidacion & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: LiquidacionCabeza. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Private Sub PresentaGrid1(ByVal EsTotal As Boolean)

        For Each Row1 As DataGridViewRow In Grid1.Rows
            If Row1.Cells("Concepto").Value = 5 Or Row1.Cells("Concepto").Value = 6 Then
                Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
                Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
            End If
            If EsTotal Then
                Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
            Else
                If Row1.Cells("Tipo").Value = 1 Then
                    Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                    Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
                End If
                If Row1.Cells("Concepto").Value = 1 Or Row1.Cells("Concepto").Value = 2 Or Row1.Cells("Concepto").Value = 10 Or Row1.Cells("Concepto").Value = 11 Then
                    Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                    Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
                End If
                If Row1.Cells("Concepto").Value = 3 Then
                    Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
                End If
                If Row1.Cells("Concepto").Value = 4 Then
                    Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                End If
            End If
        Next

        'PrecentaGrid1 concepto retenciones segun codigo
        PresentaGrid1Retenciones()

    End Sub
    Private Sub PresentaGrid1Retenciones()

        For Each Row1 As DataGridViewRow In Grid1.Rows
            If Row1.Cells("Tipo").Value = 1 Then
                If PLiquidacion <> 0 Then
                    Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                    Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
                Else
                    If HallaFormulaRetencion(Row1.Cells("Concepto").Value) = 0 Or RetencionManual Then
                        Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                        Row1.Cells("Importe").ReadOnly = False : Row1.Cells("Importe").Style.BackColor = Color.White
                    Else
                        Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                        Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
                    End If
                End If
            End If
        Next

    End Sub
    Private Function HallaFormulaRetencion(ByVal Retencion As Integer) As Integer

        For Each Fila As ItemIvaReten In ListaDeRetenciones
            If Fila.Clave = Retencion Then Return Fila.Formula
        Next

    End Function
    Private Function Valida() As Boolean

        For Each Fila As FilaLiquidacion In PListaDeLotes
            If HallaLiquidacionLote(Fila.Lote, Fila.Secuencia, Fila.Operacion) <> 0 Then
                MsgBox("Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " Ya esta liquidado.")
                Return False
            End If
        Next

        If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 And PLiquidacion = 0 Then
            MsgBox("Fecha Incorrecta. Existe Otro Comprobante con Fecha " & UltimaFechaW & " Posterior a la Informada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If

        If TextFechaContable.Text = "" Then
            MsgBox("Falta Informar Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If Not ValidaFechaContable(CDate(TextFechaContable.Text), UltimaFechaContableW) Then
            TextFechaContable.Focus()
            Return False
        End If

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtGrid1.Select("Concepto = 1")
        If RowsBusqueda(0).Item("Importe") <= 0 Then
            MsgBox("Venta Bruta Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        RowsBusqueda = DtGrid1.Select("Concepto = 11")
        If RowsBusqueda(0).Item("Importe") <= 0 Then
            MsgBox("Total Neto Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        RowsBusqueda = DtGrid1.Select("Concepto = 100")
        If RowsBusqueda(0).Item("Importe") < 0 Then
            MsgBox("Total NO Debe Ser Negativo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        RowsBusqueda = DtGrid1.Select("Concepto = 7")
        Select Case PSenia
            Case Is > 0
                If RowsBusqueda(0).Item("Importe") < 0 Then
                    MsgBox("Seña NO Debe ser Negativa.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                If RowsBusqueda(0).Item("Importe") > PSenia Then
                    MsgBox("Seña Distinta a la Informada en la Pre-Liquidación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            Case Is < 0
                If RowsBusqueda(0).Item("Importe") > 0 Then
                    MsgBox("Seña NO Debe ser Positiva.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                If RowsBusqueda(0).Item("Importe") < PSenia Then
                    MsgBox("Seña Distinta a la Informada en la Pre-Liquidación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
        End Select

        If PDirecto <> 100 Then
            RowsBusqueda = DtGrid1.Select("Concepto = 4")
            If -RowsBusqueda(0).Item("Importe") > PDescarga Then
                MsgBox("Descarga Mayor a Informada en la Pre-Liquidación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            RowsBusqueda = DtGrid1.Select("Concepto = 3")
            If -Truncate(RowsBusqueda(0).Item("Importe")) > Truncate(PMontoComision) Then
                MsgBox("Comisión Mayor a Informada en la Pre-Liquidación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------               MANEJO DEL GRID.                 --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) And _
              Not IsNothing(Grid.Rows(e.RowIndex).Cells("Lote").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
            End If
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Merma" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Precio" Or Grid.Columns(e.ColumnIndex).Name = "PrecioF" Then
            If IsDBNull(e.Value) Then e.Value = 0
            e.Value = Format(e.Value, "0.#####")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Total" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = "01/01/1800" Then
                    e.Value = ""
                Else
                    e.Value = Format(e.Value, "dd/MM/yyyy")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Total" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "RemitoGuia" Then
            If Not IsDBNull(e.Value) Then
                e.Value = NumeroEditado(e.Value)
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Not Grid.Columns.Item(columna).Name = "PrecioF" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioF" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioF" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 5)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------               MANEJO DEL GRID1.                --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid1_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellEnter

        If Not Grid1.Columns(e.ColumnIndex).ReadOnly Then
            Grid1.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid1_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellEndEdit

        If Grid1.Columns(e.ColumnIndex).Name = "Valor" Or Grid1.Columns(e.ColumnIndex).Name = "Importe" Then
            If IsDBNull(Grid1.CurrentCell.Value) Then Grid1.CurrentCell.Value = 0
        End If

        If Grid1.Columns(e.ColumnIndex).Name = "Valor" Then
            If Grid1.Rows(e.RowIndex).Cells("Concepto").Value = 5 Or Grid1.Rows(e.RowIndex).Cells("Concepto").Value = 6 Then
                If Not IvaExiste(TablaIva, Grid1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
                    MsgBox("Alicuota IVA. No Existe en el Sistema.", MsgBoxStyle.Critical)
                    Grid1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0
                    Grid1.Rows(e.RowIndex).Cells("Importe").Value = 0
                End If
            End If
        End If

        CalculaTotales()

    End Sub
    Private Sub Grid1_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid1.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid1.Columns(e.ColumnIndex).Name = "Valor" Or Grid1.Columns(e.ColumnIndex).Name = "Importe" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

    End Sub
    Private Sub Grid1_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid1.EditingControlShowing

        Dim columna As Integer = Grid1.CurrentCell.ColumnIndex

        If Grid1.Columns.Item(columna).Name = "Concepto" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress1
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged1

    End Sub
    Private Sub ValidaKey_KeyPress1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Valor" Or Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Importe" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If Grid1.CurrentRow.Cells("Concepto").Value = 7 Then
                If InStr("-0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
            Else
                If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
            End If
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged1(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Valor" Or Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Importe" Then
            If CType(sender, TextBox).Text <> "" Then
                If Grid1.CurrentRow.Cells("Concepto").Value = 7 Then
                    EsNumericoGridBox.ValidaConSigno(CType(sender, TextBox), GDecimales)
                Else
                    EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
                End If
            End If
        End If

    End Sub
    Private Sub Grid1_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid1.DataError
        Exit Sub
    End Sub


End Class