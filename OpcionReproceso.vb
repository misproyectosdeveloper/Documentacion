Public Class OpcionReproceso
    Public PAbierto As Boolean
    Public PLote As Integer
    Public PSecuencia As Integer
    Public PDeposito As Integer
    Public PArticulo As Integer
    Public PStock As Double
    Public PKilosXUnidad As Double
    Public PBaja As Double
    Public PEsAGranel As Double
    '
    Dim DtArticulosConStock As DataTable
    ' 
    Dim ArticuloAnt As Integer
    Dim DepositoAnt As Integer
    Private Sub OpcionIngreso_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        GeneraCombosGrid()

        PLote = 0
        PSecuencia = 0
        PDeposito = 0
        PStock = 0
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

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value Then
                PLote = Row.Cells("Lote").Value
                PSecuencia = Row.Cells("Secuencia").Value
                PKilosXUnidad = Row.Cells("KilosXUnidad").Value
                PStock = Row.Cells("Stock").Value
                If Row.Cells("Operacion").Value = 1 Then
                    PAbierto = True
                Else : PAbierto = False
                End If
                PDeposito = ComboDeposito.SelectedValue
                PArticulo = ComboArticulo.SelectedValue
                PBaja = CDbl(TextBaja.Text)
            End If
        Next

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
        If TextBaja.Text.Trim = "" Then
            MsgBox("Falta Cantidad de Baja.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            TextBaja.Focus()
            Exit Sub
        End If
        If CDbl(TextBaja.Text) = 0 Then
            MsgBox("Cantidad de Baja debe ser distinto de cero.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            TextBaja.Focus()
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
        TextBaja.Text = 0

        If EsArticuloAGranel(ComboArticulo.SelectedValue) Then
            Label7.Text = "Kg. de Baja"
            PEsAGranel = True
        Else : Label7.Text = "Unidades de Baja"
            PEsAGranel = False
        End If

        Grid.DataSource = Nothing

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If ComboDeposito.SelectedValue = DepositoAnt Then Exit Sub

        DepositoAnt = ComboDeposito.SelectedValue

        Grid.DataSource = Nothing

        ArmaArticulosConStock()

        TextBaja.Text = 0

    End Sub
    Private Sub TextBaja_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBaja.KeyPress

        If PEsAGranel Then
            EsNumerico(e.KeyChar, TextBaja.Text, 2)
        Else : EsNumerico(e.KeyChar, TextBaja.Text, 0)
        End If

    End Sub
    Private Sub TextBaja_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBaja.Validating

        If TextBaja.Text = "" Then TextBaja.Text = "0"

    End Sub
    Private Sub LLenaGrid()

        Dim Dt As New DataTable
        Dim Baja As Double = CDbl(TextBaja.Text)

        CambiarPuntoDecimal(".")

        Dim SqlB As String = "SELECT 1 as Operacion,Lote,Secuencia,Proveedor,Stock,KilosXUnidad,Fecha,Articulo FROM Lotes WHERE Stock >= " & Baja & " AND Deposito = " & ComboDeposito.SelectedValue & " AND Articulo = " & ComboArticulo.SelectedValue & ";"
        Dim SqlN As String = "SELECT 2 as Operacion,Lote,Secuencia,Proveedor,Stock,KilosXUnidad,Fecha,Articulo FROM Lotes WHERE Stock >= " & Baja & " AND Deposito = " & ComboDeposito.SelectedValue & " AND Articulo = " & ComboArticulo.SelectedValue & ";"

        CambiarPuntoDecimal(",")

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Lote,Secuencia"

        Grid.DataSource = View

        Dt.Dispose()

    End Sub
    Private Sub GeneraCombosGrid()

        Proveedor.DataSource = ProveedoresDeFrutasYCosteo()
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

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

        Dt = Tablas.Leer("SELECT A.Clave,A.Nombre,A.Envase,E.Kilos FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave WHERE A.Clave = " & Articulo & ";")

        BuscaVigencia(10, Date.Now, Senia, Dt.Rows(0).Item("Envase"))
        HallaNombreArticulo = Dt.Rows(0).Item("Nombre") & " (S:" & FormatNumber(Senia, GDecimales) & " Kg:" & Dt.Rows(0).Item("Kilos") & ")"

        Dt.Dispose()

    End Function
    Private Function Valida() As Boolean

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
        If CDbl(TextBaja.Text) = 0 Then
            MsgBox("Falta Unidades de Baja.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Grid.Rows.Count = 0 Then
            MsgBox("No Existe Lotes en el Deposito.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        Dim Con As Integer = 0
        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value = True Then
                Con = Con + 1
                If CDbl(TextBaja.Text) > Row.Cells("Stock").Value Then
                    MsgBox("Unidades de Baja Mayor que Stock del Lote.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
        Next
        If Con > 1 Then
            MsgBox("Solo debe elegir un Lote.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If
        If Con = 0 Then
            MsgBox("Debe elegir un Lote.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Not IsNothing(Grid.Rows(e.RowIndex).Cells("Lote").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

    End Sub

   
   
End Class