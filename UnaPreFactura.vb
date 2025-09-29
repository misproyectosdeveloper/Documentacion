Public Class UnaPreFactura
    Public PBloquea As Boolean
    Public PDtAFacturar As DataTable
    Public PDeposito As Integer
    Public PLista As Integer
    Public PFechaEntrega As DateTime
    Public PCliente As Integer
    Public PTieneCodigoCliente As Boolean
    Public PListaDeLotes As List(Of FilaAsignacion)
    Public PEsFactura As Boolean
    Public PEsRemito As Boolean
    Public PRegresar As Boolean
    ' 
    Dim DtGrid As DataTable
    Dim DtPreFactura As DataTable
    Dim cb As ComboBox
    Private WithEvents bs As New BindingSource
    Private Sub UnaPreFactura_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        GridF.AutoGenerateColumns = False

        LlenaComboTablas(ComboEspecie, 1)
        With ComboEspecie
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboEspecie.SelectedValue = 0

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = PDeposito
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        Panel4.Visible = True

        CreaDtGridLotes()

        GridF.DataSource = PDtAFacturar

        If PEsRemito Then
            ButtonVerFactura.Text = "Ver Remito"
            Label1.Text = "Pre-Remito"
            Grid.Columns("Descuento").Visible = False
            GridF.Columns("DescuentoF").Visible = False
            Grid.Columns("Senia").Visible = False
            GridF.Columns("SeniaF").Visible = False
        End If
        If PEsFactura Then ButtonVerFactura.Text = "Ver Factura" : Label1.Text = "Pre-Factura"

        If PLista <> 0 Then Grid.Columns("Precio").ReadOnly = True : Grid.Columns("TipoPrecio").ReadOnly = True : Panel4.Visible = False

        ComboTipoPrecio.DisplayMember = "Text"
        ComboTipoPrecio.ValueMember = "Value"
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(Integer))
        tb.Rows.Add("", 0)
        tb.Rows.Add("Uni.", 1)
        tb.Rows.Add("Kgs.", 2)
        ComboTipoPrecio.DataSource = tb

        PRegresar = True

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonSeleccionar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSeleccionar.Click

        Dim SqlB As String
        Dim SqlN As String

        If ComboEspecie.SelectedValue = 0 Then
            MsgBox("Falta Especie.", MsgBoxStyle.Exclamation) : Exit Sub
        End If
        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Deposito.", MsgBoxStyle.Exclamation) : Exit Sub
        End If

        Dim DtArticulos As New DataTable
        If Not Tablas.Read("SELECT Clave,Especie,Variedad FROM Articulos WHERE Especie = " & ComboEspecie.SelectedValue & ";", Conexion, DtArticulos) Then Me.Close() : Exit Sub

        Dim StrArticulo As String
        Dim StrEspecie As String
        Dim StrVariedad As String
        Dim Dt As New DataTable
        For Each Row As DataRow In DtArticulos.Rows
            StrArticulo = Row("Clave")
            StrEspecie = Row("Especie")
            StrVariedad = Row("Variedad")
            SqlB = "SELECT 1 as Operacion,Lote,Secuencia,Articulo,Proveedor,KilosXUnidad,Stock,Calibre," & StrEspecie & " AS Especie," & StrVariedad & " AS Variedad,Senia FROM Lotes WHERE Stock <> 0 AND Deposito = " & ComboDeposito.SelectedValue & " AND Articulo = " & StrArticulo & ";"
            SqlN = "SELECT 2 as Operacion,Lote,Secuencia,Articulo,Proveedor,KilosXUnidad,Stock,Calibre," & StrEspecie & " AS Especie," & StrVariedad & " AS Variedad,Senia FROM Lotes WHERE Stock <> 0 AND Deposito = " & ComboDeposito.SelectedValue & " AND Articulo = " & StrArticulo & ";"
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
            If PermisoTotal Then
                If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
            End If
        Next

        DtGrid.Clear()
        Grid.DataSource = Nothing

        Dim PrecioW As Decimal = 0
        Dim TipoPrecioW As Integer = 0
        Dim Ok As Boolean
        Dim CantidadW As Decimal = 0
        Dim Codigo As String = ""
        Dim ArticuloExisteEnPedido As Boolean

        For Each Row As DataRow In Dt.Rows
            Ok = True
            If PTieneCodigoCliente Or PLista <> 0 Then
                If Not AnalizaPropiedadesArticulo(PCliente, Row("Articulo"), Row("KilosXUnidad"), PFechaEntrega, PTieneCodigoCliente, PLista, 0, CantidadW, TipoPrecioW, PrecioW, Codigo, ArticuloExisteEnPedido) Then Me.Close() : Exit Sub
                If IsNothing(Codigo) Then Codigo = "-1"
                If PTieneCodigoCliente And Codigo = "-1" Then Ok = False
                If PLista <> 0 And PrecioW = 0 Then Ok = False
            End If
            If Ok Then
                Dim RowGrid As DataRow = DtGrid.NewRow
                RowGrid("Operacion") = Row("Operacion")
                RowGrid("Lote") = Row("Lote")
                RowGrid("Secuencia") = Row("Secuencia")
                RowGrid("LoteYSecuencia") = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
                RowGrid("Articulo") = Row("Articulo")
                RowGrid("Proveedor") = Row("Proveedor")
                RowGrid("Especie") = Row("Especie")
                RowGrid("Variedad") = Row("Variedad")
                RowGrid("Calibre") = Row("Calibre")
                RowGrid("KilosXUnidad") = Row("KilosXUnidad")
                RowGrid("Stock") = Row("Stock")
                RowGrid("Senia") = Row("Senia")
                If RowGrid("Senia") = -1 Then RowGrid("Senia") = 0
                If PEsRemito Then
                    RowGrid("Senia") = 0
                End If
                RowGrid("Descuento") = 0
                RowGrid("Cantidad") = 0
                RowGrid("Precio") = PrecioW
                RowGrid("TipoPrecio") = TipoPrecioW
                If TipoPrecioW = 0 And ComboTipoPrecio.SelectedValue <> 0 Then
                    RowGrid("TipoPrecio") = ComboTipoPrecio.SelectedValue
                End If
                Dim Cantidad As Decimal
                Dim Precio As Decimal
                Dim KilosXUnidad As Decimal
                Dim Descuento As Decimal
                Dim Senia As Decimal
                Dim Existe As Boolean = False
                TraeDatosFactura(Row("Operacion"), Row("Lote"), Row("Secuencia"), Cantidad, Precio, KilosXUnidad, Descuento, Senia, Existe)
                If Existe Then
                    RowGrid("Stock") = Row("Stock") - Cantidad
                End If
                If RowGrid("Stock") > 0 Then DtGrid.Rows.Add(RowGrid)
            End If
        Next

        Dim View As New DataView
        View = DtGrid.DefaultView
        View.Sort = "Lote,Secuencia"

        Grid.DataSource = bs
        bs.DataSource = View

        For I As Integer = 0 To Grid.Rows.Count - 1
            HallaAGranelYMedidaFactura(False, False, Grid.Rows(I).Cells("Articulo").Value, Grid.Rows(I).Cells("AGranel").Value, Grid.Rows(I).Cells("Medida").Value)
        Next

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid_ColumnChanging)
        AddHandler DtGrid.ColumnChanged, New DataColumnChangeEventHandler(AddressOf Dtgrid_ColumnChanged)

        Dt.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        For Each Row As DataRow In DtGrid.Rows
            If PEsFactura Then
                If Row("Cantidad") <> 0 And Row("Precio") = 0 Then
                    MsgBox("Falta Precio En Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000"), MsgBoxStyle.Information)
                    Exit Sub
                End If
                If Row("Cantidad") <> 0 And Row("TipoPrecio") = 0 Then
                    MsgBox("Falta TipoPrecio En Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000"), MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
            If Row("Cantidad") = 0 And Row("Precio") <> 0 And PLista = 0 Then
                MsgBox("Falta Cantidad En Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000"), MsgBoxStyle.Information)
                Exit Sub
            End If
            If Row("Cantidad") <> 0 And Row("Precio") <> 0 And Row("TipoPrecio") = 0 Then
                MsgBox("Falta Informar TipoPrecio con Precio Informado En Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000"), MsgBoxStyle.Information)
                Exit Sub
            End If
            If Row("Cantidad") <> 0 And Row("Precio") = 0 And Row("TipoPrecio") <> 0 Then
                MsgBox("Falta Informar Precio con TipoPrecio Informado En Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000"), MsgBoxStyle.Information)
                Exit Sub
            End If
            If Row("KilosXUnidad") = 0 Then
                MsgBox("Falta KilosXUnidad En Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000"), MsgBoxStyle.Information)
                Exit Sub
            End If
        Next

        For i As Integer = 0 To Grid.Rows.Count - 2
            If TieneDecimales(Grid.Rows(i).Cells("Cantidad").Value) And Not Grid.Rows(i).Cells("AGranel").Value Then
                MsgBox("Cantidad no debe tener Decimales En Lote " & Grid.Rows(i).Cells("Lote").Value & "/" & Format(Grid.Rows(i).Cells("Secuencia").Value, "000"), MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        Next

        Dim Informadas As Integer = 0
        For Each Row As DataRow In DtGrid.Rows
            If Row("Cantidad") > 0 Then Informadas = Informadas + 1
        Next
        Informadas = Informadas + PDtAFacturar.Rows.Count
        If PEsFactura And Informadas + 1 > GLineasFacturas Then
            If MsgBox("Supera Cantidad Articulos Permitidos.(" & GLineasFacturas & ")" + vbCrLf + "Si continua, no prodra imprimirla." + vbCrLf + "Si es Electrónica no tiene limite. Desea Continuar de todas Formas?(Si/No)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
        End If
        If PEsRemito And Informadas + 1 > GLineasRemitos Then
            If MsgBox("Supera Cantidad Articulos Permitidos.(" & GLineasRemitos & ")" + vbCrLf + "Si continua, no prodra imprimirla." + vbCrLf + "Si es Remito Auto-Impreso no tiene limite. Desea Continuar de todas Formas?(Si/No)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
        End If

        Dim InformaArticulos As Boolean

        For Each Row As DataRow In DtGrid.Rows
            If Row("Cantidad") <> 0 Then
                Dim RowW As DataRow = PDtAFacturar.NewRow
                RowW("Operacion") = Row("Operacion")
                RowW("Lote") = Row("Lote")
                RowW("Secuencia") = Row("Secuencia")
                RowW("LoteYSecuencia") = Row("Lote") & "/" & Format(Row("Secuencia"), "000")
                RowW("Articulo") = Row("Articulo")
                RowW("Cantidad") = Row("Cantidad")
                RowW("TipoPrecio") = Row("TipoPrecio")
                RowW("Precio") = Row("Precio")
                RowW("Descuento") = Row("Descuento")
                RowW("KilosXUnidad") = Row("KilosXUnidad")
                RowW("Senia") = Row("Senia")
                PDtAFacturar.Rows.Add(RowW)
                InformaArticulos = True
            End If
        Next

        If InformaArticulos Then
            ButtonSeleccionar_Click(Nothing, Nothing)
        End If

    End Sub
    Private Sub ButtonVerFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerFactura.Click

        PListaDeLotes.Clear()
        DtPreFactura = PDtAFacturar.Clone

        Dim Esta As Boolean
        Dim Indice As Integer = 0

        For Each Row As DataRow In PDtAFacturar.Rows
            Indice = Indice + 1
            AgregaADtPreFactura(Indice, Row)
            AgregaAlistaDeLotes(PListaDeLotes, Indice, Row)
        Next


        DtGrid.Dispose()
        Grid.DataSource = Nothing

        PDtAFacturar.Clear()
        CopiaTabla(DtPreFactura, PDtAFacturar)

        PRegresar = False

        Me.Close()

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If GridF.Rows.Count = 0 Then Exit Sub

        GridF.Rows.Remove(GridF.CurrentRow)
        ButtonSeleccionar_Click(Nothing, Nothing)

    End Sub
    Private Sub CreaDtGridLotes()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim LoteYSecuencia As New DataColumn("LoteYSecuencia")
        LoteYSecuencia.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(LoteYSecuencia)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Especie As New DataColumn("Especie")
        Especie.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Especie)

        Dim Variedad As New DataColumn("Variedad")
        Variedad.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Variedad)

        Dim Calibre As New DataColumn("Calibre")
        Calibre.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Calibre)

        Dim KilosXUnidad As New DataColumn("KilosXUnidad")
        KilosXUnidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(KilosXUnidad)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Stock)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Descuento As New DataColumn("Descuento")
        Descuento.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Descuento)

        Dim TipoPrecio As New DataColumn("TipoPrecio")
        TipoPrecio.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoPrecio)

        Dim Precio As New DataColumn("Precio")
        Precio.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Precio)

        Dim Senia As New DataColumn("Senia")
        Senia.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Senia)

    End Sub
    Private Sub AgregaADtPreFactura(ByVal indice As Integer, ByVal Row As DataRow)

        Dim RowW As DataRow = DtPreFactura.NewRow
        RowW("Indice") = indice
        RowW("Articulo") = Row("Articulo")
        RowW("Cantidad") = Row("Cantidad")
        RowW("KilosXUnidad") = Row("KilosXUnidad")
        RowW("Descuento") = Row("Descuento")
        RowW("TipoPrecio") = Row("TipoPrecio")
        RowW("Precio") = Row("Precio")
        RowW("Senia") = Row("Senia")
        DtPreFactura.Rows.Add(RowW)

    End Sub
    Private Sub AgregaAlistaDeLotes(ByVal Lista As List(Of FilaAsignacion), ByVal indice As Integer, ByVal Row As DataRow)

        Dim Fila As New FilaAsignacion
        Fila.Indice = indice
        Fila.Operacion = Row("Operacion")
        Fila.Deposito = PDeposito
        Fila.Lote = Row("Lote")
        Fila.Secuencia = Row("Secuencia")
        Fila.Asignado = Row("Cantidad")
        Fila.Importe2 = Row("Senia")     'en Fila.Importe2 pone senia.
        Lista.Add(Fila)

    End Sub
    Private Function BuscaCoincidencia(ByRef dt As DataTable, ByRef Row1 As DataRow, ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Precio As Decimal, ByVal KilosXUnidad As Decimal, ByVal Descuento As Decimal, ByVal Senia As Decimal) As Boolean

        For Each Row As DataRow In dt.Rows
            If Row("Lote") = Lote And Row("Secuencia") = Secuencia And Row("Precio") = Precio And Row("KilosXUnidad") = KilosXUnidad And Row("Descuento") = Descuento And Row("Senia") = Senia Then
                Row1 = Row : Return True
            End If
        Next

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Especie.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 1;")
        Row = Especie.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Especie.DataSource.Rows.Add(Row)
        Especie.DisplayMember = "Nombre"
        Especie.ValueMember = "Clave"

        Variedad.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 2;")
        Row = Variedad.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Variedad.DataSource.Rows.Add(Row)
        Variedad.DisplayMember = "Nombre"
        Variedad.ValueMember = "Clave"

        Calibre.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 5;")
        Row = Calibre.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Calibre.DataSource.Rows.Add(Row)
        Calibre.DisplayMember = "Nombre"
        Calibre.ValueMember = "Clave"

        Articulo.DataSource = ArticulosActivos()     'Tablas.Leer("SELECT Clave,Nombre FROM Articulos;")
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

        ArticuloF.DataSource = ArticulosActivos()     'Tablas.Leer("SELECT Clave,Nombre FROM Articulos;")
        Row = ArticuloF.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        ArticuloF.DataSource.Rows.Add(Row)
        ArticuloF.DisplayMember = "Nombre"
        ArticuloF.ValueMember = "Clave"

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        Dim Dt2 As New DataTable
        ArmaTipoPrecio(Dt2)
        TipoPrecio.DataSource = Dt2.Copy
        TipoPrecio.DisplayMember = "Nombre"
        TipoPrecio.ValueMember = "Clave"

        Dim Dt3 As New DataTable
        ArmaTipoPrecio(Dt3)
        TipoPrecio2.DataSource = Dt3.Copy
        TipoPrecio2.DisplayMember = "Nombre"
        TipoPrecio2.ValueMember = "Clave"

        Dt2.Dispose()
        Dt3.Dispose()

    End Sub
    Private Sub TraeDatosFactura(ByVal Operacion As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef Cantidad As Decimal, ByRef Precio As Decimal, ByRef KilosXUnidad As Decimal, ByRef Descuento As Decimal, ByRef Senia As Decimal, ByRef Existe As Boolean)

        Dim RowsBusqueda() As DataRow

        Cantidad = 0
        Precio = 0
        Senia = 0

        RowsBusqueda = PDtAFacturar.Select("Operacion = " & Operacion & " AND Lote = " & Lote & " AND Secuencia = " & Secuencia)
        If RowsBusqueda.Length <> 0 Then
            For I As Integer = 0 To RowsBusqueda.Length - 1
                Cantidad = Cantidad + RowsBusqueda(I).Item("Cantidad")
                Precio = RowsBusqueda(0).Item("Precio")
                KilosXUnidad = RowsBusqueda(0).Item("KilosXUnidad")
                Descuento = RowsBusqueda(0).Item("Descuento")
                Senia = RowsBusqueda(0).Item("Senia")
                Existe = True
            Next
        End If

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        'para manejo del autocoplete de articulos.
        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
                Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
            Else : Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Precio" Or Grid.Columns(e.ColumnIndex).Name = "Descuento" Or Grid.Columns(e.ColumnIndex).Name = "KilosXUnidad" Or Grid.Columns(e.ColumnIndex).Name = "Senia" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If (Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Precio" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Descuento" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "KilosXUnidad" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Senia") And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloDecimales_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub SoloDecimales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Precio" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Senia" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Descuento" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "KilosXUnidad" Then
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

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Precio" Then
            If PEsFactura Then
                If CType(sender, TextBox).Text <> "" Then
                    EsNumericoGridBox.Valida(CType(sender, TextBox), 3)
                End If
            End If
            If PEsRemito Then
                If CType(sender, TextBox).Text <> "" Then
                    EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
                End If
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Senia" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Descuento" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "KilosXUnidad" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 3)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Dtgrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Precio") Or e.Column.ColumnName.Equals("KilosXUnidad") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Cantidad") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0 : Exit Sub
            If e.ProposedValue > e.Row("Stock") Then
                MsgBox("Cantidad Supera Stock.", MsgBoxStyle.Exclamation)
                e.ProposedValue = 0
            End If
        End If

    End Sub
    Private Sub Dtgrid_ColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRIDF.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridF_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridF.CellEnter

        'para manejo del autocoplete de articulos.
        If Not GridF.Columns(e.ColumnIndex).ReadOnly Then
            GridF.BeginEdit(True)
        End If

    End Sub
    Private Sub GridF_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles GridF.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If GridF.Columns(e.ColumnIndex).Name = "LoteYSecuenciaF" Then
            If GridF.Rows(e.RowIndex).Cells("OperacionF").Value = 1 Then
                GridF.Rows(e.RowIndex).Cells("CandadoF").Value = ImageList1.Images.Item("Abierto")
            Else : GridF.Rows(e.RowIndex).Cells("CandadoF").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If GridF.Columns(e.ColumnIndex).Name = "CantidadF" Then
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

        If GridF.Columns(e.ColumnIndex).Name = "PrecioF" Or GridF.Columns(e.ColumnIndex).Name = "DescuentoF" Or GridF.Columns(e.ColumnIndex).Name = "KilosXUnidadF" Or GridF.Columns(e.ColumnIndex).Name = "SeniaF" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

    End Sub
    Private Sub GridF_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles GridF.EditingControlShowing

        Dim columna As Integer = GridF.CurrentCell.ColumnIndex

        If (GridF.Columns.Item(GridF.CurrentCell.ColumnIndex).Name = "PrecioF" Or GridF.Columns.Item(GridF.CurrentCell.ColumnIndex).Name = "DescuentoF" Or GridF.Columns.Item(GridF.CurrentCell.ColumnIndex).Name = "KilosXUnidadF") And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloDecimalesF_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChangedF_TextChanged
        End If

    End Sub
    Private Sub SoloDecimalesF_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If GridF.Columns.Item(GridF.CurrentCell.ColumnIndex).Name = "PrecioF" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If GridF.Columns.Item(GridF.CurrentCell.ColumnIndex).Name = "DescuentoF" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If GridF.Columns.Item(GridF.CurrentCell.ColumnIndex).Name = "KilosXUnidadF" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChangedF_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If GridF.Columns.Item(GridF.CurrentCell.ColumnIndex).Name = "PrecioF" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 3)
            End If
        End If

        If GridF.Columns.Item(GridF.CurrentCell.ColumnIndex).Name = "DescuentoF" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

        If GridF.Columns.Item(GridF.CurrentCell.ColumnIndex).Name = "KilosXUnidadF" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 3)
            End If
        End If

    End Sub
    Private Sub Gridf_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles GridF.DataError
        Exit Sub
    End Sub

End Class