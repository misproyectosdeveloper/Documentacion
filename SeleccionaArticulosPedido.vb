Imports System.Transactions
Public Class SeleccionaArticulosPedido
    Public PCliente As Integer
    Public PPedido As Integer
    Public PSucursal As Integer
    Public PFechaEntrega As Date
    Public PDtPedido As DataTable
    Public PPorCuentaYOrden As Integer
    Public PSucursalPorCuentaYOrden As Integer
    Public PEsRemito As Boolean
    Public PEsFactura As Boolean
    '
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtGrid2 As DataTable
    ' 
    Private WithEvents bs As New BindingSource
    Private WithEvents bs2 As New BindingSource
    '
    Dim cb As ComboBox
    Dim Lista As Integer = 0
    Private Sub SeleccionaArticulosPedido_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        Grid2.AutoGenerateColumns = False

        LlenaCombosGrid2()
        LlenaCombosGrid()

        TextCliente.Text = NombreCliente(PCliente)
        FechaEntrega.Value = PFechaEntrega

        ArmaArchivos2()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        bs.EndEdit()

        If Not Valida() Then Exit Sub

        For Each Row As DataGridViewRow In Grid2.Rows
            If Row.Cells("Sel2").Value Then
                PPedido = Row.Cells("Pedido").Value
                PSucursal = Row.Cells("Sucursal").Value
            End If
        Next

        PDtPedido.Clear()

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value Then
                Dim Row1 As DataRow = PDtPedido.NewRow
                Row1("Articulo") = Row.Cells("Articulo").Value
                PDtPedido.Rows.Add(Row1)
            End If
        Next

        Me.Close()

    End Sub
    Private Sub ButtonMarcarTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMarcarTodos.Click

        For Each Row As DataGridViewRow In Grid.Rows
            Row.Cells("Sel").Value = True
        Next

    End Sub
    Private Sub ButtonDesmarcarTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDesmarcarTodos.Click

        For Each Row As DataGridViewRow In Grid.Rows
            Row.Cells("Sel").Value = False
        Next

    End Sub
    Private Sub ButtonImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportar.Click

        OpcionNumero.PEsImportacionRemito = True
        OpcionNumero.PEsPedido = True
        OpcionNumero.ShowDialog()
        If OpcionNumero.PRegresar Then OpcionNumero.Dispose() : Exit Sub
        Dim ConexionStr As String
        Dim Remito As Decimal = OpcionNumero.PRemito
        Dim ConCantidad As Boolean = OpcionNumero.PConCantidad
        Dim ConPrecios As Boolean = OpcionNumero.PConPrecios
        If OpcionNumero.PAbierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If
        OpcionNumero.Dispose()

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Articulo, (Cantidad - Devueltas) AS Cantidad,Precio,KilosXUnidad FROM RemitosDetalle WHERE Remito = " & Remito & ";", ConexionStr, Dt) Then Exit Sub
        If Dt.Rows.Count = 0 Then
            Dt.Dispose()
            MsgBox("Remito NO Existe. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Total As Integer = 0
        Dim Importados As Integer = 0

        For Each Row1 As DataRow In Dt.Rows
            Total = Total + 1
            For Each Row As DataGridViewRow In Grid.Rows
                If Row1("Articulo") = Row.Cells("Articulo").Value Then Row.Cells("Sel").Value = True : Importados = Importados + 1
            Next
        Next

        MsgBox("Importados: " & Importados & "    No Importados: " & Total - Importados)

        Dt.Dispose()

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
    Private Sub ButtonUltimo2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUltimo2.Click

        bs2.MoveLast()

    End Sub
    Private Sub ButtonAnterior2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnterior2.Click

        bs2.MovePrevious()

    End Sub
    Private Sub ButtonPosterior2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPosterior2.Click

        bs2.MoveNext()

    End Sub
    Private Sub ButtonPrimero2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPrimero2.Click

        bs2.MoveFirst()

    End Sub
    Private Sub ArmaArchivos2()

        DtGrid2 = New DataTable
        If Not PideDatosPedido(PFechaEntrega, PCliente, PSucursal, False, True, DtGrid2) Then Me.Close() : Exit Sub

        Grid2.DataSource = bs2
        bs2.DataSource = DtGrid2

        For Each Row As DataGridViewRow In Grid2.Rows
            If Row.Cells("Pedido").Value = PPedido Then
                Row.Cells("Sel2").Value = True
                ArmaArchivos(PPedido)
            End If
        Next

    End Sub
    Private Sub ArmaArchivos(ByVal Pedido As Integer)

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtCabeza = New DataTable
        If Not Tablas.Read("SELECT Cliente FROM PedidosCabeza WHERE Pedido = " & Pedido & ";", Conexion, DtCabeza) Then Me.Close() : Exit Sub

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT L.*,A.Nombre FROM PedidosDetalle AS L INNER JOIN Articulos AS A ON A.Clave = L.Articulo WHERE L.Cantidad > 0 AND L.pedido = " & Pedido & "ORDER BY A.Nombre;", Conexion, DtDetalle) Then Me.Close() : Exit Sub

        Grid.EndEdit()  'Para que no cancele (?).
        Grid.DataSource = bs
        bs.DataSource = DtDetalle

        For Each Row As DataGridViewRow In Grid.Rows
            Row.Cells("Saldo").Value = Row.Cells("Cantidad").Value - Row.Cells("Entregada").Value
            If Lista <> 0 Then
                Row.Cells("Precio").Value = HalloPrecioDeLista(Lista, Row.Cells("Articulo").Value, PFechaEntrega)
            End If
        Next

        If Pedido = PPedido Then
            For Each Row1 As DataRow In PDtPedido.Rows
                For Each Row As DataGridViewRow In Grid.Rows
                    If Row.Cells("Articulo").Value = Row1("Articulo") Then
                        Row.Cells("Sel").Value = True
                    End If
                Next
            Next
        End If

        Grid.ReadOnly = True

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LlenaCombosGrid2()

        Dim Sql As String = "SELECT Clave,Nombre FROM SucursalesClientes WHERE Estado = 1 AND Cliente = " & PCliente & ";"

        Sucursal.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = Sucursal.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        Sucursal.DataSource.Rows.Add(Row)
        Sucursal.DisplayMember = "Nombre"
        Sucursal.ValueMember = "Clave"

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Articulo.DataSource = ArticulosActivos()
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        Dim Contador As Integer = 0

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value Then
                Contador = Contador + 1
            End If
        Next

        If Contador = 0 Then
            MsgBox("Debe Informar un Articulo.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID2.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid2_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid2.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid2.Columns(e.ColumnIndex).Name = "FechaEnt" Or Grid2.Columns(e.ColumnIndex).Name = "FechaHas" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

    End Sub
    Private Sub Grid2_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid2.CellContentClick
        'Sel cambia de valor solo con: Grid.Columns("Sel").ReadOnly = True o Grid.ReadOnly = true

        Dim Pedido As Integer = 0
        Dim PorUnidadEnLista As Boolean, FinalEnLista As Boolean

        Lista = 0

        If Grid2.Columns(e.ColumnIndex).Name = "Sel2" Then
            Dim Check As New DataGridViewCheckBoxCell
            Check = Grid2.Rows(e.RowIndex).Cells("Sel2")
            Check.Value = Not Check.Value
            If Check.Value Then
                For Each Row As DataGridViewRow In Grid2.Rows
                    If Row.Index <> e.RowIndex Then Row.Cells("Sel2").Value = False
                    If Row.Index = e.RowIndex Then
                        Pedido = Row.Cells("Pedido").Value
                        If PPorCuentaYOrden = 0 Then
                            Lista = HallaListaPrecios(PCliente, Row.Cells("Sucursal").Value, PFechaEntrega, PorUnidadEnLista, FinalEnLista)
                        Else
                            Lista = HallaListaPrecios(PPorCuentaYOrden, PSucursalPorCuentaYOrden, PFechaEntrega, PorUnidadEnLista, FinalEnLista)
                        End If
                        If Lista = -1 Then
                            If PPorCuentaYOrden <> 0 Then
                                MsgBox("Error en Lista de Precios Para Cliente Por Cuenta Y Orden " & NombreCliente(PPorCuentaYOrden), MsgBoxStyle.Critical)
                            Else : MsgBox("Error en Lista de Precios Para Cliente " & NombreCliente(PCliente), MsgBoxStyle.Critical)
                            End If
                            Pedido = 0
                        End If
                    End If
                Next
                ArmaArchivos(Pedido)
            End If
        End If

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Entregada" Or Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            If e.Value = 0 Then e.Value = Format(0, "#")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Precio" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = Format(e.Value, "0.00")
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Grid_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellContentClick

        If Grid.Columns(e.ColumnIndex).Name = "Sel" Then
            Dim Check As New DataGridViewCheckBoxCell
            Check = Grid.Rows(e.RowIndex).Cells("Sel")
            Check.Value = Not Check.Value
            If Check.Value Then
                Dim Contador As Integer = 0
                For Each Row As DataGridViewRow In Grid.Rows
                    If Row.Cells("Sel").Value Then Contador = contador + 1
                Next
                ' If PEsRemito And Contador > GLineasRemitos Then
                '    MsgBox("Supera Cantidad Articulo Permitidos en Remito.")
                '    Check.Value = False
                '    Exit Sub
                'End If
                If PEsFactura And Contador > GLineasFacturas Then
                    MsgBox("Supera Cantidad Articulo Permitidos en Factura.")
                    Check.Value = False
                    Exit Sub
                End If
                If Lista <> 0 Then
                    If Grid.Rows(e.RowIndex).Cells("Precio").Value = 0 Then
                        If PPorCuentaYOrden = 0 Then
                            MsgBox("Articulo No esta definido en Lista de Precios " & Lista & " Cliente " & NombreCliente(PCliente) & " y Fecha de Entrega " & Format(PFechaEntrega, "dd/MM/yyyy"))
                        Else
                            MsgBox("Articulo No esta definido en Lista de Precios " & Lista & " Cliente " & NombreCliente(PPorCuentaYOrden) & " y Fecha de Entrega " & Format(PFechaEntrega, "dd/MM/yyyy"))
                        End If
                        Check.Value = False
                    End If
                End If
            End If
        End If

    End Sub
   
End Class


