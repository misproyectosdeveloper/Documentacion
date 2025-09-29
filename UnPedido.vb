Imports System.Transactions
Public Class UnPedido
    Public PPedido As Integer
    Public PBloqueaFunciones As Boolean
    Public PSinSucursal As Boolean
    '
    Dim DtCabeza As New DataTable
    Dim DtDetalle As New DataTable
    Dim DtArticulos As DataTable
    ' 
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim cb As ComboBox
    Dim Cliente As Integer
    Dim FechaEntregaDesde As Date
    Dim FechaEntregaHasta As Date
    Dim Lista As Integer
    Dim Sucursal As Integer
    Dim PedidoCliente As String
    Dim Cerrado As Boolean
    Dim Zona As Integer = 0
    Dim Vendedor As Integer
    Dim FinalEnListaW As Boolean
    Dim PorUnidadEnListaW As Boolean
    Dim EsImportacionKrikos As Boolean
    Dim SucursalPedida As Integer
    'Para impresora
    Dim Ancho As Integer
    Dim Alto As Integer
    Dim mRow As Integer = 0
    Dim newpage As Boolean = True
    Dim CantidadTotal As Decimal = 0
    Dim WithEvents pd As New System.Drawing.Printing.PrintDocument()
    Private Sub UnPedido_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(1001) Then PBloqueaFunciones = True

        Me.Top = 5

        Me.Text = Me.Text & "        " & GNombreEmpresa

        Grid.AutoGenerateColumns = False

        LlenaCombo(ComboCliente, "", "Clientes")
        ComboCliente.SelectedValue = 0

        If PPedido = 0 Then
            If Not EsImportacionKrikos Then Opciones()
            If Cliente = 0 Then Me.Close() : Exit Sub
            If EsImportacionKrikos Then
                ButtonImportacionKrikos.Visible = True
                ButtonImportar.Enabled = False
            Else
                ButtonImportacionKrikos.Visible = False
                ButtonImportar.Enabled = True
                ButtonEliminar.Enabled = True
                Grid.ReadOnly = False
            End If
        Else
            ButtonImportacionKrikos.Visible = False
        End If

        If GTipoIva = 2 Then    'Cuando la empresa que factura es EXENTA.
            PanelIva.Enabled = False
            RadioSinIva.Checked = True
        End If

        ArmaArchivos()

        ComboTipoPrecio.DisplayMember = "Text"
        ComboTipoPrecio.ValueMember = "Value"
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(Integer))
        tb.Rows.Add("", 0)
        tb.Rows.Add("Uni.", 1)
        tb.Rows.Add("Kgs.", 2)
        ComboTipoPrecio.DataSource = tb

    End Sub
    Private Sub UnPedido_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

        Entrada.Show()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        bs.EndEdit()
        MiEnlazador.EndEdit()

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtDetalle.HasErrors Then
            MsgBox("Hay Errores. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If IsNothing(DtDetalle.GetChanges) And IsNothing(DtCabeza.GetChanges) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No Hay Cambios. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        GModificacionOk = False

        If PPedido = 0 Then
            HacerAlta()
        Else
            HacerModificacion()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PPedido = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        For Each Row As DataRow In DtDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row("Entregada") <> 0 Then
                    MsgBox("Pedido Tiene Entregas. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                End If
            End If
        Next

        If MsgBox("Pedido se Borrara del Sistema. ¿Desea Borrarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        PedidoCliente = TextPedidoCliente.Text

        DtCabeza.Rows(0).Delete()
        For Each Row As DataRow In DtDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Row.Delete()
            End If
        Next

        Dim Resul As Double

        Resul = ActualizaArchivos()

        If Resul = -1 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = -2 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArchivoCambiaDirectorio(PedidoCliente, True)
            Me.Close()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonNuevaIgualCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevaIgualCliente.Click

        EsImportacionKrikos = False
        PPedido = 0
        UnPedido_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportar.Click

        If PPedido <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtDetalle.Rows.Count <> 0 Then
            If MsgBox("Existe Articulos. Desa Continuar igualmente (Los Articulos se Mantendrán)?.", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Pedido As Integer = 0
        Dim ConCantidades As Boolean
        Dim RowsBusqueda() As DataRow
        Dim RowsBusqueda1() As DataRow

        OpcionPedidos.PEsImportacion = True
        OpcionPedidos.ShowDialog()
        If OpcionPedidos.PRegresar Then OpcionPedidos.Dispose() : Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        Pedido = OpcionPedidos.PPedido
        ConCantidades = OpcionPedidos.PConCantidades
        OpcionPedidos.Dispose()

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Articulo,Cantidad FROM PedidosDetalle AS D INNER JOIN Articulos AS A ON D.Articulo = A.Clave WHERE Pedido = " & Pedido & " ORDER BY A.Nombre;", Conexion, Dt) Then Me.Close() : Exit Sub
        If Dt.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Pedido No Existe o No Tiene Articulos.")
            Dt.Dispose()
            Exit Sub
        End If

        For Each Row As DataRow In Dt.Rows
            RowsBusqueda = DtDetalle.Select("Articulo = " & Row("Articulo"))
            If RowsBusqueda.Length = 0 Then
                RowsBusqueda1 = Articulo.DataSource.Select("Clave = " & Row("Articulo"))
                If RowsBusqueda1.Length <> 0 Then
                    Dim Row1 As DataRow = DtDetalle.NewRow
                    Row1("Articulo") = Row("Articulo")
                    If ConCantidades Then
                        Row1("Cantidad") = Row("Cantidad")
                    End If
                    DtDetalle.Rows.Add(Row1)
                End If
            End If
        Next

        Grid.Refresh()
        Dt.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        If bs.Current Is Nothing Then
            MsgBox("No hay registro actual para eliminar.", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Dim Row As DataRowView = bs.Current
        If Row("Entregada") <> 0 Then
            MsgBox("El Articulo Tiene Entregas.", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        bs.RemoveCurrent()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        '    Grid.Columns("Armador").Visible = True   'Solo titulos.
        '     Grid.Columns("Fletero").Visible = True

        If ComboSucursal.Text = "" Then
            GridAExcel(Grid, Date.Now, "Pedido del Cliente " & ComboCliente.Text & "   Fecha Entrega " & FechaDesde.Value, "")
        Else
            GridAExcel(Grid, Date.Now, "Pedido del Cliente " & ComboCliente.Text & "  Sucursal " & ComboSucursal.Text & "   Fecha Entrega " & FechaDesde.Value, "")
        End If

        '   Grid.Columns("Armador").Visible = False
        '   Grid.Columns("Fletero").Visible = False

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonExportarExcelCorto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcelCorto.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Grid.Columns("Entregada").Visible = False
        Grid.Columns("Saldo").Visible = False
        Grid.Columns("Precio").Visible = False
        Grid.Columns("TipoPrecio").Visible = False

        If ComboSucursal.Text = "" Then
            GridAExcel(Grid, "Pedido del Cliente " & ComboCliente.Text, "Fecha Entrega " & FechaDesde.Value, "")
        Else
            GridAExcel(Grid, "Pedido del Cliente " & ComboCliente.Text, "Sucursal " & ComboSucursal.Text, "Fecha Entrega " & FechaDesde.Value)
        End If

        Grid.Columns("Entregada").Visible = True
        Grid.Columns("Saldo").Visible = True
        Grid.Columns("Precio").Visible = True

        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        pd.DefaultPageSettings.Landscape = False
        UnSeteoImpresora.SeteaImpresion(pd)
        CantidadTotal = 0
        mRow = 0
        newpage = True

        pd.Print()
        pd.Dispose()

    End Sub
    Private Sub PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles pd.PrintPage

        ImpresionPedido(e, DtCabeza.Rows(0), DtDetalle, mRow, newpage, CantidadTotal)

    End Sub
    Private Sub ButtonUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUltimo.Click

        bs.MoveLast()

    End Sub
    Private Sub ButtonAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnterior.Click

        bs.MovePrevious()

    End Sub
    Private Sub ButtonPosterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPosterior.Click

        bs.MoveNext()

    End Sub
    Private Sub ButtonPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPrimero.Click

        bs.MoveFirst()

    End Sub
    Private Sub ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM PedidosCabeza WHERE Pedido = " & PPedido & ";", Conexion, DtCabeza) Then Me.Close() : Exit Sub
        If PPedido > 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Pedido No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PPedido > 0 Then
            Sucursal = DtCabeza.Rows(0).Item("Sucursal")
            Cliente = DtCabeza.Rows(0).Item("Cliente")
            Cerrado = DtCabeza.Rows(0).Item("Cerrado")
            FechaEntregaDesde = DtCabeza.Rows(0).Item("FechaEntregaDesde")
            FechaEntregaHasta = DtCabeza.Rows(0).Item("FechaEntregaHasta")
        End If

        'Ve si hay lista de precios.
        PorUnidadEnListaW = False : FinalEnListaW = False
        Lista = HallaListaPrecios(Cliente, Sucursal, FechaEntregaDesde, PorUnidadEnListaW, FinalEnListaW)
        If Lista = -1 Then
            MsgBox("Falta Lista de Precios Para la Fecha del Pedido. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
            Exit Sub
        End If
        If Lista > 0 Then
            If Not ConsisteFechaDesdeHasta(Lista, FechaEntregaDesde, FechaEntregaHasta) Then Me.Close() : Exit Sub
        End If
        '----------------------------------

        If PPedido = 0 And DtCabeza.Rows.Count = 0 Then
            Dim Row As DataRow = DtCabeza.NewRow
            Row("Pedido") = PPedido
            Row("Sucursal") = Sucursal
            Row("Cliente") = Cliente
            Row("Fecha") = Now
            Row("FechaEntregaDesde") = FechaEntregaDesde
            Row("FechaEntregaHasta") = FechaEntregaHasta
            Row("PorUnidad") = PorUnidadEnListaW
            Row("Final") = FinalEnListaW
            Row("PedidoCliente") = PedidoCliente
            Row("Cerrado") = False
            DtCabeza.Rows.Add(Row)
        End If

        If Lista > 0 Then
            Grid.Columns("Precio").ReadOnly = True
            Grid.Columns("TipoPrecio").ReadOnly = True
            Label9.Visible = True
            PanelPrecio.Enabled = False
        Else
            Grid.Columns("Precio").ReadOnly = False
            Grid.Columns("TipoPrecio").ReadOnly = False
            Label9.Visible = False
            PanelPrecio.Enabled = True
        End If

        Dim Row1 As DataRow

        ComboSucursal.DataSource = Nothing
        ComboSucursal.DataSource = Tablas.Leer("SELECT Nombre,Clave FROM SucursalesClientes WHERE Cliente = " & DtCabeza.Rows(0).Item("Cliente") & ";")
        Row1 = ComboSucursal.DataSource.NewRow
        Row1("Nombre") = ""
        Row1("Clave") = 0
        ComboSucursal.DataSource.Rows.Add(Row1)
        ComboSucursal.DisplayMember = "Nombre"
        ComboSucursal.ValueMember = "Clave"

        MuestraCabeza()

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT L.* FROM PedidosDetalle AS L INNER JOIN Articulos AS A ON A.Clave = L.Articulo WHERE L.pedido = " & PPedido & "ORDER BY A.Nombre;", Conexion, DtDetalle) Then Me.Close() : Exit Sub

        LlenaCombosGrid()

        Grid.DataSource = bs
        bs.DataSource = DtDetalle
        Grid.EndEdit()  'ponerlo para que no cancele ?????.

        'Esto es para que en los articulos ya grabados si lo quiere cambiar solo lo haga con 'Borrar Linea' y dar de alta.
        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Articulo").Value <> 0 Then
                Row.Cells("Articulo").ReadOnly = True
            End If
        Next

        Dim Entregada As Decimal = 0
        For Each Row As DataGridViewRow In Grid.Rows
            Row.Cells("Saldo").Value = Row.Cells("Cantidad").Value - Row.Cells("Entregada").Value
            Entregada = Entregada + Row.Cells("Entregada").Value
        Next

        If Entregada = 0 Then
            ComboSucursal.Enabled = True
        End If

        If DtCabeza.Rows(0).Item("Cerrado") Then
            Grid.ReadOnly = True
            PanelPrecio.Enabled = False
            ButtonEliminar.Enabled = False
        End If

        AddHandler DtDetalle.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtDetalle_ColumnChanging)
        AddHandler DtDetalle.RowChanging, New DataRowChangeEventHandler(AddressOf DtDetalle_RowChanging)
        AddHandler DtDetalle.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtDetalle_NewRow)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub HacerAlta()

        Dim Resul As Double
        Dim UltimoNumero As Integer

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            UltimoNumero = UltimaNumeracion(Conexion)
            If UltimoNumero <= 0 Then
                MsgBox("Error Base De Datos al Leer Tabla: PedidosCabeza. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If

            DtCabeza.Rows(0).Item("Pedido") = UltimoNumero
            For Each Row As DataRow In DtDetalle.Rows
                Row("Pedido") = UltimoNumero
            Next

            PedidoCliente = TextPedidoCliente.Text

            Resul = ActualizaArchivos()

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
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Dispose() : Me.Close() : Exit Sub
        End If
        If Resul > 0 Then
            GModificacionOk = True
            PPedido = UltimoNumero
            Select Case ArreglaArchivo()
                Case 10
                    MsgBox("El Pedido NO Pudo Ser Grabado! Reintente 'Aceptar Cambios'. Si persiste el error, Ingrese a 'Nuevo Remito' en Mmenu 'Comercial'.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ATENCION!")
                    Exit Sub
                Case 0
                    MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3, "Alta!")
                    If EsImportacionKrikos Then BorraArchivoW(PedidoCliente)
                Case -1
                    MsgBox("EL PEDIDO 0(cero) NO PUDO SER BORRADO.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!")
                    PPedido = 0
            End Select
            ArmaArchivos()
        End If

    End Sub
    Private Sub HacerModificacion()

        Dim Resul As Double

        Resul = ActualizaArchivos()

        If Resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = -2 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

    End Sub
    Private Function ActualizaArchivos() As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)


                If Not IsNothing(DtCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtCabeza.GetChanges, "PedidosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtDetalle.GetChanges, "PedidosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Pedido")
        AddHandler Enlace.Format, AddressOf FormatEnteros
        TextPedido.DataBindings.Clear()
        TextPedido.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Cliente")
        ComboCliente.DataBindings.Clear()
        ComboCliente.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Sucursal")
        ComboSucursal.DataBindings.Clear()
        ComboSucursal.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaEntregaDesde")
        FechaDesde.DataBindings.Clear()
        FechaDesde.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaEntregaHasta")
        FechaHasta.DataBindings.Clear()
        FechaHasta.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Final")
        RadioFinal.DataBindings.Clear()
        RadioFinal.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "PedidoCliente")
        TextPedidoCliente.DataBindings.Clear()
        TextPedidoCliente.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Cerrado")
        CheckCerrado.DataBindings.Clear()
        CheckCerrado.DataBindings.Add(Enlace)

        Dim Row As DataRow = DtCabeza.Rows(0)

        FechaEntregaDesde = Row("FechaEntregaDesde")
        FechaEntregaHasta = Row("FechaEntregaHasta")
        Cliente = Row("Cliente")

        If PPedido <> 0 Or (PPedido = 0 And Lista <> 0) Then  'Caso de un pedido grabado o un alta con lista de precio.
            If Row("Final") Then
                RadioFinal.Checked = True
            Else
                RadioSinIva.Checked = True
            End If
        End If

    End Sub
    Private Sub FormatEnteros(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then Numero.Value = Format(Numero.Value, "#")

    End Sub
    Private Sub Opciones()

        If PPedido = 0 Then OpcionPedidos.PEsAlta = True
        OpcionPedidos.PCliente = Cliente
        OpcionPedidos.ShowDialog()
        If OpcionPedidos.PRegresar Then OpcionPedidos.Dispose() : Exit Sub
        EsImportacionKrikos = OpcionPedidos.PEsImportacionKrikos
        Cliente = OpcionPedidos.PCliente
        Sucursal = OpcionPedidos.PSucursal
        PedidoCliente = OpcionPedidos.PPedidoCliente
        FechaEntregaDesde = OpcionPedidos.PFechaEntregaDesde
        FechaEntregaHasta = OpcionPedidos.PFechaEntregaHasta
        OpcionPedidos.Dispose()

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow
        Dim DtArticulos As DataTable

        If PPedido = 0 Then
            DtArticulos = ArticulosActivos()
        Else
            DtArticulos = TodosLosArticulos()
        End If

        Dim Dt As New DataTable
        If Lista <> 0 Then
            Dt = HallaArticulosListaDePrecio(Lista)
            Dim RowsBusqueda() As DataRow
            Dim RowsBusqueda2() As DataRow
            For Each Row In DtArticulos.Rows
                RowsBusqueda = Dt.Select("Articulo = " & Row("Clave"))
                If RowsBusqueda.Length = 0 Then
                    If DtDetalle.Rows.Count <> 0 Then
                        RowsBusqueda2 = DtDetalle.Select("Articulo = " & Row("Clave"))
                        If RowsBusqueda2.Length = 0 Then Row.Delete()
                    Else
                        Row.Delete()
                    End If
                End If
            Next
            Dt.Dispose()
        End If

        'CON CODIGO CLIENTE
        If PPedido = 0 And Lista = 0 And TieneCodigoCliente(Cliente) Then
            ActualizoConCodigoCliente(DtArticulos, Cliente)
        End If

        Articulo.DataSource = DtArticulos
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

        Dim DTTipoPrecio As New DataTable
        ArmaTipoPrecio(DTTipoPrecio)
        TipoPrecio.DataSource = DTTipoPrecio.Copy
        TipoPrecio.DisplayMember = "Nombre"
        TipoPrecio.ValueMember = "Clave"
        DTTipoPrecio.Dispose()

    End Sub
    Private Function AgregoArticulos(ByVal DtArticulos As DataTable, ByVal Lista As Integer) As Boolean

        Dim Dt As New DataTable
        Dim RowAux As DataRow

        If Not Tablas.Read("SELECT Articulo FROM ListaDePreciosDetalle WHERE Lista = " & Lista & ";", Conexion, Dt) Then Return False
        For Each Row As DataRow In Dt.Rows
            RowAux = DtArticulos.NewRow
            RowAux("Clave") = Row("Articulo")
            DtArticulos.Rows.Add(RowAux)
        Next

        Return True

    End Function
    Private Function ArreglaArchivo() As Integer
        'Se registro el siguiente Error: Cuando Graba pone Numero de pedido en la Base con '0'. Error no encontrado. Esto borra el pedido con '0' y avisa del problema al operador.

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT * FROM PedidosCabeza WHERE Pedido = " & 0 & ";", Conexion, Dt) Then Return -1
        If Dt.Rows.Count <> 0 Then
            For Each Row As DataRow In Dt.Rows
                Row.Delete()
            Next
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM PedidosCabeza;", Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(Dt)
                End Using
            End Using
        Catch ex As OleDb.OleDbException
            Return -1
        Catch ex As DBConcurrencyException
            Return -1
        Finally
        End Try

        Dt.Clear()

        If Not Tablas.Read("SELECT * FROM PedidosDetalle WHERE Pedido = " & 0 & ";", Conexion, Dt) Then Return False
        If Dt.Rows.Count <> 0 Then
            For Each Row As DataRow In Dt.Rows
                Row.Delete()
            Next
        Else
            Return 0
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM PedidosDetalle;", Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(Dt)
                    Return 10
                End Using
            End Using
        Catch ex As OleDb.OleDbException
            Return -1
        Catch ex As DBConcurrencyException
            Return -1
        Finally
        End Try

    End Function
    Public Function UltimaNumeracion(ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Pedido) FROM PedidosCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CInt(Ultimo) + 1
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function ConsisteFechaDesdeHasta(ByVal Lista As Integer, ByVal Desde As Date, ByVal Hasta As Date) As Boolean

        Dim Dt As New DataTable
        Dim Sql As String
        Dim Respuesta As Boolean

        Dim IntDesde As Integer = Format(Desde.Year, "0000") & Format(Desde.Month, "00") & Format(Desde.Day, "00")
        Dim IntHasta As Integer = Format(Hasta.Year, "0000") & Format(Hasta.Month, "00") & Format(Hasta.Day, "00")

        Dim SqlFecha As String = ""
        SqlFecha = "(IntFechaDesde <=" & IntDesde & " AND IntFechaHasta >= " & IntHasta & ") "

        Sql = "Select Lista,IntFechaDesde,IntFechaHasta FROM ListaDePreciosCabeza WHERE " & SqlFecha & " AND Lista = " & Lista & ";"

        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        If Dt.Rows.Count = 0 Then
            MsgBox("Entorno de Fecha del Pedido No Se Corresponde con la Lista de Precios.") : Respuesta = False
        Else
            Respuesta = True
        End If

        Dt.Dispose()
        Return Respuesta

    End Function
    Private Function Valida() As Boolean

        '    If PPedido = 0 Then
        'Dim Dt As New DataTable
        '     If Not Tablas.Read("SELECT Pedido FROM PedidosCabeza WHERE PedidoCliente = '" & Trim(TextPedidoCliente.Text) & "';", Conexion, Dt) Then Return False
        '     If Dt.Rows.Count <> 0 Then
        'MsgBox("Pedido Cliente Ya Existe.") : Return False
        '      End If
        '      End If

        If PPedido <> 0 Then
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT * FROM PedidosCabeza WHERE Pedido = " & PPedido & ";", Conexion, Dt) Then Dt.Dispose() : Return False
            If Dt.Rows.Count = 0 Then
                MsgBox("Pedido fue Borrado por Otro Usuario. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Dt.Dispose() : Return False
            End If
            Dt.Dispose()
        End If

        Dim Contador As Integer = 0
        For Each Row As DataRow In DtDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Contador = Contador + 1
            End If
        Next
        If Contador = 0 Then
            MsgBox("Falta Informar Articulos. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Lista = 0 Then
            If Not RadioSinIva.Checked And Not RadioFinal.Checked Then
                MsgBox("Falta Informar si Precio es Sin Iva o Final. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If DiferenciaDias(FechaHasta.Value, Date.Now) > 0 And Cerrado And Not CheckCerrado.Checked Then
            If MsgBox("Error al ABRIR Pedido, Fecha Entrega Hasta Menor a Fecha del Dia. Desea Continuar?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                CheckCerrado.Checked = Cerrado
                CheckCerrado.Focus()
                Return False
            End If
        End If

        For I As Integer = 0 To Grid.Rows.Count - 2
            If Grid.Rows(I).Cells("Cantidad").Value = 0 Then
                '    MsgBox("Debe Informar Cantidad en la linea " & I + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                '      Grid.CurrentCell = Grid.Rows(I).Cells("Cantidad")
                '       Grid.BeginEdit(True)
                '        Return False
            End If
            Dim AGranel As Boolean
            Dim Medida As String = ""
            HallaAGranelYMedida(Grid.Rows(I).Cells("Articulo").Value, AGranel, Medida)
            If TieneDecimales(Grid.Rows(I).Cells("Cantidad").Value) And Not AGranel Then
                MsgBox("Cantidad no debe tener Decimales en la linea " & I + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(I).Cells("Cantidad")
                Grid.BeginEdit(True)
                Return False
            End If
        Next

        If (FechaEntregaDesde <> FechaDesde.Value Or FechaEntregaHasta <> FechaHasta.Value) And Lista <> 0 Then
            If Not ConsisteFechaDesdeHasta(Lista, FechaDesde.Value, FechaHasta.Value) Then
                Return False
            End If
        End If
        If (FechaEntregaDesde <> FechaDesde.Value Or FechaEntregaHasta <> FechaHasta.Value) And Lista = 0 Then
            If DiferenciaDias(FechaDesde.Value, FechaHasta.Value) < 0 Then
                MsgBox("Fecha Entrega Desde Mayor a Fecha Entrega Hasta. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                FechaDesde.Focus()
                Return False
            End If
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
                If cb.SelectedIndex < 0 Then cb.SelectedValue = 0
            End If
            Grid.Refresh()
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

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Articulo" Then Exit Sub
        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "TipoPrecio" Then Exit Sub

        If Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloConDecimal_KeyPress
            AddHandler Texto.TextChanged, AddressOf TextoConDecimal_TextChanged
        End If

    End Sub
    Private Sub SoloConDecimal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If e.KeyChar = "." Then e.KeyChar = ","
        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextoConDecimal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If CType(sender, TextBox).Text <> "" Then
            EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Entregada" Or Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            If e.Value = 0 Then e.Value = Format(0, "#")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Precio" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = Format(e.Value, "0.00")
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtDetalle_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Pedido") = PPedido
        e.Row("Sucursal") = Sucursal
        e.Row("Articulo") = 0
        e.Row("Cantidad") = 0
        e.Row("Entregada") = 0
        e.Row("Precio") = 0
        e.Row("TipoPrecio") = ComboTipoPrecio.SelectedValue

    End Sub
    Private Sub DtDetalle_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        e.Row.SetColumnError(e.Column, "")

        If e.Column.ColumnName.Equals("Articulo") Then
            If e.ProposedValue <> 0 Then
                e.Row.RowError = ""
                Dim RowsBusqueda() As DataRow
                RowsBusqueda = DtDetalle.Select("Articulo = " & e.ProposedValue)
                If RowsBusqueda.Length <> 0 Then
                    MsgBox("Articulo ya Existe.")
                    e.ProposedValue = 0
                    Exit Sub
                End If
            End If
        End If

        If e.Column.ColumnName.Equals("Cantidad") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If Not IsDBNull(e.Row("Entregada")) Then
                If e.ProposedValue < e.Row("Entregada") Then
                    MsgBox("Cantidad Menor a Entregado.")
                    e.ProposedValue = e.Row("Cantidad")
                End If
            End If
        End If

        If e.Column.ColumnName.Equals("Precio") Then
            If IsNothing(e.ProposedValue) Then e.ProposedValue = 0
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("TipoPrecio") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If IsNothing(e.ProposedValue) Then e.ProposedValue = 0
        End If

    End Sub
    Private Sub DtDetalle_RowChanging(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        If e.Row.RowState = DataRowState.Deleted Then Exit Sub

        e.Row.ClearErrors()

        If e.Row("Articulo") = 0 Then
            e.Row.SetColumnError("Articulo", "Error")
        End If

        If e.Row("Cantidad") = 0 Then
            e.Row.SetColumnError("Cantidad", "Error")
        End If

        If e.Row("TipoPrecio") = 0 And e.Row("Precio") <> 0 Then
            e.Row.SetColumnError("TipoPrecio", "DEBE INFORMAR TIPO DE PRECIO!")
            MsgBox("Debe Informar Tipo Precio si Informa Precio.")
        End If
        If e.Row("TipoPrecio") <> 0 And e.Row("Precio") = 0 Then
            e.Row.SetColumnError("Precio", "DEBE INFORMAR PRECIO!")
            MsgBox("Debe Informar Precio si Informa Tipo Precio.")
        End If

        If e.Row.GetColumnsInError.Length <> 0 Then Grid.Refresh()

    End Sub
    Private Sub ButtonImportacionKrikos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportacionKrikos.Click

        PedidoCliente = ""
        Dim SucursalW As Integer

        If PSinSucursal Then
            SucursalW = 0
        Else
            SucursalW = Sucursal
        End If

        If LeeArchivoTXT(Cliente, SucursalW, FechaEntregaDesde, PedidoCliente) Then
            Sucursal = SucursalW
            RadioSinIva.Checked = False
            RadioFinal.Checked = False
            PPedido = 0
            UnPedido_Load(Nothing, Nothing)
            MuestaDetalleKrikos(Cliente, PedidoCliente)
        End If

    End Sub
    Private Function MuestaDetalleKrikos(ByVal Cliente As Integer, ByVal PedidoClienteW As String) As Boolean

        DtDetalle.Rows.Clear()

        If Not LeeDetalleTXT(Cliente, Lista, PedidoClienteW, Sucursal, DtDetalle) Then
            Return False
        End If

        Grid.Refresh()

        Return True

    End Function
End Class
