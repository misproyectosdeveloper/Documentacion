Public Class ListaNVLP
    Private WithEvents bs As New BindingSource
    '
    Dim Dt As DataTable
    Dim DtFormasPago As DataTable
    Dim DtGrid As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaNVLP_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing
        Grid.Columns("CandadoN").DefaultCellStyle.NullValue = Nothing

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboCliente.DataSource = Nothing
        ComboCliente.DataSource = Tablas.Leer("Select Clave,Nombre From Clientes;")
        Dim Row As DataRow = ComboCliente.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboCliente.DataSource.Rows.Add(Row)
        ComboCliente.DisplayMember = "Nombre"
        ComboCliente.ValueMember = "Clave"
        ComboCliente.SelectedValue = 0
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Checked = False
            If Not PermisoTotal Then
                Grid.Columns("LiquidacionN").HeaderText = ""
                Grid.Columns("ImporteN").HeaderText = ""
                Grid.Columns("Total").HeaderText = ""
            End If
        Else
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        GeneraCombosGrid()

        CreaDtGrid()

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 AS Operacion,C.* FROM NVLPCabeza AS C WHERE "
        SqlN = "SELECT 2 AS Operacion,C.* FROM NVLPCabeza AS C WHERE "

        Dim SqlFecha As String
        SqlFecha = "C.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlCliente As String = ""
        If ComboCliente.SelectedValue <> 0 Then
            SqlCliente = " AND C.Cliente = " & ComboCliente.SelectedValue
        End If

        Dim SqlEstado As String
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND C.Estado = " & ComboEstado.SelectedValue & " "
        End If

        SqlB = SqlB & SqlFecha & SqlCliente & SqlEstado & ";"
        SqlN = SqlN & SqlFecha & SqlCliente & SqlEstado & ";"

        LLenaGrid()

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        Grid.Focus()

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
    Private Sub ComboDestino_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub TextMes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "N.V.L.P. Desde el " & DateTimeDesde.Text & "  Hasta el " & DateTimeHasta.Text, "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LLenaGrid()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        Dt = New DataTable

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked = True Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Liquidacion"

        DtGrid.Clear()
        Dim Row1 As DataRow
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRowView In View
            If Row("Operacion") = 1 Or (Row("Operacion") = 2 And Row("Rel")) = 0 Then
                Row1 = DtGrid.NewRow
                Row1("Operacion") = Row("Operacion")
                Row1("LiquidacionB") = Row("Liquidacion")
                Row1("Cliente") = Row("Cliente")
                Row1("ReciboOficial") = Row("ReciboOficial")
                Row1("Fecha") = Format(Row("Fecha"), "dd/MM/yyyy 00:00:00")
                Row1("ImporteB") = Row("Importe")
                Row1("Estado") = Row("Estado")
                If Row("Estado") <> 3 Then Row1("Estado") = 0
                Row1("Total") = 0
                Row1("ImporteN") = 0
                Row1("OperacionN") = 0
                Row1("LiquidacionN") = 0
                If Row("Operacion") = 1 And Row("Rel") Then
                    RowsBusqueda = Dt.Select("Operacion = 2 AND NRel = " & Row("Liquidacion"))
                    If RowsBusqueda.Length <> 0 Then
                        Row1("OperacionN") = 2
                        Row1("LiquidacionN") = RowsBusqueda(0).Item("Liquidacion")
                        Row1("ImporteN") = RowsBusqueda(0).Item("Importe")
                        Row1("Total") = Row1("ImporteB") + Row1("ImporteN")
                    End If
                End If
                DtGrid.Rows.Add(Row1)
            End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim LiquidacionB As New DataColumn("LiquidacionB")
        LiquidacionB.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(LiquidacionB)

        Dim Cliente As New DataColumn("Cliente")
        Cliente.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cliente)

        Dim ReciboOficial As New DataColumn("ReciboOficial")
        ReciboOficial.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ReciboOficial)

        Dim ImporteB As New DataColumn("ImporteB")
        ImporteB.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ImporteB)

        Dim OperacionN As New DataColumn("OperacionN")
        OperacionN.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(OperacionN)

        Dim LiquidacionN As New DataColumn("LiquidacionN")
        LiquidacionN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(LiquidacionN)

        Dim ImporteN As New DataColumn("ImporteN")
        ImporteN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ImporteN)

        Dim SaldoN As New DataColumn("SaldoN")
        SaldoN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(SaldoN)

        Dim Total As New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Total)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

    End Sub
    Private Sub GeneraCombosGrid()

        Estado.DataSource = DtEstadoActivoYBaja()
        Dim Row As DataRow = Estado.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Estado.DataSource.Rows.Add(Row)
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        Cliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes;")
        Cliente.DisplayMember = "Nombre"
        Cliente.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LiquidacionB" Then
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
            e.Value = NumeroEditado(e.Value)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LiquidacionN" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = NumeroEditado(e.Value)
                End If
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("OperacionN").Value = 1 Then Grid.Rows(e.RowIndex).Cells("CandadoN").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("OperacionN").Value = 2 Then Grid.Rows(e.RowIndex).Cells("CandadoN").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
            End If

            If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
                If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
            End If

            If Grid.Columns(e.ColumnIndex).Name = "NVLP" Then
                If Not IsDBNull(e.Value) Then e.Value = NumeroEditado(e.Value)
            End If

            If Grid.Columns(e.ColumnIndex).Name = "ImporteB" Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If

            If Grid.Columns(e.ColumnIndex).Name = "ImporteN" Or Grid.Columns(e.ColumnIndex).Name = "Total" Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else : e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LiquidacionB" Or Grid.Columns(e.ColumnIndex).Name = "LiquidacionN" Then
            Dim Liquidacion As Double = 0
            Dim Abierto As Boolean

            Liquidacion = Grid.CurrentCell.Value
            If Grid.Columns(e.ColumnIndex).Name = "LiquidacionB" Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Abierto = True
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Abierto = False
            End If
            If Grid.Columns(e.ColumnIndex).Name = "LiquidacionN" Then Abierto = False

            UnaNVLP.PLiquidacion = Liquidacion
            UnaNVLP.PAbierto = Abierto
            UnaNVLP.ShowDialog()
            UnaNVLP.Dispose()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim pa As New PrintGPantalla(Me)
        pa.Print()

    End Sub

End Class