Public Class ListaListaDePrecios
    Public PEsVendedor As Boolean
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    '
    Dim Sql As String
    Private Sub ListaListaDePrecios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        If PEsVendedor Then
            LlenaComboTablas(ComboEmisor, 37)
        Else
            LlenaCombo(ComboEmisor, "", "ClientesNacionales")
        End If
        ComboEmisor.SelectedValue = 0
        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PEsVendedor Then
            LabelClienteProveedor.Text = "Vendedor"
        End If

        LlenaCombosGrid()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Exit Sub
        End If

        Sql = "SELECT Lista,Cliente,IntFechaDesde,IntFechaHasta,Zona FROM ListaDePreciosCabeza "

        Dim SqlFecha As String
        SqlFecha = "WHERE IntFechaDesde >=" & Format(DateTimeDesde.Value, "yyyyMMdd") & " AND IntFechaDesde <= " & Format(DateTimeHasta.Value, "yyyyMMdd") & " "

        Dim SqlCliente As String = ""
        If ComboEmisor.SelectedValue <> 0 Then
            SqlCliente = "AND Cliente = " & ComboEmisor.SelectedValue & " "
        End If

        Dim SqlTipo As String = ""
        If PEsVendedor Then
            SqlTipo = "AND EsPorVendedor = 1"
        Else
            SqlTipo = "AND EsPorVendedor = 0"
        End If

        Sql = Sql & SqlFecha & SqlCliente & SqlTipo & "ORDER BY IntFechaDesde;"

        DtGrid = New DataTable
        If Not Tablas.Read(Sql, Conexion, DtGrid) Then Me.Close() : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position
        Grid.Focus()

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
    Private Sub LlenaCombosGrid()

        If PEsVendedor Then
            Cliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 37;")
        Else
            Cliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes;")
        End If
        Cliente.DisplayMember = "Nombre"
        Cliente.ValueMember = "Clave"

        If PEsVendedor Then
            Zona.Visible = False
            Cliente.HeaderText = "Vendedor"
        Else
            Zona.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 40;")
            Dim Row As DataRow = Zona.DataSource.NewRow()
            Row("Clave") = 0
            Row("Nombre") = " "
            Zona.DataSource.Rows.Add(Row)
            Zona.DisplayMember = "Nombre"
            Zona.ValueMember = "Clave"
        End If

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "IntFechaDesde" Or Grid.Columns(e.ColumnIndex).Name = "IntFechaHasta" Then
            If Not IsDBNull(e.Value) Then
                e.Value = e.Value.ToString.Substring(6, 2) & "/" & e.Value.ToString.Substring(4, 2) & "/" & e.Value.ToString.Substring(0, 4)
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        UnaListaDePrecios.PEsVendedor = PEsVendedor
        UnaListaDePrecios.PLista = Grid.CurrentRow.Cells("Lista").Value
        UnaListaDePrecios.ShowDialog()
        UnaListaDePrecios.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
End Class