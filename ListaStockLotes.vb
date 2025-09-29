Public Class ListaStockLotes
    '  valor false equivale 0.
    '  valor true  equivale 1.
    Dim DtGrid As DataTable
    Dim DtCalibres As DataTable
    '
    Private WithEvents bs As New BindingSource
    '
    Dim SqlN As String
    Dim SqlB As String
    Private Sub ListaStockLotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        GeneraCombosGrid()

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboProveedor, "", "Proveedores")
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboArticulo, "", "Articulos")
        ComboArticulo.SelectedValue = 0
        With ComboArticulo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboEspecie, 1)
        With ComboEspecie
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboEspecie.SelectedValue = 0

        LlenaComboTablas(ComboVariedad, 2)
        With ComboVariedad
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboVariedad.SelectedValue = 0

        If Not PermisoTotal Then
            Panel2.Visible = False
            CheckCerrado.Checked = False
            Grid.Columns("Candado").Visible = False
        Else
            Grid.Columns("Candado").Visible = True
        End If

        ArmaDtCalibres(DtCalibres)

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub TrasabilidadLotes_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 AS Operacion,L.*,C.Costeo FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE L.Stock <> 0 "
        SqlN = "SELECT 2 AS Operacion,L.*,C.Costeo FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE L.Stock <> 0 "

        Dim SqlArticulo As String = ""
        If ComboArticulo.SelectedValue <> 0 Then
            SqlArticulo = "AND L.Articulo = " & ComboArticulo.SelectedValue & " "
        Else : SqlArticulo = "AND L.Articulo LIKE '%' "
        End If

        Dim Lote As Integer = 0
        Dim Secuencia As Integer = 0
        If Val(MaskedLote.Text) <> 0 Then
            ConsisteMaskedLote(MaskedLote.Text, Lote, Secuencia)
        End If

        Dim SqlLote As String = ""
        If Lote <> 0 Then
            SqlLote = "AND L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & " "
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "AND L.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND L.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlProveedor As String = ""
        If ComboProveedor.SelectedValue <> 0 Then
            SqlProveedor = "AND L.Proveedor = " & ComboProveedor.SelectedValue & " "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND L.Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        SqlB = SqlB & SqlArticulo & SqlLote & SqlFecha & SqlProveedor & SqlDeposito
        SqlN = SqlN & SqlArticulo & SqlLote & SqlFecha & SqlProveedor & SqlDeposito

        LLenaGrid()

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Stock Lotes.", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        UnaImpresionRecibo.PDesdeCtaCte = DateTimeDesde.Value
        UnaImpresionRecibo.PHastaCtaCte = DateTimeHasta.Value
        UnaImpresionRecibo.GridCompro = Grid
        UnaImpresionSaldoLotes()

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboArticulor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboArticulo.Validating

        If IsNothing(ComboArticulo.SelectedValue) Then ComboArticulo.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub ComboEspecie_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEspecie.Validating

        If IsNothing(ComboEspecie.SelectedValue) Then ComboEspecie.SelectedValue = 0

    End Sub
    Private Sub ComboVariedad_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboVariedad.Validating

        If IsNothing(ComboVariedad.SelectedValue) Then ComboVariedad.SelectedValue = 0

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
    Private Sub LLenaGrid()

        Dim LoteAnt As String = ""
        Dim LoteOrigenAnt As String = ""
        Dim Dt As New DataTable
        Dim Sql As String = ""
        Dim RowGrid As DataRow
        Dim Total As Decimal = 0
        Dim CostoTotal As Decimal = 0

        CreaDtGrid()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If
        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Sql = ""

        If ComboEspecie.SelectedValue <> 0 Then
            If Sql = "" Then
                Sql = "Especie = " & ComboEspecie.SelectedValue
            Else : Sql = "AND Especie = " & ComboEspecie.SelectedValue
            End If
        End If
        If ComboVariedad.SelectedValue <> 0 Then
            If Sql = "" Then
                Sql = "Variedad = " & ComboVariedad.SelectedValue
            Else : Sql = Sql & " AND Variedad = " & ComboVariedad.SelectedValue
            End If
        End If

        If Sql <> "" Then
            Sql = "WHERE " & Sql
            Dim DtArticulos As New DataTable
            If Not Tablas.Read("SELECT Clave,Especie,Variedad FROM Articulos " & Sql & ";", Conexion, DtArticulos) Then Me.Close() : Exit Sub
            Dim RowsBusqueda() As DataRow
            For Each Row As DataRow In Dt.Rows
                RowsBusqueda = DtArticulos.Select("Clave = " & Row("Articulo"))
                If RowsBusqueda.Length = 0 Then
                    Row.Delete()
                End If
            Next
            DtArticulos.Dispose()
        End If

        Dim View As New DataView

        View = Dt.DefaultView
        View.Sort = "Lote,Secuencia"

        For Each Row As DataRowView In View
            'Agrega registro en DtGrid.
            RowGrid = DtGrid.NewRow()
            RowGrid("Color") = 0
            RowGrid("Operacion") = Row("Operacion")
            RowGrid("Lote") = Row("Lote")
            RowGrid("Secuencia") = Row("Secuencia")
            RowGrid("Costeo") = Row("Costeo")
            RowGrid("Deposito") = Row("Deposito")
            RowGrid("Articulo") = Row("Articulo")
            If Row("Calibre") <> 0 Then RowGrid("Calibre") = TraeCalibre(Row("Calibre"))
            RowGrid("Proveedor") = Row("Proveedor")
            RowGrid("Fecha") = Row("Fecha")
            RowGrid("Cantidad") = Row("Cantidad")
            RowGrid("Stock") = Row("Stock")
            RowGrid("PrecioCosto") = Row("PrecioF")
            RowGrid("CostoTotal") = CalculaNeto(Row("PrecioF"), Row("Stock"))
            Total = Total + Row("Stock")
            CostoTotal = CostoTotal + RowGrid("CostoTotal")
            DtGrid.Rows.Add(RowGrid)
        Next

        If Total <> 0 Then AgregaTotal(Total, CostoTotal)

        Dt.Dispose()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

    End Sub
    Private Sub AgregaTotal(ByVal Total As Decimal, ByVal CostoTotal As Decimal)

        Dim Cartel As String = "TOTAL STOCK:  "
        Dim RowGrid As DataRow

        RowGrid = DtGrid.NewRow()
        RowGrid("Color") = 2
        RowGrid("Stock") = Total
        RowGrid("CostoTotal") = CostoTotal
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Sub GeneraCombosGrid()

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        Deposito.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

        Articulo.DataSource = TodosLosArticulos()
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

        Costeo.DataSource = Tablas.Leer("SELECT Costeo,Nombre FROM Costeos;")
        Costeo.DisplayMember = "Nombre"
        Costeo.ValueMember = "Costeo"

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Color As New DataColumn("Color")
        Color.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Color)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Costeo As New DataColumn("Costeo")
        Costeo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Costeo)

        Dim Deposito As New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cantidad)

        Dim Baja As New DataColumn("Stock")
        Baja.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Baja)

        Dim PrecioCosto As New DataColumn("PrecioCosto")
        PrecioCosto.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioCosto)

        Dim CostoTotal As New DataColumn("CostoTotal")
        CostoTotal.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CostoTotal)

        Dim Calibre As New DataColumn("Calibre")
        Calibre.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Calibre)

    End Sub
    Private Sub ArmaDtCalibres(ByRef Dt As DataTable)

        Dt = New DataTable

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        Dt.Columns.Add(Nombre)

        Dt = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 5;")

    End Sub
    Private Function TraeCalibre(ByVal Calibre As Integer) As String

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtCalibres.Select("Clave = " & Calibre)
        If RowsBusqueda.Length <> 0 Then Return RowsBusqueda(0).Item("Nombre")

    End Function
    Private Function Valida() As Boolean

        Dim Lote As Integer = 0
        Dim Secuencia As Integer = 0
        If Val(MaskedLote.Text) <> 0 Then
            If Not ConsisteMaskedLote(MaskedLote.Text, Lote, Secuencia) Then
                MaskedLote.Focus()
                Return False
            End If
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) And _
              Not IsNothing(Grid.Rows(e.RowIndex).Cells("Lote").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
            End If
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
            End If
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 2 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.Yellow
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) And Not IsNothing(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Stock" Then
            If Not IsDBNull(e.Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "PrecioCosto" Or Grid.Columns(e.ColumnIndex).Name = "CostoTotal" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 2)
                Else : e.Value = ""
                End If
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        Exit Sub

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        Dim Abierto As Boolean

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Grid.Rows(e.RowIndex).Cells("Secuencia").Value > 99 Then
                MsgBox("Debe Elegir un Lote Original", MsgBoxStyle.Information)
                Exit Sub
            End If
            If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
                Abierto = True
            Else
                Abierto = False
            End If
            UnLote.PAbierto = Abierto
            UnLote.PLote = Grid.Rows(e.RowIndex).Cells("Lote").Value
            UnLote.PSecuencia = Grid.Rows(e.RowIndex).Cells("Secuencia").Value
            UnLote.ShowDialog()
            UnLote.Dispose()
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub

End Class