Public Class SeleccionarCuenta
    Public PEsSoloCuentas As Boolean
    Public PEsNivelCuenta As Boolean
    Public PCentro As Integer
    Public PCuenta As Double
    Public PCuentaStr As String
    Private Sub ListaCentrosYSubCuentas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If PEsSoloCuentas Then
            ArmaArbolSoloCuentas()
        Else : ArmaArbol()
        End If

        TreeView1.SelectedNode = TreeView1.Nodes(0)

        PCuenta = 0
        PCuentaStr = ""

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Dim Nodo As TreeNode = TreeView1.SelectedNode

        If PEsNivelCuenta Then
            If Nodo.Level = 0 Then PCuentaStr = "           "
            If Nodo.Level = 1 Then PCuentaStr = Format(Val(Nodo.Name), "000") & "        "
            If Nodo.Level = 2 Then PCuentaStr = Format(Val(Nodo.Parent.Name), "000") & Format(Val(Nodo.Name), "000000") & "  "
            If Nodo.Level = 3 Then PCuentaStr = Format(Val(Nodo.Parent.Parent.Name), "000") & Format(Val(Nodo.Name), "00000000")
            Me.Close()
            Exit Sub
        End If

        If PEsSoloCuentas Then
            If Nodo.Level <> 2 Then
                MsgBox("Selección Incorrecta.")
                Exit Sub
            End If
        Else
            If Nodo.Level <> 3 Then
                MsgBox("Selección Incorrecta.")
                Exit Sub
            End If
        End If

        PCuenta = Nodo.Name
        If Not PEsSoloCuentas Then
            PCuenta = Nodo.Parent.Parent.Name & Format(PCuenta, "00000000")
        End If

        Me.Close()

    End Sub
    Private Sub TreeView1_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseDoubleClick

        If PEsSoloCuentas Then
            If e.Node.Level <> 2 Then
                e.Node.ExpandAll()
            Else
                ButtonAceptar_Click(Nothing, Nothing)
            End If
        Else
            If e.Node.Level <> 3 Then
                e.Node.ExpandAll()
            Else
                ButtonAceptar_Click(Nothing, Nothing)
            End If
        End If

    End Sub
    Private Sub ArmaArbol()

        Dim NodoRaiz As New TreeNode

        TreeView1.Nodes.Clear()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        NodoRaiz.Name = "Cuentas"
        NodoRaiz.Text = "Centros de Costos"
        TreeView1.Nodes.Add(NodoRaiz)

        If Not Nivel_1Y2Y3(NodoRaiz) Then Me.Close() : Exit Sub

        TreeView1.Nodes("Cuentas").Expand()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ArmaArbolSoloCuentas()

        Dim NodoRaiz As New TreeNode

        TreeView1.Nodes.Clear()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        NodoRaiz.Name = "Cuentas"
        NodoRaiz.Text = "Cuentas"
        TreeView1.Nodes.Add(NodoRaiz)

        If Not Nivel_1Y2(NodoRaiz) Then Me.Close() : Exit Sub

        TreeView1.Nodes("Cuentas").Expand()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function Nivel_1Y2Y3(ByVal Padre As TreeNode) As Boolean

        Dim Dt As New DataTable
        Dim CentroAnt As Integer = 0
        Dim CuentaAnt As Integer = 0
        Dim NodoCentro As TreeNode
        Dim NodoCuenta As TreeNode
        Dim Sql As String

        If PCentro = 0 Then
            Sql = "SELECT Centro,Cuenta,SubCuenta FROM PlanDeCuentas ORDER BY ClaveCuenta;"
        Else
            Sql = "SELECT Centro,Cuenta,SubCuenta FROM PlanDeCuentas WHERE Centro = " & PCentro & " ORDER BY ClaveCuenta;"
        End If
        If Not Tablas.Read(Sql, Conexion, Dt) Then Return False
        For Each Row As DataRow In Dt.Rows
            If CentroAnt <> Row("Centro") Then
                NodoCentro = New TreeNode
                NodoCentro.Name = Row("Centro")
                NodoCentro.Text = Format(Row("Centro"), "000") & "-" & NombreCentro(Row("Centro"))
                Padre.Nodes.Add(NodoCentro)
                CentroAnt = Row("Centro")
                CuentaAnt = 0
            End If
            If CuentaAnt <> Row("Cuenta") Then
                NodoCuenta = New TreeNode
                NodoCuenta.Name = Row("Cuenta")
                NodoCuenta.Text = Format(Row("Cuenta"), "000000") & "-" & NombreCuenta(Row("Cuenta"))
                NodoCentro.Nodes.Add(NodoCuenta)
                CuentaAnt = Row("Cuenta")
            End If
            Dim NodoSubCuenta As New TreeNode
            NodoSubCuenta.Name = Row("SubCuenta")
            NodoSubCuenta.Text = Strings.Right(Row("SubCuenta"), 2) & "-" & NombreSubCuenta(Row("SubCuenta"))
            NodoCuenta.Nodes.Add(NodoSubCuenta)
        Next

        Dt.Dispose()

        Return True

    End Function
    Private Function Nivel_1Y2(ByVal Padre As TreeNode) As Boolean
        'Cuelga Cuenta.

        Dim Dt As New DataTable
        Dim CuentaAnt As Integer = 0
        Dim NodoCuenta As TreeNode

        Dim Sql As String = "SELECT Clave,Cuenta,Nombre FROM SubCuentas ORDER BY Clave;"
        If Not Tablas.Read(Sql, Conexion, Dt) Then Return False
        For Each Row As DataRow In Dt.Rows
            If CuentaAnt <> Row("Cuenta") Then
                NodoCuenta = New TreeNode
                NodoCuenta.Name = Row("Cuenta")
                NodoCuenta.Text = Format(Row("Cuenta"), "000000") & "-" & NombreCuenta(Row("Cuenta"))
                Padre.Nodes.Add(NodoCuenta)
                CuentaAnt = Row("Cuenta")
            End If
            Dim NodoSubCuenta As New TreeNode
            NodoSubCuenta.Name = Row("Clave")
            NodoSubCuenta.Text = Strings.Right(Row("Clave"), 2) & "-" & Row("Nombre")
            NodoCuenta.Nodes.Add(NodoSubCuenta)
        Next

        Dt.Dispose()

        Return True

    End Function

End Class