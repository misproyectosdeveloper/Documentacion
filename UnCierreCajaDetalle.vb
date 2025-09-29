Public Class UnCierreCajaDetalle
    Private WithEvents bs As New BindingSource
    ' 
    Dim DtFormasPago As DataTable
    Dim DtGrid As DataTable
    Dim DtCuentas As DataTable
    Dim DtTotales As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaCierreCajaDetalle_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ArmaMedioPagoTodos(DtFormasPago, True)

        LlenaCombosGrid()

        ArmaDtCuentas()

        CreaDtGrid()
        CreaDtTotales()

        ComboConcepto.DataSource = DtFormasPago
        ComboConcepto.DisplayMember = "Nombre"
        ComboConcepto.ValueMember = "Clave"
        ComboConcepto.SelectedValue = 0
        With ComboConcepto
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboCaja.DataSource = ArmaDtCajas()
        ComboCaja.DisplayMember = "Nombre"
        ComboCaja.ValueMember = "Clave"
        ComboCaja.SelectedValue = GCaja
        With ComboCaja
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not GCajaTotal Then
            ComboCaja.Enabled = False
        End If

        ComboTipoNota.DataSource = DtTiposTiposNotasCaja(True)
        Dim Row As DataRow = ComboTipoNota.DataSource.newrow
        Row("Nombre") = "Prestamo"
        Row("Codigo") = 1000
        ComboTipoNota.DataSource.rows.add(Row)
        Row = ComboTipoNota.DataSource.newrow
        Row("Nombre") = "Pago Sueldo"
        Row("Codigo") = 4010
        ComboTipoNota.DataSource.rows.add(Row)
        Row = ComboTipoNota.DataSource.newrow
        Row("Nombre") = "Contado Efectivo."
        Row("Codigo") = 1000000
        ComboTipoNota.DataSource.rows.add(Row)
        Row = ComboTipoNota.DataSource.newrow
        Row("Nombre") = "Saldo Inicial"
        Row("Codigo") = 444
        ComboTipoNota.DataSource.rows.add(Row)
        Row = ComboTipoNota.DataSource.newrow
        Row("Nombre") = "Recupero Seña Terceros"
        Row("Codigo") = 66
        ComboTipoNota.DataSource.rows.add(Row)
        Row = ComboTipoNota.DataSource.newrow
        Row("Nombre") = "Compra Divisas"
        Row("Codigo") = 6000
        ComboTipoNota.DataSource.rows.add(Row)
        Row = ComboTipoNota.DataSource.newrow
        Row("Nombre") = "Venta Divisas"
        Row("Codigo") = 6001
        ComboTipoNota.DataSource.rows.add(Row)
        Row = ComboTipoNota.DataSource.newrow
        Row("Nombre") = "Alta Fondo Fijo"
        Row("Codigo") = 7000
        ComboTipoNota.DataSource.rows.add(Row)
        Row = ComboTipoNota.DataSource.newrow
        Row("Nombre") = "Ajuste -Aumenta"
        Row("Codigo") = 7001
        ComboTipoNota.DataSource.rows.add(Row)
        Row = ComboTipoNota.DataSource.newrow
        Row("Nombre") = "Ajuste -Disminuye"
        Row("Codigo") = 7002
        ComboTipoNota.DataSource.rows.add(Row)
        ComboTipoNota.DisplayMember = "Nombre"
        ComboTipoNota.ValueMember = "Codigo"
        ComboTipoNota.SelectedValue = 0
        With ComboTipoNota
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Checked = False
            CheckNoContables.Visible = False
            CheckContables.Visible = False
        End If

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaCierreCajaDetalle_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub ListaCierreCajaDetalle_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Hasta debe ser mayor o igual a Fecha Desde.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Exit Sub
        End If

        If ComboCaja.SelectedValue = 0 Then
            MsgBox("Debe Informar Caja.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboCaja.Focus()
            Exit Sub
        End If

        SqlB = "SELECT 1 AS Operacion,0 AS Origen,0 AS TipoPase,D.MedioPago, D.Importe,D.TipoNota,D.Banco,D.Cuenta,C.Fecha,C.Nota AS Comprobante,C.Estado,C.Emisor,C.ContadoEfectivo,D.ClaveCheque,C.TR,C.NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM RecibosDetallePago AS D INNER JOIN RecibosCabeza AS C " & _
                             "ON D.Nota = C.Nota AND D.TipoNota = C.TipoNota WHERE C.Tr = 0 AND (C.TipoNota = 60 OR C.TipoNota = 600 OR C.TipoNota = 65 OR C.TipoNota = 64 OR C.TipoNota = 604) AND C.Caja = " & ComboCaja.SelectedValue & " AND (D.Banco = 0 OR D.MedioPago = 3)" & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,0 AS Origen,-100 AS TipoPase,D.MedioPago, D.Importe,D.TipoNota,D.Banco,D.Cuenta,C.Fecha,C.Nota AS Comprobante,C.Estado,C.Emisor,C.ContadoEfectivo,D.ClaveCheque,C.Tr,C.NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM RecibosDetallePago AS D INNER JOIN RecibosCabeza AS C " & _
                             "ON D.Nota = C.Nota AND D.TipoNota = C.TipoNota WHERE C.Tr = 1 AND (C.TipoNota = 600 OR C.TipoNota = 60) AND C.Caja = " & ComboCaja.SelectedValue & " AND (D.Banco = 0 OR D.MedioPago = 1)" & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,C.Origen,0 As TipoPase,P.MedioPago,P.Importe,1000 AS TipoNota,P.Banco,P.Cuenta,C.Fecha,C.Prestamo AS Comprobante,C.Estado,Emisor,0 AS ContadoEfectivo,P.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM PrestamosCabeza AS C INNER JOIN PrestamosDetalle AS P ON C.Prestamo = P.Prestamo WHERE C.Caja = " & ComboCaja.SelectedValue & _
                             " AND (P.Banco = 0 OR P.MedioPago = 3)" & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,C.Origen,P.Tipo As TipoPase,P.MedioPago,P.Importe,C.TipoMovimiento AS TipoNota,P.Banco,P.Cuenta,C.Fecha,C.Movimiento AS Comprobante,C.Estado,Emisor,0 AS ContadoEfectivo,P.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM CompraDivisasCabeza AS C INNER JOIN CompraDivisasPago AS P ON C.Movimiento = P.Movimiento WHERE C.Caja = " & ComboCaja.SelectedValue & _
                             " AND (P.Banco = 0 OR P.MedioPago = 3)" & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,C.Origen,0 As TipoPase,P.MedioPago,P.Importe,MC.TipoNota,P.Banco,P.Cuenta,MC.Fecha,MC.Movimiento AS Comprobante,MC.Estado,C.Emisor,0 AS ContadoEfectivo,P.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM PrestamosCabeza AS C INNER JOIN (PrestamosMovimientoCabeza AS MC INNER JOIN PrestamosMovimientoPago AS P ON MC.Movimiento = P.Movimiento) ON C.Prestamo = MC.Prestamo WHERE MC.Caja = " & ComboCaja.SelectedValue & _
                            " AND (P.Banco = 0 OR P.MedioPago = 3) AND (MC.TipoNota = 1015 OR MC.TipoNota = 1010)" & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,0 AS Origen,0 As TipoPase,P.MedioPago,P.Importe,C.TipoNota,P.Banco,P.Cuenta,C.Fecha,C.Movimiento AS Comprobante,C.Estado,C.Legajo AS Emisor,0 AS ContadoEfectivo,ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM SueldosMovimientoCabeza AS C INNER JOIN SueldosMovimientoPAGO AS P ON C.Movimiento = P.Movimiento WHERE C.Caja = " & ComboCaja.SelectedValue & _
                            " AND (P.Banco = 0 OR P.MedioPago = 3) AND C.TipoNota = 4010" & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,0 AS Origen,0 AS TipoPase,C.MedioPago, C.Importe,C.TipoNota,C.Banco,C.Cuenta,C.Fecha,C.Movimiento AS Comprobante,C.Estado,C.Banco AS Emisor,0 AS ContadoEfectivo,D.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,D.Importe AS ImporteDeposito,C.Comentario FROM MovimientosBancarioCabeza AS C LEFT JOIN MovimientosBancarioDetalle AS D ON C.Movimiento = D.Movimiento " & _
                             "WHERE (C.TipoNota = 90 Or C.TipoNota = 91) And C.Caja = " & ComboCaja.SelectedValue & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,0 AS Origen,1 AS TipoPase,D.MedioPago,D.Importe,80 AS TipoNota,D.Banco,D.Cuenta,C.Fecha,C.Pase AS Comprobante,1 AS Estado,0 As Emisor,0 AS ContadoEfectivo,D.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,'' AS Comentario FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS D ON C.Pase = D.Pase WHERE C.Aceptado = 1 AND C.CajaOrigen = " & ComboCaja.SelectedValue & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,0 AS Origen,2 AS TipoPase,D.MedioPago,D.Importe,80 AS TipoNota,D.Banco,D.Cuenta,C.Fecha,C.Pase AS Comprobante,1 AS Estado,0 As Emisor,0 AS ContadoEfectivo,D.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,'' AS Comentario FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS D ON C.Pase = D.Pase WHERE C.Aceptado = 1 AND C.CajaDestino = " & ComboCaja.SelectedValue & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,0 AS Origen,0 As TipoPase,P.MedioPago,P.Importe,C.TipoNota,P.Banco,P.Cuenta,C.Fecha,C.Movimiento AS Comprobante,C.Estado,C.Proveedor AS Emisor,0 As ContadoEfectivo,P.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM OtrosPagosCabeza AS C INNER JOIN OtrosPagosPago AS P ON C.Movimiento = P.Movimiento WHERE C.Caja = " & ComboCaja.SelectedValue & _
                            " AND (P.Banco = 0 OR P.MedioPago = 3) AND (C.TipoNota = 5010 OR C.TipoNota = 5020)" & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,C.Origen AS Origen,0 As TipoPase,C.MedioPago,C.Importe,C.TipoOrigen AS TipoNota,C.Banco,C.Cuenta,C.Recibido AS Fecha,0 AS Comprobante,CH.Estado,C.Emisor AS Emisor,0 As ContadoEfectivo,C.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,'' AS Comentario FROM ChequesIniciales AS C INNER JOIN Cheques AS CH ON C.MedioPago = CH.MedioPago AND C.ClaveCheque = CH.ClaveCheque WHERE C.Caja = " & ComboCaja.SelectedValue & _
                            " AND (C.Banco = 0 OR C.MedioPago = 3)" & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,0 AS Origen,0 As TipoPase,6 AS MedioPago,C.Importe,66 AS TipoNota,0 AS Banco,0 AS Cuenta,C.Fecha,C.Nota AS Comprobante,C.Estado,C.Proveedor AS Emisor,0 As ContadoEfectivo,0 AS ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,'' AS Comentario FROM RecuperoSenia AS C WHERE C.Caja = " & ComboCaja.SelectedValue & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,0 AS Origen,0 As TipoPase,C.MedioPago,C.Importe,444 AS TipoNota,0 AS Banco,0 AS Cuenta,'01/01/1800' AS Fecha,0 AS Comprobante,1 AS Estado,0 AS Emisor,0 As ContadoEfectivo,0 AS ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,'' AS Comentario FROM CajasSaldoInicial AS C WHERE C.Caja = " & ComboCaja.SelectedValue & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,0 AS Origen,0 As TipoPase,P.MedioPago,P.Importe,C.Tipo AS TipoNota,P.Banco,P.Cuenta,C.Fecha,C.Movimiento AS Comprobante,C.Estado,C.FondoFijo AS Emisor,0 As ContadoEfectivo,ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM MovimientosFondoFijoCabeza AS C INNER JOIN MovimientosFondoFijoPago AS P ON C.Movimiento = P.Movimiento WHERE (C.Tipo = 7001 OR C.Tipo = 7002) AND C.Caja = " & ComboCaja.SelectedValue & _
                            " AND (P.Banco = 0 OR P.MedioPago = 3)"

        'Agrego para PEsTrOPagoEspecial1000. incluye cheque 3ros. para que lo de por entregado y lo saque de la caja.
        SqlB = SqlB & _
               " UNION ALL " & _
               "SELECT 1 AS Operacion,0 AS Origen,0 AS TipoPase,D.MedioPago, D.Importe,D.TipoNota,D.Banco,D.Cuenta,C.Fecha,C.Nota AS Comprobante,C.Estado,C.Emisor,C.ContadoEfectivo,D.ClaveCheque,C.TR,C.NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM RecibosDetallePago AS D INNER JOIN RecibosCabeza AS C " & _
                             "ON D.Nota = C.Nota AND D.TipoNota = C.TipoNota WHERE C.Tr = 1 AND C.TipoNota = 600 AND C.Caja = " & ComboCaja.SelectedValue & " AND C.Comentario = 'O.P.Esp.';"


        '
        SqlN = "SELECT 2 AS Operacion,0 AS Origen,0 AS TipoPase,D.MedioPago, D.Importe,D.TipoNota,D.Banco,D.Cuenta,C.Fecha,C.Nota AS Comprobante,C.Estado,C.Emisor,C.ContadoEfectivo,D.ClaveCheque,C.Tr,NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM RecibosDetallePago AS D INNER JOIN RecibosCabeza AS C " & _
                             "ON D.Nota = C.Nota AND D.TipoNota = C.TipoNota WHERE C.Tr = 0 AND (C.TipoNota = 60 OR C.TipoNota = 600 OR C.TipoNota = 65 OR C.TipoNota = 64 OR C.TipoNota = 604) AND C.Caja = " & ComboCaja.SelectedValue & " AND (D.Banco = 0 OR D.MedioPago = 3)" & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,0 AS Origen,0 AS TipoPase,D.MedioPago, D.Importe,D.TipoNota,D.Banco,D.Cuenta,C.Fecha,C.Nota AS Comprobante,C.Estado,C.Emisor,0 AS ContadoEfectivo,D.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM RecibosExteriorDetallePago AS D INNER JOIN RecibosExteriorCabeza AS C " & _
                             "ON D.Nota = C.Nota AND D.TipoNota = C.TipoNota WHERE (C.TipoNota = 11 OR C.TipoNota = 12) AND C.Caja = " & ComboCaja.SelectedValue & " AND D.Banco = 0" & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,C.Origen,0 As TipoPase,P.MedioPago,P.Importe,1000 AS TipoNota,P.Banco,P.Cuenta,C.Fecha,C.Prestamo AS Comprobante,C.Estado,Emisor,0 AS ContadoEfectivo,P.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM PrestamosCabeza AS C INNER JOIN PrestamosDetalle AS P ON C.Prestamo = P.Prestamo WHERE C.Caja = " & ComboCaja.SelectedValue & _
                             " AND (P.Banco = 0 OR P.MedioPago = 3)" & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,C.Origen,P.Tipo As TipoPase,P.MedioPago,P.Importe,C.TipoMovimiento AS TipoNota,P.Banco,P.Cuenta,C.Fecha,C.Movimiento AS Comprobante,C.Estado,Emisor,0 AS ContadoEfectivo,P.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM CompraDivisasCabeza AS C INNER JOIN CompraDivisasPago AS P ON C.Movimiento = P.Movimiento WHERE C.Caja = " & ComboCaja.SelectedValue & _
                             " AND (P.Banco = 0 OR P.MedioPago = 3)" & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,C.Origen,0 As TipoPase,P.MedioPago,P.Importe,MC.TipoNota,P.Banco,P.Cuenta,MC.Fecha,MC.Movimiento AS Comprobante,MC.Estado,C.Emisor,0 AS ContadoEfectivo,P.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM PrestamosCabeza AS C INNER JOIN (PrestamosMovimientoCabeza AS MC INNER JOIN PrestamosMovimientoPago AS P ON MC.Movimiento = P.Movimiento) ON C.Prestamo = MC.Prestamo WHERE MC.Caja = " & ComboCaja.SelectedValue & _
                            " AND (P.Banco = 0 OR P.MedioPago = 3) AND (MC.TipoNota = 1010 OR MC.TipoNota = 1015)" & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,0 AS Origen,0 As TipoPase,P.MedioPago,P.Importe,C.TipoNota,P.Banco,P.Cuenta,C.Fecha,C.Movimiento AS Comprobante,C.Estado,C.Legajo AS Emisor,0 AS ContadoEfectivo,ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM SueldosMovimientoCabeza AS C INNER JOIN SueldosMovimientoPago AS P ON C.Movimiento = P.Movimiento WHERE C.Caja = " & ComboCaja.SelectedValue & _
                            " AND (P.Banco = 0 OR P.MedioPago = 3) AND C.TipoNota = 4010" & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,0 AS Origen,0 AS TipoPase,C.MedioPago, C.Importe,C.TipoNota,C.Banco,C.Cuenta,C.Fecha,C.Movimiento AS Comprobante,C.Estado,C.Banco AS Emisor,0 AS ContadoEfectivo,D.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,D.Importe AS ImporteDeposito,C.Comentario FROM MovimientosBancarioCabeza AS C LEFT JOIN MovimientosBancarioDetalle AS D ON C.Movimiento = D.Movimiento " & _
                             "WHERE (C.TipoNota = 90 Or C.TipoNota = 91) And C.Caja = " & ComboCaja.SelectedValue & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,0 AS Origen,1 AS TipoPase,D.MedioPago,D.Importe,80 AS TipoNota,D.Banco,D.Cuenta,C.Fecha,C.Pase AS Comprobante,1 AS Estado,0 As Emisor,0 AS ContadoEfectivo,D.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,'' AS Comentario FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS D ON C.Pase = D.Pase WHERE C.Aceptado = 1 AND C.CajaOrigen = " & ComboCaja.SelectedValue & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,0 AS Origen,2 AS TipoPase,D.MedioPago,D.Importe,80 AS TipoNota,D.Banco,D.Cuenta,C.Fecha,C.Pase AS Comprobante,1 AS Estado,0 As Emisor,0 AS ContadoEfectivo,D.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,'' AS Comentario FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS D ON C.Pase = D.Pase WHERE C.Aceptado = 1 AND C.CajaDestino = " & ComboCaja.SelectedValue & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,0 AS Origen,0 As TipoPase,P.MedioPago,P.Importe,C.TipoNota,P.Banco,P.Cuenta,C.Fecha,C.Movimiento AS Comprobante,C.Estado,C.Proveedor as Emisor,0 AS ContadoEfectivo,P.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM OtrosPagosCabeza AS C INNER JOIN OtrosPagosPago AS P ON C.Movimiento = P.Movimiento WHERE C.Caja = " & ComboCaja.SelectedValue & _
                            " AND (P.Banco = 0 OR P.MedioPago = 3) AND (C.TipoNota = 5010 OR C.TipoNota = 5020)" & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,C.Origen AS Origen,0 As TipoPase,C.MedioPago,C.Importe,C.TipoOrigen AS TipoNota,C.Banco,C.Cuenta,C.Recibido AS Fecha,0 AS Comprobante,CH.Estado,C.Emisor AS Emisor,0 As ContadoEfectivo,C.ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,'' AS Comentario FROM ChequesIniciales AS C INNER JOIN Cheques AS CH ON C.MedioPago = CH.MedioPago AND C.ClaveCheque = CH.ClaveCheque WHERE C.Caja = " & ComboCaja.SelectedValue & _
                            " AND (C.Banco = 0 OR C.MedioPago = 3)" & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,0 AS Origen,0 As TipoPase,6 AS MedioPago,C.Importe,66 AS TipoNota,0 AS Banco,0 AS Cuenta,C.Fecha,C.Nota AS Comprobante,C.Estado,C.Proveedor AS Emisor,0 As ContadoEfectivo,0 AS ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,'' AS Comentario FROM RecuperoSenia AS C WHERE C.Caja = " & ComboCaja.SelectedValue & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,0 AS Origen,0 As TipoPase,C.MedioPago,C.Importe,444 AS TipoNota,0 AS Banco,0 AS Cuenta,'01/01/1800' AS Fecha,0 AS Comprobante,1 AS Estado,0 AS Emisor,0 As ContadoEfectivo,0 AS ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,'' AS Comentario FROM CajasSaldoInicial AS C WHERE C.Caja = " & ComboCaja.SelectedValue & _
               " UNION ALL " & _
               "SELECT 2 AS Operacion,0 AS Origen,0 As TipoPase,P.MedioPago,P.Importe,C.Tipo AS TipoNota,P.Banco,P.Cuenta,C.Fecha,C.Movimiento AS Comprobante,C.Estado,C.FondoFijo AS Emisor,0 As ContadoEfectivo,ClaveCheque,0 AS Tr,0 AS NumeroFondoFijo,0 AS ImporteDeposito,C.Comentario FROM MovimientosFondoFijoCabeza AS C INNER JOIN MovimientosFondoFijoPago AS P ON C.Movimiento = P.Movimiento WHERE (C.Tipo = 7001 OR C.Tipo = 7002) AND C.Caja = " & ComboCaja.SelectedValue & _
                            " AND (P.Banco = 0 OR P.MedioPago = 3);"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, "Detalle Cierre de la Caja " & ComboCaja.Text, "", "Desde el " & DateTimeDesde.Text & " Hasta el " & DateTimeHasta.Text)

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
    Private Sub LLenaGrid()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If
        If Not CheckNoContables.Checked And Not CheckContables.Checked Then
            CheckNoContables.Checked = True
            CheckContables.Checked = True
        End If

        DtGrid.Clear()
        DtTotales.Clear()

        Dim Dt As New DataTable
        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Fecha"

        Dim Deb As Decimal
        Dim Cred As Decimal
        Dim SubDeb As Decimal = 0
        Dim SubCred As Decimal = 0
        Dim RowsBusqueda() As DataRow
        Dim RowGrid As DataRow
        Dim ConexionStr As String
        Dim Ok As Boolean

        For Each Row As DataRowView In View
            If DiferenciaDias(Row("Fecha"), DateTimeHasta.Value) < 0 Then Exit For
            If ComboConcepto.SelectedValue <> 0 And Row("MedioPago") <> ComboConcepto.SelectedValue Then Continue For
            If TipoNotaSeleccionada(Row) And EsContableOk(Row("Tr")) Then
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
                HallaTipoImporte(Row("TipoNota"), Row("TipoPase"), Deb, Cred, Row("Importe"))
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                If Row("MedioPago") = 5 Then Cred = Deb
                Ok = True
                If Cred <> 0 And Deb = 0 And Row("Mediopago") = 3 And Row("TipoNota") <> 80 Then
                    If ChequeReemplazado(Row("Mediopago"), Row("ClaveCheque"), Row("TipoNota"), Row("Comprobante"), ConexionStr) Then Ok = False
                End If
                If Ok Then
                    If EsMovimientoBueno(Row("TipoNota"), Row("MedioPago"), RowsBusqueda(0).Item("Tipo"), Row("TipoPase")) And EsChequeOK(Row("TipoNota"), Row("MedioPago"), Row("ClaveCheque"), ConexionStr) Then
                        If DiferenciaDias(Row("Fecha"), DateTimeDesde.Value) > 0 Then
                            If Row("Estado") = 1 Then AcumulaTotales(Row("TipoNota"), Row("MedioPago"), Row("TipoPase"), Row("Banco"), Row("Cuenta"), Row("Operacion"), Deb, Cred, True)
                        Else
                            SubDeb = SubDeb + Deb
                            SubCred = SubCred + Cred
                            If Row("Estado") = 1 Then AcumulaTotales(Row("TipoNota"), Row("MedioPago"), Row("TipoPase"), Row("Banco"), Row("Cuenta"), Row("Operacion"), Deb, Cred, False)
                            LineaDetalle(Row, Deb, Cred)
                        End If
                    End If
                End If
            End If
        Next
        'Subtotal del periodo.
        RowGrid = DtGrid.NewRow
        RowGrid("MarcaTotal") = 1
        RowGrid("Emisor") = "Total Periodo"
        RowGrid("Debito") = SubDeb
        RowGrid("Credito") = SubCred
        RowGrid("Saldo") = SubDeb - SubCred
        DtGrid.Rows.Add(RowGrid)
        'Subtotal del General.
        RowGrid = DtGrid.NewRow
        DtGrid.Rows.Add(RowGrid)
        RowGrid = DtGrid.NewRow
        RowGrid("Emisor") = "TOTALES GENERALES"
        DtGrid.Rows.Add(RowGrid)
        For Each Row As DataRow In DtTotales.Rows
            RowGrid = DtGrid.NewRow
            RowGrid("MarcaTotal") = 1
            RowGrid("Operacion") = Row("Operacion")
            RowGrid("Comprobante") = 0
            RowGrid("Mediopago") = Row("Mediopago")
            RowGrid("Debito") = Row("Debito")
            RowGrid("Credito") = Row("Credito")
            RowGrid("Saldo") = Row("Debito") - Row("Credito") + Row("SaldoAnt")
            RowGrid("SaldoAnt") = Row("SaldoAnt")
            DtGrid.Rows.Add(RowGrid)
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

    End Sub
    Private Sub LineaDetalle(ByVal Row As DataRowView, ByVal DebW As Double, ByVal CredW As Double)

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtFormasPago.Select("Clave = " & Row("Mediopago"))

        Dim RowGrid As DataRow = DtGrid.NewRow
        RowGrid("Operacion") = Row("Operacion")
        RowGrid("Fecha") = Row("Fecha")
        RowGrid("Comprobante") = Row("Comprobante")
        RowGrid("NumeroFondoFijo") = Row("NumeroFondoFijo")
        RowGrid("Emisor") = ""
        RowGrid("Comentario") = Row("Comentario")
        If (Row("TipoNota") = 90 Or Row("TipoNota") = 91) Then RowGrid("Emisor") = NombreBanco(Row("Emisor"))
        If Row("TipoNota") = 60 Or Row("TipoNota") = 65 Or Row("TipoNota") = 11 Or Row("TipoNota") = 12 Or Row("TipoNota") = 64 Then RowGrid("Emisor") = NombreCliente(Row("Emisor"))
        If (Row("TipoNota") = 600 Or Row("TipoNota") = 604) And Row("NumeroFondoFijo") = 0 Then RowGrid("Emisor") = NombreProveedor(Row("Emisor"))
        If Row("TipoNota") = 600 And Row("NumeroFondoFijo") <> 0 Then RowGrid("Emisor") = NombreProveedorFondoFijo(Row("Emisor"))
        If Row("TipoNota") = 7000 Or Row("TipoNota") = 7001 Or Row("TipoNota") = 7002 Then RowGrid("Emisor") = NombreProveedorFondoFijo(Row("Emisor"))
        If Row("TipoNota") = 1000 Or Row("TipoNota") = 1010 Or Row("TipoNota") = 1015 Or Row("TipoNota") = 6000 Or Row("TipoNota") = 6001 Then
            If Row("Origen") = 1 Then RowGrid("Emisor") = NombreBanco(Row("Emisor"))
            If Row("Origen") = 2 Then RowGrid("Emisor") = NombreProveedor(Row("Emisor"))
            If Row("Origen") = 3 Then RowGrid("Emisor") = NombreCliente(Row("Emisor"))
        End If
        If Row("TipoNota") = 4010 Then
            If Row("Emisor") < 5000 Then
                RowGrid("Emisor") = NombreLegajo(Row("Emisor"), Conexion)
            Else : RowGrid("Emisor") = NombreLegajo(Row("Emisor"), ConexionN)
            End If
        End If
        If Row("TipoNota") = 5010 Or Row("TipoNota") = 5020 Then
            RowGrid("Emisor") = NombreDestino(Row("Emisor"))
        End If
        RowGrid("MedioPago") = Row("MedioPago")
        RowGrid("TipoNota") = Row("TipoNota")
        If Row("ContadoEfectivo") Then RowGrid("TipoNota") = 1000000
        RowGrid("Debito") = DebW
        RowGrid("Credito") = CredW
        RowGrid("Saldo") = 0
        If RowsBusqueda(0).Item("Tipo") = 7 Or RowsBusqueda(0).Item("Tipo") = 2 Then
            RowsBusqueda = DtCuentas.Select("Banco = " & Row("Banco") & " AND Numero = " & Row("Cuenta"))
            If RowsBusqueda.Length = 0 Then
                MsgBox("Cuenta " & Row("Cuenta") & " en Banco " & Row("Banco") & " No Encontrada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                RowGrid("Moneda") = "????"
            Else
                RowGrid("Moneda") = RowsBusqueda(0).Item("Moneda")
            End If
        Else
            RowGrid("Moneda") = 0
        End If
        If Row("MedioPago") = 3 Then RowGrid("Moneda") = 1
        If Row("MedioPago") = 1 Then RowGrid("Moneda") = 1
        RowGrid("Estado") = 0
        If Row("Estado") = 3 Then RowGrid("Estado") = Row("Estado")
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Sub AcumulaTotales(ByVal TipoNota As Integer, ByVal MedioPago As Integer, ByVal TipoPase As Integer, ByVal Banco As Integer, ByVal Cuenta As Double, ByVal Operacion As Integer, ByVal DebW As Double, ByVal CredW As Double, ByVal EsSaldoAnterior As Boolean)

        Dim RowsBusqueda() As DataRow
        Dim Moneda As Integer

        If MedioPago = 2 Or MedioPago = 12 Or MedioPago = 8 Then
            RowsBusqueda = DtCuentas.Select("Banco = " & Banco & " AND Numero = " & Cuenta)
            If RowsBusqueda.Length = 0 Then
                MsgBox("Cuenta " & Cuenta & " en Banco " & Banco & " No Encontrada En Archivo de Cuentas Bancarias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End If
            Moneda = RowsBusqueda(0).Item("Moneda")
        End If
        If MedioPago = 3 Then Moneda = 1
        If MedioPago = 1 Then Moneda = 1

        RowsBusqueda = DtFormasPago.Select("Clave = " & MedioPago)
        If RowsBusqueda(0).Item("Tipo") = 3 Then Moneda = MedioPago

        If MedioPago = 8 Then MedioPago = Moneda
        If MedioPago = 12 Then MedioPago = Moneda
        If MedioPago = 2 Then MedioPago = Moneda

        If TipoPase = -100 Then
            If TipoNota = 60 Then
                If CheckAbierto.Checked = True And CheckCerrado.Checked = True Then
                    AgregaADtTotales(MedioPago, Moneda, DebW, 0, 1, EsSaldoAnterior) 'aumenta importe en candado abierto.
                    AgregaADtTotales(MedioPago, Moneda, 0, DebW, 2, EsSaldoAnterior) 'disminuye importe en candado cerrado.
                End If
                If CheckAbierto.Checked = True And CheckCerrado.Checked = False Then
                    AgregaADtTotales(MedioPago, Moneda, DebW, 0, 1, EsSaldoAnterior)  'aumenta importe en candado abierto.
                End If
            Else
                If CheckAbierto.Checked = True And CheckCerrado.Checked = True Then     'igual para tiponota <> 60.
                    AgregaADtTotales(MedioPago, Moneda, 0, CredW, 1, EsSaldoAnterior)
                    AgregaADtTotales(MedioPago, Moneda, CredW, 0, 2, EsSaldoAnterior)
                End If
                If CheckAbierto.Checked = True And CheckCerrado.Checked = False Then
                    AgregaADtTotales(MedioPago, Moneda, 0, CredW, 1, EsSaldoAnterior)
                End If
            End If
        Else
            AgregaADtTotales(MedioPago, Moneda, DebW, CredW, Operacion, EsSaldoAnterior)
        End If

    End Sub
    Private Sub AgregaADtTotales(ByVal MedioPago As Integer, ByVal Moneda As Integer, ByVal DebW As Double, ByVal CredW As Double, ByVal Operacion As Integer, ByVal EsSaldoAnterior As Boolean)

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtTotales.Select("MedioPago = " & MedioPago & "AND Moneda = " & Moneda & " AND Operacion = " & Operacion)
        If RowsBusqueda.Length <> 0 Then
            If EsSaldoAnterior Then
                RowsBusqueda(0).Item("SaldoAnt") = RowsBusqueda(0).Item("SaldoAnt") + DebW - CredW
            Else
                RowsBusqueda(0).Item("Debito") = RowsBusqueda(0).Item("Debito") + DebW
                RowsBusqueda(0).Item("Credito") = RowsBusqueda(0).Item("Credito") + CredW
            End If
        Else              'Inserta una linea nueva. 
            Dim Row As DataRow = DtTotales.NewRow
            Row("MedioPago") = MedioPago
            Row("Moneda") = Moneda
            Row("Operacion") = Operacion
            Row("Debito") = 0
            Row("Credito") = 0
            Row("Saldo") = 0
            Row("SaldoAnt") = 0
            If EsSaldoAnterior Then
                Row("SaldoAnt") = DebW - CredW
            Else
                Row("Debito") = DebW
                Row("Credito") = CredW
            End If
            DtTotales.Rows.Add(Row)
        End If

    End Sub
    Private Sub LlenaCombosGrid()

        Estado.DataSource = DtEstadoActivoYBaja()
        Dim Row As DataRow = Estado.DataSource.newrow
        Row("Nombre") = "Suspendido"
        Row("Clave") = 2
        Estado.DataSource.rows.add(Row)

        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        TipoNota.DataSource = DtTiposComprobantes(True)
        Row = TipoNota.DataSource.newrow
        Row("Nombre") = "Contado Efectivo."
        Row("Codigo") = 1000000
        TipoNota.DataSource.rows.add(Row)
        Row = TipoNota.DataSource.newrow
        Row("Nombre") = "Saldo Inicial"
        Row("Codigo") = 444
        TipoNota.DataSource.rows.add(Row)
        TipoNota.DisplayMember = "Nombre"
        TipoNota.ValueMember = "Codigo"

        Concepto.DataSource = DtFormasPago
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

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

        Dim MarcaTotal As New DataColumn("MarcaTotal")
        MarcaTotal.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(MarcaTotal)

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

        Dim NumeroFondoFijo As New DataColumn("NumeroFondoFijo")
        NumeroFondoFijo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(NumeroFondoFijo)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Comprobante)

        Dim Debito As New DataColumn("Debito")
        Debito.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Debito)

        Dim Credito As New DataColumn("Credito")
        Credito.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Credito)

        Dim Saldo As New DataColumn("Saldo")
        Credito.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Saldo)

        Dim SaldoAnt As New DataColumn("SaldoAnt")
        SaldoAnt.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(SaldoAnt)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Emisor As New DataColumn("Emisor")
        Emisor.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Emisor)

        Dim Comentario As New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comentario)

    End Sub
    Private Sub CreaDtTotales()

        DtTotales = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtTotales.Columns.Add(Operacion)

        Dim MedioPago As New DataColumn("MedioPago")
        MedioPago.DataType = System.Type.GetType("System.Int32")
        DtTotales.Columns.Add(MedioPago)

        Dim Moneda As New DataColumn("Moneda")
        Moneda.DataType = System.Type.GetType("System.Int32")
        DtTotales.Columns.Add(Moneda)

        Dim Debito As New DataColumn("Debito")
        Debito.DataType = System.Type.GetType("System.Decimal")
        DtTotales.Columns.Add(Debito)

        Dim Credito As New DataColumn("Credito")
        Credito.DataType = System.Type.GetType("System.Decimal")
        DtTotales.Columns.Add(Credito)

        Dim Saldo As New DataColumn("Saldo")
        Credito.DataType = System.Type.GetType("System.Decimal")
        DtTotales.Columns.Add(Saldo)

        Dim SaldoAnt As New DataColumn("SaldoAnt")
        SaldoAnt.DataType = System.Type.GetType("System.Decimal")
        DtTotales.Columns.Add(SaldoAnt)


    End Sub
    Private Sub ArmaDtCuentas()

        DtCuentas = New DataTable

        If Not Tablas.Read("SELECT Banco,Numero,Moneda,0.0 AS Debito,0.0 AS Credito FROM CuentasBancarias;", Conexion, DtCuentas) Then End

    End Sub
    Private Function EsContableOk(ByVal Tr As Boolean) As Boolean

        If CheckNoContables.Checked And CheckContables.Checked Then Return True

        If CheckNoContables.Checked And Tr = True Then Return False
        If CheckContables.Checked And Tr = False Then Return False

        Return True

    End Function
    Private Function TipoNotaSeleccionada(ByVal Row As DataRowView) As Boolean

        If ComboTipoNota.SelectedValue = 0 Then Return True
        If ComboTipoNota.SelectedValue = 1000000 Then
            If Row("ContadoEfectivo") Then
                Return True
            Else
                Return False
            End If
        End If
        If ComboTipoNota.SelectedValue = 600 Then
            If Row("TipoNota") = 600 Or Row("TipoNota") = 5010 Then
                Return True
            Else
                Return False
            End If
        End If
        If Row("TipoNota") = ComboTipoNota.SelectedValue Then
            Return True
        Else : Return False
        End If

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
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

        If Grid.Columns(e.ColumnIndex).Name = "Saldo" Or Grid.Columns(e.ColumnIndex).Name = "SaldoAnt" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("MarcaTotal").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else
                e.Value = Format(0, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = "01/01/1800" Then
                    e.Value = ""
                Else
                    e.Value = Format(e.Value, "dd/MM/yyyy")
                End If
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If IsDBNull(Grid.Rows(e.RowIndex).Cells("TipoNota").Value) Then Exit Sub

        If Grid.Rows(e.RowIndex).Cells("Comprobante").Value = 0 Then
            MsgBox("Corresponde a la Carga Inicial del Sistema.")
            Exit Sub
        End If

        Dim Abierto As Boolean

        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 60 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 600 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 65 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 5 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 8 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 64 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 604 Then
            If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 600 And Grid.Rows(e.RowIndex).Cells("NumeroFondoFijo").Value <> 0 Then
                UnReciboReposicion.PAbierto = Abierto
                UnReciboReposicion.PTipoNota = Grid.Rows(e.RowIndex).Cells("TipoNota").Value
                UnReciboReposicion.PNota = Grid.Rows(e.RowIndex).Cells("Comprobante").Value
                UnReciboReposicion.PBloqueaFunciones = True
                UnReciboReposicion.ShowDialog()
            Else
                Select Case Grid.Rows(e.RowIndex).Cells("TipoNota").Value
                    Case 60, 64, 65, 600, 604
                        UnRecibo.PAbierto = Abierto
                        UnRecibo.PTipoNota = Grid.Rows(e.RowIndex).Cells("TipoNota").Value
                        UnRecibo.PNota = Grid.Rows(e.RowIndex).Cells("Comprobante").Value
                        UnRecibo.PBloqueaFunciones = True
                        UnRecibo.ShowDialog()
                    Case Else
                        UnReciboDebitoCredito.PAbierto = Abierto
                        UnReciboDebitoCredito.PTipoNota = Grid.Rows(e.RowIndex).Cells("TipoNota").Value
                        UnReciboDebitoCredito.PNota = Grid.Rows(e.RowIndex).Cells("Comprobante").Value
                        UnReciboDebitoCredito.PBloqueaFunciones = True
                        UnReciboDebitoCredito.ShowDialog()
                End Select
            End If
        End If
        If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 11 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 12 Then
            MsgBox("Detalle en Modulo de Exportación.", MsgBoxStyle.Information)
            Exit Sub
        End If
        If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 1000000 Then
            UnRecibo.PAbierto = Abierto
            UnRecibo.PTipoNota = 60
            UnRecibo.PNota = Grid.Rows(e.RowIndex).Cells("Comprobante").Value
            UnRecibo.PBloqueaFunciones = True
            UnRecibo.ShowDialog()
        End If
        If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 90 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 91 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 92 Then
            UnMovimientoBancario.PMovimiento = Grid.CurrentRow.Cells("Comprobante").Value
            UnMovimientoBancario.PTipoNota = Grid.CurrentRow.Cells("TipoNota").Value
            UnMovimientoBancario.PAbierto = Abierto
            UnMovimientoBancario.PBloqueaFunciones = True
            UnMovimientoBancario.ShowDialog()
            UnMovimientoBancario.Dispose()
        End If
        If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 80 Then
            UnPaseCaja.PPase = Grid.CurrentRow.Cells("Comprobante").Value
            UnPaseCaja.PAbierto = Abierto
            UnPaseCaja.PBloqueaFunciones = True
            UnPaseCaja.ShowDialog()
            UnPaseCaja.Dispose()
        End If
        If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 1000 Then
            UnPrestamo.PPrestamo = Grid.CurrentRow.Cells("Comprobante").Value
            UnPrestamo.PAbierto = Abierto
            UnPrestamo.PBloqueaFunciones = True
            UnPrestamo.ShowDialog()
            UnPrestamo.Dispose()
        End If
        If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 1010 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 1015 Then
            UnMovimientoPrestamo.PMovimiento = Grid.CurrentRow.Cells("Comprobante").Value
            UnMovimientoPrestamo.PAbierto = Abierto
            UnMovimientoPrestamo.PBloqueaFunciones = True
            UnMovimientoPrestamo.ShowDialog()
            UnMovimientoPrestamo.Dispose()
        End If
        If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 4010 Then
            UnaOrdenPagoSueldos.PNota = Grid.CurrentRow.Cells("Comprobante").Value
            UnaOrdenPagoSueldos.PAbierto = Abierto
            UnaOrdenPagoSueldos.PBloqueaFunciones = True
            UnaOrdenPagoSueldos.ShowDialog()
        End If
        If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 5010 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 5020 Then
            UnReciboOtrosProveedores.PNota = Grid.CurrentRow.Cells("Comprobante").Value
            UnReciboOtrosProveedores.PAbierto = Abierto
            UnReciboOtrosProveedores.PBloqueaFunciones = True
            UnReciboOtrosProveedores.ShowDialog()
        End If
        If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 66 Then
            UnRecuperoSenia.PNota = Grid.CurrentRow.Cells("Comprobante").Value
            UnRecuperoSenia.PAbierto = Abierto
            UnRecuperoSenia.PBloqueaFunciones = True
            UnRecuperoSenia.ShowDialog()
            UnRecuperoSenia.Dispose()
        End If
        If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 6000 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 6001 Then
            UnaCompraDivisas.PMovimiento = Grid.CurrentRow.Cells("Comprobante").Value
            UnaCompraDivisas.PAbierto = Abierto
            UnaCompraDivisas.PBloqueaFunciones = True
            UnaCompraDivisas.ShowDialog()
        End If
        If Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 7000 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 7001 Or Grid.Rows(e.RowIndex).Cells("TipoNota").Value = 7002 Then
            UnMovimientoFondoFijo.PMovimiento = Grid.CurrentRow.Cells("Comprobante").Value
            UnMovimientoFondoFijo.PAbierto = Abierto
            UnMovimientoFondoFijo.PBloqueaFunciones = True
            UnMovimientoFondoFijo.ShowDialog()
            UnMovimientoFondoFijo.Dispose()
        End If
    End Sub
End Class