Imports System.Transactions
Public Class UnConsumoDeInsumo
    Public PConsumo As Integer
    Public PEsReventa As Boolean
    Public PEsConsignacion As Boolean
    Public PEsNegocio As Boolean
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Dim ListaDeLotes As List(Of FilaComprobanteFactura)
    '
    Private MiEnlazador As New BindingSource
    Dim DtConsumoCabeza As DataTable
    Dim DtConsumoDetalle As DataTable
    Dim DtConsumoLotes As DataTable
    Dim DtStock As New DataTable
    '
    Dim Insumo As Integer
    Dim Deposito As Integer
    Dim ConexionConsumo As String
    Dim ImporteB As Double
    Private Sub UnConsumoDeInsumo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(12) Then PBloqueaFunciones = True

        If PConsumo = 0 Then
            Opciones()
            If Insumo = 0 Then Me.Close() : Exit Sub
        End If

        LlenaComboTablas(ComboDeposito, 20)

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        ComboInsumo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Insumos;")
        ComboInsumo.DisplayMember = "Nombre"
        ComboInsumo.ValueMember = "Clave"

        Dim Row As DataRow

        ComboNegocio.DataSource = New DataTable
        ComboNegocio.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;")
        Row = ComboNegocio.DataSource.NewRow()
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboNegocio.DataSource.Rows.Add(Row)
        ComboNegocio.DisplayMember = "Nombre"
        ComboNegocio.ValueMember = "Clave"
        ComboNegocio.SelectedValue = 0
        With ComboNegocio
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        With ComboCosteo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ConexionConsumo = Conexion

        If GGeneraAsiento Then
            Dim Conta = TieneTabla1(6000)
            If Conta < 0 Then
                MsgBox("Error Base de Datos al leer Seteo de Documento.")
                Me.Close()
                Exit Sub
            End If
            If Conta = 0 Then
                LabelCuentas.Visible = False
                ListCuentas.Visible = False
                PictureLupaCuenta.Visible = False
            End If
        End If

        ListCuentas.Clear()

        ArmaArchivos()

        GModificacionOk = False

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtConsumoLotesAux As DataTable = DtConsumoLotes.Copy

        Dim HayCambios As Boolean = False
        Dim RowsBusqueda() As DataRow
        For Each Item As FilaComprobanteFactura In ListaDeLotes
            RowsBusqueda = DtConsumoLotes.Select("Lote = " & Item.Lote & " AND Secuencia = " & Item.Secuencia)
            If RowsBusqueda.Length = 0 Then
                Dim Row As DataRow = DtConsumoLotesAux.NewRow
                Row("Consumo") = PConsumo
                Row("Lote") = Item.Lote
                Row("Secuencia") = Item.Secuencia
                Row("ImporteConIva") = 0
                Row("ImporteSinIva") = 0
                DtConsumoLotesAux.Rows.Add(Row)
                HayCambios = True
            End If
        Next
        For Each Row As DataRow In DtConsumoLotesAux.Rows
            If Not ExisteEnLista(ListaDeLotes, Row("Lote"), Row("Secuencia")) Then
                Row.Delete()
                HayCambios = True
            End If
        Next

        If HayCambios Then
            Dim Cantidad As Integer
            For Each Row As DataRow In DtConsumoLotesAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Cantidad = Cantidad + HallaCantidadEnLista(ListaDeLotes, Row("Lote"), Row("Secuencia"))
                End If
            Next
            For Each Row As DataRow In DtConsumoLotesAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("ImporteConIva") = Trunca(HallaCantidadEnLista(ListaDeLotes, Row("Lote"), Row("Secuencia")) * CDbl(TextCosto.Text) / Cantidad)
                    Row("ImporteSinIva") = Trunca(HallaCantidadEnLista(ListaDeLotes, Row("Lote"), Row("Secuencia")) * CDbl(TextCostoSinIva.Text) / Cantidad)
                End If
            Next
        End If

        If PConsumo = 0 Then
            HacerAlta(DtConsumoLotesAux)
        Else
            HacerModificacion(DtConsumoLotesAux)
        End If
    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PConsumo = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Consumo ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If MsgBox("Consumo se Anulara. ¿Desea Anularlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        DtStock = New DataTable

        DtConsumoCabeza.Rows(0).Item("Estado") = 3

        For Each Row As DataRow In DtConsumoDetalle.Rows
            If Not Tablas.Read("SELECT * FROM StockInsumos WHERE OrdenCompra = " & Row("OrdenCompra") & " AND Articulo = " & DtConsumoCabeza.Rows(0).Item("Insumo") & " AND Deposito = " & DtConsumoCabeza.Rows(0).Item("Deposito"), ConexionConsumo, DtStock) Then Me.Close() : Exit Sub
        Next

        Dim RowsBusqueda() As DataRow
        For Each Row As DataRow In DtConsumoDetalle.Rows
            RowsBusqueda = DtStock.Select("OrdenCompra = " & Row("OrdenCompra"))
            RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") + Row("Cantidad")
        Next

        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtConsumoLotesAux As DataTable = DtConsumoLotes.Copy
        '
        If Not HallaAsientosCabeza(6000, DtConsumoCabeza.Rows(0).Item("Consumo"), DtAsientoCabezaB, Conexion) Then Exit Sub
        For Each Row As DataRow In DtAsientoCabezaB.Rows
            Row("Estado") = 3
        Next

        For Each Row As DataRow In DtConsumoLotesAux.Rows
            Row.Delete()
        Next

        Dim Resul As Integer = ActualizaConsumo(DtAsientoCabezaB, DtAsientoDetalleB, DtConsumoLotesAux)

        If Resul < 0 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario Modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul > 0 Then
            MsgBox("Baja Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PConsumo = 0
        UnConsumoDeInsumo_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PConsumo = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 6000

        ListaAsientos.PDocumentoB = PConsumo
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonCalculaCosto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCalculaCosto.Click

        If TextCantidad.Text = "" Then Exit Sub
        If CDbl(TextCantidad.Text) = 0 Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Resul As Double = 0

        'Arma DtConsumoDetalle con las ordenes de pago con saldo del Insumo (de la mas viejas a las mas nuevas).
        Resul = CalculaAsignacion(Insumo, Deposito, ConexionConsumo, CDbl(TextCantidad.Text))

        If Resul < 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Error Base de datos al leer tabla de Stock.")
            TextCosto.Text = "0,00"
            TextCostoSinIva.Text = "0,00"
            Exit Sub
        End If
        If Resul <> 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No Existe Stock para el Insumo.")
            TextCosto.Text = "0,00"
            TextCostoSinIva.Text = "0,00"
            Exit Sub
        End If

        Dim CostoConIva As Double
        Dim CostoSinIva As Double
        ImporteB = 0

        'Recorre los importes de las Ordenes de Compra y calcula los costos.
        Resul = HallaCostoConsumo(DtConsumoDetalle, Insumo, Conexion, CostoConIva, CostoSinIva, ImporteB)
        If Resul = -1 Then
            MsgBox("Error Base de Datos.")
            Me.Close() : Exit Sub
        End If
        TextCosto.Text = FormatNumber(CostoConIva, GDecimales)
        TextCostoSinIva.Text = FormatNumber(CostoSinIva, GDecimales)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub PictureLupaCuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupaCuenta.Click

        If PConsumo <> 0 Then Exit Sub

        SeleccionarCuentaDocumento.PListaDeCuentas = New List(Of ItemCuentasAsientos)

        Dim Item As New ListViewItem

        For I As Integer = 0 To ListCuentas.Items.Count - 1
            Dim Fila As New ItemCuentasAsientos
            Dim CuentaStr As String = ListCuentas.Items.Item(I).SubItems(0).Text
            Fila.Cuenta = Mid(CuentaStr, 1, 3) & Mid(CuentaStr, 5, 6) & Mid(CuentaStr, 12, 2)
            Fila.ImporteB = CDbl(ListCuentas.Items.Item(I).SubItems(1).Text)
            Fila.ImporteN = 0
            SeleccionarCuentaDocumento.PListaDeCuentas.Add(Fila)
        Next

        SeleccionarCuentaDocumento.PSoloUnImporte = True
        SeleccionarCuentaDocumento.PImporteB = CDbl(TextCostoSinIva.Text)
        SeleccionarCuentaDocumento.ShowDialog()
        If SeleccionarCuentaDocumento.PAcepto Then
            ListCuentas.Clear()
            For Each Fila As ItemCuentasAsientos In SeleccionarCuentaDocumento.PListaDeCuentas
                Item = New ListViewItem(Format(Fila.Cuenta, "000-000000-00"))
                Item.SubItems.Add(Fila.ImporteB.ToString)
                Item.SubItems.Add("0")
                ListCuentas.Items.Add(Item)
            Next
        End If

        SeleccionarCuentaDocumento.Dispose()

    End Sub
    Private Sub ButtonVerDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerDetalle.Click

        SeleccionarVarios.PEsCostoInsumo = True
        SeleccionarVarios.Pinsumo = ComboInsumo.SelectedValue
        SeleccionarVarios.PDtDetalleInsumo = DtConsumoDetalle.Copy
        SeleccionarVarios.ShowDialog()
        SeleccionarVarios.Dispose()

    End Sub
    Private Sub ButtonVerDetalleLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerDetalleLotes.Click

        If PConsumo = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        SeleccionarVarios.PEsConsumos = True
        SeleccionarVarios.PConsumo = PConsumo
        SeleccionarVarios.PAbierto = PAbierto
        SeleccionarVarios.ShowDialog()
        SeleccionarVarios.Dispose()

    End Sub
    Private Sub ComboNegocio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboNegocio.Validating

        If IsNothing(ComboNegocio.SelectedValue) Then ComboNegocio.SelectedValue = 0

        Dim SqlFecha As String = ""
        SqlFecha = "IntFechaDesde <= " & Format(DateTime1.Value, "yyyyMMdd") & " AND IntFechaHasta >= " & Format(DateTime1.Value, "yyyyMMdd") & ";"
        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & ComboNegocio.SelectedValue & " AND Cerrado = 0 AND " & SqlFecha
        ComboCosteo.DataSource = New DataTable
        ComboCosteo.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboCosteo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        ComboCosteo.DataSource.rows.add(Row)
        ComboCosteo.DisplayMember = "Nombre"
        ComboCosteo.ValueMember = "Costeo"
        ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub TextCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCantidad.KeyPress

        EsNumerico(e.KeyChar, TextCantidad.Text, GDecimales)

    End Sub
    Private Sub TextCantidad_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextCantidad.Validating

        If Not IsNumeric(TextCantidad.Text) Then TextCantidad.Text = ""

    End Sub
    Private Sub PictureLupaLote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupaLote.Click

        If PEsReventa Then
            SeleccionaLotesAfectados.PTipoOperacion = 2
        Else
            SeleccionaLotesAfectados.PTipoOperacion = 1
        End If
        SeleccionaLotesAfectados.PEsConsumoInsumos = True
        SeleccionaLotesAfectados.ComboTipoOperacion.Enabled = False
        SeleccionaLotesAfectados.CheckAbierto.Checked = True
        SeleccionaLotesAfectados.CheckCerrado.Checked = True
        SeleccionaLotesAfectados.CheckAbierto.Enabled = False
        SeleccionaLotesAfectados.CheckCerrado.Enabled = False
        SeleccionaLotesAfectados.PListaDeLotes = ListaDeLotes
        SeleccionaLotesAfectados.ShowDialog()
        If SeleccionaLotesAfectados.PAceptado Then
            ListaDeLotes = SeleccionaLotesAfectados.PListaDeLotes
        End If
        SeleccionaLotesAfectados.Dispose()

    End Sub
    Private Sub ArmaArchivos()                                            'ArmaArchivos

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtConsumoCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM ConsumosCabeza WHERE Consumo = " & PConsumo & ";", ConexionConsumo, DtConsumoCabeza) Then Me.Close() : Exit Sub
        If PConsumo <> 0 And DtConsumoCabeza.Rows.Count = 0 Then
            MsgBox("Consumo No Existen En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If DtConsumoCabeza.Rows.Count = 0 Then
            Dim Row As DataRow = DtConsumoCabeza.NewRow
            Row("Consumo") = 0
            Row("Insumo") = Insumo
            Row("Negocio") = 0
            Row("Costeo") = 0
            Row("Deposito") = Deposito
            Row("Fecha") = Now
            Row("ImporteConIva") = 0
            Row("ImporteSinIva") = 0
            Row("Cantidad") = 0
            Row("Estado") = 1
            Row("Comentario") = ""
            DtConsumoCabeza.Rows.Add(Row)
        End If

        Insumo = DtConsumoCabeza.Rows(0).Item("Insumo")

        If PConsumo <> 0 Then
            ComboCosteo.DataSource = Tablas.Leer("SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & DtConsumoCabeza.Rows(0).Item("Negocio") & " AND Costeo = " & DtConsumoCabeza.Rows(0).Item("Costeo") & ";")
            ComboCosteo.DisplayMember = "Nombre"
            ComboCosteo.ValueMember = "Costeo"
        End If

        MuestraCabeza()

        DtConsumoDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM ConsumosDetalle WHERE Consumo = " & PConsumo & ";", ConexionConsumo, DtConsumoDetalle) Then Me.Close() : Exit Sub
        If PConsumo <> 0 And DtConsumoDetalle.Rows.Count = 0 Then
            MsgBox("Detalle Consumo No Existe En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        DtConsumoLotes = New DataTable
        If Not Tablas.Read("SELECT * FROM ConsumosLotes WHERE Consumo = " & PConsumo & ";", ConexionConsumo, DtConsumoLotes) Then Me.Close() : Exit Sub

        ListaDeLotes = New List(Of FilaComprobanteFactura)
        For Each Row As DataRow In DtConsumoLotes.Rows
            Dim Item As New FilaComprobanteFactura
            Item.Lote = Row("Lote")
            Item.Secuencia = Row("Secuencia")
            ListaDeLotes.Add(Item)
        Next

        Dim TipoOperacion As Integer
        If DtConsumoLotes.Rows.Count <> 0 Then
            TipoOperacion = HallaTipoOperacion(DtConsumoLotes.Rows(0).Item("Lote"), DtConsumoLotes.Rows(0).Item("Secuencia"), Conexion)
            If TipoOperacion < 0 Then
                MsgBox("Error Base de Datos al leer Consumos.")
                Me.Close() : Exit Sub
            End If
            If TipoOperacion = 0 And Not PermisoTotal Then               'No esta en Conexion entoces esta en conexionN, 
                MsgBox("Error Base de Datos al leer Consumos(1000).")
                Me.Close() : Exit Sub
            End If
            If TipoOperacion = 0 Then
                TipoOperacion = HallaTipoOperacion(DtConsumoLotes.Rows(0).Item("Lote"), DtConsumoLotes.Rows(0).Item("Secuencia"), ConexionN)
                If TipoOperacion < 0 Then
                    MsgBox("Error Base de Datos al leer Consumos.")
                    Me.Close() : Exit Sub
                End If
            End If
        End If

        If PConsumo <> 0 Then
            If TipoOperacion = 1 Then PEsConsignacion = True
            If TipoOperacion = 2 Then PEsReventa = True
            If DtConsumoCabeza.Rows(0).Item("Negocio") <> 0 Then PEsNegocio = True
            Panel6.Enabled = False
            Panel4.Enabled = False
            Panel3.Enabled = False
        Else
            Panel6.Enabled = True
            Panel4.Enabled = True
            Panel3.Enabled = True
        End If

        LabelTitulo.Text = "    Sin Destino"

        If PEsReventa Then
            LabelTitulo.Text = "   Para Lotes Reventa"
            Panel2.Visible = True
        End If
        If PEsConsignacion Then
            LabelTitulo.Text = "Para Lotes Consignación"
            Panel2.Visible = True
        End If
        If PEsNegocio Then
            LabelTitulo.Text = "Para Negocio"
            Panel4.Visible = True
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtConsumoCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Consumo")
        AddHandler Enlace.Format, AddressOf FormatCero
        TextConsumo.DataBindings.Clear()
        TextConsumo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Insumo")
        ComboInsumo.DataBindings.Clear()
        ComboInsumo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Deposito")
        ComboDeposito.DataBindings.Clear()
        ComboDeposito.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "ImporteConIva")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextCosto.DataBindings.Clear()
        TextCosto.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "ImporteSinIva")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextCostoSinIva.DataBindings.Clear()
        TextCostoSinIva.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Negocio")
        ComboNegocio.DataBindings.Clear()
        ComboNegocio.DataBindings.Add(Enlace)

        If ComboNegocio.SelectedValue <> 0 Then ComboNegocio_Validating(Nothing, Nothing)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Costeo")
        ComboCosteo.DataBindings.Clear()
        ComboCosteo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cantidad")
        AddHandler Enlace.Format, AddressOf FormatCero
        TextCantidad.DataBindings.Clear()
        TextCantidad.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub FormatCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then Numero.Value = Format(Numero.Value, "#")

    End Sub
    Private Sub ParseTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Function ExisteEnLista(ByVal ListaDeLotesW As List(Of FilaComprobanteFactura), ByVal Lote As Integer, ByVal Secuencia As Integer) As Boolean

        For Each Fila As FilaComprobanteFactura In ListaDeLotesW
            If Fila.Lote = Lote And Fila.Secuencia = Secuencia Then Return True
        Next

        Return False

    End Function
    Private Function HallaCantidadEnLista(ByVal ListaDeLotesW As List(Of FilaComprobanteFactura), ByVal Lote As Integer, ByVal Secuencia As Integer) As Integer

        For Each Fila As FilaComprobanteFactura In ListaDeLotesW
            If Fila.Lote = Lote And Fila.Secuencia = Secuencia Then Return Fila.Cantidad
        Next

    End Function
    Private Sub HacerAlta(ByVal DtConsumoLotesAux As DataTable)

        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable

        'Arma Tipo Operaciones de Lotes para Asientos.
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaB) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleB) Then Me.Close() : Exit Sub
            If ImporteB <> 0 Then
                If Not ArmaArchivosAsiento("A", DtAsientoCabezaB, DtAsientoDetalleB, ImporteB) Then Exit Sub
            End If
        End If

        Dim Numero As Double
        Dim Resul As Double
        Dim NumeroAsientoB As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            Numero = UltimaNumeracion(ConexionConsumo)
            If Numero < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If

            DtConsumoCabeza.Rows(0).Item("Consumo") = Numero
            For Each Row As DataRow In DtConsumoDetalle.Rows
                Row("Consumo") = Numero
            Next
            For Each Row As DataRow In DtConsumoLotesAux.Rows
                Row("Consumo") = Numero
            Next

            'Halla Ultima numeracion Asiento.
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                NumeroAsientoB = UltimaNumeracionAsiento(Conexion)
                If NumeroAsientoB < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                DtAsientoCabezaB.Rows(0).Item("Asiento") = NumeroAsientoB
                DtAsientoCabezaB.Rows(0).Item("Documento") = Numero
                For Each Row As DataRow In DtAsientoDetalleB.Rows
                    Row("Asiento") = NumeroAsientoB
                Next
            End If
            '
            Resul = ActualizaConsumo(DtAsientoCabezaB, DtAsientoDetalleB, DtConsumoLotesAux)

            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = -2 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario Modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PConsumo = Numero
            GModificacionOk = True
            ArmaArchivos()
        End If

    End Sub
    Private Sub HacerModificacion(ByVal DtConsumoLotesAux As DataTable)

        Dim DtasientoCabezaB As New DataTable
        Dim DtasientoDetalleB As New DataTable

        Dim Resul As Integer = ActualizaConsumo(DtasientoCabezaB, DtasientoDetalleB, DtConsumoLotesAux)

        If Resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = -2 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario Modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

    End Sub
    Private Function ActualizaConsumo(ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtConsumoLotesAux As DataTable) As Integer

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtConsumoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtConsumoCabeza.GetChanges, "ConsumosCabeza", ConexionConsumo)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtConsumoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtConsumoDetalle.GetChanges, "ConsumosDetalle", ConexionConsumo)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtConsumoLotesAux.GetChanges) Then
                    Resul = GrabaTabla(DtConsumoLotesAux.GetChanges, "ConsumosLotes", ConexionConsumo)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtStock.GetChanges) Then
                    Resul = GrabaTabla(DtStock.GetChanges, "StockInsumos", ConexionConsumo)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabezaB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaB.GetChanges, "AsientosCabeza", ConexionConsumo)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoDetalleB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleB.GetChanges, "AsientosDetalle", ConexionConsumo)
                    If Resul <= 0 Then Return Resul
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
    Private Sub Opciones()

        OpcionInsumo.ShowDialog()
        Insumo = OpcionInsumo.PInsumo
        Deposito = OpcionInsumo.PDeposito
        OpcionInsumo.Dispose()

    End Sub
    Private Function CalculaAsignacion(ByVal Insumo As Integer, ByVal Deposito As Integer, ByVal ConexionStr As String, ByVal Cantidad As Double) As Double

        Dim Sql As String
        DtStock = New DataTable
        Dim Asignado As Double

        Sql = "SELECT * FROM StockInsumos WHERE Articulo = " & Insumo & " AND Deposito = " & Deposito & " AND Stock <> 0 ORDER BY OrdenCompra;"

        If Not Tablas.Read(Sql, ConexionStr, DtStock) Then Return -1
        DtConsumoDetalle.Clear()
        For Each Row As DataRow In DtStock.Rows
            If Row("Stock") <= Cantidad Then
                Asignado = Row("Stock")
                Cantidad = Cantidad - Row("Stock") : Row("Stock") = 0
            Else
                Asignado = Cantidad
                Row("Stock") = Row("Stock") - Cantidad : Cantidad = 0
            End If
            Dim Row1 As DataRow = DtConsumoDetalle.NewRow
            Row1("Consumo") = 0
            Row1("OrdenCompra") = Row("OrdenCompra")
            Row1("Cantidad") = Asignado
            DtConsumoDetalle.Rows.Add(Row1)
            If Cantidad = 0 Then Return 0
        Next

        Return Cantidad

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Consumo) FROM ConsumosCabeza;", Miconexion)
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
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable, ByVal Importe As Double) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)

        'Arma Lista con Cuentas definidas en documento.
        If ListCuentas.Visible Then
            If ListCuentas.Items.Count = 0 Then
                MsgBox("Falta Informar Cuenta. Operación se CANCELA.")
                Return False
            End If
            Dim ImporteB As Double
            For I As Integer = 0 To ListCuentas.Items.Count - 1
                Dim Fila As New ItemCuentasAsientos
                Dim Cuenta As String = ListCuentas.Items.Item(I).SubItems(0).Text
                Fila.Cuenta = Mid$(Cuenta, 1, 3) & Mid$(Cuenta, 5, 6) & Mid$(Cuenta, 12, 2)
                Dim Porcentaje As Double = CDbl(ListCuentas.Items.Item(I).SubItems(1).Text) * 100 / CDbl(TextCostoSinIva.Text)
                Fila.Importe = Importe * Porcentaje / 100
                Fila.Clave = 202
                ListaCuentas.Add(Fila)
                ImporteB = ImporteB + CDbl(ListCuentas.Items.Item(I).SubItems(1).Text)
            Next
            If ImporteB <> CDbl(TextCostoSinIva.Text) Then
                MsgBox("Importe de Cuentas Informada Difiere del Costo Sin Iva.")
                Return False
            End If
        End If

        'Arma lista de Insumos, Utiliso listaRetenciones.
        Dim Item As New ItemListaConceptosAsientos
        Item.Clave = ComboInsumo.SelectedValue
        Item.Importe = Importe
        Item.TipoIva = 0
        ListaRetenciones.Add(Item)

        If Not Asiento(6000, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

        Return True

    End Function
    Private Function Valida() As Boolean

        If Panel2.Visible Then
            If ListaDeLotes.Count = 0 Then
                MsgBox("Falta Informar Lote.")
                Return False
            End If
        End If

        If Panel4.Visible Then
            If ComboNegocio.SelectedValue = 0 Then
                MsgBox("Debe Informar Negocio.")
                Return False
            End If
            If ComboCosteo.SelectedValue = 0 Then
                MsgBox("Debe Informar Costeo.")
                Return False
            End If
        End If

        If TextCantidad.Text = "" Then
            MsgBox("Falta Informar Cantidad.")
            Return False
        End If

        If CDbl(TextCantidad.Text) = 0 Then
            MsgBox("Falta Informar Cantidad.")
            Return False
        End If

        If CDbl(TextCosto.Text) = 0 Then
            MsgBox("Falta Calcular Costo.")
            Return False
        End If

        Return True

    End Function
    Public Function HallaOperacionOrden(ByVal ConexionStr) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Orden FROM OrdenCompraCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        If CDbl(Ultimo) <> 0 Then Return 1
                    Else : Return -1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try
        '
        If Not PermisoTotal Then Return -2
        '
        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Orden FROM OrdenCompraCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        If CDbl(Ultimo) <> 0 Then Return 2
                    Else : Return -1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

        Return -1

    End Function

   
   
End Class