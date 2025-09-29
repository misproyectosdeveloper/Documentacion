Imports System.Transactions
Imports System.Drawing.Printing
Imports System.Math
Public Class UnaLiquidacionContable
    Public PLiquidacion As Double
    Public PAbierto As Boolean
    Public PProveedor As Integer
    Public PMontoComision As Double
    Public PActualizacionOk As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim DtGrid As DataTable
    Dim DtGrid1 As DataTable
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtDetalleConceptos As DataTable
    '
    Dim ListaDeRetenciones As New List(Of ItemIvaReten)
    Dim TablaIva(0) As Double
    Dim UltimoNumero As Double
    Dim ConexionLiquidacion As String
    Dim ConexionRelacionada As String
    Dim Relacionada As Double
    Dim UltimaFechaW As DateTime
    Dim UltimaFechaContableW As DateTime
    Dim TipoFactura As Integer
    Dim DocumentoAsiento As Integer
    Dim EsPreliquidacion As Boolean
    Dim ErrorImpresion As Boolean
    Dim RetencionManual As Boolean
    Dim Provincia As Integer
    Dim cb As ComboBox
    Dim Calle As String
    Dim Numero As Integer
    Dim Localidad As String
    'para impresion.
    Dim Paginas As Integer
    Dim Copias As Integer
    Dim CopiasSegunPuntoVenta As Integer = 0
    Dim UltimoPuntoVentaParaCopiaSeleccionado As Integer = 0
    Private Sub UnaLiquidacionContable_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(2) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False

        ComboProveedor.DataSource = ProveedoresDeFrutas()
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"

        If PLiquidacion = 0 Then
            OpcionProveedor.PSoloArticulos = True
            OpcionProveedor.PEsPreLiquidacion = True
            OpcionProveedor.PSoloNacional = True
            OpcionProveedor.ShowDialog()
            ComboProveedor.SelectedValue = OpcionProveedor.PProveedor
            PProveedor = OpcionProveedor.PProveedor
            OpcionProveedor.Dispose()
            If ComboProveedor.SelectedValue = 0 Then Me.Close() : Exit Sub
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        LlenaComboTablas(ComboPais, 28)

        LlenaCombosGrid()
        LlenaCombosGrid1()

        ArmaTablaIva(TablaIva)
        ArmaTipoIva(ComboTipoIva)

        PActualizacionOk = False

        CreaDtGrid()
        CreaDtGrid1()

        If Not MuestraDatos() Then Me.Close() : Exit Sub

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.ColumnChanged, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanged)
        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)
        AddHandler DtGrid.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid_RowChanged)

        If Not PermisoTotal Then LabelTr.Visible = False

        UltimaFechaW = UltimaFecha(Conexion)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

        UltimaFechaContableW = UltimaFechacontableLiquidacion(Conexion, GPuntoDeVenta, TipoFactura)
        If UltimaFechaContableW = "2/1/1000" Then
            MsgBox("Error Base de Datos al Leer Ultima Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PLiquidacion <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        If DtGrid.HasErrors Then
            MsgBox("Debe Corregir Errores. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Row As DataRow
        Dim DtCabezaB As New DataTable
        Dim DtdetalleB As New DataTable
        Dim DtdetalleConceptosB As New DataTable

        DtCabezaB = DtCabeza.Clone
        DtdetalleB = DtDetalle.Clone
        DtdetalleConceptosB = DtDetalleConceptos.Clone

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGrid1.Select("Concepto = 1")
        Dim BrutoB As Decimal = RowsBusqueda(0).Item("Importe")
        RowsBusqueda = DtGrid1.Select("Concepto = 2")
        Dim AlicuotaB As Decimal = RowsBusqueda(0).Item("Valor")
        RowsBusqueda = DtGrid1.Select("Concepto = 3")
        Dim ComisionB As Decimal = -RowsBusqueda(0).Item("Importe")
        Dim PorComisionB As Decimal = RowsBusqueda(0).Item("Valor")
        RowsBusqueda = DtGrid1.Select("Concepto = 4")
        Dim DescargaB As Decimal = -RowsBusqueda(0).Item("Importe")
        RowsBusqueda = DtGrid1.Select("Concepto = 11")
        Dim NetoB As Decimal = RowsBusqueda(0).Item("Importe")
        RowsBusqueda = DtGrid1.Select("Concepto = 5")
        Dim AlicuotaComisionB As Decimal = RowsBusqueda(0).Item("Valor")
        RowsBusqueda = DtGrid1.Select("Concepto = 6")
        Dim AlicuotaDescargaB As Decimal = RowsBusqueda(0).Item("Valor")

        Row = DtCabezaB.NewRow
        Row("Proveedor") = PProveedor
        Row("Tr") = True
        Row("Rel") = False
        Row("Nrel") = 0
        Row("EsReventa") = False
        Row("EsConsignacion") = False
        Row("EsInsumos") = False
        Row("Fecha") = DateTime1.Value
        Row("FechaContable") = CDate(TextFechaContable.Text)
        Row("Proveedor") = PProveedor
        Row("Bruto") = BrutoB
        Row("Alicuota") = AlicuotaB
        Row("Comision") = PorComisionB
        Row("AlicuotaComision") = AlicuotaComisionB
        Row("Descarga") = DescargaB
        Row("AlicuotaDescarga") = AlicuotaDescargaB
        Row("Directo") = 100
        Row("Factura") = 0
        Row("Neto") = NetoB
        Row("Saldo") = NetoB
        Row("Estado") = 1
        Row("Impreso") = False
        Row("Comentario") = TextComentario.Text
        Row("Importe") = NetoB
        Row("Senia") = 0
        DtCabezaB.Rows.Add(Row)
        For Each Row1 As DataRow In DtGrid.Rows
            Row = DtdetalleB.NewRow
            Row("Lote") = Row1("Lote")
            Row("Secuencia") = Row1("Articulo")
            Row("RemitoGuia") = Row1("RemitoGuia")
            Row("Operacion") = 1
            Row("Alicuota") = AlicuotaB
            Row("Merma") = Row1("Merma")
            Row("Cantidad") = Row1("Cantidad")
            Row("PrecioS") = 0
            Row("Precio") = Row1("PrecioF")
            Row("Importe") = Row1("Total")
            Row("NetoConIva") = 0
            Row("NetoSinIva") = 0
            DtdetalleB.Rows.Add(Row)
        Next
        Dim Item As Integer = 0
        For Each Row1 As DataRow In DtGrid1.Rows
            Row = DtdetalleConceptosB.NewRow
            Item = Item + 1
            Row("Item") = Item
            Row("Concepto") = Row1("Concepto")
            Row("Valor") = Row1("Valor")
            Row("Importe") = Row1("Importe")
            If Row("Importe") < 0 Then Row("Importe") = -Row("Importe")
            If Row("Importe") <> 0 Then DtdetalleConceptosB.Rows.Add(Row)
        Next

        'Arma Tipo Operaciones de Lotes para Asientos.
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        '
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaB) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleB) Then Me.Close() : Exit Sub
            If DtCabezaB.Rows.Count <> 0 Then
                If Not ArmaArchivosAsiento("A", DtCabezaB, DtdetalleConceptosB, DtAsientoCabezaB, DtAsientoDetalleB) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            End If
        End If

        'Graba Liquidacion.
        Dim NumeroAsientoB As Double = 0
        Dim NumeroW As Double

        For i As Integer = 1 To 50
            'Actualiza numeracion Liquidacion B.
            If DtCabezaB.Rows.Count <> 0 Then
                'Cabeza.
                DtCabezaB.Rows(0).Item("Liquidacion") = UltimoNumero
                DtCabezaB.Rows(0).Item("Interno") = UltimoNumeroInternoLiquidacion(TipoFactura, Conexion)
                If DtCabezaB.Rows(0).Item("Interno") < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close()
                    Exit Sub
                End If
                DtCabezaB.Rows(0).Item("Nrel") = 0
                'Detalle.
                For Each Row1 As DataRow In DtdetalleB.Rows
                    Row1("Liquidacion") = UltimoNumero
                Next
                For Each Row1 As DataRow In DtdetalleConceptosB.Rows
                    Row1("Liquidacion") = UltimoNumero
                Next
            End If
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                NumeroAsientoB = UltimaNumeracionAsiento(Conexion)
                If NumeroAsientoB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close()
                    Exit Sub
                End If
            End If
            'Actualiza Asientos.
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                DtAsientoCabezaB.Rows(0).Item("Asiento") = NumeroAsientoB
                DtAsientoCabezaB.Rows(0).Item("Documento") = UltimoNumero
                For Each Row In DtAsientoDetalleB.Rows
                    Row("Asiento") = NumeroAsientoB
                Next
            End If
            '
            NumeroW = AltaLiquidacion(DtCabezaB, DtdetalleB, DtdetalleConceptosB, DtAsientoCabezaB, DtAsientoDetalleB)
            '
            If NumeroW >= 0 Then Exit For
            If NumeroW = -2 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -10 Then
            MsgBox("Numero de Liquidacion Ya Fue Impresa. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = -2 Then
            MsgBox("ERROR en Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PActualizacionOk = True
            PLiquidacion = DtCabezaB.Rows(0).Item("Liquidacion")
            PAbierto = True
            If Not MuestraDatos() Then Me.Close() : Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub CheckRetencionManual_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckRetencionManual.CheckedChanged

        If CheckRetencionManual.Checked Then
            RetencionManual = True
        Else
            RetencionManual = False
            CalculaTotales()
        End If

        PresentaGrid1Retenciones()
        Grid1.Refresh()

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        'http://vb-helper.com/howto_net_print_and_preview.html

        If PLiquidacion = 0 Then
            MsgBox("Opcion Invalida. Liquidación debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Impreso") And PAbierto Then
            If MsgBox("Liquidación Ya fue Impresa. Quiere Re-Imprimir?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        ErrorImpresion = False
        Paginas = 0

        Dim PuntoVentaW As Integer = Val(Strings.Mid(DtCabeza.Rows(0).Item("Liquidacion"), 2, 4))
        If PAbierto And (CopiasSegunPuntoVenta = 0 Or PuntoVentaW <> UltimoPuntoVentaParaCopiaSeleccionado) Then
            UltimoPuntoVentaParaCopiaSeleccionado = PuntoVentaW
            CopiasSegunPuntoVenta = TraeCopiasComprobante(4, PuntoVentaW)
            If CopiasSegunPuntoVenta < 0 Then CopiasSegunPuntoVenta = 0 : MsgBox("Error al Leer Tabla: PuntosDeVenta. Operacion se CANCELA.", MsgBoxStyle.Critical) : Exit Sub
        End If

        Copias = CopiasSegunPuntoVenta

        Dim print_document As New PrintDocument
        AddHandler print_document.PrintPage, AddressOf Print_PrintPage

        print_document.Print()

        If ErrorImpresion Then Exit Sub

        If Not GrabaImpreso() Then Exit Sub
        DtCabeza.Rows(0).Item("Impreso") = True

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

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Liquidacion ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If CierreContableCerrado(CDate(TextFechaContable.Text).Month, CDate(TextFechaContable.Text).Year) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If HallaAsignacion(10, DtCabeza.Rows(0).Item("Liquidacion"), DtCabeza.Rows(0).Item("Proveedor"), ConexionLiquidacion) > 0 Then
            MsgBox("Liquidación imputada en cuenta corriente.  Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim DtAsientoCabeza As New DataTable
        '
        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(DocumentoAsiento, DtCabeza.Rows(0).Item("Liquidacion"), DtAsientoCabeza, ConexionLiquidacion) Then Me.Close() : Exit Sub
            If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3
        End If

        If MsgBox("Liquidación se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        DtCabezaAux.Rows(0).Item("Estado") = 3

        If Not AnulaLiquidacion(DtCabezaAux, DtAsientoCabeza) Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        Else
            MsgBox("Liquidación Anulada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PActualizacionOk = True
        End If

        If Not MuestraDatos() Then Me.Close() : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PLiquidacion = 0
        UnaLiquidacionContable_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonDatosCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDatosEmisor.Click

        If ComboProveedor.SelectedValue = 0 Then Exit Sub

        UnDatosEmisor.PEsProveedor = True

        UnDatosEmisor.PEmisor = ComboProveedor.SelectedValue
        UnDatosEmisor.ShowDialog()

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then
            MsgBox("No hay un registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If
        Grid.Rows.Remove(Grid.CurrentRow)

        CalculaTotales()

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PLiquidacion = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = DocumentoAsiento
        ListaAsientos.PDocumentoB = PLiquidacion
        ListaAsientos.PDocumentoN = 0

        ListaAsientos.Show()

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueContable.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Function MuestraDatos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PAbierto Then
            ConexionLiquidacion = Conexion
            ConexionRelacionada = ConexionN
        Else : ConexionLiquidacion = ConexionN
            ConexionRelacionada = Conexion
        End If
        If PermisoTotal Then
            If PAbierto Then
                PictureCandado.Image = ImageList1.Images.Item("Abierto")
            Else : PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            End If
        End If

        DtCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM LiquidacionCabeza WHERE Liquidacion = " & PLiquidacion & ";", ConexionLiquidacion, DtCabeza) Then Return False
        If PLiquidacion <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Liquidacion No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PLiquidacion <> 0 Then
            PProveedor = DtCabeza.Rows(0).Item("Proveedor")
        End If

        If Not LlenaDatosEmisor(PProveedor) Then Return False

        If PLiquidacion = 0 Then  'debe ir antes de AgregarCabeza.
            TipoFactura = LetrasPermitidasProveedor(HallaTipoIvaProveedor(PProveedor), 100)
            TextTipoFactura.Text = LetraTipoIva(TipoFactura)
            GPuntoDeVenta = HallaPuntoVentaSegunTipo(910, TipoFactura)
            If GPuntoDeVenta = 0 Then
                MsgBox("Usuario No tiene definido Punto de Venta para el Tipo de Iva de este Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Not ExistePuntoDeVenta(GPuntoDeVenta) Then
                MsgBox("Punto de Venta " & GPuntoDeVenta & " No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If EsPuntoDeVentaZ(GPuntoDeVenta) Then
                MsgBox("Punto de Venta del Operador " & GPuntoDeVenta & " Reservado Para Ticket Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            LabelPuntodeVenta.Text = "Punto de Venta " & Format(GPuntoDeVenta, "0000")
        End If

        If PLiquidacion = 0 Then
            If Not AgregaCabeza() Then Return False
        End If

        DocumentoAsiento = 913

        EnlazaCabeza()

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM LiquidacionDetalle WHERE Liquidacion = " & PLiquidacion & ";", ConexionLiquidacion, DtDetalle) Then Return False

        DtDetalleConceptos = New DataTable
        If Not Tablas.Read("SELECT * FROM LiquidacionDetalleConceptos WHERE Liquidacion = " & PLiquidacion & ";", ConexionLiquidacion, DtDetalleConceptos) Then Return False

        Relacionada = 0
        If Not PAbierto Then Relacionada = DtCabeza.Rows(0).Item("Nrel")
        If PAbierto And DtCabeza.Rows(0).Item("Rel") Then
            Relacionada = HallaRelacionada(DtCabeza.Rows(0).Item("Liquidacion"))
            If Relacionada < 0 Then Return False
        End If

        Dim Row As DataRow
        Dim Unidades As Decimal = 0

        DtGrid1.Clear()
        DtGrid.Clear()

        If PLiquidacion = 0 Then
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 1
            Row("Valor") = 0
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 2
            Row("Valor") = 0
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 3
            Row("Valor") = 0  'Comision
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 4
            Row("Valor") = 0
            Row("Importe") = 0 'Descarga
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 5
            Row("Valor") = AgregaIvaComision(100)
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 6
            Row("Valor") = AgregaIvaDescarga(100)
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            AgregaRetenciones()
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 10
            Row("Valor") = 0
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            Row = DtGrid1.NewRow
            Row("Tipo") = 0
            Row("Concepto") = 11
            Row("Valor") = 0
            Row("Importe") = 0
            DtGrid1.Rows.Add(Row)
            '
            If PermisoTotal Then PictureCandado.Image = ImageList1.Images.Item("Abierto")
            TextUnidades.Text = Unidades
            TextFechaContable.Enabled = True
            PictureAlmanaqueContable.Enabled = True
            TextComentario.Enabled = True
            LabelPuntodeVenta.Visible = True
        Else
            For Each Row In DtDetalle.Rows
                Dim Row1 As DataRow = DtGrid.NewRow
                Row1("Lote") = Row("Lote")
                Row1("Secuencia") = Row("Secuencia")
                Row1("Articulo") = Row("Secuencia")  'Representa al articulo en el archivo, ya que no se usa la secuencia.
                Row1("RemitoGuia") = Row("RemitoGuia")
                Row1("Ingresados") = Row("Cantidad") + Row("Merma")
                Row1("Cantidad") = Row("Cantidad")
                Unidades = Unidades + Row("Cantidad")
                Row1("Merma") = Row("Merma")
                Row1("PrecioF") = Row("Precio")
                Row1("Total") = Row("Importe")
                Row1("AGranel") = False
                Row1("Medida") = ""
                HallaAGranelYMedida(Row1("Articulo"), Row1("AGranel"), Row1("Medida"))
                DtGrid.Rows.Add(Row1)
            Next
            For Each Row1 As DataRow In DtDetalleConceptos.Rows
                Dim Row2 As DataRow = DtGrid1.NewRow
                Row2("Tipo") = HallaCodigoRetencion(Row1("Concepto"))
                Row2("Concepto") = Row1("Concepto")
                Row2("Valor") = Row1("Valor")
                Row2("Importe") = Row1("Importe")
                If Row2("Concepto") <> 1 And Row2("Concepto") <> 2 And Row2("Concepto") <> 11 Then
                    Row2("Importe") = -Row2("Importe")
                End If
                DtGrid1.Rows.Add(Row2)
            Next
            TextUnidades.Text = Unidades
            TextFechaContable.Enabled = False
            PictureAlmanaqueContable.Enabled = False
            TextComentario.Enabled = False
            LabelPuntodeVenta.Visible = False
        End If

        Grid.DataSource = bs
        bs.DataSource = DtGrid
        Grid1.DataSource = DtGrid1

        If PLiquidacion = 0 Then
            PresentaGrid1(False)
            Grid.ReadOnly = False
            ButtonEliminarLinea.Enabled = True
        Else
            PresentaGrid1(True)
            Grid.ReadOnly = True
            ButtonEliminarLinea.Enabled = False
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub EnlazaCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Row As DataRowView = MiEnlazador.Current

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Liquidacion")
        AddHandler Enlace.Format, AddressOf Formatliquidacion
        AddHandler Enlace.Parse, AddressOf FormatParseLiquidacion
        MaskedLiquidacion.DataBindings.Clear()
        MaskedLiquidacion.DataBindings.Add(Enlace)

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

        Numero.Value = CDbl(TipoFactura & Format(Val(MaskedLiquidacion.Text), "000000000000"))

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

        Row = DtCabeza.NewRow()
        ArmaNuevaLiquidacion(Row)
        Row("Liquidacion") = UltimoNumero
        Row("EsReventa") = True
        Row("EsConsignacion") = False
        Row("Proveedor") = PProveedor
        Row("Fecha") = DateTime1.Value
        Row("Tr") = True

        DtCabeza.Rows.Add(Row)

        Return True

    End Function
    Private Function AltaLiquidacion(ByVal DtCabezaB As DataTable, ByVal DtDetalleB As DataTable, ByVal DtDetalleConceptosB As DataTable, ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable) As Double

        'CUIDADO: en GrabaTabla siempre poner getChange de la tabla para que tome los cambio cuando pase por segunda ves.

        Dim NumeroW As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not ReGrabaUltimaNumeracionLiquidacion(DtCabezaB.Rows(0).Item("Liquidacion"), TipoFactura) Then Return -10
                NumeroW = GrabaTabla(DtCabezaB.GetChanges, "LiquidacionCabeza", Conexion)
                If NumeroW <= 0 Then Return NumeroW
                NumeroW = GrabaTabla(DtDetalleB.GetChanges, "LiquidacionDetalle", Conexion)
                If NumeroW <= 0 Then Return NumeroW
                NumeroW = GrabaTabla(DtDetalleConceptosB.GetChanges, "LiquidacionDetalleConceptos", Conexion)
                If NumeroW <= 0 Then Return NumeroW
                '
                ' graba Asiento B.
                If DtAsientoCabezaB.Rows.Count <> 0 Then
                    NumeroW = GrabaTabla(DtAsientoCabezaB.GetChanges, "AsientosCabeza", Conexion)
                    If NumeroW <= 0 Then Return NumeroW
                    NumeroW = GrabaTabla(DtAsientoDetalleB.GetChanges, "AsientosDetalle", Conexion)
                    If NumeroW <= 0 Then Return NumeroW
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
    Private Function AnulaLiquidacion(ByVal DtCabezaAux As DataTable, ByVal DtAsientoCabeza As DataTable) As Boolean

        Dim MitransactionOptions As New TransactionOptions
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                'Graba Cabeza de la Liquidacion.
                If GrabaTabla(DtCabezaAux, "LiquidacionCabeza", ConexionLiquidacion) <= 0 Then Return False
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    If GrabaTabla(DtAsientoCabeza, "AsientosCabeza", ConexionLiquidacion) <= 0 Then Return False
                End If
                '
                Scope.Complete()
                Return True
            End Using
        Catch ex As TransactionException
            Return False
        Finally
        End Try

    End Function
    Private Sub CalculaTotales()

        If PLiquidacion <> 0 Then Exit Sub

        Dim NetoB As Decimal = 0
        Dim IvaB As Decimal = 0
        Dim BrutoB As Decimal = 0
        Dim Unidades As Decimal = 0
        Dim RowsBusqueda() As DataRow

        For I As Integer = 0 To Grid.Rows.Count - 2
            Unidades = Unidades + Grid.Rows(I).Cells("Cantidad").Value
            BrutoB = BrutoB + Grid.Rows(I).Cells("Total").Value
        Next

        RowsBusqueda = DtGrid1.Select("Concepto = 2")
        Dim AlicuotaSobreVentaT As Decimal = RowsBusqueda(0).Item("Valor")
        RowsBusqueda = DtGrid1.Select("Concepto = 3")
        Dim ComisionT As Decimal = RowsBusqueda(0).Item("Valor")
        RowsBusqueda = DtGrid1.Select("Concepto = 4")
        Dim DescargaT As Decimal = RowsBusqueda(0).Item("Importe")
        If DescargaT < 0 Then DescargaT = -DescargaT
        RowsBusqueda = DtGrid1.Select("Concepto = 5")
        Dim AlicuotaComisionT As Decimal = RowsBusqueda(0).Item("Valor")
        RowsBusqueda = DtGrid1.Select("Concepto = 6")
        Dim AlicuotaDescargaT As Decimal = RowsBusqueda(0).Item("Valor")
        Dim IvaDescargaW As Decimal = Trunca(DescargaT * AlicuotaDescargaT / 100)

        'Reten. Ingreso Bruto.
        Dim AlicuotaProvincia As Decimal = 0
        Dim RetencionesW As Decimal = 0

        Dim Fecha As Date
        Fecha = DateTime1.Value

        RowsBusqueda = DtGrid1.Select("Tipo = 1")   'Tipo = 1 es Reten/Perc.
        For Each Row As DataRow In RowsBusqueda
            For Each Fila As ItemIvaReten In ListaDeRetenciones
                If Fila.Clave = Row("Concepto") Then
                    If Fila.Formula = 0 Or RetencionManual Then
                        Fila.Importe = Abs(Row("Importe"))
                    Else
                        Fila.Importe = 0
                        If Fila.Formula = 2 Then
                            AlicuotaProvincia = Row("Valor")
                            Fila.Importe = Trunca(AlicuotaProvincia / 100 * BrutoB)
                        End If
                        If Fila.Formula = 1 Then
                            Dim Retencion As Decimal = CalculaRetencionFormulaLiquidacionProv1(Fila.Nombre, PProveedor, Month(Fecha), Year(Fecha), BrutoB, AlicuotaSobreVentaT, Fila.Clave, Fila.Base, Fila.Alicuota)
                            If Retencion > 0 Then
                                Fila.Importe = Retencion
                            End If
                        End If
                    End If
                    RetencionesW = RetencionesW + Fila.Importe
                End If
            Next
        Next

        TextUnidades.Text = FormatNumber(Unidades, GDecimales)
        IvaB = Trunca(BrutoB * AlicuotaSobreVentaT / 100)
        Dim ImporteComisionW As Decimal = Trunca(BrutoB * ComisionT / 100)
        Dim IvaImporteComisionW As Decimal = Trunca(ImporteComisionW * AlicuotaComisionT / 100)

        NetoB = BrutoB + IvaB - ImporteComisionW - IvaImporteComisionW - DescargaT - IvaDescargaW - RetencionesW

        For Each Row As DataRow In DtGrid1.Rows
            Select Case Row("Concepto")
                Case 1
                    Row("Importe") = BrutoB
                Case 2
                    Row("Importe") = IvaB
                Case 3
                    Row("Importe") = -ImporteComisionW
                Case 4
                    If Row("Importe") > 0 Then Row("Importe") = -Row("Importe")
                Case 5
                    Row("Importe") = -IvaImporteComisionW
                Case 6
                    Row("Importe") = -IvaDescargaW
                Case 11
                    Row("Importe") = NetoB
                Case Else
                    If Row("Tipo") = 1 Then
                        For Each Fila As ItemIvaReten In ListaDeRetenciones
                            If Fila.Clave = Row("Concepto") Then Row("Importe") = -Fila.Importe
                        Next
                    End If
            End Select
        Next

    End Sub
    Private Sub AgregaRetenciones()

        ListaDeRetenciones = HallaRetencionesAplicables(10, 1, PProveedor, DateTime1.Value)
        For Each Fila As ItemIvaReten In ListaDeRetenciones
            If Fila.Formula = 2 Then
                Dim AlicuotaProvinciaW As Decimal = HallaAlicuotaRetIngBruto(Provincia)
                If AlicuotaProvinciaW < 0 Then
                    MsgBox("Error Base de Datos al Leer tabla de Provincias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close() : Exit Sub
                End If
                Dim Row As DataRow = DtGrid1.NewRow
                Row("Tipo") = 1
                Row("Concepto") = Fila.Clave
                Row("Valor") = AlicuotaProvinciaW
                Row("Importe") = 0
                DtGrid1.Rows.Add(Row)
            End If
            If Fila.Formula = 1 Then
                Dim Row As DataRow = DtGrid1.NewRow
                Row("Tipo") = 1
                Row("Concepto") = Fila.Clave
                Row("Valor") = Fila.Alicuota
                Row("Importe") = 0
                DtGrid1.Rows.Add(Row)
            End If
            If Fila.Formula = 0 Then
                Dim Row As DataRow = DtGrid1.NewRow
                Row("Tipo") = 1
                Row("Concepto") = Fila.Clave
                Row("Valor") = 0
                Row("Importe") = 0
                DtGrid1.Rows.Add(Row)
            End If
        Next

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Ingresados As New DataColumn("Ingresados")
        Ingresados.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Ingresados)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Merma As New DataColumn("Merma")
        Merma.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Merma)

        Dim PrecioF As New DataColumn("PrecioF")
        PrecioF.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioF)

        Dim Total As New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Total)

        Dim RemitoGuia As New DataColumn("RemitoGuia")
        RemitoGuia.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(RemitoGuia)

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Medida)

        Dim AGranel As New DataColumn("AGranel")
        AGranel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(AGranel)

    End Sub
    Private Sub CreaDtGrid1()

        DtGrid1 = New DataTable

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Tipo)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Concepto)

        Dim Valor As New DataColumn("Valor")
        Valor.DataType = System.Type.GetType("System.Decimal")
        DtGrid1.Columns.Add(Valor)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGrid1.Columns.Add(Importe)

    End Sub
    Private Function TodosLosConceptos() As DataTable

        Dim Dt As New DataTable

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        Dt.Columns.Add(Nombre)

        Try
            Dim Row As DataRow = Dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 1
            Row("Nombre") = "Venta Bruta"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 2
            Row("Nombre") = "IVA Sobre Venta"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 3
            Row("Nombre") = "Comisión"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 4
            Row("Nombre") = "Descarga"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 5
            Row("Nombre") = "IVA Comisión"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 6
            Row("Nombre") = "IVA Descarga"
            Dt.Rows.Add(Row)
            '
            If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 25;", Conexion, Dt) Then End
            '
            Row = Dt.NewRow
            Row("Clave") = 10
            Row("Nombre") = "Neto Anterior"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 11
            Row("Nombre") = "Total Neto"
            Dt.Rows.Add(Row)
            '
            Return Dt
        Catch ex As Exception
            Dt.Dispose()
        End Try

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Articulo.DataSource = TodosLosArticulos()
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Sub LlenaCombosGrid1()

        Concepto.DataSource = TodosLosConceptos()
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

    End Sub
    Private Function LlenaDatosEmisor(ByVal Proveedor As Integer) As Boolean

        Dim Dta As New DataTable
        Dim Sql As String = ""

        Sql = "SELECT * FROM Proveedores WHERE Clave = " & Proveedor & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, Proveedor ya no existe o error en la Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Calle = Dta.Rows(0).Item("Calle")
        Numero = Dta.Rows(0).Item("Numero")
        Localidad = Dta.Rows(0).Item("Localidad")
        TextProvincia.Text = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        Provincia = Dta.Rows(0).Item("Provincia")
        ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
        TextCuit.Text = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")
        If Dta.Rows(0).Item("TipoIva") = 6 And PLiquidacion = 0 Then
            MsgBox("Tipo Iva MONOTRIBUTO no puede hacer liquidaciones.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Not Dta.Rows(0).Item("Opr") And PLiquidacion = 0 Then
            If MsgBox("Proveedor Solo Opera a Candado Cerrado. Debe Cambiar Candado en Proveedor. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
        End If

        Dta.Dispose()

        Return True

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Dim Patron As String = TipoFactura & Format(GPuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Liquidacion) FROM LiquidacionCabeza WHERE CAST(CAST(LiquidacionCabeza.Liquidacion AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(TipoFactura & Format(GPuntoDeVenta, "0000") & Format(1, "00000000"))
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
    Private Function HallaRelacionada(ByVal Liquidacion As Double) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Liquidacion FROM LiquidacionCabeza WHERE Nrel = " & Liquidacion & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos.  " & ex.Message)
            Return -1
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
    Private Function IvaExiste(ByVal Tablaiva() As Double, ByVal Alicuota As Double) As Boolean

        For Each Item As Double In Tablaiva
            If Item = Alicuota Then Return True
        Next

    End Function
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtFacturaCabeza As DataTable, ByVal DtDetalleConceptos As DataTable, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow

        Dim Comision As Decimal = Trunca(DtFacturaCabeza.Rows(0).Item("Comision") * DtFacturaCabeza.Rows(0).Item("Bruto") / 100)
        '
        Dim Item As New ItemListaConceptosAsientos

        If DtFacturaCabeza.Rows(0).Item("Alicuota") <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = HallaClaveIva(DtFacturaCabeza.Rows(0).Item("Alicuota"))
            Item.Importe = Trunca(DtFacturaCabeza.Rows(0).Item("Bruto") * DtFacturaCabeza.Rows(0).Item("Alicuota") / 100)
            Item.TipoIva = 5
            If Item.Importe <> 0 Then ListaIVA.Add(Item)
        End If
        '
        If DtFacturaCabeza.Rows(0).Item("AlicuotaComision") <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = HallaClaveIva(DtFacturaCabeza.Rows(0).Item("AlicuotaComision"))
            Item.Importe = Trunca(Comision * DtFacturaCabeza.Rows(0).Item("AlicuotaComision") / 100)
            Item.TipoIva = 6
            If Item.Importe <> 0 Then ListaIVA.Add(Item)
        End If
        '
        If DtFacturaCabeza.Rows(0).Item("AlicuotaDescarga") <> 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = HallaClaveIva(DtFacturaCabeza.Rows(0).Item("AlicuotaDescarga"))
            Item.Importe = Trunca(DtFacturaCabeza.Rows(0).Item("Descarga") * DtFacturaCabeza.Rows(0).Item("AlicuotaDescarga") / 100)
            Item.TipoIva = 6
            If Item.Importe <> 0 Then ListaIVA.Add(Item)
        End If
        '
        For Each Row As DataRow In DtDetalleConceptos.Rows
            RowsBusqueda = DtGrid1.Select("Concepto = " & Row("Concepto"))
            If RowsBusqueda.Length <> 0 Then
                If RowsBusqueda(0).Item("Tipo") = 1 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("Concepto")
                    Item.Importe = Row("Importe")
                    Item.TipoIva = 11    'Debito fiscal. 
                    If Item.Importe <> 0 Then ListaRetenciones.Add(Item)
                End If
            End If
        Next
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 202
        Item.Importe = DtFacturaCabeza.Rows(0).Item("Bruto")
        If Item.Importe <> 0 Then ListaConceptos.Add(Item)
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 208
        Item.Importe = Comision
        If Item.Importe <> 0 Then ListaConceptos.Add(Item)
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 207
        Item.Importe = DtFacturaCabeza.Rows(0).Item("Descarga")
        If Item.Importe <> 0 Then ListaConceptos.Add(Item)
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = DtFacturaCabeza.Rows(0).Item("Neto")
        If Item.Importe <> 0 Then ListaConceptos.Add(Item)
        '
        If Funcion = "A" Then
            If Not Asiento(DocumentoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, CDate(TextFechaContable.Text), 0) Then Return False
        End If

        Return True

    End Function
    Public Function GrabaImpreso() As Boolean

        Dim Sql As String = "UPDATE LiquidacionCabeza Set Impreso = 1 WHERE Liquidacion = " & DtCabeza.Rows(0).Item("Liquidacion") & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionLiquidacion)
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

        x = MIzq : y = MTop + 46

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            PrintFont = New Font("Courier New", 11)
            'Titulos.
            Texto = TextFechaContable.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 130, y - 25)
            If PAbierto Or EsPreliquidacion Then
                Texto = "REMITENTE  : " & ComboProveedor.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                If Numero <> 0 Then
                    Texto = "DOMICILIO  : " & Calle & " No: " & Numero
                Else
                    Texto = "DOMICILIO  : " & Calle
                End If
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "LOCALIDAD  : " & Localidad
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                Texto = "PROVINCIA : " & TextProvincia.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 100, y)
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
            Dim LineaLote As Integer = x + 20
            Dim LineaArticulo As Integer = x + 73
            Dim LineaRemito As Integer = x + 97
            Dim LineaIngresado As Integer = x + 112
            Dim LineaMerma As Integer = x + 126
            Dim LineaCantidad As Integer = x + 141 '144
            Dim LineaPrecio As Integer = x + 161
            Dim LineaTotal As Integer = x + Ancho
            Dim LineaResumen As Integer = x + 140
            Dim Longi As Double
            Dim Articulo As String
            Dim Partes() As String
            Dim Xq As Integer

            y = MTop + 64

            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaLote, y, LineaLote, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaArticulo, y, LineaArticulo, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaRemito, y, LineaRemito, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaIngresado, y, LineaIngresado, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaMerma, y, LineaMerma, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCantidad, y, LineaCantidad, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaPrecio, y, LineaPrecio, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaTotal, y, LineaTotal, y + Alto)
            'Titulos de descripcion.
            PrintFont = New Font("Courier New", 9)
            Texto = "LOTE"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 1, y + 2)
            Texto = "ARTICULO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaArticulo - Longi - 36  '40
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "REM/GUIA"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaRemito - Longi - 5
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "INGRESO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaIngresado - Longi - 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "MERMA"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaMerma - Longi - 2
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "A"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi - 5
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 1)
            Texto = "LIQUI."
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi - 1   'hugo
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 4)
            Texto = "PRECIO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaPrecio - Longi - 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaTotal - Longi - 3
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'Linea Horizontal 
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)

            'Descripcion de Articulos.
            PrintFont = New Font("Courier New", 8)
            y = y - SaltoLinea
            For Each Row As DataGridViewRow In Grid.Rows    'hugo
                y = y + SaltoLinea
                'Imprime Articulo.
                Partes = Split(Row.Cells("Articulo").FormattedValue, "(")
                Articulo = Partes(0)
                Texto = Articulo
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaLote
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime remito.
                Texto = Row.Cells("RemitoGuia").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaRemito - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime lotes Ingresado.
                Texto = Row.Cells("Ingresados").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaIngresado - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime Merma.
                Texto = Row.Cells("Merma").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaMerma - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime Cantidad.
                Dim Medida As String = Row.Cells("Medida").FormattedValue   'hugo
                If Medida.Length > 0 Then Medida = Medida.Substring(0, 2)   'hugo
                Texto = Row.Cells("Cantidad").FormattedValue & Medida       'hugo
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaCantidad - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime Precio.
                Texto = Row.Cells("PrecioF").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaPrecio - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
                'Imprime Importe.
                Texto = Row.Cells("Total").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaTotal - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            Next

            y = MTop + 191
            '
            'Imprime Totales.
            PrintFont = New Font("Courier New", 11)
            Texto = "TOTAL UNIDADES :   " & TextUnidades.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 50, y)
            y = MTop + 196
            For Each Row As DataGridViewRow In Grid1.Rows
                y = y + SaltoLinea
                Dim Valor As String = ""
                If Row.Cells("Valor").FormattedValue <> "" Then Valor = Row.Cells("Valor").FormattedValue & " %"
                Texto = Row.Cells("Concepto").FormattedValue & "  " & Valor
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 40, y)
                Texto = Row.Cells("Importe").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaResumen - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            Next

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
    Private Sub PresentaGrid1(ByVal EsTotal As Boolean)

        For Each Row1 As DataGridViewRow In Grid1.Rows
            If Row1.Cells("Concepto").Value = 5 Or Row1.Cells("Concepto").Value = 6 Then
                Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
            End If
            If EsTotal Then
                Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
            Else
                If Row1.Cells("Tipo").Value = 1 Then
                    Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                    Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
                End If
                If Row1.Cells("Concepto").Value = 2 Then
                    If GTipoIva = 2 Then
                        Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                    End If
                    Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
                End If
                If Row1.Cells("Concepto").Value = 1 Or Row1.Cells("Concepto").Value = 10 Or Row1.Cells("Concepto").Value = 11 Then
                    Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                    Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
                End If
                If Row1.Cells("Concepto").Value = 3 Then
                    Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
                End If
                If Row1.Cells("Concepto").Value = 4 Then
                    Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                End If
                If Row1.Cells("Concepto").Value = 5 Or Row1.Cells("Concepto").Value = 6 Then
                    Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
                End If

            End If
        Next

        'PrecentaGrid1 concepto retenciones segun codigo
        PresentaGrid1Retenciones()

    End Sub
    Private Sub PresentaGrid1Retenciones()

        For Each Row1 As DataGridViewRow In Grid1.Rows
            If Row1.Cells("Tipo").Value = 1 Then
                If PLiquidacion <> 0 Then
                    Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                    Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
                Else
                    If HallaFormulaRetencion(Row1.Cells("Concepto").Value) = 0 Or RetencionManual Then
                        Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                        Row1.Cells("Importe").ReadOnly = False : Row1.Cells("Importe").Style.BackColor = Color.White
                    Else
                        Row1.Cells("Valor").ReadOnly = True : Row1.Cells("Valor").Style.BackColor = Color.LightGray
                        Row1.Cells("Importe").ReadOnly = True : Row1.Cells("Importe").Style.BackColor = Color.LightGray
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
    Private Function Valida() As Boolean

        If Grid.Rows.Count = 1 Then
            MsgBox("Falta Informar Articulos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        For i As Integer = 0 To Grid.Rows.Count - 2
            If Grid.Rows(i).Cells("Articulo").Value = 0 Then
                MsgBox("Falta Articulo En Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.CurrentCell = Grid.Rows(i).Cells("Articulo")
                Grid.BeginEdit(True)
                Return False
            End If
            If Grid.Rows(i).Cells("Ingresados").Value = 0 Then
                MsgBox("Falta Ingresados En Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.CurrentCell = Grid.Rows(i).Cells("Ingresados")
                Grid.BeginEdit(True)
                Return False
            End If
            If TieneDecimales(Grid.Rows(i).Cells("Ingresados").Value) And Not Grid.Rows(i).Cells("AGranel").Value Then
                MsgBox("Ingresada no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Ingresados")
                Grid.BeginEdit(True)
                Return False
            End If
            If Grid.Rows(i).Cells("Merma").Value >= Grid.Rows(i).Cells("Ingresados").Value Then
                MsgBox("Merma Mayor o Igual que Ingresados En Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.CurrentCell = Grid.Rows(i).Cells("Merma")
                Grid.BeginEdit(True)
                Return False
            End If
            If TieneDecimales(Grid.Rows(i).Cells("Cantidad").Value) And Not Grid.Rows(i).Cells("AGranel").Value Then
                MsgBox("Cantidad no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Cantidad")
                Grid.BeginEdit(True)
                Return False
            End If
            If Grid.Rows(i).Cells("PrecioF").Value = 0 Then
                MsgBox("Falta Precio En Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.CurrentCell = Grid.Rows(i).Cells("PrecioF")
                Grid.BeginEdit(True)
                Return False
            End If
        Next

        If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 And PLiquidacion = 0 Then
            MsgBox("Fecha Incorrecta. Existe Otro Comprobante con Fecha " & UltimaFechaW & " Posterior a la Informada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
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

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtGrid1.Select("Concepto = 1")
        If RowsBusqueda(0).Item("Importe") <= 0 Then
            MsgBox("Venta Bruta Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        RowsBusqueda = DtGrid1.Select("Concepto = 2")
        If RowsBusqueda(0).Item("Valor") = 0 Then
            If MsgBox("No esta Informando IVA sobre Venta. Quiere Continuar de todas Formas?. (Y/N)", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Return False
            End If
        End If

        RowsBusqueda = DtGrid1.Select("Concepto = 11")
        If RowsBusqueda(0).Item("Importe") <= 0 Then
            MsgBox("Total Neto Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------               MANEJO DEL GRID.                 --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
            End If
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        If Not Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Grid.Rows(e.RowIndex).Cells("Articulo").Value <> 0 Then
                Grid.Rows(e.RowIndex).Cells("Cantidad").Value = Grid.Rows(e.RowIndex).Cells("Ingresados").Value - Grid.Rows(e.RowIndex).Cells("Merma").Value
                Grid.Rows(e.RowIndex).Cells("Total").Value = CalculaNeto(Grid.Rows(e.RowIndex).Cells("Cantidad").Value, Grid.Rows(e.RowIndex).Cells("PrecioF").Value)
                HallaAGranelYMedida(Grid.Rows(e.RowIndex).Cells("Articulo").Value, Grid.Rows(e.RowIndex).Cells("AGranel").Value, Grid.Rows(e.RowIndex).Cells("Medida").Value)
                CalculaTotales()
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "PrecioF" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = FormatNumber(e.Value, 3)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Total" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else
                    e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Ingresados" Or Grid.Columns(e.ColumnIndex).Name = "Merma" Or Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then e.Value = Format(0, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "RemitoGuia" Then
            If Not IsNothing(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else
                    e.Value = NumeroEditado(e.Value)
                End If
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Articulo" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioF" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Ingresados" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Merma" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "RemitoGuia" Then
            If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Ingresados" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Merma" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioF" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 3)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Lote") = 0
        e.Row("Secuencia") = 0
        e.Row("Articulo") = 0
        e.Row("RemitoGuia") = 0
        e.Row("Ingresados") = 0
        e.Row("Merma") = 0
        e.Row("Cantidad") = 0
        e.Row("PrecioF") = 0
        e.Row("Total") = 0
        e.Row("AGranel") = False
        e.Row("Medida") = ""

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Ingresados") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If
        If e.Column.ColumnName.Equals("Merma") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If
        If e.Column.ColumnName.Equals("RemitoGuia") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If
        If e.Column.ColumnName.Equals("PrecioF") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

    End Sub
    Private Sub DtGrid_ColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Articulo") Then
            If e.ProposedValue <> 0 Then
                Dim RowsBusqueda() As DataRow
                RowsBusqueda = DtGrid.Select("Articulo = " & e.ProposedValue)
                If RowsBusqueda.Length > 0 Then
                    MsgBox("Articulo Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    Dim Row As DataRowView = bs.Current
                    Row.Delete()
                End If
            End If
        End If

        If DtGrid.Rows.Count > GLineasPreLiquidacion Then
            MsgBox("Supera Cantidad Articulos Permitidos.", MsgBoxStyle.Information)
            Dim Row As DataRowView = bs.Current
            Row.Delete()
        End If

    End Sub
    Private Sub DtGrid_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        'Borra las lineas en blancos que aparecen cuando hago click en el ultimo renglon y sin informar nada regreso a algun renglon anterior. 
        If e.Row("Articulo") = 0 And e.Row("Ingresados") = 0 And e.Row("Cantidad") = 0 Then e.Row.Delete()

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------               MANEJO DEL GRID1.                --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid1_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellEnter

        If Not Grid1.Columns(e.ColumnIndex).ReadOnly Then
            Grid1.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid1_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellEndEdit

        If Grid1.Columns(e.ColumnIndex).Name = "Valor" Or Grid1.Columns(e.ColumnIndex).Name = "Importe" Then
            If IsDBNull(Grid1.CurrentCell.Value) Then Grid1.CurrentCell.Value = 0
        End If

        If Grid1.Columns(e.ColumnIndex).Name = "Valor" Then
            If Grid1.Rows(e.RowIndex).Cells("Concepto").Value = 2 Or Grid1.Rows(e.RowIndex).Cells("Concepto").Value = 5 Or Grid1.Rows(e.RowIndex).Cells("Concepto").Value = 6 Then
                If Not IvaExiste(TablaIva, Grid1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
                    MsgBox("Alicuota IVA. No Existe en el Sistema.", MsgBoxStyle.Critical)
                    Grid1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0
                    Grid1.Rows(e.RowIndex).Cells("Importe").Value = 0
                End If
            End If
        End If

        CalculaTotales()

    End Sub
    Private Sub Grid1_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid1.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid1.Columns(e.ColumnIndex).Name = "Valor" Or Grid1.Columns(e.ColumnIndex).Name = "Importe" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

    End Sub
    Private Sub Grid1_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid1.EditingControlShowing

        Dim columna As Integer = Grid1.CurrentCell.ColumnIndex

        If Grid1.Columns.Item(columna).Name = "Concepto" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress1
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged1

    End Sub
    Private Sub ValidaKey_KeyPress1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Valor" Or Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Importe" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged1(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Valor" Or Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Importe" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid1_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid1.DataError
        Exit Sub
    End Sub

End Class