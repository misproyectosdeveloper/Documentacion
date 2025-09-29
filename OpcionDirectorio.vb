Imports System.IO
Public Class OpcionDirectorio
    Public PPath As String
    Public PFile As String
    Public PExtencion As String
    Public PRegresar As Boolean
    Private Sub OpcionDirectorio_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        PRegresar = True
        PPath = ""
        PFile = ""
        PExtencion = ""

        Dim fd As OpenFileDialog = New OpenFileDialog()

        fd.Title = "Open File Dialog"
        fd.InitialDirectory = "C:\"
        fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            PPath = fd.FileName
            PFile = fd.SafeFileName
            PExtencion = Path.GetExtension(fd.FileName)
            PRegresar = False
        End If

        Me.Close()

    End Sub
    
   
End Class