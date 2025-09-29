Imports System.Transactions
Public Class UnRemitoLogistico
    ' si orden compra es mixta Solo actualiza las recibidas en la orden de compra en B.
    ' Opcion ingreso muestra la orden en B si es mixta.
    Public PConsumo As Double
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Private DtRemitoCabeza As DataTable
    Private DtRemitoDetalle As DataTable
    '
    Private DtGrid As DataTable
    '
    Dim Cliente As Integer
    Dim Deposito As Integer
    Dim Sucursal As Integer
    Dim ConexionRemito As String
    Dim cb As ComboBox
    Dim RemitoAnt As Double
    Dim RemitoRelacionadoAnt As Double
    Private Sub UnRemitoLogistico_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(12) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False

        LlenaComboTablas(ComboDeposito, 20)
        ComboDeposito.SelectedValue = 0

        If PConsumo = 0 Then
            Opciones()
            If Cliente = 0 Then Me.Close() : Exit Sub
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        LlenaCombosGrid()

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PAbierto Then
            ConexionRemito = Conexion
            PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
        Else
            ConexionRemito = ConexionN
            PictureCandado.Image = UnRemito.ImageList1.Images.Item("Cerrado")
        End If

        GModificacionOk = False

        ArmaArchivos()

    End Sub
    Private Sub UnRemitoLogistico_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Remito de Baja.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtRemitoCabezaAux As DataTable = DtRemitoCabeza.Copy
        Dim DtRemitoDetalleAux As DataTable = DtRemitoDetalle.Copy
        Dim DtStockInsumosAux As New DataTable

        If Not ArmaStcok(DtGrid, DtRemitoDetalleAux, DtStockInsumosAux) Then Exit Sub

        If Not ActualizaArchivos(DtRemitoCabezaAux, DtRemitoDetalleAux, DtStockInsumosAux) Then Exit Sub

        If IsNothing(DtRemitoCabezaAux.GetChanges) And IsNothing(DtRemitoDetalleAux.GetChanges) Then
            MsgBox("No hay Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If CheckConfirmado.Checked And Not IsNothing(DtRemitoDetalleAux.GetChanges) Then
            MsgBox("Remito fue Confirmado por el Cliente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PConsumo = 0 Then
            HacerAlta(DtRemitoCabezaAux, DtRemitoDetalleAux, DtStockInsumosAux)
        Else
            HacerModificacion(DtRemitoCabezaAux, DtRemitoDetalleAux, DtStockInsumosAux)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

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
            MsgBox("ERROR, Remito Ya esta Anulado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtRemitoCabezaAux As DataTable = DtRemitoCabeza.Copy
        Dim DtRemitoDetalleAux As DataTable = DtRemitoDetalle.Copy
        Dim DtStockInsumosAux As New DataTable

        If Not ArmaStcok(DtGrid, DtRemitoDetalleAux, DtStockInsumosAux) Then Exit Sub

        ActualizaArchivos(DtRemitoCabezaAux, DtRemitoDetalleAux, DtStockInsumosAux)

        If Not IsNothing(DtRemitoCabezaAux.GetChanges) Or Not IsNothing(DtRemitoDetalleAux.GetChanges) Then
            MsgBox("Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtRemitoDetalleAux.Rows
            RowsBusqueda = DtStockInsumosAux.Select("Articulo = " & Row("Articulo"))
            RowsBusqueda(0).Item("Stock") = CDec(RowsBusqueda(0).Item("Stock")) + CDec(Row("Cantidad"))
        Next

        If MsgBox("El Remito se Anulara. ¿Desea Anularlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtRemitoCabezaAux.Rows(0).Item("Estado") = 3

        Dim Resul As Double = ActualizaConsumo(DtRemitoCabezaAux, DtRemitoDetalleAux, DtStockInsumosAux)

        If Resul = -1 Or Resul = -2 Then
            MsgBox("ERROR, Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Remito fue Anulado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Cliente = 0
        Deposito = 0
        Sucursal = 0
        PAbierto = True
        PConsumo = 0
        UnRemitoLogistico_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonNuevoIgualCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevoIgualCliente.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PConsumo = 0
        UnRemitoLogistico_Load(Nothing, Nothing)

    End Sub
    Private Sub MaskedRemito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MaskedRemito.Validating

        If Val(MaskedRemito.Text) = 0 Then Exit Sub

        If Not MaskedOK(MaskedRemito) Then
            MsgBox("Remito Logistico Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedRemito.Text = "000000000000"
            e.Cancel = True
            Exit Sub
        End If

    End Sub
    Private Sub MaskedRemitoRelacionado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MaskedRemitoRelacionado.Validating

        If Val(MaskedRemitoRelacionado.Text) = 0 Then Exit Sub

        If Not MaskedOK(MaskedRemitoRelacionado) Then
            MsgBox("Remito Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedRemitoRelacionado.Text = "000000000000"
            e.Cancel = True
            Exit Sub
        End If

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then Exit Sub

        Grid.Rows.Remove(Grid.CurrentRow)

    End Sub
    Private Sub ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Row As DataRow

        CreaDtGrid()

        DtRemitoCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM RemitosLogisticosCabeza WHERE Consumo = " & PConsumo & ";", ConexionRemito, DtRemitoCabeza) Then Me.Close() : Exit Sub
        If PConsumo <> 0 And DtRemitoCabeza.Rows.Count = 0 Then
            MsgBox("Remito No Existe En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If DtRemitoCabeza.Rows.Count = 0 Then
            Row = DtRemitoCabeza.NewRow
            Row("Consumo") = 0
            Row("Remito") = 0
            Row("Cliente") = Cliente
            Row("Deposito") = Deposito
            Row("Sucursal") = Sucursal
            Row("Fecha") = Now
            Row("FechaRemito") = Format(Now, "dd/MM/yyyy")
            Row("Confirmado") = False
            Row("Estado") = 1
            Row("RemitoRelacionado") = 0
            Row("Comentario") = ""
            DtRemitoCabeza.Rows.Add(Row)
        End If

        Cliente = DtRemitoCabeza.Rows(0).Item("Cliente")
        Deposito = DtRemitoCabeza.Rows(0).Item("Deposito")

        Dim Row1 As DataRow

        ComboSucursal.DataSource = Nothing
        ComboSucursal.DataSource = Tablas.Leer("SELECT Nombre,Clave FROM SucursalesClientes WHERE Estado = 1 AND Cliente = " & Cliente & ";")
        Row1 = ComboSucursal.DataSource.NewRow
        Row1("Nombre") = ""
        Row1("Clave") = 0
        ComboSucursal.DataSource.Rows.Add(Row1)
        ComboSucursal.DisplayMember = "Nombre"
        ComboSucursal.ValueMember = "Clave"

        MuestraCabeza()

        DtRemitoDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM RemitosLogisticosDetalle WHERE Consumo = " & PConsumo & ";", ConexionRemito, DtRemitoDetalle) Then Me.Close() : Exit Sub
        For Each Row In DtRemitoDetalle.Rows
            Row1 = DtGrid.NewRow
            Row1("Consumo") = Row("Consumo")
            Row1("Articulo") = Row("Articulo")
            Row1("Cantidad") = Row("Cantidad")
            Row1("AGranel") = False
            Row1("Medida") = ""
            HallaAGranelYMedidaLogistico(Row1("Articulo"), Row1("AGranel"), Row1("Medida"))
            DtGrid.Rows.Add(Row1)
        Next

        Grid.EndEdit()
        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If DtRemitoCabeza.Rows(0).Item("Estado") = 3 Then
            Grid.ReadOnly = True
            Panel1.Enabled = False
            ButtonEliminarLinea.Enabled = False
        Else
            Grid.ReadOnly = False
            Panel1.Enabled = True
            ButtonEliminarLinea.Enabled = True
        End If

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)
        AddHandler DtGrid.ColumnChanged, New DataColumnChangeEventHandler(AddressOf Dtgrid_ColumnChanged)
        AddHandler DtGrid.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid_RowChanged)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtRemitoCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Consumo")
        AddHandler Enlace.Format, AddressOf FormatConsumo
        TextConsumo.DataBindings.Clear()
        TextConsumo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Remito")
        AddHandler Enlace.Format, AddressOf FormatRemito
        MaskedRemito.DataBindings.Clear()
        MaskedRemito.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "RemitoRelacionado")
        AddHandler Enlace.Format, AddressOf FormatRemito
        MaskedRemitoRelacionado.DataBindings.Clear()
        MaskedRemitoRelacionado.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Deposito")
        ComboDeposito.DataBindings.Clear()
        ComboDeposito.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Sucursal")
        ComboSucursal.DataBindings.Clear()
        ComboSucursal.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaRemito")
        DateFechaRemito.DataBindings.Clear()
        DateFechaRemito.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Confirmado")
        Checkconfirmado.DataBindings.Clear()
        CheckConfirmado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Parse, AddressOf ParseTexto
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        TextCliente.Text = NombreCliente(DtRemitoCabeza.Rows(0).Item("Cliente"))
        RemitoAnt = DtRemitoCabeza.Rows(0).Item("Remito")
        RemitoRelacionadoAnt = DtRemitoCabeza.Rows(0).Item("RemitoRelacionado")

    End Sub
    Private Sub FormatRemito(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "0000-00000000")

    End Sub
    Private Sub FormatConsumo(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then Format(Numero.Value, "#")

    End Sub
    Private Sub ParseTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Function ArmaStcok(ByVal DtGridAux As DataTable, ByVal DtRemitoDetalleAux As DataTable, ByVal DtStockInsumosAux As DataTable) As Boolean

        Dim Sql As String
        Dim RowsBusqueda() As DataRow

        For Each Row1 As DataRow In DtGridAux.Rows
            Sql = "SELECT * FROM StockArticulosLogisticos WHERE Deposito = " & DtRemitoCabeza.Rows(0).Item("Deposito") & " AND Articulo = " & Row1("Articulo") & ";"
            If Not Tablas.Read(Sql, ConexionRemito, DtStockInsumosAux) Then Return False
        Next

        For Each Row1 As DataRow In DtRemitoDetalleAux.Rows
            RowsBusqueda = DtStockInsumosAux.Select("Articulo = " & Row1("Articulo") & " AND Deposito = " & ComboDeposito.SelectedValue)
            If RowsBusqueda.Length = 0 Then
                Sql = "SELECT * FROM StockArticulosLogisticos WHERE Deposito = " & ComboDeposito.SelectedValue & " AND Articulo = " & Row1("Articulo") & ";"
                If Not Tablas.Read(Sql, ConexionRemito, DtStockInsumosAux) Then Return False
            End If
        Next

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim AGranel As New DataColumn("AGranel")
        AGranel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(AGranel)

        Dim Consumo As New DataColumn("Consumo")
        Consumo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Consumo)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Medida)

    End Sub
    Private Function ActualizaArchivos(ByVal DtRemitoCabezaAux As DataTable, ByVal DtRemitoDetalleAux As DataTable, ByVal DtStockInsumosAux As DataTable) As Boolean

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtRemitoDetalleAux.Select("Articulo = " & Row("Articulo"))
            If RowsBusqueda.Length = 0 Then
                If Not AgregarArticulo(DtRemitoDetalleAux, DtStockInsumosAux, Row("Articulo"), Row("Cantidad")) Then Return False
            Else
                If RowsBusqueda(0).Item("Cantidad") <> Row("Cantidad") Then
                    If Not BorrarArticulo(DtRemitoDetalleAux, DtStockInsumosAux, Row("Articulo"), RowsBusqueda(0).Item("Cantidad")) Then Return False
                    If Not AgregarArticulo(DtRemitoDetalleAux, DtStockInsumosAux, Row("Articulo"), Row("Cantidad")) Then Return False
                End If
            End If
        Next

        For Each Row As DataRow In DtRemitoDetalleAux.Rows
            If Row.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtGrid.Select("Articulo = " & Row("Articulo"))
                If RowsBusqueda.Length = 0 Then
                    If Not BorrarArticulo(DtRemitoDetalleAux, DtStockInsumosAux, Row("Articulo"), Row("Cantidad")) Then Return False
                End If
            End If
        Next

        Return True

    End Function
    Private Function AgregarArticulo(ByVal DtRemitoDetalleAux As DataTable, ByVal DtStockInsumosAux As DataTable, ByVal Articulo As Integer, ByVal Cantidad As Decimal) As Boolean

        Dim Row As DataRow = DtRemitoDetalleAux.NewRow
        Row("Consumo") = PConsumo
        Row("Articulo") = Articulo
        Row("Cantidad") = Cantidad
        DtRemitoDetalleAux.Rows.Add(Row)

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtStockInsumosAux.Select("Articulo = " & Articulo & " AND Deposito = " & ComboDeposito.SelectedValue)
        If RowsBusqueda.Length = 0 Then
            MsgBox(NombreArticuloLogistico(Articulo) & " No Tiene Stock. Operación se CANCELA.")
            Return False
        Else
            RowsBusqueda(0).Item("Stock") = CDec(RowsBusqueda(0).Item("Stock")) - Cantidad
            If RowsBusqueda(0).Item("Stock") < 0 Then
                MsgBox(NombreArticuloLogistico(Articulo) & " No Tiene Stock. Operación se CANCELA.")
                Return False
            End If
        End If

        Return True

    End Function
    Private Function BorrarArticulo(ByVal DtRemitoDetalleAux As DataTable, ByVal DtStockInsumosAux As DataTable, ByVal Articulo As Integer, ByVal Cantidad As Decimal) As Boolean

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtRemitoDetalleAux.Select("Articulo = " & Articulo)
        RowsBusqueda(0).Delete()

        RowsBusqueda = DtStockInsumosAux.Select("Articulo = " & Articulo & " AND Deposito = " & ComboDeposito.SelectedValue)
        RowsBusqueda(0).Item("Stock") = CDec(RowsBusqueda(0).Item("Stock")) + Cantidad

        Return True

    End Function
    Private Sub HacerAlta(ByVal DtRemitoCabezaAux As DataTable, ByVal DtRemitoDetalleAux As DataTable, ByVal DtStockInsumos As DataTable)

        Dim ConsumoW As Integer
        Dim Resul As Double = 0

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Ingreso.
            ConsumoW = UltimoConsumo(ConexionRemito)
            If ConsumoW < 0 Then
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            '
            DtRemitoCabezaAux.Rows(0).Item("Consumo") = ConsumoW
            For Each Row1 As DataRow In DtRemitoDetalleAux.Rows
                Row1("Consumo") = DtRemitoCabezaAux.Rows(0).Item("Consumo")
            Next
            '
            Resul = ActualizaConsumo(DtRemitoCabezaAux, DtRemitoDetalleAux, DtStockInsumos)
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
            GModificacionOk = True
            PConsumo = DtRemitoCabezaAux.Rows(0).Item("Consumo")
            ArmaArchivos()
        End If

    End Sub
    Private Sub HacerModificacion(ByVal DtRemitoCabezaAux As DataTable, ByVal DtRemitoDetalleAux As DataTable, ByVal DtStockInsumos As DataTable)

        Dim Resul As Integer = ActualizaConsumo(DtRemitoCabezaAux, DtRemitoDetalleAux, DtStockInsumos)

        If Resul = -1 Or Resul = -2 Then
            MsgBox("ERROR, base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

    End Sub
    Private Function ActualizaConsumo(ByVal DtRemitoCabezaAux As DataTable, ByVal DtRemitoDetalleAux As DataTable, ByVal DtStockInsumosAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtRemitoCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtRemitoCabezaAux.GetChanges, "RemitosLogisticosCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtRemitoDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtRemitoDetalleAux.GetChanges, "RemitosLogisticosDetalle", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtStockInsumosAux.GetChanges) Then
                    Resul = GrabaTabla(DtStockInsumosAux.GetChanges, "StockArticulosLogisticos", ConexionRemito)
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

        Articulo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo =6;")
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Function UltimoConsumo(ByVal ConexionStr) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Consumo) FROM RemitosLogisticosCabeza;", Miconexion)
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
    Private Sub Opciones()

        OpcionIFCO.PCliente = Cliente
        OpcionIFCO.PDeposito = Deposito
        If PAbierto And Cliente <> 0 Then
            OpcionIFCO.PAbierto = False
        Else
            OpcionIFCO.PAbierto = True
        End If
        OpcionIFCO.ShowDialog()
        If OpcionIFCO.PRegresar Then Cliente = 0 : OpcionIFCO.Dispose() : Exit Sub
        Cliente = OpcionIFCO.PCliente
        Deposito = OpcionIFCO.PDeposito
        Sucursal = OpcionIFCO.PSucursal
        PAbierto = OpcionIFCO.PAbierto
        OpcionIFCO.Dispose()

    End Sub
    Private Function ExisteRemito(ByVal SQL As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionRemito)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(SQL, Miconexion)
                    Return CDbl(Cmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            MsgBox("ERROR DE BASE DE DATOS!!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!")
            Return -1
        End Try

    End Function
    Private Function Valida() As Boolean

        Dim Sql As String = ""

        If (Val(MaskedRemito.Text) = 0 Or Not MaskedOK(MaskedRemito)) Then
            MsgBox("Numero Remito Logistico Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Dim Existe As Integer = 0

        If Val(MaskedRemito.Text) <> RemitoAnt Then
            ' ---------------------------------------------- BUSCA SI EXISTE --------------------------------------------------------- '
            Sql = "SELECT COUNT(Remito) FROM RemitosLogisticosCabeza WHERE Remito = " & CDbl(MaskedRemito.Text) & " AND Cliente = " & Cliente & ";"
            Existe = ExisteRemito(Sql)

            If Existe > 0 Then
                If MsgBox("Remito Logistico Ya Existe. Desea Continuar?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Return False
                End If
            End If
            If Existe < 0 Then Return False
        End If

        If Val(MaskedRemitoRelacionado.Text) <> RemitoRelacionadoAnt Then
            If Not MaskedOK(MaskedRemitoRelacionado) Then
                MsgBox("Numero Remito Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            ' ---------------------------------------------- BUSCA SI EXISTE EL REMITO RELACIONADO --------------------------------------------------------- '
            Sql = "SELECT COUNT(Remito) FROM RemitosCabeza WHERE Estado <> 3 AND Remito = " & CDbl(MaskedRemitoRelacionado.Text) & ";"
            Existe = ExisteRemito(Sql)

            If Existe = 0 Then
                MsgBox("REMITO NO EXISTE!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!")
                Return False
            End If
            If Existe < 0 Then Return False
            ' ---------------------------------------------- BUSCA SI YA ESTA ASIGNADO --------------------------------------------------------- '
            Sql = "SELECT COUNT(Remito) FROM RemitosLogisticosCabeza WHERE RemitoRelacionado = " & CDbl(MaskedRemitoRelacionado.Text) & ";"
            Existe = ExisteRemito(Sql)

            If Existe > 0 Then
                MsgBox("Número Remito Relacionado Ya Está Asignado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Existe < 0 Then Return False
        End If

        If Not HallaOPR(Cliente, "C") And PAbierto Then
            If MsgBox("Cliente Solo Opera a Candado Cerrado. Debe Cambiar Candado en Cliente. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
        End If

        If DiferenciaDias(DateFechaRemito.Value, DateTime1.Value) < 0 Then
            '      MsgBox("Fecha Remito Mayor a la Fecha Ingreso.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            '         TextFechaRemito.Focus()
            '           Return False
        End If

        If Grid.Rows.Count = 0 Then
            MsgBox("Falta Informar Insumo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        For i As Integer = 0 To Grid.RowCount - 2
            If Grid.Rows(i).Cells("Articulo").Value = 0 Then
                MsgBox("Debe Informar Articulo en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Articulo")
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

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
            End If
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then            'Completa el grid.
            HallaAGranelYMedidaLogistico(Grid.Rows(e.RowIndex).Cells("Articulo").Value, Grid.Rows(e.RowIndex).Cells("AGranel").Value, Grid.Rows(e.RowIndex).Cells("Medida").Value)
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
            AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

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
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then e.Value = Format(e.Value, "#")
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Consumo") = PConsumo
        e.Row("Articulo") = 0
        e.Row("Cantidad") = 0
        e.Row("AGranel") = False
        e.Row("Medida") = ""

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

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
        '  Dim Indice As Integer = DtDetalle.Rows.IndexOf(e.Row)

        'Borra las lineas en blancos que aparecen cuando hago click en el ultimo renglon y sin informar nada regreso a algun renglon anterior. 

        If e.Row("Articulo") = 0 And e.Row("Cantidad") = 0 Then e.Row.Delete()

    End Sub
End Class
