Public Class ListaLibroIvaVenta
    ' http://www.experts-exchange.com/Software/Office_Productivity/Office_Suites/MS_Office/Q_27075166.html
    'ActiveWindow.SelectedSheets.HPageBreaks.
    '
    Dim TotalGrabado As Decimal = 0
    Dim TotalExento As Decimal = 0
    Dim TotalOtroIva As Decimal = 0
    Dim TotalRetPerc As Decimal = 0
    Dim TotalTotal As Decimal = 0
    Dim TablaIvas(0) As ItemIvaReten
    Dim TablaTotalIvas(0) As ItemIvaReten
    '
    Private WithEvents bs As New BindingSource
    '
    Dim Dt As DataTable
    Dim DtTipo As DataTable
    Dim DtConceptosNVLP As DataTable
    Dim DtGastosGrabados As DataTable
    Dim DtRetPerc As DataTable
    Dim View As DataView
    '
    Dim DtGrid As DataTable
    Dim DtProvincias As DataTable
    Dim Opcion1LibrosIva As Boolean
    Dim InicioColumnaIva As Integer = 10
    Private Sub InformeLibroIva_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        LlenaCombosGrid()

        ComboTipoComprobante.DataSource = DtTipo
        ComboTipoComprobante.DisplayMember = "Nombre"
        ComboTipoComprobante.ValueMember = "Codigo"
        ComboTipoComprobante.SelectedValue = 0
        With ComboTipoComprobante
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboCliente, "", "Clientes")
        ComboCliente.SelectedValue = 0
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboProveedor, "", "Proveedores")
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboPuntoDeVenta.DataSource = Tablas.Leer("SELECT Clave,RIGHT('0000' + CAST(Clave AS varchar),4) as Nombre FROM PuntosDeVenta WHERE Clave > 0 ORDER BY Nombre;")
        Dim Row As DataRow = ComboPuntoDeVenta.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboPuntoDeVenta.DataSource.Rows.Add(Row)
        ComboPuntoDeVenta.DisplayMember = "Nombre"
        ComboPuntoDeVenta.ValueMember = "Clave"
        ComboPuntoDeVenta.SelectedValue = 0
        With ComboPuntoDeVenta
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboTipoIva.DataSource = CreaDtTipoIva()
        ComboTipoIva.DisplayMember = "Nombre"
        ComboTipoIva.ValueMember = "Codigo"
        ComboTipoIva.SelectedValue = 0
        With ComboTipoIva
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        CreaDtGrid()
        CreaDtProvincias()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        ArmaTablaIvaConImporte(TablaIvas)
        ArmaTablaIvaConImporte(TablaTotalIvas)

        DtConceptosNVLP = ArmaConceptosNVLP(0)

        DtRetPerc = New DataTable
        If Not Tablas.Read("SELECT Clave FROM Tablas WHERE Tipo = 25;", Conexion, DtRetPerc) Then
            Me.Close()
            Exit Sub
        End If

        DtGastosGrabados = New DataTable
        If Not Tablas.Read("SELECT Clave,Activo2 AS Grabado FROM Tablas WHERE Tipo = 33 or Tipo = 32;", Conexion, DtGastosGrabados) Then
            Me.Close()
            Exit Sub
        End If

        Dim k As Integer = InicioColumnaIva   'Comienzo de las columnas de ivas.

        For I As Integer = 1 To UBound(TablaIvas)
            If I <= 5 Then
                Grid.Columns(k).HeaderText = Format(TablaIvas(I).Alicuota, "00.00") & "%" : Grid.Columns(k).Visible = True
                k = k + 1
            Else
                Grid.Columns("OtroIVA").Visible = True
            End If
        Next

        If Not PermisoTotal Then
            CheckBoxSinContables.Visible = False
            CheckBoxSoloContables.Visible = False
        Else
            CheckBoxSinContables.Visible = True
            CheckBoxSoloContables.Visible = True
        End If

        GExcepcion = HallaDatoGenerico("SELECT Opcion1LibrosIva FROM DatosEmpresa WHERE Indice = 1;", Conexion, Opcion1LibrosIva)
        If Not IsNothing(GExcepcion) Then
            MsgBox("Error al Leer Tabla: DatosEmpresa." + vbCrLf + vbCrLf + GExcepcion.Message)
            Me.Close() : Exit Sub
        End If

        Select Case Opcion1LibrosIva
            Case True
                Label5.Text = "Método de Calculo Según Opción 1."
            Case False
                Label5.Text = "Método de Calculo Según Opción 2."
        End Select

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Hasta debe ser mayor o igual a Fecha Desde.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        TotalGrabado = 0
        TotalExento = 0
        TotalOtroIva = 0
        TotalRetPerc = 0
        TotalTotal = 0

        AceraTabla(TablaTotalIvas)

        DtGrid.Clear()
        DtProvincias.Clear()

        Dim SqlFecha As String
        SqlFecha = "F.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaContable As String
        SqlFechaContable = "F.FechaContable >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.FechaContable < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaOficial As String
        SqlFechaOficial = "F.FechaReciboOficial >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.FechaReciboOficial < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaElectronica As String
        SqlFechaElectronica = "F.FechaElectronica >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.FechaElectronica < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlContable As String = ""
        If CheckBoxSinContables.Checked Then SqlContable = " AND F.Tr = 0 "
        If CheckBoxSoloContables.Checked Then SqlContable = " AND F.Tr = 1 "
        If CheckBoxSinContables.Checked And CheckBoxSoloContables.Checked Then SqlContable = ""

        Dim Sql As String
        Sql = "SELECT 1 AS TipoComprobante,C.Nombre,C.Cuit,C.TipoIva,C.Provincia,F.Factura AS Comprobante,F.Remito AS Remito,D.Cantidad,D.Precio,D.Iva,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,0 AS Factura,0 As Impuesto,0 As Importe,F.Cambio,F.Factura AS Rec,F.Senia AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,F.EsZ,F.ComprobanteDesde,F.ComprobanteHasta,F.Comentario,F.Percepciones AS RetPerc FROM Clientes AS C INNER JOIN (FacturasCabeza AS F INNER JOIN FacturasDetalle AS D " & _
                      "ON F.Factura = D.Factura) ON F.Cliente = C.Clave WHERE F.EsExterior = 0 AND " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT 1 AS TipoComprobante,C.Nombre,C.Cuit,C.TipoIva,C.Provincia,F.Factura AS Comprobante,F.Remito AS Remito,D.Cantidad,D.Precio,D.Iva,CAST(FLOOR(CAST(F.FechaElectronica AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,0 AS Factura,0 As Impuesto,0 As Importe,F.Cambio,F.Factura AS Rec,F.Senia AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,F.EsZ,F.ComprobanteDesde,F.ComprobanteHasta,F.Comentario,F.Percepciones AS RetPerc FROM Clientes AS C INNER JOIN (FacturasCabeza AS F INNER JOIN FacturasDetalle AS D " & _
                      "ON F.Factura = D.Factura) ON F.Cliente = C.Clave WHERE F.EsExterior = 1 AND " & SqlFechaElectronica & SqlContable & _
                   " UNION ALL " & _
              "SELECT 4 AS TipoComprobante,'' AS Nombre,0 AS Cuit,0 AS TipoIva,0 As Provincia,F.NotaCredito As Comprobante, 0 AS Remito,D.Cantidad,D.Precio,D.Iva,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado AS Estado,F.Factura as Factura,0 As Impuesto,0 As Importe,F.Cambio,F.NotaCredito AS Rec,0 AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,1 As Pais,EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,F.Comentario,F.Percepciones AS RetPerc FROM NotasCreditoCabeza AS F INNER JOIN NotasCreditoDetalle AS D ON F.NotaCredito = D.NotaCredito WHERE " & SqlFechaContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,C.TipoIva,C.Provincia,F.Nota As Comprobante,0 AS Remito,0 AS Cantidad,0 AS Precio,D.Alicuota As IVA,CAST(FLOOR(CAST(F.Fecha AS FLOAT)) AS DATETIME) AS Fecha,F.Estado AS Estado,0 AS Factura,D.Mediopago As Impuesto,D.Neto AS Importe,F.Cambio,F.Nota AS Rec,0 AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,F.Comentario,D.Importe AS RetPerc FROM Clientes AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.ChequeRechazado = 0 AND (F.TipoNota = 5 or F.TipoNota = 7) AND F.EsElectronica = 0 AND " & SqlFecha & SqlContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,C.TipoIva,C.Provincia,F.Nota As Comprobante,0 AS Remito,0 AS Cantidad,0 AS Precio,D.Alicuota As IVA,CAST(FLOOR(CAST(F.Fecha AS FLOAT)) AS DATETIME) AS Fecha,F.Estado AS Estado,0 AS Factura,D.Mediopago As Impuesto,D.Neto AS Importe,F.Cambio,F.Nota AS Rec,0 AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,F.Comentario,D.Importe AS RetPerc FROM Proveedores AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.ChequeRechazado = 0 AND (F.TipoNota = 6 or F.TipoNota = 8) AND F.EsElectronica = 0 AND " & SqlFecha & SqlContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,C.TipoIva,C.Provincia,F.Nota As Comprobante,0 AS Remito,0 AS Cantidad,0 AS Precio,D.Alicuota As IVA,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado AS Estado,0 AS Factura,D.Mediopago As Impuesto,D.Neto AS Importe,F.Cambio,F.Nota AS Rec,0 AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,F.Comentario,D.Importe AS RetPerc FROM Clientes AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.ChequeRechazado = 0 AND (F.TipoNota = 5 or F.TipoNota = 7) AND F.EsElectronica = 1 AND " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,C.TipoIva,C.Provincia,F.Nota As Comprobante,0 AS Remito,0 AS Cantidad,0 AS Precio,D.Alicuota As IVA,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado AS Estado,0 AS Factura,D.Mediopago As Impuesto,D.Neto AS Importe,F.Cambio,F.Nota AS Rec,0 AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,F.Comentario,D.Importe AS RetPerc FROM Proveedores AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.ChequeRechazado = 0 AND (F.TipoNota = 6 or F.TipoNota = 8) AND F.EsElectronica = 1 AND " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,C.TipoIva,C.Provincia,F.ReciboOficial As Comprobante,0 AS Remito,0 AS Cantidad,0 AS Precio,D.Alicuota As IVA,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado AS Estado,0 AS Factura,D.MedioPago As Impuesto,D.Neto As Importe,F.Cambio,F.Nota AS Rec,0 AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,F.Comentario,D.Importe AS RetPerc FROM Clientes AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.ChequeRechazado = 0 AND (F.TipoNota = 50 or F.TipoNota = 70) AND " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT 100 AS TipoComprobante,C.Nombre,C.Cuit,C.TipoIva,C.Provincia,F.ReciboOficial As Comprobante,0 AS Remito,0 AS Cantidad,0 AS Precio,D.Alicuota As IVA,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,0 AS Factura,D.Impuesto,D.Importe,1 AS Cambio,F.Liquidacion AS Rec,0 AS Comision,0 As AlicuotaComision,0 AS Descarga,0 As AlicuotaDescarga,C.Pais,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,F.Comentario,0 AS RetPerc FROM Clientes AS C INNER JOIN (NVLPCabeza AS F INNER JOIN NVLPDetalle AS D ON F.Liquidacion = D.Liquidacion) ON C.Clave = F.Cliente WHERE " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT 200 AS TipoComprobante,C.Nombre,C.Cuit,C.TipoIva,C.Provincia,F.Liquidacion As Comprobante,0 AS Remito,1 AS Cantidad,Bruto AS Precio,D.Valor AS IVA,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,0 AS Factura,D.Concepto AS Impuesto,D.Importe AS Importe,1 AS Cambio,F.Liquidacion AS Rec,F.Comision,F.AlicuotaComision,F.Descarga,F.AlicuotaDescarga,C.Pais,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,F.Comentario,0 AS RetPerc FROM Proveedores AS C INNER JOIN (LiquidacionCabeza AS F INNER JOIN LiquidacionDetalleConceptos AS D ON F.Liquidacion = D.Liquidacion) ON C.Clave = F.Proveedor WHERE F.EsInsumos = 0 AND " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT 210 AS TipoComprobante,C.Nombre,C.Cuit,C.TipoIva,C.Provincia,F.Liquidacion As Comprobante,0 AS Remito,1 AS Cantidad,Bruto AS Precio,D.Valor AS IVA,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,0 AS Factura,D.Concepto AS Impuesto,D.Importe AS Importe,1 AS Cambio,F.Liquidacion AS Rec,F.Comision,F.AlicuotaComision,F.Descarga,F.AlicuotaDescarga,C.Pais,0 AS EsZ,0 AS ComprobanteDesde,0 AS ComprobanteHasta,F.Comentario,0 AS RetPerc FROM Proveedores AS C INNER JOIN (LiquidacionCabeza AS F INNER JOIN LiquidacionDetalleConceptos AS D ON F.Liquidacion = D.Liquidacion) ON C.Clave = F.Proveedor WHERE F.EsInsumos = 1 AND " & SqlFechaContable & SqlContable & ";"

        Dt = New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        View = New DataView
        View = Dt.DefaultView
        View.Sort = "Fecha,Comprobante,TipoComprobante"

        'Saca Notas de credito con devolucion si es solo contables.
        If Not CheckBoxSinContables.Checked And CheckBoxSoloContables.Checked Then
            For I As Integer = View.Count - 1 To 0 Step -1
                If View(I).Item("TipoComprobante") = 4 Then View(I).Delete()
            Next
            View.Table.AcceptChanges()
        End If

        For I As Integer = 0 To View.Count - 1
            If View(I).Item("Pais") <> 1 Then
                View(I).Item("Cuit") = HallaCuitPais(View(I).Item("Pais"))
                If View(I).Item("Cuit") < 0 Then
                    MsgBox("Error Base de Datos al leer Tabla Paises.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close() : Exit Sub
                End If
            End If
            If View(I).Item("TipoComprobante") = 4 Then
                HallaDatosClienteW(View(I).Item("Factura"), View(I).Item("Nombre"), View(I).Item("Cuit"), View(I).Item("Pais"), View(I).Item("Provincia"), View(I).Item("ComprobanteDesde"), View(I).Item("ComprobanteHasta"), View(I).Item("TipoIva"))
                If View(I).Item("Nombre") = "" Then
                    MsgBox("Error Base de Datos al leer Tabla de Facturas Cabeza", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close() : Exit Sub
                End If
            End If
            If View(I).Item("TipoComprobante") = 100 And ComboPuntoDeVenta.SelectedValue <> 0 Then
                MsgBox("Seleccion por lote incorrecta. Hay NVLP que no tienen definido Punto de Venta.")
                Me.Cursor = System.Windows.Forms.Cursors.Default
                DtGrid.Clear()
                Exit Sub
            End If
            If MovimientoOk(View(I).Item("Comprobante"), View(I).Item("TipoIva")) Then
                Select Case View(I).Item("TipoComprobante")
                    Case 6, 8
                        If Not Opcion1LibrosIva Then ListaLinea(I)
                    Case 50, 70
                        If Opcion1LibrosIva Then ListaLinea(I)
                    Case Else
                        ListaLinea(I)
                End Select
            Else
                SaltaComprobante(I)
            End If
        Next
        CorteTotales()
        CorteTotalesPorProvincia()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim PuntoDeVenta As String = ""
        If ComboPuntoDeVenta.SelectedValue <> 0 Then PuntoDeVenta = " Pto. de Venta: " & ComboPuntoDeVenta.Text

        GridAExcel(Grid, GNombreEmpresa & "  " & Format(Date.Now, "dd/MM/yyyy"), "", "DIARIO I.V.A. VENTAS - PERIODO : " & Format(DateTimeDesde.Value, "dd/MM/yyyy") & " - " & Format(DateTimeHasta.Value, "dd/MM/yyyy") & "  " & ComboTipoIva.Text & "  " & PuntoDeVenta)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboTipoComprobante_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipoComprobante.Validating

        If IsNothing(ComboTipoComprobante.SelectedValue) Then ComboTipoComprobante.SelectedValue = 0

    End Sub
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0

    End Sub
    Private Sub ComboPuntoDeVenta_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVenta.Validating

        If IsNothing(ComboPuntoDeVenta.SelectedValue) Then ComboPuntoDeVenta.SelectedValue = 0

    End Sub
    Private Sub ComboTipoIva_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipoIva.Validating

        If IsNothing(ComboTipoIva.SelectedValue) Then ComboTipoIva.SelectedValue = 0

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
    Private Sub ListaLinea(ByRef I As Integer)

        Dim Grabado As Decimal = 0
        Dim Exento As Decimal = 0
        Dim OtroIva As Decimal = 0
        Dim RetPerc As Decimal = 0
        Dim Total As Decimal = 0
        Dim Provincia As Integer = View(I).Item("Provincia")

        AceraTabla(TablaIvas)

        Dim Row As DataRow = DtGrid.NewRow()

        Row("Grabado") = 0
        Row("Exento") = 0
        Row("Iva1") = 0
        Row("Iva2") = 0
        Row("Iva3") = 0
        Row("Iva4") = 0
        Row("Iva5") = 0
        Row("OtroIva") = 0
        Row("RetPerc") = 0
        Row("Total") = 0

        Dim Tipo As String = ""
        Select Case View(I).Item("TipoComprobante")
            Case 1
                If View(I).Item("Remito") = 0 Then
                    Tipo = 1
                    Row("Cartel") = "Factura-Rem."
                Else
                    Tipo = 2
                    Row("Cartel") = "Factura"
                End If
                If View(I).Item("EsZ") Then
                    Tipo = 44
                    Select Case Strings.Left(View(I).Item("Comprobante").ToString, 1)
                        Case 9
                            Row("Cartel") = "Fact.Ticket"
                        Case 1
                            Row("Cartel") = "Fact.Ticket A"
                        Case 2
                            Row("Cartel") = "Fact.Ticket B"
                        Case 3
                            Row("Cartel") = "Fact.Ticket C"
                        Case 5
                            Row("Cartel") = "Fact.Ticket M"
                    End Select
                End If
            Case 4
                Tipo = 4
                Row("Cartel") = "N.Credito"
                If View(I).Item("EsZ") Then
                    Tipo = 54
                    Select Case Strings.Left(View(I).Item("Comprobante").ToString, 1)
                        Case 9
                            Row("Cartel") = "N.Credito Ticket"
                        Case 1
                            Row("Cartel") = "N.Credito Ticket A"
                        Case 2
                            Row("Cartel") = "N.Credito Ticket B"
                        Case 3
                            Row("Cartel") = "N.Credito Ticket C"
                        Case 5
                            Row("Cartel") = "N.Credito Ticket M"
                    End Select
                End If
            Case 5
                Tipo = 5
                Row("Cartel") = "N.Deb.Financiera"
            Case 7
                Tipo = 7
                Row("Cartel") = "N.Cred.Financiera"
            Case 6
                Tipo = 6
                Row("Cartel") = "N.Deb.Fin.Provedor"
            Case 8
                Tipo = 8
                Row("Cartel") = "N.Cred.Fin.Provedor"
            Case 50
                Tipo = 50
                Row("Cartel") = "N.Deb. del Cliente"
            Case 70
                Tipo = 70
                Row("Cartel") = "N.Cred. del Cliente"
            Case 100
                Tipo = 100
                Row("Cartel") = "N.V.L.P."
            Case 200
                Tipo = 200
                Row("Cartel") = "Liquidación"
            Case 210
                Tipo = 200
                Row("Cartel") = "Liquidación"
        End Select

        If ComboTipoComprobante.SelectedValue <> 0 And ComboTipoComprobante.SelectedValue <> Tipo Then Exit Sub
        If ComboCliente.SelectedValue <> 0 And ComboCliente.Text <> View(I).Item("Nombre") Then Exit Sub

        Row("Tipo") = Tipo
        Row("Comprobante") = View(I).Item("Comprobante")
        Row("Fecha") = View(I).Item("Fecha")
        Row("Rec") = View(I).Item("Rec")
        Row("Estado") = View(I).Item("Estado")
        Row("Cuit") = View(I).Item("Cuit")
        Row("Cliente") = View(I).Item("Nombre")
        Row("Pais") = View(I).Item("Pais")
        Row("Comentario") = View(I).Item("Comentario")
        Row("PuntoDeVenta") = 0
        Row("ComprobanteHasta") = View(I).Item("ComprobanteHasta")
        Row("ComprobanteDesde") = View(I).Item("ComprobanteDesde")
        If View(I).Item("EsZ") Then
            Row("PuntoDeVenta") = Strings.Mid(View(I).Item("Comprobante").ToString, 2, 4)
        End If
        If View(I).Item("Estado") <> 3 Then
            Select Case View(I).Item("TipoComprobante")
                Case 200
                    TotalizaComprobanteLiquidacion(I, Grabado, Exento, TablaIvas, OtroIva, RetPerc)
                Case 210
                    TotalizaComprobanteLiquidacionInsumos(I, Grabado, Exento, TablaIvas, OtroIva, RetPerc)
                Case 100
                    TotalizaComprobanteNVLP(I, Grabado, Exento, TablaIvas, OtroIva)
                Case 4, 54
                    TotalizaComprobanteNotaCredito(I, Grabado, Exento, TablaIvas, OtroIva, RetPerc)
                Case 1, 44
                    TotalizaComprobanteFactura(I, Grabado, Exento, TablaIvas, OtroIva, RetPerc)
                Case Else
                    TotalizaComprobanteRecibo(I, Grabado, Exento, TablaIvas, OtroIva, RetPerc)
            End Select
            Row("Grabado") = Grabado
            Row("Exento") = Exento
            Row("Iva1") = 0
            Row("Iva2") = 0
            Row("Iva3") = 0
            Row("Iva4") = 0
            Row("Iva5") = 0
            Row("RetPerc") = RetPerc
            If UBound(TablaIvas) >= 1 Then Row("Iva1") = TablaIvas(1).Importe
            If UBound(TablaIvas) >= 2 Then Row("Iva2") = TablaIvas(2).Importe
            If UBound(TablaIvas) >= 3 Then Row("Iva3") = TablaIvas(3).Importe
            If UBound(TablaIvas) >= 4 Then Row("Iva4") = TablaIvas(4).Importe
            If UBound(TablaIvas) >= 5 Then Row("Iva5") = TablaIvas(5).Importe
            Row("OtroIVA") = OtroIva
            Row("Total") = Grabado + Exento + OtroIva + RetPerc
            For I2 As Integer = 1 To UBound(TablaIvas)
                Row("Total") = Row("Total") + TablaIvas(I2).Importe
            Next
            '  
            TotalGrabado = TotalGrabado + Grabado
            TotalExento = TotalExento + Exento
            TotalOtroIva = TotalOtroIva + OtroIva
            TotalRetPerc = TotalRetPerc + RetPerc
            For I2 As Integer = 1 To UBound(TablaIvas)
                TablaTotalIvas(I2).Importe = TablaTotalIvas(I2).Importe + TablaIvas(I2).Importe
            Next
            TotalTotal = TotalTotal + Row("Total")
            ArmaDtProvincia(Row, Provincia)
        Else
            SaltaComprobante(I)
        End If

        DtGrid.Rows.Add(Row)

    End Sub
    Private Sub CorteTotales()

        Dim Row As DataRow

        Row = DtGrid.NewRow()
        DtGrid.Rows.Add(Row)

        Row = DtGrid.NewRow()
        Row("Estilo") = 1
        Row("Tipo") = 0
        Row("Comprobante") = "TOTALES"
        Row("Grabado") = TotalGrabado
        Row("Exento") = TotalExento
        Row("Iva1") = 0
        Row("Iva2") = 0
        Row("Iva3") = 0
        Row("Iva4") = 0
        Row("Iva5") = 0
        If UBound(TablaTotalIvas) >= 1 Then Row("Iva1") = TablaTotalIvas(1).Importe
        If UBound(TablaTotalIvas) >= 2 Then Row("Iva2") = TablaTotalIvas(2).Importe
        If UBound(TablaTotalIvas) >= 3 Then Row("Iva3") = TablaTotalIvas(3).Importe
        If UBound(TablaTotalIvas) >= 4 Then Row("Iva4") = TablaTotalIvas(4).Importe
        If UBound(TablaTotalIvas) >= 5 Then Row("Iva5") = TablaTotalIvas(5).Importe
        Row("OtroIVA") = TotalOtroIva
        Row("RetPerc") = TotalRetPerc
        Row("Total") = TotalTotal
        DtGrid.Rows.Add(Row)

    End Sub
    Private Sub CorteTotalesPorProvincia()

        Dim Row As DataRow

        Row = DtGrid.NewRow()
        DtGrid.Rows.Add(Row)
        Row = DtGrid.NewRow()
        DtGrid.Rows.Add(Row)

        For Each Row1 As DataRow In DtProvincias.Rows
            Row = DtGrid.NewRow()
            Row("Estilo") = 1
            Row("Tipo") = 0
            If Row1("Provincia") <> 9999999 Then
                Row("Comprobante") = NombreProvincia(Row1("Provincia"))
            Else : Row("Comprobante") = "Exportación"
            End If
            Row("Grabado") = Row1("Grabado")
            Row("Exento") = Row1("Exento")
            Row("Iva1") = Row1("Iva1")
            Row("Iva2") = Row1("Iva2")
            Row("Iva3") = Row1("Iva3")
            Row("Iva4") = Row1("Iva4")
            Row("Iva5") = Row1("Iva5")
            Row("OtroIVA") = Row1("OtroIva")
            Row("RetPerc") = Row1("RetPerc")
            Row("Total") = Row1("Total")
            DtGrid.Rows.Add(Row)
        Next

    End Sub
    Private Sub TotalizaComprobanteNotaCredito(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double, ByRef RetPerc As Decimal)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Z As Integer = 0, J As Integer = 0

        RetPerc = 0
        RetPerc = -View(I).Item("RetPerc")

        For Z = I To View.Count - 1
            If ComproAnt <> View(Z).Item("TipoComprobante") & View(Z).Item("Rec") Then
                Exit For
            End If
            View(Z).Item("Precio") = -1 * View(Z).Item("Precio")
            If View(Z).Item("Cambio") <> 1 Then
                View(Z).Item("Precio") = View(Z).Item("Precio") * View(Z).Item("Cambio")
            End If
            '
            If View(Z).Item("Iva") = 0 Then
                Exento = Exento + CalculaNeto(View(Z).Item("Cantidad"), View(Z).Item("Precio"))
            Else
                For J = 1 To UBound(TablaIvas)
                    If View(Z).Item("Iva") = TablaIvas(J).Alicuota Then
                        TablaIvas(J).Importe = TablaIvas(J).Importe + CalculaIva(View(Z).Item("Cantidad"), View(Z).Item("Precio"), View(Z).Item("Iva"))
                        Exit For
                    End If
                Next
                If J > 5 Then OtroIva = OtroIva + CalculaIva(View(Z).Item("Cantidad"), View(Z).Item("Precio"), View(Z).Item("Iva"))
                Grabado = Grabado + CalculaNeto(View(Z).Item("Cantidad"), View(Z).Item("Precio"))
            End If
        Next

        I = Z - 1

    End Sub
    Private Sub TotalizaComprobanteFactura(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double, ByRef RetPerc As Double)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Z As Integer = 0, J As Integer = 0
        Dim Importe As Double = 0
        Dim Senia As Double = View(I).Item("Comision")

        RetPerc = 0
        RetPerc = View(I).Item("RetPerc")

        If View(I).Item("Cambio") <> 1 Then
            Senia = Trunca(Senia * View(Z).Item("Cambio"))
        End If
        Exento = Exento + Senia

        For Z = I To View.Count - 1
            If ComproAnt <> View(Z).Item("TipoComprobante") & View(Z).Item("Rec") Then
                Exit For
            End If
            '
            If View(Z).Item("Iva") = 0 Then
                Importe = CalculaNeto(View(Z).Item("Cantidad"), View(Z).Item("Precio"))
                If View(Z).Item("Cambio") <> 1 Then Importe = Importe * View(Z).Item("Cambio")
                Exento = Exento + Importe
            Else
                For J = 1 To UBound(TablaIvas)
                    If View(Z).Item("Iva") = TablaIvas(J).Alicuota Then
                        Importe = CalculaIva(View(Z).Item("Cantidad"), View(Z).Item("Precio"), View(Z).Item("Iva"))
                        If View(Z).Item("Cambio") <> 1 Then Importe = Importe * View(Z).Item("Cambio")
                        TablaIvas(J).Importe = TablaIvas(J).Importe + Importe
                        Exit For
                    End If
                Next
                If J > 5 Then
                    Importe = CalculaIva(View(Z).Item("Cantidad"), View(Z).Item("Precio"), View(Z).Item("Iva"))
                    If View(Z).Item("Cambio") <> 1 Then Importe = Importe * View(Z).Item("Cambio")
                    OtroIva = OtroIva + Importe
                End If

                Importe = CalculaNeto(View(Z).Item("Cantidad"), View(Z).Item("Precio"))
                If View(Z).Item("Cambio") <> 1 Then Importe = Importe * View(Z).Item("Cambio")
                Grabado = Grabado + Importe
            End If
        Next

        I = Z - 1

    End Sub
    Private Sub TotalizaComprobanteRecibo(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double, ByRef RetPerc As Double)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Z As Integer = 0, J As Integer = 0

        For Z = I To View.Count - 1
            If ComproAnt <> View(Z).Item("TipoComprobante") & View(Z).Item("Rec") Then
                Exit For
            End If
            'Cambia de signo segun opciones-libros-iva.
            Select Case View(I).Item("TipoComprobante")
                Case 7, 8, 50
                    View(Z).Item("Importe") = -1 * View(Z).Item("Importe")
            End Select
            If View(Z).Item("Cambio") <> 1 Then
                View(Z).Item("Importe") = Trunca(View(Z).Item("Importe") * View(Z).Item("Cambio"))
            End If
            '
            If View(Z).Item("Iva") <> 0 Then
                Grabado = Grabado + View(Z).Item("Importe")
                For J = 1 To UBound(TablaIvas)
                    If View(Z).Item("Iva") = TablaIvas(J).Alicuota Then
                        TablaIvas(J).Importe = TablaIvas(J).Importe + CalculaIva(1, View(Z).Item("Importe"), View(Z).Item("Iva"))
                        Exit For
                    End If
                Next
                If J > 5 Then OtroIva = OtroIva + CalculaIva(1, View(Z).Item("Importe"), View(Z).Item("Iva"))
            Else
                Dim RowsBusqueda() As DataRow
                RowsBusqueda = DtRetPerc.Select("Clave = " & View(Z).Item("Impuesto"))
                If RowsBusqueda.Length <> 0 Then
                    Select Case View(I).Item("TipoComprobante")
                        Case 7, 8, 50
                            RetPerc = RetPerc - View(Z).Item("RetPerc")
                        Case Else
                            RetPerc = RetPerc + View(Z).Item("RetPerc")
                    End Select
                Else
                    Exento = Exento + View(Z).Item("Importe")
                End If
            End If
        Next

        I = Z - 1

    End Sub
    Private Sub TotalizaComprobanteNVLP(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Z As Integer = 0, J As Integer = 0

        For Z = I To View.Count - 1
            If ComproAnt <> View(Z).Item("TipoComprobante") & View(Z).Item("Rec") Then
                Exit For
            End If
            Select Case View(Z).Item("Impuesto")
                Case 1
                    Grabado = View(Z).Item("Importe")
                Case 2
                    Exento = View(Z).Item("Importe")
                Case 3
                    If View(Z).Item("Importe") <> 0 Then
                        For J = 1 To UBound(TablaIvas)
                            If View(Z).Item("Iva") = TablaIvas(J).Alicuota Then
                                TablaIvas(J).Importe = View(Z).Item("Importe")
                                Exit For
                            End If
                        Next
                        If J > 5 Then OtroIva = View(Z).Item("Importe")
                    End If
            End Select
        Next

        I = Z - 1

    End Sub
    Private Sub TotalizaComprobanteLiquidacion(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double, ByRef RetPerc As Double)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Z As Integer = 0, J As Integer = 0
        Dim Impuesto As Integer
        Dim RowsBusqueda() As DataRow

        For Z = I To View.Count - 1
            If ComproAnt <> View(Z).Item("TipoComprobante") & View(Z).Item("Rec") Then
                Exit For
            End If
            Impuesto = View(Z).Item("Impuesto")
            RowsBusqueda = DtRetPerc.Select("Clave = " & Impuesto)
            If RowsBusqueda.Length <> 0 Then Impuesto = -1
            Select Case Impuesto
                Case -1
                    RetPerc = RetPerc + View(Z).Item("Importe")
                Case 3
                    If View(Z).Item("AlicuotaComision") <> 0 Then
                        Grabado = Grabado + View(Z).Item("Importe")
                    Else
                        Exento = Exento + View(Z).Item("Importe")
                    End If
                Case 4
                    If View(Z).Item("AlicuotaDescarga") <> 0 Then
                        Grabado = Grabado + View(Z).Item("Importe")
                    Else
                        Exento = Exento + View(Z).Item("Importe")
                    End If
                Case 5
                    For J = 1 To UBound(TablaIvas)
                        If View(Z).Item("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(Z).Item("Importe")
                            Exit For
                        End If
                    Next
                    If J > 5 Then OtroIva = OtroIva + View(Z).Item("Importe")
                Case 6
                    For J = 1 To UBound(TablaIvas)
                        If View(Z).Item("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(Z).Item("Importe")
                            Exit For
                        End If
                    Next
                    If J > 5 Then OtroIva = OtroIva + View(Z).Item("Importe")
                Case 7
                    Exento = Exento + View(Z).Item("Importe")
            End Select
        Next

        I = Z - 1

    End Sub
    Private Sub TotalizaComprobanteLiquidacionInsumos(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double, ByRef RetPerc As Double)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Z As Integer = 0, J As Integer = 0
        Dim Impuesto As Integer
        Dim RowsBusqueda() As DataRow
        Dim Comision As Double = 0
        Dim InsumosProduccion As Double = 0
        Dim ServiciosProduccion As Double = 0
        Dim OtrosConceptos As Double = 0
        Dim HayIvaComision As Boolean = False
        Dim HayIvaInsumosProduccion As Boolean = False
        Dim HayIvaServiciosProduccion As Boolean = False
        Dim HayIvaOtrosConceptos As Boolean = False

        For Z = I To View.Count - 1
            If ComproAnt <> View(Z).Item("TipoComprobante") & View(Z).Item("Rec") Then
                Exit For
            End If
            Impuesto = View(Z).Item("Impuesto")
            RowsBusqueda = DtRetPerc.Select("Clave = " & Impuesto)
            If RowsBusqueda.Length <> 0 Then Impuesto = -1
            Select Case Impuesto
                Case -1
                    RetPerc = RetPerc + View(Z).Item("Importe")
                Case 5
                    Comision = View(Z).Item("Importe")
                Case 7
                    InsumosProduccion = View(Z).Item("Importe")
                Case 9
                    ServiciosProduccion = View(Z).Item("Importe")
                Case 10
                    OtrosConceptos = View(Z).Item("Importe")
                Case 6
                    For J = 1 To UBound(TablaIvas)
                        If View(Z).Item("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(Z).Item("Importe")
                            Exit For
                        End If
                    Next
                    If J > 5 Then OtroIva = OtroIva + View(Z).Item("Importe")
                    HayIvaComision = True
                Case 8
                    For J = 1 To UBound(TablaIvas)
                        If View(Z).Item("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(Z).Item("Importe")
                            Exit For
                        End If
                    Next
                    If J > 5 Then OtroIva = OtroIva + View(Z).Item("Importe")
                    HayIvaInsumosProduccion = True
                Case 11
                    For J = 1 To UBound(TablaIvas)
                        If View(Z).Item("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(Z).Item("Importe")
                            Exit For
                        End If
                    Next
                    If J > 5 Then OtroIva = OtroIva + View(Z).Item("Importe")
                    HayIvaServiciosProduccion = True
                Case 12
                    For J = 1 To UBound(TablaIvas)
                        If View(Z).Item("Iva") = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(Z).Item("Importe")
                            Exit For
                        End If
                    Next
                    If J > 5 Then OtroIva = OtroIva + View(Z).Item("Importe")
                    HayIvaOtrosConceptos = True
            End Select
        Next

        If HayIvaComision Then
            Grabado = Trunca(Grabado + Comision)
        Else : Exento = Trunca(Exento + Comision)
        End If
        If HayIvaInsumosProduccion Then
            Grabado = Trunca(Grabado + InsumosProduccion)
        Else : Exento = Trunca(Exento + InsumosProduccion)
        End If
        If HayIvaServiciosProduccion Then
            Grabado = Trunca(Grabado + ServiciosProduccion)
        Else : Exento = Trunca(Exento + ServiciosProduccion)
        End If
        If HayIvaOtrosConceptos Then
            Grabado = Trunca(Grabado + OtrosConceptos)
        Else : Exento = Trunca(Exento + OtrosConceptos)
        End If

        I = Z - 1

    End Sub
    Private Sub ArmaDtProvincia(ByVal Row As DataRow, ByVal Provincia As Integer)

        Dim RowsBusqueda() As DataRow
        Dim Row1 As DataRow

        If Row("Pais") <> 1 Then Provincia = 9999999

        RowsBusqueda = DtProvincias.Select("Provincia = " & Provincia)
        If RowsBusqueda.Length = 0 Then
            Row1 = DtProvincias.NewRow
            Row1("Provincia") = Provincia
            Row1("Grabado") = 0
            Row1("Exento") = 0
            Row1("Iva1") = 0
            Row1("Iva2") = 0
            Row1("Iva3") = 0
            Row1("Iva4") = 0
            Row1("Iva5") = 0
            Row1("OtroIva") = 0
            Row1("RetPerc") = 0
            Row1("Total") = 0
            DtProvincias.Rows.Add(Row1)
        Else
            Row1 = RowsBusqueda(0)
        End If

        Row1("Grabado") = Row1("Grabado") + Row("Grabado")
        Row1("Exento") = Row1("Exento") + Row("Exento")
        Row1("Iva1") = Row1("Iva1") + Row("Iva1")
        Row1("Iva2") = Row1("Iva2") + Row("Iva2")
        Row1("Iva3") = Row1("Iva3") + Row("Iva3")
        Row1("Iva4") = Row1("Iva4") + Row("Iva4")
        Row1("Iva5") = Row1("Iva5") + Row("Iva5")
        Row1("OtroIva") = Row1("OtroIva") + Row("OtroIva")
        Row1("RetPerc") = Row1("RetPerc") + Row("RetPerc")
        Row1("Total") = Row1("Total") + Row("Total")

    End Sub
    Private Function NombreProvincia(ByVal Clave As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Tipo = 31 AND Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: Tabla de Provincias.")
            End
        End Try

    End Function
    Private Sub SaltaComprobante(ByRef I As Integer)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Z As Integer = 0, J As Integer = 0

        For Z = I To View.Count - 1
            If ComproAnt <> View(Z).Item("TipoComprobante") & View(Z).Item("Rec") Then
                Exit For
            End If
        Next

        I = Z - 1

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Estilo As DataColumn = New DataColumn("Estilo")
        Estilo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estilo)

        Dim Tipo As DataColumn = New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Tipo)

        Dim Comprobante As DataColumn = New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comprobante)

        Dim PuntoDeVenta As DataColumn = New DataColumn("PuntoDeVenta")
        PuntoDeVenta.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(PuntoDeVenta)

        Dim ComprobanteDesde As DataColumn = New DataColumn("ComprobanteDesde")
        ComprobanteDesde.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ComprobanteDesde)

        Dim ComprobanteHasta As DataColumn = New DataColumn("ComprobanteHasta")
        ComprobanteHasta.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ComprobanteHasta)

        Dim Cartel As DataColumn = New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

        Dim Rec As DataColumn = New DataColumn("Rec")
        Rec.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Rec)

        Dim Fecha As DataColumn = New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Cuit As DataColumn = New DataColumn("Cuit")
        Cuit.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cuit)

        Dim Cliente As DataColumn = New DataColumn("Cliente")
        Cliente.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cliente)

        Dim Iva1 As DataColumn = New DataColumn("Iva1")
        Iva1.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Iva1)

        Dim Iva2 As DataColumn = New DataColumn("Iva2")
        Iva2.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Iva2)

        Dim Iva3 As DataColumn = New DataColumn("Iva3")
        Iva3.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Iva3)

        Dim Iva4 As DataColumn = New DataColumn("Iva4")
        Iva4.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Iva4)

        Dim Iva5 As DataColumn = New DataColumn("Iva5")
        Iva5.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Iva5)

        Dim OtroIva As DataColumn = New DataColumn("OtroIva")
        OtroIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(OtroIva)

        Dim Grabado As DataColumn = New DataColumn("Grabado")
        Grabado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Grabado)

        Dim Exento As DataColumn = New DataColumn("Exento")
        Exento.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Exento)

        Dim RetPerc As DataColumn = New DataColumn("RetPerc")
        RetPerc.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(RetPerc)

        Dim Total As DataColumn = New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Total)

        Dim Estado As DataColumn = New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Pais As DataColumn = New DataColumn("Pais")
        Pais.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Pais)

        Dim Comentario As DataColumn = New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comentario)

    End Sub
    Private Sub CreaDtProvincias()

        DtProvincias = New DataTable

        Dim Provincia As DataColumn = New DataColumn("Provincia")
        Provincia.DataType = System.Type.GetType("System.Int32")
        DtProvincias.Columns.Add(Provincia)

        Dim Iva1 As DataColumn = New DataColumn("Iva1")
        Iva1.DataType = System.Type.GetType("System.Double")
        DtProvincias.Columns.Add(Iva1)

        Dim Iva2 As DataColumn = New DataColumn("Iva2")
        Iva2.DataType = System.Type.GetType("System.Double")
        DtProvincias.Columns.Add(Iva2)

        Dim Iva3 As DataColumn = New DataColumn("Iva3")
        Iva3.DataType = System.Type.GetType("System.Double")
        DtProvincias.Columns.Add(Iva3)

        Dim Iva4 As DataColumn = New DataColumn("Iva4")
        Iva4.DataType = System.Type.GetType("System.Double")
        DtProvincias.Columns.Add(Iva4)

        Dim Iva5 As DataColumn = New DataColumn("Iva5")
        Iva5.DataType = System.Type.GetType("System.Double")
        DtProvincias.Columns.Add(Iva5)

        Dim OtroIva As DataColumn = New DataColumn("OtroIva")
        OtroIva.DataType = System.Type.GetType("System.Double")
        DtProvincias.Columns.Add(OtroIva)

        Dim Grabado As DataColumn = New DataColumn("Grabado")
        Grabado.DataType = System.Type.GetType("System.Double")
        DtProvincias.Columns.Add(Grabado)

        Dim Exento As DataColumn = New DataColumn("Exento")
        Exento.DataType = System.Type.GetType("System.Double")
        DtProvincias.Columns.Add(Exento)

        Dim RetPerc As DataColumn = New DataColumn("RetPerc")
        RetPerc.DataType = System.Type.GetType("System.Double")
        DtProvincias.Columns.Add(RetPerc)

        Dim Total As DataColumn = New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Double")
        DtProvincias.Columns.Add(Total)

    End Sub
    Private Sub LlenaCombosGrid()

        DtTipo = New DataTable

        Dim Codigo As DataColumn = New DataColumn("Codigo")
        Codigo.DataType = System.Type.GetType("System.Int32")
        DtTipo.Columns.Add(Codigo)

        Dim Nombre As DataColumn = New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTipo.Columns.Add(Nombre)
        '
        Dim Row As DataRow = DtTipo.NewRow
        'Row("Nombre") = "Facturas Rem. al Cliente"
        Row("Nombre") = "FRM"
        Row("Codigo") = 1
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "Facturas al Cliente"
        Row("Nombre") = "FC"
        Row("Codigo") = 2
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "NC al Cliente"
        Row("Nombre") = "NC"
        Row("Codigo") = 4
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "NDF al Cliente"
        Row("Nombre") = "NDF"
        Row("Codigo") = 5
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        ' Row("Nombre") = "NDF al Proveedor"
        Row("Nombre") = "NDF.P"
        Row("Codigo") = 6
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "NCF al Cliente"
        Row("Nombre") = "NCF"
        Row("Codigo") = 7
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "NCF al Proveedor"
        Row("Nombre") = "NCF.P"
        Row("Codigo") = 8
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "ND del Cliente"
        Row("Nombre") = "NDC"
        Row("Codigo") = 50
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "NC del Cliente"
        Row("Nombre") = "NCC"
        Row("Codigo") = 70
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "NVLP del Cliente"
        Row("Nombre") = "NVLP"
        Row("Codigo") = 100
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "Liq. del Proveedor"
        Row("Nombre") = "Li"
        Row("Codigo") = 200
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "Fact.TicKet"
        Row("Nombre") = "TicKet"
        Row("Codigo") = 44
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        Row("Nombre") = "Nota Cred.TicKet"
        Row("Codigo") = 54
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        Row("Nombre") = ""
        Row("Codigo") = 0
        '
        DtTipo.Rows.Add(Row)
        TipoComprobante.DataSource = DtTipo
        TipoComprobante.DisplayMember = "Nombre"
        TipoComprobante.ValueMember = "Codigo"

    End Sub
    Private Function CreaDtTipoIva() As DataTable

        Dim DtTipoIva As New DataTable

        Dim Codigo As DataColumn = New DataColumn("Codigo")
        Codigo.DataType = System.Type.GetType("System.Int32")
        DtTipoIva.Columns.Add(Codigo)

        Dim Nombre As DataColumn = New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTipoIva.Columns.Add(Nombre)
        '
        Dim Row As DataRow = DtTipoIva.NewRow
        Row("Nombre") = ""
        Row("Codigo") = 0
        DtTipoIva.Rows.Add(Row)
        '
        Row = DtTipoIva.NewRow
        Row("Nombre") = "INSCRIPTOS-A"
        Row("Codigo") = 1
        DtTipoIva.Rows.Add(Row)
        '
        Row = DtTipoIva.NewRow
        Row("Nombre") = "EXENTOS"
        Row("Codigo") = 2
        DtTipoIva.Rows.Add(Row)
        '
        Row = DtTipoIva.NewRow
        Row("Nombre") = "CONSUMIDOR FINAL"
        Row("Codigo") = 3
        DtTipoIva.Rows.Add(Row)
        '
        Row = DtTipoIva.NewRow
        Row("Nombre") = "EXPORTACION"
        Row("Codigo") = 4
        DtTipoIva.Rows.Add(Row)
        '
        Row = DtTipoIva.NewRow
        Row("Nombre") = "INSCRIPTOS-M"
        Row("Codigo") = 5
        DtTipoIva.Rows.Add(Row)
        '
        Row = DtTipoIva.NewRow
        Row("Nombre") = "MONOTRIBUTO"
        Row("Codigo") = 6
        DtTipoIva.Rows.Add(Row)
        '
        Row = DtTipoIva.NewRow
        Row("Nombre") = "TICKETS"
        Row("Codigo") = 9
        DtTipoIva.Rows.Add(Row)

        Return DtTipoIva

    End Function
    Public Sub HallaDatosClienteW(ByVal Factura As Double, ByRef Nombre As String, ByRef Cuit As Double, ByRef Pais As Integer, ByRef Provincia As Integer, ByRef ComprobanteDesde As Decimal, ByRef ComprobanteHasta As Decimal, ByRef TipoIva As Integer)

        Dim Dt As New DataTable

        Nombre = ""
        ComprobanteDesde = 0
        ComprobanteHasta = 0

        If Not Tablas.Read("SELECT C.Nombre,C.Cuit,C.TipoIva,C.Pais,C.Provincia,F.ComprobanteDesde,F.ComprobanteHasta FROM Clientes AS C INNER JOIN FacturasCabeza AS F ON C.Clave = F.Cliente WHERE F.Factura = " & Factura & ";", Conexion, Dt) Then Exit Sub
        If Dt.Rows.Count = 0 Then Exit Sub
        Nombre = Dt.Rows(0).Item("Nombre")
        Cuit = Dt.Rows(0).Item("Cuit")
        TipoIva = Dt.Rows(0).Item("TipoIva")
        Pais = Dt.Rows(0).Item("Pais")
        Provincia = Dt.Rows(0).Item("Provincia")
        ComprobanteDesde = Dt.Rows(0).Item("ComprobanteDesde")
        ComprobanteHasta = Dt.Rows(0).Item("ComprobanteHasta")
        If Dt.Rows(0).Item("Pais") <> 1 Then
            Cuit = HallaCuitPais(Dt.Rows(0).Item("Pais"))
            If Cuit < 0 Then
                Nombre = ""
            End If
        End If

        Dt.Dispose()

    End Sub
    Private Function MovimientoOk(ByVal Comprobante As Decimal, ByVal TipoIva As Integer) As Boolean

        Dim PuntoDeVenta As Integer = HallaPuntoDeVenta(Comprobante)

        If ComboPuntoDeVenta.SelectedValue <> 0 Then
            If ComboPuntoDeVenta.SelectedValue <> PuntoDeVenta Then Return False
        End If

        If ComboTipoIva.SelectedValue <> 0 Then
            If TipoIva <> ComboTipoIva.SelectedValue Then Return False
        End If

        Return True

    End Function
    Private Function HallaPuntoDeVenta(ByVal Comprobante As Decimal) As Integer

        Dim ComprobanteStr As String = Comprobante.ToString
        If ComprobanteStr.Length > 12 Then
            Return Val(Strings.Mid(ComprobanteStr, 2, 4))
        End If
        If ComprobanteStr.Length <= 12 Then
            Return Val(Strings.Left(ComprobanteStr, ComprobanteStr.Length - 8))
        End If

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Grabado" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Estado").Value) Then
                If Grid.Rows(e.RowIndex).Cells("Estado").Value = 3 Then
                    e.Value = "*** ANULADA *** "
                End If
            End If
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Estilo").Value) Then
                If Grid.Rows(e.RowIndex).Cells("Estilo").Value = 1 Then
                    Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Yellow
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            If Not IsDBNull(e.Value) Then
                If IsNumeric(e.Value) Then e.Value = NumeroEditado(e.Value)
            End If
            Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cuit" Then
            If Not IsDBNull(e.Value) Then
                If Grid.Rows(e.RowIndex).Cells("Pais").Value = 1 Then e.Value = Format(e.Value, "00-00000000-0")
                Exit Sub
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha1" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
            Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "PuntoDeVenta" Or Grid.Columns(e.ColumnIndex).Name = "ComprobanteDesde" Or Grid.Columns(e.ColumnIndex).Name = "ComprobanteHasta" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else
                    e.Value = FormatNumber(e.Value, 0)
                End If
            End If
            Exit Sub
        End If

        If IsNumeric(e.Value) Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If IsDBNull(Grid.CurrentRow.Cells("TipoComprobante").Value) Then Exit Sub
        If IsDBNull(Grid.CurrentRow.Cells("Comprobante").Value) Then Exit Sub

        Select Case Grid.CurrentRow.Cells("TipoComprobante").Value
            Case 1, 2, 44
                UnaFactura.PAbierto = True
                UnaFactura.PFactura = Grid.CurrentRow.Cells("Comprobante").Value
                UnaFactura.PBloqueaFunciones = True
                UnaFactura.ShowDialog()
            Case 4, 54
                UnaNotaCredito.PAbierto = True
                UnaNotaCredito.PNota = Grid.CurrentRow.Cells("Comprobante").Value
                UnaNotaCredito.PBloqueaFunciones = True
                UnaNotaCredito.ShowDialog()
                UnaNotaCredito.Dispose()
            Case 5, 7
                UnReciboDebitoCredito.PAbierto = True
                UnReciboDebitoCredito.PTipoNota = Grid.CurrentRow.Cells("TipoComprobante").Value
                UnReciboDebitoCredito.PNota = Grid.CurrentRow.Cells("Comprobante").Value
                UnReciboDebitoCredito.PBloqueaFunciones = True
                UnReciboDebitoCredito.ShowDialog()
            Case 50, 70
                UnReciboDebitoCredito.PAbierto = True
                UnReciboDebitoCredito.PTipoNota = Grid.CurrentRow.Cells("TipoComprobante").Value
                UnReciboDebitoCredito.PNota = Grid.CurrentRow.Cells("Rec").Value
                UnReciboDebitoCredito.PBloqueaFunciones = True
                UnReciboDebitoCredito.ShowDialog()
            Case 100
                UnaNVLP.PAbierto = True
                UnaNVLP.PLiquidacion = Grid.CurrentRow.Cells("Rec").Value
                UnaNVLP.PBloqueaFunciones = True
                UnaNVLP.ShowDialog()
                UnaNVLP.Dispose()
            Case 200, 210
                If LiquidacionTrucha(Conexion, Grid.CurrentRow.Cells("Rec").Value) Then
                    UnaLiquidacionContable.PAbierto = True
                    UnaLiquidacionContable.PLiquidacion = Grid.CurrentRow.Cells("Rec").Value
                    UnaLiquidacionContable.PBloqueaFunciones = True
                    UnaLiquidacionContable.ShowDialog()
                    UnaLiquidacionContable.Dispose()
                Else
                    UnaLiquidacion.PAbierto = True
                    UnaLiquidacion.PLiquidacion = Grid.CurrentRow.Cells("Rec").Value
                    UnaLiquidacion.PBloqueaFunciones = True
                    UnaLiquidacion.ShowDialog()
                    UnaLiquidacion.Dispose()
                End If
        End Select

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs)
        Exit Sub
    End Sub


End Class