Public Class ListaRetencionesPercibidas
    ' 
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim Dt As DataTable
    Dim DtTipo As DataTable
    Dim DtTipoComprobante As DataTable
    Dim View As DataView
    Private Sub ListaRetencionesPercibidas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        ArmaTipoComprobante()

        DtTipo = New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 25 AND CodigoRetencion = 1;", Conexion, DtTipo) Then Me.Close() : Exit Sub
        Dim Row As DataRow = DtTipo.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        DtTipo.Rows.Add(Row)
        ComboTipo.DataSource = DtTipo
        ComboTipo.DisplayMember = "Nombre"
        ComboTipo.ValueMember = "Clave"
        ComboTipo.SelectedValue = 0
        With ComboTipo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboTipoComprobante.DataSource = DtTipoComprobante
        ComboTipoComprobante.DisplayMember = "Nombre"
        ComboTipoComprobante.ValueMember = "Codigo"
        ComboTipoComprobante.SelectedValue = 0
        With ComboTipoComprobante
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboProveedor, "", "Proveedores")
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboProveedor, "", "Proveedores")
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
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

        LlenaCombosGrid()

        CreaDtGrid()

    End Sub
    Private Sub ListaRetencionesPercibidas_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If IsNothing(ComboTipo.SelectedValue) Then ComboTipo.SelectedValue = 0
        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Hasta debe ser mayor o igual a Fecha Desde.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtGrid.Clear()

        Dim SqlFecha As String
        SqlFecha = "F.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaComprobante As String
        SqlFechaComprobante = "D.FechaComprobante >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND D.FechaComprobante < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlFechaContable As String
        SqlFechaContable = "F.FechaContable >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND F.FechaContable < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "
        Dim SqlContable As String = ""
        If CheckBoxSinContables.Checked Then SqlContable = " AND F.Tr = 0 "
        Dim SqlFechaOtrosProveedores As String = ""
        Dim FechaWW As String = "cast(F.Anio as varchar) + RIGHT('00' + CAST(F.Mes AS Varchar), 2) + '01'"
        SqlFechaOtrosProveedores = FechaWW & " >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND " & FechaWW & " < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim Sql As String
        Sql = "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,F.Nota As Comprobante,CAST(FLOOR(CAST(D.FechaComprobante AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.MedioPago,D.Importe,D.Comprobante AS Retencion,F.Nota AS Rec,'' AS CompStr FROM Clientes AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.TipoNota = 60 AND " & SqlFecha & SqlContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,F.Nota As Comprobante,CAST(FLOOR(CAST(D.FechaComprobante AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.MedioPago,D.Importe,D.Comprobante AS Retencion,F.Nota AS Rec,'' AS CompStr FROM Proveedores AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.TipoNota = 604 AND " & SqlFecha & SqlContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.MedioPago,D.Importe,F.ReciboOficial AS Retencion,F.Nota AS Rec,'' AS CompStr FROM Clientes AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago AS D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.ChequeRechazado = 0 AND (F.TipoNota = 50 or F.TipoNota = 70) AND " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.MedioPago,D.Importe,F.ReciboOficial AS Retencion,F.Nota AS Rec,'' AS CompStr FROM Proveedores AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago AS D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.ChequeRechazado = 0 AND(F.TipoNota = 500 or F.TipoNota = 700) AND " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT 1010 AS TipoComprobante,'' AS Nombre,0 AS Cuit,F.ReciboOficial As Comprobante,CAST(FLOOR(CAST(F.Fecha AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Concepto AS MedioPago,D.Importe,F.ReciboOficial AS Retencion,F.Movimiento AS Rec,'' AS CompStr FROM PrestamosMovimientoCabeza AS F INNER JOIN PrestamosMovimientoDetalle D ON F.Movimiento = D.Movimiento WHERE F.ChequeRechazado = 0 AND " & SqlFecha & _
                   " UNION ALL " & _
              "SELECT 5000 AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Impuesto AS MedioPago,D.Importe,F.ReciboOficial AS Retencion,F.Factura AS Rec,'' AS CompStr FROM Proveedores AS C INNER JOIN (FacturasProveedorCabeza AS F INNER JOIN FacturasProveedorDetalle AS D ON F.Factura = D.Factura) ON C.Clave = F.Proveedor WHERE " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT 800 AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Impuesto AS MedioPago,D.Importe,F.ReciboOficial AS Retencion,F.Liquidacion AS Rec,'' AS CompStr FROM Clientes AS C INNER JOIN (NVLPCabeza AS F INNER JOIN NVLPDetalle AS D ON F.Liquidacion = D.Liquidacion) ON C.Clave = F.Cliente WHERE " & SqlFechaContable & SqlContable & _
                   " UNION ALL " & _
              "SELECT 3000 AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Concepto AS MedioPago,D.Importe,F.ReciboOficial AS Retencion,F.Movimiento AS Rec,'' AS CompStr FROM Tablas AS C INNER JOIN (GastosBancarioCabeza AS F INNER JOIN GastosBancarioDetalle D ON F.Movimiento = D.Movimiento) ON C.Clave = F.Banco WHERE " & SqlFechaContable & _
                   " UNION ALL " & _
              "SELECT 10000 AS TipoComprobante,C.Nombre,C.Cuit,F.ReciboOficial As Comprobante,cast(F.Anio as varchar) + RIGHT('00' + CAST(F.Mes AS Varchar), 2) + '01' as Fecha,F.Estado,D.Concepto AS MedioPago,D.Importe,F.ReciboOficial AS Retencion,F.Factura AS Rec,F.Comprobante AS CompStr FROM OtrosProveedores AS C INNER JOIN (OtrasFacturasCabeza AS F INNER JOIN OtrasFacturasDetalle D ON F.Factura = D.Factura) ON C.Clave = F.Proveedor WHERE " & SqlFechaOtrosProveedores & ";"

        Dt = New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        View = New DataView
        View = Dt.DefaultView
        View.Sort = "MedioPago,Fecha,Comprobante"

        Dim RowsBusqueda() As DataRow
        Dim RowGrid As DataRow
        Dim Row As DataRowView
        Dim TipoAnt As Integer = 0
        Dim Total As Double = 0
        Dim SubTotal As Double = 0
        Dim DtPorProvincia As DataTable

        For I As Integer = 0 To View.Count - 1
            Row = View(I)
            RowsBusqueda = DtTipo.Select("Clave = " & Row("MedioPago"))
            If RowsBusqueda.Length <> 0 And MovimientoOk(Row) Then
                If TipoAnt <> Row("MedioPago") Then
                    If TipoAnt <> 0 Then
                        CortePorRetencion(SubTotal, "Sub-Total Retención")
                        Total = Total + SubTotal
                        SubTotal = 0
                    End If
                    TipoAnt = Row("MedioPago")
                End If
                RowGrid = DtGrid.NewRow
                RowGrid("Tipo") = Row("MedioPago")
                Select Case Row("TipoComprobante")
                    Case 10000
                        RowGrid("Comprobante") = NumeroEditado(Row("Comprobante"))
                    Case Else
                        RowGrid("Comprobante") = NumeroEditado(Row("Comprobante"))
                End Select
                If Row("TipoComprobante") = 50 Then RowGrid("Cartel") = "N.D. del Cliente"
                If Row("TipoComprobante") = 70 Then
                    RowGrid("Cartel") = "N.C. del Cliente"
                    '     Row("Importe") = -Row("Importe")
                End If
                If Row("TipoComprobante") = 500 Then RowGrid("Cartel") = "N.D. del Proveedor"
                If Row("TipoComprobante") = 700 Then RowGrid("Cartel") = "N.C. del Proveedor"
                If Row("TipoComprobante") = 100 Then RowGrid("Cartel") = "Liquidación Prov."
                If Row("TipoComprobante") = 600 Then RowGrid("Cartel") = "Orden de Pago"
                If Row("TipoComprobante") = 604 Then RowGrid("Cartel") = "Dev. del Proveedor"
                If Row("TipoComprobante") = 60 Then RowGrid("Cartel") = "Cobranza"
                If Row("TipoComprobante") = 3000 Then RowGrid("Cartel") = "Gasto Bancario"
                If Row("TipoComprobante") = 1010 Then RowGrid("Cartel") = "Cancelación Prestamo"
                If Row("TipoComprobante") = 10000 Then RowGrid("Cartel") = "Otras Facturas"
                If Row("TipoComprobante") = 5000 Then RowGrid("Cartel") = "Facturas del Proveedor"
                If Row("TipoComprobante") = 800 Then RowGrid("Cartel") = "N.V.L.P."
                RowGrid("TipoComprobante") = Row("TipoComprobante")
                RowGrid("Fecha") = Row("Fecha")
                RowGrid("Retencion") = Row("Retencion")
                RowGrid("Cuit") = Row("Cuit")
                RowGrid("RazonSocial") = Row("Nombre")
                RowGrid("Rec") = Row("Rec")
                RowGrid("Estado") = Row("Estado")
                '
                DtPorProvincia = New DataTable
                Dim SqlComprobante As String = ""
                Select Case Row("TipoComprobante")
                    Case 60
                        SqlComprobante = " AND Comprobante = " & Row("Retencion")
                End Select
                If Not Tablas.Read("SELECT Provincia,Importe FROM RecibosRetenciones WHERE TipoNota = " & Row("TipoComprobante") & " AND Nota = " & Row("Rec") & " AND Retencion = " & Row("MedioPago") & SqlComprobante & ";", Conexion, DtPorProvincia) Then
                    Me.Close()
                    Exit Sub
                End If
                Dim Signo As String = "+"
                If Row("TipoComprobante") = 700 Or Row("TipoComprobante") = 70 Then Signo = "-"
                Select Case DtPorProvincia.Rows.Count
                    Case 0
                        If Signo = "-" Then Row("Importe") = -Row("Importe")
                        RowGrid("Provincia") = 0
                        If Row("Estado") <> 3 Then
                            RowGrid("Importe") = Row("Importe")
                        Else
                            RowGrid("Importe") = 0
                        End If
                        DtGrid.Rows.Add(RowGrid)
                        SubTotal = SubTotal + RowGrid("Importe")
                    Case Else
                        RowGrid("Provincia") = DtPorProvincia.Rows(0).Item("Provincia")
                        If Signo = "-" Then DtPorProvincia.Rows(0).Item("Importe") = -DtPorProvincia.Rows(0).Item("Importe")
                        If Row("Estado") <> 3 Then
                            RowGrid("Importe") = DtPorProvincia.Rows(0).Item("Importe")
                        Else
                            RowGrid("Importe") = 0
                        End If
                        DtGrid.Rows.Add(RowGrid)
                        SubTotal = SubTotal + RowGrid("Importe")
                        If DtPorProvincia.Rows.Count > 1 Then
                            AgregaProvinciaAlGrid(RowGrid, DtPorProvincia, SubTotal, Row("Estado"), Row("TipoComprobante"), Signo)
                        End If
                End Select
            End If
        Next
        Total = Total + SubTotal
        CortePorRetencion(SubTotal, "Sub-Total Retención")
        CortePorRetencion(Total, "Total Gral.")

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonTXTParaAfip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTXTParaAfip.Click
        ''''''''''''  If PercepcionArba = 0 Then MsgBox("No se Encontro Código Percepción ARBA.") : Exit Sub

        UnTXTPerRetPercibida.PPercepcion = False
        UnTXTPerRetPercibida.PDesde = DateTimeDesde.Value
        UnTXTPerRetPercibida.PHasta = DateTimeHasta.Value
        UnTXTPerRetPercibida.PDtGrid = DtGrid
        UnTXTPerRetPercibida.ShowDialog()

    End Sub
    Private Sub ButtonExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, GNombreEmpresa & "    " & Date.Now, "", "Retenciones Percibidas - PERIODO :           " & Format(DateTimeDesde.Value, "dd/MM/yyyy") & "      -      " & Format(DateTimeHasta.Value, "dd/MM/yyyy"))

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
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboTipoComprobante_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipo.Validating

        If IsNothing(ComboTipo.SelectedValue) Then ComboTipo.SelectedValue = 0

    End Sub
    Private Sub ComboBanco_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboBanco.Validating

        If IsNothing(ComboBanco.SelectedValue) Then ComboBanco.SelectedValue = 0

    End Sub
    Private Sub ComboOtrosproveedores_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboOtrosProveedores.Validating

        If IsNothing(ComboOtrosProveedores.SelectedValue) Then ComboOtrosProveedores.SelectedValue = 0

    End Sub
    Private Sub CortePorRetencion(ByVal Total As Double, ByVal Cartel As String)

        Dim Row As DataRow = DtGrid.NewRow
        Row("Cartel") = Cartel
        Row("Importe") = Total
        Row("Estilo") = 1
        DtGrid.Rows.Add(Row)

    End Sub
    Private Sub AgregaProvinciaAlGrid(ByVal RowGridAnterior As DataRow, ByVal DtPorProvincia As DataTable, ByRef SubTotal As Double, ByVal Estado As Integer, ByVal TipoComprobante As Integer, ByVal Signo As String)

        Dim RowGrid As DataRow

        For I As Integer = 1 To DtPorProvincia.Rows.Count - 1
            RowGrid = DtGrid.NewRow
            For i2 As Integer = 0 To DtGrid.Columns.Count - 1
                RowGrid(i2) = RowGridAnterior(i2)
            Next
            RowGrid("Provincia") = DtPorProvincia.Rows(I).Item("Provincia")
            If Signo = "-" Then DtPorProvincia.Rows(I).Item("Importe") = -DtPorProvincia.Rows(I).Item("Importe")
            If Estado <> 3 Then
                RowGrid("Importe") = DtPorProvincia.Rows(I).Item("Importe")
            Else
                RowGrid("Importe") = 0
            End If
            DtGrid.Rows.Add(RowGrid)
            SubTotal = SubTotal + RowGrid("Importe")
        Next

    End Sub
    Private Function MovimientoOk(ByRef Row As DataRowView) As Boolean

        If Row("TipoComprobante") = 1010 Then
            HallaDatosPrestamo(Row("Rec"), Row("Cuit"), Row("Nombre"))
        End If

        If ComboTipo.SelectedValue <> 0 And ComboTipo.SelectedValue <> Row("MedioPago") Then Return False
        If ComboTipoComprobante.SelectedValue <> 0 And ComboTipoComprobante.SelectedValue <> Row("TipoComprobante") Then Return False
        If ComboProveedor.SelectedValue <> 0 And ComboProveedor.Text <> Row("Nombre") Then Return False
        If ComboCliente.SelectedValue <> 0 And ComboCliente.Text <> Row("Nombre") Then Return False
        If ComboBanco.SelectedValue <> 0 And ComboBanco.Text <> Row("Nombre") Then Return False
        If ComboOtrosProveedores.SelectedValue <> 0 And ComboOtrosProveedores.Text <> Row("Nombre") Then Return False

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Estilo As DataColumn = New DataColumn("Estilo")
        Estilo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estilo)

        Dim Tipo As DataColumn = New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Tipo)

        Dim Retencion As DataColumn = New DataColumn("Retencion")
        Retencion.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Retencion)

        Dim TipoComprobante As DataColumn = New DataColumn("TipoComprobante")
        TipoComprobante.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoComprobante)

        Dim Comprobante As DataColumn = New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comprobante)

        Dim Cartel As DataColumn = New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

        Dim Fecha As DataColumn = New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Cuit As DataColumn = New DataColumn("Cuit")
        Cuit.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cuit)

        Dim RazonSocial As DataColumn = New DataColumn("RazonSocial")
        RazonSocial.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(RazonSocial)

        Dim Provincia As DataColumn = New DataColumn("Provincia")
        Provincia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Provincia)

        Dim Importe As DataColumn = New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim Rec As DataColumn = New DataColumn("Rec")
        Rec.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Rec)

        Dim Estado As DataColumn = New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

    End Sub
    Private Sub ArmaTipoComprobante()

        DtTipoComprobante = New DataTable

        Dim Codigo As DataColumn = New DataColumn("Codigo")
        Codigo.DataType = System.Type.GetType("System.Int32")
        DtTipoComprobante.Columns.Add(Codigo)

        Dim Nombre As DataColumn = New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTipoComprobante.Columns.Add(Nombre)
        '
        Dim Row As DataRow
        '
        Row = DtTipoComprobante.NewRow
        Row("Nombre") = "Cob."
        Row("Codigo") = 60
        DtTipoComprobante.Rows.Add(Row)
        '
        Row = DtTipoComprobante.NewRow
        Row("Nombre") = "Dev. del Proveedor"
        Row("Codigo") = 604
        DtTipoComprobante.Rows.Add(Row)
        '
        Row = DtTipoComprobante.NewRow
        Row("Nombre") = "GsBa"
        Row("Codigo") = 3000
        DtTipoComprobante.Rows.Add(Row)
        '
        Row = DtTipoComprobante.NewRow
        Row("Nombre") = "C.Pres."
        Row("Codigo") = 1010
        DtTipoComprobante.Rows.Add(Row)
        '
        Row = DtTipoComprobante.NewRow
        Row("Nombre") = "Otr.Fact."
        Row("Codigo") = 10000
        DtTipoComprobante.Rows.Add(Row)
        '
        Row = DtTipoComprobante.NewRow
        Row("Nombre") = "Factura Prov."
        Row("Codigo") = 5000
        DtTipoComprobante.Rows.Add(Row)
        '
        Row = DtTipoComprobante.NewRow
        Row("Nombre") = ""
        Row("Codigo") = 0
        DtTipoComprobante.Rows.Add(Row)

    End Sub
    Private Sub LlenaCombosGrid()

        TipoComprobante.DataSource = DtTipoComprobante
        TipoComprobante.DisplayMember = "Nombre"
        TipoComprobante.ValueMember = "Codigo"

        Tipo.DataSource = DtTipo
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Clave"

        Provincia.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 31;")
        Dim Row As DataRow = Provincia.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Provincia.DataSource.Rows.Add(Row)
        Provincia.DisplayMember = "Nombre"
        Provincia.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            e.Value = FormatNumber(e.Value, GDecimales)
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Estilo").Value) Then
                If Grid.Rows(e.RowIndex).Cells("Estilo").Value = 1 Then
                    Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Yellow
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cartel" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Estado").Value) Then
                If Grid.Rows(e.RowIndex).Cells("Estado").Value = 3 Then
                    e.Value = "*** ANULADA *** "
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "NumeroRetencion" Then
            If Not IsDBNull(e.Value) Then
                If IsNumeric(e.Value) Then e.Value = NumeroEditado(e.Value)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cuit" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "00-00000000-0")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If


    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If IsDBNull(Grid.CurrentRow.Cells("Tipo").Value) Then Exit Sub
        If IsDBNull(Grid.CurrentRow.Cells("Comprobante").Value) Then Exit Sub

        Select Case Grid.CurrentRow.Cells("TipoComprobante").Value
            Case 5, 7, 6, 8, 50, 70, 500, 700
                UnReciboDebitoCredito.PNota = Grid.CurrentRow.Cells("Rec").Value
                UnReciboDebitoCredito.PTipoNota = Grid.CurrentRow.Cells("TipoComprobante").Value
                UnReciboDebitoCredito.PAbierto = True
                UnReciboDebitoCredito.PBloqueaFunciones = True
                UnReciboDebitoCredito.ShowDialog()
            Case 600, 60, 604
                UnRecibo.PAbierto = True
                UnRecibo.PTipoNota = Grid.CurrentRow.Cells("TipoComprobante").Value
                UnRecibo.PNota = Grid.CurrentRow.Cells("Rec").Value
                UnRecibo.PBloqueaFunciones = True
                UnRecibo.ShowDialog()
            Case 3000
                UnGastoBancario.PMovimiento = Grid.CurrentRow.Cells("Rec").Value
                UnGastoBancario.PAbierto = True
                UnGastoBancario.PBloqueaFunciones = True
                UnGastoBancario.ShowDialog()
                UnGastoBancario.Dispose()
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
            Case 1010
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
            Case 5000
                UnaFacturaProveedor.PFactura = Grid.CurrentRow.Cells("Rec").Value
                UnaFacturaProveedor.PAbierto = True
                UnaFacturaProveedor.PBloqueaFunciones = True
                UnaFacturaProveedor.ShowDialog()
                UnaFacturaProveedor.Dispose()
            Case 800
                UnaNVLP.PLiquidacion = Grid.CurrentRow.Cells("Rec").Value
                UnaNVLP.PAbierto = True
                UnaNVLP.PBloqueaFunciones = True
                UnaNVLP.ShowDialog()
                UnaNVLP.Dispose()
        End Select

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub

   
End Class