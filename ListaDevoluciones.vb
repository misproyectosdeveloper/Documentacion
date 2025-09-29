Public Class ListaDevoluciones
    Public PBloqueaFunciones As Boolean
    Dim SqlB As String
    Dim SqlN As String
    Private WithEvents bs As New BindingSource
    Dim Dt As DataTable
    Private Sub ListaDevoluciones_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboCliente, "", "Clientes")
        ComboCliente.SelectedValue = 0
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        '     DateTimeDesde.Value = DateTimeHasta.Value.AddMonths(-1)

        '    ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaDevoluciones_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 as Operacion,DevolucionCabeza.*,RemitosCabeza.Cliente AS Cliente,RemitosCabeza.Deposito FROM DevolucionCabeza INNER JOIN RemitosCabeza " & _
              "ON DevolucionCabeza.Remito = RemitosCabeza.Remito "
        SqlN = "SELECT 2 as Operacion,DevolucionCabeza.*,RemitosCabeza.Cliente AS Cliente,RemitosCabeza.Deposito FROM DevolucionCabeza INNER JOIN RemitosCabeza " & _
              "ON DevolucionCabeza.Remito = RemitosCabeza.Remito "

        Dim SqlRemito As String
        If MaskedRemito.Text <> "" Then
            SqlRemito = "WHERE DevolucionCabeza.Remito = " & Val(MaskedRemito.Text) & " "
        Else : SqlRemito = "WHERE DevolucionCabeza.Remito LIKE '%' "
        End If

        Dim SqlCliente As String
        If ComboCliente.SelectedValue <> 0 Then
            SqlCliente = "AND Cliente = " & ComboCliente.SelectedValue & " "
        End If

        Dim SqlDeposito As String
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        Dim SqlFecha As String
        Dim FechaDesde As Date = CDate(DateTimeDesde.Value)
        Dim FechaHasta As Date = CDate(DateTimeHasta.Value)
        SqlFecha = "AND DevolucionCabeza.Fecha BETWEEN '" & FechaParaSql(DateTimeDesde.Value) & "' AND '" & FechaParaSql(DateTimeHasta.Value.AddDays(1)) & "' "

        SqlB = SqlB & SqlRemito & SqlCliente & SqlDeposito & SqlFecha
        SqlN = SqlN & SqlRemito & SqlCliente & SqlDeposito & SqlFecha

        LLenaGrid()

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position
        Grid.Focus()

    End Sub
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

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

        Try
            Dt = New DataTable
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
            If PermisoTotal Then
                If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
            End If
            Grid.DataSource = bs
            bs.DataSource = Dt
        Catch err As OleDb.OleDbException
            MsgBox(err.Message)
        End Try

        If PermisoTotal Then
            Grid.Columns("Candado").Visible = True
        End If

    End Sub
    Private Sub LlenaCombosGrid()

        Cliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes;")
        Cliente.DisplayMember = "Nombre"
        Cliente.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoSuspendidoBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Codigo"

        Deposito.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        If MaskedRemito.Text <> "" And Not MaskedRemito.MaskCompleted Then
            MsgBox("Debe completar Remito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedRemito.Focus()
            Return False
        End If
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

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "0000-00000000")
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
                        Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    Else : Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                    End If
                End If
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

        Dim Row As DataRowView
        Row = bs.Current
        If bs.Count > 0 Then
            If IsDBNull(Row("Devolucion")) Then Exit Sub
        Else : Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Devolucion" Then
            GModificacionOk = False
            UnaDevolucion.PDevolucion = Row("Devolucion")
            If Row("Operacion") = 1 Then
                UnaDevolucion.PAbierto = True
            Else : UnaDevolucion.PAbierto = False
            End If
            UnaDevolucion.ShowDialog()
            UnaDevolucion.Dispose()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            GModificacionOk = False
            UnRemito.PRemito = Row("Remito")
            UnRemito.PCliente = Row("Cliente")
            If Row("Operacion") = 1 Then
                UnRemito.PAbierto = True
            Else : UnRemito.PAbierto = False
            End If
            UnRemito.PBloqueaFunciones = True
            UnRemito.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

    End Sub
   
   
End Class