Public Class ListaRecibosSueldo
    Private WithEvents bs As New BindingSource
    '
    Dim Dt As DataTable
    Dim DtFormasPago As DataTable
    Dim DtGrid As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListasRecibos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        Dim DtLegajos As New DataTable
        If Not Tablas.Read("SELECT Legajo,RIGHT('0000' + CAST(Legajo AS varchar),4) + ' - ' + Apellidos + ' ' + Nombres As Con FROM Empleados WHERE Estado = 1;", Conexion, DtLegajos) Then Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read("SELECT Legajo,RIGHT('0000' + CAST(Legajo AS varchar),4) + ' - ' + Apellidos + ' ' + Nombres As Con FROM Empleados WHERE Estado = 1;", ConexionN, DtLegajos) Then Exit Sub
        End If
        Dim Row As DataRow = DtLegajos.NewRow
        Row("Legajo") = 0
        Row("Con") = ""
        DtLegajos.Rows.Add(Row)
        ComboLegajo.DataSource = DtLegajos
        ComboLegajo.DisplayMember = "Con"
        ComboLegajo.ValueMember = "Legajo"
        ComboLegajo.SelectedValue = 0
        With ComboLegajo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
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

        SqlB = "SELECT 1 AS Operacion,C.* FROM RecibosSueldosCabeza AS C WHERE "
        SqlN = "SELECT 2 AS Operacion,C.* FROM RecibosSueldosCabeza AS C WHERE "

        Dim SqlFecha As String
        SqlFecha = "C.FechaContable >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND C.FechaContable < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlLegajo As String = ""
        If ComboLegajo.SelectedValue <> 0 Then
            SqlLegajo = "AND C.Legajo = " & ComboLegajo.SelectedValue
        End If

        SqlB = SqlB & SqlFecha & SqlLegajo & ";"
        SqlN = SqlN & SqlFecha & SqlLegajo & ";"

        LLenaGrid()

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        Grid.Focus()

    End Sub
    Private Sub Textcomprobante_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

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
    Private Sub ComboLegajo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboLegajo.Validating

        If IsNothing(ComboLegajo.SelectedValue) Then ComboLegajo.SelectedValue = 0

    End Sub
    Private Sub TextMes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextAnio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

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
        If CheckCerrado.Checked And PermisoTotal Then
            Tablas.Read(SqlN, ConexionN, Dt)
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Legajo,Anio,Mes"

        DtGrid.Clear()
        Dim Row1 As DataRow

        For Each Row As DataRowView In View
            Row1 = DtGrid.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Legajo") = Row("Legajo")
            If Row("Legajo") < 5000 Then
                Row1("Nombre") = NombreLegajo(Row("Legajo"), Conexion)
            Else : Row1("Nombre") = NombreLegajo(Row("Legajo"), ConexionN)
            End If
            Row1("Recibo") = Row("Recibo")
            Row1("FechaContable") = Row("FechaContable")
            Row1("Importe") = Row("Importe")
            Row1("Saldo") = Row("Saldo")
            Row1("Estado") = Row("Estado")
            If Row("Estado") <> 3 Then Row1("Estado") = 0
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

        Dim Recibo As New DataColumn("Recibo")
        Recibo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Recibo)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Nombre)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Saldo)

        Dim FechaContable As New DataColumn("FechaContable")
        FechaContable.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaContable)

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

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Legajo" Then
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FechaContable" Then
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

        UnReciboSueldo.PLegajo = Grid.CurrentRow.Cells("Legajo").Value
        UnReciboSueldo.PRecibo = Grid.CurrentRow.Cells("Recibo").Value
        UnReciboSueldo.PAbierto = Abierto
        UnReciboSueldo.ShowDialog()
        UnReciboSueldo.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub


    
End Class