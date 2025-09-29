Public Class MuestraLotesAsignados
    Public PLista As List(Of FilaAsignacion)
    Public PArticulo As Integer
    Public PIndice As Integer
    Public PEsDevolucion As Boolean
    Public PEsAumento As Boolean
    Public PMuestraPermiso As Boolean
    Public PPaseDeProyectos As ItemPaseDeProyectos
    Private Sub MuestraLotesAsignadosArticulo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            PermisoTotal = PPaseDeProyectos.PermisoTotal
            Conexion = PPaseDeProyectos.Conexion
            ConexionN = PPaseDeProyectos.ConexionN
        End If
        '----------------------------------------------------------------------------------

        Grid.AutoGenerateColumns = False
        GeneraCombosGrid()
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False
        If PMuestraPermiso Then Grid.Columns("MuestraImp").Visible = False

        If PEsDevolucion Then
            Me.Text = "Lotes Devueltos"
            Grid.Columns("Cantidad").HeaderText = "Devueltos"
        Else : Me.Text = "Lotes Asignados"
            Grid.Columns("Cantidad").HeaderText = "Asignados"
        End If
        If PEsAumento Then
            Me.Text = "Lotes Aumentados"
            Grid.Columns("Cantidad").HeaderText = "Aumento"
        End If

        LLenaGrid()

    End Sub
    Private Sub LLenaGrid()

        Grid.Rows.Clear()

        For Each Fila As FilaAsignacion In PLista
            If Fila.Indice = PIndice Then
                If Fila.Operacion = 1 Or (Fila.Operacion = 2 And PermisoTotal) Then
                    If PEsDevolucion Then
                        Grid.Rows.Add(Fila.Operacion, Fila.Lote, Fila.Secuencia, Nothing, Nothing, PArticulo, Fila.Deposito, Fila.PermisoImp, Fila.Devolucion)
                    Else : Grid.Rows.Add(Fila.Operacion, Fila.Lote, Fila.Secuencia, Nothing, Nothing, PArticulo, Fila.Deposito, Fila.PermisoImp, Fila.Asignado)
                    End If
                End If
            End If
        Next

    End Sub
    Private Sub GeneraCombosGrid()

        Deposito.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

        Articulo.DataSource = TodosLosArticulos()
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) And _
               Not IsNothing(Grid.Rows(e.RowIndex).Cells("Lote").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRecibo.ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRecibo.ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Cantidad").Value) And _
                   Not IsNothing(Grid.Rows(e.RowIndex).Cells("Cantidad").Value) Then
                If Val(Grid.Rows(e.RowIndex).Cells("Cantidad").Value) = 0 Then e.Value = ""
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub


End Class