Public Class ListaSaldosFondosFijoDetalle
    Public PBloqueaFunciones As Boolean
    Public PFondoFijo As Integer
    Public DtGrid As DataTable
    '
    Private WithEvents bs As New BindingSource
    Dim PView As DataView
    '
    Dim SqlB As String
    Dim SqlN As String
    Dim ConDetalle As Boolean
    Private Sub ListaSaldosFondosFijoDetalle_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ComboFondoFijo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM MaestroFondoFijo;")
        Dim Row As DataRow = ComboFondoFijo.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboFondoFijo.DataSource.rows.add(Row)
        ComboFondoFijo.DisplayMember = "Nombre"
        ComboFondoFijo.ValueMember = "Clave"
        ComboFondoFijo.SelectedValue = PFondoFijo
        With ComboFondoFijo
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

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar.Focus() : ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaSaldosFondosFijoDetalle_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Exit Sub
        End If

        If TextNumero.Text <> "" Then
            If CInt(TextNumero.Text) = 0 Then
                MsgBox("Incorrecto Numero Fondo Fijo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextNumero.Focus()
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
                DtGrid.Rows(LineaTotal).Item("FondoFijo") = DtGrid.Rows(I).Item("FondoFijo")
                Final = I
                BorraRowGrid(Comienzo, Final)
            End If
        Next

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Cuenta Corriente Fondos Fijos", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub TextNumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumero.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, 0)

    End Sub
    Private Sub ButtonNoImputados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNoImputados.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Numero As Integer = 0
        If TextNumero.Text = "" Then
            Numero = 0
        Else
            Numero = CInt(TextNumero.Text)
        End If

        InformeDocumentosFondoFijoNoImputados(ComboFondoFijo.SelectedValue, Numero, DateTimeDesde.Value, DateTimeHasta.Value, CheckAbierto.Checked, CheckCerrado.Checked, ComboFondoFijo.DataSource, 0, Tipo.DataSource)

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

        If CheckDetalle.Checked Then
            ConDetalle = True
        Else : ConDetalle = False
        End If

        DtGrid.Clear()

        Dim StrFondoFijo As String
        If ComboFondoFijo.SelectedValue <> 0 Then
            StrFondoFijo = "FondoFijo = " & ComboFondoFijo.SelectedValue
        Else
            StrFondoFijo = "FondoFijo LIKE '%'"
        End If

        Dim StrFondoFijoF As String
        If ComboFondoFijo.SelectedValue <> 0 Then
            StrFondoFijoF = "M.FondoFijo = " & ComboFondoFijo.SelectedValue
        Else
            StrFondoFijoF = "M.FondoFijo LIKE '%'"
        End If

        Dim StrFondoFijoR As String
        If ComboFondoFijo.SelectedValue <> 0 Then
            StrFondoFijoR = "Emisor = " & ComboFondoFijo.SelectedValue
        Else
            StrFondoFijoR = "Emisor LIKE '%'"
        End If

        Dim StrNumeroFondoFijo As String = ""
        If TextNumero.Text <> "" Then
            If CInt(TextNumero.Text) <> 0 Then
                StrNumeroFondoFijo = " AND Numero = " & CInt(TextNumero.Text)
            End If
        End If

        Dim StrNumeroFondoFijoF As String = ""
        If TextNumero.Text <> "" Then
            If CInt(TextNumero.Text) <> 0 Then
                StrNumeroFondoFijoF = " AND M.Numero = " & CInt(TextNumero.Text)
            End If
        End If

        Dim StrNumeroFondoFijoR As String = ""
        If TextNumero.Text <> "" Then
            If CInt(TextNumero.Text) <> 0 Then
                StrNumeroFondoFijoR = " AND NumeroFondoFijo = " & CInt(TextNumero.Text)
            End If
        End If

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        SqlB = "SELECT 1 AS Operacion,M.FondoFijo,M.Numero,M.Fecha,M.Tipo,M.Movimiento As Comprobante,M.Importe,M.Estado,ChequeRechazado,ClaveChequeReemplazado,M.Saldo,F.Cerrado AS Cerrado FROM MovimientosFondoFijoCabeza AS M INNER JOIN FondosFijos AS F ON M.Numero = F.Numero WHERE " & StrFondoFijoF & StrNumeroFondoFijoF & _
               " UNION ALL " & _
               "SELECT 1 As Operacion,FondoFijo,Numero,Fecha,7003 AS Tipo,Rendicion As Comprobante,ImporteFacturas AS Importe,1 AS Estado,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,Saldo,Cerrado FROM RendicionFondoFijo WHERE " & StrFondoFijo & StrNumeroFondoFijo & _
               " UNION ALL " & _
               "SELECT 1 As Operacion,Emisor AS FondoFijo,NumeroFondoFijo as Numero,Fecha,7004 AS Tipo,Nota As Comprobante,Importe,Estado,ChequeRechazado,ClaveChequeReemplazado,Saldo,0 As Cerrado FROM RecibosCabeza WHERE NumeroFondoFijo <> 0 AND " & StrFondoFijoR & StrNumeroFondoFijoR & ";"


        SqlN = "SELECT 2 AS Operacion,M.FondoFijo,M.Numero,M.Fecha,M.Tipo,M.Movimiento As Comprobante,M.Importe,M.Estado,ChequeRechazado,ClaveChequeReemplazado,M.Saldo,F.Cerrado AS Cerrado FROM MovimientosFondoFijoCabeza AS M INNER JOIN FondosFijos AS F ON M.Numero = F.Numero WHERE " & StrFondoFijoF & StrNumeroFondoFijoF & _
               " UNION ALL " & _
               "SELECT 2 As Operacion,FondoFijo,Numero,Fecha,7003 AS Tipo,Rendicion As Comprobante,ImporteFacturas AS Importe,1 AS Estado,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,Saldo,Cerrado FROM RendicionFondoFijo WHERE " & StrFondoFijo & StrNumeroFondoFijo & _
               " UNION ALL " & _
               "SELECT 2 As Operacion,Emisor AS FondoFijo,NumeroFondoFijo as Numero,Fecha,7004 AS Tipo,Nota As Comprobante,Importe,Estado,ChequeRechazado,ClaveChequeReemplazado,Saldo,0 As Cerrado FROM RecibosCabeza WHERE NumeroFondoFijo <> 0 AND " & StrFondoFijoR & StrNumeroFondoFijoR & ";"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Dt As New DataTable

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked = True Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        PView = Dt.DefaultView
        PView.Sort = "FondoFijo,Numero,Fecha"

        Dim SaldoCta As Decimal = 0
        Dim SaldoTotal As Decimal = 0
        Dim ClaveAnt As String = ""
        Dim NumeroAnt As String = 0
        Dim Tipo As Integer
        Dim SinAsignar As Decimal = 0

        For Each Row As DataRowView In PView
            If DiferenciaDias(Row("Fecha"), DateTimeHasta.Value) >= 0 Then
                If ClaveAnt <> Row("FondoFijo") & Row("Numero") Then
                    If ClaveAnt <> "" Then
                        AddGrid(Nothing, Nothing, Nothing, Nothing, 7000000, Nothing, Nothing, Nothing, Nothing, SaldoCta, 0, Nothing, Nothing, NumeroAnt)
                        SaldoTotal = SaldoTotal + SaldoCta
                    End If
                    ClaveAnt = Row("FondoFijo") & Row("Numero")
                    NumeroAnt = Row("Numero")
                    SaldoCta = 0
                    AddGrid(Row("FondoFijo"), Row("Operacion"), Nothing, Nothing, 6000000, Nothing, Nothing, Nothing, Nothing, SaldoCta, 0, Nothing, Nothing, Row("Numero"))
                End If
                Tipo = Row("Tipo")
                If Row("ChequeRechazado") <> 0 Then Tipo = 3000000 : Row("Saldo") = 0
                If Row("ClaveChequeReemplazado") <> 0 Then Tipo = 3000001
                If Row("Tipo") = 7001 Then 'Ajuste aumento.
                    If Row("Estado") = 1 Then SaldoCta = SaldoCta + CDec(Row("Importe"))
                    If Row("Cerrado") Then
                        SinAsignar = 0
                    Else
                        SinAsignar = Row("Importe")
                    End If
                    If FechaOk(Row("Fecha")) Then
                        AddGrid(Nothing, Row("Operacion"), Row("Fecha"), Row("Tipo"), Tipo, Row("Comprobante"), Row("Importe"), 0, SinAsignar, SaldoCta, Row("Estado"), 0, 0, Row("Numero"))
                    End If
                End If
                If Row("Tipo") = 7002 Then         'Ajuste disminuye.
                    If Row("Estado") = 1 Then SaldoCta = SaldoCta - CDec(Row("Importe"))
                    If Row("Cerrado") Then
                        SinAsignar = 0
                    Else
                        SinAsignar = Row("Importe")
                    End If
                    If FechaOk(Row("Fecha")) Then
                        AddGrid(Nothing, Row("Operacion"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), 0, Row("Importe"), SinAsignar, SaldoCta, Row("Estado"), 0, 0, Row("Numero"))
                    End If
                End If
                If Row("Tipo") = 7003 Then         'Rendicion.
                    If Row("Estado") = 1 Then SaldoCta = SaldoCta - CDec(Row("Importe"))
                    If Row("Cerrado") Then
                        SinAsignar = 0
                    Else
                        SinAsignar = Row("Saldo")
                    End If
                    If FechaOk(Row("Fecha")) Then
                        AddGrid(Nothing, Row("Operacion"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), 0, Row("Importe"), SinAsignar, SaldoCta, Row("Estado"), 0, 0, Row("Numero"))
                    End If
                End If
                If Row("Tipo") = 7004 Then         'Reposicion.
                    If Row("Estado") = 1 Then SaldoCta = SaldoCta + CDec(Row("Importe"))
                    If FechaOk(Row("Fecha")) Then
                        AddGrid(Nothing, Row("Operacion"), Row("Fecha"), Row("Tipo"), Tipo, Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), 0, 0, Row("Numero"))
                    End If
                End If
                If Row("Tipo") = 7005 Then         'Nota Debito.
                    If Row("Estado") = 1 Then SaldoCta = SaldoCta + CDec(Row("Importe"))
                    If FechaOk(Row("Fecha")) Then
                        AddGrid(Nothing, Row("Operacion"), Row("Fecha"), Row("Tipo"), Tipo, Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), 0, 0, Row("Numero"))
                    End If
                End If
                If Row("Tipo") = 7007 Then         'Nota Credito.
                    If Row("Estado") = 1 Then SaldoCta = SaldoCta - CDec(Row("Importe"))
                    If FechaOk(Row("Fecha")) Then
                        AddGrid(Nothing, Row("Operacion"), Row("Fecha"), Row("Tipo"), Tipo, Row("Comprobante"), 0, Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), 0, 0, Row("Numero"))
                    End If
                End If
                If DiferenciaDias(Row("Fecha"), DateTimeDesde.Value) > 0 Then DtGrid.Rows(DtGrid.Rows.Count - 1).Item("SaldoCta") = SaldoCta
            End If
        Next
        If ClaveAnt <> "" Then
            AddGrid(Nothing, Nothing, Nothing, Nothing, 7000000, Nothing, Nothing, Nothing, Nothing, SaldoCta, 0, Nothing, Nothing, NumeroAnt)
            SaldoTotal = SaldoTotal + SaldoCta
        End If

        If ComboFondoFijo.SelectedValue = 0 Then BorraCuentasConSaldocero()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        TextSaldoCta.Text = FormatNumber(SaldoTotal, GDecimales)

        Dt.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub AddGrid(ByVal FondoFijo As Integer, ByVal Operacion As Integer, ByVal Fecha As DateTime, ByVal TipoOrigen As Integer, ByVal Tipo As Integer, ByVal Comprobante As Double, _
                   ByVal Debito As Double, ByVal Credito As Double, ByVal Saldo As Double, ByVal SaldoCta As Double, ByVal Estado As Integer, ByVal MedioPagoRechazado As Integer, ByVal ChequeRechazado As Integer, ByVal Numero As Integer)

        If Not CheckDetalle.Checked And Tipo <> 6000000 And Tipo <> 7000000 Then Exit Sub

        If Not CheckConAnuladas.Checked And Estado = 3 Then Exit Sub

        Dim RowGrid As DataRow

        RowGrid = DtGrid.NewRow()
        RowGrid("Operacion") = Operacion
        RowGrid("FondoFijo") = FondoFijo
        RowGrid("Numero") = Numero
        RowGrid("Fecha") = Format(Fecha, "dd/MM/yyyy 00:00:00")
        RowGrid("TipoOrigen") = TipoOrigen
        RowGrid("Tipo") = Tipo
        RowGrid("Comprobante") = Comprobante
        RowGrid("Debito") = Debito
        RowGrid("Credito") = Credito
        RowGrid("Saldo") = Saldo
        RowGrid("SaldoCta") = SaldoCta
        RowGrid("Estado") = Estado
        If Estado = 1 Then RowGrid("Estado") = 0
        RowGrid("Comentario") = ""
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

        Dim FondoFijo As New DataColumn("FondoFijo")
        FondoFijo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(FondoFijo)

        Dim Numero As New DataColumn("Numero")
        Numero.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Numero)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim TipoOrigen As New DataColumn("TipoOrigen")
        TipoOrigen.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoOrigen)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Tipo)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante)

        Dim FechaMovimiento As New DataColumn("FechaMovimiento")
        FechaMovimiento.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaMovimiento)

        Dim Debito As New DataColumn("Debito")
        Debito.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Debito)

        Dim Credito As New DataColumn("Credito")
        Credito.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Credito)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Saldo)

        Dim SaldoCta As New DataColumn("SaldoCta")
        SaldoCta.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(SaldoCta)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

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

        Tipo.DataSource = ArmaNotasFondosFijo()
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
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Rendición"
        Row("Clave") = 7003
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Reposición"
        Row("Clave") = 7004
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Nota Debito"
        Row("Clave") = 7005
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Nota Credito"
        Row("Clave") = 7007
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        Tipo.DataSource.rows.add(Row)
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Clave"

        FondoFijo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM MaestroFondoFijo;")
        Row = FondoFijo.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        FondoFijo.DataSource.rows.add(Row)
        FondoFijo.DisplayMember = "Nombre"
        FondoFijo.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else : e.Value = NumeroEditado(e.Value)
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

        If Grid.Columns(e.ColumnIndex).Name = "SaldoCta" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        Dim Abierto As Boolean
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        Dim Imputa As Boolean
        If PermisoEscritura(213) Then
            Imputa = True
        Else
            Imputa = False
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            If Grid.CurrentRow.Cells("TipoOrigen").Value = 7001 Or Grid.CurrentRow.Cells("TipoOrigen").Value = 7002 Then
                UnMovimientoFondoFijo.PMovimiento = Grid.CurrentRow.Cells("Comprobante").Value
                UnMovimientoFondoFijo.PAbierto = Abierto
                UnMovimientoFondoFijo.ShowDialog()
                UnMovimientoFondoFijo.Dispose()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            End If
            If Grid.CurrentRow.Cells("TipoOrigen").Value = 7003 Then
                UnaRendicion.PRendicion = Grid.CurrentRow.Cells("Comprobante").Value
                UnaRendicion.PBloqueaFunciones = True
                UnaRendicion.PAbierto = Abierto
                UnaRendicion.ShowDialog()
                UnaRendicion.Dispose()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            End If
            If Grid.CurrentRow.Cells("TipoOrigen").Value = 7004 Then
                UnReciboReposicion.PNota = Grid.CurrentRow.Cells("Comprobante").Value
                UnReciboReposicion.PAbierto = Abierto
                UnReciboReposicion.PImputa = Imputa
                UnReciboReposicion.ShowDialog()
                UnReciboReposicion.Dispose()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            End If
            If Grid.CurrentRow.Cells("TipoOrigen").Value = 7005 Then
                UnReciboDebitoCreditoGenerica.PNota = Grid.CurrentRow.Cells("Comprobante").Value
                UnReciboDebitoCreditoGenerica.PTipoNota = 7005
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.ShowDialog()
                UnReciboDebitoCreditoGenerica.Dispose()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            End If
            If Grid.CurrentRow.Cells("TipoOrigen").Value = 7007 Then
                UnReciboDebitoCreditoGenerica.PNota = Grid.CurrentRow.Cells("Comprobante").Value
                UnReciboDebitoCreditoGenerica.PTipoNota = 7007
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.ShowDialog()
                UnReciboDebitoCreditoGenerica.Dispose()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
            End If
        End If

    End Sub


End Class
