Public Class Prueba

    Private Sub Prueba_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Cbo As DataGridViewComboBoxColumn

        Cbo = New DataGridViewComboBoxColumn
        Cbo.HeaderText = "Calibre"
        Cbo.Name = "Calibre"
        Cbo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Cbo.MinimumWidth = 90
        Cbo.Width = 90
        Cbo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Calibres;")
        Cbo.DisplayMember = "Nombre"
        Cbo.DataPropertyName = "Calibre"
        Cbo.ValueMember = "Clave"
        DataGridView1.Columns.Insert(7, Cbo)

        DataGridView1.Rows.Add(1, 2, 33, 8, 8, 9, 0, 1)
        DataGridView1.Rows.Add(1, 999, 43, 8, 9, 55, 33, 2)
        DataGridView1.Rows.Add(1, 42, 999, 60, 77, 7, 5, 3)
        DataGridView1.Rows.Add(1, 42, 999, 60, 77, 7, 5, 4)
        DataGridView1.Rows.Add(1, 42, 999, 160, 77, 7, 5, 4)
        DataGridView1.Rows.Add(1, 42, 999, 160, 77, 7, 5, 1)
    End Sub
    Private Sub DataGridView1_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting

        If (e.ColumnIndex = 3 Or e.ColumnIndex = 0) AndAlso e.RowIndex <> -1 Then

            Using gridBrush As Brush = New SolidBrush(Me.DataGridView1.GridColor), backColorBrush As Brush = New SolidBrush(e.CellStyle.BackColor)

                Using gridLinePen As Pen = New Pen(gridBrush)
                    ' Clear cell   
                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds)

                    ' Draw line (bottom border and right border of current cell)   
                    'If next row cell has different content, only draw bottom border line of current cell   
                    If e.RowIndex < DataGridView1.Rows.Count - 2 AndAlso DataGridView1.Rows(e.RowIndex + 1).Cells(e.ColumnIndex).Value.ToString() <> e.Value.ToString() Then
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
                    End If

                    ' Draw right border line of current cell   
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom)

                    ' draw/fill content in current cell, and fill only one cell of multiple same cells   
                    If Not e.Value Is Nothing Then
                        If e.RowIndex > 0 AndAlso DataGridView1.Rows(e.RowIndex - 1).Cells(e.ColumnIndex).Value.ToString() = e.Value.ToString() Then
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