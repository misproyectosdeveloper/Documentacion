Public Class Lotes
    Dim DaP As OleDb.OleDbDataAdapter
    Private WithEvents bs As New BindingSource
    Dim MiConexion As OleDb.OleDbConnection = New OleDb.OleDbConnection(Conexion)
    Private Sub Lotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        LLenaGrid()

    End Sub
    Private Sub LLenaGrid()

        '   Dim SelectStr As String = "Especies.Nombre as NombreEspecie,Variedad,Marca,Categoria,Envase,Articulos.Nombre AS NombreArticulo,Lote,Secuencia"
        '        Dim FromStr As String = "Lotes INNER JOIN (Articulos INNER JOIN (Especies INNER JOIN Variedades"
        '        Dim FromStr As String = "Lotes INNER JOIN Articulos INNER JOIN Especies INNER JOIN Variedades"
        Dim FromStr As String = "Lotes INNER JOIN Articulos INNER JOIN Especies INNER JOIN Variedades"
        '    Dim OnStr As String = "ON Variedades.Clave = Articulos.Variedad) ON Especies.Clave = Articulos.Especie) ON Articulos.Clave = Lotes.Articulo;"
        Dim OnStr As String = "ON Variedades.Clave = Articulos.Variedad ON Especies.Clave = Articulos.Especie ON Articulos.Clave = Lotes.Articulo;"
        '   Dim Sql As String = "SELECT " & SelectStr & " FROM " & FromStr & " " & OnStr

        Dim Sql As String = "SELECT Especies.Nombre as NombreEspecie,Lote,Secuencia " & _
                                  "FROM Lotes , (SELECT Articulos.Clave as clave,Especies.Nombre FROM Articulos,Especies WHERE " & _
                                  "Articulos.Especie = Especies.Clave) WHERE Lotes.Articulo = Clave"

        Dim SelectStr As String = "SELECT Especies.Nombre AS NombreEspecie,Variedades.Nombre as NombreVariedad,Marcas.Nombre AS NombreMarca, " & _
                                  "Categorias.Nombre AS NombreCategoria, Envases.Nombre as NombreEnvase,Articulos.Nombre AS NombreArticulo, Lote , Secuencia"
        '        FromStr = " FROM Lotes INNER JOIN(Articulos INNER JOIN Especies ON Especies.Clave = Articulos.Especie) ON Lotes.Articulo=Articulos.Clave"
        Dim ArchJoin As String = "Lotes INNER JOIN articulos ON Lotes.Articulo=Articulos.Clave"
        Dim WhereStr As String = "articulos.especie = especies.clave and articulos.variedad = Variedades.Clave AND Articulos.Marca = Marcas.Clave " & _
                              "AND Articulos.Categoria = Categorias.Clave AND Articulos.Envase = Envases.Clave"
        Sql = SelectStr & " FROM (" & ArchJoin & "),Especies,Variedades,Marcas,Categorias,Envases WHERE " & WhereStr & ";"

        'http://www.wikilearning.com/tutorial/inner_join_o_equi_join_consultas_mysql_a_multiples_tablas_relacionadas-inner_join_o_equi_join_consultas_mysql_a_multiples_tablas_relacionadas/18001-1
        Try
            Dt.Clear()
            MiConexion.Open()
            DaP = New OleDb.OleDbDataAdapter(Sql, MiConexion)
            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
            DaP.FillSchema(Dt, SchemaType.Source)
            DaP.Fill(Dt)
            Grid.DataSource = bs
            bs.DataSource = Dt
            MiConexion.Close()
        Catch err As OleDb.OleDbException
            MsgBox(err.Message)
        Finally
            If MiConexion.State = ConnectionState.Open Then MiConexion.Close()
        End Try

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteSecuencia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "00")
            End If
        End If
    End Sub
    Private Sub Grid_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles Grid.CellPainting

        If (e.ColumnIndex >= 2 And e.ColumnIndex <= 7) AndAlso e.RowIndex <> -1 Then

            Using gridBrush As Brush = New SolidBrush(Me.Grid.GridColor), backColorBrush As Brush = New SolidBrush(e.CellStyle.BackColor)

                Using gridLinePen As Pen = New Pen(gridBrush)
                    ' Clear cell   
                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds)

                    ' Draw line (bottom border and right border of current cell)   
                    'If next row cell has different content, only draw bottom border line of current cell   
                    If e.RowIndex < Grid.Rows.Count - 2 AndAlso Grid.Rows(e.RowIndex + 1).Cells(e.ColumnIndex).Value.ToString() <> e.Value.ToString() Then
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
                    End If
                    ' Draw right border line of current cell   
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom)
                    ' draw/fill content in current cell, and fill only one cell of multiple same cells   
                    If Not e.Value Is Nothing Then
                        If e.RowIndex > 0 AndAlso Grid.Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value.ToString() = e.Value.ToString() Then
                        Else
                            e.Graphics.DrawString(CType(e.Value, String), e.CellStyle.Font, Brushes.Black, e.CellBounds.X + 2, e.CellBounds.Y + 5, StringFormat.GenericDefault)
                        End If
                    End If
                    e.Handled = True
                End Using
            End Using
        End If
    End Sub
End Class