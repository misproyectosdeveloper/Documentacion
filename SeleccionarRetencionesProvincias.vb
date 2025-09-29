Public Class SeleccionarRetencionesProvincias
    Public PRetencion As Integer
    Public PImporte As Decimal
    Public PComprobante As Integer
    Public PDtGrid As DataTable
    Public PFuncionBloqueada As Boolean
    ' 
    Dim DtGrid As DataTable
    Dim cc As ComboBox
    Private Sub SeleccionarRetencionesProvincias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        LlenaCombosGrid()

        CreaDtGrid()

        For Each Row As DataRow In PDtGrid.Rows
            If Row("Retencion") = PRetencion And Row("Comprobante") = PComprobante Then
                Dim Row1 As DataRow = DtGrid.NewRow
                Row1("Retencion") = PRetencion
                Row1("Importe") = Row("Importe")
                Row1("Provincia") = Row("Provincia")
                Row1("Comprobante") = Row("Comprobante")
                DtGrid.Rows.Add(Row1)
            End If
        Next

        Grid.DataSource = DtGrid
        Grid.EndEdit()

        CalculaTotales()

        Label2.Text = "Importe a Distribuir:  " & FormatNumber(PImporte)

        If PFuncionBloqueada Then
            Grid.ReadOnly = True
            ButtonAceptar.Enabled = False
            ButtonEliminarLineaConcepto.Enabled = False
        End If

        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Grid.EndEdit()

        CalculaTotales()

        If PImporte <> CDec(TextImporte.Text) Then
            MsgBox("Importe Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        For I As Integer = 0 To DtGrid.Rows.Count - 1
            DtGrid.Rows(I).RowError = ""
            For Z As Integer = I + 1 To DtGrid.Rows.Count - 1
                If DtGrid.Rows(I).Item("provincia") = DtGrid.Rows(Z).Item("provincia") Then
                    MsgBox("Provincia Repetida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    DtGrid.Rows(Z).RowError = "Error"
                    Grid.Refresh()
                    Exit Sub
                End If
            Next
        Next

        For Each Row As DataRow In DtGrid.Rows
            Row.RowError = ""
            If Row("Provincia") = 0 Then
                MsgBox("Falta Informar Provincia.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Row.RowError = "Error"
                Grid.Refresh()
                Exit Sub
            End If
            If Row("Importe") = 0 Then
                MsgBox("Falta Informar Importe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Row.RowError = "Error"
                Grid.Refresh()
                Exit Sub
            End If
        Next

        Grid.Refresh()

        For I As Integer = PDtGrid.Rows.Count - 1 To 0 Step -1
            If PDtGrid.Rows(I).Item("Retencion") = PRetencion And PDtGrid.Rows(I).Item("Comprobante") = PComprobante Then PDtGrid.Rows(I).Delete()
        Next
        '
        For Each Row As DataRow In DtGrid.Rows
            Dim Row1 As DataRow = PDtGrid.NewRow
            Row1("Retencion") = PRetencion
            Row1("Provincia") = Row("Provincia")
            Row1("Comprobante") = PComprobante
            Row1("Importe") = Row("Importe")
            PDtGrid.Rows.Add(Row1)
        Next

        DtGrid.Dispose()

        Me.Close()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonEliminarLineaConcepto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLineaConcepto.Click

        If Grid.Rows.Count = 1 Then Exit Sub

        Dim Row As DataRow
        Row = DtGrid.Rows.Item(Grid.CurrentRow.Index)
        Row.Delete()

        CalculaTotales()

    End Sub
    Private Sub CalculaTotales()

        Dim Total As Decimal = 0

        For Each Row As DataRow In DtGrid.Rows
            Total = Total + Row("Importe")
        Next

        TextImporte.Text = FormatNumber(Total, GDecimales)

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Retencion As New DataColumn("Retencion")
        Retencion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Retencion)

        Dim Provincia As New DataColumn("Provincia")
        Provincia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Provincia)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Comprobante)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Importe)

    End Sub
    Private Sub LlenaCombosGrid()

        Provincia.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 31 ORDER BY Nombre;")
        Dim Row As DataRow = Provincia.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Provincia.DataSource.Rows.Add(Row)
        Provincia.DisplayMember = "Nombre"
        Provincia.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                 --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        'para manejo del autocoplete de conceptos.
        If Grid.Columns(e.ColumnIndex).Name = "Provincia" Then
            If Not cc Is Nothing Then
                cc.SelectedIndex = cc.FindStringExact(cc.Text)
                If cc.SelectedIndex < 0 Then cc.SelectedValue = 0
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cc = e.Control
            cc.DropDownStyle = ComboBoxStyle.DropDown
            cc.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cc.AutoCompleteSource = AutoCompleteSource.ListItems
            Exit Sub
        End If

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey1_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged1_TextChanged

    End Sub
    Private Sub ValidaKey1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe1" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe1" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        If Grid.Columns(e.ColumnIndex).Name = "Importe1" Then
            CalculaTotales()
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Importe1" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#.##")
                Else : e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Retencion") = PRetencion
        e.Row("Provincia") = 0
        e.Row("Importe") = 0

    End Sub

End Class