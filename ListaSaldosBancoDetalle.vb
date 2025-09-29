Public Class ListaSaldosBancoDetalle
    Private WithEvents bs As New BindingSource
    ' 
    Dim DtFormasPago As DataTable
    Dim DtConceptosGastos As DataTable
    Dim DtGrid As DataTable
    Dim DtCuentas As DataTable
    Dim DtMonedas As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Dim SaldoInicial As Decimal
    Private Sub UnSaldosCuentasDetalle_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        LlenaComboTablas(ComboBancos, 26)

        ListaBancos.PEsSeleccionaCuenta = True
        ListaBancos.ShowDialog()
        ComboBancos.SelectedValue = ListaBancos.PBanco
        TextCuenta.Text = FormatNumber(ListaBancos.PCuenta, 0)
        SaldoInicial = ListaBancos.PSaldoInicial
        ListaBancos.Dispose()

        If ComboBancos.SelectedValue = 0 Then Me.Close() : Exit Sub

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ArmaMedioPagoTodosYGastos(DtFormasPago, True)
        DtConceptosGastos = ArmaConceptosParaGastosBancarios(1, True)

        ComboConcepto.DataSource = DtFormasPago
        ComboConcepto.DisplayMember = "Nombre"
        ComboConcepto.ValueMember = "Clave"
        ComboConcepto.SelectedValue = 0
        With ComboConcepto
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        ArmaDtCuentas()
        ArmaDtMonedas()

        CreaDtGrid()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ComboConcepto_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboConcepto.Validating

        If IsNothing(ComboConcepto.SelectedValue) Then ComboConcepto.SelectedValue = 0

    End Sub
    Private Sub UnSaldosCuentasDetalle_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, "Saldos Bancarios", "", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Nota AS Recibo,C.Fecha,D.MedioPago,D.Importe,D.Banco,D.Cuenta,D.TipoNota,C.Estado,C.Cambio,C.Emisor FROM RecibosDetallePago AS D INNER JOIN RecibosCabeza AS C " & _
                             "ON D.Nota = C.Nota AND D.TipoNota = C.TipoNota WHERE (D.TipoNota = 60 OR D.TipoNota = 600 OR D.TipoNota = 64 OR D.TipoNota = 604) AND D.MedioPago <> 2 AND D.MedioPago <> 3 AND D.MedioPago <> 14 AND D.Cuenta <> 0" & _
              " UNION ALL " & _
       "SELECT 1 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Nota AS Recibo,C.Fecha,D.MedioPago,D.Importe,D.Banco,D.Cuenta,D.TipoNota,C.Estado,C.Cambio,C.Emisor FROM RecibosExteriorDetallePago AS D INNER JOIN RecibosExteriorCabeza AS C " & _
                             "ON D.Nota = C.Nota AND D.TipoNota = C.TipoNota WHERE D.TipoNota = 13 AND D.MedioPago <> 2 AND D.MedioPago <> 3 AND D.Mediopago <> 14 AND D.Cuenta <> 0" & _
             " UNION ALL " & _
       "SELECT 1 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,CH.CompDestino AS Recibo,CH.FechaDeposito AS Fecha,CH.MedioPago,CH.Importe,CH.Banco,CH.Cuenta,CH.TipoDestino AS TipoNota,CH.Estado,1 AS Cambio,0 As Emisor FROM Cheques AS CH " & _
                             "WHERE (CH.MedioPago = 2 OR CH.MedioPago = 14) AND Year(CH.FechaDeposito) <> 1800" & _
              " UNION ALL " & _
       "SELECT 1 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Movimiento AS Recibo,C.FechaContable AS Fecha,P.MedioPago,P.Importe,P.Banco,P.Cuenta,C.TipoMovimiento AS TipoNota,C.Estado,1 AS Cambio,C.Emisor FROM CompraDivisasPago AS P INNER JOIN CompraDivisasCabeza AS C ON C.Movimiento = P.Movimiento " & _
                             "WHERE P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.Cuenta <> 0" & _
              " UNION ALL " & _
       "SELECT 1 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Movimiento AS Recibo,C.Fecha,P.MedioPago,P.Importe,P.Banco,P.Cuenta,TipoNota,C.Estado,1 AS Cambio,C.Prestamo AS Emisor FROM PrestamosMovimientoPago AS P INNER JOIN PrestamosMovimientoCabeza AS C ON C.Movimiento = P.Movimiento " & _
                             "WHERE P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.Cuenta <> 0" & _
              " UNION ALL " & _
       "SELECT 1 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,D.Prestamo AS Recibo,C.Fecha,D.MedioPago, D.Importe,D.Banco,D.Cuenta,1000 AS TipoNota,C.Estado,1 AS Cambio,C.Emisor FROM PrestamosDetalle AS D INNER JOIN PrestamosCabeza AS C " & _
                             "ON D.Prestamo = C.Prestamo WHERE D.MedioPago <> 3 AND D.Cuenta <> 0" & _
              " UNION ALL " & _
       "SELECT 1 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Movimiento AS Recibo,C.Fecha,P.MedioPago, P.Importe,P.Banco,P.Cuenta,4010 AS TipoNota,C.Estado,1 AS Cambio,C.Legajo As Emisor FROM SueldosMovimientoCabeza AS C INNER JOIN SueldosMovimientoPago AS P " & _
                             "ON P.Movimiento = C.Movimiento WHERE P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.MedioPago <> 14 AND P.Cuenta <> 0" & _
              " UNION ALL " & _
       "SELECT 1 AS Operacion,BancoDestino,CuentaDestino,Movimiento AS Recibo,FechaComprobante AS Fecha,MedioPago,Importe,Banco,Cuenta,TipoNota,Estado,1 AS Cambio,0 As Emisor FROM MovimientosBancarioCabeza WHERE MedioPago <> 2" & _
              " UNION ALL " & _
       "SELECT 1 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Movimiento AS Recibo,FechaContable AS Fecha,D.Concepto AS MedioPago,D.Importe,C.Banco,C.Cuenta,3000 AS TipoNota,C.Estado,1 AS Cambio,0 As Emisor FROM GastosBancarioCabeza AS C INNER JOIN GastosBancarioDetalle AS D ON C.Movimiento = D.Movimiento" & _
              " UNION ALL " & _
       "SELECT 1 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Movimiento AS Recibo,C.Fecha,P.MedioPago, P.Importe,P.Banco,P.Cuenta,TipoNota,C.Estado,1 AS Cambio,C.Proveedor AS Emisor FROM OtrosPagosCabeza AS C INNER JOIN OtrosPagosPago AS P " & _
                             "ON P.Movimiento = C.Movimiento WHERE P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.MedioPago <> 14 AND P.Cuenta <> 0" & _
              " UNION ALL " & _
       "SELECT 1 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,D.Movimiento AS Recibo,C.Fecha,D.MedioPago, D.Importe,D.Banco,D.Cuenta,C.Tipo AS TipoNota,C.Estado,1 AS Cambio,C.FondoFijo As Emisor FROM MovimientosFondoFijoPago AS D INNER JOIN MovimientosFondoFijoCabeza AS C " & _
                             "ON D.Movimiento = C.Movimiento WHERE D.MedioPago <> 3 AND D.Mediopago <> 2 AND D.Cuenta <> 0" & _
              " UNION ALL " & _
       "SELECT 1 AS Operacion,Banco AS BancoDestino,CuentaDeposito AS CuentaDestino,Nota AS Recibo,Fecha,0 AS MedioPago,Importe,Banco,CuentaRetiro AS Cuenta,-100 AS TipoNota,Estado,Cambio,Emisor FROM LiquidacionDivisasCabeza;"




        SqlN = "SELECT 2 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Nota AS Recibo,C.Fecha,D.MedioPago,D.Importe,D.Banco,D.Cuenta,D.TipoNota,C.Estado,C.Cambio,C.Emisor FROM RecibosDetallePago AS D INNER JOIN RecibosCabeza AS C " & _
                             "ON D.Nota = C.Nota AND D.TipoNota = C.TipoNota WHERE (D.TipoNota = 60 OR D.TipoNota = 600 OR D.TipoNota = 64 OR D.TipoNota = 604) AND D.MedioPago <> 2 AND D.MedioPago <> 3 AND D.Mediopago <> 14 AND D.Cuenta <> 0" & _
              " UNION ALL " & _
       "SELECT 2 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Nota AS Recibo,C.Fecha,D.MedioPago,D.Importe,D.Banco,D.Cuenta,D.TipoNota,C.Estado,C.Cambio,C.Emisor FROM RecibosExteriorDetallePago AS D INNER JOIN RecibosExteriorCabeza AS C " & _
                             "ON D.Nota = C.Nota AND D.TipoNota = C.TipoNota WHERE (D.TipoNota = 12 OR D.TipoNota = 11) AND D.MedioPago <> 2 AND D.MedioPago <> 3 AND D.Mediopago <> 14 AND D.Cuenta <> 0" & _
              " UNION ALL " & _
       "SELECT 2 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,CH.CompDestino AS Recibo,CH.FechaDeposito AS Fecha,CH.MedioPago,CH.Importe,CH.Banco,CH.Cuenta,CH.TipoDestino AS TipoNota,CH.Estado,1 AS Cambio,0 AS Emisor FROM Cheques AS CH " & _
                             "WHERE (CH.MedioPago = 2 OR CH.MedioPago = 14) AND Year(CH.FechaDeposito) <> 1800" & _
              " UNION ALL " & _
       "SELECT 2 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Movimiento AS Recibo,C.FechaContable AS Fecha,P.MedioPago,P.Importe,P.Banco,P.Cuenta,C.TipoMovimiento AS TipoNota,C.Estado,1 AS Cambio,C.Emisor FROM CompraDivisasPago AS P INNER JOIN CompraDivisasCabeza AS C ON C.Movimiento = P.Movimiento " & _
                             "WHERE P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.Cuenta <> 0" & _
          " UNION ALL " & _
       "SELECT 2 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Movimiento AS Recibo,C.Fecha,P.MedioPago,P.Importe,P.Banco,P.Cuenta,TipoNota,C.Estado,1 AS Cambio,Prestamo As Emisor FROM PrestamosMovimientoPago AS P INNER JOIN PrestamosMovimientoCabeza AS C ON C.Movimiento = P.Movimiento " & _
                             "WHERE P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.Cuenta <> 0" & _
              " UNION ALL " & _
       "SELECT 2 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,D.Prestamo AS Recibo,C.Fecha,D.MedioPago, D.Importe,D.Banco,D.Cuenta,1000 AS TipoNota,C.Estado,1 AS Cambio,C.Emisor FROM PrestamosDetalle AS D INNER JOIN PrestamosCabeza AS C " & _
                             "ON D.Prestamo = C.Prestamo WHERE D.MedioPago <> 3 AND D.Cuenta <> 0" & _
              " UNION ALL " & _
       "SELECT 2 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Movimiento AS Recibo,C.Fecha,P.MedioPago, P.Importe,P.Banco,P.Cuenta,4010 AS TipoNota,C.Estado,1 AS Cambio,C.Legajo As Emisor FROM SueldosMovimientoCabeza AS C INNER JOIN SueldosMovimientoPago AS P " & _
                             "ON P.Movimiento = C.Movimiento WHERE P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.MedioPago <> 14 AND P.Cuenta <> 0" & _
              " UNION ALL " & _
       "SELECT 2 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Movimiento AS Recibo,FechaContable AS Fecha,D.Concepto AS MedioPago,D.Importe,C.Banco,C.Cuenta,3000 AS TipoNota,C.Estado,1 AS Cambio,0 As Emisor FROM GastosBancarioCabeza AS C INNER JOIN GastosBancarioDetalle AS D ON C.Movimiento = D.Movimiento" & _
              " UNION ALL " & _
       "SELECT 2 AS Operacion,BancoDestino,CuentaDestino,Movimiento AS Recibo,FechaComprobante AS Fecha,MedioPago,Importe,Banco,Cuenta,TipoNota,Estado,1 AS Cambio,0 AS Emisor FROM MovimientosBancarioCabeza WHERE MedioPago <> 2" & _
              " UNION ALL " & _
       "SELECT 2 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,C.Movimiento AS Recibo,C.Fecha,P.MedioPago, P.Importe,P.Banco,P.Cuenta,TipoNota,C.Estado,1 AS Cambio,C.Proveedor As Emisor FROM OtrosPagosCabeza AS C INNER JOIN OtrosPagosPago AS P " & _
                             "ON P.Movimiento = C.Movimiento WHERE P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.Mediopago <> 14 AND P.Cuenta <> 0" & _
              " UNION ALL " & _
       "SELECT 2 AS Operacion,0 AS BancoDestino,0 AS CuentaDestino,D.Movimiento AS Recibo,C.Fecha,D.MedioPago, D.Importe,D.Banco,D.Cuenta,C.Tipo AS TipoNota,C.Estado,1 AS Cambio,C.FondoFijo As Emisor FROM MovimientosFondoFijoPago AS D INNER JOIN MovimientosFondoFijoCabeza AS C " & _
                             "ON D.Movimiento = C.Movimiento WHERE D.MedioPago <> 3 AND D.Mediopago <> 2 AND D.Cuenta <> 0;"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

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
    Private Sub LLenaGrid()

        DtGrid.Clear()

        Dim Dt As New DataTable

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Fecha"

        Dim Deb As Decimal
        Dim Cred As Decimal
        Dim Saldo As Decimal = SaldoInicial
        Dim MostroSaldoInicial As Boolean
        Dim TotalDebito As Decimal = 0
        Dim TotalCredito As Decimal = 0

        If SaldoInicial < 0 Then
            TotalCredito = -SaldoInicial
        Else : TotalDebito = SaldoInicial
        End If

        For Each Row As DataRowView In View
            If DiferenciaDias(Row("Fecha"), DateTimeHasta.Value) < 0 Then Exit For
            If Row("TipoNota") = 3000 And Row("MedioPago") = 0 Then Continue For
            If (Row("Banco") = ComboBancos.SelectedValue And CDbl(TextCuenta.Text) = Row("Cuenta")) Or (Row("BancoDestino") = ComboBancos.SelectedValue And CDbl(TextCuenta.Text) = Row("CuentaDestino")) Then
                HallaTipoImporte(Row("Banco"), Row("Cuenta"), Row("TipoNota"), Deb, Cred, Row("Importe"), Row("CuentaDestino"), Row("Cambio"), Row("MedioPago"))
                If Row("Estado") = 1 Then TotalDebito = Trunca(TotalDebito + Deb) : TotalCredito = Trunca(TotalCredito + Cred)
                If DiferenciaDias(Row("Fecha"), DateTimeDesde.Value) > 0 Then
                    If Row("Estado") = 1 Then Saldo = Saldo + Deb - Cred
                Else
                    If Not MostroSaldoInicial Then
                        LineaSaldoInicial(Saldo)
                        MostroSaldoInicial = True
                    End If
                    If ComboConcepto.SelectedValue = 0 Or ComboConcepto.SelectedValue = Row("MedioPago") Then
                        If Row("Estado") = 1 Then Saldo = Saldo + Deb - Cred
                        Lineadetalle(Row, Deb, Cred, Saldo)
                    End If
                End If
            End If
        Next
        If Not MostroSaldoInicial Then
            LineaSaldoInicial(Saldo)
        Else
            LineaTotales(TotalDebito, TotalCredito)
        End If

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub Lineadetalle(ByVal Row As DataRowView, ByVal DebW As Double, ByVal CredW As Double, ByVal SaldoW As Double)

        Dim RowGrid As DataRow = DtGrid.NewRow
        RowGrid("Operacion") = Row("Operacion")
        RowGrid("Fecha") = Row("Fecha")
        RowGrid("Recibo") = Row("Recibo")
        RowGrid("MedioPago") = Row("MedioPago")
        RowGrid("TipoNota") = Row("TipoNota")
        RowGrid("Debito") = DebW
        RowGrid("Credito") = CredW
        RowGrid("Saldo") = SaldoW
        RowGrid("Moneda") = HallaMoneda(Row("Banco"), Row("Cuenta"))
        If RowGrid("Moneda") = -1 Then End
        RowGrid("Estado") = 0
        If Row("Estado") = 3 Then RowGrid("Estado") = Row("Estado")
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Sub LineaSaldoInicial(ByVal SaldoW As Double)

        Dim RowGrid As DataRow = DtGrid.NewRow
        RowGrid("Mediopago") = 10000000
        RowGrid("Saldo") = SaldoW
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Sub LineaTotales(ByVal Debito As Double, ByVal Credito As Double)

        Dim RowGrid As DataRow = DtGrid.NewRow
        RowGrid("Debito") = Debito
        RowGrid("Credito") = Credito
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Sub HallaTipoImporte(ByVal Banco As Integer, ByVal Cuenta As Double, ByVal TipoNota As Integer, ByRef DebW As Decimal, ByRef CredW As Decimal, ByVal ImporteW As Decimal, ByVal CuentaDestino As Decimal, ByVal Cambio As Decimal, ByVal MedioPago As Integer)

        DebW = 0 : CredW = 0

        Select Case TipoNota  'Para compatibilizar los recibos anteriores al funcionamiento del modulo de exportacion.  
            Case 60, 600
                If HallaMonedaDeLaCuenta(Banco, Cuenta, DtCuentas) = 1 Then ImporteW = CalculaNeto(ImporteW, Cambio)
        End Select

        Select Case TipoNota
            Case 60, 91, 1000, 7002, 11, 12, 13, 604, 5020, 1015
                DebW = ImporteW
            Case 600, 90, 93, 1010, 4010, 5010, 7000, 7001, 64
                CredW = ImporteW
            Case 3000
                If Operador(MedioPago) = 2 Then
                    CredW = ImporteW
                Else
                    DebW = ImporteW
                End If
            Case 92
                If Banco = ComboBancos.SelectedValue And Cuenta = CDbl(TextCuenta.Text) Then
                    CredW = ImporteW
                Else
                    DebW = ImporteW
                End If
            Case 6000
                If HallaMonedaDeLaCuenta(Banco, Cuenta, DtCuentas) <> 1 Then DebW = ImporteW
                If HallaMonedaDeLaCuenta(Banco, Cuenta, DtCuentas) = 1 Then CredW = ImporteW
            Case 6001
                If HallaMonedaDeLaCuenta(Banco, Cuenta, DtCuentas) <> 1 Then CredW = ImporteW
                If HallaMonedaDeLaCuenta(Banco, Cuenta, DtCuentas) = 1 Then DebW = ImporteW
            Case -100
                If Cuenta = CDbl(TextCuenta.Text) Then
                    CredW = ImporteW
                End If
                If CuentaDestino = CDbl(TextCuenta.Text) Then
                    DebW = Trunca(ImporteW * Cambio)
                End If
            Case Else
        End Select

    End Sub
    Private Sub LlenaCombosGrid()

        Estado.DataSource = DtEstadoActivoYBaja()
        Dim Row As DataRow = Estado.DataSource.newrow
        Row("Nombre") = "Suspendido"
        Row("Clave") = 2
        Estado.DataSource.rows.add(Row)
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        Concepto.DataSource = DtFormasPago
        Row = Concepto.DataSource.newrow
        Row("Nombre") = "Saldo Anterior"
        Row("Clave") = 10000000
        Concepto.DataSource.rows.add(Row)
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

        TipoNota.DataSource = DtTiposComprobantes(True)
        Row = TipoNota.DataSource.newrow
        Row("Nombre") = "Liquidación Divisas"
        Row("Codigo") = -100
        TipoNota.DataSource.rows.add(Row)
        TipoNota.DisplayMember = "Nombre"
        TipoNota.ValueMember = "Codigo"

        Moneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Row = Moneda.DataSource.newrow
        Row("Nombre") = "Pesos"
        Row("Clave") = 1
        Moneda.DataSource.rows.add(Row)
        Row = Moneda.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        Moneda.DataSource.rows.add(Row)
        Moneda.DisplayMember = "Nombre"
        Moneda.ValueMember = "Clave"

    End Sub
    Private Function HallaMoneda(ByVal Banco As Integer, ByVal Cuenta As Double) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtCuentas.Select("Banco = " & Banco & " AND Numero = " & Cuenta)
        If RowsBusqueda.Length = 0 Then
            MsgBox("Cuenta " & Cuenta & " en Banco " & Banco & " No Encontrada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return -1
        Else
            Return RowsBusqueda(0).Item("Moneda")
        End If

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim MedioPago As New DataColumn("MedioPago")
        MedioPago.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(MedioPago)

        Dim Moneda As New DataColumn("Moneda")
        Moneda.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Moneda)

        Dim TipoNota As New DataColumn("TipoNota")
        TipoNota.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoNota)

        Dim Recibo As New DataColumn("Recibo")
        Recibo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Recibo)

        Dim Debito As New DataColumn("Debito")
        Debito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Debito)

        Dim Credito As New DataColumn("Credito")
        Credito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Credito)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Saldo)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

    End Sub
    Private Sub ArmaDtCuentas()

        DtCuentas = New DataTable

        If Not Tablas.Read("SELECT Banco,Numero,Moneda,0.0 AS Debito,0.0 AS Credito FROM CuentasBancarias;", Conexion, DtCuentas) Then End

    End Sub
    Private Sub ArmaDtMonedas()

        DtMonedas = New DataTable

        If Not Tablas.Read("SELECT Clave AS Moneda,0.0 AS Debito,0.0 AS Credito FROM Tablas WHERE Tipo = 12;", Conexion, DtMonedas) Then End
        Dim Row As DataRow = DtMonedas.NewRow
        Row("Moneda") = 1
        Row("Debito") = 0
        Row("Credito") = 0
        DtMonedas.Rows.Add(Row)

    End Sub
    Private Function Operador(ByVal Concepto As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtConceptosGastos.Select("Clave = " & Concepto)
        Return RowsBusqueda(0).Item("Operador")

    End Function
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Hasta debe ser mayor o igual a Fecha Desde.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Recibo" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else : e.Value = NumeroEditado(e.Value)
                End If
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                    If PermisoTotal Then
                        If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                        If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                    End If
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Debito" Or Grid.Columns(e.ColumnIndex).Name = "Credito" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            If Not IsDBNull(e.Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If IsDBNull(Grid.CurrentRow.Cells("Recibo").Value) Then Exit Sub

        If Grid.Rows(e.RowIndex).Cells("Recibo").Value = 0 Then
            MsgBox("Cheque Viene de Craga Inicial del Sistema", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Abierto As Boolean
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        Select Case Grid.Rows(e.RowIndex).Cells("TipoNota").Value
            Case 3000
                UnGastoBancario.PMovimiento = Grid.Rows(e.RowIndex).Cells("Recibo").Value
                UnGastoBancario.PAbierto = Abierto
                UnGastoBancario.PBloqueaFunciones = True
                UnGastoBancario.ShowDialog()
                UnGastoBancario.Dispose()
            Case 1010
                UnMovimientoPrestamo.PAbierto = Abierto
                UnMovimientoPrestamo.PMovimiento = Grid.Rows(e.RowIndex).Cells("Recibo").Value
                UnMovimientoPrestamo.PBloqueaFunciones = True
                UnMovimientoPrestamo.ShowDialog()
                UnMovimientoPrestamo.Dispose()
            Case 1000
                UnPrestamo.PAbierto = Abierto
                UnPrestamo.PBloqueaFunciones = True
                UnPrestamo.PPrestamo = Grid.Rows(e.RowIndex).Cells("Recibo").Value
                UnPrestamo.ShowDialog()
                UnPrestamo.Dispose()
            Case 90, 92, 91
                UnMovimientoBancario.PMovimiento = Grid.Rows(e.RowIndex).Cells("Recibo").Value
                UnMovimientoBancario.PBloqueaFunciones = True
                UnMovimientoBancario.PAbierto = Abierto
                UnMovimientoBancario.PTipoNota = Grid.CurrentRow.Cells("TipoNota").Value
                UnMovimientoBancario.ShowDialog()
                UnMovimientoBancario.Dispose()
            Case 93
                UnReciboDebitoCreditoGenerica.PNota = Grid.Rows(e.RowIndex).Cells("Recibo").Value
                UnReciboDebitoCreditoGenerica.PTipoNota = 93
                UnReciboDebitoCreditoGenerica.PBloqueaFunciones = True
                UnReciboDebitoCreditoGenerica.PAbierto = Abierto
                UnReciboDebitoCreditoGenerica.ShowDialog()
            Case 4010
                UnaOrdenPagoSueldos.PNota = Grid.CurrentRow.Cells("Recibo").Value
                UnaOrdenPagoSueldos.PAbierto = Abierto
                UnaOrdenPagoSueldos.PBloqueaFunciones = True
                UnaOrdenPagoSueldos.ShowDialog()
            Case 5010, 5020
                UnReciboOtrosProveedores.PNota = Grid.CurrentRow.Cells("Recibo").Value
                UnReciboOtrosProveedores.PAbierto = Abierto
                UnReciboOtrosProveedores.PBloqueaFunciones = True
                UnReciboOtrosProveedores.ShowDialog()
            Case 6000, 6001
                UnaCompraDivisas.PMovimiento = Grid.CurrentRow.Cells("Recibo").Value
                UnaCompraDivisas.PAbierto = Abierto
                UnaCompraDivisas.PBloqueaFunciones = True
                UnaCompraDivisas.ShowDialog()
            Case 7001, 7002
                UnMovimientoFondoFijo.PMovimiento = Grid.CurrentRow.Cells("Recibo").Value
                UnMovimientoFondoFijo.PAbierto = Abierto
                UnMovimientoFondoFijo.PBloqueaFunciones = True
                UnMovimientoFondoFijo.ShowDialog()
            Case -100
                MsgBox("Detalle en Modulo de Exportación.")
                Exit Sub
            Case Else
                If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 600 Then
                    If EsReposicion(Grid.Rows(e.RowIndex).Cells("Recibo").Value, Abierto) Then
                        UnReciboReposicion.PAbierto = Abierto
                        UnReciboReposicion.PTipoNota = Grid.Rows(e.RowIndex).Cells("TipoNota").Value
                        UnReciboReposicion.PNota = Grid.Rows(e.RowIndex).Cells("Recibo").Value
                        UnReciboReposicion.PBloqueaFunciones = True
                        UnReciboReposicion.ShowDialog()
                        Exit Sub
                    End If
                End If
                If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 11 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 12 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 13 Then
                    MsgBox("Detalle en Modulo de Exportación.", MsgBoxStyle.Information)
                Else
                    UnRecibo.PAbierto = Abierto
                    UnRecibo.PTipoNota = Grid.Rows(e.RowIndex).Cells("TipoNota").Value
                    UnRecibo.PNota = Grid.Rows(e.RowIndex).Cells("Recibo").Value
                    UnRecibo.PBloqueaFunciones = True
                    UnRecibo.ShowDialog()
                End If
        End Select

    End Sub
End Class