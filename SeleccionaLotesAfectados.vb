Public Class SeleccionaLotesAfectados
    Public PEsConsumoInsumos As Boolean
    Public PEsConceptosGastos As Boolean
    Public PEsPorCuentaYOrden As Boolean
    Public PEsRecibo As Boolean
    Public PBorraUsados As Boolean
    Public PConceptoGasto As Integer
    Public PEmisor As Integer
    Public PListaDeLotes As List(Of FilaComprobanteFactura)
    Public PTipoOperacion As Integer
    Public PLote As Integer
    Public PSecuencia As Integer
    Public PAceptado As Boolean
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub SeleccionaLotesAfectados_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        LlenaCombo(ComboProveedor, "", "Proveedores")
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboArticulo, "", "Articulos")
        ComboArticulo.DisplayMember = "Nombre"
        ComboArticulo.ValueMember = "Clave"
        ComboArticulo.SelectedValue = 0
        With ComboArticulo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboTipoOperacion.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Miselaneas WHERE Codigo = 1;")
        Dim Row As DataRow = ComboTipoOperacion.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboTipoOperacion.DataSource.rows.add(Row)
        ComboTipoOperacion.DisplayMember = "Nombre"
        ComboTipoOperacion.ValueMember = "Clave"
        ComboTipoOperacion.SelectedValue = PTipoOperacion
        With ComboTipoOperacion
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        BuscaCosteo(0)

        ArmaGrid()

        CreaDtGrid()

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
        End If

        PLote = 0
        PSecuencia = 0

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar.Focus() : ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Lote As Integer = 0
        Dim Secuencia As Integer = 0
        If Val(MaskedLote.Text) <> 0 Then
            If Not ConsisteMaskedLote(MaskedLote.Text, Lote, Secuencia) Then Exit Sub
        End If

        Dim SqlFecha As String
        SqlFecha = " AND L.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND L.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlProveedor As String = ""
        If ComboProveedor.SelectedValue <> 0 Then
            SqlProveedor = " AND L.Proveedor = " & ComboProveedor.SelectedValue
        End If

        Dim SqlArticulo As String = ""
        If ComboArticulo.SelectedValue <> 0 Then
            SqlArticulo = " AND L.Articulo = " & ComboArticulo.SelectedValue
        End If

        Dim SqlTipoOperacion As String = ""
        If ComboTipoOperacion.SelectedValue <> 0 Then
            SqlTipoOperacion = " AND L.TipoOperacion = " & ComboTipoOperacion.SelectedValue
        End If

        Dim SqlLote As String = ""
        If Lote <> 0 Then
            SqlLote = " AND L.Lote = " & Lote & " AND Secuencia = " & Secuencia
        End If

        Dim SqlCosteo As String = ""
        If ComboCosteo.SelectedValue <> 0 Then
            SqlLote = " AND C.Costeo = " & ComboCosteo.SelectedValue
        End If

        SqlB = "SELECT 1 as Operacion,L.Cantidad,L.Baja,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.TipoOperacion,L.Fecha,C.Guia,C.Remito"
        SqlN = "SELECT 2 as Operacion,L.Cantidad,L.Baja,L.Lote,L.Secuencia,L.Articulo,L.Proveedor,L.TipoOperacion,L.Fecha,C.Guia,C.Remito"

        If PEsPorCuentaYOrden Then
            SqlB = SqlB & " FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE (L.Cantidad - L.Baja) > 0 AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.Proveedor = " & PEmisor
            SqlN = SqlN & " FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE (L.Cantidad - L.Baja) > 0 AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND L.Proveedor = " & PEmisor
        Else
            SqlB = SqlB & " FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE (L.Cantidad - L.Baja) > 0 AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen"
            SqlN = SqlN & " FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE (L.Cantidad - L.Baja) > 0 AND L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen"
        End If

        SqlB = SqlB & SqlFecha & SqlProveedor & SqlArticulo & SqlTipoOperacion & SqlLote & SqlCosteo & ";"
        SqlN = SqlN & SqlFecha & SqlProveedor & SqlArticulo & SqlTipoOperacion & SqlLote & SqlCosteo & ";"

        If PEsConsumoInsumos Then LlenaGrid()
        If PEsRecibo Then LlenaGridRecibo()
        If PEsConceptosGastos Then LlenaGridConceptosGastos()

    End Sub
    Private Sub ButtonElegir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonElegir.Click

        Dim Con As Integer

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value = True Then Con = Con + 1
        Next

        If PEsConsumoInsumos Then
            If Con = 0 Then
                MsgBox("Debe Elegir Items.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Exit Sub
            End If
            PListaDeLotes.Clear()
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    Dim Item As New FilaComprobanteFactura
                    Item.Lote = Row.Cells("Lote").Value
                    Item.Secuencia = Row.Cells("Secuencia").Value
                    Item.Cantidad = Row.Cells("Ingresado").Value
                    PListaDeLotes.Add(Item)
                End If
            Next
            PAceptado = True
        End If

        If PEsConceptosGastos Then
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    Dim Item As New FilaComprobanteFactura
                    Item.Lote = Row.Cells("Lote").Value
                    Item.Secuencia = Row.Cells("Secuencia").Value
                    Item.Operacion = Row.Cells("Operacion").Value
                    Item.Ingreso = Row.Cells("Ingresado").Value
                    Item.Remito = Row.Cells("Remito").Value
                    If Item.Remito = 0 Then Item.Remito = Row.Cells("Guia").Value
                    Item.Fecha = Row.Cells("Fecha").Value
                    PListaDeLotes.Add(Item)
                End If
            Next
            PAceptado = True
        End If

        If PEsRecibo Then
            PListaDeLotes.Clear()
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    Dim Item As New FilaComprobanteFactura
                    Item.Lote = Row.Cells("Lote").Value
                    Item.Secuencia = Row.Cells("Secuencia").Value
                    Item.Operacion = Row.Cells("Operacion").Value
                    Item.Cantidad = Row.Cells("Ingresado").Value
                    Item.Remito = Row.Cells("Remito").Value
                    If Item.Remito = 0 Then Item.Remito = Row.Cells("Guia").Value
                    Item.Fecha = Row.Cells("Fecha").Value
                    PListaDeLotes.Add(Item)
                End If
            Next
            PAceptado = True
        End If

        Me.Close()

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

        BuscaCosteo(0)

        If ComboProveedor.SelectedValue <> 0 Then
            If EsUnNegocio(ComboProveedor.SelectedValue) Then
                BuscaCosteo(ComboProveedor.SelectedValue)
            End If
        End If

    End Sub
    Private Sub ComboArticulo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboArticulo.Validating

        If IsNothing(ComboArticulo.SelectedValue) Then ComboArticulo.SelectedValue = 0

    End Sub
    Private Sub ComboTipoOperacion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipoOperacion.Validating

        If IsNothing(ComboTipoOperacion.SelectedValue) Then ComboTipoOperacion.SelectedValue = 0

    End Sub
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

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
    Private Sub LlenaGrid()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        Dim Dt As New DataTable

        DtGrid.Clear()

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim Ok As Boolean

        For Each Row As DataRow In Dt.Rows
            Ok = True
            If Row("TipoOperacion") = 4 Then
                If VerCierreCosteo(Row("Operacion"), Row("Lote")) < 0 Then Ok = False
            End If
            If Row("Cantidad") - Row("Baja") = 0 Then Ok = False
            If Ok Then
                Dim Row1 As DataRow = DtGrid.NewRow
                Row1("Marca") = 0
                Row1("Operacion") = Row("Operacion")
                Row1("Lote") = Row("Lote")
                Row1("Secuencia") = Row("Secuencia")
                Row1("Ingresado") = Row("Cantidad") - Row("Baja")
                Row1("Articulo") = Row("Articulo")
                Row1("TipoOperacion") = Row("TipoOperacion")
                Row1("Proveedor") = Row("Proveedor")
                Row1("Fecha") = Row("Fecha")
                Row1("Remito") = Row("Remito")
                Row1("Guia") = Row("Guia")
                Row1("Importe") = 0
                DtGrid.Rows.Add(Row1)
            End If
        Next

        Dt.Dispose()

        Dim RowsBusqueda() As DataRow
        'Marca los seleccionados.
        For Each Item As FilaComprobanteFactura In PListaDeLotes
            RowsBusqueda = DtGrid.Select("Lote = " & Item.Lote & " AND Secuencia = " & Item.Secuencia)
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Marca") = 1
            End If
        Next

        Dim View As New DataView
        View = DtGrid.DefaultView
        View.Sort = "Lote,Secuencia"

        Grid.DataSource = bs
        bs.DataSource = View

        Dt.Dispose()

    End Sub
    Private Sub LlenaGridRecibo()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        Dim Dt As New DataTable

        DtGrid.Clear()

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        For Each Row As DataRow In Dt.Rows
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Marca") = 0
            Row1("Operacion") = Row("Operacion")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("Ingresado") = Row("Cantidad") - Row("Baja")
            Row1("Articulo") = Row("Articulo")
            Row1("Proveedor") = Row("Proveedor")
            Row1("Fecha") = Row("Fecha")
            Row1("Remito") = Row("Remito")
            Row1("Guia") = Row("Guia")
            Row1("Importe") = 0
            DtGrid.Rows.Add(Row1)
        Next

        If PEsPorCuentaYOrden Then
            Dim Sql As String
            For Each Row As DataRow In DtGrid.Rows
                Sql = "SELECT L.Lote,L.Secuencia,L.ImporteConIva FROM RecibosLotes AS L INNER JOIN RecibosCabeza AS C ON L.TipoNota = C.TipoNota AND L.Nota = C.Nota " & _
                          "WHERE C.Estado = 1 AND C.TipoNota = 600 AND C.ACuenta <> 0 AND L.Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & ";"
                Dt = New DataTable
                If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
                If PermisoTotal Then
                    If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
                End If
                For Each Row1 As DataRow In Dt.Rows
                    Row("Importe") = Row("Importe") + Row1("ImporteConIva")
                Next
            Next
        End If

        Dt.Dispose()

        Dim RowsBusqueda() As DataRow
        'Borra los seleccionados.
        For Each Item As FilaComprobanteFactura In PListaDeLotes
            RowsBusqueda = DtGrid.Select("Lote = " & Item.Lote & " AND Secuencia = " & Item.Secuencia)
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Delete()
            End If
        Next

        Dim View As New DataView
        View = DtGrid.DefaultView
        View.Sort = "Lote,Secuencia"

        Grid.DataSource = bs
        bs.DataSource = View

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dt.Dispose()

    End Sub
    Private Sub LlenaGridConceptosGastos()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        Dim Dt As New DataTable
        Dim Sql As String

        DtGrid.Clear()

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim Ok As Boolean

        For Each Row As DataRow In Dt.Rows
            Ok = True
            If Ok Then
                Dim Row1 As DataRow = DtGrid.NewRow
                Row1("Marca") = 0
                Row1("Operacion") = Row("Operacion")
                Row1("Lote") = Row("Lote")
                Row1("Secuencia") = Row("Secuencia")
                Row1("Articulo") = Row("Articulo")
                Row1("TipoOperacion") = Row("TipoOperacion")
                Row1("Proveedor") = Row("Proveedor")
                Row1("Fecha") = Row("Fecha")
                Row1("Remito") = Row("Remito")
                Row1("Guia") = Row("Guia")
                Row1("Importe") = 0
                Row1("Ingresado") = Row("Cantidad") - Row("Baja")
                DtGrid.Rows.Add(Row1)
            End If
        Next
        For Each Row As DataRow In DtGrid.Rows
            Sql = "SELECT L.ImporteConIva AS Importe FROM ComproFacturados AS L INNER JOIN FacturasProveedorCabeza AS C ON L.Factura = C.Factura " & _
                    "WHERE C.EsAfectaCostoLotes = 1 AND C.Estado = 1 AND C.ConceptoGasto = " & PConceptoGasto & " AND L.Lote = " & Row("Lote") & " AND L.Secuencia = " & Row("Secuencia") & ";"
            Dt.Clear()
            If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
            For Each Row1 As DataRow In Dt.Rows
                Row("Importe") = Row("Importe") + Row1("Importe")
            Next
            If PermisoTotal Then
                Dt.Clear()
                If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
                For Each Row1 As DataRow In Dt.Rows
                    Row("Importe") = Row("Importe") + Row1("Importe")
                Next
            End If
        Next

        Dt.Dispose()

        Dim RowsBusqueda() As DataRow
        If PBorraUsados Then
            'Borra los seleccionados.
            For Each Item As FilaComprobanteFactura In PListaDeLotes
                RowsBusqueda = DtGrid.Select("Lote = " & Item.Lote & " AND Secuencia = " & Item.Secuencia)
                If RowsBusqueda.Length <> 0 Then
                    RowsBusqueda(0).Delete()
                End If
            Next
        Else
            'Marca los seleccionados.
            For Each Item As FilaComprobanteFactura In PListaDeLotes
                RowsBusqueda = DtGrid.Select("Lote = " & Item.Lote & " AND Secuencia = " & Item.Secuencia)
                If RowsBusqueda.Length <> 0 Then
                    RowsBusqueda(0).Item("Marca") = 1
                End If
            Next
        End If

        Dim View As New DataView
        View = DtGrid.DefaultView
        View.Sort = "Lote,Secuencia"

        Grid.DataSource = bs
        bs.DataSource = View

        PListaDeLotes.Clear()

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        Dt.Dispose()

    End Sub
    Private Sub BuscaCosteo(ByVal Negocio As Integer)

        Dim SqlFecha As String = ""
        SqlFecha = "IntFechaDesde <= " & Format(DateTimeHasta.Value, "yyyyMMdd") & " AND IntFechaHasta >= " & Format(DateTimeDesde.Value, "yyyyMMdd") & ";"
        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & Negocio & " AND " & SqlFecha
        ComboCosteo.DataSource = New DataTable
        ComboCosteo.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboCosteo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        ComboCosteo.DataSource.rows.add(Row)
        ComboCosteo.DisplayMember = "Nombre"
        ComboCosteo.ValueMember = "Costeo"
        ComboCosteo.SelectedValue = 0
        With ComboCosteo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Private Sub ArmaGrid()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridImageBox As DataGridViewImageColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn
        Dim GridComboBox As DataGridViewComboBoxColumn

        Grid.AutoGenerateColumns = False

        '      Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Grid.AllowUserToAddRows = False
        Grid.AllowUserToDeleteRows = False
        Grid.AlternatingRowsDefaultCellStyle.BackColor = Color.AntiqueWhite
        Grid.DefaultCellStyle.BackColor = Color.WhiteSmoke
        Grid.BackgroundColor = Color.White
        Grid.Columns.Clear()

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        '     style.ForeColor = Color.IndianRed
        '     style.BackColor = Color.Ivory
        style.Font = New Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point)

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 40
        GridChekBox.Width = 40
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Marca"
        GridTextBox.Visible = False
        GridTextBox.ReadOnly = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Marca").DataPropertyName = "Marca"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "Lote"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        GridTextBox.ReadOnly = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Lote").DataPropertyName = "Lote"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Secuencia"
        GridTextBox.Name = "Secuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.Visible = False
        GridTextBox.ReadOnly = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Secuencia").DataPropertyName = "Secuencia"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.DefaultCellStyle = style
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "Lote"
        GridTextBox.Name = "LoteYSecuencia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 60
        GridTextBox.MaxInputLength = 60
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Ingresado"
        GridTextBox.Name = "Ingresado"
        GridTextBox.ReadOnly = True
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Ingresado").DataPropertyName = "Ingresado"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.MinimumWidth = 100
        GridTextBox.MaxInputLength = 100
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Fecha"
        GridTextBox.Name = "Fecha"
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Fecha").DataPropertyName = "Fecha"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.SortMode = DataGridViewColumnSortMode.Automatic
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 150
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Articulo"
        GridComboBox.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Articulos;")
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Articulo"
        GridTextBox.ReadOnly = True
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Articulo").DataPropertyName = "Articulo"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.SortMode = DataGridViewColumnSortMode.Automatic
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 150
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Proveedor"
        GridComboBox.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Proveedor"
        GridTextBox.ReadOnly = True
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Proveedor").DataPropertyName = "Proveedor"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Remito"
        GridTextBox.Name = "Remito"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridTextBox.ReadOnly = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Remito").DataPropertyName = "Remito"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.MinimumWidth = 50
        GridTextBox.MaxInputLength = 50
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Guia"
        GridTextBox.Name = "Guia"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridTextBox.ReadOnly = True
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Guia").DataPropertyName = "Guia"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.SortMode = DataGridViewColumnSortMode.Automatic
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.MinimumWidth = 70
        GridComboBox.ReadOnly = True
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = ""

        GridComboBox.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Miselaneas WHERE Codigo = 1;")
        Dim Row As DataRow = GridComboBox.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        GridComboBox.DataSource.rows.add(Row)
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "TipoOperacion"
        GridComboBox.ReadOnly = True
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("TipoOperacion").DataPropertyName = "TipoOperacion"

        If PEsConceptosGastos Or PEsPorCuentaYOrden Then
            GridTextBox = New DataGridViewTextBoxColumn
            GridTextBox.DefaultCellStyle.ForeColor = Color.Black
            GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            GridTextBox.MinimumWidth = 100
            GridTextBox.MaxInputLength = 100
            GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            GridTextBox.HeaderText = "Importe Ant."
            GridTextBox.Name = "Importe"
            GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            GridTextBox.ReadOnly = True
            Grid.Columns.Add(GridTextBox)
            Grid.Columns.Item("Importe").DataPropertyName = "Importe"
        End If

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Marca As New DataColumn("Marca")
        Marca.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Marca)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Ingresado As New DataColumn("Ingresado")
        Ingresado.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Ingresado)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim TipoOperacion As New DataColumn("TipoOperacion")
        TipoOperacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoOperacion)

        Dim Remito As New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Remito)

        Dim Guia As New DataColumn("Guia")
        Guia.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Guia)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Importe)

    End Sub
    Private Function VerCierreCosteo(ByVal Operacion As Integer, ByVal Lote As Integer) As Integer

        Dim Costeo As Integer = HallaCosteoLote(Operacion, Lote)
        If Costeo <= 0 Then Return -1
        Dim Cerrado As Boolean = HallaCosteoCerrado(Costeo)
        If Cerrado Then Return -1

        Return 0

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
            If PermisoTotal Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                End If
            End If
            If Grid.Rows(e.RowIndex).Cells("Marca").Value = 1 Then
                Grid.Rows(e.RowIndex).Cells("Sel").Value = True
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = Format(e.Value, "0000-00000000")
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Guia" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 0)
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If

    End Sub
End Class