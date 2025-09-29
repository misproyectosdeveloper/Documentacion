Public Class ListaCierreFactura
    Public PSoloPendientes As Boolean
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    '
    Dim SqlN As String
    Private Sub ListaCierreFacturasExportacion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        GeneraCombosGrid()

        ComboCliente.DataSource = Tablas.Leer("Select Clave,Nombre FROM Clientes WHERE TipoIva = 4;")
        Dim Row As DataRow = ComboCliente.DataSource.NewRow
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

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Clientes WHERE Alias <> '' AND TipoIva = 4;")
        Row = ComboAlias.DataSource.NewRow
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

        DateTimeDesde.Value = DateTimeHasta.Value.AddMonths(-6)
        MaskedFactura.Text = "000000000000"

        CreaDtGrid()

        If Not PermisoTotal Then Me.Close() : Exit Sub

    End Sub
    Private Sub ListaCierreFacturasExportacion_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub ListaCierreFacturasExportacion_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Valida() Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        SqlN = "SELECT * FROM CierreFacturasCabeza "

        Dim SqlFactura As String = ""
        If Val(MaskedFactura.Text) <> 0 Then
            SqlFactura = "WHERE Factura = " & Val(MaskedFactura.Text) & " "
        Else : SqlFactura = "WHERE Factura LIKE '%' "
        End If

        Dim Cliente As Integer = 0
        If ComboCliente.SelectedValue <> 0 Then Cliente = ComboCliente.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Cliente = ComboAlias.SelectedValue

        Dim SqlFecha As String = ""
        SqlFecha = "AND Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlEstado As String = ""
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND Estado = " & ComboEstado.SelectedValue & " "
        End If

        SqlN = SqlN & SqlFactura & SqlFecha & SqlEstado

        LLenaGrid()

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position
        Grid.Focus()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

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

        Cliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes;")
        Cliente.DisplayMember = "Nombre"
        Cliente.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    Private Sub LLenaGrid()

        DtGrid.Clear()

        Dim Dt As New DataTable
        Tablas.Read(SqlN, ConexionN, Dt)

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Nota,Fecha"

        Dim Cliente As Integer

        For I As Integer = 0 To View.Count - 1
            Dim Row As DataRowView = View(I)
            Cliente = HallaClienteFactura(Row("Factura"))
            If EsBueno(Cliente) Then
                Dim Row1 As DataRow = DtGrid.NewRow
                Row1("Nota") = Row("Nota")
                Row1("Factura") = Row("Factura")
                Row1("Cliente") = Cliente
                Row1("Fecha") = Row("Fecha")
                If Row("Estado") = 1 Then
                    Row1("Estado") = 0
                Else
                    Row1("Estado") = Row("Estado")
                End If
                DtGrid.Rows.Add(Row1)
            End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

    End Sub
    Private Function EsBueno(ByVal Cliente As Integer) As Boolean

        If ComboCliente.SelectedValue <> 0 And ComboCliente.SelectedValue <> Cliente Then Return False

        Return True

    End Function
    Private Function HallaClienteFactura(ByVal Factura As Double) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cliente FROM FacturasCabeza WHERE Factura = " & Factura & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla de Clientes.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Nota As New DataColumn("Nota")
        Nota.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Nota)

        Dim Factura As New DataColumn("Factura")
        Factura.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Factura)

        Dim Cliente As New DataColumn("Cliente")
        Cliente.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cliente)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

    End Sub
    Private Function Valida() As Boolean

        If Val(MaskedFactura.Text) <> 0 And Not MaskedOK(MaskedFactura) Then
            MsgBox("Factura Invalida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedFactura.Focus()
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

        If Grid.Columns(e.ColumnIndex).Name = "Factura" Then
            If Not IsNothing(e.Value) Then
                e.Value = NumeroEditado(e.Value)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Nota" Then
            UnCierreFactura.PNota = Grid.Rows(e.RowIndex).Cells("Nota").Value
            UnCierreFactura.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            UnCierreFactura.Dispose()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Factura" Then
            '           UnaFactura.PFactura = Grid.Rows(e.RowIndex).Cells("Factura").Value
            '          UnaFactura.PAbierto = True
            '          UnaFactura.PBloqueaFunciones = True
            '          UnaFactura.ShowDialog()
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub

End Class