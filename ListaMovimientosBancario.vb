Public Class ListaMovimientosBancario
    Private WithEvents bs As New BindingSource
    '
    Dim Dt As DataTable
    Dim DtFormasPago As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaMovimientosBancario_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        ArmaMedioPagoTodos(DtFormasPago, True)

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        DateTimeDesde.Value = DateTimeHasta.Value.AddMonths(-1)

        MaskedMovimiento.Text = "000000000000"

        GeneraCombosGrid()

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaReprocesos_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub ListaReprocesos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 AS Operacion,* FROM MovimientosBancarioCabeza "
        SqlN = "SELECT 2 AS Operacion,* FROM MovimientosBancarioCabeza "

        Dim SqlMovimiento As String = ""
        If CDbl(MaskedMovimiento.Text) <> 0 Then
            SqlMovimiento = "WHERE Movimiento = " & CDbl(MaskedMovimiento.Text) & " "
        Else : SqlMovimiento = "WHERE Movimiento LIKE '%' "
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "AND Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        SqlB = SqlB & SqlMovimiento & SqlFecha
        SqlN = SqlN & SqlMovimiento & SqlFecha

        LLenaGrid()

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        Grid.Focus()

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
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Movimiento"

        Grid.DataSource = bs
        bs.DataSource = View

    End Sub
    Private Sub GeneraCombosGrid()

        '    Estado.DataSource = DtEstadoActivoYBaja()
        '    Estado.DisplayMember = "Nombre"
        '    Estado.ValueMember = "Clave"

        TipoMovimiento.DataSource = DtTiposComprobantes(True)
        TipoMovimiento.DisplayMember = "Nombre"
        TipoMovimiento.ValueMember = "Codigo"

        MedioPago.DataSource = DtFormasPago
        MedioPago.DisplayMember = "Nombre"
        MedioPago.ValueMember = "Clave"

        Dim Row As DataRow

        Banco.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 order By Nombre;")
        Row = Banco.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Banco.DataSource.Rows.Add(Row)
        Banco.DisplayMember = "Nombre"
        Banco.ValueMember = "Clave"

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

        If Grid.Columns(e.ColumnIndex).Name = "Movimiento" Then
            If Not IsNothing(e.Value) Then
                e.Value = NumeroEditado(e.Value)
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaComprobante" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FechaComprobante" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = "1/1/1800" Then e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cuenta" Or Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else : e.Value = FormatNumber(e.Value, 0)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            e.Value = FormatNumber(e.Value, 2)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Estado" Then
            If e.Value = 3 Then
                e.Value = "Anulado"
            Else : e.Value = ""
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        Dim Abierto As Boolean

        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        Else : Abierto = False
        End If

        If Grid.CurrentRow.Cells("TipoMovimiento").Value = 93 Then
            UnReciboDebitoCreditoGenerica.PNota = Grid.Rows(e.RowIndex).Cells("Movimiento").Value
            UnReciboDebitoCreditoGenerica.PTipoNota = 93
            UnReciboDebitoCreditoGenerica.PBloqueaFunciones = True
            UnReciboDebitoCreditoGenerica.PAbierto = Abierto
            UnReciboDebitoCreditoGenerica.ShowDialog()
            Exit Sub
        End If

        UnMovimientoBancario.PMovimiento = Grid.CurrentRow.Cells("Movimiento").Value
        UnMovimientoBancario.PTipoNota = Grid.CurrentRow.Cells("TipoMovimiento").Value
        UnMovimientoBancario.PAbierto = Abierto
        UnMovimientoBancario.ShowDialog()
        UnMovimientoBancario.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub

End Class