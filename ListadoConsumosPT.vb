Public Class ListaConsumosPT
    Public PBloqueaFunciones As Boolean
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaConsumosPT_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        GeneraCombosGrid()

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboNegocio.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 AND Producto = " & Fruta & " ORDER BY Nombre;")
        Dim Row As DataRow = ComboNegocio.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboNegocio.DataSource.rows.add(Row)
        ComboNegocio.DisplayMember = "Nombre"
        ComboNegocio.ValueMember = "Clave"
        ComboNegocio.SelectedValue = 0
        With ComboNegocio
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

        CreaDtGrid()

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
        End If

    End Sub
    Private Sub ListaConsumosPT_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub ListaConsumosPT_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

        Entrada.Activate()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Valida() Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        If ComboCosteo.SelectedValue = 0 And ComboNegocio.SelectedValue <> 0 Then
            ComboNegocio.SelectedValue = 0
        End If

        SqlB = "SELECT 1 as Operacion,* FROM ConsumosPTCabeza "
        SqlN = "SELECT 2 as Operacion,* FROM ConsumosPTCabeza "

        Dim SqlConsumo As String = ""
        If TextConsumo.Text <> "" Then
            SqlConsumo = "WHERE Consumo = " & Val(TextConsumo.Text) & " "
        Else : SqlConsumo = "WHERE Consumo LIKE '%' "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        Dim SqlFecha As String
        SqlFecha = "AND Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlEstado As String = ""
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND Estado = " & ComboEstado.SelectedValue & " "
        End If

        Dim SqlCosteo As String = ""
        If ComboCosteo.SelectedValue <> 0 Then
            SqlCosteo = "AND Costeo = " & ComboCosteo.SelectedValue & " "
        End If

        SqlB = SqlB & SqlConsumo & SqlDeposito & SqlFecha & SqlEstado & SqlCosteo & ";"
        SqlN = SqlN & SqlConsumo & SqlDeposito & SqlFecha & SqlEstado & SqlCosteo & ";"

        LLenaGrid()

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position
        Grid.Focus()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboNegocio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboNegocio.Validating

        If IsNothing(ComboNegocio.SelectedValue) Then ComboNegocio.SelectedValue = 0

        BuscaCosteo(0)

        BuscaCosteo(ComboNegocio.SelectedValue)

    End Sub
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Consumos", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

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
    Private Sub BuscaCosteo(ByVal Negocio As Integer)

        Dim SqlFecha As String = ""
        SqlFecha = "IntFechaDesde <= " & Format(DateTimeHasta.Value, "yyyyMMdd") & " AND IntFechaHasta >= " & Format(DateTimeDesde.Value, "yyyyMMdd") & ";"
        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & Negocio & " AND " & SqlFecha & ";"
        ComboCosteo.DataSource = New DataTable
        ComboCosteo.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboCosteo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        ComboCosteo.DataSource.rows.add(Row)
        ComboCosteo.DisplayMember = "Nombre"
        ComboCosteo.ValueMember = "Costeo"
        ComboCosteo.SelectedValue = 0
        With ComboCosteo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Private Sub GeneraCombosGrid()

        Deposito.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    Private Sub LLenaGrid()

        DtGrid.Clear()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        Dim Dt As New DataTable
        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked And PermisoTotal Then
            Tablas.Read(SqlN, ConexionN, Dt)
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Consumo"

        For I As Integer = 0 To View.Count - 1
            Detalle(View(I))
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If PermisoTotal Then
            Grid.Columns("Candado").Visible = True
        Else : Grid.Columns("Candado").Visible = False
        End If

        Dt.Dispose()

    End Sub
    Private Sub Detalle(ByVal Row As DataRowView)

        Dim Row1 As DataRow = DtGrid.NewRow
        Row1("Operacion") = Row("Operacion")
        Row1("Consumo") = Row("Consumo")
        Row1("Negocio") = NombreProveedor(HallaNegocio(Row("Costeo")))
        Row1("Costeo") = NombreCosteo(Row("Costeo"))
        Row1("Fecha") = Format(Row("Fecha"), "dd/MM/yyy 00:00:00")
        Row1("Deposito") = Row("Deposito")
        Row1("Estado") = Row("Estado")
        Row1("Cartel") = ""
        DtGrid.Rows.Add(Row1)

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Consumo As New DataColumn("Consumo")
        Consumo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Consumo)

        Dim Negocio As New DataColumn("Negocio")
        Negocio.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Negocio)

        Dim Costeo As New DataColumn("Costeo")
        Costeo.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Costeo)

        Dim Deposito As New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Cartel As New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Consumo" Then
            If Not IsNothing(e.Value) Then
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Estado" Then
            If Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 3 Then e.CellStyle.ForeColor = Color.Red
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        Dim Abierto As Boolean

        If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Consumo" Then
            UnConsumoPT.PConsumo = Grid.Rows(e.RowIndex).Cells("Consumo").Value
            UnConsumoPT.PAbierto = Abierto
            UnConsumoPT.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub


End Class