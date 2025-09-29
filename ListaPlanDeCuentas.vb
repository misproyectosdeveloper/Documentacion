Public Class ListaPlanDeCuentas
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Private Sub ListaPlanDeCuentas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        GeneraCombosGrid()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor


        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT * FROM PlanDeCuentas;", Conexion, Dt) Then Me.Close() : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = Dt

        Me.Cursor = System.Windows.Forms.Cursors.Default


    End Sub
    Private Sub ButtonExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "", "Listado de Plan de Cuentas")

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
    Public Sub ArmaDtGrid()

        DtGrid = New DataTable

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Clave)

        Dim Centro As New DataColumn("Centro")
        Centro.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Centro)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cuenta)

        Dim SubCuenta As New DataColumn("SubCuenta")
        SubCuenta.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(SubCuenta)

    End Sub
    Private Sub GeneraCombosGrid()

        Centro.DataSource = Tablas.Leer("SELECT Centro,Nombre FROM CentrosDeCosto;")
        Dim Row As DataRow = Centro.DataSource.NewRow
        Row("Centro") = 0
        Row("Nombre") = ""
        Centro.DataSource.Rows.Add(Row)
        Centro.DisplayMember = "Nombre"
        Centro.ValueMember = "Centro"

        Cuenta.DataSource = Tablas.Leer("SELECT Cuenta,Nombre FROM Cuentas;")
        Row = Cuenta.DataSource.NewRow
        Row("Cuenta") = 0
        Row("Nombre") = ""
        Cuenta.DataSource.Rows.Add(Row)
        Cuenta.DisplayMember = "Nombre"
        Cuenta.ValueMember = "Cuenta"

        SubCuenta.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM SubCuentas;")
        Row = SubCuenta.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        SubCuenta.DataSource.Rows.Add(Row)
        SubCuenta.DisplayMember = "Nombre"
        SubCuenta.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Clave" Then
            If Not IsNothing(e.Value) Then
                e.Value = Format(e.Value, "000-000000-00")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "SaldoInicial" Then
            If Not IsNothing(e.Value) Then
                e.Value = FormatNumber(e.Value, 2)
            End If
        End If

    End Sub

End Class