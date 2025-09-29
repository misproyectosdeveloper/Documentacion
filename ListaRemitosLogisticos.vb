Public Class ListaRemitosLogisticos
    Public PSoloPendientes As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaRemitos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 40

        Grid.AutoGenerateColumns = False

        GeneraCombosGrid()

        LlenaComboTablas(ComboDeposito, 20)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboCliente, "", "Clientes")
        ComboCliente.SelectedValue = 0
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Clientes WHERE Alias <> '';")
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

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        MaskedRemito.Text = "000000000000"

        CreaDtGrid()

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
        End If

        If Not PermisoTotal Then
            Grid.Columns("Candado").Visible = False
        End If

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaRemitos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

        Entrada.Activate()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Valida() Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        SqlB = "SELECT 1 as Operacion,* FROM RemitosLogisticosCabeza "
        SqlN = "SELECT 2 as Operacion,* FROM RemitosLogisticosCabeza "

        Dim SqlRemito As String = ""
        If Val(MaskedRemito.Text) <> 0 Then
            SqlRemito = "WHERE Remito = " & Val(MaskedRemito.Text) & " "
        Else : SqlRemito = "WHERE Remito LIKE '%' "
        End If

        Dim Cliente As Integer = 0
        If ComboCliente.SelectedValue <> 0 Then Cliente = ComboCliente.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Cliente = ComboAlias.SelectedValue

        Dim SqlCliente As String = ""
        If Cliente > 0 Then
            SqlCliente = "AND Cliente = " & Cliente & " "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        Dim SqlFecha As String
        SqlFecha = "AND Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlEstado As String
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND Estado = " & ComboEstado.SelectedValue & " "
        End If

        Dim SqlNoConfirmados As String
        If CheckNoConfirmados.Checked Then
            SqlNoConfirmados = "AND Confirmado = 0 AND Estado = 1 "
            SqlEstado = ""
        End If

        SqlB = SqlB & SqlRemito & SqlCliente & SqlDeposito & SqlFecha & SqlEstado & SqlNoConfirmados & ";"
        SqlN = SqlN & SqlRemito & SqlCliente & SqlDeposito & SqlFecha & SqlEstado & SqlNoConfirmados & ";"

        LLenaGrid()

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position
        Grid.Focus()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub Grid_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellContentClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name <> "Confirmado" Then Exit Sub
        Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Not Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        If Not RegrabaConfirmacion(Grid.Rows(e.RowIndex).Cells("Remito").Value, Grid.Rows(e.RowIndex).Cells("Confirmado").Value, Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
            MsgBox("Error al Grabar Confirmación.")
            Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Not Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
        End If
        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0

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
    Private Sub GeneraCombosGrid()

        Deposito.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 20;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

        Cliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes;")
        Cliente.DisplayMember = "Nombre"
        Cliente.ValueMember = "Clave"

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

        Dt.Dispose()

    End Sub
    Private Sub Detalle(ByVal Row As DataRowView)

        Dim Row1 As DataRow = DtGrid.NewRow
        Row1("Operacion") = Row("Operacion")
        Row1("Consumo") = Row("Consumo")
        Row1("Remito") = Row("Remito")
        Row1("Cliente") = Row("Cliente")
        Row1("Sucursal") = Row("Sucursal")
        If Row("Sucursal") = 0 Then
            Row1("Sucursal") = ""
        Else
            Row1("Sucursal") = NombreSucursalCliente(Row("Cliente"), Row("Sucursal"))
        End If
        Row1("Fecha") = Row("Fecha")
        Row1("FechaRemito") = Row("FechaRemito")
        Row1("Deposito") = Row("Deposito")
        Row1("CantidadEnvases") = CantidadDeEnvases(Row("Operacion"), Row("Consumo"))
        Row1("Confirmado") = Row("Confirmado")
        Row1("ConfirmadoInvisible") = Row("Confirmado")
        Row1("Estado") = Row("Estado")
        If Row("Estado") = 1 Then Row1("Estado") = 0
        Row1("Cartel") = ""
        DtGrid.Rows.Add(Row1)

    End Sub
    Private Function RegrabaConfirmacion(ByVal Remito As Decimal, ByVal Confirmado As Boolean, ByVal Operacion As String) As Boolean

        Dim Dt As DataTable
        Dim ConexionStr As String
        If Operacion = 1 Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM RemitosLogisticosCabeza WHERE Remito = " & Remito & ";", Miconexion)
                    Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                    Dt = New DataTable
                    DaP.Fill(Dt)
                    Dt.Rows(0).Item("Confirmado") = Confirmado
                    DaP.Update(Dt)
                End Using
                Dt.Dispose()
                Return True
            End Using
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
        End Try

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Consumo As New DataColumn("Consumo")
        Consumo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Consumo)

        Dim Remito As New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Remito)

        Dim Cliente As New DataColumn("Cliente")
        Cliente.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cliente)

        Dim Deposito As New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Sucursal As New DataColumn("Sucursal")
        Sucursal.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Sucursal)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim FechaRemito As New DataColumn("FechaRemito")
        FechaRemito.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaRemito)

        Dim CantidadEnvases As New DataColumn("CantidadEnvases")
        CantidadEnvases.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(CantidadEnvases)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Confirmado As New DataColumn("Confirmado")
        Confirmado.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Confirmado)

        Dim ConfirmadoInvisible As New DataColumn("ConfirmadoInvisible")
        ConfirmadoInvisible.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(ConfirmadoInvisible)

        Dim Cartel As New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

    End Sub
    Private Function CantidadDeEnvases(ByVal Operacion As Integer, ByVal Consumo As Decimal) As Decimal

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
                Using Cmd As New OleDb.OleDbCommand("SELECT SUM(Cantidad) FROM RemitosLogisticosDetalle WHERE Consumo = " & Consumo & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then Return CDec(Ultimo)
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: RemitosLogisticosDetalle", MsgBoxStyle.Critical)
            End
        Finally
        End Try

    End Function
    Private Function Valida() As Boolean

        If Val(MaskedRemito.Text) <> 0 And Not MaskedOK(MaskedRemito) Then
            MsgBox("Remito Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedRemito.Focus()
            Return False
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If ComboCliente.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Cliente o Alias.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            If Not IsNothing(e.Value) Then
                e.Value = NumeroEditado(e.Value)
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If DtGrid.Rows.Count = 0 Then Exit Sub

        Dim Abierto As Boolean

        If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        UnRemitoLogistico.PConsumo = Grid.Rows(e.RowIndex).Cells("Consumo").Value
        UnRemitoLogistico.PAbierto = Abierto
        UnRemitoLogistico.ShowDialog()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub


End Class