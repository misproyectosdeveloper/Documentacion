Public Class ListaLiquidaciones
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim Dt As DataTable
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaLiquidaciones_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        GeneraCombosGrid()
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing
        Grid.Columns("CandadoN").DefaultCellStyle.NullValue = Nothing

        ComboProveedor.DataSource = Tablas.Leer("SELECT Nombre,Clave FROM Proveedores;")
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = ProveedoresDeFrutasAlias()
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PermisoTotal Then
            Grid.Columns("CandadoN").Visible = True
            Grid.Columns("LiquidacionN").Visible = True
            Grid.Columns("ImporteN").Visible = True
            Grid.Columns("Total").Visible = True
        Else
            Grid.Columns("CandadoN").Visible = False
            Grid.Columns("LiquidacionN").Visible = False
            Grid.Columns("ImporteN").Visible = False
            Grid.Columns("Total").Visible = False
        End If

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If ComboProveedor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Proveedor o Alias.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        SqlB = "SELECT 1 as Operacion,* FROM LiquidacionCabeza "
        SqlN = "SELECT 2 as Operacion,* FROM LiquidacionCabeza "

        Dim SqlLiquidacion As String
        If TextLiquidacion.Text <> "" Then
            SqlLiquidacion = "WHERE Liquidacion = " & TraeNumero(TextLiquidacion.Text) & " "
        Else : SqlLiquidacion = "WHERE Liquidacion LIKE '%' "
        End If

        Dim Proveedor As Integer = 0
        If ComboProveedor.SelectedValue <> 0 Then Proveedor = ComboProveedor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Proveedor = ComboAlias.SelectedValue

        Dim SqlProveedor As String = ""
        If Proveedor > 0 Then
            SqlProveedor = "AND Proveedor = " & Proveedor & " "
        End If

        Dim SqlFecha As String
        SqlFecha = "AND FechaContable BETWEEN '" & Format(DateTimeDesde.Value, "yyyyMMdd 00:00:00") & "' AND '" & Format(DateTimeHasta.Value, "yyyyMMdd 23:59:59") & "' "

        Dim SqlEstado As String = ""
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND LiquidacionCabeza.Estado = " & ComboEstado.SelectedValue & " "
        End If
        '
        SqlB = SqlB & SqlLiquidacion & SqlProveedor & SqlFecha & SqlEstado & ";"
        SqlN = SqlN & SqlLiquidacion & SqlProveedor & SqlFecha & SqlEstado & " AND Nrel = 0;"

        CreaDtGrid()
        BorraGrid(Grid)

        If Not LLenaGrid() Then Me.Close() : Exit Sub

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub TextLiquidacion_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextLiquidacion.KeyPress

        If Asc(e.KeyChar) = 13 Then TextLiquidacion_Validating(Nothing, Nothing) : Exit Sub
        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextLiquidacion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextLiquidacion.Validating

        If TextLiquidacion.Text = "" Then Exit Sub

        Dim MiArray() As String
        MiArray = Split(TextLiquidacion.Text, "-")
        Dim Entrada As String = MiArray(0)
        If MiArray.Length = 2 Then Entrada = MiArray(0) & MiArray(1)
        If Val(Entrada) > 999999999999 Or Val(Entrada) < 100000000 Then
            MsgBox("Error Numero de Liquidación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextLiquidacion.Text = ""
        Else : TextLiquidacion.Text = Format(Val(Entrada), "0000-00000000")
        End If

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
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Liquidaciones Desde el " & DateTimeDesde.Text & "  Hasta el " & DateTimeHasta.Text, "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function LLenaGrid() As Boolean

        Dt = New DataTable

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Return False
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Liquidacion"

        Dim DtAux As New DataTable
        Dim Sql As String = ""
        Dim Total As Double = 0
        Dim LiquidacionN As Double = 0
        Dim NetoN As Double = 0
        Dim Cartel As String

        For Each Row As DataRowView In View
            If Row("Tr") And PermisoTotal Then
                Cartel = "CONTABLE"
            Else : Cartel = ""
            End If
            Total = 0
            LiquidacionN = 0
            NetoN = 0
            If Row("Operacion") = 1 And Row("Rel") = True And PermisoTotal Then
                Total = Row("Neto")
                Sql = "SELECT Liquidacion,Neto,Estado FROM LiquidacionCabeza WHERE Nrel = " & Row("Liquidacion") & ";"
                DtAux.Clear()
                If Not Tablas.Read(Sql, ConexionN, DtAux) Then Return False
                If DtAux.Rows.Count <> 0 Then
                    Dim Row2 As DataRow
                    Row2 = DtAux.Rows(0)
                    LiquidacionN = Row2("Liquidacion")
                    NetoN = Row2("Neto")
                    Total = Total + Row2("neto")
                End If
                AgregaADtGrid(Row("Operacion"), Row("Liquidacion"), Row("Interno"), Row("Proveedor"), Row("FechaContable"), Row("Neto"), Row("Estado"), LiquidacionN, NetoN, Total, Cartel, Row("Tr"))
            Else
                AgregaADtGrid(Row("Operacion"), Row("Liquidacion"), Row("Interno"), Row("Proveedor"), Row("FechaContable"), Row("Neto"), Row("Estado"), 0, 0, 0, Cartel, Row("Tr"))
            End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()
        DtAux.Dispose()

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As DataColumn = New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Liquidacion As DataColumn = New DataColumn("Liquidacion")
        Liquidacion.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Liquidacion)

        Dim Interno As DataColumn = New DataColumn("Interno")
        Interno.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Interno)

        Dim Tr As DataColumn = New DataColumn("Tr")
        Tr.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Tr)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Importe As DataColumn = New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim LiquidacionN As DataColumn = New DataColumn("LiquidacionN")
        LiquidacionN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(LiquidacionN)

        Dim ImporteN As DataColumn = New DataColumn("ImporteN")
        ImporteN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ImporteN)

        Dim Total As DataColumn = New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Total)

        Dim Cartel As DataColumn = New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

    End Sub
    Private Sub AgregaADtGrid(ByVal Operacion As Integer, ByVal Liquidacion As Double, ByVal Interno As Double, ByVal Proveedor As Integer, ByVal Fecha As DateTime, ByVal Importe As Double, _
                          ByVal Estado As Integer, ByVal LiquidacionN As Double, ByVal ImporteN As Double, ByVal Total As Double, ByVal Cartel As String, ByVal Tr As Boolean)

        If Tr And Not PermisoTotal Then Exit Sub

        Dim Row As DataRow

        Row = DtGrid.NewRow()
        Row("Operacion") = Operacion
        Row("Tr") = Tr
        Row("Liquidacion") = Liquidacion
        Row("Interno") = Val(Strings.Right(Interno.ToString, 9))
        Row("Proveedor") = Proveedor
        Row("Cartel") = Cartel
        Row("Fecha") = Fecha
        Row("Importe") = Importe
        Row("Estado") = 0
        If Estado <> 1 Then Row("Estado") = Estado
        Row("LiquidacionN") = LiquidacionN
        Row("ImporteN") = ImporteN
        Row("Total") = Total
        DtGrid.Rows.Add(Row)

    End Sub
    Private Sub GeneraCombosGrid()

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Liquidacion" Then
            If Not IsNothing(e.Value) Then
                e.Value = NumeroEditado(e.Value)
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
                        Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    Else : Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                    End If
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Importe").Value) Then
                e.Value = FormatNumber(e.Value, 2, True, True, True)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LiquidacionN" Then
            If Not IsNothing(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = NumeroEditado(e.Value)
                Else : e.Value = ""
                End If
                If Grid.Rows(e.RowIndex).Cells("LiquidacionN").Value <> 0 Then
                    Grid.Rows(e.RowIndex).Cells("CandadoN").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "ImporteN" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("ImporteN").Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 2, True, True, True)
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Total" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Total").Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 2, True, True, True)
                Else : e.Value = ""
                End If
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Liquidacion" Then
            Dim Abierto As Boolean
            If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
                Abierto = True
            Else
                Abierto = False
            End If
            If Grid.CurrentRow.Cells("Tr").Value Then
                UnaLiquidacionContable.PLiquidacion = Grid.CurrentCell.Value
                UnaLiquidacionContable.PAbierto = Abierto
                UnaLiquidacionContable.ShowDialog()
                If UnaLiquidacionContable.PActualizacionOk Then ButtonAceptar_Click(Nothing, Nothing)
                UnaLiquidacionContable.Dispose()
            Else
                UnaLiquidacion.PLiquidacion = Grid.CurrentCell.Value
                UnaLiquidacion.PAbierto = Abierto
                UnaLiquidacion.ShowDialog()
                If UnaLiquidacion.PActualizacionOk Then ButtonAceptar_Click(Nothing, Nothing)
                UnaLiquidacion.Dispose()
            End If
        End If
        If Grid.Columns(e.ColumnIndex).Name = "LiquidacionN" Then
            If Grid.CurrentCell.Value = 0 Then Exit Sub
            UnaLiquidacion.PLiquidacion = Grid.CurrentCell.Value
            UnaLiquidacion.PAbierto = False
            UnaLiquidacion.ShowDialog()
            If UnaLiquidacion.PActualizacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            UnaLiquidacion.Dispose()
        End If

    End Sub

End Class