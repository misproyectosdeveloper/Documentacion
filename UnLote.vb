Public Class UnLote
    Public PLote As Integer
    Public PSecuencia As Integer
    Public PAbierto As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Private Dt As DataTable
    '
    Dim ConexionLote As String
    Private Sub UnLote_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If PAbierto Then
            ConexionLote = Conexion
        Else : ConexionLote = ConexionN
        End If

        Grid.AutoGenerateColumns = False
        GeneraCombosGrid()

        TextLote.Text = PLote & "/" & Format(PSecuencia, "000")

        LlenaCombo(ComboProveedor, "", "Proveedores")
        LlenaCombo(ComboArticulo, "", "Articulos")

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

        MuestraDatos()

    End Sub
    Private Sub MuestraDatos()

        Dt = New DataTable
        Dim Sql As String = "SELECT * FROM Lotes WHERE LoteOrigen = " & PLote & " AND SecuenciaOrigen = " & PSecuencia & " AND DepositoOrigen = Deposito ORDER BY Secuencia;"
        If Not Tablas.Read(Sql, ConexionLote, Dt) Then Me.Close() : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = Dt

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Secuencia").Value > 99 Then Row.Cells("Cantidad").ReadOnly = True
        Next

        ComboProveedor.SelectedValue = Dt.Rows(0).Item("Proveedor")
        ComboArticulo.SelectedValue = Dt.Rows(0).Item("Articulo")
        TextCantidad.Text = FormatNumber(Dt.Rows(0).Item("Cantidad"))
        DateTime1.Value = Format(Dt.Rows(0).Item("Fecha"), "dd/MM/yyyy")
        ComboTipoOperacion.SelectedValue = Dt.Rows(0).Item("TipoOperacion")

        If Dt.Rows(0).Item("Liquidado") = 0 Then
            Label4.Visible = False
        Else : Label4.Visible = True
        End If

    End Sub
    Private Sub FormatNumerico(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Not IsDBNull(Numero.Value) Then Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub GeneraCombosGrid()

        Deposito.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

        Articulo.DataSource = TodosLosArticulos()
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        Return True

    End Function
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteySecuencia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) And _
              Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Secuencia").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If

    End Sub

  
   
   
End Class