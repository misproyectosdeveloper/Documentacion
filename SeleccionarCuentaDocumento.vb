Public Class SeleccionarCuentaDocumento
    Public PListaDeCuentas As List(Of ItemCuentasAsientos)
    Public PAcepto As Boolean
    Public PSoloUnImporte As Boolean
    Public PImporteB As Decimal
    Public PImporteN As Decimal
    Private Sub SeleccionarCuentaDocumento_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        For Each Fila As ItemCuentasAsientos In PListaDeCuentas
            Grid.Rows.Add(Fila.Cuenta, Fila.ImporteB, Fila.ImporteN)
        Next

        If Not PermisoTotal Then Grid.Columns("ImporteB").Visible = False
        If PSoloUnImporte Then Grid.Columns("ImporteN").Visible = False

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        Grid.Rows.Remove(Grid.CurrentRow)

    End Sub
    Private Sub PictureLupa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupa.Click

        SeleccionarCuenta.PCentro = 0
        SeleccionarCuenta.ShowDialog()
        If SeleccionarCuenta.PCuenta <> 0 Then
            For I As Integer = 0 To Grid.Rows.Count - 1
                If Grid.Rows(I).Cells("Cuenta").Value = SeleccionarCuenta.PCuenta Then
                    MsgBox("Cuenta Ya Existe")
                    Exit Sub
                End If
            Next
            Grid.Rows.Add(SeleccionarCuenta.PCuenta)
            If PListaDeCuentas.Count = 0 And Grid.Rows.Count = 1 Then
                Grid.Rows(Grid.Rows.Count - 1).Cells("ImporteB").Value = PImporteB
                Grid.Rows(Grid.Rows.Count - 1).Cells("ImporteN").Value = PImporteN
            End If
        End If
        SeleccionarCuenta.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Grid.Rows.Count = 0 Then
            MsgBox("Falta Informar Cuentas.")
            Exit Sub
        End If

        Dim ImporteB As Decimal
        Dim ImporteN As Decimal

        For I As Integer = 0 To Grid.Rows.Count - 1
            If Grid.Rows(I).Cells("ImporteB").Value = 0 And Grid.Rows(I).Cells("ImporteN").Value = 0 Then
                MsgBox("Falta Importe en Linea " & I + 1)
                Exit Sub
            End If
            ImporteB = ImporteB + Grid.Rows(I).Cells("ImporteB").Value
            ImporteN = ImporteN + Grid.Rows(I).Cells("ImporteN").Value
        Next

        If ImporteB <> 0 Or ImporteN <> 0 Then
            If ImporteB <> PImporteB Or ImporteN <> PImporteN Then
                MsgBox("Importes No Coinciden Con Importe Factura " & PImporteB & " " & PImporteN)
                Exit Sub
            End If
        End If

        PListaDeCuentas.Clear()

        For I As Integer = 0 To Grid.Rows.Count - 1
            Dim Fila As New ItemCuentasAsientos
            Fila.Cuenta = Grid.Rows(I).Cells("Cuenta").Value
            Fila.ImporteB = Grid.Rows(I).Cells("ImporteB").Value
            Fila.ImporteN = Grid.Rows(I).Cells("ImporteN").Value
            PListaDeCuentas.Add(Fila)
        Next

        PAcepto = True

        Me.Close()

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        If Grid.Columns(e.ColumnIndex).Name = "ImporteB" Or Grid.Columns(e.ColumnIndex).Name = "ImporteN" Then
            If IsDBNull(Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0
            Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Trunca(Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Cuenta" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = Format(e.Value, "000-000000-00")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "ImporteB" Or Grid.Columns(e.ColumnIndex).Name = "ImporteN" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "ImporteB" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "ImporteN" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "ImporteB" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "ImporteN" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
End Class