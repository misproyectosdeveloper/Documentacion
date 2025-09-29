Module PlantillaReciboSueldo

    Public DTDetalle As New DataTable
    Public VectorCabeza(12) As String
    Public VectorTotales(4) As Decimal
    Public ObraSocial As String
    Public FechaRecibo As String
    Public TipoReciboStr As String

    Dim ImpresionDocumento As New System.Drawing.Printing.PrintDocument()
    Dim PrintPenLineas As Pen
    Dim PrintFontStr As Font
    Dim PosX As Decimal = 10
    Dim PosY As Decimal = 10
    Dim Margen As Decimal
    Dim VectorLineas(6) As Integer
    Dim VectorTitulos(6) As String
    Dim I As Integer
    Dim EsEmpleador As Boolean
    Dim DistanciaLineas As Decimal
    Dim TotalNeto As Decimal
    Dim NumeroFormateado As String
    Dim ultimo_dia As String
    Dim MesRecibo As Integer
    Dim AnioRecibo As Integer
    Dim LongitudStr As Single
    Dim TextoPesos As String = ""
    Dim TextoPesos2 As String = ""
    Dim DistanciaLineaAnt As Single
    Dim FueImpreso As Boolean

    Private Const AnchoHoja As Single = 132.0F
    Private Const LargoHoja As Single = 180.0F

    Public Sub IniciaImpresion()

        Dim index As Integer = 0

        For index = 0 To VectorCabeza.Length - 1
            VectorCabeza(index) = ""
        Next
        For index = 0 To VectorTotales.Length - 1
            VectorTotales(index) = 0
        Next
        For index = 0 To VectorLineas.Length - 1
            VectorLineas(index) = 0
        Next
        For index = 0 To VectorTitulos.Length - 1
            VectorTitulos(index) = ""
        Next

        PrintPenLineas = New Pen(Color.Black, 0.3)
        PrintFontStr = New Font("Courier New", 6.5)
        '''''PrintFontStr = New Font("Courier New", 8)

        PosX = 10
        PosY = 10
        Margen = 0
        I = 0
        EsEmpleador = False
        DistanciaLineas = 0
        TotalNeto = 0
        NumeroFormateado = ""
        ultimo_dia = ""
        MesRecibo = 0
        AnioRecibo = 0
        LongitudStr = 0
        TextoPesos = ""
        TextoPesos2 = ""
        DistanciaLineaAnt = 0
        FueImpreso = False
        FechaRecibo = ""

        CreaDTDetalle(DTDetalle)

    End Sub

    Private Sub Print_Recibo(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        If FueImpreso Then Exit Sub

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        MesRecibo = CInt(UnReciboSueldo.TextMes.Text)
        AnioRecibo = CInt(UnReciboSueldo.TextAnio.Text)

        If FechaRecibo = "" Then
            FechaRecibo = (New DateTime(AnioRecibo, MesRecibo, 1)).AddMonths(1).AddDays(-1).Day & "/" & Format(MesRecibo, "00") & "/" & AnioRecibo
        Else
            FechaRecibo = FechaRecibo.Substring(0, 2) & "/" & FechaRecibo.Substring(2, 2) & "/" & FechaRecibo.Substring(4, 4)
        End If

        TextoPesos = "Son Pesos: " & MontoALetras.Letras(VectorTotales(3))

        For J As Integer = 1 To 2
            e.Graphics.DrawRectangle(PrintPenLineas, PosX, PosY, AnchoHoja, LargoHoja)

            If J = 1 Then
                EsEmpleador = True
            Else
                EsEmpleador = False
            End If

            DibujaEncabezado(e)
            DibujaCabeza(e)
            DibujaDetalle(e)
            DibujaPiePagina(e)

            PosX += AnchoHoja + 5.0F
            PosY = 10.0F
            Margen = AnchoHoja + 5.0F
        Next

        FueImpreso = True

    End Sub
    Private Sub DibujaEncabezado(ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim Calle As String
        Dim Numero As String
        Dim Localidad As String
        Dim Provincia As String
        Dim CodigoPostal As String

        If Not HallaDatosEmpresa(Calle, Numero, Localidad, Provincia, CodigoPostal) Then Exit Sub
        Dim Direccion As String = Calle
        If Numero <> 0 Then Direccion = Direccion + " " + Numero
        If CodigoPostal <> "" Then Direccion = Direccion + " -" + CodigoPostal + "- "
        Direccion = Direccion + "  " + HallaNombreProvincia(Provincia)

        e.Graphics.DrawString(GNombreEmpresa, PrintFontStr, Brushes.Black, PosX + 3.0F, PosY + 4.0F)
        e.Graphics.DrawString(Direccion, PrintFontStr, Brushes.Black, PosX + 3.0F, PosY + 8.0F)
        e.Graphics.DrawString("C.U.I.T.: " & GCuitEmpresa, PrintFontStr, Brushes.Black, PosX + 3.0F, PosY + 12.0F)
        e.Graphics.DrawString("RECIBO DE REMUNERACIONES", PrintFontStr, Brushes.Black, PosX + 95.0F, PosY + 2.0F)
        e.Graphics.DrawString("Período: " & TipoReciboStr, PrintFontStr, Brushes.Black, PosX + 92.0F, PosY + 16.0F)

        e.Graphics.DrawLine(PrintPenLineas, PosX, PosY + 20.0F, PosX + AnchoHoja, PosY + 20.0F)

        PosY += 20.0F

    End Sub
    Private Sub DibujaCabeza(ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        VectorLineas(0) = 0
        VectorLineas(1) = PosX - Margen
        VectorLineas(2) = VectorLineas(1) + 37
        VectorLineas(3) = VectorLineas(2) + 22
        VectorLineas(4) = VectorLineas(3) + 19
        VectorLineas(5) = VectorLineas(4) + 22
        VectorLineas(6) = VectorLineas(5) + 22
        '
        VectorTitulos(0) = ""
        VectorTitulos(1) = "Legajo"
        VectorTitulos(2) = "Apellido y Nombres"
        VectorTitulos(3) = "C.U.I.L."
        VectorTitulos(4) = "Fecha Ing."
        VectorTitulos(5) = "Sector"
        VectorTitulos(6) = "Sueldo/Jrnal"


        For I = 1 To 6
            DistanciaLineaAnt = VectorLineas(I) - VectorLineas(I - 1)

            LongitudStr = e.Graphics.MeasureString(VectorTitulos(I), PrintFontStr).Width
            DistanciaLineas = DistanciaLineaAnt / 2 - LongitudStr / 2 ' DIBUJA LINEA
            e.Graphics.DrawString(VectorTitulos(I), PrintFontStr, Brushes.Black, PosX + VectorLineas(I - 1) + DistanciaLineas, PosY + 0.5F)

            LongitudStr = e.Graphics.MeasureString(VectorCabeza(I), PrintFontStr).Width
            DistanciaLineas = DistanciaLineaAnt / 2 - LongitudStr / 2 ' ESCRIBE DATOS
            e.Graphics.DrawString(VectorCabeza(I), PrintFontStr, Brushes.Black, PosX + VectorLineas(I - 1) + DistanciaLineas, PosY + 5.5F)

            e.Graphics.DrawLine(PrintPenLineas, PosX + VectorLineas(I), PosY, PosX + VectorLineas(I), PosY + 10)
        Next

        e.Graphics.DrawLine(PrintPenLineas, PosX, PosY + 10.0F, PosX + AnchoHoja, PosY + 10.0F)
        e.Graphics.DrawString("ULTIMO DEPOSITO", PrintFontStr, Brushes.Black, PosX + 16.0F, PosY + 10.25F)
        e.Graphics.DrawString("LIQUIDACION", PrintFontStr, Brushes.Black, PosX + 83.0F, PosY + 10.25F)
        e.Graphics.DrawLine(PrintPenLineas, PosX, PosY + 13.0F, PosX + AnchoHoja, PosY + 13.0F)

        VectorLineas(0) = 0
        VectorLineas(1) = PosX - Margen
        VectorLineas(2) = VectorLineas(1) + 13
        VectorLineas(3) = VectorLineas(2) + 34
        VectorLineas(4) = VectorLineas(3) + 36
        VectorLineas(5) = VectorLineas(4) + 39
        '
        VectorTitulos(0) = ""
        VectorTitulos(1) = "Fecha"
        VectorTitulos(2) = "Periodo"
        VectorTitulos(3) = "Banco"
        VectorTitulos(4) = "Categoria"
        VectorTitulos(5) = "Función"

        Dim PosicionVector As Integer = 0

        For I = 1 To 5
            PosicionVector = 6 + I

            DistanciaLineaAnt = VectorLineas(I) - VectorLineas(I - 1)

            LongitudStr = e.Graphics.MeasureString(VectorTitulos(I), PrintFontStr).Width
            DistanciaLineas = DistanciaLineaAnt / 2 - LongitudStr / 2 ' ESCRIBE TITULOS
            e.Graphics.DrawString(VectorTitulos(I), PrintFontStr, Brushes.Black, PosX + VectorLineas(I - 1) + DistanciaLineas, PosY + 13.5F)

            LongitudStr = e.Graphics.MeasureString(VectorCabeza(PosicionVector), PrintFontStr).Width
            DistanciaLineas = DistanciaLineaAnt / 2 - LongitudStr / 2 ' ESCRIBE DATOS
            Select Case I
                Case 1
                    e.Graphics.DrawString(VectorCabeza(PosicionVector), PrintFontStr, Brushes.Black, PosX + VectorLineas(I - 1) + DistanciaLineas, PosY + 18.5F)
                Case 2
                    e.Graphics.DrawString(VectorCabeza(PosicionVector), PrintFontStr, Brushes.Black, PosX + VectorLineas(I - 1) + DistanciaLineas, PosY + 18.5F)
                Case 3, 4, 5
                    e.Graphics.DrawString(VectorCabeza(PosicionVector), PrintFontStr, Brushes.Black, PosX + VectorLineas(I - 1) + 2, PosY + 18.5F)
            End Select

            If I = 3 Then ' DIBUJA LINEAS
                e.Graphics.DrawLine(PrintPenLineas, PosX + VectorLineas(I), PosY + 10, PosX + VectorLineas(I), PosY + 23)
            Else
                e.Graphics.DrawLine(PrintPenLineas, PosX + VectorLineas(I), PosY + 13, PosX + VectorLineas(I), PosY + 23)
            End If
        Next

        PosY += 23.0F
        e.Graphics.DrawLine(PrintPenLineas, PosX, PosY, PosX + AnchoHoja, PosY)

    End Sub
    Private Sub DibujaDetalle(ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        VectorLineas(0) = PosX - Margen
        VectorLineas(1) = VectorLineas(0) + 44
        VectorLineas(2) = VectorLineas(1) + 15
        VectorLineas(3) = VectorLineas(2) + 21
        VectorLineas(4) = VectorLineas(3) + 21
        VectorLineas(5) = VectorLineas(4) + 21
        '
        VectorTitulos(0) = "Cod"
        VectorTitulos(1) = "CONCEPTO"
        VectorTitulos(2) = "Unidades"
        VectorTitulos(3) = "Remunerativo"
        VectorTitulos(4) = "No Remunera."
        VectorTitulos(5) = "Descuentos"
        '

        For I = 0 To 5
            If I = 0 Then
                DistanciaLineas = VectorLineas(I) / 2 - VectorTitulos(I).Length / 2
                e.Graphics.DrawString(VectorTitulos(I), PrintFontStr, Brushes.Black, Margen + (VectorLineas(I) + DistanciaLineas) - 1, PosY + 0.5F)
            Else
                DistanciaLineas = (VectorLineas(I) - VectorLineas(I - 1)) / 2 - VectorTitulos(I).Length / 2 ' CALCULA EN EL MEDIO DE LA COLUMNA.
                e.Graphics.DrawString(VectorTitulos(I), PrintFontStr, Brushes.Black, (PosX + VectorLineas(I - 1) + DistanciaLineas) - 2, PosY + 0.5F)
            End If

            EscribeDetalle(e, I)

            e.Graphics.DrawLine(PrintPenLineas, PosX + VectorLineas(I), PosY, PosX + VectorLineas(I), PosY + 78)
        Next

        e.Graphics.DrawLine(PrintPenLineas, PosX, PosY + 78.0F, PosX + AnchoHoja, PosY + 78.0F)

        PosY += 78

    End Sub
    Private Sub EscribeDetalle(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal Columna As Integer)

        Dim DistanciaRenglon As Single = 6.0F

        For Fila As Integer = 0 To DTDetalle.Rows.Count - 1
            NumeroFormateado = " "
            Select Case Columna
                Case 0
                    NumeroFormateado = DTDetalle.Rows(Fila).Item(Columna).ToString.PadLeft(4, "0")
                    DistanciaLineas = VectorLineas(Columna) / 2 - NumeroFormateado.ToString.Length / 2
                    e.Graphics.DrawString(NumeroFormateado, PrintFontStr, Brushes.Black, Margen + (VectorLineas(Columna) + DistanciaLineas) - 1, PosY + DistanciaRenglon)
                Case 1
                    e.Graphics.DrawString(DTDetalle.Rows(Fila).Item(Columna), PrintFontStr, Brushes.Black, PosX + VectorLineas(Columna - 1) + 1, PosY + DistanciaRenglon)
                Case Else
                    If DTDetalle.Rows(Fila).Item(Columna) <> 0 Then NumeroFormateado = FormatNumber(DTDetalle.Rows(Fila).Item(Columna), 2)

                    LongitudStr = e.Graphics.MeasureString(NumeroFormateado, PrintFontStr).Width + 1
                    e.Graphics.DrawString(NumeroFormateado, PrintFontStr, Brushes.Black, (PosX + VectorLineas(Columna)) - LongitudStr, PosY + DistanciaRenglon)
            End Select
            DistanciaRenglon += 3.0F
            If Fila = DTDetalle.Rows.Count - 1 Then DistanciaRenglon = 6.0F
        Next

    End Sub
    Private Sub DibujaPiePagina(ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim TextoFirma As String = "Firma"
        Dim LongitudStr As Single

        e.Graphics.DrawLine(PrintPenLineas, PosX, PosY + 10.0F, PosX + AnchoHoja, PosY + 10.0F)

        VectorLineas(0) = PosX + 58 - Margen
        VectorLineas(1) = VectorLineas(0) + 21
        VectorLineas(2) = VectorLineas(1) + 21
        VectorLineas(3) = VectorLineas(2) + 21

        For I = 0 To 2
            e.Graphics.DrawLine(PrintPenLineas, PosX + VectorLineas(I), PosY + 10, PosX + VectorLineas(I), PosY + 16)

            NumeroFormateado = " "
            If VectorTotales(I) <> 0 Then NumeroFormateado = FormatNumber(VectorTotales(I), 2)

            LongitudStr = e.Graphics.MeasureString(NumeroFormateado, PrintFontStr).Width + 1
            e.Graphics.DrawString(NumeroFormateado, PrintFontStr, Brushes.Black, PosX + VectorLineas(I + 1) - LongitudStr, PosY + 12.0F)
        Next

        e.Graphics.DrawString("TOTALES", PrintFontStr, Brushes.Black, PosX + 55.0F, PosY + 12.0F)
        e.Graphics.DrawString("NETO", PrintFontStr, Brushes.Black, PosX + 100.0F, PosY + 18.0F)

        e.Graphics.DrawLine(PrintPenLineas, PosX + 68, PosY + 16.0F, PosX + AnchoHoja, PosY + 16.0F)
        e.Graphics.DrawLine(PrintPenLineas, PosX + 110, PosY + 16.0F, PosX + 110.0F, PosY + 22.0F)
        e.Graphics.DrawLine(PrintPenLineas, PosX + 110, PosY + 22.0F, PosX + AnchoHoja, PosY + 22.0F)

        NumeroFormateado = FormatNumber(VectorTotales(3), 2)
        LongitudStr = e.Graphics.MeasureString(NumeroFormateado, PrintFontStr).Width + 1
        e.Graphics.DrawString(NumeroFormateado, PrintFontStr, Brushes.Black, PosX + VectorLineas(3) - LongitudStr, PosY + 18.0F)

        e.Graphics.DrawString(ObraSocial, PrintFontStr, Brushes.Black, PosX + 2, PosY + 20.0F)

        e.Graphics.DrawString("Lugar y Fecha de Pago: Villa Celina, " & FechaRecibo, PrintFontStr, Brushes.Black, PosX + 2, PosY + 23.0F)

        e.Graphics.DrawLine(PrintPenLineas, PosX, PosY + 28.0F, PosX + AnchoHoja, PosY + 28.0F)

        EscribeNumeroALetras(e)

        e.Graphics.DrawLine(PrintPenLineas, PosX, PosY + 39.0F, PosX + AnchoHoja, PosY + 39.0F)

        e.Graphics.DrawString("Recibo Leyes 17250, 20744 y 21297", PrintFontStr, Brushes.Black, PosX + 2, PosY + 43.0F)

        e.Graphics.DrawString("_________________", PrintFontStr, Brushes.Black, PosX + 95.0F, PosY + 52.0F)
        If EsEmpleador Then
            TextoFirma += " Empleador"
        Else
            TextoFirma += " Empleado"
            e.Graphics.DrawString("Recibí el importe neto y duplicado de la presente", PrintFontStr, Brushes.Black, PosX + 2, PosY + 45.0F)
            e.Graphics.DrawString("liquidacion en pago de mi remuneracion", PrintFontStr, Brushes.Black, PosX + 2, PosY + 47.0F)
            e.Graphics.DrawString("correspondiente al periodo indicado.", PrintFontStr, Brushes.Black, PosX + 2, PosY + 49.0F)
        End If
        e.Graphics.DrawString(TextoFirma, PrintFontStr, Brushes.Black, PosX + 97.0F, PosY + 55.0F)

    End Sub
    Private Sub EscribeNumeroALetras(ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim Texto As String = TextoPesos
        Dim Str1 As String = ""
        Dim Str2 As String = ""
        Dim PosicionCaracter As Integer = Texto.IndexOf("con")
        Dim ParteDecimal() As String = NumeroFormateado.Split(",")

        If ParteDecimal(1) <> "00" Then
            Str2 = Texto.Substring(PosicionCaracter)
            Str1 = Texto.Remove(PosicionCaracter, Str2.Length) & "Pesos "
            Texto = Str1 & Str2
        Else
            Str1 += Texto & "Pesos"
            Texto = Str1
        End If

        LongitudStr = e.Graphics.MeasureString(Texto, PrintFontStr).Width
        While LongitudStr < AnchoHoja - 4
            Texto += "*"
            LongitudStr = e.Graphics.MeasureString(Texto, PrintFontStr).Width
        End While

        If LongitudStr > AnchoHoja - 4 And Not Texto.Contains("*") Then
            LongitudStr = e.Graphics.MeasureString(Str1, PrintFontStr).Width
            While LongitudStr < AnchoHoja - 4
                Str1 += "*"
                LongitudStr = e.Graphics.MeasureString(Str1, PrintFontStr).Width
            End While
            Texto = Str1
            TextoPesos2 = Str2
        End If

        LongitudStr = e.Graphics.MeasureString(TextoPesos2, PrintFontStr).Width
        While LongitudStr < AnchoHoja - 4
            TextoPesos2 += "*"
            LongitudStr = e.Graphics.MeasureString(TextoPesos2, PrintFontStr).Width
        End While

        e.Graphics.DrawString(Texto, PrintFontStr, Brushes.Black, PosX + 2, PosY + 30.0F)
        e.Graphics.DrawString(TextoPesos2, PrintFontStr, Brushes.Black, PosX + 2, PosY + 34.0F)

    End Sub

    Public Sub CreaDTDetalle(ByRef DTDetalle)

        DTDetalle = New DataTable

        Dim Cod As New DataColumn("Cod")
        Cod.DataType = System.Type.GetType("System.String")
        DTDetalle.Columns.Add(Cod)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.String")
        DTDetalle.Columns.Add(Concepto)

        Dim Unidades As New DataColumn("Unidades")
        Unidades.DataType = System.Type.GetType("System.Decimal")
        DTDetalle.Columns.Add(Unidades)

        Dim HaberCDesc As New DataColumn("HaberCDesc")
        HaberCDesc.DataType = System.Type.GetType("System.Decimal")
        DTDetalle.Columns.Add(HaberCDesc)

        Dim HaberSDesc As New DataColumn("HaberSDesc")
        HaberSDesc.DataType = System.Type.GetType("System.Decimal")
        DTDetalle.Columns.Add(HaberSDesc)

        Dim Deducciones As New DataColumn("Deducciones")
        Deducciones.DataType = System.Type.GetType("System.Decimal")
        DTDetalle.Columns.Add(Deducciones)

    End Sub
    Public Function HallaDatosEmpresa(ByRef Calle As String, ByRef Numero As Integer, ByRef Localidad As String, ByRef Provincia As String, ByRef CodigoPostal As String) As Boolean

        Dim Dt As New DataTable
        Calle = ""
        Numero = 0
        Localidad = ""
        Provincia = ""
        CodigoPostal = ""

        If Not Tablas.Read("SELECT Calle,Numero, Localidad, Provincia, CodigoPostal FROM DatosEmpresa WHERE Indice = 1;", Conexion, Dt) Then
            MsgBox("ERROR: No se Pudo Leer Tablas de DatosEmpres.") : Return False
        End If
        If Dt.Rows.Count <> 0 Then
            Calle = Dt.Rows(0).Item("Calle")
            Numero = Dt.Rows(0).Item("Numero")
            Localidad = Dt.Rows(0).Item("Localidad")
            Provincia = Dt.Rows(0).Item("Provincia")
            CodigoPostal = Dt.Rows(0).Item("CodigoPostal")
        End If

        Dt.Dispose()

        Return True

    End Function
    Public Sub Imprime()

        Try
            UnSeteoImpresora.SeteaImpresion(ImpresionDocumento)
            ImpresionDocumento.DefaultPageSettings.Landscape = True
            AddHandler ImpresionDocumento.PrintPage, AddressOf Print_Recibo
            ImpresionDocumento.Print()
        Catch ex As Exception
            MsgBox("NO SE PUDO REALIZAR LA IMPRESION DEL RECIBO! DETALLE DEL ERROR: " & ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR!")
        Finally
            ImpresionDocumento.Dispose()
        End Try
    End Sub

End Module
