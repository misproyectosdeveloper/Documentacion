Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnMovimientoFondoFijo
    '7001  Aumenta Fondo Fijo.
    '7002  Disminuye Fondo Fijo.
    Public PMovimiento As Integer
    Public PFondoFijo As Integer
    Public PAbierto As Boolean
    Public PNumero As Integer
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim DtCabeza As DataTable
    Dim DtDetallePago As DataTable
    Dim DtFondoFijo As DataTable
    Dim DtGrid As DataTable
    Dim DtFormasPago As DataTable
    '
    Dim TotalConceptos As Decimal
    Dim TipoAsiento As Integer
    Dim Tipo As Integer
    Dim ConexionMovimiento As String
    'Variables Impresion. 
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim LineasParaImpresion As Integer = 0
    Private Sub UnFondoFijo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If GCaja = 0 And PMovimiento = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PMovimiento <> 0 Then
            If EsReemplazoChequeFondoFijo(PMovimiento, PAbierto) Then
                UnChequeReemplazo.PTipoNota = 7001
                UnChequeReemplazo.PNota = PMovimiento
                UnChequeReemplazo.PAbierto = PAbierto
                UnChequeReemplazo.POrigenDestino = 6
                UnChequeReemplazo.PBloqueaFunciones = PBloqueaFunciones
                UnChequeReemplazo.ShowDialog()
                Me.Close()
                Exit Sub
            End If
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        If PMovimiento = 0 Then
            OpcionFondoFijo.ShowDialog()
            If OpcionFondoFijo.PRegresar Then OpcionFondoFijo.Dispose() : Me.Close() : Exit Sub
            PFondoFijo = OpcionFondoFijo.PFondoFijo
            PNumero = OpcionFondoFijo.PNumeroFondoFijo
            PAbierto = OpcionFondoFijo.PAbierto
            OpcionFondoFijo.Dispose()
            If FondoFijoCerrado(PNumero, PAbierto) Then
                MsgBox("Fondo Fijo Cerrado.", MsgBoxStyle.Critical)
                Me.Close() : Exit Sub
            End If
        End If

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionMovimiento = Conexion
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionMovimiento = ConexionN
        End If

        GModificacionOk = False

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

    End Sub
    Private Sub UnFondoFijo_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

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

        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        Dim DtDetallePagoAux As DataTable = DtDetallePago.Copy
        Dim DtFondoFijoAux As DataTable = DtFondoFijo.Copy

        Dim Row As DataRow

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PMovimiento = 0 Then
            'Enumero los registros.
            Dim Item As Integer = 0
            For Each Row In DtGrid.Rows
                Item = Item + 1
                Row("Item") = Item
            Next
            'Genero archivos de pagos.
            For Each Row In DtGrid.Rows
                Dim Row1 As DataRow = DtDetallePagoAux.NewRow
                Row1("Movimiento") = 0
                Row1("Item") = Row("Item")
                Row1("MedioPago") = Row("MedioPago")
                Row1("Cambio") = Row("Cambio")
                Row1("Importe") = Row("Importe")
                Row1("Banco") = Row("Banco")
                Row1("Cuenta") = Row("Cuenta")
                Row1("Comprobante") = Row("Comprobante")
                Row1("FechaComprobante") = Row("FechaComprobante")
                Row1("ClaveInterna") = Row("ClaveInterna")
                Row1("ClaveCheque") = Row("ClaveCheque")
                DtDetallePagoAux.Rows.Add(Row1)
            Next
            DtCabezaAux.Rows(0).Item("Tipo") = Tipo
            DtCabezaAux.Rows(0).Item("Saldo") = CDec(TextImporte.Text)
        End If

        If IsNothing(DtCabezaAux.GetChanges) And IsNothing(DtDetallePagoAux.GetChanges) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No hay Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Select Case Tipo
            Case 7001
                DtFondoFijoAux.Rows(0).Item("Saldo") = CDec(DtFondoFijoAux.Rows(0).Item("Saldo")) + CDec(TextImporte.Text)
            Case 7002
                DtFondoFijoAux.Rows(0).Item("Saldo") = CDec(DtFondoFijoAux.Rows(0).Item("Saldo")) - CDec(TextImporte.Text)
        End Select

        'Arma Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        '
        If GGeneraAsiento And PMovimiento = 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtDetallePagoAux, DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If PMovimiento = 0 Then
            If HacerAlta(DtCabezaAux, DtDetallePagoAux, DtAsientoCabeza, DtAsientoDetalle, DtFondoFijoAux) Then
                If Not ArmaArchivos() Then Me.Close() : Exit Sub
            End If
        Else
            '   If HacerModificacion(DtCabezaAux, DtDetallePagoAux, DtAsientoCabeza, DtAsientoDetalle) Then
            '    UnFondoFijo_Load(Nothing, Nothing)
            '   End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        MiEnlazador.EndEdit()

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If GCaja = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Ajuste ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If FondoFijoCerrado(PNumero, PAbierto) Then
            MsgBox("Fondo Fijo Cerrado. Operación se CANCELA.", MsgBoxStyle.Critical)
            Exit Sub
        End If

        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        Dim DtDetallePagoAux As DataTable = DtDetallePago.Copy
        Dim DtFondoFijoAux As DataTable = DtFondoFijo.Copy

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Select Case Tipo
            Case 7001
                DtFondoFijoAux.Rows(0).Item("Saldo") = CDec(DtFondoFijoAux.Rows(0).Item("Saldo")) - CDec(TextImporte.Text)
                If DtFondoFijoAux.Rows(0).Item("Saldo") < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("La Anulación Hace Negativo el Saldo del Fondo Fijo. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            Case 7002
                DtFondoFijoAux.Rows(0).Item("Saldo") = CDec(DtFondoFijoAux.Rows(0).Item("Saldo")) + CDec(TextImporte.Text)
        End Select

        Dim Resul As Double

        For Each Row As DataRow In DtGrid.Rows
            If Row("Mediopago") = 2 Or Row("Mediopago") = 3 Then
                Dim Anulado As Boolean, Rechazado As Boolean, Depositado As Boolean, Entregado As Boolean, Afectado As Boolean
                If Not EstadoCheque(Row("Mediopago"), Row("ClaveCheque"), ConexionMovimiento, Anulado, Rechazado, Depositado, Entregado, Afectado) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Error Base de Datos.")
                    Exit Sub
                End If
                If Rechazado Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Existe Nota de Rechazo. Operación se CANCELA.")
                    Exit Sub
                End If
                If Anulado Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Cheque Anulado. Operación se CANCELA.")
                    Exit Sub
                End If
                If Depositado Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Cheque Depositado. Operación se CANCELA.")
                    Exit Sub
                End If
                If Afectado Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Cheque " & Row("Numero") & " no se puede Borrar. Esta Afectado a Otra Orden. Operación se CANCELA.")
                    Exit Sub
                End If
            End If
            If Row("MedioPago") = 3 Then
                If ExiteChequeEnPaseCaja(ConexionMovimiento, Row("ClaveCheque")) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Cheque " & Row("Numero") & " en Proceso de Pase de Caja. Operación se CANCELA.")
                    Exit Sub
                End If
            End If
        Next

        If Tipo = 7001 Then
            For Each Row As DataRow In DtGrid.Rows
                If Row("Mediopago") = 2 Or Row("Mediopago") = 3 Then
                    If ChequeReemplazado(Row("Mediopago"), Row("ClaveCheque"), Tipo, PMovimiento, ConexionMovimiento) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Cheque " & Row("Numero") & " no se puede Borrar, fue Reemplazado. Operación se CANCELA.")
                        Exit Sub
                    End If
                End If
            Next
        End If

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        '
        If Not HallaAsientosCabeza(TipoAsiento, PMovimiento, DtAsientoCabeza, ConexionMovimiento) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If
        For Each Row As DataRow In DtAsientoCabeza.Rows
            Row("Estado") = 3
        Next

        If MsgBox("El Movimiento se Anulara. ¿Desea Anularlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtCabezaAux.Rows(0).Item("Estado") = 3

        Resul = ActualizaMovi("B", DtCabezaAux, DtDetallePagoAux, DtAsientoCabeza, DtAsientoDetalle, DtFondoFijoAux)
        If Resul < 0 And Resul <> -3 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Prestamo Anulado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            If Not ArmaArchivos() Then Me.Close() : Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub RadioAumenta_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioAumenta.CheckedChanged

        If RadioAumenta.Checked = True Then
            ArmaMedioPagoOrdenFondoFijo(DtFormasPago)
            Tipo = 7001
        End If

    End Sub
    Private Sub RadioDisminuye_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioDisminuye.CheckedChanged

        If RadioDisminuye.Checked = True Then
            ArmaMedioPagoCobranzaFondoFijo(DtFormasPago)
            Tipo = 7002
        End If

    End Sub
    Private Sub ButtonMediosDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMediosDePago.Click

        If Tipo = 0 Then
            MsgBox("Falta Informar si Aumenta o Disminuye al Fondo Fijo.")
            Exit Sub
        End If

        Select Case Tipo
            Case 7001
                UnMediosPago.PTipoNota = 600
            Case 7002
                UnMediosPago.PTipoNota = 60
        End Select
        UnMediosPago.PDtGrid = DtGrid
        UnMediosPago.PAbierto = PAbierto
        If PMovimiento = 0 Then
            UnMediosPago.PBloqueaFunciones = False
        Else : UnMediosPago.PBloqueaFunciones = True
        End If
        UnMediosPago.PDtFormasPago = DtFormasPago
        UnMediosPago.PDtRetencionProvincia = New DataTable
        UnMediosPago.PEsChequeRechazado = False
        UnMediosPago.PEsExterior = False
        UnMediosPago.PDtRetencionesAutomaticas = New DataTable
        UnMediosPago.PDiferenciaDeCambio = False
        UnMediosPago.PMoneda = 1
        UnMediosPago.PCambio = 1
        UnMediosPago.PImporte = CDec(TextTotalRecibo.Text)
        UnMediosPago.PImporteAInformar = CDec(TextImporte.Text)
        UnMediosPago.PEsTr = False
        UnMediosPago.PEsEnMonedaExtranjera = False
        UnMediosPago.ShowDialog()
        DtGrid = UnMediosPago.PDtGrid

        UnMediosPago.Dispose()

        CalculaTotales()

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PMovimiento = 0 Then
            MsgBox("Opcion Invalida. Nota debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ErrorImpresion = False
        Paginas = 0

        Copias = 1

        Dim print_document As New PrintDocument

        LineasParaImpresion = 0
        AddHandler print_document.PrintPage, AddressOf Print_PrintOrdenPago
        print_document.Print()

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PMovimiento = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = TipoAsiento
        If PAbierto Then
            ListaAsientos.PDocumentoB = PMovimiento
            ListaAsientos.PDocumentoN = 0
        Else
            ListaAsientos.PDocumentoB = 0
            ListaAsientos.PDocumentoN = PMovimiento
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonEliminarTodo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarTodo.Click

        If MsgBox("Conceptos se Anularan. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtGrid.Clear()
        CalculaTotales()

    End Sub
    Private Sub TextImporte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextImporte.KeyPress

        EsNumerico(e.KeyChar, TextImporte.Text, 2)

    End Sub
    Private Sub TextNumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumero.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, 0)

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtGrid = CreaDtParaGrid()

        DtCabeza = New DataTable
        Dim Sql As String = "SELECT * FROM MovimientosFondoFijoCabeza WHERE Movimiento = " & PMovimiento & ";"
        If Not Tablas.Read(Sql, ConexionMovimiento, DtCabeza) Then Return False
        If PMovimiento <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PMovimiento = 0 Then AgregaCabeza()

        MuestraCabeza()

        Tipo = DtCabeza.Rows(0).Item("Tipo")
        PFondoFijo = DtCabeza.Rows(0).Item("FondoFijo")
        PNumero = DtCabeza.Rows(0).Item("Numero")
        TextNombreFondoFijo.Text = NombreProveedorFondoFijo(DtCabeza.Rows(0).Item("FondoFijo"))

        TipoAsiento = 7201

        DtFondoFijo = New DataTable
        Sql = "SELECT * FROM FondosFijos WHERE FondoFijo = " & PFondoFijo & " AND Numero = " & PNumero & ";"
        If Not Tablas.Read(Sql, ConexionMovimiento, DtFondoFijo) Then Return False
        If DtFondoFijo.Rows.Count = 0 Then
            MsgBox("Numero de Fondo Fijo No Existe.", MsgBoxStyle.Critical)
            Return False
        End If

        DtDetallePago = New DataTable
        Sql = "SELECT * FROM MovimientosFondoFijoPago WHERE Movimiento = " & PMovimiento & ";"
        If Not Tablas.Read(Sql, ConexionMovimiento, DtDetallePago) Then Return False

        Dim RowsBusqueda() As DataRow

        Dim DtCheques As New DataTable
        For Each Row As DataRow In DtDetallePago.Rows
            If Row("ClaveCheque") <> 0 And (Row("MedioPago") = 2 Or Row("MedioPago") = 3) Then
                If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionMovimiento, DtCheques) Then Return False
            End If
        Next

        For Each Row As DataRow In DtDetallePago.Rows
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Item") = Row("Item")
            Row1("MedioPago") = Row("MedioPago")
            Row1("Importe") = Row("Importe")
            Row1("Cambio") = Row("Cambio")
            Row1("Bultos") = 0
            Row1("Detalle") = ""
            Row1("Alicuota") = 0
            Row1("Neto") = 0
            Row1("ImporteIva") = 0
            Row1("Comprobante") = Row("Comprobante")
            Row1("FechaComprobante") = Row("FechaComprobante")
            Row1("ClaveCheque") = Row("ClaveCheque")
            Row1("ClaveInterna") = Row("ClaveInterna")
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
                    Row1("Fecha") = RowsBusqueda(0).Item("Fecha")
                    Row1("eCheq") = RowsBusqueda(0).Item("eCheq")
                Else
                    Row1("Banco") = 0
                    Row1("Cuenta") = 0
                    Row1("Serie") = ""
                    Row1("Numero") = 0
                    Row1("EmisorCheque") = ""
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

        CalculaTotales()

        If PMovimiento = 0 Then
            Panel3.Enabled = True
            ButtonEliminarTodo.Enabled = True
        Else
            Panel3.Enabled = False
            ButtonEliminarTodo.Enabled = False
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub AgregaCabeza()

        Dim Row As DataRow

        Row = DtCabeza.NewRow
        ArmaNuevoMovimientoFondoFijo(Row)
        Row("Numero") = PNumero
        Row("FondoFijo") = PFondoFijo
        Row("Fecha") = Now
        Row("Caja") = GCaja
        Row("Estado") = 1
        DtCabeza.Rows.Add(Row)

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Movimiento")
        AddHandler Enlace.Format, AddressOf FormatMaskedEntero
        AddHandler Enlace.Parse, AddressOf ParseMaskedImporte
        TextMovimiento.DataBindings.Clear()
        TextMovimiento.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Numero")
        AddHandler Enlace.Format, AddressOf FormatMaskedEntero
        AddHandler Enlace.Parse, AddressOf ParseMaskedEntero
        TextNumero.DataBindings.Clear()
        TextNumero.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatMaskedImporte
        AddHandler Enlace.Parse, AddressOf ParseMaskedImporte
        TextImporte.DataBindings.Clear()
        TextImporte.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Dim Row As DataRowView = MiEnlazador.Current

        If Row("Tipo") = 7001 Then RadioAumenta.Checked = True : Panel4.Enabled = False
        If Row("Tipo") = 7002 Then RadioDisminuye.Checked = True : Panel4.Enabled = False

    End Sub
    Private Sub FormatMaskedEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "#")

    End Sub
    Private Sub ParseMaskedEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatMaskedImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub ParseMaskedImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub CalculaTotales()

        TotalConceptos = 0
        For Each Row As DataRow In DtGrid.Rows
            If Row("Cambio") = 0 Then
                TotalConceptos = TotalConceptos + Row("Importe")
            Else : TotalConceptos = Trunca(TotalConceptos + Trunca(Row("Cambio") * Row("Importe")))
            End If
        Next

        TextTotalRecibo.Text = FormatNumber(TotalConceptos, GDecimales)

    End Sub
    Private Function HacerAlta(ByVal DtCabezaAux As DataTable, ByVal DtDetallePagoAux As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtFondoFijoAux As DataTable) As Boolean

        Dim NumeroMovi As Integer
        Dim Resul As Double
        Dim NumeroAsiento As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            NumeroMovi = UltimaNumeracion(ConexionMovimiento)
            If NumeroMovi < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            '
            DtCabezaAux.Rows(0).Item("Movimiento") = NumeroMovi
            For Each Row As DataRow In DtDetallePagoAux.Rows
                Row("Movimiento") = NumeroMovi
            Next

            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionMovimiento)
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

            Resul = ActualizaMovi("A", DtCabezaAux, DtDetallePagoAux, DtAsientoCabeza, DtAsientoDetalle, DtFondoFijoAux)

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
    Private Function ActualizaMovi(ByVal Funcion As String, ByVal DtCabezaAux As DataTable, ByVal DtDetallePagoAux As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtFondoFijoAux As DataTable) As Double

        Dim Resul As Double
        Dim RowsBusqueda() As DataRow

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaAux.GetChanges, "MovimientosFondoFijoCabeza", ConexionMovimiento)
                    If Resul <= 0 Then Return Resul
                End If
                '
                For Each Row As DataRow In DtDetallePagoAux.Rows
                    If Row.RowState = DataRowState.Added Then
                        If (Row("MedioPago") = 2 Or Row("MedioPago") = 3) Then
                            RowsBusqueda = DtGrid.Select("Item = " & Row("Item"))
                        End If
                        If Tipo = 7001 Then
                            If Row("MedioPago") = 2 Then
                                Row("ClaveCheque") = ActualizaClavesComprobantes("A", 0, Tipo, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), RowsBusqueda(0).Item("Serie"), RowsBusqueda(0).Item("Numero"), RowsBusqueda(0).Item("Fecha"), Row("Importe"), "", DtCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionMovimiento, False, False, RowsBusqueda(0).Item("eCheq"))
                                If Row("ClaveCheque") <= 0 Then
                                    MsgBox("Cheque " & RowsBusqueda(0).Item("Serie") & " " & RowsBusqueda(0).Item("Numero") & " ya fue Emitido.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                    Return -3
                                End If
                            End If
                            If Row("MedioPago") = 3 Then
                                If ActualizaClavesComprobantes("A", Row("ClaveCheque"), Tipo, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), RowsBusqueda(0).Item("Serie"), RowsBusqueda(0).Item("Numero"), RowsBusqueda(0).Item("Fecha"), Row("Importe"), RowsBusqueda(0).Item("EmisorCheque"), DtCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionMovimiento, False, False, RowsBusqueda(0).Item("eCheq")) <= 0 Then
                                    MsgBox("Cheque " & RowsBusqueda(0).Item("Serie") & " " & RowsBusqueda(0).Item("Numero") & " Error al Grabar.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                    Return -3
                                End If
                            End If
                        End If
                        If Tipo = 7002 Then
                            If Row("MedioPago") = 3 Then
                                Row("ClaveCheque") = ActualizaClavesComprobantes("A", Row("ClaveCheque"), Tipo, Row("MedioPago"), 0, Row("Banco"), Row("Cuenta"), RowsBusqueda(0).Item("Serie"), RowsBusqueda(0).Item("Numero"), RowsBusqueda(0).Item("Fecha"), Row("Importe"), RowsBusqueda(0).Item("EmisorCheque"), DtCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionMovimiento, False, False, RowsBusqueda(0).Item("eCheq"))
                                If Row("ClaveCheque") <= 0 Then
                                    MsgBox("Cheque " & RowsBusqueda(0).Item("Serie") & " " & RowsBusqueda(0).Item("Numero") & " Error al Grabar.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                    Return -3
                                End If
                            End If
                        End If
                    End If
                Next
                '
                If Funcion = "B" Then
                    For Each Row As DataRow In DtDetallePagoAux.Rows
                        If (Row("MedioPago") = 2 Or Row("MedioPago") = 3) Then
                            RowsBusqueda = DtGrid.Select("Item = " & Row("Item"))
                        End If
                        If Row("MedioPago") = 3 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), Tipo, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionMovimiento, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("ClaveCheque") & " " & Row("Cuenta") & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            End If
                        End If
                        If Row("MedioPago") = 2 Then
                            If ActualizaClavesComprobantes("B", Row("ClaveCheque"), Tipo, Row("MedioPago"), 1, Row("Banco"), Row("Cuenta"), "", 1, Now, Row("Importe"), "", DtCabezaAux.Rows(0).Item("Movimiento"), "01/01/1800", ConexionMovimiento, False, False, False) <= 0 Then
                                MsgBox("Cheque " & Row("Banco") & " " & Row("Cuenta") & " Error de datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                                Return -3
                            End If
                        End If
                    Next
                End If
                '
                If Not IsNothing(DtDetallePagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtDetallePagoAux.GetChanges, "MovimientosFondoFijoPago", ConexionMovimiento)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionMovimiento)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionMovimiento)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtFondoFijoAux.GetChanges) Then
                    Resul = GrabaTabla(DtFondoFijoAux.GetChanges, "FondosFijos", ConexionMovimiento)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtDetallePagoW As DataTable, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos

        For Each Row As DataRow In DtDetallePagoW.Rows
            Item = New ItemListaConceptosAsientos
            Item.Clave = Row("MedioPago")
            Select Case Tipo
                Case 7001
                    Item.Importe = Row("Importe")
                Case 7002
                    Item.Importe = -Row("Importe")
            End Select
            If Row("Cambio") <> 0 And Row("Cambio") <> 1 Then Item.Importe = Trunca(Row("Cambio") * Item.Importe)
            ListaConceptos.Add(Item)
        Next
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Select Case Tipo
            Case 7001
                Item.Importe = CDec(TextImporte.Text)
            Case 7002
                Item.Importe = -CDec(TextImporte.Text)
        End Select
        ListaConceptos.Add(Item)
        '
        If Not Asiento(TipoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

        Return True

    End Function
    Private Sub Print_PrintOrdenPago(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 37
        Dim UltimaLinea As Integer
        Dim LineasImpresas As Integer
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        Try
            Print_TituloOrdenPago(e, MIzq, MTop)

            If LineasParaImpresion < DtGrid.Rows.Count Then
                Print_MediosPago(e, MIzq, MTop, UltimaLinea, LineasImpresas)
            End If

            Print_FinalOrdenPago(e, MIzq, MTop)

            If (LineasParaImpresion < DtGrid.Rows.Count) Then
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
    Private Sub Print_TituloOrdenPago(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

        Dim Texto As String = ""

        'Encabezado.
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
        Texto = GNombreEmpresa
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
        If RadioAumenta.Checked Then
            Texto = "AUMENTO FONDO FIJO"
        Else : Texto = "DISMINUCION FONDO FIJO"
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 120, MTop)
        ' 
        PrintFont = New Font("Courier New", 12)
        If RadioAumenta.Checked Then
            Texto = "Nro. Aumento F.F.: " & TextMovimiento.Text
        Else : Texto = "Nro. Disminución F.F.: " & TextMovimiento.Text
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
        Texto = "Fecha:  " & DateTime1.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
        Texto = "Fondo Fijo: " & TextNombreFondoFijo.Text & "  Numero: " & TextNumero.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 27)
        Texto = "Importe       : " & TextImporte.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 60, MTop + 40)

    End Sub
    Private Sub Print_FinalOrdenPago(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

        Dim Texto As String = ""
        Dim Yq As Integer
        Dim SaltoLinea As Integer = 4
        Dim PrintFont As System.Drawing.Font

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

    End Sub
    Private Sub Print_MediosPago(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer, ByRef UltimaLinea As Integer, ByRef LineasImpresas As Integer)

        Dim Texto As String = ""
        Dim SaltoLinea As Integer = 4
        Dim Longi As Integer
        Dim Xq As Integer
        Dim Yq As Integer
        Dim x As Integer = MIzq
        Dim y As Integer = MTop + 50
        Dim Ancho As Integer = 180
        Dim RowsBusqueda() As DataRow
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 37

        'Grafica -Rectangulo medios de pagos. ----------------------------------------------------------------------

        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 10)
        Dim LineaDescripcion As Integer = x + 40
        Dim LineaCambio As Integer = x + 55
        Dim LineaImporte As Integer = x + 85
        Dim LineaBanco As Integer = x + 125
        Dim LineaNumero As Integer = x + 154
        Dim LineaVencimiento As Integer = x + Ancho
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
        'Descripcion del pago.
        Yq = y + SaltoLinea
        While Contador < LineasPorPagina And LineasParaImpresion < DtGrid.Rows.Count
            Dim Row As DataRow = DtGrid.Rows(LineasParaImpresion)
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
                Texto = FormatNumber(Trunca(Row("Cambio") * Row("Importe")), GDecimales)
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
                'Imprime FechaComprobante.
                If Format(Row("FechaComprobante"), "dd/MM/yyyy") <> "01/01/1800" Then
                    Texto = Format(Row("FechaComprobante"), "dd/MM/yyyy")
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaVencimiento - Longi - 2
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
            End If
            Contador = Contador + 1
            LineasParaImpresion = LineasParaImpresion + 1
        End While

        Yq = Yq + SaltoLinea

        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Yq - y)
        'Lineas vertical.
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaDescripcion, y, LineaDescripcion, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCambio, y, LineaCambio, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte, y, LineaImporte, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaBanco, y, LineaBanco, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaNumero, y, LineaNumero, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y + 2 * SaltoLinea, x + Ancho, y + 2 * SaltoLinea)

        UltimaLinea = Yq
        LineasImpresas = Contador

    End Sub
    Public Function UltimaNumeracion(ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Movimiento) FROM MovimientosFondoFijoCabeza;", Miconexion)
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
    Private Function Valida() As Boolean

        If CDec(TextImporte.Text) = 0 Then
            MsgBox("Falta Informar Importe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextImporte.Focus()
            Return False
        End If

        If CDbl(TextTotalRecibo.Text) = 0 Then
            MsgBox("Falta Informar Total Conceptos Pago/Cobranza. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextTotalRecibo.Focus()
            Return False
        End If

        If CDec(TextImporte.Text) <> CDec(TextTotalRecibo.Text) Then
            MsgBox("Total Importe Distinto a Total Conceptos Pago/Cobranza. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextImporte.Focus()
            Return False
        End If

        Dim RowsBusqueda() As DataRow
        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
            If RowsBusqueda.Length = 0 Then
                MsgBox("Existe Conceptos de Pago/Cobranza que No Pertenece a un Aumento o a una Disminución.", MsgBoxStyle.Critical)
                Return False
            End If
        Next

        If Tipo = 7002 Then
            If (CDec(DtFondoFijo.Rows(0).Item("Saldo")) - CDec(TextImporte.Text)) < 0 Then
                MsgBox("Importe Hace Negativo al Fondo Fijo. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextImporte.Focus()
                Return False
            End If
            If CDec(DtFondoFijo.Rows(0).Item("Saldo")) - CDec(TextImporte.Text) <= HallaSaldosRendicionesCerradas(PNumero, ConexionMovimiento) Then
                MsgBox("Disminución hace al Saldo del Fondo Fijo menor al importe consumido en Rendiciones YA Cerradas. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextImporte.Focus()
                Return False
            End If
        End If

        Return True

    End Function


End Class
