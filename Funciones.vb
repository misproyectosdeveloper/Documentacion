Imports System.Data
Imports System.Data.OleDb
Imports Microsoft.Reporting.WinForms
Imports Microsoft.Office.Interop
Imports System.Math
Imports System.Transactions
Imports ClassPassWord
Module Funciones
    Public Sub CambiarPuntoDecimal(ByVal Signo As String)

        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-AR", True)
        System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = Signo

    End Sub
    Public Function TieneDecimales(ByVal Numero As Double) As Boolean

        If Numero - Int(Numero) <> 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Sub EsNumerico(ByRef e As String, ByVal Texto As String, ByVal Decimales As Integer)

        If e = "." Then e = ","

        If InStr("0123456789," & Chr(8), e) = 0 Then e = ""

        Dim MiArray() As String
        MiArray = Split(Texto, ",")
        If UBound(MiArray) > 1 Then e = "" : Exit Sub 'Para las columnas del gridDataView.
        If e = "," Then
            If Decimales = 0 Then e = "" : Exit Sub
            If UBound(MiArray) = 1 Then e = "" : Exit Sub
        End If
        If UBound(MiArray) = 1 Then
            If MiArray(1).ToString.Length + 1 > Decimales Then e = "" : Exit Sub
        End If

    End Sub
    Public Sub EsNumericoConSigno(ByRef e As String, ByVal Texto As String, ByVal Decimales As Integer)

        If e = "." Then e = ","

        If InStr("-0123456789," & Chr(8), e) = 0 Then e = ""

        If e = "-" And Texto.Length > 0 Then e = "" : Exit Sub

        Dim MiArray() As String
        MiArray = Split(Texto, ",")
        If UBound(MiArray) > 1 Then e = "" : Exit Sub 'Para las columnas del gridDataView.
        If e = "," Then
            If Decimales = 0 Then e = "" : Exit Sub
            If UBound(MiArray) = 1 Then e = "" : Exit Sub
        End If
        If UBound(MiArray) = 1 Then
            If MiArray(1).ToString.Length + 1 > Decimales Then e = "" : Exit Sub
        End If

    End Sub
    Public Sub EsPorcentaje(ByRef e As String, ByVal Texto As String)

        If e = "." Then e = ","

        If InStr("0123456789," & Chr(8), e) = 0 Then e = ""

        Dim MiArray() As String
        MiArray = Split(Texto, ",")
        If UBound(MiArray) > 1 Then e = "" : Exit Sub 'Para las columnas del gridDataView.
        If e = "," Then
            If UBound(MiArray) = 1 Then e = "" : Exit Sub
        End If
        If MiArray(0).ToString.Length + 1 > 3 Then e = "" : Exit Sub
        If UBound(MiArray) = 1 Then
            If MiArray(1).ToString.Length + 1 > 2 Then e = "" : Exit Sub
        End If

    End Sub
    Public Function Trunca(ByVal Valor As Double) As Double

        Return CDbl(Format(Valor, "0.00"))

    End Function
    Public Function Trunca3(ByVal Valor As Double) As Double

        Return CDbl(Format(Valor, "0.000"))

    End Function
    Public Function Trunca4(ByVal Valor As Double) As Double

        Return CDbl(Format(Valor, "0.0000"))

    End Function
    Public Function Trunca5(ByVal Valor As Double) As Double

        Return CDbl(Format(Valor, "0.00000"))

    End Function
    Public Function TruncaSR(ByVal Numero As Double, ByVal Decimales As Integer) As Double

        Return Fix(Numero * 10 ^ Decimales) / (10 ^ Decimales)

    End Function
    Public Sub HallaParteEnteraYDecimal(ByVal Numero As Decimal, ByRef Entero As Integer, ByRef Decimales As Integer)

        Entero = Int(Numero)
        Decimales = (Numero - Entero) * 100

    End Sub
    Public Function CalculaNeto(ByVal Cantidad As Decimal, ByVal Precio As Decimal) As Decimal

        Return Trunca(Cantidad * Precio)

    End Function
    Public Function CalculaIva(ByVal Cantidad As Decimal, ByVal Precio As Decimal, ByVal Iva As Decimal) As Decimal

        Return Trunca(Cantidad * Precio * Iva / 100)

    End Function
    Public Function FormularioOK(ByVal Form As Form) As Boolean

        If Form.WindowState = FormWindowState.Minimized Then Form.WindowState = FormWindowState.Normal : Return True
        If Form.Visible Then MsgBox("Formulario Ya esta Abierto") : Return False
        Return True

    End Function
    Public Function TraeNumero(ByVal Entrada As String) As Double

        Dim MiArray() As String

        If Entrada = "" Then Return 0
        MiArray = Split(Entrada, "-")
        Dim EntradaW As String = MiArray(0) & MiArray(1)
        Return CDbl(EntradaW)

    End Function
    Public Function DiferenciaDias(ByVal Fecha1 As DateTime, ByVal Fecha2 As DateTime) As Integer

        Return DateDiff(DateInterval.Day, CDate(Format(Fecha1, "dd/MM/yyyy")), CDate(Format(Fecha2, "dd/MM/yyyy")))

    End Function
    Public Function ChequeVencido(ByVal Fecha As DateTime, ByVal FechaActual As DateTime) As Boolean

        Fecha = DateAdd(DateInterval.Day, 29, Fecha)
        If DateDiff(DateInterval.Day, CDate(Format(Fecha, "dd/MM/yyyy")), CDate(Format(FechaActual, "dd/MM/yyyy"))) > 0 Then Return True

    End Function
    Public Function NumeroEditado(ByVal Numero As Double) As String

        If Numero.ToString.Length < 9 Then Return Format(Numero, "00000000")
        If Numero.ToString.Length < 13 Then Return (" " & Format(Numero, "0000-00000000"))
        Return LetraTipoIva(Strings.Left(Numero, 1)) & Format(CDbl(Strings.Right(Numero, 12)), "0000-00000000")

    End Function
    Public Function RellenarCeros(ByVal Numero As String, ByVal Loguitud As Integer) As String

        Dim Str As String = ""

        Str = Numero

        For i As Integer = 1 To Loguitud - Str.Length
            Str = "0" & Str
        Next

        Return Str

    End Function
    Public Function StringSonCeros(ByVal Str As String) As Boolean

        Dim charArray() As Char = Str.ToCharArray

        For I As Integer = 0 To Str.Length - 1
            If charArray(I) <> "0" Then Return False
        Next

        Return True

    End Function
    Public Function HallaPuntoVentaSegunTipo(ByVal TipoNota As Integer, ByVal TipoIva As Integer) As Integer

        Select Case TipoNota
            Case 1      'Remitos.
                Return GPuntoDeVentaRemitos
            Case 2, 800 'Factura venta,NVLP.
                Select Case TipoIva
                    Case 1, 5
                        Return GPuntoDeVentaResponsableInsc
                    Case 2, 6
                        Return GPuntoDeVentaResponsableNoInsc
                    Case 3
                        Return GPuntoDeVentaConsumidorFinal
                    Case 4
                        Return GPuntoDeVentaExportacion
                End Select
            Case 60, 600, 65, 64, 604
                Return GPuntoDeVentaRecibos
            Case 5, 6, 50, 500
                Select Case TipoIva
                    Case 1, 5
                        Return GPuntoDeVentaDebResponsableInsc
                    Case 2, 6
                        Return GPuntoDeVentaDebResponsableNoInsc
                    Case 3
                        Return GPuntoDeVentaDebConsumidorFinal
                    Case 4
                        Return GPuntoDeVentaDebExportacion
                End Select
            Case 7, 8, 70, 700
                Select Case TipoIva
                    Case 1, 5
                        Return GPuntoDeVentaCredResponsableInsc
                    Case 2, 6
                        Return GPuntoDeVentaCredResponsableNoInsc
                    Case 3
                        Return GPuntoDeVentaCredConsumidorFinal
                    Case 4
                        Return GPuntoDeVentaCredExportacion
                End Select
            Case 910 'Liquidacion.
                Select Case TipoIva
                    Case 1, 5
                        Return GPuntoDeVentaLiqResponsableInsc
                    Case 2, 6
                        Return GPuntoDeVentaLiqResponsableNoInsc
                    Case 3
                        Return GPuntoDeVentaLiqConsumidorFinal
                End Select
            Case Else
                MsgBox("Tipo nota " & TipoNota & " No Contemplada.")
                End
        End Select

    End Function
    Public Function HallaPuntoVentaFce() As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT PuntoDeVentaFCE FROM Usuarios WHERE Clave =  " & GClaveUsuario & ";", Miconexion)
                    Return Cmd.ExecuteScalar
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: PuntosDeVenta.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function ExistePuntoDeVenta(ByVal PuntoDeVenta As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Clave FROM PuntosDeVenta WHERE Clave =  " & PuntoDeVenta & ";", Miconexion)
                    If Cmd.ExecuteScalar = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: PuntosDeVenta.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function EsPuntoDeVentaZ(ByVal PuntoDeVenta As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT EsZ FROM PuntosDeVenta WHERE Clave =  " & PuntoDeVenta & ";", Miconexion)
                    If Cmd.ExecuteScalar = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: PuntosDeVenta.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function EsPuntoDeVentaCFE(ByVal PuntoDeVenta As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT EsFCE FROM PuntosDeVenta WHERE Clave =  " & PuntoDeVenta & ";", Miconexion)
                    If Cmd.ExecuteScalar = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: PuntosDeVenta.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Sub DatosPuntoDeVentaParaRemito(ByVal PuntoDeVenta As Integer, ByRef EsAutoImpreso As Boolean, ByRef EsZ As Boolean, ByRef Cai As Decimal, ByRef IntFechaCai As Integer)

        EsAutoImpreso = False
        EsZ = False
        Cai = 0

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT EsRemitoAutoImpreso,EsZ,CAIRemito,FechaRemito FROM PuntosDeVenta WHERE Clave = " & PuntoDeVenta & ";", Conexion, Dt) Then
            MsgBox("Error Base de Datos al leer Tabla: PuntosDeVenta: " & PuntoDeVenta, MsgBoxStyle.Critical)
            End
        End If
        If Dt.Rows.Count <> 0 Then
            EsAutoImpreso = Dt.Rows(0).Item("EsRemitoAutoImpreso")
            EsZ = Dt.Rows(0).Item("EsZ")
            Cai = Dt.Rows(0).Item("CAIRemito")
            If EsAutoImpreso Then
                IntFechaCai = Dt.Rows(0).Item("FechaRemito")
            End If
        End If

        Dt.Dispose()

    End Sub
    Public Function FacturasConComprobantesZ(ByVal Factura As Decimal, ByVal Desde As Integer, ByVal Hasta As Integer, ByVal PuntoDeVenta As Integer, ByVal Tipo As Integer) As Integer

        Dim Patron As String = Tipo & Format(PuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Dim Sql As String

        If Factura = 0 Then
            Sql = "SELECT COUNT(Factura) FROM FacturasCabeza WHERE Estado <> 3 AND EsZ = 1 AND ComprobanteHasta >= " & Desde & " AND ComprobanteDesde <= " & Hasta & " AND CAST(CAST(Factura AS numeric) as char)LIKE '" & Patron & "';"
        Else
            Sql = "SELECT COUNT(Factura) FROM FacturasCabeza WHERE Estado <> 3 AND EsZ = 1 AND Factura <> " & Factura & " AND ComprobanteHasta >= " & Desde & " AND ComprobanteDesde <= " & Hasta & " AND CAST(CAST(Factura AS numeric) as char)LIKE '" & Patron & "';"
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function ConsistePagos(ByRef Grid As DataGridView, ByRef DtFormasPago As DataTable, ByVal PTipoNota As Integer, ByVal EsTr As Boolean) As Boolean

        Dim Row As DataGridViewRow
        Dim RowsBusqueda() As DataRow
        Dim Tipo As Integer
        Dim Tope As Integer

        If Grid.AllowUserToAddRows Then
            Tope = Grid.Rows.Count - 2
        Else : Tope = Grid.Rows.Count - 1
        End If

        For i As Integer = 0 To Tope
            Row = Grid.Rows(i)
            If Row.Cells("Concepto").Value = 0 Then
                MsgBox("Debe Informar Concepto en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Concepto")
                Grid.BeginEdit(True)
                Return False
            End If
            If Row.Cells("Importe").Value = 0 Then
                MsgBox("Debe Informar Importe en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Importe")
                Grid.BeginEdit(True)
                Return False
            End If
            '
            RowsBusqueda = DtFormasPago.Select("Clave = " & Row.Cells("Concepto").Value)
            Tipo = RowsBusqueda(0).Item("Tipo")
            '
            If Tipo = 8 Or Tipo = 9 Then             'Importeo Ingreso Bruto de Cred/Deb.
                If Row.Cells("Detalle").Value.ToString.Length = 0 Then
                    MsgBox("Debe Informar Detalle en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Detalle")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If Row.Cells("Neto").Value = 0 Then
                    MsgBox("Debe Informar Neto en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Neto")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
            If Tipo = 2 Then             'es cheque Propio.
                If Row.Cells("Banco").Value = 0 Then
                    MsgBox("Debe Informar Banco en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Banco")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If Row.Cells("Cuenta").Value = 0 Then
                    MsgBox("Debe Informar Cuenta en la Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Cuenta")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If Row.Cells("Numero").Value = 0 Then
                    MsgBox("Debe Informar numero Comprobante o cheque en la Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Numero")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If Not Row.Cells("eCheq").Value And (Row.Cells("Numero").Value < Row.Cells("NumeracionInicial").Value Or Row.Cells("Numero").Value > Row.Cells("NumeracionFinal").Value) Then
                    MsgBox("Numero Cheque no corresponde a la Numeración de la Chequera en la Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Numero")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If Row.Cells("Fecha").Value = "1/1/1800" Then
                    MsgBox("Debe Informar Fecha en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Fecha")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If ChequeVencido(Row.Cells("Fecha").Value, Date.Now) And Not EsTr Then
                    MsgBox("Cheque Vencido en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Fecha")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
            If Tipo = 6 Then             'es cheque tercero.
                If Row.Cells("Banco").Value = 0 Then
                    MsgBox("Debe Informar Banco en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Banco")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If Row.Cells("Numero").Value = 0 Then
                    MsgBox("Debe Informar numero Comprobante o cheque en la Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Numero")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If Row.Cells("EmisorCheque").Value = "" Then
                    MsgBox("Debe Informar Emisor del Cheque en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("EmisorCheque")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If Row.Cells("Fecha").Value = "1/1/1800" Then
                    MsgBox("Debe Informar Fecha en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Fecha")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If ChequeVencido(Row.Cells("Fecha").Value, Date.Now) And Not EsTr Then
                    MsgBox("Cheque Vencido en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Fecha")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
            If Tipo = 3 Then        'es moneda extranjera.
                If Grid.Rows(i).Cells("Cambio").Visible Then
                    If PTipoNota <> 80 Then    'Si no es un pase.
                        If Row.Cells("Cambio").Value = 0 Then
                            MsgBox("Debe Informar Cambio en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            Grid.CurrentCell = Grid.Rows(i).Cells("Cambio")
                            Grid.BeginEdit(True)
                            Return False
                        End If
                    End If
                End If
            End If
            If Tipo = 7 Or Tipo = 15 Then     'es interbanking,transferencia,deposito,deposito exterior,comprobanteMostrador,Interbanking divisa,deposito divisa,transf. divisa.
                If Row.Cells("Banco").Value = 0 Then
                    MsgBox("Debe Informar Banco en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Banco")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If Row.Cells("Cuenta").Value = 0 And Row.Cells("Cuenta").Visible = True Then
                    MsgBox("Debe Informar Cuenta en la Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Cuenta")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If Row.Cells("Comprobante").Value = 0 Then
                    MsgBox("Debe Informar Numero Comprobante en la Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Comprobante")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If Row.Cells("FechaComprobante").Value = "1/1/1800" Then
                    MsgBox("Debe Informar Fecha del Comprobante en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("FechaComprobante")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If Row.Cells("Concepto").Value <> 14 And DiferenciaDias(Row.Cells("FechaComprobante").Value, Date.Now) < 0 Then
                    MsgBox("Fecha del Comprobante es mayor a la actual en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("FechaComprobante")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
            If Tipo = 10 Then     'es Vale Propio.
                If Row.Cells("Bultos").Value = 0 Then
                    MsgBox("Debe Informar Bultos en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Bultos")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
            '
            If (Tipo = 4 And (PTipoNota = 60 Or PTipoNota = 604)) Or Tipo = 10 Then     'es retencion de clientes o Vale de proveedor. Junto al comprobante viene un certificado con numero y fecha retencion.
                If Row.Cells("Comprobante").Value = 0 Then
                    MsgBox("Debe Informar Numero Comprobante en la Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Comprobante")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If Row.Cells("FechaComprobante").Value = "1/1/1800" Then
                    MsgBox("Debe Informar Fecha del Comprobante en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("FechaComprobante")
                    Grid.BeginEdit(True)
                    Return False
                End If
                If DiferenciaDias(Row.Cells("FechaComprobante").Value, Date.Now) < 0 Then
                    MsgBox("Fecha del Comprobante es mayor a la actual en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("FechaComprobante")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
            If Tipo = 4 And (PTipoNota = 5 Or PTipoNota = 6 Or PTipoNota = 7 Or PTipoNota = 8) Then     'es percepcion a clientes o proveedor.
                If Row.Cells("Detalle").Value.ToString.Length = 0 Then
                    MsgBox("Debe Informar Detalle en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Detalle")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
        Next

        For i As Integer = 1 To Tope
            Row = Grid.Rows(i)
            RowsBusqueda = DtFormasPago.Select("Clave = " & Row.Cells("Concepto").Value)
            Tipo = RowsBusqueda(0).Item("Tipo")
            If Tipo <> 11 Then  'saque tipo=6
                If Row.Cells("Numero").Value <> 0 Or Row.Cells("Comprobante").Value <> 0 Then
                    For Y As Integer = 0 To i - 1
                        If Row.Cells("Concepto").Value = Grid.Rows(Y).Cells("Concepto").Value And Row.Cells("Banco").Value = Grid.Rows(Y).Cells("Banco").Value And Row.Cells("Cuenta").Value = Grid.Rows(Y).Cells("Cuenta").Value _
                            And Row.Cells("Serie").Value = Grid.Rows(Y).Cells("Serie").Value And Row.Cells("Numero").Value = Grid.Rows(Y).Cells("Numero").Value And Row.Cells("Comprobante").Value = Grid.Rows(Y).Cells("Comprobante").Value Then
                            MsgBox("Cheque o Comprobante Repetido en la Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            Grid.CurrentCell = Grid.Rows(i).Cells("Concepto")
                            Grid.BeginEdit(True)
                            Return False
                        End If
                    Next
                End If
            End If
        Next

        For i As Integer = 1 To Tope
            Row = Grid.Rows(i)
            RowsBusqueda = DtFormasPago.Select("Clave = " & Row.Cells("Concepto").Value)
            Tipo = RowsBusqueda(0).Item("Tipo")
            If Tipo = 6 Then
                If Row.Cells("ClaveCheque").Value <> 0 Then
                    For Y As Integer = 0 To i - 1
                        If Row.Cells("ClaveCheque").Value = Grid.Rows(Y).Cells("ClaveCheque").Value Then
                            MsgBox("Cheque Terceros Repetido en la Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            Grid.CurrentCell = Grid.Rows(i).Cells("Concepto")
                            Grid.BeginEdit(True)
                            Return False
                        End If
                    Next
                End If
            End If
        Next

        For i As Integer = 1 To Tope
            Row = Grid.Rows(i)
            RowsBusqueda = DtFormasPago.Select("Clave = " & Row.Cells("Concepto").Value)
            Tipo = RowsBusqueda(0).Item("Tipo")
            If Tipo = 11 Then
                If Row.Cells("ClaveInterna").Value <> 0 Then
                    For Y As Integer = 0 To i - 1
                        If Row.Cells("ClaveInterna").Value = Grid.Rows(Y).Cells("ClaveInterna").Value Then
                            MsgBox("Vales Terceros Repetido en la Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            Grid.CurrentCell = Grid.Rows(i).Cells("Concepto")
                            Grid.BeginEdit(True)
                            Return False
                        End If
                    Next
                End If
            End If
        Next

        Return True

    End Function
    Public Sub ArmaGridSegunConcepto(ByRef Row As DataGridViewRow, ByVal Tipo As Integer, ByVal TipoNota As Integer, ByVal EsTrucha As Boolean, ByVal RetencionManual As Boolean, ByVal PAbierto As Boolean)

        Row.Cells("Importe").ReadOnly = False
        Row.Cells("Importe").Style.BackColor = Color.White
        Row.Cells("Cambio").ReadOnly = False
        Row.Cells("Cambio").Style.BackColor = Color.White
        Row.Cells("Banco").ReadOnly = False
        Row.Cells("Banco").Style.BackColor = Color.White
        Row.Cells("Cuenta").ReadOnly = False
        Row.Cells("Cuenta").Style.BackColor = Color.White
        Row.Cells("LupaCuenta").Style.BackColor = Color.White
        Row.Cells("Serie").ReadOnly = False
        Row.Cells("Serie").Style.BackColor = Color.White
        Row.Cells("Numero").ReadOnly = False
        Row.Cells("Numero").Style.BackColor = Color.White
        Row.Cells("EmisorCheque").ReadOnly = False
        Row.Cells("EmisorCheque").Style.BackColor = Color.White
        Row.Cells("Fecha").ReadOnly = True
        Row.Cells("Fecha").Style.BackColor = Color.White
        Row.Cells("Comprobante").ReadOnly = False
        Row.Cells("Comprobante").Style.BackColor = Color.White
        Row.Cells("FechaComprobante").ReadOnly = True
        Row.Cells("FechaComprobante").Style.BackColor = Color.White
        Row.Cells("ClaveCheque").ReadOnly = True
        Row.Cells("ClaveCheque").Style.BackColor = Color.White
        Row.Cells("Bultos").ReadOnly = False
        Row.Cells("Bultos").Style.BackColor = Color.White
        '
        Row.Cells("ClaveChequeVisual").Style.BackColor = Color.LightGray
        '
        Row.Cells("Iva").ReadOnly = False
        Row.Cells("Alicuota").ReadOnly = False

        If Tipo = 1 Then                                'pesos.
            Row.Cells("Cambio").ReadOnly = True
            Row.Cells("Cambio").Style.BackColor = Color.LightGray
            Row.Cells("Banco").ReadOnly = True
            Row.Cells("Banco").Style.BackColor = Color.LightGray
            Row.Cells("Cuenta").ReadOnly = True
            Row.Cells("Cuenta").Style.BackColor = Color.LightGray
            Row.Cells("LupaCuenta").Style.BackColor = Color.LightGray
            Row.Cells("Serie").ReadOnly = True
            Row.Cells("Serie").Style.BackColor = Color.LightGray
            Row.Cells("Numero").ReadOnly = True
            Row.Cells("Numero").Style.BackColor = Color.LightGray
            Row.Cells("EmisorCheque").ReadOnly = True
            Row.Cells("EmisorCheque").Style.BackColor = Color.LightGray
            Row.Cells("Fecha").Style.BackColor = Color.LightGray
            Row.Cells("Comprobante").ReadOnly = True
            Row.Cells("Comprobante").Style.BackColor = Color.LightGray
            Row.Cells("FechaComprobante").Style.BackColor = Color.LightGray
            Row.Cells("ClaveCheque").Style.BackColor = Color.LightGray
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
        End If
        If Tipo = 2 And Not EsTrucha Then                                'Cheque propio no trucho.
            Row.Cells("Cambio").ReadOnly = True
            Row.Cells("Cambio").Style.BackColor = Color.LightGray
            Row.Cells("Banco").ReadOnly = True
            Row.Cells("Cuenta").ReadOnly = True
            Row.Cells("Serie").ReadOnly = True
            Row.Cells("EmisorCheque").ReadOnly = True
            Row.Cells("EmisorCheque").Style.BackColor = Color.LightGray
            Row.Cells("Comprobante").ReadOnly = True
            Row.Cells("Comprobante").Style.BackColor = Color.LightGray
            Row.Cells("FechaComprobante").Style.BackColor = Color.LightGray
            Row.Cells("ClaveCheque").Style.BackColor = Color.LightGray
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
            If TipoNota = 600 Then
                Row.Cells("Fecha").ReadOnly = False
            End If
        End If
        If Tipo = 2 And EsTrucha Then                                    'Cheque propio trucho.
            Row.Cells("Importe").ReadOnly = True
            Row.Cells("Cambio").ReadOnly = True
            Row.Cells("Cambio").Style.BackColor = Color.LightGray
            Row.Cells("Banco").ReadOnly = True
            Row.Cells("Cuenta").ReadOnly = True
            Row.Cells("Cuenta").Style.BackColor = Color.LightGray
            Row.Cells("Serie").ReadOnly = True
            Row.Cells("Numero").ReadOnly = True
            Row.Cells("EmisorCheque").ReadOnly = True
            Row.Cells("EmisorCheque").Style.BackColor = Color.LightGray
            Row.Cells("Comprobante").ReadOnly = True
            Row.Cells("Comprobante").Style.BackColor = Color.LightGray
            Row.Cells("FechaComprobante").Style.BackColor = Color.LightGray
            Row.Cells("ClaveCheque").Style.BackColor = Color.LightGray
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
        End If
        If Tipo = 3 Then                              'moneda extrangera. 
            If TipoNota = 80 Then 'es un pase.
                Row.Cells("Cambio").ReadOnly = True
                Row.Cells("Cambio").Style.BackColor = Color.LightGray
            End If
            Row.Cells("Banco").ReadOnly = True
            Row.Cells("Banco").Style.BackColor = Color.LightGray
            Row.Cells("Cuenta").ReadOnly = True
            Row.Cells("Cuenta").Style.BackColor = Color.LightGray
            Row.Cells("LupaCuenta").Style.BackColor = Color.LightGray
            Row.Cells("Serie").ReadOnly = True
            Row.Cells("Serie").Style.BackColor = Color.LightGray
            Row.Cells("Numero").ReadOnly = True
            Row.Cells("Numero").Style.BackColor = Color.LightGray
            Row.Cells("EmisorCheque").ReadOnly = True
            Row.Cells("EmisorCheque").Style.BackColor = Color.LightGray
            Row.Cells("Fecha").Style.BackColor = Color.LightGray
            Row.Cells("Comprobante").ReadOnly = True
            Row.Cells("Comprobante").Style.BackColor = Color.LightGray
            Row.Cells("FechaComprobante").Style.BackColor = Color.LightGray
            Row.Cells("ClaveCheque").Style.BackColor = Color.LightGray
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
        End If
        If Tipo = 4 And TipoNota = 600 Then           'Retencion Proveedores.
            Row.Cells("Concepto").ReadOnly = True
            Row.Cells("Importe").ReadOnly = True
            Row.Cells("Cambio").ReadOnly = True
            Row.Cells("Cambio").Style.BackColor = Color.LightGray
            Row.Cells("Banco").ReadOnly = True
            Row.Cells("Banco").Style.BackColor = Color.LightGray
            Row.Cells("Cuenta").ReadOnly = True
            Row.Cells("Cuenta").Style.BackColor = Color.LightGray
            Row.Cells("LupaCuenta").Style.BackColor = Color.LightGray
            Row.Cells("LupaCuenta").Style.BackColor = Color.LightGray
            Row.Cells("Serie").ReadOnly = True
            Row.Cells("Serie").Style.BackColor = Color.LightGray
            Row.Cells("Numero").ReadOnly = True
            Row.Cells("Numero").Style.BackColor = Color.LightGray
            Row.Cells("Comprobante").ReadOnly = True
            Row.Cells("EmisorCheque").ReadOnly = True
            Row.Cells("EmisorCheque").Style.BackColor = Color.LightGray
            Row.Cells("Fecha").Style.BackColor = Color.LightGray
            Row.Cells("ClaveCheque").Style.BackColor = Color.LightGray
            If RetencionManual Then
                Row.Cells("Concepto").ReadOnly = False
                Row.Cells("Importe").ReadOnly = False
            Else
                If HallaFormulaRetencion(Row.Cells("Concepto").Value) = 0 Then
                    Row.Cells("Concepto").ReadOnly = False
                    Row.Cells("Importe").ReadOnly = False
                End If
            End If
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
        End If
        If Tipo = 4 And (TipoNota = 60 Or TipoNota = 604) Then           'Retencion Clientes.
            Row.Cells("Cambio").ReadOnly = True
            Row.Cells("Cambio").Style.BackColor = Color.LightGray
            Row.Cells("Banco").ReadOnly = True
            Row.Cells("Banco").Style.BackColor = Color.LightGray
            Row.Cells("Cuenta").ReadOnly = True
            Row.Cells("Cuenta").Style.BackColor = Color.LightGray
            Row.Cells("LupaCuenta").Style.BackColor = Color.LightGray
            Row.Cells("LupaCuenta").Style.BackColor = Color.LightGray
            Row.Cells("Serie").ReadOnly = True
            Row.Cells("Serie").Style.BackColor = Color.LightGray
            Row.Cells("Numero").ReadOnly = True
            Row.Cells("Numero").Style.BackColor = Color.LightGray
            Row.Cells("EmisorCheque").ReadOnly = True
            Row.Cells("EmisorCheque").Style.BackColor = Color.LightGray
            Row.Cells("Fecha").Style.BackColor = Color.LightGray
            Row.Cells("ClaveCheque").Style.BackColor = Color.LightGray
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
        End If
        If Tipo = 4 And Not (TipoNota = 60 Or TipoNota = 600 Or TipoNota = 65) Then   'Retenciones/Percepciones para ND/NC 
            Row.Cells("Alicuota").ReadOnly = True
            Row.Cells("Alicuota").Style.BackColor = Color.LightGray
            Row.Cells("Iva").ReadOnly = True
            Row.Cells("Iva").Style.BackColor = Color.LightGray
            Row.Cells("Neto").ReadOnly = True
            Row.Cells("Neto").Style.BackColor = Color.LightGray
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
        End If
        If Tipo = 4 Then           'Para percepcion en N. Debito y Credito financieras.
            Select Case TipoNota
                Case 5, 6, 7, 8
                    Row.Cells("Concepto").ReadOnly = True
                    Row.Cells("Importe").ReadOnly = True
                    Row.Cells("Cambio").ReadOnly = True
                    Row.Cells("Cambio").Style.BackColor = Color.LightGray
                    Row.Cells("Banco").ReadOnly = True
                    Row.Cells("Banco").Style.BackColor = Color.LightGray
                    Row.Cells("Cuenta").ReadOnly = True
                    Row.Cells("Cuenta").Style.BackColor = Color.LightGray
                    Row.Cells("LupaCuenta").Style.BackColor = Color.LightGray
                    Row.Cells("LupaCuenta").Style.BackColor = Color.LightGray
                    Row.Cells("Serie").ReadOnly = True
                    Row.Cells("Serie").Style.BackColor = Color.LightGray
                    Row.Cells("Numero").ReadOnly = True
                    Row.Cells("Numero").Style.BackColor = Color.LightGray
                    Row.Cells("Comprobante").ReadOnly = True
                    Row.Cells("EmisorCheque").ReadOnly = True
                    Row.Cells("EmisorCheque").Style.BackColor = Color.LightGray
                    Row.Cells("Fecha").Style.BackColor = Color.LightGray
                    Row.Cells("ClaveCheque").Style.BackColor = Color.LightGray
                    If RetencionManual Then
                        Row.Cells("Concepto").ReadOnly = False
                        Row.Cells("Importe").ReadOnly = False
                    Else
                        If HallaFormulaRetencion(Row.Cells("Concepto").Value) = 0 Then
                            Row.Cells("Concepto").ReadOnly = False
                            Row.Cells("Importe").ReadOnly = False
                        End If
                    End If
                    Row.Cells("Bultos").ReadOnly = True
                    Row.Cells("Bultos").Style.BackColor = Color.LightGray
            End Select
        End If
        If Tipo = 5 Then                               'Seña.
            Row.Cells("Cambio").ReadOnly = True
            Row.Cells("Banco").ReadOnly = True
            Row.Cells("Cuenta").ReadOnly = True
            Row.Cells("LupaCuenta").Style.BackColor = Color.LightGray
            Row.Cells("Serie").ReadOnly = True
            Row.Cells("Numero").ReadOnly = True
            Row.Cells("EmisorCheque").ReadOnly = True
            Row.Cells("Comprobante").ReadOnly = True
            Row.Cells("ClaveCheque").Style.BackColor = Color.LightGray
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
        End If
        If Tipo = 6 And (TipoNota = 600 Or TipoNota = 91 Or TipoNota = 80 Or TipoNota = 65 Or TipoNota = 6000 Or TipoNota = 64) Then  'cheque tercero.
            Row.Cells("Importe").ReadOnly = True
            Row.Cells("Cambio").ReadOnly = True
            Row.Cells("Cambio").Style.BackColor = Color.LightGray
            Row.Cells("Banco").ReadOnly = True
            Row.Cells("Cuenta").ReadOnly = True
            Row.Cells("Cuenta").Style.BackColor = Color.LightGray
            Row.Cells("Serie").ReadOnly = True
            Row.Cells("Numero").ReadOnly = True
            Row.Cells("EmisorCheque").ReadOnly = True
            Row.Cells("Comprobante").ReadOnly = True
            Row.Cells("Comprobante").Style.BackColor = Color.LightGray
            Row.Cells("FechaComprobante").Style.BackColor = Color.LightGray
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
        End If
        If Tipo = 6 And ((TipoNota = 60 And Not EsTrucha) Or TipoNota = 6001 Or TipoNota = 604) Then  'cheque tercero.
            Row.Cells("Cambio").ReadOnly = True
            Row.Cells("Cambio").Style.BackColor = Color.LightGray
            Row.Cells("Cuenta").ReadOnly = True
            Row.Cells("Cuenta").Style.BackColor = Color.LightGray
            Row.Cells("LupaCuenta").Style.BackColor = Color.LightGray
            Row.Cells("Comprobante").ReadOnly = True
            Row.Cells("Comprobante").Style.BackColor = Color.LightGray
            Row.Cells("FechaComprobante").Style.BackColor = Color.LightGray
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
            Row.Cells("Fecha").ReadOnly = False
        End If
        If Tipo = 6 And TipoNota = 60 And EsTrucha Then
            Row.Cells("Importe").ReadOnly = True
            Row.Cells("Cambio").ReadOnly = True
            Row.Cells("Cambio").Style.BackColor = Color.LightGray
            Row.Cells("Banco").ReadOnly = True
            Row.Cells("Cuenta").ReadOnly = True
            Row.Cells("Cuenta").Style.BackColor = Color.LightGray
            Row.Cells("Serie").ReadOnly = True
            Row.Cells("Numero").ReadOnly = True
            Row.Cells("EmisorCheque").ReadOnly = True
            Row.Cells("Comprobante").ReadOnly = True
            Row.Cells("Comprobante").Style.BackColor = Color.LightGray
            Row.Cells("FechaComprobante").Style.BackColor = Color.LightGray
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
        End If
        If Tipo = 7 Or Tipo = 15 Then  'Interbanking,ComprobanteMostrador,Transferencia Bancaria,Deposito, deposito exterior,Debito Auto.,Debito Auto.Dif.,Trenasf divisa,Interbanking divisa,deposito divisa.
            Row.Cells("Cambio").ReadOnly = True
            Row.Cells("Cambio").Style.BackColor = Color.LightGray
            Row.Cells("Banco").ReadOnly = True
            Row.Cells("Cuenta").ReadOnly = True
            Row.Cells("Serie").ReadOnly = True
            Row.Cells("Serie").Style.BackColor = Color.LightGray
            Row.Cells("Numero").ReadOnly = True
            Row.Cells("Numero").Style.BackColor = Color.LightGray
            Row.Cells("Fecha").Style.BackColor = Color.LightGray
            Row.Cells("EmisorCheque").ReadOnly = True
            Row.Cells("EmisorCheque").Style.BackColor = Color.LightGray
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
            If TipoNota = 600 Then
                Row.Cells("FechaComprobante").ReadOnly = False
            End If
        End If
        If Tipo = 8 Then                            'importe. 
            Row.Cells("Iva").ReadOnly = True
            Row.Cells("Importe").ReadOnly = True
            Row.Cells("Comprobante").ReadOnly = True
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
        End If
        If Tipo = 9 Then                            'ingreso bruto. 
            Row.Cells("Alicuota").ReadOnly = True
            Row.Cells("Iva").ReadOnly = True
            Row.Cells("Importe").ReadOnly = True
            Row.Cells("Comprobante").ReadOnly = True
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
        End If
        If Tipo = 10 Then                               'Vale Propios.
            Row.Cells("Cambio").ReadOnly = True
            Row.Cells("Cambio").Style.BackColor = Color.LightGray
            Row.Cells("Banco").ReadOnly = True
            Row.Cells("Banco").Style.BackColor = Color.LightGray
            Row.Cells("Cuenta").ReadOnly = True
            Row.Cells("Cuenta").Style.BackColor = Color.LightGray
            Row.Cells("LupaCuenta").Style.BackColor = Color.LightGray
            Row.Cells("Serie").ReadOnly = True
            Row.Cells("Serie").Style.BackColor = Color.LightGray
            Row.Cells("Numero").ReadOnly = True
            Row.Cells("Numero").Style.BackColor = Color.LightGray
            Row.Cells("Fecha").ReadOnly = True
            Row.Cells("Fecha").Style.BackColor = Color.LightGray
            Row.Cells("EmisorCheque").ReadOnly = True
            Row.Cells("EmisorCheque").Style.BackColor = Color.LightGray
        End If
        If Tipo = 11 And (TipoNota = 80 Or TipoNota = 600) Then     'Vale de Terceros en pase de caja,orden de pago.
            Row.Cells("Importe").ReadOnly = True
            Row.Cells("Cambio").ReadOnly = True
            Row.Cells("Cambio").Style.BackColor = Color.LightGray
            Row.Cells("Banco").ReadOnly = True
            Row.Cells("Banco").Style.BackColor = Color.LightGray
            Row.Cells("Cuenta").ReadOnly = True
            Row.Cells("Cuenta").Style.BackColor = Color.LightGray
            Row.Cells("Serie").ReadOnly = True
            Row.Cells("Serie").Style.BackColor = Color.LightGray
            Row.Cells("Numero").ReadOnly = True
            Row.Cells("Numero").Style.BackColor = Color.LightGray
            Row.Cells("Fecha").ReadOnly = True
            Row.Cells("Fecha").Style.BackColor = Color.LightGray
            Row.Cells("FechaComprobante").ReadOnly = True
            Row.Cells("FechaComprobante").Style.BackColor = Color.LightGray
            Row.Cells("Comprobante").ReadOnly = True
            Row.Cells("EmisorCheque").ReadOnly = True
            Row.Cells("EmisorCheque").Style.BackColor = Color.LightGray
            Row.Cells("ClaveCheque").Style.BackColor = Color.LightGray
            Row.Cells("Bultos").ReadOnly = True
            Row.Cells("Bultos").Style.BackColor = Color.LightGray
        End If

        If Not PAbierto Or GTipoIva = 2 Then
            Row.Cells("Alicuota").ReadOnly = True
            Row.Cells("Alicuota").Style.BackColor = Color.LightGray
            Row.Cells("Iva").ReadOnly = True
            Row.Cells("Iva").Style.BackColor = Color.LightGray
        End If

    End Sub
    Public Sub PresentaLineasGrid(ByVal Grilla As DataGridView, ByVal DTFormasPago As DataTable, ByVal TipoNota As Integer, ByVal EsTr As Boolean, ByVal EsRetencionManual As Boolean, ByVal OprNota As Boolean)

        'Precenta las lineas del grid.
        Dim RowsBusqueda() As DataRow

        For Each Row As DataGridViewRow In Grilla.Rows
            If Not IsNothing(Row.Cells("Concepto").Value) Then
                RowsBusqueda = DTFormasPago.Select("Clave = " & Row.Cells("Concepto").Value)
                ArmaGridSegunConcepto(Row, RowsBusqueda(0).Item("Tipo"), TipoNota, EsTr, EsRetencionManual, OprNota)
            End If
        Next

    End Sub
    Public Function DtTipoCuenta() As DataTable

        Dim Dt As New DataTable

        Try
            Dim Clave As DataColumn = New DataColumn("Clave")
            Clave.DataType = System.Type.GetType("System.Int32")
            Dt.Columns.Add(Clave)

            Dim Nombre As DataColumn = New DataColumn("Nombre")
            Nombre.DataType = System.Type.GetType("System.String")
            Dt.Columns.Add(Nombre)

            Dim Row As DataRow

            Row = Dt.NewRow
            Row("Clave") = 1
            Row("Nombre") = "CA"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 2
            Row("Nombre") = "CC"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            Return Dt
        Catch ex As Exception
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function DtEstadoActivoYBaja() As DataTable

        Dim Dt As New DataTable

        Try
            Dim Clave As DataColumn = New DataColumn("Clave")
            Clave.DataType = System.Type.GetType("System.Int32")
            Dt.Columns.Add(Clave)

            Dim Nombre As DataColumn = New DataColumn("Nombre")
            Nombre.DataType = System.Type.GetType("System.String")
            Dt.Columns.Add(Nombre)

            Dim Row As DataRow

            Row = Dt.NewRow
            Row("Clave") = 1
            Row("Nombre") = "Activo"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 3
            Row("Nombre") = "Anulado"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            Return Dt
        Catch ex As Exception
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function DtEstadoLegajoActivoYBaja() As DataTable

        Dim Dt As New DataTable

        Try
            Dim Clave As DataColumn = New DataColumn("Clave")
            Clave.DataType = System.Type.GetType("System.Int32")
            Dt.Columns.Add(Clave)

            Dim Nombre As DataColumn = New DataColumn("Nombre")
            Nombre.DataType = System.Type.GetType("System.String")
            Dt.Columns.Add(Nombre)

            Dim Row As DataRow

            Row = Dt.NewRow
            Row("Clave") = 1
            Row("Nombre") = "Activo"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 3
            Row("Nombre") = "Baja"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            Return Dt
        Catch ex As Exception
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function DtEstadoAfectaStockYBaja() As DataTable

        Dim Dt As New DataTable

        Try
            Dim Clave As DataColumn = New DataColumn("Clave")
            Clave.DataType = System.Type.GetType("System.Int32")
            Dt.Columns.Add(Clave)

            Dim Nombre As DataColumn = New DataColumn("Nombre")
            Nombre.DataType = System.Type.GetType("System.String")
            Dt.Columns.Add(Nombre)

            Dim Row As DataRow

            Row = Dt.NewRow
            Row("Clave") = 1
            Row("Nombre") = "Afecta Stock"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 3
            Row("Nombre") = "Anulado"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            Return Dt
        Catch ex As Exception
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function DtTipoFactura() As DataTable

        Dim Dt As New DataTable

        Try
            Dim Clave As DataColumn = New DataColumn("Clave")
            Clave.DataType = System.Type.GetType("System.Int32")
            Dt.Columns.Add(Clave)

            Dim Nombre As DataColumn = New DataColumn("Nombre")
            Nombre.DataType = System.Type.GetType("System.String")
            Dt.Columns.Add(Nombre)

            Dim Row As DataRow

            Row = Dt.NewRow
            Row("Clave") = 1
            Row("Nombre") = " A"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 2
            Row("Nombre") = " B"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 3
            Row("Nombre") = " C"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            Return Dt
        Catch ex As Exception
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function ArmaDtTipoIva() As DataTable

        Dim Dt As New DataTable

        Try
            Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
            Dt.Columns.Add("Nombre", Type.GetType("System.String"))
            Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

            Dim Row As DataRow = Dt.NewRow
            Row("Nombre") = "RESPONSABLE INSCR.(A)"
            Row("Clave") = 1
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            '            Row("Nombre") = "RESPONSABLE NO INSC."
            Row("Nombre") = "EXENTO"
            Row("Clave") = 2
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "CONSUMIDOR FINAL"
            Row("Clave") = 3
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "EXTERIOR"
            Row("Clave") = 4
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "RESPONSABLE INSCR.(M)"
            Row("Clave") = 5
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "MONOTRIBUTO"
            Row("Clave") = 6
            Dt.Rows.Add(Row)
            '       
            Row = Dt.NewRow
            Row("Nombre") = ""
            Row("Clave") = 0
            Dt.Rows.Add(Row)
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function ArmaNotasOtrosPagos() As DataTable

        Dim Dt As New DataTable

        Try
            Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
            Dt.Columns.Add("Nombre", Type.GetType("System.String"))
            ' 
            Dim Row As DataRow = Dt.NewRow
            Row("Nombre") = "Factura"
            Row("Clave") = 5000
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Orden Pago"
            Row("Clave") = 5010
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Devolución del Proveedor"
            Row("Clave") = 5020
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Nota Debito"
            Row("Clave") = 5005
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Nota Crédito"
            Row("Clave") = 5007
            Dt.Rows.Add(Row)
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function ArmaNotasFondosFijo() As DataTable

        Dim Dt As New DataTable

        Try
            Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
            Dt.Columns.Add("Nombre", Type.GetType("System.String"))
            ' 
            Dim Row As DataRow = Dt.NewRow
            Row("Nombre") = "Alta"
            Row("Clave") = 7000
            Dt.Rows.Add(Row)
            ' 
            Row = Dt.NewRow
            Row("Nombre") = "Ajuste (Aumenta)"
            Row("Clave") = 7001
            Dt.Rows.Add(Row)
            ' 
            Row = Dt.NewRow
            Row("Nombre") = "Ajuste (Disminuye)"
            Row("Clave") = 7002
            Dt.Rows.Add(Row)
            '
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Sub ArmaTipoPrecio(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Nombre") = ""
        Row("Clave") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "Uni."
        Row("Clave") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "Kgs."
        Row("Clave") = 2
        Dt.Rows.Add(Row)

    End Sub
    Public Function ArmaConceptosOtrosPagos() As DataTable

        Dim Dt As New DataTable

        Try
            Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
            Dt.Columns.Add("Nombre", Type.GetType("System.String"))
            ' 
            Dt = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 36;")
            '
            Dim Row As DataRow = Dt.NewRow
            Row("Nombre") = ""
            Row("Clave") = 0
            Dt.Rows.Add(Row)
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Sub ArmaTipoIva(ByVal Combo As ComboBox)

        Combo.DataSource = ArmaDtTipoIva()
        Combo.DisplayMember = "Nombre"
        Combo.ValueMember = "Clave"

    End Sub
    Public Function ArmaDtCajas() As DataTable

        Dim Dt As New DataTable

        Try
            If Not Tablas.Read("SELECT DISTINCT Caja AS Clave,RIGHT('0000' + CAST(Caja AS varchar),4) As Nombre FROM Usuarios WHERE Caja <> 0;", Conexion, Dt) Then End
            Dim Row1 As DataRow = Dt.NewRow
            Row1("Nombre") = ""
            Row1("Clave") = 0
            Dt.Rows.Add(Row1)
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function LetraTipoIva(ByVal Tipo As Integer) As String

        If Tipo = 1 Then Return "A"
        If Tipo = 2 Then Return "B"
        If Tipo = 3 Then Return "C"
        If Tipo = 4 Then Return "E"
        If Tipo = 5 Then Return "M"
        If Tipo = 9 Then Return "T" 'Ticket

    End Function
    Public Function HallaNumeroLetra(ByVal Letra As String) As Integer

        If Letra = "A" Then Return 1
        If Letra = "B" Then Return 2
        If Letra = "C" Then Return 3
        If Letra = "E" Then Return 4
        If Letra = "M" Then Return 5
        If Letra = "T" Then Return 9 'Ticket

    End Function
    Public Function ArmaConceptosPagoProveedores(ByVal EsImportacion As Boolean, ByVal Factura As Double) As DataTable

        Dim Dt As New DataTable
        Dim Row As DataRow

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        Dt.Columns.Add(Nombre)

        Dim Iva As New DataColumn("Iva")
        Iva.DataType = System.Type.GetType("System.Decimal")
        Dt.Columns.Add(Iva)

        Dim ImporteB As New DataColumn("ImporteB")
        ImporteB.DataType = System.Type.GetType("System.Decimal")
        Dt.Columns.Add(ImporteB)

        Dim ImporteN As New DataColumn("ImporteN")
        ImporteN.DataType = System.Type.GetType("System.Decimal")
        Dt.Columns.Add(ImporteN)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Tipo)

        Dim TieneLupa As New DataColumn("TieneLupa")
        TieneLupa.DataType = System.Type.GetType("System.Boolean")
        Dt.Columns.Add(TieneLupa)
        '
        Try
            Row = Dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = " "
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = 0
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 1
            Row("Nombre") = "Neto Gravado"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = 0
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 2
            Row("Nombre") = "Neto No Gravado"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = 0
            Dt.Rows.Add(Row)
            '
            If Not EsImportacion Then
                Row = Dt.NewRow
                Row("Clave") = 10
                Row("Nombre") = "Seña"
                Row("Iva") = 0
                Row("ImporteB") = 0
                Row("ImporteN") = 0
                Row("Tipo") = 0
                Row("TieneLupa") = 0
                Dt.Rows.Add(Row)
                '
                If Factura = 0 Then
                    If Not Tablas.Read("SELECT 0.0 AS ImporteB,0.0 AS ImporteN,Clave,Nombre,Iva,Tipo,0 AS TieneLupa FROM Tablas Where Tipo = 22 AND Activo = 1;", Conexion, Dt) Then End
                    If Not Tablas.Read("SELECT 0.0 AS ImporteB,0.0 AS ImporteN,T.Clave,T.Nombre,0 AS Iva,T.Tipo,0 AS TieneLupa  FROM Tablas AS T INNER JOIN DocuRetenciones AS D ON T.Clave = D.Clave Where T.Tipo = 25 AND D.TipoDocumento = 3;", Conexion, Dt) Then End
                Else
                    If Not Tablas.Read("SELECT 0.0 AS ImporteB,0.0 AS ImporteN,Clave,Nombre,Iva,Tipo,0 AS TieneLupa FROM Tablas Where Tipo = 22;", Conexion, Dt) Then End
                    If Not Tablas.Read("SELECT 0.0 AS ImporteB,0.0 AS ImporteN,Clave,Nombre,0 AS Iva,Tipo,0 AS TieneLupa FROM Tablas WHERE Tipo = 25;", Conexion, Dt) Then End
                End If
            End If
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function ArmaConceptosNVLP(ByVal Clave1 As Double) As DataTable

        Dim Dt As New DataTable

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Clave)
        '
        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        Dt.Columns.Add(Nombre)
        '
        Dim Iva As New DataColumn("Iva")
        Iva.DataType = System.Type.GetType("System.Double")
        Dt.Columns.Add(Iva)
        '
        Dim ImporteB As New DataColumn("ImporteB")
        ImporteB.DataType = System.Type.GetType("System.Double")
        Dt.Columns.Add(ImporteB)
        '
        Dim ImporteN As New DataColumn("ImporteN")
        ImporteN.DataType = System.Type.GetType("System.Double")
        Dt.Columns.Add(ImporteN)
        '
        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Tipo)
        '
        Dim TieneLupa As New DataColumn("TieneLupa")
        TieneLupa.DataType = System.Type.GetType("System.Boolean")
        Dt.Columns.Add(TieneLupa)
        '
        Try
            Dim Row As DataRow = Dt.NewRow
            Row("Clave") = 1
            Row("Nombre") = "Neto Gravado"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = False
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 2
            Row("Nombre") = "Neto No Gravado"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = False
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 3
            Row("Nombre") = "I.V.A."
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = False
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 5
            Row("Nombre") = "Comisión"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = False
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 7
            Row("Nombre") = "Descarga"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = False
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 9
            Row("Nombre") = "Flete Terrestre"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = False
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 10
            Row("Nombre") = "Otros Conceptos"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = False
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 6
            Row("Nombre") = "I.V.A. Comisión"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = False
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 8
            Row("Nombre") = "I.V.A. Descarga"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = False
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 11
            Row("Nombre") = "I.V.A. Flete Terrestre"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = False
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Clave") = 12
            Row("Nombre") = "I.V.A. Otros Conceptos"
            Row("Iva") = 0
            Row("ImporteB") = 0
            Row("ImporteN") = 0
            Row("Tipo") = 0
            Row("TieneLupa") = False
            Dt.Rows.Add(Row)
            '
            'Si se agrega otros medios ver los listados impositivos e importcion a la AFIP.
            If Clave1 = 0 Then
                If Not Tablas.Read("SELECT 0.0 AS ImporteB,0.0 AS ImporteN,T.Clave,T.Nombre,0 AS Iva, 4 AS Tipo,0 AS TieneLupa FROM Tablas AS T INNER JOIN DocuRetenciones AS D ON T.Clave = D.Clave Where T.Tipo = 25 AND D.TipoDocumento = 800;", Conexion, Dt) Then End
            Else
                If Not Tablas.Read("SELECT 0.0 AS ImporteB,0.0 AS ImporteN,Clave,Nombre,0 AS Iva, 4 AS Tipo,0 AS TieneLupa FROM Tablas Where Tipo = 25;", Conexion, Dt) Then End
            End If

            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function DtAfectaPendienteAnulada() As DataTable

        Dim Dt As New DataTable

        Dt.Columns.Add("Codigo", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Try
            Dim Row As DataRow = Dt.NewRow
            Row("Codigo") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 1
            Row("Nombre") = "Afecta Stock"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 2
            Row("Nombre") = "Pendiente"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 3
            Row("Nombre") = "Anulada"
            Dt.Rows.Add(Row)
            '
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function DtEstadoActivoSuspendidoBaja() As DataTable

        Dim Dt As New DataTable

        Dt.Columns.Add("Codigo", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Try
            Dim Row As DataRow = Dt.NewRow
            Row("Codigo") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 1
            Row("Nombre") = "Activo"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 2
            Row("Nombre") = "Suspendido"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 3
            Row("Nombre") = "Anulado"
            Dt.Rows.Add(Row)
            '
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function DtActivoDeshabilitado() As DataTable

        Dim Dt As New DataTable

        Dt.Columns.Add("Codigo", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Try
            Dim Row As DataRow = Dt.NewRow
            Row("Codigo") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 1
            Row("Nombre") = "Activo"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 3
            Row("Nombre") = "Deshabilitado"
            Dt.Rows.Add(Row)          '
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function DtTiposComprobantes(ByVal Abierto As Boolean) As DataTable

        Dim Dt As New DataTable

        Dt.Columns.Add("Codigo", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Try
            Dim Row As DataRow = Dt.NewRow
            Row("Codigo") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 2
            Row("Nombre") = "Factura"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 4
            Row("Nombre") = "Nota Credito"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 5
            Row("Nombre") = "Nota Debito(F)"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 13005
            Row("Nombre") = "Nota Debito Interna"
            Dt.Rows.Add(Row)           '
            '
            Row = Dt.NewRow
            Row("Codigo") = 6
            Row("Nombre") = "Nota Debito"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 13006
            Row("Nombre") = "Nota Debito Interna"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 7
            Row("Nombre") = "Nota Credito(F)"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 13007
            Row("Nombre") = "Nota Credito Interna"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 8
            Row("Nombre") = "Nota Credito"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 13008
            Row("Nombre") = "Nota Credito Interna"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 10
            Row("Nombre") = "Liquidación"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 30
            Row("Nombre") = "Seña"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 50
            Row("Nombre") = "Nota Debito(C)"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 60
            Row("Nombre") = "Cobranza"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 64
            Row("Nombre") = "Devolución a Cliente"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 65
            Row("Nombre") = "Devol.Seña a Cliente"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 70
            Row("Nombre") = "Nota Credito(C)"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 500
            Row("Nombre") = "Nota Debito(P)"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 600
            Row("Nombre") = "Orden Pago"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 604
            Row("Nombre") = "Devolución del Proveedor"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 700
            Row("Nombre") = "Nota Credito(P)"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 90
            Row("Nombre") = "Extraccion"   'En banco propios.
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 91
            Row("Nombre") = "Deposito"   'En banco propios.
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 92
            Row("Nombre") = "Transferencia Propia"   'En banco propios.
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 93
            Row("Nombre") = "Rechazo Cheque"   'En banco propios.
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 80
            Row("Nombre") = "Pase de Caja"   'Pase de Caja.
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 800
            Row("Nombre") = "N.V.L.P."   'Nota venta y liquido producto.
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 900
            Row("Nombre") = "Saldo Inicial"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 11
            Row("Nombre") = "Conbranza en Argentina"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 12
            Row("Nombre") = "Conbranza en el Exterior"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 13
            Row("Nombre") = "Conbranza a Aduana"
            Dt.Rows.Add(Row)
            '------
            Row = Dt.NewRow
            Row("Nombre") = "Prestamo"
            Row("Codigo") = 1000
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Cancelación Prestamo"
            Row("Codigo") = 1010
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Ajuste Capital"
            Row("Codigo") = 1015
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Gastos Bancario"
            Row("Codigo") = 3000
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Pago Sueldo"
            Row("Codigo") = 4010
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Orden Pago O.P."
            Row("Codigo") = 5010
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Devolución O.P."
            Row("Codigo") = 5020
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Recupero Seña"
            Row("Codigo") = 66
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Compra Divisas"
            Row("Codigo") = 6000
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Venta Divisas"
            Row("Codigo") = 6001
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Alta Fondo Fijo"
            Row("Codigo") = 7000
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Ajuste - Aumenta"
            Row("Codigo") = 7001
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Ajuste - Disminuye"
            Row("Codigo") = 7002
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Rendición"
            Row("Codigo") = 7003
            Dt.Rows.Add(Row)
            '
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Sub ArmaTiposCliente(ByVal Combo As ComboBox)

        Dim Dt As New DataTable

        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Nombre") = "Nota Debito(C)"
        Row("Tipo") = 50
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "Nota Credito(C)"
        Row("Tipo") = 70
        Dt.Rows.Add(Row)
        '
        '  Row = Dt.NewRow
        '  Row("Nombre") = "Devolución Seña"
        '  Row("Tipo") = 65
        '  Dt.Rows.Add(Row)
        '       
        Row = Dt.NewRow
        Row("Nombre") = ""
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Combo.DataSource = Dt.Copy
        Combo.DisplayMember = "Nombre"
        Combo.ValueMember = "Tipo"

        Dt.Dispose()

    End Sub
    Public Function DtTiposTiposNotasCaja(ByVal Abierto As Boolean) As DataTable

        Dim Dt As New DataTable

        Dt.Columns.Add("Codigo", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Try
            Dim Row As DataRow = Dt.NewRow
            Row("Codigo") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 60
            Row("Nombre") = "Cobranza"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 64
            Row("Nombre") = "Devolución a Cliente"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 600
            Row("Nombre") = "Orden Pago"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 604
            Row("Nombre") = "Devolución del Proveedor"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 90
            Row("Nombre") = "Extraccion"   'En banco propios.
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 91
            Row("Nombre") = "Deposito"   'En banco propios.
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 92
            Row("Nombre") = "Transferencia Propia"   'En banco propios.
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 80
            Row("Nombre") = "Pase de Caja"   'Pase de Caja.
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 10
            Row("Nombre") = "Cobranza en el Exterior"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 11
            Row("Nombre") = "Cobranza en Argentina"
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 5010
            Row("Nombre") = "Orden pago O.P."
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Codigo") = 5020
            Row("Nombre") = "Devolución O.P."
            Dt.Rows.Add(Row)
            '
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Sub ArmaTiposProveedor(ByVal combo As ComboBox)

        Dim Dt As New DataTable

        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Nombre") = "Nota Debito del Prov."
        Row("Tipo") = 500
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "Nota Credito del Prov."
        Row("Tipo") = 700
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "Nota Debito Financiera"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "Nota Credito Financiera"
        Row("Tipo") = 8
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "Nota Debito Financiera Interna"
        Row("Tipo") = 13006
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "Nota Credito Financiera Interna"
        Row("Tipo") = 13008
        Dt.Rows.Add(Row)
        '       
        Row = Dt.NewRow
        Row("Nombre") = ""
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        combo.DataSource = Dt.Copy
        combo.DisplayMember = "Nombre"
        combo.ValueMember = "Tipo"

        Dt.Dispose()

    End Sub
    Public Sub ArmaTiposPrestamo(ByVal Combo As ComboBox)

        Dim Dt As New DataTable

        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Dim Row As DataRow = Dt.NewRow
        Row("Nombre") = "Cancelación Capital,Intereses,Gastos"
        Row("Clave") = 1010
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "Ajuste Capital"
        Row("Clave") = 1015
        Dt.Rows.Add(Row)
        '
        ''Row = Dt.NewRow
        ''Row("Nombre") = "Nota Debito"
        ''Row("Clave") = 1005
        ''      Dt.Rows.Add(Row)
        '
        ''Row = Dt.NewRow
        ''Row("Nombre") = "Nota Credito"
        ''Row("Clave") = 1007
        ''    Dt.Rows.Add(Row)
        '       
        Row = Dt.NewRow
        Row("Nombre") = ""
        Row("Clave") = 0
        Dt.Rows.Add(Row)
        '
        Combo.DataSource = Dt.Copy
        Combo.DisplayMember = "Nombre"
        Combo.ValueMember = "Clave"

        Dt.Dispose()

    End Sub
    Public Sub ArmaIvaProveedores(ByVal Combo As ComboBox)

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre,Iva FROM Tablas Where Tipo = 22 AND Activo = 1;", Conexion, Dt) Then End

        For Each Row1 As DataRow In Dt.Rows
            Row1("Nombre") = Format(Row1("Iva"), "0.00")
        Next
        '
        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = "00,00"
        Row("Iva") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = ""
        Row("Iva") = 0
        Dt.Rows.Add(Row)
        '
        Combo.DataSource = Dt.Copy
        Combo.DisplayMember = "Nombre"
        Combo.ValueMember = "Clave"

        Dt.Dispose()

    End Sub
    Public Sub ArmaMedioPagoTodos(ByRef Dt As DataTable, ByVal Abierto As Boolean)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "ChequePropio"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 8
        Row("Nombre") = "CompMostrador"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 9
        Row("Nombre") = "Debito Auto."
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 14
        Row("Nombre") = "Debito Auto.Dife."
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 11
        Row("Nombre") = "Transferencia"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 12
        Row("Nombre") = "Deposito"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 92
        Row("Nombre") = "TransferenciaPropia"
        Row("Tipo") = 92
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 15
        Row("Nombre") = "InterBanking Divisa"
        Row("Tipo") = 15
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 16
        Row("Nombre") = "Transferencia Divisa"
        Row("Tipo") = 15
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 17
        Row("Nombre") = "Deposito Divisa"
        Row("Tipo") = 15
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 5
        Row("Nombre") = "Vale Propios"
        Row("Tipo") = 10
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 6
        Row("Nombre") = "Vale Terceros"
        Row("Tipo") = 11
        Dt.Rows.Add(Row)
        '
        If Not Abierto Then
            Row = Dt.NewRow
            Row("Clave") = 10
            Row("Nombre") = "Seña"
            Row("Tipo") = 5
            Dt.Rows.Add(Row)
        End If
        '
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End
        '
        If Not Tablas.Read("SELECT 4 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 25;", Conexion, Dt) Then End

    End Sub
    Public Sub ArmaMedioPagoTodosYGastos(ByRef Dt As DataTable, ByVal Abierto As Boolean)

        ArmaMedioPagoTodos(Dt, Abierto)
        ' 
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 33;", Conexion, Dt) Then End
        '
        If Not Tablas.Read("SELECT 22 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 22;", Conexion, Dt) Then End

    End Sub
    Public Sub ArmaMedioPagoOrden(ByRef Dt As DataTable, ByVal Abierto As Boolean, ByVal Clave As Double)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "ChequePropio"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 9
        Row("Nombre") = "Debito Auto."
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 14
        Row("Nombre") = "Debito Auto. Dife."
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 11
        Row("Nombre") = "Transferencia"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 6
        Row("Nombre") = "Vale Terceros"
        Row("Tipo") = 11
        Dt.Rows.Add(Row)
        '
        If Not Abierto Then
            '     Row = Dt.NewRow
            '     Row("Clave") = 5
            '    Row("Nombre") = "Vale"
            '   Row("Tipo") = 10
            '  Dt.Rows.Add(Row)
        End If
        '
        'If Not Abierto Then
        ' Row = Dt.NewRow
        ' Row("Clave") = 10
        ' Row("Nombre") = "Seña"
        ' Row("Tipo") = 5
        ' Dt.Rows.Add(Row)
        ' End If
        '
        'Moneda extranjera.
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End
        'Retenciones.
        If Abierto Then
            If Clave = 0 Then
                If Not Tablas.Read("SELECT 4 AS Tipo,T.Clave,T.Nombre FROM Tablas AS T INNER JOIN DocuRetenciones AS D ON T.Clave = D.Clave Where T.Tipo = 25 AND D.TipoDocumento = 600;", Conexion, Dt) Then End
            Else
                If Not Tablas.Read("SELECT 4 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 25;", Conexion, Dt) Then End
            End If
        End If

    End Sub
    Public Sub ArmaMedioPagoOrdenPagoClientes(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "ChequePropio"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 11
        Row("Nombre") = "Transferencia"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        'Moneda extranjera.
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End

    End Sub
    Public Sub ArmaMedioPagoDevolucionSenia(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "ChequePropio"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 11
        Row("Nombre") = "Transferencia"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
    End Sub
    Public Sub ArmaMedioPagoOrdenFondoFijo(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "ChequePropio"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 9
        Row("Nombre") = "Debito Auto."
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 11
        Row("Nombre") = "Transferencia"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        'Moneda extranjera.
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End

    End Sub
    Public Sub ArmaMedioPagoCobranzaFondoFijo(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 12
        Row("Nombre") = "Deposito"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End

    End Sub
    Public Sub ArmaMedioPagoOrdenOtrosPagos(ByRef Dt As DataTable, ByVal Abierto As Boolean)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "ChequePropio"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 9
        Row("Nombre") = "Debito Auto."
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 14
        Row("Nombre") = "Debito Auto.Dife."
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 11
        Row("Nombre") = "Transferencia"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)

    End Sub
    Public Sub ArmaMedioPagoDevolucionOtrosPagos(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 11
        Row("Nombre") = "Transferencia"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)

    End Sub
    Public Sub ArmaTruchaEspecial(ByRef Dt As DataTable, ByVal Abierto As Boolean, ByVal Clave As Double)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)

    End Sub
    Public Sub ArmaTrucha(ByRef Dt As DataTable, ByVal Abierto As Boolean, ByVal Clave As Double)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "ChequePropio"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)

        If Abierto Then
            If Clave = 0 Then
                If Not Tablas.Read("SELECT 4 AS Tipo,T.Clave,T.Nombre FROM Tablas AS T INNER JOIN DocuRetenciones AS D ON T.Clave = D.Clave Where T.Tipo = 25 AND D.TipoDocumento = 600;", Conexion, Dt) Then End
                If Not Tablas.Read("SELECT 4 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 25 AND Activo4 = 1 AND Formula = 1;", Conexion, Dt) Then End
            Else
                If Not Tablas.Read("SELECT 4 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 25;", Conexion, Dt) Then End
            End If
        End If

    End Sub
    Public Sub ArmaReemplazo(ByRef Dt As DataTable, ByVal Abierto As Boolean, ByVal Clave As Double)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "ChequePropio"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)

    End Sub
    Public Sub ArmaMedioPagoCobranza(ByRef Dt As DataTable, ByVal Abierto As Boolean, ByVal Clave As Double)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        '      Row = Dt.NewRow
        '      Row("Clave") = 2
        '      Row("Nombre") = "ChequePropio"
        '      Row("Tipo") = 2
        '      Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 12
        Row("Nombre") = "Deposito"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 5
        Row("Nombre") = "Vale Propio"
        Row("Tipo") = 10
        Dt.Rows.Add(Row)
        '
        If Not Abierto Then
            '          Row = Dt.NewRow
            '        Row("Clave") = 10
            '       Row("Nombre") = "Seña"
            '      Row("Tipo") = 5
            '     Dt.Rows.Add(Row)
        End If
        '
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End
        '
        If Abierto Then
            If Clave = 0 Then
                If Not Tablas.Read("SELECT 4 AS Tipo,T.Clave,T.Nombre FROM Tablas AS T INNER JOIN DocuRetenciones AS D ON T.Clave = D.Clave Where T.Tipo = 25 AND D.TipoDocumento = 60;", Conexion, Dt) Then End
            Else
                If Not Tablas.Read("SELECT 4 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 25;", Conexion, Dt) Then End
            End If
        End If

    End Sub
    Public Sub ArmaCobranzaTrucha(ByRef Dt As DataTable, ByVal Abierto As Boolean, ByVal Clave As Double)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        If Not Abierto Then
            '          Row = Dt.NewRow
            '        Row("Clave") = 10
            '       Row("Nombre") = "Seña"
            '      Row("Tipo") = 5
            '     Dt.Rows.Add(Row)
        End If
        '
        If Abierto Then
            If Clave = 0 Then
                If Not Tablas.Read("SELECT 4 AS Tipo,T.Clave,T.Nombre FROM Tablas AS T INNER JOIN DocuRetenciones AS D ON T.Clave = D.Clave Where T.Tipo = 25 AND D.TipoDocumento = 60;", Conexion, Dt) Then End
            Else
                If Not Tablas.Read("SELECT 4 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 25;", Conexion, Dt) Then End
            End If
        End If

    End Sub
    Public Sub ArmaMedioPagoCobranzaProveedores(ByRef Dt As DataTable, ByVal Abierto As Boolean, ByVal Clave As Double)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 12
        Row("Nombre") = "Deposito"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End
        '
        If Abierto Then
            If Clave = 0 Then
                If Not Tablas.Read("SELECT 4 AS Tipo,T.Clave,T.Nombre FROM Tablas AS T INNER JOIN DocuRetenciones AS D ON T.Clave = D.Clave Where T.Tipo = 25 AND D.TipoDocumento = 604;", Conexion, Dt) Then End
            Else
                If Not Tablas.Read("SELECT 4 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 25;", Conexion, Dt) Then End
            End If
        End If

    End Sub
    Public Sub ArmaMedioPagoCobranzaExterior(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 11
        Row("Nombre") = "Transferencia"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 12
        Row("Nombre") = "Deposito"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        ' 
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End

    End Sub
    Public Sub ArmaMedioPagoPrestamo(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "ChequePropio"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 12
        Row("Nombre") = "Deposito"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 9
        Row("Nombre") = "Debito Auto."
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 11
        Row("Nombre") = "Transferencia"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End

    End Sub
    Public Sub ArmaMedioPagoSueldo(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "ChequePropio"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 12
        Row("Nombre") = "Deposito"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 11
        Row("Nombre") = "Transferencia"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)

    End Sub
    Public Sub ArmaMedioPagoParaPrestamo(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 12
        Row("Nombre") = "Deposito"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 11
        Row("Nombre") = "Transferencia"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End

    End Sub
    Public Sub ArmaMedioExtraccion(ByRef Dt As DataTable, ByVal Abierto As Boolean)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 8
        Row("Nombre") = "CompMostrador"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "ChequePropio"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)
        '
    End Sub
    Public Sub ArmaMedioExtraccionEfectivo(ByRef Dt As DataTable, ByVal Abierto As Boolean)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 8
        Row("Nombre") = "CompMostrador"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '

    End Sub
    Public Sub ArmaMedioExtraccionChequePropio(ByRef Dt As DataTable, ByVal Abierto As Boolean)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "ChequePropio"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)
        '
    End Sub
    Public Sub ArmaMedioDeposito(ByRef Dt As DataTable, ByVal Abierto As Boolean)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 12
        Row("Nombre") = "Deposito"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '

    End Sub
    Public Sub ArmaMedioDepositoEfectivo(ByRef Dt As DataTable, ByVal Abierto As Boolean)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 12
        Row("Nombre") = "Deposito"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)

    End Sub
    Public Sub ArmaMedioDepositoChequesTerceros(ByRef Dt As DataTable, ByVal Abierto As Boolean)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)

    End Sub
    Public Sub ArmaMedioPagoOtras(ByVal TipoNota As Integer, ByRef Dt As DataTable, ByVal Abierto As Boolean, ByVal Nota As Double)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 100
        Row("Nombre") = "Importe"
        Row("Tipo") = 8
        Dt.Rows.Add(Row)
        '
        Dim TipoNotaW As Integer
        Select Case TipoNota
            Case 5, 6
                TipoNotaW = 5
            Case 7, 8
                TipoNotaW = 7
            Case Else
                TipoNotaW = TipoNota
        End Select

        If Abierto Then
            If Nota = 0 Then
                If Not Tablas.Read("SELECT T.Clave,T.Nombre,4 AS Tipo FROM Tablas AS T INNER JOIN DocuRetenciones AS D ON T.Clave = D.Clave Where T.Tipo = 25 AND D.TipoDocumento = " & TipoNotaW & ";", Conexion, Dt) Then End
            Else
                If Not Tablas.Read("SELECT Clave,Nombre,4 AS Tipo FROM Tablas Where Tipo = 25" & ";", Conexion, Dt) Then End
            End If
        End If

    End Sub
    Public Sub ArmaMedioPagoOtrasExterior(ByRef Dt As DataTable, ByVal Abierto As Boolean)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 100
        Row("Nombre") = "Importe"
        Row("Tipo") = 8
        Dt.Rows.Add(Row)

    End Sub
    Public Sub ArmaMedioPagoPase(ByRef Dt As DataTable, ByVal Abierto As Boolean)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 6
        Row("Nombre") = "Vales Terceros"
        Row("Tipo") = 11
        Dt.Rows.Add(Row)
        '
        'Moneda extranjera.
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End

    End Sub
    Public Sub ArmaDueño(ByRef Dt As DataTable, ByVal Abierto As Boolean)

        Dt = New DataTable
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "PROPIO"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "PRODUCTOR"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "IFCO"
        Row("Tipo") = 3
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "ARGEN POOL"
        Row("Tipo") = 4
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "OTROS"
        Row("Tipo") = 5
        Dt.Rows.Add(Row)
        '
    End Sub
    Public Sub ArmaCalculoFlete(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "Bulto"
        Row("Tipo") = Bulto
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "Medio Bulto"
        Row("Tipo") = MedioBulto
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "Unidad"
        Row("Tipo") = Unidad
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Nombre") = "Kilo"
        Row("Tipo") = Kilo
        Dt.Rows.Add(Row)

    End Sub
    Public Sub ArmaMedioPagoCobroDivisas(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "ChequePropio"
        Row("Tipo") = 2
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 3
        Row("Nombre") = "ChequeTercero"
        Row("Tipo") = 6
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 12
        Row("Nombre") = "Deposito"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 4
        Row("Nombre") = "InterBanking"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 11
        Row("Nombre") = "Transferencia"
        Row("Tipo") = 7
        Dt.Rows.Add(Row)
        '
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End

    End Sub
    Public Sub ArmaMedioPagoCompraDivisas(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 17
        Row("Nombre") = "Deposito Divisa"
        Row("Tipo") = 15
        Dt.Rows.Add(Row)
        '
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End

    End Sub
    Public Sub ArmaMedioPagoVentaDivisas(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Tipo") = 0
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 15
        Row("Nombre") = "InterBanking Divisa"
        Row("Tipo") = 15
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 16
        Row("Nombre") = "Transferencia Divisa"
        Row("Tipo") = 15
        Dt.Rows.Add(Row)
        '
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End

    End Sub
    Public Function ArmaConceptosPrestamo(ByVal Abierto As Boolean) As DataTable

        Dim dt As New DataTable

        Try
            dt = New DataTable
            dt.Columns.Add("Clave", Type.GetType("System.Int32"))
            dt.Columns.Add("Nombre", Type.GetType("System.String"))

            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = " "
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 1
            Row("Nombre") = "Capital"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 2
            Row("Nombre") = "Ajuste Capital"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 6
            Row("Nombre") = "Capital A Cancelar"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 7
            Row("Nombre") = "Intereses"
            dt.Rows.Add(Row)
            '
            If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas Where Tipo = 32;", Conexion, dt) Then End
            '
            If Abierto Then
                If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas Where Tipo = 22;", Conexion, dt) Then End
            End If
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ArmaTodosConceptosPrestamo(ByVal Abierto As Boolean) As DataTable

        Dim dt As New DataTable

        Try
            dt = New DataTable
            dt.Columns.Add("Clave", Type.GetType("System.Int32"))
            dt.Columns.Add("Nombre", Type.GetType("System.String"))

            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = " "
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 1
            Row("Nombre") = "Capital"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 2
            Row("Nombre") = "Ajuste Capital"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 6
            Row("Nombre") = "Capital A Cancelar"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 7
            Row("Nombre") = "Intereses"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 100
            Row("Nombre") = "Importe"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 93
            Row("Nombre") = "Rechazo Cheque"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 94
            Row("Nombre") = "Reemplazo Cheque"
            dt.Rows.Add(Row)
            '
            If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas Where Tipo = 32;", Conexion, dt) Then End
            '
            If Abierto Then
                If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas Where Tipo = 22;", Conexion, dt) Then End
                If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas Where Tipo = 25;", Conexion, dt) Then End
            End If
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ArmaTodosDocumentosPrestamo(ByVal Abierto As Boolean) As DataTable

        Dim dt As New DataTable

        Try
            dt = New DataTable
            dt.Columns.Add("Clave", Type.GetType("System.Int32"))
            dt.Columns.Add("Nombre", Type.GetType("System.String"))

            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = " "
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 1000
            Row("Nombre") = "Capital Prestamo"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 1010
            Row("Nombre") = "Cancelación Prestamo"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 1015
            Row("Nombre") = "Ajuste Capital"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 1005
            Row("Nombre") = "Nota Debito"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 1007
            Row("Nombre") = "Nota Credito"
            dt.Rows.Add(Row)
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ArmaTodosConceptosSueldos(ByVal Abierto As Boolean) As DataTable

        Dim dt As New DataTable

        Try
            dt = New DataTable
            dt.Columns.Add("Clave", Type.GetType("System.Int32"))
            dt.Columns.Add("Nombre", Type.GetType("System.String"))

            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = " "
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 1
            Row("Nombre") = "Pago Sueldo"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 93
            Row("Nombre") = "Rechazo Cheque"
            dt.Rows.Add(Row)
            '
            If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas Where Tipo = 34;", Conexion, dt) Then End
            '
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ArmaMovimientosSueldo() As DataTable

        Dim dt As New DataTable

        Try
            dt = New DataTable
            dt.Columns.Add("Clave", Type.GetType("System.Int32"))
            dt.Columns.Add("Nombre", Type.GetType("System.String"))

            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = " "
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 4010
            Row("Nombre") = "Pago Sueldo"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 4000
            Row("Nombre") = "Recibo Sueldo"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 4005
            Row("Nombre") = "Nota Debito"
            dt.Rows.Add(Row)
            '
            Row = dt.NewRow
            Row("Clave") = 4007
            Row("Nombre") = "Nota Credito"
            dt.Rows.Add(Row)
            '
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ArmaProductos() As DataTable

        Dim Dt As New DataTable

        Try
            Dt.Columns.Add("Codigo", Type.GetType("System.Int32"))
            Dt.Columns.Add("Nombre", Type.GetType("System.String"))

            Dim Row As DataRow = Dt.NewRow
            Row("Nombre") = "Articulos"
            Row("Codigo") = 5
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Servicios"
            Row("Codigo") = 6
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Insumos"
            Row("Codigo") = 7
            Dt.Rows.Add(Row)
            '
            Row = Dt.NewRow
            Row("Nombre") = "Transportes"
            Row("Codigo") = 8
            Dt.Rows.Add(Row)
            '       
            Row = Dt.NewRow
            Row("Nombre") = ""
            Row("Codigo") = 0
            Dt.Rows.Add(Row)
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function ArmaConceptosParaGastosBancarios(ByVal Movimiento As Double, ByVal Abierto As Boolean) As DataTable

        Dim Dt As New DataTable

        Try
            Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
            Dt.Columns.Add("Nombre", Type.GetType("System.String"))
            Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))
            Dt.Columns.Add("TieneLupa", Type.GetType("System.Boolean"))
            Dt.Columns.Add("Activo2", Type.GetType("System.Int32"))
            Dt.Columns.Add("Alicuota", Type.GetType("System.Decimal"))

            Dim Row As DataRow = Dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = " "
            Row("Tipo") = 0
            Row("TieneLupa") = False
            Row("Activo2") = 0
            Row("Alicuota") = 0
            Dt.Rows.Add(Row)
            '
            If Not Tablas.Read("SELECT Clave,Nombre,Tipo,Operador,0 AS TieneLupa,Activo2,0 AS Alicuota FROM Tablas Where Tipo = 33;", Conexion, Dt) Then End
            '
            If Abierto Then
                If Not Tablas.Read("SELECT Clave,Nombre,Tipo,2 AS Operador,0 AS TieneLupa,0 AS Activo2,Iva AS Alicuota FROM Tablas Where Tipo = 22;", Conexion, Dt) Then End
                '
                If Movimiento = 0 Then
                    If Not Tablas.Read("SELECT T.Clave,T.Nombre,T.Tipo,2 AS Operador,0 AS TieneLupa,0 AS Activo2,0 AS Alicuota FROM Tablas AS T INNER JOIN DocuRetenciones AS D ON T.Clave = D.Clave Where T.Tipo = 25 AND D.TipoDocumento = 3000;", Conexion, Dt) Then End
                Else
                    If Not Tablas.Read("SELECT Clave,Nombre,Tipo,2 AS Operador,0 AS TieneLupa,0 AS Activo2,0 AS Alicuota FROM Tablas Where Tipo = 25;", Conexion, Dt) Then End
                End If
            End If
            ' 
            Return Dt
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function CreaDtAFacturar() As DataTable

        Dim DtAFacturar As New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtAFacturar.Columns.Add(Operacion)

        Dim Indice As New DataColumn("Indice")
        Indice.DataType = System.Type.GetType("System.Int32")
        DtAFacturar.Columns.Add(Indice)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtAFacturar.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtAFacturar.Columns.Add(Secuencia)

        Dim LoteYSecuencia As New DataColumn("LoteYSecuencia")
        LoteYSecuencia.DataType = System.Type.GetType("System.String")
        DtAFacturar.Columns.Add(LoteYSecuencia)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtAFacturar.Columns.Add(Articulo)

        Dim Descuento As New DataColumn("Descuento")
        Descuento.DataType = System.Type.GetType("System.Decimal")
        DtAFacturar.Columns.Add(Descuento)

        Dim KilosXUnidad As New DataColumn("KilosXUnidad")
        KilosXUnidad.DataType = System.Type.GetType("System.Decimal")
        DtAFacturar.Columns.Add(KilosXUnidad)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtAFacturar.Columns.Add(Cantidad)

        Dim TipoPrecio As New DataColumn("TipoPrecio")
        TipoPrecio.DataType = System.Type.GetType("System.Int32")
        DtAFacturar.Columns.Add(TipoPrecio)

        Dim Precio As New DataColumn("Precio")
        Precio.DataType = System.Type.GetType("System.Decimal")
        DtAFacturar.Columns.Add(Precio)

        Dim Senia As New DataColumn("Senia")
        Senia.DataType = System.Type.GetType("System.Decimal")
        DtAFacturar.Columns.Add(Senia)

        Return DtAFacturar

    End Function
    Public Sub ProtejeGrid(ByVal Grid As DataGridView)

        Dim Row As DataGridViewRow

        For i As Integer = 0 To Grid.Rows.Count - 2
            Row = Grid.Rows(i)
            For Y As Integer = 0 To Grid.Columns.Count - 1
                Row.Cells(Y).ReadOnly = True
            Next
        Next

    End Sub
    Public Function HallaSaldoCtaCteCliente(ByVal Cliente As Integer, ByVal Abierto As Boolean) As Double

        Dim Sql As String
        If Abierto Then
            Sql = ArmaSqlCtaCteClienteB(Cliente)
        Else : Sql = ArmaSqlCtaCteClienteN(Cliente)
        End If

        Dim Dt As New DataTable
        If Abierto = True Then
            If Not Tablas.Read(Sql, Conexion, Dt) Then End
        End If
        If Abierto = False Then
            If Not Tablas.Read(Sql, ConexionN, Dt) Then End
        End If

        Dim SaldoCta As Double = 0

        For Each Row As DataRow In Dt.Rows
            If Row("Tipo") = 2 Then
                'Analiza una factura.
                SaldoCta = SaldoCta + Row("Importe")
            End If
            If Row("Tipo") = 4 Then         'Analiza Notas de Credito Con devolucion de articulo.
                SaldoCta = SaldoCta - Row("Importe")
            End If
            If Row("Tipo") = 800 Then         'Analiza NVLP.
                SaldoCta = SaldoCta + Row("Importe")
            End If
            If Row("Tipo") = 50 Then         'Analiza Notas debitos del Cliente.
                SaldoCta = SaldoCta - Row("Importe")
            End If
            If Row("Tipo") = 500 Then        'Analiza Notas debitos del Proveedor. 
                SaldoCta = SaldoCta - Row("Importe")
            End If
            If Row("Tipo") = 60 Then          'Analiza Pago del Cliente.
                SaldoCta = SaldoCta - Row("Importe")
            End If
            If Row("Tipo") = 600 Then         'Analiza Pago del Proveedor.
                SaldoCta = SaldoCta + Row("Importe")
            End If
            If Row("Tipo") = 70 Then          'Analiza Nota Credito del Cliente.
                SaldoCta = SaldoCta + Row("Importe")
            End If
            If Row("Tipo") = 700 Then         'Analiza Nota Credito del Proveedor.
                SaldoCta = SaldoCta + Row("Importe")
            End If
            If Row("Tipo") = 5 Or Row("Tipo") = 13005 Then           'Analiza Nota Debito.
                SaldoCta = SaldoCta + Row("Importe")
            End If
            If Row("Tipo") = 7 Or Row("Tipo") = 13007 Then           'Analiza Nota Credito.
                SaldoCta = SaldoCta - Row("Importe")
            End If
            If Row("Tipo") = 10 Then           'Analiza Liquidacion A Proveedores.
                SaldoCta = SaldoCta - Row("Importe")
            End If
            If Row("Tipo") = 30 Then           'Analiza Sena.
                SaldoCta = SaldoCta + Row("Importe")
            End If
            If Row("Tipo") = 65 Then           'Devolucion Sena.
                SaldoCta = SaldoCta - Row("Importe") + Row("Importe")    '(doble imputacion.)
            End If
        Next

        Dt.Dispose()

        Return SaldoCta

    End Function
    Public Function EsMovimientoBueno(ByVal TipoNota As Integer, ByVal MedioPago As Integer, ByVal Tipo As Integer, ByVal TipoPase As Integer) As Boolean

        If TipoNota = 80 Then Return True
        If TipoNota = 90 Then Return True
        If TipoNota = 91 Then Return True
        If TipoNota = 7001 Or TipoNota = 7002 Then Return True

        If TipoNota = 6000 And TipoPase = 0 Then
            If TipoPase = 0 Then
                Select Case Tipo
                    Case 3
                        Return True
                    Case Else
                        Return False
                End Select
            End If
        End If

        Select Case Tipo
            Case 1, 6, 3, 11, 10
                Return True
            Case Else
                Return False
        End Select

    End Function
    Public Function ReGrabaUltimoNumero(ByVal Banco As Integer, ByVal Cuenta As Double, ByVal Numero As Integer, ByVal ConexionStr As String) As Integer

        Dim Sql As String = "UPDATE " & "CuentasBancarias" &
            " Set UltimoNumero = " & Numero &
            " WHERE Banco = " & Banco & " AND Cuenta = " & Cuenta & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                Return Resul
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de datos al Ingresar a Cuentas Bancarias. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function ActualizaRecibo(ByVal DtFormasPago As DataTable, ByVal Funcion As String, ByVal DtGrid As DataTable, ByVal DtNotaCabezaAux As DataTable, ByVal DtMedioPagoAux As DataTable, ByVal DtNotaDetalleAux As DataTable, ByVal ConexionStr As String, ByVal EsTrEspecial As Boolean) As Double

        Dim Resul As Double
        Dim Row1 As DataRow
        Dim LetraIva As Integer = Strings.Left(DtNotaCabezaAux.Rows(0).Item("Nota").ToString, 1)
        Dim Pabierto As Boolean
        Dim PtipoNota As Integer = DtNotaCabezaAux.Rows(0).Item("TipoNota")
        Dim Emisor As Integer = DtNotaCabezaAux.Rows(0).Item("Emisor")
        Dim Tr As Boolean = DtNotaCabezaAux.Rows(0).Item("Tr")
        Dim RowsBusqueda() As DataRow
        Dim Tipo As Integer

        If ConexionStr = Conexion Then
            Pabierto = True
        Else : Pabierto = False
        End If

        If (PtipoNota = 5 Or PtipoNota = 6) And Pabierto And Funcion = "A" Then
            If Not ReGrabaUltimaNumeracionNDebito(DtNotaCabezaAux.Rows(0).Item("Nota"), LetraIva) Then Return -10
        End If
        If (PtipoNota = 7 Or PtipoNota = 8) And Pabierto And Funcion = "A" Then
            If Not ReGrabaUltimaNumeracionNCredito(DtNotaCabezaAux.Rows(0).Item("Nota"), LetraIva) Then Return -10
        End If
        '
        If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
            Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "RecibosCabeza", ConexionStr)
            If Resul <= 0 Then Return Resul
        End If
        If Not IsNothing(DtNotaDetalleAux.GetChanges) Then
            Resul = GrabaTabla(DtNotaDetalleAux.GetChanges, "RecibosDetalle", ConexionStr)
            If Resul <= 0 Then Return Resul
        End If
        '
        If Funcion = "A" Then
            For Each Row As DataRow In DtGrid.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                    Tipo = RowsBusqueda(0).Item("Tipo")
                    Row1 = DtMedioPagoAux.NewRow
                    Row1("TipoNota") = PtipoNota
                    Row1("Nota") = DtNotaCabezaAux.Rows(0).Item("Nota")
                    Row1("MedioPago") = Row("MedioPago")
                    Row1("Detalle") = Row("Detalle")
                    Row1("Neto") = Row("Neto")
                    Row1("Alicuota") = Row("Alicuota")
                    Row1("Cambio") = Row("Cambio")
                    Row1("Importe") = Row("Importe")
                    Row1("Bultos") = Row("Bultos")
                    Row1("Banco") = Row("Banco")
                    Row1("Cuenta") = Row("Cuenta")
                    Row1("Comprobante") = Row("Comprobante")
                    Row1("FechaComprobante") = Row("FechaComprobante")
                    Row1("ClaveInterna") = Row("ClaveInterna")
                    Row1("ClaveCheque") = Row("ClaveCheque")
                    If Row("MedioPago") = 2 Or (Row("MedioPago") = 3 And (PtipoNota = 60 Or PtipoNota = 65 Or PtipoNota = 604)) Then
                        Row1("ClaveCheque") = ActualizaClavesComprobantes("A", Row("ClaveCheque"), PtipoNota, Row("MedioPago"), True, Row("Banco"), Row("Cuenta"), Row("Serie"), Row("Numero"), Row("Fecha"), Row("Importe"), Row("EmisorCheque"), DtNotaCabezaAux.Rows(0).Item("Nota"), "01/01/1800", ConexionStr, Tr, False, Row("eCheq"))
                        If Row1("ClaveCheque") <= 0 Then Return Row1("ClaveCheque")
                    End If
                    If Row("MedioPago") = 3 And (PtipoNota = 600 Or PtipoNota = 64) Then
                        Dim Resul2 As Integer = 0
                        Resul2 = ActualizaClavesComprobantes("A", Row("ClaveCheque"), PtipoNota, Row("MedioPago"), True, Row("Banco"), Row("Cuenta"), Row("Serie"), Row("Numero"), Row("Fecha"), Row("Importe"), Row("EmisorCheque"), DtNotaCabezaAux.Rows(0).Item("Nota"), "01/01/1800", ConexionStr, Tr, EsTrEspecial, Row("eCheq"))
                        If Resul2 <= 0 Then Return Resul2
                    End If
                    If Row("MedioPago") = 14 Then
                        Row1("ClaveCheque") = ActualizaClavesDebito("A", Row("ClaveCheque"), PtipoNota, Row("MedioPago"), Row("Banco"), Row("Cuenta"), Row("Comprobante"), Row("FechaComprobante"), Row("Importe"), DtNotaCabezaAux.Rows(0).Item("Nota"), "01/01/1800", ConexionStr)
                        If Row1("ClaveCheque") <= 0 Then Return Row1("ClaveCheque")
                    End If
                    If Row("MedioPago") = 6 And PtipoNota = 600 Then
                        Dim Resul2 As Integer = 0
                        Resul2 = ActualizaValeTerceros("A", Row("ClaveInterna"), PtipoNota, ConexionStr)
                        If Resul2 <= 0 Then Return Resul2
                    End If
                    Dim Clave As String = ""
                    ' If Tipo = 4 And PtipoNota = 600 Then Clave = Row("MedioPago") & Row("Comprobante") & Emisor 'retenciones.
                    If Tipo = 10 And PtipoNota = 60 Then Clave = Row("MedioPago") & Row("Comprobante") & Emisor 'Seña Vale a cliente.
                    If Clave <> "" Then
                        If Not GrabaControl(Clave, 0) Then
                            MsgBox("Comprobante " & Row("Comprobante") & " Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            Return -3
                        End If
                    End If
                    DtMedioPagoAux.Rows.Add(Row1)
                End If
            Next
        End If
        '
        If Funcion = "B" Then
            Dim Resul2 As Integer = 0
            For Each Row As DataRow In DtGrid.Rows
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row("MedioPago"))
                Tipo = RowsBusqueda(0).Item("Tipo")
                If Row("MedioPago") = 2 Or Row("MedioPago") = 3 Then
                    Resul2 = ActualizaClavesComprobantes("B", Row("ClaveCheque"), PtipoNota, Row("MedioPago"), True, Row("Banco"), Row("Cuenta"), Row("Serie"), Row("Numero"), Row("Fecha"), Row("Importe"), Row("EmisorCheque"), DtNotaCabezaAux.Rows(0).Item("Nota"), "01/01/1800", ConexionStr, Tr, EsTrEspecial, False)
                    If Resul2 <= 0 Then Return Resul2
                End If
                If Row("MedioPago") = 14 Then
                    Resul2 = ActualizaClavesDebito("B", Row("ClaveCheque"), PtipoNota, Row("MedioPago"), Row("Banco"), Row("Cuenta"), Row("Comprobante"), Row("FechaComprobante"), Row("Importe"), DtNotaCabezaAux.Rows(0).Item("Nota"), "01/01/1800", ConexionStr)
                    If Resul2 <= 0 Then Return Resul2
                End If
                If Row("MedioPago") = 6 Then
                    Resul2 = ActualizaValeTerceros("B", Row("ClaveInterna"), PtipoNota, ConexionStr)
                    If Resul2 <= 0 Then Return Resul2
                End If
                Dim Clave As String = ""
                If Tipo = 4 And PtipoNota = 600 Then Clave = Row("MedioPago") & Row("Comprobante") & Emisor 'retenciones.
                If Tipo = 10 And PtipoNota = 60 Then Clave = Row("MedioPago") & Row("Comprobante") & Emisor 'Seña Vale a cliente.
                If Clave <> "" Then BorraControl(Clave)
            Next
        End If
        '        
        If Not IsNothing(DtMedioPagoAux.GetChanges) Then
            Resul = GrabaTabla(DtMedioPagoAux.GetChanges, "RecibosDetallePago", ConexionStr)
            If Resul <= 0 Then Return Resul
        End If
        '
        Return DtNotaCabezaAux.Rows(0).Item("Nota")

    End Function
    Public Function ActualizaClavesComprobantes(ByVal Funcion As String, ByVal ClaveCheque As Integer, ByVal TipoNota As Integer,
             ByVal MedioPago As Integer, ByVal Operacion As Integer, ByVal Banco As Integer, ByVal Cuenta As Double, ByVal Serie As String,
             ByVal Numero As Integer, ByVal Fecha As DateTime, ByVal Importe As Double, ByVal EmisorCheque As String, ByVal Comprobante As Double,
             ByVal FechaDeposito As DateTime, ByVal ConexionStr As String, ByVal Trucho As Boolean, ByVal EsTrEspecial As Boolean, ByVal eCheq As Boolean) As Integer

        Dim Clave As Integer = 0
        Dim ConexionCheque As String = ""

        If MedioPago = 3 Then
            Select Case TipoNota
                Case 60, 1000, 1010, 1015, 600, 91, 4010, 65, 6000, 6001, 7001, 7002, 64, 604, 5010, 5020
                Case Else
                    MsgBox("Tipo Nota " & TipoNota & " No prevista en actualizacion cheque terceros.")
                    Return 0
            End Select
        End If

        If Funcion = "A" Then
            If MedioPago = 2 And Not Trucho Then   'Cheque propio No trucho.  
                Clave = GrabaCheque(TipoNota, MedioPago, Banco, Serie, Cuenta, Numero, Fecha, Importe, "", TipoNota, Comprobante, FechaDeposito, ConexionStr, eCheq)
                If Clave = -80 Then Return -1
                If Clave <= 0 Then
                    MsgBox("Cheque " & Numero & " ya fue emitido.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                If Not eCheq Then
                    If ReGrabaUltimoCheque(Banco, Cuenta, Numero, Conexion) < 0 Then
                        MsgBox("Cheque " & Numero & " Error al Grabar Ultimo Numero Cheque.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                        Return -3
                    End If
                End If
                Return Clave
            End If
            If MedioPago = 2 And Trucho Then   'Cheque propio trucho.  
                If ReGrabaChequePropioTrucho(ClaveCheque, MedioPago, Comprobante) <= 0 Then
                    MsgBox("Cheque " & Numero & " Error de Base de datos o esta Afectado.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return ClaveCheque
            End If
            If MedioPago = 3 And ((TipoNota = 60 And Not Trucho) Or TipoNota = 1000 Or TipoNota = 1015 Or TipoNota = 6001 Or TipoNota = 7002 Or TipoNota = 604 Or TipoNota = 5020) Then   'cheque terceros y cobranza o prestamo o devolucion seña o Ajuste disminuye Fondo Fijo,Devolución del proveedores,Devolucion Otros proveedores.
                Clave = GrabaCheque(TipoNota, MedioPago, Banco, Serie, Cuenta, Numero, Fecha, Importe, EmisorCheque, TipoNota, Comprobante, FechaDeposito, ConexionStr, eCheq)
                If Clave = -80 Then Return -1
                If Clave <= 0 Then
                    MsgBox("Cheque " & Numero & " ya Existe.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return Clave
            End If
            If MedioPago = 3 And (TipoNota = 600 Or TipoNota = 64 Or TipoNota = 5010) And Not Trucho Then  'cheque tercero y orden de pago.
                If ReGrabaComprobanteChequeTerceros(ClaveCheque, MedioPago, TipoNota, Comprobante, ConexionStr) <= 0 Then
                    MsgBox("Cheque " & Numero & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return ClaveCheque
            End If
            If Not EsTrEspecial Then
                If MedioPago = 3 And (TipoNota = 600 Or TipoNota = 60) And Trucho Then  'cheque tercero y orden de pago.
                    If ReGrabaChequePropioTrucho(ClaveCheque, MedioPago, Comprobante) <= 0 Then
                        MsgBox("Cheque " & Numero & " Error de Base de datos o esta Afectado.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                        Return -3
                    End If
                    Return ClaveCheque
                End If
            End If
            If EsTrEspecial Then    'Marca el cheque 3ros. como afectado y entregado. 
                If MedioPago = 3 And (TipoNota = 600 Or TipoNota = 60) And Trucho Then  'cheque tercero y orden de pago.
                    If ReGrabaChequePropioTruchoEspecial(TipoNota, ClaveCheque, MedioPago, Comprobante) <= 0 Then
                        MsgBox("Cheque " & Numero & " Error de Base de datos o esta Afectado o esta Entregado.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                        Return -3
                    End If
                    Return ClaveCheque
                End If
            End If
            If MedioPago = 3 And (TipoNota = 6000 Or TipoNota = 7001) Then  'cheque tercero y compra de divisa,alta fondo fijo, Ajuste aumento Fondo Fijo .
                If ReGrabaComprobanteChequeTerceros(ClaveCheque, MedioPago, TipoNota, Comprobante, ConexionStr) <= 0 Then
                    MsgBox("Cheque " & Numero & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return ClaveCheque
            End If
            If MedioPago = 3 And TipoNota = 91 Then   'cheque tercero y movimiento deposito bancario.
                If ReGrabaComprobanteChequeTerceros(ClaveCheque, MedioPago, TipoNota, Comprobante, ConexionStr) <= 0 Then
                    MsgBox("Cheque " & Numero & " Fue Modificado por Otro Usuario.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return ClaveCheque
            End If
            If MedioPago = 3 And (TipoNota = 1010 Or TipoNota = 4010 Or TipoNota = 65) Then   'cheque tercero y Cancelacion Prestamo y pago sueldo y devolucion Seña.
                If ReGrabaComprobanteChequeTerceros(ClaveCheque, MedioPago, TipoNota, Comprobante, ConexionStr) <= 0 Then
                    MsgBox("Cheque " & Numero & " Fue Modificado por Otro Usuario.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return ClaveCheque
            End If
        End If
        '
        If Funcion = "B" Then
            If MedioPago = 2 And Not Trucho Then
                If AnulaCheque(ClaveCheque, MedioPago, ConexionStr) <= 0 Then
                    MsgBox("Cheque " & Numero & " Otro Usuario Modifico Datos,No esta en Cartera o Fue Afectado a una Orden de Pago.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return ClaveCheque
            End If
            If MedioPago = 2 And Trucho Then
                If AnulaChequeTrucho(ClaveCheque, MedioPago) <= 0 Then
                    MsgBox("Cheque " & Numero & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return ClaveCheque
            End If
            If MedioPago = 3 And ((TipoNota = 60 And Not Trucho) Or TipoNota = 1000 Or TipoNota = 1015 Or TipoNota = 6001 Or TipoNota = 7002 Or TipoNota = 604 Or TipoNota = 5020) Then
                If AnulaCheque(ClaveCheque, MedioPago, ConexionStr) <= 0 Then
                    MsgBox("Cheque " & Numero & " Otro Usuario Modifico Datos,No esta en Cartera o Fue Afectado a una Orden de Pago.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return ClaveCheque
            End If
            If MedioPago = 3 And (TipoNota = 600 Or TipoNota = 64 Or TipoNota = 5010) And Not Trucho Then
                If ReGrabaCerosACheque(ClaveCheque, MedioPago, ConexionStr) <= 0 Then
                    MsgBox("Cheque " & Numero & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return ClaveCheque
            End If
            If Not EsTrEspecial Then
                If MedioPago = 3 And (TipoNota = 600 Or TipoNota = 60) And Trucho Then
                    If AnulaChequeTrucho(ClaveCheque, MedioPago) <= 0 Then
                        MsgBox("Cheque " & Numero & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                        Return -3
                    End If
                    Return ClaveCheque
                End If
            End If
            If EsTrEspecial Then         'libera el cheque 3ros. como afectado y entregado.
                If MedioPago = 3 And (TipoNota = 600 Or TipoNota = 60) And Trucho Then
                    If AnulaChequeTruchoEspecial(ClaveCheque, MedioPago) <= 0 Then
                        MsgBox("Cheque " & Numero & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                        Return -3
                    End If
                    Return ClaveCheque
                End If
            End If
            If MedioPago = 3 And (TipoNota = 6000 Or TipoNota = 7001) Then
                If ReGrabaCerosACheque(ClaveCheque, MedioPago, ConexionStr) <= 0 Then
                    MsgBox("Cheque " & Numero & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return ClaveCheque
            End If
            If MedioPago = 3 And TipoNota = 91 Then
                If ReGrabaCerosACheque(ClaveCheque, MedioPago, ConexionStr) <= 0 Then
                    MsgBox("Cheque " & Numero & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return ClaveCheque
            End If
            If MedioPago = 3 And (TipoNota = 1010 Or TipoNota = 4010 Or TipoNota = 65) Then
                If ReGrabaCerosACheque(ClaveCheque, MedioPago, ConexionStr) <= 0 Then
                    MsgBox("Cheque " & Numero & " Otro Usuario Modifico Datos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return ClaveCheque
            End If
        End If

    End Function
    Public Function ActualizaClavesDebito(ByVal Funcion As String, ByVal ClaveCheque As Integer, ByVal TipoNota As Integer, ByVal MedioPago As Integer, ByVal Banco As Integer, ByVal Cuenta As Double, ByVal Numero As Integer, ByVal Fecha As DateTime, ByVal Importe As Double, ByVal Comprobante As Double, ByVal FechaDeposito As DateTime, ByVal ConexionStr As String) As Integer

        Dim Clave As Integer = 0

        If Funcion = "A" Then
            If MedioPago = 14 Then   'Debito Automatico Diferido.    'eCheq poner false a lo ultimo. 
                Clave = GrabaCheque(TipoNota, MedioPago, Banco, "", Cuenta, Numero, Fecha, Importe, "", TipoNota, Comprobante, FechaDeposito, ConexionStr, False)
                If Clave = -80 Then Return -1
                If Clave <= 0 Then
                    MsgBox("Debito Autom.Dife. " & Numero & " ya fue emitido.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return -3
                End If
                Return Clave
            End If
        End If
        '
        If Funcion = "B" Then
            If AnulaCheque(ClaveCheque, MedioPago, ConexionStr) <= 0 Then
                MsgBox("Cheque " & Numero & " Otro Usuario Modifico Datos,No esta en Cartera.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Return -3
            End If
            Return ClaveCheque
        End If

    End Function
    Public Function ActualizaValeTerceros(ByVal Funcion As String, ByVal ClaveInterna As Integer, ByVal TipoNota As Integer, ByVal ConexionStr As String) As Integer

        If Funcion = "A" Then
            Dim Sql As String = "UPDATE " & "RecuperoSenia" &
                " Set Usado = 1" &
                " WHERE Nota = " & ClaveInterna & " AND Usado = 0;"

            Try
                Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                    Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Miconexion.Open()
                    Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                    Miconexion.Close()
                    Return Resul
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de datos al Ingresar a Tabla RecuperoSenia. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End If

        If Funcion = "B" Then
            Dim Sql As String = "UPDATE " & "RecuperoSenia" &
                " Set Usado = 0" &
                " WHERE Nota = " & ClaveInterna & ";"

            Try
                Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                    Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Miconexion.Open()
                    Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                    Miconexion.Close()
                    Return Resul
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de datos al Ingresar a Tabla RecuperoSenia. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End If

    End Function
    Private Function LeeUltimaClaveCheque(ByVal Mediopago As Integer, ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Dim Cmd As New OleDb.OleDbCommand("SELECT MAX(ClaveCheque) FROM Cheques WHERE MedioPago = " & Mediopago & ";", Miconexion)
                Miconexion.Open()
                Dim Ultimo = Cmd.ExecuteScalar()
                If Not IsDBNull(Ultimo) Then
                    Return CInt(Ultimo)
                Else : Return 0
                End If
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Function GrabaCheque(ByVal TipoNota As Integer, ByVal Mediopago As Integer, ByVal Banco As Integer, ByVal Serie As String, ByVal Cuenta As Double, ByVal Numero As Integer, ByVal Fecha As DateTime,
                                ByVal Importe As Double, ByVal EmisorCheque As String, ByVal TipoPago As Integer, ByVal Comprobante As Double, ByVal FechaDeposito As DateTime, ByVal ConexionStr As String, ByVal eCheq As Boolean) As Integer

        ' http://www.elguille.info/net/adonet/sql_instrucciones_insert.aspx

        Dim Val As String = ""

        Dim ClaveCheque As Integer = LeeUltimaClaveCheque(Mediopago, ConexionStr)
        If ClaveCheque < 0 Then Return 0
        Dim Resul As Integer

        CambiarPuntoDecimal(".")

        ClaveCheque = ClaveCheque + 1

        Dim FechaDep As String
        If Format(FechaDeposito, "dd/MM/yyyy") <> "01/01/1800" Then
            FechaDep = Format(FechaDeposito, "yyyyMMdd")
        Else : FechaDep = "18000101"
        End If

        Dim TipoOrigen As Integer
        Dim TipoDestino As Integer
        Dim CompOrigen As Double
        Dim CompDestino As Double

        If Mediopago = 2 Or Mediopago = 14 Then TipoOrigen = 0 : CompOrigen = 0 : TipoDestino = TipoPago : CompDestino = Comprobante
        If Mediopago = 3 Then TipoOrigen = TipoPago : CompOrigen = Comprobante : TipoDestino = 0 : CompDestino = 0


        Val = ClaveCheque & "," & Mediopago & "," & Banco & "," & Cuenta & ",'" & Serie & "'," & Numero & "," &
              Importe & ",'" & EmisorCheque & "','" & Format(Fecha, "yyyyMMdd") & "'," & GCaja & "," & TipoOrigen & "," & CompOrigen & "," & TipoDestino & "," & CompDestino & ",'" & FechaDep & "'," & 1 & "," & 0 & "," & 0 & "," & 0 & "," & Int(eCheq)

        Dim Sql As String = "INSERT INTO " & "Cheques" &
           "(ClaveCheque,MedioPago,Banco,Cuenta,Serie,Numero,Importe,EmisorCheque,Fecha,Caja,TipoOrigen,CompOrigen,TipoDestino,CompDestino,FechaDeposito,Estado,Afectado,Ord,TieneDebito,eCheq) " &
           "VALUES " &
            "(" & Val & ")"

        CambiarPuntoDecimal(",")

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Resul = CInt(Cmd.ExecuteNonQuery())
                '        Dim Cmd2 As New OleDb.OleDbCommand("SELECT @@Identity", Miconexion)
                '        ClaveCheque = Cmd2.ExecuteScalar
                '        Miconexion.Close()
                '        If ClaveCheque <= 0 Then Return 0
                If Resul <= 0 Then Return 0
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de datos al Ingresar Cheque. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3) : Return 0
        End Try

        If Mediopago <> 2 Then Return ClaveCheque

        'Graba archivo control de cheques para cheques propios.
        Val = "'" & Mediopago & Banco & Format(Cuenta, "0000000000") & Serie & Format(Numero, "00000000") & "'," & ClaveCheque

        Sql = "INSERT INTO " & "ControlCheques" &
           "(Clave,ClaveCheque) " &
           "VALUES " &
            "(" & Val & ")"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Resul = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                If Resul <= 0 Then
                    Return Resul
                Else
                    Return ClaveCheque
                End If
            End Using
        Catch ex As OleDb.OleDbException
            If ex.ErrorCode = GAltaExiste Then
                Return 0
            Else
                MsgBox("Error, Base de datos al Ingresar Cheque o Comprobante. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End If
        End Try

    End Function
    Public Function ReGrabaCerosACheque(ByVal Clave As Integer, ByVal MedioPago As Integer, ByVal ConexionStr As String) As Integer

        Dim Sql As String = "UPDATE " & "Cheques" &
            " Set TipoDestino = 0,CompDestino = 0" &
            " WHERE ClaveCheque = " & Clave & " AND MedioPago = " & MedioPago & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                Return Resul
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de datos al Ingresar a Cheque. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function ReGrabaComprobanteChequeTerceros(ByVal Clave As Integer, ByVal MedioPago As Integer, ByVal TipoNota As Integer, ByVal Comprobante As Double, ByVal ConexionStr As String) As Integer

        Dim Sql As String = "UPDATE " & "Cheques" &
            " Set TipoDestino = " & TipoNota & ",CompDestino = " & Comprobante &
            " WHERE ClaveCheque = " & Clave & " AND MedioPago = " & MedioPago & " AND CompDestino = 0;"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                Return Resul
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de datos al Ingresar a Cheque. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function AnulaCheque(ByVal Clave As Integer, ByVal MedioPago As Integer, ByVal ConexionStr As String) As Integer

        Dim Sql As String

        If MedioPago = 2 Then Sql = "UPDATE Cheques Set Estado = 3 WHERE Afectado = 0 AND ClaveCheque = " & Clave & " AND MedioPago = " & MedioPago & ";"
        If MedioPago = 3 Then Sql = "UPDATE Cheques Set Estado = 3 WHERE Afectado = 0 AND CompDestino = 0 AND ClaveCheque = " & Clave & " AND MedioPago = " & MedioPago & ";"
        If MedioPago = 14 Then Sql = "UPDATE Cheques Set Estado = 3 WHERE ClaveCheque = " & Clave & " AND MedioPago = " & MedioPago & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                Return Resul
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de datos al Ingresar a Cheque. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function RestauraChequePropio(ByVal Clave As Integer, ByVal ConexionStr As String) As Integer

        Dim Sql As String

        Sql = "UPDATE Cheques Set Estado = 1 WHERE ClaveCheque = " & Clave & " AND MedioPago = 2;"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                Return Resul
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de datos al Restaurar Cheque Propio. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return -2
        End Try

    End Function
    Public Function AnulaChequeTrucho(ByVal Clave As Integer, ByVal MedioPago As Integer) As Integer

        Dim Sql As String = "UPDATE Cheques Set Afectado = 0 WHERE ClaveCheque = " & Clave & " AND MedioPago = " & MedioPago & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                Return Resul
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de datos al Ingresar a Cheque. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function AnulaChequeTruchoEspecial(ByVal Clave As Integer, ByVal MedioPago As Integer) As Integer

        Dim Sql As String = "UPDATE " & "Cheques" &
            " Set TipoDestino = " & 0 & ",CompDestino = " & 0 & ",Afectado = " & 0 &
            " WHERE ClaveCheque = " & Clave & " AND MedioPago = " & MedioPago & " AND CompDestino <> 0 AND Afectado <> 0;"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                Return Resul
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de datos al Ingresar a Cheque. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function ReGrabaChequePropioTrucho(ByVal Clave As Integer, ByVal MedioPago As Integer, ByVal Comprobante As Double) As Integer

        Dim Sql As String = "UPDATE " & "Cheques" &
            " Set Afectado = " & Comprobante &
            " WHERE MedioPago = " & MedioPago & " AND ClaveCheque = " & Clave & " AND Afectado = 0;"

        Dim ConexionStr As String
        ConexionStr = ConexionN

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                Return Resul
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de datos al Ingresar a Cheque. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function ReGrabaChequePropioTruchoEspecial(ByVal TipoNota As Integer, ByVal Clave As Integer, ByVal MedioPago As Integer, ByVal Comprobante As Double) As Integer


        Dim Sql As String = "UPDATE " & "Cheques" &
            " Set TipoDestino = " & TipoNota & ",CompDestino = " & Comprobante & ",Afectado = " & Comprobante &
            " WHERE ClaveCheque = " & Clave & " AND MedioPago = " & MedioPago & " AND CompDestino = 0 AND Afectado = 0;"

        Dim ConexionStr As String
        ConexionStr = Conexion

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                Return Resul
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de datos al Ingresar a Cheque. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function ReGrabaUltimoCheque(ByVal Banco As Integer, ByVal Cuenta As Double, ByVal Numero As Integer, ByVal ConexionStr As String) As Integer

        Dim Sql As String = "UPDATE " & "CuentasBancarias" &
            " Set UltimoNumero = " & Numero &
            " WHERE Banco = " & Banco & " AND Numero = " & Cuenta & " AND UltimoNumero < " & Numero & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                Return Resul
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos al Regrabar Ultimo Numero Cheque. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function GrabaControl(ByVal Clave As String, ByVal Recibo As Double) As Boolean

        'Graba archivo control de cheques.
        Dim Val As String = "'" & Clave & "'," & Recibo

        Dim Sql As String = "INSERT INTO " & "ControlCheques" &
           "(Clave,ClaveCheque) " &
           "VALUES " &
            "(" & Val & ")"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
                If Resul = 1 Then Return True
            End Using
        Catch ex As OleDb.OleDbException
            If ex.ErrorCode = GAltaExiste Then
                Return False
            Else
                MsgBox("Error, Base de datos al Ingresar Cheque o Comprobante. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End If
        End Try

    End Function
    Public Sub BorraControl(ByVal Clave As String)

        'Borra Archivo Control de cheques.
        Dim Sql As String = "DELETE " & "ControlCheques" &
              " WHERE Clave = '" & Clave & "';"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                Miconexion.Close()
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos al Regrabar Ultimo Numero Cheque. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Sub
    Public Function NombreLegajo(ByVal Legajo As Integer, ByVal ConexionStr As String) As String

        Dim Dt As New DataTable

        Try
            If Not Tablas.Read("SELECT Apellidos,Nombres FROM Empleados WHERE Legajo = " & Legajo & ";", ConexionStr, Dt) Then Return ""
            If Dt.Rows.Count <> 0 Then
                Return Dt.Rows(0).Item("Apellidos") & " " & Dt.Rows(0).Item("Nombres")
            Else : Return ""
            End If
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function HallaImporteRepetido(ByVal TipoNota1 As Integer, ByVal Nota1 As Double, ByVal TipoNota2 As Integer, ByVal Nota2 As Double, ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Importe FROM RecibosDetalle WHERE TipoNota = " & TipoNota1 & " AND Nota = " & Nota1 & " AND TipoComprobante = " & TipoNota2 & " AND Comprobante = " & Nota2 & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
        Finally
        End Try

    End Function
    Public Function HallaAsignacion(ByVal TipoComprobante As Integer, ByVal Comprobante As Double, ByVal Emisor As Integer, ByVal ConexionStr As String) As Double

        Dim Sql As String = ""

        Sql = "SELECT SUM(C.Importe) FROM RecibosCabeza AS C INNER JOIN RecibosDetalle AS D ON C.TipoNota = D.TipoNota AND C.Nota = D.Nota " &
              "WHERE C.Estado <> 3 AND D.TipoComprobante = " & TipoComprobante & " AND D.Comprobante = " & Comprobante & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Resul = Cmd.ExecuteScalar()
                    If Not IsDBNull(Resul) Then
                        Return CDbl(Resul)
                    Else : Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos al leer asignado." & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        Finally
        End Try

    End Function
    Public Function HallaTipofacturaProveedor(ByVal Factura As Double, ByVal Abierto As Boolean) As Integer

        Dim Dt As New DataTable

        If Abierto Then
            If Not Tablas.Read("SELECT EsInsumos,EsReventa,EsSinComprobante,EsAfectaCostoLotes FROM FacturasProveedorCabeza WHERE Factura = " & Factura & ";", Conexion, Dt) Then End
        Else
            If Not Tablas.Read("SELECT EsInsumos,EsReventa,EsSinComprobante,EsAfectaCostoLotes FROM FacturasProveedorCabeza WHERE Factura = " & Factura & ";", ConexionN, Dt) Then End
        End If
        If Dt.Rows(0).Item("EsReventa") Then Dt.Dispose() : Return 1
        If Dt.Rows(0).Item("EsInsumos") Then Dt.Dispose() : Return 2
        If Dt.Rows(0).Item("EsSinComprobante") Then Dt.Dispose() : Return 3
        If Dt.Rows(0).Item("EsAfectaCostoLotes") Then Dt.Dispose() : Return 4

    End Function
    Public Function ChequeEntregado(ByVal MedioPago As Integer, ByVal ConexionStr As String, ByVal ClaveCheque As Integer) As Double

        If ClaveCheque = 0 Then Return 0
        If MedioPago = 2 Then Return 1000

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT CompDestino FROM Cheques WHERE Estado <> 3 AND ClaveCheque = " & ClaveCheque & " AND MedioPago = " & MedioPago & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function ChequeRechazado(ByVal MedioPago As Integer, ByVal ConexionStr As String, ByVal ClaveCheque As Integer) As Double

        If ClaveCheque = 0 Then Return 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Numero FROM Cheques WHERE Estado = 4 AND ClaveCheque = " & ClaveCheque & " AND MedioPago = " & MedioPago & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function ChequeDepositado(ByVal MedioPago As Integer, ByVal ConexionStr As String, ByVal ClaveCheque As Integer) As Double

        If MedioPago <> 2 Then Return 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT FechaDeposito FROM Cheques WHERE Estado = 1 AND ClaveCheque = " & ClaveCheque & " AND MedioPago = " & MedioPago & ";", Miconexion)
                    MsgBox(Cmd.ExecuteScalar())
                    If Cmd.ExecuteScalar() = "01/01/1800" Then
                        Return 0
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function EstadoCheque(ByVal MedioPago As Integer, ByVal ClaveCheque As Integer, ByVal ConexionStr As String, ByRef Anulado As Boolean, ByRef Rechazado As Boolean, ByRef Depositado As Boolean, ByRef Entregado As Boolean, ByRef Afectado As Boolean) As Boolean

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Estado,FechaDeposito,Afectado,CompDestino FROM Cheques WHERE MedioPago = " & MedioPago & " AND ClaveCheque = " & ClaveCheque & ";", ConexionStr, Dt) Then Return False
        If Dt.Rows.Count <> 0 Then
            If Dt.Rows(0).Item("Estado") = 3 Then Anulado = True
            If Dt.Rows(0).Item("Estado") = 4 Then Rechazado = True
            If Dt.Rows(0).Item("FechaDeposito") <> "01/01/1800" Then Depositado = True
            If Dt.Rows(0).Item("CompDestino") <> 0 And Dt.Rows(0).Item("Estado") = 1 Then Entregado = True
            If Dt.Rows(0).Item("Afectado") <> 0 And Dt.Rows(0).Item("Estado") = 1 Then Afectado = True
        End If

        Return True

        Dt.Dispose()

    End Function
    Public Function ChequeReemplazado(ByVal MedioPago As Integer, ByVal ClaveCheque As Integer, ByVal TipoNota As Integer, ByVal Nota As Double, ByVal ConexionStr As String) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ClaveCheque FROM Cheques WHERE MedioPago = " & MedioPago & " AND ClaveCheque = " & ClaveCheque & " AND TipoDestino = " & TipoNota & " AND CompDestino = " & Nota & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Cheques.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    '----------------------------------------------------------------------------------------------------------------- 
    '------------------------------------- Analisis de Un Prestamo --------------------------------------------------- 
    '----------------------------------------------------------------------------------------------------------------- 
    Public Function ProcesaPrestamo(ByVal GeneraArchivo As Boolean, ByRef CapitalAjustado As Double, ByRef ImporteCancelado As Double, ByRef InteresCancelado As Double, ByRef Gastos As Double, ByRef DtMovimientos As DataTable, ByVal Prestamo As Double, ByVal Capital As Double, ByVal Fecha As DateTime, ByVal Estado As Integer, ByVal ConexionStr As String) As Boolean

        Dim Dt As New DataTable
        Dim Row1 As DataRow

        ImporteCancelado = 0
        Gastos = 0
        CapitalAjustado = Capital

        If GeneraArchivo Then DtMovimientos.Clear()

        Dim Sql As String = "SELECT C.TipoNota,C.Fecha,C.Estado,C.Movimiento,C.OrigenRechazo,C.Importe AS ImporteCabeza,C.MedioPagoRechazado,C.ChequeRechazado,D.Concepto,D.Importe,C.ClaveChequeReemplazado FROM PrestamosMovimientoCabeza AS C LEFT JOIN PrestamosMovimientoDetalle AS D ON C.Movimiento = D.Movimiento WHERE C.Prestamo = " & Prestamo & ";"
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then Return False
        For Each Row As DataRow In Dt.Rows
            If Row("Estado") = 1 Then
                If Not IsDBNull(Row("Concepto")) Then
                    If Row("Concepto") = 6 Then ImporteCancelado = ImporteCancelado + Row("Importe")
                    If Row("Concepto") = 2 Then CapitalAjustado = CapitalAjustado + Row("Importe")
                    If Row("Concepto") = 7 Then InteresCancelado = InteresCancelado + Row("Importe")
                    If Row("Concepto") > 499 Then Gastos = Gastos + Row("Importe")
                End If
                If Row("ChequeRechazado") <> 0 Then
                    If Row("OrigenRechazo") = 1010 Then ImporteCancelado = ImporteCancelado - Row("ImporteCabeza")
                    If Row("OrigenRechazo") = 1000 Or Row("OrigenRechazo") = 1015 Then CapitalAjustado = CapitalAjustado - Row("ImporteCabeza")
                End If
            End If
            If GeneraArchivo Then
                Row1 = DtMovimientos.NewRow
                Row1("Movimiento") = Row("Movimiento")
                Row1("TipoNota") = Row("TipoNota")
                Row1("Concepto") = Row("Concepto")
                If Row("ChequeRechazado") <> 0 Then
                    Row1("Concepto") = 93
                End If
                If Row("ClaveChequeReemplazado") <> 0 Then
                    Row1("Concepto") = 94
                    Row1("Credito") = 0
                    Row1("Debito") = 0
                End If
                If Row("TipoNota") = 1010 And Row("ClaveChequeReemplazado") = 0 Then
                    Row1("Credito") = 0
                    Row1("Debito") = Row("Importe")
                End If
                If Row("TipoNota") = 1015 Then
                    Row1("Credito") = Row("Importe")
                    Row1("Debito") = 0
                End If
                If Row("TipoNota") = 1005 Then
                    If Row("ChequeRechazado") <> 0 Then
                        Row1("Credito") = 0
                        Row1("Debito") = Row("ImporteCabeza")
                    Else
                        Row1("Credito") = 0
                        Row1("Debito") = Row("Importe")
                    End If
                End If
                If Row("TipoNota") = 1007 Then
                    If Row("ChequeRechazado") <> 0 Then
                        Row1("Credito") = Row("ImporteCabeza")
                        Row1("Debito") = 0
                    Else
                        Row1("Credito") = Row("Importe")
                        Row1("Debito") = 0
                    End If
                End If
                Row1("Fecha") = Row("Fecha")
                Row1("Estado") = Row("Estado")
                Row1("MedioPagoRechazado") = Row("MedioPagoRechazado")
                Row1("ChequeRechazado") = Row("ChequeRechazado")
                Row1("Estado") = Row("Estado")
                DtMovimientos.Rows.Add(Row1)
            End If
        Next

        Return True

    End Function
    '----------------------------------------------------------------------------------------------------------------- 
    '------------------------------------- Analisis de Costo de un Lotes --------------------------------------------- 
    '----------------------------------------------------------------------------------------------------------------- 
    Public Function AnalisisCostoLote(ByVal ConMermaTr As Boolean, ByVal Operacion As Integer, ByVal Proveedor As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef Saldo As Decimal, ByRef PrecioS As Decimal, ByRef PrecioSSinIva As Decimal, ByRef Remitido As Decimal, ByRef Facturado As Decimal,
              ByRef Importe As Decimal, ByRef ImporteSinIva As Decimal, ByRef Baja As Decimal, ByRef Merma As Decimal, ByRef MermaTr As Decimal, ByRef Stock As Decimal, ByRef Liquidado As Decimal, ByRef PrecioF As Decimal,
              ByRef PrecioCompra As Decimal, ByRef CantidadInicial As Decimal, ByRef Senia As Decimal, ByRef Descarga As Decimal, ByRef DescargaSinIva As Decimal, ByRef GastoComercial As Decimal, ByRef GastoComercialSinIva As Decimal, ByVal ConCostos As Boolean, ByVal SinReintegro As Boolean, ByRef ListaOrden As String) As Boolean

        Dim Dt As New DataTable
        Dim DtAux As New DataTable
        Dim ListaBoniComercial As New List(Of Vigencia)
        Dim ListaBoniLogistica As New List(Of Vigencia)
        Dim ListaIngresoBruto As New List(Of Vigencia)
        Dim ListaFletePorBulto As New List(Of Vigencia)
        Dim ListaFletePorMedioBulto As New List(Of Vigencia)
        Dim ListaFletePorUnidad As New List(Of Vigencia)
        Dim ListaFletePorKilo As New List(Of Vigencia)
        Dim ListaImpDebCred As New List(Of Vigencia)
        Dim ListaCosto As New List(Of Vigencia)
        Dim KilosXUnidad As Decimal
        Dim BoniComercial As Decimal = 0
        Dim BoniComercialSinIva As Decimal = 0
        Dim BoniLogistica As Decimal = 0
        Dim BoniLogisticaSinIva As Decimal = 0
        Dim IngresoBruto As Decimal = 0
        Dim IngresoBrutoSinIva As Decimal = 0
        Dim ImpDebCred As Decimal = 0
        Dim ImpDebCredSinIva As Decimal = 0
        Dim FleteConIva As Decimal = 0
        Dim FleteSinIva As Decimal = 0
        Dim Envase As Integer = 0
        Dim EnvaseSinIva As Integer = 0
        Dim CostoConIva As Decimal = 0
        Dim CostoSinIva As Decimal = 0
        Dim FechaIngreso As DateTime
        Dim EsReventa As Boolean
        Dim EsCosteo As Boolean
        Dim Descarte As Decimal = 0
        Dim DiferenciaInventario As Decimal = 0
        Dim Articulo As Integer
        Dim Fecha As Date
        Dim ImporteLotesReintegrosConIva As Decimal = 0
        Dim ImporteLotesReintegrosSinIva As Decimal = 0
        Dim OrdenCompra As Decimal

        Remitido = 0
        Facturado = 0
        PrecioS = 0
        PrecioSSinIva = 0
        Importe = 0
        ImporteSinIva = 0
        Baja = 0
        Merma = 0
        MermaTr = 0
        Saldo = 0
        Stock = 0
        Liquidado = 0
        PrecioF = 0
        PrecioCompra = 0
        CantidadInicial = 0
        Senia = 0
        Descarga = 0
        DescargaSinIva = 0
        GastoComercial = 0
        GastoComercialSinIva = 0
        ListaOrden = ""           'Guarda "1" si tiene lista-precio, "2" si tiene Orden-Compra, "" Nada.  

        GListaLotesDeReintegros = New List(Of ItemCostosAsignados)

        Dim Sql As String = "SELECT L.*,C.OrdenCompra FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE L.LoteOrigen = " & Lote & " AND L.SecuenciaOrigen = " & Secuencia & ";"
        If Operacion = 1 Then
            If Not Tablas.Read(Sql, Conexion, Dt) Then Return False
        Else
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Return False
        End If

        For Each Row As DataRow In Dt.Rows
            Dim StrLote As String = "C.Lote = " & Row("Lote") & " AND C.Secuencia = " & Row("Secuencia") & " AND C.Deposito = " & Row("Deposito")
            If Row("Lote") = Row("LoteOrigen") And Row("Secuencia") = Row("SecuenciaOrigen") And Row("Deposito") = Row("DepositoOrigen") Then
                KilosXUnidad = Row("KilosXUnidad")
                Baja = Row("Baja")
                Liquidado = Row("Liquidado")
                PrecioF = Row("PrecioF")
                PrecioCompra = Row("PrecioCompra")
                CantidadInicial = Row("Cantidad")
                Articulo = Row("Articulo")
                Fecha = Row("Fecha")
                MermaTr = Row("MermaTr")
                OrdenCompra = Row("OrdenCompra")
                Envase = HallaEnvase(Row("Articulo"))
                If Envase < 0 Then Return False
                If Row("TipoOperacion") = 2 Then       'Reventa.
                    If Row("Senia") = -1 Then
                        Senia = CalculaSenia(Envase, Row("Fecha"))  'es sena que se cobra al Cliente y sale tabla de envases.
                        If Senia < 0 Then Return False
                    Else
                        Senia = Row("Senia")
                    End If
                    Senia = CalculaNeto(CantidadInicial - Baja, Senia)
                End If
                FechaIngreso = Row("Fecha")
                If Row("TipoOperacion") = 2 Then EsReventa = True
                If Row("TipoOperacion") = 4 Then EsCosteo = True
            End If
            Stock = Stock + Row("Stock") * Row("KilosXUnidad")
            Merma = Merma + Row("Merma") * Row("KilosXUnidad")
            DiferenciaInventario = DiferenciaInventario + Row("DiferenciaInventario") * Row("KilosXUnidad")
            Descarte = Descarte + Row("Descarte") * Row("KilosXUnidad")
            Dim CostoConIvaW As Decimal
            Dim CostoSinIvaW As Decimal
            If Not CalculaCostoEnvase(Row("Articulo"), Row("Fecha"), CostoConIvaW, CostoSinIvaW) Then Return False
            'Proceso de Remitos.
            Dim CantRemitida As Decimal = CantidadRemitida(Row("Lote"), Row("Secuencia"), Row("Deposito"), Conexion)
            If CantRemitida < 0 Then Return False
            Remitido = Remitido + CantRemitida * Row("KilosXUnidad")
            If PermisoTotal Then
                CantRemitida = CantidadRemitida(Row("Lote"), Row("Secuencia"), Row("Deposito"), ConexionN)
                If CantRemitida < 0 Then Return False
                Remitido = Remitido + CantRemitida * Row("KilosXUnidad")
            End If
            'procesa facturas.
            Dim StrFacturas As String = "SELECT C.Cantidad AS Cantidad,C.Importe AS Importe,C.ImporteSinIva AS ImporteSinIva, " &
                                        "F.Cliente AS Cliente,F.Fecha As Fecha,C.Rel FROM AsignacionLotes AS C INNER JOIN FacturasCabeza AS F ON C.Comprobante = F.Factura AND C.TipoComprobante = 2 " &
                                        "WHERE F.Tr = 0 AND " & StrLote & ";"
            DtAux = New DataTable
            If Not Tablas.Read(StrFacturas, Conexion, DtAux) Then Return False
            If PermisoTotal Then
                StrFacturas = "SELECT C.Cantidad AS Cantidad,C.Importe AS Importe,C.ImporteSinIva AS ImporteSinIva, " &
                              "F.Cliente AS Cliente,F.Fecha As Fecha,C.Rel FROM AsignacionLotes AS C INNER JOIN FacturasCabeza AS F ON C.Comprobante = F.Factura AND C.TipoComprobante = 2 " &
                              "WHERE F.Tr = 0 AND F.Rel = 0 AND " & StrLote & ";"   'Con Rel = 0(false) para sumar cantidad de los que tienen negro solo. 
                If Not Tablas.Read(StrFacturas, ConexionN, DtAux) Then Return False
                StrFacturas = "SELECT 0 AS Cantidad,C.Importe AS Importe,C.ImporteSinIva AS ImporteSinIva, " &
                              "F.Cliente AS Cliente,F.Fecha As Fecha,C.Rel FROM AsignacionLotes AS C INNER JOIN FacturasCabeza AS F ON C.Comprobante = F.Factura AND C.TipoComprobante = 2 " &
                              "WHERE F.Tr = 0 AND F.Rel = 1 AND " & StrLote & ";"   'Con Rel = 1(true) y cantidad = 0 para no sumar dos veces.
                If Not Tablas.Read(StrFacturas, ConexionN, DtAux) Then Return False
            End If
            'Procesa NVLP.
            StrFacturas = "SELECT C.Cantidad,C.NetoConIva AS Importe,C.NetoSinIva AS ImporteSinIva, " &
                                        "F.Cliente AS Cliente,F.Fecha As Fecha,F.Rel FROM NVLPLotes AS C INNER JOIN NVLPCabeza AS F ON C.Liquidacion = F.Liquidacion " &
                                        "WHERE F.Estado = 1 AND " & StrLote & ";"
            If Not Tablas.Read(StrFacturas, Conexion, DtAux) Then Return False
            If PermisoTotal Then
                StrFacturas = "SELECT C.Cantidad,C.NetoConIva AS Importe,C.NetoSinIva AS ImporteSinIva, " &
                              "F.Cliente AS Cliente,F.Fecha As Fecha,F.Rel FROM NVLPLotes AS C INNER JOIN NVLPCabeza AS F ON C.Liquidacion = F.Liquidacion " &
                              "WHERE F.Estado = 1 AND F.Rel = 0 AND " & StrLote & ";"   'Con Rel = 0(false) para sumar cantidad de los que tienen negro solo. 
                If Not Tablas.Read(StrFacturas, ConexionN, DtAux) Then Return False
                StrFacturas = "SELECT 0 AS Cantidad,C.NetoConIva AS Importe,C.NetoSinIva AS ImporteSinIva, " &
                              "F.Cliente AS Cliente,F.Fecha As Fecha,F.Rel FROM NVLPLotes AS C INNER JOIN NVLPCabeza AS F ON C.Liquidacion = F.Liquidacion " &
                              "WHERE F.Estado = 1 AND F.Rel = 1 AND " & StrLote & ";"   'Con Rel = 1(true) y cantidad = 0 para no sumar dos veces.
                If Not Tablas.Read(StrFacturas, ConexionN, DtAux) Then Return False
            End If
            '
            Dim View As New DataView
            View = DtAux.DefaultView
            View.Sort = "Cliente"
            Dim ClienteAnt As Integer = 0
            '
            For Each Row1 As DataRowView In View
                If Row1("Cliente") <> ClienteAnt Then
                    If Not ArmaLista(ListaBoniComercial, 1, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaBoniLogistica, 2, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaIngresoBruto, 3, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaFletePorBulto, Bulto, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaFletePorMedioBulto, MedioBulto, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaFletePorUnidad, Unidad, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaFletePorKilo, Kilo, Row1("Cliente")) Then Return False
                    If Not ArmaLista(ListaImpDebCred, 5, Row1("Cliente")) Then Return False
                    ClienteAnt = Row1("Cliente")
                End If
                Facturado = Facturado + Row1("Cantidad") * Row("KilosXUnidad")
                Dim ImporteW As Decimal = Row1("Importe")
                Dim ImporteSinIvaW As Decimal = Row1("ImporteSinIva")
                Dim BoniComercialW As Decimal = CalculaIva(1, ImporteSinIvaW, EncuentraVigencia(ListaBoniComercial, Row1("Fecha")))
                Dim BoniLogisticaW As Decimal = CalculaIva(1, ImporteSinIvaW, EncuentraVigencia(ListaBoniLogistica, Row1("Fecha")))
                Importe = Importe + ImporteW
                ImporteSinIva = ImporteSinIva + ImporteSinIvaW
                BoniComercial = BoniComercial + BoniComercialW + CalculaIva(1, BoniComercialW, EncuentraVigenciaAlicuota(ListaBoniComercial, Row1("Fecha")))
                BoniComercialSinIva = BoniComercialSinIva + BoniComercialW
                BoniLogistica = BoniLogistica + BoniLogisticaW + CalculaIva(1, BoniLogisticaW, EncuentraVigenciaAlicuota(ListaBoniLogistica, Row1("Fecha")))
                BoniLogisticaSinIva = BoniLogisticaSinIva + BoniLogisticaW
                IngresoBruto = IngresoBruto + CalculaIva(1, ImporteW, EncuentraVigencia(ListaIngresoBruto, Row1("Fecha")))
                IngresoBrutoSinIva = IngresoBrutoSinIva + CalculaIva(1, ImporteSinIvaW, EncuentraVigencia(ListaIngresoBruto, Row1("Fecha")))
                ImpDebCred = ImpDebCred + CalculaIva(1, ImporteW, EncuentraVigencia(ListaImpDebCred, Row1("Fecha")))
                ImpDebCredSinIva = ImpDebCredSinIva + CalculaIva(1, ImporteSinIvaW, EncuentraVigencia(ListaImpDebCred, Row1("Fecha")))
                Dim FleteConIvaW As Decimal = 0
                Dim FleteSinIvaW As Decimal = 0
                If Not CalculaFlete(ListaFletePorBulto, ListaFletePorMedioBulto, ListaFletePorUnidad, ListaFletePorKilo, Row("Articulo"), Row1("Fecha"), FleteConIvaW, FleteSinIvaW) Then
                    Return False
                End If
                FleteConIva = FleteConIva + CalculaNeto(Row1("Cantidad"), FleteConIvaW)
                FleteSinIva = FleteSinIva + CalculaNeto(Row1("Cantidad"), FleteSinIvaW)
                CostoConIva = CostoConIva + CalculaNeto(Row1("Cantidad"), CostoConIvaW)
                CostoSinIva = CostoSinIva + CalculaNeto(Row1("Cantidad"), CostoSinIvaW)
            Next
            'Halla costo de reintegros para lotes primarios y reprocesos.
            Dim LotesReintegrosConIva As Decimal = 0
            Dim LotesReintegrosSinIva As Decimal = 0
            If Not SinReintegro Then
                HallaImportesLotesReintegros(Row("Lote"), Row("Secuencia"), LotesReintegrosConIva, LotesReintegrosSinIva)
                ImporteLotesReintegrosConIva = ImporteLotesReintegrosConIva + LotesReintegrosConIva
                ImporteLotesReintegrosSinIva = ImporteLotesReintegrosSinIva + LotesReintegrosSinIva
            End If
            'Halla costo de consumos de lotes productos Terminados de Costeos.
            Dim LotesConsumosTerminados As Decimal = 0
            Dim CantidadLotesConsumosTerminados As Decimal = 0
            ''''  If EsCosteo Then
            HallaImportesLotesConsumosTermindos(Row("Lote"), Row("Secuencia"), LotesConsumosTerminados, CantidadLotesConsumosTerminados)
            Importe = Importe + LotesConsumosTerminados
            ImporteSinIva = ImporteSinIva + LotesConsumosTerminados
            Facturado = Facturado + CantidadLotesConsumosTerminados * KilosXUnidad
            ''''  End If
        Next

        GastoComercial = BoniComercial + BoniLogistica + IngresoBruto + ImpDebCred + FleteConIva + CostoConIva
        GastoComercialSinIva = BoniComercialSinIva + BoniLogisticaSinIva + IngresoBrutoSinIva + ImpDebCredSinIva + FleteSinIva + CostoSinIva

        Descarte = Trunca(Descarte / KilosXUnidad)
        Facturado = Trunca(Facturado / KilosXUnidad)
        Merma = Trunca(Merma / KilosXUnidad)
        DiferenciaInventario = Trunca(DiferenciaInventario / KilosXUnidad)
        Remitido = Trunca(Remitido / KilosXUnidad)
        Stock = Trunca(Stock / KilosXUnidad)
        '        Stock = Trunca(CantidadInicial - Baja - Remitido - Facturado - Merma - DiferenciaInventario - Descarte)
        'Halla costo de descarga.
        If Not BuscaVigenciaValorAlicuota(11, FechaIngreso, Descarga, DescargaSinIva, Envase) Then Return False
        Descarga = CalculaNeto(CantidadInicial, Descarga)
        DescargaSinIva = CalculaNeto(CantidadInicial, DescargaSinIva)
        '
        Dim ComisionAdicional As Double = HallaComisionAdicional(Proveedor)
        If ComisionAdicional < 0 Then Return False
        '
        Dim CantidadW As Decimal = 0

        If EsReventa Or EsCosteo Then
            CantidadW = CantidadInicial - Baja
        Else
            CantidadW = CantidadInicial - Merma - Baja - Descarte   'Si es condignacion.
        End If

        Dim OtrosCostosConIva As Decimal = 0
        Dim OtrosCostosSinIva As Decimal = 0

        If ConCostos Then   'Costos por Facturas de Proveedor que afecta al lote, Consumos de insumos para el lote, Costos asociados al costeo del lote.
            If Not HallaCostosDelLote(Operacion, Lote, Secuencia, Proveedor, Articulo, Fecha, CantidadInicial, Baja, KilosXUnidad, False, OtrosCostosConIva, OtrosCostosSinIva) Then Return False
        End If

        Dim ImporteLotesRecibosConIva As Decimal = 0
        Dim ImporteLotesRecibosSinIva As Decimal = 0
        HallaImportesLotesRecibos(Lote, Secuencia, ImporteLotesRecibosConIva, ImporteLotesRecibosSinIva)
        OtrosCostosConIva = OtrosCostosConIva + ImporteLotesRecibosConIva
        OtrosCostosSinIva = OtrosCostosSinIva + ImporteLotesRecibosSinIva
        '
        'resta reintegros de Otros Costos.
        OtrosCostosConIva = OtrosCostosConIva - ImporteLotesReintegrosConIva
        OtrosCostosSinIva = OtrosCostosSinIva - ImporteLotesReintegrosSinIva
        '
        Dim ImporteLotesOtrasFacturasConIva As Decimal = 0
        Dim ImporteLotesOtrasfacturasSinIva As Decimal = 0
        HallaImportesLotesOtrasFacturas(Lote, Secuencia, ImporteLotesOtrasFacturasConIva, ImporteLotesOtrasfacturasSinIva)
        OtrosCostosConIva = OtrosCostosConIva + ImporteLotesOtrasFacturasConIva
        OtrosCostosSinIva = OtrosCostosSinIva + ImporteLotesOtrasfacturasSinIva
        '
        Dim ImporteLotesAsientosManualesConIva As Decimal = 0
        Dim ImporteLotesAsientosManualesSinIva As Decimal = 0
        HallaImportesLotesAsientosManuales(Lote, Secuencia, ImporteLotesAsientosManualesConIva, ImporteLotesAsientosManualesSinIva)
        OtrosCostosConIva = OtrosCostosConIva + ImporteLotesAsientosManualesConIva
        OtrosCostosSinIva = OtrosCostosSinIva + ImporteLotesAsientosManualesSinIva
        '
        Dim ImporteMermasNVLPConIva As Double = 0
        Dim ImporteMermasNLPSinIva As Double = 0
        HallaImporteMermasNVLP(Lote, Secuencia, ImporteMermasNVLPConIva, ImporteMermasNLPSinIva)
        OtrosCostosConIva = OtrosCostosConIva + ImporteMermasNVLPConIva
        OtrosCostosSinIva = OtrosCostosSinIva + ImporteMermasNLPSinIva

        If CantidadW > 0 Then
            If EsReventa Or EsCosteo Then
                PrecioS = Trunca5((Importe - (Importe * ComisionAdicional / 100) - BoniComercial - BoniLogistica - FleteConIva - IngresoBruto - ImpDebCred - CostoConIva - Descarga - OtrosCostosConIva) / CantidadW)
                PrecioSSinIva = Trunca((ImporteSinIva - (ImporteSinIva * ComisionAdicional / 100) - BoniComercialSinIva - BoniLogisticaSinIva - FleteSinIva - IngresoBrutoSinIva - ImpDebCredSinIva - CostoSinIva - DescargaSinIva - OtrosCostosSinIva) / CantidadW)
            Else
                PrecioS = Trunca5((Importe - (Importe * ComisionAdicional / 100) - BoniComercial - BoniLogistica - FleteConIva - IngresoBruto - ImpDebCred - CostoConIva - OtrosCostosConIva) / CantidadW)
                PrecioSSinIva = Trunca((ImporteSinIva - (ImporteSinIva * ComisionAdicional / 100) - BoniComercialSinIva - BoniLogisticaSinIva - FleteSinIva - IngresoBrutoSinIva - ImpDebCredSinIva - CostoSinIva - OtrosCostosSinIva) / CantidadW)
            End If
        End If

        Merma = Merma + Descarte

        If EsReventa Then
            If PrecioF = 0 And OrdenCompra <> 0 Then
                PrecioF = HallaOrdenCompraYPrecio(Proveedor, Articulo, Operacion, Lote)
                If PrecioF <> 0 Then ListaOrden = "2" 'tiene orden compra.
            End If
            If PrecioF = 0 Then
                PrecioF = HallaPrecioDeListaDePrecios(Proveedor, Lote, Fecha, Articulo, KilosXUnidad, Operacion)
                If PrecioF <> 0 Then ListaOrden = "1" 'tiene lista de precio.
            End If
        End If

        DtAux.Dispose()
        Dt.Dispose()

        Return True

    End Function
    '----------------------------------------------------------------------------------------------------------------- 
    '------------------------------------- Analisis de Ventas por Lote Original--------------------------------------- 
    '----------------------------------------------------------------------------------------------------------------- 
    Public Function GestionVentaLoteOriginal(ByVal ConRemitidos As Boolean, ByVal Operacion As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef Remitido As Decimal, ByRef Stock As Decimal, ByRef Facturado As Decimal,
                            ByRef CantidadInicial As Decimal, ByRef Baja As Decimal, ByRef Importe As Decimal, ByRef ImporteSinIva As Decimal, ByRef Senia As Decimal, ByRef GastoComercial As Decimal, ByRef GastoComercialSinIva As Decimal) As Boolean

        Dim Dt As New DataTable
        Dim DtAux As New DataTable
        Dim KilosXUnidad As Decimal
        Dim Envase As Integer = 0
        Dim EnvaseSinIva As Integer = 0
        Dim FechaIngreso As DateTime
        Dim EsReventa As Boolean
        Dim EsCosteo As Boolean
        Dim Articulo As Integer
        Dim Fecha As Date
        Dim KilosFacturado As Decimal = 0

        Remitido = 0
        Facturado = 0
        Stock = 0
        CantidadInicial = 0
        Baja = 0
        Importe = 0
        ImporteSinIva = 0
        Senia = 0
        GastoComercial = 0
        GastoComercialSinIva = 0

        GListaLotesDeReintegros = New List(Of ItemCostosAsignados)

        Dim Sql As String = "SELECT * FROM Lotes WHERE Lotes.LoteOrigen = " & Lote & " AND Lotes.SecuenciaOrigen = " & Secuencia & ";"
        If Operacion = 1 Then
            If Not Tablas.Read(Sql, Conexion, Dt) Then Return False
        Else
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Return False
        End If

        For Each Row As DataRow In Dt.Rows
            If Row("Lote") = Row("LoteOrigen") And Row("Secuencia") = Row("SecuenciaOrigen") And Row("Deposito") = Row("DepositoOrigen") Then
                KilosXUnidad = Row("KilosXUnidad")
                Articulo = Row("Articulo")
                Fecha = Row("Fecha")
                CantidadInicial = Row("Cantidad")
                Baja = Row("Baja")
                Envase = HallaEnvase(Row("Articulo"))
                If Envase < 0 Then Return False
                If Row("TipoOperacion") = 2 Then
                    If Row("Senia") = -1 Then
                        Senia = CalculaSenia(Envase, Row("Fecha"))  'es sena que se cobra al Cliente y sale tabla de envases.
                        If Senia < 0 Then Return False
                    Else
                        Senia = Row("Senia")
                    End If
                    Senia = CalculaNeto(CantidadInicial - Baja, Senia)
                End If
                FechaIngreso = Row("Fecha")
                If Row("TipoOperacion") = 2 Then EsReventa = True
                If Row("TipoOperacion") = 4 Then EsCosteo = True
            End If
            Stock = Stock + Row("Stock") * Row("KilosXUnidad")
            '
            Dim FechaW As Date
            Dim FacturadoW As Decimal = 0
            Dim KilosFacturadoW As Decimal = 0
            Dim ImporteW As Decimal = 0
            Dim ImporteSinIvaW As Decimal = 0
            Dim GastoComercialW As Decimal = 0
            Dim GastoComercialSinIvaW As Decimal = 0
            If Not GestionLotesVendidos(0, Operacion, Row("Lote"), Row("Secuencia"), Row("Deposito"), FechaW, FacturadoW, KilosFacturadoW, ImporteW, ImporteSinIvaW, GastoComercialW, GastoComercialSinIvaW) Then Return False
            Importe = Importe + ImporteW
            ImporteSinIva = ImporteSinIva + ImporteSinIvaW
            KilosFacturado = KilosFacturado + KilosFacturadoW
            GastoComercial = GastoComercial + GastoComercialW
            GastoComercialSinIva = GastoComercialSinIva + GastoComercialSinIvaW
            '
            'Proceso de Remitos.
            If ConRemitidos Then
                Dim CantRemitida As Decimal = CantidadRemitida(Row("Lote"), Row("Secuencia"), Row("Deposito"), Conexion)
                If CantRemitida < 0 Then Return False
                Remitido = Remitido + CantRemitida * Row("KilosXUnidad")
                If PermisoTotal Then
                    CantRemitida = CantidadRemitida(Row("Lote"), Row("Secuencia"), Row("Deposito"), ConexionN)
                    If CantRemitida < 0 Then Return False
                    Remitido = Remitido + CantRemitida * Row("KilosXUnidad")
                End If
            End If
        Next

        Facturado = KilosFacturado / KilosXUnidad
        Stock = Stock / KilosXUnidad

        DtAux.Dispose()
        Dt.Dispose()

        Return True

    End Function
    '----------------------------------------------------------------------------------------------------------------- 
    '------------------------------------- Analisis de Ventas por Lotes ---------------------------------------------- 
    '----------------------------------------------------------------------------------------------------------------- 
    Public Function GestionLotesVendidos(ByVal Cliente As Integer, ByVal Operacion As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Deposito As Integer, ByVal FechaIngreso As Date, ByRef Facturado As Decimal, ByRef KilosFacturado As Decimal,
                ByRef Importe As Decimal, ByRef ImporteSinIva As Decimal, ByRef GastoComercial As Decimal, ByRef GastoComercialSinIva As Decimal) As Boolean

        Dim Dt As New DataTable
        Dim DtAux As New DataTable
        Dim ListaBoniComercial As New List(Of Vigencia)
        Dim ListaBoniLogistica As New List(Of Vigencia)
        Dim ListaIngresoBruto As New List(Of Vigencia)
        Dim ListaFletePorBulto As New List(Of Vigencia)
        Dim ListaFletePorMedioBulto As New List(Of Vigencia)
        Dim ListaFletePorUnidad As New List(Of Vigencia)
        Dim ListaFletePorKilo As New List(Of Vigencia)
        Dim ListaImpDebCred As New List(Of Vigencia)
        Dim ListaCosto As New List(Of Vigencia)
        Dim BoniComercial As Decimal = 0
        Dim BoniComercialSinIva As Decimal = 0
        Dim BoniLogistica As Decimal = 0
        Dim BoniLogisticaSinIva As Decimal = 0
        Dim IngresoBruto As Decimal = 0
        Dim IngresoBrutoSinIva As Decimal = 0
        Dim ImpDebCred As Decimal = 0
        Dim ImpDebCredSinIva As Decimal = 0
        Dim FleteConIva As Decimal = 0
        Dim FleteSinIva As Decimal = 0
        Dim Envase As Integer = 0
        Dim EnvaseSinIva As Integer = 0
        Dim CostoConIva As Decimal = 0
        Dim CostoSinIva As Decimal = 0
        Dim EsReventa As Boolean
        Dim EsCosteo As Boolean
        Dim Fecha As Date
        Dim Articulo As Integer = 0
        Dim KilosXUnidad As Decimal = 0
        Dim ImporteLotesReintegrosConIva As Decimal = 0
        Dim ImporteLotesReintegrosSinIva As Decimal = 0
        Dim Proveedor As Integer

        Facturado = 0
        KilosFacturado = 0
        Importe = 0
        ImporteSinIva = 0
        GastoComercial = 0
        GastoComercialSinIva = 0

        Dim ConexionStr
        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim Sql As String = "SELECT Articulo,KilosXUnidad,Proveedor FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"

        If Not Tablas.Read(Sql, ConexionStr, Dt) Then Return False
        KilosXUnidad = Dt.Rows(0).Item("KilosXUnidad")
        Articulo = Dt.Rows(0).Item("Articulo")
        Proveedor = Dt.Rows(0).Item("Proveedor")

        Dim CostoConIvaW As Decimal
        Dim CostoSinIvaW As Decimal
        If Not CalculaCostoEnvase(Articulo, FechaIngreso, CostoConIvaW, CostoSinIvaW) Then Return False
        '
        'procesa facturas.Halla importe facturado........................................................................................................
        Dim StrLote As String
        If Deposito = 0 Then
            StrLote = "C.Lote = " & Lote & " AND C.Secuencia = " & Secuencia
        Else
            StrLote = "C.Lote = " & Lote & " AND C.Secuencia = " & Secuencia & " AND C.Deposito = " & Deposito
        End If
        Dim SqlCliente As String = ""
        If Cliente <> 0 Then
            SqlCliente = " AND F.Cliente = " & Cliente
        End If
        Dim StrFacturas As String = "SELECT C.Cantidad AS Cantidad,C.Importe AS Importe,C.ImporteSinIva AS ImporteSinIva, " &
                                    "F.Cliente AS Cliente,F.Fecha As Fecha,C.Rel FROM AsignacionLotes AS C INNER JOIN FacturasCabeza AS F ON C.Comprobante = F.Factura AND C.TipoComprobante = 2 " &
                                    "WHERE F.Tr = 0 AND " & StrLote & SqlCliente & ";"
        DtAux = New DataTable
        If Not Tablas.Read(StrFacturas, Conexion, DtAux) Then Return False
        If PermisoTotal Then
            StrFacturas = "SELECT C.Cantidad AS Cantidad,C.Importe AS Importe,C.ImporteSinIva AS ImporteSinIva, " &
                          "F.Cliente AS Cliente,F.Fecha As Fecha,C.Rel FROM AsignacionLotes AS C INNER JOIN FacturasCabeza AS F ON C.Comprobante = F.Factura AND C.TipoComprobante = 2 " &
                          "WHERE F.Tr = 0 AND F.Rel = 0 AND " & StrLote & SqlCliente & ";"   'Con Rel = 0(false) para sumar cantidad de los que tienen negro solo. 
            If Not Tablas.Read(StrFacturas, ConexionN, DtAux) Then Return False
            StrFacturas = "SELECT 0 AS Cantidad,C.Importe AS Importe,C.ImporteSinIva AS ImporteSinIva, " &
                          "F.Cliente AS Cliente,F.Fecha As Fecha,C.Rel FROM AsignacionLotes AS C INNER JOIN FacturasCabeza AS F ON C.Comprobante = F.Factura AND C.TipoComprobante = 2 " &
                          "WHERE F.Tr = 0 AND F.Rel = 1 AND " & StrLote & SqlCliente & ";"   'Con Rel = 1(true) y cantidad = 0 para no sumar dos veces.
            If Not Tablas.Read(StrFacturas, ConexionN, DtAux) Then Return False
        End If
        'Procesa NVLP.
        StrFacturas = "SELECT C.Cantidad,C.NetoConIva AS Importe,C.NetoSinIva AS ImporteSinIva, " &
                                    "F.Cliente AS Cliente,F.Fecha As Fecha,F.Rel FROM NVLPLotes AS C INNER JOIN NVLPCabeza AS F ON C.Liquidacion = F.Liquidacion " &
                                    "WHERE F.Estado = 1 AND " & StrLote & SqlCliente & ";"
        If Not Tablas.Read(StrFacturas, Conexion, DtAux) Then Return False
        If PermisoTotal Then
            StrFacturas = "SELECT C.Cantidad,C.NetoConIva AS Importe,C.NetoSinIva AS ImporteSinIva, " &
                          "F.Cliente AS Cliente,F.Fecha As Fecha,F.Rel FROM NVLPLotes AS C INNER JOIN NVLPCabeza AS F ON C.Liquidacion = F.Liquidacion " &
                          "WHERE F.Estado = 1 AND F.Rel = 0 AND " & StrLote & SqlCliente & ";"   'Con Rel = 0(false) para sumar cantidad de los que tienen negro solo. 
            If Not Tablas.Read(StrFacturas, ConexionN, DtAux) Then Return False
            StrFacturas = "SELECT 0 AS Cantidad,C.NetoConIva AS Importe,C.NetoSinIva AS ImporteSinIva, " &
                          "F.Cliente AS Cliente,F.Fecha As Fecha,F.Rel FROM NVLPLotes AS C INNER JOIN NVLPCabeza AS F ON C.Liquidacion = F.Liquidacion " &
                          "WHERE F.Estado = 1 AND F.Rel = 1 AND " & StrLote & SqlCliente & ";"   'Con Rel = 1(true) y cantidad = 0 para no sumar dos veces.
            If Not Tablas.Read(StrFacturas, ConexionN, DtAux) Then Return False
        End If
        '
        Dim View As New DataView
        View = DtAux.DefaultView
        View.Sort = "Cliente"
        Dim ClienteAnt As Integer = 0
        '
        For Each Row1 As DataRowView In View
            If Row1("Cliente") <> ClienteAnt Then
                If Not ArmaLista(ListaBoniComercial, 1, Row1("Cliente")) Then Return False
                If Not ArmaLista(ListaBoniLogistica, 2, Row1("Cliente")) Then Return False
                If Not ArmaLista(ListaIngresoBruto, 3, Row1("Cliente")) Then Return False
                If Not ArmaLista(ListaFletePorBulto, Bulto, Row1("Cliente")) Then Return False
                If Not ArmaLista(ListaFletePorMedioBulto, MedioBulto, Row1("Cliente")) Then Return False
                If Not ArmaLista(ListaFletePorUnidad, Unidad, Row1("Cliente")) Then Return False
                If Not ArmaLista(ListaFletePorKilo, Kilo, Row1("Cliente")) Then Return False
                If Not ArmaLista(ListaImpDebCred, 5, Row1("Cliente")) Then Return False
                ClienteAnt = Row1("Cliente")
            End If
            KilosFacturado = KilosFacturado + Row1("Cantidad") * KilosXUnidad
            Facturado = Facturado + Row1("Cantidad")
            Dim ImporteW As Decimal = Row1("Importe")
            Dim ImporteSinIvaW As Decimal = Row1("ImporteSinIva")
            Dim BoniComercialW As Decimal = CalculaIva(1, ImporteSinIvaW, EncuentraVigencia(ListaBoniComercial, Row1("Fecha")))
            Dim BoniLogisticaW As Decimal = CalculaIva(1, ImporteSinIvaW, EncuentraVigencia(ListaBoniLogistica, Row1("Fecha")))
            Importe = Importe + ImporteW
            ImporteSinIva = ImporteSinIva + ImporteSinIvaW
            BoniComercial = BoniComercial + BoniComercialW + CalculaIva(1, BoniComercialW, EncuentraVigenciaAlicuota(ListaBoniComercial, Row1("Fecha")))
            BoniComercialSinIva = BoniComercialSinIva + BoniComercialW
            BoniLogistica = BoniLogistica + BoniLogisticaW + CalculaIva(1, BoniLogisticaW, EncuentraVigenciaAlicuota(ListaBoniLogistica, Row1("Fecha")))
            BoniLogisticaSinIva = BoniLogisticaSinIva + BoniLogisticaW
            IngresoBruto = IngresoBruto + CalculaIva(1, ImporteW, EncuentraVigencia(ListaIngresoBruto, Row1("Fecha")))
            IngresoBrutoSinIva = IngresoBrutoSinIva + CalculaIva(1, ImporteSinIvaW, EncuentraVigencia(ListaIngresoBruto, Row1("Fecha")))
            ImpDebCred = ImpDebCred + CalculaIva(1, ImporteW, EncuentraVigencia(ListaImpDebCred, Row1("Fecha")))
            ImpDebCredSinIva = ImpDebCredSinIva + CalculaIva(1, ImporteSinIvaW, EncuentraVigencia(ListaImpDebCred, Row1("Fecha")))
            Dim FleteConIvaW As Decimal = 0
            Dim FleteSinIvaW As Decimal = 0
            If Not CalculaFlete(ListaFletePorBulto, ListaFletePorMedioBulto, ListaFletePorUnidad, ListaFletePorKilo, Articulo, Row1("Fecha"), FleteConIvaW, FleteSinIvaW) Then
                Return False
            End If
            FleteConIva = FleteConIva + CalculaNeto(Row1("Cantidad"), FleteConIvaW)
            FleteSinIva = FleteSinIva + CalculaNeto(Row1("Cantidad"), FleteSinIvaW)
            CostoConIva = CostoConIva + CalculaNeto(Row1("Cantidad"), CostoConIvaW)
            CostoSinIva = CostoSinIva + CalculaNeto(Row1("Cantidad"), CostoSinIvaW)
        Next

        GastoComercial = BoniComercial + BoniLogistica + IngresoBruto + ImpDebCred + FleteConIva + CostoConIva
        GastoComercialSinIva = BoniComercialSinIva + BoniLogisticaSinIva + IngresoBrutoSinIva + ImpDebCredSinIva + FleteSinIva + CostoSinIva

        DtAux.Dispose()
        Dt.Dispose()

        Return True

    End Function
    Public Function CostoDeUnLote(ByVal Operacion As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef Baja As Decimal, ByRef Merma As Decimal, ByRef Stock As Decimal, ByRef Liquidado As Decimal,
               ByRef CantidadInicial As Decimal, ByRef KilosXUnidad As Decimal, ByRef FechaIngreso As Date, ByRef Senia As Decimal, ByRef Descarga As Decimal, ByRef DescargaSinIva As Decimal, ByRef OtrosCostosConIva As Decimal, ByRef OtrosCostosSinIva As Decimal, ByRef CostoFruta As Decimal, ByRef CostoFrutaSinIva As Decimal, ByRef Total As Decimal, ByRef TotalSinIva As Decimal, ByVal SinReintegro As Boolean) As Boolean

        Dim Dt As New DataTable
        Dim ListaCosto As New List(Of Vigencia)
        Dim Envase As Integer = 0
        Dim EnvaseSinIva As Integer = 0
        Dim EsReventa As Boolean
        Dim EsCosteo As Boolean
        Dim Descarte As Decimal = 0
        Dim DiferenciaInventario As Decimal = 0
        Dim Articulo As Integer
        Dim Proveedor As Integer
        Dim Fecha As Date
        Dim ImporteLotesReintegrosConIva As Decimal = 0
        Dim ImporteLotesReintegrosSinIva As Decimal = 0

        Baja = 0
        Merma = 0
        Stock = 0
        Liquidado = 0
        CantidadInicial = 0
        Senia = 0
        Descarga = 0
        DescargaSinIva = 0
        KilosXUnidad = 0

        GListaLotesDeReintegros = New List(Of ItemCostosAsignados)

        Dim ConexionStr
        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim Sql As String = "SELECT * FROM Lotes WHERE LoteOrigen = " & Lote & " AND SecuenciaOrigen = " & Secuencia & ";"
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then Return False

        For Each Row As DataRow In Dt.Rows
            Dim StrLote As String = "C.Lote = " & Row("Lote") & " AND C.Secuencia = " & Row("Secuencia") & " AND C.Deposito = " & Row("Deposito")
            If Row("Lote") = Row("LoteOrigen") And Row("Secuencia") = Row("SecuenciaOrigen") And Row("Deposito") = Row("DepositoOrigen") Then
                KilosXUnidad = Row("KilosXUnidad")
                Baja = Row("Baja")
                Liquidado = Row("Liquidado")
                CantidadInicial = Row("Cantidad")
                Articulo = Row("Articulo")
                Fecha = Row("Fecha")
                Envase = HallaEnvase(Row("Articulo"))
                Proveedor = Row("Proveedor")
                If Envase < 0 Then Return False
                If Row("TipoOperacion") = 2 Then
                    If Row("Senia") = -1 Then
                        Senia = CalculaSenia(Envase, Row("Fecha"))  'es sena que se cobra al Cliente y sale tabla de envases.
                        If Senia < 0 Then Return False
                    Else
                        Senia = Row("Senia")
                    End If
                    Senia = CalculaNeto(CantidadInicial - Baja, Senia)
                End If
                FechaIngreso = Row("Fecha")
                If Row("TipoOperacion") = 2 Then EsReventa = True
                If Row("TipoOperacion") = 4 Then EsCosteo = True
            End If
            Stock = Stock + Row("Stock") * Row("KilosXUnidad")
            Merma = Merma + Row("Merma") * Row("KilosXUnidad")
            DiferenciaInventario = DiferenciaInventario + Row("DiferenciaInventario") * Row("KilosXUnidad")
            Descarte = Descarte + Row("Descarte") * Row("KilosXUnidad")
            'Halla costo de reintegros para lotes primarios y reprocesos.
            Dim LotesReintegrosConIva As Decimal = 0
            Dim LotesReintegrosSinIva As Decimal = 0
            If Not SinReintegro Then
                HallaImportesLotesReintegros(Row("Lote"), Row("Secuencia"), LotesReintegrosConIva, LotesReintegrosSinIva)
                ImporteLotesReintegrosConIva = ImporteLotesReintegrosConIva + LotesReintegrosConIva
                ImporteLotesReintegrosSinIva = ImporteLotesReintegrosSinIva + LotesReintegrosSinIva
            End If
        Next

        Descarte = Trunca(Descarte / KilosXUnidad)
        Merma = Trunca(Merma / KilosXUnidad)
        DiferenciaInventario = Trunca(DiferenciaInventario / KilosXUnidad)
        Stock = Trunca(Stock / KilosXUnidad)

        OtrosCostosConIva = 0
        OtrosCostosSinIva = 0

        'Halla costo de descarga.
        If Not BuscaVigenciaValorAlicuota(11, FechaIngreso, Descarga, DescargaSinIva, Envase) Then Return False
        Descarga = CalculaNeto(CantidadInicial, Descarga)
        DescargaSinIva = CalculaNeto(CantidadInicial, DescargaSinIva)
        OtrosCostosConIva = OtrosCostosConIva + Descarga
        OtrosCostosSinIva = OtrosCostosSinIva + DescargaSinIva
        '
        'Costos por Facturas de Proveedor que afecta al lote.......
        Dim ImporteFacturasProveedorAfectaLotesConIva As Decimal = 0
        Dim ImporteFacturasProveedorAfectaLotesSinIva As Decimal = 0
        If Not FacturaAfectaLotes(Lote, Secuencia, ImporteFacturasProveedorAfectaLotesConIva, ImporteFacturasProveedorAfectaLotesSinIva) Then Return False
        OtrosCostosConIva = OtrosCostosConIva + ImporteFacturasProveedorAfectaLotesConIva
        OtrosCostosSinIva = OtrosCostosSinIva + ImporteFacturasProveedorAfectaLotesSinIva
        '
        'costos por consumos de insumos........................................
        Dim ImportePorConsumosDeInsumosConIva As Decimal = 0
        Dim ImportePorConsumosDeInsumosSinIva As Decimal = 0
        If HallaCostoInsumosPorLote(Lote, Secuencia, ImportePorConsumosDeInsumosConIva, ImportePorConsumosDeInsumosSinIva) < 0 Then Return False
        OtrosCostosConIva = OtrosCostosConIva + ImportePorConsumosDeInsumosConIva
        OtrosCostosSinIva = OtrosCostosSinIva + ImportePorConsumosDeInsumosSinIva
        '
        'costos del Costeo......................................................
        Dim ImporteCostoDelCosteoConIva As Decimal = 0
        Dim ImporteCostoDelCosteoSinIva As Decimal = 0
        If EsCosteo Then
            If Not HallaCostoCosteoXKilo(Operacion, Lote, ImporteCostoDelCosteoConIva, ImporteCostoDelCosteoSinIva) Then Return False
            ImporteCostoDelCosteoConIva = ImporteCostoDelCosteoConIva * (CantidadInicial - Baja) * KilosXUnidad
            ImporteCostoDelCosteoSinIva = ImporteCostoDelCosteoSinIva * (CantidadInicial - Baja) * KilosXUnidad
        End If
        OtrosCostosConIva = OtrosCostosConIva + ImporteCostoDelCosteoConIva
        OtrosCostosSinIva = OtrosCostosSinIva + ImporteCostoDelCosteoSinIva
        '
        'Costos en lotes en recibos...............................................
        Dim ImporteLotesRecibosConIva As Decimal = 0
        Dim ImporteLotesRecibosSinIva As Decimal = 0
        HallaImportesLotesRecibos(Lote, Secuencia, ImporteLotesRecibosConIva, ImporteLotesRecibosSinIva)
        OtrosCostosConIva = OtrosCostosConIva + ImporteLotesRecibosConIva
        OtrosCostosSinIva = OtrosCostosSinIva + ImporteLotesRecibosSinIva
        '
        'resta reintegros de Otros Costos.
        OtrosCostosConIva = OtrosCostosConIva - ImporteLotesReintegrosConIva
        OtrosCostosSinIva = OtrosCostosSinIva - ImporteLotesReintegrosSinIva
        '
        Dim ImporteLotesOtrasFacturasConIva As Decimal = 0
        Dim ImporteLotesOtrasfacturasSinIva As Decimal = 0
        HallaImportesLotesOtrasFacturas(Lote, Secuencia, ImporteLotesOtrasFacturasConIva, ImporteLotesOtrasfacturasSinIva)
        OtrosCostosConIva = OtrosCostosConIva + ImporteLotesOtrasFacturasConIva
        OtrosCostosSinIva = OtrosCostosSinIva + ImporteLotesOtrasfacturasSinIva
        '
        Dim ImporteLotesAsientosManualesConIva As Decimal = 0
        Dim ImporteLotesAsientosManualesSinIva As Decimal = 0
        HallaImportesLotesAsientosManuales(Lote, Secuencia, ImporteLotesAsientosManualesConIva, ImporteLotesAsientosManualesSinIva)
        OtrosCostosConIva = OtrosCostosConIva + ImporteLotesAsientosManualesConIva
        OtrosCostosSinIva = OtrosCostosSinIva + ImporteLotesAsientosManualesSinIva
        '
        Dim ImporteMermasNVLPConIva As Double = 0
        Dim ImporteMermasNLPSinIva As Double = 0
        HallaImporteMermasNVLP(Lote, Secuencia, ImporteMermasNVLPConIva, ImporteMermasNLPSinIva)
        OtrosCostosConIva = OtrosCostosConIva + ImporteMermasNVLPConIva
        OtrosCostosSinIva = OtrosCostosSinIva + ImporteMermasNLPSinIva

        'costo fruta.
        Dim ImporteCostoFruta As Decimal = 0       ' Costo de pago por el lote.
        Dim ImporteCostoFrutaSinIva As Decimal = 0
        FacturaReventaLiquidacion(Lote, Secuencia, Operacion, ImporteCostoFruta, ImporteCostoFrutaSinIva)
        CostoFruta = ImporteCostoFruta
        CostoFrutaSinIva = ImporteCostoFrutaSinIva

        If EsReventa Or EsCosteo Then
            Total = Descarga + OtrosCostosConIva + ImporteCostoFruta
            TotalSinIva = DescargaSinIva + OtrosCostosSinIva + ImporteCostoFrutaSinIva
        Else
            Total = OtrosCostosConIva + ImporteCostoFruta
            TotalSinIva = OtrosCostosSinIva + ImporteCostoFrutaSinIva
        End If

        Merma = Merma + Descarte

        Dt.Dispose()

        Return True

    End Function
    Public Function HallaPrecioDeListaDePrecios(ByVal Proveedor As Integer, ByVal Lote As Integer, ByVal Fecha As Date, ByVal Articulo As Integer, ByVal KilosXUnidad As Decimal, ByVal Operacion As Integer) As Decimal

        Dim Lista As Integer = 0
        Dim PorUnidadEnLista As Boolean
        Dim FinalEnLista As Boolean
        Dim Sucursal As Integer = 0
        Dim Zona As Integer = 0
        Dim Precio As Decimal = 0
        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Sucursal = HallaSucursalLote(Lote, ConexionStr)
        If Sucursal <> 0 Then Zona = HallaZonaProveedor(Proveedor, Sucursal)

        If Zona <> 0 Then
            If HallaListaProveedorConZona(Proveedor, Fecha, Lista, PorUnidadEnLista, FinalEnLista, Zona) < 0 Then Return 0
        End If
        If Lista = 0 Then
            If HallaListaProveedorConZona(Proveedor, Fecha, Lista, PorUnidadEnLista, FinalEnLista, 0) < 0 Then Return 0
        End If

        If Lista > 0 Then
            Precio = HallaPrecioArticuloProveedor(Lista, Articulo, KilosXUnidad, PorUnidadEnLista, FinalEnLista)
        End If

        Return Precio

    End Function
    Public Function HallaSucursalLote(ByVal Lote As Integer, ByVal ConexionAux As String) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionAux)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Sucursal FROM IngresoMercaderiasCabeza WHERE Lote = " & Lote & ";", Miconexion)
                    Return Cmd.ExecuteScalar
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: IngresoMercaderiasCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaImportesLotesRecibos(ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Boolean

        Dim Dt As New DataTable

        Dim SqlTipoNota = "(C.TipoNota = 5 OR C.TipoNota = 13005 OR C.TipoNota = 6 OR C.TipoNota = 13006 OR C.TipoNota = 7 OR C.TipoNota = 13007 OR C.TipoNota = 8 OR C.TipoNota = 13008 OR C.TipoNota = 50 OR C.TipoNota = 70 OR C.TipoNota = 500 OR C.TipoNota = 700)"

        Dim SqlB As String = "SELECT 1 AS Operacion,C.TipoNota,C.Nota,L.ImporteConIva,L.ImporteSinIva FROM RecibosCabeza As C INNER JOIN RecibosLotes AS L ON C.TipoNota = L.TipoNota AND C.Nota = L.Nota WHERE C.Estado <> 3 AND " & SqlTipoNota & " AND L.Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion,C.TipoNota,C.Nota,L.ImporteConIva,L.ImporteSinIva FROM RecibosCabeza As C INNER JOIN RecibosLotes AS L ON C.TipoNota = L.TipoNota AND C.Nota = L.Nota WHERE C.Estado <> 3 AND " & SqlTipoNota & " AND L.Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Return False
        End If

        GListaLotesDeRecibos = New List(Of ItemCostosAsignados)

        ImporteConIva = 0
        ImporteSinIva = 0

        For Each Row As DataRow In Dt.Rows
            Select Case Row("TipoNota")
                Case 5, 13005, 6, 13006, 70, 700
                    Row("ImporteConIva") = -Row("ImporteConIva")
                    Row("ImporteSinIva") = -Row("ImporteSinIva")
            End Select
            ImporteConIva = ImporteConIva + Row("ImporteConIva")
            ImporteSinIva = ImporteSinIva + Row("ImporteSinIva")
            If GConListaDeCostos Then
                Dim Item As New ItemCostosAsignados
                Item.Operacion = Row("Operacion")
                Item.Nombre = "Nota Cred/Deb."
                Item.Comprobante = NumeroEditado(Row("Nota"))
                Item.TipoComprobante = Row("TipoNota")
                Item.NroComprobante = Row("Nota")
                Item.ImporteConIva = Row("ImporteConIva")
                Item.ImporteSinIva = Row("ImporteSinIva")
                GListaLotesDeRecibos.Add(Item)
            End If
        Next

        Dt.Dispose()
        Return True

    End Function
    Public Function HallaImportesLotesReintegros(ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Boolean

        Dim Dt As New DataTable

        Dim SqlB As String = "SELECT 1 AS Operacion,C.Nota,L.ImporteConIva,L.ImporteSinIva FROM ReintegrosCabeza As C INNER JOIN ReintegrosLotes AS L ON C.Nota = L.Nota WHERE C.Estado <> 3 AND L.Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Return False

        ImporteConIva = 0
        ImporteSinIva = 0

        For Each Row As DataRow In Dt.Rows
            ImporteConIva = ImporteConIva + Row("ImporteConIva")
            ImporteSinIva = ImporteSinIva + Row("ImporteSinIva")
            If GConListaDeCostos Then
                Dim Item As New ItemCostosAsignados
                Item.Operacion = Row("Operacion")
                Item.Nombre = "Reintegro Aduana"
                Item.Comprobante = NumeroEditado(Row("Nota"))
                Item.ImporteConIva = Row("ImporteConIva")
                Item.ImporteSinIva = Row("ImporteSinIva")
                GListaLotesDeReintegros.Add(Item)
            End If
        Next

        Dt.Dispose()
        Return True

    End Function
    Public Function HallaImportesLotesConsumosTermindos(ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef Importe As Decimal, ByRef Cantidad As Decimal) As Boolean

        Dim Dt As New DataTable
        Importe = 0

        Dim SqlB As String = "SELECT 1 AS Operacion,C.Consumo,L.Importe,L.Cantidad FROM ConsumosPTCabeza As C INNER JOIN ConsumosPTLotes AS L ON C.Consumo = L.Consumo WHERE C.Estado <> 3 AND L.Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion,C.Consumo,L.Importe,L.Cantidad FROM ConsumosPTCabeza As C INNER JOIN ConsumosPTLotes AS L ON C.Consumo = L.Consumo WHERE C.Estado <> 3 AND L.Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Return False
        End If

        For Each Row As DataRow In Dt.Rows
            Importe = Importe + Row("Importe")
            Cantidad = Cantidad + Row("Cantidad")
            If GConListaDeVentas Then
                Dim Item As New ItemCostosAsignados
                Item.Operacion = Row("Operacion")
                Item.Nombre = "Consumos Terminados"
                Item.Comprobante = NumeroEditado(Row("Consumo"))
                Item.ImporteConIva = Row("Importe")
                Item.ImporteSinIva = Row("Importe")
                Item.Cantidad = Row("Cantidad")
                GListaLotesDeConsumosTerminados.Add(Item)
            End If
        Next

        Dt.Dispose()

    End Function
    Public Function HallaImportesLotesOtrasFacturas(ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Boolean

        Dim Dt As New DataTable

        Dim SqlB As String = "SELECT 1 AS Operacion,C.Factura,L.ImporteConIva,L.ImporteSinIva FROM OtrasFacturasCabeza As C INNER JOIN OtrasFacturasLotes AS L ON C.Factura = L.Factura WHERE C.Estado <> 3 AND L.Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion,C.Factura,L.ImporteConIva,L.ImporteSinIva FROM OtrasFacturasCabeza As C INNER JOIN OtrasFacturasLotes AS L ON C.Factura = L.Factura WHERE C.Estado <> 3 AND L.Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Return False
        End If

        GListaLotesDeOtrasFacturas = New List(Of ItemCostosAsignados)

        ImporteConIva = 0
        ImporteSinIva = 0

        For Each Row As DataRow In Dt.Rows
            ImporteConIva = ImporteConIva + Row("ImporteConIva")
            ImporteSinIva = ImporteSinIva + Row("ImporteSinIva")
            If GConListaDeCostos Then
                Dim Item As New ItemCostosAsignados
                Item.Operacion = Row("Operacion")
                Item.Nombre = "Otras Facturas"
                Item.Comprobante = NumeroEditado(Row("Factura"))
                Item.ImporteConIva = Row("ImporteConIva")
                Item.ImporteSinIva = Row("ImporteSinIva")
                GListaLotesDeOtrasFacturas.Add(Item)
            End If
        Next

        Dt.Dispose()
        Return True

    End Function
    Public Function HallaImportesLotesAsientosManuales(ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Boolean

        Dim Dt As New DataTable

        Dim SqlB As String = "SELECT 1 AS Operacion,C.Asiento,C.Debito,C.Credito,L.ImporteConIva,L.ImporteSinIva FROM AsientosCabeza As C INNER JOIN AsientosLotes AS L ON C.Asiento = L.Asiento WHERE C.Estado <> 3 AND L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion,C.Asiento,C.Debito,C.Credito,L.ImporteConIva,L.ImporteSinIva FROM AsientosCabeza As C INNER JOIN AsientosLotes AS L ON C.Asiento = L.Asiento WHERE C.Estado <> 3 AND L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & ";"

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Return False
        End If

        GListaLotesDeAsientosManuales = New List(Of ItemCostosAsignados)

        ImporteConIva = 0
        ImporteSinIva = 0

        For Each Row As DataRow In Dt.Rows
            If Row("Credito") Then
                Row("ImporteConIva") = -Row("ImporteConIva")
                Row("ImporteSinIva") = -Row("ImporteSinIva")
            End If
            ImporteConIva = ImporteConIva + Row("ImporteConIva")
            ImporteSinIva = ImporteSinIva + Row("ImporteSinIva")
            If GConListaDeCostos Then
                Dim Item As New ItemCostosAsignados
                Item.Operacion = Row("Operacion")
                Item.Nombre = "Asientos Manuales"
                Item.Comprobante = NumeroEditado(Row("Asiento"))
                Item.ImporteConIva = Row("ImporteConIva")
                Item.ImporteSinIva = Row("ImporteSinIva")
                GListaLotesDeAsientosManuales.Add(Item)
            End If
        Next

        Dt.Dispose()
        Return True

    End Function
    Public Function HallaImporteMermasNVLP(ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Boolean

        Dim Dt As New DataTable

        Dim SqlB As String = ""
        Dim SqlN As String = ""

        GListaMermaPositivasNVLP = New List(Of ItemCostosAsignados)

        ImporteConIva = 0
        ImporteSinIva = 0

        SqlB = "SELECT 1 AS Operacion,C.Liquidacion,L.MermaAux,L.Cantidad,L.NetoConIva,L.NetoSinIva FROM NVLPCabeza As C INNER JOIN NVLPLotes AS L ON C.Liquidacion = L.Liquidacion WHERE C.Estado <> 3 AND MermaAux < 0 AND L.Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"
        SqlN = "SELECT 2 AS Operacion,C.Liquidacion,L.MermaAux,L.Cantidad,L.NetoConIva,L.NetoSinIva FROM NVLPCabeza As C INNER JOIN NVLPLotes AS L ON C.Liquidacion = L.Liquidacion WHERE C.Estado <> 3 AND MermaAux < 0 AND L.Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Return False
        End If

        Dim ImporteMermaConIva As Decimal = 0
        Dim ImporteMermaSinIva As Decimal = 0
        Dim MermaAux As Decimal

        For Each Row As DataRow In Dt.Rows
            MermaAux = -Row("MermaAux")
            ImporteMermaConIva = Trunca(MermaAux * Row("NetoConIva") / (Row("Cantidad") + MermaAux))
            ImporteMermaSinIva = Trunca(MermaAux * Row("NetoSinIva") / (Row("Cantidad") + MermaAux))
            ImporteConIva = ImporteConIva + ImporteMermaConIva
            ImporteSinIva = ImporteSinIva + ImporteMermaSinIva
            If GConListaDeCostos Then
                Dim Item As New ItemCostosAsignados
                Item.Operacion = Row("Operacion")
                Item.Nombre = "NVLP Merma Positiva"
                Item.Comprobante = NumeroEditado(Row("Liquidacion"))
                Item.ImporteConIva = ImporteMermaConIva
                Item.ImporteSinIva = ImporteMermaSinIva
                GListaMermaPositivasNVLP.Add(Item)
            End If
        Next

        Dt.Dispose()
        Return True

    End Function
    Private Function HallaCostosDelLote(ByVal Operacion As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Proveedor As Integer, ByVal Articulo As Integer, ByVal Fecha As Date, ByVal Cantidad As Decimal, ByVal Baja As Decimal, ByVal KilosXUnidad As Decimal, ByVal EsCosteo As Boolean, ByRef CostoConIva As Decimal, ByRef CostoSinIva As Decimal) As Boolean

        'costos asignados..........................................
        Dim CostoAsignadoConIva As Decimal = 0
        Dim CostoAsignadoSinIva As Decimal = 0
        If Not FacturaAfectaLotes(Lote, Secuencia, CostoAsignadoConIva, CostoAsignadoSinIva) Then Return False
        '
        'costos por insumos........................................
        Dim InsumosConIva As Decimal = 0
        Dim InsumosSinIva As Decimal = 0
        If HallaCostoInsumosPorLote(Lote, Secuencia, InsumosConIva, InsumosSinIva) < 0 Then Return False
        '
        'costos del Costeo.........................................
        Dim CostoDelCosteoConIva As Decimal = 0
        Dim CostoDelCosteoSinIva As Decimal = 0
        If EsCosteo Then
            If Not HallaCostoCosteoXKilo(Operacion, Lote, CostoDelCosteoConIva, CostoDelCosteoSinIva) Then Return False
            CostoDelCosteoConIva = CostoDelCosteoConIva * (Cantidad - Baja) * KilosXUnidad
            CostoDelCosteoSinIva = CostoDelCosteoSinIva * (Cantidad - Baja) * KilosXUnidad
        End If

        CostoConIva = CostoAsignadoConIva + InsumosConIva + CostoDelCosteoConIva
        CostoSinIva = CostoAsignadoSinIva + InsumosSinIva + CostoDelCosteoSinIva

        Return True

    End Function
    Public Function CantidadRemitida(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Deposito As Integer, ByVal ConexionStr As String) As Double

        Dim StrLote As String = "Lote = " & Lote & " AND Secuencia = " & Secuencia & " AND Deposito = " & Deposito
        Dim StrRecibos As String = "SELECT SUM(Cantidad) FROM AsignacionLotes WHERE Liquidado = 0 AND Facturado = 0 AND TipoComprobante = 1 AND " & StrLote & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(StrRecibos, Miconexion)
                    Dim Resul = Cmd.ExecuteScalar()
                    If Not IsDBNull(Resul) Then
                        Return CDbl(Resul)
                    Else : Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function ArmaLista(ByVal Lista As List(Of Vigencia), ByVal Codigo As Integer, ByVal Origen As Integer) As Boolean

        Dim Dt As New DataTable

        Lista.Clear()

        If Not Tablas.Read("SELECT * FROM Vigencias WHERE Codigo = " & Codigo & " AND Origen = " & Origen & ";", Conexion, Dt) Then Return False
        If Dt.Rows.Count = 0 Then Dt.Dispose() : Dt.Dispose() : Return True

        For Each Row As DataRow In Dt.Rows
            Dim Fila As New Vigencia
            Fila.Fecha = Row("Fecha")
            Fila.Valor = Row("Valor")
            If GTipoIva = 2 Then
                Fila.Alicuota = 0
            Else
                Fila.Alicuota = Row("Alicuota")
            End If
            Lista.Add(Fila)
        Next

        Dt.Dispose()

        Return True

    End Function
    Public Function CalculaCostoEnvase(ByVal Articulo As Integer, ByVal Fecha As Date, ByRef CostoConIva As Double, ByRef CostoSinIva As Double) As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Envase,Secundario,CantidadPrimarios FROM Articulos WHERE Clave = " & Articulo & ";", Conexion, Dt) Then
            MsgBox("ERROR al Leer Articulos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Not BuscaVigenciaValorAlicuota(12, Fecha, CostoConIva, CostoSinIva, Dt.Rows(0).Item("Envase")) Then Return False
        If Dt.Rows(0).Item("Secundario") = 0 Then Dt.Dispose() : Return True

        'Analiza secundario
        Dim CostoConIvaW As Double = 0
        Dim CostoSinIvaW As Double = 0
        If Not BuscaVigenciaValorAlicuota(12, Fecha, CostoConIvaW, CostoSinIvaW, Dt.Rows(0).Item("Secundario")) Then Return False
        CostoConIva = CostoConIva + Trunca(CostoConIvaW / Dt.Rows(0).Item("CantidadPrimarios"))
        CostoSinIva = CostoSinIva + Trunca(CostoSinIvaW / Dt.Rows(0).Item("CantidadPrimarios"))

        Dt.Dispose()

        Return True

    End Function
    Public Function CalculaFlete(ByVal ListaFletePorBulto As List(Of Vigencia), ByVal ListaFletePorMedioBulto As List(Of Vigencia), ByVal ListaFletePorUnidad As List(Of Vigencia), ByVal ListaFletePorKilo As List(Of Vigencia), ByVal Articulo As Integer, ByVal Fecha As Date, ByRef FleteConIva As Double, ByRef FleteSinIva As Double) As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT E.CalculoFlete,A.Secundario,A.CantidadPrimarios FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave WHERE A.Clave = " & Articulo & ";", Conexion, Dt) Then
            MsgBox("ERROR al Leer Articulos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Select Case Dt.Rows(0).Item("CalculoFlete")
            Case Bulto
                FleteSinIva = CalculaNeto(1, EncuentraVigencia(ListaFletePorBulto, Fecha))
                FleteConIva = FleteSinIva + CalculaIva(1, FleteSinIva, EncuentraVigenciaAlicuota(ListaFletePorBulto, Fecha))
            Case MedioBulto
                FleteSinIva = CalculaNeto(1, EncuentraVigencia(ListaFletePorMedioBulto, Fecha))
                FleteConIva = FleteSinIva + CalculaIva(1, FleteSinIva, EncuentraVigenciaAlicuota(ListaFletePorMedioBulto, Fecha))
            Case Unidad
                FleteSinIva = CalculaNeto(1, EncuentraVigencia(ListaFletePorUnidad, Fecha))
                FleteConIva = FleteSinIva + CalculaIva(1, FleteSinIva, EncuentraVigenciaAlicuota(ListaFletePorUnidad, Fecha))
            Case Kilo
                FleteSinIva = CalculaNeto(1, EncuentraVigencia(ListaFletePorKilo, Fecha))
                FleteConIva = FleteSinIva + CalculaIva(1, FleteSinIva, EncuentraVigenciaAlicuota(ListaFletePorKilo, Fecha))
        End Select

        If Dt.Rows(0).Item("Secundario") = 0 Then Dt.Dispose() : Return True

        FleteSinIva = FleteSinIva / Dt.Rows(0).Item("CantidadPrimarios")
        FleteConIva = FleteConIva / Dt.Rows(0).Item("CantidadPrimarios")

        Dt.Dispose()

        Return True

    End Function
    Public Function CalculaCosto(ByVal Articulo As Integer, ByVal Fecha As Date) As Double

        Dim Envase As Integer = HallaEnvase(Articulo)
        If Envase <= 0 Then
            MsgBox("ERROR al Leer Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return -1
        End If
        Dim Costo As Double = 0
        If Not BuscaVigencia(12, Fecha, Costo, Envase) Then Return -1
        Return Costo

    End Function
    Public Function CalculaSenia(ByVal Envase As Integer, ByVal Fecha As Date) As Double

        Dim Dueño As Integer = HallaDueño(Envase)
        If Dueño = 0 Then
            MsgBox("ERROR al Leer Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return -1
        End If

        If Dueño <> 2 Then Return 0

        Dim Senia As Double = 0
        If Not BuscaVigencia(10, Fecha, Senia, Envase) Then Return -1
        Return Senia

    End Function
    Public Function EncuentraVigencia(ByVal Lista As List(Of Vigencia), ByVal Fecha As Date) As Double

        Dim FechaW As Date = Format(Fecha, "dd/MM/yyyy")

        If Lista.Count = 0 Then Return 0

        For I As Integer = Lista.Count - 1 To 0 Step -1
            If DiferenciaDias(FechaW, Lista.Item(I).Fecha) <= 0 Then
                Return Lista.Item(I).Valor
            End If
        Next

    End Function
    Public Function EncuentraVigenciaAlicuota(ByVal Lista As List(Of Vigencia), ByVal Fecha As Date) As Double

        Dim FechaW As Date = Format(Fecha, "dd/MM/yyyy")

        If Lista.Count = 0 Then Return 0

        For I As Integer = Lista.Count - 1 To 0 Step -1
            If DiferenciaDias(FechaW, Lista.Item(I).Fecha) <= 0 Then
                Return Lista.Item(I).Alicuota
            End If
        Next

    End Function
    Public Function BuscaVigencia(ByVal Codigo As Integer, ByVal Fecha As Date, ByRef Valor As Double, ByVal Origen As Integer) As Boolean

        Dim Dt As New DataTable
        Dim FechaW As Date = Format(Fecha, "dd/MM/yyyy")
        Valor = 0

        If Not Tablas.Read("SELECT *,CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) FROM Vigencias WHERE Codigo = " & Codigo & " AND Origen = " & Origen & ";", Conexion, Dt) Then Return False
        If Dt.Rows.Count = 0 Then Dt.Dispose() : Return True

        For i As Integer = Dt.Rows.Count - 1 To 0 Step -1
            If DiferenciaDias(FechaW, Dt.Rows(i).Item("Fecha")) <= 0 Then
                Valor = Dt.Rows(i).Item("valor")
                Dt.Dispose()
                Return True
            End If
        Next

        Dt.Dispose()
        Return True

    End Function
    Public Function BuscaVigenciaValorAlicuota(ByVal Codigo As Integer, ByVal Fecha As Date, ByRef ValorConIva As Double, ByRef ValorSinIva As Double, ByVal Origen As Integer) As Boolean

        Dim Dt As New DataTable
        Dim FechaW As Date = Format(Fecha, "dd/MM/yyyy")
        ValorConIva = 0
        ValorSinIva = 0

        If Not Tablas.Read("SELECT *,CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) FROM Vigencias WHERE Codigo = " & Codigo & " AND Origen = " & Origen & ";", Conexion, Dt) Then Return False
        If Dt.Rows.Count = 0 Then Dt.Dispose() : Return True

        For i As Integer = Dt.Rows.Count - 1 To 0 Step -1
            If DiferenciaDias(FechaW, Dt.Rows(i).Item("Fecha")) <= 0 Then
                ValorConIva = Dt.Rows(i).Item("valor") + CalculaIva(1, Dt.Rows(i).Item("valor"), Dt.Rows(i).Item("Alicuota"))
                ValorSinIva = Dt.Rows(i).Item("valor")
                Dt.Dispose()
                Return True
            End If
        Next

        If GTipoIva = 2 Then ValorConIva = ValorSinIva

        Dt.Dispose()

        Return True

    End Function
    Public Function EsConsignacion(ByVal Proveedor As Integer) As Boolean


        Dim Miconexion As New OleDb.OleDbConnection(Conexion)

        Try
            Miconexion.Open()
            Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Clave) FROM Proveedores WHERE Clave = " & Proveedor & " AND TipoOperacion= 1;", Miconexion)
                If Cmd.ExecuteScalar() <> 0 Then Return True
            End Using
        Catch ex As Exception
            Return False
        Finally
            If Miconexion.State = ConnectionState.Open Then Miconexion.Close()
        End Try

    End Function
    Public Function HallaComisionAdicional(ByVal Proveedor As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ComisionAdicional FROM Proveedores WHERE Clave = " & Proveedor & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Function HallaComision(ByVal Proveedor As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Comision FROM Proveedores WHERE Clave = " & Proveedor & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Function HallaProveedorRemitoFacturaP(ByVal Proveedor As Integer, ByVal Remito As Double) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Remito) FROM FacturasProveedorCabeza WHERE Estado = 1 AND Proveedor = " & Proveedor & " AND Remito = " & Remito & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Function HallaProveedorGuiaFacturaP(ByVal Proveedor As Integer, ByVal Guia As Double) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Guia) FROM FacturasProveedorCabeza WHERE Estado = 1 AND Proveedor = " & Proveedor & " AND Guia = " & Guia & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Function HallaProveedorOrdenCompraFacturaP(ByVal Proveedor As Integer, ByVal Orden As Double, ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(OrdenCompra) FROM FacturasProveedorCabeza WHERE Estado = 1 AND Proveedor = " & Proveedor & " AND OrdenCompra = " & Orden & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Sub BorraGrid(ByVal Grid As DataGridView)

        Do Until Grid.Rows.Count = 0
            Grid.Rows.RemoveAt(Grid.Rows.Count - 1)
        Loop

    End Sub
    Public Function HallaPuntoVenta(ByVal Numero As Double) As Integer

        Return CInt(Strings.Left(Numero.ToString, Numero.ToString.Length - 8))

    End Function
    Public Sub LlenaComboTablas(ByVal Combo As ComboBox, ByVal Tipo As Integer)

        Dim Row As DataRow

        Combo.DataSource = Nothing
        Combo.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = " & Tipo & " ORDER BY Nombre;")
        If Tipo = 28 Then
            Row = Combo.DataSource.NewRow()
            Row("Clave") = 1
            Row("Nombre") = "ARGENTINA"
            Combo.DataSource.Rows.Add(Row)
        End If
        Row = Combo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Combo.DataSource.Rows.Add(Row)
        Combo.DisplayMember = "Nombre"
        Combo.ValueMember = "Clave"

    End Sub
    Public Sub LlenaCombo(ByVal Combo As ComboBox, ByVal Opcion As String, ByVal Archivo As String)

        Dim Row As DataRow

        Combo.DataSource = Nothing

        Select Case Archivo
            Case "Articulos"
                Combo.DataSource = TodosLosArticulos()
            Case "Negocios"
                Combo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;")
            Case "Clientes"
                Combo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes WHERE DeOperacion = 0 ORDER BY Nombre;")
            Case "ClientesNacionales"
                Combo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes WHERE DeOperacion = 0 AND TipoIva <> 4 ORDER BY Nombre;")
            Case "ProveedoresNacionales"
                Combo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM proveedores WHERE TipoOperacion <> 4 AND TipoIva <> 4 ORDER BY Nombre;")
            Case Else
                Combo.DataSource = Tablas.Leer("Select Clave,Nombre From " & Archivo & " ORDER BY Nombre;")
        End Select
        Row = Combo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = Opcion
        Combo.DataSource.Rows.Add(Row)
        Combo.DisplayMember = "Nombre"
        Combo.ValueMember = "Clave"

    End Sub
    Public Sub LlenaComboProveedor(ByVal Combo As ComboBox, ByVal Opcion As String)

        Dim Row As DataRow
        Combo.DataSource = Tablas.Leer("Select Clave,Nombre From Proveedores;")
        Row = Combo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = Opcion
        Combo.DataSource.Rows.Add(Row)
        Combo.DisplayMember = "Nombre"
        Combo.ValueMember = "Clave"

    End Sub
    Public Function UltimaNumeracionFactura(ByVal TipoFactura As Integer, ByVal PuntoDeVenta As Integer) As Double

        Dim Numeracion As Double
        Dim Docu As String = ""

        If TipoFactura = 1 Then Docu = "FacturasA"
        If TipoFactura = 2 Then Docu = "FacturasB"
        If TipoFactura = 3 Then Docu = "FacturasC"
        If TipoFactura = 4 Then Docu = "FacturasE"
        If TipoFactura = 5 Then Docu = "FacturasM"
        If TipoFactura = 6 Then Docu = "FacturasZ"

        Dim Sql As String = "SELECT " & Docu & " FROM PuntosDeVenta WHERE Clave = " & PuntoDeVenta & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Numeracion = Cmd.ExecuteScalar()
                End Using
            End Using
            Return CDbl(TipoFactura & Format(PuntoDeVenta, "0000") & Format(Numeracion + 1, "00000000"))
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Function UltimaNumeracionPagoYOrden(ByVal TipoNota As String, ByVal ConexionStr As String) As Double

        Dim Patron As String = GPuntoDeVenta & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Nota) FROM RecibosCabeza WHERE TipoNota = " & TipoNota & " AND CAST(CAST(RecibosCabeza.Nota AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(GPuntoDeVenta & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function UltimaNumeracionLiquidacion(ByVal TipoLiquidacion As Integer, ByVal PuntoDeVenta As Integer) As Double

        Dim Numeracion As Double
        Dim Docu As String = ""

        If TipoLiquidacion = 1 Then Docu = "LiquidacionA"
        If TipoLiquidacion = 2 Then Docu = "LiquidacionB"
        If TipoLiquidacion = 3 Then Docu = "LiquidacionC"
        If TipoLiquidacion = 4 Then Docu = "LiquidacionE"
        If TipoLiquidacion = 5 Then Docu = "LiquidacionM"

        Dim Sql As String = "SELECT " & Docu & " FROM PuntosDeVenta WHERE Clave = " & PuntoDeVenta & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Numeracion = Cmd.ExecuteScalar()
                End Using
            End Using
            Return CDbl(TipoLiquidacion & Format(PuntoDeVenta, "0000") & Format(Numeracion + 1, "00000000"))
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Function UltimaNumeracionNCredito(ByVal TipoFactura As Integer, ByVal PuntoDeVenta As Integer) As Double

        Dim Numeracion As Double
        Dim Docu As String = ""

        If TipoFactura = 1 Then Docu = "NotasCreditoA"
        If TipoFactura = 2 Then Docu = "NotasCreditoB"
        If TipoFactura = 3 Then Docu = "NotasCreditoC"
        If TipoFactura = 4 Then Docu = "NotasCreditoE"
        If TipoFactura = 5 Then Docu = "NotasCreditoM"
        If TipoFactura = 6 Then Docu = "NotasCreditoZ"

        Dim Sql As String = "SELECT " & Docu & " FROM PuntosDeVenta WHERE Clave = " & PuntoDeVenta & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Numeracion = Cmd.ExecuteScalar()
                End Using
            End Using
            Return CDbl(TipoFactura & Format(PuntoDeVenta, "0000") & Format(Numeracion + 1, "00000000"))
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Function UltimaNumeracionNDebito(ByVal TipoFactura As Integer, ByVal PuntoDeVenta As Integer) As Double

        Dim Numeracion As Double
        Dim Docu As String = ""

        If TipoFactura = 1 Then Docu = "NotasDebitoA"
        If TipoFactura = 2 Then Docu = "NotasDebitoB"
        If TipoFactura = 3 Then Docu = "NotasDebitoC"
        If TipoFactura = 4 Then Docu = "NotasDebitoE"
        If TipoFactura = 5 Then Docu = "NotasDebitoM"

        Dim Sql As String = "SELECT " & Docu & " FROM PuntosDeVenta WHERE Clave = " & PuntoDeVenta & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Numeracion = Cmd.ExecuteScalar()
                End Using
            End Using
            Return CDbl(TipoFactura & Format(PuntoDeVenta, "0000") & Format(Numeracion + 1, "00000000"))
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Function UltimaNumeracionPV(ByVal TipoDocumento As Integer, ByVal PuntoDeVenta As Integer) As Double

        Dim Docu As String = ""
        Dim Numeracion As Double

        If TipoDocumento = 1 Then Docu = "Remitos"
        If TipoDocumento = 2 Then Docu = "Facturas"
        If TipoDocumento = 4 Then Docu = "NotasCredito"
        If TipoDocumento = 7 Then Docu = "NotasCredito"
        If TipoDocumento = 5 Then Docu = "NotasDebito"
        If TipoDocumento = 10 Then Docu = "Liquidacion"
        If TipoDocumento = 11 Then Docu = "DevolucionMercaderia"

        Dim Sql As String = "SELECT " & Docu & " FROM PuntosDeVenta WHERE Clave = " & PuntoDeVenta & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Numeracion = Cmd.ExecuteScalar()
                End Using
            End Using
            Return CDbl(PuntoDeVenta & Format(Numeracion + 1, "00000000"))
        Catch ex As Exception
            MsgBox(ex.Message)
            Return 0
        Finally
        End Try

    End Function
    Public Sub NumeracionAlternativa(ByVal TipoDocumento As Integer, ByVal PuntoDeVenta As Integer, ByVal TipoFactura As Integer, ByVal NumeroViejo As Decimal, ByVal EsElectronica As Boolean, ByRef NumeroNuevo As Decimal)

        NumeroNuevo = 0
        Dim Numeracion As Decimal = 0

        If TipoDocumento = 2 Then
            Numeracion = UltimaNumeracionFactura(TipoFactura, PuntoDeVenta)
            If Numeracion = -1 Then Exit Sub
        End If
        If TipoDocumento = 1 Then
            Numeracion = UltimaNumeracionPV(TipoDocumento, PuntoDeVenta)
        End If
        If Numeracion = NumeroViejo Then Exit Sub
        If MsgBox("Numeración " & NumeroEditado(NumeroViejo) & " Fue Utilizada. Próxima Numeración es " & NumeroEditado(Numeracion) + vbCrLf + "Desea GRABARLA (Si/No)?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
            If TipoDocumento = 2 And EsElectronica And Numeracion > 1 Then
                If HallaCaeFactura(Numeracion - 1) = 0 Then
                    MsgBox("Factura Anterior a " & NumeroEditado(Numeracion) & " Es Electrónica No Autorizada por AFIP." + vbCrLf + "No se puede GRABAR.", MsgBoxStyle.Critical)
                    Exit Sub
                End If
            End If
            NumeroNuevo = Numeracion
        End If

    End Sub
    Public Function NumeroInternoRecibos(ByVal Letra As Integer, ByVal TipoNota As Integer, ByVal ConexionStr As String) As Double

        Select Case TipoNota
            Case 5, 6, 7, 8
                Return UltimoNumeroInternoDebitoCredito(Letra, TipoNota, ConexionStr)
            Case Else
                Return UltimoNumeroInternoRecibo(TipoNota, ConexionStr)
        End Select

    End Function
    Public Function UltimoNumeroInternoRecibo(ByVal TipoNota As Integer, ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Interno) FROM RecibosCabeza WHERE TipoNota = " & TipoNota & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(TipoNota & Format(1, "000000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function UltimoNumeroInternoDebitoCredito(ByVal Letra As Integer, ByVal TipoNota As Integer, ByVal ConexionStr As String) As Double

        Dim Patron As String = Letra & Format(TipoNota, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Interno) FROM RecibosCabeza WHERE TipoNota = " & TipoNota & " AND CAST(CAST(RecibosCabeza.Interno AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(Letra & Format(TipoNota, "0000") & Format(1, "000000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function NombreArticulo(ByVal Clave As Integer) As String

        Dim Dta As New DataTable
        Dim Nombre As String = ""

        Dta = Tablas.Leer("SELECT Nombre FROM Articulos WHERE Clave = " & Clave & ";")
        If Dta.Rows.Count <> 0 Then Nombre = Dta.Rows(0).Item("Nombre")
        Dta = Nothing
        Return Nombre

    End Function
    Public Function NombreArticuloServicios(ByVal Clave As Integer) As String

        Dim Dta As New DataTable
        Dim Nombre As String = ""

        Dta = Tablas.Leer("SELECT Nombre FROM ArticulosServicios WHERE Clave = " & Clave & ";")
        If Dta.Rows.Count <> 0 Then Nombre = Dta.Rows(0).Item("Nombre")
        Dta = Nothing
        Return Nombre

    End Function
    Public Function NombreArticuloLogistico(ByVal Clave As Integer) As String

        Dim Dta As New DataTable
        Dim Nombre As String = ""

        Dta = Tablas.Leer("SELECT Nombre FROM Tablas WHERE Tipo = 6 AND Clave = " & Clave & ";")
        If Dta.Rows.Count <> 0 Then Nombre = Dta.Rows(0).Item("Nombre")
        Dta = Nothing
        Return Nombre

    End Function
    Public Function ArticulosConStock(ByVal Deposito As Integer) As DataTable

        Dim Dt As New DataTable
        Dim Senia As Double = 0

        Dt = Tablas.Leer("SELECT A.Clave,A.Nombre,A.Envase,E.Kilos FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave;")

        For Each Row As DataRow In Dt.Rows
            If HallaStock(Row("Clave"), Deposito) = 0 Then
                Row.Delete()
            Else
                BuscaVigencia(10, Date.Now, Senia, Row("Envase"))
                Row("Nombre") = Row("Nombre").ToString.PadRight(30, " ") & " (S:" & FormatCurrency(Senia, GDecimales) & " Kg:" & Row("Kilos") & ")"
            End If
        Next

        ArticulosConStock = Dt

        Dt.Dispose()

    End Function
    Public Function TodosLosArticulos() As DataTable

        Dim Dt As New DataTable
        Dim Senia As Double = 0

        Dt = Tablas.Leer("SELECT A.Clave,A.Nombre,A.Envase,A.Estado,E.Kilos,E.Unidad FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave;")

        For Each Row As DataRow In Dt.Rows
            BuscaVigencia(10, Date.Now, Senia, Row("Envase"))
            Row("Nombre") = Row("Nombre") & " (S:" & FormatCurrency(Senia, GDecimales) & " " & Row("Unidad") & ":" & Row("Kilos") & ")"
        Next

        TodosLosArticulos = Dt

        Dt.Dispose()

    End Function
    Public Function HallaDatosArticulo(ByRef Dt As DataTable, ByVal Clave As Integer, ByVal SoloActivos As Boolean) As DataTable

        Dim Senia As Double = 0

        Dim Sql As String
        Sql = "A.Clave = " & Clave
        If SoloActivos Then
            Sql = Sql & " AND A.Estado = 1;"
        Else
            Sql = Sql & ";"
        End If

        Tablas.Read("SELECT A.Clave,A.Nombre,A.Envase,A.Estado,E.Kilos,E.Unidad FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave WHERE " & Sql, Conexion, Dt)
        Dim Row As DataRow = Dt.Rows(0)
        BuscaVigencia(10, Date.Now, Senia, Row("Envase"))
        Row("Nombre") = Row("Nombre") & " (S:" & FormatCurrency(Senia, GDecimales) & " " & Row("Unidad") & ":" & Row("Kilos") & ")"

    End Function
    Public Function ArticulosActivos() As DataTable

        Dim Dt As New DataTable
        Dim Senia As Double = 0

        Dt = Tablas.Leer("SELECT A.Clave,A.Nombre,A.Envase,E.Kilos,E.Unidad,E.Unidad FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave WHERE A.Estado = 1;")

        For Each Row As DataRow In Dt.Rows
            BuscaVigencia(10, Date.Now, Senia, Row("Envase"))
            Row("Nombre") = Row("Nombre") & " (S:" & FormatCurrency(Senia, GDecimales) & " " & Row("Unidad") & ":" & Row("Kilos") & ")"
        Next

        ArticulosActivos = Dt

        Dt.Dispose()

    End Function
    Public Sub LLenaComboEnvases(ByVal Combo As ComboBox)

        Dim Dt As New DataTable
        Dim Senia As Double = 0

        If Not Tablas.Read("Select Clave,Nombre,Kilos,Unidad FROM Envases;", Conexion, Dt) Then End
        Combo.DataSource = Dt
        Combo.DisplayMember = "Nombre"
        Combo.ValueMember = "Clave"
        For Each Row As DataRow In Combo.DataSource.Rows
            BuscaVigencia(10, Date.Now, Senia, Row("Clave"))
            Row("Nombre") = Row("Nombre").ToString.PadRight(15, " ") & " (S:" & FormatCurrency(Senia, GDecimales) & " " & Row("Unidad") & ":" & Row("Kilos") & ")"
        Next
        Dim Row1 As DataRow = Combo.DataSource.NewRow()
        Row1("Clave") = 0
        Row1("Nombre") = ""
        Row1("Kilos") = 0
        Combo.DataSource.Rows.Add(Row1)

    End Sub
    Public Function SeniaXArticulo(ByVal Nombre As String) As Double

        Dim ImporteStr As String = ""

        For I As Integer = Nombre.Length To 1 Step -1
            If InStr("0123456789.", Mid$(Nombre, I, 1)) <> 0 Then ImporteStr = Mid$(Nombre, I, 1) & ImporteStr
        Next

        If ImporteStr = "" Then
            Return 0
        Else
            Return CDbl(ImporteStr)
        End If

    End Function
    Public Function ProveedoresDeInsumos() As DataTable

        Dim dt As DataTable = New DataTable
        dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Try
            Tablas.Read("SELECT Clave,Nombre,Estado FROM Proveedores WHERE Producto = " & Insumo & " AND TipoOperacion <> 4 AND TipoIva <> " & Exterior & " ORDER BY Nombre;", Conexion, dt)
            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            dt.Rows.Add(Row)
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ProveedoresDeInsumosAlias() As DataTable

        Dim dt As DataTable = New DataTable
        dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Try
            Tablas.Read("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '' AND Producto = " & Insumo & " AND TipoOperacion <> 4 AND TipoIva <> " & Exterior & " ORDER BY Alias;", Conexion, dt)
            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Alias") = ""
            dt.Rows.Add(Row)
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ProveedoresDeFrutas() As DataTable

        Dim dt As DataTable = New DataTable

        Try
            Tablas.Read("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion <> 4 AND Producto = " & Fruta & " ORDER BY Nombre;", Conexion, dt)
            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            dt.Rows.Add(Row)
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ProveedoresDeFrutasAlias() As DataTable

        Dim dt As DataTable = New DataTable

        Try
            Tablas.Read("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '' AND TipoOperacion <> 4 AND Producto = " & Fruta & " ORDER BY Alias;", Conexion, dt)
            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Alias") = ""
            dt.Rows.Add(Row)
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ProveedoresDeFrutasNacional() As DataTable

        Dim dt As DataTable = New DataTable

        Try
            Tablas.Read("SELECT Clave,Nombre FROM Proveedores WHERE TipoIva <> 4 AND TipoOperacion <> 4 AND Producto = " & Fruta & " ORDER BY Nombre;", Conexion, dt)
            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            dt.Rows.Add(Row)
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ProveedoresDeFrutasAliasNacional() As DataTable

        Dim dt As DataTable = New DataTable

        Try
            Tablas.Read("SELECT Clave,Alias FROM Proveedores WHERE TipoIva <> 4 AND Alias <> '' AND TipoOperacion <> 4 AND Producto = " & Fruta & " ORDER BY Alias;", Conexion, dt)
            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Alias") = ""
            dt.Rows.Add(Row)
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ProveedoresDeFrutasYCosteo() As DataTable

        Dim dt As DataTable = New DataTable

        Try
            Tablas.Read("SELECT Clave,Nombre FROM Proveedores WHERE Producto = " & Fruta & " or TipoOperacion = 4 ORDER BY Nombre;", Conexion, dt)
            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            dt.Rows.Add(Row)
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ProveedoresTodos() As DataTable

        Dim dt As DataTable = New DataTable

        Try
            Tablas.Read("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion <> 4 ORDER BY Nombre;", Conexion, dt)
            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            dt.Rows.Add(Row)
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function ProveedoresTodosAlias() As DataTable

        Dim dt As DataTable = New DataTable

        Try
            Tablas.Read("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '' AND TipoOperacion <> 4 ORDER BY Alias;", Conexion, dt)
            Dim Row As DataRow = dt.NewRow
            Row("Clave") = 0
            Row("Alias") = ""
            dt.Rows.Add(Row)
            Return dt
        Catch ex As Exception
        Finally
            dt.Dispose()
        End Try

    End Function
    Public Function OrdenCompraCompleta(ByVal Orden As Double) As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Cantidad,Recibido FROM OrdenCompraDetalle WHERE Orden = " & Orden & ";", Conexion, Dt) Then Dt.Dispose() : Return False

        Dim CantidadOrdenada As Double = 0
        Dim CantidadRecibida As Double = 0
        For Each Row As DataRow In Dt.Rows
            CantidadOrdenada = CantidadOrdenada + Row("Cantidad")
            CantidadRecibida = CantidadRecibida + Row("Recibido")
        Next

        Dt.Dispose()

        If CantidadOrdenada <> CantidadRecibida Then Return False

        Return True

    End Function
    Public Function HallaImportesRemito(ByVal OrdenCompra As Double, ByVal Ingreso As Double, ByRef ImporteB As Double, ByRef ImporteN As Double, ByRef ImporteBSinIva As Double, ByVal Operacion As Integer) As Boolean

        Dim ConexionStr As String = ""
        Dim DtRemito As New DataTable
        Dim DtOrdenCompra As New DataTable
        Dim Sql As String = ""
        Dim RowsBusqueda() As DataRow

        ImporteB = 0
        ImporteN = 0
        ImporteBSinIva = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT Articulo,(Cantidad - Devueltas) AS Cantidad FROM IngresoInsumoDetalle WHERE Ingreso = " & Ingreso & ";", ConexionStr, DtRemito) Then Return False

        If Not Tablas.Read("SELECT C.Orden,D.Articulo,D.Precio,D.Iva,C.Rel FROM OrdenCompraCabeza AS C INNER JOIN OrdenCompraDetalle AS D ON C.Orden = D.Orden WHERE C.Orden = " & OrdenCompra & ";", Conexion, DtOrdenCompra) Then Return False
        For Each Row As DataRow In DtOrdenCompra.Rows
            RowsBusqueda = DtRemito.Select("Articulo = " & Row("Articulo"))
            If RowsBusqueda.Length <> 0 Then
                If Operacion = 1 Then
                    ImporteB = ImporteB + Trunca(CalculaNeto(RowsBusqueda(0).Item("Cantidad"), Row("Precio")) + CalculaIva(RowsBusqueda(0).Item("Cantidad"), Row("Precio"), Row("Iva")))
                    ImporteBSinIva = ImporteBSinIva + CalculaNeto(RowsBusqueda(0).Item("Cantidad"), Row("Precio"))
                Else
                    ImporteN = ImporteN + CalculaNeto(RowsBusqueda(0).Item("Cantidad"), Row("Precio"))
                End If
            End If
        Next

        DtRemito.Dispose()
        DtOrdenCompra.Dispose()

        Return True

    End Function
    Public Function Consignatarios() As DataTable

        Dim dt As DataTable = New DataTable

        dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Dim Row As DataRow

        Row = dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        dt.Rows.Add(Row)

        Tablas.Read("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 1 ORDER BY Nombre;", Conexion, dt)
        Consignatarios = dt

        dt.Dispose()

    End Function
    Public Function HallaStock(ByVal Articulo As Integer, ByVal Deposito As Integer) As Decimal

        Dim Miconexion As New OleDb.OleDbConnection(Conexion)
        Dim Stock As Decimal = 0

        Try
            Miconexion.Open()
            Using Cmd As New OleDb.OleDbCommand("SELECT SUM(Stock) FROM Lotes WHERE Articulo = " & Articulo & " AND Deposito= " & Deposito & ";", Miconexion)
                Dim Resul = Cmd.ExecuteScalar()
                If Not IsDBNull(Resul) Then Stock = CDec(Resul)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            Return 0
        Finally
            If Miconexion.State = ConnectionState.Open Then Miconexion.Close()
        End Try

        If PermisoTotal Then
            Miconexion = New OleDb.OleDbConnection(ConexionN)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT SUM(Stock) FROM Lotes WHERE Articulo = " & Articulo & " AND Deposito= " & Deposito & ";", Miconexion)
                    Dim Resul = Cmd.ExecuteScalar()
                    If Not IsDBNull(Resul) Then Stock = Stock + CDec(Resul)
                End Using
            Catch ex As Exception
                Return 0
            Finally
                If Miconexion.State = ConnectionState.Open Then Miconexion.Close()
            End Try
        End If

        Return Stock

    End Function
    Public Function HallaStockLote(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Deposito As Integer, ByVal ConexionLote As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionLote)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Stock FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & " AND Deposito= " & Deposito & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function NombreDeposito(ByVal Clave As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Tipo = 19 AND Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return ""
        End Try

    End Function
    Public Function NombreDepositoInsumos(ByVal Clave As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Tipo = 20 AND Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return ""
        End Try

    End Function
    Public Function HallaImporteNotasCredito(ByVal Factura As Double, ByVal ConexionFactura As String) As Double

        Dim Miconexion As New OleDb.OleDbConnection(ConexionFactura)
        Dim Importe As Double = 0

        Try
            Miconexion.Open()
            Using Cmd As New OleDb.OleDbCommand("SELECT SUM(Importe) FROM NotasCreditoCabeza WHERE Factura = " & Factura & ";", Miconexion)
                Dim Resul = Cmd.ExecuteScalar()
                If Not IsDBNull(Resul) Then Importe = Resul
            End Using
            Return Importe
        Catch ex As Exception
            MsgBox(ex.Message)
            Return -1
        Finally
            If Miconexion.State = ConnectionState.Open Then Miconexion.Close()
        End Try

    End Function
    Public Function HallaImporteRecibosFactura(ByVal Factura As Double, ByVal ConexionFactura As String, ByVal Tipo As Integer) As Double

        Dim Miconexion As New OleDb.OleDbConnection(ConexionFactura)
        Dim Importe As Double = 0

        Try
            Miconexion.Open()
            Using Cmd As New OleDb.OleDbCommand("SELECT SUM(Importe) FROM RecibosDetalle WHERE Comprobante = " & Factura & " AND Tipo = " & Tipo & ";", Miconexion)
                Dim Resul = Cmd.ExecuteScalar()
                If Not IsDBNull(Resul) Then Importe = Resul
            End Using
            Return Importe
        Catch ex As Exception
            MsgBox(ex.Message)
            Return -1
        Finally
            If Miconexion.State = ConnectionState.Open Then Miconexion.Close()
        End Try

    End Function
    Public Function NombreCliente(ByVal Clave As Integer) As String

        Dim Dt As New DataTable
        Dim Nombre As String = ""

        If Not Tablas.Read("SELECT Nombre FROM Clientes WHERE Clave = " & Clave & ";", Conexion, Dt) Then Return ""
        If Dt.Rows.Count <> 0 Then Nombre = Dt.Rows(0).Item("Nombre")
        Dt = Nothing

        Return Nombre

    End Function
    Public Function NombreProveedor(ByVal Clave As Integer) As String

        Dim Dt As New DataTable
        Dim Nombre As String = ""

        If Not Tablas.Read("SELECT Nombre FROM Proveedores WHERE Clave = " & Clave & ";", Conexion, Dt) Then Return ""
        If Dt.Rows.Count <> 0 Then Nombre = Dt.Rows(0).Item("Nombre")
        Dt = Nothing

        Return Nombre

    End Function
    Public Function NombreVendedor(ByVal Clave As Integer) As String

        Dim Dt As New DataTable
        Dim Nombre As String = ""

        If Not Tablas.Read("SELECT Nombre FROM Tablas WHERE Tipo = 37 AND Clave = " & Clave & ";", Conexion, Dt) Then Return ""
        If Dt.Rows.Count <> 0 Then Nombre = Dt.Rows(0).Item("Nombre")
        Dt = Nothing

        Return Nombre

    End Function
    Public Function NombreProveedorFondoFijo(ByVal Clave As Integer) As String

        Dim Dt As New DataTable
        Dim Nombre As String = ""

        If Not Tablas.Read("SELECT Nombre FROM MaestroFondoFijo WHERE Clave = " & Clave & ";", Conexion, Dt) Then Return ""
        If Dt.Rows.Count <> 0 Then Nombre = Dt.Rows(0).Item("Nombre")
        Dt = Nothing

        Return Nombre

    End Function
    Public Function NombreBanco(ByVal Clave As Integer) As String

        Dim Dt As New DataTable
        Dim Nombre As String = ""

        If Not Tablas.Read("SELECT Nombre FROM Tablas WHERE Tipo = 26 AND Clave = " & Clave & ";", Conexion, Dt) Then Return ""
        If Dt.Rows.Count <> 0 Then Nombre = Dt.Rows(0).Item("Nombre")
        Dt = Nothing

        Return Nombre

    End Function
    Public Function NombreDestino(ByVal Clave As Integer) As String

        Dim Dt As New DataTable
        Dim Nombre As String = ""

        If Not Tablas.Read("SELECT Nombre FROM OtrosProveedores WHERE Clave = " & Clave & ";", Conexion, Dt) Then Return ""
        If Dt.Rows.Count <> 0 Then Nombre = Dt.Rows(0).Item("Nombre")
        Dt = Nothing

        Return Nombre

    End Function
    Public Function NombreCosteo(ByVal Costeo As Integer) As String

        Dim Dt As New DataTable
        Dim Nombre As String = ""

        If Not Tablas.Read("SELECT Nombre FROM Costeos WHERE Costeo = " & Costeo & ";", Conexion, Dt) Then Return ""
        If Dt.Rows.Count <> 0 Then Nombre = Dt.Rows(0).Item("Nombre")
        Dt = Nothing

        Return Nombre

    End Function
    Public Function NombreEnvase(ByVal Clave As Integer) As String

        Dim Dt As New DataTable
        Dim Nombre As String = ""

        If Not Tablas.Read("SELECT Nombre FROM Envases WHERE Clave = " & Clave & ";", Conexion, Dt) Then Return ""
        If Dt.Rows.Count <> 0 Then Nombre = Dt.Rows(0).Item("Nombre")
        Dt = Nothing

        Return Nombre

    End Function
    Public Function NombreAduana(ByVal Clave As Integer) As String

        Dim Dt As New DataTable
        Dim Nombre As String = ""

        If Not Tablas.Read("SELECT Nombre FROM ClientesAduana WHERE Clave = " & Clave & ";", Conexion, Dt) Then Return ""
        If Dt.Rows.Count <> 0 Then Nombre = Dt.Rows(0).Item("Nombre")
        Dt = Nothing

        Return Nombre

    End Function
    Public Function NombrePais(ByVal Pais As Integer) As String

        If Pais = 1 Then Return "Agentina"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Tipo = 28 AND Clave = " & Pais & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function LotesNotasCredito(ByVal Factura As Double, ByVal Abierto As Boolean, ByRef Lista As List(Of FilaAsignacion)) As Boolean

        Dim Dt As New DataTable
        Dim ConexionNotaCredito As String = ""

        Dim sql As String = "SELECT Indice,Lote,Secuencia,Cantidad FROM NotasCreditoCabeza INNER JOIN  NotasCreditoDetalle ON NotasCreditoCabeza.NotaCredito = NotasCreditoDetalle.NotaCredito WHERE NotasCreditoCabeza.Factura = " & Factura & " AND Lote <> 0;"
        If Abierto Then
            ConexionNotaCredito = Conexion
        Else : ConexionNotaCredito = ConexionN
        End If

        If Not Tablas.Read(sql, ConexionNotaCredito, Dt) Then Return False

        Lista = New List(Of FilaAsignacion)
        Dim Esta As Boolean

        For Each Row As DataRow In Dt.Rows
            Esta = False
            For Each Fila As FilaAsignacion In Lista
                If Fila.Indice = Row("Indice") And Fila.Lote = Row("Lote") And Fila.Secuencia = Row("Secuencia") Then
                    Fila.Devolucion = Fila.Devolucion + Row("Cantidad")
                    Esta = True
                    Exit For
                End If
            Next
            If Not Esta Then
                Dim Fila1 As New FilaAsignacion
                Fila1.Indice = Row("Indice")
                Fila1.Lote = Row("Lote")
                Fila1.Secuencia = Row("Secuencia")
                Fila1.Devolucion = Row("Cantidad")
                Lista.Add(Fila1)
            End If
        Next

        Return True

    End Function
    Public Function ArticulosNotasCredito(ByVal Factura As Double, ByVal Abierto As Boolean, ByRef Lista As List(Of FilaFactura)) As Boolean

        Dim Dt As New DataTable
        Dim ConexionNotaCredito As String = ""

        Dim sql As String = "SELECT Indice,Cantidad FROM NotasCreditoCabeza INNER JOIN  NotasCreditoDetalle ON NotasCreditoCabeza.NotaCredito = NotasCreditoDetalle.NotaCredito WHERE NotasCreditoCabeza.Factura = " & Factura & " AND Lote = 0;"
        If Abierto Then
            ConexionNotaCredito = Conexion
        Else : ConexionNotaCredito = ConexionN
        End If

        If Not Tablas.Read(sql, ConexionNotaCredito, Dt) Then Return False

        Lista = New List(Of FilaFactura)
        Dim Esta As Boolean

        For Each Row As DataRow In Dt.Rows
            Esta = False
            For Each Fila As FilaFactura In Lista
                If Fila.Indice = Row("Indice") Then
                    Fila.Cantidad = Fila.Cantidad + Row("Cantidad")
                    Esta = True
                    Exit For
                End If
            Next
            If Not Esta Then
                Dim Fila1 As New FilaFactura
                Fila1.Indice = Row("Indice")
                Fila1.Cantidad = Row("Cantidad")
                Lista.Add(Fila1)
            End If
        Next

        Return True

    End Function
    Public Function HallaOperacion(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Deposito As Integer, ByVal Coneccion As String) As Integer

        Dim Sql As String = "SELECT COUNT(Lote) FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & " AND Deposito = " & Deposito & ";"
        Dim Miconexion As New OleDb.OleDbConnection(Coneccion)
        Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)

        Try
            Miconexion.Open()
            Return Cmd.ExecuteScalar()
        Catch ex As Exception
            Return -1
        Finally
            If Miconexion.State = ConnectionState.Open Then Miconexion.Close()
            Miconexion.Dispose()
            Cmd.Dispose()
        End Try

    End Function
    Public Function HallaCantidadPrimaria(ByVal Articulo As Integer) As Decimal

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT CantidadPrimarios As CantidadPorBulto FROM Articulos WHERE Clave = " & Articulo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("ERROR DE BASE DE DATOS!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!")
            Return -1
        End Try

    End Function
    Public Function HallaKilosXUnidad(ByVal Articulo As Integer) As Double

        Dim Dt As New DataTable
        Dim kilos As Double = 0

        Dim SqlStr As String = "SELECT Kilos FROM Articulos,Envases Where Articulos.Clave= " & Articulo &
                     " AND Articulos.Envase = Envases.Clave;"
        Dt = Tablas.Leer(SqlStr)
        If Not Dt.Rows.Count = 0 Then kilos = Dt.Rows(0).Item("Kilos")
        Dt.Dispose()
        Return kilos

    End Function
    Public Sub HallaKilosXUnidadYUMedida(ByVal Articulo As Integer, ByRef KilosXUnidad As Decimal, ByRef UMedida As String)

        Dim Dt As New DataTable
        KilosXUnidad = 0
        UMedida = ""

        Dim SqlStr As String = "SELECT Kilos,Unidad FROM Articulos,Envases Where Articulos.Clave= " & Articulo &
                     " AND Articulos.Envase = Envases.Clave;"
        Dt = Tablas.Leer(SqlStr)
        If Not Dt.Rows.Count = 0 Then KilosXUnidad = Dt.Rows(0).Item("Kilos") : UMedida = Dt.Rows(0).Item("Unidad")
        Dt.Dispose()

    End Sub
    Public Sub HallaKilosIva(ByVal Articulo As Integer, ByRef Kilos As Double, ByRef Iva As Double)

        Dim Dt As New DataTable
        Kilos = 0
        Iva = 0

        Dim SqlStr As String = "SELECT Kilos,Iva FROM Articulos,Envases Where Articulos.Clave= " & Articulo &
                     " AND Articulos.Envase = Envases.Clave;"
        Dt = Tablas.Leer(SqlStr)
        If Not Dt.Rows.Count = 0 Then Kilos = Dt.Rows(0).Item("Kilos") : Iva = Dt.Rows(0).Item("Iva")
        Dt.Dispose()

        If GTipoIva = 2 Then Iva = 0

    End Sub
    Public Function HallaEnvase(ByVal Articulo As Integer) As Integer

        Dim Dt As New DataTable
        Dim Envase As Integer

        Dim Sql As String = "SELECT Envase FROM Articulos WHERE Articulos.Clave = " & Articulo & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then Return -1
        If Dt.Rows.Count = 0 Then Return -2
        Envase = Dt.Rows(0).Item("Envase")
        Dt.Dispose()

        Return Envase

    End Function
    Public Function HallaDueño(ByVal Envase As Integer) As Integer

        Dim Dt As New DataTable
        Dim Dueño As Integer = 0

        Dim Sql As String = "SELECT Dueño FROM Envases WHERE Clave = " & Envase & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then Return 0
        If Dt.Rows.Count = 0 Then Return 0
        Dueño = Dt.Rows(0).Item("Dueño")
        Dt.Dispose()

        Return Dueño

    End Function
    Public Function HallaIvaInsumo(ByVal Insumo As Integer) As Double

        If GTipoIva = 2 Then Return 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Iva FROM Insumos WHERE Clave = " & Insumo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Function HallaIvaEspecie(ByVal Especie As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Iva FROM Tablas WHERE Tipo = 1 AND Clave = " & Especie & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Function HallaIva(ByVal Articulo As Integer) As Double

        If GTipoIva = 2 Then Return 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Iva FROM Articulos WHERE Clave = " & Articulo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Function HallaIvaDeCodigo(ByVal Clave As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Iva FROM Tablas WHERE Tipo = 22 AND Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Public Function HallaPrecioSinIva(ByVal Articulo As Integer, ByVal Precio As Decimal) As Decimal

        Dim Iva As Decimal = HallaIva(Articulo)
        Return Trunca(Precio / (1 + Iva / 100))

    End Function
    Public Function HallaArticulo(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal ConexionLote As String) As Integer

        Dim Sql As String = "SELECT Articulo FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";"
        Dim Miconexion As New OleDb.OleDbConnection(ConexionLote)
        Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)

        Try
            Miconexion.Open()
            Return Cmd.ExecuteScalar()
        Catch ex As Exception
            Return -1
        Finally
            If Miconexion.State = ConnectionState.Open Then Miconexion.Close()
            Miconexion.Dispose()
            Cmd.Dispose()
        End Try

    End Function
    Public Function HallaNombreConcepto(ByVal Concepto As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Tipo = 34 AND UltimoNumero = " & Concepto & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return ""
        End Try

    End Function
    Public Function ConsisteMaskedLote(ByVal Entrada As String, ByRef Lote As Integer, ByRef Secuencia As Integer) As Boolean

        Lote = 0
        Secuencia = 0

        If Not IsNumeric(Strings.Right(Entrada, 3)) Then
            MsgBox("Lote Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Val(Strings.Right(Entrada, 3)) = 0 Then
            MsgBox("Lote Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        Secuencia = Val(Strings.Right(Entrada, 3))

        If Val(Strings.Left(Entrada, Entrada.Length - 3)) = 0 Then
            MsgBox("Lote Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        Lote = Val(Strings.Left(Entrada, Entrada.Length - 3))

        Return True

    End Function
    Public Function FechaParaSql(ByVal Fecha As Date) As String

        Return Format(Fecha.Year, "0000") & Format(Fecha.Month, "00") & Format(Fecha.Day, "00")

    End Function
    Public Function ParaBetween(ByVal Desde As DateTime, ByVal Hasta As DateTime) As String

        Return "AND CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) BETWEEN '" & FechaParaSql(Desde) & "' AND '" & FechaParaSql(Hasta) & "' "

    End Function
    Public Function GetAppPath() As String

        Return System.IO.Directory.GetCurrentDirectory

    End Function
    Public Function GrabaEnvases(ByVal DtEnvases As DataTable) As Integer

        Dim Sql As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Sql = "SELECT * FROM Envases;"
                Using DaP As New OleDb.OleDbDataAdapter(Sql, Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(DtEnvases.GetChanges)
                End Using
                Return 0
            End Using
        Catch ex As OleDb.OleDbException
            Return -1
        Catch ex As DBConcurrencyException
            Return -2
        Finally
        End Try

    End Function
    Public Function GrabaVigencias(ByVal DtVigencias As DataTable) As Integer

        Dim Sql As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Sql = "SELECT * FROM Vigencias;"
                Using DaP As New OleDb.OleDbDataAdapter(Sql, Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(DtVigencias)
                End Using
            End Using
            Return 0
        Catch ex As OleDb.OleDbException
            Return -1
        Catch ex As DBConcurrencyException
            Return -2
        Finally
        End Try

    End Function
    Public Function GrabaAsignaciones(ByVal DtLotes As DataTable, ByVal ConexionAsignacion As String) As Boolean

        Dim Sql As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionAsignacion)
                Miconexion.Open()
                Sql = "SELECT * FROM AsignacionLotes;"
                Using DaP As New OleDb.OleDbDataAdapter(Sql, Miconexion)
                    Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(DtLotes)
                End Using
            End Using
            Return True
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
        End Try

    End Function
    Public Function ActualizaSaldoFacturas(ByVal TipoNota As Integer, ByVal DtFacturaCabezaW As DataTable, ByVal ConexionFactura As String) As Boolean

        Dim Sql As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionFactura)
                Miconexion.Open()
                If Not IsNothing(DtFacturaCabezaW.GetChanges) Then
                    If TipoNota = 5 Or TipoNota = 7 Or TipoNota = 50 Or TipoNota = 60 Or TipoNota = 70 Then
                        Sql = "SELECT * FROM FacturasCabeza;"
                    Else : Sql = "SELECT * FROM FacturasProveedorCabeza;"
                    End If
                    Using DaP As New OleDb.OleDbDataAdapter(Sql, Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtFacturaCabezaW)
                    End Using
                End If
            End Using
            Return True
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
        End Try

    End Function
    Public Function ActualizaSaldoNotas(ByVal DtNotasCabezaW As DataTable, ByVal ConexionNota As String) As Boolean

        Dim Sql As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionNota)
                Miconexion.Open()
                Sql = "SELECT * FROM RecibosCabeza;"
                Using DaP As New OleDb.OleDbDataAdapter(Sql, Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(DtNotasCabezaW)
                End Using
            End Using
            Return True
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
        End Try

    End Function
    Public Function HallaNombreProvincia(ByVal Clave As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return ""
        End Try

    End Function
    Public Function NombreInsumo(ByVal Clave As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Insumos WHERE Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return ""
        End Try

    End Function
    Public Function GrabaTabla(ByVal Tabla As DataTable, ByVal NombreTabla As String, ByVal ConexionTabla As String) As Integer

        If GTipoLicencia = "E" Then End

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionTabla)
                Miconexion.Open()
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM " & NombreTabla & ";", Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(Tabla)
                End Using
                Return 1000
            End Using
        Catch ex As OleDb.OleDbException
            MsgBox(ex.Message)
            If ex.ErrorCode = GAltaExiste Then
                Return -1
            Else
                Return -2
            End If
        Catch ex As DBConcurrencyException
            Return 0
        Finally
        End Try

    End Function
    Public Function ActualizaSaldoLiquidacion(ByVal DtLiquidacionCabezaW As DataTable, ByVal ConexionNota As String) As Boolean

        Dim Sql As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionNota)
                Miconexion.Open()
                Sql = "SELECT * FROM LiquidacionCabeza;"
                Using DaP As New OleDb.OleDbDataAdapter(Sql, Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(DtLiquidacionCabezaW)
                End Using
            End Using
            Return True
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
        End Try

    End Function
    Public Function ActualizaSaldosIniciales(ByVal DtLSaldosInicialesW As DataTable, ByVal ConexionNota As String) As Boolean

        Dim Sql As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionNota)
                Miconexion.Open()
                Sql = "SELECT * FROM SaldosInicialesCabeza;"
                Using DaP As New OleDb.OleDbDataAdapter(Sql, Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(DtLSaldosInicialesW)
                End Using
            End Using
            Return True
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
        End Try

    End Function
    Public Function ActualizaStockDevolucion(ByVal ListaDeLotes As List(Of FilaAsignacion), ByVal Operacion As String) As Boolean

        If Operacion <> "+" And Operacion <> "-" Then
            MsgBox("ERROR, Operacion en Actualizacion de lotes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If

        Dim Sql As String
        Dim Dt As DataTable

        Try
            Dim ConexionDelLote As String
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Devolucion <> 0 Then
                    Sql = "SELECT * FROM Lotes WHERE Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia &
                    " AND Deposito = " & Fila.Deposito & ";"
                    If Fila.Operacion = 1 Then
                        ConexionDelLote = Conexion
                    Else : ConexionDelLote = ConexionN
                    End If
                    Using Miconexion As New OleDb.OleDbConnection(ConexionDelLote)
                        Dt = New DataTable
                        Using DaP As New OleDb.OleDbDataAdapter(Sql, Miconexion)
                            Miconexion.Open()
                            DaP.Fill(Dt)
                            If Dt.Rows.Count = 0 Then
                                MsgBox("Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " No se encuentra en Stock. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                                Return False
                            End If
                            If Operacion = "+" Then
                                Dt.Rows(0).Item("Stock") = CDec(Dt.Rows(0).Item("Stock")) + Fila.Devolucion
                            Else : Dt.Rows(0).Item("Stock") = CDec(Dt.Rows(0).Item("Stock")) - Fila.Devolucion
                            End If
                            If Dt.Rows(0).Item("Stock") < 0 Then
                                If MsgBox("Stock del Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " Negativo. De continuar no modificara stock del lote. Desea Continuar de todas maneras? (No,Si).)", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3, "Error") = MsgBoxResult.No Then
                                    Return False
                                Else
                                    Return True
                                End If
                            End If
                            Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                            DaP.Update(Dt)
                        End Using
                    End Using
                End If
            Next
            Return True
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
        End Try

    End Function
    Public Function ActualizaStockLotes(ByVal ListaDeLotes As List(Of FilaAsignacion), ByVal Operacion As String) As Boolean

        If Operacion <> "+" And Operacion <> "-" Then
            MsgBox("ERROR, Operacion en Actualizacion de lotes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If

        Dim Sql As String
        Dim Dt As DataTable

        Try
            Dim ConexionDelLote As String
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Asignado <> 0 Then
                    Sql = "SELECT * FROM Lotes WHERE Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia &
                    " AND Deposito = " & Fila.Deposito & ";"
                    If Fila.Operacion = 1 Then
                        ConexionDelLote = Conexion
                    Else : ConexionDelLote = ConexionN
                    End If
                    Using Miconexion As New OleDb.OleDbConnection(ConexionDelLote)
                        Dt = New DataTable
                        Using DaP As New OleDb.OleDbDataAdapter(Sql, Miconexion)
                            Miconexion.Open()
                            DaP.Fill(Dt)
                            If Dt.Rows.Count = 0 Then
                                MsgBox("Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " No se encuentra en Stock. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                                Return False
                            End If
                            If Operacion = "+" Then
                                Dt.Rows(0).Item("Stock") = CDec(Dt.Rows(0).Item("Stock")) + Fila.Asignado
                            Else : Dt.Rows(0).Item("Stock") = CDec(Dt.Rows(0).Item("Stock")) - Fila.Asignado
                            End If
                            If Dt.Rows(0).Item("Stock") < 0 Then
                                MsgBox("Stock del Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " Negativo. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                                Return False
                            End If
                            Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                            DaP.Update(Dt)
                        End Using
                    End Using
                End If
            Next
            Return True
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
        End Try

    End Function
    Public Function ReGrabaUltimaNumeracionFactura(ByVal Numero As Double, ByVal TipoFactura As Integer) As Boolean

        Dim Dt As New DataTable

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Dim Numer As Integer = CInt(Strings.Right(Numero, 8))
                'Actualiza UltimaNumeracion PuntodeVenta.
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM PuntosDeVenta WHERE Clave = " & GPuntoDeVenta & ";", Miconexion)
                    Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Fill(Dt)
                    If TipoFactura = 1 Then
                        If Dt.Rows(0).Item("FacturasA") = Numer - 1 Then
                            Dt.Rows(0).Item("FacturasA") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 2 Then
                        If Dt.Rows(0).Item("FacturasB") = Numer - 1 Then
                            Dt.Rows(0).Item("FacturasB") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 3 Then
                        If Dt.Rows(0).Item("FacturasC") = Numer - 1 Then
                            Dt.Rows(0).Item("FacturasC") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 4 Then
                        If Dt.Rows(0).Item("FacturasE") = Numer - 1 Then
                            Dt.Rows(0).Item("FacturasE") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 5 Then
                        If Dt.Rows(0).Item("FacturasM") = Numer - 1 Then
                            Dt.Rows(0).Item("FacturasM") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 6 Then
                        If Dt.Rows(0).Item("FacturasZ") = Numer - 1 Then
                            Dt.Rows(0).Item("FacturasZ") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                End Using
            End Using
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function ReGrabaUltimaNumeracionLiquidacion(ByVal Numero As Double, ByVal TipoFactura As Integer) As Boolean

        Dim Dt As New DataTable

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Dim Numer As Integer = CInt(Strings.Right(Numero, 8))
                'Actualiza UltimaNumeracion PuntodeVenta.
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM PuntosDeVenta WHERE Clave = " & GPuntoDeVenta & ";", Miconexion)
                    Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Fill(Dt)
                    If TipoFactura = 1 Then
                        If Dt.Rows(0).Item("LiquidacionA") = Numer - 1 Then
                            Dt.Rows(0).Item("LiquidacionA") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 2 Then
                        If Dt.Rows(0).Item("LiquidacionB") = Numer - 1 Then
                            Dt.Rows(0).Item("LiquidacionB") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 3 Then
                        If Dt.Rows(0).Item("LiquidacionC") = Numer - 1 Then
                            Dt.Rows(0).Item("LiquidacionC") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 4 Then
                        If Dt.Rows(0).Item("LiquidacionE") = Numer - 1 Then
                            Dt.Rows(0).Item("LiquidacionE") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 5 Then
                        If Dt.Rows(0).Item("LiquidacionM") = Numer - 1 Then
                            Dt.Rows(0).Item("LiquidacionM") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                End Using
            End Using
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function ReGrabaUltimaNumeracionNCredito(ByVal Numero As Double, ByVal TipoFactura As Integer) As Boolean

        Dim Dt As New DataTable
        Dim PuntoDeVenta As Integer = 0
        Dim Numer As Integer = 0
        Dim NumeroStr As String = Format(Numero, "000000000000000")

        Numer = CInt(NumeroStr.Substring(7, 8))
        PuntoDeVenta = CInt(NumeroStr.Substring(3, 4))

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                'Actualiza UltimaNumeracion PuntodeVenta.
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM PuntosDeVenta WHERE Clave = " & PuntoDeVenta & ";", Miconexion)
                    Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Fill(Dt)
                    If TipoFactura = 1 Then
                        If Dt.Rows(0).Item("NotasCreditoA") = Numer - 1 Then
                            Dt.Rows(0).Item("NotasCreditoA") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 2 Then
                        If Dt.Rows(0).Item("NotasCreditoB") = Numer - 1 Then
                            Dt.Rows(0).Item("NotasCreditoB") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 3 Then
                        If Dt.Rows(0).Item("NotasCreditoC") = Numer - 1 Then
                            Dt.Rows(0).Item("NotasCreditoC") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 4 Then
                        If Dt.Rows(0).Item("NotasCreditoE") = Numer - 1 Then
                            Dt.Rows(0).Item("NotasCreditoE") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 5 Then
                        If Dt.Rows(0).Item("NotasCreditoM") = Numer - 1 Then
                            Dt.Rows(0).Item("NotasCreditoM") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 6 Then
                        If Dt.Rows(0).Item("NotasCreditoZ") = Numer - 1 Then
                            Dt.Rows(0).Item("NotasCreditoZ") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                End Using
            End Using
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function ReGrabaUltimaNumeracionNDebito(ByVal Numero As Double, ByVal TipoFactura As Integer) As Boolean

        Dim Dt As New DataTable
        Dim PuntoDeVenta As Integer = 0
        Dim Numer As Integer = 0
        Dim NumeroStr As String = Format(Numero, "000000000000000")

        Numer = CInt(NumeroStr.Substring(7, 8))
        PuntoDeVenta = CInt(NumeroStr.Substring(3, 4))

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                'Actualiza UltimaNumeracion PuntodeVenta.
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM PuntosDeVenta WHERE Clave = " & PuntoDeVenta & ";", Miconexion)
                    Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Fill(Dt)
                    If TipoFactura = 1 Then
                        If Dt.Rows(0).Item("NotasDebitoA") = Numer - 1 Then
                            Dt.Rows(0).Item("NotasDebitoA") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 2 Then
                        If Dt.Rows(0).Item("NotasDebitoB") = Numer - 1 Then
                            Dt.Rows(0).Item("NotasDebitoB") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 3 Then
                        If Dt.Rows(0).Item("NotasDebitoC") = Numer - 1 Then
                            Dt.Rows(0).Item("NotasDebitoC") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 4 Then
                        If Dt.Rows(0).Item("NotasDebitoE") = Numer - 1 Then
                            Dt.Rows(0).Item("NotasDebitoE") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                    If TipoFactura = 5 Then
                        If Dt.Rows(0).Item("NotasDebitoM") = Numer - 1 Then
                            Dt.Rows(0).Item("NotasDebitoM") = Numer
                            DaP.Update(Dt)
                            Return True
                        Else : Return False
                        End If
                    End If
                End Using
            End Using
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function ReGrabaUltimaNumeracion(ByVal Numero As Double, ByVal Tipo As Integer) As Boolean

        Dim Dt As DataTable

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Dim Numer As Integer = CInt(Strings.Right(Numero, 8))
                'Actualiza UltimaNumeracion PuntodeVenta.
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM PuntosDeVenta WHERE Clave = " & GPuntoDeVenta & ";", Miconexion)
                    Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                    Dt = New DataTable
                    DaP.Fill(Dt)
                    If Tipo = 5 Then
                        If Dt.Rows(0).Item("NotasDebito") = Numer - 1 Then
                            Dt.Rows(0).Item("NotasDebito") = Numer
                            DaP.Update(Dt)
                        Else : Return False
                        End If
                    End If
                    If Tipo = 7 Or Tipo = 4 Then
                        If Dt.Rows(0).Item("NotasCredito") = Numer - 1 Then
                            Dt.Rows(0).Item("NotasCredito") = Numer
                            DaP.Update(Dt)
                        Else : Return False
                        End If
                    End If
                    If Tipo = 1 Then
                        If Dt.Rows(0).Item("Remitos") = Numer - 1 Then
                            Dt.Rows(0).Item("Remitos") = Numer
                            DaP.Update(Dt)
                        Else : Return False
                        End If
                    End If
                    If Tipo = 11 Then
                        If Dt.Rows(0).Item("DevolucionMercaderia") = Numer - 1 Then
                            Dt.Rows(0).Item("DevolucionMercaderia") = Numer
                            DaP.Update(Dt)
                        Else : Return False
                        End If
                    End If
                End Using
                Dt.Dispose()
                Return True
            End Using
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
        End Try

    End Function
    Public Function ReGrabaUltimoNumeroLote(ByVal Lote As Integer) As Boolean

        Dim Dt As DataTable

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Dim Numer As Integer = Lote
                'Actualiza UltimaNumeracion Lote.
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM NumeracionDocumentos WHERE Clave = 1;", Miconexion)
                    Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                    Dt = New DataTable
                    DaP.Fill(Dt)
                    If Dt.Rows(0).Item("UltimoLote") = Numer - 1 Then
                        Dt.Rows(0).Item("UltimoLote") = Numer
                        DaP.Update(Dt)
                    Else : Return False
                    End If
                End Using
                Dt.Dispose()
                Return True
            End Using
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
        End Try

    End Function
    Public Function EstaRemitoLiquidado(ByVal Operacion As Integer, ByVal Remito As Double) As Boolean

        Dim ConexionStr As String
        Dim Dt As New DataTable

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT Liquidado FROM AsignacionLotes WHERE Cantidad > 0 AND TipoComprobante = 1 AND Comprobante = " & Remito & ";", ConexionStr, Dt) Then End
        If Dt.Rows.Count = 0 Then Dt.Dispose() : Return False
        For Each Row As DataRow In Dt.Rows
            If Not Row("Liquidado") Then Dt.Dispose() : Return False
        Next

        Dt.Dispose()
        Return True

    End Function
    Public Function HalloPrecioDeLista(ByVal Lista As Integer, ByVal Articulo As Integer, ByVal Fecha As DateTime) As Decimal

        Dim DiaStr As String = ""

        If Weekday(Fecha) = 1 Then DiaStr = "Domingo"
        If Weekday(Fecha) = 2 Then DiaStr = "Lunes"
        If Weekday(Fecha) = 3 Then DiaStr = "Martes"
        If Weekday(Fecha) = 4 Then DiaStr = "Miercoles"
        If Weekday(Fecha) = 5 Then DiaStr = "Jueves"
        If Weekday(Fecha) = 6 Then DiaStr = "Viernes"
        If Weekday(Fecha) = 7 Then DiaStr = "Sabado"

        Dim Sql As String = "SELECT " & DiaStr & " FROM ListaDePreciosDetalle WHERE Lista = " & Lista & " AND Articulo = " & Articulo & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Sub ArmaTablaIva(ByRef TablaIva() As Double)

        Dim Dt As New DataTable
        Dt = Tablas.Leer("SELECT Iva FROM Tablas Where Tipo = 22;")
        For Each Row As DataRow In Dt.Rows
            ReDim Preserve TablaIva(UBound(TablaIva) + 1)
            TablaIva(UBound(TablaIva)) = Row("Iva")
        Next

        Dt.Dispose()

    End Sub
    Public Sub ArmaTablaIvaConImporte(ByRef TablaIva() As ItemIvaReten)

        Dim Dt As New DataTable
        Dt = Tablas.Leer("SELECT Clave,Iva,Nombre FROM Tablas Where Tipo = 22;")

        For Each Row As DataRow In Dt.Rows
            ReDim Preserve TablaIva(UBound(TablaIva) + 1)
            TablaIva(UBound(TablaIva)) = New ItemIvaReten
            TablaIva(UBound(TablaIva)).Clave = Row("Clave")
            TablaIva(UBound(TablaIva)).Alicuota = Row("Iva")
            TablaIva(UBound(TablaIva)).Nombre = Row("Nombre")
            TablaIva(UBound(TablaIva)).Importe = 0
        Next

        Dt.Dispose()

    End Sub
    Public Sub ArmaDtGridReciboPago(ByRef DtGridW As DataTable)

        DtGridW = New DataTable

        Dim MedioPago As New DataColumn("MedioPago")
        MedioPago.DataType = System.Type.GetType("System.Int32")
        DtGridW.Columns.Add(MedioPago)

        Dim Detalle As New DataColumn("Detalle")
        Detalle.DataType = System.Type.GetType("System.String")
        DtGridW.Columns.Add(Detalle)

        Dim Neto As New DataColumn("Neto")
        Neto.DataType = System.Type.GetType("System.Double")
        DtGridW.Columns.Add(Neto)

        Dim Alicuota As New DataColumn("Alicuota")
        Alicuota.DataType = System.Type.GetType("System.Double")
        DtGridW.Columns.Add(Alicuota)

        Dim ImporteIva As New DataColumn("ImporteIva")
        ImporteIva.DataType = System.Type.GetType("System.Double")
        DtGridW.Columns.Add(ImporteIva)

        Dim Cambio As New DataColumn("Cambio")
        Cambio.DataType = System.Type.GetType("System.Double")
        DtGridW.Columns.Add(Cambio)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGridW.Columns.Add(Importe)

        Dim Banco As New DataColumn("Banco")
        Banco.DataType = System.Type.GetType("System.Int32")
        DtGridW.Columns.Add(Banco)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Double")
        DtGridW.Columns.Add(Cuenta)

        Dim Serie As New DataColumn("Serie")
        Serie.DataType = System.Type.GetType("System.String")
        DtGridW.Columns.Add(Serie)

        Dim Numero As New DataColumn("Numero")
        Numero.DataType = System.Type.GetType("System.Int32")
        DtGridW.Columns.Add(Numero)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGridW.Columns.Add(Fecha)

        Dim EmisorCheque As New DataColumn("EmisorCheque")
        EmisorCheque.DataType = System.Type.GetType("System.String")
        DtGridW.Columns.Add(EmisorCheque)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGridW.Columns.Add(Comprobante)

        Dim FechaComprobante As New DataColumn("FechaComprobante")
        FechaComprobante.DataType = System.Type.GetType("System.DateTime")
        DtGridW.Columns.Add(FechaComprobante)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGridW.Columns.Add(Operacion)

        Dim ClaveCheque As New DataColumn("ClaveCheque")
        ClaveCheque.DataType = System.Type.GetType("System.Int32")
        DtGridW.Columns.Add(ClaveCheque)

        Dim NumeracionInicial As New DataColumn("NumeracionInicial")
        NumeracionInicial.DataType = System.Type.GetType("System.Int32")
        DtGridW.Columns.Add(NumeracionInicial)

        Dim NumeracionFinal As New DataColumn("NumeracionFinal")
        NumeracionFinal.DataType = System.Type.GetType("System.Int32")
        DtGridW.Columns.Add(NumeracionFinal)

    End Sub
    Public Function HallaNVLP(ByVal OperacionLote As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Deposito As Integer, ByVal OperacionRemito As Integer, ByVal Remito As Double, ByRef Liquidacion As Double, ByRef ReciboOficial As Double, ByRef Cantidad As Decimal) As Boolean

        Dim Dt As New DataTable

        Liquidacion = 0
        ReciboOficial = 0
        Cantidad = 0

        Dim Sql As String = "SELECT C.Liquidacion,C.ReciboOficial,L.Cantidad FROM NVLPCabeza AS C INNER JOIN NVLPLotes AS L ON C.Liquidacion = L.Liquidacion WHERE C.Estado = 1 AND L.Lote = " & Lote & " AND L.Secuencia = " &
            Secuencia & " AND L.Deposito = " & Deposito & " AND L.Remito = " & Remito & " AND OPR = " & OperacionRemito & " AND Operacion = " & OperacionLote & ";"

        If OperacionRemito = 1 Then
            If Not Tablas.Read(Sql, Conexion, Dt) Then Return False
            If Dt.Rows.Count = 0 And PermisoTotal Then
                If Not Tablas.Read(Sql, ConexionN, Dt) Then Return False
            End If
        Else
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Return False
        End If

        If Dt.Rows.Count <> 0 Then
            Liquidacion = Dt.Rows(0).Item("Liquidacion")
            ReciboOficial = Dt.Rows(0).Item("ReciboOficial")
            Cantidad = Dt.Rows(0).Item("Cantidad")
        End If

        Dt.Dispose()
        Return True

    End Function
    Public Function HallaOperacionRemito(ByVal OperacionFactura As Boolean, ByVal Remito As Double, ByVal Factura As Double, ByVal FacturaN As Double) As Integer

        If OperacionFactura Then Return 1
        If OperacionFactura = False And Factura <> 0 And FacturaN <> 0 Then Return 1

        'Si factura es falsa y la factura no es mixta entonces debo buscar en la base N y base B.

        'Busco en Blanca
        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Remito FROM RemitosCabeza WHERE Remito = " & Remito & " AND Factura = 1;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then If CDbl(Ultimo) <> 0 Then Return 1
                End Using
            End Using
        Catch ex As Exception
            Return 0
        End Try
        'Busco en Negra
        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Remito FROM RemitosCabeza WHERE Remito = " & Remito & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then If CDbl(Ultimo) <> 0 Then Return 2
                End Using
            End Using
        Catch ex As Exception
            Return 0
        End Try

    End Function
    Public Sub ArmaNuevaFacturaProveedor(ByRef Row As DataRow)

        Row("Factura") = 0
        Row("ReciboOficial") = 0
        Row("Proveedor") = 0
        Row("Fecha") = Date.Now
        Row("FechaFactura") = Date.Now
        Row("FechaContable") = Date.Now
        Row("EsReventa") = False
        Row("EsInsumos") = False
        Row("EsSinComprobante") = False
        Row("EsAfectaCostoLotes") = False
        Row("ConceptoGasto") = 0
        Row("Costeo") = 0
        Row("Importe") = 0
        Row("Saldo") = 0
        Row("Liquidacion") = 0
        Row("Rel") = False
        Row("Nrel") = 0
        Row("Tr") = False
        Row("Estado") = 1
        Row("Tr") = False
        Row("Moneda") = 1
        Row("Cambio") = 1
        Row("EsExterior") = False
        Row("Secos") = False
        Row("IncoTerm") = 0
        Row("TipoNota") = 0
        Row("NotaDebito") = 0
        Row("Rendicion") = 0
        Row("Comentario") = ""

    End Sub
    Public Sub ArmaNuevaLiquidacion(ByRef Row As DataRow)

        Row("Liquidacion") = 0
        Row("Interno") = 0
        Row("Rel") = False
        Row("NRel") = 0
        Row("EsReventa") = False
        Row("EsConsignacion") = False
        Row("EsInsumos") = False
        Row("NRel") = 0
        Row("Tr") = False
        Row("Proveedor") = 0
        Row("Fecha") = Date.Now
        Row("FechaContable") = "01/01/1800"
        Row("Bruto") = 0
        Row("Alicuota") = 0
        Row("Comision") = 0
        Row("AlicuotaComision") = 0
        Row("Descarga") = 0
        Row("Bruto") = 0
        Row("AlicuotaDescarga") = 0
        Row("Directo") = 0
        Row("Neto") = 0
        Row("Estado") = 1
        Row("Saldo") = 0
        Row("Factura") = 0
        Row("Impreso") = False
        Row("Comentario") = ""
        Row("Senia") = 0
        Row("Importe") = 0

    End Sub
    Public Sub ArmaNuevaNVLP(ByRef Row As DataRow)

        Row("Liquidacion") = 0
        Row("ReciboOficial") = 0
        Row("Cliente") = 0
        Row("Fecha") = Now.Date
        Row("FechaLiquidacion") = "01/01/1800"
        Row("FechaContable") = "01/01/1800"
        Row("ImporteBruto") = 0
        Row("Importe") = 0
        Row("Saldo") = 0
        Row("Rel") = False
        Row("Nrel") = 0
        Row("Estado") = 1
        Row("Tr") = False
        Row("Comentario") = ""

    End Sub
    Public Sub ArmaNuevaFactura(ByRef Row As DataRow)

        InicializaRegistros.ArmaNuevaFactura(Row)

    End Sub
    Public Sub ArmaNuevoRecibo(ByRef Row As DataRow)

        Row("TipoNota") = 0
        Row("Nota") = 0
        Row("Interno") = 0
        Row("Emisor") = 0
        Row("NumeroFondoFijo") = 0
        Row("Fecha") = Date.Now
        Row("FechaReciboOficial") = "01/01/1800"
        Row("FechaContable") = "01/01/1800"
        Row("CodigoIva") = 0
        Row("Importe") = 0
        Row("Bultos") = 0
        Row("Saldo") = 0
        Row("Estado") = 0
        Row("Caja") = 0
        Row("ReciboOficial") = 0
        Row("MedioPagoRechazado") = 0
        Row("ChequeRechazado") = 0
        Row("Comentario") = ""
        Row("ACuenta") = 0
        Row("ContadoEfectivo") = False
        Row("Secos") = False
        Row("Tr") = False
        Row("EsFCE") = False
        Row("ClaveChequeReemplazado") = 0
        Row("ClaveChequeReemplazo") = 0
        Row("Moneda") = 0
        Row("Cambio") = 0
        Row("Manual") = False
        Row("Impreso") = False
        Row("EsExterior") = False
        Row("DiferenciaDeCambio") = False
        Row("ClienteOperacion") = 0
        Row("Costeo") = 0
        Row("RetencionManual") = False
        Row("Cae") = 0
        Row("FechaCae") = 0
        Row("EsElectronica") = False
        Row("TipoCompAsociado") = 0
        Row("CompAsociado") = 0
        Row("NotaAnulacion") = 0
        Row("ConceptoGasto") = 0

    End Sub
    Public Sub ArmaNuevaRendicionFondoFijo(ByRef Row As DataRow)

        Row("Factura") = 0
        Row("OP") = 0
        Row("FondoFijo") = 0
        Row("Numero") = 0
        Row("ReciboOficial") = 0
        Row("Proveedor") = 0
        Row("Fecha") = Date.Now
        Row("FechaFactura") = Date.Now
        Row("FechaContable") = Date.Now
        Row("ConceptoGasto") = 0
        Row("Costeo") = 0
        Row("Importe") = 0
        Row("Saldo") = 0
        Row("Rel") = False
        Row("Nrel") = 0
        Row("Estado") = 1
        Row("Moneda") = 1
        Row("Cambio") = 1
        Row("EsExterior") = False
        Row("Secos") = False
        Row("Comentario") = ""

    End Sub
    Public Sub ArmaNuevoMovimientoOtroPago(ByRef Row As DataRow)

        Row("Proveedor") = 0
        Row("TipoNota") = 0
        Row("TipoPago") = 0
        Row("Movimiento") = 0
        Row("Importe") = 0
        Row("Fecha") = Date.Now
        Row("MedioPagoRechazado") = 0
        Row("ChequeRechazado") = 0
        Row("Caja") = 0
        Row("Comentario") = ""
        Row("Estado") = 0
        Row("Saldo") = 0
        Row("ClaveChequeReemplazado") = 0
        Row("ClaveChequeReemplazo") = 0

    End Sub
    Public Sub ArmaNuevoMovimientoPrestamo(ByRef Row As DataRow)

        Row("Prestamo") = 0
        Row("TipoNota") = 0
        Row("Movimiento") = 0
        Row("Importe") = 0
        Row("OrigenRechazo") = 0
        Row("Fecha") = Date.Now
        Row("MedioPagoRechazado") = 0
        Row("ChequeRechazado") = 0
        Row("ClaveChequeReemplazado") = 0
        Row("ClaveChequeReemplazo") = 0
        Row("Caja") = 0
        Row("Estado") = 0
        Row("ReciboOficial") = 0
        Row("Comentario") = ""

    End Sub
    Public Sub ArmaNuevoPagoSueldo(ByRef Row As DataRow)

        Row("Legajo") = 0
        Row("TipoNota") = 0
        Row("Movimiento") = 0
        Row("Importe") = 0
        Row("Fecha") = Date.Now
        Row("MedioPagoRechazado") = 0
        Row("ChequeRechazado") = 0
        Row("Caja") = 0
        Row("Comentario") = ""
        Row("Estado") = 0
        Row("Saldo") = 0
        Row("ClaveChequeReemplazado") = 0
        Row("ClaveChequeReemplazo") = 0

    End Sub
    Public Sub ArmaNuevoCompraDivisas(ByRef Row As DataRow)

        Row("Movimiento") = 0
        Row("TipoMovimiento") = 0
        Row("Emisor") = 0
        Row("Origen") = 0
        Row("Fecha") = Date.Now
        Row("FechaContable") = "01/01/1800"
        Row("Moneda") = 0
        Row("Cambio") = 0
        Row("Importe") = 0
        Row("Caja") = 0
        Row("MedioPagoRechazado") = 0
        Row("ChequeRechazado") = 0
        Row("ClaveChequeReemplazado") = 0
        Row("ClaveChequeReemplazo") = 0
        Row("Comentario") = ""
        Row("Estado") = 0

    End Sub
    Public Sub ArmaNuevoMovimientoFondoFijo(ByRef Row As DataRow)

        Row("Tipo") = 0
        Row("Numero") = 0
        Row("Movimiento") = 0
        Row("FondoFijo") = 0
        Row("Fecha") = Date.Now
        Row("Importe") = 0
        Row("Caja") = 0
        Row("Estado") = 1
        Row("MedioPagoRechazado") = 0
        Row("ChequeRechazado") = 0
        Row("ClaveChequeReemplazado") = 0
        Row("ClaveChequeReemplazo") = 0
        Row("Saldo") = 0
        Row("Comentario") = ""

    End Sub
    Public Sub ArmaNuevoMovimientoBancario(ByRef Row As DataRow)

        Row("Movimiento") = 0
        Row("TipoNota") = 0
        Row("MedioPago") = 0
        Row("Fecha") = Date.Now
        Row("Comprobante") = 0
        Row("FechaComprobante") = Date.Now
        Row("Banco") = 0
        Row("Cuenta") = 0
        Row("BancoDestino") = 0
        Row("CuentaDestino") = 0
        Row("Importe") = 0
        Row("Diferido") = 0
        Row("MedioPagoRechazado") = 0
        Row("ChequeRechazado") = 0
        Row("Caja") = 0
        Row("Estado") = 0
        Row("Comentario") = ""
        Row("Cambio") = 0

    End Sub
    Public Function HallaAfectado(ByVal Orden As Double) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT P.Nombre FROM Proveedores AS P INNER JOIN RecibosCabeza AS R ON P.Clave = R.Emisor AND R.TipoNota = 600 WHERE R.Nota = " & Orden & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then Return CStr(Ultimo)
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos.")
            End
        End Try

    End Function
    Public Sub ArmaSqlInterBanking(ByVal Banco As Integer, ByVal Cuenta As Double, ByVal Caja As Integer, ByRef SqlBInter As String, ByRef SqlNInter As String, ByVal VencimientoDesde As Date, ByVal VencimientoHasta As Date, ByVal SoloSecos As Boolean)

        Dim StrBancoInter As String = ""
        If Banco <> 0 Then
            StrBancoInter = " AND P.Banco = " & Banco
        End If

        Dim StrCuentaInter As String = ""
        If Cuenta <> 0 Then
            StrCuentaInter = " AND P.Cuenta = " & Cuenta
        End If

        Dim SqlCajaInter As String = ""
        If Caja <> 999 Then SqlCajaInter = " AND C.Caja = " & Caja
        Dim SqlFechaInter As String
        If Format(VencimientoDesde, "dd/MM/yyyy") <> "01/01/1000" Then
            SqlFechaInter = " AND C.Fecha >='" & Format(VencimientoDesde, "yyyyMMdd") & "' AND C.Fecha < DATEADD(dd,1,'" & Format(VencimientoHasta, "yyyyMMdd") & "') "
        End If

        SqlBInter = "SELECT 1 AS Operacion,2 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Emisor,C.Fecha,C.Caja,C.Cambio,NumeroFondoFijo FROM RecibosCabeza AS C INNER JOIN RecibosDetallePago AS P " &
                            "ON C.TipoNota = P.TipoNota AND C.Nota = P.Nota WHERE C.Estado <> 3 AND P.TipoNota = 600 AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                       " UNION ALL " &
                     "SELECT 1 AS Operacion,3 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Emisor,C.Fecha,C.Caja,C.Cambio,NumeroFondoFijo FROM RecibosCabeza AS C INNER JOIN RecibosDetallePago AS P " &
                            "ON C.TipoNota = P.TipoNota AND C.Nota = P.Nota WHERE C.Estado <> 3 AND (P.TipoNota = 64 OR P.TipoNota = 65) AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                       " UNION ALL " &
                    "SELECT 1 AS Operacion,C.Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Emisor,C.Fecha,C.Caja,1 as Cambio,0 As NumeroFondoFijo FROM CompraDivisasCabeza AS C INNER JOIN CompraDivisasPago AS P ON C.Movimiento = P.Movimiento " &
                            "WHERE C.Estado <> 3 AND C.TipoMovimiento = 6000 AND P.Tipo = 1 AND (P.MedioPago = 4 OR P.MedioPago = 11)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                       " UNION ALL " &
                    "SELECT 1 AS Operacion,C.Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Emisor,MC.Fecha,C.Caja,1 as Cambio,0 As NumeroFondoFijo FROM PrestamosCabeza AS C INNER JOIN (PrestamosMovimientoCabeza AS MC INNER JOIN PrestamosMovimientoPago AS P ON MC.Movimiento = P.Movimiento) " &
                            "ON C.Prestamo = MC.Prestamo WHERE MC.Estado <> 3 AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                       " UNION ALL " &
                    "SELECT 1 AS Operacion,4 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Legajo AS Emisor,C.Fecha ,C.Caja,1 As Cambio,0 As NumeroFondoFijo FROM SueldosMovimientoCabeza AS C INNER JOIN SueldosMovimientoPago AS P ON C.Movimiento = P.Movimiento " &
                            "WHERE C.Estado <> 3 AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                       " UNION ALL " &
                    "SELECT 1 AS Operacion,5 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Proveedor AS Emisor,C.Fecha,C.Caja,1 AS Cambio,0 As NumeroFondoFijo FROM OtrosPagosCabeza AS C INNER JOIN OtrosPagosPago AS P ON C.Movimiento = P.Movimiento " &
                            "WHERE C.Estado <> 3 AND C.TipoNota <> 5020 AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                       " UNION ALL " &
                    "SELECT 1 AS Operacion,6 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.FondoFijo AS Emisor,C.Fecha,C.Caja,1 AS Cambio,0 As NumeroFondoFijo FROM MovimientosFondoFijoCabeza AS C INNER JOIN MovimientosFondoFijoPago AS P ON C.Movimiento = P.Movimiento " &
                            "WHERE C.Estado <> 3 AND C.Tipo = 7001 AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter & ";"



        SqlNInter = "SELECT 2 AS Operacion,2 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Emisor,C.Fecha,C.Caja,C.Cambio,NumeroFondoFijo FROM RecibosCabeza AS C INNER JOIN RecibosDetallePago AS P " &
                                    "ON C.TipoNota = P.TipoNota AND C.Nota = P.Nota WHERE C.Estado <> 3 AND P.TipoNota = 600 AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                       " UNION ALL " &
                     "SELECT 2 AS Operacion,3 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Emisor,C.Fecha,C.Caja,C.Cambio,NumeroFondoFijo FROM RecibosCabeza AS C INNER JOIN RecibosDetallePago AS P " &
                            "ON C.TipoNota = P.TipoNota AND C.Nota = P.Nota WHERE C.Estado <> 3 AND (P.TipoNota = 64 OR P.TipoNota = 65) AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                     " UNION ALL " &
                    "SELECT 2 AS Operacion,C.Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Emisor,C.Fecha,C.Caja,1 as Cambio,0 As NumeroFondoFijo FROM CompraDivisasCabeza AS C INNER JOIN CompraDivisasPago AS P ON C.Movimiento = P.Movimiento " &
                            "WHERE C.Estado <> 3 AND C.TipoMovimiento = 6000 AND P.Tipo = 1 AND (P.MedioPago = 4 OR P.MedioPago = 11)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                       " UNION ALL " &
                    "SELECT 2 AS Operacion,C.Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Emisor,MC.Fecha,C.Caja,1 AS Cambio,0 As NumeroFondoFijo FROM PrestamosCabeza AS C INNER JOIN (PrestamosMovimientoCabeza AS MC INNER JOIN PrestamosMovimientoPago AS P ON MC.Movimiento = P.Movimiento) " &
                            "ON C.Prestamo = MC.Prestamo WHERE MC.Estado <> 3 AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                       " UNION ALL " &
                    "SELECT 2 AS Operacion,4 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Legajo AS Emisor,C.Fecha,C.Caja,1 AS Cambio,0 As NumeroFondoFijo FROM SueldosMovimientoCabeza AS C INNER JOIN SueldosMovimientoPago AS P ON C.Movimiento = P.Movimiento " &
                            "WHERE C.Estado <> 3 AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                       " UNION ALL " &
                    "SELECT 2 AS Operacion,5 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Proveedor AS Emisor,C.Fecha,C.Caja,1 AS Cambio,0 As NumeroFondoFijo FROM OtrosPagosCabeza AS C INNER JOIN OtrosPagosPago AS P ON C.Movimiento = P.Movimiento " &
                            "WHERE C.Estado <> 3 AND C.TipoNota <> 5020 AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                       " UNION ALL " &
                    "SELECT 2 AS Operacion,6 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.FondoFijo AS Emisor,C.Fecha,C.Caja,1 AS Cambio,0 As NumeroFondoFijo FROM MovimientosFondoFijoCabeza AS C INNER JOIN MovimientosFondoFijoPago AS P ON C.Movimiento = P.Movimiento " &
                            "WHERE C.Estado <> 3 AND C.Tipo = 7001 AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter & ";"


        If SoloSecos Then
            SqlBInter = "SELECT 1 AS Operacion,2 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Emisor,C.Fecha,C.Caja,C.Cambio,NumeroFondoFijo FROM RecibosCabeza AS C INNER JOIN RecibosDetallePago AS P " &
                                "ON C.TipoNota = P.TipoNota AND C.Nota = P.Nota WHERE C.Secos = 1 AND C.Estado <> 3 AND P.TipoNota = 600 AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                            " UNION ALL " &
                        "SELECT 1 AS Operacion,3 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Emisor,C.Fecha,C.Caja,C.Cambio,NumeroFondoFijo FROM RecibosCabeza AS C INNER JOIN RecibosDetallePago AS P " &
                                "ON C.TipoNota = P.TipoNota AND C.Nota = P.Nota WHERE C.Secos = 1 AND C.Estado <> 3 AND (P.TipoNota = 64 OR P.TipoNota = 65) AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter & ";"

            SqlNInter = "SELECT 2 AS Operacion,2 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Emisor,C.Fecha,C.Caja,C.Cambio,NumeroFondoFijo FROM RecibosCabeza AS C INNER JOIN RecibosDetallePago AS P " &
                                        "ON C.TipoNota = P.TipoNota AND C.Nota = P.Nota WHERE C.Secos = 1 AND C.Estado <> 3 AND P.TipoNota = 600 AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter &
                            " UNION ALL " &
                        "SELECT 2 AS Operacion,3 AS Origen,P.MedioPago,P.Banco,P.Cuenta,P.Comprobante,P.FechaComprobante,P.Importe,C.Emisor,C.Fecha,C.Caja,C.Cambio,NumeroFondoFijo FROM RecibosCabeza AS C INNER JOIN RecibosDetallePago AS P " &
                                "ON C.TipoNota = P.TipoNota AND C.Nota = P.Nota WHERE C.Secos = 1 AND C.Estado <> 3 AND (P.TipoNota = 64 OR P.TipoNota = 65) AND (P.MedioPago = 4 OR P.MedioPago = 11 OR P.MedioPago = 9)" & StrBancoInter & StrCuentaInter & SqlCajaInter & SqlFechaInter & ";"
        End If

    End Sub
    Public Function ArmaDtChequesTerceros(ByRef Dt As DataTable, ByVal Caja As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Blanco As Boolean, ByVal Negro As Boolean, ByVal EnCartera As Boolean, ByVal Entregado As Boolean, ByVal ClaveCheque As Integer) As Boolean

        Dim Sql1 As String = " CH.MedioPago = 3"
        If Caja <> 999 Then Sql1 = Sql1 & " AND CH.Caja = " & Caja
        If Format(Desde, "dd/MM/yyyy") <> "01/01/1000" Then
            Sql1 = Sql1 & " AND CH.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND CH.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "')"
        End If
        If EnCartera And Not Entregado Then Sql1 = Sql1 & " AND CH.CompDestino = 0"
        If Entregado And Not EnCartera Then Sql1 = Sql1 & " AND CH.CompDestino <> 0"
        If ClaveCheque <> 0 Then
            Sql1 = Sql1 & " AND CH.ClaveCheque = " & ClaveCheque
        End If

        Dim SqlB As String
        Dim SqlN As String

        SqlB = "SELECT 1 AS Operacion,CH.*,C.Emisor,C.Fecha AS FechaRecibido,3 AS Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino,NumeroFondoFijo FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoOrigen = C.TipoNota AND CH.CompOrigen = C.Nota WHERE " & Sql1 & " AND ReciboOficial = 0 " &
                " UNION ALL " &
             "SELECT 1 AS Operacion,CH.*,C.Emisor,C.FechaReciboOficial AS FechaRecibido,3 AS Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino,NumeroFondoFijo FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoOrigen = C.TipoNota AND CH.CompOrigen = C.Nota WHERE " & Sql1 & " AND ReciboOficial <> 0 " &
                " UNION ALL " &
             "SELECT 1 AS Operacion,CH.*,C.Emisor,C.Fecha AS FechaRecibido,C.Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino, 0 AS NumeroFondoFijo FROM CompraDivisasCabeza As C INNER JOIN Cheques AS CH ON CH.TipoOrigen = C.TipoMovimiento AND CH.CompOrigen = C.Movimiento WHERE " & Sql1 &
                " UNION ALL " &
             "SELECT 1 AS Operacion,CH.*,Proveedor AS Emisor,C.Fecha As FechaRecibido,5 AS Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino, 0 AS NumeroFondoFijo FROM OtrosPagosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoOrigen = 5020 AND CH.CompOrigen = C.Movimiento WHERE " & Sql1 &
                " UNION ALL " &
             "SELECT 1 AS Operacion,CH.*,C.Emisor,C.Fecha As FechaRecibido,C.Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino, 0 AS NumeroFondoFijo FROM PrestamosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoOrigen = 1000 AND CH.CompOrigen = Prestamo WHERE " & Sql1 &
               " UNION ALL " &
             "SELECT 1 AS Operacion,CH.*,C.Emisor,MC.Fecha AS FechaRecibido,C.Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino, 0 AS NumeroFondoFijo FROM PrestamosCabeza As C INNER JOIN (PrestamosMovimientoCabeza AS MC INNER JOIN Cheques AS CH ON CH.TipoOrigen = MC.TipoNota AND CH.CompOrigen = MC.Movimiento) ON C.Prestamo = MC.Prestamo WHERE " & Sql1 &
               " UNION ALL " &
             "SELECT 1 AS Operacion,CH.*,C.Emisor,C.Recibido As FechaRecibido,C.Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino, 0 AS NumeroFondoFijo FROM ChequesIniciales As C INNER JOIN Cheques AS CH ON C.MedioPago = CH.MedioPago AND CH.ClaveCheque = C.ClaveCheque WHERE " & Sql1 &
                " UNION ALL " &
             "SELECT 1 AS Operacion,CH.*,C.FondoFijo AS Emisor,C.Fecha As FechaRecibido,6 AS Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino, 0 AS NumeroFondoFijo FROM MovimientosFondoFijoCabeza As C INNER JOIN Cheques AS CH ON CH.TipoOrigen = 7002 AND CH.CompOrigen = C.Movimiento WHERE " & Sql1 & ";"


        SqlN = "SELECT 2 AS Operacion,CH.*,C.Emisor,C.Fecha AS FechaRecibido,3 AS Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino,NumeroFondoFijo FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoOrigen = C.TipoNota AND CH.CompOrigen = C.Nota WHERE " & Sql1 & " AND ReciboOficial = 0 " &
                " UNION ALL " &
             "SELECT 2 AS Operacion,CH.*,C.Emisor,C.FechaReciboOficial AS FechaRecibido,3 AS Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino,NumeroFondoFijo FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoOrigen = C.TipoNota AND CH.CompOrigen = C.Nota WHERE " & Sql1 & " AND ReciboOficial <> 0 " &
               " UNION ALL " &
             "SELECT 2 AS Operacion,CH.*,C.Emisor,C.Fecha AS FechaRecibido,C.Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino, 0 AS NumeroFondoFijo FROM CompraDivisasCabeza As C INNER JOIN Cheques AS CH ON CH.TipoOrigen = C.TipoMovimiento AND CH.CompOrigen = C.Movimiento WHERE " & Sql1 &
                " UNION ALL " &
             "SELECT 2 AS Operacion,CH.*,Proveedor AS Emisor,C.Fecha As FechaRecibido,5 AS Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino, 0 AS NumeroFondoFijo FROM OtrosPagosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoOrigen = 5020 AND CH.CompOrigen = C.Movimiento WHERE " & Sql1 &
                " UNION ALL " &
             "SELECT 2 AS Operacion,CH.*,C.Emisor,C.Fecha As FechaRecibido,C.Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino, 0 AS NumeroFondoFijo FROM PrestamosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoOrigen = 1000 AND CH.CompOrigen = Prestamo WHERE " & Sql1 &
               " UNION ALL " &
             "SELECT 2 AS Operacion,CH.*,C.Emisor,MC.Fecha AS FechaRecibido,C.Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino, 0 AS NumeroFondoFijo FROM PrestamosCabeza As C INNER JOIN (PrestamosMovimientoCabeza AS MC INNER JOIN Cheques AS CH ON CH.TipoOrigen = MC.TipoNota AND CH.CompOrigen = MC.Movimiento) ON C.Prestamo = MC.Prestamo WHERE " & Sql1 &
               " UNION ALL " &
             "SELECT 2 AS Operacion,CH.*,C.Emisor,C.Recibido As FechaRecibido,C.Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino, 0 AS NumeroFondoFijo FROM ChequesIniciales As C INNER JOIN Cheques AS CH ON C.MedioPago = CH.MedioPago AND CH.ClaveCheque = C.ClaveCheque WHERE " & Sql1 &
                " UNION ALL " &
             "SELECT 2 AS Operacion,CH.*,C.FondoFijo AS Emisor,C.Fecha As FechaRecibido,6 AS Origen,0 AS Destino,0 AS EmisorDestino,'1/1/1800' AS FechaDestino, 0 AS NumeroFondoFijo FROM MovimientosFondoFijoCabeza As C INNER JOIN Cheques AS CH ON CH.TipoOrigen = 7002 AND CH.CompOrigen = C.Movimiento WHERE " & Sql1 & ";"

        If Blanco Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Return False
        End If
        If Negro And PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Return False
        End If

        Dim ConexionStr As String
        Dim Destino As Integer
        Dim EmisorDestino As Integer
        Dim FechaDestino As Date

        For Each Row As DataRow In Dt.Rows
            If Row("TipoOrigen") = 604 Then Row("Origen") = 2
            If Row("Operacion") = 1 Then
                ConexionStr = Conexion
            Else
                ConexionStr = ConexionN
            End If
            If Row("TipoDestino") <> 0 Then
                If Not HallaDatosDestino(Row("TipoDestino"), Row("CompDestino"), ConexionStr, EmisorDestino, Destino, FechaDestino) Then Return False
                Row("Destino") = Destino
                Row("EmisorDestino") = EmisorDestino
                Row("Fechadestino") = FechaDestino
            End If
        Next

        Return True

    End Function
    Public Function ArmaDtChequesPropios(ByRef Dt As DataTable, ByVal Caja As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Blanco As Boolean, ByVal Negro As Boolean, ByVal ClaveCheque As Integer, ByVal Banco As Integer, ByVal Cuenta As Double, ByVal SoloSecos As Boolean) As Boolean

        Dim Sql1 As String = " CH.MedioPago = 2"
        If Caja <> 999 Then Sql1 = Sql1 & " AND CH.Caja = " & Caja
        If Format(Desde, "dd/MM/yyyy") <> "01/01/1000" Then
            Sql1 = Sql1 & " AND CH.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND CH.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "')"
        End If
        If ClaveCheque <> 0 Then
            Sql1 = Sql1 & " AND CH.ClaveCheque = " & ClaveCheque
        End If
        If Banco <> 0 Then
            Sql1 = Sql1 & " AND CH.Banco = " & Banco
        End If
        If Cuenta <> 0 Then
            Sql1 = Sql1 & " AND CH.Cuenta = " & Cuenta
        End If

        Dim SqlB As String
        Dim SqlN As String

        SqlB = "SELECT 1 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.FechaReciboOficial AS FechaDestino,3 AS Destino,NumeroFondoFijo FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Nota WHERE (C.TipoNota = 65 OR C.TipoNota = 64) AND " & Sql1 &
                   " UNION ALL " &
              "SELECT 1 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Fecha AS FechaDestino,2 AS Destino,NumeroFondoFijo FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Nota WHERE C.TipoNota = 600 AND " & Sql1 &
                   " UNION ALL " &
              "SELECT 1 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Fecha AS FechaDestino,C.Origen AS Destino,0 AS NumeroFondoFijo FROM CompraDivisasCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoMovimiento AND CH.CompDestino = C.Movimiento WHERE C.TipoMovimiento = 6000 AND " & Sql1 &
                   " UNION ALL " &
              "SELECT 1 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Fecha AS FechaDestino,C.Origen AS Destino,0 AS NumeroFondoFijo FROM PrestamosCabeza As C INNER JOIN (PrestamosMovimientoCabeza AS MC INNER JOIN Cheques AS CH ON CH.TipoDestino = MC.TipoNota AND CH.CompDestino = MC.Movimiento) ON C.Prestamo = MC.Prestamo WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 1 AS Operacion,CH.*,C.Legajo AS EmisorDestino,C.Fecha AS FechaDestino,4 AS Destino,0 AS NumeroFondoFijo FROM SueldosMovimientoCabeza AS C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Movimiento WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 1 AS Operacion,CH.*,C.Proveedor AS EmisorDestino,C.Fecha AS FechaDestino,5 AS Destino,0 AS NumeroFondoFijo FROM OtrosPagosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Movimiento WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 1 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Emision AS FechaDestino,C.Destino,0 AS NumeroFondoFijo FROM ChequesIniciales As C INNER JOIN Cheques AS CH ON C.MedioPago = CH.MedioPago AND CH.ClaveCheque = C.ClaveCheque WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 1 AS Operacion,CH.*,C.Banco As EmisorDestino,C.FechaComprobante AS FechaDestino,1 As Destino,0 AS NumeroFondoFijo FROM MovimientosBancarioCabeza AS C INNER JOIN Cheques AS CH ON CH.TipoDestino = 90 AND CH.CompDestino = C.Movimiento WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 1 AS Operacion,CH.*,C.FondoFijo As EmisorDestino,C.Fecha AS FechaDestino,6 As Destino,0 AS NumeroFondoFijo FROM MovimientosFondoFijoCabeza AS C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.Tipo AND CH.CompDestino = C.Movimiento WHERE C.Tipo = 7001 AND " & Sql1 & ";"



        SqlN = "SELECT 2 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.FechaReciboOficial AS FechaDestino,3 AS Destino,NumeroFondoFijo FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Nota WHERE (C.TipoNota = 65 OR C.TipoNota = 64) AND " & Sql1 &
                   " UNION ALL " &
              "SELECT 2 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Fecha AS FechaDestino,2 AS Destino,NumeroFondoFijo FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Nota WHERE C.TipoNota = 600 AND " & Sql1 &
                   " UNION ALL " &
              "SELECT 2 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Fecha AS FechaDestino,C.Origen AS Destino,0 AS NumeroFondoFijo FROM CompraDivisasCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoMovimiento AND CH.CompDestino = C.Movimiento WHERE C.TipoMovimiento = 6000 AND " & Sql1 &
                   " UNION ALL " &
              "SELECT 2 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Fecha AS FechaDestino,C.Origen AS Destino,0 AS NumeroFondoFijo FROM PrestamosCabeza As C INNER JOIN (PrestamosMovimientoCabeza AS MC INNER JOIN Cheques AS CH ON CH.TipoDestino = MC.TipoNota AND CH.CompDestino = MC.Movimiento) ON C.Prestamo = MC.Prestamo WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 2 AS Operacion,CH.*,C.Legajo AS EmisorDestino,C.Fecha AS FechaDestino,4 AS Destino,0 AS NumeroFondoFijo FROM SueldosMovimientoCabeza AS C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Movimiento WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 2 AS Operacion,CH.*,C.Proveedor AS EmisorDestino,C.Fecha AS FechaDestino,5 AS Destino,0 AS NumeroFondoFijo FROM OtrosPagosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Movimiento WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 2 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Emision AS FechaDestino,C.Destino,0 AS NumeroFondoFijo FROM ChequesIniciales As C INNER JOIN Cheques AS CH ON C.MedioPago = CH.MedioPago AND CH.ClaveCheque = C.ClaveCheque WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 2 AS Operacion,CH.*,C.Banco As EmisorDestino,C.FechaComprobante AS FechaDestino,1 As Destino,0 AS NumeroFondoFijo FROM MovimientosBancarioCabeza AS C INNER JOIN Cheques AS CH ON CH.TipoDestino = 90 AND CH.CompDestino = C.Movimiento WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 2 AS Operacion,CH.*,C.FondoFijo As EmisorDestino,C.Fecha AS FechaDestino,6 As Destino,0 AS NumeroFondoFijo FROM MovimientosFondoFijoCabeza AS C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.Tipo AND CH.CompDestino = C.Movimiento WHERE C.Tipo = 7001 AND " & Sql1 & ";"

        If SoloSecos Then
            SqlB = "SELECT 1 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.FechaReciboOficial AS FechaDestino,3 AS Destino,NumeroFondoFijo FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Nota WHERE C.Secos = 1 AND (C.TipoNota = 65 OR C.TipoNota = 64) AND " & Sql1 &
                       " UNION ALL " &
                  "SELECT 1 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Fecha AS FechaDestino,2 AS Destino,NumeroFondoFijo FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Nota WHERE C.Secos = 1 AND C.TipoNota = 600 AND " & Sql1 & ";"

            SqlN = "SELECT 2 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.FechaReciboOficial AS FechaDestino,3 AS Destino,NumeroFondoFijo FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Nota WHERE C.Secos = 1 AND (C.TipoNota = 65 OR C.TipoNota = 64) AND " & Sql1 &
                       " UNION ALL " &
                  "SELECT 2 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Fecha AS FechaDestino,2 AS Destino,NumeroFondoFijo  FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Nota WHERE C.Secos = 1 AND C.TipoNota = 600 AND " & Sql1 & ";"
        End If

        If Blanco Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Return False
        End If
        If Negro And PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Return False
        End If

        Return True

    End Function
    Public Function ArmaDtDebitosAutomaticosDiferido(ByRef Dt As DataTable, ByVal Caja As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Blanco As Boolean, ByVal Negro As Boolean, ByVal ClaveCheque As Integer, ByVal Banco As Integer, ByVal Cuenta As Double, ByVal SoloSecos As Boolean) As Boolean

        Dim Sql1 As String = " CH.MedioPago =14"
        If Caja <> 999 Then Sql1 = Sql1 & " AND CH.Caja = " & Caja
        If Format(Desde, "dd/MM/yyyy") <> "01/01/1000" Then
            Sql1 = Sql1 & " AND CH.Fecha >='" & Format(Desde, "yyyyMMdd") & "' AND CH.Fecha < DATEADD(dd,1,'" & Format(Hasta, "yyyyMMdd") & "')"
        End If
        If ClaveCheque <> 0 Then
            Sql1 = Sql1 & " AND CH.ClaveCheque = " & ClaveCheque
        End If
        If Banco <> 0 Then
            Sql1 = Sql1 & " AND CH.Banco = " & Banco
        End If
        If Cuenta <> 0 Then
            Sql1 = Sql1 & " AND CH.Cuenta = " & Cuenta
        End If

        Dim SqlB As String
        Dim SqlN As String

        SqlB = "SELECT 1 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Fecha AS FechaDestino,2 AS Destino FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Nota WHERE C.TipoNota = 600 AND " & Sql1 &
                   " UNION ALL " &
              "SELECT 1 AS Operacion,CH.*,C.Legajo AS EmisorDestino,C.Fecha AS FechaDestino,4 AS Destino FROM SueldosMovimientoCabeza AS C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Movimiento WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 1 AS Operacion,CH.*,C.Proveedor AS EmisorDestino,C.Fecha AS FechaDestino,5 AS Destino FROM OtrosPagosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Movimiento WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 1 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Emision AS FechaDestino,C.Destino FROM ChequesIniciales As C INNER JOIN Cheques AS CH ON C.MedioPago = CH.MedioPago AND CH.ClaveCheque = C.ClaveCheque WHERE " & Sql1 & ";"

        SqlN = "SELECT 2 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Fecha AS FechaDestino,2 AS Destino FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Nota WHERE C.TipoNota = 600 AND " & Sql1 &
                   " UNION ALL " &
              "SELECT 2 AS Operacion,CH.*,C.Legajo AS EmisorDestino,C.Fecha AS FechaDestino,4 AS Destino FROM SueldosMovimientoCabeza AS C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Movimiento WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 2 AS Operacion,CH.*,C.Proveedor AS EmisorDestino,C.Fecha AS FechaDestino,5 AS Destino FROM OtrosPagosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Movimiento WHERE " & Sql1 &
                   " UNION ALL " &
              "SELECT 2 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Emision AS FechaDestino,C.Destino FROM ChequesIniciales As C INNER JOIN Cheques AS CH ON C.MedioPago = CH.MedioPago AND CH.ClaveCheque = C.ClaveCheque WHERE " & Sql1 & ";"

        If SoloSecos Then
            SqlB = "SELECT 1 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Fecha AS FechaDestino,2 AS Destino FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Nota WHERE C.Secos = 1 AND C.TipoNota = 600 AND " & Sql1 & ";"
            SqlN = "SELECT 2 AS Operacion,CH.*,C.Emisor AS EmisorDestino,C.Fecha AS FechaDestino,2 AS Destino FROM RecibosCabeza As C INNER JOIN Cheques AS CH ON CH.TipoDestino = C.TipoNota AND CH.CompDestino = C.Nota WHERE C.Secos = 1 AND C.TipoNota = 600 AND " & Sql1 & ";"
        End If

        If Blanco Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Return False
        End If
        If Negro And PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Return False
        End If

        Return True

    End Function
    Public Function ArmaSqlCtaCteClienteB(ByVal Emisor As Integer) As String

        Dim SqlB As String

        SqlB = "SELECT 1 AS Operacion, 2 AS Tipo,0 AS Saldo,Fecha,Factura AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ACuenta,0 As ReciboOficial,0 As Tr FROM FacturasCabeza WHERE Tr = 0 AND Estado <> 3 AND Cliente = " & Emisor &
               " UNION ALL " &
               "SELECT 1 AS Operacion, 800 AS Tipo,0 AS Saldo,Fecha,Liquidacion AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ACuenta,ReciboOficial,0 As Tr FROM NVLPCabeza WHERE Estado = 1 AND Cliente = " & Emisor &
               " UNION ALL " &
               "SELECT 1 AS Operacion, TipoNota AS Tipo,Saldo,Fecha,Nota AS Comprobante,Importe,ChequeRechazado,ACuenta,ReciboOficial,Tr FROM RecibosCabeza WHERE Estado = 1 AND (TipoNota = 5 or TipoNota = 7 or TipoNota = 13005 or TipoNota = 13007 or TipoNota = 50 or TipoNota = 60 or TipoNota = 70 or TipoNota = 65) AND Emisor = " & Emisor &
               " UNION ALL " &
               "SELECT 1 AS Operacion, 4 AS Tipo,0 AS Saldo,NotasCreditoCabeza.Fecha,NotasCreditoCabeza.NotaCredito AS Comprobante,NotasCreditoCabeza.Importe AS Importe,0 AS ChequeRechazado,0 AS ACuenta,0 As ReciboOficial,0 As Tr FROM NotasCreditoCabeza,FacturasCabeza WHERE NotasCreditoCabeza.Estado <> 3 AND FacturasCabeza.Cliente = " & Emisor &
               " AND FacturasCabeza.Factura = NotasCreditoCabeza.Factura;"

        Return SqlB

    End Function
    Public Function ArmaSqlCtaCteClienteN(ByVal Emisor As Integer) As String

        Dim SqlN As String

        SqlN = "SELECT 2 AS Operacion, 2 AS Tipo,0 AS Saldo,Fecha,Factura AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ACuenta,0 As ReciboOficial,0 As Tr FROM FacturasCabeza WHERE Tr = 0 AND Estado <> 3 AND Cliente = " & Emisor &
               " UNION ALL " &
               "SELECT 2 AS Operacion, 800 AS Tipo,0 AS Saldo,Fecha,Liquidacion AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ACuenta,ReciboOficial,0 As Tr FROM NVLPCabeza WHERE Estado = 1 AND Cliente = " & Emisor &
               " UNION ALL " &
               "SELECT 2 AS Operacion, TipoNota AS Tipo,Saldo,Fecha,Nota AS Comprobante,Importe, ChequeRechazado,ACuenta,ReciboOficial,0 As Tr FROM RecibosCabeza WHERE Estado = 1 AND (TipoNota = 5 or TipoNota = 7 or TipoNota = 13005 or TipoNota = 13007 or TipoNota = 50 or TipoNota = 60 or TipoNota = 70 or TipoNota = 65) AND Emisor = " & Emisor &
               " UNION ALL " &
               "SELECT 2 AS Operacion, 4 AS Tipo,0 AS Saldo,NotasCreditoCabeza.Fecha AS Fecha,NotasCreditoCabeza.NotaCredito AS Comprobante,NotasCreditoCabeza.Importe AS Importe,0 AS ChequeRechazado,0 AS ACuenta,0 As ReciboOficial,0 As Tr FROM NotasCreditoCabeza,FacturasCabeza WHERE NotasCreditoCabeza.Estado <> 3 AND FacturasCabeza.Cliente = " & Emisor &
               " AND FacturasCabeza.Factura = NotasCreditoCabeza.Factura;"

        Return SqlN

    End Function
    Private Function HallaDatosDestino(ByVal TipoNota As Integer, ByVal Nota As Double, ByVal ConexionStr As String, ByRef Emisor As Integer, ByRef TipoEmisor As Integer, ByRef Fecha As Date) As Boolean

        Dim Dt As New DataTable

        Emisor = 0

        Select Case TipoNota
            Case 600, 65, 64
                If Not Tablas.Read("SELECT Emisor,Fecha,NumeroFondoFijo FROM RecibosCabeza WHERE TipoNota = " & TipoNota & " AND Nota = " & Nota & ";", ConexionStr, Dt) Then Return False
                If Dt.Rows.Count <> 0 Then
                    Emisor = Dt.Rows(0).Item("Emisor")
                    Fecha = Dt.Rows(0).Item("Fecha")
                    If TipoNota = 600 Then TipoEmisor = 2
                    If TipoNota = 65 Then TipoEmisor = 3
                    If TipoNota = 64 Then TipoEmisor = 3
                    If Dt.Rows(0).Item("NumeroFondoFijo") <> 0 Then TipoEmisor = 6
                End If
            Case 1010, 1015
                If Not Tablas.Read("SELECT C.Emisor,C.Origen,C.Fecha FROM PrestamosCabeza AS C INNER JOIN PrestamosMovimientoCabeza AS M ON C.Prestamo = M.Prestamo WHERE Movimiento = " & Nota & ";", ConexionStr, Dt) Then Return False
                If Dt.Rows.Count <> 0 Then
                    Emisor = Dt.Rows(0).Item("Emisor")
                    Fecha = Dt.Rows(0).Item("Fecha")
                    TipoEmisor = Dt.Rows(0).Item("Origen")
                End If
            Case 4010
                If Not Tablas.Read("SELECT Legajo,Fecha FROM SueldosMovimientoCabeza WHERE TipoNota = 4010 AND Movimiento = " & Nota & ";", ConexionStr, Dt) Then Return False
                If Dt.Rows.Count <> 0 Then
                    Emisor = Dt.Rows(0).Item("Legajo")
                    Fecha = Dt.Rows(0).Item("Fecha")
                    TipoEmisor = 4
                End If
            Case 5010
                If Not Tablas.Read("SELECT Proveedor,Fecha FROM OtrosPagosCabeza WHERE TipoNota = 5010 AND Movimiento = " & Nota & ";", ConexionStr, Dt) Then Return False
                If Dt.Rows.Count <> 0 Then
                    Emisor = Dt.Rows(0).Item("Proveedor")
                    Fecha = Dt.Rows(0).Item("Fecha")
                    TipoEmisor = 5
                End If
            Case 91
                If Not Tablas.Read("SELECT Banco,FechaComprobante FROM MovimientosBancarioCabeza AS C WHERE Movimiento = " & Nota & ";", ConexionStr, Dt) Then Return False
                If Dt.Rows.Count <> 0 Then
                    Emisor = Dt.Rows(0).Item("Banco")
                    Fecha = Dt.Rows(0).Item("FechaComprobante")
                    TipoEmisor = 1
                End If
            Case 6000
                If Not Tablas.Read("SELECT Emisor,Fecha,Origen FROM CompraDivisasCabeza WHERE TipoMovimiento = 6000 AND Movimiento = " & Nota & ";", ConexionStr, Dt) Then Return False
                If Dt.Rows.Count <> 0 Then
                    Emisor = Dt.Rows(0).Item("Emisor")
                    Fecha = Dt.Rows(0).Item("Fecha")
                    TipoEmisor = Dt.Rows(0).Item("Origen")
                End If
            Case 7001
                If Not Tablas.Read("SELECT FondoFijo,Fecha FROM MovimientosFondoFijoCabeza WHERE Movimiento = " & Nota & ";", ConexionStr, Dt) Then Return False
                If Dt.Rows.Count <> 0 Then
                    Emisor = Dt.Rows(0).Item("FondoFijo")
                    Fecha = Dt.Rows(0).Item("Fecha")
                    TipoEmisor = 6
                End If
        End Select

        Return True

    End Function
    Public Function HallaStockInsumo(ByVal Insumo As Integer, ByVal Deposito As Integer, ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT SUM(Stock) FROM StockInsumos WHERE Articulo = " & Insumo & " AND Deposito = " & Deposito & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then Return CDec(Ultimo)
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function HallaStockArticuloLogistico(ByVal Articulo As Integer, ByVal Deposito As Integer, ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT SUM(Stock) FROM StockArticulosLogisticos WHERE Articulo = " & Articulo & " AND Deposito = " & Deposito & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then Return CDec(Ultimo)
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function CostoPorInsumos(ByVal Costeo As Integer, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Integer

        Dim Dt As New DataTable

        ImporteConIva = 0
        ImporteSinIva = 0

        If Not Tablas.Read("SELECT Consumo,ImporteConIva,ImporteSinIva FROM ConsumosCabeza WHERE Estado = 1 AND Costeo = " & Costeo & ";", Conexion, Dt) Then Return -1
        For Each Row As DataRow In Dt.Rows
            ImporteConIva = ImporteConIva + CDec(Row("ImporteConIva"))
            ImporteSinIva = ImporteSinIva + CDec(Row("ImporteSinIva"))
        Next

        If Not PermisoTotal Then
            Dt.Dispose()
            Return 0
        End If

        Dt.Clear()
        If Not Tablas.Read("SELECT Consumo,ImporteConIva,ImporteSinIva FROM ConsumosCabeza WHERE Estado = 1 AND Costeo = " & Costeo & ";", ConexionN, Dt) Then Return -1
        For Each Row As DataRow In Dt.Rows
            ImporteConIva = ImporteConIva + CDec(Row("ImporteConIva"))
            ImporteSinIva = ImporteSinIva + CDec(Row("ImporteSinIva"))
        Next

        Dt.Dispose()

        Return 0

    End Function
    Public Function HallaCostoConsumo(ByRef DtCostoDetalle As DataTable, ByVal Insumo As Integer, ByVal ConexionStr As String, ByRef CostoConIva As Double, ByRef CostoSinIva As Double, ByRef ImporteB As Double) As Integer

        Dim CostoConIvaW As Double
        Dim CostoSinIvaW As Double
        Dim ImporteBW As Double
        Dim PrecioB As Double
        Dim PrecioN As Double
        Dim Iva As Double

        CostoConIva = 0
        CostoSinIva = 0
        ImporteB = 0

        For Each Row As DataRow In DtCostoDetalle.Rows
            CostoConIvaW = 0
            CostoSinIvaW = 0
            Dim Resul As Integer = CostoInsumoOCompra(Row("OrdenCompra"), Insumo, Row("Cantidad"), CostoConIvaW, CostoSinIvaW, ImporteBW, ConexionStr, PrecioB, PrecioN, Iva)
            CostoConIva = CostoConIva + CostoConIvaW
            CostoSinIva = CostoSinIva + CostoSinIvaW
            ImporteB = ImporteB + ImporteBW
            If Resul = -1 Then
                Return Resul
            End If
        Next

        Return 0

    End Function
    Public Function CostoInsumoOCompra(ByVal OrdenCompra As Double, ByVal Insumo As Integer, ByVal Cantidad As Double, ByRef CostoConIva As Double, ByRef CostoSinIva As Double, ByRef ImporteB As Double, ByVal ConexionStr As String, ByRef PrecioB As Double, ByRef PrecioN As Double, ByRef Iva As Double) As Integer

        Dim Dt As New DataTable

        CostoConIva = 0
        CostoSinIva = 0
        ImporteB = 0
        PrecioB = 0
        PrecioN = 0
        Iva = 0

        If Not Tablas.Read("SELECT Precio,Iva FROM OrdenCompraDetalle WHERE Orden = " & OrdenCompra & " AND Articulo = " & Insumo & ";", Conexion, Dt) Then Return -1
        If Dt.Rows.Count <> 0 Then
            CostoConIva = CalculaNeto(Cantidad, Dt.Rows(0).Item("precio")) + Trunca(CalculaIva(Cantidad, Dt.Rows(0).Item("precio"), Dt.Rows(0).Item("Iva")))
            CostoSinIva = CalculaNeto(Cantidad, Dt.Rows(0).Item("precio"))
            If ConexionStr = Conexion Then
                PrecioB = Dt.Rows(0).Item("Precio")
                Iva = Dt.Rows(0).Item("Iva")
                ImporteB = CostoSinIva
            Else
                PrecioN = Dt.Rows(0).Item("Precio")
            End If
        End If

        Dt.Dispose()
        Return 0

    End Function
    Public Function HallaTipoOperacion(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT TipoOperacion FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then Return CInt(Ultimo)
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function HallaTipoOperacion(ByVal Proveedor) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT TipoOperacion FROM Proveedores WHERE Clave = " & Proveedor & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then Return CInt(Ultimo)
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function HallaCostosPrecioKilos(ByVal Costeo As Integer, ByRef CostoConIva As Decimal, ByRef CostoSinIva As Decimal, ByRef PrecioEst As Decimal, ByRef Kilos As Decimal, ByRef Cerrado As Boolean, ByRef Iva As Decimal) As Integer

        CostoConIva = 0
        CostoSinIva = 0
        PrecioEst = 0
        Iva = 0
        Kilos = 0

        If CostoPorFacturasProveedores(Costeo, CostoConIva, CostoSinIva) < 0 Then Return -1

        Dim CostoConIvaN As Decimal
        Dim CostoSinIvaN As Decimal
        If CostoPorNotasCreditosDebitos(Costeo, CostoConIvaN, CostoSinIvaN) < 0 Then Return -1
        CostoConIva = CostoConIva + CostoConIvaN
        CostoSinIva = CostoSinIva + CostoSinIvaN

        Dim CostoConIvaW As Decimal
        Dim CostoSinIvaW As Decimal
        If CostoPorInsumos(Costeo, CostoConIvaW, CostoSinIvaW) < 0 Then Return -1
        CostoConIva = CostoConIva + CostoConIvaW
        CostoSinIva = CostoSinIva + CostoSinIvaW

        CostoConIvaW = 0
        CostoSinIvaW = 0
        If CostoPorAsientosManuales(Costeo, CostoConIvaW, CostoSinIvaW) < 0 Then Return -1
        CostoConIva = CostoConIva + CostoConIvaW
        CostoSinIva = CostoSinIva + CostoSinIvaW

        CostoConIvaW = 0
        CostoSinIvaW = 0
        If CostoPorConsumosTerminados(Costeo, CostoConIvaW, CostoSinIvaW) < 0 Then Return -1
        CostoConIva = CostoConIva + CostoConIvaW
        CostoSinIva = CostoSinIva + CostoSinIvaW

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Costo,Cerrado,Iva FROM Costeos WHERE Costeo = " & Costeo & ";", Conexion, Dt) Then Return -1
        If Dt.Rows.Count = 0 Then
            MsgBox("Costeo " & Costeo & " No Existe en Base de Datos.")
            Dt.Dispose()
            Return -1
        End If

        PrecioEst = Dt.Rows(0).Item("Costo")
        If GTipoIva <> 2 Then Iva = Dt.Rows(0).Item("Iva")
        Cerrado = Dt.Rows(0).Item("Cerrado")

        Kilos = CalculaKilosDelCosteo(Costeo)
        If Kilos < 0 Then Dt.Dispose() : Return -1

        Dt.Dispose()

        Return 0

    End Function
    Public Function HallaCostoInsumosPorLote(ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Integer

        Dim Dt As New DataTable
        Dim Item As ItemCostosAsignados

        GListaCostosInsumos = New List(Of ItemCostosAsignados)

        ImporteConIva = 0
        ImporteSinIva = 0

        Dt = New DataTable
        If Not Tablas.Read("SELECT Consumo,ImporteConIva,ImporteSinIva FROM ConsumosLotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", Conexion, Dt) Then Return -1
        For Each Row As DataRow In Dt.Rows
            ImporteConIva = ImporteConIva + Row("ImporteConIva")
            ImporteSinIva = ImporteSinIva + Row("ImporteSinIva")
            '
            If GConListaDeCostos Then
                Item = New ItemCostosAsignados
                Item.Operacion = 1
                Item.Nombre = "Consumo Insumo"
                Item.Comprobante = NumeroEditado(Row("Consumo"))
                Item.ImporteConIva = Row("ImporteConIva")
                Item.ImporteSinIva = Row("ImporteSinIva")
                GListaCostosInsumos.Add(Item)
            End If
        Next

        If Not PermisoTotal Then Dt.Dispose() : Return 0

        Dt = New DataTable
        If Not Tablas.Read("SELECT Consumo,ImporteConIva,ImporteSinIva FROM ConsumosLotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", ConexionN, Dt) Then Return -1
        For Each Row As DataRow In Dt.Rows
            ImporteConIva = ImporteConIva + Row("ImporteConIva")
            ImporteSinIva = ImporteSinIva + Row("ImporteSinIva")
            '
            If GConListaDeCostos Then
                Item = New ItemCostosAsignados
                Item.Operacion = 2
                Item.Nombre = "Consumo Insumo"
                Item.Comprobante = NumeroEditado(Row("Consumo"))
                Item.ImporteConIva = Row("ImporteConIva")
                Item.ImporteSinIva = Row("ImporteSinIva")
                GListaCostosInsumos.Add(Item)
            End If
        Next

        Dt.Dispose()

        Return 0

    End Function
    Public Function CostoPorFacturasProveedores(ByVal Costeo As Integer, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Integer

        Dim Sql As String
        Dim Dt As New DataTable

        ImporteConIva = 0
        ImporteSinIva = 0

        Sql = "SELECT D.Impuesto,D.Importe,C.Cambio FROM FacturasProveedorCabeza AS C INNER JOIN FacturasProveedorDetalle AS D ON C.Factura = D.Factura " &
                  "WHERE C.Estado = 1 AND C.Tr = 0 AND C.Costeo = " & Costeo & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then Return -1
        For Each Row As DataRow In Dt.Rows
            If Row("Impuesto") <> 10 Then
                If Row("Cambio") <> 1 Then Row("Importe") = Trunca(Row("Importe") * Row("Cambio"))
                ImporteConIva = ImporteConIva + CDec(Row("Importe"))
                If Row("Impuesto") = 1 Or Row("Impuesto") = 2 Then ImporteSinIva = ImporteSinIva + CDec(Row("Importe"))
            End If
        Next

        Dt = New DataTable

        Sql = "SELECT D.Impuesto,D.Importe,C.Cambio FROM FacturasProveedorCabeza AS C INNER JOIN FacturasProveedorDetalle AS D ON C.Factura = D.Factura " &
                  "WHERE C.Estado = 1 AND C.Tr = 0 AND D.Impuesto = 2 AND C.Costeo = " & Costeo & ";"
        If Not Tablas.Read(Sql, ConexionN, Dt) Then Return -1
        For Each Row As DataRow In Dt.Rows
            If Row("Cambio") <> 1 Then Row("Importe") = Trunca(Row("Importe") * Row("Cambio"))
            ImporteConIva = ImporteConIva + CDec(Row("Importe"))
            ImporteSinIva = ImporteSinIva + CDec(Row("Importe"))
        Next

        Dt.Dispose()

        Return 0

    End Function
    Public Function CostoPorNotasCreditosDebitos(ByVal Costeo As Integer, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Integer

        Dim Sql As String
        Dim Dt As New DataTable

        ImporteConIva = 0
        ImporteSinIva = 0

        Sql = "SELECT 1 As Operacion,TipoNota,Nota,Cambio FROM RecibosCabeza WHERE (TipoNota = 8 OR TipoNota = 13008 OR TipoNota = 500 OR TipoNota = 6 OR TipoNota = 13006 OR TipoNota = 700) AND Estado = 1 AND Costeo = " & Costeo & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then Return -1
        If PermisoTotal Then
            Sql = "SELECT 2 As Operacion,TipoNota,Nota,Cambio FROM RecibosCabeza WHERE (TipoNota = 8 OR TipoNota = 13008 OR TipoNota = 500 OR TipoNota = 6 OR TipoNota = 13006 OR TipoNota = 700) AND Estado = 1 AND Costeo = " & Costeo & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Return -1
        End If

        Dim NetoGrabado As Decimal = 0
        Dim NetoNoGrabado As Decimal = 0
        Dim ImporteRetencion As Decimal = 0
        Dim ImporteIva As Decimal = 0

        For Each Row As DataRow In Dt.Rows
            HallaTotalesImportesNotasDebitoCredito(Row("Operacion"), Row("TipoNota"), Row("Nota"), NetoGrabado, NetoNoGrabado, ImporteRetencion, ImporteIva, Row("Cambio"))
            Select Case Row("TipoNota")
                Case 8, 13008, 500
                    ImporteConIva = ImporteConIva + NetoGrabado + NetoNoGrabado + ImporteRetencion + ImporteIva
                    ImporteSinIva = ImporteSinIva + NetoGrabado + NetoNoGrabado
                Case 6, 13006, 700
                    ImporteConIva = ImporteConIva - NetoGrabado - NetoNoGrabado - ImporteRetencion - ImporteIva
                    ImporteSinIva = ImporteSinIva - NetoGrabado - NetoNoGrabado
            End Select
        Next

        Dt.Dispose()

        Return 0

    End Function
    Public Function CostoPorAsientosManuales(ByVal Costeo As Integer, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Integer

        Dim Sql As String
        Dim Dt As New DataTable

        ImporteConIva = 0
        ImporteSinIva = 0

        Sql = "SELECT 1 As Operacion,Asiento,Debito,Credito FROM AsientosCabeza WHERE Estado = 1 AND Costeo = " & Costeo & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then Return -1
        If PermisoTotal Then
            Sql = "SELECT 2 As Operacion,Asiento,Debito,Credito FROM AsientosCabeza WHERE Estado = 1 AND Costeo = " & Costeo & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then Return -1
        End If

        Dim ImporteW As Decimal = 0

        For Each Row As DataRow In Dt.Rows
            HallaTotalesImportesAsientosManuales(Row("Operacion"), Row("Asiento"), ImporteW)
            If Row("Debito") Then
                ImporteConIva = ImporteConIva + ImporteW
                ImporteSinIva = ImporteSinIva + ImporteW
            Else
                ImporteConIva = ImporteConIva - ImporteW
                ImporteSinIva = ImporteSinIva - ImporteW
            End If
        Next

        Dt.Dispose()

        Return 0

    End Function
    Public Function CostoPorConsumosTerminados(ByVal Costeo As Integer, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Integer

        Dim Dt As New DataTable

        ImporteConIva = 0
        ImporteSinIva = 0

        Dim SqlB As String = "SELECT 1 AS Operacion,Importe FROM ConsumosPTCabeza WHERE Estado <> 3 AND Costeo = " & Costeo & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion,Importe FROM ConsumosPTCabeza WHERE Estado <> 3 AND Costeo = " & Costeo & ";"

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Return -1
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Return -1
        End If

        For Each Row As DataRow In Dt.Rows
            ImporteConIva = ImporteConIva + Row("Importe")
            ImporteSinIva = ImporteSinIva + Row("Importe")
        Next

        Dt.Dispose()

        Return 0

    End Function
    Public Function CalculaKilosDelCosteo(ByVal Costeo As Integer) As Double

        Dim Cantidad As Double = 0

        Dim Sql As String = "SELECT SUM((L.Cantidad - L.Baja) * L.KilosXUnidad) AS Cantidad FROM Lotes AS L INNER JOIN IngresoMercaderiasCabeza AS C ON L.Lote = C.Lote WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND C.Costeo = " & Costeo & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Cant = Cmd.ExecuteScalar()
                    If Not IsDBNull(Cant) Then Cantidad = Cant
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

        If PermisoTotal Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                        Dim Cant = Cmd.ExecuteScalar()
                        If Not IsDBNull(Cant) Then Cantidad = Cantidad + Cant
                    End Using
                End Using
            Catch ex As Exception
                Return -1
            End Try
        End If

        Return Cantidad

    End Function
    Public Function HallaCantidadLote(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As Integer) As Decimal

        Dim ConexionLote As String

        If Operacion = 1 Then
            ConexionLote = Conexion
        Else : ConexionLote = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionLote)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT (Cantidad - Baja) AS Cantidad FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla de Lotes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function HallaCosteoLote(ByVal Operacion As Integer, ByVal Lote As Integer) As Integer

        Dim Dt As New DataTable
        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Costeo FROM IngresoMercaderiasCabeza WHERE Lote = " & Lote & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: Costeos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return -1
        End Try

    End Function
    Public Function HallaCosteoCerrado(ByVal Costeo As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cerrado FROM Costeos WHERE Costeo = " & Costeo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: Costeos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Sub HallaPartesCuenta(ByVal ClaveCuenta As Double, ByRef Centro As Integer, ByRef Cuenta As Integer, ByRef SubCuenta As Integer)

        Centro = 0
        Cuenta = 0
        SubCuenta = 0

        Dim ClaveCuentaStr As String = ClaveCuenta.ToString.PadLeft(11, "0")

        Centro = CInt(Strings.Left(ClaveCuentaStr, 3))
        Cuenta = CInt(Mid$(ClaveCuentaStr, 4, 6))
        SubCuenta = CInt(Strings.Right(ClaveCuentaStr, 2))

    End Sub
    Public Function NombreCentro(ByVal Centro As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM CentrosDeCosto WHERE Centro = " & Centro & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla de Centros.")
            End
        End Try

    End Function
    Public Function NombreCuenta(ByVal Cuenta As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Cuentas WHERE Cuenta = " & Cuenta & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla de Cuentas.")
            End
        End Try

    End Function
    Public Function NombreSubCuenta(ByVal SubCuenta As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM SubCuentas WHERE Clave = " & SubCuenta & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla de SubCuentas.")
            End
        End Try

    End Function
    Public Function FacturaAfectaLotes(ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Boolean

        Dim SqlB As String
        Dim SqlN As String
        Dim DtFacturas As DataTable
        Dim Item As ItemCostosAsignados

        GListaCostosAsignados = New List(Of ItemCostosAsignados)

        ImporteConIva = 0
        ImporteSinIva = 0

        ' Halla Importes por lotes afectados para facturas afecta costo lotes.
        SqlB = "SELECT 1 AS Operacion,L.ImporteConIva,L.ImporteSinIva,C.ReciboOficial,C.Factura FROM FacturasProveedorCabeza AS C INNER JOIN ComproFacturados AS L ON C.Factura = L.Factura " &
                  "WHERE C.EsAfectaCostoLotes = 1 AND C.Estado = 1 AND L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & ";"
        SqlN = "SELECT 2 AS Operacion,L.ImporteConIva,L.ImporteSinIva,C.ReciboOficial,C.Factura FROM FacturasProveedorCabeza AS C INNER JOIN ComproFacturados AS L ON C.Factura = L.Factura " &
                  "WHERE C.EsAfectaCostoLotes = 1 AND C.Estado = 1 AND L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & ";"

        DtFacturas = New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtFacturas) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtFacturas) Then Return False
        End If
        For Each Row1 As DataRow In DtFacturas.Rows
            ImporteConIva = ImporteConIva + Row1("ImporteConIva")
            ImporteSinIva = ImporteSinIva + Row1("ImporteSinIva")
            If GConListaDeCostos Then
                Item = New ItemCostosAsignados
                Item.Operacion = Row1("Operacion")
                Item.Nombre = "Factura Proveedor"
                Item.Comprobante = NumeroEditado(Row1("ReciboOficial"))
                Item.NroComprobante = Row1("Factura")
                Item.ImporteConIva = Row1("ImporteConIva")
                Item.ImporteSinIva = Row1("ImporteSinIva")
                GListaCostosAsignados.Add(Item)
            End If
        Next

        DtFacturas.Clear()

        ' Halla Importes por lotes afectados para Otras facturas proveedor.
        SqlB = "SELECT 1 AS Operacion,L.ImporteConIva,L.ImporteSinIva,C.ReciboOficial FROM FacturasProveedorCabeza AS C INNER JOIN ComproFacturados AS L ON C.Factura = L.Factura " &
                  "WHERE C.EsSinComprobante = 1 AND C.Estado = 1 AND L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & ";"
        SqlN = "SELECT 2 AS Operacion,L.ImporteConIva,L.ImporteSinIva,C.ReciboOficial FROM FacturasProveedorCabeza AS C INNER JOIN ComproFacturados AS L ON C.Factura = L.Factura " &
                  "WHERE C.EsSinComprobante = 1 AND C.Estado = 1 AND L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & ";"

        DtFacturas = New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtFacturas) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtFacturas) Then Return False
        End If
        For Each Row1 As DataRow In DtFacturas.Rows
            ImporteConIva = ImporteConIva + Row1("ImporteConIva")
            ImporteSinIva = ImporteSinIva + Row1("ImporteSinIva")
            If GConListaDeCostos Then
                Item = New ItemCostosAsignados
                Item.Operacion = Row1("Operacion")
                Item.Nombre = "Otras Factura Proveedor"
                Item.Comprobante = NumeroEditado(Row1("ReciboOficial"))
                Item.ImporteConIva = Row1("ImporteConIva")
                Item.ImporteSinIva = Row1("ImporteSinIva")
                GListaCostosAsignados.Add(Item)
            End If
        Next

        DtFacturas.Dispose()

        Return True

    End Function
    Public Function HallaClaveIva(ByVal Iva As Double) As Double

        CambiarPuntoDecimal(".")

        Dim Sql As String = "SELECT Clave FROM Tablas WHERE Iva = " & Iva & " AND Tipo = 22;"

        CambiarPuntoDecimal(",")

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function EsReemplazo(ByVal Abierto As Boolean, ByVal Recibo As Double) As Boolean

        Dim ConexionStr As String
        If Abierto Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ClaveChequeReemplazado FROM RecibosCabeza WHERE TipoNota = 600 AND Nota = " & Recibo & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then
                        Return True
                    Else : Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox("Error al Leer Tabla de Recibos")
            End
        End Try

    End Function
    Public Function ConvierteCuentaStr(ByVal CuentaStr As String) As Double

        If CuentaStr = "" Then Return 0

        Dim Str As String = CuentaStr.PadLeft(13)

        Return Val(Mid(Str, 1, 3) & Mid(Str, 5, 6) & Mid(Str, 12, 2))

    End Function
    Public Function HallaNombreEmisor(ByVal Tipo As Integer, ByVal Emisor As Integer) As String

        Select Case Tipo
            Case 1
                Return NombreBanco(Emisor)
            Case 2
                Return NombreProveedor(Emisor)
            Case 3
                Return NombreCliente(Emisor)
            Case 5
                Return NombreDestino(Emisor)
            Case 6
                Return NombreProveedorFondoFijo(Emisor)
            Case Else
                MsgBox("No Existe Codigo Origen del Cheque.")
        End Select

    End Function
    Public Function HallaNombreDestino(ByVal Tipo As Integer, ByVal Emisor As Integer) As String

        Select Case Tipo
            Case 1
                Return NombreBanco(Emisor)
            Case 2
                Return NombreProveedor(Emisor)
            Case 3
                Return NombreCliente(Emisor)
            Case 4
                If Emisor < 5000 Then
                    Return NombreLegajo(Emisor, Conexion)
                Else : Return NombreLegajo(Emisor, ConexionN)
                End If
            Case 5
                Return NombreDestino(Emisor)
            Case 6
                Return NombreProveedorFondoFijo(Emisor)
            Case Else
                MsgBox("No Existe Codigo Destino del Cheque.")
        End Select

    End Function
    Public Sub HallaTipoImporte(ByVal TipoNota As Integer, ByVal TipoPase As Integer, ByRef DebW As Double, ByRef CredW As Double, ByVal ImporteW As Double)

        DebW = 0 : CredW = 0

        Select Case TipoNota
            Case 60, 90, 1000, 1015, 444, 66, 7002, 11, 12, 604, 5020
                DebW = ImporteW
            Case 600, 65, 91, 1010, 4010, 5010, 7001, 64
                CredW = ImporteW
            Case 80
                If TipoPase = 1 Then
                    CredW = ImporteW
                Else
                    DebW = ImporteW
                End If
            Case 6000
                If TipoPase = 0 Then
                    DebW = ImporteW
                Else
                    CredW = ImporteW
                End If
            Case 6001
                If TipoPase = 0 Then
                    CredW = ImporteW
                Else
                    DebW = ImporteW
                End If
            Case Else
                MsgBox("Codigo Nota en detalle caja No Contemplado")
        End Select

    End Sub
    Public Function HallaPrecioYCentroCosteo(ByVal Emisor As Integer, ByVal Costeo As Integer, ByRef Centro As Integer, ByRef Costo As Double) As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Costo,Cerrado FROM Costeos WHERE Costeo = " & Costeo & ";", Conexion, Dt) Then Return False
        Costo = Dt.Rows(0).Item("Costo")
        If Costo = 0 Then
            MsgBox("Falta Costo en Costeo. Operación se CANCELA.")
            Return False
        End If
        If Dt.Rows(0).Item("Cerrado") Then
            MsgBox("Costeo Cerrado. Operación se CANCELA.")
            Return False
        End If

        Dt.Clear()
        If Not Tablas.Read("SELECT Centro FROM Proveedores WHERE Clave = " & Emisor & ";", Conexion, Dt) Then Return False
        Centro = Dt.Rows(0).Item("Centro")

        Dt.Dispose()

        Return True

    End Function
    Public Function HallaProveedorLote(ByVal Operacion As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer) As Integer

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Proveedor FROM Lotes WHERE Lote = " & Lote & "AND Secuencia = " & Secuencia & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function EsRetencionPorProvincia(ByVal Clave As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT EsPorProvincia FROM Tablas WHERE Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos, al leer Tablas.")
            End
        End Try

    End Function
    Public Function HallaCuitPais(ByVal Pais As Integer) As Double

        If Pais = 1 Then Return 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cuit FROM Tablas WHERE Tipo = 28 AND Clave = " & Pais & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function MaskedOK(ByVal Mask As MaskedTextBox) As Boolean

        If Len(Mask.Text) <> 12 Then Return False
        If Val(Strings.Right(Mask.Text, 8)) = 0 Then Return False
        If Val(Strings.Left(Mask.Text, 4)) = 0 Then Return False

        Return True

    End Function
    Public Function MaskedOK2(ByRef Mask As MaskedTextBox) As Boolean

        Mask.Text = Mask.Text.Replace(" ", "0")
        Dim Longui As Integer = 12 - Mask.Text.Length
        Dim Str As String = ""
        For i As Integer = 1 To Longui
            Str = Str & "0"
        Next
        If Longui > 0 Then Mask.Text = Mask.Text & Str

        If Val(Strings.Right(Mask.Text, 8)) = 0 Then Return False
        If Val(Strings.Left(Mask.Text, 4)) = 0 Then Return False

        Return True

    End Function
    Public Function ConsisteFecha(ByRef Fecha As String) As Boolean

        Dim Partes() As String

        If Fecha.Length = 8 And IsNumeric(Fecha) Then
            Fecha = Mid(Fecha, 1, 2) & "/" & Mid(Fecha, 3, 2) & "/" & Mid(Fecha, 5, 4)
        End If

        Partes = Split(Fecha, "/")
        If Partes.Length < 3 Then Return False
        '
        If Not IsNumeric(Partes(0)) Then Return False
        If Partes(0).Length = 1 Then Partes(0) = "0" & Partes(0)
        If Val(Partes(0)) > 31 Or Val(Partes(0)) < 1 Then Return False
        '
        If Not IsNumeric(Partes(1)) Then Return False
        If Partes(1).Length = 1 Then Partes(1) = "0" & Partes(1)
        If Val(Partes(1)) > 12 Or Val(Partes(1)) < 1 Then Return False
        '
        If Not IsNumeric(Partes(2)) Then Return False
        If Val(Partes(2)) < 1900 Then Return False
        '
        Dim FechaW As New DateTimePicker
        Try
            FechaW.Value = Fecha
        Catch ex As Exception
            Return False
        End Try

        Return True

    End Function
    Public Function HallaCodigoRetencion(ByVal Clave As Integer) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT CodigoRetencion FROM Tablas WHERE Clave = " & Clave & " AND Tipo = 25;", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Código de Retención en Tablas.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function HallaTipoIvaCliente(ByVal Cliente As Integer) As Integer

        Dim TipoIva As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT TipoIva FROM Clientes WHERE Clave = " & Cliente & ";", Miconexion)
                    TipoIva = Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

        'If TipoIva = 6 Then TipoIva = 2  'modificacion se anula, retorna 6
        Return TipoIva

    End Function
    Public Function HallaTipoIvaProveedor(ByVal Proveedor As Integer) As Integer

        Dim TipoIva As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT TipoIva FROM Proveedores WHERE Clave = " & Proveedor & ";", Miconexion)
                    TipoIva = Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

        If TipoIva = 6 Then TipoIva = 2 'Convierte a un MONOTRIBUTO en EXENTO.
        Return TipoIva

    End Function
    Public Function HallaTipoIvaOtroProveedor(ByVal Proveedor As Integer) As Integer

        Dim TipoIva As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT TipoIva FROM OtrosProveedores WHERE Clave = " & Proveedor & ";", Miconexion)
                    TipoIva = Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

        If TipoIva = 6 Then TipoIva = 2
        Return TipoIva

    End Function

    Public Function ActualizaStockAsignados(ByVal DtAsignacionLotesW As DataTable, ByRef DtStock As DataTable, ByVal ListaDeLotesW As List(Of FilaAsignacion), ByVal Operacion As Integer) As Boolean

        Dim Diferencia As Decimal
        Dim RowsBusqueda() As DataRow
        Dim ConexionStr As String
        Dim ListaAux As New List(Of FilaAsignacion)

        For Each Fila As FilaAsignacion In ListaDeLotesW
            If Fila.Operacion = Operacion Then
                Dim Linea As New FilaAsignacion
                Linea.Indice = Fila.Indice
                Linea.Lote = Fila.Lote
                Linea.Secuencia = Fila.Secuencia
                Linea.Deposito = Fila.Deposito
                Linea.Asignado = Fila.Asignado
                ListaAux.Add(Linea)
            End If
        Next

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT * FROM Lotes WHERE Lote = " & 0 & " AND Secuencia = " & 0 & " AND Deposito = " & 0 & ";", ConexionStr, DtStock) Then Return False

        For Each Fila As FilaAsignacion In ListaAux
            RowsBusqueda = DtAsignacionLotesW.Select("Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
            If RowsBusqueda.Length <> 0 Then
                If RowsBusqueda(0).Item("Cantidad") <> Fila.Asignado Then
                    Diferencia = Fila.Asignado - CDec(RowsBusqueda(0).Item("Cantidad"))
                    RowsBusqueda = DtStock.Select("Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
                    If RowsBusqueda.Length = 0 Then
                        If Not Tablas.Read("SELECT * FROM Lotes WHERE Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & Fila.Deposito & ";", ConexionStr, DtStock) Then Return False
                    End If
                    RowsBusqueda = DtStock.Select("Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
                    RowsBusqueda(0).Item("Stock") = CDec(RowsBusqueda(0).Item("Stock")) - Diferencia
                    If RowsBusqueda(0).Item("Stock") < 0 Then
                        MsgBox("Asignación supera Stock en Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & ". Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                        Return False
                    End If
                End If
            Else
                RowsBusqueda = DtStock.Select("Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
                If RowsBusqueda.Length = 0 Then
                    If Not Tablas.Read("SELECT * FROM Lotes WHERE Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & Fila.Deposito & ";", ConexionStr, DtStock) Then Return False
                End If
                RowsBusqueda = DtStock.Select("Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
                RowsBusqueda(0).Item("Stock") = CDec(RowsBusqueda(0).Item("Stock")) - Fila.Asignado
                If RowsBusqueda(0).Item("Stock") < 0 Then
                    MsgBox("Asignación supera Stock en Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & ". Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
        Next
        For Each Row As DataRow In DtAsignacionLotesW.Rows
            If Row("Operacion") = Operacion Then
                If Not ExisteEnListaAsignacionIndiceLotes(ListaDeLotesW, Row("Indice"), Row("Lote"), Row("Secuencia")) Then
                    RowsBusqueda = DtStock.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
                    If RowsBusqueda.Length = 0 Then
                        If Not Tablas.Read("SELECT * FROM Lotes WHERE Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito") & ";", ConexionStr, DtStock) Then Return False
                    End If
                    RowsBusqueda = DtStock.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
                    RowsBusqueda(0).Item("Stock") = CDec(RowsBusqueda(0).Item("Stock")) + CDec(Row("Cantidad"))
                End If
            End If
        Next

        Return True

    End Function
    Public Function ExisteEnListaAsignacionIndiceLotes(ByVal ListaDeLotesW As List(Of FilaAsignacion), ByVal Indice As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer) As Boolean

        For Each Fila As FilaAsignacion In ListaDeLotesW
            If Fila.Indice = Indice And Fila.Lote = Lote And Fila.Secuencia = Secuencia Then Return True
        Next

        Return False

    End Function
    Public Sub AsignaLotesDeFacturasVenta(ByVal DtCabeza As DataTable, ByVal DtCabezaRel As DataTable, ByVal ListaDeLotes As List(Of FilaAsignacion), ByRef DtAsignacionLotesAux As DataTable, ByVal DtDetalle As DataTable, ByVal Factura As Double)

        Dim RowsBusqueda() As DataRow
        Dim RowsBusqueda1() As DataRow

        For Each Fila As FilaAsignacion In ListaDeLotes
            RowsBusqueda = DtAsignacionLotesAux.Select("Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Cantidad") = Fila.Asignado
                RowsBusqueda1 = DtDetalle.Select("Indice = " & Fila.Indice)
                If RowsBusqueda1.Length <> 0 Then  'Si fue copiado de un remito puede ser que una devolucion en remito lo halla dejado en 0 y al copia la asignacion de la factura no lo encuentre en detalle.
                    RowsBusqueda(0).Item("ImporteSinIva") = CalculaNeto(Fila.Asignado, RowsBusqueda1(0).Item("Precio"))
                    RowsBusqueda(0).Item("Importe") = Trunca(RowsBusqueda(0).Item("ImporteSinIva") + CalculaIva(Fila.Asignado, RowsBusqueda1(0).Item("Precio"), RowsBusqueda1(0).Item("Iva")))
                    If DtCabeza.Rows(0).Item("Moneda") <> 1 Then
                        RowsBusqueda(0).Item("ImporteSinIva") = Trunca(RowsBusqueda(0).Item("ImporteSinIva") * DtCabeza.Rows(0).Item("Cambio"))
                        RowsBusqueda(0).Item("Importe") = Trunca(RowsBusqueda(0).Item("Importe") * DtCabeza.Rows(0).Item("Cambio"))
                    End If
                End If
            Else
                Dim Row As DataRow = DtAsignacionLotesAux.NewRow()
                Row("TipoComprobante") = 2
                Row("Comprobante") = Factura
                Row("Indice") = Fila.Indice
                Row("Lote") = Fila.Lote
                Row("Secuencia") = Fila.Secuencia
                Row("Deposito") = Fila.Deposito
                Row("Operacion") = Fila.Operacion
                Row("Cantidad") = Fila.Asignado
                If DtCabezaRel.Rows.Count <> 0 Then
                    Row("Rel") = True
                Else : Row("Rel") = False
                End If
                RowsBusqueda = DtDetalle.Select("Indice = " & Fila.Indice)
                If RowsBusqueda.Length <> 0 Then ' igual = 0 es el caso en que hubo una devolucion por todo lo asignado.
                    Row("ImporteSinIva") = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                    Row("Importe") = Trunca(Row("ImporteSinIva") + CalculaIva(Fila.Asignado, RowsBusqueda(0).Item("Precio"), RowsBusqueda(0).Item("Iva")))
                    If DtCabeza.Rows(0).Item("Moneda") <> 1 Then
                        Row("ImporteSinIva") = Trunca(Row("ImporteSinIva") * DtCabeza.Rows(0).Item("Cambio"))
                        Row("Importe") = Trunca(Row("Importe") * DtCabeza.Rows(0).Item("Cambio"))
                    End If
                Else
                    Row("ImporteSinIva") = 0
                    Row("Importe") = 0
                End If
                Row("Facturado") = False
                Row("Liquidado") = False
                DtAsignacionLotesAux.Rows.Add(Row)
            End If
        Next
        For Each Row As DataRow In DtAsignacionLotesAux.Rows
            '            If Not ExisteEnListaAsignacionIndiceLotes(ListaDeLotes, Row("Indice"), Row("Lote"), Row("Secuencia")) Then Row.Delete()
            If Not ExisteEnListaAsignacionIndiceLotes(ListaDeLotes, Row("Indice"), Row("Lote"), Row("Secuencia")) Then
                Row("Cantidad") = 0
                Row("ImporteSinIva") = 0
                Row("Importe") = 0
            End If
        Next

    End Sub
    Public Function HallaPermisoImp(ByVal Operacion As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Deposito As Integer) As String

        Dim ConexionStr As String
        Dim Dt As New DataTable
        Dim Permiso As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT LoteOrigen,SecuenciaOrigen,DepositoOrigen,PermisoImp FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & " AND Deposito = " & Deposito & ";", ConexionStr, Dt) Then Return -1
        If Dt.Rows.Count = 0 Then Return -1
        If Lote = Dt.Rows(0).Item("LoteOrigen") And Secuencia = Dt.Rows(0).Item("SecuenciaOrigen") And Deposito = Dt.Rows(0).Item("DepositoOrigen") Then
            Permiso = Dt.Rows(0).Item("PermisoImp")
            Dt.Dispose()
            Return Permiso
        Else
            Lote = Dt.Rows(0).Item("LoteOrigen")
            Secuencia = Dt.Rows(0).Item("SecuenciaOrigen")
            Deposito = Dt.Rows(0).Item("DepositoOrigen")
            Dt.Clear()
            If Not Tablas.Read("SELECT PermisoImp FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & " AND Deposito = " & Deposito & ";", ConexionStr, Dt) Then Return -1
            If Dt.Rows.Count = 0 Then Return -1
            Permiso = Dt.Rows(0).Item("PermisoImp")
            Dt.Dispose()
            Return Permiso
        End If

        Return -1

    End Function
    Public Function HallaRemitoGuia(ByVal Lote As Integer, ByVal Operacion As Integer, ByRef RemitoGuia As Double) As Boolean

        Dim Dt As New DataTable
        Dim ConexionStr As String

        RemitoGuia = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT Remito,Guia FROM IngresoMercaderiasCabeza WHERE Lote = " & Lote & ";", ConexionStr, Dt) Then Return False
        If Dt.Rows.Count = 0 Then
            MsgBox("Error, Lote " & Lote & " NO Encontrada.")
            Return False
        End If

        If Dt.Rows(0).Item("Remito") <> 0 Then
            RemitoGuia = Dt.Rows(0).Item("Remito")
        Else : RemitoGuia = Dt.Rows(0).Item("Guia")
        End If

        Dt.Dispose()

        Return True

    End Function
    Public Function HallaDepositoLote(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As Integer) As Integer

        Dim ConexionStr As String
        Dim Sql As String = "SELECT Deposito FROM IngresoMercaderiasCabeza WHERE Lote = " & Lote & ";"

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Sub AceraTabla(ByVal Tabla() As ItemIvaReten)

        For I As Integer = 1 To UBound(Tabla)
            Tabla(I).Base = 0
            Tabla(I).Importe = 0
        Next

    End Sub
    Public Function EsChequeOK(ByVal TipoNota As Integer, ByVal MedioPago As Integer, ByVal ClaveCheque As Integer, ByVal ConexionStr As String) As Boolean

        If ClaveCheque = 0 Then Return True
        If MedioPago <> 3 Then Return True
        '    If TipoNota = 600 Then Return True

        Dim Dt As New DataTable
        Dim Ok As Boolean = True


        If Not Tablas.Read("SELECT Estado,Fecha FROM Cheques WHERE MedioPago = 3 AND ClaveCheque = " & ClaveCheque & ";", ConexionStr, Dt) Then End
        If Dt.Rows(0).Item("Estado") <> 1 Then Ok = False
        If ChequeVencido(Dt.Rows(0).Item("Fecha"), Now) Then Ok = False
        Dt.Dispose()

        Return Ok

    End Function
    Public Sub HallaDatosPrestamo(ByVal Movimiento As Double, ByRef Cuit As Double, ByRef Nombre As String)

        Dim Dt As New DataTable
        Dim Origen As Integer
        Dim Emisor As Integer

        Cuit = 0 : Nombre = ""

        Dim Sql As String = "SELECT C.Emisor,C.Origen FROM PrestamosCabeza as C INNER JOIN PrestamosMovimientoCabeza As M ON C.Prestamo = M.Prestamo WHERE M.Movimiento = " & Movimiento & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        Origen = Dt.Rows(0).Item("Origen")
        Emisor = Dt.Rows(0).Item("Emisor")

        Dt = New DataTable
        Select Case Origen
            Case 1
                If Not Tablas.Read("SELECT Nombre,Cuit FROM Tablas WHERE Tipo = 26 AND Clave = " & Emisor & ";", Conexion, Dt) Then End
            Case 2
                If Not Tablas.Read("SELECT Nombre,Cuit FROM Proveedores WHERE Clave = " & Emisor & ";", Conexion, Dt) Then End
            Case 3
                If Not Tablas.Read("SELECT Nombre,Cuit FROM Clientes WHERE Clave = " & Emisor & ";", Conexion, Dt) Then End
        End Select

        Cuit = Dt.Rows(0).Item("Cuit")
        Nombre = Dt.Rows(0).Item("Nombre")

        Dt.Dispose()

    End Sub
    Public Function HallaPrestamoManual(ByVal conexionStr As String, ByVal Emisor As Integer) As Integer

        Dim Prestamo As Integer

        OpcionNumero.PEsNumeroConPuntoVenta = True
        OpcionNumero.ShowDialog()
        If OpcionNumero.PRegresar Then
            Prestamo = 0
        Else
            Prestamo = OpcionNumero.PNumero
        End If
        OpcionNumero.Dispose()

        Dim Sql As String = "SELECT Prestamo FROM PrestamosCabeza WHERE Prestamo = " & Prestamo & " AND Emisor = " & Emisor & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(conexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        MsgBox("Prestamo No Existe.", MsgBoxStyle.Critical)
                        Return 0
                    Else
                        Return Prestamo
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer PrestamosCabeza.", MsgBoxStyle.Critical)
            Return "-1"
        End Try

    End Function
    Public Function HallaDireccionSucursalCliente(ByVal Cliente As Integer, ByVal Clave As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Direccion FROM SucursalesClientes WHERE Estado = 1 AND Cliente = " & Cliente & " AND Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Sucursales Clientes.", MsgBoxStyle.Critical)
            Return -1
        End Try

    End Function
    Public Function HallaComentarioSucursalCliente(ByVal Cliente As Integer, ByVal Clave As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Comentario FROM SucursalesClientes WHERE Estado = 1 AND Cliente = " & Cliente & " AND Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Sucursales Clientes.", MsgBoxStyle.Critical)
            Return -1
        End Try

    End Function
    Public Function HallaCodigoCliente(ByVal Cliente As Integer, ByVal Articulo As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT CodigoDeCliente FROM CodigosCliente WHERE Cliente = " & Cliente & " AND Articulo = " & Articulo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Codigos del Cliente.", MsgBoxStyle.Critical)
            Return -1
        End Try

    End Function
    Public Sub HallaCantidadYPrecioPedido(ByVal Pedido As Integer, ByVal Articulo As Integer, ByRef Cantidad As Decimal, ByRef Entregada As Decimal, ByRef Precio As Decimal, ByRef TipoPrecio As Integer, ByRef ArticuloExisteEnPedido As Boolean)

        Dim Dt As New DataTable

        Cantidad = 0
        Entregada = 0
        Precio = 0
        TipoPrecio = 0
        ArticuloExisteEnPedido = False

        Dim Sql As String = "SELECT Cantidad,Entregada,Precio,TipoPrecio FROM PedidosDetalle WHERE Pedido = " & Pedido & " AND Articulo = " & Articulo & ";"

        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        If Dt.Rows.Count <> 0 Then ArticuloExisteEnPedido = True
        For Each Row As DataRow In Dt.Rows
            Cantidad = Row("Cantidad")
            Entregada = Row("Entregada")
            Precio = Row("Precio")
            TipoPrecio = Row("TipoPrecio")
        Next

        Dt.Dispose()

    End Sub
    Public Sub HallaCantidadYPrecioIngreso(ByVal Ingreso As Integer, ByVal StrConexion As String, ByVal Articulo As Integer, ByRef Cantidad As Decimal)

        Dim Dt As New DataTable

        Cantidad = 0

        Dim Sql As String = "SELECT (Cantidad - Baja) AS Cantidad FROM Lotes WHERE Secuencia < 100 AND Lote  = " & Ingreso & " AND Articulo = " & Articulo & ";"

        If Not Tablas.Read(Sql, StrConexion, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Row("Cantidad")
        Next

        Dt.Dispose()

    End Sub
    Public Function CierreContableCerrado(ByVal Mes As Integer, ByVal Anio As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cerrado FROM CierreContable WHERE Mes = " & Mes & " AND Anio = " & Anio & ";", Miconexion)
                    Return Cmd.ExecuteScalar
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Cierre Contable.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function UltimaFechacontable(ByVal ConexionStr As String, ByVal Tipo As Integer) As Date

        Dim Sql As String = ""

        If Tipo = 1 Then Sql = "SELECT MAX(FechaContable) FROM NVLPCabeza;"
        If Tipo = 2 Then Sql = "SELECT MAX(FechaContable) FROM LiquidacionCabeza;"
        If Tipo = 3 Then Sql = "SELECT MAX(FechaContable) FROM FacturasProveedorCabeza;"
        If Tipo = 4 Then Sql = "SELECT MAX(FechaContable) FROM FacturasCabeza;"
        If Tipo = 5 Then Sql = "SELECT MAX(FechaContable) FROM RecibosCabeza;"
        If Tipo = 6 Then Sql = "SELECT MAX(FechaContable) FROM NotasCreditoCabeza;"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return "1/1/1000"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "2/1/1000"
        End Try

    End Function
    Public Function UltimaFechacontableDebitoCredito(ByVal ConexionStr As String, ByVal PuntoDeVenta As Integer, ByVal TipoIva As Integer, ByVal TipoNota As Integer) As Date

        Dim Sql As String = ""
        Dim Patron As String = TipoIva & Format(PuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Sql = "SELECT MAX(FechaContable) FROM RecibosCabeza WHERE TipoNota = " & TipoNota & " AND Estado <> 3 AND CAST(CAST(RecibosCabeza.Nota AS numeric) as char)LIKE '" & Patron & "';"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return "1/1/1000"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "2/1/1000"
        End Try

    End Function
    Public Function UltimaFechacontableFactura(ByVal ConexionStr As String, ByVal PuntoDeVenta As Integer, ByVal TipoIva As Integer) As Date

        Dim Sql As String = ""
        Dim Patron As String = TipoIva & Format(PuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Sql = "SELECT MAX(FechaContable) FROM FacturasCabeza WHERE EsElectronica = 1 AND Estado <> 3 AND CAST(CAST(FacturasCabeza.Factura AS numeric) as char)LIKE '" & Patron & "';"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return "1/1/1000"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "2/1/1000"
        End Try

    End Function
    Public Function UltimaFechacontableNotaCreditoDevolucion(ByVal ConexionStr As String, ByVal PuntoDeVenta As Integer, ByVal TipoIva As Integer) As Date

        Dim Sql As String = ""
        Dim Patron As String = TipoIva & Format(PuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Sql = "SELECT MAX(FechaContable) FROM NotasCreditoCabeza WHERE EsElectronica = 1 AND Estado <> 3 AND CAST(CAST(NotasCreditoCabeza.NotaCredito AS numeric) as char)LIKE '" & Patron & "';"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return "1/1/1000"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "2/1/1000"
        End Try

    End Function
    Public Function UltimaFechacontableLiquidacion(ByVal ConexionStr As String, ByVal PuntoDeVenta As Integer, ByVal TipoIva As Integer) As Date

        Dim Sql As String = ""
        '        Dim Patron As String = "[0-9]" & Format(PuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Dim Patron As String = TipoIva & Format(PuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Sql = "SELECT MAX(FechaContable) FROM LiquidacionCabeza WHERE Estado <> 3 AND CAST(CAST(LiquidacionCabeza.Liquidacion AS numeric) as char)LIKE '" & Patron & "';"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return "1/1/1000"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "2/1/1000"
        End Try

    End Function
    Public Function LiquidacionTrucha(ByVal ConexionStr As String, ByVal Liquidacion As Double) As Boolean

        Dim Sql As String = ""

        Sql = "SELECT Tr FROM LiquidacionCabeza WHERE Liquidacion = " & Liquidacion & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error al Leer Tabla Liquidacion Cabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function ExiteChequeEnPaseCaja(ByVal ConexionStr As String, ByVal ClaveCheque As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT D.ClaveCheque FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS D ON C.Pase = D.Pase WHERE C.Aceptado = 0 AND D.MedioPago = 3 AND D.ClaveCheque = " & ClaveCheque & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else : Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function AgregaIvaComision(ByVal Directo As Decimal) As Decimal

        If Directo = 0 Or GTipoIva = 2 Then Return 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT IvaComision FROM DatosEmpresa WHERE Indice = 1;", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Dato Empresa.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function AgregaIvaDescarga(ByVal Directo As Double) As Double

        If Directo = 0 Or GTipoIva = 2 Then Return 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT IvaDescarga FROM DatosEmpresa WHERE Indice = 1;", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Dato Empresa.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Sub ArmaLotesParaModificar(ByRef Lista As List(Of FilaAsignacion), ByVal DtAsignacionLotes As DataTable, ByVal ListaDeLotes As List(Of FilaAsignacion))

        Dim RowsBusqueda() As DataRow
        Dim Saldo As Integer

        For Each Fila As FilaAsignacion In ListaDeLotes
            RowsBusqueda = DtAsignacionLotes.Select("Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
            If RowsBusqueda.Length <> 0 Then
                Saldo = Fila.Asignado - RowsBusqueda(0).Item("Cantidad")
                If Saldo <> 0 Then
                    Dim fila2 As New FilaAsignacion
                    fila2.Operacion = Fila.Operacion
                    fila2.Indice = Fila.Indice
                    fila2.Lote = Fila.Lote
                    fila2.Secuencia = Fila.Secuencia
                    fila2.Asignado = Saldo
                    Lista.Add(fila2)
                End If
            Else
                Dim fila2 As New FilaAsignacion
                fila2.Operacion = Fila.Operacion
                fila2.Indice = Fila.Indice
                fila2.Lote = Fila.Lote
                fila2.Secuencia = Fila.Secuencia
                fila2.Asignado = Fila.Asignado
                Lista.Add(fila2)
            End If
        Next

        For Each Row As DataRow In DtAsignacionLotes.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Not ExisteEnListaAsignacionIndiceLotes(ListaDeLotes, Row("Indice"), Row("Lote"), Row("Secuencia")) Then
                    Dim fila2 As New FilaAsignacion
                    fila2.Operacion = Row("Operacion")
                    fila2.Indice = Row("Indice")
                    fila2.Lote = Row("Lote")
                    fila2.Secuencia = Row("Secuencia")
                    fila2.Asignado = -Row("cantidad")
                    Lista.Add(fila2)
                End If
            End If
        Next

    End Sub
    Public Function NombreEspecie(ByVal Especie As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Tipo = 1 AND Clave = " & Especie & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tablas.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function NombreVariedad(ByVal Variedad As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Tipo = 2 AND Clave = " & Variedad & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tablas.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaSeniaArticulo(ByVal Articulo As Integer, ByVal Fecha As Date) As Double

        Dim Envase As Integer = HallaEnvase(Articulo)
        If Envase < 0 Then
            MsgBox("Error Al Leer Tabla: Articulos.")
            End
        End If
        Dim Senia = CalculaSenia(Envase, Fecha)
        If Senia < 0 Then
            MsgBox("Error Al Leer Tabla: Envases.")
            End
        End If

        Return Senia

    End Function
    Public Function HallaSeniaYAGranelArticulo(ByVal Articulo As Integer, ByVal Fecha As Date, ByRef Senia As Double, ByRef AGranel As Boolean) As Boolean

        Dim Dt As New DataTable
        Dim Envase As Integer

        Dim Sql As String = "SELECT A.Envase,E.AGranel FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave WHERE A.Clave = " & Articulo & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then Return False
        If Dt.Rows.Count = 0 Then Dt.Dispose() : Return False
        Envase = Dt.Rows(0).Item("Envase")
        AGranel = Dt.Rows(0).Item("AGranel")
        Dt.Dispose()

        Senia = CalculaSenia(Envase, Fecha)
        If Senia < 0 Then Return False

        Return True

    End Function
    Public Function HallaCierreFactura(ByVal Factura As Double) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nota FROM CierreFacturasCabeza WHERE Estado <> 3 AND Factura = " & Factura & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tablas.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaEstadoFactura(ByVal Factura As Double, ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Estado FROM FacturasCabeza WHERE Factura = " & Factura & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tablas.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaSeniaMaxima() As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT SeniaMaxima FROM DatosEmpresa WHERE Indice = 1;", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Datos Empresa.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function PermisoLectura(ByVal Menu As Integer) As Boolean

        If GAdministrador Then Return True

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = GDtPerfil.Select("Menu = " & Menu)
        If RowsBusqueda.Length = 0 Then
            Return False
        Else
            Return RowsBusqueda(0).Item("Lectura")
        End If

    End Function
    Public Function PermisoEscritura(ByVal Menu As Integer) As Boolean

        If GAdministrador Then Return True

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = GDtPerfil.Select("Menu = " & Menu)
        If RowsBusqueda.Length = 0 Then
            Return False
        Else
            Return RowsBusqueda(0).Item("Escritura")
        End If

    End Function
    Public Function PermisoEspecial1(ByVal Menu As Integer) As Boolean

        If GAdministrador Then Return True

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = GDtPerfil.Select("Menu = " & Menu)
        If RowsBusqueda.Length = 0 Then
            Return False
        Else
            Return RowsBusqueda(0).Item("Especial1")
        End If

    End Function
    Public Function HallaArticulosListaDePrecio(ByVal Lista As Integer) As DataTable

        Dim Dt As New DataTable

        Dt = Tablas.Leer("SELECT Articulo FROM ListaDePreciosDetalle WHERE Lista = " & Lista & ";")

        HallaArticulosListaDePrecio = Dt

        Dt.Dispose()

    End Function
    Public Function HallaArticulosListaDePrecioProveedor(ByVal Lista As Integer) As DataTable

        Dim Dt As New DataTable

        Dt = Tablas.Leer("SELECT Articulo FROM ListaDePreciosProveedoresDetalle WHERE Lista = " & Lista & ";")

        HallaArticulosListaDePrecioProveedor = Dt

        Dt.Dispose()

    End Function
    Public Function HallaArticulosConCodigo(ByVal Cliente As Integer) As DataTable

        Dim Dt As New DataTable

        Dt = Tablas.Leer("SELECT Articulo FROM CodigosCliente WHERE Cliente = " & Cliente & ";")

        HallaArticulosConCodigo = Dt

        Dt.Dispose()

    End Function
    Public Function HallaArticulosPedido(ByVal Pedido As Integer) As DataTable

        Dim Dt As New DataTable

        Dt = Tablas.Leer("SELECT Articulo,Cantidad,Precio FROM PedidosDetalle WHERE Pedido = " & Pedido & ";")

        HallaArticulosPedido = Dt

        Dt.Dispose()

    End Function
    Public Function HallaEstadoArticulo(ByVal Articulo As Integer) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Estado FROM Articulos WHERE Clave = " & Articulo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Articulos.", MsgBoxStyle.Critical)
            End
        End Try


    End Function
    Public Function HallaEstadoRemito(ByVal Remito As Decimal, ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Estado FROM RemitosCabeza WHERE Remito = " & Remito & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error al leer Tabla: RemitosCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaEstadoArticuloServicios(ByVal Articulo As Integer) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Estado FROM ArticulosServicios WHERE Clave = " & Articulo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Articulos.", MsgBoxStyle.Critical)
            End
        End Try


    End Function
    Public Function HallaArticuloDeshabilitado(ByVal dt As DataTable) As Boolean

        For Each Row As DataRow In dt.Rows
            If HallaEstadoArticulo(Row("Articulo")) = 3 Then
                MsgBox("Articulo " & NombreArticulo(Row("Articulo")) & " Esta Deshabilitado; debe Activarlo previamente. Operación se CANCELA.", MsgBoxStyle.Critical)
                dt.Dispose()
                Return True
            End If
        Next

        dt.Dispose()
        Return False

    End Function
    Public Function HallaArticuloServiciosDeshabilitado(ByVal dt As DataTable) As Boolean

        For Each Row As DataRow In dt.Rows
            If HallaEstadoArticuloServicios(Row("Articulo")) = 3 Then
                MsgBox("Articulo " & NombreArticuloServicios(Row("Articulo")) & " Esta Deshabilitado; debe Activarlo previamente. Operación se CANCELA.", MsgBoxStyle.Critical)
                dt.Dispose()
                Return True
            End If
        Next

        dt.Dispose()
        Return False

    End Function
    Public Sub CopiaTabla(ByVal DtOrigen As DataTable, ByVal DtCopia As DataTable)

        For Each Row As DataRow In DtOrigen.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Dim Row1 As DataRow = DtCopia.NewRow
                For I As Integer = 0 To DtOrigen.Columns.Count - 1
                    Row1.Item(I) = Row.Item(I)
                Next
                DtCopia.Rows.Add(Row1)
            End If
        Next

    End Sub
    Public Function TieneNotasPendientes(ByVal Proveedor As Integer) As Boolean

        Dim Miconexion As New OleDb.OleDbConnection(Conexion)

        Try
            Miconexion.Open()
            Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Factura) FROM FacturasProveedorCabeza WHERE Estado = 1 AND NotaDebito = -1 AND Proveedor = " & Proveedor & ";", Miconexion)
                If Cmd.ExecuteScalar() <> 0 Then Return True
            End Using
        Catch ex As Exception
            Return False
        Finally
            If Miconexion.State = ConnectionState.Open Then Miconexion.Close()
        End Try

    End Function
    Public Function HallaFacturaDiferencia(ByVal TipoNota As Integer, ByVal Nota As Double) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Factura FROM FacturasProveedorCabeza WHERE TipoNota = " & TipoNota & " AND NotaDebito = " & Nota & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer FacturasProveedorCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function RemitoTieneNVLP(ByVal Remito As Double, ByVal Operacion As Integer) As Boolean

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim Sql As String = "SELECT COUNT(Lote) FROM AsignacionLotes WHERE Liquidado = 1 AND TipoComprobante = 1 AND Comprobante = " & Remito & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla AsignacionLotes.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function CreaDtParaGrid() As DataTable

        Dim DtGrid As New DataTable
        InicializaRegistros.ArmaArchivoMediosPago(DtGrid)
        CreaDtParaGrid = DtGrid
        DtGrid.Dispose()

    End Function
    Public Function HallaMonedaDeLaCuenta(ByVal Banco As Integer, ByVal Cuenta As Double, ByVal DtCuentas As DataTable) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtCuentas.Select("Banco = " & Banco & " AND Numero = " & Cuenta)
        Return RowsBusqueda(0).Item("Moneda")

    End Function
    Public Function EsReposicion(ByVal Nota As Double, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT NumeroFondoFijo FROM RecibosCabeza WHERE TipoNota = 600 AND Nota = " & Nota & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Proveedores.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function FondoFijoCerrado(ByVal Numero As Integer, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cerrado FROM FondosFijos WHERE Numero = " & Numero & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: RendicionFondoFijo. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function HallaCostoCosteoXKilo(ByVal Operacion As Integer, ByVal Lote As Integer, ByRef CostoConIva As Decimal, ByRef CostoSinIva As Decimal) As Boolean

        CostoConIva = 0
        CostoSinIva = 0

        Dim Costeo As Integer = HallaCosteoLote(Operacion, Lote)
        If Costeo = -1 Then Return False
        If Costeo = 0 Then Return True

        Dim Cerrado As Boolean
        Dim CostoEst As Decimal
        Dim Iva As Decimal
        Dim TotalKilos As Decimal

        If HallaCostosPrecioKilos(Costeo, CostoConIva, CostoSinIva, CostoEst, TotalKilos, Cerrado, Iva) < 0 Then Return False

        If Cerrado Then
            If TotalKilos <> 0 Then
                CostoConIva = Trunca3(CostoConIva / TotalKilos)
                CostoSinIva = Trunca3(CostoSinIva / TotalKilos)
            End If
        Else
            CostoConIva = Trunca3(CostoEst * (1 + Iva / 100))
            CostoSinIva = CostoEst
        End If

        Return True

    End Function
    Public Function NombreTipoPago(ByVal TipoPago As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Tipo = 39 AND Clave = " & TipoPago & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: RendicionFondoFijo. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function NombreSucursalCliente(ByVal Cliente As Integer, ByVal Clave As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM SucursalesClientes WHERE Cliente = " & Cliente & " AND Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: SucursalesClientes. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function NombreSucursalProveedor(ByVal Proveedor As Integer, ByVal Clave As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM SucursalesProveedores WHERE Proveedor = " & Proveedor & " AND Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: SucursalesProveedores. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function HallaCantidadIndiceEnNVLP(ByVal Operacion As Integer, ByVal Remito As Double, ByVal Indice As Integer, ByVal CantidadRemito As Double) As Double

        Dim Sql As String
        Dim ConexionStr As String
        Dim Cantidad As Double = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Sql = "SELECT SUM(D.Cantidad) FROM NVLPCabeza AS C INNER JOIN NVLPLotes AS D ON C.Liquidacion = D.Liquidacion WHERE C.Estado = 1 AND D.Remito = " & Remito & " AND D.Indice = " & Indice & " AND D.OPR = " & Operacion & ";"

        If Operacion = 2 Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                        Dim Resul = Cmd.ExecuteScalar()
                        If Not IsDBNull(Resul) Then
                            Cantidad = CDbl(Resul)
                        Else : Cantidad = 0
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer Tabla: NVLPCabeza. " & ex.Message, MsgBoxStyle.Critical)
                End
            Finally
            End Try
            Return Cantidad
        End If

        Dim DtB As New DataTable
        Dim DtN As New DataTable
        Dim RowsBusqueda() As DataRow

        Sql = "SELECT D.Indice,D.Lote,D.Secuencia,D.Cantidad FROM NVLPCabeza AS C INNER JOIN NVLPLotes AS D ON C.Liquidacion = D.Liquidacion WHERE C.Estado = 1 AND D.Remito = " & Remito & " AND D.Indice = " & Indice & " AND D.OPR = " & Operacion & ";"

        If Not Tablas.Read(Sql, Conexion, DtB) Then
            MsgBox("Error Base de Datos al leer Tabla: NVLPCabeza. ", MsgBoxStyle.Critical)
            End
        End If
        For Each Row As DataRow In DtB.Rows
            Cantidad = Cantidad + Row("Cantidad")
        Next

        If Cantidad = CantidadRemito Then DtB.Dispose() : Return Cantidad

        If Not PermisoTotal Then DtB.Dispose() : Return Cantidad

        If Not Tablas.Read(Sql, ConexionN, DtN) Then
            MsgBox("Error Base de Datos al leer Tabla: NVLPCabeza. ", MsgBoxStyle.Critical)
            End
        End If
        For Each Row As DataRow In DtN.Rows
            RowsBusqueda = DtB.Select("Indice = " & Row("Indice") & " AND Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
            If RowsBusqueda.Length = 0 Then
                Cantidad = Cantidad + Row("Cantidad")
            End If
        Next

        DtB.Dispose()
        DtN.Dispose()

        Return Cantidad

    End Function
    Public Function ValidaCosteo(ByVal Proveedor As Integer, ByVal Costeo As Integer, ByVal ListaDeArticulos As List(Of ItemListaDePrecios), ByVal Fecha As Date) As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Negocio,Cerrado,IntFechaDesde,Intfechahasta,Especie,Variedad FROM Costeos WHERE Costeo = " & Costeo & ";", Conexion, Dt) Then Return False
        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Costeo.", MsgBoxStyle.Critical)
            Dt.Dispose()
            Return False
        End If
        '
        If Dt.Rows(0).Item("Negocio") <> Proveedor Then
            MsgBox("Costeo No Existe para el Proveedor informado.", MsgBoxStyle.Critical)
            Dt.Dispose()
            Return False
        End If
        '
        If Not (Format(Fecha, "yyyyMMdd") >= Dt.Rows(0).Item("IntFechaDesde") And Format(Fecha, "yyyyMMdd") <= Dt.Rows(0).Item("IntFechaHasta")) Then
            MsgBox("Fecha del Ingreso NO Corresponde a Fecha del Costeo.", MsgBoxStyle.Critical)
            Dt.Dispose()
            Return False
        End If
        '
        If Dt.Rows(0).Item("Cerrado") Then
            MsgBox("Costeo Cerrado.", MsgBoxStyle.Critical)
            Dt.Dispose()
            Return False
        End If

        Dim Especie As Integer = Dt.Rows(0).Item("Especie")
        Dim Variedad As Integer = Dt.Rows(0).Item("Variedad")

        For Each Fila As ItemListaDePrecios In ListaDeArticulos
            Dt = New DataTable
            If Not Tablas.Read("SELECT Especie,Variedad FROM Articulos WHERE Clave = " & Fila.Articulo & ";", Conexion, Dt) Then Return False
            If Especie <> 0 Then
                If Dt.Rows(0).Item("Especie") <> Especie Then
                    MsgBox("Especie de " & NombreArticulo(Fila.Articulo) & " No Corresponde al Costeo", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Dt.Dispose()
                    Return False
                End If
            End If
            If Variedad <> 0 Then
                If Dt.Rows(0).Item("Variedad") <> Variedad Then
                    MsgBox("Variedad de " & NombreArticulo(Fila.Articulo) & " No Corresponde al Costeo", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Dt.Dispose()
                    Return False
                End If
            End If
        Next

        Dt.Dispose()

        Return True

    End Function
    Public Function HallaMonedaCliente(ByVal Cliente As Integer) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Moneda FROM Clientes WHERE Clave = " & Cliente & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer tabla: Clientes.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaMonedaProveedor(ByVal Proveedor As Integer) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Moneda FROM Proveedores WHERE Clave = " & Proveedor & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer tabla: Clientes.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaCierreFacturasExterior(ByVal Factura As Double) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nota FROM CierreFacturasCabeza WHERE Estado <> 3 AND Factura = " & Factura & ";", Miconexion)
                    Return Cmd.ExecuteScalar
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla Cierre de Facturas Exterior.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Sub PreparaEnlace(ByVal PaseDeProyectos As ItemPaseDeProyectos)

        PermisoTotal = PaseDeProyectos.PermisoTotal

        PreparaInicio(PaseDeProyectos.Servidor, PaseDeProyectos.ClaveEmpresa, PaseDeProyectos.ClaveUsuario)

        If GFaltaDatosEmpresa Then
            End
        End If

    End Sub
    Public Sub PreparaInicio(ByVal Servidor As String, ByVal ClaveEmpresa As Integer, ByVal ClaveUsuario As Integer)

        Dim Dt As New DataTable

        GClaveEmpresa = ClaveEmpresa

        ConexionEmpresa = ConexionEmpresa.Replace("YYYY", Servidor)

        If Not Tablas.Read("SELECT Nombre,Cuit,Base1,Base2,TieneGestionExportacion,TieneGestionProduccion,LicenciaUso FROM Empresas WHERE Clave = " & GClaveEmpresa & ";", ConexionEmpresa, Dt) Then End

        Conexion = ConexionW1.Replace("XXXX", Dt.Rows(0).Item("Base1")).Replace("YYYY", Servidor)
        ConexionN = ConexionNW1.Replace("XXXX", Dt.Rows(0).Item("Base2")).Replace("YYYY", Servidor)
        GNombreEmpresa = Desencriptar(Dt.Rows(0).Item("Nombre"))
        Dim CuitNumerico As Double = Desencriptar(Dt.Rows(0).Item("Cuit"))
        GCuitEmpresa = Format(CuitNumerico, "00-00000000-0")
        GTieneGestionExportacion = Dt.Rows(0).Item("TieneGestionExportacion")
        GFechaVencimiento = "1800/01/01"
        If Dt.Rows(0).Item("LicenciaUso") <> "" Then
            Dim Licenciaw As String = Desencriptar(Dt.Rows(0).Item("LicenciaUso"))
            GFechaVencimiento = Strings.Mid(Licenciaw, 7, 2) & "/" & Strings.Mid(Licenciaw, 5, 2) & "/" & Strings.Mid(Licenciaw, 1, 4)
        End If

        Entrada.Text = GNombreEmpresa

        Dt = New DataTable
        Dt = Tablas.Leer("SELECT ColorPantallaInicial,TipoIva,Tolerancia,Direccion1,Direccion2,Direccion3,IngBruto,FechaInicio,TipoIva,LimiteParaConsumidorFinal FROM DatosEmpresa WHERE Indice =1")
        If Dt.Rows.Count = 0 Then
            MsgBox("Debe Completar Datos del la Empresa antes de Ingresar al Sistema y Reingresar Nuevamente.", MsgBoxStyle.Critical)
            GFaltaDatosEmpresa = True
            Exit Sub
        End If

        If Dt.Rows(0).Item("ColorPantallaInicial") <> "" Then
            Dim ColorR As Integer = Mid(Dt.Rows(0).Item("ColorPantallaInicial"), 1, 3)
            Dim ColorG As Integer = Mid(Dt.Rows(0).Item("ColorPantallaInicial"), 4, 3)
            Dim ColorB As Integer = Mid(Dt.Rows(0).Item("ColorPantallaInicial"), 7, 3)
            Entrada.BackColor = Color.FromArgb(ColorR, ColorG, ColorB)
        End If

        GTipoIva = Dt.Rows(0).Item("TipoIva")
        GTolerancia = Dt.Rows(0).Item("Tolerancia")
        GDireccion1 = Dt.Rows(0).Item("Direccion1")
        GDireccion2 = Dt.Rows(0).Item("Direccion2")
        GDireccion3 = Dt.Rows(0).Item("Direccion3")
        GIngBruto = Dt.Rows(0).Item("IngBruto")
        GFechaInicio = Dt.Rows(0).Item("FechaInicio")
        GTipoIvaEmpresa = Dt.Rows(0).Item("TipoIva")
        GCondicionIvaEmpresa = HallaCondicionIva(Dt.Rows(0).Item("TipoIva"))
        GLimiteParaConsumidorFinal = Dt.Rows(0).Item("LimiteParaConsumidorFinal")

        Dt.Dispose()

        ArmaPermisos(ClaveUsuario)

    End Sub
    Private Sub ArmaPermisos(ByVal ClaveUsuario As Integer)

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT * FROM Usuarios WHERE Clave = " & ClaveUsuario & ";", Conexion, Dt) Then End

        GCaja = Dt.Rows(0).Item("Caja")
        GCajaTotal = Dt.Rows(0).Item("CajaTotal")
        GPuntoDeVentaResponsableInsc = Dt.Rows(0).Item("PuntoDeVentaResponsableInsc")
        GPuntoDeVentaResponsableNoInsc = Dt.Rows(0).Item("PuntoDeVentaResponsableNoInsc")
        GPuntoDeVentaConsumidorFinal = Dt.Rows(0).Item("PuntoDeVentaConsumidorFinal")
        GPuntoDeVentaExportacion = Dt.Rows(0).Item("PuntoDeVentaExportacion")
        GPuntoDeVentaRemitos = Dt.Rows(0).Item("PuntoDeVentaRemitos")
        GPuntoDeVentaRecibos = Dt.Rows(0).Item("PuntoDeVentaRecibos")
        GPuntoDeVentaDebResponsableInsc = Dt.Rows(0).Item("PuntoDeVentaDebResponsableInsc")
        GPuntoDeVentaDebResponsableNoInsc = Dt.Rows(0).Item("PuntoDeVentaDebResponsableNoInsc")
        GPuntoDeVentaDebConsumidorFinal = Dt.Rows(0).Item("PuntoDeVentaDebConsumidorFinal")
        GPuntoDeVentaDebExportacion = Dt.Rows(0).Item("PuntoDeVentaDebExportacion")
        GPuntoDeVentaCredResponsableInsc = Dt.Rows(0).Item("PuntoDeVentaCredResponsableInsc")
        GPuntoDeVentaCredResponsableNoInsc = Dt.Rows(0).Item("PuntoDeVentaCredResponsableNoInsc")
        GPuntoDeVentaCredConsumidorFinal = Dt.Rows(0).Item("PuntoDeVentaCredConsumidorFinal")
        GPuntoDeVentaCredExportacion = Dt.Rows(0).Item("PuntoDeVentaCredExportacion")
        GPuntoDeVentaLiqResponsableInsc = Dt.Rows(0).Item("PuntoDeVentaLiqResponsableInsc")
        GPuntoDeVentaLiqResponsableNoInsc = Dt.Rows(0).Item("PuntoDeVentaLiqResponsableNoInsc")
        GPuntoDeVentaLiqConsumidorFinal = Dt.Rows(0).Item("PuntoDeVentaLiqConsumidorFinal")

        If Dt.Rows(0).Item("Funcion") = 1 Then GAdministrador = True
        GClaveUsuario = Dt.Rows(0).Item("Clave")

        GDtPerfil = New DataTable
        If Not Tablas.Read("SELECT * FROM Perfiles WHERE Usuario = " & GClaveUsuario & ";", Conexion, GDtPerfil) Then End

        Dt.Dispose()

    End Sub
    Public Function HallaNegocio(ByVal Costeo As Integer) As Integer

        If Costeo = 0 Then Return 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Negocio FROM Costeos WHERE Costeo = " & Costeo & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then Return CInt(Ultimo)
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos, al leer Tabla de Costeos.")
            End
        End Try

    End Function
    Public Sub HallaTotalesImportesNotasDebitoCredito(ByVal Operacion As Integer, ByVal TipoNota As Integer, ByVal Nota As Double, ByRef NetoGrabado As Decimal, ByRef NetoNoGrabado As Decimal, ByRef ImporteRetencion As Decimal, ByRef ImporteIva As Decimal, ByVal Cambio As Decimal)

        Dim Sql As String
        Dim Dt As New DataTable
        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        NetoGrabado = 0
        NetoNoGrabado = 0
        ImporteRetencion = 0
        ImporteIva = 0

        Sql = "SELECT MedioPago,Neto,Alicuota,Importe FROM RecibosDetallePago WHERE TipoNota = " & TipoNota & " AND Nota = " & Nota & ";"
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Select Case Row("MedioPago")
                Case 100
                    If Row("Alicuota") <> 0 Then
                        NetoGrabado = NetoGrabado + CalculaNeto(Row("Neto"), Cambio)
                        ImporteIva = ImporteIva + CalculaNeto(CalculaIva(1, Row("Neto"), Row("Alicuota")), Cambio)
                    Else
                        NetoNoGrabado = NetoNoGrabado + CalculaNeto(Row("Neto"), Cambio)
                    End If
                Case Else
                    If EsRetencion(Row("MedioPago")) Then
                        ImporteRetencion = ImporteRetencion + CalculaNeto(Row("Importe"), Cambio)
                    End If
            End Select
        Next

    End Sub
    Public Sub HallaTotalesImportesAsientosManuales(ByVal Operacion As Integer, ByVal Asiento As Double, ByRef ImporteW As Decimal)

        Dim Sql As String
        Dim Dt As New DataTable
        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        ImporteW = 0

        Sql = "SELECT Debe FROM AsientosDetalle WHERE Asiento = " & Asiento & ";"
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            ImporteW = ImporteW + Row("Debe")
        Next

    End Sub
    Private Function EsRetencion(ByVal Impuesto As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Clave FROM Tablas WHERE Tipo = 25 AND Clave = " & Impuesto & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos, al leer Tabla de Costeos.")
            End
        End Try

    End Function
    Public Function HallaListaProveedorConZona(ByVal Proveedor As Integer, ByVal Fecha As DateTime, ByRef Lista As Integer, ByRef PorUnidad As Boolean, ByRef Final As Boolean, ByVal Zona As Integer) As Integer

        Lista = 0

        Dim Sql As String = "SELECT Lista,PorUnidad,Final FROM ListaDePreciosProveedoresCabeza WHERE Proveedor = " & Proveedor & " AND Zona = " & Zona & " AND IntFechaDesde <= " & Format(Fecha, "yyyyMMdd") & " AND IntFechaHasta >= " & Format(Fecha, "yyyyMMdd") & ";"
        Dim Dt As New DataTable

        If Not Tablas.Read(Sql, Conexion, Dt) Then Dt.Dispose() : Return -1
        If Dt.Rows.Count <> 0 Then
            Lista = Dt.Rows(0).Item("Lista")
            PorUnidad = Dt.Rows(0).Item("PorUnidad")
            Final = Dt.Rows(0).Item("Final")
        End If

        Dt.Dispose()

    End Function
    Public Function EsBancoNegro(ByVal Banco As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Activo2 = 1 AND Tipo = 26 AND Clave = " & Banco & ";", Miconexion)
                    If Cmd.ExecuteScalar() = "" Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function EsArticuloAGranel(ByVal Articulo As Integer) As Boolean


        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT E.AGranel FROM Articulos AS A INNER JOIN Envases As E ON A.Envase = E.Clave WHERE A.Clave = " & Articulo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error base de Datos al Leer Tabla: Articulos. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Sub HallaAGranelYMedida(ByVal Articulo As Integer, ByRef AGranel As Boolean, ByRef Medida As String)

        If Articulo = 0 Then Exit Sub

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT E.AGranel,E.Unidad FROM Articulos AS A INNER JOIN Envases As E ON A.Envase = E.Clave WHERE A.Clave = " & Articulo & ";", Conexion, Dt) Then
            MsgBox("Error base de Datos al Leer Tabla: Articulos,Envases. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End If

        If Dt.Rows(0).Item("AGranel") Then
            Medida = Dt.Rows(0).Item("Unidad")
            AGranel = True
        Else
            Medida = "Uni"
            AGranel = False
        End If

        Dt.Dispose()

    End Sub
    Public Sub HallaAGranelYMedidaFactura(ByVal EsSecos As Boolean, ByVal EsServicios As Boolean, ByVal Articulo As Integer, ByRef AGranel As Boolean, ByRef Medida As String)

        If EsSecos Or EsServicios Then
            AGranel = False
            Medida = "Uni"
            Exit Sub
        End If

        HallaAGranelYMedida(Articulo, AGranel, Medida)

    End Sub
    Public Sub HallaAGranelYMedidaLogistico(ByVal Articulo As Integer, ByRef AGRanel As Boolean, ByRef Medida As String)

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Activo2 FROM Tablas WHERE Tipo = 6 AND Clave = " & Articulo & ";", Miconexion)
                    AGRanel = Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error base de Datos al Leer Tabla: Articulos. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

        If AGRanel Then
            Medida = "Kg"
        Else
            Medida = "Un"
        End If

    End Sub
    Public Function HallaUMedidaArticulo(ByVal Articulo As Integer) As String


        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT E.Unidad FROM Articulos AS A INNER JOIN Envases As E ON A.Envase = E.Clave WHERE A.Clave = " & Articulo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: Articulos/Envase. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Function TieneListaDePrecios(ByVal Cliente As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ListaDePrecios FROM Clientes WHERE Clave = " & Cliente & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Dato Empresa.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function TieneListaDePreciosW(ByVal Cliente As Integer) As Integer

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT ListaDePrecios,ListaDePreciosPorZona,ListaDePreciosPorVendedor FROM Clientes WHERE Clave = " & Cliente & ";", Conexion, Dt) Then End
        If Dt.Rows.Count = 0 Then
            MsgBox("Cliente No Encontrado.", MsgBoxStyle.Critical)
            Dt.Dispose() : Return -1
        End If
        If Dt.Rows(0).Item("ListaDePrecios") Then Dt.Dispose() : Return 1
        If Dt.Rows(0).Item("ListaDePreciosPorZona") Then Dt.Dispose() : Return 2
        If Dt.Rows(0).Item("ListaDePreciosPorVendedor") Then Dt.Dispose() : Return 3

        Dt.Dispose()
        Return 0

    End Function
    Public Function TieneListaDePreciosProveedorW(ByVal Proveedor As Integer) As Integer

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT ListaDePrecios,ListaDePreciosPorZona FROM Proveedores WHERE Clave = " & Proveedor & ";", Conexion, Dt) Then End
        If Dt.Rows.Count = 0 Then
            MsgBox("Proveedor No Encontrado.", MsgBoxStyle.Critical)
            Dt.Dispose() : Return -1
        End If
        If Dt.Rows(0).Item("ListaDePrecios") Then Dt.Dispose() : Return 1
        If Dt.Rows(0).Item("ListaDePreciosPorZona") Then Dt.Dispose() : Return 2

        Dt.Dispose()
        Return 0

    End Function
    Public Function HallaPedidoCliente(ByVal Pedido As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT PedidoCliente FROM PedidosCabeza WHERE Pedido = " & Pedido & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: PedidosCabeza.")
            End
        End Try

    End Function
    Public Function HallaSaldosRendicionesCerradas(ByVal FondoFijo As Integer, ByVal ConexionStr As String) As Decimal

        Dim Sql As String = "SELECT SUM(Saldo) FROM RendicionFondoFijo WHERE Numero = " & FondoFijo & " AND Cerrado = 1;"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then Return CDec(Ultimo)
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: RendicionFondoFijo", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function EsUnNegocio(ByVal Proveedor As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT TipoOperacion FROM Proveedores WHERE Clave = " & Proveedor & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 4 Then
                        Return True
                    Else : Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error base de Datos al Leer Tabla: Proveedores. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Public Sub CerrarAutomaticamentePedidos()

        Dim SqlFecha As String = ""
        SqlFecha = "FechaEntregaHasta <'" & Format(Date.Now, "yyyyMMdd") & "'"

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT * FROM PedidosCabeza WHERE Cerrado = 0 AND " & SqlFecha & ";", Conexion, Dt) Then Exit Sub

        For Each Row As DataRow In Dt.Rows
            Row("Cerrado") = True
        Next

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(Dt.GetChanges) Then
                    Resul = GrabaTabla(Dt.GetChanges, "PedidosCabeza", Conexion)
                    If Resul <= 0 Then
                        MsgBox("Error de Base de Datos al Grabar Tabla: PedidosCabeza. Cierre Automático de Pedidos se CANCELA.")
                    End If
                End If
                Scope.Complete()
            End Using
        Catch ex As TransactionException
            MsgBox("Otro Usuario modifico el Archivo: PedidosCabeza. . Cierre Automático de Pedidos se CANCELA.")
        End Try

        Dt.Dispose()

    End Sub
    Public Function ConvierteANumerico(ByVal Str As String) As Double

        Dim Letra As String = Mid$(Str, 1, 1)
        Dim CentroCosto As String = Format(Val(Mid$(Str, 2, 4)), "0000")
        Dim Numero As String = Format(Val(Mid$(Str, 7, 8)), "00000000")

        Return HallaNumeroLetra(Letra) & CentroCosto & Numero

    End Function
    Public Function ConvierteNumeroAString(ByVal Importe As Decimal, ByVal Separador As String, ByVal LongitudEntero As Integer) As String

        ' decimales del importe debe tener 1 o 2 digitos.

        Dim ImporteAux As Decimal = Importe
        If Importe < 0 Then Importe = -1 * Importe

        Dim Arr() As String
        Arr = Importe.ToString.Split(",")

        Dim EnteroStr As String = Arr(0)
        If Arr(0).Length < LongitudEntero Then
            Do
                EnteroStr = "0" & EnteroStr
            Loop Until EnteroStr.Length = LongitudEntero
        End If

        If ImporteAux < 0 Then EnteroStr = "-" & Strings.Right(EnteroStr, LongitudEntero - 1)

        Dim DecimalStr As String
        If Arr.Length = 1 Then
            DecimalStr = "00"
        Else
            DecimalStr = Arr(1)
            If DecimalStr.Length = 1 Then DecimalStr = DecimalStr & "0"
            If DecimalStr.Length = 0 Then DecimalStr = DecimalStr & "00"
            If DecimalStr.Length > 2 Then DecimalStr = Strings.Left(DecimalStr, 2)
        End If

        Return EnteroStr & Separador & DecimalStr

    End Function
    Public Function EsExterior(ByVal Factura As Double) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT EsExterior FROM FacturasCabeza WHERE Factura = " & Factura & ";", Miconexion)
                    Return Cmd.ExecuteScalar
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Facturas.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function NombreTabla(ByVal Clave As Integer) As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Clave = " & Clave & ";", Miconexion)
                    Return Cmd.ExecuteScalar
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tablas.", MsgBoxStyle.Critical)
            End
        End Try


    End Function
    Public Function HallaZonaCliente(ByVal Cliente As Integer, ByVal Sucursal As Integer) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Zona FROM SucursalesClientes WHERE Estado = 1 AND Cliente = " & Cliente & " AND Clave = " & Sucursal & ";", Miconexion)
                    Return Cmd.ExecuteScalar
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: SucursalesClientes.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaZonaProveedor(ByVal Proveedor As Integer, ByVal Sucursal As Integer) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Zona FROM SucursalesProveedores WHERE Estado = 1 AND Proveedor = " & Proveedor & " AND Clave = " & Sucursal & ";", Miconexion)
                    Return Cmd.ExecuteScalar
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: SucursalesProveedores.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaPrecioArticuloProveedor(ByVal Lista As Integer, ByVal Articulo As Integer, ByVal KilosXUnidad As Double, ByVal PorUnidadEnLista As Boolean, ByVal FinalEnLista As Boolean) As Decimal

        Dim Dt As New DataTable
        Dim Precio As Decimal = 0

        Dim Sql As String = "SELECT Precio FROM ListaDePreciosProveedoresDetalle WHERE Lista = " & Lista & " AND Articulo = " & Articulo & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        If Dt.Rows.Count = 0 Then Dt.Dispose() : Return 0
        Precio = Dt.Rows(0).Item("Precio")

        Dim Iva As Double = HallaIva(Articulo)

        If Not PorUnidadEnLista Then
            Precio = Precio * KilosXUnidad
        End If

        If Not FinalEnLista Then
            Precio = Precio + CalculaIva(1, Precio, Iva)
        End If

        Dt.Dispose()
        Return Trunca3(Precio)

    End Function
    Public Function HallaListaPrecioProveedor(ByVal Proveedor As Integer, ByVal Sucursal As Integer, ByVal Fecha As Date, ByRef PorUnidadEnLista As Boolean, ByRef FinalEnLista As Boolean, ByRef Zona As Integer) As Decimal

        Zona = 0
        Dim TipoListaDePrecios As Integer = TieneListaDePreciosProveedorW(Proveedor)

        Select Case TipoListaDePrecios
            Case 0
                Return 0
            Case 1
                Zona = 0
            Case 2
                If Sucursal = 0 Then
                    MsgBox("Falta Definir Sucursal para Hallar Lista de Precios por Zona.", MsgBoxStyle.Critical)
                    Return -1
                End If
                Zona = HallaZonaProveedor(Proveedor, Sucursal)
                If Zona = 0 Then
                    MsgBox("Falta Definir Zona a la Sucursal.", MsgBoxStyle.Critical)
                    Return -1
                End If
        End Select

        Dim Lista As Integer = 0
        If HallaListaProveedorConZona(Proveedor, Fecha, Lista, PorUnidadEnLista, FinalEnLista, Zona) < 0 Then Return -1
        If Lista = 0 Then
            MsgBox("Falta Definir Lista de Precios para este Proveedor y Fecha de Entrega.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return -1
        End If

        Return Lista

    End Function
    Public Function HallaListaPrecios(ByVal Cliente As Integer, ByVal Sucursal As Integer, ByVal FechaEntrega As Date, ByRef PorUnidadEnLista As Boolean, ByRef FinalEnLista As Boolean) As Integer

        Dim Lista As Integer = 0
        Dim TipoListaDePrecios As Integer = TieneListaDePreciosW(Cliente)
        Dim Zona As Integer = 0

        Select Case TipoListaDePrecios
            Case 0
                Return 0
            Case 1
                Zona = 0
            Case 2
                If Sucursal = 0 Then
                    MsgBox("Falta Definir Sucursal para Hallar Lista de Precios por Zona.", MsgBoxStyle.Critical)
                    Return -1
                End If
                Zona = HallaZonaCliente(Cliente, Sucursal)
                If Zona = 0 Then
                    MsgBox("Falta Definir Zona a la Sucursal.", MsgBoxStyle.Critical)
                    Return -1
                End If
            Case 3
                Zona = 0
                Cliente = HallaVendedorCliente(Cliente)
        End Select

        If HallaListaConZona(Cliente, FechaEntrega, Lista, PorUnidadEnLista, FinalEnLista, Zona, TipoListaDePrecios) < 0 Then Return False
        If Lista = 0 Then
            MsgBox("Falta Definir Lista de Precios para este Cliente y Fecha de Entrega: " & Format(FechaEntrega, "dd/MM/yyyy"), MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return -1
        End If

        Return Lista

    End Function
    Public Function HallaListaConZona(ByVal Cliente As Integer, ByVal Fecha As DateTime, ByRef Lista As Integer, ByRef PorUnidad As Boolean, ByRef Final As Boolean, ByVal Zona As Integer, ByVal TipoListaDePrecios As Integer) As Integer

        Lista = 0

        Dim SqlTipo As String = ""
        If TipoListaDePrecios = 3 Then
            SqlTipo = " AND EsPorVendedor = 1"
        Else : SqlTipo = " AND EsPorVendedor = 0"
        End If

        Dim Sql As String = "SELECT Lista,PorUnidad,Final FROM ListaDePreciosCabeza WHERE Cliente = " & Cliente & " AND Zona = " & Zona & " AND IntFechaDesde <= " & Format(Fecha, "yyyyMMdd") & " AND IntFechaHasta >= " & Format(Fecha, "yyyyMMdd") & SqlTipo & ";"
        Dim Dt As New DataTable

        If Not Tablas.Read(Sql, Conexion, Dt) Then Dt.Dispose() : Return -1
        If Dt.Rows.Count <> 0 Then
            Lista = Dt.Rows(0).Item("Lista")
            PorUnidad = Dt.Rows(0).Item("PorUnidad")
            Final = Dt.Rows(0).Item("Final")
        End If

        Dt.Dispose()

    End Function
    Public Function HallaLista(ByVal Cliente As Integer, ByVal Fecha As DateTime, ByRef Lista As Integer, ByRef PorUnidad As Boolean, ByRef Final As Boolean) As Integer

        Lista = 0

        Dim Sql As String = "SELECT Lista,PorUnidad,Final FROM ListaDePreciosCabeza WHERE Cliente = " & Cliente & " AND IntFechaDesde <= " & Format(Fecha, "yyyyMMdd") & " AND IntFechaHasta >= " & Format(Fecha, "yyyyMMdd") & ";"
        Dim Dt As New DataTable

        If Not Tablas.Read(Sql, Conexion, Dt) Then Dt.Dispose() : Return -1
        If Dt.Rows.Count <> 0 Then
            Lista = Dt.Rows(0).Item("Lista")
            PorUnidad = Dt.Rows(0).Item("PorUnidad")
            Final = Dt.Rows(0).Item("Final")
        End If

        Dt.Dispose()

    End Function
    Public Function HallaVendedorCliente(ByVal Cliente As Integer) As Integer

        Dim Dt As New DataTable

        Try
            If Not Tablas.Read("SELECT Vendedor FROM Clientes WHERE Clave = " & Cliente & ";", Conexion, Dt) Then End
            Return Dt.Rows(0).Item("Vendedor")
        Catch ex As Exception
        Finally
            Dt.Dispose()
        End Try

    End Function
    Public Function ArticuloEstaEnLista(ByVal Lista As Integer, ByVal Articulo As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Articulo FROM ListaDePreciosDetalle WHERE Lista =  " & Lista & " AND Articulo = " & Articulo & ";", Miconexion)
                    If Cmd.ExecuteScalar = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: Lista de Precios " & Lista, MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function ActualizaDetallePedido(ByVal Funcion As String, ByVal Tipo As String, ByVal Pedido As Integer, ByVal DtDetalle As DataTable, ByRef DtPedido As DataTable) As Boolean

        Dim RowsBusqueda() As DataRow
        Dim DtPedidoW As New DataTable

        If Not Tablas.Read("SELECT * FROM PedidosDetalle WHERE Pedido = " & Pedido & ";", Conexion, DtPedidoW) Then Return False
        If DtPedidoW.Rows.Count = 0 Then DtPedidoW.Dispose() : Return -2

        For Each Row As DataRow In DtDetalle.Rows
            RowsBusqueda = DtPedidoW.Select("Articulo = " & Row("Articulo"))
            Dim Cantidad As Decimal
            If Tipo = "R" Then
                Cantidad = Row("Cantidad") - Row("Devueltas")
            End If
            If Tipo = "D" Then
                Cantidad = Row("Cantidad")
            End If
            If RowsBusqueda.Length <> 0 Then
                If Funcion = "A" Then
                    RowsBusqueda(0).Item("Entregada") = CDec(RowsBusqueda(0).Item("Entregada")) + Cantidad
                End If
                If Funcion = "B" Then
                    RowsBusqueda(0).Item("Entregada") = CDec(RowsBusqueda(0).Item("Entregada")) - Cantidad
                    If RowsBusqueda(0).Item("Entregada") < 0 Then
                        If MsgBox("Proceso hace Entregadas menor a cero para el articulo: " & NombreArticulo(Row("Articulo")) & " en el pedido: " & Pedido & " " + vbCrLf + "Quiere Cancelar el Proceso(S/N)", MsgBoxStyle.YesNo, MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                            DtPedidoW.Dispose() : Return False
                        End If
                    End If
                End If
            Else
                MsgBox("Articulo " & NombreArticulo(Row("Articulo")) & " No Encontrado en el Pedido " & Pedido, MsgBoxStyle.Critical)
                DtPedidoW.Dispose() : Return False
            End If
        Next

        DtPedido = DtPedidoW.Copy
        DtPedidoW.Dispose()

        Return True

    End Function
    Public Function CalculaEOM13(ByRef Numero As String, ByRef Digito As Integer) As Boolean

        Dim aa As New DllVarias
        Dim DigitoW As Integer = aa.CalculaEAM13(Numero)
        Select Case DigitoW
            Case -1, -2
                MsgBox("Incorrecto Argumento para EAN 13.")
                Return False
        End Select

        Return True

    End Function
    Public Function EAN13OK(ByVal Numero As String) As Boolean

        Dim aa As New DllVarias
        Dim NumeroW As String = Strings.Left(Numero, 12)
        Dim DigitoW As Integer = aa.CalculaEAM13(NumeroW)
        Select Case DigitoW
            Case -1, -2
                Return False
        End Select
        If Numero <> NumeroW Then Return False

        Return True

    End Function
    Public Function EsPuntoDeVentaFacturasElectronicas(ByVal PuntoDeVenta As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT FacturasElectronicas FROM PuntosDeVenta WHERE Clave =  " & PuntoDeVenta & ";", Miconexion)
                    If Cmd.ExecuteScalar Then
                        Return True
                    Else
                        Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: PuntosDeVenta.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaCompAsociado(ByVal Tipo As Integer, ByVal Comprobante As Decimal) As String

        Select Case Tipo
            Case 5
                Return "Nota Debito a Cliente: " & NumeroEditado(Comprobante)
            Case 7
                Return "Nota Crédito a Cliente: " & NumeroEditado(Comprobante)
            Case 6
                Return "Nota Debito a Proveedor: " & NumeroEditado(Comprobante)
            Case 8
                Return "Nota Crédito a Proveedor: " & NumeroEditado(Comprobante)
            Case 1
                Return "Factura: " & NumeroEditado(Comprobante)
            Case 2
                Return "Liquidación: " & NumeroEditado(Comprobante)
            Case 0
                Return "No Existe Comprobante Asociado."
        End Select

    End Function
    Public Function ArmaListado(ByVal Grid As DataGridView, ByVal LineasPorPagina As Integer) As List(Of ItemListado)

        Dim Listado As New List(Of ItemListado)

        GoTo hh               'anulado por pedido cliente. imprime la primera hoja sin agrupar.
        If Grid.Rows.Count <= LineasPorPagina Then
            For Each Row As DataGridViewRow In Grid.Rows
                If IsNothing(Row.Cells("Articulo").Value) Then Exit For
                If Row.Cells("Articulo").Value <> 0 Then
                    Dim Precio As Decimal = TruncaSR(Row.Cells("Precio").Value, 3)
                    Dim PrecioLista As Decimal = TruncaSR(Row.Cells("PrecioLista").Value, 3)
                    Dim TipoPrecio As Integer = Row.Cells("TipoPrecio").Value
                    AltaItem(Listado, Row.Cells("Articulo").Value, TipoPrecio, Precio, Row.Cells("Cantidad").Value, Row.Cells("TotalArticulo").Value, Row.Cells("Medida").FormattedValue, Row.Cells("CodigoCliente").Value, Row.Cells("KilosXUnidad").Value, PrecioLista, Row.Cells("Iva").Value, Row.Cells("Neto").Value)
                End If
            Next
            Return Listado
        End If
hh:

        For Each Row As DataGridViewRow In Grid.Rows
            If IsNothing(Row.Cells("Articulo").Value) Then Exit For
            If Row.Cells("Articulo").Value <> 0 Then
                Dim Precio As Decimal = TruncaSR(Row.Cells("Precio").Value, 3)
                Dim PrecioLista As Decimal = TruncaSR(Row.Cells("PrecioLista").Value, 3)
                Dim TipoPrecio As Integer = Row.Cells("TipoPrecio").Value
                If Listado.Count = 0 Then
                    AltaItem(Listado, Row.Cells("Articulo").Value, TipoPrecio, Precio, Row.Cells("Cantidad").Value, Row.Cells("TotalArticulo").Value, Row.Cells("Medida").FormattedValue, Row.Cells("CodigoCliente").Value, Row.Cells("KilosXUnidad").Value, PrecioLista, Row.Cells("Iva").Value, Row.Cells("Neto").Value)
                Else
                    If Listado.Count > 0 Then
                        Dim I As Integer = BuscaEnListado(Listado, Row.Cells("Articulo").Value, PrecioLista, Precio)
                        If I <> -1 Then
                            Listado.Item(I).Cantidad = Listado.Item(I).Cantidad + CDec(Row.Cells("Cantidad").Value)
                            Listado.Item(I).TotalItem = Listado.Item(I).TotalItem + CDec(Row.Cells("TotalArticulo").Value)
                        Else
                            AltaItem(Listado, Row.Cells("Articulo").Value, TipoPrecio, Precio, Row.Cells("Cantidad").Value, Row.Cells("TotalArticulo").Value, Row.Cells("Medida").FormattedValue, Row.Cells("CodigoCliente").Value, Row.Cells("KilosXUnidad").Value, PrecioLista, Row.Cells("Iva").Value, Row.Cells("Neto").Value)
                        End If
                    End If
                End If
            End If
        Next

        Return Listado

    End Function
    Public Function ArmaListadoNotaCredito(ByVal Grid As DataGridView, ByVal LineasPorPagina As Integer, ByVal Abierto As Boolean) As List(Of ItemListado)

        Dim Listado As New List(Of ItemListado)

        If Grid.Rows.Count <= LineasPorPagina Then
            For Each Row As DataGridViewRow In Grid.Rows
                If IsNothing(Row.Cells("Articulo").Value) Then Exit For
                If Row.Cells("Articulo").Value <> 0 Then
                    Dim Iva As Decimal
                    If Abierto Then
                        Iva = Row.Cells("IvaB").Value
                    Else
                        Iva = Row.Cells("IvaN").Value
                    End If
                    AltaItem(Listado, Row.Cells("Articulo").Value, 0, Row.Cells("Precio").Value, Row.Cells("Cantidad").Value, Row.Cells("Importe").Value, Row.Cells("Medida").FormattedValue, 0, 0, Row.Cells("PrecioLista").Value, Iva, Row.Cells("Neto").Value)
                End If
            Next
            Return Listado
        End If

        For Each Row As DataGridViewRow In Grid.Rows
            If IsNothing(Row.Cells("Articulo").Value) Then Exit For
            Dim Iva As Decimal
            If Abierto Then
                Iva = Row.Cells("IvaB").Value
            Else
                Iva = Row.Cells("IvaN").Value
            End If
            If Row.Cells("Articulo").Value <> 0 Then
                If Listado.Count = 0 Then
                    AltaItem(Listado, Row.Cells("Articulo").Value, 0, Row.Cells("Precio").Value, Row.Cells("Cantidad").Value, Row.Cells("Importe").Value, Row.Cells("Medida").FormattedValue, 0, 0, Row.Cells("PrecioLista").Value, Iva, Row.Cells("Neto").Value)
                Else
                    If Listado.Count > 0 Then
                        Dim I As Integer = BuscaEnListadoNotaCredito(Listado, Row.Cells("Articulo").Value, Row.Cells("Precio").Value)
                        If I <> -1 Then
                            Listado.Item(I).Cantidad = Listado.Item(I).Cantidad + Row.Cells("Cantidad").Value
                            Listado.Item(I).TotalItem = Listado.Item(I).TotalItem + Row.Cells("Importe").Value
                            Listado.Item(I).Neto = Listado.Item(I).Neto + Row.Cells("Neto").Value
                        Else
                            AltaItem(Listado, Row.Cells("Articulo").Value, 0, Row.Cells("Precio").Value, Row.Cells("Cantidad").Value, Row.Cells("Importe").Value, Row.Cells("Medida").FormattedValue, 0, 0, Row.Cells("PrecioLista").Value, Iva, Row.Cells("Neto").Value)
                        End If
                    End If
                End If
            End If
        Next

        Return Listado

    End Function
    Private Sub AltaItem(ByVal Listado As List(Of ItemListado), ByVal Articulo As Integer, ByVal TipoPrecio As Integer, ByVal Precio As Decimal, ByVal Cantidad As Decimal, ByVal TotalArticulo As Decimal, ByVal Medida As String, ByVal CodigoCliente As String, ByVal KilosXUnidad As Decimal, ByVal PrecioLista As Decimal, ByVal Iva As Decimal, ByVal Neto As Decimal)

        Dim Item As New ItemListado
        Item.Articulo = Articulo
        Item.TipoPrecio = TipoPrecio
        Item.Precio = Precio
        Item.Cantidad = Cantidad
        Item.TotalItem = TotalArticulo
        Item.Medida = Medida
        Item.CodigoCliente = CodigoCliente
        Item.KilosXUnidad = KilosXUnidad
        Item.PrecioLista = PrecioLista
        Item.Iva = Iva
        Item.Neto = Neto
        Listado.Add(Item)

    End Sub
    Private Function BuscaEnListado(ByVal Listado As List(Of ItemListado), ByVal Articulo As Integer, ByVal PrecioLista As Decimal, ByVal Precio As Decimal) As Integer

        For I As Integer = 0 To Listado.Count - 1
            If Listado.Item(I).Articulo = Articulo And Listado.Item(I).PrecioLista = PrecioLista And Listado.Item(I).Precio = Precio Then Return I
        Next

        Return -1

    End Function
    Private Function BuscaEnListadoNotaCredito(ByVal Listado As List(Of ItemListado), ByVal Articulo As Integer, ByVal Precio As Decimal) As Integer

        For I As Integer = 0 To Listado.Count - 1
            If Listado.Item(I).Articulo = Articulo And Listado.Item(I).Precio = Precio Then Return I
        Next

        Return -1

    End Function
    Public Sub DescomponeNumeroComprobante(ByVal Comprobante As Decimal, ByRef NumeroLetra As Integer, ByRef PuntoDeVenta As Integer, ByRef Numero As Decimal)

        PuntoDeVenta = Strings.Mid(Comprobante, 2, 4)
        Numero = Strings.Right(Comprobante, 8)
        NumeroLetra = Strings.Left(Comprobante.ToString, Comprobante.ToString.Length - 12)

    End Sub
    Public Function CuitNumerico(ByVal Cuit As String) As Decimal

        Return Strings.Left(Cuit, 2) & Strings.Mid(Cuit, 4, 8) & Strings.Right(Cuit, 1)

    End Function
    Public Function HallaCondicionIvaArticulo(ByVal TipoArticulo As Integer, ByVal Articulo As Integer) As Integer

        Dim Sql As String
        Dim Dt As New DataTable
        Dim Res As Integer = 0

        If TipoArticulo = 1 Then
            Sql = "SELECT Exento,NoGrabado From Articulos WHERE Clave = " & Articulo & ";"
        Else
            Sql = "SELECT Exento,NoGrabado From ArticulosServicios WHERE Clave = " & Articulo & ";"
        End If

        If Not Tablas.Read(Sql, Conexion, Dt) Then Return -1
        If Dt.Rows(0).Item("Exento") Then Res = 1
        If Dt.Rows(0).Item("NoGrabado") Then Res = 2

        Dt.Dispose()
        Return Res

    End Function
    Public Function AsignacionLotesAutomatico(ByVal DtCabeza As DataTable, ByVal DtDetalle As DataTable, ByVal Ingreso As Integer, ByRef ListaDeLotes As List(Of FilaAsignacion)) As Boolean

        Dim Dt As New DataTable
        Dim SqlB As String = ""
        Dim SqlN As String = ""

        Dim SqlIngreso As String = ""
        If Ingreso <> 0 Then
            SqlIngreso = " AND Lote = " & Ingreso
        End If

        ListaDeLotes = New List(Of FilaAsignacion)

        Dim Deposito = DtCabeza.Rows(0).Item("Deposito")
        Dim RowsBusqueda() As DataRow
        Dim EsRepetido As Boolean = False

        For Each Row As DataRow In DtDetalle.Rows
            If CDec(Row("Cantidad")) - CDec(Row("Devueltas")) > 0 Then
                If Dt.Rows.Count <> 0 Then
                    RowsBusqueda = Dt.Select("Articulo = " & Row("Articulo"))
                    If RowsBusqueda.Length = 0 Then
                        EsRepetido = False
                    Else
                        EsRepetido = True
                    End If
                End If
                If Not EsRepetido Then


                    'SqlB = "SELECT 1 as Operacion,0.00 as Asignado,Lote,Secuencia,Deposito,LoteOrigen,SecuenciaOrigen,DepositoOrigen,Stock,KilosXUnidad,Proveedor,Fecha,Calibre,PermisoImp,Articulo FROM Lotes WHERE Deposito = " & Deposito & " AND Articulo = " & Row("Articulo") & SqlIngreso &
                    '                                        " ORDER BY lote,secuencia;"
                    'SqlN = "SELECT 2 as Operacion,0.00 as Asignado,Lote,Secuencia,Deposito,LoteOrigen,SecuenciaOrigen,DepositoOrigen,Stock,KilosXUnidad,Proveedor,Fecha,Calibre,PermisoImp,Articulo FROM Lotes WHERE Deposito = " & Deposito & " AND Articulo = " & Row("Articulo") & SqlIngreso &
                    '                                        " ORDER BY lote,secuencia;"


                    'PMERCADO 10-06-2025 Inicio
                    SqlB = " With Stock2 As ( " &
                            " Select  1 As Operacion,0.00 As Asignado,Lote,Secuencia,Deposito,LoteOrigen,SecuenciaOrigen,DepositoOrigen,Stock As Stock, SUM(Stock) OVER (ORDER BY lote, secuencia) As Stock2,KilosXUnidad,Proveedor,Fecha,Calibre,PermisoImp,Articulo " &
                            "   From Lotes " &
                            "  Where Stock <> 0 AND Deposito = " & Deposito & " And Articulo = " & Row("Articulo") & SqlIngreso &
                            " ), " &
                            "  Limite AS ( " &
                            "  Select Min(Stock2) As Corte " &
                            "    From Stock2 " &
                            "   Where Stock2 >= " & Replace(Row("Cantidad"), ",", ".") &
                            " ) " &
                            " Select 1 As Operacion,0.00 As Asignado,Lote,Secuencia,Deposito,LoteOrigen,SecuenciaOrigen,DepositoOrigen,Stock,KilosXUnidad,Proveedor,Fecha,Calibre,PermisoImp,Articulo " &
                            "   From Stock2 " &
                            "  Where Stock2 <= (Select Corte From Limite) " &
                            "  Order By lote, secuencia; "


                    SqlN = " With Stock2 As ( " &
                            " Select  2 As Operacion,0.00 As Asignado,Lote,Secuencia,Deposito,LoteOrigen,SecuenciaOrigen,DepositoOrigen,Stock As Stock, SUM(Stock) OVER (ORDER BY lote, secuencia) As Stock2,KilosXUnidad,Proveedor,Fecha,Calibre,PermisoImp,Articulo " &
                            "   From Lotes " &
                            "  Where Stock <> 0 AND Deposito = " & Deposito & " And Articulo = " & Row("Articulo") & SqlIngreso &
                            " ), " &
                            "  Limite AS ( " &
                            "  Select Min(Stock2) As Corte " &
                            "    From Stock2 " &
                            "   Where Stock2 >= " & Replace(Row("Cantidad"), ",", ".") &
                            " ) " &
                            " Select 2 As Operacion,0.00 As Asignado,Lote,Secuencia,Deposito,LoteOrigen,SecuenciaOrigen,DepositoOrigen,Stock,KilosXUnidad,Proveedor,Fecha,Calibre,PermisoImp,Articulo " &
                            "   From Stock2 " &
                            "  Where Stock2 <= (Select Corte From Limite) " &
                            "  Order By lote, secuencia; "
                    'PMERCADO 10-06-2025 Fin


                    If Not Tablas.Read(SqlB, Conexion, Dt) Then Return False
                    If PermisoTotal Then
                        If Not Tablas.Read(SqlN, ConexionN, Dt) Then Return False
                    End If
                End If
            End If
        Next

        Dim SortOrder As String = "Fecha ASC"

        For Each Row As DataRow In DtDetalle.Rows
            Dim CantidadAAsignar As Decimal = CDec(Row("Cantidad")) - CDec(Row("Devueltas"))
            If CantidadAAsignar <> 0 Then
                RowsBusqueda = Dt.Select("Articulo = " & Row("Articulo"), SortOrder)
                If RowsBusqueda.Length = 0 Then
                    MsgBox("No Existe Lotes Para Asignar al Articulo " & NombreArticulo(Row("Articulo")) & " Operación se CANCELA.", MsgBoxStyle.Critical)
                    ListaDeLotes.Clear() : Dt.Dispose() : Return False
                End If
                Dim AAsignar As Decimal = 0
                For Each Row1 As DataRow In RowsBusqueda
                    Dim YaAsignado As Decimal = CDec(Row1.Item("Stock")) - CDec(Row1.Item("Asignado"))
                    If YaAsignado <> 0 Then
                        If CantidadAAsignar > YaAsignado Then AAsignar = YaAsignado
                        If CantidadAAsignar <= YaAsignado Then AAsignar = CantidadAAsignar
                        Dim Fila As New FilaAsignacion
                        Fila.Indice = Row("Indice")
                        Fila.Lote = Row1("Lote")
                        Fila.Secuencia = Row1("Secuencia")
                        Fila.Deposito = Deposito
                        Fila.Asignado = AAsignar
                        Fila.Operacion = Row1("Operacion")
                        Fila.LoteOrigen = Row1("LoteOrigen")
                        Fila.SecuenciaOrigen = Row1("SecuenciaOrigen")
                        Fila.PermisoImp = Row1("PermisoImp")
                        ListaDeLotes.Add(Fila)
                        CantidadAAsignar = CantidadAAsignar - AAsignar
                        Row1("Asignado") = Row1("Asignado") + AAsignar
                        If CantidadAAsignar = 0 Then Exit For
                    End If
                Next
                If CantidadAAsignar <> 0 Then
                    MsgBox("No Existe Suficientes Cantidad en Lotes Para Asignar al Articulo " & NombreArticulo(Row("Articulo")) & " Operación se CANCELA.", MsgBoxStyle.Critical)
                    ListaDeLotes.Clear() : Dt.Dispose() : RowsBusqueda = Nothing : Return False
                End If
            End If
        Next

        Dt.Dispose()
        RowsBusqueda = Nothing


        Return True

    End Function
    Public Sub HallaEspecieYVariedad(ByVal Articulo As Integer, ByRef Especie As Integer, ByRef Variedad As Integer)

        Dim Dt As New DataTable

        Especie = 0
        Variedad = 0

        Dim SqlStr As String = "SELECT Especie,Variedad FROM Articulos WHERE Clave = " & Articulo & ";"
        Dt = Tablas.Leer(SqlStr)
        If Not Dt.Rows.Count = 0 Then Especie = Dt.Rows(0).Item("Especie") : Variedad = Dt.Rows(0).Item("Variedad")

        Dt.Dispose()

    End Sub
    Public Function FacturaReventaLiquidacion(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As String, ByRef ImporteConIva As Decimal, ByRef ImporteSinIva As Decimal) As Boolean

        Dim Sql As String
        Dim ConexionLote As String
        Dim DtFacturas As DataTable

        ImporteConIva = 0
        ImporteSinIva = 0

        If Operacion = 1 Then
            ConexionLote = Conexion
        Else : ConexionLote = ConexionN
        End If

        'analiza facturas.
        Sql = "SELECT L.ImporteConIva,L.ImporteSinIva FROM FacturasProveedorCabeza AS C INNER JOIN ComproFacturados AS L ON C.Factura = L.Factura " &
                  "WHERE C.EsReventa = 1 AND C.Estado = 1 AND L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & ";"
        DtFacturas = New DataTable
        If Not Tablas.Read(Sql, Conexion, DtFacturas) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(Sql, ConexionN, DtFacturas) Then Return False
        End If
        For Each Row1 As DataRow In DtFacturas.Rows
            ImporteConIva = ImporteConIva + Row1("ImporteConIva")
            ImporteSinIva = ImporteSinIva + Row1("ImporteSinIva")
        Next

        'analiza liquidaciones. 
        Sql = "SELECT 1 AS Operacion,L.NetoConIva,L.NetoSinIva FROM LiquidacionCabeza AS C INNER JOIN LiquidacionDetalle AS L ON C.Liquidacion = L.Liquidacion " &
              "WHERE C.Estado = 1 AND L.Lote = " & Lote & " AND L.Secuencia = " & Secuencia & ";"
        DtFacturas = New DataTable
        If Not Tablas.Read(Sql, Conexion, DtFacturas) Then Return False
        If PermisoTotal Then
            If Not Tablas.Read(Sql, ConexionN, DtFacturas) Then Return False
        End If
        For Each Row1 As DataRow In DtFacturas.Rows
            ImporteConIva = ImporteConIva + Row1("NetoConIva")
            ImporteSinIva = ImporteSinIva + Row1("NetoSinIva")
        Next

        DtFacturas.Dispose()

        Return True

    End Function
    Public Function HallaNumeroFondoFijoRendicion(ByVal Recibo As Double, ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT NumeroFondoFijo FROM RecibosCabeza WHERE TipoNota = 600 AND Nota = " & Recibo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: RecibosCabezas. " & ex.Message, MsgBoxStyle.Critical)
            End
        End Try


    End Function
    Public Function HallaNumeroFondoFijoAjuste(ByVal Recibo As Double, ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Numero FROM MovimientosFondoFijoCabeza WHERE Movimiento = " & Recibo & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: MovimientosFondoFijoCabeza. " & ex.Message, MsgBoxStyle.Critical)
            End
        End Try


    End Function
    Public Function HallaCaeFactura(ByVal Factura As Double) As Decimal

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cae FROM FacturasCabeza WHERE Factura = " & Factura & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: FacturasCabeza. " & ex.Message, MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaCodigoFactura(ByVal Factura As Decimal, ByVal ConexionStr As String) As Integer

        Dim CodigoFactura As Integer = 0

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT EsReventa,EsAfectaCostoLotes,EsInsumos,EsSinComprobante FROM FacturasProveedorCabeza WHERE Factura = " & Factura & ";", ConexionStr, Dt) Then End
        If Dt.Rows.Count = 0 Then
            MsgBox("Factura Proveedor " & NumeroEditado(Factura) & " No Encontrada.")
            Return -1
        End If

        If Dt.Rows(0).Item("EsReventa") Then CodigoFactura = 900
        If Dt.Rows(0).Item("EsAfectaCostoLotes") Then CodigoFactura = 901
        If Dt.Rows(0).Item("EsInsumos") Then CodigoFactura = 902
        If Dt.Rows(0).Item("EsSinComprobante") Then CodigoFactura = 903

        Dt.Dispose()

        Return CodigoFactura

    End Function
    Public Function NombreArticuloYEstado(ByVal Clave As Integer, ByVal ConexionStr As String) As String

        Dim Dta As New DataTable
        Dim Nombre As String = ""

        If Not Tablas.Read("SELECT Nombre,Estado FROM Articulos WHERE Estado = 1 AND Clave = " & Clave & ";", ConexionStr, Dta) Then End
        If Dta.Rows.Count <> 0 Then Nombre = Dta.Rows(0).Item("Nombre")
        Dta.Dispose()
        Return Nombre

    End Function
    Public Function HallaLoteOrigen(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As Integer, ByRef LoteOrigen As Integer, ByRef SecuenciaOrigen As Integer, ByRef DepositoOrigen As Integer, ByRef NombreProveedorW As String, ByRef TipoOperacion As Integer, ByRef KilosXUnidad As Decimal, ByRef FechaIngreso As Date) As Boolean

        Dim ConexionStr As String
        Dim Dt As New DataTable

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT LoteOrigen,SecuenciaOrigen,DepositoOrigen,Proveedor,TipoOperacion,KilosXUnidad,Fecha FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", ConexionStr, Dt) Then Return False
        If Dt.Rows.Count = 0 Then Dt.Dispose() : Return False

        LoteOrigen = Dt.Rows(0).Item("LoteOrigen")
        SecuenciaOrigen = Dt.Rows(0).Item("SecuenciaOrigen")
        DepositoOrigen = Dt.Rows(0).Item("DepositoOrigen")
        NombreProveedorW = NombreProveedor(Dt.Rows(0).Item("Proveedor"))
        TipoOperacion = Dt.Rows(0).Item("TipoOperacion")
        KilosXUnidad = Dt.Rows(0).Item("KilosXUnidad")
        FechaIngreso = Dt.Rows(0).Item("Fecha")

        Dt.Dispose()
        Return True

    End Function
    Public Function PideCarpeta() As String

        Dim Directorio As String = ""

        Dim FolderBrowserDialog1 As New FolderBrowserDialog

        FolderBrowserDialog1.Reset() ' resetea
        FolderBrowserDialog1.Description = " Seleccionar una carpeta "

        ' Path " Mis documentos "
        ' FolderBrowserDialog1.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        ' deshabilita el botón " crear nueva carpeta "  
        FolderBrowserDialog1.ShowNewFolderButton = False
        ' FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop  
        ' FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.StartMenu  

        Dim ret As DialogResult = FolderBrowserDialog1.ShowDialog
        If ret = Windows.Forms.DialogResult.OK Then
            Directorio = FolderBrowserDialog1.SelectedPath
        End If

        FolderBrowserDialog1.Dispose()

        Return Directorio

    End Function
    Public Sub HallaDatosCliente(ByVal Factura As Double, ByRef Nombre As String, ByRef Cuit As Double, ByRef Pais As Integer, ByRef Provincia As Integer, ByRef ComprobanteDesde As Decimal, ByRef ComprobanteHasta As Decimal)

        Dim Dt As New DataTable

        Nombre = ""
        ComprobanteDesde = 0
        ComprobanteHasta = 0

        If Not Tablas.Read("SELECT C.Nombre,C.Cuit,C.Pais,C.Provincia,F.ComprobanteDesde,F.ComprobanteHasta FROM Clientes AS C INNER JOIN FacturasCabeza AS F ON C.Clave = F.Cliente WHERE F.Factura = " & Factura & ";", Conexion, Dt) Then Exit Sub
        If Dt.Rows.Count = 0 Then Exit Sub
        Nombre = Dt.Rows(0).Item("Nombre")
        Cuit = Dt.Rows(0).Item("Cuit")
        Pais = Dt.Rows(0).Item("Pais")
        Provincia = Dt.Rows(0).Item("Provincia")
        ComprobanteDesde = Dt.Rows(0).Item("ComprobanteDesde")
        ComprobanteHasta = Dt.Rows(0).Item("ComprobanteHasta")
        If Dt.Rows(0).Item("Pais") <> 1 Then
            Cuit = HallaCuitPais(Dt.Rows(0).Item("Pais"))
            If Cuit < 0 Then
                Nombre = ""
            End If
        End If

        Dt.Dispose()

    End Sub
    Public Function LetrasPermitidas(ByVal TipoEmisor As Integer, ByVal NumeroLetra As Integer) As Boolean

        ' 1.Cliente
        ' 2.Proveedor

        Select Case TipoEmisor
            Case 2
                Select Case GTipoIvaEmpresa
                    Case 1, 5
                        Select Case NumeroLetra
                            Case 1, 3, 5
                            Case Else
                                MsgBox("Tipo Letra: " & LetraTipoIva(NumeroLetra) & "  No Permitida.")
                                Return False
                        End Select
                    Case 2
                        Select Case NumeroLetra
                            Case 2, 3
                            Case Else
                                MsgBox("Tipo Letra: " & LetraTipoIva(NumeroLetra) & "  No Permitida.")
                                Return False
                        End Select
                    Case Else
                        MsgBox("Tipo Iva Empresa: " & GTipoIvaEmpresa & "  No Contemplada.")
                        Return False
                End Select
        End Select

        Return True

    End Function
    Public Function LetrasPermitidasCliente(ByVal TipoIvaEmisor As Integer, ByVal TipoComprobante As Integer) As Integer

        If TipoIvaEmisor = 4 Then Return 4 'E

        ' Para Empresa Responsable Inscripto A. 
        Dim ComprobantesIA_IA As String = "AAAA"
        Dim ComprobantesIA_IM As String = "AMAM"
        Dim ComprobantesIA_NI As String = "BCBC"
        Dim ComprobantesIA_CF As String = "BCBC"
        Dim ComprobantesIA_MO As String = "ACAC"  'modificacion

        ' Para Empresa Responsable Inscripto M. 
        Dim ComprobantesIM_IA As String = "MAMA"
        Dim ComprobantesIM_IM As String = "MMMM"
        Dim ComprobantesIM_NI As String = "BCBC"
        Dim ComprobantesIM_CF As String = "BCBC"
        Dim ComprobantesIM_MO As String = "MCMC"  'modificacion

        ' Para Empresa EXENTO. 
        Dim ComprobantesNI_IA As String = "CBCB"
        Dim ComprobantesNI_IM As String = "CBCB"
        Dim ComprobantesNI_NI As String = "CCCC"
        Dim ComprobantesNI_CF As String = "CCCC"

        Select Case GTipoIvaEmpresa    'Tipo Iva Empresa que emite.
            Case 1         'Responsable Inscripto A.
                Select Case TipoIvaEmisor    'Tipo Iva Cliente.
                    Case 1           ' Responsable Inscripto A. 
                        Return VerLetraPorComprobante(ComprobantesIA_IA, TipoComprobante)
                    Case 5         ' Responsable Inscripto M.
                        Return VerLetraPorComprobante(ComprobantesIA_IM, TipoComprobante)
                    Case 2         ' 2.Exento. 
                        Return VerLetraPorComprobante(ComprobantesIA_NI, TipoComprobante)
                    Case 3         ' Consumidor Final.
                        Return VerLetraPorComprobante(ComprobantesIA_CF, TipoComprobante)
                    Case 6         ' Monotributo.
                        Return VerLetraPorComprobante(ComprobantesIA_MO, TipoComprobante)
                End Select
            Case 5         'Responsable Inscripto M.
                Select Case TipoIvaEmisor    'Tipo Iva Cliente.
                    Case 1           ' Responsable Inscripto A. 
                        Return VerLetraPorComprobante(ComprobantesIM_IA, TipoComprobante)
                    Case 5         ' Responsable Inscripto M.
                        Return VerLetraPorComprobante(ComprobantesIM_IM, TipoComprobante)
                    Case 2         ' Exento.
                        Return VerLetraPorComprobante(ComprobantesIM_NI, TipoComprobante)
                    Case 3         ' Consumidor Final.
                        Return VerLetraPorComprobante(ComprobantesIM_CF, TipoComprobante)
                    Case 6         ' Monotributo.
                        Return VerLetraPorComprobante(ComprobantesIM_MO, TipoComprobante)
                End Select
            Case 2         'Exento.
                Select Case TipoIvaEmisor    'Tipo Iva Cliente.
                    Case 1           ' Responsable Inscripto A. 
                        Return VerLetraPorComprobante(ComprobantesNI_IA, TipoComprobante)
                    Case 5         ' Responsable Inscripto M.
                        Return VerLetraPorComprobante(ComprobantesNI_IM, TipoComprobante)
                    Case 2         ' Exento.
                        Return VerLetraPorComprobante(ComprobantesNI_NI, TipoComprobante)
                    Case 3         ' Consumidor Final.
                        Return VerLetraPorComprobante(ComprobantesNI_CF, TipoComprobante)
                End Select
            Case Else
                MsgBox("Tipo Iva Empresa: " & GTipoIvaEmpresa & "  No Contemplada.")
                Return False
        End Select

    End Function
    Public Function LetrasPermitidasProveedor(ByVal TipoIvaEmisor As Integer, ByVal TipoComprobante As Integer) As Integer

        If TipoIvaEmisor = 4 Then Return 4 'E

        ' Para Empresa Responsable Inscripto A. 
        Dim ComprobantesIA_IA As String = "AAAA"
        Dim ComprobantesIA_IM As String = "MAAM"
        Dim ComprobantesIA_NI As String = "CBBC"

        ' Para Empresa Responsable Inscripto M. 
        Dim ComprobantesIM_IA As String = "AMMA"
        Dim ComprobantesIM_IM As String = "MMMM"
        Dim ComprobantesIM_NI As String = "CBBC"
        Dim ComprobantesIM_CF As String = "BCBC"

        ' Para Empresa Exento. 
        Dim ComprobantesNI_IA As String = "BCCB"
        Dim ComprobantesNI_IM As String = "BCCB"
        Dim ComprobantesNI_NI As String = "CCCC"

        Select Case GTipoIvaEmpresa    'Tipo Iva Empresa.
            Case 1         'Responsable Inscripto A.
                Select Case TipoIvaEmisor    'Tipo Iva Cliente.
                    Case 1           ' Responsable Inscripto A. 
                        Return VerLetraPorComprobante(ComprobantesIA_IA, TipoComprobante)
                    Case 5         ' Responsable Inscripto M.
                        Return VerLetraPorComprobante(ComprobantesIA_IM, TipoComprobante)
                    Case 2         ' Exento.
                        Return VerLetraPorComprobante(ComprobantesIA_NI, TipoComprobante)
                End Select
            Case 5         'Responsable Inscripto M.
                Select Case TipoIvaEmisor    'Tipo Iva Cliente.
                    Case 1           ' Responsable Inscripto A. 
                        Return VerLetraPorComprobante(ComprobantesIM_IA, TipoComprobante)
                    Case 5         ' Responsable Inscripto M.
                        Return VerLetraPorComprobante(ComprobantesIM_IM, TipoComprobante)
                    Case 2         ' Exento.
                        Return VerLetraPorComprobante(ComprobantesIM_NI, TipoComprobante)
                End Select
            Case 2         'Exento.
                Select Case TipoIvaEmisor    'Tipo Iva Cliente.
                    Case 1           ' Responsable Inscripto A. 
                        Return VerLetraPorComprobante(ComprobantesNI_IA, TipoComprobante)
                    Case 5         ' Responsable Inscripto M.
                        Return VerLetraPorComprobante(ComprobantesNI_IM, TipoComprobante)
                    Case 2         ' Exento.
                        Return VerLetraPorComprobante(ComprobantesNI_NI, TipoComprobante)
                End Select
            Case Else
                MsgBox("Tipo Iva Empresa: " & GTipoIvaEmpresa & "  No Contemplada.")
                Return False
        End Select

    End Function
    Private Function VerLetraPorComprobante(ByVal Str As String, ByVal TipoComprobante As Integer) As Integer

        '  Tipo Comprobantes:
        '                      1        - Factura Venta.
        '                      800      - N.V.L.P.
        '                      5,7      - Notas al Cliente.
        '                      50,70    - Notas del Cliente.
        '                      5000     - Factura Proveedor.
        '                      100      - Liquidacion a Proveedores
        '                      6,8      - Notas Al Proveedor.
        '                      500,700  - Notas Al Proveedor.

        Select Case TipoComprobante
            Case 1          'Factura Venta.
                Return HallaNumeroLetra(Strings.Mid(Str, 1, 1))
            Case 800        'NVLP.
                Return HallaNumeroLetra(Strings.Mid(Str, 2, 1))
            Case 5, 7       'Notas al Cliente.
                Return HallaNumeroLetra(Strings.Mid(Str, 3, 1))
            Case 50, 70     'Notas del Cliente.
                Return HallaNumeroLetra(Strings.Mid(Str, 4, 1))
            Case 5000       'Factura Proveedor.
                Return HallaNumeroLetra(Strings.Mid(Str, 1, 1))
            Case 100        'Liquidacion Proveedor.
                Return HallaNumeroLetra(Strings.Mid(Str, 2, 1))
            Case 6, 8       'Notas al Proveedor.
                Return HallaNumeroLetra(Strings.Mid(Str, 3, 1))
            Case 500, 700   'Notas del Proveedor.
                Return HallaNumeroLetra(Strings.Mid(Str, 4, 1))
        End Select

    End Function
    Public Function EmpresaOk() As Boolean

        'Natural Box:30-71415737-6 Cuadro Norte:30-71499881-8

        If GCuitEmpresa <> MercadoCentral And GCuitEmpresa <> Fruticola And GCuitEmpresa <> "30-71415737-6" Then
            Return True
        End If

    End Function
    Public Function HallaUltimaNumeracionDebitoCreditoW(ByVal TipoNota As Integer, ByVal TipoIva As Integer, ByVal PuntoVenta As Integer, ByVal ConexionStr As String) As Double

        Dim Patron As String = TipoIva & Format(PuntoVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Nota) FROM RecibosCabeza WHERE TipoNota = " & TipoNota & " AND CAST(CAST(RecibosCabeza.Nota AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo)
                    Else : Return CDbl(TipoIva & Format(GPuntoDeVenta, "0000") & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function ArmaListaIvaPorValor(ByRef Lista As List(Of AutorizacionAFIP.ItemIva), ByVal BaseW As Decimal, ByVal Iva As Decimal, ByVal ImpIva As Decimal) As Decimal

        Dim TotalIva As Decimal = 0

        Dim NewItem As New AutorizacionAFIP.ItemIva
        NewItem.Base = BaseW
        NewItem.Clave = 0
        NewItem.Importe = ImpIva
        NewItem.Iva = Iva

        Dim Esta As Boolean = False
        For Each Item As AutorizacionAFIP.ItemIva In Lista
            If Item.Iva = Iva Then
                Item.Base = Item.Base + BaseW
                Esta = True
            End If
        Next

        If Not Esta Then Lista.Add(NewItem)

        For Each Item As AutorizacionAFIP.ItemIva In Lista
            Item.Importe = CalculaIva(1, Item.Base, Item.Iva)
            TotalIva = TotalIva + Item.Importe
        Next

        Return TotalIva

    End Function
    Public Function ValidaStringNombres(ByVal Str As String) As String

        If Str = "" Then Return ""

        For I As Integer = 1 To Str.Length
            Dim W As String = Strings.Mid(Str, I, 1)
            If Not (Asc(W) = 46 Or Asc(W) = 8 Or Asc(W) = 13 Or Asc(W) = 32 Or Asc(W) = 40 Or Asc(W) = 41 Or (Asc(W) >= 48 And Asc(W) <= 57) Or (Asc(W) >= 64 And Asc(W) <= 90) Or (Asc(W) >= 97 And Asc(W) <= 122) Or (Asc(W) >= 164 And Asc(W) <= 165)) Then
                MsgBox("Caracter " & W & " no permitido.", MsgBoxStyle.Exclamation)
                Return W
            End If
        Next

        Return ""

    End Function
    Public Function ValidaTextBoxDeRemitos(ByVal TextRemitos As TextBox) As Boolean

        If TextRemitos.Lines.Length <> 0 Then
            For Each strLine As String In TextRemitos.Text.Split(vbNewLine)
                Dim MiArray() As String
                MiArray = Split(strLine, "-")
                If MiArray.Length > 2 Then
                    MsgBox("Erroneo Numero de Remito : " & strLine, MsgBoxStyle.Exclamation)
                    Return False
                End If
                If MiArray.Length = 2 Then
                    If MiArray(0).Length > 4 Then
                        MsgBox("Punto de venta erroneo en Numero de Remito : " & strLine, MsgBoxStyle.Exclamation)
                        Return False
                    End If
                    If Val(MiArray(0)) = 0 Then
                        MsgBox("Punto de venta erroneo en Numero de Remito : " & strLine, MsgBoxStyle.Exclamation)
                        Return False
                    End If
                    If MiArray(1).Length > 8 Or MiArray(1).Length < 8 Then
                        MsgBox("Nunmero a la derecha del guion debe tener 8 digitos en Numero de Remito : " & strLine, MsgBoxStyle.Exclamation)
                        Return False
                    End If
                Else
                    If strLine.Length > 12 Then
                        MsgBox("Numero de Guia no debe tener mas de 12 digitos : " & strLine, MsgBoxStyle.Exclamation)
                        Return False
                    End If
                End If
            Next
        End If

        Return True

    End Function
    Public Function ValidaTextBoxComprobantes(ByVal Text As TextBox, ByRef ListaDeComprobantes As List(Of Decimal)) As Boolean

        For Each strLine As String In Text.Text.Split(vbNewLine)
            Dim MiArray() As String
            If strLine <> "" Then
                If Asc(Strings.Left(strLine, 1)) = 10 Then strLine = Strings.Mid(strLine, 2, strLine.Length - 1) 'Excluye la primera posicion si es ascii=10
            End If
            strLine = Trim(strLine)
            strLine = Strings.Replace(strLine, " ", "")
            If strLine <> "" Then
                If Asc(Strings.Left(strLine, 1)) = 10 Then strLine = Strings.Mid(strLine, 2, strLine.Length - 1) 'Excluye la primera posicion si es ascii=10
                MiArray = Split(strLine, "-")
                If MiArray.Length = 1 Then
                    MsgBox("Falta Guion en Comprobante : " & strLine, MsgBoxStyle.Exclamation)
                    Return False
                End If
                If MiArray.Length <> 2 Then
                    MsgBox("Erroneo Numero en Comprobante : " & strLine, MsgBoxStyle.Exclamation)
                    Return False
                End If
                If MiArray(0).Length <> 5 And MiArray(0).Length <> 4 Then
                    MsgBox("Punto de Venta erroneo en Comprobante : " & strLine, MsgBoxStyle.Exclamation)
                    Return False
                End If
                If MiArray(0).Length = 4 Then
                    MiArray(0) = "0" & Strings.Right(MiArray(0), 4)
                End If
                If MiArray(0).Length = 5 Then
                    Dim Primer As String = Strings.Left(MiArray(0), 1)
                    If Primer <> "0" Then
                        Dim nn As String = HallaNumeroLetra(Strings.Left(MiArray(0), 1))
                        If nn = 0 Then
                            MsgBox("Punto de Venta erroneo en Comprobante : " & strLine, MsgBoxStyle.Exclamation)
                            Return False
                        End If
                        MiArray(0) = nn & Strings.Right(MiArray(0), 4)
                    End If
                End If
                If Not IsNumeric(MiArray(0)) Then
                    MsgBox("Punto de venta no Numérico en Comprobante : " & strLine, MsgBoxStyle.Exclamation)
                    Return False
                End If
                If Val(MiArray(0)) = 0 Then
                    MsgBox("Punto de venta erroneo en Comprobante : " & strLine, MsgBoxStyle.Exclamation)
                    Return False
                End If
                If MiArray(1).Length > 8 Or MiArray(1).Length < 8 Then
                    MsgBox("Numero a la derecha del guion debe tener 8 digitos en Comprobante : " & strLine, MsgBoxStyle.Exclamation)
                    Return False
                End If
                If Not IsNumeric(MiArray(1)) Then
                    MsgBox("Numero no Numérico en Comprobante : " & strLine, MsgBoxStyle.Exclamation)
                    Return False
                End If
                If Val(MiArray(1)) = 0 Then
                    MsgBox("Numero Incorrecto en Comprobante : " & strLine, MsgBoxStyle.Exclamation)
                    Return False
                End If
                ListaDeComprobantes.Add(CDec(MiArray(0) & MiArray(1)))
            End If
        Next

        Return True

    End Function
    Public Function ActualizaPedido(ByVal Pedido As Integer, ByVal Funcion As String, ByRef DtPedido As DataTable, ByVal ListaParaPedido As List(Of ItemPedido)) As Boolean

        Dim RowsBusqueda() As DataRow
        DtPedido = New DataTable

        If Pedido = 0 Then Return True

        If Not Tablas.Read("SELECT * FROM PedidosDetalle WHERE Pedido = " & Pedido & ";", Conexion, DtPedido) Then Return False

        For Each Fila As ItemPedido In ListaParaPedido
            RowsBusqueda = DtPedido.Select("Articulo = " & Fila.Articulo)
            If RowsBusqueda.Length <> 0 Then
                If Funcion = "A" Then
                    RowsBusqueda(0).Item("Entregada") = CDec(RowsBusqueda(0).Item("Entregada")) + Fila.Cantidad
                End If
                If Funcion = "B" Then
                    RowsBusqueda(0).Item("Entregada") = CDec(RowsBusqueda(0).Item("Entregada")) - Fila.Cantidad
                End If
            End If
        Next

        Return True

    End Function
    Public Function AnalizaPropiedadesArticulo(ByVal Cliente As Integer, ByVal Articulo As Integer, ByVal KilosXUnidad As Decimal, ByVal FechaEntrega As Date, ByVal TieneCodigoCliente As Boolean, ByVal Lista As Integer, ByVal Pedido As Integer, ByRef Cantidad As Decimal, ByRef TipoPrecio As Integer, ByRef Precio As Decimal, ByRef Codigo As String, ByRef ArticuloExisteEnPedido As Boolean) As Boolean


        Cantidad = 0
        Precio = 0
        TipoPrecio = 0
        Codigo = ""

        If TieneCodigoCliente Then
            Codigo = HallaCodigoCliente(Cliente, Articulo)
            If Codigo = "-1" Then Return False
        End If

        If Pedido <> 0 Then
            Dim Entregada As Decimal
            HallaCantidadYPrecioPedido(Pedido, Articulo, Cantidad, Entregada, Precio, TipoPrecio, ArticuloExisteEnPedido)
            Cantidad = Cantidad - Entregada
            If Cantidad < 0 Then Cantidad = 0
        End If

        If Lista <> 0 Then
            HallaPrecioDeListaSegunArticulo(Lista, FechaEntrega, Articulo, Precio, TipoPrecio)
            If Precio = -1 Then Return False
        End If


        Return True

    End Function
    Public Sub HallaPrecioDeListaSegunArticulo(ByVal Lista As Integer, ByVal Fecha As Date, ByVal Articulo As Integer, ByRef Precio As Double, ByRef TipoPrecio As Integer)

        Precio = 0
        TipoPrecio = 0

        Dim DiaStr As String = ""

        If Weekday(Fecha) = 1 Then DiaStr = "Domingo"
        If Weekday(Fecha) = 2 Then DiaStr = "Lunes"
        If Weekday(Fecha) = 3 Then DiaStr = "Martes"
        If Weekday(Fecha) = 4 Then DiaStr = "Miercoles"
        If Weekday(Fecha) = 5 Then DiaStr = "Jueves"
        If Weekday(Fecha) = 6 Then DiaStr = "Viernes"
        If Weekday(Fecha) = 7 Then DiaStr = "Sabado"

        Dim Dt As New DataTable

        Dim Sql As String = "SELECT PorUnidad,Final," & DiaStr & " AS Precio,TipoPrecio FROM ListaDePreciosCabeza AS C INNER JOIN ListaDePreciosDetalle As D ON C.Lista = D.Lista WHERE C.Lista = " & Lista & " AND D.Articulo = " & Articulo & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            Precio = Dt.Rows(0).Item("Precio")
            TipoPrecio = Dt.Rows(0).Item("TipoPrecio")
        End If

    End Sub
    Public Function LicenciaVencida(ByVal Fecha As Date) As Boolean

        If GFechaVencimiento = "1800/01/01" Then Return False
        If DiferenciaDias(Fecha, GFechaVencimiento) <= 0 Then Return True

    End Function
    Public Function ExisteReciboOficial(ByVal TipoNota As Integer, ByVal Nota As Decimal, ByVal Emisor As Integer, ByVal ReciboOficial As Decimal, ByVal ConexionStr As String) As Boolean

        Dim Sql As String
        Dim NotaW As Decimal

        Select Case TipoNota
            Case 5000     'Facturas Otros Proveedores.
                Sql = "SELECT Factura FROM OtrasFacturasCabeza WHERE Estado <> 3 AND Factura <> " & Nota & " AND Proveedor = " & Emisor & " AND ReciboOficial = " & ReciboOficial & ";"
            Case 3000     'Gastos bancarios.
                Sql = "SELECT Movimiento FROM GastosBancarioCabeza WHERE Estado <> 3 AND Movimiento <> " & Nota & " AND Banco = " & Emisor & " AND ReciboOficial = " & ReciboOficial & ";"
            Case 1010     'Cancelacion Prestamo.
                Sql = "SELECT Movimiento FROM PrestamosMovimientoCabeza WHERE Estado <> 3 AND TipoNota = 1010 AND Movimiento <> " & Nota & " AND Prestamo = " & Emisor & " AND ReciboOficial = " & ReciboOficial & ";"
            Case Else     'Notas Debito/Creditos.
                Sql = "SELECT Nota FROM RecibosCabeza WHERE Estado <> 3 AND TipoNota = " & TipoNota & " AND Nota <> " & Nota & " AND Emisor = " & Emisor & " AND ReciboOficial = " & ReciboOficial & ";"
        End Select

        GExcepcion = HallaDatoGenerico(Sql, ConexionStr, NotaW)
        If Not IsNothing(GExcepcion) Then
            MsgBox("Error al Leer Tabla." + vbCrLf + vbCrLf + GExcepcion.Message)
            Return -10
            Exit Function
        End If

        If NotaW = 0 Then
            Return False
        Else
            Return True
        End If

    End Function
    Public Function ExisteOrdenCompra(ByVal OrdenCompra As Double, ByVal ConexionStr As String) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(OrdenCompra) FROM IngresoMercaderiasCabeza WHERE OrdenCompra = " & OrdenCompra & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then
                        Return True
                    Else : Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error de Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        Finally
        End Try

    End Function
    Public Function HallaDatoGenerico(ByVal Sql As String, ByVal ConexionStr As String, ByRef Resultado As Object) As Exception

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Resultado = Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return ex
        Finally
        End Try

    End Function
    Public Function EsReemplazoCheque(ByVal TipoNota As Integer, ByVal Nota As Double, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ClaveChequeReemplazado FROM RecibosCabeza WHERE TipoNota = " & TipoNota & " AND Nota = " & Nota & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer RecibosCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function EsReemplazoChequePrestamo(ByVal Movimiento As Double, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ClaveChequeReemplazado FROM PrestamosMovimientoCabeza WHERE Movimiento = " & Movimiento & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Proveedores.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function EsReemplazoChequeSueldos(ByVal TipoNota As Integer, ByVal Nota As Double, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ClaveChequeReemplazado FROM SueldosMovimientoCabeza WHERE Movimiento = " & Nota & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Proveedores.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function EsReemplazoChequeOtrosProveedores(ByVal TipoNota As Integer, ByVal Nota As Double, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ClaveChequeReemplazado FROM OtrosPagosCabeza WHERE Movimiento = " & Nota & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Proveedores.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function EsReemplazoChequeCompraDivisas(ByVal Movimiento As Double, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ClaveChequeReemplazado FROM CompraDivisasCabeza WHERE Movimiento = " & Movimiento & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Proveedores. " & ex.Message, MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function EsReemplazoChequeFondoFijo(ByVal Nota As Double, ByVal Abierto As Boolean) As Boolean

        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT ClaveChequeReemplazado FROM MovimientosFondoFijoCabeza WHERE Movimiento = " & Nota & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Proveedores.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function ExisteFactura(ByVal Factura As Decimal, ByVal ConexionStr As String) As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Factura FROM FacturasCabeza WHERE Factura = " & Factura & ";", ConexionStr, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            Dt.Dispose()
            Return True
        End If

        Dt.Dispose()

    End Function
    Public Function ExisteRemito(ByVal Remito As Decimal, ByVal ConexionStr As String) As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Remito FROM RemitosCabeza WHERE Remito = " & Remito & ";", ConexionStr, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            Dt.Dispose()
            Return True
        End If

        Dt.Dispose()

    End Function
    Public Function HallaFacturaRelacionada(ByVal Factura As Decimal, ByVal ConexionStr As String) As Decimal

        Dim Dt As New DataTable
        Dim Relacionada As Decimal = 0

        If Not Tablas.Read("SELECT Relacionada FROM FacturasCabeza WHERE Factura = " & Factura & ";", ConexionStr, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            Relacionada = Dt.Rows(0).Item("Relacionada")
        End If

        Dt.Dispose()

        Return Relacionada

    End Function
    Public Function ValidaFechaContable(ByVal Fecha As Date, ByVal UltimaFechaContableW As Date) As Boolean

        If Not ConsisteFecha(Fecha) Then
            MsgBox("Fecha Contable Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If DiferenciaDias(Fecha, Date.Now) < 0 Then
            MsgBox("Fecha Contable Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If CierreContableCerrado(Fecha.Month, Fecha.Year) Then
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If DiferenciaDias(Fecha, UltimaFechaContableW) > 0 Then
            MsgBox("Fecha Contable Menor a la Ultima Fecha Grabada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    Public Function HallaCondicionIva(ByVal TipoIva As Integer) As String

        Select Case TipoIva
            Case 1, 5
                Return "RESPONSABLE INSCRIPTO"
            Case 2
                Return "EXENTO"
            Case 3
                Return "CONSUMIDOR FINAL"
            Case 4
                Return "EXTERIOR"
        End Select

    End Function
    Public Function EsRemitoManual(ByVal PuntoDeVenta As Integer) As Boolean

        Dim Dt As New DataTable
        Dim EsManual As Boolean

        If Not Tablas.Read("SELECT EsRemitoManual FROM PuntosDeVenta WHERE Clave = " & PuntoDeVenta & ";", Conexion, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            EsManual = Dt.Rows(0).Item("EsRemitoManual")
        End If

        Dt.Dispose()

        Return EsManual

    End Function
    Public Function HallaMesesDelAñio() As DataTable

        Dim DtMeses As New DataTable

        Dim Mes1 As New DataColumn("Mes")
        Mes1.DataType = System.Type.GetType("System.Int32")
        DtMeses.Columns.Add(Mes1)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtMeses.Columns.Add(Nombre)

        Dim Row As DataRow = DtMeses.NewRow
        Row("Mes") = 1
        Row("Nombre") = "Enero"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 2
        Row("Nombre") = "Febrero"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 3
        Row("Nombre") = "Marzo"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 4
        Row("Nombre") = "Abril"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 5
        Row("Nombre") = "Mayo"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 6
        Row("Nombre") = "Junio"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 7
        Row("Nombre") = "Julio"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 8
        Row("Nombre") = "Agosto"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 9
        Row("Nombre") = "Septiembre"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 10
        Row("Nombre") = "Octubre"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 11
        Row("Nombre") = "Noviembre"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 12
        Row("Nombre") = "Diciembre"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 0
        Row("Nombre") = ""
        DtMeses.Rows.Add(Row)

        Return DtMeses

    End Function
    Public Function TraeCopiasComprobante(ByVal TipoComprobante As Integer, ByVal PuntoVenta As Integer) As Integer

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT CopiasRemitos,CopiasFacturas,CopiasDebitosCreditos,CopiasLiquidaciones FROM PuntosDeVenta WHERE Clave = " & PuntoVenta & ";", Conexion, Dt) Then Dt.Dispose() : Return -200
        If Dt.Rows.Count = 0 Then
            Dt.Dispose()
            Return -100
        End If

        Dim Respuesta As Integer

        Select Case TipoComprobante
            Case 1
                Respuesta = Dt.Rows(0).Item("CopiasRemitos")
            Case 2
                Respuesta = Dt.Rows(0).Item("CopiasFacturas")
            Case 3
                Respuesta = Dt.Rows(0).Item("CopiasDebitosCreditos")
            Case 4
                Respuesta = Dt.Rows(0).Item("CopiasLiquidaciones")
        End Select

        Dt.Dispose()
        Return Respuesta

    End Function
    Public Function PideDatosPedido(ByVal FechaEntrega As Date, ByVal Cliente As Integer, ByVal Sucursal As Integer, ByVal Cerrado As Boolean, ByVal Abierto As Boolean, ByRef Dt As DataTable) As Boolean

        Dt = New DataTable

        Dim SqlFecha As String = ""
        SqlFecha = "AND FechaEntregaDesde <='" & Format(FechaEntrega, "yyyyMMdd") & "' AND FechaEntregaHasta >='" & Format(FechaEntrega, "yyyyMMdd") & "'"

        Dim SqlSucursal As String = ""
        If Sucursal <> 0 Then
            SqlSucursal = " AND (Sucursal = 0 OR Sucursal = " & Sucursal & ")"
        End If
        Dim SqlCerrado As String = ""
        If Cerrado Then
            SqlCerrado = " AND Cerrado = 1"
        End If
        If Abierto Then
            SqlCerrado = " AND Cerrado = 0"
        End If
        If Cerrado And Abierto Then
            SqlCerrado = ""
        End If

        Dim Sql As String = "SELECT Pedido,Sucursal,FechaEntregaDesde,FechaEntregaHasta,PedidoCliente,Cerrado,PorUnidad,Final FROM PedidosCabeza WHERE Cliente = " & Cliente & SqlFecha & SqlSucursal & SqlCerrado & ";"

        If Not Tablas.Read(Sql, Conexion, Dt) Then Exit Function

        Return True

    End Function
    Public Sub HallaTipoAsientoFactura(ByVal EsExterior As Boolean, ByVal EsServicios As Boolean, ByVal EsSecos As Boolean, ByVal EsContable As Boolean, ByRef TipoAsiento As Integer, ByRef TipoAsientoCosto As Integer)

        If EsExterior Then
            TipoAsiento = 7006
            TipoAsientoCosto = 6071
        Else : TipoAsiento = 2
            TipoAsientoCosto = 6070
        End If
        If EsServicios Then
            TipoAsiento = 7009
        End If
        If EsSecos Then
            TipoAsiento = 7010
        End If
        If EsContable Then
            TipoAsiento = 71001
        End If

    End Sub
    Public Function PatenteOk(ByVal Patente As String) As Boolean

        If Patente.Length <> 6 Then Return False
        If IsNumeric(Strings.Mid(Patente, 1, 1)) Then Return False
        If IsNumeric(Strings.Mid(Patente, 2, 1)) Then Return False
        If IsNumeric(Strings.Mid(Patente, 3, 1)) Then Return False
        If Not IsNumeric(Strings.Right(Patente, 3)) Then Return False

        Return True

    End Function
    Public Function TieneClientesFCE() As Boolean

        Dim Sql As String = "SELECT COUNT(Clave) FROM Clientes WHERE EsFCE = 1;"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla Clientes.", MsgBoxStyle.Critical)
            End
        End Try
    End Function
    Public Function EsProveedorDelGupo(ByVal Proveedor As Integer) As Boolean

        Dim DT As New DataTable
        If Not Tablas.Read("SELECT Clave FROM Proveedores WHERE EsDelGrupo = 1 AND Clave = " & Proveedor & ";", Conexion, DT) Then End
        If DT.Rows.Count <> 0 Then EsProveedorDelGupo = True
        DT.Dispose()

    End Function
    Public Function EsClienteDelGupo(ByVal Proveedor As Integer) As Boolean

        Dim DT As New DataTable
        If Not Tablas.Read("SELECT Clave FROM Clientes WHERE EsDelGrupo = 1 AND Clave = " & Proveedor & ";", Conexion, DT) Then End
        If DT.Rows.Count <> 0 Then EsClienteDelGupo = True
        DT.Dispose()

    End Function
    Public Function HallaOPR(ByVal Clave As Integer, ByVal Tipo As String) As Boolean

        If Tipo = "C" Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(Conexion)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT OPR FROM Clientes WHERE Clave = " & Clave & ";", Miconexion)
                        Return Cmd.ExecuteScalar()
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error a leer Tabla: Clientes.") : End
            End Try
        End If

        If Tipo = "P" Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(Conexion)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT OPR FROM Proveedores WHERE Clave = " & Clave & ";", Miconexion)
                        Return Cmd.ExecuteScalar()
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error a leer Tabla: Proveedores.") : End
            End Try
        End If

    End Function
    Public Function ExisteCuentaBancaria(ByVal Banco As Integer, ByVal Cuenta As Decimal) As Decimal

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Numero FROM CuentasBancarias WHERE Banco = " & Banco & " AND Numero = " & Cuenta & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
        Finally
        End Try

    End Function
    Public Function PideCaja() As Integer

        Dim CajaStr As String = ""
        Dim Ok As Boolean = False

        Do Until Ok = True
            CajaStr = InputBox("Ingrese Numero de Caja: ", "Saldos-Iniciales.", "", )
            If CajaStr = "" Then Return 0
            Ok = True
            For I As Integer = 1 To CajaStr.Length
                If Not (Mid(CajaStr, I, 1) <= "9" And Mid(CajaStr, I) >= "0") Then
                    MsgBox("Incorrecto Numero de Caja.", MsgBoxStyle.Critical)
                    Ok = False
                    Exit For
                End If
            Next
            If Ok Then
                If CajaStr.Length > 4 Then
                    MsgBox("Incorrecta Cantidad de Digitos.", MsgBoxStyle.Critical)
                    Ok = False
                End If
            End If
        Loop

        Return CInt(CajaStr)

    End Function
    Public Function HallaPrecioOrdenCompra(ByVal OrdenCompra As Decimal, ByVal Articulo As Integer) As Decimal

        Dim Dt As New DataTable
        Dim Precio As Decimal

        If Not Tablas.Read("SELECT TotalArticulo,Cantidad FROM OrdenCompraCabeza AS C INNER JOIN OrdenCompraDetalle AS D ON C.Orden = D.Orden WHERE C.Estado = 1 AND C.Orden = " & OrdenCompra & " AND Articulo = " & Articulo & ";", Conexion, Dt) Then
            MsgBox("Se produjo un error al leer Tabla: OrdenCompraCabeza/OrdenCompraDetalle.")
        Else
            Precio = Trunca(Dt.Rows(0).Item("TotalArticulo") / Dt.Rows(0).Item("Cantidad"))
        End If

        Dt.Dispose()
        Return Precio

    End Function
    Public Function HallaOrdenCompraYPrecio(ByVal Proveedor As Integer, ByVal Articulo As Integer, ByVal Operacion As Integer, ByVal Lote As Integer) As Decimal

        Dim DT As New DataTable
        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT OrdenCompra FROM IngresoMercaderiasCabeza WHERE Lote = " & Lote & ";", ConexionStr, DT) Then
            MsgBox("Error Lectura Tabla:IngresoMercaderiasCabeza.") : End
        End If
        Dim OrdenCompra As Decimal = DT.Rows(0).Item("OrdenCompra")
        DT.Dispose()
        If OrdenCompra = 0 Then Return 0

        Return HallaPrecioOrdenCompra(OrdenCompra, Articulo)

    End Function
    Public Sub ActualizoConCodigoCliente(ByVal DtArticulo As DataTable, ByVal Cliente As Integer)

        Dim dt As DataTable
        Dim RowsBusqueda() As DataRow

        dt = HallaArticulosConCodigo(Cliente)
        For Each Row As DataRow In DtArticulo.Rows
            RowsBusqueda = dt.Select("Articulo = " & Row("Clave"))
            If RowsBusqueda.Length = 0 Then
                Row.Delete()
            End If
        Next
        dt.Dispose()

    End Sub
    Public Function TieneCodigoCliente(ByVal Cliente As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT TieneCodigoCliente FROM Clientes WHERE Clave = " & Cliente & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
        Finally
        End Try

    End Function
    Public Sub HallaPrecioDeListaSegunArticuloConTipoPrecio(ByVal Lista As Integer, ByVal Fecha As Date, ByVal Articulo As Integer, ByRef Precio As Double, ByRef TipoPrecio As Integer, ByRef Final As Boolean)

        Precio = 0
        TipoPrecio = 0

        Dim DiaStr As String = ""

        If Weekday(Fecha) = 1 Then DiaStr = "Domingo"
        If Weekday(Fecha) = 2 Then DiaStr = "Lunes"
        If Weekday(Fecha) = 3 Then DiaStr = "Martes"
        If Weekday(Fecha) = 4 Then DiaStr = "Miercoles"
        If Weekday(Fecha) = 5 Then DiaStr = "Jueves"
        If Weekday(Fecha) = 6 Then DiaStr = "Viernes"
        If Weekday(Fecha) = 7 Then DiaStr = "Sabado"

        Dim Dt As New DataTable

        Dim Sql As String = "SELECT PorUnidad,Final," & DiaStr & " AS Precio,TipoPrecio FROM ListaDePreciosCabeza AS C INNER JOIN ListaDePreciosDetalle As D ON C.Lista = D.Lista WHERE C.Lista = " & Lista & " AND D.Articulo = " & Articulo & ";"
        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            Precio = Dt.Rows(0).Item("Precio")
            TipoPrecio = Dt.Rows(0).Item("TipoPrecio")
            Final = Dt.Rows(0).Item("Final")
        End If

    End Sub
    Public Function HallaLiquidacionLote(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As Integer) As Decimal

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Dim Miconexion As New OleDb.OleDbConnection(ConexionStr)

        Try
            Miconexion.Open()
            Using Cmd As New OleDb.OleDbCommand("SELECT Liquidado FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", Miconexion)
                Return Cmd.ExecuteScalar()
            End Using
        Catch ex As Exception
        Finally
            If Miconexion.State = ConnectionState.Open Then Miconexion.Close()
        End Try

    End Function
    Public Sub AsignacionDevoluciones(ByVal Grid As DataGridView, ByRef ListaDeLotes As List(Of FilaAsignacion))

        If Grid.Rows.Count = 0 Then Exit Sub

        Dim AAsignar As Decimal
        Dim YaAsignados As Decimal
        Dim RestanAsignar As Decimal
        Dim CantidadFilasCero As Integer
        Dim HuboAumento As Boolean = False

        For Each FilaGrilla As DataGridViewRow In Grid.Rows
            If FilaGrilla.Cells("Cantidad").Value = 0 Then CantidadFilasCero = CantidadFilasCero + 1 : Continue For
            If FilaGrilla.Cells("Cantidad").Value < 0 Then HuboAumento = True : Exit For

            If EsArticuloAjustado(FilaGrilla.Cells("Indice").Value, FilaGrilla.Cells("Cantidad").Value, ListaDeLotes) Then Continue For
            BlanqueaLista(FilaGrilla.Cells("Indice").Value, ListaDeLotes)

            AAsignar = FilaGrilla.Cells("Cantidad").Value
            YaAsignados = 0
            RestanAsignar = 0

            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Indice = FilaGrilla.Cells("Indice").Value Then
                    RestanAsignar = AAsignar - Fila.Asignado
                    If RestanAsignar < 0 Then Fila.Devolucion = AAsignar : Exit For
                    Fila.Devolucion = Fila.Asignado
                    If RestanAsignar = 0 Then Exit For
                    AAsignar = RestanAsignar
                End If
            Next
        Next

        If HuboAumento Then MsgBox("AJUSTE AUTOMATICO NO VALIDO CON AUMENTO! (DEBE INFORMARLO MANUALMENTE). Operación Se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Exit Sub
        If CantidadFilasCero = Grid.Rows.Count Then MsgBox("Debe Informar Cantidad a Devolver. Operación se CANCELA.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR!") : Exit Sub

        MsgBox("Lotes Ajustados Exitosamente!", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Exito!")

    End Sub
    Private Sub BlanqueaLista(ByVal Indice As Integer, ByRef ListaDeLotes As List(Of FilaAsignacion))

        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Indice = Indice Then Fila.Devolucion = 0
        Next

    End Sub
    Private Function EsArticuloAjustado(ByVal Indice As Integer, ByVal Cantidad As Decimal, ByRef ListaDeLotes As List(Of FilaAsignacion)) As Boolean

        Dim CantidadDevuelto As Decimal = 0

        For Each FilaLotes As FilaAsignacion In ListaDeLotes
            If FilaLotes.Indice = Indice Then
                CantidadDevuelto = CantidadDevuelto + FilaLotes.Devolucion
            End If
        Next

        If CantidadDevuelto <> Cantidad Then Return False

        Return True

    End Function
    Public Function ImpresionPedido(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal RowCabeza As DataRow, ByVal DTDetalle As DataTable, ByRef mRow As Integer, ByRef newpage As Boolean, ByRef CantidadTotal As Decimal)

        Dim PrintFontTitulo As System.Drawing.Font
        Dim PrintFontTituloCeldas As System.Drawing.Font
        Dim PrintFontCeldas As System.Drawing.Font
        Dim y As Single = e.MarginBounds.Top - 15
        Dim RectanguloTitulo1 As New RectangleF(0.0F, 0.0F, e.PageBounds.Width, 100.0F)
        Dim RectanguloTitulo2 As New RectangleF(0.0F, 25.0F, e.PageBounds.Width, 120.0F)
        Dim RectanguloTitulo3 As New RectangleF(0.0F, 50.0F, e.PageBounds.Width, 140.0F)
        Dim Formato As StringFormat = New StringFormat(StringFormatFlags.LineLimit)
        Dim FormatoLineasTitulos As New StringFormat
        Dim WidthCelda As Integer = 0
        Dim ValorCelda As String = ""
        Dim TotalUnidadDeMedida As Decimal = 0

        Dim Sucursal As String = ""

        Formato.LineAlignment = StringAlignment.Center
        Formato.Trimming = StringTrimming.EllipsisCharacter
        Formato.Alignment = StringAlignment.Center

        FormatoLineasTitulos.Alignment = StringAlignment.Center

        PrintFontTitulo = New Font("Arial", 14)
        PrintFontTituloCeldas = New Font("Arial", 12)
        PrintFontCeldas = New Font("", 9)

        Sucursal = NombreSucursalCliente(RowCabeza.Item("Cliente"), RowCabeza.Item("Sucursal"))
        If Sucursal = "0" Then Sucursal = ""

        e.Graphics.DrawString("Pedido del Cliente: " & NombreCliente(RowCabeza.Item("Cliente")), PrintFontTitulo, Brushes.Black, RectanguloTitulo1, FormatoLineasTitulos)
        e.Graphics.DrawString("Sucursal: " & Sucursal, PrintFontTitulo, Brushes.Black, RectanguloTitulo2, FormatoLineasTitulos)
        e.Graphics.DrawString("Fecha Entrega:    Desde: " & Format(RowCabeza.Item("FechaEntregaDesde"), "dd/MM/yyyy") & "   Hasta: " & Format(RowCabeza.Item("FechaEntregaHasta"), "dd/MM/yyyy"), PrintFontTitulo, Brushes.Black, RectanguloTitulo3, FormatoLineasTitulos)

        Do While mRow < DTDetalle.Rows.Count + 1
            Dim x As Single = e.MarginBounds.Left + 20
            Dim h As Single = 0
            For Each columna As DataColumn In DTDetalle.Columns
                If Not columna.ColumnName = "Articulo" And Not columna.ColumnName = "Cantidad" Then Continue For
                Select Case columna.ColumnName
                    Case "Articulo"
                        WidthCelda = 370
                    Case "Cantidad"
                        WidthCelda = 210
                    Case Else
                        Continue For
                End Select

                Dim rc As RectangleF = New RectangleF(x, y, WidthCelda, 22)
                e.Graphics.DrawRectangle(Pens.Black, rc.Left, rc.Top, rc.Width, rc.Height)
                If (newpage) Then
                    Formato.Alignment = StringAlignment.Center
                    e.Graphics.DrawString(columna.ColumnName, PrintFontTituloCeldas, Brushes.Black, rc, Formato)
                Else
                    Formato.Alignment = StringAlignment.Near
                    ValorCelda = DTDetalle.Rows(mRow - 1).Item(columna.ColumnName)
                    If columna.ColumnName = "Cantidad" Then Formato.Alignment = StringAlignment.Far : CantidadTotal = CantidadTotal + DTDetalle.Rows(mRow - 1).Item("Cantidad") : TotalUnidadDeMedida = TotalUnidadDeMedida + DTDetalle.Rows(mRow - 1).Item("Cantidad") * HallaKilosXUnidad(DTDetalle.Rows(mRow - 1).Item("Articulo"))
                    If columna.ColumnName = "Articulo" Then ValorCelda = NombreArticulo(DTDetalle.Rows(mRow - 1).Item(columna.ColumnName))
                    e.Graphics.DrawString(ValorCelda, PrintFontCeldas, Brushes.Black, rc, Formato)
                End If
                x += rc.Width
                h = Math.Max(h, rc.Height)
            Next
            If mRow = DTDetalle.Rows.Count Then
                y += h
                x = e.MarginBounds.Left + 20
                Dim rc As RectangleF = New RectangleF(x, y, 370, 42)
                e.Graphics.DrawString(" Cantidad Total: " & FormatNumber(CantidadTotal, 2), PrintFontCeldas, Brushes.Black, rc, Formato)
                y += h
                x = e.MarginBounds.Left + 20
                rc = New RectangleF(x, y, 370, 42)
                e.Graphics.DrawString(" Total Unidad de Medida: " & FormatNumber(TotalUnidadDeMedida, 2), PrintFontCeldas, Brushes.Black, rc, Formato)
                Exit Function
            End If
            newpage = False
            y += h
            mRow += 1
            If y + h > e.MarginBounds.Bottom Then
                e.HasMorePages = True
                mRow -= 1
                newpage = True
                Exit Function
            End If
        Loop
        mRow = 0

    End Function

End Module
