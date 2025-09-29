Imports System.Transactions
Imports System.Drawing.Printing
Imports System.Math
Imports ClassPassWord
Public Class UnaNotaCredito
    Public PFacturaB As Double
    Public PFacturaN As Double
    Public PNota As Double
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    Public PPaseDeProyectos As ItemPaseDeProyectos
    '
    Dim ListaDeLotes As List(Of FilaAsignacion)
    '
    Private MiEnlazador As New BindingSource
    '
    Private DtGrid As DataTable
    'Definicion para notas.
    Dim DtCabezaN As DataTable
    Dim DtCabezaB As DataTable
    Dim DtDetalleN As DataTable
    Dim DtDetalleB As DataTable
    Dim DtDetalleCompN As DataTable
    Dim DtDetalleCompB As DataTable
    Dim DtPercepciones As DataTable
    Dim DtLotesB As DataTable
    Dim DtLotesN As DataTable
    'Definicion para facturas. 
    Dim DtCabezaFacturaB As DataTable
    Dim DtCabezaFacturaN As DataTable
    Dim DtDetalleFacturaB As DataTable
    Dim DtDetalleFacturaN As DataTable
    Dim DtLotesFacturaB As DataTable
    Dim DtLotesFacturaN As DataTable
    '
    Dim DtFacturasCabeza As DataTable
    Dim DtNVLPCabeza As DataTable
    Dim DtComprobantesCabeza As DataTable
    Dim DtSaldosIniciales As DataTable
    Dim DtGridCompro As DataTable
    '
    Dim ListaDePercepciones As New List(Of ItemIvaReten)
    '
    Dim NotaRelacionada As Double
    Dim FacturaRelacionada As Double
    Dim UltimoNumero As Double
    Dim TipoFactura As Integer
    Dim UltimaFechaW As DateTime
    Dim UltimafechaContableW As DateTime
    Dim FechaContableAnt As DateTime
    Dim Pedido As Integer
    Dim Opcion As Integer = 0
    Dim NotaMixta As Boolean
    Dim FacturaMixta As Boolean
    Dim MonedaFactura As Integer
    Dim FormaPago As Integer
    Dim Remito As Double
    Dim EsExportacion As Boolean
    Dim EsFacturaElectronica As Boolean
    Dim DocumentoAsiento As Integer
    Dim DocumentoAsientoCosto As Integer
    Dim EsServicios As Boolean
    Dim EsSecos As Boolean
    Dim Calle As String
    Dim Numero As Integer
    Dim Localidad As String
    Dim Provincia As String
    Dim FacturaAnteriorOK As Boolean
    Dim TotalNetoPerc As Decimal
    Dim TotalPercepciones As Decimal
    Dim DocTipo As Integer
    Dim DocNro As Decimal
    'Para FCE.
    Dim EsFCE As Boolean
    'Para Nota Ticket Z.
    Dim EsZ As Boolean
    Dim EsTicket As Boolean
    Dim EsTicketA As Boolean
    Dim EsTicketB As Boolean
    'Para impresion.
    Dim Paginas As Integer = 0
    Dim Copias As Integer
    Dim ErrorImpresion As Boolean
    Dim ContadorCopias As Integer = 0
    Dim ContadorPaginas As Integer = 0
    Dim TotalPaginas As Integer = 0
    Dim ContadorLineasListado As Integer = 0
    Dim Listado As List(Of ItemListado)
    Private Sub UnaNotaDeCredito_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            PreparaEnlace(PPaseDeProyectos)
        End If
        '----------------------------------------------------------------------------------

        If Not PermisoEscritura(6) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False

        LlenaCombo(ComboCliente, "", "Clientes")
        LlenaComboTablas(ComboDeposito, 19)

        ArmaTipoIva(ComboTipoIva)
        LlenaComboTablas(ComboPais, 28)

        ComboEstado.DataSource = DtAfectaPendienteAnulada()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Codigo"
        ComboEstado.SelectedValue = 2

        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Dim Row As DataRow = ComboMoneda.DataSource.newrow
        Row("Nombre") = "PESOS"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"
        With ComboMoneda
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        GModificacionOk = False

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        LlenaCombosGrid()

        If PNota <> 0 Then ButtonAsigna.Visible = False

        UnTextoParaRecibo.Dispose()

        If EsExportacion Then
            UltimaFechaW = UltimaFechaExportacion(Conexion)
            If UltimaFechaW = "2/1/1000" Then
                MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
            End If
        Else
            UltimaFechaW = UltimaFecha(Conexion)
            If UltimaFechaW = "2/1/1000" Then
                MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
            End If
        End If

        If PNota = 0 And PFacturaB <> 0 Then    'Los puntos de venta de factura y nota deben tener la misma condicionn sobre Factura-Electronica.
            If DtCabezaFacturaB.Rows(0).Item("EsElectronica") And DtCabezaFacturaB.Rows(0).Item("Cae") = 0 Then  'Cuida que la factura este aprobada por Afip.
                MsgBox("Factura-Electrónica No Tiene CAE. Debe ser Aprobada por la AFIP.", MsgBoxStyle.Information)
                Me.Close() : Exit Sub
            End If
            If EsPuntoDeVentaFacturasElectronicas(GPuntoDeVenta) <> EsFacturaElectronica Then
                If EsFacturaElectronica Then
                    MsgBox("Punto de Venta de la Nota debe estar definida como Factura-Electrónica.", MsgBoxStyle.Information)
                    Me.Close() : Exit Sub
                Else
                    MsgBox("Punto de Venta de la Nota NO debe estar definida como Factura-Electrónica.", MsgBoxStyle.Information)
                    Me.Close() : Exit Sub
                End If
            End If
        End If

        If EsFacturaElectronica And PNota = 0 Then
            If Not VerificaRecursosAFIP("C:\XML Afip\") Then Me.Close() : Exit Sub
            UltimafechaContableW = UltimaFechacontableNotaCreditoDevolucion(Conexion, GPuntoDeVenta, TipoFactura)
            If UltimafechaContableW = "2/1/1000" Then
                MsgBox("Error Base de Datos al Leer Ultima Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
            End If
        End If

        If PNota = 0 Then
            If EsFacturaElectronica Or EsZ Then
                TextFechaContable.Text = ""
            Else
                TextFechaContable.Text = DateTime1.Text
                Panel7.Enabled = False
            End If
        End If
        If PNota <> 0 Then
            If DtCabezaB.Rows.Count <> 0 Then
                If (EsFacturaElectronica And DtCabezaB.Rows(0).Item("Cae") = 0) Or EsZ Then
                    Panel7.Enabled = False  'Cuando se pueda modificar poner en true. 
                End If
            Else
                If EsZ Then 'Cuando se pueda modificar poner en true. 
                    Panel7.Enabled = False
                Else
                    Panel7.Enabled = False
                End If
            End If
        End If

        FacturaAnteriorOK = True
        If EsFacturaElectronica And PNota = 0 And DtCabezaFacturaB.Rows.Count <> 0 Then
            If Not EsNotaCreditoAnteriorOk(UltimoNumero) Then
                FacturaAnteriorOK = False
                MsgBox("La Nota Credito " & NumeroEditado(UltimoNumero - 1) & " No Tiene Autorización AFIP. Si continua, Afip Rechazara este Comprobante.")
            End If
        End If

    End Sub
    Private Sub UnaNotaCredito_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If HayModifcacionInputacion() Then
            If MsgBox("Existe Modificaciones en Comprobantes Inputados. Desa Grabar Los Cambios? (Si/No) ", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                e.Cancel = True
            End If
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        GModificacionOk = False

        MiEnlazador.EndEdit()
        Grid.EndEdit()

        If FacturaMixta And Not PermisoTotal Then
            MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PNota <> 0 Then
            If HacerModificacion() Then
                If Not ArmaArchivos() Then Me.Close() : Exit Sub
            End If
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        'Arma Archivos para nota de exportacion. 
        Dim DtComprobantesOperacionB As New DataTable
        Dim DtComprobantesOperacionN As New DataTable
        If EsExportacion And Not IsNothing(PPaseDeProyectos) Then
            If Not LeeComprobantesOperacion(DtComprobantesOperacionB, DtComprobantesOperacionN) Then
                MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        '........................Arma Nota B................................
        If DtCabezaFacturaB.Rows.Count <> 0 Then
            If Not ArmaArchivoParaAlta(DtCabezaB, DtDetalleB, DtPercepciones, "B", DtCabezaFacturaB, DtDetalleFacturaB, DtLotesFacturaB, DtComprobantesOperacionB, DtDetalleCompB) Then Exit Sub
            If Not FacturaAnteriorOK Then
                MsgBox("La Nota de Credito no se puede Grabar hasta que no autorice la Nota de Credito anterior por la AFIP. Operacion se CANCELA.", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        '........................Arma Nota N................................. 
        If DtCabezaFacturaN.Rows.Count <> 0 Then
            If Not ArmaArchivoParaAlta(DtCabezaN, DtDetalleN, New DataTable, "N", DtCabezaFacturaN, DtDetalleFacturaN, DtLotesFacturaN, DtComprobantesOperacionN, DtDetalleCompN) Then Exit Sub
        End If

        ArmaAsignacionLotes(DtLotesB, 0, DtLotesN, 0)

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If DtDetalleB.Rows.Count <> 0 Then
            Dim EsFruta As Boolean = False
            If DtCabezaFacturaB.Rows(0).Item("EsSecos") Or DtCabezaFacturaB.Rows(0).Item("EsServicios") Then
                EsFruta = False
            Else
                EsFruta = True
            End If
            If TieneArticulosDesHabilitados(DtDetalleB, Conexion, EsFruta) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                If Not ArmaArchivos() Then Me.Close()
                Exit Sub
            End If
        End If
        If DtDetalleN.Rows.Count <> 0 Then
            Dim EsFruta As Boolean = False
            If DtCabezaFacturaN.Rows(0).Item("EsSecos") Or DtCabezaFacturaN.Rows(0).Item("EsServicios") Then
                EsFruta = False
            Else
                EsFruta = True
            End If
            If TieneArticulosDesHabilitados(DtDetalleN, ConexionN, EsFruta) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                If Not ArmaArchivos() Then Me.Close()
                Exit Sub
            End If
        End If

        'Arma Tipo Operaciones de Lotes para Asientos.
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable
        Dim DtAsientoCabezaCostoB As New DataTable
        Dim DtAsientoDetalleCostoB As New DataTable
        Dim DtAsientoCabezaCostoN As New DataTable
        Dim DtAsientoDetalleCostoN As New DataTable

        Dim PorcentajeB As Decimal = 0
        Dim PorcentajeN As Decimal = 0
        Dim ImporteW As Decimal = 0
        Dim ImporteRW As Decimal = 0

        If DtCabezaFacturaB.Rows.Count <> 0 And DtCabezaFacturaN.Rows.Count <> 0 Then
            If DtCabezaFacturaB.Rows(0).Item("Final") Then
                ImporteW = DtDetalleFacturaB.Rows(0).Item("TotalArticulo")
                ImporteRW = DtDetalleFacturaN.Rows(0).Item("TotalArticulo")
                PorcentajeB = Trunca(ImporteW * 100 / (ImporteW + ImporteRW))
                PorcentajeN = 100 - PorcentajeB
            Else
                ImporteW = CDec(DtDetalleFacturaB.Rows(0).Item("TotalArticulo")) - CalculaIva(DtDetalleFacturaB.Rows(0).Item("Cantidad"), DtDetalleFacturaB.Rows(0).Item("Precio"), DtDetalleFacturaB.Rows(0).Item("Iva"))
                ImporteRW = DtDetalleFacturaN.Rows(0).Item("TotalArticulo")
                PorcentajeB = Trunca(ImporteW * 100 / (ImporteW + ImporteRW))
                PorcentajeN = 100 - PorcentajeB
            End If
        Else
            If DtCabezaFacturaB.Rows.Count <> 0 Then PorcentajeB = 100
            If DtCabezaFacturaN.Rows.Count <> 0 Then PorcentajeN = 100
        End If

        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = -10;", Conexion, DtAsientoCabezaB) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = -10;", Conexion, DtAsientoDetalleB) Then Me.Close() : Exit Sub
            DtAsientoCabezaN = DtAsientoCabezaB.Clone
            DtAsientoDetalleN = DtAsientoDetalleB.Clone
            If DtCabezaB.Rows.Count <> 0 Then
                If Not ArmaArchivosAsiento("A", DtLotesB, DtAsientoCabezaB, DtAsientoDetalleB, 1) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    If Not ArmaArchivos() Then Me.Close()
                    Exit Sub
                End If
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                If Not ArmaArchivosAsiento("A", DtLotesN, DtAsientoCabezaN, DtAsientoDetalleN, 2) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    If Not ArmaArchivos() Then Me.Close()
                    Exit Sub
                End If
            End If
            DtAsientoCabezaCostoB = DtAsientoCabezaB.Clone
            DtAsientoDetalleCostoB = DtAsientoDetalleB.Clone
            DtAsientoCabezaCostoN = DtAsientoCabezaB.Clone
            DtAsientoDetalleCostoN = DtAsientoDetalleB.Clone
            If DtCabezaB.Rows.Count <> 0 Then
                If Not DtCabezaFacturaB.Rows(0).Item("EsExterior") Then
                    If Not ArmaArchivosAsientoCosto("A", DtAsientoCabezaCostoB, DtAsientoDetalleCostoB, PorcentajeB) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        If Not ArmaArchivos() Then Me.Close()
                        Exit Sub
                    End If
                Else
                    If Not ArmaArchivosAsientoCostoExportacion("A", DtDetalleFacturaB, DtDetalleFacturaN, DtAsientoCabezaCostoB, DtAsientoDetalleCostoB) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        If Not ArmaArchivos() Then Me.Close()
                        Exit Sub
                    End If
                End If
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                If Not DtCabezaFacturaN.Rows(0).Item("EsExterior") Then
                    If Not ArmaArchivosAsientoCosto("A", DtAsientoCabezaCostoN, DtAsientoDetalleCostoN, PorcentajeN) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        If Not ArmaArchivos() Then Me.Close()
                        Exit Sub
                    End If
                Else
                    If Not ArmaArchivosAsientoCostoExportacion("A", DtDetalleFacturaN, DtDetalleFacturaB, DtAsientoCabezaCostoN, DtAsientoDetalleCostoN) Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        If Not ArmaArchivos() Then Me.Close()
                        Exit Sub
                    End If
                End If
            End If
        End If

        'Actualiza Pedido.
        Dim DtPedidoDetalle As New DataTable
        If Pedido <> 0 Then
            If Not PreparaPedidoDetalle("B", DtPedidoDetalle) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                If Not ArmaArchivos() Then Me.Close()
                Exit Sub
            End If
        End If

        Dim NumeroN As Double = 0
        Dim NumeroAsientoB As Double
        Dim NumeroAsientoN As Double
        Dim NumeroAsientoCostoB As Double
        Dim NumeroAsientoCostoN As Double
        Dim NumeroW As Double
        Dim InternoB As Double
        Dim InternoN As Double

        For i As Integer = 1 To 50
            'Halla ultima numeracion Nota N.
            If DtCabezaN.Rows.Count <> 0 Then
                NumeroN = UltimaNumeracion(ConexionN)
                If NumeroN < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    If Not ArmaArchivos() Then Me.Close() : Exit Sub
                    Exit Sub
                End If
            End If
            'Halla ultima numeracion Nota B si es Ticket.
            If DtCabezaB.Rows.Count <> 0 And EsZ Then
                UltimoNumero = UltimaNumeracion(Conexion)
                If UltimoNumero < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    If Not ArmaArchivos() Then Me.Close() : Exit Sub
                    Exit Sub
                End If
            End If
            'Halla ultima numeracion interna.
            If DtCabezaB.Rows.Count <> 0 Then
                InternoB = UltimoNumeroInternoNotaCredito(TipoFactura, Conexion)
                If InternoB < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    If Not ArmaArchivos() Then Me.Close() : Exit Sub
                    Exit Sub
                End If
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                InternoN = UltimoNumeroInternoNotaCredito(TipoFactura, ConexionN)
                If InternoN < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    If Not ArmaArchivos() Then Me.Close() : Exit Sub
                    Exit Sub
                End If
            End If
            ' 
            If DtCabezaB.Rows.Count <> 0 Then
                DtCabezaB.Rows(0).Item("NotaCredito") = UltimoNumero
                DtCabezaB.Rows(0).Item("Interno") = InternoB
                If NumeroN <> 0 Then
                    DtCabezaB.Rows(0).Item("Rel") = True
                Else : DtCabezaB.Rows(0).Item("Rel") = False
                End If
                DtCabezaB.Rows(0).Item("Relacionada") = 0
                For Each Row As DataRow In DtDetalleB.Rows
                    Row.Item("NotaCredito") = UltimoNumero
                Next
                For Each Row As DataRow In DtPercepciones.Rows
                    If Row("Importe") <> 0 Then
                        Row.Item("Comprobante") = UltimoNumero
                    End If
                Next
                For Each Row As DataRow In DtLotesB.Rows
                    Row.Item("Comprobante") = UltimoNumero
                Next
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                DtCabezaN.Rows(0).Item("NotaCredito") = NumeroN
                DtCabezaN.Rows(0).Item("Interno") = InternoN
                If DtCabezaB.Rows.Count <> 0 Then
                    DtCabezaN.Rows(0).Item("Relacionada") = UltimoNumero
                    DtCabezaN.Rows(0).Item("Rel") = True
                Else : DtCabezaN.Rows(0).Item("Relacionada") = 0
                    DtCabezaN.Rows(0).Item("Rel") = False
                End If
                For Each Row As DataRow In DtDetalleN.Rows
                    Row.Item("NotaCredito") = NumeroN
                Next
                For Each Row As DataRow In DtLotesN.Rows
                    Row.Item("Comprobante") = NumeroN
                Next
            End If
            '
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                NumeroAsientoB = UltimaNumeracionAsiento(Conexion)
                If NumeroAsientoB < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    If Not ArmaArchivos() Then Me.Close() : Exit Sub
                    Exit Sub
                End If
                NumeroAsientoCostoB = NumeroAsientoB + 1
            End If
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                NumeroAsientoN = UltimaNumeracionAsiento(ConexionN)
                If NumeroAsientoN < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    If Not ArmaArchivos() Then Me.Close() : Exit Sub
                    Exit Sub
                End If
                NumeroAsientoCostoN = NumeroAsientoN + 1
            End If
            '
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                DtAsientoCabezaB.Rows(0).Item("Asiento") = NumeroAsientoB
                DtAsientoCabezaB.Rows(0).Item("Documento") = UltimoNumero
                For Each Row As DataRow In DtAsientoDetalleB.Rows
                    Row("Asiento") = NumeroAsientoB
                Next
            End If
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                DtAsientoCabezaN.Rows(0).Item("Asiento") = NumeroAsientoN
                DtAsientoCabezaN.Rows(0).Item("Documento") = NumeroN
                For Each Row As DataRow In DtAsientoDetalleN.Rows
                    Row("Asiento") = NumeroAsientoN
                Next
            End If
            If DtAsientoCabezaCostoB.Rows.Count <> 0 Then
                DtAsientoCabezaCostoB.Rows(0).Item("Asiento") = NumeroAsientoCostoB
                DtAsientoCabezaCostoB.Rows(0).Item("Documento") = UltimoNumero
                For Each Row As DataRow In DtAsientoDetalleCostoB.Rows
                    Row("Asiento") = NumeroAsientoCostoB
                Next
            End If
            If DtAsientoCabezaCostoN.Rows.Count <> 0 Then
                DtAsientoCabezaCostoN.Rows(0).Item("Asiento") = NumeroAsientoCostoN
                DtAsientoCabezaCostoN.Rows(0).Item("Documento") = NumeroN
                For Each Row As DataRow In DtAsientoDetalleCostoN.Rows
                    Row("Asiento") = NumeroAsientoCostoN
                Next
            End If
            '
            If DtDetalleCompB.Rows.Count <> 0 Then
                DtDetalleCompB.Rows(0).Item("Nota") = UltimoNumero
            End If
            If DtDetalleCompN.Rows.Count <> 0 Then
                DtDetalleCompN.Rows(0).Item("Nota") = NumeroN
            End If

            NumeroW = AltaNotaCredito(DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, DtAsientoCabezaCostoB, DtAsientoDetalleCostoB, DtAsientoCabezaCostoN, DtAsientoDetalleCostoN, DtComprobantesOperacionB, DtComprobantesOperacionN, DtPedidoDetalle, DtDetalleCompB, DtDetalleCompN)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If NumeroW = -10 Then
            MsgBox("Nota Ya Fue Impresa. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            PideAutorizacionAfip(DtCabezaB, DtDetalleB, DtPercepciones)          'Pide y graba CAE a la AFIP.----
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = False
            If DtCabezaB.Rows.Count <> 0 Then
                PNota = UltimoNumero
                PAbierto = True
            Else
                PNota = NumeroN
                PAbierto = False
            End If
            PFacturaB = 0
            PFacturaN = 0
        End If

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota Ya esta Anulada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If EsFacturaElectronica Then
            MsgBox("Comprobante Electrónico solo se Anula con Nota de Debito Electrónica. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If NotaMixta And Not PermisoTotal Then
            MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabezaFacturaB.Rows.Count <> 0 Then
            If DtLotesB.Rows.Count = 0 And DtCabezaFacturaB.Rows(0).Item("Estado") = 1 Then
                MsgBox("Factura Tiene Lotes Asignados. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
            If DtCabezaFacturaB.Rows(0).Item("EsExterior") Then
                MsgBox("Nota Exportación No Puede Anularse. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
        Else
            If DtLotesN.Rows.Count = 0 And DtCabezaFacturaN.Rows(0).Item("Estado") = 1 Then
                MsgBox("Factura Tiene Lotes Asignados. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
            If DtCabezaFacturaN.Rows(0).Item("EsExterior") Then
                MsgBox("Nota Exportación No Puede Anularse. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
        End If

        If DtDetalleCompB.Rows.Count <> 0 Or DtDetalleCompN.Rows.Count <> 0 Then
            MsgBox("Existe Comprobantes Imputados. Debe Borrar Imputaciones Para Continuar.", MsgBoxStyle.Exclamation) : Exit Sub
        End If

        If Not Valida() Then Exit Sub

        'Arma Archivos para nota de exportacion. 
        Dim DtComprobantesOperacionB As New DataTable
        Dim DtComprobantesOperacionN As New DataTable
        If EsExportacion Then
            If Not LeeComprobantesOperacion(DtComprobantesOperacionB, DtComprobantesOperacionN) Then
                MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If DtCabezaB.Rows.Count <> 0 Then
            ArmaArchivoParaBaja(DtCabezaB, DtDetalleB, "B", DtCabezaFacturaB, DtDetalleFacturaB, DtLotesFacturaB, DtComprobantesOperacionB)
        End If
        If DtCabezaN.Rows.Count <> 0 Then
            ArmaArchivoParaBaja(DtCabezaN, DtDetalleN, "N", DtCabezaFacturaN, DtDetalleFacturaN, DtLotesFacturaN, DtComprobantesOperacionN)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoCabezaCostoB As New DataTable
        Dim DtAsientoCabezaCostoN As New DataTable

        If GGeneraAsiento Then
            If DtCabezaB.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(4, DtCabezaB.Rows(0).Item("NotaCredito"), DtAsientoCabezaB, Conexion) Then
                    If Not ArmaArchivos() Then Me.Close()
                    Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                End If
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(4, DtCabezaN.Rows(0).Item("NotaCredito"), DtAsientoCabezaN, ConexionN) Then
                    If Not ArmaArchivos() Then Me.Close()
                    Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                End If
            End If
            If DtAsientoCabezaB.Rows.Count <> 0 Then DtAsientoCabezaB.Rows(0).Item("Estado") = 3
            If DtAsientoCabezaN.Rows.Count <> 0 Then DtAsientoCabezaN.Rows(0).Item("Estado") = 3
            '
            If DtCabezaB.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(6072, DtCabezaB.Rows(0).Item("NotaCredito"), DtAsientoCabezaCostoB, Conexion) Then
                    If Not ArmaArchivos() Then Me.Close()
                    Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                End If
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(6072, DtCabezaN.Rows(0).Item("NotaCredito"), DtAsientoCabezaCostoN, ConexionN) Then
                    If Not ArmaArchivos() Then Me.Close()
                    Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                End If
            End If
            If DtAsientoCabezaCostoB.Rows.Count <> 0 Then DtAsientoCabezaCostoB.Rows(0).Item("Estado") = 3
            If DtAsientoCabezaCostoN.Rows.Count <> 0 Then DtAsientoCabezaCostoN.Rows(0).Item("Estado") = 3
        End If

        'Actualiza Pedido.
        Dim DtPedidoDetalle As New DataTable
        If Pedido <> 0 Then
            If Not PreparaPedidoDetalle("A", DtPedidoDetalle) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                If Not ArmaArchivos() Then Me.Close()
                Exit Sub
            End If
        End If

        If MsgBox("Nota Credito se Anulara del Sistema. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            If Not ArmaArchivos() Then Me.Close()
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        GModificacionOk = False

        If Not AnulaNotaCredito(DtAsientoCabezaB, DtAsientoCabezaN, DtAsientoCabezaCostoB, DtAsientoCabezaCostoN, DtComprobantesOperacionB, DtComprobantesOperacionN, DtPedidoDetalle) Then
            MsgBox("Otro Usuario modifico datos o Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        Else
            MsgBox("Nota fue Anulada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
        End If

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Nota Ya esta Anulada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If DtCabezaB.Rows.Count <> 0 Then
            If EsFacturaElectronica And DtCabezaB.Rows(0).Item("Cae") <> 0 Then
                MsgBox("Nota Electrónica No se Puede BORRAR pues Tiene Autorización en la AFIP. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If NotaMixta And Not PermisoTotal Then
            MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabezaFacturaB.Rows.Count <> 0 Then
            If DtCabezaFacturaB.Rows(0).Item("EsExterior") Then
                MsgBox("Nota Exportación No Puede Borrarce. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
            If Not EsFacturaElectronica Then
                MsgBox("Nota Puede ANULARCE. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
        Else
            If DtCabezaFacturaN.Rows(0).Item("EsExterior") Then
                MsgBox("Nota Exportación No Puede Borrarce. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
        End If

        'ver si hay posteriores.
        Dim DtPuntosDeVenta As New DataTable
        If DtCabezaB.Rows.Count = 1 Then
            Dim PuntoVentaW As Integer
            Dim TipoIvaW As Integer
            Dim UltimoNumeroW As Decimal
            DescomponeNumeroComprobante(DtCabezaB.Rows(0).Item("NotaCredito"), TipoIvaW, PuntoVentaW, UltimoNumeroW)
            If HallaUltimaNumeracionW(TipoIvaW, PuntoVentaW, Conexion) <> DtCabezaB.Rows(0).Item("NotaCredito") Then
                MsgBox("Existe Notas Crédito Posteriores a Esta. Nota Crédito a Borrar Debe ser La Ultima Realizada. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            'Corre numeracion de las nota del punto de venta.
            UltimoNumeroW = UltimoNumeroW - 1
            If Not Tablas.Read("SELECT * FROM PuntosDeVenta WHERE Clave = " & PuntoVentaW & ";", Conexion, DtPuntosDeVenta) Then Exit Sub
            Select Case TipoIvaW
                Case 1
                    DtPuntosDeVenta.Rows(0).Item("NotasCreditoA") = UltimoNumeroW
                Case 2
                    DtPuntosDeVenta.Rows(0).Item("NotasCreditoB") = UltimoNumeroW
                Case 3
                    DtPuntosDeVenta.Rows(0).Item("NotasCreditoC") = UltimoNumeroW
                Case 4
                    DtPuntosDeVenta.Rows(0).Item("NotasCreditoE") = UltimoNumeroW
                Case 5
                    DtPuntosDeVenta.Rows(0).Item("NotasCreditoM") = UltimoNumeroW
            End Select
        End If

        If DtCabezaFacturaB.Rows.Count <> 0 Then
            If DtLotesB.Rows.Count = 0 And DtCabezaFacturaB.Rows(0).Item("Estado") = 1 Then
                MsgBox("Factura Tiene Lotes Asignados. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
        End If

        If DtDetalleCompB.Rows.Count <> 0 Or DtDetalleCompN.Rows.Count <> 0 Then
            MsgBox("Existe Comprobantes Imputados. Debe Borrar Imputaciones Para Continuar.", MsgBoxStyle.Exclamation) : Exit Sub
        End If

        If Not Valida() Then Exit Sub

        'Arma Archivos para nota de exportacion. 
        Dim DtComprobantesOperacionB As New DataTable
        Dim DtComprobantesOperacionN As New DataTable
        If EsExportacion Then
            If Not LeeComprobantesOperacion(DtComprobantesOperacionB, DtComprobantesOperacionN) Then
                MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If DtCabezaB.Rows.Count <> 0 Then
            ArmaArchivoParaBaja(DtCabezaB, DtDetalleB, "B", DtCabezaFacturaB, DtDetalleFacturaB, DtLotesFacturaB, DtComprobantesOperacionB)
        End If
        If DtCabezaN.Rows.Count <> 0 Then
            ArmaArchivoParaBaja(DtCabezaN, DtDetalleN, "N", DtCabezaFacturaN, DtDetalleFacturaN, DtLotesFacturaN, DtComprobantesOperacionN)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable
        Dim DtAsientoCabezaCostoB As New DataTable
        Dim DtAsientoDetalleCostoB As New DataTable
        Dim DtAsientoCabezaCostoN As New DataTable
        Dim DtAsientoDetalleCostoN As New DataTable

        If GGeneraAsiento Then
            If DtCabezaB.Rows.Count <> 0 Then
                If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & DocumentoAsiento & " AND Documento = " & DtCabezaB.Rows(0).Item("NotaCredito") & ";", Conexion, DtAsientoCabezaB) Then
                    If Not ArmaArchivos() Then Me.Close()
                    Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                End If
                If DtAsientoCabezaB.Rows.Count <> 0 Then
                    If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaB.Rows(0).Item("Asiento") & ";", Conexion, DtAsientoDetalleB) Then
                        If Not ArmaArchivos() Then Me.Close()
                        Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                    End If
                End If
                DtAsientoCabezaB.Rows(0).Delete()
                For Each Row As DataRow In DtAsientoDetalleB.Rows
                    Row.Delete()
                Next
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & DocumentoAsiento & " AND Documento = " & DtCabezaN.Rows(0).Item("NotaCredito") & ";", ConexionN, DtAsientoCabezaN) Then
                    If Not ArmaArchivos() Then Me.Close()
                    Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                End If
                If DtAsientoCabezaN.Rows.Count <> 0 Then
                    If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaN.Rows(0).Item("Asiento") & ";", ConexionN, DtAsientoDetalleN) Then
                        If Not ArmaArchivos() Then Me.Close()
                        Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                    End If
                End If
                DtAsientoCabezaN.Rows(0).Delete()
                For Each Row As DataRow In DtAsientoDetalleN.Rows
                    Row.Delete()
                Next
            End If
            '
            If DtCabezaB.Rows.Count <> 0 Then
                If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & DocumentoAsientoCosto & " AND Documento = " & DtCabezaB.Rows(0).Item("NotaCredito") & ";", Conexion, DtAsientoCabezaCostoB) Then
                    If Not ArmaArchivos() Then Me.Close()
                    Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                End If
                If DtAsientoCabezaCostoB.Rows.Count <> 0 Then
                    If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaCostoB.Rows(0).Item("Asiento") & ";", Conexion, DtAsientoDetalleCostoB) Then
                        If Not ArmaArchivos() Then Me.Close()
                        Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                    End If
                    DtAsientoCabezaCostoB.Rows(0).Delete()
                    For Each Row As DataRow In DtAsientoDetalleCostoB.Rows
                        Row.Delete()
                    Next
                End If
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & DocumentoAsientoCosto & " AND Documento = " & DtCabezaN.Rows(0).Item("NotaCredito") & ";", ConexionN, DtAsientoCabezaCostoN) Then
                    If Not ArmaArchivos() Then Me.Close()
                    Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                End If
                If DtAsientoCabezaCostoN.Rows.Count <> 0 Then
                    If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaCostoN.Rows(0).Item("Asiento") & ";", ConexionN, DtAsientoDetalleCostoN) Then
                        If Not ArmaArchivos() Then Me.Close()
                        Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                    End If
                    DtAsientoCabezaCostoN.Rows(0).Delete()
                    For Each Row As DataRow In DtAsientoDetalleCostoN.Rows
                        Row.Delete()
                    Next
                End If
            End If
        End If

        'Actualiza Pedido.
        Dim DtPedidoDetalle As New DataTable
        If Pedido <> 0 Then
            If Not PreparaPedidoDetalle("A", DtPedidoDetalle) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                If Not ArmaArchivos() Then Me.Close()
                Exit Sub
            End If
        End If

        If MsgBox("Nota Credito se Borrara del Sistema. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            If Not ArmaArchivos() Then Me.Close()
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If DtCabezaB.Rows.Count <> 0 Then
            DtCabezaB.Rows(0).Delete()
            For Each Row As DataRow In DtDetalleB.Rows
                Row.Delete()
            Next
            For Each Row As DataRow In DtLotesB.Rows
                Row.Delete()
            Next
            For Each Row As DataRow In DtPercepciones.Rows
                Row.Delete()
            Next
        End If
        If DtCabezaN.Rows.Count <> 0 Then
            DtCabezaN.Rows(0).Delete()
            For Each Row As DataRow In DtDetalleN.Rows
                Row.Delete()
            Next
            For Each Row As DataRow In DtLotesN.Rows
                Row.Delete()
            Next
        End If

        GModificacionOk = False

        If Not BorraNotaCredito(DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, DtAsientoCabezaCostoB, DtAsientoDetalleCostoB, DtAsientoCabezaCostoN, DtAsientoDetalleCostoN, DtComprobantesOperacionB, DtComprobantesOperacionN, DtPedidoDetalle, DtPuntosDeVenta) Then
            MsgBox("Otro Usuario modifico datos o Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        Else
            MsgBox("Nota fue Borrada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close()
            Exit Sub
        End If

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonComprobantesAImputar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonComprobantesAImputar.Click

        If PNota = 0 Then
            MsgBox("Debe Grabar la Nota de Crédito Previamente.", MsgBoxStyle.Exclamation) : Exit Sub
        End If

        UNComprobanteAImputar.PDtGridCompro = DtGridCompro
        UNComprobanteAImputar.PTipo = 4
        UNComprobanteAImputar.PAbierto = PAbierto
        UNComprobanteAImputar.PTotalConceptos = TextTotalGeneral.Text
        UNComprobanteAImputar.PTipoIva = ComboTipoIva.SelectedValue
        UNComprobanteAImputar.PMoneda = ComboMoneda.SelectedValue
        UNComprobanteAImputar.PCambio = TextCambio.Text
        UNComprobanteAImputar.ShowDialog()
        DtGridCompro = UNComprobanteAImputar.PDtGridCompro
        UNComprobanteAImputar.Dispose()

        GridCompro.Rows.Clear()
        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Asignado") <> 0 Then
                GridCompro.Rows.Add(Row("Operacion"), "", Row("Tipo"), Row("TipoVisible"), Row("Comprobante"), Row("Recibo"), Row("Fecha"), Row("Moneda"), Row("Importe"), Row("Saldo"), Row("Asignado"))
            End If
        Next

    End Sub
    Private Sub ButtonPedirAutorizacionAfip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPedirAutorizacionAfip.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not PAbierto Then
            MsgBox("No Permitido en esta Factura", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If Not DtCabezaB.Rows(0).Item("EsElectronica") Then
            MsgBox("Comprobante No Es Electrónico.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If DtCabezaB.Rows(0).Item("CAE") <> 0 Then
            MsgBox("Autorización Ya Existe.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Dim Cae As String
        Dim FechaCae As String
        Dim Resultado As String
        Dim ImpTotal As String
        Dim CbteTipoAsociado As Integer
        Dim CbteAsociado As Decimal
        Dim ConceptoW As String

        Dim mensaje As String = HallaCaeComprobante("C", 0, TipoFactura, DtCabezaB.Rows(0).Item("NotaCredito"), Cae, FechaCae, Resultado, ImpTotal, CbteTipoAsociado, CbteAsociado, ConceptoW, DtCabezaB.Rows(0).Item("EsFCE"))
        If mensaje = "" And Cae <> "" Then
            If Not GrabaCAE(DtCabezaB.Rows(0).Item("NotaCredito"), CDec(Cae), CInt(FechaCae)) Then
                MsgBox("CAE de la AFIP No se pudo Grabar." + vbCrLf + "Intentelo Nuevamente.")
            Else
                If Not ArmaArchivos() Then Me.Close() : Exit Sub
            End If
            Exit Sub
        End If

        If PideAutorizacionAfip(DtCabezaB, DtDetalleB, DtPercepciones) Then   'Pide y graba CAE a la AFIP.----
            If Not ArmaArchivos() Then Me.Close() : Exit Sub
        End If

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueContable.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub ButtonDatosCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDatosCliente.Click

        If ComboCliente.SelectedValue = 0 Then Exit Sub

        UnDatosEmisor.PEsCliente = True
        UnDatosEmisor.PEmisor = ComboCliente.SelectedValue
        UnDatosEmisor.ShowDialog()

    End Sub
    Private Sub ButtonTextoRecibo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTextoRecibo.Click

        UnTextoParaRecibo.ShowDialog()

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Nota debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsFacturaElectronica Then
            If DtCabezaB.Rows(0).Item("Cae") = 0 Then
                MsgBox("Falta CAE.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If Grid.Rows.Count > GLineasFacturas And Not EsFacturaElectronica Then
            MsgBox("Nora Tiene Mas Articulos Permitidos para Impresión. Operación se CANCELA", MsgBoxStyle.Information)
            Exit Sub
        End If

        If PAbierto Then
            If DtCabezaB.Rows(0).Item("Impreso") Then
                If MsgBox("Nota Ya Fue Impresa. Quiere Re-Imprimir?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
        End If

        ErrorImpresion = False
        Paginas = 0
        If PAbierto Then
            Copias = 2
        Else : Copias = 1
        End If

        Dim print_document As New PrintDocument

        ImprimeNota()

        If ErrorImpresion Then Exit Sub

        If PAbierto Then
            If Not GrabaImpreso(DtCabezaB, Conexion) Then Exit Sub
            DtCabezaB.Rows(0).Item("Impreso") = True
        Else
            If Not GrabaImpreso(DtCabezaN, ConexionN) Then Exit Sub
            DtCabezaN.Rows(0).Item("Impreso") = True
        End If

    End Sub
    Private Sub PictureLupa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupa.Click

        Dim Lista As New List(Of ItemIvaReten)
        If PNota <> 0 Then
            If DtPercepciones.Rows.Count = 0 Then Exit Sub
            For Each Row As DataRow In DtPercepciones.Rows
                Dim Fila As New ItemIvaReten
                Fila.Clave = Row("Percepcion")
                Fila.Importe = Row("Importe")
                Lista.Add(Fila)
            Next
        End If

        If PNota = 0 Then
            SeleccionarVarios.PListaDePercepciones = ListaDePercepciones
        Else
            SeleccionarVarios.PListaDePercepciones = Lista
        End If
        SeleccionarVarios.PTipoNota = 4
        SeleccionarVarios.PEsPercepciones = True
        SeleccionarVarios.ShowDialog()
        SeleccionarVarios.Dispose()

    End Sub
    Private Sub ButtonAsigna_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsigna.Click

        If ListaDeLotes.Count = 0 Then
            MsgBox("FACTURA NO LOTEAD0! Operación Se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Exit Sub
        End If

        AsignacionDevoluciones(Grid, ListaDeLotes)

    End Sub
    Private Sub ButtonBorraComprobantes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorraComprobantes.Click

        If GridCompro.Rows.Count = 0 Then Exit Sub

        If MsgBox("Se Desasignarán todos los comprobantes de la Nota de Créditos. Desea Continuar (Si-No)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub

        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Asignado") <> 0 Then
                Row("Saldo") = Row("Saldo") + Row("Asignado")
                Row("Asignado") = 0
            End If
        Next
        GridCompro.Rows.Clear()

    End Sub
    Private Function HacerModificacion() As Boolean

        Dim DtRecibosDetalle As DataTable
        Dim Nota As Decimal
        Dim ConexionStr As String

        If PAbierto Then
            DtRecibosDetalle = DtDetalleCompB.Copy
            Nota = DtCabezaB.Rows(0).Item("NotaCredito")
            ConexionStr = Conexion
        Else : DtRecibosDetalle = DtDetalleCompN.Copy
            Nota = DtCabezaN.Rows(0).Item("NotaCredito")
            ConexionStr = ConexionN
        End If

        'Actualiza Saldo de Comprobantes Imputados.
        Dim DtFacturasCabezaAux As DataTable = DtFacturasCabeza.Copy
        Dim DtNVLPCabezaAux As DataTable = DtNVLPCabeza.Copy
        Dim DtComprobantesCabezaAux As DataTable = DtComprobantesCabeza.Copy
        Dim DtSaldosInicialesAux As DataTable = DtSaldosIniciales.Copy
        ActualizaComprobantes("M", DtFacturasCabezaAux, DtNVLPCabezaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux)
        '-------------------------------------------

        'Actualiza RecibosDetalle de imputacion.
        Dim RowsBusqueda() As DataRow
        For Each Row1 As DataRow In DtGridCompro.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtRecibosDetalle.Select("TipoComprobante = " & Row1("Tipo") & " AND Comprobante = " & Row1("Comprobante"))
                If RowsBusqueda.Length <> 0 Then
                    If RowsBusqueda(0).Item("Importe") <> Row1.Item("Asignado") Then
                        If Row1.Item("Asignado") = 0 Then
                            RowsBusqueda(0).Delete()
                        Else
                            RowsBusqueda(0).Item("Importe") = Row1.Item("Asignado")
                        End If
                    End If
                Else
                    If Row1.Item("Asignado") <> 0 Then
                        Dim Row As DataRow = DtRecibosDetalle.NewRow()
                        Row("TipoNota") = 4
                        Row("Nota") = Nota
                        Row("TipoComprobante") = Row1("Tipo")
                        Row("Comprobante") = Row1("Comprobante")
                        Row("Importe") = Row1.Item("Asignado")
                        DtRecibosDetalle.Rows.Add(Row)
                    End If
                End If
            End If
        Next


        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoCabezaCostoB As New DataTable
        Dim DtAsientoCabezaCostoN As New DataTable

        If FechaContableAnt <> CDate(TextFechaContable.Text) Then
            If GGeneraAsiento Then
                If Not ModificaAsientosCabeza(DtAsientoCabezaB, DtAsientoCabezaN, DtAsientoCabezaCostoB, DtAsientoCabezaCostoN) Then
                    MsgBox("Error Al Modigicar Asientos Contables. OPERACION se Cancela.") : Return False
                End If
            End If
            If DtCabezaB.Rows.Count <> 0 Then
                DtCabezaB.Rows(0).Item("FechaContable") = CDate(TextFechaContable.Text)
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                DtCabezaN.Rows(0).Item("FechaContable") = CDate(TextFechaContable.Text)
            End If
        End If

        Dim NumeroW As Integer

        NumeroW = ModificaNotaCredito(DtRecibosDetalle, DtFacturasCabezaAux, DtNVLPCabezaAux, DtComprobantesCabezaAux, DtSaldosInicialesAux, ConexionStr, DtAsientoCabezaB, DtAsientoCabezaN, DtAsientoCabezaCostoB, DtAsientoCabezaCostoN,
                                      DtCabezaB, DtCabezaN)

        If NumeroW < 0 Then
            MsgBox("ERROR: En base de datos. Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = 0 Then
            MsgBox("ERROR: Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW > 0 Then
            MsgBox("Modificación Realizada Exitosamente. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Sub ActualizaComprobantes(ByVal Funcion As String, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtSaldosInicialesAux As DataTable)

        Dim RowsBusqueda() As DataRow

        'Actualiza Saldo de Comprobantes Imputados.

        If Funcion = "M" Then
            For Each Row1 As DataRow In DtGridCompro.Rows
                If Row1.RowState <> DataRowState.Deleted Then
                    If Row1("Saldo") <> Row1("SaldoAnt") Then
                        Select Case Row1("Tipo")
                            Case 2
                                RowsBusqueda = DtFacturasCabezaAux.Select("Factura = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                            Case 800
                                RowsBusqueda = DtNVLPCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                            Case 900
                                RowsBusqueda = DtSaldosInicialesAux.Select("Clave = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                            Case Else
                                RowsBusqueda = DtComprobantesCabezaAux.Select("TipoNota = " & Row1("Tipo") & " AND Nota = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                        End Select
                    End If
                End If
            Next
        End If

        If Funcion = "B" Then
            Dim Importe As Double = 0
            For Each Row1 As DataRow In DtGridCompro.Rows
                If Row1.RowState <> DataRowState.Deleted Then
                    If Row1.Item("Asignado") <> 0 Then
                        Select Case Row1("Tipo")
                            Case 2
                                RowsBusqueda = DtFacturasCabezaAux.Select("Factura = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                            Case 800
                                RowsBusqueda = DtNVLPCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                            Case 900
                                RowsBusqueda = DtSaldosInicialesAux.Select("Clave = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo") + Row1.Item("Asignado"))
                            Case Else
                                RowsBusqueda = DtComprobantesCabezaAux.Select("TipoNota = " & Row1("Tipo") & " AND Nota = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = CDec(RowsBusqueda(0).Item("Saldo")) + Row1.Item("Asignado")
                        End Select
                    End If
                End If
            Next
        End If

    End Sub
    Private Function PreparaPedidoDetalle(ByVal Funcion As String, ByRef DtPedidoDetalle As DataTable) As Boolean

        Dim ListaParaPedido As New List(Of ItemPedido)

        For Each Row As DataRow In DtGrid.Rows
            If Row("Cantidad") <> 0 Then
                Dim Fila As New ItemPedido
                Fila.Articulo = Row("Articulo")
                Fila.Cantidad = Row("Cantidad")
                ListaParaPedido.Add(Fila)
            End If
        Next
        If ListaParaPedido.Count <> 0 Then
            If Not ActualizaPedido(Pedido, Funcion, DtPedidoDetalle, ListaParaPedido) Then Return False
        End If

        ListaParaPedido = Nothing

        Return True

    End Function
    Private Function HallaUltimaNumeracionW(ByVal TipoIva As Integer, ByVal PuntoVenta As Integer, ByVal ConexionStr As String) As Double

        Dim Patron As String = TipoIva & Format(PuntoVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(NotaCredito) FROM NotasCreditoCabeza WHERE CAST(CAST(NotasCreditoCabeza.NotaCredito AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo)
                    Else : Return CDbl(TipoIva & Format(PuntoVenta, "0000") & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function ImprimeNota() As Boolean

        Listado = ArmaListadoNotaCredito(Grid, GLineasFacturas, PAbierto)
        ContadorLineasListado = 0
        ContadorCopias = 1
        ContadorPaginas = 1

        TotalPaginas = Listado.Count / GLineasFacturas
        If Listado.Count / GLineasFacturas - TotalPaginas > 0 Then
            TotalPaginas = TotalPaginas + 1
        End If

        Dim print_document As New PrintDocument

        AddHandler print_document.PrintPage, AddressOf Print_Nota

        print_document.Print()

    End Function
    Private Sub Print_Nota(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        PrintUnaPagina(e, Listado, ContadorLineasListado)
        If ErrorImpresion Then e.HasMorePages = False : Exit Sub
        '
        If ContadorLineasListado < Listado.Count Then
            ContadorPaginas = ContadorPaginas + 1
            e.HasMorePages = True
        Else
            If ContadorCopias < Copias Then
                ContadorLineasListado = 0
                ContadorCopias = ContadorCopias + 1
                ContadorPaginas = 1
                e.HasMorePages = True
            Else : e.HasMorePages = False
            End If
        End If

    End Sub
    Private Sub PrintUnaPagina(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal lista As List(Of ItemListado), ByRef ContadorLineasListado As Integer)

        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim PrintFont As System.Drawing.Font
        Dim SaltoLinea As Integer = 4
        Dim FechaEntregaImp As String = ""
        Dim LineasPorPagina As Integer = GLineasFacturas
        Dim Contador As Integer = 0
        Dim Ancho As Integer = 185
        Dim Alto As Integer = 125
        Dim ToTalNetoLista As Decimal = 0
        Dim ToTalDescuento As Decimal = 0

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        If EsFacturaElectronica And PAbierto Then
            Dim NombreEmpres As String = GNombreEmpresa '"Patagonia Sunrise S.R.L."
            Dim CuitEmpresa As String = GCuitEmpresa    '"30-12345678-5"
            Dim TipoComprobante As Integer = 3             '1. factura 2. debito 3. credito
            Dim Direccion1 As String = GDireccion1      '"Nave S-2 Puesto 34 - Mercado Central de Buenos Aires"
            Dim Direccion2 As String = GDireccion2      '"Autopista Ricchieri y Boulongne Sur Mer"
            Dim Direccion3 As String = GDireccion3      '"C.P.(1771) Villa Celina Pcia. de Buenos Aires"
            Dim LetraComprobante As String = LetraTipoIva(TipoFactura)
            Dim IngBruto As String = GIngBruto     '"Conv.Mult. Nª 901-898989-6"
            Dim FechaInicio As String = GFechaInicio
            '
            MascaraImpresion.ImprimeMascara(NombreEmpres, CuitEmpresa, TipoComprobante, Direccion1, Direccion2, Direccion3, LetraComprobante, GCondicionIvaEmpresa, IngBruto, FechaInicio, "C:\XML Afip\", e, EsFCE, 2)
            Texto = NumeroEditado(Strings.Right(DtCabezaB.Rows(0).Item("NotaCredito").ToString, 12))
            PrintFont = New Font("Arial", 16)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 135, MTop + 6)
        End If

        Dim Str1 As String = ""
        If EsFCE Then
            Str1 = "miPyMEs (FCE)"
            Texto = Str1
            PrintFont = New Font("Arial", 12)
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 115, MTop - 1)
        End If

        Str1 = ""
        Select Case ContadorCopias
            Case 1
                Str1 = "ORIGINAL"
            Case 2
                Str1 = "DUPLICADO"
            Case 3
                Str1 = "TRIPLICADO"
        End Select
        Texto = Str1
        PrintFont = New Font("Arial", 10)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 165, MTop)
        '
        PrintFont = New Font("Courier New", 12)
        Texto = "Pagina: " & ContadorPaginas & "/" & TotalPaginas
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + Ancho - 40, MTop - 17)
        '

        PrintFont = New Font("Courier New", 11)
        x = MIzq + 135 : y = MTop

        Texto = TextFechaContable.Text
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y + 15)

        x = MIzq : y = MTop + 42

        Try
            'Titulos.
            If PAbierto Then
                Texto = "CLIENTE    : " & ComboCliente.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                If Numero <> 0 Then
                    Texto = "DOMICILIO  : " & Calle & " No: " & Numero
                Else
                    Texto = "DOMICILIO  : " & Calle
                End If
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "LOCALIDAD  : " & Localidad & " " & Provincia
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "DOM.ENTREGA: "
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "CUIT       : " & TextCuit.Text & " " & ComboTipoIva.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                'Condicion de venta.
                Texto = ""
                If FormaPago = 1 Then
                    Texto = "CONDICION DE VENTA: Contado"
                End If
                If FormaPago = 2 Then
                    Texto = "CONDICION DE VENTA: Cuenta Corriente"
                End If
                If FormaPago = 3 Then
                    Texto = "CONDICION DE VENTA: Mixta"
                End If
                If FormaPago = 4 Then
                    Texto = "CONDICION DE VENTA: Contado Efectivo"
                End If
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                Texto = "REMITO: " & Format(Remito, "0000-00000000")
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 400, y)
                y = y + SaltoLinea
                Texto = "FACTURA    : " & NumeroEditado(DtCabezaFacturaB.Rows(0).Item("Factura")) & "   SUCURSAL: " & ComboSucursal.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            Else
                Texto = "CLIENTE    : " & ComboCliente.Text & "                Nro.: " & LabelNota.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            End If

            PrintFont = New Font("Courier New", 11)

            'Grafica -Rectangulo----------------------------------------------------------------------
            x = MIzq
            y = MTop + 72

            Dim LineaCantidad As Integer = x + 115
            Dim LineaUnitario As Integer = x + 150
            Dim LineaImporte As Integer = x + Ancho
            Dim Longi As Integer
            Dim Xq As Integer
            Dim Yq As Integer

            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x + 90, y, x + 90, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCantidad, y, LineaCantidad, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaUnitario, y, LineaUnitario, y + Alto)
            'Titulos de descripcion.
            Texto = "ARTICULO"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 2, y + 2)
            Texto = "CANTIDAD"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi - 2
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "PRECIO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaUnitario - Longi - 7
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE TOTAL"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi - 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)
            Yq = y - SaltoLinea
            While Contador < LineasPorPagina And ContadorLineasListado < lista.Count
                Yq = Yq + SaltoLinea
                'Imprime Articulo.
                Texto = HallaNombreArticulo(lista.Item(ContadorLineasListado).Articulo)
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime cantidad.
                Texto = lista.Item(ContadorLineasListado).Cantidad & lista.Item(ContadorLineasListado).Medida
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaCantidad - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Unitario.
                Dim Precio As Decimal = 0
                Precio = lista.Item(ContadorLineasListado).PrecioLista
                Texto = FormatNumber(Precio, 3) & lista.Item(ContadorLineasListado).Medida
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaUnitario - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Importe.
                Dim Neto As Decimal = CalculaNeto(lista.Item(ContadorLineasListado).Cantidad, Precio)
                Texto = FormatNumber(Neto, GDecimales)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Contador = Contador + 1
                ContadorLineasListado = ContadorLineasListado + 1
            End While
            If ContadorLineasListado = lista.Count Then
                FinalDePagina(e, MTop, SaltoLinea, LineaCantidad, LineaImporte, Xq, Yq, x, Alto)
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
        End Try

    End Sub
    Private Sub FinalDePagina(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal MTop As Integer, ByVal SaltoLinea As Integer, ByVal LineaCantidad As Integer, ByVal LineaImporte As Integer, ByVal Xq As Integer, ByVal Yq As Integer, ByVal x As Integer, ByVal Alto As Integer)

        Dim PrintFont As System.Drawing.Font
        Dim Texto As String
        Dim Longi As Integer

        Yq = MTop + 72 + Alto + 2
        PrintFont = New Font("Courier New", 10)

        Try
            If PAbierto Then
                Texto = GNombreEmpresa & " " & LabelNota.Text
            Else
                Texto = LabelNota.Text
            End If
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, Yq)
            'Totales
            PrintFont = New Font("Courier New", 11)

            Dim NetoPrecioLista As Decimal
            Dim Descuento As Decimal
            Dim StrNetoPrecioLista As String
            Dim StrDescuento As String
            StrNetoPrecioLista = TextNetoLista.Text
            StrDescuento = TextDescuento.Text
            Texto = "Neto"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 10, Yq)
            Texto = StrNetoPrecioLista
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Yq = Yq + SaltoLinea
            'Descuento
            Texto = "Descuento"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 10, Yq)
            Texto = StrDescuento
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            'Neto
            Yq = Yq + SaltoLinea
            Texto = "Neto"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 10, Yq)
            Texto = TextTotalNeto.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            '
            Dim ListaIva As New List(Of ItemIva)
            ArmaListaImportesIva(ListaIva)
            'Iva.
            For Each Fila As ItemIva In ListaIva
                Yq = Yq + SaltoLinea
                Texto = "IVA. " & FormatNumber(Fila.Iva, GDecimales)
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 10, Yq)
                Texto = FormatNumber(Fila.Importe, GDecimales)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Next
            ListaIva = Nothing
            'Percepciones.
            If DtPercepciones.Rows.Count <> 0 Then
                Yq = Yq + SaltoLinea
                Texto = HallaNombreRetencion(DtPercepciones.Rows(0).Item("Percepcion")) 'hay una sola.
                e.Graphics.DrawString(Texto, New Font("Courier New", 8), Brushes.Black, LineaCantidad + 10, Yq + 0.5)
                Texto = TextPercepcion.Text
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            End If
            'Total
            Yq = Yq + SaltoLinea
            Texto = "Total"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 10, Yq)
            Texto = TextTotalGeneral.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            'Imprime Leyenda.
            Yq = MTop + 204
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox1.Text, PrintFont, Brushes.Black, x, Yq)
            Yq = Yq + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox2.Text, PrintFont, Brushes.Black, x, Yq)
            Yq = Yq + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox3.Text, PrintFont, Brushes.Black, x, Yq)
            Yq = Yq + SaltoLinea
            e.Graphics.DrawString(UnTextoParaRecibo.TextBox4.Text, PrintFont, Brushes.Black, x, Yq)
            Yq = Yq + SaltoLinea
            'Imprime Cae ------------------
            If DtCabezaB.Rows.Count <> 0 Then
                If DtCabezaB.Rows(0).Item("Cae") <> 0 Then
                    PrintFont = New Font("Courier New", 12)
                    Yq = 265
                    e.Graphics.DrawString(LabelCAE.Text, PrintFont, Brushes.Black, x + 25, Yq)
                    '----------------------------------Codigo de Barra anulado. Lo reemplaza QR. ----------------------------
                    '  Dim CodigoBarra As String = Format(CuitNumerico(GCuitEmpresa), "00000000000") & Format(HallaTipo("C", TipoFactura, 0), "00") & Format(CInt(Strings.Mid(DtCabezaB.Rows(0).Item("NotaCredito").ToString, 2, 4)), "0000") & Format(DtCabezaB.Rows(0).Item("Cae"), "00000000000000") & Format(DtCabezaB.Rows(0).Item("FechaCae"), "00000000")
                    '    Dim aa As New DllVarias
                    '    CodigoBarra = aa.CombierteTextoAInterleaved2Of5(CodigoBarra, True)
                    '    Dim Tamanio As Integer = 18
                    '    Dim fuente As Font
                    '    fuente = CustomFont.GetInstance(Tamanio, FontStyle.Regular)
                    '    e.Graphics.DrawString(CodigoBarra, fuente, Brushes.Black, x, Yq + 7)
                    '------------------------------------------------------------------------------
                    ImprimeQR(x - 4, Yq - 2, 22, 22, e)
                    '------------------------------------------------------------------------------------------------------------------------------------
                End If
            End If
            'Cartel para montributo ley 27.618
            If ComboTipoIva.SelectedValue = 6 And (TipoFactura = 1 Or TipoFactura = 5) And PAbierto Then
                PrintFont = New Font("Courier New", 8)
                Texto = "El crédito fiscal discriminado en el presente comprobante,sólo podra ser"
                Yq = Yq + 2 * SaltoLinea
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 25, Yq)
                Yq = Yq + SaltoLinea
                Texto = "computado a efectos del Régimen de Sostenimiento e Inclusión Fiscal para"
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 25, Yq)
                Yq = Yq + SaltoLinea
                Texto = "Pequeños Contribuyentes de la ley 27.618"
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 25, Yq)
            End If
            '-------------------------------------------------------------
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
        End Try

    End Sub
    Private Sub ImprimeQR(ByVal X As Long, ByVal Y As Long, ByVal Ancho As Long, ByVal Alto As Long, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim ImageQR As Image = ArmaDatoParaQR(2, DtCabezaB.Rows(0).Item("FechaContable"), DtCabezaB.Rows(0).Item("NotaCredito"), DtCabezaB.Rows(0).Item("EsFCE"), DtCabezaB.Rows(0).Item("Cae"), DtCabezaB.Rows(0).Item("Importe"), TextCuit.Text, DocTipo, DocNro, ComboTipoIva.SelectedValue)
        e.Graphics.DrawImage(ImageQR, X, Y, Ancho, Alto)

    End Sub
    Private Function PideAutorizacionAfip(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtPercepcionesW As DataTable) As Boolean

        If DtCabezaW.Rows.Count = 0 Then Return True
        If Not DtCabezaW.Rows(0).Item("EsElectronica") Then Return True

        Dim CAE As String = ""
        Dim FechaCae As String = ""
        Dim Concepto As Integer = 0
        Dim FchServDesde As Date = DateAdd(DateInterval.Day, 30, CDate(TextFechaContable.Text))
        Dim FchServHasta As Date = DateAdd(DateInterval.Day, 30, CDate(TextFechaContable.Text))
        Dim FchVtoPago As Date = DateAdd(DateInterval.Day, 30, CDate(TextFechaContable.Text))
        If EsServicios Then
            Concepto = 2
        Else
            Concepto = 1
        End If
        Dim CbteTipoAsociado As Integer = 0
        Dim CbteAsociado As Decimal = 0
        Dim CancelarGrabar As Boolean
        Dim TipoArticulo As Integer
        If EsSecos Or EsServicios Then
            TipoArticulo = 2
        Else
            TipoArticulo = 1
        End If

        Dim DatosParaAfip As New ItemDatosParaAFIP  'Pone Datos de la factura que se encia a la AFIP como ComprobanteAsociado.

        Dim NumeroLetra As String
        Dim Nro As String
        Dim PtoVta As String
        DescomponeNumeroComprobante(DtCabezaW.Rows(0).Item("Factura"), NumeroLetra, PtoVta, Nro)
        DatosParaAfip.EsFCE = EsFCE
        DatosParaAfip.Tipo = 1
        DatosParaAfip.TipoIva = NumeroLetra
        DatosParaAfip.NroCbte = Nro
        DatosParaAfip.PtoVta = PtoVta
        DatosParaAfip.Cbte = DtCabezaW.Rows(0).Item("Factura")
        DatosParaAfip.Cuit = CuitNumerico(GCuitEmpresa)
        DatosParaAfip.FechaCbte = DtCabezaFacturaB.Rows(0).Item("FechaContable")
        DatosParaAfip.EsAnulacion = "N"   'N- No es anulacion. S- Es anulacion.   (Para anular FCE rechazada por el cliente.)
        If EsFCE Then
            If CheckBoxPorAnulacion.Checked = True Then
                DatosParaAfip.EsAnulacion = "S"
            End If
        End If
        DatosParaAfip.InscripcionAfip = ComboTipoIva.SelectedValue  'Responsable insc, montributo, etc.
        DatosParaAfip.DocTipo = DocTipo    'Tipo Documento Consumidor Final.
        DatosParaAfip.DocNro = DocNro      'Numero Documento Consumidor Final.

        Dim Mensaje = Autorizar("C", DtCabezaW, DtDetalleW, DtPercepcionesW, FchServDesde, FchServHasta, FchVtoPago, TipoFactura, DtCabezaW.Rows(0).Item("NotaCredito"), TextCuit.Text, Concepto, TipoArticulo, CAE, FechaCae, CbteTipoAsociado, CbteAsociado, CancelarGrabar, DatosParaAfip)
        If CAE = "" Then
            MsgBox(Mensaje + vbCrLf + "Deberá Pedir Autorización AFIP.")
            Return False
        End If

        If CAE <> "" Then
            If Not GrabaCAE(DtCabezaW.Rows(0).Item("NotaCredito"), CDec(CAE), CInt(FechaCae)) Then
                MsgBox("CAE de la AFIP No se pudo Grabar." + vbCrLf + "Deberá Pedir Autorización AFIP.")
                Return False
            End If
        End If

        Return True

    End Function
    Public Function GrabaCAE(ByVal NotaCredito As Decimal, ByVal Cae As Decimal, ByVal FechaCae As Integer) As Boolean

        Dim Sql As String = "UPDATE NotasCreditoCabeza Set CAE = " & Cae & ",FechaCae = " & FechaCae & _
            " WHERE NotaCredito = " & NotaCredito & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                If CInt(Cmd.ExecuteNonQuery()) = 0 Then Return False
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos al Grabar CAE en NotasCreditoCabeza." & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End Try

        Return True

    End Function
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PNota = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = DocumentoAsiento
        If PAbierto Then
            ListaAsientos.PDocumentoB = DtCabezaB.Rows(0).Item("NotaCredito")
        Else
            ListaAsientos.PDocumentoN = DtCabezaN.Rows(0).Item("NotaCredito")
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonRelacionada_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRelacionada.Click

        PAbierto = Not PAbierto

        If PNota = 0 Then
            If DtCabezaFacturaB.Rows.Count <> 0 And DtCabezaFacturaN.Rows.Count = 0 Then
                LabelFactura.Text = NumeroEditado(DtCabezaFacturaB.Rows(0).Item("Factura"))
            End If
            If DtCabezaFacturaB.Rows.Count = 0 And DtCabezaFacturaN.Rows.Count <> 0 Then
                LabelFactura.Text = NumeroEditado(DtCabezaFacturaN.Rows(0).Item("Factura"))
            End If
            If DtCabezaFacturaB.Rows.Count <> 0 And DtCabezaFacturaN.Rows.Count <> 0 Then
                LabelFactura.Text = NumeroEditado(DtCabezaFacturaB.Rows(0).Item("Factura")) & " / " & NumeroEditado(DtCabezaFacturaN.Rows(0).Item("Factura"))
            End If
            LabelNota.Text = NumeroEditado(UltimoNumero) & " / " & NumeroEditado(0)
            PictureCandado.Image = Nothing
        End If

        If PNota <> 0 Then
            If PAbierto Then
                LabelFactura.Text = NumeroEditado(DtCabezaFacturaB.Rows(0).Item("Factura"))
                LabelNota.Text = NumeroEditado(DtCabezaB.Rows(0).Item("NotaCredito"))
                PictureCandado.Image = ImageList1.Images.Item("Abierto")
            End If
            If Not PAbierto Then
                LabelFactura.Text = NumeroEditado(DtCabezaFacturaN.Rows(0).Item("Factura"))
                LabelNota.Text = NumeroEditado(DtCabezaN.Rows(0).Item("NotaCredito"))
                PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            End If
        End If

        CalculaTotal()

        If PNota <> 0 Then
            EsFCE = False
            LabelFCE.Visible = False
            If PAbierto Then
                If DtCabezaB.Rows.Count <> 0 Then EsFCE = DtCabezaB.Rows(0).Item("EsFCE")
            End If
            If EsFCE Then LabelFCE.Visible = True
        End If

        If PNota <> 0 Then
            If Not ArmaComprobantesAImputar() Then Exit Sub
        End If

        'Muestra DtGedriCompro con Imputaciones.----
        If PAbierto Then
            MuestraImputaciones(DtDetalleCompB)
        Else
            MuestraImputaciones(DtDetalleCompN)
        End If
        '---------------------------------------------

    End Sub
    Private Sub PictureAlmanaqueFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueFecha.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            DateTime1.Text = ""
        Else : DateTime1.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub TextCambio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCambio.KeyPress

        EsNumerico(e.KeyChar, TextCambio.Text, 3)

    End Sub
    Private Sub TextCambio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextCambio.Validating

        If TextCambio.Text = "" Then Exit Sub

        If CDbl(TextCambio.Text) = 0 Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextCambio.Text = ""
            TextCambio.Focus()
        End If

    End Sub
    Private Function ArmaArchivos() As Boolean   'ArmaArchivos

        DtCabezaB = New DataTable
        DtCabezaN = New DataTable
        DtDetalleB = New DataTable
        DtDetalleN = New DataTable
        DtDetalleCompB = New DataTable
        DtDetalleCompN = New DataTable
        DtLotesB = New DataTable
        DtLotesN = New DataTable
        DtCabezaFacturaB = New DataTable
        DtCabezaFacturaN = New DataTable
        DtDetalleFacturaB = New DataTable
        DtDetalleFacturaN = New DataTable
        DtLotesFacturaB = New DataTable
        DtLotesFacturaN = New DataTable

        CreaDtGrid()
        CreaDtGridCompro()

        NotaRelacionada = 0

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PNota <> 0 Then
            If PAbierto Then
                If Not LeerArchivosNota(PNota, DtCabezaB, DtDetalleB, DtLotesB, DtDetalleCompB, Conexion) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
                FechaContableAnt = DtCabezaB.Rows(0).Item("FechaContable")
                If DtCabezaB.Rows(0).Item("Rel") Then
                    NotaRelacionada = HallaNotaRelacionada(PNota)
                    If NotaRelacionada < 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("Error Base de datos al Leer Notas de Credito.")
                        Return False
                    End If
                    If Not LeerArchivosNota(NotaRelacionada, DtCabezaN, DtDetalleN, DtLotesN, DtDetalleCompN, ConexionN) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
                Else
                    DtCabezaN = DtCabezaB.Clone
                    DtDetalleN = DtDetalleB.Clone
                    DtDetalleCompN = DtDetalleCompB.Clone
                    DtLotesN = DtLotesB.Clone
                End If
                If DtCabezaB.Rows(0).Item("Cae") <> 0 Then
                    LabelCAE.Text = "Autorización AFIP  CAE: " & DtCabezaB.Rows(0).Item("Cae") & "  Vto: " & Strings.Right(DtCabezaB.Rows(0).Item("FechaCae"), 2) & "/" & Strings.Mid(DtCabezaB.Rows(0).Item("FechaCae"), 5, 2) & "/" & Strings.Left(DtCabezaB.Rows(0).Item("FechaCae"), 4) : LabelCAE.Visible = True
                Else
                    LabelCAE.Visible = False
                End If
            Else
                If Not LeerArchivosNota(PNota, DtCabezaN, DtDetalleN, DtLotesN, DtDetalleCompN, ConexionN) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
                FechaContableAnt = DtCabezaN.Rows(0).Item("FechaContable")
                If DtCabezaN.Rows(0).Item("Rel") Then
                    NotaRelacionada = DtCabezaN.Rows(0).Item("Relacionada")
                    If Not LeerArchivosNota(NotaRelacionada, DtCabezaB, DtDetalleB, DtLotesB, DtDetalleCompB, Conexion) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
                Else
                    DtCabezaB = DtCabezaN.Clone
                    DtDetalleB = DtDetalleN.Clone
                    DtDetalleCompB = DtDetalleCompN.Clone
                    DtLotesB = DtLotesN.Clone
                End If
            End If
        Else
            If Not LeerArchivosNota(0, DtCabezaB, DtDetalleB, DtLotesB, DtDetalleCompB, Conexion) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
            DtCabezaN = DtCabezaB.Clone
            DtDetalleN = DtDetalleB.Clone
            DtDetalleCompN = DtDetalleCompB.Clone
            DtLotesN = DtLotesB.Clone
        End If

        Dim FacturaWB As Double = 0
        Dim FacturaWN As Double = 0

        If DtCabezaN.Rows.Count <> 0 Then FacturaWN = DtCabezaN.Rows(0).Item("Factura") : TextFechaContable.Text = Format(DtCabezaN.Rows(0).Item("FechaContable"), "dd/MM/yyyy")
        If DtCabezaB.Rows.Count <> 0 Then FacturaWB = DtCabezaB.Rows(0).Item("Factura") : TextFechaContable.Text = Format(DtCabezaB.Rows(0).Item("FechaContable"), "dd/MM/yyyy")
        If PNota = 0 Then
            FacturaWB = PFacturaB
            FacturaWN = PFacturaN
        End If

        'Arma Lista de Percpciones realizadas.--------------------------------------- 
        DtPercepciones = New DataTable
        TotalPercepciones = 0
        Dim Sql As String
        If PNota = 0 Then
            Sql = "SELECT * FROM RecibosPercepciones WHERE TipoComprobante = 0 AND Comprobante = " & 0 & ";"
            If Not Tablas.Read(Sql, Conexion, DtPercepciones) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        End If
        If PNota <> 0 And PAbierto Then
            Sql = "SELECT * FROM Recibospercepciones WHERE TipoComprobante = 4 AND Comprobante = " & PNota & ";"
            If Not Tablas.Read(Sql, Conexion, DtPercepciones) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        End If

        If Not LeerArchivosFactura(DtCabezaFacturaB, DtDetalleFacturaB, DtLotesFacturaB, FacturaWB, DtCabezaFacturaN, DtDetalleFacturaN, DtLotesFacturaN, FacturaWN) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False

        Dim Cliente As Integer
        Dim Sucursal As Integer
        Dim DesdeZ As Decimal
        Dim HastaZ As Decimal
        Pedido = 0

        EsFacturaElectronica = False
        If DtCabezaFacturaB.Rows.Count <> 0 Then
            Cliente = DtCabezaFacturaB.Rows(0).Item("Cliente")
            FormaPago = DtCabezaFacturaB.Rows(0).Item("FormaPago")
            Remito = DtCabezaFacturaB.Rows(0).Item("Remito")
            EsServicios = DtCabezaFacturaB.Rows(0).Item("EsServicios")
            EsSecos = DtCabezaFacturaB.Rows(0).Item("EsSecos")
            EsFacturaElectronica = DtCabezaFacturaB.Rows(0).Item("EsElectronica")
            Sucursal = DtCabezaFacturaB.Rows(0).Item("Sucursal")
            Pedido = DtCabezaFacturaB.Rows(0).Item("Pedido")
            EsZ = DtCabezaFacturaB.Rows(0).Item("EsZ")
            EsFCE = DtCabezaFacturaB.Rows(0).Item("EsFCE")
            DesdeZ = DtCabezaFacturaB.Rows(0).Item("ComprobanteDesde")
            HastaZ = DtCabezaFacturaB.Rows(0).Item("ComprobanteHasta")
        Else : Cliente = DtCabezaFacturaN.Rows(0).Item("Cliente")
            FormaPago = DtCabezaFacturaN.Rows(0).Item("FormaPago")
            Remito = DtCabezaFacturaN.Rows(0).Item("Remito")
            EsServicios = DtCabezaFacturaN.Rows(0).Item("EsServicios")
            EsSecos = DtCabezaFacturaN.Rows(0).Item("EsSecos")
            Sucursal = DtCabezaFacturaN.Rows(0).Item("Sucursal")
            Pedido = DtCabezaFacturaN.Rows(0).Item("Pedido")
            EsZ = DtCabezaFacturaN.Rows(0).Item("EsZ")
            DesdeZ = DtCabezaFacturaN.Rows(0).Item("ComprobanteDesde")
            HastaZ = DtCabezaFacturaN.Rows(0).Item("ComprobanteHasta")
        End If

        If EsFCE Then
            LabelFCE.Text = "Factura de Crédito MiPyMEs(FCE)"
            CheckBoxPorAnulacion.Visible = True
            CheckBoxPorAnulacion.Checked = False
        Else
            LabelFCE.Text = ""
            CheckBoxPorAnulacion.Visible = False
            CheckBoxPorAnulacion.Checked = False
        End If

        If Not LlenaDatosCliente(Cliente) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False

        ComboSucursal.DataSource = Nothing
        ComboSucursal.DataSource = Tablas.Leer("SELECT Nombre,Clave FROM SucursalesClientes WHERE Cliente = " & Cliente & ";")
        Dim Row2 As DataRow = ComboSucursal.DataSource.NewRow
        Row2("Nombre") = ""
        Row2("Clave") = 0
        ComboSucursal.DataSource.Rows.Add(Row2)
        ComboSucursal.DisplayMember = "Nombre"
        ComboSucursal.ValueMember = "Clave"
        ComboSucursal.SelectedValue = Sucursal
        With ComboSucursal
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If DtCabezaB.Rows.Count <> 0 Then
            ArmaGridConNota(DtDetalleB)
        Else
            If DtCabezaN.Rows.Count <> 0 Then
                ArmaGridConNota(DtDetalleN)
            Else
                If DtCabezaFacturaB.Rows.Count <> 0 Then
                    ArmaGridConFactura(DtDetalleFacturaB)
                Else : ArmaGridConFactura(DtDetalleFacturaN)
                End If
            End If
        End If

        If DtCabezaFacturaB.Rows.Count <> 0 Then
            TipoFactura = Strings.Left(DtCabezaFacturaB.Rows(0).Item("Factura"), 1)
        Else
            TipoFactura = Strings.Left(DtCabezaFacturaN.Rows(0).Item("Factura"), 1)
        End If

        TextTipoFactura.Text = LetraTipoIva(TipoFactura)

        If Not EsZ Then
            If EsFCE Then
                GPuntoDeVenta = HallaPuntoVentaFce()
            Else
                GPuntoDeVenta = HallaPuntoVentaSegunTipo(7, TipoFactura)
            End If
        Else
            Dim NumeroFactura As Decimal
            If DtCabezaFacturaB.Rows.Count <> 0 Then
                NumeroFactura = DtCabezaFacturaB.Rows(0).Item("Factura")
                GPuntoDeVenta = Strings.Mid(DtCabezaFacturaB.Rows(0).Item("Factura"), 2, 4)
            Else
                GPuntoDeVenta = Strings.Mid(DtCabezaFacturaN.Rows(0).Item("Factura"), 2, 4)
                NumeroFactura = DtCabezaFacturaN.Rows(0).Item("Factura")
            End If
            EsTicketA = False
            EsTicketB = False
            EsTicket = False
            Select Case Strings.Left(NumeroFactura, 1)
                Case 1
                    EsTicketA = True
                Case 2
                    EsTicketB = True
                Case 9
                    EsTicket = True
            End Select
        End If

        If GPuntoDeVenta = 0 Then
            MsgBox("No tiene Definido Punto de Venta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Not ExistePuntoDeVenta(GPuntoDeVenta) Then
            MsgBox("Punto de Venta " & GPuntoDeVenta & " No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Not EsZ Then
            If EsPuntoDeVentaZ(GPuntoDeVenta) Then
                MsgBox("Punto de Venta Operador " & GPuntoDeVenta & " Es Exclusivo Para Ticket Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        Else
            If Not EsPuntoDeVentaZ(GPuntoDeVenta) Then
                MsgBox("Punto de Venta " & GPuntoDeVenta & " No es Para Ticket Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If
        Dim EsFCEW As Boolean = EsPuntoDeVentaCFE(GPuntoDeVenta)
        If EsFCE And Not EsFCEW Then
            MsgBox("Punto de Venta del Operador " & Format(GPuntoDeVenta, "0000") & " No es para FCE.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Not EsFCE And EsFCEW Then
            MsgBox("Punto de Venta del Operador " & Format(GPuntoDeVenta, "0000") & " Reservado para FCE.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        LabelPuntodeVenta.Text = "Punto de Venta " & Format(GPuntoDeVenta, "0000")

        If Not EsZ Then
            Panel10.Visible = False
        Else
            If TipoFactura = 9 Then LabelTicket.Text = "Ticket "
            If TipoFactura = 1 Then LabelTicket.Text = "Ticket A"
            If TipoFactura = 2 Then LabelTicket.Text = "Ticket B"
            If TipoFactura = 3 Then LabelTicket.Text = "Ticket C"
            If TipoFactura = 5 Then LabelTicket.Text = "Ticket M"
            LabelTicket.Text = LabelTicket.Text & "   Comp. Desde  " & DesdeZ & "   Hasta  " & HastaZ
            Panel10.Visible = True
        End If

        If PNota = 0 And Not EsZ Then
            If DtCabezaFacturaB.Rows.Count <> 0 Then
                UltimoNumero = UltimaNumeracionNCredito(Strings.Left(DtCabezaFacturaB.Rows(0).Item("Factura"), 1), GPuntoDeVenta)
                If UltimoNumero <= 0 Then
                    MsgBox("ERROR Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                LabelNota.Text = Format(Val(Strings.Right(UltimoNumero, 12)), "0000-00000000")
            End If
        End If

        If PNota = 0 Then
            ListaDeLotes = New List(Of FilaAsignacion)
            If PFacturaB <> 0 Then
                For Each Row As DataRow In DtLotesFacturaB.Rows
                    Dim Fila As New FilaAsignacion
                    Fila.Indice = Row("Indice")
                    Fila.Lote = Row("Lote")
                    Fila.Secuencia = Row("Secuencia")
                    Fila.Deposito = Row("Deposito")
                    Fila.Operacion = Row("Operacion")
                    Fila.Asignado = Row("Cantidad")
                    ListaDeLotes.Add(Fila)
                Next
            Else
                For Each Row As DataRow In DtLotesFacturaN.Rows
                    Dim Fila As New FilaAsignacion
                    Fila.Indice = Row("Indice")
                    Fila.Lote = Row("Lote")
                    Fila.Secuencia = Row("Secuencia")
                    Fila.Deposito = Row("Deposito")
                    Fila.Operacion = Row("Operacion")
                    Fila.Asignado = Row("Cantidad")
                    ListaDeLotes.Add(Fila)
                Next
            End If
        Else
            ListaDeLotes = New List(Of FilaAsignacion)
            If DtCabezaB.Rows.Count <> 0 Then
                For Each Row As DataRow In DtLotesB.Rows
                    Dim Fila As New FilaAsignacion
                    Fila.Indice = Row("Indice")
                    Fila.Lote = Row("Lote")
                    Fila.Secuencia = Row("Secuencia")
                    Fila.Deposito = Row("Deposito")
                    Fila.Operacion = Row("Operacion")
                    Fila.Devolucion = Row("Cantidad")
                    ListaDeLotes.Add(Fila)
                Next
            Else
                For Each Row As DataRow In DtLotesN.Rows
                    Dim Fila As New FilaAsignacion
                    Fila.Indice = Row("Indice")
                    Fila.Lote = Row("Lote")
                    Fila.Secuencia = Row("Secuencia")
                    Fila.Deposito = Row("Deposito")
                    Fila.Operacion = Row("Operacion")
                    Fila.Devolucion = Row("Cantidad")
                    ListaDeLotes.Add(Fila)
                Next
            End If
        End If

        If DtCabezaB.Rows.Count <> 0 Then
            MuestraCabeza(DtCabezaB)
        Else
            If DtCabezaN.Rows.Count <> 0 Then
                MuestraCabeza(DtCabezaN)
            End If
        End If

        If DtCabezaFacturaB.Rows.Count <> 0 Then
            ComboCliente.SelectedValue = DtCabezaFacturaB.Rows(0).Item("Cliente")
            ComboDeposito.SelectedValue = DtCabezaFacturaB.Rows(0).Item("Deposito")
            ComboMoneda.SelectedValue = DtCabezaFacturaB.Rows(0).Item("Moneda")
            MonedaFactura = DtCabezaFacturaB.Rows(0).Item("Moneda")
            If Not DtCabezaFacturaB.Rows(0).Item("EsExterior") Then
                DocumentoAsiento = 4 : DocumentoAsientoCosto = 6072
                EsExportacion = False
            Else
                DocumentoAsiento = 41 : DocumentoAsientoCosto = 6073
                EsExportacion = True
            End If
        Else
            ComboCliente.SelectedValue = DtCabezaFacturaN.Rows(0).Item("Cliente")
            ComboDeposito.SelectedValue = DtCabezaFacturaN.Rows(0).Item("Deposito")
            ComboMoneda.SelectedValue = DtCabezaFacturaN.Rows(0).Item("Moneda")
            MonedaFactura = DtCabezaFacturaN.Rows(0).Item("Moneda")
            If Not DtCabezaFacturaN.Rows(0).Item("EsExterior") Then
                DocumentoAsiento = 4 : DocumentoAsientoCosto = 6072
                EsExportacion = False
            Else
                DocumentoAsiento = 41 : DocumentoAsientoCosto = 6073
                EsExportacion = True
            End If
        End If

        'Muestra panel de Moneda.
        Dim TipoIva As Integer = TipoFactura
        If TipoIva = Exterior Then
            PanelMoneda.Visible = True
            Label1.Text = "Fecha AFIP."
            PictureAlmanaqueFecha.Visible = True
            Panel2.Visible = True
        Else : PanelMoneda.Visible = False
            Panel2.Visible = False
        End If
        If PNota = 0 Then
            If ComboMoneda.SelectedValue = 1 Then
                TextCambio.Text = 1
            Else : TextCambio.Text = ""
            End If
        Else
            If DtCabezaB.Rows.Count <> 0 Then
                TextCambio.Text = FormatNumber(DtCabezaB.Rows(0).Item("Cambio"), 3)
            Else
                TextCambio.Text = FormatNumber(DtCabezaN.Rows(0).Item("Cambio"), 3)
            End If
        End If

        'Muestra fecha.
        If TipoIva = Exterior Then
            If PNota = 0 Then
                DateTime1.Text = ""
            End If
        Else
            If PNota = 0 Then
                DateTime1.Text = Format(Date.Now, "dd/MM/yyyy")
            End If
        End If

        If PNota = 0 And DtCabezaFacturaB.Rows.Count <> 0 And Not EsExportacion Then
            If Not AgregalistaPercepciones() Then Me.Close() : Exit Function
        End If

        LabelFactura.Text = ""
        If DtCabezaFacturaB.Rows.Count <> 0 Then LabelFactura.Text = NumeroEditado(DtCabezaFacturaB.Rows(0).Item("Factura"))
        If DtCabezaFacturaN.Rows.Count <> 0 Then
            If LabelFactura.Text = "" Then
                LabelFactura.Text = NumeroEditado(DtCabezaFacturaN.Rows(0).Item("Factura"))
            Else
                LabelFactura.Text = LabelFactura.Text & " / " & NumeroEditado(DtCabezaFacturaN.Rows(0).Item("Factura"))
            End If
        End If

        Dim RowsBusqueda() As DataRow

        'Completa grid con Datos Facturas. Recordar que si no es permisototal DtDetalleFacturaN estara bacio y muestra importes en B.
        If DtDetalleFacturaB.Rows.Count <> 0 Then
            For Each Row As DataRow In DtGrid.Rows
                RowsBusqueda = DtDetalleFacturaB.Select("Indice = " & Row("Indice"))
                If RowsBusqueda.Length <> 0 Then
                    Row("Remito") = RowsBusqueda(0).Item("Remito")
                    Row("CantidadOriginal") = RowsBusqueda(0).Item("Cantidad")
                    Row("PrecioB") = RowsBusqueda(0).Item("Precio")
                    Row("IvaB") = RowsBusqueda(0).Item("Iva")
                    Row("PrecioListaB") = RowsBusqueda(0).Item("PrecioLista")
                    Row("Descuento") = RowsBusqueda(0).Item("Descuento")
                End If
            Next
        End If
        If DtDetalleFacturaN.Rows.Count <> 0 Then
            For Each Row As DataRow In DtGrid.Rows
                RowsBusqueda = DtDetalleFacturaN.Select("Indice = " & Row("Indice"))
                If RowsBusqueda.Length <> 0 Then
                    Row("Remito") = RowsBusqueda(0).Item("Remito")
                    Row("CantidadOriginal") = RowsBusqueda(0).Item("Cantidad")
                    Row("PrecioN") = RowsBusqueda(0).Item("Precio")
                    Row("IvaN") = RowsBusqueda(0).Item("Iva")
                    Row("PrecioListaN") = RowsBusqueda(0).Item("PrecioLista")
                    Row("Descuento") = RowsBusqueda(0).Item("Descuento")
                End If
            Next
        End If

        If PNota <> 0 Then
            If Not ArmaComprobantesAImputar() Then Return False
        End If

        'Muestra DtGedriCompro con Imputaciones.----
        If PAbierto Then
            MuestraImputaciones(DtDetalleCompB)
        Else
            MuestraImputaciones(DtDetalleCompN)
        End If
        '---------------------------------------------

        PAbierto = Not PAbierto   'se lo ponga como engaño para que quede igual en Pabierto = not Pabierto ButtonRelacionada_Click().
        ButtonRelacionada_Click(Nothing, Nothing)

        If DtCabezaB.Rows.Count <> 0 And DtCabezaN.Rows.Count <> 0 Then
            ButtonRelacionada.Visible = True
        Else : ButtonRelacionada.Visible = False
        End If
        If Not PermisoTotal Then ButtonRelacionada.Visible = False

        Grid.DataSource = Nothing
        Grid.DataSource = DtGrid

        If PermisoTotal Then
            PictureCandado.Visible = True
            If PAbierto Then
                PictureCandado.Image = ImageList1.Images.Item("Abierto")
            Else
                PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            End If
            If PNota = 0 Then
                PictureCandado.Image = Nothing
            End If
        Else
            PictureCandado.Visible = False
        End If

        If PNota = 0 Then
            PanelFecha.Enabled = True
            Panel2.Enabled = True
            PanelMoneda.Enabled = True
            TextComentario.ReadOnly = False
            Grid.Columns("Cantidad").ReadOnly = False
            ButtonDevolverTodos.Visible = True
            LabelPuntodeVenta.Visible = True
        Else
            PanelFecha.Enabled = False
            Panel2.Enabled = False
            PanelMoneda.Enabled = False
            TextComentario.ReadOnly = True
            Grid.Columns("Cantidad").ReadOnly = True
            ButtonDevolverTodos.Visible = False
            LabelPuntodeVenta.Visible = False
        End If

        CalculaTotal()

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.ColumnChanged, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanged)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Function LeerArchivosNota(ByVal NotaW As Double, ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtLotesW As DataTable, ByVal DtDetalleCompW As DataTable, ByVal ConexionStr As String) As Boolean

        If Not Tablas.Read("SELECT * FROM NotasCreditoCabeza WHERE NotaCredito= " & NotaW & ";", ConexionStr, DtCabezaW) Then Return False
        If NotaW <> 0 And DtCabezaW.Rows.Count = 0 Then
            MsgBox("Nota No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Not Tablas.Read("SELECT * FROM NotasCreditoDetalle WHERE NotaCredito = " & NotaW & ";", ConexionStr, DtDetalleW) Then Return False
        If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante = 4 AND Comprobante = " & NotaW & ";", ConexionStr, DtLotesW) Then Return False
        If Not Tablas.Read("SELECT * FROM RecibosDetalle WHERE TipoNota = 4 AND Nota = " & NotaW & ";", ConexionStr, DtDetalleCompW) Then Return False

        Return True

    End Function
    Private Function LeerArchivosFactura(ByVal DtCabezaWB As DataTable, ByVal DtDetalleWB As DataTable, ByVal DtLotesWB As DataTable, ByVal FacturaB As Double, ByVal DtCabezaWN As DataTable, ByVal DtDetalleWN As DataTable, ByVal DtLotesWN As DataTable, ByVal facturaN As Double) As Boolean

        'lee facturaB.
        If FacturaB <> 0 Then
            If Not ArmaArchivosFactura(DtCabezaWB, DtDetalleWB, DtLotesWB, FacturaB, Conexion) Then Return False
            If DtCabezaWB.Rows.Count = 0 Then
                MsgBox("Factura No se encuentra en la Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            TextFechafactura.Text = Format(DtCabezaWB.Rows(0).Item("Fecha"), "dd/MM/yyyy")
            DateTimeFacturaElectronica.Value = DtCabezaWB.Rows(0).Item("FechaElectronica")
            FacturaMixta = DtCabezaWB.Rows(0).Item("Rel")
        End If
        'lee facturaN.

        If facturaN <> 0 Then
            If Not ArmaArchivosFactura(DtCabezaWN, DtDetalleWN, DtLotesWN, facturaN, ConexionN) Then Return False
            If DtCabezaWN.Rows.Count = 0 Then
                MsgBox("Factura No se encuentra en la Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            TextFechafactura.Text = Format(DtCabezaWN.Rows(0).Item("Fecha"), "dd/MM/yyyy")
            DateTimeFacturaElectronica.Value = DtCabezaWN.Rows(0).Item("FechaElectronica")
            FacturaRelacionada = DtCabezaWN.Rows(0).Item("Relacionada")
            FacturaMixta = DtCabezaWN.Rows(0).Item("Rel")
        End If

        Return True

    End Function
    Private Function LeeComprobantesOperacion(ByVal dtB As DataTable, ByVal DtN As DataTable) As Boolean

        If DtCabezaFacturaB.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM ComprobantesOperacion WHERE Operacion = 1 AND TipoComprobante = 2 AND Comprobante = " & DtCabezaFacturaB.Rows(0).Item("Factura") & ";", ConexionN, dtB) Then Return False
        End If
        If DtCabezaFacturaN.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM ComprobantesOperacion WHERE Operacion = 2 AND TipoComprobante = 2 AND Comprobante = " & DtCabezaFacturaN.Rows(0).Item("Factura") & ";", ConexionN, DtN) Then Return False
        End If

        Return True

    End Function
    Private Function ArmaArchivoParaAlta(ByVal DtCabeza As DataTable, ByVal DtDetalle As DataTable, ByVal DtPercepciones As DataTable, ByVal Tipo As String, _
           ByVal DtCabezaFactura As DataTable, ByVal DtDetalleFactura As DataTable, ByVal DtLotesFactura As DataTable, ByVal DtComprobantesOperacion As DataTable, ByVal DtDetalleComp As DataTable) As Boolean

        Dim Row As DataRow
        Dim Importe As Decimal = 0
        Dim Neto As Decimal = 0
        Dim ImporteSinIva As Decimal = 0
        Dim RowsBusqueda() As DataRow
        Dim ListaDeIvas As New List(Of AutorizacionAFIP.ItemIva)
        Dim TotalIva As Decimal = 0

        'Arma Detalle Para Nota Credito. 
        For Each Row1 As DataRow In DtGrid.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                If Row1("Cantidad") <> 0 Then
                    Row = DtDetalle.NewRow()
                    Row("Indice") = Row1("Indice")
                    Row("Articulo") = Row1("Articulo")
                    Row("Cantidad") = Row1("Cantidad")
                    If Tipo = "B" Then
                        Row("ImporteSinIva") = CalculaNeto(Row1("Cantidad"), Row1("PrecioB"))
                        Neto = Neto + Row("ImporteSinIva")
                        Row("Importe") = Trunca(Row("ImporteSinIva") + CalculaIva(Row1("Cantidad"), Row1("PrecioB"), Row1("IvaB")))
                        Row("Iva") = Row1("IvaB")
                        Row("Precio") = Row1("PrecioB")
                        TotalIva = ArmaListaIvaPorValor(ListaDeIvas, Row("ImporteSinIva"), Row("Iva"), 0)
                    Else : Row("ImporteSinIva") = CalculaNeto(Row1("Cantidad"), Row1("PrecioN"))
                        Neto = Neto + Row("ImporteSinIva")
                        Row("Importe") = Trunca(Row("ImporteSinIva") + CalculaIva(Row1("Cantidad"), Row1("PrecioN"), Row1("IvaN")))
                        Row("Iva") = Row1("IvaN")
                        Row("Precio") = Row1("PrecioN")
                        TotalIva = ArmaListaIvaPorValor(ListaDeIvas, Row("ImporteSinIva"), Row("Iva"), 0)
                    End If
                    DtDetalle.Rows.Add(Row)
                    '   Importe = Trunca(Importe + Row("Importe"))
                    ImporteSinIva = Trunca(ImporteSinIva + Row("ImporteSinIva"))
                End If
            End If
        Next

        TotalPercepciones = 0
        If Tipo = "B" Then
            TotalPercepciones = CalculaTotalPercepciones()
            If TotalPercepciones <> 0 Then ArmaRecibosPercepciones(4, 0, ListaDePercepciones, DtPercepciones)
        End If

        Importe = Neto + TotalIva

        '' Para igualar importe factura con importe de la nota credito en devolucion total. Aveces da una diferencia de centavos y lo rechaza la AFIP....................
        If EsDevolucionTotal() Then
            Dim diferencia As Decimal = DtCabezaFactura.Rows(0).Item("Importe") + DtCabezaFactura.Rows(0).Item("Percepciones") - DtCabezaFactura.Rows(0).Item("ImporteDev")
            diferencia = diferencia - (Importe + TotalPercepciones)
            If diferencia < 0 Then Importe = Importe + diferencia
        End If
        '----------------------------------------------------------------------------------------------------------------------------------------------------------------

        'Arma Cabeza Para Nota Credito.
        Row = DtCabeza.NewRow
        Row.Item("Fecha") = CDate(DateTime1.Text)
        Row.Item("Factura") = DtCabezaFactura.Rows(0).Item("Factura")
        Row.Item("Importe") = Importe
        Row.Item("Percepciones") = TotalPercepciones
        Row.Item("ImporteSinIva") = ImporteSinIva
        Row.Item("Moneda") = MonedaFactura
        Row.Item("Cambio") = CDbl(TextCambio.Text)
        Row.Item("Impreso") = False
        Row.Item("Cae") = 0
        Row.Item("FechaCae") = 0
        If Tipo = "B" Then
            Row.Item("EsElectronica") = EsFacturaElectronica
        Else
            Row.Item("EsElectronica") = False
        End If
        Row.Item("Comentario") = TextComentario.Text.Trim
        If ListaDeLotes.Count <> 0 Then
            Row.Item("Estado") = 1
        Else : Row.Item("Estado") = 2
        End If
        If EsSecos Or EsServicios Then Row.Item("Estado") = 2
        Row.Item("FechaContable") = CDate(TextFechaContable.Text)
        Row("EsZ") = EsZ
        If EsFCE And Tipo = "B" Then
            Row("EsFCE") = EsFCE
        Else
            Row("EsFCE") = False
        End If
        DtCabeza.Rows.Add(Row)

        Dim ImporteGeneral As Decimal = Importe + TotalPercepciones

        'Actualiza cabeza de facturas con importes devueltos. ComprobantesOperacion si es exportacion.
        DtCabezaFactura.Rows(0).Item("ImporteDev") = Trunca(DtCabezaFactura.Rows(0).Item("ImporteDev") + ImporteGeneral)
        DtCabezaFactura.Rows(0).Item("Saldo") = CDec(DtCabezaFactura.Rows(0).Item("Saldo")) - ImporteGeneral
        If Abs(CDec(DtCabezaFactura.Rows(0).Item("Saldo"))) < 0.5 And Abs(CDec(DtCabezaFactura.Rows(0).Item("Saldo"))) > 0 Then DtCabezaFactura.Rows(0).Item("Saldo") = 0
        If DtCabezaFactura.Rows(0).Item("Saldo") < 0 Then
            MsgBox("Importe Devolución supera el Saldo de la Factura(Tiene Otras Imputaciones). Operación se CANCELA.", MsgBoxStyle.Critical)
            Return False
        End If
        If DtComprobantesOperacion.Rows.Count <> 0 Then
            DtComprobantesOperacion.Rows(0).Item("Saldo") = CDec(DtComprobantesOperacion.Rows(0).Item("Saldo")) - Importe
            If Abs(CDec(DtComprobantesOperacion.Rows(0).Item("Saldo"))) < 0.5 And Abs(CDec(DtComprobantesOperacion.Rows(0).Item("Saldo"))) > 0 Then DtComprobantesOperacion.Rows(0).Item("Saldo") = 0
            If DtComprobantesOperacion.Rows(0).Item("Saldo") < 0 Then
                MsgBox("Importe Devolución supera el Saldo de la Factura(Tiene Otras Imputaciones). Operación se CANCELA.", MsgBoxStyle.Critical)
                Return False
            End If
        End If

        'Actualiza Detalle De Facturas. 
        For Each Row In DtGrid.Rows
            If Row("Cantidad") <> 0 Then
                RowsBusqueda = DtDetalleFactura.Select("Indice = " & Row("Indice"))
                If RowsBusqueda.Length <> 0 Then
                    RowsBusqueda(0).Item("Devueltas") = CDec(Trunca(RowsBusqueda(0).Item("Devueltas")) + CDec(Row("Cantidad")))
                End If
            End If
        Next

        'Actualiza Asignacion de lotes de Facturas.
        Dim RowsBusqueda2() As DataRow
        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Devolucion <> 0 Then
                RowsBusqueda = DtLotesFactura.Select("Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & Fila.Deposito)
                'Si Cantidad = 0 entonces debo descontar en la parte B. 
                If RowsBusqueda(0).Item("Cantidad") <> 0 Then RowsBusqueda(0).Item("Cantidad") = CDec(RowsBusqueda(0).Item("Cantidad")) - Fila.Devolucion
                RowsBusqueda2 = DtGrid.Select("Indice = " & Fila.Indice)
                Dim NetoW As Decimal
                Dim ImporteW As Decimal
                If Tipo = "B" Then
                    NetoW = CalculaNeto(Fila.Devolucion, RowsBusqueda2(0).Item("PrecioB"))
                    If MonedaFactura <> 1 Then NetoW = Trunca(CDbl(TextCambio.Text) * NetoW)
                    ImporteW = CalculaNeto(Fila.Devolucion, RowsBusqueda2(0).Item("PrecioB")) + CalculaIva(Fila.Devolucion, RowsBusqueda2(0).Item("PrecioB"), RowsBusqueda2(0).Item("IvaB"))
                    If MonedaFactura <> 1 Then ImporteW = Trunca(CDbl(TextCambio.Text) * ImporteW)
                    '
                    RowsBusqueda(0).Item("ImporteSinIva") = CDec(RowsBusqueda(0).Item("ImporteSinIva")) - NetoW
                    RowsBusqueda(0).Item("Importe") = CDec(RowsBusqueda(0).Item("Importe")) - ImporteW
                Else
                    NetoW = CalculaNeto(Fila.Devolucion, RowsBusqueda2(0).Item("PrecioN"))
                    If MonedaFactura <> 1 Then NetoW = Trunca(CDbl(TextCambio.Text) * NetoW)
                    ImporteW = CalculaNeto(Fila.Devolucion, RowsBusqueda2(0).Item("PrecioN"))
                    If MonedaFactura <> 1 Then ImporteW = Trunca(CDbl(TextCambio.Text) * ImporteW)
                    '
                    RowsBusqueda(0).Item("ImporteSinIva") = CDec(RowsBusqueda(0).Item("ImporteSinIva")) - NetoW
                    RowsBusqueda(0).Item("Importe") = CDec(RowsBusqueda(0).Item("Importe")) - ImporteW
                End If
            End If
        Next

        'Graba en RecibosDetalle la factura a la que corresponde la nota de credito.
        Row = DtDetalleComp.NewRow
        Row("TipoNota") = 4
        Row("Nota") = 0
        Row("TipoComprobante") = 2
        Row("Comprobante") = DtCabezaFactura.Rows(0).Item("Factura")
        Row("Importe") = ImporteGeneral
        DtDetalleComp.Rows.Add(Row)

        Return True

    End Function
    Private Sub ArmaArchivoParaBaja(ByVal DtCabezaw As DataTable, ByVal DtDetallew As DataTable, ByVal Tipo As String, _
              ByVal DtCabezaFacturaw As DataTable, ByVal DtDetalleFacturaw As DataTable, ByVal DtLotesFacturaw As DataTable, ByVal DtComprobantesOperacion As DataTable)

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow

        'Arma Cabeza Para Nota Credito.
        DtCabezaw.Rows(0).Item("Estado") = 3

        'Actualiza cabeza de facturas con importes devueltos.
        Dim TotalW As Decimal
        Dim TotalGeneral As Decimal
        If DtCabezaFacturaw.Rows.Count <> 0 Then
            TotalW = DtCabezaw.Rows(0).Item("Importe")
            TotalGeneral = DtCabezaw.Rows(0).Item("Importe") + DtCabezaw.Rows(0).Item("Percepciones")
            DtCabezaFacturaw.Rows(0).Item("ImporteDev") = CDec(DtCabezaFacturaw.Rows(0).Item("ImporteDev")) - TotalGeneral
            'esto era para cuando la imputacion estaba fija en la factura. Ahora antes de la baja pide que saque las imputaciones.  
            ' DtCabezaFacturaw.Rows(0).Item("Saldo") = CDec(DtCabezaFacturaw.Rows(0).Item("Saldo")) + TotalGeneral
            If DtComprobantesOperacion.Rows.Count <> 0 Then DtComprobantesOperacion.Rows(0).Item("Saldo") = CDec(DtComprobantesOperacion.Rows(0).Item("Saldo")) + TotalW
        End If

        'Actualiza Detalle De Facturas. 
        For Each Row In DtGrid.Rows
            If Row("Cantidad") <> 0 Then
                RowsBusqueda = DtDetalleFacturaw.Select("Indice = " & Row("Indice"))
                If RowsBusqueda.Length <> 0 Then
                    RowsBusqueda(0).Item("Devueltas") = CDec(RowsBusqueda(0).Item("Devueltas")) - Row("Cantidad")
                End If
            End If
        Next

        Dim RowsBusqueda2() As DataRow
        'Actualiza Asignacion de lotes facturas.
        If DtLotesFacturaw.Rows.Count <> 0 Then
            ' Esta rutina restitute un lote de la devolucion que no esta en AsignacionLote de la factura, por que se dio el caso
            ' de tener una devolucion del lote, luego reasignar el lote a otro numero de lote en la factura y al querer anular la devolucion
            ' y no encontrar el lote anterior a la reasignacion da error.
            ' Ej.: en la factura asigne al lote 200/001 1000 Un. luego una devolucion de 500 un. Luego reasigne en la factura las 500 Un restante al lote
            ' 300/001, cuando quise anular la devolucion no encontro el lote 200/001 en la factura se produce un error en ejecucion. 
            For Each Fila As FilaAsignacion In ListaDeLotes
                RowsBusqueda = DtLotesFacturaw.Select("Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & Fila.Deposito)
                If RowsBusqueda.Length = 0 Then
                    Row = DtLotesFacturaw.NewRow()
                    Row("TipoComprobante") = 2
                    Row("Comprobante") = DtCabezaFacturaw.Rows(0).Item("Factura")
                    Row("Indice") = Fila.Indice
                    Row("Lote") = Fila.Lote
                    Row("Secuencia") = Fila.Secuencia
                    Row("Deposito") = Fila.Deposito
                    Row("Operacion") = Fila.Operacion
                    Row("Cantidad") = Fila.Asignado
                    Row("Rel") = DtCabezaFacturaw.Rows(0).Item("Rel")
                    Row("ImporteSinIva") = 0
                    Row("Importe") = 0
                    Row("Facturado") = False
                    Row("Liquidado") = False
                    DtLotesFacturaw.Rows.Add(Row)
                End If
            Next
            '
            For Each Fila As FilaAsignacion In ListaDeLotes
                Dim TotalWW As Decimal
                Dim ImporteIvaW As Decimal
                If Fila.Devolucion <> 0 Then
                    RowsBusqueda = DtLotesFacturaw.Select("Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & Fila.Deposito)
                    If Tipo = "B" Or (Tipo = "N" And Not RowsBusqueda(0).Item("Rel")) Then
                        RowsBusqueda(0).Item("Cantidad") = CDec(RowsBusqueda(0).Item("Cantidad")) + Fila.Devolucion
                    End If
                    RowsBusqueda2 = DtGrid.Select("Indice = " & Fila.Indice)
                    If Tipo = "B" Then
                        TotalWW = CalculaNeto(Fila.Devolucion, RowsBusqueda2(0).Item("PrecioB"))
                        If MonedaFactura <> 1 Then TotalWW = Trunca(CDbl(TextCambio.Text) * TotalWW)
                        ImporteIvaW = CalculaIva(Fila.Devolucion, RowsBusqueda2(0).Item("PrecioB"), RowsBusqueda2(0).Item("IvaB"))
                        If MonedaFactura <> 1 Then ImporteIvaW = Trunca(CDbl(TextCambio.Text) * ImporteIvaW)
                        RowsBusqueda(0).Item("ImporteSinIva") = CDec(RowsBusqueda(0).Item("ImporteSinIva")) + TotalWW
                        RowsBusqueda(0).Item("Importe") = CDec(RowsBusqueda(0).Item("Importe")) + TotalWW + ImporteIvaW
                    Else
                        TotalWW = CalculaNeto(Fila.Devolucion, RowsBusqueda2(0).Item("PrecioN"))
                        If MonedaFactura <> 1 Then TotalWW = Trunca(CDbl(TextCambio.Text) * TotalWW)
                        RowsBusqueda(0).Item("ImporteSinIva") = CDec(RowsBusqueda(0).Item("ImporteSinIva")) + TotalWW
                        RowsBusqueda(0).Item("Importe") = CDec(RowsBusqueda(0).Item("Importe")) + TotalWW
                    End If
                End If
            Next
        End If

    End Sub
    Private Sub ArmaGridConFactura(ByVal DtdetalleW As DataTable)

        For Each Row As DataRow In DtdetalleW.Rows
            If (Row("Cantidad") - Row("Devueltas")) <> 0 Then
                AgregaADtGrid(0, 0, 0, 0, 0, 0, Row("Indice"), Row("Articulo"), Row("KilosXUnidad"), Row("Cantidad"), Row("Devueltas"), 0)
            End If
        Next

    End Sub
    Private Sub ArmaGridConNota(ByVal DtdetalleW As DataTable)

        For Each Row As DataRow In DtdetalleW.Rows
            AgregaADtGrid(0, 0, 0, 0, 0, 0, Row("Indice"), Row("Articulo"), 0, 0, 0, Row("Cantidad"))
        Next

    End Sub
    Private Function ArmaArchivosFactura(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtLotesW As DataTable, ByVal Factura As Double, ByVal ConexionW As String) As Boolean

        If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Factura = " & Factura & ";", ConexionW, DtCabezaW) Then Return False
        If DtCabezaW.Rows.Count = 0 Then
            MsgBox("Factura No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Not Tablas.Read("SELECT * FROM FacturasDetalle WHERE Factura = " & Factura & ";", ConexionW, DtDetalleW) Then Return False
        If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante = 2 AND Comprobante = " & Factura & ";", ConexionW, DtLotesW) Then Return False

        Return True

    End Function
    Private Sub MuestraCabeza(ByVal Dt As DataTable)

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = Dt

        Dim Enlace As Binding

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        AddHandler Enlace.Format, AddressOf Formatfecha
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub Formatfecha(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "01/01/1800" Then
            Numero.Value = ""
        Else : Numero.Value = Format(Numero.Value, "dd/MM/yyyy")
        End If

    End Sub
    Private Sub ButtonDevolverTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDevolverTodos.Click

        For i As Integer = 0 To Grid.Rows.Count - 1
            Grid.Rows(i).Cells("Cantidad").Value = Grid.Rows(i).Cells("CantidadOriginal").Value - Grid.Rows(i).Cells("Devueltas").Value
        Next

    End Sub
    Private Function BuscaIndiceEnListaDeLotes(ByVal Indice As Integer) As Boolean

        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Indice = Indice Then Return True
        Next
        Return False

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        If EsServicios Then
            Articulo.DataSource = Tablas.Leer("SELECT * FROM ArticulosServicios WHERE Secos = 0;")
        Else
            If EsSecos Then
                Articulo.DataSource = Tablas.Leer("SELECT * FROM ArticulosServicios WHERE Secos = 1;")
            Else
                Articulo.DataSource = TodosLosArticulos()
            End If
        End If

        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

        Tipo.DataSource = DtTiposComprobantes(PAbierto)
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Codigo"

        TipoVisible.DataSource = DtTiposComprobantes(PAbierto)
        Row = TipoVisible.DataSource.NewRow()
        Row("Codigo") = 44
        Row("Nombre") = "Ticket Fiscal"
        TipoVisible.DataSource.Rows.Add(Row)
        TipoVisible.DisplayMember = "Nombre"
        TipoVisible.ValueMember = "Codigo"

        Moneda.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 27 order By Nombre;")
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 1
        Row("Nombre") = "PESOS"
        Moneda.DataSource.Rows.Add(Row)
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Moneda.DataSource.Rows.Add(Row)
        Moneda.DisplayMember = "Nombre"
        Moneda.ValueMember = "Clave"

    End Sub
    Private Function AltaNotaCredito(ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtAsientoCabezaN As DataTable, ByVal DtAsientoDetalleN As DataTable, ByVal DtAsientoCabezaCostoB As DataTable, ByVal DtAsientoDetalleCostoB As DataTable, ByVal DtAsientoCabezaCostoN As DataTable, ByVal DtAsientoDetalleCostoN As DataTable, ByVal DtComprobantesOperacionB As DataTable, ByVal DtComprobantesOperacionN As DataTable, ByVal DtPedidoDetalle As DataTable, ByVal DtDetalleCompB As DataTable, ByVal DtDetalleCompN As DataTable) As Double

        Dim NumeroB As Double
        Dim NumeroN As Double
        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaB.GetChanges) And Not EsZ Then
                    If Not ReGrabaUltimaNumeracionNCredito(DtCabezaB.Rows(0).Item("NotaCredito"), TipoFactura) Then Return -10
                End If

                If Not IsNothing(DtCabezaB.GetChanges) Then
                    NumeroB = GrabaNotaCredito(DtCabezaB, DtDetalleB, DtPercepciones, DtLotesB, DtCabezaFacturaB, DtDetalleFacturaB, DtLotesFacturaB, Conexion)
                    If NumeroB <= 0 Then Return NumeroB
                End If

                If Not IsNothing(DtCabezaN.GetChanges) Then
                    NumeroN = GrabaNotaCredito(DtCabezaN, DtDetalleN, New DataTable, DtLotesN, DtCabezaFacturaN, DtDetalleFacturaN, DtLotesFacturaN, ConexionN)
                    If NumeroN <= 0 Then Return NumeroN
                End If
                '
                ' graba Asiento B.
                If DtAsientoCabezaB.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaB.GetChanges, "AsientosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalleB.GetChanges, "AsientosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba Asiento N.
                If DtAsientoCabezaN.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalleN.GetChanges, "AsientosDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba Asiento costo B.
                If DtAsientoCabezaCostoB.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaCostoB.GetChanges, "AsientosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalleCostoB.GetChanges, "AsientosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba Asiento costo N.
                If DtAsientoCabezaCostoN.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaCostoN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalleCostoN.GetChanges, "AsientosDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If

                ' Actualiza Stock
                If Not ActualizaStockDevolucion(ListaDeLotes, "+") Then Return 0

                ' Actualiza ComprobantesOperacion si es exportacion.
                If Not IsNothing(DtComprobantesOperacionB.GetChanges) Then
                    Resul = GrabaTabla(DtComprobantesOperacionB.GetChanges, "ComprobantesOperacion", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtComprobantesOperacionN.GetChanges) Then
                    Resul = GrabaTabla(DtComprobantesOperacionN.GetChanges, "ComprobantesOperacion", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza Pedido.
                If Not IsNothing(DtPedidoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtPedidoDetalle.GetChanges, "PedidosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If

                ' Actualiza RecibosDetalle con Imputaciones.
                If Not IsNothing(DtDetalleCompB.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleCompB.GetChanges, "RecibosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtDetalleCompN.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleCompN.GetChanges, "RecibosDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        Finally
        End Try

    End Function
    Private Function ModificaNotaCredito(ByVal DtRecibosDetalleAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtSaldosInicialesAux As DataTable, ByVal ConexionStr As String,
                                         ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoCabezaN As DataTable, ByVal DtAsientoCabezaCostoB As DataTable, ByVal DtAsientoCabezaCostoN As DataTable, ByVal DtCabezaB As DataTable, ByVal DtCabezaN As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtRecibosDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtRecibosDetalleAux.GetChanges, "RecibosDetalle", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtFacturasCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtFacturasCabezaAux.GetChanges, "FacturasCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtNVLPCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNVLPCabezaAux.GetChanges, "NVLPCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtComprobantesCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtComprobantesCabezaAux.GetChanges, "RecibosCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtSaldosInicialesAux.GetChanges) Then
                    Resul = GrabaTabla(DtSaldosInicialesAux.GetChanges, "SaldosInicialesCabeza", ConexionStr)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtAsientoCabezaB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaB.GetChanges, "AsientosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtAsientoCabezaCostoB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaCostoB.GetChanges, "AsientosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtAsientoCabezaN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtAsientoCabezaCostoN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaCostoN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtCabezaB.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaB.GetChanges, "NotasCreditoCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtCabezaN.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaN.GetChanges, "NotasCreditoCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        Finally
        End Try

    End Function
    Private Function AnulaNotaCredito(ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoCabezaN As DataTable, ByVal DtAsientoCabezaCostoB As DataTable, ByVal DtAsientoCabezaCostoN As DataTable, ByVal DtComprobantesOperacionB As DataTable, ByVal DtComprobantesOperacionN As DataTable, ByVal DtPedidoDetalle As DataTable) As Boolean

        Dim NumeroB As Double
        Dim NumeroN As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaB.GetChanges) Then
                    NumeroB = GrabaNotaCredito(DtCabezaB, DtDetalleB, New DataTable, DtLotesB, DtCabezaFacturaB, DtDetalleFacturaB, DtLotesFacturaB, Conexion)
                    If NumeroB <= 0 Then Return False
                End If
                '
                If Not IsNothing(DtCabezaN.GetChanges) Then
                    NumeroN = GrabaNotaCredito(DtCabezaN, DtDetalleN, New DataTable, DtLotesN, DtCabezaFacturaN, DtDetalleFacturaN, DtLotesFacturaN, ConexionN)
                    If NumeroN <= 0 Then Return False
                End If
                '
                If Not IsNothing(DtAsientoCabezaB.GetChanges) Then
                    NumeroN = GrabaTabla(DtAsientoCabezaB.GetChanges, "AsientosCabeza", Conexion)
                    If NumeroN <= 0 Then Return False
                End If
                '
                If Not IsNothing(DtAsientoCabezaN.GetChanges) Then
                    NumeroN = GrabaTabla(DtAsientoCabezaN.GetChanges, "AsientosCabeza", ConexionN)
                    If NumeroN <= 0 Then Return False
                End If
                '
                If Not IsNothing(DtAsientoCabezaCostoB.GetChanges) Then
                    NumeroN = GrabaTabla(DtAsientoCabezaCostoB.GetChanges, "AsientosCabeza", Conexion)
                    If NumeroN <= 0 Then Return False
                End If
                '
                If Not IsNothing(DtAsientoCabezaCostoN.GetChanges) Then
                    NumeroN = GrabaTabla(DtAsientoCabezaCostoN.GetChanges, "AsientosCabeza", ConexionN)
                    If NumeroN <= 0 Then Return False
                End If
                '
                If Not ActualizaStockDevolucion(ListaDeLotes, "-") Then Return 0
                '
                ' Actualiza ComprobantesOperacion si es exportacion.
                If Not IsNothing(DtComprobantesOperacionB.GetChanges) Then
                    NumeroN = GrabaTabla(DtComprobantesOperacionB.GetChanges, "ComprobantesOperacion", ConexionN)
                    If NumeroN <= 0 Then Return False
                End If
                If Not IsNothing(DtComprobantesOperacionN.GetChanges) Then
                    NumeroN = GrabaTabla(DtComprobantesOperacionN.GetChanges, "ComprobantesOperacion", ConexionN)
                    If NumeroN <= 0 Then Return False
                End If

                'Actualiza Pedido.
                If Not IsNothing(DtPedidoDetalle.GetChanges) Then
                    NumeroN = GrabaTabla(DtPedidoDetalle.GetChanges, "PedidosDetalle", Conexion)
                    If NumeroN <= 0 Then Return False
                End If

                Scope.Complete()
                Return True
            End Using
        Catch ex As TransactionException
            Return False
        Finally
        End Try

    End Function
    Private Function BorraNotaCredito(ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtAsientoCabezaN As DataTable, ByVal DtAsientoDetalleN As DataTable, ByVal DtAsientoCabezaCostoB As DataTable, ByVal DtAsientoDetalleCostoB As DataTable, ByVal DtAsientoCabezaCostoN As DataTable, ByVal DtAsientoDetalleCostoN As DataTable, ByVal DtComprobantesOperacionB As DataTable, ByVal DtComprobantesOperacionN As DataTable, ByVal DtPedidoDetalle As DataTable, ByVal DtPuntosDeVenta As DataTable) As Boolean

        Dim NumeroB As Double
        Dim NumeroN As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaB.GetChanges) Then
                    NumeroB = GrabaNotaCredito(DtCabezaB, DtDetalleB, DtPercepciones, DtLotesB, DtCabezaFacturaB, DtDetalleFacturaB, DtLotesFacturaB, Conexion)
                    If NumeroB <= 0 Then Return False
                End If
                '
                If Not IsNothing(DtCabezaN.GetChanges) Then
                    NumeroN = GrabaNotaCredito(DtCabezaN, DtDetalleN, New DataTable, DtLotesN, DtCabezaFacturaN, DtDetalleFacturaN, DtLotesFacturaN, ConexionN)
                    If NumeroN <= 0 Then Return False
                End If
                '
                If Not IsNothing(DtAsientoCabezaB.GetChanges) Then
                    NumeroN = GrabaTabla(DtAsientoCabezaB.GetChanges, "AsientosCabeza", Conexion)
                    If NumeroN <= 0 Then Return False
                End If
                If Not IsNothing(DtAsientoDetalleB.GetChanges) Then
                    NumeroN = GrabaTabla(DtAsientoDetalleB.GetChanges, "AsientosDetalle", Conexion)
                    If NumeroN <= 0 Then Return False
                End If
                '
                If Not IsNothing(DtAsientoCabezaN.GetChanges) Then
                    NumeroN = GrabaTabla(DtAsientoCabezaN.GetChanges, "AsientosCabeza", ConexionN)
                    If NumeroN <= 0 Then Return False
                End If
                If Not IsNothing(DtAsientoDetalleN.GetChanges) Then
                    NumeroN = GrabaTabla(DtAsientoDetalleN.GetChanges, "AsientosDetalle", ConexionN)
                    If NumeroN <= 0 Then Return False
                End If
                '
                If Not IsNothing(DtAsientoCabezaCostoB.GetChanges) Then
                    NumeroN = GrabaTabla(DtAsientoCabezaCostoB.GetChanges, "AsientosCabeza", Conexion)
                    If NumeroN <= 0 Then Return False
                End If
                If Not IsNothing(DtAsientoDetalleCostoB.GetChanges) Then
                    NumeroN = GrabaTabla(DtAsientoDetalleCostoB.GetChanges, "AsientosDetalle", Conexion)
                    If NumeroN <= 0 Then Return False
                End If
                '
                If Not IsNothing(DtAsientoCabezaCostoN.GetChanges) Then
                    NumeroN = GrabaTabla(DtAsientoCabezaCostoN.GetChanges, "AsientosCabeza", ConexionN)
                    If NumeroN <= 0 Then Return False
                End If
                If Not IsNothing(DtAsientoDetalleCostoN.GetChanges) Then
                    NumeroN = GrabaTabla(DtAsientoDetalleCostoN.GetChanges, "AsientosDetalle", ConexionN)
                    If NumeroN <= 0 Then Return False
                End If
                '
                If Not ActualizaStockDevolucion(ListaDeLotes, "-") Then Return 0
                '
                ' Actualiza ComprobantesOperacion si es exportacion.
                If Not IsNothing(DtComprobantesOperacionB.GetChanges) Then
                    NumeroN = GrabaTabla(DtComprobantesOperacionB.GetChanges, "ComprobantesOperacion", ConexionN)
                    If NumeroN <= 0 Then Return False
                End If
                If Not IsNothing(DtComprobantesOperacionN.GetChanges) Then
                    NumeroN = GrabaTabla(DtComprobantesOperacionN.GetChanges, "ComprobantesOperacion", ConexionN)
                    If NumeroN <= 0 Then Return False
                End If

                'Actualiza Pedido.
                If Not IsNothing(DtPedidoDetalle.GetChanges) Then
                    NumeroN = GrabaTabla(DtPedidoDetalle.GetChanges, "PedidosDetalle", Conexion)
                    If NumeroN <= 0 Then Return False
                End If

                'Corre numeracion en Punto de venta.
                If Not IsNothing(DtPuntosDeVenta.GetChanges) Then
                    If GrabaTabla(DtPuntosDeVenta.GetChanges, "PuntosDeVenta", Conexion) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Return False
                    End If
                End If

                Scope.Complete()
                Return True
            End Using
        Catch ex As TransactionException
            Return False
        Finally
        End Try

    End Function
    Private Function GrabaNotaCredito(ByVal DtCabeza As DataTable, ByVal DtDetalle As DataTable, ByVal DtPercepciones As DataTable, ByVal DtLotes As DataTable, ByVal DtCabezaFactura As DataTable, ByVal DtDetalleFactura As DataTable, _
                            ByVal DtLotesFactura As DataTable, ByVal ConexionNota As String) As Double

        Dim Sql As String
        Dim Resul As Double = 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionNota)
                Miconexion.Open()
                'Graba Cabeza Nota.
                If Not IsNothing(DtCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtCabeza.GetChanges, "NotasCreditoCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtDetalle.GetChanges, "NotasCreditoDetalle", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtPercepciones.GetChanges) Then
                    Resul = GrabaTabla(DtPercepciones.GetChanges, "RecibosPercepciones", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtLotes.GetChanges) Then
                    Resul = GrabaTabla(DtLotes.GetChanges, "AsignacionLotes", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                'Actualiza Facturas.
                If Not IsNothing(DtCabezaFactura.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaFactura.GetChanges, "FacturasCabeza", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtDetalleFactura.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleFactura.GetChanges, "FacturasDetalle", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtLotesFactura.GetChanges) Then
                    Resul = GrabaTabla(DtLotesFactura.GetChanges, "AsignacionLotes", ConexionNota)
                    If Resul <= 0 Then Return Resul
                End If
                Return 1000
            End Using
        Catch ex As OleDb.OleDbException
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
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim AGranel As New DataColumn("AGranel")
        AGranel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(AGranel)

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Medida)

        Dim PrecioListaB As New DataColumn("PrecioListaB")
        PrecioListaB.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioListaB)

        Dim PrecioB As New DataColumn("PrecioB")
        PrecioB.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioB)

        Dim IvaB As New DataColumn("IvaB")
        IvaB.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(IvaB)

        Dim PrecioListaN As New DataColumn("PrecioListaN")
        PrecioListaN.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioListaN)

        Dim PrecioN As New DataColumn("PrecioN")
        PrecioN.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioN)

        Dim IvaN As New DataColumn("IvaN")
        IvaN.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(IvaN)

        Dim PrecioLista As New DataColumn("PrecioLista")
        PrecioLista.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioLista)

        Dim Precio As New DataColumn("Precio")
        Precio.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Precio)

        Dim Indice As New DataColumn("Indice")
        Indice.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Indice)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Remito As New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Remito)

        Dim KilosXUnidad As New DataColumn("KilosXUnidad")
        KilosXUnidad.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(KilosXUnidad)

        Dim Descuento As New DataColumn("Descuento")
        Descuento.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Descuento)

        Dim NetoLista As New DataColumn("NetoLista")
        NetoLista.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(NetoLista)

        Dim Neto As New DataColumn("Neto")
        Neto.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Neto)

        Dim ImporteIva As New DataColumn("ImporteIva")
        ImporteIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ImporteIva)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Importe)

        Dim CantidadOriginal As New DataColumn("CantidadOriginal")
        CantidadOriginal.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(CantidadOriginal)

        Dim Devueltas As New DataColumn("Devueltas")
        Devueltas.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Devueltas)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

    End Sub
    Private Sub CreaDtGridCompro()

        DtGridCompro = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Operacion)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Tipo)

        Dim TipoVisible As New DataColumn("TipoVisible")
        TipoVisible.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(TipoVisible)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Comprobante)

        Dim Comentario As New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGridCompro.Columns.Add(Comentario)

        Dim Recibo As New DataColumn("Recibo")
        Recibo.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Recibo)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGridCompro.Columns.Add(Fecha)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Importe)

        Dim Moneda As New DataColumn("Moneda")
        Moneda.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Moneda)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Saldo)

        Dim SaldoAnt As New DataColumn("SaldoAnt")
        SaldoAnt.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(SaldoAnt)

        Dim Asignado As New DataColumn("Asignado")
        Asignado.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Asignado)

    End Sub
    Private Sub AgregaADtGrid(ByVal PrecioListaB As Decimal, ByVal PrecioB As Double, ByVal IvaB As Double, ByVal PrecioListaN As Decimal, ByVal PrecioN As Double, ByVal IvaN As Double, _
            ByVal Indice As Integer, ByVal Articulo As Integer, ByVal KilosXUnidad As Decimal, ByVal CantidadOriginal As Decimal, ByVal Devueltas As Decimal, ByVal Cantidad As Decimal)

        Dim Row As DataRow

        Row = DtGrid.NewRow()
        Row("PrecioListaB") = PrecioListaB
        Row("PrecioB") = PrecioB
        Row("IvaB") = IvaB
        Row("PrecioListaN") = PrecioListaN
        Row("PrecioN") = PrecioN
        Row("IvaN") = IvaN
        Row("Indice") = Indice
        Row("Articulo") = Articulo
        Row("KilosXUnidad") = KilosXUnidad
        Row("Precio") = 0
        Row("NetoLista") = 0
        Row("Neto") = 0
        Row("ImporteIva") = 0
        Row("Importe") = 0
        Row("CantidadOriginal") = CantidadOriginal
        Row("Devueltas") = Devueltas
        Row("Cantidad") = Cantidad
        Row("AGranel") = False
        Row("Medida") = ""
        HallaAGranelYMedida(Articulo, Row("AGranel"), Row("Medida"))
        DtGrid.Rows.Add(Row)

    End Sub
    Private Function EstanDevueltos(ByVal Cantidad As Decimal, ByVal Indice As Integer) As Boolean

        Dim cantidadEnLotes As Decimal = 0
        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Indice = Indice Then cantidadEnLotes = cantidadEnLotes + Fila.Devolucion
        Next
        If cantidadEnLotes = Cantidad Then Return True

        Return False

    End Function
    Private Function EsDevolucionTotal() As Boolean

        For Each Row As DataRow In DtGrid.Rows
            If Row("CantidadOriginal") <> Row("Devueltas") + Row("Cantidad") Then Return False
        Next

        Return True

    End Function
    Private Function CalculaTotalPercepciones() As Decimal

        Dim Total As Decimal = 0

        If EsDevolucionTotal() Then
            For Each item As ItemIvaReten In ListaDePercepciones
                Total = Total + item.Importe
            Next
        End If

        Return Total

    End Function
    Private Function HallaPercepcionesRealizada(ByVal Percepcion As Integer, ByVal Factura As Double) As Decimal

        Dim Total As Decimal = 0
        Dim DtPercepcionesRealizadas As New DataTable
        Dim DtNotas As New DataTable

        'Halla Notas de Credito Sobre Factura.
        If Not Tablas.Read("SELECT NotaCredito FROM NotasCreditoCabeza Where Factura = " & Factura & " AND Estado <> 3;", Conexion, DtNotas) Then
            MsgBox("Error al leer Tabla: NotasCreditosCabeza. Proceso se CANCELA.") : Me.Close() : Exit Function
        End If
        'Halla Percepciones Realizadas en Notas de Credito.
        For Each Row As DataRow In DtNotas.Rows
            If Not Tablas.Read("SELECT Importe FROM RecibosPercepciones WHERE TipoComprobante = 4 AND Comprobante = " & Row("NotaCredito") & " AND Percepcion = " & Percepcion & ";", Conexion, DtPercepcionesRealizadas) Then
                MsgBox("Error al leer Tabla: RecibosPercepciones. Proceso se CANCELA.") : Me.Close() : Exit Function
            End If
        Next
        For Each Row As DataRow In DtPercepcionesRealizadas.Rows
            Total = Total + Row("Importe")
        Next

        DtNotas.Dispose()
        DtPercepcionesRealizadas.Dispose()

        Return Total

    End Function
    Private Sub CalculaTotal()

        Dim NetoLista As Decimal = 0
        Dim NetoB As Decimal = 0
        Dim NetoN As Decimal = 0
        Dim Neto As Decimal = 0
        Dim ImporteIva As Decimal = 0
        Dim Importe As Decimal = 0
        Dim ListaDeIvas As New List(Of AutorizacionAFIP.ItemIva)
        Dim TotalIva As Decimal = 0

        TotalNetoPerc = 0

        For Each Row As DataGridViewRow In Grid.Rows
            If IsNothing(Row.Cells("Articulo").Value) Then Exit For
            If PNota = 0 Then
                Row.Cells("Precio").Value = CDec(Row.Cells("PrecioB").Value) + CDec(Row.Cells("PrecioN").Value)
                Row.Cells("PrecioLista").Value = CDec(Row.Cells("PrecioListaB").Value) + CDec(Row.Cells("PrecioListaN").Value)
                Row.Cells("Neto").Value = CalculaNeto(Row.Cells("PrecioB").Value, Row.Cells("Cantidad").Value) + CalculaNeto(Row.Cells("PrecioN").Value, Row.Cells("Cantidad").Value)
                NetoB = CalculaNeto(Row.Cells("PrecioB").Value, Row.Cells("Cantidad").Value)
                NetoN = CalculaNeto(Row.Cells("PrecioN").Value, Row.Cells("Cantidad").Value)
                TotalNetoPerc = TotalNetoPerc + NetoB
                Row.Cells("NetoLista").Value = CalculaNeto(Row.Cells("PrecioListaB").Value, Row.Cells("Cantidad").Value) + CalculaNeto(Row.Cells("PrecioListaN").Value, Row.Cells("Cantidad").Value)
                Row.Cells("ImporteIva").Value = CalculaIva(Row.Cells("PrecioB").Value, Row.Cells("Cantidad").Value, Row.Cells("IvaB").Value) + CalculaIva(Row.Cells("PrecioN").Value, Row.Cells("Cantidad").Value, Row.Cells("IvaN").Value)
                TotalIva = ArmaListaIvaPorValor(ListaDeIvas, NetoB, Row.Cells("IvaB").Value, 0)
            End If
            If PNota <> 0 And PAbierto Then
                Row.Cells("Precio").Value = Row.Cells("PrecioB").Value
                Row.Cells("PrecioLista").Value = Row.Cells("PrecioListaB").Value
                Row.Cells("Neto").Value = CalculaNeto(Row.Cells("PrecioB").Value, Row.Cells("Cantidad").Value)
                NetoB = CalculaNeto(Row.Cells("PrecioB").Value, Row.Cells("Cantidad").Value)
                NetoN = CalculaNeto(Row.Cells("PrecioN").Value, Row.Cells("Cantidad").Value)
                TotalNetoPerc = TotalNetoPerc + NetoB
                Row.Cells("NetoLista").Value = CalculaNeto(Row.Cells("PrecioListaB").Value, Row.Cells("Cantidad").Value)
                Row.Cells("ImporteIva").Value = CalculaIva(Row.Cells("PrecioB").Value, Row.Cells("Cantidad").Value, Row.Cells("IvaB").Value)
                TotalIva = ArmaListaIvaPorValor(ListaDeIvas, NetoB, Row.Cells("IvaB").Value, 0)
            End If
            If PNota <> 0 And Not PAbierto Then
                Row.Cells("Precio").Value = Row.Cells("PrecioN").Value
                Row.Cells("PrecioLista").Value = Row.Cells("PrecioListaN").Value
                Row.Cells("Neto").Value = CalculaNeto(Row.Cells("PrecioN").Value, Row.Cells("Cantidad").Value)
                NetoB = CalculaNeto(Row.Cells("PrecioB").Value, Row.Cells("Cantidad").Value)
                NetoN = CalculaNeto(Row.Cells("PrecioN").Value, Row.Cells("Cantidad").Value)
                Row.Cells("NetoLista").Value = CalculaNeto(Row.Cells("PrecioListaN").Value, Row.Cells("Cantidad").Value)
                Row.Cells("ImporteIva").Value = CalculaIva(Row.Cells("PrecioN").Value, Row.Cells("Cantidad").Value, Row.Cells("IvaN").Value)
                TotalIva = ArmaListaIvaPorValor(ListaDeIvas, NetoB, Row.Cells("IvaN").Value, 0)
            End If
            Row.Cells("Importe").Value = CDec(Row.Cells("Neto").Value) + CDec(Row.Cells("ImporteIva").Value)
            Neto = Neto + CDec(Row.Cells("Neto").Value)
            ImporteIva = ImporteIva + CDec(Row.Cells("ImporteIva").Value)
            Importe = Importe + CDec(Row.Cells("Importe").Value)
            NetoLista = NetoLista + CDec(Row.Cells("NetoLista").Value)
        Next

        TotalPercepciones = 0
        If PNota = 0 Then
            TotalPercepciones = CalculaTotalPercepciones()
        Else
            If PAbierto Then
                TotalPercepciones = DtCabezaB.Rows(0).Item("percepciones")  'muestra el total de percepiones (de todas las juridicciones).
            End If
        End If

        Importe = Neto + TotalIva

        TextNetoLista.Text = FormatNumber(NetoLista, GDecimales)
        TextDescuento.Text = FormatNumber(NetoLista - Neto, GDecimales)
        TextTotalNeto.Text = FormatNumber(Neto, GDecimales)
        TextTotalIva.Text = FormatNumber(TotalIva, GDecimales)
        TextPercepcion.Text = FormatNumber(TotalPercepciones, GDecimales)
        TextImporte.Text = FormatNumber(Importe, GDecimales)
        TextTotalGeneral.Text = FormatNumber(Importe + TotalPercepciones, GDecimales)

    End Sub
    Private Sub ArmaAsignacionLotes(ByVal DtLotesWB As DataTable, ByVal ComprobanteWB As Double, ByVal DtLotesWN As DataTable, ByVal ComprobanteWN As Double)

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow
        Dim NetoW As Decimal
        Dim ImporteIvaW As Decimal

        If DtCabezaFacturaB.Rows.Count <> 0 Then
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Devolucion <> 0 Then
                    Row = DtLotesWB.NewRow()
                    Row("TipoComprobante") = 4
                    Row("Comprobante") = ComprobanteWB
                    Row("Indice") = Fila.Indice
                    Row("Lote") = Fila.Lote
                    Row("Secuencia") = Fila.Secuencia
                    Row("Deposito") = Fila.Deposito
                    Row("Operacion") = Fila.Operacion
                    Row("Cantidad") = Fila.Devolucion
                    Row("Facturado") = False
                    Row("Liquidado") = False
                    Row("Rel") = False
                    RowsBusqueda = DtGrid.Select("Indice = " & Fila.Indice)
                    NetoW = CalculaNeto(Fila.Devolucion, RowsBusqueda(0).Item("PrecioB"))
                    If MonedaFactura <> 1 Then Trunca(CDbl(TextCambio.Text) * NetoW)
                    ImporteIvaW = CalculaIva(Fila.Devolucion, RowsBusqueda(0).Item("PrecioB"), RowsBusqueda(0).Item("IvaB"))
                    If MonedaFactura <> 1 Then Trunca(CDbl(TextCambio.Text) * ImporteIvaW)
                    Row("ImporteSinIva") = NetoW
                    Row("Importe") = NetoW + ImporteIvaW
                    DtLotesWB.Rows.Add(Row)
                End If
            Next
        End If

        If DtCabezaFacturaN.Rows.Count <> 0 Then
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Devolucion <> 0 Then
                    Row = DtLotesWN.NewRow()
                    Row("TipoComprobante") = 4
                    Row("Comprobante") = ComprobanteWN
                    Row("Indice") = Fila.Indice
                    Row("Lote") = Fila.Lote
                    Row("Secuencia") = Fila.Secuencia
                    Row("Deposito") = Fila.Deposito
                    Row("Operacion") = Fila.Operacion
                    Row("Cantidad") = Fila.Devolucion
                    Row("Facturado") = False
                    Row("Liquidado") = False
                    Row("Rel") = False
                    RowsBusqueda = DtGrid.Select("Indice = " & Fila.Indice)
                    NetoW = CalculaNeto(Fila.Devolucion, RowsBusqueda(0).Item("PrecioN"))
                    If MonedaFactura <> 1 Then Trunca(CDbl(TextCambio.Text) * NetoW)
                    Row("ImporteSinIva") = NetoW
                    Row("Importe") = NetoW
                    DtLotesWN.Rows.Add(Row)
                End If
            Next
        End If

        If DtLotesWB.Rows.Count <> 0 And DtLotesWN.Rows.Count <> 0 Then
            For Each Row In DtLotesWB.Rows
                Row("Rel") = True
            Next
            For Each Row In DtLotesWN.Rows
                Row("Rel") = True
            Next
        End If

    End Sub
    Private Function ArmaComprobantesAImputar() As Boolean

        'Arma archivo Comprobantes a Imputar.
        DtFacturasCabeza = New DataTable
        DtNVLPCabeza = New DataTable
        DtComprobantesCabeza = New DataTable
        DtSaldosIniciales = New DataTable

        DtGridCompro.Clear()

        Dim Sql As String = ""

        Dim ConexionStr As String
        If PAbierto Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If Not ArmaConFacturas(ConexionStr) Then Return False
        If Not ArmaConSaldosIniciales(ConexionStr) Then Return False
        If Not ArmaConNVLP(ConexionStr) Then Return False
        If Not ArmaConNotas(64, ConexionStr) Then Return False 'Devolucións a Clientes. 
        If Not ArmaConNotas(5, ConexionStr) Then Return False 'N.Debito Financiera a Clientes.     
        If Not ArmaConNotas(13005, ConexionStr) Then Return False 'N.Debito Financiera a Clientes interna.     
        If Not ArmaConNotas(70, ConexionStr) Then Return False 'N.Credito Financiera del Clientes.  

        'Agrega a Dtgrid.
        'Agrega Factura.
        For Each Row As DataRow In DtFacturasCabeza.Rows
            Dim Row1 As DataRow = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 2
            Row1("TipoVisible") = 2
            If Row("Esz") Then Row1("TipoVisible") = 44
            Row1("Comprobante") = Row("Factura")
            Row1("Recibo") = Row("Factura")
            Row1("Fecha") = Row("Fecha")
            Row1("Comentario") = Row("Comentario")
            Row1("Importe") = Row("Importe") + Row("Percepciones")
            Row1("Moneda") = Row("Moneda")
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next
        'Arma Saldos Iniciales.
        For Each Row As DataRow In DtSaldosIniciales.Rows
            Dim Row1 As DataRow = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 900
            Row1("TipoVisible") = 900
            Row1("Comprobante") = Row("Clave")
            Row1("Recibo") = Row("Clave")
            Row1("Fecha") = Row("Fecha")
            Row1("Comentario") = ""
            Row1("Importe") = Row("Importe")
            If Row1("Importe") < 0 Then Row1("Importe") = -Row1("Importe")
            Row1("Moneda") = Row("Moneda")
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next
        'Arma con NVLP.
        For Each Row As DataRow In DtNVLPCabeza.Rows
            Dim Row1 As DataRow = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 800
            Row1("TipoVisible") = 800
            Row1("Comprobante") = Row("Liquidacion")
            Row1("Recibo") = Row("ReciboOficial")
            Row1("Comentario") = Row("Comentario")
            Row1("Fecha") = Row("FechaLiquidacion")
            Row1("Importe") = Row("Importe")
            Row1("Moneda") = 1
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next
        'Arma con Recibos.
        For Each Row As DataRow In DtComprobantesCabeza.Rows
            Dim Row1 As DataRow = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = Row("TipoNota")
            Row1("TipoVisible") = Row("TipoNota")
            Row1("Comprobante") = Row("Nota")
            If Row("TipoNota") = 50 Or Row("TipoNota") = 70 Or Row("TipoNota") = 500 Or Row("TipoNota") = 700 Then
                Row1("Recibo") = Row("ReciboOficial")
                Row1("Fecha") = Row("FechaReciboOficial")
            Else
                Row1("Recibo") = Row("Nota")
                Row1("Fecha") = Row("Fecha")
            End If
            Row1("Comentario") = Row("Comentario")
            Row1("Importe") = Row("Importe")
            Row1("Moneda") = Row("Moneda")
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next

        Dim DtNotaDetalle As DataTable
        If PAbierto Then
            DtNotaDetalle = DtDetalleCompB.Copy
        Else
            DtNotaDetalle = DtDetalleCompN.Copy
        End If

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtNotaDetalle.Rows
            RowsBusqueda = DtGridCompro.Select("Tipo = " & Row("TipoComprobante") & " AND Comprobante = " & Row("Comprobante"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Asignado") = Row("Importe")
            End If
        Next

        'Borra los documentos con saldo 0 y no tienen asignacion. 
        DtGridCompro.AcceptChanges()
        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Saldo") = 0 And Row("Asignado") = 0 Then Row.Delete()
        Next

        DtGridCompro.AcceptChanges()

        Return True

    End Function
    Private Function ArmaConFacturas(ByVal ConexionStr As String) As Boolean

        Dim Sql As String = ""
        '------------------------------------------------------------------------------------------------------------
        'ClienteOperacion = 0 para que no aparezcan las generadas en el modulo de exportacion.-----------------------
        '------------------------------------------------------------------------------------------------------------

        Sql = "SELECT * FROM FacturasCabeza WHERE ClienteOperacion = 0 AND EsZ = 0 AND Estado <> 3 AND FacturasCabeza.Cliente = " & ComboCliente.SelectedValue & " ORDER BY Factura,Fecha;"

        If Not Tablas.Read(Sql, ConexionStr, DtFacturasCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConNVLP(ByVal ConexionStr As String) As Boolean

        Dim Sql As String = ""

        Sql = "SELECT * FROM NVLPCabeza WHERE Estado = 1 AND Cliente = " & ComboCliente.SelectedValue & " ORDER BY Liquidacion,Fecha;"
        If Not Tablas.Read(Sql, ConexionStr, DtNVLPCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConNotas(ByVal TipoNota As Integer, ByVal ConexionStr As String) As Boolean

        Dim Sql As String = ""

        Sql = "SELECT * FROM RecibosCabeza WHERE Estado = 1 AND TipoNota = " & TipoNota & " AND Emisor = " & ComboCliente.SelectedValue & " ORDER BY Nota,Fecha;"
        If Not Tablas.Read(Sql, ConexionStr, DtComprobantesCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConSaldosIniciales(ByVal ConexionStr As String) As Boolean

        Dim Sql As String = ""

        Sql = "SELECT * FROM SaldosInicialesCabeza WHERE Tipo = 3 AND Importe > 0 AND Emisor = " & ComboCliente.SelectedValue & ";"
        If Not Tablas.Read(Sql, ConexionStr, DtSaldosIniciales) Then Return False

        Return True

    End Function
    Private Function LlenaDatosCliente(ByVal Cliente As Integer) As Boolean

        Dim Dta As New DataTable

        Dim Sql As String = "SELECT * FROM Clientes WHERE Clave = " & Cliente & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, el Cliente ya no existe o error en la Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Dta.Dispose()
            Return False
        End If

        Calle = Dta.Rows(0).Item("Calle")
        Numero = Dta.Rows(0).Item("Numero")
        Localidad = Dta.Rows(0).Item("Localidad")
        DocTipo = Dta.Rows(0).Item("DocumentoTipo")
        DocNro = Dta.Rows(0).Item("DocumentoNumero")
        Provincia = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
        TextCuit.Text = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")
        TextTipoFactura.Text = LetraTipoIva(Dta.Rows(0).Item("TipoIva"))

        Dta.Dispose()

        Return True

    End Function
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtNotaLotes As DataTable, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable, ByVal Operacion As Integer) As Boolean

        Dim ListaLotesParaAsientoAux As New List(Of ItemLotesParaAsientos)
        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow

        If ListaDeLotes.Count <> 0 Then
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Devolucion <> 0 Then
                    Dim Tipo As Integer
                    Dim Centro As Integer
                    Dim Fila2 As New ItemLotesParaAsientos
                    If Fila.Operacion = 1 Then
                        HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, Conexion, Tipo, Centro)
                    Else : HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, ConexionN, Tipo, Centro)
                    End If
                    If Centro <= 0 Then
                        MsgBox("Error en Tipo Operacion en Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                        Return False
                    End If
                    RowsBusqueda = DtGrid.Select("Indice = " & Fila.Indice)
                    Fila2.TipoOperacion = Tipo
                    Fila2.Centro = Centro
                    If Operacion = 1 Then
                        Fila2.MontoNeto = CalculaNeto(Fila.Devolucion, RowsBusqueda(0).Item("PrecioB"))
                    Else
                        Fila2.MontoNeto = CalculaNeto(Fila.Devolucion, RowsBusqueda(0).Item("PrecioN"))
                    End If
                    If MonedaFactura <> 1 Then Fila2.MontoNeto = Trunca(CDbl(TextCambio.Text) * Fila2.MontoNeto)
                    If Tipo = 1 Then Fila2.Clave = 301 'consignacion
                    If Tipo = 2 Then Fila2.Clave = 300 'reventa
                    If Tipo = 3 Then Fila2.Clave = 303 'reventa MG
                    If Tipo = 4 Then Fila2.Clave = 302 'costeo
                    ListaLotesParaAsiento.Add(Fila2)
                End If
            Next
        End If

        Dim Item As New ItemListaConceptosAsientos

        For Each Row As DataRow In DtGrid.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row("IvaB") <> 0 And Operacion = 1 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = HallaClaveIva(Row("IvaB"))
                    If Item.Clave <= 0 Then
                        MsgBox("Error al leer Tabla de IVA. Operación se CANCELA.")
                        Return False
                    End If
                    Item.Importe = CalculaIva(Row("Cantidad"), Row("PrecioB"), Row("IvaB"))
                    If MonedaFactura <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
                    Item.TipoIva = 6
                    ListaIVA.Add(Item)
                End If
            End If
        Next
        '
        Dim MontoNeto As Decimal
        Dim MontoFinal As Decimal
        For Each Row As DataRow In DtGrid.Rows
            If Operacion = 1 Then
                MontoNeto = MontoNeto + CalculaNeto(Row("Cantidad"), Row("PrecioB"))
                MontoFinal = MontoFinal + CalculaNeto(Row("Cantidad"), Row("PrecioB")) + CalculaIva(Row("Cantidad"), Row("PrecioB"), Row("IvaB"))
            Else
                MontoNeto = MontoNeto + CalculaNeto(Row("Cantidad"), Row("PrecioN"))
                MontoFinal = MontoFinal + CalculaNeto(Row("Cantidad"), Row("PrecioN")) + CalculaIva(Row("Cantidad"), Row("PrecioN"), Row("IvaN"))
            End If
        Next
        '
        'Arma lista de Percepciones.
        Dim TotalPercepciones As Decimal = 0
        If Operacion = 1 Then
            For Each Row As DataRow In DtPercepciones.Rows
                If Row("Importe") <> 0 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("Percepcion")
                    Item.Importe = Row("Importe")
                    TotalPercepciones = TotalPercepciones + Row("Importe")
                    Item.TipoIva = 11   'Debito fiscal.
                    ListaRetenciones.Add(Item)
                End If
            Next
        End If
        '
        If ListaDeLotes.Count = 0 Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = 202
            Item.Importe = MontoNeto
            If MonedaFactura <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
            ListaConceptos.Add(Item)
        End If
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = MontoFinal + TotalPercepciones
        If MonedaFactura <> 1 Then Item.Importe = Trunca(CDbl(TextCambio.Text) * Item.Importe)
        ListaConceptos.Add(Item)

        If Funcion = "A" Then
            If Not Asiento(DocumentoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, CDate(TextFechaContable.Text), 0) Then Return False
        End If

        Return True

    End Function
    Private Function ArmaArchivosAsientoCosto(ByVal Funcion As String, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable, ByVal Porcentaje As Double) As Boolean

        If EsSecos Or EsServicios Then Return True

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow

        Dim Precio As Decimal = 0
        Dim Tipo As Integer
        Dim Centro As Integer
        Dim ImporteTotal As Decimal
        Dim Item As New ItemListaConceptosAsientos

        If ListaDeLotes.Count <> 0 Then
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Devolucion <> 0 Then
                    Tipo = 0
                    Centro = 0
                    Dim Fila2 As New ItemLotesParaAsientos
                    '
                    RowsBusqueda = DtGrid.Select("Indice = " & Fila.Indice)
                    '
                    If Fila.Operacion = 1 Then
                        HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, Conexion, Tipo, Centro)
                    Else : HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, ConexionN, Tipo, Centro)
                    End If
                    If Centro <= 0 Then
                        MsgBox("Error en Tipo Operacion en Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                        Return False
                    End If
                    If Tipo = 4 Then
                        Dim Negocio As Integer = HallaProveedorLote(Fila.Operacion, Fila.Lote, Fila.Secuencia)
                        If Negocio <= 0 Then
                            MsgBox("Error al Leer Lotes " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                            Return False
                        End If
                        Dim Costeo = HallaCosteoLote(Fila.Operacion, Fila.Lote)
                        If Costeo = -1 Then Return False
                        If Not HallaPrecioYCentroCosteo(Negocio, Costeo, Centro, Precio) Then Return False
                    Else
                        Precio = Refe
                    End If
                    Dim Kilos As Decimal
                    Dim Iva As Decimal
                    HallaKilosIva(RowsBusqueda(0).Item("Articulo"), Kilos, Iva)
                    '
                    Fila2.Centro = Centro
                    Fila2.MontoNeto = Trunca(Precio * Fila.Devolucion * Kilos * Porcentaje / 100)
                    ImporteTotal = ImporteTotal + Fila2.MontoNeto
                    If Tipo = 1 Then Fila2.Clave = 301 'consignacion
                    If Tipo = 2 Then Fila2.Clave = 300 'reventa
                    If Tipo = 3 Then Fila2.Clave = 303 'reventa MG
                    If Tipo = 4 Then Fila2.Clave = 302 'costeo
                    ListaLotesParaAsiento.Add(Fila2)
                End If
            Next
        Else
            For Each Row As DataRow In DtGrid.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Dim Kilos As Decimal
                    Dim Iva As Decimal
                    HallaKilosIva(Row("Articulo"), Kilos, Iva)
                    ImporteTotal = ImporteTotal + Trunca(Refe * Row("Cantidad") * Kilos * Porcentaje / 100)
                End If
            Next
            Item = New ItemListaConceptosAsientos
            Item.Clave = 202
            Item.Importe = ImporteTotal
            ListaConceptos.Add(Item)
        End If

        If Funcion = "A" Then
            If Not Asiento(DocumentoAsientoCosto, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, CDate(TextFechaContable.Text), 0) Then Return False
        End If

        Return True

    End Function
    Private Function ArmaArchivosAsientoCostoExportacion(ByVal Funcion As String, ByVal DtFacturaDetalle As DataTable, ByVal DtFacturaDetalleComplemento As DataTable, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow

        Dim Precio As Decimal = 0
        Dim Tipo As Integer
        Dim Centro As Integer
        Dim ImporteTotal As Decimal
        Dim Item As New ItemListaConceptosAsientos

        If ListaDeLotes.Count <> 0 Then
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Devolucion <> 0 Then
                    Tipo = 0
                    Centro = 0
                    Dim Fila2 As New ItemLotesParaAsientos
                    '
                    RowsBusqueda = DtGrid.Select("Indice = " & Fila.Indice)
                    '
                    Dim Porcentaje As Decimal = 0
                    Dim Total As Decimal = 0
                    Dim Importe As Decimal = 0
                    RowsBusqueda = DtFacturaDetalle.Select("Indice = " & Fila.Indice)
                    Total = RowsBusqueda(0).Item("TotalArticulo")
                    Importe = Total
                    If DtFacturaDetalleComplemento.Rows.Count <> 0 Then
                        RowsBusqueda = DtFacturaDetalleComplemento.Select("Indice = " & Fila.Indice)
                        Total = Total + CDec(RowsBusqueda(0).Item("TotalArticulo"))
                    End If
                    Porcentaje = Importe * 100 / Total
                    '
                    If Fila.Operacion = 1 Then
                        HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, Conexion, Tipo, Centro)
                    Else : HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, ConexionN, Tipo, Centro)
                    End If
                    If Centro <= 0 Then
                        MsgBox("Error en Tipo Operacion en Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                        Return False
                    End If
                    If Tipo = 4 Then
                        Dim Negocio As Integer = HallaProveedorLote(Fila.Operacion, Fila.Lote, Fila.Secuencia)
                        If Negocio <= 0 Then
                            MsgBox("Error al Leer Lotes " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                            Return False
                        End If
                        Dim Costeo = HallaCosteoLote(Fila.Operacion, Fila.Lote)
                        If Costeo = -1 Then Return False
                        If Not HallaPrecioYCentroCosteo(Negocio, Costeo, Centro, Precio) Then Return False
                    Else
                        Precio = Refe
                    End If
                    Dim Kilos As Decimal
                    Dim Iva As Decimal
                    HallaKilosIva(RowsBusqueda(0).Item("Articulo"), Kilos, Iva)
                    '
                    Fila2.Centro = Centro
                    Fila2.MontoNeto = Trunca(Precio * Fila.Devolucion * Kilos * Porcentaje / 100)
                    ImporteTotal = ImporteTotal + Fila2.MontoNeto
                    If Tipo = 1 Then Fila2.Clave = 301 'consignacion
                    If Tipo = 2 Then Fila2.Clave = 300 'reventa
                    If Tipo = 3 Then Fila2.Clave = 303 'reventa MG
                    If Tipo = 4 Then Fila2.Clave = 302 'costeo
                    ListaLotesParaAsiento.Add(Fila2)
                End If
            Next
        Else
            For Each Row As DataRow In DtGrid.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Dim Kilos As Decimal
                    Dim Iva As Decimal
                    HallaKilosIva(Row("Articulo"), Kilos, Iva)
                    '
                    Dim Porcentaje As Decimal = 0
                    Dim Total As Decimal = 0
                    Dim Importe As Decimal = 0
                    RowsBusqueda = DtFacturaDetalle.Select("Indice = " & Row("Indice"))
                    Total = RowsBusqueda(0).Item("TotalArticulo")
                    Importe = Total
                    If DtFacturaDetalleComplemento.Rows.Count <> 0 Then
                        RowsBusqueda = DtFacturaDetalleComplemento.Select("Indice = " & Row("Indice"))
                        Total = Total + CDec(RowsBusqueda(0).Item("TotalArticulo"))
                    End If
                    Porcentaje = Importe * 100 / Total
                    ImporteTotal = ImporteTotal + Trunca(Refe * Row("Cantidad") * Kilos * Porcentaje / 100)
                End If
            Next
            Item = New ItemListaConceptosAsientos
            Item.Clave = 202
            Item.Importe = ImporteTotal
            ListaConceptos.Add(Item)
        End If

        If Funcion = "A" Then
            If Not Asiento(DocumentoAsientoCosto, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, CDate(DateTime1.Text), 0) Then Return False
        End If

        Return True

    End Function
    Private Function TieneArticulosDesHabilitados(ByVal DtDetalle As DataTable, ByVal ConexionStr As String, ByVal EsFruta As Boolean) As Boolean

        If EsFruta Then
            Return HallaArticuloDeshabilitado(DtDetalle)
        Else
            Return HallaArticuloServiciosDeshabilitado(DtDetalle)
        End If

    End Function
    Private Sub CalculaNetoSinDescuentoMasIva(ByRef Neto As Decimal, ByRef Descuento As Decimal)

        Neto = 0
        Descuento = 0

        Dim NetoTotal As Decimal = 0

        For I As Integer = 0 To Grid.Rows.Count - 1
            Dim Iva As Decimal
            If PAbierto Then
                Iva = Grid.Rows(I).Cells("IvaB").Value
            Else
                Iva = Grid.Rows(I).Cells("IvaN").Value
            End If
            Neto = Neto + CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioLista").Value) + CalculaIva(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioLista").Value, Iva)             'Total
            NetoTotal = NetoTotal + Grid.Rows(I).Cells("Importe").Value
        Next

        Descuento = Neto - NetoTotal

    End Sub
    Private Function HallaNombreArticulo(ByVal Articulo As Integer) As String

        Dim EsFruta As Boolean = False

        If DtCabezaFacturaB.Rows.Count <> 0 Then
            If DtCabezaFacturaB.Rows(0).Item("EsServicios") Or DtCabezaFacturaB.Rows(0).Item("EsSecos") Then
                EsFruta = False
            Else
                EsFruta = True
            End If
        Else
            If DtCabezaFacturaN.Rows(0).Item("EsServicios") Or DtCabezaFacturaN.Rows(0).Item("EsSecos") Then
                EsFruta = False
            Else
                EsFruta = True
            End If
        End If

        If Not EsFruta Then
            Return NombreArticuloServicios(Articulo)
        Else
            Return NombreArticulo(Articulo)
        End If

    End Function
    Private Function EsNotaCreditoAnteriorOk(ByVal Nota As Decimal) As Boolean

        Dim Numero As Decimal = Strings.Right(Nota, 8)
        If Numero - 1 = 0 Then Return True

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Cae FROM RecibosCabeza WHERE (TipoNota = 7 or TipoNota = 8) AND Nota = " & Nota - 1 & ";", Conexion, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            If Dt.Rows(0).Item("Cae") = 0 Then
                Dt.Dispose() : Return False
            Else
                Dt.Dispose() : Return True
            End If
        End If

        Dt = New DataTable

        If Not Tablas.Read("SELECT Cae FROM NotasCreditoCabeza WHERE NotaCredito = " & Nota - 1 & ";", Conexion, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            If Dt.Rows(0).Item("Cae") = 0 Then
                Dt.Dispose() : Return False
            Else
                Dt.Dispose() : Return True
            End If
        Else
            Return True
        End If

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Dim Patron As String = TipoFactura & Format(GPuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(NotaCredito) FROM NotasCreditoCabeza WHERE CAST(CAST(NotasCreditoCabeza.NotaCredito AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(TipoFactura & Format(GPuntoDeVenta, "0000") & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function UltimoNumeroInternoNotaCredito(ByVal TipoFacturaW As Integer, ByVal ConexionStr As String) As Double

        Dim Patron As String = TipoFacturaW & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Interno) FROM NotasCreditoCabeza WHERE CAST(CAST(NotasCreditoCabeza.Interno AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(TipoFacturaW & Format(1, "000000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function UltimaNumeracionAsiento(ByVal ConexionStr) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Asiento) FROM AsientosCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Dim Sql As String = "SELECT MAX(N.Fecha) FROM NotasCreditoCabeza AS N INNER JOIN FacturasCabeza AS C ON C.Factura = N.Factura WHERE C.EsExterior = 0;"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM NotasCreditoCabeza;", Miconexion)
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
    Private Function UltimaFechaExportacion(ByVal ConexionStr) As Date

        Dim Sql As String = "SELECT MAX(N.Fecha) FROM NotasCreditoCabeza AS N INNER JOIN FacturasCabeza AS C ON C.Factura = N.Factura WHERE C.EsExterior = 1;"

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
    Private Function HallaFacturaRelacionada(ByVal Factura As Double) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Factura FROM FacturasCabeza WHERE Relacionada = " & Factura & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaNVLPLote(ByVal Deposito As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Operacion As Integer) As Integer

        Dim ConexionLote As String
        Dim Liquidado As Boolean = False

        If Operacion = 1 Then
            ConexionLote = Conexion
        Else : ConexionLote = ConexionN
        End If

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Liquidado FROM AsignacionLotes WHERE TipoComprobante = 1 AND Deposito = " & Deposito & " AND Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", ConexionLote, Dt) Then Return -1
        If Dt.Rows.Count <> 0 Then Liquidado = Dt.Rows(0).Item("Liquidado")
        Dt.Dispose()

        If Liquidado Then
            Return 1
        Else
            Return 0
        End If

    End Function
    Private Function FormatoSinRedondeo3Decimales(ByVal Numero As Double) As Double

        Dim PosicionDecimal As Integer = InStr(1, Numero.ToString, ",")
        Return CDbl(Mid(Numero.ToString, 1, PosicionDecimal + 3))

    End Function
    Public Function GrabaImpreso(ByVal DtCabezaW As DataTable, ByVal ConexionAux As String) As Boolean

        Dim Sql As String = "UPDATE NotasCreditoCabeza Set Impreso = 1 WHERE NotaCredito = " & DtCabezaW.Rows(0).Item("NotaCredito") & ";"

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
    Private Sub ArmaListaImportesIva(ByRef Lista As List(Of ItemIva))

        Dim Esta As Boolean
        Dim Iva As Double = 0

        For Each Row As DataGridViewRow In Grid.Rows
            If IsNothing(Row.Cells("Articulo").Value) Then Exit For
            Esta = False
            Iva = Row.Cells("IvaB").Value
            If Iva <> 0 Then
                For Each Fila As ItemIva In Lista
                    If Fila.Iva = Iva Then
                        Fila.Importe = Fila.Importe + Row.Cells("ImporteIva").Value
                        Esta = True
                    End If
                Next
                If Not Esta Then
                    Dim Fila As New ItemIva
                    Fila.Iva = Iva
                    Fila.Importe = Row.Cells("ImporteIva").Value
                    Lista.Add(Fila)
                End If
            End If
        Next

    End Sub
    Private Function HallaNotaRelacionada(ByVal NotaCredito As Double) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT NotaCredito FROM NotasCreditoCabeza WHERE Relacionada = " & NotaCredito & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function AgregalistaPercepciones() As Boolean
        'Arma lista con precepciones de la factura y resta las realizadas en otras nots de credio.

        ListaDePercepciones = New List(Of ItemIvaReten)

        ListaDePercepciones = New List(Of ItemIvaReten)
        Dim DtPercepcionesFactura As New DataTable
        If Not Tablas.Read("SELECT * FROM RecibosPercepciones WHERE TipoComprobante = 2 AND Comprobante = " & DtCabezaFacturaB.Rows(0).Item("Factura") & ";", Conexion, DtPercepcionesFactura) Then Return False
        For Each Row As DataRow In DtPercepcionesFactura.Rows
            Dim Fila As New ItemIvaReten
            Fila.Clave = Row("Percepcion")
            Fila.CodigoAfipElectronico = Row("CodigoAfip")
            Fila.Alicuota = Row("Alicuota")
            Fila.Formula = Row("Formula")
            'Resto las ya realizadas en otras Notas de credito por devolucion.
            Dim Total As Decimal = HallaPercepcionesRealizada(Fila.Clave, DtCabezaFacturaB.Rows(0).Item("Factura"))
            Fila.Importe = Row("Importe") - Total
            Fila.Base = Row("Base")
            ListaDePercepciones.Add(Fila)
        Next

        Return True

    End Function
    Private Sub MuestraImputaciones(ByVal Dt As DataTable)

        GridCompro.Rows.Clear()

        For Each Row As DataRow In Dt.Rows
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = DtGridCompro.Select("Tipo = " & Row("TipoComprobante") & " AND Comprobante = " & Row("Comprobante"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Asignado") = Row("Importe")
                Dim Row2 As DataRow = RowsBusqueda(0)
                GridCompro.Rows.Add(Row2("Operacion"), "", Row2("Tipo"), Row2("TipoVisible"), Row2("Comprobante"), Row2("Recibo"), Row2("Fecha"), Row2("Moneda"), Row2("Importe"), Row2("Saldo"), Row2("Asignado"))
            End If
        Next

    End Sub
    Private Function HayModifcacionInputacion() As Boolean

        If PNota = 0 Then Return False

        Dim RowsBusqueda() As DataRow

        Dim DtRecibosDetalle As DataTable
        If PAbierto Then
            DtRecibosDetalle = DtDetalleCompB.Copy
        Else : DtRecibosDetalle = DtDetalleCompN.Copy
        End If

        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Asignado") <> 0 Then
                RowsBusqueda = DtRecibosDetalle.Select("TipoComprobante = " & Row("Tipo") & " AND Comprobante = " & Row("Comprobante"))
                If RowsBusqueda.Length = 0 Then
                    Return True
                Else
                    If RowsBusqueda(0).Item("Importe") <> Row("Asignado") Then Return True
                End If
            End If
        Next

        For Each Row As DataRow In DtRecibosDetalle.Rows
            RowsBusqueda = DtGridCompro.Select("Tipo = " & Row("TipoComprobante") & " AND Comprobante = " & Row("Comprobante"))
            If RowsBusqueda.Length = 0 Then
                Return True
            Else
                If RowsBusqueda(0).Item("Asignado") <> Row("Importe") Then Return True
            End If
        Next

    End Function
    Private Function ModificaAsientosCabeza(ByRef DtAsientoCabezaB As DataTable, ByRef DtAsientoCabezaN As DataTable, ByRef DtAsientoCabezaCostoB As DataTable, ByRef DtAsientoCabezaCostoN As DataTable) As Boolean

        If DtCabezaB.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & DocumentoAsiento & " AND Documento = " & DtCabezaB.Rows(0).Item("NotaCredito") & ";", Conexion, DtAsientoCabezaB) Then Return False
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                DtAsientoCabezaB.Rows(0).Item("intFecha") = Format(CDate(TextFechaContable.Text), "yyyyMMdd")
            End If
        End If
        If DtCabezaN.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & DocumentoAsiento & " AND Documento = " & DtCabezaN.Rows(0).Item("NotaCredito") & ";", ConexionN, DtAsientoCabezaN) Then Return False
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                DtAsientoCabezaN.Rows(0).Item("intFecha") = Format(CDate(TextFechaContable.Text), "yyyyMMdd")
            End If
        End If
        If DtCabezaB.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & DocumentoAsientoCosto & " AND Documento = " & DtCabezaB.Rows(0).Item("NotaCredito") & ";", Conexion, DtAsientoCabezaCostoB) Then Return False
            If DtAsientoCabezaCostoB.Rows.Count <> 0 Then
                DtAsientoCabezaCostoB.Rows(0).Item("intFecha") = Format(CDate(TextFechaContable.Text), "yyyyMMdd")
            End If
        End If
        If DtCabezaN.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & DocumentoAsientoCosto & " AND Documento = " & DtCabezaN.Rows(0).Item("NotaCredito") & ";", ConexionN, DtAsientoCabezaCostoN) Then Return False
            If DtAsientoCabezaCostoN.Rows.Count <> 0 Then
                DtAsientoCabezaCostoN.Rows(0).Item("intFecha") = Format(CDate(TextFechaContable.Text), "yyyyMMdd")
            End If
        End If

        Return True

    End Function
    Private Function Valida() As Boolean

        If Not EsDevolucionTotal() And CheckBoxPorAnulacion.Checked Then
            MsgBox("Anulación Por Factura Rechazada por el Cliente solo Valida para Devolución Total.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            CheckBoxPorAnulacion.Focus()
            Return False
        End If

        If DateTime1.Text = "" Then
            MsgBox("Fecha Informat Fecha.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If

        If CierreContableCerrado(CDate(DateTime1.Text).Month, CDate(DateTime1.Text).Year) Then
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If DiferenciaDias(CDate(DateTime1.Text), UltimaFechaW) > 0 And PNota = 0 Then
            MsgBox("Fecha Menor a la Ultima Fecha Grabada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If
        If DiferenciaDias(CDate(DateTime1.Text), Date.Now) < 0 And PNota = 0 Then
            MsgBox("Fecha Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If
        If Not EsExportacion Then
            If DiferenciaDias(CDate(DateTime1.Text), CDate(TextFechafactura.Text)) > 0 And PNota = 0 Then
                MsgBox("Fecha debe ser mayor o igual a Fecha Factura.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                DateTime1.Focus()
                Return False
            End If
        End If
        If EsExportacion Then
            If DiferenciaDias(CDate(DateTime1.Text), DtCabezaFacturaB.Rows(0).Item("FechaElectronica")) > 0 And PNota = 0 Then
                MsgBox("Fecha debe ser mayor o igual a Fecha Factura.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                DateTime1.Focus()
                Return False
            End If
        End If
        If EsFacturaElectronica Or EsZ Then
            If TextFechaContable.Text = "" Then
                MsgBox("Falta Informar Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
            If Not ConsisteFecha(TextFechaContable.Text) Then
                MsgBox("Fecha Contable Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
            If DiferenciaDias(TextFechaContable.Text, Date.Now) < 0 Then
                MsgBox("Fecha Contable Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
            If DiferenciaDias(TextFechaContable.Text, UltimafechaContableW) > 0 Then
                MsgBox("Fecha Contable Menor a la Ultima Nota Informada.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
            If CierreContableCerrado(CDate(TextFechaContable.Text).Month, CDate(TextFechaContable.Text).Year) Then
                MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaContable.Focus()
                Return False
            End If
            If DiferenciaDias(TextFechaContable.Text, CDate(DateTime1.Text)) <> 0 And EsFacturaElectronica Then
                If CDate(DateTime1.Text).Day > 10 Then
                    MsgBox("Fecha Contable menor a fecha actual solo puede ser informada del 1 al 10 del mes actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    TextFechaContable.Focus()
                    Return False
                End If
            End If
        End If

        If TextCambio.Text = "" Then
            MsgBox("Falta Informar Cambio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If
        If ComboMoneda.SelectedValue <> 1 And CDbl(TextCambio.Text) = 0 Then
            MsgBox("Falta Informar Cambio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If
        If ComboMoneda.SelectedValue = 1 And CDbl(TextCambio.Text) <> 1 Then
            MsgBox("Error, Cambio de Moneda Local debe ser 1.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If

        Dim Cantidad As Decimal = 0
        For i As Integer = 0 To Grid.RowCount - 1
            Cantidad = Cantidad + Grid.Rows(i).Cells("Cantidad").Value
            If Grid.Rows(i).Cells("Cantidad").Value <> 0 Then
                If TieneDecimales(Grid.Rows(i).Cells("Cantidad").Value) And Not Grid.Rows(i).Cells("AGranel").Value Then
                    MsgBox("Cantidad no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Cantidad")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
        Next
        If Cantidad = 0 Then
            MsgBox("Debe Informar Cantidad a Devolver. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If ListaDeLotes.Count <> 0 Then
            For i As Integer = 0 To Grid.RowCount - 1
                If Grid.Rows(i).Cells("Cantidad").Value <> 0 Then
                    Dim CantidadEnLotes As Decimal = 0
                    For Each Fila As FilaAsignacion In ListaDeLotes
                        If Fila.Indice = Grid.Rows(i).Cells("Indice").Value Then
                            CantidadEnLotes = CantidadEnLotes + Fila.Devolucion
                        End If
                    Next
                    If Grid.Rows(i).Cells("Cantidad").Value <> CantidadEnLotes Then
                        MsgBox("Debe Informar Lotes a Devolver en la linea " & i + 1 & "  Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Grid.CurrentCell = Grid.Rows(i).Cells("Cantidad")
                        Grid.BeginEdit(True)
                        Return False
                    End If
                End If
            Next
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        'para manejo del autocoplete de articulos.
        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Grid.CurrentRow.Cells("LoteYSecuencia").Value = "" Then Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" And ComboEstado.SelectedValue = 2 Then
            SeleccionaLotesDevolucion.PArticulo = Grid.CurrentRow.Cells("Articulo").Value
            SeleccionaLotesDevolucion.PDeposito = ComboDeposito.SelectedValue
            If Grid.CurrentRow.Cells("Cantidad").Value = 0 Then
                MsgBox("Falta Informar Cantidad a Devolver.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3, "Error")
                Exit Sub
            End If
            SeleccionaLotesDevolucion.PCantidad = Grid.CurrentRow.Cells("Cantidad").Value
            SeleccionaLotesDevolucion.PEsAumento = False
            SeleccionaLotesDevolucion.PIndice = Grid.CurrentRow.Cells("Indice").Value
            SeleccionaLotesDevolucion.PLista = ListaDeLotes
            SeleccionaLotesDevolucion.ShowDialog()
            ListaDeLotes = SeleccionaLotesDevolucion.PLista
            SeleccionaLotesDevolucion.Dispose()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" And ComboEstado.SelectedValue <> 2 Then
            MuestraLotesAsignados.PEsDevolucion = True
            MuestraLotesAsignados.PArticulo = Grid.CurrentRow.Cells("Articulo").Value
            MuestraLotesAsignados.PIndice = CInt(Grid.CurrentRow.Cells("Indice").Value)
            MuestraLotesAsignados.PLista = ListaDeLotes
            MuestraLotesAsignados.ShowDialog()
            MuestraLotesAsignados.Dispose()
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    If BuscaIndiceEnListaDeLotes(Grid.Rows(e.RowIndex).Cells("Indice").Value) Then
                        If EstanDevueltos(Grid.Rows(e.RowIndex).Cells("Cantidad").Value, Grid.Rows(e.RowIndex).Cells("Indice").Value) Then
                            Grid.Rows(e.RowIndex).Cells("LoteYSecuencia").Value = "Devueltos" : Grid.Rows(e.RowIndex).Cells("LoteYSecuencia").Style.ForeColor = Color.Green
                        Else : Grid.Rows(e.RowIndex).Cells("LoteYSecuencia").Value = "Falta Dev." : Grid.Rows(e.RowIndex).Cells("LoteYSecuencia").Style.ForeColor = Color.Red
                        End If
                    End If
                Else : Grid.Rows(e.RowIndex).Cells("LoteYSecuencia").Value = ""
                    e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "RemitoP" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then e.Value = NumeroEditado(e.Value)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Devueltas" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "PrecioLista" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales + 1, True, True, True)
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Precio" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales + 1, True, True, True)
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales, True, True, True)
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "ImporteIva" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales, True, True, True)
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Neto" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales, True, True, True)
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloDecimales_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub SoloDecimales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Cantidad") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If e.ProposedValue > (e.Row("CantidadOriginal") - e.Row("Devueltas")) Then
                MsgBox("Cantidad de Devolución supera la Permitida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                e.ProposedValue = e.Row("Cantidad")
            End If
            If e.ProposedValue <> e.Row("Cantidad") Then
                For Each Fila As FilaAsignacion In ListaDeLotes
                    If Fila.Indice = e.Row("Indice") Then Fila.Devolucion = 0
                Next
            End If
        End If

    End Sub
    Private Sub DtGrid_ColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Cantidad") Then
            CalculaTotal()
        End If

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRIDCOMPRO DE COMPROBANTES.      ----------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridCompro_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles GridCompro.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If GridCompro.Columns(e.ColumnIndex).Name = "Recibo" Then
            e.Value = NumeroEditado(e.Value)
            If PermisoTotal Then
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "FechaCompro" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Asignado" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "ImporteCompro" Or GridCompro.Columns(e.ColumnIndex).Name = "Saldo" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

    End Sub
    Private Sub GridCompro_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles GridCompro.DataError
        Exit Sub
    End Sub


End Class