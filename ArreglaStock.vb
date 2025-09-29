Public Class ArreglaStock

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim DtLotes As New DataTable
        DtLotes = CreaDt()

        SeleccionaLotes(DtLotes, Conexion)
        SeleccionaLotes(DtLotes, ConexionN)

        Dim Cantidad As Decimal = 0

        For Each Row As DataRow In DtLotes.Rows
            Row("Cantidad") = TraeCantidad(Row("Lote"), Row("Secuencia"))
            Dim Articulo As Integer
            TraeStock(Row("Lote"), Row("Secuencia"), Row("Operacion"), Row("Ingreso"), Row("Stock"), Articulo)
            Dim Dt1 As New DataTable
            If Not Tablas.Read("SELECT Especie FROM Articulos WHERE Clave = " & Articulo & ";", Conexion, Dt1) Then End
            Row("Especie") = Dt1.Rows(0).Item("Especie")
            Row("Articulo") = Articulo
        Next

        Grid.Rows.Clear()

        For Each Row As DataRow In DtLotes.Rows
            If Row("Ingreso") - Row("Cantidad") <> Row("Stock") Then
                Grid.Rows.Add(Row("Operacion"), Row("Lote"), Row("Secuencia"), NombreEspecie(Row("Especie")), NombreArticulo(Row("Articulo")), Row("Ingreso"), Row("Stock"), Row("Cantidad"))
            End If
        Next

    End Sub
    Private Sub SeleccionaLotes(ByRef DtLotes As DataTable, ByVal ConexionStr As String)

        Dim Operacion As Integer = 0
        Dim RowsBusqueda() As DataRow

        If ConexionStr = Conexion Then
            Operacion = 2
        Else
            Operacion = 1
        End If

        Dim SqlFecha As String
        SqlFecha = " AND R.Fecha >= '20161201'"

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT A.Operacion,A.Lote, A.Secuencia FROM AsignacionLotes AS A INNER JOIN RemitosCabeza AS R ON A.Comprobante = R.Remito WHERE A.Facturado = 0 AND A.TipoComprobante = 1 AND A.Operacion = " & Operacion & SqlFecha & ";", ConexionStr, Dt) Then End
        If Not Tablas.Read("SELECT A.Operacion,A.Lote, A.Secuencia FROM AsignacionLotes AS A INNER JOIN FacturasCabeza AS R ON A.Comprobante = R.Factura WHERE R.Rel = 0 AND A.TipoComprobante = 2 AND A.Operacion = " & Operacion & SqlFecha & ";", ConexionStr, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            RowsBusqueda = DtLotes.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
            If RowsBusqueda.Length = 0 Then
                Dim RowA As DataRow = DtLotes.NewRow
                RowA("Operacion") = Row("Operacion")
                RowA("Lote") = Row("Lote")
                RowA("Secuencia") = Row("Secuencia")
                RowA("Cantidad") = 0
                RowA("Ingreso") = 0
                RowA("Stock") = 0
                RowA("Especie") = 0
                RowA("Articulo") = 0
                DtLotes.Rows.Add(RowA)
            End If
        Next

        Dt.Dispose()

    End Sub
    Private Function TraeCantidad(ByVal Lote As Integer, ByVal Secuencia As Integer) As Decimal

        Dim Cantidad As Decimal = 0
        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Cantidad FROM AsignacionLotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & " AND (TipoComprobante = 1 OR TipoComprobante = 2);", Conexion, Dt) Then End
        If Not Tablas.Read("SELECT Cantidad FROM AsignacionLotes WHERE Rel = 0 AND Lote = " & Lote & " AND Secuencia = " & Secuencia & " AND (TipoComprobante = 1 OR TipoComprobante = 2);", ConexionN, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Cantidad")
        Next

        Dt.Dispose()

        Return Cantidad

    End Function
    Private Sub TraeStock(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As Integer, ByRef Ingreso As Decimal, ByRef Stock As Decimal, ByRef articulo As Integer)

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Ingreso = 0 : Stock = 0 : articulo = 0

        'Concidero no hay translado.
        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Cantidad,Stock,Baja,BajaReproceso,Descarte,Merma,Articulo FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", ConexionStr, Dt) Then End
        '        Ingreso = Dt.Rows(0).Item("Cantidad") - Dt.Rows(0).Item("Baja") - Dt.Rows(0).Item("BajaReproceso") - Dt.Rows(0).Item("Descarte") - Dt.Rows(0).Item("Merma")
        Ingreso = Dt.Rows(0).Item("Cantidad") - Dt.Rows(0).Item("Baja") - Dt.Rows(0).Item("BajaReproceso") - Dt.Rows(0).Item("Descarte")
        Stock = Dt.Rows(0).Item("Stock")
        articulo = Dt.Rows(0).Item("Articulo")

        Dt.Dispose()

    End Sub
    Public Function CreaDt() As DataTable

        Dim Dt As New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Secuencia)

        Dim Especie As New DataColumn("Especie")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Especie)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Articulo)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        Dt.Columns.Add(Cantidad)

        Dim Ingreso As New DataColumn("Ingreso")
        Ingreso.DataType = System.Type.GetType("System.Decimal")
        Dt.Columns.Add(Ingreso)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Decimal")
        Dt.Columns.Add(Stock)

        Return Dt

    End Function
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, "", "", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
End Class