Imports System.Transactions
Public Class UnReemplazoChequeEnContable
    Public PTipoNota As Integer
    Public PNota As Double
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim DtNotaCabeza As DataTable
    Dim DtMedioPago As DataTable
    Dim DtFormasPago As DataTable
    Dim DtRetencionProvincia As DataTable
    Dim DtGrid As DataTable
    '
    Dim Emisor As Integer
    Dim TienePesos As Boolean
    Dim ConexionNota As String
    Dim TipoAsiento As Integer = 605
    Private Sub UnReemplazoChequeEnContable_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If GCaja = 0 And PNota = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        Grid.AutoGenerateColumns = False

        If PAbierto Then
            ConexionNota = Conexion
        Else : ConexionNota = ConexionN
        End If

        ArmaTrucha(DtFormasPago, PAbierto, PNota)

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

    End Sub
    Private Sub UnReemplazoChequeEnContable_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If GCaja = 0 Then
            MsgBox("Usuario No Tiene Caja.")
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

        If DtNotaCabeza.Rows(0).Item("Estado") = 3 Then
            MsgBox("Nota Esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtMediopagoAux As DataTable = DtMedioPago.Copy

        Dim Importe As Decimal

        ActualizaDetallePagos(DtMediopagoAux, Importe)

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalleAux As New DataTable

        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(TipoAsiento, DtNotaCabeza.Rows(0).Item("Nota"), DtAsientoCabezaAux, ConexionNota) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaAux.Rows(0).Item("Asiento") & ";", ConexionNota, DtAsientoDetalleAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            Dim DtAsientoCabezaWW As DataTable = DtAsientoCabezaAux.Clone
            Dim DtAsientoDetalleWW As DataTable = DtAsientoDetalleAux.Clone
            If Not ArmaArchivosAsiento("A", DtAsientoCabezaWW, DtAsientoDetalleWW, DtMediopagoAux) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            For Each Row As DataRow In DtAsientoDetalleAux.Rows
                Row.Delete()
            Next
            For I As Integer = 0 To DtAsientoDetalleWW.Rows.Count - 1
                DtAsientoDetalleAux.ImportRow(DtAsientoDetalleWW.Rows(I))
            Next
            For Each Row As DataRow In DtAsientoDetalleAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Asiento") = DtAsientoCabezaAux.Rows(0).Item("Asiento")
                End If
            Next
            DtAsientoCabezaWW.Dispose()
            DtAsientoDetalleWW.Dispose()
        End If

        If MsgBox("Importe por " & FormatNumber(Importe, GDecimales) & " en Cheques se cambiaran por Pesos. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        Dim Resul As Double = ActualizaNota("B", DtMediopagoAux, DtAsientoDetalleAux)

        If Resul = -1 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Reemplazo Realizado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        ListaAsientos.PTipoDocumento = TipoAsiento
        If PAbierto Then
            ListaAsientos.PDocumentoB = PNota
        Else
            ListaAsientos.PDocumentoN = PNota
        End If
        ListaAsientos.Show()

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()

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

        MuestraCabeza()

        Emisor = DtNotaCabeza.Rows(0).Item("Emisor")
        TextCliente.Text = NombreCliente(Emisor)

        DtRetencionProvincia = New DataTable
        If PAbierto Then
            If Not Tablas.Read("SELECT * FROM RecibosRetenciones WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";", Conexion, DtRetencionProvincia) Then Return False
        End If

        DtMedioPago = New DataTable
        Sql = "SELECT * FROM RecibosDetallePago WHERE TipoNota = " & PTipoNota & " AND Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtMedioPago) Then Return False

        Dim DtCheques As New DataTable
        For Each Row As DataRow In DtMedioPago.Rows
            If Row("ClaveCheque") <> 0 And (Row("MedioPago") = 2 Or Row("MedioPago") = 3) Then
                If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionN, DtCheques) Then Return False
            End If
        Next

        For Each Row As DataRow In DtMedioPago.Rows
            Row1 = DtGrid.NewRow
            Row1("Sel") = False
            Row1("Item") = Row("Item")
            Row1("MedioPago") = Row("MedioPago")
            Row1("Importe") = Row("Importe")
            Row1("Cambio") = Row("Cambio")
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
            If PAbierto Then
                RowsBusqueda = DtRetencionProvincia.Select("Retencion = " & Row("MedioPago"))
                If RowsBusqueda.Length <> 0 Then Row1("TieneLupa") = True
            End If
            DtGrid.Rows.Add(Row1)
        Next

        DtCheques.Dispose()

        Grid.DataSource = DtGrid

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Concepto").Value <> 2 And Row.Cells("Concepto").Value <> 3 Then
                Row.Cells("Sel").ReadOnly = True
            End If
        Next

        CalculaTotales()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtNotaCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Nota")
        AddHandler Enlace.Format, AddressOf FormatMaskedNota
        MaskedNota.DataBindings.Clear()
        MaskedNota.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        LabelCaja.DataBindings.Clear()
        LabelCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalOrden.DataBindings.Clear()
        TextTotalOrden.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatMaskedNota(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "000000000000")

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub CalculaTotales()

        Dim TotalConceptos As Decimal = 0

        For Each Row As DataRow In DtGrid.Rows
            TotalConceptos = TotalConceptos + Row("Importe")
        Next

    End Sub
    Private Sub ActualizaDetallePagos(ByVal Dt As DataTable, ByRef Importe As Decimal)

        Importe = 0
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid.Rows
            If Row("Sel") Then
                Importe = Importe + Row("Importe")
                RowsBusqueda = Dt.Select("ClaveCheque = " & Row("ClaveCheque"))
                RowsBusqueda(0).Delete()
            End If
        Next

        TienePesos = False
        For Each Row As DataRow In Dt.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row("MedioPago") = 1 Then
                    TienePesos = True
                    Row("Importe") = CDec(Row("Importe")) + Importe
                End If
            End If
        Next

        If Not TienePesos Then
            Dim Row1 As DataRow = Dt.NewRow
            Row1("TipoNota") = DtNotaCabeza.Rows(0).Item("TipoNota")
            Row1("Nota") = DtNotaCabeza.Rows(0).Item("Nota")
            Row1("Item") = 0
            Row1("MedioPago") = 1
            Row1("Detalle") = ""
            Row1("Neto") = 0
            Row1("Alicuota") = 0
            Row1("Cambio") = 0
            Row1("Importe") = Importe
            Row1("Banco") = 0
            Row1("Cuenta") = 0
            Row1("Comprobante") = 0
            Row1("FechaComprobante") = "01/01/1800"
            Row1("ClaveCheque") = 0
            Row1("ClaveInterna") = 0
            Dt.Rows.Add(Row1)
        End If

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Sel As New DataColumn("Sel")
        Sel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Sel)

        Dim Item As New DataColumn("Item")
        Item.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Item)

        Dim MedioPago As New DataColumn("MedioPago")
        MedioPago.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(MedioPago)

        Dim Detalle As New DataColumn("Detalle")
        Detalle.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Detalle)

        Dim Neto As New DataColumn("Neto")
        Neto.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Neto)

        Dim Alicuota As New DataColumn("Alicuota")
        Alicuota.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Alicuota)

        Dim ImporteIva As New DataColumn("ImporteIva")
        ImporteIva.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ImporteIva)

        Dim Cambio As New DataColumn("Cambio")
        Cambio.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cambio)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim Banco As New DataColumn("Banco")
        Banco.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Banco)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cuenta)

        Dim Serie As New DataColumn("Serie")
        Serie.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Serie)

        Dim Numero As New DataColumn("Numero")
        Numero.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Numero)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim EmisorCheque As New DataColumn("EmisorCheque")
        EmisorCheque.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(EmisorCheque)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante)

        Dim FechaComprobante As New DataColumn("FechaComprobante")
        FechaComprobante.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaComprobante)

        Dim ClaveCheque As New DataColumn("ClaveCheque")
        ClaveCheque.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ClaveCheque)

        Dim ClaveInterna As New DataColumn("ClaveInterna")
        ClaveInterna.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ClaveInterna)

        Dim ClaveChequeVisual As New DataColumn("ClaveChequeVisual")
        ClaveChequeVisual.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ClaveChequeVisual)

        Dim NumeracionInicial As New DataColumn("NumeracionInicial")
        NumeracionInicial.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(NumeracionInicial)

        Dim NumeracionFinal As New DataColumn("NumeracionFinal")
        NumeracionFinal.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(NumeracionFinal)

        Dim TieneLupa As New DataColumn("TieneLupa")
        TieneLupa.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(TieneLupa)

        Dim eCheq As New DataColumn("eCheq")
        eCheq.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(eCheq)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

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
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable, ByVal DtMedioPagoAux As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos
        Dim RowsBusqueda() As DataRow

        If Funcion = "A" Then
            For Each Row As DataRow In DtMedioPagoAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
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
                End If
            Next
            For Each Row As DataRow In DtMedioPagoAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                    If RowsBusqueda(0).Item("Tipo") = 4 Then
                        Item = New ItemListaConceptosAsientos
                        Item.Clave = Row("MedioPago")
                        Item.Importe = Row("Importe")
                        Item.TipoIva = 11    'Debito fiscal. 
                        ListaRetenciones.Add(Item)
                    End If
                End If
            Next
            Item = New ItemListaConceptosAsientos
            Item.Clave = 213
            Item.Importe = CDbl(TextTotalOrden.Text)
            If DtNotaCabeza.Rows(0).Item("Moneda") <> 1 Then Item.Importe = Trunca(DtNotaCabeza.Rows(0).Item("Cambio") * Item.Importe)
            ListaConceptos.Add(Item)
        End If
        '
        Dim Fecha As Date = DateTime1.Value 'No tiene importacia pues reemplaza a una anterior.

        If Not Asiento(TipoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Fecha, 0) Then Return False

        Return True

    End Function
    Private Function ActualizaNota(ByVal Funcion As String, ByVal DtMedioPagoAux As DataTable, ByVal DtAsientoDetalleAux As DataTable) As Double

        Dim Resul As Double
        Dim Tr As Boolean = True

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                Dim Resul2 As Integer = 0
                For Each Row As DataRow In DtGrid.Rows
                    If (Row("MedioPago") = 2 Or Row("MedioPago") = 3) And Row("Sel") Then
                        Resul2 = ActualizaClavesComprobantes("B", Row("ClaveCheque"), PTipoNota, Row("MedioPago"), True, Row("Banco"), Row("Cuenta"), Row("Serie"), Row("Numero"), Row("Fecha"), Row("Importe"), Row("EmisorCheque"), DtNotaCabeza.Rows(0).Item("Nota"), "01/01/1800", ConexionN, Tr, False, False)
                        If Resul2 <= 0 Then Return Resul2
                    End If
                Next
                '        
                If Not IsNothing(DtMedioPagoAux.GetChanges) Then
                    Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "RecibosDetallePago", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                '        
                If Not IsNothing(DtAsientoDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleAux.GetChanges, "AsientosDetalle", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function Valida() As Boolean


        Dim Contador As Integer = 0

        For Each Row As DataRow In DtGrid.Rows
            If Row("Sel") Then
                If Row("MedioPago") <> 2 And Row("MedioPago") <> 3 Then
                    MsgBox("Solo Cheques Pueden Ser Reemplazados. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                Contador = Contador + 1
            End If
        Next

        If Contador = 0 Then
            MsgBox("No Hay Cheques a Reemplazar. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cambio" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, 3)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaComprobante" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = "1/1/1800" Then
                    e.Value = ""
                Else : e.Value = Format(e.Value, "dd/MM/yyyy")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Or Grid.Columns(e.ColumnIndex).Name = "Comprobante" Or Grid.Columns(e.ColumnIndex).Name = "Cuenta" Or Grid.Columns(e.ColumnIndex).Name = "ClaveChequeVisual" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, 0)
            End If
        End If

    End Sub

End Class