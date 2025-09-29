Imports System.Transactions
Public Class UnaOrdenCompra
    Public PTipo As Integer   '1.Articulos Terminados    2.Insumos.
    Public POrden As Double
    Public PActualizacionOk As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtGrid As DataTable
    '
    Dim Abierto As Boolean
    Dim cb As ComboBox
    Dim Proveedor As Integer
    Dim TotalB As Double
    Dim ConexionOrden As String
    Dim FechaEntregaAnt As Date
    Dim FechaEntregaHastaAnt As Date
    Dim PlazoPagoAnt As String
    Dim ClienteOpr As Boolean
    'Para impresora
    Dim Ancho As Integer
    Dim Alto As Integer
    Dim mRow As Integer = 0
    Dim ContadorPaginas As Integer = 0
    Dim TotalPaginas As Integer = 0
    Dim newpage As Boolean = True
    Dim WithEvents Impresora As New System.Drawing.Printing.PrintDocument()

    'Datos.
    Dim Calle As String
    Dim Localidad As String
    Private Sub UnaOrdenCompra_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(12) Then PBloqueaFunciones = True

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        Select Case PTipo
            Case 1
                Comboproveedor.DataSource = ProveedoresDeFrutas()   'Articulos terminados.
            Case 2
                Comboproveedor.DataSource = ProveedoresTodos()  'Todos. 
        End Select
        Comboproveedor.DisplayMember = "Nombre"
        Comboproveedor.ValueMember = "Clave"

        If POrden = 0 Then
            Select Case PTipo
                Case 1
                    OpcionProveedor.PSoloArticulos = True
                Case 2
                    OpcionProveedor.PSoloInsumos = True
            End Select
            OpcionProveedor.PEsOrdenCompra = True
            OpcionProveedor.ShowDialog()
            Proveedor = OpcionProveedor.PProveedor
            OpcionProveedor.Dispose()
            If Proveedor = 0 Then Me.Close() : Exit Sub
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        ArmaTipoIva(ComboTipoIva)
        LlenaComboTablas(ComboPais, 28)

        LlenaCombosGrid()

        PActualizacionOk = False

        ArmaArchivos()

        LlenaDatosEmisor(Proveedor)

        If PTipo = 1 Then
            Grid.Columns("Consumido").Visible = False
        Else
            Grid.Columns("Consumido").Visible = True
        End If

    End Sub
    Private Sub UnIngresoMercaderia_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub UnaOrdenCompra_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

        Entrada.Show()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        If POrden = 0 Then
            HacerAlta()
        Else
            HacerModificacion()
        End If

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If POrden = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Orden ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If CDec(TextSaldo.Text) = 0 Then
            MsgBox("Orden Compra ya esta Facturada. Operación se CANCELA.") : Exit Sub
        End If

        For Each Row As DataRow In DtDetalle.Rows
            If Row("Recibido") <> 0 Then
                MsgBox("Orden de Compra Tiene Recepciones. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        Next

        If DtCabeza.Rows(0).Item("Saldo") <> DtCabeza.Rows(0).Item("Importe") And POrden <> 0 Then
            MsgBox("Orden de Compra esta Facturada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        Dim DtCabezaRelAux As New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If MsgBox("Orden de Compra se Anulara. ¿Desea Anularla?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtCabezaAux.Rows(0).Item("Estado") = 3

        Dim Resul As Double = AnulaOrden(DtCabezaAux)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If Resul < 0 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul > 0 Then
            MsgBox("Baja Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PActualizacionOk = True
        End If

        DtCabezaAux.Dispose()
        DtCabezaRelAux.Dispose()

        ArmaArchivos()

    End Sub
    Private Sub RadioFinal_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioFinal.CheckedChanged

        CalculaTotal()

    End Sub
    Private Sub ButtonDatosCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDatosEmisor.Click

        If Comboproveedor.SelectedValue = 0 Then Exit Sub

        UnDatosEmisor.PEsProveedor = True

        UnDatosEmisor.PEmisor = Comboproveedor.SelectedValue
        UnDatosEmisor.ShowDialog()

    End Sub
    Private Sub ButtonExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExcel.Click

        InformeFacturasDeOrdenCompra(POrden)

    End Sub
    Private Sub ButtonAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAgregar.Click

        If CDec(TextSaldo.Text) = 0 And POrden <> 0 Then
            MsgBox("Orden Compra Ya Facturada. Operación se CANCELA.")
            Exit Sub
        End If

        Grid.Columns("Articulo").ReadOnly = False

    End Sub
    Private Sub ButtonImprime_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprime.Click

        UnSeteoImpresora.SeteaImpresion(Impresora) 'setea impresora con lo definido en "Control Impresora" del menu.

        Impresora.DefaultPageSettings.Landscape = False
        Alto = Impresora.DefaultPageSettings.PaperSize.Width / 1.15
        Ancho = Impresora.DefaultPageSettings.PaperSize.Height / 1.15

        mRow = 0
        ContadorPaginas = 1
        TotalPaginas = Grid.Rows.Count / 36
        If Grid.Rows.Count / 36 - TotalPaginas > 0 Then
            TotalPaginas = TotalPaginas + 1
        End If
        ContadorPaginas = 1

        Dim DTProveedor As New DataTable
        If Not Tablas.Read("SELECT Calle,Localidad FROM Proveedores WHERE Clave = " & Comboproveedor.SelectedValue & ";", Conexion, DTProveedor) Then End

        If DTProveedor.Rows.Count = 0 Then Exit Sub

        Calle = DTProveedor.Rows(0).Item("Calle")
        Localidad = DTProveedor.Rows(0).Item("Localidad")

        Impresora.Print()
        newpage = True
        Impresora.Dispose()

    End Sub
    Private Sub ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        ArmaGrid()

        ConexionOrden = Conexion

        DtCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM OrdenCompraCabeza WHERE Orden = " & POrden & ";", ConexionOrden, DtCabeza) Then Me.Close() : Exit Sub
        If POrden <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Orden de Compra No Existen En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If POrden = 0 Then
            Dim Row As DataRow = DtCabeza.NewRow
            Row("Tipo") = PTipo
            Row("Orden") = 0
            Row("Proveedor") = Proveedor
            Row("Fecha") = Now
            Row("FechaEntrega") = Now
            Row("FechaEntregaHasta") = Now
            Row("PlazoPago") = ""
            Row("PrecioFinal") = False
            Row("Importe") = 0
            Row("Rel") = False         'no se usa.  dejar false.
            Row("NRel") = 0            'no se usa.
            Row("Estado") = 1
            Row("Saldo") = 0
            Row("Comentario") = ""
            DtCabeza.Rows.Add(Row)
        End If

        Proveedor = DtCabeza.Rows(0).Item("Proveedor")

        MuestraCabeza()

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM OrdenCompraDetalle WHERE Orden = " & POrden & ";", ConexionOrden, DtDetalle) Then Me.Close() : Exit Sub
        For Each Row As DataRow In DtDetalle.Rows
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("PrecioB") = Row("Precio")
            Row1("Articulo") = Row("Articulo")
            Row1("Cantidad") = Row("Cantidad")
            Row1("Recibido") = Row("Recibido")
            Row1("Iva") = Row("Iva")
            Row1("Precio") = Row("Precio")
            Row1("Neto") = CalculaNeto(Row("Cantidad"), Row("Precio"))
            Row1("MontoIva") = CalculaIva(Row("Cantidad"), Row("Precio"), Row("Iva"))
            Row1("TotalArticulo") = Row1("Neto") + Row1("MontoIva")
            Row1("Consumido") = HallaConsumido(POrden, Row("Articulo"))   'para consumo de insumos.
            DtGrid.Rows.Add(Row1)
        Next

        TotalB = 0

        For Each Row As DataRow In DtGrid.Rows
            TotalB = TotalB + CalculaNeto(Row("Cantidad"), Row("PrecioB")) + Row("MontoIva")
        Next

        TextTotal.Text = FormatNumber(TotalB, GDecimales)

        If DtDetalle.Rows.Count > 0 Then ButtonImprime.Enabled = True

        Grid.DataSource = DtGrid

        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)
        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.RowChanged, New DataRowChangeEventHandler(AddressOf Dtgrid_RowChanged)

        Grid.Enabled = True
        LabelOrdenFacturada.Visible = False

        If POrden = 0 Then
            Panel5.Enabled = True
            Grid.Columns("Articulo").ReadOnly = False
            ButtonEliminarLinea.Enabled = True
            Panel5.Visible = True
        Else
            RadioSinIva.Checked = True
            Grid.Columns("Articulo").ReadOnly = True
            ButtonEliminarLinea.Enabled = False
            Panel5.Visible = False
            If CDec(TextSaldo.Text) = 0 Then
                Grid.Enabled = False
                LabelOrdenFacturada.Visible = True
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Orden")
        AddHandler Enlace.Format, AddressOf Formatorden
        TextOrden.DataBindings.Clear()
        TextOrden.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextImporteWW.DataBindings.Clear()
        TextImporteWW.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Saldo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextSaldo.DataBindings.Clear()
        TextSaldo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Proveedor")
        Comboproveedor.DataBindings.Clear()
        Comboproveedor.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        FechaEmision.DataBindings.Clear()
        FechaEmision.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaEntrega")
        FechaEntregaDesde.DataBindings.Clear()
        FechaEntregaDesde.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaEntregaHasta")
        FechaEntregaHasta.DataBindings.Clear()
        FechaEntregaHasta.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "PlazoPago")
        AddHandler Enlace.Format, AddressOf FormatTexto
        TextPlazoPago.DataBindings.Clear()
        TextPlazoPago.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Format, AddressOf FormatTexto
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        FechaEntregaAnt = DtCabeza.Rows(0).Item("FechaEntrega")
        FechaEntregaHastaAnt = DtCabeza.Rows(0).Item("FechaEntregaHasta")

        PlazoPagoAnt = DtCabeza.Rows(0).Item("PlazoPago")

    End Sub
    Private Sub Formatorden(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "00000000")

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub FormatTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        POrden = 0
        UnaOrdenCompra_Load(Nothing, Nothing)

    End Sub
    Private Sub HacerAlta()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtCabezaB As New DataTable
        Dim DtDetalleB As New DataTable

        DtCabezaB = DtCabeza.Copy
        DtCabezaB.Rows(0).Item("Importe") = TotalB
        DtCabezaB.Rows(0).Item("Saldo") = TotalB
        DtDetalleB = DtDetalle.Clone
        For Each Row As DataRow In DtGrid.Rows
            Dim Row1 As DataRow = DtDetalleB.NewRow
            Row1("Articulo") = Row("Articulo")
            Row1("Iva") = Row("Iva")
            Row1("Cantidad") = Row("Cantidad")
            Row1("Precio") = Row("PrecioB")
            Row1("TotalArticulo") = CalculaNeto(Row("Cantidad"), Row("PrecioB")) + Trunca(CalculaIva(Row("Cantidad"), Row("PrecioB"), Row("Iva")))
            Row1("Recibido") = 0
            DtDetalleB.Rows.Add(Row1)
        Next
        '
        'Graba Orden Compra.
        Dim NumeroB As Double = 0
        Dim Resul As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion para Orden.
            If DtCabezaB.Rows.Count <> 0 Then
                NumeroB = UltimaNumeracion(Conexion)
                If NumeroB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                DtCabezaB.Rows(0).Item("Orden") = NumeroB
                For Each Row As DataRow In DtDetalleB.Rows
                    Row("Orden") = NumeroB
                Next
            End If

            Resul = ActualizaOrden(DtCabezaB, DtDetalleB)

            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = -2 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            If NumeroB <> 0 Then
                POrden = NumeroB
                PActualizacionOk = True
            End If
        End If

        DtCabezaB.Dispose()
        DtDetalleB.Dispose()

        ArmaArchivos()

    End Sub
    Private Sub HacerModificacion()

        Dim Dt As New DataTable
        Dim Facturado As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Tablas.Read("SELECT Facturado FROM IngresoInsumoCabeza WHERE OrdenCompra = " & POrden & ";", ConexionOrden, Dt) Then Me.Close() : Exit Sub
        For Each Row As DataRow In Dt.Rows
            If Row("Facturado") > 0 Then
                Facturado = True
            End If
        Next
        Dt.Dispose()

        Dim RowsBusqueda() As DataRow
        Dim ModificoPrecio As Boolean
        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtDetalle.Select("Articulo = " & Row("Articulo"))
            If RowsBusqueda.Length <> 0 Then
                If Row("Precio") <> RowsBusqueda(0).Item("Precio") Then ModificoPrecio = True
            End If
        Next

        If Facturado And ModificoPrecio Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Precio No se Debe Modificar, Orden de Compra Tiene Facturas. Operación se CANCELA.")
            Exit Sub
        End If

        Dim DtCabezaW As DataTable = DtCabeza.Copy
        Dim DtDetalleW As DataTable = DtDetalle.Copy
        Dim Importe As Decimal

        'Agrega articulos nuevos.
        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtDetalle.Select("Articulo = " & Row("Articulo"))
            If RowsBusqueda.Length = 0 Then
                Dim Row1 As DataRow = DtDetalleW.NewRow
                Row1("Orden") = POrden
                Row1("Articulo") = Row("Articulo")
                Row1("Iva") = Row("Iva")
                Row1("Cantidad") = Row("Cantidad")
                Row1("Precio") = Row("PrecioB")
                Row1("TotalArticulo") = CalculaNeto(Row("Cantidad"), Row("PrecioB")) + Trunca(CalculaIva(Row("Cantidad"), Row("PrecioB"), Row("Iva")))
                Row1("Recibido") = 0
                DtDetalleW.Rows.Add(Row1)
            End If
        Next
        '------------------------------

        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtDetalleW.Select("Articulo = " & Row("Articulo"))
            If Row("Precio") <> RowsBusqueda(0).Item("Precio") Or Row("Cantidad") <> RowsBusqueda(0).Item("Cantidad") Then
                RowsBusqueda(0).Item("Precio") = Row("Precio")
                RowsBusqueda(0).Item("Cantidad") = Row("Cantidad")
                RowsBusqueda(0).Item("TotalArticulo") = Trunca(CalculaNeto(RowsBusqueda(0).Item("Cantidad"), RowsBusqueda(0).Item("Precio")) + CalculaIva(RowsBusqueda(0).Item("Cantidad"), RowsBusqueda(0).Item("Precio"), RowsBusqueda(0).Item("Iva")))
            End If
        Next

        For Each Row As DataRow In DtDetalleW.Rows
            Importe = Importe + Row("TotalArticulo")
        Next

        Dim Imputado As Decimal = DtCabezaW.Rows(0).Item("Importe") - DtCabezaW.Rows(0).Item("Saldo")
        DtCabezaW.Rows(0).Item("Importe") = Importe
        DtCabezaW.Rows(0).Item("Saldo") = Importe - Imputado

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaW.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaW.GetChanges, "OrdenCompraCabeza", ConexionOrden)
                End If
                If Resul > 0 Then
                    If Not IsNothing(DtDetalleW.GetChanges) Then
                        Resul = GrabaTabla(DtDetalleW.GetChanges, "OrdenCompraDetalle", ConexionOrden)
                    End If
                End If
                '
                If Resul > 0 Then Scope.Complete()
            End Using
        Catch ex As TransactionException
            Resul = 0
        Finally
        End Try
        '
        If Resul = -1 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PActualizacionOk = True
            ArmaArchivos()
        End If

        DtCabezaW.Dispose()
        DtDetalleW.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function ActualizaOrden(ByVal DtCabezaBAux As DataTable, ByVal DtDetalleBAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaBAux.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaBAux.GetChanges, "OrdenCompraCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtDetalleBAux.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleBAux.GetChanges, "OrdenCompraDetalle", Conexion)
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
    Private Function AnulaOrden(ByVal DtCabezaAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaAux.GetChanges, "OrdenCompraCabeza", ConexionOrden)
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
    Private Sub LlenaDatosEmisor(ByVal Proveedor As Integer)

        Dim Dta As New DataTable
        Dim Sql As String = ""

        Sql = "SELECT * FROM Proveedores WHERE Clave = " & Proveedor & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, Proveedor ya no existe o error en la Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
            Exit Sub
        End If

        TextProvincia.Text = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")
        TextCuit.Text = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ClienteOpr = Dta.Rows(0).Item("Opr")

        Dta.Dispose()

    End Sub
    Private Sub ArmaGrid()

        DtGrid = New DataTable

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Precio As New DataColumn("Precio")
        Precio.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Precio)

        Dim PrecioB As New DataColumn("PrecioB")
        PrecioB.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(PrecioB)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cantidad)

        Dim Recibido As New DataColumn("Recibido")
        Recibido.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Recibido)

        Dim Iva As New DataColumn("Iva")
        Iva.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Iva)

        Dim Neto As New DataColumn("Neto")
        Neto.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Neto)

        Dim MontoIva As New DataColumn("MontoIva")
        MontoIva.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(MontoIva)

        Dim TotalArticulo As New DataColumn("TotalArticulo")
        TotalArticulo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(TotalArticulo)

        Dim Consumido As New DataColumn("Consumido")
        Consumido.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Consumido)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Select Case PTipo
            Case 1
                Articulo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Articulos;")
            Case 2
                Articulo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Insumos;")
        End Select
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Sub CalculaTotal()

        Dim Precio As Double = 0
        Dim TotalW As Double = 0

        TotalB = 0

        For I As Integer = 0 To Grid.Rows.Count - 2
            Precio = Grid.Rows(I).Cells("Precio").Value
            If RadioFinal.Checked Then
                Grid.Rows(I).Cells("PrecioB").Value = Trunca3(Precio / (1 + Grid.Rows(I).Cells("Iva").Value / 100))
                Grid.Rows(I).Cells("PrecioB").Value = Precio / (1 + Grid.Rows(I).Cells("Iva").Value / 100)
            Else
                Grid.Rows(I).Cells("PrecioB").Value = Trunca3(Precio)
                Grid.Rows(I).Cells("PrecioB").Value = Precio
            End If
            Grid.Rows(I).Cells("Neto").Value = CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioB").Value)
            Grid.Rows(I).Cells("MontoIva").Value = CalculaIva(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioB").Value, Grid.Rows(I).Cells("Iva").Value)
            Grid.Rows(I).Cells("TotalArticulo").Value = Trunca(Grid.Rows(I).Cells("Neto").Value + Grid.Rows(I).Cells("MontoIva").Value)
            TotalW = TotalW + Grid.Rows(I).Cells("TotalArticulo").Value
            TotalB = TotalB + Trunca(CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioB").Value) + Grid.Rows(I).Cells("MontoIva").Value)
        Next

        TotalB = Trunca(TotalB)

        TextTotal.Text = FormatNumber(TotalW, GDecimales)

    End Sub
    Private Sub PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles Impresora.PrintPage

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim PrintFont As System.Drawing.Font
        Dim SaltoLinea As Integer = 4
        Dim MIzq = 9
        Dim MTop = 20

        MascaraImpresion.ImprimeMascara(GNombreEmpresa, GCuitEmpresa, 50, GDireccion1, GDireccion2, GDireccion3, "", GCondicionIvaEmpresa, GIngBruto, GFechaInicio, "C:\XML Afip\", e, False, 1)

        Texto = Val(TextOrden.Text)
        PrintFont = New Font("Arial", 16)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, MTop + 6)

        '**************************************************************************************************************'

        e.Graphics.PageUnit = GraphicsUnit.Millimeter
        'Titulos.
        x = MIzq
        y = MTop + 44

        PrintFont = New Font("Courier New", 10)

        e.Graphics.DrawString("ESTADO: " & ComboEstado.Text, PrintFont, Brushes.Black, x + 155, MTop + 2)
        e.Graphics.DrawString(FechaEmision.Text, PrintFont, Brushes.Black, x + 135, MTop + 18)
        'Titulos.
        Texto = "PROVEEDOR      : " & Comboproveedor.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
        Texto = "FECHA ENTREGA DESDE: " & FechaEntregaDesde.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 120, y)
        y = y + SaltoLinea
        Texto = "DOMICILIO      : " & Calle
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
        Texto = "FECHA ENTREGA HASTA: " & FechaEntregaHasta.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 120, y)
        y = y + SaltoLinea
        Texto = "LOCALIDAD      : " & Localidad
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
        y = y + SaltoLinea
        Texto = "CUIT           : " & TextCuit.Text
        PrintFont = New Font("Courier New", 10)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
        y = y + SaltoLinea
        Texto = "CONDICION PAGO: " & TextPlazoPago.Text
        If TextPlazoPago.Text <> "" Then Texto = Texto & " DIAS"
        PrintFont = New Font("Courier New", 10, FontStyle.Bold)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)

        Dim YInicioRectangulo As Integer = 70

        x = 9
        y = MTop + YInicioRectangulo

        Dim x1 As Integer = x - 5   'margen izquierdo para cuadro de articulos.
        Dim Ancho As Integer = 198
        Dim Alto As Integer = 150
        Dim LineaArticulo As Integer = x1 + 70 '87
        Dim LineaCantidad As Integer = x1 + 98 '112
        Dim LineaPrecio As Integer = x1 + 120 '129    '132
        Dim LineaNeto As Integer = x1 + 146 '150
        Dim LineaIVA As Integer = x1 + 167 '172   '174
        Dim LineaTotal As Integer = x1 + Ancho - 2
        Dim Longi As Integer
        Dim Xq As Integer
        Dim Excedente As Integer = Grid.Rows.Count - 36
        '
        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.2), x1, y, Ancho - 2, Alto)
        'Lineas vertical.
        e.Graphics.DrawLine(New Pen(Color.Black, 0.2), LineaArticulo, y, LineaArticulo, y + Alto)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.2), LineaCantidad, y, LineaCantidad, y + Alto)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.2), LineaPrecio, y, LineaPrecio, y + Alto)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.2), LineaNeto, y, LineaNeto, y + Alto)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.2), LineaIVA, y, LineaIVA, y + Alto)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.2), LineaTotal, y, LineaTotal, y + Alto)
        '
        PrintFont = New Font("Courier New", 12)
        Texto = "Pagina: " & ContadorPaginas & "/" & TotalPaginas
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + Ancho - 40, MTop - 17)
        '
        'Titulos de descripcion.
        PrintFont = New Font("Courier New", 11)
        Texto = "ARTICULO"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x1 + 2, y + 2)
        Texto = "CANTIDAD"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaCantidad - Longi - 3
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "PRECIO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaPrecio - Longi - 3
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "NETO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaNeto - Longi - 9
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "Imp.IVA"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaIVA - Longi
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "TOTAL"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaTotal - Longi - 8
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        'linea horizontal.
        y = y + 2 * SaltoLinea
        e.Graphics.DrawLine(New Pen(Color.Black, 0.3), x1, y, x1 + Ancho - 2, y)
        'Descripcion de Articulos.
        PrintFont = New Font("Courier New", 9)
        y = y - SaltoLinea

        Do While mRow < Grid.Rows.Count
            Dim Row As DataGridViewRow = Grid.Rows(mRow)
            '  Articulo ------------------------------------------------
            If IsNothing(Row.Cells("Articulo").Value) Then Exit Do
            y = y + SaltoLinea
            Texto = Row.Cells("Articulo").FormattedValue
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x1, y)
            '
            '  Cantidad ------------------------------------------------
            Texto = Row.Cells("Cantidad").FormattedValue
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            '  Precio ------------------------------------------------
            Texto = Row.Cells("Precio").FormattedValue
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaPrecio - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            '  Neto ------------------------------------------------
            Texto = Row.Cells("Neto").FormattedValue
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaNeto - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            '  Monto IVA ------------------------------------------------
            Texto = Row.Cells("MontoIVA").FormattedValue
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaIVA - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            '  TOTAL ------------------------------------------------
            Texto = Row.Cells("TotalArticulo").FormattedValue
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaTotal - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            '---------------------------------------------------------
            mRow += 1
            newpage = False
            If Excedente <= 0 Then Continue Do
            If mRow + 1 = Grid.Rows.Count - Excedente Then
                e.HasMorePages = True
                newpage = True
                ContadorPaginas = ContadorPaginas + 1
                Exit Do
            End If
        Loop

        y = MTop + YInicioRectangulo + Alto + 1

        If mRow + 1 = Grid.RowCount Then
            Texto = "TOTAL"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaIVA - 15, y + 4)
            Texto = "SALDO"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaIVA - 15, y + 12)

            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.2), LineaIVA, y + 3, (LineaTotal - LineaIVA), 5)
            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.2), LineaIVA, y + 11, (LineaTotal - LineaIVA), 5)

            Texto = TextTotal.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaTotal - Longi, y + 4)

            Texto = TextSaldo.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaTotal - Longi, y + 12)
        End If

        Texto = "Recibí Conforme:........................"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 45, y + 15)

    End Sub
    Private Function HallaConsumido(ByVal OrdenCompra As Double, ByVal Articulo As Integer) As Decimal

        Dim Sql As String = "SELECT SUM(D.Cantidad) FROM ConsumosCabeza AS C INNER JOIN ConsumosDetalle AS D ON C.Consumo = D.Consumo WHERE C.Estado = 1 AND D.OrdenCompra = " & OrdenCompra & " AND C.Insumo = " & Articulo & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionOrden)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Cantidad = Cmd.ExecuteScalar()
                    If Not IsDBNull(Cantidad) Then Return CDec(Cantidad)
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: ConsumosCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Orden) FROM OrdenCompraCabeza;", Miconexion)
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

        If POrden <> 0 And DtCabeza.Rows(0).Item("Rel") And Not PermisoTotal Then
            MsgBox("Error, en este momento no puede modificar la Orden de Compra(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If Comboproveedor.SelectedValue = 0 Then
            MsgBox("Falta Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Comboproveedor.Focus()
            Return False
        End If
        If Grid.Rows.Count = 1 Then
            MsgBox("Falta Informar Articulos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Not ClienteOpr Then
            If MsgBox("Proveedor Solo Opera a Candado Cerrado. Debe Cambiar Candado en Proveedor. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
        End If

        For i As Integer = 0 To Grid.RowCount - 2
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
            If IsDBNull(Grid.Rows(i).Cells("Precio").Value) Then
                MsgBox("Debe Informar Precio en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Precio")
                Grid.BeginEdit(True)
                Return False
            End If
            If Grid.Rows(i).Cells("Precio").Value = 0 Then
                MsgBox("Debe Informar Precio en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Precio")
                Grid.BeginEdit(True)
                Return False
            End If
        Next

        If DiferenciaDias(FechaEntregaDesde.Value, FechaEntregaHasta.Value) < 0 Then
            MsgBox("Fecha Entrega Invalida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            FechaEntregaDesde.Focus()
            Return False
        End If

        Return True

    End Function

    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick
        If CDec(TextSaldo.Text) = 0 Then
            '      Grid.Enabled = False
        End If

    End Sub
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

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Precio" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPressConDecimal
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub ValidaKey_KeyPressConDecimal(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If CDec(TextSaldo.Text) = 0 And POrden <> 0 Then
            MsgBox("Orden Compra Ya Facturada. Operación se CANCELA.")
            e.KeyChar = "" : Exit Sub
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Precio" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Precio" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 3)
            End If
        End If

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

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Recibido" Or Grid.Columns(e.ColumnIndex).Name = "Consumido" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Precio" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else : e.Value = FormatNumber(e.Value, 3)
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "MontoIva" Or Grid.Columns(e.ColumnIndex).Name = "Neto" Or Grid.Columns(e.ColumnIndex).Name = "TotalArticulo" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else : e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        CalculaTotal()

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then Exit Sub

        If Grid.CurrentRow.Cells("Recibido").Value <> 0 Then
            MsgBox("Articulo no se puede borrar pues tiene Recepciones.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid.Rows.Remove(Grid.CurrentRow)

        CalculaTotal()

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row
        Row("Articulo") = 0
        Row("Cantidad") = 0
        Row("Recibido") = 0
        Row("Iva") = 0
        Row("PrecioB") = 0
        Row("Precio") = 0
        Row("Neto") = 0
        Row("MontoIva") = 0
        Row("TotalArticulo") = 0
        Row("Consumido") = 0

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Articulo")) Then
            If e.ProposedValue <> 0 Then
                For Each Row As DataRow In DtGrid.Rows
                    If Row("Articulo") = e.ProposedValue Then
                        MsgBox("Insumo Ya Existe en la Orden.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Grid.Rows.Remove(Grid.CurrentRow)
                        Exit Sub
                    End If
                Next
                Select Case PTipo
                    Case 1
                        e.Row("Iva") = HallaIva(e.ProposedValue)
                    Case 2
                        e.Row("Iva") = HallaIvaInsumo(e.ProposedValue)
                End Select
                If e.Row("Iva") < 0 Then
                    MsgBox("ERROR Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close()
                End If
            End If
        End If

        If (e.Column.ColumnName.Equals("Cantidad")) Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If Not IsDBNull(e.Row("Recibido")) Then
                If e.ProposedValue < e.Row("Recibido") Then
                    MsgBox("Cantidad Menor a lo ya Recibido. Modificación se Cancela.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    e.ProposedValue = e.Row("cantidad")
                End If
            End If
        End If

        If (e.Column.ColumnName.Equals("Precio")) Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca3(e.ProposedValue)
            If PTipo = 2 Then
                If Not IsDBNull(e.Row("Consumido")) Then
                    If e.Row("Consumido") <> 0 Then
                        MsgBox("El Insumos Fue Consumido. Modificación se Cancela.", MsgBoxStyle.Information)
                        e.ProposedValue = e.Row("Precio")
                    End If
                End If
            End If
            If PTipo = 1 Then
                If Not IsDBNull(e.Row("Recibido")) Then
                    If e.Row("Recibido") <> 0 Then
                        MsgBox("El Articulo tiene Ingreso. Modificación se Cancela.", MsgBoxStyle.Information)
                        e.ProposedValue = e.Row("Precio")
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub Dtgrid_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        'Borra las lineas en blancos que aparecen cuando hago click en el ultimo renglon y sin informar nada regreso a algun renglon anterior. 
        If e.Row("Articulo") = 0 And e.Row("Cantidad") = 0 And e.Row("Precio") = 0 Then e.Row.Delete()

    End Sub
End Class