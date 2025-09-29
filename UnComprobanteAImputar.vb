Public Class UNComprobanteAImputar
    'Gestiona los movimientos del archivo PdtGridCompro.
    Public PDtGridCompro As DataTable
    Public PTipo As Integer
    Public PAbierto As Boolean
    Public PTotalConceptos As Decimal
    Public PTipoIva As Integer
    Public PMoneda As Integer
    Public PCambio As Decimal
    Public PSinImporte As Decimal
    Public PImputado As Decimal
    '
    Dim TotalConceptos As Decimal
    Dim TotalFacturas As Decimal
    Dim DtGridCompro As DataTable
    Private Sub UNComprobanteAImputar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GridCompro.AutoGenerateColumns = False
        GridCompro.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        DtGridCompro = PDtGridCompro.Copy

        GridCompro.DataSource = DtGridCompro

        'maca en columna seleccion filas con Imp.Asignado <> 0.
        For Each Row As DataGridViewRow In GridCompro.Rows
            If Row.Cells("Asignado").Value <> 0 Then
                Row.Cells("Seleccion").Value = True
            End If
        Next
        GridCompro.RefreshEdit()  'Ponerlo para que marque la priner fila(Sino lo borra).

        AddHandler DtGridCompro.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGridCompro_ColumnChanging)

        LlenaCombosGrid()

        Label1.Text = "Total a Imputar  " & FormatNumber(PTotalConceptos, GDecimales)

        CalculaTotales()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida Then Exit Sub

        PDtGridCompro = DtGridCompro.Copy
        PImputado = CDec(TextTotalFacturas.Text)

        Me.Close()

    End Sub
    Private Sub CalculaTotales()

        TotalConceptos = PTotalConceptos

        TotalFacturas = 0
        For Each Row As DataRow In DtGridCompro.Rows
            If Row.RowState <> DataRowState.Deleted Then
                TotalFacturas = Trunca(TotalFacturas + Row("Asignado"))
            End If
        Next

        TextTotalFacturas.Text = FormatNumber(TotalFacturas, GDecimales)
        TextSaldo.Text = FormatNumber(TotalConceptos - TotalFacturas, GDecimales)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Tipo.DataSource = DtTiposComprobantes(PAbierto)
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Codigo"

        TipoVisible.DataSource = DtTiposComprobantes(PAbierto)
        Row = TipoVisible.DataSource.NewRow()
        Row("Codigo") = 44
        Row("Nombre") = "Ticket Fiscal"
        TipoVisible.DataSource.Rows.Add(Row)
        TipoVisible.DisplayMember = "Nombre"
        TipoVisible.ValueMember = "Codigo"

        Moneda.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 27 order By Nombre;")
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 1
        Row("Nombre") = "PESOS"
        Moneda.DataSource.Rows.Add(Row)
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Moneda.DataSource.Rows.Add(Row)
        Moneda.DisplayMember = "Nombre"
        Moneda.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        If PSinImporte Then Return True

        If TotalConceptos - TotalFacturas < 0 Then
            MsgBox("Importes Imputados Supera Importe a Distribuir.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridCompro_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellContentClick

        If GridCompro.Columns(e.ColumnIndex).Name = "Seleccion" Then
            Dim chkCell As DataGridViewCheckBoxCell = Me.GridCompro.Rows(e.RowIndex).Cells("Seleccion")
            chkCell.Value = Not chkCell.Value
            If chkCell.Value Then
                If PSinImporte Then
                    GridCompro.Rows(e.RowIndex).Cells("Asignado").Value = GridCompro.Rows(e.RowIndex).Cells("Saldo").Value
                    CalculaTotales()
                    Exit Sub
                End If
                Dim Importe As Decimal = GridCompro.Rows(e.RowIndex).Cells("Saldo").Value + GridCompro.Rows(e.RowIndex).Cells("Asignado").Value
                Dim TotalLibre As Decimal = PTotalConceptos - TotalFacturas + GridCompro.Rows(e.RowIndex).Cells("Asignado").Value
                If TotalLibre >= Importe Then
                    GridCompro.Rows(e.RowIndex).Cells("Asignado").Value = Importe
                Else
                    GridCompro.Rows(e.RowIndex).Cells("Asignado").Value = TotalLibre
                End If
                CalculaTotales()
            Else
                GridCompro.Rows(e.RowIndex).Cells("Asignado").Value = 0
                CalculaTotales()
            End If
            GridCompro.Refresh()
        End If

    End Sub
    Private Sub GridCompro_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEnter

        If Not GridCompro.Columns(e.ColumnIndex).ReadOnly Then
            GridCompro.BeginEdit(True)
        End If

    End Sub
    Private Sub GridCompro_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles GridCompro.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If GridCompro.Columns(e.ColumnIndex).Name = "Recibo" Then
            e.Value = NumeroEditado(e.Value)
            If PermisoTotal Then
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "FechaCompro" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Asignado" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "ImporteCompro" Or GridCompro.Columns(e.ColumnIndex).Name = "Saldo" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

    End Sub
    Private Sub GridCompro_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles GridCompro.DataError
        Exit Sub
    End Sub
    Private Sub GridCompro_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles GridCompro.EditingControlShowing

        Dim columna As Integer = GridCompro.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPressCompro
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChangedCompro

    End Sub
    Private Sub ValidaKey_KeyPressCompro(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Asignado" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChangedCompro(ByVal sender As Object, ByVal e As System.EventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Asignado" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

    End Sub
    Private Sub GridCompro_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEndEdit

        If GridCompro.Columns(e.ColumnIndex).Name = "Asignado" Then
            If IsDBNull(GridCompro.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then GridCompro.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0
            CalculaTotales()
        End If

    End Sub
    Private Sub DtGridCompro_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        Dim SaldoAnt As Decimal = 0

        If (e.Column.ColumnName.Equals("Asignado")) Then
            If IsDBNull(e.Row("Asignado")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
            If TotalFacturas - e.Row("Asignado") + CDec(e.ProposedValue) > PTotalConceptos Then
                If Not PSinImporte Then
                    MsgBox("Importe Documentos Supera Total del Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    e.ProposedValue = e.Row("Asignado")
                    Exit Sub
                End If
            End If
            '
            SaldoAnt = e.Row("Saldo")
            '
            e.Row("Saldo") = e.Row("Saldo") + e.Row("Asignado") - CDec(e.ProposedValue)
            '
            If e.Row("Saldo") < 0 Then
                MsgBox("Imputación Supera el Saldo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Asignado")
                e.Row("Saldo") = SaldoAnt
                Exit Sub
            End If
        End If

        CalculaTotales()

    End Sub
    Private Sub ButtonMarcarTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMarcarTodos.Click

        'Prugunta si ya hay imputados. Si hay no hace nada.
        For Each Row As DataGridViewRow In GridCompro.Rows
            If Row.Cells("Asignado").Value <> 0 Then
                MsgBox("Debe des-imputar todos.") : Exit Sub
            End If
        Next
        'Marca imputacion. 
        For Each Row As DataGridViewRow In GridCompro.Rows
            If CDbl(TextSaldo.Text) <> 0 Then   'llama GridCompro_CellContentClick hasta agotar saldo.
                Dim arg = New DataGridViewCellEventArgs(1, Row.Index)
                GridCompro_CellContentClick(GridCompro, arg)
            End If
        Next

        DtGridCompro.AcceptChanges()  'ponerlo para que tome todas las filas.

    End Sub
    Private Sub ButtonDesmarcarTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDesmarcarTodos.Click

        'Recorre el grid y solo los que tienen imp-asignado <> 0 deriba a GridCompro_CellContentClick(GridCompro, arg). 
        For Each Row As DataGridViewRow In GridCompro.Rows
            If Row.Cells("Asignado").Value <> 0 Then
                Dim arg = New DataGridViewCellEventArgs(1, Row.Index)
                GridCompro_CellContentClick(GridCompro, arg)
            End If
        Next

        DtGridCompro.AcceptChanges()  'ponerlo para que tome todas las filas.

    End Sub
End Class