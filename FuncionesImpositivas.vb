Module FuncionesImpositivas
    Public Function AgregaPercepciones(ByRef ListaDePercepciones As List(Of ItemIvaReten), ByVal TipoComprobante As Integer, ByVal Cliente As Integer, ByVal Cuit As String, ByVal TipoEmisor As Integer, ByVal Fecha As Date) As Boolean

        ListaDePercepciones = New List(Of ItemIvaReten)


        ListaDePercepciones = HallaRetencionesAplicables(TipoComprobante, TipoEmisor, Cliente, Fecha)
        For I As Integer = ListaDePercepciones.Count - 1 To 0 Step -1
            Dim Fila As ItemIvaReten = ListaDePercepciones(I)
            If Fila.Formula = 4 Then
                Dim Alicuota As Decimal = LeerPadron(Fila.Clave, Fecha, CuitNumerico(Cuit))
                If Alicuota = -100 Then
                    If MsgBox("Error al Leer Archivo de AFIP para Percep/Retenc. o Archivo no Existe." + vbCrLf + "Si continua no se realizara la Percep/Retenc." + vbCrLf + "Desean Continuar de todos modos?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                        Alicuota = 0
                    Else
                        Exit Function
                    End If
                End If
                Fila.Alicuota = Alicuota
                If Alicuota = 0 Then ListaDePercepciones.RemoveAt(I)
            End If
        Next

        Return True

    End Function
    Public Function ArmaDtRetencionesExentas(ByVal Tipo As Integer, ByRef DtExentas As DataTable) As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Clave FROM Tablas WHERE Tipo = 25 AND Activo2 = 1;", Conexion, Dt) Then Return False
        For Each Row As DataRow In Dt.Rows
            Dim Row1 As DataRow = DtExentas.NewRow
            Row1("Clave") = Row("Clave")
            Row1("TipoEmisor") = Tipo
            Row1("Emisor") = 0
            DtExentas.Rows.Add(Row1)
        Next

        Dt.Dispose()
        Return True

    End Function
    Public Function HallaRetencionesAplicables(ByVal TipoComprobante As Integer, ByVal TipoEmisor As Integer, ByVal Emisor As Integer, ByVal FechaComprobante As Date) As List(Of ItemIvaReten)

        Dim Dt As New DataTable

        HallaRetencionesAplicables = New List(Of ItemIvaReten)

        ArmaRetencionesATercerosAplicables(TipoComprobante, TipoEmisor, Emisor, Dt, FechaComprobante, 0)
        For Each Row As DataRow In Dt.Rows
            'En tablas con tipo=25 (Retenciones/percepciones) en iva esta fecha de vigencia.
            Dim Fila As New ItemIvaReten
            Fila.Clave = Row("Clave")
            Fila.Formula = Row("Formula")
            Fila.Nombre = Row("Nombre")
            If Row("Formula") = 1 Then
                Fila.Alicuota = Row("AlicuotaRetencion")
                Fila.Base = Row("TopeMes")
            End If
            Fila.CodigoAfip = Row("OrigenPercepcion")
            Fila.CodigoAfipElectronico = Row("CodigoAfipElectronico")
            HallaRetencionesAplicables.Add(Fila)
        Next

        Dt.Dispose()

    End Function
    Public Sub ArmaRetencionesATercerosAplicables(ByVal TipoComprobante As Integer, ByVal TipoEmisor As Integer, ByVal Emisor As Integer, ByRef DtRetencionesAutomaticas As DataTable, ByVal Fecha As Date, ByVal Comprobante As Decimal)

        Dim Borrar As Boolean = False
        DtRetencionesAutomaticas = New DataTable

        If Not Tablas.Read("SELECT T.Clave,T.OrigenPercepcion,T.CodigoAfipElectronico,T.Nombre,T.CodigoRetencion,T.Formula,T.TopeMes,T.AlicuotaRetencion,T.TipoIva,0.0 AS Importe,Iva FROM Tablas AS T INNER JOIN DocuRetenciones AS D ON T.Clave = D.Clave WHERE T.Tipo = 25 AND D.TipoDocumento = " & TipoComprobante & ";", Conexion, DtRetencionesAutomaticas) Then End
        'En tablas con tipo=25 (Retenciones/percepciones) en iva esta fecha de vigencia.
        For Each Row As DataRow In DtRetencionesAutomaticas.Rows
            Borrar = False
            Dim Exenta As Integer = EsExenta(Row("Clave"), TipoEmisor, Emisor)
            If Exenta < 0 Then
                MsgBox("Error Base de Datos al Leer Retenciones Exentas.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End If
            If Exenta And Comprobante = 0 <> 0 Then
                Borrar = True
            End If
            If Not Borrar Then
                Dim TipoContribuyente As Integer = HallaTipoContribuyente(Emisor, TipoEmisor)
                If TipoContribuyente < 0 Then
                    MsgBox("Error al leer Tabla: Clientes o Proveedores.", MsgBoxStyle.Critical) : End
                End If
                If TipoContribuyente = 6 And Row("Formula") <> 4 And Comprobante = 0 Then
                    Borrar = True
                Else
                    '''  If TipoContribuyente = 2 And Comprobante = 0 Then Borrar = True
                End If
            End If
            If Not Borrar Then
                If Not HallaVigenciaRetencion(Row("Nombre"), Row("Iva"), Fecha) Then Borrar = True
            End If
            If Borrar Then Row.Delete()
        Next
        DtRetencionesAutomaticas.AcceptChanges()

    End Sub
    Public Function CalculaRetenFormulaLiquidacionProv1(ByVal Proveedor As Integer, ByVal Bruto As Decimal, ByVal AlicuotaIva As Decimal, ByRef ListaReten As List(Of ItemIvaReten), ByVal RetencionManual As Boolean, ByVal Fecha As Date) As Decimal

        Dim Retencion As Decimal = 0
        Dim RetencionTotal As Decimal = 0

        For Each Fila As ItemIvaReten In ListaReten
            If Fila.Formula = 1 And Not RetencionManual Then
                Retencion = CalculaRetencionFormulaLiquidacionProv1(Fila.Nombre, Proveedor, Month(Fecha), Year(Fecha), Bruto, AlicuotaIva, Fila.Clave, Fila.Base, Fila.Alicuota)
                If Retencion > 0 Then
                    Fila.Importe = Retencion
                    RetencionTotal = RetencionTotal + Retencion
                End If
            End If
        Next

        Return RetencionTotal

    End Function
    Public Function HallaFormulaRetencion(ByVal Retencion As Integer) As Integer

        Dim DT As New DataTable
        Dim Formula As Integer

        If Not Tablas.Read("SELECT Formula FROM Tablas Where Tipo = 25 and Clave = " & Retencion & ";", Conexion, DT) Then End
        If DT.Rows.Count <> 0 Then
            Formula = DT.Rows(0).Item("Formula")
        End If

        DT.Dispose()
        Return Formula

    End Function
    Public Function HallaNombreRetencion(ByVal Retencion As Integer) As String

        Dim DT As New DataTable

        If Not Tablas.Read("SELECT Nombre FROM Tablas Where Tipo = 25 and Clave = " & Retencion & ";", Conexion, DT) Then End
        If DT.Rows.Count <> 0 Then
            Return DT.Rows(0).Item("Nombre")
        End If

        DT.Dispose()

    End Function
    Public Function HallaVigenciaRetencion(ByVal Nombre As String, ByVal Iva As Decimal, ByVal Fecha As Date) As Boolean

        If Iva = 0 Then
            MsgBox("Retenc./Precep. " & Nombre & " No Tiene Vigencia. NO se puede Informar.", MsgBoxStyle.Information)
            Return False
        End If

        Dim FechaStr As String = Iva.ToString   'Vigencia.
        If Iva <> 0 Then
            If Format(Fecha, "yyyyMMdd") < Strings.Left(FechaStr, 4) & Strings.Mid(FechaStr, 5, 2) & Strings.Right(FechaStr, 2) Then
                Return False
            Else
                Return True
            End If
        End If

    End Function
    Public Function CalculaRetencionFormula1(ByVal Nombre As String, ByVal Emisor As Integer, ByVal Mes As Integer, ByVal Anio As Integer, ByVal ImporteOrden As Decimal, ByVal IvaOrden As Decimal, ByVal ClaveRetencion As Integer, ByVal TopeMes As Decimal, ByVal AlicuotaRetencion As Decimal) As Decimal

        Dim ImporteBruto As Decimal = 0
        Dim Row As DataRow
        Dim Sql As String = "SELECT Importe,CodigoIva FROM RecibosCabeza WHERE TipoNota = 600 AND Estado <> 3 AND Emisor = " & Emisor & " AND YEAR(Fecha) = " & Anio & " AND MONTH(Fecha) = " & Mes & ";"

        Dim Dt As New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        For Each Row In Dt.Rows
            ImporteBruto = ImporteBruto + Row("Importe") / (1 + HallaIvaDeCodigo(Row("CodigoIva")) / 100)
        Next
        ImporteBruto = ImporteBruto + ImporteOrden / (1 + HallaIvaDeCodigo(IvaOrden) / 100)

        Sql = "SELECT P.Importe AS Importe FROM RecibosCabeza AS C INNER JOIN RecibosDetallePago AS P ON C.TipoNota = P.TipoNota AND C.Nota = P.Nota " & _
              "WHERE C.TipoNota = 600 AND C.Estado <> 3 AND C.Emisor = " & Emisor & " AND YEAR(C.Fecha) = " & Anio & " AND MONTH(C.Fecha) = " & Mes & " AND P.Mediopago = " & ClaveRetencion & ";"
        Dt = New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        Dim RetencionPagada As Decimal = 0
        For Each Row In Dt.Rows
            RetencionPagada = RetencionPagada + Row("Importe")
        Next
        Dt.Dispose()

        Dim Retencion As Decimal = 0

        If (ImporteBruto - TopeMes) > 0 Then
            Retencion = Trunca((ImporteBruto - TopeMes) * AlicuotaRetencion / 100 - RetencionPagada)
            If Retencion > ImporteOrden Then
                MsgBox("La Retencion/Percepcion: " & Nombre & " Supera Importe de la Orden de Pago." + vbCrLf + _
                       "Puede ser que halla sido definida con Ordenes Pagos realizadas anteriormente." + vbCrLf + _
                       "Puede Informarla manualmente.", MsgBoxStyle.Information)
                Retencion = 0
            End If
            If Retencion < 0 Then
                MsgBox("La Retencion/Percepcion: " & Nombre & " es negativa." + vbCrLf + _
                       "Puede ser que halla sido informada manualmente con importe superior al correspondiente." + vbCrLf + _
                       "Puede Informarla manualmente.", MsgBoxStyle.Information)
                Retencion = 0
            End If
        End If

        Return Retencion

    End Function
    Public Function CalculaRetencionFormulaLiquidacionProv1(ByVal Nombre As String, ByVal Emisor As Integer, ByVal Mes As Integer, ByVal Anio As Integer, ByVal ImporteLiquidacion As Decimal, ByVal IvaLiquidacion As Decimal, ByVal ClaveRetencion As Integer, ByVal TopeMes As Decimal, ByVal AlicuotaRetencion As Decimal) As Decimal

        Dim ImporteBruto As Decimal = 0
        Dim Row As DataRow
        Dim Sql As String = "SELECT Bruto AS Importe,Alicuota FROM LiquidacionCabeza WHERE Estado <> 3 AND Proveedor = " & Emisor & " AND YEAR(Fecha) = " & Anio & " AND MONTH(Fecha) = " & Mes & ";"

        Dim Dt As New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        For Each Row In Dt.Rows
            ImporteBruto = ImporteBruto + Row("Importe")
        Next
        ImporteBruto = ImporteBruto + ImporteLiquidacion

        Sql = "SELECT P.Importe AS Importe FROM LiquidacionCabeza AS C INNER JOIN LiquidacionDetalleConceptos AS P ON C.Liquidacion = P.Liquidacion " & _
              "WHERE C.Estado <> 3 AND C.Proveedor = " & Emisor & " AND YEAR(C.Fecha) = " & Anio & " AND MONTH(C.Fecha) = " & Mes & " AND P.Concepto = " & ClaveRetencion & ";"
        Dt = New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then End
        Dim RetencionPagada As Decimal = 0
        For Each Row In Dt.Rows
            RetencionPagada = RetencionPagada + Row("Importe")
        Next
        Dt.Dispose()

        Dim Retencion As Decimal = 0

        If (ImporteBruto - TopeMes) > 0 Then
            Retencion = Trunca((ImporteBruto - TopeMes) * AlicuotaRetencion / 100 - RetencionPagada)
            If Retencion > ImporteLiquidacion Then
                MsgBox("La Retencion/Percepcion: " & Nombre & " Supera Importe de la Liquidación." + vbCrLf + _
                       "Puede ser que halla sido definida con Liquidaciones realizadas anteriormente." + vbCrLf + _
                       "", MsgBoxStyle.Information)
                Retencion = 0
            End If
            If Retencion < 0 Then
                MsgBox("La Retencion/Percepcion: " & Nombre & " es negativa." + vbCrLf + _
                       "Puede ser que halla sido informada manualmente con importe superior al correspondiente." + vbCrLf + _
                       "", MsgBoxStyle.Information)
                Retencion = 0
            End If

        End If

        Return Retencion

    End Function
    Public Function CalculaRetencionFormula4(ByVal ClaveRetencion As Integer, ByVal Emisor As Integer, ByVal Cuit As String, ByVal Fecha As Date, ByVal ImporteOrden As Decimal, ByVal IvaOrden As Decimal) As Decimal

        Dim ImporteBruto As Decimal = 0
        Dim Coeficiente As Decimal = 1 + HallaIvaDeCodigo(IvaOrden) / 100

        Dim Alicuota As Decimal = LeerPadron(ClaveRetencion, Fecha, CuitNumerico(Cuit))
        If Alicuota = -100 Then Return Alicuota

        ImporteBruto = ImporteOrden / Coeficiente

        Dim Retencion As Decimal = ImporteBruto * Alicuota / 100

        Return Trunca(Retencion)

    End Function
    Public Function EsExenta(ByVal Retencion As Integer, ByVal TipoEmisor As Integer, ByVal Emisor As Integer) As Integer

        Dim Sql As String = "SELECT COUNT(Clave) FROM RetencionesExentas WHERE Clave = " & Retencion & " AND TipoEmisor = " & TipoEmisor & " AND Emisor = " & Emisor & ";"
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
    Public Function HallaAlicuotaRetIngBruto(ByVal Provincia As Integer) As Double

        Dim Sql As String = "SELECT AlicuotaRetIngBruto FROM Tablas WHERE Tipo = 31 AND Clave = " & Provincia & ";"
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
    Public Function CalculaPercepciones(ByRef ListaDePercepciones As List(Of ItemIvaReten), ByVal Neto As Decimal) As Decimal

        Dim Total As Decimal = 0

        For Each Fila As ItemIvaReten In ListaDePercepciones
            Fila.Importe = 0
            If Fila.Formula = 4 Then
                Fila.Importe = CalculaIva(1, Neto, Fila.Alicuota)
                Total = Total + Fila.Importe
            End If
        Next

        Return Total

    End Function
    Public Sub ArmaRecibosPercepciones(ByVal TipoComprobante As Integer, ByVal Comprobante As Decimal, ByVal ListaDePercepciones As List(Of ItemIvaReten), ByRef Dt As DataTable)


        For Each Fila As ItemIvaReten In ListaDePercepciones
            If Fila.Importe <> 0 Then
                Dim RowP As DataRow = Dt.NewRow
                RowP("TipoComprobante") = TipoComprobante
                RowP("Comprobante") = Comprobante
                RowP("Percepcion") = Fila.Clave
                RowP("Base") = Fila.Base
                RowP("Formula") = Fila.Formula
                RowP("CodigoAfip") = Fila.CodigoAfipElectronico
                RowP("Alicuota") = Fila.Alicuota
                RowP("Importe") = Fila.Importe
                Dt.Rows.Add(RowP)
            End If
        Next

    End Sub
    Public Function HallaTipoContribuyente(ByVal Emisor As Integer, ByVal TipoEmisor As Integer) As Integer
        'no uso HallaTipoIva() pues si es monotributo(6) lo transforma en 2. si se arregla usar esta rutina. 

        Dim TipoContribuyente As Integer
        Dim Sql As String

        If TipoEmisor = 2 Then
            Sql = "SELECT TipoIva FROM Clientes WHERE Clave = " & Emisor & ";"
        Else
            Sql = "SELECT TipoIva FROM Proveedores WHERE Clave = " & Emisor & ";"
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    TipoContribuyente = Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

        Return TipoContribuyente

    End Function
End Module
