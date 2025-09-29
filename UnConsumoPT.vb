Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnConsumoPT
    Public PConsumo As Integer
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '   
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim ConsumoLotes As DataTable
    '
    Dim ListaDeLotes As New List(Of FilaAsignacion)
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim TipoFactura As Integer
    '
    Dim PermiteMuchosArticulos As Boolean
    Dim IndiceW As Integer
    Dim ConexionConsumo As String
    Dim IndiceCoincidencia As Integer
    Dim cb As ComboBox
    Dim UltimoNumero As Double
    Dim UltimaFechaW As DateTime
    Dim UltimafechaContableW As DateTime
    Dim TipoAsiento As Integer
    Dim TipoAsientoCosto As Integer
    '
    Dim Calle As String
    Dim Localidad As String
    Dim Provincia As String
    Dim Cuit As String
    'Variables Impresion. 
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim LineasParaImpresion As Integer = 0
    Dim LineasParaImpresionImputacion As Integer = 0
    '
    Dim Costeo As Integer
    Private Sub UnConsumoPT_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        Grid.Columns("LupaLotes").DefaultCellStyle.NullValue = Nothing

        PermiteMuchosArticulos = True

        LlenaComboTablas(ComboDeposito, 19)

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0

        ComboNegocio.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;")
        Dim Row As DataRow = ComboNegocio.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboNegocio.DataSource.rows.add(Row)
        ComboNegocio.DisplayMember = "Nombre"
        ComboNegocio.ValueMember = "Clave"
        ComboNegocio.SelectedValue = 0
        With ComboNegocio
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PConsumo = 0 Then
            PideDatosCosteo()
            If Costeo = 0 Then Me.Close() : Exit Sub
        End If

        If PAbierto Then
            ConexionConsumo = Conexion
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
        Else
            ConexionConsumo = ConexionN
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
        End If

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        TipoAsiento = 61000
        TipoAsientoCosto = 61001

        GModificacionOk = False

        If Not MuestraDatos() Then Me.Close() : Exit Sub

        LlenaCombosGrid()

        If GGeneraAsiento Then
            Dim Conta = TieneTabla1(TipoAsiento)
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

        UnTextoParaRecibo.Dispose()

        UltimaFechaW = UltimaFecha(Conexion)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub UnaFactura_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()
        Grid.EndEdit()

        If Not Valida() Then Exit Sub

        HacerAlta()

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
            MsgBox("Factura ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Valida() Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If HallaArticuloDeshabilitado(DtDetalle) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim DtCabezaAux As DataTable = DtCabeza.Copy

        DtCabezaAux.Rows(0).Item("Estado") = 3

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoCabezaCosto As New DataTable
        ' 
        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(TipoAsiento, DtCabeza.Rows(0).Item("Consumo"), DtAsientoCabeza, ConexionConsumo) Then Me.Close() : Exit Sub
            If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3
            If Not HallaAsientosCabeza(TipoAsientoCosto, DtCabeza.Rows(0).Item("Consumo"), DtAsientoCabezaCosto, ConexionConsumo) Then Me.Close() : Exit Sub
            If DtAsientoCabezaCosto.Rows.Count <> 0 Then DtAsientoCabezaCosto.Rows(0).Item("Estado") = 3
        End If

        If MsgBox("Consumo se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    If GrabaTabla(DtCabezaAux.GetChanges, "ConsumosPTCabeza", ConexionConsumo) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    If GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionConsumo) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsientoCabezaCosto.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaCosto.GetChanges, "AsientosCabeza", ConexionConsumo) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If

                If Not ActualizaStockLotes(ListaDeLotes, "+") Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If

                Scope.Complete()
                GModificacionOk = True
                MsgBox("Baja Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            End Using
        Catch ex As TransactionException
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        Finally
        End Try

        If Not MuestraDatos() Then Me.Close()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PConsumo = 0
        UnConsumoPT_Load(Nothing, Nothing)

    End Sub
    Private Sub PictureLupaCuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupaCuenta.Click

        If PConsumo <> 0 Then Exit Sub

        If CDec(TextTotalConsumo.Text) = 0 Then
            MsgBox("Falta Informar Importe del Consumo.")
            Exit Sub
        End If

        Dim Neto As Decimal = CDec(TextTotalConsumo.Text)

        SeleccionarCuentaDocumento.PListaDeCuentas = New List(Of ItemCuentasAsientos)

        Dim Item As New ListViewItem

        For I As Integer = 0 To ListCuentas.Items.Count - 1
            Dim Fila As New ItemCuentasAsientos
            Dim CuentaStr As String = ListCuentas.Items.Item(I).SubItems(0).Text
            Fila.Cuenta = Mid(CuentaStr, 1, 3) & Mid(CuentaStr, 5, 6) & Mid(CuentaStr, 12, 2)
            Fila.ImporteB = CDec(ListCuentas.Items.Item(I).SubItems(1).Text)
            Fila.ImporteN = 0
            SeleccionarCuentaDocumento.PListaDeCuentas.Add(Fila)
        Next

        SeleccionarCuentaDocumento.PSoloUnImporte = True
        SeleccionarCuentaDocumento.PImporteB = Neto
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
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PConsumo = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = TipoAsiento
        If PAbierto Then
            ListaAsientos.PDocumentoB = PConsumo
        Else
            ListaAsientos.PDocumentoN = PConsumo
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonNetoPorLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNetoPorLotes.Click

        SeleccionarVarios.PAbierto = PAbierto
        SeleccionarVarios.PConsumo = PConsumo
        SeleccionarVarios.PEsConsumosLotesPT = True
        SeleccionarVarios.Show()

    End Sub
    Private Sub ButtonTextoRecibo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTextoRecibo.Click

        UnTextoParaRecibo.ShowDialog()

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        'http://vb-helper.com/howto_net_print_and_preview.html

        If PConsumo = 0 Then
            MsgBox("Opcion Invalida. Factura debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim print_document As New PrintDocument

        LineasParaImpresion = 0
        LineasParaImpresionImputacion = 0
        Copias = 1

        AddHandler print_document.PrintPage, AddressOf Print_PrintConsumos
        print_document.Print()

    End Sub
    Private Sub Print_PrintConsumos(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 37
        Dim UltimaLinea As Integer
        Dim LineasImpresas As Integer
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        Try
            Print_TituloConsumos(e, MIzq, MTop)

            If LineasParaImpresion < DtDetalle.Rows.Count Then
                Print_Detalle(e, MIzq, MTop, UltimaLinea, LineasImpresas)
            End If
            If LineasParaImpresion < DtDetalle.Rows.Count Then
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
    Private Sub Print_TituloConsumos(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

        Dim Texto As String = ""

        'Encabezado.
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
        Texto = GNombreEmpresa
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
        Texto = "CONSUMOS DE INSUMOS TERMINADOS"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 60, MTop + 10)
        ' 
        PrintFont = New Font("Courier New", 12)
        Texto = "Nro. Consumo:  " & TextConsumo.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
        Texto = "Fecha:  " & DateTime1.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
        Texto = "Costeo: " & ComboCosteo.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 33)
        Texto = "Importe Consumo: " & TextTotalConsumo.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 60, MTop + 40)

    End Sub
    Private Sub Print_Detalle(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer, ByRef UltimaLinea As Integer, ByRef LineasImpresas As Integer)

        Dim Texto As String = ""
        Dim SaltoLinea As Integer = 4
        Dim Longi As Integer
        MIzq = 10
        MTop = 20
        Dim Xq As Integer
        Dim Yq As Integer
        Dim x As Integer = MIzq
        Dim y As Integer = MTop + 50
        Dim Ancho As Integer = 180
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 37

        'Grafica -Rectangulo medios de pagos. ----------------------------------------------------------------------

        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 10)
        Dim LineaDescripcion As Integer = x + 70
        Dim LineaCantidad As Integer = x + 100
        Dim LineaPrecio As Integer = x + 130
        Dim LineaImporte As Integer = x + 180
        'Titulos de descripcion.
        Texto = "DESCRIPCION DEL CONSUMO"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, y - 4)
        Texto = "DESCRIPCION"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 2, y + 2)
        Texto = "CANTIDAD"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaCantidad - Longi - 10
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "PRECIO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaPrecio - Longi - 10
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "IMPORTE"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaImporte - Longi - 20
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        'Descripcion del pago.
        Yq = y + SaltoLinea
        While Contador < LineasPorPagina And LineasParaImpresion < DtDetalle.Rows.Count
            Dim Row As DataRow = DtDetalle.Rows(LineasParaImpresion)
            Yq = Yq + SaltoLinea
            'Imprime Detalle.
            Texto = Strings.Left(NombreArticulo(Row("Articulo")), 30)
            Xq = x
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            'Imprime Cantidad.
            Texto = FormatNumber(Row("Cantidad"), GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            'Imprime Precio.
            Texto = FormatNumber(Row("Precio"), GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaPrecio - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            'Imprime Importe.
            Texto = FormatNumber(Row("TotalArticulo"), GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Contador = Contador + 1
            LineasParaImpresion = LineasParaImpresion + 1
        End While

        Yq = Yq + SaltoLinea

        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Yq - y)
        'Lineas vertical.
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaDescripcion, y, LineaDescripcion, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCantidad, y, LineaCantidad, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaPrecio, y, LineaPrecio, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y + 2 * SaltoLinea, x + Ancho, y + 2 * SaltoLinea)

        UltimaLinea = Yq
        LineasImpresas = Contador

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then
            MsgBox("No hay un registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        IndiceCoincidencia = Grid.CurrentRow.Cells("Indice").Value
        Grid.Rows.Remove(Grid.CurrentRow)
        CalculaSubTotal()

    End Sub
    Private Sub RadioFinal_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        CalculaSubTotal()

    End Sub
    Private Sub RadioPorUnidad_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioPorUnidad.CheckedChanged

        If RadioPorUnidad.Checked = False Then
            Grid.Columns("Precio").HeaderText = "Precio xKgs"
        Else : Grid.Columns("Precio").HeaderText = "Precio xUni"
        End If

        CalculaSubTotal()

    End Sub
    Private Function MuestraDatos() As Boolean

        Dim Sql As String

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtCabeza = New DataTable
        Sql = "SELECT * FROM ConsumosPTCabeza WHERE Consumo = " & PConsumo & ";"
        If Not Tablas.Read(Sql, ConexionConsumo, DtCabeza) Then Return False
        If PConsumo <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Consumo No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return False
        End If

        If PConsumo = 0 Then
            If Not AgregaCabeza() Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        End If

        EnlazaCabeza()

        DtDetalle = New DataTable
        Sql = "SELECT * FROM ConsumosPTDetalle WHERE Consumo = " & PConsumo & ";"
        If Not Tablas.Read(Sql, ConexionConsumo, DtDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False

        Grid.DataSource = bs
        bs.DataSource = DtDetalle

        Grid.Columns("Indice").Visible = False
        Grid.Columns("Precio").HeaderText = "Precio xUni"

        Dim TotalNeto As Decimal = 0

        For I As Integer = 0 To Grid.Rows.Count - 2
            Grid.Rows(I).Cells("Neto").Value = CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("Precio").Value)
            TotalNeto = TotalNeto + Grid.Rows(I).Cells("Neto").Value
            HallaAGranelYMedida(Grid.Rows(I).Cells("Articulo").Value, Grid.Rows(I).Cells("AGranel").Value, Grid.Rows(I).Cells("Medida").Value)
        Next

        TextTotalConsumo.Text = FormatNumber(TotalNeto, GDecimales)

        'Arma tabla con AsignacionLotes del Consumo. 
        ConsumoLotes = New DataTable
        ListaDeLotes = New List(Of FilaAsignacion)
        Sql = "SELECT * FROM ConsumosPTLotes WHERE Consumo = " & PConsumo & ";"
        If Not Tablas.Read(Sql, ConexionConsumo, ConsumoLotes) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        If ConsumoLotes.Rows.Count <> 0 Then
            For Each Row As DataRow In ConsumoLotes.Rows
                Dim Fila As New FilaAsignacion
                Fila.Indice = Row("Indice")
                Fila.Lote = Row("Lote")
                Fila.Secuencia = Row("Secuencia")
                Fila.Operacion = Row("Operacion")
                Fila.Deposito = Row("Deposito")
                Fila.Asignado = Row("Cantidad")
                Fila.Importe = Row("Importe")
                ListaDeLotes.Add(Fila)
            Next
        End If

        If PConsumo <> 0 Then
            ButtonAceptar.Text = "Graba Consumo"
            ButtonAceptar.Enabled = False
            Panel4.Enabled = False
            ButtonEliminarLinea.Enabled = False
            Grid.Columns("Cantidad").ReadOnly = True
            Grid.Columns("Articulo").ReadOnly = True
            Grid.Columns("KilosXunidad").ReadOnly = True
            Grid.Columns("PrecioLista").ReadOnly = True
            Grid.Columns("PrecioLista").HeaderText = ""
        Else : ButtonAceptar.Text = "Graba Consumo"
            ButtonAceptar.Enabled = True
            Panel4.Enabled = True
            ButtonEliminarLinea.Enabled = True
            Grid.Columns("Cantidad").ReadOnly = False
            Grid.Columns("Articulo").ReadOnly = False
            Grid.Columns("KilosXunidad").ReadOnly = False
            Grid.Columns("PrecioLista").ReadOnly = False
            Grid.Columns("PrecioLista").HeaderText = "Precio Lista"
        End If

        AddHandler DtDetalle.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtdetalle_ColumnChanging)
        AddHandler DtDetalle.ColumnChanged, New DataColumnChangeEventHandler(AddressOf Dtdetalle_ColumnChanged)
        AddHandler DtDetalle.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtDetalle_NewRow)
        AddHandler DtDetalle.RowChanged, New DataRowChangeEventHandler(AddressOf DtDetalle_RowChanged)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub EnlazaCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Row As DataRowView = MiEnlazador.Current

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Consumo")
        TextConsumo.DataBindings.Clear()
        TextConsumo.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Deposito")
        ComboDeposito.DataBindings.Clear()
        ComboDeposito.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalConsumo.DataBindings.Clear()
        TextTotalConsumo.DataBindings.Add(Enlace)

        If Row("Costeo") <> 0 Then
            ComboNegocio.SelectedValue = HallaNegocio(Row("Costeo"))
            Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & ComboNegocio.SelectedValue & ";"
            ComboCosteo.DataSource = New DataTable
            ComboCosteo.DataSource = Tablas.Leer(Sql)
            Dim Row1 As DataRow = ComboCosteo.DataSource.newrow
            Row1("Costeo") = 0
            Row1("Nombre") = ""
            ComboCosteo.DataSource.rows.add(Row1)
            ComboCosteo.DisplayMember = "Nombre"
            ComboCosteo.ValueMember = "Costeo"
            ComboCosteo.SelectedValue = 0
            With ComboCosteo
                .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                .AutoCompleteSource = AutoCompleteSource.ListItems
            End With
        End If

        Enlace = New Binding("SelectedValue", MiEnlazador, "Costeo")
        ComboCosteo.DataBindings.Clear()
        ComboCosteo.DataBindings.Add(Enlace)

        If Row("PorUnidad") Then
            RadioPorUnidad.Checked = True
        Else : RadioPorKilo.Checked = True
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If IsDBNull(Numero.Value) Then Numero.Value = 0
        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow
        Dim DtArticulo As New DataTable

        If PConsumo <> 0 Then
            DtArticulo = TodosLosArticulos()
        End If

        If PConsumo = 0 Then
            DtArticulo = ArticulosActivos()
        End If

        Articulo.DataSource = DtArticulo
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Function Coincidencia(ByVal Fila As FilaAsignacion) As Boolean

        If Fila.Indice = IndiceCoincidencia Then Return True

    End Function
    Private Function AgregaCabeza() As Boolean

        Dim Row As DataRow

        Row = DtCabeza.NewRow()
        Row("Consumo") = 0
        Row("Costeo") = Costeo
        Row("Deposito") = ComboDeposito.SelectedValue
        Row("Fecha") = Now
        Row("Estado") = 1
        Row("PorUnidad") = True
        Row("Importe") = 0
        Row("Comentario") = ""
        DtCabeza.Rows.Add(Row)

        Return True

    End Function
    Private Sub CalculaSubTotal()

        If PConsumo <> 0 Then Exit Sub

        Dim Precio As Decimal = 0
        Dim TotalNeto As Decimal = 0
        Dim Total As Decimal = 0

        For I As Integer = 0 To Grid.Rows.Count - 2
            Precio = Trunca3(Grid.Rows(I).Cells("PrecioLista").Value)
            If RadioPorUnidad.Checked Then
            Else
                Precio = Trunca3(Grid.Rows(I).Cells("PrecioLista").Value * Grid.Rows(I).Cells("KilosXUnidad").Value)
            End If
            '
            Grid.Rows(I).Cells("Precio").Value = Precio
            Grid.Rows(I).Cells("Neto").Value = CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Precio)
            TotalNeto = TotalNeto + Grid.Rows(I).Cells("Neto").Value
            '
            Grid.Rows(I).Cells("TotalArticulo").Value = Trunca(Grid.Rows(I).Cells("Neto").Value)
            Total = Total + Grid.Rows(I).Cells("TotalArticulo").Value
        Next

        TextTotalConsumo.Text = FormatNumber(Total, GDecimales)

    End Sub
    Private Sub HacerAlta()

        Dim DtCabezaB As New DataTable
        Dim DtDetalleB As New DataTable
        Dim ConsumoLotesB As New DataTable
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        'Para Costo.
        Dim DtAsientoCabezaCosto As New DataTable
        Dim DtAsientoDetalleCosto As New DataTable
        '
        DtCabezaB = DtCabeza.Clone
        DtDetalleB = DtDetalle.Copy
        ConsumoLotesB = ConsumoLotes.Clone

        ArmaArchivoParaAlta(DtCabezaB, DtDetalleB)

        AsignaLotesDeConsumos(DtCabezaB, ListaDeLotes, ConsumoLotesB, DtDetalleB)

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma Asientos.
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Me.Close() : Exit Sub
            If Not ArmaArchivosAsiento("A", DtDetalleB, DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            DtAsientoCabezaCosto = DtAsientoCabeza.Clone
            DtAsientoDetalleCosto = DtAsientoDetalle.Clone
            If Not ArmaArchivosAsientoCosto("A", DtDetalleB, DtAsientoCabezaCosto, DtAsientoDetalleCosto) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        'Graba Facturas.
        Dim NumeroConsumo As Integer = 0
        Dim NumeroAsiento As Integer = 0
        Dim NumeroW As Integer

        For i As Integer = 1 To 50
            'Halla ultima numeracion Consumo.
            NumeroConsumo = UltimoNumeroConsumo(ConexionConsumo)
            If NumeroConsumo < 0 Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionConsumo)
                If NumeroAsiento < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            'Actualiza numeracion consumo.
            DtCabezaB.Rows(0).Item("Consumo") = NumeroConsumo
            For Each Row As DataRow In DtDetalleB.Rows
                Row("Consumo") = NumeroConsumo
            Next
            For Each Row As DataRow In ConsumoLotesB.Rows
                Row("Consumo") = NumeroConsumo
            Next
            'Actualiza Asientos.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = NumeroConsumo
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
                DtAsientoCabezaCosto.Rows(0).Item("Asiento") = NumeroAsiento + 1
                DtAsientoCabezaCosto.Rows(0).Item("Documento") = NumeroConsumo
                For Each Row As DataRow In DtAsientoDetalleCosto.Rows
                    Row("Asiento") = NumeroAsiento + 1
                Next
            End If

            NumeroW = AltaConsumo(DtCabezaB, DtDetalleB, ConsumoLotesB, DtAsientoCabeza, DtAsientoDetalle, DtAsientoCabezaCosto, DtAsientoDetalleCosto, ConexionConsumo)

            Me.Cursor = System.Windows.Forms.Cursors.Default

            If NumeroW >= 0 Then Exit For
            If NumeroW = -2 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If NumeroW = -2 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If NumeroW > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PConsumo = DtCabezaB.Rows(0).Item("Consumo")
            GModificacionOk = True
            If Not MuestraDatos() Then Me.Close() : Exit Sub
        End If

    End Sub
    Private Sub ArmaArchivoParaAlta(ByVal DtCabeza As DataTable, ByVal DtDetalle As DataTable)

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow

        'Prepara Detalle.
        For I As Integer = 0 To Grid.Rows.Count - 2
            ''''          RowsBusqueda = DtDetalle.Select("Indice = " & Grid.Rows(I).Cells("Indice").Value)
            ''''RowsBusqueda(0).Item("Iva") = Grid.Rows(I).Cells("Iva").Value
            ''''RowsBusqueda(0).Item("Precio") = Grid.Rows(I).Cells("PrecioBlanco").Value
            '''' RowsBusqueda(0).Item("TotalArticulo") = Trunca(Grid.Rows(I).Cells("NetoBlanco").Value + Grid.Rows(I).Cells("MontoIva").Value)
        Next

        'Prepara Cabeza.
        Row = DtCabeza.NewRow
        Row("Consumo") = 0
        Row("Importe") = CDec(TextTotalConsumo.Text)
        Row("Fecha") = DateTime1.Value
        Row("Costeo") = ComboCosteo.SelectedValue
        Row("Deposito") = ComboDeposito.SelectedValue
        Row("Estado") = 1
        Row("PorUnidad") = RadioPorUnidad.Checked
        Row("Comentario") = TextComentario.Text.Trim
        DtCabeza.Rows.Add(Row)

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtConsumoDetalle As DataTable, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow

        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Asignado <> 0 Then
                Dim Tipo As Integer
                Dim Centro As Integer
                Dim Fila2 As New ItemLotesParaAsientos
                Dim ConexionLote As String
                If Fila.Operacion = 1 Then
                    ConexionLote = Conexion
                Else : ConexionLote = ConexionN
                End If
                HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, ConexionLote, Tipo, Centro)
                If Centro <= 0 Then
                    MsgBox("Error en Tipo Operacion en Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                    Return False
                End If
                RowsBusqueda = DtConsumoDetalle.Select("Indice = " & Fila.Indice)
                Dim Importe As Decimal = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                Fila2.Centro = Centro
                Fila2.MontoNeto = Importe         'Para Consumo.
                If Tipo = 1 Then Fila2.Clave = -101 'consignacion
                If Tipo = 2 Then Fila2.Clave = -100 'reventa
                If Tipo = 3 Then Fila2.Clave = -102 'reventa MG
                If Tipo = 4 Then Fila2.Clave = -103 'costeo
                ListaLotesParaAsiento.Add(Fila2)
                '
                ''''''  Fila2 = New ItemLotesParaAsientos  'Para Baja.
                '''''  Fila2.Centro = Centro
                '''''     Fila2.MontoNeto = Importe
                '''''     If Tipo = 1 Then Fila2.Clave = -201 'consignacion
                '''''   If Tipo = 2 Then Fila2.Clave = -200 'reventa
                '''''    If Tipo = 3 Then Fila2.Clave = -202 'reventa MG
                '''''    If Tipo = 4 Then Fila2.Clave = -203 'costeo
                '''''  ListaLotesParaAsiento.Add(Fila2)
            End If
        Next

        'Arma Lista con Cuentas definidas en documento.
        If ListCuentas.Visible Then
            Dim Neto As Decimal = CDec(TextTotalConsumo.Text)
            '
            If ListCuentas.Items.Count = 0 Then
                MsgBox("Falta Informar Cuenta. Operación se CANCELA.")
                Return False
            End If
            Dim ImporteLista As Decimal = 0
            For I As Integer = 0 To ListCuentas.Items.Count - 1
                Dim Fila As New ItemCuentasAsientos
                Dim Cuenta As String = ListCuentas.Items.Item(I).SubItems(0).Text
                Fila.Cuenta = Mid$(Cuenta, 1, 3) & Mid$(Cuenta, 5, 6) & Mid$(Cuenta, 12, 2)
                Fila.Importe = CDec(ListCuentas.Items.Item(I).SubItems(1).Text)
                Fila.Clave = 213
                ListaCuentas.Add(Fila)
                ImporteLista = ImporteLista + Fila.Importe
            Next
            If ImporteLista <> Neto Then
                MsgBox("Importe de Cuentas Informada Difiere del Neto de la Factura. " & ImporteLista & " " & Neto)
                Return False
            End If
        End If

        Dim Item As New ItemListaConceptosAsientos

        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = CDec(TextTotalConsumo.Text)
        ListaConceptos.Add(Item)

        If Funcion = "A" Then
            If Not Asiento(TipoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 61000) Then Return False
        End If

        Return True

    End Function
    Private Function ArmaArchivosAsientoCosto(ByVal Funcion As String, ByVal DtConsumoDetalle As DataTable, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow

        Dim Precio As Decimal = 0
        Dim Tipo As Integer
        Dim Centro As Integer
        Dim ImporteTotal As Decimal
        Dim KilosNoAsignado As Decimal = 0
        Dim Item As New ItemListaConceptosAsientos

        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Asignado <> 0 Then
                Tipo = 0
                Centro = 0
                Dim Fila2 As New ItemLotesParaAsientos
                '
                RowsBusqueda = DtConsumoDetalle.Select("Indice = " & Fila.Indice)
                '
                If Fila.Operacion = 1 Then
                    HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, Conexion, Tipo, Centro)
                Else : HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, ConexionN, Tipo, Centro)
                End If
                If Centro <= 0 Then
                    MsgBox("Error en Tipo Operacion en Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                    Return False
                End If
                If Tipo = 4 Then
                    Dim Negocio As Integer = HallaProveedorLote(Fila.Operacion, Fila.Lote, Fila.Secuencia)
                    If Negocio <= 0 Then
                        MsgBox("Error al Leer Lotes " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                        Return False
                    End If
                    Dim Costeo = HallaCosteoLote(Fila.Operacion, Fila.Lote)
                    If Costeo = -1 Then Return False
                    If Not HallaPrecioYCentroCosteo(Negocio, Costeo, Centro, Precio) Then Return False
                Else
                    Precio = Refe
                End If
                Dim Kilos As Double
                Dim Iva As Double
                HallaKilosIva(RowsBusqueda(0).Item("Articulo"), Kilos, Iva)
                '
                Fila2.Centro = Centro
                Fila2.MontoNeto = Trunca(Precio * Fila.Asignado * Kilos)
                ImporteTotal = ImporteTotal + Fila2.MontoNeto
                KilosNoAsignado = Trunca(KilosNoAsignado + (Fila.Asignado * Kilos))
                If Tipo = 1 Then Fila2.Clave = -201 'consignacion
                If Tipo = 2 Then Fila2.Clave = -200 'reventa
                If Tipo = 3 Then Fila2.Clave = -202 'reventa MG
                If Tipo = 4 Then Fila2.Clave = -203 'costeo
                ListaLotesParaAsiento.Add(Fila2)
            End If
        Next
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 202
        If Funcion = "A" Then Item.Importe = -Trunca(Refe * KilosNoAsignado)
        If Funcion = "B" Then Item.Importe = Trunca(Refe * KilosNoAsignado)
        '    ListaConceptos.Add(Item)

        If Not Asiento(TipoAsientoCosto, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 61000) Then Return False

        Return True

    End Function
    Private Function AltaConsumo(ByVal DtCabezaB As DataTable, ByVal DtDetalleB As DataTable, ByVal ConsumoLotesB As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtAsientoCabezaCosto As DataTable, ByVal DtAsientoDetalleCosto As DataTable, ByVal ConexionStr As String) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                ' graba consumo.
                If Not IsNothing(DtCabezaB.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaB.GetChanges, "ConsumosPTCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtDetalleB.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleB.GetChanges, "ConsumosPTDetalle", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(ConsumoLotesB.GetChanges) Then
                    Resul = GrabaTabla(ConsumoLotesB.GetChanges, "ConsumosPTLotes", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If

                ' graba Asiento.
                If DtAsientoCabeza.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionStr)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoCabezaCosto.GetChanges, "AsientosCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalleCosto.GetChanges, "AsientosDetalle", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If

                ' Actualiza Stock.
                If Not ActualizaStockLotes(ListaDeLotes, "-") Then Return -3

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        Finally
        End Try

    End Function
    Private Sub ArmaAsignacionLotes(ByVal DtDetalleWB As DataTable, ByVal DtLotesWB As DataTable, ByVal ComprobanteWB As Double, ByVal DtDetalleWN As DataTable, ByVal DtLotesWN As DataTable, ByVal ComprobanteWN As Double)

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow

        If DtDetalleWB.Rows.Count <> 0 Then
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Asignado <> 0 Then
                    Row = DtLotesWB.NewRow()
                    Row("TipoComprobante") = 2
                    Row("Comprobante") = ComprobanteWB
                    Row("Indice") = Fila.Indice
                    Row("Lote") = Fila.Lote
                    Row("Secuencia") = Fila.Secuencia
                    Row("Deposito") = Fila.Deposito
                    Row("Operacion") = Fila.Operacion
                    Row("Cantidad") = Fila.Asignado
                    Row("Rel") = False
                    RowsBusqueda = DtDetalleWB.Select("Indice = " & Fila.Indice)
                    Row("ImporteSinIva") = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                    Row("Importe") = Trunca(Row("ImporteSinIva") + CalculaIva(Fila.Asignado, RowsBusqueda(0).Item("Precio"), RowsBusqueda(0).Item("Iva")))
                    Row("Facturado") = False
                    Row("Liquidado") = False
                    DtLotesWB.Rows.Add(Row)
                End If
            Next
        End If

        If DtDetalleWN.Rows.Count <> 0 Then
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Asignado <> 0 Then
                    Row = DtLotesWN.NewRow()
                    Row("TipoComprobante") = 2
                    Row("Comprobante") = ComprobanteWN
                    Row("Indice") = Fila.Indice
                    Row("Lote") = Fila.Lote
                    Row("Secuencia") = Fila.Secuencia
                    Row("Deposito") = Fila.Deposito
                    Row("Operacion") = Fila.Operacion
                    Row("Cantidad") = Fila.Asignado
                    Row("Rel") = False
                    RowsBusqueda = DtDetalleWN.Select("Indice = " & Fila.Indice)
                    Row("ImporteSinIva") = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                    Row("Importe") = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                    Row("Facturado") = False
                    Row("Liquidado") = False
                    DtLotesWN.Rows.Add(Row)
                End If
            Next
        End If

        If DtLotesWB.Rows.Count <> 0 And DtLotesWN.Rows.Count <> 0 Then
            For Each Row In DtLotesWB.Rows
                Row("Rel") = True
            Next
            For Each Row In DtLotesWN.Rows
                Row("Rel") = True
            Next
        End If

    End Sub
    Private Function BuscaIndice(ByVal Indice As Integer) As Boolean

        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Indice = Indice Then Return True
        Next

        Return False

    End Function
    Public Function UltimoNumeroConsumo(ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Consumo) FROM ConsumosPTCabeza;", Miconexion)
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
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM ConsumosPTCabeza;", Miconexion)
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
    Private Sub PideDatosCosteo()

        OpcionCosteo.ShowDialog()
        If Not OpcionCosteo.PRegresar Then
            Costeo = OpcionCosteo.PCosteo
            ComboDeposito.SelectedValue = OpcionCosteo.PDeposito
            PAbierto = OpcionCosteo.PAbierto
            DateTime1.Value = OpcionCosteo.PFecha
        Else
            Costeo = 0
        End If
        OpcionCosteo.Dispose()

    End Sub
    Public Sub AsignaLotesDeConsumos(ByVal DtCabeza As DataTable, ByVal ListaDeLotes As List(Of FilaAsignacion), ByRef ConsumoLotesAux As DataTable, ByVal DtDetalle As DataTable)

        Dim RowsBusqueda() As DataRow

        For Each Fila As FilaAsignacion In ListaDeLotes
            Dim Row As DataRow = ConsumoLotesAux.NewRow()
            Row("Consumo") = 0
            Row("Indice") = Fila.Indice
            Row("Lote") = Fila.Lote
            Row("Secuencia") = Fila.Secuencia
            Row("Operacion") = Fila.Operacion
            Row("Deposito") = Fila.Deposito
            Row("Cantidad") = Fila.Asignado
            RowsBusqueda = DtDetalle.Select("Indice = " & Fila.Indice)
            Row("Importe") = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
            ConsumoLotesAux.Rows.Add(Row)
        Next

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 And PConsumo = 0 Then
            MsgBox("Fecha Incorrecta. Existe Otro Comprobante con Fecha " & UltimaFechaW & " Posterior a la Informada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If

        If ComboCosteo.SelectedValue = 0 Then
            MsgBox("Falta Costeo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboCosteo.Focus()
            Return False
        End If

        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Return False
        End If

        If Grid.Rows.Count = 1 Then
            MsgBox("Falta Informar Articulos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        Dim Fecha As Date = DtCabeza.Rows(0).Item("Fecha")

        If CierreContableCerrado(Fecha.Month, Fecha.Year) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        For i As Integer = 0 To Grid.Rows.Count - 2
            If Not IsNumeric(Grid.Rows(i).Cells("Cantidad").Value) Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Grid.Rows(i).Cells("Cantidad").Value = 0 Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If TieneDecimales(Grid.Rows(i).Cells("Cantidad").Value) And Not Grid.Rows(i).Cells("AGranel").Value Then
                MsgBox("Cantidad no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            If Not IsNumeric(Grid.Rows(i).Cells("KilosXUnidad").Value) Then
                MsgBox("Debe Informar Kilos por Unidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Grid.Rows(i).Cells("KilosXUnidad").Value = 0 Then
                MsgBox("Debe Informar Kilos por Unidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            If Not IsNumeric(Grid.Rows(i).Cells("Precio").Value) Then
                MsgBox("Debe Informar Precio en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Grid.Rows(i).Cells("Precio").Value = 0 Then
                MsgBox("Debe Informar Precio en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            If Grid.Rows(i).Cells("TotalArticulo").Value = 0 Then
                MsgBox("Precio Debe Tener por lo menos 2 Digitos Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        Next

        Dim Cantidad As Decimal, CantidadAsignada As Decimal

        For i As Integer = 0 To Grid.Rows.Count - 1
            Cantidad = Cantidad + Grid.Rows(i).Cells("Cantidad").Value
        Next

        For Each Fila As FilaAsignacion In ListaDeLotes
            CantidadAsignada = CantidadAsignada + Fila.Asignado
        Next

        If CantidadAsignada = 0 Then
            MsgBox("Debe Asingnar Lotes. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If CantidadAsignada <> Cantidad Then
            MsgBox("Cantidad Lotes Asingnados no Corresponde con Cantidad de Articulos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
            End If
        End If

    End Sub
    Private Sub Grid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.CellBeginEdit

        If Grid.Columns(e.ColumnIndex).Name = "PrecioLista" Or Grid.Columns(e.ColumnIndex).Name = "KilosXUnidad" Or Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Articulo").Value) Then
                If Grid.CurrentRow.Cells("Articulo").Value = 0 Then e.Cancel = True
            End If
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        Dim Row As DataGridViewRow = Grid.CurrentRow

        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then            'Completa el grid con datos para ser utilizados en el Recalculo de cantidad.
            HallaAGranelYMedida(Row.Cells("Articulo").Value, Row.Cells("AGranel").Value, Row.Cells("Medida").Value)
        End If

        'Tiene que ir delante que cualquier codigo que utilize la columna "Cantidad". De lo contrario no se actulizara correctamente.
        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then    'Recalcula Cantidad segun sea a granel.
            CalculaSubTotal()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "PrecioLista" Then
            If IsDBNull(Row.Cells("Precio").Value) Then Row.Cells("PrecioLista").Value = 0
            CalculaSubTotal()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "KilosXUnidad" Then
            If IsDBNull(Row.Cells("KilosXUnidad").Value) Then Row.Cells("KilosXUnidad").Value = 0
            If PConsumo = 0 Then CalculaSubTotal()
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

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioLista" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "KilosXUnidad" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "TotalArticulo" Then
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

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "PrecioLista" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 3)
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "KilosXUnidad" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 4)
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If IsNumeric(e.Value) Then
            If e.Value = 0 Then e.Value = Format(0, "#") : Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Grid.Rows(e.RowIndex).Cells("Articulo").Value = 0 Then Exit Sub
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Indice").Value) And Not IsNothing(Grid.Rows(e.RowIndex).Cells("Indice").Value) Then
                If BuscaIndice(Grid.Rows(e.RowIndex).Cells("Indice").Value) Then
                    e.Value = "  Asignados  " : e.CellStyle.ForeColor = Color.Green
                Else
                    e.Value = "No Asignado"
                    e.CellStyle.ForeColor = Color.Red
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Precio" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Precio").Value) Then
                e.Value = FormatNumber(e.Value, 3)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "PrecioLista" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("PrecioLista").Value) Then
                e.Value = FormatNumber(e.Value, 3)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Neto" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Neto").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "TotalArticulo" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("TotalArticulo").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LupaLotes" Then
            e.Value = ImageList1.Images.Item("Lupa")
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name <> "LupaLotes" Then Exit Sub

        If PConsumo <> 0 Then
            MuestraLotesAsignados.PArticulo = Grid.CurrentRow.Cells("Articulo").Value
            MuestraLotesAsignados.PIndice = CInt(Grid.CurrentRow.Cells("Indice").Value)
            MuestraLotesAsignados.PLista = ListaDeLotes
            MuestraLotesAsignados.ShowDialog()
            MuestraLotesAsignados.Dispose()
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Cantidad").Value = 0 Then
            MsgBox("Debe Informar Cantidad a Aignar.", MsgBoxStyle.Information)
            Exit Sub
        End If

        SeleccionaLotesAAsignar.PEsConPrecio = True
        SeleccionaLotesAAsignar.PArticulo = Grid.CurrentRow.Cells("Articulo").Value
        SeleccionaLotesAAsignar.PDeposito = DtCabeza.Rows(0).Item("Deposito")
        SeleccionaLotesAAsignar.PCantidad = Grid.CurrentRow.Cells("Cantidad").Value
        SeleccionaLotesAAsignar.PComprobante = PConsumo
        SeleccionaLotesAAsignar.PConexion = ConexionConsumo
        SeleccionaLotesAAsignar.PIndice = CInt(Grid.CurrentRow.Cells("Indice").Value)
        SeleccionaLotesAAsignar.PLista = ListaDeLotes
        SeleccionaLotesAAsignar.PListaOriginal = New List(Of FilaAsignacion)
        SeleccionaLotesAAsignar.ShowDialog()
        '
        ListaDeLotes = SeleccionaLotesAAsignar.PLista
        Grid.Rows(e.RowIndex).Cells("PrecioLista").Value = SeleccionaLotesAAsignar.PPrecio : CalculaSubTotal()

        SeleccionaLotesAAsignar.Dispose()

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtDetalle_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        IndiceW = IndiceW + 1
        e.Row("Indice") = IndiceW
        e.Row("Consumo") = 0
        e.Row("Articulo") = 0
        e.Row("KilosXUnidad") = 0
        e.Row("Precio") = 0
        e.Row("Cantidad") = 0
        e.Row("TotalArticulo") = 0

    End Sub
    Private Sub Dtdetalle_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Articulo")) Then
            If Not IsDBNull(e.Row("Articulo")) Then
                Dim Kilos As Decimal
                Dim Iva As Decimal
                HallaKilosIva(e.ProposedValue, Kilos, Iva)
                e.Row("KilosXUnidad") = Kilos
                Grid.Refresh()
            End If
        End If

    End Sub
    Private Sub Dtdetalle_ColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Articulo")) Then
            If e.ProposedValue <> 0 Then
                If DtDetalle.Rows.Count + 1 > GLineasFacturas And Not PermiteMuchosArticulos Then
                    MsgBox("Supera Cantidad Articulos Permitidos.(" & GLineasFacturas & ")", MsgBoxStyle.Information)
                    Dim Row As DataRowView = bs.Current
                    Row.Delete()
                End If
            End If
        End If

    End Sub
    Private Sub DtDetalle_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        'Borra las lineas en blancos que aparecen cuando hago click en el ultimo renglon y sin informar nada regreso a algun renglon anterior. 
        If e.Row("Articulo") = 0 And e.Row("Cantidad") = 0 And e.Row("Precio") = 0 Then e.Row.Delete()

    End Sub

End Class
