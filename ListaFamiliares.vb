Imports ClassPassWord
Public Class ListaFamiliares
    Public PLegajo As Integer
    Public PNombre As String
    Public PApellido As String
    '                 
    Dim Dt As DataTable
    Private WithEvents bs As New BindingSource
    Dim ConexionLegajo As String
    Private Sub ListaFamiliares_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        Label2.Text = PNombre & " " & PApellido

        If PLegajo < 5000 Then
            ConexionLegajo = Conexion
        Else : ConexionLegajo = ConexionN
        End If

        LlenaCombosGrid()

        LLenaGrid()

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

        AddHandler Dt.RowChanging, New DataRowChangeEventHandler(AddressOf Dt_RowChanging)
        AddHandler Dt.TableNewRow, New DataTableNewRowEventHandler(AddressOf Dt_NewRow)

    End Sub
    Private Sub UnaTabla_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If Not IsNothing(Dt.GetChanges) Then
            If MsgBox("Los cambios no fueron Actualizados. Quiere Actualizarlos?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                ButtonAceptar_Click(Nothing, Nothing)
                If Not GModificacionOk Then e.Cancel = True : Exit Sub
            End If
        End If

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        bs.EndEdit()

        If IsNothing(Dt.GetChanges) Then Exit Sub

        If Dt.HasErrors Then
            MsgBox("Debe Corregir Errores antes de Realizar los Cambios.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GModificacionOk = False

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Dim Sql1 As String
                Sql1 = "SELECT * FROM Familiares;"
                Using DaP As New OleDb.OleDbDataAdapter(Sql1, Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(Dt.GetChanges)
                End Using
                MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                GModificacionOk = True
            End Using
        Catch ex As OleDb.OleDbException
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        Catch ex As DBConcurrencyException
            MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        Finally
        End Try

        Me.Cursor = System.Windows.Forms.Cursors.Default

        ListaFamiliares_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        If bs.Current Is Nothing Then
            MsgBox("No hay registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Dim Row As DataRowView = bs.Current

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Row("Indice") <> 0 Then
            If MsgBox("El Item se eliminara definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
        End If

        bs.RemoveCurrent()

        Me.Cursor = System.Windows.Forms.Cursors.Default

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
    Private Sub LLenaGrid()

        Dt = New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Tablas.Read("SELECT * FROM Familiares WHERE Legajo = " & PLegajo & ";", ConexionLegajo, Dt) Then End : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = Dt

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow
        Dim Dt As New DataTable

        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Row = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Dt.Rows.Add(Row)
        ' 
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Conyuge"
        Dt.Rows.Add(Row)
        ' 
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "Hijo"
        Dt.Rows.Add(Row)
        '
        Parentesco.DataSource = Dt
        Parentesco.DisplayMember = "Nombre"
        Parentesco.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        Dim Conyuges As Integer
        For Each Row As DataRow In Dt.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row("Parentesco") = 1 Then Conyuges = Conyuges + 1
            End If
        Next
        If Conyuges > 1 Then
            MsgBox("Mas de un conyuge. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        If Grid.Columns(e.ColumnIndex).Name = "Nacimiento" Then
            If IsDBNull(Grid.CurrentCell.Value) Then Grid.CurrentCell.Value = "01/01/1800"
            If Not ConsisteFecha(Grid.CurrentCell.Value) Then
                MsgBox("Fecha Incorrecta.", MsgBoxStyle.Exclamation)
                Grid.CurrentCell.Value = "01/01/1800"
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cuil" Then
            If IsDBNull(Grid.CurrentCell.Value) Then Grid.CurrentCell.Value = 0
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Cuil" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = Format(e.Value, "00-00000000-0")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Nacimiento" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = "01/01/1800" Then
                    e.Value = ""
                Else
                    e.Value = Format(e.Value, "dd/MM/yyyy")
                End If
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex
        If Grid.Columns(columna).Name = "Parentesco" Or Grid.Columns(columna).Name = "Masculino" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns(columna).Name = "Cuil" Or Grid.Columns(columna).Name = "Nacimiento" Then
            If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(columna).Name = "Nacimiento" Then
            Dim Text As String = CType(sender, TextBox).Text
            If Text.Length = 8 Then
                CType(sender, TextBox).Text = Format(Val(Text), "00/00/0000")
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Dt_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Legajo") = PLegajo
        e.Row("Indice") = 0
        e.Row("Nombre") = ""
        e.Row("Apellido") = ""
        e.Row("Masculino") = 0
        e.Row("FechaNacimiento") = "01/01/1800"
        e.Row("Parentesco") = 0
        e.Row("Cuil") = 0

    End Sub
    Private Sub Dt_RowChanging(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        e.Row.RowError = ""
        If e.Row.RowState = DataRowState.Deleted Then Exit Sub
        If e.Row.GetColumnsInError.Length <> 0 Then e.Row.RowError = "Error." : Exit Sub

        If e.Row("Nombre").ToString.Trim = "" Then
            e.Row.RowError = "Error."
            MsgBox("Falta Nombre.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If

        If e.Row("Apellido").ToString.Trim = "" Then
            e.Row.RowError = "Error."
            MsgBox("Falta Apellido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If

        If e.Row("Parentesco") = 0 Then
            e.Row.RowError = "Error."
            MsgBox("Falta Parentesco.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If

        If IsDBNull(e.Row("FechaNacimiento")) Then
            e.Row.RowError = "Error."
            MsgBox("Falta Fecha.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If

        If IsDBNull(e.Row("Cuil")) Then
            e.Row.RowError = "Error."
            MsgBox("Falta Cuil.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If

        Dim aa As New DllVarias
        If Not aa.ValidaCuiT(e.Row("Cuil").ToString) Then
            e.Row.RowError = "Error."
            MsgBox("Cuil Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If

    End Sub
End Class