Public Class UnaCtaCteSueldos
    Public PBloqueaFunciones As Boolean
    Public PLegajo As Integer
    Public DtGrid As DataTable
    '
    Private WithEvents bs As New BindingSource
    '
    Dim PView As DataView
    Dim SqlB As String
    Dim SqlN As String
    Dim ConDetalle As Boolean
    Dim DtEmpleados As DataTable
    Private Sub ListaSaldosSueldosDetalle_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        DtEmpleados = New DataTable
        '        If Not Tablas.Read("SELECT 1 AS Operacion,Legajo,RIGHT('0000' + CAST(Legajo AS varchar),4) + ' - ' + Apellidos + ' ' + Nombres As Con,SaldoInicial,Estado FROM Empleados;", Conexion, DtEmpleados) Then End
        If Not Tablas.Read("SELECT 1 AS Operacion,Legajo, + Apellidos + ' ' + Nombres + ' - ' + RIGHT('0000' + CAST(Legajo AS varchar),4) As Con,SaldoInicial,Estado FROM Empleados;", Conexion, DtEmpleados) Then End
        If PermisoTotal Then
            '            If Not Tablas.Read("SELECT 2 AS Operacion,Legajo,RIGHT('0000' + CAST(Legajo AS varchar),4) + ' - ' + Apellidos + ' ' + Nombres As Con,SaldoInicial,Estado FROM Empleados;", ConexionN, DtEmpleados) Then End
            If Not Tablas.Read("SELECT 2 AS Operacion,Legajo, + Apellidos + ' ' + Nombres + ' - ' + RIGHT('0000' + CAST(Legajo AS varchar),4) As Con,SaldoInicial,Estado FROM Empleados;", ConexionN, DtEmpleados) Then End
        End If

        Dim Row As DataRow = DtEmpleados.NewRow
        Row("Legajo") = 0
        Row("Con") = ""
        DtEmpleados.Rows.Add(Row)
        ComboLegajos.DataSource = DtEmpleados
        ComboLegajos.DisplayMember = "Con"
        ComboLegajos.ValueMember = "Legajo"
        With ComboLegajos
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboLegajos.SelectedValue = PLegajo

        LlenaCombosGrid()

        CreaDtGrid()

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Checked = False
        End If

    End Sub
    Private Sub UnaCtaCteSueldos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar.Focus() : ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If TextMes.Text <> "" Then
            If Not (CInt(TextMes.Text) > 0 And CInt(TextMes.Text) < 13) Then
                MsgBox("Mes Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextMes.Focus()
                Exit Sub
            End If
        End If

        PreparaArchivos()

    End Sub
    Private Sub ButtonSaldoFacturas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImputaciones.Click

        If Not ConDetalle Then
            MsgBox("Debe Activar Vista Detalle.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ListaSaldosSueldosImputaciones.PAbierto = CheckAbierto.Checked
        ListaSaldosSueldosImputaciones.PCerrado = CheckCerrado.Checked
        ListaSaldosSueldosImputaciones.DateTimeDesde.Value = DateTimeDesde.Value
        ListaSaldosSueldosImputaciones.DateTimeHasta.Value = DateTimeHasta.Value
        ListaSaldosSueldosImputaciones.TextMes.Text = TextMes.Text
        ListaSaldosSueldosImputaciones.TextAnio.Text = TextAnio.Text
        ListaSaldosSueldosImputaciones.ShowDialog()
        ListaSaldosSueldosImputaciones.Dispose()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Cuentas Corrientes Sueldos", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonNoImputados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNoImputados.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeDocumentosNoImputados(2, DateTimeDesde.Value, DateTimeHasta.Value, CheckAbierto.Checked, CheckCerrado.Checked, SqlB, SqlN, ComboLegajos.DataSource, 0, Tipo.DataSource)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonSoloTotales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSoloTotales.Click

        Dim Comienzo As Integer
        Dim Final As Integer
        Dim LineaTotal As Integer = 0

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        For I As Integer = DtGrid.Rows.Count - 1 To 0 Step -1
            If DtGrid.Rows(I).Item("Tipo") = 7000000 Then
                LineaTotal = I
                Comienzo = I - 1
            End If
            If DtGrid.Rows(I).Item("Tipo") = 6000000 Then
                DtGrid.Rows(LineaTotal).Item("Legajo") = DtGrid.Rows(I).Item("Legajo")
                Final = I
                BorraRowGrid(Comienzo, Final)
            End If
        Next

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub TextMes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextMes.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextAnio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextAnio.KeyPress

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

        If CheckDetalle.Checked Then
            ConDetalle = True
        Else : ConDetalle = False
        End If

        DtGrid.Clear()

        Dim StrLegajo As String
        If ComboLegajos.SelectedValue <> 0 Then
            StrLegajo = " C.Legajo = " & ComboLegajos.SelectedValue
        Else : StrLegajo = " C.Legajo LIKE '%'"
        End If

        Dim StrMes As String
        If TextMes.Text <> "" Then
            If CInt(TextMes.Text) = 0 Or CInt(TextMes.Text) > 12 Then
                MsgBox("Mes Invalido.")
                Exit Sub
            End If
            StrMes = " AND C.Mes = " & CInt(TextMes.Text)
        End If

        Dim StrAnio As String
        If TextAnio.Text <> "" Then
            If CInt(TextAnio.Text) < 2000 And CInt(TextAnio.Text) > 3000 Then
                MsgBox("Año Invalido.")
                Exit Sub
            End If
            StrAnio = " AND C.Anio = " & CInt(TextAnio.Text)
        End If

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        SqlB = "SELECT 1 as Operacion,C.Legajo,4000 AS Tipo,C.Recibo As Comprobante,C.FechaContable AS Fecha,C.Estado,C.Saldo,C.Importe,0 as Concepto,0 As MedioPagoRechazado,0 As ChequeRechazado,0 As ChequeReemplazado,C.Mes,C.Anio,C.Comentario FROM RecibosSueldosCabeza AS C WHERE " & StrLegajo & _
                    " UNION ALL " & _
               "SELECT 1 as Operacion,C.Legajo,C.TipoNota AS Tipo,C.Movimiento As Comprobante,C.Fecha,C.Estado,C.Saldo,C.Importe,0 AS Concepto,C.MedioPagoRechazado,C.Chequerechazado,C.ClaveChequeReemplazado AS ChequeReemplazado,0 AS Mes,0 AS Anio,C.Comentario FROM SueldosMovimientoCabeza AS C WHERE " & StrLegajo & ";"
        '
        SqlN = "SELECT 2 as Operacion,C.Legajo,4000 AS Tipo,C.Recibo As Comprobante,C.FechaContable As Fecha,C.Estado,C.Saldo,C.Importe,0 as Concepto,0 As MedioPagoRechazado,0 As Chequerechazado,0 As ChequeReemplazado,C.Mes,C.Anio,C.Comentario FROM RecibosSueldosCabeza AS C WHERE " & StrLegajo & _
                    " UNION ALL " & _
               "SELECT 2 as Operacion,C.Legajo,C.TipoNota AS Tipo,C.Movimiento As Comprobante,C.Fecha,C.Estado,C.Saldo,C.Importe,0 as Concepto,C.MedioPagoRechazado,C.Chequerechazado,C.ClaveChequeReemplazado AS ChequeReemplazado,0 AS Mes,0 AS Anio,C.Comentario FROM SueldosMovimientoCabeza AS C  WHERE " & StrLegajo & ";"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Dt As New DataTable
        Dim RowsBusqueda() As DataRow

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked = True Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        If Not CheckConBajas.Checked Then
            For Each Row As DataRow In Dt.Rows
                RowsBusqueda = DtEmpleados.Select("Legajo = " & Row("Legajo"))
                If RowsBusqueda(0).Item("Estado") = 3 Then Row.Delete()
            Next
        End If
        If Not CheckConAnuladas.Checked Then
            For Each Row As DataRow In Dt.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    If Row("Estado") = 3 Then Row.Delete()
                End If
            Next
        End If

        PView = New DataView
        PView = Dt.DefaultView
        PView.Sort = "Legajo,Fecha,Tipo,Comprobante"

        Dim SaldoCta As Decimal = 0
        Dim TotalSaldo As Decimal = 0
        Dim TotalDebitos As Decimal = 0
        Dim TotalCreditos As Decimal = 0
        Dim LegajoAnt As Integer = 0
        Dim Tipo As Integer

        For Each Row As DataRowView In PView
            If DiferenciaDias(Row("Fecha"), DateTimeHasta.Value) >= 0 Then
                If Row("Legajo") <> LegajoAnt Then
                    If LegajoAnt <> 0 Then
                        TotalSaldo = TotalSaldo + SaldoCta
                        AddGrid(Nothing, Nothing, Nothing, Nothing, Nothing, 7000000, Nothing, Nothing, Nothing, Nothing, SaldoCta, Nothing, Nothing, Nothing, Nothing, Nothing, "")
                    End If
                    SaldoCta = 0
                    RowsBusqueda = DtEmpleados.Select("Legajo = " & Row("Legajo"))
                    'Analiza Saldo Inicial.
                    SaldoCta = SaldoCta + RowsBusqueda(0).Item("SaldoInicial")
                    AddGrid(Row("Legajo"), Row("Operacion"), Nothing, Nothing, Nothing, 6000000, Nothing, Nothing, Nothing, Nothing, SaldoCta, RowsBusqueda(0).Item("Estado"), Nothing, Nothing, Nothing, Nothing, "")
                    LegajoAnt = Row("Legajo")
                End If
                Tipo = Row("Tipo")
                If Row("ChequeRechazado") <> 0 Then Tipo = 3000000
                If Row("ChequeReemplazado") <> 0 Then Tipo = 3000001
                Select Case Row("Tipo")
                    Case 4000         'Analiza Recibo Sueldos.
                        If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe")
                        If FechaOk(Row("Fecha")) Then
                            If Row("Estado") <> 3 Then TotalCreditos = TotalCreditos + Row("Importe")
                            AddGrid(Nothing, Row("Operacion"), Row("Concepto"), Row("Fecha"), Row("Tipo"), Tipo, Row("Comprobante"), 0, Row("Importe"), 0, SaldoCta, Row("Estado"), 0, 0, Row("Mes"), Row("Anio"), Row("Comentario"))
                        End If
                    Case 4010, 4005    'Analiza Pagos y notas debito.
                        If Row("Estado") = 1 Then SaldoCta = SaldoCta + Row("Importe")
                        If FechaOk(Row("Fecha")) Then
                            If Row("Estado") <> 3 Then TotalDebitos = TotalDebitos + Row("Importe")
                            AddGrid(Nothing, Row("Operacion"), 0, Row("Fecha"), Row("Tipo"), Tipo, Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), 0, 0, 0, 0, Row("Comentario"))
                        End If
                    Case 4007          'Analiza notas credito. 
                        If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe")
                        If FechaOk(Row("Fecha")) Then
                            If Row("Estado") <> 3 Then TotalCreditos = TotalCreditos + Row("Importe")
                            AddGrid(Nothing, Row("Operacion"), 0, Row("Fecha"), Row("Tipo"), Tipo, Row("Comprobante"), 0, Row("Importe"), 0, SaldoCta, Row("Estado"), Row("MediopagoRechazado"), Row("ChequeRechazado"), 0, 0, Row("Comentario"))
                        End If
                    Case Else
                        MsgBox("Tipo Nota " & Row("Tipo") & " No contemplada.")
                        End
                End Select
                If DiferenciaDias(Row("Fecha"), DateTimeDesde.Value) > 0 Then DtGrid.Rows(DtGrid.Rows.Count - 1).Item("SaldoCta") = SaldoCta
            End If
        Next
        If LegajoAnt <> 0 Then
            TotalSaldo = TotalSaldo + SaldoCta
            AddGrid(Nothing, Nothing, Nothing, Nothing, Nothing, 7000000, Nothing, Nothing, Nothing, Nothing, SaldoCta, Nothing, Nothing, Nothing, Nothing, Nothing, "")
        End If
        TextTotalDebitos.Text = FormatNumber(TotalDebitos, 2)
        TextTotalCreditos.Text = FormatNumber(TotalCreditos, 2)

        If ComboLegajos.SelectedValue = 0 Then BorraCuentasConSaldocero()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        TextSaldoCta.Text = FormatNumber(TotalSaldo, GDecimales)

        Dt.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub AddGrid(ByVal Legajo As Integer, ByVal Operacion As Integer, ByVal Concepto As Integer, ByVal Fecha As DateTime, ByVal TipoOrigen As Integer, ByVal Tipo As Integer, ByVal Comprobante As Double, _
                   ByVal Debito As Double, ByVal Credito As Double, ByVal Saldo As Double, ByVal SaldoCta As Double, ByVal Estado As Integer, ByVal MedioPagoRechazado As Integer, ByVal ChequeRechazado As Integer, ByVal Mes As Integer, ByVal Anio As Integer, ByVal Comentario As String)

        If Not CheckDetalle.Checked And Tipo <> 6000000 And Tipo <> 7000000 Then Exit Sub

        Dim RowGrid As DataRow

        RowGrid = DtGrid.NewRow()
        If Legajo <> Nothing Then RowGrid("Legajo") = Legajo
        RowGrid("Operacion") = Operacion
        RowGrid("Fecha") = Format(Fecha, "dd/MM/yyyy 00:00:00")
        RowGrid("TipoOrigen") = TipoOrigen
        RowGrid("Tipo") = Tipo
        RowGrid("Mes") = Mes
        RowGrid("Anio") = Anio
        RowGrid("Comentario") = Comentario
        RowGrid("Comprobante") = Comprobante
        RowGrid("Concepto") = Concepto
        RowGrid("Debito") = Debito
        RowGrid("Credito") = Credito
        RowGrid("Saldo") = Saldo
        RowGrid("SaldoCta") = SaldoCta
        RowGrid("Estado") = Estado
        If Estado = 1 Then RowGrid("Estado") = 0
        RowGrid("MedioPagoRechazado") = MedioPagoRechazado
        RowGrid("ChequeRechazado") = ChequeRechazado
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Function FechaOk(ByVal Fecha As DateTime) As Boolean

        If DiferenciaDias(Fecha, DateTimeDesde.Value) > 0 Then Return False
        If DiferenciaDias(Fecha, DateTimeHasta.Value) < 0 Then Return False

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Legajo As New DataColumn("Legajo")
        Legajo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Legajo)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim TipoOrigen As New DataColumn("TipoOrigen")
        TipoOrigen.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoOrigen)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Tipo)

        Dim Mes As New DataColumn("Mes")
        Mes.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Mes)

        Dim Anio As New DataColumn("Anio")
        Anio.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Anio)

        Dim Comentario As New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comentario)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Concepto)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante)

        Dim Debito As New DataColumn("Debito")
        Debito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Debito)

        Dim Credito As New DataColumn("Credito")
        Credito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Credito)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Saldo)

        Dim SaldoCta As New DataColumn("SaldoCta")
        SaldoCta.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(SaldoCta)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim MedioPagoRechazado As New DataColumn("MedioPagoRechazado")
        MedioPagoRechazado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(MedioPagoRechazado)

        Dim ChequeRechazado As New DataColumn("ChequeRechazado")
        ChequeRechazado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ChequeRechazado)

    End Sub
    Private Sub LlenaCombosGrid()

        Tipo.DataSource = ArmaMovimientosSueldo()
        Dim Row As DataRow = Tipo.DataSource.newrow
        Row("Nombre") = "SALDO ANTERIOR"
        Row("Clave") = 6000000
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "TOTAL LEGAJO"
        Row("Clave") = 7000000
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Rechazo Cheque"
        Row("Clave") = 3000000
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Reemplazo Cheque"
        Row("Clave") = 3000001
        Tipo.DataSource.rows.add(Row)
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        Dim Dt2 As New DataTable
        Dt2 = DtEmpleados
        Legajo.DataSource = DtEmpleados
        Legajo.DisplayMember = "Con"
        Legajo.ValueMember = "Legajo"

    End Sub
    Private Sub BorraCuentasConSaldocero()

        Dim Comienzo As Integer
        Dim Final As Integer

        For I As Integer = DtGrid.Rows.Count - 1 To 0 Step -1
            If DtGrid.Rows(I).Item("Tipo") = 7000000 And DtGrid.Rows(I).Item("SaldoCta") = 0 Then
                Comienzo = I
                For Final = I To 0 Step -1
                    If DtGrid.Rows(Final).Item("Tipo") = 6000000 Then
                        BorraRowGrid(Comienzo, Final)
                        I = Final
                        Exit For
                    End If
                Next
            End If
        Next

    End Sub
    Private Sub BorraRowGrid(ByVal Comienzo As Integer, ByVal Final As Integer)

        For I As Integer = Comienzo To Final Step -1
            Dim Row As DataRow = DtGrid.Rows(I)
            Row.Delete()
        Next

    End Sub
    Public Function EsLegajoBaja(ByVal Legajo As Integer, ByVal Operacion As Integer) As Boolean

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Estado FROM Empleados WHERE Legajo = " & Legajo & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 3 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Empleados.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else : e.Value = FormatNumber(e.Value, 0)
            End If
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Debito" Or Grid.Columns(e.ColumnIndex).Name = "Credito" Or Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Mes" Or Grid.Columns(e.ColumnIndex).Name = "Anio" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = Format(e.Value, 0)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "SaldoCta" Then
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

        If Grid.CurrentRow.Cells("Comprobante").Value = 0 Then Exit Sub

        If Grid.CurrentRow.Cells("TipoOrigen").Value = 4000 Then
            UnReciboSueldo.PRecibo = Grid.CurrentRow.Cells("Comprobante").Value
            UnReciboSueldo.PAbierto = Abierto
            UnReciboSueldo.ShowDialog()
            UnReciboSueldo.Dispose()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If
        If Grid.CurrentRow.Cells("TipoOrigen").Value = 4010 Then
            If EsReemplazoChequeSueldos(4010, Grid.CurrentRow.Cells("Comprobante").Value, Abierto) Then
                UnChequeReemplazo.PTipoNota = 4010
                UnChequeReemplazo.PNota = Grid.CurrentRow.Cells("Comprobante").Value
                UnChequeReemplazo.PAbierto = Abierto
                UnChequeReemplazo.ShowDialog()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
                Exit Sub
            End If
            UnaOrdenPagoSueldos.PNota = Grid.CurrentRow.Cells("Comprobante").Value
            UnaOrdenPagoSueldos.PTipoNota = 4010
            UnaOrdenPagoSueldos.PAbierto = Abierto
            UnaOrdenPagoSueldos.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If
        If Grid.CurrentRow.Cells("TipoOrigen").Value = 4007 Then
            UnReciboDebitoCreditoGenerica.PNota = Grid.CurrentRow.Cells("Comprobante").Value
            UnReciboDebitoCreditoGenerica.PTipoNota = 4007
            UnReciboDebitoCreditoGenerica.PAbierto = Abierto
            UnReciboDebitoCreditoGenerica.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

    End Sub

End Class