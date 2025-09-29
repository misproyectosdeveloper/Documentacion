Imports System.Transactions
Public Class UnaRecepcion
    Public PIngreso As Double
    Public PActualizacionOk As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Private DtIngresoCabeza As DataTable
    Private DtIngresoDetalle As DataTable
    Private DtOrdenCabeza As DataTable
    Private DtOrdenDetalle As DataTable
    Private DtStockInsumos As DataTable
    Private DtGrid As DataTable
    '
    Dim OrdenCompra As Double
    Dim ConexionIngreso As String
    Dim ConexionOrdenCompra As String
    Dim cb As ComboBox
    Dim RemitoAnt As Double
    Private Sub UnaRecepcion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(12) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False

        ComboProveedor.DataSource = ProveedoresDeInsumos()
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"

        LlenaComboTablas(ComboDeposito, 20)

        If PIngreso = 0 Then
            Opciones()
            If OrdenCompra = 0 Then Me.Close() : Exit Sub
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        LlenaCombosGrid()

        ConexionIngreso = Conexion
        ConexionOrdenCompra = Conexion

        PActualizacionOk = False

        ArmaArchivos()

    End Sub
    Private Sub UnIngresoMercaderia_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        MaskedRemito.Focus()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PIngreso <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        If DtIngresoCabeza.Rows(0).Item("Facturado") <> 0 Then
            MsgBox("ERROR, Remito Ya fue Facturado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        ActualizaArchivos()

        If IsNothing(DtIngresoCabeza.GetChanges) And IsNothing(DtIngresoDetalle.GetChanges) Then
            MsgBox("No hay Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtIngresoDetalle, DtOrdenDetalle, DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        HacerAlta(DtAsientoCabeza, DtAsientoDetalle)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PIngreso = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("ERROR, Ingreso Ya esta Anulado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtIngresoCabeza.Rows(0).Item("Facturado") <> 0 Then
            MsgBox("ERROR, Remito Ya fue Facturado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not IsNothing(DtIngresoCabeza.GetChanges) Then
            MsgBox("Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtIngresoDetalle.Rows
            RowsBusqueda = DtStockInsumos.Select("Articulo = " & Row("Articulo"))
            If (RowsBusqueda(0).Item("Stock") - Row("Cantidad")) < 0 Then
                MsgBox("ERROR, No se puede Anular el Ingreso pues YA FUE Consumido. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        Next

        If MsgBox("El Ingreso se Anulara. ¿Desea Anularlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        For Each Row As DataRow In DtIngresoDetalle.Rows
            RowsBusqueda = DtStockInsumos.Select("Articulo = " & Row("Articulo"))
            RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") - Row("Cantidad")
            '
            RowsBusqueda = DtOrdenDetalle.Select("Articulo = " & Row("Articulo"))
            RowsBusqueda(0).Item("Recibido") = RowsBusqueda(0).Item("Recibido") - Row("Cantidad")
        Next

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(7003, DtIngresoCabeza.Rows(0).Item("Ingreso"), DtAsientoCabeza, ConexionIngreso) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                ArmaArchivos()
                Exit Sub
            End If
            If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3
        End If

        DtIngresoCabeza.Rows(0).Item("Estado") = 3

        Dim Resul As Double = ActualizaIngreso(DtAsientoCabeza, DtAsientoDetalle)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If Resul = -1 Or Resul = -2 Then
            MsgBox("ERROR, Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Ingreso fue Anulado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PActualizacionOk = True
        End If

        ArmaArchivos()

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PIngreso = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 7003
        ListaAsientos.PDocumentoB = PIngreso

        ListaAsientos.Show()

    End Sub
    Private Sub PictureAlmanaqueRemito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueRemito.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaRemito.Text = ""
        Else : TextFechaRemito.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PIngreso = 0
        UnaRecepcion_Load(Nothing, Nothing)

    End Sub
    Private Sub ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Row As DataRow

        CreaDtGrid()

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)

        DtIngresoCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM IngresoInsumoCabeza WHERE Ingreso = " & PIngreso & ";", ConexionIngreso, DtIngresoCabeza) Then Me.Close() : Exit Sub
        If PIngreso <> 0 And DtIngresoCabeza.Rows.Count = 0 Then
            MsgBox("Ingreso No Existe En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If DtIngresoCabeza.Rows.Count = 0 Then
            Row = DtIngresoCabeza.NewRow
            Row("Ingreso") = 0
            Row("Proveedor") = ComboProveedor.SelectedValue
            Row("Remito") = 0
            Row("OrdenCompra") = OrdenCompra
            Row("Deposito") = ComboDeposito.SelectedValue
            Row("Fecha") = Now
            Row("FechaRemito") = "01/01/1800"
            Row("Estado") = 1
            Row("Facturado") = 0
            Row("Comentario") = ""
            DtIngresoCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        If PIngreso <> 0 Then
            OrdenCompra = DtIngresoCabeza.Rows(0).Item("OrdenCompra")
        End If

        DtIngresoDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM IngresoInsumoDetalle WHERE Ingreso = " & PIngreso & ";", ConexionIngreso, DtIngresoDetalle) Then Me.Close() : Exit Sub

        DtStockInsumos = New DataTable
        If Not Tablas.Read("SELECT * FROM StockInsumos WHERE OrdenCompra = " & OrdenCompra & " AND Deposito = " & ComboDeposito.SelectedValue & ";", ConexionIngreso, DtStockInsumos) Then Me.Close() : Exit Sub

        DtOrdenCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM OrdenCompraCabeza WHERE Orden = " & OrdenCompra & ";", ConexionOrdenCompra, DtOrdenCabeza) Then Me.Close() : Exit Sub
        If DtOrdenCabeza.Rows.Count = 0 Then
            MsgBox("ERROR, Orden de Compra No Encontrada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PIngreso = 0 And DtOrdenCabeza.Rows(0).Item("Estado") = 3 Then
            MsgBox("Orden de Compra Esta Anulada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        DtOrdenDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM OrdenCompraDetalle WHERE Orden = " & OrdenCompra & ";", ConexionOrdenCompra, DtOrdenDetalle) Then Me.Close() : Exit Sub
        If DtOrdenDetalle.Rows.Count = 0 Then
            MsgBox("ERROR, Detalle Orden de Compra No Encontrada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
            Exit Sub
        End If

        Dim RowsBusqueda() As DataRow

        If PIngreso = 0 Then
            For Each Row In DtOrdenDetalle.Rows
                Dim Row1 As DataRow = DtGrid.NewRow
                Row1("Articulo") = Row("Articulo")
                Row1("CantidadOrden") = Row("Cantidad")
                Row1("Cantidad") = 0
                Row1("Saldo") = Row("Cantidad") - Row("Recibido")
                If Row1("Saldo") <> 0 Then DtGrid.Rows.Add(Row1)
            Next
        Else
            For Each Row In DtIngresoDetalle.Rows
                Dim Row1 As DataRow = DtGrid.NewRow
                RowsBusqueda = DtOrdenDetalle.Select("Articulo = " & Row("Articulo"))
                Row1("Articulo") = Row("Articulo")
                Row1("CantidadOrden") = RowsBusqueda(0).Item("Cantidad")
                Row1("Saldo") = RowsBusqueda(0).Item("Cantidad") - RowsBusqueda(0).Item("Recibido")
                Row1("Cantidad") = Row("Cantidad")
                Row1("Devueltas") = Row("Devueltas")
                DtGrid.Rows.Add(Row1)
            Next
        End If

        Grid.DataSource = DtGrid

        RemitoAnt = DtIngresoCabeza.Rows(0).Item("Remito")

        If PIngreso = 0 Then
            Grid.ReadOnly = False
            Panel1.Enabled = True
            Grid.Columns("Devueltas").Visible = False
        Else
            Grid.ReadOnly = True
            Grid.Columns("Sel").Visible = False
            Panel1.Enabled = False
            Grid.Columns("Devueltas").Visible = True
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtIngresoCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Ingreso")
        AddHandler Enlace.Format, AddressOf FormatOrden
        MaskedIngreso.DataBindings.Clear()
        MaskedIngreso.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Remito")
        AddHandler Enlace.Format, AddressOf FormatRemito
        MaskedRemito.DataBindings.Clear()
        MaskedRemito.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "OrdenCompra")
        AddHandler Enlace.Format, AddressOf FormatOrden
        MaskedOrden.DataBindings.Clear()
        MaskedOrden.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Facturado")
        AddHandler Enlace.Format, AddressOf FormatOrden
        MaskedFactura.DataBindings.Clear()
        MaskedFactura.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Proveedor")
        ComboProveedor.DataBindings.Clear()
        ComboProveedor.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Deposito")
        ComboDeposito.DataBindings.Clear()
        ComboDeposito.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Dim Row As DataRowView = MiEnlazador.Current

        If Row("FechaRemito") = "01/01/1800" Then
            TextFechaRemito.Text = ""
        Else : TextFechaRemito.Text = Format(Row("FechaRemito"), "dd/MM/yyyy")
        End If

    End Sub
    Private Sub FormatRemito(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "0000-00000000")

    End Sub
    Private Sub FormatOrden(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "00000000")

    End Sub
    Private Sub ParseTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim CantidadOrden As New DataColumn("CantidadOrden")
        CantidadOrden.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CantidadOrden)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Saldo)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Devueltas As New DataColumn("Devueltas")
        Devueltas.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Devueltas)

    End Sub
    Private Sub ActualizaArchivos()

        Dim RowsBusqueda() As DataRow

        If Format(DtIngresoCabeza.Rows(0).Item("FechaRemito"), "dd/MM/yyyy") <> CDate(TextFechaRemito.Text) Then DtIngresoCabeza.Rows(0).Item("FechaRemito") = CDate(TextFechaRemito.Text)

        'Actualiza detalle Ingreso.
        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Cantidad").Value <> 0 Then
                Dim Row1 As DataRow = DtIngresoDetalle.NewRow
                Row1("Ingreso") = 0
                Row1("Articulo") = Row.Cells("Articulo").Value
                Row1("Cantidad") = Row.Cells("Cantidad").Value
                Row1("Devueltas") = 0
                DtIngresoDetalle.Rows.Add(Row1)
                RowsBusqueda = DtStockInsumos.Select("Articulo = " & Row.Cells("Articulo").Value)
                If RowsBusqueda.Length <> 0 Then
                    RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") + Row.Cells("Cantidad").Value
                Else
                    Dim Row2 As DataRow = DtStockInsumos.NewRow
                    Row2("Articulo") = Row.Cells("Articulo").Value
                    Row2("OrdenCompra") = OrdenCompra
                    Row2("Deposito") = ComboDeposito.SelectedValue
                    Row2("Stock") = Row.Cells("Cantidad").Value
                    DtStockInsumos.Rows.Add(Row2)
                End If
                RowsBusqueda = DtOrdenDetalle.Select("Articulo = " & Row.Cells("Articulo").Value)
                RowsBusqueda(0).Item("Recibido") = RowsBusqueda(0).Item("Recibido") + Row.Cells("Cantidad").Value
            End If
        Next

    End Sub
    Private Sub HacerAlta(ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable)

        Dim NumeroW As Double = 0
        Dim Resul As Double = 0
        Dim NumeroAsiento As Double = 0
        Dim NumeroAsientoRel As Double = 0

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Ingreso.
            NumeroW = UltimaNumeracion(ConexionIngreso)
            If NumeroW < 0 Then
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            '
            DtIngresoCabeza.Rows(0).Item("Ingreso") = NumeroW
            For Each Row1 As DataRow In DtIngresoDetalle.Rows
                Row1("Ingreso") = NumeroW
            Next
            '
            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionIngreso)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = NumeroW
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            Resul = ActualizaIngreso(DtAsientoCabeza, DtAsientoDetalle)
            '
            If Resul >= 0 Or Resul = -2 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -1 Or Resul = -2 Then
            MsgBox("ERROR, base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PActualizacionOk = True
            PIngreso = NumeroW
        End If

        ArmaArchivos()

    End Sub
    Private Function ActualizaIngreso(ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtIngresoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtIngresoCabeza.GetChanges, "IngresoInsumoCabeza", ConexionIngreso)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtIngresoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtIngresoDetalle.GetChanges, "IngresoInsumoDetalle", ConexionIngreso)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtStockInsumos.GetChanges) Then
                    Resul = GrabaTabla(DtStockInsumos.GetChanges, "StockInsumos", ConexionIngreso)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtOrdenDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtOrdenDetalle.GetChanges, "OrdenCompraDetalle", ConexionOrdenCompra)
                    If Resul <= 0 Then Return Resul
                End If
                '
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
            Return -2
        Finally
        End Try

    End Function
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
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Ingreso) FROM IngresoInsumoCabeza;", Miconexion)
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
    Private Function HallaProveedorRemitoInsumo(ByVal Proveedor As Integer, ByVal Remito As Double, ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Ingreso) FROM IngresoInsumoCabeza WHERE Estado = 1 AND Proveedor = " & Proveedor & " AND Remito = " & Remito & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Private Sub Opciones()

        OpcionIngreso.PSoloInsumos = True
        OpcionIngreso.PConOrdenCompra = True
        OpcionIngreso.ShowDialog()
        If OpcionIngreso.PRegresar Then OrdenCompra = 0 : OpcionIngreso.Dispose() : Exit Sub
        ComboProveedor.SelectedValue = OpcionIngreso.ComboEmisor.SelectedValue
        ComboDeposito.SelectedValue = OpcionIngreso.ComboDeposito.SelectedValue
        OrdenCompra = OpcionIngreso.POrdenCompra
        OpcionIngreso.Dispose()

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtIngresoDetalle As DataTable, ByVal DtOrdenDetalleAux As DataTable, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)

        Dim RowsBusqueda() As DataRow
        Dim Precio As Double = 0
        Dim ImporteTotal As Double
        Dim Importe As Double = 0

        'Arma lista de Insumos, Utiliso listaRetenciones.
        Dim Item As New ItemListaConceptosAsientos

        For Each Row As DataRow In DtIngresoDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row("Cantidad") <> 0 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("Articulo")
                    RowsBusqueda = DtOrdenDetalleAux.Select("Articulo = " & Row("Articulo"))
                    Importe = CalculaNeto(Row("Cantidad"), RowsBusqueda(0).Item("Precio"))
                    Item.Importe = Importe
                    Item.TipoIva = 0
                    ListaRetenciones.Add(Item)
                    ImporteTotal = ImporteTotal + Importe
                End If
            End If
        Next
        ' 
        Item = New ItemListaConceptosAsientos
        Item.Clave = 202
        Item.Importe = Trunca(ImporteTotal)
        ListaConceptos.Add(Item)

        If Funcion = "A" Then
            If Not Asiento(7003, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False
        End If

        If DtAsientoDetalle.Rows.Count = 0 Then
            '  MsgBox("No se pudo Generar Asiento. Operación se CANCELA.")
            ' Return False
        End If

        Return True

    End Function
    Private Function Valida() As Boolean

        If ComboProveedor.SelectedValue = 0 Then
            MsgBox("Falta Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboProveedor.Focus()
            Return False
        End If
        If Val(MaskedRemito.Text) = 0 Or Not MaskedOK(MaskedRemito) Then
            MsgBox("Numero Remito Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If PIngreso = 0 Then
            If HallaProveedorRemitoInsumo(ComboProveedor.SelectedValue, Val(MaskedRemito.Text), ConexionIngreso) <> 0 Then
                MsgBox("Remito ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If
        If PIngreso <> 0 And RemitoAnt <> Val(MaskedRemito.Text) Then
            If HallaProveedorRemitoInsumo(ComboProveedor.SelectedValue, Val(MaskedRemito.Text), ConexionIngreso) <> 0 Then
                MsgBox("Remito ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If
        If Val(MaskedOrden.Text) = 0 Then
            MsgBox("Numero Orden Compra Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Return False
        End If
        If TextFechaRemito.Text = "" Then
            MsgBox("Falta Informar Fecha Remito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaRemito.Focus()
            Return False
        End If
        If Not ConsisteFecha(TextFechaRemito.Text) Then
            MsgBox("Fecha Remito Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaRemito.Focus()
            Return False
        End If
        If DiferenciaDias(TextFechaRemito.Text, DateTime1.Value) < 0 Then
            MsgBox("Fecha Remito Mayor a la Fecha Ingreso.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaRemito.Focus()
            Return False
        End If

        If Grid.Rows.Count = 0 Then
            MsgBox("Falta Informar Articulo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Dim Cantidad As Integer = 0
        For Each Row As DataGridViewRow In Grid.Rows
            Cantidad = Cantidad + Row.Cells("Cantidad").Value
        Next
        If Cantidad = 0 Then
            MsgBox("Debe elegir un Articulo de la Orden de Compra.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
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

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
            End If
        End If

    End Sub
    Private Sub Grid_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellContentClick

        If Grid.Columns(e.ColumnIndex).Name = "Sel" Then
            Dim Check As New DataGridViewCheckBoxCell
            Check = Grid.Rows(e.RowIndex).Cells("Sel")
            Check.Value = Not Check.Value
            If Not Check.Value Then
                Grid.Rows(e.RowIndex).Cells("Cantidad").Value = 0
            Else : Grid.Rows(e.RowIndex).Cells("Cantidad").Value = Grid.Rows(e.RowIndex).Cells("Cantidad").Value + Grid.Rows(e.RowIndex).Cells("Saldo").Value
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

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPressConDecimal
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged2Decimales
        End If


    End Sub
    Private Sub ValidaKey_KeyPressConDecimal(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If e.KeyChar = "." Then e.KeyChar = ","
        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ValidaChanged_TextChanged2Decimales(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "CantidadOrden" Or Grid.Columns(e.ColumnIndex).Name = "Saldo" Or Grid.Columns(e.ColumnIndex).Name = "Devueltas" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = FormatNumber(e.Value, 2)
                End If
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Cantidad")) Then
            If Not IsDBNull(e.Row("Cantidad")) Then
                If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
                If e.ProposedValue > e.Row("Saldo") + e.Row("Cantidad") Then
                    MsgBox("Nueva Cantidad SUPERA Saldo del la Orden de Compra.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                    e.ProposedValue = e.Row("Cantidad")
                    Exit Sub
                End If
                e.Row("Saldo") = e.Row("Saldo") + e.Row("Cantidad") - CDec(e.ProposedValue)
            End If
        End If

    End Sub

    
End Class