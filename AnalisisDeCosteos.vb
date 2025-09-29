Public Class AnalisisDeCosteos
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim Sql As String
    Dim TotalGralConIva As Decimal
    Dim TotalGralSinIva As Decimal
    Private Sub AnalisisDeCosteos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        ComboNegocio.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;")
        Dim Row As DataRow = ComboNegocio.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboNegocio.DataSource.rows.add(Row)
        ComboNegocio.DisplayMember = "Nombre"
        ComboNegocio.ValueMember = "Clave"
        ComboNegocio.SelectedValue = 0
        With ComboNegocio
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        With ComboCosteo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        CreaDtGrid()


    End Sub
    Private Sub ListaConsumos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, "", "", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

        If e.KeyData = 112 Then
            Dim pa As New PrintGPantalla(Me)
            pa.Print()
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Sql = "SELECT Negocio,Costeo,IntFechaDesde,IntFechaHasta FROM Costeos WHERE "

        Dim SqlFecha As String
        SqlFecha = "IntFechaDesde >= " & Format(DateTimeDesde.Value, "yyyyMMdd") & " AND IntFechaDesde <= " & Format(DateTimeHasta.Value, "yyyyMMdd") & " "

        Dim SqlNegocio As String = ""
        If ComboNegocio.SelectedValue <> 0 Then
            SqlNegocio = "AND Negocio = " & ComboNegocio.SelectedValue & " "
        End If

        Dim SqlCosteo As String = ""
        If ComboCosteo.SelectedValue <> 0 Then
            SqlCosteo = "AND Costeo = " & ComboCosteo.SelectedValue & " "
        End If

        Sql = Sql & SqlFecha & SqlNegocio & SqlCosteo & ";"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub TextConsumo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

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
    Private Sub ComboNegocio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboNegocio.Validating

        If IsNothing(ComboNegocio.SelectedValue) Then ComboNegocio.SelectedValue = 0

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & ComboNegocio.SelectedValue & ";"
        ComboCosteo.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboCosteo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        ComboCosteo.DataSource.rows.add(Row)
        ComboCosteo.DisplayMember = "Nombre"
        ComboCosteo.ValueMember = "Costeo"
        ComboCosteo.SelectedValue = 0

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub RadioImportesiniva_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioImporteSinIva.CheckedChanged

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Grid.Columns("CostoEstConIva").Visible = Not Grid.Columns("CostoEstConIva").Visible
        Grid.Columns("CostoEstSinIva").Visible = Not Grid.Columns("CostoEstSinIva").Visible
        Grid.Columns("GastosConIva").Visible = Not Grid.Columns("GastosConIva").Visible
        Grid.Columns("GastosSinIva").Visible = Not Grid.Columns("GastosSinIva").Visible
        Grid.Columns("CostoRealConIva").Visible = Not Grid.Columns("CostoRealConIva").Visible
        Grid.Columns("CostoRealSinIva").Visible = Not Grid.Columns("CostoRealSinIva").Visible
        If RadioImporteTotal.Checked Then
            Label4.Text = "Total Gastos C/IVA"
            TextTotalGastos.Text = FormatNumber(TotalGralConIva)
        Else
            Label4.Text = "Total Gastos S/IVA"
            TextTotalGastos.Text = FormatNumber(TotalGralSinIva)
        End If

        If RadioImporteTotal.Checked Then
            RadioImporteTotal.ForeColor = Color.Red
            RadioImporteSinIva.ForeColor = Color.Black
        Else
            RadioImporteTotal.ForeColor = Color.Black
            RadioImporteSinIva.ForeColor = Color.Red
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LLenaGrid()

        DtGrid.Clear()

        Dim Dt As New DataTable

        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Negocio,Costeo"

        TotalGralConIva = 0
        TotalGralSinIva = 0

        For Each Row As DataRowView In View
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Negocio") = Row("Negocio")
            Row1("Costeo") = Row("Costeo")
            Row1("FechaDesde") = Row("IntFechaDesde")
            Row1("FechaHasta") = Row("IntFechaHasta")
            Row1("GastosConIva") = 0
            Row1("GastosSinIva") = 0
            Row1("CostoEstConIva") = 0
            Row1("CostoEstSinIva") = 0
            Row1("Kilos") = 0
            Row1("Cerrado") = False
            Dim Iva As Decimal
            If HallaCostosPrecioKilos(Row("Costeo"), Row1("GastosConIva"), Row1("GastosSinIva"), Row1("CostoEstSinIva"), Row1("Kilos"), Row1("Cerrado"), Iva) < 0 Then Me.Close() : Exit Sub
            Row1("CostoEstConIva") = Trunca(Row1("CostoEstSinIva") + Row1("CostoEstSinIva") * Iva / 100)
            If Row1("Kilos") <> 0 Then
                Row1("CostoRealConIva") = Row1("GastosConIva") / Row1("Kilos")
                Row1("CostoRealSinIva") = Row1("GastosSinIva") / Row1("Kilos")
            Else : Row1("CostoRealConIva") = 0
                Row1("CostoRealSinIva") = 0
            End If
            TotalGralConIva = TotalGralConIva + Row1("GastosConIva")
            TotalGralSinIva = TotalGralSinIva + Row1("GastosSinIva")
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        TextTotalGastos.Text = FormatNumber(TotalGralConIva, GDecimales)

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        Dt.Dispose()

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Negocio As New DataColumn("Negocio")
        Negocio.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Negocio)

        Dim Costeo As New DataColumn("Costeo")
        Costeo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Costeo)

        Dim Kilos As New DataColumn("Kilos")
        Kilos.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Kilos)

        Dim GastosConIva As New DataColumn("GastosConIva")
        GastosConIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(GastosConIva)

        Dim GastosSinIva As New DataColumn("GastosSinIva")
        GastosSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(GastosSinIva)

        Dim CostoEstConIva As New DataColumn("CostoEstConIva")
        CostoEstConIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CostoEstConIva)

        Dim CostoEstSinIva As New DataColumn("CostoEstSinIva")
        CostoEstSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CostoEstSinIva)

        Dim CostoRealConIva As New DataColumn("CostoRealConIva")
        CostoRealConIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CostoRealConIva)

        Dim CostoRealSinIva As New DataColumn("CostoRealSinIva")
        CostoRealSinIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CostoRealSinIva)

        Dim FechaDesde As New DataColumn("FechaDesde")
        FechaDesde.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(FechaDesde)

        Dim FechaHasta As New DataColumn("FechaHasta")
        FechaHasta.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(FechaHasta)

        Dim Cerrado As New DataColumn("Cerrado")
        Cerrado.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Cerrado)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Negocio.DataSource = Tablas.Leer("SELECT Nombre,Clave FROM Proveedores WHERE TipoOperacion = 4;")
        Row = Negocio.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        Negocio.DataSource.rows.add(Row)
        Negocio.DisplayMember = "Nombre"
        Negocio.ValueMember = "Clave"

        Costeo.DataSource = Tablas.Leer("SELECT Nombre,Costeo FROM Costeos;")
        Row = Costeo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        Costeo.DataSource.rows.add(Row)
        Costeo.DisplayMember = "Nombre"
        Costeo.ValueMember = "Costeo"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "FechaDesde" Or Grid.Columns(e.ColumnIndex).Name = "FechaHasta" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else : e.Value = e.Value.ToString.Substring(6, 2) & "/" & e.Value.ToString.Substring(4, 2) & "/" & e.Value.ToString.Substring(0, 4)
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "GastosConIva" Or Grid.Columns(e.ColumnIndex).Name = "GastosSinIva" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "CostoEstConIva" Or Grid.Columns(e.ColumnIndex).Name = "CostoEstSinIva" Or Grid.Columns(e.ColumnIndex).Name = "CostoRealConIva" Or Grid.Columns(e.ColumnIndex).Name = "CostoRealSinIva" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, 3)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Kilos" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, 0)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Consumo" Then
            If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
                UnConsumoDeInsumo.PAbierto = True
            Else : UnConsumoDeInsumo.PAbierto = False
            End If
            UnConsumoDeInsumo.PConsumo = Grid.CurrentCell.Value
            UnConsumoDeInsumo.ShowDialog()
            UnConsumoDeInsumo.Dispose()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

    End Sub

End Class