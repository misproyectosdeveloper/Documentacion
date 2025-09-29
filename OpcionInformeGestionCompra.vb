Public Class OpcionInformeGestionCompra
    Public PDesdeIngreso As Date
    Public PHastaIngreso As Date
    Public PDesdeFactura As Date
    Public PHastaFactura As Date
    Public PDomesticas As Boolean
    Public PImportacion As Boolean
    Public PAbierto As Boolean
    Public PCerrado As Boolean
    Public PRegresar As Boolean = True
    Private Sub OpcionInformeGestionCompra_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoTotal Then Panel1.Visible = False : CheckCerrado.Checked = False

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        PDesdeIngreso = DateIngresoDesde.Value
        PHastaIngreso = DateIngresoHasta.Value
        PDesdeFactura = DateFacturaDesde.Value
        PHastaFactura = DateFacturaHasta.Value
        PDomesticas = CheckDomesticas.Checked
        PImportacion = CheckImportacion.Checked
        PAbierto = CheckAbierto.Checked
        PCerrado = CheckCerrado.Checked
        PRegresar = False

        Me.Close()

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateIngresoDesde.Value, DateIngresoHasta.Value) < 0 Then
            MsgBox("Fecha Ingreso Invalida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If DiferenciaDias(DateFacturaDesde.Value, DateFacturaHasta.Value) < 0 Then
            MsgBox("Fecha Factura Invalida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Not CheckDomesticas.Checked And Not CheckImportacion.Checked Then
            MsgBox("Falta Informar Tipo de Factura.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            MsgBox("Falta Informar Candado Abierto o Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function

   
End Class