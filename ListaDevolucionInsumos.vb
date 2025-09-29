Public Class ListaDevolucionInsumos
    Public POrdenCompra As Integer
    Private WithEvents bs As New BindingSource
    Private Sub ListaDevolucionInsumos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50
        Grid.AutoGenerateColumns = False

        ComboEmisor.DataSource = ProveedoresDeInsumos()
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = 0

        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboDeposito, 20)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaDevolucionInsumos_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        ComboEmisor.Focus()

    End Sub
    Private Sub ListaDevolucionInsumos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PreparaArchivos()

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

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
    Private Sub PreparaArchivos()

        Dim Importe As Double = 0

        Dim SqlB As String = ""

        SqlB = "SELECT 1 AS Operacion,D.Devolucion,CAST(FLOOR(CAST(D.Fecha AS FLOAT)) AS DATETIME) AS Fecha1,D.Estado,I.Ingreso,I.Proveedor,I.Remito,I.OrdenCompra,I.Deposito FROM DevolucionInsumoCabeza AS D INNER JOIN IngresoInsumoCabeza AS I ON D.Ingreso = I.Ingreso WHERE "

        Dim SqlFecha As String
        SqlFecha = "D.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND D.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlProveedor As String = ""
        If ComboEmisor.SelectedValue <> 0 Then
            SqlProveedor = "AND I.Proveedor = " & ComboEmisor.SelectedValue & " "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND I.Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        SqlB = SqlB & SqlFecha & SqlProveedor & SqlDeposito

        Dim Dt As New DataTable
        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Devolucion"

        Grid.DataSource = bs
        bs.DataSource = View

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

    End Sub
    Private Sub LlenaCombosGrid()

        Proveedor.DataSource = ProveedoresDeInsumos()
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        Deposito.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = " & 20 & " ORDER BY Nombre;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Devolucion" Then
            e.Value = NumeroEditado(e.Value)
            If Grid.Rows(e.RowIndex).Cells("Estado").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Estado").Value = 0
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Or Grid.Columns(e.ColumnIndex).Name = "OrdenCompra" Then
            e.Value = NumeroEditado(e.Value)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            If e.Value <> 0 Then
                e.Value = Format(e.Value, "0000-00000000")
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Devolucion" Then
            UnaDevolucionRecepcion.PDevolucion = Grid.CurrentCell.Value
            UnaDevolucionRecepcion.ShowDialog()
            UnaDevolucionRecepcion.Dispose()
            If GModificacionOk Then PreparaArchivos()
            Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            UnaRecepcion.PIngreso = Grid.CurrentCell.Value
            UnaRecepcion.ShowDialog()
            If UnaRecepcion.PActualizacionOk Then PreparaArchivos()
            UnaRecepcion.Dispose()
            Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "OrdenCompra" Then
            UnaOrdenCompra.POrden = Grid.CurrentCell.Value
            UnaOrdenCompra.ShowDialog()
            If UnaOrdenCompra.PActualizacionOk Then PreparaArchivos()
            UnaOrdenCompra.Dispose()
            Exit Sub
        End If

    End Sub

End Class