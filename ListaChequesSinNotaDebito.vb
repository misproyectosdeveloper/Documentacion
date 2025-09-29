Public Class ListaChequesSinNotaDebito
    Private WithEvents bs As New BindingSource
    Private Sub ListaChequesSinNotaDebito_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50
        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        PreparaArchivos()

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

    End Sub
    Private Sub PreparaArchivos()

        Dim SqlB As String = ""
        Dim SqlN As String = ""

        SqlB = "SELECT 1 AS Operacion,ClaveCheque,Numero FROM Cheques WHERE MedioPago = 3 AND TieneDebito = 0 AND Estado = 4;"
        SqlN = "SELECT 2 AS Operacion,ClaveCheque,Numero FROM Cheques WHERE MedioPago = 3 AND TieneDebito = 0 AND Estado = 4;"

        Dim Dt As New DataTable
        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Grid.DataSource = bs
        bs.DataSource = Dt

        For Each Row As DataRow In Dt.Rows
            '          Grid.Rows.Add(Row("Operacion"), Nothing, Row("ClaveCheque"), Row("Numero"))
        Next

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

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
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Clave" Then
            If PermisoTotal Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

    End Sub
End Class