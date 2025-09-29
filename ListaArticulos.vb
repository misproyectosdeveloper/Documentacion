'http://www.recursosvisualbasic.com.ar/htm/vb-net/38-buscar-un-nodo-en-treeview.htm
' sobre treeview: http://www.syncfusion.com/FAQ/windowsforms/faq_c91c.aspx#q1097q
Imports System.data
Imports System.Data.OleDb
Public Class ListaArticulos
    Dim Dt As DataTable
    Dim DaP As OleDb.OleDbDataAdapter
    Dim MiConexion As OleDb.OleDbConnection = New OleDb.OleDbConnection(Conexion)
    Private Sub Articulos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Dim NodoRaiz As New TreeNode
        NodoRaiz.Name = "Articulos"
        NodoRaiz.Text = "Articulos"
        TreeView1.Nodes.Add(NodoRaiz)
        Llena_Nivel_1(NodoRaiz)
        '   TreeView1.Nodes("Articulos").Expand()

        NodoRaiz = New TreeNode
        NodoRaiz.Name = "Servicios"
        NodoRaiz.Text = "Servicios"
        TreeView1.Nodes.Add(NodoRaiz)
        Llena_Nivel_1_Servicios(NodoRaiz)

        NodoRaiz = New TreeNode
        NodoRaiz.Name = "Secos"
        NodoRaiz.Text = "Secos"
        '  TreeView1.Nodes.Add(NodoRaiz)
        '  Llena_Nivel_1_Secos(NodoRaiz)

    End Sub
    Private Sub TreeView1_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick

        If e.Button = MouseButtons.Right Then
            ArmaPath(e)
            If e.Node.Level = 5 Then Exit Sub
            If e.Node.Level = 1 Then
                If e.Node.Parent.Name = "Servicios" Or e.Node.Parent.Name = "Secos" Then Exit Sub
            End If
            Dim ClickPoint As Point = New Point(e.X, e.Y)
            Dim ClickNode As TreeNode = TreeView1.GetNodeAt(ClickPoint)
            If ClickNode Is Nothing Then
                Return
            End If
            ' Convert from Tree coordinates to Screen coordinates 
            Dim ScreenPoint As Point = TreeView1.PointToScreen(ClickPoint)
            ' Convert from Screen coordinates to Form coordinates 
            Dim FormPoint As Point = Me.PointToClient(ScreenPoint)
            ' Show context menu 
            Dim item1 As New ToolStripMenuItem("Alta de articulo")
            AddHandler item1.Click, AddressOf AltaDeArticuloClicked
            ContextMenuStrip1.Items.Clear()
            ContextMenuStrip1.Items.Add(item1)
            ContextMenuStrip1.Show(Me, FormPoint)
        End If

    End Sub
    Private Sub TreeView1_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseDoubleClick

        Dim NodoRaiz As New TreeNode
        NodoRaiz = HallaNodoRaiz(e.Node)

        If e.Button = MouseButtons.Left Then
            If NodoRaiz.Name = "Servicios" Then
                If e.Node.Level <> 1 Then
                    e.Node.ExpandAll()
                Else
                    UnArticulo.PEsServicios = True
                    UnArticulo.PClave = e.Node.Name
                    UnArticulo.ShowDialog()
                    If GModificacionOk Then ReArmaArbolServicios()
                End If
            Else
                If NodoRaiz.Name = "Secos" Then
                    If e.Node.Level <> 1 Then
                        e.Node.ExpandAll()
                    Else
                        UnArticulo.PEssecos = True
                        UnArticulo.PClave = e.Node.Name
                        UnArticulo.ShowDialog()
                        If GModificacionOk Then ReArmaArbolSecos()
                    End If
                Else
                    If e.Node.Level <> 5 Then
                        e.Node.ExpandAll()
                    Else
                        ArmaPath(e)
                        UnArticulo.ShowDialog()
                        If GModificacionOk Then ReArmaArbol()
                    End If
                End If
            End If
        End If

    End Sub
    Sub AltaDeArticuloClicked(ByVal sender As Object, ByVal e As EventArgs)

        Dim NodoRaiz As New TreeNode
        NodoRaiz = HallaNodoRaiz(TreeView1.SelectedNode)

        If NodoRaiz.Name = "Servicios" Then
            UnArticulo.PEsServicios = True
        End If
        If NodoRaiz.Name = "Secos" Then
            UnArticulo.PEsSecos = True
        End If
        UnArticulo.PClave = 0
        UnArticulo.ShowDialog()
        If GModificacionOk Then
            If NodoRaiz.Name = "Servicios" Then
                ReArmaArbolServicios()
            Else
                If NodoRaiz.Name = "Secos" Then
                    ReArmaArbolSecos()
                Else
                    ReArmaArbol()
                End If
            End If
            TreeView1.SelectedNode.Expand()
        End If

    End Sub
    Private Sub ButtonListaAlfaveticamentecial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonListaAlfaveticamentecial.Click

        Select Case TreeView1.SelectedNode.Text
            Case "Articulos"
                ListaArticulosSecuencial.PEsConStock = True
            Case "Servicios"
                ListaArticulosSecuencial.PEsServicios = True
            Case "Secos"
                ListaArticulosSecuencial.PEsSecos = True
            Case Else
                MsgBox("Debe Elegir un Nodo Raiz.")
                Exit Sub
        End Select

        Me.Close()

        ListaArticulosSecuencial.ShowDialog()

    End Sub
    Private Sub ReArmaArbol()

        Dim NodoPadre As New TreeNode

        If TreeView1.SelectedNode.Level = 5 Then
            NodoPadre = TreeView1.SelectedNode.Parent
        Else : NodoPadre = TreeView1.SelectedNode
        End If

        NodoPadre.Nodes.Clear()
        If NodoPadre.Level = 0 Then
            Llena_Nivel_1(NodoPadre)
        End If
        If NodoPadre.Level = 1 Then
            Llena_Nivel_2(NodoPadre)
        End If
        If NodoPadre.Level = 2 Then
            Llena_Nivel_3(NodoPadre)
        End If
        If NodoPadre.Level = 3 Then
            Llena_Nivel_4(NodoPadre)
        End If
        If NodoPadre.Level = 4 Then
            Llena_Nivel_5(NodoPadre)
        End If

    End Sub
    Private Sub ReArmaArbolServicios()

        Dim NodoRaiz As New TreeNode
        NodoRaiz = HallaNodoRaiz(TreeView1.SelectedNode)

        NodoRaiz.Nodes.Clear()
        Llena_Nivel_1_Servicios(NodoRaiz)

    End Sub
    Private Sub ReArmaArbolSecos()

        Dim NodoRaiz As New TreeNode
        NodoRaiz = HallaNodoRaiz(TreeView1.SelectedNode)

        NodoRaiz.Nodes.Clear()
        Llena_Nivel_1_Secos(NodoRaiz)

    End Sub
    Private Function Llena_Nivel_1_Servicios(ByVal Padre As TreeNode) As Boolean

        Dim dt As New DataTable
        Dim Sql As String = ""
        Dim Fila As DataRow

        Sql = "SELECT Clave,Nombre FROM ArticulosServicios WHERE Estado = 1 AND Secos = 0 ORDER BY Nombre;"
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
    Private Function Llena_Nivel_1_Secos(ByVal Padre As TreeNode) As Boolean

        Dim dt As New DataTable
        Dim Sql As String = ""
        Dim Fila As DataRow

        Sql = "SELECT Clave,Nombre FROM ArticulosServicios WHERE Estado = 1 AND Secos = 1 ORDER BY Nombre;"
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
    Private Function Llena_Nivel_1(ByVal Padre As TreeNode) As Boolean

        Dim nodo As New TreeNode

        Dim dt As New DataTable
        Dim Sql As String = ""
        Dim Fila As DataRow
        Dim Anterior As String = ""

        Sql = "SELECT Especie,Tablas.Nombre as Nombre FROM Articulos INNER JOIN Tablas ON Articulos.Especie = Tablas.Clave " & _
                     "AND Tablas.Tipo = 1 ORDER BY Tablas.Nombre;"
        If Not LLenaDt(dt, Sql) Then Me.Close() : Exit Function
        If dt.Rows.Count = 0 Then Exit Function
        For Each Fila In dt.Rows
            If Anterior <> Fila.Item("Nombre") Then
                nodo = New TreeNode
                nodo.Text = Fila.Item("Nombre")
                nodo.Name = Fila.Item("Especie")
                Padre.Nodes.Add(nodo)
                Call Llena_Nivel_2(nodo)
                Anterior = Fila.Item("Nombre")
            End If
        Next
        dt.Dispose()

    End Function
    Private Function Llena_Nivel_2(ByVal Padre As TreeNode) As Boolean

        Dim dt As New DataTable
        Dim Sql As String = ""
        Dim Fila As DataRow
        Dim Anterior As String = ""

        Sql = "SELECT Variedad,Tablas.Nombre as Nombre FROM Articulos INNER JOIN Tablas ON Articulos.Variedad = Tablas.Clave " & _
              "AND Tipo = 2 WHERE Especie = " & Val(Padre.Name) & " ORDER BY Tablas.Nombre;"
        If Not LLenaDt(dt, Sql) Then Me.Close() : Exit Function
        If dt.Rows.Count = 0 Then Exit Function
        For Each Fila In dt.Rows
            If Anterior <> Fila.Item("Nombre") Then
                Dim nodo As New TreeNode
                nodo.Text = Fila.Item("Nombre")
                nodo.Name = Fila.Item("Variedad")
                Padre.Nodes.Add(nodo)
                Call Llena_Nivel_3(nodo)
                Anterior = Fila.Item("Nombre")
            End If
        Next
        dt.Dispose()

    End Function
    Private Function Llena_Nivel_3(ByVal Padre As TreeNode) As Boolean

        Dim dt As New DataTable
        Dim Sql As String = ""
        Dim Fila As DataRow
        Dim Anterior As String = ""

        Sql = "SELECT Marca, Tablas.Nombre as Nombre FROM Articulos INNER JOIN Tablas ON Articulos.Marca = Tablas.Clave " & _
              "AND TIPO = 3 WHERE Variedad = " & Val(Padre.Name) & " AND Especie = " & Val(Padre.Parent.Name) & " ORDER BY Tablas.Nombre;"
        If Not LLenaDt(dt, Sql) Then Me.Close() : Exit Function
        If dt.Rows.Count = 0 Then Exit Function
        For Each Fila In dt.Rows
            If Anterior <> Fila.Item("Nombre") Then
                Dim nodo As New TreeNode
                nodo.Text = Fila.Item("Nombre")
                nodo.Name = Fila.Item("Marca")
                Padre.Nodes.Add(nodo)
                Call Llena_Nivel_4(nodo)
                Anterior = Fila.Item("Nombre")
            End If
        Next
        dt.Dispose()

    End Function
    Private Function Llena_Nivel_4(ByVal Padre As TreeNode) As Boolean

        Dim dt As New DataTable
        Dim Sql As String = ""
        Dim Fila As DataRow
        Dim Anterior As String = ""

        Sql = "SELECT Categoria,Tablas.Nombre as Nombre FROM Articulos INNER JOIN Tablas ON Articulos.Categoria = Tablas.Clave " & _
              "AND Tipo = 4 WHERE Marca = " & Val(Padre.Name) & " AND Variedad = " & Val(Padre.Parent.Name) & " AND Especie = " & Val(Padre.Parent.Parent.Name) & " ORDER BY Tablas.Nombre;"
        If Not LLenaDt(dt, Sql) Then Me.Close() : Exit Function
        If dt.Rows.Count = 0 Then Exit Function
        For Each Fila In dt.Rows
            If Anterior <> Fila.Item("Nombre") Then
                Dim nodo As New TreeNode
                nodo.Text = Fila.Item("Nombre")
                nodo.Name = Fila.Item("Categoria")
                Padre.Nodes.Add(nodo)
                Call Llena_Nivel_5(nodo)
                Anterior = Fila.Item("Nombre")
            End If
        Next
        dt.Dispose()

    End Function
    Private Function Llena_Nivel_5(ByVal Padre As TreeNode) As Boolean

        Dim dt As New DataTable
        Dim Sql As String = ""
        Dim Fila As DataRow
        '        Dim Anterior As String = ""
        Dim Anterior As Integer = 0     'Para colgar articulo en envase uso clave del envase, pues nombre puede ser repetido. 

        Sql = "SELECT Articulos.Nombre as NombreArticulo,Envase,Envases.Nombre as Nombre FROM Articulos INNER JOIN Envases ON Articulos.Envase = Envases.Clave " & _
              "WHERE Categoria = " & Val(Padre.Name) & " AND Marca = " & Val(Padre.Parent.Name) & " AND Variedad = " & Val(Padre.Parent.Parent.Name) & _
              " AND Especie = " & Val(Padre.Parent.Parent.Parent.Name) & " AND Estado = 1 ORDER BY Envases.Nombre;"
        If Not LLenaDt(dt, Sql) Then Me.Close() : Exit Function
        If dt.Rows.Count = 0 Then Exit Function
        For Each Fila In dt.Rows
            If Anterior <> Fila.Item("Envase") Then
                Dim nodo As New TreeNode
                nodo.Text = Fila.Item("Nombre") & " - " & Fila.Item("NombreArticulo")
                nodo.Name = Fila.Item("Envase")
                Padre.Nodes.Add(nodo)
                Anterior = Fila.Item("Envase")
            End If
        Next
        dt.Dispose()

    End Function
    Private Function LLenaDt(ByRef Dt As DataTable, ByVal Sql As String) As Boolean

        Try
            Dt.Clear()
            MiConexion.Open()
            DaP = New OleDb.OleDbDataAdapter(Sql, MiConexion)
            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
            ' DaP.FillSchema(Dt, SchemaType.Source)
            DaP.Fill(Dt)
            MiConexion.Close()
            Return True
        Catch err As OleDb.OleDbException
            MsgBox(err.Message)
        Finally
            If MiConexion.State = ConnectionState.Open Then MiConexion.Close()
        End Try

    End Function
    Sub ArmaPath(ByVal Nodo As System.Windows.Forms.TreeNodeMouseClickEventArgs)

        If Nodo.Node.Level = 5 Then
            GEnvase = Val(Nodo.Node.Name)
            GCategoria = Val(Nodo.Node.Parent.Name)
            GMarca = Val(Nodo.Node.Parent.Parent.Name)
            GVariedad = Val(Nodo.Node.Parent.Parent.Parent.Name)
            GEspecie = Val(Nodo.Node.Parent.Parent.Parent.Parent.Name)
        End If
        If Nodo.Node.Level = 4 Then
            GEnvase = 0
            GCategoria = Val(Nodo.Node.Name)
            GMarca = Val(Nodo.Node.Parent.Name)
            GVariedad = Val(Nodo.Node.Parent.Parent.Name)
            GEspecie = Val(Nodo.Node.Parent.Parent.Parent.Name)
        End If
        If Nodo.Node.Level = 3 Then
            GEnvase = 0
            GCategoria = 0
            GMarca = Val(Nodo.Node.Name)
            GVariedad = Val(Nodo.Node.Parent.Name)
            GEspecie = Val(Nodo.Node.Parent.Parent.Name)
        End If
        If Nodo.Node.Level = 2 Then
            GEnvase = 0
            GCategoria = 0
            GMarca = 0
            GVariedad = Val(Nodo.Node.Name)
            GEspecie = Val(Nodo.Node.Parent.Name)
        End If
        If Nodo.Node.Level = 1 Then
            GEnvase = 0
            GCategoria = 0
            GMarca = 0
            GVariedad = 0
            GEspecie = Val(Nodo.Node.Name)
        End If
        If Nodo.Node.Level = 0 Then
            GEnvase = 0
            GCategoria = 0
            GMarca = 0
            GVariedad = 0
            GEspecie = 0
        End If

    End Sub
    Private Function HallaNodoRaiz(ByVal Nodo As TreeNode) As TreeNode

        If TreeView1.SelectedNode.Level = 0 Then Return TreeView1.SelectedNode
        If TreeView1.SelectedNode.Level = 1 Then Return TreeView1.SelectedNode.Parent
        If TreeView1.SelectedNode.Level = 2 Then Return TreeView1.SelectedNode.Parent.Parent
        If TreeView1.SelectedNode.Level = 3 Then Return TreeView1.SelectedNode.Parent.Parent.Parent
        If TreeView1.SelectedNode.Level = 4 Then Return TreeView1.SelectedNode.Parent.Parent.Parent.Parent
        If TreeView1.SelectedNode.Level = 5 Then Return TreeView1.SelectedNode.Parent.Parent.Parent.Parent.Parent

    End Function

  
End Class