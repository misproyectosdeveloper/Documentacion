Public Class ListaSaldosContablesDetalle
    Public PCuenta As Double
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaSaldosContablesDetalle_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
        End If

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaCombosGrid()

        CreaDtGrid()

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dim CentroW As Integer
        Dim CuentaW As Integer
        Dim SubCuentaW As Integer

        HallaPartesCuenta(PCuenta, CentroW, CuentaW, SubCuentaW)
        TextCentro.Text = CentroW
        TextCuenta.Text = Format(CuentaW, "000000")
        TextSubCuenta.Text = Format(SubCuentaW, "00")

        ButtonAceptar()

    End Sub
    Private Sub UnaCtaCte_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar()

    End Sub
    Private Sub ButtonAceptar()

        Dim SqlFecha As String
        SqlFecha = " C.IntFecha >= " & DateTimeDesde.Value.Year & Format(DateTimeDesde.Value.Month, "00") & Format(DateTimeDesde.Value.Day, "00") & _
                   " AND C.IntFecha <= " & DateTimeHasta.Value.Year & Format(DateTimeHasta.Value.Month, "00") & Format(DateTimeHasta.Value.Day, "00")

        SqlFecha = " C.IntFecha <= " & DateTimeHasta.Value.Year & Format(DateTimeHasta.Value.Month, "00") & Format(DateTimeHasta.Value.Day, "00")

        Dim SqlFechaPlanDeCuentas As String
        SqlFechaPlanDeCuentas = "Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        SqlFechaPlanDeCuentas = "Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlCuenta As String
        SqlCuenta = " AND D.Cuenta = " & PCuenta

        Dim SqlCuentaPlanDeCuentas As String = ""
        SqlCuentaPlanDeCuentas = " AND ClaveCuenta = " & PCuenta

        Dim IntFecha As String = "CAST(YEAR(Fecha) as varchar(4)) + RIGHT('00' + cast(MONTH(Fecha) as varchar),2) + RIGHT('00' + cast(DAY(Fecha) as varchar),2)"

        SqlB = "SELECT 1 As Operacion,D.Cuenta,D.Debe,D.haber,C.IntFecha,C.Estado,C.Asiento,C.Comentario,0 AS SaldoInicial,C.TipoDocumento,C.Documento,C.TipoComprobante FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE" & SqlFecha & SqlCuenta & _
               "UNION ALL " & _
               "SELECT 1 As Operacion,ClaveCuenta AS Cuenta,0 AS Debe,0 AS haber," & IntFecha & " AS IntFecha,1 AS Estado,0 As Asiento,'' AS Comentario,SaldoInicial,0 AS TipoDocumento,0 AS Documento,0 AS TipoComprobante FROM PlanDeCuentas WHERE SaldoInicial <> 0 AND " & SqlFechaPlanDeCuentas & SqlCuentaPlanDeCuentas & ";"

        SqlN = "SELECT 2 As Operacion,D.Cuenta,D.Debe,D.haber,C.IntFecha,C.Estado,C.Asiento,C.Comentario,0 As SaldoInicial,C.TipoDocumento,C.Documento,C.TipoComprobante FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE" & SqlFecha & SqlCuenta & _
               "UNION ALL " & _
               "SELECT 2 As Operacion,ClaveCuenta AS Cuenta,0 AS Debe,0 AS haber," & IntFecha & " AS IntFecha,1 AS Estado,0 As Asiento,'' AS Comentario,SaldoInicial,0 AS TipoDocumento,0 AS Documento,0 AS TipoComprobante FROM PlanDeCuentas WHERE SaldoInicial <> 0 AND " & SqlFechaPlanDeCuentas & SqlCuentaPlanDeCuentas & ";"

        PreparaArchivos()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, "", "", "")

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
    Private Sub PreparaArchivos()

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
        View.Sort = "IntFecha,Asiento,Operacion"

        Dim TotalDebe As Double = 0
        Dim TotalHaber As Double = 0
        Dim TotalDebeAnt As Double = 0
        Dim TotalHaberAnt As Double = 0
        Dim Saldo As Double = 0

        Dim IntFechaDesde As Integer = DateTimeDesde.Value.Year & Format(DateTimeDesde.Value.Month, "00") & Format(DateTimeDesde.Value.Day, "00")

        AddGrid(0, 0, 0, 0, 0, 1, "Saldo Anterior", 0, 0, 0, 0)

        For Each Row As DataRowView In View
            If Row("Asiento") = 0 Then
                If Row("SaldoInicial") < 0 Then
                    Row("Haber") = -Row("SaldoInicial")
                Else : Row("Debe") = Row("SaldoInicial")
                End If
                Row("Comentario") = "Saldo Inicial"
            End If
            If Row("IntFecha") < IntFechaDesde Then
                If Row("Estado") = 1 Then
                    DtGrid.Rows(0).Item("DebePer") = Trunca(DtGrid.Rows(0).Item("DebePer") + Row("Debe"))
                    DtGrid.Rows(0).Item("HaberPer") = Trunca(DtGrid.Rows(0).Item("HaberPer") + Row("Haber"))
                    DtGrid.Rows(0).Item("Saldo") = Trunca(DtGrid.Rows(0).Item("Saldo") + Row("Debe") - Row("Haber"))
                    TotalDebe = TotalDebe + Row("Debe")
                    TotalHaber = TotalHaber + Row("Haber")
                    Saldo = Trunca(Saldo + Row("Debe") - Row("Haber"))
                End If
            Else
                If Row("Estado") = 1 Then
                    TotalDebe = TotalDebe + Row("Debe")
                    TotalHaber = TotalHaber + Row("Haber")
                    Saldo = Trunca(Saldo + Row("Debe") - Row("Haber"))
                End If
                AddGrid(Row("Operacion"), Row("Asiento"), Row("Debe"), Row("Haber"), Row("IntFecha"), Row("Estado"), Row("Comentario"), Saldo, Row("TipoDocumento"), Row("Documento"), Row("TipoComprobante"))
            End If
        Next
        AddGridTotales(0, 0, TotalDebe, TotalHaber, 0, 0, Saldo)

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub AddGrid(ByVal Operacion As Integer, ByVal Asiento As String, ByVal DebePer As Double, ByVal HaberPer As Double, ByVal Intfecha As Integer, ByVal Estado As Integer, ByVal Comentario As String, ByVal Saldo As Double, ByVal TipoDocumento As Integer, ByVal Documento As Decimal, ByVal TipoComprobante As Integer)

        Dim RowGrid As DataRow

        RowGrid = DtGrid.NewRow()
        RowGrid("Operacion") = Operacion
        RowGrid("TipoDocumento") = TipoDocumento
        RowGrid("Documento") = Documento
        RowGrid("Asiento") = Asiento
        If Intfecha <> 0 Then RowGrid("Fecha") = Intfecha.ToString.Substring(6, 2) & "/" & Intfecha.ToString.Substring(4, 2) & "/" & Intfecha.ToString.Substring(0, 4)
        RowGrid("Mensaje") = Comentario
        RowGrid("DebePer") = DebePer
        RowGrid("HaberPer") = HaberPer
        RowGrid("Saldo") = Saldo
        RowGrid("TipoComprobante") = TipoComprobante
        RowGrid("Comentario") = Comentario
        RowGrid("Estado") = Estado
        If Estado <> 3 Then RowGrid("Estado") = 0
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Sub AddGridTotales(ByVal Operacion As Integer, ByVal Asiento As String, ByVal DebePer As Double, ByVal HaberPer As Double, ByVal Intfecha As Integer, ByVal Estado As Integer, ByVal Saldo As Double)

        Dim RowGrid As DataRow

        RowGrid = DtGrid.NewRow()
        DtGrid.Rows.Add(RowGrid)

        RowGrid = DtGrid.NewRow()
        RowGrid("Operacion") = Operacion
        RowGrid("TipoDocumento") = 0
        RowGrid("Documento") = 0
        RowGrid("Asiento") = Asiento
        If Intfecha <> 0 Then RowGrid("Fecha") = Intfecha.ToString.Substring(6, 2) & "/" & Intfecha.ToString.Substring(4, 2) & "/" & Intfecha.ToString.Substring(0, 4)
        RowGrid("Mensaje") = ""
        RowGrid("DebePer") = DebePer
        RowGrid("HaberPer") = HaberPer
        RowGrid("Saldo") = Saldo
        RowGrid("TipoComprobante") = 0
        RowGrid("Estado") = Estado
        If Estado <> 3 Then RowGrid("Estado") = 0
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim TipoDocumento As New DataColumn("TipoDocumento")
        TipoDocumento.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoDocumento)

        Dim Documento As New DataColumn("Documento")
        Documento.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Documento)

        Dim Asiento As New DataColumn("Asiento")
        Asiento.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Asiento)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Mensaje As New DataColumn("Mensaje")
        Mensaje.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Mensaje)

        Dim DebePer As New DataColumn("DebePer")
        DebePer.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(DebePer)

        Dim HaberPer As New DataColumn("HaberPer")
        HaberPer.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(HaberPer)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Saldo)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim TipoComprobante As New DataColumn("TipoComprobante")
        TipoComprobante.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoComprobante)

        Dim Comentario As New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comentario)

    End Sub
    Private Sub LlenaCombosGrid()

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        Dim Dt As DataTable = ArmaDocumentosAsientos()
        Dim Row As DataRow = Dt.NewRow
        Row("Codigo") = 0
        Row("Nombre") = ""
        Dt.Rows.Add(Row)

        TipoDocumento.DataSource = Dt
        TipoDocumento.DisplayMember = "Nombre"
        TipoDocumento.ValueMember = "Codigo"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If IsDBNull(e.Value) Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Asiento" Then
            If e.Value = 0 Then e.Value = Format(0, "#")
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "DebePer" Or Grid.Columns(e.ColumnIndex).Name = "HaberPer" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Documento" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = NumeroEditado(e.Value)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            e.Value = FormatNumber(e.Value, GDecimales)
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

        If Grid.Columns(e.ColumnIndex).Name = "Asiento" Then
            If Abierto Then
                UnAsiento.PAbierto = True
            Else : UnAsiento.PAbierto = False
            End If
            UnAsiento.PBloqueaFunciones = True
            UnAsiento.PAsiento = Grid.Rows(e.RowIndex).Cells("Asiento").Value
            UnAsiento.ShowDialog()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Documento" Then
            MuestraComprobanteDelTipoAsiento(Grid.Rows(e.RowIndex).Cells("TipoDocumento").Value, Grid.Rows(e.RowIndex).Cells("TipoComprobante").Value, Grid.Rows(e.RowIndex).Cells("Documento").Value, Abierto)
        End If

    End Sub
   

End Class