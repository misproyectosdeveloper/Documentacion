Public Class UnCambioPassword
    Public PPasswordActual As String
    Public PUsuario As String
    Public ConexionUsuario As String
    Dim PasswordB As String
    Dim PasswordN As String
    Private Sub UnCambioPassword_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        PasswordB = ""
        PasswordN = ""

        ConexionUsuario = Conexion
        If Not HallaPassword(PUsuario, PasswordB, ConexionUsuario) Then
            MsgBox("Error Base de Datos.")
            Me.Close()
            Exit Sub
        End If
        If PasswordB = "" Then
            MsgBox("Error Al Leer Tabla de Usuarios.")
            Me.Close()
            Exit Sub
        End If

        ConexionUsuario = ConexionN
        HallaPassword(PUsuario, PasswordN, ConexionUsuario)

        If PasswordB <> PPasswordActual And PasswordN <> PPasswordActual Then
            MsgBox("No Existe Usuario o Password Incorrecta.")
            Me.Close()
            Exit Sub
        End If

        If PasswordB = PPasswordActual Then
            ConexionUsuario = Conexion
        Else : ConexionUsuario = ConexionN
        End If

    End Sub
    Private Sub UnCambioPassword_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If Not GrabaPassword(PUsuario, Encriptar(PPasswordActual), Encriptar(TextPassword.Text), ConexionUsuario) Then
            MsgBox("No se pudo Grabar la Nueva Password.")
        Else
            Me.Close()
        End If

    End Sub
    Private Sub TextPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextPassword.KeyPress

        If e.KeyChar = " " Then e.KeyChar = ""

    End Sub
    Private Sub TextConfirmacion_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextConfirmacion.KeyPress

        If e.KeyChar = " " Then e.KeyChar = ""

    End Sub
    Private Function HallaPassword(ByVal Usuario As String, ByRef Password As String, ByVal ConexionStr As String) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Password FROM Usuarios WHERE Estado <> 3 AND Usuario = '" & Usuario & "';", Miconexion)
                    Password = Desencriptar(Cmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Return False
        End Try

        Return True

    End Function
    Private Function GrabaPassword(ByVal Usuario As String, ByVal PasswordAnterior As String, ByVal Password As String, ByVal ConexionStr As String) As Boolean

        Dim Sql As String = "UPDATE " & "Usuarios" & _
            " Set Password = '" & Password & "' WHERE Usuario = '" & Usuario & "' AND Estado <> 3 And Password = '" & PasswordAnterior & "';"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return False
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try

        Return True

    End Function
    Private Function Valida() As Boolean

        TextPassword.Text = TextPassword.Text.Trim
        TextConfirmacion.Text = TextConfirmacion.Text.Trim

        If TextPassword.Text = "" Then
            MsgBox("Falta Informar Password")
            TextPassword.Focus()
            Return False
        End If
        If TextPassword.Text.Length <> 8 Then
            MsgBox("Password debe tener 8 Digitos.")
            TextPassword.Focus()
            Return False
        End If
        If TextPassword.Text <> TextConfirmacion.Text Then
            MsgBox("Confirmación Password No Coincidente.")
            TextPassword.Focus()
            Return False
        End If
        If PPasswordActual = TextPassword.Text Then
            MsgBox("Nueva Password Igual a la Actual.")
            TextPassword.Focus()
            Return False
        End If
        If TextPassword.Text = PasswordB Or TextPassword.Text = PasswordN Then
            MsgBox("Nueva Password Ya Existe para este Usuario.")
            TextPassword.Focus()
            Return False
        End If

        Return True

    End Function

   
End Class