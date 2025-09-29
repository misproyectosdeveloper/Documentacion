Public Class ListaLibroIvaCompra
    Dim TotalGrabado As Double = 0
    Dim TotalExento As Double = 0
    Dim TotalOtroIva As Double = 0
    Dim TotalTotal As Double = 0
    Dim TotalRetPerc As Double = 0
    Dim TablaIvas(0) As ItemIvaReten
    Dim TablaTotalIvas(0) As ItemIvaReten
    '
    Private WithEvents bs As New BindingSource
    '
    Dim DtTipo As DataTable
    Dim Dt As DataTable
    Dim DtConceptosFacturas As DataTable
    Dim DtGastosGrabados As DataTable
    Dim DtGastosBancarios As DataTable
    Dim DtRetPerc As DataTable
    Dim View As DataView
    Dim InicioColumnaIva As Integer = 14
    '
    Dim Opcion1LibrosIva As Boolean
    Dim DtGrid As DataTable
    Private Sub ListaLibroIvaCompraNuevo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        LlenaCombo(ComboEmisor, "", "Proveedores")
        ComboEmisor.SelectedValue = 0
        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboCliente, "", "Clientes")
        ComboCliente.SelectedValue = 0
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboBanco, 26)
        ComboBanco.SelectedValue = 0
        With ComboBanco
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboOtrosProveedores, "", "OtrosProveedores")
        ComboOtrosProveedores.SelectedValue = 0
        With ComboOtrosProveedores
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        CreaDtGrid()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        ArmaTablaIvaConImporte(TablaIvas)
        ArmaTablaIvaConImporte(TablaTotalIvas)

        DtRetPerc = New DataTable
        If Not Tablas.Read("SELECT Clave FROM Tablas WHERE Tipo = 25;", Conexion, DtRetPerc) Then
            Me.Close()
            Exit Sub
        End If

        DtGastosGrabados = New DataTable
        If Not Tablas.Read("SELECT Clave,Activo2 AS Grabado,Activo3 FROM Tablas WHERE Tipo = 33 or Tipo = 32;", Conexion, DtGastosGrabados) Then
            Me.Close()
            Exit Sub
        End If

        DtGastosBancarios = ArmaConceptosParaGastosBancarios(1, True)

        DtConceptosFacturas = ArmaConceptosPagoProveedores(False, 1)

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
                Label8.Text = "Método de Calculo Según Opción 1."
            Case False
                Label8.Text = "Método de Calculo Según Opción 2."
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
        TotalTotal = 0
        TotalRetPerc = 0

        AceraTabla(TablaTotalIvas)

        DtGrid.Clear()

        Dim SqlFecha As String = ""
        SqlFecha = "F.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaContable As String = ""
        SqlFechaContable = "F.FechaContable >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.FechaContable < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaOficial As String = ""
        SqlFechaOficial = "F.FechaReciboOficial >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.FechaReciboOficial < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        '
        Dim SqlOpcionesFactura As String = ""
        If RadioReventa.Checked Then SqlOpcionesFactura = " AND F.EsReventa = 1 "
        If RadioOtras.Checked Then SqlOpcionesFactura = " AND F.EsSinComprobante = 1 "
        If RadioConOrdenCompra.Checked Then SqlOpcionesFactura = " AND F.EsInsumos = 1 "
        If RadioAfectaLotes.Checked Then SqlOpcionesFactura = " AND F.EsAfectaCostoLotes = 1 "

        Dim SqlContable As String = ""
        If CheckBoxSinContables.Checked Then SqlContable = " AND F.Tr = 0 "
        If CheckBoxSoloContables.Checked Then SqlContable = " AND F.Tr = 1 "
        If CheckBoxSinContables.Checked And CheckBoxSoloContables.Checked Then SqlContable = ""
        Dim SqlSecos As String = ""
        If CheckBoxesSecos.Checked Then SqlSecos = " AND Secos = 1 "

        Dim Sql As String
        Sql = "SELECT 1 AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial AS Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Impuesto,0 AS Neto,0 AS Iva,D.Importe,F.Cambio,F.Factura AS Rec,C.Pais,F.ConceptoGasto AS Gasto,F.Rendicion,'' AS CompStr,F.Comentario FROM Proveedores AS C INNER JOIN (FacturasProveedorCabeza AS F INNER JOIN FacturasProveedorDetalle AS D " & _
                      "ON F.Factura = D.Factura) ON F.Proveedor = C.Clave WHERE " & SqlFechaContable & SqlOpcionesFactura & SqlContable & SqlSecos & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,F.Nota As Comprobante,CAST(FLOOR(CAST(F.Fecha AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.MedioPago As Impuesto,D.Neto,Alicuota AS Iva,D.Importe,F.Cambio,F.Nota AS Rec,C.Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Comentario FROM Proveedores AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE (F.TipoNota = 6 or F.TipoNota = 8) AND F.EsElectronica = 0 AND " & SqlFecha & SqlContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,F.Nota As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.MedioPago As Impuesto,D.Neto,Alicuota AS Iva,D.Importe,F.Cambio,F.Nota AS Rec,C.Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Comentario FROM Proveedores AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE (F.TipoNota = 6 or F.TipoNota = 8) AND F.EsElectronica = 1 AND " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.MedioPago As Impuesto,D.Neto,D.Alicuota AS Iva,D.Importe,F.Cambio,F.Nota AS Rec,C.Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Comentario FROM Proveedores AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE (F.TipoNota = 500 or F.TipoNota = 700) AND " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.MedioPago As Impuesto,D.Neto,D.Alicuota AS Iva,D.Importe,F.Cambio,F.Nota AS Rec,C.Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Comentario FROM Clientes AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE (F.TipoNota = 50 or F.TipoNota = 70) AND " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT 300 AS TipoComprobante,C.Nombre,C.Cuit,F.Movimiento As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Concepto AS Impuesto,0 AS Neto,0 AS Iva,D.Importe AS Importe,Cambio,F.Movimiento AS Rec,1 AS Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Comentario FROM Tablas AS C INNER JOIN (GastosBancarioCabeza AS F INNER JOIN GastosBancarioDetalle D ON F.Movimiento = D.Movimiento) ON C.Clave = F.Banco WHERE " & SqlFechaContable & _
                   " UNION ALL " & _
              "SELECT 5000 AS TipoComprobante,'' AS Nombre,0 AS Cuit,F.Movimiento As Comprobante,CAST(FLOOR(CAST(F.Fecha AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Concepto AS Impuesto,0 AS Neto,0 AS Iva,D.Importe AS Importe,1 AS Cambio,F.Movimiento AS Rec,1 AS Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Comentario FROM PrestamosMovimientoCabeza AS F INNER JOIN PrestamosMovimientoDetalle D ON F.Movimiento = D.Movimiento WHERE D.Concepto <> 6 AND " & SqlFecha & _
                   " UNION ALL " & _
              "SELECT 200 AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Impuesto,0 AS Neto,Alicuota AS Iva,D.Importe,1 AS Cambio,F.Liquidacion AS Rec,C.Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Comentario FROM Clientes AS C INNER JOIN (NVLPCabeza AS F INNER JOIN NVLPDetalle D ON F.Liquidacion = D.Liquidacion) ON C.Clave = F.Cliente WHERE " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT 100 AS TipoComprobante,C.Nombre,C.Cuit,F.Liquidacion As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,0 AS Impuesto,F.Bruto AS Neto,F.Alicuota AS Iva,0 AS Importe,1 AS Cambio,F.Liquidacion AS Rec,C.Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,F.Comentario FROM Proveedores AS C INNER JOIN LiquidacionCabeza AS F ON C.Clave = F.Proveedor WHERE " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT 10000 AS TipoComprobante,C.Nombre,C.Cuit,F.Factura As Comprobante,CAST(FLOOR(CAST(F.Fecha AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Concepto AS Impuesto,0 AS Neto,0 AS Iva,D.Importe,1 AS Cambio,F.Factura AS Rec,C.Pais,0 AS Gasto,0 As Rendicion,F.Comprobante AS CompStr,F.Comentario FROM OtrosProveedores AS C INNER JOIN (OtrasFacturasCabeza AS F INNER JOIN OtrasFacturasDetalle D ON F.Factura = D.Factura) ON C.Clave = F.Proveedor WHERE " & SqlFecha & _
                           " UNION ALL " & _
              "SELECT 66 AS TipoComprobante,C.Nombre,C.Cuit,F.Nota As Comprobante,CAST(FLOOR(CAST(F.Fecha AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,0 AS Impuesto,F.Importe AS Neto,0 AS Iva,0 AS Importe,1 AS Cambio,F.Nota AS Rec,1 AS Pais,0 AS Gasto,0 As Rendicion,'' AS CompStr,'' AS Comentario FROM Proveedores AS C INNER JOIN RecuperoSenia AS F ON C.Clave = F.Proveedor WHERE " & SqlFecha & ";"

        Dt = New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        View = New DataView
        View = Dt.DefaultView
        View.Sort = "Fecha,Comprobante"

        'Saca tipocomprobante 66,100,300 si es solo contables.
        If Not CheckBoxSinContables.Checked And CheckBoxSoloContables.Checked Then
            For I As Integer = View.Count - 1 To 0 Step -1
                Select Case View(I).Item("TipoComprobante")
                    Case 66, 10000, 300, 5000
                        View(I).Delete()
                End Select
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
            Select Case View(I).Item("TipoComprobante")
                Case 6, 8
                    If Opcion1LibrosIva Then ListaLinea(I)
                Case 50, 70
                    If Not Opcion1LibrosIva Then ListaLinea(I)
                Case Else
                    ListaLinea(I)
            End Select
        Next
        CorteTotales()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, GNombreEmpresa & "    " & Date.Now, "", "DIARIO I.V.A. COMPRAS - PERIODO :           " & Format(DateTimeDesde.Value, "dd/MM/yyyy") & "      -      " & Format(DateTimeHasta.Value, "dd/MM/yyyy"))

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboTipoComprobante_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboTipoComprobante.SelectionChangeCommitted

        ComboTipoComprobante_Validating(Nothing, Nothing)

    End Sub
    Private Sub ComboTipoComprobante_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipoComprobante.Validating

        If IsNothing(ComboTipoComprobante.SelectedValue) Then ComboTipoComprobante.SelectedValue = 0

        If ComboTipoComprobante.SelectedValue = 2 Then
            Panel2.Visible = True
            RadioTodas.Checked = True
            CheckBoxEsSecos.Visible = True
        Else
            Panel2.Visible = False
            RadioTodas.Checked = True
            CheckBoxSinContables.Visible = False
            CheckBoxEsSecos.Visible = False
        End If

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

    End Sub
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0

    End Sub
    Private Sub ComboBanco_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboBanco.Validating

        If IsNothing(ComboBanco.SelectedValue) Then ComboBanco.SelectedValue = 0

    End Sub
    Private Sub ComboOtrosproveedores_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboOtrosProveedores.Validating

        If IsNothing(ComboOtrosProveedores.SelectedValue) Then ComboOtrosProveedores.SelectedValue = 0

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

        Dim Grabado As Double = 0
        Dim Exento As Double = 0
        Dim OtroIva As Double = 0
        Dim Total As Double = 0
        Dim RetPerc As Double = 0
        Dim TotalIva As Double = 0

        Dim TipoComprobante As Integer = 0

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

        TipoComprobante = View(I).Item("TipoComprobante")

        Dim Tipo As Integer = 0
        Select Case View(I).Item("TipoComprobante")
            Case 1
                Tipo = 2
                Row("Cartel") = "Factura"
            Case 6
                Tipo = 6
                Row("Cartel") = "N.Ded.Fin. al Proveedor"
            Case 8
                Tipo = 8
                Row("Cartel") = "N.Cred.Fin. al Proveedor"
            Case 50
                Tipo = 50
                Row("Cartel") = "N.Ded. del Cliente"
            Case 70
                Tipo = 70
                Row("Cartel") = "N.Cred. del Cliente"
            Case 500
                Tipo = 500
                Row("Cartel") = "N.Ded. del Proveedor"
            Case 700
                Tipo = 700
                Row("Cartel") = "N.Cred. del Proveedor"
            Case 100
                Tipo = 100
                Row("Cartel") = "Liq. Proveedor"
            Case 200
                Tipo = 200
                Row("Cartel") = "N.V.L.P."
            Case 300
                Tipo = 300
                Row("Cartel") = "Gastos Bancarios"
            Case 5000
                Tipo = 5000
                Row("Cartel") = "Cancelación Prestamo"
            Case 10000
                Tipo = 10000
                Row("Cartel") = "Otras Facturas"
            Case 66
                Tipo = 66
                Row("Cartel") = "Recupero Seña"
        End Select

        If ComboTipoComprobante.SelectedValue <> 0 And ComboTipoComprobante.SelectedValue <> Tipo Then Exit Sub
        If ComboEmisor.SelectedValue <> 0 And ComboEmisor.Text <> View(I).Item("Nombre") Then Exit Sub
        If ComboBanco.SelectedValue <> 0 And ComboBanco.Text <> View(I).Item("Nombre") Then Exit Sub
        If ComboOtrosProveedores.SelectedValue <> 0 And ComboOtrosProveedores.Text <> View(I).Item("Nombre") Then Exit Sub

        Row("Tipo") = Tipo
        Select Case View(I).Item("TipoComprobante")
            Case 10000
                Row("Comprobante") = View(I).Item("CompStr")
            Case 300
                Row("Comprobante") = FormatNumber(View(I).Item("Comprobante"), 0)
            Case Else
                Row("Comprobante") = NumeroEditado(View(I).Item("Comprobante"))
        End Select
        Row("Rec") = View(I).Item("Rec")
        Row("Fecha") = View(I).Item("Fecha")
        Row("Estado") = View(I).Item("Estado")
        Row("Cuit") = View(I).Item("Cuit")
        Row("Cliente") = View(I).Item("Nombre")
        Row("Gasto") = View(I).Item("Gasto")
        Row("Rendicion") = View(I).Item("Rendicion")
        If View(I).Item("TipoComprobante") = 5000 Then
            HallaDatosPrestamo(View(I).Item("Comprobante"), Row("Cuit"), Row("Cliente"))
        End If
        Row("Pais") = View(I).Item("Pais")
        Row("Comentario") = View(I).Item("Comentario")
        If View(I).Item("Estado") <> 3 Then
            Select Case View(I).Item("TipoComprobante")
                Case 100
                    TotalizaComprobanteLiquidacion(I, Grabado, Exento, TablaIvas, OtroIva)
                Case 200
                    TotalizaComprobanteNVLP(I, Grabado, Exento, TablaIvas, OtroIva, RetPerc)
                Case 1
                    TotalizaComprobanteFacturaProveedor(I, Grabado, Exento, TablaIvas, OtroIva, RetPerc)
                Case 6, 8, 500, 700, 50, 70
                    TotalizaComprobanteRecibo(I, Grabado, Exento, TablaIvas, OtroIva, RetPerc)
                Case 300
                    TotalizaComprobanteGastosBancario(I, Grabado, Exento, TablaIvas, OtroIva, RetPerc)
                Case 5000
                    TotalizaComprobanteCancelacionPrestamo(I, Grabado, Exento, TablaIvas, OtroIva, RetPerc)
                Case 10000
                    TotalizaComprobanteOtrasFacturas(I, Grabado, Exento, TablaIvas, OtroIva, RetPerc)
                Case 66
                    TotalizaComprobanteRecuperoSenia(I, Grabado, Exento, TablaIvas, OtroIva)
            End Select
            Row("Grabado") = Grabado
            Row("Exento") = Exento
            Row("RetPerc") = RetPerc
            Row("Iva1") = 0
            Row("Iva2") = 0
            Row("Iva3") = 0
            Row("Iva4") = 0
            Row("Iva5") = 0
            If UBound(TablaIvas) >= 1 Then Row("Iva1") = TablaIvas(1).Importe
            If UBound(TablaIvas) >= 2 Then Row("Iva2") = TablaIvas(2).Importe
            If UBound(TablaIvas) >= 3 Then Row("Iva3") = TablaIvas(3).Importe
            If UBound(TablaIvas) >= 4 Then Row("Iva4") = TablaIvas(4).Importe
            If UBound(TablaIvas) >= 5 Then Row("Iva5") = TablaIvas(5).Importe
            Row("OtroIVA") = OtroIva

            TotalIva = OtroIva

            Row("Total") = Grabado + Exento + OtroIva + RetPerc
            For I2 As Integer = 1 To UBound(TablaIvas)
                Row("Total") = Row("Total") + TablaIvas(I2).Importe
                TotalIva = TotalIva + TablaIvas(I2).Importe
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
        Else
            SaltaComprobante(I)
        End If

        If (TipoComprobante = 300 Or TipoComprobante = 5000) And Row("Total") = 0 Then Exit Sub
        If TipoComprobante = 10000 And TotalIva = 0 Then Exit Sub

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
    Private Sub TotalizaComprobanteRecibo(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double, ByRef RetPerc As Double)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Z As Integer = 0, J As Integer = 0
        Dim Cambio As Double = View(I).Item("Cambio")

        For Z = I To View.Count - 1
            If ComproAnt <> View(Z).Item("TipoComprobante") & View(Z).Item("Rec") Then
                Exit For
            End If
            If Cambio <> 1 Then View(Z).Item("Neto") = Trunca(View(Z).Item("Neto") * Cambio)
            Select Case View(I).Item("TipoComprobante")
                Case 6, 700, 70
                    View(Z).Item("Neto") = -1 * View(Z).Item("Neto")
            End Select
            ' 
            If View(Z).Item("Iva") <> 0 Then
                Grabado = Grabado + View(Z).Item("Neto")
                For J = 1 To UBound(TablaIvas)
                    If View(Z).Item("Iva") = TablaIvas(J).Alicuota Then
                        TablaIvas(J).Importe = TablaIvas(J).Importe + CalculaIva(1, View(Z).Item("Neto"), View(Z).Item("Iva"))
                        Exit For
                    End If
                Next
                If J > 5 Then OtroIva = OtroIva + CalculaIva(1, View(Z).Item("Neto"), View(Z).Item("Iva"))
            Else
                Dim RowsBusqueda() As DataRow
                RowsBusqueda = DtRetPerc.Select("Clave = " & View(Z).Item("Impuesto"))
                If RowsBusqueda.Length <> 0 Then
                    Select Case View(I).Item("TipoComprobante")
                        Case 6, 700, 70
                            RetPerc = RetPerc - View(Z).Item("Importe")
                        Case Else
                            RetPerc = RetPerc + View(Z).Item("Importe")
                    End Select
                Else
                    Exento = Exento + View(Z).Item("Neto")
                End If
            End If
        Next

        I = Z - 1

    End Sub
    Private Sub TotalizaComprobanteGastosBancario(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double, ByRef RetPerc As Double)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Cambio As Double = View(I).Item("Cambio")
        Dim Z As Integer = 0, J As Integer = 0
        Dim RowsBusqueda() As DataRow
        Dim BaseImponible As Decimal = 0

        For Z = I To View.Count - 1
            If ComproAnt <> View(Z).Item("TipoComprobante") & View(Z).Item("Rec") Then
                Exit For
            End If
            ' 
            If Cambio <> 1 Then View(Z).Item("Importe") = CalculaNeto(View(Z).Item("Importe"), Cambio)
            RowsBusqueda = DtGastosGrabados.Select("Clave = " & View(Z).Item("Impuesto"))
            If RowsBusqueda.Length <> 0 Then
                If Operador(RowsBusqueda(0).Item("Clave")) = 2 Then
                    If RowsBusqueda(0).Item("Grabado") Then
                        Grabado = Grabado + View(Z).Item("Importe")
                    Else
                        If RowsBusqueda(0).Item("Activo3") = 0 Then Exento = Exento + View(Z).Item("Importe")
                    End If
                End If
                If Operador(RowsBusqueda(0).Item("Clave")) = 1 Then
                    If RowsBusqueda(0).Item("Grabado") Then
                        Grabado = Grabado - View(Z).Item("Importe")
                    Else
                        If RowsBusqueda(0).Item("Activo3") = 0 Then Exento = Exento - View(Z).Item("Importe")
                    End If
                End If
            End If
            '
            RowsBusqueda = DtRetPerc.Select("Clave = " & View(Z).Item("Impuesto"))
            If RowsBusqueda.Length <> 0 Then
                RetPerc = RetPerc + View(Z).Item("Importe")
            End If
            '
            For J = 1 To UBound(TablaIvas)
                If View(Z).Item("Impuesto") = TablaIvas(J).Clave Then
                    If J <= 5 Then
                        TablaIvas(J).Importe = TablaIvas(J).Importe + View(Z).Item("Importe")
                        If TablaIvas(J).Alicuota <> 0 Then BaseImponible = BaseImponible + Trunca(View(Z).Item("Importe") * 100 / TablaIvas(J).Alicuota)
                    Else : OtroIva = OtroIva + View(Z).Item("Importe")
                    End If
                End If
            Next
        Next

        If BaseImponible <> 0 Then Grabado = BaseImponible '????????????????????????????

        I = Z - 1

    End Sub
    Private Sub TotalizaComprobanteCancelacionPrestamo(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double, ByRef RetPerc As Double)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Z As Integer = 0, J As Integer = 0
        Dim RowsBusqueda() As DataRow
        Dim BaseImponible As Decimal = 0

        For Z = I To View.Count - 1
            If ComproAnt <> View(Z).Item("TipoComprobante") & View(Z).Item("Rec") Then
                Exit For
            End If
            ' 
            RowsBusqueda = DtGastosGrabados.Select("Clave = " & View(Z).Item("Impuesto"))
            If RowsBusqueda.Length <> 0 Then
                If RowsBusqueda(0).Item("Grabado") Then
                    Grabado = Grabado + View(Z).Item("Importe")
                Else
                    Exento = Exento + View(Z).Item("Importe")
                End If
            End If
            '
            If View(Z).Item("Impuesto") = 7 Then Grabado = Grabado + View(Z).Item("Importe") 'Intereses
            If View(Z).Item("Impuesto") = 6 Then Exento = Exento + View(Z).Item("Importe") 'Capital a Cancelar. 
            '
            RowsBusqueda = DtRetPerc.Select("Clave = " & View(Z).Item("Impuesto"))
            If RowsBusqueda.Length <> 0 Then
                RetPerc = RetPerc + View(Z).Item("Importe")
            End If
            '
            For J = 1 To UBound(TablaIvas)
                If View(Z).Item("Impuesto") = TablaIvas(J).Clave Then
                    If J <= 5 Then
                        TablaIvas(J).Importe = TablaIvas(J).Importe + View(Z).Item("Importe")
                        If TablaIvas(J).Alicuota <> 0 Then BaseImponible = BaseImponible + Trunca(View(Z).Item("Importe") * 100 / TablaIvas(J).Alicuota)
                    Else : OtroIva = OtroIva + View(Z).Item("Importe")
                    End If
                End If
            Next
        Next

        If BaseImponible <> 0 Then Grabado = BaseImponible

        I = Z - 1

    End Sub
    Private Sub TotalizaComprobanteLiquidacion(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double)

        Dim J As Integer = 0

        If View(I).Item("Iva") <> 0 Then
            Grabado = Grabado + View(I).Item("Neto")
            For J = 1 To UBound(TablaIvas)
                If View(I).Item("Iva") = TablaIvas(J).Alicuota Then
                    TablaIvas(J).Importe = CalculaIva(1, View(I).Item("Neto"), View(I).Item("Iva"))
                    Exit For
                End If
            Next
            If J > 5 Then OtroIva = OtroIva + CalculaIva(1, View(I).Item("Neto"), View(I).Item("Iva"))
        Else
            Exento = Exento + View(I).Item("Neto")
        End If

    End Sub
    Private Sub TotalizaComprobanteFacturaProveedor(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double, ByRef RetPerc As Double)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Z As Integer = 0, J As Integer = 0
        Dim RowsBusqueda() As DataRow
        Dim Cambio As Decimal = View(I).Item("Cambio")
        Dim Impuesto As Integer = 0
        Dim BaseImponible As Decimal = 0

        For Z = I To View.Count - 1
            If ComproAnt <> View(Z).Item("TipoComprobante") & View(Z).Item("Rec") Then
                Exit For
            End If
            If Cambio <> 1 Then View(Z).Item("Importe") = Trunca(View(Z).Item("Importe") * Cambio)
            Impuesto = View(Z).Item("Impuesto")
            RowsBusqueda = DtRetPerc.Select("Clave = " & Impuesto)
            If RowsBusqueda.Length <> 0 Then Impuesto = -1
            Select Case Impuesto
                Case -1
                    RetPerc = RetPerc + View(Z).Item("Importe")
                Case 1
                    '   Grabado = View(Z).Item("Importe")
                Case 2
                    Exento = Exento + View(Z).Item("Importe")
                Case 10                      'Senia. 
                    Exento = Exento + View(Z).Item("Importe")
                Case Else
                    RowsBusqueda = DtConceptosFacturas.Select("Clave = " & Impuesto)
                    If RowsBusqueda(0).Item("Tipo") = 22 Then
                        For J = 1 To UBound(TablaIvas)
                            If Impuesto = TablaIvas(J).Clave Then
                                TablaIvas(J).Importe = View(Z).Item("Importe")
                                If TablaIvas(J).Alicuota <> 0 Then Grabado = Grabado + Trunca(View(Z).Item("Importe") * 100 / TablaIvas(J).Alicuota)
                                Exit For
                            End If
                        Next
                        If J > 5 Then
                            OtroIva = View(Z).Item("Importe")
                        End If
                    End If
            End Select
        Next

        I = Z - 1

    End Sub
    Private Sub BuscaAlicuotaComisionDescargaNVLP(ByVal I As Integer, ByRef AlicuotaComision As Double, ByRef AlicuotaDescarga As Double, ByRef AlicuotaFleteTerrestreas As Double, ByRef AlicuotaOtrosConceptos As Double)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Z As Integer = 0, J As Integer = 0

        For Z = I To View.Count - 1
            If ComproAnt <> View(Z).Item("TipoComprobante") & View(Z).Item("Rec") Then
                Exit For
            End If
            Select Case View(Z).Item("Impuesto")
                Case 6
                    AlicuotaComision = View(Z).Item("Iva")
                Case 8
                    AlicuotaDescarga = View(Z).Item("Iva")
                Case 11
                    AlicuotaFleteTerrestreas = View(Z).Item("Iva")
                Case 12
                    AlicuotaOtrosConceptos = View(Z).Item("Iva")
            End Select
        Next

    End Sub
    Private Sub TotalizaComprobanteNVLP(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double, ByRef RetPerc As Double)

        Dim AlicuotaComision As Double = 0
        Dim AlicuotaDescarga As Double = 0
        Dim AlicuotaFleteTerrestre As Double = 0
        Dim AlicuotaOtrosConceptos As Double = 0

        BuscaAlicuotaComisionDescargaNVLP(I, AlicuotaComision, AlicuotaDescarga, AlicuotaFleteTerrestre, AlicuotaOtrosConceptos)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Z As Integer = 0, J As Integer = 0
        Dim RowsBusqueda() As DataRow
        Dim Impuesto As Integer = 0

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
                    If AlicuotaComision <> 0 Then
                        Grabado = Grabado + View(Z).Item("Importe")
                    Else
                        Exento = Exento + View(Z).Item("Importe")
                    End If
                Case 7
                    If AlicuotaDescarga <> 0 Then
                        Grabado = Grabado + View(Z).Item("Importe")
                    Else
                        Exento = Exento + View(Z).Item("Importe")
                    End If
                Case 9
                    If AlicuotaFleteTerrestre <> 0 Then
                        Grabado = Grabado + View(Z).Item("Importe")
                    Else
                        Exento = Exento + View(Z).Item("Importe")
                    End If
                Case 10
                    If AlicuotaOtrosConceptos <> 0 Then
                        Grabado = Grabado + View(Z).Item("Importe")
                    Else
                        Exento = Exento + View(Z).Item("Importe")
                    End If
                Case 6
                    For J = 1 To UBound(TablaIvas)
                        If AlicuotaComision = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(Z).Item("Importe")
                            Exit For
                        End If
                    Next
                    If J > 5 Then OtroIva = OtroIva + View(Z).Item("Importe")
                Case 8, 11, 12
                    Dim Alicuota As Double = View(Z).Item("Iva")
                    For J = 1 To UBound(TablaIvas)
                        If Alicuota = TablaIvas(J).Alicuota Then
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(Z).Item("Importe")
                            Exit For
                        End If
                    Next
                    If J > 5 Then OtroIva = OtroIva + View(Z).Item("Importe")
            End Select
        Next

        I = Z - 1

    End Sub
    Private Sub TotalizaComprobanteOtrasFacturas(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double, ByRef RetPerc As Double)

        Dim ComproAnt As Double = View(I).Item("TipoComprobante") & View(I).Item("Rec")
        Dim Z As Integer = 0, J As Integer = 0
        Dim RowsBusqueda() As DataRow
        Dim Impuesto As Integer = 0

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
                Case Else
                    For J = 1 To UBound(TablaIvas)
                        If Impuesto = TablaIvas(J).Clave Then
                            TablaIvas(J).Importe = TablaIvas(J).Importe + View(Z).Item("Importe")
                            Exit For
                        End If
                    Next
            End Select
        Next

        I = Z - 1

    End Sub
    Private Sub TotalizaComprobanteRecuperoSenia(ByRef I As Integer, ByRef Grabado As Double, ByRef Exento As Double, ByRef TablaIvas() As ItemIvaReten, ByRef OtroIva As Double)

        Dim J As Integer = 0

        Exento = Exento - View(I).Item("Neto")

    End Sub
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

        Dim Rendicion As DataColumn = New DataColumn("Rendicion")
        Rendicion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Rendicion)

        Dim Comprobante As DataColumn = New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comprobante)

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

        Dim Gasto As DataColumn = New DataColumn("Gasto")
        Gasto.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Gasto)

        Dim Iva1 As DataColumn = New DataColumn("Iva1")
        Iva1.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Iva1)

        Dim Iva2 As DataColumn = New DataColumn("Iva2")
        Iva2.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Iva2)

        Dim Iva3 As DataColumn = New DataColumn("Iva3")
        Iva3.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Iva3)

        Dim Iva4 As DataColumn = New DataColumn("Iva4")
        Iva4.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Iva4)

        Dim Iva5 As DataColumn = New DataColumn("Iva5")
        Iva5.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Iva5)

        Dim OtroIva As DataColumn = New DataColumn("OtroIva")
        OtroIva.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(OtroIva)

        Dim Grabado As DataColumn = New DataColumn("Grabado")
        Grabado.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Grabado)

        Dim Exento As DataColumn = New DataColumn("Exento")
        Exento.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Exento)

        Dim RetPerc As DataColumn = New DataColumn("RetPerc")
        RetPerc.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(RetPerc)

        Dim Total As DataColumn = New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Total)

        Dim Comentario As DataColumn = New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comentario)

        Dim Estado As DataColumn = New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Pais As DataColumn = New DataColumn("Pais")
        Pais.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Pais)

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
        Row = DtTipo.NewRow
        'Row("Nombre") = "Facturas del Proveedor"
        Row("Nombre") = "FC"
        Row("Codigo") = 2
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "NDF al Proveedor"
        Row("Nombre") = "NDF"
        Row("Codigo") = 6
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "NCF al Proveedor"
        Row("Nombre") = "NCF"
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
        'Row("Nombre") = "ND del Proveedor"
        Row("Nombre") = "NDP"
        Row("Codigo") = 500
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "NC del Proveedor"
        Row("Nombre") = "NCP"
        Row("Codigo") = 700
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "Liq. del Proveedor"
        Row("Nombre") = "LI"
        Row("Codigo") = 100
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "NVLP"
        Row("Nombre") = "NVLP"
        Row("Codigo") = 200
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "Gastos Banc."
        Row("Nombre") = "GB"
        Row("Codigo") = 300
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "Prestamos"
        Row("Nombre") = "PRE"
        Row("Codigo") = 5000
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "Otras Facturas"
        Row("Nombre") = "O.Fac."
        Row("Codigo") = 10000
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        'Row("Nombre") = "Recupero Seña"
        Row("Nombre") = "Rec.Seña"
        Row("Codigo") = 66
        DtTipo.Rows.Add(Row)
        '
        Row = DtTipo.NewRow
        Row("Nombre") = ""
        Row("Codigo") = 0
        DtTipo.Rows.Add(Row)
        '
        TipoComprobante.DataSource = DtTipo
        TipoComprobante.DisplayMember = "Nombre"
        TipoComprobante.ValueMember = "Codigo"
        '
        Gasto.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = " & 29 & " ORDER BY Nombre;")
        Row = Gasto.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Gasto.DataSource.Rows.Add(Row)
        Gasto.DisplayMember = "Nombre"
        Gasto.ValueMember = "Clave"

    End Sub
    Private Function Operador(ByVal Concepto As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGastosBancarios.Select("Clave = " & Concepto)
        Return RowsBusqueda(0).Item("Operador")

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Grabado" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Estado").Value) Then
                If Grid.Rows(e.RowIndex).Cells("Estado").Value = 3 Then
                    e.Value = "** ANULADA **"
                End If
            End If
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Estilo").Value) Then
                If Grid.Rows(e.RowIndex).Cells("Estilo").Value = 1 Then
                    Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Yellow
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cuit" Then
            If Not IsDBNull(e.Value) Then
                If Grid.Rows(e.RowIndex).Cells("Pais").Value = 1 Then e.Value = Format(e.Value, "00-00000000-0")
                Exit Sub
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
            Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Rendicion" Then
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
            Case 2
                UnaFacturaProveedor.PAbierto = True
                UnaFacturaProveedor.PFactura = Grid.CurrentRow.Cells("Rec").Value
                UnaFacturaProveedor.PBloqueaFunciones = True
                UnaFacturaProveedor.ShowDialog()
                UnaFacturaProveedor.Dispose()
            Case 6, 8
                UnReciboDebitoCredito.PAbierto = True
                UnReciboDebitoCredito.PTipoNota = Grid.CurrentRow.Cells("TipoComprobante").Value
                UnReciboDebitoCredito.PNota = Grid.CurrentRow.Cells("Rec").Value
                UnReciboDebitoCredito.PBloqueaFunciones = True
                UnReciboDebitoCredito.ShowDialog()
            Case 500, 700
                UnReciboDebitoCredito.PAbierto = True
                UnReciboDebitoCredito.PTipoNota = Grid.CurrentRow.Cells("TipoComprobante").Value
                UnReciboDebitoCredito.PNota = Grid.CurrentRow.Cells("Rec").Value
                UnReciboDebitoCredito.PBloqueaFunciones = True
                UnReciboDebitoCredito.ShowDialog()
            Case 100
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
            Case 200
                UnaNVLP.PAbierto = True
                UnaNVLP.PLiquidacion = Grid.CurrentRow.Cells("Rec").Value
                UnaNVLP.PBloqueaFunciones = True
                UnaNVLP.ShowDialog()
                UnaNVLP.Dispose()
            Case 300
                UnGastoBancario.PMovimiento = Grid.CurrentRow.Cells("Rec").Value
                UnGastoBancario.PAbierto = True
                UnGastoBancario.PBloqueaFunciones = True
                UnGastoBancario.ShowDialog()
                UnGastoBancario.Dispose()
            Case 5000
                UnMovimientoPrestamo.PMovimiento = Grid.CurrentRow.Cells("Rec").Value
                UnMovimientoPrestamo.PAbierto = True
                UnMovimientoPrestamo.PBloqueaFunciones = True
                UnMovimientoPrestamo.ShowDialog()
                UnMovimientoPrestamo.Dispose()
            Case 10000
                UnaFacturaOtrosProveedores.PRecibo = Grid.CurrentRow.Cells("Rec").Value
                UnaFacturaOtrosProveedores.PAbierto = True
                UnaFacturaOtrosProveedores.PBloqueaFunciones = True
                UnaFacturaOtrosProveedores.ShowDialog()
                UnaFacturaOtrosProveedores.Dispose()
            Case 66
                UnRecuperoSenia.PNota = Grid.CurrentRow.Cells("Rec").Value
                UnRecuperoSenia.PAbierto = True
                UnRecuperoSenia.PBloqueaFunciones = True
                UnRecuperoSenia.ShowDialog()
                UnRecuperoSenia.Dispose()
        End Select

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub

    

    
End Class