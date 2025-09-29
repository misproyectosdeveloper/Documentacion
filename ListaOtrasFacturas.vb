Public Class ListaOtrasFacturas
    Private WithEvents bs As New BindingSource
    '
    Dim Dt As DataTable
    Dim DtFormasPago As DataTable
    Dim DtGrid As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaOtrasFacturas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ComboProveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM OtrosProveedores;")
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Checked = False
        Else
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        '   DateTimeDesde.Value = DateTimeHasta.Value.AddMonths(-1)

        GeneraCombosGrid()

        CreaDtGrid()

        '     ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 AS Operacion,C.* FROM OtrasFacturasCabeza AS C WHERE "
        SqlN = "SELECT 2 AS Operacion,C.* FROM OtrasFacturasCabeza AS C WHERE "

        Dim SqlFecha As String
        SqlFecha = "C.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlSaldo As String
        If CheckPendientes.Checked Then
            SqlSaldo = "AND C.Saldo <> 0 "
        End If
        If CheckCancelados.Checked Then
            SqlSaldo = "AND C.Saldo = 0 "
        End If

        Dim SqlMes As String
        If TextMes.Text <> "" Then
            SqlMes = "AND C.Mes = " & CInt(TextMes.Text)
        End If

        Dim SqlProveedor As String = ""
        If ComboProveedor.SelectedValue <> 0 Then
            SqlProveedor = " AND C.Proveedor = " & ComboProveedor.SelectedValue
        End If

        SqlB = SqlB & SqlFecha & SqlSaldo & SqlMes & SqlProveedor & ";"
        SqlN = SqlN & SqlFecha & SqlSaldo & SqlMes & SqlProveedor & ";"

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
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub TextMes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextMes.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub LLenaGrid()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If
        If Not CheckCancelados.Checked And Not CheckPendientes.Checked Then
            CheckCancelados.Checked = True
            CheckPendientes.Checked = True
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
        View.Sort = "Fecha"

        DtGrid.Clear()
        Dim Row1 As DataRow

        For Each Row As DataRowView In View
            Row1 = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Factura") = Row("Factura")
            Row1("Proveedor") = Row("Proveedor")
            Row1("Comprobante") = Row("Comprobante")
            Row1("Fecha") = Format(Row("Fecha"), "dd/MM/yyyy 00:00:00")
            Row1("Importe") = Row("Importe")
            Row1("Mes") = Row("Mes")
            Row1("Anio") = Row("Anio")
            Row1("Saldo") = Row("Saldo")
            Row1("Estado") = Row("Estado")
            If Row("Estado") <> 3 Then Row1("Estado") = 0
            Row1("Comentario") = Row("Comentario")
            DtGrid.Rows.Add(Row1)
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

        Dim Factura As New DataColumn("Factura")
        Factura.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Factura)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comprobante)

        Dim Mes As New DataColumn("Mes")
        Mes.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Mes)

        Dim Anio As New DataColumn("Anio")
        Anio.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Anio)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Saldo)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Comentario As New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comentario)

    End Sub
    Private Sub GeneraCombosGrid()

        Estado.DataSource = DtEstadoActivoYBaja()
        Dim Row As DataRow = Estado.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Estado.DataSource.Rows.Add(Row)
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM OtrosProveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Return False
        End If

        If TextMes.Text <> "" Then
            If CInt(TextMes.Text) < 1 Or CInt(TextMes.Text) > 12 Then
                MsgBox("Mes Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextMes.Focus()
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
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
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

        UnaFacturaOtrosProveedores.PRecibo = Grid.CurrentRow.Cells("Factura").Value
        UnaFacturaOtrosProveedores.PAbierto = Abierto
        UnaFacturaOtrosProveedores.ShowDialog()
        UnaFacturaOtrosProveedores.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
  

End Class