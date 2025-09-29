Public Class UnSaldosCuentas
    Public PPaseDeProyectos As ItemPaseDeProyectos
    'Para Cuando se pide un saldo desde otro programa.--------
    Public PBanco As Integer
    Public PCuenta As Double
    Public PSaldo As Double
    ' ------------------------------------------------------
    Dim DtFormasPago As DataTable
    Dim DtGrid As DataTable
    Dim DtCuentas As DataTable
    Dim DtConceptosGastos As DataTable
    Dim DtMonedas As DataTable
    Private Sub UnSaldosCuentas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            PermisoTotal = PPaseDeProyectos.PermisoTotal
            Conexion = PPaseDeProyectos.Conexion
            ConexionN = PPaseDeProyectos.ConexionN
        End If
        '----------------------------------------------------------------------------------

        Grid.AutoGenerateColumns = False

        LlenaCombosGrid()

        ArmaDtCuentas()
        ArmaDtMonedas()

        DtConceptosGastos = ArmaConceptosParaGastosBancarios(1, True)

        LLenaGrid()

        Grid.Sort(Grid.Columns("Banco"), System.ComponentModel.ListSortDirection.Ascending)

        If PBanco <> 0 Then
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = DtGrid.Select("Banco = " & PBanco & " AND Cuenta = " & PCuenta)
            PSaldo = RowsBusqueda(0).Item("Saldo")
            Me.Close()
        End If

    End Sub
    Private Sub LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Dt As New DataTable

        CreaDtGrid()

        Dim SqlB As String = "SELECT 0 AS BancoDestino,0 AS CuentaDestino,P.MedioPago, P.Importe,P.Banco,P.Cuenta,P.TipoNota,C.Cambio FROM RecibosDetallePago AS P INNER JOIN RecibosCabeza AS C " & _
                                     "ON P.Nota = C.Nota AND P.TipoNota = C.TipoNota WHERE C.Estado <> 3 AND (P.TipoNota = 60 OR P.TipoNota = 600 OR P.TipoNota = 604 OR P.TipoNota = 64) AND P.MedioPago <> 2 AND P.MedioPago <> 3 AND P.Cuenta <> 0 AND P.MedioPago <> 14" & _
                                         " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,P.MedioPago, P.Importe,P.Banco,P.Cuenta,P.TipoNota,C.Cambio FROM RecibosExteriorDetallePago AS P INNER JOIN RecibosExteriorCabeza AS C " & _
                                     "ON P.Nota = C.Nota AND P.TipoNota = C.TipoNota WHERE C.Estado <> 3 AND P.TipoNota = 13 AND P.MedioPago <> 2 AND P.MedioPago <> 3 AND P.MedioPago <> 14 AND P.Cuenta <> 0" & _
                                   " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,CH.MedioPago,CH.Importe,CH.Banco,CH.Cuenta,CH.TipoDestino AS TipoNota,1 AS Cambio FROM Cheques AS CH " & _
                                     "WHERE CH.Estado = 1 AND (CH.MedioPago = 2 OR CH.MedioPago = 14) AND Year(CH.FechaDeposito) <> 1800" & _
                                        " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,D.MedioPago, D.Importe,D.Banco,D.Cuenta,C.TipoMovimiento AS TipoNota,D.Cambio FROM CompraDivisasPago AS D INNER JOIN CompraDivisasCabeza AS C " & _
                                     "ON D.Movimiento = C.Movimiento WHERE C.Estado <> 3 AND D.MedioPago <> 3 AND D.MedioPago <> 2 AND D.Cuenta <> 0" & _
                                        " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,D.MedioPago, D.Importe,D.Banco,D.Cuenta,1000 AS TipoNota,1 AS Cambio FROM PrestamosDetalle AS D INNER JOIN PrestamosCabeza AS C " & _
                                     "ON D.Prestamo = C.Prestamo WHERE C.Estado <> 3 AND D.MedioPago <> 3 AND D.Cuenta <> 0" & _
                                        " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,P.MedioPago, P.Importe,P.Banco,P.Cuenta,TipoNota,1 AS Cambio FROM PrestamosMovimientoCabeza AS MC INNER JOIN PrestamosMovimientoPago AS P " & _
                                     "ON P.Movimiento = MC.Movimiento WHERE MC.Estado <> 3 AND P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.Cuenta <> 0" & _
                                        " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,P.MedioPago, P.Importe,P.Banco,P.Cuenta,4010 AS TipoNota,1 AS Cambio FROM SueldosMovimientoCabeza AS C INNER JOIN SueldosMovimientoPago AS P " & _
                                     "ON P.Movimiento = C.Movimiento WHERE C.Estado <> 3 AND P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.MedioPago <> 14 AND P.Cuenta <> 0" & _
                                        " UNION ALL " & _
                             "SELECT BancoDestino,CuentaDestino,MedioPago,Importe,Banco,Cuenta,TipoNota,1 AS Cambio FROM MovimientosBancarioCabeza WHERE Estado = 1 AND MedioPago <> 2" & _
                                        " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,D.Concepto AS MedioPago,D.Importe,C.Banco,C.Cuenta,3000 as TipoNota,1 AS Cambio FROM GastosBancarioCabeza AS C INNER JOIN GastosBancarioDetalle AS D ON C.Movimiento = D.Movimiento WHERE Estado = 1" & _
                                        " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,P.MedioPago, P.Importe,P.Banco,P.Cuenta,TipoNota,1 AS Cambio FROM OtrosPagosCabeza AS C INNER JOIN OtrosPagosPago AS P " & _
                                     "ON P.Movimiento = C.Movimiento WHERE C.Estado <> 3 AND P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.MedioPago <> 14 AND P.Cuenta <> 0" & _
                                        " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,D.MedioPago, D.Importe,D.Banco,D.Cuenta,C.Tipo AS TipoNota,1 AS Cambio FROM MovimientosFondoFijoPago AS D INNER JOIN MovimientosFondoFijoCabeza AS C " & _
                                     "ON D.Movimiento = C.Movimiento WHERE C.Estado <> 3 AND D.MedioPago <> 3 AND D.MedioPago <> 2 AND D.Cuenta <> 0" & _
                                        " UNION ALL " & _
                             "SELECT Banco AS BancoDestino,CuentaDeposito AS CuentaDestino,0 AS MedioPago, Importe,Banco,CuentaRetiro AS Cuenta,-100 AS TipoNota,Cambio FROM LiquidacionDivisasCabeza WHERE Estado <> 3;"




        Dim SqlN As String = "SELECT 0 AS BancoDestino,0 AS CuentaDestino,P.MedioPago, P.Importe,P.Banco,P.Cuenta,P.TipoNota,C.Cambio FROM RecibosDetallePago AS P INNER JOIN RecibosCabeza AS C " & _
                                     "ON P.Nota = C.Nota AND P.TipoNota = C.TipoNota WHERE C.Estado <> 3 AND (P.TipoNota = 60 OR P.TipoNota = 600 OR P.TipoNota = 604 OR P.TipoNota = 64) AND P.MedioPago <> 2 AND P.MedioPago <> 3 AND P.MedioPago <> 14 AND P.Cuenta <> 0" & _
                                         " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,P.MedioPago, P.Importe,P.Banco,P.Cuenta,P.TipoNota,C.Cambio FROM RecibosExteriorDetallePago AS P INNER JOIN RecibosExteriorCabeza AS C " & _
                                     "ON P.Nota = C.Nota AND P.TipoNota = C.TipoNota WHERE C.Estado <> 3 AND (P.TipoNota = 11 OR P.TipoNota = 12) AND P.MedioPago <> 2 AND P.MedioPago <> 3 AND P.MedioPago <> 14 AND P.Cuenta <> 0" & _
                                         " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,CH.MedioPago,CH.Importe,CH.Banco,CH.Cuenta,CH.TipoDestino AS TipoNota,1 AS Cambio FROM Cheques AS CH " & _
                                     "WHERE CH.Estado = 1 AND (CH.MedioPago = 2 OR CH.MedioPago = 14) AND Year(CH.FechaDeposito) <> 1800" & _
                                       " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,D.MedioPago, D.Importe,D.Banco,D.Cuenta,C.TipoMovimiento AS TipoNota,C.Cambio FROM CompraDivisasPago AS D INNER JOIN CompraDivisasCabeza AS C " & _
                                     "ON D.Movimiento = C.Movimiento WHERE C.Estado <> 3 AND D.MedioPago <> 3 AND D.MedioPago <> 2 AND D.Cuenta <> 0" & _
                                        " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,D.MedioPago, D.Importe,D.Banco,D.Cuenta,1000 AS TipoNota,1 AS Cambio FROM PrestamosDetalle AS D INNER JOIN PrestamosCabeza AS C " & _
                                     "ON D.Prestamo = C.Prestamo WHERE C.Estado <> 3  AND D.MedioPago <> 3 AND D.Cuenta <> 0" & _
                                        " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,P.MedioPago, P.Importe,P.Banco,P.Cuenta,TipoNota,1 AS Cambio FROM PrestamosMovimientoCabeza AS MC INNER JOIN PrestamosMovimientoPago AS P " & _
                                     "ON P.Movimiento = MC.Movimiento WHERE MC.Estado <> 3 AND P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.Cuenta <> 0" & _
                                        " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,P.MedioPago, P.Importe,P.Banco,P.Cuenta,4010 AS TipoNota,1 AS Cambio FROM SueldosMovimientoCabeza AS C INNER JOIN SueldosMovimientoPago AS P " & _
                                     "ON P.Movimiento = C.Movimiento WHERE C.Estado <> 3 AND P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.MedioPago <> 14 AND P.Cuenta <> 0" & _
                                        " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,D.Concepto AS MedioPago,D.Importe,C.Banco,C.Cuenta,3000 as TipoNota,1 AS Cambio FROM GastosBancarioCabeza AS C INNER JOIN GastosBancarioDetalle AS D ON C.Movimiento = D.Movimiento WHERE Estado = 1" & _
                                        " UNION ALL " & _
                             "SELECT BancoDestino,CuentaDestino,MedioPago,Importe,Banco,Cuenta,TipoNota,1 AS Cambio FROM MovimientosBancarioCabeza WHERE Estado = 1 AND MedioPago <> 2" & _
                                        " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,P.MedioPago, P.Importe,P.Banco,P.Cuenta, TipoNota,1 AS Cambio FROM OtrosPagosCabeza AS C INNER JOIN OtrosPagosPago AS P " & _
                                     "ON P.Movimiento = C.Movimiento WHERE C.Estado <> 3 AND P.MedioPago <> 3 AND P.MedioPago <> 2 AND P.MedioPago <> 14 AND P.Cuenta <> 0" & _
                                        " UNION ALL " & _
                             "SELECT 0 AS BancoDestino,0 AS CuentaDestino,D.MedioPago, D.Importe,D.Banco,D.Cuenta,C.Tipo AS TipoNota,1 AS Cambio FROM MovimientosFondoFijoPago AS D INNER JOIN MovimientosFondoFijoCabeza AS C " & _
                                     "ON D.Movimiento = C.Movimiento WHERE C.Estado <> 3 AND D.MedioPago <> 3 AND D.MedioPago <> 2 AND D.Cuenta <> 0" & ";"

        '
        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If
        '
        'genara registro con saldos iniciales de cuentas.
        For Each Row As DataRow In DtCuentas.Rows
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Banco") = Row("Banco")
            Row1("Cuenta") = Row("Numero")
            Row1("Moneda") = Row("Moneda")
            Row1("Debito") = 0
            Row1("Credito") = 0
            If Row("SaldoInicial") < 0 Then
                Row1("Credito") = -Row("SaldoInicial")
            Else : Row1("Debito") = Row("SaldoInicial")
            End If
            Row1("Saldo") = Row1("Debito") - Row1("Credito")
            DtGrid.Rows.Add(Row1)
        Next

        For Each Row As DataRow In Dt.Rows
            If Row("TipoNota") = 600 Or Row("TipoNota") = 90 Or Row("TipoNota") = 93 Or Row("TipoNota") = 1010 Or Row("TipoNota") = 4010 Or Row("TipoNota") = 5010 Or Row("TipoNota") = 7000 Or Row("TipoNota") = 7001 Or Row("TipoNota") = 64 Then AcumulaEnCuenta(Row("TipoNota"), Row("Banco"), Row("Cuenta"), 0, Row("Importe"), Row("Cambio"))
            If Row("TipoNota") = 60 Or Row("TipoNota") = 91 Or Row("TipoNota") = 1000 Or Row("TipoNota") = 1015 Or Row("TipoNota") = 7002 Or Row("TipoNota") = 604 Or Row("TipoNota") = 5020 Then AcumulaEnCuenta(Row("TipoNota"), Row("Banco"), Row("Cuenta"), Row("Importe"), 0, Row("Cambio"))
            If Row("TipoNota") = 11 Or Row("TipoNota") = 12 Or Row("TipoNota") = 13 Then AcumulaEnCuenta(Row("TipoNota"), Row("Banco"), Row("Cuenta"), Row("Importe"), 0, 1)
            If Row("TipoNota") = 3000 Then
                If Operador(Row("MedioPago")) = 2 Then
                    AcumulaEnCuenta(Row("TipoNota"), Row("Banco"), Row("Cuenta"), 0, Row("Importe"), Row("Cambio"))
                Else
                    AcumulaEnCuenta(Row("TipoNota"), Row("Banco"), Row("Cuenta"), Row("Importe"), 0, Row("Cambio"))
                End If
            End If
            If Row("TipoNota") = -100 Then
                AcumulaEnCuenta(Row("TipoNota"), Row("Banco"), Row("CuentaDestino"), Row("Importe"), 0, Row("Cambio"))
                AcumulaEnCuenta(Row("TipoNota"), Row("Banco"), Row("Cuenta"), 0, Row("Importe"), 1)
            End If
            If Row("TipoNota") = 92 Then
                AcumulaEnCuenta(Row("TipoNota"), Row("Banco"), Row("Cuenta"), 0, Row("Importe"), 1)
                AcumulaEnCuenta(Row("TipoNota"), Row("BancoDestino"), Row("CuentaDestino"), Row("Importe"), 0, 1)
            End If
            If Row("TipoNota") = 6000 And HallaMonedaDeLaCuenta(Row("Banco"), Row("Cuenta"), DtCuentas) <> 1 Then AcumulaEnCuenta(Row("TipoNota"), Row("Banco"), Row("Cuenta"), Row("Importe"), 0, 1) 'en cambio pongo 1 para que no lo combierta en $.
            If Row("TipoNota") = 6000 And HallaMonedaDeLaCuenta(Row("Banco"), Row("Cuenta"), DtCuentas) = 1 Then AcumulaEnCuenta(Row("TipoNota"), Row("Banco"), Row("Cuenta"), 0, Row("Importe"), 1) 'en cambio pongo 1 para que no lo combierta en $.
            If Row("TipoNota") = 6001 And HallaMonedaDeLaCuenta(Row("Banco"), Row("Cuenta"), DtCuentas) <> 1 Then AcumulaEnCuenta(Row("TipoNota"), Row("Banco"), Row("Cuenta"), 0, Row("Importe"), 1) 'en cambio pongo 1 para que no lo combierta en $.
            If Row("TipoNota") = 6001 And HallaMonedaDeLaCuenta(Row("Banco"), Row("Cuenta"), DtCuentas) = 1 Then AcumulaEnCuenta(Row("TipoNota"), Row("Banco"), Row("Cuenta"), Row("Importe"), 0, 1) 'en cambio pongo 1 para que no lo combierta en $.
        Next

        If Not PermisoTotal Then
            Dim RowsBusqueda() As DataRow
            Dim DtBancosNegro As New DataTable
            If Not Tablas.Read("SELECT Clave FROM Tablas WHERE Tipo = 26 AND Activo2 = 1;", Conexion, DtBancosNegro) Then
                Me.Close()
                Exit Sub
            End If
            For I As Integer = DtGrid.Rows.Count - 1 To 0 Step -1
                Dim Row1 As DataRow = DtGrid.Rows(I)
                RowsBusqueda = DtBancosNegro.Select("Clave = " & Row1("Banco"))
                If RowsBusqueda.Length <> 0 Then Row1.Delete()
            Next
            DtBancosNegro.Dispose()
        End If

        Grid.DataSource = DtGrid

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function AcumulaEnCuenta(ByVal TipoNota As Integer, ByVal Banco As Integer, ByVal Cuenta As Double, ByVal Debito As Double, ByVal Credito As Double, ByVal Cambio As Double) As Boolean

        Dim Moneda As Integer = 0
        Dim SaldoInicial As Double = 0
        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtCuentas.Select("Banco = " & Banco & " AND Numero = " & Cuenta)
        If RowsBusqueda.Length = 0 Then
            MsgBox("Cuenta " & Cuenta & " en Banco " & Banco & " No Encontrada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Moneda = RowsBusqueda(0).Item("Moneda")
        SaldoInicial = RowsBusqueda(0).Item("SaldoInicial")

        If TipoNota = 60 Or TipoNota = 600 Then
            If Moneda = 1 Then
                Credito = Trunca(Cambio * Credito)
                Debito = Trunca(Cambio * Debito)
            End If
        End If

        If TipoNota = -100 Then
            Credito = Trunca(Cambio * Credito)
            Debito = Trunca(Cambio * Debito)
        End If

        '        Credito = Trunca(Cambio * Credito)
        '        Debito = Trunca(Cambio * Debito)


        RowsBusqueda = DtGrid.Select("Banco = " & Banco & "AND Cuenta = " & Cuenta)
        If RowsBusqueda.Length <> 0 Then
            RowsBusqueda(0).Item("Debito") = RowsBusqueda(0).Item("Debito") + Debito
            RowsBusqueda(0).Item("Credito") = RowsBusqueda(0).Item("Credito") + Credito
            RowsBusqueda(0).Item("Saldo") = RowsBusqueda(0).Item("Debito") - RowsBusqueda(0).Item("Credito")
        End If

        Return True

    End Function
    Private Sub LlenaCombosGrid()

        Banco.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 order By Nombre;")
        Dim Row As DataRow = Banco.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Banco.DataSource.Rows.Add(Row)
        Banco.DisplayMember = "Nombre"
        Banco.ValueMember = "Clave"

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
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Banco As New DataColumn("Banco")
        Banco.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Banco)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cuenta)

        Dim Moneda As New DataColumn("Moneda")
        Moneda.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Moneda)

        Dim Debito As New DataColumn("Debito")
        Debito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Debito)

        Dim Credito As New DataColumn("Credito")
        Credito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Credito)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Saldo)

    End Sub
    Private Sub ArmaDtCuentas()

        DtCuentas = New DataTable

        If Not Tablas.Read("SELECT Banco,Numero,Moneda,SaldoInicial,0.0 AS Debito,0.0 AS Credito FROM CuentasBancarias;", Conexion, DtCuentas) Then End

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
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Debito" Or Grid.Columns(e.ColumnIndex).Name = "Credito" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            If IsDBNull(e.Value) Then e.Value = 0
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cuenta" Then
            If Not IsDBNull(e.Value) Then
                e.Value = FormatNumber(e.Value, 0)
            End If
        End If

    End Sub


End Class