Public Class ListaEmpleados
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim DtSucursales As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaEmpleados_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaCombosGrid()

        CreaDtGrid()

        If Not PermisoTotal Then Grid.Columns("BrutoN").Visible = False

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 as Operacion,* FROM Empleados;"
        SqlN = "SELECT 2 as Operacion,* FROM Empleados;"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

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

        GridAExcel(Grid, Date.Now, "Empleados", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LLenaGrid()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        DtGrid.Clear()

        Dim Dt As New DataTable

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked = True Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Operacion,Legajo"

        For Each Row As DataRowView In View
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Legajo") = Row("Legajo")
            Row1("Apellidos") = Row("Apellidos")
            Row1("Nombres") = Row("Nombres")
            Row1("Ingreso") = Format(Row("FechaAlta"), "dd/MM/yyyy 00:00:00")
            If Row("Estado") <> 3 Then
                Row1("Baja") = "01/01/1800"
            Else : Row1("Baja") = Format(Row("FechaBaja"), "dd/MM/yyyy 00:00:00")
            End If
            Row1("Bruto") = Row("Bruto")
            Row1("BrutoN") = Row("Bruto2")
            Row1("Banco") = Row("Banco")
            Row1("Cuenta") = Row("Cuenta")
            Row1("Estado") = 0
            If Row("Estado") <> 1 Then Row1("Estado") = Row("Estado")
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

        Dim Legajo As New DataColumn("Legajo")
        Legajo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Legajo)

        Dim Apellidos As New DataColumn("Apellidos")
        Apellidos.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Apellidos)

        Dim Nombres As New DataColumn("Nombres")
        Nombres.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Nombres)

        Dim Ingreso As New DataColumn("Ingreso")
        Ingreso.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Ingreso)

        Dim Baja As New DataColumn("Baja")
        Baja.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Baja)

        Dim Bruto As New DataColumn("Bruto")
        Bruto.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Bruto)

        Dim BrutoN As New DataColumn("BrutoN")
        BrutoN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(BrutoN)

        Dim Banco As New DataColumn("Banco")
        Banco.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Banco)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cuenta)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Estado.DataSource = DtEstadoLegajoActivoYBaja()
        Row = Estado.DataSource.NewRow()
        Row("Clave") = 2
        Row("Nombre") = "Suspendido"
        Estado.DataSource.Rows.Add(Row)
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        Banco.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 order By Nombre;")
        Row = Banco.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Banco.DataSource.Rows.Add(Row)
        Banco.DisplayMember = "Nombre"
        Banco.ValueMember = "Clave"

    End Sub
    Private Function NombreSucursal(ByVal Banco As Integer, ByVal Sucursal As Integer) As String

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtSucursales.Select("Banco = " & Banco & " AND Sucursal = " & Sucursal)
        Return RowsBusqueda(0).Item("Nombre")

    End Function
    Private Sub ArmaDtSucursales()

        DtSucursales = New DataTable

        If Not Tablas.Read("SELECT Banco,Sucursal,NombreSucursal AS Nombre FROM CuentasBancarias;", Conexion, DtSucursales) Then End

    End Sub
    Private Function Valida() As Boolean
        Return True
        
    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Legajo" Then
            e.Value = FormatNumber(e.Value, 0)
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cuenta" Or Grid.Columns(e.ColumnIndex).Name = "NumeracionTercero" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, 0)
            Else : e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Baja" Then
            If Not IsDBNull(e.Value) Then
                If Year(e.Value) = 1800 Then e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Bruto" Or Grid.Columns(e.ColumnIndex).Name = "BrutoN" Then
            e.Value = FormatNumber(e.Value, GDecimales)
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

        UnLegajo.PLegajo = Grid.CurrentRow.Cells("Legajo").Value
        UnLegajo.PAbierto = Abierto
        UnLegajo.ShowDialog()
        UnLegajo.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub

  
End Class