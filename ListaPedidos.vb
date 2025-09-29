Imports System.Transactions
Public Class ListaPedidos
    Private WithEvents bs As New BindingSource
    Dim Dt As DataTable
    '
    Dim Sql As String
    Dim DTDetalle As DataTable
    Public PBloqueaFunciones As Boolean
    Dim CantidadTotal = 0
    Dim mRow As Integer = 0
    Dim newpage As Boolean = True
    Dim WithEvents pd As New System.Drawing.Printing.PrintDocument()
    Private Sub ListaListaDePrecios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(1001) Then PBloqueaFunciones = True

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        LlenaCombo(ComboCliente, "", "Clientes")
        ComboCliente.SelectedValue = 0
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)
        If e.KeyData = 112 Then
            ArreglaArchivo() '  Arregla un registro grabado con pedido con valor 0. Problema no encontrado.
            Me.Close()
            Exit Sub
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Exit Sub
        End If

        Sql = "SELECT *, '' as SucursalStr FROM PedidosCabeza "

        Dim SqlFecha As String = ""
        SqlFecha = "WHERE FechaEntregaDesde >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND FechaEntregaDesde < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlCliente As String = ""
        If ComboCliente.SelectedValue <> 0 Then
            SqlCliente = "AND Cliente = " & ComboCliente.SelectedValue & " "
        End If

        Sql = Sql & SqlFecha & SqlCliente & ";"

        Dt = New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = Dt

        For Each Fila As DataGridViewRow In Grid.Rows
            Fila.Cells("SucursalStr").Value = Fila.Cells("Sucursal").FormattedValue
        Next



        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position
        Grid.Focus()

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
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Pedidos", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonImpesion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImpesion.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        DTDetalle = New DataTable
        If Not Tablas.Read("SELECT L.* FROM PedidosDetalle AS L INNER JOIN Articulos AS A ON A.Clave = L.Articulo WHERE L.pedido = " & Grid.CurrentRow.Cells("Pedido").Value & " ORDER BY A.Nombre;", Conexion, DTDetalle) Then Exit Sub

        pd.DefaultPageSettings.Landscape = False
        UnSeteoImpresora.SeteaImpresion(pd)
        CantidadTotal = 0
        mRow = 0
        newpage = True

        pd.Print()
        pd.Dispose()

    End Sub
    Private Sub pd_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles pd.PrintPage

        ImpresionPedido(e, Dt.Select("PEDIDO = " & Grid.CurrentRow.Cells("Pedido").Value)(0), DTDetalle, mRow, newpage, CantidadTotal)

    End Sub
    Private Sub LlenaCombosGrid()

        Cliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes;")
        Cliente.DisplayMember = "Nombre"
        Cliente.ValueMember = "Clave"

        Sucursal.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM SucursalesClientes;")
        Dim Row As DataRow
        Row = Sucursal.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Sucursal.DataSource.Rows.Add(Row)
        Sucursal.DisplayMember = "Nombre"
        Sucursal.ValueMember = "Clave"

    End Sub
    Public Function ArreglaArchivo() As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT * FROM PedidosCabeza WHERE Pedido = " & 0 & ";", Conexion, Dt) Then Return False
        If Dt.Rows.Count <> 0 Then
            For Each Row As DataRow In Dt.Rows
                Row.Delete()
            Next
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM PedidosCabeza;", Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(Dt)
                End Using
            End Using
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
        End Try

        Dt.Clear()

        If Not Tablas.Read("SELECT * FROM PedidosDetalle WHERE Pedido = " & 0 & ";", Conexion, Dt) Then Return False
        If Dt.Rows.Count <> 0 Then
            For Each Row As DataRow In Dt.Rows
                Row.Delete()
            Next
        Else
            Return True
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM PedidosDetalle;", Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(Dt)
                    Return True
                End Using
            End Using
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
        End Try

    End Function
    Private Sub HacerModificacion()

        Dim Resul As Double

        Resul = ActualizaArchivos()

        If Resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = -2 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ButtonAceptar_Click(Nothing, Nothing)
        End If

    End Sub
    Private Function ActualizaArchivos() As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(Dt.GetChanges) Then
                    Resul = GrabaTabla(Dt.GetChanges, "PedidosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaEntregaDesde" Or Grid.Columns(e.ColumnIndex).Name = "FechaEntregaHsta" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        UnPedido.PPedido = Grid.CurrentRow.Cells("Pedido").Value
        UnPedido.ShowDialog()
        UnPedido.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub

    Private Sub ButtonAceptaCambios_Click(sender As Object, e As EventArgs) Handles ButtonAceptaCambios.Click

        bs.EndEdit()

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If IsNothing(Dt.GetChanges) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No Hay Cambios. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        GModificacionOk = False

        HacerModificacion()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub

    Private Sub ButtonMarcaTodos_Click(sender As Object, e As EventArgs) Handles ButtonMarcaTodos.Click

        If Grid.Rows.Count = 0 Then MsgBox("NO HAY PEDIDOS PARA SELECCIONAR!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Exit Sub

        For Each Fila As DataGridViewRow In Grid.Rows
            If Fila.Cells("Cerrado").Value Then Fila.Cells("Cerrado").Value = False
        Next

    End Sub

    Private Sub Grid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles Grid.CellContentClick

        If Grid.Columns(e.ColumnIndex).Name = "Cerrado" Then
            Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Not Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
        End If

    End Sub
End Class