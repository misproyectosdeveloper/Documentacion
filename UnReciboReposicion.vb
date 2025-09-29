Option Explicit On
Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnReciboReposicion
    Public PNota As Double
    Public PAbierto As Boolean
    Public PTipoNota As Integer = 600
    Public PBloqueaFunciones As Boolean
    Public PImputa As Boolean
    '
    Dim DtNotaDetalle As DataTable
    Dim DtNotaCabeza As DataTable
    Dim DtMedioPago As DataTable
    Dim DtRendiciones As DataTable
    Dim DtGridCompro As DataTable
    Dim DtGrid As DataTable
    Dim DtFormasPago As DataTable
    '
    Private MiEnlazador As New BindingSource
    '
    Dim ProveedorFondoFijo As Integer
    Dim Numero As Integer
    '
    Dim cb As ComboBox
    Dim ConexionNota As String
    Dim TotalFacturas As Double
    Dim TotalConceptos As Double
    Dim UltimoNumero As Double = 0
    Dim ImputacionDeOtros As Double = 0
    Dim LetraIva As Integer
    Dim UltimaFechaW As DateTime
    Dim TablaIva(0) As Double
    Dim TipoAsiento As Integer
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim LineasParaImpresion As Integer = 0
    Dim LineasParaImpresionImputacion As Integer = 0
    Private Sub UnReciboReposicion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(8) Then PBloqueaFunciones = True

        Me.Top = 50

        If GCaja = 0 And PNota = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PNota <> 0 Then
            If EsReemplazoCheque(PTipoNota, PNota, PAbierto) Then
                UnChequeReemplazo.PTipoNota = PTipoNota
                UnChequeReemplazo.PNota = PNota
                UnChequeReemplazo.PAbierto = PAbierto
                UnChequeReemplazo.POrigenDestino = 6
                UnChequeReemplazo.PBloqueaFunciones = PBloqueaFunciones
                UnChequeReemplazo.ShowDialog()
                Me.Close()
                Exit Sub
            End If
        End If

        GridCompro.AutoGenerateColumns = False
        GridCompro.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        If PNota = 0 Then
            PideDatosProveedorFondoFijo()
            If ProveedorFondoFijo = 0 Then Me.Close() : Exit Sub
            If FondoFijoCerrado(Numero, PAbierto) Then
                MsgBox("Fondo Fijo cerrado.", MsgBoxStyle.Critical)
                Me.Close()
                Exit Sub
            End If
        End If

        If PAbierto Then
            ConexionNota = Conexion
        Else : ConexionNota = ConexionN
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        ArmaTablaIva(TablaIva)

        ArmaMedioPagoOrdenFondoFijo(DtFormasPago)
        LabelTipoNota.Text = "Reposición"

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

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        LlenaCombosGrid()

        UnTextoParaRecibo.Dispose()

        UltimaFechaW = UltimaFecha(Conexion)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub UnReciboReposicion_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones And Not PImputa Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If FondoFijoCerrado(Numero, PAbierto) Then
            MsgBox("Fondo Fijo cerrado.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        If CInt(LabelCaja.Text) <> GCaja And PNota = 0 Then
            MsgBox("Modificación Solo Permitida para Caja " & LabelCaja.Text & ". Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        GridCompro.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim RowsBusqueda() As DataRow

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy
        Dim DtNotaDetalleAux = DtNotaDetalle.Copy
        Dim DtRendicionesAux As DataTable = DtRendiciones.Copy

        'Actualiza Saldo de Comprobantes Imputados.
        ActualizaComprobantes("M", DtRendicionesAux)

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
                        Row("TipoNota") = PTipoNota
                        Row("Nota") = DtNotaCabeza.Rows(0).Item("Nota")
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
        If GGeneraAsiento And PNota = 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtAsientoCabezaAux, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If PNota = 0 Then
            If HacerAlta(DtNotaCabezaAux, DtRendicionesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux) Then
                ArmaArchivos()
            End If
        Else
            If HacerModificacion(DtNotaCabezaAux, DtRendicionesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux) Then
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

        If FondoFijoCerrado(Numero, PAbierto) Then
            MsgBox("Fondo Fijo cerrado.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If CInt(LabelCaja.Text) <> GCaja Then
            MsgBox("Modificación Solo Permitida para Caja " & LabelCaja.Text & ". Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        If DtNotaCabeza.Rows(0).Item("ChequeRechazado") <> 0 Or DtNotaCabeza.Rows(0).Item("ClaveChequeReemplazado") <> 0 Then
            MsgBox("Anulación Nota Por Cheque Rechazado o Reemplazado, Debe Realizarce Por Menu Rechazo de Cheques. Operación se CANCELA.")
            Exit Sub
        End If

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy
        Dim DtMedioPagoAux As DataTable = DtMedioPago.Copy

        'Actualiza Saldo de Comprobantes Imputados.
        Dim DtRendicionesAux As DataTable = DtRendiciones.Copy

        ActualizaComprobantes("M", DtRendicionesAux)

        If Not IsNothing(DtRendicionesAux.GetChanges) Then
            MsgBox("Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Resul As Double

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        For Each Row As DataRow In DtGrid.Rows
            If Row("MedioPago") = 2 Or Row("Mediopago") = 3 Then
                Dim Anulado As Boolean, Rechazado As Boolean, Depositado As Boolean, Entregado As Boolean, Afectado As Boolean
                If Not EstadoCheque(Row("Mediopago"), Row("ClaveCheque"), ConexionNota, Anulado, Rechazado, Depositado, Entregado, Afectado) Then
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
                If ChequeReemplazado(Row("MedioPago"), Row("ClaveCheque"), PTipoNota, PNota, ConexionNota) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Cheque " & Row("Numero") & " no se puede Borrar, fue Reemplazado. Operación se CANCELA.")
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
            If Row("MedioPago") = 3 Then
                If ExiteChequeEnPaseCaja(ConexionNota, Row("ClaveCheque")) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Cheque " & Row("Numero") & " en Proceso de Pase de Caja. Operación se CANCELA.")
                    Exit Sub
                End If
            End If
        Next

        If MsgBox("Nota se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        'Actualiza Saldo de Comprobantes Imputados.
        DtRendicionesAux = DtRendiciones.Copy

        ActualizaComprobantes("B", DtRendicionesAux)

        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable
        '
        If Not HallaAsientosCabeza(TipoAsiento, DtNotaCabeza.Rows(0).Item("Nota"), DtAsientoCabezaAux, ConexionNota) Then Exit Sub
        For Each Row As DataRow In DtAsientoCabezaAux.Rows
            Row("Estado") = 3
        Next

        DtNotaCabezaAux.Rows(0).Item("Estado") = 3

        Resul = ActualizaNota("B", DtGrid, DtNotaCabezaAux, DtRendicionesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux)
        If Resul = -1 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Baja Fue Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonMediosDePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMediosDePago.Click

        'No permite informar en grid si total orden de compra no esta informado.  
        If TextTotalOrden.Text = "" Then
            MsgBox("Falta Informar Importe de la Reposición.")
            TextTotalOrden.Focus()
            Exit Sub
        End If

        UnMediosPago.PTipoNota = PTipoNota
        UnMediosPago.PDtGrid = DtGrid
        UnMediosPago.PAbierto = PAbierto
        If PNota = 0 Then
            UnMediosPago.PBloqueaFunciones = False
        Else : UnMediosPago.PBloqueaFunciones = True
        End If
        UnMediosPago.PDtFormasPago = DtFormasPago
        UnMediosPago.PDtRetencionProvincia = New DataTable
        If DtNotaCabeza.Rows(0).Item("ChequeRechazado") <> 0 Then UnMediosPago.PEsChequeRechazado = True
        UnMediosPago.PEsExterior = False
        UnMediosPago.PDtRetencionesAutomaticas = New DataTable
        UnMediosPago.PDiferenciaDeCambio = False
        UnMediosPago.PMoneda = 1
        UnMediosPago.PCambio = 0
        UnMediosPago.PImporte = CDbl(TextTotalRecibo.Text)
        If TextTotalOrden.Text <> "" Then
            UnMediosPago.PImporteAInformar = CDbl(TextTotalOrden.Text)
        End If
        UnMediosPago.PEsTr = False
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
    Private Sub ButtonTextoRecibo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTextoRecibo.Click

        UnTextoParaRecibo.ShowDialog()

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Nota debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ErrorImpresion = False
        Paginas = 0
        If Not PAbierto Then Copias = 1

        Dim print_document As New PrintDocument

        LineasParaImpresion = 0
        LineasParaImpresionImputacion = 0
        AddHandler print_document.PrintPage, AddressOf Print_PrintOrdenPago
        print_document.Print()

        If PAbierto Then
            If Not GrabaImpreso(DtNotaCabeza, Conexion) Then Exit Sub
            DtNotaCabeza.Rows(0).Item("Impreso") = True
        Else
            If Not GrabaImpreso(DtNotaCabeza, ConexionN) Then Exit Sub
            DtNotaCabeza.Rows(0).Item("Impreso") = True
        End If

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PNota = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = TipoAsiento
        If PAbierto Then
            ListaAsientos.PDocumentoB = PNota
        Else
            ListaAsientos.PDocumentoN = PNota
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If FondoFijoCerrado(Numero, PAbierto) Then
            MsgBox("Fondo Fijo cerrado.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        PNota = 0
        UnReciboReposicion_Load(Nothing, Nothing)
        TextTotalOrden.Text = ""
        TextTotalOrden.Focus()

    End Sub
    Private Sub TextTotalOrden_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextTotalOrden.KeyPress

        EsNumerico(e.KeyChar, TextTotalOrden.Text, GDecimales)

    End Sub
    Private Function ArmaArchivos() As Boolean                     'ArmaArchivos

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()
        CreaDtGridCompro()

        Dim Sql As String
        Dim Row1 As DataRow
        Dim RowsBusqueda() As DataRow

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM RecibosCabeza WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaCabeza) Then Return False
        If PNota <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then AgregaCabeza()

        Numero = DtNotaCabeza.Rows(0).Item("NumeroFondoFijo")
        ProveedorFondoFijo = DtNotaCabeza.Rows(0).Item("Emisor")
        TextNombreFondoFijo.Text = NombreProveedorFondoFijo(ProveedorFondoFijo)
        TextNumero.Text = Numero

        MuestraCabeza()

        DtNotaDetalle = New DataTable
        Sql = "SELECT * FROM RecibosDetalle WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaDetalle) Then Return False

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM RecibosDetallePago WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
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
            Row1("Detalle") = Row("Detalle")
            Row1("Alicuota") = Row("Alicuota")
            Row1("Neto") = Row("Neto")
            Row1("ImporteIva") = Row("Neto") * Row("Alicuota") / 100
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
                Else
                    Row1("Banco") = 0
                    Row1("Cuenta") = 0
                    Row1("Serie") = ""
                    Row1("Numero") = 0
                    Row1("EmisorCheque") = ""
                    Row1("Fecha") = "1/1/1800"
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
        DtRendiciones = New DataTable

        If Not ArmaConRendiciones() Then Return False

        'Procesa Reposicines.
        DtGridCompro.Clear()
        For Each Row As DataRow In DtRendiciones.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 7003
            Row1("TipoVisible") = 7003
            Row1("Comprobante") = Row("Rendicion")
            Row1("Recibo") = Row("Rendicion")
            Row1("Fecha") = Row("Fecha")
            Row1("Importe") = Row("Importe")
            Row1("Moneda") = 1
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next

        For Each Row As DataRow In DtNotaDetalle.Rows
            RowsBusqueda = DtGridCompro.Select("Tipo = " & Row("TipoComprobante") & " AND Comprobante = " & Row("Comprobante"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Asignado") = Row("Importe")
            End If
        Next

        'Borra los documentos con saldo 0 y no tienen asignacion. 
        DtGridCompro.AcceptChanges()
        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Saldo") = 0 And Row("Asignado") = 0 Then Row.Delete()
        Next
        DtGridCompro.AcceptChanges()
        ArmaGridCompro()

        TipoAsiento = 7205

        If PNota <> 0 Then
            ButtonAceptar.Text = "Modificar Recibo"
            LabelPuntoDeVenta.Visible = False
            Panel3.Enabled = False
            ButtonEliminarTodo.Enabled = False
        Else
            ButtonAceptar.Text = "Grabar Recibo"
            LabelPuntoDeVenta.Visible = True
            Panel3.Enabled = True
            ButtonEliminarTodo.Enabled = True
        End If

        If DtNotaCabeza.Rows(0).Item("Estado") = 3 Then
            GridCompro.ReadOnly = True
        Else : GridCompro.ReadOnly = False
        End If

        CalculaTotales()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Function ArmaConRendiciones() As Boolean

        Dim Sql As String = "SELECT * FROM RendicionFondoFijo WHERE Cerrado = 0 AND Numero = " & Numero & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtRendiciones) Then Return False

        Return True

    End Function
    Private Sub ArmaGridCompro()

        GridCompro.Rows.Clear()

        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Asignado") <> 0 Then
                GridCompro.Rows.Add(Row("Operacion"), "", Row("Tipo"), Row("TipoVisible"), Row("Comprobante"), Row("Recibo"), Row("Comentario"), Row("Fecha"), Row("Importe"), Row("Moneda"), Row("Saldo"), Row("Asignado"))
            End If
        Next

    End Sub
    Private Sub ButtonComprobantesAImputar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonComprobantesAImputar.Click

        UNComprobanteAImputar.PDtGridCompro = DtGridCompro
        UNComprobanteAImputar.PTipo = PTipoNota
        UNComprobanteAImputar.PAbierto = PAbierto
        UNComprobanteAImputar.PTotalConceptos = TextTotalRecibo.Text
        UNComprobanteAImputar.PTipoIva = 0
        UNComprobanteAImputar.PMoneda = 1
        UNComprobanteAImputar.PCambio = 1
        UNComprobanteAImputar.ShowDialog()
        DtGridCompro = UNComprobanteAImputar.PDtGridCompro
        UNComprobanteAImputar.Dispose()

        ArmaGridCompro()
        CalculaTotales()

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtNotaCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Nota")
        AddHandler Enlace.Format, AddressOf FormatMaskedNota
        AddHandler Enlace.Parse, AddressOf ParseMaskedNota
        MaskedNota.DataBindings.Clear()
        MaskedNota.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NumeroFondoFijo")
        TextNumero.DataBindings.Clear()
        TextNumero.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Secos")
        CheckSecos.DataBindings.Clear()
        CheckSecos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalRecibo.DataBindings.Clear()
        TextTotalRecibo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Saldo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextSaldo.DataBindings.Clear()
        TextSaldo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Dim Row As DataRowView = MiEnlazador.Current

        If Row("Importe") <> 0 Then
            TextTotalOrden.Text = FormatNumber(Row("Importe"))
        End If

    End Sub
    Private Sub FormatMaskedNota(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "000000000000")

    End Sub
    Private Sub ParseMaskedNota(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatCambio(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, 3)

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos
        Dim RowsBusqueda() As DataRow

        If Funcion = "A" Then
            For Each Row As DataRow In DtGrid.Rows
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                If RowsBusqueda(0).Item("Tipo") <> 4 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("MedioPago")
                    If Row("Cambio") <> 0 Then
                        Item.Importe = Trunca(Row("Cambio") * Row("Importe"))
                    Else : Item.Importe = Row("Importe")
                    End If
                    ListaConceptos.Add(Item)
                End If
            Next
            For Each Row As DataRow In DtGrid.Rows
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                If RowsBusqueda(0).Item("Tipo") = 4 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("MedioPago")
                    Item.Importe = Row("Importe")
                    If PTipoNota = 60 Then
                        Item.TipoIva = 9        'Credito fiscal.
                    Else : Item.TipoIva = 11    'Debito fiscal. 
                    End If
                    ListaRetenciones.Add(Item)
                End If
            Next
            Item = New ItemListaConceptosAsientos
            Item.Clave = 213
            Item.Importe = CDbl(TextTotalRecibo.Text)
            ListaConceptos.Add(Item)
        End If

        Dim Fecha As Date = DateTime1.Value

        If Not Asiento(TipoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Fecha, 0) Then Return False

        Return True

    End Function
    Private Sub AgregaCabeza()

        Dim Row As DataRow

        Row = DtNotaCabeza.NewRow
        ArmaNuevoRecibo(Row)
        Row("TipoNota") = PTipoNota
        Row("Emisor") = ProveedorFondoFijo
        Row("NumeroFondoFijo") = Numero
        Row("Fecha") = Now
        Row("CodigoIva") = 0
        Row("Estado") = 1
        Row("Caja") = GCaja
        Row("Moneda") = 1
        Row("Cambio") = 1
        DtNotaCabeza.Rows.Add(Row)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Dt As New DataTable

        Dim Codigo As New DataColumn("Codigo")
        Codigo.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Codigo)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        Dt.Columns.Add(Nombre)

        Dim Row As DataRow = Dt.NewRow()
        Row("Codigo") = 7003
        Row("Nombre") = "Rendición"
        Dt.Rows.Add(Row)

        Tipo.DataSource = Dt
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Codigo"

        Dim Dt2 As DataTable = Dt.Copy
        TipoVisible.DataSource = Dt2
        TipoVisible.DisplayMember = "Nombre"
        TipoVisible.ValueMember = "Codigo"

        Moneda.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 27 order By Nombre;")
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 1
        Row("Nombre") = "PESOS"
        Moneda.DataSource.Rows.Add(Row)
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Moneda.DataSource.Rows.Add(Row)
        Moneda.DisplayMember = "Nombre"
        Moneda.ValueMember = "Clave"

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

        Dim TipoVisible As New DataColumn("TipoVisible")
        TipoVisible.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(TipoVisible)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Comprobante)

        Dim Comentario As New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGridCompro.Columns.Add(Comentario)

        Dim Recibo As New DataColumn("Recibo")
        Recibo.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Recibo)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGridCompro.Columns.Add(Fecha)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Importe)

        Dim Moneda As New DataColumn("Moneda")
        Moneda.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Moneda)

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
    Private Function HacerAlta(ByVal DtNotaCabezaAux As DataTable, ByVal DtRendicionesAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroNota As Double
        Dim NumeroAsiento As Double
        Dim NumeroW As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion comprobante.
            NumeroNota = UltimaNumeracionPagoYOrden(PTipoNota, ConexionNota)
            '
            DtNotaCabezaAux.Rows(0).Item("TipoNota") = PTipoNota
            DtNotaCabezaAux.Rows(0).Item("Nota") = NumeroNota
            DtNotaCabezaAux.Rows(0).Item("Interno") = NumeroInternoRecibos(LetraIva, PTipoNota, ConexionNota)
            If DtNotaCabezaAux.Rows(0).Item("Interno") <= 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            For Each Row As DataRow In DtNotaDetalleAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("TipoNota") = PTipoNota
                    Row("Nota") = NumeroNota
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

            Dim DtGridAux As New DataTable
            DtGridAux = DtGrid.Copy
            DtMedioPagoAux.Rows.Clear()

            For Each Row As DataRow In DtGridAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    If PTipoNota = 600 And HallaTipo(Row("MedioPago")) = 4 Then
                        Row("FechaComprobante") = DateTime1.Value
                        Row("Comprobante") = UltimaNumeracionRetenciones(Row("MedioPago"))
                        If Row("Comprobante") <= 0 Then
                            MsgBox("Error Base de Datos al leer Tabla de Retenciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            Return False
                        End If
                    End If
                End If
            Next

            NumeroW = ActualizaNota("A", DtGridAux, DtNotaCabezaAux, DtRendicionesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -10 Then
            MsgBox("Recibo Ya Fue Impreso. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function HacerModificacion(ByVal DtNotaCabezaAux As DataTable, ByVal DtRendicionesAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable) As Boolean

        'Graba Facturas.
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
                DtAsientoCabezaAux.Rows(0).Item("Documento") = DtNotaCabezaAux.Rows(0).Item("Nota")
                For Each Row As DataRow In DtAsientoDetalleAux.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            Resul = ActualizaNota("M", DtGrid, DtNotaCabezaAux, DtRendicionesAux, DtMedioPagoAux, DtNotaDetalleAux, DtAsientoCabezaAux, DtAsientoDetalleAux)

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
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function ActualizaNota(ByVal Funcion As String, ByVal DtGridAux As DataTable, ByVal DtNotaCabezaAux As DataTable, ByVal DtRendicionesAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                Resul = ActualizaRecibo(DtFormasPago, Funcion, DtGridAux, DtNotaCabezaAux, DtMedioPagoAux, DtNotaDetalleAux, ConexionNota, False)
                If Resul <= 0 Then Return Resul
                '
                If Not IsNothing(DtRendicionesAux.GetChanges) Then
                    Resul = GrabaTabla(DtRendicionesAux.GetChanges, "RendicionFondoFijo", ConexionNota)
                    If Resul <= 0 Then Return 0
                End If
                '
                If Not IsNothing(DtAsientoCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaAux.GetChanges, "AsientosCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleAux.GetChanges, "AsientosDetalle", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return DtNotaCabezaAux.Rows(0).Item("Nota")
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function UltimaNumeracionRetenciones(ByVal Retencion As Integer) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT UltimoNumero FROM Tablas WHERE Tipo = 25 AND Clave = " & Retencion & ";", Miconexion)
                    Return Cmd.ExecuteScalar() + 1
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function ReGrabaUltimaNumeracionRetenciones(ByVal Retencion As Integer, ByVal Numero As Integer) As Boolean

        Dim Sql As String = "UPDATE " & "Tablas" & _
                 " Set UltimoNumero = " & Numero & _
                 " WHERE UltimoNumero = " & Numero - 1 & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                If Resul > 0 Then Return True
            End Using
        Catch ex As Exception
            Return False
        End Try

    End Function
    Private Sub PideDatosProveedorFondoFijo()

        OpcionFondoFijo.ShowDialog()
        If OpcionFondoFijo.PRegresar Then Me.Close() : Exit Sub
        ProveedorFondoFijo = OpcionFondoFijo.PFondoFijo
        Numero = OpcionFondoFijo.PNumeroFondoFijo
        PAbierto = OpcionFondoFijo.PAbierto
        OpcionFondoFijo.Dispose()

        GPuntoDeVenta = HallaPuntoVentaSegunTipo(PTipoNota, 0)
        If GPuntoDeVenta = 0 Then
            MsgBox("No tiene Definido Punto de Venta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ProveedorFondoFijo = 0
        End If
        If Not ExistePuntoDeVenta(GPuntoDeVenta) Then
            MsgBox("Punto de Venta " & GPuntoDeVenta & " No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ProveedorFondoFijo = 0
        End If
        If EsPuntoDeVentaZ(GPuntoDeVenta) Then
            MsgBox("Punto de Venta del Operador " & GPuntoDeVenta & " Reservado Para Ticket Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ProveedorFondoFijo = 0
        End If

        LabelPuntoDeVenta.Text = "Punto de Venta " & Format(GPuntoDeVenta, "0000")

    End Sub
    Private Sub ActualizaComprobantes(ByVal Funcion As String, ByVal DtRendicionesAux As DataTable)

        Dim RowsBusqueda() As DataRow

        'Actualiza Saldo de Comprobantes Imputados.

        If Funcion = "M" Then
            For Each Row1 As DataRow In DtGridCompro.Rows
                If Row1.RowState <> DataRowState.Deleted Then
                    If Row1("Saldo") <> Row1("SaldoAnt") Then
                        RowsBusqueda = DtRendicionesAux.Select("Rendicion = " & Row1("Comprobante"))
                        RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
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
                        RowsBusqueda = DtRendicionesAux.Select("Rendicion = " & Row1("Comprobante"))
                        RowsBusqueda(0).Item("Saldo") = Trunca(RowsBusqueda(0).Item("Saldo") + Row1.Item("Asignado"))
                    End If
                End If
            Next
        End If

    End Sub
    Private Function HallaTipo(ByVal MedioPago As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtFormasPago.Select("Clave = " & MedioPago)
        Return RowsBusqueda(0).Item("Tipo")

    End Function
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM RecibosCabeza;", Miconexion)
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
    Private Function EsReciboManual(ByVal PuntoVenta As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT EsReciboManual FROM PuntosDeVenta WHERE Clave = " & PuntoVenta & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Punto de Venta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Private Sub CalculaTotales()

        TotalConceptos = 0
        For Each Row As DataRow In DtGrid.Rows
            If Row("Cambio") = 0 Then
                TotalConceptos = TotalConceptos + Row("Importe")
            Else : TotalConceptos = Trunca(TotalConceptos + Trunca(Row("Cambio") * Row("Importe")))
            End If
        Next

        If DtNotaCabeza.Rows(0).Item("Importe") = 0 And PNota <> 0 Then
            TotalConceptos = 0
        End If

        TextTotalRecibo.Text = FormatNumber(TotalConceptos, GDecimales)

        TotalFacturas = 0
        For Each Row As DataRow In DtGridCompro.Rows
            If Row.RowState <> DataRowState.Deleted Then
                TotalFacturas = Trunca(TotalFacturas + Row("Asignado"))
            End If
        Next

        TextTotalFacturas.Text = FormatNumber(TotalFacturas, GDecimales)

        TextSaldo.Text = FormatNumber(CDbl(TextTotalRecibo.Text) - ImputacionDeOtros - TotalFacturas, GDecimales)

    End Sub
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

        Dim Imputaciones As Integer = CantidadImputaciones()

        Try
            Print_TituloOrdenPago(e, MIzq, MTop)

            If LineasParaImpresion < DtGrid.Rows.Count Then
                Print_MediosPago(e, MIzq, MTop, UltimaLinea, LineasImpresas)
                'Imprime imputaciones.
                If Imputaciones > 0 And LineasParaImpresion >= DtGrid.Rows.Count And LineasImpresas < 27 Then
                    Print_Imputaciones(e, MIzq, UltimaLinea + 10, 37 - LineasImpresas - 4)
                End If
            Else
                If Imputaciones > 0 Then
                    Print_Imputaciones(e, MIzq, MTop + 50, 37)
                End If
            End If

            Print_FinalOrdenPago(e, MIzq, MTop)

            If (LineasParaImpresion < DtGrid.Rows.Count) Or (Imputaciones > 0 And LineasParaImpresionImputacion < GridCompro.Rows.Count) Then
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
    Private Function CantidadImputaciones() As Integer

        Dim Contador As Integer = 0

        For Each Row As DataGridViewRow In GridCompro.Rows
            If Row.Cells("Asignado").Value <> 0 Then Contador = Contador + 1
        Next

        Return Contador

    End Function
    Private Sub Print_TituloOrdenPago(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

        Dim Texto As String = ""

        'Encabezado.
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
        Texto = GNombreEmpresa
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
        Texto = "REPOSICION"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 140, MTop)
        ' 
        PrintFont = New Font("Courier New", 12)
        Texto = "Nro. Reposición:  " & NumeroEditado(MaskedNota.Text)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
        Texto = "Fecha:  " & DateTime1.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
        Texto = "Fondo Fijo: " & TextNombreFondoFijo.Text & "  Numero: " & TextNumero.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 27)
        Texto = "Importe Reposición: " & TextTotalRecibo.Text
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
                'Imprime Vencimiento.
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
    Private Sub Print_Imputaciones(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer, ByVal LineasPorPagina As Integer)

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim Contador As Integer = 0
        Dim PrintFont As System.Drawing.Font

        Dim Longi As Integer
        Dim Xq As Integer
        Dim Yq As Integer
        Dim Ancho As Integer

        'Grafica -Rectangulo Imputacion. ----------------------------------------------------------------------
        y = MTop
        x = MIzq
        Ancho = 183
        PrintFont = New Font("Courier New", 10)

        Dim LineaTipo As Integer = x + 35
        Dim LineaComprobante As Integer = x + 69
        Dim LineaFecha As Integer = x + 94
        Dim LineaImporte1 As Integer = x + 125
        Dim LineaSaldo As Integer = x + 155
        Dim LineaImporte2 As Integer = x + Ancho
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

        'Descripcion Imputacion.
        Yq = y + SaltoLinea
        While Contador < LineasPorPagina And LineasParaImpresionImputacion < GridCompro.Rows.Count
            Dim Row As DataGridViewRow = GridCompro.Rows(LineasParaImpresionImputacion)
            If Row.Cells("Asignado").Value <> 0 Then
                Yq = Yq + SaltoLinea
                'Imprime Tipo.
                Texto = Row.Cells("Tipo").FormattedValue
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Comprobante.
                Texto = Row.Cells("Recibo").FormattedValue
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
                Contador = Contador + 1
            End If
            LineasParaImpresionImputacion = LineasParaImpresionImputacion + 1
        End While

        Yq = Yq + SaltoLinea

        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Yq - y)
        'Lineas vertical.
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaTipo, y, LineaTipo, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaComprobante, y, LineaComprobante, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaFecha, y, LineaFecha, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte1, y, LineaImporte1, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaSaldo, y, LineaSaldo, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte2, y, LineaImporte2, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y + 2 * SaltoLinea, x + Ancho, y + 2 * SaltoLinea)

    End Sub
    Public Function GrabaImpreso(ByVal DtCabezaW As DataTable, ByVal ConexionAux As String) As Boolean

        Dim Sql As String = "UPDATE RecibosCabeza Set Impreso = 1 WHERE TipoNota = " & PTipoNota & " AND Nota = " & DtCabezaW.Rows(0).Item("Nota") & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionAux)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                If CInt(Cmd.ExecuteNonQuery()) = 0 Then
                    MsgBox("Error, Base de Datos al Grabar Nota de Credito. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos al Grabar Nota de Credito. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End Try

        Return True

    End Function
    Private Function EsReemplazoCheque(ByVal TipoNota As Integer, ByVal Nota As Double, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ClaveChequeReemplazado FROM RecibosCabeza WHERE TipoNota = " & TipoNota & " AND Nota = " & Nota & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Proveedores.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function Valida() As Boolean

        Dim PuntoVentaManual As Boolean

        PuntoVentaManual = EsReciboManual(GPuntoDeVenta)

        If PNota = 0 Then
            If PuntoVentaManual Then
                MsgBox("Punto de Venta del Comprobante SOLO Habilitado para Recibo Manual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedNota.Focus()
                Return False
            End If
        End If

        If CDbl(TextTotalRecibo.Text) <> CDbl(TextTotalOrden.Text) Then
            MsgBox("Total Recibo no coincide con Importe de la Orden de Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TotalConceptos = 0 Then
            MsgBox("Debe informar Importes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If CDbl(TextSaldo.Text) <> 0 Then
            MsgBox("Pago a Cuenta No Permitido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TotalConceptos - TotalFacturas < 0 Then
            MsgBox("Importes Imputados supera importe de Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    Public Function FondoFijoCerrado(ByVal Numero As Integer, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cerrado FROM FondosFijos WHERE Numero = " & Numero & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: RendicionFondoFijo. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridCompro_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles GridCompro.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If GridCompro.Columns(e.ColumnIndex).Name = "Recibo" Then
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





End Class
