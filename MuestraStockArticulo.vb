Public Class MuestraStockArticulo
    Public PArticulo As Integer
    Public PDeposito As Integer
    Private WithEvents bs As New BindingSource
    Private Sub MuestraStockArticulo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        LlenaCombo(ComboArticulo, "", "Articulos")
        ComboArticulo.SelectedValue = PArticulo
        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = PDeposito
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaCombosGrid()

        BorraGrid(Grid)
        LLenaGrid()

    End Sub
    Private Sub LLenaGrid()

        Dim SqlB As String = ""
        Dim SqlN As String = ""

        SqlB = "SELECT 1 as Operacion,0 as Asignado,0 as OtrasAsignaciones, * FROM Lotes WHERE Deposito = " & PDeposito & " AND Articulo = " & PArticulo & _
                                                " AND Stock <> 0 ORDER BY lote,secuencia;"
        SqlN = "SELECT 2 as Operacion,0 as Asignado,0 as OtrasAsignaciones, * FROM Lotes WHERE Deposito = " & PDeposito & " AND Articulo = " & PArticulo & _
                                                " AND Stock <> 0 ORDER BY lote,secuencia;"

        Dim Dt As New DataTable
        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            Tablas.Read(SqlN, ConexionN, Dt)
        End If

        Grid.DataSource = bs
        bs.DataSource = Dt

        Dim Total As Integer = 0
        For Each Row1 As DataRow In Dt.Rows
            Total = Total + Row1("Stock")
        Next

        bs.AddNew()
        Dim Row As DataRowView
        Row = bs.Current
        Row("Stock") = Total

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) And _
               Not IsNothing(Grid.Rows(e.RowIndex).Cells("Lote").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
                    Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                Else : Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Calibre.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Calibres;")
        Row = Calibre.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Calibre.DataSource.Rows.Add(Row)
        Calibre.DisplayMember = "Nombre"
        Calibre.ValueMember = "Clave"

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

    End Sub
   
End Class