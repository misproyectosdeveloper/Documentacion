Public Class ListaRecepciones
    Public POrdenCompra As Integer
    Public PTipo As Integer
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    Private Sub ListaRecepciones_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50
        Grid.AutoGenerateColumns = False

        ComboEmisor.DataSource = ProveedoresTodos()
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

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        CreaDtGrid()

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaRecepciones_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        ComboEmisor.Focus()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaRecepciones_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        LLenaGrid()

    End Sub
    Private Sub ButtonDevolucion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDevolucion.Click

        If Not PermisoEscritura(12) Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.Rows.Count = 0 Then Exit Sub

        If IsDBNull(Grid.CurrentRow.Cells("Ingreso").Value) Then Exit Sub

        If Grid.CurrentRow.Cells("Estado").Value <> 0 Then
            MsgBox("Ingreso Esta Anulado.")
            Exit Sub
        End If

        UnaDevolucionRecepcion.PIngreso = Grid.CurrentRow.Cells("Ingreso").Value
        UnaDevolucionRecepcion.ShowDialog()
        UnaDevolucionRecepcion.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Recepción de Insumos", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

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

        Dim Importe As Double = 0

        Dim SqlB As String = ""
        Dim SqlN As String = ""

        If PTipo = 2 Then
            SqlB = "SELECT 1 AS Operacion,*,CAST(FLOOR(CAST(C.Fecha AS FLOAT)) AS DATETIME) AS Fecha1,C.Estado,D.Articulo,D.Cantidad,D.Devueltas FROM IngresoInsumoCabeza AS C INNER JOIN IngresoInsumoDetalle AS D ON C.Ingreso = D.Ingreso WHERE "
            SqlN = "SELECT 2 AS Operacion,*,CAST(FLOOR(CAST(C.Fecha AS FLOAT)) AS DATETIME) AS Fecha1,C.Estado,D.Articulo,D.Cantidad,D.Devueltas FROM IngresoInsumoCabeza AS C INNER JOIN IngresoInsumoDetalle AS D ON C.Ingreso = D.Ingreso WHERE "
        End If

        Dim SqlFecha As String
        SqlFecha = "Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlProveedor As String = ""
        If ComboEmisor.SelectedValue <> 0 Then
            SqlProveedor = "AND Proveedor = " & ComboEmisor.SelectedValue & " "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        Dim SqlEstado As String = ""
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = " AND Estado = " & ComboEstado.SelectedValue
        End If

        SqlB = SqlB & SqlFecha & SqlProveedor & SqlDeposito & SqlEstado
        SqlN = SqlN & SqlFecha & SqlProveedor & SqlDeposito & SqlEstado

        If POrdenCompra <> 0 Then
            If PTipo = 1 Then
                SqlB = "SELECT 1 AS Operacion,C.Lote AS Ingreso,C.Guia,C.*,CAST(FLOOR(CAST(C.Fecha AS FLOAT)) AS DATETIME) AS Fecha1,1 AS Estado,D.Articulo,D.Cantidad,D.Baja AS Devueltas FROM IngresoMercaderiasCabeza AS C INNER JOIN Lotes AS D ON C.Lote = D.Lote WHERE C.OrdenCompra = " & POrdenCompra & ";"
                SqlN = "SELECT 2 AS Operacion,C.Lote AS Ingreso,C.Guia,C.*,CAST(FLOOR(CAST(C.Fecha AS FLOAT)) AS DATETIME) AS Fecha1,1 AS Estado,D.Articulo,D.Cantidad,D.Baja AS Devueltas FROM IngresoMercaderiasCabeza AS C INNER JOIN Lotes AS D ON C.Lote = D.Lote WHERE C.OrdenCompra = " & POrdenCompra & ";"
            End If
            If PTipo = 2 Then
                SqlB = "SELECT 1 AS Operacion,C.Ingreso,0 AS C.Guia,*,CAST(FLOOR(CAST(C.Fecha AS FLOAT)) AS DATETIME) AS Fecha1,C.Estado,D.Articulo,D.Cantidad,D.Devueltas FROM IngresoInsumoCabeza AS C INNER JOIN IngresoInsumoDetalle AS D ON C.Ingreso = D.Ingreso WHERE Tipo = 1 AND OrdenCompra = " & POrdenCompra & ";"
                SqlN = "SELECT 2 AS Operacion,C.Ingreso,0 AS C.Guia,*,CAST(FLOOR(CAST(C.Fecha AS FLOAT)) AS DATETIME) AS Fecha1,C.Estado,D.Articulo,D.Cantidad,D.Devueltas FROM IngresoInsumoCabeza AS C INNER JOIN IngresoInsumoDetalle AS D ON C.Ingreso = D.Ingreso WHERE Tipo = 1 AND OrdenCompra = " & POrdenCompra & ";"
            End If
            ComboEmisor.Enabled = False
            ButtonDevolucion.Visible = False
        End If

        Dim Dt As New DataTable
        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Ingreso"

        DtGrid.Clear()

        Dim IngresoAnt As Double = 0
        Dim Row1 As DataRow

        For Each Row As DataRowView In View
            If IngresoAnt <> Row("Ingreso") Then
                Row1 = DtGrid.NewRow
                Row1("Operacion") = Row("Operacion")
                Row1("Ingreso") = Row("Ingreso")
                Row1("Proveedor") = Row("Proveedor")
                Row1("Fecha") = Row("Fecha")
                Row1("OrdenCompra") = Row("OrdenCompra")
                If Row("Remito") = 0 Then
                    Row1("Remito") = Row("Guia")
                Else
                    Row1("Remito") = Row("Remito")
                End If
                Row1("Deposito") = Row("Deposito")
                Row1("Articulo") = Row("Articulo")
                Row1("Cantidad") = Row("Cantidad")
                Row1("Devueltas") = Row("Devueltas")
                If Row("Estado") = 3 Then
                    Row1("Estado") = Row("Estado")
                Else
                    Row1("Estado") = 0
                End If
                DtGrid.Rows.Add(Row1)
                IngresoAnt = Row("Ingreso")
            Else
                Row1 = DtGrid.NewRow
                Row1("Proveedor") = 0
                Row1("Deposito") = 0
                Row1("Articulo") = Row("Articulo")
                Row1("Cantidad") = Row("Cantidad")
                Row1("Devueltas") = Row("Devueltas")
                Row1("Estado") = 0
                DtGrid.Rows.Add(Row1)
            End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Ingreso As New DataColumn("Ingreso")
        Ingreso.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Ingreso)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim OrdenCompra As New DataColumn("OrdenCompra")
        OrdenCompra.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(OrdenCompra)

        Dim Remito As New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Remito)

        Dim Deposito As New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cantidad)

        Dim Devueltas As New DataColumn("Devueltas")
        Devueltas.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Devueltas)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

    End Sub
    Private Sub LlenaCombosGrid()

        Proveedor.DataSource = ProveedoresTodos()
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        If PTipo = 1 Then
            Deposito.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = " & 19 & " ORDER BY Nombre;")
        End If
        If PTipo = 2 Then
            Deposito.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = " & 20 & " ORDER BY Nombre;")
        End If
        Dim Row As DataRow = Deposito.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Deposito.DataSource.Rows.Add(Row)
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

        If PTipo = 1 Then
            Articulo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Articulos;")
            Articulo.DisplayMember = "Nombre"
            Articulo.ValueMember = "Clave"
        End If
        If PTipo = 2 Then
            Articulo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Insumos;")
            Articulo.DisplayMember = "Nombre"
            Articulo.ValueMember = "Clave"
        End If

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    Private Function HallaTipoOrdenCompra(ByVal Orden As Decimal) As Integer

        Dim Dt As New DataTable
        Dim OrdenW As Decimal

        If Not Tablas.Read("SELECT Tipo FROM OrdenCompraCabeza WHERE Orden = " & Orden & ";", Conexion, Dt) Then
            MsgBox("Error al leer Tabla: OrdenCompraCabeza.", MsgBoxStyle.Critical)
            OrdenW = -1
        End If
        If Dt.Rows.Count = 0 Then
            MsgBox("Orden Compra no se Encuentra.", MsgBoxStyle.Exclamation)
            OrdenW = -1
        End If
        OrdenW = Dt.Rows(0).Item("Tipo")

        Dt.Dispose()
        Return OrdenW

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If IsDBNull(e.Value) Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Ingreso" Then
            e.Value = NumeroEditado(e.Value)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "OrdenCompra" Or Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            e.Value = NumeroEditado(e.Value)
        End If


        If Grid.Columns(e.ColumnIndex).Name = "Devueltas" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, 0)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        Dim Abierto As Boolean

        If Grid.Columns(e.ColumnIndex).Name = "Ingreso" Then
            If IsDBNull(Grid.CurrentRow.Cells("Ingreso").Value) Then Exit Sub
            If PTipo = 1 Then
                UnIngresoMercaderia.PAbierto = Abierto
                UnIngresoMercaderia.PLote = Grid.CurrentCell.Value
                UnIngresoMercaderia.ShowDialog()
            End If
            If PTipo = 2 Then
                UnaRecepcion.PIngreso = Grid.CurrentCell.Value
                UnaRecepcion.ShowDialog()
                If UnaRecepcion.PActualizacionOk Then LLenaGrid()
                UnaRecepcion.Dispose()
                Exit Sub
            End If
            Exit Sub
        End If
    End Sub

  
End Class