Public Class ListaTransferenciasInsumo
    Private WithEvents bs As New BindingSource
    '
    Dim Dt As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaTransferenciasInsumo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50
        Grid.AutoGenerateColumns = False

        LlenaComboTablas(ComboDeposito, 20)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        GeneraCombosGrid()

    End Sub
    Private Sub ListaTransferenciasInsumo_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub ListaTransferenciasInsumo_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 as Operacion,CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) AS Fecha,* FROM TransInsumoCabeza "

        Dim SqlComprobante As String = ""
        If Val(TextComprobante.Text) <> 0 Then
            SqlComprobante = "WHERE Transferencia = " & Val(TextComprobante.Text) & " "
        Else : SqlComprobante = "WHERE Transferencia LIKE '%' "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND Origen = " & ComboDeposito.SelectedValue & " "
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "AND Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        SqlB = SqlB & SqlComprobante & SqlDeposito & SqlFecha

        LLenaGrid()

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

    End Sub
    Private Sub Textcomprobante_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

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
        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Transferencia"

        Grid.DataSource = bs
        bs.DataSource = View

    End Sub
    Private Sub GeneraCombosGrid()

        Origen.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = " & 20 & " ORDER BY Nombre;")
        Origen.DisplayMember = "Nombre"
        Origen.ValueMember = "Clave"

        Destino.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = " & 20 & " ORDER BY Nombre;")
        Destino.DisplayMember = "Nombre"
        Destino.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            If Not IsNothing(e.Value) Then
                e.Value = Format(e.Value, "00000000")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Estado" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> "Anulado" Then e.Value = ""
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Rows.Count = 0 Then Exit Sub

        UnaTransferenciaInsumo.PTrans = Grid.CurrentRow.Cells("Comprobante").Value
        UnaTransferenciaInsumo.ShowDialog()
        If UnaTransferenciaInsumo.PActualizacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        UnaTransferenciaInsumo.Dispose()

    End Sub
End Class