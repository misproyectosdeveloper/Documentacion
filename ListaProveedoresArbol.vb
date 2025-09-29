Imports System.Drawing.Printing
Imports System.Data.OleDb
Public Class ListaProveedoresArbol
    Dim Dt As DataTable
    Dim Producto As Integer
    Dim DtListado As DataTable
    Dim I As Integer = 0
    Private Sub Proveedores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Dim NodoRaiz As New TreeNode

        TreeView1.Nodes.Clear()
        NodoRaiz.Name = "0"
        NodoRaiz.Text = "Proveedores"
        TreeView1.Nodes.Add(NodoRaiz)
        Llena_Nivel_1(NodoRaiz)
        TreeView1.Nodes("0").Expand()

    End Sub
    Private Sub TreeView1_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseDoubleClick

        If e.Node.Level <> 2 Then Exit Sub

        UnProveedor.PProveedor = Val(e.Node.Name)
        UnProveedor.ShowDialog()
        If UnProveedor.PModificacionOk Then Proveedores_Load(Nothing, Nothing)
        UnProveedor.Dispose()

    End Sub
    Private Sub ReArmaArbol(ByVal Nodo As TreeNode)

        Nodo.Nodes.Clear()
        If Nodo.Level > 0 Then
            Call Llena_Nivel_2(Nodo)
        Else : Call Llena_Nivel_1(Nodo)
        End If

    End Sub
    Private Function Llena_Nivel_1(ByVal Padre As TreeNode) As Boolean

        Dim dt As New DataTable
        Dim Sql As String = ""
        Dim Fila As DataRow

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Try
            dt.Clear()
            dt = ArmaProductos()
            For Each Fila In dt.Rows
                Dim nodo As New TreeNode
                nodo.Text = Fila.Item("Nombre")
                nodo.Name = Fila.Item("Codigo")
                If Fila.Item("Codigo") <> 0 Then
                    Padre.Nodes.Add(nodo)
                    Call Llena_Nivel_2(nodo)
                End If
            Next
        Catch ex As Exception
        Finally
            dt.Dispose()
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End Try

    End Function
    Private Function Llena_Nivel_2(ByVal Padre As TreeNode) As Boolean

        Dim dt As New DataTable
        Dim Sql As String = ""
        Dim Fila As DataRow

        Sql = "SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion <> 4 AND Producto = " & Val(Padre.Name) & _
              " ORDER BY Nombre;"
        If Not LLenaDt(dt, Sql) Then Me.Close() : Exit Function
        If dt.Rows.Count = 0 Then Exit Function
        For Each Fila In dt.Rows
            Dim nodo As New TreeNode
            nodo.Text = Fila.Item("Nombre")
            nodo.Name = Fila.Item("Clave")
            Padre.Nodes.Add(nodo)
        Next
        dt.Dispose()

    End Function
    Private Function LLenaDt(ByRef Dt As DataTable, ByVal Sql As String) As Boolean

        Dt.Clear()
        If Not Tablas.Read(Sql, Conexion, Dt) Then Return False

        Return True

    End Function
    Private Sub ButtonListado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonListado.Click

        DtListado = New DataTable

        DtListado = Tablas.Leer("SELECT * FROM Proveedores ORDER BY Nombre;")

        Dim print_document As New PrintDocument
        AddHandler print_document.PrintPage, AddressOf Print_PrintPage

        print_document.Print()

    End Sub
    Private Sub Print_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '        Dim xPos As Single = e.MarginBounds.Left

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim PrintFont As System.Drawing.Font
        Dim SaltoLinea As Integer = 4
        Dim MIzq = 10
        Dim MTop = 20
        e.Graphics.PageUnit = GraphicsUnit.Millimeter
        'Titulos.

        PrintFont = New Font("Courier New", 9)
        Dim Lineas As Integer = 0
        Dim LineasPorPagina As Integer = 10
        Dim TotalRegistros As Integer = DtListado.Rows.Count

        y = MTop + 10

        Dim Count As Integer = 0
        Dim LineasPoPagina As Integer = 60
        Dim LineaNombre As Integer = MIzq
        Dim LineaTipoOperacion As Integer = MIzq + 57
        Dim LineaCuit As Integer = MIzq + 85

        While Count < LineasPoPagina And I < DtListado.Rows.Count
            Dim row As DataRow = DtListado.Rows(I)
            y = y + SaltoLinea
            x = LineaNombre
            e.Graphics.DrawString(row("Nombre"), PrintFont, Brushes.Black, x, y)
            x = LineaTipoOperacion
            Texto = ""
            If row("TipoOperacion") = 1 Then Texto = "Consignación"
            If row("TipoOperacion") = 2 Then Texto = "Reventa"
            If row("TipoOperacion") = 3 Then Texto = "Reventa MG"
            If row("TipoOperacion") = 4 Then Texto = "Costeo"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            x = LineaCuit
            e.Graphics.DrawString(Format(row("Cuit"), "00-00000000-0"), PrintFont, Brushes.Black, x, y)
            Count = Count + 1
            I = I + 1
        End While

        If I < DtListado.Rows.Count Then
            e.HasMorePages = True
        Else : e.HasMorePages = False
            I = 0
        End If

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim Dt As New DataTable
        Dt = Tablas.Leer("SELECT * FROM Proveedores;")
        TablaAExcel(Dt, "Titulo1", "Titulo2", "Titulo3")
        Dt.Dispose()

    End Sub
End Class