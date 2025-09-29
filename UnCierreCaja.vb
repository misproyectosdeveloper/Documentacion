Public Class UnCierreCaja
    Dim DtFormasPago As DataTable
    Dim DtGrid As DataTable
    Dim DtCuentas As DataTable
    Private Sub UnCierreCaja_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing
        Grid2.Columns("Candado2").DefaultCellStyle.NullValue = Nothing

        ArmaMedioPagoTodos(DtFormasPago, True)

        LlenaCombosGrid()

        ArmaDtCuentas()

        ComboCaja.DataSource = ArmaDtCajas()
        Dim Row As DataRow = ComboCaja.DataSource.Newrow
        Row("Nombre") = "Todas"
        Row("Clave") = 99999
        ComboCaja.DataSource.rows.add(Row)
        ComboCaja.DisplayMember = "Nombre"
        ComboCaja.ValueMember = "Clave"
        ComboCaja.SelectedValue = GCaja

        If Not GCajaTotal Then
            ComboCaja.Enabled = False
        End If

        LLenaGrid()

        If Not LlenaGridPasesPropios() Then Me.Close() : Exit Sub
        If Not LlenaGridPasesTerceros() Then Me.Close() : Exit Sub

        If Grid1.Rows.Count = 0 Then
            Label3.Visible = False
            Grid1.Visible = False
        Else : Label3.Visible = True
            Grid1.Visible = True
        End If

        If Grid2.Rows.Count = 0 Then
            Label2.Visible = False
            Grid2.Visible = False
        Else : Label2.Visible = True
            Grid2.Visible = True
        End If

    End Sub
    Private Sub ComboCaja_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboCaja.SelectionChangeCommitted

        LLenaGrid()

    End Sub
    Private Sub DateTime1_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTime1.ValueChanged

        LLenaGrid()

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPaseCaja.Click

        If ComboCaja.SelectedValue <> GCaja Then
            MsgBox("Usuario no puede hacer un pase de una caja no propia.")
            Exit Sub
        End If

        OpcionPaseCaja.ShowDialog()
        UnPaseCaja.PCajaDestino = OpcionPaseCaja.ComboCaja.SelectedValue
        UnPaseCaja.PAbierto = OpcionPaseCaja.PAbierto
        OpcionPaseCaja.Dispose()
        If UnPaseCaja.PCajaDestino = 0 Then Exit Sub

        UnPaseCaja.PCajaOrigen = GCaja
        UnPaseCaja.PPase = 0
        UnPaseCaja.PDtGrid = DtGrid
        UnPaseCaja.ShowDialog()
        UnPaseCaja.Dispose()
        If GModificacionOk Then UnCierreCaja_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonListaChequesTerceros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonListaChequesTerceros.Click

        SeleccionarCheques.PCaja = ComboCaja.SelectedValue
        SeleccionarCheques.PBloqueado = True
        If PermisoTotal Then
            SeleccionarCheques.PEsTodo = True
        Else
            SeleccionarCheques.PAbierto = True
        End If
        SeleccionarCheques.PEsChequeEnCartera = True
        SeleccionarCheques.PEsSoloUno = True
        SeleccionarCheques.ShowDialog()
        SeleccionarCheques.Dispose()

    End Sub
    Private Sub LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Dt As New DataTable
        Dim RowsBusqueda() As DataRow
        Dim ConexionStr As String

        CreaDtGrid()

        Dim SqlB As String
        Dim SqlN As String

        Dim SqlCaja As String = ""
        If ComboCaja.SelectedValue <> 99999 Then
            SqlCaja = "CAST(C.Caja AS CHAR(4)) LIKE '" & ComboCaja.SelectedValue & "'"
            ButtonListaChequesTerceros.Visible = True
            ButtonPaseCaja.Visible = True
        Else
            SqlCaja = "CAST(C.Caja AS VARCHAR(4)) LIKE '%'"
            ButtonListaChequesTerceros.Visible = False
            ButtonPaseCaja.Visible = False
        End If

        SqlB = "SELECT 1 AS Operacion,0 AS TipoPase,P.MedioPago,P.Importe,P.Banco,P.Cuenta,P.TipoNota,C.Fecha,P.ClaveCheque,C.Nota AS Comprobante,0 AS ImporteDeposito FROM RecibosDetallePago AS P INNER JOIN RecibosCabeza AS C " & _
                             "ON P.Nota = C.Nota AND P.TipoNota = C.TipoNota WHERE C.Tr = 0 AND (C.TipoNota = 60 OR C.TipoNota = 600 OR C.TipoNota = 65 OR C.TipoNota = 604 OR C.TipoNota = 64) AND " & SqlCaja & " AND C.Estado <> 3 AND (P.Banco = 0 OR P.MedioPago = 3) " & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,-100 AS TipoPase,P.MedioPago,P.Importe,P.Banco,P.Cuenta,P.TipoNota,C.Fecha,P.ClaveCheque,C.Nota AS Comprobante,0 AS ImporteDeposito FROM RecibosDetallePago AS P INNER JOIN RecibosCabeza AS C " & _
                             "ON P.Nota = C.Nota AND P.TipoNota = C.TipoNota WHERE C.Tr = 1 AND (C.TipoNota = 600 OR C.TipoNota = 60) AND " & SqlCaja & " AND C.Estado <> 3 AND (P.Banco = 0 OR P.MedioPago = 1) " & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,0 As TipoPase,P.MedioPago,P.Importe,P.Banco,P.Cuenta,1000 AS TipoNota,C.Fecha,P.ClaveCheque,C.Prestamo AS Comprobante,0 AS ImporteDeposito FROM PrestamosCabeza AS C INNER JOIN PrestamosDetalle AS P ON C.Prestamo = P.Prestamo WHERE " & SqlCaja & " AND C.Estado <> 3" & _
                             " AND (P.Banco = 0 OR P.MedioPago =3)" & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,P.Tipo As TipoPase,P.MedioPago,P.Importe,P.Banco,P.Cuenta,C.TipoMovimiento AS TipoNota,C.Fecha,P.ClaveCheque,C.Movimiento AS Comprobante,0 AS ImporteDeposito FROM CompraDivisasCabeza AS C INNER JOIN CompraDivisasPago AS P ON C.Movimiento = P.Movimiento WHERE " & SqlCaja & " AND C.Estado <> 3" & _
                             " AND (P.Banco = 0 OR P.MedioPago =3)" & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,0 As TipoPase,P.MedioPago,P.Importe,P.Banco,P.Cuenta,C.TipoNota,C.Fecha,P.ClaveCheque,C.Movimiento AS Comprobante,0 AS ImporteDeposito FROM PrestamosMovimientoCabeza AS C INNER JOIN PrestamosMovimientoPago AS P ON C.Movimiento = P.Movimiento WHERE " & SqlCaja & " AND C.Estado <> 3" & _
                             " AND (P.Banco = 0 or P.MedioPago = 3) AND (C.TipoNota = 1010 OR C.TipoNota = 1015)" & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,0 As TipoPase,P.MedioPago,P.Importe,P.Banco,P.Cuenta,C.TipoNota,C.Fecha,P.ClaveCheque,C.Movimiento AS Comprobante,0 AS ImporteDeposito FROM SueldosMovimientoCabeza AS C INNER JOIN SueldosMovimientoPago AS P ON C.Movimiento = P.Movimiento WHERE " & SqlCaja & " AND C.Estado <> 3" & _
                             " AND (P.Banco = 0 OR P.MedioPago = 3) AND C.TipoNota = 4010" & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,0 AS TipoPase,C.MedioPago, C.Importe,C.Banco,C.Cuenta,C.TipoNota,C.Fecha,D.ClaveCheque,C.Movimiento AS Comprobante,D.Importe as ImporteDeposito FROM MovimientosBancarioCabeza AS C LEFT JOIN MovimientosBancarioDetalle AS D ON C.Movimiento = D.Movimiento " & _
                             "WHERE C.Estado <> 3 AND (C.TipoNota = 90 or C.TipoNota = 91) AND " & SqlCaja & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,1 AS TipoPase,D.MedioPago,D.Importe,D.Banco,D.Cuenta,80 AS TipoNota,C.Fecha,D.ClaveCheque,C.Pase AS Comprobante,0 AS ImporteDeposito FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS D ON C.Pase = D.Pase WHERE C.Aceptado = 1 AND C.CajaOrigen = " & ComboCaja.SelectedValue & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,2 AS TipoPase,D.MedioPago,D.Importe,D.Banco,D.Cuenta,80 AS TipoNota,C.Fecha,D.ClaveCheque,C.Pase AS Comprobante,0 AS ImporteDeposito FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS D ON C.Pase = D.Pase WHERE C.Aceptado = 1 AND C.CajaDestino = " & ComboCaja.SelectedValue & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,0 As TipoPase,P.MedioPago,P.Importe,P.Banco,P.Cuenta,C.TipoNota,C.Fecha,P.ClaveCheque,C.Movimiento AS Comprobante,0 AS ImporteDeposito FROM OtrosPagosCabeza AS C INNER JOIN OtrosPagosPago AS P ON C.Movimiento = P.Movimiento WHERE " & SqlCaja & " AND C.Estado <> 3" & _
                             " AND (P.Banco = 0 OR P.MedioPago = 3) AND (C.TipoNota = 5010 OR C.TipoNota = 5020)" & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,0 As TipoPase,C.MedioPago,C.Importe,C.Banco,C.Cuenta,C.TipoOrigen AS TipoNota,C.Recibido as Fecha,C.ClaveCheque,0 AS Comprobante,0 AS ImporteDeposito FROM ChequesIniciales AS C INNER JOIN Cheques AS CH ON C.MedioPago = CH.MedioPago AND C.ClaveCheque = CH.ClaveCheque WHERE CH.Estado = 1 AND " & SqlCaja & _
                             " AND (C.Banco = 0 OR C.MedioPago = 3)" & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,0 As TipoPase,6 AS MedioPago,C.Importe,0 AS Banco,0 AS Cuenta,66 AS TipoNota,Fecha,0 AS ClaveCheque,C.Nota AS Comprobante,0 AS ImporteDeposito FROM RecuperoSenia AS C WHERE C.Estado = 1 AND " & SqlCaja & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,0 As TipoPase,C.MedioPago,C.Importe,0 AS Banco,0 AS Cuenta,444 AS TipoNota,'01/01/1800' AS Fecha,0 AS ClaveCheque,0 AS Comprobante,0 AS ImporteDeposito FROM CajasSaldoInicial AS C WHERE " & SqlCaja & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,0 As TipoPase,D.MedioPago,D.Importe,D.Banco,D.Cuenta,C.Tipo AS TipoNota,C.Fecha,D.ClaveCheque,C.Movimiento AS Comprobante,0 AS ImporteDeposito FROM MovimientosFondoFijoCabeza AS C INNER JOIN MovimientosFondoFijoPago AS D ON C.Movimiento = D.Movimiento WHERE C.Estado <> 3 AND (Tipo = 7001 OR Tipo = 7002) AND " & SqlCaja & " AND (D.Banco = 0 OR D.MedioPago = 3)"

        'Agrego para PEsTrOPagoEspecial1000. incluye cheque 3ros. para que lo de por entregado y lo saque de la caja.
        SqlB = SqlB & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,0 AS TipoPase,P.MedioPago,P.Importe,P.Banco,P.Cuenta,P.TipoNota,C.Fecha,P.ClaveCheque,C.Nota AS Comprobante,0 AS ImporteDeposito FROM RecibosDetallePago AS P INNER JOIN RecibosCabeza AS C " & _
                             "ON P.Nota = C.Nota AND P.TipoNota = C.TipoNota WHERE C.Tr = 1 AND C.TipoNota = 600 AND " & SqlCaja & " AND C.Estado <> 3 AND C.Comentario = 'O.P.Esp.';"



        SqlN = "SELECT 2 AS Operacion,0 AS TipoPase,P.MedioPago, P.Importe,P.Banco,P.Cuenta,P.TipoNota,C.Fecha,P.ClaveCheque,C.Nota AS Comprobante,0 AS ImporteDeposito FROM RecibosDetallePago AS P INNER JOIN RecibosCabeza AS C " & _
                             "ON P.Nota = C.Nota AND P.TipoNota = C.TipoNota WHERE C.Tr = 0 AND (C.TipoNota = 60 OR C.TipoNota = 600 OR C.TipoNota = 65 OR C.TipoNota = 604 OR C.TipoNota = 64) AND " & SqlCaja & " AND C.Estado <> 3 AND (P.Banco = 0 OR P.MedioPago = 3) " & _
                  " UNION ALL " & _
               "SELECT 2 AS Operacion,0 AS TipoPase,P.MedioPago, P.Importe,P.Banco,P.Cuenta,P.TipoNota,C.Fecha,P.ClaveCheque,C.Nota AS Comprobante,0 AS ImporteDeposito FROM RecibosExteriorDetallePago AS P INNER JOIN RecibosExteriorCabeza AS C " & _
                             "ON P.Nota = C.Nota AND P.TipoNota = C.TipoNota WHERE (C.TipoNota = 11 OR C.TipoNota = 12) AND " & SqlCaja & " AND C.Estado <> 3 AND P.Banco = 0 " & _
                  " UNION ALL " & _
               "SELECT 2 AS Operacion,0 As TipoPase,P.MedioPago,P.Importe,P.Banco,P.Cuenta,1000 AS TipoNota,C.Fecha,P.ClaveCheque,C.Prestamo AS Comprobante,0 AS ImporteDeposito FROM PrestamosCabeza AS C INNER JOIN PrestamosDetalle AS P ON C.Prestamo = P.Prestamo WHERE " & SqlCaja & " AND C.Estado <> 3" & _
                             " AND (P.Banco = 0 OR P.MedioPago =3)" & _
                  " UNION ALL " & _
               "SELECT 2 AS Operacion,P.Tipo As TipoPase,P.MedioPago,P.Importe,P.Banco,P.Cuenta,C.TipoMovimiento AS TipoNota,C.Fecha,P.ClaveCheque,C.Movimiento AS Comprobante,0 AS ImporteDeposito FROM CompraDivisasCabeza AS C INNER JOIN CompraDivisasPago AS P ON C.Movimiento = P.Movimiento WHERE " & SqlCaja & " AND C.Estado <> 3" & _
                             " AND (P.Banco = 0 OR P.MedioPago =3)" & _
                  " UNION ALL " & _
               "SELECT 2 AS Operacion,0 As TipoPase,P.MedioPago,P.Importe,P.Banco,P.Cuenta,C.TipoNota,C.Fecha,P.ClaveCheque,C.Movimiento AS Comprobante,0 AS ImporteDeposito FROM PrestamosMovimientoCabeza AS C INNER JOIN PrestamosMovimientoPago AS P ON C.Movimiento = P.Movimiento WHERE " & SqlCaja & " AND C.Estado <> 3" & _
                             " AND (P.Banco = 0 or P.MedioPago = 3) AND (C.TipoNota = 1010 OR C.TipoNota = 1015)" & _
                  " UNION ALL " & _
               "SELECT 2 AS Operacion,0 As TipoPase,P.MedioPago,P.Importe,P.Banco,P.Cuenta,C.TipoNota,C.Fecha,ClaveCheque,C.Movimiento AS Comprobante,0 AS ImporteDeposito FROM SueldosMovimientoCabeza AS C INNER JOIN SueldosMovimientoPago AS P ON C.Movimiento = P.Movimiento WHERE " & SqlCaja & " AND C.Estado <> 3" & _
                             " AND (P.Banco = 0 OR P.MedioPago = 3) AND C.TipoNota = 4010" & _
                  " UNION ALL " & _
               "SELECT 2 AS Operacion,0 AS TipoPase,C.MedioPago, C.Importe,C.Banco,C.Cuenta,C.TipoNota,C.Fecha,D.ClaveCheque,C.Movimiento AS Comprobante,D.Importe AS ImporteDeposito FROM MovimientosBancarioCabeza AS C LEFT JOIN MovimientosBancarioDetalle AS D ON C.Movimiento = D.Movimiento " & _
                             "WHERE C.Estado <> 3 AND (C.TipoNota = 90 or C.TipoNota = 91) AND " & SqlCaja & _
                  " UNION ALL " & _
               "SELECT 2 AS Operacion,1 AS TipoPase,D.MedioPago,D.Importe,D.Banco,D.Cuenta,80 AS TipoNota,C.Fecha,D.ClaveCheque,C.Pase AS Comprobante,0 AS ImporteDeposito FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS D ON C.Pase = D.Pase WHERE C.Aceptado = 1 AND C.CajaOrigen = " & ComboCaja.SelectedValue & _
                  " UNION ALL " & _
               "SELECT 2 AS Operacion,2 AS TipoPase,D.MedioPago,D.Importe,D.Banco,D.Cuenta,80 AS TipoNota,C.Fecha,D.ClaveCheque,C.Pase AS Comprobante,0 AS ImporteDeposito FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS D ON C.Pase = D.Pase WHERE C.Aceptado = 1 AND C.CajaDestino = " & ComboCaja.SelectedValue & _
                  " UNION ALL " & _
               "SELECT 2 AS Operacion,0 As TipoPase,P.MedioPago,P.Importe,P.Banco,P.Cuenta,C.TipoNota,C.Fecha,P.ClaveCheque,C.Movimiento AS Comprobante,0 AS ImporteDeposito FROM OtrosPagosCabeza AS C INNER JOIN OtrosPagosPago AS P ON C.Movimiento = P.Movimiento WHERE " & SqlCaja & " AND C.Estado <> 3" & _
                             " AND (P.Banco = 0 OR P.MedioPago = 3) AND (C.TipoNota = 5010 OR C.TipoNota = 5020)" & _
                   " UNION ALL " & _
               "SELECT 2 AS Operacion,0 As TipoPase,C.MedioPago,C.Importe,C.Banco,C.Cuenta,C.TipoOrigen AS TipoNota,C.Recibido as Fecha,C.ClaveCheque,0 AS Comprobante,0 AS ImporteDeposito FROM ChequesIniciales AS C INNER JOIN Cheques AS CH ON C.MedioPago = CH.MedioPago AND C.ClaveCheque = CH.ClaveCheque WHERE CH.Estado = 1 AND " & SqlCaja & _
                             " AND (C.Banco = 0 OR C.MedioPago = 3)" & _
                   " UNION ALL " & _
               "SELECT 2 AS Operacion,0 As TipoPase,6 AS MedioPago,C.Importe,0 Banco,0 AS Cuenta,66 AS TipoNota,Fecha,0 AS ClaveCheque,C.Nota AS Comprobante,0 AS ImporteDeposito FROM RecuperoSenia AS C WHERE C.Estado = 1 AND " & SqlCaja & _
                   " UNION ALL " & _
               "SELECT 2 AS Operacion,0 As TipoPase,C.MedioPago,C.Importe,0 AS Banco,0 AS Cuenta,444 AS TipoNota,'01/01/1800' AS Fecha,0 AS ClaveCheque,0 AS Comprobante,0 AS ImporteDeposito FROM CajasSaldoInicial AS C WHERE " & SqlCaja & _
                   " UNION ALL " & _
               "SELECT 2 AS Operacion,0 As TipoPase,D.MedioPago,D.Importe,D.Banco,D.Cuenta,C.Tipo AS TipoNota,C.Fecha,D.ClaveCheque,C.Movimiento AS Comprobante,0 AS ImporteDeposito FROM MovimientosFondoFijoCabeza AS C INNER JOIN MovimientosFondoFijoPago AS D ON C.Movimiento = D.Movimiento WHERE C.Estado <> 3 AND (Tipo = 7001 OR Tipo = 7002) AND " & SqlCaja & " AND (D.Banco = 0 OR D.MedioPago = 3)" & ";"

        ' AND C.CajaOrigen = " & ComboCaja.SelectedValue  .Para Todas las cajas (99999)  no trae nada.
        ' AND C.CajaDestino = " & ComboCaja.SelectedValue .Para Todas las cajas (99999)  no trae nada.

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        For Each Row As DataRow In Dt.Rows
            If Row("Operacion") = 1 Then
                ConexionStr = Conexion
            Else : ConexionStr = ConexionN
            End If
            'Modifica clave cheque e importe con los deposito (91) de cheques terceros con MovimientosBancarioDetalle.
            If (Row("TipoNota") = 91 Or Row("TipoNota") = 90) Then
                If IsDBNull(Row("ClaveCheque")) Then
                    Row("ClaveCheque") = 0
                Else
                    Row("Importe") = Row("ImporteDeposito")
                End If
            End If
            RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
            If EsMovimientoBueno(Row("TipoNota"), Row("MedioPago"), RowsBusqueda(0).Item("Tipo"), Row("TipoPase")) And EsChequeOK(Row("TipoNota"), Row("MedioPago"), Row("ClaveCheque"), ConexionStr) Then
                If Not Acumula(Row("Operacion"), RowsBusqueda(0).Item("Tipo"), Row("TipoPase"), Row("TipoNota"), Row("MedioPago"), Row("Banco"), Row("Cuenta"), Row("Importe"), Row("Fecha"), Row("ClaveCheque"), Row("Comprobante")) Then Me.Close() : Exit Sub
            End If
        Next

        Dt.Dispose()

        DtGrid.AcceptChanges()

        For Each Row As DataRow In DtGrid.Rows
            Row("Saldo") = Row("SaldoAnt") + Row("Debito") - Row("Credito")
        Next

        Grid.DataSource = DtGrid

        Grid.Sort(Grid.Columns("Concepto"), System.ComponentModel.ListSortDirection.Ascending)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function Acumula(ByVal Operacion As Integer, ByVal Tipo As Integer, ByVal TipoPase As Integer, ByVal TipoNota As Integer, ByVal MedioPago As Integer, ByVal Banco As Integer, ByVal Cuenta As Double, ByVal Importe As Decimal, ByVal Fecha As Date, ByVal ClaveCheque As Integer, ByVal Comprobante As Decimal) As Boolean

        Dim Moneda As Integer = 0
        Dim RowsBusqueda() As DataRow
        Dim Debito As Decimal = 0
        Dim Credito As Decimal = 0

        If TipoNota = 90 Or TipoNota = 91 Then
            RowsBusqueda = DtCuentas.Select("Banco = " & Banco & " AND Numero = " & Cuenta)
            If RowsBusqueda.Length = 0 Then
                MsgBox("Cuenta " & Cuenta & " en Banco " & Banco & " No Encontrada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            Moneda = RowsBusqueda(0).Item("Moneda")
        End If

        If MedioPago = 3 Then Moneda = 1
        If Tipo = 3 Or MedioPago = 1 Then Moneda = MedioPago
        If Tipo = 11 Then Moneda = 1

        'En TipoNota = 6000 TipoPase indica si el medio de pago ingresa (=0) o se entrega (=1)

        HallaTipoImporte(TipoNota, TipoPase, Debito, Credito, Importe)

        If Credito <> 0 And Debito = 0 And MedioPago = 3 And TipoNota <> 80 Then
            Dim ConexionStr As String
            If Operacion = 1 Then
                ConexionStr = Conexion
            Else : ConexionStr = ConexionN
            End If
            If ChequeReemplazado(MedioPago, ClaveCheque, TipoNota, Comprobante, ConexionStr) Then Return True
        End If

        If (TipoNota = 91 And MedioPago <> 3) Or TipoNota = 90 Then MedioPago = Moneda

        If TipoPase = -100 Then
            If TipoNota = 60 Then
                AgregaAlGrid(MedioPago, Moneda, Fecha, Importe, 0, Importe, 1)
                AgregaAlGrid(MedioPago, Moneda, Fecha, 0, Importe, Importe, 2)
            Else
                AgregaAlGrid(MedioPago, Moneda, Fecha, 0, Importe, Importe, 1)
                AgregaAlGrid(MedioPago, Moneda, Fecha, Importe, 0, Importe, 2)
            End If
        Else
            If MedioPago = 5 Then
                Debito = Importe : Credito = Importe
            End If
            AgregaAlGrid(MedioPago, Moneda, Fecha, Debito, Credito, Importe, Operacion)
        End If

        Return True

    End Function
    Private Sub AgregaAlGrid(ByVal MedioPago As Integer, ByVal Moneda As Integer, ByVal Fecha As Date, ByVal Debito As Decimal, ByVal Credito As Decimal, ByVal Importe As Decimal, ByVal Operacion As Integer)

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGrid.Select("MedioPago = " & MedioPago & "AND Moneda = " & Moneda & " AND Operacion = " & Operacion)
        If RowsBusqueda.Length <> 0 Then
            If DiferenciaDias(Fecha, DateTime1.Value) > 0 Then
                If Debito <> 0 Then RowsBusqueda(0).Item("SaldoAnt") = RowsBusqueda(0).Item("SaldoAnt") + Importe
                If Credito <> 0 Then RowsBusqueda(0).Item("SaldoAnt") = RowsBusqueda(0).Item("SaldoAnt") - Importe
            Else
                RowsBusqueda(0).Item("Debito") = RowsBusqueda(0).Item("Debito") + Debito
                RowsBusqueda(0).Item("Credito") = RowsBusqueda(0).Item("Credito") + Credito
            End If
        Else              'Inserta una linea nueva. 
            Dim Row As DataRow = DtGrid.NewRow
            Row("MedioPago") = MedioPago
            Row("Moneda") = Moneda
            Row("Operacion") = Operacion
            If DiferenciaDias(Fecha, DateTime1.Value) > 0 Then
                Row("Credito") = 0 : Row("Debito") = 0
                If Debito <> 0 Then Row.Item("SaldoAnt") = Importe
                If Credito <> 0 Then Row.Item("SaldoAnt") = -Importe
            Else
                Row("SaldoAnt") = 0
                Row("Debito") = Debito : Row("Credito") = Credito
            End If
            DtGrid.Rows.Add(Row)
        End If

    End Sub
    Private Function LlenaGridPasesPropios() As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT 1 AS Operacion,Pase FROM PaseCajaCabeza WHERE Aceptado = 0 AND CajaOrigen = " & ComboCaja.SelectedValue & ";", Conexion, Dt) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read("SELECT 2 AS Operacion,Pase FROM PaseCajaCabeza WHERE Aceptado = 0 AND CajaOrigen = " & ComboCaja.SelectedValue & ";", ConexionN, Dt) Then Return False
        End If

        BorraGrid(Grid2)

        For Each Row As DataRow In Dt.Rows
            Grid2.Rows.Add(Row("Operacion"), Nothing, Row("Pase"))
        Next

        Return True

    End Function
    Private Function LlenaGridPasesTerceros() As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT 1 AS Operacion,Pase FROM PaseCajaCabeza WHERE CajaDestino = " & ComboCaja.SelectedValue & " AND Aceptado = 0;", Conexion, Dt) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read("SELECT 2 AS Operacion,Pase FROM PaseCajaCabeza WHERE CajaDestino = " & ComboCaja.SelectedValue & " AND Aceptado = 0;", ConexionN, Dt) Then Return False
        End If

        BorraGrid(Grid1)

        For Each Row As DataRow In Dt.Rows
            Grid1.Rows.Add(Row("Operacion"), Nothing, Row("Pase"))
        Next

        Return True

    End Function
    Private Sub LlenaCombosGrid()

        Concepto.DataSource = DtFormasPago
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

        Moneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Dim Row As DataRow = Moneda.DataSource.newrow
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

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim MedioPago As New DataColumn("MedioPago")
        MedioPago.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(MedioPago)

        Dim Moneda As New DataColumn("Moneda")
        Moneda.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Moneda)

        Dim SaldoAnt As New DataColumn("SaldoAnt")
        SaldoAnt.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(SaldoAnt)

        Dim Debito As New DataColumn("Debito")
        Debito.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Debito)

        Dim Credito As New DataColumn("Credito")
        Credito.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Credito)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Saldo)

    End Sub
    Private Sub ArmaDtCuentas()

        DtCuentas = New DataTable

        If Not Tablas.Read("SELECT Banco,Numero,1 AS Operacion,Moneda,0.0 AS Debito,0.0 AS Credito FROM CuentasBancarias;", Conexion, DtCuentas) Then End
        If Not Tablas.Read("SELECT Banco,Numero,2 AS Operacion,Moneda,0.0 AS Debito,0.0 AS Credito FROM CuentasBancarias;", Conexion, DtCuentas) Then End

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
            If PermisoTotal Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
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

        If Grid.Columns(e.ColumnIndex).Name = "SaldoAnt" Or Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            If IsDBNull(e.Value) Then e.Value = 0
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta" Then
            e.Value = Nothing
        End If

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID1 DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid1_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid1.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid1.Columns(e.ColumnIndex).Name = "Pase1" Then
            If Not IsNothing(e.Value) Then
                e.Value = Format(e.Value, "0000-00000000")
                If PermisoTotal Then
                    If Not IsDBNull(Grid1.Rows(e.RowIndex).Cells("Operacion1").Value) Then
                        If Grid1.Rows(e.RowIndex).Cells("Operacion1").Value = 1 Then Grid1.Rows(e.RowIndex).Cells("Candado1").Value = Me.ImageList1.Images.Item("Abierto")
                        If Grid1.Rows(e.RowIndex).Cells("Operacion1").Value = 2 Then Grid1.Rows(e.RowIndex).Cells("Candado1").Value = Me.ImageList1.Images.Item("Cerrado")
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub Grid1_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        Dim Abierto As Boolean
        If Grid1.CurrentRow.Cells("Operacion1").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        UnPaseCaja.PPase = Grid1.Rows(e.RowIndex).Cells("Pase1").Value
        UnPaseCaja.PAbierto = Abierto
        UnPaseCaja.PDtGrid = DtGrid
        If ComboCaja.SelectedValue <> GCaja Then UnPaseCaja.PBloqueaFunciones = True
        UnPaseCaja.ShowDialog()
        UnPaseCaja.Dispose()
        If GModificacionOk Then UnCierreCaja_Load(Nothing, Nothing)

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID2 DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid2_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid2.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid2.Columns(e.ColumnIndex).Name = "Pase2" Then
            If Not IsNothing(e.Value) Then
                e.Value = Format(e.Value, "0000-00000000")
                If PermisoTotal Then
                    If Not IsDBNull(Grid2.Rows(e.RowIndex).Cells("Operacion2").Value) Then
                        If Grid2.Rows(e.RowIndex).Cells("Operacion2").Value = 1 Then Grid2.Rows(e.RowIndex).Cells("Candado2").Value = Me.ImageList1.Images.Item("Abierto")
                        If Grid2.Rows(e.RowIndex).Cells("Operacion2").Value = 2 Then Grid2.Rows(e.RowIndex).Cells("Candado2").Value = Me.ImageList1.Images.Item("Cerrado")
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub Grid2_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid2.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        Dim Abierto As Boolean
        If Grid2.CurrentRow.Cells("Operacion2").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        UnPaseCaja.PPase = Grid2.Rows(e.RowIndex).Cells("Pase2").Value
        UnPaseCaja.PAbierto = Abierto
        UnPaseCaja.PDtGrid = DtGrid
        UnPaseCaja.PCajaOrigen = GCaja
        If ComboCaja.SelectedValue <> GCaja Then UnPaseCaja.PBloqueaFunciones = True
        UnPaseCaja.ShowDialog()
        UnPaseCaja.Dispose()
        If GModificacionOk Then UnCierreCaja_Load(Nothing, Nothing)

    End Sub

End Class