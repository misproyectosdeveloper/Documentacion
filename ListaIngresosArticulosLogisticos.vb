Public Class ListaIngresosArticulosLogisticos
    Public PEsSaldoInicial As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Dim SqlB As String
    Dim SqlN As String
    Private WithEvents bs As New BindingSource
    '
    Dim DtIngresos As DataTable
    Dim DtGrid As DataTable
    Private Sub ListaIngresosArticulosLogisticos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If PEsSaldoInicial Then
            Me.Text = "Saldos Iniciales Artículos Logísticos"
            ButtonDevolucion.Visible = False
            Grid.Columns("Remito").Visible = False
            Grid.Columns("Guia").Visible = False
        End If

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        GeneraComboGrid()

        LlenaComboTablas(ComboDeposito, 20)
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboDeposito.SelectedValue = 0

        ComboProveedor.DataSource = ProveedoresDeInsumos()
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = ProveedoresDeInsumosAlias()
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

        CreaDtGrid()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Visible = False
        End If

    End Sub
    Private Sub ListaIngresosArticulosLogisticos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 AS Operacion,Proveedor,Deposito,Fecha,Remito,Guia,Ingreso,Estado,Pagado FROM IngresoArticulosLogisticosCabeza "
        SqlN = "SELECT 2 AS Operacion,Proveedor,Deposito,Fecha,Remito,Guia,Ingreso,Estado,Pagado FROM IngresoArticulosLogisticosCabeza "

        Dim Proveedor As Integer = 0
        If ComboProveedor.SelectedValue <> 0 Then Proveedor = ComboProveedor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Proveedor = ComboAlias.SelectedValue

        Dim SqlProveedor As String
        If Proveedor = 0 Then
            SqlProveedor = "WHERE Proveedor LIKE '%' "
        Else
            SqlProveedor = "WHERE Proveedor = " & Proveedor & " "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        Dim SqlEstado As String = ""
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND Estado = " & ComboEstado.SelectedValue & " "
        End If

        Dim SqlPagados As String = ""
        If CheckPagados.Checked Then
            SqlPagados = "AND Pagado = 1 "
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "AND Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlSaldoInicial As String = ""
        If PEsSaldoInicial Then
            SqlSaldoInicial = "AND Remito = 0 and Guia = 0 "
        Else
            SqlSaldoInicial = "AND (Remito <> 0 OR Guia <> 0) "
        End If

        SqlB = SqlB & SqlProveedor & SqlDeposito & SqlFecha & SqlEstado & SqlSaldoInicial & SqlPagados & ";"
        SqlN = SqlN & SqlProveedor & SqlDeposito & SqlFecha & SqlEstado & SqlSaldoInicial & SqlPagados & ";"

        LLenaGrid()

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position
        Grid.Focus()

    End Sub
    Private Sub ButtonDevolucion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDevolucion.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.Rows.Count = 0 Then Exit Sub

        If Grid.CurrentRow.Cells("Estado").Value = 3 Then
            MsgBox("Ingreso Esta Anulado.")
            Exit Sub
        End If

        UnaDevolucionIngresoLogistico.PIngreso = Grid.CurrentRow.Cells("Ingreso").Value
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            UnaDevolucionIngresoLogistico.PAbierto = True
        Else : UnaDevolucionIngresoLogistico.PAbierto = False
        End If
        UnaDevolucionIngresoLogistico.ShowDialog()
        UnaDevolucionIngresoLogistico.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub TextFechaDesde_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextFechaHasta_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

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

        GridAExcel(Grid, Date.Now, "Ingreso Artículos Logísticos", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LLenaGrid()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        DtIngresos = New DataTable
        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, DtIngresos) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked Then
            If Not Tablas.Read(SqlN, ConexionN, DtIngresos) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView

        View = DtIngresos.DefaultView
        View.Sort = "Ingreso"
        DtGrid.Clear()

        Dim Cartel As String

        For Each Row As DataRowView In View
            AgregaADtGrid(Row("Operacion"), Row("Ingreso"), Row("Proveedor"), Row("Deposito"), Row("Remito"), Row("Guia"), Row("Fecha"), Cartel, Row("Estado"), Row("Pagado"))
        Next

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As DataColumn = New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Ingreso As New DataColumn("Ingreso")
        Ingreso.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Ingreso)

        Dim Proveedor As DataColumn = New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Deposito As DataColumn = New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Remito As DataColumn = New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Remito)

        Dim Guia As DataColumn = New DataColumn("Guia")
        Guia.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Guia)

        Dim Fecha As DataColumn = New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Cartel As New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

        Dim Pagado As New DataColumn("Pagado")
        Pagado.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Pagado)

        Dim CantidadEnvases As New DataColumn("CantidadEnvases")
        CantidadEnvases.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(CantidadEnvases)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

    End Sub
    Private Sub AgregaADtGrid(ByVal Operacion As Integer, ByVal Ingreso As Integer, ByVal Proveedor As Integer, ByVal Deposito As Integer, ByVal Remito As Double, ByVal Guia As Double, ByVal Fecha As Date, ByVal Cartel As String, ByVal Estado As Integer, ByVal Pagado As Boolean)

        Dim Row As DataRow

        Row = DtGrid.NewRow()
        Row("Operacion") = Operacion
        Row("Ingreso") = Ingreso
        Row("Proveedor") = Proveedor
        Row("Deposito") = Deposito
        Row("Remito") = Remito
        Row("Guia") = Guia
        Row("Fecha") = Fecha
        Row("Cartel") = Cartel
        Row("CantidadEnvases") = CantidadDeEnvases(Row("Operacion"), Row("Ingreso"))
        Row("Estado") = Estado
        If Estado = 1 Then Row("Estado") = 0
        Row("Pagado") = Pagado
        DtGrid.Rows.Add(Row)

    End Sub
    Private Function CantidadDeEnvases(ByVal Operacion As Integer, ByVal Ingreso As Decimal) As Decimal

        Dim ConexionStr As String = ""

        Select Case Operacion
            Case 1
                ConexionStr = Conexion
            Case 2
                ConexionStr = ConexionN
        End Select

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT SUM(Cantidad - Devueltas) FROM IngresoArticulosLogisticosDetalle WHERE Ingreso = " & Ingreso & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then Return CDec(Ultimo)
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: IngresoArticulosLogisticaDetalle", MsgBoxStyle.Critical)
            End
        Finally
        End Try

    End Function
    Private Sub GeneraComboGrid()

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        Deposito.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo =20;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If ComboProveedor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Proveedor o Alias.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Proveedor" Then
            If Not IsNothing(e.Value) Then
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
                        Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    Else : Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                    End If
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            If Not IsNothing(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = Format(e.Value, "0000-00000000")
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Guia" Then
            If Not IsNothing(e.Value) Then e.Value = NumeroEditado(e.Value)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If


    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        Dim Abierto As Boolean
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If
        UnIngresoArticulosLogisticos.PIngreso = Grid.CurrentRow.Cells("Ingreso").Value
        UnIngresoArticulosLogisticos.PAbierto = Abierto
        UnIngresoArticulosLogisticos.ShowDialog()
        UnIngresoArticulosLogisticos.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub

End Class
