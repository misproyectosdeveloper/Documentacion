Public Class SeleccionarReprocesado
    Public PLote As Integer
    Public PSecuencia As Integer
    Public PDeposito As Integer
    Public PStock As Integer
    Public POperacion As Integer
    Dim DaP As OleDb.OleDbDataAdapter
    Private WithEvents bs As New BindingSource
    Dim MiConexion As OleDb.OleDbConnection = New OleDb.OleDbConnection(Conexion)
    Dim MiConexionN As OleDb.OleDbConnection = New OleDb.OleDbConnection(ConexionN)
    Private Sub SeleccionaReprocesados_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        LlenaCombosGrid()

        BorraGrid()
        LLenaGrid()
        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

        Grid.Focus()

    End Sub
    Private Sub SeleccionaReprocesados_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Grid.EndEdit()

    End Sub
    Private Sub LLenaGrid()

        Dim SqlB As String = ""
        Dim SqlN As String = ""

        SqlB = "SELECT 1 as Operacion, * FROM Lotes WHERE LoteOrigen = " & PLote & " AND SecuenciaOrigen = " & PSecuencia & _
                          " AND DepositoOrigen = " & PDeposito & " AND Stock <> 0 AND Secuencia > 99 AND Deposito = " & PDeposito & " ORDER BY lote,secuencia;"

        SqlN = "SELECT 2 as Operacion, * FROM Lotes WHERE LoteOrigen = " & PLote & " AND SecuenciaOrigen = " & PSecuencia & _
                          " AND DepositoOrigen = " & PDeposito & " AND Stock <> 0 AND Secuencia > 99 AND Deposito = " & PDeposito & " ORDER BY lote,secuencia;"

        Try
            Dt = New DataTable
            MiConexion.Open()
            DaP = New OleDb.OleDbDataAdapter(SqlB, MiConexion)
            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
            DaP.Fill(Dt)
            If PermisoTotal Then
                MiConexionN.Open()
                DaP = New OleDb.OleDbDataAdapter(SqlN, MiConexionN)
                DaP.Fill(Dt)
            End If
            Grid.DataSource = bs
            bs.DataSource = Dt
        Catch err As OleDb.OleDbException
            MsgBox(err.Message)
        Finally
            If MiConexion.State = ConnectionState.Open Then MiConexion.Close()
            If MiConexionN.State = ConnectionState.Open Then MiConexionN.Close()
            If Not PermisoTotal Then Grid.Columns("Candado").Visible = False
        End Try
        Exit Sub

        '  Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            PLote = Val(Grid.CurrentRow.Cells("Lote").Value)
            PSecuencia = Val(Grid.CurrentRow.Cells("Secuencia").Value)
            PDeposito = Val(Grid.CurrentRow.Cells("Deposito").Value)
            PStock = Val(Grid.CurrentRow.Cells("Stock").Value)
            POperacion = Val(Grid.CurrentRow.Cells("Operacion").Value)
            Me.Close()
        End If

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
    Private Sub BorraGrid()

        Do Until Grid.Rows.Count = 0
            Grid.Rows.RemoveAt(Grid.Rows.Count - 1)
        Loop

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

        Articulo.DataSource = TodosLosArticulos()
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        Deposito.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

    End Sub
End Class