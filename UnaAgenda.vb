Imports System.IO
Public Class UnaAgenda
    Public PArchivo As String
    '
    Dim Ruta As String
    Private Sub UnaAgenda_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Directorio As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Agenda" & GClaveEmpresa

        If Not Directory.Exists(Directorio) Then
            Directory.CreateDirectory(Directorio)
        End If

        Ruta = Directorio & "\" & PArchivo & ".TXT"

        Try
            Dim Agenda As StreamReader = File.OpenText(Ruta)
            TextBox1.Text = Agenda.ReadToEnd
            FileClose()
            Agenda.Close()
        Catch ex As Exception
        End Try

    End Sub
    Private Sub UnaAgenda_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Try
            Dim Agenda As New StreamWriter(Ruta)
            Agenda.WriteLine(TextBox1.Text)
            Agenda.Close()
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
        End Try

    End Sub

End Class