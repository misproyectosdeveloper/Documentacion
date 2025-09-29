Imports System.Transactions
Public Class UnaDevolucionRecepcion
    Public PDevolucion As Integer
    Public PIngreso As Double
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    '
    Dim DtGrid As DataTable
    Dim DtStockInsumos As DataTable
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtIngresoDetalle As DataTable
    Dim DtOrdenDetalle As DataTable
    '
    Dim ConexionIngreso As String
    Dim FechaIngreso As Date
    Dim OrdenCompra As Double
    Dim Factura As Double
    Dim Rel As Boolean
    Private Sub UnaDevolucionInsumo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(12) Then PBloqueaFunciones = True

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        ComboProveedor.DataSource = ProveedoresDeInsumos()
        Dim Row As DataRow = ComboProveedor.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboProveedor.DataSource.rows.add(Row)
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0

        LlenaComboTablas(ComboDeposito, 20)
        ComboDeposito.SelectedValue = 0

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        LlenaCombosGrid()

        CreaDtGrid()

        ConexionIngreso = Conexion

        ArmaArchivos()

        GModificacionOk = False

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid_ColumnChanging)
        AddHandler DtGrid.ColumnChanged, New DataColumnChangeEventHandler(AddressOf Dtgrid_ColumnChanged)

    End Sub
    Private Sub UnaDevolucionInsumo_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        If PDevolucion = 0 Then
            If Grid.RowCount <> 0 Then
                Grid.CurrentCell = Grid.Rows(0).Cells("Devolucion")
            End If
        End If

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

        Dim DtCabezaW As New DataTable
        Dim DtDetalleW As New DataTable
        Dim DtIngresoDetalleW As New DataTable
        Dim DtStockInsumosW As New DataTable
        Dim DtOrdenDetalleW As New DataTable

        DtCabezaW = DtCabeza.Copy
        DtDetalleW = DtDetalle.Clone
        DtIngresoDetalleW = DtIngresoDetalle.Copy
        DtStockInsumosW = DtStockInsumos.Copy
        DtOrdenDetalleW = DtOrdenDetalle.Copy

        'Arma detalle de devolucion.
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("Devolucion") <> 0 Then
                Dim Row As DataRow = DtDetalleW.NewRow()
                Row("Devolucion") = 0
                Row("Articulo") = Row1("Articulo")
                Row("Cantidad") = Row1("Devolucion")
                DtDetalleW.Rows.Add(Row)
            End If
        Next
        'Actualiza detalle Recepcion.
        Dim RowsBusqueda() As DataRow
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("Devolucion") <> 0 Then
                RowsBusqueda = DtIngresoDetalleW.Select("Articulo = " & Row1("Articulo"))
                RowsBusqueda(0).Item("Devueltas") = RowsBusqueda(0).Item("Devueltas") + Row1("Devolucion")
            End If
        Next
        'Actualiza Orden de compra.
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("Devolucion") <> 0 Then
                RowsBusqueda = DtOrdenDetalleW.Select("Articulo = " & Row1("Articulo"))
                RowsBusqueda(0).Item("Recibido") = RowsBusqueda(0).Item("Recibido") - Row1("Devolucion")
            End If
        Next
        'Actualiza Stock de Insumos.
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("Devolucion") <> 0 Then
                RowsBusqueda = DtStockInsumosW.Select("Articulo = " & Row1("Articulo"))
                RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") - Row1("Devolucion")
            End If
        Next

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", ConexionIngreso, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", ConexionIngreso, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtDetalleW, DtOrdenDetalle, DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        Dim NumeroDevolucion As Integer = 0
        Dim i As Integer = 0
        Dim NumeroW As Double = 0
        Dim NumeroAsiento As Double = 0

        For i = 1 To 50
            NumeroDevolucion = UltimaNumeracion(ConexionIngreso)
            If NumeroDevolucion < 0 Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If

            DtCabezaW.Rows(0).Item("Devolucion") = NumeroDevolucion
            For Each Row1 As DataRow In DtDetalleW.Rows
                Row1("Devolucion") = NumeroDevolucion
            Next

            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionIngreso)
                If NumeroAsiento < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = NumeroDevolucion
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            NumeroW = ActualizaDevolucion(DtCabezaW, DtDetalleW, DtIngresoDetalleW, DtStockInsumosW, DtOrdenDetalleW, DtAsientoCabeza, DtAsientoDetalle)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -2 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -1 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = -2 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            MsgBox("Devolución Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PDevolucion = NumeroDevolucion
            GModificacionOk = True
            UnaDevolucionInsumo_Load(Nothing, Nothing)
        End If

        DtCabezaW.Dispose()
        DtDetalleW.Dispose()
        DtIngresoDetalleW.Dispose()
        DtStockInsumosW.Dispose()
        DtOrdenDetalleW.Dispose()
        DtAsientoCabeza.Dispose()
        DtAsientoDetalle.Dispose()

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


        If MsgBox("Devolución se Anulara. ¿Desea Anularla?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim DtCabezaW As New DataTable
        Dim DtDetalleW As New DataTable
        Dim DtIngresoDetalleW As New DataTable
        Dim DtStockInsumosW As New DataTable
        Dim DtOrdenDetalleW As New DataTable

        DtCabezaW = DtCabeza.Copy
        DtDetalleW = DtDetalle.Clone
        DtIngresoDetalleW = DtIngresoDetalle.Copy
        DtStockInsumosW = DtStockInsumos.Copy
        DtOrdenDetalleW = DtOrdenDetalle.Copy

        Dim RowsBusqueda() As DataRow
        'Actualiza detalle Recepcion.
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("Devolucion") <> 0 Then
                RowsBusqueda = DtIngresoDetalleW.Select("Articulo = " & Row1("Articulo"))
                RowsBusqueda(0).Item("Devueltas") = RowsBusqueda(0).Item("Devueltas") - Row1("Devolucion")
            End If
        Next
        'Actualiza Orden de compra.
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("Devolucion") <> 0 Then
                RowsBusqueda = DtOrdenDetalleW.Select("Articulo = " & Row1("Articulo"))
                RowsBusqueda(0).Item("Recibido") = RowsBusqueda(0).Item("Recibido") + Row1("Devolucion")
            End If
        Next
        'Actualiza Stock de Insumos.
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1("Devolucion") <> 0 Then
                RowsBusqueda = DtStockInsumosW.Select("Articulo = " & Row1("Articulo"))
                RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") + Row1("Devolucion")
            End If
        Next

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Anula Asiento.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(7004, PDevolucion, DtAsientoCabeza, ConexionIngreso) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                ArmaArchivos()
                Exit Sub
            End If
            If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3
        End If

        DtCabezaW.Rows(0).Item("Estado") = 3

        Dim Resul As Integer = ActualizaDevolucion(DtCabezaW, DtDetalleW, DtIngresoDetalleW, DtStockInsumosW, DtOrdenDetalleW, DtAsientoCabeza, DtAsientoDetalle)

        If Resul < 0 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Devolución Anulada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            UnaDevolucionInsumo_Load(Nothing, Nothing)
        End If

        DtCabezaW.Dispose()
        DtDetalleW.Dispose()
        DtIngresoDetalleW.Dispose()
        DtStockInsumosW.Dispose()
        DtOrdenDetalleW.Dispose()
        DtAsientoCabeza.Dispose()
        DtAsientoDetalle.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PDevolucion = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 7004
        ListaAsientos.PDocumentoB = PDevolucion

        ListaAsientos.Show()

    End Sub
    Private Sub ButtonDevolverTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDevolverTodos.Click

        For i As Integer = 0 To Grid.Rows.Count - 1
            Grid.Rows(i).Cells("Devolucion").Value = Grid.Rows(i).Cells("Cantidad").Value - Grid.Rows(i).Cells("Devueltas").Value
        Next

    End Sub
    Private Sub ArmaArchivos()

        DtGrid.Clear()

        DtCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM DevolucionInsumoCabeza WHERE Devolucion = " & PDevolucion & ";", ConexionIngreso, DtCabeza) Then Me.Close() : Exit Sub
        If PDevolucion <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Devolución No Existen En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PDevolucion = 0 Then
            Dim Row As DataRow = DtCabeza.NewRow
            Row("Devolucion") = 0
            Row("Ingreso") = PIngreso
            Row("Fecha") = DateTime1.Value
            Row("NotaCredito") = 0
            Row("Estado") = 1
            Row("Comentario") = ""
            DtCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM DevolucionInsumoDetalle WHERE Devolucion = " & PDevolucion & ";", ConexionIngreso, DtDetalle) Then Me.Close() : Exit Sub

        If PDevolucion <> 0 Then
            PIngreso = DtCabeza.Rows(0).Item("Ingreso")
        End If

        If Not LeerIngreso() Then Me.Close() : Exit Sub
        If Not LeerOrdenCompra(OrdenCompra) Then Me.Close() : Exit Sub

        Dim RowsBusqueda() As DataRow

        If PDevolucion = 0 Then
            For Each Row As DataRow In DtIngresoDetalle.Rows
                Dim Row1 As DataRow = DtGrid.NewRow()
                Row1("Articulo") = Row("Articulo")
                Row1("Cantidad") = Row("Cantidad")
                Row1("Devueltas") = Row("Devueltas")
                Row1("Devolucion") = 0
                Row1("Stock") = 0
                DtGrid.Rows.Add(Row1)
            Next
        Else
            For Each Row As DataRow In DtDetalle.Rows
                Dim Row1 As DataRow = DtGrid.NewRow()
                Row1("Articulo") = Row("Articulo")
                RowsBusqueda = DtIngresoDetalle.Select("Articulo = " & Row("Articulo"))
                Row1("Cantidad") = RowsBusqueda(0).Item("Cantidad")
                Row1("Devueltas") = 0
                Row1("Devolucion") = Row("Cantidad")
                Row1("Stock") = 0
                DtGrid.Rows.Add(Row1)
            Next
        End If

        DtStockInsumos = New DataTable
        For Each Row As DataRow In DtGrid.Rows
            If Not Tablas.Read("SELECT * FROM StockInsumos WHERE Articulo = " & Row("Articulo") & " AND Deposito = " & ComboDeposito.SelectedValue & " AND OrdenCompra = " & CDbl(TextOrdenCompra.Text) & ";", ConexionIngreso, DtStockInsumos) Then Me.Close() : Exit Sub
        Next
        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtStockInsumos.Select("Articulo = " & Row("Articulo"))
            If RowsBusqueda.Length <> 0 Then Row("Stock") = RowsBusqueda(0).Item("Stock")
        Next

        Try
            Grid.DataSource = bs
            bs.DataSource = DtGrid
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        CalculaTotales()

        If PDevolucion = 0 Then
            Panel1.Enabled = True
            Grid.ReadOnly = False
            Grid.Columns("Devueltas").Visible = True
            ButtonDevolverTodos.Enabled = True
        Else
            Panel1.Enabled = False
            Grid.ReadOnly = True
            Grid.Columns("Devueltas").Visible = False
            ButtonDevolverTodos.Enabled = False
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

        Enlace = New Binding("Text", MiEnlazador, "NotaCredito")
        AddHandler Enlace.Format, AddressOf FormatNotaCredito
        MaskedNotaCredito.DataBindings.Clear()
        MaskedNotaCredito.DataBindings.Add(Enlace)

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

        Numero.Value = Format(Numero.Value, "#")

    End Sub
    Private Sub FormatTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Sub FormatNotaCredito(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "000000000000")

    End Sub
    Private Function LeerIngreso() As Boolean

        Dim DtIngresoCabeza As New DataTable
        If Not Tablas.Read("SELECT * FROM IngresoInsumoCabeza WHERE Ingreso = " & PIngreso & ";", ConexionIngreso, DtIngresoCabeza) Then Return False
        If DtIngresoCabeza.Rows.Count = 0 Then
            MsgBox("Ingreso de Insumos NO EXISTE.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If

        DtIngresoDetalle = New DataTable
        Dim Sql As String = "SELECT * FROM IngresoInsumoDetalle WHERE Ingreso = " & PIngreso & ";"
        If Not Tablas.Read(Sql, ConexionIngreso, DtIngresoDetalle) Then Return False
        If DtIngresoDetalle.Rows.Count = 0 Then
            MsgBox("Ingreso de Insumos NO EXISTE.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If

        ComboProveedor.SelectedValue = DtIngresoCabeza.Rows(0).Item("Proveedor")
        ComboDeposito.SelectedValue = DtIngresoCabeza.Rows(0).Item("Deposito")
        TextRemito.Text = Format(DtIngresoCabeza.Rows(0).Item("Remito"), "0000-00000000")
        TextOrdenCompra.Text = Format(DtIngresoCabeza.Rows(0).Item("OrdenCompra"), "00000000")
        OrdenCompra = DtIngresoCabeza.Rows(0).Item("OrdenCompra")
        TextFechaIngreso.Text = Format(DtIngresoCabeza.Rows(0).Item("Fecha"), "dd/MM/yyyy")
        FechaIngreso = DtIngresoCabeza.Rows(0).Item("Fecha")
        Factura = DtIngresoCabeza.Rows(0).Item("Facturado")

        DtIngresoCabeza.Dispose()

        Return True

    End Function
    Private Function LeerOrdenCompra(ByVal OrdenCompra As Double) As Boolean

        DtOrdenDetalle = New DataTable

        If Not Tablas.Read("SELECT * FROM OrdenCompraDetalle WHERE Orden = " & OrdenCompra & ";", Conexion, DtOrdenDetalle) Then Return False

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Articulo As DataColumn = New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Devueltas As New DataColumn("Devueltas")
        Devueltas.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Devueltas)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cantidad)

        Dim Devolucion As New DataColumn("Devolucion")
        Devolucion.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Devolucion)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Stock)

    End Sub
    Private Function ActualizaDevolucion(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtIngresoDetalleW As DataTable, ByVal DtStockInsumosW As DataTable, ByVal DtOrdenDetalleW As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaW.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaW.GetChanges, "DevolucionInsumoCabeza", ConexionIngreso)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtDetalleW.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleW.GetChanges, "DevolucionInsumoDetalle", ConexionIngreso)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtIngresoDetalleW.GetChanges) Then
                    Resul = GrabaTabla(DtIngresoDetalleW.GetChanges, "IngresoInsumoDetalle", ConexionIngreso)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtStockInsumosW.GetChanges) Then
                    Resul = GrabaTabla(DtStockInsumosW.GetChanges, "StockInsumos", ConexionIngreso)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtOrdenDetalleW.GetChanges) Then
                    Resul = GrabaTabla(DtOrdenDetalleW.GetChanges, "OrdenCompraDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionIngreso)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionIngreso)
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
    Private Sub CalculaTotales()

        Dim RowsBusqueda() As DataRow
        Dim Total As Double

        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtOrdenDetalle.Select("Articulo = " & Row("Articulo"))
            Total = Total + CalculaNeto(Row("Devolucion"), RowsBusqueda(0).Item("Precio")) + Trunca(CalculaIva(Row("Devolucion"), RowsBusqueda(0).Item("Precio"), RowsBusqueda(0).Item("Iva")))
        Next

        TextTotal.Text = FormatNumber(Total, GDecimales)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Articulo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Insumos;")
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtDetalleW As DataTable, ByVal DtOrdenDetalleAux As DataTable, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)

        Dim RowsBusqueda() As DataRow
        Dim ImporteTotal As Double
        Dim Item As New ItemListaConceptosAsientos
        Dim Importe As Double = 0

        'Arma lista de Insumos, Utiliso listaRetenciones.
        For Each Row As DataRow In DtDetalleW.Rows
            Item = New ItemListaConceptosAsientos
            Item.Clave = Row("Articulo")
            RowsBusqueda = DtOrdenDetalleAux.Select("Articulo = " & Row("Articulo"))
            Importe = CalculaNeto(Row("Cantidad"), RowsBusqueda(0).Item("Precio"))
            Item.Importe = Importe
            Item.TipoIva = 0
            ListaRetenciones.Add(Item)
            ImporteTotal = ImporteTotal + Importe
        Next
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 202
        Item.Importe = Trunca(ImporteTotal)
        ListaConceptos.Add(Item)

        If Funcion = "A" Then
            If Not Asiento(7004, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False
        End If

        Return True

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Devolucion) FROM DevolucionInsumoCabeza;", Miconexion)
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

        If Factura <> 0 Then
            MsgBox("Ingreso Tiene Factura. Operción se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If DiferenciaDias(FechaIngreso, DateTime1.Value) < 0 Then
            MsgBox("Fecha Devolución no debe ser menor a Fecha de Ingreso. Operción se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Val(MaskedNotaCredito.Text) <> 0 And Not MaskedOK(MaskedNotaCredito) Then
            MsgBox("Numero Nota Credito Incorrecto. Operción se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedNotaCredito.Focus()
            Return False
        End If

        Dim Cantidad As Double = 0
        For i As Integer = 0 To Grid.RowCount - 1
            Cantidad = Cantidad + Grid.Rows(i).Cells("Devolucion").Value
        Next

        If Cantidad = 0 Then
            MsgBox("Falta Informar Devolución. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

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
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Devolucion" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloEntero_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub SoloEntero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If CType(sender, TextBox).Text <> "" Then
            If Not IsNumeric(CType(sender, TextBox).Text) Then
                CType(sender, TextBox).Text = ""
                CType(sender, TextBox).Focus()
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Devolucion" Or Grid.Columns(e.ColumnIndex).Name = "Devueltas" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 0)
                Else : e.Value = Format(0, "#")
                End If
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Dtgrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Devolucion")) Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If (e.Row("Cantidad") - e.Row("Devueltas")) < e.ProposedValue Then
                'no se que pasa se repite cuando trata el ultimo renglon del grid  MsgBox("Devolución Supera Cantidad Remito.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3, "Error")
                e.ProposedValue = e.Row("Devolucion")
                Exit Sub
            End If
            If e.Row("Stock") < e.ProposedValue Then
                ' MsgBox("Devolución Supera Stock de la Orden de Compra en el Deposito " & ComboDeposito.Text, MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3, "Error")
                e.ProposedValue = e.Row("Devolucion")
            End If
        End If

    End Sub
    Private Sub Dtgrid_ColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        CalculaTotales()

    End Sub

End Class