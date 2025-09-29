Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnIngresoArticulosLogisticos
    Public PIngreso As Integer
    Public PAbierto As Boolean
    Public PEsSaldoInicial As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Private DtIngresoCabeza As DataTable
    Private DtIngresoDetalle As DataTable
    Private DtGrid As DataTable

    Dim cb As ComboBox
    Dim ConexionIngreso As String
    Dim RemitoAnt As Double = 0
    Dim GuiaAnt As Double = 0
    Dim Emisor As Integer
    Dim Deposito As Integer
    Dim UltimaFechaW As Date
    'Para impresion.
    Dim ErrorImpresion As Boolean
    Dim LineasParaImpresion As Integer = 0
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Private Sub UnIngresoMercaderia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If Not PermisoEscritura(12) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False

        LlenaComboTablas(ComboDeposito, 20)
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        If PIngreso = 0 Then
            If Not Opcion() Then Me.Close() : Exit Sub
        End If

        LlenaCombosGrid()

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PAbierto Then
            ConexionIngreso = Conexion
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionIngreso = ConexionN
        End If

        GModificacionOk = False

        ArmaArchivos()

        If PEsSaldoInicial Then
            MaskedRemito.Visible = False
            Label1.Visible = False
            TextGuia.Visible = False
            Label2.Visible = False
            Me.Text = "Saldo Iniciales Artículos Logísticos"
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Ingreso esta Anulado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtIngresoCabezaAux As DataTable = DtIngresoCabeza.Copy
        Dim DtIngresoDetalleAux As DataTable = DtIngresoDetalle.Copy
        Dim DtStock As New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not ArmaStcok(DtGrid, DtIngresoDetalleAux, DtStock) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        If Not ActualizaArchivos(DtIngresoCabezaAux, DtIngresoDetalleAux, DtStock) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        If IsNothing(DtIngresoDetalleAux.GetChanges) And IsNothing(DtIngresoCabezaAux.GetChanges) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No hay Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PIngreso = 0 Then
            HacerAlta(DtIngresoCabezaAux, DtIngresoDetalleAux, DtStock)
        Else : HacerModificacion(DtIngresoCabezaAux, DtIngresoDetalleAux, DtStock)
        End If

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
            MsgBox("Nota ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If IsNothing(DtGrid.GetChanges) Then
            MsgBox("Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtIngresoCabezaAux As DataTable = DtIngresoCabeza.Copy
        Dim DtIngresoDetalleAux As DataTable = DtIngresoDetalle.Copy
        Dim DtStock As New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not ArmaStcok(DtGrid, DtIngresoDetalleAux, DtStock) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        If Not ActualizaArchivos(DtIngresoCabezaAux, DtIngresoDetalleAux, DtStock) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        If Not IsNothing(DtIngresoDetalleAux.GetChanges) Or Not IsNothing(DtIngresoCabezaAux.GetChanges) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim RowsBusqueda() As DataRow
        Dim OK As Boolean = True

        For Each Row1 As DataRow In DtGrid.Rows
            RowsBusqueda = DtStock.Select("Articulo = " & Row1("Articulo"))
            RowsBusqueda(0).Item("Stock") = CDec(RowsBusqueda(0).Item("Stock")) - Row1("Cantidad")
            If RowsBusqueda(0).Item("Stock") < 0 Then
                MsgBox("Baja Hace al Stock Negativo en el Artiuclo " & NombreArticuloLogistico(Row1("Articulo")) & ". Operación se CANCELA.")
                OK = False
            End If
        Next
        If Not OK Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        If MsgBox("El Ingreso se Anulara. ¿Desea Anularlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtIngresoCabezaAux.Rows(0).Item("Estado") = 3

        Dim NumeroW As Integer = ActualizaIngreso(DtIngresoCabezaAux, DtIngresoDetalleAux, DtStock)

        If NumeroW = -1 Or NumeroW = -2 Then
            MsgBox("ERROR, Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            GModificacionOk = True
            MsgBox("Baja Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        'http://vb-helper.com/howto_net_print_and_preview.html

        If PIngreso = 0 Then
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
    Private Sub TextGuia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextGuia.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Row As DataRow

        CreaDtGrid()

        DtIngresoCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM IngresoArticulosLogisticosCabeza WHERE Ingreso = " & PIngreso & ";", ConexionIngreso, DtIngresoCabeza) Then Me.Close() : Exit Sub
        If PIngreso <> 0 And DtIngresoCabeza.Rows.Count = 0 Then
            MsgBox("Ingreso No Existen En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If DtIngresoCabeza.Rows.Count <> 0 Then
            Emisor = DtIngresoCabeza.Rows(0).Item("Proveedor")
        End If

        LlenaDatosEmisor(Emisor)

        If DtIngresoCabeza.Rows.Count = 0 Then
            Row = DtIngresoCabeza.NewRow
            Row("Ingreso") = 0
            Row("Proveedor") = Emisor
            Row("Remito") = 0
            Row("Guia") = 0
            Row("Deposito") = Deposito
            Row("Fecha") = Now
            Row("Comentario") = ""
            Row("Estado") = 1
            Row("Pagado") = False
            DtIngresoCabeza.Rows.Add(Row)
        End If

        RemitoAnt = DtIngresoCabeza.Rows(0).Item("Remito")
        GuiaAnt = DtIngresoCabeza.Rows(0).Item("Guia")

        MuestraCabeza()

        DtIngresoDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM IngresoArticulosLogisticosDetalle WHERE Ingreso = " & PIngreso & ";", ConexionIngreso, DtIngresoDetalle) Then Me.Close() : Exit Sub

        For Each Row1 As DataRow In DtIngresoDetalle.Rows
            Row = DtGrid.NewRow
            Row("Ingreso") = Row1("Ingreso")
            Row("Articulo") = Row1("Articulo")
            Row("Cantidad") = Row1("Cantidad")
            Row("AGranel") = False
            Row("Medida") = ""
            HallaAGranelYMedidaLogistico(Row1("Articulo"), Row("AGranel"), Row("Medida"))
            DtGrid.Rows.Add(Row)
        Next

        Grid.EndEdit()
        Grid.DataSource = bs
        bs.DataSource = DtGrid

        For I As Integer = 0 To Grid.Rows.Count - 2
            Grid.Rows(I).Cells("Articulo").ReadOnly = True
        Next

        If PIngreso = 0 Then
            ButtonAceptar.Text = "Graba Ingreso"
        Else
            ButtonAceptar.Text = "Modifica Ingreso"
        End If

        UltimaFechaW = UltimaFecha(ConexionIngreso)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

        If PIngreso <> 0 Then
            If DtIngresoCabeza.Rows(0).Item("Remito") = 0 And DtIngresoCabeza.Rows(0).Item("Guia") = 0 Then
                MaskedRemito.Visible = False
                Label1.Visible = False
                TextGuia.Visible = False
                Label2.Visible = False
                Me.Text = "Saldo Iniciales Artículos Logísticos"
                PEsSaldoInicial = True
            Else
                MaskedRemito.Visible = True
                Label1.Visible = True
                TextGuia.Visible = True
                Label2.Visible = True
                Me.Text = "Ingreso Artículos Logísticos"
                PEsSaldoInicial = False
            End If
        End If

        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf Dtgrid_NewRow)
        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.ColumnChanged, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanged)
        AddHandler DtGrid.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid_RowChanged)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtIngresoCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Ingreso")
        AddHandler Enlace.Format, AddressOf FormatGuia
        TextIngreso.DataBindings.Clear()
        TextIngreso.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Remito")
        AddHandler Enlace.Format, AddressOf FormatRemito
        MaskedRemito.DataBindings.Clear()
        MaskedRemito.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Guia")
        AddHandler Enlace.Format, AddressOf FormatGuia
        AddHandler Enlace.Parse, AddressOf ParseGuia
        TextGuia.DataBindings.Clear()
        TextGuia.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Deposito")
        ComboDeposito.DataBindings.Clear()
        ComboDeposito.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Pagado")
        CheckPagado.DataBindings.Clear()
        CheckPagado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Format, AddressOf FormatTexto
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

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
    Private Sub ParseGuia(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Sub FormatHora(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "00")

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Opcion() Then Me.Close() : Exit Sub

        PIngreso = 0
        ArmaArchivos()

    End Sub
    Private Function ActualizaArchivos(ByVal DtIngresoCabezaAux As DataTable, ByVal DtIngresoDetalleAux As DataTable, ByVal DtStockInsumosAux As DataTable) As Boolean

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtIngresoDetalleAux.Select("Articulo = " & Row("Articulo"))
            If RowsBusqueda.Length = 0 Then
                If Not AgregarArticulo(DtIngresoDetalleAux, DtStockInsumosAux, Row("Articulo"), Row("Cantidad")) Then Return False
            Else
                If RowsBusqueda(0).Item("Cantidad") <> Row("Cantidad") Then
                    If Not BorrarArticulo(DtIngresoDetalleAux, DtStockInsumosAux, Row("Articulo"), RowsBusqueda(0).Item("Cantidad")) Then Return False
                    If Not AgregarArticulo(DtIngresoDetalleAux, DtStockInsumosAux, Row("Articulo"), Row("Cantidad")) Then Return False
                End If
            End If
        Next

        For Each Row As DataRow In DtIngresoDetalleAux.Rows
            If Row.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtGrid.Select("Articulo = " & Row("Articulo"))
                If RowsBusqueda.Length = 0 Then
                    If Not BorrarArticulo(DtIngresoDetalleAux, DtStockInsumosAux, Row("Articulo"), Row("Cantidad")) Then Return False
                End If
            End If
        Next

        Return True

    End Function
    Private Function AgregarArticulo(ByVal DtIngresoDetalleAux As DataTable, ByVal DtStockInsumosAux As DataTable, ByVal Articulo As Integer, ByVal Cantidad As Decimal) As Boolean

        Dim Row As DataRow = DtIngresoDetalleAux.NewRow
        Row("Ingreso") = PIngreso
        Row("Articulo") = Articulo
        Row("Cantidad") = Cantidad
        Row("Devueltas") = 0
        DtIngresoDetalleAux.Rows.Add(Row)

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtStockInsumosAux.Select("Articulo = " & Articulo & " AND Deposito = " & ComboDeposito.SelectedValue)
        If RowsBusqueda.Length = 0 Then
            Dim Row1 As DataRow = DtStockInsumosAux.NewRow
            Row1("Articulo") = Articulo
            Row1("Deposito") = ComboDeposito.SelectedValue
            Row1("Stock") = Cantidad
            DtStockInsumosAux.Rows.Add(Row1)
        Else
            RowsBusqueda(0).Item("Stock") = CDec(RowsBusqueda(0).Item("Stock")) + Cantidad
        End If

        Return True

    End Function
    Private Function BorrarArticulo(ByVal DtIngresoDetalleAux As DataTable, ByVal DtStockInsumosAux As DataTable, ByVal Articulo As Integer, ByVal Cantidad As Decimal) As Boolean

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtIngresoDetalleAux.Select("Articulo = " & Articulo)
        RowsBusqueda(0).Delete()

        RowsBusqueda = DtStockInsumosAux.Select("Articulo = " & Articulo & " AND Deposito = " & ComboDeposito.SelectedValue)
        RowsBusqueda(0).Item("Stock") = CDec(RowsBusqueda(0).Item("Stock")) - Cantidad
        If RowsBusqueda(0).Item("Stock") < 0 Then
            MsgBox(NombreArticuloLogistico(Articulo) & " No Tiene Stock. Operación se CANCELA.")
        End If

        Return True

    End Function
    Private Sub HacerAlta(ByVal DtIngresoCabezaAux As DataTable, ByVal DtIngresoDetalleAux As DataTable, ByVal DtStock As DataTable)

        Dim NumeroIngreso As Integer
        Dim Resul As Integer

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Lote.
            NumeroIngreso = UltimaNumeracion(ConexionIngreso)
            If NumeroIngreso < 0 Then
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            '
            DtIngresoCabezaAux.Rows(0).Item("Ingreso") = NumeroIngreso
            For Each Row1 As DataRow In DtIngresoDetalleAux.Rows
                Row1("Ingreso") = NumeroIngreso
            Next
            '
            Resul = ActualizaIngreso(DtIngresoCabezaAux, DtIngresoDetalleAux, DtStock)
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
            PIngreso = NumeroIngreso
            ArmaArchivos()
        End If

    End Sub
    Private Sub HacerModificacion(ByVal DtIngresoCabezaAux As DataTable, ByVal DtIngresoDetalleAux As DataTable, ByVal DtStock As DataTable)

        Dim NumeroW As Double

        NumeroW = ActualizaIngreso(DtIngresoCabezaAux, DtIngresoDetalleAux, DtStock)

        If NumeroW < 0 Then
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
    Private Function ActualizaIngreso(ByVal DtIngresoCabezaAux As DataTable, ByVal DtIngresoDetalleAux As DataTable, ByVal DtStock As DataTable) As Integer

        Dim Numero As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtIngresoCabezaAux.GetChanges) Then
                    Numero = GrabaTabla(DtIngresoCabezaAux.GetChanges, "IngresoArticulosLogisticosCabeza", ConexionIngreso)
                    If Numero <= 0 Then Return Numero
                End If
                '
                If Not IsNothing(DtIngresoDetalleAux.GetChanges) Then
                    Numero = GrabaTabla(DtIngresoDetalleAux.GetChanges, "IngresoArticulosLogisticosDetalle", ConexionIngreso)
                    If Numero <= 0 Then Return Numero
                End If
                '
                If Not IsNothing(DtStock.GetChanges) Then
                    Numero = GrabaTabla(DtStock.GetChanges, "StockArticulosLogisticos", ConexionIngreso)
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
    Private Function ArmaStcok(ByVal DtGridAux As DataTable, ByVal DtRemitoDetalleAux As DataTable, ByVal DtStockInsumosAux As DataTable) As Boolean

        Dim Sql As String
        Dim RowsBusqueda() As DataRow

        For Each Row1 As DataRow In DtGridAux.Rows
            Sql = "SELECT * FROM StockArticulosLogisticos WHERE Deposito = " & DtIngresoCabeza.Rows(0).Item("Deposito") & " AND Articulo = " & Row1("Articulo") & ";"
            If Not Tablas.Read(Sql, ConexionIngreso, DtStockInsumosAux) Then Return False
        Next

        For Each Row1 As DataRow In DtRemitoDetalleAux.Rows
            RowsBusqueda = DtStockInsumosAux.Select("Articulo = " & Row1("Articulo") & " AND Deposito = " & ComboDeposito.SelectedValue)
            If RowsBusqueda.Length = 0 Then
                Sql = "SELECT * FROM StockArticulosLogisticos WHERE Deposito = " & ComboDeposito.SelectedValue & " AND Articulo = " & Row1("Articulo") & ";"
                If Not Tablas.Read(Sql, ConexionIngreso, DtStockInsumosAux) Then Return False
            End If
        Next

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim AGranel As New DataColumn("AGranel")
        AGranel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(AGranel)

        Dim Ingreso As New DataColumn("Ingreso")
        Ingreso.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Ingreso)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Medida As DataColumn = New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Medida)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Articulo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 6;")
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

        Sql = "SELECT Nombre FROM Proveedores WHERE Clave = " & Proveedor & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, Proveedor ya no existe o error en la Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
            Exit Sub
        End If

        TextEmisor.Text = Dta.Rows(0).Item("Nombre")

        Dta.Dispose()

    End Sub
    Private Function Opcion() As Boolean

        OpcionIngreso.PSoloInsumos = True
        OpcionIngreso.PArticulosLogisticos = True
        OpcionIngreso.PConOrdenCompra = False
        OpcionIngreso.ShowDialog()
        If OpcionIngreso.PRegresar Then
            OpcionIngreso.Dispose() : Return False
        End If
        Emisor = OpcionIngreso.PEmisor
        TextEmisor.Text = OpcionIngreso.PNombreEmisor
        Deposito = OpcionIngreso.ComboDeposito.SelectedValue
        PAbierto = OpcionIngreso.PAbierto
        OpcionIngreso.Dispose()

        Return True

    End Function
    Private Function RemitoExiste(ByVal Remito As Double) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionIngreso)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Remito) FROM IngresoArticulosLogisticosCabeza WHERE Estado = 1 AND Proveedor = " & Emisor & " AND Remito = " & Remito & ";", Miconexion)
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
            Using Miconexion As New OleDb.OleDbConnection(ConexionIngreso)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Guia) FROM IngresoArticulosLogisticosCabeza WHERE Estado = 1 AND Proveedor = " & Emisor & " AND Guia = " & Guia & ";", Miconexion)
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
            Texto = "INGRESO ARTICULOS LOGISTICOS"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 100, MTop)
        End If
        ' 
        PrintFont = New Font("Courier New", 12)
        Texto = "Remito:  " & NumeroEditado(MaskedRemito.Text) & "       Guia: " & TextGuia.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
        Texto = "Fecha :  " & DateTime1.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
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
        Dim LineaArticulo As Integer = x
        Dim LineaCantidad As Integer = x + 135

        'Titulos de descripcion.
        Texto = "ARTICULO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaArticulo + 2
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "CANTIDAD"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaCantidad - Longi - 2
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        'Descripcion del pago.
        Yq = y + SaltoLinea
        While Contador < LineasPorPagina And LineasParaImpresion < Grid.Rows.Count
            Dim Row As DataGridViewRow = Grid.Rows(LineasParaImpresion)
            If IsNothing(Row.Cells("Articulo").Value) Then Exit While
            Yq = Yq + SaltoLinea
            Texto = Strings.Left(Row.Cells("Articulo").FormattedValue, 39)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaArticulo + 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Texto = Row.Cells("Cantidad").FormattedValue & " " & Row.Cells("Medida").Value
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            'Imprime Comprobante.
            Contador = Contador + 1
            LineasParaImpresion = LineasParaImpresion + 1
        End While

        Yq = Yq + SaltoLinea

        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Yq - y)
        'Lineas vertical.
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaArticulo, y, LineaArticulo, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCantidad, y, LineaCantidad, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y + 2 * SaltoLinea, x + Ancho, y + 2 * SaltoLinea)

        UltimaLinea = Yq
        LineasImpresas = Contador

    End Sub
    Private Sub Print_Final(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

    End Sub
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM IngresoArticulosLogisticosCabeza;", Miconexion)
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
    Private Function UltimaNumeracion(ByVal ConexionStr) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Ingreso) FROM IngresoArticulosLogisticosCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDec(Ultimo) + 1
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTime1.Value, Date.Now) < 0 Then
            MsgBox("Fecha Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If

        If Not PEsSaldoInicial Then
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
            If DtIngresoCabeza.Rows(0).Item("Guia") <> GuiaAnt And DtIngresoCabeza.Rows(0).Item("Guia") <> 0 Then
                If GuiaExiste(DtIngresoCabeza.Rows(0).Item("Guia")) Then
                    MsgBox("Guia ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    TextGuia.Focus()
                    Return False
                End If
            End If
        End If

        If Grid.Rows.Count = 1 Then
            MsgBox("Falta Informar Articulos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
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
            If Grid.Rows(i).Cells("Cantidad").Value = 0 And PIngreso = 0 Then
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
        Next

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

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then            'Completa el grid.
            If Grid.Rows(e.RowIndex).Cells("Articulo").Value <> 0 Then
                HallaAGranelYMedidaLogistico(Grid.Rows(e.RowIndex).Cells("Articulo").Value, Grid.Rows(e.RowIndex).Cells("AGranel").Value, Grid.Rows(e.RowIndex).Cells("Medida").Value)
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

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Cantidad").Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                End If
            End If
        End If

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then Exit Sub

        Grid.Rows.Remove(Grid.CurrentRow)

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Dtgrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row
        Row("Ingreso") = 0
        Row("Articulo") = 0
        Row("Cantidad") = 0
        Row("AGranel") = False
        Row("Medida") = ""

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Articulo") Then
            If e.Row("Ingreso") <> 0 Then
                e.ProposedValue = e.Row("Articulo")
                Exit Sub
            End If
        End If

        If e.Column.ColumnName.Equals("Cantidad") Then
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

    End Sub
    Private Sub DtGrid_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        'Borra las lineas en blancos que aparecen cuando hago click en el ultimo renglon y sin informar nada regreso a algun renglon anterior. 

        If e.Row("Articulo") = 0 And e.Row("Cantidad") = 0 Then e.Row.Delete()

    End Sub


End Class