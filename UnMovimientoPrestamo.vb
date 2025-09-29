Imports System.Drawing.Printing
Imports System.Transactions
Imports System.Math
Public Class UnMovimientoPrestamo
    Public PPrestamo As Double
    Public PMovimiento As Double
    Public PTipoNota As Integer
    Public PAbierto As Boolean
    '''''  Public PTipo As Integer         '6. Cancelacion, 2. Ajuste, 93.Cheque rechazado. 
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim DtPrestamosCabeza As DataTable
    Dim DtNotaCabeza As DataTable
    Dim DtNotaDetalle As DataTable
    Dim DtMedioPago As DataTable
    Dim DtFormasPago As DataTable
    Dim DtConceptos As DataTable
    Dim DtGrid As DataTable
    Dim DtGrid1 As DataTable
    Dim DtRetencionProvincia As DataTable
    Dim DtRetencionProvinciaAux As DataTable
    ' 
    Dim ReciboOficialAnt As Decimal
    Dim cb As ComboBox
    Dim cc As ComboBox
    Dim ConexionPrestamo As String
    Dim TipoAsiento As Integer
    Dim FechaAnt As Date
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Private Sub UnPrestamoMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(801) Then PBloqueaFunciones = True

        Me.Top = 50

        If GCaja = 0 And PPrestamo = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PMovimiento <> 0 And PTipoNota = 1010 Then
            If EsReemplazoChequePrestamo(PMovimiento, PAbierto) Then
                UnChequeReemplazo.PTipoNota = PTipoNota
                UnChequeReemplazo.PNota = PMovimiento
                UnChequeReemplazo.PAbierto = PAbierto
                UnChequeReemplazo.PBloqueaFunciones = PBloqueaFunciones
                UnChequeReemplazo.ShowDialog()
                Me.Close()
                Exit Sub
            End If
        End If

        Grid1.AutoGenerateColumns = False
        Grid1.Columns("Lupa").DefaultCellStyle.NullValue = Nothing
        Grid.AutoGenerateColumns = False
        Grid.Columns("LupaCuenta").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
        Else : PictureCandado.Image = ImageList1.Images.Item("Cerrado")
        End If

        If Not PermisoTotal Then
            PictureCandado.Visible = False
        End If

        If PTipoNota = 1015 Then
            ArmaMedioPagoParaPrestamo(DtFormasPago)
        Else
            ArmaMedioPagoPrestamo(DtFormasPago)
        End If

        Grid.Columns("Detalle").Visible = False
        Grid.Columns("Neto").Visible = False
        Grid.Columns("Alicuota").Visible = False
        Grid.Columns("Iva").Visible = False
        If PTipoNota = 1005 Or PTipoNota = 1007 Then
            Grid1.Columns("Importe1").ReadOnly = True
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        Dim Row As DataRow = ComboEstado.DataSource.NewRow()
        Row("Clave") = 2
        Row("Nombre") = "Suspendido"
        ComboEstado.DataSource.Rows.Add(Row)
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        If PAbierto Then
            ConexionPrestamo = Conexion
        Else : ConexionPrestamo = ConexionN
        End If

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        GModificacionOk = False

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid.EndEdit()
        Grid1.EndEdit()
        MiEnlazador.EndEdit()

        'Poner antes de Valida.
        If ReciboOficialAnt <> DtNotaCabeza.Rows(0).Item("ReciboOficial") Then
            DtNotaCabeza.Rows(0).Item("ReciboOficial") = CDbl(HallaNumeroLetra(TextLetra.Text) & MaskedReciboOficial.Text)
        End If

        If Not Valida() Then Exit Sub

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy

        If PMovimiento = 0 Then ActualizaArchivos(DtNotaDetalleAux, DtMedioPagoAux)

        'Actualiza Archivo de distribucion retenciones por provincia.
        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone
        If PMovimiento = 0 Then
            For Each Row As DataRow In DtRetencionProvinciaAux.Rows
                Dim Row1 As DataRow = DtRetencionProvinciaWW.NewRow
                Row1("TipoNota") = 1010
                Row1("Nota") = 0
                Row1("Provincia") = Row("Provincia")
                Row1("Retencion") = Row("Retencion")
                Row1("Comprobante") = Row("Comprobante")
                Row1("Importe") = Row("Importe")
                DtRetencionProvinciaWW.Rows.Add(Row1)
            Next
        End If

        If IsNothing(DtNotaCabezaAux.GetChanges) And IsNothing(DtNotaDetalleAux.GetChanges) And IsNothing(DtMedioPagoAux.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        '
        If PMovimiento = 0 And GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtNotaDetalleAux, DtMedioPagoAux, DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If PMovimiento <> 0 And DiferenciaDias(DateTime1.Value, FechaAnt) <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & TipoAsiento & " AND Documento = " & PMovimiento & ";", ConexionPrestamo, DtAsientoCabeza) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default : MsgBox("No se pudo Leer Tabla AsientosCabeza.") : Exit Sub
            End If
            If DtAsientoCabeza.Rows.Count <> 0 Then
                DtAsientoCabeza.Rows(0).Item("Intfecha") = Format(DtNotaCabezaAux.Rows(0).Item("Fecha"), "yyyyMMdd")
            End If
        End If

        If PMovimiento = 0 Then
            If HacerAlta(DtNotaCabezaAux, DtNotaDetalleAux, DtMedioPagoAux, DtAsientoCabeza, DtAsientoDetalle, DtRetencionProvinciaWW) Then
                ArmaArchivos()
            End If
        Else
            Dim resul As Integer = ActualizaMovi("M", DtNotaCabezaAux, DtNotaDetalleAux, DtMedioPagoAux, ConexionPrestamo, DtAsientoCabeza, DtAsientoDetalle, DtRetencionProvinciaWW)
            If resul < 0 And resul <> -3 Then
                MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            End If
            If resul = 0 Then
                MsgBox("Otro usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            End If
            If resul > 0 Then
                MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                GModificacionOk = True
                ArmaArchivos()
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBaja_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBaja.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If GCaja = 0 And PPrestamo = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PMovimiento = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Cancelación esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtNotaCabeza.Rows(0).Item("ChequeRechazado") <> 0 Or DtNotaCabeza.Rows(0).Item("ClaveChequeReemplazado") <> 0 Then
            MsgBox("Anulación Por Cheque Rechazado o Reemplazado, Debe Realizarce Por Menu Rechazo de Cheques. Operación se CANCELA.")
            Exit Sub
        End If

        If CInt(LabelCaja.Text) <> GCaja And Not GAdministrador Then
            MsgBox("Modificación Solo Permitida para Caja " & LabelCaja.Text & " o un Administrador. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy

        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Resul As Double

        For Each Row As DataGridViewRow In Grid.Rows
            If IsDBNull(Row.Cells("Concepto").Value) Then Exit For
            If Row.Cells("Concepto").Value = 2 Or Row.Cells("Concepto").Value = 3 Then
                Dim Anulado As Boolean, Rechazado As Boolean, Depositado As Boolean, Entregado As Boolean, Afectado As Boolean
                If Not EstadoCheque(Row.Cells("Concepto").Value, Row.Cells("ClaveCheque").Value, ConexionPrestamo, Anulado, Rechazado, Depositado, Entregado, Afectado) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Error Base de Datos.")
                    Exit Sub
                End If
                If Anulado Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Cheque " & Row.Cells("Numero").Value & " no se puede Borrar. Cheque Anulado. Operación se CANCELA.")
                    Exit Sub
                End If
                If Rechazado Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Cheque " & Row.Cells("Numero").Value & " no se puede Borrar. Existe Nota de Rechazo. Operación se CANCELA.")
                    Exit Sub
                End If
                If Row.Cells("Concepto").Value = 2 Then
                    If Depositado Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row.Cells("Numero").Value & " no se puede Borrar. Fue Depositado. Operación se CANCELA.")
                        Exit Sub
                    End If
                End If
            End If
            If Row.Cells("Concepto").Value = 3 Then
                If ExiteChequeEnPaseCaja(ConexionPrestamo, Row.Cells("ClaveCheque").Value) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Cheque " & Row.Cells("Numero").Value & " en Proceso de Pase de Caja. Operación se CANCELA.")
                    Exit Sub
                End If
            End If
        Next

        If PTipoNota = 1010 Then
            For Each Row As DataGridViewRow In Grid.Rows
                If IsDBNull(Row.Cells("Concepto").Value) Then Exit For
                If Row.Cells("Concepto").Value = 2 Or Row.Cells("Concepto").Value = 3 Then
                    If ChequeReemplazado(Row.Cells("Concepto").Value, Row.Cells("ClaveCheque").Value, PTipoNota, PMovimiento, ConexionPrestamo) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row.Cells("Numero").Value & " no se puede Borrar, fue Reemplazado. Operación se CANCELA.")
                        Exit Sub
                    End If
                End If
            Next
        End If

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        '
        If Not HallaAsientosCabeza(TipoAsiento, PMovimiento, DtAsientoCabeza, ConexionPrestamo) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If
        For Each Row As DataRow In DtAsientoCabeza.Rows
            Row("Estado") = 3
        Next

        If MsgBox("Movimiento se Dara de Baja. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtNotaCabezaAux.Rows(0).Item("Estado") = 3

        Resul = ActualizaMovi("B", DtNotaCabezaAux, DtNotaDetalleAux, DtMedioPagoAux, ConexionPrestamo, DtAsientoCabeza, DtAsientoDetalle, DtRetencionProvinciaWW)
        If Resul < 0 And Resul <> -3 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Movimiento Fue Dado de Baja Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PMovimiento = 0 Then
            MsgBox("Opcion Invalida. Movimiento debe ser Grabado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ErrorImpresion = False
        Paginas = 0
        If Not PAbierto Then Copias = 1
        If PAbierto Then Copias = 2

        Dim print_document As New PrintDocument
        AddHandler print_document.PrintPage, AddressOf Print_PrintOrdenPago

        print_document.Print()

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PMovimiento = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = TipoAsiento
        If PAbierto Then
            ListaAsientos.PDocumentoB = PMovimiento
        Else
            ListaAsientos.PDocumentoN = PMovimiento
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then Exit Sub

        Dim Row As DataRow
        Row = DtGrid.Rows.Item(Grid.CurrentRow.Index)

        Row.Delete()

        CalculaTotales()

    End Sub
    Private Sub ButtonEliminarLineaConcepto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLineaConcepto.Click

        If Grid1.Rows.Count = 1 Then Exit Sub

        Dim Row As DataRow
        Row = DtGrid1.Rows.Item(Grid1.CurrentRow.Index)
        Dim Concepto As Integer = Row("Concepto")
        Row.Delete()

        For I As Integer = DtRetencionProvinciaAux.Rows.Count - 1 To 0 Step -1
            Row = DtRetencionProvinciaAux.Rows(I)
            If Row("Retencion") = Concepto Then Row.Delete()
        Next

        CalculaTotales()

    End Sub
    Private Sub ButtonEliminarLineaGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLineaGrid.Click

        If Grid.Rows.Count = 1 Then Exit Sub

        Dim Row As DataRow
        Row = DtGrid.Rows.Item(Grid.CurrentRow.Index)
        Row.Delete()

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()
        CreaDtGrid1()
        CreaDtRetencionProvinciaAux()

        Dim Sql As String
        Dim RowsBusqueda() As DataRow

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM PrestamosMovimientoCabeza WHERE Movimiento = " & PMovimiento & ";"
        If Not Tablas.Read(Sql, ConexionPrestamo, DtNotaCabeza) Then Return False
        If PMovimiento <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            MsgBox("Movimiento No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PMovimiento = 0 Then
            Dim Row As DataRow = DtNotaCabeza.NewRow
            ArmaNuevoMovimientoPrestamo(Row)
            Row("Prestamo") = PPrestamo
            Row("TipoNota") = PTipoNota
            Row("Fecha") = DateTime1.Value
            Row("Caja") = GCaja
            Row("Estado") = 1
            DtNotaCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtPrestamosCabeza = New DataTable
        Sql = "SELECT * FROM PrestamosCabeza WHERE Prestamo = " & DtNotaCabeza.Rows(0).Item("Prestamo") & ";"
        If Not Tablas.Read(Sql, ConexionPrestamo, DtPrestamosCabeza) Then Return False
        If DtPrestamosCabeza.Rows.Count = 0 Then
            MsgBox("Prestamo No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If DtPrestamosCabeza.Rows(0).Item("Origen") = 1 Then TextEmisor.Text = "Bco. " & NombreBanco(DtPrestamosCabeza.Rows(0).Item("Emisor"))
        If DtPrestamosCabeza.Rows(0).Item("Origen") = 2 Then TextEmisor.Text = NombreProveedor(DtPrestamosCabeza.Rows(0).Item("Emisor"))
        If DtPrestamosCabeza.Rows(0).Item("Origen") = 3 Then TextEmisor.Text = NombreCliente(DtPrestamosCabeza.Rows(0).Item("Emisor"))

        Dim CapitalAjustado As Double
        Dim Cancelado As Double
        Dim InteresCancelado As Double
        Dim Gastos As Double
        Dim DtMovimientos As DataTable

        If Not ProcesaPrestamo(False, CapitalAjustado, Cancelado, InteresCancelado, Gastos, DtMovimientos, DtNotaCabeza.Rows(0).Item("Prestamo"), DtPrestamosCabeza.Rows(0).Item("Capital"), DtPrestamosCabeza.Rows(0).Item("Fecha"), DtPrestamosCabeza.Rows(0).Item("Estado"), ConexionPrestamo) Then Return False

        TextCancelado.Text = FormatNumber(Cancelado, GDecimales)

        TextCapital.Text = FormatNumber(CapitalAjustado, GDecimales)
        TextInteres.Text = FormatNumber(DtPrestamosCabeza.Rows(0).Item("Interes"), GDecimales)
        TextCuotas.Text = FormatNumber(DtPrestamosCabeza.Rows(0).Item("Cuotas"), 0)

        DtRetencionProvincia = New DataTable
        If Not Tablas.Read("SELECT * FROM RecibosRetenciones WHERE TipoNota = " & 1010 & " AND Nota = " & PMovimiento & ";", Conexion, DtRetencionProvincia) Then Return False
        For Each Row As DataRow In DtRetencionProvincia.Rows
            Dim Row1 As DataRow = DtRetencionProvinciaAux.NewRow
            Row1("Retencion") = Row("Retencion")
            Row1("Provincia") = Row("Provincia")
            Row1("Comprobante") = Row("Comprobante")
            Row1("Importe") = Row("Importe")
            DtRetencionProvinciaAux.Rows.Add(Row1)
        Next

        DtNotaDetalle = New DataTable
        Sql = "SELECT * FROM PrestamosMovimientoDetalle WHERE Movimiento = " & PMovimiento & ";"
        If Not Tablas.Read(Sql, ConexionPrestamo, DtNotaDetalle) Then Return False

        For Each Row As DataRow In DtNotaDetalle.Rows
            Dim Row1 As DataRow = DtGrid1.NewRow
            Row1("Item") = Row("Item")
            Row1("Movimiento") = Row("Movimiento")
            Row1("Concepto") = Row("Concepto")
            Row1("Importe") = Row("Importe")
            Row1("TieneLupa") = False
            RowsBusqueda = DtRetencionProvincia.Select("Retencion = " & Row("Concepto"))
            If RowsBusqueda.Length <> 0 Then Row1("TieneLupa") = True
            DtGrid1.Rows.Add(Row1)
        Next

        PTipoNota = DtNotaCabeza.Rows(0).Item("TipoNota")

        LlenaCombosGrid()

        Panel4.Visible = False
        If PTipoNota = 1010 Then Me.Text = "Cancelación Capital,Intereses,Gastos" : TipoAsiento = 8001 : Panel4.Visible = True
        If PTipoNota = 1015 Then Me.Text = "Ajuste de Capital" : TipoAsiento = 8002

        Grid1.DataSource = DtGrid1
        Grid1.EndEdit()

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM PrestamosMovimientoPago WHERE Movimiento = " & PMovimiento & ";"
        If Not Tablas.Read(Sql, ConexionPrestamo, DtMedioPago) Then Return False

        Dim DtCheques As New DataTable
        For Each Row As DataRow In DtMedioPago.Rows
            If Row("ClaveCheque") <> 0 Then
                If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionPrestamo, DtCheques) Then Return False
            End If
        Next

        For Each Row As DataRow In DtMedioPago.Rows
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Item") = Row("Item")
            Row1("MedioPago") = Row("MedioPago")
            Row1("Importe") = Row("Importe")
            Row1("Cambio") = Row("Cambio")
            Row1("Bultos") = 0
            Row1("Detalle") = 0
            Row1("Alicuota") = 0
            Row1("Neto") = 0
            Row1("ImporteIva") = 0
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("NumeracionInicial") = 1
            Row1("NumeracionFinal") = 999999999
            Row1("ClaveCheque") = Row("ClaveCheque")
            Row1("ClaveChequeVisual") = Row("ClaveCheque")
            If Row("MedioPago") = 2 Then Row1("ClaveChequeVisual") = 0
            If Row("ClaveCheque") <> 0 Then
                RowsBusqueda = DtCheques.Select("MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque"))
                If RowsBusqueda.Length <> 0 Then
                    Row1("Banco") = RowsBusqueda(0).Item("Banco")
                    Row1("Cuenta") = RowsBusqueda(0).Item("Cuenta")
                    Row1("Serie") = RowsBusqueda(0).Item("Serie")
                    Row1("Numero") = RowsBusqueda(0).Item("Numero")
                    Row1("EmisorCheque") = RowsBusqueda(0).Item("EmisorCheque")
                    Row1("Fecha") = RowsBusqueda(0).Item("Fecha")
                    Row1("eCheq") = RowsBusqueda(0).Item("eCheq")               'eCheq.
                Else
                    Row1("Banco") = 0
                    Row1("Cuenta") = 0
                    Row1("Serie") = ""
                    Row1("Numero") = 0
                    Row1("EmisorCheque") = "Comp.No Existe."
                    Row1("Fecha") = "1/1/1800"
                    Row1("eCheq") = 0                             'eCheq.
                End If
            Else
                Row1("Banco") = Row("Banco")
                Row1("Cuenta") = Row("Cuenta")
                Row1("Serie") = ""
                Row1("Numero") = 0
                Row1("EmisorCheque") = ""
                Row1("Fecha") = "1/1/1800"
            End If
            DtGrid.Rows.Add(Row1)
        Next

        DtCheques.Dispose()

        Grid.DataSource = DtGrid
        Grid.EndEdit()

        '       If PTipoNota = 1015 And PMovimiento = 0 Then
        '     Dim Row As DataRow = DtGrid1.NewRow
        '      Row("Movimiento") = 0
        '      Row("Concepto") = 2
        '      Row("Item") = 0
        '      Row("Importe") = 0
        '      DtGrid1.Rows.Add(Row)
        '      Grid1.Columns("Concepto1").ReadOnly = True
        '      Grid1.AllowUserToAddRows = False
        '  End If

        'Precenta las lineas del grid.
        PresentaLineasGrid()

        If PMovimiento = 0 Then
            Grid.ReadOnly = False
            Grid1.ReadOnly = False
            ButtonEliminarLineaConcepto.Enabled = True
            ButtonEliminarLinea.Enabled = True
        Else
            Grid.ReadOnly = True
            Grid1.ReadOnly = True
            ButtonEliminarLineaConcepto.Enabled = False
            ButtonEliminarLinea.Enabled = False
        End If

        CalculaTotales()

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)
        AddHandler DtGrid.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid_RowChanged)
        AddHandler DtGrid1.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid1_ColumnChanging)
        AddHandler DtGrid1.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid1_NewRow)
        AddHandler DtGrid1.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid1_RowChanged)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtNotaCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Prestamo")
        AddHandler Enlace.Format, AddressOf FormatPrestamo
        TextPrestamo.DataBindings.Clear()
        TextPrestamo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Movimiento")
        AddHandler Enlace.Format, AddressOf Formatmovimiento
        TextMovimiento.DataBindings.Clear()
        TextMovimiento.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextImporte.DataBindings.Clear()
        TextImporte.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)
        FechaAnt = DateTime1.Value

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "ReciboOficial")
        AddHandler Enlace.Format, AddressOf FormatReciboOficial
        MaskedReciboOficial.DataBindings.Clear()
        MaskedReciboOficial.DataBindings.Add(Enlace)
        ReciboOficialAnt = Val(MaskedReciboOficial.Text)

    End Sub
    Private Sub FormatPrestamo(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = Format(Numero.Value, "0000-00000000")
        End If

    End Sub
    Private Sub Formatmovimiento(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub FormatCuenta(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub ParseCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatReciboOficial(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value <> 0 Then
            TextLetra.Text = LetraTipoIva(Val(Strings.Left(Numero.Value.ToString, 1)))
            Numero.Value = Format(Val(Strings.Right(Numero.Value.ToString, 12)), "000000000000")
        Else
            Numero.Value = Format(Numero.Value, "000000000000")
        End If

    End Sub
    Private Sub ActualizaArchivos(ByRef DtNotaDetalleAux As DataTable, ByRef DtMedioPagoAux As DataTable)

        'Actualizo Conceptos.
        For Each Row As DataRow In DtGrid1.Rows
            Dim Row1 As DataRow = DtNotaDetalleAux.NewRow
            Row1("Movimiento") = PMovimiento
            Row1("Concepto") = Row("Concepto")
            Row1("Importe") = Row("Importe")
            DtNotaDetalleAux.Rows.Add(Row1)
        Next

        'Actualizo MedioPago.
        For Each Row As DataRow In DtGrid.Rows
            Dim Row1 As DataRow = DtMedioPagoAux.NewRow
            Row1("Movimiento") = PMovimiento
            Row1("Item") = 0
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
            DtMedioPagoAux.Rows.Add(Row1)
        Next

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = CreaDtParaGrid()

    End Sub
    Private Sub CreaDtGrid1()

        DtGrid1 = New DataTable

        Dim Item As New DataColumn("Item")
        Item.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Item)

        Dim Movimiento As New DataColumn("Movimiento")
        Movimiento.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Movimiento)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Concepto)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid1.Columns.Add(Importe)

        Dim TieneLupa As New DataColumn("TieneLupa")
        TieneLupa.DataType = System.Type.GetType("System.Boolean")
        DtGrid1.Columns.Add(TieneLupa)

    End Sub
    Private Sub CreaDtRetencionProvinciaAux()

        DtRetencionProvinciaAux = New DataTable

        Dim Retencion As New DataColumn("Retencion")
        Retencion.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Retencion)

        Dim Provincia As New DataColumn("Provincia")
        Provincia.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Provincia)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Comprobante)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtRetencionProvinciaAux.Columns.Add(Importe)

    End Sub
    Private Function HacerAlta(ByVal DtNotaCabezaAux As DataTable, ByVal DtCancelacionDetalleAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtRetencionProvinciaWW As DataTable) As Boolean

        Dim NumeroMovi As Double
        Dim Resul As Double
        Dim NumeroAsiento As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            NumeroMovi = UltimaNumeracion(ConexionPrestamo)
            If NumeroMovi < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            DtNotaCabezaAux.Rows(0).Item("Movimiento") = NumeroMovi
            For Each Row As DataRow In DtCancelacionDetalleAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Movimiento") = NumeroMovi
                End If
            Next
            For Each Row As DataRow In DtMedioPagoAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Movimiento") = NumeroMovi
                End If
            Next

            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionPrestamo)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = NumeroMovi
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            For Each Row As DataRow In DtRetencionProvinciaWW.Rows
                Row("Nota") = NumeroMovi
            Next

            Resul = ActualizaMovi("A", DtNotaCabezaAux, DtCancelacionDetalleAux, DtMedioPagoAux, ConexionPrestamo, DtAsientoCabeza, DtAsientoDetalle, DtRetencionProvinciaWW)

            If Resul >= 0 Then Exit For
            If Resul = -3 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = -3 Then
            Return False
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PMovimiento = NumeroMovi
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function ActualizaMovi(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal ConexionStr As String, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtRetencionProvinciaWW As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "PrestamosMovimientoCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtNotaDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaDetalleAux.GetChanges, "PrestamosMovimientoDetalle", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If PTipoNota = 1015 Then
                    If Funcion = "A" Then
                        For I As Integer = 0 To DtMedioPagoAux.Rows.Count - 1
                            Dim Row As DataRow = DtMedioPagoAux.Rows(I)
                            If Row("MedioPago") = 3 Then
                                Row("ClaveCheque") = ActualizaClavesComprobantes("A", Row("ClaveCheque"), 1015, Row("MedioPago"), 0, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), DtGrid.Rows(I).Item("EmisorCheque"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionStr, False, False, DtGrid.Rows(I).Item("eCheq"))
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
                                If ActualizaClavesComprobantes("B", Row("ClaveCheque"), 1015, Row("MedioPago"), 0, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionStr, False, False, False) <= 0 Then
                                    MsgBox("Cheque " & Row("ClaveCheque") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                    Return -3
                                End If
                            End If
                        Next
                    End If
                Else
                    If Funcion = "A" Then
                        For I As Integer = 0 To DtMedioPagoAux.Rows.Count - 1
                            Dim Row As DataRow = DtMedioPagoAux.Rows(I)
                            If Row("MedioPago") = 3 Or Row("MedioPago") = 2 Then
                                If Row("MedioPago") = 2 Then
                                    Row("ClaveCheque") = ActualizaClavesComprobantes("A", 0, DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionStr, False, False, DtGrid.Rows(I).Item("eCheq"))
                                    If Row("ClaveCheque") <= 0 Then Return Row("ClaveCheque")
                                End If
                                If Row("MedioPago") = 3 Then
                                    If ActualizaClavesComprobantes("A", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), DtGrid.Rows(I).Item("Serie"), DtGrid.Rows(I).Item("Numero"), DtGrid.Rows(I).Item("Fecha"), Row("Importe"), DtGrid.Rows(I).Item("EmisorCheque"), DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionStr, False, False, DtGrid.Rows(I).Item("eCheq")) <= 0 Then
                                        MsgBox("Banco " & Row("Banco") & " Cuenta " & Row("Cuenta") & " Error al Grabar Cheque.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                        Return -3
                                    End If
                                End If
                            End If
                        Next
                    End If
                    If Funcion = "B" Then
                        For Each Row As DataRow In DtMedioPagoAux.Rows
                            If Row("MedioPago") = 3 Then
                                If ActualizaClavesComprobantes("B", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionStr, False, False, False) <= 0 Then
                                    MsgBox("Cheque " & Row("ClaveCheque") & " " & Row("Cuenta") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                    Return -3
                                Else
                                End If
                            End If
                            If Row("MedioPago") = 2 Then
                                If ActualizaClavesComprobantes("B", Row("ClaveCheque"), DtNotaCabezaAux.Rows(0).Item("TipoNota"), Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtNotaCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionStr, False, False, False) <= 0 Then
                                    MsgBox("Cheque " & Row("Banco") & " " & Row("Cuenta") & " Error de datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                    Return -3
                                Else
                                End If
                            End If
                        Next
                    End If
                End If
                '
                If Not IsNothing(DtMedioPagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "PrestamosMovimientoPago", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtRetencionProvinciaWW.GetChanges) Then
                    Resul = GrabaTabla(DtRetencionProvinciaWW.GetChanges, "RecibosRetenciones", ConexionStr)
                    If Resul <= 0 Then Return 0
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Public Function UltimaNumeracion(ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Movimiento) FROM PrestamosMovimientoCabeza;", Miconexion)
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
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        DtConceptos = New DataTable
        If PTipoNota = 1010 Then DtConceptos = ArmaConceptosParaCancelacion(PAbierto)
        If PTipoNota = 1015 Then DtConceptos = ArmaConceptosParaAjuste()

        Concepto1.DataSource = DtConceptos
        Concepto1.DisplayMember = "Nombre"
        Concepto1.ValueMember = "Clave"

        Concepto.DataSource = DtFormasPago
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

        Banco.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 order By Nombre;")
        Row = Banco.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Banco.DataSource.Rows.Add(Row)
        Banco.DisplayMember = "Nombre"
        Banco.ValueMember = "Clave"

    End Sub
    Private Function HallaTipo(ByVal MedioPago As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtFormasPago.Select("Clave = " & MedioPago)
        Return RowsBusqueda(0).Item("Tipo")

    End Function
    Private Sub CalculaTotales()

        Dim Total As Double = 0
        Dim Grabado As Decimal = 0
        Dim BaseImponible As Decimal = 0
        Dim TotalIva As Decimal = 0
        Dim RowsBusqueda() As DataRow

        bs.EndEdit()

        For Each Row As DataGridViewRow In Grid1.Rows
            Total = Total + Row.Cells("Importe1").Value
        Next
        TextImporte.Text = FormatNumber(Total, GDecimales)

        Total = 0
        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Cambio").Value = 0 Then
                Total = Total + Row.Cells("Importe").Value
            Else : Total = Total + Row.Cells("Cambio").Value * Row.Cells("Importe").Value
            End If
        Next

        'poner despues de lo anterior.
        For Each Row As DataRow In DtGrid1.Rows
            If Row("Concepto") <> 0 Then
                RowsBusqueda = DtConceptos.Select("Clave = " & Row("Concepto"))
                If RowsBusqueda(0).Item("Activo2") Then
                    Grabado = Grabado + Row("Importe")
                Else
                    If RowsBusqueda(0).Item("Tipo") = 22 Then
                        BaseImponible = BaseImponible + Trunca(Row("Importe") * 100 / RowsBusqueda(0).Item("Alicuota"))
                        TotalIva = TotalIva + Row("Importe")
                    End If
                End If
            End If
        Next

        TextTotalMedioPago.Text = FormatNumber(Total, GDecimales)
        TextGrabado.Text = FormatNumber(Grabado, GDecimales)
        TextBaseImponible.Text = FormatNumber(BaseImponible, GDecimales)
        TextTotalIva.Text = FormatNumber(TotalIva, GDecimales)

    End Sub
    Private Function HallaSecuenciaCheque(ByVal Banco As Integer, ByVal Cuenta As Double, ByVal UltimoNumero As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGrid.Select("Banco = " & Banco & " AND Cuenta = " & Cuenta & " AND Item = 0")

        Return UltimoNumero + 1 + RowsBusqueda.Length

    End Function
    Public Function ArmaConceptosParaCancelacion(ByVal Abierto As Boolean) As DataTable

        Dim dt As New DataTable

        Try
            dt = New DataTable
            dt.Columns.Add("Clave", Type.GetType("System.Int32"))
            dt.Columns.Add("Nombre", Type.GetType("System.String"))
            dt.Columns.Add("Tipo", Type.GetType("System.Int32"))
            dt.Columns.Add("Activo2", Type.GetType("System.Boolean"))
            dt.Columns.Add("Alicuota", Type.GetType("System.Decimal"))

            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = " "
            Row("Tipo") = 0
            Row("Alicuota") = 0
            Row("Activo2") = False
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 6
            Row("Nombre") = "Capital A Cancelar"
            Row("Tipo") = 0
            Row("Alicuota") = 0
            Row("Activo2") = False
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 7
            Row("Nombre") = "Intereses"
            Row("Tipo") = 0
            Row("Alicuota") = 0
            Row("Activo2") = True
            dt.Rows.Add(Row)
            '
            If Not Tablas.Read("SELECT Clave,Nombre,Tipo,Activo2,0 AS Alicuota FROM Tablas Where Tipo = 32;", Conexion, dt) Then End
            '
            If Abierto Then
                If Not Tablas.Read("SELECT Clave,Nombre,Tipo,0 AS Activo2,Iva AS Alicuota FROM Tablas Where Tipo = 22;", Conexion, dt) Then End
                If PMovimiento = 0 Then
                    If Not Tablas.Read("SELECT T.Clave,T.Nombre,4 AS Tipo,0 AS Activo2,0 AS Alicuota FROM Tablas AS T INNER JOIN DocuRetenciones AS D ON T.Clave = D.Clave Where T.Tipo = 25 AND D.TipoDocumento = 1010;", Conexion, dt) Then End
                Else
                    If Not Tablas.Read("SELECT Clave,Nombre,4 AS Tipo,0 AS Activo2,0 AS Alicuota FROM Tablas Where Tipo = 25;", Conexion, dt) Then End
                End If
            End If
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ArmaConceptosParaAjuste() As DataTable

        Dim dt As New DataTable

        Try
            dt = New DataTable
            dt.Columns.Add("Clave", Type.GetType("System.Int32"))
            dt.Columns.Add("Nombre", Type.GetType("System.String"))
            dt.Columns.Add("Tipo", Type.GetType("System.Int32"))
            dt.Columns.Add("Activo2", Type.GetType("System.Boolean"))
            dt.Columns.Add("Alicuota", Type.GetType("System.Decimal"))
            '
            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = " "
            Row("Tipo") = 0
            Row("Alicuota") = 0
            Row("Activo2") = True
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 2
            Row("Nombre") = "Ajuste Capital"
            Row("Tipo") = 0
            Row("Alicuota") = 0
            Row("Activo2") = True
            dt.Rows.Add(Row)
            '
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Private Sub IngresaCheques(ByVal ListaDeChequesAux As List(Of ItemCheque))

        Grid.DataSource = Nothing
        Dim Dt As DataTable = DtGrid.Copy
        DtGrid.Clear()

        For Each Row As DataRow In Dt.Rows
            If Row("Item") <> 0 Then
                DtGrid.ImportRow(Row)
            End If
            If Row("Item") = 0 And Row("MedioPago") <> ListaDeChequesAux(0).MedioPago Then
                DtGrid.ImportRow(Row)
            End If
        Next
        For Each Item As ItemCheque In ListaDeChequesAux
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Item") = 0
            Row1("MedioPago") = Item.MedioPago
            Row1("eCheq") = Item.echeq                     'eCheq.
            Row1("Detalle") = ""
            Row1("Neto") = 0
            Row1("Alicuota") = 0
            Row1("ImporteIva") = 0
            Row1("Banco") = Item.Banco
            Row1("Fecha") = Item.Fecha
            Row1("Cuenta") = Item.Cuenta
            Row1("Serie") = Item.Serie
            Row1("Numero") = Item.Numero
            Row1("EmisorCheque") = Item.EmisorCheque
            Row1("Cambio") = 0
            Row1("Importe") = Item.Importe
            Row1("Comprobante") = 0
            Row1("FechaComprobante") = "1/1/1800"
            Row1("ClaveCheque") = Item.ClaveCheque
            If Item.MedioPago = 2 Then
                Row1("ClaveChequeVisual") = 0
            Else : Row1("ClaveChequeVisual") = Item.ClaveCheque
            End If
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = DtGrid
        PresentaLineasGrid()
        Grid.EndEdit()

        Dt.Dispose()

    End Sub
    Private Sub PresentaLineasGrid()

        'Precenta las lineas del grid.
        Dim RowsBusqueda() As DataRow

        For Each Row As DataGridViewRow In Grid.Rows
            If Not IsNothing(Row.Cells("Concepto").Value) Then
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row.Cells("Concepto").Value)
                ArmaGridSegunConcepto(Row, RowsBusqueda(0).Item("Tipo"), 600, False, False, PAbierto)
            End If
        Next

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtDetalleAux As DataTable, ByVal DtDetallePagoAux As DataTable, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos
        Dim RowsBusqueda() As DataRow

        'Arma Medios pago.
        For Each Row As DataRow In DtDetallePagoAux.Rows
            Item = New ItemListaConceptosAsientos
            Item.Clave = Row("MedioPago")
            Item.Importe = Row("Importe")
            If Row("Cambio") <> 0 Then Item.Importe = Trunca(Item.Importe * Row("Cambio"))
            ListaConceptos.Add(Item)
        Next
        '
        'Arma conceptos de cancelacion exepto Los Ivas y Retenciones.
        For Each Row As DataRow In DtDetalleAux.Rows
            RowsBusqueda = DtConceptos.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda(0).Item("Tipo") <> 22 And RowsBusqueda(0).Item("Tipo") <> 4 Then
                Item = New ItemListaConceptosAsientos
                If RowsBusqueda(0).Item("Tipo") = 0 Then
                    Item.Clave = -Row("Concepto")
                Else
                    Item.Clave = Row("Concepto")
                End If
                Item.Importe = Row("Importe")
                ListaConceptos.Add(Item)
            End If
        Next
        '
        'Arma lista de iva.
        For Each Row As DataRow In DtDetalleAux.Rows
            RowsBusqueda = DtConceptos.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda(0).Item("Tipo") = 22 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = Row("Concepto")
                Item.Importe = Row("Importe")
                Item.TipoIva = 5
                ListaIVA.Add(Item)
            End If
        Next
        '
        'Arma lista de Retencion.
        For Each Row As DataRow In DtGrid1.Rows
            RowsBusqueda = DtConceptos.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda(0).Item("Tipo") = 4 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = Row("Concepto")
                Item.Importe = Row("Importe")
                Item.TipoIva = 11
                ListaRetenciones.Add(Item)
            End If
        Next
        '
        If Not Asiento(TipoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

        Return True

    End Function
    Private Sub Print_PrintOrdenPago(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.


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
            Texto = "ORDEN DE PAGO PRESTAMO"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 115, MTop)
            ' 
            PrintFont = New Font("Courier New", 12)
            Texto = "Nro. Orden Pago:  " & NumeroEditado(TextMovimiento.Text)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
            Texto = "Fecha:  " & DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
            Texto = "Emisor: " & TextEmisor.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 30)
            Texto = "Importe Orden : " & TextImporte.Text
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
            For Each Row As DataGridViewRow In Grid.Rows
                If IsNothing(Row.Cells("Concepto").Value) Then Exit For
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row.Cells("Concepto").Value)
                Yq = Yq + SaltoLinea
                'Imprime Detalle.
                Texto = Row.Cells("Concepto").FormattedValue
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                If Row.Cells("Cambio").Value <> 0 Then
                    'Imprime Cambio.
                    Texto = Row.Cells("Cambio").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaCambio - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Importe.
                    Texto = FormatNumber(Trunca(Row.Cells("Cambio").Value * Row.Cells("Importe").Value), GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Else
                    'Imprime Importe.
                    Texto = Row.Cells("Importe").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
                'Imprime Banco.
                Texto = Row.Cells("Banco").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte + 1
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Numero.
                Texto = Row.Cells("Numero").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaNumero - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Vencimeinto.
                Texto = Row.Cells("Fecha").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaVencimiento - Longi - 2
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Comprobante.
                If Row.Cells("Comprobante").FormattedValue <> "" Then
                    Texto = Row.Cells("Comprobante").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaNumero - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
                'Imprime FechaComprobante.
                If Row.Cells("FechaComprobante").FormattedValue <> "" Then
                    Texto = Row.Cells("FechaComprobante").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaVencimiento - Longi - 2
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
            Next
            'Grafica -Rectangulo Conceptos. ----------------------------------------------------------------------
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
            Texto = "CONCEPTOS PAGADOS"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, y - 4)
            Texto = "CONCEPTO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 2, y + 2)
            Xq = LineaFecha - Longi - 5
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)
            'Descripcion Imputacion.
            Yq = y - SaltoLinea
            For Each Row As DataGridViewRow In Grid1.Rows
                If IsNothing(Row.Cells("Concepto1").Value) Then Exit For
                Yq = Yq + SaltoLinea
                'Imprime Concepto.
                Texto = Row.Cells("Concepto1").FormattedValue
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Importe.
                Texto = Row.Cells("Importe1").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte1 - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
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
            ErrorImpresion = True
            e.HasMorePages = False
        End Try

    End Sub
    Private Function Valida() As Boolean

        If DtGrid1.Rows.Count = 0 Then
            MsgBox("Falta Informar Conceptos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        If DiferenciaDias(DateTime1.Value, DtPrestamosCabeza.Rows(0).Item("FechaOtorgado")) > 0 Then
            If MsgBox("Fecha Nota Menor a la Fecha en que fue otorgado el Prestamo. Quiere Continuar?", MsgBoxStyle.YesNo + MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Return False
            End If
        End If

        Dim Row As DataGridViewRow
        Dim Tope As Integer

        If Grid1.AllowUserToAddRows Then
            Tope = Grid1.Rows.Count - 2
        Else : Tope = Grid1.Rows.Count - 1
        End If

        For i As Integer = 0 To Tope
            Row = Grid1.Rows(i)
            If Row.Cells("Concepto1").Value = 0 Then
                MsgBox("Debe Informar Concepto en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.CurrentCell = Grid1.Rows(i).Cells("Concepto1")
                Grid1.BeginEdit(True)
                Return False
            End If
            If Row.Cells("Importe1").Value = 0 Then
                MsgBox("Debe Informar Importe en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.CurrentCell = Grid1.Rows(i).Cells("Importe1")
                Grid1.BeginEdit(True)
                Return False
            End If
        Next

        If PTipoNota = 1010 And PMovimiento = 0 Then
            Dim Importe As Double = 0
            For Each Row1 As DataRow In DtGrid1.Rows
                If Row1("Concepto") = 6 Then Importe = Importe + Row1("Importe")
            Next
            If Importe + CDbl(TextCancelado.Text) > CDbl(TextCapital.Text) Then
                MsgBox("Cancelación Supera Capital Prestado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.CurrentCell = Grid1.Rows(0).Cells("Importe1")
                Grid1.BeginEdit(True)
                Return False
            End If
        End If

        If Grid.Rows.Count = 1 Then
            MsgBox("Falta Informar Medio de Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        If CDbl(TextImporte.Text) <> CDbl(TextTotalMedioPago.Text) Then
            MsgBox("Monto Nota no Coincide con Total de Medio de Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        If Not ConsistePagos(Grid, DtFormasPago, 60, False) Then Return False

        Dim ImporteRetencion As Double = 0
        For Each Row1 As DataGridViewRow In Grid1.Rows
            If Not IsNothing(Row1.Cells("Concepto1").Value) Then
                If Row1.Cells("TieneLupa").Value Then
                    ImporteRetencion = ImporteRetencion + Row1.Cells("Importe1").Value
                End If
            End If
        Next
        If ImporteRetencion <> 0 Then
            If DtRetencionProvinciaAux.Rows.Count = 0 Then
                MsgBox("Falta Distribuir Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.Focus()
                Return False
            End If
            Dim ImporteDistribuido As Double
            For Each Row1 As DataRow In DtRetencionProvinciaAux.Rows
                ImporteDistribuido = ImporteDistribuido + Row1("Importe")
            Next
            If ImporteDistribuido <> ImporteRetencion Then
                MsgBox("Incorrecta Distribución Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.Focus()
                Return False
            End If
        End If

        If CDec(TextGrabado.Text) <> 0 And CDec(TextTotalIva.Text) = 0 Then
            If MsgBox("No se Informó Iva para Conceptos-Grabados Informado. " + vbCrLf + "Puede producir inconsistencias en Informes Impositivos. Desea Continuar?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then Return False
        End If

        If CDec(TextGrabado.Text) = 0 And CDec(TextTotalIva.Text) <> 0 Then
            If MsgBox("No se Informó Conceptos-Grabados para Iva Informado. " + vbCrLf + "Puede producir inconsistencias en Informes Impositivos. Desea Continuar?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then Return False
        End If

        If CDec(TextTotalIva.Text) <> 0 Then
            If Abs(CDec(TextGrabado.Text) - CDec(TextBaseImponible.Text)) > 2 Then
                If MsgBox("Base-Imponible de los Ivas Informados difiere del Neto-Grabado Informado. " + vbCrLf + "Puede producir inconsistencias en Informes Impositivos. Desea Continuar?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then Return False
            End If
        End If

        '--------------Consiste recibo 0ficial------------------------
        If PAbierto And Panel4.Visible Then
            If TextLetra.Text = "" Then
                MsgBox("Falta Informar Letra.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextLetra.Focus()
                Return False
            Else
                If TextLetra.Text <> "A" And TextLetra.Text <> "B" And TextLetra.Text <> "C" And TextLetra.Text <> "M" And TextLetra.Text <> "E" Then
                    MsgBox("Letra Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    TextLetra.Focus()
                    Return False
                Else
                    If Not LetraOk(DtPrestamosCabeza.Rows(0).Item("Origen"), TextLetra.Text) Then
                        MsgBox("Letra no se Corresponde con Categoría IVA.")
                        TextLetra.Focus()
                        Return False
                    End If
                End If
            End If
            If TextLetra.Visible Then
                If Val(MaskedReciboOficial.Text) = 0 Then
                    MsgBox("Falta informar Recibo Oficial.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    MaskedReciboOficial.Focus()
                    Return False
                End If
                If Not MaskedOK(MaskedReciboOficial) Then
                    MsgBox("Numero Recibo Oficial Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    MaskedReciboOficial.Focus()
                    Return False
                End If
                If ReciboOficialAnt <> DtNotaCabeza.Rows(0).Item("ReciboOficial") Then
                    If ExisteReciboOficial(1010, DtNotaCabeza.Rows(0).Item("Movimiento"), DtNotaCabeza.Rows(0).Item("Prestamo"), DtNotaCabeza.Rows(0).Item("ReciboOficial"), Conexion) Then
                        MsgBox("Recibo Oficial Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        MaskedReciboOficial.Focus()
                        Return False
                    End If
                End If
            End If
        End If
        '--------------------------------------------------------

        Return True

    End Function
    Private Function LetraOk(ByVal Origen As Integer, ByVal Letra As String) As Boolean

        Dim LetraW As String

        If Origen = 1 Then Return True

        Select Case Origen
            Case 2
                LetraW = LetraTipoIva(LetrasPermitidasProveedor(HallaTipoIvaProveedor(DtPrestamosCabeza.Rows(0).Item("Emisor")), 500))
            Case 3
                LetraW = LetraTipoIva(LetrasPermitidasCliente(HallaTipoIvaCliente(DtPrestamosCabeza.Rows(0).Item("Emisor")), 50))
        End Select

        If Letra = LetraW Then Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        Dim RowsBusqueda() As DataRow

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Or Grid.Columns(e.ColumnIndex).Name = "Banco" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
                If cb.SelectedIndex < 0 Then cb.SelectedValue = 0
            Else
                Exit Sub
            End If

            If IsNothing(cb.SelectedValue) Then Exit Sub

            If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
                If cb.SelectedIndex <= 0 Then Exit Sub
                RowsBusqueda = DtFormasPago.Select("Clave = " & cb.SelectedValue)
                If PTipoNota = 1015 Then
                    ArmaGridSegunConcepto(Grid.Rows(e.RowIndex), RowsBusqueda(0).Item("Tipo"), 60, False, False, PAbierto)
                Else
                    ArmaGridSegunConcepto(Grid.Rows(e.RowIndex), RowsBusqueda(0).Item("Tipo"), 600, False, False, PAbierto)
                End If
            End If
        End If

    End Sub
    Private Sub Grid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.CellBeginEdit
        'eCheq.
        If Grid.Columns(e.ColumnIndex).Name = "eCheq" Then
            If Grid.Rows(e.RowIndex).Cells("Concepto").Value <> 2 And PTipoNota = 1010 Then e.Cancel = True
            If Grid.Rows(e.RowIndex).Cells("Concepto").Value <> 3 And PTipoNota = 1015 Then e.Cancel = True
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        If Grid.Columns(e.ColumnIndex).Name = "Neto" Or Grid.Columns(e.ColumnIndex).Name = "Alicuota" Or _
           Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "Cambio" Then
            If IsDBNull(Grid.CurrentCell.ToString) Then Grid.CurrentCell.Value = 0
            CalculaTotales()
        End If

    End Sub
    Private Sub Grid_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellValueChanged

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Concepto")) Then
                Grid.CurrentRow.Cells("Neto").Value = 0
                Grid.CurrentRow.Cells("Operacion").Value = 0
                Grid.CurrentRow.Cells("Alicuota").Value = 0
                Grid.CurrentRow.Cells("Iva").Value = 0
                Grid.CurrentRow.Cells("Importe").Value = 0
                Grid.CurrentRow.Cells("Banco").Value = 0
                Grid.CurrentRow.Cells("Cuenta").Value = 0
                Grid.CurrentRow.Cells("Serie").Value = ""
                Grid.CurrentRow.Cells("Numero").Value = 0
                Grid.CurrentRow.Cells("Fecha").Value = "1/1/1800"
                Grid.CurrentRow.Cells("Cambio").Value = 0
                Grid.CurrentRow.Cells("EmisorCheque").Value = ""
                Grid.CurrentRow.Cells("ClaveCheque").Value = 0
                Grid.CurrentRow.Cells("Comprobante").Value = 0
                Grid.CurrentRow.Cells("FechaComprobante").Value = "1/1/1800"
                Grid.CurrentRow.Cells("eCheq").Value = False                         'eCheq.
                Grid.Refresh()
                CalculaTotales()
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Banco" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Banco")) Then
                Grid.CurrentRow.Cells("Cuenta").Value = 0
                Grid.Refresh()
            End If
        End If

    End Sub
    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If ComboEstado.SelectedValue = 3 Then Exit Sub

        If Grid.Rows(e.RowIndex).Cells("Concepto").ReadOnly Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta" And PTipoNota = 1015 Then
            If IsDBNull(Grid.CurrentRow.Cells("Concepto").Value) Then Exit Sub
            If HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 7 Then
                ListaBancos.PEsSeleccionaCuenta = True
                ListaBancos.PEsSoloPesos = True
                ListaBancos.ShowDialog()
                If ListaBancos.PCuenta <> 0 Then
                    Grid.CurrentRow.Cells("Banco").Value = ListaBancos.PBanco
                    Grid.CurrentRow.Cells("Cuenta").Value = ListaBancos.PCuenta
                    Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Comprobante")
                End If
                ListaBancos.Dispose()
            End If
            '
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta" And PTipoNota <> 1015 Then
            If IsDBNull(Grid.CurrentRow.Cells("Concepto").Value) Then Exit Sub
            '
            If Grid.CurrentRow.Cells("Concepto").Value = 2 Then
                ListaBancos.PEsSeleccionaCuenta = True
                ListaBancos.PEsSoloPesos = True
                ListaBancos.PConChequera = True
                ListaBancos.ShowDialog()
                If ListaBancos.PCuenta <> 0 Then
                    Grid.CurrentRow.Cells("Banco").Value = ListaBancos.PBanco
                    Grid.CurrentRow.Cells("Cuenta").Value = ListaBancos.PCuenta
                    Grid.CurrentRow.Cells("Serie").Value = ListaBancos.PSerie
                    Grid.CurrentRow.Cells("NumeracionInicial").Value = ListaBancos.PNumeracionInicial
                    Grid.CurrentRow.Cells("NumeracionFinal").Value = ListaBancos.PNumeracionFinal
                    If Not Grid.CurrentRow.Cells("eCheq").Value Then        'eCheq.
                        Grid.CurrentRow.Cells("Numero").Value = HallaSecuenciaCheque(ListaBancos.PBanco, ListaBancos.PCuenta, ListaBancos.PUltimoNumero)
                        Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Numero")
                    End If
                End If
                ListaBancos.Dispose()
            End If

            If HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 7 Then
                ListaBancos.PEsSeleccionaCuenta = True
                ListaBancos.PEsSoloPesos = True
                ListaBancos.ShowDialog()
                If ListaBancos.PCuenta <> 0 Then
                    Grid.CurrentRow.Cells("Banco").Value = ListaBancos.PBanco
                    Grid.CurrentRow.Cells("Cuenta").Value = ListaBancos.PCuenta
                    Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Comprobante")
                End If
                ListaBancos.Dispose()
            End If
            '
            If Grid.CurrentRow.Cells("Concepto").Value = 3 Then
                SeleccionarCheques.PCaja = GCaja
                SeleccionarCheques.PEsChequeEnCartera = True
                SeleccionarCheques.PListaDeCheques = New List(Of ItemCheque)
                For Each Row As DataRow In DtGrid.Rows
                    If Row("MedioPago") = 3 And Row("ClaveCheque") <> 0 Then
                        Dim Item As New ItemCheque
                        Item.ClaveCheque = Row("ClaveCheque")
                        SeleccionarCheques.PListaDeCheques.Add(Item)
                    End If
                Next
                SeleccionarCheques.PAbierto = PAbierto
                SeleccionarCheques.ShowDialog()
                If SeleccionarCheques.PListaDeCheques.Count <> 0 Then
                    IngresaCheques(SeleccionarCheques.PListaDeCheques)
                    CalculaTotales()
                End If
                SeleccionarCheques.Dispose()
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If PTipoNota = 1015 Then
                If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
                    If Grid.Rows(e.RowIndex).Cells("Concepto").Value = 3 Then
                        Calendario.ShowDialog()
                        Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                        Grid.EndEdit()
                        Calendario.Dispose()
                    End If
                End If
            Else
                If Grid.Rows(e.RowIndex).Cells("Concepto").Value = 2 Then
                    Calendario.ShowDialog()
                    Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                    Grid.EndEdit()
                    Calendario.Dispose()
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FechaComprobante" Then
            If HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 7 Or HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 4 Then
                Calendario.ShowDialog()
                Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                Grid.EndEdit()
                Calendario.Dispose()
            End If
            If HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 10 Then
                Calendario.ShowDialog()
                Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                Grid.EndEdit()
                Calendario.Dispose()
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "Neto" Or _
           Grid.Columns(e.ColumnIndex).Name = "Alicuota" Or Grid.Columns(e.ColumnIndex).Name = "Iva" Or Grid.Columns(e.ColumnIndex).Name = "Cambio" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaComprobante" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = "1/1/1800" Then e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Or Grid.Columns(e.ColumnIndex).Name = "Comprobante" Or Grid.Columns(e.ColumnIndex).Name = "Cuenta" Or Grid.Columns(e.ColumnIndex).Name = "ClaveChequeVisual" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, 0)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta" Then
            e.Value = Nothing
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

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Detalle" Then Exit Sub
        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Banco" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Not Grid.Columns(Grid.CurrentCell.ColumnIndex).Name = "Concepto" Then
            If IsDBNull(Grid.CurrentRow.Cells("Concepto").Value) Then
                e.KeyChar = ""
                Exit Sub
            End If
            If Grid.CurrentRow.Cells("Concepto").Value = 0 Then
                e.KeyChar = ""
                Exit Sub
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Neto" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Alicuota" Then
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cambio" Then
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Numero" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Comprobante" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cuenta" Then
            If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Serie" Then
            If e.KeyChar = "" Then Exit Sub
            e.KeyChar = e.KeyChar.ToString.ToUpper
            If Asc(e.KeyChar) < 65 Or Asc(e.KeyChar) > 90 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Neto" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Alicuota" Or _
           Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cambio" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row

        Row("Item") = 0
        Row("MedioPago") = 0
        Row("Detalle") = ""
        Row("Neto") = 0
        Row("Alicuota") = 0
        Row("ImporteIva") = 0
        Row("Banco") = 0
        Row("Fecha") = "1/1/1800"
        Row("Cuenta") = 0
        Row("Serie") = ""
        Row("Numero") = 0
        Row("EmisorCheque") = ""
        Row("Cambio") = 0
        Row("Importe") = 0
        Row("Comprobante") = 0
        Row("FechaComprobante") = "1/1/1800"
        Row("ClaveCheque") = 0
        Row("eCheq") = False                            'eCheq.

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Cuenta") Then
            If IsDBNull(e.Row("Cuenta")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Numero") Then
            If IsDBNull(e.Row("Numero")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Cambio") Then
            If IsDBNull(e.Row("Cambio")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

        If e.Column.ColumnName.Equals("Importe") Then
            If IsDBNull(e.Row("Importe")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

        If (e.Column.ColumnName.Equals("Comprobante")) Then
            If IsDBNull(e.Row("Comprobante")) Then Exit Sub
            If IsNothing(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If (e.Column.ColumnName.Equals("EmisorCheque")) Then
            If IsDBNull(e.Row("EmisorCheque")) Then Exit Sub
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
        End If

        If (e.Column.ColumnName.Equals("Fecha") Or e.Column.ColumnName.Equals("FechaComprobante")) Then
            If IsDBNull(e.Row("Fecha")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = "1/1/1800"
        End If

        CalculaTotales()

    End Sub
    Private Sub DtGrid_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        If DtGrid.Rows.Count > GLineasPagoRecibos Then
            MsgBox("Supera Cantidad Items Permitidos.", MsgBoxStyle.Information)
            e.Row.Delete()
        End If

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID1.                 --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid1_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellEnter

        If Not Grid1.Columns(e.ColumnIndex).ReadOnly Then
            Grid1.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid1_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellLeave

        If PMovimiento <> 0 Then Exit Sub

        Dim RowsBusqueda() As DataRow
        Dim RowsBusqueda1() As DataRow

        'para manejo del autocoplete de conceptos.
        If Grid1.Columns(e.ColumnIndex).Name = "Concepto1" Then
            If Not cc Is Nothing Then
                cc.SelectedIndex = cc.FindStringExact(cc.Text)
                If cc.SelectedIndex <= 0 Then cc.SelectedValue = 0 : Exit Sub
                If IsNothing(cc.SelectedValue) Then Exit Sub
                RowsBusqueda = DtConceptos.Select("Clave = " & cc.SelectedValue)
                If RowsBusqueda(0).Item("Tipo") = 4 Then
                    RowsBusqueda1 = DtGrid1.Select("Concepto = " & cc.SelectedValue)
                    If RowsBusqueda1.Length <> 0 Then
                        MsgBox("Retención o Percepción ya Existe.")
                        cc.SelectedValue = 0
                        Exit Sub
                    End If
                    If EsRetencionPorProvincia(cc.SelectedValue) Then
                        Grid1.Rows(e.RowIndex).Cells("TieneLupa").Value = True
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub Grid1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid1.Columns(e.ColumnIndex).Name = "Lupa" Then
            If Not Grid1.CurrentRow.Cells("TieneLupa").Value Then Exit Sub
            If Grid1.CurrentRow.Cells("Importe1").Value = 0 Then
                MsgBox("Debe Informar Importe")
                Exit Sub
            End If
            If PMovimiento <> 0 Then SeleccionarRetencionesProvincias.PFuncionBloqueada = True
            SeleccionarRetencionesProvincias.PDtGrid = DtRetencionProvinciaAux
            SeleccionarRetencionesProvincias.PImporte = Grid1.CurrentRow.Cells("Importe1").Value
            SeleccionarRetencionesProvincias.PRetencion = Grid1.CurrentRow.Cells("Concepto1").Value
            SeleccionarRetencionesProvincias.ShowDialog()
            SeleccionarRetencionesProvincias.Dispose()
        End If

    End Sub
    Private Sub Grid1_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid1.EditingControlShowing

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cc = e.Control
            cc.DropDownStyle = ComboBoxStyle.DropDown
            cc.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cc.AutoCompleteSource = AutoCompleteSource.ListItems
            Exit Sub
        End If

        Dim columna As Integer = Grid1.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey1_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged1_TextChanged

    End Sub
    Private Sub ValidaKey1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Importe1" Then
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Importe1" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid1_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellEndEdit

        If Grid1.Columns(e.ColumnIndex).Name = "Importe1" Then
            CalculaTotales()
        End If

    End Sub
    Private Sub Grid1_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid1.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid1.Columns(e.ColumnIndex).Name = "Importe1" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#.##")
                Else : e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

        If Grid1.Columns(e.ColumnIndex).Name = "Lupa" Then
            If Not IsDBNull(Grid1.Rows(e.RowIndex).Cells("TieneLupa").Value) Then
                If Grid1.Rows(e.RowIndex).Cells("TieneLupa").Value Then
                    e.Value = ImageList1.Images.Item("Lupa")
                Else : e.Value = Nothing
                End If
            End If
        End If

    End Sub
    Private Sub Grid1_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid1.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid1_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row
        Row("Movimiento") = PMovimiento
        Row("Item") = 0
        Row("Concepto") = 0
        Row("Importe") = 0
        Row("TieneLupa") = False

    End Sub
    Private Sub DtGrid1_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Importe") Then
            If IsDBNull(e.Row("Importe")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

    End Sub
    Private Sub DtGrid1_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        CalculaTotales()

        If DtGrid1.Rows.Count > GLineasConceptosPrestamos Then
            MsgBox("Supera Cantidad Items Permitidos.", MsgBoxStyle.Information)
            e.Row.Delete()
        End If

    End Sub

End Class