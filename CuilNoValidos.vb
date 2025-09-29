Imports ClassPassWord
Public Class CuilNoValidos

    Private Sub CuilNoValidos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Dispose()
    End Sub
    Private Sub CuilNoValidos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Nombre,Cuit FROM Clientes WHERE Pais = 1 AND DeOperacion = 0;", Conexion, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Dim aa As New DllVarias
            If Not aa.ValidaCuiT(Row("Cuit").ToString) Then
                Grid.Rows.Add("Cliente", Row("Nombre"), Format(Row("Cuit"), "00-00000000-0"))
            End If
        Next

        If Not Tablas.Read("SELECT Nombre,Cuit FROM Proveedores WHERE Pais = 1 AND TipoOperacion <> 4;", Conexion, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Dim aa As New DllVarias
            If Not aa.ValidaCuiT(Row("Cuit").ToString) Then
                Grid.Rows.Add("proveedor", Row("Nombre"), Format(Row("Cuit"), "00-00000000-0"))
            End If
        Next

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "CUIT. No Validos", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
End Class