Public Class ListaSaldosSueldosImputaciones
    Public PAbierto As Boolean
    Public PCerrado As Boolean
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    '
    Dim NombreColor As Boolean
    Private Sub ListaSaldosSueldosImputaciones_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaCombosGrid()

        CreaDtGrid()

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
        End If

        CheckAbierto.Checked = PAbierto
        CheckCerrado.Checked = PCerrado

        PreparaArchivos()

    End Sub
    Private Sub ListaSaldosSueldosImputaciones_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub PreparaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Saldo As Double = 0
        Dim SaldoCtaCte As Double
        Dim SaldoTotal As Double
        Dim Row As DataRow

        For Each Row In UnaCtaCteSueldos.DtGrid.Rows
            If Row("Tipo") = 6000000 Then
                If DtGrid.Rows.Count <> 0 Then
                    SaldoTotal = SaldoTotal + SaldoCtaCte
                    AddGrid(Nothing, Nothing, Nothing, Row("Operacion"), Nothing, Nothing, Nothing, 7000000, Nothing, Nothing, Nothing, Nothing, Nothing, SaldoCtaCte, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
                End If
                AddGrid(Row("Legajo"), Nothing, Nothing, Row("Operacion"), Nothing, Nothing, Nothing, 6000000, Nothing, Nothing, Nothing, Nothing, Nothing, Row("SaldoCta"), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
                SaldoCtaCte = Row("SaldoCta")
            End If
            If Not ProcesaComprobanteC(Row("TipoOrigen"), Row("Tipo"), Row("Comprobante"), Row("Debito"), Row("Credito"), Row("Saldo"), Row("Fecha"), Row("Operacion"), Row("Estado"), SaldoCtaCte, Row("MediopagoRechazado"), Row("ChequeRechazado"), Row("Mes"), Row("Anio")) Then Me.Close() : Exit Sub
        Next
        If DtGrid.Rows.Count <> 0 Then
            SaldoTotal = SaldoTotal + SaldoCtaCte
            AddGrid(Nothing, Nothing, Nothing, Row("Operacion"), Nothing, Nothing, Nothing, 7000000, Nothing, Nothing, Nothing, Nothing, Nothing, SaldoCtaCte, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        End If

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        TextSaldoCtaCte.Text = FormatCurrency(SaldoTotal, GDecimales)
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
    Private Function ProcesaComprobanteC(ByVal TipoOrigen As Integer, ByVal TipoComprobante As Integer, ByVal Comprobante As Double, ByVal Debito As Double, ByVal Credito As Double, ByVal SaldoComprobante As Double, ByVal Fecha As Date, ByVal Operacion As Integer, ByVal Estado As Integer, ByRef SaldoCuenta As Double, ByVal MedioPagoRechazado As Integer, ByVal ChequeRechazado As Integer, ByVal Mes As Integer, ByVal Anio As Integer) As Boolean

        Dim Saldo As Double
        Dim ConexionStr As String

        NombreColor = False

        'Procesa Factura Cliente.
        If TipoOrigen = 4000 Then
            If Estado = 0 Then SaldoCuenta = SaldoCuenta + (Debito - Credito)
            Saldo = (Debito - Credito)
            If Saldo < 0 Then Saldo = -Saldo
            If Operacion = 1 Then
                ConexionStr = Conexion
            Else : ConexionStr = ConexionN
            End If
            Dim Sql As String = "SELECT C.Movimiento,C.Fecha,D.Importe FROM SueldosMovimientoCabeza AS C INNER JOIN SueldosMovimientoDetalle AS D ON C.Movimiento = D.Movimiento " & _
                      "WHERE C.Estado = 1 AND D.TipoComprobante = 4000 AND D.Comprobante = " & Comprobante & ";"
            Dim DtAux As New DataTable
            If Not Tablas.Read(Sql, ConexionStr, DtAux) Then Return False
            If DtAux.Rows.Count <> 0 Then NombreColor = True
            AddGrid(Nothing, NombreColor, True, Operacion, "", Fecha, TipoOrigen, TipoComprobante, Comprobante, Nothing, Nothing, Debito, Credito, SaldoCuenta, Saldo, Estado, MedioPagoRechazado, ChequeRechazado, Mes, Anio)
            For Each RowAux As DataRow In DtAux.Rows
                Saldo = Saldo - RowAux("Importe")
                SaldoCuenta = SaldoCuenta + RowAux("Importe")
                AddGrid(Nothing, NombreColor, True, Operacion, "", RowAux("Fecha"), Nothing, Nothing, Nothing, 4010, RowAux("Movimiento"), RowAux("Importe"), 0, SaldoCuenta, Saldo, 0, 0, 0, 0, 0)
            Next
            DtAux.Dispose()
        End If

        If TipoOrigen = 4010 Then
            If Estado = 0 Then SaldoCuenta = SaldoCuenta + SaldoComprobante
            Saldo = SaldoComprobante
            AddGrid(Nothing, NombreColor, True, Operacion, "", Fecha, TipoOrigen, TipoComprobante, Comprobante, Nothing, Nothing, SaldoComprobante, 0, SaldoCuenta, Saldo, Estado, MedioPagoRechazado, ChequeRechazado, 0, 0)
        End If

        If TipoOrigen = 4005 Then
            If Estado = 0 Then SaldoCuenta = SaldoCuenta + SaldoComprobante
            Saldo = SaldoComprobante
            AddGrid(Nothing, NombreColor, True, Operacion, "", Fecha, TipoOrigen, TipoComprobante, Comprobante, Nothing, Nothing, SaldoComprobante, 0, SaldoCuenta, Saldo, Estado, MedioPagoRechazado, ChequeRechazado, 0, 0)
        End If

        If TipoOrigen = 4007 Then
            If Estado = 0 Then SaldoCuenta = SaldoCuenta - SaldoComprobante
            Saldo = SaldoComprobante
            AddGrid(Nothing, NombreColor, True, Operacion, "", Fecha, TipoOrigen, TipoComprobante, Comprobante, Nothing, Nothing, 0, SaldoComprobante, SaldoCuenta, Saldo, Estado, MedioPagoRechazado, ChequeRechazado, 0, 0)
        End If

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim NombreColor As New DataColumn("NombreColor")
        NombreColor.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(NombreColor)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Legajo As New DataColumn("Legajo")
        Legajo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Legajo)

        Dim Mes As New DataColumn("Mes")
        Mes.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Mes)

        Dim Anio As New DataColumn("Anio")
        Anio.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Anio)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim TipoOrigen As New DataColumn("TipoOrigen")
        TipoOrigen.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoOrigen)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Tipo)

        Dim Mensaje As New DataColumn("Mensaje")
        Mensaje.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Mensaje)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante)

        Dim Tipo2 As New DataColumn("Tipo2")
        Tipo2.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Tipo2)

        Dim Comprobante2 As New DataColumn("Comprobante2")
        Comprobante2.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante2)

        Dim Debito As New DataColumn("Debito")
        Debito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Debito)

        Dim Credito As New DataColumn("Credito")
        Credito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Credito)

        Dim Imputado As New DataColumn("Imputado")
        Imputado.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Imputado)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Saldo)

        Dim SaldoFactura As New DataColumn("SaldoFactura")
        SaldoFactura.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(SaldoFactura)

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
        Row("Nombre") = "Saldo Anterior"
        Row("Clave") = 6000000
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Total periodo"
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

        Tipo2.DataSource = ArmaMovimientosSueldo()
        Tipo2.DisplayMember = "Nombre"
        Tipo2.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Legajo,RIGHT('0000' + CAST(Legajo AS varchar),4) + ' - ' + Apellidos + ' ' + Nombres As Con FROM Empleados;", Conexion, Dt) Then End
        If PermisoTotal Then
            If Not Tablas.Read("SELECT Legajo,RIGHT('0000' + CAST(Legajo AS varchar),4) + ' - ' + Apellidos + ' ' + Nombres As Con FROM Empleados;", ConexionN, Dt) Then End
        End If
        legajo.DataSource = Dt
        legajo.DisplayMember = "Con"
        legajo.ValueMember = "Legajo"

    End Sub
    Private Sub AddGrid(ByVal Legajo As Integer, ByVal NombreColor As Boolean, ByVal EsFactura As Boolean, ByVal Operacion As Integer, ByVal Mensaje As String, ByVal Fecha As DateTime, ByVal TipoOrigen As Integer, ByVal Tipo As Integer, ByVal Comprobante As Double, _
                      ByVal Tipo2 As Integer, ByVal Comprobante2 As Double, ByVal Debito As Double, ByVal Credito As Double, ByVal Saldo As Double, ByVal SaldoFactura As Double, ByVal Estado As Integer, ByVal MedioPagoRechazado As Integer, ByVal ChequeRechazado As Integer, ByVal Mes As Integer, ByVal Anio As Integer)

        Dim RowGrid As DataRow

        RowGrid = DtGrid.NewRow()
        If Legajo <> Nothing Then RowGrid("Legajo") = Legajo
        RowGrid("NombreColor") = NombreColor
        RowGrid("Operacion") = Operacion
        RowGrid("Mensaje") = Mensaje
        If Not Fecha = Nothing Then RowGrid("Fecha") = Format(Fecha, "dd/MM/yyyy 00:00:00")
        RowGrid("TipoOrigen") = TipoOrigen
        RowGrid("Tipo") = Tipo
        RowGrid("Mes") = Mes
        RowGrid("Anio") = Anio
        RowGrid("Comprobante") = Comprobante
        RowGrid("Tipo2") = Tipo2
        RowGrid("Comprobante2") = Comprobante2
        RowGrid("Debito") = Debito
        RowGrid("Credito") = Credito
        RowGrid("Saldo") = Saldo
        RowGrid("SaldoFactura") = SaldoFactura
        RowGrid("Estado") = Estado
        If Estado = 1 Then RowGrid("Estado") = 0
        RowGrid("MedioPagoRechazado") = MedioPagoRechazado
        RowGrid("ChequeRechazado") = ChequeRechazado
        DtGrid.Rows.Add(RowGrid)

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            '     If Grid.Rows(e.RowIndex).Cells("NombreColor1").Value Then
            '          Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.White
            '      Else : Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.PaleGoldenrod
            '      End If
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, 0)
            Else : e.Value = Format(e.Value, "#")
            End If
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante2" Or Grid.Columns(e.ColumnIndex).Name = "Mes" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, 0)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Anio" Then
            e.Value = Format(e.Value, "#")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Debito" Or Grid.Columns(e.ColumnIndex).Name = "Credito" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            If IsDBNull(e.Value) Then Exit Sub
            '      If e.Value > 0 Then Grid.Rows(e.RowIndex).Cells("Saldo").Style.ForeColor = Color.Red
            '     If e.Value <= 0 Then Grid.Rows(e.RowIndex).Cells("Saldo").Style.ForeColor = Color.Black
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "SaldoFactura" Then
            If IsDBNull(e.Value) Then Exit Sub
            '        If e.Value > 0 Then Grid.Rows(e.RowIndex).Cells("SaldoFactura").Style.ForeColor = Color.Red
            '       If e.Value <= 0 Then Grid.Rows(e.RowIndex).Cells("SaldoFactura").Style.ForeColor = Color.Black
            e.Value = FormatNumber(e.Value, GDecimales)
            If Grid.Rows(e.RowIndex).Cells("NombreColor1").Value Then
                Grid.Rows(e.RowIndex).Cells("Tipo2").Style.BackColor = Color.Yellow
                Grid.Rows(e.RowIndex).Cells("Comprobante2").Style.BackColor = Color.Yellow
                Grid.Rows(e.RowIndex).Cells("Debito").Style.BackColor = Color.Yellow
                Grid.Rows(e.RowIndex).Cells("Credito").Style.BackColor = Color.Yellow
                Grid.Rows(e.RowIndex).Cells("SaldoFactura").Style.BackColor = Color.Yellow
            End If
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

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            If Grid.CurrentRow.Cells("TipoOrigen").Value = 4000 Then
                UnReciboSueldo.PAbierto = Abierto
                UnReciboSueldo.PBloqueaFunciones = True
                UnReciboSueldo.PRecibo = Grid.CurrentCell.Value
                UnReciboSueldo.ShowDialog()
                UnReciboSueldo.Dispose()
            End If
            If Grid.CurrentRow.Cells("TipoOrigen").Value = 4010 Then
                UnaOrdenPagoSueldos.PAbierto = Abierto
                UnaOrdenPagoSueldos.PBloqueaFunciones = True
                UnaOrdenPagoSueldos.PNota = Grid.CurrentCell.Value
                UnaOrdenPagoSueldos.PTipoNota = 4010
                UnaOrdenPagoSueldos.ShowDialog()
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante2" Then
            If Grid.CurrentRow.Cells("Tipo2").Value = 4010 Then
                UnaOrdenPagoSueldos.PAbierto = Abierto
                UnaOrdenPagoSueldos.PBloqueaFunciones = True
                UnaOrdenPagoSueldos.PNota = Grid.CurrentCell.Value
                UnaOrdenPagoSueldos.PTipoNota = 4010
                UnaOrdenPagoSueldos.ShowDialog()
            End If
        End If

    End Sub
End Class