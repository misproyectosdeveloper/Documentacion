Public Class UnSaldoFCE
    Public PTipoNota As Integer
    Public PFactura As Decimal
    Public PDevolucion As Decimal
    Public PNCreditoF As Decimal
    Public PNDebitoF As Decimal
    Public PImporteComprobante As Decimal
    Public PSaldo As Decimal
    Public PDifer As Decimal
    Public PEsAnulacion As Boolean
    Public PEsDevuelto As Boolean
    Public PError As Boolean
    Public PMuestraFormulario As Boolean
    '
    Dim ImporteFactura As Decimal
    Dim ImporteSenia As Decimal
    Private Sub UnCalculoFCE_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Label1.Text = "Factura " & NumeroEditado(PFactura)
        LabelCartel.Text = ""
        Grid.Rows.Clear()

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Importe,Percepciones,Cae,Senia FROM FacturasCabeza WHERE Factura = " & PFactura & ";", Conexion, Dt) Then End
        Dim ImporteStr As String = FormatNumber(Dt.Rows(0).Item("Importe") + Dt.Rows(0).Item("Percepciones") - CDec(Dt.Rows(0).Item("Senia")), 2)
        PSaldo = CDec(Dt.Rows(0).Item("Importe")) + CDec(Dt.Rows(0).Item("Percepciones"))
        ImporteFactura = CDec(Dt.Rows(0).Item("Importe")) + CDec(Dt.Rows(0).Item("Percepciones")) - CDec(Dt.Rows(0).Item("Senia"))
        ImporteSenia = Dt.Rows(0).Item("Senia")
        Dim StrCae As String
        If Dt.Rows(0).Item("Cae") = 0 Then
            StrCae = "NO"
        Else
            StrCae = "SI"
        End If
        Grid.Rows.Add("Factura ", NumeroEditado(PFactura), ImporteStr, StrCae)
        If ImporteSenia Then
            Grid.Rows.Add("Seña ", "", FormatNumber(ImporteSenia, 2), StrCae)
        End If

        'Halla Devolucion.
        Dim DtDetalleFactura As New DataTable
        PEsDevuelto = True
        If Not Tablas.Read("SELECT Articulo,Cantidad,Devueltas FROM FacturasDetalle WHERE Factura = " & PFactura & ";", Conexion, DtDetalleFactura) Then End
        For Each Row As DataRow In DtDetalleFactura.Rows
            If Row("Cantidad") <> Row("Devueltas") Then PEsDevuelto = False
        Next
        DtDetalleFactura.Dispose()

        Dt = New DataTable
        If Not Tablas.Read("SELECT NotaCredito,Importe,Percepciones,Cae FROM NotasCreditoCabeza WHERE Factura = " & PFactura & ";", Conexion, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            ImporteStr = FormatNumber(-(Row("Importe") + Row("Percepciones")), 2)
            PSaldo = PSaldo - (CDec(Row("Importe")) + CDec(Row("Percepciones")))
            PDevolucion = PDevolucion + (CDec(Row("Importe")) + CDec(Row("Percepciones")))
            If Row("Cae") = 0 Then
                StrCae = "NO"
            Else
                StrCae = "SI"
            End If
            Grid.Rows.Add("N.C.Devolución", NumeroEditado(Row("NotaCredito")), ImporteStr, StrCae)
        Next

        Dt = New DataTable
        If Not Tablas.Read("SELECT Nota,Importe,Cae FROM RecibosCabeza WHERE TipoNota = 7 AND TipoCompAsociado = 1 AND CompAsociado = " & PFactura & ";", Conexion, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            ImporteStr = FormatNumber(-Row("Importe"), 2)
            PSaldo = PSaldo - CDec(Row("Importe"))
            PNCreditoF = PNCreditoF + CDec(Row("Importe"))
            If Row("Cae") = 0 Then
                StrCae = "NO"
            Else
                StrCae = "SI"
            End If
            Grid.Rows.Add("N.C.Financiera", NumeroEditado(Row("Nota")), ImporteStr, StrCae)
        Next

        Dt = New DataTable
        If Not Tablas.Read("SELECT Nota,Importe,Cae FROM RecibosCabeza WHERE TipoNota = 5 AND TipoCompAsociado = 1 AND CompAsociado = " & PFactura & ";", Conexion, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            ImporteStr = FormatNumber(Row("Importe"), 2)
            PSaldo = PSaldo + CDec(Row("Importe"))
            PNDebitoF = PNDebitoF + CDec(Row("Importe"))
            If Row("Cae") = 0 Then
                StrCae = "NO"
            Else
                StrCae = "SI"
            End If
            Grid.Rows.Add("N.D.Financiera", NumeroEditado(Row("Nota")), ImporteStr, StrCae)
        Next

        Grid.Rows.Add("SALDO", "", FormatNumber(PSaldo))

        Procedimiento()

        If Not PMuestraFormulario Then
            Me.Close()
        End If

    End Sub
    Private Sub Procedimiento()

        PDifer = PNDebitoF + ImporteSenia - PNCreditoF

        If PSaldo = 0 Then
            If PMuestraFormulario Then LabelCartel.Text = "OPERACION ANULADA."
        End If

        If PSaldo > 0 And PEsDevuelto Then
            If PMuestraFormulario Then LabelCartel.Text = "1. Debe emitir una N.C.F por " & FormatNumber(PSaldo, 2)
        End If

        If PSaldo < 0 And PEsDevuelto Then
            If PMuestraFormulario Then LabelCartel.Text = "1. Debe emitir una N.D.F por " & FormatNumber(PSaldo, 2)
        End If

        If PSaldo > 0 And Not PEsDevuelto Then
            If PMuestraFormulario Then LabelCartel.Text = "1. Emitir N.C.Devolución por el total de la factura y" + vbCrLf + _
               "2. Emitir una N.C.F o N.D.F por el Saldo resultante."
        End If

        If PSaldo < 0 And Not PEsDevuelto Then
            If PMuestraFormulario Then LabelCartel.Text = "1. Emitir N.C.Devolución por el total de la factura y" + vbCrLf + _
               "2. Emitir una N.C.F o N.D.F por el Saldo resultante."
        End If

    End Sub
    
End Class