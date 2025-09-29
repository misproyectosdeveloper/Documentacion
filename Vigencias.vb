Public Class Vigencias
    Public PCodigo As Integer
    Public POrigen As Integer
    Public PActualizacionOk As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Dim DtVigencias As DataTable
    '
    Private WithEvents bs As New BindingSource
    ' 
    Dim TablaIva(0) As Double
    Dim SeniaMaxima As Double
    Private Sub Vigencias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        '     Grid.Columns("Fecha").SortMode = DataGridViewColumnSortMode.Automatic

        PActualizacionOk = False

        If PCodigo = 1 Then LabelTipoTabla.Text = "BONIFICACION COMERCIAL"
        If PCodigo = 2 Then LabelTipoTabla.Text = "BONIFICACION LOGISTICA"
        If PCodigo = 3 Then LabelTipoTabla.Text = "INGRESO BRUTO"
        If PCodigo = 4 Then LabelTipoTabla.Text = "FLETE"
        If PCodigo = 5 Then LabelTipoTabla.Text = "IMP.DEBITO/CREDITO"
        If PCodigo = 10 Then LabelTipoTabla.Text = "SEÑA ENVASES"
        If PCodigo = 11 Then LabelTipoTabla.Text = "DESCARGA ENVASES"
        If PCodigo = 12 Then LabelTipoTabla.Text = "COSTO ENVASES"
        If PCodigo = Bulto Then LabelTipoTabla.Text = "COSTO Por BULTO"
        If PCodigo = MedioBulto Then LabelTipoTabla.Text = "COSTO Por MEDIO BULTO"
        If PCodigo = Unidad Then LabelTipoTabla.Text = "COSTO Por UNIDAD"
        If PCodigo = Kilo Then LabelTipoTabla.Text = "COSTO Por KILO"

        If Not (PCodigo = 4 Or PCodigo = 11 Or PCodigo = 12 Or PCodigo = 1 Or PCodigo = 2 Or PCodigo = Bulto Or PCodigo = MedioBulto Or PCodigo = Unidad Or PCodigo = Kilo) Then
            Grid.Columns("Alicuota").Visible = False
            Grid.Columns("Valor").HeaderText = "Valor"
        End If

        ArmaTablaIva(TablaIva)

        If PCodigo = 10 Then
            SeniaMaxima = HallaSeniaMaxima()
        End If

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        '    bs.Sort = "Fecha ASC"

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        bs.EndEdit()

        If IsNothing(DtVigencias.GetChanges) Then
            MsgBox("No Hay Modificaciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If DtVigencias.HasErrors Then
            MsgBox("Debe Corregir errores antes de Realizar los Cambios. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Dim resul As Integer = GrabaVigencias(DtVigencias)

        If resul = -1 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If resul = -2 Then
            MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If resul = 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PActualizacionOk = True
        End If

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

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
    Private Function ArmaArchivos() As Boolean

        Dim Sql As String

        DtVigencias = New DataTable
        Sql = "SELECT *,CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) FROM Vigencias WHERE Codigo = " & PCodigo & " AND Origen = " & POrigen & "ORDER BY Fecha;"
        If Not Tablas.Read(Sql, Conexion, DtVigencias) Then Return False

        Grid.DataSource = bs
        bs.DataSource = DtVigencias

        AddHandler DtVigencias.RowChanging, New DataRowChangeEventHandler(AddressOf DtVigencias_RowChanging)
        AddHandler DtVigencias.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtVigencias_ColumnChanging)
        AddHandler DtVigencias.TableNewRow, New DataTableNewRowEventHandler(AddressOf Dtvigencias_NewRow)

        Return True

    End Function
    Private Function FechaRepetida(ByVal Fecha As Date) As Boolean

        For Each Row As DataRow In DtVigencias.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row.RowError = "" Then
                    If DiferenciaDias(Row("Fecha"), Fecha) = 0 Then
                        Return True
                        Exit For
                    End If
                End If
            End If
        Next

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        Grid.BeginEdit(True)

    End Sub
    Private Sub Grid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.CellBeginEdit

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            e.Cancel = True
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Valor" Then
            If Not IsDBNull(e.Value) Then
                If PCodigo = 10 Then
                    e.Value = FormatNumber(e.Value, 4)
                Else
                    e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Alicuota" Then
            If Not IsDBNull(e.Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            Calendario.ShowDialog()
            Calendario.PFecha = Format(Calendario.PFecha, "dd/MM/yyyy 00:00:00")
            Grid.Rows(e.RowIndex).Cells("Fecha").Value = Calendario.PFecha
            Calendario.Dispose()
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Fecha" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Valor" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Alicuota" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Valor" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Alicuota" Then
            If CType(sender, TextBox).Text <> "" Then
                If Not IsNumeric(CType(sender, TextBox).Text) Then
                    '        MsgBox("Importe erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    CType(sender, TextBox).Text = ""
                    CType(sender, TextBox).Focus()
                End If
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then
            MsgBox("No hay un registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Grid.Rows.Remove(Grid.CurrentRow)

    End Sub
    Private Sub Dtvigencias_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Codigo") = PCodigo
        e.Row("Origen") = POrigen
        e.Row("Alicuota") = 0

    End Sub
    Private Sub DtVigencias_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Fecha") Then
            If FechaRepetida(e.ProposedValue) Then
                MsgBox("Fecha Ya existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                If Not IsDBNull(e.Row("fecha")) Then
                    e.ProposedValue = e.Row("fecha")
                Else : e.ProposedValue = Nothing
                End If
            End If
        End If

        If e.Column.ColumnName.Equals("Valor") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If PCodigo = 10 Then
                e.ProposedValue = Trunca4(e.ProposedValue)
            Else
                e.ProposedValue = Trunca(e.ProposedValue)
            End If
        End If

        If e.Column.ColumnName.Equals("Alicuota") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

    End Sub
    Private Sub DtVigencias_RowChanging(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        If e.Row.RowState = DataRowState.Deleted Then Exit Sub

        e.Row.RowError = ""

        If IsDBNull(e.Row("Fecha")) Then
            e.Row.RowError = "Error."
            MsgBox("Falta Fecha.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If

        If IsDBNull(e.Row("Valor")) Then
            e.Row.RowError = "Error."
            MsgBox("Valor Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If

        If e.Row("Valor") <> 0 Then
            If PCodigo = 1 Or PCodigo = 2 Or PCodigo = 3 Or PCodigo = 5 Then
                If e.Row("Valor") > 100 Then
                    e.Row.RowError = "Error."
                    MsgBox("Valor Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    Grid.Refresh()
                    Exit Sub
                End If
            End If
            If PCodigo = 10 Then
                If e.Row("Valor") > SeniaMaxima Then
                    e.Row.RowError = "Error."
                    MsgBox("Seña Supera Máximo permitido.(Dar Aviso al Administrador del Sistema).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    Grid.Refresh()
                    Exit Sub
                End If
            End If
        End If

        If e.Row("Alicuota") <> 0 Then
            If e.Row("Alicuota") > 100 Then
                e.Row.RowError = "Error."
                MsgBox("Alicuota Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.Refresh()
                Exit Sub
            End If
            Dim Esta As Boolean
            For Each Item As Double In TablaIva
                If Item = e.Row("Alicuota") Then Esta = True : Exit For
            Next
            If Esta = False Then
                e.Row.RowError = "Error."
                MsgBox("Alicuota no Existe en el Sistema.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.Refresh()
                Exit Sub
            End If
        End If

    End Sub

End Class