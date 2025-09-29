Imports System.Transactions
Public Class UnaDevolucionLote
    Public PLote As Integer
    Public PAbierto As Boolean
    Public PDevolucion As Double
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    '
    Dim DtGrid As DataTable
    Dim DtLotes As New DataTable
    Dim DtCabeza As New DataTable
    Dim DtDetalle As New DataTable
    Dim DtOrdenCompraDetalle As DataTable
    '
    Dim ConexionLote As String
    Dim TipoOperacion As Integer
    Dim OrdenCompra As Double
    Private Sub UnaDevolucionLote_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If Not PermisoEscritura(5) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False
        LlenaCombo(ComboProveedor, "", "Proveedores")
        ComboProveedor.SelectedValue = 0
        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        LlenaCombosGrid()

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

        CreaDtGrid()

        ArmaArchivos()

        GModificacionOk = False

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid_ColumnChanging)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PDevolucion <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtCabezaAlta As New DataTable
        Dim DtDetalleAlta As New DataTable

        DtCabezaAlta = DtCabeza.Copy
        DtDetalleAlta = DtDetalle.Clone

        'Actualiza Lote.
        Dim DtLotesAlta As New DataTable
        DtLotesAlta = DtLotes.Copy
        '
        ActualizaLotes("A", DtLotesAlta)

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma detalle de devolucion.
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("Devolucion") <> 0 Then
                Dim Row As DataRow = DtDetalleAlta.NewRow()
                Row("Devolucion") = 0
                Row("Lote") = Row1("Lote")
                Row("Secuencia") = Row1("Secuencia")
                Row("Cantidad") = Row1("Devolucion")
                DtDetalleAlta.Rows.Add(Row)
            End If
        Next

        'Actualiza Orden Compra.
        Dim DtOrdenCompraDetalleAux As DataTable = DtOrdenCompraDetalle.Copy
        If OrdenCompra <> 0 Then
            If Not ActualizaOrdenCompra("A", DtOrdenCompraDetalleAux) Then Exit Sub
        End If

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        Dim NumeroDevolucion As Integer
        Dim i As Integer
        Dim NumeroW As Double
        Dim NumeroAsiento As Double

        For i = 1 To 50
            NumeroDevolucion = UltimaNumeracion(ConexionLote)
            If NumeroDevolucion < 0 Then
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If

            DtCabezaAlta.Rows(0).Item("Devolucion") = NumeroDevolucion
            For Each Row1 As DataRow In DtDetalleAlta.Rows
                Row1("Devolucion") = NumeroDevolucion
            Next

            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionLote)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = NumeroDevolucion
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            NumeroW = GrabaDevolucion(DtCabezaAlta, DtDetalleAlta, DtAsientoCabeza, DtAsientoDetalle, DtOrdenCompraDetalleAux, DtLotesAlta)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -1 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            MsgBox("Devolución Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PDevolucion = NumeroDevolucion
            ArmaArchivos()
        End If

        DtCabezaAlta.Dispose()
        DtDetalleAlta.Dispose()
        DtAsientoCabeza.Dispose()
        DtAsientoDetalle.Dispose()
        DtOrdenCompraDetalleAux.Dispose()
        DtLotesAlta.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PDevolucion = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Devolución Ya esta Anulada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If HallaArticuloDeshabilitado(DtGrid) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        'Actualiza Lote.
        Dim DtLotesBaja As New DataTable
        DtLotesBaja = DtLotes.Copy
        '
        ActualizaLotes("B", DtLotesBaja)

        'Actualiza Orden Compra.
        Dim DtOrdenCompraDetalleAux As DataTable = DtOrdenCompraDetalle.Copy
        If OrdenCompra <> 0 Then
            If Not ActualizaOrdenCompra("B", DtOrdenCompraDetalleAux) Then Exit Sub
        End If

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If Not HallaAsientosCabeza(6052, PDevolucion, DtAsientoCabeza, ConexionLote) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        For Each Row As DataRow In DtAsientoCabeza.Rows
            Row("Estado") = 3
        Next

        If MsgBox("Devolución se eliminara definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        'Anula cabeza devolucion.
        Dim DtCabezaBaja As New DataTable
        DtCabezaBaja = DtCabeza.Copy

        DtCabezaBaja.Rows(0).Item("Estado") = 3

        Dim DtDetalleW As New DataTable

        Dim NumeroW As Double = GrabaDevolucion(DtCabezaBaja, DtDetalleW, DtAsientoCabeza, DtAsientoDetalle, DtOrdenCompraDetalleAux, DtLotesBaja)

        If NumeroW = -1 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            MsgBox("Devolución Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            ArmaArchivos()
        End If

        DtLotesBaja.Dispose()
        DtOrdenCompraDetalleAux.Dispose()
        DtCabezaBaja.Dispose()
        DtDetalleW.Dispose()
        DtAsientoCabeza.Dispose()
        DtAsientoDetalle.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PDevolucion = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 6052
        If PAbierto Then
            ListaAsientos.PDocumentoB = PDevolucion
        Else
            ListaAsientos.PDocumentoN = PDevolucion
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonDevolverTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDevolverTodos.Click

        For i As Integer = 0 To Grid.Rows.Count - 1
            Grid.Rows(i).Cells("Devolucion").Value = Grid.Rows(i).Cells("Stock").Value
        Next

    End Sub
    Private Sub ArmaArchivos()

        If Not LeerIngreso() Then Me.Close() : Exit Sub

        DtCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM DevolucionMercaCabeza WHERE Devolucion = " & PDevolucion & ";", ConexionLote, DtCabeza) Then Me.Close() : Exit Sub
        If PDevolucion <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Devolución No Existen En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PDevolucion = 0 Then
            Dim Row As DataRow = DtCabeza.NewRow
            Row("Devolucion") = 0
            Row("Lote") = PLote
            Row("Fecha") = DateTime1.Value
            Row("Estado") = 1
            Row("Comentario") = ""
            DtCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM DevolucionMercaDetalle WHERE Devolucion = " & PDevolucion & ";", ConexionLote, DtDetalle) Then Me.Close() : Exit Sub

        'Lee orden compra detalle.-----------------------------------------------------------
        Dim IngresoMercaderiasCabeza As New DataTable
        If Not Tablas.Read("SELECT OrdenCompra FROM IngresoMercaderiasCabeza WHERE Lote = " & PLote & ";", ConexionLote, IngresoMercaderiasCabeza) Then Me.Close() : Exit Sub
        OrdenCompra = IngresoMercaderiasCabeza.Rows(0).Item("OrdenCompra")
        IngresoMercaderiasCabeza.Dispose()
        DtOrdenCompraDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM OrdenCompraDetalle WHERE orden = " & OrdenCompra & ";", Conexion, DtOrdenCompraDetalle) Then Me.Close() : Exit Sub
        '--------------------------------------------------------------------------------------

        'No procesa los traslados de depositos ni los reprocesos.
        Dim Sql As String = "SELECT * FROM Lotes WHERE Lote = " & PLote & " AND Lote = LoteOrigen AND Secuencia = SecuenciaOrigen AND Deposito = DepositoOrigen;"

        DtLotes = New DataTable
        If Not Tablas.Read(Sql, ConexionLote, DtLotes) Then Me.Close() : Exit Sub
        If DtLotes.Rows.Count = 0 Then
            MsgBox("Lotes No Existen En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        DtGrid.Clear()

        If PDevolucion = 0 Then
            For Each Row As DataRow In DtLotes.Rows
                Dim Row1 As DataRow = DtGrid.NewRow()
                Row1("Lote") = Row("Lote")
                Row1("Secuencia") = Row("Secuencia")
                Row1("Articulo") = Row("Articulo")
                Row1("Cantidad") = Row("Cantidad")
                Row1("Stock") = Row("Stock")
                Row1("Devolucion") = 0
                Row1("AGranel") = EsArticuloAGranel(Row1("Articulo"))
                DtGrid.Rows.Add(Row1)
            Next
        Else
            For Each Row As DataRow In DtDetalle.Rows
                Dim Row1 As DataRow = DtGrid.NewRow()
                Row1("Lote") = Row("Lote")
                Row1("Secuencia") = Row("Secuencia")
                Row1("Articulo") = HallaArticulo(Row("Lote"), Row("Secuencia"), ConexionLote)
                Row1("Cantidad") = 0
                Row1("Stock") = 0
                Row1("Devolucion") = Row("Cantidad")
                Row1("AGranel") = EsArticuloAGranel(Row1("Articulo"))
                DtGrid.Rows.Add(Row1)
            Next
        End If

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If PDevolucion = 0 Then
            Grid.Columns("Cantidad").HeaderText = "Cantidad Ini."
            Grid.Columns("Stock").HeaderText = "Stock"
            Panel1.Enabled = True
            Panel2.Enabled = True
        Else
            Grid.Columns("Cantidad").HeaderText = ""
            Grid.Columns("Stock").HeaderText = ""
            Panel1.Enabled = False
            Panel2.Enabled = False
        End If

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Devolucion")
        AddHandler Enlace.Format, AddressOf FormatDevolucion
        TextDevolucion.DataBindings.Clear()
        TextDevolucion.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Parse, AddressOf FormatTexto
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatDevolucion(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "#,###")

    End Sub
    Private Sub FormatTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Function LeerIngreso() As Boolean

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT * FROM IngresoMercaderiasCabeza WHERE Lote = " & PLote & ";", ConexionLote, Dt) Then Return False
        If Dt.Rows.Count = 0 Then
            MsgBox("Ingreso de Mercaderia NO EXISTE.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If

        ComboProveedor.SelectedValue = Dt.Rows(0).Item("Proveedor")
        ComboDeposito.SelectedValue = Dt.Rows(0).Item("Deposito")
        TextRemito.Text = Format(Dt.Rows(0).Item("Remito"), "0000-00000000")
        TextGuia.Text = Format(Dt.Rows(0).Item("Guia"), "#,###")
        DateTime2.Value = Format(Dt.Rows(0).Item("Fecha"), "dd/MM/yyyy")
        TipoOperacion = Dt.Rows(0).Item("TipoOperacion")

        Dt.Dispose()

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Lote As DataColumn = New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As DataColumn = New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Stock)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Devolucion As New DataColumn("Devolucion")
        Devolucion.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Devolucion)

        Dim AGranel As New DataColumn("AGranel")
        AGranel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(AGranel)

    End Sub
    Private Function GrabaDevolucion(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtOrdenCompraDetalleW As DataTable, ByVal DtLotesW As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                'Devolucion.
                If Not IsNothing(DtCabezaW.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaW.GetChanges, "DevolucionMercaCabeza", ConexionLote)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtDetalleW.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleW.GetChanges, "DevolucionMercaDetalle", ConexionLote)
                    If Resul <= 0 Then Return Resul
                End If

                'Lote.
                If Not IsNothing(DtLotesW.GetChanges) Then
                    Resul = GrabaTabla(DtLotesW.GetChanges, "Lotes", ConexionLote)
                    If Resul <= 0 Then Return Resul
                End If

                'Orden Compra.
                If Not IsNothing(DtOrdenCompraDetalleW.GetChanges) Then
                    Resul = GrabaTabla(DtOrdenCompraDetalleW.GetChanges, "OrdenCompraDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If

                'Asientos.
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionLote)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionLote)
                    If Resul <= 0 Then Return Resul
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
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
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Devolucion) FROM DevolucionMercaCabeza;", Miconexion)
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
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)

        Dim RowsBusqueda() As DataRow
        Dim ImporteTotal As Double = 0
        Dim Precio As Double
        Dim Centro As Integer

        If TipoOperacion <> 4 Then
            If Not HallaPrecioCentroTipoOperacion(TipoOperacion, Centro, Precio) Then Return False
        End If

        If Funcion = "A" Then
            For Each Row As DataRow In DtGrid.Rows
                If TipoOperacion = 4 Then
                    Centro = 0
                    Dim Operacion As Integer
                    If PAbierto Then
                        Operacion = 1
                    Else : Operacion = 2
                    End If
                    Dim Costeo = HallaCosteoLote(Operacion, Row("Lote"))
                    If Costeo = -1 Then Return False
                    If Not HallaPrecioYCentroCosteo(ComboProveedor.SelectedValue, Costeo, Centro, Precio) Then Return False
                End If
                Dim Fila As New ItemLotesParaAsientos
                Fila.Centro = Centro
                RowsBusqueda = DtLotes.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
                Fila.MontoNeto = Trunca(Precio * RowsBusqueda(0).Item("KilosXUnidad") * Row("Devolucion"))
                ImporteTotal = ImporteTotal + Fila.MontoNeto
                If TipoOperacion = 1 Then Fila.Clave = 301
                If TipoOperacion = 2 Then Fila.Clave = 300
                If TipoOperacion = 3 Then Fila.Clave = 303
                If TipoOperacion = 4 Then Fila.Clave = 302
                ListaLotesParaAsiento.Add(Fila)
            Next
        End If

        If Funcion = "B" Then
            For Each Row As DataRow In DtGrid.Rows
                If TipoOperacion = 4 Then
                    Centro = 0
                    Dim Operacion As Integer
                    If PAbierto Then
                        Operacion = 1
                    Else : Operacion = 2
                    End If
                    Dim Costeo = HallaCosteoLote(Operacion, Row("Lote"))
                    If Costeo = -1 Then Return False
                    If Not HallaPrecioYCentroCosteo(ComboProveedor.SelectedValue, Costeo, Centro, Precio) Then Return False
                End If
                Dim Fila As New ItemLotesParaAsientos
                Fila.Centro = Centro
                RowsBusqueda = DtLotes.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
                Fila.MontoNeto = -1 * Trunca(Precio * RowsBusqueda(0).Item("KilosXUnidad") * Row("Cantidad"))
                ImporteTotal = ImporteTotal + Fila.MontoNeto
                If TipoOperacion = 1 Then Fila.Clave = 301
                If TipoOperacion = 2 Then Fila.Clave = 300
                If TipoOperacion = 3 Then Fila.Clave = 303
                If TipoOperacion = 4 Then Fila.Clave = 302
                ListaLotesParaAsiento.Add(Fila)
            Next
        End If

        If Not Asiento(6052, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

        Return True

    End Function
    Private Function ActualizaOrdenCompra(ByVal Funcion As String, ByRef DtOrdenCompra As DataTable) As Boolean

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid.Rows
            If Not Row.RowState = DataRowState.Deleted Then
                RowsBusqueda = DtOrdenCompra.Select("Articulo = " & Row("Articulo"))
                If Funcion = "A" Then
                    RowsBusqueda(0).Item("Recibido") = RowsBusqueda(0).Item("Recibido") - Row("Devolucion")
                End If
                If Funcion = "B" Then
                    RowsBusqueda(0).Item("Recibido") = RowsBusqueda(0).Item("Recibido") + Row("Devolucion")
                End If
            End If
        Next

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
    Private Sub ActualizaLotes(ByVal Funcion As String, ByRef DtLotes As DataTable)

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtLotes.Select("Lote = " & Row("lote") & " AND Secuencia = " & Row("Secuencia"))
            If Funcion = "A" Then
                RowsBusqueda(0).Item("Stock") = CDec(RowsBusqueda(0).Item("Stock")) - CDec(Row("Devolucion"))
                RowsBusqueda(0).Item("Baja") = CDec(RowsBusqueda(0).Item("Baja")) + CDec(Row("Devolucion"))
            End If
            If Funcion = "B" Then
                RowsBusqueda(0).Item("Stock") = CDec(RowsBusqueda(0).Item("Stock")) + CDec(Row("Devolucion"))
                RowsBusqueda(0).Item("Baja") = CDec(RowsBusqueda(0).Item("Baja")) - CDec(Row("Devolucion"))
            End If
        Next

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTime2.Value, DateTime1.Value) < 0 Then
            MsgBox("Fecha Devolución no debe ser menor a Fecha de Ingreso. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        Dim Cantidad As Double = 0
        For i As Integer = 0 To Grid.RowCount - 2
            Cantidad = Cantidad + Grid.Rows(i).Cells("Devolucion").Value
        Next

        If Cantidad = 0 Then
            MsgBox("Falta Informar Devolución. Operación se CANCELA. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Dim RowsBusqueda() As DataRow
        For i As Integer = 0 To Grid.RowCount - 2
            If Grid.Rows(i).Cells("Devolucion").Value <> 0 Then
                RowsBusqueda = DtLotes.Select("Lote = " & Grid.Rows(i).Cells("Lote").Value & " AND Secuencia = " & Grid.Rows(i).Cells("Secuencia").Value)
                If RowsBusqueda(0).Item("Liquidado") <> 0 Then
                    MsgBox("Lote " & RowsBusqueda(0).Item("Lote") & "/" & Format(RowsBusqueda(0).Item("Secuencia"), "000") & " Esta Liquidado o Facturado. Operación se CANCELA.", MsgBoxStyle.Critical)
                    Return False
                End If
            End If
        Next

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Grid.Columns(e.ColumnIndex).ReadOnly = False Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.CellBeginEdit

        If Grid.Columns(e.ColumnIndex).Name = "Devolucion" Then
            If IsDBNull(Grid.Rows(e.RowIndex).Cells("Articulo").Value) Then e.Cancel = True
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Devolucion" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloEntero_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub SoloEntero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If e.KeyChar = "." Then e.KeyChar = ","
        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If CType(sender, TextBox).Text <> "" Then
            EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) And _
              Not IsNothing(Grid.Rows(e.RowIndex).Cells("Secuencia").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Devolucion" Or Grid.Columns(e.ColumnIndex).Name = "Stock" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                End If
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Dtgrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Devolucion") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If PDevolucion = 0 Then
                If e.Row("Stock") < e.ProposedValue Then
                    MsgBox("Devolución Supera Stock", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                    e.ProposedValue = e.Row("Devolucion")
                End If
            End If
        End If

    End Sub

End Class