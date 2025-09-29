Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnReciboOtrosProveedores
    Public PNota As Double
    Public PAbierto As Boolean
    Public PTipoNota As Integer
    Public PBloqueaFunciones As Boolean
    '
    Dim DtNotaDetalle As DataTable
    Dim DtNotaCabeza As DataTable
    Dim DtMedioPago As DataTable
    '
    Dim DtRecibosCabeza As DataTable
    Dim DtDevolucionesCabeza As DataTable
    Dim DtNotasDBCabeza As DataTable
    Dim DtGridCompro As DataTable
    Dim DtGrid As DataTable
    Dim DtFormasPago As DataTable
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim cb As ComboBox
    Dim ConexionNota As String
    Dim TotalRecibos As Decimal
    Dim TotalConceptos As Decimal
    Dim UltimoNumero As Double = 0
    Dim Proveedor As Integer
    Dim TipoPagoW As Integer
    'Para Impresion.
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Private Sub UnReciboOtrosProveedores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If GCaja = 0 And PNota = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If Not PermisoEscritura(8) Then PBloqueaFunciones = True

        GridCompro.AutoGenerateColumns = False
        GridCompro.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        If PNota <> 0 Then
            Select Case PTipoNota
                Case 5010
                    If EsReemplazoChequeOtrosProveedores(PTipoNota, PNota, PAbierto) Then
                        UnChequeReemplazo.PTipoNota = PTipoNota
                        UnChequeReemplazo.PNota = PNota
                        UnChequeReemplazo.PAbierto = PAbierto
                        UnChequeReemplazo.PBloqueaFunciones = PBloqueaFunciones
                        UnChequeReemplazo.ShowDialog()
                        Me.Close()
                        Exit Sub
                    End If
            End Select
        End If

        Me.Top = 50

        If PNota = 0 Then
            OpcionOtrosProveedores.ShowDialog()
            Proveedor = OpcionOtrosProveedores.PProveedor
            TipoPagoW = OpcionOtrosProveedores.PTipoPago
            PAbierto = OpcionOtrosProveedores.PAbierto
            OpcionOtrosProveedores.Dispose()
            If Proveedor = 0 Then Me.Close() : Exit Sub
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        GModificacionOk = False

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionNota = Conexion
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionNota = ConexionN
        End If

        LlenaCombosGrid()

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

    End Sub
    Private Sub UnReciboOtrosProveedores_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        GridCompro.EndEdit()
        bs.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim RowsBusqueda() As DataRow

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtNotaDetalleAux = DtNotaDetalle.Copy
        ' 
        Dim DtRecibosCabezaAux As DataTable = DtRecibosCabeza.Copy
        Dim DtDevolucionesCabezaAux As DataTable = DtDevolucionesCabeza.Copy
        Dim DtNotasDBCabezaAux As DataTable = DtNotasDBCabeza.Copy

        'Actualiza Archivos de la nota.
        If PNota = 0 Then ActualizaArchivos(DtNotaCabezaAux, DtMedioPagoAux)
        'Actualiza Saldo de Comprobantes Imputados.
        ActualizaComprobantes("M", DtRecibosCabezaAux, DtDevolucionesCabezaAux, DtNotasDBCabezaAux)

        'Actualiza Notas Detalle.
        For Each Row1 As DataRow In DtGridCompro.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtNotaDetalleAux.Select("TipoComprobante = " & Row1("Tipo") & " AND Comprobante = " & Row1("Comprobante"))
                If RowsBusqueda.Length <> 0 Then
                    If RowsBusqueda(0).Item("Importe") <> Row1.Item("Asignado") Then
                        If Row1.Item("Asignado") = 0 Then
                            RowsBusqueda(0).Delete()
                        Else
                            RowsBusqueda(0).Item("Importe") = Row1.Item("Asignado")
                        End If
                    End If
                Else
                    If Row1.Item("Asignado") <> 0 Then
                        Dim Row As DataRow = DtNotaDetalleAux.NewRow()
                        Row("Movimiento") = DtNotaCabeza.Rows(0).Item("Movimiento")
                        Row("TipoComprobante") = Row1("Tipo")
                        Row("Comprobante") = Row1("Comprobante")
                        Row("Importe") = Row1.Item("Asignado")
                        DtNotaDetalleAux.Rows.Add(Row)
                    End If
                End If
            End If
        Next

        If IsNothing(DtNotaDetalleAux.GetChanges) And IsNothing(DtNotaCabezaAux.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GridCompro.Focus()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If PNota = 0 Then
                If Not ArmaArchivosAsiento("A", DtAsientoCabezaAux, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            Else
                If Not ArmaArchivosAsiento("M", DtAsientoCabezaAux, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            End If
        End If

        If PNota = 0 Then
            If HacerAlta(DtNotaCabezaAux, DtRecibosCabezaAux, DtDevolucionesCabezaAux, DtNotasDBCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux) Then
                ArmaArchivos()
            End If
        Else
            If HacerModificacion(DtNotaCabezaAux, DtRecibosCabezaAux, DtDevolucionesCabezaAux, DtNotasDBCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux) Then
                ArmaArchivos()
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If GCaja = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtNotaCabeza.Rows(0).Item("ChequeRechazado") <> 0 Or DtNotaCabeza.Rows(0).Item("ClaveChequeReemplazado") <> 0 Then
            MsgBox("Anulación Nota Por Cheque Rechazado o Reemplazado Debe Realizarce Por Menu Rechazo de Cheques. Operación se CANCELA.")
            Exit Sub
        End If

        '    If DtNotaCabeza.Rows(0).Item("Importe") <> DtNotaCabeza.Rows(0).Item("Saldo") Then
        '         MsgBox("Nota tiene Imputaciones en Cuenta Corriente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        '         Exit Sub
        '    End If

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy

        'Actualiza Saldo de Comprobantes Imputados.
        Dim DtRecibosCabezaAux As DataTable = DtRecibosCabeza.Copy
        Dim DtDevolucionesCabezaAux As DataTable = DtDevolucionesCabeza.Copy
        Dim DtNotasDBCabezaAux As DataTable = DtNotasDBCabeza.Copy

        ActualizaComprobantes("M", DtRecibosCabezaAux, DtDevolucionesCabezaAux, DtNotasDBCabezaAux)

        If Not IsNothing(DtRecibosCabezaAux.GetChanges) Or Not IsNothing(DtDevolucionesCabezaAux.GetChanges) Or Not IsNothing(DtNotasDBCabezaAux.GetChanges) Then
            MsgBox("Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Resul As Double

        For Each Row As DataRow In DtGrid.Rows
            If Row("MedioPago") = 2 Or Row("MedioPago") = 3 Then
                Dim Anulado As Boolean, Rechazado As Boolean, Depositado As Boolean, Entregado As Boolean, Afectado As Boolean
                If Not EstadoCheque(Row("Mediopago"), Row("ClaveCheque"), ConexionNota, Anulado, Rechazado, Depositado, Entregado, Afectado) Then
                    MsgBox("Error Base de Datos")
                    Exit Sub
                End If
                If Rechazado Then
                    MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Existe Nota de Rechazo. Operación se CANCELA.")
                    Exit Sub
                End If
                If Anulado Then
                    MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Cheque Anulado. Operación se CANCELA.")
                    Exit Sub
                End If
                If Depositado Then
                    MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Cheque Depositado. Operación se CANCELA.")
                    Exit Sub
                End If
            End If
            If Row("MedioPago") = 3 Then
                If ExiteChequeEnPaseCaja(ConexionNota, Row("ClaveCheque")) Then
                    MsgBox("Cheque " & Row("Numero") & " en Proceso de Pase de Caja. Operación se CANCELA.")
                    Exit Sub
                End If
            End If
            If Row("MedioPago") = 14 Then
                Dim Anulado As Boolean, Rechazado As Boolean, Depositado As Boolean, Entregado As Boolean, Afectado As Boolean
                If Not EstadoCheque(Row("MedioPago"), Row("ClaveCheque"), ConexionNota, Anulado, Rechazado, Depositado, Entregado, Afectado) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Error Base de Datos.")
                    Exit Sub
                End If
                If Depositado Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Debito Auto.Dife. " & Row("Comprobante") & " no se puede Borrar, Fue Depositado. Operación se CANCELA.")
                    Exit Sub
                End If
            End If
        Next

        If PTipoNota = 5010 Then
            For Each Row As DataRow In DtGrid.Rows
                If Row("MedioPago") = 2 Or Row("MedioPago") = 3 Then
                    If ChequeReemplazado(Row("MedioPago"), Row("ClaveCheque"), PTipoNota, PNota, ConexionNota) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar, fue Reemplazado. Operación se CANCELA.")
                        Exit Sub
                    End If
                End If
            Next
        End If

        'Actualiza Saldo de Comprobantes Imputados.
        DtRecibosCabezaAux = DtRecibosCabeza.Copy
        DtDevolucionesCabezaAux = DtDevolucionesCabeza.Copy
        DtNotasDBCabezaAux = DtNotasDBCabeza.Copy

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        ActualizaComprobantes("B", DtRecibosCabezaAux, DtDevolucionesCabezaAux, DtNotasDBCabezaAux)

        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        '
        If Not HallaAsientosCabeza(PTipoNota, DtNotaCabezaAux.Rows(0).Item("Movimiento"), DtAsientoCabezaAux, ConexionNota) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        For Each Row As DataRow In DtAsientoCabezaAux.Rows
            Row("Estado") = 3
        Next

        DtNotaCabezaAux.Rows(0).Item("Estado") = 3

        If MsgBox("Nota se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Resul = ActualizaNota("B", DtNotaCabezaAux, DtRecibosCabezaAux, DtDevolucionesCabezaAux, DtNotasDBCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux)
        If Resul = -1 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Baja Fue Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonMediosDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMediosDePago.Click

        Dim TipoNota As Integer

        Select Case PTipoNota
            Case 5010
                TipoNota = 600
            Case 5020
                TipoNota = 60
        End Select

        UnMediosPago.PTipoNota = TipoNota
        UnMediosPago.PEmisor = 0
        UnMediosPago.PDtGrid = DtGrid
        UnMediosPago.PAbierto = PAbierto
        If PNota = 0 Then
            UnMediosPago.PBloqueaFunciones = False
        Else : UnMediosPago.PBloqueaFunciones = True
        End If
        UnMediosPago.PDtFormasPago = DtFormasPago
        UnMediosPago.PDtRetencionProvincia = New DataTable
        If DtNotaCabeza.Rows(0).Item("ChequeRechazado") <> 0 Then UnMediosPago.PEsChequeRechazado = True
        UnMediosPago.PDtRetencionesAutomaticas = New DataTable
        UnMediosPago.PMoneda = 1
        UnMediosPago.PCambio = 0
        UnMediosPago.ShowDialog()
        DtGrid = UnMediosPago.PDtGrid

        UnMediosPago.Dispose()

        CalculaTotales()

    End Sub
    Private Sub ButtonEliminarTodo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarTodo.Click

        If MsgBox("Medios Pagos se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtGrid.Clear()
        CalculaTotales()

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PNota = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = PTipoNota
        If PAbierto Then
            ListaAsientos.PDocumentoB = PNota
        Else
            ListaAsientos.PDocumentoN = PNota
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Nota debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PTipoNota <> 5010 Then
            MsgBox("Opcion Valida Solo para Orden Pago. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Paginas = 0
        Copias = 1

        Dim print_document As New PrintDocument
        AddHandler print_document.PrintPage, AddressOf Print_Print

        print_document.Print()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PNota = 0
        UnReciboOtrosProveedores_Load(Nothing, Nothing)

    End Sub
    Private Function ArmaArchivos() As Boolean                        'ArmaArchivos

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()
        CreaDtGridCompro()

        Dim Sql As String
        Dim Row1 As DataRow
        Dim RowsBusqueda() As DataRow

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM OtrospagosCabeza WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaCabeza) Then Return False
        If PNota <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then
            Dim Row As DataRow = DtNotaCabeza.NewRow
            ArmaNuevoMovimientoOtroPago(Row)
            Row("Proveedor") = Proveedor
            Row("TipoNota") = PTipoNota
            Row("TipoPago") = TipoPagoW
            Row("Fecha") = Now
            Row("Caja") = GCaja
            Row("Estado") = 1
            DtNotaCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        Proveedor = DtNotaCabeza.Rows(0).Item("Proveedor")
        PTipoNota = DtNotaCabeza.Rows(0).Item("TipoNota")
        TipoPagoW = DtNotaCabeza.Rows(0).Item("TipoPago")
        TextProveedor.Text = NombreProveedor(Proveedor)
        TextTipoPago.Text = NombreTipoPago(TipoPagoW)

        DtNotaDetalle = New DataTable
        Sql = "SELECT * FROM OtrosPagosDetalle WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaDetalle) Then Return False

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM OtrosPagosPago WHERE Movimiento = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtMedioPago) Then Return False

        Dim DtCheques As New DataTable
        For Each Row As DataRow In DtMedioPago.Rows
            If Row("ClaveCheque") <> 0 And (Row("MedioPago") = 2 Or Row("MedioPago") = 3) Then
                If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionNota, DtCheques) Then Return False
            End If
        Next

        For Each Row As DataRow In DtMedioPago.Rows
            Row1 = DtGrid.NewRow
            Row1("Item") = Row("Item")
            Row1("MedioPago") = Row("MedioPago")
            Row1("Importe") = Row("Importe")
            Row1("Cambio") = Row("Cambio")
            Row1("Bultos") = 0
            Row1("Detalle") = 0
            Row1("Alicuota") = 0
            Row1("Neto") = 0
            Row1("ImporteIva") = 0
            Row1("NumeracionInicial") = 1
            Row1("NumeracionFinal") = 999999999
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            Row1("ClaveInterna") = 0
            Row1("ClaveChequeVisual") = 0
            If Row("MedioPago") = 3 Then Row1("ClaveChequeVisual") = Row("ClaveCheque")
            If Row("ClaveCheque") <> 0 And (Row("MedioPago") = 2 Or Row("MedioPago") = 3) Then
                RowsBusqueda = DtCheques.Select("MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque"))
                If RowsBusqueda.Length <> 0 Then
                    Row1("Banco") = RowsBusqueda(0).Item("Banco")
                    Row1("Cuenta") = RowsBusqueda(0).Item("Cuenta")
                    Row1("Serie") = RowsBusqueda(0).Item("Serie")
                    Row1("Numero") = RowsBusqueda(0).Item("Numero")
                    Row1("EmisorCheque") = RowsBusqueda(0).Item("EmisorCheque")
                    Row1("Fecha") = RowsBusqueda(0).Item("Fecha")
                    Row1("eCheq") = RowsBusqueda(0).Item("eCheq")
                Else
                    Row1("Banco") = 0
                    Row1("Cuenta") = 0
                    Row1("Serie") = ""
                    Row1("Numero") = 0
                    Row1("EmisorCheque") = "Comp.No Existe."
                    Row1("Fecha") = "1/1/1800"
                    Row1("eCheq") = 0
                End If
            Else
                Row1("Banco") = Row("Banco")
                Row1("Cuenta") = Row("Cuenta")
                Row1("Serie") = ""
                Row1("Numero") = 0
                Row1("EmisorCheque") = ""
                Row1("Fecha") = "1/1/1800"
            End If
            Row1("TieneLupa") = False
            DtGrid.Rows.Add(Row1)
        Next

        DtCheques.Dispose()

        'Muestra Comprobantes a Imputar.
        DtRecibosCabeza = New DataTable
        DtDevolucionesCabeza = New DataTable
        DtNotasDBCabeza = New DataTable
        If ComboEstado.SelectedValue = 1 And PTipoNota = 5010 Then
            If Not ArmaConRecibos() Then Return False
            If Not ArmaConDevoluciones() Then Return False
            If Not ArmaConNotas(5007) Then Return False
        End If

        DtGridCompro.Clear()
        'Procesa Recibos.
        For Each Row As DataRow In DtRecibosCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 5000
            Row1("Comprobante") = Row("Factura")
            Row1("Fecha") = Row("Fecha")
            Row1("TipoPago") = Row("TipoPago")
            Row1("Comentario") = Row("Comentario")
            Row1("Importe") = Row("Importe")
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next
        'Procesa Devoluciones.
        For Each Row As DataRow In DtDevolucionesCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 5020
            Row1("Comprobante") = Row("Movimiento")
            Row1("Fecha") = Row("Fecha")
            Row1("TipoPago") = Row("TipoPago")
            Row1("Comentario") = Row("Comentario")
            Row1("Importe") = Row("Importe")
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next
        'Procesa Notas 5007.
        For Each Row As DataRow In DtNotasDBCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 5007
            Row1("Comprobante") = Row("Movimiento")
            Row1("Fecha") = Row("Fecha")
            Row1("TipoPago") = Row("TipoPago")
            Row1("Comentario") = Row("Comentario")
            Row1("Importe") = Row("Importe")
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next

        For Each Row As DataRow In DtNotaDetalle.Rows
            RowsBusqueda = DtGridCompro.Select("Tipo = " & Row("TipoComprobante") & " AND Comprobante = " & Row("Comprobante"))
            If RowsBusqueda.Length <> 0 Then RowsBusqueda(0).Item("Asignado") = Row("Importe")
        Next

        'Borra los documentos con saldo 0 y no tienen asignacion. 
        DtGridCompro.AcceptChanges()
        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Saldo") = 0 And Row("Asignado") = 0 Then Row.Delete()
        Next

        GridCompro.DataSource = DtGridCompro

        Select Case PTipoNota
            Case 5010
                ArmaMedioPagoOrdenOtrosPagos(DtFormasPago, PAbierto)
                LabelTipoNota.Text = "Orden Pago"
            Case 5020
                ArmaMedioPagoDevolucionOtrosPagos(DtFormasPago)
                LabelTipoNota.Text = "Devolución del Proveedor"
        End Select

        If PNota <> 0 Then
            Panel4.Enabled = False
            ButtonEliminarTodo.Enabled = False
        Else
            Panel4.Enabled = True
            ButtonEliminarTodo.Enabled = True
        End If

        CalculaTotales()

        AddHandler DtGridCompro.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGridCompro_ColumnChanging)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Function ArmaConRecibos() As Boolean

        Dim Sql As String = ""

        If PNota = 0 Then
            Sql = "SELECT * FROM OtrasFacturasCabeza WHERE Estado = 1 AND Proveedor = " & Proveedor & " AND Saldo <> 0 ORDER BY Factura,Fecha;"
        Else
            Sql = "SELECT * FROM OtrasFacturasCabeza WHERE Estado = 1 AND Proveedor = " & Proveedor & " ORDER BY Factura,Fecha;"
        End If

        If Not Tablas.Read(Sql, ConexionNota, DtRecibosCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConDevoluciones() As Boolean

        Dim Sql As String = ""

        If PNota = 0 Then
            Sql = "SELECT * FROM OtrosPagosCabeza WHERE Estado = 1 AND TipoNota = 5020 AND Proveedor = " & Proveedor & " AND Saldo <> 0 ORDER BY Movimiento,Fecha;"
        Else
            Sql = "SELECT * FROM OtrosPagosCabeza WHERE Estado = 1 AND TipoNota = 5020 AND Proveedor = " & Proveedor & " ORDER BY Movimiento,Fecha;"
        End If

        If Not Tablas.Read(Sql, ConexionNota, DtDevolucionesCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConNotas(ByVal TipoNota As Integer) As Boolean

        Dim Sql As String = ""

        If PNota = 0 Then
            Sql = "SELECT * FROM OtrosPagosCabeza WHERE Estado = 1 AND TipoNota = " & TipoNota & " AND Proveedor = " & Proveedor & " AND Saldo <> 0 ORDER BY Movimiento,Fecha;"
        Else
            Sql = "SELECT * FROM OtrosPagosCabeza WHERE Estado = 1 AND TipoNota = " & TipoNota & " AND Proveedor = " & Proveedor & " ORDER BY Movimiento,Fecha;"
        End If

        If Not Tablas.Read(Sql, ConexionNota, DtNotasDBCabeza) Then Return False

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtNotaCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Movimiento")
        AddHandler Enlace.Format, AddressOf FormatNota
        TextComprobante.DataBindings.Clear()
        TextComprobante.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalRecibo.DataBindings.Clear()
        TextTotalRecibo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Saldo")
        TextSaldo.DataBindings.Clear()
        TextSaldo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub FormatNota(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub LlenaCombosGrid()

        Tipo.DataSource = ArmaNotasOtrosPagos()
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Clave"

        TipoPago.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 39;")
        TipoPago.DisplayMember = "Nombre"
        TipoPago.ValueMember = "Clave"

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = CreaDtParaGrid()

    End Sub
    Private Sub CreaDtGridCompro()

        DtGridCompro = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Operacion)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Tipo)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Comprobante)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGridCompro.Columns.Add(Fecha)

        Dim TipoPago As New DataColumn("TipoPago")
        TipoPago.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(TipoPago)

        Dim Comentario As New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGridCompro.Columns.Add(Comentario)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Importe)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Saldo)

        Dim SaldoAnt As New DataColumn("SaldoAnt")
        SaldoAnt.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(SaldoAnt)

        Dim Asignado As New DataColumn("Asignado")
        Asignado.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Asignado)

    End Sub
    Private Sub CalculaTotales()

        TotalConceptos = 0
        For Each Row As DataRow In DtGrid.Rows
            If Row("Cambio") = 0 Then
                TotalConceptos = TotalConceptos + Row("Importe")
            Else : TotalConceptos = TotalConceptos + CalculaNeto(Row("Cambio"), Row("Importe"))
            End If
        Next
        If DtNotaCabeza.Rows(0).Item("Importe") = 0 And PNota <> 0 Then
            TotalConceptos = 0
        End If

        TotalConceptos = TotalConceptos

        TextTotalRecibo.Text = FormatNumber(TotalConceptos, GDecimales)

        TotalRecibos = 0
        For Each Row As DataRow In DtGridCompro.Rows
            If Row.RowState <> DataRowState.Deleted Then
                TotalRecibos = TotalRecibos + Row("Asignado")
            End If
        Next

        TotalRecibos = TotalRecibos

        TextTotalRecibos.Text = FormatNumber(TotalRecibos, GDecimales)

        Select Case PTipoNota    'Esto es para que no modifique el saldo cuando se regraba la nota.(Saldo esta enlazado con Registro cabeza.
            Case 5010
                TextSaldo.Text = FormatNumber(TotalConceptos - TotalRecibos, GDecimales)
            Case 5020
                If PNota = 0 Then TextSaldo.Text = FormatNumber(TotalConceptos, GDecimales)
        End Select

    End Sub
    Private Function HacerAlta(ByVal DtNotaCabezaAux As DataTable, ByVal DtRecibosCabezaAux As DataTable, ByVal DtDevolucionesCabezaAux As DataTable, ByVal DtNotasDBCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroNota As Double
        Dim NumeroW As Double
        Dim NumeroAsiento As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            NumeroNota = UltimaNumeracion(ConexionNota)
            If NumeroNota < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            DtNotaCabezaAux.Rows(0).Item("Movimiento") = NumeroNota
            For Each Row As DataRow In DtNotaDetalleAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Movimiento") = NumeroNota
                End If
            Next
            For Each Row As DataRow In DtMedioPagoAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Movimiento") = NumeroNota
                End If
            Next
            'Halla Ultima numeracion Asiento.
            If DtAsientoCabezaAux.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionNota)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Function
                End If
                DtAsientoCabezaAux.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabezaAux.Rows(0).Item("Documento") = NumeroNota
                For Each Row As DataRow In DtAsientoDetalleAux.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If
            '
            NumeroW = ActualizaNota("A", DtNotaCabezaAux, DtRecibosCabezaAux, DtDevolucionesCabezaAux, DtNotasDBCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux)
            '
            If NumeroW >= 0 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -10 Then
            MsgBox("Factura Ya Fue Impresa. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PNota = NumeroNota
            Return True
        End If

    End Function
    Private Function HacerModificacion(ByVal DtNotaCabezaAux As DataTable, ByVal DtRecibosCabezaAux As DataTable, ByVal DtDevolucionesCabezaAux As DataTable, ByVal DtNotasDBCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable) As Boolean

        Dim NumeroAsiento As Double
        Dim Resul As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Asiento.
            If DtAsientoCabezaAux.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionNota)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Function
                End If
                DtAsientoCabezaAux.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabezaAux.Rows(0).Item("Documento") = DtNotaCabezaAux.Rows(0).Item("Movimiento")
                For Each Row As DataRow In DtAsientoDetalleAux.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If
            '
            Resul = ActualizaNota("M", DtNotaCabezaAux, DtRecibosCabezaAux, DtDevolucionesCabezaAux, DtNotasDBCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux)
            '
            If Resul >= 0 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Return True
        End If

    End Function
    Private Function ActualizaNota(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtRecibosCabezaAux As DataTable, ByVal DtDevolucionesCabezaAux As DataTable, ByVal DtNotasDBCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable) As Double

        Dim Resul As Double

        GModificacionOk = False

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "OtrosPagosCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtNotaDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaDetalleAux.GetChanges, "OtrosPagosDetalle", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Select Case PTipoNota
                    Case 5020
                        If Funcion = "A" Then
                            For I As Integer = 0 To DtMedioPagoAux.Rows.Count - 1
                                Dim Row As DataRow = DtMedioPagoAux.Rows(I)
                                If Row("MedioPago") = 3 Then
                                    Row("ClaveCheque") = ActualizaClavesComprobantes("A", Row("ClaveCheque"), PTipoNota, Row("MedioPago"), 0, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), DtGrid.Rows(I).Item("EmisorCheque"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq"))
                                    If Row("ClaveCheque") <= 0 Then
                                        MsgBox("Cheque " & DtGrid.Rows(I).Item("Serie") & " " & DtGrid.Rows(I).Item("Numero") & " ya fue Emitido.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    End If
                                End If
                            Next
                        End If
                        If Funcion = "B" Then
                            For Each Row As DataRow In DtMedioPagoAux.Rows
                                If Row("MedioPago") = 3 Then
                                    If ActualizaClavesComprobantes("B", Row("ClaveCheque"), PTipoNota, Row("MedioPago"), 0, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                        MsgBox("Cheque " & Row("ClaveCheque") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    End If
                                End If
                            Next
                        End If
                    Case 5010
                        If Funcion = "A" Then
                            For I As Integer = 0 To DtMedioPagoAux.Rows.Count - 1
                                Dim Row As DataRow = DtMedioPagoAux.Rows(I)
                                If Row("MedioPago") = 3 Or Row("MedioPago") = 2 Then
                                    If Row("MedioPago") = 2 Then
                                        Row("ClaveCheque") = ActualizaClavesComprobantes("A", 0, DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq"))
                                        If Row("ClaveCheque") <= 0 Then Return Row("ClaveCheque")
                                    End If
                                    If Row("MedioPago") = 3 Then
                                        If ActualizaClavesComprobantes("A", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), DtGrid.Rows(I).Item("EmisorCheque"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, DtGrid.Rows(I).Item("eCheq")) <= 0 Then
                                            MsgBox("Banco " & Row("Banco") & " Cuenta " & Row("Cuenta") & " Error al Grabar Cheque.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                            Return -3
                                        End If
                                    End If
                                End If
                                If Row("MedioPago") = 14 Then
                                    Row("ClaveCheque") = ActualizaClavesDebito("A", Row("ClaveCheque"), PTipoNota, Row("MedioPago"), Row("Banco"), Row("Cuenta"), Row("Comprobante"), Row("FechaComprobante"), Row("Importe"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota)
                                    If Row("ClaveCheque") <= 0 Then Return Row("ClaveCheque")
                                End If
                            Next
                        End If
                        If Funcion = "B" Then
                            For Each Row As DataRow In DtMedioPagoAux.Rows
                                If Row("MedioPago") = 3 Then
                                    If ActualizaClavesComprobantes("B", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                        MsgBox("Cheque " & Row("ClaveCheque") & " " & Row("Cuenta") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    Else
                                    End If
                                End If
                                If Row("MedioPago") = 2 Then
                                    If ActualizaClavesComprobantes("B", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota, False, False, False) <= 0 Then
                                        MsgBox("Cheque " & Row("Banco") & " " & Row("Cuenta") & " Error de datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    Else
                                    End If
                                End If
                                If Row("MedioPago") = 14 Then
                                    If ActualizaClavesDebito("B", Row("ClaveCheque"), PTipoNota, Row("MedioPago"), Row("Banco"), Row("Cuenta"), Row("Comprobante"), Row("FechaComprobante"), Row("Importe"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionNota) <= 0 Then
                                        MsgBox("Debito Auto.Dife. " & Row("Comprobante") & " " & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    Else
                                    End If
                                End If
                            Next
                        End If
                End Select
                '
                If Not IsNothing(DtMedioPagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "OtrosPagosPago", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtRecibosCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtRecibosCabezaAux.GetChanges, "OtrasFacturasCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtDevolucionesCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtDevolucionesCabezaAux.GetChanges, "OtrosPagosCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtNotasDBCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotasDBCabezaAux.GetChanges, "OtrosPagosCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaAux.GetChanges, "AsientosCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleAux.GetChanges, "AsientosDetalle", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                GModificacionOk = True
                Return DtNotaCabezaAux.Rows(0).Item("Movimiento")
                '
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Sub ActualizaArchivos(ByRef DtNotaCabezaAux As DataTable, ByRef DtMediopagoAux As DataTable)

        'Actualizo MedioPago.
        For Each Row As DataRow In DtGrid.Rows
            Dim Row1 As DataRow = DtMediopagoAux.NewRow
            Row1("Movimiento") = DtNotaCabezaAux.Rows(0).Item("Movimiento")
            Row1("MedioPago") = Row("MedioPago")
            Row1("Detalle") = ""
            Row1("Neto") = 0
            Row1("Alicuota") = 0
            Row1("Cambio") = Row("Cambio")
            Row1("Importe") = Row("Importe")
            Row1("Banco") = Row("Banco")
            Row1("Cuenta") = Row("Cuenta")
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            DtMediopagoAux.Rows.Add(Row1)
        Next

    End Sub
    Private Sub ActualizaComprobantes(ByVal Funcion As String, ByVal DtRecibosCabezaAux As DataTable, ByVal DtDevolucionesCabezaAux As DataTable, ByVal DtNotasDBCabezaAux As DataTable)

        Dim RowsBusqueda() As DataRow

        'Actualiza Saldo de Comprobantes Imputados.

        If Funcion = "M" Then
            For Each Row1 As DataRow In DtGridCompro.Rows
                If Row1.RowState <> DataRowState.Deleted Then
                    If Row1("Saldo") <> Row1("SaldoAnt") Then
                        If Row1("Tipo") = 5000 Then
                            RowsBusqueda = DtRecibosCabezaAux.Select("Factura = " & Row1("Comprobante"))
                            RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                        End If
                        If Row1("Tipo") = 5020 Then
                            RowsBusqueda = DtDevolucionesCabezaAux.Select("Movimiento = " & Row1("Comprobante"))
                            RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                        End If
                        If Row1("Tipo") = 5007 Then
                            RowsBusqueda = DtNotasDBCabezaAux.Select("Movimiento = " & Row1("Comprobante"))
                            RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                        End If
                    End If
                End If
            Next
        End If
        '
        If Funcion = "B" Then
            Dim Importe As Double = 0
            For Each Row1 As DataRow In DtGridCompro.Rows
                If Row1.RowState <> DataRowState.Deleted Then
                    If Row1.Item("Asignado") <> 0 Then
                        If Row1("Tipo") = 5000 Then
                            RowsBusqueda = DtRecibosCabezaAux.Select("Factura = " & Row1("Comprobante"))
                            RowsBusqueda(0).Item("Saldo") = ActualizaSaldo("B", Row1("Tipo"), RowsBusqueda(0).Item("Saldo"), 0, Row1.Item("Asignado"))
                        End If
                        If Row1("Tipo") = 5020 Then
                            RowsBusqueda = DtDevolucionesCabezaAux.Select("Movimiento = " & Row1("Comprobante"))
                            RowsBusqueda(0).Item("Saldo") = ActualizaSaldo("B", Row1("Tipo"), RowsBusqueda(0).Item("Saldo"), 0, Row1.Item("Asignado"))
                        End If
                        If Row1("Tipo") = 5007 Then
                            RowsBusqueda = DtNotasDBCabezaAux.Select("Movimiento = " & Row1("Comprobante"))
                            RowsBusqueda(0).Item("Saldo") = ActualizaSaldo("B", Row1("Tipo"), RowsBusqueda(0).Item("Saldo"), 0, Row1.Item("Asignado"))
                        End If
                    End If
                End If
            Next
        End If

    End Sub
    Private Function ActualizaSaldo(ByVal Funcion As String, ByVal Tipo As Integer, ByVal Saldo As Double, ByVal AsignadoAnt As Double, ByVal Imputado As Double) As Double

        If Funcion = "M" Then Return Trunca(Saldo + AsignadoAnt - Imputado)
        If Funcion = "B" Then Return Trunca(Saldo + Imputado)

    End Function
    Private Function HallaTipo(ByVal MedioPago As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtFormasPago.Select("Clave = " & MedioPago)
        Return RowsBusqueda(0).Item("Tipo")

    End Function
    Private Function HallaSecuenciaCheque(ByVal Banco As Integer, ByVal Cuenta As Double, ByVal UltimoNumero As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGrid.Select("Banco = " & Banco & " AND Cuenta = " & Cuenta)
        Return UltimoNumero + 1 + RowsBusqueda.Length

    End Function
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow
        Dim Item As ItemListaConceptosAsientos

        If Funcion = "A" Then
            Dim ImporteTotal As Double
            For Each Row As DataRow In DtGrid.Rows
                Item = New ItemListaConceptosAsientos
                Item.Clave = Row("MedioPago")
                Item.Importe = Row("Importe")
                ImporteTotal = ImporteTotal + Row("Importe")
                ListaConceptos.Add(Item)
            Next
            Item = New ItemListaConceptosAsientos
            Item.Clave = 213
            Item.Importe = ImporteTotal
            ListaConceptos.Add(Item)
            '
            Item = New ItemListaConceptosAsientos
            Item.Clave = TipoPagoW
            Item.Importe = ImporteTotal
            ListaRetenciones.Add(Item)
        End If
        '
        Dim MontoAfectado As Double
        For Each Row As DataRow In DtGridCompro.Rows
            If Row.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtNotaDetalle.Select("TipoComprobante = " & Row("Tipo") & " AND Comprobante = " & Row("Comprobante"))
                If RowsBusqueda.Length = 0 Then
                    If Row("Asignado") <> 0 Then
                        MontoAfectado = MontoAfectado + Row("Asignado")
                    End If
                Else
                    If Row("Asignado") <> RowsBusqueda(0).Item("Importe") Then
                        MontoAfectado = MontoAfectado + Row("Asignado") - RowsBusqueda(0).Item("Importe")
                    End If
                End If
            End If
        Next
        '
        If MontoAfectado <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = 214
            Item.Importe = MontoAfectado
            ListaConceptos.Add(Item)
        End If

        If MontoAfectado = 0 And Funcion = "M" Then Return True

        If Not Asiento(PTipoNota, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

        Return True

    End Function
    Private Sub Print_Print(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            'Encabezado.
            Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
            Texto = GNombreEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
            Texto = "ORDEN DE PAGO OTROS PROVEEDORES"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 90, MTop)
            ' 
            PrintFont = New Font("Courier New", 12)
            Texto = "Nro. Orden Pago:  " & NumeroEditado(TextComprobante.Text)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
            Texto = "Fecha:  " & DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
            Texto = "Nombre: " & TextProveedor.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 30)
            Texto = "Importe Orden : " & TextTotalRecibo.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 60, MTop + 40)

            Dim Longi As Integer
            Dim Xq As Integer
            Dim Yq As Integer
            Dim RowsBusqueda() As DataRow
            Dim Ancho As Integer
            Dim Alto As Integer

            'Grafica -Rectangulo medios de pagos. ----------------------------------------------------------------------
            x = MIzq
            y = MTop + 50
            Ancho = 180
            Alto = 95
            PrintFont = New Font("Courier New", 10)
            Dim LineaDescripcion As Integer = x + 40
            Dim LineaCambio As Integer = x + 55
            Dim LineaImporte As Integer = x + 85
            Dim LineaBanco As Integer = x + 125
            Dim LineaNumero As Integer = x + 154
            Dim LineaVencimiento As Integer = x + Ancho
            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaDescripcion, y, LineaDescripcion, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCambio, y, LineaCambio, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte, y, LineaImporte, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaBanco, y, LineaBanco, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaNumero, y, LineaNumero, y + Alto)
            'Titulos de descripcion.
            Texto = "DESCRIPCION DEL PAGO"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, y - 4)
            Texto = "DESCRIPCION"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 2, y + 2)
            Texto = "CAMBIO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCambio - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "BANCO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaBanco - Longi - 20
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "NUMERO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaNumero - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "VTO."
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaVencimiento - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)
            '------------------------------------------------------------------------------------------------------------
            'Descripcion del pago.
            Yq = y - SaltoLinea
            For Each Row As DataRow In DtGrid.Rows
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                Yq = Yq + SaltoLinea
                'Imprime Detalle.
                Texto = RowsBusqueda(0).Item("Nombre")
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                If Row("Cambio") <> 0 Then
                    'Imprime Cambio.
                    Texto = FormatNumber(Row("Cambio"), GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaCambio - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Importe.
                    Texto = FormatNumber(CalculaNeto(Row("Cambio"), Row("Importe")), GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Else
                    'Imprime Importe.
                    Texto = FormatNumber(Row("Importe"), GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
                'Imprime Banco.
                Texto = NombreBanco(Row("Banco"))
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte + 1
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Numero.
                If Row("Numero") <> 0 Then
                    Texto = FormatNumber(Row("Numero"), 0)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaNumero - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Vencimeinto.
                    Texto = Format(Row("Fecha"), "dd/MM/yyyy")
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaVencimiento - Longi - 2
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
                'Imprime Comprobante.
                If Row("Comprobante") <> 0 Then
                    Texto = FormatNumber(Row("Comprobante"), 0)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaNumero - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
                'Imprime FechaComprobante.
                If Format(Row("FechaComprobante"), "dd/MM/yyyy") <> "01/01/1800" Then
                    Texto = Format(Row("FechaComprobante"), "dd/MM/yyyy")
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaVencimiento - Longi - 2
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
            Next
            'Grafica -Rectangulo Imputacion. ----------------------------------------------------------------------
            y = MTop + 150
            Ancho = 183
            Alto = 50
            Dim LineaTipo As Integer = x + 35
            Dim LineaComprobante As Integer = x + 69
            Dim LineaFecha As Integer = x + 94
            Dim LineaImporte1 As Integer = x + 125
            Dim LineaSaldo As Integer = x + 155
            Dim LineaImporte2 As Integer = x + Ancho
            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaTipo, y, LineaTipo, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaComprobante, y, LineaComprobante, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaFecha, y, LineaFecha, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte1, y, LineaImporte1, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaSaldo, y, LineaSaldo, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte2, y, LineaImporte2, y + Alto)
            'Titulos de descripcion.
            Texto = "ASIGNACIONES"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, y - 4)
            Texto = "TIPO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 2, y + 2)
            Texto = "COMPROBANTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaComprobante - Longi - 6
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "FECHA"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaFecha - Longi - 5
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte1 - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "SALDO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaSaldo - Longi - 5
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPUTADO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte2 - Longi - 5
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)
            'Descripcion Imputacion.
            Yq = y - SaltoLinea
            For Each Row As DataGridViewRow In GridCompro.Rows
                If Row.Cells("Asignado").Value <> 0 Then
                    Yq = Yq + SaltoLinea
                    'Imprime Tipo.
                    Texto = Row.Cells("Tipo").FormattedValue
                    Xq = x
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Comprobante.
                    Texto = Row.Cells("Comprobante1").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaComprobante - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Fecha.
                    Texto = Row.Cells("FechaCompro").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaFecha - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Importe.
                    Texto = Row.Cells("ImporteCompro").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte1 - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime saldo.
                    Texto = Row.Cells("Saldo").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaSaldo - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Asignado.
                    Texto = Row.Cells("Asignado").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte2 - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
            Next
            'Final.
            Yq = MTop + 215
            PrintFont = New Font("Courier New", 10)
            Texto = "RECIBI LOS VALORES DESCRIPTOS PRECEDENTEMENTE  ........................... "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
            Yq = Yq + SaltoLinea
            Texto = "                                                       F I R M A           "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
            Yq = Yq + 2 * SaltoLinea
            Texto = "ACLARACION APELLIDO Y NOMBRE :............................................."
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
            Yq = Yq + 2 * SaltoLinea
            Texto = "DOCUMENTO  TIPO :...........  Nro.:........................................"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
            Yq = Yq + 3 * SaltoLinea
            Texto = "A U T O R I Z O : ............................."
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
            Paginas = Paginas + 1

            If Paginas < Copias Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            e.HasMorePages = False
        End Try

    End Sub
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Movimiento) FROM OtrosPagosCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function NombreProveedor(ByVal Proveedor As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM OtrosProveedores WHERE Clave = " & Proveedor & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then Return CStr(Ultimo)
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error de Base de Datos.")
            End
        End Try

    End Function
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM OtrosPagosCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return "1/1/1000"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "2/1/1000"
        End Try

    End Function
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTime1.Value, Date.Now) < 0 Then
            MsgBox("Fecha Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If

        If PNota = 0 Then
            Dim UltimaFechaW As Date = UltimaFecha(ConexionNota)
            If UltimaFechaW = "2/1/1000" Then
                MsgBox("Error Base de Datos al Leer Tabla: OtrosPagosCabeza.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
            End If
            If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 Then
                '    MsgBox("Fecha Menor a la Fecha del la Ultima Orden de Pago para esta Caja.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                '    DateTime1.Focus()
                '    Return False
            End If
        End If

        Dim Conta As Integer = 0
        For Each Row As DataRow In DtGridCompro.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row("Asignado") <> 0 Then Conta = Conta + 1
            End If
        Next
        If Conta > GLineasImutacionRecibos Then
            MsgBox("Comprobantes Imputados Supera Permitidos ( " & GLineasImutacionRecibos & " ).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TotalConceptos = 0 Then
            MsgBox("Debe informar Importes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TotalConceptos - TotalRecibos < 0 Then
            MsgBox("Importes Imputados supera importe del Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridCompro_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellContentClick

        If GridCompro.Columns(e.ColumnIndex).Name = "Seleccion" Then
            Dim chkCell As DataGridViewCheckBoxCell = Me.GridCompro.Rows(e.RowIndex).Cells("Seleccion")
            chkCell.Value = Not chkCell.Value
            If chkCell.Value Then
                If GridCompro.Rows(e.RowIndex).Cells("Saldo").Value < 0 Then Exit Sub
                Dim Diferencia As Double = 0
                Diferencia = TotalConceptos - TotalRecibos - GridCompro.Rows(e.RowIndex).Cells("Asignado").Value
                If Diferencia <= 0 Then Exit Sub
                If (GridCompro.Rows(e.RowIndex).Cells("Saldo").Value - Diferencia) <= 0 Then
                    GridCompro.Rows(e.RowIndex).Cells("Asignado").Value = GridCompro.Rows(e.RowIndex).Cells("Saldo").Value
                Else
                    GridCompro.Rows(e.RowIndex).Cells("Asignado").Value = Diferencia
                End If
                CalculaTotales()
            Else
                GridCompro.Rows(e.RowIndex).Cells("Asignado").Value = 0
                CalculaTotales()
            End If
        End If

    End Sub
    Private Sub GridCompro_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEnter

        If Not GridCompro.Columns(e.ColumnIndex).ReadOnly Then
            GridCompro.BeginEdit(True)
            Exit Sub
        End If

    End Sub
    Private Sub GridCompro_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles GridCompro.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If GridCompro.Columns(e.ColumnIndex).Name = "Comprobante1" Then
            e.Value = NumeroEditado(e.Value)
            If PermisoTotal Then
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "FechaCompro" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Asignado" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "ImporteCompro" Or GridCompro.Columns(e.ColumnIndex).Name = "Saldo" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

    End Sub
    Private Sub GridCompro_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles GridCompro.DataError
        Exit Sub
    End Sub
    Private Sub GridCompro_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles GridCompro.EditingControlShowing

        Dim columna As Integer = GridCompro.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPressCompro
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChangedCompro

    End Sub
    Private Sub ValidaKey_KeyPressCompro(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Asignado" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChangedCompro(ByVal sender As Object, ByVal e As System.EventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Asignado" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

    End Sub
    Private Sub GridCompro_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEndEdit

        If GridCompro.Columns(e.ColumnIndex).Name = "Asignado" Then
            If IsDBNull(GridCompro.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then GridCompro.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0
            CalculaTotales()
        End If

    End Sub
    Private Sub DtGridCompro_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        Dim SaldoAnt As Decimal = 0

        If (e.Column.ColumnName.Equals("Asignado")) Then
            If IsDBNull(e.Row("Asignado")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
            '
            SaldoAnt = e.Row("Saldo")
            e.Row("Saldo") = ActualizaSaldo("M", e.Row("Tipo"), e.Row("Saldo"), e.Row("Asignado"), CDec(e.ProposedValue))
            If e.Row("Saldo") < 0 Then
                MsgBox("Imputación Supera el Saldo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Asignado")
                e.Row("Saldo") = SaldoAnt
                Exit Sub
            End If
        End If

        CalculaTotales()

    End Sub
End Class