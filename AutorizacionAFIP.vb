Imports System.Xml
Imports System.IO
Imports ClassPassWord
Module AutorizacionAFIP
    Public Class ItemIva
        Private IntClave As Integer
        Private DecIva As Decimal
        Private DecBase As Decimal
        Private DecImporte As Decimal
        Public Sub New()
        End Sub
        Public Property Clave() As Integer
            Get
                Return IntClave
            End Get
            Set(ByVal value As Integer)
                IntClave = value
            End Set
        End Property
        Public Property Iva() As Decimal
            Get
                Return DecIva
            End Get
            Set(ByVal value As Decimal)
                DecIva = value
            End Set
        End Property
        Public Property Base() As Decimal
            Get
                Return DecBase
            End Get
            Set(ByVal value As Decimal)
                DecBase = value
            End Set
        End Property
        Public Property Importe() As Decimal
            Get
                Return DecImporte
            End Get
            Set(ByVal value As Decimal)
                DecImporte = value
            End Set
        End Property
    End Class
    Public Class ItemDatosParaAFIP
        Private IntTipo As Integer
        Private IntPtoVta As Integer
        Private DecCbte As Decimal
        Private DecNroCbte As Decimal
        Private intTipoIva As Integer
        Private BolEsFCE As Boolean
        Private StrEsAnulacion As String
        Private DecCuit As Decimal
        Private StrAgenteDeposito As String
        Private DateFechaCbte As Date
        Private DateFechaPago As Date
        Private StrCBU As String
        Private intInscripcionAfip As Integer
        Private intDocTipo As Integer
        Private DecDocNro As Decimal
        Public Sub New()
        End Sub
        Public Property Tipo() As Integer
            Get
                Return IntTipo
            End Get
            Set(ByVal value As Integer)
                IntTipo = value
            End Set
        End Property
        Public Property PtoVta() As Integer
            Get
                Return IntPtoVta
            End Get
            Set(ByVal value As Integer)
                IntPtoVta = value
            End Set
        End Property
        Public Property Cbte() As Decimal
            Get
                Return DecCbte
            End Get
            Set(ByVal value As Decimal)
                DecCbte = value
            End Set
        End Property
        Public Property NroCbte() As Decimal
            Get
                Return DecNroCbte
            End Get
            Set(ByVal value As Decimal)
                DecNroCbte = value
            End Set
        End Property
        Public Property TipoIva() As Integer
            Get
                Return intTipoIva
            End Get
            Set(ByVal value As Integer)
                intTipoIva = value
            End Set
        End Property
        Public Property EsFCE() As Boolean
            Get
                Return BolEsFCE
            End Get
            Set(ByVal value As Boolean)
                BolEsFCE = value
            End Set
        End Property
        Public Property EsAnulacion() As String
            Get
                Return StrEsAnulacion
            End Get
            Set(ByVal value As String)
                StrEsAnulacion = value
            End Set
        End Property
        Public Property Cuit() As Decimal
            Get
                Return DecCuit
            End Get
            Set(ByVal value As Decimal)
                DecCuit = value
            End Set
        End Property
        Public Property FechaCbte() As Date
            Get
                Return DateFechaCbte
            End Get
            Set(ByVal value As Date)
                DateFechaCbte = value
            End Set
        End Property
        Public Property FechaPago() As Date
            Get
                Return DateFechaPago
            End Get
            Set(ByVal value As Date)
                DateFechaPago = value
            End Set
        End Property
        Public Property CBU() As String
            Get
                Return StrCBU
            End Get
            Set(ByVal value As String)
                StrCBU = value
            End Set
        End Property
        Public Property AgenteDeposito() As String
            Get
                Return StrAgenteDeposito
            End Get
            Set(ByVal value As String)
                StrAgenteDeposito = value
            End Set
        End Property
        Public Property InscripcionAfip() As Integer
            Get
                Return intInscripcionAfip
            End Get
            Set(ByVal value As Integer)
                intInscripcionAfip = value
            End Set
        End Property
        Public Property DocTipo() As Integer
            Get
                Return intDocTipo
            End Get
            Set(ByVal value As Integer)
                intDocTipo = value
            End Set
        End Property
        Public Property DocNro() As Decimal
            Get
                Return DecDocNro
            End Get
            Set(ByVal value As Decimal)
                DecDocNro = value
            End Set
        End Property
    End Class
    Public Function Autorizar(ByVal Tipo As String, ByVal DtCabeza As DataTable, ByVal DtDetalle As DataTable, ByVal DtPercepciones As DataTable, ByVal FchServDesde As Date, ByVal FchServHasta As Date, ByVal FchVtoPago As Date, ByVal TipoFactura As Integer, ByVal MaskedFactura As String, ByVal Cuit As String, ByVal Concepto As Integer, ByVal TipoArticulo As Integer, ByRef CAE As String, ByRef FechaCae As String, ByRef CbteTipoAsociadoW As Integer, ByRef CbteAsociadoW As Decimal, ByRef CancelarGrabar As Boolean, ByVal DatosParaAfip As ItemDatosParaAFIP) As String

        Dim CbteTipo As String = ""
        Dim PtoVta As Integer
        Dim CbteDesde As Decimal
        Dim CbteHasta As Decimal
        Dim CuitW = Mid$(Cuit, 1, 2) & Mid$(Cuit, 4, 8) & Mid$(Cuit, 13, 1)
        Dim CuitEmpresaW = Mid$(GCuitEmpresa, 1, 2) & Mid$(GCuitEmpresa, 4, 8) & Mid$(GCuitEmpresa, 13, 1)
        Dim Mensaje As String = ""

        CAE = ""
        FechaCae = ""
        ''  CbteTipoAsociadoW = 0
        ''  CbteAsociadoW = 0
        CancelarGrabar = False

        If Not DatosParaAfip.EsFCE Then
            Select Case Tipo
                Case "CD"
                    CbteTipo = HallaTipoSegunAfip(TipoFactura, DtCabeza.Rows(0).Item("TipoNota"))
                Case "F"
                    CbteTipo = HallaTipoSegunAfip(TipoFactura, 1)
                Case "C"
                    CbteTipo = HallaTipoSegunAfip(TipoFactura, 2)
            End Select
        Else
            Select Case Tipo
                Case "CD"
                    CbteTipo = HallaTipoSegunAfipFCE(TipoFactura, DtCabeza.Rows(0).Item("TipoNota"))
                Case "F"
                    CbteTipo = HallaTipoSegunAfipFCE(TipoFactura, 1)
                Case "C"
                    CbteTipo = HallaTipoSegunAfipFCE(TipoFactura, 2)
            End Select
        End If

        If Tipo = "F" Then                   'Factura.
            PtoVta = Strings.Left(MaskedFactura, 4)
            CbteDesde = Strings.Right(MaskedFactura, 8)
            CbteHasta = Strings.Right(MaskedFactura, 8)
        End If
        If Tipo = "CD" Or Tipo = "C" Then    'Credito/Debito, Credito con devolucion. 
            PtoVta = Mid$(MaskedFactura, 2, 4)
            CbteDesde = Strings.Right(MaskedFactura, 8)
            CbteHasta = Strings.Right(MaskedFactura, 8)
        End If

        'Halla Comprobante asociado.
        Dim PtoVtaAsociadoW As Integer = 0
        Dim CbteTipoAsociadoAfip As Integer
        Dim NroAsociadoW As Decimal = 0
        Dim TipoIvaAsociadoW As Integer = 0
        Dim PeriodoDesdeW As String
        Dim PeriodoHastaW As String
        If Tipo = "CD" Then
            If Not DatosParaAfip.EsFCE Then
                UnComprobanteAsociado.PCbteTipo = DtCabeza.Rows(0).Item("TipoNota")
                UnComprobanteAsociado.PTipoIva = TipoFactura
                If DatosParaAfip.InscripcionAfip = 3 Then UnComprobanteAsociado.PEsConsumidorFinal = True
                UnComprobanteAsociado.ShowDialog()
                If UnComprobanteAsociado.PCancelar Then CancelarGrabar = True : Return "Grabación  Cancelada."
                CbteTipoAsociadoW = UnComprobanteAsociado.PCbteTipoAsociado
                PtoVtaAsociadoW = UnComprobanteAsociado.PPtoVtaAsociado
                CbteAsociadoW = UnComprobanteAsociado.PCbteAsociado
                NroAsociadoW = UnComprobanteAsociado.PNroAsociado
                TipoIvaAsociadoW = UnComprobanteAsociado.PTipoIvaAsociado
                Concepto = UnComprobanteAsociado.PConcepto
                CbteTipoAsociadoAfip = HallaTipoSegunAfip(TipoIvaAsociadoW, CbteTipoAsociadoW)
                PeriodoDesdeW = UnComprobanteAsociado.PFechaPeriodoDesde
                PeriodoHastaW = UnComprobanteAsociado.PFechaPeriodoHasta
            Else
                CbteTipoAsociadoW = DatosParaAfip.Tipo  'Caso es FCE y DatosParaAfip trae datos de la factura. 
                PtoVtaAsociadoW = DatosParaAfip.PtoVta
                CbteAsociadoW = DatosParaAfip.Cbte
                NroAsociadoW = DatosParaAfip.NroCbte
                TipoIvaAsociadoW = DatosParaAfip.TipoIva
                CbteTipoAsociadoAfip = HallaTipoSegunAfipFCE(TipoIvaAsociadoW, CbteTipoAsociadoW)
            End If
        End If
        If Tipo = "C" Then
            CbteTipoAsociadoW = DatosParaAfip.Tipo
            PtoVtaAsociadoW = DatosParaAfip.PtoVta
            CbteAsociadoW = DatosParaAfip.Cbte
            NroAsociadoW = DatosParaAfip.NroCbte
            TipoIvaAsociadoW = DatosParaAfip.TipoIva
            If Not DatosParaAfip.EsFCE Then
                CbteTipoAsociadoAfip = HallaTipoSegunAfip(DatosParaAfip.TipoIva, DatosParaAfip.Tipo)
            Else
                CbteTipoAsociadoAfip = HallaTipoSegunAfipFCE(DatosParaAfip.TipoIva, DatosParaAfip.Tipo)
            End If
        End If

        'Halla totales por condicion Iva del Envio al WS..........................................................................
        Dim TotalExento As Decimal = 0
        Dim TotalNoGrabado As Decimal = 0
        Dim TotalBase As Decimal = 0
        Dim TotalIva As Decimal = 0
        Dim DetalleIva As XmlDocument = New XmlDocument()
        '
        DetalleIva.LoadXml("<Detalle></Detalle>")
        'En DetalleIva agrupa Base imponible por alicuota.
        If Tipo = "F" Then
            If DtCabeza.Rows(0).Item("Final") And EmpresaOk() Then
                Mensaje = ArmaXMLIvaPrecioFinal(TipoArticulo, DtDetalle, DetalleIva, TotalBase, TotalExento, TotalNoGrabado, TotalIva)
            Else
                Mensaje = ArmaXMLIva(TipoArticulo, DtDetalle, DetalleIva, TotalBase, TotalExento, TotalNoGrabado, TotalIva)
            End If
            TotalExento = TotalExento + DtCabeza.Rows(0).Item("Senia")
        End If
        If Tipo = "C" Then
            Mensaje = ArmaXMLIvaCredito(TipoArticulo, DtDetalle, DetalleIva, TotalBase, TotalExento, TotalNoGrabado, TotalIva)
        End If
        If Tipo = "CD" Then
            Mensaje = ArmaXMLIvaDebitoCredito(DtDetalle, DetalleIva, TotalBase, TotalExento, TotalNoGrabado, TotalIva)
        End If
        If Mensaje <> "" Then Return Mensaje

        'Define DetalleTributos para Envio al WS....................................................................
        Dim TotalTributo As Decimal = 0
        Dim DetalleTributos As XmlDocument = New XmlDocument()
        DetalleTributos.LoadXml("<Detalle></Detalle>")
        If DtPercepciones.Rows.Count <> 0 Then
            If Tipo = "F" Or Tipo = "C" Or Tipo = "CD" Then
                Mensaje = ArmaXMLTributos(DetalleTributos, DtPercepciones, TotalTributo)
                If Mensaje <> "" Then Return Mensaje
            End If
        End If

        'Corrige problemas de redondeo. El TotalIva no es igual al calculado en el comprobante.
        Dim ImporteTotal As Decimal = DtCabeza.Rows(0).Item("Importe")
        If Tipo = "F" Or Tipo = "C" Then
            ImporteTotal = ImporteTotal + TotalTributo   ' en facturas y N.C con devolucion importe y percepcion estan separados.
        End If

        Dim Diferencia As Decimal = ImporteTotal - (TotalExento + TotalNoGrabado + TotalBase + TotalIva + TotalTributo)
        If Diferencia > 0 Then TotalExento = TotalExento + Diferencia

        'Define Cabeza del Envio al WS...............................................................................
        Dim Cabeza As XmlDocument = New XmlDocument()
        Cabeza.LoadXml("<Cabeza></Cabeza>")
        '
        Dim NodoCabeza As XmlElement = Cabeza.CreateElement("CbteTipo")
        NodoCabeza.InnerText = CbteTipo
        Cabeza.DocumentElement.AppendChild(NodoCabeza)
        '
        NodoCabeza = Cabeza.CreateElement("PtoVta")
        NodoCabeza.InnerText = PtoVta
        Cabeza.DocumentElement.AppendChild(NodoCabeza)
        '
        NodoCabeza = Cabeza.CreateElement("CantReg")
        NodoCabeza.InnerText = "01"                                     ' Siempre 1.
        Cabeza.DocumentElement.AppendChild(NodoCabeza)

        'Define Detalle del Envio al WS...................................................................
        Dim Detalle As XmlDocument = New XmlDocument()
        Detalle.LoadXml("<Detalle></Detalle>")
        '
        NodoCabeza = Detalle.CreateElement("Concepto")
        NodoCabeza.InnerText = Concepto
        Detalle.DocumentElement.AppendChild(NodoCabeza)

        '-----  Prepara Tipo y Numero de documento.
        Dim DocTipo As Integer = 0
        Dim DocNro As Decimal = 0
        If DatosParaAfip.InscripcionAfip = 3 Then   'Consumidor Final.
            If DatosParaAfip.DocTipo <> 0 And DatosParaAfip.DocNro <> 0 Then
                DocTipo = DatosParaAfip.DocTipo : DocNro = DatosParaAfip.DocNro
            Else
                DocTipo = 99 : DocNro = 0
            End If
            If Tipo = "F" Then
                If ImporteTotal >= GLimiteParaConsumidorFinal Then
                    If DatosParaAfip.DocTipo = 0 Or DatosParaAfip.DocNro = 0 Then
                        If MsgBox("Importe Mayor a " & Format(GLimiteParaConsumidorFinal, "0.00") & " debe Informar Tipo Y Numero Documento del Cliente. Puede que tenga que actualizar el Limite para Consumidor Final en Datos de la Empresa." + vbCrLf + _
                                  "Puede ser Rechazado por AFIP. Quiere Enviar Comprobante a la AFIP de Todas Maneras?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return "NO AUTORIZADO."
                    End If
                End If
            End If
        Else
            DocTipo = 80 : DocNro = CuitW
        End If
        '----------------------------------------
        NodoCabeza = Detalle.CreateElement("DocTipo")
        NodoCabeza.InnerText = DocTipo
        Detalle.DocumentElement.AppendChild(NodoCabeza)
        '
        NodoCabeza = Detalle.CreateElement("DocNro")
        NodoCabeza.InnerText = DocNro
        Detalle.DocumentElement.AppendChild(NodoCabeza)
        '
        NodoCabeza = Detalle.CreateElement("CbteDesde")
        NodoCabeza.InnerText = CbteDesde
        Detalle.DocumentElement.AppendChild(NodoCabeza)
        '
        NodoCabeza = Detalle.CreateElement("CbteHasta")
        NodoCabeza.InnerText = CbteHasta
        Detalle.DocumentElement.AppendChild(NodoCabeza)
        '
        NodoCabeza = Detalle.CreateElement("CbteFch")
        NodoCabeza.InnerText = Format(DtCabeza.Rows(0).Item("FechaContable"), "yyyyMMdd")
        Detalle.DocumentElement.AppendChild(NodoCabeza)
        '
        NodoCabeza = Detalle.CreateElement("ImpTotal")
        NodoCabeza.InnerText = ImporteTotal
        Detalle.DocumentElement.AppendChild(NodoCabeza)
        '
        NodoCabeza = Detalle.CreateElement("ImpTotConc")
        NodoCabeza.InnerText = TotalNoGrabado
        Detalle.DocumentElement.AppendChild(NodoCabeza)
        '
        NodoCabeza = Detalle.CreateElement("ImpNeto")
        NodoCabeza.InnerText = TotalBase
        Detalle.DocumentElement.AppendChild(NodoCabeza)
        '
        NodoCabeza = Detalle.CreateElement("ImpOpEx")
        NodoCabeza.InnerText = TotalExento
        Detalle.DocumentElement.AppendChild(NodoCabeza)
        '
        NodoCabeza = Detalle.CreateElement("ImpTrib")
        NodoCabeza.InnerText = TotalTributo
        Detalle.DocumentElement.AppendChild(NodoCabeza)
        '
        NodoCabeza = Detalle.CreateElement("ImpIva")
        NodoCabeza.InnerText = TotalIva
        Detalle.DocumentElement.AppendChild(NodoCabeza)

        ' 'halla Condicion iva receptor segun tipo Iva. 

        NodoCabeza = Detalle.CreateElement("CondicionIvaReceptor")
        Dim Condicion As Integer = HallaCondicionIvaReceptor(DatosParaAfip.InscripcionAfip)
        NodoCabeza.InnerText = Condicion
        Detalle.DocumentElement.AppendChild(NodoCabeza)

        If PeriodoDesdeW <> "" Then
            Dim DetallePeriodo As XmlElement = Detalle.CreateElement("PeriodoAsoc")
            Detalle.DocumentElement.AppendChild(DetallePeriodo)
            '
            Dim DesdeNode As XmlElement = Detalle.CreateElement("FchDesde")
            DesdeNode.InnerText = PeriodoDesdeW
            DetallePeriodo.AppendChild(DesdeNode)
            '
            Dim HastaNode As XmlElement = Detalle.CreateElement("FchHasta")
            HastaNode.InnerText = PeriodoHastaW
            DetallePeriodo.AppendChild(HastaNode)
        End If
        '
        If Concepto = 2 Or Concepto = 3 Then
            NodoCabeza = Detalle.CreateElement("FchServDesde")
            NodoCabeza.InnerText = Format(FchServDesde, "yyyyMMdd")
            Detalle.DocumentElement.AppendChild(NodoCabeza)
            '
            NodoCabeza = Detalle.CreateElement("FchServHasta")
            NodoCabeza.InnerText = Format(FchServHasta, "yyyyMMdd")
            Detalle.DocumentElement.AppendChild(NodoCabeza)
            '
            If Not DatosParaAfip.EsFCE Then
                NodoCabeza = Detalle.CreateElement("FchVtoPago")
                NodoCabeza.InnerText = Format(FchVtoPago, "yyyyMMdd")
                Detalle.DocumentElement.AppendChild(NodoCabeza)
            End If
        End If
        '
        If DatosParaAfip.EsFCE And Tipo = "F" Then
            NodoCabeza = Detalle.CreateElement("FchVtoPago")
            NodoCabeza.InnerText = Format(DatosParaAfip.FechaPago, "yyyyMMdd")
            Detalle.DocumentElement.AppendChild(NodoCabeza)
        End If
        '
        NodoCabeza = Detalle.CreateElement("MonId")
        NodoCabeza.InnerText = "PES"
        Detalle.DocumentElement.AppendChild(NodoCabeza)
        '
        NodoCabeza = Detalle.CreateElement("MonCotiz")
        NodoCabeza.InnerText = 1                                     'Siempre 1.
        Detalle.DocumentElement.AppendChild(NodoCabeza)

        'Define DetalleComprobantesAsociados del Envio al  WS......................................................
        Dim DetalleAsociados As XmlDocument = New XmlDocument()
        DetalleAsociados.LoadXml("<Detalle></Detalle>")
        If CbteAsociadoW <> 0 Then
            Dim CbteAsocDetalleAsociados As XmlElement = DetalleAsociados.CreateElement("CbteAsoc")
            DetalleAsociados.DocumentElement.AppendChild(CbteAsocDetalleAsociados)
            '
            Dim IdNode As XmlNode = DetalleAsociados.CreateElement("Tipo")
            IdNode.AppendChild(DetalleAsociados.CreateTextNode(CbteTipoAsociadoAfip))
            CbteAsocDetalleAsociados.AppendChild(IdNode)
            ' 
            Dim AlicuotaNode As XmlNode = DetalleAsociados.CreateElement("PtoVta")
            AlicuotaNode.AppendChild(DetalleAsociados.CreateTextNode(PtoVtaAsociadoW))
            CbteAsocDetalleAsociados.AppendChild(AlicuotaNode)
            '
            Dim BaseNode As XmlNode = DetalleAsociados.CreateElement("Nro")
            BaseNode.AppendChild(DetalleAsociados.CreateTextNode(NroAsociadoW))
            CbteAsocDetalleAsociados.AppendChild(BaseNode)
            '
            If DatosParaAfip.EsFCE Then
                Dim CuitNode As XmlNode = DetalleAsociados.CreateElement("Cuit")
                CuitNode.AppendChild(DetalleAsociados.CreateTextNode(DatosParaAfip.Cuit))
                CbteAsocDetalleAsociados.AppendChild(CuitNode)
            Else
                Dim CuitNode As XmlNode = DetalleAsociados.CreateElement("Cuit")
                CuitNode.AppendChild(DetalleAsociados.CreateTextNode("00000000000"))
                CbteAsocDetalleAsociados.AppendChild(CuitNode)
            End If
            '
            Dim CbteFch As XmlNode = DetalleAsociados.CreateElement("CbteFch")
            CbteFch.AppendChild(DetalleAsociados.CreateTextNode(Format(DatosParaAfip.FechaCbte, "yyyyMMdd")))
            CbteAsocDetalleAsociados.AppendChild(CbteFch)
        End If
        '
        'Define estructura Opcionales del Envio al  WS para Adherido....................................
        Dim DetalleOpcinales As XmlDocument = New XmlDocument()
        DetalleOpcinales.LoadXml("<Detalle></Detalle>")
        If DatosParaAfip.EsFCE And Tipo = "F" Then
            Dim CbteAsocDetalleOpciones As XmlElement = DetalleOpcinales.CreateElement("Opcional")
            DetalleOpcinales.DocumentElement.AppendChild(CbteAsocDetalleOpciones)
            '
            Dim IdNode As XmlNode = DetalleOpcinales.CreateElement("Id")
            IdNode.AppendChild(DetalleOpcinales.CreateTextNode(2101))   'codigo para informar CBU.
            CbteAsocDetalleOpciones.AppendChild(IdNode)
            '
            Dim AlicuotaNode As XmlNode = DetalleOpcinales.CreateElement("Valor")
            AlicuotaNode.AppendChild(DetalleOpcinales.CreateTextNode(DatosParaAfip.CBU))
            CbteAsocDetalleOpciones.AppendChild(AlicuotaNode)
            '''
            Dim CbteAsocDetalleOpciones27 As XmlElement = DetalleOpcinales.CreateElement("Opcional")
            DetalleOpcinales.DocumentElement.AppendChild(CbteAsocDetalleOpciones27)
            '
            Dim IdNode27 As XmlNode = DetalleOpcinales.CreateElement("Id")
            IdNode27.AppendChild(DetalleOpcinales.CreateTextNode(27))   'codigo para informar CBU.
            CbteAsocDetalleOpciones27.AppendChild(IdNode27)
            '
            Dim AlicuotaNode27 As XmlNode = DetalleOpcinales.CreateElement("Valor")
            'no poner DatosParaAfip.AgenteDeposito en VALOR, anda en homologacion pero no en produccion. 
            If DatosParaAfip.AgenteDeposito = "" Then  'para las que fueron grabadas antes de la modificacion.
                AlicuotaNode27.AppendChild(DetalleOpcinales.CreateTextNode("ADC"))
            End If
            If DatosParaAfip.AgenteDeposito = "ADC" Then
                AlicuotaNode27.AppendChild(DetalleOpcinales.CreateTextNode("ADC"))
            End If
            If DatosParaAfip.AgenteDeposito = "SCA" Then
                AlicuotaNode27.AppendChild(DetalleOpcinales.CreateTextNode("SCA"))
            End If
            CbteAsocDetalleOpciones27.AppendChild(AlicuotaNode27)
        End If
        If DatosParaAfip.EsFCE And Tipo <> "F" Then
            Dim CbteAsocDetalleOpciones As XmlElement = DetalleOpcinales.CreateElement("Opcional")
            DetalleOpcinales.DocumentElement.AppendChild(CbteAsocDetalleOpciones)
            '
            Dim IdNode As XmlNode = DetalleOpcinales.CreateElement("Id")
            IdNode.AppendChild(DetalleOpcinales.CreateTextNode(22))   'codigo para si es anulacion.
            CbteAsocDetalleOpciones.AppendChild(IdNode)
            '
            Dim AlicuotaNode As XmlNode = DetalleOpcinales.CreateElement("Valor")
            AlicuotaNode.AppendChild(DetalleOpcinales.CreateTextNode(DatosParaAfip.EsAnulacion))
            CbteAsocDetalleOpciones.AppendChild(AlicuotaNode)
        End If


        '--Produccion o-------------------------------------------------------------------------------------------------
        Dim Certificado As String = "CER" & Strings.Left(GCuitEmpresa, 2) & Strings.Mid(GCuitEmpresa, 4, 8) & Strings.Right(GCuitEmpresa, 1) & ".p12"
        Dim Aut As New LibreriaAfipProduccion.Autorizar
        Mensaje = Aut.AurotizaComprobante("C:\XML Afip\", Certificado, CuitEmpresaW, Cabeza, Detalle, DetalleIva, DetalleTributos, DetalleAsociados, DetalleOpcinales)
        '--------------------------------------------------------------------------------------------------------------

        If Aut.CAE <> "" Then
            CAE = Aut.CAE
            FechaCae = Aut.FechaCAE
            Mensaje = "AUTORIZADO.      CAE:  " & Aut.CAE & "   Fecha CAE: " & Aut.FechaCAE + vbCrLf + vbCrLf + Mensaje
        Else
            Dim Mens As String = AnalizaError("C:\XML Afip", "WSFEV1_objFECAEResponse.xml")
            If Mens <> "" Then
                Mensaje = Mens + vbCrLf + "NO AUTORIZADO." + vbCrLf + vbCrLf + Mensaje
            Else
                Mensaje = "NO AUTORIZADO." + vbCrLf + vbCrLf + Mensaje
            End If
        End If

        Return Mensaje

    End Function
    Private Function AnalizaError(ByVal Path As String, ByVal Archivo As String) As String

        Dim Mensaje As String = ""

        Try
            Dim m_xmld As XmlDocument
            Dim m_nodelist As XmlNodeList
            Dim m_node As XmlNode

            'Creamos el "Document"
            m_xmld = New XmlDocument()

            'Cargamos el archivo
            m_xmld.Load(Path & "\" & Archivo)

            'Cargamos Lista de Codigos de Errores. 
            Dim elemList As XmlNodeList = m_xmld.GetElementsByTagName("Code")
            For Each m_node In elemList
                Dim Men = BuscaMensajeError("A", m_node.InnerText)
                If Men <> "" Then
                    If Mensaje <> "" Then
                        Mensaje = Mensaje + vbCrLf + Men
                    Else
                        Mensaje = Men
                    End If
                End If
            Next
        Catch ex As Exception
            Mensaje = ""
        End Try

        Return Mensaje

    End Function
    Public Function HallaCaeComprobante(ByVal Tipo As String, ByVal TipoNota As Integer, ByVal TipoFactura As Integer, ByVal Comprobante As Decimal, ByRef Cae As String, ByRef FechaCae As String, ByRef Resultado As String, ByRef ImpTotal As String, ByRef CbteTipoAsociado As Integer, ByRef CbteAsociado As Decimal, ByRef Concepto As String, ByVal ESFCE As Boolean) As String

        Cae = ""
        FechaCae = ""
        Resultado = ""
        ImpTotal = ""
        CbteTipoAsociado = 0
        CbteAsociado = 0
        Concepto = ""

        Dim CbteTipoAsociadoW As String
        Dim CbtePtoVtaAsociadoW As String
        Dim CbteNroAsociadoW As String

        Dim CbteTipo As Integer

        If Not ESFCE Then
            Select Case Tipo
                Case "CD"
                    CbteTipo = HallaTipoSegunAfip(TipoFactura, TipoNota)
                Case "F"
                    CbteTipo = HallaTipoSegunAfip(TipoFactura, 1)
                Case "C"
                    CbteTipo = HallaTipoSegunAfip(TipoFactura, 2)
            End Select
        Else
            Select Case Tipo
                Case "CD"
                    CbteTipo = HallaTipoSegunAfipFCE(TipoFactura, TipoNota)
                Case "F"
                    CbteTipo = HallaTipoSegunAfipFCE(TipoFactura, 1)
                Case "C"
                    CbteTipo = HallaTipoSegunAfipFCE(TipoFactura, 2)
            End Select
        End If

        Dim PtoVta As Integer = Strings.Mid(Comprobante.ToString, 2, 4)
        Dim Cbte As Decimal = Strings.Right(Comprobante.ToString, 8)
        Dim Certificado As String = "CER" & Strings.Left(GCuitEmpresa, 2) & Strings.Mid(GCuitEmpresa, 4, 8) & Strings.Right(GCuitEmpresa, 1) & ".p12"
        Dim Cuit As String = Strings.Left(GCuitEmpresa, 2) & Strings.Mid(GCuitEmpresa, 4, 8) & Strings.Right(GCuitEmpresa, 1)


        Dim Con As New LibreriaAfipProduccion.Consultas
        Dim Mensaje As String = Con.ConsultaCAECbte("C:\XML Afip\", Certificado, Cuit, PtoVta, CbteTipo, Cbte, Cae, FechaCae, Resultado, ImpTotal, CbteTipoAsociadoW, CbtePtoVtaAsociadoW, CbteNroAsociadoW, Concepto)
        '-------------------------------------------------------------------------------------------------------

        If Mensaje = "" And Cae <> "" And CbteNroAsociadoW <> "" Then
            CbteAsociado = HallaTipoIva(CInt(CbteTipoAsociadoW)) & Format(CInt(CbtePtoVtaAsociadoW), "0000") & Format(CDec(CbteNroAsociadoW), "00000000")
            CbteTipoAsociado = HallaTipoComprobanteDelSistema(CInt(CbteTipoAsociadoW))
        End If

        Return Mensaje

    End Function
    Private Function HallaConceptoAsociado(ByVal TipoNota As Integer, ByVal LetraIva As Integer, ByVal Nota As Decimal, ByRef Concepto As Integer) As Boolean

        Dim Cae As String
        Dim FechaCae As String
        Dim Resultado As String
        Dim ImpTotal As String
        Dim CbteTipoAsociado As Integer
        Dim CbteAsociado As Decimal
        Dim ConceptoW = ""

        Dim mensaje As String = HallaCaeComprobante("CD", TipoNota, LetraIva, Nota, Cae, FechaCae, Resultado, ImpTotal, CbteTipoAsociado, CbteAsociado, ConceptoW, False)
        If ConceptoW <> "" Then
            Concepto = Val(ConceptoW)
            Return True
        Else
            MsgBox(mensaje)
            Return False
        End If

    End Function
    Private Function ArmaXMLIva(ByVal TipoArticulo As Integer, ByVal DtDetalle As DataTable, ByRef DetalleIva As XmlDocument, ByRef BaseIva As Decimal, ByRef Exento As Decimal, ByRef NoGrabado As Decimal, ByRef Iva As Decimal) As String

        Exento = 0
        NoGrabado = 0
        Iva = 0
        BaseIva = 0

        Dim BaseW As Decimal = 0
        Dim ListaDeIvas As New List(Of ItemIva)

        For Each Row As DataRow In DtDetalle.Rows
            BaseW = CalculaNeto(Row("Cantidad"), Row("Precio"))
            Dim Res = HallaCondicionIvaArticulo(TipoArticulo, Row("Articulo"))
            Select Case Res
                Case -1
                    Return "Error Base de Datos al Leer Tabla: Articulos."
                Case 1
                    Exento = Exento + BaseW
                Case 2
                    NoGrabado = NoGrabado + BaseW
                Case 0
                    BaseIva = BaseIva + BaseW
                    ArmaListaIva(ListaDeIvas, BaseW, Row("Iva"), 0)
            End Select
        Next

        For Each Item As ItemIva In ListaDeIvas
            Item.Importe = CalculaIva(1, Item.Base, Item.Iva)
            Iva = Iva + Item.Importe
            Dim Respuesta = GeneraXMLIva(DetalleIva, Item.Base, Item.Importe, Item.Iva)
            If Respuesta <> "" Then Return Respuesta
        Next

        Return ""

    End Function
    Private Function ArmaXMLIvaPrecioFinal(ByVal TipoArticulo As Integer, ByVal DtDetalle As DataTable, ByRef DetalleIva As XmlDocument, ByRef BaseIva As Decimal, ByRef Exento As Decimal, ByRef NoGrabado As Decimal, ByRef Iva As Decimal) As String

        Exento = 0
        NoGrabado = 0
        Iva = 0
        BaseIva = 0

        Dim BaseW As Decimal = 0
        Dim ImporteIvaW As Decimal = 0
        Dim ListaDeIvas As New List(Of ItemIva)

        For Each Row As DataRow In DtDetalle.Rows
            Dim CoeficienteW As Decimal = 1 + CDec(Row("Iva")) / 100
            BaseW = Row("TotalArticulo") / CoeficienteW
            ImporteIvaW = Row("TotalArticulo") - BaseW
            Dim Res = HallaCondicionIvaArticulo(TipoArticulo, Row("Articulo"))
            Select Case Res
                Case -1
                    Return "Error Base de Datos al Leer Tabla: Articulos."
                Case 1
                    Exento = Exento + BaseW
                Case 2
                    NoGrabado = NoGrabado + BaseW
                Case 0
                    BaseIva = BaseIva + BaseW
                    ArmaListaIva(ListaDeIvas, BaseW, Row("Iva"), ImporteIvaW)
            End Select
        Next

        BaseIva = Trunca(BaseIva)

        For Each Item As ItemIva In ListaDeIvas
            Iva = Iva + Item.Importe
            Item.Importe = Trunca(Item.Importe)
            Item.Base = Trunca(Item.Base)
            Dim Respuesta = GeneraXMLIva(DetalleIva, Item.Base, Item.Importe, Item.Iva)
            If Respuesta <> "" Then Return Respuesta
        Next

        Iva = Trunca(Iva)
        Exento = Trunca(Exento)
        NoGrabado = Trunca(NoGrabado)

        Return ""

    End Function
    Private Function ArmaXMLIvaCredito(ByVal TipoArticulo As Integer, ByVal DtDetalle As DataTable, ByRef DetalleIva As XmlDocument, ByRef BaseIva As Decimal, ByRef Exento As Decimal, ByRef NoGrabado As Decimal, ByRef Iva As Decimal) As String

        Exento = 0
        NoGrabado = 0
        Iva = 0
        BaseIva = 0

        Dim BaseW As Decimal = 0
        Dim ListaDeIvas As New List(Of ItemIva)

        For Each Row As DataRow In DtDetalle.Rows
            BaseW = CalculaNeto(Row("Cantidad"), Row("Precio"))
            Dim Res = HallaCondicionIvaArticulo(TipoArticulo, Row("Articulo"))
            Select Case Res
                Case -1
                    Return "Error Base de Datos al Leer Tabla: Articulos."
                Case 1
                    Exento = Exento + BaseW
                Case 2
                    NoGrabado = NoGrabado + BaseW
                Case 0
                    BaseIva = BaseIva + BaseW
                    ArmaListaIva(ListaDeIvas, BaseW, Row("Iva"), 0)
            End Select
        Next

        For Each Item As ItemIva In ListaDeIvas
            Item.Importe = CalculaIva(1, Item.Base, Item.Iva)
            Iva = Iva + Item.Importe
            Dim Respuesta = GeneraXMLIva(DetalleIva, Item.Base, Item.Importe, Item.Iva)
            If Respuesta <> "" Then Return Respuesta
        Next

        Return ""

    End Function
    Private Function ArmaXMLIvaDebitoCredito(ByVal DtDetalle As DataTable, ByRef DetalleIva As XmlDocument, ByRef BaseIva As Decimal, ByRef Exento As Decimal, ByRef NoGrabado As Decimal, ByRef Iva As Decimal) As String

        Exento = 0
        NoGrabado = 0
        Iva = 0
        BaseIva = 0

        Dim BaseW As Decimal = 0
        Dim ListaDeIvas As New List(Of ItemIva)

        For Each Row As DataRow In DtDetalle.Rows
            If Row("MedioPago") = 100 Then          'Para que no procese las Reten/Perc.
                BaseW = Row("Neto")
                BaseIva = BaseIva + BaseW
                ArmaListaIva(ListaDeIvas, BaseW, Row("Alicuota"), 0)
            End If
        Next

        For Each Item As ItemIva In ListaDeIvas
            Item.Importe = CalculaIva(1, Item.Base, Item.Iva)
            Iva = Iva + Item.Importe
            Dim Respuesta = GeneraXMLIva(DetalleIva, Item.Base, Item.Importe, Item.Iva)
            If Respuesta <> "" Then Return Respuesta
        Next

        Return ""

    End Function
    Private Function GeneraXMLIva(ByRef DetalleIva As XmlDocument, ByVal BaseW As Decimal, ByVal ImporteIvaW As Decimal, ByVal Iva As Decimal) As String

        Dim Codigo As Integer = 0
        Dim Esta As Boolean

        Select Case Iva
            Case 0
                Codigo = 3
            Case 10.5
                Codigo = 4
            Case 21
                Codigo = 5
            Case 27
                Codigo = 6
            Case 5
                Codigo = 8
            Case 2.5
                Codigo = 9
            Case Else
                Return "Iva: " & Iva & " No registrado en el Sistema AFIP."
        End Select
        '
        Dim Nodolist As XmlNodeList = DetalleIva.SelectNodes("/Detalle/Iva")
        Dim Nodo As XmlNode
        If Nodolist.Count = 0 Then Esta = False
        For Each Nodo In Nodolist
            Esta = False
            If Nodo.ChildNodes.Item(0).InnerText = Codigo Then
                Dim ImporteAux As Decimal = (CDec(Nodo.ChildNodes.Item(1).InnerText) + BaseW)
                Nodo.ChildNodes.Item(1).InnerText = ImporteAux.ToString
                ImporteAux = (CDec(Nodo.ChildNodes.Item(2).InnerText) + ImporteIvaW)
                Nodo.ChildNodes.Item(2).InnerText = ImporteAux.ToString
                Esta = True
            End If
        Next
        If Not Esta Then
            Dim IvaDetalleIva As XmlElement = DetalleIva.CreateElement("Iva")
            DetalleIva.DocumentElement.AppendChild(IvaDetalleIva)
            '
            Dim IdNode As XmlNode = DetalleIva.CreateElement("Id")
            IdNode.AppendChild(DetalleIva.CreateTextNode(Codigo))
            IvaDetalleIva.AppendChild(IdNode)
            '
            Dim BaseNode As XmlNode = DetalleIva.CreateElement("BaseImp")
            BaseNode.AppendChild(DetalleIva.CreateTextNode(BaseW))
            IvaDetalleIva.AppendChild(BaseNode)
            '
            Dim ImporteNode As XmlNode = DetalleIva.CreateElement("Importe")
            ImporteNode.AppendChild(DetalleIva.CreateTextNode(ImporteIvaW))
            IvaDetalleIva.AppendChild(ImporteNode)
        End If

        Return ""

    End Function
    Private Function ArmaXMLTributos(ByRef DetalleTributo As XmlDocument, ByVal DtPercepcionesW As DataTable, ByRef Total As Decimal) As String

        Total = 0

        For Each Row As DataRow In DtPercepcionesW.Rows
            Dim TributoDetalleTributo As XmlElement = DetalleTributo.CreateElement("Tributo")
            DetalleTributo.DocumentElement.AppendChild(TributoDetalleTributo)
            '
            Dim IdNode As XmlNode = DetalleTributo.CreateElement("Id")
            IdNode.AppendChild(DetalleTributo.CreateTextNode(Row("CodigoAfip")))
            TributoDetalleTributo.AppendChild(IdNode)
            '
            Dim AlicNode As XmlNode = DetalleTributo.CreateElement("Alic")
            AlicNode.AppendChild(DetalleTributo.CreateTextNode(Row("Alicuota")))
            TributoDetalleTributo.AppendChild(AlicNode)
            '
            Dim Base As Decimal = Trunca(Row("Importe") * 100 / Row("Alicuota"))
            Dim BaseImpNode As XmlNode = DetalleTributo.CreateElement("BaseImp")
            BaseImpNode.AppendChild(DetalleTributo.CreateTextNode(Base))
            TributoDetalleTributo.AppendChild(BaseImpNode)
            '
            Dim ImporteNode As XmlNode = DetalleTributo.CreateElement("Importe")
            ImporteNode.AppendChild(DetalleTributo.CreateTextNode(Row("Importe")))
            TributoDetalleTributo.AppendChild(ImporteNode)
            '
            Dim Nombre As String = ""
            HallaDatoGenerico("SELECT Nombre FROM Tablas WHERE Tipo = 41 and CodigoAfipElectronico = " & Row("CodigoAfip") & ";", Conexion, Nombre)
            If Nombre = "" Then
                Return "Error al Leer Arch. Tablas.Percepcion/Retencion o Falta Código AFIP para : " & Row("Percepcion")
            End If
            Dim DescNode As XmlNode = DetalleTributo.CreateElement("Desc")
            DescNode.AppendChild(DetalleTributo.CreateTextNode(Nombre))
            TributoDetalleTributo.AppendChild(DescNode)
            Total = Total + Row("Importe")
        Next

        Return ""

    End Function
    Private Sub ArmaListaIva(ByRef Lista As List(Of ItemIva), ByVal BaseW As Decimal, ByVal Iva As Decimal, ByVal ImpIva As Decimal)

        Dim NewItem As New ItemIva
        NewItem.Base = BaseW
        NewItem.Clave = 0
        NewItem.Importe = ImpIva
        NewItem.Iva = Iva

        If Lista.Count = 0 Then
            Lista.Add(NewItem) : Exit Sub
        End If

        For Each Item As ItemIva In Lista
            If Item.Iva = Iva Then
                Item.Base = Item.Base + BaseW
                Item.Importe = Item.Importe + ImpIva
                Exit Sub
            End If
        Next

        Lista.Add(NewItem)

    End Sub
    Public Function VerificaRecursosAFIP(ByVal Carpeta As String) As Boolean

        If Not IsConnectionAvailable() Then
            If MsgBox("No Hay Conexión con AFIP. Si el Comprobante es Electrónicoa podría ser Rechazado. Desea Continuar Igualmente? (S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Return False
            Else
                Return True
            End If
        End If

        If Not System.IO.Directory.Exists(Carpeta) Then
            If MsgBox("Carpeta XML AFIP No se Encontro. No se Podra Generar Comprobante Electrónico. Desea Continuar Igualmente? (S/N)? ", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Return False
            Else
                Return True
            End If
            If Not File.Exists(Carpeta & "CER" & Strings.Left(GCuitEmpresa, 2) & Strings.Mid(GCuitEmpresa, 4, 8) & Strings.Right(GCuitEmpresa, 1) & ".p12") Then
                If MsgBox("Certificado de la Empresa No se Encontro. No se Podra Generar Comprobante Electrónico. Desea Continuar Igualmente? (S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Return False
                Else
                    Return True
                End If
            End If
        End If

        Return True

    End Function
    Private Function IsConnectionAvailable() As Boolean

        'Anulada por que daba false cuando la comunicacion con afip era Mala. 
        Return True

        Dim objUrl As New System.Uri("http://www.afip.gob.ar")
        Dim objWebReq As System.Net.WebRequest
        objWebReq = System.Net.WebRequest.Create(objUrl)
        Dim objResp As System.Net.WebResponse
        Try
            objResp = objWebReq.GetResponse
            objResp.Close()
            objWebReq = Nothing
            Return True
        Catch ex As Exception
            If (objResp IsNot Nothing) Then
                objResp.Close() : objResp = Nothing
            End If
            Return False
        End Try

    End Function
    Public Sub ControlaComprobante(ByVal Tipo As String, ByVal TipoNota As Integer, ByVal TipoFactura As Integer, ByVal Comprobante As Decimal, ByRef Cae As String, ByRef ImpTotal As String)

        Dim CbteTipo As Integer

        CbteTipo = HallaTipo(Tipo, TipoFactura, TipoNota)

        Dim PtoVta As Integer = Strings.Mid(Comprobante.ToString, 2, 4)
        Dim Cbte As Decimal = Strings.Right(Comprobante.ToString, 8)
        Dim Certificado As String = "CER" & Strings.Left(GCuitEmpresa, 2) & Strings.Mid(GCuitEmpresa, 4, 8) & Strings.Right(GCuitEmpresa, 1) & ".p12"
        Dim Cuit As String = Strings.Left(GCuitEmpresa, 2) & Strings.Mid(GCuitEmpresa, 4, 8) & Strings.Right(GCuitEmpresa, 1)

        Dim Con As New LibreriaAfipProduccion.Consultas
        Dim Mensaje As String = Con.ConsultaCbte("C:\XML Afip\", Certificado, Cuit, PtoVta, CbteTipo, Cbte, ImpTotal, Cae)

        Exit Sub

        'Anulado------ pero sirve.
        Dim XmlConsulta As New System.Xml.XmlDocument()
        Dim DireccionConsulta As String = "CONSULTA" & Strings.Left(GCuitEmpresa, 2) & Strings.Mid(GCuitEmpresa, 4, 8) & Strings.Right(GCuitEmpresa, 1) & ".xml"  '"WSFEV1_objFECompConsulta.xml"
        XmlConsulta.Load("C:\XML Afip\" & DireccionConsulta)

        Dim nodoe As XmlNode = XmlConsulta.DocumentElement

        nodoe = nodoe.ChildNodes(0)
        Dim NodoList As XmlNodeList = nodoe.ChildNodes

        MuestraNodos(NodoList, ImpTotal, Cae)

    End Sub
    Private Sub MuestraNodos(ByVal NodoList As XmlNodeList, ByRef ImpTotal As String, ByRef Cae As String)

        Dim oNodo As XmlNode

        For Each oNodo In NodoList
            If oNodo.NodeType = XmlNodeType.Text Then
                Select Case oNodo.ParentNode.Name
                    Case "ImpTotal"
                        ImpTotal = oNodo.Value
                    Case "CodAutorizacion"
                        Cae = oNodo.Value
                    Case Else
                        '      MsgBox(oNodo.ParentNode.Name)
                End Select
            Else
                MuestraNodos(oNodo.ChildNodes, ImpTotal, Cae)
            End If
        Next

    End Sub
    Public Function HallaTipo(ByVal Tipo As String, ByVal TipoFactura As Integer, ByVal TipoNota As Integer) As Integer

        If Tipo = "F" Then              'factura.
            Select Case TipoFactura
                Case 1
                    Return 1
                Case 2
                    Return 6
                Case 3
                    Return 11
                Case 5
                    Return 51
            End Select
        End If

        If Tipo = "C" Then         'credito con devolucion.
            Select Case TipoFactura
                Case 1
                    Return 3
                Case 2
                    Return 8
                Case 3
                    Return 13
                Case 5
                    Return 53
            End Select
        End If

        If Tipo = "CD" Then        'notas debito/credito. 
            Select Case TipoFactura
                Case 1
                    Select Case TipoNota
                        Case 5, 6
                            Return 2
                        Case 7, 8
                            Return 3
                    End Select
                Case 2
                    Select Case TipoNota
                        Case 5, 6
                            Return 7
                        Case 7, 8
                            Return 8
                    End Select
                Case 3
                    Select Case TipoNota
                        Case 5, 6
                            Return 12
                        Case 7, 8
                            Return 13
                    End Select
                Case 5
                    Select Case TipoNota
                        Case 5, 6
                            Return 52
                        Case 7, 8
                            Return 53
                    End Select
            End Select
        End If

    End Function
    Public Function HallaTipoEsFCE(ByVal Tipo As String, ByVal TipoFactura As Integer, ByVal TipoNota As Integer) As Integer

        If Tipo = "F" Then              'factura.
            Select Case TipoFactura
                Case 1
                    Return 201
                Case 2
                    Return 206
                Case 3
                    Return 211
                Case 5
                    Return 51
            End Select
        End If

        If Tipo = "C" Then         'credito con devolucion.
            Select Case TipoFactura
                Case 1
                    Return 203
                Case 2
                    Return 208
                Case 3
                    Return 213
                Case 5
                    Return 53
            End Select
        End If

        If Tipo = "CD" Then        'notas debito/credito. 
            Select Case TipoFactura
                Case 1
                    Select Case TipoNota
                        Case 5, 6
                            Return 202
                        Case 7, 8
                            Return 203
                    End Select
                Case 2
                    Select Case TipoNota
                        Case 5, 6
                            Return 207
                        Case 7, 8
                            Return 208
                    End Select
                Case 3
                    Select Case TipoNota
                        Case 5, 6
                            Return 212
                        Case 7, 8
                            Return 213
                    End Select
                Case 5
                    Select Case TipoNota
                        Case 5, 6
                            Return 52
                        Case 7, 8
                            Return 53
                    End Select
            End Select
        End If

    End Function
    Public Function HallaTipoSegunAfip(ByVal TipoIva As Integer, ByVal TipoNota As Integer) As Integer

        If TipoNota = 2 Then TipoNota = 7 'si es nota credito con devolucion la transform en N.C.Financiera.

        Select Case TipoNota
            Case 1   'Factura.
                Select Case TipoIva
                    Case 1
                        Return 1
                    Case 2
                        Return 6
                    Case 3
                        Return 11
                    Case 5
                        Return 51
                End Select
            Case 5, 6     'N.Debitos.
                Select Case TipoIva
                    Case 1
                        Return 2
                    Case 2
                        Return 7
                    Case 3
                        Return 12
                    Case 5
                        Return 52
                End Select
            Case 7, 8     'N.Creditos.
                Select Case TipoIva
                    Case 1
                        Return 3
                    Case 2
                        Return 8
                    Case 3
                        Return 13
                    Case 5
                        Return 53
                End Select
        End Select

    End Function
    Public Function HallaTipoSegunAfipFCE(ByVal TipoIva As Integer, ByVal TipoNota As Integer) As Integer

        If TipoNota = 2 Then TipoNota = 7 'si es nota credito con devolucion la transform en N.C.Financiera.

        Select Case TipoNota
            Case 1   'Factura.
                Select Case TipoIva
                    Case 1
                        Return 201
                    Case 2
                        Return 206
                    Case 3
                        Return 211
                    Case 5
                        Return 51
                End Select
            Case 5, 6     'N.Debitos.
                Select Case TipoIva
                    Case 1
                        Return 202
                    Case 2
                        Return 207
                    Case 3
                        Return 212
                    Case 5
                        Return 52
                End Select
            Case 7, 8     'N.Creditos.
                Select Case TipoIva
                    Case 1
                        Return 203
                    Case 2
                        Return 208
                    Case 3
                        Return 213
                    Case 5
                        Return 53
                End Select
            Case Else
                MsgBox("Tipo Iva " & TipoIva & " No Se encuentra en el sistema AFIP.") : Return -1000
        End Select

    End Function
    Private Function HallaCondicionIvaReceptor(ByVal TipoIva As Integer) As Integer

        Select Case TipoIva
            Case 1
                Return 1
            Case 6
                Return 6
            Case 2
                Return 4
            Case 3
                Return 5
            Case 4
                Return 9
            Case 5
                Return 1
            Case Else
                MsgBox("Tipo Iva " & TipoIva & " No Se encuentra en el sistema AFIP.") : Return -1000
        End Select

    End Function
    Private Function HallaTipoIva(ByVal Tipo As Integer) As Integer

        Select Case Tipo
            Case 1, 2, 3
                Return 1
            Case 6, 7, 8
                Return 2
            Case 11, 12, 13
                Return 3
            Case 51, 52, 53
                Return 5
        End Select

    End Function
    Private Function HallaTipoComprobanteDelSistema(ByVal Tipo As Integer) As Integer

        Select Case Tipo
            Case 1, 6, 11, 51
                Return 1            'Factura.
            Case 2, 7, 12, 52
                Return 5            'N.Debito.
            Case 3, 8, 13, 53
                Return 7            'N.Credito.  
        End Select

    End Function
    Public Sub VerificaCAE(ByVal Tipo As String, ByVal TipoNota As Integer, ByVal TipoFactura As Integer, ByVal Comprobante As Decimal, ByVal Cae As Decimal, ByVal ImpTotal As Decimal)

        Dim CaeW As String = 0
        Dim ImporteW As String = 0

        ControlaComprobante("F", TipoNota, TipoFactura, Comprobante, CaeW, ImporteW)

        If Cae <> CaeW Then
            MsgBox(" Cae Grabado en factura No Se Corresponde con el grabado en AFIP.")
        End If

        ImporteW = Strings.Replace(ImporteW, ".", ",")

        If CDec(ImporteW) <> ImpTotal Then
            MsgBox(" Importe Grabado en factura No Se Corresponde con el grabado en AFIP.")
        End If

    End Sub
    Public Sub HallaDatosFCE(ByRef Cbu As String, ByRef AliasW As String, ByRef Minimo As Decimal)

        Cbu = ""
        AliasW = ""
        Minimo = 0

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT CBU,ImporteMinimoFCE FROM DatosEmpresa;", Conexion, Dt) Then End
        Cbu = Dt.Rows(0).Item("CBU")
        Minimo = Dt.Rows(0).Item("ImporteMinimoFCE")

        Dt.Dispose()

    End Sub
    Private Function ExisteTabla(ByVal ConexionStr As String, ByVal NombreTabla As String) As Boolean

        Dim Sql As String = "SELECT COUNT(*) FROM " & NombreTabla & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Resul = Cmd.ExecuteScalar()
                    If Not IsDBNull(Resul) Then
                        Return True
                    Else : Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return False
        Finally
        End Try

    End Function
    Private Function HallaClienteCreditoDevolucion(ByVal Factura As Decimal) As Integer

        Dim ClienteW As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cliente FROM FacturasCabeza WHERE Factura = " & Factura & ";", Miconexion)
                    ClienteW = Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error al leer Archivo Clientes. ", MsgBoxStyle.Critical)
            End
        End Try

        Return ClienteW

    End Function
    Private Function HallaTipoIvaCliente(ByVal Cliente As Integer) As Integer

        Dim TipoIvaW As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT TipoIva FROM Clientes WHERE Clave = " & Cliente & ";", Miconexion)
                    TipoIvaW = Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error al leer Archivo Clientes. ", MsgBoxStyle.Critical)
            End
        End Try

        Return TipoIvaW

    End Function
    Public Function ArmaDatoParaQR(ByVal TipoNota As Integer, ByVal Fecha As Date, ByVal Comprobante As Decimal, ByVal EsFCE As Boolean, ByVal Cae As Decimal, ByVal Importe As Decimal, ByVal Cuit As String, ByVal DocTipo As Integer, ByVal DocNro As Double, ByVal TipoIva As Integer) As Image

        Dim DatoQr As String

        Dim FechaStr As String = Format(Fecha.Year, "0000") & "-" & Format(Fecha.Month, "00") & "-" & Format(Fecha.Day, "00")

        Dim TipoCmp As Integer
        Dim NroCmp As Integer
        Dim NumeroLetra As Integer
        Dim PuntoDeVenta As Integer
        Dim Numero As Decimal
        Dim ImporteW As String

        DescomponeNumeroComprobante(Comprobante, NumeroLetra, PuntoDeVenta, Numero)

        If EsFCE Then
            TipoCmp = HallaTipoSegunAfipFCE(NumeroLetra, TipoNota)
        Else
            TipoCmp = HallaTipoSegunAfip(NumeroLetra, TipoNota)
        End If
        NroCmp = Numero

        ImporteW = Importe.ToString.Replace(",", ".")

        Dim Moneda As String = "PES"
        Dim Cotizacion As Integer = 1
        Dim TipoDocRec As Integer
        Dim NroDocRec As Decimal

        If TipoIva <> 3 Then
            TipoDocRec = 80
            NroDocRec = CuitNumerico(Cuit)
        Else
            If DocTipo <> 0 And DocNro <> 0 Then
                TipoDocRec = DocTipo : NroDocRec = DocNro
            Else
                TipoDocRec = 99 : NroDocRec = 0
            End If
        End If

        Dim tipoCodAut As String = "E"
        Dim codAut As Long = Cae

        DatoQr = "{""ver"":1,""fecha"":""" & FechaStr & """,""cuit"":" & CuitNumerico(GCuitEmpresa) & ",""ptoVta"":" & PuntoDeVenta & ",""tipoCmp"":" & TipoCmp & ",""nroCmp"":" & NroCmp & ",""importe"":" & ImporteW & ",""moneda"":""" & Moneda & """,""ctz"":" & Cotizacion & ",""tipoDocRec"":" & TipoDocRec & ",""nroDocRec"":" & NroDocRec & ",""tipoCodAut"":""" & tipoCodAut & """,""codAut"":" & codAut & "}"

        Dim aa As New DllVarias
        DatoQr = aa.ConvertirBase64(DatoQr)

        Return aa.CovertirQR("https://www.afip.gob.ar/fe/qr/?p=" & DatoQr)

    End Function

End Module
