Public Class ListaDescartes
    Private WithEvents bs As New BindingSource
    '
    Dim Dt As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaDescartes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50
        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        GeneraCombosGrid()

        If PermisoTotal Then
            Grid.Columns("Candado").Visible = True
        Else : Grid.Columns("Candado").Visible = False
        End If

    End Sub
    Private Sub ListaDescartes_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub ListaDescartes_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 as Operacion,* FROM DescarteCabeza "
        SqlN = "SELECT 2 as Operacion,* FROM DescarteCabeza "

        Dim SqlComprobante As String = ""
        If TextComprobante.Text <> "" Then
            SqlComprobante = "WHERE Clave = " & TextComprobante.Text & " "
        Else : SqlComprobante = "WHERE Clave LIKE '%' "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "AND Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlEstado As String
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND Estado = " & ComboEstado.SelectedValue & " "
        End If

        SqlB = SqlB & SqlComprobante & SqlDeposito & SqlFecha & SqlEstado
        SqlN = SqlN & SqlComprobante & SqlDeposito & SqlFecha & SqlEstado

        LLenaGrid()

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        Grid.Focus()

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub Textcomprobante_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComprobante.KeyPress

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
        If PermisoTotal Then
            Tablas.Read(SqlN, ConexionN, Dt)
        End If

        Dim View As New DataView

        View = Dt.DefaultView
        View.Sort = "Clave"

        Grid.DataSource = bs
        bs.DataSource = View

    End Sub
    Private Sub GeneraCombosGrid()

        Deposito.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

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
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Estado" Then
            If Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value <> 3 Then e.Value = ""
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Rows.Count = 0 Then Exit Sub

        UnDescarte.PComprobante = Grid.CurrentRow.Cells("Comprobante").Value
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            UnDescarte.PAbierto = True
        Else : UnDescarte.PAbierto = False
        End If
        UnDescarte.ShowDialog()
        UnDescarte.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
  
End Class