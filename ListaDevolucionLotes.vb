Public Class ListaDevolucionLotes
    Public PBloqueaFunciones As Boolean
    Dim SqlB As String
    Dim SqlN As String
    Private WithEvents bs As New BindingSource
    '
    Dim DtIngresos As DataTable
    Dim DtGrid As DataTable
    Private Sub ListaDevolucionLotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        GeneraComboGrid()
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboProveedor.DataSource = ProveedoresDeFrutasYCosteo()
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        CreaDtGrid()

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaDevolucionLotes_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 AS Operacion,D.Estado AS Estado,D.Devolucion AS Devolucion,I.Proveedor,I.Deposito AS Deposito,D.Fecha AS Fecha,I.Remito AS Remito,I.Guia AS Guia,I.Lote AS Lote FROM DevolucionMercaCabeza AS D INNER JOIN IngresoMercaderiasCabeza AS I ON D.Lote = I.Lote "
        SqlN = "SELECT 2 AS Operacion,D.Estado AS Estado,D.Devolucion AS Devolucion,I.Proveedor,I.Deposito AS Deposito,D.Fecha AS Fecha,I.Remito AS Remito,I.Guia AS Guia,I.Lote AS Lote FROM DevolucionMercaCabeza AS D INNER JOIN IngresoMercaderiasCabeza AS I ON D.Lote = I.Lote "

        Dim SqlProveedor As String
        If ComboProveedor.SelectedValue = 0 Then
            SqlProveedor = "WHERE I.Proveedor LIKE '%' "
        Else
            SqlProveedor = "WHERE I.Proveedor = " & ComboProveedor.SelectedValue
        End If

        Dim SqlDeposito As String
        If ComboDeposito.SelectedValue = 0 Then
            SqlDeposito = "AND I.Deposito LIKE '%' "
        Else
            SqlDeposito = "AND I.Deposito = " & ComboDeposito.SelectedValue
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "AND D.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND D.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        SqlB = SqlB & SqlProveedor & SqlDeposito & SqlFecha
        SqlN = SqlN & SqlProveedor & SqlDeposito & SqlFecha

        LLenaGrid()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        ' If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position
        Grid.Focus()

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
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub TextFechaDesde_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextFechaHasta_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub LLenaGrid()

        DtIngresos = New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtIngresos) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtIngresos) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView

        View = DtIngresos.DefaultView
        View.Sort = "Devolucion"
        DtGrid.Clear()

        For Each Row As DataRowView In View
            AgregaADtGrid(Row("Operacion"), Row("Devolucion"), Row("Lote"), Row("Proveedor"), Row("Deposito"), Row("Remito"), Row("Guia"), Row("Fecha"), Row("Estado"))
        Next

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As DataColumn = New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Devolucion As DataColumn = New DataColumn("Devolucion")
        Devolucion.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Devolucion)

        Dim Proveedor As DataColumn = New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Remito As DataColumn = New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Remito)

        Dim Guia As DataColumn = New DataColumn("Guia")
        Guia.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Guia)

        Dim Deposito As DataColumn = New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Fecha As DataColumn = New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

    End Sub
    Private Sub AgregaADtGrid(ByVal Operacion As Integer, ByVal Devolucion As Double, ByVal Lote As Integer, ByVal Proveedor As Integer, ByVal Deposito As Integer, ByVal Remito As Double, ByVal Guia As Double, ByVal Fecha As Date, ByVal Estado As Integer)

        Dim Row As DataRow

        Row = DtGrid.NewRow()

        Row("Operacion") = Operacion
        Row("Devolucion") = Devolucion
        Row("Lote") = Lote
        Row("Proveedor") = Proveedor
        Row("Deposito") = Deposito
        Row("Remito") = Remito
        Row("Guia") = Guia
        Row("Fecha") = Fecha
        If Estado = 3 Then
            Row("Estado") = Estado
        Else
            Row("Estado") = 0
        End If

        DtGrid.Rows.Add(Row)

    End Sub
    Private Sub GeneraComboGrid()

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

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

        If Grid.Columns(e.ColumnIndex).Name = "Proveedor" Then
            If Not IsNothing(e.Value) Then
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
                        Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    Else : Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                    End If
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Devolucion" Then
            If Not IsNothing(e.Value) Then
                e.Value = Format(e.Value, "#,###")

            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            If Not IsNothing(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = Format(e.Value, "0000-00000000")
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Guia" Then
            If Not IsNothing(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 0)
                Else : e.Value = Format(0, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Stock" Then
            If Not IsNothing(e.Value) Then
                e.Value = FormatNumber(e.Value, 0)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If


    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        Dim Abierto As Boolean
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Devolucion" Then
            If Grid.CurrentCell.Value = 0 Then Exit Sub
            UnaDevolucionLote.PDevolucion = Grid.CurrentCell.Value
            UnaDevolucionLote.PLote = Grid.CurrentRow.Cells("Lote").Value
            UnaDevolucionLote.PAbierto = Abierto
            UnaDevolucionLote.PBloqueaFunciones = PBloqueaFunciones
            UnaDevolucionLote.ShowDialog()
            UnaDevolucionLote.Dispose()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Or Grid.Columns(e.ColumnIndex).Name = "Guia" Then
            If Grid.CurrentCell.Value = 0 Then Exit Sub
            UnIngresoMercaderia.PLote = Grid.CurrentRow.Cells("Lote").Value
            UnIngresoMercaderia.PAbierto = Abierto
            UnIngresoMercaderia.PBloqueaFunciones = PBloqueaFunciones
            UnIngresoMercaderia.ShowDialog()
            UnIngresoMercaderia.Dispose()
        End If

    End Sub

End Class