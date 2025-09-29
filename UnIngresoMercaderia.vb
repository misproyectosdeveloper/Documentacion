Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnIngresoMercaderia
    Public PLote As Integer
    Public PAbierto As Boolean
    Public PConOrdenCompra As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Private DtLotes As DataTable
    Private DtIngreso As DataTable
    Private DtOrdenCompraDetalle As DataTable
    Private DtGrid As DataTable

    Private Abierto As Boolean
    Dim cb As ComboBox
    Dim ConexionLote As String
    Dim RemitoAnt As Double = 0
    Dim GuiaAnt As Double = 0
    Dim OrdenCompra As Double
    Dim Emisor As Integer
    Dim PEsCosteo As Boolean
    Dim SeniaMaxima As Double
    Dim MonedaProveedor As Integer
    Dim UltimaFechaW As DateTime
    Dim Lista As Integer
    Dim Zona As Integer
    Dim EmisorAnt As Integer
    Dim NegocioAnt As Integer
    Dim DepositoAnt As Integer
    'Para importacion.
    Dim DtImportacion As DataTable
    Dim RemitoImportacion As Decimal
    Dim FechaRemito As Date
    Dim Sucursal As Integer = 0
    'Para impresion.
    Dim ErrorImpresion As Boolean
    Dim LineasParaImpresion As Integer = 0
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim WithEvents ImpresoraEtiqueta As New System.Drawing.Printing.PrintDocument()
    Private Sub UnIngresoMercaderia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Para copiar datagridview al clipboard (portapapeles) poner en true MultiSelect del grid.  
        Me.Top = 50

        If Not PermisoEscritura(5) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False

        LlenaCombosGrid()  'poner antes de Opcion.

        Sucursal = 0
        FechaRemito = Now
        Lista = 0
        OrdenCompra = 0
        If PLote = 0 Then
            If Not Opcion() Then Me.Close() : Exit Sub
        End If

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PAbierto Then
            ConexionLote = Conexion
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionLote = ConexionN
        End If

        GModificacionOk = False

        SeniaMaxima = HallaSeniaMaxima()

        ArmaArchivos()

        If RemitoImportacion <> 0 Then
            For Each Row1 As DataRow In DtImportacion.Rows
                Dim Row2 As DataRow = DtGrid.NewRow
                Row2("Articulo") = Row1("Articulo")
                Row2("Cantidad") = Row1("Cantidad")
                DtGrid.Rows.Add(Row2)
            Next
            MaskedRemito.Text = Format(RemitoImportacion, "000000000000")
            RemitoImportacion = 0
        End If

    End Sub
    Private Sub UnIngresoMercaderia_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtLotesAux As DataTable = DtLotes.Copy
        Dim DtIngresoAux As DataTable = DtIngreso.Copy
        Dim DtOrdenCompraDetalleAux As DataTable = DtOrdenCompraDetalle.Copy

        If Not ActualizaArchivos(DtLotesAux) Then Exit Sub
        If OrdenCompra <> 0 Then
            If Not ActualizaOrdenCompra(DtOrdenCompraDetalleAux, DtGrid) Then Exit Sub
        End If

        If IsNothing(DtLotesAux.GetChanges) And IsNothing(DtIngresoAux.GetChanges) Then
            MsgBox("No hay Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable

        If GGeneraAsiento And Not IsNothing(DtLotesAux.GetChanges) Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtLotesAux, DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If PLote = 0 Then
            HacerAlta(DtIngresoAux, DtLotesAux, DtAsientoCabeza, DtAsientoDetalle, DtOrdenCompraDetalleAux)
        Else : HacerModificacion(DtIngresoAux, DtLotesAux, DtAsientoCabeza, DtAsientoDetalle, DtOrdenCompraDetalleAux)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PLote = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If HallaAsignacionLote(PLote) <> 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Hay Lotes del Ingreso Asignados. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If HallaLoteLiquidado(PLote) <> 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Hay Lotes Liquidados en Ingreso.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim dtTodosLoslotes As New DataTable
        If Not Tablas.Read("SELECT * FROM Lotes WHERE Lote = " & PLote & ";", ConexionLote, dtTodosLoslotes) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Hay Cambios, Debe Grabar Cambios ante de la Baja. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        For Each Row As DataRow In dtTodosLoslotes.Rows
            Row("Cantidad") = 0
            Row("Stock") = 0
        Next

        Dim DtIngresoAux As DataTable = DtIngreso.Copy
        DtIngresoAux.Rows(0).Item("Estado") = 3

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If Not HallaAsientosCabeza(6050, PLote, DtAsientoCabeza, ConexionLote) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        For Each Row As DataRow In DtAsientoCabeza.Rows
            Row("Estado") = 3
        Next

        If MsgBox("El Ingreso se Anulara. ¿Desea Anularlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim Dt As New DataTable

        Dim NumeroW As Integer = ActualizaIngreso("M", DtIngresoAux, dtTodosLoslotes, DtAsientoCabeza, DtAsientoDetalle, Dt)

        If NumeroW = -1 Or NumeroW = -2 Then
            MsgBox("ERROR, Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            GModificacionOk = True
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonRemitoCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRemitoCliente.Click

        If PLote = 0 Then
            MsgBox("Opcion Invalida. Ingreso debe ser Grabado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtIngreso.Rows(0).Item("RemitoCliente") <> 0 Then
            MsgBox("Ingreso Ya Tiene Remito. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Sql As String = "SELECT Lote,Secuencia,Articulo,(Cantidad - Baja) AS Cantidad FROM Lotes WHERE Cantidad > Baja AND Lote = " & PLote & " AND Secuencia < 100" & ";"

        Dim DtIngresoParaRemito As New DataTable
        If Not Tablas.Read(Sql, ConexionLote, DtIngresoParaRemito) Then Exit Sub
        If DtIngresoParaRemito.Rows.Count = 0 Then
            MsgBox("Ingreso Fue Devuelto.", MsgBoxStyle.Exclamation)
            DtIngresoParaRemito.Dispose()
            Exit Sub
        End If

        UnRemito.PAbiertoIngreso = PAbierto
        UnRemito.PDepositoIngreso = ComboDeposito.SelectedValue
        UnRemito.PIngreso = PLote
        UnRemito.PDtIngreso = DtIngresoParaRemito
        UnRemito.ShowDialog()
        DtIngresoParaRemito.Dispose()
        ArmaArchivos()

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PLote = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 6050
        If PAbierto Then
            ListaAsientos.PDocumentoB = PLote
        Else
            ListaAsientos.PDocumentoN = PLote
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        'http://vb-helper.com/howto_net_print_and_preview.html

        If PLote = 0 Then
            MsgBox("Opcion Invalida. Ingreso debe ser Grabado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        LineasParaImpresion = 0
        ErrorImpresion = False
        Paginas = 0
        Copias = 1

        Dim print_document As New PrintDocument
        AddHandler print_document.PrintPage, AddressOf Print_PrintPage

        print_document.Print()

        If ErrorImpresion Then Exit Sub

    End Sub
    Private Sub ButtonEtiquetado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEtiquetado.Click

        If PLote = 0 Then
            MsgBox("Debe Grabar El Ingreso! Operacion Se Cancela!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!")
            Exit Sub
        End If

        UnSeteoImpresora.SeteaImpresion(ImpresoraEtiqueta)
        ImpresoraEtiqueta.DefaultPageSettings.Landscape = True

        AddHandler ImpresoraEtiqueta.PrintPage, AddressOf ImprimeEtiqueta

        ImpresoraEtiqueta.Print()
        ImpresoraEtiqueta.Dispose()

    End Sub
    Private Sub ButtonRefrescar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRefrescar.Click

        If OrdenCompra <> 0 Then
            MsgBox("Precio Asigndo en Orden de Compra.") : Exit Sub
        End If

        If Lista = 0 Then
            MsgBox("No tiene Lista de Precios.") : Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PLote = 0 Then
            ArmaArticulosParaAltaParaGrid()
        Else
            ArmaArticulosParaModificacionParaGrid()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub Comboprovincia_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProvincia.Validating

        If IsNothing(ComboProvincia.SelectedValue) Then ComboProvincia.SelectedValue = 0

    End Sub
    Private Sub ComboTipoOperacion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipoOperacion.Validating

        If IsNothing(ComboTipoOperacion.SelectedValue) Then ComboTipoOperacion.SelectedValue = 0

    End Sub
    Private Sub TextGuia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextGuia.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextHora_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextHora.KeyPress

        If Asc(e.KeyChar) = 13 Then TextHora_Validating(Nothing, Nothing) : Exit Sub
        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextHora_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextHora.Validating

        If Val(TextHora.Text) > 23 Then
            MsgBox("Error Hora.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextHora.Text = "00"
        End If

    End Sub
    Private Sub TextMinuto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextMinuto.KeyPress

        If Asc(e.KeyChar) = 13 Then TextMinuto_Validating(Nothing, Nothing) : Exit Sub
        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextMinuto_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextMinuto.Validating

        If Val(TextMinuto.Text) > 59 Then
            MsgBox("Error Minuto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextMinuto.Text = "00"
        End If

    End Sub
    Private Sub ButtonEliminarLinea_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.GotFocus

        ButtonAceptar.Focus()

    End Sub
    Private Sub ArmaArchivos()                                 'ArmaArchivos

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Sql As String
        Dim Row As DataRow

        CreaDtGrid()

        DtIngreso = New DataTable
        If Not Tablas.Read("SELECT * FROM IngresoMercaderiasCabeza WHERE Lote = " & PLote & ";", ConexionLote, DtIngreso) Then Me.Close() : Exit Sub
        If PLote <> 0 And DtIngreso.Rows.Count = 0 Then
            MsgBox("Ingreso No Existen En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If DtIngreso.Rows.Count <> 0 Then
            Emisor = DtIngreso.Rows(0).Item("Proveedor")
            OrdenCompra = DtIngreso.Rows(0).Item("OrdenCompra")
        End If

        LlenaDatosEmisor(Emisor) 'antes de DtIngreso.Rows.Count = 0.

        If DtIngreso.Rows.Count = 0 Then
            Row = DtIngreso.NewRow
            Row("Proveedor") = Emisor
            Row("Lote") = 0
            Row("Remito") = 0
            Row("Guia") = 0
            Row("Provincia") = ComboProvincia.SelectedValue
            Row("Deposito") = ComboDeposito.SelectedValue
            Row("Fecha") = FechaRemito
            Row("Hora") = 0
            Row("Minuto") = 0
            Row("TipoOperacion") = ComboTipoOperacion.SelectedValue
            Row("Costeo") = 0
            Row("Moneda") = MonedaProveedor
            Row("Comentario") = ""
            Row("Sucursal") = Sucursal
            Row("RemitoCliente") = 0
            Row("OrdenCompra") = OrdenCompra
            DtIngreso.Rows.Add(Row)
        End If

        DtOrdenCompraDetalle = New DataTable
        If OrdenCompra <> 0 Then   'Lee ordenes de compra.
            If Not Tablas.Read("SELECT * FROM OrdenCompraDetalle WHERE Orden = " & OrdenCompra & ";", Conexion, DtOrdenCompraDetalle) Then Me.Close() : Exit Sub
        End If

        ComboCosteo.DataSource = Nothing
        ComboCosteo.DataSource = Tablas.Leer("SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & Emisor & ";")
        Row = ComboCosteo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        ComboCosteo.DataSource.rows.add(Row)
        ComboCosteo.DisplayMember = "Nombre"
        ComboCosteo.ValueMember = "Costeo"
        ComboCosteo.SelectedValue = 0
        With ComboCosteo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        VerSucursales(DtIngreso.Rows(0).Item("Proveedor"))

        RemitoAnt = DtIngreso.Rows(0).Item("Remito")
        GuiaAnt = DtIngreso.Rows(0).Item("Guia")

        MuestraCabeza()

        If ComboTipoOperacion.SelectedValue = 4 Then
            ComboTipoOperacion.Enabled = False
            PEsCosteo = True
        Else
            ComboTipoOperacion.Enabled = True
            PEsCosteo = False
        End If

        Sql = "SELECT * FROM Lotes WHERE Lotes.Lote = " & PLote & " AND Lotes.Lote = Lotes.LoteOrigen AND Lotes.Secuencia = Lotes.SecuenciaOrigen AND Lotes.Deposito = Lotes.DepositoOrigen;"

        DtLotes = New DataTable
        If Not Tablas.Read(Sql, ConexionLote, DtLotes) Then Me.Close() : Exit Sub
        If PLote <> 0 And DtLotes.Rows.Count = 0 Then
            MsgBox("Lotes No Existen En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        'Halla si tiene Lista.
        If PLote <> 0 And OrdenCompra = 0 Then
            Lista = HallaListaPrecios()
            If Lista = -1 Then
                MsgBox("No podra Modificar el Ingreso.", MsgBoxStyle.Information)
            End If
        End If

        'llena columna articulos del grid
        If PLote = 0 Then
            ArmaArticulosParaAltaParaGrid()
        Else
            ArmaArticulosParaModificacionParaGrid()  'Lee el archivo DtLotes.
        End If

        For Each Row1 As DataRow In DtLotes.Rows
            Row = DtGrid.NewRow
            Row("Lote") = Row1("Lote")
            Row("Secuencia") = Row1("Secuencia")
            Row("Articulo") = Row1("Articulo")
            Row("Secuencia") = Row1("Secuencia")
            Row("Calibre") = Row1("Calibre")
            Row("CantidadOriginal") = Row1("Cantidad")
            Row("Cantidad") = Row1("Cantidad")
            Row("Stock") = Row1("Stock")
            Row("Senia") = Row1("Senia")
            If Row("Senia") = -1 Then
                Row("Senia") = HallaSeniaArticulo(Row("Articulo"), Row1("Fecha"))
                If Row("Senia") < 0 Then
                    MsgBox("Error Base de Datos al Calcular Seña del Articulo.", MsgBoxStyle.Critical)
                    Me.Close()
                    Exit Sub
                End If
            End If
            Row("PermisoImp") = Row1("PermisoImp")
            Row("AGranel") = False
            Row("Medida") = ""
            HallaAGranelYMedida(Row1("Articulo"), Row("AGranel"), Row("Medida"))
            If OrdenCompra <> 0 Then
                Row("SaldoOrdenCompra") = HallaDatosOrdenCompra(DtOrdenCompraDetalle, Row("Articulo"))
            End If
            DtGrid.Rows.Add(Row)
        Next

        Grid.DataSource = DtGrid

        For I As Integer = 0 To Grid.Rows.Count - 2
            Grid.Rows(I).Cells("Articulo").ReadOnly = True
        Next

        If PLote = 0 Then
            ButtonAceptar.Text = "Graba Ingreso"
            ComboTipoOperacion.Enabled = True
            ComboTipoOperacion.Enabled = True
        Else
            ButtonAceptar.Text = "Modifica Ingreso"
            ComboTipoOperacion.Enabled = False
            ComboTipoOperacion.Enabled = False
        End If

        If PLote = 0 Then
            If ComboTipoOperacion.SelectedValue = 4 Then
                ComboTipoOperacion.Enabled = False
            Else : ComboTipoOperacion.Enabled = True
            End If
        End If

        If PLote = 0 And Lista <> 0 Then
            DateTime1.Enabled = False : ComboSucursal.Enabled = False
        End If
        If PLote <> 0 Then
            DateTime1.Enabled = True : ComboSucursal.Enabled = True
        End If

        If RemitoImportacion <> 0 Then
            MaskedRemito.Enabled = False
        Else : MaskedRemito.Enabled = True
        End If

        UltimaFechaW = UltimaFecha(ConexionLote)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf Dtgrid_NewRow)
        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtIngreso

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Remito")
        AddHandler Enlace.Format, AddressOf FormatRemito
        MaskedRemito.DataBindings.Clear()
        MaskedRemito.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Guia")
        AddHandler Enlace.Format, AddressOf FormatGuia
        AddHandler Enlace.Parse, AddressOf ParseGuia
        TextGuia.DataBindings.Clear()
        TextGuia.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "OrdenCompra")
        AddHandler Enlace.Format, AddressOf FormatGuia
        AddHandler Enlace.Parse, AddressOf ParseGuia
        TextOrdenCompra.DataBindings.Clear()
        TextOrdenCompra.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Sucursal")
        ComboSucursal.DataBindings.Clear()
        ComboSucursal.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Provincia")
        ComboProvincia.DataBindings.Clear()
        ComboProvincia.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Deposito")
        ComboDeposito.DataBindings.Clear()
        ComboDeposito.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "TipoOperacion")
        ComboTipoOperacion.DataBindings.Clear()
        ComboTipoOperacion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Costeo")
        ComboCosteo.DataBindings.Clear()
        ComboCosteo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Moneda")
        ComboMoneda.DataBindings.Clear()
        ComboMoneda.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Hora")
        AddHandler Enlace.Format, AddressOf FormatHora
        TextHora.DataBindings.Clear()
        TextHora.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Minuto")
        AddHandler Enlace.Format, AddressOf FormatHora
        TextMinuto.DataBindings.Clear()
        TextMinuto.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "RemitoCliente")
        AddHandler Enlace.Format, AddressOf FormatRemitoCliente
        TextRemitoCliente.DataBindings.Clear()
        TextRemitoCliente.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Format, AddressOf FormatTexto
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatRemito(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "000000000000")

    End Sub
    Private Sub FormatGuia(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(0, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub FormatRemitoCliente(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = ""
        Else : Numero.Value = NumeroEditado(Numero.Value)
        End If

    End Sub
    Private Sub FormatTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Sub FormatHora(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "00")

    End Sub
    Private Sub ParseGuia(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PLote = 0
        EmisorAnt = 0
        NegocioAnt = 0
        DepositoAnt = 0
        UnIngresoMercaderia_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonNuevaIgualProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevaIgualProveedor.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PLote = 0
        If ComboCosteo.SelectedValue = 0 Then
            EmisorAnt = Emisor
        Else
            NegocioAnt = Emisor
        End If
        DepositoAnt = ComboDeposito.SelectedValue
        UnIngresoMercaderia_Load(Nothing, Nothing)

    End Sub
    Private Sub HacerAlta(ByVal DtIngresoAux As DataTable, ByVal DtLotesAux As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtOrdenDeCompraDetalleW As DataTable)

        Dim NumeroLote As Integer
        Dim Numerow As Double
        Dim NumeroAsiento As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Lote.
            NumeroLote = UltimaNumeracion(Conexion)
            If NumeroLote < 0 Then
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            '
            DtIngresoAux.Rows(0).Item("Lote") = NumeroLote
            For Each Row1 As DataRow In DtLotesAux.Rows
                Row1("Lote") = NumeroLote
                Row1("LoteOrigen") = NumeroLote
            Next
            '
            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionLote)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = NumeroLote
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If
            '
            Numerow = ActualizaIngreso("A", DtIngresoAux, DtLotesAux, DtAsientoCabeza, DtAsientoDetalle, DtOrdenDeCompraDetalleW)
            '
            If Numerow >= 0 Or Numerow = -2 Then Exit For
            If Numerow = -1 And i = 50 Then Exit For
        Next

        If Numerow = -1 Or Numerow = -2 Then
            MsgBox("ERROR, base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Numerow = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Numerow > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PLote = NumeroLote
        End If

        ArmaArchivos()

    End Sub
    Private Sub HacerModificacion(ByVal DtIngresoAux As DataTable, ByVal DtLotesAux As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtOrdenCompraDetalleW As DataTable)

        Dim NumeroW As Double
        Dim NumeroAsiento As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionLote)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = DtIngreso.Rows(0).Item("Lote")
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If
            '
            NumeroW = ActualizaIngreso("M", DtIngresoAux, DtLotesAux, DtAsientoCabeza, DtAsientoDetalle, DtOrdenCompraDetalleW)
            '
            If NumeroW >= 0 Or NumeroW = -2 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -1 Or NumeroW = -2 Then
            MsgBox("ERROR, Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            GModificacionOk = True
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            ArmaArchivos()
        End If

    End Sub
    Private Function ActualizaIngreso(ByVal Funcion As String, ByVal DtIngresoW As DataTable, ByVal DtLotesW As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtOrdenDeCompraDetalleW As DataTable) As Double

        Dim Numero As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Funcion = "A" Then
                    If Not ReGrabaUltimoNumeroLote(DtIngresoW.Rows(0).Item("Lote")) Then Return -1
                End If
                '
                If Not IsNothing(DtIngresoW.GetChanges) Then
                    Numero = GrabaTabla(DtIngresoW.GetChanges, "IngresoMercaderiasCabeza", ConexionLote)
                    If Numero <= 0 Then Return Numero
                End If
                '
                If Not IsNothing(DtLotesW.GetChanges) Then
                    Numero = GrabaTabla(DtLotesW.GetChanges, "Lotes", ConexionLote)
                    If Numero <= 0 Then Return Numero
                End If
                '
                If Not IsNothing(DtOrdenDeCompraDetalleW.GetChanges) Then
                    Numero = GrabaTabla(DtOrdenDeCompraDetalleW.GetChanges, "OrdenCompraDetalle", Conexion)
                    If Numero <= 0 Then Return Numero
                End If
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Numero = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionLote)
                    If Numero <= 0 Then Return Numero
                End If
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Numero = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionLote)
                    If Numero <= 0 Then Return Numero
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return -2
        Finally
        End Try

    End Function
    Private Function ActualizaArchivos(ByRef DtLotesAux As DataTable) As Boolean

        Dim RowsBusqueda() As DataRow
        Dim Row As DataRow
        Dim Secuencia As Integer

        Dim PorUnidadEnLista As Boolean
        Dim FinalEnLista As Boolean
        Dim Lista As Integer

        For Each Row1 As DataRow In DtLotesAux.Rows
            If Row1("Secuencia") > Secuencia Then Secuencia = Row1("Secuencia")
            If Row1("Fecha") <> DateTime1.Value Then Row1("Fecha") = DateTime1.Value
        Next

        For Each Row1 As DataRow In DtGrid.Rows
            RowsBusqueda = DtLotesAux.Select("Lote = " & Row1("Lote") & " AND Secuencia = " & Row1("Secuencia"))
            If RowsBusqueda.Length <> 0 Then
                If Row1("Calibre") <> RowsBusqueda(0).Item("Calibre") Then RowsBusqueda(0).Item("Calibre") = Row1("Calibre")
                If Row1("PermisoImp") <> RowsBusqueda(0).Item("PermisoImp") Then RowsBusqueda(0).Item("PermisoImp") = Row1("PermisoImp")
                If Row1("Senia") <> RowsBusqueda(0).Item("Senia") Then
                    If TieneLiquidacion(Row1("Lote"), Row1("Secuencia"), RowsBusqueda(0).Item("Liquidado")) Then Return False
                    RowsBusqueda(0).Item("Senia") = Row1("Senia")
                End If
                If Row1("Cantidad") <> RowsBusqueda(0).Item("Cantidad") Then
                    If TieneLiquidacion(Row1("Lote"), Row1("Secuencia"), RowsBusqueda(0).Item("Liquidado")) Then Return False
                    Dim Gasto As Decimal = RowsBusqueda(0).Item("Cantidad") - RowsBusqueda(0).Item("Stock")
                    RowsBusqueda(0).Item("Cantidad") = Row1("Cantidad")
                    RowsBusqueda(0).Item("Stock") = Row1("Cantidad") - Gasto
                End If
            Else
                Row = DtLotesAux.NewRow
                Secuencia = Secuencia + 1
                Row("Lote") = PLote
                Row("Secuencia") = Secuencia
                Row("Deposito") = ComboDeposito.SelectedValue
                Row("Articulo") = Row1("Articulo")
                Row("Calibre") = Row1("Calibre")
                Row("Cantidad") = Row1("Cantidad")
                Row("Senia") = Row1("Senia")
                Row("Baja") = 0
                Row("Fecha") = DateTime1.Value
                Row("Traslado") = 0
                Row("BajaReproceso") = 0
                Row("Stock") = Row1("Cantidad")
                Row("LoteOrigen") = PLote
                Row("SecuenciaOrigen") = Secuencia
                Row("DepositoOrigen") = ComboDeposito.SelectedValue
                Row("ClientePotencial") = 0
                Row("Descarte") = 0
                Row("Merma") = 0
                Row("Directo") = -1
                Row("Proveedor") = Emisor
                Row("KilosXUnidad") = HallaKilosXUnidad(Row1("Articulo"))
                Row("TipoOperacion") = ComboTipoOperacion.SelectedValue
                Row("Liquidado") = 0
                Row("PrecioF") = 0
                Row("PrecioPorLista") = False
                Row("PrecioCompra") = 0
                Row("PermisoImp") = Row1("PermisoImp")
                Row("MermaTr") = -100
                Row("DiferenciaInventario") = 0
                DtLotesAux.Rows.Add(Row)
            End If
        Next

        For Each Row In DtLotesAux.Rows
            If Row.RowState <> DataRowState.Added Then
                RowsBusqueda = DtGrid.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
                If RowsBusqueda.Length = 0 Then
                    If TieneLiquidacion(Row("Lote"), Row("Secuencia"), Row("Liquidado")) Then Return False
                    Row.Delete()
                End If
            End If
        Next

        Return True

    End Function
    Private Function ActualizaOrdenCompra(ByRef DtOrdenCompra As DataTable, ByVal DtGrid As DataTable) As Boolean

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid.Rows
            If Not Row.RowState = DataRowState.Deleted Then
                RowsBusqueda = DtOrdenCompra.Select("Articulo = " & Row("Articulo"))
                RowsBusqueda(0).Item("Recibido") = RowsBusqueda(0).Item("Recibido") + (Row("Cantidad") - Row("CantidadOriginal"))
            End If
        Next

        Return True

    End Function
    Private Function HallaPrecio(ByVal Articulo As Integer) As Decimal

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtOrdenCompraDetalle.Select("Articulo = " & Articulo)
        Return Trunca(RowsBusqueda(0).Item("TotalArticulo") / RowsBusqueda(0).Item("Cantidad"))


    End Function
    Private Function TieneLiquidacion(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Liquidado As Double) As Boolean

        If Liquidado <> 0 Then
            MsgBox("Existe Liquidación o Factura  para el Lote " & Lote & "/" & Format(Secuencia, "000") & ". Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Return True
        End If

        Return False

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Medida As DataColumn = New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Medida)

        Dim Lote As DataColumn = New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As DataColumn = New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Calibre As New DataColumn("Calibre")
        Calibre.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Calibre)

        Dim CantidadOriginal As New DataColumn("CantidadOriginal")
        CantidadOriginal.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(CantidadOriginal)

        Dim SaldoOrdenCompra As New DataColumn("SaldoOrdenCompra")
        SaldoOrdenCompra.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(SaldoOrdenCompra)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cantidad)

        Dim Senia As New DataColumn("Senia")
        Senia.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Senia)

        Dim PermisoImp As New DataColumn("PermisoImp")
        PermisoImp.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(PermisoImp)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Stock)

        Dim AGranel As New DataColumn("AGranel")
        AGranel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(AGranel)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        LlenaComboTablas(ComboProvincia, 31)
        ComboProvincia.SelectedValue = 0
        With ComboProvincia
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboDeposito, 19)

        ComboTipoOperacion.DataSource = Nothing
        ComboTipoOperacion.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Miselaneas WHERE Codigo = 1;")
        Row = ComboTipoOperacion.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboTipoOperacion.DataSource.rows.add(Row)
        ComboTipoOperacion.DisplayMember = "Nombre"
        ComboTipoOperacion.ValueMember = "Clave"
        ComboTipoOperacion.SelectedValue = 0
        With ComboTipoOperacion
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboMoneda.DataSource = Nothing
        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Row = ComboMoneda.DataSource.newrow
        Row("Nombre") = "PESOS"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"

        Calibre.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 5;")
        Row = Calibre.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Calibre.DataSource.Rows.Add(Row)
        Calibre.DisplayMember = "Nombre"
        Calibre.ValueMember = "Clave"

    End Sub
    Private Sub ArmaArticulosParaAltaParaGrid()

        Dim Row As DataRow
        Dim Dt As New DataTable
        Dim Sql As String = ""

        Dt = ArticulosActivos()

        If Lista <> 0 Then
            Sql = "SELECT Articulo FROM ListaDePreciosProveedoresDetalle WHERE Lista = " & Lista & ";"
        End If
        If OrdenCompra <> 0 Then
            Sql = "SELECT Articulo FROM OrdenCompraDetalle WHERE Orden = " & OrdenCompra & ";"
        End If

        If Sql <> "" Then
            Dim Dtw As New DataTable
            Dtw = Tablas.Leer(Sql)
            For I As Integer = Dt.Rows.Count - 1 To 0 Step -1
                Row = Dt.Rows(I)
                Dim RowsBusqueda() As DataRow
                RowsBusqueda = Dtw.Select("Articulo = " & Row("Clave"))
                If RowsBusqueda.Length = 0 Then Row.Delete()
            Next
            Dtw.Dispose()
        End If

        Articulo.DataSource = Dt
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Sub ArmaArticulosParaModificacionParaGrid()

        Dim Row As DataRow
        Dim Dt As New DataTable
        Dim RowsBusqueda() As DataRow
        Dim Sql As String = ""

        Dt = TodosLosArticulos()

        If Lista <> 0 And Lista <> -1 Then
            Sql = "SELECT Articulo FROM ListaDePreciosProveedoresDetalle WHERE Lista = " & Lista & ";"
        End If
        If OrdenCompra <> 0 Then
            Sql = "SELECT Articulo FROM OrdenCompraDetalle WHERE Orden = " & OrdenCompra & ";"
        End If

        If Sql <> "" Then
            Dim Dtw As New DataTable
            Dtw = Tablas.Leer(Sql)
            For I As Integer = Dt.Rows.Count - 1 To 0 Step -1
                Row = Dt.Rows(I)
                RowsBusqueda = Dtw.Select("Articulo = " & Row("Clave"))
                If RowsBusqueda.Length = 0 Then
                    RowsBusqueda = DtLotes.Select("Articulo = " & Row("Clave"))
                    If RowsBusqueda.Length = 0 Then Row.Delete()
                End If
            Next
            Dtw.Dispose()
        End If

        Articulo.DataSource = Dt
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Sub LlenaDatosEmisor(ByVal Proveedor As Integer)

        Dim Dta As New DataTable
        Dim Sql As String = ""

        Sql = "SELECT TipoOperacion,Provincia,Nombre,Moneda,Opr FROM Proveedores WHERE Clave = " & Proveedor & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, Proveedor ya no existe o error en la Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
            Exit Sub
        End If

        If PLote = 0 Then
            ComboTipoOperacion.SelectedValue = Dta.Rows(0).Item("TipoOperacion")
            ComboProvincia.SelectedValue = Dta.Rows(0).Item("Provincia")
        End If
        TextEmisor.Text = Dta.Rows(0).Item("Nombre")
        MonedaProveedor = Dta.Rows(0).Item("Moneda")

        If Not Dta.Rows(0).Item("Opr") And PAbierto And PLote = 0 Then
            If MsgBox("Cliente Solo Opera a Candado Cerrado. Debe Cambiar Candado en Cliente. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Me.Close() : Exit Sub
        End If

        Dta.Dispose()

    End Sub
    Private Function Opcion() As Boolean

        OpcionIngreso.PSoloFrutas = True
        If PLote = 0 Then OpcionIngreso.PEsSoloAltas = True
        OpcionIngreso.PEmisorAnt = EmisorAnt
        OpcionIngreso.PNegocioAnt = NegocioAnt
        OpcionIngreso.PDepositoAnt = DepositoAnt
        OpcionIngreso.PConOrdenCompra = True
        OpcionIngreso.ShowDialog()
        If OpcionIngreso.PRegresar Then
            OpcionIngreso.Dispose() : Return False
        End If
        If OpcionIngreso.PEmisor <> 0 Then
            Emisor = OpcionIngreso.PEmisor
            TextEmisor.Text = OpcionIngreso.PNombreEmisor
        End If
        ComboDeposito.SelectedValue = OpcionIngreso.ComboDeposito.SelectedValue
        PAbierto = OpcionIngreso.PAbierto
        RemitoImportacion = OpcionIngreso.PRemitoImportacion
        DtImportacion = OpcionIngreso.PDtImportacion
        Sucursal = OpcionIngreso.PSucursal
        FechaRemito = OpcionIngreso.PFechaRemito
        Lista = OpcionIngreso.Plista
        OrdenCompra = OpcionIngreso.POrdenCompra
        OpcionIngreso.Dispose()

        '------------------------------------------------------------------------------------
        If OrdenCompra <> 0 Then Lista = 0 'Siempre toma precios de articulos de la orden de compra.  --
        '------------------------------------------------------------------------------------

        Return True

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT UltimoLote FROM NumeracionDocumentos WHERE Clave = 1;", Miconexion)
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
    Private Function RemitoExiste(ByVal Remito As Double) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionLote)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Lote) FROM IngresoMercaderiasCabeza WHERE Proveedor = " & Emisor & " AND Remito = " & Remito & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then
                        Return True
                    Else : Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error de Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        Finally
        End Try

    End Function
    Private Function GuiaExiste(ByVal Guia As Double) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionLote)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Lote) FROM IngresoMercaderiasCabeza WHERE Proveedor = " & Emisor & " AND Guia = " & Guia & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then
                        Return True
                    Else : Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error de Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        Finally
        End Try

    End Function
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtLotesAux As DataTable, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)

        Dim RowsBusqueda() As DataRow
        Dim ImporteTotal As Double = 0
        Dim PrecioBaja As Double = 0
        Dim Precio As Double = 0
        Dim Centro As Integer = 0

        If DtIngreso.Rows(0).Item("TipoOperacion") <> 4 Then
            If Not HallaPrecioCentroTipoOperacion(DtIngreso.Rows(0).Item("TipoOperacion"), Centro, Precio) Then Return False
        End If

        If Funcion = "A" Then
            For Each Row As DataRow In DtLotesAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    If DtIngreso.Rows(0).Item("TipoOperacion") = 4 Then
                        'Halla Precio si lote es de Negocio.
                        Centro = 0
                        If Not HallaPrecioYCentroCosteo(Emisor, DtIngreso.Rows(0).Item("Costeo"), Centro, Precio) Then Return False
                    End If
                    Dim Fila As New ItemLotesParaAsientos
                    Fila.Centro = Centro
                    RowsBusqueda = DtLotes.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
                    If RowsBusqueda.Length <> 0 Then
                        Fila.MontoNeto = Trunca(Precio * Row("KilosXUnidad") * (Row("Cantidad") - RowsBusqueda(0).Item("Cantidad")))
                    Else : Fila.MontoNeto = Trunca(Precio * Row("KilosXUnidad") * Row("Cantidad"))
                    End If
                    ImporteTotal = ImporteTotal + Fila.MontoNeto
                    If DtIngreso.Rows(0).Item("TipoOperacion") = 1 Then Fila.Clave = 301
                    If DtIngreso.Rows(0).Item("TipoOperacion") = 2 Then Fila.Clave = 300
                    If DtIngreso.Rows(0).Item("TipoOperacion") = 3 Then Fila.Clave = 303
                    If DtIngreso.Rows(0).Item("TipoOperacion") = 4 Then Fila.Clave = 302
                    If Fila.MontoNeto <> 0 Then ListaLotesParaAsiento.Add(Fila)
                End If
            Next
            For Each Row As DataRow In DtLotes.Rows
                RowsBusqueda = DtLotesAux.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
                If RowsBusqueda.Length = 0 Then
                    If DtIngreso.Rows(0).Item("TipoOperacion") = 4 Then
                        'Halla Precio si Lote es de Negocio.
                        Centro = 0
                        If Not HallaPrecioYCentroCosteo(Emisor, DtIngreso.Rows(0).Item("Costeo"), Centro, Precio) Then Return False
                    End If
                    Dim Fila As New ItemLotesParaAsientos
                    Fila.Centro = Centro
                    Fila.MontoNeto = Trunca(-Precio * Row("KilosXUnidad") * Row("Cantidad"))
                    ImporteTotal = ImporteTotal + Fila.MontoNeto
                    If DtIngreso.Rows(0).Item("TipoOperacion") = 1 Then Fila.Clave = 301
                    If DtIngreso.Rows(0).Item("TipoOperacion") = 2 Then Fila.Clave = 300
                    If DtIngreso.Rows(0).Item("TipoOperacion") = 3 Then Fila.Clave = 303
                    If DtIngreso.Rows(0).Item("TipoOperacion") = 4 Then Fila.Clave = 302
                    ListaLotesParaAsiento.Add(Fila)
                End If
            Next
        End If

        If Funcion = "B" Then
            For Each Row As DataRow In DtLotesAux.Rows
                If DtIngreso.Rows(0).Item("TipoOperacion") = 4 Then
                    Centro = 0
                    If Not HallaPrecioYCentroCosteo(Emisor, DtIngreso.Rows(0).Item("Costeo"), Centro, Precio) Then Return False
                End If
                Dim Fila As New ItemLotesParaAsientos
                Fila.Centro = Centro
                Fila.MontoNeto = Trunca(-Precio * Row("KilosXUnidad") * Row("Cantidad"))
                ImporteTotal = ImporteTotal + Fila.MontoNeto
                If DtIngreso.Rows(0).Item("TipoOperacion") = 1 Then Fila.Clave = 301
                If DtIngreso.Rows(0).Item("TipoOperacion") = 2 Then Fila.Clave = 300
                If DtIngreso.Rows(0).Item("TipoOperacion") = 3 Then Fila.Clave = 303
                If DtIngreso.Rows(0).Item("TipoOperacion") = 4 Then Fila.Clave = 302
                ListaLotesParaAsiento.Add(Fila)
            Next
        End If

        If ListaLotesParaAsiento.Count <> 0 Then
            If Not Asiento(6050, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False
        End If

        Return True

    End Function
    Private Function HallaPrecioCentroTipoOperacion(ByVal TipoOperacion As Integer, ByRef Centro As Integer, ByRef Precio As Double) As Boolean

        Dim Dt As New DataTable

        Precio = Refe

        If Not Tablas.Read("SELECT Centro FROM Miselaneas WHERE Codigo = 1 AND Clave = " & TipoOperacion & ";", Conexion, Dt) Then Return False
        Centro = Dt.Rows(0).Item("Centro")

        Dt.Dispose()

        Return True

    End Function
    Private Sub VerSucursales(ByVal Emisor As Integer)

        Dim Sql As String

        Sql = "SELECT Clave,Nombre FROM SucursalesProveedores WHERE Estado = 1 AND Proveedor = " & Emisor & ";"
        ComboSucursal.DataSource = New DataTable
        ComboSucursal.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboSucursal.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboSucursal.DataSource.rows.add(Row)
        ComboSucursal.DisplayMember = "Nombre"
        ComboSucursal.ValueMember = "Clave"
        ComboSucursal.SelectedValue = 0
        With ComboSucursal
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Private Sub Print_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim Contador As Integer = 0
        Dim UltimaLinea As Integer = 0
        Dim LineasImpresas As Integer = 0
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        Try
            Print_Titulo(e, MIzq, MTop)

            If LineasParaImpresion < DtGrid.Rows.Count Then
                Print_Cuerpo(e, MIzq, MTop, UltimaLinea, LineasImpresas)
            End If

            Print_Final(e, MIzq, MTop)

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
    Private Sub Print_Titulo(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

        Dim Texto As String = ""

        'Encabezado.
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
        If PAbierto Then
            Texto = GNombreEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
            Texto = "INGRESO LOTES"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 140, MTop)
        End If
        ' 
        PrintFont = New Font("Courier New", 12)
        Texto = "Fecha :  " & DateTime1.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 10)
        Texto = "Remito:  " & NumeroEditado(MaskedRemito.Text) & "     Guia: " & TextGuia.Text & "   O.de Compra: " & TextOrdenCompra.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
        Texto = "Proveedor: " & TextEmisor.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 27)

    End Sub
    Private Sub Print_Cuerpo(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer, ByRef UltimaLinea As Integer, ByRef LineasImpresas As Integer)

        Dim Texto As String = ""
        Dim SaltoLinea As Integer = 4
        Dim Longi As Integer
        Dim Xq As Integer
        Dim Yq As Integer
        Dim x As Integer = MIzq
        Dim y As Integer = MTop + 50
        Dim Ancho As Integer = 180
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 37

        'Grafica -Rectangulo medios de pagos. ----------------------------------------------------------------------

        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 10)
        Dim LineaLote As Integer = x + 25
        Dim LineaArticulo As Integer = x + 113
        Dim LineaCantidad As Integer = x + 135
        Dim LineaCalibre As Integer = x + 160
        Dim LineaSenia As Integer = x + Ancho
        'Titulos de descripcion.
        Texto = "DESCRIPCION"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y - 4)
        Texto = "Lote"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaLote - Longi - 5
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "ARTICULO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaArticulo - Longi - 33
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "CANTIDAD"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaCantidad - Longi - 2
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "CALIBRE"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaCalibre - Longi - 4
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "SEÑA"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaSenia - Longi - 5
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        'Descripcion del pago.
        Yq = y + SaltoLinea
        While Contador < LineasPorPagina And LineasParaImpresion < Grid.Rows.Count
            Dim Row As DataGridViewRow = Grid.Rows(LineasParaImpresion)
            If IsNothing(Row.Cells("Articulo").Value) Then Exit While
            Yq = Yq + SaltoLinea
            Texto = Row.Cells("LoteYSecuencia").FormattedValue
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaLote - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Texto = Strings.Left(Row.Cells("Articulo").FormattedValue, 39)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaLote + 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Texto = Row.Cells("Cantidad").FormattedValue & Row.Cells("Medida").Value
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Texto = Row.Cells("Calibre").FormattedValue
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad + 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Texto = Row.Cells("Senia").FormattedValue
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaSenia - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            'Imprime Comprobante.
            Contador = Contador + 1
            LineasParaImpresion = LineasParaImpresion + 1
        End While

        Yq = Yq + SaltoLinea

        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Yq - y)
        'Lineas vertical.
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaLote, y, LineaLote, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaArticulo, y, LineaArticulo, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCantidad, y, LineaCantidad, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCalibre, y, LineaCalibre, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaSenia, y, LineaSenia, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y + 2 * SaltoLinea, x + Ancho, y + 2 * SaltoLinea)

        UltimaLinea = Yq
        LineasImpresas = Contador

    End Sub
    Private Sub ImprimeEtiqueta(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim Texto As String
        Dim PrintFont As System.Drawing.Font
        Dim SaltoLinea As Integer = 4

        Texto = PLote
        PrintFont = New Font("Christian Robertson", 210)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, e.PageSettings.Bounds.Width / 2 - e.Graphics.MeasureString(Texto, PrintFont).Width / 2, MTop + 170)

        Texto = Format(DateTime1.Value, "dd/MM/yyyy")
        PrintFont = New Font("Arial", 32)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 750, MTop + 650)

        Texto = TextEmisor.Text
        PrintFont = New Font("Arial", 32)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 100, MTop + 650)

    End Sub
    Private Sub Print_Final(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

    End Sub
    Private Function HallaAsignacionLote(ByVal Lote As Integer) As Integer


        Dim Miconexion As New OleDb.OleDbConnection(ConexionLote)

        Try
            Miconexion.Open()
            Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Comprobante) FROM AsignacionLotes WHERE Lote = " & Lote & ";", Miconexion)
                Return Cmd.ExecuteScalar()
            End Using
        Catch ex As Exception
            End
        Finally
            If Miconexion.State = ConnectionState.Open Then Miconexion.Close()
        End Try

    End Function
    Private Function HallaLoteLiquidado(ByVal Lote As Integer) As Integer


        Dim Miconexion As New OleDb.OleDbConnection(ConexionLote)

        Try
            Miconexion.Open()
            Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Lote) FROM Lotes WHERE Liquidado <> 0 AND Lote = " & Lote & ";", Miconexion)
                Return Cmd.ExecuteScalar()
            End Using
        Catch ex As Exception
            End
        Finally
            If Miconexion.State = ConnectionState.Open Then Miconexion.Close()
        End Try


    End Function
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM IngresoMercaderiasCabeza;", Miconexion)
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
    Private Function HallaListaPrecios() As Integer

        Lista = 0
        Zona = 0
        Dim PPorUnidadEnLista = False, PFinalEnLista = False

        Return HallaListaPrecioProveedor(DtIngreso.Rows(0).Item("Proveedor"), ComboSucursal.SelectedValue, DateTime1.Value, PPorUnidadEnLista, PFinalEnLista, Zona)

    End Function
    Private Function CantidadOrdenCompraOK(ByVal Articulo As Integer, ByVal Cantidad As Decimal) As Boolean

        Dim RowsBusqueda() As DataRow

        If Not IsNothing(DtOrdenCompraDetalle) Then
            RowsBusqueda = DtOrdenCompraDetalle.Select("Articulo = " & Articulo)
        End If

        If Cantidad <= RowsBusqueda(0).Item("Cantidad") - RowsBusqueda(0).Item("Reibido") Then Return True

        Return False

    End Function
    Private Function ExisteArticuloEnGrid(ByVal Articulo As Integer) As Boolean

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGrid.Select("Articulo = " & Articulo)
        If RowsBusqueda.Length > 0 Then Return True

        Return False

    End Function
    Private Function HallaDatosOrdenCompra(ByVal DtOrdenCompraDetalle As DataTable, ByVal Articulo As Integer) As Decimal

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtOrdenCompraDetalle.Select("Articulo = " & Articulo)
        Return RowsBusqueda(0).Item("Cantidad") - RowsBusqueda(0).Item("Recibido")

    End Function
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTime1.Value, Date.Now) < 0 Then
            MsgBox("Fecha Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If

        If ComboTipoOperacion.SelectedValue <> 2 And OrdenCompra <> 0 Then
            MsgBox("Tipo Operacion debe ser REVENTA con Orden de Compra.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboTipoOperacion.Focus()
            Return False
        End If

        If Val(MaskedRemito.Text) <> 0 And Not MaskedOK(MaskedRemito) Then
            MsgBox("Numero Remito Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedRemito.Focus()
            Return False
        End If
        If Val(MaskedRemito.Text) = 0 And Val(TextGuia.Text) = 0 Then
            MsgBox("Falta Informar Remito o Guia.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedRemito.Focus()
            Return False
        End If
        If Val(MaskedRemito.Text) <> RemitoAnt And Val(MaskedRemito.Text) <> 0 Then
            If RemitoExiste(Val(MaskedRemito.Text)) Then
                MsgBox("Remito ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedRemito.Focus()
                Return False
            End If
        End If
        If Val(MaskedRemito.Text) <> 0 And TextGuia.Text <> "" Then
            MsgBox("Debe Informar Remito o Guia.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedRemito.Focus()
            Return False
        End If

        If DtIngreso.Rows(0).Item("Guia") <> GuiaAnt And DtIngreso.Rows(0).Item("Guia") <> 0 Then
            If GuiaExiste(DtIngreso.Rows(0).Item("Guia")) Then
                MsgBox("Guia ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextGuia.Focus()
                Return False
            End If
        End If

        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Return False
        End If

        If ComboSucursal.Items.Count > 1 And ComboSucursal.SelectedValue = 0 Then
            MsgBox("Debe Informar Sucursal.", MsgBoxStyle.Critical)
            ComboSucursal.Focus()
            Return False
        End If

        If ComboProvincia.SelectedValue = 0 Then
            MsgBox("Falta Provincia.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Return False
        End If
        If Grid.Rows.Count = 1 Then
            MsgBox("Falta Informar Articulos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If
        If Not PEsCosteo And ComboTipoOperacion.SelectedValue = 4 Then
            MsgBox("Tipo Operacion COSTEO no es valido para Este Proveedor.")
            Return False
        End If

        If ComboTipoOperacion.SelectedValue = 0 Then
            MsgBox("Falta Tipo de Operacion.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If ComboTipoOperacion.SelectedValue = 4 And ComboCosteo.SelectedValue = 0 Then
            MsgBox("Falta Definir Costeo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboCosteo.Focus()
            Return False
        End If

        If ComboTipoOperacion.SelectedValue <> 4 And ComboCosteo.SelectedValue <> 0 Then
            MsgBox("Tipo Operacion no Valido para un Costeo Definido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboCosteo.Focus()
            Return False
        End If

        Dim i As Integer
        For i = 0 To Grid.RowCount - 2
            If Grid.Rows(i).Cells("Articulo").Value = 0 Then
                MsgBox("Debe Informar Articulo en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Articulo")
                Grid.BeginEdit(True)
                Return False
            End If
            If IsDBNull(Grid.Rows(i).Cells("Cantidad").Value) Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Cantidad")
                Grid.BeginEdit(True)
                Return False
            End If
            If Grid.Rows(i).Cells("Cantidad").Value = 0 Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Cantidad")
                Grid.BeginEdit(True)
                Return False
            End If
            If TieneDecimales(Grid.Rows(i).Cells("Cantidad").Value) And Not Grid.Rows(i).Cells("AGranel").Value Then
                MsgBox("Cantidad no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Cantidad")
                Grid.BeginEdit(True)
                Return False
            End If
            If Grid.Rows(i).Cells("PermisoImp").Value <> "" Then
                If Not PermisoImportacion.Valida(Grid.Rows(i).Cells("PermisoImp").Value) Then
                    Grid.CurrentCell = Grid.Rows(i).Cells("PermisoImp")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
            If Grid.Rows(i).Cells("Senia").Value > SeniaMaxima Then
                MsgBox("Seña Supera Máximo permitido en Linea " & i + 1 & ". Modificar 'Valor Seña Maxima' en Menu->Tablas->Datos de la Empresa.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Senia")
                Grid.BeginEdit(True)
                Return False
            End If
        Next

        If ComboTipoOperacion.SelectedValue = 4 Then
            Dim ListaDeArticulos As New List(Of ItemListaDePrecios)
            For Each Row As DataRow In DtGrid.Rows
                Dim Fila As New ItemListaDePrecios
                Fila.Articulo = Row("Articulo")
                ListaDeArticulos.Add(Fila)
            Next
            If Not ValidaCosteo(Emisor, ComboCosteo.SelectedValue, ListaDeArticulos, DateTime1.Value) Then Return False
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
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        If Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly Then Exit Sub 'Para que no cancele cuando hay rows protegidas.

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Calibre" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
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
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PermisoImp" Then
            e.KeyChar = Char.ToUpper(e.KeyChar)
            If e.KeyChar = " " Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Senia" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Senia" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 4)
            End If
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        If Grid.Columns(e.ColumnIndex).Name = "Senia" Then
            If IsDBNull(Grid.CurrentCell.Value) Then Grid.CurrentCell.Value = 0
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) And _
               Not IsNothing(Grid.Rows(e.RowIndex).Cells("Lote").Value) Then
                If Grid.Rows(e.RowIndex).Cells("Lote").Value <> 0 Then
                    e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Cantidad").Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Senia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Senia").Value) Then
                If e.Value = -1 Then
                    e.Value = ""
                Else : e.Value = FormatNumber(e.Value, 4)
                End If
            End If
        End If

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then Exit Sub

        If Grid.CurrentRow.Cells("Lote").Value <> 0 Then
            MsgBox("Funcion No Permitida.", MsgBoxStyle.Exclamation)
            Exit Sub
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Lote").Value = 0 Then
            Grid.Rows.Remove(Grid.CurrentRow)
            Exit Sub
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Dtgrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row
        Row("Lote") = 0
        Row("Secuencia") = 0
        Row("Articulo") = 0
        Row("Calibre") = 0
        Row("Cantidad") = 0
        Row("CantidadOriginal") = 0
        Row("Stock") = 0
        Row("Senia") = 0
        Row("PermisoImp") = ""
        Row("AGranel") = False
        Row("Medida") = ""
        Row("SaldoOrdenCompra") = 0

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Articulo") Then
            If e.Row("Lote") <> 0 Then
                e.ProposedValue = e.Row("Articulo")
            Else
                If e.ProposedValue <> 0 Then
                    e.Row("Senia") = HallaSeniaArticulo(e.ProposedValue, DateTime1.Value)
                    HallaAGranelYMedida(e.ProposedValue, e.Row("AGranel"), e.Row("Medida"))
                    If ExisteArticuloEnGrid(e.ProposedValue) And OrdenCompra > 0 Then
                        MsgBox("Articulo YA EXiste.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                        e.ProposedValue = 0
                        Exit Sub
                    End If
                    If OrdenCompra <> 0 Then
                        e.Row("SaldoOrdenCompra") = HallaDatosOrdenCompra(DtOrdenCompraDetalle, e.ProposedValue)
                    End If
                End If
            End If
        End If

        If e.Column.ColumnName.Equals("Cantidad") Then
            If IsDBNull(e.Row("Cantidad")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If e.Row("Articulo") = 0 And OrdenCompra <> 0 Then
                MsgBox("Debe Ingresar Articulo.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = 0
                Exit Sub
            End If
            If e.ProposedValue = 0 Then
                MsgBox("Ingreso debe ser distino de cero.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Cantidad")
                Exit Sub
            End If
            e.ProposedValue = Trunca(e.ProposedValue)
            If PLote <> 0 Then
                Dim Gasto As Double = e.Row("CantidadOriginal") - e.Row("Stock")
                If e.ProposedValue < Gasto Then
                    MsgBox("No se puede Modificar, Nuevo Ingreso menor a Unidades ya Vendidas o Remitidas.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                    e.ProposedValue = e.Row("Cantidad")
                    Exit Sub
                End If
            End If
            If OrdenCompra <> 0 Then
                Dim Gasto As Decimal = e.ProposedValue - e.Row("CantidadOriginal")
                If Gasto > 0 Then
                    If e.Row("SaldoOrdenCompra") < Gasto Then
                        MsgBox("Cantidad mayor al Saldo de la Orden de Compra.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                        e.ProposedValue = e.Row("Cantidad")
                        Exit Sub
                    End If
                End If
            End If
            Grid.Refresh()
        End If

        If e.Column.ColumnName.Equals("Senia") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca4(e.ProposedValue)
        End If

        If e.Column.ColumnName.Equals("PermisoImp") Then
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
        End If

    End Sub
    
End Class