Public Class ListaLotesFacturasProveedorAfectaLotes
    Dim SqlB As String
    Dim SqlN As String
    Private WithEvents bs As New BindingSource
    Dim Dt As DataTable
    Dim DtGrid As DataTable
    Dim TotalImporteB As Double
    Dim TotalImporteN As Double
    Dim TotalTotal As Double
    Dim LoteW As Integer
    Dim SecuenciaW As Integer
    Private Sub ListaFacturasAfectaLotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing
        Grid.Columns("CandadoN").DefaultCellStyle.NullValue = Nothing

        GeneraCombosGrid()

        LlenaCombo(ComboProveedor, "", "Proveedores")
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '';")
        Dim Row As DataRow = ComboAlias.DataSource.NewRow
        Row("Clave") = 0
        Row("Alias") = ""
        ComboAlias.DataSource.Rows.Add(Row)
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboProveedorLote, "", "Proveedores")
        ComboProveedorLote.SelectedValue = 0
        With ComboProveedorLote
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not PermisoTotal Then
            Grid.Columns("FacturaN").HeaderText = ""
            Grid.Columns("ImporteN").HeaderText = ""
            Grid.Columns("Total").HeaderText = ""
        End If

        MaskedFactura.Text = "000000000000"

    End Sub
    Private Sub ListaFacturas_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        ComboProveedor.Focus()

    End Sub
    Private Sub ListaFacturasProveedor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs)
        'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 as Operacion,C.* FROM FacturasProveedorCabeza AS C WHERE C.EsAfectaCostoLotes = 1 AND C.Estado = 1 "
        SqlN = "SELECT 2 as Operacion,C.* FROM FacturasProveedorCabeza AS C WHERE C.EsAfectaCostoLotes = 1 AND C.Estado = 1 "

        Dim SqlFecha As String
        SqlFecha = "AND C.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlFactura As String = ""
        If Val(MaskedFactura.Text) <> 0 Then
            SqlFactura = "AND C.Factura = " & Val(MaskedFactura.Text)
        End If

        Dim Proveedor As Integer = 0
        If ComboProveedor.SelectedValue <> 0 Then Proveedor = ComboProveedor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Proveedor = ComboAlias.SelectedValue

        Dim Sqlproveedor As String = ""
        If Proveedor > 0 Then
            Sqlproveedor = " AND C.Proveedor = " & Proveedor
        End If

        Dim SqlReciboOficial As String = ""
        If Val(MaskedReciboOficial.Text) <> 0 Then
            SqlFactura = "AND C.ReciboOficial = " & Val(MaskedReciboOficial.Text)
        End If

        SqlB = SqlB & SqlFecha & SqlFactura & Sqlproveedor & SqlReciboOficial & ";"
        SqlN = SqlN & SqlFecha & SqlFactura & Sqlproveedor & SqlReciboOficial & ";"

        CreaDtGrid()

        LLenaGrid()

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

    End Sub
    Private Sub RadioImportesiniva_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioImporteSinIva.CheckedChanged

        If RadioImporteSinIva.Checked Then
            Grid.Columns("Importe").HeaderText = "Importe S/I"
            Grid.Columns("ImporteN").HeaderText = "Importe S/I"
            Grid.Columns("Total").HeaderText = "Total S/I"
        Else
            Grid.Columns("Importe").HeaderText = "Importe C/I"
            Grid.Columns("ImporteN").HeaderText = "Importe C/I"
            Grid.Columns("Total").HeaderText = "Total C/I"
        End If

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Lotes Afectados por Facturas de Proveedores", "")

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
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

    End Sub
    Private Sub ComboProveedorLote_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedorLote.Validating

        If IsNothing(ComboProveedorLote.SelectedValue) Then ComboProveedorLote.SelectedValue = 0

    End Sub
    Private Sub GeneraCombosGrid()

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        ProveedorLote.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        ProveedorLote.DisplayMember = "Nombre"
        ProveedorLote.ValueMember = "Clave"

        Concepto.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = " & 29 & ";")
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

    End Sub
    Private Sub LLenaGrid()

        Dt = New DataTable

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Dt.Dispose()
            Exit Sub
        End If

        If PermisoTotal Then
            Grid.Columns("Candado").Visible = True
        Else : Grid.Columns("Candado").Visible = False
        End If

        Dim View As New DataView

        View = Dt.DefaultView
        View.Sort = "Factura"

        TotalImporteB = 0
        TotalImporteN = 0
        TotalTotal = 0

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRowView In View
            If Row("Operacion") = 1 Or (Row("Operacion") = 2 And Row("Rel")) = 0 Then
                Dim Row1 As DataRow = DtGrid.NewRow()
                Row1("Operacion") = Row("Operacion")
                Row1("Factura") = Row("Factura")
                Row1("ReciboOficial") = Row("ReciboOficial")
                Row1("Proveedor") = Row("Proveedor")
                Row1("Lote") = ""
                Row1("Fecha") = Row("Fecha")
                Row1("Concepto") = Row("ConceptoGasto")
                Row1("Liquidacion") = Row("Liquidacion")
                Row1("OperacionN") = 0
                Row1("FacturaN") = 0
                If Row("Operacion") = 1 And Row("Rel") Then
                    RowsBusqueda = Dt.Select("Operacion = 2 AND NRel = " & Row("Factura"))
                    If RowsBusqueda.Length <> 0 Then
                        Row1("OperacionN") = 2
                        Row1("LoteN") = 0
                        Row1("FacturaN") = RowsBusqueda(0).Item("Factura")
                    End If
                End If
                MuestraLotes(Row1)
            End If
        Next

        Dim Row2 As DataRow = DtGrid.NewRow()
        Row2("Lote") = "Totales          "
        Row2("Importe") = TotalImporteB
        Row2("ImporteN") = TotalImporteN
        Row2("Total") = TotalTotal
        DtGrid.Rows.Add(Row2)

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

    End Sub
    Private Sub MuestraLotes(ByVal Row2 As DataRow)

        Dim Dt As New DataTable
        Dim ConexionStr As String
        Dim Row As DataRow
        Dim EsPrimero As Boolean = True
        Dim ProveedorLote As Integer
        Dim FacturaParaCompro As Double
        If Row2("OperacionN") <> 0 Then
            ConexionStr = ConexionN
            FacturaParaCompro = Row2("FacturaN")
        Else
            If Row2("Operacion") = 1 Then
                ConexionStr = Conexion
                FacturaParaCompro = Row2("Factura")
            Else
                ConexionStr = ConexionN
                FacturaParaCompro = Row2("Factura")
            End If
        End If

        If Not Tablas.Read("SELECT * FROM ComproFacturados WHERE Factura = " & FacturaParaCompro & ";", ConexionStr, Dt) Then Me.Close() : Exit Sub
        For I As Integer = 0 To Dt.Rows.Count - 1
            Row = Dt.Rows(I)
            ProveedorLote = ProveedorDelLote(Row("Lote"), Row("Operacion"))
            If ComboProveedorLote.SelectedValue = 0 Or ComboProveedorLote.SelectedValue = ProveedorLote Then
                If LoteW = 0 Or (Row("Lote") = LoteW And Row("Secuencia") = SecuenciaW) Then
                    If EsPrimero Then
                        Row2("Lote") = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
                        If RadioImporteTotal.Checked Then
                            Row2("Importe") = Row("ImporteConIva")
                            TotalImporteB = TotalImporteB + Row2("Importe")
                        Else
                            Row2("Importe") = Row("ImporteSinIva")
                            TotalImporteB = TotalImporteB + Row2("Importe")
                        End If
                        If Row2("FacturaN") <> 0 Then
                            Row2("LoteN") = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
                            Row2("ImporteN") = ImporteFactura(Row2("FacturaN"), Row("Lote"), Row("Secuencia"))
                            Row2("Total") = Row2("Importe") + Row2("ImporteN")
                            TotalImporteN = TotalImporteN + Row2("ImporteN")
                            TotalTotal = TotalTotal + Row2("Total")
                        End If
                        Row2("ProveedorLote") = ProveedorLote
                        EsPrimero = False
                        DtGrid.Rows.Add(Row2)
                    Else
                        Dim Row1 As DataRow = DtGrid.NewRow()
                        Row1("Lote") = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
                        If RadioImporteTotal.Checked Then
                            Row1("Importe") = Row("ImporteConIva")
                            TotalImporteB = TotalImporteB + Row1("Importe")
                        Else
                            Row1("Importe") = Row("ImporteSinIva")
                            TotalImporteB = TotalImporteB + Row1("Importe")
                        End If
                        If Row2("FacturaN") <> 0 Then
                            Row1("LoteN") = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
                            Row1("ImporteN") = ImporteFactura(Row2("FacturaN"), Row("Lote"), Row("Secuencia"))
                            Row1("Total") = Row1("Importe") + Row1("ImporteN")
                            TotalImporteN = TotalImporteN + Row1("ImporteN")
                            TotalTotal = TotalTotal + Row1("Total")
                        End If
                        Row1("ProveedorLote") = ProveedorLote
                        DtGrid.Rows.Add(Row1)
                    End If
                End If
            End If
        Next

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As DataColumn = New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Factura As DataColumn = New DataColumn("Factura")
        Factura.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Factura)

        Dim ReciboOficial As DataColumn = New DataColumn("ReciboOficial")
        ReciboOficial.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ReciboOficial)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim ProveedorLote As New DataColumn("ProveedorLote")
        ProveedorLote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ProveedorLote)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Concepto)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Lote As DataColumn = New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Lote)

        Dim Liquidacion As New DataColumn("Liquidacion")
        Liquidacion.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Liquidacion)

        Dim Importe As DataColumn = New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim OperacionN As DataColumn = New DataColumn("OperacionN")
        OperacionN.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(OperacionN)

        Dim FacturaN As DataColumn = New DataColumn("FacturaN")
        FacturaN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(FacturaN)

        Dim LoteN As DataColumn = New DataColumn("LoteN")
        LoteN.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(LoteN)

        Dim ImporteN As DataColumn = New DataColumn("ImporteN")
        ImporteN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ImporteN)

        Dim Total As DataColumn = New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Total)

    End Sub
    Public Function ImporteFactura(ByVal Factura As Double, ByVal Lote As Integer, ByVal Secuencia As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ImporteSinIva FROM ComproFacturados WHERE Factura = " & Factura & " AND Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla de Facturas Proveedor.")
            End
        End Try

    End Function
    Public Function ProveedorDelLote(ByVal Lote As Integer, ByVal Operacion As Integer) As Integer

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Proveedor FROM IngresoMercaderiasCabeza WHERE Lote = " & Lote & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla de Facturas Proveedor.")
            End
        End Try

    End Function
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If ComboProveedor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Proveedor o Alias.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If Val(MaskedFactura.Text) <> 0 And Not MaskedOK(MaskedFactura) Then
            MsgBox("Factura Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedFactura.Text = "000000000000"
            Return False
        End If

        LoteW = 0
        SecuenciaW = 0
        If Val(MaskedLote.Text) <> 0 Then
            If Not ConsisteMaskedLote(MaskedLote.Text, LoteW, SecuenciaW) Then
                MaskedLote.Focus()
                Return False
            End If
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Factura" Then
            If Not IsDBNull(e.Value) Then
                Dim Numero As String = NumeroEditado(e.Value)
                e.Value = Numero
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FacturaN" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else : e.Value = NumeroEditado(e.Value)
                End If
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("OperacionN").Value = 1 Then Grid.Rows(e.RowIndex).Cells("CandadoN").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("OperacionN").Value = 2 Then Grid.Rows(e.RowIndex).Cells("CandadoN").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "ReciboOficial" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else : e.Value = NumeroEditado(e.Value)
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Liquidacion" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = Format(e.Value, "0000-00000000")
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "ImporteN" Or Grid.Columns(e.ColumnIndex).Name = "Total" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 2)
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Factura" Or Grid.Columns(e.ColumnIndex).Name = "FacturaN" Then
            Dim Factura As Double = 0
            Dim Abierto As Boolean

            Factura = Grid.CurrentCell.Value
            If Grid.Columns(e.ColumnIndex).Name = "Factura" Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Abierto = True
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Abierto = False
            End If
            If Grid.Columns(e.ColumnIndex).Name = "FacturaN" Then Abierto = False
            UnaFacturaProveedor.PBloqueaFunciones = True
            UnaFacturaProveedor.PFactura = Factura
            UnaFacturaProveedor.PCodigoFactura = 901
            UnaFacturaProveedor.PProveedor = Grid.Rows(e.RowIndex).Cells("Proveedor").Value
            UnaFacturaProveedor.PAbierto = Abierto
            UnaFacturaProveedor.ShowDialog()
            UnaFacturaProveedor.Dispose()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            Exit Sub
        End If

    End Sub
End Class