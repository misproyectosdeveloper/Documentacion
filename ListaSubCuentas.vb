Public Class ListaSubCuentas
    Public PBloqueaFunciones As Boolean
    Public PSubCuenta As Integer
    Public PNombreCuenta As String
    Public PNombreSubCuenta As String
    Public PCentro As Integer
    ' 
    Dim CuentaAnt As Integer
    Dim SubCuentaAnt As Integer
    Dim NodoCuenta As TreeNode
    Dim NodoSubCuenta As TreeNode
    Dim Funcion As Integer
    Private Sub ListaCuentasYSubCuentas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(11) Then PBloqueaFunciones = True

        Me.Top = 50

        ArmaArbol()

        TreeView1.SelectedNode = TreeView1.Nodes(0)
        MuestraNodo(TreeView1.Nodes(0))

    End Sub
    Private Sub ListaCuentasYSubCuentas_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        TreeView1.Focus()

    End Sub
    Private Sub ListaCuentasYSubCuentas_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Nodo As TreeNode = TreeView1.SelectedNode
        Dim NodoPadre As TreeNode = Nodo.Parent

        If Nodo.Level = 0 Then Exit Sub

        If Not TextCuenta.ReadOnly Or Not TextSubCuenta.ReadOnly Then Exit Sub

        Dim I As Integer
        If Nodo.Level = 1 Then
            For Each tn As TreeNode In Nodo.Nodes
                I = I + 1
            Next
            If I > 0 Then
                MsgBox("Cuenta Tiene Sub-Cuentas Asociadas. Operación se CANCELA.")
                Exit Sub
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Nodo.Level = 1 Then
            If MsgBox("Cuenta Se Borrar Definitivamente. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
            If BorraCuenta() Then
                TreeView1.Nodes.Remove(Nodo)
            End If
        End If

        If Nodo.Level = 2 Then
            Dim Clave As Integer = CInt(Val(TextCuenta.Text) & Format(Val(TextSubCuenta.Text), "00"))
            If ExisteEnPlanDeCuenta(Clave) <> 0 Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Cuenta Utilizada en Plan de Cuentas.")
                Exit Sub
            End If
            If MsgBox("Cuenta Se Borrar Definitivamente. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
            If BorraSubCuenta() Then
                TreeView1.Nodes.Remove(Nodo)
            End If
        End If

        MuestraNodo(NodoPadre)

        Nodo = Nothing
        NodoPadre = Nothing

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonGraba_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonGraba.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Nodo As TreeNode = TreeView1.SelectedNode

        TextNombreCuenta.Text = TextNombreCuenta.Text.Trim
        TextNombreSubCuenta.Text = TextNombreSubCuenta.Text.Trim
        TextComentario.Text = TextComentario.Text.Trim

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Funcion = 1 Then
            HacerAltaCuenta()
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If
        If Funcion = 2 Then
            HacerAltaSubCuenta()
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If
        If Funcion = 0 And Nodo.Level = 1 Then
            HacerModificacionCuenta()
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If
        If Funcion = 0 And Nodo.Level = 2 Then
            HacerModificacionSubCuenta()
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonNuevaCuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevaCuenta.Click

        Funcion = 1

        Dim Nodo As TreeNode = TreeView1.SelectedNode

        TextComentario.Text = ""
        TextComentario.ReadOnly = False
        PanelCuenta.Visible = True

        TextCuenta.ReadOnly = False
        TextNombreCuenta.ReadOnly = False
        PanelSubCuenta.Visible = False

    End Sub
    Private Sub ButtonNuevaSubCuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevaSubCuenta.Click

        Funcion = 2

        Dim Nodo As TreeNode = TreeView1.SelectedNode

        TextComentario.Text = ""
        TextComentario.ReadOnly = False
        PanelCuenta.Visible = True

        TextCuenta.ReadOnly = True
        TextNombreCuenta.ReadOnly = True
        TextSubCuenta.ReadOnly = False
        TextNombreSubCuenta.ReadOnly = False
        PanelSubCuenta.Visible = True

    End Sub
    Private Sub TreeView1_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick

        MuestraNodo(e.Node)

    End Sub
    Private Sub TreeView1_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseDoubleClick

        e.Node.ExpandAll()

    End Sub
    Private Sub TextCuenta_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCuenta.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextSubCuenta_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSubCuenta.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub HacerAltaCuenta()

        If TextCuenta.Text = "" Then
            MsgBox("Falta Cuenta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Val(TextCuenta.Text) = 0 Then
            MsgBox("Falta Cuenta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If TextNombreCuenta.Text = "" Then
            MsgBox("Falta Nombre de la Cuenta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtCuenta As New DataTable
        If Not Tablas.Read("SELECT * FROM Cuentas WHERE Cuenta = " & Val(TextCuenta.Text) & ";", Conexion, DtCuenta) Then Me.Close() : Exit Sub
        If DtCuenta.Rows.Count <> 0 Then
            MsgBox("Cuenta Ya existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DtCuenta.Dispose()
            Exit Sub
        End If

        Dim Row As DataRow = DtCuenta.NewRow
        Row("Cuenta") = Val(TextCuenta.Text)
        Row("Nombre") = TextNombreCuenta.Text
        DtCuenta.Rows.Add(Row)

        If ActualizaCuenta(DtCuenta) <= 0 Then
            DtCuenta.Dispose()
            Exit Sub
        End If

        Cuelga(Val(TextCuenta.Text), 0, TextNombreCuenta.Text, "")

        DtCuenta.Dispose()

        SeleccionaNodo(Val(TextCuenta.Text))

    End Sub
    Private Sub HacerAltaSubCuenta()

        If TextSubCuenta.Text = "" Then
            MsgBox("Falta Sub-Cuenta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Val(TextCuenta.Text) = 0 Then
            MsgBox("Falta Sub-Cuenta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If TextNombreSubCuenta.Text = "" Then
            MsgBox("Falta Nombre de la Sub-Cuenta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim SubCuenta As Integer = CInt(TextCuenta.Text & Format(Val(TextSubCuenta.Text), "00"))

        Dim DtSubCuenta As New DataTable
        If Not Tablas.Read("SELECT * FROM SubCuentas WHERE Clave = " & SubCuenta & ";", Conexion, DtSubCuenta) Then Me.Close() : Exit Sub
        If DtSubCuenta.Rows.Count <> 0 Then
            MsgBox("Sub-Cuenta Ya existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DtSubCuenta.Dispose()
            Exit Sub
        End If
        Dim Row As DataRow = DtSubCuenta.NewRow
        Row("Clave") = SubCuenta
        Row("Cuenta") = Val(TextCuenta.Text)
        Row("Nombre") = TextNombreSubCuenta.Text
        Row("Comentario") = TextComentario.Text
        DtSubCuenta.Rows.Add(Row)

        If ActualizaSubCuenta(DtSubCuenta) <= 0 Then
            DtSubCuenta.Dispose()
            Exit Sub
        End If

        Cuelga(Val(TextCuenta.Text), SubCuenta, TextNombreCuenta.Text, TextNombreSubCuenta.Text)

        DtSubCuenta.Dispose()

        SeleccionaNodo(SubCuenta)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function BorraCuenta() As Boolean

        Dim DtCuenta As New DataTable
        If Not Tablas.Read("SELECT * FROM Cuentas WHERE Cuenta = " & Val(TextCuenta.Text) & ";", Conexion, DtCuenta) Then Me.Close()
        If DtCuenta.Rows.Count = 0 Then
            MsgBox("Cuenta Ya No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DtCuenta.Dispose()
            Return False
        End If
        DtCuenta.Rows(0).Delete()

        If ActualizaCuenta(DtCuenta) <= 0 Then
            DtCuenta.Dispose()
            Return False
        End If

        DtCuenta.Dispose()

        Return True

    End Function
    Private Function BorraSubCuenta() As Boolean

        Dim Clave As Integer = CInt(Val(TextCuenta.Text) & Format(Val(TextSubCuenta.Text), "00"))

        Dim DtSubCuenta As New DataTable
        If Not Tablas.Read("SELECT * FROM SubCuentas WHERE Clave = " & Clave & ";", Conexion, DtSubCuenta) Then Me.Close()
        If DtSubCuenta.Rows.Count = 0 Then
            MsgBox("Sub-Cuenta Ya No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DtSubCuenta.Dispose()
            Return False
        End If
        DtSubCuenta.Rows(0).Delete()

        If ActualizaSubCuenta(DtSubCuenta) <= 0 Then
            DtSubCuenta.Dispose()
            Return False
        End If

        DtSubCuenta.Dispose()

        Return True

    End Function
    Private Sub HacerModificacionCuenta()

        If TextNombreCuenta.Text = "" Then
            MsgBox("Falta Nombre de la Cuenta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT * FROM Cuentas WHERE Cuenta = " & Val(TextCuenta.Text) & ";", Conexion, Dt) Then Me.Close() : Exit Sub
        If Dt.Rows.Count = 0 Then
            MsgBox("Cuenta no Existe en Base de Datos.")
            Dt.Dispose()
            Exit Sub
        End If

        Dt.Rows(0).Item("Nombre") = TextNombreCuenta.Text

        If ActualizaCuenta(Dt) <= 0 Then Dt.Dispose() : Exit Sub

        Dim tvn() As TreeNode = TreeView1.Nodes.Find(Val(TextCuenta.Text), True)
        If tvn IsNot Nothing AndAlso tvn.Length > 0 Then
            For I As Integer = 0 To tvn.Length - 1
                tvn(I).Text = Format(Val(TextCuenta.Text), "000000") & "-" & TextNombreCuenta.Text : tvn = Nothing : Exit For
            Next
        End If

        Dt.Dispose()

    End Sub
    Private Sub HacerModificacionSubCuenta()

        Dim Dt As New DataTable
        Dim Clave As Integer = CInt(Val(TextCuenta.Text) & Format(Val(TextSubCuenta.Text), "00"))

        If Not Tablas.Read("SELECT * FROM SubCuentas WHERE Clave = " & Clave & ";", Conexion, Dt) Then Me.Close() : Exit Sub
        If Dt.Rows.Count = 0 Then
            MsgBox("Sub Cuenta no Existe en Base de Datos.")
            Dt.Dispose()
            Exit Sub
        End If

        Dt.Rows(0).Item("Nombre") = TextNombreSubCuenta.Text
        Dt.Rows(0).Item("Comentario") = TextComentario.Text

        If ActualizaSubCuenta(Dt) <= 0 Then Dt.Dispose() : Exit Sub

        Dim tvn() As TreeNode = TreeView1.Nodes.Find(Clave, True)
        If tvn IsNot Nothing AndAlso tvn.Length > 0 Then
            For I As Integer = 0 To tvn.Length - 1
                tvn(I).Text = Strings.Right(Clave, 2) & "-" & TextNombreSubCuenta.Text : tvn = Nothing : Exit For
            Next
        End If

        Dt.Dispose()

    End Sub
    Private Function ActualizaCuenta(ByVal DtCuenta As DataTable) As Integer

        If IsNothing(DtCuenta.GetChanges) Then Return True

        Using MiConexion As New OleDb.OleDbConnection(Conexion)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Try
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Cuentas;", MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtCuenta.GetChanges)
                    End Using
                    MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return 1000
                Catch ex As OleDb.OleDbException
                    If ex.ErrorCode = GAltaExiste Then
                        MsgBox("Cuenta o Nombre de Cuenta Ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Return -1
                    Else
                        Return -2
                        MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    End If
                Catch ex As DBConcurrencyException
                    MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return 0
                Finally
                End Try
            Catch ex As Exception
                MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return -2
            End Try
        End Using

    End Function
    Private Function ActualizaSubCuenta(ByVal DtSubCuenta As DataTable) As Integer

        If IsNothing(DtSubCuenta.GetChanges) Then Return True

        Using MiConexion As New OleDb.OleDbConnection(Conexion)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Try
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM SubCuentas;", MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtSubCuenta.GetChanges)
                    End Using
                    MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return 1000
                Catch ex As OleDb.OleDbException
                    If ex.ErrorCode = GAltaExiste Then
                        MsgBox("Sub-Cuenta o Nombre de Sub-Cuenta Ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Return -1
                    Else
                        MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Return -2
                    End If
                Catch ex As DBConcurrencyException
                    MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return 0
                Finally
                End Try
            Catch ex As Exception
                MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return -2
            End Try
        End Using

    End Function
    Private Sub ArmaArbol()

        Dim NodoRaiz As New TreeNode

        TreeView1.Nodes.Clear()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        NodoRaiz.Name = "Cuentas"
        NodoRaiz.Text = "Sub-Cuentas"
        TreeView1.Nodes.Add(NodoRaiz)

        If Not Nivel_1(NodoRaiz) Then Me.Close() : Exit Sub

        TreeView1.Nodes("Cuentas").Expand()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function Nivel_1(ByVal Padre As TreeNode) As Boolean
        'Cuelga Cuenta.

        Dim Dt As New DataTable

        If PCentro = 0 Then
            If Not Tablas.Read("SELECT * FROM Cuentas ORDER BY Cuenta;", Conexion, Dt) Then Return False
        Else
            If Not Tablas.Read("SELECT DISTINCT C.Cuenta,C.Nombre FROM Cuentas AS C INNER JOIN PlanDeCuentas AS P ON C.Cuenta = P.Cuenta WHERE P.Centro = " & PCentro & " ORDER BY C.Cuenta;", Conexion, Dt) Then Return False
        End If

        For Each Row As DataRow In Dt.Rows
            Dim NodoCuenta As New TreeNode
            NodoCuenta.Text = Format(Row("Cuenta"), "000000") & "-" & Row("Nombre")
            NodoCuenta.Name = Row("Cuenta")
            Padre.Nodes.Add(NodoCuenta)
            If Not Nivel_2(NodoCuenta, Row("Cuenta")) Then Dt.Dispose() : Return False
        Next

        Dt.Dispose()

        Return True

    End Function
    Private Function Nivel_2(ByVal Padre As TreeNode, ByVal Cuenta As Integer) As Boolean
        'Cuelga SubCuenta.

        Dim Dt As New DataTable

        If PCentro = 0 Then
            If Not Tablas.Read("SELECT Clave,Nombre FROM SubCuentas WHERE Cuenta = " & Cuenta & " ORDER BY Clave;", Conexion, Dt) Then Return False
        Else
            If Not Tablas.Read("SELECT DISTINCT S.Clave,S.Nombre FROM SubCuentas AS S INNER JOIN PlanDeCuentas AS P ON S.Clave = P.SubCuenta AND P.Cuenta = " & Cuenta & " AND P.Centro = " & PCentro & " ORDER BY S.Clave;", Conexion, Dt) Then Return False
        End If

        If Dt.Rows.Count = 0 Then Dt.Dispose() : Return True

        For Each Row As DataRow In Dt.Rows
            Dim NodoSubCuenta As New TreeNode
            NodoSubCuenta.Text = Strings.Right(Row("Clave"), 2) & "-" & Row("Nombre")
            NodoSubCuenta.Name = Row("Clave")
            Padre.Nodes.Add(NodoSubCuenta)
        Next

        Dt.Dispose()

        Return True

    End Function
    Private Sub MuestraNodo(ByVal Nodo As TreeNode)

        Funcion = 0

        TextCuenta.Text = ""
        TextNombreCuenta.Text = ""
        TextSubCuenta.Text = ""
        TextNombreSubCuenta.Text = ""
        TextComentario.Text = ""
        '
        TextCuenta.ReadOnly = True
        TextNombreCuenta.ReadOnly = True
        TextSubCuenta.ReadOnly = True
        TextNombreSubCuenta.ReadOnly = True
        TextComentario.ReadOnly = True

        PanelCuenta.Visible = True
        PanelSubCuenta.Visible = True
        ButtonNuevaCuenta.Visible = True
        ButtonNuevaSubCuenta.Visible = True

        If Nodo.Level = 0 Then
            ButtonNuevaCuenta.Visible = True
            PanelCuenta.Visible = False
            ButtonNuevaSubCuenta.Visible = False
        End If
        If Nodo.Level = 1 Then
            TextCuenta.Text = Format(Val(Nodo.Name), "000000")
            TextNombreCuenta.Text = Mid$(Nodo.Text, 8, 30)
            TextNombreCuenta.ReadOnly = False
            PanelSubCuenta.Visible = False
            ButtonNuevaCuenta.Visible = False
        End If
        If Nodo.Level = 2 Then
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            TextCuenta.Text = Format(Val(Nodo.Parent.Name), "000000")
            TextNombreCuenta.Text = Mid$(Nodo.Parent.Text, 8, 30)
            TextSubCuenta.Text = Format(Val(Strings.Right(Nodo.Name, 2)), "00")
            TextNombreSubCuenta.Text = Mid$(Nodo.Text, 4, 30)
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT Comentario FROM SubCuentas WHERE Clave = " & Nodo.Name & ";", Conexion, Dt) Then Me.Close() : Exit Sub
            If Dt.Rows.Count = 0 Then
                MsgBox("Sub-Cuenta Ya No Existe en la Base de Datos.")
                Dt.Dispose()
                Me.Close() : Exit Sub
            End If
            TextComentario.Text = Dt.Rows(0).Item("Comentario")
            TextNombreSubCuenta.ReadOnly = False
            TextComentario.ReadOnly = False
            ButtonNuevaCuenta.Visible = False
            ButtonNuevaSubCuenta.Visible = False
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End If

    End Sub
    Private Sub Cuelga(ByVal Cuenta As Integer, ByVal SubCuenta As Integer, ByVal NombreCuenta As String, ByVal NombreSubCuenta As String)

        Dim NodoRaiz As New TreeNode

        NodoRaiz = TreeView1.Nodes("Cuentas")

        If SubCuenta = 0 Then
            Dim NodoCuenta As New TreeNode
            NodoCuenta.Name = Cuenta
            NodoCuenta.Text = Format(Cuenta, "000000") & "-" & NombreCuenta
            NodoRaiz.Nodes.Add(NodoCuenta)
            Exit Sub
        End If

        For Each Nodo As TreeNode In NodoRaiz.Nodes
            If Nodo.Name = Cuenta Then
                Dim NodoSubCuenta As New TreeNode
                NodoSubCuenta.Name = SubCuenta
                NodoSubCuenta.Text = Strings.Right(SubCuenta, 2) & "-" & NombreSubCuenta
                Nodo.Nodes.Add(NodoSubCuenta)
                Exit Sub
            End If
        Next

    End Sub
    Private Sub CuelgaBack(ByVal Cuenta As Integer, ByVal SubCuenta As Integer, ByVal NombreCuenta As String, ByVal NombreSubCuenta As String)
        'lo guardo como ejemplo.

        Dim NodoRaiz As New TreeNode

        NodoRaiz = TreeView1.Nodes("Cuentas")

        Dim tvn() As TreeNode = NodoRaiz.Nodes.Find(Cuenta, True)
        If tvn IsNot Nothing AndAlso tvn.Length > 0 Then
            Dim NodoSubCuenta As New TreeNode
            NodoSubCuenta.Name = SubCuenta
            NodoSubCuenta.Text = "(" & Format(SubCuenta, "00") & ")-" & NombreSubCuenta
            tvn(0).Nodes.Add(NodoSubCuenta)
        Else
            Dim NodoCuenta As New TreeNode
            NodoCuenta.Name = Cuenta
            NodoCuenta.Text = "(" & Format(Cuenta, "000000") & ")-" & NombreCuenta
            NodoRaiz.Nodes.Add(NodoCuenta)
            Dim NodoSubCuenta As New TreeNode
            NodoSubCuenta.Name = SubCuenta
            NodoSubCuenta.Text = "(" & Format(SubCuenta, "00") & ")-" & NombreSubCuenta
            NodoCuenta.Nodes.Add(NodoSubCuenta)
        End If

    End Sub
    Private Sub SeleccionaNodo(ByVal Nodo As Integer)

        Dim tvn() As TreeNode = TreeView1.Nodes.Find(Nodo, True)
        If tvn IsNot Nothing AndAlso tvn.Length > 0 Then
            For I As Integer = 0 To tvn.Length - 1
                TreeView1.SelectedNode = tvn(I)
                MuestraNodo(tvn(I))
                '  tvn(i).Expand()
                tvn = Nothing
                Exit Sub
            Next
        End If

    End Sub
    Private Function ExisteEnPlanDeCuenta(ByVal SubCuenta As Integer) As Integer

        Dim Sql As String = "SELECT COUNT(ClaveCuenta) FROM PlanDeCuentas WHERE SubCuenta = " & SubCuenta & ";"
        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer PlanDeCuentas.")
            End
        End Try

    End Function

End Class