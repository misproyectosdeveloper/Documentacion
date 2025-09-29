Imports System.IO
Public Class UnTXTPerRetPercibida
    Public PDtGrid As DataTable
    Public PPercepcion As Boolean
    Public PCodigo As Integer
    Public PDesde As Date
    Public PHasta As Date
    '
    Dim Directorio As String
    Dim DtGrid As New DataTable
    Dim Total As Decimal
    Private Sub UnTXTPerRetPercibida_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        DtGrid = PDtGrid.Copy
        Descartalineas()

        LlenaCombosGrid()

        TextDesde.Text = Format(PDesde, "dd/MM/yyyy")
        TextHasta.Text = Format(PHasta, "dd/MM/yyyy")

        If PPercepcion Then
            RadioPercepcion.Checked = True
            RadioPercepcion.BackColor = Color.LightBlue
            RadioRetencion.Checked = False
        Else
            RadioRetencion.Checked = True
            RadioRetencion.BackColor = Color.LightBlue
            RadioPercepcion.Checked = False
        End If

        If Not ValidaComprobante() Then UnTXTPerRetPercibida_FormClosing(Nothing, Nothing)


    End Sub
    Private Sub UnTXTPerRetPercibida_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonProcesar.Click

        If ComboTipo.SelectedValue = 0 Then
            MsgBox("Debe Seleccionar Percepción a Prcesar")
            Exit Sub
        End If

        Total = 0
        Dim ErrorW As String

        Directorio = ""
        Directorio = PideCarpeta()
        If Directorio = "" Then Exit Sub

        If PPercepcion Then
            Directorio = Directorio & "\" & "Per-IIBB" & "-" & Format(PDesde, "dd-MM-yyyy") & " Al " & Format(PHasta, "dd-MM-yyyy") & ".txt"
        Else
            Directorio = Directorio & "\" & "Ret-IIBB" & "-" & Format(PDesde, "dd-MM-yyyy") & " Al " & Format(PHasta, "dd-MM-yyyy") & ".txt"
        End If

        Using writer As StreamWriter = New StreamWriter(Directorio)                        'Borra Archivo.
        End Using

        For I As Integer = 0 To DtGrid.Rows.Count - 1
            Dim Row As DataRow = DtGrid.Rows(I)
            If Row("Tipo") = ComboTipo.SelectedValue And Row("Estado") <> 3 Then
                ImprimeRow(Row, ErrorW)
                If ErrorW <> "" Then
                    LabelArchivo.Text = ErrorW : LabelArchivo.Visible = True : Exit Sub
                End If
            End If
        Next

        LabelImporte.Text = "Importe Procesado:  " & Format(Total, "0.00") : LabelImporte.Visible = True
        LabelArchivo.Text = "Archivo Generado    " & Directorio : LabelArchivo.Visible = True

    End Sub
    Private Sub ImprimeRow(ByVal Row As DataRow, ByRef ErrorW As String)

        Dim Str As String
        ErrorW = ""

        If IsDBNull(Row("Provincia")) Then ErrorW = "Error: Falta Provincia en Comprobante " & Row("Comprobante") : Exit Sub

        Dim Jurisdiccion As String
        If Not IsDBNull(Row("Provincia")) Then
            Jurisdiccion = Format(HallaJurisdiccionProvincia(Row("Provincia")), "000")
            If Jurisdiccion = "000" Then
                ErrorW = "Error: Falta informar Jurisdicción Provincia " & HallaNombreProvincia(Row("Provincia"))
                Exit Sub
            End If
        End If
        If Jurisdiccion <> ComboJurisdiccion.SelectedValue And ComboJurisdiccion.SelectedValue <> 0 Then Exit Sub

        Dim Cuit As String = Format(Row("Cuit"), "00-00000000-0")
        Dim Fecha As String = Format(Row("Fecha"), "dd/MM/yyyy")
        Dim TipoComprobante As String = HallaTipo(Row("TipoComprobante"))
        Dim Sucursal As String
        Dim Constancia As String
        Dim Letra As String
        Dim NumeroComprobanteOriginal As String
        If PPercepcion Then       'Es Percepción.
            Letra = Strings.Left(Row("Comprobante"), 1)
            Sucursal = Strings.Mid(Row("Comprobante"), 2, 4)
            Constancia = Strings.Mid(Row("Comprobante"), 7, 8)
        End If
        If Not PPercepcion Then   'Es Retención.
            Select Case Row("TipoComprobante")
                Case 60, 604
                    Letra = HallaLetra(Row("TipoComprobante"), TraeNumero(Row("Comprobante")))
                    Sucursal = Strings.Mid(Row("Comprobante"), 2, 4)  'Trae comprobante ya editado del listado y al editar pone un espacio adelante porque no tiene Letra IVA.
                    Constancia = String.Format("{0:0000000000000000}", Val(Strings.Right(Row("Comprobante"), 8)))
                    NumeroComprobanteOriginal = String.Format("{0:00000000000000000000}", Row("Retencion"))
                Case 50, 70, 500, 700, 1010, 5000, 800, 3000, 10000
                    Letra = Strings.Mid(Row("Comprobante"), 1, 1)
                    Sucursal = Strings.Mid(Row("Comprobante"), 2, 4)
                    Constancia = String.Format("{0:0000000000000000}", Val(Strings.Right(Row("Comprobante"), 8)))
                    NumeroComprobanteOriginal = String.Format("{0:00000000000000000000}", Val(Strings.Right(Row("Retencion").ToString, 12)))
                Case Else
                    ErrorW = "Tipo Comprobante " & Row("TipoComprobante") & " No Previsto. Avisar a Sistema." : Exit Sub
            End Select
        End If

        Dim ImporteStr As String = Format(Row("Importe"), "00000000.00")
        ImporteStr = CovierteImporte(Row("Importe"))

        Total = Total + Row("Importe")

        If PPercepcion Then
            Str = Jurisdiccion & Cuit & Fecha & Sucursal & Constancia & TipoComprobante & Letra & ImporteStr
        Else
            Str = Jurisdiccion & Cuit & Fecha & Sucursal & Constancia & TipoComprobante & Letra & NumeroComprobanteOriginal & ImporteStr
        End If

        Try
            Using writer As StreamWriter = New StreamWriter(Directorio, True)
                writer.WriteLine(Str)
                writer.Close()
            End Using
        Catch ex As Exception
            ErrorW = "Error al Grabar Archivo." & ex.Message : Exit Sub
        End Try

    End Sub
    Private Sub LlenaCombosGrid()

        Dim SqlStr As String
        If PPercepcion Then
            SqlStr = " AND CodigoRetencion = 2;"
        Else
            SqlStr = " AND CodigoRetencion = 1;"
        End If

        Dim DtTipo As New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 25 AND TipoPago = 1" & SqlStr, Conexion, DtTipo) Then Me.Close() : Exit Sub
        Dim Row As DataRow = DtTipo.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        DtTipo.Rows.Add(Row)
        ComboTipo.DataSource = DtTipo
        ComboTipo.DisplayMember = "Nombre"
        ComboTipo.ValueMember = "Clave"
        ComboTipo.SelectedItem = 0
        With ComboTipo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        Dim DtProvincias As New DataTable
        If Not Tablas.Read("SELECT Operador as Clave,Nombre FROM Tablas WHERE Operador <> 0 AND Tipo = 31;", Conexion, DtProvincias) Then Me.Close() : Exit Sub
        Row = DtProvincias.NewRow
        Row("Clave") = 0
        Row("Nombre") = "Todas"
        DtProvincias.Rows.Add(Row)
        ComboJurisdiccion.DataSource = DtProvincias
        ComboJurisdiccion.DisplayMember = "Nombre"
        ComboJurisdiccion.ValueMember = "Clave"
        ComboJurisdiccion.SelectedValue = 0
        With ComboJurisdiccion
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Private Function CovierteImporte(ByVal Importe As Decimal) As String

        If Importe < 0 Then
            Return "-" & Format(-Importe, "0000000.00")
        Else
            Return Format(Importe, "00000000.00")
        End If

    End Function
    Private Function HallaJurisdiccionProvincia(ByVal Provincia As Integer) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Operador FROM Tablas WHERE Tipo = 31 AND Clave = " & Provincia & ";", Miconexion)
                    Return Cmd.ExecuteScalar
                End Using
                Miconexion.Close()
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: Provincias." & ex.Message, MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function HallaTipo(ByVal TipoComprobante As Integer) As String

        Select Case TipoComprobante
            Case 100
                ' Liquidación a Prov.
                Return "O"
            Case 600
                'Orden de Pago
                Return "R"
            Case 60
                'Cobranza
                Return "R"
            Case 604
                'Devolucion del Proveedor
                Return "R"
            Case 5000
                'Factura Proveedor
                Return "F"
            Case 3000
                ' Gasto Bancario
                Return "O"
            Case 5
                'N.D. a Cliente
                Return "D"
            Case 7
                'N.C. a Cliente
                Return "C"
            Case 6
                ' N.D. a Proveedor
                Return "D"
            Case 8
                'N.C. a Proveedor
                Return "C"
            Case 50
                'N.D. del Cliente
                Return "D"
            Case 70
                'N.C. del Cliente
                Return "C"
            Case 800
                'N.V.L.P.
                Return "O"
            Case 500
                'N.D. del Proveedor
                Return "D"
            Case 700
                'N.C. del Proveedor
                Return "C"
            Case 1010
                'Cancelación Prestamo
                Return "O"
            Case 10000
                'Otras Facturas
                Return "O"
            Case Else
                MsgBox("No Existe Código comprobante " & TipoComprobante) : Return ""
        End Select

    End Function
    Private Sub Descartalineas() 'Saca del Dtgrid los renglones de Totales.

        For I As Integer = DtGrid.Rows.Count - 1 To 0 Step -1
            Dim Row As DataRow = DtGrid.Rows(I)
            If Not IsDBNull(Row("Estilo")) Then  'Es un renglon con totales y lo borro.
                If Row("Estilo") = 1 Then Row.Delete()
            End If
        Next
        DtGrid.AcceptChanges()

    End Sub
    Private Function HallaLetra(ByVal TipoComprobante As Integer, ByVal Comprobante As Decimal) As String

        Dim Emisor As Integer
        Dim LetraW As String

        Select Case TipoComprobante
            Case 60
                Emisor = HallaEmisor(TipoComprobante, Comprobante)
                LetraW = LetraTipoIva(LetrasPermitidasCliente(HallaTipoIvaCliente(Emisor), 50))
            Case 604
                Emisor = HallaEmisor(TipoComprobante, Comprobante)
                LetraW = LetraTipoIva(LetrasPermitidasProveedor(HallaTipoIvaProveedor(Emisor), 500))
        End Select

        Return LetraW

    End Function
    Private Function HallaEmisor(ByVal TipoComprobante As Integer, ByVal Comprobante As Decimal) As Integer

        Dim Dt As New DataTable
        Dim Emisor As Integer

        If Not Tablas.Read("SELECT Emisor FROM RecibosCabeza WHERE TipoNota = " & TipoComprobante & " AND Nota = " & Comprobante & ";", Conexion, Dt) Then Return -1000

        If Dt.Rows.Count = 0 Then Dt.Dispose() : Return -1000
        Emisor = Dt.Rows(0).Item("Emisor")
        Dt.Dispose()
        Return Emisor

    End Function
    Private Function ValidaComprobante() As Boolean

        For I As Integer = 0 To DtGrid.Rows.Count - 1
            Dim Row As DataRow = DtGrid.Rows(I)
            If Row("Comprobante") = "00000000" Then
                If MsgBox("Existen Comprobantes con numero = 0. Si Continua producirá Error en AFIP." + vbCrLf + _
                          "Si es Factura Otro Proveedores, Gastos Bancarios o Cancelación de Prestamos, debe Informar Recibo-Oficial. Quiere continuar Igualmente? ", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Return False
                End If
            End If
        Next

        Return True

    End Function
End Class