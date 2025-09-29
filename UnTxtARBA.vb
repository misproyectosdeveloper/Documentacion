Imports System.IO
Public Class UnTxtARBA
    Public PDtGrid As DataTable
    Public PPercepcion As Boolean
    Public PCodigo As Integer
    Public PDesde As Date
    Public PHasta As Date
    Public PTotal As Decimal
    Public PRetencionGanancias As Boolean
    '
    Dim Directorio As String
    Private Sub UnTxtARBA_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        TextDesde.Text = Format(PDesde, "dd/MM/yyyy")
        TextHasta.Text = Format(PHasta, "dd/MM/yyyy")

        If PRetencionGanancias Then
            RadioRetencion.Checked = True
            ButtonRencionGanancias.Visible = True
            ButtonPercepcion.Visible = False
            ButtonRetencion.Visible = False
            Label4.Text = ""
            Exit Sub
        End If

        If PPercepcion Then
            RadioPercepcion.Checked = True
            If RadioPercepcion.Checked Then
                ButtonPercepcion.Visible = True
                Panel1.Visible = False
            End If
        Else
            RadioRetencion.Checked = True
            If RadioRetencion.Checked Then
                ButtonPercepcion.Visible = False
                Panel1.Visible = True
            End If
        End If

        Label4.Text = ""

    End Sub
    Private Sub UnTxtARBA_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Private Sub ButtonPercepcion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPercepcion.Click

        Directorio = ""
        Directorio = PideCarpeta()
        If Directorio = "" Then Exit Sub

        Directorio = Directorio & "\" & "AR-" & Format(PDesde, "yyyyMM") & "0-7-lote1.txt"

        Using writer As StreamWriter = New StreamWriter(Directorio)                        'Borra Archivo.
        End Using

        Dim Str As String
        Dim TipoComprobante As String
        Dim ImporteStr As String
        Dim Base As Decimal
        Dim BaseStr As String
        Dim LetraInt As Integer
        Dim PuntoDeVenta As Integer
        Dim Numero As Integer
        Dim Total As Decimal = 0

        For Each Row As DataRow In PDtGrid.Rows
            If Row("Estilo") = 0 Then
                If Row("Tipo") = PCodigo And Row("Estado") <> 3 Then
                    Str = ""
                    Select Case Row("TipoComprobante")
                        Case 2
                            TipoComprobante = "F"   'factura.
                        Case 4
                            TipoComprobante = "C"   'credito con devolucion.
                        Case 100  'No se procesa.
                            '''                       RowGrid("Cartel") = "Liquidación a Prov."
                        Case 5
                            TipoComprobante = "F"   'N.D. a Cliente"
                        Case 7
                            TipoComprobante = "C"   'N.C. a Cliente"
                        Case 6
                            TipoComprobante = "F"   'N.D. a Proveedor"
                        Case 8
                            TipoComprobante = "C"   'N.C. a Proveedor"
                    End Select
                    ImporteStr = ConvierteNumeroAString(Row("Importe"), ".", 8)
                    Total = Total + Row("Importe")
                    Base = 100 * Row("Importe") / Row("Alicuota")
                    BaseStr = ConvierteNumeroAString(Base, ".", 9)
                    DescomponeNumeroComprobante(Row("Comprobante"), LetraInt, PuntoDeVenta, Numero)
                    Str = Format(Row("Cuit"), "00-00000000-0") & Format(Row("Fecha"), "dd/MM/yyyy") & TipoComprobante & LetraTipoIva(LetraInt) & Format(PuntoDeVenta, "0000") & Format(Numero, "00000000") & BaseStr & ImporteStr & "A"
                    Using writer As StreamWriter = New StreamWriter(Directorio, True)
                        writer.WriteLine(Str)
                        writer.Close()
                    End Using
                End If
            End If
        Next

        If PTotal <> Total Then
            MsgBox("ERROR: Total $ Procesado: " & FormatNumber(Total) & " No Coinciden con total de Percepciones.")
        Else
            MsgBox("Total $ Procesado: " & FormatNumber(Total))
            Label4.Text = "Se genero archivo: " & Directorio
        End If

    End Sub
    Private Sub ButtonRetencion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRetencion.Click

        If TextNumero.Text = "" Then
            MsgBox("Debe Informar numero de Quincena.") : Exit Sub
        End If
        If TextNumero.Text <> "1" And TextNumero.Text <> "2" Then
            MsgBox("Incorrecto numero de Quincena.") : Exit Sub
        End If

        Directorio = ""
        Directorio = PideCarpeta()
        If Directorio = "" Then Exit Sub

        Directorio = Directorio & "\" & "AR-" & Format(PDesde, "yyyyMM") & TextNumero.Text & "-6-lote1.txt"

        Using writer As StreamWriter = New StreamWriter(Directorio)                        'Borra Archivo.
        End Using

        Dim Str As String
        Dim TipoComprobante As String
        Dim ImporteStr As String
        Dim LetraInt As Integer
        Dim PuntoDeVenta As Integer
        Dim Numero As Integer
        Dim Total As Decimal = 0

        For Each Row As DataRow In PDtGrid.Rows
            If Row("Estilo") = 0 Then
                If Row("Tipo") = PCodigo And Row("Estado") <> 3 Then
                    Str = ""
                    ImporteStr = ConvierteNumeroAString(Row("Importe"), ".", 8)
                    If Row("Comprobante").ToString.Length > 12 Then
                        DescomponeNumeroComprobante(Row("Comprobante"), LetraInt, PuntoDeVenta, Numero)
                    Else
                        PuntoDeVenta = Strings.Left(Row("Comprobante").ToString, Row("Comprobante").ToString.Length - 8)
                        Numero = Strings.Right(Row("Comprobante").ToString, 8)
                    End If
                    Str = Format(Row("Cuit"), "00-00000000-0") & Format(Row("Fecha"), "dd/MM/yyyy") & Format(PuntoDeVenta, "0000") & Format(Row("Retencion"), "00000000") & ImporteStr & "A"
                    Total = Total + Row("Importe")
                    Using writer As StreamWriter = New StreamWriter(Directorio, True)
                        writer.WriteLine(Str)
                        writer.Close()
                    End Using
                End If
            End If
        Next

        If PTotal <> Total Then
            MsgBox("ERROR: Total $ Procesado: " & FormatNumber(Total) & " No Coinciden con total de Retenciones.")
        Else
            MsgBox("Total $ Procesado: " & FormatNumber(Total))
            Label4.Text = "Se genero archivo: " & Directorio
        End If

    End Sub
    Private Sub ButtonRencionGanancias_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRencionGanancias.Click

        Directorio = ""
        Directorio = PideCarpeta()
        If Directorio = "" Then Exit Sub

        ''    Directorio = Directorio & "\" & "RetenGan-Desde:" & Format(PDesde, "yyyyMM") & "-Hasta:" & Format(PHasta, "yyyyMM") & ".TXT"
        Directorio = Directorio & "\" & "Reten-Ganancias" & ".TXT"

        Using writer As StreamWriter = New StreamWriter(Directorio)                        'Borra Archivo.
        End Using

        Dim Str As String
        Dim CodigoDeComprobante As String
        Dim Fecha As String
        Dim NumeroComprobante As String
        Dim ImporteComprobante As String
        Dim CodigoImpuesto As String
        Dim CodigoRegimen As String
        Dim CodigoOperacion As String
        Dim BaseCalculo As String
        Dim FechaEmisionRetencion As String
        Dim CodigoCondicion As String
        Dim RetenPracticadaSujetosSuspendidos As String
        Dim ImporteRetencion As String
        Dim PorcentajeExclusion As String
        Dim FechaPublicacion As String
        Dim TipoDocumentoRetenido As String
        Dim NumeroDocumentoRetenido As String
        Dim NumeroCertificadoOriginal As String
        Dim Total As Decimal = 0

        For Each Row As DataRow In PDtGrid.Rows
            If Row("Estilo") = 1 Then Continue For
            If Row("Estado") = 3 Then Continue For
            If IsDBNull(Row("Formula")) Then Continue For
            If Row("Formula") <> 1 Then Continue For
            Select Case Row("TipoComprobante")
                Case 100                             'Ver Instructivo.
                    CodigoDeComprobante = "05"
                Case 600
                    CodigoDeComprobante = "06"
            End Select
            Fecha = Format(Row("Fecha"), "dd/MM/yyyy")
            NumeroComprobante = RellenarCeros(Row("Comprobante"), 16)
            ImporteComprobante = Format(Row("ImporteComprobante"), "0000000000000.00")   'Importe comprobante. 
            CodigoImpuesto = "0217"
            CodigoRegimen = "078"
            CodigoOperacion = "1"
            BaseCalculo = Format(HallaBaseCalculo(Row("Tipo"), Row("TipoComprobante"), Row("Comprobante"), Row("ImporteComprobante"), Row("Importe")), "00000000000.00")   '??????????????" 'Format(Row("ImporteComprobante"), "00000000000.00")   'Importe Retencion.????????
            FechaEmisionRetencion = Format(Row("Fecha"), "dd/MM/yyyy")
            CodigoCondicion = Format(HallaTipoIvaProveedor(Row("Proveedor")), "00")
            RetenPracticadaSujetosSuspendidos = "0"
            ImporteRetencion = Format(Row("Importe"), "00000000000.00")  'Importe Retencion.
            Total = Total + ImporteRetencion
            PorcentajeExclusion = "000,00"
            FechaPublicacion = "          "
            TipoDocumentoRetenido = "80"
            NumeroDocumentoRetenido = RellenarBlancos(Row("Cuit"), 20)
            NumeroCertificadoOriginal = Strings.Right(NumeroComprobante, 14)
            Str = CodigoDeComprobante & Fecha & NumeroComprobante & ImporteComprobante & CodigoImpuesto & _
                  CodigoRegimen & CodigoOperacion & BaseCalculo & FechaEmisionRetencion & CodigoCondicion & _
                  RetenPracticadaSujetosSuspendidos & ImporteRetencion & PorcentajeExclusion & FechaPublicacion & _
                  TipoDocumentoRetenido & NumeroDocumentoRetenido & NumeroCertificadoOriginal
            ' Reemplaza coma decimal por punto.  
            Str = Replace(Str, ",", ".")
            Using writer As StreamWriter = New StreamWriter(Directorio, True)
                writer.WriteLine(Str)
                writer.Close()
            End Using
        Next

        MsgBox("Total Importe Retenciones Procesadas: " & FormatNumber(Total))
        Label4.Text = "Se Genero Archivo: " & Directorio

    End Sub
    Private Sub TextNumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumero.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, 0)

    End Sub
    Private Function HallaBaseCalculo(ByVal Tipo As Integer, ByVal TipoComprobante As Integer, ByVal NumeroComprobante As Decimal, ByVal ImporteComprobante As Decimal, ByVal ImporteRetencion As Decimal) As Decimal

        Dim Alicuota As Decimal

        If TipoComprobante = 600 Then  'Orden pago.
            Alicuota = HallaAlicuotaRetencionOrdenPago(Tipo)
        End If
        If TipoComprobante = 100 Then  'Liquidación.
            Alicuota = HallaAlicuotaRetencionLiquidacion(Tipo, NumeroComprobante)
        End If
        If Alicuota = -1 Then
            MsgBox("Error: al Hallar Alicuota Retención " & Tipo) : Me.Close()
        End If
        If Alicuota = 0 Then
            MsgBox("Error: Alicuota Retención = 0. en Retención " & Tipo) : Me.Close()
        End If
        Return Trunca(ImporteRetencion * 100 / Alicuota)

    End Function
    Private Function HallaAlicuotaRetencionOrdenPago(ByVal Tipo As Integer) As Decimal

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT AlicuotaRetencion FROM Tablas WHERE Clave = " & Tipo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaAlicuotaRetencionLiquidacion(ByVal Tipo As Integer, ByVal NumeroDocumento As Decimal) As Decimal

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Valor FROM LiquidacionDetalleConceptos WHERE Liquidacion = " & NumeroDocumento & " AND Concepto = " & Tipo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function

End Class