Public Class ListaCentrosDeCosto
    Public PBloqueaFunciones As Boolean
    '
    Private WithEvents bs As New BindingSource
    ' 
    Dim Dt As DataTable
    Private Sub ListaCentrosDeCosto_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'poner en propiedades del formulario Multiselect=true para copiar a excel.

        If Not PermisoEscritura(11) Then PBloqueaFunciones = True

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Lupa").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")

        ArmaArchivos()

        ' bs.Sort = "Fecha ASC"

    End Sub
    Private Sub ListaCentrosDeCosto_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If Not IsNothing(Dt.GetChanges) And Not Dt.HasErrors Then
            If MsgBox("Los cambios no fueron Actualizados. Quiere Actualizarlos?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                ButtonAceptar_Click(Nothing, Nothing)
            End If
        End If

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        bs.EndEdit()

        If IsNothing(Dt.GetChanges) Then
            MsgBox("No Hay Modificaciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If Dt.HasErrors Then
            MsgBox("Debe Corregir errores antes de Realizar los Cambios. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Resul As Integer = ActualizaArchivo()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If Resul <= 0 Then
            MsgBox("Error al Grabar Cambios. Algunos Cambios no se Realizaron.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        ArmaArchivos()

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
    Private Sub ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM CentrosDeCosto ORDER BY Centro;", Conexion, Dt) Then Me.Close() : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = Dt

        For Each Row As DataGridViewRow In Grid.Rows
            If Not IsNothing(Row.Cells("Centro").Value) Then
                Row.Cells("Centro").ReadOnly = True
            End If
        Next
        Grid.EndEdit()

        AddHandler Dt.RowChanging, New DataRowChangeEventHandler(AddressOf Dt_RowChanging)
        AddHandler Dt.TableNewRow, New DataTableNewRowEventHandler(AddressOf Dt_NewRow)
        AddHandler Dt.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dt_ColumnChanging)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function ActualizaArchivo() As Integer

        Using MiConexion As New OleDb.OleDbConnection(Conexion)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Try
                    If Not IsNothing(Dt.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM CentrosDeCosto;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.Update(Dt.GetChanges)
                        End Using
                    End If
                    Return 1000
                Catch ex As OleDb.OleDbException
                    If ex.ErrorCode = GAltaExiste Then
                        Return -1
                    Else
                        Return -2
                    End If
                Catch ex As DBConcurrencyException
                    Return 0
                Finally
                End Try
            Catch ex As Exception
                Return -2
            End Try
        End Using

    End Function
    Function Usado(ByVal Centro As Integer, ByRef ErrorW As String) As Boolean

        ErrorW = ""

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                ErrorW = "Usado en Plan de Cuentas."
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Centro) FROM PlanDeCuentas WHERE Centro = " & Centro & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                ErrorW = "Usado en Negocio."
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Centro) FROM Proveedores WHERE Centro = " & Centro & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                ErrorW = "Tipo de Operaciones."
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Centro) FROM Miselaneas WHERE Centro = " & Centro & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        Grid.BeginEdit(True)

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Centro" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else
                    e.Value = Format(e.Value, "000")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            e.Value = Nothing
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            If Grid.Rows(e.RowIndex).Cells("Centro").ReadOnly = False Then
                MsgBox("Debe dar de Alta Al centro.", MsgBoxStyle.Information)
                Exit Sub
            End If
            ListaCuentasDelCentro.PCentro = Grid.Rows(e.RowIndex).Cells("Centro").Value
            ListaCuentasDelCentro.PNombreCentro = Grid.Rows(e.RowIndex).Cells("Nombre").Value
            ListaCuentasDelCentro.ShowDialog()
            ListaCuentasDelCentro.Dispose()
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name <> "Centro" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Centro" Then
            If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Dt.Rows.Count = 0 Then
            MsgBox("No hay un registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Row As DataRowView = bs.Current
        Dim ErrorW As String = ""

        If Row.Row.RowState <> DataRowState.Added Then
            If Usado(Row("Centro"), ErrorW) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox(ErrorW & " Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If MsgBox("Centro " & Row("Centro") & " se eliminara definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
        End If

        Grid.Rows.Remove(Grid.CurrentRow)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub Dt_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Centro") = 0
        e.Row("Nombre") = ""
        e.Row("Comentario") = ""

    End Sub
    Private Sub Dt_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        e.Row.SetColumnError(e.Column, "")

        If e.Column.ColumnName.Equals("Centro") Then
            If Not IsDBNull(e.Row("Centro")) Then
                If IsNothing(e.ProposedValue) Then e.ProposedValue = 0
                If e.Row("Centro") <> e.ProposedValue And e.ProposedValue <> 0 Then
                    Dim RowsBusqueda() As DataRow
                    RowsBusqueda = Dt.Select("Centro = " & e.ProposedValue)
                    If RowsBusqueda.Length <> 0 Then
                        MsgBox("Centro ya Existe.")
                        e.ProposedValue = e.Row("Centro")
                    End If
                End If
            End If
        End If

        If e.Column.ColumnName.Equals("Nombre") Then
            If Not IsDBNull(e.Row("Nombre")) Then
                If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
                If e.Row("Nombre") <> e.ProposedValue And e.ProposedValue <> "" Then
                    Dim RowsBusqueda() As DataRow
                    RowsBusqueda = Dt.Select("Nombre = '" & e.ProposedValue & "'")
                    If RowsBusqueda.Length <> 0 Then
                        MsgBox("Nombre del Centro ya Existe.")
                        e.ProposedValue = e.Row("Nombre")
                    End If
                End If
            End If
        End If

        If e.Column.ColumnName.Equals("Comentario") Then
            If Not IsDBNull(e.Row("Comentario")) Then
                If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
            End If
        End If

    End Sub
    Private Sub Dt_RowChanging(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        If e.Row.RowState = DataRowState.Deleted Then Exit Sub

        If e.Row("Centro") = 0 Then
            e.Row.SetColumnError("Centro", "Error")
        End If

        If e.Row("Nombre") = "" Then
            e.Row.SetColumnError("Nombre", "Error")
        End If

        Grid.Refresh()

    End Sub

End Class