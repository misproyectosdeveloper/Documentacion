Public Class OpcionReproceso1
    Public PAbierto As Boolean
    Public PDeposito As Integer
    Public PDtGridBaja As DataTable
    Public PRegresar As Boolean
    '
    Dim DtArticulosConStock As DataTable
    Dim DtGridBaja As DataTable
    '
    Dim ArticuloAnt As Integer
    Dim DepositoAnt As Integer
    Private Sub OpcionIngreso_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GridBajas.AutoGenerateColumns = False
        GridBajas.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        GeneraCombosGrid()

        PDeposito = 0
        ArticuloAnt = 0
        DepositoAnt = 0

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ArmaArticulosConStock()

        With ComboArticulo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        TextTotalBaja.Text = "0,00"

        PRegresar = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        For Each Row As DataGridViewRow In GridBajas.Rows
            If Row.Cells("Baja").Value <> 0 Then
                If Row.Cells("Operacion").Value = 1 Then
                    PAbierto = True
                Else : PAbierto = False
                End If
                Exit For
            End If
        Next
        PDtGridBaja = DtGridBaja
        PDeposito = ComboDeposito.SelectedValue
        PRegresar = False

        Me.Close()

    End Sub
    Private Sub ButtonVerLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerLotes.Click

        If ComboArticulo.SelectedValue = 0 Then
            MsgBox("Falta Articulo.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            ComboArticulo.Focus()
            Exit Sub
        End If
        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Deposito.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Exit Sub
        End If

        LLenaGrid()

    End Sub
    Private Sub ComboArticulo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboArticulo.KeyUp

        If e.KeyValue = 13 Then ComboArticulo_Validating(Nothing, Nothing)

    End Sub
    Private Sub ComboArticulo_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboArticulo.SelectionChangeCommitted

        ComboArticulo_Validating(Nothing, Nothing)

    End Sub
    Private Sub ComboArticulo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboArticulo.Validating

        If IsNothing(ComboArticulo.SelectedValue) Then ComboArticulo.SelectedValue = 0

        If ComboArticulo.SelectedValue = ArticuloAnt Then Exit Sub
        ArticuloAnt = ComboArticulo.SelectedValue

        GridBajas.DataSource = Nothing

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If ComboDeposito.SelectedValue = DepositoAnt Then Exit Sub

        DepositoAnt = ComboDeposito.SelectedValue

        GridBajas.DataSource = Nothing

        ArmaArticulosConStock()

    End Sub
    Private Sub LLenaGrid()

        CreaDtGridBaja()

        Dim Dt As New DataTable

        CambiarPuntoDecimal(".")

        Dim SqlB As String = "SELECT 1 as Operacion,Lote,Secuencia,Proveedor,Stock,KilosXUnidad,Fecha,Articulo,Calibre FROM Lotes WHERE Stock > " & 0 & " AND Deposito = " & ComboDeposito.SelectedValue & " AND Articulo = " & ComboArticulo.SelectedValue & ";"
        Dim SqlN As String = "SELECT 2 as Operacion,Lote,Secuencia,Proveedor,Stock,KilosXUnidad,Fecha,Articulo,Calibre FROM Lotes WHERE Stock > " & 0 & " AND Deposito = " & ComboDeposito.SelectedValue & " AND Articulo = " & ComboArticulo.SelectedValue & ";"

        CambiarPuntoDecimal(",")

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Lote,Secuencia"

        Dim Row As DataRowView

        'Prepara Grid de Baja.
        For Each Row In View
            Dim Row1 As DataRow = DtGridBaja.NewRow
            Row1("Operacion") = Row("Operacion")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("Articulo") = Row("Articulo")
            Row1("Proveedor") = Row("Proveedor")
            Row1("Stock") = Row("Stock")
            Row1("KilosXUnidad") = Row("KilosXUnidad")
            Row1("Calibre") = Row("Calibre")
            Row1("Fecha") = Row("Fecha")
            Row1("AGranel") = EsArticuloAGranel(Row("Articulo"))
            If Row1("AGranel") Then
                Row1("Medida") = HallaUMedidaArticulo(Row("Articulo"))
            Else : Row1("Medida") = "Un"
            End If
            Row1("Merma") = 0
            Row1("Baja") = 0
            DtGridBaja.Rows.Add(Row1)
        Next

        GridBajas.DataSource = DtGridBaja

        AddHandler DtGridBaja.RowChanging, New DataRowChangeEventHandler(AddressOf DtGridBaja_RowChanging)
        AddHandler DtGridBaja.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGridBaja_ColumnChanging)

        Dt.Dispose()

    End Sub
    Private Sub CreaDtGridBaja()

        DtGridBaja = New DataTable

        Dim Operacion As DataColumn = New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGridBaja.Columns.Add(Operacion)

        Dim AGranel As DataColumn = New DataColumn("AGranel")
        AGranel.DataType = System.Type.GetType("System.Boolean")
        DtGridBaja.Columns.Add(AGranel)

        Dim KilosXUnidad As DataColumn = New DataColumn("KilosXUnidad")
        KilosXUnidad.DataType = System.Type.GetType("System.Double")
        DtGridBaja.Columns.Add(KilosXUnidad)

        Dim Lote As DataColumn = New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGridBaja.Columns.Add(Lote)

        Dim Secuencia As DataColumn = New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGridBaja.Columns.Add(Secuencia)

        Dim LoteySecuencia As DataColumn = New DataColumn("LoteySecuencia")
        LoteySecuencia.DataType = System.Type.GetType("System.String")
        DtGridBaja.Columns.Add(LoteySecuencia)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGridBaja.Columns.Add(Articulo)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGridBaja.Columns.Add(Proveedor)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGridBaja.Columns.Add(Fecha)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Decimal")
        DtGridBaja.Columns.Add(Stock)

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGridBaja.Columns.Add(Medida)

        Dim Calibre As New DataColumn("Calibre")
        Calibre.DataType = System.Type.GetType("System.Int32")
        DtGridBaja.Columns.Add(Calibre)

        Dim Baja As New DataColumn("Baja")
        Baja.DataType = System.Type.GetType("System.Decimal")
        DtGridBaja.Columns.Add(Baja)

        Dim Merma As New DataColumn("Merma")
        Merma.DataType = System.Type.GetType("System.Decimal")
        DtGridBaja.Columns.Add(Merma)

    End Sub
    Private Sub GeneraCombosGrid()

        Proveedor.DataSource = ProveedoresDeFrutasYCosteo()
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        Calibre.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 5 ORDER BY Nombre;")
        Dim Row As DataRow = Calibre.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Calibre.DataSource.Rows.Add(Row)
        Calibre.DisplayMember = "Nombre"
        Calibre.ValueMember = "Clave"

    End Sub
    Private Sub ArmaArticulosConStock()

        Dim Sql As String = "SELECT DISTINCT A.Clave,A.Nombre FROM Lotes AS L INNER JOIN Articulos AS A ON L.Articulo = A.Clave WHERE L.Stock <> 0 AND L.Deposito = " & ComboDeposito.SelectedValue & ";"
        Dim SqlN As String = "SELECT DISTINCT Articulo FROM Lotes WHERE Stock <> 0 AND Deposito = " & ComboDeposito.SelectedValue & ";"

        DtArticulosConStock = New DataTable
        If Not Tablas.Read(Sql, Conexion, DtArticulosConStock) Then End

        Dim DtN As New DataTable
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtN) Then End
        End If

        Dim RowsBusqueda() As DataRow
        Dim Row As DataRow

        For Each Row In DtArticulosConStock.Rows
            Row("Nombre") = HallaNombreArticulo(Row("Clave"))
        Next

        For Each Row In DtN.Rows
            RowsBusqueda = DtArticulosConStock.Select("Clave = " & Row("Articulo"))
            If RowsBusqueda.Length = 0 Then
                Dim Row1 As DataRow = DtArticulosConStock.NewRow
                Row1("Clave") = Row("Articulo")
                Row1("Nombre") = HallaNombreArticulo(Row("Articulo"))
                DtArticulosConStock.Rows.Add(Row1)
            End If
        Next
        DtN.Dispose()

        Row = DtArticulosConStock.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        DtArticulosConStock.Rows.Add(Row)

        ComboArticulo.DataSource = Nothing
        ComboArticulo.DataSource = DtArticulosConStock
        ComboArticulo.DisplayMember = "Nombre"
        ComboArticulo.ValueMember = "Clave"
        ComboArticulo.SelectedValue = 0

    End Sub
    Private Function HallaNombreArticulo(ByVal Articulo As Integer) As String

        Dim Dt As New DataTable
        Dim Senia As Double = 0

        Dt = Tablas.Leer("SELECT A.Clave,A.Nombre,A.Envase,E.Kilos,E.Unidad FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave WHERE A.Clave = " & Articulo & ";")

        BuscaVigencia(10, Date.Now, Senia, Dt.Rows(0).Item("Envase"))
        HallaNombreArticulo = Dt.Rows(0).Item("Nombre") & " (S:" & FormatNumber(Senia, GDecimales) & " " & Dt.Rows(0).Item("Unidad") & ":" & Dt.Rows(0).Item("Kilos") & ")"

        Dt.Dispose()

    End Function
    Private Function Valida() As Boolean

        If DtGridBaja.HasErrors Then
            MsgBox("Debe Corregir Errores antes de Realizar los Cambios.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If

        If ComboArticulo.SelectedValue = 0 Then
            MsgBox("Falta Articulo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboArticulo.Focus()
            Return False
        End If

            If ComboDeposito.SelectedValue = 0 Then
                MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboDeposito.Focus()
                Return False
            End If

            If GridBajas.Rows.Count = 0 Then
                MsgBox("No Existe Lotes en el Deposito.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            If CDec(TextTotalBaja.Text) = 0 Then
                MsgBox("Debe Informar Lotes a Reprocesar.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            Dim Operacion As Integer = 0
            For Each Row As DataRow In DtGridBaja.Rows
                If Row("Baja") <> 0 Then
                    If Operacion = 0 Then Operacion = Row("Operacion")
                    If Row("Operacion") <> Operacion Then
                        MsgBox("Lotes deben Tener Igual Candado.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                        Return False
                    End If
                End If
            Next

            Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID BAJA.             --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridBajas_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridBajas.CellEnter

        'para manejo del autocoplete de articulos.
        If Not GridBajas.Columns(e.ColumnIndex).ReadOnly Then
            GridBajas.BeginEdit(True)
        End If

    End Sub
    Private Sub GridBajas_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridBajas.CellEndEdit

        Dim Baja As Decimal = 0

        For Each Row As DataRow In DtGridBaja.Rows
            Baja = Baja + Row("Baja")
        Next
        TextTotalBaja.Text = FormatNumber(Baja, 2)

    End Sub
    Private Sub GridBajas_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles GridBajas.EditingControlShowing

        Dim columna As Integer = GridBajas.CurrentCell.ColumnIndex

        If GridBajas.Columns.Item(GridBajas.CurrentCell.ColumnIndex).Name = "Baja" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloDecimales_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub SoloDecimales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If GridBajas.Columns.Item(GridBajas.CurrentCell.ColumnIndex).Name = "Baja" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If CType(sender, TextBox).Text = "" Then Exit Sub

        If GridBajas.Columns.Item(GridBajas.CurrentCell.ColumnIndex).Name = "Baja" Then
            EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
        End If

    End Sub
    Private Sub GridBajas_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles GridBajas.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If GridBajas.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Not IsNothing(GridBajas.Rows(e.RowIndex).Cells("Lote").Value) Then
                e.Value = GridBajas.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(GridBajas.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
                If PermisoTotal Then
                    If GridBajas.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then GridBajas.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                    If GridBajas.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then GridBajas.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If GridBajas.Columns(e.ColumnIndex).Name = "Fecha" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If GridBajas.Columns(e.ColumnIndex).Name = "Baja" Then
            If Not IsNothing(GridBajas.Rows(e.RowIndex).Cells("Baja").Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else : e.Value = Format(e.Value, "0.00")
                End If
            End If
        End If

    End Sub
    Private Sub DtGridBaja_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Baja")) Then
            If IsDBNull(e.Row("Baja")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

    End Sub
    Private Sub DtGridBaja_RowChanging(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        e.Row.RowError = ""

        If e.Row("Baja") > e.Row("Stock") Then
            MsgBox("Baja Mayor al Stock.")
            e.Row.RowError = "ERROR"
            GridBajas.Refresh()
        End If
        If Not e.Row("AGRanel") And TieneDecimales(e.Row("Baja")) Then
            MsgBox("Baja No debe tener decimales.")
            e.Row.RowError = "ERROR"
            GridBajas.Refresh()
        End If

    End Sub
    
End Class
