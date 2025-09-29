Imports System.Transactions
Imports System.Drawing.Printing
Imports System.Math
Public Class UnaLiquidacionInsumos
    Public PLiquidacion As Double
    Public PProveedor As Integer
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim ListaDeLotes As List(Of FilaComprobanteFactura)
    Dim ListaDeRetenciones As New List(Of ItemIvaReten)
    '
    Dim DtLiquidacionCabezaB As DataTable
    Dim DtLiquidacionDetalleB As DataTable
    Dim DtLiquidacionConceptosB As DataTable
    Dim DtLiquidacionCabezaN As DataTable
    Dim DtLiquidacionDetalleN As DataTable
    Dim DtLiquidacionConceptosN As DataTable
    Dim DtGrid As DataTable
    Dim DtGridCompro As DataTable
    '
    Dim TablaIva(0) As Double
    Dim IvaW As Decimal
    Dim TotalImporteB As Decimal
    Dim TotalImporteN As Decimal
    Dim TotalGrabadoW As Decimal
    Dim AlicuotaProvincia As Double = 0
    Dim UltimoNumero As Double
    Dim ConexionLiquidacion As String
    Dim UltimaFechaW As DateTime
    Dim UltimaFechaContableW As DateTime
    Dim Provincia As Integer
    Dim TipoFactura As Integer
    Dim Calle As String
    Dim Localidad As String
    Dim RetencionManual As Boolean
    Dim ProveedorOpr As Boolean
    'para imprimir.
    Dim LiquidacionAImprimir As Double
    Dim AbiertoParaImpresion As Boolean
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Private Sub UnaLiquidacionConOrdenDeCompra_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Top = 50

        If Not PermisoEscritura(6) Then PBloqueaFunciones = True

        If PLiquidacion = 0 Then
            Opciones()
            If PProveedor = 0 Then Me.Close() : Exit Sub
        End If

        GridCompro.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        Grid.AutoGenerateColumns = False

        GridCompro.AutoGenerateColumns = False

        ComboProveedor.DataSource = Nothing
        ComboProveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion <> 4 AND TipoIva <> " & Exterior & ";")
        Dim Row As DataRow = ComboProveedor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboProveedor.DataSource.Rows.Add(Row)
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ArmaTipoIva(ComboTipoIva)
        LlenaComboTablas(ComboPais, 28)

        CreaDtGridCompro()

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        If PAbierto Then
            ConexionLiquidacion = Conexion
        Else
            ConexionLiquidacion = ConexionN
        End If

        LlenaCombosGrid()

        ArmaTablaIva(TablaIva)

        ArmaArchivos()

        GModificacionOk = False

        If Not PermisoTotal Then
            Grid.Columns("ImporteN").Visible = False
            Grid.Columns("ImporteB").MinimumWidth = Grid.Columns("ImporteB").MinimumWidth * 2
            TextTotalN.Visible = False
            GridCompro.Columns("Precio2").Visible = False
            GridCompro.Columns("Importe2").Visible = False
            GridCompro.Columns("Precio").MinimumWidth = GridCompro.Columns("Precio").MinimumWidth * 2
            GridCompro.Columns("Importe").MinimumWidth = GridCompro.Columns("Importe").MinimumWidth * 2
            Panel2.Visible = False
        End If

        UltimaFechaW = UltimaFecha(Conexion)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        UltimaFechaContableW = UltimaFechacontableLiquidacion(Conexion, GPuntoDeVenta, TipoFactura)
        If UltimaFechaContableW = "2/1/1000" Then
            MsgBox("Error Base de Datos al Leer Ultima Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

    End Sub
    Private Sub UnaLiquidacionConOrdenDeCompra_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        If PLiquidacion <> 0 Then GridCompro.Focus()

    End Sub
    Private Sub UnaLiquidacionConOrdenDeCompra_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

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
        Grid.EndEdit()
        GridCompro.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtLiquidacionCabezaBAux As DataTable = DtLiquidacionCabezaB.Copy
        Dim DtLiquidacionDetalleBAux As DataTable = DtLiquidacionDetalleB.Copy
        Dim DtLiquidacionConceptosBAux As DataTable = DtLiquidacionConceptosB.Copy
        Dim DtLiquidacionCabezaNAux As DataTable = DtLiquidacionCabezaN.Copy
        Dim DtLiquidacionDetalleNAux As DataTable = DtLiquidacionDetalleN.Copy
        Dim DtLiquidacionConceptosNAux As DataTable = DtLiquidacionConceptosN.Copy
        Dim DtRemitosB As New DataTable
        Dim DtRemitosN As New DataTable

        If PLiquidacion = 0 Then
            If Not ActualizaArchivos("A", DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionConceptosBAux, DtLiquidacionConceptosNAux, DtRemitosB, DtRemitosN) Then Exit Sub
        Else
            If Not ActualizaArchivos("M", DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionConceptosBAux, DtLiquidacionConceptosNAux, DtRemitosB, DtRemitosN) Then Exit Sub
        End If

        'Arma Asientos.
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable
        '
        If GGeneraAsiento And PLiquidacion = 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaB) Then Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleB) Then Exit Sub
            DtAsientoCabezaN = DtAsientoCabezaB.Clone
            DtAsientoDetalleN = DtAsientoDetalleB.Clone
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                If Not ArmaArchivosAsiento("A", DtAsientoCabezaB, DtAsientoDetalleB, 1) Then Exit Sub
            End If
            If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then
                If Not ArmaArchivosAsiento("A", DtAsientoCabezaN, DtAsientoDetalleN, 2) Then Exit Sub
            End If
        End If

        If IsNothing(DtLiquidacionCabezaBAux.GetChanges) And IsNothing(DtLiquidacionCabezaNAux.GetChanges) And _
           IsNothing(DtLiquidacionDetalleBAux.GetChanges) And IsNothing(DtLiquidacionDetalleNAux.GetChanges) And _
           IsNothing(DtLiquidacionConceptosBAux.GetChanges) And IsNothing(DtLiquidacionConceptosNAux.GetChanges) Then
            MsgBox("NO Hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PLiquidacion = 0 Then
            HacerAlta(DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionConceptosBAux, DtLiquidacionConceptosNAux, DtRemitosB, DtRemitosN, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN)
        Else
            Dim Resul As Double = ActualizaFactura(DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionConceptosBAux, DtLiquidacionConceptosNAux, DtRemitosB, DtRemitosN, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN)
            If Resul = 0 Then
                MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End If
            If Resul < 0 Then
                MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End If
            If Resul > 0 Then
                MsgBox("Modificacion Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                GModificacionOk = True
                ArmaArchivos()
            End If
        End If

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PLiquidacion = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            If DtLiquidacionCabezaB.Rows(0).Item("Rel") And Not PermisoTotal Then
                MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("ERROR, Liquidación Ya esta Anulada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            If DtLiquidacionCabezaB.Rows(0).Item("Saldo") <> DtLiquidacionCabezaB.Rows(0).Item("Neto") Then
                MsgBox("Liquidación fue Imputada en la Cuenta Corriente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If
        If DtLiquidacionCabezaN.Rows.Count <> 0 Then
            If DtLiquidacionCabezaN.Rows(0).Item("Saldo") <> DtLiquidacionCabezaN.Rows(0).Item("Neto") Then
                MsgBox("Liquidación fue Imputada en la Cuenta Corriente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If CierreContableCerrado(CDate(TextFechaContable.Text).Month, CDate(TextFechaContable.Text).Year) Then
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Exit Sub
        End If

        Dim DtLiquidacionCabezaBAux As DataTable = DtLiquidacionCabezaB.Copy
        Dim DtLiquidacionDetalleBAux As DataTable = DtLiquidacionDetalleB.Copy
        Dim DtLiquidacionConceptosBAux As DataTable = DtLiquidacionConceptosB.Copy
        Dim DtLiquidacionCabezaNAux As DataTable = DtLiquidacionCabezaN.Copy
        Dim DtLiquidacionDetalleNAux As DataTable = DtLiquidacionDetalleN.Copy
        Dim DtLiquidacionConceptosNAux As DataTable = DtLiquidacionConceptosN.Copy
        Dim DtRemitosB As New DataTable
        Dim DtRemitosN As New DataTable

        'Anula asiento.
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable

        If GGeneraAsiento Then
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(915, DtLiquidacionCabezaBAux.Rows(0).Item("Liquidacion"), DtAsientoCabezaB, Conexion) Then Exit Sub
            End If
            If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(915, DtLiquidacionCabezaNAux.Rows(0).Item("Liquidacion"), DtAsientoCabezaN, ConexionN) Then Exit Sub
            End If
            If DtAsientoCabezaB.Rows.Count <> 0 Then DtAsientoCabezaB.Rows(0).Item("Estado") = 3
            If DtAsientoCabezaN.Rows.Count <> 0 Then DtAsientoCabezaN.Rows(0).Item("Estado") = 3
        End If

        If MsgBox("Liquidación se Anulara. ¿Desea Anularla?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        If Not ActualizaArchivos("B", DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionConceptosBAux, DtLiquidacionConceptosNAux, DtRemitosB, DtRemitosN) Then Exit Sub

        Dim Resul As Double = ActualizaFactura(DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionConceptosBAux, DtLiquidacionConceptosNAux, DtRemitosB, DtRemitosN, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN)
        '
        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -2 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Liquidación Anulada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PLiquidacion = 0
        UnaLiquidacionConOrdenDeCompra_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonNuevaIgualCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevaIgualCliente.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PLiquidacion = 0
        ArmaArchivos()

    End Sub
    Private Sub CheckRetencionManual_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckRetencionManual.CheckedChanged

        If CheckRetencionManual.Checked Then
            RetencionManual = True
        Else
            RetencionManual = False
            CalculaTotal()
        End If

        PresentaGrid1Retenciones()
        Grid.Refresh()

    End Sub
    Private Sub ButtonImprimeB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimeB.Click
        'http://vb-helper.com/howto_net_print_and_preview.html

        AbiertoParaImpresion = True
        LiquidacionAImprimir = DtLiquidacionCabezaB.Rows(0).Item("Liquidacion")

        ButtonImprimir_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonImprimeN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimeN.Click
        'http://vb-helper.com/howto_net_print_and_preview.html

        AbiertoParaImpresion = False
        LiquidacionAImprimir = DtLiquidacionCabezaN.Rows(0).Item("Liquidacion")

        ButtonImprimir_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'http://vb-helper.com/howto_net_print_and_preview.html

        If PLiquidacion = 0 Then
            MsgBox("Opcion Invalida. Liquidación debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If AbiertoParaImpresion Then
            If DtLiquidacionCabezaB.Rows(0).Item("Impreso") Then
                If MsgBox("Liquidación Ya fue Impresa. Quiere Re-Imprimir? ", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then Exit Sub
            End If
        Else
            If DtLiquidacionCabezaN.Rows(0).Item("Impreso") Then
                If MsgBox("Liquidación Ya fue Impresa. Quiere Re-Imprimir? ", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then Exit Sub
            End If
        End If

        ErrorImpresion = False
        Paginas = 0
        If AbiertoParaImpresion Then Copias = 2
        If Not AbiertoParaImpresion Then Copias = 1

        Dim print_document As New PrintDocument
        AddHandler print_document.PrintPage, AddressOf Print_PrintPage

        print_document.Print()

        If ErrorImpresion Then Exit Sub

        If Not GrabaImpreso(LiquidacionAImprimir, AbiertoParaImpresion) Then Exit Sub

        If AbiertoParaImpresion Then
            DtLiquidacionCabezaB.Rows(0).Item("Impreso") = True
        Else : DtLiquidacionCabezaN.Rows(0).Item("Impreso") = True
        End If

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PLiquidacion = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 915
        If DtLiquidacionCabezaB.Rows.Count <> 0 Then ListaAsientos.PDocumentoB = DtLiquidacionCabezaB.Rows(0).Item("Liquidacion")
        If DtLiquidacionCabezaN.Rows.Count <> 0 Then ListaAsientos.PDocumentoN = DtLiquidacionCabezaN.Rows(0).Item("Liquidacion")
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonNetoPorLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNetoPorLotes.Click

        If PLiquidacion = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        SeleccionarVarios.PEsNetoPorLiquidacionInsumos = True
        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            SeleccionarVarios.PLiquidacion = DtLiquidacionCabezaB.Rows(0).Item("Liquidacion")
        End If
        If DtLiquidacionCabezaN.Rows.Count <> 0 Then
            SeleccionarVarios.PLiquidacion2 = DtLiquidacionCabezaN.Rows(0).Item("Liquidacion")
        End If
        SeleccionarVarios.ShowDialog()
        SeleccionarVarios.Dispose()

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueContable.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureLupa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupa.Click

        ListaDeLotes = New List(Of FilaComprobanteFactura)
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGridCompro.Rows
            Dim Item As New FilaComprobanteFactura
            Item.Operacion = Row("Operacion")
            Item.Ingreso = Row("Ingreso")
            ListaDeLotes.Add(Item)
        Next

        SeleccionarVarios.PEsLiquidacionInsumos = True
        SeleccionarVarios.PEmisor = PProveedor
        SeleccionarVarios.PListaDeLotes = ListaDeLotes
        SeleccionarVarios.ShowDialog()

        If Not HuboCambio(SeleccionarVarios.PListaDeLotes, DtGridCompro) Then
            SeleccionarVarios.Dispose()
            Exit Sub
        End If

        If SeleccionarVarios.PListaDeLotes.Count = 0 Then
            SeleccionarVarios.Dispose()
            Exit Sub
        End If
        '
        TotalImporteB = 0 : TotalImporteN = 0
        For Each Fila As FilaComprobanteFactura In SeleccionarVarios.PListaDeLotes
            TotalImporteB = Trunca(TotalImporteB + Fila.ImporteB)
            TotalImporteN = Trunca(TotalImporteN + Fila.ImporteN)
        Next
        TextTeoricoB.Text = FormatNumber(TotalImporteB, GDecimales)
        TextTeoricoN.Text = FormatNumber(TotalImporteN, GDecimales)
        '
        Dim Dt As DataTable = DtGridCompro.Copy
        For Each Fila As FilaComprobanteFactura In SeleccionarVarios.PListaDeLotes
            RowsBusqueda = Dt.Select("Operacion = " & Fila.Operacion & " AND Ingreso = " & Fila.Ingreso)
            If RowsBusqueda.Length = 0 Then
                AgregarArticulos(Fila.OrdenCompra, Fila.Remito, Dt, Fila.Ingreso, Fila.Operacion)  'agrega datos de la orden de compra.
            End If
        Next
        '
        For I As Integer = Dt.Rows.Count - 1 To 0 Step -1
            Dim Row As DataRow = Dt.Rows(I)
            If Not ExisteEnLista(SeleccionarVarios.PListaDeLotes, Row("Operacion"), Row("Ingreso")) Then
                Row.Delete()
            End If
        Next
        '
        IvaW = 0
        If Dt.Rows.Count <> 0 And TotalImporteB <> 0 Then  'verifica que tengan el mismo IVA.
            IvaW = HallaIvaInsumo(Dt.Rows(0).Item("Articulo"))
            '
            For Each Row As DataRow In Dt.Rows
                If IvaW <> HallaIvaInsumo(Row("Articulo")) Then
                    MsgBox("Iva del Insumo " & NombreInsumo(Row("Articulo")) & " difiere a los anteriores.")
                    SeleccionarVarios.Dispose()
                    Exit Sub
                End If
            Next
        End If

        If IvaW <> 0 Then
            TotalGrabadoW = TotalImporteB
        Else
            TotalGrabadoW = 0
        End If

        If Dt.Rows.Count > GLineasPreLiquidacion Then     '28
            MsgBox("ERROR, Supera Cantidad de Insumos permitidos:( " & GLineasPreLiquidacion & " ).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Dt.Dispose()
            Exit Sub
        End If

        DtGridCompro.Clear()
        For Each Row As DataRow In Dt.Rows
            DtGridCompro.ImportRow(Row)
        Next
        Dt.Dispose()

        If TotalImporteB <> 0 Then
            RowsBusqueda = DtGrid.Select("Clave = 3")
            RowsBusqueda(0).Item("Iva") = Format(IvaW, "0.00")
        Else
            For Each Row As DataRow In DtGrid.Rows
                Row("ImporteB") = 0
                Row("Iva") = 0
            Next
        End If

        SeleccionarVarios.Dispose()

        CalculaTotal()

        BloqueaItemsGrid(TotalImporteB, TotalImporteN)

    End Sub
    Private Sub ArmaArchivos()

        DtLiquidacionCabezaB = New DataTable
        DtLiquidacionCabezaN = New DataTable
        DtLiquidacionDetalleB = New DataTable
        DtLiquidacionDetalleN = New DataTable
        DtLiquidacionConceptosB = New DataTable
        DtLiquidacionConceptosN = New DataTable

        If PLiquidacion <> 0 Then
            If PAbierto Then
                If Not Tablas.Read("SELECT * FROM LiquidacionCabeza WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionCabezaB) Then Me.Close() : Exit Sub
                If DtLiquidacionCabezaB.Rows.Count = 0 Then
                    MsgBox("Liquidacion No Existe En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close() : Exit Sub
                End If
                If Not Tablas.Read("SELECT * FROM LiquidacionInsumosDetalle WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionDetalleB) Then Me.Close() : Exit Sub
                If Not Tablas.Read("SELECT * FROM LiquidacionDetalleConceptos WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionConceptosB) Then Me.Close() : Exit Sub
                If DtLiquidacionCabezaB.Rows(0).Item("Rel") And PermisoTotal Then
                    If Not Tablas.Read("SELECT * FROM LiquidacionCabeza WHERE Nrel = " & PLiquidacion & ";", ConexionN, DtLiquidacionCabezaN) Then Me.Close() : Exit Sub
                    If Not Tablas.Read("SELECT * FROM LiquidacionInsumosDetalle WHERE Liquidacion= " & DtLiquidacionCabezaN.Rows(0).Item("Liquidacion") & ";", ConexionN, DtLiquidacionDetalleN) Then Me.Close() : Exit Sub
                    If Not Tablas.Read("SELECT * FROM LiquidacionDetalleConceptos WHERE Liquidacion= " & DtLiquidacionCabezaN.Rows(0).Item("Liquidacion") & ";", ConexionN, DtLiquidacionConceptosN) Then Me.Close() : Exit Sub
                Else
                    DtLiquidacionCabezaN = DtLiquidacionCabezaB.Clone
                    DtLiquidacionDetalleN = DtLiquidacionDetalleB.Clone
                    DtLiquidacionConceptosN = DtLiquidacionConceptosB.Clone
                End If
            Else
                If Not Tablas.Read("SELECT * FROM LiquidacionCabeza WHERE Liquidacion = " & PLiquidacion & ";", ConexionN, DtLiquidacionCabezaN) Then Me.Close() : Exit Sub
                If DtLiquidacionCabezaN.Rows.Count = 0 Then
                    MsgBox("Liquidacion No Existe En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close() : Exit Sub
                End If
                If Not Tablas.Read("SELECT * FROM LiquidacionInsumosDetalle WHERE Liquidacion = " & PLiquidacion & ";", ConexionN, DtLiquidacionDetalleN) Then Me.Close() : Exit Sub
                If Not Tablas.Read("SELECT * FROM LiquidacionDetalleConceptos WHERE Liquidacion = " & PLiquidacion & ";", ConexionN, DtLiquidacionConceptosN) Then Me.Close() : Exit Sub
                If DtLiquidacionCabezaN.Rows(0).Item("Rel") Then
                    If Not Tablas.Read("SELECT * FROM LiquidacionCabeza WHERE Liquidacion = " & DtLiquidacionCabezaN.Rows(0).Item("NRel") & ";", Conexion, DtLiquidacionCabezaB) Then Me.Close() : Exit Sub
                    If Not Tablas.Read("SELECT * FROM LiquidacionInsumosDetalle WHERE Liquidacion = " & DtLiquidacionCabezaB.Rows(0).Item("Liquidacion") & ";", Conexion, DtLiquidacionDetalleB) Then Me.Close() : Exit Sub
                    If Not Tablas.Read("SELECT * FROM LiquidacionDetalleConceptos WHERE Liquidacion = " & DtLiquidacionCabezaB.Rows(0).Item("Liquidacion") & ";", Conexion, DtLiquidacionConceptosB) Then Me.Close() : Exit Sub
                Else
                    DtLiquidacionCabezaB = DtLiquidacionCabezaN.Clone
                    DtLiquidacionDetalleB = DtLiquidacionDetalleN.Clone
                    DtLiquidacionConceptosB = DtLiquidacionConceptosN.Clone
                End If
            End If
        End If

        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            PProveedor = DtLiquidacionCabezaB.Rows(0).Item("Proveedor")
        Else
            If DtLiquidacionCabezaN.Rows.Count <> 0 Then
                PProveedor = DtLiquidacionCabezaN.Rows(0).Item("Proveedor")
            End If
        End If

        If Not LlenaDatosProveedor(PProveedor) Then Me.Close() : Exit Sub

        If PLiquidacion = 0 Then  'debe ir antes de AgregarCabeza.
            TipoFactura = LetrasPermitidasProveedor(HallaTipoIvaProveedor(PProveedor), 100)
            TextTipoFactura.Text = LetraTipoIva(TipoFactura)
            GPuntoDeVenta = HallaPuntoVentaSegunTipo(910, TipoFactura)
            If GPuntoDeVenta = 0 Then
                MsgBox("Usuario No tiene definido Punto de Venta para el Tipo de Iva de este Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close() : Exit Sub
            End If
            If Not ExistePuntoDeVenta(GPuntoDeVenta) Then
                MsgBox("Punto de Venta " & GPuntoDeVenta & " No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close() : Exit Sub
            End If
            If EsPuntoDeVentaZ(GPuntoDeVenta) Then
                MsgBox("Punto de Venta del Operador " & GPuntoDeVenta & " Reservado Para Ticket Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close() : Exit Sub
            End If

            LabelPuntodeVenta.Text = "Punto de Venta " & Format(GPuntoDeVenta, "0000")
        End If

        If PLiquidacion = 0 Then
            If Not Tablas.Read("SELECT * FROM LiquidacionCabeza WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionCabezaB) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM LiquidacionInsumosDetalle WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionDetalleB) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM LiquidacionDetalleConceptos WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionConceptosB) Then Me.Close() : Exit Sub
            DtLiquidacionCabezaN = DtLiquidacionCabezaB.Clone
            DtLiquidacionDetalleN = DtLiquidacionDetalleB.Clone
            DtLiquidacionConceptosN = DtLiquidacionConceptosB.Clone
            '
            AgregaCabeza()
        End If

        ArmaGrid()

        EnlazaCabeza()

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtLiquidacionConceptosB.Rows
            RowsBusqueda = DtGrid.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("ImporteB") = Row("Importe")
                RowsBusqueda(0).Item("Iva") = Row("Valor")
            End If
        Next
        For Each Row As DataRow In DtLiquidacionConceptosN.Rows
            RowsBusqueda = DtGrid.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("ImporteN") = Row("Importe")
                If RowsBusqueda(0).Item("Iva") = 0 Then RowsBusqueda(0).Item("Iva") = Row("Valor")
            End If
        Next

        DtGridCompro.Clear()
        For Each Row As DataRow In DtLiquidacionDetalleB.Rows
            Dim Row1 As DataRow = DtGridCompro.NewRow
            Row1("Indice") = Row("Indice")
            Row1("Ingreso") = Row("Ingreso")
            Dim OrdenCompra As Double, Remito As Double
            HallaDatosIngreso(Row("OPR"), Row("Ingreso"), OrdenCompra, Remito)
            Row1("OrdenCompra") = OrdenCompra
            Row1("Remito") = Remito
            Row1("Operacion") = Row("OPR")
            Row1("Articulo") = Row("Articulo")
            Row1("Cantidad") = Row("Cantidad")
            Row1("PrecioB") = Row("Precio")
            Row1("ImporteB") = Row("Importe")
            Row1("PrecioN") = 0
            Row1("ImporteN") = 0
            DtGridCompro.Rows.Add(Row1)
        Next
        For Each Row As DataRow In DtLiquidacionDetalleN.Rows
            RowsBusqueda = DtGridCompro.Select("Operacion = " & Row("OPR") & " AND Ingreso = " & Row("Ingreso") & " AND Articulo = " & Row("Articulo"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("PrecioN") = Row("Precio")
                RowsBusqueda(0).Item("ImporteN") = Row("Importe")
            Else
                Dim Row1 As DataRow = DtGridCompro.NewRow
                Row1("Indice") = Row("Indice")
                Row1("Ingreso") = Row("Ingreso")
                Dim OrdenCompra As Double, Remito As Double
                HallaDatosIngreso(Row("OPR"), Row("Ingreso"), OrdenCompra, Remito)
                Row1("OrdenCompra") = OrdenCompra
                Row1("Remito") = Remito
                Row1("Operacion") = Row("OPR")
                Row1("Articulo") = Row("Articulo")
                Row1("Cantidad") = Row("Cantidad")
                Row1("PrecioB") = 0
                Row1("ImporteB") = 0
                Row1("PrecioN") = Row("Precio")
                Row1("ImporteN") = Row("Importe")
                DtGridCompro.Rows.Add(Row1)
            End If
        Next

        GridCompro.DataSource = DtGridCompro

        If PLiquidacion = 0 Then
            PictureLupa.Enabled = True
            GridCompro.ReadOnly = False
            Grid.ReadOnly = False
            PanelN.Visible = False
            PanelB.Visible = False
            MaskedLiquidacionB.Text = ""
            MaskedLiquidacionN.Text = ""
            Panel4.Enabled = True
        Else
            If DtLiquidacionCabezaB.Rows.Count <> 0 And DtLiquidacionCabezaN.Rows.Count <> 0 Then
                MaskedLiquidacionB.Text = Strings.Right(DtLiquidacionCabezaB.Rows(0).Item("Liquidacion"), 12)
                MaskedLiquidacionN.Text = Strings.Right(DtLiquidacionCabezaN.Rows(0).Item("Liquidacion"), 12)
                PanelB.Visible = True
                PanelN.Visible = True
            End If
            If DtLiquidacionCabezaB.Rows.Count <> 0 And DtLiquidacionCabezaN.Rows.Count = 0 Then
                MaskedLiquidacionB.Text = Strings.Right(DtLiquidacionCabezaB.Rows(0).Item("Liquidacion"), 12)
                PanelB.Visible = True
                PanelN.Visible = False
                Grid.Columns("ImporteN").ReadOnly = True
            End If
            If DtLiquidacionCabezaB.Rows.Count = 0 And DtLiquidacionCabezaN.Rows.Count <> 0 Then
                MaskedLiquidacionN.Text = Strings.Right(DtLiquidacionCabezaN.Rows(0).Item("Liquidacion"), 12)
                PanelB.Visible = False
                PanelN.Visible = True
                PanelN.Left = PanelB.Left
                Grid.Columns("ImporteB").ReadOnly = True
            End If
            PictureLupa.Enabled = False
            GridCompro.ReadOnly = True
            Grid.ReadOnly = True
            If ComboEstado.SelectedValue = 3 Then
                Panel4.Enabled = False
            End If
            Panel4.Enabled = False
        End If

        ComboProveedor.SelectedValue = PProveedor

        TextTotalB.Text = "0.00"
        TextTotalN.Text = "0.00"

        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            TextTotalB.Text = FormatNumber(DtLiquidacionCabezaB.Rows(0).Item("Neto"), GDecimales)
        End If
        If DtLiquidacionCabezaN.Rows.Count <> 0 Then
            TextTotalN.Text = FormatNumber(DtLiquidacionCabezaN.Rows(0).Item("Neto"), GDecimales)
        End If

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGridCompro.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGridCompro_ColumnChanging)

    End Sub
    Private Sub EnlazaCabeza()

        MiEnlazador = New BindingSource
        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            MiEnlazador.DataSource = DtLiquidacionCabezaB
        Else : MiEnlazador.DataSource = DtLiquidacionCabezaN
        End If

        Dim Row As DataRowView = MiEnlazador.Current

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Liquidacion")
        AddHandler Enlace.Format, AddressOf Formatliquidacion
        AddHandler Enlace.Parse, AddressOf FormatParseLiquidacion
        MaskedLiquidacionB.DataBindings.Clear()
        MaskedLiquidacionB.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Proveedor")
        ComboProveedor.DataBindings.Clear()
        ComboProveedor.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Parse, AddressOf FormatParseString
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        If Row("FechaContable") = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Row("FechaContable"), "dd/MM/yyyy")
        End If

    End Sub
    Private Sub Formatliquidacion(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        TipoFactura = Strings.Left(Numero.Value, 1)
        Dim PuntoDeVenta = Strings.Mid(Numero.Value, 2, 4)
        TextTipoFactura.Text = LetraTipoIva(Strings.Left(Numero.Value, 1))
        Numero.Value = Strings.Right(Numero.Value, 12)
        LabelPuntodeVenta.Text = "Punto de Venta " & PuntoDeVenta

    End Sub
    Private Sub FormatParseLiquidacion(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = CDbl(TipoFactura & Format(Val(MaskedLiquidacionB.Text), "000000000000"))

    End Sub
    Private Sub FormatParseString(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Sub FormatFactura(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = NumeroEditado(Numero.Value)
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If IsDBNull(Numero.Value) Then Numero.Value = 0
        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Function AgregaCabeza() As Boolean

        Dim Row As DataRow

        UltimoNumero = UltimaNumeracionLiquidacion(TipoFactura, GPuntoDeVenta)
        If UltimoNumero < 0 Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Row = DtLiquidacionCabezaB.NewRow
        ArmaNuevaLiquidacion(Row)
        Row("Liquidacion") = UltimoNumero
        Row("EsInsumos") = True
        Row("Fecha") = Date.Now
        Row("FechaContable") = "01/01/1800"
        Row("Proveedor") = PProveedor
        Row("Estado") = 1
        DtLiquidacionCabezaB.Rows.Add(Row)

        Return True

    End Function
    Private Function ActualizaArchivos(ByVal Funcion As String, ByRef DtLiquidacionCabezaBAux As DataTable, ByRef DtLiquidacionCabezaNAux As DataTable, ByRef DtLiquidacionDetalleBAux As DataTable, ByRef DtLiquidacionDetalleNAux As DataTable, ByRef DtLiquidacionConceptosBAux As DataTable, ByRef DtLiquidacionConceptosNAux As DataTable, ByRef DtRemitosB As DataTable, ByRef DtRemitosN As DataTable) As Boolean

        Dim RowsBusqueda() As DataRow

        If Funcion = "A" And CDbl(TextTotalN.Text) <> 0 Then DtLiquidacionCabezaNAux = DtLiquidacionCabezaBAux.Copy
        If Funcion = "A" And CDbl(TextTotalB.Text) = 0 Then DtLiquidacionCabezaBAux.Clear()

        RowsBusqueda = DtGrid.Select("Clave = 1")
        Dim BrutoB As Decimal = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 2")
        Dim BrutoNoGrabadoB As Decimal = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 3")
        Dim Alicuota As Decimal = RowsBusqueda(0).Item("Iva")

        'Actualizo Cabeza de Liquidacion.
        Dim ImporteBruto As Decimal = BrutoB + BrutoNoGrabadoB
        If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
            If Format(DtLiquidacionCabezaBAux.Rows(0).Item("FechaContable"), "dd/MM/yyyy") <> CDate(TextFechaContable.Text) Then DtLiquidacionCabezaBAux.Rows(0).Item("FechaContable") = CDate(TextFechaContable.Text)
            If CDbl(TextTotalB.Text) <> DtLiquidacionCabezaBAux.Rows(0).Item("Neto") Then
                DtLiquidacionCabezaBAux.Rows(0).Item("Neto") = CDec(TextTotalB.Text)
                DtLiquidacionCabezaBAux.Rows(0).Item("Importe") = CDec(TextTotalB.Text)
                DtLiquidacionCabezaBAux.Rows(0).Item("Saldo") = CDec(TextTotalB.Text)
                DtLiquidacionCabezaBAux.Rows(0).Item("Bruto") = ImporteBruto
                DtLiquidacionCabezaBAux.Rows(0).Item("Alicuota") = Alicuota
            End If
            If PLiquidacion = 0 Then
                If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then
                    DtLiquidacionCabezaBAux.Rows(0).Item("Rel") = True
                Else
                    DtLiquidacionCabezaBAux.Rows(0).Item("Rel") = False
                End If
            End If
        End If

        If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then
            RowsBusqueda = DtGrid.Select("Clave = 2")
            ImporteBruto = RowsBusqueda(0).Item("ImporteN")
            If Format(DtLiquidacionCabezaNAux.Rows(0).Item("FechaContable"), "dd/MM/yyyy") <> CDate(TextFechaContable.Text) Then DtLiquidacionCabezaNAux.Rows(0).Item("FechaContable") = CDate(TextFechaContable.Text)
            If CDbl(TextTotalN.Text) <> DtLiquidacionCabezaNAux.Rows(0).Item("Neto") Then
                DtLiquidacionCabezaNAux.Rows(0).Item("Neto") = CDec(TextTotalN.Text)
                DtLiquidacionCabezaNAux.Rows(0).Item("Importe") = CDec(TextTotalN.Text)
                DtLiquidacionCabezaNAux.Rows(0).Item("Saldo") = CDec(TextTotalN.Text)
                DtLiquidacionCabezaNAux.Rows(0).Item("Bruto") = ImporteBruto
            End If
            If PLiquidacion = 0 Then
                If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                    DtLiquidacionCabezaNAux.Rows(0).Item("Rel") = True
                Else
                    DtLiquidacionCabezaNAux.Rows(0).Item("Rel") = False
                End If
            End If
        End If

        If Funcion = "B" Then
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then DtLiquidacionCabezaBAux.Rows(0).Item("Estado") = 3
            If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then DtLiquidacionCabezaNAux.Rows(0).Item("Estado") = 3
        End If

        RowsBusqueda = DtGrid.Select("Clave = 1")
        Dim NetoGravado As Double = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 2")
        Dim NetoNoGravado As Double = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 5")
        Dim Comision As Double = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 7")
        Dim InsumosDeProduccion As Double = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 9")
        Dim ServiciosDeProducion As Double = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 10")
        Dim OtrsoConceptos As Double = RowsBusqueda(0).Item("ImporteB")
        '
        Dim IndiceCorreccionCIvaB As Double
        Dim IndiceCorreccionSIvaB As Double

        If NetoGravado <> 0 Then
            IndiceCorreccionCIvaB = CDbl(TextTotalB.Text) / NetoGravado
            IndiceCorreccionSIvaB = (NetoGravado - Comision - InsumosDeProduccion - ServiciosDeProducion - OtrsoConceptos) / NetoGravado
        Else
            IndiceCorreccionCIvaB = CDbl(TextTotalB.Text) / NetoNoGravado
            IndiceCorreccionSIvaB = (NetoNoGravado - Comision - InsumosDeProduccion - ServiciosDeProducion - OtrsoConceptos) / NetoNoGravado
        End If

        RowsBusqueda = DtGrid.Select("Clave = 2")
        NetoNoGravado = RowsBusqueda(0).Item("ImporteN")
        RowsBusqueda = DtGrid.Select("Clave = 5")
        Comision = RowsBusqueda(0).Item("ImporteN")
        RowsBusqueda = DtGrid.Select("Clave = 7")
        InsumosDeProduccion = RowsBusqueda(0).Item("ImporteN")
        RowsBusqueda = DtGrid.Select("Clave = 9")
        ServiciosDeProducion = RowsBusqueda(0).Item("ImporteN")
        RowsBusqueda = DtGrid.Select("Clave = 10")
        OtrsoConceptos = RowsBusqueda(0).Item("ImporteN")
        '
        Dim IndiceCorreccionCIvaN As Double = CDbl(TextTotalN.Text) / NetoNoGravado
        Dim IndiceCorreccionSIvaN As Double = (NetoNoGravado - Comision - InsumosDeProduccion - ServiciosDeProducion - OtrsoConceptos) / NetoNoGravado

        Dim NetoConIva As Double
        Dim NetoSinIva As Double
        Dim IndiceB As Integer = 0
        Dim IndiceN As Integer = 0

        If Funcion = "A" Then
            For Each Row As DataRow In DtGridCompro.Rows
                'Para blanco.
                NetoConIva = Trunca(Row("ImporteB") * IndiceCorreccionCIvaB)
                NetoSinIva = Trunca(Row("ImporteB") * IndiceCorreccionSIvaB)
                If Row("ImporteB") <> 0 Then
                    Dim Row1 As DataRow = DtLiquidacionDetalleBAux.NewRow
                    Row1("Liquidacion") = 0
                    IndiceB = IndiceB + 1
                    Row1("Indice") = IndiceB
                    Row1("OPR") = Row("Operacion")
                    Row1("Ingreso") = Row("Ingreso")
                    Row1("Articulo") = Row("Articulo")
                    Row1("Cantidad") = Row("Cantidad")
                    Row1("Precio") = Row("PrecioB")
                    Row1("Importe") = Row("ImporteB")
                    Row1("NetoConIva") = NetoConIva
                    Row1("NetoSinIva") = NetoSinIva
                    DtLiquidacionDetalleBAux.Rows.Add(Row1)
                End If
                'Para negro.
                NetoConIva = Trunca(Row("ImporteN") * IndiceCorreccionCIvaN)
                NetoSinIva = Trunca(Row("ImporteN") * IndiceCorreccionSIvaN)
                If Row("ImporteN") <> 0 Then
                    Dim Row1 As DataRow = DtLiquidacionDetalleNAux.NewRow
                    Row1("Liquidacion") = 0
                    IndiceN = IndiceN + 1
                    Row1("Indice") = IndiceN
                    Row1("OPR") = Row("Operacion")
                    Row1("Ingreso") = Row("Ingreso")
                    Row1("Articulo") = Row("Articulo")
                    Row1("Cantidad") = Row("Cantidad")
                    Row1("Precio") = Row("PrecioN")
                    Row1("Importe") = Row("ImporteN")
                    Row1("NetoConIva") = NetoConIva
                    Row1("NetoSinIva") = NetoSinIva
                    DtLiquidacionDetalleNAux.Rows.Add(Row1)
                End If
            Next
        End If

        'Actualiza Conceptos.
        If Funcion = "A" Then
            IndiceB = 0
            For Each Row As DataRow In DtGrid.Rows
                If Row("ImporteB") <> 0 Then
                    Dim Row1 As DataRow = DtLiquidacionConceptosBAux.NewRow
                    Row1("Liquidacion") = 0
                    IndiceB = IndiceB + 1
                    Row1("Item") = IndiceB
                    Row1("Concepto") = Row("Clave")
                    Row1("Valor") = Row("Iva")
                    Row1("Importe") = Row("ImporteB")
                    DtLiquidacionConceptosBAux.Rows.Add(Row1)
                End If
            Next
            IndiceN = 0
            For Each Row As DataRow In DtGrid.Rows
                If Row("ImporteN") <> 0 Then
                    Dim Row1 As DataRow = DtLiquidacionConceptosNAux.NewRow
                    Row1("Liquidacion") = 0
                    IndiceN = IndiceN + 1
                    Row1("Item") = IndiceN
                    Row1("Concepto") = Row("Clave")
                    Row1("Valor") = Row("Iva")
                    Row1("Importe") = Row("ImporteN")
                    DtLiquidacionConceptosNAux.Rows.Add(Row1)
                End If
            Next
        End If

        'Actualizo el campo "Facturado" en IngresoInsumosCabeza.
        For Each Row As DataRow In DtGridCompro.Rows
            Dim Leer As Boolean = False
            If Row("Operacion") = 1 Then
                If DtRemitosB.Rows.Count = 0 Then
                    Leer = True
                Else
                    RowsBusqueda = DtRemitosB.Select("Ingreso = " & Row("Ingreso"))
                    If RowsBusqueda.Length = 0 Then Leer = True
                End If
                If Leer Then
                    If Not Tablas.Read("SELECT * FROM IngresoInsumoCabeza WHERE Ingreso = " & Row("Ingreso") & ";", Conexion, DtRemitosB) Then
                        MsgBox("Error Base de Datos al leer Tabla: IngresoInsumoCabeza.")
                        End
                    End If
                End If
            Else
                If DtRemitosN.Rows.Count = 0 Then
                    Leer = True
                Else
                    RowsBusqueda = DtRemitosN.Select("Ingreso = " & Row("Ingreso"))
                    If RowsBusqueda.Length = 0 Then Leer = True
                End If
                If Leer Then
                    If Not Tablas.Read("SELECT * FROM IngresoInsumoCabeza WHERE Ingreso = " & Row("Ingreso") & ";", ConexionN, DtRemitosN) Then
                        MsgBox("Error Base de Datos al leer Tabla: IngresoInsumoCabeza.")
                        End
                    End If
                End If
            End If
        Next
        For Each Row As DataRow In DtRemitosB.Rows
            If Funcion = "B" Then Row("Facturado") = 0
        Next
        For Each Row As DataRow In DtRemitosN.Rows
            If Funcion = "B" Then Row("Facturado") = 0
        Next
        If DtRemitosB.Rows.Count = 0 Then DtRemitosB = DtRemitosN.Clone
        If DtRemitosN.Rows.Count = 0 Then DtRemitosN = DtRemitosB.Clone

        Return True

    End Function
    Private Sub ArmaGrid()

        DtGrid = ArmaConceptosLiquidacionOrdenCompra(PLiquidacion)

        Grid.DataSource = DtGrid

        BloqueaItemsGrid(0, 0)

    End Sub
    Private Sub BloqueaItemsGrid(ByVal ImporteB As Double, ByVal ImporteN As Double)

        If ImporteB = 0 And ImporteN = 0 Then
            Grid.Columns("Alicuota").ReadOnly = True
            Grid.Columns("ImporteB").ReadOnly = True
            Grid.Columns("ImporteN").ReadOnly = True
            Exit Sub
        End If

        Grid.Columns("ImporteB").ReadOnly = False
        Grid.Columns("Alicuota").ReadOnly = False
        Grid.Columns("ImporteN").ReadOnly = True
        Grid.Columns("Sel").ReadOnly = True

        If ImporteB = 0 Then
            Grid.Columns("Alicuota").ReadOnly = True
            Grid.Columns("ImporteN").ReadOnly = True
        End If

        Dim RowsBusqueda() As DataRow

        For Each Row1 As DataGridViewRow In Grid.Rows
            Select Case Row1.Cells("Clave").Value
                Case 1, 2, 3, 90
                    Row1.Cells("ImporteB").ReadOnly = True
                    Row1.Cells("Alicuota").ReadOnly = True
                Case 5, 6, 8, 11, 12
                    Row1.Cells("ImporteB").ReadOnly = True
                Case 7, 9, 10
                    Row1.Cells("Alicuota").ReadOnly = True
            End Select
            ' 
            RowsBusqueda = DtGrid.Select("Clave = " & Row1.Cells("Clave").Value)
            If RowsBusqueda(0).Item("Tipo") = 4 Then
                Row1.Cells("ImporteB").ReadOnly = True
                Row1.Cells("Alicuota").ReadOnly = True
            End If
        Next

        If GTipoIva = 2 Then
            For Each Row1 As DataGridViewRow In Grid.Rows
                Select Case Row1.Cells("Clave").Value
                    Case 6, 8, 11, 12
                        Row1.Cells("Alicuota").ReadOnly = True
                End Select
            Next
        End If

        PresentaGrid1Retenciones()

    End Sub
    Private Sub PresentaGrid1Retenciones()

        For Each Row1 As DataGridViewRow In Grid.Rows
            If HallaTipo(Row1.Cells("Clave").Value) = 4 Then
                If PLiquidacion <> 0 Then
                    Row1.Cells("ImporteB").ReadOnly = True : Row1.Cells("ImporteB").Style.BackColor = Color.LightGray
                Else
                    If HallaFormulaRetencion(Row1.Cells("Clave").Value) = 0 Or RetencionManual Then
                        Row1.Cells("ImporteB").ReadOnly = False : Row1.Cells("ImporteB").Style.BackColor = Color.White
                    Else
                        Row1.Cells("ImporteB").ReadOnly = True : Row1.Cells("ImporteB").Style.BackColor = Color.LightGray
                    End If
                    If TotalGrabadoW = 0 Then
                        Row1.Cells("ImporteB").ReadOnly = True : Row1.Cells("ImporteB").Style.BackColor = Color.LightGray
                    End If
                End If
            End If
        Next

    End Sub
    Private Function HallaFormulaRetencion(ByVal Retencion As Integer) As Integer

        For Each Fila As ItemIvaReten In ListaDeRetenciones
            If Fila.Clave = Retencion Then Return Fila.Formula
        Next

    End Function
    Private Function HallaTipo(ByVal Clave As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGrid.Select("Clave = " & Clave)
        Return RowsBusqueda(0).Item("Tipo")

    End Function
    Private Sub HacerAlta(ByVal DtLiquidacionCabezaBAux As DataTable, ByVal DtLiquidacionCabezaNAux As DataTable, ByVal DtLiquidacionDetalleBAux As DataTable, ByVal DtLiquidacionDetalleNAux As DataTable, ByVal DtLiquidacionConceptosBAux As DataTable, ByVal DtLiquidacionConceptosNAux As DataTable, ByVal DtRemitosBAux As DataTable, ByVal DtRemitosNAux As DataTable, ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtAsientoCabezaN As DataTable, ByVal DtAsientoDetalleN As DataTable)

        'Graba Factura.
        Dim NumeroN As Double = 0
        Dim NumeroAsientoB As Double = 0
        Dim NumeroAsientoN As Double = 0

        Dim Resul As Double = 0

        For i As Integer = 1 To 50
            If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then
                NumeroN = UltimaNumeracion(ConexionN)
                If NumeroN < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            '
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                DtLiquidacionCabezaBAux.Rows(0).Item("Liquidacion") = UltimoNumero
                DtLiquidacionCabezaBAux.Rows(0).Item("Interno") = UltimoNumeroInternoLiquidacion(TipoFactura, Conexion)
                If DtLiquidacionCabezaBAux.Rows(0).Item("Interno") < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close()
                    Exit Sub
                End If

                For Each Row As DataRow In DtLiquidacionDetalleBAux.Rows
                    Row("Liquidacion") = UltimoNumero
                Next
                For Each Row As DataRow In DtLiquidacionConceptosBAux.Rows
                    Row("Liquidacion") = UltimoNumero
                Next
            End If
            '
            If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then
                DtLiquidacionCabezaNAux.Rows(0).Item("Liquidacion") = NumeroN
                DtLiquidacionCabezaNAux.Rows(0).Item("Interno") = UltimoNumeroInternoLiquidacion(TipoFactura, ConexionN)
                If DtLiquidacionCabezaNAux.Rows(0).Item("Interno") < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close()
                    Exit Sub
                End If
                If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then DtLiquidacionCabezaNAux.Rows(0).Item("Nrel") = UltimoNumero
                For Each Row As DataRow In DtLiquidacionDetalleNAux.Rows
                    Row("Liquidacion") = NumeroN
                Next
                For Each Row As DataRow In DtLiquidacionConceptosNAux.Rows
                    Row("Liquidacion") = NumeroN
                Next
            End If
            '
            If DtRemitosBAux.Rows.Count <> 0 Then
                For Each Row As DataRow In DtRemitosBAux.Rows
                    Row("Facturado") = UltimoNumero
                Next
            End If
            '
            If DtRemitosNAux.Rows.Count <> 0 Then
                For Each Row As DataRow In DtRemitosNAux.Rows
                    Row("Facturado") = NumeroN
                Next
            End If
            '
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                NumeroAsientoB = UltimaNumeracionAsiento(Conexion)
                If NumeroAsientoB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                NumeroAsientoN = UltimaNumeracionAsiento(ConexionN)
                If NumeroAsientoN < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            'Actualiza Asientos.
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                DtAsientoCabezaB.Rows(0).Item("Asiento") = NumeroAsientoB
                DtAsientoCabezaB.Rows(0).Item("Documento") = UltimoNumero
                For Each Row As DataRow In DtAsientoDetalleB.Rows
                    Row("Asiento") = NumeroAsientoB
                Next
            End If
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                DtAsientoCabezaN.Rows(0).Item("Asiento") = NumeroAsientoN
                DtAsientoCabezaN.Rows(0).Item("Documento") = NumeroN
                For Each Row As DataRow In DtAsientoDetalleN.Rows
                    Row("Asiento") = NumeroAsientoN
                Next
            End If
            '
            Resul = ActualizaFactura(DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionConceptosBAux, DtLiquidacionConceptosNAux, DtRemitosBAux, DtRemitosNAux, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN)
            '
            If Resul = -10 Then Exit For
            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -10 Then
            MsgBox("Numero de Liquidacion Ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                PLiquidacion = UltimoNumero
                PAbierto = True
            Else : PLiquidacion = NumeroN
                PAbierto = False
            End If
            GModificacionOk = True
            ArmaArchivos()
        End If

    End Sub
    Private Function ActualizaFactura(ByVal DtLiquidacionCabezaBAux As DataTable, ByVal DtLiquidacionCabezaNAux As DataTable, ByVal DtLiquidacionDetalleBAux As DataTable, ByVal DtLiquidacionDetalleNAux As DataTable, ByVal DtLiquidacionConceptosBAux As DataTable, ByVal DtLiquidacionConceptosNAux As DataTable, ByVal DtRemitosBAux As DataTable, ByVal DtRemitosNAux As DataTable, ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtAsientoCabezaN As DataTable, ByVal DtAsientoDetalleN As DataTable) As Double

        'CUIDADO: en GrabaTabla siempre poner getChange de la tabla para que tome los cambio cuando pase por segunda ves.

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If PLiquidacion = 0 And Not IsNothing(DtLiquidacionCabezaBAux.GetChanges) Then
                    If Not ReGrabaUltimaNumeracionLiquidacion(DtLiquidacionCabezaBAux.Rows(0).Item("Liquidacion"), TipoFactura) Then Return -10
                End If

                If Not IsNothing(DtLiquidacionCabezaBAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionCabezaBAux.GetChanges, "LiquidacionCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLiquidacionCabezaNAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionCabezaNAux.GetChanges, "LiquidacionCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLiquidacionDetalleBAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionDetalleBAux.GetChanges, "LiquidacionInsumosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLiquidacionDetalleNAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionDetalleNAux.GetChanges, "LiquidacionInsumosDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLiquidacionConceptosBAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionConceptosBAux.GetChanges, "LiquidacionDetalleConceptos", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLiquidacionConceptosNAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionConceptosNAux.GetChanges, "LiquidacionDetalleConceptos", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtRemitosBAux.GetChanges) Then
                    Resul = GrabaTabla(DtRemitosBAux.GetChanges, "IngresoInsumoCabeza", Conexion)
                    If Resul <= 0 Then Return -2
                End If
                '
                If Not IsNothing(DtRemitosNAux.GetChanges) Then
                    Resul = GrabaTabla(DtRemitosNAux.GetChanges, "IngresoInsumoCabeza", ConexionN)
                    If Resul <= 0 Then Return -2
                End If

                ' graba Asiento B.
                If Not IsNothing(DtAsientoCabezaB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaB.GetChanges, "AsientosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalleB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleB.GetChanges, "AsientosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoCabezaN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalleN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleN.GetChanges, "AsientosDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        Finally
        End Try

    End Function
    Private Function LlenaDatosProveedor(ByVal Proveedor As Integer) As Boolean

        Dim Dta As New DataTable
        Dim Sql As String = ""

        Sql = "SELECT * FROM Proveedores WHERE Clave = " & Proveedor & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, Proveedor No existe o error en la Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Calle = Dta.Rows(0).Item("Calle")
        Localidad = Dta.Rows(0).Item("Localidad")
        ProveedorOpr = Dta.Rows(0).Item("Opr")
        TextProvincia.Text = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        Provincia = Dta.Rows(0).Item("Provincia")
        ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
        TextCuit.Text = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")
        If Dta.Rows(0).Item("TipoIva") = 6 And PLiquidacion = 0 Then
            MsgBox("Tipo Iva MONOTRIBUTO no puede hacer liquidaciones.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Dta.Dispose()

        Return True

    End Function
    Private Sub CalculaTotal()

        If PLiquidacion <> 0 Then Exit Sub

        Dim NetoB As Decimal = TotalImporteB
        Dim Grabado As Decimal = TotalGrabadoW

        Dim IndiceCorreccion As Decimal = 1

        Dim TotalRemitosB As Double = 0
        Dim TotalRemtosN As Double = 0
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGridCompro.Rows
            TotalRemitosB = TotalRemitosB + Row("ImporteB")
            TotalRemtosN = TotalRemtosN + Row("ImporteN")
        Next

        'Trae todos los importes manuales.
        RowsBusqueda = DtGrid.Select("Clave = 5")
        Dim ComisionT As Decimal = RowsBusqueda(0).Item("Iva")
        RowsBusqueda = DtGrid.Select("Clave = 7")
        Dim InsumosProduccion As Double = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 9")
        Dim ServiciosProduccion As Double = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 10")
        Dim OtrosConceptos As Double = RowsBusqueda(0).Item("ImporteB")
        'Trae todos los ivas.
        RowsBusqueda = DtGrid.Select("Clave = 3")
        Dim IvaB As Decimal = RowsBusqueda(0).Item("Iva")
        RowsBusqueda = DtGrid.Select("Clave = 6")
        Dim IvaComision As Decimal = RowsBusqueda(0).Item("Iva")
        RowsBusqueda = DtGrid.Select("Clave = 8")
        Dim IvaInsumosProduccion As Decimal = RowsBusqueda(0).Item("Iva")
        RowsBusqueda = DtGrid.Select("Clave = 11")
        Dim IvaServiciosProduccion As Decimal = RowsBusqueda(0).Item("Iva")
        RowsBusqueda = DtGrid.Select("Clave = 12")
        Dim IvaOtrosConceptos As Decimal = RowsBusqueda(0).Item("Iva")

        'Reten. Ingreso Bruto.
        Dim AlicuotaProvincia As Decimal = 0
        Dim ImpRetencionesManual As Decimal = 0

        RowsBusqueda = DtGrid.Select("Tipo = 4")   'Tipo = 4 es Reten/Perc.
        For Each Row As DataRow In RowsBusqueda
            For Each Fila As ItemIvaReten In ListaDeRetenciones
                If Fila.Clave = Row("Clave") Then
                    If Fila.Formula = 0 Or RetencionManual Then
                        Fila.Importe = Abs(Row("ImporteB"))
                        ImpRetencionesManual = ImpRetencionesManual + Fila.Importe
                    Else
                        Fila.Importe = 0
                        If Fila.Formula = 2 Then Fila.Alicuota = Row("Iva") : AlicuotaProvincia = AlicuotaProvincia + Row("Iva")
                    End If
                End If
            Next
        Next

        Dim RetencionesGanancia As Decimal = 0
        Dim TotalManuales As Decimal = InsumosProduccion + CalculaIva(1, InsumosProduccion, IvaInsumosProduccion) + ServiciosProduccion + CalculaIva(1, ServiciosProduccion, IvaServiciosProduccion) + OtrosConceptos + CalculaIva(1, OtrosConceptos, IvaOtrosConceptos)

        If Grabado = 0 Then AlicuotaProvincia = 0

        Dim Fecha As Date
        Fecha = DateTime1.Value

        If TotalRemitosB <> 0 Then
            Dim BrutoW As Decimal = CalculaBruto(NetoB, IvaB, ComisionT, IvaComision, AlicuotaProvincia, TotalManuales, ImpRetencionesManual)
            IndiceCorreccion = BrutoW / TotalRemitosB
            've si existe ret/perc. a las ganancias recalcula a partir del BrutoW calculado arriva y recalcula el BrutoW.
            If Grabado <> 0 Then RetencionesGanancia = CalculaRetenFormulaLiquidacionProv1(PProveedor, BrutoW, IvaB, ListaDeRetenciones, RetencionManual, Fecha)
            If RetencionesGanancia > 0 Then
                ImpRetencionesManual = ImpRetencionesManual + RetencionesGanancia
                BrutoW = CalculaBruto(NetoB, IvaB, ComisionT, IvaComision, AlicuotaProvincia, TotalManuales, ImpRetencionesManual)
                IndiceCorreccion = BrutoW / TotalRemitosB
            End If
            CalculaRetenFormula2(BrutoW, AlicuotaProvincia, ListaDeRetenciones)
        End If

        Dim BrutoB As Decimal = 0
        Dim GrabadoB As Decimal = 0
        Dim NoGrabadoB As Decimal = 0
        For Each Row As DataRow In DtGridCompro.Rows
            Row("PrecioB") = Row("ImporteB") * IndiceCorreccion / Row("Cantidad")
            BrutoB = BrutoB + CalculaNeto(Row("Cantidad"), Row("PrecioB"))
        Next
        BrutoB = Trunca(BrutoB)

        If IvaW = 0 Then NoGrabadoB = BrutoB
        If IvaW <> 0 Then GrabadoB = BrutoB

        Dim Comision As Decimal = CalculaIva(1, BrutoB, ComisionT)

        ' que pasa con el grabado y total b? 
        For Each Row As DataRow In DtGrid.Rows
            If Row("Clave") = 1 Then
                Row("ImporteB") = GrabadoB
            End If
            If Row("Clave") = 2 Then
                Row("ImporteB") = NoGrabadoB
                Row("ImporteN") = TotalRemtosN
            End If
            If Row("Clave") = 3 Then
                Row("ImporteB") = Trunca(GrabadoB * Row("Iva") / 100)
            End If
            If Row("Clave") = 5 Then
                Row("ImporteB") = Comision
            End If
            If Row("Clave") = 6 Then
                Row("ImporteB") = Trunca(Comision * Row("Iva") / 100)
            End If
            If Row("Clave") = 8 Then
                Row("ImporteB") = Trunca(InsumosProduccion * Row("Iva") / 100)
            End If
            If Row("Clave") = 11 Then
                Row("ImporteB") = Trunca(ServiciosProduccion * Row("Iva") / 100)
            End If
            If Row("Clave") = 12 Then
                Row("ImporteB") = Trunca(OtrosConceptos * Row("Iva") / 100)
            End If
            If Row("Clave") > 500 Then     'es una retencion
                If HallaTipo(Row("Clave")) = 4 Then
                    For Each Fila As ItemIvaReten In ListaDeRetenciones
                        If Fila.Clave = Row("Clave") Then Row("ImporteB") = Fila.Importe
                    Next
                End If
            End If
        Next

        Dim TotalB As Double = 0
        Dim TotalN As Double = 0

        For Each Row As DataRow In DtGrid.Rows
            If Row("Clave") <> 90 Then
                If Row("Clave") = 1 Or Row("Clave") = 2 Or Row("Clave") = 3 Then
                    TotalB = TotalB + Row("ImporteB")
                    TotalN = TotalN + Row("ImporteN")
                Else
                    TotalB = TotalB - Row("ImporteB")
                    TotalN = TotalN - Row("ImporteN")
                End If
            End If
        Next

        RowsBusqueda = DtGrid.Select("Clave = 90")
        If TextTeoricoB.Text <> "" Then RowsBusqueda(0).Item("ImporteB") = Trunca(CDbl(TextTeoricoB.Text) - TotalB)
        If TextTeoricoN.Text <> "" Then RowsBusqueda(0).Item("ImporteN") = Trunca(CDbl(TextTeoricoN.Text) - TotalN)

        TotalB = 0
        TotalN = 0

        For Each Row As DataRow In DtGrid.Rows
            If Row("Clave") = 1 Or Row("Clave") = 2 Or Row("Clave") = 3 Or Row("Clave") = 90 Then
                TotalB = TotalB + Row("ImporteB")
                TotalN = TotalN + Row("ImporteN")
            Else
                TotalB = TotalB - Row("ImporteB")
                TotalN = TotalN - Row("ImporteN")
            End If
        Next

        TextTotalB.Text = FormatNumber(TotalB, GDecimales)
        TextTotalN.Text = FormatNumber(TotalN, GDecimales)

    End Sub
    Private Function CalculaBruto(ByVal NetoB As Decimal, ByVal PromedioIva As Decimal, ByVal ComisionT As Decimal, ByVal AlicuotaComisionT As Decimal, ByVal AlicuotaProvincia As Decimal, ByVal TotalManuales As Decimal, ByVal RetencionGanancia As Decimal) As Decimal

        Dim IndiceCorreccion As Decimal = 1

        'IndiceCorreccion = bruto * (1 + IvaVenta - Comision - IvaComision) / bruto   
        IndiceCorreccion = 1 + PromedioIva / 100 - ComisionT / 100 - ComisionT / 100 * AlicuotaComisionT / 100 - AlicuotaProvincia / 100
        'neto = bruto * IndiceCorreccion - TotalManuales
        Dim BrutoW As Decimal = (NetoB + TotalManuales + RetencionGanancia) / IndiceCorreccion

        Return BrutoW

    End Function
    Private Function CalculaRetenFormula2(ByVal Bruto As Decimal, ByVal AlicuotaProvincia As Decimal, ByRef ListaReten As List(Of ItemIvaReten)) As Decimal

        Dim TotRetencion As Decimal = 0

        For Each Fila As ItemIvaReten In ListaReten
            If Fila.Formula = 2 And Not RetencionManual Then
                Fila.Importe = Trunca(Fila.Alicuota / 100 * Bruto)
                TotRetencion = TotRetencion + Fila.Importe
            End If
        Next

        Return TotRetencion

    End Function
    Private Sub CalculaTotalAnt()

        Dim TotalRemitosB As Double = 0
        Dim TotalRemitosBG As Double = 0
        Dim TotalRemitosBE As Double = 0
        Dim TotalRemtosN As Double = 0
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGridCompro.Rows
            TotalRemitosB = TotalRemitosB + Row("ImporteB")
            TotalRemtosN = TotalRemtosN + Row("ImporteN")
        Next

        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Iva") <> 0 Then
                TotalRemitosBG = TotalRemitosBG + Row("ImporteB")
            Else
                TotalRemitosBE = TotalRemitosBE + Row("ImporteB")
            End If
        Next

        If PLiquidacion = 0 Then
            RowsBusqueda = DtGrid.Select("Clave = 1")
            RowsBusqueda(0).Item("ImporteB") = TotalRemitosBG
            RowsBusqueda = DtGrid.Select("Clave = 2")
            RowsBusqueda(0).Item("ImporteB") = TotalRemitosBE
        End If

        RowsBusqueda = DtGrid.Select("Clave = 2")
        RowsBusqueda(0).Item("ImporteN") = TotalRemtosN

        RowsBusqueda = DtGrid.Select("Clave = 1")
        Dim Grabado As Double = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 2")
        Dim NoGrabado As Double = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 5")
        '        Dim Comision As Double = RowsBusqueda(0).Item("ImporteB")
        Dim Comision As Double = CalculaIva(1, TotalRemitosB, RowsBusqueda(0).Item("Iva"))
        RowsBusqueda = DtGrid.Select("Clave = 7")
        Dim InsumosProduccion As Double = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 9")
        Dim ServiciosProduccion As Double = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 10")
        Dim OtrosConceptos As Double = RowsBusqueda(0).Item("ImporteB")

        For Each Row As DataRow In DtGrid.Rows
            If Row("Clave") = 3 Then
                Row("ImporteB") = Trunca(Grabado * Row("Iva") / 100)
            End If
            If Row("Clave") = 5 Then
                Row("ImporteB") = Comision
            End If
            If Row("Clave") = 6 Then
                Row("ImporteB") = Trunca(Comision * Row("Iva") / 100)
            End If
            If Row("Clave") = 8 Then
                Row("ImporteB") = Trunca(InsumosProduccion * Row("Iva") / 100)
            End If
            If Row("Clave") = 11 Then
                Row("ImporteB") = Trunca(ServiciosProduccion * Row("Iva") / 100)
            End If
            If Row("Clave") = 12 Then
                Row("ImporteB") = Trunca(OtrosConceptos * Row("Iva") / 100)
            End If
            If Row("Clave") > 500 Then     'es una retencion
                Row("ImporteB") = Trunca(Row("Iva") / 100 * Grabado)
            End If
        Next

        Dim TotalB As Double = 0
        Dim TotalN As Double = 0

        For Each Row As DataRow In DtGrid.Rows
            If Row("Clave") <> 90 Then
                If Row("Clave") = 1 Or Row("Clave") = 2 Or Row("Clave") = 3 Then
                    TotalB = TotalB + Row("ImporteB")
                    TotalN = TotalN + Row("ImporteN")
                Else
                    TotalB = TotalB - Row("ImporteB")
                    TotalN = TotalN - Row("ImporteN")
                End If
            End If
        Next

        RowsBusqueda = DtGrid.Select("Clave = 90")
        If TextTeoricoB.Text <> "" Then RowsBusqueda(0).Item("ImporteB") = Trunca(CDbl(TextTeoricoB.Text) - TotalB)
        If TextTeoricoN.Text <> "" Then RowsBusqueda(0).Item("ImporteN") = Trunca(CDbl(TextTeoricoN.Text) - TotalN)

        TotalB = 0
        TotalN = 0

        For Each Row As DataRow In DtGrid.Rows
            If Row("Clave") = 1 Or Row("Clave") = 2 Or Row("Clave") = 3 Or Row("Clave") = 90 Then
                TotalB = TotalB + Row("ImporteB")
                TotalN = TotalN + Row("ImporteN")
            Else
                TotalB = TotalB - Row("ImporteB")
                TotalN = TotalN - Row("ImporteN")
            End If
        Next

        TextTotalB.Text = FormatNumber(TotalB, GDecimales)
        TextTotalN.Text = FormatNumber(TotalN, GDecimales)

    End Sub
    Public Function ArmaConceptosLiquidacionOrdenCompra(ByVal Clave1 As Double) As DataTable

        Dim Dt As New DataTable

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Clave)
        '
        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        Dt.Columns.Add(Nombre)
        '
        Dim Iva As New DataColumn("Iva")
        Iva.DataType = System.Type.GetType("System.Double")
        Dt.Columns.Add(Iva)
        '
        Dim ImporteB As New DataColumn("ImporteB")
        ImporteB.DataType = System.Type.GetType("System.Double")
        Dt.Columns.Add(ImporteB)
        '
        Dim ImporteN As New DataColumn("ImporteN")
        ImporteN.DataType = System.Type.GetType("System.Double")
        Dt.Columns.Add(ImporteN)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Tipo)
        '
        Dim TieneLupa As New DataColumn("TieneLupa")
        TieneLupa.DataType = System.Type.GetType("System.Boolean")
        Dt.Columns.Add(TieneLupa)
        '
        Try
            Dim Row As DataRow = Dt.NewRow
            Row("Clave") = 1
            Row("Nombre") = "Neto Gravado"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 2
            Row("Nombre") = "Neto No Gravado"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 3
            Row("Nombre") = "I.V.A."
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 5
            Row("Nombre") = "Comisión"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 7
            Row("Nombre") = "Insumos de Producción"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 9
            Row("Nombre") = "Servicios de Producción"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 10
            Row("Nombre") = "Otros Conceptos"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 6
            Row("Nombre") = "I.V.A. Comisión"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 8
            Row("Nombre") = "I.V.A. Insumos de Producción"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 11
            Row("Nombre") = "I.V.A. Servicios de Producción"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 12
            Row("Nombre") = "I.V.A. Otros Conceptos"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Dt.Rows.Add(Row)
            '
            AgregaRetenciones(Dt)
            '
            Row = Dt.NewRow
            Row("Clave") = 90
            Row("Nombre") = "Redondeo"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Dt.Rows.Add(Row)
            '
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Private Sub AgregaRetenciones(ByVal Dt As DataTable)

        ListaDeRetenciones = HallaRetencionesAplicables(10, 1, PProveedor, DateTime1.Value)
        For Each Fila As ItemIvaReten In ListaDeRetenciones
            If Fila.Formula = 2 Then
                Dim AlicuotaProvinciaW As Decimal = HallaAlicuotaRetIngBruto(Provincia)
                If AlicuotaProvinciaW < 0 Then
                    MsgBox("Error Base de Datos al Leer tabla de Provincias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close() : Exit Sub
                End If
                Dim Row As DataRow = Dt.NewRow
                Row("Tipo") = 4
                Row("Clave") = Fila.Clave
                Row("Nombre") = Fila.Nombre
                Row("Iva") = AlicuotaProvinciaW
                Row("ImporteB") = 0
                Row("ImporteN") = 0
                Dt.Rows.Add(Row)
                AlicuotaProvincia = AlicuotaProvinciaW
            End If
            If Fila.Formula = 1 Then
                Dim Row As DataRow = Dt.NewRow
                Row("Tipo") = 4
                Row("Clave") = Fila.Clave
                Row("Nombre") = Fila.Nombre
                Row("Iva") = Fila.Alicuota
                Row("ImporteB") = 0
                Row("ImporteN") = 0
                Dt.Rows.Add(Row)
            End If
            If Fila.Formula = 0 Then
                Dim Row As DataRow = Dt.NewRow
                Row("Tipo") = 4
                Row("Clave") = Fila.Clave
                Row("Nombre") = Fila.Nombre
                Row("Iva") = 0
                Row("ImporteB") = 0
                Row("ImporteN") = 0
                Dt.Rows.Add(Row)
            End If
        Next
    End Sub
    Private Sub CreaDtGridCompro()

        DtGridCompro = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Operacion)

        Dim Indice As New DataColumn("Indice")
        Indice.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Indice)

        Dim OrdenCompra As New DataColumn("OrdenCompra")
        OrdenCompra.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(OrdenCompra)

        Dim Ingreso As New DataColumn("Ingreso")
        Ingreso.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Ingreso)

        Dim Remito As New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Remito)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Articulo)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Cantidad)

        Dim PrecioB As New DataColumn("PrecioB")
        PrecioB.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(PrecioB)

        Dim ImporteB As New DataColumn("ImporteB")
        ImporteB.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(ImporteB)

        Dim PrecioN As New DataColumn("PrecioN")
        PrecioN.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(PrecioN)

        Dim ImporteN As New DataColumn("ImporteN")
        ImporteN.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(ImporteN)

        Dim Iva As New DataColumn("Iva")
        Iva.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Iva)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Articulo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Insumos;")
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Public Function GrabaImpreso(ByVal Liquidacion As Double, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String
        If Abierto Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim Sql As String = "UPDATE LiquidacionCabeza Set Impreso = 1 WHERE Liquidacion = " & Liquidacion & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                If CInt(Cmd.ExecuteNonQuery()) = 0 Then Return False
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos al Grabar Liquidación. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End Try

        Return True

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Liquidacion) FROM LiquidacionCabeza;", Miconexion)
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
    Private Sub Opciones()

        OpcionEmisorYLetra.PEsProveedor = True
        OpcionEmisorYLetra.PEsLocal = True
        OpcionEmisorYLetra.PEsNoNegocio = True
        OpcionEmisorYLetra.PictureCandado.Visible = False
        OpcionEmisorYLetra.PEsSinLetra = True
        OpcionEmisorYLetra.ShowDialog()
        ComboProveedor.SelectedValue = OpcionEmisorYLetra.PEmisor
        PProveedor = OpcionEmisorYLetra.PEmisor
        OpcionEmisorYLetra.Dispose()

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable, ByVal Operacion As Integer) As Boolean

        Dim ListaLotesParaAsientoAux As New List(Of ItemLotesParaAsientos)
        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow

        Dim Item As New ItemListaConceptosAsientos
        Item.Clave = 213
        If Operacion = 1 Then
            Item.Importe = CDbl(TextTotalB.Text)
        Else
            Item.Importe = CDbl(TextTotalN.Text)
        End If
        ListaConceptos.Add(Item)
        '
        Dim Neto As Decimal = 0
        Dim Comision As Decimal = 0
        Dim InsumosProduccion As Decimal = 0
        Dim ServiciosProduccion As Decimal = 0
        Dim OtrosConceptos As Decimal = 0
        Dim Redondeo As Decimal = 0
        '
        If Operacion = 1 Then
            RowsBusqueda = DtGrid.Select("Clave = 1")
            Neto = RowsBusqueda(0).Item("ImporteB")
            RowsBusqueda = DtGrid.Select("Clave = 2")
            Neto = Neto + RowsBusqueda(0).Item("ImporteB")
            RowsBusqueda = DtGrid.Select("Clave = 5")
            Comision = RowsBusqueda(0).Item("ImporteB")
            RowsBusqueda = DtGrid.Select("Clave = 7")
            InsumosProduccion = RowsBusqueda(0).Item("ImporteB")
            RowsBusqueda = DtGrid.Select("Clave = 9")
            ServiciosProduccion = RowsBusqueda(0).Item("ImporteB")
            RowsBusqueda = DtGrid.Select("Clave = 10")
            OtrosConceptos = RowsBusqueda(0).Item("ImporteB")
            RowsBusqueda = DtGrid.Select("Clave = 90")
            Redondeo = RowsBusqueda(0).Item("ImporteB")
        Else
            RowsBusqueda = DtGrid.Select("Clave = 2")
            Neto = RowsBusqueda(0).Item("ImporteN")
            RowsBusqueda = DtGrid.Select("Clave = 5")
            Comision = RowsBusqueda(0).Item("ImporteN")
            RowsBusqueda = DtGrid.Select("Clave = 7")
            InsumosProduccion = RowsBusqueda(0).Item("ImporteN")
            RowsBusqueda = DtGrid.Select("Clave = 9")
            ServiciosProduccion = RowsBusqueda(0).Item("ImporteN")
            RowsBusqueda = DtGrid.Select("Clave = 10")
            OtrosConceptos = RowsBusqueda(0).Item("ImporteN")
            RowsBusqueda = DtGrid.Select("Clave = 90")
            Redondeo = RowsBusqueda(0).Item("ImporteN")
        End If
        '
        If Neto <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = 1
            Item.Importe = Neto
            ListaConceptos.Add(Item)
        End If
        '
        If Comision <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = 5
            Item.Importe = Comision
            ListaConceptos.Add(Item)
        End If
        '
        If InsumosProduccion <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = 7
            Item.Importe = InsumosProduccion
            ListaConceptos.Add(Item)
        End If
        '
        If ServiciosProduccion <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = 9
            Item.Importe = ServiciosProduccion
            ListaConceptos.Add(Item)
        End If
        '
        If OtrosConceptos <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = 10
            Item.Importe = OtrosConceptos
            ListaConceptos.Add(Item)
        End If
        '
        If Redondeo <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = 90
            Item.Importe = Redondeo
            ListaConceptos.Add(Item)
        End If
        '
        RowsBusqueda = DtGrid.Select("Clave = 3")
        If Operacion = 1 Then
            If RowsBusqueda(0).Item("ImporteB") <> 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = HallaClaveIva(RowsBusqueda(0).Item("Iva"))
                Item.Importe = RowsBusqueda(0).Item("ImporteB")
                Item.TipoIva = 5
                ListaIVA.Add(Item)
            End If
        End If
        '
        RowsBusqueda = DtGrid.Select("Clave = 6")
        If Operacion = 1 Then
            If RowsBusqueda(0).Item("ImporteB") <> 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = HallaClaveIva(RowsBusqueda(0).Item("Iva"))
                Item.Importe = RowsBusqueda(0).Item("ImporteB")
                Item.TipoIva = 6
                ListaIVA.Add(Item)
            End If
        End If
        '
        RowsBusqueda = DtGrid.Select("Clave = 8")
        If Operacion = 1 Then
            If RowsBusqueda(0).Item("ImporteB") <> 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = HallaClaveIva(RowsBusqueda(0).Item("Iva"))
                Item.Importe = RowsBusqueda(0).Item("ImporteB")
                Item.TipoIva = 6
                ListaIVA.Add(Item)
            End If
        End If
        '
        RowsBusqueda = DtGrid.Select("Clave = 11")
        If Operacion = 1 Then
            If RowsBusqueda(0).Item("ImporteB") <> 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = HallaClaveIva(RowsBusqueda(0).Item("Iva"))
                Item.Importe = RowsBusqueda(0).Item("ImporteB")
                Item.TipoIva = 6
                ListaIVA.Add(Item)
            End If
        End If
        '
        RowsBusqueda = DtGrid.Select("Clave = 12")
        If Operacion = 1 Then
            If RowsBusqueda(0).Item("ImporteB") <> 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = HallaClaveIva(RowsBusqueda(0).Item("Iva"))
                Item.Importe = RowsBusqueda(0).Item("ImporteB")
                Item.TipoIva = 6
                ListaIVA.Add(Item)
            End If
        End If
        '
        If Operacion = 1 Then
            For Each Row As DataRow In DtGrid.Rows
                RowsBusqueda = DtGrid.Select("Clave = " & Row("Clave"))
                If Row("Tipo") = 4 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("Clave")
                    Item.Importe = Row("ImporteB")
                    Item.TipoIva = 11    'Debito fiscal. 
                    If Item.Importe <> 0 Then ListaRetenciones.Add(Item)
                End If
            Next
        End If

        If Funcion = "A" Then
            If Not Asiento(915, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, CDate(TextFechaContable.Text), 0) Then Return False
        End If

        Return True

    End Function
    Private Sub AgregarArticulos(ByVal OrdenCompra As Double, ByVal Remito As Double, ByVal dt1 As DataTable, ByVal Ingreso As Double, ByVal Operacion As Integer)

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim DtIngreso As New DataTable
        If Not Tablas.Read("SELECT Articulo,Cantidad - Devueltas AS Cantidad FROM IngresoInsumoDetalle WHERE Ingreso = " & Ingreso & ";", ConexionStr, DtIngreso) Then
            MsgBox("Error Base de Datos  al leer Tabla: IngresoInsumoDetalle.")
            End
        End If

        For Each Row As DataRow In DtIngreso.Rows
            If Row("Cantidad") <> 0 Then
                Dim Row1 As DataRow = dt1.NewRow
                Row1("Operacion") = Operacion
                Row1("OrdenCompra") = OrdenCompra
                Row1("Ingreso") = Ingreso
                Row1("Remito") = Remito
                Row1("Articulo") = Row("Articulo")
                Row1("Cantidad") = Row("Cantidad")
                Row1("PrecioB") = 0
                Row1("PrecioN") = 0
                Row1("Iva") = 0
                Dim CostoConIva As Double
                Dim CostoSinIva As Double
                Dim ImporteB As Double
                If CostoInsumoOCompra(OrdenCompra, Row("Articulo"), Row("Cantidad"), CostoConIva, CostoSinIva, ImporteB, ConexionStr, Row1("PrecioB"), Row1("PrecioN"), Row1("Iva")) <> 0 Then
                    MsgBox("Error Base de Datos al Clacular Costo del Insumo: " & NombreInsumo(Row("Articulo")))
                    End
                End If
                Row1("ImporteB") = CalculaNeto(Row("Cantidad"), Row1("PrecioB"))
                Row1("ImporteN") = CalculaNeto(Row("Cantidad"), Row1("PrecioN"))
                dt1.Rows.Add(Row1)
            End If
        Next

        DtIngreso.Dispose()

    End Sub
    Private Function ExisteEnLista(ByVal ListaDeLotesW As List(Of FilaComprobanteFactura), ByVal Operacion As Integer, ByVal Ingreso As Integer) As Boolean

        For Each Fila As FilaComprobanteFactura In ListaDeLotesW
            If Fila.Operacion = Operacion And Fila.Ingreso = Ingreso Then Return True
        Next

        Return False

    End Function
    Private Sub HallaDatosIngreso(ByVal Operacion As Integer, ByVal Ingreso As Double, ByRef OrdenCompra As Double, ByRef Remito As Double)

        Dim ConexionStr As String
        Dim Dt As New DataTable

        OrdenCompra = 0 : Remito = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT OrdenCompra,Remito FROM IngresoInsumoCabeza WHERE Ingreso = " & Ingreso & ";", ConexionStr, Dt) Then
            MsgBox("Error Base De Datos al Leer Tabla: IngresoInsumoCabeza.")
        End If

        OrdenCompra = Dt.Rows(0).Item("OrdenCompra")
        Remito = Dt.Rows(0).Item("Remito")

        Dt.Dispose()

    End Sub
    Private Sub Print_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        '        Dim xPos As Single = e.MarginBounds.Left

        Dim MIzq = 15
        Dim MTop = 20

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim PrintFont As System.Drawing.Font
        Dim SaltoLinea As Integer = 4

        x = MIzq : y = MTop + 40

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            PrintFont = New Font("Courier New", 11)
            'Titulos.
            Texto = TextFechaContable.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 130, y - 25)
            If AbiertoParaImpresion Then
                Texto = "REMITENTE  : " & ComboProveedor.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "DOMICILIO  : " & Calle
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "LOCALIDAD  : " & Localidad
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                Texto = "PROVINCIA : " & TextProvincia.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 60, y)
                y = y + SaltoLinea
                Texto = "CUIT       : " & TextCuit.Text & "    " & ComboTipoIva.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Else
                Texto = "REMITENTE  : " & ComboProveedor.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + 3 * SaltoLinea
            End If

            'Grafica 
            Dim Ancho As Integer = 185
            Dim Alto As Integer = 125
            Dim LineaOrdenCompra As Integer = x + 20
            Dim LineaRemito As Integer = x + 50
            Dim LineaArticulo As Integer = x + 106
            Dim LineaCantidad As Integer = x + 125
            Dim LineaPrecio As Integer = x + 155
            Dim LineaImporte As Integer = x + Ancho
            Dim LineaResumen As Integer = x + 160
            Dim Longi As Double
            Dim Xq As Integer

            y = MTop + 58

            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaOrdenCompra, y, LineaOrdenCompra, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaRemito, y, LineaRemito, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaArticulo, y, LineaArticulo, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCantidad, y, LineaCantidad, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaPrecio, y, LineaPrecio, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte, y, LineaImporte, y + Alto)
            'Titulos de descripcion.
            PrintFont = New Font("Courier New", 9)
            Texto = "O. COMPRA"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 1, y + 2)
            Texto = "REMITO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaRemito - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "ARTICULO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaArticulo - Longi - 20
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "CANTIDAD"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi - 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "PRECIO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaPrecio - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'Linea Horizontal 
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)

            'Descripcion de Articulos.
            PrintFont = New Font("Courier New", 8)
            y = y - SaltoLinea
            For Each Row As DataGridViewRow In GridCompro.Rows
                If AbiertoParaImpresion And Row.Cells("Precio").Value <> 0 Or Not AbiertoParaImpresion And Row.Cells("Precio2").Value <> 0 Then
                    y = y + SaltoLinea
                    'Imprime Orden Compra.
                    Texto = Row.Cells("OrdenCompra").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaOrdenCompra - Longi - 2
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                    'Imprime Remito.
                    Texto = Row.Cells("Remito").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaRemito - Longi - 2
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                    'Articulo.
                    Texto = Row.Cells("Articulo").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaRemito + 1
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                    'Cantidad
                    Texto = Row.Cells("Cantidad").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaCantidad - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                    'Precio.
                    If AbiertoParaImpresion Then
                        Texto = Row.Cells("Precio").FormattedValue
                    Else
                        Texto = Row.Cells("Precio2").FormattedValue
                    End If
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaPrecio - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                    'Importe.
                    If AbiertoParaImpresion Then
                        Texto = Row.Cells("Importe").FormattedValue
                    Else
                        Texto = Row.Cells("Importe2").FormattedValue
                    End If
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                End If
            Next

            y = MTop + 185
            '
            'Imprime Totales.
            PrintFont = New Font("Courier New", 11)
            For Each Row As DataGridViewRow In Grid.Rows
                y = y + SaltoLinea
                Dim Alicuota As String = ""
                If Row.Cells("Alicuota").FormattedValue <> "" Then Alicuota = Row.Cells("Alicuota").FormattedValue & " %"
                Texto = Row.Cells("Nombre").FormattedValue & "  " & Alicuota
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 20, y)
                If AbiertoParaImpresion Then
                    Texto = Row.Cells("ImporteB").FormattedValue
                Else
                    Texto = Row.Cells("ImporteN").FormattedValue
                End If
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaResumen - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            Next
            y = y + SaltoLinea
            Texto = "T O T A L "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 20, y)
            If AbiertoParaImpresion Then
                Texto = TextTotalB.Text
            Else
                Texto = TextTotalN.Text
            End If
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaResumen - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)

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
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Dim Patron As String = TipoFactura & Format(GPuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM LiquidacionCabeza WHERE CAST(CAST(LiquidacionCabeza.Liquidacion AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
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
    Private Function UltimaNumeracionAsiento(ByVal ConexionStr) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Asiento) FROM AsientosCabeza;", Miconexion)
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
    Public Function UltimoNumeroInternoLiquidacion(ByVal TipoFacturaW As Integer, ByVal ConexionStr As String) As Double

        Dim Patron As String = TipoFacturaW & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Interno) FROM LiquidacionCabeza WHERE CAST(CAST(LiquidacionCabeza.Interno AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(TipoFacturaW & Format(1, "000000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HuboCambio(ByVal Lista As List(Of FilaComprobanteFactura), ByVal dt As DataTable) As Boolean

        If dt.Rows.Count = 0 Then Return True

        Dim RowsBusqueda() As DataRow

        For Each Fila As FilaComprobanteFactura In Lista
            RowsBusqueda = dt.Select("Ingreso = " & Fila.Ingreso)
            If RowsBusqueda.Length = 0 Then Return True
        Next

        Dim Existe As Boolean = False
        For Each Row As DataRow In dt.Rows
            Existe = False
            For Each Fila As FilaComprobanteFactura In Lista
                If Fila.Ingreso = Row("Ingreso") Then Existe = True : Exit For
            Next
            If Not Existe Then Return True
        Next

    End Function
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 And PLiquidacion = 0 Then
            MsgBox("Fecha Incorrecta. Existe Otro Comprobante con Fecha " & UltimaFechaW & " Posterior a la Informada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If

        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            If DtLiquidacionCabezaB.Rows(0).Item("Rel") And Not PermisoTotal Then
                MsgBox("Error, en este momento no puede modificar(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If GridCompro.Rows.Count = 0 Then
            MsgBox("Falta Informar Items a Liquidar.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If CDbl(TextTotalB.Text) < 0 Or CDbl(TextTotalN.Text) < 0 Then
            MsgBox("Total Negativo", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        For Each Row As DataGridViewRow In GridCompro.Rows
            If Row.Cells("Cantidad").Value = 0 Then
                MsgBox("Falta Cantidad en Remito " & NumeroEditado(Row.Cells("Remito").Value))
                Return False
            End If
            If Row.Cells("Precio").Value = 0 And Row.Cells("Precio2").Value = 0 Then
                MsgBox("Falta Precio en Remito " & NumeroEditado(Row.Cells("Remito").Value))
                Return False
            End If
            If Row.Cells("Importe").Value = 0 And Row.Cells("Importe2").Value = 0 Then
                MsgBox("Falta Importe en Remito " & NumeroEditado(Row.Cells("Remito").Value))
                Return False
            End If
        Next

        If CDbl(TextTotalB.Text) = 0 And CDbl(TextTotalN.Text) = 0 Then
            MsgBox("Falta Informar Neto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        For i As Integer = 0 To Grid.Rows.Count - 1
            Dim ImporteB As Double = 0
            Dim ComisionB As Double = 0
            Dim InsumosProduccionB As Double = 0
            Dim ServiciosProduccionB As Double = 0
            Dim OtrosConceptosB As Double = 0
            If Grid.Rows(i).Cells("Clave").Value = 1 Then ImporteB = Grid.Rows(i).Cells("ImporteB").Value
            If Grid.Rows(i).Cells("Clave").Value = 5 Then ComisionB = Grid.Rows(i).Cells("ImporteB").Value
            If Grid.Rows(i).Cells("Clave").Value = 7 Then InsumosProduccionB = Grid.Rows(i).Cells("ImporteB").Value
            If Grid.Rows(i).Cells("Clave").Value = 9 Then ServiciosProduccionB = Grid.Rows(i).Cells("ImporteB").Value
            If Grid.Rows(i).Cells("Clave").Value = 10 Then OtrosConceptosB = Grid.Rows(i).Cells("ImporteB").Value
            If Grid.Rows(i).Cells("Clave").Value = 3 Then
                If Grid.Rows(i).Cells("Alicuota").Value <> 0 And Grid.Rows(i).Cells("ImporteB").Value = 0 And ImporteB <> 0 Then
                    MsgBox("Falta Informar Importe en I.V.A.")
                    Return False
                End If
                If Grid.Rows(i).Cells("Alicuota").Value = 0 And Grid.Rows(i).Cells("ImporteB").Value <> 0 And ImporteB <> 0 Then
                    MsgBox("Falta Informar Alicuota en I.V.A.")
                    Return False
                End If
            End If
            If Grid.Rows(i).Cells("Clave").Value = 6 Then
                If Grid.Rows(i).Cells("Alicuota").Value <> 0 And Grid.Rows(i).Cells("ImporteB").Value = 0 And ComisionB <> 0 Then
                    MsgBox("Falta Informar Importe en I.V.A. Comisión.")
                    Return False
                End If
                If Grid.Rows(i).Cells("Alicuota").Value = 0 And Grid.Rows(i).Cells("ImporteB").Value <> 0 And ComisionB <> 0 Then
                    MsgBox("Falta Informar Alicuota en I.V.A. Comisión.")
                    Return False
                End If
            End If
            If Grid.Rows(i).Cells("Clave").Value = 8 Then
                If Grid.Rows(i).Cells("Alicuota").Value <> 0 And Grid.Rows(i).Cells("ImporteB").Value = 0 And InsumosProduccionB <> 0 Then
                    MsgBox("Falta Informar Importe en I.V.A.  Insumos de Produccion.")
                    Return False
                End If
                If Grid.Rows(i).Cells("Alicuota").Value = 0 And Grid.Rows(i).Cells("ImporteB").Value <> 0 And InsumosProduccionB <> 0 Then
                    MsgBox("Falta Informar Alicuota en I.V.A. Insumos de Produccion.")
                    Return False
                End If
            End If
            If Grid.Rows(i).Cells("Clave").Value = 11 Then
                If Grid.Rows(i).Cells("Alicuota").Value <> 0 And Grid.Rows(i).Cells("ImporteB").Value = 0 And ServiciosProduccionB <> 0 Then
                    MsgBox("Falta Informar Importe en I.V.A. Servicios de Produccion.")
                    Return False
                End If
                If Grid.Rows(i).Cells("Alicuota").Value = 0 And Grid.Rows(i).Cells("ImporteB").Value <> 0 And ServiciosProduccionB <> 0 Then
                    MsgBox("Falta Informar Alicuota en I.V.A. Servicios de Produccion.")
                    Return False
                End If
            End If
            If Grid.Rows(i).Cells("Clave").Value = 12 Then
                If Grid.Rows(i).Cells("Alicuota").Value <> 0 And Grid.Rows(i).Cells("ImporteB").Value = 0 And OtrosConceptosB <> 0 Then
                    MsgBox("Falta Informar Importe en I.V.A.Otros Conceptos.")
                    Return False
                End If
                If Grid.Rows(i).Cells("Alicuota").Value = 0 And Grid.Rows(i).Cells("ImporteB").Value <> 0 And OtrosConceptosB <> 0 Then
                    MsgBox("Falta Informar Alicuota en I.V.A. Otros Conceptos.")
                    Return False
                End If
            End If
            Select Case Grid.Rows(i).Cells("Clave").Value
                Case 5, 7, 9, 10
                    If Grid.Rows(i).Cells("Alicuota").Value <> 0 And (Grid.Rows(i).Cells("ImporteB").Value = 0) Then
                        MsgBox("Falta Informar Importe correspondiente al concepto IVA.")
                        Return False
                    End If
            End Select
        Next

        If Grid.Rows(0).Cells("ImporteB").Value = 0 And Grid.Rows(1).Cells("ImporteB").Value = 0 And CDbl(TextTotalB.Text) <> 0 Then
            MsgBox("Falta Informar Neto en Importe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        If TextFechaContable.Text = "" Then
            MsgBox("Falta Informar Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If Not ValidaFechaContable(CDate(TextFechaContable.Text), UltimaFechaContableW) Then
            TextFechaContable.Focus()
            Return False
        End If

        If Not ProveedorOpr And CDec(TextTotalB.Text) <> 0 Then
            If MsgBox("Proveedor Solo Opera a Candado Cerrado. Debe Cambiar Candado en Proveedor. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        CalculaTotal()

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "ImporteB" Or Grid.Columns(e.ColumnIndex).Name = "ImporteN" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else
                If Grid.Rows(e.RowIndex).Cells("Clave").Value = 1 Or Grid.Rows(e.RowIndex).Cells("Clave").Value = 2 Or Grid.Rows(e.RowIndex).Cells("Clave").Value = 3 Or Grid.Rows(e.RowIndex).Cells("Clave").Value = 90 Then
                    e.Value = FormatNumber(e.Value, GDecimales)
                Else
                    e.Value = "-" & FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Alicuota" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If e.KeyChar = "." Then e.KeyChar = ","
        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If CType(sender, TextBox).Text <> "" Then
            EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
        End If

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Iva") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
            If e.ProposedValue > 100 Then e.ProposedValue = 0
            Select Case e.Row("Clave")
                Case 3, 6, 8, 11, 12
                    For Each Item As Double In TablaIva
                        If Item = e.ProposedValue Then Exit Sub
                    Next
                    MsgBox("Alicuota no Existe.")
                    e.ProposedValue = 0
            End Select
        End If

        If e.Column.ColumnName.Equals("ImporteB") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

        If e.Column.ColumnName.Equals("ImporteN") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID de comprobantes.  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridCompro_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEnter

        If Not GridCompro.Columns(e.ColumnIndex).ReadOnly Then
            GridCompro.BeginEdit(True)
        End If

    End Sub
    Private Sub Gridcompro_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEndEdit

        CalculaTotal()

    End Sub
    Private Sub GridCompro_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles GridCompro.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If GridCompro.Columns(e.ColumnIndex).Name = "OrdenCompra" Then
            If PermisoTotal Then
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
            e.Value = NumeroEditado(e.Value)
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Remito" Then
            If Not IsDBNull(e.Value) Then
                e.Value = NumeroEditado(e.Value)
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Importe" Or GridCompro.Columns(e.ColumnIndex).Name = "Importe2" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Precio" Or GridCompro.Columns(e.ColumnIndex).Name = "Precio2" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else
                e.Value = FormatNumber(e.Value, 3)
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else
                e.Value = FormatNumber(e.Value, 0)
            End If
        End If

    End Sub
    Private Sub GridCompro_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles GridCompro.EditingControlShowing

        Dim columna As Integer = GridCompro.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPressCompro
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChangedCompro

    End Sub
    Private Sub ValidaKey_KeyPressCompro(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Precio" Or GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Precio2" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChangedCompro(ByVal sender As Object, ByVal e As System.EventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Precio" Or GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Precio2" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 3)
            End If
        End If

    End Sub
    Private Sub DtGridCompro_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If PLiquidacion <> 0 Then Exit Sub

        If e.Column.ColumnName.Equals("Cantidad") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If (e.Column.ColumnName.Equals("PrecioB")) Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca3(e.ProposedValue)
            If e.ProposedValue > 0 And e.Row("Operacion") = 2 Then
                MsgBox("Remito solo puede tener Importe(2).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                e.ProposedValue = 0
            End If
            e.Row("ImporteB") = CalculaNeto(e.Row("Cantidad"), e.ProposedValue)
        End If

        If (e.Column.ColumnName.Equals("PrecioN")) Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca3(e.ProposedValue)
            e.Row("ImporteN") = CalculaNeto(e.Row("Cantidad"), e.ProposedValue)
        End If

        GridCompro.Refresh()

    End Sub
    Private Sub Gridcompro_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles GridCompro.DataError
        Exit Sub
    End Sub

    
   
End Class