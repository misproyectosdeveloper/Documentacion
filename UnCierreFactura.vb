Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnCierreFactura
    Public PFacturaB As Double
    Public PFacturaN As Double
    Public PNota As Double
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Dim ListaDeLotes As List(Of FilaAsignacion)
    '
    Private MiEnlazador As New BindingSource
    '
    Private DtGrid As DataTable
    'Definicion para notas.
    Dim DtCabezaN As DataTable
    Dim DtDetalleN As DataTable
    'Definicion para facturas. 
    Dim DtCabezaFacturaB As DataTable
    Dim DtCabezaFacturaN As DataTable
    Dim DtDetalleFacturaB As DataTable
    Dim DtDetalleFacturaN As DataTable
    Dim DtLotesFacturaB As DataTable
    Dim DtLotesFacturaN As DataTable
    '
    Dim FacturaRelacionada As Double
    Dim UltimoNumero As Double
    Dim TipoFactura As Integer
    Dim UltimaFechaW As DateTime
    Dim Opcion As Integer = 0
    Dim NotaMixta As Boolean
    Dim FacturaMixta As Boolean
    Dim MonedaFactura As Integer
    Dim ErrorImpresion As Boolean
    Dim FormaPago As Integer
    Dim Remito As Integer
    Dim DocumentoAsiento As Integer
    Dim DocumentoAsientoCosto As Integer
    Dim Paginas As Integer = 0
    Dim Copias As Integer
    Dim CambioBak As Double = 0
    Private Sub UnCierreFacturaExportacion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If Not PermisoEscritura(6) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False

        LlenaCombo(ComboCliente, "", "Clientes")
        LlenaComboTablas(ComboDeposito, 19)

        ComboClienteOperacion.DataSource = New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Clientes WHERE DeOperacion = 1;", Conexion, ComboClienteOperacion.DataSource) Then Me.Close() : Exit Sub
        Dim Row As DataRow = ComboClienteOperacion.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboClienteOperacion.DataSource.Rows.Add(Row)
        ComboClienteOperacion.DisplayMember = "Nombre"
        ComboClienteOperacion.ValueMember = "Clave"

        ArmaTipoIva(ComboTipoIva)
        LlenaComboTablas(ComboPais, 28)

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Row = ComboMoneda.DataSource.newrow
        Row("Nombre") = "PESOS"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"
        With ComboMoneda
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        GModificacionOk = False

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        UltimaFechaW = UltimaFecha(Conexion)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        GModificacionOk = False

        MiEnlazador.EndEdit()
        Grid.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtCabezaNAux As DataTable = DtCabezaN.Copy
        Dim DtDetalleNAux As DataTable = DtDetalleN.Copy
        Dim DtLotesFacturaBAux As DataTable = DtLotesFacturaB.Copy
        Dim DtLotesFacturaNAux As DataTable = DtLotesFacturaN.Copy

        ActualizaArchivos("A", DtCabezaNAux, DtDetalleNAux, DtLotesFacturaBAux, DtLotesFacturaNAux)

        If IsNothing(DtCabezaNAux.GetChanges) And IsNothing(DtDetalleNAux.GetChanges) Then
            MsgBox("No Hay Cambios. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma Tipo Operaciones de Lotes para Asientos.
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable

        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaN) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleN) Then Me.Close() : Exit Sub
            If Not ArmaArchivosAsiento("A", DtAsientoCabezaN, DtAsientoDetalleN) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
        End If

        Dim NumeroN As Double = 0
        Dim NumeroAsientoN As Double
        Dim NumeroW As Double

        For i As Integer = 1 To 50
            NumeroN = UltimaNumeracion(ConexionN)
            If NumeroN < 0 Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            ' 
            DtCabezaNAux.Rows(0).Item("Nota") = NumeroN
            For Each Row As DataRow In DtDetalleNAux.Rows
                Row.Item("Nota") = NumeroN
            Next
            '
            'Halla Ultima numeracion Asiento N
            NumeroAsientoN = UltimaNumeracionAsiento(ConexionN)
            If NumeroAsientoN < 0 Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            DtAsientoCabezaN.Rows(0).Item("Asiento") = NumeroAsientoN
            DtAsientoCabezaN.Rows(0).Item("Documento") = NumeroN
            For Each Row As DataRow In DtAsientoDetalleN.Rows
                Row("Asiento") = NumeroAsientoN
            Next
            '
            NumeroW = ActualizaNota(DtCabezaNAux, DtDetalleNAux, DtLotesFacturaBAux, DtLotesFacturaNAux, DtAsientoCabezaN, DtAsientoDetalleN)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If NumeroW = -10 Then
            MsgBox("Nota Ya Fue Impresa. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = False
            PNota = NumeroN
            If Not ArmaArchivos() Then Me.Close() : Exit Sub
        End If

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota Ya esta Anulada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If DtCabezaN.Rows(0).Item("Importe") <> DtCabezaN.Rows(0).Item("Saldo") Then
            MsgBox("Cierre Factura Tiene Imputaciones. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        Dim DtCabezaNAux As DataTable = DtCabezaN.Copy
        Dim DtDetalleNAux As DataTable = DtDetalleN.Copy
        Dim DtLotesFacturaBAux As DataTable = DtLotesFacturaB.Copy
        Dim DtLotesFacturaNAux As DataTable = DtLotesFacturaN.Copy

        ActualizaArchivos("B", DtCabezaNAux, DtDetalleNAux, DtLotesFacturaBAux, DtLotesFacturaNAux)

        DtCabezaNAux.Rows(0).Item("Estado") = 3

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable

        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(7100, DtCabezaN.Rows(0).Item("Nota"), DtAsientoCabezaN, ConexionN) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            End If
        End If
        If DtAsientoCabezaN.Rows.Count <> 0 Then DtAsientoCabezaN.Rows(0).Item("Estado") = 3
        '
        If MsgBox("Nota Credito se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        GModificacionOk = False

        Dim Resul = ActualizaNota(DtCabezaNAux, DtDetalleNAux, DtLotesFacturaBAux, DtLotesFacturaNAux, DtAsientoCabezaN, DtAsientoDetalleN)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If Resul < 0 Then
            MsgBox("Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario Modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Cierre Factura Anulada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            If Not ArmaArchivos() Then Me.Close() : Exit Sub
        End If

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Nota debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ErrorImpresion = False
        Paginas = 0
        If PAbierto Then
            Copias = 2
        Else : Copias = 1
        End If

        Dim print_document As New PrintDocument
        AddHandler print_document.PrintPage, AddressOf Print_PrintPage

        print_document.Print()

        If ErrorImpresion Then Exit Sub

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PNota = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 7100
        ListaAsientos.PDocumentoN = DtCabezaN.Rows(0).Item("Nota")
        ListaAsientos.Show()

    End Sub
    Private Sub TextCambio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCambio.KeyPress

        EsNumerico(e.KeyChar, TextCambio.Text, 3)

    End Sub
    Private Sub TextCambio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextCambio.Validating

        If TextCambio.Text = "" Then Exit Sub

        If CDbl(TextCambio.Text) = 0 Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextCambio.Text = ""
            TextCambio.Focus()
        End If

    End Sub
    Private Function ArmaArchivos() As Boolean

        DtCabezaN = New DataTable
        DtDetalleN = New DataTable
        DtCabezaFacturaB = New DataTable
        DtCabezaFacturaN = New DataTable
        DtDetalleFacturaB = New DataTable
        DtDetalleFacturaN = New DataTable
        DtLotesFacturaB = New DataTable
        DtLotesFacturaN = New DataTable

        CreaDtGrid()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Sql As String = "SELECT * FROM CierreFacturasCabeza WHERE Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionN, DtCabezaN) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        If PNota <> 0 And DtCabezaN.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Error Base de Datos al leer Cierre de Facturas.", MsgBoxStyle.Critical)
            Return False
        End If

        If PNota <> 0 Then
            PFacturaB = DtCabezaN.Rows(0).Item("Factura")
        End If

        If Not LeerArchivosFactura(DtCabezaFacturaB, DtDetalleFacturaB, DtLotesFacturaB, PFacturaB, Conexion) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
            MsgBox("Error Base de Datos al leer Facturas.", MsgBoxStyle.Critical)
            Return False
        End If

        If DtCabezaFacturaB.Rows(0).Item("Rel") Then
            PFacturaN = HallaFacturaRelacionada(PFacturaB)
        Else
            PFacturaN = 0
        End If

        If PFacturaN <> 0 Then
            If Not LeerArchivosFactura(DtCabezaFacturaN, DtDetalleFacturaN, DtLotesFacturaN, PFacturaN, ConexionN) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
                MsgBox("Error Base de Datos al leer Facturas.", MsgBoxStyle.Critical)
                Return False
            End If
        End If

        If PNota = 0 Then
            Dim Row1 As DataRow = DtCabezaN.NewRow
            Row1("Nota") = 0
            Row1("Factura") = PFacturaB
            Row1("FechaFactura") = DtCabezaFacturaB.Rows(0).Item("Fecha")
            Row1("Cliente") = DtCabezaFacturaB.Rows(0).Item("Cliente")
            Row1("ClienteOperacion") = DtCabezaFacturaB.Rows(0).Item("ClienteOperacion")
            Row1("Importe") = 0
            Row1("AsignadoAFactura") = 0
            Row1("Fecha") = Date.Now
            Row1("Moneda") = DtCabezaFacturaB.Rows(0).Item("Moneda")
            Row1("Cambio") = 0
            Row1("Estado") = 1
            DtCabezaN.Rows.Add(Row1)
        End If

        MuestraCabeza(DtCabezaN)

        Sql = "SELECT * FROM CierreFacturasDetalle WHERE Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionN, DtDetalleN) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False

        If PNota <> 0 Then
            PFacturaB = DtCabezaN.Rows(0).Item("Factura")
        End If

        If PNota = 0 Then
            DtCabezaN.Rows(0).Item("Cliente") = DtCabezaFacturaB.Rows(0).Item("Cliente")
        End If

        If Not LlenaDatosCliente(DtCabezaN.Rows(0).Item("Cliente")) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False

        For Each Row As DataRow In DtDetalleFacturaB.Rows
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Indice") = Row("Indice")
            Row1("Articulo") = Row("Articulo")
            Row1("KilosXUnidad") = Row("KilosXUnidad")
            Row1("Cantidad") = Row("Cantidad") - Row("Devueltas")
            Row1("PrecioB") = Row("Precio")
            Row1("NetoB") = CalculaNeto(Row1("Cantidad"), Row1("PrecioB"))
            Row1("PrecioN") = 0
            Row1("NetoN") = 0
            Row1("PrecioCierre") = 0
            Row1("NetoCierre") = 0
            Row1("Importe") = 0
            DtGrid.Rows.Add(Row1)
        Next

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtDetalleFacturaN.Rows
            RowsBusqueda = DtGrid.Select("Indice = " & Row("Indice"))
            RowsBusqueda(0).Item("PrecioN") = Row("Precio")
            RowsBusqueda(0)("NetoN") = CalculaNeto(RowsBusqueda(0)("Cantidad"), Row("Precio"))
        Next

        If PNota <> 0 Then
            For Each Row As DataRow In DtDetalleN.Rows
                RowsBusqueda = DtGrid.Select("Indice = " & Row("Indice"))
                RowsBusqueda(0).Item("PrecioCierre") = Row("Precio")
                RowsBusqueda(0).Item("NetoCierre") = Row("Importe")
            Next
        End If

        TipoFactura = Strings.Left(DtCabezaFacturaB.Rows(0).Item("Factura"), 1)

        ComboDeposito.SelectedValue = DtCabezaFacturaB.Rows(0).Item("Deposito")
        MonedaFactura = DtCabezaFacturaB.Rows(0).Item("Moneda")
        DocumentoAsiento = 41 : DocumentoAsientoCosto = 6073

        'Muestra panel de Moneda.
        If PNota = 0 Then
            TextCambio.Text = ""
        Else
            TextCambio.Text = FormatNumber(DtCabezaN.Rows(0).Item("Cambio"), 3)
        End If

        LabelFactura.Text = ""
        LabelFactura.Text = NumeroEditado(DtCabezaFacturaB.Rows(0).Item("Factura"))
        If DtCabezaFacturaN.Rows.Count <> 0 Then
            LabelFactura.Text = LabelFactura.Text & " / " & NumeroEditado(DtCabezaFacturaN.Rows(0).Item("Factura"))
        End If

        ListaDeLotes = New List(Of FilaAsignacion)
        For Each Row As DataRow In DtLotesFacturaB.Rows
            Dim Fila As New FilaAsignacion
            Fila.Indice = Row("Indice")
            Fila.Lote = Row("Lote")
            Fila.Secuencia = Row("Secuencia")
            Fila.Deposito = Row("Deposito")
            Fila.Operacion = Row("Operacion")
            Fila.Asignado = Row("Cantidad")
            ListaDeLotes.Add(Fila)
        Next

        Grid.DataSource = DtGrid


        If PNota = 0 Then
            Panel1.Enabled = True
            Grid.ReadOnly = False
        Else
            Panel1.Enabled = False
            Grid.ReadOnly = True
        End If

        CalculaTotal()

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub ActualizaArchivos(ByVal Funcion As String, ByVal DtCabeza As DataTable, ByVal DtDetalle As DataTable, ByVal DtLotesFacturaBW As DataTable, ByVal DtLotesFacturaNW As DataTable)

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow

        If Funcion = "A" Then
            DtCabeza.Rows(0).Item("Saldo") = DtCabeza.Rows(0).Item("Importe")
            For Each Row In DtGrid.Rows
                Dim Row1 As DataRow = DtDetalle.NewRow
                Row1("Indice") = Row("Indice")
                Row1("Articulo") = Row("Articulo")
                Row1("Cantidad") = Row("Cantidad")
                Row1("Precio") = Row("PrecioCierre")
                Row1("Importe") = Row("NetoCierre")
                DtDetalle.Rows.Add(Row1)
            Next
            'Actualiza Asignacion de lotes de Facturas.
            If DtLotesFacturaNW.Rows.Count <> 0 Then
                For Each Row In DtLotesFacturaNW.Rows
                    RowsBusqueda = DtGrid.Select("Indice = " & Row("Indice"))
                    Dim NetoW As Double = 0
                    NetoW = CalculaNeto(Row("Cantidad"), RowsBusqueda(0).Item("PrecioCierre"))
                    If MonedaFactura <> 1 Then NetoW = Trunca(CDbl(TextCambio.Text) * NetoW)
                    Row("ImporteSinIva") = Row("ImporteSinIva") + NetoW
                    Row("Importe") = Row("Importe") + NetoW
                Next
            Else
                If DtLotesFacturaBW.Rows.Count = 0 Then
                    For Each Row In DtLotesFacturaBW.Rows
                        RowsBusqueda = DtGrid.Select("Indice = " & Row("Indice"))
                        Dim NetoW As Double = 0
                        NetoW = CalculaNeto(Row("Cantidad"), RowsBusqueda(0).Item("PrecioCierre"))
                        If MonedaFactura <> 1 Then NetoW = Trunca(CDbl(TextCambio.Text) * NetoW)
                        Row("ImporteSinIva") = Row("ImporteSinIva") + NetoW
                        Row("Importe") = Row("Importe") + NetoW
                    Next
                End If
            End If
        End If

        If Funcion = "B" Then
            If DtLotesFacturaNW.Rows.Count <> 0 Then
                For Each Row In DtLotesFacturaNW.Rows
                    RowsBusqueda = DtGrid.Select("Indice = " & Row("Indice"))
                    Dim NetoW As Double = 0
                    NetoW = CalculaNeto(Row("Cantidad"), RowsBusqueda(0).Item("PrecioCierre"))
                    If MonedaFactura <> 1 Then NetoW = Trunca(CDbl(TextCambio.Text) * NetoW)
                    Row("ImporteSinIva") = Row("ImporteSinIva") - NetoW
                    Row("Importe") = Row("Importe") - NetoW
                Next
            Else
                If DtLotesFacturaBW.Rows.Count = 0 Then
                    For Each Row In DtLotesFacturaBW.Rows
                        RowsBusqueda = DtGrid.Select("Indice = " & Row("Indice"))
                        Dim NetoW As Double = 0
                        NetoW = CalculaNeto(Row("Cantidad"), RowsBusqueda(0).Item("PrecioCierre"))
                        If MonedaFactura <> 1 Then NetoW = Trunca(CDbl(TextCambio.Text) * NetoW)
                        Row("ImporteSinIva") = Row("ImporteSinIva") - NetoW
                        Row("Importe") = Row("Importe") - NetoW
                    Next
                End If
            End If
        End If

    End Sub
    Private Function LeerArchivosFactura(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtLotesW As DataTable, ByVal Factura As Double, ByVal ConexionW As String) As Boolean

        If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Factura = " & Factura & ";", ConexionW, DtCabezaW) Then Return False
        If Not Tablas.Read("SELECT * FROM FacturasDetalle WHERE Factura = " & Factura & ";", ConexionW, DtDetalleW) Then Return False
        If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante = 2 AND Comprobante = " & Factura & ";", ConexionW, DtLotesW) Then Return False

        Return True

    End Function
    Private Function LeerArchivosFacturaRelacionada(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtLotesW As DataTable, ByVal Factura As Double, ByVal ConexionW As String) As Boolean

        If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Relacionada = " & Factura & ";", ConexionW, DtCabezaW) Then Return False

        Dim Relacionada As Double = DtCabezaW.Rows(0).Item("Factura")

        If Not Tablas.Read("SELECT * FROM FacturasDetalle WHERE Factura = " & Relacionada & ";", ConexionW, DtDetalleW) Then Return False
        If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante = 2 AND Comprobante = " & Relacionada & ";", ConexionW, DtLotesW) Then Return False

        Return True

    End Function
    Private Sub MuestraCabeza(ByVal Dt As DataTable)

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = Dt

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Nota")
        LabelNota.DataBindings.Clear()
        LabelNota.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Cliente")
        ComboCliente.DataBindings.Clear()
        ComboCliente.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "ClienteOperacion")
        ComboClienteOperacion.DataBindings.Clear()
        ComboClienteOperacion.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        AddHandler Enlace.Format, AddressOf FormatFecha
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaFactura")
        AddHandler Enlace.Format, AddressOf FormatFecha
        TextFechafactura.DataBindings.Clear()
        TextFechafactura.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Moneda")
        ComboMoneda.DataBindings.Clear()
        ComboMoneda.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cambio")
        AddHandler Enlace.Format, AddressOf FormatCambio
        TextCambio.DataBindings.Clear()
        TextCambio.DataBindings.Add(Enlace)
        CambioBak = Dt.Rows(0).Item("Cambio")

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextNetoCierre.DataBindings.Clear()
        TextNetoCierre.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatCambio(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 3)
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub FormatFecha(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "dd/MM/yyyy")

    End Sub
    Private Sub LlenaCombosGrid()

        Articulo.DataSource = TodosLosArticulos()
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Function ActualizaNota(ByVal DtCabezaNAux As DataTable, ByVal DtDetalleNAux As DataTable, ByVal DtLotesFacturaBAux As DataTable, ByVal DtLotesFacturaNAux As DataTable, ByVal DtAsientoCabezaN As DataTable, ByVal DtAsientoDetalleN As DataTable) As Double

        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaNAux.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaNAux.GetChanges, "CierreFacturasCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtDetalleNAux.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleNAux.GetChanges, "CierreFacturasDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLotesFacturaBAux.GetChanges) Then
                    Resul = GrabaTabla(DtLotesFacturaBAux.GetChanges, "AsignacionLotes", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLotesFacturaNAux.GetChanges) Then
                    Resul = GrabaTabla(DtLotesFacturaNAux.GetChanges, "AsignacionLotes", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabezaN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
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
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Indice As New DataColumn("Indice")
        Indice.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Indice)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim KilosXUnidad As New DataColumn("KilosXUnidad")
        KilosXUnidad.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(KilosXUnidad)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cantidad)

        Dim PrecioB As New DataColumn("PrecioB")
        PrecioB.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(PrecioB)

        Dim NetoB As New DataColumn("NetoB")
        NetoB.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(NetoB)

        Dim PrecioN As New DataColumn("PrecioN")
        PrecioN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(PrecioN)

        Dim NetoN As New DataColumn("NetoN")
        NetoN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(NetoN)

        Dim PrecioCierre As New DataColumn("PrecioCierre")
        PrecioCierre.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(PrecioCierre)

        Dim NetoCierre As New DataColumn("NetoCierre")
        NetoCierre.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(NetoCierre)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

    End Sub
    Private Sub CalculaTotal()

        Dim NetoB As Double = 0
        Dim NetoN As Double = 0
        Dim NetoCierre As Double = 0
        Dim Importe As Double = 0

        For Each Row As DataRow In DtGrid.Rows
            Row("Importe") = Trunca(Row("NetoB") + Row("NetoN") + Row("NetoCierre"))
            NetoCierre = Trunca(NetoCierre + Row("NetoCierre"))
            NetoB = Trunca(NetoB + Row("NetoB"))
            NetoN = Trunca(NetoN + Row("NetoN"))
            Importe = Trunca(Importe + Row("Importe"))
        Next

        Grid.Refresh()

        TextNetoB.Text = FormatNumber(NetoB, GDecimales)
        TextNetoN.Text = FormatNumber(NetoN, GDecimales)
        TextNetoCierre.Text = FormatNumber(NetoCierre, GDecimales)
        TextImporte.Text = FormatNumber(Importe, GDecimales)

    End Sub
    Private Function LlenaDatosCliente(ByVal Cliente As Integer) As Boolean

        Dim Dta As New DataTable

        Dim Sql As String = "SELECT * FROM Clientes WHERE Clave = " & Cliente & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, el Cliente ya no existe o error en la Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Dta.Dispose()
            Return False
        End If

        TextCalle.Text = Dta.Rows(0).Item("Calle")
        TextLocalidad.Text = Dta.Rows(0).Item("Localidad")
        TextProvincia.Text = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
        TextTelefonos.Text = Dta.Rows(0).Item("Telefonos")
        TextFaxes.Text = Dta.Rows(0).Item("Faxes")
        TextCuit.Text = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")
        TipoFactura = Dta.Rows(0).Item("TipoIva")

        Dta.Dispose()

        Return True

    End Function
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsientoAux As New List(Of ItemLotesParaAsientos)
        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow

        If ListaDeLotes.Count <> 0 Then
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Asignado <> 0 Then
                    Dim Tipo As Integer
                    Dim Centro As Integer
                    Dim Fila2 As New ItemLotesParaAsientos
                    If Fila.Operacion = 1 Then
                        HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, Conexion, Tipo, Centro)
                    Else : HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, ConexionN, Tipo, Centro)
                    End If
                    If Centro <= 0 Then
                        MsgBox("Error en Tipo Operacion en Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                        Return False
                    End If
                    RowsBusqueda = DtGrid.Select("Indice = " & Fila.Indice)
                    Fila2.TipoOperacion = Tipo
                    Fila2.Centro = Centro
                    Fila2.MontoNeto = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("PrecioCierre"))
                    If MonedaFactura <> 1 Then Fila2.MontoNeto = Trunca(CDbl(TextCambio.Text) * Fila2.MontoNeto)
                    If Tipo = 1 Then Fila2.Clave = 301 'consignacion
                    If Tipo = 2 Then Fila2.Clave = 300 'reventa
                    If Tipo = 3 Then Fila2.Clave = 303 'reventa MG
                    If Tipo = 4 Then Fila2.Clave = 302 'costeo
                    ListaLotesParaAsiento.Add(Fila2)
                End If
            Next
        End If

        Dim Item As New ItemListaConceptosAsientos
        '
        Dim MontoFinal As Double = 0
        For Each Row As DataRow In DtGrid.Rows
            MontoFinal = MontoFinal + CalculaNeto(Row("Cantidad"), Row("PrecioCierre"))
        Next
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = MontoFinal
        If MonedaFactura <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
        ListaConceptos.Add(Item)

        If Funcion = "A" Then
            If Not Asiento(7100, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False
        End If

        If DtAsientoDetalle.Rows.Count = 0 Then
            '   MsgBox("No se pudo Generar Asiento. Operación se CANCELA.")
            '  Return False
        End If

        Return True

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Nota) FROM CierreFacturasCabeza;", Miconexion)
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

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM NotasCreditoCabeza;", Miconexion)
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
    Private Sub Print_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20

        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 11)

        x = MIzq + 135 : y = MTop

        Texto = Format(DateTime1.Value, "dd/MM/yyyy")
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y + 15)

        x = MIzq : y = MTop + 42

        Try
            'Titulos.
            If PAbierto Then
                Texto = "CLIENTE    : " & ComboCliente.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "DOMICILIO  : " & TextCalle.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "LOCALIDAD  : " & TextLocalidad.Text & " " & TextProvincia.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "DOM.ENTREGA: "
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "CUIT       : " & TextCuit.Text & " " & ComboTipoIva.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                'Condicion de venta.
                Texto = ""
                If FormaPago = 1 Then
                    Texto = "CONDICION DE VENTA: Contado"
                End If
                If FormaPago = 2 Then
                    Texto = "CONDICION DE VENTA: Cuenta Corriente"
                End If
                If FormaPago = 3 Then
                    Texto = "CONDICION DE VENTA: Mixta"
                End If
                If FormaPago = 4 Then
                    Texto = "CONDICION DE VENTA: Contado Efectivo"
                End If
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                Texto = "REMITO: " & Format(Remito, "0000-00000000")
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 400, y)
            Else
                Texto = "CLIENTE    : " & ComboCliente.Text & "                Nro.: " & LabelNota.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            End If

            PrintFont = New Font("Courier New", 11)

            'Grafica -Rectangulo----------------------------------------------------------------------
            x = MIzq
            y = MTop + 72

            Dim Ancho As Integer = 185
            Dim Alto As Integer = 125
            Dim LineaCantidad As Integer = x + 115
            Dim LineaUnitario As Integer = x + 150
            Dim LineaImporte As Integer = x + Ancho
            Dim Longi As Integer
            Dim Xq As Integer
            Dim Yq As Integer
            Dim Partes() As String
            Dim Articulo As String

            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x + 90, y, x + 90, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCantidad, y, LineaCantidad, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaUnitario, y, LineaUnitario, y + Alto)
            'Titulos de descripcion.
            Texto = "ARTICULO"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 2, y + 2)
            Texto = "CANTIDAD"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi - 2
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "PR.UNITARIO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaUnitario - Longi - 2
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE TOTAL"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi - 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)
            '------------------------------------------------------------------------------------------------------------
            'Descripcion de Articulos.
            Yq = y - SaltoLinea
            For Each Row As DataGridViewRow In Grid.Rows
                If IsNothing(Row.Cells("Articulo").Value) Then Exit For
                Yq = Yq + SaltoLinea
                'Imprime Articulo.
                Partes = Split(Row.Cells("Articulo").FormattedValue, "(")
                Articulo = Partes(0)
                Texto = Articulo
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime cantidad.
                Dim Cantidad As Double
                Cantidad = Row.Cells("Cantidad").Value
                Texto = Cantidad.ToString
                Texto = Texto & "Uni"
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaCantidad - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Unitario.
                Dim Precio As Double = 0
                If ComboTipoIva.SelectedValue = 3 Then
                    Precio = Row.Cells("Precio").Value + Row.Cells("Precio").Value * Row.Cells("IvaB").Value / 100
                    Texto = FormatNumber(FormatoSinRedondeo3Decimales(Precio), 3) & "xUni"
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaUnitario - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Importe.
                    Texto = FormatNumber(Row.Cells("Importe").Value, GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Else
                    Precio = Row.Cells("Precio").Value
                    Texto = FormatNumber(Precio, 3) & "xUni"
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaUnitario - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Importe.
                    Texto = FormatNumber(Row.Cells("Neto").Value, GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
            Next

            Yq = MTop + 72 + Alto + 2
            PrintFont = New Font("Courier New", 10)
            If PAbierto Then
                Texto = GNombreEmpresa & " " & LabelNota.Text
            Else
                Texto = LabelNota.Text
            End If
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, Yq)

            'Totales
            PrintFont = New Font("Courier New", 11)

            If ComboTipoIva.SelectedValue = 3 Then
                'Neto
                Texto = "Sub-Total"
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
                Texto = TextImporte.Text
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Else
                'Neto
                Texto = "Neto"
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
                Texto = TextNetoB.Text
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                '
                Dim ListaIva As New List(Of ItemIva)
                'Iva.
                For Each Fila As ItemIva In ListaIva
                    Yq = Yq + SaltoLinea
                    Texto = "IVA. " & FormatNumber(Fila.Iva, GDecimales)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
                    Texto = FormatNumber(Fila.Importe, GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Next
                ListaIva = Nothing
            End If

            'Total
            Yq = Yq + SaltoLinea
            Texto = "Total"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
            Texto = TextImporte.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)

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
    Private Function FormatoSinRedondeo3Decimales(ByVal Numero As Double) As Double

        Dim PosicionDecimal As Integer = InStr(1, Numero.ToString, ",")
        If PosicionDecimal = 0 Then
            Return Numero
        Else
            Return CDbl(Mid(Numero.ToString, 1, PosicionDecimal + 3))
        End If

    End Function
    Private Function HallaFacturaRelacionada(ByVal Factura As Double) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Factura FROM FacturasCabeza WHERE Relacionada = " & Factura & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla de Facturas.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 And PNota = 0 Then
            MsgBox("Fecha Incorrecta. Existe Otro Comprobante con Fecha " & UltimaFechaW & " Posterior a la Informada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If
        If DateTime1.Value < CDate(TextFechafactura.Text) Then
            MsgBox("Fecha debe ser mayor o igual a fecha factura.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If
        If TextCambio.Text = "" Then
            MsgBox("Falta Informar Cambio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If
        If ComboMoneda.SelectedValue <> 1 And CDbl(TextCambio.Text) = 0 Then
            MsgBox("Falta Informar Cambio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If
        If ComboMoneda.SelectedValue = 1 And CDbl(TextCambio.Text) <> 1 Then
            MsgBox("Error, Cambio de Moneda Local debe ser 1.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If

        Dim Importe As Double = 0
        For Each Row As DataRow In DtGrid.Rows
            Importe = Importe + Row("NetoCierre")
        Next
        If Importe = 0 Then
            MsgBox("Debe Informar Nuevos Precios. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        'para manejo del autocoplete de articulos.
        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        CalculaTotal()

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "PrecioB" Or Grid.Columns(e.ColumnIndex).Name = "PrecioN" Or Grid.Columns(e.ColumnIndex).Name = "PrecioCierre" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales)
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "NetoB" Or Grid.Columns(e.ColumnIndex).Name = "NetoN" Or Grid.Columns(e.ColumnIndex).Name = "NetoCierre" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales)
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioCierre" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf Importe_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub Importe_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioCierre" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("-0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioCierre" Then
            Dim x As String = "1"
            EsNumerico(x, CType(sender, TextBox).Text.Trim, GDecimales + 1)
            If x = "" Then
                MsgBox("Importe erroneo o supera dicimales permitidos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                CType(sender, TextBox).Text = ""
                CType(sender, TextBox).Focus()
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("PrecioCierre")) Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            Dim NetoCierre As Double = CalculaNeto(e.Row("Cantidad"), e.ProposedValue)
            If Trunca(NetoCierre + e.Row("NetoB") + e.Row("NetoN")) < 0 Then
                '         MsgBox("Nuevo Precio Hace Negativo Importe del Articulo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                '         e.ProposedValue = e.Row("PrecioCierre")
                '          Exit Sub
            End If
            e.Row("NetoCierre") = CalculaNeto(e.Row("Cantidad"), e.ProposedValue)
        End If

    End Sub

End Class