Public Class UnTextoParaRecibo
    Public Texto1 As String
    Public Texto2 As String
    Public Texto3 As String
    Public Texto4 As String
    Public TextoFijoParaFacturas1 As String
    Public TextoFijoParaFacturas2 As String
    Private Sub UnTextoParaRecibo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""

    End Sub
    Private Sub ButtonTextoFijoParaFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTextoFijoParaFactura.Click

        TextBox1.Text = TextoFijoParaFacturas1
        TextBox2.Text = TextoFijoParaFacturas2



    End Sub
End Class