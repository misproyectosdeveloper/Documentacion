Public Class OpcionFondoFijo
    Public PFondoFijo As Integer
    Public PNumeroFondoFijo As Integer
    Public PAbierto As Boolean
    Public PRegresar As Boolean
    '
    Dim ConexionFondoFijo As String
    Private Sub OpcionFondoFijo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboFondoFijo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM MaestroFondoFijo;")
        Dim Row As DataRow = ComboFondoFijo.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboFondoFijo.DataSource.rows.add(Row)
        ComboFondoFijo.DisplayMember = "Nombre"
        ComboFondoFijo.ValueMember = "Clave"
        ComboFondoFijo.SelectedValue = 0
        With ComboFondoFijo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        PictureCandado.Image = ImageList1.Images.Item("Abierto")
        PAbierto = True
        ConexionFondoFijo = Conexion

        If Not PermisoTotal Then PictureCandado.Visible = False

        PRegresar = True

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        PFondoFijo = ComboFondoFijo.SelectedValue
        PNumeroFondoFijo = CInt(TextNumero.Text)

        PRegresar = False
        Me.Close()

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            PAbierto = False
            ConexionFondoFijo = ConexionN
        Else : PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
            ConexionFondoFijo = Conexion
        End If

    End Sub
    Private Sub ComboFondoFijo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboFondoFijo.Validating

        If IsNothing(ComboFondoFijo.SelectedValue) Then ComboFondoFijo.SelectedValue = 0

    End Sub
    Private Sub TextNumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumero.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, 0)

    End Sub
    Public Function ExisteFondoFijo() As Boolean

        Dim Sql As String = "SELECT COUNT(Numero) FROM FondosFijos WHERE FondoFijo = " & ComboFondoFijo.SelectedValue & " AND Numero = " & CInt(TextNumero.Text) & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionFondoFijo)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla Fondos Fijos.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function Valida() As Boolean

        If ComboFondoFijo.SelectedValue = 0 Then
            MsgBox("Debe Informar Fondo Fijo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboFondoFijo.Focus()
            Return False
        End If

        If TextNumero.Text = "" Then
            MsgBox("Debe Informar Numero del Fondo Fijo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNumero.Focus()
            Return False
        End If
        If CInt(TextNumero.Text) = 0 Then
            MsgBox("Debe Informar Numero del Fondo Fijo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNumero.Focus()
            Return False
        End If
        If Not ExisteFondoFijo() Then
            MsgBox("Numero del Fondo Fijo NO Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNumero.Focus()
            Return False
        End If

        Return True

    End Function

 
End Class