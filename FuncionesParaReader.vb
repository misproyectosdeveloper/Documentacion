Imports System.IO
Imports System.Xml
Module FuncionesParaReader
    Public Function LeerPadron(ByVal CodigoRetencion As Integer, ByVal Fecha As Date, ByVal Cuit As Decimal) As Decimal

        Dim BusquedaStr As String = Format(CodigoRetencion, "00000") & Format(Fecha.Month, "00") & Fecha.Year & ".TXT"

        Dim Alicuota As Decimal = 0
        Dim File As String

        Dim Path As String = System.AppDomain.CurrentDomain.BaseDirectory()
        Path = Path & "\" & "PadronRetenciones"

        If Not System.IO.Directory.Exists(Path) Then
            Return -100
        End If

        Dim di As DirectoryInfo = New DirectoryInfo(Path)
        For Each fi As Object In di.GetFiles()
            If Strings.Right(fi.Name, 15) = BusquedaStr And Strings.Left(fi.Name, 3) = Format(CInt(GClaveEmpresa), "000") Then File = fi.Name : Exit For
        Next
        ' para compatibilizar con los arcivosanteriores que no tenian clave de empresas.  
        '    If File = "" Then
        '       For Each fi As Object In di.GetFiles()
        '           If Strings.Right(fi.Name, 15) = BusquedaStr Then File = fi.Name : Exit For
        '       Next
        '    End If
        '-------------------------------------------------------------------------------

        If File = "" Then
            Return -100   'Padrón para Retención No Existe.. En esta carpeta debe estar el archivo de Padrón para Retención.
        End If

        Dim Linea As String

        Using reader As StreamReader = New StreamReader(Path & "\" & File)
            Linea = reader.ReadLine
            Do While (Not Linea Is Nothing)
                If CDec(Mid$(Linea, 30, 11)) = Cuit Then
                    Alicuota = CDec(Mid$(Linea, 48, 4))
                    Exit Do
                End If
                Linea = reader.ReadLine
            Loop
            reader.Dispose()
        End Using

        Return Alicuota

    End Function
    Public Sub ExportaCheques(ByVal dt As DataTable, ByVal Emisor As Integer)


        Dim Directorio As String = PidePath("OPEN")
        If Directorio = "" Then Exit Sub

        Using writer As StreamWriter = New StreamWriter(Directorio)   'Borra Archivo.
        End Using

        Dim MedioPagoW As String
        Dim BancoW As String = ""
        Dim CuentaW As String = ""
        Dim SerieW As String = ""
        Dim NumeroW As String = ""
        Dim ImporteW As String = ""
        Dim EmisorChequeW As String = ""
        Dim FechaW As String = ""
        Dim CuitBancoW = 0

        For Each Row As DataRow In dt.Rows
            If Row("MedioPago") = 2 Or Row("MedioPago") = 3 Then
                MedioPagoW = Row("MedioPago")
                BancoW = RellenarBlancos(NombreBanco(Row("Banco")), 20)
                CuentaW = RellenarCeros(Row("Cuenta"), 15)
                SerieW = RellenarBlancos(Row("Serie"), 2)
                NumeroW = RellenarCeros(Row("Numero"), 15)
                ImporteW = RellenarCeros(FormateaImporte(Row("Importe")), 15)
                FechaW = FormateaFecha(Row("Fecha"))
                If Row("MedioPago") = 3 Then
                    EmisorChequeW = RellenarBlancos(Row("EmisorCheque"), 30)
                Else
                    EmisorChequeW = RellenarBlancos(GNombreEmpresa, 30)
                End If
                CuitBancoW = HallaCuitBanco(Row("Banco"))
                Dim Str As String = "IECH" & MedioPagoW & BancoW & CuentaW & SerieW & NumeroW & ImporteW & FechaW & EmisorChequeW & CuitBancoW
                Using writer As StreamWriter = New StreamWriter(Directorio, True)
                    writer.WriteLine(Str)
                    writer.Close()
                End Using
            End If
        Next

        dt.Dispose()

        MsgBox("Exportación Terminada.")

    End Sub
    Public Sub ImportaCheques(ByVal Dt As DataTable)

        Dim Directorio As String = PidePath("READ")
        If Directorio = "" Then Exit Sub

        Dim Linea As String

        Using reader As StreamReader = New StreamReader(Directorio)
            Linea = reader.ReadLine
            If Mid$(Linea, 1, 4) <> "IECH" Then
                MsgBox("Archivo no Proviene de una Importación de Cheques.") : Exit Sub
            End If
            Do While (Not Linea Is Nothing)
                Dim row As DataRow = Dt.NewRow
                row("Item") = 0
                row("MedioPago") = 3
                row("Detalle") = ""
                row("Neto") = 0
                row("Alicuota") = 0
                row("ImporteIva") = 0
                row("Banco") = HallaClaveBanco(Int(Mid$(Linea, 111, 11)))
                row("Fecha") = Mid$(Linea, 79, 2) & "/" & Mid$(Linea, 77, 2) & "/" & Mid$(Linea, 73, 4)
                row("Cuenta") = CDec(Mid$(Linea, 26, 15))
                row("Serie") = Mid$(Linea, 41, 2)
                row("Numero") = CDec(Mid$(Linea, 43, 15))
                row("EmisorCheque") = Mid$(Linea, 81, 30)
                row("Cambio") = 0
                row("Importe") = CDec(Mid$(Linea, 58, 15)) / 100
                row("Bultos") = 0
                row("Comprobante") = 0
                row("FechaComprobante") = "1/1/1800"
                row("ClaveCheque") = 0
                row("ClaveChequeVisual") = 0
                row("ClaveInterna") = 0
                row("TieneLupa") = False
                Dt.Rows.Add(row)
                Linea = reader.ReadLine
            Loop
            reader.Dispose()
        End Using

        MsgBox("Importación Terminada.")

    End Sub
    Private Function FormateaFecha(ByVal Fecha As Date) As String

        Return Format(Fecha.Year, "0000") & Format(Fecha.Month, "00") & Format(Fecha.Day, "00")

    End Function
    Private Function RellenarCeros(ByVal Numero As String, ByVal Logitud As Integer) As String

        Dim Str As String = ""

        Str = Numero

        For i As Integer = 1 To Logitud - Str.Length
            Str = "0" & Str
        Next

        Return Str

    End Function
    Public Function RellenarBlancos(ByVal Dato As String, ByVal Logitud As Integer) As String

        Dim Str As String = ""

        Str = Dato

        For i As Integer = 1 To Logitud - Str.Length
            Str = Str & " "
        Next

        Return Str

    End Function
    Private Function FormateaImporte(ByVal Importe As Decimal) As String

        ' decimales del importe debe tener 1 o 2 digitos.

        Dim Arr(2) As String
        Arr = Importe.ToString.Split(",")

        Dim EnteroStr As String = Arr(0)
        Dim DecimalStr As String = ""
        If Arr.Length = 2 Then
            DecimalStr = Arr(1)
        End If
        If DecimalStr.Length = 1 Then DecimalStr = DecimalStr & "0"
        If DecimalStr.Length = 0 Then DecimalStr = DecimalStr & "00"

        Return EnteroStr & DecimalStr

    End Function
    Public Function PidePath(ByVal Funcion As String) As String

        Dim FileDialog1 As Object

        If Funcion = "OPEN" Then
            FileDialog1 = New SaveFileDialog()
            FileDialog1.Title = "Seleccione Archivo a Grabar."
        End If
        If Funcion = "READ" Then
            FileDialog1 = New OpenFileDialog()
            FileDialog1.Title = "Seleccione Archivo a Leer."
        End If

        '        FileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        FileDialog1.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        FileDialog1.InitialDirectory = "C:\"
        FileDialog1.FilterIndex = 1
        FileDialog1.RestoreDirectory = True

        If FileDialog1.ShowDialog() = DialogResult.OK Then
            Return FileDialog1.FileName
        End If


    End Function
    Private Function HallaCuitBanco(ByVal Banco As Integer) As Decimal

        Dim Dt As New DataTable

        Try
            Tablas.Read("SELECT Cuit FROM Tablas WHERE Tipo = 26 AND Clave = " & Banco & ";", Conexion, Dt)
            If Dt.Rows.Count = 0 Then Return 0
            Return Dt.Rows(0).Item("Cuit")
        Catch ex As Exception
            Return 0
        Finally
            Dt.Dispose()
        End Try

    End Function
    Private Function HallaClaveBanco(ByVal Cuit As Decimal) As Integer

        Dim Dt As New DataTable

        Try
            Tablas.Read("SELECT Clave FROM Tablas WHERE Tipo = 26 AND Cuit = " & Cuit & ";", Conexion, Dt)
            If Dt.Rows.Count = 0 Then Return 0
            Return Dt.Rows(0).Item("Clave")
        Catch ex As Exception
            Return 0
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function BuscaMensajeError(ByVal Tipo As String, ByVal CodigoError As String) As String

        'Tipo reservada para tipos de errores. (A: AFIP, S: Sistema)

        Dim Path As String = System.AppDomain.CurrentDomain.BaseDirectory()
        Path = Path & "SCErrores.xml"
        If Not File.Exists(Path) Then Return ""

        Dim m_xmld As XmlDocument
        Dim m_nodelist As XmlNodeList
        Dim m_node As XmlNode

        Try
            'Creamos el "Document"
            m_xmld = New XmlDocument()
            'Cargamos el archivo
            m_xmld.Load(Path)
            'Cargamos listado de errores
            Dim elemList As XmlNodeList = m_xmld.GetElementsByTagName("CodigosAfip")
            For Each m_node In elemList
                If m_node("Cod").InnerText = CodigoError Then Return m_node("Mensaje").InnerText
            Next
        Catch ex As Exception
            Return ""
        Finally
            m_xmld = Nothing
        End Try

    End Function

End Module
