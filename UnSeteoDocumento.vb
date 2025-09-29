Public Class UnSeteoDocumento
    'Codigos de Tabla:
    '-----------------
    '1.  En Documento
    '2.  Tipo Operacion
    '3.  Medio Pago
    '4.  Retenciones
    '5.  IVA 
    '6.  Otros pagos. 
    '7.  Negocios. 
    '8.  Gastos Bancarios. 
    '9.  Gastos de Prestamos. 
    '10. Insumos. 
    '11. Articulos Servicios. 
    '12. Empleados. 
    '13. Articulos Secos. 
    '14. Tipos Pago Otras Facturas. 
    'Codigos Conceptos:
    '------------------
    '200.  Articulos Servicios. 
    '202.  Neto no dependen de tablas y no asignado a lote. 
    '204.  Seña
    '205.  IVA Compra
    '206.  IVA Venta
    '207.  Descarga 
    '208.  Comision 
    '209. Retencion o Percepcion.  
    '210. Flete 
    '211. Otros Conceptos 
    '213. Monto Total
    '214. Monto Afectado
    '300  neto Reventa
    '301  neto Consignacion
    '302  neto Costeo
    '303  neto Reventa MG
    '400  Comision Reventa
    '401  Comision Consignacion
    '402  Comision Costeo
    '403  Comision Reventa MG
    '600  Descarga Reventa
    '601  Descarga Consignacion
    '602  Descarga Costeo
    '603  Descarga Reventa MG
    '410  Flete Terrestre Reventa
    '411  Flete Terrestre Consignacion
    '412  Flete Terrestre Costeo
    '413  Flete Terrestre Reventa MG
    '420  Otros Conceptos Reventa
    '421  Otros Conceptos Consignacion
    '422  Otros Conceptos Costeo
    '423  Otros Conceptos Reventa MG
    '-100 Merma Reventa
    '-101 Merma Consignacion
    '-102 Merma Costeo
    '-103 Merma Reventa MG
    '-105 Alta Reventa
    '-106 Alta Consignacion
    '-107 Alta Costeo
    '-108 Alta Reventa MG
    '-110 Neto Reventa
    '-111 Neto Consignacion
    '-112 Neto Costeo
    '-113 Neto Reventa MG
    '-200 Importe Retiro
    '-201 Importe Deposito
    '-500 Cheque Reemplazado Terceros y rechazo terceros
    '-501 Cheque Reemplazado Propios  y rechazos propios
    'No usar codigo 10 pues es seña para facturas. 
    Public PTipoDocumento As Integer
    Public PBloqueaFunciones As Boolean
    '
    Dim Dt As DataTable
    Dim DtGrid As DataTable
    Dim DtTablasCuentas As DataTable
    Dim DtTablaConceptos As DataTable
    Private WithEvents bs As New BindingSource
    '
    Dim cb As ComboBox
    Private Sub SeteoDocumento_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(11) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False
        Grid.Columns("Lupa").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")

        Select Case PTipoDocumento
            Case 2, 7006, 7100
                ArmaConceptosFacturasVenta()
            Case 7009, 7010
                ArmaConceptosFacturasServicios()
            Case 71001
                ArmaConceptosFacturasContables()
            Case 4, 41
                ArmaConceptosNotaCreditoVenta()
            Case 5, 7, 50, 70, 10005, 10007, 11005, 11007, 11010
                ArmaConceptosNotaFinanceraACliente()
            Case 6, 8, 500, 700, 10006, 10008, 11006, 11008, 11009
                ArmaConceptosNotaFinanceraAProveedores()
                '      Case 11005, 11007, 11006, 11008
                '          ArmaConceptosNotaFinanceraPorDiferenciaDeCambio()
            Case 5005, 5007
                ArmaConceptosNotaFinanceraOtrosProveedores()
            Case 60, 61, 62
                ArmaConceptosReciboCobro()
            Case 64
                ArmaConceptosOrdenPagoACliente()
            Case 600, 601, 605, 607
                ArmaConceptosOrdenPago()
            Case 604
                ArmaConceptosReciboCobroAProveedores()
            Case 65
                ArmaConceptosDevolucionSenia()
            Case 800
                ArmaConceptosNVLP()
            Case 801
                ArmaConceptosNVLPContable()
            Case 900, 901, 902, 903, 7007, 7008, 7030
                ArmaConceptosFacturasProveedores()
            Case 910, 911, 912
                ArmaConceptosLiquidacion()
            Case 913
                ArmaConceptosLiquidacionContable()
            Case 915
                ArmaConceptosLiquidacionInsumos()
            Case 4000
                ArmaConceptosReciboSueldo()
            Case 4010
                ArmaConceptosOrdenPagoSueldos()
            Case 5000
                ArmaConceptosOtraFactura()
            Case 5010
                ArmaConceptosOrdenOtroPago()
            Case 5020
                ArmaConceptosDevolucionOtrosProveedores()
            Case 90, 91, 6091, 93, 6080, 6010, 6011, 12000, 12003, 12004
                ArmaConceptosBancario()
            Case 16000
                ArmaConceptosCompraDivisas()
            Case 16001
                ArmaConceptosVentaDivisas()
            Case 6050, 6052, 6055, 6056
                ArmaConceptosIngresoMercaderia()
            Case 6000, 7003, 7004
                ArmaConceptosConsumoInsumo()
            Case 6060, 6062, 6070, 6071
                ArmaConceptosRemito()
            Case 6072, 6073
                ArmaConceptosNotaCredito()
            Case 7000
                ArmaConceptosReproceso()
            Case 1005, 1007, 7001, 7002, 7206, 7207, 7011
                ArmaConceptosRechazoCheque()
            Case 1008
                ArmaConceptosReemplazoCheque()
            Case 7005
                ArmaConceptosGastosBancarios()
            Case 7201, 7205
                ArmaConceptosAltaFondoFijo()
            Case 7203
                ArmaConceptosRendicionFondoFijo()
            Case 7204
                ArmaConceptosCierreFondoFijo()
            Case 8000
                ArmaConceptosRegistroPrestamo()
            Case 8001
                ArmaConceptosMovimientoPrestamo()
            Case 8002
                ArmaConceptosMovimientoPrestamoAjusteCapital()
            Case 7020
                ArmaConceptosRecuperoSenia()
            Case 12010
                ArmaConceptosTransferenciaBancoExportacion()
            Case 12006, 12007
                ArmaConceptosReintegros()
            Case 61000
                ArmaConceptosConsumoLotesProductosTerminados()
            Case 61001
                ArmaConceptosCostoConsumoLotesProductosTerminados()
        End Select

        CargaTablasCuentasDelSistema()

        LlenaCombosGrid()

        CreaDtGrid()

        ArmaArchivos()

        Select Case PTipoDocumento
            Case 2
                Label1.Text = "Factura Venta"
            Case 7009
                Label1.Text = "Factura Servicios"
            Case 7010
                Label1.Text = "Factura Secos"
            Case 71001
                Label1.Text = "Factura Contable"
            Case 4
                Label1.Text = "Nota de Credito Domestica"
            Case 41
                Label1.Text = "Nota de Credito Exportación"
            Case 5
                Label1.Text = "Nota DEBITOS FINANCIEROS a Cliente"
            Case 10005
                Label1.Text = "Nota DEBITOS FINANCIEROS a Cliente Exportación"
            Case 11005
                Label1.Text = "Nota DEBITOS FINANCIEROS a Cliente por Diferencia de Cambio"
            Case 6
                Label1.Text = "Nota DEBITO al Proveedor"
            Case 10006
                Label1.Text = "Nota DEBITO al Proveedor Importación"
            Case 11006
                Label1.Text = "Nota DEBITO al Proveedor por Diferencia de Cambio"
            Case Is = 8
                Label1.Text = "Nota CREDITO al Proveedor"
            Case Is = 10008
                Label1.Text = "Nota CREDITO al Proveedor Importación"
            Case Is = 11009
                Label1.Text = "Nota CREDITO Contable del Proveedor"
            Case Is = 11008
                Label1.Text = "Nota CREDITO al Proveedor por Diferencia de Cambio"
            Case Is = 11010
                Label1.Text = "Nota CREDITO Contable a Cliente"
            Case 500
                Label1.Text = "Nota DEBITO del Proveedor"
            Case Is = 700
                Label1.Text = "Nota CREDITO del Proveedor"
            Case 7
                Label1.Text = "Nota CREDITOS FINANCIEROS a Cliente"
            Case 10007
                Label1.Text = "Nota CREDITOS FINANCIEROS a Cliente Exportación"
            Case 11007
                Label1.Text = "Nota CREDITOS FINANCIEROS a Cliente por Diferencia de Cambio"
            Case 50
                Label1.Text = "Nota DEBITO del Cliente"
            Case 70
                Label1.Text = "Nota CREDITO del Cliente"
            Case 60
                Label1.Text = "Recibo de Cobro"
            Case 61
                Label1.Text = "Recibo de Cobro Comer. Exterior"
            Case 62
                Label1.Text = "Recibo de Cobro Contable"
            Case 65
                Label1.Text = "Devolución Seña"
            Case 64
                Label1.Text = "Devolución a Cliente"
            Case 600
                Label1.Text = "Orden de Pago"
            Case 601
                Label1.Text = "Orden de Pago Comer. Exterior"
            Case 607
                Label1.Text = "Egreso de Caja a Cuenta de Resultado"
            Case 604
                Label1.Text = "Devolución del Proveedor"
            Case 605
                Label1.Text = "Orden de Pago Contable"
            Case 800
                Label1.Text = "N.V.L.P."
            Case 801
                Label1.Text = "N.V.L.P. Contable"
            Case 900
                Label1.Text = "Factura Proveedores Reventa"
            Case 901
                Label1.Text = "Factura Proveedores que Afecta Lotes"
            Case 902
                Label1.Text = "Factura Proveedores con Orden de Compra"
            Case 903
                Label1.Text = "Otras Factura Proveedores"
            Case 7030
                Label1.Text = "Factura Proveedor Contable"
            Case 913
                Label1.Text = "Liquidaciones Contable"
            Case 915
                Label1.Text = "Liquidaciones Insumos"
            Case 910
                Label1.Text = "Liquidaciones Reventa"
            Case 911
                Label1.Text = "Liquidacion que Reemplaza Factura"
            Case 912
                Label1.Text = "Liquidacion Consignación"
            Case 4000
                Label1.Text = "Recibo Pago de Sueldo"
            Case 4010
                Label1.Text = "Orden Pago de Sueldo"
            Case 5000
                Label1.Text = "Factura de Otros Proveedores"
            Case 5005
                Label1.Text = "Nota DEBITO Financiera a Otros Proveedores"
            Case 5007
                Label1.Text = "Nota CREDITO Financiera a Otros Proveedores"
            Case 5010
                Label1.Text = "Orden pago de Otros Proveedores"
            Case 5020
                Label1.Text = "Devolución de Otros Proveedores"
            Case 1005
                Label1.Text = "Rechazo Cheque Prestamo Capital"
            Case 1007
                Label1.Text = "Rechazo Cheque Pago de Prestamo"
            Case 1008
                Label1.Text = "Reemplazo de Cheque"
            Case 90
                Label1.Text = "Extracción Bancaria"
            Case 91
                Label1.Text = "Deposito Efectivo"
            Case 6091
                Label1.Text = "Deposito Cheques al Dia y Diferidos"
            Case 93
                Label1.Text = "Rechazo Cheque Terceros en Deposito Bancario"
            Case 6000
                Label1.Text = "Consumo de Insumo"
            Case 6010
                Label1.Text = "Deposito Cheque Propio"
            Case 6011
                Label1.Text = "Deposito Debito Automatico Diferido"
            Case 16000
                Label1.Text = "Compra Divisas"
            Case 16001
                Label1.Text = "Venta Divisas"
            Case 6080
                Label1.Text = "Transferencias entre Ctas. Propias"
            Case 6050
                Label1.Text = "Ingreso Mercaderia"
            Case 6052
                Label1.Text = "Devolución Ingreso Mercaderia"
            Case 6060
                Label1.Text = "Remito"
            Case 6062
                Label1.Text = "Devolución Remito"
            Case 6070
                Label1.Text = "Costo Factura/Remito"
            Case 6071
                Label1.Text = "Costo Factura/Remito Exportación"
            Case 6072
                Label1.Text = "Costo Nota de Credito Domestica"
            Case 6073
                Label1.Text = "Costo Nota de Credito Exportación"
            Case 7000
                Label1.Text = "Reproceso de Mercaderia"
            Case 7001
                Label1.Text = "Rechazo de Cheques"
            Case 7002
                Label1.Text = "Rechazo de Cheques"
            Case 7003
                Label1.Text = "Recepción de Insumo"
            Case 7004
                Label1.Text = "Devolución de Recepción de Insumos"
            Case 7005
                Label1.Text = "Gastos Bancarios"
            Case 7006
                Label1.Text = "Factura Venta Exportación"
            Case 7007
                Label1.Text = "Factura Proveedor Reventa Importación"
            Case 7008
                Label1.Text = "Otra Factura Proveedor Importación"
            Case 7011
                Label1.Text = "Rechazo Cheque Otro Proveedore"
            Case 6055
                Label1.Text = "Descarte Mercaderia"
            Case 6056
                Label1.Text = "Diferencia Inventario"
            Case 7100
                Label1.Text = "Ajuste Factura Exportación (Ajuste Positivo)"
            Case 7201
                Label1.Text = "Ajuste Fondo Fijo"
            Case 7203
                Label1.Text = "Rendición Fondo Fijo"
            Case 7204
                Label1.Text = "Cierre Fondo Fijo"
            Case 7205
                Label1.Text = "Reposición Fondo Fijo"
            Case 7206
                Label1.Text = "Rechazo Cheque(Aumenta Fondo)"
            Case 7207
                Label1.Text = "Rechazo Cheque en Reposición"
            Case 8000
                Label1.Text = "Registro de Prestamo"
            Case 8001
                Label1.Text = "Movimiento de Prestamo Por Cancelacion,Intereses,Gastos"
            Case 8002
                Label1.Text = "Movimiento de Prestamo Por Ajuste de Capital"
            Case 7020
                Label1.Text = "Recupero de Seña"
            Case 12000
                Label1.Text = "Cobranza en el Exterior"
            Case 12003
                Label1.Text = "Cobranza en Argentina"
            Case 12010
                Label1.Text = "Transferencia Banco Exportación"
            Case 12004
                Label1.Text = "Liquidacion Divisas Transferidas"
            Case 12006
                Label1.Text = "Reintegros Aduaneros"
            Case 12007
                Label1.Text = "Cobranza Reintegros Aduaneros"
            Case 61000
                Label1.Text = "Consumo Lotes Prod. Terminados"
            Case 61001
                Label1.Text = "Costo Consumo Lotes Prod. Terminados"
            Case Else
                MsgBox("Error Documento No Definido para Asientos Automaticos.")
        End Select

    End Sub
    Private Sub UnSeteoDocumento_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Dim DtAux As DataTable = Dt.Copy
        ActualizaCambiosArhivo(DtAux)

        If Not IsNothing(DtAux.GetChanges) Then
            If MsgBox("Los cambios no fueron Actualizados. Quiere Actualizarlos?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                ButtonAceptar_Click(Nothing, Nothing)
                If Not GModificacionOk Then e.Cancel = True : Exit Sub
            End If
        End If

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        GModificacionOk = False

        ActualizaCambiosArhivo(Dt)

        If IsNothing(Dt.GetChanges) Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Resul As Integer = ActualizaArchivo(Dt)

        If Resul = -1 Then
            MsgBox("Alta ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -2 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
        End If

        ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        If bs.Current Is Nothing Then
            MsgBox("No hay registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Dim Row As DataRowView = bs.Current

        If Row("Item") <> 0 Then
            If MsgBox("El Item se eliminara definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        bs.RemoveCurrent()

    End Sub
    Private Sub ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtGrid.Clear()

        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM SeteoDocumentos WHERE TipoDocumento = " & PTipoDocumento & ";", Conexion, Dt) Then Me.Close() : Exit Sub
        For Each Row As DataRow In Dt.Rows
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Item") = Row("Item")
            Row1("Concepto") = Row("Concepto")
            Row1("Debe") = Row("Debe")
            Row1("Haber") = Row("Haber")
            Row1("ClaveCuenta") = Row("ClaveCuenta")
            Row1("Tabla") = Row("Tabla")
            Dim Centro As Integer
            Dim Cuenta As Integer
            Dim SubCuenta As Integer
            HallaPartesCuenta(Row("ClaveCuenta"), Centro, Cuenta, SubCuenta)
            If Centro <> 0 Then Row1("NombreCentro") = NombreCentro(Centro)
            If Cuenta <> 0 Then Row1("NombreCuenta") = NombreCuenta(Cuenta)
            If SubCuenta <> 0 Then Row1("NombreSubCuenta") = NombreSubCuenta(Cuenta & Format(SubCuenta, "00"))
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        For I As Integer = 0 To Grid.Rows.Count - 2
            Grid.Rows(I).Cells("Concepto").ReadOnly = True
        Next

        '    Grid.Sort(Grid.Columns("Concepto"), System.ComponentModel.ListSortDirection.Ascending)

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function ActualizaArchivo(ByVal DtAux As DataTable) As Double

        Dim Trans As OleDb.OleDbTransaction

        Using MiConexion As New OleDb.OleDbConnection(Conexion)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Try
                    If Not IsNothing(DtAux.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM SeteoDocumentos;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtAux)
                        End Using
                    End If
                    Trans.Commit()
                    Return 1000
                Catch ex As OleDb.OleDbException
                    Trans.Rollback()
                    If ex.ErrorCode = GAltaExiste Then
                        Return -1
                    Else
                        Return -2
                    End If
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                    Return 0
                Finally
                    Trans = Nothing
                End Try
            Catch ex As Exception
                Return -2
            End Try
        End Using

    End Function
    Private Sub ActualizaCambiosArhivo(ByVal DtAux As DataTable)

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid.Rows
            If Row("Item") = 0 Then
                Dim Row1 As DataRow = DtAux.NewRow
                Row1("Item") = 0
                Row1("TipoDocumento") = PTipoDocumento
                Row1("Concepto") = Row("Concepto")
                Row1("Debe") = Row("Debe")
                Row1("Haber") = Row("Haber")
                Row1("Tabla") = Row("Tabla")
                Row1("ClaveCuenta") = Row("ClaveCuenta")
                DtAux.Rows.Add(Row1)
            Else
                RowsBusqueda = DtAux.Select("Item = " & Row("Item"))
                If Row("ClaveCuenta") <> RowsBusqueda(0).Item("ClaveCuenta") Then RowsBusqueda(0).Item("ClaveCuenta") = Row("ClaveCuenta")
                If Row("Debe") <> RowsBusqueda(0).Item("Debe") Then RowsBusqueda(0).Item("Debe") = Row("Debe")
                If Row("Haber") <> RowsBusqueda(0).Item("Haber") Then RowsBusqueda(0).Item("Haber") = Row("Haber")
                If Row("Tabla") <> RowsBusqueda(0).Item("Tabla") Then RowsBusqueda(0).Item("Tabla") = Row("Tabla")
            End If
        Next

        For Each Row As DataRow In DtAux.Rows
            RowsBusqueda = DtGrid.Select("Item = " & Row("Item"))
            If RowsBusqueda.Length = 0 Then
                Row.Delete()
            End If
        Next

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Item As New DataColumn("Item")
        Item.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Item)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Concepto)

        Dim Debe As New DataColumn("Debe")
        Debe.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Debe)

        Dim Haber As New DataColumn("Haber")
        Haber.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Haber)

        Dim ClaveCuenta As New DataColumn("ClaveCuenta")
        ClaveCuenta.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ClaveCuenta)

        Dim NombreCentro As New DataColumn("NombreCentro")
        NombreCentro.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(NombreCentro)

        Dim NombreCuenta As New DataColumn("NombreCuenta")
        NombreCuenta.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(NombreCuenta)

        Dim NombreSubCuenta As New DataColumn("NombreSubCuenta")
        NombreSubCuenta.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(NombreSubCuenta)

        Dim Tabla As New DataColumn("Tabla")
        Tabla.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Tabla)

    End Sub
    Private Sub LlenaCombosGrid()

        Concepto.DataSource = DtTablaConceptos
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

        Tabla1.DataSource = DtTablasCuentas
        Tabla1.DisplayMember = "Nombre"
        Tabla1.ValueMember = "Clave"

    End Sub
    Public Sub CargaTablasCuentasDelSistema()

        Dim Row As DataRow
        DtTablasCuentas = New DataTable

        DtTablasCuentas.Columns.Add("Clave", Type.GetType("System.Int32"))
        DtTablasCuentas.Columns.Add("Nombre", Type.GetType("System.String"))
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 1
        Row("Nombre") = "En Documentos"
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 2
        Row("Nombre") = "Tipo Operacion"
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 5
        Row("Nombre") = "Tabla de IVA"
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 7
        Row("Nombre") = "Negocios"
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 3
        Row("Nombre") = "Tabla Medios de Pago"
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 4
        Row("Nombre") = "Tabla Retenciones/Percep."
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 6
        Row("Nombre") = "Otros Pagos"
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 8
        Row("Nombre") = "Gastos Bancarios"
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 9
        Row("Nombre") = "Gast.de Prestamos"
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 10
        Row("Nombre") = "Insumos"
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 11
        Row("Nombre") = "Articulos Servicios"
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 12
        Row("Nombre") = "Legajos"
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 13
        Row("Nombre") = "Articulos Secos"
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 14
        Row("Nombre") = "Tipos Pago Otras Facturas"
        DtTablasCuentas.Rows.Add(Row)
        '
        Row = DtTablasCuentas.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        DtTablasCuentas.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosFacturasVenta()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos.Columns.Add("Clave", Type.GetType("System.Int32"))
        DtTablaConceptos.Columns.Add("Nombre", Type.GetType("System.String"))
        DtTablaConceptos.Columns.Add("Tipo", Type.GetType("System.Int32"))
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        If PTipoDocumento <> 7100 Then
            Row = DtTablaConceptos.NewRow
            Row("Clave") = 202
            Row("Nombre") = "Neto No Asignado a Lotes"
            Row("Tipo") = 0
            DtTablaConceptos.Rows.Add(Row)
        End If
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 300
        Row("Nombre") = "Neto(Reventa)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 301
        Row("Nombre") = "Neto(Consignación)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 302
        Row("Nombre") = "Neto(Costeo)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 303
        Row("Nombre") = "Neto(Reventa MG)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 206
        Row("Nombre") = "IVA Venta"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 204
        Row("Nombre") = "Seña"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        If PTipoDocumento <> 7006 Then
            Row = DtTablaConceptos.NewRow
            Row("Clave") = 209
            Row("Nombre") = "Percepción"
            Row("Tipo") = 0
            DtTablaConceptos.Rows.Add(Row)
        End If
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosFacturasServicios()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos.Columns.Add("Clave", Type.GetType("System.Int32"))
        DtTablaConceptos.Columns.Add("Nombre", Type.GetType("System.String"))
        DtTablaConceptos.Columns.Add("Tipo", Type.GetType("System.Int32"))
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 202
        Row("Nombre") = "Neto"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

        Row = DtTablaConceptos.NewRow
        Row("Clave") = 206
        Row("Nombre") = "IVA Venta"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        ' 
        If PTipoDocumento <> 7006 Then
            Row = DtTablaConceptos.NewRow
            Row("Clave") = 209
            Row("Nombre") = "Percepción"
            Row("Tipo") = 0
            DtTablaConceptos.Rows.Add(Row)
        End If
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosFacturasContables()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos.Columns.Add("Clave", Type.GetType("System.Int32"))
        DtTablaConceptos.Columns.Add("Nombre", Type.GetType("System.String"))
        DtTablaConceptos.Columns.Add("Tipo", Type.GetType("System.Int32"))
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 202
        Row("Nombre") = "Neto"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

        Row = DtTablaConceptos.NewRow
        Row("Clave") = 206
        Row("Nombre") = "IVA Venta"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        ' 
        If PTipoDocumento <> 7006 Then
            Row = DtTablaConceptos.NewRow
            Row("Clave") = 209
            Row("Nombre") = "Percepción"
            Row("Tipo") = 0
            DtTablaConceptos.Rows.Add(Row)
        End If
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosNotaCreditoVenta()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos.Columns.Add("Clave", Type.GetType("System.Int32"))
        DtTablaConceptos.Columns.Add("Nombre", Type.GetType("System.String"))
        DtTablaConceptos.Columns.Add("Tipo", Type.GetType("System.Int32"))
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 202
        Row("Nombre") = "Neto No Asignado a Lotes"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 300
        Row("Nombre") = "Neto(Reventa)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 301
        Row("Nombre") = "Neto(Consignación)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 302
        Row("Nombre") = "Neto(Costeo)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 303
        Row("Nombre") = "Neto(Reventa MG)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        If PTipoDocumento <> 41 Then
            Row = DtTablaConceptos.NewRow
            Row("Clave") = 206
            Row("Nombre") = "IVA Venta"
            Row("Tipo") = 0
            DtTablaConceptos.Rows.Add(Row)
            '
            Row = DtTablaConceptos.NewRow
            Row("Clave") = 209
            Row("Nombre") = "Percepción"
            Row("Tipo") = 0
            DtTablaConceptos.Rows.Add(Row)
        End If
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosReproceso()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos.Columns.Add("Clave", Type.GetType("System.Int32"))
        DtTablaConceptos.Columns.Add("Nombre", Type.GetType("System.String"))
        DtTablaConceptos.Columns.Add("Tipo", Type.GetType("System.Int32"))
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -100
        Row("Nombre") = "Merma(Reventa)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -101
        Row("Nombre") = "Merma(Consignación)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -102
        Row("Nombre") = "Merma(Costeo)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -103
        Row("Nombre") = "Merma(Reventa MG)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -105
        Row("Nombre") = "Alta(Reventa)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -106
        Row("Nombre") = "Alta(Consignación)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -107
        Row("Nombre") = "Alta(Costeo)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -108
        Row("Nombre") = "Alta(Reventa MG)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -110
        Row("Nombre") = "Baja Total(Reventa)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -111
        Row("Nombre") = "Baja Total(Consignación)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -112
        Row("Nombre") = "Baja Total(Costeo)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -113
        Row("Nombre") = "Baja Total(Reventa MG)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosReciboCobro()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        If PTipoDocumento = 60 Or PTipoDocumento = 62 Then
            ArmaMedioPagoCobranza(DtTablaConceptos, True, 1)
            For Each Row In DtTablaConceptos.Rows
                If Row("Tipo") = 4 Then Row.Delete()
            Next
        Else
            ArmaMedioPagoCobranzaExterior(DtTablaConceptos)
        End If
        '
        If PTipoDocumento = 60 Or PTipoDocumento = 62 Then
            Row = DtTablaConceptos.NewRow
            Row("Clave") = 209
            Row("Nombre") = "Percepciones/Retenciones"
            Row("Tipo") = 0
            DtTablaConceptos.Rows.Add(Row)
        End If
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 214
        Row("Nombre") = "Monto Afectado"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosReciboCobroAProveedores()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        ArmaMedioPagoCobranzaProveedores(DtTablaConceptos, True, 1)
        For Each Row In DtTablaConceptos.Rows
            If Row("Tipo") = 4 Then Row.Delete()
        Next
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 209
        Row("Nombre") = "Percepciones/Retenciones"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 214
        Row("Nombre") = "Monto Afectado"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosOrdenPago()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        If PTipoDocumento = 600 Or 605 Then
            ArmaMedioPagoOrden(DtTablaConceptos, True, 1)
            For Each Row In DtTablaConceptos.Rows
                If Row("Tipo") = 4 Then Row.Delete()
            Next
        Else
            ArmaMedioPagoCobranzaExterior(DtTablaConceptos)
        End If
        '
        If PTipoDocumento = 600 Or PTipoDocumento = 605 Then
            Row = DtTablaConceptos.NewRow
            Row("Clave") = 209
            Row("Nombre") = "Percepciones/Retenciones"
            Row("Tipo") = 0
            DtTablaConceptos.Rows.Add(Row)
        End If
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        If PTipoDocumento <> 607 Then
            Row = DtTablaConceptos.NewRow
            Row("Clave") = 214
            Row("Nombre") = "Monto Afectado"
            Row("Tipo") = 0
            DtTablaConceptos.Rows.Add(Row)
        End If

    End Sub
    Public Sub ArmaConceptosOrdenPagoACliente()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        ArmaMedioPagoOrdenPagoClientes(DtTablaConceptos)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 214
        Row("Nombre") = "Monto Afectado"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosDevolucionSenia()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        ArmaMedioPagoDevolucionSenia(DtTablaConceptos)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosOrdenPagoSueldos()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        ArmaMedioPagoSueldo(DtTablaConceptos)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 214
        Row("Nombre") = "Monto Afectado"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosFacturasProveedores()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos = ArmaConceptosPagoProveedores(False, 1)
        For I As Integer = DtTablaConceptos.Rows.Count - 1 To 0 Step -1
            Row = DtTablaConceptos.Rows(I)
            If Row("Tipo") = 22 Or Row("Tipo") = 25 Or Row("Clave") = 1 Or Row("Clave") = 2 Then Row.Delete()
        Next
        '
        Select Case PTipoDocumento
            Case 900, 7007
                Row = DtTablaConceptos.NewRow
                Row("Clave") = 300
                Row("Nombre") = "Neto(Reventa)"
                Row("Tipo") = 0
                DtTablaConceptos.Rows.Add(Row)
            Case 901, 7008
                Row = DtTablaConceptos.NewRow
                Row("Clave") = 202
                Row("Nombre") = "Neto"
                Row("Tipo") = 0
                DtTablaConceptos.Rows.Add(Row)
            Case 902, 903, 7030
                Row = DtTablaConceptos.NewRow
                Row("Clave") = 202
                Row("Nombre") = "Neto"
                Row("Tipo") = 0
                DtTablaConceptos.Rows.Add(Row)
            Case Else
                MsgBox("Tipo Documento " & PTipoDocumento & "  No Considerado.")
        End Select
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 205
        Row("Nombre") = "IVA Compra"
        Row("Iva") = 0
        Row("ImporteB") = 0
        Row("ImporteN") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 209
        Row("Nombre") = "Percepciones/Retenciones"
        Row("Iva") = 0
        Row("ImporteB") = 0
        Row("ImporteN") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Iva") = 0
        Row("ImporteB") = 0
        Row("ImporteN") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosLiquidacion()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos.Columns.Add("Clave", Type.GetType("System.Int32"))
        DtTablaConceptos.Columns.Add("Nombre", Type.GetType("System.String"))
        DtTablaConceptos.Columns.Add("Tipo", Type.GetType("System.Int32"))
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Select Case PTipoDocumento
            Case 910, 911
                Row = DtTablaConceptos.NewRow
                Row("Clave") = 300
                Row("Nombre") = "Neto(Reventa)"
                Row("Tipo") = 0
                DtTablaConceptos.Rows.Add(Row)
                '
                Row = DtTablaConceptos.NewRow
                Row("Clave") = 400
                Row("Nombre") = "Comisión(Reventa)"
                Row("Tipo") = 0
                DtTablaConceptos.Rows.Add(Row)
                '
                Row = DtTablaConceptos.NewRow
                Row("Clave") = 600
                Row("Nombre") = "Descarga(Reventa)"
                Row("Tipo") = 0
                DtTablaConceptos.Rows.Add(Row)
            Case 912
                Row = DtTablaConceptos.NewRow
                Row("Clave") = 301
                Row("Nombre") = "Neto(Consignación)"
                Row("Tipo") = 0
                DtTablaConceptos.Rows.Add(Row)
                '
                Row = DtTablaConceptos.NewRow
                Row("Clave") = 401
                Row("Nombre") = "Comisión(Consignación)"
                Row("Tipo") = 0
                DtTablaConceptos.Rows.Add(Row)
                '
                Row = DtTablaConceptos.NewRow
                Row("Clave") = 601
                Row("Nombre") = "Descarga(Consignación)"
                Row("Tipo") = 0
                DtTablaConceptos.Rows.Add(Row)
            Case Else
                MsgBox("Tipo Documento " & PTipoDocumento & "  No Considerado.")
        End Select
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 205
        Row("Nombre") = "IVA Compra"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 206
        Row("Nombre") = "IVA Venta"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 209
        Row("Nombre") = "Percepciones/Retenciones"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Bruto"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 204
        Row("Nombre") = "Seña"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosLiquidacionInsumos()
        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Neto"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 205
        Row("Nombre") = "IVA Compra"
        Row("Tipo") = 22
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 206
        Row("Nombre") = "IVA Venta"
        Row("Tipo") = 22
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 5
        Row("Nombre") = "Comisión"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 7
        Row("Nombre") = "Insumos de Producción"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 9
        Row("Nombre") = "Servicios de Producción"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 10
        Row("Nombre") = "Otros Conceptos"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 209
        Row("Nombre") = "Percepciones/Retenciones"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 90
        Row("Nombre") = "Redondeo"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosLiquidacionContable()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos.Columns.Add("Clave", Type.GetType("System.Int32"))
        DtTablaConceptos.Columns.Add("Nombre", Type.GetType("System.String"))
        DtTablaConceptos.Columns.Add("Tipo", Type.GetType("System.Int32"))
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 202
        Row("Nombre") = "Neto"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 208
        Row("Nombre") = "Comisión"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 207
        Row("Nombre") = "Descarga"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 205
        Row("Nombre") = "IVA Compra"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 206
        Row("Nombre") = "IVA Venta"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 209
        Row("Nombre") = "Percepciones/Retenciones"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Bruto"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosOtraFactura()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos.Columns.Add("Clave", Type.GetType("System.Int32"))
        DtTablaConceptos.Columns.Add("Nombre", Type.GetType("System.String"))
        DtTablaConceptos.Columns.Add("Tipo", Type.GetType("System.Int32"))
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        If Not Tablas.Read("SELECT Clave,Nombre,Tipo FROM Tablas WHERE Tipo = 36 ORDER BY Nombre;", Conexion, DtTablaConceptos) Then Me.Close() : Exit Sub
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 205
        Row("Nombre") = "IVA Compra"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 209
        Row("Nombre") = "Percepciones/Retenciones"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosOrdenOtroPago()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        ArmaMedioPagoOrdenOtrosPagos(DtTablaConceptos, True)
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 214
        Row("Nombre") = "Monto Afectado"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosDevolucionOtrosProveedores()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        ArmaMedioPagoDevolucionOtrosPagos(DtTablaConceptos)        ' 

        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 214
        Row("Nombre") = "Monto Afectado"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosCompraDivisas()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos.Columns.Add("Clave", Type.GetType("System.Int32"))
        DtTablaConceptos.Columns.Add("Nombre", Type.GetType("System.String"))
        DtTablaConceptos.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Dt As New DataTable
        ArmaMedioPagoCompraDivisas(Dt)

        For Each Row In Dt.Rows
            Dim Row1 As DataRow = DtTablaConceptos.NewRow
            Row1("Clave") = Row("Clave")
            Row1("Tipo") = Row("Tipo")
            Row1("Nombre") = Row("Nombre")
            If Row("Clave") <> 0 Then
                Row1("Clave") = Row("Clave") + 2000000
                Row1("Nombre") = "Medio Compra Divisa - " & Row("Nombre")
            End If
            DtTablaConceptos.Rows.Add(Row1)
        Next

        Dt = New DataTable
        ArmaMedioPagoCobroDivisas(Dt)

        For Each Row In Dt.Rows
            Dim Row1 As DataRow = DtTablaConceptos.NewRow
            Row1("Clave") = Row("Clave")
            Row1("Tipo") = Row("Tipo")
            Row1("Nombre") = Row("Nombre")
            If Row("Clave") <> 0 Then
                Row1("Clave") = Row("Clave") + 4000000
                Row1("Nombre") = "Medio Pago - " & Row("Nombre")
            End If
            DtTablaConceptos.Rows.Add(Row1)
        Next

        Dt.Dispose()

    End Sub
    Public Sub ArmaConceptosVentaDivisas()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos.Columns.Add("Clave", Type.GetType("System.Int32"))
        DtTablaConceptos.Columns.Add("Nombre", Type.GetType("System.String"))
        DtTablaConceptos.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Dt As New DataTable
        ArmaMedioPagoVentaDivisas(Dt)

        For Each Row In Dt.Rows
            Dim Row1 As DataRow = DtTablaConceptos.NewRow
            Row1("Clave") = Row("Clave")
            Row1("Tipo") = Row("Tipo")
            Row1("Nombre") = Row("Nombre")
            If Row("Clave") <> 0 Then
                Row1("Clave") = Row("Clave") + 3000000
                Row1("Nombre") = "Medio Venta Divisa - " & Row("Nombre")
            End If
            DtTablaConceptos.Rows.Add(Row1)
        Next

        Dt = New DataTable
        ArmaMedioPagoCobroDivisas(Dt)

        For Each Row In Dt.Rows
            If Row("Clave") <> 2 Then
                Dim Row1 As DataRow = DtTablaConceptos.NewRow
                Row1("Clave") = Row("Clave")
                Row1("Tipo") = Row("Tipo")
                Row1("Nombre") = Row("Nombre")
                If Row("Clave") <> 0 Then
                    Row1("Clave") = Row("Clave") + 5000000
                    Row1("Nombre") = "Medio Cobro - " & Row("Nombre")
                End If
                DtTablaConceptos.Rows.Add(Row1)
            End If
        Next

        Dt.Dispose()

    End Sub
    Public Sub ArmaConceptosReciboSueldo()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos.Columns.Add("Clave", Type.GetType("System.Int32"))
        DtTablaConceptos.Columns.Add("Nombre", Type.GetType("System.String"))
        DtTablaConceptos.Columns.Add("Tipo", Type.GetType("System.Int32"))
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        If Not Tablas.Read("SELECT Clave,Nombre,Tipo FROM Tablas WHERE Tipo = 34 ORDER BY Nombre;", Conexion, DtTablaConceptos) Then Me.Close() : Exit Sub
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosNotaFinanceraACliente()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 202
        Row("Nombre") = "Neto Sin IVA"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 206
        Row("Nombre") = "IVA Venta"
        Row("Tipo") = 22
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 209
        Row("Nombre") = "Percepciones/Retenciones"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosNotaFinanceraPorDiferenciaDeCambio()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 202
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosNotaFinanceraAProveedores()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 202
        Row("Nombre") = "Neto Sin IVA"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 205
        Row("Nombre") = "IVA Compra"
        Row("Tipo") = 22
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 209
        Row("Nombre") = "Percepciones/Retenciones"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosNotaFinanceraOtrosProveedores()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosNVLP()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 300
        Row("Nombre") = "Neto(Reventa)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 301
        Row("Nombre") = "Neto(Consignación)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 302
        Row("Nombre") = "Neto(Costeo)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 303
        Row("Nombre") = "Neto(Reventa MG)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 205
        Row("Nombre") = "IVA Compra"
        Row("Tipo") = 22
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 206
        Row("Nombre") = "IVA Venta"
        Row("Tipo") = 22
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 400
        Row("Nombre") = "Comisión(Reventa)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 401
        Row("Nombre") = "Comisión(Consignación)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 402
        Row("Nombre") = "Comisión(Costeo)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 403
        Row("Nombre") = "Comisión(Reventa MG)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 600
        Row("Nombre") = "Descarga(Reventa)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 601
        Row("Nombre") = "Descarga(Consignación)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 602
        Row("Nombre") = "Descarga(Costeo)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 603
        Row("Nombre") = "Descarga(Reventa MG)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 410
        Row("Nombre") = "Flete Terrestre(Reventa)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 411
        Row("Nombre") = "Flete Terrestre(Consignación)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 412
        Row("Nombre") = "Flete Terrestre(Costeo)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 413
        Row("Nombre") = "Flete Terrestre(Reventa MG)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 420
        Row("Nombre") = "Otros Conceptos(Reventa)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 421
        Row("Nombre") = "Otros Conceptos(Consignación)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 422
        Row("Nombre") = "Otros Conceptos(Costeo)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 423
        Row("Nombre") = "Otros Conceptos(Reventa MG)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 209
        Row("Nombre") = "Percepciones/Retenciones"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosNVLPContable()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 202
        Row("Nombre") = "Neto"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 205
        Row("Nombre") = "IVA Compra"
        Row("Tipo") = 22
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 206
        Row("Nombre") = "IVA Venta"
        Row("Tipo") = 22
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 208
        Row("Nombre") = "Comisión"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 207
        Row("Nombre") = "Descarga"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 210
        Row("Nombre") = "Flete Terrestre"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 211
        Row("Nombre") = "Otros Conceptos"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 209
        Row("Nombre") = "Percepciones/Retenciones"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosBancario()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosRechazoCheque()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Total Cheque Terceros"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -501
        Row("Nombre") = "Total Cheque Propio"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosReemplazoCheque()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        ArmaMedioPagoOrden(DtTablaConceptos, True, 1)
        For Each Row In DtTablaConceptos.Rows
            If Row("Tipo") = 4 Then Row.Delete()
        Next
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -500
        Row("Nombre") = "Total Cheque Tercero"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -501
        Row("Nombre") = "Total Cheque Propio"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosRecuperoSenia()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 6
        Row("Nombre") = "Vale Terceros"
        Row("Tipo") = 11
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosIngresoMercaderia()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 300
        Row("Nombre") = "Importe Total(Reventa)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 301
        Row("Nombre") = "Importe Total(Consignación)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 302
        Row("Nombre") = "Importe Total(Costeo)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 303
        Row("Nombre") = "Importe Total(Reventa MG)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosRemito()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 202
        Row("Nombre") = "Importe No Asignado a Lotes"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 300
        Row("Nombre") = "Importe Total(Reventa)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 301
        Row("Nombre") = "Importe Total(Consignación)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 302
        Row("Nombre") = "Importe Total(Costeo)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 303
        Row("Nombre") = "Importe Total(Reventa MG)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Importe Total"
        Row("Tipo") = 0
        '   DtTablaConceptos.Rows.Add(Row)
        '
        If PTipoDocumento = 6070 Or PTipoDocumento = 6071 Then
            Row = DtTablaConceptos.NewRow
            Row("Clave") = 204
            Row("Nombre") = "Seña"
            Row("Tipo") = 0
            DtTablaConceptos.Rows.Add(Row)
        End If

    End Sub
    Private Sub ArmaConceptosNotaCredito()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 202
        Row("Nombre") = "Importe Total(No Asignado a Lotes)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 300
        Row("Nombre") = "Importe Total(Reventa)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 301
        Row("Nombre") = "Importe Total(Consignación)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 302
        Row("Nombre") = "Importe Total(Costeo)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 303
        Row("Nombre") = "Importe Total(Reventa MG)"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosConsumoInsumo()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 202
        Row("Nombre") = "Neto "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosGastosBancarios()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        ' 
        If Not Tablas.Read("SELECT Clave,Nombre,Tipo FROM Tablas WHERE Tipo = 33 ORDER BY Nombre;", Conexion, DtTablaConceptos) Then Me.Close() : Exit Sub
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 205
        Row("Nombre") = "IVA Compra"
        Row("Tipo") = 22
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 209
        Row("Nombre") = "Percepciones/Retenciones"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosRegistroPrestamo()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        ArmaMedioPagoParaPrestamo(DtTablaConceptos)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosMovimientoPrestamo()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        ArmaMedioPagoPrestamo(DtTablaConceptos)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -6
        Row("Nombre") = "Capital A Cancelar"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -7
        Row("Nombre") = "Intereses"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 205
        Row("Nombre") = "IVA Compra"
        Row("Tipo") = 22
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 209
        Row("Nombre") = "Percepciones/Retenciones"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        If Not Tablas.Read("SELECT Clave,Nombre,Tipo FROM Tablas Where Tipo = 32;", Conexion, DtTablaConceptos) Then End
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosMovimientoPrestamoAjusteCapital()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        ArmaMedioPagoPrestamo(DtTablaConceptos)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -2
        Row("Nombre") = "Ajuste Capital"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosAltaFondoFijo()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        ArmaMedioPagoOrdenFondoFijo(DtTablaConceptos)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosRendicionFondoFijo()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos = ArmaConceptosPagoProveedores(False, 1)
        For I As Integer = DtTablaConceptos.Rows.Count - 1 To 0 Step -1
            Row = DtTablaConceptos.Rows(I)
            If Row("Tipo") = 22 Or Row("Tipo") = 25 Or Row("Clave") = 1 Or Row("Clave") = 2 Then Row.Delete()
        Next
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 202
        Row("Nombre") = "Neto"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 205
        Row("Nombre") = "IVA Compra"
        Row("Iva") = 0
        Row("ImporteB") = 0
        Row("ImporteN") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 209
        Row("Nombre") = "Percepciones/Retenciones"
        Row("Iva") = 0
        Row("ImporteB") = 0
        Row("ImporteN") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Iva") = 0
        Row("ImporteB") = 0
        Row("ImporteN") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosCierreFondoFijo()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        DtTablaConceptos.Columns.Add("Clave", Type.GetType("System.Int32"))
        DtTablaConceptos.Columns.Add("Nombre", Type.GetType("System.String"))
        DtTablaConceptos.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosTransferenciaBancoExportacion()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -200
        Row("Nombre") = "Importe Retiro"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -201
        Row("Nombre") = "Importe Deposito"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Public Sub ArmaConceptosReintegros()

        Dim Row As DataRow
        DtTablaConceptos = New DataTable

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)

        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Monto Total"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        ' 
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosConsumoLotesProductosTerminados()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 213
        Row("Nombre") = "Total Consumo"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -100
        Row("Nombre") = "SubTotal Consumo Reventa"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -101
        Row("Nombre") = "SubTotal Consumo Consignación"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -102
        Row("Nombre") = "SubTotal Consumo Reventa MG."
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -103
        Row("Nombre") = "SubTotal Consumo Costeo"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Sub ArmaConceptosCostoConsumoLotesProductosTerminados()

        DtTablaConceptos = New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtTablaConceptos.Columns.Add(Nombre)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtTablaConceptos.Columns.Add(Tipo)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -200
        Row("Nombre") = "Baja SubTotal Reventa"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -201
        Row("Nombre") = "Baja SubTotal Consignación"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -202
        Row("Nombre") = "Baja SubTotal Reventa MG."
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)
        '
        Row = DtTablaConceptos.NewRow
        Row("Clave") = -203
        Row("Nombre") = "Baja SubTotal Costeo"
        Row("Tipo") = 0
        DtTablaConceptos.Rows.Add(Row)

    End Sub
    Private Function HallaNombreConcepto(ByVal Concepto1 As Integer) As String

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = Concepto.DataSource.Select("Concepto = " & Concepto1)
        Return RowsBusqueda(0).Item("Nombre")

    End Function
    Private Function ConsisteConceptosConTabla(ByVal Concepto As Integer, ByVal Tabla As Integer) As Boolean

        Dim RowsBusqueda() As DataRow

        If Tabla = 1 Then
            Select Case PTipoDocumento
                Case 902, 903, 7003, 7203
                    Select Case Concepto
                        Case 1, 2, 202, 300, 301, 302, 303
                        Case Else
                            MsgBox("Tabla Solo Corresponde a Neto Gravado o Neto No Gravado.")
                            Return False
                    End Select
                    'ojo con 5,7,8 en rechazo cheque no se puede informar cuenta en documento.
                Case 607
                    Select Case Concepto
                        Case 213
                        Case Else
                            MsgBox("Tabla Solo Corresponde a Total.")
                            Return False
                    End Select
                Case 50, 70, 6, 500, 700, 5, 7, 8, 6000, 10005, 10006, 10007, 10008, 11005, 11006, 11007, 11008, 901, 7008, 7030
                    Select Case Concepto
                        Case 202
                        Case Else
                            MsgBox("Tabla Solo Corresponde a Neto.")
                            Return False
                    End Select
                Case 61000
                    Select Case Concepto
                        Case 213
                        Case Else
                            MsgBox("Tabla Solo Corresponde a Total Consumo.")
                            Return False
                    End Select
                Case Else
                    MsgBox("Tabla No Corresponde al Concepto.")
                    Return False
            End Select
        End If
        '
        If Tabla = 2 Then
            If PTipoDocumento = 61001 Then Return True
            Select Case Concepto
                Case 300, 301, 303, 400, 401, 403, 600, 601, 603, 410, 411, 413, 420, 421, 423, -100, -101, -102, -103, -105, -106, -107, -108, -110, -111, -112, -113
                Case Else
                    MsgBox("Tabla No Corresponde al Concepto.")
                    Return False
            End Select
        End If
        '
        If Tabla = 3 Then
            RowsBusqueda = DtTablaConceptos.Select("Clave = " & Concepto)
            Select Case RowsBusqueda(0).Item("Tipo")
                Case 1, 2, 3, 4, 6, 7, 9, 10, 11, 15
                Case Else
                    MsgBox("Concepto No Corresponde a Un Medio de Pago.")
                    Return False
            End Select
        End If
        '
        If Tabla = 4 Then
            Select Case Concepto
                Case 209
                Case Else
                    MsgBox("Concepto No Corresponde a Una Retención.")
                    Return False
            End Select
        End If
        '
        If Tabla = 5 Then
            Select Case Concepto
                Case 205, 206
                Case Else
                    MsgBox("Concepto No Corresponde a Una Alícuota de IVA.")
                    Return False
            End Select
        End If
        '
        If Tabla = 6 Then
            RowsBusqueda = DtTablaConceptos.Select("Clave = " & Concepto)
            Select Case RowsBusqueda(0).Item("Tipo")
                Case 36
                Case Else
                    MsgBox("Concepto No Corresponde a Otros Pagos.")
                    Return False
            End Select
        End If
        '
        If Tabla = 7 Then
            Select Case Concepto
                Case 302, 502, 402, 602, 412, 422, -100, -101, -102, -103, -105, -106, -107, -108, -110, -111, -112, -113
                Case Else
                    MsgBox("Concepto No Depende del Negocio.")
                    Return False
            End Select
        End If
        '
        If Tabla = 8 Then
            RowsBusqueda = DtTablaConceptos.Select("Clave = " & Concepto)
            Select Case RowsBusqueda(0).Item("Tipo")
                Case 33
                Case Else
                    MsgBox("Concepto No Corresponde a Otros Pagos.")
                    Return False
            End Select
        End If
        '
        If Tabla = 9 Then
            RowsBusqueda = DtTablaConceptos.Select("Clave = " & Concepto)
            Select Case RowsBusqueda(0).Item("Tipo")
                Case 32
                Case Else
                    MsgBox("Concepto No Corresponde a Gastos de Prestamos.")
                    Return False
            End Select
        End If
        ' 
        If Tabla = 10 Then
            Select Case Concepto
                Case 202
                Case Else
                    MsgBox("Concepto No Corresponde a Un Insumo.")
                    Return False
            End Select
        End If
        ' 
        If Tabla = 11 Then
            If PTipoDocumento <> 7009 Then
                MsgBox("Concepto No Corresponde a Art. de Servicios.")
                Return False
            End If
            Select Case Concepto
                Case 202
                Case Else
                    MsgBox("Concepto No Corresponde a Art. de Servicios.")
                    Return False
            End Select
        End If
        ' 
        If Tabla = 12 Then
            Select Case PTipoDocumento
                Case 4000
                Case Else
                    MsgBox("Concepto No Corresponde a Un Legajo.")
                    Return False
            End Select
        End If
        '
        If Tabla = 13 Then
            If PTipoDocumento <> 7010 Then
                MsgBox("Concepto No Corresponde a Art. de Secos.")
                Return False
            End If
            Select Case Concepto
                Case 202
                Case Else
                    MsgBox("Concepto No Corresponde a Art. de Secos.")
                    Return False
            End Select
        End If
        '
        If Tabla = 14 Then
            Select Case PTipoDocumento
                Case 5000, 5010, 5020
                    Select Case Concepto
                        Case 213
                        Case Else
                            MsgBox("Tabla Solo Corresponde a Importe Total.")
                            Return False
                    End Select
                Case Else
                    MsgBox("Concepto No Corresponde a Tipo Pago Otra Factura.")
                    Return False
            End Select
        End If

        Return True

    End Function
    Private Function Valida() As Boolean

        For Each Row As DataRow In DtGrid.Rows
            Row.RowError = ""
            If Row("Concepto") = 0 Then
                MsgBox("Falta Concepto.")
                Row.RowError = "ERROR"
                Grid.Refresh()
                Return False
            End If
            If Not Row("Debe") And Not Row("Haber") Then
                MsgBox("Falta Informar Debe/Haber.")
                Row.RowError = "ERROR"
                Grid.Refresh()
                Return False
            End If
            If Row("Tabla") = 0 And Row("ClaveCuenta") = 0 Then
                MsgBox("Falta Informar Tabla o Cuenta.")
                Row.RowError = "ERROR"
                Grid.Refresh()
                Return False
            End If
            If Row("Tabla") <> 0 Then
                If Not ConsisteConceptosConTabla(Row("Concepto"), Row("Tabla")) Then
                    Row.RowError = "ERROR"
                    Grid.Refresh()
                    Return False
                End If
                If PTipoDocumento = 61000 And Row("Tabla") <> 1 Then
                    MsgBox("Solo Admite Tabla 'En Documento'.")
                    Row.RowError = "ERROR"
                    Grid.Refresh()
                    Return False
                End If
            End If
            If Row("Tabla") = 2 Or Row("Tabla") = 7 Then
                If Row("ClaveCuenta") = 0 Then
                    MsgBox("Falta Cuenta.")
                    Row.RowError = "ERROR"
                    Grid.Refresh()
                    Return False
                End If
            End If
        Next

        Grid.Refresh()

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        Grid.BeginEdit(True)

    End Sub
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Or Grid.Columns(e.ColumnIndex).Name = "Tabla1" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
                If cb.SelectedIndex < 0 Then cb.SelectedValue = 0
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
            Exit Sub
        End If

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        e.KeyChar = ""

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "ClaveCuenta" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = Format(e.Value, "000-000000-00")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            e.Value = Nothing
        End If

    End Sub
    Private Sub Grid_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellValueChanged

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Concepto")) Then
                Grid.CurrentRow.Cells("Debe").Value = False
                Grid.CurrentRow.Cells("Haber").Value = False
                Grid.CurrentRow.Cells("ClaveCuenta").Value = 0
                Grid.CurrentRow.Cells("Tabla1").Value = 0
                Grid.CurrentRow.Cells("NombreCentro1").Value = ""
                Grid.CurrentRow.Cells("NombreCuenta1").Value = ""
                Grid.CurrentRow.Cells("NombreSubCuenta1").Value = ""
                Grid.Refresh()
            End If
        End If
        '
        If Grid.Columns(e.ColumnIndex).Name = "Tabla1" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Tabla1")) Then
                Grid.CurrentRow.Cells("ClaveCuenta").Value = 0
                Grid.CurrentRow.Cells("NombreCentro1").Value = ""
                Grid.CurrentRow.Cells("NombreCuenta1").Value = ""
                Grid.CurrentRow.Cells("NombreSubCuenta1").Value = ""
                Grid.Refresh()
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            If Grid.Rows(e.RowIndex).Cells("Concepto").Value = 0 Then
                MsgBox("Debe Informar Concepto.")
                Exit Sub
            End If
            If Grid.Rows(e.RowIndex).Cells("Tabla").Value = 3 Or Grid.Rows(e.RowIndex).Cells("Tabla").Value = 4 Or Grid.Rows(e.RowIndex).Cells("Tabla").Value = 5 Or Grid.Rows(e.RowIndex).Cells("Tabla").Value = 6 Then
                MsgBox("Cuenta Informada en Tabla.")
                Exit Sub
            End If
            If Grid.Rows(e.RowIndex).Cells("Tabla").Value = 1 Then
                MsgBox("Cuenta Informada en Documento.")
                Exit Sub
            End If
            If Grid.Rows(e.RowIndex).Cells("Tabla").Value = 2 Or Grid.Rows(e.RowIndex).Cells("Tabla").Value = 7 Then
                SeleccionarCuenta.PEsSoloCuentas = True
            Else
                SeleccionarCuenta.PCentro = 0
            End If
            SeleccionarCuenta.ShowDialog()
            If SeleccionarCuenta.PCuenta <> 0 Then
                Dim Centro As Integer
                Dim Cuenta As Integer
                Dim SubCuenta As Integer
                HallaPartesCuenta(SeleccionarCuenta.PCuenta, Centro, Cuenta, SubCuenta)
                Grid.Rows(e.RowIndex).Cells("ClaveCuenta").Value = SeleccionarCuenta.PCuenta
                If Centro <> 0 Then Grid.Rows(e.RowIndex).Cells("NombreCentro1").Value = NombreCentro(Centro)
                Grid.Rows(e.RowIndex).Cells("NombreCuenta1").Value = NombreCuenta(Cuenta)
                Grid.Rows(e.RowIndex).Cells("NombreSubCuenta1").Value = NombreSubCuenta(Cuenta & Format(SubCuenta, "00"))
            End If
            SeleccionarCuenta.Dispose()
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Item") = 0
        e.Row("Concepto") = 0
        e.Row("Debe") = 0
        e.Row("Haber") = 0
        e.Row("ClaveCuenta") = 0
        e.Row("Tabla") = 0
        e.Row("NombreCentro") = ""
        e.Row("NombreCuenta") = ""
        e.Row("NombreSubCuenta") = ""

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

    End Sub

End Class