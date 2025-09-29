Public Class ListaSaldosContables
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaSaldosContables_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Checked = False
        End If

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaCombosGrid()

        CreaDtGrid()

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

    End Sub
    Private Sub UnaCtaCte_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If TextCentro.Text <> "" Then TextCentro.Text = Format(Val(TextCentro.Text), "000")
        If TextCuenta.Text <> "" Then TextCuenta.Text = Format(Val(TextCuenta.Text), "000000")
        If TextSubCuenta.Text <> "" Then TextSubCuenta.Text = Format(Val(TextSubCuenta.Text), "00")

        Dim SqlCuenta As String = ""
        Dim SqlCuentaPlanDeCuentas As String = ""

        If TextCentro.Text = "" Then
            SqlCuenta = "%"
        Else
            SqlCuenta = Val(TextCentro.Text)
        End If

        If TextCuenta.Text = "" Then
            SqlCuenta = SqlCuenta & "[0-9][0-9][0-9][0-9][0-9][0-9]"
        Else
            SqlCuenta = SqlCuenta & TextCuenta.Text
        End If

        If TextSubCuenta.Text = "" Then
            SqlCuenta = SqlCuenta & "[0-9][0-9]"
        Else
            SqlCuenta = SqlCuenta & TextSubCuenta.Text
        End If

        If TextCentro.Text = "" And TextCuenta.Text = "" And TextSubCuenta.Text = "" Then
            SqlCuenta = ""
        Else
            SqlCuenta = " AND CAST(CAST(D.Cuenta AS numeric) as char)LIKE '" & SqlCuenta & "'"
        End If

        SqlCuentaPlanDeCuentas = SqlCuenta
        SqlCuentaPlanDeCuentas = SqlCuentaPlanDeCuentas.Replace("D.Cuenta", "ClaveCuenta")

        Dim IntFecha As String = "CAST(YEAR(Fecha) as varchar(4)) + RIGHT('00' + cast(MONTH(Fecha) as varchar),2) + RIGHT('00' + cast(DAY(Fecha) as varchar),2)"

        SqlB = "SELECT 1 As Operacion,0 As Tipo,D.Cuenta,D.Debe,D.haber,C.IntFecha,0 As SaldoInicial FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.Estado = 1 " & SqlCuenta & _
               "UNION ALL " & _
               "SELECT 1 As Operacion,1 AS Tipo,ClaveCuenta AS Cuenta,0 AS Debe,0 AS haber," & IntFecha & " AS Fecha,SaldoInicial FROM PlanDeCuentas WHERE SaldoInicial <> 0 " & SqlCuentaPlanDeCuentas


        SqlN = "SELECT 2 As Operacion,0 AS Tipo,D.Cuenta,D.Debe,D.haber,C.IntFecha,0 As SaldoInicial FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.Estado = 1 " & SqlCuenta & _
               "UNION ALL " & _
               "SELECT 2 As Operacion,1 AS Tipo,ClaveCuenta AS Cuenta,0 AS Debe,0 AS haber," & IntFecha & " AS Fecha,SaldoInicial FROM PlanDeCuentas WHERE SaldoInicial <> 0 " & SqlCuentaPlanDeCuentas

        SqlB = SqlB & ";"
        SqlN = SqlN & ";"

        PreparaArchivos()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, "", "", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub PictureLupa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupa.Click

        SeleccionarCuenta.PCentro = 0
        SeleccionarCuenta.PEsNivelCuenta = True
        SeleccionarCuenta.ShowDialog()
        If SeleccionarCuenta.PCuentaStr <> "" Then
            TextCentro.Text = Mid$(SeleccionarCuenta.PCuentaStr, 1, 3).Trim
            TextCuenta.Text = Mid$(SeleccionarCuenta.PCuentaStr, 4, 6).Trim
            TextSubCuenta.Text = Mid$(SeleccionarCuenta.PCuentaStr, 10, 2).Trim
        End If
        SeleccionarCuenta.Dispose()

    End Sub
    Private Sub TextCentro_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCentro.KeyPress

        If Asc(e.KeyChar) = 13 Then ButtonAceptar_Click(Nothing, Nothing) : Exit Sub

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCuenta_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCuenta.KeyPress

        If Asc(e.KeyChar) = 13 Then ButtonAceptar_Click(Nothing, Nothing) : Exit Sub

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextSubCuenta_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSubCuenta.KeyPress

        If Asc(e.KeyChar) = 13 Then ButtonAceptar_Click(Nothing, Nothing) : Exit Sub

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
    Private Sub PreparaArchivos()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        DtGrid.Clear()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Dt As New DataTable
        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked And PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Cuenta,IntFecha"

        Dim TotalDebePeriodoGral As Double = 0
        Dim TotalHaberPeriodoGral As Double = 0
        Dim TotalDebeAntGral As Double = 0
        Dim TotalHaberAntGral As Double = 0
        Dim TotalDebeAnt As Double = 0
        Dim TotalHaberAnt As Double = 0
        Dim TotalDebePeriodo As Double = 0
        Dim TotalHaberPeriodo As Double = 0
        Dim CuentaAnt As Double

        Dim SaldoInicial As Double = 0
        Dim DebitoSaldoInicial As Double = 0
        Dim CreditoSaldoInicial As Double = 0

        Dim Operacion As Integer

        If CheckAbierto.Checked And Not CheckCerrado.Checked Then Operacion = 1
        If Not CheckAbierto.Checked And CheckCerrado.Checked Then Operacion = 2
        If CheckAbierto.Checked And CheckCerrado.Checked Then Operacion = 0

        For Each Row As DataRowView In View
            If Row("Cuenta") <> CuentaAnt Then
                If CuentaAnt <> 0 Then CortePorCuenta(Operacion, CuentaAnt, TotalDebeAnt, TotalHaberAnt, TotalDebePeriodo, TotalHaberPeriodo, SaldoInicial)
                TotalDebeAnt = 0
                TotalHaberAnt = 0
                TotalDebePeriodo = 0
                TotalHaberPeriodo = 0
                SaldoInicial = 0
                DebitoSaldoInicial = 0
                CreditoSaldoInicial = 0
                CuentaAnt = Row("Cuenta")
            End If
            If Row("IntFecha") <= DateTimeHasta.Value.Year & Format(DateTimeHasta.Value.Month, "00") & Format(DateTimeHasta.Value.Day, "00") Then
                If Row("SaldoInicial") <> 0 Then
                    SaldoInicial = SaldoInicial + Row("SaldoInicial")
                    If Row("SaldoInicial") < 0 Then
                        CreditoSaldoInicial = -Row("SaldoInicial")
                    Else : DebitoSaldoInicial = Row("SaldoInicial")
                    End If
                End If
                If Row("IntFecha") < DateTimeDesde.Value.Year & Format(DateTimeDesde.Value.Month, "00") & Format(DateTimeDesde.Value.Day, "00") Then
                    TotalDebeAnt = Trunca(TotalDebeAnt + Row("Debe") + DebitoSaldoInicial)
                    TotalHaberAnt = Trunca(TotalHaberAnt + Row("Haber") + CreditoSaldoInicial)
                    TotalDebeAntGral = Trunca(TotalDebeAntGral + Row("Debe") + DebitoSaldoInicial)
                    TotalHaberAntGral = Trunca(TotalHaberAntGral + Row("Haber") + CreditoSaldoInicial)
                Else
                    TotalDebePeriodo = Trunca(TotalDebePeriodo + Row("Debe") + DebitoSaldoInicial)
                    TotalHaberPeriodo = Trunca(TotalHaberPeriodo + Row("Haber") + CreditoSaldoInicial)
                    TotalDebePeriodoGral = Trunca(TotalDebePeriodoGral + Row("Debe") + DebitoSaldoInicial)
                    TotalHaberPeriodoGral = Trunca(TotalHaberPeriodoGral + Row("Haber") + CreditoSaldoInicial)
                End If
                DebitoSaldoInicial = 0
                CreditoSaldoInicial = 0
            End If
        Next
        CortePorCuenta(Operacion, CuentaAnt, TotalDebeAnt, TotalHaberAnt, TotalDebePeriodo, TotalHaberPeriodo, 0)
        AddGridTotal(TotalDebeAntGral, TotalHaberAntGral, TotalDebePeriodoGral, TotalHaberPeriodoGral)

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub CortePorCuenta(ByVal Operacion As Integer, ByVal Cuenta As Double, ByVal TotalDebeAnt As Double, ByVal TotalHaberAnt As Double, ByVal TotalDebePeriodo As Double, ByVal TotalHaberPeriodo As Double, ByVal SaldoInicial As Double)

        If Operacion = 0 And Cuenta = 0 Then Exit Sub

        Dim CentroW As Integer
        Dim CuentaW As Integer
        Dim SubCuentaW As Integer

        Dim SaldoAnt As Double = Trunca(TotalDebeAnt - TotalHaberAnt)
        Dim SaldoPer As Double = Trunca(TotalDebePeriodo - TotalHaberPeriodo)

        HallaPartesCuenta(Cuenta, CentroW, CuentaW, SubCuentaW)

        AddGrid(Operacion, Cuenta, CentroW, CuentaW, CuentaW & Format(SubCuentaW, "00"), SaldoAnt, TotalDebePeriodo, TotalHaberPeriodo, SaldoPer, Trunca(SaldoAnt + SaldoPer), SaldoInicial)

    End Sub
    Private Sub AddGrid(ByVal Operacion As Integer, ByVal CuentaP As Double, ByVal Centro As Integer, ByVal Cuenta As Integer, ByVal SubCuenta As Integer, ByVal SaldoAnt As Double, ByVal DebePer As Double, ByVal HaberPer As Double, ByVal SaldoPer As Double, ByVal Total As Double, ByVal SaldoInicial As Double)

        Dim RowGrid As DataRow

        RowGrid = DtGrid.NewRow()
        RowGrid("Color") = 0
        RowGrid("Operacion") = Operacion
        RowGrid("CuentaStr") = Format(CuentaP, "000-000000-00")
        RowGrid("CuentaP") = CuentaP
        RowGrid("Centro") = Centro
        RowGrid("Cuenta") = Cuenta
        RowGrid("SubCuenta") = SubCuenta
        RowGrid("SaldoInicial") = SaldoInicial
        RowGrid("DebePer") = DebePer
        RowGrid("HaberPer") = HaberPer
        RowGrid("SaldoAnt") = SaldoAnt
        RowGrid("SaldoPer") = SaldoPer
        RowGrid("Total") = Total
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Sub AddGridTotal(ByVal DebeAnt As Double, ByVal HaberAnt As Double, ByVal DebePer As Double, ByVal HaberPer As Double)

        Dim SaldoAnt As Double = DebeAnt - HaberAnt
        Dim SaldoPer As Double = DebePer - HaberPer

        Dim RowGrid As DataRow

        RowGrid = DtGrid.NewRow()
        DtGrid.Rows.Add(RowGrid)

        RowGrid = DtGrid.NewRow()
        RowGrid("Color") = 1
        RowGrid("Operacion") = 0
        RowGrid("CuentaStr") = ""
        RowGrid("CuentaP") = 0
        RowGrid("Centro") = 0
        RowGrid("Cuenta") = 0
        RowGrid("SubCuenta") = 0
        RowGrid("Debeper") = DebePer
        RowGrid("HaberPer") = HaberPer
        RowGrid("SaldoAnt") = SaldoAnt
        RowGrid("SaldoPer") = SaldoPer
        RowGrid("Total") = Trunca(SaldoAnt + SaldoPer)
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Color As New DataColumn("Color")
        Color.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Color)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim CuentaStr As New DataColumn("CuentaStr")
        CuentaStr.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(CuentaStr)

        Dim CuentaP As New DataColumn("CuentaP")
        CuentaP.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(CuentaP)

        Dim Centro As New DataColumn("Centro")
        Centro.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Centro)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cuenta)

        Dim SubCuenta As New DataColumn("SubCuenta")
        SubCuenta.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(SubCuenta)

        Dim Mensaje As New DataColumn("Mensaje")
        Mensaje.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Mensaje)

        Dim SaldoInicial As New DataColumn("SaldoInicial")
        SaldoInicial.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(SaldoInicial)

        Dim DebePer As New DataColumn("DebePer")
        DebePer.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(DebePer)

        Dim HaberPer As New DataColumn("HaberPer")
        HaberPer.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(HaberPer)

        Dim SaldoAnt As New DataColumn("SaldoAnt")
        SaldoAnt.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(SaldoAnt)

        Dim SaldoPer As New DataColumn("SaldoPer")
        SaldoPer.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(SaldoPer)

        Dim Total As New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Total)

    End Sub
    Private Sub LlenaCombosGrid()

        Centro.DataSource = Tablas.Leer("SELECT Centro,Nombre FROM CentrosDeCosto;")
        Dim Row As DataRow = Centro.DataSource.newrow
        Row("Centro") = 0
        Row("Nombre") = " "
        Centro.DataSource.rows.add(Row)
        Centro.DisplayMember = "Nombre"
        Centro.ValueMember = "Centro"

        Cuenta.DataSource = Tablas.Leer("SELECT Cuenta,Nombre FROM Cuentas;")
        Row = Cuenta.DataSource.newrow
        Row("Cuenta") = 0
        Row("Nombre") = " "
        Cuenta.DataSource.rows.add(Row)
        Cuenta.DisplayMember = "Nombre"
        Cuenta.ValueMember = "Cuenta"

        SubCuenta.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM SubCuentas;")
        Row = SubCuenta.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = " "
        SubCuenta.DataSource.rows.add(Row)
        SubCuenta.DisplayMember = "Nombre"
        SubCuenta.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If IsDBNull(e.Value) Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "CuentaStr" Then
            If PermisoTotal Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 1 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.Yellow
        End If

        If Grid.Columns(e.ColumnIndex).Name = "DebePer" Or Grid.Columns(e.ColumnIndex).Name = "HaberPer" Or Grid.Columns(e.ColumnIndex).Name = "SaldoAnt" Or Grid.Columns(e.ColumnIndex).Name = "SaldoPer" Or _
           Grid.Columns(e.ColumnIndex).Name = "DebeAnt" Or Grid.Columns(e.ColumnIndex).Name = "Total" Or Grid.Columns(e.ColumnIndex).Name = "SaldoInicial" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "CuentaStr" Then
            ListaSaldosContablesDetalle.PCuenta = Grid.CurrentRow.Cells("CuentaP").Value
            Select Case Grid.CurrentRow.Cells("Operacion").Value
                Case 1
                    ListaSaldosContablesDetalle.CheckAbierto.Checked = True
                    ListaSaldosContablesDetalle.CheckCerrado.Checked = False
                Case 2
                    ListaSaldosContablesDetalle.CheckAbierto.Checked = False
                    ListaSaldosContablesDetalle.CheckCerrado.Checked = True
                Case 0
                    ListaSaldosContablesDetalle.CheckAbierto.Checked = True
                    ListaSaldosContablesDetalle.CheckCerrado.Checked = True
            End Select
            ListaSaldosContablesDetalle.DateTimeDesde.Value = DateTimeDesde.Value
            ListaSaldosContablesDetalle.DateTimeHasta.Value = DateTimeHasta.Value
            ListaSaldosContablesDetalle.Show()
        End If

    End Sub


End Class