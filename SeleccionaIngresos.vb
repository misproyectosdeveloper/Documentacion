Public Class SeleccionaIngresos
    Public PEmisor As Integer
    Public PLote As Integer
    Public POperacion As Integer
    Public PAceptado As Boolean
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub SeleccionaLotesAfectados_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        Me.Top = 50

        LlenaCombo(ComboProveedor, "", "Proveedores")
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
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
        ComboTipoOperacion.SelectedValue = 0
        With ComboTipoOperacion
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        BuscaCosteo(0)

        CreaDtGrid()

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
        End If

        PLote = 0

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar.Focus() : ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim SqlFecha As String
        SqlFecha = "AND Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlProveedor As String = ""
        If ComboProveedor.SelectedValue <> 0 Then
            SqlProveedor = " AND Proveedor = " & ComboProveedor.SelectedValue
        End If

        Dim SqlTipoOperacion As String = ""
        If ComboTipoOperacion.SelectedValue <> 0 Then
            SqlTipoOperacion = " AND TipoOperacion = " & ComboTipoOperacion.SelectedValue
        End If

        Dim SqlLote As String = ""
        If TextLote.Text <> "" Then
            If CInt(TextLote.Text) <> 0 Then
                SqlLote = " AND Lote = " & TextLote.Text
            End If
        End If

        Dim SqlCosteo As String = ""
        If ComboCosteo.SelectedValue <> 0 Then
            SqlLote = " AND Costeo = " & ComboCosteo.SelectedValue
        End If

        SqlB = "SELECT 1 as Operacion,Lote,Proveedor,Fecha,Guia,Remito,TipoOperacion FROM IngresoMercaderiasCabeza WHERE RemitoCliente = 0 "
        SqlN = "SELECT 2 as Operacion,Lote,Proveedor,Fecha,Guia,Remito,TipoOperacion FROM IngresoMercaderiasCabeza WHERE RemitoCliente = 0 "

        SqlB = SqlB & SqlFecha & SqlProveedor & SqlTipoOperacion & SqlLote & SqlCosteo & ";"
        SqlN = SqlN & SqlFecha & SqlProveedor & SqlTipoOperacion & SqlLote & SqlCosteo & ";"

        LlenaGrid()

    End Sub
    Private Sub ButtonElegir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonElegir.Click

        Dim Con As Integer

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value = True Then Con = Con + 1
        Next

        If Con > 1 Then
            MsgBox("Debe Elegir un Solo Items.", MsgBoxStyle.Critical)
            Exit Sub
        End If

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value Then
                PLote = Row.Cells("Lote").Value
                POperacion = Row.Cells("Operacion").Value
            End If
        Next
        PAceptado = True

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
    Private Sub ComboTipoOperacion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipoOperacion.Validating

        If IsNothing(ComboTipoOperacion.SelectedValue) Then ComboTipoOperacion.SelectedValue = 0

    End Sub
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub TextLote_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextLote.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

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
            If Ok Then
                Dim Row1 As DataRow = DtGrid.NewRow
                Row1("Marca") = 0
                Row1("Operacion") = Row("Operacion")
                Row1("Lote") = Row("Lote")
                Row1("TipoOperacion") = Row("TipoOperacion")
                Row1("Proveedor") = Row("Proveedor")
                Row1("Fecha") = Row("Fecha")
                Row1("Remito") = Row("Remito")
                Row1("Guia") = Row("Guia")
                DtGrid.Rows.Add(Row1)
            End If
        Next

        Dt.Dispose()

        Dim RowsBusqueda() As DataRow
        'Marca los seleccionados.
        RowsBusqueda = DtGrid.Select("Lote = " & PLote)
        If RowsBusqueda.Length <> 0 Then
            RowsBusqueda(0).Item("Marca") = 1
        End If

        Dim View As New DataView
        View = DtGrid.DefaultView
        View.Sort = "Lote"

        Grid.DataSource = bs
        bs.DataSource = View

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

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Row = Proveedor.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Proveedor.DataSource.Rows.Add(Row)
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        TipoOperacion.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Miselaneas WHERE Codigo = 1;")
        Row = TipoOperacion.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        TipoOperacion.DataSource.Rows.Add(Row)
        TipoOperacion.DisplayMember = "Nombre"
        TipoOperacion.ValueMember = "Clave"

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

        If Grid.Columns(e.ColumnIndex).Name = "Lote" Then
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