Public Class ListaConsumos
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaConsumos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ComboInsumo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Insumos;")
        Dim Row As DataRow = ComboInsumo.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboInsumo.DataSource.rows.add(Row)
        ComboInsumo.DisplayMember = "Nombre"
        ComboInsumo.ValueMember = "Clave"
        ComboInsumo.SelectedValue = 0
        With ComboInsumo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboNegocio.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;")
        Row = ComboNegocio.DataSource.newrow
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

        If Not PermisoTotal Then
            Grid.Columns("CostoSinIva").Visible = False
        End If

        CreaDtGrid()

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaConsumos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        SqlB = "SELECT 1 As Operacion,* FROM ConsumosCabeza WHERE "
        SqlN = "SELECT 2 As Operacion,* FROM ConsumosCabeza WHERE "

        Dim SqlFecha As String
        SqlFecha = "Fecha BETWEEN '" & Format(DateTimeDesde.Value, "yyyyMMdd 00:00:00") & "' AND '" & Format(DateTimeHasta.Value, "yyyyMMdd 23:59:59") & "' "

        Dim SqlInsumo As String = ""
        If ComboInsumo.SelectedValue <> 0 Then
            SqlInsumo = "AND Insumo = " & ComboInsumo.SelectedValue & " "
        End If

        Dim SqlConsumo As String = ""
        If TextConsumo.Text <> "" Then
            SqlConsumo = "AND Consumo = " & Val(TextConsumo.Text) & " "
        End If

        Dim SqlNegocio As String = ""
        If ComboNegocio.SelectedValue <> 0 Then
            SqlNegocio = "AND Negocio = " & ComboNegocio.SelectedValue & " "
        End If

        Dim SqlCosteo As String = ""
        If ComboCosteo.SelectedValue <> 0 Then
            SqlCosteo = "AND Costeo = " & ComboCosteo.SelectedValue & " "
        End If

        SqlB = SqlB & SqlFecha & SqlInsumo & SqlConsumo & SqlNegocio & SqlCosteo & ";"
        SqlN = SqlN & SqlFecha & SqlInsumo & SqlConsumo & SqlNegocio & SqlCosteo & ";"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Consumos", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub TextConsumo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextConsumo.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ComboInsumo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboInsumo.Validating

        If IsNothing(ComboInsumo.SelectedValue) Then ComboInsumo.SelectedValue = 0

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
    Private Sub LLenaGrid()

        DtGrid.Clear()

        Dim Dt As New DataTable

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Consumo"

        Dim TotalConIva As Double
        Dim TotalSinIva As Double

        For Each Row As DataRowView In View
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Consumo") = Row("Consumo")
            Row1("Insumo") = Row("Insumo")
            Row1("Negocio") = Row("Negocio")
            Row1("Costeo") = Row("Costeo")
            Row1("Deposito") = Row("Deposito")
            Row1("Fecha") = Format(Row("Fecha"), "dd/MM/yyyy")
            Row1("Estado") = 0
            If Row("Estado") <> 1 Then Row1("Estado") = Row("Estado")
            Row1("CostoConIva") = Row("ImporteConIva")
            Row1("CostoSinIva") = Row("ImporteSinIva")
            TotalConIva = TotalConIva + Row("ImporteConIva")
            TotalSinIva = TotalSinIva + Row("ImporteSinIva")
            DtGrid.Rows.Add(Row1)
        Next
        Dim Row2 As DataRow = DtGrid.NewRow
        Row2("CostoConIva") = TotalConIva
        Row2("CostoSinIva") = TotalSinIva
        DtGrid.Rows.Add(Row2)

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        Dt.Dispose()

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Consumo As New DataColumn("Consumo")
        Consumo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Consumo)

        Dim Insumo As New DataColumn("Insumo")
        Insumo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Insumo)

        Dim TipoOperacion As New DataColumn("TipoOperacion")
        TipoOperacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoOperacion)

        Dim Negocio As New DataColumn("Negocio")
        Negocio.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Negocio)

        Dim Costeo As New DataColumn("Costeo")
        Costeo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Costeo)

        Dim Deposito As New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim CostoConIva As New DataColumn("CostoConIva")
        CostoConIva.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(CostoConIva)

        Dim CostoSinIva As New DataColumn("CostoSinIva")
        CostoSinIva.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(CostoSinIva)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Estado.DataSource = DtEstadoActivoYBaja()
        Row = Estado.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        Estado.DataSource.rows.add(Row)
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

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

        Deposito.DataSource = Tablas.Leer("SELECT Nombre,Clave FROM Tablas WHERE Tipo = 20;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

        Insumo.DataSource = Tablas.Leer("SELECT Nombre,Clave FROM Insumos;")
        Insumo.DisplayMember = "Nombre"
        Insumo.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If IsDBNull(e.Value) Or IsNothing(e.Value) Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Consumo" Then
            e.Value = Format(e.Value, "00000000")
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "CostoConIva" Or Grid.Columns(e.ColumnIndex).Name = "CostoSinIva" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
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