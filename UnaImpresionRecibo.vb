Imports System.Drawing.Printing
Module UnaImpresionRecibo
    Dim TipoNota As Integer
    Dim DtNotaCabeza As DataTable
    Dim DtGrid As DataTable
    Dim DtFormasPago As DataTable
    Dim Abierto As Boolean
    Dim EsRetencionManual As Boolean
    Dim Cuit As String
    Dim NombreEmisor As String
    Dim TextImpresionOrdenPago As String
    '-----------------------------------------------Para impresion Cta.Cte.--------------------------------------
    Public PNombreClienteCtaCte As String
    Public PDesdeCtaCte As Date
    Public PHastaCtaCte As Date
    Public PMoneda As String
    Public GridCompro As DataGridView
    Public PtipoEmisor As Integer
    Public PEsAmbasOpr As Boolean
    Dim Listado As List(Of ItemCtaCte)
    Dim TotalComprobante As Decimal = 0
    Dim TotalImpuesto As Decimal = 0
    Dim TotalSenia As Decimal = 0
    Dim TotalGralCtaCte As Decimal
    Dim TotalSaldoPeriodo As Decimal
    Dim ReciboOficialAnt As New Hashtable
    Dim DiferenciaTotales As Decimal = 0
    'Variables Impresion. 
    Dim ErrorImpresion As Boolean
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim LineasParaImpresion As Integer = 0
    Dim LineasParaImpresionImputacion As Integer = 0
    'variables para imprimir retenciones manuales.
    Dim ClaveRetencion As Integer
    Dim NumeroRetencion As Integer
    Dim NombreRetencion As String
    Dim ImporteRetencion As Decimal
    Dim PagoNetoMesRetencion As Decimal
    Dim RetencionPagadaMes As Decimal
    Dim TopeMesRetencion As Decimal
    Dim AlicuotaRetencion As Decimal
    Dim PaginasImpresas As Integer = 0
    Public Sub UnaImpresionReciboW(ByVal TipoNotaW As Integer, ByVal AbiertoW As Boolean, ByVal DtNotaCabezaW As DataTable, ByVal DtGridW As DataTable, ByVal GridComproW As DataGridView, ByVal DtFormasPagoW As DataTable, _
                                   ByVal EsRetencionManualW As Boolean, ByVal CuitW As String, ByVal NombreEmisorW As String, ByVal TextImpresionOrdenPagoW As String)

        TipoNota = TipoNotaW
        Abierto = AbiertoW
        DtNotaCabeza = DtNotaCabezaW
        DtGrid = DtGridW
        GridCompro = GridComproW
        DtFormasPago = DtFormasPagoW
        EsRetencionManual = EsRetencionManualW
        Cuit = CuitW
        NombreEmisor = NombreEmisorW
        TextImpresionOrdenPago = TextImpresionOrdenPagoW

        ErrorImpresion = False
        Paginas = 0
        If Not Abierto Then Copias = 1

        Dim print_document As New PrintDocument

        If TipoNota = 60 Or TipoNota = 604 Then
            LineasParaImpresion = 0
            LineasParaImpresionImputacion = 0
            Copias = 1
            AddHandler print_document.PrintPage, AddressOf Print_PrintCobranza
            print_document.Print()
        End If

        If TipoNota = 600 Or TipoNota = 64 Then
            LineasParaImpresion = 0
            LineasParaImpresionImputacion = 0
            AddHandler print_document.PrintPage, AddressOf Print_PrintOrdenPago
            print_document.Print()
            'Imprime retenciones.
            If Not ImprimirRetenciones() Then Exit Sub
        End If

        If Abierto Then
            If Not GrabaImpreso(DtNotaCabeza, Conexion) Then Exit Sub
            DtNotaCabeza.Rows(0).Item("Impreso") = True
        Else
            If Not GrabaImpreso(DtNotaCabeza, ConexionN) Then Exit Sub
            DtNotaCabeza.Rows(0).Item("Impreso") = True
        End If

    End Sub
    Private Sub Print_PrintCobranza(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 37
        Dim UltimaLinea As Integer
        Dim LineasImpresas As Integer
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        Dim Imputaciones As Integer = CantidadImputaciones()

        Try
            Print_TituloCobranza(e, MIzq, MTop)

            If LineasParaImpresion < DtGrid.Rows.Count Then
                Print_MediosPago(e, MIzq, MTop, UltimaLinea, LineasImpresas)
                'Imprime imputaciones.
                If Imputaciones > 0 And LineasParaImpresion >= DtGrid.Rows.Count And LineasImpresas < 27 Then
                    Print_Imputaciones(e, MIzq, UltimaLinea + 10, 37 - LineasImpresas - 4)
                End If
            Else
                If Imputaciones > 0 Then
                    Print_Imputaciones(e, MIzq, MTop + 50, 37)
                End If
            End If

            ''''''''''''       Print_FinalOrdenPago(e, MIzq, MTop)

            If (LineasParaImpresion < DtGrid.Rows.Count) Or (Imputaciones > 0 And LineasParaImpresionImputacion < GridCompro.Rows.Count) Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
            e.HasMorePages = False
        End Try

    End Sub
    Private Sub Print_PrintOrdenPago(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 37
        Dim UltimaLinea As Integer
        Dim LineasImpresas As Integer
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        Dim Imputaciones As Integer = CantidadImputaciones()

        Try
            Print_TituloOrdenPago(e, MIzq, MTop)

            If LineasParaImpresion < DtGrid.Rows.Count Then
                Print_MediosPago(e, MIzq, MTop, UltimaLinea, LineasImpresas)
                'Imprime imputaciones.
                If Imputaciones > 0 And LineasParaImpresion >= DtGrid.Rows.Count And LineasImpresas < 27 Then
                    Print_Imputaciones(e, MIzq, UltimaLinea + 10, 37 - LineasImpresas - 4)
                End If
            Else
                If Imputaciones > 0 Then
                    Print_Imputaciones(e, MIzq, MTop + 50, 37)
                End If
            End If

            Print_FinalOrdenPago(e, MIzq, MTop)

            If (LineasParaImpresion < DtGrid.Rows.Count) Or (Imputaciones > 0 And LineasParaImpresionImputacion < GridCompro.Rows.Count) Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
            e.HasMorePages = False
        End Try

    End Sub
    Private Function ImprimirRetenciones() As Boolean

        Dim DtRetencionesAux As New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre,TopeMes,CodigoRetencion,AlicuotaRetencion,TipoIva,Formula,0.0 AS Importe FROM Tablas WHERE Tipo= 25;", Conexion, DtRetencionesAux) Then End

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtRetencionesAux.Select("Clave = " & Row("MedioPago"))
            If RowsBusqueda.Length <> 0 Then
                ClaveRetencion = Row("MedioPago")
                NumeroRetencion = Row("Comprobante")
                ImporteRetencion = Row("Importe")
                NombreRetencion = RowsBusqueda(0).Item("Nombre")
                TopeMesRetencion = RowsBusqueda(0).Item("TopeMes")
                AlicuotaRetencion = RowsBusqueda(0).Item("AlicuotaRetencion")
                ErrorImpresion = False
                Paginas = 0
                Copias = 1
                Dim print_document As New PrintDocument
                If EsRetencionManual Then
                    AddHandler print_document.PrintPage, AddressOf Print_RetencionManual
                Else
                    Select Case RowsBusqueda(0).Item("Formula")
                        Case 0
                            AddHandler print_document.PrintPage, AddressOf Print_RetencionManual
                        Case 1
                            TotalesImprisionRetencionGanancia(RowsBusqueda(0))
                            AddHandler print_document.PrintPage, AddressOf Print_RetencionGanancia
                        Case 4
                            AlicuotaRetencion = LeerPadron(RowsBusqueda(0).Item("Clave"), DtNotaCabeza.Rows(0).Item("Fecha"), CuitNumerico(Cuit))
                            If AlicuotaRetencion = -100 Then AlicuotaRetencion = 0
                            AddHandler print_document.PrintPage, AddressOf Print_RetencionFormula4
                    End Select
                End If
                print_document.Print()
                If ErrorImpresion Then Return False
            End If
        Next

        Return True

    End Function
    Private Sub Print_TituloOrdenPago(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

        Dim Texto As String = ""

        'Encabezado.
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
        Texto = GNombreEmpresa
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
        Texto = "ORDEN DE PAGO"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 140, MTop)
        If TipoNota = 64 Then
            Texto = "(Devolución) "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 140, MTop + 4)
        End If
        ' 
        PrintFont = New Font("Courier New", 12)
        Texto = "Nro. Orden Pago:  " & Format(DtNotaCabeza.Rows(0).Item("Nota"), "0000-00000000")
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
        Texto = "Fecha:  " & Format(DtNotaCabeza.Rows(0).Item("Fecha"), "dd/MM/yyyy")
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
        Texto = "Proveedor: " & NombreEmisor & "  Cuit.: " & Cuit
        If TipoNota = 64 Then
            Texto = "Cliente  : " & NombreEmisor & "  Cuit.: " & Cuit
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 27)
        If TipoNota = 64 Then
            Texto = "Alias    : " & HallaAliasCliente()
        Else
            Texto = "Alias    : " & HallaAliasProveedor() & "     " & TextImpresionOrdenPago
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 33)
        Texto = "Importe Orden de Pago : " & FormatNumber(DtNotaCabeza.Rows(0).Item("Importe"), 2)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 60, MTop + 40)

    End Sub
    Private Sub Print_TituloCobranza(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

        Dim Texto As String = ""

        'Encabezado.
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
        Texto = GNombreEmpresa
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
        Texto = "RECIBO DE COBRANZA"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 125, MTop)
        If TipoNota = 604 Then
            Texto = "(Devolución) "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 125, MTop + 4)
        End If
        ' 
        PrintFont = New Font("Courier New", 12)
        '        Texto = "Nro. Cobranza:  " & NumeroEditado(DtNotaCabeza.Rows(0).Item("Nota").ToString)
        Texto = "Nro. Cobranza:  " & Format(DtNotaCabeza.Rows(0).Item("Nota"), "0000-00000000")
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
        Texto = "Fecha:  " & Format(DtNotaCabeza.Rows(0).Item("Fecha"), "dd/MM/yyyy")
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
        Texto = "Cliente: " & NombreEmisor & "  Cuit.: " & Cuit
        If TipoNota = 604 Then
            Texto = "Proveedor  : " & NombreEmisor & "  Cuit.: " & Cuit
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 27)
        If TipoNota = 604 Then
            Texto = "Alias    : " & HallaAliasProveedor()
        Else
            Texto = "Alias    : " & HallaAliasCliente()
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 33)
        Texto = "Importe Cobranza: " & FormatNumber(DtNotaCabeza.Rows(0).Item("Importe"), 2)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 60, MTop + 40)

    End Sub
    Private Sub Print_FinalOrdenPago(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer)

        Dim Texto As String = ""
        Dim Yq As Integer
        Dim SaltoLinea As Integer = 4
        Dim PrintFont As System.Drawing.Font

        Yq = MTop + 215
        PrintFont = New Font("Courier New", 10)
        Texto = "RECIBI LOS VALORES DESCRIPTOS PRECEDENTEMENTE  ........................... "
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
        Yq = Yq + SaltoLinea
        Texto = "                                                       F I R M A           "
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
        Yq = Yq + 2 * SaltoLinea
        Texto = "ACLARACION APELLIDO Y NOMBRE :............................................."
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
        Yq = Yq + 2 * SaltoLinea
        Texto = "DOCUMENTO  TIPO :...........  Nro.:........................................"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
        Yq = Yq + 3 * SaltoLinea
        Texto = "A U T O R I Z O : ............................."
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)

    End Sub
    Private Sub Print_MediosPago(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer, ByRef UltimaLinea As Integer, ByRef LineasImpresas As Integer)

        Dim Texto As String = ""
        Dim SaltoLinea As Integer = 4
        Dim Longi As Integer
        Dim Xq As Integer
        Dim Yq As Integer
        Dim x As Integer = MIzq
        Dim y As Integer = MTop + 50
        Dim Ancho As Integer = 180
        Dim RowsBusqueda() As DataRow
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 37

        'Grafica -Rectangulo medios de pagos. ----------------------------------------------------------------------

        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 10)
        Dim LineaDescripcion As Integer = x + 40
        Dim LineaCambio As Integer = x + 55
        Dim LineaImporte As Integer = x + 85
        Dim LineaBanco As Integer = x + 125
        Dim LineaNumero As Integer = x + 154
        Dim LineaVencimiento As Integer = x + Ancho
        'Titulos de descripcion.
        Texto = "DESCRIPCION DEL PAGO"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, y - 4)
        Texto = "DESCRIPCION"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 2, y + 2)
        Texto = "CAMBIO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaCambio - Longi
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "IMPORTE"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaImporte - Longi - 10
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "BANCO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaBanco - Longi - 20
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "NUMERO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaNumero - Longi - 10
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "VTO."
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaVencimiento - Longi - 10
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        'Descripcion del pago.
        Yq = y + SaltoLinea
        While Contador < LineasPorPagina And LineasParaImpresion < DtGrid.Rows.Count
            Dim Row As DataRow = DtGrid.Rows(LineasParaImpresion)
            RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
            Yq = Yq + SaltoLinea
            'Imprime Detalle.
            Texto = RowsBusqueda(0).Item("Nombre")
            Xq = x
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            If Row("Cambio") <> 0 Then
                'Imprime Cambio.
                Texto = FormatNumber(Row("Cambio"), GDecimales)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaCambio - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Importe.
                Texto = FormatNumber(Trunca(Row("Cambio") * Row("Importe")), GDecimales)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Else
                'Imprime Importe.
                Texto = FormatNumber(Row("Importe"), GDecimales)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            End If
            'Imprime Banco.
            Texto = NombreBanco(Row("Banco"))
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte + 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            'Imprime Numero.
            If Row("Numero") <> 0 Then
                Texto = FormatNumber(Row("Numero"), 0)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaNumero - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Vencimeinto.
                Texto = Format(Row("Fecha"), "dd/MM/yyyy")
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaVencimiento - Longi - 2
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            End If
            'Imprime Comprobante.
            If Row("Comprobante") <> 0 Then
                Texto = FormatNumber(Row("Comprobante"), 0)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaNumero - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime FechaComprobante.
                If Format(Row("FechaComprobante"), "dd/MM/yyyy") <> "01/01/1800" Then
                    Texto = Format(Row("FechaComprobante"), "dd/MM/yyyy")
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaVencimiento - Longi - 2
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
            End If
            Contador = Contador + 1
            LineasParaImpresion = LineasParaImpresion + 1
        End While

        Yq = Yq + SaltoLinea

        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Yq - y)
        'Lineas vertical.
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaDescripcion, y, LineaDescripcion, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCambio, y, LineaCambio, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte, y, LineaImporte, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaBanco, y, LineaBanco, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaNumero, y, LineaNumero, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y + 2 * SaltoLinea, x + Ancho, y + 2 * SaltoLinea)

        UltimaLinea = Yq
        LineasImpresas = Contador

    End Sub
    Private Sub Print_Imputaciones(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal MIzq As Integer, ByVal MTop As Integer, ByVal LineasPorPagina As Integer)

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim Contador As Integer = 0
        Dim PrintFont As System.Drawing.Font

        Dim Longi As Integer
        Dim Xq As Integer
        Dim Yq As Integer
        Dim Ancho As Integer

        'Grafica -Rectangulo Imputacion. ----------------------------------------------------------------------
        y = MTop
        x = MIzq
        Ancho = 183
        PrintFont = New Font("Courier New", 10)

        Dim LineaTipo As Integer = x + 35
        Dim LineaComprobante As Integer = x + 69
        Dim LineaFecha As Integer = x + 94
        Dim LineaImporte1 As Integer = x + 125
        Dim LineaSaldo As Integer = x + 155
        Dim LineaImporte2 As Integer = x + Ancho
        'Titulos de descripcion.
        Texto = "ASIGNACIONES"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, y - 4)
        Texto = "TIPO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 2, y + 2)
        Texto = "COMPROBANTE"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaComprobante - Longi - 6
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "FECHA"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaFecha - Longi - 5
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "IMPORTE"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaImporte1 - Longi - 10
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "SALDO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaSaldo - Longi - 5
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
        Texto = "IMPUTADO"
        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
        Xq = LineaImporte2 - Longi - 5
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)

        'Descripcion Imputacion.
        Yq = y + SaltoLinea
        While Contador < LineasPorPagina And LineasParaImpresionImputacion < GridCompro.Rows.Count
            Dim Row As DataGridViewRow = GridCompro.Rows(LineasParaImpresionImputacion)
            If Row.Cells("Asignado").Value <> 0 Then
                Yq = Yq + SaltoLinea
                'Imprime Tipo.
                Texto = Row.Cells("Tipo").FormattedValue
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Comprobante.
                Texto = Row.Cells("Recibo").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaComprobante - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Fecha.
                Texto = Row.Cells("FechaCompro").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaFecha - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Importe.
                Texto = Row.Cells("ImporteCompro").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte1 - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime saldo.
                Texto = Row.Cells("Saldo").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaSaldo - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Asignado.
                Texto = Row.Cells("Asignado").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte2 - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Contador = Contador + 1
            End If
            LineasParaImpresionImputacion = LineasParaImpresionImputacion + 1
        End While

        Yq = Yq + SaltoLinea

        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Yq - y)
        'Lineas vertical.
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaTipo, y, LineaTipo, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaComprobante, y, LineaComprobante, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaFecha, y, LineaFecha, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte1, y, LineaImporte1, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaSaldo, y, LineaSaldo, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte2, y, LineaImporte2, Yq)
        e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y + 2 * SaltoLinea, x + Ancho, y + 2 * SaltoLinea)

    End Sub
    Private Sub Print_RetencionManual(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 60
        Dim Longi As Integer
        Dim LineaImporte As Integer = MIzq + 150
        Dim Xq As Integer

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            '--------------------------------------------------- Logo -----------------------------------------------
            Dim NombreEmpres As String = GNombreEmpresa '"Patagonia Sunrise S.R.L."
            Dim CuitEmpresa As String = GCuitEmpresa    '"30-12345678-5"
            Dim TipoComprobante As Integer = 14          '14. CERTIFICADO
            Dim Direccion1 As String = GDireccion1      '"Nave S-2 Puesto 34 - Mercado Central de Buenos Aires"
            Dim Direccion2 As String = GDireccion2      '"Autopista Ricchieri y Boulongne Sur Mer"
            Dim Direccion3 As String = GDireccion3      '"C.P.(1771) Villa Celina Pcia. de Buenos Aires"
            Dim LetraComprobante As String = ""
            Dim IngBruto As String = GIngBruto     '"Conv.Mult. Nª 901-898989-6"
            Dim FechaInicio As String = GFechaInicio
            '
            MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, LetraComprobante, GCondicionIvaEmpresa, IngBruto, FechaInicio, "C:\XML Afip\", e, False, TipoComprobante)
            Texto = FormatNumber(NumeroRetencion, 0)
            Dim PrintFont As System.Drawing.Font = New Font("Arial", 16)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, 25)
            Texto = Format(DtNotaCabeza.Rows(0).Item("Fecha"), "dd/MM/yyyy")
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, 35)
            ''''''''''''''''''''''''''''''''''''''''''''''''Fin Logo --------------------------------------------------
            'Encabezado.
            y = MTop
            x = MIzq
            y = y + SaltoLinea
            PrintFont = New Font("Courier New", 14, FontStyle.Bold)
            Texto = "CERTIFICADO DE RETENCION"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 50, y)
            '
            PrintFont = New Font("Courier New", 13, FontStyle.Bold)
            y = y + 5 * SaltoLinea
            Texto = "RETENCION : " & NombreRetencion
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = "FECHA : " & Format(DtNotaCabeza.Rows(0).Item("Fecha"), "dd/MM/yyyy")
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 120, y)
            y = y + SaltoLinea
            Texto = "NRO. CERTIFICADO : " & FormatNumber(NumeroRetencion, 0)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            y = y + 2 * SaltoLinea
            Texto = "AGENTE DE RETENCION : " & GNombreEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "CUIT.: " & GCuitEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + 2 * SaltoLinea
            Texto = "SUJETO DE RETENCION : " & NombreEmisor
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "CUIT.: " & Cuit
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            PrintFont = New Font("Courier New", 13)
            y = y + 4 * SaltoLinea
            Texto = "EN EL DIA DE LA FECHA HEMOS RETENIDO EL IMPORTE DE $ " & FormatNumber(ImporteRetencion, GDecimales)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "Son pesos " & Letras(ImporteRetencion.ToString)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            Dim PagadoActual As Double = CDec(DtNotaCabeza.Rows(0).Item("Importe")) - CDec(DtNotaCabeza.Rows(0).Item("Importe")) * HallaIvaDeCodigo(DtNotaCabeza.Rows(0).Item("CodigoIva")) / 100
            '
            y = y + 2 * SaltoLinea
            y = y + SaltoLinea
            Texto = "MONTO RETENIDO                "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(ImporteRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + 8 * SaltoLinea
            Texto = "FIRMA ........................"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 80, y)

            Paginas = Paginas + 1

            If Paginas < Copias Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
            e.HasMorePages = False
        End Try

    End Sub
    Private Sub Print_RetencionGanancia(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 60
        Dim RowsBusqueda() As DataRow
        Dim Longi As Integer
        Dim LineaImporte As Integer = MIzq + 150
        Dim Xq As Integer

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            '--------------------------------------------------- Logo -----------------------------------------------
            Dim NombreEmpres As String = GNombreEmpresa '"Patagonia Sunrise S.R.L."
            Dim CuitEmpresa As String = GCuitEmpresa    '"30-12345678-5"
            Dim TipoComprobante As Integer = 14          '14. CERTIFICADO
            Dim Direccion1 As String = GDireccion1      '"Nave S-2 Puesto 34 - Mercado Central de Buenos Aires"
            Dim Direccion2 As String = GDireccion2      '"Autopista Ricchieri y Boulongne Sur Mer"
            Dim Direccion3 As String = GDireccion3      '"C.P.(1771) Villa Celina Pcia. de Buenos Aires"
            Dim LetraComprobante As String = ""
            Dim IngBruto As String = GIngBruto     '"Conv.Mult. Nª 901-898989-6"
            Dim FechaInicio As String = GFechaInicio
            '
            MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, LetraComprobante, GCondicionIvaEmpresa, IngBruto, FechaInicio, "C:\XML Afip\", e, False, TipoComprobante)
            Texto = FormatNumber(NumeroRetencion, 0)
            Dim PrintFont As System.Drawing.Font = New Font("Arial", 16)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, 25)
            Texto = Format(DtNotaCabeza.Rows(0).Item("Fecha"), "dd/MM/yyyy")
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, 35)
            ''''''''''''''''''''''''''''''''''''''''''''''''Fin Logo --------------------------------------------------
            'Encabezado.
            y = MTop
            x = MIzq
            y = y + SaltoLinea
            PrintFont = New Font("Courier New", 14, FontStyle.Bold)
            Texto = "CERTIFICADO DE RETENCION"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 50, y)
            '
            PrintFont = New Font("Courier New", 13, FontStyle.Bold)
            y = y + 5 * SaltoLinea
            Texto = "RETENCION : " & NombreRetencion
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = "FECHA : " & Format(DtNotaCabeza.Rows(0).Item("Fecha"), "dd/MM/yyyy")
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 120, y)
            y = y + SaltoLinea
            Texto = "NRO. CERTIFICADO : " & FormatNumber(NumeroRetencion, 0)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            y = y + 2 * SaltoLinea
            Texto = "AGENTE DE RETENCION : " & GNombreEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "CUIT.: " & GCuitEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + 2 * SaltoLinea
            Texto = "SUJETO DE RETENCION : " & NombreEmisor
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "CUIT.: " & Cuit
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            PrintFont = New Font("Courier New", 13)
            y = y + 4 * SaltoLinea
            Texto = "EN EL DIA DE LA FECHA HEMOS RETENIDO EL IMPORTE DE $ " & FormatNumber(ImporteRetencion, GDecimales)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "Son pesos " & Letras(ImporteRetencion.ToString)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            Dim Coeficiente As Decimal = 1 + HallaIvaDeCodigo(DtNotaCabeza.Rows(0).Item("CodigoIva")) / 100
            Dim PagadoActual As Decimal = CDec(DtNotaCabeza.Rows(0).Item("Importe")) / Coeficiente
            '
            y = y + 2 * SaltoLinea
            Texto = "PAGOS EFECTUADOS EN EL PRESENTE MES      "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(PagoNetoMesRetencion - PagadoActual, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "PAGOS ACTUAL                            "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(PagadoActual, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "---------------"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            '
            y = y + SaltoLinea
            Texto = "TOTAL PAGADO EN EL MES                  "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(PagoNetoMesRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "TOPE DE PAGOS SEGUN RESOLUCION          "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(TopeMesRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "---------------"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "DIFERENCIA A APLICAR PARA RETENCION     "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(PagoNetoMesRetencion - TopeMesRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + 2 * SaltoLinea
            Texto = "TOTAL A RETENER CON ALICUOTA DEL " & FormatNumber(AlicuotaRetencion, 2) & "%"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Dim Ret As Double = Trunca((PagoNetoMesRetencion - TopeMesRetencion) * AlicuotaRetencion / 100)
            Texto = FormatNumber(Ret, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "RETENCIONES EFECTUADAS EN ESTE MES        "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(RetencionPagadaMes - ImporteRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = "RETENCIONES ACTUAL                     "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(ImporteRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + 8 * SaltoLinea
            Texto = "FIRMA ........................"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 80, y)

            Paginas = Paginas + 1

            If Paginas < Copias Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
            e.HasMorePages = False
        End Try

    End Sub
    Private Sub Print_RetencionFormula4(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 60   '20
        Dim RowsBusqueda() As DataRow
        Dim Longi As Integer
        Dim LineaImporte As Integer = MIzq + 150
        Dim Xq As Integer

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            '--------------------------------------------------- Logo -----------------------------------------------
            Dim NombreEmpres As String = GNombreEmpresa '"Patagonia Sunrise S.R.L."
            Dim CuitEmpresa As String = GCuitEmpresa    '"30-12345678-5"
            Dim TipoComprobante As Integer = 14          '14. CERTIFICADO
            Dim Direccion1 As String = GDireccion1      '"Nave S-2 Puesto 34 - Mercado Central de Buenos Aires"
            Dim Direccion2 As String = GDireccion2      '"Autopista Ricchieri y Boulongne Sur Mer"
            Dim Direccion3 As String = GDireccion3      '"C.P.(1771) Villa Celina Pcia. de Buenos Aires"
            Dim LetraComprobante As String = ""
            Dim IngBruto As String = GIngBruto     '"Conv.Mult. Nª 901-898989-6"
            Dim FechaInicio As String = GFechaInicio
            '
            MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, LetraComprobante, GCondicionIvaEmpresa, IngBruto, FechaInicio, "C:\XML Afip\", e, False, TipoComprobante)
            Texto = FormatNumber(NumeroRetencion, 0)
            Dim PrintFont As System.Drawing.Font = New Font("Arial", 16)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, 25)
            Texto = Format(DtNotaCabeza.Rows(0).Item("Fecha"), "dd/MM/yyyy")
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, 35)
            ''''''''''''''''''''''''''''''''''''''''''''''''Fin Logo --------------------------------------------------
            'Encabezado.
            y = MTop
            x = MIzq
            y = y + SaltoLinea
            PrintFont = New Font("Courier New", 14, FontStyle.Bold)
            Texto = "CERTIFICADO DE RETENCION"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 50, y)
            '
            PrintFont = New Font("Courier New", 13, FontStyle.Bold)
            y = y + 5 * SaltoLinea
            Texto = "RETENCION : " & NombreRetencion
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = "FECHA : " & Format(DtNotaCabeza.Rows(0).Item("Fecha"), "dd/MM/yyyy")
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 120, y)
            y = y + SaltoLinea
            Texto = "NRO. CERTIFICADO : " & FormatNumber(NumeroRetencion, 0)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            y = y + 2 * SaltoLinea
            Texto = "AGENTE DE RETENCION : " & GNombreEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "CUIT.: " & GCuitEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + 2 * SaltoLinea
            Texto = "SUJETO DE RETENCION : " & NombreEmisor
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "CUIT.: " & Cuit
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            PrintFont = New Font("Courier New", 13)
            y = y + 4 * SaltoLinea
            Texto = "EN EL DIA DE LA FECHA HEMOS RETENIDO EL IMPORTE DE $ " & FormatNumber(ImporteRetencion, GDecimales)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            y = y + SaltoLinea
            Texto = "Son pesos " & Letras(ImporteRetencion.ToString)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            '
            Dim Coeficiente As Decimal = 1 + HallaIvaDeCodigo(DtNotaCabeza.Rows(0).Item("CodigoIva")) / 100
            Dim PagadoActual As Decimal = DtNotaCabeza.Rows(0).Item("Importe") / Coeficiente
            '
            y = y + 2 * SaltoLinea
            Texto = "PAGO ACTUAL                             "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(PagadoActual, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + SaltoLinea
            Texto = ""
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            '
            y = y + SaltoLinea
            Texto = "TOTAL A RETENER CON ALICUOTA DEL " & FormatNumber(AlicuotaRetencion, 2) & "%"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Texto = FormatNumber(ImporteRetencion, GDecimales)
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y)
            y = y + 8 * SaltoLinea
            Texto = "FIRMA ........................"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 80, y)

            Paginas = Paginas + 1

            If Paginas < Copias Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
            e.HasMorePages = False
        End Try

    End Sub
    Public Function GrabaImpreso(ByVal DtCabezaW As DataTable, ByVal ConexionAux As String) As Boolean

        Dim Sql As String = "UPDATE RecibosCabeza Set Impreso = 1 WHERE TipoNota = " & TipoNota & " AND Nota = " & DtCabezaW.Rows(0).Item("Nota") & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionAux)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                If CInt(Cmd.ExecuteNonQuery()) = 0 Then
                    MsgBox("Error, Base de Datos al Grabar Nota de Credito. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos al Grabar Nota de Credito. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End Try

        Return True

    End Function
    Private Function CantidadImputaciones() As Integer

        Dim Contador As Integer = 0

        For Each Row As DataGridViewRow In GridCompro.Rows
            If Row.Cells("Asignado").Value <> 0 Then Contador = Contador + 1
        Next

        Return Contador

    End Function
    Private Sub TotalesImprisionRetencionGanancia(ByVal row1 As DataRow)

        Dim Sql As String = "SELECT Importe,CodigoIva FROM RecibosCabeza WHERE TipoNota = 600 AND Estado <> 3 AND Emisor = " & DtNotaCabeza.Rows(0).Item("Emisor") & " AND MONTH(Fecha) = " & Month(DtNotaCabeza.Rows(0).Item("Fecha")) & " AND YEAR(Fecha) = " & Year(DtNotaCabeza.Rows(0).Item("Fecha")) & ";"

        Dim Dt As New DataTable
        Dim Row2 As DataRow

        PagoNetoMesRetencion = 0

        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        For Each Row2 In Dt.Rows
            Dim Coeficiente As Decimal = 1 + HallaIvaDeCodigo(Row2("CodigoIva")) / 100
            PagoNetoMesRetencion = PagoNetoMesRetencion + Row2("Importe") / Coeficiente
        Next

        Sql = "SELECT P.Importe AS Importe FROM RecibosCabeza AS C INNER JOIN RecibosDetallePago AS P ON C.TipoNota = P.TipoNota AND C.Nota = P.Nota " & _
              "WHERE C.TipoNota = 600 AND C.Estado <> 3 AND C.Emisor = " & DtNotaCabeza.Rows(0).Item("Emisor") & " AND MONTH(C.Fecha) = " & Month(DtNotaCabeza.Rows(0).Item("Fecha")) & " AND Year(C.Fecha) = " & Year(DtNotaCabeza.Rows(0).Item("Fecha")) & " AND P.MedioPago = " & row1("Clave") & ";"
        Dt = New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        RetencionPagadaMes = 0
        For Each Row2 In Dt.Rows
            RetencionPagadaMes = RetencionPagadaMes + Row2("Importe")
        Next
        Dt.Dispose()

    End Sub
    Public Function HallaAliasProveedor() As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Alias FROM Proveedores WHERE Clave = " & DtNotaCabeza.Rows(0).Item("Emisor") & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Proveedores.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaAliasCliente() As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Alias FROM Clientes WHERE Clave = " & DtNotaCabeza.Rows(0).Item("Emisor") & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Clientes.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    '-----------------------------------------------------------------------------------------------------------------------------
    '------------------------------------------------Impresion Facturas Cuenta Corriente---------------------------------------------------
    '-----------------------------------------------------------------------------------------------------------------------------
    Public Function UnaImpresionCtaCte() As Boolean

        TotalComprobante = 0
        TotalImpuesto = 0
        TotalSenia = 0
        TotalGralCtaCte = GridCompro.Rows(0).Cells("SaldoCta").Value

        If PtipoEmisor = 1 Then
            GeneraListaCtaCteCliente(GridCompro)
        Else
            GeneraListaCtaCteProveedor(GridCompro)
        End If

        ErrorImpresion = False
        Paginas = 0
        If Not Abierto Then Copias = 1

        Dim print_document As New PrintDocument
        UnSeteoImpresora.SeteaImpresion(print_document)

        print_document.DefaultPageSettings.Landscape = True

        PaginasImpresas = 0
        LineasParaImpresion = 0
        LineasParaImpresionImputacion = 0
        Copias = 1

        AddHandler print_document.PrintPage, AddressOf Print_PrintCtaCte
        print_document.Print()
        print_document.Dispose()

    End Function
    Private Sub Print_PrintCtaCte(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 5
        Dim LineasPorPagina As Integer = 47  '37
        Dim LineasImpresas As Integer
        Dim InprimioFinal As Boolean = False
        Dim Contador As Integer = 0

        Dim LineaFecha As Integer = MIzq
        Dim LineaTipo As Integer = MIzq + 23
        Dim LineaComprobante As Integer = MIzq + 57
        Dim LineaDebito As Integer = MIzq + 91
        Dim LineaCredito As Integer = MIzq + 122
        Dim LineaSaldoPeriodo As Integer = MIzq + 153
        Dim LineaEstado As Integer = MIzq + 249
        Dim TotalRenglonesHojas = 47
        'Para detalle factura.
        Dim LineaArticulo As Integer = MIzq + 59   '88
        Dim LineaCantidad As Integer = MIzq + 115  '134
        Dim LineaPrecio As Integer = MIzq + 131    '150
        Dim LineaImpuesto As Integer = MIzq + 146  '165
        Dim LineaSenia As Integer = MIzq + 166     '185
        Dim LineaTotal As Integer = MIzq + 186
        Dim LineaGral As Integer = MIzq + 206

        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        Try
            Print_TituloCtaCte(e, MTop, SaltoLinea, LineaFecha, LineaTipo, LineaComprobante, LineaArticulo, LineaCantidad, LineaPrecio, _
                                             LineaImpuesto, LineaSenia, LineaDebito, LineaCredito, LineaSaldoPeriodo, LineaTotal, LineaEstado, Contador)
            Contador = Contador + 1
            Print_DetalleCtaCte(e, MTop, SaltoLinea, LineasPorPagina, LineaFecha, LineaTipo, LineaComprobante, LineaArticulo, LineaCantidad, LineaPrecio, _
                                             LineaImpuesto, LineaSenia, LineaDebito, LineaCredito, LineaSaldoPeriodo, LineaTotal, LineaGral, LineaEstado, LineasImpresas, Contador)
            If LineasParaImpresion = Listado.Count And Contador + 4 < TotalRenglonesHojas Then
                Contador = Contador + 1
                Print_FinalCtaCte(e, MTop, SaltoLinea, TotalSaldoPeriodo - TotalGralCtaCte, TotalGralCtaCte, Contador)
                InprimioFinal = True
            End If

            If Not InprimioFinal Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
            e.HasMorePages = False
        End Try

    End Sub
    Private Sub Print_TituloCtaCte(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal Y1 As Integer, ByVal SaltoLinea As Integer, ByVal LineaFecha As Integer, ByVal LineaTipo As Integer, ByVal LineaComprobante As Integer, ByVal LineaArticulo As Integer, ByVal LineaCantidad As Integer, ByVal LineaPrecio As Integer, _
                                                         ByVal LineaImpuesto As Integer, ByVal LineaSenia As Integer, ByVal LineaDebito As Integer, ByVal LineaCredito As Integer, ByVal LineaSaldoPeriodo As Integer, ByVal LineaTotal As Integer, ByVal LineaEstado As Integer, ByRef Contador As Integer)

        Dim Texto As String = ""
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 10)
        Dim Y As Integer

        PaginasImpresas = PaginasImpresas + 1

        Texto = "Estado Cuenta Corriente del " & Format(PDesdeCtaCte, "dd/MM/yyyy") & " Hasta el " & Format(PHastaCtaCte, "dd/MM/yyy")
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaFecha, Y1 + Contador * SaltoLinea)
        Texto = "Pagina " & PaginasImpresas
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaFecha + 200, Y1 + Contador * SaltoLinea)
        Contador = Contador + 1
        If PtipoEmisor = 1 Then
            Texto = "Del Cliente: " & PNombreClienteCtaCte
        Else
            Texto = "Del Proveedor: " & PNombreClienteCtaCte
        End If
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaFecha, Y1 + Contador * SaltoLinea)
        Contador = Contador + 1
        Texto = "Expresada en: " & PMoneda
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaFecha, Y1 + Contador * SaltoLinea)
        '
        Contador = Contador + 2
        Y = Y1 + Contador * SaltoLinea
        Texto = "Fecha"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaFecha, Y)
        Texto = "Tipo"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaTipo, Y)
        Texto = "Comprobante"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaComprobante, Y)
        Texto = "Debito"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaDebito, Y)
        Texto = "Crédito"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCredito, Y)
        Texto = "Saldo Per."
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaSaldoPeriodo, Y)
        Texto = "Estado"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaEstado, Y)

    End Sub
    Private Sub Print_DetalleCtaCte(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal Y1 As Integer, ByVal SaltoLinea As Integer, ByVal LineasPorPagina As Integer, ByVal LineaFecha As Integer, ByVal LineaTipo As Integer, ByVal LineaComprobante As Integer, ByVal LineaArticulo As Integer, ByVal LineaCantidad As Integer, ByVal LineaPrecio As Integer, _
                                                ByVal LineaImpuesto As Integer, ByVal LineaSenia As Integer, ByVal LineaDebito As Integer, ByVal LineaCredito As Integer, ByVal LineaSaldoPeriodo As Integer, ByVal LineaTotal As Integer, ByVal LineaGral As Integer, ByVal LineaEstado As Integer, ByRef LineasImpresas As Integer, ByRef Contador As Integer)

        Dim Texto As String = ""
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 10)
        Dim Y As Integer

        While Contador < LineasPorPagina And LineasParaImpresion < Listado.Count
            Dim Item As New ItemCtaCte
            Item = Listado(LineasParaImpresion)
            Y = Y1 + Contador * SaltoLinea
            Select Case Strings.Left(Item.Articulo, 4)
                Case ""
                    PrintFont = New Font("Courier New", 10, FontStyle.Bold)
                    Texto = Item.Fecha
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaFecha, Y)
                    Texto = Strings.Left(Item.Tipo, 15)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaTipo, Y)
                    Texto = Item.Comprobante
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaComprobante, Y)
                    Texto = Format(Item.Debe, "#,###,###,##0.00")
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaDebito, Y)
                    Texto = Format(Item.Haber, "#,###,###,##0.00")
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCredito, Y)
                    Texto = Format(Item.SaldoPeriodo, "#,###,###,##0.00")
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaSaldoPeriodo, Y)
                    Texto = Item.Estado
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaEstado, Y)
                Case "Tota"
                    PrintFont = New Font("Courier New", 8, FontStyle.Bold)
                    Texto = Item.Articulo
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaArticulo, Y)
                    Texto = RellenaBlancos(Format(Item.Impuesto, "#,###,###,##0.00"), 10)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaImpuesto, Y)
                    Texto = RellenaBlancos(Format(Item.Senia, "#,###,###,##0.00"), 10)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaSenia, Y)
                    Texto = RellenaBlancos(Format(Item.Debe, "#,###,###,##0.00"), 10)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaTotal, Y)
                    Texto = RellenaBlancos(Format(Item.Total, "#,###,###,##0.00"), 10)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaGral, Y)
                Case "Desc"
                    PrintFont = New Font("Courier New", 8)
                    Texto = "Articulo"
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaArticulo, Y)
                    Texto = "Cant."
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad, Y)
                    Texto = "Precio"
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaPrecio, Y)
                    Texto = "I.V.A."
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaImpuesto, Y)
                    Texto = "Seña"
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaSenia, Y)
                    Texto = "Importe"
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaTotal, Y)
                    Texto = "Total Gral."
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaGral, Y)
                Case Else
                    PrintFont = New Font("Courier New", 8)
                    Texto = Strings.Left(Item.Articulo, 25)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaArticulo, Y)
                    Texto = RellenaBlancos(Format(Item.Cantidad, "#,###,###,##0.00"), 8)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad, Y)
                    Texto = RellenaBlancos(Format(Item.Precio, "#,###,###,##0.00"), 8)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaPrecio, Y)
                    Texto = RellenaBlancos(Format(Item.Impuesto, "#,###,###,##0.00"), 10)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaImpuesto, Y)
                    Texto = RellenaBlancos(Format(Item.Senia, "#,###,###,##0.00"), 10)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaSenia, Y)
                    Texto = RellenaBlancos(Format(Item.Debe, "#,###,###,##0.00"), 10)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaTotal, Y)
            End Select

            Contador = Contador + 1
            LineasParaImpresion = LineasParaImpresion + 1
        End While

    End Sub
    Private Sub Print_FinalCtaCte(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal Y1 As Integer, ByVal SaltoLinea As Integer, ByVal SaldoPeriodo As Decimal, ByVal SaldoGeneral As Decimal, ByRef contador As Integer)

        Dim Texto As String = ""
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 10)
        Dim Total As Decimal = SaldoGeneral + SaldoPeriodo

        Texto = "Saldo Anterior General: " & RellenaBlancosStr(Format(SaldoGeneral, "#,###,###,##0.00"), 20)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, 90, Y1 + contador * SaltoLinea)
        contador = contador + 1
        Texto = "Saldo Actual Periodo  : " & RellenaBlancosStr(Format(SaldoPeriodo, "#,###,###,##0.00"), 20)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, 90, Y1 + contador * SaltoLinea)
        contador = contador + 1
        Texto = "Saldo Actual General  : " & RellenaBlancosStr(Format(Total, "#,###,###,##0.00"), 20)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, 90, Y1 + contador * SaltoLinea)

    End Sub
    Private Function GeneraListaCtaCteCliente(ByVal GridCompro As DataGridView)

        Listado = New List(Of ItemCtaCte)

        TotalSaldoPeriodo = TotalGralCtaCte

        For Each Row As DataGridViewRow In GridCompro.Rows
            If Row.Index = 0 Or Row.Index = GridCompro.Rows.Count - 1 Then Continue For

            If Row.Cells("Estado").Value <> 3 Then
                TotalSaldoPeriodo += Row.Cells("Debito").Value - Row.Cells("Credito").Value
            End If

            EscribeCabezaNota(Row)

            If Not Row.Cells("Tipo").Value = -1 And Not Row.Cells("Tipo").Value = 2 Then Continue For

            Dim ListaArticulos As New List(Of ItemCtaCte)
            Dim FilaD As New ItemCtaCte
            FilaD.Articulo = "Descripcion"
            Listado.Add(FilaD)

            TotalSenia = 0
            TotalImpuesto = 0
            TotalComprobante = 0
            If Row.Cells("Tipo").Value = -1 Then
                TraeDatosRemito(Row.Cells("Comprobante").Value, Row.Cells("Operacion").Value, ListaArticulos)
                For Each Fila As ItemCtaCte In ListaArticulos
                    Dim FilaRemito As New ItemCtaCte
                    FilaRemito.Articulo = Fila.Articulo
                    FilaRemito.Cantidad = Fila.Cantidad
                    FilaRemito.Precio = Fila.Precio
                    FilaRemito.Debe = Fila.Debe
                    FilaRemito.Impuesto = Fila.Impuesto
                    FilaRemito.Senia = Fila.Senia
                    TotalComprobante = TotalComprobante + FilaRemito.Debe
                    Listado.Add(FilaRemito)
                Next
            End If
            If Row.Cells("Tipo").Value = 2 Then    'Procesa Factura.
                '------------------------------------
                Dim Percepciones As Decimal
                TraeDatosFactura(Row.Cells("Comprobante").Value, Row.Cells("Operacion").Value, ListaArticulos, TotalSenia, Percepciones)
                For Each Fila As ItemCtaCte In ListaArticulos  'Linea con detalle factura.
                    Dim FilaW As New ItemCtaCte
                    FilaW.Articulo = Fila.Articulo
                    FilaW.Cantidad = Fila.Cantidad
                    FilaW.Precio = Fila.Precio
                    FilaW.Impuesto = Fila.Impuesto   'Iva.
                    FilaW.Debe = CalculaNeto(FilaW.Cantidad, FilaW.Precio) + FilaW.Impuesto
                    FilaW.Senia = CalculaNeto(Fila.Cantidad, Fila.Senia)
                    Listado.Add(FilaW)
                    TotalImpuesto = TotalImpuesto + FilaW.Impuesto
                    TotalComprobante = TotalComprobante + FilaW.Debe
                Next
                If Percepciones <> 0 Then   'Linea con percepciones.
                    Dim FilaW As New ItemCtaCte
                    FilaW.Articulo = "Percepciones"
                    FilaW.Debe = Percepciones
                    Listado.Add(FilaW)
                    TotalComprobante = TotalComprobante + FilaW.Debe
                End If
                '----------------------------------
            End If
            EscribePieNota(TotalComprobante, TotalImpuesto, TotalSenia)
        Next

    End Function
    Private Function GeneraListaCtaCteProveedor(ByVal Grid As DataGridView)

        Listado = New List(Of ItemCtaCte)

        Dim Percepciones As Decimal
        Dim OperacionBl As Boolean
        Dim OperacionInt As Integer

        TotalSaldoPeriodo = TotalGralCtaCte

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Index = 0 Or Row.Index = GridCompro.Rows.Count - 1 Then Continue For

            If Row.Cells("Estado").Value <> 3 Then
                TotalSaldoPeriodo += Row.Cells("Debito").Value - Row.Cells("Credito").Value
            End If

            Dim ListaArticulos As New List(Of ItemCtaCte)

            TotalComprobante = 0
            TotalImpuesto = 0
            TotalSenia = 0
            DiferenciaTotales = 0
            Select Case Row.Cells("Tipo").Value
                Case 2
                    If Row.Cells("Operacion").Value = 1 Then OperacionBl = True
                    If Row.Cells("Operacion").Value = 2 Then OperacionBl = False

                    Dim Tipo As Integer = HallaTipofacturaProveedor(Row.Cells("Comprobante").Value, OperacionBl)
                    If Tipo <> 1 Then EscribeCabezaNota(Row) : Continue For

                    Dim TotalDetalleCompro As Decimal
                    Dim ValorTotal As Decimal
                    Dim Comprobante As Decimal = 0
                    Dim Relacion As Decimal = HallaDatosRelacionada(OperacionBl, Row.Cells("Comprobante").Value, ValorTotal, Comprobante)
                    Select Case Relacion
                        Case -10
                            Exit Function
                        Case -1
                            MsgBox("NO EXISTE COMPROBANTE!!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR!")
                        Case 0
                            If Row.Cells("Estado").Value <> 3 Then
                                TotalSaldoPeriodo += -(Row.Cells("Debito").Value - Row.Cells("Credito").Value)
                            End If
                            Continue For
                        Case 1, 10
                            EscribeCabezaNota(Row)
                            If Relacion = 10 And Row.Cells("Estado").Value <> 3 Then
                                Listado(Listado.Count - 1).SaldoPeriodo += -(ValorTotal - Listado(Listado.Count - 1).Haber)
                                TotalSaldoPeriodo += -(ValorTotal - Listado(Listado.Count - 1).Haber)
                            End If

                            Listado(Listado.Count - 1).Haber = ValorTotal
                    End Select

                    If Comprobante = 0 Then
                        TraeDatosFacturaProveedores(Row.Cells("Comprobante").Value, Row.Cells("ReciboOficial").Value, OperacionBl, ListaArticulos, TotalDetalleCompro)
                    Else
                        TraeDatosFacturaProveedores(Comprobante, Row.Cells("ReciboOficial").Value, Not OperacionBl, ListaArticulos, TotalDetalleCompro)
                    End If

                    If TotalDetalleCompro <> ValorTotal Then DiferenciaTotales = ValorTotal - TotalDetalleCompro
                Case 10
                    EscribeCabezaNota(Row)
                    TraeDatosLiquidacion(Row.Cells("Comprobante").Value, Row.Cells("Operacion").Value, ListaArticulos)
                Case Else
                    EscribeCabezaNota(Row) : Continue For
            End Select

            For Each FilaArticulo As ItemCtaCte In ListaArticulos
                Dim FilaComprobante As New ItemCtaCte
                FilaComprobante.Articulo = FilaArticulo.Articulo
                FilaComprobante.Cantidad = FilaArticulo.Cantidad
                FilaComprobante.Precio = FilaArticulo.Precio
                FilaComprobante.Impuesto = FilaArticulo.Impuesto
                FilaComprobante.Senia = FilaArticulo.Senia
                FilaComprobante.Debe = FilaArticulo.Debe
                Listado.Add(FilaComprobante)

                If Row.Cells("Tipo").Value <> 10 Then
                    TotalSenia = TotalSenia + FilaComprobante.Senia
                    TotalComprobante = TotalComprobante + FilaComprobante.Debe
                End If

            Next
            EscribePieNota(TotalComprobante, TotalImpuesto, TotalSenia)

        Next

        ReciboOficialAnt.Clear()

    End Function
    Private Sub EscribeCabezaNota(ByVal Row As DataGridViewRow)

        Dim Item As New ItemCtaCte
        Item.Fecha = Row.Cells("Fecha").FormattedValue
        Item.Tipo = Strings.Left(Row.Cells("Tipo").FormattedValue, 15)
        If Row.Cells("ReciboOficial").FormattedValue <> "" Then
            Item.Comprobante = Row.Cells("ReciboOficial").FormattedValue
        Else
            Item.Comprobante = Row.Cells("Comprobante").FormattedValue
        End If
        Item.Debe = Row.Cells("Debito").Value
        Item.Haber = Row.Cells("Credito").Value
        Item.Estado = Row.Cells("Estado").FormattedValue

        Item.SaldoPeriodo = TotalSaldoPeriodo

        Listado.Add(Item)

    End Sub
    Private Sub EscribePieNota(ByVal TotalComprobante As Decimal, ByVal TotalImpuesto As Decimal, ByVal TotalSenia As Decimal)

        Dim FilaT As New ItemCtaCte
        FilaT.Articulo = "Totales: "
        FilaT.Impuesto = TotalImpuesto
        If TotalSenia <> 0 Then
            FilaT.Senia = TotalSenia
        End If
        FilaT.Debe = TotalComprobante
        FilaT.Total = FilaT.Debe + FilaT.Senia + DiferenciaTotales
        Listado.Add(FilaT)

    End Sub
    Private Sub TraeDatosFactura(ByVal Factura As Decimal, ByVal Operacion As Integer, ByRef Lista As List(Of ItemCtaCte), ByRef SeniaCabeza As Decimal, ByRef Percepciones As Decimal)

        Dim DT As New DataTable
        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT C.Senia AS SeniaCabeza,C.Percepciones,C.EsServicios, D.Articulo,D.Cantidad,D.Precio,D.TotalArticulo,D.Senia,D.Iva FROM FacturasCabeza AS C INNER JOIN FacturasDetalle AS D ON C.Factura = D.Factura WHERE C.Factura = " & Factura & ";", ConexionStr, DT) Then End
        For Each Row As DataRow In DT.Rows
            Dim Item As New ItemCtaCte
            If Row("EsServicios") Then
                Item.Articulo = NombreArticuloServicios(Row("Articulo"))
            Else
                Item.Articulo = NombreArticulo(Row("Articulo"))
            End If
            Item.Cantidad = Row("Cantidad")
            Item.Precio = Row("Precio")
            Item.Senia = Row("Senia")
            Item.Impuesto = CalculaIva(Row("Cantidad"), Row("Precio"), Row("Iva"))
            Item.Total = Row("TotalArticulo")
            Lista.Add(Item)
        Next

        SeniaCabeza = DT.Rows(0).Item("SeniaCabeza")
        Percepciones = DT.Rows(0).Item("Percepciones")

        DT.Dispose()

    End Sub
    Private Sub TraeDatosRemito(ByVal Remito As Decimal, ByVal Operacion As Integer, ByRef Lista As List(Of ItemCtaCte))

        Dim DT As New DataTable
        Dim ConexionStr As String
        Dim PrecioSinIva As Decimal = 0
        Dim PrecioFinal As Decimal = 0
        Dim Debe As Decimal = 0
        Dim Impuesto As Decimal = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT Articulo,(Cantidad-Devueltas) As TotalCantidad,Precio,TipoPrecio,KilosXUnidad FROM RemitosDetalle WHERE Remito = " & Remito & ";", ConexionStr, DT) Then End
        For Each Fila As DataRow In DT.Rows
            Dim Item As New ItemCtaCte
            Item.Articulo = NombreArticulo(Fila("Articulo"))
            Item.Cantidad = Fila("TotalCantidad")

            PrecioSinIva = FormatNumber(HallaPrecioSinIva(Fila("Articulo"), Fila("Precio")), 5)
            Item.Precio = PrecioSinIva

            If Fila("TipoPrecio") = 2 Then
                Item.Precio = Item.Precio * Fila("KilosXUnidad")
                PrecioFinal = Fila("Precio") * Fila("KilosXUnidad")
            Else
                PrecioFinal = Fila("Precio")
            End If

            If Operacion = 2 Then PrecioFinal = Item.Precio

            Item.Debe = CalculaNeto(Fila("TotalCantidad"), PrecioFinal)
            Debe = CalculaNeto(Fila("TotalCantidad"), Item.Precio)
            Item.Impuesto = Item.Debe - Debe

            Lista.Add(Item)
        Next

        DT.Dispose()

    End Sub
    Private Sub TraeDatosFacturaProveedores(ByVal Factura As Decimal, ByVal ReciboOficial As Decimal, ByVal Operacion As Boolean, ByRef Lista As List(Of ItemCtaCte), ByRef TotalDetalleCompro As Decimal)

        Dim ConexionStr As String
        Dim DT As New DataTable

        Dim FilaD As New ItemCtaCte
        FilaD.Articulo = "Descripcion"
        Listado.Add(FilaD)

        ConexionStr = ConexionN
        If Operacion Then ConexionStr = Conexion

        If Not Tablas.Read("SELECT Lote,Secuencia,0 As Articulo,Importe,ImporteConIva,ImporteSinIva,Senia,Operacion FROM ComproFacturados WHERE Factura = " & Factura & ";", ConexionStr, DT) Then End

        TotalDetalleCompro = 0
        For Each Fila As DataRow In DT.Rows
            Dim Item As New ItemCtaCte
            If Not HallaDatosLotes(Fila.Item("Operacion"), Fila.Item("Lote"), Fila.Item("Secuencia"), Item.Cantidad, Nothing, Fila.Item("Articulo"), Item.Precio) Then End
            Item.Articulo = NombreArticulo(Fila.Item("Articulo"))
            Item.Senia = Fila.Item("Senia")
            Item.Debe = Fila.Item("Importe")
            Lista.Add(Item)
            TotalDetalleCompro += Item.Senia + Item.Debe
        Next

        DT.Dispose()

    End Sub
    Private Sub TraeDatosLiquidacion(ByVal Liquidacion As Double, ByVal Operacion As Integer, ByRef Lista As List(Of ItemCtaCte))

        Dim ConexionStr As String
        Dim DT As New DataTable

        Dim FilaD As New ItemCtaCte
        FilaD.Articulo = "Descripcion"
        Listado.Add(FilaD)

        ConexionStr = ConexionN
        If Operacion = 1 Then ConexionStr = Conexion

        If Not Tablas.Read("SELECT 0 As Articulo,D.Cantidad,0 As PrecioF,D.Operacion,D.Lote,D.Secuencia,C.Senia,D.Precio,C.Neto,C.Tr FROM LiquidacionCabeza C, LiquidacionDetalle D WHERE D.Liquidacion = " & Liquidacion & " AND C.Liquidacion = D.Liquidacion;", ConexionStr, DT) Then End

        For Each Fila As DataRow In DT.Rows
            Dim Item As New ItemCtaCte

            If Fila.Item("Tr") Then
                Item.Articulo = NombreArticulo(Fila.Item("Secuencia"))
                Item.Precio = Fila.Item("Precio")
                Item.Debe = Fila.Item("Cantidad") * Fila.Item("Precio")
            Else
                If Not HallaDatosLotes(Fila.Item("Operacion"), Fila.Item("Lote"), Fila.Item("Secuencia"), Nothing, Nothing, Fila.Item("Articulo"), Fila.Item("PrecioF")) Then End
                Item.Articulo = NombreArticulo(Fila.Item("Articulo"))
                Item.Precio = Fila.Item("PrecioF")
                Item.Debe = Fila.Item("Cantidad") * Fila.Item("PrecioF")
            End If

            Item.Cantidad = Fila.Item("Cantidad")
            Lista.Add(Item)

            TotalSenia = Fila.Item("Senia")
            TotalComprobante = Fila.Item("Neto")
        Next

        DT.Dispose()

    End Sub
    Private Function HallaDatosRelacionada(ByVal Operacion As Boolean, ByVal Comprobante As Decimal, ByRef ValorTotal As Decimal, ByRef NRel As Decimal) As Integer

        Dim DT As New DataTable
        Dim DTRel As New DataTable
        Dim ConexionStr As String = ConexionN

        If Operacion Then ConexionStr = Conexion

        If Not Tablas.Read("SELECT Importe,Rel,NRel FROM FacturasProveedorCabeza WHERE Factura = " & Comprobante & ";", ConexionStr, DT) Then Return -10

        If DT.Rows.Count = 0 Then Return -1

        If Not DT.Rows(0).Item("Rel") Then ValorTotal = DT.Rows(0).Item("Importe") : Return 1

        If DT.Rows(0).Item("Rel") And DT.Rows(0).Item("NRel") <> 0 Then
            ValorTotal = DT.Rows(0).Item("Importe")

            If Not PEsAmbasOpr Then NRel = 0 : Return 1

            If Not Tablas.Read("SELECT Importe FROM FacturasProveedorCabeza WHERE Factura = " & DT.Rows(0).Item("NRel") & ";", Conexion, DTRel) Then End
            If DTRel.Rows.Count = 0 Then Return -1

            ValorTotal += DTRel.Rows(0).Item("Importe")
            Return 10
        Else
            If PEsAmbasOpr Then Return 0

            If Not Tablas.Read("SELECT Factura,Importe FROM FacturasProveedorCabeza WHERE NRel = " & Comprobante & ";", ConexionN, DTRel) Then End
            If DTRel.Rows.Count = 0 Then Return -1

            ValorTotal = DT.Rows(0).Item("Importe")
            NRel = DTRel.Rows(0).Item("Factura")

            Return 1
        End If

    End Function
    Public Function HallaDatosLotes(ByVal Operacion As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef Ingreso As Double, ByRef Fecha As Date, ByRef Articulo As Integer, ByRef Precio As Decimal) As Boolean

        Dim Dt As New DataTable
        Dim ConexionLote As String

        If Operacion = 1 Then
            ConexionLote = Conexion
        Else : ConexionLote = ConexionN
        End If

        If Not Tablas.Read("SELECT Cantidad,Baja,Fecha,Articulo,PrecioF FROM Lotes WHERE DepositoOrigen = Deposito AND Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", ConexionLote, Dt) Then Return False
        If Dt.Rows.Count = 0 Then Return False
        Ingreso = Dt.Rows(0).Item("Cantidad") - Dt.Rows(0).Item("Baja")
        Fecha = Dt.Rows(0).Item("Fecha")
        Articulo = Dt.Rows(0).Item("Articulo")
        Precio = Dt.Rows(0).Item("PrecioF")

        Dt.Dispose()
        Return True

    End Function
    '-----------------------------------------------------------------------------------------------------------------------------
    '------------------------------------------------Impresion Saldo Lotes--------------------------------------------------------
    '-----------------------------------------------------------------------------------------------------------------------------
    Public Function UnaImpresionSaldoLotes() As Boolean

        ErrorImpresion = False
        Paginas = 0
        If Not Abierto Then Copias = 1

        Dim print_document As New PrintDocument

        LineasParaImpresion = 0
        LineasParaImpresionImputacion = 0
        Copias = 1

        AddHandler print_document.PrintPage, AddressOf Print_PrintSaldoLotes
        print_document.Print()

    End Function
    Private Sub Print_PrintSaldoLotes(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 8  '5
        Dim MTop As Integer = 20
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 37
        Dim LineasImpresas As Integer
        '
        Dim LineaLote As Integer = MIzq
        Dim LineaArticulo As Integer = MIzq + 21
        Dim LineaProveedor As Integer = MIzq + 81
        Dim LineaIngreso As Integer = MIzq + 128
        Dim LineaInicial As Integer = MIzq + 146
        Dim LineaStock As Integer = MIzq + 169

        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        Try
            Print_TituloSaldoLotes(e, MTop + 5, SaltoLinea, LineaLote, LineaArticulo, LineaProveedor, LineaIngreso, LineaInicial, LineaStock)
            Print_DetalleSaldoLotes(e, MTop + 7, SaltoLinea, LineaLote, LineaArticulo, LineaProveedor, LineaIngreso, LineaInicial, LineaStock, _
                                              LineasImpresas)
            If LineasParaImpresion < GridCompro.Rows.Count Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
            e.HasMorePages = False
        End Try

    End Sub
    Private Sub Print_TituloSaldoLotes(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal Y As Integer, ByVal SaltoLinea As Integer, ByVal LineaLote As Integer, ByVal LineaArticulo As Integer, ByVal LineaProveedor As Integer, ByVal LineaIngreso As Integer, ByVal LineaInicial As Integer, ByVal LineaStock As Integer)

        Dim Texto As String = ""
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 10)

        Texto = "Informe Stock de Lotes Desde el " & Format(PDesdeCtaCte, "dd/MM/yyyy") & " al " & Format(PHastaCtaCte, "dd/MM/yyy")
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaLote, Y - 3 * SaltoLinea)
        Texto = "Lote"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaLote, Y)
        Texto = "Articulo"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaArticulo, Y)
        Texto = "Proveedor"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaProveedor, Y)
        Texto = "Ingreso"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaIngreso, Y)
        Texto = "Inicial"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaInicial, Y)
        Texto = "Stock"
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaStock, Y)

    End Sub
    Private Sub Print_DetalleSaldoLotes(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal Y As Integer, ByVal SaltoLinea As Integer, ByVal Linealote As Integer, ByVal Lineaarticulo As Integer, ByVal Lineaproveedor As Integer, ByVal Lineaingreso As Integer, ByVal Lineainicial As Integer, ByVal Lineastock As Integer, _
                                               ByRef LineasImpresas As Integer)

        Dim Texto As String = ""
        Dim Contador As Integer = 0
        Dim LineasPorPagina As Integer = 61
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 9)

        '' Yq = y + SaltoLinea
        While Contador < LineasPorPagina And LineasParaImpresion < GridCompro.Rows.Count
            Dim Row As DataGridViewRow = GridCompro.Rows(LineasParaImpresion)
            Y = Y + SaltoLinea
            Texto = RellenaBlancosStr(Row.Cells("LoteYSecuencia").FormattedValue, 10)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Linealote, Y)
            Texto = Strings.Left(Row.Cells("Articulo").FormattedValue, 30)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Lineaarticulo, Y)
            Texto = Strings.Left(Row.Cells("Proveedor").FormattedValue, 20)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Lineaproveedor, Y)
            If Row.Cells("Fecha").FormattedValue <> "" Then
                Texto = Format(Row.Cells("Fecha").Value, "dd/MM/yy")
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Lineaingreso, Y)
            End If
            If Not IsDBNull(Row.Cells("Cantidad").Value) Then
                Texto = RellenaBlancos(Row.Cells("Cantidad").Value, 11)
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Lineainicial, Y)
            End If
            Texto = RellenaBlancos(Row.Cells("Stock").Value, 11)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Lineastock, Y)
            Contador = Contador + 1
            LineasParaImpresion = LineasParaImpresion + 1
        End While

    End Sub
    Private Function RellenaBlancos(ByVal Dato As Decimal, ByVal Longitud As Integer) As String

        Dim Str As String

        If Dato = 0 Then
            Str = ""
        Else
            Str = Format(Dato, "0.00")
        End If

        If Str.Length > Longitud Then Return Strings.Left(Str, Longitud)

        For i As Integer = 1 To Longitud - Str.Length
            Str = " " & Str
        Next

        Return Str

    End Function
    Private Function RellenaBlancosStr(ByVal Dato As String, ByVal Longitud As Integer) As String

        If Dato = "" Then Return ""

        If Dato.Length > Longitud Then Return Strings.Right(Dato, Longitud)

        Dim Str As String = Dato

        For i As Integer = 1 To Longitud - Dato.Length
            Str = " " & Str
        Next

        Return Str

    End Function
End Module
