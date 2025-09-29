Public Class ListaSaldosOtrosPagosDetalle
    Public PBloqueaFunciones As Boolean
    Public PProveedor As Integer
    Public DtGrid As DataTable
    '
    Private WithEvents bs As New BindingSource
    Dim PView As DataView
    '
    Dim SqlB As String
    Dim SqlN As String
    Dim ConDetalle As Boolean
    Private Sub UnaCtaCteOtrosPagos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ComboProveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM OtrosProveedores;")
        Dim Row As DataRow = ComboProveedor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboProveedor.DataSource.rows.add(Row)
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = PProveedor
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        CreaDtGrid()

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Checked = False
        End If

        DateTimeDesde.Value = DateTimeHasta.Value.AddMonths(-1)

        ButtonAceptar_Click(Nothing, Nothing)

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
                DtGrid.Rows(LineaTotal).Item("Proveedor") = DtGrid.Rows(I).Item("Proveedor")
                Final = I
                BorraRowGrid(Comienzo, Final)
            End If
        Next

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Cuenta Corriente Otros Provedores", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonNoImputados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNoImputados.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeDocumentosNoImputados(3, DateTimeDesde.Value, DateTimeHasta.Value, CheckAbierto.Checked, CheckCerrado.Checked, SqlB, SqlN, ComboProveedor.DataSource, 0, Tipo.DataSource)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub TextMes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextMes.KeyPress

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
    Private Sub ButtonCompQueLoImputan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCompQueLoImputan.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        Dim Tipo As Integer = Grid.CurrentRow.Cells("TipoOrigen").Value

        Select Case Tipo
            Case 5010, 5005
                MsgBox("Imputanciones en el Comprobante.")
                Exit Sub
        End Select

        If Grid.CurrentRow.Cells("TipoOrigen").Value = 0 Then Tipo = 900

        HallaDocumentosQueImputan(Tipo, Grid.CurrentRow.Cells("Comprobante").Value, Grid.CurrentRow.Cells("operacion").Value, True)

    End Sub
    Private Sub PreparaArchivos()

        If CheckDetalle.Checked Then
            ConDetalle = True
        Else : ConDetalle = False
        End If

        DtGrid.Clear()

        Dim StrProveedor As String
        If ComboProveedor.SelectedValue <> 0 Then
            StrProveedor = "C.Proveedor = " & ComboProveedor.SelectedValue
        Else
            StrProveedor = "C.Proveedor LIKE '%'"
        End If

        Dim StrClave As String
        If ComboProveedor.SelectedValue <> 0 Then
            StrClave = "C.Clave = " & ComboProveedor.SelectedValue
        Else
            StrClave = "C.Clave LIKE '%'"
        End If

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        SqlB = "SELECT 1 as Operacion,C.Proveedor,5000 AS Tipo,C.Factura As Comprobante,C.Fecha,C.Estado,C.Saldo,C.Importe,0 AS Concepto,0 As MedioPagoRechazado,0 As Chequerechazado,0 As ChequeReemplazado,C.Comprobante AS CompOficial,C.Mes,C.Anio,C.Comentario,C.TipoPago FROM OtrasFacturasCabeza AS C WHERE " & StrProveedor & _
                    " UNION ALL " & _
               "SELECT 1 as Operacion,C.Clave AS Proveedor,0 AS Tipo,0 As Comprobante,'1/1/1800' As Fecha,C.Estado,C.SaldoInicial AS Saldo,0 AS Importe,0 AS Concepto,0 As MedioPagoRechazado,0 As Chequerechazado,0 As ChequeReemplazado,'' AS CompOficial,0 AS Mes,0 AS Anio,'' AS Comentario,0 AS TipoPago FROM OtrosProveedores AS C WHERE " & StrClave & _
                    " UNION ALL " & _
               "SELECT 1 as Operacion,C.Proveedor,C.TipoNota AS Tipo,C.Movimiento As Comprobante,C.Fecha,C.Estado,C.Saldo,C.Importe,0 AS Concepto,C.MedioPagoRechazado,C.Chequerechazado,C.ClaveChequeReemplazado As ChequeReemplazado,'' AS CompOficial,0 as Mes,0 AS Anio,C.Comentario,C.TipoPago FROM OtrosPagosCabeza AS C WHERE " & StrProveedor & ";"
        '
        SqlN = "SELECT 2 as Operacion,C.Proveedor,5000 AS Tipo,C.Factura As Comprobante,C.Fecha,C.Estado,C.Saldo,C.Importe,0 AS Concepto,0 As MedioPagoRechazado,0 As Chequerechazado,0 As ChequeReemplazado,C.Comprobante AS CompOficial,C.Mes,C.Anio,C.Comentario,C.TipoPago FROM OtrasFacturasCabeza AS C WHERE " & StrProveedor & _
                    " UNION ALL " & _
               "SELECT 2 as Operacion,C.Proveedor,C.TipoNota AS Tipo,C.Movimiento As Comprobante,C.Fecha,C.Estado,C.Saldo,C.Importe,0 AS Concepto,C.MedioPagoRechazado,C.Chequerechazado,C.ClaveChequeReemplazado As ChequeReemplazado,'' AS CompOficial,0 as Mes,0 AS Anio,C.Comentario,C.TipoPago FROM OtrosPagosCabeza AS C WHERE " & StrProveedor & ";"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Dt As New DataTable

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked = True Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        PView = Dt.DefaultView
        PView.Sort = "Proveedor,Fecha,Tipo,Comprobante"

        Dim SaldoCta As Double = 0
        Dim SaldoTotal As Double = 0
        Dim ProveedorAnt As Integer = 0
        Dim Tipo As Integer

        For Each Row As DataRowView In PView
            If DiferenciaDias(Row("Fecha"), DateTimeHasta.Value) >= 0 Then
                If ProveedorAnt <> Row("Proveedor") Then
                    If ProveedorAnt <> 0 Then
                        AddGrid(Nothing, Nothing, Nothing, Nothing, Nothing, 7000000, Nothing, Nothing, Nothing, Nothing, SaldoCta, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
                        SaldoTotal = SaldoTotal + SaldoCta
                    End If
                    ProveedorAnt = Row("Proveedor")
                    SaldoCta = 0
                    AddGrid(Row("Proveedor"), Nothing, Nothing, Nothing, Nothing, 6000000, Nothing, Nothing, Nothing, Nothing, SaldoCta, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
                End If
                Tipo = Row("Tipo")
                If Row("ChequeRechazado") Then Tipo = 3000000
                If Row("ChequeReemplazado") Then Tipo = 3000001
                If Row("Tipo") = 0 Then
                    'Analiza Saldo Inicial.
                    SaldoCta = SaldoCta + Row("Saldo")
                End If
                If Row("Tipo") = 5000 Then
                    'Analiza Recibo Sueldos.
                    If Row("Importe") < 0 Then
                        If Row("Estado") = 1 Then SaldoCta = SaldoCta + (-Row("Importe"))
                        If FechaOk(Row("Fecha")) Then
                            AddGrid(Nothing, Row("Operacion"), Row("Concepto"), Row("Fecha"), Row("Tipo"), Tipo, Row("Comprobante"), -Row("Importe"), 0, -Row("Saldo"), SaldoCta, Row("Estado"), 0, 0, Row("CompOficial"), Row("Mes"), Row("Anio"), Row("Comentario"), Row("TipoPago"))
                        End If
                    Else
                        If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe")
                        If FechaOk(Row("Fecha")) Then
                            AddGrid(Nothing, Row("Operacion"), Row("Concepto"), Row("Fecha"), Row("Tipo"), Tipo, Row("Comprobante"), 0, Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), 0, 0, Row("CompOficial"), Row("Mes"), Row("Anio"), Row("Comentario"), Row("TipoPago"))
                        End If
                    End If
                End If
                If Row("Tipo") = 5010 Then         'Analiza Pagos.
                    If Row("Estado") = 1 Then SaldoCta = SaldoCta + Row("Importe")
                    If FechaOk(Row("Fecha")) Then
                        AddGrid(Nothing, Row("Operacion"), 0, Row("Fecha"), Row("Tipo"), Tipo, Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), 0, 0, 0, 0, 0, Row("Comentario"), Row("TipoPago"))
                    End If
                End If
                If Row("Tipo") = 5020 Then         'Analiza Devolucion del proveedor.
                    If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe")
                    If FechaOk(Row("Fecha")) Then
                        AddGrid(Nothing, Row("Operacion"), 0, Row("Fecha"), Row("Tipo"), Tipo, Row("Comprobante"), 0, Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), 0, 0, 0, 0, 0, Row("Comentario"), Row("TipoPago"))
                    End If
                End If
                If Row("Tipo") = 5005 Then           'Nota debito.
                    If Row("Estado") = 1 Then SaldoCta = SaldoCta + Row("Importe")
                    If FechaOk(Row("Fecha")) Then
                        AddGrid(Nothing, Row("Operacion"), 0, Row("Fecha"), Row("Tipo"), Tipo, Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("MediopagoRechazado"), Row("ChequeRechazado"), 0, 0, 0, Row("Comentario"), Row("TipoPago"))
                    End If
                End If
                If Row("Tipo") = 5007 Then           'Nota Credito.
                    If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe")
                    If FechaOk(Row("Fecha")) Then
                        AddGrid(Nothing, Row("Operacion"), 0, Row("Fecha"), Row("Tipo"), Tipo, Row("Comprobante"), 0, Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("MediopagoRechazado"), Row("ChequeRechazado"), 0, 0, 0, Row("Comentario"), Row("TipoPago"))
                    End If
                End If
                If DiferenciaDias(Row("Fecha"), DateTimeDesde.Value) > 0 Then DtGrid.Rows(DtGrid.Rows.Count - 1).Item("SaldoCta") = SaldoCta
            End If
        Next
        If ProveedorAnt <> 0 Then
            AddGrid(Nothing, Nothing, Nothing, Nothing, Nothing, 7000000, Nothing, Nothing, Nothing, Nothing, SaldoCta, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            SaldoTotal = SaldoTotal + SaldoCta
        End If

        If ComboProveedor.SelectedValue = 0 Then BorraCuentasConSaldocero()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        TextSaldoCta.Text = FormatNumber(SaldoTotal, GDecimales)

        Dt.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub AddGrid(ByVal Proveedor As Integer, ByVal Operacion As Integer, ByVal Concepto As Integer, ByVal Fecha As DateTime, ByVal TipoOrigen As Integer, ByVal Tipo As Integer, ByVal Comprobante As Double, _
                   ByVal Debito As Double, ByVal Credito As Double, ByVal Saldo As Double, ByVal SaldoCta As Double, ByVal Estado As Integer, ByVal MedioPagoRechazado As Integer, ByVal ChequeRechazado As Integer, ByVal CompOficial As String, ByVal Mes As Integer, ByVal Anio As Integer, ByVal Comentario As String, ByVal TipoPago As Integer)

        If Not CheckDetalle.Checked And Tipo <> 6000000 And Tipo <> 7000000 Then Exit Sub

        If Not CheckConAnuladas.Checked And Estado = 3 Then Exit Sub

        Dim RowGrid As DataRow

        RowGrid = DtGrid.NewRow()
        RowGrid("Operacion") = Operacion
        RowGrid("Proveedor") = Proveedor
        RowGrid("Fecha") = Format(Fecha, "dd/MM/yyyy 00:00:00")
        RowGrid("Mes") = Mes
        RowGrid("Anio") = Anio
        RowGrid("TipoOrigen") = TipoOrigen
        RowGrid("Tipo") = Tipo
        RowGrid("Comprobante") = Comprobante
        RowGrid("Concepto") = Concepto
        RowGrid("Debito") = Debito
        RowGrid("Credito") = Credito
        RowGrid("Saldo") = Saldo
        RowGrid("SaldoCta") = SaldoCta
        RowGrid("CompOficial") = CompOficial
        RowGrid("Estado") = Estado
        If Estado = 1 Then RowGrid("Estado") = 0
        RowGrid("MedioPagoRechazado") = MedioPagoRechazado
        RowGrid("ChequeRechazado") = ChequeRechazado
        RowGrid("Comentario") = Comentario
        RowGrid("TipoPago") = TipoPago
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

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Mes As New DataColumn("Mes")
        Mes.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Mes)

        Dim Anio As New DataColumn("Anio")
        Anio.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Anio)

        Dim TipoOrigen As New DataColumn("TipoOrigen")
        TipoOrigen.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoOrigen)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Tipo)

        Dim TipoPago As New DataColumn("TipoPago")
        TipoPago.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoPago)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Concepto)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante)

        Dim CompOficial As New DataColumn("CompOficial")
        CompOficial.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(CompOficial)

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

        Dim Comentario As New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comentario)

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
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Tipo.DataSource = ArmaNotasOtrosPagos()
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Saldo Anterior"
        Row("Clave") = 6000000
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Total"
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

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM OtrosProveedores;")
        Row = Proveedor.DataSource.newrow
        Row("Nombre") = " "
        Row("Clave") = 0
        Proveedor.DataSource.rows.add(Row)
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        TipoPago.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 39;")
        Row = TipoPago.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        TipoPago.DataSource.rows.add(Row)
        TipoPago.DisplayMember = "Nombre"
        TipoPago.ValueMember = "Clave"

    End Sub
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

        If Grid.Columns(e.ColumnIndex).Name = "Mes" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = FormatNumber(e.Value, 0)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Anio" Then
            If e.Value = 0 Then e.Value = Format(0, "#")
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

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            If Grid.CurrentRow.Cells("TipoOrigen").Value = 5000 Then
                UnaFacturaOtrosProveedores.PRecibo = Grid.CurrentRow.Cells("Comprobante").Value
                UnaFacturaOtrosProveedores.PAbierto = Abierto
                UnaFacturaOtrosProveedores.PBloqueaFunciones = True
                UnaFacturaOtrosProveedores.ShowDialog()
                UnaFacturaOtrosProveedores.Dispose()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            End If
            If Grid.CurrentRow.Cells("TipoOrigen").Value = 5010 Or Grid.CurrentRow.Cells("TipoOrigen").Value = 5020 Then
                If Grid.CurrentRow.Cells("TipoOrigen").Value = 5010 Then
                    If EsReemplazoChequeOtrosProveedores(5010, Grid.CurrentRow.Cells("Comprobante").Value, Abierto) Then
                        UnChequeReemplazo.PTipoNota = 5010
                        UnChequeReemplazo.PNota = Grid.CurrentRow.Cells("Comprobante").Value
                        UnChequeReemplazo.PAbierto = Abierto
                        UnChequeReemplazo.ShowDialog()
                        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
                        Exit Sub
                    End If
                End If
                UnReciboOtrosProveedores.PNota = Grid.CurrentRow.Cells("Comprobante").Value
                UnReciboOtrosProveedores.PTipoNota = Grid.CurrentRow.Cells("TipoOrigen").Value
                UnReciboOtrosProveedores.PAbierto = Abierto
                UnReciboOtrosProveedores.PBloqueaFunciones = True
                UnReciboOtrosProveedores.ShowDialog()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            End If
            If Grid.CurrentRow.Cells("TipoOrigen").Value = 5005 Or Grid.CurrentRow.Cells("TipoOrigen").Value = 5007 Then
                UnReciboDebitoCreditoOtrosProveedores.PTipoNota = Grid.CurrentRow.Cells("TipoOrigen").Value
                UnReciboDebitoCreditoOtrosProveedores.PNota = Grid.CurrentRow.Cells("Comprobante").Value
                UnReciboDebitoCreditoOtrosProveedores.PAbierto = Abierto
                UnReciboDebitoCreditoOtrosProveedores.ShowDialog()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            End If
        End If

    End Sub
   
End Class