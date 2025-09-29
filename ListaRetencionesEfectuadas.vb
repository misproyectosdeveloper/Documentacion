Public Class ListaRetencionesEfectuadas
    ' 
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim Dt As DataTable
    Dim DtTipo As DataTable
    Dim DtTipoComprobante As DataTable
    Dim View As DataView
    Dim RetencionArba As Decimal
    Dim TotalArba As Decimal
    Private Sub ListaRetenciones_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        ArmaTipoComprobante()

        DtTipo = New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre,Formula FROM Tablas WHERE Tipo = 25 AND CodigoRetencion = 1;", Conexion, DtTipo) Then Me.Close() : Exit Sub
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

        LlenaCombosGrid()

        CreaDtGrid()

        If Not PermisoTotal Then
            CheckBoxSinContables.Visible = False
        Else
            CheckBoxSinContables.Visible = True
        End If

        RetencionArba = 0
        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtTipo.Select("Formula = 4")
        If RowsBusqueda.Length > 0 Then
            RetencionArba = RowsBusqueda(0).Item("Clave")
        End If

    End Sub
    Private Sub ListaRetenciones_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

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

        Dim Sql As String
        Sql = "SELECT 100 AS TipoComprobante,C.Clave AS Proveedor,C.Nombre,C.Cuit,F.Liquidacion As Comprobante,CAST(FLOOR(CAST(F.FechaContable AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.Concepto AS MedioPago,D.Importe,D.Liquidacion AS Retencion,F.Liquidacion AS Rec,F.Importe AS ImporteComprobante FROM Proveedores AS C INNER JOIN (LiquidacionCabeza AS F INNER JOIN LiquidacionDetalleConceptos D ON F.Liquidacion = D.Liquidacion) ON C.Clave = F.Proveedor WHERE " & SqlFechaContable & SqlContable & _
              " UNION ALL " & _
              "SELECT F.TipoNota AS TipoComprobante,C.Clave AS Proveedor,C.Nombre,C.Cuit,F.Nota As Comprobante,CAST(FLOOR(CAST(D.FechaComprobante AS FLOAT)) AS DATETIME) AS Fecha,F.Estado,D.MedioPago,D.Importe,D.Comprobante AS Retencion,F.Nota AS Rec,F.Importe AS ImporteComprobante FROM Proveedores AS C INNER JOIN (RecibosCabeza AS F INNER JOIN RecibosDetallePago D ON F.TipoNota = D.TipoNota AND F.Nota = D.Nota) ON C.Clave = F.Emisor WHERE F.TipoNota = 600 AND " & SqlFechaComprobante & SqlContable & ";"

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

        For I As Integer = 0 To View.Count - 1
            Row = View(I)
            RowsBusqueda = DtTipo.Select("Clave = " & Row("MedioPago"))
            If RowsBusqueda.Length <> 0 And MovimientoOk(Row) Then
                If TipoAnt <> Row("MedioPago") Then
                    If TipoAnt <> 0 Then
                        CortePorRetencion(SubTotal, "Sub-Total")
                        Total = Total + SubTotal
                        If TipoAnt = RetencionArba Then TotalArba = SubTotal
                        SubTotal = 0
                    End If
                    TipoAnt = Row("MedioPago")
                End If
                RowGrid = DtGrid.NewRow
                RowGrid("Formula") = RowsBusqueda(0).Item("Formula")
                RowGrid("Proveedor") = Row("Proveedor")
                RowGrid("ImporteComprobante") = Row("ImporteComprobante") 'Importe Comprobante.
                RowGrid("Tipo") = Row("MedioPago")
                If Row("TipoComprobante") = 100 Then RowGrid("Cartel") = "Liquidación Prov."
                If Row("TipoComprobante") = 600 Then RowGrid("Cartel") = "Orden de Pago"
                If Row("TipoComprobante") = 60 Then RowGrid("Cartel") = "Cobranza"
                If Row("TipoComprobante") = 300 Then RowGrid("Cartel") = "Gasto Bancario"
                If Row("TipoComprobante") = 1010 Then RowGrid("Cartel") = "Cancelación Prestamo"
                RowGrid("TipoComprobante") = Row("TipoComprobante")
                RowGrid("Comprobante") = Row("Comprobante")
                RowGrid("Fecha") = Row("Fecha")
                RowGrid("Retencion") = Row("Retencion")
                RowGrid("Cuit") = Row("Cuit")
                RowGrid("RazonSocial") = Row("Nombre")
                If Row("Estado") <> 3 Then
                    RowGrid("Importe") = Row("Importe") 'Importe de la Retencion.
                Else
                    RowGrid("Importe") = 0
                End If
                RowGrid("Estado") = Row("Estado")
                RowGrid("Estilo") = 0
                DtGrid.Rows.Add(RowGrid)
                SubTotal = SubTotal + RowGrid("Importe")
            End If
        Next
        Total = Total + SubTotal
        If TipoAnt = RetencionArba Then TotalArba = SubTotal
        CortePorRetencion(SubTotal, "Sub-Total")
        CortePorRetencion(Total, "Total")

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, GNombreEmpresa & "    " & Date.Now, "", "Retenciones Efectuadas - PERIODO :           " & Format(DateTimeDesde.Value, "dd/MM/yyyy") & "      -      " & Format(DateTimeHasta.Value, "dd/MM/yyyy"))

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonTXTParaARBA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTXTParaARBA.Click

        If RetencionArba = 0 Then
            MsgBox("No se Encontro Código Retención ARBA.") : Exit Sub
        End If

        UnTxtARBA.PPercepcion = False
        UnTxtARBA.PCodigo = RetencionArba
        UnTxtARBA.PTotal = TotalArba
        UnTxtARBA.PDesde = DateTimeDesde.Value
        UnTxtARBA.PHasta = DateTimeHasta.Value
        UnTxtARBA.PDtGrid = DtGrid
        UnTxtARBA.ShowDialog()

    End Sub
    Private Sub ButtonRetencionGananciasParaArba_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRetencionGananciasParaArba.Click

        UnTxtARBA.PPercepcion = False
        UnTxtARBA.PRetencionGanancias = True
        UnTxtARBA.PDesde = DateTimeDesde.Value
        UnTxtARBA.PHasta = DateTimeHasta.Value
        UnTxtARBA.PDtGrid = DtGrid
        UnTxtARBA.ShowDialog()

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
    Private Sub CortePorRetencion(ByVal Total As Double, ByVal Cartel As String)

        Dim Row As DataRow = DtGrid.NewRow
        Row("Cartel") = Cartel
        Row("Importe") = Total
        Row("Estilo") = 1
        DtGrid.Rows.Add(Row)

    End Sub
    Private Function MovimientoOk(ByRef Row As DataRowView) As Boolean

        If Row("TipoComprobante") = 1010 Then
            HallaDatosPrestamo(Row("Comprobante"), Row("Cuit"), Row("Nombre"))
        End If

        If ComboTipo.SelectedValue <> 0 And ComboTipo.SelectedValue <> Row("MedioPago") Then Return False
        If ComboTipoComprobante.SelectedValue <> 0 And ComboTipoComprobante.SelectedValue <> Row("TipoComprobante") Then Return False
        If ComboProveedor.SelectedValue <> 0 And ComboProveedor.Text <> Row("Nombre") Then Return False
        If ComboCliente.SelectedValue <> 0 And ComboCliente.Text <> Row("Nombre") Then Return False
        If ComboBanco.SelectedValue <> 0 And ComboBanco.Text <> Row("Nombre") Then Return False

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Proveedor As DataColumn = New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

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

        Dim Importe As DataColumn = New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim ImporteComprobante As DataColumn = New DataColumn("ImporteComprobante")
        ImporteComprobante.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ImporteComprobante)

        Dim Formula As DataColumn = New DataColumn("Formula")
        Formula.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Formula)

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
        Dim Row As DataRow = DtTipoComprobante.NewRow
        Row = DtTipoComprobante.NewRow
        Row("Nombre") = "OP"
        Row("Codigo") = 600
        DtTipoComprobante.Rows.Add(Row)
        '
        Row = DtTipoComprobante.NewRow
        Row("Nombre") = "Liq"
        Row("Codigo") = 100
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

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Or Grid.Columns(e.ColumnIndex).Name = "NumeroRetencion" Then
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
            Case 600, 60
                UnRecibo.PAbierto = True
                UnRecibo.PTipoNota = Grid.CurrentRow.Cells("TipoComprobante").Value
                UnRecibo.PNota = Grid.CurrentRow.Cells("Comprobante").Value
                UnRecibo.PBloqueaFunciones = True
                UnRecibo.ShowDialog()
            Case 300
                UnGastoBancario.PMovimiento = Grid.CurrentRow.Cells("Comprobante").Value
                UnGastoBancario.PAbierto = True
                UnGastoBancario.PBloqueaFunciones = True
                UnGastoBancario.ShowDialog()
                UnGastoBancario.Dispose()
            Case 100
                If LiquidacionTrucha(Conexion, Grid.CurrentRow.Cells("Comprobante").Value) Then
                    UnaLiquidacionContable.PAbierto = True
                    UnaLiquidacionContable.PLiquidacion = Grid.CurrentRow.Cells("Comprobante").Value
                    UnaLiquidacionContable.PBloqueaFunciones = True
                    UnaLiquidacionContable.ShowDialog()
                    UnaLiquidacionContable.Dispose()
                Else
                    UnaLiquidacion.PAbierto = True
                    UnaLiquidacion.PLiquidacion = Grid.CurrentRow.Cells("Comprobante").Value
                    UnaLiquidacion.PBloqueaFunciones = True
                    UnaLiquidacion.ShowDialog()
                    UnaLiquidacion.Dispose()
                End If
            Case 1010
                UnMovimientoPrestamo.PMovimiento = Grid.CurrentRow.Cells("Comprobante").Value
                UnMovimientoPrestamo.PAbierto = True
                UnMovimientoPrestamo.PBloqueaFunciones = True
                UnMovimientoPrestamo.ShowDialog()
                UnMovimientoPrestamo.Dispose()
        End Select

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub

   
   
End Class